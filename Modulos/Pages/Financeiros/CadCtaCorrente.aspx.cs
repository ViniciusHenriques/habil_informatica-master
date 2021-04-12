using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class CadCtaCorrente : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }

        List<BaixaDocumento> ListaBaixa = new List<BaixaDocumento>();

        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaTela()
        {

            CarregaTiposSituacoes();

            txtDescricao.Text = "";
            txtCodCredor.Text = "";
            txtCredor.Text = "";

            ddlBanco.Text = "..... SELECIONE UMA CONTA CORRENTE .....";




        }
        protected void CarregaTiposSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            BancoDAL ban = new BancoDAL();
            ddlBanco.DataSource = ban.ListarBancos("","","","");
            ddlBanco.DataTextField = "DescricaoBanco";
            ddlBanco.DataValueField = "CodigoBanco";
            ddlBanco.DataBind();
            ddlBanco.Items.Insert(0, "..... SELECIONE UM BANCO .....");


        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Codigo da pessoa", txtCodCredor.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);


            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCod.Focus();

                }

                return false;
            }



                return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["TabFocadaManDocCtaPagar"] != null)
            {
                PanelSelect = Session["TabFocadaManDocCtaPagar"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaManDocCtaPagar"] = "home";
            }


            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConCtaCorrente.aspx");
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
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {

                if (Session["ZoomContaCorrente"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomContaCorrente2"] != null)
                {
                    string s = Session["ZoomContaCorrente2"].ToString();
                    Session["ZoomContaCorrente2"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                txtCod.Text = word;

                                CarregaTiposSituacoes();

                                ContaCorrente conta = new ContaCorrente();
                                ContaCorrenteDAL contaDAL = new ContaCorrenteDAL();

                                conta = contaDAL.PesquisarContaCorrente(Convert.ToInt32(txtCod.Text));
                                txtDescricao.Text = conta.DescricaoContaCorrente;
                                txtCodCredor.Text = conta.CodigoPessoa.ToString();
                                SelectedCredor(sender, e);
                                ddlSituacao.SelectedValue = conta.CodigoSituacao.ToString();
                                ddlBanco.SelectedValue = conta.CodigoBanco.ToString();

                                


                            }
                        }
                    }
                }
                else
                {
                    LimpaTela();
                    txtCod.Text = "Novo";
                    btnExcluir.Visible = false;
                    lista.ForEach(delegate (Permissao p)
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
            if (Session["RetornoContaCorrente"]!= null)
            {
                if (Session["ContaCorrente2"] != null)
                {

                    string s = Session["ContaCorrente2"].ToString();
                    Session["ContaCorrente2"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                        {
                            if(word != "")
                                txtCodCredor.Text = word;
                        }
                    }
                }
                ContaCorrente cta = (ContaCorrente)Session["RetornoContaCorrente"];
                if (cta.CodigoBanco == 0)
                    ddlBanco.SelectedValue = "..... SELECIONE UM BANCO .....";
                else
                    ddlBanco.SelectedValue = cta.CodigoBanco.ToString();

                SelectedCredor(sender, e);

                txtDescricao.Text = cta.DescricaoContaCorrente;
                if (cta.CodigoContaCorrente == 0)
                {
                    txtCod.Text = "Novo";
                    btnExcluir.Visible = false;
                }
                else
                    txtCod.Text = cta.CodigoContaCorrente.ToString();

                ddlSituacao.SelectedValue = cta.CodigoSituacao.ToString();

                Session["RetornoContaCorrente"] = null;
            }
            if (txtCod.Text == "")
                btnVoltar_Click(sender, e);
        }
        
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCod.Text.Trim() != "")
                {
                    ContaCorrenteDAL d = new ContaCorrenteDAL();
                    d.Excluir(Convert.ToInt32(txtCod.Text));
                    Session["MensagemTela"] = "Conta corrente excluído com Sucesso!!!";

                    btnVoltar_Click(sender, e);

                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da conta corrente não identificado.&emsp;&emsp;&emsp;";

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
            if (Session["Baixa"] != null )
            {
                if (Session["Ssn_TipoPessoa"] != null)
                    Response.Redirect("~/Pages/Financeiros/ManCtaBaixa.aspx?cad=1");
                else if(Session["Ssn_CtaReceber"] != null)
                    Response.Redirect("~/Pages/Financeiros/ManCtaBaixa.aspx?cad=2");
            }
            else
            { 
                Response.Redirect("~/Pages/Financeiros/conCtaCorrente.aspx");
            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {


            if (ValidaCampos() == false)
                return;

            ContaCorrente cta = new ContaCorrente();
            ContaCorrenteDAL ctaDAL = new ContaCorrenteDAL();

            cta.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            cta.DescricaoContaCorrente = txtDescricao.Text.ToUpper();
            if(ddlBanco.SelectedValue == "..... SELECIONE UM BANCO .....")
            {
                cta.CodigoBanco = 0;
            }
            else
            {
                cta.CodigoBanco = Convert.ToInt32(ddlBanco.SelectedValue);
            }
            
            cta.CodigoPessoa = Convert.ToInt32(txtCodCredor.Text);


            if (txtCod.Text == "Novo")
            {
                ctaDAL.Inserir(cta);
                Session["MensagemTela"] = "Conta Corrente cadastrada com sucesso!";
                if (Session["Ssn_TipoPessoa"] != null)
                {
                    Response.Redirect("~/Pages/Financeiros/ManCtaBaixa.aspx");
                }
            }
            else
            {
                cta.CodigoContaCorrente = Convert.ToInt32(txtCod.Text);
                ctaDAL.Atualizar(cta);

                Session["MensagemTela"] = "Conta Corrente Alterado com Sucesso!!!";
            }


            btnVoltar_Click(sender, e);




        }
        protected void ConCredor(object sender, EventArgs e)
        {
            ContaCorrente CTA = new ContaCorrente();
            if (txtCod.Text == "Novo")
                CTA.CodigoContaCorrente = 0;
            else
                CTA.CodigoContaCorrente = Convert.ToInt32(txtCod.Text);

            CTA.DescricaoContaCorrente = txtDescricao.Text;
            CTA.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            decimal numero = Convert.ToDecimal(CTA.CodigoBanco);
            if (ddlBanco.SelectedValue == "..... SELECIONE UM BANCO .....")
            {
                CTA.CodigoBanco = 0;
            }
            else
            {
                CTA.CodigoBanco = Convert.ToInt32(ddlBanco.SelectedValue);
            }

            Session["RetornoContaCorrente"] = CTA;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=2");

        }
        protected void SelectedCredor(object sender, EventArgs e)
        {

            Boolean blnCampo = false;

            if (txtCodCredor.Text.Equals(""))
            {

                return;
            }
            else
            {
                v.CampoValido("Codigo Credor", txtCodCredor.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (!blnCampo)
                {
                    txtCodCredor.Text = "";
                    return;

                }


            }



            Int64 codigoCredor = Convert.ToInt64(txtCodCredor.Text);
            PessoaDAL pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();

            PessoaInscricaoDAL ins = new PessoaInscricaoDAL();
            List<Pessoa_Inscricao> ins2 = new List<Pessoa_Inscricao>();
            ins2 = ins.ObterPessoaInscricoes(codigoCredor);
            p2 = pessoa.PesquisarPessoa(codigoCredor);

            if (p2 == null)
            {
                ShowMessage("Pessoa não existente!", MessageType.Info);
                txtCodCredor.Text = "";
                txtCredor.Text = "";
                txtCodCredor.Focus();

                return;
            }
            txtCredor.Text = p2.NomePessoa;

        }



    }
}