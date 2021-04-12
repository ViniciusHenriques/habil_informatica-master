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
using DAL;

namespace SoftHabilInformatica.Pages.Compras
{
    public partial class ManCotPreco : System.Web.UI.Page
    {
		string modal = "";
		string mostrar = "show";
		public string PanelSelect { get; set; }
		public string Panels { get; set; }

		List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
        List<PessoaProdutoDocumento> ListaFornecedoresProdutos = new List<PessoaProdutoDocumento>();
        List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
		List<ProdutoDocumento> ListaProdutos2 = new List<ProdutoDocumento>();
		List<Habil_Log> ListaLog = new List<Habil_Log>();
        List<Pessoa> ListaFornecedores = new List<Pessoa>();

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

            btnEncerrar.Visible = false;
            btnSalvar.Visible = true;

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
            txtdtvalidade.Text = Hoje.AddDays(ListPar[0].DiasValidadeOrc).ToString("dd/MM/yyyy");
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
            ddlSituacao.DataSource = sd.SituacaoCotPreco();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
			ddlSituacao.DataBind();

            Doc_CotPreco p = new Doc_CotPreco();

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
                ShowMessage("Adicione itens a Cotação de Preço !", MessageType.Warning);
				txtProduto.Focus();
                return false;
            }

            if (GridFornecedor.Rows.Count < 0)
            {
                ShowMessage("Adicione Fornecedores a Cotação de Preço !", MessageType.Warning);
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
                                                "ConCotPreco.aspx");
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
				if (Session["ZoomCotPreco2"] == null)
					Session["Pagina"] = Request.CurrentExecutionFilePath;

				if (Session["ZoomCotPreco"] != null)
				{
					string s = Session["ZoomCotPreco"].ToString();
					Session["ZoomCotPreco"] = null;

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
                                Doc_CotPreco p = new Doc_CotPreco();
                                Doc_CotPrecoDAL docDAL = new Doc_CotPrecoDAL();
                                PessoaProdutoDocumentoDAL PpdDAL = new PessoaProdutoDocumentoDAL();

                                p = docDAL.PesquisarCotPreco(Convert.ToInt32(txtCodigo.Text));
								if (p == null)
								{
									Session["MensagemTela"] = "Esta Cotação de Preço  não existe";
									btnVoltar_Click(sender, e);
								}

								ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
								ddlEmpresa_TextChanged(sender, e);

								ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();

								Panels = "display:block";

								txtNroDocumento.Text = p.NumeroDocumento.ToString();
								txtNroSerie.Text = "";
								txtdtemissao.Text = p.DataHoraEmissao.ToString();
								txtdtvalidade.Text = p.DataValidade.ToString();
								txtObs.Text = p.DescricaoDocumento;
                                lblNrSolicitacao.Text = p.CodigoDocumentoOriginal.ToString();

                                if (lblNrSolicitacao.Text !="0")
                                {
                                    txtNrSolicitacao.Text= (new Doc_SolCompraDAL()).PesquisarSolCompra(Convert.ToInt32(lblNrSolicitacao.Text)).NumeroDocumento.ToString();
                                }
                                txtNrSolicitacao.Enabled = false;
                                btnPesSolicitacao.Enabled = false;

                                ListaProdutos = RnPd.ObterItemCotPreco(Convert.ToDecimal(txtCodigo.Text));
								Session["ProdutosCotacao"] = ListaProdutos;
								GridItemProdutos.DataSource = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList(); ;
								GridItemProdutos.DataBind();

                                ListaFornecedores = docDAL.ListarPessoasDocumento(Convert.ToInt32(txtCodigo.Text));
                                Session["NovoFornecedores"] = ListaFornecedores;
                                grdFornParticipantes.DataSource = ListaFornecedores;
                                grdFornParticipantes.DataBind();

                                ListaFornecedoresProdutos = PpdDAL.ListarPessoaProdutosDocumento(p.CodigoDocumento);
                                ListaFornecedoresProdutos = ListaFornecedoresProdutos.OrderBy(x => x.CodigoPessoa).OrderBy(x => x.CodigoProduto).ToList();
                                Session["NovoFornProdutos"] = ListaFornecedoresProdutos;
                                grdPrdCotacao.DataSource = ListaFornecedoresProdutos;
                                grdPrdCotacao.DataBind();

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
								Session["LogsCotPreco"] = ListaLog;
								grdLogDocumento.DataSource = ListaLog;
								grdLogDocumento.DataBind();

								AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
								ListaAnexo = anexo.ObterAnexos(Convert.ToInt32(txtCodigo.Text));
								Session["NovoAnexo"] = ListaAnexo;
								grdAnexo.DataSource = ListaAnexo;
								grdAnexo.DataBind();

                                if ((Convert.ToInt32(ddlSituacao.SelectedValue) == 206))
                                {
                                    btnEncerrar.Visible = true;
                                    btnSalvar.Visible = true;
                                    btnExcluir.Visible = true;

                                }
                                else
                                {
                                    btnEncerrar.Visible = false;
                                    btnSalvar.Visible = false;
                                    btnExcluir.Visible = false;

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
				Session["LogsCotPreco"] = ListaLog.OrderByDescending(x => x.DataHora).ToList();
				grdLogDocumento.DataSource = ListaLog;
				grdLogDocumento.DataBind();
			}
			if (Session["NovoAnexo"] != null)
			{
				ListaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];
				grdAnexo.DataSource = ListaAnexo;
				grdAnexo.DataBind();
			}

            if (Session["NovoFornecedores"] != null)
            {
                ListaFornecedores = (List<Pessoa>)Session["NovoFornecedores"];
                grdFornParticipantes.DataSource = ListaFornecedores;
                grdFornParticipantes.DataBind();
            }

            if (Session["ProdutosCotacao"] != null)
            {
                ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosCotacao"];
                GridItemProdutos.DataSource = ListaProdutos;
                GridItemProdutos.DataBind();
            }


            if (Session["NovoFornProdutos"] != null)
            {
                ListaFornecedoresProdutos = (List<PessoaProdutoDocumento>)Session["NovoFornProdutos"];
                grdPrdCotacao.DataSource = ListaFornecedoresProdutos;
                grdPrdCotacao.DataBind();
            }
        }
        //Buttons
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {

            ddlSituacao.SelectedValue = "207";

            if (!SalvarDocumento(sender, e))
                return;

            btnVoltar_Click(sender, e);
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            

        }
		protected void btnVoltar_Click(object sender, EventArgs e)
        {

            if (Session["IndicadorURL"] != null && Session["IndicadorURL"].ToString() == "1")
            {
                Session["IndicadorURL"] = null;
                Response.Redirect("~/Pages/Compras/LibCotPreco.aspx");
            }
            else
                Response.Redirect("~/Pages/Compras/ConCotPreco.aspx");


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



			if (Session["ProdutosCotacao"] != null)
				ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosCotacao"];
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
                        tabi.DsMarca = txtMarca.Text;

                        ListaProdutos2.Add(tabi);
					}
				}

				ListaProdutos = ListaProdutos2;
				Session["ProdutosCotacao"] = ListaProdutos;
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
                    ListaItem.DsMarca = txtMarca.Text;
                    ListaItem.Cpl_DescRoteiro = txtOBSItem.Text;
                    ListaItem.CodigoSituacao = 135;

					if (intEndItem != 0)
						ListaProdutos.RemoveAll(x => x.CodigoItem == intEndItem);

					ListaProdutos.Add(ListaItem);

					Session["ProdutosCotacao"] = ListaProdutos;
					GridItemProdutos.DataSource = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList(); ;
					GridItemProdutos.DataBind();
				}
			}

            AtualizaItensCotacao();


            LimpaCampos();
		}
		protected void BtnExcluirProduto_Click(object sender, EventArgs e)
		{

			List<ProdutoDocumento> ListaItem = new List<ProdutoDocumento>();


			if (txtProduto.Text == "")
				return;

            if (ExisteRespotaItensCotacao(0, Convert.ToInt32(txtProduto.Text)))
            {
                ShowMessage("Produto não pode ser Excluído!!! Existe Item de Cotação Respondido.", MessageType.Info);
                return;
            }
            else
                ExcluiItensCotacao(0, Convert.ToInt32(txtProduto.Text));

            if (Session["ProdutosCotacao"] != null)
				ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosCotacao"];
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

			Session["ProdutosCotacao"] = ListaProdutos;
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
            ListaGerDoc = gerDocDAL.ListarGeracaoSequencial("CD_TIPO_DOCUMENTO","INT","13", "VALIDADE DESC");

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
                        {
                            txtNomeFornecedor.Text = p.NomePessoa;
                            txtCidade.Text = p.Cpl_Municipio;
                            txtEstado.Text = p.Cpl_Estado;
                            txtFone.Text = p.Cpl_Fone;
                            txtEmail.Text = p.Cpl_Email;
                        }
                    }

				}
				else
				{
					txtFornecedor.Text = "";
					txtNomeFornecedor.Text = "";
                    txtEstado.Text = "";
                    txtFone.Text = "";
                    txtEmail.Text = "";
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
				if (Session["ProdutosCotacao"] != null)
					ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosCotacao"];
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
			if (Session["ProdutosCotacao"] != null)
				ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosCotacao"];
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
                Doc_SolCompraDAL RnSol = new Doc_SolCompraDAL();

                Doc_CotPreco p = new Doc_CotPreco();
				Doc_CotPrecoDAL RnCot = new Doc_CotPrecoDAL();
				Habil_Estacao he = new Habil_Estacao();
				Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
				he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

				p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
				p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
				p.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
				p.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
				p.DataValidade = Convert.ToDateTime(txtdtvalidade.Text);
				p.CodigoUsuario = Convert.ToInt32(ddlUsuario.SelectedValue);
				p.DescricaoDocumento = txtObs.Text;
                
                if (Session["ProdutosCotacao"] != null)
					ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosCotacao"];
					
				if (ListaProdutos.Count == 0)
				{
					ShowMessage("Deve conter pelo menos 1 Produto na Cotação de Preço ", MessageType.Info);
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

                    if ((lblNrSolicitacao.Text != "") && (txtNrSolicitacao.Enabled == false))
                    {
                        p.CodigoDocumentoOriginal = Convert.ToInt32 ( lblNrSolicitacao.Text);
                    }

                    RnCot.SalvarCotacao(p, ListaProdutos, EventoDocumento(), null, ListaFornecedores, ListaFornecedoresProdutos);

                    if ((lblNrSolicitacao.Text != "") && (txtNrSolicitacao.Enabled == false))
                    {
                        if (ddlSituacao.SelectedIndex == 207)
                            RnSol.AtualizaSituacaoSolicitacaoCompra(Convert.ToInt32(lblNrSolicitacao.Text), 202, p.Cpl_Usuario, p.Cpl_Maquina);
                        else
                            RnSol.AtualizaSituacaoSolicitacaoCompra(Convert.ToInt32(lblNrSolicitacao.Text), 204, p.Cpl_Usuario, p.Cpl_Maquina);
                    }
                    Session["MensagemTela"] = "Cotação de Preço Incluída com Sucesso!!!";
				}
				else
				{
					p.CodigoDocumento = Convert.ToInt64(txtCodigo.Text);
					p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
					p.Cpl_Usuario = Convert.ToInt32(Session["CodUsuario"]);

					if (Convert.ToInt32(ddlSituacao.SelectedValue) != p.CodigoSituacao)
						RnCot.SalvarCotacao(p, ListaProdutos, EventoDocumento(), ListaAnexo, ListaFornecedores, ListaFornecedoresProdutos);
					else
						RnCot.SalvarCotacao(p, ListaProdutos, null, ListaAnexo, ListaFornecedores, ListaFornecedoresProdutos);

                    if ((lblNrSolicitacao.Text != "") && (txtNrSolicitacao.Enabled == false))
                    {
                        if (ddlSituacao.SelectedValue.ToString() == "207")
                            RnSol.AtualizaSituacaoSolicitacaoCompra(Convert.ToInt32(lblNrSolicitacao.Text), 202, p.Cpl_Usuario, p.Cpl_Maquina);
                    }

                    Session["MensagemTela"] = "Cotação de Preço Atualizada com Sucesso!!!";
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
            txtOBSItem.Text = "";
            txtMarca.Text = "";
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
        protected void btnAddForn_Click(object sender, EventArgs e)
        {
            bool blnExiste = false;

            if (ListaFornecedores.Count >= 5)
            {
                ShowMessage("Máximo de Fornecedores são 5.", MessageType.Info);
                txtFornecedor.Text = "";
                txtNomeFornecedor.Text = "";
                txtCidade.Text = "";
                txtEstado.Text = "";
                txtFone.Text = "";
                txtEmail.Text = "";

                return;
            }
            if (txtFornecedor.Text != "")
            {
                foreach (var item in ListaFornecedores)
                {
                    if (Convert.ToInt32(txtFornecedor.Text) == item.CodigoPessoa)
                        blnExiste = true;
                }

                if (!blnExiste)
                {
                    Pessoa pe;

                    pe = (new PessoaDAL()).PesquisarFornecedor(Convert.ToInt32(txtFornecedor.Text));
                    if (pe != null)
                    {
                        ListaFornecedores.Add(pe);
                        Session["NovoFornecedores"] = ListaFornecedores;
                        grdFornParticipantes.DataSource = (ListaFornecedores);
                        grdFornParticipantes.DataBind();
                    }
                    else
                        ShowMessage("Código de Fornecedor não existente!!!", MessageType.Info);
                }

            }


            txtFornecedor.Text = "";
            txtNomeFornecedor.Text = "";
            txtCidade.Text = "";
            txtEstado.Text = "";
            txtFone.Text = "";
            txtEmail.Text = "";
            AtualizaItensCotacao();
        }
        protected void btnExcForn_Click(object sender, EventArgs e)
        {
            if (txtFornecedor.Text != "")
            {
                bool blnAchou = false;

                if (ExisteRespotaItensCotacao(Convert.ToInt32(txtFornecedor.Text),0))
                {
                    ShowMessage("Fornecedor não pode ser Excluído!!! Existe Item de Cotação Respondido.",MessageType.Info);
                    return;
                }
                else
                    ExcluiItensCotacao(Convert.ToInt32(txtFornecedor.Text), 0);


                foreach (var item in ListaFornecedores)
                {
                    if (Convert.ToInt32(txtFornecedor.Text) == item.CodigoPessoa)
                    {
                        ListaFornecedores.Remove(item);
                        blnAchou =true ;
                        goto Achou;
                    }

                }

                Achou:
                if (blnAchou)
                {
                    Session["NovoFornecedores"] = ListaFornecedores;
                    grdFornParticipantes.DataSource = (ListaFornecedores);
                    grdFornParticipantes.DataBind();

                    txtFornecedor.Text = "";
                    txtNomeFornecedor.Text = "";
                    txtCidade.Text = "";
                    txtEstado.Text = "";
                    txtFone.Text = "";
                    txtEmail.Text = "";
                }
            }

        }
        protected void grdFornParticipantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFornecedor.Text = HttpUtility.HtmlDecode(grdFornParticipantes.SelectedRow.Cells[0].Text);
            txtFornecedor_TextChanged(null, null);
        }
        protected void grdPrdCotacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFornec.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[4].Text);
            txtCodPrdFornecedor.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[2].Text);
            txtPrdFornec.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[1].Text);
            txtPrcCompra.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[5].Text);
            txtNomePrdFornec.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[3].Text);
            txtFornDtDiaEntrega.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[6].Text);
            txtFornObsFinanceira.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[7].Text);
            txtFornObsImposto.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[8].Text);

            if (txtFornObsImposto.Text.Trim() == "")
                txtFornObsImposto.Text = "ICMS.: .... -IPI.: .... -PIS.: .... -COFINS.: .... -ST.: .... -MVA.: .... -DIFAL.: ....";

            foreach (GridViewRow row in grdPrdCotacao.Rows)
            {
                System.Web.UI.WebControls.CheckBox chkBox = (System.Web.UI.WebControls.CheckBox)row.FindControl("chkTrabProd");
                if ((row.Cells[1].Text  == txtPrdFornec.Text) && (row.Cells[4].Text == txtFornec.Text))
                {
                    chkBoxNaoAtende.Checked = chkBox.Checked;
                }
            }




            txtFornDtResposta.Text = HttpUtility.HtmlDecode(grdPrdCotacao.SelectedRow.Cells[9].Text);
        }

        private void AtualizaItensCotacao()
        {


            if (Session["ProdutosCotacao"] != null)
                ListaProdutos = (List<ProdutoDocumento>)Session["ProdutosCotacao"];

            PessoaProdutoDocumento fPpd;
            foreach (Pessoa itemforn in ListaFornecedores)
            {
                foreach (ProdutoDocumento itemprd in ListaProdutos)
                {
                    //Find it!!!  
                   bool blnExiste = ListaFornecedoresProdutos.Any(x => (x.CodigoPessoa == itemforn.CodigoPessoa) && (x.CodigoProduto == itemprd.CodigoProduto));

                    if (!blnExiste)
                    {
                        Int32 intMax = 0;
                        if (ListaFornecedoresProdutos.Count != 0)
                        {
                            intMax = ListaFornecedoresProdutos.Max(x => x.CodigoItem);
                        }
                        intMax++;

                        fPpd = new PessoaProdutoDocumento();
                        fPpd.CodigoPessoa = itemforn.CodigoPessoa;
                        fPpd.CodigoProduto = itemprd.CodigoProduto;
                        fPpd.CodigoProdutoPessoa = "";
                        fPpd.Cpl_DscProduto = itemprd.Cpl_DscProduto;
                        fPpd.CodigoItem = intMax;
                        fPpd.Cpl_DscSituacao= "";
                        fPpd.CodigoDocumento = 0;
                        fPpd.PrecoItem = 0;
                        fPpd.CodigoSituacao = 1;
                        fPpd.OBSImposto = "";
                        fPpd.OBSFinanceira = "";
                        fPpd.DataDiaEntrega = "";
                        fPpd.NaoAtendeItem = 0;
                        fPpd.DataResposta = null;
                        fPpd.DataAprovacao = null;
                        ListaFornecedoresProdutos.Add(fPpd);
                    }

                }
            }

            ListaFornecedoresProdutos = ListaFornecedoresProdutos.OrderBy(x => x.CodigoPessoa).OrderBy(x => x.CodigoProduto).ToList();
            Session["NovoFornProdutos"] = ListaFornecedoresProdutos;
            grdPrdCotacao.DataSource = ListaFornecedoresProdutos;
            grdPrdCotacao.DataBind();
        }
        private bool  ExisteRespotaItensCotacao(Int64 CodPessoa, Int64 CodProduto)
        {
            List<PessoaProdutoDocumento> ListaFnfPrd = new List<PessoaProdutoDocumento>();
            if (CodPessoa != 0)
                ListaFnfPrd = ListaFornecedoresProdutos.Where(x => (x.CodigoPessoa == CodPessoa)).ToList();
            if (CodProduto != 0)
                ListaFnfPrd = ListaFornecedoresProdutos.Where(x => (x.CodigoProduto== CodProduto)).ToList();
            foreach (PessoaProdutoDocumento item in ListaFnfPrd)
            {
                if (item.DataResposta != null)
                    return true;
            }
            return false;
        }
        private void ExcluiItensCotacao(Int64 CodPessoa, Int64 CodProduto)
        {
            if (CodPessoa != 0)
                ListaFornecedoresProdutos.RemoveAll(x => x.CodigoPessoa == CodPessoa);
            if (CodProduto != 0)
                ListaFornecedoresProdutos.RemoveAll(x => x.CodigoProduto == CodProduto);

            ListaFornecedoresProdutos = ListaFornecedoresProdutos.OrderBy(x => x.CodigoPessoa).OrderBy(x => x.CodigoProduto).ToList();
            Session["NovoFornProdutos"] = ListaFornecedoresProdutos;
            grdPrdCotacao.DataSource = ListaFornecedoresProdutos;
            grdPrdCotacao.DataBind();
        }
        protected void BtnAddItemCotPreco_Click(object sender, EventArgs e)
        {
            txtPrcCompra_TextChanged(sender, e);

            if ((txtFornec.Text != "") && (txtPrdFornec.Text !=""))
                    {
                ListaFornecedoresProdutos.RemoveAll(x => (x.CodigoPessoa == Convert.ToInt32(txtFornec.Text)) && x.CodigoProduto == Convert.ToInt32(txtPrdFornec.Text));
                // Atualiza Itens

                PessoaProdutoDocumento fPpd;
                Int32 intMax = 0;
                if (ListaFornecedoresProdutos.Count != 0)
                {
                    intMax = ListaFornecedoresProdutos.Max(x => x.CodigoItem);
                }
                intMax++;

                txtCodPrdFornecedor.Text = txtCodPrdFornecedor.Text.ToUpper().Trim();

                fPpd = new PessoaProdutoDocumento();
                fPpd.CodigoPessoa = Convert.ToInt32(txtFornec.Text);
                fPpd.CodigoProdutoPessoa = txtCodPrdFornecedor.Text.ToUpper().Trim();
                fPpd.CodigoProduto = Convert.ToInt32(txtPrdFornec.Text);
                fPpd.Cpl_DscProduto = txtNomePrdFornec.Text;
                fPpd.CodigoItem = intMax;
                fPpd.Cpl_DscSituacao = "";
                fPpd.CodigoDocumento = 0;
                fPpd.PrecoItem = Convert.ToDecimal(txtPrcCompra.Text);
                fPpd.CodigoSituacao = 1;
                fPpd.OBSImposto = txtFornObsImposto.Text;
                fPpd.OBSFinanceira = txtFornObsFinanceira.Text;
                fPpd.DataDiaEntrega = txtFornDtDiaEntrega.Text;
                fPpd.DataAprovacao = null;

                if (chkBoxNaoAtende.Checked)
                    fPpd.NaoAtendeItem = 1;
                else
                    fPpd.NaoAtendeItem = 0;

                fPpd.DataResposta = (new DBTabelaDAL()).ObterDataHoraServidor();
                ListaFornecedoresProdutos.Add(fPpd);

                txtFornec.Text = "";
                txtPrdFornec.Text = "";
                txtCodPrdFornecedor.Text = "";
                txtNomePrdFornec.Text = "";
                txtPrcCompra.Text = "0,00";
                txtFornObsFinanceira.Text = "";
                txtFornObsImposto.Text = "";
                txtFornDtDiaEntrega.Text = "";
                txtFornDtResposta.Text = "";
                chkBoxNaoAtende.Checked = false;
                //

                ListaFornecedoresProdutos = ListaFornecedoresProdutos.OrderBy(x => x.CodigoPessoa).OrderBy(x => x.CodigoProduto).ToList();
                Session["NovoFornProdutos"] = ListaFornecedoresProdutos;
                grdPrdCotacao.DataSource = ListaFornecedoresProdutos;
                grdPrdCotacao.DataBind();

            }
        }
        protected void txtPrcCompra_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtPrcCompra.Text.Equals(""))
            {
                txtPrcCompra.Text = "0,00";
            }
            else
            {
                v.CampoValido("Preço de Compra", txtPrcCompra.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtPrcCompra.Text = Convert.ToDecimal(txtPrcCompra.Text).ToString("###,##0.00");
                }
                else
                {
                    txtPrcCompra.Text = "0,00";
                    return;
                }
            }

        }

        protected void btnPesSolicitacao_Click(object sender, EventArgs e)
        {
            Doc_SolCompraDAL RnProd = new Doc_SolCompraDAL();
            List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
            GridSolCompra.Enabled = true;

            DBTabelaCampos rowp2 = new DBTabelaCampos();
            rowp2.Filtro = "CD_SITUACAO";
            rowp2.Inicio = "202";
            rowp2.Fim = "202";
            rowp2.Tipo = "INT";
            listaT.Add(rowp2);

            GridSolCompra.DataSource = RnProd.ListarSolCompraCompleto(listaT);
            GridSolCompra.DataBind();

            AbrirCaixa(sender, e, "CaixaSolicitacao", mostrar);
        }

        protected void GridSolCompra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridSolCompra.Enabled)
            {
                txtNrSolicitacao.Text = HttpUtility.HtmlDecode(GridSolCompra.SelectedRow.Cells[0].Text);
                txtNrSolicitacao_TextChanged(null, null);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "CaixaSolicitacaoHide();", true);
                GridSolCompra.Enabled = false;

                lblNrSolicitacao.Text = HttpUtility.HtmlDecode(GridSolCompra.SelectedRow.Cells[0].Text);
                txtNrSolicitacao.Text = HttpUtility.HtmlDecode(GridSolCompra.SelectedRow.Cells[1].Text);
                txtNrSolicitacao.Enabled = false;
                btnPesSolicitacao.Enabled = false; 


            }

        }

        protected void txtNrSolicitacao_TextChanged(object sender, EventArgs e)
        {
            ProdutoDocumentoDAL RnPd = new ProdutoDocumentoDAL();
            List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
            Doc_SolCompra p = new Doc_SolCompra();
            Doc_SolCompraDAL docDAL = new Doc_SolCompraDAL();

            ListaProdutos = RnPd.ObterItemSolCompra(Convert.ToDecimal(txtNrSolicitacao.Text));
            Session["ProdutosCotacao"] = ListaProdutos;
            //ListaProdutos2 = ListaProdutos;

            GridItemProdutos.DataSource = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList(); ;
            GridItemProdutos.DataBind();


            p = docDAL.PesquisarSolCompra(Convert.ToInt32(txtNrSolicitacao.Text));
            if (p == null)
            {
                Session["MensagemTela"] = "Solicitação de Compra não Encontrada";
                return;
            }

            txtFornecedor.Text = Convert.ToString(p.CodigoFornecedor);
            txtFornecedor_TextChanged(null, null);
            btnAddForn_Click(null, null);
            


        }

        protected void btnEncerrarSim_Click(object sender, EventArgs e)
        {
            ddlSituacao.SelectedValue = "208";
            if (!SalvarDocumento(sender, e))
                return;
            btnVoltar_Click(sender, e);
        }

        protected void btnEnviarSim_Click(object sender, EventArgs e)
        {
            HabilEmailCriado Hec_Mail = new HabilEmailCriado();
            HabilEmailCriadoDAL Hec_Mail2 = new HabilEmailCriadoDAL();

            HabilEmailDestinatario Hec_MailDest = new HabilEmailDestinatario();
            HabilEmailAnexo Hec_MailAnexo = new HabilEmailAnexo();

            List<HabilEmailCriado> listMails = new List<HabilEmailCriado>();
            List<HabilEmailAnexo> listAnexos = new List<HabilEmailAnexo>();
            List<HabilEmailDestinatario> listDestinatarios = new List<HabilEmailDestinatario>();
            AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();

            foreach (var item in ListaFornecedores)
            {
                if (item.Cpl_Email.Contains("@"))
                {

                    try
                    {
                        Hec_Mail.CD_USU_REMETENTE = 0;
                        Hec_Mail.IN_HTML = 1;
                        Hec_Mail.TX_ASSUNTO = Session["NomeEmpresa"].ToString() + " - Cotação de Preço: Código " + txtCodigo.Text + @"\ N° " + txtNroDocumento.Text;

                        string strCorpo = "";

                        strCorpo = "<p>A/C <strong>[NOME DO CONTATO]</strong></p>" +
                        "<p><strong>[(99) 09999-9999]</strong></p>" +
                        "<p>&nbsp;</p>" +
                        "<p>Prezado Fornecedor.</p>" +
                        "<p><strong>[NOME DA EMPRESA]</strong></p>" +
                        "<p><strong>[ENDERECO] - [BAIRRO] - [CIDADE] - [UF]</strong></p>" +
                        "<p>&nbsp;</p>" +
                        "<p>Ol&aacute;.</p>" +
                        "<p>O sistema Gestão de Compras está enviando uma cotação de preços sobre determinados produtos.</p>" +
                        "<p> Clique aqui: &nbsp;<a href='[ENDERECO_SITE]'><span style='color:#2980b9'><strong>ACESSAR</strong></span></a>. para entrada no sistema e lançar os dados solicitados ou obter o arquivo em excel para preenchimento. </p>" +
                        "<p>&nbsp;</p>" +
                        "<table>" +
                        "	<tr>" +
                        "	<td>Usuário: </td><td><strong>[ACESSO DO USUARIO]</strong></td>" +
                        "	</tr>" +
                        "	<tr>" +
                        "	<td>&nbsp;</td><td>&nbsp;</td>" +
                        "	</tr>" +
                        "	<tr>" +
                        "	<td>Senha: </td><td><strong>[SENHA DO USUARIO]</strong></td>" +
                        "	</tr>" +
                        "</table>	" +
                        "<p>&nbsp;</p>" +
                        "<p>Muito Obrigado.</p>" +
                        "<p><strong>[EMPRESA EMITENTE]</strong> - Gestão de Compras.</p>" +
                        "<p>Saiba mais em:&nbsp;<a href='http://www.habilinformatica.com.br'><strong>www.habilinformatica.com.br</strong></a></p>" +
                        " ";

                        strCorpo = strCorpo.Replace("[NOME DO CONTATO]", "MARCIO CARDOSO MARTHA");
                        strCorpo = strCorpo.Replace("[(99) 09999-9999]", "(51) 99615-6020");
                        strCorpo = strCorpo.Replace("[NOME DA EMPRESA]", "HÁBIL SOLUÇÕES EM INFORMÁTICA LTDA");
                        strCorpo = strCorpo.Replace("[ENDERECO]", "RUA MARIO ALMEIDA FLORES, 96");
                        strCorpo = strCorpo.Replace("[BAIRRO]", "SÃO JOSÉ");
                        strCorpo = strCorpo.Replace("[CIDADE]", "CANOAS");
                        strCorpo = strCorpo.Replace("[UF]", "RS");
                        strCorpo = strCorpo.Replace("[ENDERECO_SITE]", "www.google.com");
                        strCorpo = strCorpo.Replace("[ACESSO DO USUARIO]", "MARCIO");
                        strCorpo = strCorpo.Replace("[SENHA DO USUARIO]", "MARTHA");
                        strCorpo = strCorpo.Replace("[EMPRESA EMITENTE]", "Hábil Informática LTDA");

                        Hec_Mail.TX_CORPO = strCorpo;

                        Hec_Mail.CD_SITUACAO = 113;  //Envia direto
                        Hec_Mail.CD_USU_REMETENTE = Convert.ToInt64(Session["CodUsuario"]);
                        listMails.Add(Hec_Mail);

                        Hec_MailDest = new HabilEmailDestinatario();
                        Hec_MailDest.CD_EMAIL_DESTINATARIO = 1;
                        Hec_MailDest.TP_DESTINATARIO = 1;
                        Hec_MailDest.NM_DESTINATARIO = item.NomePessoa;
                        Hec_MailDest.TX_EMAIL = item.Cpl_Email;

                        listDestinatarios.Add(Hec_MailDest);

                        listAnexos.Add(Hec_MailAnexo);

                        long longCodigoIndexEmail = 0;
                        Hec_Mail2.Gera_Email(listMails, listDestinatarios, listAnexos, ref longCodigoIndexEmail);
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex.Message, MessageType.Error);
                    }

                    ShowMessage("Envio de E-mails realizado com Sucesso!!!", MessageType.Info);
                    return;

                }
            } 
        }
    }
}