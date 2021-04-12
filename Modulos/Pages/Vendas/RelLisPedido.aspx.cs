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
    public partial class RelLisPedido : System.Web.UI.Page
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
            MontaCrystal();
            
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["PrimeiraVez"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void CarregaDados()
        {

        }
        protected void MontarDescricaoInventario()
        {
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RptDoc"] = null;
                RptDoc = null;
                //MontaCrystal();
            }
            if (Session["RptDoc"] != null)
            {
                CRViewer.ReportSource = (ReportDocument)Session["RptDoc"];
                CRViewer.DataBind();
            }
        }
        private void MontaCrystal()
         {
            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
            int intMaquina = 0;
            RptDoc = new ReportDocument();

            if (Session["LST_REL"] == null)
            {
                Session["LST_REL"] = 0;
            }
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            if (he != null)
            {
                intMaquina = Convert.ToInt32(he.CodigoEstacao);
            }

            RptDoc.Load(Server.MapPath("~/Pages/Vendas/RPT/RelLisDocumento.rpt"));

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";
            if (Session["Estacao_Logada"] != null)
                RptDoc.DataDefinition.FormulaFields["MAQ"].Text = "'" + intMaquina.ToString() + "'";
            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            if (Session["LISTAGEMPORPEDIDO"] != null)
            {
                RptDoc.SetDataSource(RnPedido.RelLisDocumento(Session["LISTAGEMPORPEDIDO"].ToString()));
            }
            else
            {
                RptDoc.SetDataSource(RnPedido.RelLisDocumento(Session["LST_REL"].ToString()));
            }
            if (Session["IMPRIMIR_DIRETO"] != null)
            {
                //RptDoc.PrintOptions.PrinterName = (@"\\Compaq\HP LaserJet Professional M1132 MFP");
                //RptDoc.PrintToPrinter(1, false, 0, 0);
                //btnVoltar_Click(null, null);
            }

            //CRViewer.Zoom(125);

            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (Session["LISTAGEMPORPEDIDO"] != null)
            {
                Session["LISTAGEMPORPEDIDO"] = null;
                Response.Redirect("~/Pages/Vendas/ConPedido.aspx");
            }
            else
            {
                Session["VoltaDoRel"] = "a";
                Session["LST_REL"] = null;
                Response.Redirect("~/Pages/Vendas/LisPedido.aspx");
            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            RptDoc.PrintOptions.PrinterName = (@"\\Compaq\HP LaserJet Professional M1132 MFP");
            RptDoc.PrintToPrinter(1, false, 0, 0);
        }
    }
}