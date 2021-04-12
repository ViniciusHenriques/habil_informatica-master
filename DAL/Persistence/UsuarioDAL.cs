using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;


namespace DAL.Persistence
{
    public class UsuarioDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(Usuario p)
        {
            try
            {
                AbrirConexao();
                strSQL = "insert into [USUARIO] " +
                    " (NM_COMPLETO, LOGIN, SENHA, TX_GUID, CD_PFL_USUARIO, CD_SITUACAO,CD_PESSOA,CD_CARGO) " +
                    " values ( @v2, @v3, @v4, @v5, @v6, @v7,@v8,@v9)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v2", p.NomeUsuario);
                Cmd.Parameters.AddWithValue("@v3", p.Login);
                Cmd.Parameters.AddWithValue("@v4", p.Senha);
                Cmd.Parameters.AddWithValue("@v5", p.GUID);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoPerfil);
                Cmd.Parameters.AddWithValue("@v7", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoCargo);
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
                            throw new Exception("Erro ao incluir Usuario: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Usuario p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [Usuario] set NM_COMPLETO = @v2, LOGIN = @v3";
                
                if (p.Senha != null)
                    strSQL = strSQL + " , SENHA = @v4, TX_GUID = @v5";

                if (p.ResetarSenha  != null)
                    strSQL = strSQL + " , PW_RESET = @v10";

                strSQL = strSQL + " , CD_PFL_USUARIO = @v6, CD_SITUACAO = @v7, CD_PESSOA = @v8, CD_CARGO = @v9 Where [CD_Usuario] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v2", p.NomeUsuario);
                Cmd.Parameters.AddWithValue("@v3", p.Login);

                if (p.Senha != null)
                    Cmd.Parameters.AddWithValue("@v4", p.Senha);
                if (p.Senha != null)
                    Cmd.Parameters.AddWithValue("@v5", p.GUID);
                
                Cmd.Parameters.AddWithValue("@v6", p.CodigoPerfil);
                Cmd.Parameters.AddWithValue("@v7", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoCargo);

                if (p.ResetarSenha != null)
                    Cmd.Parameters.AddWithValue("@v10", p.ResetarSenha);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [Usuario] Where [CD_Usuario] = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            throw new InvalidOperationException("Exclusão não Permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao excluir Usuário: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public Usuario PesquisarUsuario(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Usuario] Where CD_Usuario = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Usuario p = null;

                if (Dr.Read())
                {
                    p = new Usuario();

                    p.CodigoUsuario= Convert.ToInt32(Dr["CD_Usuario"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_COMPLETO"]);
                    p.Login = Convert.ToString(Dr["LOGIN"]);
                    p.Senha = Convert.ToString(Dr["SENHA"]);
                    p.GUID = Convert.ToString(Dr["TX_GUID"]);
                    p.CodigoPerfil = Convert.ToInt32(Dr["CD_PFL_Usuario"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Usuario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Usuario> ListarUsuarios(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Usuario]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Usuario> lista = new List<Usuario>();

                while (Dr.Read())
                {
                    Usuario p = new Usuario();

                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_Usuario"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_COMPLETO"]);
                    p.Login = Convert.ToString(Dr["LOGIN"]);
                    p.Senha = Convert.ToString(Dr["SENHA"]);
                    p.GUID = Convert.ToString(Dr["TX_GUID"]);
                    p.CodigoPerfil = Convert.ToInt32(Dr["CD_PFL_Usuario"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Usuários: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterUsuarios(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Usuario]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoUsuario", typeof(Int32));
                dt.Columns.Add("NomeUsuario", typeof(string));
                dt.Columns.Add("Login", typeof(string));
                dt.Columns.Add("Senha", typeof(string));
                dt.Columns.Add("GUID", typeof(string));
                dt.Columns.Add("CodigoPerfil", typeof(Int32));
                dt.Columns.Add("CodigoSituacao", typeof(Int32));
                dt.Columns.Add("CodigoPessoa", typeof(Int32));
                dt.Columns.Add("CodigoCargo", typeof(Int32));


                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_Usuario"]), 
                                Convert.ToString(Dr["NM_COMPLETA"]), 
                                Convert.ToString(Dr["LOGIN"]), 
                                Convert.ToString(Dr["SENHA"]), 
                                Convert.ToInt32(Dr["CD_PFL_Usuario"]), 
                                Convert.ToInt32(Dr["CD_SITUACAO"]),
                                Convert.ToInt32(Dr["CD_PESSOA"]),
                                Convert.ToInt32(Dr["CD_CARGO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Usuários: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public Boolean UsuarioMaster(string Login, string Senha)
        {
            if ((Login == "SoftGateSis") && (Senha == "123adm"))
                return true;
            else
                return false; 
        }

        public Usuario PesquisarUsuarioPorCodPessoa(Int64 codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Usuario] Where CD_PESSOA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", codigo);

                Dr = Cmd.ExecuteReader();

                Usuario p = null;
                            
                if (Dr.Read())
                {
                    p = new Usuario();
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_Usuario"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_COMPLETO"]);
                    p.Login = Convert.ToString(Dr["LOGIN"]);
                    p.Senha = Convert.ToString(Dr["SENHA"]);
                    p.GUID = Convert.ToString(Dr["TX_GUID"]);
                    p.CodigoPerfil = Convert.ToInt32(Dr["CD_PFL_Usuario"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.ResetarSenha = Convert.ToString(Dr["PW_RESET"]);
                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);

                }


                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Usuario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }


        public Usuario PesquisarLogin(string Login, string Senha, out Boolean LoginOK)
        {
            LoginOK = false;

            try
            {
                AbrirConexao();
                strSQL = "Select * from [Usuario] Where LOGIN = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Login);

                Dr = Cmd.ExecuteReader();

                Usuario p = new Usuario();

                if (Dr.Read())
                {
                    p = new Usuario();

                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_Usuario"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_COMPLETO"]);
                    p.Login = Convert.ToString(Dr["LOGIN"]);
                    p.Senha = Convert.ToString(Dr["SENHA"]);
                    p.GUID = Convert.ToString(Dr["TX_GUID"]);
                    p.CodigoPerfil = Convert.ToInt32(Dr["CD_PFL_Usuario"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.ResetarSenha = Convert.ToString(Dr["PW_RESET"]);
                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);

                }

                clsHash clsh = new clsHash(SHA512.Create());
                string strTesteSenha = clsh.CriptografarSenha(Senha);

                if (p != null)
                    if (p.Senha == strTesteSenha)
                        LoginOK = true;

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Usuario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Usuario PesquisarCodLogin(Int64 Login, string Senha, out Boolean LoginOK)
        {
            LoginOK = false;

            try
            {
                AbrirConexao();
                strSQL = "Select * from [Usuario] Where CD_USUARIO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Login);

                Dr = Cmd.ExecuteReader();

                Usuario p = null;

                if (Dr.Read())
                {
                    p = new Usuario();

                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_Usuario"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_COMPLETO"]);
                    p.Login = Convert.ToString(Dr["LOGIN"]);
                    p.Senha = Convert.ToString(Dr["SENHA"]);
                    p.GUID = Convert.ToString(Dr["TX_GUID"]);
                    p.CodigoPerfil = Convert.ToInt32(Dr["CD_PFL_Usuario"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);
                }

                clsHash clsh = new clsHash(SHA512.Create());
                string strTesteSenha = clsh.CriptografarSenha(Senha);

                if (p != null)
                    if (p.Senha == strTesteSenha)
                        LoginOK = true;

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Usuario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Boolean VerificaDisponibilidadeLogin(string Login, string CodUsuario)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Usuario] Where LOGIN = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Login);

                Dr = Cmd.ExecuteReader();

                Usuario p = null;

                if (Dr.Read())
                {
                    p = new Usuario();

                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_Usuario"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_COMPLETO"]);
                    p.Login = Convert.ToString(Dr["LOGIN"]);
                    p.Senha = Convert.ToString(Dr["SENHA"]);
                    p.GUID = Convert.ToString(Dr["TX_GUID"]);
                    p.CodigoPerfil = Convert.ToInt32(Dr["CD_PFL_Usuario"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);
                }

                if (p == null)
                    return false;
                else
                    if (p.CodigoUsuario.ToString() != CodUsuario)
                        return true;
                    else
                        return false;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Verificar Disponibilidade Usuario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Int64 EmpresaDoLogin(string strCodUsuario, out int intQtas)
        {
            intQtas = 0;
            Int64 IntCodEmpresa = 0;

            try
            {
                AbrirConexao();
                strSQL = "select E.CD_EMPRESA "; 
                strSQL = strSQL + " From Usuario as U ";
                strSQL = strSQL + "   INNER JOIN PERFIL_DO_USUARIO AS P ";
                strSQL = strSQL + "     ON U.CD_PFL_USUARIO = P.CD_PFL_USUARIO ";
                strSQL = strSQL + "   INNER JOIN PERFIL_DO_USUARIO_NA_EMPRESA AS E ";
                strSQL = strSQL + "     ON P.CD_PFL_USUARIO = E.CD_PFL_USUARIO ";
                strSQL = strSQL + " Where U.CD_USUARIO = " + strCodUsuario;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    intQtas ++; 
                    IntCodEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                }

                return IntCodEmpresa;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Usuario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public bool  ReiniciarSenha(long lngCodUsuario)
        {
            string strUsuarioEmail = "";
            string strUsuarioSenha = "";
            string strHtml = "";
            Guid g;

            HabilEmailCriado Hec_Mail = new HabilEmailCriado();
            HabilEmailDestinatario Hec_MailDest = new HabilEmailDestinatario();

            // Create and display the value of two GUIDs.
            g = Guid.NewGuid();
            strUsuarioSenha = g.ToString();
            strUsuarioSenha = strUsuarioSenha.Substring(0, 8).ToUpper();


            try
            {
                AbrirConexao();

                Usuario u = new Usuario();
                UsuarioDAL clsUsuarioDAL = new UsuarioDAL();
                u = clsUsuarioDAL.PesquisarUsuario(lngCodUsuario);

                strHtml += "<!DOCTYPE html>" + "\n";
                strHtml += "<html>" + "\n";
                strHtml += "<head>" + "\n";
                strHtml += "<meta charset = 'utf-8'>" + "\n";
                strHtml += "<title> Hábil Informática </title>" + "\n";
                strHtml += "</head>" + "\n";
                strHtml += "<body style = 'font-family:Arial;'>" + "\n";
                strHtml += "<br/>" + "\n";
                strHtml += "<br/>" + "\n";
                strHtml += "<div style='background-color:#CCEEFF'>" + "\n";
                strHtml += "<div style='background-color:#FFFF00;text-align:center'>" + "\n";
                strHtml += "<h3>.....Hábil Informática - Gestão de Módulos do Sistema.....</h3>" + "\n";
                strHtml += "</div>" + "\n";
                strHtml += "<br/>" + "\n";
                strHtml += "<h4> &nbsp;&nbsp; &nbsp; Olá, " + u.NomeUsuario + "</h4>" + "\n";
                strHtml += "<br/>" + "\n";
                strHtml += "<br/>" + "\n";
                strHtml += "<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Devido a ajustes no cadastro de usuários do sistema, por questões de segurança,</p>" + "\n";
                strHtml += "<p>&nbsp; &nbsp; é impressindível atualizar a sua senha, sob pena de utilizar mais os módulos ou gestões do sistema.</p>" + "\n";
                strHtml += "<p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Por favor no próximo acesso, faça a atualização.</p>" + "\n";
                strHtml += "<p>&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Confirme seus dados abaixo:</p>" + "\n";
                strHtml += "<br/>" + "\n";
                strHtml += "<br/>" + "\n";
                strHtml += "<h4>&nbsp;&nbsp;&nbsp; Nome: " + u.NomeUsuario  + " </h4>" + "\n";
                strHtml += "<h4>&nbsp;&nbsp;&nbsp; Login: " + u.Login + " </h4>" + "\n";
                strHtml += "<h4>&nbsp;&nbsp;&nbsp; Senha: " + strUsuarioSenha + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button><a href='http://localhost:59900/'>Clique para Atualizar a Senha.</a></button></h4>" + "\n";
                strHtml += "<br/>" + "\n";
                strHtml += "<h4>&nbsp; &nbsp; &nbsp; Obrigado!!!</h4>" + "\n";
                strHtml += "<h4>&nbsp; &nbsp; &nbsp; Hábil Informática -Gestão de Módulos do Sistema </h4>" + "\n";
                strHtml += "<h4>&nbsp;&nbsp; &nbsp;<a href='http://www.habilinformatica.com.br/'>Site:www.habilinformatica.com.br</h4>" + "\n";
                strHtml += "<br/></div></body></html>" + "\n";

                List<HabilEmailCriado> listMails = new List<HabilEmailCriado>();
                List<HabilEmailAnexo> listAnexos = new List<HabilEmailAnexo>();
                List<HabilEmailDestinatario> listDestinatarios = new List<HabilEmailDestinatario>();

                HabilEmailCriadoDAL Hec_Mail2 = new HabilEmailCriadoDAL();

                Hec_Mail.CD_USU_REMETENTE = 0;
                Hec_Mail.IN_HTML = 1;
                Hec_Mail.TX_ASSUNTO = "Habil Informática - Módulos - Troca de Senha";
                Hec_Mail.TX_CORPO = strHtml ;
                listMails.Add(Hec_Mail);

                Hec_MailDest = new HabilEmailDestinatario();
                Hec_MailDest.CD_EMAIL_DESTINATARIO = 1;
                Hec_MailDest.TP_DESTINATARIO = 1;
                MontarEmailUsuario(lngCodUsuario, out strUsuarioEmail, out strSQL);
                Hec_MailDest.NM_DESTINATARIO = u.NomeUsuario;
                Hec_MailDest.TX_EMAIL = strUsuarioEmail;
                listDestinatarios.Add(Hec_MailDest);

                long longCodigoIndexEmail = 0;
                Hec_Mail2.Gera_Email(listMails, listDestinatarios, listAnexos, ref longCodigoIndexEmail);

                u.ResetarSenha = strUsuarioSenha;

                Atualizar(u);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Erro ao Reiniciar Senha do Usuario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        private void MontarEmailUsuario(long lngCod, out string Nome, out string Senha)
        {
            Nome = "";
            Senha = "";
            try
            {
                strSQL = "Select PC.TX_MAIL_SENHA, PC.TX_MAIL1 from USUARIO AS U WITH (NOLOCK) INNER JOIN [PESSOA_CONTATO] AS PC ON PC.CD_PESSOA = U.CD_PESSOA WHERE U.CD_USUARIO = " + lngCod.ToString();
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                if (Dr.Read())
                {
                    Nome = Dr["TX_MAIL1"].ToString();
                    Senha = Dr["TX_MAIL_SENHA"].ToString();
                }
                /**********************************************************************************************************************************************/
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Usuário: " + ex.Message.ToString());
            }
        }
    }
}
