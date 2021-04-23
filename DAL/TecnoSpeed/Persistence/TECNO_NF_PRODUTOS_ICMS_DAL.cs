
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
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.ICMS_CST = Convert.ToDecimal(Dr["ICMS_CST"]);
                    obj.ICMS_ORIG = Convert.ToDecimal(Dr["ICMS_ORIG"]);
                    obj.ICMS_MODBC = Convert.ToDecimal(Dr["ICMS_MODBC"]);
                    obj.ICMS_VBC = Convert.ToDecimal(Dr["ICMS_VBC"]);
                    obj.ICMS_PICMS = Convert.ToDecimal(Dr["ICMS_PICMS"]);
                    obj.ICMS_VICMS = Convert.ToDecimal(Dr["ICMS_VICMS"]);
                    obj.ICMS_MODBCST = Convert.ToDecimal(Dr["ICMS_MODBCST"]);
                    obj.ICMS_PMVAST = Convert.ToDecimal(Dr["ICMS_PMVAST"]);
                    obj.ICMS_PREDBCST = Convert.ToDecimal(Dr["ICMS_PREDBCST"]);
                    obj.ICMS_VBCST = Convert.ToDecimal(Dr["ICMS_VBCST"]);
                    obj.ICMS_PICMSST = Convert.ToDecimal(Dr["ICMS_PICMSST"]);
                    obj.ICMS_VICMSST = Convert.ToDecimal(Dr["ICMS_VICMSST"]);
                    obj.ICMS_PREDBC = Convert.ToDecimal(Dr["ICMS_PREDBC"]);
                    obj.ICMS_MOTDESICMS = Convert.ToDecimal(Dr["ICMS_MOTDESICMS"]);
                    obj.ICMS_VBCSTRET = Convert.ToDecimal(Dr["ICMS_VBCSTRET"]);
                    obj.ICMS_VICMSSTRET = Convert.ToDecimal(Dr["ICMS_VICMSSTRET"]);
                    obj.ICMS_ICMSPART = Convert.ToDecimal(Dr["ICMS_ICMSPART"]);
                    obj.ICMS_PBCOP = Convert.ToDecimal(Dr["ICMS_PBCOP"]);
                    obj.ICMS_UFST = Convert.ToString(Dr["ICMS_UFST"]);
                    obj.ICMS_ICMSST = Convert.ToDecimal(Dr["ICMS_ICMSST"]);
                    obj.ICMS_VBCSTDEST = Convert.ToDecimal(Dr["ICMS_VBCSTDEST"]);
                    obj.ICMS_VICMSSTDEST = Convert.ToDecimal(Dr["ICMS_VICMSSTDEST"]);
                    obj.ICMS_CSOSN = Convert.ToDecimal(Dr["ICMS_CSOSN"]);
                    obj.ICMS_PCREDSN = Convert.ToDecimal(Dr["ICMS_PCREDSN"]);
                    obj.ICMS_VCREDICMSSN = Convert.ToDecimal(Dr["ICMS_VCREDICMSSN"]);
                    obj.ICMS_VICMSDESON = Convert.ToDecimal(Dr["ICMS_VICMSDESON"]);
                    obj.ICMS_VICMSOP = Convert.ToDecimal(Dr["ICMS_VICMSOP"]);
                    obj.ICMS_PDIF = Convert.ToDecimal(Dr["ICMS_PDIF"]);
                    obj.ICMS_VICMSDIF = Convert.ToDecimal(Dr["ICMS_VICMSDIF"]);
                    obj.BO_MONTAR_VICMS = Convert.ToString(Dr["BO_MONTAR_VICMS"]);
                    obj.ICMS_VBCFCP = Convert.ToDecimal(Dr["ICMS_VBCFCP"]);
                    obj.ICMS_PFCP = Convert.ToDecimal(Dr["ICMS_PFCP"]);
                    obj.ICMS_VFCP = Convert.ToDecimal(Dr["ICMS_VFCP"]);
                    obj.ICMS_VBCFCPST = Convert.ToDecimal(Dr["ICMS_VBCFCPST"]);
                    obj.ICMS_PFCPST = Convert.ToDecimal(Dr["ICMS_PFCPST"]);
                    obj.ICMS_VFCPST = Convert.ToDecimal(Dr["ICMS_VFCPST"]);
                    obj.ICMS_VBCFCPSTRET = Convert.ToDecimal(Dr["ICMS_VBCFCPSTRET"]);
                    obj.ICMS_PFCPSTRET = Convert.ToDecimal(Dr["ICMS_PFCPSTRET"]);
                    obj.ICMS_VFCPSTRET = Convert.ToDecimal(Dr["ICMS_VFCPSTRET"]);
                    obj.ICMS_PST = Convert.ToDecimal(Dr["ICMS_PST"]);
                    obj.ICMS_REDBCEFET = Convert.ToDecimal(Dr["ICMS_REDBCEFET"]);
                    obj.ICMS_VBCEFET = Convert.ToDecimal(Dr["ICMS_VBCEFET"]);
                    obj.ICMS_PEFET = Convert.ToDecimal(Dr["ICMS_PEFET"]);
                    obj.ICMS_VEFET = Convert.ToDecimal(Dr["ICMS_VEFET"]);
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
        public TECNO_NF_PRODUTOS_ICMS PesquisarTECNO_NF_PRODUTOS_ICMS(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_ICMS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_ICMS obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_ICMS();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.ICMS_CST = Convert.ToDecimal(Dr["ICMS_CST"]);
                    obj.ICMS_ORIG = Convert.ToDecimal(Dr["ICMS_ORIG"]);
                    obj.ICMS_MODBC = Convert.ToDecimal(Dr["ICMS_MODBC"]);
                    obj.ICMS_VBC = Convert.ToDecimal(Dr["ICMS_VBC"]);
                    obj.ICMS_PICMS = Convert.ToDecimal(Dr["ICMS_PICMS"]);
                    obj.ICMS_VICMS = Convert.ToDecimal(Dr["ICMS_VICMS"]);
                    obj.ICMS_MODBCST = Convert.ToDecimal(Dr["ICMS_MODBCST"]);
                    obj.ICMS_PMVAST = Convert.ToDecimal(Dr["ICMS_PMVAST"]);
                    obj.ICMS_PREDBCST = Convert.ToDecimal(Dr["ICMS_PREDBCST"]);
                    obj.ICMS_VBCST = Convert.ToDecimal(Dr["ICMS_VBCST"]);
                    obj.ICMS_PICMSST = Convert.ToDecimal(Dr["ICMS_PICMSST"]);
                    obj.ICMS_VICMSST = Convert.ToDecimal(Dr["ICMS_VICMSST"]);
                    obj.ICMS_PREDBC = Convert.ToDecimal(Dr["ICMS_PREDBC"]);
                    obj.ICMS_MOTDESICMS = Convert.ToDecimal(Dr["ICMS_MOTDESICMS"]);
                    obj.ICMS_VBCSTRET = Convert.ToDecimal(Dr["ICMS_VBCSTRET"]);
                    obj.ICMS_VICMSSTRET = Convert.ToDecimal(Dr["ICMS_VICMSSTRET"]);
                    obj.ICMS_ICMSPART = Convert.ToDecimal(Dr["ICMS_ICMSPART"]);
                    obj.ICMS_PBCOP = Convert.ToDecimal(Dr["ICMS_PBCOP"]);
                    obj.ICMS_UFST = Convert.ToString(Dr["ICMS_UFST"]);
                    obj.ICMS_ICMSST = Convert.ToDecimal(Dr["ICMS_ICMSST"]);
                    obj.ICMS_VBCSTDEST = Convert.ToDecimal(Dr["ICMS_VBCSTDEST"]);
                    obj.ICMS_VICMSSTDEST = Convert.ToDecimal(Dr["ICMS_VICMSSTDEST"]);
                    obj.ICMS_CSOSN = Convert.ToDecimal(Dr["ICMS_CSOSN"]);
                    obj.ICMS_PCREDSN = Convert.ToDecimal(Dr["ICMS_PCREDSN"]);
                    obj.ICMS_VCREDICMSSN = Convert.ToDecimal(Dr["ICMS_VCREDICMSSN"]);
                    obj.ICMS_VICMSDESON = Convert.ToDecimal(Dr["ICMS_VICMSDESON"]);
                    obj.ICMS_VICMSOP = Convert.ToDecimal(Dr["ICMS_VICMSOP"]);
                    obj.ICMS_PDIF = Convert.ToDecimal(Dr["ICMS_PDIF"]);
                    obj.ICMS_VICMSDIF = Convert.ToDecimal(Dr["ICMS_VICMSDIF"]);
                    obj.BO_MONTAR_VICMS = Convert.ToString(Dr["BO_MONTAR_VICMS"]);
                    obj.ICMS_VBCFCP = Convert.ToDecimal(Dr["ICMS_VBCFCP"]);
                    obj.ICMS_PFCP = Convert.ToDecimal(Dr["ICMS_PFCP"]);
                    obj.ICMS_VFCP = Convert.ToDecimal(Dr["ICMS_VFCP"]);
                    obj.ICMS_VBCFCPST = Convert.ToDecimal(Dr["ICMS_VBCFCPST"]);
                    obj.ICMS_PFCPST = Convert.ToDecimal(Dr["ICMS_PFCPST"]);
                    obj.ICMS_VFCPST = Convert.ToDecimal(Dr["ICMS_VFCPST"]);
                    obj.ICMS_VBCFCPSTRET = Convert.ToDecimal(Dr["ICMS_VBCFCPSTRET"]);
                    obj.ICMS_PFCPSTRET = Convert.ToDecimal(Dr["ICMS_PFCPSTRET"]);
                    obj.ICMS_VFCPSTRET = Convert.ToDecimal(Dr["ICMS_VFCPSTRET"]);
                    obj.ICMS_PST = Convert.ToDecimal(Dr["ICMS_PST"]);
                    obj.ICMS_REDBCEFET = Convert.ToDecimal(Dr["ICMS_REDBCEFET"]);
                    obj.ICMS_VBCEFET = Convert.ToDecimal(Dr["ICMS_VBCEFET"]);
                    obj.ICMS_PEFET = Convert.ToDecimal(Dr["ICMS_PEFET"]);
                    obj.ICMS_VEFET = Convert.ToDecimal(Dr["ICMS_VEFET"]);
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

