using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Impostos
{
    public partial class CadGpoTribPessoa : System.Web.UI.Page
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

            v.CampoValido("Descrição do Grupo de Tributação", txtDescricao.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

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

            if (Session["ZoomGpoTribPessoa2"] != null)
            {
                if (Session["ZoomGpoTribPessoa2"].ToString() == "RELACIONAL")
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
                                               "ConGpoTribPessoa.aspx");

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    if (Session["ZoomGpoTribPessoa2"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomGpoTribPessoa"] != null)
                    {
                        string s = Session["ZoomGpoTribPessoa"].ToString();
                        Session["ZoomGpoTribPessoa"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            btnExcluir.Visible = true;
                            foreach (string word in words)
                                if (txtCodigo.Text == "")
                                {
                                    txtCodigo.Text = word;
                                    txtCodigo.Enabled = false;

                                    GpoTribPessoaDAL r = new GpoTribPessoaDAL();
                                    GpoTribPessoa p = new GpoTribPessoa();

                                    p = r.PesquisarGpoTribPessoa(Convert.ToInt32(txtCodigo.Text));

                                    txtDescricao.Text = p.DescricaoGpoTribPessoa;

                                    if (p.Icms==1)
                                        chkICMS.Checked =true;
                                    else
                                        chkICMS.Checked = false;

                                    if (p.IPI == 1)
                                        chkIPI.Checked = true;
                                    else
                                        chkIPI.Checked = false;


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
                    GpoTribPessoaDAL d = new GpoTribPessoaDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Grupo de Tributação não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomGpoTribPessoa"] = null;
            if (Session["ZoomGpoTribPessoa2"] != null)
            {
                Session["ZoomGpoTribPessoa2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadGpoTribPessoa.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return; 
            }

            Response.Redirect("~/Pages/Impostos/ConGpoTribPessoa.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            GpoTribPessoaDAL d = new GpoTribPessoaDAL();
            GpoTribPessoa p = new GpoTribPessoa();

            p.DescricaoGpoTribPessoa = txtDescricao.Text.ToUpper();

            if (chkICMS.Checked)
                p.Icms = 1;
            else
                p.Icms = 0;

            if (chkIPI.Checked)
                p.IPI= 1;
            else
                p.IPI = 0;

            if (txtCodigo.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p.CodigoGpoTribPessoa = Convert.ToInt32(txtCodigo.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }

            if (Session["IncCEP"] != null)
            {
                Session["MensagemTela"] = null;
                Response.Redirect("~/Pages/CEPs/CadCEP.aspx");

            }
            else
                btnVoltar_Click(sender, e);

        }
    }
}