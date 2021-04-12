using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class BIConsumoClienteProdutoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(BIConsumoClienteProduto p)
        {
            try
            {
                AbrirConexao();

                strSQL = "INSERT INTO [dbo].[BI_CONSUMO_CLIENTE_PRODUTO]" +
                                           " ([DT_ATUALIZACAO]" +
                                           ",[CD_GPO_PESSOA]" +
                                           ",[CD_PESSOA]" +
                                           ",[CD_PRODUTO]" +
                                           ",[DS_PRODUTO]" +
                                           ",[CD_VENDEDOR]" +
                                           ",[VL_MES_1]" +
                                           ",[QT_MES_1]" +
                                           ",[DS_MES_1]" +
                                           ",[VL_MES_2]" +
                                           ",[QT_MES_2]" +
                                           ",[DS_MES_2]" +
                                           ",[VL_MES_3]" +
                                           ",[QT_MES_3]" +
                                           ",[DS_MES_3]" +
                                           ",[VL_MES_4]" +
                                           ",[QT_MES_4]" +
                                           ",[DS_MES_4]" +
                                           ",[VL_MES_5]" +
                                           ",[QT_MES_5]" +
                                           ",[DS_MES_5]" +
                                           ",[VL_MES_6]" +
                                           ",[QT_MES_6]" +
                                           ",[DS_MES_6]" +
                                           ",[QT_MEDIA]" +
                                           ",[NR_PROJECAO]" +
                                           ",[QT_COMPRAR]" +
                                           ",[VL_VENDA]) " +
                                           ",[CD_DEPARTAMENTO]) " +
                                        "VALUES( @v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26,@v27,@v28)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1",p.DataAtualizacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoGrupoPessoa);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v5", p.DescricaoProduto);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoVendedor);
                Cmd.Parameters.AddWithValue("@v7", p.ValorMes1);
                Cmd.Parameters.AddWithValue("@v8", p.QuantidadeMes1);
                Cmd.Parameters.AddWithValue("@v9", p.DescricaoMes1);
                Cmd.Parameters.AddWithValue("@v10", p.ValorMes2);
                Cmd.Parameters.AddWithValue("@v11", p.QuantidadeMes2);
                Cmd.Parameters.AddWithValue("@v12", p.DescricaoMes2);
                Cmd.Parameters.AddWithValue("@v13", p.ValorMes3);
                Cmd.Parameters.AddWithValue("@v14", p.QuantidadeMes3);
                Cmd.Parameters.AddWithValue("@v15", p.DescricaoMes3);
                Cmd.Parameters.AddWithValue("@v16", p.ValorMes4);
                Cmd.Parameters.AddWithValue("@v17", p.QuantidadeMes4);
                Cmd.Parameters.AddWithValue("@v18", p.DescricaoMes4);
                Cmd.Parameters.AddWithValue("@v19", p.ValorMes5);
                Cmd.Parameters.AddWithValue("@v20", p.QuantidadeMes5);
                Cmd.Parameters.AddWithValue("@v21", p.DescricaoMes5);
                Cmd.Parameters.AddWithValue("@v22", p.ValorMes6);
                Cmd.Parameters.AddWithValue("@v23", p.QuantidadeMes6);
                Cmd.Parameters.AddWithValue("@v24", p.DescricaoMes6);
                Cmd.Parameters.AddWithValue("@v25", p.QuantidadeMedia);
                Cmd.Parameters.AddWithValue("@v26", p.NumeroProjecao);
                Cmd.Parameters.AddWithValue("@v27", p.QuantidadeComprar);
                Cmd.Parameters.AddWithValue("@v28", p.PrecoVenda);
                Cmd.Parameters.AddWithValue("@v29", p.CodigoDepartamento);

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
                            throw new Exception("Erro ao Incluir BI consumo pessoa produto: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar BI consumo pessoa produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(BIConsumoClienteProduto p)
        {
            try
            {
                AbrirConexao();
                strSQL = "UPDATE [dbo].[BI_CONSUMO_CLIENTE_PRODUTO]" +
                               "SET[DT_ATUALIZACAO] = @v1" +
                                 ",[CD_GPO_PESSOA] = @v2" +
                                 ",[CD_PESSOA] = @v3" +
                                 ",[CD_PRODUTO] = @v4" +
                                 ",[DS_PRODUTO] = @v5" +
                                 ",[CD_VENDEDOR] = @v6" +
                                 ",[VL_MES_1] = @v7" +
                                 ",[QT_MES_1] = @v8" +
                                 ",[DS_MES_1] = @v9" +
                                 ",[VL_MES_2] = @v10" +
                                 ",[QT_MES_2] = @v11" +
                                 ",[DS_MES_2] = @v12" +
                                 ",[VL_MES_3] = @v13" +
                                 ",[QT_MES_3] = @v14" +
                                 ",[DS_MES_3] = @v15" +
                                 ",[VL_MES_4] = @v16 "+
                                 ",[QT_MES_4] = @v17" +
                                 ",[DS_MES_4] = @v18" +
                                 ",[VL_MES_5] = @v19" +
                                 ",[QT_MES_5] = @v20" +
                                 ",[DS_MES_5] = @v21" +
                                 ",[VL_MES_6] = @v22" +
                                 ",[QT_MES_6] = @v23" +
                                 ",[DS_MES_6] = @v24" +
                                 ",[QT_MEDIA] = @v25" +
                                 ",[NR_PROJECAO] = @v26" +
                                 ",[QT_COMPRAR] = @v27" +
                                 ",[VL_VENDA] = @v28 " +
                                 ",[CD_DEPARTAMENTO] = @v29 WHERE CD_INDEX = @CODIGO";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@CODIGO", p.CodigoIndex);
                Cmd.Parameters.AddWithValue("@v1", p.DataAtualizacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoGrupoPessoa);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v5", p.DescricaoProduto);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoVendedor);
                Cmd.Parameters.AddWithValue("@v7", p.ValorMes1);
                Cmd.Parameters.AddWithValue("@v8", p.QuantidadeMes1);
                Cmd.Parameters.AddWithValue("@v9", p.DescricaoMes1);
                Cmd.Parameters.AddWithValue("@v10", p.ValorMes2);
                Cmd.Parameters.AddWithValue("@v11", p.QuantidadeMes2);
                Cmd.Parameters.AddWithValue("@v12", p.DescricaoMes2);
                Cmd.Parameters.AddWithValue("@v13", p.ValorMes3);
                Cmd.Parameters.AddWithValue("@v14", p.QuantidadeMes3);
                Cmd.Parameters.AddWithValue("@v15", p.DescricaoMes3);
                Cmd.Parameters.AddWithValue("@v16", p.ValorMes4);
                Cmd.Parameters.AddWithValue("@v17", p.QuantidadeMes4);
                Cmd.Parameters.AddWithValue("@v18", p.DescricaoMes4);
                Cmd.Parameters.AddWithValue("@v19", p.ValorMes5);
                Cmd.Parameters.AddWithValue("@v20", p.QuantidadeMes5);
                Cmd.Parameters.AddWithValue("@v21", p.DescricaoMes5);
                Cmd.Parameters.AddWithValue("@v22", p.ValorMes6);
                Cmd.Parameters.AddWithValue("@v23", p.QuantidadeMes6);
                Cmd.Parameters.AddWithValue("@v24", p.DescricaoMes6);
                Cmd.Parameters.AddWithValue("@v25", p.QuantidadeMedia);
                Cmd.Parameters.AddWithValue("@v26", p.NumeroProjecao);
                Cmd.Parameters.AddWithValue("@v27", p.QuantidadeComprar);
                Cmd.Parameters.AddWithValue("@v28", p.PrecoVenda);
                Cmd.Parameters.AddWithValue("@v29", p.CodigoDepartamento);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar BI consumo CLIENTE produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [BI_CONSUMO_CLIENTE_PRODUTO] Where [CD_INDEX] = @v1", Con);

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
                            throw new Exception("Erro ao excluir BI consumo pessoa produto: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir BI consumo pessoa produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public BIConsumoClienteProduto PesquisarBIConsumoPessoaProduto(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [BI_CONSUMO_CLIENTE_PRODUTO] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                BIConsumoClienteProduto p = null;

                if (Dr.Read())
                {
                    p = new BIConsumoClienteProduto();

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoProduto = Convert.ToInt64(Dr["CD_PRODUTO"]);
                    p.DescricaoProduto = Dr["DS_PRODUTO"].ToString();
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.ValorMes1 = Convert.ToDecimal(Dr["VL_MES_1"]);
                    p.QuantidadeMes1 = Convert.ToDecimal(Dr["QT_MES_1"]);
                    p.DescricaoMes1 = Dr["DS_MES_1"].ToString();
                    p.ValorMes2 = Convert.ToDecimal(Dr["VL_MES_2"]);
                    p.QuantidadeMes2 = Convert.ToDecimal(Dr["QT_MES_2"]);
                    p.DescricaoMes2 = Dr["DS_MES_2"].ToString();
                    p.ValorMes3 = Convert.ToDecimal(Dr["VL_MES_3"]);
                    p.QuantidadeMes3 = Convert.ToDecimal(Dr["QT_MES_3"]);
                    p.DescricaoMes3 = Dr["DS_MES_3"].ToString();
                    p.ValorMes4 = Convert.ToDecimal(Dr["VL_MES_4"]);
                    p.QuantidadeMes4 = Convert.ToDecimal(Dr["QT_MES_4"]);
                    p.DescricaoMes4 = Dr["DS_MES_4"].ToString();
                    p.ValorMes5 = Convert.ToDecimal(Dr["VL_MES_5"]);
                    p.QuantidadeMes5 = Convert.ToDecimal(Dr["QT_MES_5"]);
                    p.DescricaoMes5 = Dr["DS_MES_5"].ToString();
                    p.ValorMes6 = Convert.ToDecimal(Dr["VL_MES_6"]);
                    p.QuantidadeMes6 = Convert.ToDecimal(Dr["QT_MES_6"]);
                    p.DescricaoMes6 = Dr["DS_MES_6"].ToString();
                    p.QuantidadeMedia = Convert.ToDecimal(Dr["QT_MEDIA"]);
                    p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
                    p.QuantidadeComprar = Convert.ToDecimal(Dr["QT_COMPRAR"]);
                    p.PrecoVenda = Convert.ToDecimal(Dr["VL_VENDA"]);
                    p.CodigoDepartamento = Convert.ToInt32(Dr["CD_DEPARTAMENTO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar BI_CONSUMO_CLIENTE_PRODUTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<BIConsumoClienteProduto> ListarBIConsumoPessoaProduto(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select  BCCP.*,DEP.DS_DEPARTAMENTO from BI_CONSUMO_CLIENTE_PRODUTO AS BCCP INNER JOIN dbo.DEPARTAMENTO AS DEP ON DEP.CD_DEPARTAMENTO = BCCP.CD_DEPARTAMENTO";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<BIConsumoClienteProduto> lista = new List<BIConsumoClienteProduto>();

                while (Dr.Read())
                {
                    BIConsumoClienteProduto p = new BIConsumoClienteProduto();

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoProduto = Convert.ToInt64(Dr["CD_PRODUTO"]);
                    p.DescricaoProduto = Dr["DS_PRODUTO"].ToString();
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.ValorMes1 = Convert.ToDecimal(Dr["VL_MES_1"]);
                    p.QuantidadeMes1 = Convert.ToDecimal(Dr["QT_MES_1"]);
                    p.DescricaoMes1 = Dr["DS_MES_1"].ToString();
                    p.ValorMes2 = Convert.ToDecimal(Dr["VL_MES_2"]);
                    p.QuantidadeMes2 = Convert.ToDecimal(Dr["QT_MES_2"]);
                    p.DescricaoMes2 = Dr["DS_MES_2"].ToString();
                    p.ValorMes3 = Convert.ToDecimal(Dr["VL_MES_3"]);
                    p.QuantidadeMes3 = Convert.ToDecimal(Dr["QT_MES_3"]);
                    p.DescricaoMes3 = Dr["DS_MES_3"].ToString();
                    p.ValorMes4 = Convert.ToDecimal(Dr["VL_MES_4"]);
                    p.QuantidadeMes4 = Convert.ToDecimal(Dr["QT_MES_4"]);
                    p.DescricaoMes4 = Dr["DS_MES_4"].ToString();
                    p.ValorMes5 = Convert.ToDecimal(Dr["VL_MES_5"]);
                    p.QuantidadeMes5 = Convert.ToDecimal(Dr["QT_MES_5"]);
                    p.DescricaoMes5 = Dr["DS_MES_5"].ToString();
                    p.ValorMes6 = Convert.ToDecimal(Dr["VL_MES_6"]);
                    p.QuantidadeMes6 = Convert.ToDecimal(Dr["QT_MES_6"]);
                    p.DescricaoMes6 = Dr["DS_MES_6"].ToString();
                    p.QuantidadeMedia = Convert.ToDecimal(Dr["QT_MEDIA"]);
                    p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
                    p.QuantidadeComprar = Convert.ToDecimal(Dr["QT_COMPRAR"]);
                    p.PrecoVenda = Convert.ToDecimal(Dr["VL_VENDA"]);
                    p.CodigoDepartamento = Convert.ToInt32(Dr["CD_DEPARTAMENTO"]);
                    p.Cpl_DescricaoDepartamento = Dr["DS_DEPARTAMENTO"].ToString();
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos BI_CONSUMO_PESSOA_PRODUTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
    }
}
