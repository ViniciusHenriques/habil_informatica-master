using System;
using System.Web;
using System.Web.UI;
using DAL.Persistence;
using DAL.Model;
using System.IO;
using System.Configuration;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Data;

namespace HabilInformatica
{

    public partial class Default : System.Web.UI.Page
    {

        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        clsMensagem cls = new clsMensagem();
        string strS = "";
        string strD = "";
        string strU = "";
        string strP = "";

        public string ObterNomeComputador()
        {
            try
            {
                
                string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                String ecn = System.Environment.MachineName;
                return computer_name[0].ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ObterIPComputador()
        {
            try
            {
                string Ip = "";
                System.Net.IPHostEntry v_ipMaquina = System.Net.Dns.Resolve(ObterNomeComputador());
                System.Net.IPAddress[] address = v_ipMaquina.AddressList;
                for (int i = 0; i < address.Length; i++) //ciclo q escreve o ip
                {
                    if (i == 0)
                        Ip = (Ip + address[i]);
                    else
                        Ip = Ip + " - " + address[i];
                }
                return Ip;

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        public void ExtrairDadosCnn(out string A, out string B, out string C, out string D)
        {


            string strCnn = ConfigurationManager.ConnectionStrings["HabilInfoCS"].ConnectionString;
            string strCnn2 = strCnn;

            strCnn2 = strCnn2.Split(';')[0];
            strCnn2 = strCnn2.Split('=')[1];
            A = strCnn2;

            strCnn2 = strCnn;
            strCnn2 = strCnn2.Split(';')[1];
            strCnn2 = strCnn2.Split('=')[1];
            B = strCnn2;

            strCnn2 = strCnn;
            strCnn2 = strCnn2.Split(';')[2];
            strCnn2 = strCnn2.Split('=')[1];
            C = strCnn2;

            strCnn2 = strCnn;
            strCnn2 = strCnn2.Split(';')[3];
            strCnn2 = strCnn2.Split('=')[1];
            D = strCnn2;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["DataHoraVerificaPendencias"] = null;
                Session["TabFocada"] = null;
                Session["Estacao_Logada"] = null;
                Session["IP_Logado"] = null;
                Session["UsuSis"] = null;
                Session["CodEmpresa"] = null;
                Session["QtasEmpresas"] = null;
                Session["NomeEmpresa"] = null;
                Session["CodPessoaLogada"] = null;

                if (Session["MensagemTela"] != null)
                { 
                    if (Session["MensagemTela"].ToString() != "")
                    {
                        ShowMessage(Session["MensagemTela"].ToString(), MessageType.Error);
                        Session["MensagemTela"] = null;
                    }
                }

                if ( Request.QueryString["Guid"] == "123456789")
                {
                    Session["GuidRecebido"] = "Guid=" + "123456789";
                }

                txtUsuario1.Attributes.Add("onKeyPress", "javascript:passaCampo(event, pwdSenha1);");
                pwdSenha1.Attributes.Add("onKeyPress", "javascript:passaCampo(event, btnEntrar);");
                Session["Estacao_Logada"] = ObterNomeComputador();
                Session["IP_Logado"] = ObterIPComputador();

                lblMaquinaIP.Text = "Máquina: " + Session["Estacao_Logada"].ToString() + "  - IP: " + Session["IP_Logado"].ToString();

                DBTabelaDAL t = new DBTabelaDAL();
                ExtrairDadosCnn(out strS, out strD, out strU,  out strP);
                lblInfoLicenca.Text = "";
                if (t.PesquisaBancoExiste (strD, strS , strU, strP ) == "" )
                {
                    btnEntrar.Visible = false;
                    btnLicenca.Visible = false;
                    BtnClienteNovo.Visible = true;
                    lblInfoLicenca.Text += "<div style='text-align:center'><b>Cliente não licenciado!<br/>" +
                        "Entre em contato para mais informações<br/>" +
                        "Fone: (51)3051-4334<br/>" +
                        "Email: contato@habilinformatica.com.br</b></div>";
                }
                else
                {
                    btnEntrar.Visible = true;
                    btnLicenca.Visible = false;
                    BtnClienteNovo.Visible = false;

                    Licenca l = new Licenca();
                    LicencaDAL lDAL = new LicencaDAL();
                    l = lDAL.PesquisarLicenca(0);
                    lblInfoLicenca.Text = "<b>Cliente: " + l.NomeDoCliente + "<br/></b>";

                    ItemDaLicenca item = new ItemDaLicenca();
                    item = lDAL.PesquisarUltimoItemLicenca(l.CodigoDaLicenca);
                    lblInfoLicenca.Text += "<b>Licença válida até  " + item.DataDeValidade.ToString("dd/MM/yyyy")+ "<br/></b>";
                    lblInfoLicenca.Text += "<b>Módulos disponíveis:</b>";
                    DataTable dt = new DataTable();
                    ModuloSistemaDAL mDAL = new ModuloSistemaDAL();
                    dt = mDAL.ObterModulosSistema("", "", "", "");

                    lblInfoLicenca.Text += "<ul style='font-size:12px'>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lblInfoLicenca.Text += "<li>" + dt.Rows[i][1].ToString().ToUpper() + "</li>";
                    }
                    lblInfoLicenca.Text += "</ul>";
                    lblInfoLicenca.Text += "Para adquirir outros módulos, entre em contato no:<br/> <b>Fone: (51)3051-4334<br/>Email: contato@habilinformatica.com.br</b>";
                }



                if (!Page.IsPostBack)
               {
                   
                   txtUsuario1.Text = "";
                   pwdSenha1.Text = "";
                   txtUsuario1.Focus();
                   HttpCookie myCookie = Request.Cookies["UserSistema"];
               }
               
           }
           catch (Exception)
           {
               Response.Redirect("~/SistemaEmManut.aspx");
           }



        }
        protected void ShowMessageBox(string Mensagem)
        {
            String csname1 = "PopupScript";
            Type cstype = this.GetType();

            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "Location.reload();";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

        }
        protected void btnEntrar_Click(object sender, EventArgs e)
        {

            Usuario p = new Usuario();

            if (txtUsuario1.Text == "")
            {
                ShowMessage("Usuário deve ser Informado.",MessageType.Info);
                txtUsuario1.Focus();
                return;
            }
            if (pwdSenha1.Text == "")
            {
                ShowMessage("Senha deve ser Informado.", MessageType.Info);
                pwdSenha1.Focus();
                return;
            }
            UsuarioDAL m = new UsuarioDAL();
            if(m.UsuarioMaster(txtUsuario1.Text, pwdSenha1.Text))
            {

                Session["CodPessoaLogada"] = null;
                Session["UsuSis"] = "USUÁRIO MASTER DE INICIALIZAÇÃO";
                Session["CodUsuario"] = "-150380";
                Session["CodPflUsuario"] = 1;
                Session["CodModulo"] = 0;
                Session["DesModulo"] = "ESCOLHER";
                Response.Redirect("~/Selecao.aspx");
                return;
            }

            if (Session["GuidRecebido"] != null)
            {
                if (Session["GuidRecebido"].ToString() != "")
                {

                    if ((txtUsuario1.Text == "fornecedor") && (pwdSenha1.Text == "a"))
                    {
                        DBTabelaDAL RnTab = new DBTabelaDAL();
                        DateTime DataHoraAtual = RnTab.ObterDataHoraServidor();
                        Session["DataHoraVerificaPendencias"] = DataHoraAtual;

                        Session["CodModulo"] = 15;
                        Session["DesModulo"] = "Cotação de Preços para Fornecedores";

                        p.NomeUsuario ="FORNECEDOR";
                        p.CodigoUsuario = 15;
                        p.CodigoPerfil = 1;
                        p.CodigoPessoa = 11;

                        Session["UsuSis"] = p.NomeUsuario;
                        Session["CodUsuario"] = p.CodigoUsuario;
                        Session["CodPflUsuario"] = p.CodigoPerfil;
                        Session["CodPessoaLogada"] = p.CodigoPessoa;
                        HttpCookie myCookie = new HttpCookie("UserSistema");
                        myCookie["ModuloSetado"] = "15";

                        Habil_Estacao he = new Habil_Estacao();
                        Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

                        he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
                        if (he == null)
                        {
                            he = new Habil_Estacao();
                            he.NomeEstacao = Session["Estacao_Logada"].ToString();
                            he.IPEstacao = Session["IP_Logado"].ToString();
                            hedal.Inserir(he);
                        }

                        Habil_Log hl = new Habil_Log();
                        Habil_LogDAL hldal = new Habil_LogDAL();

                        hl = new Habil_Log();
                        hl.CodigoTabela = 0;
                        hl.CodigoCampo = 0;
                        hl.CodigoOperacao = 1;
                        hl.CodigoUsuario = p.CodigoUsuario;
                        hl.CodigoEstacao = he.CodigoEstacao;
                        hl.DescricaoLog = "Inicialização do Sistema - Cotação de Preços Fornecedor";

                        hldal.Inserir(hl);

                        VerificaAutorizacaoAcesso();
                        Response.Redirect("~/Pages/WelcomeFornecedor.aspx");

                        return;
                    }

                }
            }

            lblMensagem.Text = "";
            DateTime x;
            string y;
            Boolean blnLoginOK = false;
            p = m.PesquisarLogin(txtUsuario1.Text, pwdSenha1.Text, out blnLoginOK);

            if (p.ResetarSenha == pwdSenha1.Text)
            {
                pnlInicial.Visible = false;
                pnlLicenca.Visible = false;
                pnlCliNovo.Visible = false;
                pnlSenha.Visible = true;



                return;
            }

            if (blnLoginOK == true)
            {
                LicencaDAL l = new LicencaDAL();

                if (!l.LicencaEhValida(out y, out x))
                {
                    ShowMessage("Verificar Licença do Sistema pode estar vencida.", MessageType.Info);
                    btnLicenca_Click(null, null);  
                    return;
                }

                else
                {
                    DBTabelaDAL RnTab = new DBTabelaDAL();
                    DateTime DataHoraAtual = RnTab.ObterDataHoraServidor();
                    Session["DataHoraVerificaPendencias"] = DataHoraAtual;
                    Session["UsuSis"] = p.NomeUsuario;
                    Session["CodUsuario"] = p.CodigoUsuario;
                    Session["CodPflUsuario"] = p.CodigoPerfil;
                    Session["CodModulo"] = 0;
                    Session["DesModulo"] = "ESCOLHER";
                    Session["CodPessoaLogada"] = p.CodigoPessoa ;
                    HttpCookie myCookie = new HttpCookie("UserSistema");
                    myCookie["ModuloSetado"] = "0";
                    myCookie.Expires = DateTime.Now.AddDays(1d);
                    Response.Cookies.Add(myCookie);

                    Habil_Estacao he = new Habil_Estacao();
                    Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

                    he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
                    if (he == null)
                    {
                        he = new Habil_Estacao();
                        he.NomeEstacao = Session["Estacao_Logada"].ToString();
                        he.IPEstacao = Session["IP_Logado"].ToString();
                        hedal.Inserir(he);
                    }

                    Habil_Log hl = new Habil_Log();
                    Habil_LogDAL hldal = new Habil_LogDAL();

                    hl = new Habil_Log();
                    hl.CodigoTabela = 0;
                    hl.CodigoCampo = 0;
                    hl.CodigoOperacao = 1;
                    hl.CodigoUsuario = p.CodigoUsuario;
                    hl.CodigoEstacao = he.CodigoEstacao;
                    hl.DescricaoLog = "Inicialização do Sistema";

                    hldal.Inserir(hl);

                    VerificaAutorizacaoAcesso();
                    Response.Redirect("~/Selecao.aspx");
                }
            }
            else
            {
                LicencaDAL l = new LicencaDAL();

                if (!l.LicencaEhValida(out y, out x))
                {
                    ShowMessage("Verificar Licença do Sistema pode estar vencida.", MessageType.Info);
                    btnLicenca_Click(null, null);
                    return;
                }
                else
                {
                    ShowMessage("Usuário e/ou Senha não autorizados.",MessageType.Info);
                    txtUsuario1.Focus();
                }
            }

        }

        protected void ddlPesquisa_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUsuario1.Focus();
        }

        protected void button1_Click(object sender, EventArgs e)
        {

        }

        protected void btnNada_Click(object sender, EventArgs e)
        {

        }

        protected void btnLicenca_Click(object sender, EventArgs e)
        {
            pnlSenha.Visible = false;
            pnlInicial.Visible = false;
            pnlLicenca.Visible = true;
            DateTime x;
            string y;
            LicencaDAL l = new LicencaDAL();

            l.LicencaEhValida(out y, out x);

            txtCliente.Text = y;
            txtData.Text = x.ToString("dd/MM/yyyy");

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            pnlInicial.Visible = true;
            pnlLicenca.Visible = false;
            pnlCliNovo.Visible = false;
            pnlSenha.Visible = false;
        }

        protected void btnSlcnarArquivo_Click(object sender, EventArgs e)
        {
        }

        protected void btnInserirLicenca_Click(object sender, EventArgs e)

        {
            DBTabelaDAL t = new DBTabelaDAL();
            string strLinha2;


            //Verifica se tem alguma coisa postada 
            if (this.FileUploader.PostedFile.ContentLength != 0 && this.FileUploader.HasFile)
            {
                //capturando nome original do arquivo
                string fileName = this.FileUploader.FileName;

                //capturando extensão do arquivo postado
                string extension = System.IO.Path.GetExtension(fileName);

                //verificando se o arquivo escolhido é do tipo TXT
                if (!extension.Equals(".sql", StringComparison.OrdinalIgnoreCase))
                {
                    //Response.Output.WriteLine("<br />Selecione um arquivo do tipo .SQL<br />");
                    Label1.Text = "Selecione um arquivo do tipo .SQL";
                }
                else
                {
                    Label1.Text = "Instalando Nova Licença!!!";

                    //Definindo o caminho do arquivo para ser salvo no servidor
                    string vCamArq = ConfigurationManager.AppSettings["CaminhoDaLicenca"].ToString() + fileName;

                    //cria bat

                    string nomeArquivo = @"executabat.bat";

                    // Cria um novo arquivo e devolve um StreamWriter para ele

                    StreamWriter writer = new StreamWriter(ConfigurationManager.AppSettings["CaminhoDaLicenca"].ToString() + @"\" + nomeArquivo);

                    // Agora é só sair escrevendo

                    ExtrairDadosCnn(out strS, out strD, out strU, out strP);

                    strLinha2 = "sqlcmd -S " + @strS.Replace("\\", @"\") + " -d " + strD + " -U " + strU + " -P " + strP + " -i \"" + ConfigurationManager.AppSettings["CaminhoDaLicenca"].ToString() + fileName + "\" -o \"" + ConfigurationManager.AppSettings["CaminhoDaLicenca"].ToString() + "BackupLogVer.txt" + '\u0022';
                    strLinha2 = strLinha2.Replace(@"\\", @"\");
                    strLinha2 = strLinha2 + " -f 65001";
                    writer.WriteLine(strLinha2);
                    writer.WriteLine("exit");
                    writer.Close();

                    strLinha2 = ConfigurationManager.AppSettings["CaminhoDaLicenca"].ToString() + "executabat.bat";
                    strLinha2 = strLinha2.Replace(@"\\", @"\");
                    System.Diagnostics.Process pgm = new System.Diagnostics.Process();
                    pgm.StartInfo.FileName = @strLinha2;
                    pgm.Start();
                    pgm.WaitForExit();


                    //////Definindo o caminho do arquivo para ser salvo no servidor
                    ////string vCamArq = ConfigurationManager.AppSettings["CaminhoDaLicenca"].ToString() + fileName;

                    ////////Salvando o arquivo com o nome original
                    ////this.FileUploader.PostedFile.SaveAs(vCamArq);

                    //////Cria um novo arquivo e passa para o objeto StreamWriter
                    //////  .GetEncoding(1252)
                    ////StreamReader Leitura = new StreamReader(vCamArq, System.Text.Encoding.UTF8);
                    //////variavel para receber as linhas
                    ////string strLinha;
                    ////strLinha2 = "";
                    //////loop de leitura, linha por linha
                    ////while (Leitura.Peek() != -1)
                    ////{
                    ////    //lendo a linha atual
                    ////    strLinha = Leitura.ReadLine();
                    ////    //verificando se a linha esta vazia
                    ////    if (strLinha.Trim().Length > 0)
                    ////    {
                    ////        //print da linha
                    ////        //      Response.Output.Write("<br />" + strLinha);

                    ////        if (strLinha.Trim() == "GO") 
                    ////        {
                    ////            strLinha2 = strLinha2.Replace("[HABIL_SEU_BANCO]", "[" + txtBanco.Text + "]");

                    ////            t.ExecutaComandoSQL(strLinha2);
                    ////            strLinha2 = "";
                    ////        }
                    ////        else
                    ////        {
                    ////            if ((strLinha.Substring(strLinha.Length - 1, 1) == ";"))
                    ////            {
                    ////                strLinha2 = strLinha2 + "\r\n" + strLinha;
                    ////                t.ExecutaComandoSQL(strLinha2);
                    ////                strLinha2 = "";
                    ////            }
                    ////            else
                    ////                strLinha2 = strLinha2 + "\r\n" + strLinha;

                    ////        }
                    ////    }
                    ////}
                    //////fechando o arquivo
                    ////Leitura.Close();

                    Label1.Text = "Licença Instalada. Clique em Voltar e tente seu Acesso!!!";

                }
            }
            else
            {
                Label1.Text = "Selecione um arquivo para enviar.!!!";
                // Response.Output.WriteLine("<br />Selecione um arquivo para enviar.<br />");
            }
        }

        protected void BtnClienteNovo_Click(object sender, EventArgs e)
        {
            pnlInicial.Visible = false;
            pnlLicenca.Visible = false;
            pnlCliNovo.Visible = true;
            pnlSenha.Visible = false;

        }

        protected void blnImplantarBanco_Click(object sender, EventArgs e)
        {
            string strLinha2;
            DBTabelaDAL t = new DBTabelaDAL();

            //Verifica se tem alguma coisa postada 
            if (this.FileUpload1.PostedFile.ContentLength != 0 && this.FileUpload1.HasFile)
            {
                //capturando nome original do arquivo
                string fileName = this.FileUpload1.FileName;

                //capturando extensão do arquivo postado
                string extension = System.IO.Path.GetExtension(fileName);

                //verificando se o arquivo escolhido é do tipo TXT
                if (!extension.Equals(".sql", StringComparison.OrdinalIgnoreCase))
                {
                    //Response.Output.WriteLine("<br />Selecione um arquivo do tipo .SQL<br />");
                    Label2.Text = "Selecione um arquivo do tipo .SQL";
                }
                else
                {

                    ExtrairDadosCnn(out strS, out strD, out strU, out strP);

                    Label2.Text = "Banco " + strD + " Iniciando Geração.";


                    t.CriaBancoInicial(strS, strD, strU, strP);
                    strLinha2 = "";
                    Label2.Text = "Banco " + strD + " Gerado.";

                    //Definindo o caminho do arquivo para ser salvo no servidor
                    string vCamArq = ConfigurationManager.AppSettings["CaminhoDaEstrutura"].ToString() + fileName;

                    //cria bat

                    string nomeArquivo = @"executabat.bat";

                    // Cria um novo arquivo e devolve um StreamWriter para ele



                    StreamWriter writer = new StreamWriter(ConfigurationManager.AppSettings["CaminhoDaEstrutura"].ToString() + @"\" + nomeArquivo);

                    // Agora é só sair escrevendo

                    strLinha2 = "sqlcmd -S " + @strS.Replace("\\", @"\") + " -d " + strD + " -U " + strU + " -P " + strP + " -i \"" + ConfigurationManager.AppSettings["CaminhoDaEstrutura"].ToString() + fileName + "\" -o \"" + ConfigurationManager.AppSettings["CaminhoDaEstrutura"].ToString() + "BackupLog.txt" + '\u0022';
                    strLinha2 = strLinha2.Replace(@"\\", @"\");
                    strLinha2 = strLinha2 + " -f 65001";
                    writer.WriteLine(strLinha2);
                    writer.WriteLine("exit");
                    writer.Close();

                    strLinha2 = ConfigurationManager.AppSettings["CaminhoDaEstrutura"].ToString() + "executabat.bat";
                    strLinha2 = strLinha2.Replace(@"\\",  @"\");
                    System.Diagnostics.Process pgm = new System.Diagnostics.Process();
                    pgm.StartInfo.FileName = @strLinha2;
                    pgm.Start();
                    pgm.WaitForExit();

                    ////Cria um novo arquivo e passa para o objeto StreamWriter
                    ////  .GetEncoding(1252)
                    //StreamReader Leitura = new StreamReader(vCamArq, System.Text.Encoding.UTF8);
                    ////variavel para receber as linhas
                    //string strLinha;
                    //strLinha2 = "";
                    ////loop de leitura, linha por linha
                    //while (Leitura.Peek() != -1)
                    //{
                    //    //lendo a linha atual
                    //    strLinha = Leitura.ReadLine();
                    //    //verificando se a linha esta vazia
                    //    if (strLinha.Trim().Length > 0)
                    //    {
                    //        //print da linha
                    //        //      Response.Output.Write("<br />" + strLinha);


                    //        if (strLinha.Trim() == "GO")
                    //        {

                    //            strLinha2 = strLinha2.Replace("[HABIL_SEU_BANCO]", "[" + txtBanco.Text + "]");

                    //            t.ExecutaComandoSQL(strLinha2);
                    //            strLinha2 = "";
                    //        }
                    //        else
                    //        {
                    //            if ((strLinha.Substring(strLinha.Length - 1, 1) == ";"))
                    //            {
                    //                strLinha2 = strLinha2 + "\r\n" + strLinha;
                    //                t.ExecutaComandoSQL(strLinha2);
                    //                strLinha2 = "";
                    //            }
                    //            else
                    //                strLinha2 = strLinha2 + "\r\n" + strLinha;

                    //        }

                    //    }
                    //}
                    ////fechando o arquivo
                    //Leitura.Close();

                    Label2.Text = "Banco Gerado. Clique em Voltar e tente seu Acesso!!!";
                    btnVoltar_Click(null, null);
                    btnLicenca_Click(null, null);
                    return;

                }
            }
            else
            {
                Label2.Text = "Selecione um arquivo para enviar.!!!";
                // Response.Output.WriteLine("<br />Selecione um arquivo para enviar.<br />");
            }

        }

        protected void btnCfmSenhaMesmo_Click(object sender, EventArgs e)
        {
            Label5.Text = "";
            if ((txtNovaSenha.Text != "") && (txtConfirmaSenha.Text != ""))
            {
                if (txtNovaSenha.Text != txtConfirmaSenha.Text)
                {
                    Label5.Text = "Senhas informadas são diferentes.";
                    txtNovaSenha.Focus();
                }
                else
                {

                    clsHash clsh = new clsHash(SHA512.Create());
                    string teste = clsh.CriptografarSenha(txtConfirmaSenha.Text);


                    //Salva Senha Nova e Volta
                    Usuario p = new Usuario();
                    UsuarioDAL m = new UsuarioDAL();
                    Boolean blnLoginOK = false;
                    p = m.PesquisarLogin(txtUsuario1.Text, pwdSenha1.Text, out blnLoginOK);
                    p.ResetarSenha = "";
                    p.Senha = teste;
                    m.Atualizar(p);
                    btnVoltar_Click(sender,e);

                }

            }

        }
        protected void VerificaAutorizacaoAcesso()
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime DataHoraAtual = RnTab.ObterDataHoraServidor();
            bool Autorizado = false;
            PerfilUsuario Perfil = new PerfilUsuario();
            PerfilUsuarioDAL PerfilDAL = new PerfilUsuarioDAL();
            Perfil = PerfilDAL.PesquisarPerfilUsuario(Convert.ToInt64(Session["CodPflUsuario"]));
            if (Perfil != null)
            {
                List<String> listaDiaSemanaAutorizado = new List<String>();
                if (Perfil.Domingo)
                    listaDiaSemanaAutorizado.Add("Sunday");
                if (Perfil.Segunda)
                    listaDiaSemanaAutorizado.Add("Monday");
                if (Perfil.Terca)
                    listaDiaSemanaAutorizado.Add("Tuesday");
                if (Perfil.Quarta)
                    listaDiaSemanaAutorizado.Add("Wednesday");
                if (Perfil.Quinta)
                    listaDiaSemanaAutorizado.Add("Thursday");
                if (Perfil.Sexta)
                    listaDiaSemanaAutorizado.Add("Friday");
                if (Perfil.Sabado)
                    listaDiaSemanaAutorizado.Add("Saturday");

                foreach (string DiaSemanaAutorizado in listaDiaSemanaAutorizado)
                {
                    if (DataHoraAtual.DayOfWeek.ToString() == DiaSemanaAutorizado)
                    {
                        if (Perfil.HoraInicial < Perfil.HoraFinal)
                        {
                            if (DataHoraAtual.TimeOfDay >= Perfil.HoraInicial.TimeOfDay && DataHoraAtual.TimeOfDay < Perfil.HoraFinal.TimeOfDay)
                            {
                                Autorizado = true;
                            }
                        }
                        else if (Perfil.HoraInicial > Perfil.HoraFinal)
                        {
                            DateTime DataFinalMaisUm = Perfil.HoraFinal.AddDays(1);
                            DateTime DataHoraAtualMaisUm = DataHoraAtual;
                            if (DataHoraAtual <= Perfil.HoraInicial)
                                DataHoraAtualMaisUm = DataHoraAtual.AddDays(1);
                            if (DataHoraAtualMaisUm >= Perfil.HoraInicial && DataHoraAtualMaisUm <= DataFinalMaisUm)
                            {
                                Autorizado = true;
                            }
                        }
                        if (Autorizado)
                        {
                            if (Perfil.IntervaloInicio1.HasValue)
                            {
                                if (Perfil.IntervaloInicio1 < Perfil.IntervaloFim1)
                                {
                                    if (DataHoraAtual >= Perfil.IntervaloInicio1 && DataHoraAtual < Perfil.IntervaloFim1)
                                    {
                                        Autorizado = false;
                                    }
                                }
                                else if (Perfil.IntervaloInicio1 > Perfil.IntervaloFim1)
                                {
                                    DateTime? IntevaloFimMaisUm = Perfil.IntervaloFim1;
                                    DateTime DataHoraAtualMaisUm = DataHoraAtual;
                                    if (DataHoraAtual <= Perfil.IntervaloFim1)
                                        DataHoraAtualMaisUm = DataHoraAtual.AddDays(1);
                                    if (DataHoraAtualMaisUm >= Perfil.IntervaloInicio1 && DataHoraAtualMaisUm <= IntevaloFimMaisUm)
                                    {
                                        Autorizado = false;
                                    }
                                }
                            }
                            if (Perfil.IntervaloInicio2.HasValue)
                            {
                                if (Perfil.IntervaloInicio2 < Perfil.IntervaloFim2)
                                {
                                    if (DataHoraAtual >= Perfil.IntervaloInicio2 && DataHoraAtual < Perfil.IntervaloFim2)
                                    {
                                        Autorizado = false;
                                    }
                                }
                                else if (Perfil.IntervaloInicio2 > Perfil.IntervaloFim2)
                                {
                                    DateTime? IntevaloFimMaisUm = Perfil.IntervaloFim2;
                                    DateTime DataHoraAtualMaisUm = DataHoraAtual;
                                    if (DataHoraAtual <= Perfil.IntervaloFim2)
                                        DataHoraAtualMaisUm = DataHoraAtual.AddDays(1);
                                    if (DataHoraAtualMaisUm >= Perfil.IntervaloInicio2 && DataHoraAtualMaisUm <= IntevaloFimMaisUm)
                                    {
                                        Autorizado = false;
                                    }
                                }
                            }
                            if (Perfil.IntervaloInicio3.HasValue)
                            {
                                if (Perfil.IntervaloInicio3 < Perfil.IntervaloFim3)
                                {
                                    if (DataHoraAtual >= Perfil.IntervaloInicio3 && DataHoraAtual < Perfil.IntervaloFim3)
                                    {
                                        Autorizado = false;
                                    }
                                }
                                else if (Perfil.IntervaloInicio3 > Perfil.IntervaloFim3)
                                {
                                    DateTime? IntevaloFimMaisUm = Perfil.IntervaloFim3;
                                    DateTime DataHoraAtualMaisUm = DataHoraAtual;
                                    if (DataHoraAtual <= Perfil.IntervaloFim3)
                                        DataHoraAtualMaisUm = DataHoraAtual.AddDays(1);
                                    if (DataHoraAtualMaisUm >= Perfil.IntervaloInicio3 && DataHoraAtualMaisUm <= IntevaloFimMaisUm)
                                    {
                                        Autorizado = false;
                                    }
                                }
                            }
                        }
                    }
                }//fecha foreach
                if (!Autorizado)
                    if (Perfil.DataHoraInicio.HasValue && DataHoraAtual >= Perfil.DataHoraInicio && DataHoraAtual <= Perfil.DataHoraFim)
                    {
                        Autorizado = true;
                    }
                
                if (Autorizado)
                {
                    Session["DataHoraVerificaPendencias"] = DataHoraAtual;
                }
                else
                {
                    Session["MensagemTela"] = "Usuário fora do Período de Acesso ao Sistema.";
                    Response.Redirect("~/Default.aspx");

                }
            }
        }
    }
}