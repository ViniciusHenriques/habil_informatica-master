using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.CEPs
{
    public partial class ConEstado : System.Web.UI.Page
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

            try
            {
                btnMensagem.Visible = false; 

                if (!Page.IsPostBack)
                {
                    Session["ZoomEstado"] = "³";

                    DBTabelaDAL d = new DBTabelaDAL();
                    EstadoDAL  r = new EstadoDAL();

                    ddlPesquisa.DataSource = d.ListarCamposSQL("ESTADO");
                    ddlPesquisa.DataTextField = "NomeComum";
                    ddlPesquisa.DataValueField = "Coluna";
                    ddlPesquisa.DataBind();

                    grdEstado.DataSource = r.ListarEstados("", "", "", "");
                    grdEstado.DataBind();
                }



            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = d.ListarTipoCampoSQL("ESTADO", ddlPesquisa.SelectedValue).ToString().ToUpper();

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


            EstadoDAL r = new EstadoDAL();
            grdEstado.DataSource = r.ListarEstados(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);
            grdEstado.DataBind();
            if (grdEstado.Rows.Count == 0)
                 ApresentaMensagem("Estado(s) não encontrado(s) mediante a pesquisa realizada.");
            txtCodigo.Text = "";
            txtDescricao.Text = "";
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
                txtCodigo.Text = "";
                txtDescricao.Text = "";
                txtVarchar.Text = "";
                txtVarchar.Focus();
        }
        protected void grdEstado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdEstado.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdEstado.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }
        protected void grdEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtCodigo.Text = HttpUtility.HtmlDecode(grdEstado.SelectedRow.Cells[0].Text);
            txtDescricao.Text = HttpUtility.HtmlDecode(grdEstado.SelectedRow.Cells[1].Text) + " - " + HttpUtility.HtmlDecode(grdEstado.SelectedRow.Cells[2].Text); 

            Session["ZoomEstado"] = HttpUtility.HtmlDecode(grdEstado.SelectedRow.Cells[0].Text)
                   + "²" + HttpUtility.HtmlDecode(grdEstado.SelectedRow.Cells[1].Text) + " - " + HttpUtility.HtmlDecode(grdEstado.SelectedRow.Cells[2].Text);
        }
        protected void btnMensagem_Click(object sender, EventArgs e)
        {

        }
        protected void btnCfmNao_Click(object sender, EventArgs e)
        {

        }
    }

}