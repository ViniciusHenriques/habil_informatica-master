using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class CadUnidade : System.Web.UI.Page
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

            v.CampoValido("Descrição da Unidade", txtDescricao.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDescricao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage (strMensagemR, MessageType.Info);
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

            if (Session["ZoomUnidade2"] != null)
            {
                if (Session["ZoomUnidade2"].ToString() == "RELACIONAL")
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
                                               "ConUnidade.aspx");

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    if (Session["ZoomUnidade2"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomUnidade"] != null)
                    {
                        string s = Session["ZoomUnidade"].ToString();
                        Session["ZoomUnidade"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            btnExcluir.Visible = true;
                            foreach (string word in words)
                                if (txtCodigo.Text == "")
                                {
                                    txtCodigo.Text = word;
                                    txtCodigo.Enabled = false;

                                    UnidadeDAL r = new UnidadeDAL();
                                    Unidade p = new Unidade();

                                    p = r.PesquisarUnidade(Convert.ToInt32(txtCodigo.Text));

                                    txtDescricao.Text = p.DescricaoUnidade;

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
                        txtCodigo.Text = "Novo";
                        btnExcluir.Visible = false;
                        lista.ForEach(delegate(Permissao p)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoIncluir) 
                                    btnSalvar.Visible = false;
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
                    UnidadeDAL d = new UnidadeDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Unidade não identificado.&emsp;&emsp;&emsp;";

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

            //exemplo. mas não era para estar aqui
            if (Session["IncProdutoUnidade"] != null)
            {
                Session["MensagemTela"] = null;
                Session["ZoomUnidade2"] = null;
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                Response.Redirect("~/Pages/Produtos/CadProduto.aspx");

            }
                Response.Redirect("~/Pages/Produtos/ConUnidade.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            UnidadeDAL d = new UnidadeDAL();
            Unidade p = new Unidade();

            p.DescricaoUnidade = txtDescricao.Text.ToUpper();
            p.SiglaUnidade = txtSigla.Text.ToUpper();

            if (txtCodigo.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Unidade incluída com Sucesso!!!";
            }
            else
            {
                p.CodigoUnidade = Convert.ToInt32(txtCodigo.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Unidade alterada com Sucesso!!!";
            }

            btnVoltar_Click(sender, e);

        }
    }
}