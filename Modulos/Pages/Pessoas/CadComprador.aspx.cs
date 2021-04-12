using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Pessoas
{
    public partial class CadComprador : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<Comprador> listCadComprador = new List<Comprador>();
        protected Boolean ValidaCampos()
        {

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }

            if (!ValidaPessoa())
                return false;


            if (!ValidaUsuario())
                return false;

            return true;
        }
        protected Boolean ValidaUsuario()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código do Usuário", txtCodUsuario.Text.Replace(".", ""), true, true, true, false, "NUMERIC", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodUsuario.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodUsuario.Focus();
                }

                return false;
            }
            return blnCampoValido;
        }
        protected Boolean ValidaPessoa()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código da Pessoa", txtCodPessoa.Text, true, true, true, false, "NUMERIC", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodPessoa.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodPessoa.Focus();
                }

                return false;
            }
            return blnCampoValido;
        }
        protected void LimpaCampos()
        {
            txtCodComprador.Text = "Novo";
            txtNomeComprador.Text = "";
            txtCodPessoa.Text = "";
            txtCodUsuario.Text = "";
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

            if (Session["ZoomComprador2"] != null)
            {
                if (Session["ZoomComprador2"].ToString() == "RELACIONAL")
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
                                               "ConComprador.aspx");

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

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    LimpaCampos();
                    if (Session["ZoomComprador2"] == null)
                        Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if ((Session["IncCompradorUsuario"] != null) || (Session["IncCompradorPessoa"] != null))
                    {

                        if (Session["IncCompradorPessoa"] != null)
                            listCadComprador = (List<Comprador>)Session["IncCompradorPessoa"];

                        if (Session["IncCompradorUsuario"] != null)
                            listCadComprador = (List<Comprador>)Session["IncCompradorUsuario"];

                        foreach (Comprador p in listCadComprador)
                        {

                            if (p.CodigoComprador == 0)
                                txtCodComprador.Text = "Novo";
                            else
                                txtCodComprador.Text = p.CodigoComprador.ToString();

                            if (p.CodigoPessoa == 0)
                                txtCodPessoa.Text = "";
                            else
                            {
                                txtCodPessoa.Text = p.CodigoPessoa.ToString();
                                txtCodPessoa_TextChanged(sender, e);
                            }
                            if (p.CodigoUsuario == 0)
                            {
                                if (txtCodComprador.Text != "Novo")
                                    txtCodUsuario.Text = "";

                            }
                        else
                            {
                                txtCodUsuario.Text = p.CodigoUsuario.ToString();
                                txtCodUsuario_TextChanged(sender, e);
                            }
                        }
                        listCadComprador = null;
                        Session["IncCompradorPessoa"] = null;
                        Session["IncCompradorUsuario"] = null;

                    }
                    if (Session["ZoomComprador"] != null)
                    {
                        string s = Session["ZoomComprador"].ToString();
                        Session["ZoomComprador"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            btnExcluir.Visible = true;
                            foreach (string word in words)
                                if (txtCodComprador.Text == "Novo")
                                {

                                    txtCodComprador.Text = word;
                                    txtCodComprador.Enabled = false;

                                    CompradorDAL r = new CompradorDAL();
                                    Comprador p = new Comprador();

                                    p = r.PesquisarComprador(Convert.ToInt64(txtCodComprador.Text));

                                    txtNomeComprador.Text = p.NomeComprador;
                                    txtCodPessoa.Text = p.CodigoPessoa.ToString();
                                    txtCodPessoa_TextChanged(sender, e);
                                    txtCodUsuario.Text = p.CodigoUsuario.ToString();
                                    txtCodUsuario_TextChanged(sender, e);

                                }
                        }
                    }
                    else
                    {
                        btnExcluir.Visible = false;
                    }
                }
                if(txtCodComprador.Text == "")
                    btnVoltar_Click(sender, e);
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodComprador.Text.Trim() != "")
                {
                    CompradorDAL d = new CompradorDAL();
                    d.Excluir(Convert.ToInt32(txtCodComprador.Text));
                    PessoaDAL x = new PessoaDAL();
                    x.AtualizarPessoaComprador(Convert.ToInt32(txtCodPessoa.Text), 0);

                    Session["MensagemTela"] = "Comprador Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Comprador não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomComprador"] = null;
            if (Session["ZoomComprador2"] != null)
            {
                Session["ZoomComprador2"] = null;
                Session["MensagemTela"] = null;
                return; 
            }

            Response.Redirect("~/Pages/Pessoas/ConComprador.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            CompradorDAL d = new CompradorDAL();
            Comprador p = new Comprador();

            p.CodigoPessoa = Convert.ToInt64(txtCodPessoa.Text);
            p.CodigoUsuario = Convert.ToInt64(txtCodUsuario.Text);

            if (txtCodComprador.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Comprador incluso com Sucesso!!!";
            }
            else
            {
                p.CodigoComprador = Convert.ToInt64(txtCodComprador.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Comprador alterado com Sucesso!!!";
            }
            btnVoltar_Click(sender, e);

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
 
        }
        protected void txtCodUsuario_TextChanged(object sender, EventArgs e)
        {
            UsuarioDAL r = new UsuarioDAL();
            Usuario p = new Usuario();
            if (ValidaUsuario())
            {
                p = r.PesquisarUsuario(Convert.ToInt32(txtCodUsuario.Text));
                if (p == null)
                    ShowMessage("Usuário não Cadastrado", MessageType.Info);
            }
        }
        protected void btnConUsuario_Click(object sender, EventArgs e)
        {
            long CodComprador = 0;

            if (txtCodComprador.Text != "Novo")
                CodComprador = Convert.ToInt64(txtCodComprador.Text);

            Comprador x1 = new Comprador(Convert.ToInt64(CodComprador),
                                         Convert.ToInt64("0" + txtCodPessoa.Text),
                                         Convert.ToInt64("0" + txtCodUsuario.Text));

            listCadComprador = new List<Comprador>();
            listCadComprador.Add(x1);
            Session["IncCompradorUsuario"] = listCadComprador;
            Session["ZoomUsuario2"] = "RELACIONAL";
            Response.Redirect("~/Pages/Usuarios/ConUsuario.aspx");
        }
        protected void txtCodPessoa_TextChanged(object sender, EventArgs e)
        {
            PessoaDAL r = new PessoaDAL();
            Pessoa p = new Pessoa();
            txtNomeComprador.Text = "";
            if (ValidaPessoa())
            {
                p = r.PesquisarPessoa(Convert.ToInt64(txtCodPessoa.Text));
                txtNomeComprador.Text = "";

                if (p != null)
                {
                    txtNomeComprador.Text = p.NomePessoa;
                    Usuario m = new UsuarioDAL().PesquisarUsuarioPorCodPessoa(Convert.ToInt64(txtCodPessoa.Text));
                    txtCodUsuario.Text = "";

                    if (m != null)
                        txtCodUsuario.Text = m.CodigoUsuario.ToString();
                }
                else
                    ShowMessage("Pessoa não Cadastrada", MessageType.Info);
            }
        }
        protected void BtnConPessoa_Click(object sender, EventArgs e)
        {
            long CodComprador = 0;

            if (txtCodComprador.Text != "Novo")
                CodComprador = Convert.ToInt64(txtCodComprador.Text);

            Comprador x1 = new Comprador(Convert.ToInt64(CodComprador),
                                         Convert.ToInt64("0" + txtCodPessoa.Text),
                                         Convert.ToInt64("0" + txtCodUsuario.Text));

            listCadComprador = new List<Comprador>();
            listCadComprador.Add(x1);
            Session["IncCompradorPessoa"] = listCadComprador;
            Session["ZoomPessoa2"] = "RELACIONAL";
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=4");
        }

    }
}