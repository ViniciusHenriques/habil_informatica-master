using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Pessoas
{
    public partial class CadPessoa : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        List<Pessoa> listCadPessoa = new List<Pessoa>();
        List<Pessoa_Inscricao> listCadPessoaInscricao = new List<Pessoa_Inscricao>();
        List<Pessoa_Endereco> listCadPessoaEndereco = new List<Pessoa_Endereco>();
        List<Pessoa_Contato> listCadPessoaContato = new List<Pessoa_Contato>();
        List<Vendedor> listRepresentantes = new List<Vendedor>();
        List<GrupoPessoa> listCadGpoPessoa = new List<GrupoPessoa>();
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
            v.CampoValido("Código Sistema Anterior", txtCodSisAnterior.Text, false, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodSisAnterior.Focus();
                }
                return false;
            }
            v.CampoValido("Razão Social", txtRazSocial.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtRazSocial.Focus();

                }

                return false;
            }
            v.CampoValido("Nome Fantasia", txtNomeFantasia.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtNomeFantasia.Focus();

                }

                return false;
            }

            if (grdInscricao.Rows.Count == 0)
            {
                ShowMessage("Pessoa deve constar pelo menos uma inscrição cadastrado .", MessageType.Info);
                PanelSelect = "home";
                Session["TabFocada"] = "home";
                return false;
            }

            if (grdEndereco.Rows.Count == 0)
            {
                ShowMessage("Pessoa deve constar pelo menos um endereço cadastrado .", MessageType.Info);
                PanelSelect = "profile";
                Session["TabFocada"] = "profile";
                return false;
            }
            if (grdContato.Rows.Count == 0)
            {
                ShowMessage("Pessoa deve constar pelo menos um contato cadastrado .", MessageType.Info);
                PanelSelect = "contact";
                Session["TabFocada"] = "contact";
                return false;
            }

            /*if (Session["CodUsuario"].ToString() != "-150380")
            {*/


            if (ddlGpoPessoa.Text == "..... SELECIONE UM GRUPO DE PESSOA.....")
            {
                ShowMessage("Selecione um Grupo de Pessoas.", MessageType.Info);
                PanelSelect = "parameter";
                Session["TabFocada"] = "parameter";
                return false;
            }

            if((txtCodPessoa.Text != "Novo") && (chkUsuario.Checked == false) ) { 
                List<Usuario> ListUsu = new List<Usuario>();
                UsuarioDAL usuDAL = new UsuarioDAL();
                ListUsu = usuDAL.ListarUsuarios("CD_PESSOA", "INT", txtCodPessoa.Text, "");
                if (ListUsu.Count != 0 && chkUsuario.Checked == false)
                {
                    ShowMessage("Pessoa vinculada a um usuário", MessageType.Info);
                    return false;
                }
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

            txtCodFase.DataSource = sd.EtapasCadPessoa();
            txtCodFase.DataTextField = "DescricaoTipo";
            txtCodFase.DataValueField = "CodigoTipo";
            txtCodFase.DataBind();  

            GrupoPessoaDAL g = new GrupoPessoaDAL();
            ddlGpoPessoa.DataSource = g.ObterGrupoPessoa();
            ddlGpoPessoa.DataTextField = "DescricaoGrupoPessoa";
            ddlGpoPessoa.DataValueField = "CodigoGrupoPessoa";
            ddlGpoPessoa.DataBind();
            ddlGpoPessoa.Items.Insert(0, "..... SELECIONE UM GRUPO DE PESSOA.....");

            VendedorDAL vend = new VendedorDAL();
            ddlRepresentantes.DataSource = vend.ListarRepresentantes();
            ddlRepresentantes.DataTextField = "NomePessoa";
            ddlRepresentantes.DataValueField = "CodigoVendedor";
            ddlRepresentantes.DataBind();
            ddlRepresentantes.Items.Insert(0, "*Nenhum Selecionado");

        }
        protected void LimpaCampos()
        {
            txtCodPessoa.Text = "Novo";
            txtCodPessoa.Enabled = false;
            txtRazSocial.Text = "";
            txtNomeFantasia.Text = "";

            CarregaSituacoes();

            txtCodSituacao.SelectedValue = "1";
            txtCodFase.SelectedValue = "15";

            listCadPessoaInscricao.RemoveAll(x => x._CodigoItem >= 0);
            grdInscricao.DataSource = listCadPessoaInscricao;
            grdInscricao.DataBind();

            listCadPessoaEndereco.RemoveAll(x => x._CodigoItem >= 0);
            grdEndereco.DataSource = listCadPessoaEndereco;
            grdEndereco.DataBind();

            listCadPessoaContato.RemoveAll(x => x._CodigoItem >= 0);
            grdContato.DataSource = listCadPessoaContato;
            grdContato.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            chkEmpresa.Enabled = false;
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["TabFocada"] != null)
                PanelSelect = Session["TabFocada"].ToString();
            else
                if (!IsPostBack)
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Error);
                Session["MensagemTela"] = null;
                return;
            }

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomPessoa2"] != null)
            {
                if (Session["ZoomPessoa2"].ToString() == "RELACIONAL")
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
                                            "ConPessoa.aspx");

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

            if (Session["CodUsuario"].ToString() =="-150380")
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

                                PessoaDAL r = new PessoaDAL();
                                Pessoa p = new Pessoa();

                                p = r.PesquisarPessoa(Convert.ToInt64(txtCodPessoa.Text));

                                txtRazSocial.Text = p.NomePessoa;
                                txtNomeFantasia.Text = p.NomeFantasia;
                                txtCodPessoa.Text = Convert.ToString(p.CodigoPessoa);
                                if (p.DataCadastro != null)
                                    txtDataCadastro.Text = p.DataCadastro.ToString().Substring(0, 10);
                                if (p.DataAtualizacao != null)
                                    txtDataAtualizacao.Text = p.DataAtualizacao.ToString().Substring(0, 10);

                                CarregaSituacoes();
                                if (p.CodigoSisAnterior != null)
                                    txtCodSisAnterior.Text = p.CodigoSisAnterior.ToString();
                                txtCodSituacao.SelectedValue = p.CodigoSituacaoPessoa.ToString();
                                txtCodFase.SelectedValue = p.CodigoSituacaoFase.ToString();
                                ddlGpoPessoa.SelectedValue = p.CodigoGrupoPessoa.ToString();
                                ddlGpoPessoa_SelectedIndexChanged(sender, e);
                                if (p.PessoaEmpresa == 1)
                                {
                                    chkEmpresa.Checked = true;
                                    btnExcluir.Visible = false;
                                }
                                if (p.PessoaCliente == 1)
                                    chkCliente.Checked = true;
                                if (p.PessoaFornecedor == 1)
                                    chkFornecedor.Checked = true;
                                if (p.PessoaTransportador == 1)
                                    chkTransportador.Checked = true;
                                if (p.PessoaVendedor == 1)
                                    chkVendedor.Checked = true;
                                if (p.PessoaUsuario == 1)
                                    chkUsuario.Checked = true;
                                if (p.PessoaComprador == 1)
                                    chkComprador.Checked = true;

                                PessoaInscricaoDAL pe = new PessoaInscricaoDAL();
                                listCadPessoaInscricao = pe.ObterPessoaInscricoes(Convert.ToInt64(txtCodPessoa.Text));
                                Session["InscricaoPessoa"] = listCadPessoaInscricao;

                                PessoaEnderecoDAL pe2 = new PessoaEnderecoDAL();
                                listCadPessoaEndereco = pe2.ObterPessoaEnderecos(Convert.ToInt64(txtCodPessoa.Text));
                                Session["EnderecoPessoa"] = listCadPessoaEndereco;

                                PessoaContatoDAL pe3 = new PessoaContatoDAL();
                                listCadPessoaContato = pe3.ObterPessoaContatos(Convert.ToInt64(txtCodPessoa.Text));
                                Session["ContatoPessoa"] = listCadPessoaContato;

                                listRepresentantes = r.ObterRepresentantes(p.CodigoPessoa);
                                Session["ListaRepresentantes"] = listRepresentantes;

                            }
                    }
                }
                else
                {
                    LimpaCampos();
                    btnExcluir.Visible = false;
                    if (Request.QueryString["Cad"] == "12")
                        chkTransportador.Checked = true;
                    else if (Request.QueryString["Cad"] == "3")
                        chkFornecedor.Checked = true;
                    else if (Request.QueryString["Cad"] != null)
                        chkCliente.Checked = true;
                    

                }
            }
            if (Session["RetornoCadPessoa"] != null)
            {
                //btnExcluir.Visible = false;
                PanelSelect = Convert.ToString(Session["TabFocada"]);
                
                ddlGpoPessoa.Focus();
                listCadPessoa = (List<Pessoa>)Session["RetornoCadPessoa"];
                foreach (Pessoa p in listCadPessoa)
                {
                    txtCodPessoa.Text = Convert.ToString(p.CodigoPessoa);
                    txtNomeFantasia.Text = Convert.ToString(p.NomeFantasia);
                    if (txtCodPessoa.Text != "Novo")
                    {
                        txtDataAtualizacao.Text = p.DataAtualizacao.ToString().Substring(0, 10);
                        txtDataCadastro.Text = p.DataCadastro.ToString().Substring(0, 10);

                    }
                    if (txtCodPessoa.Text == "0")
                        btnExcluir.Visible = false;

                    txtCodSituacao.SelectedValue = p.CodigoSituacaoPessoa.ToString();
                    txtCodFase.SelectedValue = p.CodigoSituacaoFase.ToString();
                    txtRazSocial.Text = p.NomePessoa.ToString();

                    txtCodSisAnterior.Text = p.CodigoSisAnterior.ToString();

                    if (p.PessoaEmpresa == 1)
                        chkEmpresa.Checked = true;
                    if (p.PessoaCliente == 1)
                        chkCliente.Checked = true;
                    if (p.PessoaFornecedor == 1)
                        chkFornecedor.Checked = true;
                    if (p.PessoaTransportador == 1)
                        chkTransportador.Checked = true;
                    if (p.PessoaVendedor == 1)
                        chkVendedor.Checked = true;
                    if (p.PessoaUsuario == 1)
                        chkUsuario.Checked = true;
                    if (p.PessoaComprador == 1)
                        chkComprador.Checked = true;

                    if (Session["InscricaoPessoa"] != null)
                    {
                        listCadPessoaInscricao = (List<Pessoa_Inscricao>)Session["InscricaoPessoa"];
                        grdInscricao.DataSource = listCadPessoaInscricao;
                        grdInscricao.DataBind();
                    }

                    if (Session["EnderecoPessoa"] != null)
                    {
                        listCadPessoaEndereco = (List<Pessoa_Endereco>)Session["EnderecoPessoa"];
                        grdEndereco.DataSource = listCadPessoaEndereco;
                        grdEndereco.DataBind();
                    }

                    if (Session["ContatoPessoa"] != null)
                    {
                        listCadPessoaContato = (List<Pessoa_Contato>)Session["ContatoPessoa"];
                        grdContato.DataSource = listCadPessoaContato;
                        grdContato.DataBind();
                    }

                    Session["RetornoCadPessoa"] = null;
                }
            }
            if (Session["InscricaoPessoa"] != null)
            {
                listCadPessoaInscricao = (List<Pessoa_Inscricao>)Session["InscricaoPessoa"];
                grdInscricao.DataSource = listCadPessoaInscricao;
                grdInscricao.DataBind();
            }

            if (Session["EnderecoPessoa"] != null)
            {
                listCadPessoaEndereco = (List<Pessoa_Endereco>)Session["EnderecoPessoa"];
                grdEndereco.DataSource = listCadPessoaEndereco;
                grdEndereco.DataBind();
            }

            if (Session["ContatoPessoa"] != null)
            {
                listCadPessoaContato = (List<Pessoa_Contato>)Session["ContatoPessoa"];
                grdContato.DataSource = listCadPessoaContato;
                grdContato.DataBind();
            }

            if (Session["ListaRepresentantes"] != null)
            {
                listRepresentantes= (List<Vendedor>)Session["ListaRepresentantes"];
                grdRepresentantes.DataSource = listRepresentantes;
                grdRepresentantes.DataBind();
            }

            if (Session["CadPessoa"] != null)
            {
                listCadPessoa = (List<Pessoa>)Session["CadPessoa"];
                Session["CadPessoa"] = null;
            }

            foreach (Pessoa p in listCadPessoa)
            {
                txtCodPessoa.Text = p.CodigoPessoa.ToString();
                txtCodPessoa.Enabled = false;

                if (txtCodPessoa.Text == "0")
                {
                    txtCodPessoa.Text = "Novo";
                    txtDataCadastro.Text = "";
                    txtDataCadastro.Text = "";
                }
                else
                {
                    txtRazSocial.Text = p.NomePessoa;
                    txtNomeFantasia.Text = p.NomeFantasia;
                    if (p.DataCadastro != null)
                        txtDataCadastro.Text = p.DataCadastro.ToString().Substring(0, 10);
                    if (p.DataAtualizacao != null)
                        p.DataAtualizacao.ToString().Substring(0, 10);
                }

                CarregaSituacoes();
                txtCodSituacao.SelectedValue = p.CodigoSituacaoPessoa.ToString();
                txtCodFase.SelectedValue = p.CodigoSituacaoFase.ToString();
                if (p.CodigoGrupoPessoa != 0)
                {
                    ddlGpoPessoa.SelectedValue = p.CodigoGrupoPessoa.ToString();
                    ddlGpoPessoa_SelectedIndexChanged(sender, e);
                }


            }
            

            listCadPessoa = null;
            if(txtCodPessoa.Text == "")
                btnVoltar_Click(sender, e);

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodPessoa.Text.Trim() != "")
                {
                    PessoaDAL d = new PessoaDAL();
                    d.Excluir(Convert.ToInt32(txtCodPessoa.Text));
                    Session["MensagemTela"] = "Pessoa Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Pessoa não identificado.&emsp;&emsp;&emsp;";

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
            if (Session["IncMovAcessoPessoa"] != null)
            {
                Session["MensagemTela"] = null;
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                Response.Redirect("~/Pages/Acesso/ManMovAcesso.aspx");
                return;
            }

            if (Session["IncMovAcessoContato"] != null)
            {
                Session["MensagemTela"] = null;
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                Response.Redirect("~/Pages/Acesso/ManMovAcesso.aspx");
                return;
            }

            if (Session["Doc_SolicitacaoAtendimento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 1)
            {
                Session["MensagemTela"] = null;
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                Session["TabFocada"] = "home";
                Response.Redirect("~/Pages/Servicos/ManSolAtendimento.aspx");
                return;
            }
            if (Session["Doc_OrdemServico"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 2)
            {
                Session["MensagemTela"] = null;
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                Session["TabFocada"] = "home";
                Response.Redirect("~/Pages/Servicos/ManOrdServico.aspx");
                return;
            }

            Session["ZoomPessoa"] = null;
            if (Session["ZoomPessoa2"] != null)
            {
                Session["ZoomPessoa2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadPessoa.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }
            Session["InscricaoPessoa"] = null;
            Session["EnderecoPessoa"] = null;
            Session["ContatoPessoa"] = null;
            Session["cadPessoa"] = null;
            Session["RetornoCadPessoa"] = null;
            Session["ListaRepresentantes"] = null;

            listCadPessoaInscricao = null;
            listCadPessoaContato = null;
            listCadPessoaEndereco = null;

            if (Request.QueryString["Cad"] != null)
            {
                Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?cad=" + Request.QueryString["Cad"]);
            }
            else
                Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            PessoaDAL d = new PessoaDAL();
            Pessoa p = new Pessoa();
            
            p.NomePessoa = txtRazSocial.Text.ToUpper();
            p.NomeFantasia = txtNomeFantasia.Text.ToUpper();
            p.DataCadastro = DateTime.Today;
            p.DataAtualizacao = DateTime.Today;
            p.CodigoSituacaoFase = Convert.ToInt32(txtCodFase.SelectedValue);
            p.CodigoSituacaoPessoa = Convert.ToInt32(txtCodSituacao.SelectedValue);
            p.CodigoGrupoPessoa = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            p.CodigoSisAnterior = txtCodSisAnterior.Text;
            p.ListaRepresentantes = listRepresentantes;

            ddlGpoPessoa_SelectedIndexChanged(sender, e);

            if (chkEmpresa.Checked == true)
                p.PessoaEmpresa = 1;
            if (chkCliente.Checked == true)
                p.PessoaCliente= 1;
            if (chkFornecedor.Checked == true)
                p.PessoaFornecedor = 1;
            if (chkTransportador.Checked == true)
                p.PessoaTransportador= 1;
            if (chkVendedor.Checked == true)
                p.PessoaVendedor= 1;
            if (chkUsuario.Checked == true)
                p.PessoaUsuario= 1;
            if (chkComprador.Checked == true)
                p.PessoaComprador= 1;

            /*
            if (Session["CodUsuario"].ToString() != "-150380")
            {
                p.CodigoGpoTribPessoa = Convert.ToInt32(ddlGpoTribPessoa.SelectedValue);
                p.CodHabil_RegTributario = Convert.ToInt32(ddlRegTributario.SelectedValue);
            }
            */

            if (txtCodPessoa.Text == "Novo")
            {
                Int64 CodigoPessoa = 0;
                d.Inserir(p, listCadPessoaInscricao, listCadPessoaEndereco, listCadPessoaContato, ref CodigoPessoa);
                Session["MensagemTela"] = "Pessoa Incluído com Sucesso!!!";

                List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
                DBTabelaCampos rowp = new DBTabelaCampos();
                rowp.Filtro = "CD_PESSOA";
                rowp.Inicio = CodigoPessoa.ToString();
                rowp.Fim = "";
                rowp.Tipo = "INT";
                listaT.Add(rowp);
                if(Request.QueryString["Cad"] == "12"){
                    DBTabelaCampos rowp2 = new DBTabelaCampos();
                    rowp2.Filtro = "IN_TRANSPORTADOR";
                    rowp2.Inicio = "1";
                    rowp2.Fim = "";
                    rowp2.Tipo = "SMALLINT";
                    listaT.Add(rowp2);
                }
                Session["LST_CADPESSOA"] = listaT;

                foreach (var item in listCadPessoaContato)
                {
                    SalvarFotoContato(CodigoPessoa, item._CodigoItem, item._Foto);
                }

            }
            else
            {
                p.CodigoPessoa = Convert.ToInt64(txtCodPessoa.Text);
                d.Atualizar(p, listCadPessoaInscricao, listCadPessoaEndereco, listCadPessoaContato);

                Session["MensagemTela"] = "Pessoa Alterado com Sucesso!!!";
            }

            btnVoltar_Click(sender, e);

        }
        protected void btnNovInscricao_Click(object sender, EventArgs e)
        {
            long CodPess = 0;
            DateTime DteData1;
            DateTime DteData2;

           
            if (txtCodPessoa.Text != "Novo")
                CodPess = Convert.ToInt64(txtCodPessoa.Text);

            if (txtDataCadastro.Text != "")
            {
                DteData1 = Convert.ToDateTime(txtDataCadastro.Text);
            }
            else
                DteData1 = DateTime.UtcNow;

            if (txtDataAtualizacao.Text != "")
                DteData2 = Convert.ToDateTime(txtDataAtualizacao.Text);
            else
                DteData2 = DateTime.UtcNow;



            int CodGpoPess = 0;
            int pessoaEmpresa = 0;
            int pessoaCliente = 0;
            int pessoaFornecedor = 0;
            int pessoaTransportador = 0;
            int pessoaVendedor = 0;
            int pessoaUsuario = 0;
            int pessoaComprador = 0;

            if (chkEmpresa.Checked == true)
                pessoaEmpresa = 1;
            if (chkCliente.Checked == true)
                pessoaCliente = 1;
            if (chkFornecedor.Checked == true)
                pessoaFornecedor = 1;
            if (chkTransportador.Checked == true)
                pessoaTransportador = 1;
            if (chkVendedor.Checked == true)
                pessoaVendedor = 1;
            if (chkUsuario.Checked == true)
                pessoaUsuario = 1;
            if (chkComprador.Checked == true)
                pessoaComprador = 1;

            if (Session["CodUsuario"].ToString() != "-150380")
            {
                /*
                if (ddlRegTributario.Text != "..... SELECIONE UM REGIME DE TRIBUTAÇÃO .....")
                    CodTrib = Convert.ToInt32(ddlRegTributario.SelectedValue);

                if (ddlGpoTribPessoa.Text != "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PESSOAS .....")
                    CodGpo = Convert.ToInt32(ddlGpoTribPessoa.SelectedValue);
                    */
                if (ddlGpoPessoa.Text != "..... SELECIONE UM GRUPO DE PESSOA.....")
                    CodGpoPess = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            }

            Pessoa x1 = new Pessoa(CodPess,txtCodSisAnterior.Text, txtRazSocial.Text, txtNomeFantasia.Text,
                                DteData1, DteData2,
                                Convert.ToInt32(txtCodSituacao.Text), Convert.ToInt32(txtCodFase.Text), CodGpoPess,
                                pessoaEmpresa,pessoaCliente,pessoaFornecedor,pessoaTransportador,pessoaVendedor,pessoaUsuario,pessoaComprador);

            PanelSelect = "home";
            Session["TabFocada"] = "home";

            listCadPessoa = new List<Pessoa>();
            listCadPessoa.Add(x1);
            Session["CadPessoa"] = listCadPessoa;
            Session["RetornoCadPessoa"] = listCadPessoa;
            Session["InscricaoPessoa"] = listCadPessoaInscricao;
            Session["CadPessoa_Inscricao"] = "Novo";
            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Inscricao.aspx?cad=" + Convert.ToInt32(Request.QueryString["Cad"]));
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Inscricao.aspx");
        }
        protected void BtnIncEndereco_Click(object sender, EventArgs e)
        {
            long CodPess = 0;
            DateTime DteData1;
            DateTime DteData2;

            if (grdInscricao.Rows.Count == 0)
            {
                ShowMessage("Permitido Inclusão/Alteração/Exclusão de Endereço a partir de uma Inscrição de CNPJ/CPF Existente", MessageType.Info);
                return;
            }
            

            if (txtCodPessoa.Text != "Novo")
                CodPess = Convert.ToInt64(txtCodPessoa.Text);

            if (txtDataCadastro.Text != "")
            {
                DteData1 = Convert.ToDateTime(txtDataCadastro.Text);
            }
            else
                DteData1 = DateTime.UtcNow;

            if (txtDataAtualizacao.Text != "")
                DteData2 = Convert.ToDateTime(txtDataAtualizacao.Text);
            else
                DteData2 = DateTime.UtcNow;


            int CodGpoPess = 0;
            int pessoaEmpresa = 0;
            int pessoaCliente = 0;
            int pessoaFornecedor = 0;
            int pessoaTransportador = 0;
            int pessoaVendedor = 0;
            int pessoaUsuario = 0;
            int pessoaComprador = 0;

            if (chkEmpresa.Checked == true)
                pessoaEmpresa = 1;
            if (chkCliente.Checked == true)
                pessoaCliente = 1;
            if (chkFornecedor.Checked == true)
                pessoaFornecedor = 1;
            if (chkTransportador.Checked == true)
                pessoaTransportador = 1;
            if (chkVendedor.Checked == true)
                pessoaVendedor = 1;
            if (chkUsuario.Checked == true)
                pessoaUsuario = 1;
            if (chkComprador.Checked == true)
                pessoaComprador = 1;

            if (Session["CodUsuario"].ToString() != "-150380")
            {
                /*
                if (ddlRegTributario.Text != "..... SELECIONE UM REGIME DE TRIBUTAÇÃO .....")
                    CodTrib = Convert.ToInt32(ddlRegTributario.SelectedValue);

                if (ddlGpoTribPessoa.Text != "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PESSOAS .....")
                    CodGpo = Convert.ToInt32(ddlGpoTribPessoa.SelectedValue);
                    */
                if (ddlGpoPessoa.Text != "..... SELECIONE UM GRUPO DE PESSOA.....")
                    CodGpoPess = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            }
           
            Pessoa x1 = new Pessoa(CodPess, txtCodSisAnterior.Text, txtRazSocial.Text, txtNomeFantasia.Text,
                                DteData1, DteData2,
                                Convert.ToInt32(txtCodSituacao.Text), Convert.ToInt32(txtCodFase.Text), CodGpoPess, pessoaEmpresa, pessoaCliente, pessoaFornecedor,
                                pessoaTransportador, pessoaVendedor, pessoaUsuario, pessoaComprador );

            PanelSelect = "profile";
            Session["TabFocada"] = "profile";


            listCadPessoa = new List<Pessoa>();
            listCadPessoa.Add(x1);

            Session["RetornoCadPessoa"] = listCadPessoa;
            Session["CadPessoa"] = listCadPessoa;
            Session["EnderecoPessoa"] = listCadPessoaEndereco;
            Session["CadPessoa_Endereco"] = "Novo";
            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Endereco.aspx?cad=" + Convert.ToInt32(Request.QueryString["Cad"]));
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Endereco.aspx");

        }
        protected void grdInscricao_SelectedIndexChanged(object sender, EventArgs e)
        {

            long CodPess = 0;
            DateTime DteData1;
            DateTime DteData2;

            if (txtCodPessoa.Text != "Novo")
                CodPess = Convert.ToInt64(txtCodPessoa.Text);
           

            if (txtDataCadastro.Text != "")
            {
                DteData1 = Convert.ToDateTime(txtDataCadastro.Text);
            }
            else
                DteData1 = DateTime.UtcNow;

            if (txtDataAtualizacao.Text != "")
                DteData2 = Convert.ToDateTime(txtDataAtualizacao.Text);
            else
                DteData2 = DateTime.UtcNow;


            int CodGpoPess = 0;
            int pessoaEmpresa = 0;
            int pessoaCliente = 0;
            int pessoaFornecedor = 0;
            int pessoaTransportador = 0;
            int pessoaVendedor = 0;
            int pessoaUsuario = 0;
            int pessoaComprador = 0;

            if (chkEmpresa.Checked == true)
                pessoaEmpresa = 1;
            if (chkCliente.Checked == true)
                pessoaCliente = 1;
            if (chkFornecedor.Checked == true)
                pessoaFornecedor = 1;
            if (chkTransportador.Checked == true)
                pessoaTransportador = 1;
            if (chkVendedor.Checked == true)
                pessoaVendedor = 1;
            if (chkUsuario.Checked == true)
                pessoaUsuario = 1;
            if (chkComprador.Checked == true)
                pessoaComprador= 1;

            if (Session["CodUsuario"].ToString() != "-150380")
            {
                /*
                if (ddlRegTributario.Text != "..... SELECIONE UM REGIME DE TRIBUTAÇÃO .....")
                    CodTrib = Convert.ToInt32(ddlRegTributario.SelectedValue);

                if (ddlGpoTribPessoa.Text != "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PESSOAS .....")
                    CodGpo = Convert.ToInt32(ddlGpoTribPessoa.SelectedValue);
                    */
                if (ddlGpoPessoa.Text != "..... SELECIONE UM GRUPO DE PESSOA.....")
                    CodGpoPess = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            }
            
            Pessoa x1 = new Pessoa(CodPess, txtCodSisAnterior.Text, txtRazSocial.Text, txtNomeFantasia.Text,
                                DteData1, DteData2,
                                Convert.ToInt32(txtCodSituacao.Text), Convert.ToInt32(txtCodFase.Text), CodGpoPess, pessoaEmpresa, pessoaCliente, pessoaFornecedor, 
                                pessoaTransportador, pessoaVendedor, pessoaUsuario, pessoaComprador );

            PanelSelect = "home";
            Session["TabFocada"] = "home";

            listCadPessoa = new List<Pessoa>();
            listCadPessoa.Add(x1);
            Session["CadPessoa"] = listCadPessoa;
            Session["RetornoCadPessoa"] = listCadPessoa;
            Session["InscricaoPessoa"] = listCadPessoaInscricao;
            Session["CadPessoa_Inscricao"] = HttpUtility.HtmlDecode(grdInscricao.SelectedRow.Cells[0].Text);

            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Inscricao.aspx?cad=" + Convert.ToInt32(Request.QueryString["Cad"]));
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Inscricao.aspx");
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void grdEndereco_SelectedIndexChanged(object sender, EventArgs e)
        {
            long CodPess = 0;
            DateTime DteData1;
            DateTime DteData2;

            if (grdInscricao.Rows.Count == 0)
            {
                ShowMessage("Permitido Inclusão/Alteração/Exclusão de Endereço a partir de uma Inscrição de CNPJ/CPF Existente", MessageType.Info);
                return;
            }
           

            if (txtCodPessoa.Text != "Novo")
                CodPess = Convert.ToInt64(txtCodPessoa.Text);

            if (txtDataCadastro.Text != "")
            {
                DteData1 = Convert.ToDateTime(txtDataCadastro.Text);
            }
            else
                DteData1 = DateTime.UtcNow;

            if (txtDataAtualizacao.Text != "")
                DteData2 = Convert.ToDateTime(txtDataAtualizacao.Text);
            else
                DteData2 = DateTime.UtcNow;

            int CodGpoPess = 0;

            int pessoaEmpresa = 0;
            int pessoaCliente = 0;
            int pessoaFornecedor = 0;
            int pessoaTransportador = 0;
            int pessoaVendedor = 0;
            int pessoaUsuario = 0;
            int pessoaComprador = 0;

            if (chkEmpresa.Checked == true)
                pessoaEmpresa = 1;
            if (chkCliente.Checked == true)
                pessoaCliente = 1;
            if (chkFornecedor.Checked == true)
                pessoaFornecedor = 1;
            if (chkTransportador.Checked == true)
                pessoaTransportador = 1;
            if (chkVendedor.Checked == true)
                pessoaVendedor = 1;
            if (chkUsuario.Checked == true)
                pessoaUsuario = 1;
            if (chkComprador.Checked == true)
                pessoaComprador= 1;

            if (Session["CodUsuario"].ToString() != "-150380")
            {
                /*
                if (ddlRegTributario.Text != "..... SELECIONE UM REGIME DE TRIBUTAÇÃO .....")
                    CodTrib = Convert.ToInt32(ddlRegTributario.SelectedValue);

                if (ddlGpoTribPessoa.Text != "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PESSOAS .....")
                    CodGpo = Convert.ToInt32(ddlGpoTribPessoa.SelectedValue);

    */
                if (ddlGpoPessoa.Text != "..... SELECIONE UM GRUPO DE PESSOA.....")
                    CodGpoPess = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            }
            
            Pessoa x1 = new Pessoa(CodPess, txtCodSisAnterior.Text, txtRazSocial.Text, txtNomeFantasia.Text,
                                DteData1, DteData2,
                                Convert.ToInt32(txtCodSituacao.Text), Convert.ToInt32(txtCodFase.Text), CodGpoPess, pessoaEmpresa, pessoaCliente, pessoaFornecedor,
                                pessoaTransportador, pessoaVendedor, pessoaUsuario, pessoaComprador);

            PanelSelect = "profile";
            Session["TabFocada"] = "profile";

            listCadPessoa = new List<Pessoa>();
            listCadPessoa.Add(x1);
            Session["CadPessoa"] = listCadPessoa;
            Session["RetornoCadPessoa"] = listCadPessoa;
            Session["EnderecoPessoa"] = listCadPessoaEndereco;
            Session["CadPessoa_Endereco"] = HttpUtility.HtmlDecode(grdEndereco.SelectedRow.Cells[0].Text);

            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Endereco.aspx?cad=" + Convert.ToInt32(Request.QueryString["Cad"]));
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Endereco.aspx");
        }
        protected void BtnIncContato_Click(object sender, EventArgs e)
        {
            Session["CaminhoFotoContato"] = null;
            Session["CamLigada"] = null;
            long CodPess = 0;
            DateTime DteData1;
            DateTime DteData2;

            if (grdInscricao.Rows.Count == 0)
            {
                ShowMessage("Permitido Inclusão/Alteração/Exclusão de Endereço a partir de uma Inscrição de CNPJ/CPF Existente", MessageType.Info);
                return;
            }
            
            if (txtCodPessoa.Text != "Novo")
                CodPess = Convert.ToInt64(txtCodPessoa.Text);

            if (txtDataCadastro.Text != "")
            {
                DteData1 = Convert.ToDateTime(txtDataCadastro.Text);
            }
            else
                DteData1 = DateTime.UtcNow;

            if (txtDataAtualizacao.Text != "")
                DteData2 = Convert.ToDateTime(txtDataAtualizacao.Text);
            else
                DteData2 = DateTime.UtcNow;

            int CodGpoPess = 0;
            int pessoaEmpresa = 0;
            int pessoaCliente = 0;
            int pessoaFornecedor = 0;
            int pessoaTransportador = 0;
            int pessoaVendedor = 0;
            int pessoaUsuario = 0;
            int pessoaComprador = 0;

            if (chkEmpresa.Checked == true)
                pessoaEmpresa = 1;
            if (chkCliente.Checked == true)
                pessoaCliente = 1;
            if (chkFornecedor.Checked == true)
                pessoaFornecedor = 1;
            if (chkTransportador.Checked == true)
                pessoaTransportador = 1;
            if (chkVendedor.Checked == true)
                pessoaVendedor = 1;
            if (chkUsuario.Checked == true)
                pessoaUsuario = 1;
            if (chkComprador.Checked == true)
                pessoaComprador = 1;

            if (Session["CodUsuario"].ToString() != "-150380")
            {
                /*
                if (ddlRegTributario.Text != "..... SELECIONE UM REGIME DE TRIBUTAÇÃO .....")
                    CodTrib = Convert.ToInt32(ddlRegTributario.SelectedValue);

                if (ddlGpoTribPessoa.Text != "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PESSOAS .....")
                    CodGpo = Convert.ToInt32(ddlGpoTribPessoa.SelectedValue);
                    */
                if (ddlGpoPessoa.Text != "..... SELECIONE UM GRUPO DE PESSOA.....")
                    CodGpoPess = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            }
            

            Pessoa x1 = new Pessoa(CodPess, txtCodSisAnterior.Text, txtRazSocial.Text, txtNomeFantasia.Text,
                                DteData1, DteData2,
                                Convert.ToInt32(txtCodSituacao.Text), Convert.ToInt32(txtCodFase.Text),  CodGpoPess, pessoaEmpresa, pessoaCliente, 
                                pessoaFornecedor, pessoaTransportador, pessoaVendedor, pessoaUsuario, pessoaComprador);

            PanelSelect = "contact";
            Session["TabFocada"] = "contact";


            listCadPessoa = new List<Pessoa>();
            listCadPessoa.Add(x1);
            Session["CadPessoa"] = listCadPessoa;
            Session["RetornoCadPessoa"] = listCadPessoa;
            Session["ContatoPessoa"] = listCadPessoaContato;
            Session["CadPessoa_Contato"] = "Novo";

            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Contato.aspx?cad=" + Convert.ToInt32(Request.QueryString["Cad"]));
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Contato.aspx");
        }
        protected void grdContato_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CamLigada"] = null;
            Session["CaminhoFotoContato"] = null;
            long CodPess = 0;
            DateTime DteData1;
            DateTime DteData2;

            if (grdInscricao.Rows.Count == 0)
            {
                ShowMessage("Permitido Inclusão/Alteração/Exclusão de Endereço a partir de uma Inscrição de CNPJ/CPF Existente", MessageType.Info);
                return;
            }

           
            if (txtCodPessoa.Text != "Novo")
                CodPess = Convert.ToInt64(txtCodPessoa.Text);

            if (txtDataCadastro.Text != "")
            {
                DteData1 = Convert.ToDateTime(txtDataCadastro.Text);
            }
            else
                DteData1 = DateTime.UtcNow;

            if (txtDataAtualizacao.Text != "")
                DteData2 = Convert.ToDateTime(txtDataAtualizacao.Text);
            else
                DteData2 = DateTime.UtcNow;



            int CodGpoPess = 0;
            int pessoaEmpresa = 0;
            int pessoaCliente = 0;
            int pessoaFornecedor = 0;
            int pessoaTransportador = 0;
            int pessoaVendedor = 0;
            int pessoaUsuario = 0;
            int pessoaComprador = 0;

            if (chkEmpresa.Checked == true)
                pessoaEmpresa = 1;
            if (chkCliente.Checked == true)
                pessoaCliente = 1;
            if (chkFornecedor.Checked == true)
                pessoaFornecedor = 1;
            if (chkTransportador.Checked == true)
                pessoaTransportador = 1;
            if (chkVendedor.Checked == true)
                pessoaVendedor = 1;
            if (chkUsuario.Checked == true)
                pessoaUsuario = 1;
            if (chkComprador.Checked == true)
                pessoaComprador= 1;

            if (Session["CodUsuario"].ToString() != "-150380")
            {
                /*
                if (ddlRegTributario.Text != "..... SELECIONE UM REGIME DE TRIBUTAÇÃO .....")
                    CodTrib = Convert.ToInt32(ddlRegTributario.SelectedValue);

                if (ddlGpoTribPessoa.Text != "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PESSOAS .....")
                    CodGpo = Convert.ToInt32(ddlGpoTribPessoa.SelectedValue);
                    */
                if (ddlGpoPessoa.Text != "..... SELECIONE UM GRUPO DE PESSOA.....")
                    CodGpoPess = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            }
            
            Pessoa x1 = new Pessoa(CodPess, txtCodSisAnterior.Text, txtRazSocial.Text, txtNomeFantasia.Text,
                                DteData1, DteData2,
                                Convert.ToInt32(txtCodSituacao.Text), Convert.ToInt32(txtCodFase.Text), CodGpoPess, pessoaEmpresa, pessoaCliente, pessoaFornecedor, 
                                pessoaTransportador, pessoaVendedor, pessoaUsuario, pessoaComprador);

            PanelSelect = "contact";
            Session["TabFocada"] = "contact";

            listCadPessoa = new List<Pessoa>();
            listCadPessoa.Add(x1);
            Session["CadPessoa"] = listCadPessoa;
            Session["RetornoCadPessoa"] = listCadPessoa;
            Session["ContatoPessoa"] = listCadPessoaContato;
            Session["CadPessoa_Contato"] = HttpUtility.HtmlDecode(grdContato.SelectedRow.Cells[0].Text);

            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Contato.aspx?cad=" + Convert.ToInt32(Request.QueryString["Cad"]));
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa_Contato.aspx");
        }
        protected void BtnAddGpoPessoa_Click(object sender, EventArgs e)
        {
            long CodPess = 0;
            DateTime DteData1;
            DateTime DteData2;

            
            if (txtCodPessoa.Text != "Novo")
                CodPess = Convert.ToInt64(txtCodPessoa.Text);

            if (txtDataCadastro.Text != "")
            {
                DteData1 = Convert.ToDateTime(txtDataCadastro.Text);
            }
            else
                DteData1 = DateTime.UtcNow;

            if (txtDataAtualizacao.Text != "")
                DteData2 = Convert.ToDateTime(txtDataAtualizacao.Text);
            else
                DteData2 = DateTime.UtcNow;

            int CodGpoPess = 0;
            
            int pessoaEmpresa = 0;
            int pessoaCliente = 0;
            int pessoaFornecedor = 0;
            int pessoaTransportador = 0;
            int pessoaVendedor = 0;
            int pessoaUsuario = 0;
            int pessoaComprador = 0;

            if (chkEmpresa.Checked == true)
                pessoaEmpresa = 1;
            if (chkCliente.Checked == true)
                pessoaCliente = 1;
            if (chkFornecedor.Checked == true)
                pessoaFornecedor = 1;
            if (chkTransportador.Checked == true)
                pessoaTransportador = 1;
            if (chkVendedor.Checked == true)
                pessoaVendedor = 1;
            if (chkUsuario.Checked == true)
                pessoaUsuario = 1;
            if (chkComprador.Checked == true)
                pessoaComprador= 1;


            Pessoa x1 = new Pessoa(CodPess, txtCodSisAnterior.Text, txtRazSocial.Text, txtNomeFantasia.Text,
                                DteData1, DteData2,
                                Convert.ToInt32(txtCodSituacao.Text), Convert.ToInt32(txtCodFase.Text), CodGpoPess, pessoaEmpresa, pessoaCliente, 
                                pessoaFornecedor, pessoaTransportador, pessoaVendedor, pessoaUsuario, pessoaComprador);


            listCadPessoa = new List<Pessoa>();
            listCadPessoa.Add(x1);
            Session["RetornoCadPessoa"] = listCadPessoa;

            Session["EnderecoPessoa"] = listCadPessoaEndereco;
            Session["ContatoPessoa"] = listCadPessoaContato;
            Session["InscricaoPessoa"] = listCadPessoaInscricao;

            Session["CadPessoa_Inscricao"] = "Novo";
            if(Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadGpoPessoa.aspx?cad=" + Convert.ToInt32(Request.QueryString["Cad"]));
            else
                Response.Redirect("~/Pages/Pessoas/CadGpoPessoa.aspx");
        }

        protected void txtRazSocial_TextChanged(object sender, EventArgs e)
        {
            if (txtNomeFantasia.Text == "")
                txtNomeFantasia.Text = txtRazSocial.Text;
        }

        protected void SalvarFotoContato(Int64 intCodigoPessoa,int intCodigoContato, byte[] byteFoto)
        {
            try
            {
                if (byteFoto.Length > 0)
                {
                    AnexoDocumentoDAL ad = new AnexoDocumentoDAL();
                    string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Scripts\CapturaWebcamASPNET\uploads\";
                    byte[] XMLBinary = byteFoto;

                    string GUIDXML = intCodigoPessoa + "-" + txtRazSocial.Text + "-" + intCodigoContato + ".jpg";

                    if (System.IO.File.Exists(CaminhoArquivoLog + GUIDXML))
                        System.IO.File.Delete(CaminhoArquivoLog + GUIDXML);

                    FileStream file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Create);

                    BinaryWriter bw = new BinaryWriter(file);
                    bw.Write(XMLBinary);
                    bw.Close();

                    file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Open);
                    BinaryReader br = new BinaryReader(file);
                    file.Close();            
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                Session["CaminhoFotoContato"] = null;
            }
        }

        protected void ddlGpoPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtCodSisAnterior.Text == "")
                {
                    GrupoPessoa GP = new GrupoPessoa();
                    GrupoPessoaDAL GPDAL = new GrupoPessoaDAL();
                    GP = GPDAL.PesquisarGrupoPessoa(Convert.ToInt32(ddlGpoPessoa.SelectedValue));

                    if (GP.GerarMatricula)
                    {
                        DataTable dt = new DataTable();
                        PessoaDAL p = new PessoaDAL();
                        dt = p.RelatorioMatriculados(0);

                        txtCodSisAnterior.Text = (dt.Rows.Count + 1).ToString("D3");
                    }
                    else
                    {
                        txtCodSisAnterior.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdRepresentantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
                Session["TabFocada"] = "parameter";
                PanelSelect = "parameter";

                listRepresentantes.RemoveAll(x => x.CodigoVendedor == Convert.ToInt64(HttpUtility.HtmlDecode(grdRepresentantes.SelectedRow.Cells[0].Text))); 
                
                grdRepresentantes.DataSource = listRepresentantes;
                grdRepresentantes.DataBind();
                Session["ListaRepresentantes"] = listRepresentantes;
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void BtnAddRepresentante_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlRepresentantes.SelectedValue == "*Nenhum Selecionado")
                {
                    ShowMessage("Selecione um representante", MessageType.Info);
                    return;
                }

                foreach (Vendedor item in listRepresentantes)
                {
                    if (ddlRepresentantes.SelectedValue == item.CodigoVendedor.ToString())
                    {
                        ShowMessage("Representante já adicionado", MessageType.Info);
                        return;
                    }
                }
   
                VendedorDAL vendDAL = new VendedorDAL();                
                listRepresentantes.Add(vendDAL.PesquisarVendedor(Convert.ToInt32(ddlRepresentantes.SelectedValue)));

                Session["ListaRepresentantes"] = listRepresentantes;
                grdRepresentantes.DataSource = listRepresentantes;
                grdRepresentantes.DataBind();

                //ddlRepresentantes.SelectedValue = "*Nenhum Selecionado";

                Session["TabFocada"] = "parameter";
                PanelSelect = "parameter";
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }      
    }
}