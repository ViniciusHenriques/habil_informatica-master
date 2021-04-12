using System;
using System.Web;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class ConTiposSituacao : System.Web.UI.Page  
    {
        clsMensagem cls = new clsMensagem();

        protected void MontaSelecionar(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            ddlPesquisa.DataSource = d.ListarCamposSQL("HABIL_TIPO");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();

            cboSelecionar(sender, e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["MensagemTela"] != null)
            {
                Response.Write(cls.ExibeMensagem(Session["MensagemTela"].ToString()));
                Session["MensagemTela"] = null;
            }

            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath; 
                if (!IsPostBack)
                {
                    MontaSelecionar(sender, e);

                    Habil_TipoDAL r = new Habil_TipoDAL();
                    grdSituacao.DataSource = r.ListarHabilTipos("", "", "", "");
                    grdSituacao.DataBind();
                    if (grdSituacao.Rows.Count == 0)
                        Response.Write(cls.ExibeMensagem("Situações e Tipos Não Encontrados"));
                }
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            string strTipo = d.ListarTipoCampoSQL("HABIL_TIPO", ddlPesquisa.SelectedValue).ToString().ToUpper();
            string strValor = txtVarchar.Text + txtInt.Text + txtValor.Text;
            Habil_TipoDAL r = new Habil_TipoDAL();
            grdSituacao.DataSource = r.ListarHabilTipos(ddlPesquisa.Text, strTipo, strValor, ddlPesquisa.Text);
            grdSituacao.DataBind();
            if (grdSituacao.Rows.Count == 0)
                 Response.Write(cls.ExibeMensagem("Situações e Tipos Não Encontrados"));
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Visible = false;
            txtInt.Visible = false;
            txtValor.Visible = false;

            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = d.ListarTipoCampoSQL("HABIL_TIPO", ddlPesquisa.SelectedValue).ToString().ToUpper();

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
        protected void grdSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomHabilTipo"] = HttpUtility.HtmlDecode(grdSituacao.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/PagesAdmin/CadTiposSituacao.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomHabilTipo"] = null;
            Response.Redirect("~/PagesAdmin/CadTiposSituacao.aspx");
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Relatorios/RelTiposSituacao.aspx");

        }
        protected void grdSituacao_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdSituacao.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/PagesAdmin/WelCome.aspx");
            this.Dispose();
        }
    }
}