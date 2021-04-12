using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using DAL.Model;
using System.Drawing;
using System.Drawing.Imaging;
using DAL.Persistence;
using System.Configuration;

namespace SoftHabilInformatica.Pages.Pessoas
{
    public partial class CadPessoa_Contato : System.Web.UI.Page
    {
        private int intNroDeRefresh = 0;
        private List<Pessoa> listCadPessoa = new List<Pessoa>();
        private List<Pessoa_Contato> listCadPessoaContato = new List<Pessoa_Contato>();
        private List<Pessoa_Inscricao> listCadPessoaInscricao = new List<Pessoa_Inscricao>();
        private clsValidacao v = new clsValidacao();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void btnCanContato_Click(object sender, EventArgs e)
        {
            Session["CaminhoFotoContato"] = null;
            if (txtCodPessoa.Text == "0")
                Session["TabFocada"] = "parameter";
            Session["CARREGOUCNPJ"] = null;
            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx?cad=" + Request.QueryString["Cad"]);
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
        }
        protected void CarregaDropDownList()
        {
            PaisDAL paisDAL = new PaisDAL();
            ddlPaises.DataSource = paisDAL.ListarPaises("", "", "", "");
            ddlPaises.DataTextField = "DescricaoPais";
            ddlPaises.DataValueField = "CodigoPais";
            ddlPaises.DataBind();

            ddlPaises.SelectedValue = "1058";
            ddlPaises_SelectedIndexChanged(null, null);
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }
        protected void btnExcContato_Click(object sender, EventArgs e)
        {
            int intCttItem = 0;


            if (txtCttItem.Text != "Novo")
            {
                intCttItem = Convert.ToInt32(txtCttItem.Text);

                if (intCttItem != 0)
                    listCadPessoaContato.RemoveAll(x => x._CodigoItem == intCttItem);

                Session["ContatoPessoa"] = listCadPessoaContato;

                btnCanContato_Click(sender, e);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            
                if (Session["CaminhoFotoContato"] == null)
                {
                    imaFoto.ImageUrl = @"~\Images\CapturaNow.png" + "?cache" + DateTime.Now.Ticks.ToString();

            }
                else
                {
                    imaFoto.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoFotoContato"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();

            }
              


            if (Session["ContatoPessoa"] != null)
                listCadPessoaContato = (List<Pessoa_Contato>)Session["ContatoPessoa"];

            if (Session["CadPessoa"] != null)
                listCadPessoa = (List<Pessoa>)Session["CadPessoa"];

            foreach (Pessoa p in listCadPessoa)
            {
                txtCodPessoa.Text = p.CodigoPessoa.ToString();
                txtRazSocial.Text = p.NomePessoa;
            }

            if (Session["CARREGOUCNPJ"] == null)
            {
                if (Session["InscricaoPessoa"] != null)
                {
                    listCadPessoaInscricao = (List<Pessoa_Inscricao>)Session["InscricaoPessoa"];
                }
                else
                {
                    PessoaInscricaoDAL pi = new PessoaInscricaoDAL();
                    listCadPessoaInscricao = pi.ObterPessoaInscricoes(Convert.ToInt64(txtCodPessoa.Text));
                    
                }
                Session["CARREGOUCNPJ"] = "SIM";
                DdlTipoInscricao.DataSource = listCadPessoaInscricao;
                DdlTipoInscricao.DataTextField = "_DcrInscricao";
                DdlTipoInscricao.DataValueField = "_CodigoItem";
                DdlTipoInscricao.DataBind();

                CarregaDropDownList();
            }

            

            if (Session["CadPessoa_Contato"] != null)
            {
                string s = Session["CadPessoa_Contato"].ToString();
                string[] words = s.Split('³');

                txtCttItem.Text = "";
                foreach (string word in words)
                {
                    if (txtCttItem.Text == "")
                        txtCttItem.Text = word;
                }

                Habil_TipoDAL RnSituacao = new Habil_TipoDAL();
                ddlTipoContato.DataSource = RnSituacao.TipoContato();
                ddlTipoContato.DataTextField = "DescricaoTipo";
                ddlTipoContato.DataValueField = "CodigoTipo";
                ddlTipoContato.DataBind();
                CarregaDropDownList();


                if (txtCttItem.Text != "Novo")
                {
                    btnExcContato.Visible = true;
                    foreach (Pessoa_Contato p in listCadPessoaContato)
                    {
                        if (txtCttItem.Text == p._CodigoItem.ToString())
                        {
                            txtCttItem.Text = p._CodigoItem.ToString();
                            ddlTipoContato.SelectedValue = p._TipoContato.ToString();
                            txtNomeContato.Text = p._NomeContato.ToString();
                            if(p._Fone1.ToString().Length > 3)
                                txtfone1.Text = p._Fone1.ToString().Substring(3);
                            if (p._Fone2.ToString().Length > 3)
                                txtfone2.Text = p._Fone2.ToString().Substring(3);
                            if (p._Fone3.ToString().Length > 3)
                                txtfone3.Text = p._Fone3.ToString().Substring(3);
                            txtmailnfe.Text = p._MailNFE.ToString();
                            txtmailNFSe.Text = p._MailNFSE.ToString();
                            txtmail1.Text = p._Mail1.ToString();
                            txtmail2.Text = p._Mail2.ToString();
                            txtmail3.Text = p._Mail3.ToString();
                            ddlPaises.SelectedValue = p._CodigoPais.ToString();
                            ddlPaises_SelectedIndexChanged(sender, e);
                            txtMailSenha.Attributes["value"] = p._EmailSenha.ToString();

                            if (p._FoneWhatsApp == 1)
                                chkWhatsApp.Checked = true;
                            else
                                chkWhatsApp.Checked = false;


                            if ( p._Foto != null)
                            { 
                                byteArrayToImage(p._Foto);

                                imaFoto.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoFotoContato"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
                            }
                            else
                                imaFoto.ImageUrl = @"~\Images\CapturaNow.png" + "?cache" + DateTime.Now.Ticks.ToString(); 


                            DdlTipoInscricao.SelectedValue = p._CodigoInscricao.ToString();
                        }
                    }
                }
                else
                {
                    DdlTipoInscricao_SelectedIndexChanged(sender, e);
                }

                Session["CadPessoa_Contato"] = null;
            }

            txtNomeContato.Focus();
            if (txtCttItem.Text == "")
                btnCanContato_Click(sender, e);

           
        }
        protected Boolean ValidaTela()
        {

            foreach (Pessoa_Contato valor in listCadPessoaContato)
            {
                if (valor._CodigoInscricao.ToString().Equals(DdlTipoInscricao.SelectedValue) && (valor._CodigoItem.ToString() != txtCttItem.Text))
                {
                    ShowMessage("Inscrição já consta em outro Contato.", MessageType.Info);
                    return false;
                }
            }

            return true;
        }
        protected void btnSlvContato_Click(object sender, EventArgs e)
        {
            try
            {
                int intCttItem = 0;

                if (!ValidaTela())
                    return;


                if (txtCttItem.Text != "Novo")
                    intCttItem = Convert.ToInt32(txtCttItem.Text);
                else
                {
                    if (listCadPessoaContato.Count != 0)
                        intCttItem = Convert.ToInt32(listCadPessoaContato.Max(x => x._CodigoItem).ToString());

                    intCttItem = intCttItem + 1;
                }

                ////////////////////////////////////////////////////////////////////////////
                byte[] fotoLocal = null;
                if (Session["CaminhoFotoContato"] != null)
                { 
                    if (System.IO.File.Exists(Server.MapPath(@"\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoFotoContato"].ToString())))
                    {
                        Image image = Image.FromFile(Server.MapPath(@"\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoFotoContato"].ToString()));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            // Save image to stream.
                            image.Save(stream, ImageFormat.Bmp);
                            fotoLocal = stream.ToArray();
                        }
                        image.Dispose();
                    }
                }
                /////////////////////////////////////////////////////////////////////////////
                int indicadorWhats = 0;
                if (chkWhatsApp.Checked == true)
                    indicadorWhats = 1;

                Pessoa_Contato x1 = new Pessoa_Contato(
                                            intCttItem, 
                                            Convert.ToInt32(ddlTipoContato.SelectedValue),
                                            txtNomeContato.Text,
                                            lblFone1.Text + txtfone1.Text,
                                            lblFone2.Text + txtfone2.Text,
                                            lblFone3.Text + txtfone3.Text,
                                            txtmailnfe.Text,
                                            txtmailNFSe.Text,
                                            txtmail1.Text,
                                            txtmail2.Text,
                                            txtmail3.Text,
                                            (byte[])fotoLocal,
                                            Convert.ToInt32(DdlTipoInscricao.SelectedValue),
                                            txtMailSenha.Text,indicadorWhats,
                                            Convert.ToInt32(ddlPaises.SelectedValue)
                                        );

                if (intCttItem != 0)
                    listCadPessoaContato.RemoveAll(x => x._CodigoItem == intCttItem);

                listCadPessoaContato.Add(x1);

                Session["ContatoPessoa"] = listCadPessoaContato;
                btnCanContato_Click(sender, e);

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void txtfone1_TextChanged(object sender, EventArgs e)
        {
            try
            { 
                string telefone = txtfone1.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "").Replace(" ", "");
                decimal s = 0;
                if (decimal.TryParse(telefone, out s))
                {
                    if (ddlPaises.SelectedValue == "1058")
                    {
                        if (telefone.Length == 11)
                            txtfone1.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 00000\-0000");
                        else if (telefone.Length == 12)
                            txtfone1.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 00000\-0000");
                        else if (telefone.Length == 10)
                            txtfone1.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 0000\-0000");
                        else if (telefone.Length == 8)
                            txtfone1.Text = Convert.ToUInt64(telefone).ToString(@"0000\-0000");
                        else if (telefone.Length == 9)
                            txtfone1.Text = Convert.ToUInt64(telefone).ToString(@"00000\-0000");
                        else
                            txtfone1.Text = telefone;
                    }
                    txtfone2.Focus();
                }
                else
                {
                    txtfone1.Text = "";
                    txtfone1.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void txtfone2_TextChanged(object sender, EventArgs e)
        {
            try
            { 
                string telefone = txtfone2.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "").Replace(" ","");
                decimal s = 0;
                if (decimal.TryParse(telefone, out s))
                {
                    if (ddlPaises.SelectedValue == "1058")
                    {
                        if (telefone.Length == 11)
                            txtfone2.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 00000\-0000");
                        else if (telefone.Length == 12)
                            txtfone2.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 00000\-0000");
                        else if (telefone.Length == 10)
                            txtfone2.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 0000\-0000");
                        else if (telefone.Length == 8)
                            txtfone2.Text = Convert.ToUInt64(telefone).ToString(@"0000\-0000");
                        else if (telefone.Length == 9)
                            txtfone2.Text = Convert.ToUInt64(telefone).ToString(@"00000\-0000");
                        else
                            txtfone2.Text = telefone;
                    }

                    txtfone3.Focus();
                }
                else
                {
                    txtfone2.Text = "";
                    txtfone2.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void txtfone3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string telefone = txtfone3.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "").Replace(" ", "");
                decimal s = 0;
                if (decimal.TryParse(telefone, out s))
                {
                    if (ddlPaises.SelectedValue == "1058")
                    {
                        if (telefone.Length == 11)
                            txtfone3.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 00000\-0000");
                        else if (telefone.Length == 12)
                            txtfone3.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 00000\-0000");
                        else if (telefone.Length == 10)
                            txtfone3.Text = Convert.ToUInt64(telefone).ToString(@"\(00\)\ 0000\-0000");
                        else if (telefone.Length == 8)
                            txtfone3.Text = Convert.ToUInt64(telefone).ToString(@"0000\-0000");
                        else if (telefone.Length == 9)
                            txtfone3.Text = Convert.ToUInt64(telefone).ToString(@"00000\-0000");
                        else
                            txtfone3.Text = telefone;
                    }

                    txtmail1.Focus();
                }
                else
                {
                    txtfone3.Text = "";
                    txtfone3.Focus();
                }
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void txtmailnfe_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtmail3_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtmail2_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtmail1_TextChanged(object sender, EventArgs e)
        {

        }
        private void byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                AnexoDocumentoDAL ad = new AnexoDocumentoDAL();

                string FileName = txtCodPessoa.Text + "-" + txtRazSocial.Text + "-" + txtCttItem.Text + ".jpg";

                Session["CaminhoFotoContato"] = FileName;
                string CaminhoArquivoLog = Server.MapPath(@"\Scripts\CapturaWebcamASPNET\uploads\" + FileName);

                if (System.IO.File.Exists(CaminhoArquivoLog))
                    System.IO.File.Delete(CaminhoArquivoLog);

                FileStream file = new FileStream(CaminhoArquivoLog, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(file);
                bw.Write(byteArrayIn);
                bw.Close();

                file = new FileStream(CaminhoArquivoLog, FileMode.Open);
                BinaryReader br = new BinaryReader(file);
                file.Close();
            }
            catch (Exception ex)
            {
                throw  ex;
            }
        }
        protected void btnTiraFoto_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(ConfigurationManager.AppSettings["SoftwareDaCaptura"].ToString()))
                ShowMessage("Aplicativo da Foto Não Instalado!!! Caminho: " + ConfigurationManager.AppSettings["SoftwareDaCaptura"].ToString() + "!", MessageType.Warning);
            else
                ShowMessage("Tire a Foto pelo Aplicativo no Desktop (Área de Trabalho) e depois Clique em Capturar Caminho: " + ConfigurationManager.AppSettings["SoftwareDaCaptura"].ToString() + "!", MessageType.Warning);
        }
        protected void btnCapturaFoto_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(ConfigurationManager.AppSettings["ImagemDaCapturaFisica"].ToString()))
            {
                imaFoto.ImageUrl = ConfigurationManager.AppSettings["ImagemDaCapturaLogica"].ToString() +"?" + DateTime.Now.Ticks.ToString()+ "?cache" + DateTime.Now.Ticks.ToString();
            }
            else
            {
                imaFoto.ImageUrl = @"~\Images\Images.jpg?" + DateTime.Now.Ticks.ToString();
            }
        }

        protected void txtmailNFSe_TextChanged(object sender, EventArgs e)
        {

        }

        protected void DdlTipoInscricao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] array = DdlTipoInscricao.SelectedItem.Text.Split('(', ')');

                if (array[1] == "4")
                {
                    ddlTipoContato.SelectedValue = "13";
                }
                else if (array[1] == "3")
                {
                    ddlTipoContato.SelectedValue = "14";
                }
                List<Pessoa_Inscricao> List = new List<Pessoa_Inscricao>();
                List = (List<Pessoa_Inscricao>)Session["InscricaoPessoa"];
                List = List.Where(x => x._CodigoItem == Convert.ToInt32(DdlTipoInscricao.SelectedValue)).ToList();

                if (List.Count > 0)
                    ddlPaises.SelectedValue = List[0].CodigoPais.ToString();
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void ddlPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlPaises.SelectedValue == "1058")
            {
                lblFone1.Text = "+55";
                lblFone2.Text = "+55";
                lblFone3.Text = "+55";
            }
            else
            {
                lblFone1.Text = "";
                lblFone2.Text = "";
                lblFone3.Text = "";
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            // Convert byte[] to Image
            AnexoDocumentoDAL ad = new AnexoDocumentoDAL();
            string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Scripts\CapturaWebcamASPNET\uploads\";

            byte[] XMLBinary = Convert.FromBase64String(base_img.Text.Replace("data:image/jpeg;base64,", ""));


            string GUIDXML = txtCodPessoa.Text + "-" + txtRazSocial.Text + "-" + txtCttItem.Text + ".jpg";

            if (System.IO.File.Exists(CaminhoArquivoLog + GUIDXML))
                System.IO.File.Delete(CaminhoArquivoLog + GUIDXML);

            FileStream file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Create);

            BinaryWriter bw = new BinaryWriter(file);
            bw.Write(XMLBinary);
            bw.Close();

            file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Open);
            BinaryReader br = new BinaryReader(file);
            file.Close();
            Session["CaminhoFotoContato"] = GUIDXML;
            imaFoto.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoFotoContato"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
        }
    
        protected void btnEscolherFoto_Click(object sender, EventArgs e)
        {
            try
            {
                if (arquivo.HasFile)
                {
                    string ex = arquivo.FileName;

                    string ex2 = Path.GetExtension(ex);
                    if ((ex2.ToLower() == ".jpg" || ex2.ToLower() == ".jpeg" || ex2.ToLower() == ".png"))
                    {
                        AnexoDocumentoDAL ad = new AnexoDocumentoDAL();
                        string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Scripts\CapturaWebcamASPNET\uploads\";
                        byte[] XMLBinary = arquivo.FileBytes;

                        string GUIDXML = txtCodPessoa.Text + "-" + txtRazSocial.Text + "-" + txtCttItem.Text + ".jpg";

                        if (System.IO.File.Exists(CaminhoArquivoLog + GUIDXML))
                            System.IO.File.Delete(CaminhoArquivoLog + GUIDXML);

                        FileStream file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Create);

                        BinaryWriter bw = new BinaryWriter(file);
                        bw.Write(XMLBinary);
                        bw.Close();

                        file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Open);
                        BinaryReader br = new BinaryReader(file);
                        file.Close();
                        Session["CaminhoFotoContato"] = GUIDXML;
                        imaFoto.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoFotoContato"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
                    }
                    else
                    {
                        ShowMessage("Necessário arquivo .jpg, .jpeg ou .png", MessageType.Info);
                        Session["CaminhoFotoContato"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                Session["CaminhoFotoContato"] = null;
            }
        }

        protected void btnRemoverFoto_Click(object sender, EventArgs e)
        {
            string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Scripts\CapturaWebcamASPNET\uploads\";
            imaFoto.ImageUrl = @"~\Images\CapturaNow.png?cache" + DateTime.Now.Ticks.ToString();
            Usuario usu = new Usuario();
            UsuarioDAL usuDAL = new UsuarioDAL();
            usu = usuDAL.PesquisarUsuarioPorCodPessoa(Convert.ToInt64(txtCodPessoa.Text));
            string GUIDXML = "";
            if (usu != null)
                GUIDXML = usu.CodigoUsuario + "-" + txtRazSocial.Text + "-" + txtCttItem.Text + ".jpg";
            else
                GUIDXML = txtCodPessoa.Text + "-" + txtRazSocial.Text + "-" + txtCttItem.Text + ".jpg";

            if (System.IO.File.Exists(CaminhoArquivoLog + GUIDXML))
                System.IO.File.Delete(CaminhoArquivoLog + GUIDXML);

            Session["CaminhoFotoContato"] = null;
        }
    }
}