using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Transporte
{
    public partial class ConRegFrete : System.Web.UI.Page
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
            ddlPesquisa.DataSource = d.ListarCamposView("VW_REGRA_DE_FRETE");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["ListaCidadesRegra"] = null;
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


            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                MontaSelecionar(sender, e);

                RegraFreteDAL r = new RegraFreteDAL();
                grdGrid.DataSource = r.ListarRegraFrete("", "", "", "");
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                    ShowMessage("Não existem Regras de frete cadastrados.", MessageType.Info);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConRegFrete.aspx");
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
                
                if (Session["ZoomRegFrete2"] != null)
                {
                    btnSair.Visible = false;
                }
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = d.ListarTipoCampoView("VW_REGRA_DE_FRETE", ddlPesquisa.SelectedValue.ToString().ToUpper());

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
            RegraFreteDAL r = new RegraFreteDAL();
            grdGrid.DataSource = r.ListarRegraFrete(ddlPesquisa.Text, strTipo.ToString().ToUpper(), txtVarchar.Text, ddlPesquisa.Text);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ShowMessage("Regras(s) não encontrada(s) mediante a pesquisa realizada.", MessageType.Info);
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text = "";
            txtVarchar.Focus();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomRegFrete"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";

            Response.Redirect("~/Pages/Transporte/ManRegFrete.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomRegFrete"] = null;
            Session["ListaCidadesRegra"] = null;
            Response.Redirect("~/Pages/Transporte/ManRegFrete.aspx");
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados
            grdGrid.DataBind();
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
    }
}
