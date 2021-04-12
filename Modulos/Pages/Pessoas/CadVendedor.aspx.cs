using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftHabilInformatica.Pages.Pessoas
{
    public partial class CadVendedor : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<Vendedor> listCadVendedor = new List<Vendedor>();
        protected Boolean ValidaCampos()
        {
            bool blnCampoValido = false;


            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }

            if (!ValidaPessoa())
                return false;


            if (ddlEmpresa.SelectedValue == "*Nenhum Selecionado")
            {
                ShowMessage("Selecione uma Empresa", MessageType.Info);
                return false;
            }

            if (ddlTipoVendedor.SelectedValue == "*Nenhum Selecionado")
            {
                ShowMessage("Selecione um Tipo de Vendedor", MessageType.Info);
                return false;
            }

            v.CampoValido("% Comissão", txtPercentualComissao.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtPercentualComissao.Focus();
                    return false;
                }
                return false;
            }

            if (txtCodUsuario.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "MostrarModalConfirmSemUsuario();", true);
                return false;
            }

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

            v.CampoValido("Código da Pessoa", txtCodPessoa.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

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
            else
            {
                Vendedor p = new Vendedor();
                VendedorDAL p_DAL = new VendedorDAL();
                p = p_DAL.PesquisarVendedorPessoa(Convert.ToInt64(txtCodPessoa.Text));

                if (p != null)
                {
                    if (txtCodVendedor.Text == "Novo" && p.CodigoVendedor != 0 && p.CodigoPessoa == Convert.ToInt64(txtCodPessoa.Text))
                    {
                        ShowMessage("Pessoa já vinculada a outro Vendedor", MessageType.Info);
                        txtCodPessoa.Focus();
                        return false;
                    }
                    if ((p.CodigoVendedor != Convert.ToInt64(txtCodVendedor.Text)) && txtCodVendedor.Text != "Novo")
                    {
                        ShowMessage("Pessoa já vinculada a outro Vendedor", MessageType.Info);
                        txtCodPessoa.Focus();
                        return false;
                    }
                }
            }
            return blnCampoValido;
        }
        protected void LimpaCampos()
        {
            CarregaSituacoes();
            ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
            txtCodVendedor.Text = "Novo";
            txtPercentualComissao.Text = "0,00";
            txtCodPessoa.Text = "";
            txtNomePessoa.Text = "";
            txtCodUsuario.Text = "";
        }
        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            EmpresaDAL RnEmpresa = new EmpresaDAL();

            ddlSituacao.DataSource = sd.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("", "", "", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();
            ddlEmpresa.Items.Insert(0, "*Nenhum Selecionado");
            
            ddlTipoVendedor.DataSource = sd.TipoDeVendedor();
            ddlTipoVendedor.DataTextField = "DescricaoTipo";
            ddlTipoVendedor.DataValueField = "CodigoTipo";
            ddlTipoVendedor.DataBind();
            ddlTipoVendedor.Items.Insert(0, "*Nenhum Selecionado");

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

            if (Session["ZoomVendedor2"] != null)
            {
                if (Session["ZoomVendedor2"].ToString() == "RELACIONAL")
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
                                           "ConVendedor.aspx");

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
                //Entra quando for novo
                LimpaCampos();
                if (Session["ZoomVendedor2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if ((Session["IncVendedorUsuario"] != null) || (Session["IncVendedorPessoa"] != null))
                {

                    if (Session["IncVendedorPessoa"] != null)
                        listCadVendedor = (List<Vendedor>)Session["IncVendedorPessoa"];

                    if (Session["IncVendedorUsuario"] != null)
                        listCadVendedor = (List<Vendedor>)Session["IncVendedorUsuario"];

                    foreach (Vendedor p in listCadVendedor)
                    {

                        if (p.CodigoVendedor == 0)
                            txtCodVendedor.Text = "Novo";
                        else
                            txtCodVendedor.Text = p.CodigoVendedor.ToString();

                        if (p.CodigoPessoa == 0)
                            txtCodPessoa.Text = "";
                        else
                        {
                            txtCodPessoa.Text = p.CodigoPessoa.ToString();
                            txtCodPessoa_TextChanged(sender, e);
                        }
                        if (p.CodigoUsuario == 0)
                            txtCodUsuario.Text = "";
                        else
                        {
                            txtCodUsuario.Text = p.CodigoUsuario.ToString();
                            txtCodUsuario_TextChanged(sender, e);
                        }
                    }
                    listCadVendedor = null;
                    Session["IncVendedorPessoa"] = null;
                    Session["IncVendedorUsuario"] = null;

                }

                if (Session["Usuario"] != null)
                {
                    Usuario u = (Usuario)Session["Usuario"];

                    if (u.CodigoUsuario.ToString() != "0")
                    {
                        txtCodUsuario.Text = u.CodigoUsuario.ToString();
                        //txtNomeUsuario.Text = u.NomeUsuario.ToString();
                    }

                    Session["Usuario"] = null;

                }

                if (Session["ZoomVendedor"] != null)
                {
                    string s = Session["ZoomVendedor"].ToString();
                    Session["ZoomVendedor"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)

                            if (word != "")
                            {
                                txtCodVendedor.Text = word;
                                txtCodVendedor.Enabled = false;

                                VendedorDAL r = new VendedorDAL();
                                Vendedor p = new Vendedor();

                                p = r.PesquisarVendedor(Convert.ToInt32(txtCodVendedor.Text));

                                //  txtNomeVendedor.Text = p.Pessoa.NomePessoa.ToString();
                                txtPercentualComissao.Text = p.PercentualComissao.ToString();

                                txtCodPessoa.Text = p.CodigoPessoa.ToString();
                                txtCodPessoa_TextChanged(sender, e);

                                ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
                                ddlTipoVendedor.SelectedValue = p.CodigoTipoVendedor.ToString();

                                //txtCodUsuario.Text = p.CodigoUsuario.ToString();
                                //txtCodUsuario_TextChanged(sender, e);

                            }

                    }
                }
                else
                {
                    btnExcluir.Visible = false;
                }

                if (Session["CadVendedorPessoa"] != null)
                {
                    Vendedor x1 = new Vendedor();

                    x1 = (Vendedor)Session["CadVendedorPessoa"];

                    if (x1.CodigoVendedor == 0)
                        txtCodVendedor.Text = "Novo";
                    else
                        txtCodVendedor.Text = x1.CodigoVendedor.ToString();

                    if (x1.PercentualComissao != 0)
                        txtPercentualComissao.Text = x1.PercentualComissao.ToString();


                    if (x1.CodigoPessoa == 0)
                        txtCodPessoa.Text = "";
                    else
                    {
                        txtCodPessoa.Text = x1.CodigoPessoa.ToString();
                    }

                    if (Session["ZoomPessoa"] != null)
                    {
                        txtCodPessoa.Text = Session["ZoomPessoa"].ToString();
                    }

                    txtCodPessoa_TextChanged(sender, e);

                    ddlSituacao.SelectedValue = x1.CodigoSituacao.ToString();
                    ddlTipoVendedor.SelectedValue = x1.CodigoTipoVendedor.ToString();

                    if (x1.CodigoEmpresa != 0)
                        ddlEmpresa.SelectedValue = x1.CodigoEmpresa.ToString();

                    if (x1.CodigoUsuario != 0)
                        txtCodUsuario.Text = x1.CodigoUsuario.ToString();


                    Session["CadVendedorPessoa"] = null;
                    Session["ZoomPessoa"] = null;
                }
                else
                {
                    btnExcluir.Visible = false;
                }

            }
            if (txtCodVendedor.Text == "")
                btnVoltar_Click(sender, e);
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodVendedor.Text.Trim() != "")
                {
                    VendedorDAL d = new VendedorDAL();
                    d.Excluir(Convert.ToInt64(txtCodVendedor.Text));
                    Session["MensagemTela"] = "Vendedor Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Vendedor não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomVendedor"] = null;
            if (Session["ZoomVendedor2"] != null)
            {
                Session["ZoomVendedor2"] = null;
                Session["MensagemTela"] = null;
                return;
            }

            Response.Redirect("~/Pages/Pessoas/ConVendedor.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            VendedorDAL d = new VendedorDAL();
            Vendedor p = new Vendedor();

            p.CodigoPessoa = Convert.ToInt64(txtCodPessoa.Text);
            p.CodigoSituacao = Convert.ToInt64(ddlSituacao.SelectedValue);
            p.CodigoEmpresa = Convert.ToInt64(ddlEmpresa.SelectedValue);
            p.CodigoTipoVendedor = Convert.ToInt64(ddlTipoVendedor.SelectedValue);
            p.PercentualComissao = Convert.ToDecimal(txtPercentualComissao.Text);
            //p.CodigoUsuario = Convert.ToInt64(txtCodUsuario.Text);

            if (txtCodVendedor.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Vendedor incluso com Sucesso!!!";
            }
            else
            {
                p.CodigoVendedor = Convert.ToInt32(txtCodVendedor.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Vendedor alterado com Sucesso!!!";
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
        }
        protected void BtnConUsuario_Click(object sender, EventArgs e)
        {
            long CodVendedor = 0;

            if (txtCodVendedor.Text != "Novo")
                CodVendedor = Convert.ToInt64(txtCodVendedor.Text);

            Vendedor x1 = new Vendedor(Convert.ToInt64(CodVendedor),
                                         Convert.ToInt64("0" + txtCodPessoa.Text));

            listCadVendedor = new List<Vendedor>();
            listCadVendedor.Add(x1);
            Session["IncVendedorUsuario"] = listCadVendedor;
            Session["ZoomUsuario2"] = "RELACIONAL";
            Response.Redirect("~/Pages/Usuarios/ConUsuario.aspx");
        }
        protected void txtCodPessoa_TextChanged(object sender, EventArgs e)
        {
            PessoaDAL r = new PessoaDAL();
            Pessoa p = new Pessoa();
            txtNomePessoa.Text = "";
            if (ValidaPessoa())
            {
                p = r.PesquisarPessoa(Convert.ToInt64(txtCodPessoa.Text));
                txtNomePessoa.Text = "";

                if (p != null)
                {
                    txtNomePessoa.Text = p.NomePessoa;
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
            CompactaDocumento();
            Session["ZoomPessoa2"] = "RELACIONAL";

            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=11");
        }

        protected void PreencheDados(object sender, EventArgs e, int CodPessoa, int CodUsuario)
        {
            Vendedor ven = (Vendedor)Session[""];

            if (ven.CodigoVendedor == 0)
            {
                txtCodVendedor.Text = "Novo";
                btnExcluir.Visible = false;

            }
            else
            {
                txtCodVendedor.Text = Convert.ToString(ven.CodigoVendedor);
                btnExcluir.Visible = true;

            }

        }

        protected void CompactaDocumento()
        {
            long CodVendedor = 0;
            long CodPessoa = 0;
            long CodEmpresa = 0;


            Vendedor x1 = new Vendedor();

            if (txtCodVendedor.Text != "Novo")
                CodVendedor = Convert.ToInt64(txtCodVendedor.Text);

            if (Convert.ToInt64("0" + txtCodPessoa.Text) != 0)
                CodPessoa = Convert.ToInt64(txtCodPessoa.Text);

            if (ddlEmpresa.Text != "*Nenhum Selecionado")
                CodEmpresa = Convert.ToInt64(ddlEmpresa.SelectedValue);

            if (ddlTipoVendedor.Text != "*Nenhum Selecionado")
                x1.CodigoTipoVendedor = Convert.ToInt64(ddlTipoVendedor.SelectedValue);

            if (Convert.ToDecimal("0" + txtPercentualComissao.Text) != 0)
                x1.PercentualComissao = Convert.ToDecimal(txtPercentualComissao.Text);

            //if (Convert.ToInt64("0" + txtCodUsuario.Text) != 0)
            //{
            //    var x2 = new UsuarioDAL().PesquisarUsuarioPorCodPessoa(Convert.ToInt64(txtCodUsuario.Text));
            //    if (x2 != null)
            //        x1.CodigoUsuario = Convert.ToInt64(x2.CodigoUsuario);
            //    else
            //        x1.CodigoUsuario = 0;


            //}



            x1.CodigoVendedor = CodVendedor;
            x1.CodigoPessoa = CodPessoa;
            x1.CodigoEmpresa = CodEmpresa;
            x1.CodigoSituacao = Convert.ToInt64(ddlSituacao.SelectedValue);

            Session["CadVendedorPessoa"] = x1;
            Session["IncMovAcessoContato"] = null;
            Session["IncMovAcessoPessoa"] = null;
        }

        protected void ddlEmpresa_TextChanged(object sender, EventArgs e)
        {
            //EmpresaDAL p = new EmpresaDAL();
            //Empresa x = new Empresa();
            //x = p.PesquisarEmpresa(Convert.ToInt64(ddlEmpresa.SelectedValue));


        }

        protected void ddlTipoVendedor_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnSimSemUsuario_Click(object sender, EventArgs e)
        {
            txtCodUsuario.Text = "0";
            btnSalvar_Click(sender, e);
        }
    }
}