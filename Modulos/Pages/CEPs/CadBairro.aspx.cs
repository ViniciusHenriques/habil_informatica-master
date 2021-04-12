using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.CEPs
{
    public partial class CadBairro : System.Web.UI.Page
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

            v.CampoValido("Descrição do Bairro", txtDescricao.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDescricao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage (strMensagemR,MessageType.Info);
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

            if (Session["ZoomBairro2"] != null)
            {
                if (Session["ZoomBairro2"].ToString() == "RELACIONAL")
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
                                               "ConBairro.aspx");

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    if (Session["ZoomBairro2"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomBairro"] != null)
                    {
                        string s = Session["ZoomBairro"].ToString();
                        Session["ZoomBairro"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            btnExcluir.Visible = true;
                            foreach (string word in words)
                                if (txtCodigo.Text == "")
                                {
                                    txtCodigo.Text = word;
                                    txtCodigo.Enabled = false;

                                    BairroDAL r = new BairroDAL();
                                    Bairro p = new Bairro();

                                    p = r.PesquisarBairro(Convert.ToInt32(txtCodigo.Text));

                                    txtDescricao.Text = p.DescricaoBairro;

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

            if (Session["CodUsuario"] == "-150380")
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
                    BairroDAL d = new BairroDAL();
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
            Session["ZoomBairro"] = null;
            if (Convert.ToInt32(Request.QueryString["Cad"]) == 1)
            {
                Response.Redirect("~/Pages/CEPs/CadCEP.aspx");

            }
            if (Session["ZoomBairro2"] != null)
            {
                Session["ZoomBairro2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadBairro.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return; 
            }

            Response.Redirect("~/Pages/CEPs/ConBairro.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            BairroDAL d = new BairroDAL();
            Bairro p = new Bairro();

            p.DescricaoBairro = txtDescricao.Text.ToUpper();

            if (txtCodigo.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p.CodigoBairro = Convert.ToInt32(txtCodigo.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }

            if (Convert.ToInt32(Request.QueryString["cad"])==1)
            {
                Session["MensagemTela"] = null;
                Response.Redirect("~/Pages/CEPs/CadCEP.aspx");

            }
            else
                btnVoltar_Click(sender, e);

        }
    }
}