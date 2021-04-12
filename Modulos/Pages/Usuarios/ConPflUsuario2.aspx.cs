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
    public partial class ConPflUsuario2 : System.Web.UI.Page
    {

        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        protected void ApresentaMensagem(String strMensagem)
        {
            lblMensagem.Text = strMensagem;
            pnlMensagem.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

                if (Session["Operacao"].ToString() == "CONSULTA") {
                    return;
                }

                txtVarchar.Focus();

                if (!Page.IsPostBack)
                {
                    Session["ZoomPflUsuario"] = "³";

                    DBTabelaDAL d = new DBTabelaDAL();
                    PerfilUsuarioDAL  r = new PerfilUsuarioDAL();

                    ddlPesquisa.DataSource = d.ListarCamposSQL("PERFIL_DO_USUARIO");
                    ddlPesquisa.DataTextField = "NomeComum";
                    ddlPesquisa.DataValueField = "Coluna";
                    ddlPesquisa.DataBind();

                    grdPflUsuario.DataSource = r.ListarPerfisUsuario("", "", "", "");
                    grdPflUsuario.DataBind();
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
                    ApresentaMensagem(strMensagemR);
                    return;
                }
            }

            PerfilUsuarioDAL r = new PerfilUsuarioDAL();
            grdPflUsuario.DataSource = r.ListarPerfisUsuario(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);
            grdPflUsuario.DataBind();
            if (grdPflUsuario.Rows.Count == 0)
                ApresentaMensagem("Perfil(s) de Usuário(s) não encontrado(s) mediante a pesquisa realizada.");
            cboSelecionar(sender, e);
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtDescricao.Text = "";
            txtVarchar.Text = "";
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
        protected void grdPflUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodigo.Text = HttpUtility.HtmlDecode(grdPflUsuario.SelectedRow.Cells[0].Text);
            txtDescricao.Text = HttpUtility.HtmlDecode(grdPflUsuario.SelectedRow.Cells[1].Text);

            Session["ZoomPflUsuario"] = HttpUtility.HtmlDecode(grdPflUsuario.SelectedRow.Cells[0].Text)
                   + "²" + HttpUtility.HtmlDecode(grdPflUsuario.SelectedRow.Cells[1].Text);
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomPflUsuario"] = null;
            string _open = "window.open('CadPflUsuario.aspx','','resizable=no, menubar=no, scrollbars=yes,location=no,titlebar=no');";
            Session["ZoomPflUsuario2"] = "RELACIONAL";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }
        protected void btnMensagem_Click(object sender, EventArgs e)
        {
        }
        protected void btnCfmNao2_Click(object sender, EventArgs e)
        {
            pnlMensagem.Visible = false; 
        }
    }
}