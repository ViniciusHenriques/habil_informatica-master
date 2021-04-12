using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Impostos
{
    public partial class ConCOFINS : System.Web.UI.Page
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
            ddlPesquisa.DataSource = d.ListarCamposSQL("COFINS");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["ZoomCOFINS2"] != null)
            {
                if (Session["ZoomCOFINS2"].ToString() == "RELACIONAL")
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

                COFINSDAL r = new COFINSDAL();
                grdGrid.DataSource = r.ListarCOFINS("", "", "", "");
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                    ShowMessage("Não existem COFINS Cadastrados.", MessageType.Info);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConCOFINS.aspx");

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

                if (Session["ZoomCOFINS2"] != null)
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
            String strTipo = d.ListarTipoCampoSQL("COFINS", ddlPesquisa.SelectedValue).ToString().ToUpper();

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
            COFINSDAL r = new COFINSDAL();
            grdGrid.DataSource = r.ListarCOFINS(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ShowMessage("COFINS(s) não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text = "";
            txtVarchar.Focus();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

            ///           if (Session["IncProdutoCOFINS"] != null)
            ///            {
            ///                List<Produto> listCadProduto = new List<Produto>();
            ///                listCadProduto = (List<Produto>)Session["IncProdutoCOFINS"];
            ///                listCadProduto[0].CodigoCOFINS = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[1].Text);
            ///                Session["IncProdutoCOFINS"] = listCadProduto;
            ///                Response.Redirect("~/Pages/Produtos/CadProduto.aspx");
            ///            }
            ///            else
            ///            { 
            Session["ZoomCOFINS"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/Pages/Impostos/CadCOFINS.aspx");
            ///            }

        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomCOFINS"] = null;
            Response.Redirect("~/Pages/Impostos/CadCOFINS.aspx");
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
            if (Session["IncProdutoCOFINS"] != null)
                Response.Redirect("~/Pages/Produtos/CadProduto.aspx");
        }
    }
}