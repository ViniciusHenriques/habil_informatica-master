using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;

namespace HabilInformatica.Pages.Impostos
{
    public partial class RelRegFisIPI: System.Web.UI.Page
    {
        ReportDocument RptDoc;
        DataDefinition df;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RptDoc"] = null;
                RptDoc = null;
                MontaCrystal();
            }
            if (Session["RptDoc"] != null) 
            {
                CRViewer.ReportSource = (ReportDocument)Session["RptDoc"];
                CRViewer.DataBind();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
            }

            if ((Session["CodModulo"] != null) && (Session["CodPflUsuario"] != null))
            {
                List<Permissao> lista1 = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista1 = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConRegFisIPI.aspx");
                lista1.ForEach(delegate (Permissao x)
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
            //MontaCrystal();
        }
        protected void BtnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Impostos/ConRegFisIPI.aspx");

        }

        private void MontaCrystal()
        {
            RptDoc = new ReportDocument();

            RptDoc.Load(Server.MapPath("~/Pages/Impostos/RPT/RelRegFisIPI.rpt"));

            RegraFisIPIDAL r = new RegraFisIPIDAL();
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();

            if (Session["ZoomRelRegraIPI"] != null)
            {
                lista = (List<DBTabelaCampos>)Session["ZoomRelRegraIPI"];
            }
            else
                Response.Redirect("~/Pages/Impostos/ConRegFisIPI.aspx");

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            RptDoc.SetDataSource(r.RelRegraIPI(lista));
            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
        }
    }
}