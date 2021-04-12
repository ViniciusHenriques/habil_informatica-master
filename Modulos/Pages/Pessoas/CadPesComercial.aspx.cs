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
    public partial class CadPesComercial : System.Web.UI.Page
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
            if(Convert.ToDecimal(txtProjecao.Text) <= 0)
            {
                ShowMessage("Insira uma projeção válida", MessageType.Info);
                return false;
            }

            return true;
        }
        protected void CarregaSituacoes()
        {
            PessoaDAL p = new PessoaDAL();
            ddlTransp.DataSource = p.ListarTransportadores("", "", "", "");
            ddlTransp.DataTextField = "NomePessoa";
            ddlTransp.DataValueField = "CodigoPessoa";
            ddlTransp.DataBind();
            ddlTransp.Items.Insert(0, "* Nenhum selecionado");
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
                                txtCodPessoa.Text = word;
                                txtCodPessoa.Enabled = false;
                                txtRazSocial.Enabled = false;
                                PessoaDAL r = new PessoaDAL();
                                Pessoa p = new Pessoa();

                                p = r.PesquisarPessoa(Convert.ToInt64(txtCodPessoa.Text));

                                txtRazSocial.Text = p.NomePessoa;
                                txtCodPessoa.Text = Convert.ToString(p.CodigoPessoa);
                                txtProjecao.Text = p.NumeroProjecao.ToString();

                                if (p.DataAtualizacao != null)
                                    txtDataAtualizacao.Text = p.DataAtualizacao.ToString().Substring(0, 10);

                                CarregaSituacoes();
                                ddlTransp.SelectedValue = p.CodigoTransportador.ToString();

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

            p.CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
            p.NumeroProjecao = Convert.ToDecimal(txtProjecao.Text);
            if(ddlTransp.SelectedValue != "* Nenhum selecionado")
                p.CodigoTransportador = Convert.ToInt32(ddlTransp.SelectedValue);
            d.AtualizarPessoaComercial(p);


            Session["MensagemTela"] = "Cadastro Comercial salvo com sucesso!";

            btnVoltar_Click(sender, e);

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }

        protected void txtProjecao_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProjecao.Text.Equals(""))
            {
                txtProjecao.Text = "";
            }
            else
            {
                v.CampoValido("Projeção", txtProjecao.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                    txtProjecao.Text = "";

            }
        }
    }
}