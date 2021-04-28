
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
    public class TECNO_NF_LOCAL_ENTREGA_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_LOCAL_ENTREGA> ListarTECNO_NF_LOCAL_ENTREGA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_LOCAL_ENTREGA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_LOCAL_ENTREGA> tcn_nf = new List<TECNO_NF_LOCAL_ENTREGA>();

                while (Dr.Read())
                {
                    TECNO_NF_LOCAL_ENTREGA obj = new TECNO_NF_LOCAL_ENTREGA();
                    if (Dr["ENTREGA_CNPJ"] != DBNull.Value)
                        obj.ENTREGA_CNPJ = Convert.ToString(Dr["ENTREGA_CNPJ"]);

                    if (Dr["ENTREGA_XLGR"] != DBNull.Value)
                        obj.ENTREGA_XLGR = Convert.ToString(Dr["ENTREGA_XLGR"]);

                    if (Dr["ENTREGA_NRO"] != DBNull.Value)
                        obj.ENTREGA_NRO = Convert.ToString(Dr["ENTREGA_NRO"]);

                    if (Dr["ENTREGA_XCPL"] != DBNull.Value)
                        obj.ENTREGA_XCPL = Convert.ToString(Dr["ENTREGA_XCPL"]);

                    if (Dr["ENTREGA_XBAIRRO"] != DBNull.Value)
                        obj.ENTREGA_XBAIRRO = Convert.ToString(Dr["ENTREGA_XBAIRRO"]);

                    if (Dr["ENTREGA_CMUN"] != DBNull.Value)
                        obj.ENTREGA_CMUN = Convert.ToDecimal(Dr["ENTREGA_CMUN"]);

                    if (Dr["ENTREGA_XMUN"] != DBNull.Value)
                        obj.ENTREGA_XMUN = Convert.ToString(Dr["ENTREGA_XMUN"]);

                    if (Dr["ENTREGA_UF"] != DBNull.Value)
                        obj.ENTREGA_UF = Convert.ToString(Dr["ENTREGA_UF"]);

                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["ENTREGA_XNOME"] != DBNull.Value)
                        obj.ENTREGA_XNOME = Convert.ToString(Dr["ENTREGA_XNOME"]);

                    if (Dr["ENTREGA_CEP"] != DBNull.Value)
                        obj.ENTREGA_CEP = Convert.ToString(Dr["ENTREGA_CEP"]);

                    if (Dr["ENTREGA_CPAIS"] != DBNull.Value)
                        obj.ENTREGA_CPAIS = Convert.ToString(Dr["ENTREGA_CPAIS"]);

                    if (Dr["ENTREGA_XPAIS"] != DBNull.Value)
                        obj.ENTREGA_XPAIS = Convert.ToString(Dr["ENTREGA_XPAIS"]);

                    if (Dr["ENTREGA_FONE"] != DBNull.Value)
                        obj.ENTREGA_FONE = Convert.ToString(Dr["ENTREGA_FONE"]);

                    if (Dr["ENTREGA_EMAIL"] != DBNull.Value)
                        obj.ENTREGA_EMAIL = Convert.ToString(Dr["ENTREGA_EMAIL"]);

                    if (Dr["ENTREGA_IE"] != DBNull.Value)
                        obj.ENTREGA_IE = Convert.ToString(Dr["ENTREGA_IE"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_LOCAL_ENTREGA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_LOCAL_ENTREGA PesquisarTECNO_NF_LOCAL_ENTREGA(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_LOCAL_ENTREGA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_LOCAL_ENTREGA obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_LOCAL_ENTREGA();
                    if (Dr["ENTREGA_CNPJ"] != DBNull.Value)
                        obj.ENTREGA_CNPJ = Convert.ToString(Dr["ENTREGA_CNPJ"]);

                    if (Dr["ENTREGA_XLGR"] != DBNull.Value)
                        obj.ENTREGA_XLGR = Convert.ToString(Dr["ENTREGA_XLGR"]);

                    if (Dr["ENTREGA_NRO"] != DBNull.Value)
                        obj.ENTREGA_NRO = Convert.ToString(Dr["ENTREGA_NRO"]);

                    if (Dr["ENTREGA_XCPL"] != DBNull.Value)
                        obj.ENTREGA_XCPL = Convert.ToString(Dr["ENTREGA_XCPL"]);

                    if (Dr["ENTREGA_XBAIRRO"] != DBNull.Value)
                        obj.ENTREGA_XBAIRRO = Convert.ToString(Dr["ENTREGA_XBAIRRO"]);

                    if (Dr["ENTREGA_CMUN"] != DBNull.Value)
                        obj.ENTREGA_CMUN = Convert.ToDecimal(Dr["ENTREGA_CMUN"]);

                    if (Dr["ENTREGA_XMUN"] != DBNull.Value)
                        obj.ENTREGA_XMUN = Convert.ToString(Dr["ENTREGA_XMUN"]);

                    if (Dr["ENTREGA_UF"] != DBNull.Value)
                        obj.ENTREGA_UF = Convert.ToString(Dr["ENTREGA_UF"]);

                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["ENTREGA_XNOME"] != DBNull.Value)
                        obj.ENTREGA_XNOME = Convert.ToString(Dr["ENTREGA_XNOME"]);

                    if (Dr["ENTREGA_CEP"] != DBNull.Value)
                        obj.ENTREGA_CEP = Convert.ToString(Dr["ENTREGA_CEP"]);

                    if (Dr["ENTREGA_CPAIS"] != DBNull.Value)
                        obj.ENTREGA_CPAIS = Convert.ToString(Dr["ENTREGA_CPAIS"]);

                    if (Dr["ENTREGA_XPAIS"] != DBNull.Value)
                        obj.ENTREGA_XPAIS = Convert.ToString(Dr["ENTREGA_XPAIS"]);

                    if (Dr["ENTREGA_FONE"] != DBNull.Value)
                        obj.ENTREGA_FONE = Convert.ToString(Dr["ENTREGA_FONE"]);

                    if (Dr["ENTREGA_EMAIL"] != DBNull.Value)
                        obj.ENTREGA_EMAIL = Convert.ToString(Dr["ENTREGA_EMAIL"]);

                    if (Dr["ENTREGA_IE"] != DBNull.Value)
                        obj.ENTREGA_IE = Convert.ToString(Dr["ENTREGA_IE"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_LOCAL_ENTREGA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
