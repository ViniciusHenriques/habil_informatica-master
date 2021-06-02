using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class ConCtaReceber : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        string strMensagemR = "";
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaFiltro(object sender, EventArgs e)
        {
            int intI = 1;
            int intJ = 1;
            DBTabelaDAL d = new DBTabelaDAL();
            List<DBTabela> Lista = new List<DBTabela>();
            Lista = d.ListarCamposView("VW_DOC_CTA_RECEBER");

            PlanoContasDAL RnPlanoConta = new PlanoContasDAL();
            ddlPlanoConta.DataSource = RnPlanoConta.ListarPlanoContas("", "", "", "");
            ddlPlanoConta.DataTextField = "DescricaoPlanoConta";
            ddlPlanoConta.DataValueField = "CodigoPlanoConta";
            ddlPlanoConta.DataBind();
            ddlPlanoConta.Items.Insert(0, "..... SELECIONE UM PLANO DE CONTAS .....");

            TipoCobrancaDAL RnCobranca = new TipoCobrancaDAL();
            ddlTipoCobranca.DataSource = RnCobranca.ListarTipoCobrancas("", "", "", "");
            ddlTipoCobranca.DataTextField = "DescricaoTipoCobranca";
            ddlTipoCobranca.DataValueField = "CodigoTipoCobranca";
            ddlTipoCobranca.DataBind();
            ddlTipoCobranca.Items.Insert(0, "..... SELECIONE UM TIPO DE COBRANÇA .....");

            foreach (DBTabela Lst in Lista)
            {

                if ((Lst.Tipo.ToUpper() == "SMALLDATETIME") || (Lst.Tipo.ToUpper() == "DATETIME"))
                {
                    if (intJ == 1)
                    {
                        txtfiltrodata11.Focus();
                        pnlFiltroData1.Visible = true;
                        lblFiltroData1.Text = Lst.NomeComum.ToString();
                        lblFiltroData1.ToolTip = Lst.Coluna.ToString();
                        txtfiltrodata11.Text = "";
                        txtfiltrodata12.Text = "";
                        txtfiltrodata11.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtfiltrodata12.MaxLength = Convert.ToInt32(Lst.Tamanho);

                    }

                    if (intJ == 2)
                    {
                        pnlFiltroData2.Visible = true;
                        lblFiltroData2.Text = Lst.NomeComum.ToString();
                        lblFiltroData2.ToolTip = Lst.Coluna.ToString();
                        txtfiltrodata21.Text = "";
                        txtfiltrodata22.Text = "";
                        txtfiltrodata21.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtfiltrodata22.MaxLength = Convert.ToInt32(Lst.Tamanho);

                    }

                    if (intJ == 3)
                    {
                        pnlFiltroData3.Visible = true;
                        lblFiltroData3.Text = Lst.NomeComum.ToString();
                        lblFiltroData3.ToolTip = Lst.Coluna.ToString();
                        txtfiltrodata31.Text = "";
                        txtfiltrodata32.Text = "";
                        txtfiltrodata31.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtfiltrodata32.MaxLength = Convert.ToInt32(Lst.Tamanho);

                    }


                    intJ++;
                }
                else
                {
                    if (intI == 1)
                    {
                        txtFiltro11.Focus();
                        pnlFiltro1.Visible = true;
                        lblFiltro1.Text = Lst.NomeComum.ToString();
                        lblFiltro1.ToolTip = Lst.Coluna.ToString();
                        txtFiltro11.Text = "";
                        txtFiltro12.Text = "";
                        txtFiltro11.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro12.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }
                    if (intI == 2)
                    {
                        pnlFiltro2.Visible = true;
                        lblFiltro2.Text = Lst.NomeComum.ToString();
                        lblFiltro2.ToolTip = Lst.Coluna.ToString();
                        txtFiltro21.Text = "";
                        txtFiltro22.Text = "";
                        txtFiltro21.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro22.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }
                    if (intI == 3)
                    {
                        pnlFiltro3.Visible = true;
                        lblFiltro3.Text = Lst.NomeComum.ToString();
                        lblFiltro3.ToolTip = Lst.Coluna.ToString();
                        txtFiltro31.Text = "";
                        txtFiltro32.Text = "";
                        txtFiltro31.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro32.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }
                    if (intI == 4)
                    {
                        pnlFiltro4.Visible = true;
                        lblFiltro4.Text = Lst.NomeComum.ToString();
                        lblFiltro4.ToolTip = Lst.Coluna.ToString();
                        txtFiltro41.Text = "";
                        txtFiltro42.Text = "";
                        txtFiltro41.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro42.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }
                    if (intI == 5)
                    {
                        pnlFiltro5.Visible = true;
                        lblFiltro5.Text = Lst.NomeComum.ToString();
                        lblFiltro5.ToolTip = Lst.Coluna.ToString();
                        txtFiltro51.Text = "";
                        txtFiltro52.Text = "";
                        txtFiltro51.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro52.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }
                    if (intI == 6)
                    {
                        pnlFiltro6.Visible = true;
                        lblFiltro6.Text = Lst.NomeComum.ToString();
                        lblFiltro6.ToolTip = Lst.Coluna.ToString();
                        txtFiltro61.Text = "";
                        txtFiltro62.Text = "";
                        txtFiltro61.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro62.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }


                    if (intI == 7)
                    {
                        pnlFiltro7.Visible = true;
                        lblFiltro7.Text = Lst.NomeComum.ToString();
                        lblFiltro7.ToolTip = Lst.Coluna.ToString();
                        txtFiltro71.Text = "";
                        txtFiltro72.Text = "";
                        txtFiltro71.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro72.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }
                    if (intI == 8)
                    {
                        pnlFiltro8.Visible = true;
                        lblFiltro8.Text = Lst.NomeComum.ToString();
                        lblFiltro8.ToolTip = Lst.Coluna.ToString();
                        txtFiltro81.Text = "";
                        txtFiltro82.Text = "";
                        txtFiltro81.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro82.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }
                    if (intI == 9)
                    {
                        pnlFiltro9.Visible = true;
                        lblFiltro9.Text = Lst.NomeComum.ToString();
                        lblFiltro9.ToolTip = Lst.Coluna.ToString();
                        txtFiltro91.Text = "";
                        txtFiltro92.Text = "";
                        txtFiltro91.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro92.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }
                    if (intI == 10)
                    {
                        pnlFiltro10.Visible = true;
                        lblFiltro10.Text = Lst.NomeComum.ToString();
                        lblFiltro10.ToolTip = Lst.Coluna.ToString();
                        txtFiltro101.Text = "";
                        txtFiltro102.Text = "";
                        txtFiltro101.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtFiltro102.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    }


                    intI++;

                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocadaDocCtaPagar"] != null)
            {
                PanelSelect = Session["TabFocadaDocCtaPagar"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaDocCtaPagar"] = "home";
                txtFiltro11.Focus();
            }

            if (Session["LST_DOCCTARECEBER"] != null)
            {
                listaT = (List<DBTabelaCampos>)Session["LST_DOCCTARECEBER"];
                if (listaT.Count != 0)
                    btnConsultar_Click(sender, e);
            }


            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["IncEmpresa"] != null)
            {
                pnlPainel.Visible = false;
                cmdSair.Visible = false;
                btnSair.Visible = false;
                btnNovo.Visible = false;
                btnVoltar.Visible = true;
            }
            else
            {
                btnVoltar.Visible = false;
                pnlPainel.Visible = true;
                cmdSair.Visible = true;
                btnSair.Visible = true;
                btnNovo.Visible = true;
            }

            if (Session["ZoomDocCtaReceber2"] != null)
            {
                if (Session["ZoomDocCtaReceber2"].ToString() == "RELACIONAL")
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

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                MontaFiltro(sender, e);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConCtaReceber.aspx");
                lista.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoIncluir)
                            btnNovo.Visible = false;

                        if (!x.AcessoConsulta)
                        {
                            btnConsultar.Visible = false;
                            grdGrid.Enabled = false;
                        }

                        if (!x.AcessoRelatorio)
                            btnImprimir.Visible = false;

                    }
                });

                if (Session["ZoomDocCtaReceber2"] != null)
                {
                    //btnImprimir.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                    btnVoltar.Visible = false;

            }
            if (!lblFiltro1.Visible)
            {
                Response.Redirect("~/Pages/Welcome.aspx");
            }

        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {

            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = "";
            Boolean blnCampoValido = false;

            if (pnlFiltroData1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltroData1.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltroData1.Text, txtfiltrodata11.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata11.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltroData1.Text, txtfiltrodata12.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata12.Focus();
                        return;
                    }

                if ((txtfiltrodata11.Text != "") || (txtfiltrodata12.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroData1.ToolTip.ToString();
                    rowp.Inicio = txtfiltrodata11.Text;
                    rowp.Fim = txtfiltrodata12.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }
            if (pnlFiltroData2.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltroData2.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltroData2.Text, txtfiltrodata21.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata21.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltroData2.Text, txtfiltrodata22.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata22.Focus();
                        return;
                    }

                if ((txtfiltrodata21.Text != "") || (txtfiltrodata22.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroData2.ToolTip.ToString();
                    rowp.Inicio = txtfiltrodata21.Text;
                    rowp.Fim = txtfiltrodata22.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }
            if (pnlFiltroData3.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltroData3.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltroData3.Text, txtfiltrodata31.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata31.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltroData3.Text, txtfiltrodata32.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata32.Focus();
                        return;
                    }

                if ((txtfiltrodata31.Text != "") || (txtfiltrodata32.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroData3.ToolTip.ToString();
                    rowp.Inicio = txtfiltrodata31.Text;
                    rowp.Fim = txtfiltrodata32.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro1.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro1.Text, txtFiltro11.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro11.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro1.Text, txtFiltro12.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro12.Focus();
                        return;
                    }

                if ((txtFiltro11.Text != "") || (txtFiltro12.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro1.ToolTip.ToString();
                    rowp.Inicio = txtFiltro11.Text;
                    rowp.Fim = txtFiltro12.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro2.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro2.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro2.Text, txtFiltro21.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro21.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro2.Text, txtFiltro22.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro22.Focus();
                        return;
                    }

                if ((txtFiltro21.Text != "") || (txtFiltro22.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro2.ToolTip.ToString();
                    rowp.Inicio = txtFiltro21.Text;
                    rowp.Fim = txtFiltro22.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro3.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro3.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro3.Text, txtFiltro31.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro31.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro3.Text, txtFiltro32.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro32.Focus();
                        return;
                    }
                if ((txtFiltro31.Text != "") || (txtFiltro32.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro3.ToolTip.ToString();
                    rowp.Inicio = txtFiltro31.Text;
                    rowp.Fim = txtFiltro32.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro4.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro4.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro4.Text, txtFiltro41.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro41.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro4.Text, txtFiltro42.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro42.Focus();
                        return;
                    }
                if ((txtFiltro41.Text != "") || (txtFiltro42.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro4.ToolTip.ToString();
                    rowp.Inicio = txtFiltro41.Text;
                    rowp.Fim = txtFiltro42.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro5.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro5.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro5.Text, txtFiltro51.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro51.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro5.Text, txtFiltro52.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro52.Focus();
                        return;
                    }
                if ((txtFiltro51.Text != "") || (txtFiltro52.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro5.ToolTip.ToString();
                    rowp.Inicio = txtFiltro51.Text;
                    rowp.Fim = txtFiltro52.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro6.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro6.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro6.Text, txtFiltro61.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro61.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro6.Text, txtFiltro62.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro62.Focus();
                        return;
                    }
                if ((txtFiltro61.Text != "") || (txtFiltro62.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro6.ToolTip.ToString();
                    rowp.Inicio = txtFiltro61.Text;
                    rowp.Fim = txtFiltro62.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            if (pnlFiltro7.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro7.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro7.Text, txtFiltro71.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro71.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro7.Text, txtFiltro72.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro72.Focus();
                        return;
                    }
                if ((txtFiltro71.Text != "") || (txtFiltro72.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro7.ToolTip.ToString();
                    rowp.Inicio = txtFiltro71.Text;
                    rowp.Fim = txtFiltro72.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            if (pnlFiltro8.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro8.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro8.Text, txtFiltro81.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro81.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro8.Text, txtFiltro82.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro82.Focus();
                        return;
                    }
                if ((txtFiltro81.Text != "") || (txtFiltro82.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro8.ToolTip.ToString();
                    rowp.Inicio = txtFiltro81.Text;
                    rowp.Fim = txtFiltro82.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            if (pnlFiltro9.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro9.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro9.Text, txtFiltro91.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro91.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro9.Text, txtFiltro92.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro92.Focus();
                        return;
                    }
                if ((txtFiltro91.Text != "") || (txtFiltro92.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro9.ToolTip.ToString();
                    rowp.Inicio = txtFiltro91.Text;
                    rowp.Fim = txtFiltro92.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            if (pnlFiltro10.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_CTA_RECEBER", lblFiltro10.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro10.Text, txtFiltro101.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro101.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro10.Text, txtFiltro102.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro102.Focus();
                        return;
                    }
                if ((txtFiltro101.Text != "") || (txtFiltro62.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro10.ToolTip.ToString();
                    rowp.Inicio = txtFiltro101.Text;
                    rowp.Fim = txtFiltro102.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            PanelSelect = "consulta";
            
            Session["LST_DOCCTARECEBER"] = listaT;
            Doc_CtaReceberDAL r = new Doc_CtaReceberDAL();

            int TipoBaixa = 0;
            int TpCobranca = 0;
            int CodPlanoConta = 0;
            if (Convert.ToInt32(ddlTipoBaixa.SelectedValue) == 1)
            {
                TipoBaixa = 31;
            }
            else if (Convert.ToInt32(ddlTipoBaixa.SelectedValue) == 2)
            {
                TipoBaixa = 36;
            }
            else if (Convert.ToInt32(ddlTipoBaixa.SelectedValue) == 3)
            {
                TipoBaixa = 38;
            }

            if (ddlPlanoConta.SelectedValue != "..... SELECIONE UM PLANO DE CONTAS ....." && ddlPlanoConta.SelectedValue != "")
                CodPlanoConta = Convert.ToInt32(ddlPlanoConta.SelectedValue);

            if (ddlTipoCobranca.SelectedValue != "..... SELECIONE UM TIPO DE COBRANÇA ....." && ddlTipoCobranca.SelectedValue != "")
                TpCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);



            //-------------------cabeçalho do relatorio---------------------------
            string StrTipoCobranca = "";
            string StrPlanoConta = "";
            string StrTipoBaixa = "";
            if (Convert.ToInt32(ddlTipoBaixa.SelectedValue) == 1)
            {
                StrTipoBaixa = "ABERTO";
            }
            else if (Convert.ToInt32(ddlTipoBaixa.SelectedValue) == 2)
            {
                StrTipoBaixa = "BAIXADO";
            }
            else if (Convert.ToInt32(ddlTipoBaixa.SelectedValue) == 3)
            {
                StrTipoBaixa = "CANCELADO";
            }
            else
            {
                StrTipoBaixa = "TODOS";
            }

            if (ddlPlanoConta.SelectedValue == "..... SELECIONE UM PLANO DE CONTAS ....." || ddlPlanoConta.SelectedValue == "")
                StrPlanoConta = "TODOS";
            else
                StrPlanoConta = ddlPlanoConta.SelectedItem.Text;


            if (ddlTipoCobranca.SelectedValue == "..... SELECIONE UM TIPO DE COBRANÇA ....." || ddlTipoCobranca.SelectedValue == "")
                StrTipoCobranca = "TODOS";
            else
                StrTipoCobranca = ddlTipoCobranca.SelectedItem.Text;


            List<string> filtros = new List<string>();
            filtros.Add(StrPlanoConta);
            filtros.Add(StrTipoCobranca);        
            filtros.Add(StrTipoBaixa);
            filtros.Add(StrTipoBaixa);
            Session["FILTROS"] = filtros;

            List<int> Codigofiltros = new List<int>();
            Codigofiltros.Add(TpCobranca);
            Codigofiltros.Add(CodPlanoConta);
            Codigofiltros.Add(TipoBaixa);
            Session["COD_FILTROS"] = Codigofiltros;

            grdGrid.DataSource = r.ListarDoc_CtaReceberCompleto(listaT, TipoBaixa, TpCobranca, CodPlanoConta);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Contas a Receber não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
                Session["LST_DOCCTARECEBER"] = null;
            }


        }

        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

            Session["ZoomDocCtaReceber"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/Pages/Financeiros/ManCtaReceber.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["Ssn_CtaReceber"] = null;
            Session["ZoomDocCtaReceber"] = null;
            Session["CadDocCtaReceber"] = null;
            Session["Ssn_CtaReceber"] = null;
            Session["CadDocCtaReceber"] = null;
            Session["Ssn_TipoPessoa"] = null;
            Session["NovaBaixa"] = null;
            Session["CadDocCtaPagar"] = null;
            Session["Eventos"] = null;
            Session["Logs"] = null;
            Session["NovoAnexo"] = null;
            Session["Ssn_TipoPessoa"] = null;
            Session["Ssn_ctaReceber"] = null;
            Session["NotaFiscalServico"] = null;
            Session["Doc_orcamento"] = null;
            Session["ListaItemDocumento"] = null;
            Session["NovaBaixa"] = null;
            Session["IndicadorURL"] = null;
            Session["CadOrcamento"] = null;
            Session["ListaOutrosOrcamentos"] = null;
            Session["ListaContaReceber"] = null;
            Response.Redirect("~/Pages/Financeiros/ManCtaReceber.aspx");
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdGrid.PageSize = Convert.ToInt32(ddlRegistros.SelectedValue.ToString());
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void btnMensagem_Click(object sender, EventArgs e)
        {

        }
        protected void btnCfmNao_Click(object sender, EventArgs e)
        {
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            //    Response.Redirect("~/Pages/Acessos/CadComprador.aspx");
            //else
            //    Response.Redirect("~/Pages/Empresas/CadEmpresa.aspx");

        }
        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            Session["LST_DOCCTARECEBER"] = null;
            PanelSelect = "home";
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Financeiros/RelCtaReceber.aspx");
        }
        protected void grdGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                List<Permissao> lista1 = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista1 = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConCtaReceber.aspx");

                Session["Doc_orcamento"] = null;
                Session["Doc_Pedido"] = null;

                string x = e.CommandName;

                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = grdGrid.Rows[index];
                string Codigo = Server.HtmlDecode(row.Cells[0].Text);
                Session["ZoomDocCtaReceber"] = Codigo + "³";

                if (x == "Editar")
                {
                    if (lista1.Count > 0 && !lista1[0].AcessoCompleto)
                    {
                        if (!lista1[0].AcessoAlterar)
                        {
                            ShowMessage("Sem permissão para editar contas a receber", MessageType.Info);
                            return;
                        }
                    }
                    Session["Ssn_CtaReceber"] = null;
                    Session["CadDocCtaReceber"] = null;
                    Session["Ssn_TipoPessoa"] = null;
                    Session["NovaBaixa"] = null;
                    Session["CadDocCtaPagar"] = null;
                    Session["Eventos"] = null;
                    Session["Logs"] = null;
                    Session["NovoAnexo"] = null;
                    Session["Ssn_TipoPessoa"] = null;
                    Session["Ssn_ctaReceber"] = null;
                    Session["NotaFiscalServico"] = null;
                    Session["Doc_orcamento"] = null;
                    Session["ListaItemDocumento"] = null;
                    Session["NovaBaixa"] = null;
                    Session["IndicadorURL"] = null;
                    Session["CadOrcamento"] = null;
                    Session["ListaOutrosOrcamentos"] = null;
                    Session["ListaContaReceber"] = null;
                    Response.Redirect("~/Pages/Financeiros/ManCtaReceber.aspx");

                }
                else if (x == "Duplicar")
                {
                    if (lista1.Count > 0 && !lista1[0].AcessoCompleto)
                    {
                        if (!lista1[0].AcessoIncluir)
                        {
                            ShowMessage("Sem permissão para incluir contas a receber", MessageType.Info);
                            return;
                        }
                    }

                    Doc_CtaPagar p1 = new Doc_CtaPagar();

                    Doc_CtaPagarDAL ctaDAL = new Doc_CtaPagarDAL();

                    p1 = ctaDAL.PesquisarDocumento(Convert.ToDecimal(Codigo.Split('³')[0]));

                    txtPrefixo.Text = p1.CodigoPessoa.ToString();

                    OpDuplicar_CommandRow(sender, e);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Info);
            }
        }

        protected void OpDuplicar_CommandRow(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "MostrarModalDuplicar();", true);

        }
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtQtdDuplicar.Text == "99999")
                txtQtdDuplicar.Text = "99999";
            else
                txtQtdDuplicar.Text = (Convert.ToInt32(txtQtdDuplicar.Text) + 1).ToString();
        }

        protected void txtQtdDuplicar_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtQtdDuplicar.Text.Equals(""))
            {
                txtQtdDuplicar.Text = "1";
            }
            else
            {
                v.CampoValido("Qtd duplicar", txtQtdDuplicar.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtQtdDuplicar.Focus();
                }
                else
                    txtQtdDuplicar.Text = "1";

            }
            if (Convert.ToInt32(txtQtdDuplicar.Text) <= 0)
                txtQtdDuplicar.Text = "1";

            txtQtdDias.Focus();
        }

        protected void btnConfirmarDuplicar_Click(object sender, EventArgs e)
        {
            try
            {
                int QtdDuplicagem = Convert.ToInt32(txtQtdDuplicar.Text);

                PanelSelect = "consulta";

                if (Session["ZoomDocCtaReceber"] != null)
                {
                    decimal CodigoDocumento = Convert.ToDecimal(Session["ZoomDocCtaReceber"].ToString().Split('³')[0]);

                    Session["ZoomDocCtaReceber"] = null;

                    Doc_CtaReceber p1 = new Doc_CtaReceber();

                    Doc_CtaReceberDAL ctaDAL = new Doc_CtaReceberDAL();

                    List<BaixaDocumento> ListaBaixa = new List<BaixaDocumento>();

                    List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();

                    List<Habil_Log> ListaLog = new List<Habil_Log>();
                    
                    List<EventoDocumento> ListaEvento = new List<EventoDocumento>();

                    p1 = ctaDAL.PesquisarDocumento(CodigoDocumento);

                    ctaDAL.AtualizarContaPagarCampo("DG_DOCUMENTO", txtPrefixo.Text + " - 1/" + (QtdDuplicagem + 1), CodigoDocumento);

                    string teste = p1.DataVencimento.AddDays(Convert.ToInt32(txtQtdDuplicar.Text) * Convert.ToInt32(txtQtdDias.Text)).ToString();

                    if (((p1.DataVencimento.AddDays(Convert.ToInt32(txtQtdDuplicar.Text) * Convert.ToInt32(txtQtdDias.Text)) > Convert.ToDateTime("06-06-2079 00:00:00"))))
                    {
                        ShowMessage("Data máxima permitida foi ultrapassada, digite menos parcelas ou menos dias entre elas", MessageType.Info);
                        return;
                    }

                    DateTime DataParcelaAnterior = p1.DataVencimento;

                    Habil_Estacao he = new Habil_Estacao();
                    Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                    he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
                    p1.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
                    p1.Cpl_Usuario = Convert.ToInt32(Session["CodPflUsuario"]);
                    p1.DGSRDocumento = "";
                    p1.CodigoTipoDocumento = 3;
                    p1.CodigoSituacao = 31;

                    AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
                    ListaAnexo = anexo.ObterAnexos(CodigoDocumento);

                    for (int i = 0; i < QtdDuplicagem; i++)
                    {                      
                        p1.DataVencimento = DataParcelaAnterior.AddDays(Convert.ToInt32(txtQtdDias.Text));
                        DataParcelaAnterior = p1.DataVencimento;

                        p1.DGDocumento = txtPrefixo.Text + " - " + (i + 2) + "/" + (QtdDuplicagem + 1);
                        EventoDocumento evento = new EventoDocumento(1,31,
                                                                DateTime.Today,
                                                                he.CodigoEstacao,
                                                                Convert.ToInt32(Session["CodUsuario"]));

                        ctaDAL.Inserir(p1, ListaBaixa, evento, ListaAnexo);
                    }

                    btnConsultar_Click(sender, e);

                    ShowMessage("Documento duplicado com sucesso", MessageType.Info);
                }
            }           
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Info);
            }
        }

        protected void btnRemover_Click(object sender, EventArgs e)
        {
            if ((Convert.ToInt32(txtQtdDuplicar.Text) - 1) <= 0)
                txtQtdDuplicar.Text = "1";
            else
                txtQtdDuplicar.Text = (Convert.ToInt32(txtQtdDuplicar.Text) - 1).ToString();
        }

        protected void btnRemoverDia_Click(object sender, EventArgs e)
        {
            if ((Convert.ToInt32(txtQtdDias.Text) - 1) <= 0)
                txtQtdDias.Text = "1";
            else
                txtQtdDias.Text = (Convert.ToInt32(txtQtdDias.Text) - 1).ToString();
        }

        protected void btnAddDia_Click(object sender, EventArgs e)
        {
            if (txtQtdDias.Text == "99999")
                txtQtdDias.Text = "99999";
            else
                txtQtdDias.Text = (Convert.ToInt32(txtQtdDias.Text) + 1).ToString();
            
        }

        protected void txtQtdDias_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtQtdDias.Text.Equals(""))
            {
                txtQtdDias.Text = "1";
            }
            else
            {
                v.CampoValido("Qtd dias", txtQtdDias.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtQtdDias.Focus();
                }
                else
                    txtQtdDias.Text = "1";

            }
            if (Convert.ToInt32(txtQtdDias.Text) <= 0)
                txtQtdDias.Text = "1";
        }
    }
}