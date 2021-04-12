using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DAL.Persistence;
using DAL.Model;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using System.Web.Services;
using System.Linq;
using System.Web;
using System.Web.Caching;
namespace HabilInformatica
{
    public partial class MasterFornecedor : System.Web.UI.MasterPage
    {
        public string TamanhoDivCorpo { get; set; } 
        public string TamanhoDivMenu { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CodModulo"] = "15";

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
            {
                Menu1.Visible = false;
                pnlAuttenticado.Visible = false;
                cttCorpo.Visible = false;
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                Menu1.Dispose();
                Menu1.Visible = true;

                DataTable dt2 = new DataTable();

                if (Session["CodUsuario"] != null)
                {
                    dt2 = this.GetData(Convert.ToInt32(Session["CodModulo"]), 0, Convert.ToInt32(Session["CodPflUsuario"].ToString()));
                    MontaMenu(Convert.ToInt32(Session["CodModulo"]), dt2,  0,  null, Convert.ToInt32(Session["CodPflUsuario"].ToString()));
                }

                if(Session["MenuAberto"] != null)
                    if (Session["MenuAberto"].ToString() == "NAO")
                    {
                        Session["MenuAberto"] = "SIM";
                    }
                                        
                pnlAuttenticado.Visible = true;
                cttCorpo.Visible = true;


                lblUsuario.Text = Session["UsuSis"].ToString();
                lblModulo.Text = "Você está Logado no Módulo: Cotação de Preços para Fornecedores";
                if (Session["NomeEmpresa"] != null)
                    lblEmpresa.Text = "Empresa: " + Session["CodEmpresa"].ToString() + " - " + Session["NomeEmpresa"].ToString();


                string CaminhoFotoUsuario = @"\Scripts\CapturaWebcamASPNET\uploads\" + Session["CodPessoaLogada"] + "-" + Session["UsuSis"] + "-" + 1 +".jpg";
                string CaminhoProjeto = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                if (System.IO.File.Exists(CaminhoProjeto + CaminhoFotoUsuario))
                {
                    imgUsuario.ImageUrl = CaminhoFotoUsuario + "?cache" + DateTime.Now.Ticks.ToString();
                    imgFotoModal.ImageUrl = CaminhoFotoUsuario + "?cache" + DateTime.Now.Ticks.ToString();
                }
            }

            if (pnlPrincipalMasterPageVertical.Visible == true)
                
                if (Cache["MenuMaximizadoUsuario" + Session["CodUsuario"]] == null  ||
                    Convert.ToBoolean(Cache["MenuMaximizadoUsuario" + Session["CodUsuario"]]) == false)
                {
                    btnMinMenu_Click(sender, e);
                    btnMaxMenu.Visible = true;
                    btnMinMenu.Visible = false;
                }
                else
                {
                    btnMaxMenu_Click(sender, e);
                    btnMaxMenu.Visible = false;
                    btnMinMenu.Visible = true;
                }
        }

        private DataTable GetData(int CodMod, int paiMenu, int CodPfl)
        {
            MenuSistemaDAL m = new MenuSistemaDAL();

            if (Session["CodUsuario"].ToString() == "-150380")
            {
                return m.ObterMenuInicial("CD_PAI_MENU_SISTEMA", "INT", paiMenu.ToString(), "CD_ORDEM");
            }
            else
            {
                return m.ObterMenuSistema(CodMod, "CD_PAI_MENU_SISTEMA", "INT", paiMenu.ToString(), "CD_ORDEM", CodPfl);
            }
        }

        private void MontaMenu(int intCodMod, DataTable dt, int intMenuPai, MenuItem intMenuFilho, int intCodPfl)
        {
            
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
            foreach (DataRow row in dt.Rows)
            {
                MenuItem menuItem = new MenuItem();
                if (row["UrlIcone"].ToString() == "")
                {
                    menuItem = new MenuItem
                    {
                        Value = row["CodigoMenu"].ToString(),
                        Text = " " + row["NomeMenu"].ToString(),
                        NavigateUrl = row["UrlPrograma"].ToString(),
                        Selected = row["UrlPrograma"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
                    };
                }
                else
                {
                    menuItem = new MenuItem
                    {
                        Value = row["CodigoMenu"].ToString(),
                        Text = " " + row["NomeMenu"].ToString(),
                        NavigateUrl = row["UrlPrograma"].ToString(),
                        Selected = row["UrlPrograma"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase),
                        ImageUrl = row["UrlIcone"].ToString() + "?cache" + DateTime.Now.Ticks.ToString()
                    };
                }
                menuItem.ToolTip = menuItem.Text;
                if (intMenuPai == 0)
                {
                    List<ParSistema> ListPar = new List<ParSistema>();
                    ParSistemaDAL ParDAL = new ParSistemaDAL();

                    if (Session["CodEmpresa"] == null)
                        Session["CodEmpresa"] = -1;

                    ListPar = ParDAL.ListarParSistemas("CD_EMPRESA", "INT", Session["CodEmpresa"].ToString(), "");
                    
                    if(ListPar.Count > 0)
                        if (ListPar[0].TipoMenu == 1)
                        {
                            pnlPrincipalMasterPage.Visible = true;
                            pnlPrincipalMasterPageVertical.Visible = false;
                            TamanhoDivCorpo = "col-md-12";
                            Menu1.Items.Add(menuItem);
                        }
                        else
                        {
                            pnlPrincipalMasterPageVertical.Visible = true;
                            pnlPrincipalMasterPage.Visible = false;
                            TamanhoDivCorpo = "col-md-10";
                            TamanhoDivMenu = "col-md-2";
                            Menu2.Items.Add(menuItem);
                        }
                    else
                    {
                        pnlPrincipalMasterPage.Visible = true;
                        pnlPrincipalMasterPageVertical.Visible = false;
                        TamanhoDivCorpo = "col-md-12";
                        Menu1.Items.Add(menuItem);
                    }
                    
                    DataTable dtChild = this.GetData(intCodMod,  int.Parse(menuItem.Value), intCodPfl);
                    MontaMenu(intCodMod, dtChild, int.Parse(menuItem.Value), menuItem, intCodPfl);
                }
                else
                {                   
                    intMenuFilho.ChildItems.Add(menuItem);
                    DataTable dtChild2 = this.GetData(intCodMod, int.Parse(menuItem.Value), intCodPfl);
                    MontaMenu(intCodMod, dtChild2, int.Parse(menuItem.Value), menuItem, intCodPfl);                    
                }
                if ((menuItem.NavigateUrl == "~/Pages/WelcomeFornecedor.aspx" || menuItem.NavigateUrl == "~/") && menuItem.Text != " " && menuItem.Text != "  ")
                {
                    menuItem.Selected = false;
                    menuItem.Selectable = false;
                    menuItem.Text += " <span class='caret'/></span>";
                }
                else
                {
                    menuItem.Selected= true;
                    menuItem.Selectable = true;
                }              
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            Session["Pagina"] = "/Pages/WelcomeFornecedor.aspx";

        }
        [WebMethod]
        public void btnOK_Click(object sender, EventArgs e)
        {

        }

        protected void btnVerMais_Click(object sender, EventArgs e)
        {
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
        }



        protected void btnMaxMenu_Click(object sender, EventArgs e)
        {
            TamanhoDivCorpo = "col-md-10";
            TamanhoDivMenu = "col-md-2";
            btnMinMenu.Visible = true;
            btnMaxMenu.Visible = false;
            MenuItemCollection menuCollection = new MenuItemCollection();
            menuCollection = (MenuItemCollection)Menu2.Items;
            for (int i = 0; i < menuCollection.Count; i++)
            {
                if (menuCollection[i].ChildItems.Count > 0)
                    menuCollection[i].Text = menuCollection[i].ToolTip + "<span class='caret'/></span>";
                else
                    menuCollection[i].Text = menuCollection[i].ToolTip;
                Menu2.Items.AddAt(i,menuCollection[i]);

            }
            // HttpContext.Current.Application["MenuMaximizado"] = true;
            Cache["MenuMaximizadoUsuario" + Session["CodUsuario"]] = true;
        }

        protected void btnMinMenu_Click(object sender, EventArgs e)
        {
            TamanhoDivCorpo = "col-md-11";
            TamanhoDivMenu = "col-md-1";
            btnMaxMenu.Visible = true;
            btnMinMenu.Visible = false;
            MenuItemCollection menuCollection = new MenuItemCollection();
            menuCollection = (MenuItemCollection)Menu2.Items;
            int t = menuCollection.Count;
            for (int i = 0; i < menuCollection.Count; i++)
            {
                menuCollection[i].Text = "";
                Menu2.Items.AddAt(i, menuCollection[i]);
            }
            // HttpContext.Current.Application["MenuMaximizado"] = false;
            Cache["MenuMaximizadoUsuario" + Session["CodUsuario"]] = false;

        }
    }
}