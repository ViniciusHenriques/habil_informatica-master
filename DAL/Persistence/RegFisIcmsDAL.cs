using System;
using System.Linq;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class RegFisIcmsDAL:Conexao
    {
        protected string strSQL = "";

        public bool Inserir(RegFisIcms p)
        {

            try
            {
                AbrirConexao();
                string strCampos = "DT_VIGENCIA";
                string strValores = "@v1";

                strCampos += ", DT_ATUALIZACAO";
                strValores += ", @v2";
                strCampos += ", CD_SITUACAO";
                strValores += ", @v3";
                strCampos += ", DS_REGRA";
                strValores += ", @v4";

                strCampos += ", TX_MSG_ICMS";
                strValores += ", @v5 ";

                strCampos += ", CD_BNF_FISCAL ";
                strValores += ", @v6 ";

                strCampos += ", CD_REG_TRIBUTARIO ";
                strValores += ", @v7 ";

                strCampos += ", CD_CST_CSOSN ";
                strValores += ", @v8 ";

                strCampos += ", CD_MOD_DET_BC_ICMS ";
                strValores += ", @v9 ";

                strCampos += ", CD_MOD_DET_BC_ICMS_ST ";
                strValores += ", @v10 ";

                strCampos += ", VL_MVA_ENTRADA ";
                strValores += ", @v11 ";

                strCampos += ", VL_MVA_ORIGINAL ";
                strValores += ", @v111 ";

                if (p.CodHabil_RegTributario != 0)
                {


                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                    {
                        switch (p.CodCST_CSOSN)
                        {
                            case 00:
                                strCampos += ", CST00_ICMS ";
                                strValores += ", @v12 ";
                                break;
                            case 10:
                                strCampos += ", CST10_RED_BC_ICMS_ST ";
                                strValores += ", @v12 ";
                                strCampos += ", CST10_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CST10_RED_BC_ICMS_PROPRIO ";
                                strValores += ", @v14 ";
                                strCampos += ", CST10_ICMS_PROPRIO ";
                                strValores += ", @v15 ";
                                strCampos += ", CST10_MVA_SAIDA  ";
                                strValores += ", @v16 ";
                                strCampos += ", CST10_DIFAL ";
                                strValores += ", @v17 ";
                                break;
                            case 20:
                                strCampos += ", CST20_RED_BC_ICMS ";
                                strValores += ", @v12 ";
                                strCampos += ", CST20_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CST20_MOT_DESONERACAO ";
                                strValores += ", @v24 ";
                                break;
                            case 30:
                                strCampos += ", CST30_RED_BC_ICMS_ST ";
                                strValores += ", @v12 ";
                                strCampos += ", CST30_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CST30_ICMS_PROPRIO ";
                                strValores += ", @v14 ";
                                strCampos += ", CST30_MVA_SAIDA  ";
                                strValores += ", @v15 ";
                                strCampos += ", CST30_MOT_DESONERACAO ";
                                strValores += ", @v16 ";
                                break;
                            case 40:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO ";
                                strValores += ", @v12 ";
                                break;
                            case 41:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO ";
                                strValores += ", @v12 ";
                                break;
                            case 50:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO ";
                                strValores += ", @v12 ";
                                break;
                            case 51:
                                strCampos += ", CST51_RED_BC_ICMS ";
                                strValores += ", @v12 ";
                                strCampos += ", CST51_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CST51_DIFERIMENTO ";
                                strValores += ", @v14 ";
                                break;
                            case 70:
                                strCampos += ", CST70_RED_BC_ICMS_ST ";
                                strValores += ", @v12 ";
                                strCampos += ", CST70_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CST70_RED_BC_ICMS_PROPRIO ";
                                strValores += ", @v14 ";
                                strCampos += ", CST70_ICMS_PROPRIO ";
                                strValores += ", @v15 ";
                                strCampos += ", CST70_MVA_SAIDA  ";
                                strValores += ", @v16 ";
                                strCampos += ", CST70_MOT_DESONERACAO ";
                                strValores += ", @v17 ";
                                break;
                            case 90:
                                strCampos += ", CST90_RED_BC_ICMS_ST ";
                                strValores += ", @v12 ";
                                strCampos += ", CST90_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CST90_RED_BC_ICMS_PROPRIO ";
                                strValores += ", @v14 ";
                                strCampos += ", CST90_ICMS_PROPRIO ";
                                strValores += ", @v15 ";
                                strCampos += ", CST90_MVA_SAIDA  ";
                                strValores += ", @v16 ";
                                strCampos += ", CST90_DIFAL ";
                                strValores += ", @v17 ";
                                strCampos += ", CST90_MOT_DESONERACAO ";
                                strValores += ", @v18 ";
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
                                strValores += ", @v12 ";
                                break;
                            case 201:
                                strCampos += ", CSOSN201_RED_BC_ICMS_ST ";
                                strValores += ", @v12 ";
                                strCampos += ", CSOSN201_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CSOSN201_MVA_SAIDA ";
                                strValores += ", @v14 ";
                                strCampos += ", CSOSN201_ICMS_SIMPLES ";
                                strValores += ", @v15 ";
                                break;
                            case 202:
                                strCampos += ", CSOSN202_203_RED_BC_ICMS_ST ";
                                strValores += ", @v12 ";
                                strCampos += ", CSOSN202_203_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CSOSN202_203_ICMS_PROPRIO ";
                                strValores += ", @v14 ";
                                strCampos += ", CSOSN202_203_MVA_SAIDA ";
                                strValores += ", @v15 ";
                                break;
                            case 203:
                                strCampos += ", CSOSN202_203_RED_BC_ICMS_ST ";
                                strValores += ", @v12 ";
                                strCampos += ", CSOSN202_203_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CSOSN202_203_ICMS_PROPRIO ";
                                strValores += ", @v14 ";
                                strCampos += ", CSOSN202_203_MVA_SAIDA ";
                                strValores += ", @v15 ";
                                break;
                            case 900:
                                strCampos += ", CSOSN900_RED_BC_ICMS_ST ";
                                strValores += ", @v12 ";
                                strCampos += ", CSOSN900_ICMS ";
                                strValores += ", @v13 ";
                                strCampos += ", CSOSN900_ICMS_PROPRIO ";
                                strValores += ", @v14 ";
                                strCampos += ", CSOSN900_RED_BC_ICMS_PROPRIO ";
                                strValores += ", @v15 ";
                                strCampos += ", CSOSN900_MVA_SAIDA ";
                                strValores += ", @v16 ";
                                strCampos += ", CSOSN900_ICMS_SIMPLES ";
                                strValores += ", @v17 ";
                                break;
                            default:
                                break;
                        }

                    }

                }
                                
                strSQL = "insert into [REGRA_FISCAL_ICMS] ("+ strCampos + ") values ("+ strValores + "); SELECT SCOPE_IDENTITY()";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Convert.ToDateTime(p.DataVigencia));
                Cmd.Parameters.AddWithValue("@v2", Convert.ToDateTime(p.DataAtualizacao));
                Cmd.Parameters.AddWithValue("@v3", p.CodSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.Descricao);
                Cmd.Parameters.AddWithValue("@v5", p.MensagemIcms);
                Cmd.Parameters.AddWithValue("@v6", p.CodBeneficioFiscal);
                Cmd.Parameters.AddWithValue("@v7", p.CodHabil_RegTributario);
                Cmd.Parameters.AddWithValue("@v8", p.CodCST_CSOSN);
                Cmd.Parameters.AddWithValue("@v9", p.CodModDetBCIcms);
                Cmd.Parameters.AddWithValue("@v10", p.CodModDetBCIcmsST);
                Cmd.Parameters.AddWithValue("@v11", p.MVAEntrada);
                Cmd.Parameters.AddWithValue("@v111", p.MVAOriginal);
                Cmd.Parameters.AddWithValue("@v1111", p.VlFCP);


                if (p.CodHabil_RegTributario != 0)
                {
                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                    {

                        switch (p.CodCST_CSOSN)
                        {
                            case 00:
                                Cmd.Parameters.AddWithValue("@v12", p.CST00ICMS);
                                break;
                            case 10:
                                Cmd.Parameters.AddWithValue("@v12", p.CST10ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CST10ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST10ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CST10ICMSProprio);
                                Cmd.Parameters.AddWithValue("@V16", p.CST10MVASaida);
                                Cmd.Parameters.AddWithValue("@V17", p.CST10CalculaDifal);
                                break;
                            case 20:
                                Cmd.Parameters.AddWithValue("@v12", p.CST20ReducaoBCICMS);
                                Cmd.Parameters.AddWithValue("@v13", p.CST20ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST20MotDesoneracao);
                                break;
                            case 30:
                                Cmd.Parameters.AddWithValue("@v12", p.CST30ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CST30ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST30ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CST30MVASaida);
                                Cmd.Parameters.AddWithValue("@v16", p.CST30MotDesoneracao);
                                break;
                            case 40:
                                Cmd.Parameters.AddWithValue("@v12", p.CST404150MotDesoneracao);
                                break;
                            case 41:
                                Cmd.Parameters.AddWithValue("@v12", p.CST404150MotDesoneracao);
                                break;
                            case 50:
                                Cmd.Parameters.AddWithValue("@v12", p.CST404150MotDesoneracao);
                                break;
                            case 51:
                                Cmd.Parameters.AddWithValue("@v12", p.CST51ReducaoBCICMS);
                                Cmd.Parameters.AddWithValue("@v13", p.CST51ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST51Diferimento);
                                break;
                            case 70:
                                Cmd.Parameters.AddWithValue("@v12", p.CST70ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CST70ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST70ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CST70ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v16", p.CST70MVASaida);
                                Cmd.Parameters.AddWithValue("@v17", p.CST70MotDesoneracao);
                                break;
                            case 90:
                                Cmd.Parameters.AddWithValue("@v12", p.CST90ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CST90ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST90ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CST90ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v16", p.CST90MVASaida);
                                Cmd.Parameters.AddWithValue("@v17", p.CST90CalculaDifal);
                                Cmd.Parameters.AddWithValue("@v18", p.CST90MotDesoneracao);
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
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN101_ICMS_SIMPLES);
                                break;
                            case 201:
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN201_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CSOSN201_ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CSOSN201_MVASaida);
                                Cmd.Parameters.AddWithValue("@v15", p.CSOSN201_ICMS_SIMPLES);
                                break;
                            case 202:
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN202_203_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CSOSN202_203_ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CSOSN202_203_ICMS_PROPRIO);
                                Cmd.Parameters.AddWithValue("@v15", p.CSOSN202_203_MVASaida);
                                break;
                            case 203:
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN202_203_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CSOSN202_203_ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CSOSN202_203_ICMS_PROPRIO);
                                Cmd.Parameters.AddWithValue("@v15", p.CSOSN202_203_MVASaida);
                                break;
                            case 900:
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN900_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CSOSN900_ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CSOSN900_ICMS_Proprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CSOSN900_ReducaoBCICMSProprio);
                                Cmd.Parameters.AddWithValue("@v16", p.CSOSN900_MVASaida);
                                Cmd.Parameters.AddWithValue("@v17", p.CSOSN900_ICMS_SIMPLES);
                                break;
                            default:
                                break;
                        }
                    }
                }
                
                p.CodigoRegFisIcms = Convert.ToInt64(Cmd.ExecuteScalar());


                

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
                            throw new Exception("Erro ao Incluir Regra Fiscal Icms: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Fiscal Icms Regra Fiscal Icms: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(long CodigoRegra)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from [REGRA_FISCAL_ICMS] Where [CD_REGRA_FISCAL_ICMS] = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
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
                            throw new Exception("Erro ao excluir Regra Fiscal Icms Aplicação Uso: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Regra Fiscal Icms: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void InativarRegra(long CodigoRegra)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Update [REGRA_FISCAL_ICMS] set cd_situacao = 2 Where [CD_REGRA_FISCAL_ICMS] = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inativar Regra Fiscal Icms: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIcms  PesquisarRegFisIcms(long CodigoRegra)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select R.*, S.DS_TIPO AS DS_SITUACAO from [REGRA_FISCAL_ICMS] as R Inner Join Habil_Tipo as S on R.CD_SITUACAO = S.CD_TIPO Where R.[CD_REGRA_FISCAL_ICMS] = @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);

                Dr = Cmd.ExecuteReader();

                RegFisIcms  p = null;

                if (Dr.Read())
                {
                    p = new RegFisIcms();
                    p.CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"]);
                    p.DataVigencia = Convert.ToDateTime(Dr["DT_VIGENCIA"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.DataHora = Convert.ToDateTime(Dr["DT_GERACAO"]);
                    p.Cpl_Estados = Dr["DS_LOCALIZACAO"].ToString();
                    p.Cpl_GpoPessoas = Convert.ToString(Dr["DS_GPO_PESSOA"]);
                    p.Cpl_GpoProdutos = Convert.ToString(Dr["DS_GPO_PRODUTO"]);
                    p.Cpl_AplUso = Dr["DS_APLICACAO"].ToString();
                    p.Cpl_OprFiscal = Dr["DS_TIPO_OPERACAO"].ToString();
                    p.Descricao = Convert.ToString(Dr["DS_REGRA"]);
                    p.CodSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DcrSituacao = Dr["DS_SITUACAO"].ToString();
                    p.MensagemIcms = Convert.ToString(Dr["TX_MSG_ICMS"]);
                    p.CodBeneficioFiscal = Convert.ToString(Dr["CD_BNF_FISCAL"]);
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.CodCST_CSOSN = Convert.ToInt32(Dr["CD_CST_CSOSN"]);
                    p.CodModDetBCIcms = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS"]);
                    p.CodModDetBCIcmsST = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS_ST"]);
                    p.MVAEntrada = Convert.ToDecimal(Dr["VL_MVA_ENTRADA"]);
                    p.MVAOriginal = Convert.ToDecimal(Dr["VL_MVA_ORIGINAL"]);

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
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void SalvarRegra(RegFisIcms p, 
                       List<RegFisIcmsLocalizacao> ListaEstados, 
                       List<RegFisIcmsGpoTribPessoa> ListaGruposPessoas, 
                       List<RegFisIcmsGpoTribProduto> ListaGruposProdutos, 
                       List<RegFisIcmsHabil_AplicacaoUso> ListaAplicacoes, 
                       List<RegFisIcmsTipoOperFiscal> ListaOperacoes,
                       int CodMaquina, int CodUsuario)
        {

            RegFisIcmsLocalizacaoDAL t1 = new RegFisIcmsLocalizacaoDAL();
            RegFisIcmsLocalizacao p1;

            RegFisIcmsGpoTribPessoaDAL t2 = new RegFisIcmsGpoTribPessoaDAL();
            RegFisIcmsGpoTribPessoa p2;

            RegFisIcmsGpoTribProdutoDAL t3 = new RegFisIcmsGpoTribProdutoDAL();
            RegFisIcmsGpoTribProduto p3;

            RegFisIcmsHabil_AplicacaoUsoDAL t4 = new RegFisIcmsHabil_AplicacaoUsoDAL();
            RegFisIcmsHabil_AplicacaoUso p4;

            RegFisIcmsTipoOperFiscalDAL t5 = new RegFisIcmsTipoOperFiscalDAL();
            RegFisIcmsTipoOperFiscal p5;

            try
            {
                List<Habil_Log> listaLog = new List<Habil_Log>();
                Habil_LogDAL Rn_Log = new Habil_LogDAL();

                DataTable tbA1, tbA, tbB;

                if (p.CodigoRegFisIcms == 0) //Registro Novo
                { 
                    if (Inserir(p))
                    {
                        foreach (RegFisIcmsLocalizacao item in ListaEstados)
                        {
                            p1 = new RegFisIcmsLocalizacao {CodigoRegFisIcms = p.CodigoRegFisIcms,CodLocalizacaoUF = item.CodLocalizacaoUF};
                            t1.Inserir(p1);
                        };

                        foreach (RegFisIcmsGpoTribPessoa item in ListaGruposPessoas)
                        {
                            p2 = new RegFisIcmsGpoTribPessoa {CodigoRegFisIcms = p.CodigoRegFisIcms, CodGpoTribPessoa = item.CodGpoTribPessoa};
                            t2.Inserir(p2);
                        };
                    
                        foreach (RegFisIcmsGpoTribProduto item in ListaGruposProdutos)
                        {
                            p3 = new RegFisIcmsGpoTribProduto {CodigoRegFisIcms = p.CodigoRegFisIcms, CodGpoTribProduto= item.CodGpoTribProduto};
                            t3.Inserir(p3);
                        };

                        foreach (RegFisIcmsHabil_AplicacaoUso item in ListaAplicacoes)
                        {
                            p4 = new RegFisIcmsHabil_AplicacaoUso {CodigoRegFisIcms = p.CodigoRegFisIcms, CodHabil_AplicacaoUso = item.CodHabil_AplicacaoUso};
                            t4.Inserir(p4);
                        };

                        foreach (RegFisIcmsTipoOperFiscal item in ListaOperacoes)
                        {
                            p5 = new RegFisIcmsTipoOperFiscal {CodigoRegFisIcms = p.CodigoRegFisIcms, CodTipoOperFiscal = item.CodTipoOperFiscal};
                            t5.Inserir(p5);
                        };

                        DBTabelaDAL CLSDBTabela = new DBTabelaDAL();

                        CLSDBTabela.ExecutaComandoSQL("update REGRA_FISCAL_ICMS set DT_ATUALIZACAO = getdate() where CD_REGRA_FISCAL_ICMS = " + p.CodigoRegFisIcms.ToString());

                    }
                }
                else
                {
                    tbA1 = ObterRegFisIcms(p.CodigoRegFisIcms);

                    if (Alterar(p))
                    {
                        tbA = new DataTable();
                        tbA = t1.ObterRegFisIcmsLocalizacao(p.CodigoRegFisIcms);
                        t1.Excluir(p.CodigoRegFisIcms, 0);
                        foreach (RegFisIcmsLocalizacao item in ListaEstados)
                        {
                            p1 = new RegFisIcmsLocalizacao {CodigoRegFisIcms = p.CodigoRegFisIcms, CodLocalizacaoUF = item.CodLocalizacaoUF};
                            t1.Inserir(p1);
                        };
                        tbB = new DataTable();
                        tbB = t1.ObterRegFisIcmsLocalizacao(p.CodigoRegFisIcms);
                        listaLog = Rn_Log.ComparaDataTablesRelacional(tbA, tbB, p.CodigoRegFisIcms, CodUsuario, CodMaquina, 6, 5 , "REGRA_FISCAL_ICMS", "CD_TAB_ALIQ_ICMS");
                        foreach (Habil_Log item in listaLog)
                            Rn_Log.Inserir(item);


                        tbA = new DataTable();
                        tbA = t2.ObterRegFisIcmsGpoPessoa(p.CodigoRegFisIcms);
                        t2.Excluir(p.CodigoRegFisIcms, 0);
                        foreach (RegFisIcmsGpoTribPessoa item in ListaGruposPessoas)
                        {
                            p2 = new RegFisIcmsGpoTribPessoa {CodigoRegFisIcms = p.CodigoRegFisIcms, CodGpoTribPessoa = item.CodGpoTribPessoa};
                            t2.Inserir(p2);
                        };
                        tbB = new DataTable();
                        tbB = t2.ObterRegFisIcmsGpoPessoa(p.CodigoRegFisIcms);
                        listaLog = Rn_Log.ComparaDataTablesRelacional(tbA, tbB, p.CodigoRegFisIcms, CodUsuario, CodMaquina, 8, 7, "REGRA_FISCAL_ICMS", "CD_GPO_TRIB_PESSOA");
                        foreach (Habil_Log item in listaLog)
                            Rn_Log.Inserir(item);


                        tbA = new DataTable();
                        tbA = t3.ObterRegFisIcmsGpoProduto(p.CodigoRegFisIcms);
                        t3.Excluir(p.CodigoRegFisIcms, 0);
                        foreach (RegFisIcmsGpoTribProduto item in ListaGruposProdutos)
                        {
                            p3 = new RegFisIcmsGpoTribProduto {CodigoRegFisIcms = p.CodigoRegFisIcms, CodGpoTribProduto = item.CodGpoTribProduto};
                            t3.Inserir(p3);
                        };
                        tbB = new DataTable();
                        tbB = t3.ObterRegFisIcmsGpoProduto(p.CodigoRegFisIcms);
                        listaLog = Rn_Log.ComparaDataTablesRelacional(tbA, tbB, p.CodigoRegFisIcms, CodUsuario, CodMaquina, 10, 9, "REGRA_FISCAL_ICMS", "CD_GPO_TRIB_PRODUTO");
                        foreach (Habil_Log item in listaLog)
                            Rn_Log.Inserir(item);


                        tbA = new DataTable();
                        tbA = t4.ObterRegFisIcmsHabil_AplicacaoUso(p.CodigoRegFisIcms);
                        t4.Excluir(p.CodigoRegFisIcms, 0);
                        foreach (RegFisIcmsHabil_AplicacaoUso item in ListaAplicacoes)
                        {
                            p4 = new RegFisIcmsHabil_AplicacaoUso {CodigoRegFisIcms = p.CodigoRegFisIcms, CodHabil_AplicacaoUso = item.CodHabil_AplicacaoUso};
                            t4.Inserir(p4);
                        };
                        tbB = new DataTable();
                        tbB = t4.ObterRegFisIcmsHabil_AplicacaoUso(p.CodigoRegFisIcms);
                        listaLog = Rn_Log.ComparaDataTablesRelacional(tbA, tbB, p.CodigoRegFisIcms, CodUsuario, CodMaquina, 12,11, "REGRA_FISCAL_ICMS", "CD_APLICACAO");
                        foreach (Habil_Log item in listaLog)
                            Rn_Log.Inserir(item);


                        tbA = new DataTable();
                        tbA = t5.ObterRegFisIcmsOperFiscal(p.CodigoRegFisIcms);
                        t5.Excluir(p.CodigoRegFisIcms, 0);
                        foreach (RegFisIcmsTipoOperFiscal item in ListaOperacoes)
                        {
                            p5 = new RegFisIcmsTipoOperFiscal {CodigoRegFisIcms = p.CodigoRegFisIcms, CodTipoOperFiscal = item.CodTipoOperFiscal};
                            t5.Inserir(p5);
                        };
                        tbB = new DataTable();
                        tbB = t5.ObterRegFisIcmsOperFiscal(p.CodigoRegFisIcms);
                        listaLog = Rn_Log.ComparaDataTablesRelacional(tbA, tbB, p.CodigoRegFisIcms, CodUsuario, CodMaquina, 14, 13, "REGRA_FISCAL_ICMS", "CD_TIPO_OP_FISCAL");
                        foreach (Habil_Log item in listaLog)
                            Rn_Log.Inserir(item);

                        new DBTabelaDAL().ExecutaComandoSQL("update REGRA_FISCAL_ICMS set DT_GERACAO = getdate() where CD_REGRA_FISCAL_ICMS = " + p.CodigoRegFisIcms.ToString());

                        tbB = ObterRegFisIcms(p.CodigoRegFisIcms);
                        listaLog = Rn_Log.ComparaDataTables(tbA1, tbB, p.CodigoRegFisIcms, CodUsuario, CodMaquina, 15, "REGRA_FISCAL_ICMS");
                        foreach (Habil_Log item in listaLog)
                            Rn_Log.Inserir(item);

                    }

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
        public List<RegFisIcms> ListarRegFisIcmsCompleto(List<DBTabelaCampos> ListaFiltros , short shtQtdeRegistros = 0)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select ";

                if (shtQtdeRegistros != 0)
                    strSQL += " TOP " + shtQtdeRegistros.ToString()  + " R.*, S.DS_TIPO AS DS_SITUACAO  ";
                else
                    strSQL += " R.*, S.DS_TIPO AS DS_SITUACAO  ";

                strSQL += "from [REGRA_FISCAL_ICMS] as R Inner Join Habil_Tipo as S on R.CD_SITUACAO = S.CD_TIPO ";
                string strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL += strValor;

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<RegFisIcms> lista = new List<RegFisIcms>();
                Habil_AplicacaoUsoDAL x1 = new Habil_AplicacaoUsoDAL();
                Habil_TipoOperacaoFiscalDAL y1 = new Habil_TipoOperacaoFiscalDAL();

                while (Dr.Read())
                {
                    RegFisIcms p = new RegFisIcms();

                    p.CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"]);
                    p.DataVigencia = Convert.ToDateTime(Dr["DT_VIGENCIA"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.DataHora = Convert.ToDateTime(Dr["DT_GERACAO"]);
                    p.Cpl_Estados = Dr["DS_LOCALIZACAO"].ToString();
                    p.Cpl_GpoPessoas = Convert.ToString(Dr["DS_GPO_PESSOA"]);
                    p.Cpl_GpoProdutos = Convert.ToString(Dr["DS_GPO_PRODUTO"]);
                    p.Cpl_AplUso = Dr["DS_APLICACAO"].ToString();
                    p.Cpl_OprFiscal =Dr["DS_TIPO_OPERACAO"].ToString();
                    p.CodSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DcrSituacao = Dr["DS_SITUACAO"].ToString();

                    p.Descricao = Convert.ToString(Dr["DS_REGRA"]);
                    p.MensagemIcms = Convert.ToString(Dr["TX_MSG_ICMS"]);
                    p.CodBeneficioFiscal = Convert.ToString(Dr["CD_BNF_FISCAL"]);
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.CodCST_CSOSN = Convert.ToInt32(Dr["CD_CST_CSOSN"]);
                    p.CodModDetBCIcms = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS"]);
                    p.CodModDetBCIcmsST = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS_ST"]);
                    p.MVAEntrada = Convert.ToDecimal(Dr["VL_MVA_ENTRADA"]);
                    p.MVAOriginal = Convert.ToDecimal(Dr["VL_MVA_ORIGINAL"]);
                    

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

                lista.OrderByDescending(x => x.CodigoRegFisIcms).ToList();
                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Regras de Icms: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public void RegistrarLog(double CodigoRegra, double CodigoLog )
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Update [REGRA_FISCAL_ICMS] set CD_LOG = @v2 Where [CD_REGRA_FISCAL_ICMS] = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Cmd.Parameters.AddWithValue("@v2", CodigoLog);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao REGISTRAR REGRA Regra Fiscal Icms: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelRegFisIcmsCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_REG_FIS_ICMS]  " + MontaFiltroIntervalo(ListaFiltros);

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório de Todas Regras Fiscais de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ListarRegFisIcmsLocalizacoes()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select distinct DS_LOCALIZACAO from [REGRA_FISCAL_ICMS]";
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                DataTable dt = new DataTable();
                int intI = 1;

                dt.Columns.Add("CodigoDrop", typeof(Int32));
                dt.Columns.Add("DescricaoDrop", typeof(string));

                while (Dr.Read())
                {
                    dt.Rows.Add(intI, Convert.ToString(Dr["DS_LOCALIZACAO"]));
                    intI++;
                }
               
                //lista.OrderByDescending(x => x.CodigoRegFisIcms).ToList();

                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ListarRegFisIcmsLocalizacoes: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ListarRegFisIcmsGrupoPessoas()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select distinct DS_GPO_PESSOA from [REGRA_FISCAL_ICMS]";
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                DataTable dt = new DataTable();
                int intI = 1;

                dt.Columns.Add("CodigoDrop", typeof(Int32));
                dt.Columns.Add("DescricaoDrop", typeof(string));

                while (Dr.Read())
                {
                    dt.Rows.Add(intI, Convert.ToString(Dr["DS_GPO_PESSOA"]));
                    intI++;
                }

                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ListarRegFisIcmsGrupoPessoas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ListarRegFisIcmsGrupoProdutos()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select distinct DS_GPO_PRODUTO from [REGRA_FISCAL_ICMS]";
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                DataTable dt = new DataTable();
                int intI = 1;

                dt.Columns.Add("CodigoDrop", typeof(Int32));
                dt.Columns.Add("DescricaoDrop", typeof(string));

                while (Dr.Read())
                {
                    dt.Rows.Add(intI, Convert.ToString(Dr["DS_GPO_PRODUTO"]));
                    intI++;
                }

                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ListarRegFisIcmsGrupoProdutos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ListarRegFisIcmsAplicacao()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select distinct DS_APLICACAO from [REGRA_FISCAL_ICMS]";
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                DataTable dt = new DataTable();
                int intI = 1;

                dt.Columns.Add("CodigoDrop", typeof(Int32));
                dt.Columns.Add("DescricaoDrop", typeof(string));

                while (Dr.Read())
                {
                    dt.Rows.Add(intI, Convert.ToString(Dr["DS_APLICACAO"]));
                    intI++;
                }

                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ListarRegFisIcmsGrupoProdutos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ListarRegFisIcmsTpOpFiscal()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select distinct DS_TIPO_OPERACAO from [REGRA_FISCAL_ICMS]";
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                DataTable dt = new DataTable();
                int intI = 1;

                dt.Columns.Add("CodigoDrop", typeof(Int32));
                dt.Columns.Add("DescricaoDrop", typeof(string));

                while (Dr.Read())
                {
                    dt.Rows.Add(intI, Convert.ToString(Dr["DS_TIPO_OPERACAO"]));
                    intI++;
                }

                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ListarRegFisIcmsGrupoProdutos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public string ValidarCombinacao(DataTable dt)
        {
            string strMensagem = "";
            try
            {
                AbrirConexao();

                string strSQL2 = "";
                string strSQL1 = ""; 

                foreach (DataRow  item in dt.Rows)
                {
                    if (strSQL2 == "")
                    {
                        strSQL2 += " And ( (L.CD_TAB_ALIQ_ICMS = " + item["CodigoLocalizacao"].ToString() +
                        " And PE.[CD_GPO_TRIB_PESSOA] = " + item["CodigoGpoPessoa"].ToString() +
                        " And PR.[CD_GPO_TRIB_PRODUTO] = " + item["CodigoGpoProduto"].ToString() +
                        " And A.[CD_APLICACAO] = " + item["CodigoAplicacao"].ToString() +
                        " And T.[CD_TIPO_OP_FISCAL] = " + item["CodigoTipoOperacao"].ToString() + ") ";

                        strSQL1 = "  Where R.[CD_REGRA_FISCAL_ICMS] <> " + item["CodigoRegra"].ToString();
                        strSQL1 += " And   R.DT_VIGENCIA  >= '" + Convert.ToDateTime(item["DataVigencia"]).ToString("yyyy-MM-dd") + "'";

                    }
                    else
                    {
                        strSQL2 += " Or (L.CD_TAB_ALIQ_ICMS = " + item["CodigoLocalizacao"].ToString() +
                        " And PE.[CD_GPO_TRIB_PESSOA] = " + item["CodigoGpoPessoa"].ToString() +
                        " And PR.[CD_GPO_TRIB_PRODUTO] = " + item["CodigoGpoProduto"].ToString() +
                        " And A.[CD_APLICACAO] = " + item["CodigoAplicacao"].ToString() +
                        " And T.[CD_TIPO_OP_FISCAL] = " + item["CodigoTipoOperacao"].ToString() + ") ";
                         
                    }
                }

                strSQL2 += " ) AND R.CD_SITUACAO = 1 ";

                string strSQL = " Select  R.CD_REGRA_FISCAL_ICMS, " +
                                        " R.DT_VIGENCIA, " +
                                        " R.DT_ATUALIZACAO, " +
                                        " L.CD_TAB_ALIQ_ICMS, EO.SIGLA + ' - ' + ED.SIGLA AS LOCAL, " +
                                        " PR.CD_GPO_TRIB_PRODUTO, GPR.DS_GPO_TRIB_PRODUTO, " +
                                        " PE.CD_GPO_TRIB_PESSOA, GPE.DS_GPO_TRIB_PESSOA, " +
                                        " A.CD_APLICACAO, R.DS_APLICACAO, " +
                                        " T.CD_TIPO_OP_FISCAL, R.DS_TIPO_OPERACAO " +  
                                " from REGRA_FISCAL_ICMS as R " +
                                "  INNER JOIN REGRA_FISCAL_ICMS_LOCALIZACAO AS L " +
                                "    ON R.CD_REGRA_FISCAL_ICMS = L.CD_REGRA_FISCAL_ICMS" +
                                "  INNER JOIN HABIL_TABELA_ICMS_ALIQUOTAS AS H " +
                                "    ON H.CD_TAB_ALIQ_ICMS = L.CD_TAB_ALIQ_ICMS" +
                                "  INNER JOIN ESTADO AS EO " +
                                "    ON H.CD_EST_ORIGEM = EO.CD_ESTADO " +
                                "  INNER JOIN ESTADO AS ED " +
                                "    ON H.CD_EST_DESTINO = ED.CD_ESTADO " +
                                "  INNER JOIN REGRA_FISCAL_ICMS_GRUPO_PRODUTO AS PR " +
                                "    ON R.CD_REGRA_FISCAL_ICMS = PR.CD_REGRA_FISCAL_ICMS " +
                                "  INNER JOIN GRUPO_TRIB_PRODUTO AS GPR " +
                                "    ON GPR.CD_GPO_TRIB_PRODUTO = PR.CD_GPO_TRIB_PRODUTO " +
                                "  INNER JOIN REGRA_FISCAL_ICMS_GRUPO_PESSOA AS PE " +
                                "    ON R.CD_REGRA_FISCAL_ICMS = PE.CD_REGRA_FISCAL_ICMS " +
                                "  INNER JOIN GRUPO_TRIB_PESSOA AS GPE " +
                                "    ON GPE.CD_GPO_TRIB_PESSOA = PE.CD_GPO_TRIB_PESSOA " +
                                "  INNER JOIN REGRA_FISCAL_ICMS_APLICACAO AS A " +
                                "    ON R.CD_REGRA_FISCAL_ICMS = A.CD_REGRA_FISCAL_ICMS " +
                                "  INNER JOIN REGRA_FISCAL_ICMS_TIPO_OPERACAO_FISCAL AS T " +
                                "    ON R.CD_REGRA_FISCAL_ICMS = T.CD_REGRA_FISCAL_ICMS  ";

                Cmd = new SqlCommand(strSQL + strSQL1 + strSQL2, Con);
                Dr = Cmd.ExecuteReader();

                if (Dr.HasRows)
                    strMensagem += " Combinação da Regra a ser Criada/Alterada já existe. Verifique !!!<br/><br/> ";

                while (Dr.Read())
                {
                    strMensagem += "<strong>Regra: </strong>" + Dr["CD_REGRA_FISCAL_ICMS"].ToString();
                    strMensagem += "  <strong>Dt.Vig:</strong> " + Convert.ToDateTime( Dr["DT_VIGENCIA"]).ToString("dd/MM/yyyy");
                    strMensagem += "  <strong>Local: </strong>" + Dr["LOCAL"].ToString();
                    strMensagem += "  <strong>GpoPessoa: </strong>" + Dr["DS_GPO_TRIB_PESSOA"].ToString();
                    strMensagem += "  <strong>Gpo Produto: </strong>" + Dr["DS_GPO_TRIB_PRODUTO"].ToString();
                    strMensagem += "  <strong>Aplicação: </strong>" + Dr["DS_APLICACAO"].ToString();
                    strMensagem += "  <strong> Oper.Fiscal: </strong>" + Dr["DS_TIPO_OPERACAO"].ToString();
                    strMensagem += "<br/><br/> ";
                }


                return strMensagem;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ValidaCobinações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }

        public bool Alterar(RegFisIcms p)
        {

            try
            {
                AbrirConexao();

                string strCampos = "DT_VIGENCIA = @v1";
                strCampos += ", DT_ATUALIZACAO = @v2";
                strCampos += ", CD_SITUACAO = @v3";
                strCampos += ", DS_REGRA = @v4";
                strCampos += ", TX_MSG_ICMS = @v5 ";
                strCampos += ", CD_BNF_FISCAL = @v6 ";
                strCampos += ", CD_REG_TRIBUTARIO = @v7 ";
                strCampos += ", CD_CST_CSOSN = @v8 ";
                strCampos += ", CD_MOD_DET_BC_ICMS = @v9 ";
                strCampos += ", CD_MOD_DET_BC_ICMS_ST = @v10 ";
                strCampos += ", VL_MVA_ENTRADA = @v11 ";
                strCampos += ", VL_MVA_ORIGINAL = @v111 ";

                if (p.CodHabil_RegTributario != 0)
                {


                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                    {
                        switch (p.CodCST_CSOSN)
                        {
                            case 00:
                                strCampos += ", CST00_ICMS = @v12 ";
                                break;
                            case 10:
                                strCampos += ", CST10_RED_BC_ICMS_ST = @v12 ";
                                strCampos += ", CST10_ICMS = @v13 ";
                                strCampos += ", CST10_RED_BC_ICMS_PROPRIO = @v14 ";
                                strCampos += ", CST10_ICMS_PROPRIO = @v15 ";
                                strCampos += ", CST10_MVA_SAIDA  = @v16 ";
                                strCampos += ", CST10_DIFAL = @v17 ";
                                break;
                            case 20:
                                strCampos += ", CST20_RED_BC_ICMS = @v12 ";
                                strCampos += ", CST20_ICMS = @v13 ";
                                strCampos += ", CST20_MOT_DESONERACAO = @v24 ";
                                break;
                            case 30:
                                strCampos += ", CST30_RED_BC_ICMS_ST = @v12 ";
                                strCampos += ", CST30_ICMS = @v13 ";
                                strCampos += ", CST30_ICMS_PROPRIO = @v14 ";
                                strCampos += ", CST30_MVA_SAIDA = @v15 ";
                                strCampos += ", CST30_MOT_DESONERACAO = @v16 ";
                                break;
                            case 40:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO = @v12 ";
                                break;
                            case 41:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO = @v12 ";
                                break;
                            case 50:
                                strCampos += ", CST40_41_50_MOT_DESONERACAO = @v12 ";
                                break;
                            case 51:
                                strCampos += ", CST51_RED_BC_ICMS = @v12 ";
                                strCampos += ", CST51_ICMS = @v13 ";
                                strCampos += ", CST51_DIFERIMENTO = @v14 ";
                                break;
                            case 70:
                                strCampos += ", CST70_RED_BC_ICMS_ST = @v12 ";
                                strCampos += ", CST70_ICMS = @v13 ";
                                strCampos += ", CST70_RED_BC_ICMS_PROPRIO = @v14 ";
                                strCampos += ", CST70_ICMS_PROPRIO = @v15 ";
                                strCampos += ", CST70_MVA_SAIDA = @v16 ";
                                strCampos += ", CST70_MOT_DESONERACAO = @v17 ";
                                break;
                            case 90:
                                strCampos += ", CST90_RED_BC_ICMS_ST = @v12 ";
                                strCampos += ", CST90_ICMS = @v13 ";
                                strCampos += ", CST90_RED_BC_ICMS_PROPRIO = @v14 ";
                                strCampos += ", CST90_ICMS_PROPRIO = @v15 ";
                                strCampos += ", CST90_MVA_SAIDA = @v16 ";
                                strCampos += ", CST90_DIFAL = @v17 ";
                                strCampos += ", CST90_MOT_DESONERACAO = @v18 ";
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
                                strCampos += ", CSOSN101_ICMS_SIMPLES = @v12 ";
                                break;
                            case 201:
                                strCampos += ", CSOSN201_RED_BC_ICMS_ST = @v13 ";
                                strCampos += ", CSOSN201_ICMS = @v14 ";
                                strCampos += ", CSOSN201_MVA_SAIDA = @v15 ";
                                strCampos += ", CSOSN201_ICMS_SIMPLES = @v16 ";
                                break;
                            case 202:
                                strCampos += ", CSOSN202_203_RED_BC_ICMS_ST = @v12 ";
                                strCampos += ", CSOSN202_203_ICMS = @v13 ";
                                strCampos += ", CSOSN202_203_ICMS_PROPRIO = @v14 ";
                                strCampos += ", CSOSN202_203_MVA_SAIDA = @v15 ";
                                break;
                            case 203:
                                strCampos += ", CSOSN202_203_RED_BC_ICMS_ST = @v12 ";
                                strCampos += ", CSOSN202_203_ICMS = @v13 ";
                                strCampos += ", CSOSN202_203_ICMS_PROPRIO = @v14 ";
                                strCampos += ", CSOSN202_203_MVA_SAIDA = @v15 ";
                                break;
                            case 900:
                                strCampos += ", CSOSN900_RED_BC_ICMS_ST = @v12 ";
                                strCampos += ", CSOSN900_ICMS = @v13 ";
                                strCampos += ", CSOSN900_ICMS_PROPRIO = @v14 ";
                                strCampos += ", CSOSN900_RED_BC_ICMS_PROPRIO = @v15 ";
                                strCampos += ", CSOSN900_MVA_SAIDA = @v16 ";
                                strCampos += ", CSOSN900_ICMS_SIMPLES = @v17 ";
                                break;
                            default:
                                break;
                        }
                    }
                }

                strSQL = "UPDATE [REGRA_FISCAL_ICMS] SET " + strCampos + " WHERE CD_REGRA_FISCAL_ICMS = @COD_REG";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@COD_REG", p.CodigoRegFisIcms);
                Cmd.Parameters.AddWithValue("@v1", Convert.ToDateTime(p.DataVigencia));
                Cmd.Parameters.AddWithValue("@v2", Convert.ToDateTime(p.DataAtualizacao));
                Cmd.Parameters.AddWithValue("@v3", p.CodSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.Descricao);
                Cmd.Parameters.AddWithValue("@v5", p.MensagemIcms);
                Cmd.Parameters.AddWithValue("@v6", p.CodBeneficioFiscal);
                Cmd.Parameters.AddWithValue("@v7", p.CodHabil_RegTributario);
                Cmd.Parameters.AddWithValue("@v8", p.CodCST_CSOSN);
                Cmd.Parameters.AddWithValue("@v9", p.CodModDetBCIcms);
                Cmd.Parameters.AddWithValue("@v10", p.CodModDetBCIcmsST);
                Cmd.Parameters.AddWithValue("@v11", p.MVAEntrada);
                Cmd.Parameters.AddWithValue("@v111", p.MVAOriginal);
                Cmd.Parameters.AddWithValue("@v1111", p.VlFCP);


                if (p.CodHabil_RegTributario != 0)
                {
                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                    {

                        switch (p.CodCST_CSOSN)
                        {
                            case 00:
                                Cmd.Parameters.AddWithValue("@v12", p.CST00ICMS);
                                break;
                            case 10:
                                Cmd.Parameters.AddWithValue("@v12", p.CST10ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CST10ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST10ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CST10ICMSProprio);
                                Cmd.Parameters.AddWithValue("@V16", p.CST10MVASaida);
                                Cmd.Parameters.AddWithValue("@V17", p.CST10CalculaDifal);
                                break;
                            case 20:
                                Cmd.Parameters.AddWithValue("@v12", p.CST20ReducaoBCICMS);
                                Cmd.Parameters.AddWithValue("@v13", p.CST20ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST20MotDesoneracao);
                                break;
                            case 30:
                                Cmd.Parameters.AddWithValue("@v12", p.CST30ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CST30ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST30ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CST30MVASaida);
                                Cmd.Parameters.AddWithValue("@v16", p.CST30MotDesoneracao);
                                break;
                            case 40:
                                Cmd.Parameters.AddWithValue("@v12", p.CST404150MotDesoneracao);
                                break;
                            case 41:
                                Cmd.Parameters.AddWithValue("@v12", p.CST404150MotDesoneracao);
                                break;
                            case 50:
                                Cmd.Parameters.AddWithValue("@v12", p.CST404150MotDesoneracao);
                                break;
                            case 51:
                                Cmd.Parameters.AddWithValue("@v12", p.CST51ReducaoBCICMS);
                                Cmd.Parameters.AddWithValue("@v13", p.CST51ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST51Diferimento);
                                break;
                            case 70:
                                Cmd.Parameters.AddWithValue("@v12", p.CST70ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CST70ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST70ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CST70ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v16", p.CST70MVASaida);
                                Cmd.Parameters.AddWithValue("@v17", p.CST70MotDesoneracao);
                                break;
                            case 90:
                                Cmd.Parameters.AddWithValue("@v12", p.CST90ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CST90ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CST90ReducaoBCICMSSTProprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CST90ICMSProprio);
                                Cmd.Parameters.AddWithValue("@v16", p.CST90MVASaida);
                                Cmd.Parameters.AddWithValue("@v17", p.CST90CalculaDifal);
                                Cmd.Parameters.AddWithValue("@v18", p.CST90MotDesoneracao);
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
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN101_ICMS_SIMPLES);
                                break;
                            case 201:
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN201_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CSOSN201_ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CSOSN201_MVASaida);
                                Cmd.Parameters.AddWithValue("@v15", p.CSOSN201_ICMS_SIMPLES);
                                break;
                            case 202:
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN202_203_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CSOSN202_203_ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CSOSN202_203_ICMS_PROPRIO);
                                Cmd.Parameters.AddWithValue("@v15", p.CSOSN202_203_MVASaida);
                                break;
                            case 203:
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN202_203_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CSOSN202_203_ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CSOSN202_203_ICMS_PROPRIO);
                                Cmd.Parameters.AddWithValue("@v15", p.CSOSN202_203_MVASaida);
                                break;
                            case 900:
                                Cmd.Parameters.AddWithValue("@v12", p.CSOSN900_ReducaoBCICMSST);
                                Cmd.Parameters.AddWithValue("@v13", p.CSOSN900_ICMS);
                                Cmd.Parameters.AddWithValue("@v14", p.CSOSN900_ICMS_Proprio);
                                Cmd.Parameters.AddWithValue("@v15", p.CSOSN900_ReducaoBCICMSProprio);
                                Cmd.Parameters.AddWithValue("@v16", p.CSOSN900_MVASaida);
                                Cmd.Parameters.AddWithValue("@v17", p.CSOSN900_ICMS_SIMPLES);
                                break;
                            default:
                                break;
                        }
                    }
                }

                Cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Alteração Fiscal Icms Regra Fiscal Icms: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }

        public DataTable ObterRegFisIcms(double CodRegra)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [REGRA_FISCAL_ICMS] WHERE  CD_REGRA_FISCAL_ICMS = " + CodRegra.ToString();

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Obter Regra Fiscal de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }

        public RegFisIcms ExecutaSP_BUSCA_REGRA_ICMS(int intCodigoTipoOperacao, long intCodigoEmpresa, long intCodigoPessoa, int intCodigoAplicacaoUso)
        {
            DataTable dtSP = new DataTable();
            AbrirConexao();
            try
            {
                SqlCommand sqlComand = new SqlCommand("SP_BUSCA_REGRA_ICMS ", Con);

                sqlComand.CommandType = CommandType.StoredProcedure;
                sqlComand.Parameters.AddWithValue("@CodTipoOperacao", intCodigoTipoOperacao);
                sqlComand.Parameters.AddWithValue("@CodEmpresa", intCodigoEmpresa);
                sqlComand.Parameters.AddWithValue("@CodPessoa", intCodigoPessoa);
                sqlComand.Parameters.AddWithValue("@CodAplicacao", intCodigoAplicacaoUso);

                Dr = sqlComand.ExecuteReader();
                RegFisIcms p = null;
                if (Dr.Read())
                {

                    p = new RegFisIcms();

                    p.MensagemIcms = Convert.ToString(Dr["TX_MSG_ICMS"]);
                    p.CodBeneficioFiscal = Convert.ToString(Dr["CD_BNF_FISCAL"]);
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.CodCST_CSOSN = Convert.ToInt32(Dr["CD_CST_CSOSN"]);
                    p.CodModDetBCIcms = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS"]);
                    p.CodModDetBCIcmsST = Convert.ToInt32(Dr["CD_MOD_DET_BC_ICMS_ST"]);
                    p.MVAEntrada = Convert.ToDecimal(Dr["VL_MVA_ENTRADA"]);
                   // p.MVAOriginal = Convert.ToDecimal(Dr["VL_MVA_ORIGINAL"]);

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
                throw new Exception("Erro ao Executar SP_BUSCA_REGRA_ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
