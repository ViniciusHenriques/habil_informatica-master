
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
    public class TECNO_NF_PRODUTOS_II_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_PRODUTOS_II> ListarTECNO_NF_PRODUTOS_II(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_II Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_PRODUTOS_II> tcn_nf = new List<TECNO_NF_PRODUTOS_II>();

                while (Dr.Read())
                {
                    TECNO_NF_PRODUTOS_II obj = new TECNO_NF_PRODUTOS_II();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["II_VBC"] != DBNull.Value)
                        obj.II_VBC = Convert.ToDecimal(Dr["II_VBC"]);

                    if (Dr["II_VDESPADU"] != DBNull.Value)
                        obj.II_VDESPADU = Convert.ToDecimal(Dr["II_VDESPADU"]);

                    if (Dr["II_VII"] != DBNull.Value)
                        obj.II_VII = Convert.ToDecimal(Dr["II_VII"]);

                    if (Dr["II_VIOF"] != DBNull.Value)
                        obj.II_VIOF = Convert.ToDecimal(Dr["II_VIOF"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_II: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_PRODUTOS_II PesquisarTECNO_NF_PRODUTOS_II(decimal decID_NOTA_FISCAL, decimal decPROD_NITEM)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_II Where ID_NOTA_FISCAL = @v1 AND PROD_NITEM = @v2";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);
                Cmd.Parameters.AddWithValue("@v2", decPROD_NITEM);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_II obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_II();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["II_VBC"] != DBNull.Value)
                        obj.II_VBC = Convert.ToDecimal(Dr["II_VBC"]);

                    if (Dr["II_VDESPADU"] != DBNull.Value)
                        obj.II_VDESPADU = Convert.ToDecimal(Dr["II_VDESPADU"]);

                    if (Dr["II_VII"] != DBNull.Value)
                        obj.II_VII = Convert.ToDecimal(Dr["II_VII"]);

                    if (Dr["II_VIOF"] != DBNull.Value)
                        obj.II_VIOF = Convert.ToDecimal(Dr["II_VIOF"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_II: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
