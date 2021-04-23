
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
    public class TECNO_NF_PRODUTOS_ICMS_UFDEST_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_PRODUTOS_ICMS_UFDEST> ListarTECNO_NF_PRODUTOS_ICMS_UFDEST(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_ICMS_UFDEST Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_PRODUTOS_ICMS_UFDEST> tcn_nf = new List<TECNO_NF_PRODUTOS_ICMS_UFDEST>();

                while (Dr.Read())
                {
                    TECNO_NF_PRODUTOS_ICMS_UFDEST obj = new TECNO_NF_PRODUTOS_ICMS_UFDEST();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.VBCUFDEST = Convert.ToDecimal(Dr["VBCUFDEST"]);
                    obj.PICMSUFDEST = Convert.ToDecimal(Dr["PICMSUFDEST"]);
                    obj.PICMSINTER = Convert.ToDecimal(Dr["PICMSINTER"]);
                    obj.PICMSINTERPART = Convert.ToDecimal(Dr["PICMSINTERPART"]);
                    obj.VICMSUFDEST = Convert.ToDecimal(Dr["VICMSUFDEST"]);
                    obj.VICMSUFREMET = Convert.ToDecimal(Dr["VICMSUFREMET"]);
                    obj.PFCPUFDEST = Convert.ToDecimal(Dr["PFCPUFDEST"]);
                    obj.VFCPUFDEST = Convert.ToDecimal(Dr["VFCPUFDEST"]);
                    obj.VBCFCPUFDEST = Convert.ToDecimal(Dr["VBCFCPUFDEST"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_ICMS_UFDEST: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_PRODUTOS_ICMS_UFDEST PesquisarTECNO_NF_PRODUTOS_ICMS_UFDEST(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_ICMS_UFDEST Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_ICMS_UFDEST obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_ICMS_UFDEST();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);
                    obj.VBCUFDEST = Convert.ToDecimal(Dr["VBCUFDEST"]);
                    obj.PICMSUFDEST = Convert.ToDecimal(Dr["PICMSUFDEST"]);
                    obj.PICMSINTER = Convert.ToDecimal(Dr["PICMSINTER"]);
                    obj.PICMSINTERPART = Convert.ToDecimal(Dr["PICMSINTERPART"]);
                    obj.VICMSUFDEST = Convert.ToDecimal(Dr["VICMSUFDEST"]);
                    obj.VICMSUFREMET = Convert.ToDecimal(Dr["VICMSUFREMET"]);
                    obj.PFCPUFDEST = Convert.ToDecimal(Dr["PFCPUFDEST"]);
                    obj.VFCPUFDEST = Convert.ToDecimal(Dr["VFCPUFDEST"]);
                    obj.VBCFCPUFDEST = Convert.ToDecimal(Dr["VBCFCPUFDEST"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_PRODUTOS_ICMS_UFDEST: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
