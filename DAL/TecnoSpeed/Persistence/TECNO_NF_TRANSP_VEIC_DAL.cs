
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
    public class TECNO_NF_TRANSP_VEIC_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_TRANSP_VEIC> ListarTECNO_NF_TRANSP_VEIC(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_VEIC Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_TRANSP_VEIC> tcn_nf = new List<TECNO_NF_TRANSP_VEIC>();

                while (Dr.Read())
                {
                    TECNO_NF_TRANSP_VEIC obj = new TECNO_NF_TRANSP_VEIC();
                    if (Dr["VEICTRANSP_PLACA"] != DBNull.Value)
                        obj.VEICTRANSP_PLACA = Convert.ToString(Dr["VEICTRANSP_PLACA"]);

                    if (Dr["VEICTRANSP_UF"] != DBNull.Value)
                        obj.VEICTRANSP_UF = Convert.ToString(Dr["VEICTRANSP_UF"]);

                    if (Dr["VEICTRANSP_RNTC"] != DBNull.Value)
                        obj.VEICTRANSP_RNTC = Convert.ToString(Dr["VEICTRANSP_RNTC"]);

                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_VEIC: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_TRANSP_VEIC PesquisarTECNO_NF_TRANSP_VEIC(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_VEIC Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_TRANSP_VEIC obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_TRANSP_VEIC();
                    if (Dr["VEICTRANSP_PLACA"] != DBNull.Value)
                        obj.VEICTRANSP_PLACA = Convert.ToString(Dr["VEICTRANSP_PLACA"]);

                    if (Dr["VEICTRANSP_UF"] != DBNull.Value)
                        obj.VEICTRANSP_UF = Convert.ToString(Dr["VEICTRANSP_UF"]);

                    if (Dr["VEICTRANSP_RNTC"] != DBNull.Value)
                        obj.VEICTRANSP_RNTC = Convert.ToString(Dr["VEICTRANSP_RNTC"]);

                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_VEIC: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
