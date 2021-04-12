using System;
using System.Web;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class ConModSistema : System.Web.UI.Page
    {
        clsMensagem cls = new clsMensagem();

        protected void MontaSelecionar(object sender, EventArgs e)
        {

            DBTabelaDAL d = new DBTabelaDAL();

            ddlPesquisa.DataSource = d.ListarCamposSQL("MODULO_DO_SISTEMA");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();

            cboSelecionar(sender,e);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MensagemTela"] != null)
            {
                Response.Write(cls.ExibeMensagem(Session["MensagemTela"].ToString()));
                Session["MensagemTela"] = null;
            }

            try
            {
                if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (!IsPostBack)
                {
                    MontaSelecionar(sender, e);
                    ModuloSistemaDAL r = new ModuloSistemaDAL();
                    grdModSistema.DataSource = r.ListarModulosSistema("", "", "", "");
                    grdModSistema.DataBind();
                    if (grdModSistema.Rows.Count == 0)
                        Response.Write(cls.ExibeMensagem("Módulos do Sistema Não Encontrados"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
 
        }


        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            ModuloSistemaDAL r = new ModuloSistemaDAL();
            DBTabelaDAL d = new DBTabelaDAL();

            String strTipo = d.ListarTipoCampoSQL("MODULO_DO_SISTEMA", ddlPesquisa.SelectedValue).ToString().ToUpper();
            string strValor = txtVarchar.Text + txtInt.Text + txtValor.Text;
            grdModSistema.DataSource = r.ListarModulosSistema(ddlPesquisa.Text, strTipo, strValor, ddlPesquisa.Text);
            grdModSistema.DataBind();
            if (grdModSistema.Rows.Count == 0)
                Response.Write(cls.ExibeMensagem("Módulos do Sistema Não Encontrados"));
        }

        protected void cboSelecionar(object sender, EventArgs e)
        {
            try
            {
                txtVarchar.Visible = false;
                txtInt.Visible = false;
                txtValor.Visible = false;
                DBTabelaDAL d = new DBTabelaDAL();
                String strTipo = d.ListarTipoCampoSQL("MODULO_DO_SISTEMA", ddlPesquisa.SelectedValue).ToString().ToUpper();
                switch (strTipo)
                {
                    case "VARCHAR":
                        txtVarchar.Visible = true;
                        txtVarchar.Text = "";
                        txtVarchar.Focus();
                        break;
                    case "NVARCHAR":
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
                    default:
                        txtVarchar.Visible = true;
                        txtVarchar.Text = "";
                        txtVarchar.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void grdModSistema_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomModSistema"] = HttpUtility.HtmlDecode(grdModSistema.SelectedRow.Cells[0].Text) + "³" + HttpUtility.HtmlDecode(grdModSistema.SelectedRow.Cells[1].Text);
            Response.Redirect("~/PagesAdmin/CadModSistema.aspx");
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomModSistema"] = null;
            Response.Redirect("~/PagesAdmin/CadModSistema.aspx");
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Relatorios/RelModSistema.aspx");
        }

        protected void grdModSistema_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdModSistema.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }

        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdModSistema.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "";
            Response.Redirect("~/PagesAdmin/WelCome.aspx");
            this.Dispose();
        }
    }
}