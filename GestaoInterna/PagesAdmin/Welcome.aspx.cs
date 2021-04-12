using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label pnlPainel = (Label)Master.FindControl("lblEmpresa");
            Panel pnlPainel2 = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            pnlPainel.Text = "Empresa: Hábil Informática";
            pnlPainel2.Visible = true;

            if ((Session["UsuSis"] == null) || (Session["CodUsuario"] == null))
            {
                Response.Redirect("~/Default.aspx");
            }

        }
    }
}