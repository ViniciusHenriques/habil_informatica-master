
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
    public class TECNO_NF_FORMA_PGTO_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_FORMA_PGTO> ListarTECNO_NF_FORMA_PGTO(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_FORMA_PGTO Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_FORMA_PGTO> tcn_nf = new List<TECNO_NF_FORMA_PGTO>();

                while (Dr.Read())
                {
                    TECNO_NF_FORMA_PGTO obj = new TECNO_NF_FORMA_PGTO();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["FORMA_PGTO_NITEM"] != DBNull.Value)
                        obj.FORMA_PGTO_NITEM = Convert.ToDecimal(Dr["FORMA_PGTO_NITEM"]);

                    if (Dr["TPAG"] != DBNull.Value)
                        obj.TPAG = Convert.ToString(Dr["TPAG"]);

                    if (Dr["VPAG"] != DBNull.Value)
                        obj.VPAG = Convert.ToDecimal(Dr["VPAG"]);

                    if (Dr["VTROCO"] != DBNull.Value)
                        obj.VTROCO = Convert.ToDecimal(Dr["VTROCO"]);

                    if (Dr["INDPAG"] != DBNull.Value)
                        obj.INDPAG = Convert.ToString(Dr["INDPAG"]);

                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_FORMA_PGTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_FORMA_PGTO PesquisarTECNO_NF_FORMA_PGTO(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_FORMA_PGTO Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_FORMA_PGTO obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_FORMA_PGTO();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["FORMA_PGTO_NITEM"] != DBNull.Value)
                        obj.FORMA_PGTO_NITEM = Convert.ToDecimal(Dr["FORMA_PGTO_NITEM"]);

                    if (Dr["TPAG"] != DBNull.Value)
                        obj.TPAG = Convert.ToString(Dr["TPAG"]);

                    if (Dr["VPAG"] != DBNull.Value)
                        obj.VPAG = Convert.ToDecimal(Dr["VPAG"]);

                    if (Dr["VTROCO"] != DBNull.Value)
                        obj.VTROCO = Convert.ToDecimal(Dr["VTROCO"]);

                    if (Dr["INDPAG"] != DBNull.Value)
                        obj.INDPAG = Convert.ToString(Dr["INDPAG"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_FORMA_PGTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}

