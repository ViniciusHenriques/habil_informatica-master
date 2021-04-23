
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
    public class TECNO_NF_TRANSP_REBOQUE_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_TRANSP_REBOQUE> ListarTECNO_NF_TRANSP_REBOQUE(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_REBOQUE Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_TRANSP_REBOQUE> tcn_nf = new List<TECNO_NF_TRANSP_REBOQUE>();

                while (Dr.Read())
                {
                    TECNO_NF_TRANSP_REBOQUE obj = new TECNO_NF_TRANSP_REBOQUE();
                    obj.REBOQUE_PLACA = Convert.ToString(Dr["REBOQUE_PLACA"]);
                    obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);
                    obj.REBOQUE_UF = Convert.ToString(Dr["REBOQUE_UF"]);
                    obj.REBOQUE_RNTC = Convert.ToString(Dr["REBOQUE_RNTC"]);
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_REBOQUE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_TRANSP_REBOQUE PesquisarTECNO_NF_TRANSP_REBOQUE(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_REBOQUE Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_TRANSP_REBOQUE obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_TRANSP_REBOQUE();
                    obj.REBOQUE_PLACA = Convert.ToString(Dr["REBOQUE_PLACA"]);
                    obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);
                    obj.REBOQUE_UF = Convert.ToString(Dr["REBOQUE_UF"]);
                    obj.REBOQUE_RNTC = Convert.ToString(Dr["REBOQUE_RNTC"]);
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_REBOQUE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
