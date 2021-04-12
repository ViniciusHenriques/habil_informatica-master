using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;

namespace SoftHabilInformatica.Pages.Servicos
{
    public partial class ManSolAtendimento : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
        List<Habil_Log> ListaLog = new List<Habil_Log>();
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

            DBTabelaDAL RnTab = new DBTabelaDAL();
            txtdtemissao.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss");
            txtCodPessoa.Text = "";
            //txtDescricao.Text = "";
            txtNroDocumento.Text = "";
            txtNroSerie.Text = "";
            txtHRSPrevistas.Text = "0,00";
            txtValorTotal.Text = "0,00";
        }

        protected void CarregaTiposSituacoes()
        {
            EmpresaDAL RnEmpresa = new EmpresaDAL();
            ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("CD_SITUACAO", "INT", "1", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();
            if (Session["CodEmpresa"] != null)
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();

            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.SituacaoSolicitacaoAtendimento();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();
            ddlSituacao.SelectedValue = "94";

            ddlTipoSolicitacao.DataSource = sd.TipoSolicitacaoAtendimento();
            ddlTipoSolicitacao.DataTextField = "DescricaoTipo";
            ddlTipoSolicitacao.DataValueField = "CodigoTipo";
            ddlTipoSolicitacao.DataBind();

        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Data de Emissão", txtdtemissao.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtdtemissao.Focus();
                }
                return false;
            }


            v.CampoValido("Código Cliente", txtCodPessoa.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);


            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodPessoa.Focus();
                }
                return false;
            }

            v.CampoValido("Descrição da Solicitação", litDescricao.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);

                }

                return false;
            }

            v.CampoValido("Email Solicitante", txtEmail.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtEmail.Focus();

                }

                return false;
            }
            v.CampoValido("Telefone Solicitante", txtTelefone.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtTelefone.Focus();

                }

                return false;
            }
            if(lblNivel.Text != " - Alto" && lblNivel.Text != " - Médio" && lblNivel.Text != " - Baixo")
            {
                ShowMessage("Escolha um nível de Prioridade", MessageType.Info);
                return false;
            }
            if(ddlContato.SelectedValue == "..... SELECIONE UM CONTATO .....")
            {
                ShowMessage("Selecione um Contato", MessageType.Info);
                ddlContato.Focus();
                return false;
            }
            if (ddlTipoSolicitacao.SelectedValue == "96") {
                if (txtHRSPrevistas.Text == "0,00")
                {
                    ShowMessage("Insira uma previsão de horas", MessageType.Info);
                    txtHRSPrevistas.Focus();
                    return false;
                }
                if (txtValorTotal.Text == "0,00")
                {
                    ShowMessage("Insira um valor total", MessageType.Info);
                    txtHRSPrevistas.Focus();
                    return false;
                }
            }
            return true;
        }

        protected void MontaTela(long CodRegra)
        {

            LimpaTela();

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

            if (Session["TabFocada"] != null)
            {
                PanelSelect = Session["TabFocada"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConSolAtendimento.aspx");
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
                if (Session["ZoomSolicitacaoAtendimento2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomSolicitacaoAtendimento"] != null)
                {
                    string s = Session["ZoomSolicitacaoAtendimento"].ToString();
                    Session["ZoomSolicitacaoAtendimento"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;
                                CarregaTiposSituacoes();

                                Doc_SolicitacaoAtendimento doc = new Doc_SolicitacaoAtendimento();
                                Doc_SolicitacaoAtendimentoDAL docDAL = new Doc_SolicitacaoAtendimentoDAL();

                                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));
                                ddlTipoSolicitacao.SelectedValue = doc.CodigoTipoSolicitacao.ToString();
                                if(doc.DataConclusao.ToString() != "01/01/1900 00:00:00")
                                    txtDtConclusao.Text = doc.DataConclusao.ToString("dd/MM/yyyy");
                                txtdtemissao.Text = doc.DataHoraEmissao.ToString();
                                ddlEmpresa.SelectedValue = doc.CodigoEmpresa.ToString();
                                ddlSituacao.SelectedValue = doc.CodigoSituacao.ToString();
                                txtHRSPrevistas.Text = doc.HorasPrevistas.ToString();
                                txtValorTotal.Text = doc.ValorTotal.ToString();
                                txtNroDocumento.Text = doc.NumeroDocumento.ToString();
                                txtNroSerie.Text = doc.DGSerieDocumento;
                                txtEmail.Text = doc.Cpl_MailSolicitante.ToString();
                                txtTelefone.Text = doc.Cpl_FoneSolicitante.ToString();
                                txtCodPessoa.Text = Convert.ToString(doc.Cpl_CodigoPessoa);
                                SelectedPessoa(sender, e);
                                ddlEmpresa_TextChanged(sender, e);
                                PanelSelect = "home";
                                Session["TabFocada"] = "home";

                                string str1 = Server.HtmlDecode(doc.DescricaoDocumento);//CKEditor
                                litDescricao.Text = str1;

                                if (doc.CodigoNivelPrioridade == 90)
                                    btnNivelBaixo_Click(sender, e);
                                else if (doc.CodigoNivelPrioridade == 91)
                                    btnNivelMedio_Click(sender, e);
                                else if (doc.CodigoNivelPrioridade == 92)
                                    btnNivelAlto_Click(sender, e);

                                ddlTipoSolicitacao.Enabled = false;
                                PessoaContatoDAL ctt = new PessoaContatoDAL();
                                ddlContato.DataSource = ctt.ObterPessoaContatos(doc.Cpl_CodigoPessoa);
                                ddlContato.DataTextField = "_NomeContatoCombo";
                                ddlContato.DataValueField = "_CodigoItem";
                                ddlContato.DataBind();
                                ddlContato.Items.Insert(0, "..... SELECIONE UM CONTATO .....");
                                ddlContato.SelectedValue = doc.CodigoContato.ToString();

                                EventoDocumentoDAL eve = new EventoDocumentoDAL();
                                ListaEvento = eve.ObterEventos(Convert.ToInt64(txtCodigo.Text));
                                Session["Eventos"] = ListaEvento;

                                Habil_LogDAL log = new Habil_LogDAL();
                                ListaLog = log.ListarLogs(Convert.ToDouble(txtCodigo.Text), 100);
                                Session["Logs"] = ListaLog;

                                AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
                                ListaAnexo = anexo.ObterAnexos(Convert.ToInt32(txtCodigo.Text));
                                Session["NovoAnexo"] = ListaAnexo;

                            }
                        }
                    }
                }
                else
                {
                    LimpaTela();
                    txtCodigo.Text = "Novo";
                    btnExcluir.Visible = false;
                    ddlEmpresa_TextChanged(sender, e);
                    btnNovoAnexo.Visible = false;
                    btnNivelBaixo_Click(sender, e);

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
                if (Session["Doc_SolicitacaoAtendimento"] != null)
                {
                    CarregaTiposSituacoes();
                    if (Session["Doc_SolicitacaoAtendimento2"] != null)
                    {
                        string s = Session["Doc_SolicitacaoAtendimento2"].ToString();
                        Session["Doc_SolicitacaoAtendimento2"] = null;

                        string[] words = s.Split('³');

                        if (s != "³")
                        {
                            foreach (string word in words)
                            {
                                if (word != "")
                                {
                                    txtCodPessoa.Text = word;
                                    PreencheDados(sender,e,Convert.ToInt32(txtCodPessoa.Text));
                                    SelectedPessoa(sender, e);
                                    ddlEmpresa_TextChanged(sender, e);
                                }
                            }
                        }
                    }
                    else
                    {
                        btnExcluir.Visible = true;
                        PreencheDados(sender,e,0);
                        SelectedPessoa(sender, e);
                        Doc_SolicitacaoAtendimento doc = (Doc_SolicitacaoAtendimento)Session["Doc_SolicitacaoAtendimento"];
                        if (doc.CodigoContato != 0)
                            ddlContato.SelectedValue = doc.CodigoContato.ToString();
                        ddlEmpresa_TextChanged(sender, e);
                    }
                    Session["Doc_SolicitacaoAtendimento"] = null;
                }
            }

            if (Session["Eventos"] != null)
            {
                ListaEvento = (List<EventoDocumento>)Session["Eventos"];
                GrdEventoDocumento.DataSource = ListaEvento;
                GrdEventoDocumento.DataBind();
            }

            if (Session["Logs"] != null)
            {
                ListaLog = (List<Habil_Log>)Session["Logs"];
                grdLogDocumento.DataSource = ListaLog;
                grdLogDocumento.DataBind();
            }

            if (Session["NovoAnexo"] != null)
            {
                ListaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];
                grdAnexo.DataSource = ListaAnexo;
                grdAnexo.DataBind();
            }

            if (txtCodigo.Text == "")
            {
                btnVoltar_Click(sender, e);
            }
            if (txtCodigo.Text != "Novo")
            {
                btnNovoAnexo.Visible = true;
                ddlEmpresa.Enabled = false;
                ddlTipoSolicitacao.Enabled = false;
            }
            else
            {
                ddlTipoSolicitacao.Enabled = true;

            }

            txtNroDocumento.Enabled = false;
            txtNroSerie.Enabled = false;

        }

        protected void PreencheDados(object sender, EventArgs e,int CodPessoa)
        {
            Doc_SolicitacaoAtendimento doc = (Doc_SolicitacaoAtendimento)Session["Doc_SolicitacaoAtendimento"];

            if (doc.CodigoDocumento == 0)
            {
                txtCodigo.Text = "Novo";
                btnExcluir.Visible = false;
                
            }
            else
            {
                txtCodigo.Text = Convert.ToString(doc.CodigoDocumento);
                btnExcluir.Visible = true;
            }

            if (Convert.ToString(doc.DataHoraEmissao) != "01/01/0001 00:00:00")
                txtdtemissao.Text = doc.DataHoraEmissao.ToString();
            if (Convert.ToString(doc.DataConclusao) != "01/01/0001 00:00:00" && Convert.ToString(doc.DataConclusao) != "01/01/1900 00:00:00")
                txtDtConclusao.Text = doc.DataConclusao.ToString();

            ddlTipoSolicitacao.SelectedValue = doc.CodigoTipoSolicitacao.ToString();
            ddlEmpresa.SelectedValue = Convert.ToString(doc.CodigoEmpresa);
            ddlSituacao.SelectedValue = Convert.ToString(doc.CodigoSituacao);
            txtNroDocumento.Text = Convert.ToString(doc.NumeroDocumento);
            txtValorTotal.Text = doc.ValorTotal.ToString();
            txtHRSPrevistas.Text = doc.HorasPrevistas.ToString();
            txtNroSerie.Text = Convert.ToString(doc.DGSerieDocumento);
            txtEmail.Text = doc.Cpl_MailSolicitante;
            txtTelefone.Text = doc.Cpl_FoneSolicitante;

            if (doc.NumeroDocumento == 0)
                txtNroDocumento.Text = "";

            if (CodPessoa == 0)
            {
                if (doc.Cpl_CodigoPessoa == 0)
                    txtCodPessoa.Text = "";
                else
                    txtCodPessoa.Text = Convert.ToString(doc.Cpl_CodigoPessoa);
            }

            if (doc.CodigoNivelPrioridade == 90)
                btnNivelBaixo_Click(sender, e);
            else if (doc.CodigoNivelPrioridade == 91)
                btnNivelMedio_Click(sender, e);
            else if (doc.CodigoNivelPrioridade == 92)
                btnNivelAlto_Click(sender, e);

            if (txtCodigo.Text == "Novo")
                ddlTipoSolicitacao.Enabled = false;
            if(Session["RascunhoDocumento"] != null)
            {
                string str = Server.HtmlDecode(Session["RascunhoDocumento"].ToString());
                litDescricao.Text = str;
                Session["RascunhoDocumento"] = null;
            }
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            Doc_SolicitacaoAtendimentoDAL doc = new Doc_SolicitacaoAtendimentoDAL();
            doc.Excluir(Convert.ToDecimal(txtCodigo.Text));


            Session["MensagemTela"] = "Documento Excluído com sucesso!";
            btnVoltar_Click(sender, e);
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Servicos/ConSolAtendimento.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            Doc_SolicitacaoAtendimento p = new Doc_SolicitacaoAtendimento();
            Doc_SolicitacaoAtendimentoDAL pe = new Doc_SolicitacaoAtendimentoDAL();

            p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            p.Cpl_CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
            p.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
            p.DGSerieDocumento = txtNroSerie.Text;
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.CodigoTipoSolicitacao = Convert.ToInt32(ddlTipoSolicitacao.SelectedValue);
            p.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
            p.Cpl_FoneSolicitante = txtTelefone.Text;
            p.Cpl_MailSolicitante = txtEmail.Text;
            p.CodigoContato = Convert.ToInt32(ddlContato.SelectedValue);
            p.HorasPrevistas = Convert.ToDecimal(txtHRSPrevistas.Text);
            p.ValorTotal = Convert.ToDecimal(txtValorTotal.Text);

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            string str2 = litDescricao.Text;
            p.DescricaoDocumento = str2;


            p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
            p.Cpl_Usuario = Convert.ToInt32(Session["CodPflUsuario"]);

            if (lblNivel.Text == " - Baixo")
                p.CodigoNivelPrioridade = 90;
            else if (lblNivel.Text == " - Médio")
                p.CodigoNivelPrioridade = 91;
            else if (lblNivel.Text == " - Alto")
                p.CodigoNivelPrioridade = 92;

            if(ddlSituacao.SelectedValue == "89")
            {
                DBTabelaDAL RnTab = new DBTabelaDAL();
                p.DataConclusao = RnTab.ObterDataHoraServidor();
            }
            else
            {
                p.DataConclusao = Convert.ToDateTime("01/01/0001 00:00:00");
              
            }

            if (txtCodigo.Text == "Novo")
            {
                ddlEmpresa_TextChanged(sender, e);
                p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Session["CodigoGeradorSequencial"]);
                p.Cpl_NomeTabela = Session["NomeTabela"].ToString();

                Session["CodigoGeradorSequencial"] = null;
                pe.Inserir(p, EventoDocumento(), ListaAnexo);

            }
            else
            {

                Doc_SolicitacaoAtendimento p2 = new Doc_SolicitacaoAtendimento();
                p.CodigoDocumento = Convert.ToInt64(txtCodigo.Text);

                p2 = pe.PesquisarDocumento(Convert.ToDecimal(txtCodigo.Text));
                if (Convert.ToInt32(ddlSituacao.SelectedValue) != p2.CodigoSituacao)
                    pe.Atualizar(p, EventoDocumento(), ListaAnexo);
                else
                    pe.Atualizar(p, null, ListaAnexo);
            }

            Session["MensagemTela"] = null;
            if (txtCodigo.Text == "Novo")
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            else
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";

            Session["NomeTabela"] = null;

            btnVoltar_Click(sender, e);
           
        }
        
        protected void CompactaDocumento()
        {
            Doc_SolicitacaoAtendimento doc = new Doc_SolicitacaoAtendimento();

            if (txtCodigo.Text == "Novo")
                doc.CodigoDocumento = 0;
            else
                doc.CodigoDocumento = Convert.ToDecimal(txtCodigo.Text);

            if (txtdtemissao.Text != "")
                doc.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);

            doc.CodigoSituacao = Convert.ToInt32(ddlSituacao.Text);
            doc.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.Text);
            doc.CodigoTipoSolicitacao = Convert.ToInt32(ddlTipoSolicitacao.Text);
            doc.DGSerieDocumento = txtNroSerie.Text;
            doc.HorasPrevistas = Convert.ToDecimal(txtHRSPrevistas.Text);
            doc.ValorTotal = Convert.ToDecimal(txtValorTotal.Text);
            doc.Cpl_MailSolicitante = txtEmail.Text;
            doc.Cpl_FoneSolicitante = txtTelefone.Text;

            decimal numero;
            if (decimal.TryParse(txtNroDocumento.Text, out numero))
            {
                doc.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
            }
            else
            {
                doc.NumeroDocumento = 0;
            }
            if (doc.DataConclusao.ToString() != "01/01/1900 00:00:00" || doc.DataConclusao.ToString() != "01/01/0001 00:00:00")
                txtDtConclusao.Text = doc.DataConclusao.ToString("dd/MM/yyyy");

            if (lblNivel.Text == " - Baixo")
                doc.CodigoNivelPrioridade = 90;
            else if (lblNivel.Text == " - Médio")
                doc.CodigoNivelPrioridade = 91;
            else if (lblNivel.Text == " - Alto")
                doc.CodigoNivelPrioridade = 92;

            if(ddlContato.SelectedValue != "..... SELECIONE UM CONTATO ....." && ddlContato.SelectedValue != "")
                doc.CodigoContato = Convert.ToInt32(ddlContato.SelectedValue);

            if (txtCodPessoa.Text == "")
                doc.Cpl_CodigoPessoa = 0;
            else
                doc.Cpl_CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);

            string str = litDescricao.Text;//CKeditor
            string str2 = Server.HtmlDecode(str);
            doc.DescricaoDocumento = str2;
            Session["RascunhoDocumento"] = str2;
            Session["Doc_SolicitacaoAtendimento"] = doc;
            Session["IncMovAcessoContato"] = null;
            Session["IncMovAcessoPessoa"] = null;
        }

        protected void ConPessoa(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=9");
        }

        protected void SelectedPessoa(object sender, EventArgs e)
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
            Int64 numero;
            if (!Int64.TryParse(txtCodPessoa.Text, out numero))
            {
                return;
            }

            if (txtCodigo.Text != "Novo")
            {
                Doc_SolicitacaoAtendimento doc = new Doc_SolicitacaoAtendimento();
                Doc_SolicitacaoAtendimentoDAL docDAL = new Doc_SolicitacaoAtendimentoDAL();
                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));
                if (doc.Cpl_CodigoPessoa != Convert.ToInt32(txtCodPessoa.Text))
                {
                    ShowMessage("Você alterou a pessoa do Documento!", MessageType.Info);
                }
            }

            Int64 codigoPessoa = Convert.ToInt64(txtCodPessoa.Text);
            PessoaDAL pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();

            PessoaInscricaoDAL ins = new PessoaInscricaoDAL();
            List<Pessoa_Inscricao> ins2 = new List<Pessoa_Inscricao>();
            ins2 = ins.ObterPessoaInscricoes(codigoPessoa);
            p2 = pessoa.PesquisarCliente(codigoPessoa);

            if (p2 == null)
            {
                ShowMessage("Pessoa não existente!", MessageType.Info);
                txtCodPessoa.Text = "";
                txtPessoa.Text = "";
                txtCodPessoa.Focus();

                return;
            }

            PessoaContatoDAL ctt = new PessoaContatoDAL();
            ddlContato.DataSource = ctt.ObterPessoaContatos(codigoPessoa);
            ddlContato.DataTextField = "_NomeContatoCombo";
            ddlContato.DataValueField = "_CodigoItem";
            ddlContato.DataBind();
            ddlContato.Items.Insert(0, "..... SELECIONE UM CONTATO .....");
            
            txtPessoa.Text = p2.NomePessoa;
            //txtDescricao.Focus();
            Session["TabFocada"] = null;
            //Session["Doc_SolicitacaoAtendimento"] = null;
        }

        protected void ddlEmpresa_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "Novo")
                return;

            DBTabelaDAL db = new DBTabelaDAL();
            List<GeracaoSequencialDocumento> ListaGerDoc = new List<GeracaoSequencialDocumento>();
            GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
            GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
            ListaGerDoc = gerDocDAL.ListarGeracaoSequencial("CD_TIPO_DOCUMENTO", "INT", "6", "");

            // Se existe a tabela sequencial
            if(ListaGerDoc.Count == 0)
            {
                Session["MensagemTela"] = "Gerador Sequencial não iniciado";
                btnVoltar_Click(sender,e);
            }
            foreach (GeracaoSequencialDocumento ger in ListaGerDoc)
            {

                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
                if (ger.Nome == "" || ger.CodigoSituacao == 2)
                {
                    Session["MensagemTela"] = "Gerador Sequencial não iniciado";
                    btnVoltar_Click(sender, e);
                }
                else
                {
                    if (ger.Validade > DateTime.Now)
                    {
                        Session["MensagemTela"] = "Gerador Sequencial venceu em " + ger.Validade.ToString("dd/MM/yyyy");
                        btnVoltar_Click(sender, e);
                    }
                }
                Session["NomeTabela"] = ger.Nome;
                Session["CodigoGeradorSequencial"] = ger.CodigoGeracaoSequencial;

                double NroSequencial = geradorDAL.ExibeProximoNroSequencial(ger.Nome);
                if (NroSequencial == 0)
                    txtNroDocumento.Text = ger.NumeroInicial.ToString();
                else
                    txtNroDocumento.Text = NroSequencial.ToString();

                txtNroSerie.Text = ger.SerieConteudo.ToString();

            }
        }

        protected void grdAnexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["ZoomDocCtaAnexo"] = HttpUtility.HtmlDecode(grdAnexo.SelectedRow.Cells[0].Text);
            Response.Redirect("~/Pages/financeiros/ManAnexo.aspx?cad=5");
        }

        protected void btnNovoAnexo_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=5");
        }

        protected EventoDocumento EventoDocumento()
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime DataHoraEvento = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm"));

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            int intCttItem = 0;

            if (GrdEventoDocumento.Rows.Count != 0)
                intCttItem = Convert.ToInt32(ListaEvento.Max(x => x.CodigoEvento).ToString());


            intCttItem = intCttItem + 1;

            if (intCttItem != 0)
                ListaEvento.RemoveAll(x => x.CodigoEvento == intCttItem);
            EventoDocumento evento = new EventoDocumento(intCttItem,
                                                       Convert.ToInt32(ddlSituacao.SelectedValue),
                                                       DataHoraEvento,
                                                       he.CodigoEstacao,
                                                       Convert.ToInt32(Session["CodUsuario"]));
            return evento;

        }

        protected void grdLogDocumento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PanelSelect = "consulta3";
            grdLogDocumento.PageIndex = e.NewPageIndex;
            ListaLog = (List<Habil_Log>)Session["Logs"];
            grdLogDocumento.DataSource = ListaLog;
            grdLogDocumento.DataBind();

        }

        protected void grdLogDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSair_Click(object sender, EventArgs e)
        {

        }

        protected void btnNivelMedio_Click(object sender, EventArgs e)
        {
            lblNivel.Text = " - Médio";
        }

        protected void btnNivelAlto_Click(object sender, EventArgs e)
        {
            lblNivel.Text = " - Alto";
        }

        protected void btnNivelBaixo_Click(object sender, EventArgs e)
        {
            lblNivel.Text = " - Baixo";
        }

        protected void BtnAddContato_Click(object sender, EventArgs e)
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
            Int64 numero;
            if (!Int64.TryParse(txtCodPessoa.Text, out numero))
            {
                return;
            }

            CompactaDocumento();

            Session["ZoomPessoa"] = txtCodPessoa.Text;
            Session["ZoomCadPessoa3"] = "RELACIONAL";
            Session["TabFocada"] = "contact";
            Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx?cad=1");
            
        }

        protected void ddlContato_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlContato.SelectedValue == "..... SELECIONE UM CONTATO .....")
            {
                txtTelefone.Text = "";
                txtEmail.Text = "";
                return;
            }
                

            Pessoa_Contato Ctt = new Pessoa_Contato();
            PessoaContatoDAL CttDAL = new PessoaContatoDAL();
            Ctt = CttDAL.PesquisarPessoaContato(Convert.ToInt32(txtCodPessoa.Text), Convert.ToInt32(ddlContato.SelectedValue));
            txtEmail.Text = Ctt._Mail1;
            txtTelefone.Text = Ctt._Fone1;
        }

        protected void BtnEditarDS_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            
            Response.Redirect("~/Pages/Servicos/ManRasDocumento.aspx?cad=1");
            
        }

        protected void txtHRSPrevistas_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtHRSPrevistas.Text.Equals(""))
            {
                txtHRSPrevistas.Text = "0,00";
            }
            else
            {
                v.CampoValido("Horas Previstas", txtHRSPrevistas.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtHRSPrevistas.Text = Convert.ToDecimal(txtHRSPrevistas.Text).ToString("###,##0.00");
                    txtValorTotal.Focus();

                }
                else
                    txtHRSPrevistas.Text = "0,00";

            }
        }

        protected void txtValorTotal_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtValorTotal.Text.Equals(""))
            {
                txtValorTotal.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor Total", txtValorTotal.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtValorTotal.Text = Convert.ToDecimal(txtValorTotal.Text).ToString("###,##0.00");
                    BtnEditarDS.Focus();

                }
                else
                    txtValorTotal.Text = "0,00";

            }
        }
    }
}