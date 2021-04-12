using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class ManMovInterna : System.Web.UI.Page
    {
        public int pnl = 0;
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<MovimentacaoInterna> listMovimentacaoEstoque = new List<MovimentacaoInterna>();
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Documento", txtDocumento.Text, true, false, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtDocumento.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDocumento.Focus();
                }
                return false;
            }
            v.CampoValido("Tipo de Operação", TxtCdTpOperacao.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                TxtCdTpOperacao.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    TxtCdTpOperacao.Focus();
                }
                return false;
            }
            v.CampoValido("Código do Produto", txtProduto.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtProduto.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtProduto.Focus();
                }
                return false;
            }
            if (ddlLocalizacao.SelectedValue == " * Nenhum Selecionado * ")
            {
                ShowMessage("Selecione uma localização!", MessageType.Info);
                ddlLocalizacao.Focus();
                return false;
            }
            if (TxtQtMovimentada.Text == "0,000")
            {
                ShowMessage("Quantidade Movimentada deve ser maior que Zero", MessageType.Info);
                TxtQtMovimentada.Focus();
                return false;
            }
            if (txtValorAjuste.Enabled == true && txtValorAjuste.Text == "0,00")
            {
                ShowMessage("Valor de Ajuste deve ser maior que Zero", MessageType.Info);
                txtValorAjuste.Focus();
                return false;
            }
            if (TxtValorUnitario.Text == "0,0000")
            {
                ShowMessage("Custo Unitario  deve ser maior que Zero", MessageType.Info);
                TxtValorUnitario.Focus();
                return false;
            }
            if (txtVlTotal.Text == "0,00")
            {
                ShowMessage("Valor Total Deve ser Calculado", MessageType.Info);
                txtVlTotal.Focus();
                return false;
            }
            if (pnlNovaLoc.Visible == true)
            {
                if (ddlLocalizacaoCt.SelectedValue == ddlLocalizacao.SelectedValue)
                {
                    ShowMessage("Localização de Contra Partida deve ser Diferente", MessageType.Info);
                    ddlLocalizacaoCt.Focus();
                    return false;
                }
            }
            
            return true;
        }
        protected void LimpaCampos()
        {
            DBTabelaDAL dbTDAL = new DBTabelaDAL();

            txtDtLancamento.Text = Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("dd/MM/yyyy hh:mm:ss"));
            txtDocumento.Text = "";
            TxtCdTpOperacao.Text = "";
            txtProduto.Text = "";
            ddlLocalizacao.SelectedValue = " * Nenhum Selecionado * ";
            ddlLocalizacao.Enabled = true;
            ddlLote.SelectedValue = "..... SEM LOTE .....";
            ddlLote.Enabled = true;
            txtDcrproduto.Text = "";
            Dsctpoperacao.Text = "";
            txtOperacao.Text = "OPERAÇÃO";
            TxtQtMovimentada.Text = "0,000";
            TxtValorUnitario.Text = "0,000";
            txtVlTotal.Text = "0,00";
            txtSdlAnterior.Text = "0,000";
            txtSldFinal.Text = "0,000";
            txtValorAjuste.Text = "0,000";

            btnSalvar.Visible = true;
            TxtCdTpOperacao.Enabled = true;
            btnTpOperacao.Enabled = true;
            txtValorAjuste.Enabled = false;
            TxtQtMovimentada.Enabled = true;

            CarregaEmpresa();

            pnlNovaLoc.Visible = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["ZoomMovimentacaoEstoque2"] != null)
            {
                if (Session["ZoomMovimentacaoEstoque2"].ToString() == "RELACIONAL")
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
                cmdSair.Visible = false;
            }
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
                                           "ManMovInterna.aspx");

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
                //txtLancamento.Text = "Novo";
                ddlEmpresa_SelectedIndexChanged(sender, e);             
                ObterLote();
                LimpaCampos();
                ddlEmpresa.Enabled = true;
                txtDocumento.Focus();                 
            }
            if (Session["MovimentacaoInterna"] != null)
            {
                PreencherDados(sender, e);

                if (Session["ZoomProduto"] != null)
                {
                    string[] s = Session["ZoomProduto"].ToString().Split('³');

                    txtProduto.Text = s[0].ToString();

                    Session["ZoomProduto"] = null;
                }
                if (Session["ZoomTipoOperacao"] != null)
                {
                    string[] s = Session["ZoomTipoOperacao"].ToString().Split('³');

                    TxtCdTpOperacao.Text = s[0].ToString();

                    Session["ZoomTipoOperacao"] = null;
                }
                
                txtProduto_TextChanged(sender, e);
                TxtCdTpOperacao_TextChanged(sender, e);                
            }
            if (Session["CodEmpresa"] != null)
            {
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
                ddlEmpresaCt.SelectedValue = Session["CodEmpresa"].ToString();
            }            
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            ddlEmpresa_SelectedIndexChanged(sender, e);
            ObterLote();            
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            Decimal refQtd = 0;
            Int32 CodLote = 0;
            Int32 CodigoOp = Convert.ToInt32(TxtCdTpOperacao.Text);
            Int32 CodigoOpCtPartida = Convert.ToInt32(TxtCdTpOperacao.Text);

            MovimentacaoInterna ep = new MovimentacaoInterna();
            MovimentacaoInternaDAL d = new MovimentacaoInternaDAL();

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

            
            TipoOperacaoDAL Rn_operacao = new TipoOperacaoDAL();
            TipoOperacao TipoOperacao = new TipoOperacao();

            EstoqueProduto EsP = new EstoqueProduto();
            EstoqueProdutoDAL Rn_Ep = new EstoqueProdutoDAL();

            TipoOperacao = Rn_operacao.PesquisarTipoOperacao(CodigoOp);

            if (ddlLote.SelectedValue == "..... SEM LOTE .....")
                CodLote = 0;
            else
            {
                CodLote = Convert.ToInt32(ddlLote.SelectedValue);
            }

            if (TipoOperacao.CodigoTipoMovimentacao == 60)
                ep.TpOperacao = "S";
            if (TipoOperacao.CodigoTipoMovimentacao == 61)
                ep.TpOperacao = "E";
            if (TipoOperacao.CodigoTipoMovimentacao == 62)
                ep.TpOperacao = "A";

            TipoOperacao.CodigoTipoOperacao = ep.CodigoTipoOperacao;

            ep.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());

            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            if (he != null)
            {
                ep.CodigoMaquina = he.CodigoEstacao;
            }
            ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            ep.CodigoIndiceLocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
            ep.NumeroDoc = Convert.ToString(txtDocumento.Text);
            ep.DtLancamento = Convert.ToDateTime(txtDtLancamento.Text);
            ep.CodigoTipoOperacao = Convert.ToInt32(TxtCdTpOperacao.Text);
            ep.QtMovimentada = Convert.ToDecimal(TxtQtMovimentada.Text);
            ep.ValorUnitario = Convert.ToDecimal(TxtValorUnitario.Text);
            ep.ValorSaldoAnterior = Convert.ToDecimal(txtSdlAnterior.Text);
            ep.VlSaldoFinal = Convert.ToDecimal(txtSldFinal.Text);
            ep.VlSaldoAjuste = Convert.ToDecimal(txtValorAjuste.Text);
            ep.CodigoProduto = Convert.ToInt32(txtProduto.Text);
            ep.CodigoLote = CodLote;


            Rn_Ep.EstoqueIndiceProduto(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToInt32(txtProduto.Text), Convert.ToInt32(ddlLocalizacao.SelectedValue), CodLote, ref refQtd);
            if (refQtd == 0 && ep.TpOperacao == "S")
            {
                ShowMessage("Operação de Saída Impossível sem Registro de Estoque", MessageType.Info);
                return;
            }
            if (refQtd > 0 && ep.TpOperacao == "S" && Convert.ToDecimal(TxtQtMovimentada.Text) > refQtd)
            {
                ShowMessage("Quantidade em Estoque Inferior a Quantidade Movimentada", MessageType.Info);
                return;
            }

            d.Inserir(ep);
            ShowMessage("Movimentação Interna salva com sucesso", MessageType.Success);
            if (pnlNovaLoc.Visible == true)
            {
                if (ep.TpOperacao == "E")
                    ep.TpOperacao = "S";
                else
                    ep.TpOperacao = "E";

                ep.CodigoTipoOperacao = TipoOperacao.CodTipoOperCtPartida;

                ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresaCt.SelectedValue);
                if (ddlLocalizacaoCt.SelectedValue != " * Nenhum Selecionado * ")
                    ep.CodigoIndiceLocalizacao = Convert.ToInt32(ddlLocalizacaoCt.SelectedValue);
                else
                    ep.CodigoIndiceLocalizacao = 0;



                if (ddlLoteCt.SelectedValue != "..... SEM LOTE .....")
                    ep.CodigoLote = Convert.ToInt32(ddlLoteCt.SelectedValue);
                else
                    ep.CodigoLote = 0;

                d.Inserir(ep);
                ShowMessage("Movimentação Interna de Contra Partida salva com sucesso", MessageType.Success);
            }
            btnSalvar.Visible = false;
        }
        protected void CarregaEmpresa()
        {
            EmpresaDAL ep = new EmpresaDAL();
            ddlEmpresa.DataSource = ep.ListarEmpresas("", "", "", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.SelectedValue = null;
            ddlEmpresa.DataBind();

            txtSdlAnterior.Text = ObterSaldoAnterior().ToString("###,##0.00");
        }
        protected void CarregaEmpresaCt()
        {
            EmpresaDAL ep = new EmpresaDAL();
            ddlEmpresaCt.DataSource = ep.ListarEmpresas("", "", "", "");
            ddlEmpresaCt.DataTextField = "NomeEmpresa";
            ddlEmpresaCt.DataValueField = "CodigoEmpresa";
            ddlEmpresaCt.SelectedValue = null;
            ddlEmpresaCt.DataBind();

            //txtSdlAnterior.Text = ObterSaldoAnterior().ToString("###,##0.00");
        }
        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlLocalizacao.SelectedValue != null || ddlLocalizacao.SelectedValue != " * Nenhum Selecionado * ")
            {
                LocalizacaoDAL lc = new LocalizacaoDAL();

                ddlLocalizacao.Items.Clear();
                ddlLocalizacao.DataSource = lc.ListarLocalizacao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
                ddlLocalizacao.DataTextField = "CodigoLocalizacao";
                ddlLocalizacao.DataValueField = "CodigoIndice";
                ddlLocalizacao.SelectedValue = null;
                ddlLocalizacao.DataBind();
                ddlLocalizacao.Items.Insert(0, " * Nenhum Selecionado * ");
                ddlLocalizacao.Enabled = true;
            }
            else
            {
                ddlLocalizacao.Items.Insert(0, " * Nenhum Selecionado * ");
                ddlLocalizacao.Enabled = true;
            }
        }
        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            CompactaDados(sender, e);
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=4");
        }
        protected void btnTpOperacao_Click(object sender, EventArgs e)
        {
            CompactaDados(sender, e);
            Response.Redirect("~/Pages/Fiscal/ConTipoOperacao.aspx?cad=1");
        }
        protected void TxtCdTpOperacao_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (TxtCdTpOperacao.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Tipo de Operação", TxtCdTpOperacao.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    TxtCdTpOperacao.Text = "";
                    return;
                }
            }
            bool movEst = false;
            bool movInt = false;
            bool movLoc = false;
            //int intTpContraPartida = 0;
            Int32 CodigoOp = Convert.ToInt32(TxtCdTpOperacao.Text);
            TipoOperacaoDAL TipoOperacaoDAL = new TipoOperacaoDAL();
            TipoOperacao TipoOperacao = new TipoOperacao();
            TipoOperacao = TipoOperacaoDAL.PesquisarTipoOperacao(CodigoOp);
            if (TipoOperacao != null)
            {
                movLoc = TipoOperacao.MovLocOrigemDestino;
                movEst = TipoOperacao.MovimentaEstoque;
                movInt = TipoOperacao.MovimentacaoInterna;
                //intTpContraPartida = TipoOperacao.CodTipoOperCtPartida;
                if (movEst == false)
                {
                    ShowMessage("Tipo de Operação deve ser de movimentação De: Atualização de Estoque", MessageType.Info);                
                    return;
                }
                if (movInt == false)
                {
                    ShowMessage("Tipo de Operação deve ser de movimentação De: Movimentação Interna", MessageType.Info);
                    return;
                }
                if (TipoOperacao.CodTipoOperCtPartida != 0)
                {
                    if (movLoc == false)
                    {
                        ShowMessage("Tipo de Operação deve ser de movimentação De: Movimentação de Localização Origem Destino", MessageType.Info);
                        return;
                    }
                    else
                    {
                        pnlNovaLoc.Visible = true;
                        CarregaEmpresaCt();
                        ddlEmpresaCt_SelectedIndexChanged(sender, e);
                        ObterLoteCt(); 
                    }                    
                }
                
                Dsctpoperacao.Text = TipoOperacao.DescricaoTipoOperacao;

                if (TipoOperacao.CodigoTipoMovimentacao == 60)
                {
                    txtOperacao.Text = "SAÍDA";
                    txtValorAjuste.Enabled = false;
                    TxtQtMovimentada.Enabled = true;
                }
                if (TipoOperacao.CodigoTipoMovimentacao == 61)
                {
                    txtOperacao.Text = "ENTRADA";
                    txtValorAjuste.Enabled = false;
                    TxtQtMovimentada.Enabled = true;
                }
                if (TipoOperacao.CodigoTipoMovimentacao == 62)
                {
                    txtOperacao.Text = "AJUSTE";
                    txtValorAjuste.Enabled = true;
                    TxtQtMovimentada.Enabled = false;
                }
            }
            else
            {
                TxtCdTpOperacao.Text = "";
                Dsctpoperacao.Text = "";
                ShowMessage("Tipo de Operação não cadastrado", MessageType.Info);
                return;
            }
            TxtCdTpOperacao.Enabled = false;
            btnTpOperacao.Enabled = false;
            CarregaPanel(pnl, sender, e);

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
            produto = produtoDAL.PesquisarProduto(codigoItem);

            if (produto != null)
            {
                if (produto.CodigoSituacao == 1)
                {
                    if (produto.CodigoTipoProduto != 29)
                    {
                        
                        txtDcrproduto.Text = produto.DescricaoProduto;
                        if (produto.ControlaLote == true)
                        {
                            ddlLote.Enabled = true;
                            ObterLote();
                        }
                        else
                        {
                            ddlLote.Items.Insert(0, "..... SEM LOTE .....");
                            ddlLote.Enabled = false;
                        }
                    }
                    else
                    {
                        ShowMessage("Produto do Tipo: Serviço, não pode ser Movimentado", MessageType.Info);
                        txtDcrproduto.Text = "";
                    }
                }
                else
                {
                    ShowMessage("Produto está Inativo", MessageType.Info);
                    txtDcrproduto.Text = "";
                }
            }
            else
            {
                txtDcrproduto.Text = "";
                txtProduto.Text = "";
                ShowMessage("Produto não cadastrado", MessageType.Info);
            }
            Session["TabFocada"] = null;
            txtProduto.Focus();
            if (Session["CodEmpresa"] != null)
            {
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
            }            
            //ObterLote();
            ddlLote_TextChanged(sender, e);
            ddlLocalizacao_TextChanged(sender, e);
            
        }
        protected void CompactaDados(object sender, EventArgs e)
        {
            MovimentacaoInterna ep = new MovimentacaoInterna();

            ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            ep.DtLancamento = Convert.ToDateTime(txtDtLancamento.Text);
            ep.NumeroDoc = Convert.ToString(txtDocumento.Text);
            if (TxtCdTpOperacao.Text == "")
                ep.CodigoTipoOperacao = 0;
            else
            {
                ep.CodigoTipoOperacao = Convert.ToInt32(TxtCdTpOperacao.Text);
            }
            if (txtProduto.Text == "")
                ep.CodigoProduto = 0;
            else
            {
                ep.CodigoProduto = Convert.ToInt32(txtProduto.Text);
            }
            ep.QtMovimentada = Convert.ToDecimal(TxtQtMovimentada.Text);
            ep.ValorUnitario = Convert.ToDecimal(TxtValorUnitario.Text);
            ep.ValorSaldoAnterior = Convert.ToDecimal(txtSdlAnterior.Text);
            ep.VlSaldoFinal = Convert.ToDecimal(txtSldFinal.Text);
            ep.TpOperacao = Convert.ToString(txtOperacao.Text);
            if (ddlLocalizacao.SelectedValue == " * Nenhum Selecionado * " || ddlLocalizacao.SelectedValue == "")
                ep.CodigoIndiceLocalizacao = 0;
            else
            {
                ep.CodigoIndiceLocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
            }
            if (ddlLote.SelectedValue == "..... SEM LOTE ....." || ddlLote.SelectedValue == "")
                ep.CodigoLote = 0;
            else
            {
                ep.CodigoLote = Convert.ToInt32(ddlLote.SelectedValue);
            }
            Session["MovimentacaoInterna"] = ep;
        }
        protected void PreencherDados(object sender, EventArgs e)
        {
            MovimentacaoInterna p = (MovimentacaoInterna)Session["MovimentacaoInterna"];

            CarregaEmpresa();
            ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
            ddlEmpresa_SelectedIndexChanged(sender, e);
            txtDtLancamento.Text = p.DtLancamento.ToString("dd/MM/yyyy hh:mm:ss");
            txtDocumento.Text = p.NumeroDoc.ToString();
            TxtCdTpOperacao.Text = p.CodigoTipoOperacao.ToString();
            txtSdlAnterior.Text = p.ValorSaldoAnterior.ToString("###,##0.00");
            txtSldFinal.Text = p.VlSaldoFinal.ToString("###,##0.00");
            txtOperacao.Text = p.TpOperacao.ToString();
            if (p.CodigoProduto == 0)
                txtProduto.Text = "";
            else
            {
                txtProduto.Text = p.CodigoProduto.ToString();
            }
            if (p.CodigoTipoOperacao == 0)
                TxtCdTpOperacao.Text = "";
            else
            {
                TxtCdTpOperacao.Text = p.CodigoTipoOperacao.ToString();
            }
            TxtQtMovimentada.Text = p.QtMovimentada.ToString("###,##0.00");
            TxtValorUnitario.Text = p.ValorUnitario.ToString("###,##0.00");
            if (p.CodigoLote != 0)
                ddlLote.SelectedValue = p.CodigoLote.ToString();
            else
            {
                ddlLote.SelectedValue = "..... SEM LOTE .....";
            }
            if (p.CodigoIndiceLocalizacao != 0)
                ddlLocalizacao.SelectedValue = p.CodigoIndiceLocalizacao.ToString();
            else
            {
                ddlLocalizacao.SelectedValue = " * Nenhum Selecionado * ";
            }
            Session["MovimentacaoInterna"] = null;
        }
        protected void ddlLote_TextChanged(object sender, EventArgs e)
        {
            txtSdlAnterior.Text = ObterSaldoAnterior().ToString("###,##0.00");
            TxtQtMovimentada_TextChanged(sender, e);
            txtValorAjuste_TextChanged(sender, e);
            TxtValorUnitario_TextChanged(sender, e);
        }
        protected void ddlLocalizacao_TextChanged(object sender, EventArgs e)
        {
            txtSdlAnterior.Text = ObterSaldoAnterior().ToString("###,##0.00");
            TxtQtMovimentada_TextChanged(sender, e);
            txtValorAjuste_TextChanged(sender, e);
            TxtValorUnitario_TextChanged(sender, e);
        }
        protected void TxtQtMovimentada_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (TxtQtMovimentada.Text.Equals(""))
            {
                TxtQtMovimentada.Text = "0,000";
            }
            else
            {
                v.CampoValido("Quantidade Movimentada", TxtQtMovimentada.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    TxtQtMovimentada.Text = Convert.ToDecimal(TxtQtMovimentada.Text).ToString("###,##0.000");
                }
                else
                    TxtQtMovimentada.Text = "0,000";
            }
            if (txtProduto.Text == "")
                return;
            if (ddlLocalizacao.SelectedValue == " * Nenhum Selecionado * ")
                return;
            
            if (TxtCdTpOperacao.Text != "")
            {

                MovimentacaoInterna p = new MovimentacaoInterna();
                MovimentacaoInternaDAL ep = new MovimentacaoInternaDAL();
                Int64 CodigoOp = Convert.ToInt64(TxtCdTpOperacao.Text);
                TipoOperacaoDAL TipoOperacaoDAL = new TipoOperacaoDAL();
                TipoOperacao TipoOperacao = new TipoOperacao();
                TipoOperacao = TipoOperacaoDAL.PesquisarTipoOperacao(CodigoOp);

                decimal decSaldoAnterior = 0;
                decimal decQtdMvimentada = 0;
                decimal c = 0;

                decSaldoAnterior = Convert.ToDecimal(txtSdlAnterior.Text);
                decQtdMvimentada = Convert.ToDecimal(TxtQtMovimentada.Text);

                if (TipoOperacao.CodigoTipoMovimentacao == 60)
                {
                    c = Math.Abs(decSaldoAnterior - decQtdMvimentada);
                    txtSldFinal.Text = c.ToString();
                }
                else
                if (TipoOperacao.CodigoTipoMovimentacao == 61)
                {
                    c = Math.Abs(decSaldoAnterior + decQtdMvimentada);
                    txtSldFinal.Text = c.ToString();
                }
            }
            else
            {
                if (ddlLote.SelectedValue != "..... SEM LOTE .....")
                {
                    ValidaCampos();
                }
                TxtQtMovimentada.Text = "0,000";
            }
        }
        protected void TxtValorUnitario_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (TxtValorUnitario.Text.Equals(""))
            {
                TxtQtMovimentada.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor unitario", TxtValorUnitario.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    TxtValorUnitario.Text = Convert.ToDecimal(TxtValorUnitario.Text).ToString("###,##0.000");
                }
                else
                    TxtValorUnitario.Text = "0,00";
            }

            decimal a = Convert.ToDecimal(TxtQtMovimentada.Text);
            decimal b = Convert.ToDecimal(TxtValorUnitario.Text);
            decimal c = Math.Abs(a * b);
            txtVlTotal.Text = c.ToString("###,##0.00");
            
        }
        protected void txtVlTotal_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtVlTotal.Text.Equals(""))
            {
                txtVlTotal.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor Total", txtVlTotal.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtVlTotal.Text = Convert.ToDouble(txtVlTotal.Text).ToString("###,##0.00");
                }
                else
                    txtVlTotal.Text = "0,000";
            }
        }
        protected void txtSdlAnterior_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtSdlAnterior.Text.Equals(""))
            {
                txtSdlAnterior.Text = "0,000";
            }
            else
            {
                v.CampoValido("Valor Anterior", txtSdlAnterior.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtSdlAnterior.Text = Convert.ToDouble(txtSdlAnterior.Text).ToString("###,##0.000");
                }
                else
                    txtSdlAnterior.Text = "0,000";
            }
        }
        protected void txtSldFinal_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtSldFinal.Text.Equals(""))
            {
                TxtQtMovimentada.Text = "0,000";
            }
            else
            {
                v.CampoValido("Saldo Atual", txtSldFinal.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtSldFinal.Text = Convert.ToDecimal(txtSldFinal.Text).ToString("###,##0.000");
                }
                else
                    txtSldFinal.Text = "0,000";
            }
        }
        protected decimal ObterSaldoAnterior()
        {
            MovimentacaoInterna MovInt = new MovimentacaoInterna();
            MovimentacaoInternaDAL MovIntDal = new MovimentacaoInternaDAL();

            int intLote = 0;

            if (txtProduto.Text == "")
                return 0;
            if (ddlLocalizacao.SelectedValue == " * Nenhum Selecionado * ")
                return 0;
            if (ddlLocalizacao.Items.Count <= 0)
                return 0;

            if (ddlLote.Items.Count > 0)
                if (ddlLote.SelectedValue == "..... SEM LOTE .....")
                    intLote = 0;
                else
                {
                    intLote = Convert.ToInt32(ddlLote.SelectedValue);
                }
            MovInt = MovIntDal.LerSaldoAnterior(Convert.ToInt32(ddlEmpresa.SelectedValue),
                Convert.ToInt32(ddlLocalizacao.SelectedValue), Convert.ToInt32(txtProduto.Text),
                intLote);

            if (MovInt != null)
            {
                return MovInt.VlSaldoFinal;
            }
            else
            {
                return 0;
            }
        }
        protected void txtValorAjuste_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtSldFinal.Text.Equals(""))
            {
                TxtQtMovimentada.Text = "0,000";
            }

            else
            {
                v.CampoValido("Saldo Atual", txtSldFinal.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtSldFinal.Text = Convert.ToDecimal(txtSldFinal.Text).ToString("###,##0.000");
                }
                else
                    txtSldFinal.Text = "0,000";
            }
            if (txtOperacao.Text == "AJUSTE")
            {
                decimal decSaldoAnterior = 0;
                decimal decValorAjuste = 0;
                decimal decSaldoFinal = 0;
                decimal decQtMov = 0;
                //decimal c = 0;

                decSaldoAnterior = Convert.ToDecimal(txtSdlAnterior.Text);
                decValorAjuste = Convert.ToDecimal(txtValorAjuste.Text);
                decSaldoFinal = Convert.ToDecimal(txtSldFinal.Text);
                decQtMov = Convert.ToDecimal(TxtQtMovimentada.Text);

                decQtMov = Math.Abs(decValorAjuste - decSaldoAnterior);
                decSaldoFinal = decValorAjuste;

                TxtQtMovimentada.Text = decQtMov.ToString("###,##0.000");
                txtSldFinal.Text = decSaldoFinal.ToString("###,##0.000");
            }
        }
        protected void ObterLote()
        {
            List<Lote> lstlote = new List<Lote>();
            LoteDAL LoteDAL = new LoteDAL();

            if (txtProduto.Text == "")
            {
                ddlLote.Items.Clear();
                ddlLote.DataSource = null;
                ddlLote.Items.Insert(0, "..... SEM LOTE .....");
                ddlLote.Enabled = true;
                return;
            }            

            lstlote = LoteDAL.ListarLoteDisponivel(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToInt32(txtProduto.Text), 1);
            if (lstlote.Count > 0)
            {
                ddlLote.Items.Clear();
                ddlLote.DataSource = lstlote;
                ddlLote.DataTextField = "Cpl_DescDDL";
                ddlLote.DataValueField = "CodigoIndice";
                ddlLote.SelectedValue = null;
                ddlLote.DataBind();
                ddlLote.Items.Insert(0, "..... SEM LOTE .....");
                ddlLote.Enabled = true;
            }
            else
            {
                ddlLote.Items.Clear();
                ddlLote.DataSource = null;
                ddlLote.Items.Insert(0, "..... SEM LOTE .....");
                ddlLote.Enabled = false;
            }
        }
        protected void ObterLoteCt()
        {

            List<Lote> lstlote = new List<Lote>();
            LoteDAL LoteDAL = new LoteDAL();

            if (txtProduto.Text == "")
            {
                ddlLoteCt.Items.Clear();
                ddlLoteCt.DataSource = null;
                ddlLoteCt.Items.Insert(0, "..... SEM LOTE .....");
                ddlLoteCt.Enabled = true;
                return;
            }

            lstlote = LoteDAL.ListarLoteDisponivel(Convert.ToInt32(ddlEmpresaCt.SelectedValue), Convert.ToInt32(txtProduto.Text), 1);
            if (lstlote.Count > 0)
            {
                ddlLoteCt.Items.Clear();
                ddlLoteCt.DataSource = lstlote;
                ddlLoteCt.DataTextField = "Cpl_DescDDL";
                ddlLoteCt.DataValueField = "CodigoIndice";
                ddlLoteCt.SelectedValue = null;
                ddlLoteCt.DataBind();
                ddlLoteCt.Items.Insert(0, "..... SEM LOTE .....");
                ddlLoteCt.Enabled = true;
            }
            else
            {
                ddlLoteCt.Items.Clear();
                ddlLoteCt.DataSource = null;
                ddlLoteCt.Items.Insert(0, "..... SEM LOTE .....");
                ddlLoteCt.Enabled = false;
            }
        }
        protected void ddlEmpresaCt_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlLocalizacaoCt.SelectedValue != null || ddlLocalizacaoCt.SelectedValue != " * Nenhum Selecionado * ")
            {
                LocalizacaoDAL lc = new LocalizacaoDAL();

                ddlLocalizacaoCt.Items.Clear();
                ddlLocalizacaoCt.DataSource = lc.ListarLocalizacao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
                ddlLocalizacaoCt.DataTextField = "CodigoLocalizacao";
                ddlLocalizacaoCt.DataValueField = "CodigoIndice";
                ddlLocalizacaoCt.SelectedValue = null;
                ddlLocalizacaoCt.DataBind();
                ddlLocalizacaoCt.Items.Insert(0, " * Nenhum Selecionado * ");
                ddlLocalizacaoCt.Enabled = true;
            }
            else
            {
                ddlLocalizacaoCt.Items.Insert(0, " * Nenhum Selecionado * ");
                ddlLocalizacaoCt.Enabled = true;
            }
        }
        protected void ddlLoteCt_TextChanged(object sender, EventArgs e)
        {

        }
        protected void ddlLocalizacaoCt_TextChanged(object sender, EventArgs e)
        {

        }
        protected void CarregaPanel(int pnl, object sender, EventArgs e)
        {
            if (pnl != 1)
            {
                return;
            }
            pnlNovaLoc.Visible = true;
            CarregaEmpresaCt();
            ddlEmpresaCt_SelectedIndexChanged(sender, e);
            ObterLoteCt();
            
        }
    }
}


