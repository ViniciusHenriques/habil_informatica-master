using System;
using System.Web;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class ConMenu : System.Web.UI.Page
    {
        clsMensagem cls = new clsMensagem();
        protected void MontaSelecionar(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            ddlPesquisa.DataSource = d.ListarCamposSQL("MENU_DO_SISTEMA");
            ddlPesquisa.DataTextField = "Coluna";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
            cboSelecionar(sender, e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
                {
                    Session["ZoomModSistema"] = null;
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                    MontaSelecionar(sender, e);

                    MenuSistemaDAL r = new MenuSistemaDAL();
                    grdMenuSistema.DataSource = r.ListarMenuSistema("", "", "", "");
                    grdMenuSistema.DataBind();
                    if (grdMenuSistema.Rows.Count == 0)
                            Response.Write(cls.ExibeMensagem("Menu do Sistema Não Encontrado"));
                }

                if (Session["MensagemTela"] != null)
                {
                    Response.Write(cls.ExibeMensagem(Session["MensagemTela"].ToString()));
                    Session["MensagemTela"] = null;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            MenuSistemaDAL r = new MenuSistemaDAL();
            DBTabelaDAL d = new DBTabelaDAL();

            String strTipo = d.ListarTipoCampoSQL("MENU_DO_SISTEMA", ddlPesquisa.SelectedValue).ToString().ToUpper();
            string strValor = txtVarchar.Text + txtInt.Text + txtValor.Text;
            grdMenuSistema.DataSource = r.ListarMenuSistema(ddlPesquisa.Text, strTipo, strValor, ddlPesquisa.Text);
            grdMenuSistema.DataBind();
            if (grdMenuSistema.Rows.Count == 0)
                 Response.Write(cls.ExibeMensagem("Menu do Sistema Não Encontrado"));
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            try
            {

                txtVarchar.Visible = false;
                txtInt.Visible = false;
                txtValor.Visible = false;

                DBTabelaDAL d = new DBTabelaDAL();
                String strTipo = d.ListarTipoCampoSQL("MENU_DO_SISTEMA", ddlPesquisa.SelectedValue).ToString().ToUpper();

                switch (strTipo)
                {
                    case "VARCHAR":
                        txtVarchar.Visible = true;
                        txtVarchar.Text = "";
                        txtVarchar.Focus();
                        break;

                    case "INT":
                        txtInt.Visible = true;
                        txtInt.Text = "";
                        txtInt.Focus();
                        break;

                    case "NUMERIC":
                        txtValor.Visible = true;
                        txtValor.Text = "";
                        txtValor.Focus();
                        break;

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void grdMenuSistema_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomMenu"] = HttpUtility.HtmlDecode(grdMenuSistema.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/PagesAdmin/CadMenu.aspx");
        }
        protected void grdMenuSistema_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdMenuSistema.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdMenuSistema.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Session["ZoomModSistema"] = null;
            Session["ZoomMenu"] = null;
            Response.Redirect("~/PagesAdmin/WelCome.aspx");
            this.Dispose();
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomMenu"] = null;
            Response.Redirect("~/PagesAdmin/CadMenu.aspx");

        }

    }
}