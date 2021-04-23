
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
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.II_VBC = Convert.ToDecimal(Dr["II_VBC"]);
                    obj.II_VDESPADU = Convert.ToDecimal(Dr["II_VDESPADU"]);
                    obj.II_VII = Convert.ToDecimal(Dr["II_VII"]);
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
        public TECNO_NF_PRODUTOS_II PesquisarTECNO_NF_PRODUTOS_II(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_II Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_II obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_II();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.II_VBC = Convert.ToDecimal(Dr["II_VBC"]);
                    obj.II_VDESPADU = Convert.ToDecimal(Dr["II_VDESPADU"]);
                    obj.II_VII = Convert.ToDecimal(Dr["II_VII"]);
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
