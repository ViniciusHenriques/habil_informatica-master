using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class ConContaCorrente : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaFiltro(object sender, EventArgs e)
        {
            int intI = 1;
            DBTabelaDAL d = new DBTabelaDAL();
            List<DBTabela> Lista = new List<DBTabela>();
            Lista = d.ListarCamposSQL("CONTA_CORRENTE");

            foreach (DBTabela Lst in Lista)
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
                intI++;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocada"] != null)
                PanelSelect = Session["TabFocada"].ToString();
            else
                if (!IsPostBack)
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

            txtFiltro11.Focus();



            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["LST_CONTACORRENTE"] != null)
            {
                listaT = (List<DBTabelaCampos>)Session["LST_CONTACORRENTE"];
                if (listaT.Count != 0)
                    btnConsultar_Click(sender, e);
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                MontaFiltro(sender, e);


                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConCtaCorrente.aspx");
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

                    }
                });

                if (Session["ZoomContaCorrente"] != null)
                {
                    btnSair.Visible = false;
                }
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

            if (pnlFiltro1.Visible)
            {
                strTipo = d.ListarTipoCampoSQL("CONTA_CORRENTE", lblFiltro1.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoSQL("CONTA_CORRENTE", lblFiltro2.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoSQL("CONTA_CORRENTE", lblFiltro3.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoSQL("CONTA_CORRENTE", lblFiltro4.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoSQL("CONTA_CORRENTE", lblFiltro5.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoSQL("CONTA_CORRENTE", lblFiltro6.ToolTip).ToString().ToUpper();

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

            PanelSelect = "consulta";
            Session["LST_CONTACORRENTE"] = listaT;
            ContaCorrenteDAL r = new ContaCorrenteDAL();
            grdGrid.DataSource = r.ListarContaCorrenteCompleto(listaT);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Conta(s) corrente não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
                Session["LST_CONTACORRENTE"] = null;
            }
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomContaCorrente2"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/Pages/financeiros/cadCtaCorrente.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomContaCorrente2"] = null;
            Response.Redirect("~/Pages/financeiros/cadCtaCorrente.aspx");
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
            grdGrid.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["LST_CONTACORRENTE"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            PanelSelect = "home";
            Session["LST_CONTACORRENTE"] = null;
        }
    }
}
