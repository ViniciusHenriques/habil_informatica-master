
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

        public List<TECNO_NF_PRODUTOS> ListarTECNO_NF_PRODUTOS(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_PRODUTOS> tcn_nf = new List<TECNO_NF_PRODUTOS>();

                while (Dr.Read())
                {
                    TECNO_NF_PRODUTOS obj = new TECNO_NF_PRODUTOS();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["PROD_CPROD"] != DBNull.Value)
                        obj.PROD_CPROD = Convert.ToString(Dr["PROD_CPROD"]);

                    if (Dr["PROD_CEAN"] != DBNull.Value)
                        obj.PROD_CEAN = Convert.ToString(Dr["PROD_CEAN"]);

                    if (Dr["PROD_XPROD"] != DBNull.Value)
                        obj.PROD_XPROD = Convert.ToString(Dr["PROD_XPROD"]);

                    if (Dr["PROD_NCM"] != DBNull.Value)
                        obj.PROD_NCM = Convert.ToString(Dr["PROD_NCM"]);

                    if (Dr["PROD_EXTIPI"] != DBNull.Value)
                        obj.PROD_EXTIPI = Convert.ToString(Dr["PROD_EXTIPI"]);

                    if (Dr["PROD_GENERO"] != DBNull.Value)
                        obj.PROD_GENERO = Convert.ToDecimal(Dr["PROD_GENERO"]);

                    if (Dr["PROD_CFOP"] != DBNull.Value)
                        obj.PROD_CFOP = Convert.ToDecimal(Dr["PROD_CFOP"]);

                    if (Dr["PROD_UCOM"] != DBNull.Value)
                        obj.PROD_UCOM = Convert.ToString(Dr["PROD_UCOM"]);

                    if (Dr["PROD_QCOM"] != DBNull.Value)
                        obj.PROD_QCOM = Convert.ToDecimal(Dr["PROD_QCOM"]);

                    if (Dr["PROD_VUNCOM"] != DBNull.Value)
                        obj.PROD_VUNCOM = Convert.ToDecimal(Dr["PROD_VUNCOM"]);

                    if (Dr["PROD_VPROD"] != DBNull.Value)
                        obj.PROD_VPROD = Convert.ToDecimal(Dr["PROD_VPROD"]);

                    if (Dr["PROD_CEANTRIB"] != DBNull.Value)
                        obj.PROD_CEANTRIB = Convert.ToString(Dr["PROD_CEANTRIB"]);

                    if (Dr["PROD_UTRIB"] != DBNull.Value)
                        obj.PROD_UTRIB = Convert.ToString(Dr["PROD_UTRIB"]);

                    if (Dr["PROD_QTRIB"] != DBNull.Value)
                        obj.PROD_QTRIB = Convert.ToDecimal(Dr["PROD_QTRIB"]);

                    if (Dr["PROD_VUNTRIB"] != DBNull.Value)
                        obj.PROD_VUNTRIB = Convert.ToDecimal(Dr["PROD_VUNTRIB"]);

                    if (Dr["PROD_VFRETE"] != DBNull.Value)
                        obj.PROD_VFRETE = Convert.ToDecimal(Dr["PROD_VFRETE"]);

                    if (Dr["PROD_VSEG"] != DBNull.Value)
                        obj.PROD_VSEG = Convert.ToDecimal(Dr["PROD_VSEG"]);

                    if (Dr["PROD_VDESC"] != DBNull.Value)
                        obj.PROD_VDESC = Convert.ToDecimal(Dr["PROD_VDESC"]);

                    if (Dr["PROD_INFADPROD"] != DBNull.Value)
                        obj.PROD_INFADPROD = Convert.ToString(Dr["PROD_INFADPROD"]);

                    if (Dr["PROD_VOUTRO"] != DBNull.Value)
                        obj.PROD_VOUTRO = Convert.ToDecimal(Dr["PROD_VOUTRO"]);

                    if (Dr["PROD_INDTOT"] != DBNull.Value)
                        obj.PROD_INDTOT = Convert.ToDecimal(Dr["PROD_INDTOT"]);

                    if (Dr["PROD_XPED"] != DBNull.Value)
                        obj.PROD_XPED = Convert.ToString(Dr["PROD_XPED"]);

                    if (Dr["PROD_NITEMPED"] != DBNull.Value)
                        obj.PROD_NITEMPED = Convert.ToDecimal(Dr["PROD_NITEMPED"]);

                    if (Dr["VTOTTRIB"] != DBNull.Value)
                        obj.VTOTTRIB = Convert.ToDecimal(Dr["VTOTTRIB"]);

                    if (Dr["PROD_NFCI"] != DBNull.Value)
                        obj.PROD_NFCI = Convert.ToString(Dr["PROD_NFCI"]);

                    if (Dr["prod_nve"] != DBNull.Value)
                        obj.prod_nve = Convert.ToString(Dr["prod_nve"]);

                    if (Dr["PROD_RECOPI"] != DBNull.Value)
                        obj.PROD_RECOPI = Convert.ToDecimal(Dr["PROD_RECOPI"]);

                    if (Dr["PDEVOL"] != DBNull.Value)
                        obj.PDEVOL = Convert.ToDecimal(Dr["PDEVOL"]);

                    if (Dr["VIPIDEVOL"] != DBNull.Value)
                        obj.VIPIDEVOL = Convert.ToDecimal(Dr["VIPIDEVOL"]);

                    if (Dr["PROD_CEST"] != DBNull.Value)
                        obj.PROD_CEST = Convert.ToString(Dr["PROD_CEST"]);

                    if (Dr["PROD_INDESCALA"] != DBNull.Value)
                        obj.PROD_INDESCALA = Convert.ToString(Dr["PROD_INDESCALA"]);

                    if (Dr["PROD_CNPJFAB"] != DBNull.Value)
                        obj.PROD_CNPJFAB = Convert.ToString(Dr["PROD_CNPJFAB"]);

                    if (Dr["PROD_CBENEF"] != DBNull.Value)
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
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["PROD_CPROD"] != DBNull.Value)
                        obj.PROD_CPROD = Convert.ToString(Dr["PROD_CPROD"]);

                    if (Dr["PROD_CEAN"] != DBNull.Value)
                        obj.PROD_CEAN = Convert.ToString(Dr["PROD_CEAN"]);

                    if (Dr["PROD_XPROD"] != DBNull.Value)
                        obj.PROD_XPROD = Convert.ToString(Dr["PROD_XPROD"]);

                    if (Dr["PROD_NCM"] != DBNull.Value)
                        obj.PROD_NCM = Convert.ToString(Dr["PROD_NCM"]);

                    if (Dr["PROD_EXTIPI"] != DBNull.Value)
                        obj.PROD_EXTIPI = Convert.ToString(Dr["PROD_EXTIPI"]);

                    if (Dr["PROD_GENERO"] != DBNull.Value)
                        obj.PROD_GENERO = Convert.ToDecimal(Dr["PROD_GENERO"]);

                    if (Dr["PROD_CFOP"] != DBNull.Value)
                        obj.PROD_CFOP = Convert.ToDecimal(Dr["PROD_CFOP"]);

                    if (Dr["PROD_UCOM"] != DBNull.Value)
                        obj.PROD_UCOM = Convert.ToString(Dr["PROD_UCOM"]);

                    if (Dr["PROD_QCOM"] != DBNull.Value)
                        obj.PROD_QCOM = Convert.ToDecimal(Dr["PROD_QCOM"]);

                    if (Dr["PROD_VUNCOM"] != DBNull.Value)
                        obj.PROD_VUNCOM = Convert.ToDecimal(Dr["PROD_VUNCOM"]);

                    if (Dr["PROD_VPROD"] != DBNull.Value)
                        obj.PROD_VPROD = Convert.ToDecimal(Dr["PROD_VPROD"]);

                    if (Dr["PROD_CEANTRIB"] != DBNull.Value)
                        obj.PROD_CEANTRIB = Convert.ToString(Dr["PROD_CEANTRIB"]);

                    if (Dr["PROD_UTRIB"] != DBNull.Value)
                        obj.PROD_UTRIB = Convert.ToString(Dr["PROD_UTRIB"]);

                    if (Dr["PROD_QTRIB"] != DBNull.Value)
                        obj.PROD_QTRIB = Convert.ToDecimal(Dr["PROD_QTRIB"]);

                    if (Dr["PROD_VUNTRIB"] != DBNull.Value)
                        obj.PROD_VUNTRIB = Convert.ToDecimal(Dr["PROD_VUNTRIB"]);

                    if (Dr["PROD_VFRETE"] != DBNull.Value)
                        obj.PROD_VFRETE = Convert.ToDecimal(Dr["PROD_VFRETE"]);

                    if (Dr["PROD_VSEG"] != DBNull.Value)
                        obj.PROD_VSEG = Convert.ToDecimal(Dr["PROD_VSEG"]);

                    if (Dr["PROD_VDESC"] != DBNull.Value)
                        obj.PROD_VDESC = Convert.ToDecimal(Dr["PROD_VDESC"]);

                    if (Dr["PROD_INFADPROD"] != DBNull.Value)
                        obj.PROD_INFADPROD = Convert.ToString(Dr["PROD_INFADPROD"]);

                    if (Dr["PROD_VOUTRO"] != DBNull.Value)
                        obj.PROD_VOUTRO = Convert.ToDecimal(Dr["PROD_VOUTRO"]);

                    if (Dr["PROD_INDTOT"] != DBNull.Value)
                        obj.PROD_INDTOT = Convert.ToDecimal(Dr["PROD_INDTOT"]);

                    if (Dr["PROD_XPED"] != DBNull.Value)
                        obj.PROD_XPED = Convert.ToString(Dr["PROD_XPED"]);

                    if (Dr["PROD_NITEMPED"] != DBNull.Value)
                        obj.PROD_NITEMPED = Convert.ToDecimal(Dr["PROD_NITEMPED"]);

                    if (Dr["VTOTTRIB"] != DBNull.Value)
                        obj.VTOTTRIB = Convert.ToDecimal(Dr["VTOTTRIB"]);

                    if (Dr["PROD_NFCI"] != DBNull.Value)
                        obj.PROD_NFCI = Convert.ToString(Dr["PROD_NFCI"]);

                    if (Dr["prod_nve"] != DBNull.Value)
                        obj.prod_nve = Convert.ToString(Dr["prod_nve"]);

                    if (Dr["PROD_RECOPI"] != DBNull.Value)
                        obj.PROD_RECOPI = Convert.ToDecimal(Dr["PROD_RECOPI"]);

                    if (Dr["PDEVOL"] != DBNull.Value)
                        obj.PDEVOL = Convert.ToDecimal(Dr["PDEVOL"]);

                    if (Dr["VIPIDEVOL"] != DBNull.Value)
                        obj.VIPIDEVOL = Convert.ToDecimal(Dr["VIPIDEVOL"]);

                    if (Dr["PROD_CEST"] != DBNull.Value)
                        obj.PROD_CEST = Convert.ToString(Dr["PROD_CEST"]);

                    if (Dr["PROD_INDESCALA"] != DBNull.Value)
                        obj.PROD_INDESCALA = Convert.ToString(Dr["PROD_INDESCALA"]);

                    if (Dr["PROD_CNPJFAB"] != DBNull.Value)
                        obj.PROD_CNPJFAB = Convert.ToString(Dr["PROD_CNPJFAB"]);

                    if (Dr["PROD_CBENEF"] != DBNull.Value)
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
