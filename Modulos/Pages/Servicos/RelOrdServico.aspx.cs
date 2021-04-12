using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;

namespace SoftHabilInformatica.Pages.Servicos
{
    public partial class RelOrdServico: System.Web.UI.Page
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
                                               "RelOrdServico.aspx");
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
            Response.Redirect("~/Pages/Servicos/ConOrdServico.aspx");

        }
        protected void btnSair_Click(object sender, EventArgs e)
        {

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
            try
            {
                RptDoc = new ReportDocument();
                RptDoc.Load(Server.MapPath("~/Pages/Servicos/RPT/RelOrdServico.rpt"));

                if (Session["ZoomOrdemServico"] == null || Session["ZoomOrdemServico"].ToString() == "0")
                    Response.Redirect("~/Pages/Servicos/ConOrdServico.aspx");

                string CaminhoExe = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                Doc_OrdemServicoDAL r = new Doc_OrdemServicoDAL();

                if (Session["NomeEmpresa"] != null)
                    RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

                if (Session["UsuSis"] != null)
                    RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

                RptDoc.Subreports["ModeloOSServico.rpt"].DataDefinition.FormulaFields["ImagemCapa"].Text = "'" + CaminhoExe + "Log\\ImagemRelatorio\\RelOrdServico" + Session["ZoomOrdemServico"] + ".jpg'";
                RptDoc.Subreports["RelItemDocumento.rpt"].DataDefinition.FormulaFields["ImagemItem"].Text = "'" + CaminhoExe + "Log\\ImagemRelatorio\\RelOrdServico'";

                decimal CodigoDocumento = Convert.ToDecimal(Session["ZoomOrdemServico"].ToString());
                ItemDocumentoDAL ItemDAL = new ItemDocumentoDAL();
                Doc_OrdemServico doc = new Doc_OrdemServico();
                doc = r.PesquisarDocumento(CodigoDocumento);

                DataTable dtItem = new DataTable();
                if (doc.CodigoClassificacao == 98)
                {
                    RptDoc.DataDefinition.FormulaFields["Modelo"].Text = "'1'";
                    RptDoc.Subreports["ModeloOSFatura.rpt"].DataDefinition.FormulaFields["Classificacao"].Text = "'FATURA'";
                    if (doc.CodigoCondicaoPagamento != 0)
                    {
                        CondPagamento cndPag = new CondPagamento();
                        CondPagamentoDAL cndPagDAL = new CondPagamentoDAL();
                        cndPag = cndPagDAL.PesquisarCondPagamento(doc.CodigoCondicaoPagamento);
                        RptDoc.Subreports["ModeloOSFatura.rpt"].DataDefinition.FormulaFields["CondPagamento"].Text = "'" + cndPag.DescricaoCondPagamento + "'";
                    }

                    if (doc.CodigoTipoCobranca != 0)
                    {
                        TipoCobranca tpCob = new TipoCobranca();
                        TipoCobrancaDAL tpCobDAL = new TipoCobrancaDAL();
                        tpCob = tpCobDAL.PesquisarTipoCobranca(doc.CodigoTipoCobranca);
                        RptDoc.Subreports["ModeloOSFatura.rpt"].DataDefinition.FormulaFields["TipoCobranca"].Text = "'" + tpCob.DescricaoTipoCobranca + "'";
                    }
                    dtItem = ItemDAL.RelItemDocumento(CodigoDocumento, true);
                }
                else if (doc.CodigoClassificacao == 97)
                {
                    RptDoc.Subreports["ModeloOSServico.rpt"].DataDefinition.FormulaFields["Classificacao"].Text = "'SERVIÇO'";
                    RptDoc.DataDefinition.FormulaFields["Modelo"].Text = "'2'";
                    dtItem = ItemDAL.RelItemDocumento(CodigoDocumento, false);
                }
                else
                    RptDoc.DataDefinition.FormulaFields["Classificacao"].Text = "'NÃO INFORMADO'";


                RptDoc.Subreports["ModeloOSServico.rpt"].SetDataSource(r.RelOrdemServico(CodigoDocumento, false));
                RptDoc.Subreports["ModeloOSFatura.rpt"].SetDataSource(r.RelOrdemServico(CodigoDocumento, false));

                DataTable dt1 = new DataTable();
                dt1 = r.RelServicoDocumento(CodigoDocumento);

                DataTable dt2 = new DataTable();
                dt2 = r.RelProdutoDocumento(CodigoDocumento);

                ParcelaDocumentoDAL parcelaDAL = new ParcelaDocumentoDAL();

                RptDoc.Subreports["RelOrdServico.rpt"].SetDataSource(r.RelOrdemServico(CodigoDocumento, true));
                Thread.Sleep(5000);
                RptDoc.Subreports["RelServicoDocumento.rpt"].Database.Tables["DS_VW_SERV_DOCUMENTO"].SetDataSource(dt1);
                Thread.Sleep(5000);
                RptDoc.Subreports["RelServicoDocumento.rpt"].Database.Tables["DS_VW_PROD_DOCUMENTO"].SetDataSource(dt2);
                Thread.Sleep(5000);
                RptDoc.Subreports["RelParcelaDocumento.rpt"].SetDataSource(parcelaDAL.RelParcelaDocumento(CodigoDocumento));
                Thread.Sleep(5000);
                RptDoc.Subreports["RelItemDocumento.rpt"].SetDataSource(dtItem);

                if (dtItem != null)
                    RptDoc.DataDefinition.FormulaFields["ExisteItem"].Text = "'1'";
                else
                    RptDoc.DataDefinition.FormulaFields["ExisteItem"].Text = "'0'";

                CRViewer.ReportSource = RptDoc;

                Session["RptDoc"] = RptDoc;
                //Session["LST_ORDSERVICO"] = null;
                Session["ZoomOrdemServico"] = null;
            }
            catch(ExportException ex)
            {
                NFSeFuncoes nfse = new NFSeFuncoes();
                nfse.GerandoArquivoLog(ex.Message, 2);
            }
           
        }

        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            MontaCrystal();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            ReportDocument cryRpt = new ReportDocument();
            string CaminhoArquivoLog = @"C:\Habil_Informatica\Modulos\Log\Documento.pdf";
            cryRpt = (ReportDocument)Session["RptDoc"];
            CRViewer.ReportSource = cryRpt;

            CRViewer.RefreshReport();

            cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, CaminhoArquivoLog);

            AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
            string GUID = anexoDAL.GerarGUID("pdf");
            Response.Clear();
            Response.ContentType = "application/octect-stream";
            Response.AppendHeader("content-disposition", "filename=" + GUID);
            Response.TransmitFile(CaminhoArquivoLog);
            Response.End();

        }
    }
}