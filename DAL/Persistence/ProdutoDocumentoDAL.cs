using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class ProdutoDocumentoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(decimal CodigoDocumento, List<ProdutoDocumento> listaItemOrcamento)
        {
            try
            {
                ExcluirTodos(CodigoDocumento);
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
                                                                "VL_TOTAL," +
                                                                "VL_DESCONTO," +
                                                                "QT_ATENDIDA," +
                                                                "QT_PENDENTE, " +
                                                                "CD_SITUACAO," +
                                                                "VL_VOLUME," +
                                                                "VL_PESO," +
                                                                "VL_FATOR_CUBAGEM) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15)";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoItem);
                    Cmd.Parameters.AddWithValue("@v3", p.Unidade);
                    Cmd.Parameters.AddWithValue("@v4", p.Quantidade);
                    Cmd.Parameters.AddWithValue("@v5", p.PrecoItem);
                    Cmd.Parameters.AddWithValue("@v6", p.CodigoProduto);
                    Cmd.Parameters.AddWithValue("@v7", p.Cpl_DscProduto);
                    Cmd.Parameters.AddWithValue("@v8", p.ValorTotalItem);
                    Cmd.Parameters.AddWithValue("@v9", p.ValorDesconto);
                    Cmd.Parameters.AddWithValue("@v10", p.QuantidadeAtendida);
                    Cmd.Parameters.AddWithValue("@v11", p.Quantidade - p.QuantidadeAtendida);
                    Cmd.Parameters.AddWithValue("@v12", p.CodigoSituacao);
                    Cmd.Parameters.AddWithValue("@v13", p.ValorVolume);
                    Cmd.Parameters.AddWithValue("@v14", p.ValorPeso);
                    Cmd.Parameters.AddWithValue("@v15", p.ValorFatorCubagem);
                    Cmd.ExecuteNonQuery();

                    if (p.Impostos != null)
                    {
                        p.Impostos.CodigoDocumento = CodigoDocumento;

                        ImpostoProdutoDocumentoDAL impDAL = new ImpostoProdutoDocumentoDAL();
                        impDAL.Inserir(p.Impostos);
                    }
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

        public void AtualizarItem(decimal CodigoDocumento, decimal CodigoItem, decimal QuantidadeAtendida, int CodigoSituacao)
        {
            try
            {
                AbrirConexao();

                strSQL = "UPDATE PRODUTO_DO_DOCUMENTO SET QT_ATENDIDA = @v1, CD_SITUACAO = @v4 WHERE CD_DOCUMENTO = @v2 AND CD_PROD_DOCUMENTO = @v3";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", QuantidadeAtendida);
                Cmd.Parameters.AddWithValue("@v2", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v3", CodigoItem);
                Cmd.Parameters.AddWithValue("@v4", CodigoSituacao);

                Cmd.ExecuteNonQuery();
               
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
                            throw new Exception("Erro ao atualizar produtos do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar produtos do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodos(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from PRODUTO_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ", Con);
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
                            throw new Exception("Erro ao excluir produtos do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir produtos do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<ProdutoDocumento> ObterItemOrcamentoPedido(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select PROD.*,TP.DS_TIPO AS DS_SITUACAO "+
                                        "from PRODUTO_DO_DOCUMENTO as PROD "+
                                        "INNER JOIN HABIL_TIPO AS TP ON TP.CD_TIPO = PROD.CD_SITUACAO " +
                                        "Where CD_DOCUMENTO= @v1 ", Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Dr = Cmd.ExecuteReader();
                List<ProdutoDocumento> lista = new List<ProdutoDocumento>();

                while (Dr.Read())
                {
                    ProdutoDocumento p = new ProdutoDocumento();
                    p.CodigoItem = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QT_SOLICITADA"]);
                    p.PrecoItem = Convert.ToDecimal(Dr["VL_ITEM"]);
                    p.Unidade = Dr["DS_UNIDADE"].ToString();
                    p.ValorDesconto = Convert.ToDecimal(Dr["VL_DESCONTO"]);
                    p.ValorTotalItem = Convert.ToDecimal(Dr["VL_TOTAL"]);
                    p.QuantidadeAtendida = Convert.ToDecimal(Dr["QT_ATENDIDA"]);
                    p.Cpl_DscProduto = Dr["DS_PRODUTO"].ToString();
                    p.Cpl_DscSituacao = Dr["DS_SITUACAO"].ToString();
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.QuantidadePendente = Convert.ToDecimal(Dr["QT_PENDENTE"]);
                    p.ValorVolume = Convert.ToDecimal(Dr["VL_VOLUME"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_PESO"]);
                    p.ValorFatorCubagem = Convert.ToDecimal(Dr["VL_FATOR_CUBAGEM"]);

                    if (Dr["CD_PROD_DOCUMENTO"] != DBNull.Value)
                    {
                        p.CodigoItem = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
                    }

                    ImpostoProdutoDocumentoDAL impDAL = new ImpostoProdutoDocumentoDAL();
                    p.Impostos = impDAL.PesquisarImpostoProdutoDocumento(p.CodigoDocumento, p.CodigoItem);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter produtos do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelProdutoDocumento(decimal CodigoDocumento)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "SELECT * FROM PRODUTO_DO_DOCUMENTO WHERE CD_DOCUMENTO = @v1";

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório de produto do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
		public List<ProdutoDocumento> ListarProdutosDaOrdemDeProducao(decimal CodigoDocumento)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand(" select P.*,it.*,TP.DS_TIPO AS DS_SITUACAO " +
					"from PRODUTO_DO_DOCUMENTO as p " +
						 "INNER JOIN HABIL_TIPO AS TP " +
							  "ON TP.CD_TIPO = p.CD_SITUACAO " +
						  "inner join ITEM_DA_COMPOSICAO as it " +
							  "on it.CD_COMPONENTE = p.CD_PRODUTO " +
					  "Where CD_DOCUMENTO = @v1 ", Con);

				Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
				Dr = Cmd.ExecuteReader();
				List<ProdutoDocumento> lista = new List<ProdutoDocumento>();

				while (Dr.Read())
				{
					ProdutoDocumento p = new ProdutoDocumento();

					p.Cpl_CodigoRoteiro = Convert.ToInt16(Dr["CD_ROTEIRO"]);
					p.Cpl_DescRoteiro = Convert.ToString(Dr["DS_ROTEIRO"]);
					p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
					p.Cpl_DscProduto = Dr["DS_PRODUTO"].ToString();
					p.Unidade = Dr["DS_unidade"].ToString();
					p.PerQuebraComponente = Convert.ToDecimal(Dr["vl_pct_quebra"]);
					p.Quantidade = Convert.ToDecimal(Dr["QT_SOLICITADA"]);
					p.QuantidadeAtendida = Convert.ToDecimal(Dr["QT_ATENDIDA"]);
					p.PrecoItem = Convert.ToDecimal(Dr["vl_item"]);
					p.ValorTotalItem = Convert.ToDecimal(Dr["vl_total"]);
					if (Dr["DT_INICIO_ROTEIRO"] != DBNull.Value)
						p.DataInicio = Convert.ToDateTime(Dr["DT_INICIO_ROTEIRO"]);
					if (Dr["DT_FIM_ROTEIRO"] != DBNull.Value)
						p.DataFim = Convert.ToDateTime(Dr["DT_FIM_ROTEIRO"]);
					p.LocalizacoaProducao = Convert.ToInt32(Dr["CD_LOC_PRODUCAO"]);
					p.QuantidadeAtendida = Convert.ToDecimal(Dr["QT_ATENDIDA"]);
					p.QuantidadePendente = Convert.ToDecimal(Dr["QT_PENDENTE"]);
					if (Dr["CD_PROD_DOCUMENTO"] != DBNull.Value)
					{
						p.CodigoItem = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
					}
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Obter produtos da Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<ProdutoDocumento> ObterItemSolCompra(decimal CodigoDocumento)
		{
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select PROD.*,TP.DS_TIPO AS DS_SITUACAO, M.DS_MARCA " +
                                        "from PRODUTO_DO_DOCUMENTO as PROD" +
                                        " INNER JOIN PRODUTO AS P ON P.CD_PRODUTO = PROD.CD_PRODUTO " +
                                        " INNER JOIN MARCA AS M ON P.CD_MARCA = M.CD_MARCA " +
                                        "INNER JOIN HABIL_TIPO AS TP ON TP.CD_TIPO = PROD.CD_SITUACAO " +
                                        "Where CD_DOCUMENTO= @v1 ", Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Dr = Cmd.ExecuteReader();
                List<ProdutoDocumento> lista = new List<ProdutoDocumento>();

                while (Dr.Read())
                {
                    ProdutoDocumento p = new ProdutoDocumento();
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoItem = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
                    p.Cpl_DscProduto = Dr["DS_PRODUTO"].ToString();
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QT_SOLICITADA"]);
                    p.Unidade = Dr["DS_UNIDADE"].ToString();
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DsMarca = Dr["DS_MARCA"].ToString();
                    p.Cpl_DescRoteiro = Dr["OB_PROD_DOCUMENTO"].ToString();
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter produtos do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ProdutoDocumento> ObterItemCotPreco(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select PROD.*,TP.DS_TIPO AS DS_SITUACAO, M.DS_MARCA " +
                                        "from PRODUTO_DO_DOCUMENTO as PROD" +
                                        " INNER JOIN PRODUTO AS P ON P.CD_PRODUTO = PROD.CD_PRODUTO " +
                                        " INNER JOIN MARCA AS M ON P.CD_MARCA = M.CD_MARCA " +
                                        "INNER JOIN HABIL_TIPO AS TP ON TP.CD_TIPO = PROD.CD_SITUACAO " +
                                        "Where CD_DOCUMENTO= @v1 ", Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Dr = Cmd.ExecuteReader();
                List<ProdutoDocumento> lista = new List<ProdutoDocumento>();

                while (Dr.Read())
                {
                    ProdutoDocumento p = new ProdutoDocumento();
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoItem = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
                    p.Cpl_DscProduto = Dr["DS_PRODUTO"].ToString();
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QT_SOLICITADA"]);
                    p.Unidade = Dr["DS_UNIDADE"].ToString();
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DsMarca = Dr["DS_MARCA"].ToString();
                    p.Cpl_DescRoteiro = Dr["OB_PROD_DOCUMENTO"].ToString();
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter produtos do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
