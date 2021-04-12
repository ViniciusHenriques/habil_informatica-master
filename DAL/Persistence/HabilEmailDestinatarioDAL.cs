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
    public class HabilEmailDestinatarioDAL:Conexao
    {
        string strSQL = "";
        string strSQL2 = "";


        public void InserirDestinatarios(HabilEmailDestinatario p)
        {
            try
            {
                AbrirConexao();
                strSQL = "insert into [HABIL_EMAIL_DESTINATARIO] " +
                    " (CD_INDEX, CD_EMAIL_DESTINATARIO, TP_DESTINATARIO, NM_DESTINATARIO, TX_EMAIL) " +
                    " values ( @v1, @v2, @v3, @v4, @v5);";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CD_INDEX);
                Cmd.Parameters.AddWithValue("@v2", p.CD_EMAIL_DESTINATARIO);
                Cmd.Parameters.AddWithValue("@v3", p.TP_DESTINATARIO);
                Cmd.Parameters.AddWithValue("@v4", p.NM_DESTINATARIO);
                Cmd.Parameters.AddWithValue("@v5", p.TX_EMAIL);

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
                            throw new Exception("Erro ao incluir Habil Email Destnatario: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Destinatarios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }

        public void ExcluirDestinatarios(long CD_INDEX)
        {
            try
            {
                AbrirConexao();
                strSQL = "delete from [HABIL_EMAIL_DESTINATARIO] " +
                    " where CD_INDEX = @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CD_INDEX);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Excluir Destinatarios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<HabilEmailDestinatario> ListarDestinatarioPara(long CD_Index)
        {
            List<HabilEmailDestinatario> lista = new List<HabilEmailDestinatario>();

            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Habil_Email_Destinatario] Where cd_index = @v1 and TP_DESTINATARIO = 1 ";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CD_Index);

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    HabilEmailDestinatario p = new HabilEmailDestinatario();
                    p.CD_INDEX = Convert.ToInt64(Dr["CD_INDEX"]);
                    p.CD_EMAIL_DESTINATARIO = Convert.ToInt32(Dr["CD_EMAIL_DESTINATARIO"]);
                    p.NM_DESTINATARIO= Convert.ToString(Dr["NM_DESTINATARIO"]);
                    p.TX_EMAIL = Convert.ToString(Dr["TX_EMAIL"]);
                    p.TP_DESTINATARIO = 1;
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

        public List<HabilEmailDestinatario> ListarDestinatarioCC(long CD_Index)
        {
            List<HabilEmailDestinatario> lista = new List<HabilEmailDestinatario>();

            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Habil_Email_Destinatario] Where  cd_index = @v1 and TP_DESTINATARIO = 2 ";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CD_Index);

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    HabilEmailDestinatario p = new HabilEmailDestinatario();
                    p.CD_INDEX = Convert.ToInt64(Dr["CD_INDEX"]);
                    p.CD_EMAIL_DESTINATARIO = Convert.ToInt32(Dr["CD_EMAIL_DESTINATARIO"]);
                    p.NM_DESTINATARIO = Convert.ToString(Dr["NM_DESTINATARIO"]);
                    p.TX_EMAIL = Convert.ToString(Dr["TX_EMAIL"]);
                    p.TP_DESTINATARIO = 2;
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

        public List<HabilEmailDestinatario> ListarDestinatarioCCO(long CD_Index)
        {
            List<HabilEmailDestinatario> lista = new List<HabilEmailDestinatario>();

            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Habil_Email_Destinatario] Where  cd_index = @v1 and TP_DESTINATARIO = 3 ";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CD_Index);

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    HabilEmailDestinatario p = new HabilEmailDestinatario();
                    p.CD_INDEX = Convert.ToInt64(Dr["CD_INDEX"]);
                    p.CD_EMAIL_DESTINATARIO = Convert.ToInt32(Dr["CD_EMAIL_DESTINATARIO"]);
                    p.NM_DESTINATARIO = Convert.ToString(Dr["NM_DESTINATARIO"]);
                    p.TX_EMAIL = Convert.ToString(Dr["TX_EMAIL"]);
                    p.TP_DESTINATARIO = 3;
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
    }

}
