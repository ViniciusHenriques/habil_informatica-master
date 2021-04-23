
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
    public class TECNO_NF_FORMA_PGTO_CARTAO_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_FORMA_PGTO_CARTAO> ListarTECNO_NF_FORMA_PGTO_CARTAO(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_FORMA_PGTO_CARTAO Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_FORMA_PGTO_CARTAO> tcn_nf = new List<TECNO_NF_FORMA_PGTO_CARTAO>();

                while (Dr.Read())
                {
                    TECNO_NF_FORMA_PGTO_CARTAO obj = new TECNO_NF_FORMA_PGTO_CARTAO();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.FORMA_PGTO_NITEM = Convert.ToDecimal(Dr["FORMA_PGTO_NITEM"]);
                    obj.CNPJ = Convert.ToString(Dr["CNPJ"]);
                    obj.TBAND = Convert.ToString(Dr["TBAND"]);
                    obj.CAUT = Convert.ToString(Dr["CAUT"]);
                    obj.TPINTEGRA = Convert.ToDecimal(Dr["TPINTEGRA"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_FORMA_PGTO_CARTAO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_FORMA_PGTO_CARTAO PesquisarTECNO_NF_FORMA_PGTO_CARTAO(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_FORMA_PGTO_CARTAO Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_FORMA_PGTO_CARTAO obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_FORMA_PGTO_CARTAO();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.FORMA_PGTO_NITEM = Convert.ToDecimal(Dr["FORMA_PGTO_NITEM"]);
                    obj.CNPJ = Convert.ToString(Dr["CNPJ"]);
                    obj.TBAND = Convert.ToString(Dr["TBAND"]);
                    obj.CAUT = Convert.ToString(Dr["CAUT"]);
                    obj.TPINTEGRA = Convert.ToDecimal(Dr["TPINTEGRA"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_FORMA_PGTO_CARTAO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
