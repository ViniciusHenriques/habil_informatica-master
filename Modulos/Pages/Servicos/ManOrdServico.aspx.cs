using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;
using System.Xml.XPath;
using System.Xml.Linq;
using System.IO;

namespace SoftHabilInformatica.Pages.Servicos
{
    public partial class ManOrdServico: System.Web.UI.Page
    {
        public string TabCobranca { get; set; }
        public string TabEventos{ get; set; }
        public string TabLogs{ get; set; }
        public string TabAnexos { get; set; }
        public string TabOSs{ get; set; }

        public string PanelSelect { get; set; }
        List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
        List<Habil_Log> ListaLog = new List<Habil_Log>();
        List<TipoServico> ListaTipoServico = new List<TipoServico>();
        List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
        List<ItemTipoServico> ListaItemTipoServico = new List<ItemTipoServico>();
        List<ItemDocumento> ListaItemDocumento = new List<ItemDocumento>();
        List<Doc_OrdemServico> ListaOSs = new List<Doc_OrdemServico>();

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
            txtCodigo.Text = "Novo";
            txtOBS.Text = "";
            txtCodUsu.Text = Session["CodUsuario"].ToString();
            
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
            ddlClassificacao.DataSource = sd.TipoOrdemServico();
            ddlClassificacao.DataTextField = "DescricaoTipo";
            ddlClassificacao.DataValueField = "CodigoTipo";
            ddlClassificacao.DataBind();

            if (ddlClassificacao.SelectedValue == "98")
                ddlSituacao.DataSource = sd.SituacaoOrdemFatura();
            else
                ddlSituacao.DataSource = sd.SituacaoOrdemServico();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            CondPagamentoDAL CondPagam = new CondPagamentoDAL();
            ddlPagamento.DataSource = CondPagam.ListarCondPagamento("CD_SITUACAO", "INT", "1", "");
            ddlPagamento.DataTextField = "DescricaoCondPagamento";
            ddlPagamento.DataValueField = "CodigoCondPagamento";
            ddlPagamento.DataBind();
            ddlPagamento.Items.Insert(0, "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....");

            TipoServicoDAL RnTipoServico = new TipoServicoDAL();
            ddlTipoServico.DataSource = RnTipoServico.ListarTipoServico("CD_SITUACAO", "INT", "1", "CD_TIPO_SERVICO");
            ddlTipoServico.DataTextField = "DescricaoTipoServico";
            ddlTipoServico.DataValueField = "CodigoTipoServico";
            ddlTipoServico.DataBind();
            ddlTipoServico.Items.Insert(0, "..... SELECIONE UM TIPO DE SERVIÇO.....");

            TipoCobrancaDAL RnTipoCobranca= new TipoCobrancaDAL();
            ddlTipoCobranca.DataSource = RnTipoCobranca.ListarTipoCobrancas("CD_SITUACAO", "INT", "1", "");
            ddlTipoCobranca.DataTextField = "DescricaoTipoCobranca";
            ddlTipoCobranca.DataValueField = "CodigoTipoCobranca";
            ddlTipoCobranca.DataBind();

            ProdutoDAL ed = new ProdutoDAL();
            ddlProduto.DataSource = ed.ObterTiposServicos();
            ddlProduto.DataTextField = "DescricaoProduto";
            ddlProduto.DataValueField = "CodigoProduto";
            ddlProduto.DataBind();
            ddlProduto.Items.Insert(0, "..... SELECIONE UM PRODUTO.....");
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

            v.CampoValido("Código Usuário responsável", txtCodUsu.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodUsu.Focus();
                }
                return false;
            }

            if (ddlClassificacao.SelectedValue == "98")
            {
                if (grdServicos.Rows.Count == 0)
                {
                    ShowMessage("Adicione um Serviço", MessageType.Info);
                    return false;
                }

                if (grdProduto.Rows.Count == 0)
                {
                    ShowMessage("Adicione um Produto", MessageType.Info);
                    return false;
                }

                if (Convert.ToDecimal(txtVlrTotal.Text.Substring(2)) == 0)
                {
                    ShowMessage("Valor do documento não pode ser zero", MessageType.Info);
                    return false;
                }

                if (ddlPagamento.SelectedValue == "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....")
                {
                    ShowMessage("Selecione uma Condição de pagamento", MessageType.Info);
                    return false;
                }

                if (ddlTipoCobranca.SelectedValue == "..... SELECIONE UM TIPO DE COBRANÇA.....")
                {
                    ShowMessage("Selecione um Tipo de cobrança", MessageType.Info);
                    ddlTipoCobranca.Focus();
                    return false;
                    
                }

                CondPagamento condPag = new CondPagamento();
                CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
                condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
                if (condPag.QtdeParcelas != ListaParcelaDocumento.Count)
                {
                    ShowMessage("Parcelas incompletas", MessageType.Info);
                    return false;
                }
                decimal valorTotal = 0;
                foreach (ParcelaDocumento p in ListaParcelaDocumento)
                {
                    valorTotal = valorTotal + p.ValorParcela;
                }


                if (Convert.ToDecimal(txtVlrTotal.Text.Substring(2)) != valorTotal)
                {
                    ShowMessage("Valor total das parcelas não corresponde ao valor total do serviço", MessageType.Info);
                    return false;
                }
            }
            else
            {
                v.CampoValido("Descrição da Solicitação", litDescricao.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);

                    }

                    return false;
                }
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
            if (ddlContato.SelectedValue == "..... SELECIONE UM CONTATO .....")
            {
                ShowMessage("Selecione um Contato", MessageType.Info);
                return false;
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
                Session["TabFocada"] = null;
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
                                                "ConOrdServico.aspx");
            lista.ForEach(delegate (Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoAlterar)
                        btnSalvar.Visible = false;
                    if (!x.AcessoIncluir)
                        btnFaturar.Visible = false;
                    if (!x.AcessoExcluir)
                        btnExcluir.Visible = false;
                }
            });

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomOrdemServico2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomOrdemServico"] != null)
                {
                    string s = Session["ZoomOrdemServico"].ToString();
                    Session["ZoomOrdemServico"] = null;

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
                                ddlClassificacao.Enabled = false;
                                Doc_OrdemServico doc = new Doc_OrdemServico();
                                Doc_OrdemServicoDAL docDAL = new Doc_OrdemServicoDAL();

                                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));
                                ddlClassificacao.SelectedValue = doc.CodigoClassificacao.ToString();
                                txtdtemissao.Text = doc.DataHoraEmissao.ToString();
                                ddlEmpresa.SelectedValue = doc.CodigoEmpresa.ToString();
                                ddlSituacao.SelectedValue = doc.CodigoSituacao.ToString();                                
                                txtOBS.Text = doc.ObservacaoDocumento;
                                txtNroDocumento.Text = doc.NumeroDocumento.ToString();
                                txtNroSerie.Text = doc.DGSRDocumento;
                                txtEmail.Text = doc.Cpl_MailSolicitante.ToString();
                                txtTelefone.Text = doc.Cpl_FoneSolicitante.ToString();
                                txtCodPessoa.Text = Convert.ToString(doc.Cpl_CodigoPessoa);
                                txtCodUsu.Text = doc.CodigoUsuarioResponsavel.ToString();

                                if(doc.CodigoSolicAtendimento != 0 )
                                    txtCodSolAtendimento.Text = doc.CodigoSolicAtendimento.ToString();

                                string str1 = Server.HtmlDecode(doc.DescricaoDocumento);//Ckeditor
                                litDescricao.Text = str1;
                                
                                txtCodUsu_TextChanged(sender, e);
                                ddlEmpresa_TextChanged(sender, e);
                                PanelSelect = "home";
                                Session["TabFocada"] = "home";

                                Habil_TipoDAL sd = new Habil_TipoDAL();
                                if (ddlClassificacao.SelectedValue == "98")
                                    ddlSituacao.DataSource = sd.SituacaoOrdemFatura();
                                else
                                    ddlSituacao.DataSource = sd.SituacaoOrdemServico();
                                ddlSituacao.DataTextField = "DescricaoTipo";
                                ddlSituacao.DataValueField = "CodigoTipo";
                                ddlSituacao.DataBind();

                                ListaTipoServico = docDAL.ObterTipoServico(Convert.ToDecimal(txtCodigo.Text));
                                Session["ListaTipoServico"] = ListaTipoServico;

                                ListaItemTipoServico = docDAL.ObterProdutoDocumento(Convert.ToDecimal(txtCodigo.Text));
                                Session["ListaItemTipoServico"] = ListaItemTipoServico;

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

                                ParcelaDocumentoDAL ParcelaDAL = new ParcelaDocumentoDAL();
                                ListaParcelaDocumento = ParcelaDAL.ObterParcelaDocumento(Convert.ToInt32(txtCodigo.Text));
                                Session["ListaParcelaDocumento"] = ListaParcelaDocumento;

                                ItemDocumentoDAL item = new ItemDocumentoDAL();
                                if (ddlClassificacao.SelectedValue == "98")
                                {
                                    Doc_OrdemServicoDAL OS = new Doc_OrdemServicoDAL();
                                    ListaOSs = docDAL.ListarOrdemServicoServico(doc.Cpl_CodigoPessoa, 97,Convert.ToDecimal(txtCodigo.Text));
                                    Session["ListaOSs"] = ListaOSs;
                                    foreach(Doc_OrdemServico os in ListaOSs)
                                    {
                                        if(os.BtnAdd == false && os.BtnRemove == true)
                                        {
                                            List<ItemDocumento> ListaNovaItem = new List<ItemDocumento>();
                                            ListaNovaItem = item.ObterItemDocumento(os.CodigoDocumento);
                                            foreach(ItemDocumento itemDoc in ListaNovaItem)
                                            {
                                                ListaItemDocumento.Add(itemDoc);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ListaItemDocumento = item.ObterItemDocumento(Convert.ToInt32(txtCodigo.Text));                                    
                                }
                                Session["ListaItemDocumento"] = ListaItemDocumento;

                                SelectedPessoa(sender, e);

                                ddlTipoCobranca.SelectedValue = doc.CodigoTipoCobranca.ToString();
                                ddlPagamento.SelectedValue = doc.CodigoCondicaoPagamento.ToString();
                                if (doc.CodigoCondicaoPagamento != 0)
                                    GerarParcelaExistente(sender, e);
                            }
                        }
                    }
                }
                else
                {
                    LimpaTela();
                    txtCodUsu_TextChanged(sender, e);
                    ddlClassificacao.Enabled = true;
                    txtCodigo.Text = "Novo";
                    btnExcluir.Visible = false;
                    ddlEmpresa_TextChanged(sender, e);
                    txtCodUsu_TextChanged(sender, e);
                    btnNovoAnexo.Visible = false;
                    if (Session["Doc_OrdemServico"] == null)
                    {
                        Session["ListaTipoServico"] = null;
                        Session["ListaItemTipoServico"] = null;
                    }
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                            {
                                btnSalvar.Visible = false;
                                btnFaturar.Visible = false;
                            }
                            if (!p.AcessoExcluir)
                                btnExcluir.Visible = false;
                        }
                    });
                }
                if (Session["Doc_OrdemServico"] != null)
                {
                    CarregaTiposSituacoes();
                    if (Session["Doc_OrdemServico2"] != null)
                    {
                        string s = Session["Doc_OrdemServico2"].ToString();
                        Session["Doc_OrdemServico2"] = null;

                        string[] words = s.Split('³');

                        if (s != "³")
                        {
                            foreach (string word in words)
                            {
                                if (word != "")
                                {
                                    txtCodPessoa.Text = word;
                                    PreencheDados(sender, e, Convert.ToInt32(txtCodPessoa.Text),0);
                                    SelectedPessoa(sender, e);
                                    ddlPagamento_TextChanged(sender, e);
                                    txtCodUsu_TextChanged(sender, e);
                                    ddlEmpresa_TextChanged(sender, e);
                                }
                            }
                        }
                    }
                    else if (Session["Doc_OrdemServicoUsuario"] != null)
                    {
                        string s = Session["Doc_OrdemServicoUsuario"].ToString();
                        Session["Doc_OrdemServicoUsuario"] = null;

                        string[] words = s.Split('³');

                        if (s != "³")
                        {
                            foreach (string word in words)
                            {
                                if (word != "")
                                {
                                    txtCodUsu.Text = word;
                                    txtCodUsu_TextChanged(sender, e);
                                    PreencheDados(sender, e, 0,Convert.ToInt32(txtCodUsu.Text));
                                    SelectedPessoa(sender, e);
                                    ddlPagamento_TextChanged(sender, e);
                                    ddlEmpresa_TextChanged(sender, e);
                                }
                            }
                        }
                    }
                    else
                    {
                        btnExcluir.Visible = true;
                        PreencheDados(sender, e, 0,0);
                        txtCodUsu_TextChanged(sender, e);
                        SelectedPessoa(sender, e);
                        ddlPagamento_TextChanged(sender, e);
                        Doc_OrdemServico doc = (Doc_OrdemServico)Session["Doc_OrdemServico"];
                        if (doc.CodigoContato != 0)
                            ddlContato.SelectedValue = doc.CodigoContato.ToString();
                        ddlEmpresa_TextChanged(sender, e);
                    }
                    Session["Doc_OrdemServico"] = null;
                }
                if (Session["ListaTipoServico"] != null)
                {
                    ListaTipoServico = (List<TipoServico>)Session["ListaTipoServico"];

                    if (ListaTipoServico.Count != 0)
                    {
                        ddlTipoServicoAdicionado.DataSource = ListaTipoServico;
                        ddlTipoServicoAdicionado.DataTextField = "DescricaoTipoServico";
                        ddlTipoServicoAdicionado.DataValueField = "CodigoServico";
                        ddlTipoServicoAdicionado.DataBind();
                        ddlTipoServicoAdicionado.Items.Insert(0, "..... SELECIONE UM SERVIÇO.....");
                    }
                }
            }
            if (Session["ListaTipoServico"] != null)
            {
                ListaTipoServico = (List<TipoServico>)Session["ListaTipoServico"];
                grdServicos.DataSource = ListaTipoServico;
                grdServicos.DataBind();
            }
            if (Session["ListaItemDocumento"] != null)
            {
                ListaItemDocumento = (List<ItemDocumento>)Session["ListaItemDocumento"];
                grdItemDocumento.DataSource = ListaItemDocumento;
                grdItemDocumento.DataBind();
            }

            if (Session["ListaOSs"] != null)
            {
                ListaOSs = (List<Doc_OrdemServico>)Session["ListaOSs"];
                grdOSs.DataSource = ListaOSs;
                grdOSs.DataBind();
            }
            if (Session["ListaItemTipoServico"] != null)
            {
                ListaItemTipoServico = (List<ItemTipoServico>)Session["ListaItemTipoServico"];
                MontarValorTotal(ListaItemTipoServico);
                grdProduto.DataSource = ListaItemTipoServico;
                grdProduto.DataBind();
            }
            if (Session["Eventos"] != null)
            {
                ListaEvento = (List<EventoDocumento>)Session["Eventos"];
                GrdEventoDocumento.DataSource = ListaEvento;
                GrdEventoDocumento.DataBind();
                TabEventos = "display:block";

                if (ListaEvento.Count == 0)
                    TabEventos = "display:none";
            }
            else
            {
                TabEventos = "display:none";
            }

            if (Session["Logs"] != null)
            {
                ListaLog = (List<Habil_Log>)Session["Logs"];
                grdLogDocumento.DataSource = ListaLog;
                grdLogDocumento.DataBind();               
                TabLogs= "display:block";

                if (ListaLog.Count == 0)
                    TabLogs = "display:none";
            }
            else
            {
                TabLogs = "display:none";
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
            else if (txtCodigo.Text != "Novo")
            {
                btnNovoAnexo.Visible = true;
                ddlEmpresa.Enabled = false;
                TabAnexos = "display:block";
            }else
                TabAnexos = "display:none";

            if (ddlClassificacao.SelectedValue == "98")
            {
                if (txtCodigo.Text != "Novo" && txtCodigo.Text != "")
                {
                    List<Doc_NotaFiscalServico> ListaNfse = new List<Doc_NotaFiscalServico>();
                    Doc_NotaFiscalServicoDAL nfseDAL = new Doc_NotaFiscalServicoDAL();
                    ListaNfse = nfseDAL.ListarNotaFiscalServico("CD_DOC_ORIGINAL", "NUMERIC", txtCodigo.Text, "");
                    if (ListaNfse.Count != 0)
                        btnFaturar.Visible = false;
                   
                }


                btnAddItem.Visible = false;
                ddlSituacao.Enabled = false;
                pnlCobranca.Visible = true;
                lblCampoObrigatorio.Visible = false;
                TabCobranca = "display:block";
                TabOSs = "display:block";
            }
            else
            {
                btnAddItem.Visible = true;
                btnFaturar.Visible = false;
                ddlSituacao.Enabled = true;
                pnlCobranca.Visible = false;
                lblCampoObrigatorio.Visible = true;
                TabCobranca = "display:none";
                TabOSs = "display:none";
            }

            PreencherListaParcelaDocumento(sender,e);           

            if(Session["ZoomSolAtendimento"] != null)
            {
                txtCodSolAtendimento.Text = Session["ZoomSolAtendimento"].ToString();
                txtCodSolAtendimento_TextChanged(sender, e);
                Session["ZoomSolAtendimento"] = null;
            }

            CamposProduto();

            
        }

        protected void PreencherListaParcelaDocumento(object sender, EventArgs e)
        {
            if (Session["ListaParcelaDocumento"] != null)
            {
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
                ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
                grdPagamento.DataSource = ListaParcelaDocumento;
                grdPagamento.DataBind();

                if (ListaParcelaDocumento.Count == 0)
                {
                    pnlParcelas.Visible = false;
                }
                else
                {
                    pnlParcelas.Visible = true;

                    foreach (ParcelaDocumento p in ListaParcelaDocumento)
                    {
                        if (p.CodigoParcela == 1)
                        {
                            btnExcluirParcela.Visible = true;
                            btnAddParcela.Visible = false;
                            txtDt1.Enabled = false;
                            txtDt1.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 2)
                        {
                            btnExcluirParcela2.Visible = true;
                            btnAddParcela2.Visible = false;
                            txtDt2.Enabled = false;
                            txtDt2.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 3)
                        {
                            btnExcluirParcela3.Visible = true;
                            btnAddParcela3.Visible = false;
                            txtDt3.Enabled = false;
                            txtDt3.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 4)
                        {
                            btnExcluirParcela4.Visible = true;
                            btnAddParcela4.Visible = false;
                            txtDt4.Enabled = false;
                            txtDt4.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 5)
                        {
                            btnExcluirParcela5.Visible = true;
                            btnAddParcela5.Visible = false;
                            txtDt5.Enabled = false;
                            txtDt5.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 6)
                        {
                            btnExcluirParcela6.Visible = true;
                            btnAddParcela6.Visible = false;
                            txtDt6.Enabled = false;
                            txtDt6.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 7)
                        {
                            btnExcluirParcela7.Visible = true;
                            btnAddParcela7.Visible = false;
                            txtDt7.Enabled = false;
                            txtDt7.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 8)
                        {
                            btnExcluirParcela8.Visible = true;
                            btnAddParcela8.Visible = false;
                            txtDt8.Enabled = false;
                            txtDt8.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 9)
                        {
                            btnExcluirParcela9.Visible = true;
                            btnAddParcela9.Visible = false;
                            txtDt9.Enabled = false;
                            txtDt9.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }
                        else if (p.CodigoParcela == 10)
                        {
                            btnExcluirParcela10.Visible = true;
                            btnAddParcela10.Visible = false;
                            txtDt10.Enabled = false;
                            txtDt10.Text = p.DataVencimento.ToString("dd/MM/yyyy");
                        }

                    }
                }
                if (ddlPagamento.SelectedValue != "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO ....." && ddlPagamento.SelectedValue != "")
                {
                    CondPagamento condPag = new CondPagamento();
                    CondPagamentoDAL condPagDAL = new CondPagamentoDAL();

                    condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
                    if (condPag.CodigoTipoPagamento == 24 || condPag.CodigoTipoPagamento == 25)
                    {
                        pnlAddTodos.Visible = true;
                        pnlRemoverTodos.Visible = true;

                        txtDt1.Enabled = false;
                        txtDt2.Enabled = false;
                        txtDt3.Enabled = false;
                        txtDt4.Enabled = false;
                        txtDt5.Enabled = false;
                        txtDt6.Enabled = false;
                        txtDt7.Enabled = false;
                        txtDt8.Enabled = false;
                        txtDt9.Enabled = false;
                        txtDt10.Enabled = false;
                    }
                    else
                    {
                        pnlRemoverTodos.Visible = false;
                        pnlAddTodos.Visible = false;

                        if (btnAddParcela.Visible == true)
                            txtDt1.Enabled = true;
                        if (btnAddParcela2.Visible == true)
                            txtDt2.Enabled = true;
                        if (btnAddParcela3.Visible == true)
                            txtDt3.Enabled = true;
                        if (btnAddParcela4.Visible == true)
                            txtDt4.Enabled = true;
                        if (btnAddParcela5.Visible == true)
                            txtDt5.Enabled = true;
                        if (btnAddParcela6.Visible == true)
                            txtDt6.Enabled = true;
                        if (btnAddParcela7.Visible == true)
                            txtDt7.Enabled = true;
                        if (btnAddParcela8.Visible == true)
                            txtDt8.Enabled = true;
                        if (btnAddParcela9.Visible == true)
                            txtDt9.Enabled = true;
                        if (btnAddParcela10.Visible == true)
                            txtDt10.Enabled = true;
                    }
                    GerarParcelaExistente(sender, e);
                }
            }
            if (ListaParcelaDocumento.Count == 0)
                pnlParcelas.Visible = false;
        }

        protected void PreencheDados(object sender, EventArgs e, int CodPessoa, int CodUsuario)
        {
            Doc_OrdemServico doc = (Doc_OrdemServico)Session["Doc_OrdemServico"];

            if (doc.CodigoDocumento == 0)
            {
                txtCodigo.Text = "Novo";
                btnExcluir.Visible = false;
                ddlClassificacao.Enabled = false;
                ddlClassificacao.Enabled = true;
            }
            else
            {
                txtCodigo.Text = Convert.ToString(doc.CodigoDocumento);
                btnExcluir.Visible = true;
                ddlClassificacao.Enabled = true;
                ddlClassificacao.Enabled = false;
            }

            if (Convert.ToString(doc.DataHoraEmissao) != "01/01/0001 00:00:00")
                txtdtemissao.Text = doc.DataHoraEmissao.ToString();

            ddlClassificacao.SelectedValue = doc.CodigoClassificacao.ToString();

            Habil_TipoDAL sd = new Habil_TipoDAL();
            if (ddlClassificacao.SelectedValue == "98")
                ddlSituacao.DataSource = sd.SituacaoOrdemFatura();
            else
                ddlSituacao.DataSource = sd.SituacaoOrdemServico();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            ddlEmpresa.SelectedValue = Convert.ToString(doc.CodigoEmpresa);
            ddlSituacao.SelectedValue = Convert.ToString(doc.CodigoSituacao);
            txtNroDocumento.Text = Convert.ToString(doc.NumeroDocumento);
            txtOBS.Text = doc.ObservacaoDocumento;
            txtNroSerie.Text = Convert.ToString(doc.DGSRDocumento);
            txtEmail.Text = doc.Cpl_MailSolicitante;
            txtTelefone.Text = doc.Cpl_FoneSolicitante;
            ddlAcao.SelectedValue = doc.Cpl_Acao.ToString();
            if(doc.CodigoSolicAtendimento != 0)
                txtCodSolAtendimento.Text = doc.CodigoSolicAtendimento.ToString();

            if (doc.NumeroDocumento == 0)
                txtNroDocumento.Text = "";

            if (CodPessoa == 0)
            {
                if (doc.Cpl_CodigoPessoa == 0)
                    txtCodPessoa.Text = "";
                else
                    txtCodPessoa.Text = Convert.ToString(doc.Cpl_CodigoPessoa);
            }
            if (CodUsuario == 0)
            {
                if (doc.CodigoUsuarioResponsavel == 0)
                    txtCodUsu.Text = "";
                else
                    txtCodUsu.Text = Convert.ToString(doc.CodigoUsuarioResponsavel);
            }

            if (doc.CodigoCondicaoPagamento == 0)
                ddlPagamento.SelectedValue = "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....";
            else
                ddlPagamento.SelectedValue = doc.CodigoCondicaoPagamento.ToString();

            if (doc.CodigoTipoCobranca == 0)
                ddlTipoCobranca.SelectedValue = "..... SELECIONE UM TIPO DE COBRANÇA.....";
            else
                ddlTipoCobranca.SelectedValue = doc.CodigoTipoCobranca.ToString();

            if (Session["RascunhoDocumento"] != null)
            {
                string str = Server.HtmlDecode(Session["RascunhoDocumento"].ToString());
                litDescricao.Text = str;
                Session["RascunhoDocumento"] = null;
            }
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            Doc_OrdemServicoDAL doc = new Doc_OrdemServicoDAL();
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
            Response.Redirect("~/Pages/Servicos/ConOrdServico.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            Doc_OrdemServico p = new Doc_OrdemServico();
            Doc_OrdemServicoDAL pe = new Doc_OrdemServicoDAL();

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            p.Cpl_CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
            p.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
            p.DGSRDocumento = txtNroSerie.Text;
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.CodigoClassificacao = Convert.ToInt32(ddlClassificacao.SelectedValue);
            p.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
            p.Cpl_FoneSolicitante = txtTelefone.Text;
            p.Cpl_MailSolicitante = txtEmail.Text;
            p.CodigoContato = Convert.ToInt32(ddlContato.SelectedValue);
            p.ObservacaoDocumento = txtOBS.Text;
            p.CodigoUsuarioResponsavel = Convert.ToInt32(txtCodUsu.Text);
            p.ValorTotal = Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
            p.DescricaoDocumento = litDescricao.Text;
            p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
            p.Cpl_Usuario = Convert.ToInt32(Session["CodPflUsuario"]);

            if (ddlClassificacao.SelectedValue == "98")
            {
                p.CodigoCondicaoPagamento = Convert.ToInt32(ddlPagamento.SelectedValue);
                p.CodigoTipoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);

                foreach (Doc_OrdemServico os in ListaOSs)
                {
                    os.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
                    os.Cpl_Usuario = Convert.ToInt32(Session["CodPflUsuario"]);
                }
            }
            
            if (txtCodSolAtendimento.Text != "")
                p.CodigoSolicAtendimento = Convert.ToDecimal(txtCodSolAtendimento.Text);
           
            
            if (txtCodigo.Text == "Novo")
            {
                ddlEmpresa_TextChanged(sender, e);
                p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Session["CodigoGeradorSequencial"]);
                p.Cpl_NomeTabela = Session["NomeTabela"].ToString();

                Session["CodigoGeradorSequencial"] = null;
                pe.Inserir(p, EventoDocumento(), ListaAnexo,ListaTipoServico,ListaParcelaDocumento,ListaItemTipoServico,ListaItemDocumento , ListaOSs);

            }
            else
            {
                Doc_OrdemServico p2 = new Doc_OrdemServico();
                p.CodigoDocumento = Convert.ToInt64(txtCodigo.Text);

                p2 = pe.PesquisarDocumento(Convert.ToDecimal(txtCodigo.Text));
                if (Convert.ToInt32(ddlSituacao.SelectedValue) != p2.CodigoSituacao)
                    pe.Atualizar(p, EventoDocumento(), ListaAnexo, ListaTipoServico, ListaParcelaDocumento, ListaItemTipoServico, ListaItemDocumento, ListaOSs);
                else
                    pe.Atualizar(p, null, ListaAnexo, ListaTipoServico, ListaParcelaDocumento, ListaItemTipoServico, ListaItemDocumento, ListaOSs);
                
            }

            if (Convert.ToInt32(ddlAcao.SelectedValue) == 0)
            {
                Session["MensagemTela"] = null;
                if (txtCodigo.Text == "Novo")
                    Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
                else
                    Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
                Session["ListaTipoServico"] = null;
                Session["ListaItemTipoServico"] = null;
                Session["ListaItemDocumento"] = null;
                Session["ListaOSs"] = null;
                Session["ListaParcelaDocumento"] = null;
                Session["NomeTabela"] = null;
                btnVoltar_Click(sender, e);
            }

            if (Convert.ToInt32(ddlAcao.SelectedValue) == 1)
            {
                p = pe.PesquisarDocumento(Convert.ToDecimal(p.CodigoDocumento));
                CarregaTiposSituacoes();
                ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();

                EventoDocumentoDAL eve = new EventoDocumentoDAL();
                ListaEvento = eve.ObterEventos(Convert.ToDecimal(p.CodigoDocumento));
                Session["Eventos"] = ListaEvento;
                GrdEventoDocumento.DataSource = ListaEvento;
                GrdEventoDocumento.DataBind();

                Habil_LogDAL log = new Habil_LogDAL();
                ListaLog = log.ListarLogs(Convert.ToDouble(p.CodigoDocumento), 100);
                Session["Logs"] = ListaLog;
                grdLogDocumento.DataSource = ListaLog;
                grdLogDocumento.DataBind();

                txtCodigo.Text = Convert.ToString(p.CodigoDocumento);
                txtCodPessoa.Text = Convert.ToString(p.Cpl_CodigoPessoa);
                SelectedPessoa(sender, e);
                ddlContato.SelectedValue = p.CodigoContato.ToString();

                if (p.CodigoCondicaoPagamento != 0)
                    ddlPagamento.SelectedValue = p.CodigoCondicaoPagamento.ToString();

                if (p.CodigoTipoCobranca != 0)
                    ddlTipoCobranca.SelectedValue = p.CodigoTipoCobranca.ToString();

                ddlEmpresa.Enabled = false;
                pnlParcelas.Visible = true;

                Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
                grdPagamento.DataSource = ListaParcelaDocumento;
                grdPagamento.DataBind();
                GerarParcelaExistente(sender, e);

                if(p.CodigoClassificacao == 98)
                    btnFaturar.Visible = true;
                else
                    btnFaturar.Visible = false;
                btnExcluir.Visible = true;

                Session["MensagemTela"] = null;
                if (txtCodigo.Text == "Novo")
                    ShowMessage("Registro Incluído com Sucesso!!! Continue Editando...", MessageType.Info);
                else
                    ShowMessage("Registro Alterado com Sucesso!!! Continue Editando...", MessageType.Info);

                btnNovoAnexo.Visible = true;
                TabAnexos = "display:block";
                TabEventos = "display:block";
                if(ListaLog.Count == 0)
                    TabLogs = "display:none";
                else
                    TabLogs = "display:block";

                ddlClassificacao.Enabled = false;
            }

            if (Convert.ToInt32(ddlAcao.SelectedValue) == 2)
            {
                Session["MensagemTela"] = null;

                if (txtCodigo.Text == "Novo")
                    ShowMessage("Registro Incluído com Sucesso!!! Continue incluindo...", MessageType.Info);
                else
                    ShowMessage("Registro Alterado com Sucesso!!! Continue incluindo...", MessageType.Info);

                txtCodigo.Text = "Novo";

                GrdEventoDocumento.DataSource = null;
                GrdEventoDocumento.DataBind();
                Session["Eventos"] = null;

                grdLogDocumento.DataSource = null;
                grdLogDocumento.DataBind();
                Session["Logs"] = null;
              
                grdItemDocumento.DataSource = null;
                grdItemDocumento.DataBind();
                Session["ItemDocumento"] = null;

                grdAnexo.DataSource = null;
                grdAnexo.DataBind();
                Session["NovoAnexo"] = null;

                ddlEmpresa.Enabled = true;
                ddlEmpresa_TextChanged(sender, e);

                if (p.CodigoClassificacao == 98)
                    btnFaturar.Visible = true;
                else
                    btnFaturar.Visible = false;
                btnExcluir.Visible = false;

                SelectedPessoa(sender, e);
                ddlContato.SelectedValue = p.CodigoContato.ToString();
                txtEmail.Text = p.Cpl_MailSolicitante;
                txtTelefone.Text = p.Cpl_FoneSolicitante;

                btnNovoAnexo.Visible = false;
                TabAnexos = "display:none";
                TabEventos = "display:none";
                TabLogs = "display:none";

                ddlClassificacao.Enabled = true;

            }
            Session["NomeTabela"] = null;
        }

        protected void CompactaDocumento()
        {
            Doc_OrdemServico doc = new Doc_OrdemServico();

            if (txtCodigo.Text == "Novo")
                doc.CodigoDocumento = 0;
            else
                doc.CodigoDocumento = Convert.ToDecimal(txtCodigo.Text);

            if (txtdtemissao.Text != "")
                doc.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);

            doc.CodigoSituacao = Convert.ToInt32(ddlSituacao.Text);
            doc.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.Text);
            doc.CodigoClassificacao = Convert.ToInt32(ddlClassificacao.Text);
            doc.DGSRDocumento = txtNroSerie.Text;
            doc.ObservacaoDocumento = txtOBS.Text;
            doc.Cpl_MailSolicitante = txtEmail.Text;
            doc.Cpl_FoneSolicitante = txtTelefone.Text;
            doc.Cpl_Acao = Convert.ToInt32(ddlAcao.SelectedValue);
            if (txtCodSolAtendimento.Text != "")
                doc.CodigoSolicAtendimento = Convert.ToDecimal(txtCodSolAtendimento.Text);

            decimal numero;
            if (decimal.TryParse(txtNroDocumento.Text, out numero))
            {
                doc.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
            }
            else
            {
                doc.NumeroDocumento = 0;
            }
                       
            //ckeditor
            string str = litDescricao.Text;
            string str2 = Server.HtmlDecode(str);
            Session["RascunhoDocumento"] = str2;
            doc.DescricaoDocumento = str2;      

            if (ddlContato.SelectedValue != "..... SELECIONE UM CONTATO ....." && ddlContato.SelectedValue != "")
                doc.CodigoContato = Convert.ToInt32(ddlContato.SelectedValue);

            if (txtCodPessoa.Text == "")
                doc.Cpl_CodigoPessoa = 0;
            else
                doc.Cpl_CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);

            if (txtCodUsu.Text == "")
                doc.CodigoUsuarioResponsavel = 0;
            else
                doc.CodigoUsuarioResponsavel = Convert.ToInt32(txtCodUsu.Text);

            if (ddlPagamento.SelectedValue == "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....")
                doc.CodigoCondicaoPagamento = 0;
            else
                doc.CodigoCondicaoPagamento = Convert.ToInt32(ddlPagamento.SelectedValue);

            if (ddlTipoCobranca.SelectedValue == "..... SELECIONE UM TIPO DE COBRANÇA.....")
                doc.CodigoTipoCobranca = 0;
            else
                doc.CodigoTipoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);

            Session["Doc_OrdemServico"] = doc;
            Session["IncMovAcessoContato"] = null;
            Session["IncMovAcessoPessoa"] = null;
            Session["ListaTipoServico"] = ListaTipoServico;
            Session["ListaItemTipoServico"] = ListaItemTipoServico;
            Session["ListaItemDocumento"] = ListaItemDocumento;
        }

        protected void ConPessoa(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=10");
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
                v.CampoValido("Codigo Pessoa", txtCodPessoa.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

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
            Doc_OrdemServicoDAL docDAL = new Doc_OrdemServicoDAL();
            if (txtCodigo.Text != "Novo")
            {
                Doc_OrdemServico doc = new Doc_OrdemServico();

                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));
                if (doc.Cpl_CodigoPessoa != Convert.ToInt32(txtCodPessoa.Text))
                {
                    ShowMessage("Você alterou o Pessoa do Documento!", MessageType.Info);
                    Session["ListaItemDocumento"] = null;
                    grdItemDocumento.DataSource = null;
                    grdItemDocumento.DataBind();
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
                txtCNPJCPFCredor.Text = "";
                txtCidade.Text = "";
                txtBairro.Text = "";
                txtRazSocial.Text = "";
                txtEndereco.Text = "";
                txtEstado.Text = "";
                txtCEP.Text = "";
                txtEmail1.Text = "";
                txtCodPessoa.Focus();
                Session["ListaOSs"] = null;
                return;
            }
            txtRazSocial.Text = p2.NomePessoa;
            txtPessoa.Text = p2.NomePessoa;

            foreach (Pessoa_Inscricao inscricao in ins2)
            {
                if (inscricao._CodigoItem == 1)
                {
                    txtCNPJCPFCredor.Text = Convert.ToString(inscricao._NumeroInscricao);
                }
            }
            Pessoa_Contato ctt = new Pessoa_Contato();
            PessoaContatoDAL cttDAL = new PessoaContatoDAL();
            ctt = cttDAL.PesquisarPessoaContato(Convert.ToInt64(txtCodPessoa.Text), 1);
            txtEmail1.Text = ctt._Mail1;

            PessoaEnderecoDAL end = new PessoaEnderecoDAL();
            List<Pessoa_Endereco> end2 = new List<Pessoa_Endereco>();
            end2 = end.ObterPessoaEnderecos(codigoPessoa);

            foreach (Pessoa_Endereco endereco in end2)
            {
                if (endereco._CodigoInscricao == 1)
                {
                    txtEndereco.Text = Convert.ToString(endereco._Logradouro + ", " + endereco._NumeroLogradouro + " - " + endereco._Complemento);

                    txtCEP.Text = Convert.ToString(endereco._CodigoCEP);
                    txtEstado.Text = endereco._DescricaoEstado.Substring(0, 2);
                    txtCidade.Text = endereco._DescricaoMunicipio;
                    txtBairro.Text = endereco._DescricaoBairro;
                }
            }
            if (txtCodigo.Text != "Novo")
            {
                ddlPagamento.SelectedValue = p2.CodigoCondPagamento.ToString();
                ddlPagamento_TextChanged(sender, e);
            }

            ddlContato.DataSource = cttDAL.ObterPessoaContatos(codigoPessoa);
            ddlContato.DataTextField = "_NomeContatoCombo";
            ddlContato.DataValueField = "_CodigoItem";
            ddlContato.DataBind();
            ddlContato.Items.Insert(0, "..... SELECIONE UM CONTATO .....");

            

            if(ddlClassificacao.SelectedValue == "98")
            {
                if (p2.CodigoTipoServico != 0)
                {
                    if (ddlTipoServico.Items.FindByValue(p2.CodigoTipoServico.ToString()) != null)
                        ddlTipoServico.SelectedValue = p2.CodigoTipoServico.ToString();
                    int i = 0;
                    foreach (TipoServico tiposerv in ListaTipoServico)
                    {
                        if (ddlTipoServico.SelectedValue == tiposerv.CodigoTipoServico.ToString())
                        {
                            i++;
                        }
                    }
                    if (i == 0 && ddlTipoServico.SelectedValue != "..... SELECIONE UM TIPO DE SERVIÇO.....")
                        BtnAddServico_Click(sender, e);
                }
                if (ddlPagamento.Items.FindByValue(p2.CodigoCondPagamento.ToString()) != null)
                    ddlPagamento.SelectedValue = p2.CodigoCondPagamento.ToString();


                if (txtCodigo.Text != "" && txtCodigo.Text != "Novo")
                    ListaOSs = docDAL.ListarOrdemServicoServico(codigoPessoa, 97,Convert.ToDecimal(txtCodigo.Text));
                else
                    ListaOSs = docDAL.ListarOrdemServicoServico(codigoPessoa, 97, 0);

                List<Doc_OrdemServico> ListaNova = new List<Doc_OrdemServico>();
                if (Session["ListaItemDocumento"] != null)
                {
                    ListaItemDocumento = (List<ItemDocumento>)Session["ListaItemDocumento"];

                    foreach (Doc_OrdemServico doc in ListaOSs)
                    {
                        foreach (ItemDocumento item in ListaItemDocumento)
                        {
                            if (item.CodigoDocumento == doc.CodigoDocumento)
                            {

                                doc.BtnAdd = false;
                                doc.BtnRemove = true;
                            }
                        }
                        if(txtCodigo.Text != "" && txtCodigo.Text != "Novo")
                        {
                            if (doc.CodigoDocumento == Convert.ToDecimal(txtCodigo.Text))
                            {
                                doc.BtnAdd = false;
                                doc.BtnRemove = true;
                            }
                        }                        
                        ListaNova.Add(doc);
                    }
                    grdOSs.DataSource = ListaNova;
                    grdOSs.DataBind();
                    Session["ListaOSs"] = ListaNova;
                }
                else
                {
                    grdOSs.DataSource = ListaOSs;
                    grdOSs.DataBind();
                    Session["ListaOSs"] = ListaOSs;
                }
                
            }           
            Session["TabFocada"] = null;
            
        }

        protected void ddlEmpresa_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "Novo")
                return;

            DBTabelaDAL db = new DBTabelaDAL();
            List<GeracaoSequencialDocumento> ListaGerDoc = new List<GeracaoSequencialDocumento>();
            GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
            GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
            ListaGerDoc = gerDocDAL.ListarGeracaoSequencial("CD_TIPO_DOCUMENTO", "INT", "2", "");

            // Se existe a tabela sequencial
            if (ListaGerDoc.Count == 0)
            {
                Session["MensagemTela"] = "Gerador Sequencial não iniciado";
                btnVoltar_Click(sender, e);
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
                    if (ger.Validade < DateTime.Now)
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
            Response.Redirect("~/Pages/financeiros/ManAnexo.aspx?cad=6");
        }

        protected void btnNovoAnexo_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=6");
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
            Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx?cad=2");

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

            Response.Redirect("~/Pages/Servicos/ManRasDocumento.aspx?cad=2");

        }

        protected void txtCodUsu_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodUsu.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Usuário", txtCodUsu.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodUsu.Text = "";
                    return;
                }
            }
            Int64 numero;
            if (!Int64.TryParse(txtCodUsu.Text, out numero))
            {
                return;
            }

            if (txtCodigo.Text != "Novo")
            {
                Doc_OrdemServico doc = new Doc_OrdemServico();
                Doc_OrdemServicoDAL docDAL = new Doc_OrdemServicoDAL();
                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));
                if (doc.CodigoUsuarioResponsavel != Convert.ToInt32(txtCodUsu.Text))
                {
                    ShowMessage("Você alterou o usuário do Documento!", MessageType.Info);
                }
            }

            Int64 codigoUsuario = Convert.ToInt64(txtCodUsu.Text);
            UsuarioDAL Usuario = new UsuarioDAL();
            Usuario p2 = new Usuario();

            p2 = Usuario.PesquisarUsuario(codigoUsuario);

            if (p2 == null)
            {
                ShowMessage("Usuário não existente!", MessageType.Info);
                txtCodUsu.Text = "";
                txtUsu.Text = "";
                txtCodUsu.Focus();

                return;
            }


            txtUsu.Text = p2.NomeUsuario;
            //txtDescricao.Focus();
            Session["TabFocada"] = null;
        }

        protected void btnUsu_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Response.Redirect("~/Pages/Usuarios/ConUsuario.aspx?Cad=1");
        }

        protected void btnSolAtendimento_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Response.Redirect("~/Pages/Servicos/ConSolAtendimento.aspx?cad=1");
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            pnlParcela1.Visible = false;
            txtDt1.Text = "";
            pnlParcela2.Visible = false;
            txtDt2.Text = "";
            pnlParcela3.Visible = false;
            txtDt3.Text = "";
            pnlParcela4.Visible = false;
            txtDt4.Text = "";
            pnlParcela5.Visible = false;
            txtDt5.Text = "";
            pnlParcela6.Visible = false;
            txtDt6.Text = "";
            pnlParcela7.Visible = false;
            txtDt7.Text = "";
            pnlParcela8.Visible = false;
            txtDt8.Text = "";
            pnlParcela9.Visible = false;
            txtDt9.Text = "";
            pnlParcela10.Visible = false;
            txtDt10.Text = "";
            grdPagamento.DataSource = null;
            grdPagamento.DataBind();

            Session["ListaParcelaDocumento"] = null;
            pnlParcelas.Visible = false;
            btnExcluirParcela.Visible = false; btnAddParcela.Visible = true; txtDt1.Enabled = true; txtDt1.Text = "";
            btnExcluirParcela2.Visible = false; btnAddParcela2.Visible = true; txtDt2.Enabled = true; txtDt2.Text = "";
            btnExcluirParcela3.Visible = false; btnAddParcela3.Visible = true; txtDt3.Enabled = true; txtDt3.Text = "";
            btnExcluirParcela4.Visible = false; btnAddParcela4.Visible = true; txtDt4.Enabled = true; txtDt4.Text = "";
            btnExcluirParcela5.Visible = false; btnAddParcela5.Visible = true; txtDt5.Enabled = true; txtDt5.Text = "";
            btnExcluirParcela6.Visible = false; btnAddParcela6.Visible = true; txtDt6.Enabled = true; txtDt6.Text = "";
            btnExcluirParcela7.Visible = false; btnAddParcela7.Visible = true; txtDt7.Enabled = true; txtDt7.Text = "";
            btnExcluirParcela8.Visible = false; btnAddParcela8.Visible = true; txtDt8.Enabled = true; txtDt8.Text = "";
            btnExcluirParcela9.Visible = false; btnAddParcela9.Visible = true; txtDt9.Enabled = true; txtDt9.Text = "";
            btnExcluirParcela10.Visible = false; btnAddParcela10.Visible = true; txtDt10.Enabled = true; txtDt10.Text = "";
            btnAddTodas.Visible = false;
            btnRemoverTodas.Visible = false;

            ddlPagamento.SelectedValue = "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....";
        }

        protected void btnGerarParcela_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtVlrTotal.Text.Substring(2)) <= 0)
                return;
            if (ddlPagamento.SelectedValue == "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....")
            {
                pnlParcela1.Visible = false;
                txtDt1.Text = "";
                pnlParcela2.Visible = false;
                txtDt2.Text = "";
                pnlParcela3.Visible = false;
                txtDt3.Text = "";
                pnlParcela4.Visible = false;
                txtDt4.Text = "";
                pnlParcela5.Visible = false;
                txtDt5.Text = "";
                pnlParcela6.Visible = false;
                txtDt6.Text = "";
                pnlParcela7.Visible = false;
                txtDt7.Text = "";
                pnlParcela8.Visible = false;
                txtDt8.Text = "";
                pnlParcela9.Visible = false;
                txtDt9.Text = "";
                pnlParcela10.Visible = false;
                txtDt10.Text = "";
                grdPagamento.DataSource = null;
                grdPagamento.DataBind();
                ShowMessage("Selecione uma condição de pagamento", MessageType.Info);
                return;
            }

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));

            if (ListaParcelaDocumento.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
            else
            {
                pnlParcelas.Visible = true;
            }

            Session["ListaParcelaDocumento"] = null;
            grdPagamento.DataSource = null;
            grdPagamento.DataBind();
            if (grdPagamento.Rows.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
            else
            {
                pnlParcelas.Visible = true;
            }
            pnlParcela1.Visible = false; btnExcluirParcela.Visible = false; btnAddParcela.Visible = true; txtDt1.Enabled = true;
            pnlParcela2.Visible = false; btnExcluirParcela2.Visible = false; btnAddParcela2.Visible = false; txtDt2.Enabled = true;
            pnlParcela3.Visible = false; btnExcluirParcela3.Visible = false; btnAddParcela3.Visible = false; txtDt3.Enabled = true;
            pnlParcela4.Visible = false; btnExcluirParcela4.Visible = false; btnAddParcela4.Visible = false; txtDt4.Enabled = true;
            pnlParcela5.Visible = false; btnExcluirParcela5.Visible = false; btnAddParcela5.Visible = false; txtDt5.Enabled = true;
            pnlParcela6.Visible = false; btnExcluirParcela6.Visible = false; btnAddParcela6.Visible = false; txtDt6.Enabled = true;
            pnlParcela7.Visible = false; btnExcluirParcela7.Visible = false; btnAddParcela7.Visible = false; txtDt7.Enabled = true;
            pnlParcela8.Visible = false; btnExcluirParcela8.Visible = false; btnAddParcela8.Visible = false; txtDt8.Enabled = true;
            pnlParcela9.Visible = false; btnExcluirParcela9.Visible = false; btnAddParcela9.Visible = false; txtDt9.Enabled = true;
            pnlParcela10.Visible = false; btnExcluirParcela10.Visible = false; btnAddParcela10.Visible = false; txtDt10.Enabled = true;

            decimal ValorTotal = Convert.ToDecimal((txtVlrTotal.Text).Substring(2));
            decimal Total = 0;
            decimal Resto = 0;
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime DataAtual = RnTab.ObterDataHoraServidor();
            switch (condPag.CodigoTipoPagamento)
            {
                case 23://A vista
                    pnlAddTodos.Visible = false;
                    pnlRemoverTodos.Visible = false;
                    pnlParcela1.Visible = true;

                    txtNro.Text = txtNroDocumento.Text + "/" + 1;
                    txtValor.Text = ValorTotal.ToString("N2");

                    txtDt1.Text = DataAtual.ToString("dd/MM/yyyy");
                    txtDt1.Enabled = false;

                    break;
                case 24://A Prazo
                    pnlAddTodos.Visible = true;
                    pnlRemoverTodos.Visible = true;
                    switch (condPag.QtdeParcelas)
                    {
                        case 1:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal, 2)).ToString("N2");
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            break;
                        case 2:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 3:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = DataAtual.AddDays(90).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 4:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = DataAtual.AddDays(90).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = DataAtual.AddDays(120).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 5:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = DataAtual.AddDays(90).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = DataAtual.AddDays(120).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = DataAtual.AddDays(150).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 6:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = DataAtual.AddDays(90).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = DataAtual.AddDays(120).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = DataAtual.AddDays(150).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = DataAtual.AddDays(180).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 7:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = DataAtual.AddDays(90).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = DataAtual.AddDays(120).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = DataAtual.AddDays(150).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = DataAtual.AddDays(180).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;
                            txtDt7.Text = DataAtual.AddDays(210).ToString("dd/MM/yyyy");
                            txtDt7.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        case 8:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = DataAtual.AddDays(90).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = DataAtual.AddDays(120).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = DataAtual.AddDays(150).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = DataAtual.AddDays(180).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;
                            txtDt7.Text = DataAtual.AddDays(210).ToString("dd/MM/yyyy");
                            txtDt7.Enabled = false;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;
                            txtDt8.Text = DataAtual.AddDays(240).ToString("dd/MM/yyyy");
                            txtDt8.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        case 9:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = DataAtual.AddDays(90).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = DataAtual.AddDays(120).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = DataAtual.AddDays(150).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = DataAtual.AddDays(180).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;
                            txtDt7.Text = DataAtual.AddDays(210).ToString("dd/MM/yyyy");
                            txtDt7.Enabled = false;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;
                            txtDt8.Text = DataAtual.AddDays(240).ToString("dd/MM/yyyy");
                            txtDt8.Enabled = false;

                            pnlParcela9.Visible = true;
                            txtValor9.Text = (Math.Round(ValorTotal * (condPag.Proporcao9 / 100), 2)).ToString();
                            txtNro9.Text = txtNroDocumento.Text + "/" + 9;
                            txtDt9.Text = DataAtual.AddDays(270).ToString("dd/MM/yyyy");
                            txtDt9.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text) +
                                     Convert.ToDecimal(txtValor9.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        default:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Text = DataAtual.AddDays(30).ToString("dd/MM/yyyy");
                            txtDt1.Enabled = false;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = DataAtual.AddDays(60).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = DataAtual.AddDays(90).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = DataAtual.AddDays(120).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = DataAtual.AddDays(150).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = DataAtual.AddDays(180).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;
                            txtDt7.Text = DataAtual.AddDays(210).ToString("dd/MM/yyyy");
                            txtDt7.Enabled = false;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;
                            txtDt8.Text = DataAtual.AddDays(240).ToString("dd/MM/yyyy");
                            txtDt8.Enabled = false;

                            pnlParcela9.Visible = true;
                            txtValor9.Text = (Math.Round(ValorTotal * (condPag.Proporcao9 / 100), 2)).ToString();
                            txtNro9.Text = txtNroDocumento.Text + "/" + 9;
                            txtDt9.Text = DataAtual.AddDays(270).ToString("dd/MM/yyyy");
                            txtDt9.Enabled = false;

                            pnlParcela10.Visible = true;
                            txtValor10.Text = (Math.Round(ValorTotal * (condPag.Proporcao10 / 100), 2)).ToString();
                            txtNro10.Text = txtNroDocumento.Text + "/" + 10;
                            txtDt10.Text = DataAtual.AddDays(300).ToString("dd/MM/yyyy");
                            txtDt10.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text) +
                                     Convert.ToDecimal(txtValor9.Text) +
                                     Convert.ToDecimal(txtValor10.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                    }//fecha switch parcelas

                    break;
                case 25://Dia Fixo
                    pnlAddTodos.Visible = true;
                    pnlRemoverTodos.Visible = true;

                    string[] data = DataAtual.ToString("dd/MM/yyyy").Split('/');
                    string StrDataVencimento = condPag.DiaFixo + "/" + data[1] + "/" + data[2];
                    DateTime DataVencimento = Convert.ToDateTime(StrDataVencimento);
                    DateTime DataAtual2 = Convert.ToDateTime(DataAtual.ToString("dd/MM/yyy"));
                    DateTime parcela1;
                    switch (condPag.QtdeParcelas)
                    {
                        case 1:
                            pnlParcela1.Visible = true;
                            txtValor.Text = ValorTotal.ToString("N2");
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;
                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            break;
                        case 2:


                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;

                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }



                            break;
                        case 3:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;

                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = parcela1.AddMonths(2).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }


                            break;
                        case 4:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;
                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = parcela1.AddMonths(2).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = parcela1.AddMonths(3).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 5:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;
                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = parcela1.AddMonths(2).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = parcela1.AddMonths(3).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = parcela1.AddMonths(4).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 6:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;
                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = parcela1.AddMonths(2).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = parcela1.AddMonths(3).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = parcela1.AddMonths(4).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = parcela1.AddMonths(5).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 7:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;
                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = parcela1.AddMonths(2).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = parcela1.AddMonths(3).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = parcela1.AddMonths(4).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = parcela1.AddMonths(5).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;
                            txtDt7.Text = parcela1.AddMonths(6).ToString("dd/MM/yyyy");
                            txtDt7.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 8:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;
                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = parcela1.AddMonths(2).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = parcela1.AddMonths(3).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = parcela1.AddMonths(4).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = parcela1.AddMonths(5).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;
                            txtDt7.Text = parcela1.AddMonths(6).ToString("dd/MM/yyyy");
                            txtDt7.Enabled = false;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;
                            txtDt8.Text = parcela1.AddMonths(7).ToString("dd/MM/yyyy");
                            txtDt8.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 9:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;
                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = parcela1.AddMonths(2).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = parcela1.AddMonths(3).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = parcela1.AddMonths(4).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = parcela1.AddMonths(5).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;
                            txtDt7.Text = parcela1.AddMonths(6).ToString("dd/MM/yyyy");
                            txtDt7.Enabled = false;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;
                            txtDt8.Text = parcela1.AddMonths(7).ToString("dd/MM/yyyy");
                            txtDt8.Enabled = false;

                            pnlParcela9.Visible = true;
                            txtValor9.Text = (Math.Round(ValorTotal * (condPag.Proporcao9 / 100), 2)).ToString();
                            txtNro9.Text = txtNroDocumento.Text + "/" + 9;
                            txtDt9.Text = parcela1.AddMonths(8).ToString("dd/MM/yyyy");
                            txtDt9.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text) +
                                     Convert.ToDecimal(txtValor9.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        default:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;
                            txtDt1.Enabled = false;
                            if (DataVencimento < DataAtual2)
                            {
                                txtDt1.Text = DataVencimento.AddMonths(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtDt1.Text = DataVencimento.ToString("dd/MM/yyyy");
                            }
                            parcela1 = Convert.ToDateTime(txtDt1.Text);

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;
                            txtDt2.Text = parcela1.AddMonths(1).ToString("dd/MM/yyyy");
                            txtDt2.Enabled = false;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;
                            txtDt3.Text = parcela1.AddMonths(2).ToString("dd/MM/yyyy");
                            txtDt3.Enabled = false;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;
                            txtDt4.Text = parcela1.AddMonths(3).ToString("dd/MM/yyyy");
                            txtDt4.Enabled = false;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;
                            txtDt5.Text = parcela1.AddMonths(4).ToString("dd/MM/yyyy");
                            txtDt5.Enabled = false;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;
                            txtDt6.Text = parcela1.AddMonths(5).ToString("dd/MM/yyyy");
                            txtDt6.Enabled = false;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;
                            txtDt7.Text = parcela1.AddMonths(6).ToString("dd/MM/yyyy");
                            txtDt7.Enabled = false;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;
                            txtDt8.Text = parcela1.AddMonths(7).ToString("dd/MM/yyyy");
                            txtDt8.Enabled = false;

                            pnlParcela9.Visible = true;
                            txtValor9.Text = (Math.Round(ValorTotal * (condPag.Proporcao9 / 100), 2)).ToString();
                            txtNro9.Text = txtNroDocumento.Text + "/" + 9;
                            txtDt9.Text = parcela1.AddMonths(8).ToString("dd/MM/yyyy");
                            txtDt9.Enabled = false;

                            pnlParcela10.Visible = true;
                            txtValor10.Text = (Math.Round(ValorTotal * (condPag.Proporcao10 / 100), 2)).ToString();
                            txtNro10.Text = txtNroDocumento.Text + "/" + 10;
                            txtDt10.Text = parcela1.AddMonths(9).ToString("dd/MM/yyyy");
                            txtDt10.Enabled = false;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text) +
                                     Convert.ToDecimal(txtValor9.Text) +
                                     Convert.ToDecimal(txtValor10.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                    }//fecha switch parcelas

                    break;
                case 26: //Data informada
                    pnlAddTodos.Visible = false;
                    pnlRemoverTodos.Visible = false;
                    txtDt1.Text = "";
                    txtDt2.Text = "";
                    txtDt3.Text = "";
                    txtDt4.Text = "";
                    txtDt5.Text = "";
                    txtDt6.Text = "";
                    txtDt7.Text = "";
                    txtDt8.Text = "";
                    txtDt9.Text = "";
                    txtDt10.Text = "";
                    switch (condPag.QtdeParcelas)
                    {
                        case 1:
                            pnlParcela1.Visible = true;
                            txtValor.Text = ValorTotal.ToString("N2");
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;


                            break;
                        case 2:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 3:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 4:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 5:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 6:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 7:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        case 8:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        case 9:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;

                            pnlParcela9.Visible = true;
                            txtValor9.Text = (Math.Round(ValorTotal * (condPag.Proporcao9 / 100), 2)).ToString();
                            txtNro9.Text = txtNroDocumento.Text + "/" + 9;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text) +
                                     Convert.ToDecimal(txtValor9.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        default:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;

                            pnlParcela9.Visible = true;
                            txtValor9.Text = (Math.Round(ValorTotal * (condPag.Proporcao9 / 100), 2)).ToString();
                            txtNro9.Text = txtNroDocumento.Text + "/" + 9;

                            pnlParcela10.Visible = true;
                            txtValor10.Text = (Math.Round(ValorTotal * (condPag.Proporcao10 / 100), 2)).ToString();
                            txtNro10.Text = txtNroDocumento.Text + "/" + 10;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text) +
                                     Convert.ToDecimal(txtValor9.Text) +
                                     Convert.ToDecimal(txtValor10.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                    }//fecha switch parcelas
                    break;
                case 27://Parcelado
                    pnlAddTodos.Visible = false;
                    pnlRemoverTodos.Visible = false;
                    txtDt1.Text = "";
                    txtDt2.Text = "";
                    txtDt3.Text = "";
                    txtDt4.Text = "";
                    txtDt5.Text = "";
                    txtDt6.Text = "";
                    txtDt7.Text = "";
                    txtDt8.Text = "";
                    txtDt9.Text = "";
                    txtDt10.Text = "";
                    switch (condPag.QtdeParcelas)
                    {
                        case 1:
                            pnlParcela1.Visible = true;
                            txtValor.Text = ValorTotal.ToString("N2");
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            break;
                        case 2:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 3:

                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 4:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 5:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 6:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }

                            break;
                        case 7:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        case 8:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        case 9:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;

                            pnlParcela9.Visible = true;
                            txtValor9.Text = (Math.Round(ValorTotal * (condPag.Proporcao9 / 100), 2)).ToString();
                            txtNro9.Text = txtNroDocumento.Text + "/" + 9;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text) +
                                     Convert.ToDecimal(txtValor9.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                        default:
                            pnlParcela1.Visible = true;
                            txtValor.Text = (Math.Round(ValorTotal * (condPag.Proporcao1 / 100), 2)).ToString();
                            txtNro.Text = txtNroDocumento.Text + "/" + 1;

                            pnlParcela2.Visible = true;
                            txtValor2.Text = (Math.Round(ValorTotal * (condPag.Proporcao2 / 100), 2)).ToString();
                            txtNro2.Text = txtNroDocumento.Text + "/" + 2;

                            pnlParcela3.Visible = true;
                            txtValor3.Text = (Math.Round(ValorTotal * (condPag.Proporcao3 / 100), 2)).ToString();
                            txtNro3.Text = txtNroDocumento.Text + "/" + 3;

                            pnlParcela4.Visible = true;
                            txtValor4.Text = (Math.Round(ValorTotal * (condPag.Proporcao4 / 100), 2)).ToString();
                            txtNro4.Text = txtNroDocumento.Text + "/" + 4;

                            pnlParcela5.Visible = true;
                            txtValor5.Text = (Math.Round(ValorTotal * (condPag.Proporcao5 / 100), 2)).ToString();
                            txtNro5.Text = txtNroDocumento.Text + "/" + 5;

                            pnlParcela6.Visible = true;
                            txtValor6.Text = (Math.Round(ValorTotal * (condPag.Proporcao6 / 100), 2)).ToString();
                            txtNro6.Text = txtNroDocumento.Text + "/" + 6;

                            pnlParcela7.Visible = true;
                            txtValor7.Text = (Math.Round(ValorTotal * (condPag.Proporcao7 / 100), 2)).ToString();
                            txtNro7.Text = txtNroDocumento.Text + "/" + 7;

                            pnlParcela8.Visible = true;
                            txtValor8.Text = (Math.Round(ValorTotal * (condPag.Proporcao8 / 100), 2)).ToString();
                            txtNro8.Text = txtNroDocumento.Text + "/" + 8;

                            pnlParcela9.Visible = true;
                            txtValor9.Text = (Math.Round(ValorTotal * (condPag.Proporcao9 / 100), 2)).ToString();
                            txtNro9.Text = txtNroDocumento.Text + "/" + 9;

                            pnlParcela10.Visible = true;
                            txtValor10.Text = (Math.Round(ValorTotal * (condPag.Proporcao10 / 100), 2)).ToString();
                            txtNro10.Text = txtNroDocumento.Text + "/" + 10;

                            Total = (Convert.ToDecimal(txtValor.Text) +
                                     Convert.ToDecimal(txtValor2.Text) +
                                     Convert.ToDecimal(txtValor3.Text) +
                                     Convert.ToDecimal(txtValor4.Text) +
                                     Convert.ToDecimal(txtValor5.Text) +
                                     Convert.ToDecimal(txtValor6.Text) +
                                     Convert.ToDecimal(txtValor7.Text) +
                                     Convert.ToDecimal(txtValor8.Text) +
                                     Convert.ToDecimal(txtValor9.Text) +
                                     Convert.ToDecimal(txtValor10.Text));

                            if (Total != Convert.ToDecimal(txtVlrTotal.Text.Substring(2)))
                            {
                                Resto = Total - Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
                                txtValor.Text = (Convert.ToDecimal(txtValor.Text) - Resto).ToString("N2");
                            }
                            break;
                    }//fecha switch parcelas

                    break;
                default:
                    break;

            }



        }

        protected void btnAddParcela_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt1.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 1", MessageType.Info);
                return;
            }


            ParcelaDocumento Item = new ParcelaDocumento(1,
                                                        Convert.ToDateTime(txtDt1.Text),
                                                        Convert.ToDecimal(txtValor.Text),
                                                        0,
                                                        txtNro.Text);


            ListaParcelaDocumento.Add(Item);
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;
            btnAddParcela.Visible = false;
            btnExcluirParcela.Visible = true;
            txtDt1.Enabled = false;

            if (btnExcluirParcela2.Visible == false)
                btnAddParcela2.Visible = true;
        }

        protected void btnExcluirParcela_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 1)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela.Visible = true;
            btnExcluirParcela.Visible = false;

            btnExcluirParcela2_Click(sender, e);
            btnExcluirParcela3_Click(sender, e);
            btnExcluirParcela4_Click(sender, e);
            btnExcluirParcela5_Click(sender, e);
            btnExcluirParcela6_Click(sender, e);
            btnExcluirParcela7_Click(sender, e);
            btnExcluirParcela8_Click(sender, e);
            btnExcluirParcela9_Click(sender, e);
            btnExcluirParcela10_Click(sender, e);

            btnAddParcela2.Visible = false;
            btnAddParcela3.Visible = false;
            btnAddParcela4.Visible = false;
            btnAddParcela5.Visible = false;
            btnAddParcela6.Visible = false;
            btnAddParcela7.Visible = false;
            btnAddParcela8.Visible = false;
            btnAddParcela9.Visible = false;
            btnAddParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt1.Enabled = true;
            else
                txtDt1.Enabled = false;

            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela2_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt2.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 2", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(2,
                                                        Convert.ToDateTime(txtDt2.Text),
                                                        Convert.ToDecimal(txtValor2.Text),
                                                        0,
                                                        txtNro2.Text);


            ListaParcelaDocumento.Add(Item);
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;
            btnAddParcela2.Visible = false;
            btnExcluirParcela2.Visible = true;
            txtDt2.Enabled = false;

            if (btnExcluirParcela3.Visible == false)
                btnAddParcela3.Visible = true;
        }

        protected void btnExcluirParcela2_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 2)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela2.Visible = true;
            btnExcluirParcela2.Visible = false;

            btnExcluirParcela3_Click(sender, e);
            btnExcluirParcela4_Click(sender, e);
            btnExcluirParcela5_Click(sender, e);
            btnExcluirParcela6_Click(sender, e);
            btnExcluirParcela7_Click(sender, e);
            btnExcluirParcela8_Click(sender, e);
            btnExcluirParcela9_Click(sender, e);
            btnExcluirParcela10_Click(sender, e);

            btnAddParcela3.Visible = false;
            btnAddParcela4.Visible = false;
            btnAddParcela5.Visible = false;
            btnAddParcela6.Visible = false;
            btnAddParcela7.Visible = false;
            btnAddParcela8.Visible = false;
            btnAddParcela9.Visible = false;
            btnAddParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt2.Enabled = true;
            else
                txtDt2.Enabled = false;

            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela3_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt3.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 3", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(3,
                                                        Convert.ToDateTime(txtDt3.Text),
                                                        Convert.ToDecimal(txtValor3.Text),
                                                        0,
                                                        txtNro3.Text);


            ListaParcelaDocumento.Add(Item);
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;
            btnAddParcela3.Visible = false;
            btnExcluirParcela3.Visible = true;
            txtDt3.Enabled = false;
            if (btnExcluirParcela4.Visible == false)
                btnAddParcela4.Visible = true;
        }

        protected void btnExcluirParcela3_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 3)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela3.Visible = true;
            btnExcluirParcela3.Visible = false;

            btnExcluirParcela4_Click(sender, e);
            btnExcluirParcela5_Click(sender, e);
            btnExcluirParcela6_Click(sender, e);
            btnExcluirParcela7_Click(sender, e);
            btnExcluirParcela8_Click(sender, e);
            btnExcluirParcela9_Click(sender, e);
            btnExcluirParcela10_Click(sender, e);

            btnAddParcela4.Visible = false;
            btnAddParcela5.Visible = false;
            btnAddParcela6.Visible = false;
            btnAddParcela7.Visible = false;
            btnAddParcela8.Visible = false;
            btnAddParcela9.Visible = false;
            btnAddParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt3.Enabled = true;
            else
                txtDt3.Enabled = false;
            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela4_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt4.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 4", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(4,
                                                        Convert.ToDateTime(txtDt4.Text),
                                                        Convert.ToDecimal(txtValor4.Text),
                                                        0,
                                                        txtNro4.Text);


            ListaParcelaDocumento.Add(Item);
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;
            btnAddParcela4.Visible = false;
            btnExcluirParcela4.Visible = true;
            txtDt4.Enabled = false;
            if (btnExcluirParcela5.Visible == false)
                btnAddParcela5.Visible = true;
        }

        protected void btnExcluirParcela4_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 4)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela4.Visible = true;
            btnExcluirParcela4.Visible = false;

            btnExcluirParcela5_Click(sender, e);
            btnExcluirParcela6_Click(sender, e);
            btnExcluirParcela7_Click(sender, e);
            btnExcluirParcela8_Click(sender, e);
            btnExcluirParcela9_Click(sender, e);
            btnExcluirParcela10_Click(sender, e);

            btnAddParcela5.Visible = false;
            btnAddParcela6.Visible = false;
            btnAddParcela7.Visible = false;
            btnAddParcela8.Visible = false;
            btnAddParcela9.Visible = false;
            btnAddParcela10.Visible = false;


            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt4.Enabled = true;
            else
                txtDt4.Enabled = false;
            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela5_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt5.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 5", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(5,
                                                        Convert.ToDateTime(txtDt5.Text),
                                                        Convert.ToDecimal(txtValor5.Text),
                                                        0,
                                                        txtNro5.Text);


            ListaParcelaDocumento.Add(Item);

            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;
            btnAddParcela5.Visible = false;
            btnExcluirParcela5.Visible = true;
            txtDt5.Enabled = false;
            if (btnExcluirParcela6.Visible == false)
                btnAddParcela6.Visible = true;
        }

        protected void btnExcluirParcela5_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 5)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela5.Visible = true;
            btnExcluirParcela5.Visible = false;

            btnExcluirParcela6_Click(sender, e);
            btnExcluirParcela7_Click(sender, e);
            btnExcluirParcela8_Click(sender, e);
            btnExcluirParcela9_Click(sender, e);
            btnExcluirParcela10_Click(sender, e);

            btnAddParcela6.Visible = false;
            btnAddParcela7.Visible = false;
            btnAddParcela8.Visible = false;
            btnAddParcela9.Visible = false;
            btnAddParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt5.Enabled = true;
            else
                txtDt5.Enabled = false;
            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela6_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt6.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 6", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(6,
                                                        Convert.ToDateTime(txtDt6.Text),
                                                        Convert.ToDecimal(txtValor6.Text),
                                                        0,
                                                        txtNro6.Text);


            ListaParcelaDocumento.Add(Item);
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;
            btnAddParcela6.Visible = false;
            btnExcluirParcela6.Visible = true;
            txtDt6.Enabled = false;
            if (btnExcluirParcela7.Visible == false)
                btnAddParcela7.Visible = true;
        }

        protected void btnExcluirParcela6_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 6)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela6.Visible = true;
            btnExcluirParcela6.Visible = false;

            btnExcluirParcela7_Click(sender, e);
            btnExcluirParcela8_Click(sender, e);
            btnExcluirParcela9_Click(sender, e);
            btnExcluirParcela10_Click(sender, e);

            btnAddParcela7.Visible = false;
            btnAddParcela8.Visible = false;
            btnAddParcela9.Visible = false;
            btnAddParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt6.Enabled = true;
            else
                txtDt6.Enabled = false;

            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela7_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt7.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 7", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(7,
                                                        Convert.ToDateTime(txtDt7.Text),
                                                        Convert.ToDecimal(txtValor7.Text),
                                                        0,
                                                        txtNro7.Text);


            ListaParcelaDocumento.Add(Item);
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;

            btnAddParcela7.Visible = false;
            btnExcluirParcela7.Visible = true;
            txtDt7.Enabled = false;
            if (btnExcluirParcela8.Visible == false)
                btnAddParcela8.Visible = true;
        }

        protected void btnExcluirParcela7_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 7)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela7.Visible = true;
            btnExcluirParcela7.Visible = false;

            btnExcluirParcela8_Click(sender, e);
            btnExcluirParcela9_Click(sender, e);
            btnExcluirParcela10_Click(sender, e);

            btnAddParcela8.Visible = false;
            btnAddParcela9.Visible = false;
            btnAddParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt7.Enabled = true;
            else
                txtDt7.Enabled = false;
            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela8_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt8.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 8", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(8,
                                                        Convert.ToDateTime(txtDt8.Text),
                                                        Convert.ToDecimal(txtValor8.Text),
                                                        0,
                                                        txtNro8.Text);


            ListaParcelaDocumento.Add(Item);
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;

            btnAddParcela8.Visible = false;
            btnExcluirParcela8.Visible = true;
            txtDt8.Enabled = false;
            if (btnExcluirParcela9.Visible == false)
                btnAddParcela9.Visible = true;
        }

        protected void btnExcluirParcela8_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 8)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela8.Visible = true;
            btnExcluirParcela8.Visible = false;

            btnExcluirParcela9_Click(sender, e);
            btnExcluirParcela10_Click(sender, e);
            btnAddParcela9.Visible = false;
            btnAddParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt8.Enabled = true;
            else
                txtDt8.Enabled = false;
            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela9_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt9.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 9", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(9,
                                                        Convert.ToDateTime(txtDt9.Text),
                                                        Convert.ToDecimal(txtValor9.Text),
                                                        0,
                                                        txtNro9.Text);


            ListaParcelaDocumento.Add(Item);
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();
            pnlParcelas.Visible = true;

            btnAddParcela9.Visible = false;
            btnExcluirParcela9.Visible = true;
            txtDt9.Enabled = false;
            if (btnExcluirParcela10.Visible == false)
                btnAddParcela10.Visible = true;
        }

        protected void btnExcluirParcela9_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 9)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela9.Visible = true;
            btnExcluirParcela9.Visible = false;

            btnExcluirParcela10_Click(sender, e);
            btnAddParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt9.Enabled = true;
            else
                txtDt9.Enabled = false;
            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void btnAddParcela10_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = true;

            v.CampoValido("Data de Validade", txtDt10.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                blnCampoValido = false;
                ShowMessage("Digite a data de validade da Parcela 10", MessageType.Info);
                return;
            }

            ParcelaDocumento Item = new ParcelaDocumento(10,
                                                        Convert.ToDateTime(txtDt10.Text),
                                                        Convert.ToDecimal(txtValor10.Text),
                                                        0,
                                                        txtNro10.Text);


            ListaParcelaDocumento.Add(Item);

            Session["ListaParcelaDocumento"] = ListaParcelaDocumento;
            ListaParcelaDocumento = ListaParcelaDocumento.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcelaDocumento;
            grdPagamento.DataBind();

            pnlParcelas.Visible = true;
            btnAddParcela10.Visible = false;
            btnExcluirParcela10.Visible = true;
            txtDt10.Enabled = false;
        }

        protected void btnExcluirParcela10_Click(object sender, EventArgs e)
        {
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            List<ParcelaDocumento> ListaParcela = new List<ParcelaDocumento>();

            if (Session["ListaParcelaDocumento"] != null)
                ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            else
                ListaParcelaDocumento = new List<ParcelaDocumento>();

            ParcelaDocumento tabi;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela != 10)
                {
                    tabi = new ParcelaDocumento();
                    tabi.CodigoParcela = item.CodigoParcela;
                    tabi.CodigoDocumento = item.CodigoDocumento;
                    tabi.DGNumeroDocumento = item.DGNumeroDocumento;
                    tabi.DataVencimento = item.DataVencimento;
                    tabi.ValorParcela = item.ValorParcela;
                    ListaParcela.Add(tabi);
                }
            }
            ListaParcela = ListaParcela.OrderBy(c => c.CodigoParcela).ToList();
            grdPagamento.DataSource = ListaParcela;
            grdPagamento.DataBind();
            Session["ListaParcelaDocumento"] = ListaParcela;

            btnAddParcela10.Visible = true;
            btnExcluirParcela10.Visible = false;

            CondPagamento condPag = new CondPagamento();
            CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
            condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
            if (condPag.CodigoTipoPagamento == 26 || condPag.CodigoTipoPagamento == 27)
                txtDt10.Enabled = true;
            else
                txtDt10.Enabled = false;
            if (ListaParcela.Count == 0)
            {
                pnlParcelas.Visible = false;
            }
        }

        protected void ddlPagamento_TextChanged(object sender, EventArgs e)
        {
            if (ddlPagamento.SelectedValue != "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....")
            {
                CondPagamento condPag = new CondPagamento();
                CondPagamentoDAL condPagDAL = new CondPagamentoDAL();
                condPag = condPagDAL.PesquisarCondPagamento(Convert.ToInt32(ddlPagamento.SelectedValue));
                ddlTipoCobranca.SelectedValue = condPag.CodigoTipoCobranca.ToString();
                if (pnlParcela1.Visible == true)
                {
                    //btnLimpar_Click(sender, e);
                    btnGerarParcela_Click(sender, e);
                }
            }
        }

        protected void btnAddTodas_Click(object sender, EventArgs e)
        {
            if (btnAddParcela.Visible == true)
            {
                btnAddParcela_Click(sender, e);
            }
            if (btnAddParcela2.Visible == true)
            {
                btnAddParcela2_Click(sender, e);
            }
            if (btnAddParcela3.Visible == true)
            {
                btnAddParcela3_Click(sender, e);
            }
            if (btnAddParcela4.Visible == true)
            {
                btnAddParcela4_Click(sender, e);
            }
            if (btnAddParcela5.Visible == true)
            {
                btnAddParcela5_Click(sender, e);
            }
            if (btnAddParcela6.Visible == true)
            {
                btnAddParcela6_Click(sender, e);
            }
            if (btnAddParcela7.Visible == true)
            {
                btnAddParcela7_Click(sender, e);
            }
            if (btnAddParcela8.Visible == true)
            {
                btnAddParcela8_Click(sender, e);
            }
            if (btnAddParcela9.Visible == true)
            {
                btnAddParcela9_Click(sender, e);
            }
            if (btnAddParcela10.Visible == true)
            {
                btnAddParcela10_Click(sender, e);
            }

        }

        protected void btnRemoverTodas_Click(object sender, EventArgs e)
        {
            if (btnExcluirParcela.Visible == true)
            {
                btnExcluirParcela_Click(sender, e);
            }
            if (btnExcluirParcela2.Visible == true)
            {
                btnExcluirParcela2_Click(sender, e);
                btnAddParcela2.Visible = false;
            }
            if (btnExcluirParcela3.Visible == true)
            {
                btnExcluirParcela3_Click(sender, e);
                btnAddParcela3.Visible = false;
            }
            if (btnExcluirParcela4.Visible == true)
            {
                btnExcluirParcela4_Click(sender, e);
                btnAddParcela4.Visible = false;
            }
            if (btnExcluirParcela5.Visible == true)
            {
                btnExcluirParcela5_Click(sender, e);
                btnAddParcela5.Visible = false;
            }
            if (btnExcluirParcela6.Visible == true)
            {
                btnExcluirParcela6_Click(sender, e);
                btnAddParcela6.Visible = false;
            }
            if (btnExcluirParcela7.Visible == true)
            {
                btnExcluirParcela7_Click(sender, e);
                btnAddParcela7.Visible = false;
            }
            if (btnExcluirParcela8.Visible == true)
            {
                btnExcluirParcela8_Click(sender, e);
                btnAddParcela8.Visible = false;
            }
            if (btnExcluirParcela9.Visible == true)
            {
                btnExcluirParcela9_Click(sender, e);
                btnAddParcela9.Visible = false;
            }
            if (btnExcluirParcela10.Visible == true)
            {
                btnExcluirParcela10_Click(sender, e);
                btnAddParcela10.Visible = false;
            }
        }

        protected void GerarParcelaExistente(object sender, EventArgs e)
        {
            //btnGerarParcela_Click(sender, e);

            ListaParcelaDocumento = (List<ParcelaDocumento>)Session["ListaParcelaDocumento"];
            pnlParcelas.Visible = true;
            foreach (ParcelaDocumento item in ListaParcelaDocumento)
            {
                if (item.CodigoParcela == 1)
                {
                    pnlParcela1.Visible = true;
                    txtNro.Text = txtNroDocumento.Text + "/1";
                    txtDt1.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela.Visible = false;
                    btnExcluirParcela.Visible = true;
                }
                else if (item.CodigoParcela == 2)
                {
                    pnlParcela2.Visible = true;
                    txtNro2.Text = txtNroDocumento.Text + "/2";
                    txtDt2.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor2.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela2.Visible = false;
                    btnExcluirParcela2.Visible = true;
                }
                else if (item.CodigoParcela == 3)
                {
                    pnlParcela3.Visible = true;
                    txtNro3.Text = txtNroDocumento.Text + "/3";
                    txtDt3.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor3.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela3.Visible = false;
                    btnExcluirParcela3.Visible = true;
                }
                else if (item.CodigoParcela == 4)
                {
                    pnlParcela4.Visible = true;
                    txtNro4.Text = txtNroDocumento.Text + "/4";
                    txtDt4.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor4.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela4.Visible = false;
                    btnExcluirParcela4.Visible = true;
                }
                else if (item.CodigoParcela == 5)
                {
                    pnlParcela5.Visible = true;
                    txtNro5.Text = txtNroDocumento.Text + "/5";
                    txtDt5.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor5.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela5.Visible = false;
                    btnExcluirParcela5.Visible = true;
                }
                else if (item.CodigoParcela == 6)
                {
                    pnlParcela6.Visible = true;
                    txtNro6.Text = txtNroDocumento.Text + "/6";
                    txtDt6.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor6.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela6.Visible = false;
                    btnExcluirParcela6.Visible = true;
                }
                else if (item.CodigoParcela == 7)
                {
                    pnlParcela7.Visible = true;
                    txtNro7.Text = txtNroDocumento.Text + "/7";
                    txtDt7.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor7.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela7.Visible = false;
                    btnExcluirParcela7.Visible = true;
                }
                else if (item.CodigoParcela == 8)
                {
                    pnlParcela8.Visible = true;
                    txtNro8.Text = txtNroDocumento.Text + "/8";
                    txtDt8.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor8.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela8.Visible = false;
                    btnExcluirParcela8.Visible = true;
                }
                else if (item.CodigoParcela == 9)
                {
                    pnlParcela9.Visible = true;
                    txtNro9.Text = txtNroDocumento.Text + "/9";
                    txtDt9.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor9.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela9.Visible = false;
                    btnExcluirParcela9.Visible = true;
                }
                else if (item.CodigoParcela == 10)
                {
                    pnlParcela10.Visible = true;
                    txtNro10.Text = txtNroDocumento.Text + "/10";
                    txtDt10.Text = item.DataVencimento.ToString("dd/MM/yyyy");
                    txtValor10.Text = item.ValorParcela.ToString("N2");
                    btnAddParcela10.Visible = false;
                    btnExcluirParcela10.Visible = true;
                }

            }
        }

        protected void grdPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void BtnAddServico_Click(object sender, EventArgs e)
        {

            if (ddlTipoServico.SelectedValue == "..... SELECIONE UM TIPO DE SERVIÇO.....")
            {
                ShowMessage("Selecione um Tipo de Serviço", MessageType.Info);
                return;
            }

            foreach (TipoServico item in ListaTipoServico)
            {
                if (ddlTipoServico.SelectedValue == item.CodigoTipoServico.ToString())
                {
                    ShowMessage("Serviço já adicionado", MessageType.Info);
                    return;
                }
            }
            int intEndItem = 0;
            int intItem = 0;


            TipoServico Servico = new TipoServico();
            TipoServicoDAL ServicoDAL = new TipoServicoDAL();
            Servico = ServicoDAL.PesquisarTipoServico(Convert.ToInt32(ddlTipoServico.SelectedValue));


            if (ListaTipoServico.Count != 0)
                intEndItem = Convert.ToInt32(ListaTipoServico.Max(x => x.CodigoServico).ToString());

            intEndItem = intEndItem + 1;

            TipoServico servico2 = new TipoServico(intEndItem, Servico.CodigoTipoServico, Servico.DescricaoTipoServico, Servico.CodigoSituacao, Servico.ValorISSQN, Servico.CodigoCNAE, Servico.CodigoServicoLei);


            if (intEndItem != 0)
                ListaTipoServico.RemoveAll(x => x.CodigoServico == intEndItem);

            ListaTipoServico.Add(servico2);
            Session["ListaTipoServico"] = ListaTipoServico;
            grdServicos.DataSource = ListaTipoServico;
            grdServicos.DataBind();


            ddlTipoServicoAdicionado.DataSource = ListaTipoServico;
            ddlTipoServicoAdicionado.DataTextField = "DescricaoTipoServico";
            ddlTipoServicoAdicionado.DataValueField = "CodigoServico";
            ddlTipoServicoAdicionado.DataBind();
            ddlTipoServicoAdicionado.Items.Insert(0, "..... SELECIONE UM SERVIÇO.....");


            ItemTipoServicoDAL itemServicoDAL = new ItemTipoServicoDAL();
            List<ItemTipoServico> ListaItens = new List<ItemTipoServico>();
            ListaItens = itemServicoDAL.ObterItemTipoServico(Convert.ToInt32(ddlTipoServico.SelectedValue));

            if (ListaItemTipoServico.Count != 0)
                intItem = Convert.ToInt32(ListaItemTipoServico.Max(x => x.CodigoProdutoDocumento).ToString());


            foreach (ItemTipoServico item in ListaItens)
            {
                intItem = intItem + 1;
                ItemTipoServico ItemTipoServico = new ItemTipoServico(item.CodigoItemTipoServico,
                                                                        intItem, item.Quantidade,
                                                                        item.PrecoItem,
                                                                        item.Cpl_DscProduto,
                                                                        item.CodigoProduto,
                                                                        intEndItem,
                                                                        item.CodigoTipoServico);
                ListaItemTipoServico.Add(ItemTipoServico);
            }

            MontarValorTotal(ListaItemTipoServico);
            Session["ListaItemTipoServico"] = ListaItemTipoServico;
            grdProduto.DataSource = ListaItemTipoServico;
            grdProduto.DataBind();
            Session["ManutencaoProduto"] = null;
           // ddlTipoServicoAdicionado.SelectedValue = "..... SELECIONE UM SERVIÇO.....";
            CamposProduto();
            LimparTelaItens();
        }

        protected void grdServicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<TipoServico> ListaTipoServico = new List<TipoServico>();
            List<TipoServico> ListaTipo = new List<TipoServico>();
            List<ItemTipoServico> ListaItem = new List<ItemTipoServico>();

            if (Session["ListaTipoServico"] != null)
                ListaTipoServico = (List<TipoServico>)Session["ListaTipoServico"];
            else
                ListaTipoServico = new List<TipoServico>();

            List<ItemTipoServico> ListaItens2 = new List<ItemTipoServico>();

            foreach (TipoServico item in ListaTipoServico)
            {
                TipoServico tabi = new TipoServico();

                if (item.CodigoServico != Convert.ToInt32(HttpUtility.HtmlDecode(grdServicos.SelectedRow.Cells[0].Text)))
                {
                    tabi.CodigoServico = item.CodigoServico;
                    tabi.CodigoTipoServico = item.CodigoTipoServico;
                    tabi.DescricaoTipoServico = item.DescricaoTipoServico;
                    tabi.ValorISSQN = item.ValorISSQN;
                    tabi.CodigoCNAE = item.CodigoCNAE;
                    tabi.CodigoServicoLei = item.CodigoServicoLei;
                    ListaTipo.Add(tabi);



                    foreach (ItemTipoServico item2 in ListaItemTipoServico)
                    {
                        if (item2.CodigoServico == item.CodigoServico)
                            ListaItens2.Add(item2);
                    }


                }


            }
            ddlTipoServicoAdicionado.DataSource = ListaTipo;
            ddlTipoServicoAdicionado.DataTextField = "DescricaoTipoServico";
            ddlTipoServicoAdicionado.DataValueField = "CodigoServico";
            ddlTipoServicoAdicionado.DataBind();
            ddlTipoServicoAdicionado.Items.Insert(0, "..... SELECIONE UM SERVIÇO.....");

            MontarValorTotal(ListaItens2);
            Session["ListaItemTipoServico"] = ListaItens2;
            grdProduto.DataSource = ListaItens2;
            grdProduto.DataBind();

            grdServicos.DataSource = ListaTipo;
            grdServicos.DataBind();
            Session["ListaTipoServico"] = ListaTipo;

            Session["ManutencaoProduto"] = null;
            ddlTipoServicoAdicionado.SelectedValue = "..... SELECIONE UM SERVIÇO.....";

            CamposProduto();
            LimparTelaItens();

        }

        protected void BtnExcluirProduto_Click(object sender, EventArgs e)
        {
            List<ItemTipoServico> ListaItemTipoServico = new List<ItemTipoServico>();
            List<ItemTipoServico> ListaItemTipo = new List<ItemTipoServico>();

            if (Session["ListaItemTipoServico"] != null)
                ListaItemTipoServico = (List<ItemTipoServico>)Session["ListaItemTipoServico"];
            else
                ListaItemTipoServico = new List<ItemTipoServico>();

            ItemTipoServico tabi;
            foreach (ItemTipoServico item in ListaItemTipoServico)
            {
                if (item.CodigoProdutoDocumento != Convert.ToInt32(HttpUtility.HtmlDecode(grdProduto.SelectedRow.Cells[0].Text)))
                {
                    tabi = new ItemTipoServico();
                    tabi.CodigoItemTipoServico = item.CodigoItemTipoServico;
                    tabi.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    tabi.CodigoServico = item.CodigoServico;
                    tabi.Cpl_DscProduto = item.Cpl_DscProduto;
                    tabi.CodigoProduto = item.CodigoProduto;
                    tabi.Quantidade = item.Quantidade;
                    tabi.PrecoItem = item.PrecoItem;
                    ListaItemTipo.Add(tabi);
                }
            }
            BtnExcluirProduto.Visible = false;
            ddlProduto.Enabled = true;
            ddlTipoServicoAdicionado.Enabled = true;
            LimparTelaItens();
            ddlTipoServicoAdicionado.SelectedValue = "..... SELECIONE UM SERVIÇO.....";
            MontarValorTotal(ListaItemTipo);
            grdProduto.DataSource = ListaItemTipo;
            grdProduto.DataBind();
            Session["ListaItemTipoServico"] = ListaItemTipo;
            Session["ManutencaoProduto"] = null;
        }

        protected void LimparTelaItens()
        {
            txtPreco.Text = "0,00";
            txtQtde.Text = "0,00";
            ProdutoDAL ed = new ProdutoDAL();
            ddlProduto.DataSource = ed.ObterTiposServicos();
            ddlProduto.DataTextField = "DescricaoProduto";
            ddlProduto.DataValueField = "CodigoProduto";
            ddlProduto.DataBind();
            ddlProduto.Items.Insert(0, "..... SELECIONE UM PRODUTO.....");
        }

        protected void grdProduto_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (ItemTipoServico item in ListaItemTipoServico)
            {
                if (item.CodigoProdutoDocumento == Convert.ToInt32(HttpUtility.HtmlDecode(grdProduto.SelectedRow.Cells[0].Text))
                                                && item.CodigoServico == Convert.ToInt32(HttpUtility.HtmlDecode(grdProduto.SelectedRow.Cells[1].Text)))
                {
                    Session["ManutencaoProduto"] = item;
                    ddlProduto.SelectedValue = item.CodigoProduto.ToString();
                    ddlTipoServicoAdicionado.SelectedValue = item.CodigoServico.ToString();
                    txtQtde.Text = item.Quantidade.ToString();
                    txtPreco.Text = item.PrecoItem.ToString();
                    BtnExcluirProduto.Visible = true;
                    ddlProduto.Enabled = false;
                    ddlTipoServicoAdicionado.Enabled = false;
                }
            }
        }

        protected void CamposProduto()
        {
            if (Session["ManutencaoProduto"] != null)
            {
                BtnExcluirProduto.Visible = true;
                ddlTipoServicoAdicionado.Enabled = false;
                ddlProduto.Enabled = false;
            }
            else
            {
                BtnExcluirProduto.Visible = false;
                ddlTipoServicoAdicionado.Enabled = true;
                ddlProduto.Enabled = true;

            }
        }

        protected void BtnAddProduto_Click(object sender, EventArgs e)
        {
            if (Session["ManutencaoProduto"] != null)
            {
                ItemTipoServico produto = (ItemTipoServico)Session["ManutencaoProduto"];
                Session["ManutencaoProduto"] = null;
                CamposProduto();

                List<ItemTipoServico> ListaItemTipoServico = new List<ItemTipoServico>();
                List<ItemTipoServico> ListaItemTipo = new List<ItemTipoServico>();

                if (Session["ListaItemTipoServico"] != null)
                    ListaItemTipoServico = (List<ItemTipoServico>)Session["ListaItemTipoServico"];
                else
                    ListaItemTipoServico = new List<ItemTipoServico>();

                ItemTipoServico tabi;
                foreach (ItemTipoServico item in ListaItemTipoServico)
                {
                    if (item.CodigoProdutoDocumento != produto.CodigoProdutoDocumento)
                    {
                        tabi = new ItemTipoServico();
                        tabi.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        tabi.CodigoItemTipoServico = item.CodigoItemTipoServico;
                        tabi.CodigoServico = item.CodigoServico;
                        tabi.Cpl_DscProduto = item.Cpl_DscProduto;
                        tabi.CodigoProduto = item.CodigoProduto;
                        tabi.Quantidade = item.Quantidade;
                        tabi.PrecoItem = item.PrecoItem;
                        ListaItemTipo.Add(tabi);
                    }
                    else
                    {
                        if (item.CodigoServico != produto.CodigoServico)
                        {
                            tabi = new ItemTipoServico();
                            tabi.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                            tabi.CodigoItemTipoServico = item.CodigoItemTipoServico;
                            tabi.CodigoServico = item.CodigoServico;
                            tabi.Cpl_DscProduto = item.Cpl_DscProduto;
                            tabi.CodigoProduto = item.CodigoProduto;
                            tabi.Quantidade = item.Quantidade;
                            tabi.PrecoItem = item.PrecoItem;
                            ListaItemTipo.Add(tabi);
                        }
                        else
                        {
                            tabi = new ItemTipoServico();
                            tabi.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                            tabi.CodigoItemTipoServico = item.CodigoItemTipoServico;
                            tabi.CodigoServico = item.CodigoServico;
                            tabi.Cpl_DscProduto = item.Cpl_DscProduto;
                            tabi.CodigoProduto = item.CodigoProduto;
                            tabi.Quantidade = Convert.ToDecimal(txtQtde.Text);
                            tabi.PrecoItem = Convert.ToDecimal(txtPreco.Text);
                            ListaItemTipo.Add(tabi);
                        }

                    }
                }

                MontarValorTotal(ListaItemTipo);
                LimparTelaItens();
                ddlTipoServicoAdicionado.SelectedValue = "..... SELECIONE UM SERVIÇO.....";
                grdProduto.DataSource = ListaItemTipo;
                grdProduto.DataBind();
                Session["ListaItemTipoServico"] = ListaItemTipo;


            }
            else
            {
                if (grdServicos.Rows.Count == 0)
                {
                    ShowMessage("Adicione um Serviço!", MessageType.Info);
                    return;
                }
                if (ddlTipoServicoAdicionado.SelectedValue == "..... SELECIONE UM SERVIÇO.....")
                {
                    ShowMessage("Selecione o Tipo de Serviço para adicionar o produto.", MessageType.Info);
                    return;
                }

                int intEndItem = 0;
                ItemTipoServicoDAL r = new ItemTipoServicoDAL();
                List<DBTabelaCampos> lista = new List<DBTabelaCampos>();


                if (ValidaCamposItemTipoServico() == false)
                    return;


                if (ListaItemTipoServico.Count != 0)
                    intEndItem = Convert.ToInt32(ListaItemTipoServico.Max(x => x.CodigoProdutoDocumento).ToString());

                intEndItem = intEndItem + 1;

                ItemTipoServico ListaItem = new ItemTipoServico();
                ListaItem.CodigoProdutoDocumento = intEndItem;
                ListaItem.CodigoServico = Convert.ToInt32(ddlTipoServicoAdicionado.SelectedValue);
                ListaItem.CodigoProduto = Convert.ToInt32(ddlProduto.SelectedItem.Value);
                ListaItem.Cpl_DscProduto = ddlProduto.SelectedItem.Text;
                ListaItem.Quantidade = Convert.ToDecimal(txtQtde.Text);
                ListaItem.PrecoItem = Convert.ToDecimal(txtPreco.Text);

                if (intEndItem != 0)
                    ListaItemTipoServico.RemoveAll(x => x.CodigoProdutoDocumento == intEndItem);

                ListaItemTipoServico.Add(ListaItem);

                Session["ListaItemTipoServico"] = ListaItemTipoServico;
                grdProduto.DataSource = ListaItemTipoServico;
                grdProduto.DataBind();

                MontarValorTotal(ListaItemTipoServico);
                LimparTelaItens();
                ddlTipoServicoAdicionado.SelectedValue = "..... SELECIONE UM SERVIÇO.....";
            }



        }

        protected Boolean ValidaCamposItemTipoServico()
        {

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }


            return true;
        }

        protected void MontarValorTotal(List<ItemTipoServico> ListaItens)
        {

            decimal ValorTotal = 0;
            txtVlrTotal.Text = Convert.ToDouble("0,00").ToString("C");

            foreach (ItemTipoServico itens in ListaItens)
            {
                ValorTotal += (itens.Quantidade * itens.PrecoItem);

            }
            txtVlrTotal.Text = Convert.ToString(ValorTotal);
            txtVlrTotal.Text = Convert.ToDouble(txtVlrTotal.Text).ToString("C");

        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Response.Redirect("~/Pages/Servicos/ManItemDocumento.aspx");
        }

        protected void txtCodSolAtendimento_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodSolAtendimento.Text.Equals(""))
            {
                txtCodSolAtendimento.Text = "";
                return;
            }
            else
            {
                v.CampoValido("Código Solicitação atendimento", txtCodSolAtendimento.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtCodSolAtendimento.Text = Convert.ToDecimal(txtCodSolAtendimento.Text).ToString();

                }
                else
                {
                    txtCodSolAtendimento.Text = "";
                    return;
                }

            }
            Doc_SolicitacaoAtendimento doc = new Doc_SolicitacaoAtendimento();
            Doc_SolicitacaoAtendimentoDAL docDAL = new Doc_SolicitacaoAtendimentoDAL();
            doc = docDAL.PesquisarDocumento(Convert.ToDecimal(txtCodSolAtendimento.Text));
            if (doc != null)
            {
                litDescricao.Text = doc.DescricaoDocumento;
                Session["RascunhoDocumento"] = doc.DescricaoDocumento;
                txtCodPessoa.Text = Convert.ToString(doc.Cpl_CodigoPessoa);
                SelectedPessoa(sender, e);
                ddlContato.SelectedValue = doc.CodigoContato.ToString();
                txtEmail.Text = doc.Cpl_MailSolicitante;
                txtTelefone.Text = doc.Cpl_FoneSolicitante;
                ddlEmpresa.SelectedValue = doc.CodigoEmpresa.ToString();
            }
            else
            {
                ShowMessage("Solicitação não existente", MessageType.Info);
            }
        }

        protected void grdItemDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["ZoomItemDocumento"] = HttpUtility.HtmlDecode(grdItemDocumento.SelectedRow.Cells[0].Text);
            Response.Redirect("~/Pages/Servicos/ManItemDocumento.aspx");                    
        }

        protected void grdOSs_SelectedIndexChanged(object sender, GridViewCommandEventArgs e)
        {

            string command = e.CommandName;

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = grdOSs.Rows[index];
            decimal Codigo = Convert.ToDecimal(Server.HtmlDecode(row.Cells[1].Text));

            ItemDocumentoDAL ItemDAL = new ItemDocumentoDAL();
            List<Doc_OrdemServico> listaNova = new List<Doc_OrdemServico>();
            List<ItemDocumento> ListaItemNova = new List<ItemDocumento>();
            int intEndItem = 0;
            foreach (Doc_OrdemServico doc in ListaOSs)
            {
                if(command == "btnAddOS")
                {
                    if (doc.CodigoDocumento == Codigo)
                    {
                        if (ListaItemDocumento.Count != 0)
                            intEndItem = Convert.ToInt32(ListaItemDocumento.Max(x => x.CodigoItem).ToString());
                        
                        doc.BtnAdd = false;
                        doc.BtnRemove = true;
                        List<ItemDocumento> ListaItem = new List<ItemDocumento>();
                        ListaItem = ItemDAL.ObterItemDocumento(Convert.ToDecimal(row.Cells[1].Text));

                        foreach (ItemDocumento item in ListaItem)
                        {
                            intEndItem++;
                            ItemDocumento itemNovo = new ItemDocumento(intEndItem,
                                                                        doc.CodigoDocumento,
                                                                        item.CodigoUsuarioAtendente,
                                                                        item.DataHoraInicio,
                                                                        item.DataHoraFim,
                                                                        item.CodigoSituacao,
                                                                        item.DescricaoItem,
                                                                        item.Cpl_NomeUsuario,
                                                                        item.Cpl_DescSituacao);

                            ListaItemDocumento.Add(itemNovo);                            
                        }
                    }
                    Session["ListaItemDocumento"] = ListaItemDocumento;
                    grdItemDocumento.DataSource = ListaItemDocumento;
                    grdItemDocumento.DataBind();
                }
                else if (command == "btnRemoveOS")
                {
                    if (doc.CodigoDocumento == Codigo)
                    {
                        doc.BtnAdd = true;
                        doc.BtnRemove = false;
                        int i = 0;
                        foreach(ItemDocumento item in ListaItemDocumento)
                        {
                            if(item.CodigoDocumento == Codigo)
                            {
                                i++;
                            }
                        }
                        if(i!=0)
                            ListaItemDocumento.RemoveAll(x => x.CodigoDocumento == Codigo);

                        ListaItemDocumento = ListaItemDocumento.OrderBy(c => c.CodigoItem).ToList();
                        Session["ListaItemDocumento"] = ListaItemDocumento;
                        grdItemDocumento.DataSource = ListaItemDocumento;
                        grdItemDocumento.DataBind();

                    }

                }
                listaNova.Add(doc);
            }
            
            Session["ListaOSs"] = listaNova;
            grdOSs.DataSource = listaNova;
            grdOSs.DataBind();
        }

        protected void ddlClassificacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClassificacao.SelectedValue == "98")
            {
                Habil_TipoDAL sd = new Habil_TipoDAL();
                ddlSituacao.DataSource = sd.SituacaoOrdemFatura();
                ddlSituacao.DataTextField = "DescricaoTipo";
                ddlSituacao.DataValueField = "CodigoTipo";
                ddlSituacao.DataBind();

                btnAddItem.Visible = false;
                ddlSituacao.Enabled = false;
                pnlCobranca.Visible = true;

                Session["ListaItemDocumento"] = null;
                grdItemDocumento.DataSource = null;
                grdItemDocumento.DataBind();
                lblCampoObrigatorio.Visible = false;
                TabCobranca = "display:block";
                btnFaturar.Visible = true;

            }
            else
            {
                Habil_TipoDAL sd = new Habil_TipoDAL();
                ddlSituacao.DataSource = sd.SituacaoOrdemServico();
                ddlSituacao.DataTextField = "DescricaoTipo";
                ddlSituacao.DataValueField = "CodigoTipo";
                ddlSituacao.DataBind();

                ddlSituacao.Enabled = true;
                btnAddItem.Visible = true;
                pnlCobranca.Visible = false;
                lblCampoObrigatorio.Visible = true;
                TabCobranca = "display:none";
                btnFaturar.Visible = false;
            }
                
            Boolean blnCampo = false;
            if (txtCodPessoa.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Pessoa", txtCodPessoa.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
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

            if (ddlClassificacao.SelectedValue == "98")
            {
                Doc_OrdemServicoDAL docDAL = new Doc_OrdemServicoDAL();

                if (txtCodigo.Text != "" && txtCodigo.Text != "Novo")
                    ListaOSs = docDAL.ListarOrdemServicoServico(Convert.ToInt64(txtCodPessoa.Text),97, Convert.ToDecimal(txtCodigo.Text));
                else
                    ListaOSs = docDAL.ListarOrdemServicoServico(Convert.ToInt64(txtCodPessoa.Text), 97,0);

                grdOSs.DataSource = ListaOSs;
                grdOSs.DataBind();
                Session["ListaOSs"] = ListaOSs;

                grdItemDocumento.DataSource = null;
                grdItemDocumento.DataBind();
                Session["ListaItemDocumento"] = null;
;
            }
            else if (ddlClassificacao.SelectedValue == "97")
            {
                grdOSs.DataSource = null;
                grdOSs.DataBind();
                Session["ListaOSs"] = null;

            }

        }

        protected void btnFaturar_Click(object sender, EventArgs e)
        {
            ddlAcao.SelectedValue = "1";
            btnSalvar_Click(sender, e);

            if (txtCodigo.Text == "Novo" || txtCodigo.Text == "")
                return;
            int i = 0;
            foreach (ItemDocumento item in ListaItemDocumento)
            {
                if (item.CodigoSituacao == 100)
                    i++;
            }
            if (i == 0)
            {
                ShowMessage("Para faturar, pelo menos um dos itens deve estar FINALIZADO", MessageType.Info);
                return;
            }
            Doc_NotaFiscalServico doc = new Doc_NotaFiscalServico();
            Doc_NotaFiscalServicoDAL docDAL = new Doc_NotaFiscalServicoDAL();

            GeradorSequencialDocumentoEmpresa gerador = new GeradorSequencialDocumentoEmpresa();
            GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
            gerador = geradorDAL.PesquisarGeradorSequencialEmpresa(Convert.ToInt32(ddlEmpresa.SelectedValue), 1);

            DBTabelaDAL db = new DBTabelaDAL();
            GeracaoSequencialDocumento gerDoc = new GeracaoSequencialDocumento();
            GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
            gerDoc = gerDocDAL.PesquisarGeradorSequencial(gerador.CodigoGeradorSequencialDocumento);

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            doc.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
            doc.Cpl_Usuario = Convert.ToInt32(Session["CodPflUsuario"]);
            doc.CodigoPrestador = Convert.ToInt32(ddlEmpresa.SelectedValue);
            doc.CodigoTomador = Convert.ToInt32(txtCodPessoa.Text);
            doc.ValorTotalNota = Convert.ToDecimal(txtVlrTotal.Text.Substring(2));
            doc.DescricaoGeralServico = txtOBS.Text;
            doc.DGSerieDocumento = gerDoc.SerieConteudo;
            doc.CodigoCondicaoPagamento = Convert.ToInt32(ddlPagamento.SelectedValue);
            doc.CodigoDocumentoOriginal = Convert.ToDecimal(txtCodigo.Text);

            Pessoa_Endereco pesEnd = new Pessoa_Endereco();
            PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
            pesEnd = pesEndDAL.PesquisarPessoaEndereco(Convert.ToInt32(txtCodPessoa.Text), 1);
            doc.CodigoMunicipioPrestado = pesEnd._CodigoMunicipio;

            DBTabelaDAL RnTab = new DBTabelaDAL();
            doc.DataEmissao = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"));
            doc.DataLancamento = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"));

            ParSistema parSis = new ParSistema();
            ParSistemaDAL parSisDAL = new ParSistemaDAL();
            parSis = parSisDAL.PesquisarParSistema(doc.CodigoPrestador);

            if(parSis == null)
            {
                ShowMessage("Tipo Operação Inválida", MessageType.Info);
                return;
            }
            doc.CodigoTipoOperacao = parSis.CodigoTipoOperacao;

            DateTime DataHoraEvento = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm"));
            // Se existe a tabela sequencial
            if (db.BuscaTabelas(gerador.Cpl_Nome) == gerador.Cpl_Nome)
            {
                double NroSequencial = geradorDAL.ExibeProximoNroSequencial(gerador.Cpl_Nome);
                if (NroSequencial == 0)
                    doc.NumeroDocumento = Convert.ToInt32(gerDoc.NumeroInicial.ToString());
                else
                    doc.NumeroDocumento = Convert.ToInt32(NroSequencial.ToString());

                doc.DGSerieDocumento = gerDoc.SerieConteudo.ToString();
                doc.Cpl_NomeTabela = gerador.Cpl_Nome;
            }
            else
            {
                doc.NumeroDocumento = Convert.ToInt32(gerDoc.NumeroInicial.ToString());
                doc.DGSerieDocumento = gerDoc.SerieConteudo;
            }
            List<ParcelaDocumento> ListaParcela2 = new List<ParcelaDocumento>();
            foreach (ParcelaDocumento p in ListaParcelaDocumento)
            {
                ParcelaDocumento par = new ParcelaDocumento();
                par.CodigoDocumento = p.CodigoDocumento;
                par.CodigoParcela = p.CodigoParcela;
                par.DataVencimento = p.DataVencimento;
                par.ValorParcela = p.ValorParcela;
                par.DGNumeroDocumento = doc.NumeroDocumento + "/" + p.CodigoParcela;
                ListaParcela2.Add(par);
            }
            doc.CodigoSituacao = 42;

            EventoDocumento evento = new EventoDocumento(1,
                                                        42,
                                                        DataHoraEvento,
                                                        he.CodigoEstacao,
                                                        Convert.ToInt32(Session["CodUsuario"])
                                                        );
            decimal CodigoDocumento = 0;
            docDAL.Inserir(doc, ListaTipoServico, ListaItemTipoServico, evento, ListaAnexo, ListaParcela2, ref CodigoDocumento);

            List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
            DBTabelaCampos rowp = new DBTabelaCampos();
            rowp.Filtro = "CD_DOCUMENTO";
            rowp.Inicio = CodigoDocumento.ToString();
            rowp.Fim = CodigoDocumento.ToString();
            rowp.Tipo = "NUMERIC";
            listaT.Add(rowp);
            Session["LST_NOTAFISCALSERVICO"] = listaT;

            Session["MensagemTela"] = "Nota Fiscal de Serviço Gerada com sucesso";
            Response.Redirect("~/Pages/Servicos/ConNotaFiscalServico.aspx");
        }

    }
}