using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using static System.Console;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class RelEstoque : System.Web.UI.Page
    {
        ReportDocument RptDoc;

        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        string strMensagemR = "";
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
                                               "RelCtaPagar.aspx");
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
            Response.Redirect("~/Pages/Estoque/ConEstoque.aspx");

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
            RptDoc = new ReportDocument();

            string strNomeCampo = "";
            string strTipoCampo = "";
            string strValor = "";
            string strOrdem = "";


            EstoqueProduto r = new EstoqueProduto();
            EstoqueProdutoDAL RnEstoqueProdutoDAL = new EstoqueProdutoDAL();
            List<EstoqueProduto> lista = new List<EstoqueProduto>();

            RptDoc.Load(Server.MapPath("~/Pages/Estoque/RPT/RelEstoque.rpt"));

            if (Session["LST_ESTOQUE"] != null)
            {
                string[] strParamtros = Session["LST_ESTOQUE"].ToString().Split(';');

                strNomeCampo = strParamtros[0].ToString();
                strTipoCampo = strParamtros[1].ToString();
                strValor = strParamtros[2].ToString();
                strOrdem = strParamtros[3].ToString();

            }
            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            if (Session["DescricaoMovEstoque"] != null)
                RptDoc.DataDefinition.FormulaFields["Descricao"].Text = "'" + Session["DescricaoMovEstoque"].ToString() + "'";

            if (Session["LST_ESTOQUE"] != null)
            {
                RptDoc.SetDataSource(RnEstoqueProdutoDAL.RelEstoqueProduto(strNomeCampo, strTipoCampo, strValor, strOrdem));
            }
            else
            {
                RptDoc.SetDataSource(RnEstoqueProdutoDAL.RelEstoqueProduto("", "", "", ""));
            }

            Session["LST_ESTOQUE_RELACIONAL"] = Session["LST_ESTOQUE"];

            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
            Session["LST_ESTOQUE"] = null;
            Session["DescricaoMovEstoque"] = null;
        }

        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            MontaCrystal();
        }

        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}