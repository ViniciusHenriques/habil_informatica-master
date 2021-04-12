using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
namespace SoftHabilInformatica.Pages.Pessoas

{
    public partial class CadGpoPessoa : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();

        List<GrupoPessoa> listCadTipoServico = new List<GrupoPessoa>();

        private List<Pessoa> listCadPessoa = new List<Pessoa>();
        private List<Pessoa_Inscricao> listCadPessoaInscricao = new List<Pessoa_Inscricao>();
        private List<Pessoa_Endereco> listCadPessoaEndereco = new List<Pessoa_Endereco>();
        private List<Pessoa_Contato> listCadPessoaContato = new List<Pessoa_Contato>();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaCampos()
        {

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }


            Boolean blnCampoValido = false;

            v.CampoValido("Descrição", txtDescricao.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }




            return true;
        }

        protected void CarregaSituacoes()
        {
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

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomTipoServico2"] != null)
            {
                if (Session["ZoomTipoServico2"].ToString() == "RELACIONAL")
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
                cmdSair.Visible = true;
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();

            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConGpoPessoa.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomGrupoPessoa2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomGrupoPessoa"] != null)
                {
                    string s = Session["ZoomGrupoPessoa"].ToString();
                    Session["ZoomGrupoPessoa"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                GrupoPessoaDAL r = new GrupoPessoaDAL();
                                GrupoPessoa p = new GrupoPessoa();

                                CarregaSituacoes();
                                p = r.PesquisarGrupoPessoa(Convert.ToInt32(txtCodigo.Text));
                                txtCodigo.Text = p.CodigoGrupoPessoa.ToString();
                                txtDescricao.Text = p.DescricaoGrupoPessoa.ToString();
                                txtCodSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                chkGerarMatricula.Checked = p.GerarMatricula;

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
                    LimpaCampos();

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
            if(txtCodigo.Text == "")
                btnVoltar_Click(sender, e);

            if (Session["CodUsuario"].ToString() == "-150380")
            {
                btnSalvar.Visible = true;
            }
            txtDescricao.Focus();
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void LimpaCampos()
        {

            txtCodigo.Text = "Novo";
            txtCodigo.Enabled = false;
            txtDescricao.Text = "";
            chkGerarMatricula.Checked = false;

            CarregaSituacoes();

            txtCodSituacao.SelectedValue = "1";
            //Session["ListaItemTipoServico"] = null;

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    GrupoPessoaDAL d = new GrupoPessoaDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Tipo de Serviço excluído com Sucesso!!!";

                    btnVoltar_Click(sender, e);

                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Tipo de Serviço não identificado.&emsp;&emsp;&emsp;";

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
        protected void btnVoltar_Click(object sender, EventArgs e)
        {

            Session["ZoomGrupoPessoa"] = null;
            if (Session["ZoomGrupoPessoa2"] != null)
            {
                Session["ZoomGrupoPessoa2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadGpoPessoa.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Session["InscricaoGrupoPessoa"] = null;

            listCadTipoServico = null;

            if (Session["RetornoCadPessoa"] != null)
            {
                Session["TabFocada"] = "parameter";
                if (Request.QueryString["Cad"] != null)
                    Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx?cad=" + Request.QueryString["Cad"]);
                else
                    Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
            }
            else
                Response.Redirect("~/Pages/Pessoas/ConGpoPessoa.aspx");


        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
            {
                return;
            }

            GrupoPessoaDAL d = new GrupoPessoaDAL();
            GrupoPessoa p = new GrupoPessoa();

            p.CodigoSituacao = Convert.ToInt16(txtCodSituacao.SelectedValue);
            p.DescricaoGrupoPessoa = txtDescricao.Text.ToUpper();
            p.GerarMatricula = chkGerarMatricula.Checked;

            if (txtCodigo.Text == "Novo")
            {
                d.Inserir(p);
                // Session["MensagemTela"] = "Grupo de Pessoa Incluído com Sucesso!!!";
                if (Session["RetornoCadPessoa"] != null)
                {
                    Session["TabFocada"] = "parameter";
                    Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
                }
            }
            else
            {
                p.CodigoGrupoPessoa = Convert.ToInt32(txtCodigo.Text);
                d.Atualizar(p);

                Session["MensagemTela"] = "Grupo de Pessoa Alterado com Sucesso!!!";
            }


            btnVoltar_Click(sender, e);


        }

    }




}