using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class CadEmbRetornavel : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        //string strMensagemR = "";
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaCampos()
        {
            txtLancamento.Text = "Novo";
            txtCodBarras.Text = "";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtLancamento.Focus();

            if (Session["ZoomEstoqueProduto2"] != null)
            {
                if (Session["ZoomEstoqueProduto2"].ToString() == "RELACIONAL")
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

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = "";
            }


            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt16(Session["CodModulo"].ToString()),
                                           Convert.ToInt16(Session["CodPflUsuario"].ToString()),
                                           "ConEmbRetornavel.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                txtLancamento.Text = "";
                if (Session["ZoomEmbalagemRetornavel2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomEmbalagemRetornavel"] != null)
                {
                    string s = Session["ZoomEmbalagemRetornavel"].ToString();
                    Session["ZoomEmbalagemRetornavel"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                        if (txtLancamento.Text == "")
                        {
                            MontaSituacao();
                            txtLancamento.Text = word;
                            txtLancamento.Enabled = false;

                            EmbalagemRetornavel p = new EmbalagemRetornavel();
                            EmbalagemRetornavelDAL RnEmb = new EmbalagemRetornavelDAL();

                            p = RnEmb.PesquisarEmbalagem(Convert.ToInt32(txtLancamento.Text));

                            txtLancamento.Text = p.CodigoIndice.ToString();
                            txtCodBarras.Text = p.CodigoEmbalagem.ToString();
                            ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();

                            Session["ZoomEmbalagemRetornavel3"] = p;
                            lista.ForEach(delegate (Permissao x)
                            {
                                if (!x.AcessoCompleto)
                                {
                                    if (!x.AcessoAlterar)
                                        btnSalvar.Visible = false;

                                    if (!x.AcessoExcluir)
                                        btnExcluir.Visible = false;
                                }
                            }
                            );
                            return;
                        }
                    }
                }
                else
                {
                    MontaSituacao();
                    txtLancamento.Text = "Novo";
                    btnExcluir.Visible = false;

                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = true;
                        }
                    });
                }
            }
            if (Session["CodUsuario"] == "-150380")
            {
                btnSalvar.Visible = true;
            }

            if (txtLancamento.Text == "")
                btnVoltar_Click(sender, e);
        }
        protected void txtCodBarras_TextChanged(object sender, EventArgs e)
        {
            EmbalagemRetornavelDAL RnEmb = new EmbalagemRetornavelDAL();
            EmbalagemRetornavel p = new EmbalagemRetornavel();

            int indice = 0;

            if (txtLancamento.Text == "Novo")
                indice = 0;
            else
                indice = Convert.ToInt32(txtLancamento.Text);

            p = RnEmb.EmbalagemExistente(indice, txtCodBarras.Text);

            if (p.CodigoEmbalagem == txtCodBarras.Text)
            {
                ShowMessage("Código de Barras já existente", MessageType.Info);
                txtCodBarras.Text = "";
            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["PVez"] = null;
            Response.Redirect("~/Pages/Estoque/ConEmbRetornavel.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            EmbalagemRetornavel p = new EmbalagemRetornavel();
            EmbalagemRetornavelDAL RnEmb = new EmbalagemRetornavelDAL();

            Boolean blnCampoValido = false;
            string strMensagemR = "";
            v.CampoValido("Código de Barras", txtCodBarras.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtCodBarras.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodBarras.Focus();
                    return;
                }
            }

            if (txtLancamento.Text == "Novo")
            {
                p.CodigoEmbalagem = txtCodBarras.Text;
                p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                RnEmb.Inserir(p);
                Session["MensagemTela"] = "Embalagem Retornavel Inserida com sucesso!";
                Response.Redirect("~/Pages/Estoque/ConEmbRetornavel.aspx");
            }
            else
            {
                p.CodigoIndice = Convert.ToInt32(txtLancamento.Text);
                p.CodigoEmbalagem = txtCodBarras.Text;
                p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                RnEmb.Atualizar(p);
                Session["MensagemTela"] = "Embalagem Retornavel Atualizada com sucesso!";
                Response.Redirect("~/Pages/Estoque/ConEmbRetornavel.aspx");
            }
            Session["PVez"] = null;
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            EmbalagemRetornavelDAL RnEmb = new EmbalagemRetornavelDAL();
            RnEmb.Excluir(Convert.ToInt32(txtLancamento.Text));
            Session["MensagemTela"] = "Embalagem Retornavel Excluído com sucesso!";
            Session["PVez"] = null;
            Response.Redirect("~/Pages/Estoque/ConEmbRetornavel.aspx");

        }
        protected void MontaSituacao()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.SituacaoEmbalagem();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (txtLancamento.Text == "Novo")
                return;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "abc();", true);
        }
    }
}