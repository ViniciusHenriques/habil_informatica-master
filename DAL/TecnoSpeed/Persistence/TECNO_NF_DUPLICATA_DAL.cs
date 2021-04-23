
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
    public class TECNO_NF_DUPLICATA_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_DUPLICATA> ListarTECNO_NF_DUPLICATA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_DUPLICATA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_DUPLICATA> tcn_nf = new List<TECNO_NF_DUPLICATA>();

                while (Dr.Read())
                {
                    TECNO_NF_DUPLICATA obj = new TECNO_NF_DUPLICATA();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);
                    obj.DUP_NDUP = Convert.ToString(Dr["DUP_NDUP"]);
                    obj.DUP_DVENC = Convert.ToDateTime(Dr["DUP_DVENC"]);
                    obj.DUP_VDUP = Convert.ToDecimal(Dr["DUP_VDUP"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_DUPLICATA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_DUPLICATA PesquisarTECNO_NF_DUPLICATA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_DUPLICATA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_DUPLICATA obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_DUPLICATA();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);
                    obj.DUP_NDUP = Convert.ToString(Dr["DUP_NDUP"]);
                    obj.DUP_DVENC = Convert.ToDateTime(Dr["DUP_DVENC"]);
                    obj.DUP_VDUP = Convert.ToDecimal(Dr["DUP_VDUP"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_DUPLICATA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
