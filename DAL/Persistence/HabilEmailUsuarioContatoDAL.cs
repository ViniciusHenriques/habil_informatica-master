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
    public class HabilEmailUsuarioContatoDAL:Conexao
    {
        string strSQL = "";
        string strSQL2 = "";

        public void InserirContato(HabilEmailUsuarioContato p)
        {
            string strSQL = "";
            try
            {
                AbrirConexao();
                strSQL = "insert into [HABIL_EMAIL_USUARIO_CONTATO]  (CD_USUARIO, NM_CONTATO, TX_EMAIL) values (@v1, @v2, @v3);";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CD_USUARIO);
                Cmd.Parameters.AddWithValue("@v2", p.NM_CONTATO);
                Cmd.Parameters.AddWithValue("@v3", p.TX_EMAIL);
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
                            throw new Exception("Erro ao incluir Habil Email Usuario Contato: " + ex.Message.ToString());
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
        public void AtualizarContato(HabilEmailUsuarioContato p)
        {
            string strSQL = "";
            try
            {
                AbrirConexao();
                strSQL = "update [HABIL_EMAIL_USUARIO_CONTATO] set NM_CONTATO = @v2 Where CD_USUARIO = @v1 and TX_EMAIL = @v3;";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CD_USUARIO);
                Cmd.Parameters.AddWithValue("@v2", p.NM_CONTATO);
                Cmd.Parameters.AddWithValue("@v3", p.TX_EMAIL);
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
                            throw new Exception("Erro ao alterar Habil Email Usuario Contato: " + ex.Message.ToString());
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
        public HabilEmailUsuarioContato PesquisarContato(long lngCodUsuario, string strEmail)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select * From [HABIL_EMAIL_USUARIO_CONTATO] Where CD_USUARIO = @v1 and TX_EMAIL = @v3;";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngCodUsuario);
                Cmd.Parameters.AddWithValue("@v3", strEmail);

                Dr = Cmd.ExecuteReader();

                HabilEmailUsuarioContato p = null;

                if (Dr.Read())
                {
                    p = new HabilEmailUsuarioContato();

                    p.CD_INDEX = Convert.ToInt64(Dr["CD_INDEX"]);
                    p.CD_USUARIO = Convert.ToInt64(Dr["CD_USUARIO"]);
                    p.NM_CONTATO = Convert.ToString(Dr["NM_CONTATO"]);
                    p.TX_EMAIL = Convert.ToString(Dr["TX_EMAIL"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Email Contatos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<HabilEmailUsuarioContato> ObterContatos(long lngCodUsuario)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select * From [HABIL_EMAIL_USUARIO_CONTATO] Where CD_USUARIO = @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngCodUsuario);

                Dr = Cmd.ExecuteReader();

                List<HabilEmailUsuarioContato> lista = new List<HabilEmailUsuarioContato>();    
                HabilEmailUsuarioContato p = null;

                while (Dr.Read())
                {
                    p = new HabilEmailUsuarioContato();

                    p.CD_INDEX = Convert.ToInt64(Dr["CD_INDEX"]);
                    p.CD_USUARIO = Convert.ToInt64(Dr["CD_USUARIO"]);
                    p.NM_CONTATO = Convert.ToString(Dr["NM_CONTATO"]);
                    p.TX_EMAIL = Convert.ToString(Dr["TX_EMAIL"]);
                    p.NM_CONTATO_EMAIL = Convert.ToString(Dr["TX_EMAIL"]) + " - " + Convert.ToString(Dr["NM_CONTATO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Email Contatos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void SalvarContato(HabilEmailUsuarioContato p)
        {
            try
            {
                HabilEmailUsuarioContato x = PesquisarContato(p.CD_USUARIO, p.TX_EMAIL);

                if (x == null)
                    InserirContato(p);
                else
                    AtualizarContato(p);

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Salvar Contatos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
