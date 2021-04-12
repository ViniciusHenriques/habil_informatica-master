using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using static System.Console;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using System.Drawing.Imaging;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class RelOrdProducao : System.Web.UI.Page
    {
        ReportDocument RptDoc;

        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        string strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
		protected void ShowMessage(string Message, MessageType type)
		{
			ScriptManager.RegisterStartupScript(this, GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
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
					if (!IsPostBack)
					{
						List<string> ListaImpressoras = new List<string>();
						foreach (string impressora in PrinterSettings.InstalledPrinters)
						{

							ListaImpressoras.Add(impressora);
						}
						ddlImpressora.DataSource = ListaImpressoras;
						ddlImpressora.DataBind();
					}
				});

            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
			if (Session["NrOrdProducao2"] != null)
			{
				Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();

				RnOrd.AtualizarQuantidade(0, Convert.ToDecimal(Session["NrOrdProducao2"].ToString()));

				Session["NrOrdProducao2"] = null;
			}

			Response.Redirect("~/Pages/Estoque/ConOrdProducao.aspx");
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

            Doc_OrdProducao r = new Doc_OrdProducao();
			Doc_OrdProducaoDAL RnDAL = new Doc_OrdProducaoDAL();
            List<EstoqueProduto> lista = new List<EstoqueProduto>();

			RptDoc.Load(Server.MapPath("~/Pages/Estoque/RPT/RelOrdProducao.rpt"));

			if (Session["NomeEmpresa"] != null)
				RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

			if (Session["UsuSis"] != null)
				RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";
			

			if (Session["NrOrdProducao"] != null )
			{
				RptDoc.SetDataSource(RnDAL.RelOrdemProducao(Convert.ToDecimal(Session["NrOrdProducao"].ToString())));
			}
			else if (Session["RptDocAdicionar"] != null)
			{ 
				RptDoc.SetDataSource((DataTable)Session["RptDocAdicionar"]);
			}

			CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
        }
        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            MontaCrystal();
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
		protected void btnImprimir_Click(object sender, EventArgs e)
		{
			try
			{
				if (!btnImprimir.Visible)
					btnImprimir_Click(sender, e);

				RptDoc.PrintOptions.PrinterName = (@ddlImpressora.SelectedValue);
				RptDoc.PrintToPrinter(1, false, 0, 0);
				ShowMessage("Orçamento enviado para impressora, aguarde...", MessageType.Success);
			}
			catch (Exception ex)
			{
				ShowMessage("Erro ao se conectar com a impressora. " + ex.Message, MessageType.Error);
			}
		}
	}
}