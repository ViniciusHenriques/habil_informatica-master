using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using DAL;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.HabilUtilitarios
{
    public partial class ManGerEmails : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        clsValidacao v1 = new clsValidacao();
        String strMensagemR = "";
        List<HabilEmailDestinatario> listaDestinatariosPara = new List<HabilEmailDestinatario>();
        List<HabilEmailDestinatario> listaDestinatariosComCopia = new List<HabilEmailDestinatario>();
        List<HabilEmailDestinatario> listaDestinatariosComCopiaOculta = new List<HabilEmailDestinatario>();
        List<HabilEmailAnexo> listaAnexos= new List<HabilEmailAnexo>();

        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaTela()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();

            Session["LST_DEST_PARA"] = null;
            Session["LST_DEST_CC"] = null;
            Session["LST_DEST_CCO"] = null;
            Session["LST_ANEXO"] = null;

            ddlSituacao.DataSource = sd.SituacaoGeradorEmail();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();


        }
        protected Boolean ValidaCampos()
        {
            return true;
        }
        protected void MontaDestinatarios()
        {
            txtDestinatarioPara.Text = "";
            foreach (var item in listaDestinatariosPara)
            {
                if (txtDestinatarioPara.Text != "")
                    txtDestinatarioPara.Text += "; ";
                txtDestinatarioPara.Text += "(" + item.NM_DESTINATARIO + ") -" + item.TX_EMAIL;
            }

            txtDestinatarioCopia.Text = "";
            foreach (var item in listaDestinatariosComCopia)
            {
                if (txtDestinatarioCopia.Text != "")
                    txtDestinatarioCopia.Text += "; ";
                txtDestinatarioCopia.Text += "(" + item.NM_DESTINATARIO + ") -" + item.TX_EMAIL;
            }

            txtDestinatarioCopiaOculta.Text = "";
            foreach (var item in listaDestinatariosComCopiaOculta)
            {
                if (txtDestinatarioCopiaOculta.Text != "")
                    txtDestinatarioCopiaOculta.Text += "; ";
                txtDestinatarioCopiaOculta.Text += "(" + item.NM_DESTINATARIO + ") -" + item.TX_EMAIL;
            }

        }
        protected void MontaTela()
        {
            LimpaTela();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            HabilEmailCriado Hec = new HabilEmailCriado();
            HabilEmailCriadoDAL HecDAL = new HabilEmailCriadoDAL();
            List<HabilEmailCriado> listaU = new List<HabilEmailCriado>();
            List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
            DBTabelaCampos rowp = new DBTabelaCampos();

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConGerEmails.aspx");
            lista.ForEach(delegate (Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoAlterar)
                        btnSalvar.Visible = false;
                }
            });

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomGeracaiDosEmails2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomGeracaiDosEmails"] != null)
                {
                    string s = Session["ZoomGeracaiDosEmails"].ToString();
                    Session["ZoomGeracaiDosEmails"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                rowp = new DBTabelaCampos();

                                rowp.Filtro = "CD_INDEX";
                                rowp.Inicio = txtCodigo.Text;
                                rowp.Fim = txtCodigo.Text;
                                rowp.Tipo = "INT";
                                listaT.Add(rowp);

                                listaU = HecDAL.ListarEmails(listaT);

                                MontaTela();

                                foreach (var item in listaU)
                                {
                                    if (item.CD_SITUACAO != 110)
                                    { 
                                        btnSalvar.Visible = false;
                                        BtnEditarDS.Visible = false;
                                        btnAnexo.Visible = false;
                                        btnAddContato.Visible = false;
                                        btnEnviar.Visible = false;
                                        blnContatos.Visible = false;
                                    }
                                    else
                                    {
                                        btnEncaminhar.Visible = false;
                                    }

                                    txtCodigo.Text = item.CD_INDEX.ToString();
                                    txtTentaEnvio.Text = item.NR_TENTA_ENVIO.ToString();
                                    txtAssunto.Text = item.TX_ASSUNTO;
                                    txtDataLancamento.Text = item.DT_LANCAMENTO.ToString();
                                    txtDataEnvio.Text = item.DT_ENVIO.ToString();
                                    ddlSituacao.SelectedValue  = item.CD_SITUACAO.ToString();

                                    listaDestinatariosPara = item.listaDestinatarios;
                                    GrdPara.DataSource = listaDestinatariosPara;
                                    GrdPara.DataBind();
                                    Session["LST_DEST_PARA"] = listaDestinatariosPara;

                                    listaDestinatariosComCopia = item.listaDestinatariosComCopia;
                                    GrdComCopia.DataSource = listaDestinatariosComCopia;
                                    GrdComCopia.DataBind();
                                    Session["LST_DEST_CC"] = listaDestinatariosComCopia;

                                    listaDestinatariosComCopiaOculta = item.listaDestinatariosComCopiaOculta;
                                    grdComCopiaOculta.DataSource = listaDestinatariosComCopiaOculta;
                                    grdComCopiaOculta.DataBind();
                                    Session["LST_DEST_CCO"] = listaDestinatariosComCopiaOculta;

                                    listaAnexos = item.listaAnexos;
                                    grdAnexo.DataSource = listaAnexos;
                                    grdAnexo.DataBind();
                                    Session["LST_ANEXO"] = listaAnexos;

                                    string str3 = Server.HtmlDecode(item.TX_CORPO);
                                    litDescricao.Text = str3;
                                    Session["RascunhoEmail"] = str3;


                                    if (item.TX_ERRO.ToString() != "")
                                    {
                                        ShowMessage(item.TX_ERRO.ToString(), MessageType.Info);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    LimpaTela();
                    txtCodigo.Text = "Novo";
                    btnEncaminhar.Visible = false;  

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

            if (Session["GeracaoDosEmails"] != null)
            {
                HabilEmailCriado HEC = new HabilEmailCriado();

                HEC = (HabilEmailCriado)Session["GeracaoDosEmails"];

                if(HEC.CD_INDEX != 0)
                    txtCodigo.Text = HEC.CD_INDEX.ToString();
                txtAssunto.Text = HEC.TX_ASSUNTO;
                ddlSituacao.SelectedValue = Convert.ToString(HEC.CD_SITUACAO);
                txtDataLancamento.Text = HEC.DT_LANCAMENTO.ToString("dd/MM/yyyy hh:mm:ss");
                
                listaDestinatariosPara = HEC.listaDestinatarios;
                GrdPara.DataSource = listaDestinatariosPara;
                GrdPara.DataBind();
                Session["LST_DEST_PARA"] = listaDestinatariosPara;

                listaDestinatariosComCopia = HEC.listaDestinatariosComCopia;
                GrdComCopia.DataSource = listaDestinatariosComCopia;
                GrdComCopia.DataBind();
                Session["LST_DEST_CC"] = listaDestinatariosComCopia;

                listaDestinatariosComCopiaOculta = HEC.listaDestinatariosComCopiaOculta;
                grdComCopiaOculta.DataSource = listaDestinatariosComCopiaOculta;
                grdComCopiaOculta.DataBind();
                Session["LST_DEST_CCO"] = listaDestinatariosComCopiaOculta;

                listaAnexos = HEC.listaAnexos;
                grdAnexo.DataSource = listaAnexos;
                grdAnexo.DataBind();
                Session["LST_ANEXO"] = listaAnexos;
                

                Session["GeracaoDosEmails"] = null;
            }

            string Texto = Convert.ToString(Session["RascunhoEmail"]);
            string str2 = Server.HtmlDecode(Texto);
            litDescricao.Text = str2;

            if (Session["LST_DEST_PARA"] != null)
                listaDestinatariosPara = (List<HabilEmailDestinatario>)Session["LST_DEST_PARA"];
            if (Session["LST_DEST_CC"] != null)
                listaDestinatariosComCopia = (List<HabilEmailDestinatario>)Session["LST_DEST_CC"];
            if (Session["LST_DEST_CCO"] != null)
                listaDestinatariosComCopiaOculta = (List<HabilEmailDestinatario>)Session["LST_DEST_CCO"];
            if (Session["LST_ANEXO"] != null)
                listaAnexos = (List<HabilEmailAnexo>)Session["LST_ANEXO"];

            MontaDestinatarios();


            if (txtCodigo.Text == "")
            {
                btnVoltar_Click(sender, e);
            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Request.QueryString["IN"]) == 1)
                Response.Redirect("~/Pages/Vendas/RelOrcamento.aspx");
            else if (Convert.ToInt32(Request.QueryString["IN"]) == 2)
                Response.Redirect("~/Pages/Vendas/RelPedido.aspx");
            else if (Convert.ToInt32(Request.QueryString["IN"]) == 3)
                Response.Redirect("~/Pages/Compras/ConCotPreco.aspx");
            else
                Response.Redirect("~/Pages/HabilUtilitarios/ConGerEmails.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            HabilEmailCriado HEC = new HabilEmailCriado();
            HabilEmailCriadoDAL HECDAL = new HabilEmailCriadoDAL();

            HEC.DT_LANCAMENTO = v.ObterDataHora();

            HEC.listaDestinatarios = listaDestinatariosPara;
            HEC.listaDestinatariosComCopia = listaDestinatariosComCopia;
            HEC.listaDestinatariosComCopiaOculta = listaDestinatariosComCopiaOculta;
            HEC.listaAnexos = listaAnexos;

            HEC.TX_ASSUNTO = txtAssunto.Text;
            HEC.TX_CORPO = litDescricao.Text;
            HEC.CD_USU_REMETENTE = Convert.ToInt64(Session["CodUsuario"].ToString());
            ddlSituacao.SelectedValue = "110"; //Rascunho Imediatamente
            HEC.CD_SITUACAO = Convert.ToInt64(ddlSituacao.SelectedValue); 
            HEC.IN_HTML = 1;
            
            if (txtCodigo.Text == "Novo")
                HEC.CD_INDEX = 0;
            else
                HEC.CD_INDEX = Convert.ToInt64(txtCodigo.Text);

            HECDAL.Grava_ColecaoEmail(HEC);

            txtCodigo.Text = HEC.CD_INDEX.ToString();

            ShowMessage("E-mail salvo com Sucesso!!!", MessageType.Info);

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {

        }
        protected void BtnEditarDS_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            if (Request.QueryString["IN"] != null)
                Response.Redirect("~/Pages/Servicos/ManRasDocumento.aspx?cad=4&IN="+ Request.QueryString["IN"]);
            else
                Response.Redirect("~/Pages/Servicos/ManRasDocumento.aspx?cad=4");
        }
        protected void blnContatos_Click(object sender, EventArgs e)
        {
            HabilEmailUsuarioContatoDAL HeUC = new HabilEmailUsuarioContatoDAL();
            if (pnlVerificar.Visible)
            {
                pnlVerificar.Visible = false;
                pnlAlterar.Visible = true;
                pnlNovo.Visible = false;
                pnlAnexos.Visible = false;

                if (Session["CodUsuario"] != null)
                {
                    ddlContato.DataSource = HeUC.ObterContatos(Convert.ToInt64(Session["CodUsuario"].ToString()));
                }
                else
                {
                    ddlContato.DataSource = HeUC.ObterContatos(0);
                }


                ddlContato.DataTextField = "NM_CONTATO_EMAIL";
                ddlContato.DataValueField = "CD_INDEX";
                ddlContato.DataBind();
                ddlContato.Items.Insert(0, "*Nenhum Selecionado");

            }
            else
            {
                pnlVerificar.Visible = true;
                pnlAlterar.Visible = false;
                pnlNovo.Visible = false;
                pnlAnexos.Visible = false;
            }

            MontaDestinatarios();
        }
        protected void BtnPara_Click(object sender, EventArgs e)
        {
            HabilEmailDestinatario HeDestina = new HabilEmailDestinatario();
            HeDestina = new HabilEmailDestinatario();
            bool blnGrava = true;

            if (ddlContato.Text == "*Nenhum Selecionado")
            {
                ShowMessage("Contato deve ser selecionado.", MessageType.Info);
                ddlContato.Focus();
                return;
            }

            string x = ddlContato.SelectedItem.ToString();
            string x1 = x.Split('-')[0].Trim();
            string x2 = x.Substring(x1.Length + 3, (x.Length - (x1.Length + 3)));


            if ((x1 != "") && (x2 != ""))
            {
                HeDestina.NM_DESTINATARIO = x2;
                HeDestina.TX_EMAIL = x1;

                foreach (var item in listaDestinatariosPara)
                {
                    if (item.TX_EMAIL == x1)
                        blnGrava = false;
                }

                if (blnGrava)
                {
                    listaDestinatariosPara.Add(HeDestina);

                    GrdPara.DataSource = listaDestinatariosPara;
                    GrdPara.DataBind();
                }
                Session["LST_DEST_PARA"] = listaDestinatariosPara;
            }
        }
        protected void GrdPara_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMailaExcluir = HttpUtility.HtmlDecode(GrdPara.SelectedRow.Cells[1].Text);
            foreach (var item in listaDestinatariosPara)
            {
                if (item.TX_EMAIL == strMailaExcluir)
                {
                    listaDestinatariosPara.Remove(item);
                    goto GOTO_GrdPara;
                }
            }
            GOTO_GrdPara:
            GrdPara.DataSource = listaDestinatariosPara;
            GrdPara.DataBind();
            Session["LST_DEST_PARA"] = listaDestinatariosPara;
        }
        protected void BtnComCopia_Click(object sender, EventArgs e)
        {
            HabilEmailDestinatario HeDestina = new HabilEmailDestinatario();
            HeDestina = new HabilEmailDestinatario();
            bool blnGrava = true;

            if (ddlContato.Text == "*Nenhum Selecionado")
            {
                ShowMessage("Contato deve ser selecionado.", MessageType.Info);
                ddlContato.Focus();
                return;
            }

            string x = ddlContato.SelectedItem.ToString();
            string x1 = x.Split('-')[0].Trim();
            string x2 = x.Substring(x1.Length + 3, (x.Length - (x1.Length + 3)));


            if ((x1 != "") && (x2 != ""))
            {
                HeDestina.NM_DESTINATARIO = x2;
                HeDestina.TX_EMAIL = x1;

                foreach (var item in listaDestinatariosComCopia)
                {
                    if (item.TX_EMAIL == x1)
                        blnGrava = false;
                }

                if (blnGrava)
                {
                    listaDestinatariosComCopia.Add(HeDestina);

                    GrdComCopia.DataSource = listaDestinatariosComCopia;
                    GrdComCopia.DataBind();
                }
                Session["LST_DEST_CC"] = listaDestinatariosComCopia;
            }
        }
        protected void GrdComCopia_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMailaExcluir = HttpUtility.HtmlDecode(GrdComCopia.SelectedRow.Cells[1].Text);
            foreach (var item in listaDestinatariosComCopia)
            {
                if (item.TX_EMAIL == strMailaExcluir)
                {
                    listaDestinatariosComCopia.Remove(item);
                    goto GOTO_GrdCC;
                }
            }
            GOTO_GrdCC:
            GrdComCopia.DataSource = listaDestinatariosComCopia;
            GrdComCopia.DataBind();
            Session["LST_DEST_CC"] = listaDestinatariosComCopia;
        }
        protected void BtnComCopiaOculta_Click(object sender, EventArgs e)
        {
            HabilEmailDestinatario HeDestina = new HabilEmailDestinatario();
            HeDestina = new HabilEmailDestinatario();
            bool blnGrava = true;

            if (ddlContato.Text == "*Nenhum Selecionado")
            {
                ShowMessage("Contato deve ser selecionado.", MessageType.Info);
                ddlContato.Focus();
                return;
            }

            string x = ddlContato.SelectedItem.ToString();
            string x1 = x.Split('-')[0].Trim();
            string x2 = x.Substring(x1.Length + 3, (x.Length - (x1.Length + 3)));



            if ((x1 != "") && (x2 != ""))
            {
                HeDestina.NM_DESTINATARIO = x2;
                HeDestina.TX_EMAIL = x1;

                foreach (var item in listaDestinatariosComCopiaOculta)
                {
                    if (item.TX_EMAIL == x1)
                        blnGrava = false;
                }

                if (blnGrava)
                {
                    listaDestinatariosComCopiaOculta.Add(HeDestina);

                    grdComCopiaOculta.DataSource = listaDestinatariosComCopiaOculta;
                    grdComCopiaOculta.DataBind();
                }
                Session["LST_DEST_CCO"] = listaDestinatariosComCopiaOculta;
            }
        }
        protected void grdComCopiaOculta_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMailaExcluir = HttpUtility.HtmlDecode(grdComCopiaOculta.SelectedRow.Cells[1].Text);
            foreach (var item in listaDestinatariosComCopiaOculta)
            {
                if (item.TX_EMAIL == strMailaExcluir)
                {
                    listaDestinatariosComCopiaOculta.Remove(item);
                    goto GOTO_GrdCCO;
                }
            }
            GOTO_GrdCCO:
            grdComCopiaOculta.DataSource = listaDestinatariosComCopiaOculta;
            grdComCopiaOculta.DataBind();
            Session["LST_DEST_CCO"] = listaDestinatariosComCopiaOculta;

        }
        protected void btnAddContato_Click(object sender, EventArgs e)
        {
            if (pnlNovo.Visible)
            {
                pnlVerificar.Visible = false;
                pnlAlterar.Visible = true;
                pnlNovo.Visible = false;
                pnlAnexos.Visible = false;
            }
            else
            {
                pnlVerificar.Visible = false;
                pnlAlterar.Visible = false;
                pnlNovo.Visible = true;
                pnlAnexos.Visible = false;
            }

            txtNomeContato.Text = "";
            txtEmailContato.Text = "";
            txtNomeContato.Focus();
        }
        protected void btnSalvarContato_Click(object sender, EventArgs e)
        {
            HabilEmailUsuarioContatoDAL HeUCDAL = new HabilEmailUsuarioContatoDAL();
            HabilEmailUsuarioContato HeUC = new HabilEmailUsuarioContato();

            if (txtNomeContato.Text == "")
            {
                ShowMessage("Nome deve ser informado.", MessageType.Info);
                return;
            }

            if (txtEmailContato.Text == "")
            {
                ShowMessage("Email deve ser informado.", MessageType.Info);
                return;
            }

            HeUC = new HabilEmailUsuarioContato();

            if (Session["CodUsuario"] != null)
                HeUC.CD_USUARIO = Convert.ToInt64(Session["CodUsuario"].ToString());
            else
                HeUC.CD_USUARIO = 0;

            HeUC.NM_CONTATO = txtNomeContato.Text;
            HeUC.TX_EMAIL = txtEmailContato.Text;

            HeUCDAL.SalvarContato(HeUC);


            /*--------------------------------------------------------------------------------------------------------*/
            pnlAlterar.Visible = true;
            pnlNovo.Visible = false;
            pnlVerificar.Visible = false;
            pnlAnexos.Visible = false;

            if (Session["CodUsuario"] != null)
                ddlContato.DataSource = HeUCDAL.ObterContatos(Convert.ToInt64(Session["CodUsuario"].ToString()));
            else
                ddlContato.DataSource = HeUCDAL.ObterContatos(0);


            ddlContato.DataTextField = "NM_CONTATO_EMAIL";
            ddlContato.DataValueField = "CD_INDEX";
            ddlContato.DataBind();
            ddlContato.Items.Insert(0, "*Nenhum Selecionado");
            /*--------------------------------------------------------------------------------------------------------*/

            ShowMessage("Contato Salvo", MessageType.Info);


        }
        protected void btnVoltarContato_Click(object sender, EventArgs e)
        {
            HabilEmailUsuarioContatoDAL HeUCDAL = new HabilEmailUsuarioContatoDAL();
            HabilEmailUsuarioContato HeUC = new HabilEmailUsuarioContato();
            /*--------------------------------------------------------------------------------------------------------*/
            pnlAlterar.Visible = true;
            pnlNovo.Visible = false;
            pnlVerificar.Visible = false;

            if (Session["CodUsuario"] != null)
                ddlContato.DataSource = HeUCDAL.ObterContatos(Convert.ToInt64(Session["CodUsuario"].ToString()));
            else
                ddlContato.DataSource = HeUCDAL.ObterContatos(0);


            ddlContato.DataTextField = "NM_CONTATO_EMAIL";
            ddlContato.DataValueField = "CD_INDEX";
            ddlContato.DataBind();
            ddlContato.Items.Insert(0, "*Nenhum Selecionado");
            /*--------------------------------------------------------------------------------------------------------*/

        }
        protected void CompactaDocumento()
        {

            HabilEmailCriado HEC = new HabilEmailCriado();

            string str = litDescricao.Text;
            string str2 = Server.HtmlDecode(str);
            if (txtCodigo.Text == "Novo")
                HEC.CD_INDEX = 0;
            else
                HEC.CD_INDEX = Convert.ToInt64(txtCodigo.Text);
            HEC.TX_ASSUNTO = txtAssunto.Text;
            HEC.CD_SITUACAO = Convert.ToInt64(ddlSituacao.SelectedValue.ToString());
            HEC.DT_LANCAMENTO = v.ObterDataHora();
            HEC.TX_CORPO = str2;
            HEC.listaDestinatarios = listaDestinatariosPara;
            HEC.listaDestinatariosComCopia = listaDestinatariosComCopia;
            HEC.listaDestinatariosComCopiaOculta = listaDestinatariosComCopiaOculta;
            HEC.listaAnexos = listaAnexos;

            Session["RascunhoEmail"] = str2;
            Session["GeracaoDosEmails"] = HEC;
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            HabilEmailCriado HEC = new HabilEmailCriado();
            HabilEmailCriadoDAL HECDAL = new HabilEmailCriadoDAL();



            HEC.DT_LANCAMENTO = v.ObterDataHora();

            HEC.listaDestinatarios = listaDestinatariosPara;
            HEC.listaDestinatariosComCopia = listaDestinatariosComCopia;
            HEC.listaDestinatariosComCopiaOculta = listaDestinatariosComCopiaOculta;
            HEC.listaAnexos = listaAnexos;

            HEC.TX_ASSUNTO = txtAssunto.Text;
            HEC.TX_CORPO = litDescricao.Text;
            HEC.CD_USU_REMETENTE = Convert.ToInt64(Session["CodUsuario"].ToString());
            HEC.IN_HTML = 1;

            if (txtCodigo.Text == "Novo")
                HEC.CD_INDEX = 0;
            else
                HEC.CD_INDEX = Convert.ToInt64(txtCodigo.Text);

            HECDAL.Grava_ColecaoEmail(HEC, true);

            txtCodigo.Text = HEC.CD_INDEX.ToString();

            Session["MensagemTela"] = "E-mail está sendo enviado!!!";
            btnVoltar_Click(sender, e);


        }

        protected void btnAnexo_Click(object sender, EventArgs e)
        {
            pnlNovo.Visible = false;
            pnlVerificar.Visible = false;
            pnlAlterar.Visible = false;
            pnlAnexos.Visible = true;
            grdAnexo.DataSource = listaAnexos;
            grdAnexo.DataBind();
            Session["LST_ANEXO"] = listaAnexos;
        }

        protected void btnEncaminhar_Click(object sender, EventArgs e)
        {
            if (ddlSituacao.SelectedValue != "110")
            {
                btnSalvar.Visible = true;
                BtnEditarDS.Visible = true;
                btnAnexo.Visible = true;
                btnAddContato.Visible = true;
                btnEnviar.Visible = true;
                blnContatos.Visible = true;
                btnEncaminhar.Visible = false;
                txtCodigo.Text = "Novo";
                txtDataLancamento.Text = "";
                txtDataEnvio.Text  = "";
                txtTentaEnvio.Text = "";
                ddlSituacao.SelectedValue = "110";
            }
        }

        protected void btnNovoAnexo_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            if (Request.QueryString["IN"] != null)
                Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=9&IN=" + Request.QueryString["IN"]);
            else
                Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=9");
        }

        protected void grdAnexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["ZoomDocCtaAnexo"] = HttpUtility.HtmlDecode(grdAnexo.SelectedRow.Cells[0].Text);
            if (Request.QueryString["IN"] != null)
                Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=9&IN=" + Request.QueryString["IN"]);
            else
                Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=9");
        }
    }
}