﻿
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
    public class TECNO_NF_TRANSP_TRANSPORTA_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF_TRANSP_TRANSPORTA> ListarTECNO_NF_TRANSP_TRANSPORTA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_TRANSPORTA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF_TRANSP_TRANSPORTA> tcn_nf = new List<TECNO_NF_TRANSP_TRANSPORTA>();

                while (Dr.Read())
                {
                    TECNO_NF_TRANSP_TRANSPORTA obj = new TECNO_NF_TRANSP_TRANSPORTA();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.TRANSPORTA_CNPJ_CPF = Convert.ToString(Dr["TRANSPORTA_CNPJ_CPF"]);
                    obj.TRANSPORTA_XNOME = Convert.ToString(Dr["TRANSPORTA_XNOME"]);
                    obj.TRANSPORTA_IE = Convert.ToString(Dr["TRANSPORTA_IE"]);
                    obj.TRANSPORTA_XENDER = Convert.ToString(Dr["TRANSPORTA_XENDER"]);
                    obj.TRANSPORTA_XMUN = Convert.ToString(Dr["TRANSPORTA_XMUN"]);
                    obj.TRANSPORTA_UF = Convert.ToString(Dr["TRANSPORTA_UF"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_TRANSPORTA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_NF_TRANSP_TRANSPORTA PesquisarTECNO_NF_TRANSP_TRANSPORTA(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF_TRANSP_TRANSPORTA Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF_TRANSP_TRANSPORTA obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF_TRANSP_TRANSPORTA();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.TRANSPORTA_CNPJ_CPF = Convert.ToString(Dr["TRANSPORTA_CNPJ_CPF"]);
                    obj.TRANSPORTA_XNOME = Convert.ToString(Dr["TRANSPORTA_XNOME"]);
                    obj.TRANSPORTA_IE = Convert.ToString(Dr["TRANSPORTA_IE"]);
                    obj.TRANSPORTA_XENDER = Convert.ToString(Dr["TRANSPORTA_XENDER"]);
                    obj.TRANSPORTA_XMUN = Convert.ToString(Dr["TRANSPORTA_XMUN"]);
                    obj.TRANSPORTA_UF = Convert.ToString(Dr["TRANSPORTA_UF"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF_TRANSP_TRANSPORTA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
