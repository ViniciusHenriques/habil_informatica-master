using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using DAL.Persistence;
using Microsoft.Reporting.WebForms;
using DAL.Model;

namespace HabilInformatica.Relatorios
{
    public partial class RelPflUsuario : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();

        protected void ApresentaMensagem(String strMensagem)
        {
            lblMensagem.Text = strMensagem;
            pnlMensagem.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                DBTabelaDAL d = new DBTabelaDAL();


                ddlPesquisa.DataSource = d.ListarCamposSQL("PERFIL_DO_USUARIO");
                ddlPesquisa.DataTextField = "NomeComum";
                ddlPesquisa.DataValueField = "Coluna";
                ddlPesquisa.DataBind();

                btnVisualizar_Click(sender, e);

            }


            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                           Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                           "ConPflUsuario.aspx");
            lista.ForEach(delegate(Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoImprimir)
                    {
                        btnImprimir.Visible = false;
                        ReportViewer1.ShowExportControls = false;
                        ReportViewer1.ShowPrintButton = false;
                    }

                }
            });



        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {

            try
            {
                /////////////////////////////

                DBTabelaDAL d = new DBTabelaDAL();
                PerfilUsuarioDAL r = new PerfilUsuarioDAL();
                DataTable dt = new DataTable();
                ReportViewer Rv = new ReportViewer();

                String strTipo = d.ListarTipoCampoSQL("PERFIL_DO_USUARIO", ddlPesquisa.SelectedValue).ToString().ToUpper();
                dt = r.ObterPerfisUsuario(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);

                ReportDataSource rs = new ReportDataSource("RDS_PflUsuario", dt);

                Rv.ProcessingMode = ProcessingMode.Local;
                Rv.LocalReport.ReportPath = @"Relatorios\RLDC\RelPflUsuario.rdlc";
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
            catch (Exception ex)
            {
                ApresentaMensagem(ex.Message.ToString());
            }
        }

        protected void ddlPesquisa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.Visible = false;
                txtVarchar.Text = "";
                txtVarchar.Focus();
            }
            catch (Exception ex)
            {
                ApresentaMensagem(ex.Message.ToString());
            }

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Usuarios/ConPflUsuario.aspx");
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                DBTabelaDAL d = new DBTabelaDAL();
                PerfilUsuarioDAL r = new PerfilUsuarioDAL();
                DataTable dt = new DataTable();

                String strTipo = d.ListarTipoCampoSQL("PERFIL_DO_USUARIO", ddlPesquisa.SelectedValue).ToString().ToUpper();
                dt = r.ObterPerfisUsuario(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rs = new ReportDataSource("RDS_PflUsuario", dt);
                ReportViewer1.LocalReport.DataSources.Add(rs);
                ReportViewer1.Visible = true;
            }
            catch (Exception ex)
            {
                ApresentaMensagem(ex.Message.ToString());
            }
        }

        protected void btnCfmNao_Click(object sender, EventArgs e)
        {
            pnlMensagem.Visible = false;
        }

        protected void btnMensagem_Click(object sender, EventArgs e)
        {

        }
    }
}