
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
    public class TECNO_NF_PRODUTOS_IPI_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_PRODUTOS_IPI> ListarTECNO_NF_PRODUTOS_IPI(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_IPI Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_PRODUTOS_IPI> tcn_nf = new List<TECNO_NF_PRODUTOS_IPI>();

                while (Dr.Read())
                {
                    TECNO_NF_PRODUTOS_IPI obj = new TECNO_NF_PRODUTOS_IPI();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.IPI_CLENQ = Convert.ToString(Dr["IPI_CLENQ"]);
                    obj.IPI_CNPJPROD = Convert.ToString(Dr["IPI_CNPJPROD"]);
                    obj.IPI_CSELO = Convert.ToString(Dr["IPI_CSELO"]);
                    obj.IPI_QSELO = Convert.ToDecimal(Dr["IPI_QSELO"]);
                    obj.IPI_CENQ = Convert.ToString(Dr["IPI_CENQ"]);
                    obj.IPI_CST = Convert.ToDecimal(Dr["IPI_CST"]);
                    obj.IPI_VBC = Convert.ToDecimal(Dr["IPI_VBC"]);
                    obj.IPI_QUNID = Convert.ToDecimal(Dr["IPI_QUNID"]);
                    obj.IPI_VUNID = Convert.ToDecimal(Dr["IPI_VUNID"]);
                    obj.IPI_PIPI = Convert.ToDecimal(Dr["IPI_PIPI"]);
                    obj.IPI_VIPI = Convert.ToDecimal(Dr["IPI_VIPI"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_IPI: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_PRODUTOS_IPI PesquisarTECNO_NF_PRODUTOS_IPI(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_IPI Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_IPI obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_IPI();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.IPI_CLENQ = Convert.ToString(Dr["IPI_CLENQ"]);
                    obj.IPI_CNPJPROD = Convert.ToString(Dr["IPI_CNPJPROD"]);
                    obj.IPI_CSELO = Convert.ToString(Dr["IPI_CSELO"]);
                    obj.IPI_QSELO = Convert.ToDecimal(Dr["IPI_QSELO"]);
                    obj.IPI_CENQ = Convert.ToString(Dr["IPI_CENQ"]);
                    obj.IPI_CST = Convert.ToDecimal(Dr["IPI_CST"]);
                    obj.IPI_VBC = Convert.ToDecimal(Dr["IPI_VBC"]);
                    obj.IPI_QUNID = Convert.ToDecimal(Dr["IPI_QUNID"]);
                    obj.IPI_VUNID = Convert.ToDecimal(Dr["IPI_VUNID"]);
                    obj.IPI_PIPI = Convert.ToDecimal(Dr["IPI_PIPI"]);
                    obj.IPI_VIPI = Convert.ToDecimal(Dr["IPI_VIPI"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_IPI: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
