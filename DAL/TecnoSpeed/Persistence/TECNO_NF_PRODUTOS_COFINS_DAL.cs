
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
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.COFINS_CST = Convert.ToDecimal(Dr["COFINS_CST"]);
                    obj.COFINS_VBC = Convert.ToDecimal(Dr["COFINS_VBC"]);
                    obj.COFINS_PCOFINS = Convert.ToDecimal(Dr["COFINS_PCOFINS"]);
                    obj.COFINS_VCOFINS = Convert.ToDecimal(Dr["COFINS_VCOFINS"]);
                    obj.COFINS_QBCPROD = Convert.ToDecimal(Dr["COFINS_QBCPROD"]);
                    obj.COFINS_VALIQPROD = Convert.ToDecimal(Dr["COFINS_VALIQPROD"]);
                    obj.COFINSST_VBC = Convert.ToDecimal(Dr["COFINSST_VBC"]);
                    obj.COFINSST_PCOFINS = Convert.ToDecimal(Dr["COFINSST_PCOFINS"]);
                    obj.COFINSST_QBCPROD = Convert.ToDecimal(Dr["COFINSST_QBCPROD"]);
                    obj.COFINSST_VALIQPROD = Convert.ToDecimal(Dr["COFINSST_VALIQPROD"]);
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
        public TECNO_NF_PRODUTOS_COFINS PesquisarTECNO_NF_PRODUTOS_COFINS(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_COFINS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_COFINS obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_COFINS();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.COFINS_CST = Convert.ToDecimal(Dr["COFINS_CST"]);
                    obj.COFINS_VBC = Convert.ToDecimal(Dr["COFINS_VBC"]);
                    obj.COFINS_PCOFINS = Convert.ToDecimal(Dr["COFINS_PCOFINS"]);
                    obj.COFINS_VCOFINS = Convert.ToDecimal(Dr["COFINS_VCOFINS"]);
                    obj.COFINS_QBCPROD = Convert.ToDecimal(Dr["COFINS_QBCPROD"]);
                    obj.COFINS_VALIQPROD = Convert.ToDecimal(Dr["COFINS_VALIQPROD"]);
                    obj.COFINSST_VBC = Convert.ToDecimal(Dr["COFINSST_VBC"]);
                    obj.COFINSST_PCOFINS = Convert.ToDecimal(Dr["COFINSST_PCOFINS"]);
                    obj.COFINSST_QBCPROD = Convert.ToDecimal(Dr["COFINSST_QBCPROD"]);
                    obj.COFINSST_VALIQPROD = Convert.ToDecimal(Dr["COFINSST_VALIQPROD"]);
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
