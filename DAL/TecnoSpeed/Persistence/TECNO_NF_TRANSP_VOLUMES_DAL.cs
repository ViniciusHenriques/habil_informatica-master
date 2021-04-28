
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
    public class TECNO_NF_TRANSP_VOLUMES_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_TRANSP_VOLUMES> ListarTECNO_NF_TRANSP_VOLUMES(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_VOLUMES Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_TRANSP_VOLUMES> tcn_nf = new List<TECNO_NF_TRANSP_VOLUMES>();

                while (Dr.Read())
                {
                    TECNO_NF_TRANSP_VOLUMES obj = new TECNO_NF_TRANSP_VOLUMES();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["NITEM"] != DBNull.Value)
                        obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);

                    if (Dr["VOL_QVOL"] != DBNull.Value)
                        obj.VOL_QVOL = Convert.ToDecimal(Dr["VOL_QVOL"]);

                    if (Dr["VOL_ESP"] != DBNull.Value)
                        obj.VOL_ESP = Convert.ToString(Dr["VOL_ESP"]);

                    if (Dr["VOL_MARCA"] != DBNull.Value)
                        obj.VOL_MARCA = Convert.ToString(Dr["VOL_MARCA"]);

                    if (Dr["VOL_NVOL"] != DBNull.Value)
                        obj.VOL_NVOL = Convert.ToString(Dr["VOL_NVOL"]);

                    if (Dr["VOL_PESOL"] != DBNull.Value)
                        obj.VOL_PESOL = Convert.ToDecimal(Dr["VOL_PESOL"]);

                    if (Dr["VOL_PESOB"] != DBNull.Value)
                        obj.VOL_PESOB = Convert.ToDecimal(Dr["VOL_PESOB"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_VOLUMES: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_TRANSP_VOLUMES PesquisarTECNO_NF_TRANSP_VOLUMES(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_VOLUMES Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_TRANSP_VOLUMES obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_TRANSP_VOLUMES();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["NITEM"] != DBNull.Value)
                        obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);

                    if (Dr["VOL_QVOL"] != DBNull.Value)
                        obj.VOL_QVOL = Convert.ToDecimal(Dr["VOL_QVOL"]);

                    if (Dr["VOL_ESP"] != DBNull.Value)
                        obj.VOL_ESP = Convert.ToString(Dr["VOL_ESP"]);

                    if (Dr["VOL_MARCA"] != DBNull.Value)
                        obj.VOL_MARCA = Convert.ToString(Dr["VOL_MARCA"]);

                    if (Dr["VOL_NVOL"] != DBNull.Value)
                        obj.VOL_NVOL = Convert.ToString(Dr["VOL_NVOL"]);

                    if (Dr["VOL_PESOL"] != DBNull.Value)
                        obj.VOL_PESOL = Convert.ToDecimal(Dr["VOL_PESOL"]);

                    if (Dr["VOL_PESOB"] != DBNull.Value)
                        obj.VOL_PESOB = Convert.ToDecimal(Dr["VOL_PESOB"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_VOLUMES: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
