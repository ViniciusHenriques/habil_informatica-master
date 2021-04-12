using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Usuarios
{
    public partial class ConPflUsuario : System.Web.UI.Page
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
            ddlPesquisa.DataSource = d.ListarCamposSQL("PERFIL_DO_USUARIO");
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

                    PerfilUsuarioDAL r = new PerfilUsuarioDAL();
                    grdPflUsuario.DataSource = r.ListarPerfisUsuario("", "", "", "");
                    grdPflUsuario.DataBind();
                    if (grdPflUsuario.Rows.Count == 0)
                        ShowMessage("Não existem Perfis de Usuários Cadastrados.",MessageType.Info);
                //}

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConPflUsuario.aspx");
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
                            grdPflUsuario.Enabled = false;
                        }

                    }
                });

                if (Session["ZoomPflUsuario2"] != null)
                {
                    btnImprimir.Visible = false;
                    btnSair.Visible = false;  
                }
            }
            if (Session["CodUsuario"].ToString() == "-150380")
            {
                btnNovo.Visible = true;
                btnConsultar.Visible = true;
                grdPflUsuario.Enabled = true;
            }
        }


        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = d.ListarTipoCampoSQL("PERFIL_DO_USUARIO", ddlPesquisa.SelectedValue).ToString().ToUpper();

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
            PerfilUsuarioDAL r = new PerfilUsuarioDAL();
            grdPflUsuario.DataSource = r.ListarPerfisUsuario(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);
            grdPflUsuario.DataBind();
            if (grdPflUsuario.Rows.Count == 0)
                ShowMessage("Perfil(s) de Usuário(s) não encontrado(s) mediante a pesquisa realizada.",MessageType.Info);
        }

        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text="";
            txtVarchar.Focus();
        }

        protected void grdPflUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomPflUsuario"] = HttpUtility.HtmlDecode(grdPflUsuario.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/Pages/Usuarios/CadPflUsuario.aspx");
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomPflUsuario"] = null;
            Response.Redirect("~/Pages/Usuarios/CadPflUsuario.aspx");
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Relatorios/RelPflUsuario.aspx");

        }

        protected void grdPflUsuario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdPflUsuario.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }

        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdPflUsuario.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }


        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
    }
}