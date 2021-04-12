using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class TipoOperacaoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(TipoOperacao p)
        {
            string strCampos = "";
            string strValores = "";
            try
            {
                AbrirConexao();


                strCampos = ", TX_MSG_ICMS";
                strValores += ", @v11 ";

                strCampos += ", CD_BNF_FISCAL ";
                strValores += ", @v12 ";

                strCampos += ", CD_REG_TRIBUTARIO ";
                strValores += ", @v13 ";

                strCampos += ", CD_CST_CSOSN ";
                strValores += ", @v14 ";

                strCampos += ", CD_MOD_DET_BC_ICMS ";
                strValores += ", @v15 ";

                strCampos += ", CD_MOD_DET_BC_ICMS_ST ";
                strValores += ", @v16 ";

                strCampos += ", VL_MVA_ENTRADA ";
                strValores += ", @v17 ";

                if (p.CodHabil_RegTributario != 0)
                {


                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                    {
                        switch (p.CodCST_CSOSN)
                        {
                            case 00:
                                strCampos += ", CST00_ICMS ";
                                strValores += ", @v18 ";
                                break;
                            case 10:
                                strCampos += ", CST10_RED_BC_ICMS_ST ";
                                strValores += ", @v18 ";
                                strCampos += ", CST10_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CST10_RED_BC_ICMS_PROPRIO ";
                                strValores += ", @v20 ";
                                strCampos += ", CST10_ICMS_PROPRIO ";
                                strValores += ", @v21 ";
                                strCampos += ", CST10_MVA_SAIDA  ";
                                strValores += ", @v22 ";
                                strCampos += ", CST10_DIFAL ";
                                strValores += ", @v23 ";
                                break;
                            case 20:
                                strCampos += ", CST20_RED_BC_ICMS ";
                                strValores += ", @v18 ";
                                strCampos += ", CST20_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CST20_MOT_DESONERACAO ";
                                strValores += ", @v20 ";
                                break;
                            case 30:
                                strCampos += ", CST30_RED_BC_ICMS_ST ";
                                strValores += ", @v18 ";
                                strCampos += ", CST30_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CST30_ICMS_PROPRIO ";
                                strValores += ", @v20 ";
                                strCampos += ", CST30_MVA_SAIDA  ";
                                strValores += ", @v21 ";
                                strCampos += ", CST30_MOT_DESONERACAO ";
                                strValores += ", @v22 ";
                                break;
                            case 40:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO ";
                                strValores += ", @v18 ";
                                break;
                            case 41:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO ";
                                strValores += ", @v18 ";
                                break;
                            case 50:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO ";
                                strValores += ", @v18 ";
                                break;
                            case 51:
                                strCampos += ", CST51_RED_BC_ICMS ";
                                strValores += ", @v18 ";
                                strCampos += ", CST51_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CST51_DIFERIMENTO ";
                                strValores += ", @v20 ";
                                break;
                            case 70:
                                strCampos += ", CST70_RED_BC_ICMS_ST ";
                                strValores += ", @v18 ";
                                strCampos += ", CST70_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CST70_RED_BC_ICMS_PROPRIO ";
                                strValores += ", @v20 ";
                                strCampos += ", CST70_ICMS_PROPRIO ";
                                strValores += ", @v21 ";
                                strCampos += ", CST70_MVA_SAIDA  ";
                                strValores += ", @v22 ";
                                strCampos += ", CST70_MOT_DESONERACAO ";
                                strValores += ", @v23 ";
                                break;
                            case 90:
                                strCampos += ", CST90_RED_BC_ICMS_ST ";
                                strValores += ", @v18 ";
                                strCampos += ", CST90_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CST90_RED_BC_ICMS_PROPRIO ";
                                strValores += ", @v20 ";
                                strCampos += ", CST90_ICMS_PROPRIO ";
                                strValores += ", @v21 ";
                                strCampos += ", CST90_MVA_SAIDA  ";
                                strValores += ", @v22 ";
                                strCampos += ", CST90_DIFAL ";
                                strValores += ", @v23 ";
                                strCampos += ", CST90_MOT_DESONERACAO ";
                                strValores += ", @v24 ";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (p.CodCST_CSOSN)
                        {
                            case 101:
                                strCampos += ", CSOSN101_ICMS_SIMPLES ";
                                strValores += ", @v18 ";
                                break;
                            case 201:
                                strCampos += ", CSOSN201_RED_BC_ICMS_ST ";
                                strValores += ", @v18 ";
                                strCampos += ", CSOSN201_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CSOSN201_MVA_SAIDA ";
                                strValores += ", @v20 ";
                                strCampos += ", CSOSN201_ICMS_SIMPLES ";
                                strValores += ", @v21 ";
                                break;
                            case 202:
                                strCampos += ", CSOSN202_203_RED_BC_ICMS_ST ";
                                strValores += ", @v18 ";
                                strCampos += ", CSOSN202_203_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CSOSN202_203_ICMS_PROPRIO ";
                                strValores += ", @v20 ";
                                strCampos += ", CSOSN202_203_MVA_SAIDA ";
                                strValores += ", @v21 ";
                                break;
                            case 203:
                                strCampos += ", CSOSN202_203_RED_BC_ICMS_ST ";
                                strValores += ", @v18 ";
                                strCampos += ", CSOSN202_203_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CSOSN202_203_ICMS_PROPRIO ";
                                strValores += ", @v20 ";
                                strCampos += ", CSOSN202_203_MVA_SAIDA ";
                                strValores += ", @v21 ";
                                break;
                            case 900:
                                strCampos += ", CSOSN900_RED_BC_ICMS_ST ";
                                strValores += ", @v18 ";
                                strCampos += ", CSOSN900_ICMS ";
                                strValores += ", @v19 ";
                                strCampos += ", CSOSN900_ICMS_PROPRIO ";
                                strValores += ", @v20 ";
                                strCampos += ", CSOSN900_RED_BC_ICMS_PROPRIO ";
                                strValores += ", @v21 ";
                                strCampos += ", CSOSN900_MVA_SAIDA ";
                                strValores += ", @v22 ";
                                strCampos += ", CSOSN900_ICMS_SIMPLES ";
                                strValores += ", @v23 ";
                                break;
                            default:
                                break;
                        }

                    }

                }

                strSQL = "insert into [Tipo_de_Operacao] " +
                    "(DS_TIPO_OPERACAO, CD_SITUACAO, CD_TP_MOVIMENTACAO, CD_TIPO_OP_FISCAL, CD_NAT_OPER_ESTADUAL, CD_NAT_OPER_INTERESTADUAL, " +
                         " CD_NAT_OPER_EXTERIOR, IN_MOV_ESTOQUE, IN_ATU_FINANCEIRO, IN_MOV_INTERNA, IN_BAIXA_FINANCEIRO" + strCampos + ", CD_TP_CTR_PARTIDA, IN_MOV_LOC_ORIGEM_DESTINO," +
                         "CD_PIS,CD_COFINS,CD_PRECEDENCIA_IMPOSTO_ICMS, CD_PRECEDENCIA_IMPOSTO_PIS_COFINS) " +
                         "values (@v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10,@v25 " + strValores + ", @v26, @v27, @v28, @v29, @v30, @v31 ); SELECT SCOPE_IDENTITY()";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.DescricaoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoTipoMovimentacao);
                Cmd.Parameters.AddWithValue("@v4", p.CodTipoOperFiscal);
                Cmd.Parameters.AddWithValue("@v5", p.CodCFOPEstadual);
                Cmd.Parameters.AddWithValue("@v6", p.CodCFOPInterestadual);
                Cmd.Parameters.AddWithValue("@v7", p.CodCFOPExterior);
                Cmd.Parameters.AddWithValue("@v8", p.MovimentaEstoque);
                Cmd.Parameters.AddWithValue("@v9", p.AtualizaFinanceiro);
                Cmd.Parameters.AddWithValue("@v10", p.MovimentacaoInterna);
                Cmd.Parameters.AddWithValue("@v25", p.BaixaFinanceiro);

                Cmd.Parameters.AddWithValue("@v11", p.MensagemIcms);
                Cmd.Parameters.AddWithValue("@v12", p.CodBeneficioFiscal);
                Cmd.Parameters.AddWithValue("@v13", p.CodHabil_RegTributario);
                Cmd.Parameters.AddWithValue("@v14", p.CodCST_CSOSN);
                Cmd.Parameters.AddWithValue("@v15", p.CodModDetBCIcms);
                Cmd.Parameters.AddWithValue("@v16", p.CodModDetBCIcmsST);
                Cmd.Parameters.AddWithValue("@v17", p.MVAEntrada);
                Cmd.Parameters.AddWithValue("@v26", p.CodTipoOperCtPartida);
                Cmd.Parameters.AddWithValue("@v27", p.MovLocOrigemDestino);
                Cmd.Parameters.AddWithValue("@v28", p.CodigoPIS);
                Cmd.Parameters.AddWithValue("@v29", p.CodigoCOFINS);
                Cmd.Parameters.AddWithValue("@v30", p.CodigoPrecedenciaImpostoICMS);
                Cmd.Parameters.AddWithValue("@v31", p.CodigoPrecedenciaImpostoPIS_COFINS);

                if (p.CodHabil_RegTributario != 0)
                {
                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                    {

                        switch (p.CodCST_CSOSN)
                        {
                            case 00:
                                Cmd.Parameters.AddWithValue("@v18", p.CST00ICMS);
                                break;
                            case 10:
                                Cmd.Parameters.AddWithValue("@v18", p.CST10ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v19", p.CST10ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CST10ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v21", p.CST10ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v22", p.CST10MVASaida);
                                Cmd.Parameters.AddWithValue("@v23", p.CST10CalculaDifal);
                                break;
                            case 20:
                                Cmd.Parameters.AddWithValue("@v18", p.CST20ReducaoBCICMS);
                                Cmd.Parameters.AddWithValue("@v19", p.CST20ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CST20MotDesoneracao);
                                break;
                            case 30:
                                Cmd.Parameters.AddWithValue("@v18", p.CST30ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v19", p.CST30ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CST30ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v21", p.CST30MVASaida);
                                Cmd.Parameters.AddWithValue("@v22", p.CST30MotDesoneracao);
                                break;
                            case 40:
                                Cmd.Parameters.AddWithValue("@v18", p.CST404150MotDesoneracao);
                                break;
                            case 41:
                                Cmd.Parameters.AddWithValue("@v18", p.CST404150MotDesoneracao);
                                break;
                            case 50:
                                Cmd.Parameters.AddWithValue("@v18", p.CST404150MotDesoneracao);
                                break;
                            case 51:
                                Cmd.Parameters.AddWithValue("@v18", p.CST51ReducaoBCICMS);
                                Cmd.Parameters.AddWithValue("@v19", p.CST51ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CST51Diferimento);
                                break;
                            case 70:
                                Cmd.Parameters.AddWithValue("@v18", p.CST70ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v19", p.CST70ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CST70ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v21", p.CST70ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v22", p.CST70MVASaida);
                                Cmd.Parameters.AddWithValue("@v23", p.CST70MotDesoneracao);
                                break;
                            case 90:
                                Cmd.Parameters.AddWithValue("@v18", p.CST90ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v19", p.CST90ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CST90ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v21", p.CST90ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v22", p.CST90MVASaida);
                                Cmd.Parameters.AddWithValue("@v23", p.CST90CalculaDifal);
                                Cmd.Parameters.AddWithValue("@v24", p.CST90MotDesoneracao);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (p.CodCST_CSOSN)
                        {
                            case 101:
                                Cmd.Parameters.AddWithValue("@v18", p.CSOSN101_ICMS_SIMPLES);
                                break;
                            case 201:
                                Cmd.Parameters.AddWithValue("@v18", p.CSOSN201_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v19", p.CSOSN201_ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CSOSN201_MVASaida);
                                Cmd.Parameters.AddWithValue("@v21", p.CSOSN201_ICMS_SIMPLES);
                                break;
                            case 202:
                                Cmd.Parameters.AddWithValue("@v18", p.CSOSN202_203_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v19", p.CSOSN202_203_ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CSOSN202_203_ICMS_PROPRIO);
                                Cmd.Parameters.AddWithValue("@v21", p.CSOSN202_203_MVASaida);
                                break;
                            case 203:
                                Cmd.Parameters.AddWithValue("@v18", p.CSOSN202_203_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v19", p.CSOSN202_203_ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CSOSN202_203_ICMS_PROPRIO);
                                Cmd.Parameters.AddWithValue("@v21", p.CSOSN202_203_MVASaida);
                                break;
                            case 900:
                                Cmd.Parameters.AddWithValue("@v18", p.CSOSN900_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v19", p.CSOSN900_ICMS);
                                Cmd.Parameters.AddWithValue("@v20", p.CSOSN900_ICMS_Proprio);
                                Cmd.Parameters.AddWithValue("@v21", p.CSOSN900_ReducaoBCICMSProprio);
                                Cmd.Parameters.AddWithValue("@v22", p.CSOSN900_MVASaida);
                                Cmd.Parameters.AddWithValue("@v23", p.CSOSN900_ICMS_SIMPLES);
                                break;
                            default:
                                break;
                        }
                    }
                }

                p.CodigoTipoOperacao = Convert.ToInt64(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao Incluir Tipo de Operação: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Tipo De Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(TipoOperacao p)
        {
            try
            {
                AbrirConexao();

                string strCampos = "";

                strCampos = ", TX_MSG_ICMS = @v11 ";
                strCampos += ", CD_BNF_FISCAL = @v12 ";
                strCampos += ", CD_REG_TRIBUTARIO = @v13 ";
                strCampos += ", CD_CST_CSOSN = @v14 ";
                strCampos += ", CD_MOD_DET_BC_ICMS = @v15 ";
                strCampos += ", CD_MOD_DET_BC_ICMS_ST = @v16 ";
                strCampos += ", VL_MVA_ENTRADA = @v17 ";

                if (p.CodHabil_RegTributario != 0)
                {

                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                    {
                        switch (p.CodCST_CSOSN)
                        {
                            case 00:
                                strCampos += ", CST00_ICMS = @v18 ";
                                break;
                            case 10:
                                strCampos += ", CST10_RED_BC_ICMS_ST = @v18 ";
                                strCampos += ", CST10_ICMS = @v19 ";
                                strCampos += ", CST10_RED_BC_ICMS_PROPRIO = @v20 ";
                                strCampos += ", CST10_ICMS_PROPRIO = @v21 ";
                                strCampos += ", CST10_MVA_SAIDA = @v22 ";
                                strCampos += ", CST10_DIFAL = @v23 ";
                                break;
                            case 20:
                                strCampos += ", CST20_RED_BC_ICMS = @v18 ";
                                strCampos += ", CST20_ICMS = @v19 ";
                                strCampos += ", CST20_MOT_DESONERACAO = @v20 ";
                                break;
                            case 30:
                                strCampos += ", CST30_RED_BC_ICMS_ST = @v18 ";
                                strCampos += ", CST30_ICMS = @v19 ";
                                strCampos += ", CST30_ICMS_PROPRIO = @v20 ";
                                strCampos += ", CST30_MVA_SAIDA = @v21 ";
                                strCampos += ", CST30_MOT_DESONERACAO = @v22 ";
                                break;
                            case 40:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO = @v18 ";
                                break;
                            case 41:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO = @v18 ";
                                break;
                            case 50:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO = @v18 ";
                                break;
                            case 51:
                                strCampos += ", CST51_RED_BC_ICMS = @v18 ";
                                strCampos += ", CST51_ICMS = @v19 ";
                                strCampos += ", CST51_DIFERIMENTO = @v20 ";
                                break;
                            case 70:
                                strCampos += ", CST70_RED_BC_ICMS_ST = @v18 ";
                                strCampos += ", CST70_ICMS = @v19 ";
                                strCampos += ", CST70_RED_BC_ICMS_PROPRIO = @v20 ";
                                strCampos += ", CST70_ICMS_PROPRIO = @v21 ";
                                strCampos += ", CST70_MVA_SAIDA = @v22 ";
                                strCampos += ", CST70_MOT_DESONERACAO = @v23 ";
                                break;
                            case 90:
                                strCampos += ", CST90_RED_BC_ICMS_ST = @v18 ";
                                strCampos += ", CST90_ICMS = @v19 ";
                                strCampos += ", CST90_RED_BC_ICMS_PROPRIO = @v20 ";
                                strCampos += ", CST90_ICMS_PROPRIO = @v21 ";
                                strCampos += ", CST90_MVA_SAIDA = @v22 ";
                                strCampos += ", CST90_DIFAL = @v23 ";
                                strCampos += ", CST90_MOT_DESONERACAO = @v24 ";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (p.CodCST_CSOSN)
                        {
                            case 101:
                                strCampos += ", CSOSN101_ICMS_SIMPLES = @v18 ";
                                break;
                            case 201:
                                strCampos += ", CSOSN201_RED_BC_ICMS_ST = @v18 ";
                                strCampos += ", CSOSN201_ICMS = @v19 ";
                                strCampos += ", CSOSN201_MVA_SAIDA = @v20 ";
                                strCampos += ", CSOSN201_ICMS_SIMPLES = @v21 ";
                                break;
                            case 202:
                                strCampos += ", CSOSN202_203_RED_BC_ICMS_ST = @v18 ";
                                strCampos += ", CSOSN202_203_ICMS = @v19 ";
                                strCampos += ", CSOSN202_203_ICMS_PROPRIO = @v20 ";
                                strCampos += ", CSOSN202_203_MVA_SAIDA = @v21 ";
                                break;
                            case 203:
                                strCampos += ", CSOSN202_203_RED_BC_ICMS_ST = @v18 ";
                                strCampos += ", CSOSN202_203_ICMS = @v19 ";
                                strCampos += ", CSOSN202_203_ICMS_PROPRIO = @v20 ";
                                strCampos += ", CSOSN202_203_MVA_SAIDA = @v21 ";
                                break;
                            case 900:
                                strCampos += ", CSOSN900_RED_BC_ICMS_ST = @v18 ";
                                strCampos += ", CSOSN900_ICMS = @v19 ";
                                strCampos += ", CSOSN900_ICMS_PROPRIO = @v20 ";
                                strCampos += ", CSOSN900_RED_BC_ICMS_PROPRIO = @v21 ";
                                strCampos += ", CSOSN900_MVA_SAIDA = @v22 ";
                                strCampos += ", CSOSN900_ICMS_SIMPLES = @v23 ";
                                break;
                            default:
                                break;
                        }

                    }

                }

                strSQL = "update [Tipo_de_Operacao] set DS_TIPO_OPERACAO = @v1, CD_SITUACAO = @v2, CD_TP_MOVIMENTACAO = @v3, CD_TIPO_OP_FISCAL = @v4, " +
                    "CD_NAT_OPER_ESTADUAL = @v5, CD_NAT_OPER_INTERESTADUAL = @v6, CD_NAT_OPER_EXTERIOR = @v7, IN_MOV_ESTOQUE = @v8, IN_ATU_FINANCEIRO = @v9, " +
                    "IN_MOV_INTERNA = @v10, IN_BAIXA_FINANCEIRO = @v26, CD_TP_CTR_PARTIDA = @v27, IN_MOV_LOC_ORIGEM_DESTINO = @v28, CD_PIS = @v29, CD_COFINS = @v30," +
                    "CD_PRECEDENCIA_IMPOSTO_ICMS = @v31, CD_PRECEDENCIA_IMPOSTO_PIS_COFINS = @v32 " + strCampos + " Where [CD_TIPO_OPERACAO] = @v1000";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1000", p.CodigoTipoOperacao);

                Cmd.Parameters.AddWithValue("@v1", p.DescricaoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoTipoMovimentacao);
                Cmd.Parameters.AddWithValue("@v4", p.CodTipoOperFiscal);
                Cmd.Parameters.AddWithValue("@v5", p.CodCFOPEstadual);
                Cmd.Parameters.AddWithValue("@v6", p.CodCFOPInterestadual);
                Cmd.Parameters.AddWithValue("@v7", p.CodCFOPExterior);
                Cmd.Parameters.AddWithValue("@v8", p.MovimentaEstoque);
                Cmd.Parameters.AddWithValue("@v9", p.AtualizaFinanceiro);
                Cmd.Parameters.AddWithValue("@v10", p.MovimentacaoInterna);
                Cmd.Parameters.AddWithValue("@v26", p.BaixaFinanceiro);
                Cmd.Parameters.AddWithValue("@v27", p.CodTipoOperCtPartida);
                Cmd.Parameters.AddWithValue("@v28", p.MovLocOrigemDestino);
                Cmd.Parameters.AddWithValue("@v29", p.CodigoPIS);
                Cmd.Parameters.AddWithValue("@v30", p.CodigoCOFINS);
                Cmd.Parameters.AddWithValue("@v31", p.CodigoPrecedenciaImpostoICMS);
                Cmd.Parameters.AddWithValue("@v32", p.CodigoPrecedenciaImpostoPIS_COFINS);

                Cmd.Parameters.AddWithValue("@v11", p.MensagemIcms);
                Cmd.Parameters.AddWithValue("@v12", p.CodBeneficioFiscal);
                Cmd.Parameters.AddWithValue("@v13", p.CodHabil_RegTributario);
                Cmd.Parameters.AddWithValue("@v14", p.CodCST_CSOSN);
                Cmd.Parameters.AddWithValue("@v15", p.CodModDetBCIcms);
                Cmd.Parameters.AddWithValue("@v16", p.CodModDetBCIcmsST);
                Cmd.Parameters.AddWithValue("@v17", p.MVAEntrada);

                if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                {

                    switch (p.CodCST_CSOSN)
                    {
                        case 00:
                            Cmd.Parameters.AddWithValue("@v18", p.CST00ICMS);
                            break;
                        case 10:
                            Cmd.Parameters.AddWithValue("@v18", p.CST10ReducaoBCICMSST);
                            Cmd.Parameters.AddWithValue("@v19", p.CST10ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CST10ReducaoBCICMSSTProprio);
                            Cmd.Parameters.AddWithValue("@v21", p.CST10ICMSProprio);
                            Cmd.Parameters.AddWithValue("@v22", p.CST10MVASaida);
                            Cmd.Parameters.AddWithValue("@v23", p.CST10CalculaDifal);
                            break;
                        case 20:
                            Cmd.Parameters.AddWithValue("@v18", p.CST20ReducaoBCICMS);
                            Cmd.Parameters.AddWithValue("@v19", p.CST20ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CST20MotDesoneracao);
                            break;
                        case 30:
                            Cmd.Parameters.AddWithValue("@v18", p.CST30ReducaoBCICMSST);
                            Cmd.Parameters.AddWithValue("@v19", p.CST30ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CST30ICMSProprio);
                            Cmd.Parameters.AddWithValue("@v21", p.CST30MVASaida);
                            Cmd.Parameters.AddWithValue("@v22", p.CST30MotDesoneracao);
                            break;
                        case 40:
                            Cmd.Parameters.AddWithValue("@v18", p.CST404150MotDesoneracao);
                            break;
                        case 41:
                            Cmd.Parameters.AddWithValue("@v18", p.CST404150MotDesoneracao);
                            break;
                        case 42:
                            Cmd.Parameters.AddWithValue("@v18", p.CST404150MotDesoneracao);
                            break;
                        case 51:
                            Cmd.Parameters.AddWithValue("@v18", p.CST51ReducaoBCICMS);
                            Cmd.Parameters.AddWithValue("@v19", p.CST51ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CST51Diferimento);
                            break;
                        case 70:
                            Cmd.Parameters.AddWithValue("@v18", p.CST70ReducaoBCICMSST);
                            Cmd.Parameters.AddWithValue("@v19", p.CST70ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CST70ReducaoBCICMSSTProprio);
                            Cmd.Parameters.AddWithValue("@v21", p.CST70ICMSProprio);
                            Cmd.Parameters.AddWithValue("@v22", p.CST70MVASaida);
                            Cmd.Parameters.AddWithValue("@v23", p.CST70MotDesoneracao);
                            break;
                        case 90:
                            Cmd.Parameters.AddWithValue("@v18", p.CST90ReducaoBCICMSST);
                            Cmd.Parameters.AddWithValue("@v19", p.CST90ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CST90ReducaoBCICMSSTProprio);
                            Cmd.Parameters.AddWithValue("@v21", p.CST90ICMSProprio);
                            Cmd.Parameters.AddWithValue("@v22", p.CST90MVASaida);
                            Cmd.Parameters.AddWithValue("@v23", p.CST90CalculaDifal);
                            Cmd.Parameters.AddWithValue("@v24", p.CST90MotDesoneracao);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (p.CodCST_CSOSN)
                    {
                        case 101:
                            Cmd.Parameters.AddWithValue("@v18", p.CSOSN101_ICMS_SIMPLES);
                            break;
                        case 201:
                            Cmd.Parameters.AddWithValue("@v18", p.CSOSN201_ReducaoBCICMSST);
                            Cmd.Parameters.AddWithValue("@v19", p.CSOSN201_ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CSOSN201_MVASaida);
                            Cmd.Parameters.AddWithValue("@v21", p.CSOSN201_ICMS_SIMPLES);
                            break;
                        case 202:
                            Cmd.Parameters.AddWithValue("@v18", p.CSOSN202_203_ReducaoBCICMSST);
                            Cmd.Parameters.AddWithValue("@v19", p.CSOSN202_203_ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CSOSN202_203_ICMS_PROPRIO);
                            Cmd.Parameters.AddWithValue("@v21", p.CSOSN202_203_MVASaida);
                            break;
                        case 203:
                            Cmd.Parameters.AddWithValue("@v18", p.CSOSN202_203_ReducaoBCICMSST);
                            Cmd.Parameters.AddWithValue("@v19", p.CSOSN202_203_ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CSOSN202_203_ICMS_PROPRIO);
                            Cmd.Parameters.AddWithValue("@v21", p.CSOSN202_203_MVASaida);
                            break;
                        case 900:
                            Cmd.Parameters.AddWithValue("@v18", p.CSOSN900_ReducaoBCICMSST);
                            Cmd.Parameters.AddWithValue("@v19", p.CSOSN900_ICMS);
                            Cmd.Parameters.AddWithValue("@v20", p.CSOSN900_ICMS_Proprio);
                            Cmd.Parameters.AddWithValue("@v21", p.CSOSN900_ReducaoBCICMSProprio);
                            Cmd.Parameters.AddWithValue("@v22", p.CSOSN900_MVASaida);
                            Cmd.Parameters.AddWithValue("@v23", p.CSOSN900_ICMS_SIMPLES);
                            Cmd.Parameters.AddWithValue("@v24", p.CSOSN900_ICMS_Proprio);
                            Cmd.Parameters.AddWithValue("@v25", p.CSOSN900_ICMS_Proprio);
                            break;
                        default:
                            break;
                    }
                }

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Tipo De Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(long Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [Tipo_de_Operacao] Where [CD_TIPO_OPERACAO] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Tipo de Operação: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Tipo de Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public TipoOperacao PesquisarTipoOperacao(long intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Tipo_de_Operacao] Where CD_TIPO_OPERACAO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                TipoOperacao p = null;

                if (Dr.Read())
                {
                    p = new TipoOperacao();

                    p.CodigoTipoOperacao = Convert.ToInt64(Dr["CD_TIPO_OPERACAO"]);
                    p.DescricaoTipoOperacao = Convert.ToString(Dr["DS_TIPO_OPERACAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoMovimentacao = Convert.ToInt32(Dr["CD_TP_MOVIMENTACAO"]);
                    p.CodTipoOperFiscal = Convert.ToInt32(Dr["CD_TIPO_OP_FISCAL"]);
                    p.CodCFOPEstadual = Convert.ToInt64(Dr["CD_NAT_OPER_ESTADUAL"]);
                    p.CodCFOPInterestadual = Convert.ToInt64(Dr["CD_NAT_OPER_INTERESTADUAL"]);
                    p.CodCFOPExterior = Convert.ToInt64(Dr["CD_NAT_OPER_EXTERIOR"]);
                    p.MovimentaEstoque = Convert.ToBoolean(Dr["IN_MOV_ESTOQUE"]);
                    p.AtualizaFinanceiro = Convert.ToBoolean(Dr["IN_ATU_FINANCEIRO"]);
                    p.MovimentacaoInterna = Convert.ToBoolean(Dr["IN_MOV_INTERNA"]);
                    p.BaixaFinanceiro = Convert.ToBoolean(Dr["IN_BAIXA_FINANCEIRO"]);
                    p.MovLocOrigemDestino = Convert.ToBoolean(Dr["IN_MOV_LOC_ORIGEM_DESTINO"]);
                    p.CodTipoOperCtPartida = Convert.ToInt32(Dr["CD_TP_CTR_PARTIDA"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.CodigoPrecedenciaImpostoICMS = Convert.ToInt32(Dr["CD_PRECEDENCIA_IMPOSTO_ICMS"]);
                    p.CodigoPrecedenciaImpostoPIS_COFINS = Convert.ToInt32(Dr["CD_PRECEDENCIA_IMPOSTO_PIS_COFINS"]);

                    p.MensagemIcms = Convert.ToString(Dr["TX_MSG_ICMS"]);
                    p.CodBeneficioFiscal = Convert.ToString(Dr["CD_BNF_FISCAL"]);
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.CodCST_CSOSN = Convert.ToInt32(Dr["CD_CST_CSOSN"]);
                    p.CodModDetBCIcms = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS"]);
                    p.CodModDetBCIcmsST = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS_ST"]);
                    p.MVAEntrada = Convert.ToDecimal(Dr["VL_MVA_ENTRADA"]);
                    p.CST10CalculaDifal = Convert.ToBoolean(Dr["CST10_DIFAL"]);
                    p.CST90CalculaDifal = Convert.ToBoolean(Dr["CST90_DIFAL"]);

                    if (p.CodHabil_RegTributario != 0)
                    {
                        if (Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]) == 3)  //REGIME NORMAL
                        {
                            switch (p.CodCST_CSOSN)
                            {
                                case 00:
                                    p.CST00ICMS = Convert.ToDecimal(Dr["CST00_ICMS"]);
                                    break;
                                case 10:
                                    p.CST10ReducaoBCICMSST = Convert.ToDecimal(Dr["CST10_RED_BC_ICMS_ST"]);
                                    p.CST10ICMS = Convert.ToDecimal(Dr["CST10_ICMS"]);
                                    p.CST10ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST10_RED_BC_ICMS_PROPRIO"]);
                                    p.CST10ICMSProprio = Convert.ToDecimal(Dr["CST10_ICMS_PROPRIO"]);
                                    p.CST10MVASaida = Convert.ToDecimal(Dr["CST10_MVA_SAIDA"]);
                                    p.CST10CalculaDifal = Convert.ToBoolean(Dr["CST10_DIFAL"]);
                                    break;
                                case 20:
                                    p.CST20ReducaoBCICMS = Convert.ToDecimal(Dr["CST20_RED_BC_ICMS"]);
                                    p.CST20ICMS = Convert.ToDecimal(Dr["CST20_ICMS"]);
                                    p.CST20MotDesoneracao = Convert.ToInt32(Dr["CST20_MOT_DESONERACAO"]);
                                    break;
                                case 30:
                                    p.CST30ReducaoBCICMSST = Convert.ToDecimal(Dr["CST30_RED_BC_ICMS_ST"]);
                                    p.CST30ICMS = Convert.ToDecimal(Dr["CST30_ICMS"]);
                                    p.CST30ICMSProprio = Convert.ToDecimal(Dr["CST30_ICMS_PROPRIO"]);
                                    p.CST30MVASaida = Convert.ToDecimal(Dr["CST30_MVA_SAIDA"]);
                                    p.CST30MotDesoneracao = Convert.ToInt32(Dr["CST30_MOT_DESONERACAO"]);
                                    break;
                                case 40:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 41:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 50:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 51:
                                    p.CST51ReducaoBCICMS = Convert.ToDecimal(Dr["CST51_RED_BC_ICMS"]);
                                    p.CST51ICMS = Convert.ToDecimal(Dr["CST51_ICMS"]);
                                    p.CST51Diferimento = Convert.ToDecimal(Dr["CST51_DIFERIMENTO"]);
                                    break;
                                case 70:
                                    p.CST70ReducaoBCICMSST = Convert.ToDecimal(Dr["CST70_RED_BC_ICMS_ST"]);
                                    p.CST70ICMS = Convert.ToDecimal(Dr["CST70_ICMS"]);
                                    p.CST70ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST70_RED_BC_ICMS_PROPRIO"]);
                                    p.CST70ICMSProprio = Convert.ToDecimal(Dr["CST70_ICMS_PROPRIO"]);
                                    p.CST70MVASaida = Convert.ToDecimal(Dr["CST70_MVA_SAIDA"]);
                                    p.CST70MotDesoneracao = Convert.ToInt32(Dr["CST70_MOT_DESONERACAO"]);
                                    break;
                                case 90:
                                    p.CST90ReducaoBCICMSST = Convert.ToDecimal(Dr["CST90_ICMS"]);
                                    p.CST90ICMS = Convert.ToDecimal(Dr["CST90_ICMS"]);
                                    p.CST90ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST90_RED_BC_ICMS_PROPRIO"]);
                                    p.CST90ICMSProprio = Convert.ToDecimal(Dr["CST90_ICMS_PROPRIO"]);
                                    p.CST90MVASaida = Convert.ToDecimal(Dr["CST90_MVA_SAIDA"]);
                                    p.CST90CalculaDifal = Convert.ToBoolean(Dr["CST90_DIFAL"]);
                                    p.CST90MotDesoneracao = Convert.ToInt32(Dr["CST90_MOT_DESONERACAO"]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (p.CodCST_CSOSN)
                            {

                                case 101:
                                    p.CSOSN101_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN101_ICMS_SIMPLES"]);
                                    break;
                                case 201:
                                    p.CSOSN201_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN201_RED_BC_ICMS_ST"]);
                                    p.CSOSN201_ICMS = Convert.ToDecimal(Dr["CSOSN201_ICMS"]);
                                    p.CSOSN201_MVASaida = Convert.ToDecimal(Dr["CSOSN201_MVA_SAIDA"]);
                                    p.CSOSN201_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN201_ICMS_SIMPLES"]);
                                    break;
                                case 202:
                                    p.CSOSN202_203_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN202_203_RED_BC_ICMS_ST"]);
                                    p.CSOSN202_203_ICMS = Convert.ToDecimal(Dr["CSOSN202_203_ICMS"]);
                                    p.CSOSN202_203_ICMS_PROPRIO = Convert.ToDecimal(Dr["CSOSN202_203_ICMS_PROPRIO"]);
                                    p.CSOSN202_203_MVASaida = Convert.ToDecimal(Dr["CSOSN202_203_MVA_SAIDA"]);
                                    break;
                                case 203:
                                    p.CSOSN202_203_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN202_203_RED_BC_ICMS_ST"]);
                                    p.CSOSN202_203_ICMS = Convert.ToDecimal(Dr["CSOSN202_203_ICMS"]);
                                    p.CSOSN202_203_ICMS_PROPRIO = Convert.ToDecimal(Dr["CSOSN202_203_ICMS_PROPRIO"]);
                                    p.CSOSN202_203_MVASaida = Convert.ToDecimal(Dr["CSOSN202_203_MVA_SAIDA"]);
                                    break;
                                case 900:
                                    p.CSOSN900_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN900_RED_BC_ICMS_ST"]);
                                    p.CSOSN900_ICMS = Convert.ToDecimal(Dr["CSOSN900_ICMS"]);
                                    p.CSOSN900_ICMS_Proprio = Convert.ToDecimal(Dr["CSOSN900_ICMS_PROPRIO"]);
                                    p.CSOSN900_ReducaoBCICMSProprio = Convert.ToDecimal(Dr["CSOSN900_RED_BC_ICMS_PROPRIO"]);
                                    p.CSOSN900_MVASaida = Convert.ToDecimal(Dr["CSOSN900_MVA_SAIDA"]);
                                    p.CSOSN900_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN900_ICMS_SIMPLES"]);
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tipo De Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<TipoOperacao> ListarTipoOperacoes(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Tipo_de_Operacao]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoOperacao> lista = new List<TipoOperacao>();

                while (Dr.Read())
                {
                    TipoOperacao p = new TipoOperacao();

                    p.CodigoTipoOperacao = Convert.ToInt64(Dr["CD_TIPO_OPERACAO"]);
                    p.DescricaoTipoOperacao = Convert.ToString(Dr["DS_TIPO_OPERACAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoMovimentacao = Convert.ToInt32(Dr["CD_TP_MOVIMENTACAO"]);
                    p.CodTipoOperFiscal = Convert.ToInt32(Dr["CD_TIPO_OP_FISCAL"]);
                    p.CodCFOPEstadual = Convert.ToInt64(Dr["CD_NAT_OPER_ESTADUAL"]);
                    p.CodCFOPInterestadual = Convert.ToInt64(Dr["CD_NAT_OPER_INTERESTADUAL"]);
                    p.CodCFOPExterior = Convert.ToInt64(Dr["CD_NAT_OPER_EXTERIOR"]);
                    p.MovimentaEstoque = Convert.ToBoolean(Dr["IN_MOV_ESTOQUE"]);
                    p.AtualizaFinanceiro = Convert.ToBoolean(Dr["IN_ATU_FINANCEIRO"]);
                    p.MovimentacaoInterna = Convert.ToBoolean(Dr["IN_MOV_INTERNA"]);
                    p.BaixaFinanceiro = Convert.ToBoolean(Dr["IN_BAIXA_FINANCEIRO"]);
                    p.MensagemIcms = Convert.ToString(Dr["TX_MSG_ICMS"]);
                    p.CodBeneficioFiscal = Convert.ToString(Dr["CD_BNF_FISCAL"]);
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.CodCST_CSOSN = Convert.ToInt32(Dr["CD_CST_CSOSN"]);
                    p.CodModDetBCIcms = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS"]);
                    p.CodModDetBCIcmsST = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS_ST"]);
                    p.MVAEntrada = Convert.ToDecimal(Dr["VL_MVA_ENTRADA"]);
                    p.CST10CalculaDifal = Convert.ToBoolean(Dr["CST10_DIFAL"]);
                    p.CST90CalculaDifal = Convert.ToBoolean(Dr["CST90_DIFAL"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.CodigoPrecedenciaImpostoICMS = Convert.ToInt32(Dr["CD_PRECEDENCIA_IMPOSTO_ICMS"]);
                    p.CodigoPrecedenciaImpostoPIS_COFINS = Convert.ToInt32(Dr["CD_PRECEDENCIA_IMPOSTO_PIS_COFINS"]);
                    p.Cpl_ComboDescricaoTipoOperacao = p.CodigoTipoOperacao + " | " + p.DescricaoTipoOperacao;

                    if (p.CodHabil_RegTributario != 0)
                    {
                        if (Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]) == 3)  //REGIME NORMAL
                        {
                            switch (p.CodCST_CSOSN)
                            {
                                case 00:
                                    p.CST00ICMS = Convert.ToDecimal(Dr["CST00_ICMS"]);
                                    break;
                                case 10:
                                    p.CST10ReducaoBCICMSST = Convert.ToDecimal(Dr["CST10_RED_BC_ICMS_ST"]);
                                    p.CST10ICMS = Convert.ToDecimal(Dr["CST10_ICMS"]);
                                    p.CST10ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST10_RED_BC_ICMS_PROPRIO"]);
                                    p.CST10ICMSProprio = Convert.ToDecimal(Dr["CST10_ICMS_PROPRIO"]);
                                    p.CST10MVASaida = Convert.ToDecimal(Dr["CST10_MVA_SAIDA"]);
                                    p.CST10CalculaDifal = Convert.ToBoolean(Dr["CST10_DIFAL"]);
                                    break;
                                case 20:
                                    p.CST20ReducaoBCICMS = Convert.ToDecimal(Dr["CST20_RED_BC_ICMS"]);
                                    p.CST20ICMS = Convert.ToDecimal(Dr["CST20_ICMS"]);
                                    p.CST20MotDesoneracao = Convert.ToInt32(Dr["CST20_MOT_DESONERACAO"]);
                                    break;
                                case 30:
                                    p.CST30ReducaoBCICMSST = Convert.ToDecimal(Dr["CST30_RED_BC_ICMS_ST"]);
                                    p.CST30ICMS = Convert.ToDecimal(Dr["CST30_ICMS"]);
                                    p.CST30ICMSProprio = Convert.ToDecimal(Dr["CST30_ICMS_PROPRIO"]);
                                    p.CST30MVASaida = Convert.ToDecimal(Dr["CST30_MVA_SAIDA"]);
                                    p.CST30MotDesoneracao = Convert.ToInt32(Dr["CST30_MOT_DESONERACAO"]);
                                    break;
                                case 40:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 41:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 50:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 51:
                                    p.CST51ReducaoBCICMS = Convert.ToDecimal(Dr["CST51_RED_BC_ICMS"]);
                                    p.CST51ICMS = Convert.ToDecimal(Dr["CST51_ICMS"]);
                                    p.CST51Diferimento = Convert.ToDecimal(Dr["CST51_DIFERIMENTO"]);
                                    break;
                                case 70:
                                    p.CST70ReducaoBCICMSST = Convert.ToDecimal(Dr["CST70_RED_BC_ICMS_ST"]);
                                    p.CST70ICMS = Convert.ToDecimal(Dr["CST70_ICMS"]);
                                    p.CST70ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST70_RED_BC_ICMS_PROPRIO"]);
                                    p.CST70ICMSProprio = Convert.ToDecimal(Dr["CST70_ICMS_PROPRIO"]);
                                    p.CST70MVASaida = Convert.ToDecimal(Dr["CST70_MVA_SAIDA"]);
                                    p.CST70MotDesoneracao = Convert.ToInt32(Dr["CST70_MOT_DESONERACAO"]);
                                    break;
                                case 90:
                                    p.CST90ReducaoBCICMSST = Convert.ToDecimal(Dr["CST90_ICMS"]);
                                    p.CST90ICMS = Convert.ToDecimal(Dr["CST90_ICMS"]);
                                    p.CST90ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST90_RED_BC_ICMS_PROPRIO"]);
                                    p.CST90ICMSProprio = Convert.ToDecimal(Dr["CST90_ICMS_PROPRIO"]);
                                    p.CST90MVASaida = Convert.ToDecimal(Dr["CST90_MVA_SAIDA"]);
                                    p.CST90CalculaDifal = Convert.ToBoolean(Dr["CST90_DIFAL"]);
                                    p.CST90MotDesoneracao = Convert.ToInt32(Dr["CST90_MOT_DESONERACAO"]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (p.CodCST_CSOSN)
                            {

                                case 101:
                                    p.CSOSN101_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN101_ICMS_SIMPLES"]);
                                    break;
                                case 201:
                                    p.CSOSN201_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN201_RED_BC_ICMS_ST"]);
                                    p.CSOSN201_ICMS = Convert.ToDecimal(Dr["CSOSN201_ICMS"]);
                                    p.CSOSN201_MVASaida = Convert.ToDecimal(Dr["CSOSN201_MVA_SAIDA"]);
                                    p.CSOSN201_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN201_ICMS_SIMPLES"]);
                                    break;
                                case 202:
                                    p.CSOSN202_203_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN202_203_RED_BC_ICMS_ST"]);
                                    p.CSOSN202_203_ICMS = Convert.ToDecimal(Dr["CSOSN202_203_ICMS"]);
                                    p.CSOSN202_203_ICMS_PROPRIO = Convert.ToDecimal(Dr["CSOSN202_203_ICMS_PROPRIO"]);
                                    p.CSOSN202_203_MVASaida = Convert.ToDecimal(Dr["CSOSN202_203_MVA_SAIDA"]);
                                    break;
                                case 203:
                                    p.CSOSN202_203_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN202_203_RED_BC_ICMS_ST"]);
                                    p.CSOSN202_203_ICMS = Convert.ToDecimal(Dr["CSOSN202_203_ICMS"]);
                                    p.CSOSN202_203_ICMS_PROPRIO = Convert.ToDecimal(Dr["CSOSN202_203_ICMS_PROPRIO"]);
                                    p.CSOSN202_203_MVASaida = Convert.ToDecimal(Dr["CSOSN202_203_MVA_SAIDA"]);
                                    break;
                                case 900:
                                    p.CSOSN900_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN900_RED_BC_ICMS_ST"]);
                                    p.CSOSN900_ICMS = Convert.ToDecimal(Dr["CSOSN900_ICMS"]);
                                    p.CSOSN900_ICMS_Proprio = Convert.ToDecimal(Dr["CSOSN900_ICMS_PROPRIO"]);
                                    p.CSOSN900_ReducaoBCICMSProprio = Convert.ToDecimal(Dr["CSOSN900_RED_BC_ICMS_PROPRIO"]);
                                    p.CSOSN900_MVASaida = Convert.ToDecimal(Dr["CSOSN900_MVA_SAIDA"]);
                                    p.CSOSN900_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN900_ICMS_SIMPLES"]);
                                    break;
                                default:
                                    break;
                            }
                        }

                        lista.Add(p);
                    }
                    else
                        lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Tipos de Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public List<TipoOperacao> ListarTipoOperacoesCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VW_TIPO_OPERACAO]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;



                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoOperacao> lista = new List<TipoOperacao>();

                while (Dr.Read())
                {
                    TipoOperacao p = new TipoOperacao();
                    p.CodigoTipoOperacao = Convert.ToInt64(Dr["CD_TIPO_OPERACAO"]);
                    p.DescricaoTipoOperacao = Convert.ToString(Dr["DS_TIPO_OPERACAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoMovimentacao = Convert.ToInt32(Dr["CD_TP_MOVIMENTACAO"]);
                    p.CodTipoOperFiscal = Convert.ToInt32(Dr["CD_TIPO_OP_FISCAL"]);
                    p.CodCFOPEstadual = Convert.ToInt64(Dr["CD_NAT_OPER_ESTADUAL"]);
                    p.CodCFOPInterestadual = Convert.ToInt64(Dr["CD_NAT_OPER_INTERESTADUAL"]);
                    p.CodCFOPExterior = Convert.ToInt64(Dr["CD_NAT_OPER_EXTERIOR"]);
                    p.MovimentaEstoque = Convert.ToBoolean(Dr["IN_MOV_ESTOQUE"]);
                    p.AtualizaFinanceiro = Convert.ToBoolean(Dr["IN_ATU_FINANCEIRO"]);
                    p.MovimentacaoInterna = Convert.ToBoolean(Dr["IN_MOV_INTERNA"]);
                    p.BaixaFinanceiro = Convert.ToBoolean(Dr["IN_BAIXA_FINANCEIRO"]);
                    p.CST10CalculaDifal = Convert.ToBoolean(Dr["CST10_DIFAL"]);
                    p.CST90CalculaDifal = Convert.ToBoolean(Dr["CST90_DIFAL"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.CodigoPrecedenciaImpostoICMS = Convert.ToInt32(Dr["CD_PRECEDENCIA_IMPOSTO_ICMS"]);
                    p.CodigoPrecedenciaImpostoPIS_COFINS = Convert.ToInt32(Dr["CD_PRECEDENCIA_IMPOSTO_PIS_COFINS"]);

                    p.MensagemIcms = Convert.ToString(Dr["TX_MSG_ICMS"]);
                    p.CodBeneficioFiscal = Convert.ToString(Dr["CD_BNF_FISCAL"]);
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.CodCST_CSOSN = Convert.ToInt32(Dr["CD_CST_CSOSN"]);
                    p.CodModDetBCIcms = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS"]);
                    p.CodModDetBCIcmsST = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS_ST"]);
                    p.MVAEntrada = Convert.ToDecimal(Dr["VL_MVA_ENTRADA"]);

                    if (p.CodHabil_RegTributario != 0)
                    {
                        if (Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]) == 3)  //REGIME NORMAL
                        {
                            switch (p.CodCST_CSOSN)
                            {
                                case 00:
                                    p.CST00ICMS = Convert.ToDecimal(Dr["CST00_ICMS"]);
                                    break;
                                case 10:
                                    p.CST10ReducaoBCICMSST = Convert.ToDecimal(Dr["CST10_RED_BC_ICMS_ST"]);
                                    p.CST10ICMS = Convert.ToDecimal(Dr["CST10_ICMS"]);
                                    p.CST10ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST10_RED_BC_ICMS_PROPRIO"]);
                                    p.CST10ICMSProprio = Convert.ToDecimal(Dr["CST10_ICMS_PROPRIO"]);
                                    p.CST10MVASaida = Convert.ToDecimal(Dr["CST10_MVA_SAIDA"]);
                                    p.CST10CalculaDifal = Convert.ToBoolean(Dr["CST10_DIFAL"]);
                                    break;
                                case 20:
                                    p.CST20ReducaoBCICMS = Convert.ToDecimal(Dr["CST20_RED_BC_ICMS"]);
                                    p.CST20ICMS = Convert.ToDecimal(Dr["CST20_ICMS"]);
                                    p.CST20MotDesoneracao = Convert.ToInt32(Dr["CST20_MOT_DESONERACAO"]);
                                    break;
                                case 30:
                                    p.CST30ReducaoBCICMSST = Convert.ToDecimal(Dr["CST30_RED_BC_ICMS_ST"]);
                                    p.CST30ICMS = Convert.ToDecimal(Dr["CST30_ICMS"]);
                                    p.CST30ICMSProprio = Convert.ToDecimal(Dr["CST30_ICMS_PROPRIO"]);
                                    p.CST30MVASaida = Convert.ToDecimal(Dr["CST30_MVA_SAIDA"]);
                                    p.CST30MotDesoneracao = Convert.ToInt32(Dr["CST30_MOT_DESONERACAO"]);
                                    break;
                                case 40:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 41:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 50:
                                    p.CST404150MotDesoneracao = Convert.ToInt32(Dr["CST40_41_50_MOT_DESONERACAO"]);
                                    break;
                                case 51:
                                    p.CST51ReducaoBCICMS = Convert.ToDecimal(Dr["CST51_RED_BC_ICMS"]);
                                    p.CST51ICMS = Convert.ToDecimal(Dr["CST51_ICMS"]);
                                    p.CST51Diferimento = Convert.ToDecimal(Dr["CST51_DIFERIMENTO"]);
                                    break;
                                case 70:
                                    p.CST70ReducaoBCICMSST = Convert.ToDecimal(Dr["CST70_RED_BC_ICMS_ST"]);
                                    p.CST70ICMS = Convert.ToDecimal(Dr["CST70_ICMS"]);
                                    p.CST70ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST70_RED_BC_ICMS_PROPRIO"]);
                                    p.CST70ICMSProprio = Convert.ToDecimal(Dr["CST70_ICMS_PROPRIO"]);
                                    p.CST70MVASaida = Convert.ToDecimal(Dr["CST70_MVA_SAIDA"]);
                                    p.CST70MotDesoneracao = Convert.ToInt32(Dr["CST70_MOT_DESONERACAO"]);
                                    break;
                                case 90:
                                    p.CST90ReducaoBCICMSST = Convert.ToDecimal(Dr["CST90_ICMS"]);
                                    p.CST90ICMS = Convert.ToDecimal(Dr["CST90_ICMS"]);
                                    p.CST90ReducaoBCICMSSTProprio = Convert.ToDecimal(Dr["CST90_RED_BC_ICMS_PROPRIO"]);
                                    p.CST90ICMSProprio = Convert.ToDecimal(Dr["CST90_ICMS_PROPRIO"]);
                                    p.CST90MVASaida = Convert.ToDecimal(Dr["CST90_MVA_SAIDA"]);
                                    p.CST90CalculaDifal = Convert.ToBoolean(Dr["CST90_DIFAL"]);
                                    p.CST90MotDesoneracao = Convert.ToInt32(Dr["CST90_MOT_DESONERACAO"]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (p.CodCST_CSOSN)
                            {

                                case 101:
                                    p.CSOSN101_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN101_ICMS_SIMPLES"]);
                                    break;
                                case 201:
                                    p.CSOSN201_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN201_RED_BC_ICMS_ST"]);
                                    p.CSOSN201_ICMS = Convert.ToDecimal(Dr["CSOSN201_ICMS"]);
                                    p.CSOSN201_MVASaida = Convert.ToDecimal(Dr["CSOSN201_MVA_SAIDA"]);
                                    p.CSOSN201_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN201_ICMS_SIMPLES"]);
                                    break;
                                case 202:
                                    p.CSOSN202_203_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN202_203_RED_BC_ICMS_ST"]);
                                    p.CSOSN202_203_ICMS = Convert.ToDecimal(Dr["CSOSN202_203_ICMS"]);
                                    p.CSOSN202_203_ICMS_PROPRIO = Convert.ToDecimal(Dr["CSOSN202_203_ICMS_PROPRIO"]);
                                    p.CSOSN202_203_MVASaida = Convert.ToDecimal(Dr["CSOSN202_203_MVA_SAIDA"]);
                                    break;
                                case 203:
                                    p.CSOSN202_203_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN202_203_RED_BC_ICMS_ST"]);
                                    p.CSOSN202_203_ICMS = Convert.ToDecimal(Dr["CSOSN202_203_ICMS"]);
                                    p.CSOSN202_203_ICMS_PROPRIO = Convert.ToDecimal(Dr["CSOSN202_203_ICMS_PROPRIO"]);
                                    p.CSOSN202_203_MVASaida = Convert.ToDecimal(Dr["CSOSN202_203_MVA_SAIDA"]);
                                    break;
                                case 900:
                                    p.CSOSN900_ReducaoBCICMSST = Convert.ToDecimal(Dr["CSOSN900_RED_BC_ICMS_ST"]);
                                    p.CSOSN900_ICMS = Convert.ToDecimal(Dr["CSOSN900_ICMS"]);
                                    p.CSOSN900_ICMS_Proprio = Convert.ToDecimal(Dr["CSOSN900_ICMS_PROPRIO"]);
                                    p.CSOSN900_ReducaoBCICMSProprio = Convert.ToDecimal(Dr["CSOSN900_RED_BC_ICMS_PROPRIO"]);
                                    p.CSOSN900_MVASaida = Convert.ToDecimal(Dr["CSOSN900_MVA_SAIDA"]);
                                    p.CSOSN900_ICMS_SIMPLES = Convert.ToDecimal(Dr["CSOSN900_ICMS_SIMPLES"]);
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    lista.Add(p);

                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Natureza de Operações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable RelTipoOperacoesCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_TIPO_OPERACAO]  " + MontaFiltroIntervalo(ListaFiltros);

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório de Todas Natureza de Operações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<TipoOperacao> ListarTipoAjuste()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [TIPO_DE_OPERACAO] WHERE CD_TP_MOVIMENTACAO = 62";
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoOperacao> lista = new List<TipoOperacao>();

                while (Dr.Read())
                {
                    TipoOperacao p = new TipoOperacao();

                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.DescricaoTipoOperacao = Convert.ToString(Dr["DS_TIPO_OPERACAO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas LOCALIZACAO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<TipoOperacao> ListarTpOperContraPartida(int intDocTpOper)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [TIPO_DE_OPERACAO] WHERE CD_TP_MOVIMENTACAO = " + intDocTpOper;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoOperacao> lista = new List<TipoOperacao>();

                while (Dr.Read())
                {
                    TipoOperacao p = new TipoOperacao();

                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.DescricaoTipoOperacao = Convert.ToString(Dr["DS_TIPO_OPERACAO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Tipo de Operação de Contra Partidas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void TipoAjusteInventario(int intCdEmpresa, ref int intCdTpInv)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select CD_TP_INV_AJUSTE from [PARAMETROS_DO_SISTEMA] WHERE CD_EMPRESA = " + intCdEmpresa.ToString();
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                if(Dr.Read())
                {
                    intCdTpInv = Convert.ToInt32(Dr["CD_TP_INV_AJUSTE"]);
                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas LOCALIZACAO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
		public int TipoContraPartida (int intCdEmpresa)
		{
			try
			{
				AbrirConexao();

				string strSQL = "select isnull(CD_TP_CTR_PARTIDA,0) as CD_TP_CTR_PARTIDA from TIPO_DE_OPERACAO where CD_TIPO_OPERACAO = " + intCdEmpresa.ToString();
				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				int Contra = 0;

				if (Dr.Read())
				{
					Contra = Convert.ToInt32(Dr["CD_TP_CTR_PARTIDA"]);
				}
				return Contra;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar tipo de operação: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<TipoOperacao> ListarTpOperSaidaOrdemProducao()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [TIPO_DE_OPERACAO] WHERE CD_TP_MOVIMENTACAO = 60 and CD_TP_CTR_PARTIDA > 0";
				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<TipoOperacao> lista = new List<TipoOperacao>();

				while (Dr.Read())
				{
					TipoOperacao p = new TipoOperacao();

					p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
					p.DescricaoTipoOperacao = Convert.ToString(Dr["DS_TIPO_OPERACAO"]);
					p.Cpl_ComboDescricaoTipoOperacao = p.CodigoTipoOperacao + " | " + p.DescricaoTipoOperacao;

					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas Tipo de Operação de Saídas para Ordem de Produção: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
	}
}
