using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using CrystalDecisions.CrystalReports.Engine;
using static System.Console;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class RelMovEntSai : System.Web.UI.Page
    {
        ReportDocument RptDoc;
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
            txtProduto.Text = "";
            ddlLote.Items.Clear();
            ddlLote.Items.Insert(0," * TODOS OS LOTES * ");
            ddlLote.Enabled = true;
            txtDcrproduto.Text = "";
            txtDtAte.Text = Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("dd/MM/yyyy"));
            txtDtDe.Text = Convert.ToDateTime(txtDtAte.Text).AddDays(-14).ToString("dd/MM/yyyy");

            pnlCon.Visible = true;
            pnlRel.Visible = false;
            btnVoltar.Visible = false;
            CRViewer.Visible = false;
            lbnPagName.Text = "Relatório de Movimentação de Estoque: Tipos de Entradas ou Saídas";

            CarregaEmpresa();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (!IsPostBack)
            {
                ddlEmpresa_SelectedIndexChanged(sender, e);
                LimpaCampos();
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
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoImprimir)
                        {

                            CRViewer.HasExportButton = false;
                            CRViewer.HasPrintButton = false;
                            CRViewer.HasToggleGroupTreeButton = false;

                        }
                    }
                });
                
            }         
            if (Session["CodEmpresa"] != null)
            {
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
            }
            if (Session["RelMovEntSai"] != null)
            {
                PreencherDados(sender, e);

                if (Session["ZoomProduto"] != null)
                {
                    string[] s = Session["ZoomProduto"].ToString().Split('³');

                    txtProduto.Text = s[0].ToString();

                    Session["ZoomProduto"] = null;
                }

                txtProduto_TextChanged(sender, e);
            }

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
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=7");
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
            ObterLote();
        }
        protected void CompactaDados(object sender, EventArgs e)
        {
            MovimentacaoInterna ep = new MovimentacaoInterna();

            ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            ep.DtDe = Convert.ToDateTime(txtDtDe.Text);
            ep.DtAte = Convert.ToDateTime(txtDtAte.Text);
            ep.NumeroDoc = Convert.ToString(txtDocumento.Text);
           
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
            Session["RelMovEntSai"] = ep;
        }
        protected void PreencherDados(object sender, EventArgs e)
        {
            MovimentacaoInterna p = (MovimentacaoInterna)Session["RelMovEntSai"];

            CarregaEmpresa();
            ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
            ddlEmpresa_SelectedIndexChanged(sender, e);
            txtDtDe.Text = p.DtDe.ToString("dd/MM/yyyy");
            txtDtAte.Text = p.DtAte.ToString("dd/MM/yyyy");
            txtDocumento.Text = p.NumeroDoc.ToString();
            
            if (p.CodigoProduto == 0)
                txtProduto.Text = "";
            else
            {
                txtProduto.Text = p.CodigoProduto.ToString();
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
            Session["RelMovEntSai"] = null;
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
            btnSair.Visible = true;
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RptDoc"] = null;
                RptDoc = null;
                //MontaCrystal();
            }
            if (Session["RptDoc"] != null)
            {
                CRViewer.ReportSource = (ReportDocument)Session["RptDoc"];
                CRViewer.DataBind();
            }

        }
        private void MontaCrystal()
        {
            RptDoc = new ReportDocument();

            string Descricao = " Empresa: " + ddlEmpresa.SelectedValue + " - " + ddlEmpresa.SelectedItem.Text + " - Data Inicio: " + txtDtDe.Text + " - Data Final: " + txtDtAte.Text + " ";

            MovimentacaoInternaDAL RnMovimentacaoInternaDAL = new MovimentacaoInternaDAL();
            MovimentacaoInterna p = new MovimentacaoInterna();

            if (ddlTpOper.Text == "")
                p.CodigoTipoOperacao = 0;
            else
            {
                p.CodigoTipoOperacao = 0;
                Descricao += " - Tipo de Operação: " + ddlTpOper.Text + " ";
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

            Label label = new Label();

            RptDoc.Load(Server.MapPath("~/Pages/Estoque/RPT/RelMovEstoque.rpt"));

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            if (Descricao != "")
            {
                RptDoc.DataDefinition.FormulaFields["Descricao"].Text = "'" + Descricao + "'";
            }
            if (ddlTpOper.SelectedValue == "1")
            {
                RptDoc.DataDefinition.FormulaFields["TpOper"].Text = "'2'";
                RptDoc.SetDataSource(RnMovimentacaoInternaDAL.RelMovEstoque(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToDateTime(txtDtDe.Text), Convert.ToDateTime(txtDtAte.Text),
            Convert.ToInt32(p.CodigoProduto), Convert.ToInt32(p.CodigoTipoOperacao), Convert.ToInt32(p.CodigoIndiceLocalizacao),
                Convert.ToInt32(p.CodigoLote), Convert.ToString(txtDocumento.Text), 2));
                lbnPagName.Text = "Relatório de Movimentação de Estoque de Entrada";
            }
            if (ddlTpOper.SelectedValue == "2")
            {
                RptDoc.DataDefinition.FormulaFields["TpOper"].Text = "'3'";
                RptDoc.SetDataSource(RnMovimentacaoInternaDAL.RelMovEstoque(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToDateTime(txtDtDe.Text), Convert.ToDateTime(txtDtAte.Text),
            Convert.ToInt32(p.CodigoProduto), Convert.ToInt32(p.CodigoTipoOperacao), Convert.ToInt32(p.CodigoIndiceLocalizacao),
                Convert.ToInt32(p.CodigoLote), Convert.ToString(txtDocumento.Text), 3));
                lbnPagName.Text = "Relatório de Movimentação de Estoque de Saída";
            }

            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
            btnVoltar.Visible = false;
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            btnSair.Visible = true;
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            MontaCrystal();
            btnSair.Visible = false;
            pnlCon.Visible = false;
            pnlRel.Visible = true;
            btnVoltar.Visible = true;
            CRViewer.Visible = true;
        }
    }
}


