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
    public partial class CadPesFinanceiro : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
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

            v.CampoValido("Limite de crédito", txtLimiteCredito.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtLimiteCredito.Focus();
                }
                return false;
            }
            if (Convert.ToDecimal(txtLimiteCredito.Text) <= 0)
            {
                ShowMessage("Insira um limite de crédito válido", MessageType.Info);
                return false;
            }
            return true;
        }
        protected void CarregaSituacoes()
        {
            CondPagamentoDAL sd = new CondPagamentoDAL();
            ddlCondPagamento.DataSource = sd.ListarCondPagamento("CD_SITUACAO", "INT", "1", "");
            ddlCondPagamento.DataTextField = "DescricaoCondPagamento";
            ddlCondPagamento.DataValueField = "CodigoCondPagamento";
            ddlCondPagamento.DataBind();
            ddlCondPagamento.Items.Insert(0, "* Nenhum selecionado");

            PlanoContasDAL RnPlanoConta = new PlanoContasDAL();
            ddlPlanoConta.DataSource = RnPlanoConta.ListarPlanoContas("CD_SITUACAO", "INT", "1", "");
            ddlPlanoConta.DataTextField = "DescricaoPlanoConta";
            ddlPlanoConta.DataValueField = "CodigoPlanoConta";
            ddlPlanoConta.DataBind();
            ddlPlanoConta.Items.Insert(0, "* Nenhum selecionado");

            TipoServicoDAL RnTipoServico = new TipoServicoDAL();
            ddlTipoServico.DataSource = RnTipoServico.ListarTipoServico("CD_SITUACAO", "INT", "1", "");
            ddlTipoServico.DataTextField = "DescricaoTipoServico";
            ddlTipoServico.DataValueField = "CodigoTipoServico";
            ddlTipoServico.DataBind();
            ddlTipoServico.Items.Insert(0, "* Nenhum selecionado");

            TipoCobrancaDAL RnTipoCobranca = new TipoCobrancaDAL();
            ddlTipoCobranca.DataSource = RnTipoCobranca.ListarTipoCobrancas("CD_SITUACAO", "INT", "1", "");
            ddlTipoCobranca.DataTextField = "DescricaoTipoCobranca";
            ddlTipoCobranca.DataValueField = "CodigoTipoCobranca";
            ddlTipoCobranca.DataBind();
            ddlTipoCobranca.Items.Insert(0, "* Nenhum selecionado");
        }
        protected void LimpaCampos()
        {
            txtCodPessoa.Text = "Novo";
            txtCodPessoa.Enabled = false;
            txtRazSocial.Text = "";

            CarregaSituacoes();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");


            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Error);
                Session["MensagemTela"] = null;
                return;
            }

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;



            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();

            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConPessoa.aspx");

            lista.ForEach(delegate (Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoAlterar)
                        btnSalvar.Visible = false;

                }
            });

            if (Session["CodUsuario"].ToString() == "-150380")
            {
                btnSalvar.Visible = true;
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {

                if (Session["ZoomPessoa2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomPessoa"] != null)
                {

                    string s = Session["ZoomPessoa"].ToString();
                    Session["ZoomPessoa"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                        {
                            if (txtCodPessoa.Text == "")
                            {
                                txtCodPessoa.Text = word;
                                txtCodPessoa.Enabled = false;
                                txtRazSocial.Enabled = false;
                                PessoaDAL r = new PessoaDAL();
                                Pessoa p = new Pessoa();

                                p = r.PesquisarPessoa(Convert.ToInt64(txtCodPessoa.Text));

                                txtRazSocial.Text = p.NomePessoa;
                                txtCodPessoa.Text = Convert.ToString(p.CodigoPessoa);

                                txtLimiteCredito.Text = p.ValorLimiteCredito.ToString();

                                CarregaSituacoes();

                                if (p.DataAtualizacao != null)
                                    txtDataAtualizacao.Text = p.DataAtualizacao.ToString().Substring(0, 10);

                                if (p.CodigoCondPagamento != 0)
                                    ddlCondPagamento.SelectedValue = p.CodigoCondPagamento.ToString();

                                if (p.CodigoPlanoContas != 0)
                                    ddlPlanoConta.SelectedValue = p.CodigoPlanoContas.ToString();

                                if (p.CodigoTipoServico != 0)
                                    ddlTipoServico.SelectedValue = p.CodigoTipoServico.ToString();

                                if (p.CodigoTipoCobranca != 0)
                                    ddlTipoCobranca.SelectedValue = p.CodigoTipoCobranca.ToString();

                            }
                        }
                    }
                }
                else
                {
                    LimpaCampos();
                }
            }


            if (txtCodPessoa.Text == "" || txtCodPessoa.Text == "Novo")
                btnVoltar_Click(sender, e);

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["InscricaoPessoa"] = null;
            Session["EnderecoPessoa"] = null;
            Session["ContatoPessoa"] = null;
            Session["cadPessoa"] = null;
            Session["RetornoCadPessoa"] = null;

            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            PessoaDAL d = new PessoaDAL();
            Pessoa p = new Pessoa();

            if (ddlCondPagamento.SelectedValue != "* Nenhum selecionado")
                p.CodigoCondPagamento = Convert.ToInt32(ddlCondPagamento.SelectedValue);
            if (ddlPlanoConta.SelectedValue != "* Nenhum selecionado")
                p.CodigoPlanoContas = Convert.ToInt32(ddlPlanoConta.SelectedValue);
            if (ddlTipoServico.SelectedValue != "* Nenhum selecionado")
                p.CodigoTipoServico = Convert.ToInt32(ddlTipoServico.SelectedValue);
            if (ddlTipoCobranca.SelectedValue != "* Nenhum selecionado")
                p.CodigoTipoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);

            p.CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
            p.ValorLimiteCredito = Convert.ToDecimal(txtLimiteCredito.Text);
            d.AtualizarPessoaFinanceiro(p);


            Session["MensagemTela"] = "Cadastro financeiro salvo com sucesso!";

            btnVoltar_Click(sender, e);

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }

        protected void txtCreditoCompra_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtLimiteCredito.Text.Equals(""))
            {
                txtLimiteCredito.Text = "0,00";
            }
            else
            {
                v.CampoValido("Limite de crédito", txtLimiteCredito.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtLimiteCredito.Text = Convert.ToDecimal(txtLimiteCredito.Text).ToString("###,##0.00");
                }
                else
                    txtLimiteCredito.Text = "0,00";

            }
        }
    }
}