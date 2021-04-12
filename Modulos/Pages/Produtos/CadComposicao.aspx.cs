using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class CadComposicao : System.Web.UI.Page
    {
        List<ItemDaComposicao> ListaItens = new List<ItemDaComposicao>();
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaCamposComponente()
        {
            BtnRemProduto.Visible = false;
            txtProdutoComponente.Text = "";
            txtDescComposto.Text = "";
            txtquantidade.Text = "0,00";
            txtQuebraComponente.Text = "0,00";
            txtUnd.Text = "";
            txtEmb.Text = "";
            txtValorComponente.Text = "0,00";
            txtObsComponente.Text = "";
        }
        protected void LimpaCampos()
        {
            txtProdutoComposto.Enabled = true;
            BtnRemProduto.Visible = false;
            txtProdutoComponente.Text = "";
            txtDescComposto.Text = "";
            txtquantidade.Text = "0,00";
            txtQuebraComponente.Text = "0,00";
            txtUnd.Text = "";
            txtEmb.Text = "";
            txtValorComponente.Text = "0,00";
            txtObsComponente.Text = "";
            txtProdutoComposto.Text = "";
            txtDcrproduto.Text = "";
            txtObs.Text = "";
            txtQuebra.Text = "0,00";
            txtUmidade.Text = "0,00";
            txtvalorProduto.Text = "0,00";
            txtRendimento.Text = "0,00";
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código do Produto", txtProdutoComposto.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtProdutoComposto.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtProdutoComposto.Focus();

                }
                return false;
            }
            v.CampoValido("Rendimento", txtRendimento.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtRendimento.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtRendimento.Focus();

                }
                return false;
            }
            if (txtRendimento.Text == "0,00")
            {
                ShowMessage("Rendimento deve ser Informado!", MessageType.Info);
                txtRendimento.Focus();
                return false;
            }
            v.CampoValido("Percentual de Quebra", txtQuebra.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtQuebra.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtQuebra.Focus();

                }
                return false;
            }
            v.CampoValido("Percentual de Umidade", txtUmidade.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtUmidade.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtUmidade.Focus();

                }
                return false;
            }
            return true;
        }
        protected Boolean Validacao()
        {
            if (ddlRoteiro.Text == "")
            {
                ShowMessage("Produto componente deve conter um Roteiro ", MessageType.Info);
                return false;
            }
            if(txtProdutoComposto.Text == txtProdutoComponente.Text)
            {
                txtProdutoComponente.Text = "";
                LimpaCamposComponente();
                ShowMessage("Produto Composto deve ser diferente de Produto para Composição",MessageType.Info);
                return false;
            }
            if (txtProdutoComposto.Text == "")
            {
                ShowMessage("Digite um Produto", MessageType.Info);
                return false;
            }
            Boolean blnCampoValido = false;
            v.CampoValido("Código do Produto Composto", txtProdutoComponente.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtProdutoComponente.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtProdutoComposto.Focus();

                }
                return false;
            }
            if (txtProdutoComponente.Text == "")
            {
                ShowMessage("Digite um Produto de Composição", MessageType.Info);
                return false;
            }
            if (txtValorComponente.Text == "0,00" || txtValorComponente.Text == "")
            {
                ShowMessage("Valor do Componente deve ser maior que Zero!", MessageType.Info);
                return false;
            }
            if (txtquantidade.Text == "" || txtquantidade.Text == "0,00")
            {
                ShowMessage("Quantidade deve ser maior que Zero", MessageType.Info);
                return false;
            }
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt16(Session["CodModulo"].ToString()),
                                           Convert.ToInt16(Session["CodPflUsuario"].ToString()),
                                           "ConComposicao.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomMovimentacaoEstoque2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                lista.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        //if (!x.AcessoAlterar)
                        btnSalvar.Visible = false;
                    }
                });
            }
            if(Session["ZoomProduto"] != null)
            {
                Session["ComposicaoProduto"] = null;
                txtProdutoComposto.Text = Session["ZoomProduto"].ToString();
                Session["ZoomProduto"] = null;
                txtProduto_TextChanged(sender, e);
                MontaSituacao();
            }
            if (Session["Umavez"] == null)
            {
                Session["Umavez"] = "";
                LimpaCampos();
                MontaSituacao();
            }
            if (Session["ZoomComposicao"] != null)
            {
                string s = Session["ZoomComposicao"].ToString();
                Session["ZoomComposicao"] = null;

                string[] words = s.Split('³');
                if (s != "³")
                {
                    Session["Atualizar"] = "";
                    TXTSAIR.Text = "ATUALIZAR";
                    Composicao p = new Composicao();
                    List<ItemDaComposicao> List = new List<ItemDaComposicao>();
                    ComposicaoDAL RnComposicao = new ComposicaoDAL();
                    ItemDaComposicaoDAL RnItemComposicao = new ItemDaComposicaoDAL();

                    MontaSituacao();

                    btnExcluir.Visible = true;
                    txtProdutoComposto.Text = words[0].ToString();
                    txtProduto_TextChanged(sender, e);

                    p = RnComposicao.PesquisarComposicao(Convert.ToInt32(txtProdutoComposto.Text));
                    List = RnItemComposicao.ListarItemDaComposicao(p.CodigoProdutoComposto);

                    MontaRoteiro(p.CodigoProdutoComposto);

                    txtRendimento.Text = p.Rendimento.ToString();
                    ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                    ddlTipo.SelectedValue = p.CodigoTipo.ToString();
                    txtQuebra.Text = p.PercentualQuebra.ToString();
                    txtUmidade.Text = p.PercentualUmidade.ToString();
                    txtvalorProduto.Text = p.ValorCustoProduto.ToString();
                    txtObs.Text = p.Observacao;
                    txtData.Text = p.Data.ToShortDateString();

                    Session["GridComp"] = List;
                    List = List.OrderByDescending(x => x.CodigoComponente).ToList();
                    GridComposicao.DataSource = List;
                    GridComposicao.DataBind();

                    return;
                }
            }
            else
            {
                TXTSAIR.Text = "novo";
                btnExcluir.Visible = false;
                DBTabelaDAL dbTDAL = new DBTabelaDAL();
                txtData.Text = Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("dd/MM/yyyy"));
            }
            if (TXTSAIR.Text == "")
            {
                btnVoltar_Click(sender, e);
            }
            if (Session["txtRoteiro"] != null)
            {
                ddlRoteiro.SelectedValue = Session["txtRoteiro"].ToString();
            }
        }
        protected void MontaRoteiro(int cod)
        {
            ItemDaComposicaoDAL it = new ItemDaComposicaoDAL();
            ddlRoteiro.DataSource = it.ListarRoteiros(cod);
            ddlRoteiro.DataTextField = "DescRoteiro";
            ddlRoteiro.DataValueField = "CodigoRoteiro";
            ddlRoteiro.DataBind();
        }
        protected void MontaSituacao()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            Habil_TipoDAL ht = new Habil_TipoDAL();
            ddlTipo.DataSource = ht.SituacaoComposicao();
            ddlTipo.DataTextField = "DescricaoTipo";
            ddlTipo.DataValueField = "CodigoTipo";
            ddlTipo.DataBind();
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Session["GridComp"] != null)
            {
                ListaItens = (List<ItemDaComposicao>)Session["GridComp"];
            }
            if (ValidaCampos() == false)
                return;
            if (txtProdutoComposto.Text == "")
            {
                ShowMessage("Digite um Produto", MessageType.Info);
                return;
            }
            if (GridComposicao.Rows.Count == 0)
            {
                ShowMessage("Composição deve constar pelo menos 1 Componente", MessageType.Info);
                return;
            }
            ItemDaComposicao p = new ItemDaComposicao();
            Composicao ep = new Composicao();
            ItemDaComposicaoDAL RnItem = new ItemDaComposicaoDAL();
            ComposicaoDAL RnComposicao = new ComposicaoDAL();

            RnItem.Excluir(Convert.ToInt32(txtProdutoComposto.Text));

            ep.CodigoProdutoComposto = Convert.ToInt32(txtProdutoComposto.Text);
            ep.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            ep.CodigoTipo = Convert.ToInt32(ddlTipo.SelectedValue);
            ep.Rendimento = Convert.ToDecimal(txtRendimento.Text);
            ep.PercentualQuebra = Convert.ToDecimal(txtQuebra.Text);
            ep.PercentualUmidade = Convert.ToDecimal(txtUmidade.Text); 
            ep.ValorCustoProduto = Convert.ToDecimal(txtvalorProduto.Text);
            ep.Observacao = txtObs.Text;

            foreach (ItemDaComposicao item in ListaItens)
            {
                p.CodigoRoteiro = item.CodigoRoteiro;
                p.DescRoteiro = item.DescRoteiro;
                p.CodigoComposto = Convert.ToInt32(txtProdutoComposto.Text);
                p.CodigoComponente = item.CodigoComponente;
                p.QuantidadeComponente = item.QuantidadeComponente;
                p.ValorCustoComponente = item.ValorCustoComponente;
                p.PerQuebraComponente = item.PerQuebraComponente;
                p.Observacao = item.Observacao;
                RnItem.Inserir(p);
            }

            if (Session["Atualizar"] == null)
            {
                Session["MensagemTela"] = "Composição Incluída com Sucesso!!!";
                RnComposicao.Inserir(ep);
            }
            else
            {
                Session["MensagemTela"] = "Composição Atualizada com Sucesso!!!";
                RnComposicao.Atualizar(ep);
            }
            
            btnVoltar_Click(sender, e);
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["txtRoteiro"] = null;
            Session["GridRoteiro"] = null;
            Session["Atualizar"] = null;
            Session["Umavez"] = null;
            Session["GridComp"] = null;
            this.Dispose();
            Response.Redirect("~/Pages/Produtos/ConComposicao.aspx");
        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Excluir();", true);
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            ItemDaComposicaoDAL RnItem = new ItemDaComposicaoDAL();
            ComposicaoDAL RnComp = new ComposicaoDAL();

            RnItem.Excluir(Convert.ToInt32(txtProdutoComposto.Text));
            RnComp.Excluir(Convert.ToInt32(txtProdutoComposto.Text));

            Session["MensagemTela"] = "Composição Excluída com Sucesso!!!";
            btnVoltar_Click(sender, e);
        }
        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            
            txtProdutoComposto.Enabled = false;
            btnPesquisarComposto.Enabled = false;
            Boolean blnCampo = false;
            if (txtProdutoComposto.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Produto", txtProdutoComposto.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtProdutoComposto.Text = "";
                    txtProdutoComposto.Enabled = true;
                    btnPesquisarComposto.Enabled = true;
                    return;
                }
            }
            if (Session["Atualizar"] == null)
            {
                ComposicaoDAL RnComp = new ComposicaoDAL();
                Composicao p = new Composicao();

                p = RnComp.PesquisarComposicao(Convert.ToInt32(txtProdutoComposto.Text));

                if (p.CodigoProdutoComposto > 0)
                {
                    txtProdutoComposto.Text = "";
                    txtDcrproduto.Text = "";
                    txtProdutoComposto.Enabled = true;
                    btnPesquisarComposto.Enabled = true;
                    ShowMessage("Composição já Cadastrada!", MessageType.Info);
                    return;
                }
            }
            Int64 codigoItem = Convert.ToInt64(txtProdutoComposto.Text);
            ProdutoDAL produtoDAL = new ProdutoDAL();
            Produto produto = new Produto();
            produto = produtoDAL.PesquisarProduto(codigoItem);

            if (produto != null)
            {
                if (produto.CodigoSituacao == 1)
                {
                    if (produto.CodigoTipoProduto != 30)
                    {
                        txtDcrproduto.Text = produto.DescricaoProduto;
                    }
                    else
                    {
                        txtProdutoComposto.Enabled = true;
                        btnPesquisarComposto.Enabled = true;
                        txtProdutoComposto.Text = "";
                        ShowMessage("Produto do Tipo: Matéria Prima, não pode ser de Composição", MessageType.Info);
                        txtDcrproduto.Text = "";
                    }
                }
                else
                {
                    txtProdutoComposto.Enabled = true;
                    btnPesquisarComposto.Enabled = true;
                    ShowMessage("Produto está Inativo", MessageType.Info);
                    txtDcrproduto.Text = "";
                }
            }
            else
            {
                txtProdutoComposto.Enabled = true;
                btnPesquisarComposto.Enabled = true;
                txtDcrproduto.Text = "";
                txtProdutoComposto.Text = "";
                ShowMessage("Produto não cadastrado", MessageType.Info);
            }
            txtProdutoComposto.Focus();
        }
        protected void txtProdutoComponente_TextChanged(object sender, EventArgs e)
        {
            if (txtProdutoComponente.Text == "")
            {
                txtProdutoComponente.Text = "";
                ShowMessage("Selecione em Produto Composto", MessageType.Info);
                return;
            }
            Boolean blnCampo = false;
            if (txtProdutoComponente.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Produto", txtProdutoComponente.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtProdutoComponente.Text = "";
                    return;
                }
            }
            ItemDaComposicaoDAL RnItem = new ItemDaComposicaoDAL();
            ItemDaComposicao p = new ItemDaComposicao();

            p = RnItem.ValidacaoParaComposicaoNaoCircular(Convert.ToInt32(txtProdutoComposto.Text), Convert.ToInt32(txtProdutoComponente.Text));

            if (p.CodigoComposto != 0)
            {
                txtProdutoComponente.Text = "";
                ShowMessage("Item da Composicao não permite Referência Circular", MessageType.Info);
                return;
            }
            Int64 codigoItem = Convert.ToInt64(txtProdutoComponente.Text);
            ProdutoDAL produtoDAL = new ProdutoDAL();
            Produto produto = new Produto();
            produto = produtoDAL.PesquisarProduto(codigoItem);

            if (produto != null)
            {
                if (produto.CodigoSituacao == 1)
                {
                    if (produto.ValorCompra > 0)
                    {
                        txtValorComponente.Text = produto.ValorCompra.ToString();
                        Valor(sender, e);
                        txtUnd.Text = produto.DsSigla;
                        txtEmb.Text = produto.DsEmbalagem;
                        txtDescComposto.Text = produto.DescricaoProduto;
                    }
                }
                else
                {
                    ShowMessage("Produto está Inativo", MessageType.Info);
                    txtDescComposto.Text = "";
                }
            }
            else
            {
                txtDescComposto.Text = "";
                txtProdutoComponente.Text = "";
                ShowMessage("Produto não cadastrado", MessageType.Info);
            }
            txtProdutoComponente.Focus();
        }
        protected void gridProdutosGerais_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void GridCompostos_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProdutoComponente.Text = HttpUtility.HtmlDecode(GridCompostos.SelectedRow.Cells[0].Text);
            txtProdutoComponente_TextChanged(sender, e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "GridDesaparece();", true);
        }
        protected void GridCompostos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void BtnAddProduto_Click(object sender, EventArgs e)
        {
            if (Session["GridComp"] != null)
            {
                ListaItens = (List<ItemDaComposicao>)Session["GridComp"];
            }
            if (!Validacao())
                return;
            ItemDaComposicaoDAL RnItem = new ItemDaComposicaoDAL();
            ItemDaComposicao p = new ItemDaComposicao();

            p = RnItem.ValidacaoParaComposicaoNaoCircular(Convert.ToInt32(txtProdutoComposto.Text), Convert.ToInt32(txtProdutoComponente.Text));

            if (p.CodigoComposto != 0)
            {
                txtProdutoComponente.Text = "";
                ShowMessage("Item da Composicao não permite Referência Circular", MessageType.Info);
                return;
            }
            List<ItemDaComposicao> Lista = new List<ItemDaComposicao>();

            p = new ItemDaComposicao
            {
                CodigoRoteiro = Convert.ToInt16(ddlRoteiro.SelectedValue),
                DescRoteiro = ddlRoteiro.SelectedItem.Text,
                CodigoComponente = Convert.ToInt32(txtProdutoComponente.Text),
                DescricaoComponente = txtDescComposto.Text,
                QuantidadeComponente = Convert.ToDecimal(txtquantidade.Text),
                ValorCustoComponente = Convert.ToDecimal(txtValorComponente.Text),                
                PerQuebraComponente = Convert.ToDecimal(txtQuebraComponente.Text),
                Observacao = txtObsComponente.Text,
            };

            Lista = ListaItens.Where(x => x.CodigoComponente == Convert.ToInt32(txtProdutoComponente.Text)).ToList();

            int C = 0;

            if (Session["AddMais1"] != null)
            {
                C = 1;
            }
            if (Lista.Count > C)
            {
                ShowMessage("Item da Composição já cadastrado", MessageType.Info);
            }
            else
            {
                if (Session["AddMais1"] != null)
                {
                    BtnRemProduto_Click(sender, e);
                }
                ListaItens.Add(p);
                
                decimal valor = Convert.ToDecimal(txtvalorProduto.Text);
                decimal ValorComponente = 0;
                ValorComponente = p.QuantidadeComponente * p.ValorCustoComponente;
                valor += ValorComponente;
                txtvalorProduto.Text = valor.ToString("###,##0.00");
            }            

            Session["GridComp"] = ListaItens;
            ListaItens = ListaItens.OrderByDescending(x => x.CodigoComponente).ToList();
            GridComposicao.DataSource = ListaItens;
            GridComposicao.DataBind();
            
            LimpaCamposComponente();
            Session["AddMais1"] = null;
        }
        protected void BtnRemProduto_Click(object sender, EventArgs e)
        {
            if (Session["GridComp"] == null)
            {
                ShowMessage("Lista de Componente deve Conter o mesmo produto", MessageType.Info);
                return;
            }
            if (Session["GridComp"] != null)
            {
                ListaItens = (List<ItemDaComposicao>)Session["GridComp"];
            }
            if (!Validacao())
                return;

            List<ItemDaComposicao> Lista = new List<ItemDaComposicao>();
            
            ListaItens = ListaItens.Where(x => x.CodigoComponente != Convert.ToInt32(txtProdutoComponente.Text)).ToList();

            decimal PrecoComponente = 0;
            decimal PrecoProd = 0;

            decimal quantidade = Convert.ToDecimal(Session["Quantidade"].ToString());

            if (quantidade != Convert.ToDecimal(txtquantidade.Text))
            {
                txtquantidade.Text = quantidade.ToString();
            }

            PrecoComponente = Convert.ToDecimal(txtquantidade.Text) * Convert.ToDecimal(txtValorComponente.Text);
            PrecoProd = Convert.ToDecimal(txtvalorProduto.Text) - PrecoComponente;

            txtvalorProduto.Text = PrecoProd.ToString("###,##0.00");

            Session["GridComp"] = ListaItens;
            txtProdutoComponente.Enabled = true;
            ListaItens = ListaItens.OrderByDescending(x => x.CodigoComponente).ToList();
            GridComposicao.DataSource = ListaItens;
            GridComposicao.DataBind();
            if (Session["AddMais1"] != null)
            {
                LimpaCamposComponente();
            }
            txtProdutoComponente.Enabled = true;
            Session["AddMais1"] = null;
        }
        protected void Valor(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtValorComponente.Text.Equals(""))
            {
                txtValorComponente.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor do Componente", txtValorComponente.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtValorComponente.Text = Convert.ToDecimal(txtValorComponente.Text).ToString("###,##0.00");
                }
                else
                    txtValorComponente.Text = "0,00";
            }
        }
        protected void txtquantidade_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtquantidade.Text.Equals(""))
            {
                txtquantidade.Text = "0,00";
            }
            else
            {
                v.CampoValido("Quantidade", txtquantidade.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtquantidade.Text = Convert.ToDecimal(txtquantidade.Text).ToString("###,##0.00");
                }
                else
                    txtquantidade.Text = "0,00";
            }
        }
        protected void txtQuebraComponente_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtQuebraComponente.Text.Equals(""))
            {
                txtQuebraComponente.Text = "0,00";
            }
            else
            {
                v.CampoValido("Percentual de Quebra", txtQuebraComponente.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtQuebraComponente.Text = Convert.ToDecimal(txtQuebraComponente.Text).ToString("###,##0.00");
                }
                else
                    txtQuebraComponente.Text = "0,00";
            }
        }
        protected void GridComposicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            BtnRemProduto.Visible = true;
            ddlRoteiro.SelectedValue = HttpUtility.HtmlDecode(GridComposicao.SelectedRow.Cells[0].Text);
            txtProdutoComponente.Text = HttpUtility.HtmlDecode(GridComposicao.SelectedRow.Cells[2].Text);
            txtquantidade.Text = HttpUtility.HtmlDecode(GridComposicao.SelectedRow.Cells[4].Text);
            Session["Quantidade"] = HttpUtility.HtmlDecode(GridComposicao.SelectedRow.Cells[4].Text);
            txtValorComponente.Text = HttpUtility.HtmlDecode(GridComposicao.SelectedRow.Cells[5].Text);           
            txtQuebraComponente.Text = HttpUtility.HtmlDecode(GridComposicao.SelectedRow.Cells[6].Text);
            txtObsComponente.Text = HttpUtility.HtmlDecode(GridComposicao.SelectedRow.Cells[7].Text);
            txtProdutoComponente_TextChanged(sender, e);
            txtProdutoComponente.Enabled = false;
            Session["AddMais1"] = "";
        }
        protected void txtRendimento_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtRendimento.Text.Equals(""))
            {
                txtRendimento.Text = "0,00";
            }
            else
            {
                v.CampoValido("Percentual de Quebra", txtRendimento.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtRendimento.Text = Convert.ToDecimal(txtRendimento.Text).ToString("###,##0.00");
                }
                else
                    txtRendimento.Text = "0,00";
            }
        }
        protected void txtQuebra_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtQuebra.Text.Equals(""))
            {
                txtQuebra.Text = "0,00";
            }
            else
            {
                v.CampoValido("Percentual de Quebra", txtQuebra.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtQuebra.Text = Convert.ToDecimal(txtQuebra.Text).ToString("###,##0.00");
                }
                else
                    txtQuebra.Text = "0,00";
            }
        }
        protected void txtUmidade_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtUmidade.Text.Equals(""))
            {
                txtUmidade.Text = "0,00";
            }
            else
            {
                v.CampoValido("Percentual de Quebra", txtUmidade.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtUmidade.Text = Convert.ToDecimal(txtUmidade.Text).ToString("###,##0.00");
                }
                else
                    txtUmidade.Text = "0,00";
            }
        }
        protected void txtRoteiro_TextChanged(object sender, EventArgs e)
        {
            List<ItemDaComposicao> ListaRoteiro = new List<ItemDaComposicao>();

            if (Session["GridRoteiro"] != null)
            {
                ListaRoteiro = (List<ItemDaComposicao>)Session["GridRoteiro"];
            }
            ItemDaComposicao p = new ItemDaComposicao();

            int Roteiro = 0;

            if (ddlRoteiro.SelectedValue != "")
            {
                Roteiro = Convert.ToInt32(ddlRoteiro.SelectedValue) + 1;
            }
            else
            {
                Roteiro = 1;
            }           

            p.CodigoRoteiro = Convert.ToInt16(Roteiro);
            p.DescRoteiro = txtRoteiro.Text;
            ListaRoteiro.Add(p);

            Session["GridRoteiro"] = ListaRoteiro;
            ddlRoteiro.DataSource = ListaRoteiro;
            ddlRoteiro.DataTextField = "DescRoteiro";
            ddlRoteiro.DataValueField = "CodigoRoteiro";
            ddlRoteiro.DataBind();
            ddlRoteiro.SelectedValue = p.CodigoRoteiro.ToString();

            Session["txtRoteiro"] = p.CodigoRoteiro.ToString();

            txtRoteiro.Text = "";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Tirar();", true);

        }
        protected void btnRoteiro_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "Roteiro();", true);
        }
        protected void btnPesquisarComponente_Click(object sender, EventArgs e)
        {
            ProdutoDAL RnProd = new ProdutoDAL();

            GridCompostos.DataSource = RnProd.ListarProdutosParaComposicao("");
            GridCompostos.DataBind();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "GridProdutoComposto();", true);
        }
        protected void btnPesquisarComposto_Click(object sender, EventArgs e)
        {
            Session["ComposicaoProduto"] = "";
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=8");
        }
        protected void txtPesquisaDescricao_TextChanged(object sender, EventArgs e)
        {
            string texto = txtPesquisaDescricao.Text;
            if (texto.Length > 2)
            {
                ProdutoDAL RnProd = new ProdutoDAL();

                GridCompostos.DataSource = RnProd.ListarProdutosParaComposicao(txtPesquisaDescricao.Text);
                GridCompostos.DataBind();
                txtPesquisaDescricao.Text = "";
                lblMensagem.Text = "";
                if (GridCompostos.Rows.Count == 0)
                    lblMensagem.Text ="Não existem Produto(s) mediante a pesquisa informada!";
            }
            else
            {
                lblMensagem.Text = "Pesquisa Permitida acima de 3 caracteres ";
                txtPesquisaDescricao.Text = "";
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "GridProdutoComposto();", true);

        }
    }
}
