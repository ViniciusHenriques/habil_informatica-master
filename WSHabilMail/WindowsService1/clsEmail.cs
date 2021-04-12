using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.IO;

namespace WSHabilMail
{
    class clsEmail
    {

        private string strRetornoDoErro = "";

        private List<clsHabilEmailCriado> listMails = new List<clsHabilEmailCriado>();
        private List<clsHabilEmailAnexo> listAnexos = new List<clsHabilEmailAnexo>();
        private List<clsHabilEmailDestinatario> listDestinatarios = new List<clsHabilEmailDestinatario>();

        private List<clsHabilEmailCriado> listMailsPreparadoParaEnvio = new List<clsHabilEmailCriado>();
        private List<clsHabilEmailAnexo> listAnexosPreparadoParaEnvio = new List<clsHabilEmailAnexo>();
        private List<clsHabilEmailDestinatario> listDestinatariosPreparadoParaEnvio = new List<clsHabilEmailDestinatario>();

        private clsHabilEmailCriado CadlistMails = new clsHabilEmailCriado();
        private clsHabilEmailAnexo CadlistAnexos = new clsHabilEmailAnexo();
        private clsHabilEmailDestinatario CadlistDestinatarios = new clsHabilEmailDestinatario();

        private bool ValidarEmail(String email)
        {
            bool emailValido = false;

            //Expressão regular retirada de
            //https://msdn.microsoft.com/pt-br/library/01escwtf(v=vs.110).aspx
            string emailRegex = string.Format("{0}{1}",
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\\+/=\?\^`\{\}\|~\w]))(?<=[0-9a-z])@))",
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w][0-9a-z]\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

            try
            {
                emailValido = Regex.IsMatch(
                    email,
                    emailRegex);

                emailValido = true;
            }
            catch (Exception ex )
            {
                string x = ex.Message.ToString();
                strRetornoDoErro = "E-Mail: " + email + "Inválido.";
                emailValido = false;
            }

            return emailValido;
        }
        public void ProcessaEnvioMails()
        {
            bool blnX;
            try
            {
                VerificarAgendamentoCompromisso();
                blnX = EmailsGravaLista();
                if (blnX) 
                    PrepararEmailParaEnvio();
            }
            catch (Exception ex)
            {
                string x = ex.Message.ToString();
            }
        }

        private List<clsHabilEmailCriado> VerificarAgendamentoCompromisso()
        {
            Config.ConfigData config = Config.GetConfigData();
            SqlConnection conn = new SqlConnection(config.ConnectString);

            try
            {
                conn.Open();
                List<clsHabilEmailCriado> ListaEmails = new List<clsHabilEmailCriado>();
                SqlCommand cmd = new SqlCommand("SELECT AGE.*,P.NM_PESSOA FROM " +
                                                    "AGENDAMENTO_DE_COMPROMISSO AS AGE " +
                                                    "INNER JOIN PESSOA AS P ON P.CD_PESSOA = AGE.CD_PESSOA " +
                                                    "INNER JOIN PARAMETROS_DO_SISTEMA AS PAR ON PAR.CD_EMPRESA = AGE.CD_EMPRESA " +
                                                "WHERE(GETDATE() < DT_HR_AGENDAMENTO AND GETDATE() > DATEADD(HOUR, (PAR.NR_HR_ENVIO_ALERTA * -1), DT_HR_AGENDAMENTO)) " +
                                                "AND IN_EMAIL = 1 AND IN_EMAIL_ENVIADO <> 1", conn);

                SqlDataReader drSQL = null;
                drSQL = cmd.ExecuteReader();
                
                while (drSQL.Read())
                {
                    clsHabilEmailCriado Hec_Mail = new clsHabilEmailCriado();

                    List<clsHabilEmailCriado> listMails = new List<clsHabilEmailCriado>();
                    List<clsHabilEmailAnexo> listAnexos = new List<clsHabilEmailAnexo>();
                    List<clsHabilEmailDestinatario> listDestinatarios = new List<clsHabilEmailDestinatario>();

                    Hec_Mail.CD_USU_REMETENTE = 0;
                    Hec_Mail.IN_HTML = 1;
                    Hec_Mail.TX_ASSUNTO = "Agendamento às " + Convert.ToDateTime(drSQL["DT_HR_AGENDAMENTO"]).ToString("HH:mm");
                    Hec_Mail.TX_CORPO = "<div style='text-align:center'>" +
                                            "<h2>" + 
                                                Convert.ToDateTime(drSQL["DT_HR_AGENDAMENTO"]).ToString("dd/MM/yyyy") + " às " +
                                                Convert.ToDateTime(drSQL["DT_HR_AGENDAMENTO"]).ToString("HH:mm") + 
                                            "</h2>" +   
                                            "</br></br>" +
                                            "<p style='font-size:25px'>" 
                                                + Convert.ToString(drSQL["DS_ANOTACAO"]) +
                                            "</p>" +
                                        "</div>";
                     
                    Hec_Mail.CD_SITUACAO = 113;
                    Hec_Mail.CD_USU_REMETENTE = Convert.ToInt32(drSQL["CD_USUARIO"]); 
                    listMails.Add(Hec_Mail);

                    clsHabilEmailDestinatario Hec_MailDestCliente = new clsHabilEmailDestinatario();
                    Hec_MailDestCliente.CD_EMAIL_DESTINATARIO = 1;
                    Hec_MailDestCliente.TP_DESTINATARIO = 1;
                    Hec_MailDestCliente.NM_DESTINATARIO = drSQL["NM_PESSOA"].ToString();
                    Hec_MailDestCliente.TX_EMAIL = Convert.ToString(drSQL["DS_CONTATO"]);
                    listDestinatarios.Add(Hec_MailDestCliente);

                    listDestinatarios = MontaDestinatarios(Convert.ToInt32(drSQL["CD_TIPO_AGENDAMENTO"]), Convert.ToInt32(drSQL["CD_INDEX"]), listDestinatarios);

                    long longCodigoIndexEmail = 0;
                    Gera_Email(listMails, listDestinatarios, listAnexos, ref longCodigoIndexEmail);
                   
                    AtualizarAgendamentoCompromisso(Convert.ToInt32(drSQL["CD_INDEX"]));

                }

                return ListaEmails;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos agendamentos: " + ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }          
        }
        private List<clsHabilEmailDestinatario> MontaDestinatarios(int CodigoTipoAgendamento, int CodigoAgendamento, List<clsHabilEmailDestinatario> ListaDestinatario)
        {
            Config.ConfigData config = Config.GetConfigData();
            SqlConnection conn = new SqlConnection(config.ConnectString);

            try
            {
                conn.Open();
                string SQL = "";
                if (CodigoTipoAgendamento == 171)// SE FOR PRIVADO
                    SQL = "SELECT PCTT.TX_MAIL1,U.NM_COMPLETO FROM  " +
                                "USUARIO_DO_AGENDAMENTO AS USU_A " +
                                "INNER JOIN AGENDAMENTO_DE_COMPROMISSO AS AGE ON AGE.CD_INDEX = USU_A.CD_AGENDAMENTO " +
                                "INNER JOIN USUARIO AS U ON U.CD_USUARIO = USU_A.CD_USUARIO " +
                                "INNER JOIN PESSOA_CONTATO AS PCTT ON PCTT.CD_PESSOA = U.CD_PESSOA AND PCTT.CD_CONTATO = 1 " +
                            "WHERE CD_AGENDAMENTO = " + CodigoAgendamento +
                            " UNION " +
                            "SELECT A.DS_CONTATO AS TX_MAIL1 ,P.NM_PESSOA AS NM_COMPLETO " +
                            "FROM AGENDAMENTO_DE_COMPROMISSO AS A " +
                               "INNER JOIN PESSOA AS P ON P.CD_PESSOA = A.CD_PESSOA " +
                            "WHERE A.CD_INDEX = " +CodigoAgendamento ;
                else // SE FOR PUBLICO
                    SQL = "SELECT TX_MAIL1,U.NM_COMPLETO FROM " +
                            "USUARIO AS U " +
                            " INNER JOIN PESSOA_CONTATO AS PCTT ON PCTT.CD_PESSOA = U.CD_PESSOA " +
                            "UNION " +
                            "SELECT A.DS_CONTATO AS TX_MAIL1 ,P.NM_PESSOA AS NM_COMPLETO " +
                            "FROM AGENDAMENTO_DE_COMPROMISSO AS A " +
                               "INNER JOIN PESSOA AS P ON P.CD_PESSOA = A.CD_PESSOA " +
                            "WHERE A.CD_INDEX = " + CodigoAgendamento;


                SqlCommand cmd2 = new SqlCommand(SQL, conn);
                SqlDataReader drSQL = null;
                drSQL = cmd2.ExecuteReader();


                int i = 1;
                while (drSQL.Read())
                {
                    i++;
                    clsHabilEmailDestinatario Hec_MailDest = new clsHabilEmailDestinatario();
                    Hec_MailDest.CD_EMAIL_DESTINATARIO = i;
                    Hec_MailDest.TP_DESTINATARIO = 1;
                    Hec_MailDest.NM_DESTINATARIO = drSQL["NM_COMPLETO"].ToString();
                    Hec_MailDest.TX_EMAIL = Convert.ToString(drSQL["TX_MAIL1"]);
                    ListaDestinatario.Add(Hec_MailDest);
                }
                conn.Close();

                

                return ListaDestinatario;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar agendamento de compromisso: " + ex.Message.ToString());
            }
        }
        private void AtualizarAgendamentoCompromisso(int CodigoAgendamento)
        {
            Config.ConfigData config = Config.GetConfigData();
            SqlConnection conn = new SqlConnection(config.ConnectString);
            try
            {
                conn.Open();

                SqlCommand Cmd = new SqlCommand("UPDATE AGENDAMENTO_DE_COMPROMISSO SET IN_EMAIL_ENVIADO = 1 WHERE CD_INDEX = @v1", conn);
                
                Cmd.Parameters.AddWithValue("@v1", CodigoAgendamento);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar agendamento de compromisso: " + ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    
        public void InserirEmailCriado(clsHabilEmailCriado p, ref long longCodigoIndexEmail)
        {
            Config.ConfigData config = Config.GetConfigData();
            SqlConnection conn = new SqlConnection(config.ConnectString);
            try
            {
                conn.Open();

                SqlCommand Cmd = new SqlCommand("insert into [HABIL_EMAIL_CRIADO] " +
                    " (CD_USU_REMETENTE, TX_ASSUNTO, IN_HTML, TX_CORPO, CD_SITUACAO) " +
                    " values ( @v1, @v2, @v3, @v4, @v5); SELECT SCOPE_IDENTITY()",conn);
                
                
                Cmd.Parameters.AddWithValue("@v1", p.CD_USU_REMETENTE);
                Cmd.Parameters.AddWithValue("@v2", p.TX_ASSUNTO);
                Cmd.Parameters.AddWithValue("@v3", p.IN_HTML);
                Cmd.Parameters.AddWithValue("@v4", p.TX_CORPO);
                Cmd.Parameters.AddWithValue("@v5", p.CD_SITUACAO);

                p.CD_INDEX = Convert.ToInt64(Cmd.ExecuteScalar());
                longCodigoIndexEmail = p.CD_INDEX;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Usuário: " + ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public Boolean Gera_Email(List<clsHabilEmailCriado> lstMail, List<clsHabilEmailDestinatario> lstDest, List<clsHabilEmailAnexo> lstAnexo, ref long longCodigoIndexEmail)
        {
            try
            {

                foreach (var item in lstMail)
                {
                    InserirEmailCriado(item, ref longCodigoIndexEmail);

                    foreach (var item2 in lstDest)
                    {
                        item2.CD_INDEX = item.CD_INDEX;
                        InserirDestinatarios(item2);
                    }

                    foreach (var item3 in lstAnexo)
                    {
                        item3.CD_INDEX = item.CD_INDEX;
                        InserirAnexos(item3);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;

                throw new Exception(ex.Message.ToString());
            }
        }
        public void InserirAnexos(clsHabilEmailAnexo p)
        {
            Config.ConfigData config = Config.GetConfigData();
            SqlConnection conn = new SqlConnection(config.ConnectString);

            try
            {
                conn.Open();
                SqlCommand Cmd = new SqlCommand("insert into [HABIL_EMAIL_ANEXO]  (CD_INDEX, CD_ANEXO, TX_CONTEUDO, CD_EXTENSAO, DS_ARQUIVO) values (@v1, @v2, @v3, @v4, @v5);",conn);
                Cmd.Parameters.AddWithValue("@v1", p.CD_INDEX);
                Cmd.Parameters.AddWithValue("@v2", p.CD_ANEXO);
                Cmd.Parameters.AddWithValue("@v3", p.TX_CONTEUDO);
                Cmd.Parameters.AddWithValue("@v4", p.CD_EXTENSAO);
                Cmd.Parameters.AddWithValue("@v5", p.DS_ARQUIVO);
                Cmd.ExecuteReader();
            }

            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Anexo: " + ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public void InserirDestinatarios(clsHabilEmailDestinatario p)
        {
            Config.ConfigData config = Config.GetConfigData();
            SqlConnection conn = new SqlConnection(config.ConnectString);
            try
            {
                conn.Open();
                SqlCommand Cmd = new SqlCommand("insert into [HABIL_EMAIL_DESTINATARIO] " +
                    " (CD_INDEX, CD_EMAIL_DESTINATARIO, TP_DESTINATARIO, NM_DESTINATARIO, TX_EMAIL) " +
                    " values ( @v1, @v2, @v3, @v4, @v5);",conn);

                Cmd.Parameters.AddWithValue("@v1", p.CD_INDEX);
                Cmd.Parameters.AddWithValue("@v2", p.CD_EMAIL_DESTINATARIO);
                Cmd.Parameters.AddWithValue("@v3", p.TP_DESTINATARIO);
                Cmd.Parameters.AddWithValue("@v4", p.NM_DESTINATARIO);
                Cmd.Parameters.AddWithValue("@v5", p.TX_EMAIL);

                Cmd.ExecuteReader();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Destinatarios: " + ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        private Boolean EmailsGravaLista()
        {
            try
            {
                string NomeEmail = "";
                string SenhaEmail = "";

                Config.ConfigData config = Config.GetConfigData();
                SqlConnection conn = new SqlConnection(config.ConnectString);

                SqlCommand cmd = new SqlCommand("Select * from HABIL_EMAIL_CRIADO WITH (NOLOCK) WHERE CD_SITUACAO = 113 ", conn);
                listMails = new List<clsHabilEmailCriado>();
                listDestinatarios = new List<clsHabilEmailDestinatario>();

                conn.Open();
                SqlDataReader drSQL = null;
                drSQL = cmd.ExecuteReader();

                if (!drSQL.HasRows)
                    return false;

                while (drSQL.Read())
                {
                    CadlistMails = new clsHabilEmailCriado();
                    CadlistMails.CD_INDEX = Convert.ToInt64(drSQL["CD_INDEX"]);
                    CadlistMails.DT_LANCAMENTO = Convert.ToDateTime(drSQL["DT_LANCAMENTO"]);
                    CadlistMails.CD_USU_REMETENTE = Convert.ToInt64(drSQL["CD_USU_REMETENTE"]);
                    CadlistMails.TX_CORPO = drSQL["TX_CORPO"].ToString();
                    CadlistMails.CD_SITUACAO = Convert.ToInt64(drSQL["CD_SITUACAO"]);

                    if (drSQL["TX_ERRO"] != DBNull.Value)
                        CadlistMails.TX_ERRO = drSQL["TX_ERRO"].ToString();

                    if (drSQL["NR_TENTA_ENVIO"] != DBNull.Value)
                        CadlistMails.NR_TENTA_ENVIO = Convert.ToInt32(drSQL["NR_TENTA_ENVIO"]);

                    CadlistMails.IN_HTML = Convert.ToInt32(drSQL["IN_HTML"]);
                    CadlistMails.TX_ASSUNTO = drSQL["TX_ASSUNTO"].ToString();

                    if (CadlistMails.CD_USU_REMETENTE != 0)
                    {
                        NomeEmail = "";
                        SenhaEmail = "";
                        MONTAEMAIL(CadlistMails.CD_USU_REMETENTE, out NomeEmail, out  SenhaEmail);

                        CadlistMails.EMAIL_REMETENTE = NomeEmail;
                        CadlistMails.SENHA_REMETENTE = SenhaEmail;
                    }

                    listMails.Add(CadlistMails);

                    LerEmailsGravaListaDestinatarios(CadlistMails.CD_INDEX);
                    LerEmailsGravaListaAnexos(CadlistMails.CD_INDEX);
                }
                if (drSQL != null)
                    drSQL.Close();
                if (conn != null)
                    conn.Close();

                /**********************************************************************************************************************************************/

                return true;
            }
            catch (Exception ex)
            {
                string x = ex.Message.ToString(); 
                return false;
            }


        }
        private Boolean LerEmailsGravaListaDestinatarios(long lngCD_INDEX)
        {
            try
            {
                Config.ConfigData config = Config.GetConfigData();
                SqlConnection conn = new SqlConnection(config.ConnectString);

                SqlCommand cmd = new SqlCommand("Select * from HABIL_EMAIL_DESTINATARIO WITH (NOLOCK) WHERE CD_INDEX = " + lngCD_INDEX.ToString(), conn);
                conn.Open();
                SqlDataReader drSQL = null;
                drSQL = cmd.ExecuteReader();
                while (drSQL.Read())
                {
                    CadlistDestinatarios = new clsHabilEmailDestinatario();
                    CadlistDestinatarios.CD_INDEX = Convert.ToInt64(drSQL["CD_INDEX"]);
                    CadlistDestinatarios.CD_EMAIL_DESTINATARIO = Convert.ToInt32(drSQL["CD_EMAIL_DESTINATARIO"]);
                    CadlistDestinatarios.TP_DESTINATARIO = Convert.ToInt32(drSQL["TP_DESTINATARIO"]);
                    CadlistDestinatarios.NM_DESTINATARIO = drSQL["NM_DESTINATARIO"].ToString();
                    CadlistDestinatarios.TX_EMAIL = drSQL["TX_EMAIL"].ToString();
                    CadlistDestinatarios.IN_EMAIL_VALIDADO = ValidarEmail(CadlistDestinatarios.TX_EMAIL);

                    listDestinatarios.Add(CadlistDestinatarios);
                }

                if (drSQL != null)
                    drSQL.Close();
                if (conn != null)
                    conn.Close();
                /**********************************************************************************************************************************************/

                return true;
            }
            catch (Exception ex)
            {
                strRetornoDoErro = ex.Message.ToString();
                return false;
            }


        }

        private Boolean LerEmailsGravaListaAnexos(long lngCD_INDEX)
        {
            try
            {
                Config.ConfigData config = Config.GetConfigData();
                SqlConnection conn = new SqlConnection(config.ConnectString);

                SqlCommand cmd = new SqlCommand("Select * from HABIL_EMAIL_ANEXO WITH (NOLOCK) WHERE CD_INDEX = " + lngCD_INDEX.ToString(), conn);
                conn.Open();
                SqlDataReader drSQL = null;
                drSQL = cmd.ExecuteReader();
                while (drSQL.Read())
                {
                    CadlistAnexos = new clsHabilEmailAnexo();
                    CadlistAnexos.CD_INDEX = Convert.ToInt64(drSQL["CD_INDEX"]);
                    CadlistAnexos.CD_ANEXO = Convert.ToInt32(drSQL["CD_ANEXO"]);
                    CadlistAnexos.TX_CONTEUDO = (byte[])(drSQL["TX_CONTEUDO"]);
                    CadlistAnexos.DS_ARQUIVO = drSQL["DS_ARQUIVO"].ToString();

                    listAnexos.Add(CadlistAnexos);
                }

                if (drSQL != null)
                    drSQL.Close();
                if (conn != null)
                    conn.Close();
                /**********************************************************************************************************************************************/

                return true;
            }
            catch (Exception ex)
            {

                strRetornoDoErro = ex.Message.ToString();
                return false;
            }


        }
        private void AtualizaSituacaoEmail(long CD_INDEX, Boolean bolRetorno)
        {
            Config.ConfigData config = Config.GetConfigData();
            SqlConnection conn = new SqlConnection(config.ConnectString);

            string strSQL = "";

            if (bolRetorno)
                strSQL = "UPDATE HABIL_EMAIL_CRIADO SET NR_TENTA_ENVIO = ISNULL(NR_TENTA_ENVIO,0) + 1,  CD_SITUACAO = 112, DT_ENVIO = GETDATE(), TX_ERRO = 'SUCESSO: " + strRetornoDoErro + "' WHERE CD_INDEX = @CD_INDEX";
            else
                strSQL = "UPDATE HABIL_EMAIL_CRIADO SET NR_TENTA_ENVIO = ISNULL(NR_TENTA_ENVIO,0) + 1, CD_SITUACAO = 111, DT_ENVIO = GETDATE(), TX_ERRO = 'ERRO: " + strRetornoDoErro.Replace("'","´")  + "' WHERE CD_INDEX = @CD_INDEX";

            SqlCommand cmd = new SqlCommand(strSQL, conn);

            cmd.Parameters.Add(new SqlParameter("@CD_INDEX", CD_INDEX));
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        private void PrepararEmailParaEnvio()
        {
            try
            {

                foreach (var item in listMails)
                {
                    strRetornoDoErro = "";

                    listMailsPreparadoParaEnvio.Clear();
                    listDestinatariosPreparadoParaEnvio.Clear();
                    listAnexosPreparadoParaEnvio.Clear();
                    /***************************************************************/
                    listMailsPreparadoParaEnvio.Add(item);

                    foreach (var item2 in listDestinatarios)
                    {
                        if (item2.CD_INDEX == item.CD_INDEX)
                            listDestinatariosPreparadoParaEnvio.Add(item2);
                    }

                    foreach (var item3 in listAnexos)
                    {
                        if (item3.CD_INDEX == item.CD_INDEX)
                            listAnexosPreparadoParaEnvio .Add(item3);
                    }
                    /***************************************************************/

                    AtualizaSituacaoEmail(item.CD_INDEX, EnviandoEmail(listMailsPreparadoParaEnvio, listDestinatariosPreparadoParaEnvio, listAnexosPreparadoParaEnvio));
                }

            }
            catch (Exception ex)
            {
                string x = ex.Message.ToString();
            }
        }
        private Boolean EnviandoEmail(List<clsHabilEmailCriado> lstMail, List<clsHabilEmailDestinatario> lstDest, List<clsHabilEmailAnexo> lstAnexo)
        {
            try
            {
                Config.ConfigData config = Config.GetConfigData();

                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = config.ServidorPop;
                SmtpServer.Port = config.Porta;
                SmtpServer.UseDefaultCredentials = true;

                MailMessage mail = new MailMessage();

                foreach (var item in lstMail)
                {
                    if ((item.CD_USU_REMETENTE == 0) || (item.EMAIL_REMETENTE == "") || (item.SENHA_REMETENTE == ""))
                    { 
                        mail.From = new MailAddress(config.Usuario, config.NomeUsuario);
                        mail.To.Add(new MailAddress(config.Usuario, config.NomeUsuario));
                    }
                    else
                    {
                        mail.From = new MailAddress(item.EMAIL_REMETENTE, item.EMAIL_REMETENTE);
                        mail.To.Add(new MailAddress(item.EMAIL_REMETENTE, item.EMAIL_REMETENTE));
                    }
                    
                    foreach (var item2 in lstDest)
                    {
                        if (@item2.TX_EMAIL.Trim() != "")
                        {
                            //Significa 1 é To = Para
                            if (item2.TP_DESTINATARIO == 1)
                                mail.To.Add(new MailAddress(@item2.TX_EMAIL.Trim(), @item2.NM_DESTINATARIO.Trim()));

                            //Significa 2 é CC = Cópia Carbono
                            if (item2.TP_DESTINATARIO == 2)
                                mail.CC.Add(new MailAddress(@item2.TX_EMAIL.Trim(), @item2.NM_DESTINATARIO.Trim()));

                            //Significa 3 é BCC Cópia Carbono (famoso copia oculta)
                            if (item2.TP_DESTINATARIO == 3)
                                mail.Bcc.Add(new MailAddress(@item2.TX_EMAIL.Trim(), @item2.NM_DESTINATARIO.Trim()));

                            //Significa 4 é Reply
                            if (item2.TP_DESTINATARIO == 4)
                                mail.ReplyToList.Add(new MailAddress(@item2.TX_EMAIL.Trim(), @item2.NM_DESTINATARIO.Trim()));
                        }
                    }

                    mail.Subject = item.TX_ASSUNTO;

                    // este pega do config se um dia precisar
                    //mail.IsBodyHtml = config.CorpoEhHTML;

                    mail.IsBodyHtml = item.IN_HTML==1;
                    mail.Body = item.TX_CORPO.ToString();

                    mail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
                    mail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

                    foreach (var item3 in lstAnexo)
                    {
                        string CaminhoArquivoLog = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Anexo\\";

                        if (!Directory.Exists(CaminhoArquivoLog))
                            Directory.CreateDirectory(CaminhoArquivoLog);
        
                        CaminhoArquivoLog = CaminhoArquivoLog + item3.DS_ARQUIVO;
                        if (File.Exists(CaminhoArquivoLog))
                            File.Delete(CaminhoArquivoLog);

                        byte[] strConteudo = item3.TX_CONTEUDO;
                        FileStream file = new FileStream(CaminhoArquivoLog, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(file);
                        bw.Write(strConteudo);
                        bw.Close();

                        file = new FileStream(CaminhoArquivoLog, FileMode.Open);
                        BinaryReader br = new BinaryReader(file);
                        file.Close();

                        Attachment anexar = new Attachment(CaminhoArquivoLog);
                        mail.Attachments.Add(anexar);

                    }

                    if ((item.CD_USU_REMETENTE == 0) || (item.EMAIL_REMETENTE == "") || (item.SENHA_REMETENTE == "")) 
                        SmtpServer.Credentials = new System.Net.NetworkCredential(config.Usuario, config.Senha);
                    else
                        SmtpServer.Credentials = new System.Net.NetworkCredential(item.EMAIL_REMETENTE, item.SENHA_REMETENTE);

                } // foreach Pai




                SmtpServer.EnableSsl = config.UsaSSL;


                //Add this line to bypass the certificate validation
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };


                SmtpServer.Send(mail);
                mail.Dispose();
                SmtpServer.Dispose();
                

                foreach (var item3 in lstAnexo)
                {
                    string CaminhoArquivoLog = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Anexo\\";

                    if (!Directory.Exists(CaminhoArquivoLog))
                        Directory.CreateDirectory(CaminhoArquivoLog);

                    CaminhoArquivoLog = CaminhoArquivoLog + item3.DS_ARQUIVO;
                    if (File.Exists(CaminhoArquivoLog))
                        File.Delete(CaminhoArquivoLog);
                }


                strRetornoDoErro = "Email enviado com Sucesso.";
                return true;
            }

            catch (Exception ex)
            {
                strRetornoDoErro = ex.Message.ToString();
                return false;
            }
        }
        private void MONTAEMAIL ( long lngCod, out string Nome,  out string Senha)
        {
            Nome = "";
            Senha = "";
            try
            {
                Config.ConfigData config = Config.GetConfigData();
                SqlConnection conn = new SqlConnection(config.ConnectString);

                SqlCommand cmd = new SqlCommand("Select PC.TX_MAIL_SENHA, PC.TX_MAIL1 from USUARIO AS U WITH (NOLOCK) INNER JOIN [PESSOA_CONTATO] AS PC ON PC.CD_PESSOA = U.CD_PESSOA WHERE U.CD_USUARIO = " + lngCod.ToString(), conn);
                conn.Open();
                SqlDataReader drSQL = null;
                drSQL = cmd.ExecuteReader();
                if  (drSQL.Read())
                {
                    Nome = drSQL["TX_MAIL1"].ToString();
                    Senha = drSQL["TX_MAIL_SENHA"].ToString();
                }
                if (drSQL != null)
                    drSQL.Close();
                if (conn != null)
                    conn.Close();
                /**********************************************************************************************************************************************/

            }
            catch (Exception ex)
            {
                strRetornoDoErro = ex.Message.ToString();
            }



        }
    }
}
