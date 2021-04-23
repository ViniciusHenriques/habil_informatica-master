
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
    public class TECNO_NF_REFERENCIA_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_REFERENCIA> ListarTECNO_NF_REFERENCIA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_REFERENCIA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_REFERENCIA> tcn_nf = new List<TECNO_NF_REFERENCIA>();

                while (Dr.Read())
                {
                    TECNO_NF_REFERENCIA obj = new TECNO_NF_REFERENCIA();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);
                    obj.NFREF_REFNFE = Convert.ToString(Dr["NFREF_REFNFE"]);
                    obj.REFNF_CUF = Convert.ToDecimal(Dr["REFNF_CUF"]);
                    obj.REFNF_AAMM = Convert.ToDecimal(Dr["REFNF_AAMM"]);
                    obj.REFNF_CNPJ = Convert.ToString(Dr["REFNF_CNPJ"]);
                    obj.REFNF_MOD = Convert.ToDecimal(Dr["REFNF_MOD"]);
                    obj.REFNF_NNF = Convert.ToDecimal(Dr["REFNF_NNF"]);
                    obj.REFNF_IE = Convert.ToString(Dr["REFNF_IE"]);
                    obj.REFNF_CTE = Convert.ToString(Dr["REFNF_CTE"]);
                    obj.REFNF_MODECF = Convert.ToString(Dr["REFNF_MODECF"]);
                    obj.REFNF_NECF = Convert.ToDecimal(Dr["REFNF_NECF"]);
                    obj.REFNF_NCOO = Convert.ToDecimal(Dr["REFNF_NCOO"]);
                    obj.REFNF_SERIE = Convert.ToString(Dr["REFNF_SERIE"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_REFERENCIA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_REFERENCIA PesquisarTECNO_NF_REFERENCIA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_REFERENCIA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_REFERENCIA obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_REFERENCIA();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);
                    obj.NFREF_REFNFE = Convert.ToString(Dr["NFREF_REFNFE"]);
                    obj.REFNF_CUF = Convert.ToDecimal(Dr["REFNF_CUF"]);
                    obj.REFNF_AAMM = Convert.ToDecimal(Dr["REFNF_AAMM"]);
                    obj.REFNF_CNPJ = Convert.ToString(Dr["REFNF_CNPJ"]);
                    obj.REFNF_MOD = Convert.ToDecimal(Dr["REFNF_MOD"]);
                    obj.REFNF_NNF = Convert.ToDecimal(Dr["REFNF_NNF"]);
                    obj.REFNF_IE = Convert.ToString(Dr["REFNF_IE"]);
                    obj.REFNF_CTE = Convert.ToString(Dr["REFNF_CTE"]);
                    obj.REFNF_MODECF = Convert.ToString(Dr["REFNF_MODECF"]);
                    obj.REFNF_NECF = Convert.ToDecimal(Dr["REFNF_NECF"]);
                    obj.REFNF_NCOO = Convert.ToDecimal(Dr["REFNF_NCOO"]);
                    obj.REFNF_SERIE = Convert.ToString(Dr["REFNF_SERIE"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_REFERENCIA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
