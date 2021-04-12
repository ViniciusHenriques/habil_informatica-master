using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Persistence;
using Microsoft.Reporting.WebForms;


namespace HabilInformatica.Relatorios
{
    public partial class RelSituacao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) ) 
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                btnVisualizar_Click(sender, e);  
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            /////////////////////////////

            DBTabelaDAL d = new DBTabelaDAL();
            SituacaoDAL r = new SituacaoDAL();
            DataTable dt = new DataTable();
            ReportViewer Rv = new ReportViewer();

            dt = r.ObterSituacoes("CD_SITUACAO", "", "", "CD_SITUACAO");

            ReportDataSource rs = new ReportDataSource("RDS_Situacao", dt);

            Rv.ProcessingMode = ProcessingMode.Local;
            Rv.LocalReport.ReportPath = @"Relatorios\RLDC\RelSituacao.rdlc";
            Rv.LocalReport.DataSources.Add(rs);
            Rv.LocalReport.Refresh();

            /////////////////////////////

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = Rv.LocalReport.Render(
            "Pdf", null, out mimeType, out encoding,
            out extension,
            out streamids, out warnings);

            FileStream fs = new FileStream(@"c:\RelRLDC\output.pdf",
                FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            /////////////////////////////
            var process = System.Diagnostics.Process.Start(@"c:\RelRLDC\output.pdf");
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Server.Transfer("~/PagesAdmin/ConSituacao.aspx");
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            SituacaoDAL r = new SituacaoDAL();
            DataTable dt = new DataTable();

            dt = r.ObterSituacoes("", "", "", "CD_SITUACAO");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource rs = new ReportDataSource("RDS_Situacao", dt);
            ReportViewer1.LocalReport.DataSources.Add(rs);
            ReportViewer1.Visible = true;
        }
    }
}