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
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == "")) 
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                DBTabelaDAL d = new DBTabelaDAL();

                ReportViewer1.Visible = false;

                ddlPesquisa.DataSource = d.ListarCamposSQL("SITUACAO");
                ddlPesquisa.DataTextField = "NomeComum";
                ddlPesquisa.DataValueField = "Coluna";
                ddlPesquisa.DataBind();

                ddlPesquisa.SelectedIndex = 1;

            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            /////////////////////////////

            DBTabelaDAL d = new DBTabelaDAL();
            Habil_TipoDAL r = new Habil_TipoDAL();
            DataTable dt = new DataTable();
            ReportViewer Rv = new ReportViewer();

            String strTipo = d.ListarTipoCampoSQL("SITUACAO", ddlPesquisa.SelectedValue).ToString().ToUpper();
            string strValor = txtVarchar.Text + txtInt.Text + txtValor.Text;
            dt = r.ObterHabilTipos(ddlPesquisa.Text, strTipo, strValor, ddlPesquisa.Text);

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

        protected void ddlPesquisa_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportViewer1.Visible = false;
            try
            {

                txtVarchar.Visible = false;
                txtInt.Visible = false;
                txtValor.Visible = false;
                txtVarchar.Text = "";
                txtInt.Text = "";
                txtValor.Text = "";

                DBTabelaDAL d = new DBTabelaDAL();
                String strTipo = d.ListarTipoCampoSQL("SITUACAO", ddlPesquisa.SelectedValue).ToString().ToUpper();

                switch (strTipo)
                {
                    case "VARCHAR":
                        txtVarchar.Visible = true;
                        txtVarchar.Text = "";
                        txtVarchar.Focus();
                        break;

                    case "INT":
                        txtInt.Visible = true;
                        txtInt.Text = "";
                        txtInt.Focus();
                        break;

                    case "NUMERIC":
                        txtValor.Visible = true;
                        txtValor.Text = "";
                        txtValor.Focus();
                        break;

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Server.Transfer("~/PagesAdmin/ConSituacao.aspx");
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            Habil_TipoDAL r = new Habil_TipoDAL();
            DataTable dt = new DataTable();

            String strTipo = d.ListarTipoCampoSQL("SITUACAO", ddlPesquisa.SelectedValue).ToString().ToUpper();
            string strValor = txtVarchar.Text + txtInt.Text + txtValor.Text;
            dt = r.ObterHabilTipos(ddlPesquisa.Text, strTipo, strValor, ddlPesquisa.Text);

            ReportViewer1.LocalReport.DataSources.Clear();

            ReportDataSource rs = new ReportDataSource("RDS_Situacao", dt);

            ReportViewer1.LocalReport.DataSources.Add(rs);

            ReportViewer1.Visible = true;


        }
    }
}