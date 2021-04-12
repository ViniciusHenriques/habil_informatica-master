using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Security.Cryptography;

namespace SoftHabilInformatica.Pages.Usuarios
{
    public partial class ConUsuario : System.Web.UI.Page
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
            ddlPesquisa.DataSource = d.ListarCamposSQL("USUARIO");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();

        }
        protected void IniciaPagina(object sender, EventArgs e)
        {

            Session["Pagina"] = Request.CurrentExecutionFilePath;

            MontaSelecionar(sender, e);
            UsuarioDAL r = new UsuarioDAL();
            grdUsuario.DataSource = r.ListarUsuarios("", "", "", "");
            grdUsuario.DataBind();
            if (grdUsuario.Rows.Count == 0)
                ApresentaMensagem("Não existem Usuários Cadastrados");

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                           Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                           "ConUsuario.aspx");
            lista.ForEach(delegate(Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoIncluir)
                        btnNovo.Visible = false;

                    if (!x.AcessoConsulta)
                    {
                        btnConsultar.Visible = false;
                        grdUsuario.Enabled = false;
                    }

                }
            });

            if (Session["CodUsuario"].ToString() == "-150380")
            {
                btnNovo.Visible = true;
                btnConsultar.Visible = true;
                grdUsuario.Enabled = true;
            }

            if (Session["ZoomUsuario2"] != null)
            {
                //btnImprimir.Visible = false;
                btnSair.Visible = false;
                btnVoltar.Visible = true;
            }
            else
                btnVoltar.Visible = false;

            if((Session["Doc_OrdemServico"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 1) || (Session["ItemDocumento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 2))
            {
                btnNovo.Visible = false;
                btnVoltar.Visible = true;
                btnSair.Visible = false;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

                if (Session["MensagemTela"] != null)
                {
                    ApresentaMensagem(Session["MensagemTela"].ToString());
                    Session["MensagemTela"] = null;
                }

                if (Session["ZoomUsuario2"] != null)
                {
                    if (Session["ZoomUsuario2"].ToString() == "RELACIONAL")
                    {
                        //pnlPainel.Visible = false;
                        cmdSair.Visible = false;
                    }
                    else
                    {
                        pnlPainel.Visible = true;
                        cmdSair.Visible = true;
                    }
                }
                else
                {
                    pnlPainel.Visible = true;
                    cmdSair.Visible = true;
                }
           

            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
                {
                    IniciaPagina(sender, e);
                    return;
                }
            


        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            UsuarioDAL r = new UsuarioDAL();
            DBTabelaDAL d = new DBTabelaDAL();

            String strTipo = d.ListarTipoCampoSQL("USUARIO", ddlPesquisa.SelectedValue).ToString().ToUpper();
            
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

            grdUsuario.DataSource = r.ListarUsuarios(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);
            grdUsuario.DataBind();
            if (grdUsuario.Rows.Count == 0)
                ApresentaMensagem("Usuário(s) não encontrado(s) mediante a pesquisa realizada.");
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text = "";
            txtVarchar.Focus();
        }
        protected void grdUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (Convert.ToInt32(Request.QueryString["Cad"]) == 1 && Session["Doc_OrdemServico"] != null)
            {
                Session["Doc_OrdemServicoUsuario"] = HttpUtility.HtmlDecode(grdUsuario.SelectedRow.Cells[0].Text) + "³";
                Response.Redirect("~/Pages/Servicos/ManOrdServico.aspx");

            }
            else if(Convert.ToInt32(Request.QueryString["Cad"]) == 2 && Session["ItemDocumento"] != null)
            {
                Session["ItemDocumentoUsuario"] = HttpUtility.HtmlDecode(grdUsuario.SelectedRow.Cells[0].Text);
                Response.Redirect("~/Pages/Servicos/ManItemDocumento.aspx");

            }
            else if (Session["IncCompradorUsuario"] != null)
            {
                List<Comprador> listCadComprador = new List<Comprador>();
                listCadComprador = (List<Comprador>)Session["IncCompradorUsuario"];
                listCadComprador[0].CodigoUsuario = Convert.ToInt64(HttpUtility.HtmlDecode(grdUsuario.SelectedRow.Cells[0].Text));
                Session["IncCompradorUsuario"] = listCadComprador;
                Response.Redirect("~/Pages/Pessoas/CadComprador.aspx");
            }
            else if (Session["IncVendedorUsuario"] != null)
            {
                List<Vendedor> listCadVendedor = new List<Vendedor>();
                listCadVendedor = (List<Vendedor>)Session["IncVendedorUsuario"];
                listCadVendedor[0].CodigoUsuario = Convert.ToInt64(HttpUtility.HtmlDecode(grdUsuario.SelectedRow.Cells[0].Text));
                Session["IncVendedorUsuario"] = listCadVendedor;
                Response.Redirect("~/Pages/Pessoas/CadVendedor.aspx");
            }
            else
            {
                Session["ZoomUsuario"] = HttpUtility.HtmlDecode(grdUsuario.SelectedRow.Cells[0].Text) + "³";
                Response.Redirect("~/Pages/Usuarios/CadUsuario.aspx");
            }

        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomUsuario"] = null;
            Response.Redirect("~/Pages/Usuarios/CadUsuario.aspx");
        }
        protected void grdUsuario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdUsuario.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdUsuario.PageSize = Convert.ToInt16(ddlRegistros.Text);
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
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (Session["Doc_OrdemServico"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 1)
                Response.Redirect("~/Pages/Servicos/ManOrdServico.aspx");
            if (Session["ItemDocumento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 2)
                Response.Redirect("~/Pages/Servicos/ManItemDocumento.aspx");
            if (Session["IncCompradorUsuario"] != null)
                Response.Redirect("~/Pages/Pessoas/CadComprador.aspx");
        }
    }
}