using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Windows.Forms;

namespace SoftHabilInformatica.Pages.Compras
{
    public partial class ManSolCompra : System.Web.UI.Page
    {
		string modal = "";
		string mostrar = "show";
		public string PanelSelect { get; set; }
		public string Panels { get; set; }

		List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
		List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
		List<ProdutoDocumento> ListaProdutos2 = new List<ProdutoDocumento>();
		List<Habil_Log> ListaLog = new List<Habil_Log>();

		clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
		
		//Geral
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaTela()
        {
            CarregaTiposSituacoes();
            txtCodigo.Text = "Novo";
            DBTabelaDAL RnTab = new DBTabelaDAL();

            btnEncaminhar.Visible = true;
            btnSalvar.Visible = true;
            txtMotivo.Text = "";
            txtMotivo.Visible = false;

            if (Session["CodEmpresa"] == null)
            {
                btnVoltar_Click(null,null);
                return;
            }

            List<ParSistema> ListPar = new List<ParSistema>();
            ParSistemaDAL ParDAL = new ParSistemaDAL();
            if (Session["VW_Par_Sistema"] == null)
                Session["VW_Par_Sistema"] = ParDAL.ListarParSistemas("CD_EMPRESA", "INT", Session["CodEmpresa"].ToString(), "");

            ListPar = (List<ParSistema>)Session["VW_Par_Sistema"];

            DateTime Hoje = RnTab.ObterDataHoraServidor();
            txtdtemissao.Text = Hoje.ToString("dd/MM/yyyy HH:mm:ss");
            txtdtValidade.Text = Hoje.AddDays(ListPar[0].DiasValidadeOrc).ToString("dd/MM/yyyy");
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
            if(Session["CodEmpresa"] != null)
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
			
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.SituacaoSolCompra();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
			ddlSituacao.DataBind();
			
			Doc_SolCompra p = new Doc_SolCompra();

			UsuarioDAL usu = new UsuarioDAL();
			ddlUsuario.DataSource = usu.ListarUsuarios("","","","");
			ddlUsuario.DataTextField = "NomeUsuario";
			ddlUsuario.DataValueField = "CodigoUsuario";
			ddlUsuario.DataBind();
			ddlUsuario.SelectedValue = Session["CodUsuario"].ToString();

			UnidadeDAL RnUnidade = new UnidadeDAL();
			ddlUnidade.DataSource = RnUnidade.ListarUnidades("", "", "", "");
			ddlUnidade.DataTextField = "SiglaUnidade";
			ddlUnidade.DataValueField = "CodigoUnidade";
			ddlUnidade.DataBind();

		}
		protected Boolean ValidaCampos()
        {
            //Boolean blnCampoValido = false;

            if (GridItemProdutos.Rows.Count < 0)
            {
                ShowMessage("Adicione itens a Solicitação de Compra!", MessageType.Warning);
				txtProduto.Focus();
                return false;
            }

            return true;
        }
		//Tela 
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
												"ConSolCompra.aspx");
			lista.ForEach(delegate (Permissao x)
			{
				if (!x.AcessoCompleto)
				{
					if (!x.AcessoAlterar)
					{
						btnSalvar.Visible = false;
					}
					if (!x.AcessoExcluir)
						btnExcluir.Visible = false;
				}
			});

			if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
			{
				if (Session["ZoomSolCompra2"] == null)
					Session["Pagina"] = Request.CurrentExecutionFilePath;

				if (Session["ZoomSolCompra"] != null)
				{
					string s = Session["ZoomSolCompra"].ToString();
					Session["ZoomSolCompra"] = null;

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

								PanelSelect = "aba1";
								Session["TabFocada"] = "aba1";
								Session["Atualizar"] = "";

								ProdutoDocumentoDAL RnPd = new ProdutoDocumentoDAL();
								List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
								Doc_SolCompra p = new Doc_SolCompra();
								Doc_SolCompraDAL docDAL = new Doc_SolCompraDAL();

								p = docDAL.PesquisarSolCompra(Convert.ToInt32(txtCodigo.Text));
								if (p == null)
								{
									Session["MensagemTela"] = "Esta Solicitação de Compra não existe";
									btnVoltar_Click(sender, e);
								}

								ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
								ddlEmpresa_TextChanged(sender, e);

								ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();

								Panels = "display:block";

								txtNroDocumento.Text = p.NumeroDocumento.ToString();
								txtNroSerie.Text = "";
								txtdtemissao.Text = p.DataHoraEmissao.ToString();
								txtdtValidade.Text = p.DataValidade.ToString();
								txtFornecedor.Text = Convert.ToString(p.CodigoFornecedor);
								txtObs.Text = p.DescricaoDocumento;
								txtFornecedor_TextChanged(sender, e);
								txtValorTotal.Text = p.ValorTotal.ToString("###,##0.00");
                                txtMotivo.Text = p.MotivoCancelamento; 

								ListaProdutos = RnPd.ObterItemSolCompra(Convert.ToDecimal(txtCodigo.Text));
								Session["ProdutosSolicitacao"] = ListaProdutos;
                                //ListaProdutos2 = ListaProdutos;

                                GridItemProdutos.DataSource = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList(); ;
								GridItemProdutos.DataBind();

								
								PanelSelect = "aba1";
								Session["TabFocada"] = "aba1";

								EventoDocumentoDAL eve = new EventoDocumentoDAL();
								ListaEvento = eve.ObterEventos(Convert.ToInt64(txtCodigo.Text));
								Session["Eventos"] = ListaEvento;
								GrdEventoDocumento.DataSource = ListaEvento;
								GrdEventoDocumento.DataBind();

								Habil_LogDAL log = new Habil_LogDAL();
								ListaLog = log.ListarLogs(Convert.ToDouble(txtCodigo.Text), 100);
								ListaLog= ListaLog.OrderByDescending(x => x.DataHora).ToList();
								Session["LogsSolCompra"] = ListaLog;
								grdLogDocumento.DataSource = ListaLog;
								grdLogDocumento.DataBind();

								AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
								ListaAnexo = anexo.ObterAnexos(Convert.ToInt32(txtCodigo.Text));
								Session["NovoAnexo"] = ListaAnexo;
								grdAnexo.DataSource = ListaAnexo;
								grdAnexo.DataBind();

                                if ((Convert.ToInt32(ddlSituacao.SelectedValue) == 200))
                                {
                                    btnEncCompra.Visible = true;
                                    btnSalvar.Visible = true;
                                    btnExcluir.Visible = true;
                                    txtMotivo.Visible = true;

                                }
                                else
                                {
                                    btnEncCompra.Visible = false;
                                    btnSalvar.Visible = false;
                                    btnExcluir.Visible = false;
                                    txtMotivo.Enabled = false;
                                    txtMotivo.Visible = true;

                                }
                            }

                        }
					
					}
				}
				else
				{
					LimpaTela();

					ddlEmpresa_TextChanged(sender, e);
					btnExcluir.Visible = false;

					if (Session["CodEmpresa"] != null)
						ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
					
					lista.ForEach(delegate (Permissao p)
					{
						if (!p.AcessoCompleto)
						{
							if (!p.AcessoIncluir)
							{
								btnSalvar.Visible = false;
							}
							if (!p.AcessoExcluir)
								btnExcluir.Visible = false;
						}
					});
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
				ListaLog = ListaLog.OrderByDescending(x => x.DataHora).ToList();
				Session["LogsSolCompra"] = ListaLog.OrderByDescending(x => x.DataHora).ToList();
				grdLogDocumento.DataSource = ListaLog;
				grdLogDocumento.DataBind();
			}
			if (Session["NovoAnexo"] != null)
			{
				ListaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];
				grdAnexo.DataSource = ListaAnexo;
				grdAnexo.DataBind();
			}
		}
		//Buttons
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {

            if (txtMotivo.Text == "")
            {
                ShowMessage("Motivo do Cancelamento não foi informado!!!", MessageType.Warning);
                return;
            }

            ddlSituacao.SelectedValue = "203";

            if (!SalvarDocumento(sender, e))
                return;

            btnVoltar_Click(sender, e);

//            Doc_SolCompraDAL doc = new Doc_SolCompraDAL();
//			doc.Excluir(Convert.ToDecimal(txtCodigo.Text));
//          Session["MensagemTela"] = "Documento Excluído com sucesso!";
//            btnVoltar_Click(sender, e);



        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            

        }
		protected void btnVoltar_Click(object sender, EventArgs e)
        {

            if (Session["IndicadorURL"] != null && Session["IndicadorURL"].ToString() == "1")
            {
                Session["IndicadorURL"] = null;
                Response.Redirect("~/Pages/Compras/LibSolCompra.aspx");
            }
            else
                Response.Redirect("~/Pages/Compras/ConSolCompra.aspx");


        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
			if (!SalvarDocumento(sender, e))
				return;

			btnVoltar_Click(sender, e);
        }
		protected void btnFornecedor_Click(object sender, EventArgs e)
		{
			PessoaDAL RnPss = new PessoaDAL();
			GridFornecedor.DataSource = RnPss.ListarFornecedor("",0);
			GridFornecedor.DataBind();
			GridFornecedor.Enabled = true;
			AbrirCaixa(sender, e, "CaixaFornecedor", mostrar);
		}
		protected void btnProduto_Click(object sender, EventArgs e)
		{
			ProdutoDAL RnProd = new ProdutoDAL();

            GridProdutos.Enabled = true;

            GridProdutos.DataSource = RnProd.ListarProdutosPesquisa(""); 
			GridProdutos.DataBind();

			AbrirCaixa(sender, e, "CaixaProduto", mostrar);
		}
		protected void BtnAddProduto_Click(object sender, EventArgs e)
		{

			if (txtProduto.Text == "")
				return;
			if (txtQtde.Text == "0,00")
				return;



			if (Session["ProdutosSolicitacao"] != null)
				ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosSolicitacao"];
			else
				ListaProdutos = new List<ProdutoDocumento>();

			if (Session["ManutencaoProduto"] != null)
			{
				//alterar produto existente
				ProdutoDocumento produto = (ProdutoDocumento)Session["ManutencaoProduto"];
				Session["ManutencaoProduto"] = null;

				ListaProdutos2  = new List<ProdutoDocumento>();

				ProdutoDocumento tabi;
				foreach (ProdutoDocumento item in ListaProdutos)
				{
					if (item.CodigoItem != produto.CodigoItem)
					{
						tabi = new ProdutoDocumento();
						tabi.CodigoItem = item.CodigoItem;
						tabi.CodigoDocumento = item.CodigoDocumento;
						tabi.Unidade = item.Unidade;
						tabi.Cpl_DscProduto = item.Cpl_DscProduto;
						tabi.CodigoProduto = item.CodigoProduto;
						tabi.Quantidade = item.Quantidade;
                        tabi.Cpl_DescRoteiro = item.Cpl_DescRoteiro;
                        tabi.DsMarca = item.DsMarca;

                        ListaProdutos2.Add(tabi);
					}
					else
					{
						tabi = new ProdutoDocumento();
						tabi.CodigoItem = item.CodigoItem;
						tabi.CodigoProduto = Convert.ToInt32(txtProduto.Text);
						tabi.Unidade = ddlUnidade.SelectedItem.Text;
						tabi.Cpl_DscProduto = Convert.ToString(txtDescProduto.Text);
						tabi.Quantidade = Convert.ToDecimal(txtQtde.Text);
                        tabi.Cpl_DescRoteiro = txtOBSItem.Text;
                        tabi.DsMarca = txtMarca.Text ;
                        ListaProdutos2.Add(tabi);
					}
				}

				ListaProdutos = ListaProdutos2;
				Session["ProdutosSolicitacao"] = ListaProdutos;
				GridItemProdutos.DataSource = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList();
				GridItemProdutos.DataBind();


			}
			else
			{
				if (txtProduto.Text != "")
				{
					//inserir produto novo
					int intEndItem = 0;
					ProdutoDocumentoDAL r = new ProdutoDocumentoDAL();
					List<DBTabelaCampos> lista = new List<DBTabelaCampos>();

					if (ListaProdutos.Count != 0)
						intEndItem = Convert.ToInt32(ListaProdutos.Max(x => x.CodigoItem).ToString());

					intEndItem = intEndItem + 1;

					ProdutoDocumento ListaItem = new ProdutoDocumento();
					ListaItem.CodigoItem = intEndItem;
					ListaItem.Unidade = ddlUnidade.SelectedItem.Text;
					ListaItem.CodigoProduto = Convert.ToInt32(txtProduto.Text);
					ListaItem.Cpl_DscProduto = txtDescProduto.Text;
					ListaItem.Quantidade = Convert.ToDecimal(txtQtde.Text);
					ListaItem.CodigoSituacao = 135;
                    ListaItem.Cpl_DescRoteiro = txtOBSItem.Text;
                    ListaItem.DsMarca = txtMarca.Text;


                    if (intEndItem != 0)
						ListaProdutos.RemoveAll(x => x.CodigoItem == intEndItem);

					ListaProdutos.Add(ListaItem);

					Session["ProdutosSolicitacao"] = ListaProdutos;
					GridItemProdutos.DataSource = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList(); ;
					GridItemProdutos.DataBind();
				}
			}
			LimpaCampos();
		}
		protected void BtnExcluirProduto_Click(object sender, EventArgs e)
		{

			List<ProdutoDocumento> ListaItem = new List<ProdutoDocumento>();


			if (txtProduto.Text == "")
				return;

			if (Session["ProdutosSolicitacao"] != null)
				ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosSolicitacao"];
			else
				ListaProdutos = new List<ProdutoDocumento>();

			ProdutoDocumento tabi;
			foreach (ProdutoDocumento item in ListaProdutos)
			{
				tabi = new ProdutoDocumento();
				tabi.CodigoItem = item.CodigoItem;
				tabi.CodigoDocumento = item.CodigoDocumento;
				tabi.Unidade = item.Unidade;
				tabi.Cpl_DscProduto = item.Cpl_DscProduto;
				tabi.CodigoProduto = item.CodigoProduto;
				tabi.Quantidade = item.Quantidade;
				tabi.QuantidadeAtendida = item.QuantidadeAtendida;
				tabi.PrecoItem = item.PrecoItem;
				tabi.ValorDesconto = item.ValorDesconto;
				tabi.ValorTotalItem = item.ValorTotalItem;
				tabi.Cpl_DscSituacao = item.Cpl_DscSituacao;
				tabi.ValorFatorCubagem = item.ValorFatorCubagem;
				tabi.ValorPeso = item.ValorPeso;
				tabi.ValorVolume = item.ValorVolume;

				if (item.CodigoItem == Convert.ToInt32(txtProduto.Text))
					tabi.CodigoSituacao = 134;
				else
					tabi.CodigoSituacao = item.CodigoSituacao;

				ListaItem.Add(tabi);
			}

			BtnExcluirProduto.Visible = false;

			Session["ListaItemDocumento"] = ListaItem;
			Session["ManutencaoProduto"] = null;		

			ListaProdutos.RemoveAll(x => x.CodigoProduto == Convert.ToInt32(txtProduto.Text));

			Session["ProdutosSolicitacao"] = ListaProdutos;
			GridItemProdutos.DataSource = ListaProdutos;
			GridItemProdutos.DataBind();
			LimpaCampos();
		}
		//DropDownList
		protected void ddlEmpresa_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "Novo")
                return;
            
            DBTabelaDAL db = new DBTabelaDAL();
            List<GeracaoSequencialDocumento> ListaGerDoc = new List<GeracaoSequencialDocumento>();
            GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
            GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
            ListaGerDoc = gerDocDAL.ListarGeracaoSequencial("CD_TIPO_DOCUMENTO","INT","12", "VALIDADE DESC");

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
		//TextBox
		protected void txtFornecedor_TextChanged(object sender, EventArgs e)
		{
			Boolean blnCampo = false;

			if (txtFornecedor.Text.Equals(""))
			{
				txtFornecedor.Text = "0";
				txtNomeFornecedor.Text = "";
			}
			else
			{
				v.CampoValido("Fornecedor", txtFornecedor.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
				if (blnCampo)
				{
					Pessoa p = new Pessoa();
					PessoaDAL RnPess = new PessoaDAL();

					txtNomeFornecedor.Text = "";
					p = RnPess.PesquisarFornecedor(Convert.ToInt64(txtFornecedor.Text));

					if (p != null)
					{
						if (p.CodigoSituacaoPessoa == 1)
							txtNomeFornecedor.Text = p.NomePessoa;
					}

				}
				else
				{
					txtFornecedor.Text = "";
					txtNomeFornecedor.Text = "";
				}
			}
					
		}
		protected void txtProduto_TextChanged(object sender, EventArgs e)
		{

			Boolean blnCampo = false;
			if (txtProduto.Text.Equals(""))
			{
				return;
			}
			else
			{
				v.CampoValido("Codigo Produto", txtProduto.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
				if (!blnCampo)
				{
					txtProduto.Text = "";
					return;
				}
			}

			Int64 codigoItem = Convert.ToInt64(txtProduto.Text);
			ProdutoDAL produtoDAL = new ProdutoDAL();
			Produto produto = new Produto();
			produto = produtoDAL.PesquisarProduto(codigoItem);//PESQUISAR PRODUTO DO TIPO PRODUTO ACABADO
			txtQtde.Focus();

			if (produto != null)
			{
				if (Session["ProdutosSolicitacao"] != null)
					ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosSolicitacao"];
				else
					ListaProdutos = new List<ProdutoDocumento>();

				var ItemExiste = ListaProdutos.Where(x => x.CodigoProduto == produto.CodigoProduto);
				if (ItemExiste.Count() > 0)
				{
					LimpaCampos();
					txtProduto.Focus();
					ShowMessage("Este produto já consta na Lista", MessageType.Info);
					return;
				}
				if (produto.CodigoSituacao == 1)
				{
					txtDescProduto.Text = produto.DescricaoProduto;
					ddlUnidade.SelectedValue = produto.CodigoUnidade.ToString();
                    txtMarca.Text = produto.DsMarca;
					txtQtde.Focus();
				}
				else
				{
					LimpaCampos();
					txtProduto.Focus();
					ShowMessage("Produto com Situação Inativa!", MessageType.Info);
				}
			}
			else
			{
				LimpaCampos();
				txtProduto.Focus();
				ShowMessage("Este produto não cadastrado.", MessageType.Info);
			}
			Session["TabFocada"] = null;
		}
		protected void txtPesquisaProduto_TextChanged(object sender, EventArgs e)
		{
			if (txtPesquisaProduto.Text.Length < 2)
			{
				Label2.Text = "Minimo de 3 Caracteres...";
				txtPesquisaProduto.Text = "";
			}
			ProdutoDAL RnProd = new ProdutoDAL();
			GridProdutos.DataSource = RnProd.ListarProdutosPesquisa(txtPesquisaProduto.Text);
			GridProdutos.DataBind();
			AbrirCaixa(sender, e, "CaixaProduto", mostrar);
		}
		protected void txtPesquisaFornecedor_TextChanged(object sender, EventArgs e)
		{
			if(txtPesquisaFornecedor.Text.Length < 2)
			{
				Label1.Text = "Minimo de 3 Caracteres...";
				txtPesquisaFornecedor.Text = "";
			}
			PessoaDAL RnPss = new PessoaDAL();
			GridFornecedor.DataSource = RnPss.ListarFornecedor(txtPesquisaFornecedor.Text,0);
			GridFornecedor.DataBind();
			AbrirCaixa(sender, e, "CaixaFornecedor", mostrar);
		}
		//Grids
		protected void GridProdutos_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (GridProdutos.Enabled)
            {
                txtProduto.Text = HttpUtility.HtmlDecode(GridProdutos.SelectedRow.Cells[0].Text);
                txtProduto_TextChanged(null, null);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "CaixaProdutoHide();", true);
                GridProdutos.Enabled = false;
            }

        }
		protected void GridFornecedor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (GridFornecedor.Enabled)
			{
				txtFornecedor.Text = HttpUtility.HtmlDecode(GridFornecedor.SelectedRow.Cells[0].Text);
				txtFornecedor_TextChanged(null, null);
				ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "CaixaFornecedorHide();", true);
				GridFornecedor.Enabled = false;
			}
		}
		protected void GridItemProdutos_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Session["ProdutosSolicitacao"] != null)
				ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosSolicitacao"];
			else
				ListaProdutos = new List<ProdutoDocumento>();

			foreach (ProdutoDocumento item in ListaProdutos)
			{
				if (item.CodigoItem == Convert.ToInt32(HttpUtility.HtmlDecode(GridItemProdutos.SelectedRow.Cells[0].Text)))
				{
					Session["ManutencaoProduto"] = item;
					txtProduto.Text = item.CodigoProduto.ToString();
					txtQtde.Text = item.Quantidade.ToString("###,##0.00");
                    txtMarca.Text = item.DsMarca;
					txtDescProduto.Text = item.Cpl_DscProduto;
                    txtOBSItem.Text = item.Cpl_DescRoteiro;

                    BtnExcluirProduto.Visible = true;
				}
			}
		}
		//Criadas
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
		protected bool  SalvarDocumento(object sender, EventArgs e)
		{

			try
			{
				Doc_SolCompra p = new Doc_SolCompra();
				Doc_SolCompraDAL RnSol = new Doc_SolCompraDAL();
				Habil_Estacao he = new Habil_Estacao();
				Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
				he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

				if (txtFornecedor.Text != "")
					p.CodigoFornecedor = Convert.ToInt32(txtFornecedor.Text);

				p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
				p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
				p.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
				p.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
				p.DataValidade = Convert.ToDateTime(txtdtValidade.Text);
				p.CodigoUsuario = Convert.ToInt32(ddlUsuario.SelectedValue);
				p.DescricaoDocumento = txtObs.Text;
				p.ValorTotal = Convert.ToDecimal(txtValorTotal.Text);
                p.MotivoCancelamento = txtMotivo.Text;
                
                if (Session["ProdutosSolicitacao"] != null)
					ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosSolicitacao"];
					
				if (ListaProdutos.Count == 0)
				{
					ShowMessage("Deve conter pelo menos 1 Produto na Solicitação de Compra", MessageType.Info);
					return false;
				}

				if (txtCodigo.Text == "Novo")
				{
					ddlEmpresa_TextChanged(sender, e);
					p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Session["CodigoGeradorSequencial"]);
					p.Cpl_NomeTabela  = Session["NomeTabela"].ToString();

					Session["CodigoGeradorSequencial"] = null;

					p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
					p.Cpl_Usuario = Convert.ToInt32(Session["CodUsuario"]);

					RnSol.SalvarSolicitacao(p, ListaProdutos, EventoDocumento(), null);

					Session["MensagemTela"] = "Solicitação de Compra Incluída com Sucesso!!!";
				}
				else
				{
					p.CodigoDocumento = Convert.ToInt64(txtCodigo.Text);
					p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
					p.Cpl_Usuario = Convert.ToInt32(Session["CodUsuario"]);

					if (Convert.ToInt32(ddlSituacao.SelectedValue) != p.CodigoSituacao)
						RnSol.SalvarSolicitacao(p, ListaProdutos, EventoDocumento(), ListaAnexo);
					else
						RnSol.SalvarSolicitacao(p, ListaProdutos, null, ListaAnexo);

					Session["MensagemTela"] = "Solicitação de Compra Atualizada com Sucesso!!!";
				}
				return true;
			}
			catch (Exception ex)
			{
				ShowMessage(ex.Message, MessageType.Error);
				return false;
			}
		}
		public void AbrirCaixa(object sender, EventArgs e, string modalS, string mostrarS)
		{
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "FloatingBox('" + modalS + "','" + mostrarS + "');", true);
		}
		public void LimpaCampos()
		{
			txtProduto.Text = "";
			txtDescProduto.Text = "";
			txtQtde.Text = "0,00";
            txtMarca.Text = "";
            txtOBSItem.Text = "";
        }
        protected void btnNovoAnexo_Click(object sender, EventArgs e)
		{

		}
		protected void txtQtde_TextChanged(object sender, EventArgs e)
		{
			Boolean blnCampo = false;

			if (txtQtde.Text.Equals(""))
			{
				txtQtde.Text = "0,00";
			}
			else
			{
				v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
				if (blnCampo)
				{
					txtQtde.Text = Convert.ToDecimal(txtQtde.Text).ToString("###,##0.00");
				}
				else
				{
					txtQtde.Text = "0,00";
					return;
				}
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
				v.CampoValido("Valor Total da Verba", txtValorTotal.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
				if (blnCampo)
				{
					txtValorTotal.Text = Convert.ToDecimal(txtValorTotal.Text).ToString("###,##0.00");
				}
				else
				{
					txtValorTotal.Text = "0,00";
					return;
				}
			}


		}
        protected void btnEncaminhar_Click(object sender, EventArgs e)
        {
            ddlSituacao.SelectedValue  = "201"; 

            if (!SalvarDocumento(sender, e))
                return;

            btnVoltar_Click(sender, e);

        }
    }
}