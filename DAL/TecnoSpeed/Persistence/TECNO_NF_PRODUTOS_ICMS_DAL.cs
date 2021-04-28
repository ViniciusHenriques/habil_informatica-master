
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
    public class TECNO_NF_PRODUTOS_ICMS_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_PRODUTOS_ICMS> ListarTECNO_NF_PRODUTOS_ICMS(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_ICMS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_PRODUTOS_ICMS> tcn_nf = new List<TECNO_NF_PRODUTOS_ICMS>();

                while (Dr.Read())
                {
                    TECNO_NF_PRODUTOS_ICMS obj = new TECNO_NF_PRODUTOS_ICMS();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["ICMS_CST"] != DBNull.Value)
                        obj.ICMS_CST = Convert.ToDecimal(Dr["ICMS_CST"]);

                    if (Dr["ICMS_ORIG"] != DBNull.Value)
                        obj.ICMS_ORIG = Convert.ToDecimal(Dr["ICMS_ORIG"]);

                    if (Dr["ICMS_MODBC"] != DBNull.Value)
                        obj.ICMS_MODBC = Convert.ToDecimal(Dr["ICMS_MODBC"]);

                    if (Dr["ICMS_VBC"] != DBNull.Value)
                        obj.ICMS_VBC = Convert.ToDecimal(Dr["ICMS_VBC"]);

                    if (Dr["ICMS_PICMS"] != DBNull.Value)
                        obj.ICMS_PICMS = Convert.ToDecimal(Dr["ICMS_PICMS"]);

                    if (Dr["ICMS_VICMS"] != DBNull.Value)
                        obj.ICMS_VICMS = Convert.ToDecimal(Dr["ICMS_VICMS"]);

                    if (Dr["ICMS_MODBCST"] != DBNull.Value)
                        obj.ICMS_MODBCST = Convert.ToDecimal(Dr["ICMS_MODBCST"]);

                    if (Dr["ICMS_PMVAST"] != DBNull.Value)
                        obj.ICMS_PMVAST = Convert.ToDecimal(Dr["ICMS_PMVAST"]);

                    if (Dr["ICMS_PREDBCST"] != DBNull.Value)
                        obj.ICMS_PREDBCST = Convert.ToDecimal(Dr["ICMS_PREDBCST"]);

                    if (Dr["ICMS_VBCST"] != DBNull.Value)
                        obj.ICMS_VBCST = Convert.ToDecimal(Dr["ICMS_VBCST"]);

                    if (Dr["ICMS_PICMSST"] != DBNull.Value)
                        obj.ICMS_PICMSST = Convert.ToDecimal(Dr["ICMS_PICMSST"]);

                    if (Dr["ICMS_VICMSST"] != DBNull.Value)
                        obj.ICMS_VICMSST = Convert.ToDecimal(Dr["ICMS_VICMSST"]);

                    if (Dr["ICMS_PREDBC"] != DBNull.Value)
                        obj.ICMS_PREDBC = Convert.ToDecimal(Dr["ICMS_PREDBC"]);

                    if (Dr["ICMS_MOTDESICMS"] != DBNull.Value)
                        obj.ICMS_MOTDESICMS = Convert.ToDecimal(Dr["ICMS_MOTDESICMS"]);

                    if (Dr["ICMS_VBCSTRET"] != DBNull.Value)
                        obj.ICMS_VBCSTRET = Convert.ToDecimal(Dr["ICMS_VBCSTRET"]);

                    if (Dr["ICMS_VICMSSTRET"] != DBNull.Value)
                        obj.ICMS_VICMSSTRET = Convert.ToDecimal(Dr["ICMS_VICMSSTRET"]);

                    if (Dr["ICMS_ICMSPART"] != DBNull.Value)
                        obj.ICMS_ICMSPART = Convert.ToDecimal(Dr["ICMS_ICMSPART"]);

                    if (Dr["ICMS_PBCOP"] != DBNull.Value)
                        obj.ICMS_PBCOP = Convert.ToDecimal(Dr["ICMS_PBCOP"]);

                    if (Dr["ICMS_UFST"] != DBNull.Value)
                        obj.ICMS_UFST = Convert.ToString(Dr["ICMS_UFST"]);

                    if (Dr["ICMS_ICMSST"] != DBNull.Value)
                        obj.ICMS_ICMSST = Convert.ToDecimal(Dr["ICMS_ICMSST"]);

                    if (Dr["ICMS_VBCSTDEST"] != DBNull.Value)
                        obj.ICMS_VBCSTDEST = Convert.ToDecimal(Dr["ICMS_VBCSTDEST"]);

                    if (Dr["ICMS_VICMSSTDEST"] != DBNull.Value)
                        obj.ICMS_VICMSSTDEST = Convert.ToDecimal(Dr["ICMS_VICMSSTDEST"]);

                    if (Dr["ICMS_CSOSN"] != DBNull.Value)
                        obj.ICMS_CSOSN = Convert.ToDecimal(Dr["ICMS_CSOSN"]);

                    if (Dr["ICMS_PCREDSN"] != DBNull.Value)
                        obj.ICMS_PCREDSN = Convert.ToDecimal(Dr["ICMS_PCREDSN"]);

                    if (Dr["ICMS_VCREDICMSSN"] != DBNull.Value)
                        obj.ICMS_VCREDICMSSN = Convert.ToDecimal(Dr["ICMS_VCREDICMSSN"]);

                    if (Dr["ICMS_VICMSDESON"] != DBNull.Value)
                        obj.ICMS_VICMSDESON = Convert.ToDecimal(Dr["ICMS_VICMSDESON"]);

                    if (Dr["ICMS_VICMSOP"] != DBNull.Value)
                        obj.ICMS_VICMSOP = Convert.ToDecimal(Dr["ICMS_VICMSOP"]);

                    if (Dr["ICMS_PDIF"] != DBNull.Value)
                        obj.ICMS_PDIF = Convert.ToDecimal(Dr["ICMS_PDIF"]);

                    if (Dr["ICMS_VICMSDIF"] != DBNull.Value)
                        obj.ICMS_VICMSDIF = Convert.ToDecimal(Dr["ICMS_VICMSDIF"]);

                    if (Dr["BO_MONTAR_VICMS"] != DBNull.Value)
                        obj.BO_MONTAR_VICMS = Convert.ToString(Dr["BO_MONTAR_VICMS"]);

                    if (Dr["ICMS_VBCFCP"] != DBNull.Value)
                        obj.ICMS_VBCFCP = Convert.ToDecimal(Dr["ICMS_VBCFCP"]);

                    if (Dr["ICMS_PFCP"] != DBNull.Value)
                        obj.ICMS_PFCP = Convert.ToDecimal(Dr["ICMS_PFCP"]);

                    if (Dr["ICMS_VFCP"] != DBNull.Value)
                        obj.ICMS_VFCP = Convert.ToDecimal(Dr["ICMS_VFCP"]);

                    if (Dr["ICMS_VBCFCPST"] != DBNull.Value)
                        obj.ICMS_VBCFCPST = Convert.ToDecimal(Dr["ICMS_VBCFCPST"]);

                    if (Dr["ICMS_PFCPST"] != DBNull.Value)
                        obj.ICMS_PFCPST = Convert.ToDecimal(Dr["ICMS_PFCPST"]);

                    if (Dr["ICMS_VFCPST"] != DBNull.Value)
                        obj.ICMS_VFCPST = Convert.ToDecimal(Dr["ICMS_VFCPST"]);

                    if (Dr["ICMS_VBCFCPSTRET"] != DBNull.Value)
                        obj.ICMS_VBCFCPSTRET = Convert.ToDecimal(Dr["ICMS_VBCFCPSTRET"]);

                    if (Dr["ICMS_PFCPSTRET"] != DBNull.Value)
                        obj.ICMS_PFCPSTRET = Convert.ToDecimal(Dr["ICMS_PFCPSTRET"]);

                    if (Dr["ICMS_VFCPSTRET"] != DBNull.Value)
                        obj.ICMS_VFCPSTRET = Convert.ToDecimal(Dr["ICMS_VFCPSTRET"]);

                    if (Dr["ICMS_PST"] != DBNull.Value)
                        obj.ICMS_PST = Convert.ToDecimal(Dr["ICMS_PST"]);

                    if (Dr["ICMS_REDBCEFET"] != DBNull.Value)
                        obj.ICMS_REDBCEFET = Convert.ToDecimal(Dr["ICMS_REDBCEFET"]);

                    if (Dr["ICMS_VBCEFET"] != DBNull.Value)
                        obj.ICMS_VBCEFET = Convert.ToDecimal(Dr["ICMS_VBCEFET"]);

                    if (Dr["ICMS_PEFET"] != DBNull.Value)
                        obj.ICMS_PEFET = Convert.ToDecimal(Dr["ICMS_PEFET"]);

                    if (Dr["ICMS_VEFET"] != DBNull.Value)
                        obj.ICMS_VEFET = Convert.ToDecimal(Dr["ICMS_VEFET"]);

                    if (Dr["ICMS_VICMSSUBSTITUTO"] != DBNull.Value)
                        obj.ICMS_VICMSSUBSTITUTO = Convert.ToDecimal(Dr["ICMS_VICMSSUBSTITUTO"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_PRODUTOS_ICMS PesquisarTECNO_NF_PRODUTOS_ICMS(decimal decID_NOTA_FISCAL, decimal decPROD_NITEM)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_ICMS Where ID_NOTA_FISCAL = @v1 and PROD_NITEM = @v2";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);
                Cmd.Parameters.AddWithValue("@v2", decPROD_NITEM);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_ICMS obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_ICMS();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["ICMS_CST"] != DBNull.Value)
                        obj.ICMS_CST = Convert.ToDecimal(Dr["ICMS_CST"]);

                    if (Dr["ICMS_ORIG"] != DBNull.Value)
                        obj.ICMS_ORIG = Convert.ToDecimal(Dr["ICMS_ORIG"]);

                    if (Dr["ICMS_MODBC"] != DBNull.Value)
                        obj.ICMS_MODBC = Convert.ToDecimal(Dr["ICMS_MODBC"]);

                    if (Dr["ICMS_VBC"] != DBNull.Value)
                        obj.ICMS_VBC = Convert.ToDecimal(Dr["ICMS_VBC"]);

                    if (Dr["ICMS_PICMS"] != DBNull.Value)
                        obj.ICMS_PICMS = Convert.ToDecimal(Dr["ICMS_PICMS"]);

                    if (Dr["ICMS_VICMS"] != DBNull.Value)
                        obj.ICMS_VICMS = Convert.ToDecimal(Dr["ICMS_VICMS"]);

                    if (Dr["ICMS_MODBCST"] != DBNull.Value)
                        obj.ICMS_MODBCST = Convert.ToDecimal(Dr["ICMS_MODBCST"]);

                    if (Dr["ICMS_PMVAST"] != DBNull.Value)
                        obj.ICMS_PMVAST = Convert.ToDecimal(Dr["ICMS_PMVAST"]);

                    if (Dr["ICMS_PREDBCST"] != DBNull.Value)
                        obj.ICMS_PREDBCST = Convert.ToDecimal(Dr["ICMS_PREDBCST"]);

                    if (Dr["ICMS_VBCST"] != DBNull.Value)
                        obj.ICMS_VBCST = Convert.ToDecimal(Dr["ICMS_VBCST"]);

                    if (Dr["ICMS_PICMSST"] != DBNull.Value)
                        obj.ICMS_PICMSST = Convert.ToDecimal(Dr["ICMS_PICMSST"]);

                    if (Dr["ICMS_VICMSST"] != DBNull.Value)
                        obj.ICMS_VICMSST = Convert.ToDecimal(Dr["ICMS_VICMSST"]);

                    if (Dr["ICMS_PREDBC"] != DBNull.Value)
                        obj.ICMS_PREDBC = Convert.ToDecimal(Dr["ICMS_PREDBC"]);

                    if (Dr["ICMS_MOTDESICMS"] != DBNull.Value)
                        obj.ICMS_MOTDESICMS = Convert.ToDecimal(Dr["ICMS_MOTDESICMS"]);

                    if (Dr["ICMS_VBCSTRET"] != DBNull.Value)
                        obj.ICMS_VBCSTRET = Convert.ToDecimal(Dr["ICMS_VBCSTRET"]);

                    if (Dr["ICMS_VICMSSTRET"] != DBNull.Value)
                        obj.ICMS_VICMSSTRET = Convert.ToDecimal(Dr["ICMS_VICMSSTRET"]);

                    if (Dr["ICMS_ICMSPART"] != DBNull.Value)
                        obj.ICMS_ICMSPART = Convert.ToDecimal(Dr["ICMS_ICMSPART"]);

                    if (Dr["ICMS_PBCOP"] != DBNull.Value)
                        obj.ICMS_PBCOP = Convert.ToDecimal(Dr["ICMS_PBCOP"]);

                    if (Dr["ICMS_UFST"] != DBNull.Value)
                        obj.ICMS_UFST = Convert.ToString(Dr["ICMS_UFST"]);

                    if (Dr["ICMS_ICMSST"] != DBNull.Value)
                        obj.ICMS_ICMSST = Convert.ToDecimal(Dr["ICMS_ICMSST"]);

                    if (Dr["ICMS_VBCSTDEST"] != DBNull.Value)
                        obj.ICMS_VBCSTDEST = Convert.ToDecimal(Dr["ICMS_VBCSTDEST"]);

                    if (Dr["ICMS_VICMSSTDEST"] != DBNull.Value)
                        obj.ICMS_VICMSSTDEST = Convert.ToDecimal(Dr["ICMS_VICMSSTDEST"]);

                    if (Dr["ICMS_CSOSN"] != DBNull.Value)
                        obj.ICMS_CSOSN = Convert.ToDecimal(Dr["ICMS_CSOSN"]);

                    if (Dr["ICMS_PCREDSN"] != DBNull.Value)
                        obj.ICMS_PCREDSN = Convert.ToDecimal(Dr["ICMS_PCREDSN"]);

                    if (Dr["ICMS_VCREDICMSSN"] != DBNull.Value)
                        obj.ICMS_VCREDICMSSN = Convert.ToDecimal(Dr["ICMS_VCREDICMSSN"]);

                    if (Dr["ICMS_VICMSDESON"] != DBNull.Value)
                        obj.ICMS_VICMSDESON = Convert.ToDecimal(Dr["ICMS_VICMSDESON"]);

                    if (Dr["ICMS_VICMSOP"] != DBNull.Value)
                        obj.ICMS_VICMSOP = Convert.ToDecimal(Dr["ICMS_VICMSOP"]);

                    if (Dr["ICMS_PDIF"] != DBNull.Value)
                        obj.ICMS_PDIF = Convert.ToDecimal(Dr["ICMS_PDIF"]);

                    if (Dr["ICMS_VICMSDIF"] != DBNull.Value)
                        obj.ICMS_VICMSDIF = Convert.ToDecimal(Dr["ICMS_VICMSDIF"]);

                    if (Dr["BO_MONTAR_VICMS"] != DBNull.Value)
                        obj.BO_MONTAR_VICMS = Convert.ToString(Dr["BO_MONTAR_VICMS"]);

                    if (Dr["ICMS_VBCFCP"] != DBNull.Value)
                        obj.ICMS_VBCFCP = Convert.ToDecimal(Dr["ICMS_VBCFCP"]);

                    if (Dr["ICMS_PFCP"] != DBNull.Value)
                        obj.ICMS_PFCP = Convert.ToDecimal(Dr["ICMS_PFCP"]);

                    if (Dr["ICMS_VFCP"] != DBNull.Value)
                        obj.ICMS_VFCP = Convert.ToDecimal(Dr["ICMS_VFCP"]);

                    if (Dr["ICMS_VBCFCPST"] != DBNull.Value)
                        obj.ICMS_VBCFCPST = Convert.ToDecimal(Dr["ICMS_VBCFCPST"]);

                    if (Dr["ICMS_PFCPST"] != DBNull.Value)
                        obj.ICMS_PFCPST = Convert.ToDecimal(Dr["ICMS_PFCPST"]);

                    if (Dr["ICMS_VFCPST"] != DBNull.Value)
                        obj.ICMS_VFCPST = Convert.ToDecimal(Dr["ICMS_VFCPST"]);

                    if (Dr["ICMS_VBCFCPSTRET"] != DBNull.Value)
                        obj.ICMS_VBCFCPSTRET = Convert.ToDecimal(Dr["ICMS_VBCFCPSTRET"]);

                    if (Dr["ICMS_PFCPSTRET"] != DBNull.Value)
                        obj.ICMS_PFCPSTRET = Convert.ToDecimal(Dr["ICMS_PFCPSTRET"]);

                    if (Dr["ICMS_VFCPSTRET"] != DBNull.Value)
                        obj.ICMS_VFCPSTRET = Convert.ToDecimal(Dr["ICMS_VFCPSTRET"]);

                    if (Dr["ICMS_PST"] != DBNull.Value)
                        obj.ICMS_PST = Convert.ToDecimal(Dr["ICMS_PST"]);

                    if (Dr["ICMS_REDBCEFET"] != DBNull.Value)
                        obj.ICMS_REDBCEFET = Convert.ToDecimal(Dr["ICMS_REDBCEFET"]);

                    if (Dr["ICMS_VBCEFET"] != DBNull.Value)
                        obj.ICMS_VBCEFET = Convert.ToDecimal(Dr["ICMS_VBCEFET"]);

                    if (Dr["ICMS_PEFET"] != DBNull.Value)
                        obj.ICMS_PEFET = Convert.ToDecimal(Dr["ICMS_PEFET"]);

                    if (Dr["ICMS_VEFET"] != DBNull.Value)
                        obj.ICMS_VEFET = Convert.ToDecimal(Dr["ICMS_VEFET"]);

                    if (Dr["ICMS_VICMSSUBSTITUTO"] != DBNull.Value)
                        obj.ICMS_VICMSSUBSTITUTO = Convert.ToDecimal(Dr["ICMS_VICMSSUBSTITUTO"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro a PESQUISAR TECNO_NF_PRODUTOS_ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
    }
}

