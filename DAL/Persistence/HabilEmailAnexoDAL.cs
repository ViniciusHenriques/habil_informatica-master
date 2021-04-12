using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Persistence;
using DAL.Model;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class HabilEmailAnexoDAL:Conexao
    {
        public void InserirAnexos(HabilEmailAnexo p)
        {
            string strSQL = "";
            string strSQL2 = "";

            try
            {
                AbrirConexao();
                strSQL = "insert into [HABIL_EMAIL_ANEXO]  (CD_INDEX, CD_ANEXO, TX_CONTEUDO, CD_EXTENSAO, DS_ARQUIVO) values (@v1, @v2, @v3, @v4, @v5);";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CD_INDEX);
                Cmd.Parameters.AddWithValue("@v2", p.CD_ANEXO);
                Cmd.Parameters.AddWithValue("@v3", p.TX_CONTEUDO);
                Cmd.Parameters.AddWithValue("@v4", p.CD_EXTENSAO);
                Cmd.Parameters.AddWithValue("@v5", p.DS_ARQUIVO);
                Cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao incluir Habil Email Anexo: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Anexo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public List<HabilEmailAnexo> ListarAnexos(long CD_Index)
        {
            List<HabilEmailAnexo> lista = new List<HabilEmailAnexo>();

            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Habil_Email_Anexo] Where cd_index = @v1 ";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CD_Index);

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    HabilEmailAnexo p = new HabilEmailAnexo();
                    p.CD_INDEX = Convert.ToInt64(Dr["CD_INDEX"]);
                    p.CD_ANEXO = Convert.ToInt32(Dr["CD_ANEXO"]);
                    p.TX_CONTEUDO = (byte[])Dr["TX_CONTEUDO"];
                    p.CD_EXTENSAO = Convert.ToInt64(Dr["CD_EXTENSAO"]);
                    p.DS_ARQUIVO = Dr["DS_ARQUIVO"].ToString();
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Emails: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirAnexos(long CD_INDEX)
        {
            try
            {
                AbrirConexao();
                string strSQL = "delete from [HABIL_EMAIL_ANEXO] " +
                    " where CD_INDEX = @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CD_INDEX);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Excluir Anexos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
