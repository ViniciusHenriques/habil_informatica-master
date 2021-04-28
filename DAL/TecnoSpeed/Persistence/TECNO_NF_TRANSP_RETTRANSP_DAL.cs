
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
    public class TECNO_NF_TRANSP_RETTRANSP_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_TRANSP_RETTRANSP> ListarTECNO_NF_TRANSP_RETTRANSP(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_RETTRANSP Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_TRANSP_RETTRANSP> tcn_nf = new List<TECNO_NF_TRANSP_RETTRANSP>();

                while (Dr.Read())
                {
                    TECNO_NF_TRANSP_RETTRANSP obj = new TECNO_NF_TRANSP_RETTRANSP();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["RETTRANSP_VSERV"] != DBNull.Value)
                        obj.RETTRANSP_VSERV = Convert.ToDecimal(Dr["RETTRANSP_VSERV"]);

                    if (Dr["RETTRANSP_VBCRET"] != DBNull.Value)
                        obj.RETTRANSP_VBCRET = Convert.ToDecimal(Dr["RETTRANSP_VBCRET"]);

                    if (Dr["RETTRANSP_PICMSRET"] != DBNull.Value)
                        obj.RETTRANSP_PICMSRET = Convert.ToDecimal(Dr["RETTRANSP_PICMSRET"]);

                    if (Dr["RETTRANSP_VICMSRET"] != DBNull.Value)
                        obj.RETTRANSP_VICMSRET = Convert.ToDecimal(Dr["RETTRANSP_VICMSRET"]);

                    if (Dr["RETTRANSP_CFOP"] != DBNull.Value)
                        obj.RETTRANSP_CFOP = Convert.ToDecimal(Dr["RETTRANSP_CFOP"]);

                    if (Dr["RETTRANSP_CMUNFG"] != DBNull.Value)
                        obj.RETTRANSP_CMUNFG = Convert.ToDecimal(Dr["RETTRANSP_CMUNFG"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_RETTRANSP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_TRANSP_RETTRANSP PesquisarTECNO_NF_TRANSP_RETTRANSP(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_RETTRANSP Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_TRANSP_RETTRANSP obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_TRANSP_RETTRANSP();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["RETTRANSP_VSERV"] != DBNull.Value)
                        obj.RETTRANSP_VSERV = Convert.ToDecimal(Dr["RETTRANSP_VSERV"]);

                    if (Dr["RETTRANSP_VBCRET"] != DBNull.Value)
                        obj.RETTRANSP_VBCRET = Convert.ToDecimal(Dr["RETTRANSP_VBCRET"]);

                    if (Dr["RETTRANSP_PICMSRET"] != DBNull.Value)
                        obj.RETTRANSP_PICMSRET = Convert.ToDecimal(Dr["RETTRANSP_PICMSRET"]);

                    if (Dr["RETTRANSP_VICMSRET"] != DBNull.Value)
                        obj.RETTRANSP_VICMSRET = Convert.ToDecimal(Dr["RETTRANSP_VICMSRET"]);

                    if (Dr["RETTRANSP_CFOP"] != DBNull.Value)
                        obj.RETTRANSP_CFOP = Convert.ToDecimal(Dr["RETTRANSP_CFOP"]);

                    if (Dr["RETTRANSP_CMUNFG"] != DBNull.Value)
                        obj.RETTRANSP_CMUNFG = Convert.ToDecimal(Dr["RETTRANSP_CMUNFG"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_RETTRANSP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
