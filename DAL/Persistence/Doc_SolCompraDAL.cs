using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Persistence
{
    public class Doc_SolCompraDAL : Conexao
    {
        protected string strSQL = "";
		public void SalvarSolicitacao(Doc_SolCompra p, List<ProdutoDocumento> listaProdutos, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento)
		{
			try
			{
				List<Habil_Log> listaLog = new List<Habil_Log>();
				Habil_LogDAL Rn_Log = new Habil_LogDAL();
                DBTabelaDAL RnTabela = new DBTabelaDAL(); 


                DataTable tbA, tbB;

				if (p.CodigoDocumento == 0) //Registro Novo
					Inserir(p, listaProdutos, eventoDocumento, ListaAnexoDocumento);
				else
				{
					tbA = ObterSolicitacaoCompra(p.CodigoDocumento);

					if (eventoDocumento != null)
						Atualizar(p, listaProdutos, eventoDocumento, ListaAnexoDocumento);
					else
						Atualizar(p, listaProdutos, null, ListaAnexoDocumento);

					tbB = ObterSolicitacaoCompra(p.CodigoDocumento);
					listaLog = Rn_Log.ComparaDataTables(tbA, tbB, Convert.ToDouble(p.CodigoDocumento), p.Cpl_Usuario, p.Cpl_Maquina, 17, "DOCUMENTO");
					foreach (Habil_Log item in listaLog)
						Rn_Log.Inserir(item);
				}

                if (p.CodigoSituacao == 201)
                {// Análise
                    LiberacaoDocumento p1;
                    LiberacaoDocumentoDAL Rn_LibDoc = new LiberacaoDocumentoDAL();

                    p1 = Rn_LibDoc.PesquisarLiberacaoDocumento(p.CodigoDocumento, 1);
                    if (p1 == null)
                    {
                        p1 = new LiberacaoDocumento();
                        p1.CodigoDocumento = p.CodigoDocumento;
                        p1.CodigoBloqueio = 1;
                        p1.CodigoMaquina = p.Cpl_Maquina;
                        p1.CodigoUsuario = p.Cpl_Usuario;
                        p1.DataLancamento = RnTabela.ObterDataHoraServidor();
                        Rn_LibDoc.Inserir(p1);

                    }
                }

            }
			catch (SqlException ex)
			{
				if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
				{
					throw new Exception("Erro ao Incluir Salvar Solicitação de Compras: " + ex.Message.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Salvar Solicitação de Compras: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public void Inserir(Doc_SolCompra p, List<ProdutoDocumento> listaItemSolicitacao, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento)
        {
            try
            {
                AbrirConexao();
                strSQL = "insert into DOCUMENTO (CD_TIPO_DOCUMENTO," +
                                                "CD_EMPRESA," +
												"CD_SITUACAO," +
												"NR_DOCUMENTO," +
												"DT_HR_EMISSAO," +
												"DT_VENCIMENTO," +
												"CD_GER_SEQ_DOC, " +
												"VL_TOTAL_GERAL, " +
                                                "CD_USU_RESPONSAVEL, " +
												"DT_HR_ENTRADA, OB_DOCUMENTO) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11) SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                decimal CodigoGerado = 0;
                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                if(p.Cpl_NomeTabela != null)
                    CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", 12);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
				Cmd.Parameters.AddWithValue("@v4", p.NumeroDocumento);
				Cmd.Parameters.AddWithValue("@v5", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v6", p.DataValidade);
                Cmd.Parameters.AddWithValue("@v7", CodigoGerado);
                Cmd.Parameters.AddWithValue("@v8", p.ValorTotal);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoUsuario);
				Cmd.Parameters.AddWithValue("@v10", p.DataHoraEmissao);
				Cmd.Parameters.AddWithValue("@v11", p.DescricaoDocumento);

				p.CodigoDocumento = Convert.ToDecimal(Cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao Incluir Orcamento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Solicitação de Compra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

				ExcluirPessoaDocumento(p.CodigoDocumento);//Pessoa do Documento Fornecedor
				InserirPessoaDocumento(p.CodigoDocumento, p.CodigoFornecedor, 16);//Pessoa do Documento Fornecedor

				if (eventoDocumento != null)
				{
					EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
					eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
				}
				AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
				AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);
				InserirProduto(p.CodigoDocumento, listaItemSolicitacao);
            }
        }
        public void Atualizar(Doc_SolCompra p, List<ProdutoDocumento> listaItemSolicitacao, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_TIPO_DOCUMENTO = @v1," +
                                                " CD_EMPRESA = @v2," +
												" CD_SITUACAO = @v3," +
                                                " NR_DOCUMENTO = @v4," +
												" DT_HR_EMISSAO = @v5," +
												" DT_VENCIMENTO = @v6," +
												" VL_TOTAL_GERAL = @v7," +
                                                " OB_DOCUMENTO = @v8, " +
                                                " TX_MOTIVO_BAIXA = @v9 " +
												" Where [CD_DOCUMENTO] = @CODIGO ";

                Cmd = new SqlCommand(strSQL, Con);


                Cmd.Parameters.AddWithValue("@CODIGO", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v1", 12);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v5", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v6", p.DataValidade);
				Cmd.Parameters.AddWithValue("@v7", p.ValorTotal);
				Cmd.Parameters.AddWithValue("@v8", p.DescricaoDocumento);
                Cmd.Parameters.AddWithValue("@v9", p.MotivoCancelamento);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Solicitação de Compra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

				DataTable tbProdDocA = new DataTable();
				DataTable tbProdDocB = new DataTable();
				List<Habil_Log> listaLog = new List<Habil_Log>();
				Habil_LogDAL Rn_Log = new Habil_LogDAL();

				tbProdDocA = ObterSolicitacaoCompraPessoa(p.CodigoDocumento);
				ExcluirPessoaDocumento(p.CodigoDocumento);//Pessoa do Documento Fornecedor
				InserirPessoaDocumento(p.CodigoDocumento, p.CodigoFornecedor, 16);//Pessoa do Documento Fornecedor
				tbProdDocB = ObterSolicitacaoCompraPessoa(p.CodigoDocumento);

				listaLog = Rn_Log.ComparaDataTablesRelacionalPessoa_do_Documento(tbProdDocA, tbProdDocB, Convert.ToDouble(p.CodigoDocumento), p.Cpl_Usuario, p.Cpl_Maquina, 21, 22, 23, "PESSOA_DO_DOCUMENTO", "CD_DOCUMENTO", "TP_PESSOA");
				foreach (Habil_Log item in listaLog)
					Rn_Log.Inserir(item);

                if (eventoDocumento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
                }

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
				AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);

				tbProdDocA = new DataTable();

				tbProdDocA = ObterSolicitacaoCompraProduto(p.CodigoDocumento);
				InserirProduto(p.CodigoDocumento, listaItemSolicitacao);
				tbProdDocB = new DataTable();
				tbProdDocB = ObterSolicitacaoCompraProduto(p.CodigoDocumento);

				listaLog = new List<Habil_Log>();
				listaLog = Rn_Log.ComparaDataTablesRelacionalProduto_do_Documento(tbProdDocA, tbProdDocB, Convert.ToDouble ( p.CodigoDocumento), p.Cpl_Usuario, p.Cpl_Maquina, 18, 19,20, "PRODUTO_DO_DOCUMENTO", "CD_DOCUMENTO", "CD_PROD_DOCUMENTO");
				foreach (Habil_Log item in listaLog)
					Rn_Log.Inserir(item);
			}
		}
		public void InserirProduto(decimal CodigoDocumento, List<ProdutoDocumento> listaItemOrcamento)
		{
			try
			{
				ProdutoDocumentoDAL ItemDAL = new ProdutoDocumentoDAL();
				ItemDAL.ExcluirTodos(CodigoDocumento);
				AbrirConexao();
				foreach (ProdutoDocumento p in listaItemOrcamento)
				{
					strSQL = "insert into PRODUTO_DO_DOCUMENTO(CD_DOCUMENTO," +
																"CD_PROD_DOCUMENTO," +
																"DS_UNIDADE, " +
																"QT_SOLICITADA, " +
																"CD_PRODUTO," +
                                                                "DS_PRODUTO,CD_SITUACAO, OB_PROD_DOCUMENTO) values (@v1,@v2,@v3,@v4,@v5,@v6,135,@v7)";

					Cmd = new SqlCommand(strSQL, Con);

					Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
					Cmd.Parameters.AddWithValue("@v2", p.CodigoItem);
					Cmd.Parameters.AddWithValue("@v3", p.Unidade);
					Cmd.Parameters.AddWithValue("@v4", p.Quantidade);
					Cmd.Parameters.AddWithValue("@v5", p.CodigoProduto);
					Cmd.Parameters.AddWithValue("@v6", p.Cpl_DscProduto);
                    Cmd.Parameters.AddWithValue("@v7", p.Cpl_DescRoteiro);
                    Cmd.ExecuteNonQuery();
					
				}
			}
			catch (SqlException ex)
			{
				if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
				{
					switch (ex.Errors[0].Number)
					{
						case 2601: // Primary key violation
							throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
						case 2627: // Primary key violation
							throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
						default:
							throw new Exception("Erro ao Incluir produtos do documento: " + ex.Message.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao gravar produtos do documento: " + ex.Message.ToString());
			}
			finally
			{

				FecharConexao();
			}
		}
		public void AtualizarSituacao(decimal CodigoDocumento, int CodigoSituacaoNova, int CodigoSituacaoAnterior, int CodigoUsuario, int CodigoMaquina)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_SITUACAO = @v1 WHERE CD_DOCUMENTO = @v2 ";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoSituacaoNova);
                Cmd.Parameters.AddWithValue("@v2", CodigoDocumento);

                if(CodigoSituacaoAnterior != 0 && CodigoSituacaoAnterior != CodigoSituacaoNova)
                {
                    Doc_SolCompra doc = new Doc_SolCompra();
                    Doc_SolCompraDAL docDAL = new Doc_SolCompraDAL();
                    doc = docDAL.PesquisarSolCompra(CodigoDocumento);
                    doc.Cpl_Maquina = CodigoMaquina;
                    doc.CodigoUsuario = CodigoUsuario;
                    if(doc != null)
                        EventoDocumento(doc,CodigoSituacaoNova);
                }


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar situacao Solicitação de Compra: " + ex.Message.ToString());
            }

        }
        public void Excluir(decimal Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update DOCUMENTO set CD_SITUACAO = 139 where CD_DOCUMENTO = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            throw new InvalidOperationException("Exclusão não Permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao excluir Solicitação de Compra: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Solicitação de Compra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
		public List<Doc_SolCompra> ListarSolCompraCompleto(List<DBTabelaCampos> ListaFiltros)
		{
			try
			{
				AbrirConexao();
				string strValor = "";
				string strSQL = "Select * from [VW_DOC_SOL_COMPRA] ";


				strValor = MontaFiltroIntervalo(ListaFiltros);
				strSQL = strSQL + strValor;

				strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";
				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Doc_SolCompra> lista = new List<Doc_SolCompra>();

				while (Dr.Read())
				{
					Doc_SolCompra p = new Doc_SolCompra();
					p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
					p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
					p.DataValidade = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
					p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
					p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
					p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
					p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
					p.CodigoUsuario = Convert.ToInt32(Dr["CD_USU_RESPONSAVEL"]);
					p.Cpl_NomeUsuario = Dr["NM_COMPLETO"].ToString();

					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar TODos Solicitações de Compra: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public Doc_SolCompra PesquisarSolCompra(decimal Codigo)
        {
            try
            {

                AbrirConexao();
                strSQL = "Select * from [VW_DOC_SOL_COMPRA] Where CD_DOCUMENTO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Doc_SolCompra p = null;

                if (Dr.Read())
                {
                    p = new Doc_SolCompra();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
					p.CodigoFornecedor = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.MotivoCancelamento = Dr["TX_MOTIVO_BAIXA"].ToString();

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Doc_SolCompra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void EventoDocumento(Doc_SolCompra doc, int CodigoSituacao)
        {
            List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
            EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
            ListaEvento = eventoDAL.ObterEventos(doc.CodigoDocumento);
            DBTabelaDAL RnTab = new DBTabelaDAL();

            EventoDocumento eventodoc = new EventoDocumento();
            eventodoc.CodigoDocumento = Convert.ToDecimal(doc.CodigoDocumento);
            //eventodoc.CodigoMaquina = doc.Cpl_Maquina;
            eventodoc.CodigoUsuario = doc.CodigoUsuario;
            eventodoc.CodigoSituacao = CodigoSituacao;
            eventodoc.DataHoraEvento = RnTab.ObterDataHoraServidor();
            if (ListaEvento.Count() > 0)
                eventodoc.CodigoEvento = ListaEvento.Max(x => x.CodigoEvento) + 1;
            else
                eventodoc.CodigoEvento = 1;
            eventoDAL.Inserir(eventodoc, doc.CodigoDocumento);

        }

		public long PesquisarPessoaDocumento(decimal CodDocumento, int TipoPessoa)
		{
			try
			{
				AbrirConexao();

				string comando = "Select CD_PESSOA from PESSOA_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 and TP_PESSOA = @v2 ";

				Cmd = new SqlCommand(comando, Con);
				Cmd.Parameters.AddWithValue("@v1", CodDocumento);
				Cmd.Parameters.AddWithValue("@v2", TipoPessoa);
				Dr = Cmd.ExecuteReader();

				if (Dr.Read())
					return Convert.ToInt64(Dr["CD_PESSOA"]);
				else
					return 0;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar PESSOA DO DOCUMENTO: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public bool InserirPessoaDocumento(decimal CodigoDocumento, long CodigoPessoa, int TipoPessoa)
		{
			if (CodigoPessoa == 0)
				return true;

			try
			{
				AbrirConexao();

				string strCamposPessoa = "CD_DOCUMENTO";
				string strValoresPessoa = "@v25";

				strCamposPessoa += ", TP_PESSOA";
				strValoresPessoa += ", @v26";

				strCamposPessoa += ", CD_PESSOA";
				strValoresPessoa += ", @v27";

				strCamposPessoa += ", RAZ_SOCIAL";
				strValoresPessoa += ", @v28";

				strCamposPessoa += ", INSCRICAO";
				strValoresPessoa += ", @v29";

				strCamposPessoa += ", INS_ESTADUAL";
				strValoresPessoa += ", @v30";

				strCamposPessoa += ", TELEFONE_1";
				strValoresPessoa += ", @v31";

				strCamposPessoa += ", EMAIL_NFE";
				strValoresPessoa += ", @v32";

				strCamposPessoa += ", EMAIL";
				strValoresPessoa += ", @v33";

				strCamposPessoa += ", LOGRADOURO";
				strValoresPessoa += ", @v34";

				strCamposPessoa += ", NR_ENDERECO";
				strValoresPessoa += ", @v35";

				strCamposPessoa += ", COMPLEMENTO";
				strValoresPessoa += ", @v36";

				strCamposPessoa += ", CD_CEP";
				strValoresPessoa += ", @v37";

				strCamposPessoa += ", CD_MUNICIPIO";
				strValoresPessoa += ", @v38";

				strCamposPessoa += ", CD_BAIRRO";
				strValoresPessoa += ", @v39";

				strCamposPessoa += ", DS_BAIRRO";
				strValoresPessoa += ", @v40";

				strCamposPessoa += ", EMAIL_NFSE";
				strValoresPessoa += ", @v41";

				strSQL = "insert into PESSOA_DO_DOCUMENTO (" + strCamposPessoa + ") values (" + strValoresPessoa + "); SELECT SCOPE_IDENTITY();";

				Cmd = new SqlCommand(strSQL, Con);


				PessoaDAL pessoaDAL = new PessoaDAL();
				Pessoa pessoa = new Pessoa();
				pessoa = pessoaDAL.PesquisarPessoa(CodigoPessoa);

				PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
				Pessoa_Contato pesCtt = new Pessoa_Contato();
				pesCtt = pesCttDAL.PesquisarPessoaContato(CodigoPessoa, 1);

				PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
				Pessoa_Endereco pesEnd = new Pessoa_Endereco();
				pesEnd = pesEndDAL.PesquisarPessoaEndereco(CodigoPessoa, 1);

				PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
				Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
				pesIns = pesInsDAL.PesquisarPessoaInscricao(CodigoPessoa, 1);

				Cmd.Parameters.AddWithValue("@v25", CodigoDocumento);
				Cmd.Parameters.AddWithValue("@v26", TipoPessoa);
				Cmd.Parameters.AddWithValue("@v27", CodigoPessoa);
				Cmd.Parameters.AddWithValue("@v28", pessoa.NomePessoa);
				Cmd.Parameters.AddWithValue("@v29", pesIns._NumeroInscricao);
				Cmd.Parameters.AddWithValue("@v30", pesIns._NumeroIERG);
				Cmd.Parameters.AddWithValue("@v31", pesCtt._Fone1);
				Cmd.Parameters.AddWithValue("@v32", pesCtt._MailNFE);
				Cmd.Parameters.AddWithValue("@v33", pesCtt._Mail1);
				Cmd.Parameters.AddWithValue("@v34", pesEnd._Logradouro);
				Cmd.Parameters.AddWithValue("@v35", pesEnd._NumeroLogradouro);
				Cmd.Parameters.AddWithValue("@v36", pesEnd._Complemento);
				Cmd.Parameters.AddWithValue("@v37", pesEnd._CodigoCEP);
				Cmd.Parameters.AddWithValue("@v38", pesEnd._CodigoMunicipio);
				Cmd.Parameters.AddWithValue("@v39", pesEnd._CodigoBairro);
				Cmd.Parameters.AddWithValue("@v40", pesEnd._DescricaoBairro);
				Cmd.Parameters.AddWithValue("@v41", pesCtt._MailNFSE);
				Cmd.ExecuteNonQuery();

				return true;
			}
			catch (SqlException ex)
			{
				if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
				{
					switch (ex.Errors[0].Number)
					{
						case 2601: // Primary key violation
							throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
						case 2627: // Primary key violation
							throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
						default:
							throw new Exception("Erro ao Incluir pessoa do documento: " + ex.Message.ToString());

					}
				}

				return false;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao gravar Pessoa do documento: " + ex.Message.ToString());

			}
			finally
			{
				FecharConexao();
			}
		}
		public bool AtualizarPessoaDocumento(decimal CodigoDocumento, long CodigoPessoa, int TipoPessoa)
		{
			try
			{
				AbrirConexao();

				strSQL = "update PESSOA_DO_DOCUMENTO set TP_PESSOA = @v26," +
														"CD_PESSOA = @v27," +
														"RAZ_SOCIAL = @v28," +
														"INSCRICAO = @v29," +
														"INS_ESTADUAL = @v30," +
														"TELEFONE_1 = @v31," +
														"EMAIL_NFE = @v32," +
														"EMAIL = @v33," +
														"LOGRADOURO = @v34," +
														"NR_ENDERECO = @v35," +
														"COMPLEMENTO = @v36," +
														"CD_CEP = @v37," +
														"CD_MUNICIPIO = @v38," +
														"CD_BAIRRO = @v39," +
														"DS_BAIRRO = @v40," +
														"EMAIL_NFSE = @v41 where CD_DOCUMENTO = @v25 AND TP_PESSOA = @v26";
				Cmd = new SqlCommand(strSQL, Con);

				PessoaDAL pessoaDAL = new PessoaDAL();
				Pessoa pessoa = new Pessoa();
				pessoa = pessoaDAL.PesquisarPessoa(CodigoPessoa);

				PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
				Pessoa_Contato pesCtt = new Pessoa_Contato();
				pesCtt = pesCttDAL.PesquisarPessoaContato(CodigoPessoa, 1);

				PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
				Pessoa_Endereco pesEnd = new Pessoa_Endereco();
				pesEnd = pesEndDAL.PesquisarPessoaEndereco(CodigoPessoa, 1);

				PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
				Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
				pesIns = pesInsDAL.PesquisarPessoaInscricao(CodigoPessoa, 1);

				Cmd.Parameters.AddWithValue("@v25", CodigoDocumento);
				Cmd.Parameters.AddWithValue("@v26", TipoPessoa);
				Cmd.Parameters.AddWithValue("@v27", CodigoPessoa);
				Cmd.Parameters.AddWithValue("@v28", pessoa.NomePessoa);
				Cmd.Parameters.AddWithValue("@v29", pesIns._NumeroInscricao);
				Cmd.Parameters.AddWithValue("@v30", pesIns._NumeroIERG);
				Cmd.Parameters.AddWithValue("@v31", pesCtt._Fone1);
				Cmd.Parameters.AddWithValue("@v32", pesCtt._MailNFE);
				Cmd.Parameters.AddWithValue("@v33", pesCtt._Mail1);
				Cmd.Parameters.AddWithValue("@v34", pesEnd._Logradouro);
				Cmd.Parameters.AddWithValue("@v35", pesEnd._NumeroLogradouro);
				Cmd.Parameters.AddWithValue("@v36", pesEnd._Complemento);
				Cmd.Parameters.AddWithValue("@v37", pesEnd._CodigoCEP);
				Cmd.Parameters.AddWithValue("@v38", pesEnd._CodigoMunicipio);
				Cmd.Parameters.AddWithValue("@v39", pesEnd._CodigoBairro);
				Cmd.Parameters.AddWithValue("@v40", pesEnd._DescricaoBairro);
				Cmd.Parameters.AddWithValue("@v41", pesCtt._MailNFSE);

				Cmd.ExecuteNonQuery();
				return true;
			}
			catch (SqlException ex)
			{
				if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
				{
					switch (ex.Errors[0].Number)
					{
						case 2601: // Primary key violation
							throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
						case 2627: // Primary key violation
							throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
						default:
							throw new Exception("Erro ao Incluir PESSOA DO DOCUMENTO: " + ex.Message.ToString());

					}
				}

				return false;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao gravar PESSOA DO DOCUMENTO" + ex.Message.ToString());

			}
			finally
			{
				FecharConexao();
			}
		}
		public bool ExcluirPessoaDocumento(decimal CodigoDocumento)
		{
			try
			{
				AbrirConexao();

				strSQL = "DELETE PESSOA_DO_DOCUMENTO where CD_DOCUMENTO = @v25";
				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v25", CodigoDocumento);

				Cmd.ExecuteNonQuery();
				return true;
			}
			catch (SqlException ex)
			{
				if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
				{
					switch (ex.Errors[0].Number)
					{
						case 547: // Foreign Key violation
							throw new InvalidOperationException("Exclusão não Permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
						default:
							throw new Exception("Erro ao excluir PESSOA DO DOCUMENTO: " + ex.Message.ToString());
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao gravar PESSOA DO DOCUMENTO" + ex.Message.ToString());

			}
			finally
			{
				FecharConexao();
			}
		}

		public DataTable ObterSolicitacaoCompra(decimal CodigoDocumento)
		{
			try
			{
				//// Cria DataTable
				DataTable dt = new DataTable();
				string strSQL = "Select * from Documento where cd_documento = " + CodigoDocumento;

				AbrirConexao();
				SqlCommand cmd = new SqlCommand(strSQL, Con);
				cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);
				return dt;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Documento: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}

		}
		public DataTable ObterSolicitacaoCompraProduto(decimal CodigoDocumento)
		{
			try
			{
				//// Cria DataTable
				DataTable dt = new DataTable();
				string strSQL = "Select * from Produto_do_Documento where cd_documento = " + CodigoDocumento + " order by cd_documento, cd_prod_documento ";

				AbrirConexao();
				SqlCommand cmd = new SqlCommand(strSQL, Con);
				cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);
				return dt;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Produto do Documento: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public DataTable ObterSolicitacaoCompraPessoa(decimal CodigoDocumento)
		{
			try
			{
				//// Cria DataTable
				DataTable dt = new DataTable();
				string strSQL = "Select * from Pessoa_do_Documento where cd_documento = " + CodigoDocumento + " order by cd_documento, Tp_pessoa ";

				AbrirConexao();
				SqlCommand cmd = new SqlCommand(strSQL, Con);
				cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);
				return dt;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Pessoa do Documento: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}

        public void AtualizaSituacaoSolicitacaoCompra(decimal CodigoDocumento, int CodigoSituacao, int CodigoUsuario, int CodigoMaquina)
        {



            AbrirConexao();

            SqlCommand cmd1 = new SqlCommand();
            SqlCommand cmd2 = new SqlCommand();
            SqlCommand cmd3 = new SqlCommand();


            SqlTransaction transaction = Con.BeginTransaction();

            try
            {
    
                //Monta Tela
                List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
                List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
                List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
                ProdutoDocumentoDAL RnPd = new ProdutoDocumentoDAL();
                Doc_SolCompra p1 = new Doc_SolCompra();
                Doc_SolCompraDAL docDAL = new Doc_SolCompraDAL();
                EventoDocumentoDAL eve = new EventoDocumentoDAL();
                AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();

                p1 = docDAL.PesquisarSolCompra(CodigoDocumento);
                ListaProdutos = RnPd.ObterItemSolCompra(CodigoDocumento);
                ListaProdutos = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList();
                ListaEvento = eve.ObterEventos(CodigoDocumento);
                ListaAnexo = anexo.ObterAnexos(CodigoDocumento);

                //Gravar Documento
                p1.Cpl_Maquina = CodigoMaquina;
                p1.Cpl_Usuario = CodigoUsuario;

                p1.CodigoSituacao = CodigoSituacao;
                p1.MotivoCancelamento = "";
                docDAL.SalvarSolicitacao(p1, ListaProdutos, EventoDocumento2(ListaEvento, p1.CodigoSituacao, p1.Cpl_Usuario, p1.Cpl_Maquina), ListaAnexo);

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();

                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            throw new InvalidOperationException("Alteração do Documento não permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao alterar documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao alterar documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        protected EventoDocumento EventoDocumento2(List<EventoDocumento> ListaEvento, int CodSituacao, int CodUsuario, int intMaquina)
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime DataHoraEvento = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm"));

            int intCttItem = 0;
            if (ListaEvento.Count > 0)
                intCttItem = Convert.ToInt32(ListaEvento.Max(x => x.CodigoEvento).ToString());

            intCttItem = intCttItem + 1;
            if (intCttItem != 0)
                ListaEvento.RemoveAll(x => x.CodigoEvento == intCttItem);

            EventoDocumento evento = new EventoDocumento(intCttItem, CodSituacao, DataHoraEvento, intMaquina, CodUsuario);
            return evento;
        }

    }
}
