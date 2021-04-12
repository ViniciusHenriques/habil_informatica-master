 using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class RegraFisIPIDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(RegFisIPI p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into REGRA_FISCAL_IPI (DT_VIGENCIA, DT_ATUALIZACAO, CD_NCM, DS_NCM, VL_PCT_IPI, " +
                    "CD_ENQUADRAMENTO, CD_TIPO, CD_SITUACAO, CD_EX, CD_CST_IPI ) values (@v1, getDate(), @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DtVigencia);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoNCM);
                Cmd.Parameters.AddWithValue("@v4", p.DescricaoNCM);
                Cmd.Parameters.AddWithValue("@v5", p.PercentualIPI);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoEnquadramento);
                Cmd.Parameters.AddWithValue("@v7", p.CodigoTipo);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoEx);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoSituacaoTributaria);
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
                            throw new Exception("Erro ao Incluir COFINS: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Regra Fiscal IPI: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(RegFisIPI p)
        {
            try
            {
                AbrirConexao();

                strSQL = "update [REGRA_FISCAL_IPI] set [DT_VIGENCIA] = @v2, [DT_ATUALIZACAO] = GetDate() , [CD_NCM] = @v4, " +
                    "[DS_NCM] = @v5, [VL_PCT_IPI] = @v6, [CD_ENQUADRAMENTO] = @v7, [CD_TIPO] = @v8, [CD_SITUACAO] = @v9, [CD_EX] = @v10, " +
                    " [CD_CST_IPI] = @v11 Where [CD_REGRA_FISCAL_IPI] = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoRegraFiscalIPI);
                Cmd.Parameters.AddWithValue("@v2", p.DtVigencia);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoNCM);
                Cmd.Parameters.AddWithValue("@v5", p.DescricaoNCM);
                Cmd.Parameters.AddWithValue("@v6", p.PercentualIPI);
                Cmd.Parameters.AddWithValue("@v7", p.CodigoEnquadramento);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoTipo);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoEx);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoSituacaoTributaria);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Regra Fiscal IPI: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(decimal DecCodigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [REGRA_FISCAL_IPI] Where [CD_REGRA_FISCAL_IPI] = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", DecCodigo);

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
                            throw new Exception("Erro ao excluir Regra Fiscal IPI: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Regra Fiscal IPI: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIPI PesquisarIPI(DateTime DtVigencia, string strCodIPI, string strCodEx, decimal intCodRegra)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [REGRA_FISCAL_IPI] where [CD_NCM] = '" + strCodIPI + "' " +
                    " and [DT_VIGENCIA] = @v2 and [CD_EX] = '" + strCodEx + "' ";
                if (intCodRegra > 0)
                    strSQL += "AND [CD_REGRA_FISCAL_IPI] <> " + intCodRegra;

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v2", Convert.ToDateTime(DtVigencia.ToString("dd/MM/yyyy 00:00:00")));
                Dr = Cmd.ExecuteReader();

                RegFisIPI p = new RegFisIPI();

                if (Dr.Read())
                {
                    p.CodigoRegraFiscalIPI = Convert.ToInt32(Dr["CD_REGRA_FISCAL_IPI"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                    p.CodigoEx = Convert.ToString(Dr["CD_EX"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Regra Fiscal IPI: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIPI PesquisarIPIPorRegra(decimal intCodigoRegraFiscalIPI)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [REGRA_FISCAL_IPI] where [CD_REGRA_FISCAL_IPI] = " + intCodigoRegraFiscalIPI;

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                RegFisIPI p = new RegFisIPI();

                if (Dr.Read())
                {
                    p.CodigoRegraFiscalIPI = Convert.ToInt32(Dr["CD_REGRA_FISCAL_IPI"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                    p.DescricaoNCM = Convert.ToString(Dr["DS_NCM"]);
                    p.CodigoEx = Convert.ToString(Dr["CD_EX"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.PercentualIPI = Convert.ToDecimal(Dr["VL_PCT_IPI"]);
                    p.CodigoEnquadramento = Convert.ToInt16(Dr["CD_ENQUADRAMENTO"]);
                    p.DtAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.DtVigencia = Convert.ToDateTime(Dr["DT_VIGENCIA"]);
                    p.CodigoSituacaoTributaria = Convert.ToInt16(Dr["CD_CST_IPI"]);

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Regra Fiscal IPI: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegFisIPI> ListarRegrasIPI(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();

                string strValor = "";
                string strSQL = "select EM.CD_REGRA_FISCAL_IPI, EM.CD_CST_IPI ,EM.CD_NCM ,EM.DS_NCM ,EM.CD_SITUACAO ,EM.CD_TIPO " +
                    ",EM.VL_PCT_IPI ,EM.CD_ENQUADRAMENTO ,EM.DT_ATUALIZACAO ,EM.DT_VIGENCIA, EM.CD_EX, H.DS_TIPO AS DS_TIPO_TRIBUTACAO, HT.DS_TIPO AS DS_TIPO_SITUACAO" +
                    "  from [REGRA_FISCAL_IPI] AS Em " +
                    "INNER JOIN HABIL_TIPO AS HT " +
                    "ON HT.CD_TIPO = EM.CD_SITUACAO " +
                    "INNER JOIN HABIL_TIPO AS H " +
                    "ON H.CD_TIPO = EM.CD_TIPO";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<RegFisIPI> lista = new List<RegFisIPI>();

                while (Dr.Read())
                {
                    RegFisIPI p = new RegFisIPI();

                    p.CodigoRegraFiscalIPI = Convert.ToInt32(Dr["CD_REGRA_FISCAL_IPI"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                    p.DescricaoNCM = Convert.ToString(Dr["DS_NCM"]);
                    p.CodigoEx = Convert.ToString(Dr["CD_EX"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.PercentualIPI = Convert.ToDecimal(Dr["VL_PCT_IPI"]);
                    p.CodigoEnquadramento = Convert.ToInt16(Dr["CD_ENQUADRAMENTO"]);
                    p.DtAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.DtVigencia = Convert.ToDateTime(Dr["DT_VIGENCIA"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_TIPO_SITUACAO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO_TRIBUTACAO"]);
                    p.CodigoSituacaoTributaria = Convert.ToInt16(Dr["CD_CST_IPI"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Embalagens: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelRegraIPI(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                DataTable dt = new DataTable();
                string strValor = "";
                strSQL = "select EM.CD_REGRA_FISCAL_IPI ,EM.CD_NCM ,EM.DS_NCM, EM.CD_CST_IPI ,EM.CD_SITUACAO ,EM.CD_TIPO " +
                    ",EM.VL_PCT_IPI ,EM.CD_ENQUADRAMENTO ,EM.DT_ATUALIZACAO ,EM.DT_VIGENCIA, EM.CD_EX, H.DS_TIPO AS DS_TIPO_TRIBUTACAO, HT.DS_TIPO AS DS_TIPO_SITUACAO" +
                    "  from [REGRA_FISCAL_IPI] AS Em " +
                    "INNER JOIN HABIL_TIPO AS HT " +
                    "ON HT.CD_TIPO = EM.CD_SITUACAO " +
                    "INNER JOIN HABIL_TIPO AS H " +
                    "ON H.CD_TIPO = EM.CD_TIPO";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar DataSet: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegFisIPI> ListarRegrasParaProduto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();

                string strValor = "";
                string strSQL = "select EM.CD_REGRA_FISCAL_IPI, EM.CD_CST_IPI ,EM.CD_NCM ,EM.DS_NCM ,EM.CD_SITUACAO ,EM.CD_TIPO " +
                    ",EM.VL_PCT_IPI ,EM.CD_ENQUADRAMENTO ,EM.DT_ATUALIZACAO ,EM.DT_VIGENCIA, EM.CD_EX, H.DS_TIPO AS DS_TIPO_TRIBUTACAO, HT.DS_TIPO AS DS_TIPO_SITUACAO" +
                    "  from [REGRA_FISCAL_IPI] AS Em " +
                    "INNER JOIN HABIL_TIPO AS HT " +
                    "ON HT.CD_TIPO = EM.CD_SITUACAO " +
                    "INNER JOIN HABIL_TIPO AS H " +
                    "ON H.CD_TIPO = EM.CD_TIPO " +
                    "where CD_SITUACAO = 1 ";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<RegFisIPI> lista = new List<RegFisIPI>();

                while (Dr.Read())
                {
                    RegFisIPI p = new RegFisIPI();

                    p.CodigoRegraFiscalIPI = Convert.ToInt32(Dr["CD_REGRA_FISCAL_IPI"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                    p.DescricaoNCM = Convert.ToString(Dr["DS_NCM"]);
                    p.CodigoEx = Convert.ToString(Dr["CD_EX"]);
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.PercentualIPI = Convert.ToDecimal(Dr["VL_PCT_IPI"]);
                    p.CodigoEnquadramento = Convert.ToInt16(Dr["CD_ENQUADRAMENTO"]);
                    p.DtVigencia = Convert.ToDateTime(Dr["DT_VIGENCIA"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_TIPO_SITUACAO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO_TRIBUTACAO"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Embalagens: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIPI PesquisarIPIPorCodNcm(string strCodNcm)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [REGRA_FISCAL_IPI] where [CD_NCM] = '" + strCodNcm + "' ";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                RegFisIPI p = new RegFisIPI();

                if (Dr.Read())
                {
                    p.CodigoRegraFiscalIPI = Convert.ToInt32(Dr["CD_REGRA_FISCAL_IPI"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                    p.DescricaoNCM = Convert.ToString(Dr["DS_NCM"]);
                    p.CodigoEx = Convert.ToString(Dr["CD_EX"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.PercentualIPI = Convert.ToDecimal(Dr["VL_PCT_IPI"]);
                    p.CodigoEnquadramento = Convert.ToInt16(Dr["CD_ENQUADRAMENTO"]);
                    p.DtAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.DtVigencia = Convert.ToDateTime(Dr["DT_VIGENCIA"]);
                    p.CodigoSituacaoTributaria = Convert.ToInt16(Dr["CD_CST_IPI"]);

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Regra Fiscal IPI: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegFisIPI> ListarIPIPorCodNcm(string strCodNcm, DateTime DtData)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select EM.CD_REGRA_FISCAL_IPI, EM.CD_CST_IPI ,EM.CD_NCM ,EM.DS_NCM ,EM.CD_SITUACAO ,EM.CD_TIPO " +
                    ",EM.VL_PCT_IPI ,EM.CD_ENQUADRAMENTO ,EM.DT_ATUALIZACAO ,EM.DT_VIGENCIA, EM.CD_EX, H.DS_TIPO AS DS_TIPO_TRIBUTACAO, HT.DS_TIPO AS DS_TIPO_SITUACAO" +
                    "  from [REGRA_FISCAL_IPI] AS Em " +
                    "INNER JOIN HABIL_TIPO AS HT " +
                    "ON HT.CD_TIPO = EM.CD_SITUACAO " +
                    "INNER JOIN HABIL_TIPO AS H " +
                    "ON H.CD_TIPO = EM.CD_TIPO ";
                if (strCodNcm != "")
                    strSQL += " where [CD_NCM] = '" + strCodNcm + "' and DT_VIGENCIA = '" + DtData.ToString("yyyy/MM/dd") + "' ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<RegFisIPI> lista = new List<RegFisIPI>();

                while (Dr.Read())
                {
                    RegFisIPI p = new RegFisIPI();

                    p.CodigoRegraFiscalIPI = Convert.ToInt32(Dr["CD_REGRA_FISCAL_IPI"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                    p.DescricaoNCM = Convert.ToString(Dr["DS_NCM"]);
                    p.CodigoEx = Convert.ToString(Dr["CD_EX"]);
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.PercentualIPI = Convert.ToDecimal(Dr["VL_PCT_IPI"]);
                    p.CodigoEnquadramento = Convert.ToInt16(Dr["CD_ENQUADRAMENTO"]);
                    p.DtVigencia = Convert.ToDateTime(Dr["DT_VIGENCIA"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_TIPO_SITUACAO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO_TRIBUTACAO"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Embalagens: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
