using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Empresas
{
    public partial class ConParSistema : System.Web.UI.Page
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
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }



            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) )
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                    MontaSelecionar(sender, e);

                    ParSistemaDAL r = new ParSistemaDAL();
                    grdGrid.DataSource = r.ListarParSistemas("", "", "", "");
                    grdGrid.DataBind();
                    if (grdGrid.Rows.Count == 0)
                        ShowMessage("Não existem Empresas Cadastrados.",MessageType.Info);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConParSistema.aspx");
                lista.ForEach(delegate(Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoIncluir)
                            btnNovo.Visible = false;

                        if (!x.AcessoConsulta)
                        {
                            grdGrid.Enabled = false;
                        }

                    }
                });

                if (Session["ZoomParSistema2"] != null)
                {
                    //btnImprimir.Visible = false;
                    btnSair.Visible = false;  
                }
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            ParSistemaDAL r = new ParSistemaDAL();
            grdGrid.DataSource = r.ListarParSistemas("","", "", "");
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ShowMessage("Empresa(s) não encontrada(s) mediante a pesquisa realizada.", MessageType.Info);
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomParSistema"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/Pages/Empresas/CadParSistema.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomParSistema"] = null;
            Session["CadParSistema"] = null;
            Response.Redirect("~/Pages/Empresas/CadParSistema.aspx");
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "/Pages/Welcome.aspx";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();

        }

    }
}