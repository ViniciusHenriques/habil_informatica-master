using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class RelComposicao : System.Web.UI.Page
    {
        ReportDocument RptDoc;

        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        public enum MessageType { Success, Error, Info, Warning };
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
                                               "RelComposicao.aspx");
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
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ListComposicao"] = null;
            Session["DescricaoComposicao"] = null;
            Response.Redirect("~/Pages/Produtos/ConComposicao.aspx");
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
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
        private void MontaCrystal()
        {
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();
            if (Session["ListComposicao"] != null)
            {
                lista = (List<DBTabelaCampos>)Session["ListComposicao"];
            }

            RptDoc = new ReportDocument();

            RptDoc.Load(Server.MapPath("~/Pages/Produtos/RPT/RelComposicao.rpt"));

            ComposicaoDAL RnCom = new ComposicaoDAL();
            List<Composicao> ListaComp = new List<Composicao>();

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["DescricaoComposicao"] != null)
                RptDoc.DataDefinition.FormulaFields["Descricao"].Text = "'" + Session["DescricaoComposicao"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            ListaComp = RnCom.ListarComposicao(lista);

            RptDoc.SetDataSource(RnCom.RelComposicao(lista));
            
            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
        }
    }
}