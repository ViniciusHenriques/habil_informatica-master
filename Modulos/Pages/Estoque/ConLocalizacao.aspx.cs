﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class ConLocalizacao : System.Web.UI.Page
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
            ddlPesquisa.DataSource = d.ListarCamposSQL("LOCALIZACAO");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["ZoomLocalizacao2"] != null)
            {
                if (Session["ZoomLocalizacao2"].ToString() == "RELACIONAL")
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

                    LocalizacaoDAL r = new LocalizacaoDAL();
                    grdGrid.DataSource = r.ListarLocalizacao("", "", "", "");
                    grdGrid.DataBind();
                    if (grdGrid.Rows.Count == 0)
                        ShowMessage("Não existem Localizações Cadastradas.",MessageType.Info);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConLocalizacao.aspx");

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

                if (Session["ZoomLocalizacao2"] != null)
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
            String strTipo = d.ListarTipoCampoSQL("Localizacao", ddlPesquisa.SelectedValue).ToString().ToUpper();

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
            LocalizacaoDAL r = new LocalizacaoDAL();
            grdGrid.DataSource = r.ListarLocalizacao(ddlPesquisa.Text, strTipo, txtVarchar.Text , ddlPesquisa.Text);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ShowMessage("Localizaçõe(s) não encontrada(s) mediante a pesquisa realizada.", MessageType.Info);
        }
        protected void cboSelecionar(object sender, EventArgs e)
        {
            txtVarchar.Text = "";
            txtVarchar.Focus();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

            Session["ZoomLocalizacao"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[1].Text) + "³" + HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[2].Text);
            Response.Redirect("~/Pages/Estoque/CadLocalizacao.aspx");

        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomLocalizacao"] = null;
            Response.Redirect("~/Pages/Estoque/CadLocalizacao.aspx");
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
            if (Session["IncProdutoLocalizacao"] != null)
                Response.Redirect("~/Pages/Produtos/CadProduto.aspx");
        }
    }
}