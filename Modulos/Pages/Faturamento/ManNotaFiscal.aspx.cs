using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;
using NFSeX;
using System.Data;
using System.ComponentModel;
using System.Threading;
using System.Web.Services;
using System.IO;
using System.Xml;

namespace SoftHabilInformatica.Pages.Faturamento
{
    public partial class ManNotaFiscal : System.Web.UI.Page
    {
        public string Panels { get; set; }

        public string PanelLogs { get; set; } = "display:none";

        public string PanelSelect { get; set; }

        List<Doc_CtaReceber> ListaCtaReceber = new List<Doc_CtaReceber>();

        List<ProdutoDocumento> ListaItemDocumento = new List<ProdutoDocumento>();

        List<EventoDocumento> ListaEvento = new List<EventoDocumento>();

        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();

        List<Doc_Pedido> ListaOutrosPedidos = new List<Doc_Pedido>();

        List<Habil_Log> ListaLog = new List<Habil_Log>();

        clsValidacao v = new clsValidacao();

        List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();

        String strMensagemR = "";

        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void ShowTabFocada(string TabFocada)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "TabFocada('"+ TabFocada + "')", true);
            PanelSelect = TabFocada;
        }

        protected void LimpaTela()
        {
            CarregaTiposSituacoes();
            txtCodigo.Text = "Novo";
            DBTabelaDAL RnTab = new DBTabelaDAL();

            List<ParSistema> ListPar = new List<ParSistema>();
            ParSistemaDAL ParDAL = new ParSistemaDAL();
            if (Session["VW_Par_Sistema"] == null)
                Session["VW_Par_Sistema"] = ParDAL.ListarParSistemas("CD_EMPRESA", "INT", Session["CodEmpresa"].ToString(), "");

            
            ListPar = (List<ParSistema>)Session["VW_Par_Sistema"];

            DateTime Hoje = RnTab.ObterDataHoraServidor();
            txtdtemissao.Text = Hoje.ToString("dd/MM/yyyy");
            txtHrEmissao.Text = Hoje.ToString("HH:mm:ss");

            txtCNPJCPFCredor.Text = "";
            txtEndereco.Text = "";
            txtEstado.Text = "";
            txtCEP.Text = "";
            txtCidade.Text = "";
            txtBairro.Text = "";
            txtCodPessoa.Text = "";
            txtDescricao.Text = "";
            txtNroDocumento.Text = "";
            txtNroSerie.Text = "";

            pnlChaveAcesso.Visible = false;
            pnlImportacaoNF.Visible = true;
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
            ddlSituacao.DataSource = sd.SituacaoPedido();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            ddlIndicadorPresenca.DataSource = sd.TipoIndicadorPresenca();
            ddlIndicadorPresenca.DataTextField = "DescricaoTipo";
            ddlIndicadorPresenca.DataValueField = "CodigoReferencia";
            ddlIndicadorPresenca.DataBind();

            ddlFinalidade.DataSource = sd.TipoFinalidade();
            ddlFinalidade.DataTextField = "DescricaoTipo";
            ddlFinalidade.DataValueField = "CodigoReferencia";
            ddlFinalidade.DataBind();

            ddlConsumidorFinal.DataSource = sd.TipoConsumidorFinal();
            ddlConsumidorFinal.DataTextField = "DescricaoTipo";
            ddlConsumidorFinal.DataValueField = "CodigoReferencia";
            ddlConsumidorFinal.DataBind();

            ddlModalidadeFrete.DataSource = sd.TipoModalidadeFrete();
            ddlModalidadeFrete.DataTextField = "DescricaoTipo";
            ddlModalidadeFrete.DataValueField = "CodigoReferencia";
            ddlModalidadeFrete.DataBind();

            TipoCobrancaDAL RnCobranca = new TipoCobrancaDAL();
            ddlTipoCobranca.DataSource = RnCobranca.ListarTipoCobrancas("CD_SITUACAO", "INT", "1", "");
            ddlTipoCobranca.DataTextField = "DescricaoTipoCobranca";
            ddlTipoCobranca.DataValueField = "CodigoTipoCobranca";
            ddlTipoCobranca.DataBind();
            ddlTipoCobranca.Items.Insert(0, ".....SELECIONE UM TIPO DE COBRANÇA.....");

            CondPagamentoDAL CondPagam = new CondPagamentoDAL();
            ddlPagamento.DataSource = CondPagam.ListarCondPagamento("CD_SITUACAO", "INT", "1", "");
            ddlPagamento.DataTextField = "DescricaoCondPagamento";
            ddlPagamento.DataValueField = "CodigoCondPagamento";
            ddlPagamento.DataBind();
            ddlPagamento.Items.Insert(0, "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....");

            TipoOperacaoDAL tpOP = new TipoOperacaoDAL();
            ddlTipoOperacao.DataSource = tpOP.ListarTipoOperacoes("CD_SITUACAO", "INT", "1", "").Where(x => x.CodTipoOperFiscal == 3).ToList(); 
            ddlTipoOperacao.DataTextField = "Cpl_ComboDescricaoTipoOperacao";
            ddlTipoOperacao.DataValueField = "CodigoTipoOperacao";
            ddlTipoOperacao.DataBind();
            ddlTipoOperacao.Items.Insert(0, ".....SELECIONE TIPO DE OPERAÇÃO.....");

            NatOperacaoDAL natOP = new NatOperacaoDAL();
            ddlNatOperacao.DataSource = natOP.ListarNatOperacao("", "", "", "");
            ddlNatOperacao.DataTextField = "Cpl_ComboDescricaoNatOperacao";
            ddlNatOperacao.DataValueField = "CodigoNaturezaOperacao";
            ddlNatOperacao.DataBind();
            ddlNatOperacao.Items.Insert(0, ".....SELECIONE NATUREZA DE OPERAÇÃO.....");

            Habil_RegTributarioDAL rtd = new Habil_RegTributarioDAL();
            ddlRegime.DataSource = rtd.ListaHabil_RegTributario();
            ddlRegime.DataTextField = "DescricaoHabil_RegTributario";
            ddlRegime.DataValueField = "CodigoHabil_RegTributario";
            ddlRegime.DataBind();
            ddlRegime.Items.Insert(0, "*Nenhum Selecionado");
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

            if (txtDtSaida.Text + " " + txtHrSaida.Text != "01/01/2000 00:00:00" && txtDtSaida.Text + " " + txtHrSaida.Text != " ")
            {
                v.CampoValido("Data de Saída", txtDtSaida.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtDtSaida.Focus();
                    }
                    return false;
                }
            }

            v.CampoValido("Código Cliente", txtCodPessoa.Text, true, true, true, false, "INT", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodPessoa.Focus();
                }
                return false;
            }

            if ((ddlTipoCobranca.Text == ".....SELECIONE UM TIPO DE COBRANÇA....."))
            {
                ShowMessage("Escolha um Tipo de Cobrança", MessageType.Info);
                ddlTipoCobranca.Focus();
                return false;
            }

            if (ddlPagamento.SelectedValue == "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....")
            {
                ShowMessage("Selecione uma Condição de pagamento", MessageType.Info);
                ddlPagamento.Focus();
                return false;
            }

            if (ddlTipoOperacao.SelectedValue == ".....SELECIONE TIPO DE OPERAÇÃO.....")
            {
                ShowMessage("Selecione um tipo de operação", MessageType.Info);
                ddlTipoOperacao.Focus();
                return false;
            }
            if (ddlNatOperacao.SelectedValue == ".....SELECIONE NATUREZA DE OPERAÇÃO.....")
            {
                ShowMessage("Selecione uma natureza de operação", MessageType.Info);
                ddlNatOperacao.Focus();
                return false;
            }
            if ((ddlRegime.SelectedValue == "*Nenhum Selecionado"))
            {
                ShowMessage("Escolha um Regime tributário", MessageType.Info);
                ddlRegime.Focus();
                return false;
            }
            if (ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList().Count == 0)
            {
                ShowMessage("Adicione itens na Nota Fiscal!", MessageType.Warning);
                return false;
            }

            return true;
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

            if (Session["TabFocadaNF"] != null)
            {
                if (Session["TabFocadaNF"].ToString() == "home")
                    PanelSelect = "aba1";
                else if (Session["TabFocadaNF"].ToString() == "consulta4")
                    PanelSelect = "aba7";
                else
                    PanelSelect = Session["TabFocadaNF"].ToString();
            }
            else
            {
                PanelSelect = "aba1";
                Session["TabFocadaNF"] = "aba1";
            }

            MontaTela(sender, e);
        }
        
        protected void MontaTela(object sender, EventArgs e)
        {
            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConNotaFiscal.aspx");
            lista.ForEach(delegate (Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoAlterar)
                        btnSalvar.Visible = false;
                }
            });
            
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomNotaFiscal2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomNotaFiscal"] != null)
                {
                    string s = Session["ZoomNotaFiscal"].ToString();
                    Session["ZoomNotaFiscal"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnCancelar.Visible = true;
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;
                                txtCodPessoa.Enabled = false;
                                btnPessoa.Enabled = false;
                                ddlEmpresa.Enabled = false;
                                
                                CarregaTiposSituacoes();

                                Doc_NotaFiscal doc = new Doc_NotaFiscal();
                                Doc_NotaFiscalDAL docDAL = new Doc_NotaFiscalDAL();

                                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));
                                if (doc == null)
                                {
                                    Session["MensagemTela"] = "Esta nota fiscal não existe";
                                    btnVoltar_Click(sender, e);
                                }

                                if (doc.DataHoraSaida.ToString() != "01/01/2000 00:00:00" && doc.DataHoraSaida != null)
                                {
                                    txtDtSaida.Text = doc.DataHoraSaida.Value.ToString("dd/MM/yyyy");
                                    txtHrSaida.Text = doc.DataHoraSaida.Value.ToString("HH:mm:ss");
                                }

                                txtdtemissao.Text = doc.DataHoraEmissao.ToString("dd/MM/yyyy");
                                txtHrEmissao.Text = doc.DataHoraEmissao.ToString("HH:mm:ss");
                                ddlEmpresa.SelectedValue = doc.CodigoEmpresa.ToString();
                                txtDescricao.Text = doc.DescricaoDocumento;
                                txtNroDocumento.Text = doc.NumeroDocumento.ToString();
                                txtNroSerie.Text = doc.DGSerieDocumento;
                                ddlSituacao.SelectedValue = doc.CodigoSituacao.ToString();
                                txtFrete.Text = doc.ValorFrete.ToString();
                                txtPesoBruto.Text = doc.ValorPesoBruto.ToString();
                                ddlRegime.SelectedValue = doc.CodigoRegimeTributario.ToString();
                                ddlTipoOperacao.SelectedValue = doc.CodigoTipoOperacao.ToString();
                                ddlTipoCobranca.SelectedValue = doc.CodigoTipoCobranca.ToString();
                                ddlNatOperacao.SelectedValue = doc.CodigoNaturezaOperacao.ToString();
                                ddlIndicadorPresenca.SelectedValue = doc.CodigoIndicadorPresenca.ToString();
                                ddlFinalidade.SelectedValue = doc.CodigoFinalidadeNF.ToString();
                                ddlConsumidorFinal.SelectedValue = doc.CodigoConsumidorFinal.ToString();
                                ddlModalidadeFrete.SelectedValue = doc.CodigoModalidadeFrete.ToString();

                                if (doc.NumeroWeb != 0)
                                    txtNroWeb.Text = doc.NumeroWeb.ToString();

                                txtCodPessoa.Text = Convert.ToString(doc.Cpl_CodigoPessoa);
                                txtCodPessoa_TextChanged(sender, e);

                                txtCodTransportador.Text = doc.Cpl_CodigoTransportador.ToString();
                                txtCodTransportador_TextChanged(sender, e);

                                ddlEmpresa_TextChanged(sender, e);
                                PanelSelect = "aba1";
                                Session["TabFocadaNF"] = "aba1";

                                pnlChaveAcesso.Visible = true;
                                pnlImportacaoNF.Visible = false;

                                EventoDocumentoDAL eve = new EventoDocumentoDAL();
                                ListaEvento = eve.ObterEventos(Convert.ToInt64(txtCodigo.Text));
                                Session["Eventos"] = ListaEvento;

                                Habil_LogDAL log = new Habil_LogDAL();
                                ListaLog = log.ListarLogs(Convert.ToDouble(txtCodigo.Text), 100);
                                Session["Logs"] = ListaLog;

                                AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
                                ListaAnexo = anexo.ObterAnexos(Convert.ToInt32(txtCodigo.Text));
                                Session["NovoAnexo"] = ListaAnexo;

                                ProdutoDocumentoDAL item = new ProdutoDocumentoDAL();
                                ListaItemDocumento = item.ObterItemOrcamentoPedido(Convert.ToDecimal(txtCodigo.Text));
                                Session["ListaItemNF"] = ListaItemDocumento;

                                ParcelaDocumentoDAL ParcelaDAL = new ParcelaDocumentoDAL();
                                ListaParcelaDocumento = ParcelaDAL.ObterParcelaDocumento(Convert.ToInt32(txtCodigo.Text));
                                Session["ListaParcelaDocumento"] = ListaParcelaDocumento;

                                ddlTipoOperacao.SelectedValue = doc.CodigoTipoOperacao.ToString();
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
                    
                    ddlEmpresa_TextChanged(sender, e);
                    btnNovoAnexo.Visible = false;
                    btnCancelar.Visible = false;

                    if (Session["CodEmpresa"] != null)
                        ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
                    
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;

                        }
                    });
                }
            }
            if (Session["Doc_NotaFiscal"] != null)
            {
                CarregaTiposSituacoes();
                PreencheDados(sender, e);
            }
            if (Session["ZoomTranspNF"] != null)
            {
                txtCodTransportador.Text = Session["ZoomTranspNF"].ToString().Split('³')[0];
                txtCodTransportador_TextChanged(sender, e);
                Session["ZoomTranspNF"] = null;
            }
            else if (Session["ZoomPessoaNF"] != null)
            {
                txtCodPessoa.Text = Session["ZoomPessoaNF"].ToString().Split('³')[0];
                txtCodPessoa_TextChanged(sender, e);
                Session["ZoomPessoaNF"] = null;
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

                if (ListaLog.Count > 0)
                    PanelLogs = "display:block";
            }

            if (Session["NovoAnexo"] != null)
            {
                ListaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];
                grdAnexo.DataSource = ListaAnexo;
                grdAnexo.DataBind();
            }
            if (Session["ListaItemNF"] != null)
            {
                ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemNF"];
                ListaItemDocumento = ListaItemDocumento.OrderByDescending(x => x.CodigoItem).ToList();
                grdProduto.DataSource = ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList();
                grdProduto.DataBind();
            }
            if (Session["ListaContaReceber"] != null)
            {
                ListaCtaReceber = (List<Doc_CtaReceber>)Session["ListaContaReceber"];
                grdFinanceiro.DataSource = ListaCtaReceber;
                grdFinanceiro.DataBind();
            }

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
                }
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

            if (txtCodigo.Text == "")
            {
                btnVoltar_Click(sender, e);
            }
            else if (txtCodigo.Text == "Novo")
            {
                Panels = "display:none";
                btnNovoAnexo.Visible = false;
                PanelLogs = "display:none";
            }
            else
            {
                Panels = "display:block";
                btnNovoAnexo.Visible = true;
                if (ddlSituacao.SelectedValue != "136" )
                {
                    btnSalvar.Visible = false;
                    btnCancelar.Visible = false;
                    
                }
                else
                {
                    if (txtDtSaida.Text + " " + txtHrSaida.Text != "01/01/2000 00:00:00" && txtDtSaida.Text + " " + txtHrSaida.Text != " ")
                    {
                        if (Convert.ToDateTime(txtDtSaida.Text + " " + txtHrSaida.Text) < Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")))
                        {
                            btnSalvar.Visible = false;
                            btnCancelar.Visible = false;
                        }
                    }
                }
                //ddlRegime.Enabled = false;
                ddlTipoOperacao.Enabled = false;
            }

            if(txtNroPedido.Text != "")
            {
                txtCodPessoa.Enabled = false;
                btnPessoa.Enabled = false;
                ddlEmpresa.Enabled = false;
                ddlRegime.Enabled = false;
                ddlTipoOperacao.Enabled = false;
            }

            MontarValorTotal(ListaItemDocumento);

            if (!IsPostBack && ListaItemDocumento != null && ListaItemDocumento.Count != 0)
            {
                CalcularPesoTotal();
            }

        }

        protected void PreencheDados(object sender, EventArgs e)
        {
            try
            { 
                Doc_NotaFiscal doc = (Doc_NotaFiscal)Session["Doc_NotaFiscal"];
                ddlEmpresa.SelectedValue = Convert.ToString(doc.CodigoEmpresa);
                ddlSituacao.SelectedValue = Convert.ToString(doc.CodigoSituacao);
                txtDescricao.Text = doc.DescricaoDocumento;
                txtNroSerie.Text = Convert.ToString(doc.DGSerieDocumento);
                txtFrete.Text = doc.ValorFrete.ToString();
                txtPesoBruto.Text = doc.ValorPesoBruto.ToString();
                ddlRegime.SelectedValue = doc.CodigoRegimeTributario.ToString();
                ddlIndicadorPresenca.SelectedValue = doc.CodigoIndicadorPresenca.ToString();
                ddlFinalidade.SelectedValue = doc.CodigoFinalidadeNF.ToString();
                ddlConsumidorFinal.SelectedValue = doc.CodigoConsumidorFinal.ToString();
                ddlModalidadeFrete.SelectedValue = doc.CodigoModalidadeFrete.ToString();

                if (txtNroDocumento.Text == "")
                    txtNroDocumento.Text = Convert.ToString(doc.NumeroDocumento);

                if (doc.CodigoDocumento == 0)
                {
                    txtCodigo.Text = "Novo";
                    btnCancelar.Visible = false;
                    pnlChaveAcesso.Visible = false;
                    pnlImportacaoNF.Visible = true;
                }
                else
                {
                    txtCodigo.Text = Convert.ToString(doc.CodigoDocumento);
                    txtCodPessoa.Enabled = false;
                    btnPessoa.Enabled = false;
                    ddlEmpresa.Enabled = false;
                    pnlChaveAcesso.Visible = true;
                    pnlImportacaoNF.Visible = false;
                }

                if (Convert.ToString(doc.DataHoraSaida) != "01/01/2000 00:00:00" && Convert.ToString(doc.DataHoraSaida) != "01/01/0001 00:00:00" && doc.DataHoraSaida != null)
                {
                    txtDtSaida.Text = doc.DataHoraSaida.Value.ToString("dd/MM/yyyy");
                    txtHrSaida.Text = doc.DataHoraSaida.Value.ToString("HH:mm:ss");
                }
                if (Convert.ToString(doc.DataHoraEmissao) != "01/01/2000 00:00:00" && Convert.ToString(doc.DataHoraEmissao) != "01/01/0001 00:00:00")
                {
                    txtdtemissao.Text = doc.DataHoraEmissao.ToString("dd/MM/yyyy");
                    txtHrEmissao.Text = doc.DataHoraEmissao.ToString("HH:mm:ss");
                }

                if (doc.CodigoTipoOperacao != 0)
                    ddlTipoOperacao.SelectedValue = doc.CodigoTipoOperacao.ToString();

                if (doc.CodigoNaturezaOperacao != 0)
                    ddlNatOperacao.SelectedValue = doc.CodigoNaturezaOperacao.ToString();

                if (doc.CodigoFinalidadeNF != 0)
                    ddlFinalidade.SelectedValue = doc.CodigoFinalidadeNF.ToString();

                if (doc.NumeroWeb != 0)
                    txtNroWeb.Text = doc.NumeroWeb.ToString();

                if (doc.CodigoTipoCobranca != 0)
                    ddlTipoCobranca.SelectedValue = doc.CodigoTipoCobranca.ToString();

                if (doc.CodigoCondicaoPagamento != 0)
                    ddlPagamento.SelectedValue = doc.CodigoCondicaoPagamento.ToString();

                if (doc.Cpl_CodigoTransportador != 0)
                {
                    txtCodTransportador.Text = doc.Cpl_CodigoTransportador.ToString();
                    txtCodTransportador_TextChanged(sender, e);
                }

                if (doc.Cpl_CodigoPessoa != 0)
                {
                    txtCodPessoa.Text = doc.Cpl_CodigoPessoa.ToString();
                    txtCodPessoa_TextChanged(sender, e);
                }

                Session["Doc_NotaFiscal"] = null;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            Doc_NotaFiscalDAL doc = new Doc_NotaFiscalDAL();
            doc.Excluir(Convert.ToDecimal(txtCodigo.Text));

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            Session["MensagemTela"] = "Documento Cancelado com sucesso!";
            btnVoltar_Click(sender, e);
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("~/Pages/Faturamento/ConNotaFiscal.aspx");
        }

        protected void SalvarDocumento(object sender, EventArgs e, bool ContinuarAberto)
        {
            try
            {   
                if (ValidaCampos() == false)
                    return;

                Doc_NotaFiscal p = new Doc_NotaFiscal();
                Doc_NotaFiscalDAL pe = new Doc_NotaFiscalDAL();

                p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
                p.DGNumeroDocumento = "";
                p.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
                p.DGSerieDocumento = txtNroSerie.Text;
                p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                p.DescricaoDocumento = txtDescricao.Text;
                p.CodigoCondicaoPagamento = Convert.ToInt32(ddlPagamento.SelectedValue);
                p.CodigoTipoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);
                p.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text + " " + txtHrEmissao.Text);
                p.ValorTotalDocumento = Convert.ToDecimal(txtVlrTotal.Text);
                p.ValorFrete = Convert.ToDecimal(txtFrete.Text);
                p.ValorPesoBruto = Convert.ToDecimal(txtPesoBruto.Text);
                p.CodigoRegimeTributario = Convert.ToInt32(ddlRegime.SelectedValue);
                p.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
                p.CodigoNaturezaOperacao = Convert.ToInt64(ddlNatOperacao.SelectedValue);
                p.CodigoIndicadorPresenca = Convert.ToInt32(ddlIndicadorPresenca.SelectedValue);
                p.CodigoFinalidadeNF = Convert.ToInt32(ddlFinalidade.SelectedValue);
                p.CodigoConsumidorFinal = Convert.ToInt32(ddlConsumidorFinal.SelectedValue);
                p.CodigoModalidadeFrete = Convert.ToInt32(ddlModalidadeFrete.SelectedValue);

                if (txtDtSaida.Text + " " + txtHrSaida.Text != " " && txtDtSaida.Text + " " + txtHrSaida.Text != "01/01/2000 00:00:00")
                    p.DataHoraSaida = Convert.ToDateTime(txtDtSaida.Text + " " + txtHrSaida.Text);

                if (txtNroWeb.Text != "")
                    p.NumeroWeb = Convert.ToDecimal(txtNroWeb.Text);

                if (txtNroPedido.Text != "")
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(txtNroPedido.Text);

                //COMPLEMENTARES
                p.Cpl_CodigoTransportador = Convert.ToInt64(txtCodTransportador.Text);
                p.Cpl_CodigoPessoa = Convert.ToInt64(txtCodPessoa.Text);

                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
                p.Cpl_Usuario = Convert.ToInt32(Session["CodUsuario"]);

                if (txtCodigo.Text == "Novo")
                {
                    ddlEmpresa_TextChanged(sender, e);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Session["CodigoGeradorSequencial"]);
                    p.Cpl_NomeTabela = Session["NomeTabela"].ToString();

                    Session["CodigoGeradorSequencial"] = null;

                    pe.Inserir(p, ListaItemDocumento, EventoDocumento(), ListaAnexo,ListaParcelaDocumento);
                    Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";                   
                }
                else
                {
                    Doc_NotaFiscal p2 = new Doc_NotaFiscal();
                    p.CodigoDocumento = Convert.ToInt64(txtCodigo.Text);

                    p2 = pe.PesquisarDocumento(Convert.ToDecimal(txtCodigo.Text));

                    if (Convert.ToInt32(ddlSituacao.SelectedValue) != p2.CodigoSituacao)
                        pe.Atualizar(p, ListaItemDocumento, EventoDocumento(), ListaAnexo, ListaParcelaDocumento);
                    else
                        pe.Atualizar(p, ListaItemDocumento, null, ListaAnexo, ListaParcelaDocumento);

                    Session["MensagemTela"] = "Registro alterado com Sucesso!!!";
                }                           

                btnVoltar_Click(sender, e);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void CompactaDocumento()
        {
            try
            { 
                if (txtCodigo.Text == "")
                    return;

                Doc_NotaFiscal doc = new Doc_NotaFiscal();

                doc.CodigoSituacao = Convert.ToInt32(ddlSituacao.Text);
                doc.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.Text);
                doc.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
                doc.DGSerieDocumento = txtNroSerie.Text;
                doc.DescricaoDocumento = txtDescricao.Text;
                doc.ValorFrete = Convert.ToDecimal(txtFrete.Text);
                doc.ValorPesoBruto = Convert.ToDecimal(txtPesoBruto.Text);
                doc.CodigoIndicadorPresenca = Convert.ToInt32(ddlIndicadorPresenca.SelectedValue);
                doc.CodigoFinalidadeNF = Convert.ToInt32(ddlFinalidade.SelectedValue);
                doc.CodigoConsumidorFinal = Convert.ToInt32(ddlConsumidorFinal.SelectedValue);
                doc.CodigoModalidadeFrete = Convert.ToInt32(ddlModalidadeFrete.SelectedValue);

                if (txtCodigo.Text != "Novo")
                    doc.CodigoDocumento = Convert.ToDecimal(txtCodigo.Text);

                if (txtdtemissao.Text != "")
                    doc.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text + " " + txtHrEmissao.Text);

                if (txtDtSaida.Text != "")
                    doc.DataHoraSaida = Convert.ToDateTime(txtDtSaida.Text +" "+ txtHrSaida.Text);

                if (txtNroWeb.Text != "")
                    doc.NumeroWeb = Convert.ToDecimal(txtNroWeb.Text);

                if (txtNroPedido.Text != "")
                    doc.CodigoDocumentoOriginal = Convert.ToDecimal(txtNroPedido.Text);

                if (txtCodPessoa.Text != "")
                    doc.Cpl_CodigoPessoa = Convert.ToInt64(txtCodPessoa.Text);

                if (txtCodTransportador.Text != "")
                    doc.Cpl_CodigoTransportador = Convert.ToInt64(txtCodTransportador.Text);

                if (ddlRegime.SelectedValue != "*Nenhum Selecionado")
                    doc.CodigoRegimeTributario = Convert.ToInt32(ddlRegime.SelectedValue);

                if (ddlTipoCobranca.SelectedValue != ".....SELECIONE UM TIPO DE COBRANÇA.....")
                    doc.CodigoTipoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);

                if (ddlTipoOperacao.SelectedValue != ".....SELECIONE TIPO DE OPERAÇÃO.....")
                    doc.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);

                if (ddlNatOperacao.SelectedValue != ".....SELECIONE NATUREZA DE OPERAÇÃO.....")
                    doc.CodigoNaturezaOperacao = Convert.ToInt32(ddlNatOperacao.SelectedValue);

                if (ddlPagamento.SelectedValue != "..... SELECIONE UMA CONDIÇÃO DE PAGAMENTO .....")
                    doc.CodigoCondicaoPagamento = Convert.ToInt32(ddlPagamento.SelectedValue);

                Session["Doc_NotaFiscal"] = doc;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void ConPessoa(object sender, EventArgs e)
        {
            CompactaDocumento();
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=19");
        }

        protected void txtCodPessoa_TextChanged(object sender, EventArgs e)
        {

            Boolean blnCampo = false;

            if (txtCodPessoa.Text.Equals(""))
            {
                txtPessoa.Text = "";
                return;
            }
            else
            {
                v.CampoValido("Codigo Cliente", txtCodPessoa.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodPessoa.Text = "";
                    txtPessoa.Text = "";
                    return;
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
                ShowMessage("Pessoa não existente!", MessageType.Warning);
                txtCodPessoa.Text = "";
                txtPessoa.Text = "";
                txtCNPJCPFCredor.Text = "";
                txtCidade.Text = "";
                txtBairro.Text = "";
                //txtRazSocial.Text = "";
                txtEndereco.Text = "";
                txtEstado.Text = "";
                txtCEP.Text = "";
                txtEmail.Text = "";
                txtCodPessoa.Focus();
                return;
            }
            //txtRazSocial.Text = p2.NomePessoa;
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
            txtEmail.Text = ctt._MailNFE;

            PessoaEnderecoDAL end = new PessoaEnderecoDAL();
            List<Pessoa_Endereco> end2 = new List<Pessoa_Endereco>();
            end2 = end.ObterPessoaEnderecos(Convert.ToInt32(codigoPessoa));

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

            Doc_NotaFiscalDAL docDAL = new Doc_NotaFiscalDAL();
            ListaCtaReceber = docDAL.ListarContasAbertasCliente(codigoPessoa);
            Session["ListaContaReceber"] = ListaCtaReceber;
            grdFinanceiro.DataSource = ListaCtaReceber;
            grdFinanceiro.DataBind();

            if (txtCodigo.Text == "Novo")
            {
                if(p2.CodigoCondPagamento != 0)
                    ddlPagamento.SelectedValue = p2.CodigoCondPagamento.ToString();
                if(p2.CodigoTipoOperacao != 0)
                    ddlTipoOperacao.SelectedValue = p2.CodigoTipoOperacao.ToString();
                if (p2.CodigoTipoCobranca != 0)
                    ddlTipoCobranca.SelectedValue = p2.CodigoTipoCobranca.ToString();

            }
            else
            {
                Doc_NotaFiscal doc = new Doc_NotaFiscal();
                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));

                if (doc.Cpl_CodigoPessoa != Convert.ToInt32(txtCodPessoa.Text))
                    ShowMessage("Você alterou o Pessoa do Documento!", MessageType.Info);


            }
            if (txtCodTransportador.Text == "" && p2.CodigoTransportador != 0)
            {
                txtCodTransportador.Text = p2.CodigoTransportador.ToString();
                txtCodTransportador_TextChanged(sender, e);
            }

            ddlPagamento.Focus();

            //MontarGraficoCredito();
            Session["TabFocadaNF"] = "aba1";
            Session["Doc_NotaFiscal"] = null;
        }

        protected void ddlEmpresa_TextChanged(object sender, EventArgs e)
        {
            GeradorSequencialDocumentoEmpresa gerador = new GeradorSequencialDocumentoEmpresa();
            GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
            gerador = geradorDAL.PesquisarGeradorSequencialEmpresa(Convert.ToInt32(ddlEmpresa.SelectedValue), 9);
            if (gerador.CodigoEmpresa == 0)
            {
                Session["MensagemTela"] = "Inclusão não permitida para esta empresa. Numeração sequencial não iniciada";
                btnVoltar_Click(sender, e);

            }
            else
            {
                DBTabelaDAL db = new DBTabelaDAL();
                GeracaoSequencialDocumento gerDoc = new GeracaoSequencialDocumento();
                GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
                gerDoc = gerDocDAL.PesquisarGeradorSequencial(gerador.CodigoGeradorSequencialDocumento);

                // Se existe a tabela sequencial
                if (db.BuscaTabelas(gerador.Cpl_Nome) == gerador.Cpl_Nome)
                {
                    if (txtCodigo.Text != "Novo")
                    {
                        Session["NomeTabela"] = gerador.Cpl_Nome;
                        return;
                    }
                    if (gerDoc.Nome == "" || gerDoc.CodigoSituacao == 2)
                    {
                        Session["MensagemTela"] = "Gerador Sequencial não iniciado";
                        btnVoltar_Click(sender, e);
                    }
                    else
                    {
                        if (gerDoc.Validade < DateTime.Now)
                        {
                            Session["MensagemTela"] = "Gerador Sequencial venceu em " + gerDoc.Validade.ToString("dd/MM/yyyy");
                            btnVoltar_Click(sender, e);
                        }
                    }
                    Session["NomeTabela"] = gerador.Cpl_Nome;
                    Session["CodigoGeradorSequencial"] = gerador.CodigoGeradorSequencialDocumento;

                    Habil_Estacao he = new Habil_Estacao();
                    Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                    he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                    decimal NroVago = geradorDAL.ListaNrosVagos(gerador.Cpl_Nome, Convert.ToInt32(he.CodigoEstacao), Convert.ToInt32(Session["CodPflUsuario"]));

                    if (NroVago == 0)
                    {
                        double NroSequencial = geradorDAL.ExibeProximoNroSequencial(gerador.Cpl_Nome);
                        if (NroSequencial == 0)
                            txtNroDocumento.Text = gerDoc.NumeroInicial.ToString();
                        else
                            txtNroDocumento.Text = NroSequencial.ToString();

                        txtNroSerie.Text = gerDoc.SerieConteudo.ToString();
                    }
                    else
                    {
                        txtNroDocumento.Text = NroVago.ToString();
                        txtNroSerie.Text = gerDoc.SerieConteudo.ToString();
                    }
                }
                else
                {
                    txtNroDocumento.Text = gerDoc.NumeroInicial.ToString();
                    txtNroSerie.Text = gerDoc.SerieConteudo;
                }
            }
        }

        protected void grdAnexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["ZoomDocCtaAnexo"] = HttpUtility.HtmlDecode(grdAnexo.SelectedRow.Cells[0].Text);
            Response.Redirect("~/Pages/financeiros/ManAnexo.aspx?cad=4");
        }

        protected void btnNovoAnexo_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;
            CompactaDocumento();
            Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=13");
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
            PanelSelect = "aba6";
            grdLogDocumento.PageIndex = e.NewPageIndex;
            ListaLog = (List<Habil_Log>)Session["Logs"];
            grdLogDocumento.DataSource = ListaLog;
            grdLogDocumento.DataBind();
        }

        protected void grdLogDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPagamento_TextChanged(object sender, EventArgs e)
        {

        }

        protected void grdPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlParcela1.Visible == true)
            {
                //btnLimpar_Click(sender, e);
                btnGerarParcela_Click(sender, e);
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {

        }

        protected void MontarValorTotal(List<ProdutoDocumento> ListaItens)
        {

            decimal ValorTotal = 0;
            decimal decValorFrete = Convert.ToDecimal(txtFrete.Text);
            txtVlrTotal.Text = Convert.ToDouble("0,00").ToString("F");

            decimal decValorFreteRatiado = 0;
            decimal decDiferencaValorFreteRatiado = 0;

            if ((decValorFrete * ListaItens.Where(x => x.CodigoSituacao != 134).Count()) != 0)
                decValorFreteRatiado = decValorFrete / ListaItens.Where(x => x.CodigoSituacao != 134).Count();

            if ((decValorFreteRatiado * ListaItens.Where(x => x.CodigoSituacao != 134).Count()) != decValorFrete)
            {
                decDiferencaValorFreteRatiado = decValorFrete - (decValorFreteRatiado * ListaItens.Where(x => x.CodigoSituacao != 134).Count());
            }

            List<ProdutoDocumento> NovaListaItens = new List<ProdutoDocumento>();
            foreach (ProdutoDocumento itens in ListaItens)
            {
                if (itens.CodigoSituacao != 134)
                {
                    if (txtCodPessoa.Text != "")
                    {
                        //if (decDiferencaValorFreteRatiado != 0)
                        //{
                        //    //itens.Impostos = ImpostoProdutoDocumentoDAL.PreencherImpostosProdutoDocumento(itens, Convert.ToInt64(ddlEmpresa.SelectedValue), Convert.ToInt64(txtCodPessoa.Text), Convert.ToInt32(ddlTipoOperacao.SelectedValue), Convert.ToInt32(ddlAplicacaoUso.Text), Convert.ToDecimal(txtFrete.Text) - (decDiferencaValorFreteRatiado), false);
                        //    decDiferencaValorFreteRatiado = 0;
                        //}
                        //else
                            //itens.Impostos = ImpostoProdutoDocumentoDAL.PreencherImpostosProdutoDocumento(itens, Convert.ToInt64(ddlEmpresa.SelectedValue), Convert.ToInt64(txtCodPessoa.Text), Convert.ToInt32(ddlTipoOperacao.SelectedValue.Replace(".....SELECIONE TIPO DE OPERAÇÃO.....", "0")), Convert.ToInt32(ddlAplicacaoUso.Text), Convert.ToDecimal(txtFrete.Text), false);
                    }
                    ValorTotal += itens.ValorTotalItem;// + itens.Impostos.ValorICMS + itens.Impostos.ValorPIS + itens.Impostos.ValorCOFINS + itens.Impostos.ValorIPI;
                }
                NovaListaItens.Add(itens);
            }
            lblTotalItens.Text = ValorTotal.ToString("C");
            txtTotalItens.Text = ValorTotal.ToString("F");
            txtVlrTotal.Text = (ValorTotal + Convert.ToDecimal(txtFrete.Text)).ToString("F");

        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            bool blnCampoValido = true;
            v.CampoValido("Código Cliente", txtCodPessoa.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage("Necessário informar o cliente para inserir produto(s)!!", MessageType.Warning);
                    ShowTabFocada("aba2");
                    txtCodPessoa.Focus();
                    return;
                }
            }
            CompactaDocumento();
            Response.Redirect("~/Pages/Faturamento/ManItemNotaFiscal.aspx");
        }

        protected void grdItens_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdProduto.PageIndex = e.NewPageIndex;
            // Carrega os dados
            if (Session["ListaItemNF"] != null)
            {
                ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemNF"];
                grdProduto.DataSource = ListaItemDocumento;
                grdProduto.DataBind();

            }
            PanelSelect = "aba4";
        }

        protected void grdFinanceiro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdFinanceiro.PageIndex = e.NewPageIndex;
            // Carrega os dados
            if (Session["ListaContaReceber"] != null)
            {
                ListaCtaReceber = (List<Doc_CtaReceber>)Session["ListaContaReceber"];
                grdFinanceiro.DataSource = ListaCtaReceber;
                grdFinanceiro.DataBind();

            }
        }

        protected void grdFinanceiro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtCodTransportador_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodTransportador.Text.Equals(""))
            {
                txtTransportador.Text = "";
                return;
            }
            else
            {
                v.CampoValido("Codigo Transportador", txtCodTransportador.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodTransportador.Text = "";
                    return;
                }
            }

            Int64 CodigoPessoa = Convert.ToInt64(txtCodTransportador.Text);
            PessoaDAL Pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();
            p2 = Pessoa.PesquisarTransportador(CodigoPessoa);

            if (p2 == null)
            {
                ShowMessage("Transportador não existente!", MessageType.Info);
                txtCodTransportador.Text = "";
                txtTransportador.Text = "";

                txtCNPJTransp.Text = "";
                txtCidadeTransp.Text = "";
                txtBairroTransp.Text = "";
                txtEnderecoTransp.Text = "";
                txtUFTransp.Text = "";
                txtCEPTransp.Text = "";
                txtEmailTransp.Text = "";

                txtCodTransportador.Focus();
                return;
            }
            PessoaInscricaoDAL pInsDAL = new PessoaInscricaoDAL();

            Pessoa_Contato pCtt = new Pessoa_Contato();
            PessoaContatoDAL pCttDAL = new PessoaContatoDAL();
            pCtt = pCttDAL.PesquisarPessoaContato(CodigoPessoa, 1);

            Pessoa_Endereco pEnd = new Pessoa_Endereco();
            PessoaEnderecoDAL pEndDAL = new PessoaEnderecoDAL();
            pEnd = pEndDAL.PesquisarPessoaEndereco(CodigoPessoa, 1);

            txtTransportador.Text = p2.NomePessoa;
            txtCNPJTransp.Text = pInsDAL.ObterInscricao(CodigoPessoa, 1);
            txtCidadeTransp.Text = pEnd._DescricaoMunicipio;
            txtBairroTransp.Text = pEnd._DescricaoBairro;
            txtEnderecoTransp.Text = pEnd._Logradouro +" - " + pEnd._NumeroLogradouro;
            txtUFTransp.Text = pEnd._DescricaoEstado.Substring(0,2);
            txtCEPTransp.Text = pEnd._CodigoCEP.ToString();
            txtEmailTransp.Text = pCtt._MailNFE;

            Session["TabFocadaNF"] = null;
        }

        protected void btnTransportador_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?cad=18");
        }

        protected void txtNroWeb_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtNroWeb.Text.Equals(""))
            {
                txtNroWeb.Text = "0,00";
            }
            else
            {
                v.CampoValido("Nro Web", txtNroWeb.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtNroWeb.Text = Convert.ToDecimal(txtNroWeb.Text).ToString("###,##0.00");
                }
                else
                    txtNroWeb.Text = "0,00";

            }
        }

        public SortDirection dir
        {
            get
            {
                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["dirState"];
            }
            set
            {
                ViewState["dirState"] = value;
            }
        }

        public static DataTable BuildDataTable<T>(IList<T> lst)
        {
            //create DataTable Structure
            DataTable tbl = CreateTable<T>();
            Type entType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            //get the list item and add into the list
            foreach (T item in lst)
            {
                DataRow row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                tbl.Rows.Add(row);
            }
            return tbl;
        }

        private static DataTable CreateTable<T>()
        {
            //T –> ClassName
            Type entType = typeof(T);
            //set the datatable name as class name
            DataTable tbl = new DataTable(entType.Name);
            //get the property list
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                //add property as column

                if (prop != null)

                {
                    tbl.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

            }
            return tbl;
        }

        protected void grdProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            if (dir == SortDirection.Ascending)
            {
                dir = SortDirection.Descending;
                sortingDirection = "Desc";
            }
            else
            {
                dir = SortDirection.Ascending;
                sortingDirection = "Asc";
            }

            List<ProdutoDocumento> ListOfInt = new List<ProdutoDocumento>();
            ListOfInt = (List<ProdutoDocumento>)grdProduto.DataSource;
            // populate list
            DataTable ListAsDataTable = BuildDataTable<ProdutoDocumento>(ListOfInt);
            DataView ListAsDataView = ListAsDataTable.DefaultView;

            ListAsDataView.Sort = e.SortExpression + " " + sortingDirection;

            grdProduto.DataSource = ListAsDataView;
            grdProduto.DataBind();

            ListaItemDocumento = ListAsDataView.ToTable().Rows.Cast<DataRow>().Select(t => new ProdutoDocumento()
            {
                CodigoItem = t.Field<int>("CodigoItem"),
                CodigoProduto = t.Field<int>("CodigoProduto"),
                Cpl_DscProduto = t.Field<string>("Cpl_DscProduto"),
                Unidade = t.Field<string>("Unidade"),
                Quantidade = t.Field<decimal>("Quantidade"),
                PrecoItem = t.Field<decimal>("PrecoItem"),
                ValorTotalItem = t.Field<decimal>("ValorTotalItem")
            }).ToList();

            Session["ListaItemDocumento"] = ListaItemDocumento;

            for (int i = 0; i < grdProduto.Columns.Count; i++)
            {
                if (grdProduto.Columns[i].SortExpression == e.SortExpression)
                {
                    var cell = grdProduto.HeaderRow.Cells[i];
                    Image image = new Image();
                    image.Height = 15;
                    image.Width = 15;
                    Literal litespaco = new Literal();
                    litespaco.Text = "&emsp;";
                    cell.Controls.Add(litespaco);

                    if (sortingDirection == "Desc")
                        image.ImageUrl = "../../images/down_arrow.png";
                    else
                        image.ImageUrl = "../../images/up_arrow.png";

                    cell.Controls.Add(image);
                }

            }
        }

        protected void txtFrete_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtFrete.Text.Equals(""))
            {
                txtFrete.Text = "0,00";
            }
            else
            {
                v.CampoValido("Frete", txtFrete.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtFrete.Text = Convert.ToDecimal(txtFrete.Text).ToString("###,##0.00");
                    MontarValorTotal(ListaItemDocumento);
                }
                else
                    txtFrete.Text = "0,00";

            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            SalvarDocumento(sender, e, true);
        }

        protected void CalcularPesoTotal()
        {
            txtPesoBruto.Text = Convert.ToDecimal(ListaItemDocumento.Where(y => y.CodigoSituacao != 134).Sum(x => x.ValorPeso * x.Quantidade).ToString()).ToString("#,###0.000");
        }

        protected void btnNovoAnexo_Click1(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;
            CompactaDocumento();
            Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=4");
        }

        protected void txtNroPedido_TextChanged(object sender, EventArgs e)
        {

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

            decimal ValorTotal = Convert.ToDecimal((txtVlrTotal.Text));
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

        protected void btnPesquisarPedido_Click(object sender, EventArgs e)
        {

        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                string strResultadoXML = "";
                if (fileselector.HasFile)
                {
                    string ex = fileselector.FileName;
                    string ex2 = Path.GetExtension(ex);

                    if (ex2.ToLower() == ".xml")
                    {
                        DataTable dt = new DataTable();
                        AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
                        dt.Columns.Add("Cardex");
                        dt.Columns.Add("Codigo_Produto");
                        dt.Columns.Add("Descricao_Produto");
                        dt.Columns.Add("Unidade");
                        dt.Columns.Add("Preco_Tabela");
                        dt.Columns.Add("Cliente");

                        string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\";
                        string fileName = anexo.GerarGUID("xml");

                        BinaryReader br = new BinaryReader(fileselector.PostedFile.InputStream);

                        byte[] ArquivoByte = fileselector.FileBytes;

                        if (System.IO.File.Exists(CaminhoArquivoLog + fileName))
                            System.IO.File.Delete(CaminhoArquivoLog + fileName);

                        FileStream file = new FileStream(CaminhoArquivoLog + fileName, FileMode.Create);

                        BinaryWriter bw = new BinaryWriter(file);
                        bw.Write(ArquivoByte);
                        bw.Close();

                        file = new FileStream(CaminhoArquivoLog + fileName, FileMode.Open);
                        BinaryReader br2 = new BinaryReader(file);
                        file.Close();

                        XmlTextReader xml1 = new XmlTextReader(CaminhoArquivoLog + fileName);

                        DataSet dataSet = new DataSet();
                        dataSet.ReadXml(xml1);
                        xml1.Close();

                        if (System.IO.File.Exists(CaminhoArquivoLog + fileName))
                            System.IO.File.Delete(CaminhoArquivoLog + fileName);

                        string[] InformacoesRemetente = new string[16] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                        InformacoesRemetente[0] = "0";//TIPO PESSOA - CNPJ/CPF - xNome - xFant - IE - email - cPais - UF - CEP - cMun - xMun - xBairro - xLgr - nro - xCpl - fone 
                        string[] InformacoesDestinatario = new string[16] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                        InformacoesDestinatario[0] = "1";// TIPO PESSOA - CNPJ/CPF -  xNome - xFant - IE - email - cPais - UF - CEP - cMun - xMun - xBairro - xLgr - nro - xCpl - fone 
                        string[] InformacoesTransportador = new string[16]{ "","","","","","","","","","","","","","","",""};
                        InformacoesTransportador[0] = "2";//TIPO PESSOA - CNPJ/CPF -  xNome - xFant - IE - email - cPais - UF - CEP - cMun - xMun - xBairro - xLgr - nro - xCpl - fone 

                        int CodigoItemNF = 1;
                        List<ProdutoDocumento> ListaItensXML = new List<ProdutoDocumento>();
                        List<ProdutoDocumento> ListaItensNaoCadastrados = new List<ProdutoDocumento>();
                        List<string[]> ListaInformacoesNaoCadastradas = new List<string[]>();

                        bool blnPassouNaTagPROD = false;
                        bool blnPassouNaTagDET = false;
                        bool blnPassouNaTagICMS = false;
                        bool blnPassouNaTagPIS = false;
                        bool blnPassouNaTagCOFINS = false;

                        foreach (DataTable thisTable in dataSet.Tables)
                        {
                            foreach (DataRow row in thisTable.Rows)
                            {
                                bool blnProdutoExiste = false;
                                
                                ProdutoDocumento item = new ProdutoDocumento();
                                item.CodigoItem = CodigoItemNF;

                                if (thisTable.TableName == "det")
                                {
                                    blnPassouNaTagDET = true;
                                }
                                else if (thisTable.TableName == "prod"  && blnPassouNaTagDET)
                                {
                                    CodigoItemNF++;
                                    foreach (DataColumn column in thisTable.Columns)
                                    {
                                        if (column.ColumnName == "cProd")
                                        {
                                            blnPassouNaTagPROD = true;
                                            
                                            item.CodigoSistemaAnterior = Convert.ToInt32(row[column].ToString());
                                            item.CodigoProduto = Convert.ToInt32(row[column].ToString());
                                            Produto p = new Produto();
                                            ProdutoDAL pDAL = new ProdutoDAL();

                                            p = pDAL.PesquisarProduto(item.CodigoProduto);
                                            if (p == null)
                                                blnProdutoExiste = false;
                                            else
                                            {
                                                item.CodigoProduto = Convert.ToInt32(item.CodigoProduto);
                                            }

                                            List<Produto> ListaProduto = new List<Produto>();
                                            ListaProduto = pDAL.ListarProdutos("CD_SIS_ANTERIOR", "NVARCHAR", item.CodigoSistemaAnterior.ToString(), "");
                                            if (ListaProduto.Count > 0)
                                            {
                                                blnProdutoExiste = true;
                                                item.CodigoProduto = Convert.ToInt32(ListaProduto[0].CodigoProduto);
                                            }
                                        }
                                        else if (column.ColumnName == "xProd" )
                                            item.Cpl_DscProduto = row[column].ToString();
                                        else if (column.ColumnName == "CFOP" )
                                            item.CodigoCFOP = Convert.ToDecimal(row[column].ToString());
                                        else if (column.ColumnName == "qCom" )
                                            item.Quantidade = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                        else if (column.ColumnName == "vUnCom")
                                            item.PrecoItem = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                        else if (column.ColumnName == "vProd")
                                            item.ValorTotalItem = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                        else if (column.ColumnName == "uCom")
                                            item.Unidade = row[column].ToString();
                                    }
                                }
                                else
                                {
                                    foreach (DataColumn column in thisTable.Columns)
                                    {
                                        //Emitente
                                        if (thisTable.TableName == "emit" && (column.ColumnName == "CNPJ" || column.ColumnName == "CPF"))
                                            InformacoesRemetente[1] = row[column].ToString();
                                        else if (thisTable.TableName == "emit" && column.ColumnName == "xNome")
                                            InformacoesRemetente[2] = row[column].ToString();
                                        else if (thisTable.TableName == "emit" && column.ColumnName == "xFant")
                                            InformacoesRemetente[3] = row[column].ToString();
                                        else if (thisTable.TableName == "emit" && column.ColumnName == "IE")
                                            InformacoesRemetente[4] = row[column].ToString();
                                        else if (thisTable.TableName == "emit" && column.ColumnName == "email")
                                            InformacoesRemetente[5] = row[column].ToString();
                                        else if (thisTable.TableName == "emit" && column.ColumnName == "CRT")
                                            ddlRegime.SelectedValue = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "cPais")
                                            InformacoesRemetente[6] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "UF")
                                            InformacoesRemetente[7] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "CEP")
                                            InformacoesRemetente[8] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "cMun")
                                            InformacoesRemetente[9] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "xMun")
                                            InformacoesRemetente[10] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "xBairro")
                                            InformacoesRemetente[11] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "xLgr")
                                            InformacoesRemetente[12] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "nro")
                                            InformacoesRemetente[13] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "xCpl")
                                            InformacoesRemetente[14] = row[column].ToString();
                                        else if (thisTable.TableName == "enderEmit" && column.ColumnName == "fone")
                                            InformacoesRemetente[15] = row[column].ToString();


                                        //Destinatario
                                        else if (thisTable.TableName == "dest" && (column.ColumnName == "CNPJ" || column.ColumnName == "CPF"))
                                            InformacoesDestinatario[1] = row[column].ToString();
                                        else if (thisTable.TableName == "dest" && column.ColumnName == "xNome")
                                            InformacoesDestinatario[2] = row[column].ToString();
                                        else if (thisTable.TableName == "dest" && column.ColumnName == "xFant")
                                            InformacoesDestinatario[3] = row[column].ToString();
                                        else if (thisTable.TableName == "dest" && column.ColumnName == "IE")
                                            InformacoesDestinatario[4] = row[column].ToString();
                                        else if (thisTable.TableName == "dest" && column.ColumnName == "email")
                                            InformacoesDestinatario[5] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "cPais")
                                            InformacoesDestinatario[6] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "UF")
                                            InformacoesDestinatario[7] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "CEP")
                                            InformacoesDestinatario[8] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "cMun")
                                            InformacoesDestinatario[9] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "xMun")
                                            InformacoesDestinatario[10] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "xBairro")
                                            InformacoesDestinatario[11] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "xLgr")
                                            InformacoesDestinatario[12] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "nro")
                                            InformacoesDestinatario[13] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "xCpl")
                                            InformacoesDestinatario[14] = row[column].ToString();
                                        else if (thisTable.TableName == "enderDest" && column.ColumnName == "fone")
                                            InformacoesDestinatario[15] = row[column].ToString();

                                        //Transportador
                                        else if (thisTable.TableName == "transporta" && (column.ColumnName == "CNPJ" || column.ColumnName == "CPF"))
                                            InformacoesTransportador[1] = row[column].ToString();
                                        else if (thisTable.TableName == "transporta" && column.ColumnName == "xNome")
                                            InformacoesTransportador[2] = row[column].ToString();
                                        else if (thisTable.TableName == "transporta" && column.ColumnName == "IE")
                                            InformacoesTransportador[4] = row[column].ToString();
                                        else if (thisTable.TableName == "transporta" && column.ColumnName == "xEnder")
                                            InformacoesTransportador[12] = row[column].ToString();
                                        else if (thisTable.TableName == "transporta" && column.ColumnName == "xMun")
                                            InformacoesTransportador[10] = row[column].ToString();
                                        else if (thisTable.TableName == "transporta" && column.ColumnName == "UF")
                                            InformacoesTransportador[7] = row[column].ToString();

                                        //outras informações
                                        else if (thisTable.TableName == "transp" && column.ColumnName == "modFrete")
                                            ddlModalidadeFrete.Text = row[column].ToString();
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vBC")
                                            txtBaseICMS.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vICMS")
                                            txtVlrICMS.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vBCST")
                                            txtVlrICMSST.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vST")
                                            txtVlrICMSST.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vFrete")
                                            txtFrete.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vDesc")
                                            txtDesconto.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vIPI")
                                            txtVlrIPI.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vNF")
                                            txtVlrTotal.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vProd")
                                            txtTotalItens.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ICMSTot" && column.ColumnName == "vOutro")
                                            txtDespesas.Text = row[column].ToString().Replace(".", ",");
                                        else if (thisTable.TableName == "ide" && column.ColumnName == "indPres")
                                            ddlIndicadorPresenca.SelectedValue = row[column].ToString();
                                        else if (thisTable.TableName == "ide" && column.ColumnName == "indFinal")
                                            ddlConsumidorFinal.SelectedValue = row[column].ToString();
                                        else if (thisTable.TableName == "ide" && column.ColumnName == "nNF")
                                            txtNroDocumento.Text = row[column].ToString();
                                        else if (thisTable.TableName == "ide" && column.ColumnName == "serie")
                                            txtNroSerie.Text = row[column].ToString();
                                        else if (thisTable.TableName == "ide" && column.ColumnName == "finNFe")
                                            ddlFinalidade.SelectedValue = row[column].ToString();
                                        else if (thisTable.TableName == "infProt" && column.ColumnName == "chNFe")
                                            txtChaveAcesso.Text = row[column].ToString();
                                        else if (thisTable.TableName == "ide" && column.ColumnName == "dhEmi")
                                        {
                                            txtdtemissao.Text = Convert.ToDateTime(row[column].ToString()).ToString("dd/MM/yyyy");
                                            txtHrEmissao.Text = Convert.ToDateTime(row[column].ToString()).ToString("HH:mm");
                                        }
                                        else if (thisTable.TableName == "ide" && column.ColumnName == "dhSaiEnt")
                                        {
                                            txtDtSaida.Text = Convert.ToDateTime(row[column].ToString()).ToString("dd/MM/yyyy");
                                            txtHrSaida.Text = Convert.ToDateTime(row[column].ToString()).ToString("HH:mm");
                                        }
                                        else if (thisTable.TableName == "ICMS")
                                        {
                                            blnPassouNaTagICMS = true;
                                        }
                                        else if (blnPassouNaTagICMS)
                                        {
                                            foreach (DataRow row2 in thisTable.Rows)
                                            {
                                                foreach (DataColumn column2 in thisTable.Columns)
                                                {
                                                    if (column2.ColumnName == "CSOSN")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "orig")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pCredSN")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vCredICMSSN")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "CST")
                                                       item.Impostos.CodigoCST_ICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pST")
                                                       item.Impostos.PercentualICMS_ST = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vICMSSubstituto")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vICMSSTRet")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pRedBCEfet")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vBCEfet")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pICMSEfet")
                                                       item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vICMSEfet")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "modBC")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vBC")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pICMS")
                                                        item.Impostos.PercentualICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vICMS")
                                                        item.Impostos.ValorICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "modBCST")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pMVAST")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pRedBCST")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vBCST")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pICMSST")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vICMSST")
                                                        item.Impostos.ValorBaseCalculoICMS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                }
                                            }
                                        }
                                        else if (thisTable.TableName == "PIS")
                                        {
                                            blnPassouNaTagPIS = true;
                                        }
                                        else if (blnPassouNaTagPIS)
                                        {
                                            foreach (DataRow row2 in thisTable.Rows)
                                            {
                                                foreach (DataColumn column2 in thisTable.Columns)
                                                {
                                                    if (column2.ColumnName == "vBC")
                                                        item.Impostos.ValorBaseCalculoPIS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pPIS")
                                                        item.Impostos.PercentualPIS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vPIS")
                                                        item.Impostos.ValorPIS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "CST")
                                                        item.Impostos.CodigoCST_PIS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                }
                                            }
                                        }
                                        else if (thisTable.TableName == "COFINS")
                                        {
                                            blnPassouNaTagCOFINS = true;
                                        }
                                        else if (blnPassouNaTagCOFINS)
                                        {
                                            foreach (DataRow row2 in thisTable.Rows)
                                            {
                                                foreach (DataColumn column2 in thisTable.Columns)
                                                {
                                                    if (column2.ColumnName == "vBC")
                                                        item.Impostos.ValorBaseCalculoCOFINS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "pCOFINS")
                                                        item.Impostos.PercentualCOFINS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "vCOFINS")
                                                        item.Impostos.ValorCOFINS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                    else if (column2.ColumnName == "CST")
                                                        item.Impostos.CodigoCST_COFINS = Convert.ToDecimal(row[column].ToString().Replace(".", ","));
                                                }
                                            }
                                        }
                                    }
                                }

                                if (blnPassouNaTagPROD)
                                {
                                    item.CodigoSituacao = 135;
                                    if (blnProdutoExiste)
                                    {
                                        ListaItensXML.Add(item);
                                        strResultadoXML += "<span class='glyphicon glyphicon-ok' style='color:green'></span> Produto - " + item.Cpl_DscProduto + " </br>";
                                    }
                                    else
                                    {
                                        ListaItensNaoCadastrados.Add(item);
                                        strResultadoXML += "<span class='glyphicon glyphicon-remove' style='color:red'></span> Produto - " + item.Cpl_DscProduto + " </br>";
                                    }
                                    
                                }
                            }
                            blnPassouNaTagPROD = false;
                        }

                        List<DBTabelaCampos> ListaFiltro = new List<DBTabelaCampos>();
                        PessoaDAL pessoaDAL = new PessoaDAL();

                        //PESQUISAR EMPRESA POR INSCRICAO - REMETENTE
                        DBTabelaCampos filtro0 = new DBTabelaCampos();
                        filtro0.Filtro = "NR_INSCRICAO";
                        filtro0.Fim = InformacoesRemetente[1];
                        filtro0.Inicio = InformacoesRemetente[1];
                        filtro0.Tipo = "NVARCHAR";
                        ListaFiltro.Add(filtro0);

                        List<Pessoa> pReme = new List<Pessoa>();
                        pReme = pessoaDAL.ListarPessoasCompleto(ListaFiltro, 0, 1);
                        if (pReme.Count > 0 && pReme[0].PessoaEmpresa == 1)
                        {
                            Empresa emp = new Empresa();
                            EmpresaDAL empDAL = new EmpresaDAL();
                            emp = empDAL.PesquisarEmpresaPessoa(pReme[0].CodigoPessoa);
                            ddlEmpresa.Text = emp.CodigoEmpresa.ToString();
                            ddlEmpresa_TextChanged(sender, e);
                        }
                        else
                        {
                            Session["MensagemTela"] = "Remetente no XML selecionado não existe cadastrado";
                            btnVoltar_Click(sender, e);
                        }

                        //PESQUISAR CLIENTE POR INSCRICAO - DESTINATARIO
                        ListaFiltro.Clear();
                        DBTabelaCampos filtro1 = new DBTabelaCampos();
                        filtro1.Filtro = "NR_INSCRICAO";
                        filtro1.Fim = InformacoesDestinatario[1];
                        filtro1.Inicio = InformacoesDestinatario[1]; 
                        filtro1.Tipo = "NVARCHAR";
                        ListaFiltro.Add(filtro1);

                        List<Pessoa> pDest = new List<Pessoa>();
                        pDest = pessoaDAL.ListarPessoasCompleto(ListaFiltro, 2, 1);
                        if(pDest.Count > 0)
                        {
                            txtCodPessoa.Text = pDest[0].CodigoPessoa.ToString();
                            txtCodPessoa_TextChanged(sender, e);
                            strResultadoXML += "<span class='glyphicon glyphicon-ok' style='color:green'></span> Destinatário - " + InformacoesDestinatario[2] + " </br>";
                        }
                        else
                        {
                            //CADASTRAR NOVA PESSOA
                            //CADASTRAR DESTINATARIO
                            ListaInformacoesNaoCadastradas.Add(InformacoesDestinatario);
                            strResultadoXML += "<span class='glyphicon glyphicon-remove' style='color:red'></span> Destinatário - " + InformacoesDestinatario[2] + "</br>";
                        }


                        //PESQUISAR TRANSPORTADOR POR INSCRICAO
                        ListaFiltro.Clear();
                        DBTabelaCampos filtro2 = new DBTabelaCampos();
                        filtro2.Filtro = "NR_INSCRICAO";
                        filtro2.Fim = InformacoesTransportador[1];
                        filtro2.Inicio = InformacoesTransportador[1];
                        filtro2.Tipo = "NVARCHAR";

                        ListaFiltro.Add(filtro2);
                        List<Pessoa> pTransp = new List<Pessoa>();
                        pTransp = pessoaDAL.ListarPessoasCompleto(ListaFiltro, 3, 1);
                        if (pTransp.Count > 0)
                        {
                            txtCodTransportador.Text = pTransp[0].CodigoPessoa.ToString();
                            txtCodTransportador_TextChanged(sender, e);
                            strResultadoXML += "<span class='glyphicon glyphicon-ok' style='color:green'></span> Transportador - " + InformacoesTransportador[2] + " </br>";
                        }
                        else
                        {
                            //CADASTRAR NOVA PESSOA
                            //CADASTRAR TRANSPORTADOR
                            ListaInformacoesNaoCadastradas.Add(InformacoesTransportador);
                            strResultadoXML += "<span class='glyphicon glyphicon-remove' style='color:red'></span> Transportador - " + InformacoesTransportador[2] + " </br>";
                        }
                        

                        MontarValorTotal(ListaItensXML);
                        Session["ListaItemNF"] = ListaItensXML;

                        grdProduto.DataSource = ListaItensXML;
                        grdProduto.DataBind();

                        if (ListaInformacoesNaoCadastradas.Count == 0 && ListaItensNaoCadastrados.Count == 0)
                            ShowMessage("Informações do XML importado, salve o documento!", MessageType.Info);
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "MostrarResultadoXML();", true);
                            lblResultadoXML.Text = strResultadoXML;
                            Session["NF-InformacoesParaCadastrar"] = ListaInformacoesNaoCadastradas;
                            Session["NF-ItensParaCadastrar"] = ListaItensNaoCadastrados;
                        }
                        pnlChaveAcesso.Visible = true;
                        pnlImportacaoNF.Visible = false;
                    }
                    else
                    {
                        ShowMessage("Importação apenas com arquivo XML", MessageType.Info);
                    }
                }
                else
                {
                    ShowMessage("Selecione um arquivo XML para importação", MessageType.Info);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void IncluirNovaPessoa(string[] strInformacoesXML)
        {
            try
            {
                Pessoa pessoa = new Pessoa();
                PessoaDAL pDAL = new PessoaDAL();
                List<Pessoa_Inscricao> listaInscricoes = new List<Pessoa_Inscricao>();
                List<Pessoa_Endereco> listaEnderecos = new List<Pessoa_Endereco>();
                List<Pessoa_Contato> listaContatos = new List<Pessoa_Contato>();
                //INFOS -------> CNPJ - xNome - xFant - IE - email - cPais - UF - CEP - cMun - xMun - xBairro - xLgr - nro - xCpl - fone 
                //POSICAO------>   1      2       3     4      5       6     7     8     9     10        11      12    13     14     15

                pessoa.PessoaTransportador = 1;

                Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                pesIns._CodigoItem = 1;
                pesIns._NumeroInscricao = strInformacoesXML[1];
                pesIns._NumeroIERG = strInformacoesXML[4];
                pesIns._NumeroIM = "";
                pesIns._OBS = "";
                pesIns._DataDeAbertura = Convert.ToDateTime(DateTime.Now);

                if (pesIns._NumeroInscricao.Length > 12)
                    pesIns._TipoInscricao = 3;
                else
                    pesIns._TipoInscricao = 4;

                listaInscricoes.Add(pesIns);

                Pessoa_Contato pesCtt = new Pessoa_Contato();
                pesCtt._Mail1 = strInformacoesXML[5];
                pesCtt._NomeContato = "Principal";
                pesCtt._TipoContato = 10;
                pesCtt._Fone1 = strInformacoesXML[15];
                pesCtt._Fone2 = "";
                pesCtt._Fone3 = "";
                pesCtt._Mail2 = "";
                pesCtt._Mail3 = "";
                pesCtt._MailNFE = "";
                pesCtt._MailNFSE = "";
                pesCtt._CodigoItem = 1;
                pesCtt._EmailSenha = "";
                pesCtt._CodigoPais = 1058;
                listaContatos.Add(pesCtt);

                if (strInformacoesXML[0] == "1")
                {
                    Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                    pesEnd._CodigoCEP = Convert.ToInt64(strInformacoesXML[8]);
                    pesEnd._CodigoMunicipio = Convert.ToInt64(strInformacoesXML[9]);
                    pesEnd._DescricaoMunicipio = strInformacoesXML[10];

                    Municipio mun = new Municipio();
                    MunicipioDAL munDAL = new MunicipioDAL();
                    mun = munDAL.PesquisarMunicipio(Convert.ToInt32(strInformacoesXML[9]));

                    Estado est = new Estado();
                    EstadoDAL estDAL = new EstadoDAL();
                    est = estDAL.PesquisarEstado(Convert.ToInt32(mun.CodigoEstado));

                    Bairro bairro = new Bairro();
                    BairroDAL bairroDAL = new BairroDAL();
                    bairro = bairroDAL.PesquisarBairroDescricao(strInformacoesXML[11]);
                    if (bairro.CodigoBairro == 0)
                    {
                        bairro.DescricaoBairro = strInformacoesXML[11];

                        bairroDAL.Inserir(bairro);
                        bairro = bairroDAL.PesquisarBairroDescricao(strInformacoesXML[11]);
                    }

                    pesEnd._CodigoEstado = est.CodigoEstado;
                    pesEnd._DescricaoEstado = est.Sigla + " - " + est.DescricaoEstado;
                    pesEnd._CodigoBairro = bairro.CodigoBairro;
                    pesEnd._CodigoEstado = est.CodigoEstado;
                    pesEnd._DescricaoBairro = bairro.DescricaoBairro;
                    pesEnd._Logradouro = strInformacoesXML[12];
                    pesEnd._NumeroLogradouro = strInformacoesXML[13];
                    pesEnd._TipoEndereco = 5;
                    pesEnd._Complemento = strInformacoesXML[14];
                    pesEnd._CodigoInscricao = 1;
                    pesEnd._CodigoItem = 1;

                    CEP cep = new CEP();
                    CEPDAL cepDAL = new CEPDAL();
                    cep = cepDAL.PesquisarCEP(pesEnd._CodigoCEP);
                    if (cep == null)
                    {
                        CEP cep2 = new CEP();
                        cep2.CodigoCEP = pesEnd._CodigoCEP;
                        cep2.CodigoMunicipio = Convert.ToInt64(pesEnd._CodigoMunicipio);
                        cep2.Logradouro = pesEnd._Logradouro;
                        cep2.Complemento = "";
                        cep2.DescricaoBairro = pesEnd._DescricaoBairro;
                        cep2.CodigoBairro = bairro.CodigoBairro;
                        cep2.CodigoEstado = mun.CodigoEstado;
                        cep2.DescricaoEstado = est.Sigla + " - " + est.DescricaoEstado;
                        cep2.DescricaoMunicipio = mun.DescricaoMunicipio;
                        cepDAL.Inserir(cep2);

                    }
                    pessoa.PessoaCliente = 1;
                    listaEnderecos.Add(pesEnd);
                }
                else if (strInformacoesXML[0] == "2")
                {
                    Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                    pesEnd._CodigoCEP = 0;
                    pesEnd._DescricaoMunicipio = strInformacoesXML[10];

                    Municipio mun = new Municipio();
                    MunicipioDAL munDAL = new MunicipioDAL();
                    mun = munDAL.ObterMunicipio(strInformacoesXML[10], strInformacoesXML[7]);

                    Estado est = new Estado();
                    EstadoDAL estDAL = new EstadoDAL();
                    est = estDAL.PesquisarEstado(mun.CodigoEstado);

                    Bairro bairro = new Bairro();
                    BairroDAL bairroDAL = new BairroDAL();
                    bairro = bairroDAL.PesquisarBairroDescricao(strInformacoesXML[11]);

                    if (bairro.CodigoBairro == 0)
                    {
                        bairro.DescricaoBairro = strInformacoesXML[11];

                        bairroDAL.Inserir(bairro);
                        bairro = bairroDAL.PesquisarBairroDescricao(strInformacoesXML[11]);
                    }

                    CEP cep = new CEP();
                    CEPDAL cepDAL = new CEPDAL();
                    cep = cepDAL.PesquisarCEP(pesEnd._CodigoCEP);
                    if (cep == null)
                    {

                        cep.CodigoCEP = pesEnd._CodigoCEP;
                        cep.CodigoMunicipio = Convert.ToInt64(pesEnd._CodigoMunicipio);
                        cep.Logradouro = pesEnd._Logradouro;
                        cep.Complemento = "";
                        cep.DescricaoBairro = pesEnd._DescricaoBairro;
                        cep.CodigoBairro = bairro.CodigoBairro;
                        cep.CodigoEstado = mun.CodigoEstado;
                        cep.DescricaoEstado = est.Sigla + " - " + est.DescricaoEstado;
                        cep.DescricaoMunicipio = mun.DescricaoMunicipio;
                        cepDAL.Inserir(cep);

                    }

                    pesEnd._CodigoMunicipio = mun.CodigoMunicipio;
                    pesEnd._CodigoEstado = est.CodigoEstado;
                    pesEnd._DescricaoEstado = est.Sigla + " - " + est.DescricaoEstado;
                    pesEnd._CodigoBairro = bairro.CodigoBairro;
                    pesEnd._CodigoEstado = est.CodigoEstado;
                    pesEnd._DescricaoBairro = bairro.DescricaoBairro;
                    pesEnd._Logradouro = strInformacoesXML[12];
                    pesEnd._NumeroLogradouro = "";
                    pesEnd._TipoEndereco = 5;
                    pesEnd._Complemento = "";
                    pesEnd._CodigoInscricao = 1;
                    pesEnd._CodigoItem = 1;

                    listaEnderecos.Add(pesEnd);
                    pessoa.PessoaTransportador = 1;
                }

                pessoa.NomePessoa = strInformacoesXML[2];
                pessoa.NomeFantasia = strInformacoesXML[2];
                pessoa.CodigoSituacaoPessoa = 1;
                pessoa.CodigoSituacaoFase = 15;
                pessoa.CodigoGrupoPessoa = 1;

                Int64 CodigoPessoa = 0;
                pDAL.Inserir(pessoa, listaInscricoes, listaEnderecos, listaContatos, ref CodigoPessoa);
               
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void IncluirNovoProduto(ProdutoDocumento pDoc)
        {
            try
            { 
                Produto p = new Produto();
                ProdutoDAL pDAL = new ProdutoDAL();
                p.CodigoSisAnterior = pDoc.CodigoProduto.ToString();
                p.CodigoSituacao = 1;

                DataTable dtUnidade = new DataTable();
                UnidadeDAL uDAL = new UnidadeDAL();
                dtUnidade = uDAL.ObterUnidades("DS_UNIDADE", "NVARCHAR", pDoc.Unidade, "");
                if (dtUnidade.Rows.Count == 0)
                {
                    Unidade u = new Unidade();
                    u.DescricaoUnidade = pDoc.Unidade;
                    u.SiglaUnidade = pDoc.Unidade;
                    uDAL.Inserir(u);
                    dtUnidade = uDAL.ObterUnidades("DS_UNIDADE", "NVARCHAR", pDoc.Unidade, "");
                }
                p.CodigoUnidade = Convert.ToInt32(dtUnidade.Rows[0][0]);
                p.CodigoTipoProduto = 28;
                p.DescricaoProduto = pDoc.Cpl_DscProduto;
                p.DataCadastro = DateTime.Now;
                p.DataAtualizacao = DateTime.Now;
                p.CodigoCategoria = "";
                p.DsEmbalagem = "";
                p.LinkProduto = "";
                p.CodigoNCM = "";
                p.CodigoEX= "";

                pDAL.Inserir(p);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnSalvarItensModal_Click(object sender, EventArgs e)
        {
            try
            {
                List<ProdutoDocumento> ListaItensNaoCadastrados = new List<ProdutoDocumento>();
                List<ProdutoDocumento> ListaItensCadastrados = new List<ProdutoDocumento>();
                List<string[]> ListaInformacoesNaoCadastradas = new List<string[]>();
                if(Session["ListaItemNF"] != null)
                    ListaItensCadastrados = (List<ProdutoDocumento>)Session["ListaItemNF"];
                if (Session["NF-ItensParaCadastrar"] != null)
                    ListaItensNaoCadastrados = (List<ProdutoDocumento>)Session["NF-ItensParaCadastrar"];
                if (Session["NF-InformacoesParaCadastrar"] != null)
                    ListaInformacoesNaoCadastradas = (List<string[]>)Session["NF-InformacoesParaCadastrar"];


                //PARA INCLUIR OS PRODUTO NÃO EXISTENTES
                foreach (var item in ListaItensNaoCadastrados)
                {
                    IncluirNovoProduto(item);
                    ListaItensCadastrados.Add(item);
                }

                //PARA INCLUIR PESSOAS NÃO EXISTENTES
                foreach (var item in ListaInformacoesNaoCadastradas)
                {
                    IncluirNovaPessoa(item);

                    List<DBTabelaCampos> ListaFiltro = new List<DBTabelaCampos>();
                    PessoaDAL pessoaDAL = new PessoaDAL();

                    DBTabelaCampos filtro1 = new DBTabelaCampos();
                    filtro1.Filtro = "NR_INSCRICAO";
                    filtro1.Fim = item[1];
                    filtro1.Inicio = item[1];
                    filtro1.Tipo = "NVARCHAR";
                    ListaFiltro.Add(filtro1);

                    List<Pessoa> p = new List<Pessoa>();

                    if(item[0]=="1")
                        p = pessoaDAL.ListarPessoasCompleto(ListaFiltro, 2, 1);
                    else
                        p = pessoaDAL.ListarPessoasCompleto(ListaFiltro, 3, 1);

                    if (p.Count > 0)
                    {
                        if (item[0] == "1")
                        {
                            txtCodPessoa.Text = p[0].CodigoPessoa.ToString();
                            txtCodPessoa_TextChanged(sender, e);
                        }
                        else
                        {
                            txtCodTransportador.Text = p[0].CodigoPessoa.ToString();
                            txtCodTransportador_TextChanged(sender, e);
                        }
                    }
                }

                ShowMessage("Informações do XML importados com sucesso, salve o documento!", MessageType.Info);
                Session["NF-InformacoesParaCadastrar"] = null;
                Session["NF-ItensParaCadastrar"] = null;

                Session["ListaItemNF"] = ListaItensCadastrados;
                grdProduto.DataSource = ListaItensCadastrados;
                grdProduto.DataBind();
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
    }
}