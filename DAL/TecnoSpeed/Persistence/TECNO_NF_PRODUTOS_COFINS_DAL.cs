
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
    public class TECNO_NF_PRODUTOS_COFINS_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_PRODUTOS_COFINS> ListarTECNO_NF_PRODUTOS_COFINS(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_COFINS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_PRODUTOS_COFINS> tcn_nf = new List<TECNO_NF_PRODUTOS_COFINS>();

                while (Dr.Read())
                {
                    TECNO_NF_PRODUTOS_COFINS obj = new TECNO_NF_PRODUTOS_COFINS();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["COFINS_CST"] != DBNull.Value)
                        obj.COFINS_CST = Convert.ToDecimal(Dr["COFINS_CST"]);

                    if (Dr["COFINS_VBC"] != DBNull.Value)
                        obj.COFINS_VBC = Convert.ToDecimal(Dr["COFINS_VBC"]);

                    if (Dr["COFINS_PCOFINS"] != DBNull.Value)
                        obj.COFINS_PCOFINS = Convert.ToDecimal(Dr["COFINS_PCOFINS"]);

                    if (Dr["COFINS_VCOFINS"] != DBNull.Value)
                        obj.COFINS_VCOFINS = Convert.ToDecimal(Dr["COFINS_VCOFINS"]);

                    if (Dr["COFINS_QBCPROD"] != DBNull.Value)
                        obj.COFINS_QBCPROD = Convert.ToDecimal(Dr["COFINS_QBCPROD"]);

                    if (Dr["COFINS_VALIQPROD"] != DBNull.Value)
                        obj.COFINS_VALIQPROD = Convert.ToDecimal(Dr["COFINS_VALIQPROD"]);

                    if (Dr["COFINSST_VBC"] != DBNull.Value)
                        obj.COFINSST_VBC = Convert.ToDecimal(Dr["COFINSST_VBC"]);

                    if (Dr["COFINSST_PCOFINS"] != DBNull.Value)
                        obj.COFINSST_PCOFINS = Convert.ToDecimal(Dr["COFINSST_PCOFINS"]);

                    if (Dr["COFINSST_QBCPROD"] != DBNull.Value)
                        obj.COFINSST_QBCPROD = Convert.ToDecimal(Dr["COFINSST_QBCPROD"]);

                    if (Dr["COFINSST_VALIQPROD"] != DBNull.Value)
                        obj.COFINSST_VALIQPROD = Convert.ToDecimal(Dr["COFINSST_VALIQPROD"]);

                    if (Dr["COFINSST_VCOFINS"] != DBNull.Value)
                        obj.COFINSST_VCOFINS = Convert.ToDecimal(Dr["COFINSST_VCOFINS"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_COFINS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_PRODUTOS_COFINS PesquisarTECNO_NF_PRODUTOS_COFINS(decimal decID_NOTA_FISCAL, decimal decPROD_NITEM)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_COFINS Where ID_NOTA_FISCAL = @v1 and PROD_NITEM = @v2";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);
                Cmd.Parameters.AddWithValue("@v2", decPROD_NITEM);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_COFINS obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_COFINS();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["COFINS_CST"] != DBNull.Value)
                        obj.COFINS_CST = Convert.ToDecimal(Dr["COFINS_CST"]);

                    if (Dr["COFINS_VBC"] != DBNull.Value)
                        obj.COFINS_VBC = Convert.ToDecimal(Dr["COFINS_VBC"]);

                    if (Dr["COFINS_PCOFINS"] != DBNull.Value)
                        obj.COFINS_PCOFINS = Convert.ToDecimal(Dr["COFINS_PCOFINS"]);

                    if (Dr["COFINS_VCOFINS"] != DBNull.Value)
                        obj.COFINS_VCOFINS = Convert.ToDecimal(Dr["COFINS_VCOFINS"]);

                    if (Dr["COFINS_QBCPROD"] != DBNull.Value)
                        obj.COFINS_QBCPROD = Convert.ToDecimal(Dr["COFINS_QBCPROD"]);

                    if (Dr["COFINS_VALIQPROD"] != DBNull.Value)
                        obj.COFINS_VALIQPROD = Convert.ToDecimal(Dr["COFINS_VALIQPROD"]);

                    if (Dr["COFINSST_VBC"] != DBNull.Value)
                        obj.COFINSST_VBC = Convert.ToDecimal(Dr["COFINSST_VBC"]);

                    if (Dr["COFINSST_PCOFINS"] != DBNull.Value)
                        obj.COFINSST_PCOFINS = Convert.ToDecimal(Dr["COFINSST_PCOFINS"]);

                    if (Dr["COFINSST_QBCPROD"] != DBNull.Value)
                        obj.COFINSST_QBCPROD = Convert.ToDecimal(Dr["COFINSST_QBCPROD"]);

                    if (Dr["COFINSST_VALIQPROD"] != DBNull.Value)
                        obj.COFINSST_VALIQPROD = Convert.ToDecimal(Dr["COFINSST_VALIQPROD"]);

                    if (Dr["COFINSST_VCOFINS"] != DBNull.Value)
                        obj.COFINSST_VCOFINS = Convert.ToDecimal(Dr["COFINSST_VCOFINS"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_COFINS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
