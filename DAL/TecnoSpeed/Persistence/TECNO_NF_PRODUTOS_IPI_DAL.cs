
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
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["IPI_CLENQ"] != DBNull.Value)
                        obj.IPI_CLENQ = Convert.ToString(Dr["IPI_CLENQ"]);

                    if (Dr["IPI_CNPJPROD"] != DBNull.Value)
                        obj.IPI_CNPJPROD = Convert.ToString(Dr["IPI_CNPJPROD"]);

                    if (Dr["IPI_CSELO"] != DBNull.Value)
                        obj.IPI_CSELO = Convert.ToString(Dr["IPI_CSELO"]);

                    if (Dr["IPI_QSELO"] != DBNull.Value)
                        obj.IPI_QSELO = Convert.ToDecimal(Dr["IPI_QSELO"]);

                    if (Dr["IPI_CENQ"] != DBNull.Value)
                        obj.IPI_CENQ = Convert.ToString(Dr["IPI_CENQ"]);

                    if (Dr["IPI_CST"] != DBNull.Value)
                        obj.IPI_CST = Convert.ToDecimal(Dr["IPI_CST"]);

                    if (Dr["IPI_VBC"] != DBNull.Value)
                        obj.IPI_VBC = Convert.ToDecimal(Dr["IPI_VBC"]);

                    if (Dr["IPI_QUNID"] != DBNull.Value)
                        obj.IPI_QUNID = Convert.ToDecimal(Dr["IPI_QUNID"]);

                    if (Dr["IPI_VUNID"] != DBNull.Value)
                        obj.IPI_VUNID = Convert.ToDecimal(Dr["IPI_VUNID"]);

                    if (Dr["IPI_PIPI"] != DBNull.Value)
                        obj.IPI_PIPI = Convert.ToDecimal(Dr["IPI_PIPI"]);

                    if (Dr["IPI_VIPI"] != DBNull.Value)
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
        public TECNO_NF_PRODUTOS_IPI PesquisarTECNO_NF_PRODUTOS_IPI(decimal decID_NOTA_FISCAL, decimal decPROD_NITEM)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_IPI Where ID_NOTA_FISCAL = @v1 and PROD_NITEM = @v2";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);
                Cmd.Parameters.AddWithValue("@v2", decPROD_NITEM);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_IPI obj = null;
                if (Dr.Read())
                {
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["IPI_CLENQ"] != DBNull.Value)
                        obj.IPI_CLENQ = Convert.ToString(Dr["IPI_CLENQ"]);

                    if (Dr["IPI_CNPJPROD"] != DBNull.Value)
                        obj.IPI_CNPJPROD = Convert.ToString(Dr["IPI_CNPJPROD"]);

                    if (Dr["IPI_CSELO"] != DBNull.Value)
                        obj.IPI_CSELO = Convert.ToString(Dr["IPI_CSELO"]);

                    if (Dr["IPI_QSELO"] != DBNull.Value)
                        obj.IPI_QSELO = Convert.ToDecimal(Dr["IPI_QSELO"]);

                    if (Dr["IPI_CENQ"] != DBNull.Value)
                        obj.IPI_CENQ = Convert.ToString(Dr["IPI_CENQ"]);

                    if (Dr["IPI_CST"] != DBNull.Value)
                        obj.IPI_CST = Convert.ToDecimal(Dr["IPI_CST"]);

                    if (Dr["IPI_VBC"] != DBNull.Value)
                        obj.IPI_VBC = Convert.ToDecimal(Dr["IPI_VBC"]);

                    if (Dr["IPI_QUNID"] != DBNull.Value)
                        obj.IPI_QUNID = Convert.ToDecimal(Dr["IPI_QUNID"]);

                    if (Dr["IPI_VUNID"] != DBNull.Value)
                        obj.IPI_VUNID = Convert.ToDecimal(Dr["IPI_VUNID"]);

                    if (Dr["IPI_PIPI"] != DBNull.Value)
                        obj.IPI_PIPI = Convert.ToDecimal(Dr["IPI_PIPI"]);

                    if (Dr["IPI_VIPI"] != DBNull.Value)
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
