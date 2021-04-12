using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class IntegracaoPedidoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(IntegracaoPedido p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into INTEGRACAO_PEDIDO (NR_INSCRICAO," +
                                                         "NM_TOMADOR," +
                                                         "CD_MUNICIPIO," +
                                                         "CD_CNAE," +
                                                         "DS_SERVICO," +
                                                         "QT_ITEM," +
                                                         "VL_ITEM," +
                                                         "VL_SERVICO," +
                                                         "VL_TOT_NFSE," +
                                                         "VL_ALIQUOTA," +
                                                         "CD_MUN_SERVICO," +
                                                         "CD_SITUACAO," +
                                                         "TX_MSG_RETORNO," +
                                                         "CD_EMPRESA," +
                                                         "CD_DOCUMENTO," +
                                                         "MAIL_NFSE," +
                                                         "CD_CEP," +
                                                         "TX_LOGRADOURO," +
                                                         "NR_ENDERECO," +
                                                         "DS_BAIRRO," +
                                                         "NR_IERG," +
                                                         "DS_NFSE," +
                                                         "CD_SERV_LEI) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23)";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.NumeroInscricao);
                Cmd.Parameters.AddWithValue("@v2", p.NomeTomador);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoCNAE);
                Cmd.Parameters.AddWithValue("@v5", p.DescricaoServico);
                Cmd.Parameters.AddWithValue("@v6", p.Quantidade);
                Cmd.Parameters.AddWithValue("@v7", p.PrecoItem);
                Cmd.Parameters.AddWithValue("@v8", p.ValorTotalServico);
                Cmd.Parameters.AddWithValue("@v9", p.ValorTotalNFSe);
                Cmd.Parameters.AddWithValue("@v10", p.ValorAliquota);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoMunicipioServico);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v13", p.Mensagem);
                Cmd.Parameters.AddWithValue("@v14", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v16", p.Mail_NFSe);
                Cmd.Parameters.AddWithValue("@v17", p.CodigoCEP);
                Cmd.Parameters.AddWithValue("@v18", p.Logradouro);
                Cmd.Parameters.AddWithValue("@v19", p.NumeroEndereco);
                Cmd.Parameters.AddWithValue("@v20", p.DescricaoBairro);
                Cmd.Parameters.AddWithValue("@v21", p.NumeroIERG);
                Cmd.Parameters.AddWithValue("@v22", p.DescricaoNFSE);
                Cmd.Parameters.AddWithValue("@v23", p.CodigoServicoLei);
                Cmd.ExecuteNonQuery();




            }

            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar integracao pedido: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void AtualizarDocumento(decimal CodigoDocumento, decimal CodigoIntegracaoPedido)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_INDEX_INTEGRA_PEDIDO = @v2" +
                                             " Where [CD_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", CodigoIntegracaoPedido);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar integracao pedido: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void AtualizarRetornoPedido(int CodigoSituacao, string strMensagem, decimal CodigoIntePedido, decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [INTEGRACAO_PEDIDO] set CD_SITUACAO = @v2," +
                                                  "TX_MSG_RETORNO = @v3," +
                                                  "CD_DOCUMENTO = @v4" +
                                             " Where [CD_INDEX] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoIntePedido);
                Cmd.Parameters.AddWithValue("@v2", CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", strMensagem);
                Cmd.Parameters.AddWithValue("@v4", CodigoDocumento);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar integracao pedido: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void Atualizar(IntegracaoPedido p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [INTEGRACAO_PEDIDO] set " +
                             "NR_INSCRICAO = @v2, " +
                             "NM_TOMADOR = @v3," +
                             "CD_MUNICIPIO= @v4, " +
                             "CD_CNAE = @v5" +
                             "DS_SERVICO = @v6, " +
                             "QT_ITEM = @v7," +
                             "VL_ITEM = @v8, " +
                             "VL_SERVICO = @v9" +
                             "VL_TOT_NFSE = @v10, " +
                             "VL_ALIQUOTA = @v11," +
                             "CD_MUN_SERVICO = @v12, " +
                             "CD_SITUACAO = @v13" +
                             "TX_MSG_RETORNO = @v14, " +
                             "CD_EMPRESA= @v15" +
                             "CD_DOCUMENTO = @v16," +
                             "MAIL_NFSE = @v17," +
                             "CD_CEP = @v18," +
                             "LOGRADOURO = @v19," +
                             "NR_ENDERECO = @v20," +
                             "DS_BAIRRO = @v21," +
                             "NR_IERG = @v22," +
                             "DS_NFSE = @v23," +
                             "CD_SERV_LEI = @v24" +
                         " Where [CD_INDEX] = @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.Codigo);
                Cmd.Parameters.AddWithValue("@v2", p.NumeroInscricao);
                Cmd.Parameters.AddWithValue("@v3", p.NomeTomador);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoCNAE);
                Cmd.Parameters.AddWithValue("@v6", p.DescricaoServico);
                Cmd.Parameters.AddWithValue("@v7", p.Quantidade);
                Cmd.Parameters.AddWithValue("@v8", p.PrecoItem);
                Cmd.Parameters.AddWithValue("@v9", p.ValorTotalServico);
                Cmd.Parameters.AddWithValue("@v10", p.ValorTotalNFSe);
                Cmd.Parameters.AddWithValue("@v11", p.ValorAliquota);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoMunicipioServico);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v14", p.Mensagem);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v16", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v17", p.Mail_NFSe);
                Cmd.Parameters.AddWithValue("@v18", p.CodigoCEP);
                Cmd.Parameters.AddWithValue("@v19", p.Logradouro);
                Cmd.Parameters.AddWithValue("@v20", p.NumeroEndereco);
                Cmd.Parameters.AddWithValue("@v21", p.DescricaoBairro);
                Cmd.Parameters.AddWithValue("@v22", p.NumeroIERG);
                Cmd.Parameters.AddWithValue("@v23", p.DescricaoNFSE);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoServicoLei);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar integracao pedido: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        
        
        public List<IntegracaoPedido> ListarIntegracaoPedido(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [INTEGRACAO_PEDIDO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<IntegracaoPedido> lista = new List<IntegracaoPedido>();

                while (Dr.Read())
                {
                    IntegracaoPedido p = new IntegracaoPedido();

                    p.Codigo = Convert.ToDecimal(Dr["CD_INDEX"]);
                    p.NumeroInscricao= Convert.ToString(Dr["NR_INSCRICAO"]);
                    p.NomeTomador = Convert.ToString(Dr["NM_TOMADOR"]);
                    p.CodigoMunicipio= Convert.ToDecimal(Dr["CD_MUNICIPIO"]);
                    p.CodigoCNAE= Convert.ToDecimal(Dr["CD_CNAE"]);
                    p.DescricaoServico = Convert.ToString(Dr["DS_SERVICO"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QT_ITEM"]);
                    p.PrecoItem = Convert.ToDecimal(Dr["VL_ITEM"]);
                    p.ValorTotalServico= Convert.ToDecimal(Dr["VL_SERVICO"]);
                    p.ValorTotalNFSe = Convert.ToDecimal(Dr["VL_TOT_NFSE"]);
                    p.ValorAliquota = Convert.ToDecimal(Dr["VL_ALIQUOTA"]);
                    p.CodigoMunicipioServico = Convert.ToString(Dr["CD_MUN_SERVICO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    //p.Mensagem = Convert.ToString(Dr["TX_MSG"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.Mail_NFSe = Convert.ToString(Dr["MAIL_NFSE"]);
                    p.CodigoCEP = Convert.ToInt64(Dr["CD_CEP"]);
                    p.Logradouro = Convert.ToString(Dr["TX_LOGRADOURO"]);
                    p.NumeroEndereco = Convert.ToString(Dr["NR_ENDERECO"]);
                    p.DescricaoBairro = Convert.ToString(Dr["DS_BAIRRO"]);
                    p.NumeroIERG = Convert.ToString(Dr["NR_IERG"]);
                    p.DescricaoNFSE = Convert.ToString(Dr["DS_NFSE"]);
                    p.CodigoServicoLei = Convert.ToDecimal(Dr["CD_SERV_LEI"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos integracao pedido: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        
    }
}

