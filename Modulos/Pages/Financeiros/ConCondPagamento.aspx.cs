using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class ConCondPagamento : System.Web.UI.Page
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
            ddlPesquisa.DataSource = d.ListarCamposSQL("CONDICAO_DE_PAGAMENTO");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["ZoomCondPagamento2"] != null)
            {
                if (Session["ZoomCondPagamento2"].ToString() == "RELACIONAL")
                {
                    pnlPainel.Visible = false;
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
                cmdSair.Visible = false;
            }

            
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

                    CondPagamentoDAL r = new CondPagamentoDAL();
                    grdGrid.DataSource = r.ListarCondPagamento("", "", "", "");
                    grdGrid.DataBind();
                    if (grdGrid.Rows.Count == 0)
                        ShowMessage("Não existem Condições de Pagamento Cadastrados.",MessageType.Info);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConCondPagamento.aspx");
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

                if (Session["ZoomCondPagamento2"] != null)
                {
                    //btnImprimir.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true; 
                 }
                else
                    btnVoltar.Visible = false; 

            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = d.ListarTipoCampoSQL("CONDICAO_DE_PAGAMENTO", ddlPesquisa.SelectedValue).ToString().ToUpper();

            Boolean blnCampoValido = false;
            v.CampoValido(ddlPesquisa.SelectedValue.ToString().ToUpper(), txtVarchar.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR,MessageType.Info);
                    txtVarchar.Focus();
                    return;
                }
            }
            CondPagamentoDAL r = new CondPagamentoDAL();
            grdGrid.DataSource = r.ListarCondPagamento(ddlPesquisa.Text, strTipo, txtVarchar.Text , ddlPesquisa.Text);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ShowMessage("Condições(s) de Pagamento não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text = "";
            txtVarchar.Focus();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (Session["IncProdutoCondPagamento"] != null)
            //{
            //    List<Produto> listCadProduto = new List<Produto>();
            //    listCadProduto = (List<Produto>)Session["IncProdutoCondPagamento"];
            //    listCadProduto[0].CodigoCondPagamento = Convert.ToInt64 (HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text));
            //    Session["IncProdutoCondPagamento"] = listCadProduto;
            //    Response.Redirect("~/Pages/Produto/CadProduto.aspx");
            //}
            //else
            //{
                Session["ZoomCondPagamento"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
                Response.Redirect("~/Pages/Financeiros/CadCondPagamento.aspx");
            //}

        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomCondPagamento"] = null;
            Response.Redirect("~/Pages/Financeiros/CadCondPagamento.aspx");
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
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
           // if (Session["IncProdutoCondPagamento"] != null)
           //     Response.Redirect("~/Pages/Produto/CadProduto.aspx");
        }
    }
}