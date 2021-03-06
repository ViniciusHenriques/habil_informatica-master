using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class CadGpoComissao : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();
        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Descrição do Grupo de Comissao", txtDescricao.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDescricao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }

            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtDescricao.Focus();

            if (Session["ZoomGpoComissao2"] != null)
            {
                if (Session["ZoomGpoComissao2"].ToString() == "RELACIONAL")
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
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                           Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                           "ConGpoComissao.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomGpoComissao2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomGpoComissao"] != null)
                {
                    string s = Session["ZoomGpoComissao"].ToString();
                    Session["ZoomGpoComissao"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                GpoComissaoDAL r = new GpoComissaoDAL();
                                GpoComissao p = new GpoComissao();
                                CarregaSituacoes();

                                p = r.PesquisarGpoComissao(Convert.ToInt32(txtCodigo.Text));
                                ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();

                                txtDescricao.Text = p.DescricaoGpoComissao;

                                lista.ForEach(delegate (Permissao x)
                                {
                                    if (!x.AcessoCompleto)
                                    {
                                        if (!x.AcessoAlterar)
                                            btnSalvar.Visible = false;

                                        if (!x.AcessoExcluir)
                                            btnExcluir.Visible = false;
                                    }
                                });

                                return;
                            }
                    }
                }
                else
                {
                    CarregaSituacoes();
                    txtCodigo.Text = "Novo";
                    btnExcluir.Visible = false;
                    lista.ForEach(delegate (Permissao p)

                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                        }
                    });

                }
            }

            if (Session["CodUsuario"].ToString() == "-150380")
            {
                btnSalvar.Visible = true;
            }

            if (txtCodigo.Text == "")
                btnVoltar_Click(sender, e);

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    GpoComissaoDAL d = new GpoComissaoDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Bairro não identificado.&emsp;&emsp;&emsp;";

            }
            catch (Exception ex)
            {
                strErro = "&emsp;&emsp;&emsp;" + ex.Message.ToString() + "&emsp;&emsp;&emsp;";
            }

            if (strErro != "")
            {
                lblMensagem.Text = strErro;
                pnlMensagem.Visible = true;
            }
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomGpoComissao"] = null;
            if (Convert.ToInt32(Request.QueryString["Cad"]) == 1)
            {
                Response.Redirect("~/Pages/Produtos/CadGpoComissao.aspx");

            }
            if (Session["ZoomGpoComissao2"] != null)
            {
                Session["ZoomGpoComissao2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadGpoComissao.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Response.Redirect("~/Pages/Produtos/ConGpoComissao.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            GpoComissaoDAL d = new GpoComissaoDAL();
            GpoComissao p = new GpoComissao();

            p.DescricaoGpoComissao = txtDescricao.Text.ToUpper();
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);

            if (txtCodigo.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p.CodigoGpoComissao = Convert.ToInt32(txtCodigo.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }

            if (Convert.ToInt32(Request.QueryString["cad"]) == 1)
            {
                Session["MensagemTela"] = null;
                Response.Redirect("~/Pages/Produtos/CadCEP.aspx");

            }
            else
                btnVoltar_Click(sender, e);

        }
    }
}