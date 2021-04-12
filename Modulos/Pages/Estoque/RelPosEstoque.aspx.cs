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
    public partial class RelPosEstoque : System.Web.UI.Page
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

            v.CampoValido("Data de Referência", txtDtRef.Text, true, false, false, false, "DATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtDtRef.Text = "";
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

            txtProduto.Text = "";
            txtDcrproduto.Text = "";
            txtDtRef.Text = Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("dd/MM/yyyy"));

            pnlCon.Visible = true;
            pnlRel.Visible = false;
            btnVoltar.Visible = false;
            CRViewer.Visible = false;
            lbnPagName.Text = "Relatório de Posição de Estoque";

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
            if (ddlLocalizacao2.SelectedValue != null || ddlLocalizacao2.SelectedValue != " * TODAS AS LOCALIZAÇÕES * ")
            {
                LocalizacaoDAL lc = new LocalizacaoDAL();

                ddlLocalizacao2.Items.Clear();
                ddlLocalizacao2.DataSource = lc.ListarLocalizacao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
                ddlLocalizacao2.DataTextField = "CodigoLocalizacao";
                ddlLocalizacao2.DataValueField = "CodigoIndice";
                ddlLocalizacao2.SelectedValue = null;
                ddlLocalizacao2.DataBind();
                ddlLocalizacao2.Items.Insert(0, " * TODAS AS LOCALIZAÇÕES * ");
                ddlLocalizacao2.Enabled = true;
            }
            else
            {
                ddlLocalizacao2.Items.Insert(0, " * TODAS AS LOCALIZAÇÕES * ");
                ddlLocalizacao2.Enabled = true;
            }
        }
        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            CompactaDados(sender, e);
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=8");
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
        }
        protected void CompactaDados(object sender, EventArgs e)
        {
            MovimentacaoInterna ep = new MovimentacaoInterna();

            ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
           
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
            Session["RelPosEst"] = ep;
        }
        protected void PreencherDados(object sender, EventArgs e)
        {
            MovimentacaoInterna p = (MovimentacaoInterna)Session["RelPosEst"];

            CarregaEmpresa();
            ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
            ddlEmpresa_SelectedIndexChanged(sender, e);
            
            if (p.CodigoProduto == 0)
                txtProduto.Text = "";
            else
            {
                txtProduto.Text = p.CodigoProduto.ToString();
            }
            if (p.CodigoIndiceLocalizacao != 0)
                ddlLocalizacao.SelectedValue = p.CodigoIndiceLocalizacao.ToString();
            else
            {
                ddlLocalizacao.SelectedValue = " * TODAS AS LOCALIZAÇÕES * ";
            }
            Session["RelMovEntSai"] = null;
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

            int intCodigoIndiceLocalizacao = 0;

            string Descricao = " Empresa: " + ddlEmpresa.SelectedValue + " - " + ddlEmpresa.SelectedItem.Text + " - Data de Referência: " + txtDtRef.Text;

            MovimentacaoInternaDAL RnMovimentacaoInternaDAL = new MovimentacaoInternaDAL();
            MovimentacaoInterna p = new MovimentacaoInterna();            

            if (txtProduto.Text == "")
                p.CodigoProduto = 0;
            else
            {
                p.CodigoProduto = Convert.ToInt32(txtProduto.Text);
                Descricao += " - Prouto: " + txtProduto.Text + " - " + txtDcrproduto.Text + " ";
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
            if (ddlLocalizacao2.SelectedValue != " * TODAS AS LOCALIZAÇÕES * ")
            {
                intCodigoIndiceLocalizacao = Convert.ToInt32(ddlLocalizacao2.SelectedValue);
                Descricao += " - Localização: " + ddlLocalizacao.SelectedValue + " - " + ddlLocalizacao.SelectedItem.Text + " ";
            }

            RptDoc.Load(Server.MapPath("~/Pages/Estoque/RPT/RelPosEst.rpt"));

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            if (Descricao != "")
            {
                RptDoc.DataDefinition.FormulaFields["DescricaoPosicao"].Text = "'" + Descricao + "'";
            }
            RptDoc.DataDefinition.FormulaFields["Data"].Text = "'" + txtDtRef.Text + "'";

            RptDoc.SetDataSource(RnMovimentacaoInternaDAL.RelPosicaoEstoque(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToDateTime(txtDtRef.Text),
                Convert.ToInt32(p.CodigoProduto),Convert.ToInt32(p.CodigoIndiceLocalizacao),
                Convert.ToInt32(intCodigoIndiceLocalizacao)));

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


