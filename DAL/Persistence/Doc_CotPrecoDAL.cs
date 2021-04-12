using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Persistence
{
    //teste
    public class Doc_CotPrecoDAL : Conexao
    {
        protected string strSQL = "";
		public void SalvarCotacao(Doc_CotPreco p, List<ProdutoDocumento> listaProdutos, EventoDocumento eventoDocumento, 
            List<AnexoDocumento> ListaAnexoDocumento, List<Pessoa>ListaFornecedores, List<PessoaProdutoDocumento> listaCotacoesProdutos)
		{
			try
			{
				List<Habil_Log> listaLog = new List<Habil_Log>();
				Habil_LogDAL Rn_Log = new Habil_LogDAL();
                DBTabelaDAL RnTabela = new DBTabelaDAL(); 


                DataTable tbA, tbB;

				if (p.CodigoDocumento == 0) //Registro Novo
					Inserir(p, listaProdutos, eventoDocumento, ListaAnexoDocumento, ListaFornecedores, listaCotacoesProdutos);
				else
				{
					tbA = ObterCotacaoCompra(p.CodigoDocumento);

					if (eventoDocumento != null)
						Atualizar(p, listaProdutos, eventoDocumento, ListaAnexoDocumento, ListaFornecedores, listaCotacoesProdutos);
					else
						Atualizar(p, listaProdutos, null, ListaAnexoDocumento, ListaFornecedores, listaCotacoesProdutos);

					tbB = ObterCotacaoCompra(p.CodigoDocumento);
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
					throw new Exception("Erro ao Incluir Salvar Cotação de Preço: " + ex.Message.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Salvar Cotação de Preço: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public void Inserir(Doc_CotPreco p, List<ProdutoDocumento> listaItemCotacao, EventoDocumento eventoDocumento, 
            List<AnexoDocumento> ListaAnexoDocumento, List<Pessoa> ListaFornecedores, List<PessoaProdutoDocumento> listaCotacoesProdutos)
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
												"DT_HR_ENTRADA, OB_DOCUMENTO,CD_DOC_ORIGINAL) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12) SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                decimal CodigoGerado = 0;
                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                if(p.Cpl_NomeTabela != null)
                    CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", 13);
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
                Cmd.Parameters.AddWithValue("@v12", p.CodigoDocumentoOriginal);

                

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

                short shtTpPessoa = 15;
                foreach (var item in ListaFornecedores)
                {
                    shtTpPessoa++;
                    ExcluirPessoaDocumento(p.CodigoDocumento, shtTpPessoa);//Pessoa do Documento Fornecedor
                    InserirPessoaDocumento(p.CodigoDocumento, item.CodigoPessoa, shtTpPessoa);//Pessoa do Documento Fornecedor
                }

                if (eventoDocumento != null)
				{
					EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
					eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
				}
				AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
				AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);

                InserirProduto(p.CodigoDocumento, listaItemCotacao);

                PessoaProdutoDocumentoDAL PpdDAL = new PessoaProdutoDocumentoDAL();
                PpdDAL.InserirItem(p.CodigoDocumento, listaCotacoesProdutos);

            }
        }
        public void Atualizar(Doc_CotPreco p, List<ProdutoDocumento> listaItemCotacao, EventoDocumento eventoDocumento, 
            List<AnexoDocumento> ListaAnexoDocumento, List<Pessoa> ListaFornecedores, List<PessoaProdutoDocumento> listaCotacoesProdutos)
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
                                                " CD_DOC_ORIGINAL = @v9 " +
												" Where [CD_DOCUMENTO] = @CODIGO ";

                Cmd = new SqlCommand(strSQL, Con);


                Cmd.Parameters.AddWithValue("@CODIGO", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v1", 13);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v5", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v6", p.DataValidade);
				Cmd.Parameters.AddWithValue("@v7", p.ValorTotal);
				Cmd.Parameters.AddWithValue("@v8", p.DescricaoDocumento);

                if (p.CodigoSituacao == 203)
                    p.CodigoDocumentoOriginal = 0;

                Cmd.Parameters.AddWithValue("@v9", p.CodigoDocumentoOriginal);

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

				tbProdDocA = ObterCotacaoCompraPessoa(p.CodigoDocumento);
                short shtTpPessoa = 15;
                foreach (var item in ListaFornecedores)
                {
                    shtTpPessoa++;
                    ExcluirPessoaDocumento(p.CodigoDocumento, shtTpPessoa);//Pessoa do Documento Fornecedor
                    InserirPessoaDocumento(p.CodigoDocumento, item.CodigoPessoa, shtTpPessoa);//Pessoa do Documento Fornecedor
                }
                tbProdDocB = ObterCotacaoCompraPessoa(p.CodigoDocumento);

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

				tbProdDocA = ObterCotacaoCompraProduto(p.CodigoDocumento);
				InserirProduto(p.CodigoDocumento, listaItemCotacao);
				tbProdDocB = new DataTable();
				tbProdDocB = ObterCotacaoCompraProduto(p.CodigoDocumento);

				listaLog = new List<Habil_Log>();
				listaLog = Rn_Log.ComparaDataTablesRelacionalProduto_do_Documento(tbProdDocA, tbProdDocB, Convert.ToDouble ( p.CodigoDocumento), p.Cpl_Usuario, p.Cpl_Maquina, 18, 19,20, "PRODUTO_DO_DOCUMENTO", "CD_DOCUMENTO", "CD_PROD_DOCUMENTO");
				foreach (Habil_Log item in listaLog)
					Rn_Log.Inserir(item);

                PessoaProdutoDocumentoDAL PpdDAL = new PessoaProdutoDocumentoDAL();
                PpdDAL.InserirItem(p.CodigoDocumento, listaCotacoesProdutos);

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
                                                                "DS_PRODUTO,CD_SITUACAO, OB_PROD_DOCUMENTO) values (@v1,@v2,@v3,@v4,@v5,@v6,135, @v7)";

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
                    Doc_CotPreco doc = new Doc_CotPreco();
                    Doc_CotPrecoDAL docDAL = new Doc_CotPrecoDAL();
                    doc = docDAL.PesquisarCotPreco(CodigoDocumento);
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
		public List<Doc_CotPreco> ListarCotPrecoCompleto(List<DBTabelaCampos> ListaFiltros)
		{
			try
			{
				AbrirConexao();
				string strValor = "";
				string strSQL = "Select * from [VW_DOC_COT_PRECO] ";


				strValor = MontaFiltroIntervalo(ListaFiltros);
				strSQL = strSQL + strValor;

				strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";
				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Doc_CotPreco> lista = new List<Doc_CotPreco>();

				while (Dr.Read())
				{
                    Doc_CotPreco p = new Doc_CotPreco();
					p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
					p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
					p.DataValidade = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
					p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
					p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["PessoasPARTICIPANTES"]);
                    p.Cpl_DscProdutos = Convert.ToString(Dr["PRODUTOSDOCUMENTO"]);
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
		public Doc_CotPreco PesquisarCotPreco(decimal Codigo)
        {
            try
            {

                AbrirConexao();
                strSQL = "Select * from [VW_DOC_COT_PRECO] Where CD_DOCUMENTO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Doc_CotPreco p = null;

                if (Dr.Read())
                {
                    p = new Doc_CotPreco();
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

                    if (Dr["CD_DOC_ORIGINAL"] != DBNull.Value)
                        p.CodigoDocumentoOriginal = Convert.ToInt32(Dr["CD_DOC_ORIGINAL"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Doc_CotPreco: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void EventoDocumento(Doc_CotPreco doc, int CodigoSituacao)
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
        public List<Pessoa> ListarPessoasDocumento(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();

                string strSQL = " Select P.*, SUBSTRING(Pend.DS_ESTADO, 0, 3) AS DS_ESTADO, Pend.DS_MUNICIPIO, CTT.NM_CONTATO, CTT.NR_FONE1, CTT.TX_MAIL1  " +
                                    " from Pessoa as P " +
                                    "   Inner Join Pessoa_do_Documento as PD On P.CD_PESSOA = PD.CD_PESSOA " +
                                    "   INNER JOIN PESSOA_ENDERECO AS PEND ON PEND.CD_PESSOA = P.CD_PESSOA AND PEND.CD_ENDERECO = 1 " +
                                    "   INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = P.CD_PESSOA AND INS.CD_INSCRICAO = 1 " +
                                    "   INNER JOIN PESSOA_CONTATO AS CTT ON CTT.CD_PESSOA = P.CD_PESSOA AND CTT.CD_CONTATO = 1 " +
                                    " where P.IN_FORNECEDOR = 1 and PD.CD_DOCUMENTO = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);

                Dr = Cmd.ExecuteReader();

                List<Pessoa> lista = new List<Pessoa>();
                Pessoa p;

                while (Dr.Read())
                {
                    p = new Pessoa();
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
                    p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
                    p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
                    p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
                    p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
                    p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
                    p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
                    p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
                    p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
                    p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
                    p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
                        p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
                    p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
                    p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador = Convert.ToInt32(Dr["IN_COMPRADOR"]);

                    p.Cpl_Municipio = Dr["DS_MUNICIPIO"].ToString();
                    p.Cpl_Estado = Dr["DS_ESTADO"].ToString();
                    p.Cpl_Fone = Dr["NR_FONE1"].ToString();
                    p.Cpl_Email = Dr["TX_MAIL1"].ToString();

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
		public bool ExcluirPessoaDocumento(decimal CodigoDocumento, int TipoPessoa)
		{
			try
			{
				AbrirConexao();

				strSQL = "DELETE PESSOA_DO_DOCUMENTO where CD_DOCUMENTO = @v25 And TP_PESSOA = @v26 ";
                
                Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v25", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v26", TipoPessoa);

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
		public DataTable ObterCotacaoCompra(decimal CodigoDocumento)
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
		public DataTable ObterCotacaoCompraProduto(decimal CodigoDocumento)
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
		public DataTable ObterCotacaoCompraPessoa(decimal CodigoDocumento)
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


    }
}

