
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
                    obj.ENTREGA_CNPJ = Convert.ToString(Dr["ENTREGA_CNPJ"]);
                    obj.ENTREGA_XLGR = Convert.ToString(Dr["ENTREGA_XLGR"]);
                    obj.ENTREGA_NRO = Convert.ToString(Dr["ENTREGA_NRO"]);
                    obj.ENTREGA_XCPL = Convert.ToString(Dr["ENTREGA_XCPL"]);
                    obj.ENTREGA_XBAIRRO = Convert.ToString(Dr["ENTREGA_XBAIRRO"]);
                    obj.ENTREGA_CMUN = Convert.ToDecimal(Dr["ENTREGA_CMUN"]);
                    obj.ENTREGA_XMUN = Convert.ToString(Dr["ENTREGA_XMUN"]);
                    obj.ENTREGA_UF = Convert.ToString(Dr["ENTREGA_UF"]);
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.ENTREGA_XNOME = Convert.ToString(Dr["ENTREGA_XNOME"]);
                    obj.ENTREGA_CEP = Convert.ToString(Dr["ENTREGA_CEP"]);
                    obj.ENTREGA_CPAIS = Convert.ToString(Dr["ENTREGA_CPAIS"]);
                    obj.ENTREGA_XPAIS = Convert.ToString(Dr["ENTREGA_XPAIS"]);
                    obj.ENTREGA_FONE = Convert.ToString(Dr["ENTREGA_FONE"]);
                    obj.ENTREGA_EMAIL = Convert.ToString(Dr["ENTREGA_EMAIL"]);
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
        public TECNO_NF_LOCAL_ENTREGA PesquisarTECNO_NF_LOCAL_ENTREGA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_LOCAL_ENTREGA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_LOCAL_ENTREGA obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_LOCAL_ENTREGA();
                    obj.ENTREGA_CNPJ = Convert.ToString(Dr["ENTREGA_CNPJ"]);
                    obj.ENTREGA_XLGR = Convert.ToString(Dr["ENTREGA_XLGR"]);
                    obj.ENTREGA_NRO = Convert.ToString(Dr["ENTREGA_NRO"]);
                    obj.ENTREGA_XCPL = Convert.ToString(Dr["ENTREGA_XCPL"]);
                    obj.ENTREGA_XBAIRRO = Convert.ToString(Dr["ENTREGA_XBAIRRO"]);
                    obj.ENTREGA_CMUN = Convert.ToDecimal(Dr["ENTREGA_CMUN"]);
                    obj.ENTREGA_XMUN = Convert.ToString(Dr["ENTREGA_XMUN"]);
                    obj.ENTREGA_UF = Convert.ToString(Dr["ENTREGA_UF"]);
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.ENTREGA_XNOME = Convert.ToString(Dr["ENTREGA_XNOME"]);
                    obj.ENTREGA_CEP = Convert.ToString(Dr["ENTREGA_CEP"]);
                    obj.ENTREGA_CPAIS = Convert.ToString(Dr["ENTREGA_CPAIS"]);
                    obj.ENTREGA_XPAIS = Convert.ToString(Dr["ENTREGA_XPAIS"]);
                    obj.ENTREGA_FONE = Convert.ToString(Dr["ENTREGA_FONE"]);
                    obj.ENTREGA_EMAIL = Convert.ToString(Dr["ENTREGA_EMAIL"]);
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
