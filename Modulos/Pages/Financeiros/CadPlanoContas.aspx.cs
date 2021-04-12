using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class CadPlanoContas : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código Reduzido", txtCodReduzido.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodReduzido.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodReduzido.Focus();

                }

                return false;
            }

            v.CampoValido("Descrição do Plano de Contas", txtDescricao.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDescricao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR,MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }

            return true;
        }
        protected void LimpaCampos()
        {
            txtCodigo.Text = "Novo";
            txtDescricao.Text = "";
            txtCodReduzido.Text = "";

            Habil_TipoDAL sd = new Habil_TipoDAL();
            txtCodSituacao.DataSource = sd.Atividade();
            txtCodSituacao.DataTextField = "DescricaoTipo";
            txtCodSituacao.DataValueField = "CodigoTipo";
            txtCodSituacao.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtCodigo.Enabled = false;

            if (Session["ZoomPlanoContas2"] != null)
            {
                if (Session["ZoomPlanoContas2"].ToString() == "RELACIONAL")
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
                ShowMessage(Session["MensagemTela"].ToString(),MessageType.Info);
                Session["MensagemTela"] = "";
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConPlanoContas.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomPlanoContas2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomPlanoContas"] != null)
                {
                    string s = Session["ZoomPlanoContas"].ToString();
                    Session["ZoomPlanoContas"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                LimpaCampos();

                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                PlanoContasDAL r = new PlanoContasDAL();
                                PlanoContas p = new PlanoContas();

                                p = r.PesquisarPlanoConta(Convert.ToInt64(txtCodigo.Text));

                                txtDescricao.Text = p.DescricaoPlanoConta;
                                txtCodSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                txtCodReduzido.Text = p.CodigoReduzido;
                                                                                             
                                lista.ForEach(delegate(Permissao x)
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
                LimpaCampos();
                txtDescricao.Focus();

                    if (Session["IncProdutoPlanoContas"] != null)
                    {
                        txtCodigo.Enabled = true;
                        txtCodigo.Focus();
                        btnExcluir.Visible = false;
                        return;
                    }
                    else
                    {
                        txtCodigo.Text = "Novo";
                        txtCodigo.Enabled = false ;
                        txtDescricao.Focus();
                        btnExcluir.Visible = false;
                    }

                    lista.ForEach(delegate(Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                            if (!p.AcessoExcluir)
                                btnExcluir.Visible = false;
                        }
                    });

                }
            }
            if(txtCodigo.Text == "")
                btnVoltar_Click(sender, e);
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    PlanoContasDAL d = new PlanoContasDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text.Replace(".", "")));
                    Session["MensagemTela"] = "Plano de Conta excluída com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Plano de Conta não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomPlanoContas"] = null;
            Response.Redirect("~/Pages/Financeiros/ConPlanoContas.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (ValidaCampos() == false)
                    return;


                PlanoContasDAL d = new PlanoContasDAL();
                PlanoContas p = new PlanoContas();

                p.DescricaoPlanoConta = txtDescricao.Text.ToUpper();
                p.CodigoSituacao = Convert.ToInt32(txtCodSituacao.SelectedValue);
                p.CodigoReduzido = txtCodReduzido.Text;


                if (txtCodigo.Text == "Novo")
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Plano de Conta incluso com Sucesso!!!";
                }
                else
                {
                    p.CodigoPlanoConta = Convert.ToInt64(txtCodigo.Text);
                    d.Atualizar(p);
                    Session["MensagemTela"] = "Plano de Conta alterado com Sucesso!!!";
                }

                btnVoltar_Click(sender, e);

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

    }
}