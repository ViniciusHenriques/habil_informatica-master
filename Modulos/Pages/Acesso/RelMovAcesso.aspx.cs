using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;

namespace HabilInformatica.Pages.Acesso
{
    public partial class RelMovAcesso : System.Web.UI.Page
    {
        ReportDocument RptDoc;

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
                                               "ConMovAcesso.aspx");
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
        }
        protected void BtnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Acesso/ConMovAcesso.aspx");

        }

        private void MontaCrystal()
        {
            RptDoc = new ReportDocument();
            
            if (ddlTipoRelatorio.SelectedValue == "1")
            {
                RptDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                RptDoc.Load(Server.MapPath("~/Pages/Acesso/RPT/RelMovAcesso.rpt"));
            }
            else
            {
                if (ddlTipoRelatorio.SelectedValue == "2")
                    RptDoc.Load(Server.MapPath("~/Pages/Acesso/RPT/RelMovAcesso_2.rpt"));
                else
                {
                    if (ddlTipoRelatorio.SelectedValue == "3")
                        RptDoc.Load(Server.MapPath("~/Pages/Acesso/RPT/RelMovAcesso_3.rpt"));
                    else
                    {
                        if (ddlTipoRelatorio.SelectedValue == "4")
                            RptDoc.Load(Server.MapPath("~/Pages/Acesso/RPT/RelMovAcesso_4.rpt"));
                        else
                        {
                            RptDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                            RptDoc.Load(Server.MapPath("~/Pages/Acesso/RPT/RelMovAcesso.rpt"));
                        }
                    }

                }
            }

            MovAcessoDAL r = new MovAcessoDAL();
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();
            
            if (Session["LST_MOVACESSO"] != null)
                lista = (List<DBTabelaCampos>)Session["LST_MOVACESSO"];

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            RptDoc.SetDataSource(r.RelMovAcessosCompleto(lista));
            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
        }

        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            MontaCrystal();
        }
    }
}