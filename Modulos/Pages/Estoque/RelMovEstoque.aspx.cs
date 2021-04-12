using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using static System.Console;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class RelMovEstoque : System.Web.UI.Page
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
            Session["DescricaoMovEstoque"] = null;
            Session["LST_CONMOVESTOQUE"] = null;
            Response.Redirect("~/Pages/Estoque/ConMovEstoque.aspx");
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["DescricaoMovEstoque"] = null;
            Session["LST_CONMOVESTOQUE"] = null;
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

            int intEmpresa = 0;
            string strDataInicial = DateTime.Now.ToString();
            string strDataFinal = DateTime.Now.ToString();
            int intProduto = 0;
            int intTpOperacao = 0;
            int intLocalizacao = 0;
            int intLote = 0;
            string strDocumento = "";

            MovimentacaoInterna r = new MovimentacaoInterna();
            MovimentacaoInternaDAL RnMovimentacaoInternaDAL = new MovimentacaoInternaDAL();
            List<MovimentacaoInterna> lista = new List<MovimentacaoInterna>();

            RptDoc.Load(Server.MapPath("~/Pages/Estoque/RPT/RelMovEstoque.rpt"));

            if (Session["LST_CONMOVESTOQUE"] != null)
            {
                string[] strParamtros = Session["LST_CONMOVESTOQUE"].ToString().Split(';');

                intEmpresa = Convert.ToInt32(strParamtros[0].ToString());
                strDataInicial = strParamtros[1].ToString();
                strDataFinal = strParamtros[2].ToString();
                intProduto = Convert.ToInt32(strParamtros[3].ToString());
                intTpOperacao = Convert.ToInt32(strParamtros[4].ToString());
                intLocalizacao = Convert.ToInt32(strParamtros[5].ToString());
                intLote = Convert.ToInt32(strParamtros[6].ToString());
                strDocumento = strParamtros[7].ToString();

            }
            if(ddlRegistros.SelectedIndex == 1)
                RptDoc.DataDefinition.FormulaFields["TpOper"].Text = "'1'";
            if (ddlRegistros.SelectedIndex == 2)
                RptDoc.DataDefinition.FormulaFields["TpOper"].Text = "'2'";
            if (ddlRegistros.SelectedIndex == 3)
                RptDoc.DataDefinition.FormulaFields["TpOper"].Text = "'3'";


            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            if (Session["DescricaoMovEstoque"] != null)
            {
                RptDoc.DataDefinition.FormulaFields["Descricao"].Text = "'" + Session["DescricaoMovEstoque"].ToString() + "'";
            }
            if (ddlRegistros.SelectedValue == "1")
            {
                //RptDoc.DataDefinition.FormulaFields["TpOper"].Text = "'1'";
                RptDoc.SetDataSource(RnMovimentacaoInternaDAL.RelMovEstoque(intEmpresa, Convert.ToDateTime(strDataInicial), Convert.ToDateTime(strDataFinal), intProduto, intTpOperacao,
               intLocalizacao, intLote, strDocumento, 1));
            }
            if (ddlRegistros.SelectedValue == "2")
            {
                //RptDoc.DataDefinition.FormulaFields["TpOper"].Text = "'2'";
                RptDoc.SetDataSource(RnMovimentacaoInternaDAL.RelMovEstoque(intEmpresa, Convert.ToDateTime(strDataInicial), Convert.ToDateTime(strDataFinal), intProduto, intTpOperacao,
               intLocalizacao, intLote, strDocumento, 2));
            }
            if (ddlRegistros.SelectedValue == "3")
            {
                //RptDoc.DataDefinition.FormulaFields["TpOper"].Text = "'3'";
                RptDoc.SetDataSource(RnMovimentacaoInternaDAL.RelMovEstoque(intEmpresa, Convert.ToDateTime(strDataInicial), Convert.ToDateTime(strDataFinal), intProduto, intTpOperacao,
               intLocalizacao, intLote, strDocumento, 3));
            }

            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
        }

        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            MontaCrystal();
        }
        protected void ddlRegistros_TextChanged(object sender, EventArgs e)
        {
            //MontaCrystal();
        }
    }
}