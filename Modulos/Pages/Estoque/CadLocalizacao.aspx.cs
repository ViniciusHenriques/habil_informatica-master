using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class CadLocalizacao : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        public string MaskLocalizacao { get; set; }

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void MontaDropDownList()
        {
            Habil_TipoDAL RnSituacao = new Habil_TipoDAL();
            EmpresaDAL RnEmpresa = new EmpresaDAL();

            ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("","","","");
            ddlEmpresa.DataTextField = "NomeFantasia";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();

            if (Session["CodEmpresa"] != null)
            {
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
                ddlEmpresa.Enabled = false;
            }
            else
                ddlEmpresa.Enabled = true;


            ddlSituacao.DataSource = RnSituacao.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();


            ddlTipoLocalizacao.DataSource = RnSituacao.TipoLocalizacao();
            ddlTipoLocalizacao.DataTextField = "DescricaoTipo";
            ddlTipoLocalizacao.DataValueField = "CodigoTipo";
            ddlTipoLocalizacao.DataBind();
        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            // Validar conforme a critica data pela caracterista

            v.CampoValido("Código da Localização",  txtCodigo.Text,  true, false, false, false, "NVARCHAR", ref blnCampoValido, ref strMensagemR);

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
            else
            {
                LocalizacaoDAL d = new LocalizacaoDAL();
                Localizacao p = new Localizacao();
                p = d.PesquisarLocalizacao(Convert.ToInt32(ddlEmpresa.SelectedValue),txtCodigo.Text);

                if ((txtLancamento.Text == "Novo") && (p != null))
                {
                    ShowMessage("Código da Localização já Cadastrado.", MessageType.Info);
                    txtDescricao.Focus();

                    return false;
                }
            }

            v.CampoValido("Descrição da Localização", txtDescricao.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

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
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            Int32 CodEmpresa = 0;

            if (Session["CodEmpresa"] != null)
                CodEmpresa = Convert.ToInt32(Session["CodEmpresa"].ToString());

            ParSistemaDAL RnParSis = new ParSistemaDAL();
            MaskLocalizacao = @RnParSis.FormataLocalizacao(CodEmpresa);

            txtDescricao.Focus();

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
                                               "ConLocalizacao.aspx");

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    if (Session["ZoomLocalizacao2"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomLocalizacao"] != null)
                    {
                        string s = Session["ZoomLocalizacao"].ToString();
                        Session["ZoomLocalizacao"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            MontaDropDownList();
                            btnExcluir.Visible = true;
                            LocalizacaoDAL r = new LocalizacaoDAL();
                            Localizacao p = new Localizacao();

                            p = r.PesquisarLocalizacao(Convert.ToInt32(words[0].ToString()), words[1].ToString());
                            ddlEmpresa.Enabled = false;
                            txtLancamento.Text = p.CodigoIndice.ToString();
                            ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
                            txtDescricao.Text = p.DescricaoLocalizacao;
                            txtCodigo.Text = p.CodigoLocalizacao;
                            ddlEmpresa.SelectedValue = p.CodigoSituacao.ToString();
                            ddlTipoLocalizacao.SelectedValue = p.CodigoTipoLocalizacao.ToString();
                            txtCodigo.Enabled = false;
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
                    else
                    {
                        MontaDropDownList();
                        txtLancamento.Text = "Novo";
                        txtLancamento.Enabled = false;

                        txtCodigo.Text = "";
                        txtCodigo.Enabled = true;
                        txtCodigo.Focus();
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
            if (txtLancamento.Text == "")
                btnVoltar_Click(sender, e);
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    LocalizacaoDAL d = new LocalizacaoDAL();
                    d.Excluir(Convert.ToInt32(ddlEmpresa.SelectedValue), txtCodigo.Text);
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Localização não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomLocalizacao"] = null;
            Response.Redirect("~/Pages/Estoque/ConLocalizacao.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (ValidaCampos() == false)
                    return;


                LocalizacaoDAL d = new LocalizacaoDAL();
                Localizacao p = new Localizacao();

                p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
                p.DescricaoLocalizacao = txtDescricao.Text.ToUpper();
                p.CodigoLocalizacao = txtCodigo.Text;
                p.CodigoSituacao = Convert.ToInt32(ddlEmpresa.SelectedValue);
                p.CodigoTipoLocalizacao = Convert.ToInt32(ddlTipoLocalizacao.SelectedValue);

                if (txtLancamento.Text== "Novo")
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Localização inclusa com Sucesso!!!";
                }
                else
                {
                    p.CodigoIndice= Convert.ToInt32( txtLancamento.Text);
                    d.Atualizar(p);
                    Session["MensagemTela"] = "Localização alterada com Sucesso!!!";
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