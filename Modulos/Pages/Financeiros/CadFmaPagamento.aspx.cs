using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class CadFmaPagamento : System.Web.UI.Page
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

            v.CampoValido("Descrição da Forma de Pagamento", txtDescricao.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

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

            if (DdlFormaPagamento.Text == "..... SELECIONE UMA FORMA DE PAGAMENTO .....")
            {
                ShowMessage("Selecione uma Forma de Pagamento da NF-e", MessageType.Info);
                return false;
            }

            if ((DdlBandeira.Enabled == true) && (DdlBandeira.Text == "..... SELECIONE UMA BANDEIRA DE CARTÃO ....."))
            {
                ShowMessage("Selecione uma Bandeira de Cartão da NF-e", MessageType.Info);
                return false;
            }

            return true;
        }
        protected void LimpaCampos()
        {
            txtCodigo.Text = "Novo";
            txtDescricao.Text = "";

            DdlBandeira.Enabled = false;

            FmaPagamentoNFEDAL d = new FmaPagamentoNFEDAL();
            DdlFormaPagamento.DataSource = d.ListaFmaPagamentoNFE();
            DdlFormaPagamento.DataTextField = "DescricaoFmaPagamentoNFE";
            DdlFormaPagamento.DataValueField = "CodigoFmaPagamentoNFE";
            DdlFormaPagamento.DataBind();
            DdlFormaPagamento.Items.Insert(0, "..... SELECIONE UMA FORMA DE PAGAMENTO .....");

            DdlFormaPagamento_SelectedIndexChanged(null, null);

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

            txtDescricao.Focus();
            txtCodigo.Enabled = false;

            if (Session["ZoomFmaPagamento2"] != null)
            {
                if (Session["ZoomFmaPagamento2"].ToString() == "RELACIONAL")
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
                                            "ConFmaPagamento.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomFmaPagamento2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomFmaPagamento"] != null)
                {
                    string s = Session["ZoomFmaPagamento"].ToString();
                    Session["ZoomFmaPagamento"] = null;

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

                                FmaPagamentoDAL r = new FmaPagamentoDAL();
                                FmaPagamento p = new FmaPagamento();

                                p = r.PesquisarFmasPagamento(Convert.ToInt32(txtCodigo.Text));

                                txtDescricao.Text = p.DescricaoFmaPagamento;

                                DdlFormaPagamento.SelectedValue = p.CodigoFmaPagamentoNFE;
                                DdlFormaPagamento_SelectedIndexChanged(sender, e);
                                DdlBandeira.SelectedValue = p.CodigoBandeiraNFE;
                                txtCodSituacao.SelectedValue = p.CodigoSituacao.ToString();

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

                    if (Session["IncProdutoFmaPagamento"] != null)
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
                    FmaPagamentoDAL d = new FmaPagamentoDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text.Replace(".", "")));
                    Session["MensagemTela"] = "Forma de Pagamento excluída com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Forma de Pagamento não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomFmaPagamento"] = null;
            Response.Redirect("~/Pages/Financeiros/ConFmaPagamento.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (ValidaCampos() == false)
                    return;


                FmaPagamentoDAL d = new FmaPagamentoDAL();
                FmaPagamento p = new FmaPagamento();

                p.DescricaoFmaPagamento = txtDescricao.Text.ToUpper();

                p.CodigoFmaPagamentoNFE = DdlFormaPagamento.SelectedValue;

                if (DdlBandeira.SelectedValue == "..... NÃO SELECIONAR .....")
                    p.CodigoBandeiraNFE = "00";
                else
                    p.CodigoBandeiraNFE = DdlBandeira.SelectedValue;

                p.CodigoSituacao = Convert.ToInt32(txtCodSituacao.SelectedValue);

                if (txtCodigo.Text == "Novo")
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Forma de Pagamento inclusa com Sucesso!!!";
                }
                else
                {
                    p.CodigoFmaPagamento = Convert.ToInt32(txtCodigo.Text);
                    d.Atualizar(p);
                    Session["MensagemTela"] = "Forma de Pagamento alterada com Sucesso!!!";
                }

                //if (Session["IncProdutoCategoria"] != null)
                //{
                //    List<Produto> listCadProduto = new List<Produto>();
                //    listCadProduto = (List<Produto>)Session["IncProdutoCategoria"];

                //    String strCodCateg = txtCodigo.Text.Replace(".", "");

                //    listCadProduto[0].CodigoCategoria = Convert.ToDouble(strCodCateg);
                //    Session["IncProdutoCategoria"] = listCadProduto;
                //    Session["ZoomCategoria2"] = null;

                //    Session["MensagemTela"] = null;
                //    Response.Redirect("~/Pages/Produto/CadProduto.aspx");
                //    return;
                //}
                //else
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
        protected void DdlBandeira_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void DdlFormaPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((DdlFormaPagamento.SelectedValue == "03") || (DdlFormaPagamento.SelectedValue == "04"))
            {
                DdlBandeira.Enabled = true;
                BandCartaoNFEDAL p = new BandCartaoNFEDAL();
                DdlBandeira.DataSource = p.ListaBandCartaoNFE();
                DdlBandeira.DataTextField = "DescricaoBandCartaoNFE";
                DdlBandeira.DataValueField = "CodigoBandCartaoNFE";
                DdlBandeira.DataBind();
                DdlBandeira.Items.Insert(0, "..... SELECIONE UMA BANDEIRA DE CARTÃO .....");

            }
            else
            {
                DdlBandeira.Enabled = false;
                DdlBandeira.ClearSelection();
                DdlBandeira.Items.Insert(0, "..... NÃO SELECIONAR .....");

            }
        }
    }
}