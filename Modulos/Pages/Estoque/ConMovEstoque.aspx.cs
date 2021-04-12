using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class ConMovEstoque : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        public string PanelSelect { get; set; }
        String strMensagemR = "";
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<MovimentacaoInterna> listMovimentacaoEstoque = new List<MovimentacaoInterna>();
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            List<MovimentacaoInterna> listMovimentacaoEstoque = new List<MovimentacaoInterna>();

            v.CampoValido("Data de Inicio", txtDtDe.Text, true, false, false, false, "DATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtDtDe.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;
            }
            v.CampoValido("Data de Fim", txtDtAte.Text, true, false, false, false, "DATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtDtAte.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;
            }
            return true;
        }
        protected void LimpaCampos()
        {
            DBTabelaDAL dbTDAL = new DBTabelaDAL();

            txtDocumento.Text = "";
            TxtCdTpOperacao.Text = "";
            txtProduto.Text = "";
            ddlLote.Items.Insert(0," * TODOS OS LOTES * ");
            ddlLote.Enabled = true;
            txtDcrproduto.Text = "";
            Dsctpoperacao.Text = "";
            txtDtAte.Text = Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("dd/MM/yyyy"));
            txtDtDe.Text = Convert.ToDateTime(txtDtAte.Text).AddDays(-14).ToString("dd/MM/yyyy");
            txtAva.Text = "0,00";
            txtDisp.Text = "0,00";
            txtReserv.Text = "0,00";
            txtTotal.Text = "0,00";
            txtQtAjuste.Text = "0,00";
            txtQtEntrada.Text = "0,00";
            txtQtSaida.Text = "0,00";

            btnImprimir.Visible = false;

            CarregaEmpresa();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            //if (Session["TabFocada"] != null)
            //{
            //    PanelSelect = Session["TabFocada"].ToString();
            //}
            //else
            if (!IsPostBack)
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

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
                    //if (!x.AcessoCompleto)
                    //{
                    //    if (!x.AcessoAlterar)
                    //        //btnSalvar.Visible = false;
                    //}
                });
                //txtLancamento.Text = "Novo";
                PanelSelect = "home";
                Session["TabFocada"] = "home";
                ddlEmpresa_SelectedIndexChanged(sender, e);
                LimpaCampos();

                if (Session["CodEmpresa"] != null)
                {
                    ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
                }
            }
            if (Session["ConMovEstoque"] != null)
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
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            //btnConsultar_Click(sender, e);
        }
        protected void CarregaEmpresa()
        {
            EmpresaDAL ep = new EmpresaDAL();
            ddlEmpresa.DataSource = ep.ListarEmpresas("", "", "", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.SelectedValue = null;
            ddlEmpresa.DataBind();
        }
        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocalizacao.SelectedValue != null || ddlLocalizacao.SelectedValue != " * TODAS AS LOCALIZAÇÕES * ")
            {
                LocalizacaoDAL lc = new LocalizacaoDAL();

                ddlLocalizacao.Items.Clear();
                ddlLocalizacao.DataSource = lc.ListarLocalizacao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
                ddlLocalizacao.DataTextField = "CodigoLocalizacao";
                ddlLocalizacao.DataValueField = "CodigoIndice";
                ddlLocalizacao.SelectedValue = null;
                ddlLocalizacao.DataBind();
                ddlLocalizacao.Items.Insert(0, " * TODAS AS LOCALIZAÇÕES * ");
                ddlLocalizacao.Enabled = true;
            }
            else
            {
                ddlLocalizacao.Items.Insert(0, " * TODAS AS LOCALIZAÇÕES * ");
                ddlLocalizacao.Enabled = true;
            }

        }
        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            CompactaDados(sender, e);
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=5");
        }
        protected void btnTpOperacao_Click(object sender, EventArgs e)
        {
            CompactaDados(sender, e);
            Response.Redirect("~/Pages/Fiscal/ConTipoOperacao.aspx?cad=2");

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
            Int32 CodigoOp = Convert.ToInt32(TxtCdTpOperacao.Text);
            TipoOperacaoDAL TipoOperacaoDAL = new TipoOperacaoDAL();
            TipoOperacao TipoOperacao = new TipoOperacao();
            TipoOperacao = TipoOperacaoDAL.PesquisarTipoOperacao(CodigoOp);

            if (TipoOperacao != null)
                Dsctpoperacao.Text = TipoOperacao.DescricaoTipoOperacao;
            else
            {
                ShowMessage("Tipo de Operação não cadastrado", MessageType.Info);
            }
            PanelSelect = "home";
            Session["TabFocada"] = "home";
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
                txtDcrproduto.Text = produto.DescricaoProduto;
            }
            else
            {
                ShowMessage("Produto não cadastrado", MessageType.Info);
            }
            
            txtProduto.Focus();
            if (Session["CodEmpresa"] != null)
            {
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
            }
            PanelSelect = "home";
            Session["TabFocada"] = "home";
            ObterLote();
        }
        protected void CompactaDados(object sender, EventArgs e)
        {
            MovimentacaoInterna ep = new MovimentacaoInterna();

            ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            ep.DtDe = Convert.ToDateTime(txtDtDe.Text);
            ep.DtAte = Convert.ToDateTime(txtDtAte.Text);
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

            if (ddlLocalizacao.SelectedValue == " * TODAS AS LOCALIZAÇÕES * " || ddlLocalizacao.SelectedValue == "")
                ep.CodigoIndiceLocalizacao = 0;
            else
            {
                ep.CodigoIndiceLocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
            }
            if (ddlLote.SelectedValue ==" * TODOS OS LOTES * " || ddlLote.SelectedValue == "")
                ep.CodigoLote = 0;
            else
            {
                ep.CodigoLote = Convert.ToInt32(ddlLote.SelectedValue);
            }
            Session["ConMovEstoque"] = ep;
        }
        protected void PreencherDados(object sender, EventArgs e)
        {
            MovimentacaoInterna p = (MovimentacaoInterna)Session["ConMovEstoque"];

            CarregaEmpresa();
            ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
            ddlEmpresa_SelectedIndexChanged(sender, e);
            txtDtDe.Text = p.DtDe.ToString("dd/MM/yyyy");
            txtDtAte.Text = p.DtAte.ToString("dd/MM/yyyy");
            txtDocumento.Text = p.NumeroDoc.ToString();
            TxtCdTpOperacao.Text = p.CodigoTipoOperacao.ToString();
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

            if (p.CodigoLote != 0)
                ddlLote.SelectedValue = p.CodigoLote.ToString();
            else
            {
                ddlLote.SelectedValue =" * TODOS OS LOTES * ";
            }
            if (p.CodigoIndiceLocalizacao != 0)
                ddlLocalizacao.SelectedValue = p.CodigoIndiceLocalizacao.ToString();
            else
            {
                ddlLocalizacao.SelectedValue = " * TODAS AS LOCALIZAÇÕES * ";
            }
            Session["ConMovEstoque"] = null;
        }
        protected void ObterLote()
        {
            List<Lote> lstlote = new List<Lote>();
            LoteDAL LoteDAL = new LoteDAL();

            if (txtProduto.Text == "")
            {
                ddlLote.Items.Clear();
                ddlLote.DataSource = null;
                ddlLote.Items.Insert(0," * TODOS OS LOTES * ");
                return;
            }
            lstlote = LoteDAL.ListarLote2(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToInt32(txtProduto.Text));
            if (lstlote.Count > 0)
            {
                ddlLote.Items.Clear();
                ddlLote.DataSource = lstlote;
                ddlLote.DataTextField = "Cpl_DescDDL";
                ddlLote.DataValueField = "CodigoIndice";
                ddlLote.SelectedValue = null;
                ddlLote.DataBind();
                ddlLote.Items.Insert(0," * TODOS OS LOTES * ");
                ddlLote.Enabled = true;
            }
            else
            {
                ddlLote.Items.Clear();
                ddlLote.DataSource = null;
                ddlLote.Items.Insert(0," * TODOS OS LOTES * ");
            }
        }
        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            PanelSelect = "home";
            Session["TabFocada"] = "home";
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            string Descricao = " Empresa: " + ddlEmpresa.SelectedValue + " - " + ddlEmpresa.SelectedItem.Text + " - Data Inicio: " + txtDtDe.Text + " - Data Final: "  + txtDtAte.Text  + " ";

            MovimentacaoInternaDAL ep = new MovimentacaoInternaDAL();
            MovimentacaoInterna p = new MovimentacaoInterna();

            if (TxtCdTpOperacao.Text == "")
                p.CodigoTipoOperacao = 0;
            else
            {
                p.CodigoTipoOperacao = Convert.ToInt32(TxtCdTpOperacao.Text);
                Descricao += " - Tipo de Operação: " + TxtCdTpOperacao.Text + " ";
            }
            if (txtProduto.Text == "")
                p.CodigoProduto = 0;
            else
            {
                p.CodigoProduto = Convert.ToInt32(txtProduto.Text);
                Descricao += " - Prouto: " + txtProduto.Text + " - " + txtDcrproduto.Text + " ";
            }
            if (ddlLote.SelectedValue == " * TODOS OS LOTES * ")
            {
                p.CodigoLote = 0;
            }
            else
            {
                p.CodigoLote = Convert.ToInt32(ddlLote.SelectedValue);
                Descricao += " - Lote: " + ddlLote.SelectedItem.Text + " ";  
            }
            if (ddlLocalizacao.SelectedValue == " * TODAS AS LOCALIZAÇÕES * ")
            {
                p.CodigoIndiceLocalizacao = 0;
            }
            else
            {
                p.CodigoIndiceLocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
                Descricao += " - Localização: " + ddlLocalizacao.SelectedValue + " - " + ddlLocalizacao.SelectedItem.Text + " ";
            }
            if (txtDocumento.Text != "")
                Descricao += " - Documento: " + txtDocumento.Text + "";
            
            
            
            
            grdGrid.DataSource = ep.ListarMovimentacaoEstoque(Convert.ToInt32(ddlRegistros.SelectedValue), Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToDateTime(txtDtDe.Text), Convert.ToDateTime(txtDtAte.Text),
                Convert.ToInt32(p.CodigoProduto), Convert.ToInt32(p.CodigoTipoOperacao), Convert.ToInt32(p.CodigoIndiceLocalizacao),
                Convert.ToInt32(p.CodigoLote), Convert.ToString(txtDocumento.Text)).OrderByDescending(x => x.CodigoIndice).ToList();
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Movimentação(ões) de estoque(s) não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
            }
            Label label = new Label();
            
            ObterCalculoQuantidade();
            ObterDetalhesEstoques();

            Session["LST_CONMOVESTOQUE"] = ddlEmpresa.SelectedValue + ";" + txtDtDe.Text + ";" + txtDtAte.Text + ";" +
                 p.CodigoProduto.ToString() + ";" + p.CodigoTipoOperacao.ToString() + ";" + p.CodigoIndiceLocalizacao.ToString() + ";" +
                p.CodigoLote.ToString() + ";" + txtDocumento.Text;

            Session["DescricaoMovEstoque"] = Descricao.ToString();

            PanelSelect = "consulta";
            Session["TabFocada"] = "consulta";
            btnImprimir.Visible = true;
        }
        protected void ObterCalculoQuantidade()
        {
            MovimentacaoInterna MovInt = new MovimentacaoInterna();
            MovimentacaoInternaDAL MovIntDal = new MovimentacaoInternaDAL();

            int intCodlote = 0;
            int intCodlocalizacao = 0;
            int intCodOperacao = 0;
            int intCodProduto = 0;
            decimal decQtdSaida = 0;
            decimal decQtdEntrada = 0;
            decimal decQtdAjuste = 0;

            if (ddlLote.SelectedValue ==" * TODOS OS LOTES * ")
                intCodlote = 0;
            else
            {
                intCodlote = Convert.ToInt32(ddlLote.SelectedValue);
            }
            if (ddlLocalizacao.SelectedValue == " * TODAS AS LOCALIZAÇÕES * ")
                intCodlocalizacao = 0;
            else
            {
                intCodlocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
            }
            if (TxtCdTpOperacao.Text == "")
                intCodOperacao = 0;
            else
            {
                intCodOperacao = Convert.ToInt32(TxtCdTpOperacao.Text);
            }
            if (txtProduto.Text == "")
                intCodProduto = 0;
            else
            {
                intCodProduto = Convert.ToInt32(txtProduto.Text);
            }

            MovIntDal.ObterQuantidadeTpOperacoes(Convert.ToInt32(ddlRegistros.SelectedValue), Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToDateTime(txtDtDe.Text), Convert.ToDateTime(txtDtAte.Text),
                Convert.ToInt32(intCodProduto), Convert.ToInt32(intCodOperacao), Convert.ToInt32(intCodlocalizacao),
                Convert.ToInt32(intCodlote), Convert.ToString(txtDocumento.Text), ref decQtdSaida, ref decQtdEntrada, ref decQtdAjuste);

            txtQtEntrada.Text = decQtdEntrada.ToString("###,##0.000"); 
            txtQtSaida.Text = decQtdSaida.ToString("###,##0.000");
            txtQtAjuste.Text = decQtdAjuste.ToString("###,##0.000"); 
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void ObterDetalhesEstoques()
        {
            EstoqueProduto p = new EstoqueProduto();
            EstoqueProdutoDAL ep = new EstoqueProdutoDAL();

            int intCodlote = 0;
            int intCodlocalizacao = 0;
            int intCodProduto = 0;
            decimal refQtdAvaria = 0;
            decimal refQtdTotal = 0;

            if (ddlLote.SelectedValue == " * TODOS OS LOTES * ")
                intCodlote = 0;
            else
            {
                intCodlote = Convert.ToInt32(ddlLote.SelectedValue);
            }
            if (ddlLocalizacao.SelectedValue == " * TODAS AS LOCALIZAÇÕES * ")
                intCodlocalizacao = 0;
            else
            {
                intCodlocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
            } 
            if (txtProduto.Text == "")
                intCodProduto = 0;
            else
            {
                intCodProduto = Convert.ToInt32(txtProduto.Text);
            }

            ep.ObterQuantidadeAvaria(Convert.ToInt32(ddlEmpresa.SelectedValue),
                Convert.ToInt32(intCodProduto), Convert.ToInt32(intCodlocalizacao),
                Convert.ToInt32(intCodlote), ref refQtdAvaria);

            ep.ObterQuantidadeTotal(Convert.ToInt32(ddlEmpresa.SelectedValue),
                Convert.ToInt32(intCodProduto), Convert.ToInt32(intCodlocalizacao),
                Convert.ToInt32(intCodlote), ref refQtdTotal);

            txtTotal.Text = refQtdTotal.ToString("###,##0.000");
            txtAva.Text = refQtdAvaria.ToString("###,##0.000");
            txtReserv.Text = Convert.ToString("0,00");
            txtDisp.Text = (refQtdTotal - refQtdAvaria).ToString("###,##0.000");
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Estoque/RelMovEstoque.aspx");
        }

        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}


