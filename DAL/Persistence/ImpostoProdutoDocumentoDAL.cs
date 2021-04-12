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
    public class ImpostoProdutoDocumentoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(ImpostoProdutoDocumento imp)
        {
            try
            {
                ExcluirTodos(imp.CodigoDocumento, imp.CodigoProdutoDocumento);
                AbrirConexao();

                strSQL = "INSERT INTO [dbo].[IMPOSTO_PRODUTO_DOCUMENTO] (" +
                                "[CD_DOCUMENTO]," +
                                "[CD_PROD_DOCUMENTO]," +
                                "[CD_CST_ICMS]," +
                                "[VL_BC_ICMS]," +
                                "[PC_ICMS]," +
                                "[VL_ICMS]," +
                                "[VL_MVA_ENTRADA]," +
                                "[VL_MVA_SAIDA]" +
                                ",[PC_ICMS_INTERNO] " +
                                ",[VL_RED_BC_ICMS] " +
                                ",[VL_RED_BC_ICMS_INTERNO] " +
                                ",[PC_ICMS_ST], " +
                                "[CD_CST_IPI]," +
                                "[VL_BC_IPI]," +
                                "[PC_IPI]," +
                                "[VL_IPI]," +
                                "[CD_ENQUADRAMENTO_IPI]," +
                                "[CD_CST_PIS]," +
                                "[VL_BC_PIS]," +
                                "[PC_PIS]," +
                                "[VL_PIS]," +
                                "[CD_CST_COFINS]," +
                                "[VL_BC_COFINS]," +
                                "[PC_COFINS]," +
                                "[VL_COFINS]) " +
                        "VALUES (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25)";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", imp.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", imp.CodigoProdutoDocumento);
                Cmd.Parameters.AddWithValue("@v3", imp.CodigoCST_ICMS);
                Cmd.Parameters.AddWithValue("@v4", imp.ValorBaseCalculoICMS);
                Cmd.Parameters.AddWithValue("@v5", imp.PercentualICMS);
                Cmd.Parameters.AddWithValue("@v6", imp.ValorICMS);
                Cmd.Parameters.AddWithValue("@v7", imp.ValorMVA_Entrada);
                Cmd.Parameters.AddWithValue("@v8", imp.ValorMVA_Saida);
                Cmd.Parameters.AddWithValue("@v9", imp.PercentualICMS_Interno);
                Cmd.Parameters.AddWithValue("@v10", imp.ValorReducaoBaseCalculoICMS);
                Cmd.Parameters.AddWithValue("@v11", imp.ValorReducaoBaseCalculoICMS_Interno);
                Cmd.Parameters.AddWithValue("@v12", imp.PercentualICMS_ST);
                Cmd.Parameters.AddWithValue("@v13", imp.CodigoCST_IPI);
                Cmd.Parameters.AddWithValue("@v14", imp.ValorBaseCalculoIPI);
                Cmd.Parameters.AddWithValue("@v15", imp.PercentualIPI);
                Cmd.Parameters.AddWithValue("@v16", imp.ValorIPI);
                Cmd.Parameters.AddWithValue("@v17", imp.CodigoEnquadramento);
                Cmd.Parameters.AddWithValue("@v18", imp.CodigoCST_PIS);
                Cmd.Parameters.AddWithValue("@v19", imp.ValorBaseCalculoPIS);
                Cmd.Parameters.AddWithValue("@v20", imp.PercentualPIS);
                Cmd.Parameters.AddWithValue("@v21", imp.ValorPIS);
                Cmd.Parameters.AddWithValue("@v22", imp.CodigoCST_COFINS);
                Cmd.Parameters.AddWithValue("@v23", imp.ValorBaseCalculoCOFINS);
                Cmd.Parameters.AddWithValue("@v24", imp.PercentualCOFINS);
                Cmd.Parameters.AddWithValue("@v25", imp.ValorCOFINS);
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
                            throw new Exception("Erro ao Incluir imposto do produto do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar imposto do produto do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void ExcluirTodos(decimal CodigoDocumento, int CodigoProdutoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from IMPOSTO_PRODUTO_DOCUMENTO Where CD_DOCUMENTO= @v1 AND CD_PROD_DOCUMENTO = @v2", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", CodigoProdutoDocumento);
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
                            throw new Exception("Erro ao excluir imposto do produto do documento " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir imposto do produto do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public ImpostoProdutoDocumento PesquisarImpostoProdutoDocumento(decimal CodigoDocumento, int CodigoProdutoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("SELECT * FROM [IMPOSTO_PRODUTO_DOCUMENTO] WHERE CD_DOCUMENTO = @v1 AND CD_PROD_DOCUMENTO = @v2", Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", CodigoProdutoDocumento);

                Dr = Cmd.ExecuteReader();
                ImpostoProdutoDocumento p = null;
                if (Dr.Read())
                {
                    p = new ImpostoProdutoDocumento();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoProdutoDocumento = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
                    p.CodigoCST_ICMS =Convert.ToDecimal(Dr["CD_CST_ICMS"]);
                    p.ValorBaseCalculoICMS =Convert.ToDecimal(Dr["VL_BC_ICMS"]);
                    p.PercentualICMS =Convert.ToDecimal(Dr["PC_ICMS"]);
                    p.ValorICMS =Convert.ToDecimal(Dr["VL_ICMS"]);
                    p.ValorMVA_Entrada =Convert.ToDecimal(Dr["VL_MVA_ENTRADA"]);
                    p.ValorMVA_Saida =Convert.ToDecimal(Dr["VL_MVA_SAIDA"]);
                    p.PercentualICMS_Interno = Convert.ToDecimal(Dr["PC_ICMS_INTERNO"]);
                    p.ValorReducaoBaseCalculoICMS = Convert.ToDecimal(Dr["VL_RED_BC_ICMS"]);
                    p.ValorReducaoBaseCalculoICMS = Convert.ToDecimal(Dr["VL_RED_BC_ICMS_INTERNO"]);
                    p.PercentualICMS_ST = Convert.ToDecimal(Dr["PC_ICMS_ST"]);
                    p.CodigoCST_IPI =Convert.ToDecimal(Dr["CD_CST_IPI"]);
                    p.ValorBaseCalculoIPI =Convert.ToDecimal(Dr["VL_BC_IPI"]);
                    p.PercentualIPI = Convert.ToDecimal(Dr["PC_IPI"]);
                    p.ValorIPI =Convert.ToDecimal(Dr["VL_IPI"]);
                    p.CodigoEnquadramento = Convert.ToInt32(Dr["CD_ENQUADRAMENTO_IPI"]);
                    p.CodigoCST_PIS =Convert.ToDecimal(Dr["CD_CST_PIS"]);
                    p.ValorBaseCalculoPIS =Convert.ToDecimal(Dr["VL_BC_PIS"]);
                    p.PercentualPIS = Convert.ToDecimal(Dr["PC_PIS"]);
                    p.ValorPIS =Convert.ToDecimal(Dr["VL_PIS"]);
                    p.CodigoCST_COFINS =Convert.ToDecimal(Dr["CD_CST_COFINS"]);
                    p.ValorBaseCalculoCOFINS =Convert.ToDecimal(Dr["VL_BC_COFINS"]);
                    p.PercentualCOFINS = Convert.ToDecimal(Dr["PC_COFINS"]);
                    p.ValorCOFINS =Convert.ToDecimal(Dr["VL_COFINS"]);

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter imposto do produto do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public static decimal RetornaPercentualICMS(RegFisIcms regICMS)
        {

            if (regICMS.CodHabil_RegTributario != 0)
            {
                if (regICMS.CodHabil_RegTributario == 3)  //REGIME NORMAL
                {
                    switch (regICMS.CodCST_CSOSN)
                    {
                        case 00:
                            return regICMS.CST00ICMS;
                        case 10:
                            return regICMS.CST10ICMS; 
                        case 20:
                            return regICMS.CST20ICMS;
                        case 30:
                            return regICMS.CST30ICMS;
                        case 40:
                            return 0;
                        case 41:
                            return 0;
                        case 50:
                            return 0;
                        case 51:
                            return regICMS.CST51ICMS;
                        case 70:
                            return regICMS.CST70ICMS;
                        case 90:
                            return regICMS.CST90ICMS;
                    }
                }
                else
                {
                    switch (regICMS.CodCST_CSOSN)
                    {
                        case 101:
                            return regICMS.CSOSN101_ICMS_SIMPLES;
                        case 201:
                            return regICMS.CSOSN201_ICMS_SIMPLES;
                        case 202:
                            return regICMS.CSOSN202_203_ICMS_PROPRIO;
                        case 203:
                            return regICMS.CSOSN202_203_ICMS_PROPRIO;
                        case 900:
                            return regICMS.CSOSN900_ICMS_SIMPLES;
                        default:
                            break;
                    }
                }
            }
            return 0;
        }

        public static ImpostoProdutoDocumento PreencherImpostosProdutoDocumento(ProdutoDocumento p, long intCodigoEmpresa, long longCodigoPessoa, int intCodigoTipoOperacao, int intCodigoAplicacaoUso, decimal decValorFrete, bool CalcularImpostos)
        {
            try
            {
                if (intCodigoTipoOperacao == 0)
                    return null;

                ImpostoProdutoDocumento imp = new ImpostoProdutoDocumento();

                PIS pis = new PIS();
                PISDAL pisDAL = new PISDAL();

                COFINS cofins = new COFINS();
                COFINSDAL cofinsDAL = new COFINSDAL();

                RegFisIPI ipi = new RegFisIPI();
                RegraFisIPIDAL ipiDAL = new RegraFisIPIDAL();

                RegFisIcms regICMS = new RegFisIcms();
                RegFisIcmsDAL regICMSDAL = new RegFisIcmsDAL();

                TipoOperacao tpOP = new TipoOperacao();
                TipoOperacaoDAL tpOPDAL = new TipoOperacaoDAL();
                tpOP = tpOPDAL.PesquisarTipoOperacao(intCodigoTipoOperacao);

                Produto produto = new Produto();
                ProdutoDAL produtoDAL = new ProdutoDAL();
                produto = produtoDAL.PesquisarProduto(p.CodigoProduto);

                int CodigoPISUtilizado = 0;
                int CodigoCOFINSUtilizado = 0;

                switch (tpOP.CodigoPrecedenciaImpostoPIS_COFINS)
                {
                    case 159:
                        Pessoa pessoa = new Pessoa();
                        PessoaDAL pessoaDAL = new PessoaDAL();
                        if (produto.CodigoPIS != 0 || produto.CodigoCOFINS != 0)
                            pessoa = pessoaDAL.PesquisarPessoa(longCodigoPessoa);

                        //PIS
                        if (produto.CodigoPIS == 0)//SE NÃO EXISTIR PIS NO PRODUTO 
                            if (pessoa.CodigoPIS == 0)
                                CodigoPISUtilizado = tpOP.CodigoPIS;//SE NÃO EXISTIR PIS NO PESSOA E PRODUTO                                                  
                            else
                                CodigoPISUtilizado = pessoa.CodigoPIS;
                        else
                            CodigoPISUtilizado = produto.CodigoPIS;
                        //FIM PIS

                        //COFINS
                        if (produto.CodigoCOFINS == 0)//SE NÃO EXISTIR COFINS NO PRODUTO
                            if (pessoa.CodigoCOFINS == 0)
                                CodigoCOFINSUtilizado = tpOP.CodigoCOFINS;//SE NÃO EXISTIR COFINS NO PESSOA E PRODUTO
                            else
                                CodigoCOFINSUtilizado = pessoa.CodigoCOFINS;
                        else
                            CodigoCOFINSUtilizado = produto.CodigoCOFINS;
                        //FIM COFINS

                        break;
                    case 160:
                        //PIS
                        //FIM PIS

                        //COFINS
                        //FIM COFINS
                        break;
                }

                if (CodigoPISUtilizado != 0)
                    pis = pisDAL.PesquisarPISIndice(CodigoPISUtilizado);

                if (CodigoCOFINSUtilizado != 0)
                    cofins = cofinsDAL.PesquisarCOFINSIndice(CodigoCOFINSUtilizado);

                regICMS = regICMSDAL.ExecutaSP_BUSCA_REGRA_ICMS(intCodigoTipoOperacao, intCodigoEmpresa, longCodigoPessoa, intCodigoAplicacaoUso);
                
                imp.CodigoDocumento = p.CodigoDocumento;
                imp.CodigoProdutoDocumento = p.CodigoItem;

                if (regICMS != null)
                {
                    imp.ValorMVA_Saida = regICMS.MVAOriginal;
                    imp.ValorMVA_Entrada = regICMS.MVAEntrada;
                    imp.PercentualICMS = RetornaPercentualICMS(regICMS);
                    imp.ValorBaseCalculoICMS = p.ValorTotalItem;
                }

                //testar se exite IPI
                imp.ValorBaseCalculoIPI = p.ValorTotalItem;
                imp.PercentualIPI = 5;

                if (pis != null)
                {
                    imp.ValorBaseCalculoPIS = p.ValorTotalItem;
                    imp.PercentualPIS = Convert.ToDecimal(pis.ValorPIS);
                }

                if (cofins != null)
                {
                    imp.ValorBaseCalculoCOFINS = p.ValorTotalItem;
                    imp.PercentualCOFINS = Convert.ToDecimal(cofins.ValorCOFINS);
                }


                if (CalcularImpostos)
                {
                    imp.ValorICMS = Habil_Impostos.CalcularICMS(p.ValorTotalItem, imp.PercentualICMS, regICMS);
                    imp.ValorIPI = Habil_Impostos.CalcularIPI(p.ValorTotalItem, imp.PercentualIPI);
                    imp.ValorPIS = Habil_Impostos.CalcularPIS(p.ValorTotalItem, imp.PercentualPIS);
                    imp.ValorCOFINS = Habil_Impostos.CalcularCOFINS(p.ValorTotalItem, imp.PercentualCOFINS);
                }

                return imp;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao calcular impostos: " + ex.Message.ToString());
            }
        }
    }
}
