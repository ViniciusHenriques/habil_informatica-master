using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Persistence
{
	public class Doc_OrdProducaoDAL : Conexao
	{
		protected string strSQL = "";
		//Atualizadas
		public void Inserir(Doc_OrdProducao p, List<ProdutoDocumento> listaItemOrcamento, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento)
		{
			try
			{
				AbrirConexao();
				strSQL = "insert into DOCUMENTO (CD_TIPO_DOCUMENTO," +
												"CD_EMPRESA," +
												"DT_HR_EMISSAO," +
												"DT_HR_ENTRADA," +
												"NR_DOCUMENTO," +
												"OB_DOCUMENTO," +
												"CD_SITUACAO," +
												"CD_GER_SEQ_DOC," +
												"CD_APLICACAO_USO," +
												"CD_TIPO_OPERACAO, " +
												"CD_COMPOSTO," +
												"NR_PRAZO, " +
												"TX_LOGO," +
												"TX_FORMATO, " +
												"TX_MAQUINA, " +
												"QT_PRODUZIR, " +
												"CD_USU_RESPONSAVEL," +
												"CD_DOC_ORIGINAL," +
												"VL_TOTAL_GERAL) values " +
												"(@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19); SELECT SCOPE_IDENTITY();";

				Cmd = new SqlCommand(strSQL, Con);

				decimal CodigoGerado = 0;
				GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
				if (p.Cpl_NomeTabela != null)
					CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

				Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoDocumento);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
				Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
				Cmd.Parameters.AddWithValue("@v4", p.DataHoraEmissao);
				Cmd.Parameters.AddWithValue("@v5", CodigoGerado);
				Cmd.Parameters.AddWithValue("@v6", p.DescricaoDocumento);
				Cmd.Parameters.AddWithValue("@v7", p.CodigoSituacao);
				Cmd.Parameters.AddWithValue("@v8", p.CodigoGeracaoSequencialDocumento);
				Cmd.Parameters.AddWithValue("@v9", p.CodigoAplicacaoUso);
				Cmd.Parameters.AddWithValue("@v10", p.CodigoTipoOperacao);
				Cmd.Parameters.AddWithValue("@v11", p.CodigoComposto);
				Cmd.Parameters.AddWithValue("@v12", p.Prazo);
				Cmd.Parameters.AddWithValue("@v13", p.LogoMarca);
				Cmd.Parameters.AddWithValue("@v14", p.formato);
				Cmd.Parameters.AddWithValue("@v15", p.Maquina);
				Cmd.Parameters.AddWithValue("@v16", p.QtProduzir);
				Cmd.Parameters.AddWithValue("@v17", p.CodigoOperador);
				Cmd.Parameters.AddWithValue("@v18", p.CodigoDocumentoOriginal);
				Cmd.Parameters.AddWithValue("@v19", p.ValorTotal);

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
							throw new Exception("Erro ao Incluir Ordem de Produção: " + ex.Message.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao gravar Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

				//Pessoa do Documento Cliente (colocar o código correto!!!<Mateus>)
				InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoPessoa, 15);

				InserirProdutoDocumento(p.CodigoDocumento, listaItemOrcamento);

				if (eventoDocumento != null)
				{
					EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
					eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
				}

				//if (ListaAnexoDocumento.Count > 0)
				//{
				//	AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
				//	AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);
				//}

			}
		}
		public void InserirProdutoDocumento(decimal CodigoDocumento, List<ProdutoDocumento> listaItemOrcamento)
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
																"VL_ITEM," +
																"CD_PRODUTO," +
																"DS_PRODUTO," +
																"QT_ATENDIDA," +
																"QT_PENDENTE, " +
																"CD_SITUACAO, " +
																"VL_TOTAL," +
																"CD_LOC_PRODUCAO";
					if (p.DataInicio != null)
						strSQL += ", DT_INICIO_ROTEIRO ";
					if (p.DataFim != null)
						strSQL += ", DT_FIM_ROTEIRO ";

					strSQL += ") values (@v1,(select ISNULL(max(CD_PROD_DOCUMENTO),0) + 1  from PRODUTO_DO_DOCUMENTO where CD_DOCUMENTO = " + CodigoDocumento + ")" +
						",@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12";

					if (p.DataInicio != null)
						strSQL += ",@v13";
					if (p.DataFim != null)
						strSQL += " ,@v14 ";

					strSQL += ") ";

					Cmd = new SqlCommand(strSQL, Con);
					Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
					Cmd.Parameters.AddWithValue("@v3", p.Unidade);
					Cmd.Parameters.AddWithValue("@v4", p.Quantidade);
					Cmd.Parameters.AddWithValue("@v5", p.PrecoItem);
					Cmd.Parameters.AddWithValue("@v6", p.CodigoProduto);
					Cmd.Parameters.AddWithValue("@v7", p.Cpl_DscProduto);
					Cmd.Parameters.AddWithValue("@v8", p.QuantidadeAtendida);
					Cmd.Parameters.AddWithValue("@v9", p.QuantidadePendente);
					Cmd.Parameters.AddWithValue("@v10", p.CodigoSituacao);
					Cmd.Parameters.AddWithValue("@v11", p.ValorTotalItem);
					Cmd.Parameters.AddWithValue("@v12", p.CodigoLocalizacao);
					if (p.DataInicio != null)
						Cmd.Parameters.AddWithValue("@v13", p.DataInicio);
					else
						Cmd.Parameters.AddWithValue("@v13", DBNull.Value);
					if (p.DataFim != null)
						Cmd.Parameters.AddWithValue("@v14", p.DataFim);
					else
						Cmd.Parameters.AddWithValue("@v14", DBNull.Value);
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
		public void Atualizar(Doc_OrdProducao p, List<ProdutoDocumento> listaItemOrcamento, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento)
		{
			try
			{
				Doc_OrdProducao p2 = new Doc_OrdProducao();
				p2 = PesquisarOrdem(Convert.ToInt32(p.CodigoDocumento));
				AbrirConexao();
				strSQL = "update [DOCUMENTO] set CD_TIPO_DOCUMENTO = @v1, " +
												"CD_EMPRESA = @v2, " +
												"DT_HR_EMISSAO = @v3, " +
												"DT_HR_ENTRADA = @v4, " +
												"NR_DOCUMENTO = @v5, " +
												"OB_DOCUMENTO = @v6, " +
												"CD_SITUACAO = @v7, " +
												"CD_GER_SEQ_DOC = @v8, " +
												"CD_APLICACAO_USO = @v9, " +
												"CD_TIPO_OPERACAO = @v10, " +
												"CD_COMPOSTO = @v11, " +
												"NR_PRAZO = @v12, " +
												"TX_LOGO = @v13," +
												"TX_FORMATO = @v14, " +
												"TX_MAQUINA = @v15, " +
												"QT_PRODUZIR = @v16, " +
												"CD_USU_RESPONSAVEL = @v17, " +
												"CD_DOC_ORIGINAL = @v18," +
												"VL_TOTAL_GERAL = @v19," +
												"QT_PRODUZIDA = @v20 WHERE [CD_DOCUMENTO] = @CODIGO";




				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@CODIGO", p.CodigoDocumento);
				Cmd.Parameters.AddWithValue("@v1", 10);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
				Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
				Cmd.Parameters.AddWithValue("@v4", p.DataHoraEmissao);
				Cmd.Parameters.AddWithValue("@v5", p.NumeroDocumento);
				Cmd.Parameters.AddWithValue("@v6", p.DescricaoDocumento);
				Cmd.Parameters.AddWithValue("@v7", p.CodigoSituacao);
				Cmd.Parameters.AddWithValue("@v8", p.CodigoGeracaoSequencialDocumento);
				Cmd.Parameters.AddWithValue("@v9", p.CodigoAplicacaoUso);
				Cmd.Parameters.AddWithValue("@v10", p.CodigoTipoOperacao);
				Cmd.Parameters.AddWithValue("@v11", p.CodigoComposto);
				Cmd.Parameters.AddWithValue("@v12", p.Prazo);
				Cmd.Parameters.AddWithValue("@v13", p.LogoMarca);
				Cmd.Parameters.AddWithValue("@v14", p.formato);
				Cmd.Parameters.AddWithValue("@v15", p.Maquina);
				Cmd.Parameters.AddWithValue("@v16", p.QtProduzir);
				Cmd.Parameters.AddWithValue("@v17", p.CodigoOperador);
				Cmd.Parameters.AddWithValue("@v18", p.CodigoDocumentoOriginal);
				Cmd.Parameters.AddWithValue("@v19", p.ValorTotal);
				Cmd.Parameters.AddWithValue("@v20", p.QtProduzida);
				Cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
				AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoPessoa, 6);

				InserirProdutoDocumento(p.CodigoDocumento, listaItemOrcamento);

				if (eventoDocumento != null)
				{
					EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
					eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
				}
				AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
				AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);

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

				if (CodigoSituacaoAnterior != 0 && CodigoSituacaoAnterior != CodigoSituacaoNova)
				{
					Doc_OrdProducao doc = new Doc_OrdProducao();
					Doc_OrdProducaoDAL docDAL = new Doc_OrdProducaoDAL();
					doc = docDAL.PesquisarOrdem(CodigoDocumento);
					doc.Cpl_Maquina = CodigoMaquina;
					doc.Cpl_Usuario = CodigoUsuario;
					if (doc != null)
						EventoDocumento(doc, CodigoSituacaoNova);
				}


				Cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar situacao do  Ordem de Produção: " + ex.Message.ToString());
			}

		}
		public void AtualizarQuantidade(decimal qtPendente, decimal decDoc)
		{
			try
			{
				AbrirConexao();
				strSQL = "update [PRODUTO_DO_DOCUMENTO] set QT_PENDENTE = @v1 WHERE CD_DOCUMENTO = @v2 ";

				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v1", qtPendente);
				Cmd.Parameters.AddWithValue("@v2", decDoc);

				Cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar situacao do  Ordem de Produção: " + ex.Message.ToString());
			}

		}
		public void AtualizaParaEncerramento(Doc_OrdProducao p)
		{
			try
			{
				AbrirConexao();
				strSQL = "update [DOCUMENTO] set DT_ENCERRAMENTO =getdate() WHERE CD_DOCUMENTO = @v2 ";

				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v2", p.CodigoDocumento);
				Cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar situacao do  Ordem de Produção: " + ex.Message.ToString());
			}
		}
		public void SalvarOrdem(Doc_OrdProducao p,
					   List<ProdutoDocumento> listaItemOrcamento,
					   EventoDocumento eventoDocumento, 
					   List<AnexoDocumento> ListaAnexoDocumento,
					   int CodMaquina, int CodUsuario)
		{
            try
            {
                List<Habil_Log> listaLog = new List<Habil_Log>();
                Habil_LogDAL Rn_Log = new Habil_LogDAL();

                DataTable tbA, tbB;

                if (p.CodigoDocumento == 0) //Registro Novo
                {
					Inserir(p, listaItemOrcamento, eventoDocumento, ListaAnexoDocumento);
				}
				else
                {
                    tbA = ObterOrdem(p.CodigoDocumento);

					if(eventoDocumento != null)
						Atualizar(p, listaItemOrcamento, eventoDocumento, ListaAnexoDocumento);
                    else
						Atualizar(p, listaItemOrcamento, null, ListaAnexoDocumento);

					//new DBTabelaDAL().ExecutaComandoSQL("update REGRA_FISCAL_ICMS set DT_GERACAO = getdate() where CD_REGRA_FISCAL_ICMS = " + p.CodigoDocumento.ToString());

					tbB = ObterOrdem(p.CodigoDocumento);
					listaLog = Rn_Log.ComparaDataTables(tbA, tbB, Convert.ToDouble(p.CodigoDocumento), CodUsuario, CodMaquina, 16, "DOCUMENTO");
					foreach (Habil_Log item in listaLog)
						Rn_Log.Inserir(item);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    throw new Exception("Erro ao Incluir Regra Fiscal Icms: " + ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Salvar Regra Fiscal Icms: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
		}
		public void Cancelar(decimal CodigoDocumento)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("update DOCUMENTO set cd_situacao = 178 , CD_DOC_ORIGINAL = 0" +
					" Where CD_DOCUMENTO= @v1 ", Con);
				Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
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
							throw new Exception("Erro ao Cancelar Documento: " + ex.Message.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Cancelar produtos do documento: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Doc_OrdProducao PesquisarOrdem(decimal Codigo)
		{
			try
			{
				long CodCliente = ObterCodPesDocumento(Codigo, 15);

				AbrirConexao();
				strSQL = "Select * from [VW_DOC_ORD_PRODUCAO] Where CD_DOCUMENTO = @v1";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Dr = Cmd.ExecuteReader();
				Doc_OrdProducao p = new Doc_OrdProducao();

				if (Dr.Read())
				{
					p = new Doc_OrdProducao();

					p.CodigoDocumentoOriginal = 0;

					p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
					p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);

					if (Dr["CD_DOC_ORIGINAL"] != DBNull.Value)
						p.CodigoDocumentoOriginal = Convert.ToInt32(Dr["CD_DOC_ORIGINAL"]);

					p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);

					if (Dr["DT_ENCERRAMENTO"] != DBNull.Value)
						p.DataEncerramento = Convert.ToDateTime(Dr["DT_ENCERRAMENTO"]);

					p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
					p.Cpl_CodigoPessoa = CodCliente;
					p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
					p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
					p.CodigoOperador = Convert.ToInt32(Dr["CD_USU_RESPONSAVEL"]);
					p.CodigoComposto = Convert.ToInt32(Dr["CD_COMPOSTO"]);
					p.QtProduzir = Convert.ToDecimal(Dr["QT_PRODUZIR"]);
					p.QtProduzida = Convert.ToDecimal(Dr["QT_PRODUZIDA"]);
					p.formato = Convert.ToString(Dr["TX_FORMATO"]);
					p.LogoMarca = Convert.ToString(Dr["TX_LOGO"]);
					p.Maquina = Convert.ToString(Dr["TX_MAQUINA"]);
					p.Prazo = Convert.ToInt32(Dr["NR_PRAZO"]);
				}
				return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Doc_OrdProducao> ListarOrdemProducao(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [VW_DOC_ORD_PRODUCAO] ";

				if (strValor != "")
					strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


				if (strOrdem != "")
					strSQL = strSQL + "Order By " + strOrdem;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Doc_OrdProducao> lista = new List<Doc_OrdProducao>();

				while (Dr.Read())
				{
					Doc_OrdProducao p = new Doc_OrdProducao();

					p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
					p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
					p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
					p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
					p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
					p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
					p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
					p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
					p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
					p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
					p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
					p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();

					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas as Ordens de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Doc_OrdProducao> ListarOrdemProducaoaoCompleto(List<DBTabelaCampos> ListaFiltros)
		{
			try
			{
				AbrirConexao();
				string strValor = "";
				string strSQL = "Select * from [VW_DOC_ORD_PRODUCAO] ";


				strValor = MontaFiltroIntervalo(ListaFiltros);
				strSQL = strSQL + strValor;

				strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";
				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Doc_OrdProducao> lista = new List<Doc_OrdProducao>();

				while (Dr.Read())
				{
					Doc_OrdProducao p = new Doc_OrdProducao();
					p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
					if (Dr["VL_TOTAL_GERAL"] != DBNull.Value)
						p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
					p.CodigoComposto = Convert.ToInt32(Dr["CD_composto"]);
					p.Cpl_NomeProduto = Convert.ToString(Dr["nm_produto"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
					p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
					p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.Cpl_Pessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
					p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
					p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
					p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar todas as Ordens de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public long ObterCodPesDocumento(decimal CodDocumento, int TipoPessoa)
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
		public List<Doc_OrdProducao> ListarDocumentosNaoListados(string strNomeCliente, int empresa, decimal NrDocumento)
		{
			try
			{
				AbrirConexao();
				string strSQL = "select d.CD_DOCUMENTO,	" +
								"d.CD_TIPO_DOCUMENTO, " +
								"d.CD_EMPRESA, " +
								"d.DT_HR_ENTRADA, " +
								"d.DT_HR_EMISSAO, " +
								"d.NR_DOCUMENTO, " +
								"d.CD_SITUACAO,	" +
								"isnull(d.nr_prazo,0) as nr_prazo, " +
								"d.DT_ENTREGA, " +
								"d.CD_TIPO_OPERACAO, " +
								"d.CD_DOC_ORIGINAL, " +
								"t.DS_TIPO_OPERACAO, " +
								"h.DS_TIPO, " +
								"c.CD_COMPOSTO, " +
								"prod.NM_PRODUTO, " +
								"psd.cd_pessoa, " +
								"psd.raz_social  " +
								"from DOCUMENTO as d " +
								"	inner join PRODUTO_DO_DOCUMENTO as pd " +
								"		on pd.CD_DOCUMENTO = d.CD_DOCUMENTO " +
								"	inner join COMPOSICAO as c " +
								"		on c.CD_COMPOSTO = pd.CD_PRODUTO " +
								"	inner join PRODUTO as prod " +
								"		on prod.CD_PRODUTO = c.CD_COMPOSTO " +
								"	inner join TIPO_DE_OPERACAO as t " +
								"		on d.CD_TIPO_OPERACAO = t.CD_TIPO_OPERACAO " +
								"	inner join EMPRESA as e " +
								"		on e.CD_EMPRESA = d.CD_EMPRESA  " +
								"	inner join habil_tipo as h " +
								"		on h.cd_tipo = d.CD_SITUACAO " +
								"	inner join PESSOA_DO_DOCUMENTO as psd " +
								"		on d.CD_DOCUMENTO = psd.CD_DOCUMENTO " +
								"		and tp_pessoa = 12 " +
								"where d.CD_SITUACAO = 146 and d.cd_empresa = " + empresa + "" +
								" and CD_TIPO_DOCUMENTO = 8 " +
								" 	AND d.cd_documento not in (SELECT isnull(d1.CD_DOC_ORIGINAL,0) FROM DOCUMENTO as d1 WHERE d1.CD_TIPO_DOCUMENTO = 10 and pd.cd_produto = d1.cd_composto) ";


				if (strNomeCliente != "")
				{
					strSQL += " and psd.raz_social like '%" + strNomeCliente + "%'";
				}
				if (NrDocumento != 0)
				{
					strSQL += " and d.nr_documento = " + NrDocumento;
				}

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Doc_OrdProducao> lista = new List<Doc_OrdProducao>();

				while (Dr.Read())
				{
					Doc_OrdProducao p = new Doc_OrdProducao();

					p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
					p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
					p.Cpl_DsTipoOperacao = Convert.ToString(Dr["DS_TIPO_OPERACAO"]);
					p.Cpl_CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
					p.Cpl_Pessoa = Convert.ToString(Dr["raz_social"]);
					p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
					p.CodigoComposto = Convert.ToInt32(Dr["cd_composto"]);
					p.Cpl_NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
					p.Prazo = Convert.ToInt32(Dr["nr_prazo"]);

					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas as Ordens de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Pessoa> ListarUsuarios()
		{
			try
			{
				AbrirConexao();

				string strSQL = "select CD_PESSOA, NM_COMPLETO  from usuario ";
				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Pessoa> lista = new List<Pessoa>();

				while (Dr.Read())
				{
					Pessoa p = new Pessoa();

					p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_COMPLETO"]);

					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas as Ordens de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public Doc_OrdProducao QuantidadesDisponiveisDoComponente(int intCod, int intEmp)
		{
			try
			{
				AbrirConexao();

				strSQL = "select v.CD_EMPRESA, v.CD_PRODUTO, v.QT_DISPONIVEL, p.NM_PRODUTO " +
					"from VW_ESTOQUE_TOTAL_DISPONIVEL as v " +
					"inner join produto as p " +
					"on p.CD_PRODUTO = v.CD_PRODUTO " +
					"where v.CD_PRODUTO = " + intCod;

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				Doc_OrdProducao p = new Doc_OrdProducao();

				if (Dr.Read())
				{
					p = new Doc_OrdProducao();

					p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.Quantidade = Convert.ToDecimal(Dr["QT_DISPONIVEL"]);
					p.Cpl_NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
				}
				return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todas as Ordens de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}

		}
		public bool ExecutaSpAtendimentoProducao(int maquina, string strDocumentos)
		{
			//[dbo].[sp_separa_documento_producao]

			bool blnRetorno = false;
			AbrirConexao();
			try
			{
				SqlCommand sqlComand = new SqlCommand("[dbo].[sp_separa_documento_producao]", Con);

				sqlComand.CommandType = CommandType.StoredProcedure;
				sqlComand.Parameters.AddWithValue("@CD_MAQUINA", maquina);
				sqlComand.Parameters.AddWithValue("@lst_cd_documento", strDocumentos);

				sqlComand.ExecuteReader();
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Executar Sp Separa Documento da Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
				blnRetorno = true;
			}
			return blnRetorno;
		}
		public EstoqueProduto PesquisarAtendimentoDoDocumento(decimal Codigo)
		{
			try
			{
				long CodCliente = ObterCodPesDocumento(Codigo, 15);

				AbrirConexao();
				strSQL = "Select * from ATENDIMENTO_DO_DOCUMENTO where CD_DOCUMENTO = @v1";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Dr = Cmd.ExecuteReader();
				EstoqueProduto p = new EstoqueProduto();

				if (Dr.Read())
				{
					p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_LOCALIZACAO"]);
					p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
					p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
					p.Quantidade = Convert.ToDecimal(Dr["Qt_atendida"]);

				}
				return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Doc_OrdProducao> ListarOrdemProducaoParaEncerramento(decimal decDoc)
		{
			try
			{
				AbrirConexao();

				string strSQL = "select d.CD_DOCUMENTO, a.CD_LOCALIZACAO, a.CD_LOTE, a.CD_EMPRESA, a.CD_PRODUTO, a.QT_ATENDIDA from documento as d " +
								"inner join PRODUTO_DO_DOCUMENTO as p " +
								"on p.CD_DOCUMENTO = d.CD_DOCUMENTO " +
								"inner join ATENDIMENTO_DO_DOCUMENTO as a " +
								"on a.CD_DOCUMENTO = d.CD_DOCUMENTO a" +
								"nd p.CD_PRODUTO = a.CD_PRODUTO " +
								"where d.cd_documento = " + decDoc;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Doc_OrdProducao> lista = new List<Doc_OrdProducao>();

				while (Dr.Read())
				{
					Doc_OrdProducao p = new Doc_OrdProducao();

					p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["cd_empresa"]);
					p.CodigoLocalizacao = Convert.ToInt32(Dr["cd_localizacao"]);
					p.CodigoLote = Convert.ToInt32(Dr["cd_lote"]);
					p.CodigoProduto = Convert.ToInt32(Dr["cd_produto"]);
					p.QtAtendida = Convert.ToDecimal(Dr["QT_ATENDIDA"]);

					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas as Ordens de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		//Não mexidas
		public DataTable RelOrdemProducao(decimal CodigoDocumento)
		{
			try
			{
				//// Cria DataTable
				DataTable dt = new DataTable();
				string strSQL = "SELECT " +
								"d.CD_DOCUMENTO, " +
								"d.NR_DOCUMENTO," +
								"atd.QT_ATENDIDA, " +
								"pd.QT_SOLICITADA, 	" +
								"atd.CD_PRODUTO, " +
								"d.CD_TIPO_DOCUMENTO, " +
								"d.CD_EMPRESA, " +
								"d.DT_HR_ENTRADA," +
								"d.DT_HR_EMISSAO, 	" +
								"d.CD_SITUACAO, " +
								"d.VL_TOTAL_GERAL, " +
								"d.OB_DOCUMENTO, " +
								"d.CD_TIPO_OPERACAO, " +
								"d.CD_DOC_ORIGINAL," +
								"d.CD_USU_RESPONSAVEL, " +
								"d.CD_APLICACAO_USO, " +
								"d.DT_HR_SAIDA, " +
								"d.CD_COMPOSTO, " +
								"d.NR_PRAZO, " +
								"d.TX_LOGO," +
								"d.TX_FORMATO, " +
								"d.QT_PRODUZIR, " +
								"d.QT_PRODUZIDA, " +
								"d.DT_ENCERRAMENTO, " +
								"d.TX_MAQUINA, " +
								"d.CD_COMPOSTO, " +
								"PSD.RAZ_SOCIAL, psd.CD_PESSOA, " +
								"pd.CD_PROD_DOCUMENTO, " +
								"pd.CD_PRODUTO, " +
								"pd.DS_PRODUTO, " +
								"pd.VL_TOTAL, 	" +
								"pd.QT_PENDENTE," +
								"Pd.qt_atendida as qt_atendida_total, " +
								"pd.CD_LOC_PRODUCAO," +
								"pd.DT_INICIO_ROTEIRO, " +
								"pd.DT_FIM_ROTEIRO, " +
								"it.CD_ROTEIRO, it.DS_ROTEIRO, " +
								"atd.CD_PRODUTO AS CD_PRODUTO_ATENDIMENTO, " +
								"atd.CD_LOCALIZACAO as CD_LOCALIZACAO_atd, " +
								"atd.CD_LOTE, " +
								"atd.QT_ATENDIDA , " +
								"loc.CD_LOCALIZACAO, " +
								"loc.DS_LOCALIZACAO," +
								"p.NM_PRODUTO AS DS_PRODUTO," +
								"comp.NM_PRODUTO AS DS_COMPOSTO, " +
								"u.SIGLA, " +
								"H.DS_TIPO AS DS_SITUACAO, " +
								"HT.DS_TIPO AS DS_APLICACAO_USO, " +
								"usu.NM_COMPLETO as ds_operador, " +
								"ISNULL(L.NR_LOTE + ' - ' + L.SR_LOTE + ' - F:' + CONVERT(VARCHAR, L.DT_FABRICACAO,	" +
								"                          103) + ' - V:' + CONVERT(VARCHAR, L.DT_VALIDADE, 103), '') AS CPL_LOTE" +
								" FROM dbo.DOCUMENTO AS d " +
								"	INNER JOIN dbo.PRODUTO_DO_DOCUMENTO AS pd " +
								"		ON d.CD_DOCUMENTO = pd.CD_DOCUMENTO " +
								"	INNER JOIN dbo.ATENDIMENTO_DO_DOCUMENTO AS atd 	" +
								"		ON atd.CD_DOCUMENTO = d.CD_DOCUMENTO AND pd.CD_PRODUTO = atd.CD_PRODUTO " +
								"	inner join LOCALIZACAO as loc " +
								"		on atd.CD_LOCALIZACAO = loc.CD_INDEX" +
								"	INNER JOIN PESSOA_DO_DOCUMENTO AS PSD " +
								"		ON PSD.CD_DOCUMENTO = D.CD_DOCUMENTO " +
								"	inner join ITEM_DA_COMPOSICAO as it " +
								"		on pd.CD_PRODUTO = it.CD_COMPONENTE" +
								"	INNER JOIN dbo.PRODUTO AS p " +
								"		ON p.CD_PRODUTO = pd.CD_PRODUTO " +
								"	INNER JOIN dbo.PRODUTO AS comp " +
								"		ON comp.CD_PRODUTO = d.CD_COMPOSTO " +
								"	INNER JOIN dbo.UNIDADE AS u " +
								"		ON u.CD_UNIDADE = p.CD_UNIDADE	" +
								"	LEFT JOIN dbo.LOTE AS L " +
								"		ON atd.CD_LOTE = L.CD_INDEX " +
								"		AND pd.CD_PRODUTO = L.CD_PRODUTO " +
								"	INNER JOIN HABIL_TIPO AS H " +
								"		ON H.CD_TIPO = D.CD_SITUACAO " +
								"	INNER JOIN HABIL_TIPO AS HT " +
								"		ON Ht.CD_TIPO = D.CD_APLICACAO_USO " +
								"	INNER JOIN [USUARIO] AS Usu " +
								"		on Usu.CD_USUARIO = d.CD_USU_RESPONSAVEL " +
								"" +
								" WHERE (d.CD_DOCUMENTO = @V1) AND (d.CD_TIPO_DOCUMENTO = 10)";

				AbrirConexao();
				SqlCommand cmd = new SqlCommand(strSQL, Con);
				cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);
				return dt;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Relatório de Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public DataTable RelOP(decimal CodigoDocumento)
		{
			try
			{
				//// Cria DataTable
				DataTable dt = new DataTable();
				string strSQL = "SELECT " +
								"d.CD_DOCUMENTO, " +
								"d.NR_DOCUMENTO," +
								"atd.QT_ATENDIDA, " +
								"pd.QT_SOLICITADA, 	" +
								"atd.CD_PRODUTO, " +
								"d.CD_TIPO_DOCUMENTO, " +
								"d.CD_EMPRESA, " +
								"d.DT_HR_ENTRADA," +
								"d.DT_HR_EMISSAO, 	" +
								"d.CD_SITUACAO, " +
								"d.VL_TOTAL_GERAL, " +
								"d.OB_DOCUMENTO, " +
								"d.CD_TIPO_OPERACAO, " +
								"d.CD_DOC_ORIGINAL," +
								"d.CD_USU_RESPONSAVEL, " +
								"d.CD_APLICACAO_USO, " +
								"d.DT_HR_SAIDA, " +
								"d.CD_COMPOSTO, " +
								"d.NR_PRAZO, " +
								"d.TX_LOGO," +
								"d.TX_FORMATO, " +
								"d.QT_PRODUZIR, " +
								"d.QT_PRODUZIDA, " +
								"d.DT_ENCERRAMENTO, " +
								"d.TX_MAQUINA, " +
								"d.CD_COMPOSTO, " +
								"PSD.RAZ_SOCIAL, psd.CD_PESSOA, " +
								"pd.CD_PROD_DOCUMENTO, " +
								"pd.CD_PRODUTO, " +
								"pd.DS_PRODUTO, " +
								"VL_ITEM, " +
								"pd.VL_TOTAL, 	" +
								"pd.QT_PENDENTE," +
								"Pd.qt_atendida as qt_atendida_total, " +
								"pd.CD_LOC_PRODUCAO," +
								"pd.DT_INICIO_ROTEIRO, " +
								"pd.DT_FIM_ROTEIRO, " +
								"it.CD_ROTEIRO, it.DS_ROTEIRO, " +
								"atd.CD_PRODUTO AS CD_PRODUTO_ATENDIMENTO, " +
								"atd.CD_LOCALIZACAO as CD_LOCALIZACAO_atd, " +
								"atd.CD_LOTE, " +
								"atd.QT_ATENDIDA , " +
								"loc.CD_LOCALIZACAO, " +
								"loc.DS_LOCALIZACAO," +
								"p.NM_PRODUTO AS DS_PRODUTO," +
								"comp.NM_PRODUTO AS DS_COMPOSTO, " +
								"u.SIGLA, " +
								"H.DS_TIPO AS DS_SITUACAO, " +
								"HT.DS_TIPO AS DS_APLICACAO_USO, " +
								"usu.NM_COMPLETO as ds_operador, " +
								"ISNULL(L.NR_LOTE + ' - ' + L.SR_LOTE + ' - F:' + CONVERT(VARCHAR, L.DT_FABRICACAO,	" +
								"                          103) + ' - V:' + CONVERT(VARCHAR, L.DT_VALIDADE, 103), '') AS CPL_LOTE" +
								" FROM dbo.DOCUMENTO AS d " +
								"	INNER JOIN dbo.PRODUTO_DO_DOCUMENTO AS pd " +
								"		ON d.CD_DOCUMENTO = pd.CD_DOCUMENTO " +
								"	LEFT JOIN dbo.ATENDIMENTO_DO_DOCUMENTO AS atd 	" +
								"		ON atd.CD_DOCUMENTO = d.CD_DOCUMENTO AND pd.CD_PRODUTO = atd.CD_PRODUTO " +
								"	LEFT join LOCALIZACAO as loc " +
								"		on atd.CD_LOCALIZACAO = loc.CD_INDEX" +
								"	INNER JOIN PESSOA_DO_DOCUMENTO AS PSD " +
								"		ON PSD.CD_DOCUMENTO = D.CD_DOCUMENTO " +
								"	inner join ITEM_DA_COMPOSICAO as it " +
								"		on pd.CD_PRODUTO = it.CD_COMPONENTE" +
								"	INNER JOIN dbo.PRODUTO AS p " +
								"		ON p.CD_PRODUTO = pd.CD_PRODUTO " +
								"	INNER JOIN dbo.PRODUTO AS comp " +
								"		ON comp.CD_PRODUTO = d.CD_COMPOSTO " +
								"	INNER JOIN dbo.UNIDADE AS u " +
								"		ON u.CD_UNIDADE = p.CD_UNIDADE	" +
								"	LEFT JOIN dbo.LOTE AS L " +
								"		ON atd.CD_LOTE = L.CD_INDEX " +
								"		AND pd.CD_PRODUTO = L.CD_PRODUTO " +
								"	INNER JOIN HABIL_TIPO AS H " +
								"		ON H.CD_TIPO = D.CD_SITUACAO " +
								"	INNER JOIN HABIL_TIPO AS HT " +
								"		ON Ht.CD_TIPO = D.CD_APLICACAO_USO " +
								"	INNER JOIN [USUARIO] AS Usu " +
								"		on Usu.CD_USUARIO = d.CD_USU_RESPONSAVEL " +
								"" +
								" WHERE (d.CD_DOCUMENTO = @V1) AND (d.CD_TIPO_DOCUMENTO = 10)";

				AbrirConexao();
				SqlCommand cmd = new SqlCommand(strSQL, Con);
				cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);
				return dt;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Relatório de Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public DataTable ObterOrdem(decimal CodigoDocumento)
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

				throw new Exception("Erro ao Listar Relatório de Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public void EventoDocumento(Doc_OrdProducao doc, int CodigoSituacao)
		{
			List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
			EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
			ListaEvento = eventoDAL.ObterEventos(doc.CodigoDocumento);
			DBTabelaDAL RnTab = new DBTabelaDAL();

			EventoDocumento eventodoc = new EventoDocumento();
			eventodoc.CodigoDocumento = Convert.ToDecimal(doc.CodigoDocumento);
			eventodoc.CodigoMaquina = doc.Cpl_Maquina;
			eventodoc.CodigoUsuario = doc.Cpl_Usuario;
			eventodoc.CodigoSituacao = CodigoSituacao;
			eventodoc.DataHoraEvento = RnTab.ObterDataHoraServidor();
			if (ListaEvento.Count() > 0)
				eventodoc.CodigoEvento = ListaEvento.Max(x => x.CodigoEvento) + 1;
			else
				eventodoc.CodigoEvento = 1;
			eventoDAL.Inserir(eventodoc, doc.CodigoDocumento);

		}

	}
}
