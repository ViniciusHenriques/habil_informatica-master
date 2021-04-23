
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
    public class TECNO_NF_PRODUTOS_PIS_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_PRODUTOS_PIS> ListarTECNO_NF_PRODUTOS_PIS(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_PIS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_PRODUTOS_PIS> tcn_nf = new List<TECNO_NF_PRODUTOS_PIS>();

                while (Dr.Read())
                {
                    TECNO_NF_PRODUTOS_PIS obj = new TECNO_NF_PRODUTOS_PIS();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.PIS_CST = Convert.ToDecimal(Dr["PIS_CST"]);
                    obj.PIS_VBC = Convert.ToDecimal(Dr["PIS_VBC"]);
                    obj.PIS_PPIS = Convert.ToDecimal(Dr["PIS_PPIS"]);
                    obj.PIS_VPIS = Convert.ToDecimal(Dr["PIS_VPIS"]);
                    obj.PIS_QBCPROD = Convert.ToDecimal(Dr["PIS_QBCPROD"]);
                    obj.PIS_VALIQPROD = Convert.ToDecimal(Dr["PIS_VALIQPROD"]);
                    obj.PISST_VBC = Convert.ToDecimal(Dr["PISST_VBC"]);
                    obj.PISST_PPIS = Convert.ToDecimal(Dr["PISST_PPIS"]);
                    obj.PISST_QBCPROD = Convert.ToDecimal(Dr["PISST_QBCPROD"]);
                    obj.PISST_VALIQPROD = Convert.ToDecimal(Dr["PISST_VALIQPROD"]);
                    obj.PISST_VPIS = Convert.ToDecimal(Dr["PISST_VPIS"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_PRODUTOS_PIS PesquisarTECNO_NF_PRODUTOS_PIS(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_PIS Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_PIS obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_PIS();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.PIS_CST = Convert.ToDecimal(Dr["PIS_CST"]);
                    obj.PIS_VBC = Convert.ToDecimal(Dr["PIS_VBC"]);
                    obj.PIS_PPIS = Convert.ToDecimal(Dr["PIS_PPIS"]);
                    obj.PIS_VPIS = Convert.ToDecimal(Dr["PIS_VPIS"]);
                    obj.PIS_QBCPROD = Convert.ToDecimal(Dr["PIS_QBCPROD"]);
                    obj.PIS_VALIQPROD = Convert.ToDecimal(Dr["PIS_VALIQPROD"]);
                    obj.PISST_VBC = Convert.ToDecimal(Dr["PISST_VBC"]);
                    obj.PISST_PPIS = Convert.ToDecimal(Dr["PISST_PPIS"]);
                    obj.PISST_QBCPROD = Convert.ToDecimal(Dr["PISST_QBCPROD"]);
                    obj.PISST_VALIQPROD = Convert.ToDecimal(Dr["PISST_VALIQPROD"]);
                    obj.PISST_VPIS = Convert.ToDecimal(Dr["PISST_VPIS"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
