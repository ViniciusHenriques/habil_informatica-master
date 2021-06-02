
using System.IO;
using System.Linq;
using System.Text;
using DAL.Model;
using DAL.Persistence;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DAL.TecnoSpeed.Persistence
{
    public class TECNO_INUTILIZACAO_ERP_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_INUTILIZACAO_ERP> ListarTECNO_INUTILIZACAO_ERP(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_INUTILIZACAO_ERP Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_INUTILIZACAO_ERP> tcn_nf = new List<TECNO_INUTILIZACAO_ERP>();

                while (Dr.Read())
                {
                    TECNO_INUTILIZACAO_ERP obj = new TECNO_INUTILIZACAO_ERP();

                    if (Dr["ID_INUTILIZACAO_ERP"] != DBNull.Value)
                        obj.ID_INUTILIZACAO_ERP = Convert.ToDecimal(Dr["ID_INUTILIZACAO_ERP"]);

                    if (Dr["CUF"] != DBNull.Value)
                        obj.CUF = Convert.ToDecimal(Dr["CUF"]);

                    if (Dr["ANO"] != DBNull.Value)
                        obj.ANO = Convert.ToDecimal(Dr["ANO"]);

                    if (Dr["CNPJ"] != DBNull.Value)
                        obj.CNPJ = Convert.ToString(Dr["CNPJ"]);

                    if (Dr["MOD"] != DBNull.Value)
                        obj.MOD = Convert.ToDecimal(Dr["MOD"]);

                    if (Dr["SERIE"] != DBNull.Value)
                        obj.SERIE = Convert.ToDecimal(Dr["SERIE"]);

                    if (Dr["NNFINI"] != DBNull.Value)
                        obj.NNFINI = Convert.ToDecimal(Dr["NNFINI"]);

                    if (Dr["NNFFIM"] != DBNull.Value)
                        obj.NNFFIM = Convert.ToDecimal(Dr["NNFFIM"]);

                    if (Dr["XJUST"] != DBNull.Value)
                        obj.XJUST = Convert.ToString(Dr["XJUST"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_INUTILIZACAO_ERP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_INUTILIZACAO_ERP PesquisarTECNO_INUTILIZACAO_ERP(string strID_INUTILIZACAO_ERP)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_INUTILIZACAO_ERP Where ID_INUTILIZACAO_ERP = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_INUTILIZACAO_ERP);

                Dr = Cmd.ExecuteReader();

                TECNO_INUTILIZACAO_ERP obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_INUTILIZACAO_ERP();

                    if (Dr["ID_INUTILIZACAO_ERP"] != DBNull.Value)
                        obj.ID_INUTILIZACAO_ERP = Convert.ToDecimal(Dr["ID_INUTILIZACAO_ERP"]);

                    if (Dr["CUF"] != DBNull.Value)
                        obj.CUF = Convert.ToDecimal(Dr["CUF"]);

                    if (Dr["ANO"] != DBNull.Value)
                        obj.ANO = Convert.ToDecimal(Dr["ANO"]);

                    if (Dr["CNPJ"] != DBNull.Value)
                        obj.CNPJ = Convert.ToString(Dr["CNPJ"]);

                    if (Dr["MOD"] != DBNull.Value)
                        obj.MOD = Convert.ToDecimal(Dr["MOD"]);

                    if (Dr["SERIE"] != DBNull.Value)
                        obj.SERIE = Convert.ToDecimal(Dr["SERIE"]);

                    if (Dr["NNFINI"] != DBNull.Value)
                        obj.NNFINI = Convert.ToDecimal(Dr["NNFINI"]);

                    if (Dr["NNFFIM"] != DBNull.Value)
                        obj.NNFFIM = Convert.ToDecimal(Dr["NNFFIM"]);

                    if (Dr["XJUST"] != DBNull.Value)
                        obj.XJUST = Convert.ToString(Dr["XJUST"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_INUTILIZACAO_ERP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}

