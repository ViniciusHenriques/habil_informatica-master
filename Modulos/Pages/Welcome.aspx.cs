using System;
using System.Web;
using System.Web.UI.WebControls;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages
{
    public partial class Welcome : System.Web.UI.Page
    {
        Panel pnlPainelCentral = new Panel();
        Label lblPainelCentral = new Label();

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlPainelCentral = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            lblPainelCentral = (Label)Master.FindControl("lblEmpresa");

            lblPainelCentral.Text = "Empresa: Não Identificada a Empresa";
            
            pnlSelecao.Visible = false;
            pnlFigura.Visible = false;
            pnlPainelCentral.Visible = false;
            
            Session["Pagina"] = Request.CurrentExecutionFilePath;
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["CodUsuario"].ToString() != "-150380")
            {
                if ((Session["CodEmpresa"] == null) && (Session["CodUsuario"] != null))
                {
                    UsuarioDAL U = new UsuarioDAL();
                    EmpresaDAL E = new EmpresaDAL();
                    int intQtas = 0;
                    Session["CodEmpresa"] = Convert.ToInt64(U.EmpresaDoLogin(Session["CodUsuario"].ToString(), out intQtas));
                    Session["QtasEmpresas"] = intQtas;
                    if (intQtas == 1)
                    {
                        Session["NomeEmpresa"] = E.RetornaNomeFantasia(Convert.ToInt32(Session["CodEmpresa"]));
                        pnlFigura.Visible = true;
                    }
                    else
                    {
                        pnlSelecao.Visible = true;
                        EmpresaDAL r = new EmpresaDAL();
                        grdGrid.DataSource = r.ListarEmpresas("", "", "", "");
                        grdGrid.DataBind();
                    }

                }
                if (Session["NomeEmpresa"] != null)
                {
                    lblPainelCentral.Text = "Empresa: " + Session["CodEmpresa"].ToString() + " - " + Session["NomeEmpresa"].ToString();
                    pnlFigura.Visible = true;
                    pnlPainelCentral.Visible = true;
                }

            }
            else
            {
                pnlPainelCentral.Visible = true;
            }
        }

        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CodEmpresa"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text);
            Session["NomeEmpresa"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[2].Text);
            lblPainelCentral.Text = "Empresa: " + Session["CodEmpresa"].ToString() + " - " + Session["NomeEmpresa"].ToString();
            pnlPainelCentral.Visible = true;
            lblPainelCentral.Visible = true;
        }

    }
}