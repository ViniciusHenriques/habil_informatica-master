
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
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["VBCUFDEST"] != DBNull.Value)
                        obj.VBCUFDEST = Convert.ToDecimal(Dr["VBCUFDEST"]);

                    if (Dr["PICMSUFDEST"] != DBNull.Value)
                        obj.PICMSUFDEST = Convert.ToDecimal(Dr["PICMSUFDEST"]);

                    if (Dr["PICMSINTER"] != DBNull.Value)
                        obj.PICMSINTER = Convert.ToDecimal(Dr["PICMSINTER"]);

                    if (Dr["PICMSINTERPART"] != DBNull.Value)
                        obj.PICMSINTERPART = Convert.ToDecimal(Dr["PICMSINTERPART"]);

                    if (Dr["VICMSUFDEST"] != DBNull.Value)
                        obj.VICMSUFDEST = Convert.ToDecimal(Dr["VICMSUFDEST"]);

                    if (Dr["VICMSUFREMET"] != DBNull.Value)
                        obj.VICMSUFREMET = Convert.ToDecimal(Dr["VICMSUFREMET"]);

                    if (Dr["PFCPUFDEST"] != DBNull.Value)
                        obj.PFCPUFDEST = Convert.ToDecimal(Dr["PFCPUFDEST"]);

                    if (Dr["VFCPUFDEST"] != DBNull.Value)
                        obj.VFCPUFDEST = Convert.ToDecimal(Dr["VFCPUFDEST"]);

                    if (Dr["VBCFCPUFDEST"] != DBNull.Value)
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
        public TECNO_NF_PRODUTOS_ICMS_UFDEST PesquisarTECNO_NF_PRODUTOS_ICMS_UFDEST(decimal decID_NOTA_FISCAL, decimal decPROD_NITEM)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_PRODUTOS_ICMS_UFDEST Where ID_NOTA_FISCAL = @v1 and PROD_NITEM = @v2";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);
                Cmd.Parameters.AddWithValue("@v2", decPROD_NITEM);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_PRODUTOS_ICMS_UFDEST obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_PRODUTOS_ICMS_UFDEST();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["PROD_NITEM"] != DBNull.Value)
                        obj.PROD_NITEM = Convert.ToDecimal(Dr["PROD_NITEM"]);

                    if (Dr["VBCUFDEST"] != DBNull.Value)
                        obj.VBCUFDEST = Convert.ToDecimal(Dr["VBCUFDEST"]);

                    if (Dr["PICMSUFDEST"] != DBNull.Value)
                        obj.PICMSUFDEST = Convert.ToDecimal(Dr["PICMSUFDEST"]);

                    if (Dr["PICMSINTER"] != DBNull.Value)
                        obj.PICMSINTER = Convert.ToDecimal(Dr["PICMSINTER"]);

                    if (Dr["PICMSINTERPART"] != DBNull.Value)
                        obj.PICMSINTERPART = Convert.ToDecimal(Dr["PICMSINTERPART"]);

                    if (Dr["VICMSUFDEST"] != DBNull.Value)
                        obj.VICMSUFDEST = Convert.ToDecimal(Dr["VICMSUFDEST"]);

                    if (Dr["VICMSUFREMET"] != DBNull.Value)
                        obj.VICMSUFREMET = Convert.ToDecimal(Dr["VICMSUFREMET"]);

                    if (Dr["PFCPUFDEST"] != DBNull.Value)
                        obj.PFCPUFDEST = Convert.ToDecimal(Dr["PFCPUFDEST"]);

                    if (Dr["VFCPUFDEST"] != DBNull.Value)
                        obj.VFCPUFDEST = Convert.ToDecimal(Dr["VFCPUFDEST"]);

                    if (Dr["VBCFCPUFDEST"] != DBNull.Value)
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
