using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Pessoas
{
    public partial class ConVendedor : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaSelecionar(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            ddlPesquisa.DataSource = d.ListarCamposSQL("Vendedor");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtVarchar.Focus();

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["IncVendedor"] != null)
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



            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                    MontaSelecionar(sender, e);

                    VendedorDAL r = new VendedorDAL();
                    grdGrid.DataSource = r.ListarVendedores("", "", "", "");
                    grdGrid.DataBind();
                    if (grdGrid.Rows.Count == 0)
                        ShowMessage("Não existem Vendedores Cadastrados.",MessageType.Info);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConVendedor.aspx");
                lista.ForEach(delegate(Permissao x)
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

                if (Session["ZoomPessoa2"] != null)
                {
                    //btnImprimir.Visible = false;
                    btnSair.Visible = false;  
                }
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = d.ListarTipoCampoSQL("VENDEDOR", ddlPesquisa.SelectedValue).ToString().ToUpper();

            Boolean blnCampoValido = false;
            v.CampoValido(ddlPesquisa.SelectedValue.ToString().ToUpper(), txtVarchar.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtVarchar.Focus();
                    return;
                }
            }
            VendedorDAL r = new VendedorDAL();
            grdGrid.DataSource = r.ListarVendedores(ddlPesquisa.Text, strTipo, txtVarchar.Text , ddlPesquisa.Text);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ShowMessage("Vendedor(es) não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text = "";
            txtVarchar.Focus();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Session["IncVendedor"] != null)
            {
                Session["IncVendedor2"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³" + HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[1].Text);
                Response.Redirect("~/Pages/Pessoas/CadVendedor.aspx");
            }
            else
            {
                Session["ZoomVendedor"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
                Response.Redirect("~/Pages/Pessoas/CadVendedor.aspx");
            }
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomVendedor"] = null;
            Session["CadVendedor"] = null;
            Response.Redirect("~/Pages/Pessoas/CadVendedor.aspx");
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
            Response.Redirect("~/Pages/Pessoas/CadVendedor.aspx");

        }

    }
}