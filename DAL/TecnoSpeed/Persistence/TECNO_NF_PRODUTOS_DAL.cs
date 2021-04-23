
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
    public class TECNO_NF_PRODUTOS_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_PRODUTOS> ListarTECNO_NF_PRODUTOS(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_PRODUTOS> tcn_nf = new List<TECNO_NF_PRODUTOS>();

                while (Dr.Read())
                {
                    TECNO_NF_PRODUTOS obj = new TECNO_NF_PRODUTOS();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.PROD_CPROD = Convert.ToString(Dr["PROD_CPROD"]);
                    obj.PROD_CEAN = Convert.ToString(Dr["PROD_CEAN"]);
                    obj.PROD_XPROD = Convert.ToString(Dr["PROD_XPROD"]);
                    obj.PROD_NCM = Convert.ToString(Dr["PROD_NCM"]);
                    obj.PROD_EXTIPI = Convert.ToString(Dr["PROD_EXTIPI"]);
                    obj.PROD_GENERO = Convert.ToDecimal(Dr["PROD_GENERO"]);
                    obj.PROD_CFOP = Convert.ToDecimal(Dr["PROD_CFOP"]);
                    obj.PROD_UCOM = Convert.ToString(Dr["PROD_UCOM"]);
                    obj.PROD_QCOM = Convert.ToDecimal(Dr["PROD_QCOM"]);
                    obj.PROD_VUNCOM = Convert.ToDecimal(Dr["PROD_VUNCOM"]);
                    obj.PROD_VPROD = Convert.ToDecimal(Dr["PROD_VPROD"]);
                    obj.PROD_CEANTRIB = Convert.ToString(Dr["PROD_CEANTRIB"]);
                    obj.PROD_UTRIB = Convert.ToString(Dr["PROD_UTRIB"]);
                    obj.PROD_QTRIB = Convert.ToDecimal(Dr["PROD_QTRIB"]);
                    obj.PROD_VUNTRIB = Convert.ToDecimal(Dr["PROD_VUNTRIB"]);
                    obj.PROD_VFRETE = Convert.ToDecimal(Dr["PROD_VFRETE"]);
                    obj.PROD_VSEG = Convert.ToDecimal(Dr["PROD_VSEG"]);
                    obj.PROD_VDESC = Convert.ToDecimal(Dr["PROD_VDESC"]);
                    obj.PROD_INFADPROD = Convert.ToString(Dr["PROD_INFADPROD"]);
                    obj.PROD_VOUTRO = Convert.ToDecimal(Dr["PROD_VOUTRO"]);
                    obj.PROD_INDTOT = Convert.ToDecimal(Dr["PROD_INDTOT"]);
                    obj.PROD_XPED = Convert.ToString(Dr["PROD_XPED"]);
                    obj.PROD_NITEMPED = Convert.ToDecimal(Dr["PROD_NITEMPED"]);
                    obj.VTOTTRIB = Convert.ToDecimal(Dr["VTOTTRIB"]);
                    obj.PROD_NFCI = Convert.ToString(Dr["PROD_NFCI"]);
                    obj.prod_nve = Convert.ToString(Dr["prod_nve"]);
                    obj.PROD_RECOPI = Convert.ToDecimal(Dr["PROD_RECOPI"]);
                    obj.PDEVOL = Convert.ToDecimal(Dr["PDEVOL"]);
                    obj.VIPIDEVOL = Convert.ToDecimal(Dr["VIPIDEVOL"]);
                    obj.PROD_CEST = Convert.ToString(Dr["PROD_CEST"]);
                    obj.PROD_INDESCALA = Convert.ToString(Dr["PROD_INDESCALA"]);
                    obj.PROD_CNPJFAB = Convert.ToString(Dr["PROD_CNPJFAB"]);
                    obj.PROD_CBENEF = Convert.ToString(Dr["PROD_CBENEF"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_PRODUTOS PesquisarTECNO_NF_PRODUTOS(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.PROD_CPROD = Convert.ToString(Dr["PROD_CPROD"]);
                    obj.PROD_CEAN = Convert.ToString(Dr["PROD_CEAN"]);
                    obj.PROD_XPROD = Convert.ToString(Dr["PROD_XPROD"]);
                    obj.PROD_NCM = Convert.ToString(Dr["PROD_NCM"]);
                    obj.PROD_EXTIPI = Convert.ToString(Dr["PROD_EXTIPI"]);
                    obj.PROD_GENERO = Convert.ToDecimal(Dr["PROD_GENERO"]);
                    obj.PROD_CFOP = Convert.ToDecimal(Dr["PROD_CFOP"]);
                    obj.PROD_UCOM = Convert.ToString(Dr["PROD_UCOM"]);
                    obj.PROD_QCOM = Convert.ToDecimal(Dr["PROD_QCOM"]);
                    obj.PROD_VUNCOM = Convert.ToDecimal(Dr["PROD_VUNCOM"]);
                    obj.PROD_VPROD = Convert.ToDecimal(Dr["PROD_VPROD"]);
                    obj.PROD_CEANTRIB = Convert.ToString(Dr["PROD_CEANTRIB"]);
                    obj.PROD_UTRIB = Convert.ToString(Dr["PROD_UTRIB"]);
                    obj.PROD_QTRIB = Convert.ToDecimal(Dr["PROD_QTRIB"]);
                    obj.PROD_VUNTRIB = Convert.ToDecimal(Dr["PROD_VUNTRIB"]);
                    obj.PROD_VFRETE = Convert.ToDecimal(Dr["PROD_VFRETE"]);
                    obj.PROD_VSEG = Convert.ToDecimal(Dr["PROD_VSEG"]);
                    obj.PROD_VDESC = Convert.ToDecimal(Dr["PROD_VDESC"]);
                    obj.PROD_INFADPROD = Convert.ToString(Dr["PROD_INFADPROD"]);
                    obj.PROD_VOUTRO = Convert.ToDecimal(Dr["PROD_VOUTRO"]);
                    obj.PROD_INDTOT = Convert.ToDecimal(Dr["PROD_INDTOT"]);
                    obj.PROD_XPED = Convert.ToString(Dr["PROD_XPED"]);
                    obj.PROD_NITEMPED = Convert.ToDecimal(Dr["PROD_NITEMPED"]);
                    obj.VTOTTRIB = Convert.ToDecimal(Dr["VTOTTRIB"]);
                    obj.PROD_NFCI = Convert.ToString(Dr["PROD_NFCI"]);
                    obj.prod_nve = Convert.ToString(Dr["prod_nve"]);
                    obj.PROD_RECOPI = Convert.ToDecimal(Dr["PROD_RECOPI"]);
                    obj.PDEVOL = Convert.ToDecimal(Dr["PDEVOL"]);
                    obj.VIPIDEVOL = Convert.ToDecimal(Dr["VIPIDEVOL"]);
                    obj.PROD_CEST = Convert.ToString(Dr["PROD_CEST"]);
                    obj.PROD_INDESCALA = Convert.ToString(Dr["PROD_INDESCALA"]);
                    obj.PROD_CNPJFAB = Convert.ToString(Dr["PROD_CNPJFAB"]);
                    obj.PROD_CBENEF = Convert.ToString(Dr["PROD_CBENEF"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
