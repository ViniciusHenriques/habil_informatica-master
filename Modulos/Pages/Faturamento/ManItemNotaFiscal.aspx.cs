using System;
using System.Windows;
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


namespace SoftHabilInformatica.Pages.Faturamento
{
    public partial class ManItemNotaFiscal: System.Web.UI.Page
    {
        public string PanelSelect { get; set; }

        List<ProdutoDocumento> ListaItemDocumento = new List<ProdutoDocumento>();

        clsValidacao v = new clsValidacao();

        String strMensagemR = "";

        public enum MessageType { Success, Error, Info, Warning };

        protected void CarregaTiposSituacoes()
        {
            UnidadeDAL RnUnidade = new UnidadeDAL();
            ddlUnidade.DataSource = RnUnidade.ListarUnidades("", "", "", "");
            ddlUnidade.DataTextField = "SiglaUnidade";
            ddlUnidade.DataValueField = "CodigoUnidade";
            ddlUnidade.DataBind();
        }       

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void LimpaTela()
        {
            txtCodItem.Text = "";
            txtDescricao.Text = "";
            txtPreco.Text = "0,00";
            txtQtde.Text = "0,00";
            txtVlrTotalItem.Text = "0,00";
            txtDesconto.Text = "0,00";
        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código Item", txtCodItem.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodItem.Focus();
                }
                return false;
            }
            Produto prod = new Produto();
            ProdutoDAL prodDAL = new ProdutoDAL();
            prod = prodDAL.PesquisarProduto(Convert.ToInt64(txtCodItem.Text));
            if(prod == null)
            {
                ShowMessage("Este Produto não existe!", MessageType.Info);
                return false;
            }

            v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtQtde.Focus();
                }
                return false;
            }
            else
            {
                if (Convert.ToDouble(txtQtde.Text) == 0)
                {
                    ShowMessage("Digite a Quantidade!", MessageType.Info);
                    txtQtde.Focus();
                    return false;
                }
            }

            v.CampoValido("Preço", txtPreco.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtPreco.Focus();
                }
                return false;
            }
            else
            {
                if (Convert.ToDouble(txtPreco.Text) == 0)
                {
                    ShowMessage("Digite o preço!", MessageType.Info);
                    txtPreco.Focus();
                    return false;
                }
            }

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    CarregaTiposSituacoes();
                    if (Session["ZoomProduto2"] == null)
                        Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomProduto"] != null)
                    {
                        string s = Session["ZoomProduto"].ToString();
                        Session["ZoomProduto"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            foreach (string word in words)
                            {
                                if (word != "")
                                {
                                    txtCodItem.Text = word;
                                    txtCodItem_TextChanged(sender, e);
                                }
                            }
                        }
                    }
                    else
                    {
                        LimpaTela();
                    }

                }
                else
                {                  
                    if (Session["Doc_NotaFiscal"] == null )
                    {
                        btnVoltar_Click(sender, e);
                    }
                }

                if (Session["ListaItemNF"] != null)
                {
                    ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemNF"];
                    grdProduto.DataSource = ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList();
                    grdProduto.DataBind();
                }

                MontarValorTotal(ListaItemDocumento);
                CamposProduto();

                if (!IsPostBack)
                    txtCodItem.Focus();
                if (ddlUnidade.Items.Count == 0)
                    btnVoltar_Click(sender, e);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["TabFocadaNF"] = "aba4";
            Session["ManutencaoProduto"] = null;
            Response.Redirect("~/Pages/Faturamento/ManNotaFiscal.aspx");
        }

        protected void MontarValorTotal(List<ProdutoDocumento> ListaItens)
        {
            decimal ValorTotal = 0;
            txtVlrTotal.Text = Convert.ToDouble("0,00").ToString("F");

            long CodigoPessoa = 0;
            int CodigoTipoOperacao = 0;
            decimal decValorFrete = 0;
            long CodigoEmpresa = 0;
            int CodigoAplicacaoUso = 0;

            if (Session["Doc_orcamento"] != null)
            {
                Doc_Orcamento doc = new Doc_Orcamento();
                doc = (Doc_Orcamento)Session["Doc_orcamento"];
                CodigoPessoa = doc.Cpl_CodigoPessoa;
                CodigoTipoOperacao = 1;
                decValorFrete = doc.ValorFrete;
                CodigoEmpresa = doc.CodigoEmpresa;
                CodigoAplicacaoUso = doc.CodigoAplicacaoUso;
            }
            else if (Session["Doc_Pedido"] != null)
            {
                Doc_Pedido doc = new Doc_Pedido();
                doc = (Doc_Pedido)Session["Doc_Pedido"];
                CodigoPessoa = doc.Cpl_CodigoPessoa;
                CodigoTipoOperacao = doc.CodigoTipoOperacao;
                decValorFrete = doc.ValorFrete;
                CodigoEmpresa = doc.CodigoEmpresa;
                CodigoAplicacaoUso = doc.CodigoAplicacaoUso;
            }
            else
            {
                return;
            }

            List<ProdutoDocumento> NovaListaItens = new List<ProdutoDocumento>();

            decimal decValorFreteRatiado = 0;
            decimal decDiferencaValorFreteRatiado = 0;

            if (decValorFrete > 0)
                decValorFreteRatiado = decValorFrete / ListaItens.Where(x => x.CodigoSituacao != 134).Count();
            
            if((decValorFreteRatiado * ListaItens.Where(x => x.CodigoSituacao != 134).Count()) != decValorFrete)
            {
                decDiferencaValorFreteRatiado = decValorFrete - (decValorFreteRatiado * ListaItens.Where(x => x.CodigoSituacao != 134).Count());
            }
            

            foreach (ProdutoDocumento itens in ListaItens)
            {              
                if (itens.CodigoSituacao != 134)
                {
                    if (decDiferencaValorFreteRatiado != 0)
                    {
                        itens.Impostos = ImpostoProdutoDocumentoDAL.PreencherImpostosProdutoDocumento(itens,CodigoEmpresa, CodigoPessoa, CodigoTipoOperacao,CodigoAplicacaoUso, decValorFreteRatiado - (decDiferencaValorFreteRatiado), false);
                        decDiferencaValorFreteRatiado = 0;
                    }
                    else
                        itens.Impostos = ImpostoProdutoDocumentoDAL.PreencherImpostosProdutoDocumento(itens, CodigoEmpresa, CodigoPessoa, CodigoTipoOperacao, CodigoAplicacaoUso, decValorFreteRatiado, false);

                    ValorTotal += itens.ValorTotalItem;
                }
                NovaListaItens.Add(itens);
            }

            txtVlrTotal.Text = Convert.ToString(ValorTotal + decValorFrete);
            txtVlrTotal.Text = Convert.ToDouble(txtVlrTotal.Text).ToString("F");
        }

        protected void grdProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ProdutoDocumento item in ListaItemDocumento)
            {
                if (item.CodigoItem == Convert.ToInt32(HttpUtility.HtmlDecode(grdProduto.SelectedRow.Cells[0].Text)))
                {
                    Session["ManutencaoProduto"] = item;
                    txtCodItem.Text = item.CodigoProduto.ToString();
                    txtQtde.Text = item.Quantidade.ToString("###,##0.00");
                    txtPreco.Text = item.PrecoItem.ToString("###,##0.00");
                    txtDesconto.Text = item.ValorDesconto.ToString("###,##0.00");
                    ddlUnidade.SelectedItem.Text = item.Unidade.ToString();
                    txtDescricao.Text = item.Cpl_DscProduto;
                    txtVlrTotalItem.Text = item.ValorTotalItem.ToString("###,##0.00");
                    BtnExcluirProduto.Visible = true;
                }
            }
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }

        protected void grdLogDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void CamposProduto()
        {
            if (Session["ManutencaoProduto"] != null)
            {
                BtnExcluirProduto.Visible = true;
            }
            else
            {
                BtnExcluirProduto.Visible = false;
            }
        }

        protected void BtnAddProduto_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos() == false)
                    return;
                int CodigoTipoOperacao = 0;
                if (Session["Doc_orcamento"] != null)
                {
                    Doc_Orcamento doc = new Doc_Orcamento();
                    doc = (Doc_Orcamento)Session["Doc_orcamento"];
                    CodigoTipoOperacao = doc.CodigoTipoOperacao;
                }
                else if (Session["Doc_Pedido"] != null)
                {
                    Doc_Pedido doc = new Doc_Pedido();
                    doc = (Doc_Pedido)Session["Doc_Pedido"];
                    CodigoTipoOperacao = doc.CodigoTipoOperacao;
                }
                if (CodigoTipoOperacao == 0)
                {
                    ShowMessage("Tipo de operação é necessário, volte na tela anterior e selecione um tipo de operação", MessageType.Info);
                    return;
                }

                TipoOperacao tpOP = new TipoOperacao();
                TipoOperacaoDAL tpOPDAL = new TipoOperacaoDAL();
                tpOP = tpOPDAL.PesquisarTipoOperacao(CodigoTipoOperacao);
                if (tpOP.MovimentaEstoque)
                {
                    EstoqueProdutoDAL estoqueDAL = new EstoqueProdutoDAL();
                    decimal decQuantidadeDisponivel = estoqueDAL.PesquisarEstoqueDisponivelProduto(Convert.ToInt64(txtCodItem.Text), Convert.ToInt32(Session["CodEmpresa"]));
                    if (Convert.ToDecimal(txtQtde.Text) > decQuantidadeDisponivel)
                    {
                        if (decQuantidadeDisponivel == 0)
                            ShowMessage("Este produto não tem em estoque", MessageType.Warning);
                        else
                            ShowMessage("Produto possui apenas " + decQuantidadeDisponivel + " " + ddlUnidade.SelectedItem.Text + " disponível em estoque", MessageType.Warning);
                        return;
                    }
                }

                if (Session["ManutencaoProduto"] != null)
                {
                    ProdutoDocumento produto = (ProdutoDocumento)Session["ManutencaoProduto"];
                    Session["ManutencaoProduto"] = null;

                    List<ProdutoDocumento> ListaItem = new List<ProdutoDocumento>();

                    if (Session["ListaItemNF"] != null)
                        ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemNF"];
                    else
                        ListaItemDocumento = new List<ProdutoDocumento>();

                    ProdutoDocumento tabi;
                    foreach (ProdutoDocumento item in ListaItemDocumento)
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
                            tabi.QuantidadeAtendida = item.QuantidadeAtendida;
                            tabi.PrecoItem = item.PrecoItem;
                            tabi.ValorDesconto = item.ValorDesconto;
                            tabi.ValorTotalItem = item.ValorTotalItem;
                            tabi.CodigoSituacao = item.CodigoSituacao;
                            tabi.ValorFatorCubagem = item.ValorFatorCubagem;
                            tabi.ValorPeso = item.ValorPeso;
                            tabi.ValorVolume = item.ValorVolume;
                            tabi.Impostos = item.Impostos;
                            tabi.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            ListaItem.Add(tabi);
                        }
                        else
                        {
                            tabi = new ProdutoDocumento();
                            tabi.CodigoItem = item.CodigoItem;
                            tabi.CodigoDocumento = item.CodigoDocumento;
                            tabi.Unidade = ddlUnidade.SelectedItem.Text;
                            tabi.Cpl_DscProduto = Convert.ToString(txtDescricao.Text);
                            tabi.CodigoProduto = Convert.ToInt32(txtCodItem.Text);
                            tabi.Quantidade = Convert.ToDecimal(txtQtde.Text);
                            tabi.QuantidadeAtendida = 0;
                            tabi.ValorDesconto = Convert.ToDecimal(txtDesconto.Text);
                            tabi.PrecoItem = (Convert.ToDecimal(txtPreco.Text) * (1 - (tabi.ValorDesconto / 100)));
                            tabi.ValorTotalItem = (Convert.ToDecimal(txtPreco.Text) * (1 - (tabi.ValorDesconto / 100))) * tabi.Quantidade;
                            tabi.CodigoSituacao = item.CodigoSituacao;
                            tabi.ValorFatorCubagem = item.ValorFatorCubagem;
                            tabi.ValorPeso = item.ValorPeso;
                            tabi.ValorVolume = item.ValorVolume;
                            tabi.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            //tabi.Impostos = CalcularImpostosProduto(item);
                            ListaItem.Add(tabi);
                        }
                    }

                    MontarValorTotal(ListaItem);
                    BtnExcluirProduto.Visible = false;
                    Session["ListaItemNF"] = ListaItem;

                    grdProduto.DataSource = ListaItem.Where(x => x.CodigoSituacao != 134).ToList();
                    grdProduto.DataBind();

                }//inserir produto novo
                else//alterar produto existente
                {
                    Produto p = new Produto();
                    ProdutoDAL produtoDAL = new ProdutoDAL();
                    p = produtoDAL.PesquisarProduto(Convert.ToInt64(txtCodItem.Text));

                    int intEndItem = 0;
                    ProdutoDocumentoDAL r = new ProdutoDocumentoDAL();
                    List<DBTabelaCampos> lista = new List<DBTabelaCampos>();

                    if (ListaItemDocumento.Count != 0)
                        intEndItem = Convert.ToInt32(ListaItemDocumento.Max(x => x.CodigoItem).ToString());

                    intEndItem = intEndItem + 1;

                    ProdutoDocumento ListaItem = new ProdutoDocumento();
                    ListaItem.CodigoItem = intEndItem;
                    ListaItem.CodigoProduto = Convert.ToInt32(txtCodItem.Text);
                    ListaItem.Cpl_DscProduto = txtDescricao.Text;
                    ListaItem.Quantidade = Convert.ToDecimal(txtQtde.Text);
                    ListaItem.QuantidadeAtendida = 0;
                    ListaItem.Unidade = ddlUnidade.SelectedItem.Text;
                    ListaItem.ValorDesconto = Convert.ToDecimal(txtDesconto.Text);
                    ListaItem.PrecoItem = (Convert.ToDecimal(txtPreco.Text) * (1 - (ListaItem.ValorDesconto/100))); 
                    ListaItem.ValorTotalItem = (Convert.ToDecimal(txtPreco.Text) * (1 - (ListaItem.ValorDesconto / 100))) * ListaItem.Quantidade;
                    ListaItem.CodigoSituacao = 135;
                    ListaItem.ValorVolume = p.ValorVolume;
                    ListaItem.ValorPeso = p.ValorPeso;
                    ListaItem.ValorFatorCubagem = p.ValorFatorCubagem;
                    ListaItem.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";



                    if (intEndItem != 0)
                        ListaItemDocumento.RemoveAll(x => x.CodigoItem == intEndItem);

                    //ListaItem.Impostos = CalcularImpostosProduto(ListaItem);
                    ListaItemDocumento.Add(ListaItem);

                    Session["ListaItemNF"] = ListaItemDocumento;

                    var ListaAtivos = ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList();

                    if ((ListaAtivos.Count % 5) == 0)
                        ShowMessage("Salve o documento para garantir os itens já inseridos", MessageType.Warning);

                    grdProduto.DataSource = ListaAtivos;
                    grdProduto.DataBind();

                    MontarValorTotal(ListaItemDocumento);
                }

                LimpaTela();
                txtCodItem.Focus();
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        
        protected void BtnExcluirProduto_Click(object sender, EventArgs e)
        {
            List<ProdutoDocumento> ListaItem = new List<ProdutoDocumento>();

            if (Session["ListaItemNF"] != null)
                ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemNF"];
            else
                ListaItemDocumento = new List<ProdutoDocumento>();

            ProdutoDocumento tabi;
            foreach (ProdutoDocumento item in ListaItemDocumento)
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

                if (item.CodigoItem == Convert.ToInt32(HttpUtility.HtmlDecode(grdProduto.SelectedRow.Cells[0].Text)))
                    tabi.CodigoSituacao = 134;
                else
                    tabi.CodigoSituacao = item.CodigoSituacao;

                ListaItem.Add(tabi);
            }
            

            BtnExcluirProduto.Visible = false;
            MontarValorTotal(ListaItem);

            grdProduto.DataSource = ListaItem.Where(x => x.CodigoSituacao != 134).ToList(); 
            grdProduto.DataBind();

            Session["ListaItemNF"] = ListaItem;
            Session["ManutencaoProduto"] = null;

            LimpaTela();
            txtCodItem.Focus();
        }

        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=1");
        }

        protected void txtCodItem_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCodItem.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Produto", txtCodItem.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodItem.Text = "";
                    return;
                }
            }

            Int64 codigoItem = Convert.ToInt64(txtCodItem.Text);
            ProdutoDAL produtoDAL = new ProdutoDAL();
            Produto produto = new Produto();
            produto = produtoDAL.PesquisarProduto(codigoItem);//PESQUISAR PRODUTO DO TIPO PRODUTO ACABADO
            txtQtde.Focus();
            
            if (produto != null)
            {
                var ItemExiste = ListaItemDocumento.Where(x => x.CodigoProduto == produto.CodigoProduto && x.CodigoSituacao != 134);
                if (ItemExiste.Count() > 0)
                {
                    txtCodItem.Text = "";
                    txtCodItem.Focus();
                    txtDescricao.Text = "";
                    ShowMessage("Este produto já foi adicionado", MessageType.Info);
                }
                else
                {
                    if (produto.ValorVenda != 0)
                    {
                        txtDescricao.Text = produto.DescricaoProduto;
                        ddlUnidade.SelectedValue = produto.CodigoUnidade.ToString();
                        txtPreco.Text = produto.ValorVenda.ToString("F");
                        txtQtde.Focus();
                    }
                    else
                    {
                        txtCodItem.Text = "";
                        txtCodItem.Focus();
                        txtDescricao.Text = "";
                        txtPreco.Text = "0,00";
                        ShowMessage("Preço de venda do produto está zerado", MessageType.Info);
                    }
                }
            }
            else
            {
                txtCodItem.Text = "";
                txtCodItem.Focus();
                txtDescricao.Text = "";
                txtPreco.Text = "0,00";
                ShowMessage("Este produto não existe", MessageType.Info);
            }
            Session["TabFocada"] = null;
        }

        protected void txtPreco_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtPreco.Text.Equals(""))
            {
                txtPreco.Text = "0,00";
            }
            else
            {
                v.CampoValido("Preço", txtPreco.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtPreco.Text = Convert.ToDecimal(txtPreco.Text).ToString("###,##0.00");
                    txtDesconto.Focus();
                    if (Convert.ToDecimal(txtDesconto.Text) != 0)
                        txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * (Convert.ToDecimal(txtPreco.Text) *(1 - (Convert.ToDecimal(txtDesconto.Text) / 100)))).ToString("###,##0.00");
                    else
                        txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * Convert.ToDecimal(txtPreco.Text)).ToString("###,##0.00");
                }
                else
                    txtPreco.Text = "0,00";

            }
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
                    txtPreco.Focus();
                    if (Convert.ToDecimal(txtDesconto.Text) != 0)
                        txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * (Convert.ToDecimal(txtPreco.Text) * (1 - (Convert.ToDecimal(txtDesconto.Text) / 100)))).ToString("###,##0.00");
                    else
                        txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * Convert.ToDecimal(txtPreco.Text)).ToString("###,##0.00");
                }
                else
                    txtQtde.Text = "0,00";

            }
        }

        protected void grdProduto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdProduto.PageIndex = e.NewPageIndex;
            // Carrega os dados
            if (Session["ListaItemNF"] != null)
            {
                ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemNF"];
                grdProduto.DataSource = ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList();
                grdProduto.DataBind();

            }
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
                ValorTotalItem = t.Field<decimal>("ValorTotalItem"),
                Cpl_DscSituacao = t.Field<string>("Cpl_DscSituacao")
            }).ToList();

            Session["ListaItemNF"] = ListaItemDocumento;

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

        protected void txtDesconto_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDesconto.Text.Equals(""))
            {
                txtDesconto.Text = "0,00";
            }
            else
            {
                v.CampoValido("Desconto", txtDesconto.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDesconto.Text = Convert.ToDecimal(txtDesconto.Text).ToString("###,##0.00");
                    BtnAddProduto.Focus();
                    if (Convert.ToDecimal(txtDesconto.Text) != 0)
                        txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * (Convert.ToDecimal(txtPreco.Text) * (1 - (Convert.ToDecimal(txtDesconto.Text) / 100)))).ToString("###,##0.00");
                    else
                        txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * Convert.ToDecimal(txtPreco.Text)).ToString("###,##0.00");
                }
                else
                    txtDesconto.Text = "0,00";

            }
        }
   
    }
}