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
    public partial class ConMunicipio : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";

        protected void ApresentaMensagem(String strMensagem)
        {
            lblMensagem.Text = strMensagem;
            pnlMensagem.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        protected void MontaSelecionar(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            ddlPesquisa.DataSource = d.ListarCamposSQL("MUNICIPIO");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            btnMensagem.Visible = false;
            txtVarchar.Focus();

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ApresentaMensagem(Session["MensagemTela"].ToString());
                Session["MensagemTela"] = null;
            }



            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                MontaSelecionar(sender, e);

                MunicipioDAL r = new MunicipioDAL();
                grdGrid.DataSource = r.ListarMunicipios("", "", "", "");
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                    ApresentaMensagem("Não existem Municípios Cadastrados");

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConMunicipio.aspx");
                lista.ForEach(delegate(Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoIncluir)
                            btnNovo.Visible = false;

                        if (!x.AcessoConsulta)
                        {
                            btnConsultar.Visible = false;
                            grdGrid.Enabled = false;
                        }

                    }
                });

                if (Session["ZoomMunicipio2"] != null)
                {
                    btnSair.Visible = false;  
                }
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            string strTipo = d.ListarTipoCampoSQL("MUNICIPIO", ddlPesquisa.SelectedValue).ToString().ToUpper();

            Boolean blnCampoValido = false;
            v.CampoValido(ddlPesquisa.SelectedValue.ToString().ToUpper(), txtVarchar.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ApresentaMensagem(strMensagemR);
                    txtVarchar.Focus();
                    return;
                }
            }
            MunicipioDAL r = new MunicipioDAL();
            grdGrid.DataSource = r.ListarMunicipios(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ApresentaMensagem("Município(s) não encontrado(s) mediante a pesquisa realizada.");
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text="";
            txtVarchar.Focus();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomMunicipio"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/Pages/CEPs/CadMunicipio.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomMunicipio"] = null;
            Response.Redirect("~/Pages/CEPs/CadMunicipio.aspx");
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdGrid.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
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