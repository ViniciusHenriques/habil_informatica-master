using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class AbrInventario : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public string PanelSelect { get; set; }
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<Inventario> listMovimentacaoEstoque = new List<Inventario>();
        protected Boolean ValidaCampos()
        {
            //Boolean blnCampoValido = false;
            if (ddlLocalizacao1.Enabled == true)
            {
                if (ddlLocalizacao2.SelectedValue == " * Nenhum Selecionado * ")
                {
                    ShowMessage("Selecione uma localização!", MessageType.Info);
                    ddlLocalizacao2.Enabled = true;
                    ddlLocalizacao2.Focus();
                    return false;
                }
                if (ddlLocalizacao2.Enabled == true)
                {
                    if (ddlLocalizacao1.SelectedValue == " * Nenhum Selecionado * ")
                    {
                        ShowMessage("Selecione uma localização!", MessageType.Info);
                        ddlLocalizacao2.Enabled = true;
                        ddlLocalizacao1.Focus();
                        return false;
                    }
                }
                ddlLocalizacao2.Enabled = true;
            }
            if (ddlCategoria1.Enabled == true)
            {

                if (ddlCategoria2.SelectedValue == " * Nenhum Selecionado * ")
                {
                    ShowMessage("Selecione uma Categoria!", MessageType.Info);
                    ddlCategoria2.Enabled = true;
                    ddlCategoria2.Focus();
                    return false;
                }
                if (ddlCategoria2.Enabled == true)
                {
                    if (ddlCategoria1.SelectedValue == " * Nenhum Selecionado * ")
                    {
                        ShowMessage("Selecione uma Categoria!", MessageType.Info);
                        ddlCategoria2.Enabled = true;
                        ddlCategoria2.Focus();
                        return false;
                    }
                }
                ddlCategoria2.Enabled = true;

            }

            return true;
        }
        protected void LimpaCampos()
        {
            txtProduto.Text = "";
            txtDcrproduto.Text = "";
            txtProduto.Enabled = true;

            btnSalvar.Visible = true;
            carregagrid();
            CarregaEmpresa();
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
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
                return;
            }
            if (Session["TabFocada"] != null)
            {
                PanelSelect = Session["TabFocada"].ToString();
            }
            else
            if (!IsPostBack)
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

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
                        if (!x.AcessoAlterar)
                            btnSalvar.Visible = false;
                    }
                });
                if (Session["CodEmpresa"] != null)
                {
                    ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
                }
                ddlEmpresa_SelectedIndexChanged(sender, e);
                LimpaCampos();
            }            
            if (Session["Inventario"] != null)
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
            ddlLocalizacao2.Enabled = false;
            ddlCategoria2.Enabled = false;
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            panel();
            LimpaCampos();
            ddlEmpresa_SelectedIndexChanged(sender, e);
            ddlLocalizacao2.Enabled = false;
            ddlLocalizacao1.Enabled = true;
            ddlCategoria2.Enabled = false;
            ddlCategoria1.Enabled = true;
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
            
            string descricao = "";
            decimal decIndiceInventario = 0;
            int Loc1 = 0;
            int Loc2 = 0;
            int Cat1 = 0;
            int Cat2 = 0;
            int Emp = 0;
            int Prod = 0;
            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

            Inventario p = new Inventario();            
            InventarioDAL RnInventarioDAL = new InventarioDAL();

            p.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());

            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            if (he != null)
            {
                p.CodigoMaquina = he.CodigoEstacao;
            }

            if (txtProduto.Text != "")
            {
                descricao = "Empresa: " + ddlEmpresa.SelectedItem.Text + " - Produto: " + txtProduto.Text;
                Emp = Convert.ToInt32(ddlEmpresa.SelectedValue);
                Prod = Convert.ToInt32(txtProduto.Text);
            }
            if (ddlLocalizacao1.SelectedValue != " * Nenhum Selecionado * ")
            {
                descricao = "Empresa: " + ddlEmpresa.SelectedItem.Text + " - Localização De: " + ddlLocalizacao1.SelectedItem.Text + " - Até: " + ddlLocalizacao2.SelectedItem.Text;
                Emp = Convert.ToInt32(ddlEmpresa.SelectedValue);
                Loc1 = Convert.ToInt32(ddlLocalizacao1.SelectedValue);
                Loc2 = Convert.ToInt32(ddlLocalizacao2.SelectedValue);
            }
            if (ddlCategoria1.SelectedValue != " * Nenhum Selecionado * ")
            {
                descricao = "Categoria De: " + ddlCategoria1.SelectedItem.Text + " - Até: " + ddlCategoria2.SelectedItem.Text;
                Cat1 = Convert.ToInt32(ddlCategoria1.SelectedValue);
                Cat2 = Convert.ToInt32(ddlCategoria2.SelectedValue);
                Emp = Convert.ToInt32(ddlEmpresa.SelectedValue);
            }

            // pesquisa indice estoque //
            if (RnInventarioDAL.InventarioComProdutoJaAberto(Emp, Loc1, Loc2, Prod, Cat1, Cat2))
            {
                ShowMessage("Seleção de Dados Consta Produtos em Inventario", MessageType.Info);
                panel();
                return;
            }
            //Pesquisa Indice Estoque Valido//

            if (!RnInventarioDAL.IndiceEstoqueExistente(Emp, Loc1, Loc2, Prod, Cat1, Cat2))
            {
                if (txtProduto.Text != "")
                    ShowMessage("Produto não consta registro em Estoque", MessageType.Info);
                else
                    ShowMessage("Inventário não pode ser Aberto. Verifíque o Intervalo dos Dados", MessageType.Info);

                panel();
                return;
            }
            // Continua //
            p.DescInventario = descricao.ToString();
            p.NrContagem = Convert.ToInt16(ddlContagem.SelectedItem.Text);
            
            // insere Inventario //

            RnInventarioDAL.InserirInventario(p, ref decIndiceInventario);

            // insere Item do Inventario //
            RnInventarioDAL.PopulaItemDoInventario(Emp, Loc1, Loc2, Prod, Cat1, Cat2, decIndiceInventario);

            ShowMessage(" Abertura de Inventario Salva com Sucesso! ", MessageType.Success);
           
            panel();
            carregagrid();
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
        }
        protected void CarregaLocalizacao()
        {
            if (ddlLocalizacao1.SelectedValue != null || ddlLocalizacao1.SelectedValue != " * Nenhum Selecionado * ")
            {
                LocalizacaoDAL lc = new LocalizacaoDAL();

                ddlLocalizacao1.Items.Clear();
                ddlLocalizacao1.DataSource = lc.ListarLocalizacao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
                ddlLocalizacao1.DataTextField = "Cpl_DescDDL";
                ddlLocalizacao1.DataValueField = "CodigoIndice";
                ddlLocalizacao1.SelectedValue = null;
                ddlLocalizacao1.DataBind();
                ddlLocalizacao1.Items.Insert(0, " * Nenhum Selecionado * ");
            }
            else
            {
                ddlLocalizacao1.Items.Insert(0, " * Nenhum Selecionado * ");
            }
            if (ddlLocalizacao2.SelectedValue != null || ddlLocalizacao2.SelectedValue != " * Nenhum Selecionado * ")
            {
                LocalizacaoDAL lc = new LocalizacaoDAL();

                ddlLocalizacao2.Items.Clear();
                ddlLocalizacao2.DataSource = lc.ListarLocalizacao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
                ddlLocalizacao2.DataTextField = "Cpl_DescDDL";
                ddlLocalizacao2.DataValueField = "CodigoIndice";
                ddlLocalizacao2.SelectedValue = null;
                ddlLocalizacao2.DataBind();
                ddlLocalizacao2.Items.Insert(0, " * Nenhum Selecionado * ");
            }
            else
            {
                ddlLocalizacao2.Items.Insert(0, " * Nenhum Selecionado * ");
            }
        }
        protected void CarregaCategoria()
        {
            CategoriaDAL C = new CategoriaDAL();

            ddlCategoria1.Items.Clear();
            ddlCategoria1.DataSource = C.ListarCategorias("", "", "", "CD_CATEGORIA");
            ddlCategoria1.DataTextField = "Cpl_DescDDL";
            ddlCategoria1.DataValueField = "CodigoIndice";
            ddlCategoria1.SelectedValue = null;
            ddlCategoria1.DataBind();
            ddlCategoria1.Items.Insert(0, " * Nenhum Selecionado * ");

            ddlCategoria2.Items.Clear();
            ddlCategoria2.DataSource = C.ListarCategorias("", "", "", "CD_CATEGORIA");
            ddlCategoria2.DataTextField = "Cpl_DescDDL";
            ddlCategoria2.DataValueField = "CodigoIndice";
            ddlCategoria2.SelectedValue = null;
            ddlCategoria2.DataBind();
            ddlCategoria2.Items.Insert(0, " * Nenhum Selecionado * ");
        }
        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaLocalizacao();
            CarregaCategoria();
        }
        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            CompactaDados(sender, e);
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=6");
        }
        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtProduto.Text.Equals("LIMPAR"))
            {
                txtProduto.Text = "";
                txtDcrproduto.Text = "";
                return;
            }
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
                    txtDcrproduto.Text = produto.DescricaoProduto;
                else
                {
                    ShowMessage("Produto está com situação inativa", MessageType.Info);
                    txtDcrproduto.Text = "";
                }
            }
            else
            {
                ShowMessage("Produto não cadastrado", MessageType.Info);
            }
            Session["TabFocada"] = null;
            txtProduto.Focus();
            if (Session["CodEmpresa"] != null)
            {
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
            }
            ddlCategoria1.Enabled = false;
            ddlCategoria2.Enabled = false;
            ddlLocalizacao1.Enabled = false;
            ddlLocalizacao2.Enabled = false;
        }
        protected void CompactaDados(object sender, EventArgs e)
        {
            Inventario p = new Inventario();

            p.NrContagem = Convert.ToInt16(ddlContagem.SelectedValue);

            Session["Inventario"] = p;
        }
        protected void PreencherDados(object sender, EventArgs e)
        {
            Inventario p = (Inventario)Session["Inventario"];
            CarregaEmpresa();

            Session["Inventario"] = null;
        }
        protected void ddlLocalizacao1_TextChanged(object sender, EventArgs e)
        {
            ddlCategoria1.Enabled = false;
            ddlCategoria2.Enabled = false;
            txtProduto.Enabled = false;
            ddlLocalizacao2.Enabled = true;
        }
        protected void ddlLocalizacao2_TextChanged(object sender, EventArgs e)
        {
            ddlLocalizacao2.Enabled = true;
            //ddlCategoria1.Enabled = false;
            //ddlCategoria2.Enabled = false;
            //txtProduto.Enabled = false;
        }
        protected void ddlCategoria2_TextChanged(object sender, EventArgs e)
        {
            //txtProduto.Enabled = false;
            //ddlLocalizacao1.Enabled = false;
            //ddlLocalizacao2.Enabled = false;
            ddlCategoria2.Enabled = true;
        }
        protected void ddlCategoria1_TextChanged(object sender, EventArgs e)
        {
            txtProduto.Enabled = false;
            ddlLocalizacao1.Enabled = false;
            ddlLocalizacao2.Enabled = false;
            ddlCategoria2.Enabled = true;
        }
        protected void panel()
        {
            PanelSelect = "home";
            Session["TabFocada"] = "home";
        }
        protected void carregagrid()
        {
            InventarioDAL RnInventarioDAL = new InventarioDAL();
            grdGrid.DataSource = RnInventarioDAL.ListarGrid().OrderByDescending(x => x.CodigoIndice).ToList();
            grdGrid.DataBind();
        }
    }
}