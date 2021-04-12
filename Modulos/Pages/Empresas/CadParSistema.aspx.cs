using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using DAL.Model;
using DAL.Persistence;
namespace SoftHabilInformatica.Pages.Empresas
{
    public partial class CadParSistema : System.Web.UI.Page
    {
        List<GeradorSequencialDocumentoEmpresa> ListaGeracaoSequencial= new List<GeradorSequencialDocumentoEmpresa>();

        public string PanelSelect { get; set; }
        public string PanelSelect2 { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaDropDownList(bool blnIncluir)
        {
            ParSistemaDAL RnParSistema = new ParSistemaDAL();


            if (blnIncluir)
                ddlEmpresa.DataSource = RnParSistema.ListarParSistemasInclusao ();
            else
                ddlEmpresa.DataSource = RnParSistema.ListarParSistemasAlteracao();

            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();

            if (blnIncluir)
                ddlEmpresa.Items.Insert(0, "..... SELECIONE UMA EMPRESA .....");
            else
                ddlEmpresa.Enabled = false; 

            TipoOperacaoDAL TipoOP = new TipoOperacaoDAL();
            ddlTipoOperacao.DataSource = TipoOP.ListarTipoOperacoes("", "", "", "");
            ddlTipoOperacao.DataTextField = "DescricaoTipoOperacao";
            ddlTipoOperacao.DataValueField = "CodigoTipoOperacao";
            ddlTipoOperacao.DataBind();
            ddlTipoOperacao.Items.Insert(0, "..... SELECIONE UM TIPO DE OPERAÇÃO.....");

            Habil_TipoDAL ht = new Habil_TipoDAL();
            ddlOrdem.DataSource = ht.TipoOrdemGeracaoNFe();
            ddlOrdem.DataTextField = "DescricaoTipo";
            ddlOrdem.DataValueField = "CodigoTipo";
            ddlOrdem.DataBind();
            
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;
            if (ddlEmpresa.Text == "..... SELECIONE UMA EMPRESA .....")
            {
                ShowMessage("Empresa deve ser Informada.", MessageType.Info);
                return false;
            }

            if (txtDiasOrc.Text != "")
            {
                v.CampoValido("Dias de Válidade De Orçamento", txtDiasOrc.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtDiasOrc.Text = "";

                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtDiasOrc.Focus();
                    }
                    return false;
                }
            }
            return true;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            

            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Error);
                Session["MensagemTela"] = null;
                return;
            }
            if (Session["TabFocada"] != null)
            {
                PanelSelect = Session["TabFocada"].ToString();
                Session["TabFocada"] = null;
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomParSistema2"] != null)
            {
                if (Session["ZoomParSistema2"].ToString() == "RELACIONAL")
                {
                   pnlPainel.Visible = false;
                    cmdSair.Visible = false;
                }
                else
                {
                    pnlPainel.Visible = true;
                    cmdSair.Visible = true;
                }
            }
            else
            {
                pnlPainel.Visible = true;
                cmdSair.Visible = true;
            }
            if (Session["TabFocadaParEmpresa"] != null)
            {
                PanelSelect = Session["TabFocadaParEmpresa"].ToString();
            }
            else
            {
                //PanelSelect = "home";
               // Session["TabFocadaEmpresa"] = "home";
            }

           //PanelSelect2 = "home2";
            //PanelSelect2 = "home3";

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConParSistema.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomParSistema2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomParSistema"] != null)
                {
                    string s = Session["ZoomParSistema"].ToString();
                    Session["ZoomParSistema"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                        {
                            if (ddlEmpresa.Text == "")
                            {
                                MontaDropDownList(false);
                                ParSistemaDAL r = new ParSistemaDAL();
                                ParSistema p = new ParSistema();
                                p = r.PesquisarParSistema(Convert.ToInt64(word));
                                CarregaTpOperacao();
                                ddlAjuste.SelectedValue = p.TipoAjusteInventario.ToString();
                                ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
                                txtCaracCategoria.Text = p.CaracteristaCategoria;
                                txtDiasOrc.Text = p.DiasValidadeOrc.ToString();
                                txtCaracLocalizacao.Text = p.CaracteristaLocalizacao;
                                chkEspelhamento.Checked = p.LocalizacaoEspelhada;
                                ddlTipoOperacao.SelectedValue = p.CodigoTipoOperacao.ToString();
                                txtCorFundo.Text = p.CorFundo;
                                txtCorPadrao.Text = p.CorPadrao;
                                ddlTipoMenu.SelectedValue = p.TipoMenu.ToString();
                                txtValorPedido.Text = p.ValorPedidoParaFreteMinimo.ToString();
                                txtFreteMinimo.Text = p.ValorFreteMinimo.ToString();
                                ddlOrdem.SelectedValue = p.CodigoSequenciaGeracaoNFe.ToString();
                                chkConfPedido.Checked = p.ConferePedidos;
                                chkCrtRegras.Checked = p.CriticaRegras;
                                ddlTpListPed.SelectedValue = p.TipoListagemPedido.ToString();
                                txtNroHrAlerta.Text = p.NumeroHorasEnvioAlerta.ToString();

                                lista.ForEach(delegate (Permissao x)
                                {
                                    if (!x.AcessoCompleto)
                                    {
                                        if (!x.AcessoAlterar)
                                            btnSalvar.Visible = false;
                                    }
                                });
                            }
                        }
                    }
                }
                else
                {
                    MontaDropDownList(true);
                    txtCorFundo.Text = "#FFFFFF";
                    txtCorPadrao.Text = "#000000";
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                        }
                    });

                }
            }

            if (ddlEmpresa.Text == "")
                btnVoltar_Click(sender, e);

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {

                if (ddlEmpresa.Text == "..... SELECIONE UMA EMPRESA .....")
                {
                    EmpresaDAL d = new EmpresaDAL();
                    d.Excluir(Convert.ToInt64(ddlEmpresa.SelectedValue));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Empresa não identificado.&emsp;&emsp;&emsp;";
             }
            catch (Exception ex)
            {
                strErro = "&emsp;&emsp;&emsp;" + ex.Message.ToString() + "&emsp;&emsp;&emsp;";
            }

            if (strErro != "")
            {
                lblMensagem.Text = strErro;
                pnlMensagem.Visible = true;
            }
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomParEmpresa"] = null;
            if (Session["ZoomParEmpresa2"] != null)
            {
                Session["ZoomParEmpresa2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadEmpresa.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return; 
            }
            Session["TabFocadaParEmpresa"] = null;
            Response.Redirect("~/Pages/Empresas/ConParSistema.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            string colorcode = txtCorPadrao.Text;
            int argb = Int32.Parse(colorcode.Replace("#", ""), NumberStyles.HexNumber);
            Color clr = Color.FromArgb(argb);


            int r = Convert.ToInt16(clr.R);
            int g = Convert.ToInt16(clr.G);
            int b = Convert.ToInt16(clr.B);
            int a = Convert.ToInt16(clr.A);
            decimal luminosidade = (r * 299 + g * 587 + b * 114) / 1000;

            string colorcode2 = txtCorFundo.Text;
            int argb2 = Int32.Parse(colorcode2.Replace("#", ""), NumberStyles.HexNumber);
            Color clr2 = Color.FromArgb(argb2);


            int r2 = Convert.ToInt16(clr2.R);
            int g2 = Convert.ToInt16(clr2.G);
            int b2 = Convert.ToInt16(clr2.B);
            int a2 = Convert.ToInt16(clr2.A);
            decimal luminosidade2 = (r2 * 299 + g2 * 587 + b2 * 114) / 1000;

            if(Math.Abs(luminosidade - luminosidade2) < 100)
            {
                ShowMessage("Tonalidade de cores muito parecidas", MessageType.Info);
                return;
            }
            GerarDesignSistema(luminosidade);
            if (GerarIconesMenu(luminosidade) == false)
                return;

            HttpRuntime.Cache.Insert("Pages", DateTime.Now);


            ParSistemaDAL d = new ParSistemaDAL();
            ParSistema p = new ParSistema ();

            p = d.PesquisarParSistema(Convert.ToInt64(ddlEmpresa.SelectedValue));

            if (p == null)
            {
                p = new ParSistema();
                p.CodigoEmpresa = Convert.ToInt64(ddlEmpresa.SelectedValue);
                p.CaracteristaCategoria = txtCaracCategoria.Text;
                p.CaracteristaLocalizacao = txtCaracLocalizacao.Text;
                p.LocalizacaoEspelhada = chkEspelhamento.Checked;
                p.NumeroHorasEnvioAlerta = Convert.ToInt32(txtNroHrAlerta.Text);
                if (ddlTipoOperacao.SelectedValue != "..... SELECIONE UM TIPO DE OPERAÇÃO.....")
                    p.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
                p.CorPadrao = txtCorPadrao.Text;
                p.CorFundo = txtCorFundo.Text;
                if(txtDiasOrc.Text != "")
                    p.DiasValidadeOrc = Convert.ToInt16(txtDiasOrc.Text);
                p.ValorPedidoParaFreteMinimo = Convert.ToDecimal(txtValorPedido.Text);
                p.ValorFreteMinimo = Convert.ToDecimal(txtFreteMinimo.Text);
                p.CodigoSequenciaGeracaoNFe = Convert.ToInt32(ddlOrdem.SelectedValue);
                p.ConferePedidos = chkConfPedido.Checked;
                p.CriticaRegras = chkCrtRegras.Checked;
                if(ddlTpListPed.SelectedValue != "")
                    p.TipoListagemPedido = Convert.ToInt32(ddlTpListPed.SelectedValue);
                else
                    p.TipoListagemPedido = 0;

                if (ddlTipoMenu.SelectedValue == "1")
                    p.TipoMenu = 1;
                else
                    p.TipoMenu = 2;
                d.Inserir(p);

                if (ddlAjuste.SelectedValue != " * Nenhum Selecionado * " && ddlAjuste.SelectedValue != "")
                    d.InserirTpAjuste(Convert.ToInt32(ddlAjuste.SelectedValue), Convert.ToInt32(ddlEmpresa.SelectedValue));

                    Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p = new ParSistema();
                p.CodigoEmpresa = Convert.ToInt64(ddlEmpresa.SelectedValue);
                p.CaracteristaCategoria = txtCaracCategoria.Text;
                p.CaracteristaLocalizacao = txtCaracLocalizacao.Text;
                p.NumeroHorasEnvioAlerta = Convert.ToInt32(txtNroHrAlerta.Text);
                p.LocalizacaoEspelhada = chkEspelhamento.Checked;
                if(ddlTipoOperacao.SelectedValue != "..... SELECIONE UM TIPO DE OPERAÇÃO.....")
                    p.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
                p.CorPadrao = txtCorPadrao.Text;
                p.CorFundo = txtCorFundo.Text;
                p.DiasValidadeOrc = Convert.ToInt16(txtDiasOrc.Text);
                p.ValorPedidoParaFreteMinimo = Convert.ToDecimal(txtValorPedido.Text);
                p.ValorFreteMinimo = Convert.ToDecimal(txtFreteMinimo.Text);
                p.CodigoSequenciaGeracaoNFe = Convert.ToInt32(ddlOrdem.SelectedValue);
                p.ConferePedidos = chkConfPedido.Checked;
                p.CriticaRegras = chkCrtRegras.Checked;

                if (ddlTpListPed.SelectedValue != "")
                    p.TipoListagemPedido = Convert.ToInt32(ddlTpListPed.SelectedValue);
                else
                    p.TipoListagemPedido = 0;

                if (ddlTipoMenu.SelectedValue == "1")
                    p.TipoMenu = 1;
                else
                    p.TipoMenu = 2;

                d.Atualizar(p);

                if (ddlAjuste.SelectedValue != " * Nenhum Selecionado * " && ddlAjuste.SelectedValue != "")
                    d.InserirTpAjuste(Convert.ToInt32(ddlAjuste.SelectedValue), Convert.ToInt32(ddlEmpresa.SelectedValue));

                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }
            Session["VW_Par_Sistema"] = null;

            btnVoltar_Click(sender, e);

        }
        protected void GerarDesignSistema(decimal Luminosidade)
        {
            try
            {
                List<string> dados = new List<string>();
                // indica qual o caminho do documento
                string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Content\EstiloDefinidoPorParametro.css";

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);


                FileStream file = new FileStream(filePath, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(file);
                bw.Close();

                string nomeArquivo = filePath;
                System.IO.TextWriter arquivo = System.IO.File.AppendText(nomeArquivo);



                arquivo.WriteLine(":root {--CorEscolhidaPadrao: " + txtCorPadrao.Text + ";--CorEscolhidaFundo:" + txtCorFundo.Text + ";}");
                arquivo.WriteLine(".CorPadraoEscolhida{background-color: var(--CorEscolhidaPadrao)!important;border-color:var(--CorEscolhidaPadrao)!important;}");
                arquivo.WriteLine(".CorPadraoEscolhidaItemMenu{color:var(--CorEscolhidaPadrao)!important;border-color:var(--CorEscolhidaPadrao)!important;}");
                arquivo.WriteLine(".CorPadraoEscolhidaBorder{border-bottom:2px solid var(--CorEscolhidaPadrao)}");
                arquivo.WriteLine(".CorPadraoEscolhidaTexto{color:var(--CorEscolhidaFundo)!important;}");
                arquivo.WriteLine(".CorPadraoFundo{background-color:var(--CorEscolhidaFundo)!important}");
                arquivo.WriteLine(".CorPadraoItemHover:hover{opacity:0.8!important}");
                arquivo.WriteLine(".LogoDaEmpresa{background-image: url('../Images/LogoDaEmpresa.png');background-size:65px 49px}");
                arquivo.WriteLine(".PapelDeParedePadrao{background-image: url('../Images/PapelDeParede.jpg');background-size:100%}");
                arquivo.WriteLine(".panel-heading-padrao{background-color:var(--CorEscolhidaPadrao)!important;color:var(--CorEscolhidaFundo)!important;border-color:var(--CorEscolhidaPadrao)!important}");
                if (Luminosidade > 155)
                {
                    arquivo.WriteLine(".btn-primary{background-color:var(--CorEscolhidaPadrao)!important;opacity:0.5;border:1px solid var(--CorEscolhidaFundo)!important;color:var(--CorEscolhidaFundo)!important;}");
                    arquivo.WriteLine(".panel-primary > .panel-heading{background:var(--CorEscolhidaPadrao)!important;border-color:var(--CorEscolhidaFundo)!important;color:var(--CorEscolhidaFundo)!important}");
                }
                else
                {
                    arquivo.WriteLine(".btn-primary{background-color:var(--CorEscolhidaPadrao)!important;opacity:0.5;border:1px solid var(--CorEscolhidaPadrao)!important;color:var(--CorEscolhidaFundo)!important;}");
                    arquivo.WriteLine(".panel-primary > .panel-heading{background:var(--CorEscolhidaPadrao)!important;border-color:var(--CorEscolhidaPadrao)!important;color:var(--CorEscolhidaFundo)!important}");
                }


                arquivo.WriteLine(".panel-primary{border-color:var(--CorEscolhidaPadrao)!important}");
                arquivo.WriteLine(".btn-primary:hover{background-color:var(--CorEscolhidaPadrao)!important;opacity:0.7!important;}");
                arquivo.WriteLine(".CorPadraoEscolhidaBorderRight{border-right:2px solid var(--CorEscolhidaPadrao)}");

                arquivo.Close();

                byte[] ArquivoImagemLogoByte;
                byte[] ArquivoImagemPapelParedeByte;
                Guid meuNovoGuid;
                meuNovoGuid = Guid.NewGuid();

                string LogoEmpresa = ArquivoImagemLogo.FileName;
                string PapelParede = ArquivoImagemPapelParede.FileName;

                ArquivoImagemLogoByte = null;
                ArquivoImagemPapelParedeByte = null;

                if (ArquivoImagemLogo.HasFile)
                {
                    ArquivoImagemLogoByte = ArquivoImagemLogo.FileBytes;
                }
                if (ArquivoImagemPapelParede.HasFile)
                {
                    ArquivoImagemPapelParedeByte = ArquivoImagemPapelParede.FileBytes;

                }

                if (ArquivoImagemLogoByte != null)
                {

                    string CaminhoLogoDaEmpresa = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Images\\LogoDaEmpresa.png";
                    if (System.IO.File.Exists(CaminhoLogoDaEmpresa))
                        System.IO.File.Delete(CaminhoLogoDaEmpresa);

                    FileStream file2 = new FileStream(CaminhoLogoDaEmpresa, FileMode.Create);
                    BinaryWriter bw2 = new BinaryWriter(file2);
                    bw2.Write(ArquivoImagemLogoByte);
                    bw2.Close();

                    file2 = new FileStream(CaminhoLogoDaEmpresa, FileMode.Open);
                    BinaryReader br = new BinaryReader(file2);
                    file2.Close();

                    string CaminhoLogoDaEmpresaJPG = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Images\\LogoDaEmpresa.jpg";
                    if (System.IO.File.Exists(CaminhoLogoDaEmpresaJPG))
                        System.IO.File.Delete(CaminhoLogoDaEmpresaJPG);

                    System.Drawing.Image myImage = System.Drawing.Image.FromFile(CaminhoLogoDaEmpresa);
                    using (var b = new Bitmap(myImage.Width, myImage.Height))
                    {
                        b.SetResolution(myImage.HorizontalResolution, myImage.VerticalResolution);

                        using (var g = Graphics.FromImage(b))
                        {
                            g.Clear(Color.White);
                            g.DrawImageUnscaled(myImage, 0, 0);
                        }

                        // Now save b as a JPEG like you normally would
                        b.Save(CaminhoLogoDaEmpresaJPG, System.Drawing.Imaging.ImageFormat.Jpeg);
                       
                    }
                    System.IO.File.Exists(CaminhoLogoDaEmpresaJPG);
                    System.IO.File.Exists(CaminhoLogoDaEmpresa);
                }
                if (ArquivoImagemPapelParedeByte != null)
                {
                    string CaminhoPapelDeParede = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Images\\PapelDeParede.jpg";
                    if (System.IO.File.Exists(CaminhoPapelDeParede))
                        System.IO.File.Delete(CaminhoPapelDeParede);

                    FileStream file3 = new FileStream(CaminhoPapelDeParede, FileMode.Create);
                    BinaryWriter bw3 = new BinaryWriter(file3);
                    bw3.Write(ArquivoImagemPapelParedeByte);
                    bw3.Close();

                    file3 = new FileStream(CaminhoPapelDeParede, FileMode.Open);
                    BinaryReader br3 = new BinaryReader(file3);
                    file3.Close();
                }
                
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected bool GerarIconesMenu(decimal Luminosidade)//gerando imagem .SVG com cores dinamicas
        {
            bool GerouTodas = true;
            string CaminhoSis = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());
            try
            {
                List<MenuSistema> ListMenu = new List<MenuSistema>();
                MenuSistemaDAL MenuDAL = new MenuSistemaDAL();
                ListMenu = MenuDAL.ListarMenuSistema("", "", "", "");
                string CorPadrao = txtCorPadrao.Text;
                string CorFundo = txtCorFundo.Text;
                
                //icone barra superior do sistema
                MenuSistema IconeBarraSuperior = new MenuSistema();
                IconeBarraSuperior.UrlIcone = @"\Images\IconeBarraSuperior.svg";
                ListMenu.Add(IconeBarraSuperior);

                //icone de acesso 
                MenuSistema IconeAcessar = new MenuSistema();
                IconeAcessar.UrlIcone = @"\Images\Acessar.svg";
                ListMenu.Add(IconeAcessar);

                // imagem de loading
                MenuSistema FileImagemLoading = new MenuSistema();
                FileImagemLoading.UrlIcone = @"\Images\Loading.svg";
                ListMenu.Add(FileImagemLoading);

                //icone do sistema
                MenuSistema IconeSistema = new MenuSistema();
                IconeSistema.UrlIcone = @"\Images\IconePagina.svg";
                ListMenu.Add(IconeSistema);

                //icone do sistema
                MenuSistema IconeToTop = new MenuSistema();
                IconeToTop.UrlIcone = @"\Images\ToTop.svg";
                ListMenu.Add(IconeToTop);

                MenuSistema IconeHelp = new MenuSistema();
                IconeHelp.UrlIcone = @"\Images\Help.svg";
                ListMenu.Add(IconeHelp);

                //icone do menu do sistema
                foreach (MenuSistema menu in ListMenu)
                {
                    if(menu.UrlIcone.ToLower() != "" && menu.UrlIcone.ToLower() != ("~/images/nulo.png").ToLower() && menu.UrlIcone.ToLower() != ("~/images/novo.svg").ToLower() && @menu.UrlIcone.ToLower() != (@"~\images\novo.svg").ToLower())
                    {
                        
                        string filePath = CaminhoSis + menu.UrlIcone.Replace("~", "");

                        XElement root = XElement.Load(filePath);
                         if (menu.UrlIcone == @"\Images\Acessar.svg" || menu.UrlIcone == @"\Images\IconePagina.svg" || menu.UrlIcone == @"\Images\Loading.svg" || menu.UrlIcone == @"\Images\ToTop.svg")
                        {
                            if (Luminosidade > 155)
                                root.Attribute("fill").Value = CorFundo;
                            else
                                root.Attribute("fill").Value = CorPadrao;
                        }
                        else if (menu.UrlIcone == @"\Images\IconeBarraSuperior.svg" || menu.UrlIcone == @"\Images\Help.svg")
                        {
                            root.Attribute("fill").Value = CorFundo;
                        }
                        else
                        {
                            if (menu.CodigoPaiMenu != 0)
                            {
                                if (Luminosidade > 155)
                                    root.Attribute("fill").Value = CorFundo;
                                else
                                    root.Attribute("fill").Value = CorPadrao;
                            }
                            else
                                root.Attribute("fill").Value = CorPadrao;
                        }                            
                        root.Save(filePath);
                        Console.WriteLine(root);
                    }                   
                }
            }
            catch(Exception ex)
            {
                ShowMessage("Erro ao gerar Icones do menu: "+ex, MessageType.Info);
                GerouTodas = false;
            }
            return GerouTodas;
        }
        protected void CarregaTpOperacao()
        {
            Habil_TipoDAL RnHabil_TipoDAL = new Habil_TipoDAL();
            TipoOperacaoDAL RnTpOper = new TipoOperacaoDAL();

            ddlAjuste.Items.Clear();
            ddlAjuste.DataSource = RnTpOper.ListarTipoAjuste();
            ddlAjuste.DataTextField = "DescricaoTipoOperacao";
            ddlAjuste.DataValueField = "CodigoTipoOperacao";
            ddlAjuste.SelectedValue = null;
            ddlAjuste.DataBind();
            ddlAjuste.Items.Insert(0, " * Nenhum Selecionado * ");
            ddlAjuste.Enabled = true;

            ddlTpListPed.Items.Clear();
            ddlTpListPed.DataSource = RnHabil_TipoDAL.SituacaoListagemPedido();
            ddlTpListPed.DataTextField = "DescricaoTipo";
            ddlTpListPed.DataValueField = "CodigoTipo";
            ddlTpListPed.SelectedValue = null;
            ddlTpListPed.DataBind();
            ddlTpListPed.Enabled = true;
        }
        protected void txtValorPedido_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtValorPedido.Text.Equals(""))
            {
                txtValorPedido.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor pedido", txtValorPedido.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtValorPedido.Text = Convert.ToDecimal(txtValorPedido.Text).ToString("###,##0.00");
                    txtFreteMinimo.Focus();
                }
                else
                    txtValorPedido.Text = "0,00";
            }
        }

        protected void txtFreteMinimo_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtFreteMinimo.Text.Equals(""))
            {
                txtFreteMinimo.Text = "0,00";
            }
            else
            {
                v.CampoValido("Frete minimo", txtFreteMinimo.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtFreteMinimo.Text = Convert.ToDecimal(txtFreteMinimo.Text).ToString("###,##0.00");
                    
                }
                else
                    txtFreteMinimo.Text = "0,00";

            }
        }

        protected void txtNroHrAlerta_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtNroHrAlerta.Text.Equals(""))
            {
                txtNroHrAlerta.Text = "1";
            }
            else
            {
                v.CampoValido("Nro. horas do alerta", txtNroHrAlerta.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                    txtNroHrAlerta.Text = "1";

            }
        }
    }
}