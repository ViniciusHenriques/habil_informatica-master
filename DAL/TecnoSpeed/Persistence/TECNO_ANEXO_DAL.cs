
using System.IO;
using System.Linq;
using System.Text;
using DAL.Model;
using DAL.Persistence;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using DAL.TecnoSpeed.Model;

namespace DAL.TecnoSpeed.Persistence
{
    public class TECNO_ANEXO_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_ANEXO> ListarTECNO_ANEXO(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_ANEXO Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_ANEXO> tcn_nf = new List<TECNO_ANEXO>();

                while (Dr.Read())
                {
                    TECNO_ANEXO obj = new TECNO_ANEXO();

                    if (Dr["CHAVE_BUSCA"] != DBNull.Value)
                        obj.CHAVE_BUSCA = Convert.ToString(Dr["CHAVE_BUSCA"]);

                    if (Dr["CD_ANEXO"] != DBNull.Value)
                        obj.CD_ANEXO = Convert.ToShort(Dr["CD_ANEXO"]);

                    if (Dr["DS_ARQUIVO"] != DBNull.Value)
                        obj.DS_ARQUIVO = Convert.ToString(Dr["DS_ARQUIVO"]);

                    if (Dr["TX_CONTEUDO"] != DBNull.Value)
                        obj.TX_CONTEUDO = Convert.ToByte[](Dr["TX_CONTEUDO"]);

                    if (Dr["EX_ARQUIVO"] != DBNull.Value)
                        obj.EX_ARQUIVO = Convert.ToString(Dr["EX_ARQUIVO"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_ANEXO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_ANEXO PesquisarTECNO_ANEXO(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_ANEXO Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_ANEXO obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_ANEXO();
                    obj.CHAVE_BUSCA = Convert.ToString(Dr["CHAVE_BUSCA"]);
                    obj.CD_ANEXO = Convert.ToInt16(Dr["CD_ANEXO"]);
                    obj.DS_ARQUIVO = Convert.ToString(Dr["DS_ARQUIVO"]);
                    obj.TX_CONTEUDO = (byte[])Dr["TX_CONTEUDO"];
                    obj.EX_ARQUIVO = Convert.ToString(Dr["EX_ARQUIVO"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_ANEXO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}

