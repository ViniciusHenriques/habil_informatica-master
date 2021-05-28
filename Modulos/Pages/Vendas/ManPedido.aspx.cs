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

namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class ManPedido : System.Web.UI.Page
    {
        public string Panels { get; set; }

        public string PanelLogs { get; set; } = "display:none";

        public string PanelSelect { get; set; }

        public string PanelInfoCliente { get; set; }

        List<Doc_CtaReceber> ListaCtaReceber = new List<Doc_CtaReceber>();

        List<ProdutoDocumento> ListaItemDocumento = new List<ProdutoDocumento>();

        List<EventoDocumento> ListaEvento = new List<EventoDocumento>();

        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();

        List<Doc_Pedido> ListaOutrosPedidos = new List<Doc_Pedido>();

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
            txtCodigo.Text = "Novo";
            DBTabelaDAL RnTab = new DBTabelaDAL();

            List<ParSistema> ListPar = new List<ParSistema>();
            ParSistemaDAL ParDAL = new ParSistemaDAL();
            if (Session["VW_Par_Sistema"] == null)
                Session["VW_Par_Sistema"] = ParDAL.ListarParSistemas("CD_EMPRESA", "INT", Session["CodEmpresa"].ToString(), "");

            
            ListPar = (List<ParSistema>)Session["VW_Par_Sistema"];

            DateTime Hoje = RnTab.ObterDataHoraServidor();
            txtdtemissao.Text = Hoje.ToString("dd/MM/yyyy HH:mm:ss");
            if (ListPar.Count > 0)
                txtdtValidade.Text = Hoje.AddDays(ListPar[0].DiasValidadeOrc).ToString("dd/MM/yyyy");
            txtCNPJCPFCredor.Text = "";
            txtRazSocial.Text = "";
            txtEndereco.Text = "";
            txtEstado.Text = "";
            txtCEP.Text = "";
            txtCidade.Text = "";
            txtBairro.Text = "";
            txtCodPessoa.Text = "";
            txtDescricao.Text = "";
            txtNroDocumento.Text = "";
            txtNroSerie.Text = "";

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

            VendedorDAL vendedor = new VendedorDAL();
            ddlVendedor.DataSource = vendedor.ListarVendedores("CD_SITUACAO", "INT", "1", "");
            ddlVendedor.DataTextField = "NomePessoa";
            ddlVendedor.DataValueField = "CodigoVendedor";
            ddlVendedor.DataBind();
            ddlVendedor.Items.Insert(0, ".....SELECIONE UM VENDEDOR.....");

            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.SituacaoPedido();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            ddlAplicacaoUso.DataSource = sd.TipoAplicacaoUso();
            ddlAplicacaoUso.DataTextField = "DescricaoTipo";
            ddlAplicacaoUso.DataValueField = "CodigoTipo";
            ddlAplicacaoUso.DataBind();

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
            ddlPagamento.Items.Insert(0, ".....SELECIONE CONDIÇÃO DE PAGAMENTO.....");

            TipoOperacaoDAL tpOP = new TipoOperacaoDAL();
            ddlTipoOperacao.DataSource = tpOP.ListarTipoOperacoes("CD_SITUACAO", "INT", "1", "").Where(x => x.CodTipoOperFiscal == 3).ToList(); 
            ddlTipoOperacao.DataTextField = "Cpl_ComboDescricaoTipoOperacao";
            ddlTipoOperacao.DataValueField = "CodigoTipoOperacao";
            ddlTipoOperacao.DataBind();
            ddlTipoOperacao.Items.Insert(0, ".....SELECIONE TIPO DE OPERAÇÃO.....");
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

            v.CampoValido("Data de Validade", txtdtValidade.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtdtValidade.Focus();
                }
                return false;
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

            if (ddlPagamento.SelectedValue == ".....SELECIONE CONDIÇÃO DE PAGAMENTO.....")
            {
                ShowMessage("Selecione uma Condição de pagamento", MessageType.Info);
                ddlPagamento.Focus();
                return false;
            }

            if (ddlVendedor.SelectedValue == ".....SELECIONE UM VENDEDOR.....")
            {
                ShowMessage("Selecione um vendedor!", MessageType.Info);
                ddlVendedor.Focus();
                return false;
            }
            if (ddlTipoOperacao.SelectedValue == ".....SELECIONE TIPO DE OPERAÇÃO.....")
            {
                ShowMessage("Selecione um tipo de operação", MessageType.Info);
                ddlTipoOperacao.Focus();
                return false;
            }
            if (ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList().Count == 0)
            {
                ShowMessage("Adicione itens ao pedido!", MessageType.Warning);
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

            if (Session["TabFocada"] != null)
            {
                if (Session["TabFocada"].ToString() == "home")
                    PanelSelect = "aba1";
                else if (Session["TabFocada"].ToString() == "consulta4")
                    PanelSelect = "aba7";
                else
                    PanelSelect = Session["TabFocada"].ToString();
            }
            else
            {
                PanelSelect = "aba1";
                Session["TabFocada"] = "aba1";
            }
            if (Convert.ToInt32(Request.QueryString["Cad"]) == 1)
                Session["IndicadorURL"] = Request.QueryString["Cad"];

            MontaTela(sender, e);
        }
        
        protected void MontaTela(object sender, EventArgs e)
        {
            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConPedido.aspx");
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
                if (Session["ZoomPedido2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomPedido"] != null)
                {
                    string s = Session["ZoomPedido"].ToString();
                    Session["ZoomPedido"] = null;

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
                                ddlVendedor.Enabled = false;

                                CarregaTiposSituacoes();

                                Doc_Pedido doc = new Doc_Pedido();
                                Doc_PedidoDAL docDAL = new Doc_PedidoDAL();

                                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));
                                if (doc == null)
                                {
                                    Session["MensagemTela"] = "Este pedido não existe";
                                    btnVoltar_Click(sender, e);
                                }

                                txtdtValidade.Text = doc.DataValidade.ToString("dd/MM/yyyy");
                                txtdtemissao.Text = doc.DataHoraEmissao.ToString();
                                ddlEmpresa.SelectedValue = doc.CodigoEmpresa.ToString();
                                txtDescricao.Text = doc.DescricaoDocumento;
                                txtNroDocumento.Text = doc.NumeroDocumento.ToString();
                                txtNroSerie.Text = doc.DGSerieDocumento;
                                ddlSituacao.SelectedValue = doc.CodigoSituacao.ToString();
                                ddlTipoCobranca.SelectedValue = doc.CodigoTipoCobranca.ToString();
                                ddlPagamento.SelectedValue = doc.CodigoCondicaoPagamento.ToString();
                                //ddlTipoOrcamento.SelectedValue = doc.CodigoTipoOrcamento.ToString();
                                ddlVendedor.SelectedValue = doc.CodigoVendedor.ToString();
                                txtST.Text = doc.ValorST.ToString();
                                txtComissao.Text = doc.ValorComissao.ToString();
                                txtCubagem.Text = doc.ValorCubagem.ToString();
                                txtDescontoMedio.Text = doc.ValorDescontoMedio.ToString();
                                txtFrete.Text = doc.ValorFrete.ToString();
                                txtPeso.Text = doc.ValorPeso.ToString();
                                ddlAplicacaoUso.SelectedValue = doc.CodigoAplicacaoUso.ToString();

                                if(doc.NumeroWeb != 0)
                                    txtNroWeb.Text = doc.NumeroWeb.ToString();

                                if (doc.CodigoDocumentoOriginal != 0)
                                {  
                                    lblDsOrcamento.Text = " - Orçamento: ";
                                    lblCodOrcamento.Text = doc.CodigoDocumentoOriginal.ToString();
                                }

                                txtCodPessoa.Text = Convert.ToString(doc.Cpl_CodigoPessoa);
                                txtCodPessoa_TextChanged(sender, e);
                                ddlTipoOperacao.SelectedValue = doc.CodigoTipoOperacao.ToString();

                                txtCodTransportador.Text = doc.Cpl_CodigoTransportador.ToString();
                                txtCodTransportador_TextChanged(sender, e);

                                ddlEmpresa_TextChanged(sender, e);
                                PanelSelect = "aba1";
                                Session["TabFocada"] = "aba1";

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
                                Session["ListaItemDocumento"] = ListaItemDocumento;

                                List<BIConsumoClienteProduto> ListaBIConsumo = new List<BIConsumoClienteProduto>();
                                BIConsumoClienteProdutoDAL BIConsumoDAL = new BIConsumoClienteProdutoDAL();
                                ListaBIConsumo = BIConsumoDAL.ListarBIConsumoPessoaProduto("CD_PESSOA", "INT", doc.Cpl_CodigoPessoa.ToString(), "");
                                Session["ListaBIConsumoClienteProduto"] = ListaBIConsumo;

                                PanelInfoCliente = "display:block";
                            }
                        }
                    }
                }
                else
                {
                    PanelInfoCliente = "display:none";
                    LimpaTela();

                    ddlEmpresa_TextChanged(sender, e);
                    btnNovoAnexo.Visible = false;
                    btnCancelar.Visible = false;

                    if (Session["CodEmpresa"] != null)
                        ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();

                    Usuario usuario = new Usuario();
                    UsuarioDAL usuarioDAL = new UsuarioDAL();
                    usuario = usuarioDAL.PesquisarUsuario(Convert.ToInt64(Session["CodUsuario"]));

                    Vendedor vendedor = new Vendedor();
                    VendedorDAL vendedorDAL = new VendedorDAL();
                    vendedor = vendedorDAL.PesquisarVendedorPessoa(usuario.CodigoPessoa);

                    if (vendedor != null)
                    {
                        ddlVendedor.SelectedValue = vendedor.CodigoVendedor.ToString();
                        ddlVendedor.Enabled = false;
                    }
                    
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
            if (Session["Doc_Pedido"] != null)
            {
                CarregaTiposSituacoes();
                PreencheDados(sender, e);
            }
            if (Session["ZoomTranspPedido"] != null)
            {
                txtCodTransportador.Text = Session["ZoomTranspPedido"].ToString().Split('³')[0];
                txtCodTransportador_TextChanged(sender, e);
                Session["ZoomTranspPedido"] = null;
            }
            else if (Session["ZoomPessoaPedido"] != null)
            {
                txtCodPessoa.Text = Session["ZoomPessoaPedido"].ToString().Split('³')[0];
                txtCodPessoa_TextChanged(sender, e);
                Session["ZoomPessoaPedido"] = null;
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
            if (Session["ListaItemDocumento"] != null)
            {
                ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemDocumento"];
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
            if (Session["ListaOutrosPedidos"] != null)
            {
                ListaOutrosPedidos = (List<Doc_Pedido>)Session["ListaOutrosPedidos"];
                grdConsumo.DataSource = ListaOutrosPedidos;
                grdConsumo.DataBind();
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
                if (ddlSituacao.SelectedValue != "136" || Convert.ToDateTime(txtdtValidade.Text) < Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")))
                {
                    btnSalvar.Visible = false;
                    btnCancelar.Visible = false;
                    btnFaturarSemNF.Visible = false;
                }
                ddlAplicacaoUso.Enabled = false;
                ddlTipoOperacao.Enabled = false;
            }

            if(lblCodOrcamento.Text != "")
            {
                txtCodPessoa.Enabled = false;
                btnPessoa.Enabled = false;
                ddlEmpresa.Enabled = false;
                ddlAplicacaoUso.Enabled = false;
                ddlTipoOperacao.Enabled = false;
            }

            MontarValorTotal(ListaItemDocumento);

            if (!IsPostBack && ListaItemDocumento != null && ListaItemDocumento.Count != 0)
            {
                CalcularDescontoMedio();
                CalcularCubagem();
                CalcularPesoTotal();
                CalcularComissao();
            }

            if (Session["Doc_Orcamento"] != null)
            {
                btnVoltar_Click(sender, e);
            }

            List<ParSistema> ListPar = new List<ParSistema>();
            ParSistemaDAL ParDAL = new ParSistemaDAL();
            if (Session["VW_Par_Sistema"] == null)
                Session["VW_Par_Sistema"] = ParDAL.ListarParSistemas("CD_EMPRESA", "INT", Session["CodEmpresa"].ToString(), "");

            ListPar = (List<ParSistema>)Session["VW_Par_Sistema"];

            if (ListPar.Count > 0)
                if (ListPar[0].CodigoSequenciaGeracaoNFe != 209)
                    btnFaturarSemNF.Visible = false;
        }

        protected void PreencheDados(object sender, EventArgs e)
        {
            try
            { 
                Doc_Pedido doc = (Doc_Pedido)Session["Doc_Pedido"];
                ddlVendedor.SelectedValue = Convert.ToString(doc.CodigoVendedor);
                ddlEmpresa.SelectedValue = Convert.ToString(doc.CodigoEmpresa);
                ddlSituacao.SelectedValue = Convert.ToString(doc.CodigoSituacao);
                //ddlTipoOrcamento.SelectedValue = Convert.ToString(doc.CodigoTipoOrcamento);

                if(txtNroDocumento.Text == "")
                    txtNroDocumento.Text = Convert.ToString(doc.NumeroDocumento);

                txtDescricao.Text = doc.DescricaoDocumento;
                txtNroSerie.Text = Convert.ToString(doc.DGSerieDocumento);
                txtST.Text = doc.ValorST.ToString();
                txtComissao.Text = doc.ValorComissao.ToString();
                txtCubagem.Text = doc.ValorCubagem.ToString();
                txtDescontoMedio.Text = doc.ValorDescontoMedio.ToString("C");
                txtFrete.Text = doc.ValorFrete.ToString();
                txtPeso.Text = doc.ValorPeso.ToString();
                ddlAplicacaoUso.SelectedValue = doc.CodigoAplicacaoUso.ToString();

                if (doc.IsVendedor)
                    ddlVendedor.Enabled = false;
                if (doc.CodigoDocumento == 0)
                {
                    txtCodigo.Text = "Novo";
                    btnCancelar.Visible = false;
                }
                else
                {
                    txtCodigo.Text = Convert.ToString(doc.CodigoDocumento);
                    txtCodPessoa.Enabled = false;
                    btnPessoa.Enabled = false;
                    ddlEmpresa.Enabled = false;               
                }

                if (Convert.ToString(doc.DataValidade) != "01/01/0001 00:00:00")
                    txtdtValidade.Text = doc.DataValidade.ToString("dd/MM/yyyy");

                if (Convert.ToString(doc.DataHoraEmissao) != "01/01/0001 00:00:00")
                    txtdtemissao.Text = doc.DataHoraEmissao.ToString();

                if (doc.CodigoTipoOperacao != 0)
                    ddlTipoOperacao.SelectedValue = doc.CodigoTipoOperacao.ToString();

                if (doc.NumeroWeb != 0)
                    txtNroWeb.Text = doc.NumeroWeb.ToString();

                if (doc.CodigoDocumentoOriginal != 0)
                {
                    lblDsOrcamento.Text = " - Orçamento: ";
                    lblCodOrcamento.Text = doc.CodigoDocumentoOriginal.ToString();
                }

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


                if (doc.CodigoTipoCobranca != 0)
                    ddlTipoCobranca.SelectedValue = doc.CodigoTipoCobranca.ToString();

                if (doc.CodigoCondicaoPagamento != 0)
                    ddlPagamento.SelectedValue = doc.CodigoCondicaoPagamento.ToString();

                if (doc.CodigoVendedor != 0)
                    ddlVendedor.SelectedValue = doc.CodigoVendedor.ToString();

                Session["Doc_Pedido"] = null;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL doc = new Doc_PedidoDAL();
            doc.CancelarPedido(Convert.ToDecimal(txtCodigo.Text));

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            if (lblCodOrcamento.Text != "")
                doc.AtualizaOrcamento(Convert.ToDecimal(lblCodOrcamento.Text), Convert.ToInt32(Session["CodUsuario"]), Convert.ToInt32(he.CodigoEstacao));

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
            Session["ListaBIConsumoClienteProduto"] = null;

            if (Session["IndicadorURL"] != null && Session["IndicadorURL"].ToString() == "1")
                Response.Redirect("~/Pages/Vendas/libDocumento.aspx");

            Response.Redirect("~/Pages/Vendas/ConPedido.aspx");
            Session["IndicadorURL"] = null;
        }

        protected void btnSalvar_Click(object sender, EventArgs e, bool ContinuarAberto, bool FaturarPedido)
        {
            try
            {   
                if (ValidaCampos() == false)
                    return;

                Doc_Pedido p = new Doc_Pedido();
                Doc_PedidoDAL pe = new Doc_PedidoDAL();

                p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
                p.DGNumeroDocumento = "";
                p.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
                p.DGSerieDocumento = txtNroSerie.Text;
                p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                //p.CodigoTipoOrcamento = Convert.ToInt32(ddlTipoOrcamento.SelectedValue);
                p.DataValidade = Convert.ToDateTime(txtdtValidade.Text);
                p.DescricaoDocumento = txtDescricao.Text;
                p.CodigoCondicaoPagamento = Convert.ToInt32(ddlPagamento.SelectedValue);
                p.CodigoTipoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);
                p.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
                p.ValorTotal = Convert.ToDecimal(txtVlrTotal.Text);
                p.CodigoVendedor = Convert.ToInt64(ddlVendedor.SelectedValue);
                p.ValorST = Convert.ToDecimal(txtST.Text);
                p.ValorComissao = Convert.ToDecimal(txtComissao.Text);
                p.ValorCubagem = Convert.ToDecimal(txtCubagem.Text);
                p.ValorDescontoMedio = Convert.ToDecimal(txtDescontoMedio.Text.Substring(2));
                p.ValorFrete = Convert.ToDecimal(txtFrete.Text);
                p.ValorPeso = Convert.ToDecimal(txtPeso.Text);
                if (txtNroWeb.Text != "")
                    p.NumeroWeb = Convert.ToDecimal(txtNroWeb.Text);

                p.CodigoAplicacaoUso = Convert.ToInt32(ddlAplicacaoUso.SelectedValue);
                p.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
                if (lblCodOrcamento.Text != "")
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(lblCodOrcamento.Text);

                //COMPLEMENTARES
                p.Cpl_CodigoTransportador = Convert.ToInt64(txtCodTransportador.Text);
                p.Cpl_CodigoPessoa = Convert.ToInt64(txtCodPessoa.Text);

                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
                p.Cpl_Usuario = Convert.ToInt32(Session["CodUsuario"]);

                List<LiberacaoDocumento> ListaBloqueiosPedido = new List<LiberacaoDocumento>();
                
                List<ParSistema> ListPar = new List<ParSistema>();
                ParSistemaDAL ParDAL = new ParSistemaDAL();
                if (Session["VW_Par_Sistema"] == null)
                    Session["VW_Par_Sistema"] = ParDAL.ListarParSistemas("CD_EMPRESA", "INT", Session["CodEmpresa"].ToString(), "");

                ListPar = (List<ParSistema>)Session["VW_Par_Sistema"];

                if (txtCodigo.Text == "Novo")
                {
                    ddlEmpresa_TextChanged(sender, e);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Session["CodigoGeradorSequencial"]);
                    p.Cpl_NomeTabela = Session["NomeTabela"].ToString();

                    Session["CodigoGeradorSequencial"] = null;
                    decimal CodigoDocumento = 0;

                    pe.Inserir(p, ListaItemDocumento, EventoDocumento(), ListaAnexo, ref CodigoDocumento, ref ListaBloqueiosPedido, ContinuarAberto, ListPar[0].CriticaRegras, FaturarPedido);
                    p.CodigoDocumento = CodigoDocumento;
                    Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
                    
                }
                else
                {
                    Doc_Pedido p2 = new Doc_Pedido();
                    p.CodigoDocumento = Convert.ToInt64(txtCodigo.Text);

                    p2 = pe.PesquisarDocumento(Convert.ToDecimal(txtCodigo.Text));

                    if (Convert.ToInt32(ddlSituacao.SelectedValue) != p2.CodigoSituacao)
                        pe.Atualizar(p, ListaItemDocumento, EventoDocumento(), ListaAnexo, ref ListaBloqueiosPedido, ContinuarAberto,ListPar[0].CriticaRegras,FaturarPedido);
                    else
                        pe.Atualizar(p, ListaItemDocumento, null, ListaAnexo, ref ListaBloqueiosPedido, ContinuarAberto, ListPar[0].CriticaRegras, FaturarPedido);

                    Session["MensagemTela"] = "Registro alterado com Sucesso!!!";
                }

                foreach (var bloq in ListaBloqueiosPedido)
                {
                    if (bloq.CodigoBloqueio == 1)
                        Session["MensagemTela"] += "</br> Bloqueio de Crédido excedido " + bloq.Cpl_DescricaoBloqueio ;
                    else if (bloq.CodigoBloqueio == 2)
                        Session["MensagemTela"] += "</br> Bloqueio de Cliente devedor " + bloq.Cpl_DescricaoBloqueio;
                    else if (bloq.CodigoBloqueio == 3)
                        Session["MensagemTela"] += "</br> Bloqueio Primeira compra do cliente " + bloq.Cpl_DescricaoBloqueio;
                    else if (bloq.CodigoBloqueio == 4)
                        Session["MensagemTela"] += "</br> Bloqueio de Frete Obrigatório " + bloq.Cpl_DescricaoBloqueio;
                }

                //-------------Trazer filtrado registros do cliente----------------
                List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
                if (Session["LST_PEDIDO"] != null)
                    listaT = (List<DBTabelaCampos>)Session["LST_PEDIDO"];

                DBTabelaCampos rowp2 = new DBTabelaCampos();
                rowp2.Filtro = "CD_PESSOA";
                rowp2.Inicio = txtCodPessoa.Text;
                rowp2.Fim = txtCodPessoa.Text;
                rowp2.Tipo = "INT";
                listaT.Add(rowp2);

                listaT.RemoveAll(x => x.Filtro == "CD_SITUACAO");

                DBTabelaCampos rowp3 = new DBTabelaCampos();
                rowp3.Filtro = "CD_SITUACAO";
                rowp3.Inicio = "0";
                rowp3.Fim = "0";
                rowp3.Tipo = "INT";
                listaT.Add(rowp3);

                Session["LST_PEDIDO"] = listaT;
                //-------------------------------------------------------------------

                if (p.CodigoDocumentoOriginal != 0)
                    pe.AtualizaOrcamento(p.CodigoDocumentoOriginal, p.Cpl_Usuario, p.Cpl_Maquina);
                

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

                Doc_Pedido doc = new Doc_Pedido();

                doc.CodigoSituacao = Convert.ToInt32(ddlSituacao.Text);
                doc.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.Text);
                //doc.CodigoTipoOrcamento = Convert.ToInt32(ddlTipoOrcamento.Text);
                doc.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
                doc.DGSerieDocumento = txtNroSerie.Text;
                doc.DescricaoDocumento = txtDescricao.Text;
                doc.ValorST = Convert.ToDecimal(txtST.Text);
                doc.ValorComissao = Convert.ToDecimal(txtComissao.Text);
                doc.ValorCubagem = Convert.ToDecimal(txtCubagem.Text);
                doc.ValorDescontoMedio = Convert.ToDecimal(txtDescontoMedio.Text.Substring(2));
                doc.ValorFrete = Convert.ToDecimal(txtFrete.Text);
                doc.ValorPeso = Convert.ToDecimal(txtPeso.Text);
                doc.CodigoAplicacaoUso = Convert.ToInt32(ddlAplicacaoUso.SelectedValue);

                if (ddlVendedor.Enabled)
                    doc.IsVendedor = false;

                if (txtCodigo.Text != "Novo")
                    doc.CodigoDocumento = Convert.ToDecimal(txtCodigo.Text);

                if (txtdtemissao.Text != "")
                    doc.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);

                if (txtdtValidade.Text != "")
                    doc.DataValidade = Convert.ToDateTime(txtdtValidade.Text);

                if (txtNroWeb.Text != "")
                    doc.NumeroWeb = Convert.ToDecimal(txtNroWeb.Text);

                if (lblCodOrcamento.Text != "")
                    doc.CodigoDocumentoOriginal = Convert.ToDecimal(lblCodOrcamento.Text);

                if (txtCodPessoa.Text != "")
                    doc.Cpl_CodigoPessoa = Convert.ToInt64(txtCodPessoa.Text);

                if (txtCodTransportador.Text != "")
                    doc.Cpl_CodigoTransportador = Convert.ToInt64(txtCodTransportador.Text);

                if (ddlTipoCobranca.SelectedValue != ".....SELECIONE UM TIPO DE COBRANÇA.....")
                    doc.CodigoTipoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);

                if (ddlTipoOperacao.SelectedValue != ".....SELECIONE TIPO DE OPERAÇÃO.....")
                    doc.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);

                if (ddlPagamento.SelectedValue != ".....SELECIONE CONDIÇÃO DE PAGAMENTO.....")
                    doc.CodigoCondicaoPagamento = Convert.ToInt32(ddlPagamento.SelectedValue);

                if (ddlVendedor.SelectedValue != ".....SELECIONE UM VENDEDOR.....")
                    doc.CodigoVendedor = Convert.ToInt64(ddlVendedor.SelectedValue);

                Session["Doc_Pedido"] = doc;
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
                PanelInfoCliente = "display:none";
                txtPessoa.Text = "";
                return;
            }
            else
            {
                v.CampoValido("Codigo Cliente", txtCodPessoa.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodPessoa.Text = "";
                    PanelInfoCliente = "display:none";
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
                txtRazSocial.Text = "";
                txtEndereco.Text = "";
                txtEstado.Text = "";
                txtCEP.Text = "";
                txtEmail.Text = "";
                txtCodPessoa.Focus();
                PanelInfoCliente = "display:none";
                return;
            }
            txtRazSocial.Text = p2.NomePessoa;
            txtPessoa.Text = p2.NomePessoa;
            lblNomeCliente.Text = " - " + p2.NomePessoa;
            txtCredito.Text = p2.ValorLimiteCredito.ToString();


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
            txtEmail.Text = ctt._Mail1;

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

            Doc_PedidoDAL docDAL = new Doc_PedidoDAL();
            ListaCtaReceber = docDAL.ListarContasAbertasCliente(codigoPessoa);
            Session["ListaContaReceber"] = ListaCtaReceber;
            grdFinanceiro.DataSource = ListaCtaReceber;
            grdFinanceiro.DataBind();

            decimal CodigoDocumentoAtual = 0;
            if (txtCodigo.Text == "Novo")
            {
                if(p2.CodigoCondPagamento != 0)
                    ddlPagamento.SelectedValue = p2.CodigoCondPagamento.ToString();
                if(p2.CodigoTipoOperacao != 0)
                    ddlTipoOperacao.SelectedValue = p2.CodigoTipoOperacao.ToString();
                if (p2.CodigoTipoCobranca != 0)
                    ddlTipoCobranca.SelectedValue = p2.CodigoTipoCobranca.ToString();
                List<BIConsumoClienteProduto> ListaBIConsumo = new List<BIConsumoClienteProduto>();
                BIConsumoClienteProdutoDAL BIConsumoDAL = new BIConsumoClienteProdutoDAL();

                ListaBIConsumo = BIConsumoDAL.ListarBIConsumoPessoaProduto("CD_PESSOA", "INT", codigoPessoa.ToString(), "");
                Session["ListaBIConsumoClienteProduto"] = ListaBIConsumo;
            }
            else
            {
                Doc_Pedido doc = new Doc_Pedido();
                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodigo.Text));

                if (doc.Cpl_CodigoPessoa != Convert.ToInt32(txtCodPessoa.Text))
                    ShowMessage("Você alterou o Pessoa do Documento!", MessageType.Info);

                LiberacaoDocumentoDAL libDAL = new LiberacaoDocumentoDAL();
                grdLiberacoes.DataSource = libDAL.ListarLiberacao("LIB.CD_DOCUMENTO", "INT", txtCodigo.Text, "");
                grdLiberacoes.DataBind();


            }
            if (txtCodTransportador.Text == "" && p2.CodigoTransportador != 0)
            {
                txtCodTransportador.Text = p2.CodigoTransportador.ToString();
                txtCodTransportador_TextChanged(sender, e);
            }

            ListaOutrosPedidos = docDAL.ListarPedidoPessoa(codigoPessoa, CodigoDocumentoAtual);
            Session["ListaOutrosPedidos"] = ListaOutrosPedidos;
            grdConsumo.DataSource = ListaOutrosPedidos;
            grdConsumo.DataBind();

            PanelInfoCliente = "display:block";
            ddlPagamento.Focus();

            MontarGraficoCredito();
            Session["TabFocada"] = "aba1";
            Session["Doc_Pedido"] = null;
        }

        protected void ddlEmpresa_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "Novo")
                return;

            DBTabelaDAL db = new DBTabelaDAL();
            List<GeracaoSequencialDocumento> ListaGerDoc = new List<GeracaoSequencialDocumento>();
            GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
            GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
            ListaGerDoc = gerDocDAL.ListarGeracaoSequencial("CD_TIPO_DOCUMENTO", "INT", "8", "VALIDADE DESC");

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
            Response.Redirect("~/Pages/financeiros/ManAnexo.aspx?cad=4");
        }

        protected void btnNovoAnexo_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;
            CompactaDocumento();
            Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=8&man=");
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
                        if (decDiferencaValorFreteRatiado != 0)
                        {
                            itens.Impostos = ImpostoProdutoDocumentoDAL.PreencherImpostosProdutoDocumento(itens, Convert.ToInt64(ddlEmpresa.SelectedValue), Convert.ToInt64(txtCodPessoa.Text), Convert.ToInt32(ddlTipoOperacao.SelectedValue), Convert.ToInt32(ddlAplicacaoUso.Text), Convert.ToDecimal(txtFrete.Text) - (decDiferencaValorFreteRatiado), false);
                            decDiferencaValorFreteRatiado = 0;
                        }
                        else
                            itens.Impostos = ImpostoProdutoDocumentoDAL.PreencherImpostosProdutoDocumento(itens, Convert.ToInt64(ddlEmpresa.SelectedValue), Convert.ToInt64(txtCodPessoa.Text), Convert.ToInt32(ddlTipoOperacao.SelectedValue.Replace(".....SELECIONE TIPO DE OPERAÇÃO.....", "0")), Convert.ToInt32(ddlAplicacaoUso.Text), Convert.ToDecimal(txtFrete.Text), false);
                    }
                    ValorTotal += itens.ValorTotalItem;// + itens.Impostos.ValorICMS + itens.Impostos.ValorPIS + itens.Impostos.ValorCOFINS + itens.Impostos.ValorIPI;
                }
                NovaListaItens.Add(itens);
            }
            lblTotalItens.Text = ValorTotal.ToString("C");
            txtVlrTotal.Text = (ValorTotal + Convert.ToDecimal(txtFrete.Text)).ToString("F");

        }

        [WebMethod]
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            bool blnCampoValido = true;
            v.CampoValido("Código Cliente", txtCodPessoa.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage("Necessário informar o cliente para inserir produto(s)!!", MessageType.Warning);
                    txtCodPessoa.Focus();
                    return;
                }
            }
            if (ddlTipoOperacao.SelectedValue == ".....SELECIONE TIPO DE OPERAÇÃO.....")
            {
                ShowMessage("Selecione um tipo de operação", MessageType.Info);
                ddlTipoOperacao.Focus();
                return;
            }
            CompactaDocumento();
            Response.Redirect("~/Pages/Vendas/ManItemOrcamento.aspx?cad=2");
        }

        protected void grdItens_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdProduto.PageIndex = e.NewPageIndex;
            // Carrega os dados
            if (Session["ListaItemDocumento"] != null)
            {
                ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemDocumento"];
                grdProduto.DataSource = ListaItemDocumento;
                grdProduto.DataBind();

            }
        }

        protected void MontarGraficoCredito()
        {
            try
            {
                decimal ValorDecimal = 0;
                if (!decimal.TryParse(txtCredito.Text, out ValorDecimal))
                    return;

                decimal ValorCreditoPessoa = Convert.ToDecimal(txtCredito.Text);
                decimal ValorDisponivel = 0;
                decimal ValorUsado = 0;

                decimal ValorOrcamento = Convert.ToDecimal(txtVlrTotal.Text);

                decimal PercentualUsadoGrafico;
                decimal PercentualLivreGrafico;

                PessoaDAL pDAL = new PessoaDAL();
                decimal ValorEmPedidos = 0;
                decimal ValorLimiteCredito = 0; 
                ValorUsado += pDAL.VerificaCreditoUsadoCliente(Convert.ToDecimal(txtCodPessoa.Text), ref ValorEmPedidos, ref ValorLimiteCredito) + ValorEmPedidos;
                ValorDisponivel = ValorCreditoPessoa - (ValorUsado + ValorEmPedidos);

                if (ValorDisponivel > 0)
                {
                    PercentualUsadoGrafico = ((ValorUsado) / ValorCreditoPessoa) * 100;
                    PercentualLivreGrafico = 100 - PercentualUsadoGrafico;
                }
                else
                {
                    PercentualUsadoGrafico = 100;
                    PercentualLivreGrafico = 0;
                    //ShowMessage("Você estourou seu limite!", MessageType.Info);
                }

                txtCredito.Text = ValorCreditoPessoa.ToString("F");
                txtVlrUsado.Text = ValorUsado.ToString("F");
                txtVlrDisponivel.Text = ValorDisponivel.ToString("F");
                txtPedidos.Text = ValorEmPedidos.ToString("F");

                string function = "MontaGraficoCredito(" + PercentualLivreGrafico.ToString().Replace(',', '.') + "," + PercentualUsadoGrafico.ToString().Replace(',', '.') + ")";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", function, true);

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
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
                txtCodTransportador.Focus();
                return;
            }

            txtTransportador.Text = p2.NomePessoa;
            Session["TabFocada"] = null;
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
                    txtDescontoMedio.Focus();
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

        protected void CalcularDescontoMedio()
        {
            Produto p = new Produto();
            ProdutoDAL pDAL = new ProdutoDAL();
            if (ListaItemDocumento != null && ListaItemDocumento.Count != 0)
            {
                decimal ValorDescontoMedio = 0;
                decimal PercentualDescontoMedio = 0;
                for (int i = 0; i < ListaItemDocumento.Count; i++)
                {
                    if (ListaItemDocumento[i].CodigoSituacao != 134)
                    {
                        p = pDAL.PesquisarProduto(ListaItemDocumento[i].CodigoProduto);

                        decimal DescontoItem = 100 - (((ListaItemDocumento[i].Quantidade * ListaItemDocumento[i].PrecoItem) / (ListaItemDocumento[i].Quantidade * Convert.ToDecimal(p.ValorVenda))) * 100);

                        ValorDescontoMedio += DescontoItem / 100;
                        PercentualDescontoMedio = (ValorDescontoMedio / Convert.ToDecimal(p.ValorVenda)) * 100;
                    }
                }
                txtDescontoMedio.Text = ValorDescontoMedio.ToString("C");
                txtPctDescontoMedio.Text = PercentualDescontoMedio.ToString("F");
            }
        }

        protected void grdConsumo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            List<ProdutoDocumento> ListaItemOutrosOrcamentos = new List<ProdutoDocumento>();

            string x = e.CommandName;

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = grdConsumo.Rows[index];
            string Codigo = Server.HtmlDecode(row.Cells[0].Text);

            string t = row.Cells[0].ToString();
            LinkButton btnMostrarItens = (LinkButton)row.FindControl("btnMostrarItens");


            if (x == "BtnMostrarItens")
            {
                ProdutoDocumentoDAL itensDAL = new ProdutoDocumentoDAL();
                ListaItemOutrosOrcamentos = itensDAL.ObterItemOrcamentoPedido(Convert.ToDecimal(Codigo));

                grdItemOutrosOrcamentos.DataSource = ListaItemOutrosOrcamentos.Where(Y => Y.CodigoSituacao != 134).ToList();
                grdItemOutrosOrcamentos.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "MostrarItens('Item(ns) do Pedido " + Codigo + "');", true);
            }
        }

        protected void grdConsumo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            btnSalvar_Click(sender, e, true,false);
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            btnSalvar_Click(sender, e, false,false);
        }

        protected void CalcularCubagem()
        {
            txtCubagem.Text = Convert.ToDecimal(ListaItemDocumento.Where(y => y.CodigoSituacao != 134).Sum(x => (x.ValorVolume * x.ValorFatorCubagem) * x.Quantidade).ToString()).ToString("#,####0.0000");
        }

        protected void CalcularPesoTotal()
        {
            txtPeso.Text = Convert.ToDecimal(ListaItemDocumento.Where(y => y.CodigoSituacao != 134).Sum(x => x.ValorPeso * x.Quantidade).ToString()).ToString("#,###0.000");
        }

        protected void CalcularComissao()
        {
            if (ddlVendedor.SelectedValue != ".....SELECIONE UM VENDEDOR.....")
            {
                Vendedor vendedor = new Vendedor();
                VendedorDAL vendedorDAL = new VendedorDAL();
                vendedor = vendedorDAL.PesquisarVendedor(Convert.ToInt32(ddlVendedor.SelectedValue));
                txtComissao.Text = (Convert.ToDecimal(txtVlrTotal.Text) * (vendedor.PercentualComissao / 100)).ToString("F");
            }
        }

        protected void btnConfirmarFaturaramentoSemNF_Click(object sender, EventArgs e)
        {

            ddlSituacao.SelectedValue = "150";
            btnSalvar.Visible = false;
            btnCancelar.Visible = false;
            btnSalvar_Click(sender, e,false,true);
            ShowMessage("Pedido baixado com sucesso!", MessageType.Success);
        }
    }
}