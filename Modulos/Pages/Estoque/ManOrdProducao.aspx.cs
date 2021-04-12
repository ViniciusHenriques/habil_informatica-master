using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;

namespace SoftHabilInformatica.Pages.Estoque
{
	public partial class ManOrdProducao : System.Web.UI.Page
	{
		List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
		List<ProdutoDocumento> ListaItemDocumento = new List<ProdutoDocumento>();
		List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
		List<Habil_Log> ListaLog = new List<Habil_Log>();

		public string Encerra { get; set; }
		public string Panels { get; set; }
		public string TabLogs { get; set; }
		public string PanelSelect { get; set; }
		public string PanelInfoCliente { get; set; }
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
			btnCancelar.Visible = false;
			DBTabelaDAL dbTDAL = new DBTabelaDAL();


			List<ParSistema> ListPar = new List<ParSistema>();
			ParSistemaDAL ParDAL = new ParSistemaDAL();
			if (Session["VW_Par_Sistema"] == null)
				Session["VW_Par_Sistema"] = ParDAL.ListarParSistemas("CD_EMPRESA", "INT", Session["CodEmpresa"].ToString(), "");

			ListPar = (List<ParSistema>)Session["VW_Par_Sistema"];


			txtdtemissao.Text = Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("dd/MM/yyyy"));
			txtdtEncerramento.Text = "";
			txtPrazo.Text = "0";
			txtPrazo_TextChanged(null, null);
			txtQtFinal.Text = "0,00";

			Panels = "display:none";

			ddlTipoOperacao.Focus();
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
			ddlSituacao.DataSource = sd.SituacaoOrdemDeProducao();
			ddlSituacao.DataTextField = "DescricaoTipo";
			ddlSituacao.DataValueField = "CodigoTipo";
			ddlSituacao.DataBind();

			TipoOperacaoDAL tpOP = new TipoOperacaoDAL();
			ddlTipoOperacao.DataSource = tpOP.ListarTpOperSaidaOrdemProducao();
			ddlTipoOperacao.DataTextField = "Cpl_ComboDescricaoTipoOperacao";
			ddlTipoOperacao.DataValueField = "CodigoTipoOperacao";
			ddlTipoOperacao.DataBind();
			ddlTipoOperacao.Items.Insert(0, ".....Selecione tipo de Operação.....");

			Habil_TipoDAL uso = new Habil_TipoDAL();
			ddlAppUsoProducao.DataSource = uso.SituacaoAplicaUsoOrdemProducao();
			ddlAppUsoProducao.DataTextField = "DescricaoTipo";
			ddlAppUsoProducao.DataValueField = "CodigoTipo";
			ddlAppUsoProducao.DataBind();

			Doc_OrdProducaoDAL RnPes = new Doc_OrdProducaoDAL();
			ddlOperador.DataSource = RnPes.ListarUsuarios();
			ddlOperador.DataTextField = "NomePessoa";
			ddlOperador.DataValueField = "CodigoPessoa";
			ddlOperador.DataBind();

			LocalizacaoDAL lc = new LocalizacaoDAL();
			ddlLocalizacao.Items.Clear();
			ddlLocalizacao.DataSource = lc.ListarLocalizacaoProducao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
			ddlLocalizacao.DataTextField = "CodigoLocalizacao";
			ddlLocalizacao.DataValueField = "CodigoIndice";
			ddlLocalizacao.SelectedValue = null;
			ddlLocalizacao.DataBind();
			ddlLocalizacao.Items.Insert(0, " * Nenhum Selecionado * ");
			ddlLocalizacao.Enabled = true;

			LocalizacaoDAL ld = new LocalizacaoDAL();
			ddllocalizacaoEncerramento.Items.Clear();
			ddllocalizacaoEncerramento.DataSource = ld.ListarLocalizacaoProducao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
			ddllocalizacaoEncerramento.DataTextField = "CodigoLocalizacao";
			ddllocalizacaoEncerramento.DataValueField = "CodigoIndice";
			ddllocalizacaoEncerramento.SelectedValue = null;
			ddllocalizacaoEncerramento.DataBind();
			ddllocalizacaoEncerramento.Items.Insert(0, " * Nenhum Selecionado * ");
			ddllocalizacaoEncerramento.Enabled = true;
		}
		protected Boolean ValidaCampos()
		{
			if (txtCodPessoa.Text == "")
			{
				ShowMessage("Código da Pessoa deve Existir", MessageType.Info);
				return false;
			}
			return true;
		}
		protected Boolean ValidaCamposSitAberta()
		{
			Boolean blnCampoValido = false;

			if (ddlTipoOperacao.SelectedValue == ".....Selecione tipo de Operação.....")
			{
				ShowMessage("Selecione um Tipo de Operação", MessageType.Info);
				ddlTipoOperacao.Focus();
				return false;
			}
			v.CampoValido("Código do Cliente", txtCodPessoa.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
			if (!blnCampoValido)
			{
				if (strMensagemR != "")
				{
					ShowMessage(strMensagemR, MessageType.Info);
					txtCodPessoa.Focus();
				}
				return false;
			}
			if (Convert.ToInt32(ddlAppUsoProducao.SelectedValue) == 174)
			{
				if (txtDocOriginal.Text == "0" || txtDocOriginal.Text == "")
				{
					ShowMessage("Uso da Aplicação " + ddlAppUsoProducao.SelectedItem.Text.ToLower() + " deve Conter um Pedido", MessageType.Info);
					return false;
				}

			}
			v.CampoValido("Código do Composto", txtComposto.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
			if (!blnCampoValido)
			{
				if (strMensagemR != "")
				{
					ShowMessage(strMensagemR, MessageType.Info);
					txtComposto.Focus();
				}
				return false;
			}
			v.CampoValido("Quantidade a produzir", txtQtPraProduzir.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
			if (!blnCampoValido)
			{
				if (strMensagemR != "")
				{
					ShowMessage(strMensagemR, MessageType.Info);
					txtQtPraProduzir.Focus();
				}
				return false;
			}
			if (Convert.ToDecimal(txtQtPraProduzir.Text) < 0)
			{
				ShowMessage("Quantidade Deve ser Maior que Zero!", MessageType.Info);
				return false;
			}
			v.CampoValido("Prazo", txtPrazo.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
			if (!blnCampoValido)
			{
				if (strMensagemR != "")
				{
					ShowMessage(strMensagemR, MessageType.Info);
					txtPrazo.Focus();
				}
				return false;
			}
			if (GridComponentes.Rows.Count < 0)
			{
				ShowMessage("Ordem de Produção Deve Conter Pelo menos um Componente", MessageType.Info);
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
				else
					PanelSelect = Session["TabFocada"].ToString();
			}
			else
			{
				PanelSelect = "aba1";
				Session["TabFocada"] = "aba1";
			}
			MontaTela(sender, e);
		} 
		protected void MontaTela(object sender, EventArgs e)
		{
			List<Permissao> lista = new List<Permissao>();
			PermissaoDAL r1 = new PermissaoDAL();
			lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
												Convert.ToInt32(Session["CodPflUsuario"].ToString()),
												"ConOrdProducao.aspx");
			lista.ForEach(delegate (Permissao x)
			{
				if (!x.AcessoCompleto)
				{
					if (!x.AcessoAlterar)
					{
						btnSalvar.Visible = false;
						//btnGerarPedido.Visible = false;
						//btnBaixarSV.Visible = false;
						//btnBaixarParcial.Visible = false;
					}
				}
			});

			if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
			{
				if (Session["ZoomOrdProducao2"] == null)
					Session["Pagina"] = Request.CurrentExecutionFilePath;

				if (Session["ZoomOrdProducao"] != null)
				{
					string s = Session["ZoomOrdProducao"].ToString();
					Session["ZoomOrdProducao"] = null;

					string[] words = s.Split('³');
					if (s != "³")
					{
						foreach (string word in words)
						{
							if (word != "")
							{
								txtCodigo.Text = word;
								txtCodigo.Enabled = false;
								txtComposto.Enabled = false;
								btnPesquisarComposto.Enabled = false;
								btnDoc.Enabled = false;
								CarregaTiposSituacoes();

								PanelSelect = "aba1";
								Session["TabFocada"] = "aba1";

								Session["Atualizar"] = "";

								ProdutoDocumentoDAL RnPd = new ProdutoDocumentoDAL();
								ItemDaComposicaoDAL RnItem = new ItemDaComposicaoDAL();
								List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
								Doc_OrdProducao p = new Doc_OrdProducao();
								Doc_OrdProducaoDAL docDAL = new Doc_OrdProducaoDAL();

								p = docDAL.PesquisarOrdem(Convert.ToInt32(txtCodigo.Text));
								if (p == null)
								{
									Session["MensagemTela"] = "Este orçamento não existe";
									btnVoltar_Click(sender, e);
								}

								ddlTipoOperacao.SelectedValue = p.CodigoTipoOperacao.ToString();
								ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
								ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();

								Panels = "display:block";

								txtNroDocumento.Text = p.NumeroDocumento.ToString();
								txtSerie.Text = "";
								txtDocOriginal.Text = p.CodigoDocumentoOriginal.ToString();
								txtdtemissao.Text = p.DataHoraEmissao.ToString();
								txtdtEncerramento.Text = p.DataEncerramento.ToString();
								txtCodPessoa.Text = Convert.ToString(p.Cpl_CodigoPessoa);
								ddlAppUsoProducao.SelectedValue = p.CodigoAplicacaoUso.ToString();
								ddlOperador.SelectedValue = p.CodigoOperador.ToString();
								txtComposto.Text = p.CodigoComposto.ToString();
								txtQtPraProduzir.Text = p.QtProduzir.ToString();
								txtQtJaProduzida.Text = p.QtProduzida.ToString();
								txtQtFinal.Text = p.QtProduzida.ToString();
								txtFormato.Text = p.formato;
								txtLogo.Text = p.LogoMarca;
								txtPrazo.Text = p.Prazo.ToString();
								txtMaquina.Text = p.Maquina;
								txtObsProducao.Text = p.DescricaoDocumento;
								txtQtFinal.Text = p.QtProduzida.ToString();

								txtComposto_TextChanged(sender, e);
								txtCodPessoa_TextChanged(sender, e);
								QtFinal_TextChanged(sender, e);

								ListaProdutos = RnPd.ListarProdutosDaOrdemDeProducao(Convert.ToDecimal(txtCodigo.Text));

								List<ItemDaComposicao> ListaItem = new List<ItemDaComposicao>();

								foreach (ProdutoDocumento pr in ListaProdutos)
								{
									ItemDaComposicao ep = new ItemDaComposicao();
									ItemDaComposicao ap = new ItemDaComposicao();

									ep.CodigoRoteiro = pr.Cpl_CodigoRoteiro;
									ep.DescRoteiro = pr.Cpl_DescRoteiro;
									ep.CodigoComponente = pr.CodigoProduto;
									ep.DescricaoComponente = pr.Cpl_DscProduto;
									ep.Unidade = pr.Unidade;
									ep.PerQuebraComponente = pr.PerQuebraComponente;
									ep.QuantidadeComponente = pr.Quantidade;
									ep.QuantidadeUtil = pr.QuantidadeAtendida;
									//ep.QuantidadeAdd = pr.QuantidadeAtendida;
									ep.ValorCustoComponente = pr.PrecoItem;
									ep.ValorTotal = pr.ValorTotalItem;
									if (pr.LocalizacoaProducao != 0)
									{
										ddllocalizacaoEncerramento.SelectedValue = pr.LocalizacoaProducao.ToString();
										ddlLocalizacao.SelectedValue = pr.LocalizacoaProducao.ToString();
									}
									ListaItem.Add(ep);
								}
															
								GridComponentes.DataSource = ListaItem;
								GridComponentes.DataBind();

								List<ItemDaComposicao> ListaRoteiro = new List<ItemDaComposicao>();

								ListaRoteiro = RnItem.ListarRoteirosComDistinctParaDocumento(Convert.ToDecimal(txtCodigo.Text));

								Session["ListaRoteiro"] = ListaRoteiro;
								GridRoteiros.DataSource = ListaRoteiro;
								GridRoteiros.DataBind();

								ddlEmpresa_TextChanged(sender, e);

								txtValor.Text = ListaItem.Sum(x => x.ValorTotal).ToString("###,##0.00");
								txtPrazo_TextChanged(sender, e);

								PanelSelect = "aba1";
								Session["TabFocada"] = "aba1";

								EventoDocumentoDAL eve = new EventoDocumentoDAL();
								ListaEvento = eve.ObterEventos(Convert.ToInt64(txtCodigo.Text));
								Session["Eventos"] = ListaEvento;

								Habil_LogDAL log = new Habil_LogDAL();
								ListaLog = log.ListarLogs(Convert.ToDouble(txtCodigo.Text), 100);
								Session["LogsProducao"] = ListaLog.OrderByDescending(x => x.DataHora).ToList();

								AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
								ListaAnexo = anexo.ObterAnexos(Convert.ToInt32(txtCodigo.Text));
								Session["NovoAnexo"] = ListaAnexo;

								if (Convert.ToInt32(ddlSituacao.SelectedValue) == 180)
								{
									CamposGridDesabilitados(sender, e);
									Encerra = "display:none";
									btnAdicionar.Visible = false;
									btnEncerrar.Visible = false;
									btnProduzir.Visible = false;
									txtQtPraProduzir.Enabled = false;
									btnAdicionar.Visible = false;
									txtInicio.Enabled = false;
									txtFim.Enabled = false;
								}
								else if (Convert.ToInt32(ddlSituacao.SelectedValue) == 179)
								{
									Encerra = "display:block";
									btnAdicionar.Visible = true;
									btnEncerrar.Visible = true;
									btnProduzir.Visible = false;
									txtQtPraProduzir.Enabled = false;
									btnAdicionar.Visible = true;
								}
								else if (Convert.ToInt32(ddlSituacao.SelectedValue) == 178)
								{
									CamposGridDesabilitados(sender, e);
									Encerra = "display:none";
									btnAdicionar.Visible = false;
									DesativarBtn();
									pnlEncerramento.Visible = false;
									txtQtPraProduzir.Enabled = true;
								}
								else
								{
									CamposGridDesabilitados(sender, e);
									Encerra = "display:none";
									btnAdicionar.Visible = false;
									btnProduzir.Visible = true;
									txtQtPraProduzir.Enabled = true;
									btnAdicionar.Visible = false;
								}
								btnImprimir.Visible = true;
							}
						}
					}
				}
				else
				{
					LimpaTela();

					ddlEmpresa_TextChanged(sender, e);
					//btnNovoAnexo.Visible = false;

					if (Session["CodEmpresa"] != null)
						ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();

					Usuario usuario = new Usuario();
					UsuarioDAL usuarioDAL = new UsuarioDAL();
					usuario = usuarioDAL.PesquisarUsuario(Convert.ToInt64(Session["CodUsuario"]));

					lista.ForEach(delegate (Permissao p)
					{
						if (!p.AcessoCompleto)
						{
							if (!p.AcessoIncluir)
							{
								//btnSalvar.Visible = false;
							}
						}
					});
				}
			}
			if (Session["NovoAnexo"] != null)
			{
				if(Session["Doc_ordproducao"] != null)
					DescompactaDocumento(sender, e);
				ListaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];
				grdAnexo.DataSource = ListaAnexo;
				grdAnexo.DataBind();
			}
			if (Session["Eventos"] != null)
			{
				ListaEvento = (List<EventoDocumento>)Session["Eventos"];
				GrdEventoDocumento.DataSource = ListaEvento;
				GrdEventoDocumento.DataBind();
			}
			if (Session["LogsProducao"] != null)
			{
				ListaLog = (List<Habil_Log>)Session["LogsProducao"];
				grdLogDocumento.DataSource = ListaLog.OrderByDescending(x => x.DataHora).ToList();
				grdLogDocumento.DataBind();

				if (ListaLog.Count > 0)
					TabLogs = "display:block";
			}
			if (txtCodigo.Text == "" || ddlEmpresa.Items.Count == 0)
					btnVoltar_Click(sender, e);
			
			if (Session["NrOrdProducao2"] != null)
			{
				Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();
				RnOrd.AtualizarQuantidade(0, Convert.ToDecimal(Session["NrOrdProducao2"].ToString()));
				Session["NrOrdProducao2"] = null;
			}
		}
		protected void ddlEmpresa_TextChanged(object sender, EventArgs e)
		{
			if (txtCodigo.Text != "Novo")
				return;

			DBTabelaDAL db = new DBTabelaDAL();
			List<GeracaoSequencialDocumento> ListaGerDoc = new List<GeracaoSequencialDocumento>();
			GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
			GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
			ListaGerDoc = gerDocDAL.ListarGeracaoSequencial("CD_TIPO_DOCUMENTO", "INT", "10", "VALIDADE DESC");

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
				Session["CodigoGeradorSequencialProd"] = ger.CodigoGeracaoSequencial;

				double NroSequencial = geradorDAL.ExibeProximoNroSequencial(ger.Nome);
				if (NroSequencial == 0)
					txtNroDocumento.Text = ger.NumeroInicial.ToString();
				else
					txtNroDocumento.Text = NroSequencial.ToString();

			}
		}
		protected void btnAdicionar_Click(object sender, EventArgs e)
		{
			//PreencheSession();
			if (txtCodigo.Text == "Novo")
			{
				btnProduzir.Visible = false;
				return;
			}

			Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();

			string mensagem = "";

			foreach (GridViewRow row in GridComponentes.Rows)
			{
				TextBox txtRet = (TextBox)row.FindControl("txtRet");
				TextBox txtAdd = (TextBox)row.FindControl("txtAdd");

				Doc_OrdProducao p = new Doc_OrdProducao();

				decimal quantidade = 0;
				p.CodigoComponente = Convert.ToInt32(row.Cells[2].Text);

				if (txtRet.Text != "")
				{
					quantidade = Convert.ToDecimal(txtRet.Text) * -1;
				}
				if (txtAdd.Text != "")
				{
					quantidade = Convert.ToDecimal(txtAdd.Text);
				}

				quantidade += Convert.ToDecimal(row.Cells[6].Text);

				p = RnOrd.QuantidadesDisponiveisDoComponente(p.CodigoComponente, Convert.ToInt32(ddlEmpresa.SelectedValue));

				ProdutoDAL RnProd = new ProdutoDAL();
				Produto d = new Produto();

				d = RnProd.PesquisarProduto(Convert.ToInt64(p.CodigoProduto));

				if (p.CodigoProduto == 0)
				{
					mensagem += " " + d.CodigoProduto + " - " + d.DescricaoProduto + "<br>";
				}
				else
				{
					if (p.Quantidade == 0)
						mensagem += " " + d.CodigoProduto + " - " + d.DescricaoProduto + "<br>";
				}
				if (quantidade > p.Quantidade)
					mensagem += " " + d.CodigoProduto + " - " + d.DescricaoProduto + "<br>";
			}
			if (mensagem != "")
			{
				ShowMessage("Produto(s) " + mensagem + " sem quantidade no Estoque", MessageType.Info);
				mensagem = "";
				return;
			}
			Habil_Estacao he = new Habil_Estacao();
			Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
			int intMaquina = 0;
			he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

			if (he != null)
				intMaquina = Convert.ToInt32(he.CodigoEstacao);
			
			ddlSituacao.SelectedValue = "179";

			int Resposta = 0;

			PreencheSession();
			Resposta = SalvarDocumento(sender, e);

			if (Resposta == 1)
				return;	
			
			bool SpResp = false;
			if (txtCodigo.Text != "Novo")
			{
				SpResp = RnOrd.ExecutaSpAtendimentoProducao(intMaquina, txtCodigo.Text);
			}
			Session["NrOrdProducao"] = null;
			Session["RptDocAdicionar"] = RnOrd.RelOrdemProducao(Convert.ToDecimal(txtCodigo.Text)); 

			PreencheSessionDepoisDeAdicionar();
			Resposta = SalvarDocumento(sender, e);
			Resposta = 0;
			if (Resposta == 1)
				return;

			if (SpResp)
				Response.Redirect("~/Pages/Estoque/RelOrdProducao.aspx");
			else
				ShowMessage("Não foi possível Separar os Componentes", MessageType.Info);
		}
		protected void btnCfmSim_Click(object sender, EventArgs e)
		{
			Habil_Estacao he = new Habil_Estacao();
			Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
			Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();
			Doc_OrdProducao p = new Doc_OrdProducao();
			MovimentacaoInterna ep = new MovimentacaoInterna();
			MovimentacaoInterna ep2 = new MovimentacaoInterna();
			MovimentacaoInternaDAL d = new MovimentacaoInternaDAL();
			int intReturn = 0;

			he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
			ep.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());
			if (he != null)
			{
				ep.CodigoMaquina = he.CodigoEstacao;
			}

			intReturn = SalvarDocumento(sender, e);
			if (intReturn == 1)
				return;

			if (Convert.ToInt32(ddlSituacao.SelectedValue) == 179)
			{
				if(intReturn == 1)
					return;

				try
				{
					List<Doc_OrdProducao> lista = new List<Doc_OrdProducao>();				

					lista = RnOrd.ListarOrdemProducaoParaEncerramento(Convert.ToInt32(txtCodigo.Text));

					foreach (Doc_OrdProducao item in lista)
					{

						ep = new MovimentacaoInterna();

						ep2 = d.LerSaldoAnterior(item.CodigoEmpresa, item.CodigoLocalizacao, item.CodigoProduto, item.CodigoLote);

						ep.ValorUnitario = ep2.ValorUnitario;
						ep.VlSaldoAjuste = 0;//item.QtInventario;
						ep.ValorSaldoAnterior = ep2.ValorSaldoAnterior;
						ep.QtMovimentada = item.QtAtendida;
						ep.NumeroDoc = "OP [" + txtNroDocumento.Text + "]";
						ep.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());
						ep.CodigoDocumento = Convert.ToInt32(txtCodigo.Text);
						if (he != null)
						{
							ep.CodigoMaquina = he.CodigoEstacao;
						}
						ep.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
						ep.CodigoEmpresa = item.CodigoEmpresa;
						ep.CodigoIndiceLocalizacao = item.CodigoLocalizacao;
						ep.CodigoProduto = item.CodigoProduto;
						ep.CodigoLote = item.CodigoLote;
						ep.TpOperacao = "S";

						if (ep != null)
							d.Inserir(ep);
					}
				}

				catch (Exception ex)
				{
					ShowMessage("Erro: " + ex, MessageType.Info);

				}
				finally
				{
					intReturn = 0;
				}
			}
			else if (Convert.ToInt32(ddlSituacao.SelectedValue) == 180)
			{
				d = new MovimentacaoInternaDAL();
				ep = new MovimentacaoInterna();
				ep2 = new MovimentacaoInterna();

				TipoOperacao t = new TipoOperacao();
				TipoOperacaoDAL RnTipo = new TipoOperacaoDAL();

				t.CodigoTipoOperacao = RnTipo.TipoContraPartida(t.CodTipoOperCtPartida = RnTipo.TipoContraPartida(Convert.ToInt32(ddlTipoOperacao.SelectedValue)));
				if (t.CodigoTipoOperacao == 0)
				{
					ShowMessage("Ordem de Produção não pode ser Cancelada, Estorno do Composto sem contra partida!", MessageType.Info);
					intReturn = 1;
					return;
				}
				ep2 = d.LerSaldoAnterior(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToInt32(ddllocalizacaoEncerramento.SelectedValue), Convert.ToInt32(txtComposto.Text), 0);

				ep = new MovimentacaoInterna();
				if (ep2 != null)
				{
					ep.ValorSaldoAnterior = ep2.ValorSaldoAnterior;
				}
				ep.ValorUnitario = Convert.ToDecimal(txtValor.Text);
				ep.CodigoDocumento = Convert.ToInt32(txtCodigo.Text);
				ep.VlSaldoAjuste = 0;//item.QtInventario;
				ep.QtMovimentada = Convert.ToDecimal(txtQtFinal.Text);
				ep.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());
				ep.NumeroDoc = "OP [" + txtNroDocumento.Text + "]";
				if (he != null)
				{
					ep.CodigoMaquina = he.CodigoEstacao;
				}
				ep.CodigoTipoOperacao = Convert.ToInt32(t.CodigoTipoOperacao);
				ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
				ep.CodigoIndiceLocalizacao = Convert.ToInt32(ddllocalizacaoEncerramento.SelectedValue);
				ep.CodigoProduto = Convert.ToInt32(txtComposto.Text);
				ep.CodigoLote = 0;
				ep.TpOperacao = "S";

				if (ep != null)
					d.Inserir(ep);
			}			

			intReturn = AtualizarSituacao(179, 178);
			RnOrd.Cancelar(Convert.ToDecimal(txtCodigo.Text));

			Session["MensagemTela"] = "Documento Cancelado com sucesso!!!";
			btnVoltar_Click(sender, e);
		}
		protected void btnVoltar_Click(object sender, EventArgs e)
		{
			if (Session["Botoes"] != null)
			{
				Ativarbtn();
				Session["Botoes"] = null;
				return;
			}
			Session["TabFocada"] = null;
			Session["Eventos"] = null;
			Session["NovoAnexo"] = null;
			Session["ListaComponentes"] = null;
			Session["LogsProducao"] = null;
			Response.Redirect("~/Pages/Estoque/ConOrdProducao.aspx");
			this.Dispose();
		}
		protected void btnSalvar_Click(object sender, EventArgs e)
		{
			int Resposta = 0;
			Resposta = SalvarDocumento(sender, e);
			if (Resposta == 0)
				btnVoltar_Click(sender, e);

		}
		protected void btnCancelar_Click(object sender, EventArgs e)
		{
			lblMens.Text = "Deseja Cancelar Ordem de Produção Nº " + txtCodigo.Text;
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Cancelar();", true);
		}
		protected void btnProduzir_Click(object sender, EventArgs e)
		{
			if (txtCodigo.Text == "Novo")
			{
				btnProduzir.Visible = false;
				return;
			}

			Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();

			string mensagem = "";

			foreach (GridViewRow row in GridComponentes.Rows)
			{
				TextBox txtRet = (TextBox)row.FindControl("txtRet");
				TextBox txtAdd = (TextBox)row.FindControl("txtAdd");

				Doc_OrdProducao p = new Doc_OrdProducao();

				decimal quantidade = 0;
				p.CodigoComponente = Convert.ToInt32(row.Cells[2].Text);

				if (txtRet.Text != "")
				{
					quantidade = Convert.ToDecimal(txtRet.Text) * -1;
				}
				else if (txtAdd.Text != "")
				{
					quantidade = Convert.ToDecimal(txtAdd.Text);
				}

				quantidade += Convert.ToDecimal(row.Cells[6].Text);

				p = RnOrd.QuantidadesDisponiveisDoComponente(p.CodigoComponente, Convert.ToInt32(ddlEmpresa.SelectedValue));

				ProdutoDAL RnProd = new ProdutoDAL();
				Produto d = new Produto();

				d = RnProd.PesquisarProduto(Convert.ToInt64(p.CodigoProduto));

				if (p.CodigoProduto == 0)
				{
					mensagem += " " + d.CodigoProduto + " - " + d.DescricaoProduto + "<br>";
				}
				else
				{
					if (p.Quantidade == 0)
						mensagem += " " + d.CodigoProduto + " - " + d.DescricaoProduto + "<br>";
				}
				if (quantidade > p.Quantidade)
					mensagem += " " + d.CodigoProduto + " - " + d.DescricaoProduto + "<br>";
			}
			if (mensagem != "")
			{
				ShowMessage("Produto(s) " + mensagem + " sem quantidade no Estoque", MessageType.Info);
				mensagem = "";
				return;
			}
			ddlSituacao.SelectedValue = "179";

			int Resposta = 0;

			Resposta = SalvarDocumento(sender, e);

			if (Resposta == 1)
				return;

			int intMaquina = 0;

			Habil_Estacao he = new Habil_Estacao();
			Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

			he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
			if (he != null)
			{
				intMaquina = Convert.ToInt32(he.CodigoEstacao);
			}
			bool SpResp = false;
			if (txtCodigo.Text != "Novo")
			{
				//txtCodigo.Text = "704";
				SpResp = RnOrd.ExecutaSpAtendimentoProducao(intMaquina, txtCodigo.Text);
			}

			Session["NrOrdProducao"] = txtCodigo.Text;

			RnOrd.AtualizarQuantidade(0, Convert.ToDecimal(txtCodigo.Text));

			if (SpResp)
				Response.Redirect("~/Pages/Estoque/RelOrdProducao.aspx");
		}
		protected void btnEncerrar_Click(object sender, EventArgs e)
		{
			string Descricao = "";
			List<ItemDaComposicao> Lista = new List<ItemDaComposicao>();
			foreach (GridViewRow row in GridRoteiros.Rows)
			{
				ItemDaComposicao it = new ItemDaComposicao();

				if (row.Cells[2].Text == "&nbsp;" || row.Cells[3].Text == "&nbsp;")
				{
					it.DescRoteiro = row.Cells[1].Text;
					it.CodigoRoteiro = Convert.ToInt16(row.Cells[0].Text);
				}

				if (it.CodigoRoteiro != 0)
				{
					Descricao += it.CodigoRoteiro + " - " + it.DescRoteiro + "<br>";
					Lista.Add(it);
				}
			}

			if (Lista.Count > 0)
			{
				ShowMessage("Roteiro(s) " + Descricao + " Devem Conter Data de Inicio/Fim!", MessageType.Info);
				return;
			}
			PanelSelect = "aba8";
			Session["TabFocada"] = "aba8";

			Session["Botoes"] = "";
			DesativarBtn();

			DBTabelaDAL RnTab = new DBTabelaDAL();
			DateTime Hoje = RnTab.ObterDataHoraServidor();
			txtDataEncerramento.Text = Hoje.ToString("dd/MM/yyyy HH:mm:ss");


			pnlEncerramento.Visible = true;
		}
		protected void btnEncerramento_Click(object sender, EventArgs e)
		{
			int intReturn = 0;
			try
			{
				if (ddllocalizacaoEncerramento.SelectedValue == " * Nenhum Selecionado * ")
				{
					ShowMessage("Localização para destino de Produção é obrigatória", MessageType.Info);
					intReturn = 1;
					return;
				}
				if (txtQtFinal.Text == "0,00")
				{
					ShowMessage("Quantidade Produzida deve ser maior que Zero!", MessageType.Info);
					intReturn = 1;
					return;
				}

				intReturn = SalvarDocumento(sender, e);

				if (intReturn == 1)
					return;

				Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();
				Doc_OrdProducao p = new Doc_OrdProducao();

				if (Convert.ToInt32(ddlSituacao.SelectedValue) != 179)
				{
					ShowMessage("Situação da Ordem de Pedido deve ser: Produção", MessageType.Info);
					intReturn = 1;
					return;
				}
				try
				{
					p.CodigoDocumento = Convert.ToInt32(txtCodigo.Text);

					RnOrd.AtualizaParaEncerramento(p);
				}
				catch (Exception)
				{
					ShowMessage("Não Foi Possível Atualizar Data de Encerramento", MessageType.Info);
					intReturn = 1;
					return;
				}
				try
				{
					intReturn = AtualizarSituacao(179, 180);
					if (intReturn == 1)
						return;
				}
				catch (Exception)
				{
					ShowMessage("Não Foi Possível Atualizar a Situação da Ordem de Pedido", MessageType.Info);
					intReturn = 1;
					return;
				}
				try
				{
					Habil_Estacao he = new Habil_Estacao();
					Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
					List<Doc_OrdProducao> lista = new List<Doc_OrdProducao>();
					RnOrd = new Doc_OrdProducaoDAL();
					DBTabelaDAL dbTDAL = new DBTabelaDAL();

					he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

					MovimentacaoInterna ep = new MovimentacaoInterna();
					MovimentacaoInterna ep2 = new MovimentacaoInterna();
					MovimentacaoInternaDAL d = new MovimentacaoInternaDAL();

					lista = RnOrd.ListarOrdemProducaoParaEncerramento(Convert.ToInt32(txtCodigo.Text));

					foreach (Doc_OrdProducao item in lista)
					{

						ep = new MovimentacaoInterna();

						ep2 = d.LerSaldoAnterior(item.CodigoEmpresa, item.CodigoLocalizacao, item.CodigoProduto, item.CodigoLote);

						ep.ValorUnitario = ep2.ValorUnitario;
						ep.VlSaldoAjuste = 0;
						ep.ValorSaldoAnterior = ep2.ValorSaldoAnterior;
						ep.QtMovimentada = item.QtAtendida;
						ep.NumeroDoc = "OP [" + txtNroDocumento.Text + "]";
						ep.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());
						ep.CodigoDocumento = Convert.ToInt32(txtCodigo.Text);
						if (he != null)
						{
							ep.CodigoMaquina = he.CodigoEstacao;
						}
						ep.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
						ep.CodigoEmpresa = item.CodigoEmpresa;
						ep.CodigoIndiceLocalizacao = item.CodigoLocalizacao;
						ep.CodigoProduto = item.CodigoProduto;
						ep.CodigoLote = item.CodigoLote;
						ep.TpOperacao = "S";

						if (ep != null)
							d.Inserir(ep);
					}

					TipoOperacao t = new TipoOperacao();
					TipoOperacaoDAL RnTipo = new TipoOperacaoDAL();

					t.CodTipoOperCtPartida = RnTipo.TipoContraPartida(Convert.ToInt32(ddlTipoOperacao.SelectedValue));

					ep2 = d.LerSaldoAnterior(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToInt32(ddllocalizacaoEncerramento.SelectedValue), Convert.ToInt32(txtComposto.Text), 0);

					ep = new MovimentacaoInterna();
					if (ep2 != null)
					{
						ep.ValorSaldoAnterior = ep2.ValorSaldoAnterior;
					}
					ep.ValorUnitario = Convert.ToDecimal(txtValor.Text);
					ep.CodigoDocumento = Convert.ToInt32(txtCodigo.Text);
					ep.VlSaldoAjuste = 0;//item.QtInventario;
					ep.QtMovimentada = Convert.ToDecimal(txtQtFinal.Text);
					ep.NumeroDoc = "OP [" + txtNroDocumento.Text + "]";
					ep.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());
					if (he != null)
					{
						ep.CodigoMaquina = he.CodigoEstacao;
					}
					ep.CodigoTipoOperacao = t.CodTipoOperCtPartida;
					ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
					ep.CodigoIndiceLocalizacao = Convert.ToInt32(ddllocalizacaoEncerramento.SelectedValue);
					ep.CodigoProduto = Convert.ToInt32(txtComposto.Text);
					ep.CodigoLote = 0;
					ep.TpOperacao = "E";

					if (ep != null)
						d.Inserir(ep);
				}
				catch (Exception ex)
				{
					ShowMessage("Erro: " + ex, MessageType.Info);
					intReturn = 1;
				}
				finally
				{
					intReturn = 0;
				}
			}
			catch (Exception ex)
			{
				ShowMessage("Erro: " + ex, MessageType.Info);
			}
			finally
			{
				if (intReturn != 1)
				{
					Session["Botoes"] = null;
					btnVoltar_Click(sender, e);
					Session["MensagemTela"] = "Ordem de Produção Encerrada com Sucesso!!!";
				}
			}
		}
		protected void btnImprimir_Click(object sender, EventArgs e)
		{
			Session["Codigo_OP"] = txtCodigo.Text;
			Response.Redirect("~/Pages/Estoque/RelOP.aspx");
		}
		protected void txtPrazo_TextChanged(object sender, EventArgs e)
		{
			Boolean blnCampo = false;

			if (txtPrazo.Text.Equals(""))
			{
				txtPrazo.Text = "0";
			}
			else
			{
				v.CampoValido("Percentual de Quebra", txtPrazo.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
				if (blnCampo)
				{
					txtDiasPrazo.Text = "";
					txtPrazo.Text = Convert.ToInt32(txtPrazo.Text).ToString();
					txtDiasPrazo.Text = "Dia: " + Convert.ToDateTime(txtdtemissao.Text).AddDays(+Convert.ToInt32(txtPrazo.Text)).ToString("dd/MM/yyyy");
				}
				else
					txtPrazo.Text = "0";
			}
		}
		protected void txtDocumento_TextChanged(object sender, EventArgs e)
		{

		}
		protected void txtNrDocumento_TextChanged(object sender, EventArgs e)
		{

		}
		protected void txtQtPraProduzir_TextChanged(object sender, EventArgs e)
		{
			Boolean blnCampo = false;

			if (txtQtPraProduzir.Text.Equals(""))
			{
				txtQtPraProduzir.Text = "0,000";
			}
			else
			{
				v.CampoValido("Quantidade a Produzir", txtQtPraProduzir.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
				if (blnCampo)
				{
					txtQtPraProduzir.Text = Convert.ToDecimal(txtQtPraProduzir.Text).ToString("###,##0.000");
				}
				else
				{
					txtQtPraProduzir.Text = "0,000";
					return;
				}
			}
			if (Session["Atualizar"] == null)
			{
				CalculaGrid();
			}
			else
				Session["Atualizar"] = null;
		}
		protected void txtDocOriginal_TextChanged(object sender, EventArgs e)
		{
			Doc_OrdProducao p = new Doc_OrdProducao();
			Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();

			//RnOrd.
		}
		protected void grdAnexo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ValidaCampos())
				return;
			CompactaDocumento(sender, e);
			Session["ZoomDocCtaAnexo"] = HttpUtility.HtmlDecode(grdAnexo.SelectedRow.Cells[0].Text);
			Response.Redirect("~/Pages/financeiros/ManAnexo.aspx?cad=10");
		}
		protected void btnNovoAnexo_Click(object sender, EventArgs e)
		{
			if (!ValidaCampos())
				return;

			CompactaDocumento(sender, e);
			Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=10");
		}
		protected void ddlAppUsoProducao_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Convert.ToInt32(ddlAppUsoProducao.SelectedValue) == 174)
			{
				ShowMessage("Uso da Aplicação " + ddlAppUsoProducao.SelectedItem.Text.ToLower() + " deve Conter um Pedido", MessageType.Info);
				return;
			}
		}
		protected void btnPesquisarComposto_Click(object sender, EventArgs e)
		{
			ComposicaoDAL RnComp = new ComposicaoDAL();
			List<Composicao> listaComp = new List<Composicao>();

			listaComp = RnComp.PesquisarListaComposicao("");

			GridCompostos.DataSource = listaComp;
			GridCompostos.DataBind();

			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Composto();", true);
		}
		protected void txtPesquisaComposto_TextChanged(object sender, EventArgs e)
		{
			ComposicaoDAL RnComp = new ComposicaoDAL();
			List<Composicao> listaComp = new List<Composicao>();

			listaComp = RnComp.PesquisarListaComposicao(txtPesquisaComposto.Text);

			GridCompostos.DataSource = listaComp;
			GridCompostos.DataBind();

			txtPesquisaComposto.Text = "";

			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Composto();", true);
		}
		protected void GridCompostos_SelectedIndexChanged(object sender, EventArgs e)
		{
			txtComposto.Text = HttpUtility.HtmlDecode(GridCompostos.SelectedRow.Cells[0].Text);
			txtComposto_TextChanged(sender, e);
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup",  "CompostoHide();", true);
		}
		protected void txtComposto_TextChanged(object sender, EventArgs e)
		{
			Boolean blnCampo = false;
			if (txtComposto.Text.Equals(""))
			{
				return;
			}
			else
			{
				v.CampoValido("Codigo do Composto", txtComposto.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
				if (!blnCampo)
				{
					txtComposto.Text = "";
					return;
				}
			}
			Int32 codigoItem = Convert.ToInt32(txtComposto.Text);
			ComposicaoDAL composicaoDAL = new ComposicaoDAL();
			Composicao c = new Composicao();
			c = composicaoDAL.PesquisarComposicao(codigoItem);

			if (c != null)
			{
				if (c.CodigoSituacao == 1)
				{
					txtDcrproduto.Text = c.DescricaoProduto;
					txtObsComposicao.Text = c.Observacao;

					AtualizaGridComponentes();

					if (txtQtPraProduzir.Text != "")
					{
						txtQtPraProduzir_TextChanged(sender, e);
					}
				}
				else
				{
					txtComposto.Enabled = true;
					btnPesquisarComposto.Enabled = true;
					ShowMessage("Composto está Inativo", MessageType.Info);
					txtDcrproduto.Text = "";
				}
			}
			else
			{
				txtComposto.Enabled = true;
				btnPesquisarComposto.Enabled = true;
				txtDcrproduto.Text = "";
				txtComposto.Text = "";
				ShowMessage("Composto não cadastrado", MessageType.Info);
			}
			if (Convert.ToInt32(ddlSituacao.SelectedValue) != 179)
			{
				CamposGridDesabilitados(sender, e);
			}

			txtComposto.Focus();
		}
		protected void btnPessoa_Click(object sender, EventArgs e)
		{
			PessoaDAL RnPess = new PessoaDAL();
			List<Pessoa> listaPessoas = new List<Pessoa>();

			listaPessoas = RnPess.ListarCliente("");

			GridPessoas.DataSource = listaPessoas;
			GridPessoas.DataBind();
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Pessoa();", true);
		}
		protected void txtPesquisaCliente_TextChanged(object sender, EventArgs e)
		{
			PessoaDAL RnPess = new PessoaDAL();
			List<Pessoa> listaPessoas = new List<Pessoa>();

			listaPessoas = RnPess.ListarCliente(txtPesquisaCliente.Text);

			GridPessoas.DataSource = listaPessoas;
			GridPessoas.DataBind();
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Pessoa();", true);
		}
		protected void GridPessoas_SelectedIndexChanged(object sender, EventArgs e)
		{
			txtCodPessoa.Text = HttpUtility.HtmlDecode(GridPessoas.SelectedRow.Cells[0].Text);
			txtCodPessoa_TextChanged(sender, e);
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "PessoaHide();", true);
		}
		protected void txtCodPessoa_TextChanged(object sender, EventArgs e)
		{
			Pessoa p = new Pessoa();
			PessoaDAL RnPess = new PessoaDAL();

			p = RnPess.PesquisarCliente(Convert.ToInt64(txtCodPessoa.Text));

			if (p.CodigoSituacaoPessoa == 1)
			{
				txtNomePessoa.Text = p.NomePessoa;
			}
		}
		protected void btnDoc_Click(object sender, EventArgs e)
		{
			gridDocumento.Visible = true;


			Doc_OrdProducao p = new Doc_OrdProducao();
			List<Doc_OrdProducao> Lista = new List<Doc_OrdProducao>();
			Doc_OrdProducaoDAL RnDoc = new Doc_OrdProducaoDAL();

			gridDocumento.DataSource = RnDoc.ListarDocumentosNaoListados("", Convert.ToInt32(ddlEmpresa.SelectedValue), 0);
			gridDocumento.DataBind();

			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Documento();", true);
		}
		protected void txtPesquisaDocumento_TextChanged(object sender, EventArgs e)
		{
			decimal nrdoc = 0;
			Doc_OrdProducao p = new Doc_OrdProducao();
			List<Doc_OrdProducao> Lista = new List<Doc_OrdProducao>();
			Doc_OrdProducaoDAL RnDoc = new Doc_OrdProducaoDAL();
			if (txtPesquisaDocumento.Text != "")
			{
				Boolean blnCampo = false;

				if (txtPesquisaDocumento.Text.Equals(""))
				{
					txtPesquisaDocumento.Text = "0";
				}
				else
				{
					v.CampoValido("Nº do Documento", txtPesquisaDocumento.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
					if (blnCampo)
					{
						txtPesquisaDocumento.Text = Convert.ToDecimal(txtPesquisaDocumento.Text).ToString();
						nrdoc = Convert.ToDecimal(txtPesquisaDocumento.Text);
					}
					else
						txtPesquisaDocumento.Text = "0";
				}
			}

			gridDocumento.DataSource = RnDoc.ListarDocumentosNaoListados(txtPesquisaClienteDocumento.Text, Convert.ToInt32(ddlEmpresa.SelectedValue), nrdoc);
			gridDocumento.DataBind();

			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Documento();", true);
		}
		protected void txtPesquisaClienteDocumento_TextChanged(object sender, EventArgs e)
		{
			txtPesquisaDocumento_TextChanged(sender, e);
		}
		protected void gridDocumento_SelectedIndexChanged(object sender, EventArgs e)
		{
			Doc_OrdProducao p = new Doc_OrdProducao();
			List<Doc_OrdProducao> Lista = new List<Doc_OrdProducao>();
			Doc_OrdProducaoDAL RnDoc = new Doc_OrdProducaoDAL();

			txtDocOriginal.Text = HttpUtility.HtmlDecode(gridDocumento.SelectedRow.Cells[0].Text);
			txtCodPessoa.Text = HttpUtility.HtmlDecode(gridDocumento.SelectedRow.Cells[4].Text);
			txtComposto.Text = HttpUtility.HtmlDecode(gridDocumento.SelectedRow.Cells[6].Text);
			txtPrazo.Text = HttpUtility.HtmlDecode(gridDocumento.SelectedRow.Cells[8].Text);

			txtCodPessoa_TextChanged(sender, e);
			txtComposto_TextChanged(sender, e);
			txtPrazo_TextChanged(sender, e);

			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "DocumentoHide();", true);

		}
		protected void BtnRemProduto_Click(object sender, EventArgs e)
		{

		}
		protected void BtnAddProduto_Click(object sender, EventArgs e)
		{

			List<ItemDaComposicao> Lista = new List<ItemDaComposicao>();

			if (Session["ListaRoteiro"] != null)
				Lista = (List<ItemDaComposicao>)Session["ListaRoteiro"];
			else
				return;

			if (txtRoteiro.Text == "")
			{
				ShowMessage("Selecione um Roteiro!!!", MessageType.Info);
				return;
			}

			//data2 = DateTime.Today;

			DateTime data1, data2;
			data1 = Convert.ToDateTime(txtInicio.Text);
			data2 = Convert.ToDateTime(txtdtemissao.Text);
			if (txtInicio.Text.Length > 1)
			{
				if (data1 > data2)
				{
					ShowMessage("Data de Inicio deve ser antes da data de Emissão!", MessageType.Info);
					txtInicio.Focus();
					return;
				}
			}
			if (txtFim.Text.Length > 1)
			{
				DateTime data3, data4;
				data3 = Convert.ToDateTime(txtInicio.Text);
				data4 = Convert.ToDateTime(txtFim.Text);
				if (data3 > data4)
				{
					ShowMessage("Data Final deve ser após a data de Inicial!", MessageType.Info);
					txtInicio.Focus();
					return;
				}
			}

			ItemDaComposicao p = new ItemDaComposicao();

			Lista = Lista.Where(x => x.CodigoRoteiro != Convert.ToInt16(txtRoteiro.Text)).ToList();

			p.CodigoRoteiro = Convert.ToInt16(txtRoteiro.Text);
			p.DescRoteiro = txtDescRoteiro.Text;
			if (txtInicio.Text.Length > 1)
				p.DataInicio = Convert.ToDateTime(txtInicio.Text);
			if (txtFim.Text.Length > 1)
				p.DataFim = Convert.ToDateTime(txtFim.Text);
			Lista.Add(p);

			Session["ListaRoteiro"] = Lista;

			GridRoteiros.DataSource = Lista;
			GridRoteiros.DataBind();
		}
		protected void GridRoteiros_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Convert.ToInt32(row.Cells[2].Text);

			txtRoteiro.Text = HttpUtility.HtmlDecode(GridRoteiros.SelectedRow.Cells[0].Text);
			txtDescRoteiro.Text = HttpUtility.HtmlDecode(GridRoteiros.SelectedRow.Cells[1].Text);
			txtInicio.Text = HttpUtility.HtmlDecode(GridRoteiros.SelectedRow.Cells[2].Text);
			txtFim.Text = HttpUtility.HtmlDecode(GridRoteiros.SelectedRow.Cells[3].Text);
		}
		protected void QtFinal_TextChanged(object sender, EventArgs e)
		{
			Boolean blnCampo = false;

			if (txtQtFinal.Text.Equals(""))
			{
				txtQtFinal.Text = "0,00";
			}
			else
			{
				v.CampoValido("Quantidade a Produzir", txtQtFinal.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
				if (blnCampo)
				{
					txtQtFinal.Text = Convert.ToDecimal(txtQtFinal.Text).ToString("###,##0.00");
				}
				else
				{
					txtQtFinal.Text = "0,00";
				}
			}
		}
		protected void grdLogDocumento_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
		//Criadas
		protected void AtualizaGridComponentes()
		{
			List<ItemDaComposicao> ListaItem = new List<ItemDaComposicao>();
			List<ItemDaComposicao> ListaRoteiro = new List<ItemDaComposicao>();
			ItemDaComposicaoDAL RnItem = new ItemDaComposicaoDAL();
			if (txtComposto.Text == "")
			{
				ShowMessage("Selecione um Composto!", MessageType.Info);
				return;
			}
			ListaItem = RnItem.ListarItemDaComposicao(Convert.ToInt32(txtComposto.Text));

			txtValor.Text = ListaItem.Sum(x => x.ValorCustoComponente).ToString("###,##0.00");
			Session["ListaComponentes"] = ListaItem;

			ListaRoteiro = RnItem.ListarRoteiros(Convert.ToInt32(txtComposto.Text));

			Session["ListaRoteiro"] = ListaRoteiro;

			GridRoteiros.DataSource = ListaRoteiro;
			GridRoteiros.DataBind();

			GridComponentes.DataSource = ListaItem;
			GridComponentes.DataBind();
		}
		protected void DesativarBtn()
		{
			btnProduzir.Visible = false;
			btnEncerrar.Visible = false;
			btnSalvar.Visible = false;
			btnCancelar.Visible = false;
			btnImprimir.Visible = false;
			pnlEncerramento.Visible = true;
			btnAdicionar.Visible = false;
		}
		protected void Ativarbtn()
		{
			btnProduzir.Visible = false;
			btnEncerrar.Visible = true;
			btnSalvar.Visible = true;
			btnCancelar.Visible = true;
			//btnImprimir.Visible = true;
			//pnlEncerramento.Visible = false;
			pnlEncerramento.Visible = false;
			btnAdicionar.Visible = true;

		}
		protected void CompactaDocumento(object sender, EventArgs e)
		{
			try
			{
				Doc_OrdProducao p = new Doc_OrdProducao();
				//Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();

				if (txtCodigo.Text != "Novo")
					p.CodigoDocumento = Convert.ToDecimal(txtCodigo.Text);

				if (ddlTipoOperacao.SelectedValue != ".....Selecione tipo de Operação.....")
					p.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);

				p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
				p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
				p.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
				p.DGSerieDocumento = txtSerie.Text;
				if (txtDocOriginal.Text != "")
					p.CodigoDocumentoOriginal = Convert.ToDecimal(txtDocOriginal.Text);
				p.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
				if (txtdtEncerramento.Text != "")
					p.DataEncerramento = Convert.ToDateTime(txtdtEncerramento.Text);
				if (txtCodPessoa.Text != "")
					p.Cpl_CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
				p.CodigoAplicacaoUso = Convert.ToInt32(ddlAppUsoProducao.SelectedValue);
				p.CodigoOperador = Convert.ToInt32(ddlOperador.SelectedValue);
				if (txtComposto.Text != "")
					p.CodigoComposto = Convert.ToInt32(txtComposto.Text);
				if (txtQtPraProduzir.Text != "")
					p.QtProduzir = Convert.ToDecimal(txtQtPraProduzir.Text);
				if (txtQtJaProduzida.Text != "")
					p.QtProduzida = Convert.ToDecimal(txtQtJaProduzida.Text);
				p.formato = txtFormato.Text;
				p.LogoMarca = txtLogo.Text;
				if (txtPrazo.Text != "")
					p.Prazo = Convert.ToInt32(txtPrazo.Text);

				p.Maquina = txtMaquina.Text;
				p.DescricaoDocumento = txtObsProducao.Text;
				p.ObsComposicao = txtObsComposicao.Text;

				Session["Doc_ordproducao"] = p;
			}
			catch (Exception ex)
			{
				ShowMessage(ex.ToString(), MessageType.Info);
			}
		}
		protected void DescompactaDocumento(object sender, EventArgs e)
		{
			try
			{
				Doc_OrdProducao p = (Doc_OrdProducao)Session["Doc_ordproducao"];

				CarregaTiposSituacoes();

				if (p.CodigoDocumento == 0)
				{
					txtCodigo.Text = "Novo";
				}
				else
				{
					txtCodigo.Text = Convert.ToString(p.CodigoDocumento);
				}
				if (p.CodigoTipoOperacao != 0)
					ddlTipoOperacao.SelectedValue = p.CodigoTipoOperacao.ToString();
				ddlEmpresa.SelectedValue = Convert.ToString(p.CodigoEmpresa);
				ddlSituacao.SelectedValue = Convert.ToString(p.CodigoSituacao);
				txtNroDocumento.Text = Convert.ToString(p.NumeroDocumento);
				txtSerie.Text = Convert.ToString(p.DGSerieDocumento);

				txtDocOriginal.Text = p.DescricaoDocumento.ToString();

				if (Convert.ToString(p.DataHoraEmissao) != "01/01/0001 00:00:00")
					txtdtemissao.Text = p.DataHoraEmissao.ToString();
				if (Convert.ToString(p.DataEncerramento) != "01/01/0001 00:00:00")
				{
					txtdtEncerramento.Text = p.DataEncerramento.ToString();
				}
				if (p.Cpl_CodigoPessoa != 0)
				{
					txtCodPessoa.Text = p.Cpl_CodigoPessoa.ToString();
					txtCodPessoa_TextChanged(sender, e);
				}
				if (p.CodigoComposto != 0)
				{
					txtComposto.Text = p.CodigoComposto.ToString();
					txtComposto_TextChanged(sender, e);
				}
				ddlAppUsoProducao.SelectedValue = p.CodigoAplicacaoUso.ToString();
				ddlOperador.SelectedValue = p.CodigoOperador.ToString();
				txtQtPraProduzir.Text = p.QtProduzir.ToString();
				txtQtJaProduzida.Text = p.QtProduzida.ToString();
				txtFormato.Text = p.formato;
				txtLogo.Text = p.LogoMarca;
				txtPrazo.Text = p.Prazo.ToString();
				txtMaquina.Text = p.Maquina;
				txtObsProducao.Text = p.DescricaoDocumento;
				txtObsComposicao.Text = p.ObsComposicao;

				Session["Doc_ordproducao"] = null;
			}
			catch (Exception ex)
			{
				ShowMessage(ex.Message, MessageType.Error);
			}

		}
		protected int SalvarDocumento(object sender, EventArgs e)

		{
			try
			{
				if (!ValidaCamposSitAberta())
					return 1;

				ComposicaoDAL RnComp = new ComposicaoDAL();
				Composicao p = new Composicao();
				Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();
				Doc_OrdProducao ep = new Doc_OrdProducao();
				EventoDocumento a = new EventoDocumento();
				List<AnexoDocumento> Anexolist = new List<AnexoDocumento>();

				ep.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
				ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
				ep.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
				ep.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
				if (txtDocOriginal.Text != "")
					ep.CodigoDocumentoOriginal = Convert.ToDecimal(txtDocOriginal.Text);

				ep.CodigoTipoDocumento = 10;
				ep.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
				ep.CodigoAplicacaoUso = Convert.ToInt32(ddlAppUsoProducao.SelectedValue);
				ep.CodigoOperador = Convert.ToInt32(ddlOperador.SelectedValue);
				ep.CodigoComposto = Convert.ToInt32(txtComposto.Text);
				ep.QtProduzir = Convert.ToDecimal(txtQtPraProduzir.Text);
				ep.QtProduzida = Convert.ToDecimal(txtQtJaProduzida.Text);
				if (txtQtFinal.Text != "0,00")
					ep.QtProduzida = Convert.ToDecimal(txtQtFinal.Text);
				ep.Cpl_CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
				ep.DescricaoDocumento = txtObsProducao.Text;
				ep.formato = txtFormato.Text;
				ep.LogoMarca = txtLogo.Text;
				ep.Maquina = txtMaquina.Text;
				ep.ValorTotal = Convert.ToDecimal(txtValor.Text);
				if (txtPrazo.Text != "")
				{
					ep.Prazo = Convert.ToInt32(txtPrazo.Text);
				}

				ProdutoDocumentoDAL RnProd = new ProdutoDocumentoDAL();
				List<ProdutoDocumento> Lista = new List<ProdutoDocumento>();
				List<ItemDaComposicao> ListaRoteiro = new List<ItemDaComposicao>();
				List<ItemDaComposicao> ListaProdutosRoteiro = new List<ItemDaComposicao>();

				if (Session["ListaRoteiro"] != null)
				{
					ListaRoteiro = (List<ItemDaComposicao>)Session["ListaRoteiro"];
				}

				foreach (GridViewRow row in GridComponentes.Rows)
				{
					ProdutoDocumento pd = new ProdutoDocumento();

					TextBox txtRet = (TextBox)row.FindControl("txtRet");
					TextBox txtAdd = (TextBox)row.FindControl("txtAdd");

					pd.CodigoProduto = Convert.ToInt32(row.Cells[2].Text);
					pd.Cpl_DscProduto = row.Cells[3].Text;
					pd.Unidade = row.Cells[4].Text;
					pd.Quantidade = Convert.ToDecimal(row.Cells[6].Text);
					pd.QuantidadeAtendida = Convert.ToDecimal(row.Cells[9].Text);
					pd.PrecoItem = Convert.ToDecimal(row.Cells[10].Text);
					pd.ValorTotalItem = Convert.ToDecimal(row.Cells[11].Text);
					pd.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);

					if (ddlLocalizacao.SelectedValue != " * Nenhum Selecionado * ")
						pd.CodigoLocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
					else if (ddllocalizacaoEncerramento.SelectedValue != " * Nenhum Selecionado * ")
						pd.CodigoLocalizacao = Convert.ToInt32(ddllocalizacaoEncerramento.SelectedValue);

					if (txtRet.Text != "0")
					{
						pd.QuantidadePendente = Convert.ToDecimal(txtRet.Text);
						pd.QuantidadePendente = pd.QuantidadePendente * -1;
					}
					else if (txtAdd.Text != "0")
						pd.QuantidadePendente = Convert.ToDecimal(txtAdd.Text);

					ItemDaComposicao i = new ItemDaComposicao();
					i = RnComp.PesquisarProdutoComRoteiro(Convert.ToInt32(txtComposto.Text),
						Convert.ToInt32(pd.CodigoProduto));
					foreach (ItemDaComposicao item in ListaRoteiro)
					{
						if (i.CodigoRoteiro == item.CodigoRoteiro)
						{
							if (item.DataInicio != null)
								pd.DataInicio = item.DataInicio;
							if (item.DataFim != null)
								pd.DataFim = item.DataFim;
						}
					}

					Habil_Estacao he = new Habil_Estacao();
					Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
					he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

					ep.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
					ep.Cpl_Usuario = Convert.ToInt32(Session["CodUsuario"]);

					Lista.Add(pd);
				}

				if (txtCodigo.Text == "Novo")
				{
					Session["MensagemTela"] = "Ordem de Produção Incluída com Sucesso!!!";
					ddlEmpresa_TextChanged(sender, e);
					ep.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Session["CodigoGeradorSequencialProd"]);
					ep.Cpl_NomeTabela = Session["NomeTabela"].ToString();

					Session["CodigoGeradorSequencialProd"] = null;
					RnOrd.SalvarOrdem(ep, Lista, EventoDocumento(), null, ep.Cpl_Maquina, ep.Cpl_Usuario);
				}
				else
				{
					Doc_OrdProducao p2 = new Doc_OrdProducao();
					ep.CodigoDocumento = Convert.ToInt64(txtCodigo.Text);

					p2 = RnOrd.PesquisarOrdem(Convert.ToDecimal(txtCodigo.Text));
					if (Convert.ToInt32(ddlSituacao.SelectedValue) != p2.CodigoSituacao)
						RnOrd.SalvarOrdem(ep, Lista, EventoDocumento(), ListaAnexo, ep.Cpl_Maquina, ep.Cpl_Usuario);
					else
						RnOrd.SalvarOrdem(ep, Lista, null, ListaAnexo, ep.Cpl_Maquina, ep.Cpl_Usuario);
				}

				p.Observacao = txtObsComposicao.Text;
				p.CodigoProdutoComposto = Convert.ToInt32(txtComposto.Text);
				RnComp.AtualizarObservacao(p);

			}
			catch (Exception ex)
			{
				ShowMessage(ex.ToString(), MessageType.Info);
				return 1;
			}

			if (txtCodigo.Text == "Novo")
				Session["MensagemTela"] = "Ordem de Produção Incluída com Sucesso!!!";
			else
				Session["MensagemTela"] = "Ordem de Produção Atualizada com Sucesso!!!";
			return 0;
		}
		protected int AtualizarSituacao(int Anterior, int Nova)
		{
			try
			{
				Doc_OrdProducaoDAL RnOrd = new Doc_OrdProducaoDAL();
				Doc_OrdProducao p = new Doc_OrdProducao();
				Habil_Estacao he = new Habil_Estacao();
				Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
				he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

				p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
				p.Cpl_Usuario = Convert.ToInt32(Session["CodUsuario"]);
				RnOrd.AtualizarSituacao(Convert.ToDecimal(txtCodigo.Text), Nova, Anterior, p.Cpl_Usuario, p.Cpl_Maquina);
			}
			catch (Exception)
			{
				ShowMessage("Não Foi Possível Atualizar a Situação da Ordem de Pedido", MessageType.Info);
				return 1;
			}
			return 0;
		} 
		protected void CamposGridDesabilitados(object sender, EventArgs e)
		{
			foreach (GridViewRow row in GridComponentes.Rows)
			{
				TextBox txtRet = (TextBox)row.FindControl("txtRet");
				TextBox txtAdd = (TextBox)row.FindControl("txtAdd");

				txtRet.Enabled = false;
				txtAdd.Enabled = false;
			}
		}
		protected void CalculaGrid()
		{
			List<ItemDaComposicao> Lista = new List<ItemDaComposicao>();
			List<ItemDaComposicao> ListaFim = new List<ItemDaComposicao>();
			List<ItemDaComposicao> ListaSum = new List<ItemDaComposicao>();

			if (Session["ListaComponentes"] != null)
			{
				if (Session["ListaComponentes"] != null)
				{
					Lista = (List<ItemDaComposicao>)Session["ListaComponentes"];
				}
			}
			else
			{
				AtualizaGridComponentes();
			}
			//Lista = Lista.Select(x => x.QuantidadeComponente * 1).FirstOrDefault();
			ShowMessage("Será Atualizada a Quantidade e Custo dos Componentes", MessageType.Info);
			foreach (ItemDaComposicao item in Lista)
			{
				ItemDaComposicao pd = new ItemDaComposicao();

				pd.CodigoRoteiro = item.CodigoRoteiro;
				pd.DescRoteiro = item.DescRoteiro;
				pd.CodigoComponente = item.CodigoComponente;
				pd.DescricaoComponente = item.DescricaoComponente;
				pd.Unidade = item.Unidade;
				pd.PerQuebraComponente = item.PerQuebraComponente;
				pd.QuantidadeComponente = Convert.ToDecimal((item.QuantidadeComponente * Convert.ToDecimal(txtQtPraProduzir.Text)).ToString("###,##0.00"));
				pd.QuantidadeUtil = item.QuantidadeUtil;
				pd.ValorCustoComponente = Convert.ToDecimal(item.ValorCustoComponente.ToString("###,##0.00"));
				pd.ValorTotal = Convert.ToDecimal((item.ValorCustoComponente * pd.QuantidadeComponente).ToString("###,##0.00"));

				ListaFim.Add(pd);
			}

			txtValor.Text = ListaFim.Sum(x => x.ValorTotal).ToString("###,##0.00");

			GridComponentes.DataSource = ListaFim;
			GridComponentes.DataBind();

		}
		protected void PreencheSession()
		{
			List<ItemDaComposicao> ListaItem = new List<ItemDaComposicao>();
			foreach (GridViewRow row in GridComponentes.Rows)
			{
				ItemDaComposicao p = new ItemDaComposicao();

				TextBox txtRet = (TextBox)row.FindControl("txtRet");
				TextBox txtAdd = (TextBox)row.FindControl("txtAdd");

				p.CodigoRoteiro = Convert.ToInt16(row.Cells[0].Text);
				p.DescRoteiro = row.Cells[1].Text;
				p.CodigoComponente = Convert.ToInt32(row.Cells[2].Text);
				p.DescricaoComponente = row.Cells[3].Text;
				p.Unidade = row.Cells[4].Text;
				p.PerQuebraComponente = Convert.ToDecimal(row.Cells[5].Text);
				p.QuantidadeComponente = Convert.ToDecimal(row.Cells[6].Text);
				p.QuantidadeUtil = Convert.ToDecimal(row.Cells[9].Text);
				p.ValorCustoComponente = Convert.ToDecimal(row.Cells[10].Text);
				p.ValorTotal = Convert.ToDecimal(row.Cells[11].Text);

				if (txtRet.Text != "0")
				{
					p.QuantidadeRet = Convert.ToDecimal(txtRet.Text);
					p.Quantidade = ((p.QuantidadeRet*-1) + p.QuantidadeComponente);
					p.ValorTotal = Convert.ToDecimal((p.Quantidade * p.ValorCustoComponente).ToString("###,##0.00"));
				}
				else if (txtAdd.Text != "0")
				{
					p.QuantidadeAdd = Convert.ToDecimal(txtAdd.Text);
					p.Quantidade = (p.QuantidadeAdd + p.QuantidadeComponente);
					p.ValorTotal = Convert.ToDecimal((p.Quantidade * p.ValorCustoComponente).ToString("###,##0.00"));
				}

				ListaItem.Add(p);
			}

			txtValor.Text = ListaItem.Sum(x => x.ValorTotal).ToString("###,##0.00");

			Session["ListaComponentes"] = ListaItem;
			GridComponentes.DataSource = ListaItem;
			GridComponentes.DataBind();
		}
		protected void PreencheSessionDepoisDeAdicionar()
		{
			ProdutoDocumentoDAL RnPd = new ProdutoDocumentoDAL();
			List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
			ListaProdutos = RnPd.ListarProdutosDaOrdemDeProducao(Convert.ToDecimal(txtCodigo.Text));

			List<ItemDaComposicao> ListaItem = new List<ItemDaComposicao>();

			foreach (ProdutoDocumento pr in ListaProdutos)
			{
				ItemDaComposicao ep = new ItemDaComposicao();
				ItemDaComposicao ap = new ItemDaComposicao();

				ep.CodigoRoteiro = pr.Cpl_CodigoRoteiro;
				ep.DescRoteiro = pr.Cpl_DescRoteiro;
				ep.CodigoComponente = pr.CodigoProduto;
				ep.DescricaoComponente = pr.Cpl_DscProduto;
				ep.Unidade = pr.Unidade;
				ep.PerQuebraComponente = pr.PerQuebraComponente;
				ep.QuantidadeComponente = pr.QuantidadeAtendida;
				ep.QuantidadeUtil = pr.QuantidadeAtendida;
				ep.ValorCustoComponente = pr.PrecoItem;
				ep.ValorTotal = pr.ValorTotalItem;

				ListaItem.Add(ep);
			}

			txtValor.Text = ListaItem.Sum(x => x.ValorTotal).ToString("###,##0.00");

			Session["ListaComponentes"] = ListaItem;
			GridComponentes.DataSource = ListaItem;
			GridComponentes.DataBind();
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
	}
}