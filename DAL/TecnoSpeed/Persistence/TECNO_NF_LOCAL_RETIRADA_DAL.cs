
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
    public class TECNO_NF_LOCAL_RETIRADA_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_LOCAL_RETIRADA> ListarTECNO_NF_LOCAL_RETIRADA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_LOCAL_RETIRADA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_LOCAL_RETIRADA> tcn_nf = new List<TECNO_NF_LOCAL_RETIRADA>();

                while (Dr.Read())
                {
                    TECNO_NF_LOCAL_RETIRADA obj = new TECNO_NF_LOCAL_RETIRADA();

                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["RETIRADA_CNPJ"] != DBNull.Value)
                        obj.RETIRADA_CNPJ = Convert.ToString(Dr["RETIRADA_CNPJ"]);

                    if (Dr["RETIRADA_XLGR"] != DBNull.Value)
                        obj.RETIRADA_XLGR = Convert.ToString(Dr["RETIRADA_XLGR"]);

                    if (Dr["RETIRADA_NRO"] != DBNull.Value)
                        obj.RETIRADA_NRO = Convert.ToString(Dr["RETIRADA_NRO"]);

                    if (Dr["RETIRADA_XCPL"] != DBNull.Value)
                        obj.RETIRADA_XCPL = Convert.ToString(Dr["RETIRADA_XCPL"]);

                    if (Dr["RETIRADA_XBAIRRO"] != DBNull.Value)
                        obj.RETIRADA_XBAIRRO = Convert.ToString(Dr["RETIRADA_XBAIRRO"]);

                    if (Dr["RETIRADA_CMUN"] != DBNull.Value)
                        obj.RETIRADA_CMUN = Convert.ToDecimal(Dr["RETIRADA_CMUN"]);

                    if (Dr["RETIRADA_XMUN"] != DBNull.Value)
                        obj.RETIRADA_XMUN = Convert.ToString(Dr["RETIRADA_XMUN"]);

                    if (Dr["RETIRADA_UF"] != DBNull.Value)
                        obj.RETIRADA_UF = Convert.ToString(Dr["RETIRADA_UF"]);

                    if (Dr["RETIRADA_XNOME"] != DBNull.Value)
                        obj.RETIRADA_XNOME = Convert.ToString(Dr["RETIRADA_XNOME"]);

                    if (Dr["RETIRADA_CEP"] != DBNull.Value)
                        obj.RETIRADA_CEP = Convert.ToString(Dr["RETIRADA_CEP"]);

                    if (Dr["RETIRADA_CPAIS"] != DBNull.Value)
                        obj.RETIRADA_CPAIS = Convert.ToString(Dr["RETIRADA_CPAIS"]);

                    if (Dr["RETIRADA_XPAIS"] != DBNull.Value)
                        obj.RETIRADA_XPAIS = Convert.ToString(Dr["RETIRADA_XPAIS"]);

                    if (Dr["RETIRADA_FONE"] != DBNull.Value)
                        obj.RETIRADA_FONE = Convert.ToString(Dr["RETIRADA_FONE"]);

                    if (Dr["RETIRADA_EMAIL"] != DBNull.Value)
                        obj.RETIRADA_EMAIL = Convert.ToString(Dr["RETIRADA_EMAIL"]);

                    if (Dr["RETIRADA_IE"] != DBNull.Value)
                        obj.RETIRADA_IE = Convert.ToString(Dr["RETIRADA_IE"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_LOCAL_RETIRADA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_LOCAL_RETIRADA PesquisarTECNO_NF_LOCAL_RETIRADA(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_LOCAL_RETIRADA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_LOCAL_RETIRADA obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_LOCAL_RETIRADA();

                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["RETIRADA_CNPJ"] != DBNull.Value)
                        obj.RETIRADA_CNPJ = Convert.ToString(Dr["RETIRADA_CNPJ"]);

                    if (Dr["RETIRADA_XLGR"] != DBNull.Value)
                        obj.RETIRADA_XLGR = Convert.ToString(Dr["RETIRADA_XLGR"]);

                    if (Dr["RETIRADA_NRO"] != DBNull.Value)
                        obj.RETIRADA_NRO = Convert.ToString(Dr["RETIRADA_NRO"]);

                    if (Dr["RETIRADA_XCPL"] != DBNull.Value)
                        obj.RETIRADA_XCPL = Convert.ToString(Dr["RETIRADA_XCPL"]);

                    if (Dr["RETIRADA_XBAIRRO"] != DBNull.Value)
                        obj.RETIRADA_XBAIRRO = Convert.ToString(Dr["RETIRADA_XBAIRRO"]);

                    if (Dr["RETIRADA_CMUN"] != DBNull.Value)
                        obj.RETIRADA_CMUN = Convert.ToDecimal(Dr["RETIRADA_CMUN"]);

                    if (Dr["RETIRADA_XMUN"] != DBNull.Value)
                        obj.RETIRADA_XMUN = Convert.ToString(Dr["RETIRADA_XMUN"]);

                    if (Dr["RETIRADA_UF"] != DBNull.Value)
                        obj.RETIRADA_UF = Convert.ToString(Dr["RETIRADA_UF"]);

                    if (Dr["RETIRADA_XNOME"] != DBNull.Value)
                        obj.RETIRADA_XNOME = Convert.ToString(Dr["RETIRADA_XNOME"]);

                    if (Dr["RETIRADA_CEP"] != DBNull.Value)
                        obj.RETIRADA_CEP = Convert.ToString(Dr["RETIRADA_CEP"]);

                    if (Dr["RETIRADA_CPAIS"] != DBNull.Value)
                        obj.RETIRADA_CPAIS = Convert.ToString(Dr["RETIRADA_CPAIS"]);

                    if (Dr["RETIRADA_XPAIS"] != DBNull.Value)
                        obj.RETIRADA_XPAIS = Convert.ToString(Dr["RETIRADA_XPAIS"]);

                    if (Dr["RETIRADA_FONE"] != DBNull.Value)
                        obj.RETIRADA_FONE = Convert.ToString(Dr["RETIRADA_FONE"]);

                    if (Dr["RETIRADA_EMAIL"] != DBNull.Value)
                        obj.RETIRADA_EMAIL = Convert.ToString(Dr["RETIRADA_EMAIL"]);

                    if (Dr["RETIRADA_IE"] != DBNull.Value)
                        obj.RETIRADA_IE = Convert.ToString(Dr["RETIRADA_IE"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_LOCAL_RETIRADA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
