using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace GestaoInterna
{
    public partial class MasterPrincipal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UsuSis"] == null)
            {
                return;
            }

            if ((Session["UsuSis"].ToString() != "ENTRADA INICIAL DO SISTEMA") && (Session["CodUsuario"].ToString() != "0"))
            {
                Menu1.Visible = false;
                pnlAuttenticado.Visible = false;
                cttCorpo.Visible = false;

                Response.Redirect("~/Default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    Menu1.Dispose();

                    DataTable dt = new DataTable();
                    DataTable dt2 = new DataTable();

                    dt = GetDataAdmin(0);
                    PopulateMenuAdmin(dt, 0, null);

                }
                if (Session["MenuAberto"].ToString() == "NAO")
                {
                    Session["Pagina"] = "Inicial";
                    Session["MenuAberto"] = "SIM";
                }

                Menu1.Visible = true;
                pnlAuttenticado.Visible = true;
                cttCorpo.Visible = true;
            }
        }

        private DataTable GetDataAdmin(int parentMenuId)
        {
            string query = "SELECT [MenuId], [Title], [Description], [Url], Icone  FROM [MENUADMIN] WHERE ParentMenuId = @ParentMenuId ORDER BY [MenuId]";
            string constr = @ConfigurationManager.ConnectionStrings["HabilInfoCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@ParentMenuId", parentMenuId);
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
                return dt;
            }
        }
        private void PopulateMenuAdmin(DataTable dt, int parentMenuId, MenuItem parentMenuItem)
        {
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
            foreach (DataRow row in dt.Rows)
            {
                MenuItem menuItem = new MenuItem
                {
                    Value = row["MenuId"].ToString(),
                    Text = "&nbsp;&nbsp;" +  row["Title"].ToString(),
                    NavigateUrl = row["Url"].ToString(),
                    Selected = row["Url"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase),
                    ImageUrl = row["Icone"].ToString()


                };
            
                if (parentMenuId == 0)
                {
                    Menu1.Items.Add(menuItem);
                    DataTable dtChild = this.GetDataAdmin(int.Parse(menuItem.Value));
                    PopulateMenuAdmin(dtChild, int.Parse(menuItem.Value), menuItem);
                }
                else
                {
                    parentMenuItem.ChildItems.Add(menuItem);
                    DataTable dtChild2 = this.GetDataAdmin(int.Parse(menuItem.Value));
                    PopulateMenuAdmin(dtChild2, int.Parse(menuItem.Value), menuItem);
                }
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

       

    }
}