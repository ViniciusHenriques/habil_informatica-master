using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Persistence;
using DAL.Model;

namespace GestaoInterna
{
    public partial class SistemaEmManut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnEntrar_Click(object sender, EventArgs e)
        {
           Response.Redirect("~/Default.aspx");
        }
    }
}