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
    public partial class CadPesFiscal : System.Web.UI.Page
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

            return true;
        }
        protected void CarregaSituacoes()
        {
            Habil_RegTributarioDAL rtd = new Habil_RegTributarioDAL();
            ddlRegTributario.DataSource = rtd.ListaHabil_RegTributario();
            ddlRegTributario.DataTextField = "DescricaoHabil_RegTributario";
            ddlRegTributario.DataValueField = "CodigoHabil_RegTributario";
            ddlRegTributario.DataBind();
            ddlRegTributario.Items.Insert(0, "* Nenhum selecionado");

            GpoTribPessoaDAL gtpd = new GpoTribPessoaDAL();
            ddlGpoTribPessoa.DataSource = gtpd.ObterGpoTribPessoas("", "", "", "");
            ddlGpoTribPessoa.DataTextField = "DescricaoGpoTribPessoa";
            ddlGpoTribPessoa.DataValueField = "CodigoGpoTribPessoa";
            ddlGpoTribPessoa.DataBind();
            ddlGpoTribPessoa.Items.Insert(0, "* Nenhum selecionado");

            TipoOperacaoDAL tipOp = new TipoOperacaoDAL();
            ddlTipoOperacao.DataSource = tipOp.ListarTipoOperacoes("CD_SITUACAO", "INT", "1", "");
            ddlTipoOperacao.DataTextField = "DescricaoTipoOperacao";
            ddlTipoOperacao.DataValueField = "CodigoTipoOperacao";
            ddlTipoOperacao.DataBind();
            ddlTipoOperacao.Items.Insert(0, "* Nenhum selecionado");

            PISDAL pis = new PISDAL();
            ddlPIS.DataSource = pis.ListarPIS("", "", "", "");
            ddlPIS.DataTextField = "DescricaoPIS";
            ddlPIS.DataValueField = "CodigoIndice";
            ddlPIS.DataBind();
            ddlPIS.Items.Insert(0, "* Nenhum selecionado");

            COFINSDAL cofins = new COFINSDAL();
            ddlCOFINS.DataSource = cofins.ListarCOFINS("", "", "", "");
            ddlCOFINS.DataTextField = "DescricaoCOFINS";
            ddlCOFINS.DataValueField = "CodigoIndice";
            ddlCOFINS.DataBind();
            ddlCOFINS.Items.Insert(0, "* Nenhum selecionado");
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
                    PanelSelect = "home";
                    Session["TabFocada"] = "home";

                    string s = Session["ZoomPessoa"].ToString();
                    Session["ZoomPessoa"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                            if (txtCodPessoa.Text == "")
                            {
                                CarregaSituacoes();
                                txtCodPessoa.Text = word;
                                txtCodPessoa.Enabled = false;
                                txtRazSocial.Enabled = false;
                                PessoaDAL r = new PessoaDAL();
                                Pessoa p = new Pessoa();

                                p = r.PesquisarPessoa(Convert.ToInt64(txtCodPessoa.Text));

                                txtRazSocial.Text = p.NomePessoa;
                                txtCodPessoa.Text = Convert.ToString(p.CodigoPessoa);

                                if (p.DataAtualizacao != null)
                                    txtDataAtualizacao.Text = p.DataAtualizacao.ToString().Substring(0, 10);
                                
                                ddlRegTributario.SelectedValue = p.CodHabil_RegTributario.ToString();
                                ddlGpoTribPessoa.SelectedValue = p.CodigoGpoTribPessoa.ToString();
                                ddlTipoOperacao.SelectedValue = p.CodigoTipoOperacao.ToString();
                                ddlCOFINS.SelectedValue = p.CodigoCOFINS.ToString();
                                ddlPIS.SelectedValue = p.CodigoPIS.ToString();
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

            if(ddlRegTributario.SelectedValue != "* Nenhum selecionado")
                p.CodHabil_RegTributario= Convert.ToInt32(ddlRegTributario.SelectedValue);
            if (ddlGpoTribPessoa.SelectedValue != "* Nenhum selecionado")
                p.CodigoGpoTribPessoa = Convert.ToInt32(ddlGpoTribPessoa.SelectedValue);
            if (ddlTipoOperacao.SelectedValue != "* Nenhum selecionado")
                p.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
            if (ddlPIS.SelectedValue != "* Nenhum selecionado")
                p.CodigoPIS = Convert.ToInt32(ddlPIS.SelectedValue);
            if (ddlCOFINS.SelectedValue != "* Nenhum selecionado")
                p.CodigoCOFINS = Convert.ToInt32(ddlCOFINS.SelectedValue);

            p.CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
            d.AtualizarPessoaFiscal(p);

            Session["MensagemTela"] = "Cadastro Fiscal salvo com sucesso!";

            btnVoltar_Click(sender, e);

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }
    }
}