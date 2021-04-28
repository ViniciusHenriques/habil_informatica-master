
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

        public List<TECNO_NF_REFERENCIA> ListarTECNO_NF_REFERENCIA(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_REFERENCIA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_REFERENCIA> tcn_nf = new List<TECNO_NF_REFERENCIA>();

                while (Dr.Read())
                {
                    TECNO_NF_REFERENCIA obj = new TECNO_NF_REFERENCIA();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["NITEM"] != DBNull.Value)
                        obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);

                    if (Dr["NFREF_REFNFE"] != DBNull.Value)
                        obj.NFREF_REFNFE = Convert.ToString(Dr["NFREF_REFNFE"]);

                    if (Dr["REFNF_CUF"] != DBNull.Value)
                        obj.REFNF_CUF = Convert.ToDecimal(Dr["REFNF_CUF"]);

                    if (Dr["REFNF_AAMM"] != DBNull.Value)
                        obj.REFNF_AAMM = Convert.ToDecimal(Dr["REFNF_AAMM"]);

                    if (Dr["REFNF_CNPJ"] != DBNull.Value)
                        obj.REFNF_CNPJ = Convert.ToString(Dr["REFNF_CNPJ"]);

                    if (Dr["REFNF_MOD"] != DBNull.Value)
                        obj.REFNF_MOD = Convert.ToDecimal(Dr["REFNF_MOD"]);

                    if (Dr["REFNF_NNF"] != DBNull.Value)
                        obj.REFNF_NNF = Convert.ToDecimal(Dr["REFNF_NNF"]);

                    if (Dr["REFNF_IE"] != DBNull.Value)
                        obj.REFNF_IE = Convert.ToString(Dr["REFNF_IE"]);

                    if (Dr["REFNF_CTE"] != DBNull.Value)
                        obj.REFNF_CTE = Convert.ToString(Dr["REFNF_CTE"]);

                    if (Dr["REFNF_MODECF"] != DBNull.Value)
                        obj.REFNF_MODECF = Convert.ToString(Dr["REFNF_MODECF"]);

                    if (Dr["REFNF_NECF"] != DBNull.Value)
                        obj.REFNF_NECF = Convert.ToDecimal(Dr["REFNF_NECF"]);

                    if (Dr["REFNF_NCOO"] != DBNull.Value)
                        obj.REFNF_NCOO = Convert.ToDecimal(Dr["REFNF_NCOO"]);

                    if (Dr["REFNF_SERIE"] != DBNull.Value)
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
        public TECNO_NF_REFERENCIA PesquisarTECNO_NF_REFERENCIA(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_REFERENCIA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_REFERENCIA obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_REFERENCIA();
                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["NITEM"] != DBNull.Value)
                        obj.NITEM = Convert.ToDecimal(Dr["NITEM"]);

                    if (Dr["NFREF_REFNFE"] != DBNull.Value)
                        obj.NFREF_REFNFE = Convert.ToString(Dr["NFREF_REFNFE"]);

                    if (Dr["REFNF_CUF"] != DBNull.Value)
                        obj.REFNF_CUF = Convert.ToDecimal(Dr["REFNF_CUF"]);

                    if (Dr["REFNF_AAMM"] != DBNull.Value)
                        obj.REFNF_AAMM = Convert.ToDecimal(Dr["REFNF_AAMM"]);

                    if (Dr["REFNF_CNPJ"] != DBNull.Value)
                        obj.REFNF_CNPJ = Convert.ToString(Dr["REFNF_CNPJ"]);

                    if (Dr["REFNF_MOD"] != DBNull.Value)
                        obj.REFNF_MOD = Convert.ToDecimal(Dr["REFNF_MOD"]);

                    if (Dr["REFNF_NNF"] != DBNull.Value)
                        obj.REFNF_NNF = Convert.ToDecimal(Dr["REFNF_NNF"]);

                    if (Dr["REFNF_IE"] != DBNull.Value)
                        obj.REFNF_IE = Convert.ToString(Dr["REFNF_IE"]);

                    if (Dr["REFNF_CTE"] != DBNull.Value)
                        obj.REFNF_CTE = Convert.ToString(Dr["REFNF_CTE"]);

                    if (Dr["REFNF_MODECF"] != DBNull.Value)
                        obj.REFNF_MODECF = Convert.ToString(Dr["REFNF_MODECF"]);

                    if (Dr["REFNF_NECF"] != DBNull.Value)
                        obj.REFNF_NECF = Convert.ToDecimal(Dr["REFNF_NECF"]);

                    if (Dr["REFNF_NCOO"] != DBNull.Value)
                        obj.REFNF_NCOO = Convert.ToDecimal(Dr["REFNF_NCOO"]);

                    if (Dr["REFNF_SERIE"] != DBNull.Value)
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
