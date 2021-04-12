using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Empresas
{
    public partial class ConTipoDocumento : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaSelecionar(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            ddlPesquisa.DataSource = d.ListarCamposSQL("TIPO_DE_DOCUMENTO");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            txtVarchar.Focus();

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(),MessageType.Info);
                Session["MensagemTela"] = null;
            }



            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                //if (!IsPostBack)
                //{
                    MontaSelecionar(sender, e);

                    TipoDocumentoDAL  r = new TipoDocumentoDAL();
                    grdTipoDocumento.DataSource = r.ListarTipoDocumento("", "", "", "");
                    grdTipoDocumento.DataBind();
                    if (grdTipoDocumento.Rows.Count == 0)
                        ShowMessage("Não existem Tipos de Documento Cadastrados.",MessageType.Info);
                //}

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConTipoDocumento.aspx");
                lista.ForEach(delegate(Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoIncluir)
                            btnNovo.Visible = false;

                        if (!x.AcessoRelatorio)
                            btnImprimir.Visible = false;

                        if (!x.AcessoConsulta)
                        {
                            btnConsultar.Visible = false;
                            grdTipoDocumento.Enabled = false;
                        }

                    }
                });

                if (Session["ZoomTipoDocumento2"] != null)
                {
                    btnImprimir.Visible = false;
                    btnSair.Visible = false;  
                }
            }
        }


        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = d.ListarTipoCampoSQL("TIPO_DE_DOCUMENTO", ddlPesquisa.SelectedValue).ToString().ToUpper();

            Boolean blnCampoValido = false;
            v.CampoValido(ddlPesquisa.SelectedValue.ToString().ToUpper(), txtVarchar.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR,MessageType.Info );
                    txtVarchar.Focus();
                    return;
                }
            }
            TipoDocumentoDAL r = new TipoDocumentoDAL();
            grdTipoDocumento.DataSource = r.ListarTipoDocumento(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);
            grdTipoDocumento.DataBind();
            if (grdTipoDocumento.Rows.Count == 0)
                ShowMessage("Tipo(s) de Documentonão encontrado(s) mediante a pesquisa realizada.",MessageType.Info);
        }

        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text="";
            txtVarchar.Focus();
        }

        protected void grdTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomTipoDocumento"] = HttpUtility.HtmlDecode(grdTipoDocumento.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/Pages/Empresas/CadTipoDocumento.aspx");
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomTipoDocumento"] = null;
            //Response.Redirect("~/Pages/Empresas/CadTipoDocumento.aspx");
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Empresas/RPT/RelTipoDocumento.aspx");

        }

        protected void grdTipoDocumento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdTipoDocumento.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }

        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdTipoDocumento.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }


        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
    }
}