
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
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.RETIRADA_CNPJ = Convert.ToString(Dr["RETIRADA_CNPJ"]);
                    obj.RETIRADA_XLGR = Convert.ToString(Dr["RETIRADA_XLGR"]);
                    obj.RETIRADA_NRO = Convert.ToString(Dr["RETIRADA_NRO"]);
                    obj.RETIRADA_XCPL = Convert.ToString(Dr["RETIRADA_XCPL"]);
                    obj.RETIRADA_XBAIRRO = Convert.ToString(Dr["RETIRADA_XBAIRRO"]);
                    obj.RETIRADA_CMUN = Convert.ToDecimal(Dr["RETIRADA_CMUN"]);
                    obj.RETIRADA_XMUN = Convert.ToString(Dr["RETIRADA_XMUN"]);
                    obj.RETIRADA_UF = Convert.ToString(Dr["RETIRADA_UF"]);
                    obj.RETIRADA_XNOME = Convert.ToString(Dr["RETIRADA_XNOME"]);
                    obj.RETIRADA_CEP = Convert.ToString(Dr["RETIRADA_CEP"]);
                    obj.RETIRADA_CPAIS = Convert.ToString(Dr["RETIRADA_CPAIS"]);
                    obj.RETIRADA_XPAIS = Convert.ToString(Dr["RETIRADA_XPAIS"]);
                    obj.RETIRADA_FONE = Convert.ToString(Dr["RETIRADA_FONE"]);
                    obj.RETIRADA_EMAIL = Convert.ToString(Dr["RETIRADA_EMAIL"]);
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
        public TECNO_NF_LOCAL_RETIRADA PesquisarTECNO_NF_LOCAL_RETIRADA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_LOCAL_RETIRADA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_LOCAL_RETIRADA obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_LOCAL_RETIRADA();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.RETIRADA_CNPJ = Convert.ToString(Dr["RETIRADA_CNPJ"]);
                    obj.RETIRADA_XLGR = Convert.ToString(Dr["RETIRADA_XLGR"]);
                    obj.RETIRADA_NRO = Convert.ToString(Dr["RETIRADA_NRO"]);
                    obj.RETIRADA_XCPL = Convert.ToString(Dr["RETIRADA_XCPL"]);
                    obj.RETIRADA_XBAIRRO = Convert.ToString(Dr["RETIRADA_XBAIRRO"]);
                    obj.RETIRADA_CMUN = Convert.ToDecimal(Dr["RETIRADA_CMUN"]);
                    obj.RETIRADA_XMUN = Convert.ToString(Dr["RETIRADA_XMUN"]);
                    obj.RETIRADA_UF = Convert.ToString(Dr["RETIRADA_UF"]);
                    obj.RETIRADA_XNOME = Convert.ToString(Dr["RETIRADA_XNOME"]);
                    obj.RETIRADA_CEP = Convert.ToString(Dr["RETIRADA_CEP"]);
                    obj.RETIRADA_CPAIS = Convert.ToString(Dr["RETIRADA_CPAIS"]);
                    obj.RETIRADA_XPAIS = Convert.ToString(Dr["RETIRADA_XPAIS"]);
                    obj.RETIRADA_FONE = Convert.ToString(Dr["RETIRADA_FONE"]);
                    obj.RETIRADA_EMAIL = Convert.ToString(Dr["RETIRADA_EMAIL"]);
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
