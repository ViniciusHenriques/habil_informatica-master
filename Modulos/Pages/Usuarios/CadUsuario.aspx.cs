using System;
using System.Collections.Generic;
using System.Web.UI;
using DAL.Model;
using DAL.Persistence;
using System.Security.Cryptography;

namespace SoftHabilInformatica.Pages.Usuarios
{
    public partial class CadUsuario : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        public enum MessageType { Success, Error, Info, Warning };
        String strMensagemR = "";

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            CargoDAL cargo = new CargoDAL();
            ddlCargo.DataSource = cargo.ListarCargos("","","","");
            ddlCargo.DataTextField = "DescricaoCargo";
            ddlCargo.DataValueField = "CodigoCargo";
            ddlCargo.DataBind();
            ddlCargo.Items.Insert(0, "..... SELECIONE UM CARGO .....");

        }
        protected void IniciaPagina(object sender, EventArgs e)
        {
            Session["ZoomPflUsuario"] = null;
            Session["ZoomSituacao"] = null;
            Session["Pagina"] = Request.CurrentExecutionFilePath;
            LimparTela();
            CarregaSituacoes();
            
            if (Session["ZoomUsuario"] != null)
            {

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConUsuario.aspx");


                string s = Session["ZoomUsuario"].ToString();
                Session["ZoomUsuario"] = null;

                string[] words = s.Split('³');
                if (s != "³")
                {
                    foreach (string word in words)
                        if (txtCodUsuario.Text == "Novo")
                        {
                            txtCodUsuario.Text = word;
                            txtCodUsuario.Enabled = false;
                            CarregaSituacoes();
                            UsuarioDAL m = new UsuarioDAL();
                            Usuario p = new Usuario();

                            p = m.PesquisarUsuario(Convert.ToInt32(txtCodUsuario.Text));
                            txtCodPerfil.Text = p.CodigoPerfil.ToString();
                            txtCodPerfil_TextChanged(sender, e);
                            ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                            txtSenha.Text = p.Senha;
                            txtLogin.Text = p.Login;
                            //txtNomeUsuario.Text = p.NomeUsuario;
                            ddlCargo.SelectedValue = p.CodigoCargo.ToString();
                            txtCodPessoa.Text = p.CodigoPessoa.ToString();
                            txtCodPessoa_TextChanged(sender, e);
                            btnExcluir.Visible = true;
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
                            if (Session["CodUsuario"].ToString() == "-150380")
                            {
                                btnSalvar.Visible = true;
                                if (txtCodPerfil.Text != "Novo")
                                    btnExcluir.Visible = true;

                            }
                        }
                }

            }

            if (Session["CadUsuario"] != null)
            {
                if (Session["CadUsuario2"] != null)
                {
                    string s = Session["CadUsuario2"].ToString();
                    Session["CadUsuario2"] = null;

                    string[] words = s.Split('³');

                    if (s != "³")
                    {
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                txtCodPessoa.Text = word;

                                PreencheDados(sender, e);
                                txtCodPessoa_TextChanged(sender, e);
                            }
                        }
                    }
                }
                PreencheDados(sender, e);
            }


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

                
                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    IniciaPagina(sender, e);
                    return;
                }

                if ((Session["ZoomPflUsuario"] != null) && (Session["Operacao"].ToString() == "EDICAO"))
                {
                    if (txtCodUsuario.Text.Trim() == "")
                    {
                        IniciaPagina(sender, e);
                        return;
                    }

                    string s = Session["ZoomPflUsuario"].ToString();
                    string[] words = s.Split('²');

                    if (s != "³")
                    {
                         txtCodPerfil.Text = "";
                        txtDcrPerfil.Text = "";

                        foreach (string word in words)
                        {
                            if (txtCodPerfil.Text == "")
                                txtCodPerfil.Text = word;
                            else
                                if (txtDcrPerfil.Text == "")
                                    txtDcrPerfil.Text = word;
                        }
                        Session["ZoomPflUsuario"] = null;
                        //txtNomeUsuario.Focus();
                        if (Session["CodUsuario"].ToString() == "-150380")
                        {
                            btnSalvar.Visible = true;
                            if (txtCodPerfil.Text != "Novo")
                                btnExcluir.Visible = true;

                        }

                    }
                }
          
        }
        protected void btnCfmSim2_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                UsuarioDAL d = new UsuarioDAL();

                if ((txtCodUsuario.Text != "Novo") && (txtCodUsuario.Text != ""))
                {
                    d.Excluir(Convert.ToInt32(txtCodUsuario.Text));
                    PessoaDAL pe = new PessoaDAL();
                    pe.PessoaEmpresaUsuario(Convert.ToInt64(txtCodPessoa.Text), 2, false, false);
                    Session["MensagemTela"] = "Usuário Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Usuário não identificado.&emsp;&emsp;&emsp;";

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
            Response.Redirect("~/Pages/Usuarios/ConUsuario.aspx");
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void LimparTela()
        {
            pnlCadastrarSenha.Visible = false;
 
            txtCodUsuario.Text = "Novo";
            txtCodUsuario.Enabled = false;
            txtLogin.Text = "";
            txtSenha.Text = "";
            txtCodPerfil.Text = "";
            txtDcrPerfil.Text = "";

            btnExcluir.Visible = false;

            Session["Operacao"] = "EDICAO";
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos() == false)
                    return;

                UsuarioDAL d = new UsuarioDAL();
                Usuario p = new Usuario();

                p.NomeUsuario = txtPessoa.Text;
                p.Login = txtLogin.Text;
                p.Senha = txtSenha.Text;
                p.CodigoPerfil = Convert.ToInt32(txtCodPerfil.Text);
                p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                p.CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
                p.CodigoCargo = Convert.ToInt32(ddlCargo.SelectedValue);
                p.GUID = "1";

                PessoaDAL pe = new PessoaDAL();
                pe.PessoaEmpresaUsuario(Convert.ToInt64(txtCodPessoa.Text), 2, false, true);

                if (txtCodUsuario.Text == "Novo")
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Registro Incluído com Sucesso";
                }
                else
                {
                    p.CodigoUsuario = Convert.ToInt32(txtCodUsuario.Text);
                    d.Atualizar(p);
                    Session["MensagemTela"] = "Registro Alterado com Sucesso";

                }
                

                btnVoltar_Click(sender, e);

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message.ToString(),MessageType.Error);
            }

        }
        protected void txtCodPerfil_TextChanged(object sender, EventArgs e)
        {
            if (txtCodPerfil.Text != "") 
            {
                PerfilUsuarioDAL d = new PerfilUsuarioDAL();
                PerfilUsuario p = new PerfilUsuario();
                p = d.PesquisarPerfilUsuario(Convert.ToInt32(txtCodPerfil.Text));

                if (p == null)
                {
                    txtDcrPerfil.Text = "";
                    ShowMessage("Perfil não cadastrado.",MessageType.Info);
                    txtCodPerfil.Text = "";
                }
                else
                    txtDcrPerfil.Text = p.DescricaoPflUsuario;

            }
            else
            {
                txtDcrPerfil.Text = "";
            }

            ddlSituacao.Focus();

        }
        protected Boolean ValidaCampos()
        {

            Boolean retorno = false;
            if (txtCodUsuario.Text == "")
            {
                txtCodUsuario.Focus();
                ShowMessage("Código de Usuário deve ser Informado.",MessageType.Info);
                return retorno;
            }

            if (txtLogin.Text == "")
            {
                txtLogin.Focus();
                ShowMessage("Login deve ser Informado.", MessageType.Info);
                return retorno;

            }
            else
            {
                Boolean blnExiste = false;
                UsuarioDAL d = new UsuarioDAL();
                blnExiste = d.VerificaDisponibilidadeLogin(txtLogin.Text, txtCodUsuario.Text);
                if (blnExiste)
                {
                    ShowMessage("Login: " + txtLogin.Text + " Já em uso por outro Usuário.", MessageType.Info);
                    txtLogin.Focus();
                    return retorno;
                }
            }


            if (txtCodPerfil.Text == "")
            {
                txtCodPerfil.Focus();
                ShowMessage("Perfil Usuário deve ser Informado.", MessageType.Info);
                return retorno;
            }


            v.CampoValido("Código Pessoa", txtCodPessoa.Text, true, true, true, false, "", ref retorno, ref strMensagemR);


            if (!retorno)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodPessoa.Focus();

                }

                return retorno;
            }

            Int64 numero;
            if (!Int64.TryParse(txtCodPessoa.Text, out numero))
            {
                ShowMessage("Codigo do Pessoa incorreto", MessageType.Info);
                return false;
            }
            if(ddlCargo.SelectedValue == "..... SELECIONE UM CARGO .....")
            {
                ShowMessage("Selecione um cargo", MessageType.Info);
                return retorno;
            }
           

            return true;
        }
        
        protected void btnCfmSenhaMesmo_Click(object sender, EventArgs e)
        {

            if (txtCodUsuario.Text != "Novo")
            {
                if (txtSenhaAtual.Visible == true)
                {
                    if (txtSenhaAtual.Text == "")
                    {
                        ShowMessage("Senhas Atual deve ser informada para validação.",MessageType.Info);
                        txtSenhaAtual.Focus();
                    }
                    else
                    {
                        clsHash clsh = new clsHash(SHA512.Create());
                        string teste = clsh.CriptografarSenha(txtSenhaAtual.Text);

                        if (teste != txtSenha.Text)
                        {
                            ShowMessage("Senhas Atual invalida.",MessageType.Info);
                            txtSenhaAtual.Focus();
                            return;
                        }
                    }
                }
            }

            if ((txtNovaSenha.Text != "") && (txtConfirmaSenha.Text !="")) {
                if (txtNovaSenha.Text != txtConfirmaSenha.Text)
                {
                    ShowMessage("Senhas informadas são diferentes.",MessageType.Info);
                    txtNovaSenha.Focus();
                }
                else
                {

                    clsHash clsh = new clsHash(SHA512.Create());
                    string teste = clsh.CriptografarSenha(txtConfirmaSenha.Text);
                    txtSenha.Text = teste;

                    pnlCadastrarSenha.Visible = false;
                    txtCodPerfil.Focus();

                }

            }
            if ((txtNovaSenha.Text == "") && (txtConfirmaSenha.Text ==""))
                pnlCadastrarSenha.Visible = false;
 
            
        }
        protected void btnCfmPerfil_Click(object sender, EventArgs e)
        {
            if (Session["ZoomPflUsuario"] != null)
                if ((Session["ZoomPflUsuario"].ToString() == "³") && (txtCodPerfil.Text == ""))
                    ShowMessage("Perfil do Usuário deve ser Informado.",MessageType.Info);

        }
        protected void Button4_Click(object sender, EventArgs e)
        {

            if ((txtCodUsuario.Text == "Novo") || (txtSenha.Text == ""))
                {
                txtSenhaAtual.Visible = false;
                Label1.Visible = false;
                txtNovaSenha.Focus(); 
            }
            else
            {
                txtSenhaAtual.Visible = true;
                Label1.Visible = true;
                txtSenhaAtual.Focus();
            }

            pnlCadastrarSenha.Visible = true; 
        }
       
        protected void btnDisponivel_Click(object sender, EventArgs e)
        {
            Boolean blnExiste = false;
            UsuarioDAL d = new UsuarioDAL();
            blnExiste = d.VerificaDisponibilidadeLogin(txtLogin.Text, txtCodUsuario.Text);

            if (!blnExiste)
                ShowMessage("Login: " + txtLogin.Text + "  Disponível.",MessageType.Info);
            else
                ShowMessage("Login: " + txtLogin.Text +" Já em uso por outro Usuário.",MessageType.Error);

        }

        protected void txtCodPessoa_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodPessoa.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Cliente", txtCodPessoa.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodPessoa.Text = "";
                    return;
                }
            }
            List<Usuario> ListUsu = new List<Usuario>();
            UsuarioDAL usuDAL = new UsuarioDAL();
            ListUsu = usuDAL.ListarUsuarios("CD_PESSOA", "INT", txtCodPessoa.Text, "");
            if (ListUsu.Count != 0)
            {
                foreach(Usuario Li in ListUsu)
                {
                    if (txtCodUsuario.Text != "Novo")
                    {
                        if (Li.CodigoUsuario != Convert.ToInt64(txtCodUsuario.Text))
                            ShowMessage("Já existe Usuário vinculado a essa pessoa", MessageType.Info);
                    }
                    else
                    {
                        if (Li.CodigoPessoa == Convert.ToInt64(txtCodPessoa.Text))
                            ShowMessage("Já existe Usuário vinculado a essa pessoa", MessageType.Info);

                    }
                }
                
            }
            Int64 numero;
            if (!Int64.TryParse(txtCodPessoa.Text, out numero))
            {
                return;
            }
            Int64 codigoPessoa = Convert.ToInt64(txtCodPessoa.Text);
            PessoaDAL pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();
            p2 = pessoa.PesquisarPessoaUsuario(codigoPessoa);

            if (p2 == null)
            {
                ShowMessage("Pessoa não existente!", MessageType.Info);
                txtCodPessoa.Text = "";
                txtPessoa.Text = "";
                txtCodPessoa.Focus();

                return;
            }
            txtPessoa.Text = p2.NomePessoa;

            Session["TabFocada"] = null;
        }

        protected void btnPessoa_Click(object sender, EventArgs e)
        {
            Documento();
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=8");
        }
        protected void PreencheDados(object sender, EventArgs e)
        {
            Usuario usu = (Usuario)Session["CadUsuario"];
            if (usu == null)
                return;
            if (usu.CodigoUsuario == 0)
            {
                txtCodUsuario.Text = "Novo";
                btnExcluir.Visible = false;
            }
            else
            {
                txtCodUsuario.Text = Convert.ToString(usu.CodigoUsuario);
                btnExcluir.Visible = true;
            }
            if(usu.CodigoPerfil == 0)
            {
                txtCodPerfil.Text = "";
            }
            else
            {
                txtCodPerfil.Text = usu.CodigoPerfil.ToString();
                txtCodPerfil_TextChanged(sender, e);
            }
            //txtNomeUsuario.Text = usu.NomeUsuario;
            ddlSituacao.SelectedValue = usu.CodigoSituacao.ToString();
            txtLogin.Text = usu.Login;
            ddlCargo.SelectedValue = usu.CodigoCargo.ToString();
            txtSenha.Text = usu.Senha;

            Session["CadUsuario"] = null;
        }
        protected void Documento()
        {
            Usuario usu = new Usuario();
            if (txtCodUsuario.Text != "Novo")
                usu.CodigoUsuario = Convert.ToInt32(txtCodUsuario.Text);
            else
                usu.CodigoUsuario = 0;

            if (txtCodPerfil.Text != "")
                usu.CodigoPerfil = Convert.ToInt32(txtCodPerfil.Text);
            else
                usu.CodigoPerfil = 0;

            //usu.NomeUsuario = txtNomeUsuario.Text;
            usu.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            usu.Login = txtLogin.Text;

            if(ddlCargo.SelectedValue != "..... SELECIONE UM CARGO .....")
                usu.CodigoCargo = Convert.ToInt32(ddlCargo.SelectedValue);

            usu.Senha = txtSenha.Text;
            Session["CadUsuario"] = usu;

        }

        protected void btnResetarSenha_Click(object sender, EventArgs e)
        {
            UsuarioDAL clsUsuarioDAL = new UsuarioDAL();
            clsUsuarioDAL.ReiniciarSenha(Convert.ToInt64(txtCodUsuario.Text));
            ShowMessage("o Usuário receberá em instantes um E-mail para atualizar a senha!", MessageType.Info);

        }

        protected void txtCodPessoa_TextChanged1(object sender, EventArgs e)
        {

        }
    }
}