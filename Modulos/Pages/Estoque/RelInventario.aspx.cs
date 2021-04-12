using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using static System.Console;
using CrystalDecisions.CrystalReports.Engine;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class RelInventario : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        ReportDocument RptDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocada"] != null)
                PanelSelect = Session["TabFocada"].ToString();

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "RelInventario.aspx");
                lista.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoImprimir)
                        {
                            CRViewer.HasExportButton = false;
                            CRViewer.HasPrintButton = false;
                            CRViewer.HasToggleGroupTreeButton = false;
                        }
                    }
                });
            }
            CarregaDados();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomRelInv"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[1].Text);

            CRViewer.Visible = true;
            MontaCrystal();
            pnlGrid.Visible = false;
            pnlRel.Visible = true;
            btnVoltar.Visible = true;
            btnSair.Visible = false;
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void CarregaDados()
        {
            InventarioDAL ep = new InventarioDAL();
            grdGrid.DataSource = ep.ListarGrid();
            grdGrid.DataBind();
            btnVoltar.Visible = false;
        }
        protected void MontarDescricaoInventario()
        {
            string dtlancamento = "";
            string desc = "";
            decimal indice = Convert.ToInt32(Session["ZoomRelInv"].ToString());
            InventarioDAL RnInventarioDAL = new InventarioDAL();

            RnInventarioDAL.MontaSession(indice, ref desc, ref dtlancamento);

            Session["DescInv"] = " " + indice.ToString() + " - Data: " + dtlancamento + " - Descrição: " + desc;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RptDoc"] = null;
                RptDoc = null;
               // MontaCrystal();
            }
            if (Session["RptDoc"] != null)
            {
                CRViewer.ReportSource = (ReportDocument)Session["RptDoc"];
                CRViewer.DataBind();
            }
        }
        private void MontaCrystal()
        {
            string strCon = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[6].Text);
            short shCon = Convert.ToInt16(strCon);
            //decIndice = Convert.ToDecimal(row.Cells[6].Text);
            InventarioDAL RnInventarioDAL = new InventarioDAL();

            RptDoc = new ReportDocument();

            if (Session["ZoomRelInv"] == null)
            {
                Session["ZoomRelInv"] = 0;
            }
            MontarDescricaoInventario();

            RptDoc.Load(Server.MapPath("~/Pages/Estoque/RPT/RelInventario.rpt"));

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";
            if (Session["Inventario"] == null)
                RptDoc.DataDefinition.FormulaFields["Inventario"].Text = "'" + Session["DescInv"].ToString() + "'";
            else
                return;

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            RptDoc.SetDataSource(RnInventarioDAL.RelInventario(Convert.ToInt32(Session["ZoomRelInv"].ToString()), Convert.ToInt16(ddlTpInventario.SelectedValue), shCon));

            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            pnlGrid.Visible = true;
            pnlRel.Visible = false;
            btnVoltar.Visible = false;
            btnSair.Visible = true;

        }
        protected void btnGerar_Click(object sender, EventArgs e)
        {
            CRViewer.Visible = true;
            MontaCrystal();
            pnlGrid.Visible = false;
            pnlRel.Visible = true;
            btnVoltar.Visible = true;
            btnSair.Visible = false;
        }
    }
}