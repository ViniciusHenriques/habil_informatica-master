using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class ConProduto : System.Web.UI.Page
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
            ddlPesquisa.DataSource = d.ListarCamposSQL("PRODUTO");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtVarchar.Focus();

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }



            if ((Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) || (ddlPesquisa.Text == ""))
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                MontaSelecionar(sender, e);

                ProdutoDAL r = new ProdutoDAL();
                grdGrid.DataSource = r.ListarProdutos("", "", "", "");
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                    ShowMessage("Não existem Produtos Cadastrados.", MessageType.Info);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConProduto.aspx");
                lista.ForEach(delegate (Permissao x)
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
                if (Session["Doc_orcamento"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                {
                    btnNovo.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                if (Session["EstoqueProduto"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 2)
                {
                    btnNovo.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                if (Session["Lote"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 3)
                {
                    btnNovo.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                if (Session["MovimentacaoInterna"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 4)
                {
                    btnNovo.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                if (Session["ConMovEstoque"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 5)
                {
                    btnNovo.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                if (Session["Inventario"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 6)
                {
                    btnNovo.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                if (Session["RelMovEntSai"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 7)
                {
                    btnNovo.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                if (Session["ComposicaoProduto"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 8)
                {
                    btnNovo.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                {
                    btnVoltar.Visible = false;
                }
                if (Session["ZoomProduto2"] != null)
                {
                    btnSair.Visible = false;
                }
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = d.ListarTipoCampoSQL("PRODUTO", ddlPesquisa.SelectedValue.ToString().ToUpper());

            Boolean blnCampoValido = false;
            v.CampoValido(ddlPesquisa.SelectedValue.ToString().ToUpper(), txtVarchar.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtVarchar.Focus();
                    return;
                }
            }
            ProdutoDAL r = new ProdutoDAL();
            grdGrid.DataSource = r.ListarProdutos(ddlPesquisa.Text, strTipo.ToString().ToUpper(), txtVarchar.Text, ddlPesquisa.Text);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ShowMessage("Produto(s) não encontrada(s) mediante a pesquisa realizada.", MessageType.Info);
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text = "";
            txtVarchar.Focus();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomProduto"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
            Session["CaminhoProdutoFoto0"] = null;
            Session["CaminhoProdutoFoto1"] = null;
            Session["CaminhoProdutoFoto2"] = null;
            Session["CaminhoProdutoFoto3"] = null;
            Session["CaminhoProdutoFoto4"] = null;
            Session["CaminhoProdutoFoto5"] = null;

            if (Session["Doc_orcamento"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                Response.Redirect("~/Pages/Vendas/ManItemOrcamento.aspx");
            else
            if (Session["EstoqueProduto"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 2)
                Response.Redirect("~/Pages/Estoque/CadEstoque.aspx");
            else
            if (Session["Lote"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 3)
                Response.Redirect("~/Pages/Estoque/CadLote.aspx");
            else
            if (Session["MovimentacaoInterna"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 4)
                Response.Redirect("~/Pages/Estoque/ManMovInterna.aspx");
            else
            if (Session["ConMovEstoque"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 5)
                Response.Redirect("~/Pages/Estoque/ConMovEstoque.aspx");
            else
            if (Session["Inventario"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 6)
                Response.Redirect("~/Pages/Estoque/AbrInventario.aspx");
            else
            if (Session["RelMovEntSai"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 7)
                Response.Redirect("~/Pages/Estoque/RelMovEntSai.aspx");
            if (Session["ComposicaoProduto"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 8)
            {
                Session["ZoomProduto"] = null;
                Session["ZoomProduto"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text);
                Response.Redirect("~/Pages/Produtos/CadComposicao.aspx");
            }
            else

                Response.Redirect("~/Pages/Produtos/CadProduto.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomProduto"] = null;
            Session["CadProduto"] = null;
            Session["CaminhoProdutoFoto0"] = null;
            Session["CaminhoProdutoFoto1"] = null;
            Session["CaminhoProdutoFoto2"] = null;
            Session["CaminhoProdutoFoto3"] = null;
            Session["CaminhoProdutoFoto4"] = null;
            Session["CaminhoProdutoFoto5"] = null;
            Response.Redirect("~/Pages/Produtos/CadProduto.aspx");
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados
            grdGrid.DataBind();
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
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (Session["Doc_orcamento"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                Response.Redirect("~/Pages/Vendas/ManItemOrcamento.aspx");
            else
                if (Session["EstoqueProduto"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 2)
                Response.Redirect("~/Pages/Estoque/CadEstoque.aspx");
            else
            if (Session["Lote"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 3)
                Response.Redirect("~/Pages/Estoque/CadLote.aspx");
            else
            if (Session["MovimentacaoInterna"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 4)
                Response.Redirect("~/Pages/Estoque/ManMovInterna.aspx");
            else
            if (Session["ConMovEstoque"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 5)
                Response.Redirect("~/Pages/Estoque/ConMovEstoque.aspx");
            else
            if (Session["Inventario"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 6)
                Response.Redirect("~/Pages/Estoque/AbrInventario.aspx");
            else
            if (Session["RelMovEntSai"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 7)
                Response.Redirect("~/Pages/Estoque/RelMovEntSai.aspx");
            if (Session["ComposicaoProduto"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 8)
                Response.Redirect("~/Pages/Produtos/CadComposicao.aspx");
            else
                Response.Redirect("~/Pages/Produtos/CadProduto.aspx");
        }
    }
}