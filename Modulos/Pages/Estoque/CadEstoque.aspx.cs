using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class CadEstoque : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<EstoqueProduto> listEstoqueProduto = new List<EstoqueProduto>();
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

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
            v.CampoValido("Quantidade", txtquantidade.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtquantidade.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtquantidade.Focus();
                }
                return false;
            }
            if (Convert.ToDecimal(txtquantidade.Text) == 0)
            {
                ShowMessage("Quantidade deve ser maior que zero", MessageType.Info);
                txtquantidade.Focus();
                return false;
            }
            if (ddlLocalizacao.SelectedValue == " * Nenhum Selecionado * ")
            {
                ShowMessage("Selecione uma localização!", MessageType.Info);
                return false;
            }
            if (txtLancamento.Text == "Novo")
            {
                EstoqueProduto ep = new EstoqueProduto();
                EstoqueProdutoDAL epDAL = new EstoqueProdutoDAL();

                if (ddlLote.SelectedValue == "..... SEM LOTE .....")
                {
                    ep = epDAL.LerEstoque(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToString(ddlLocalizacao.SelectedValue), Convert.ToInt32(txtProduto.Text), Convert.ToInt32(0));
                }
                else
                {
                    ep = epDAL.LerEstoque(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToString(ddlLocalizacao.SelectedValue), Convert.ToInt32(txtProduto.Text), Convert.ToInt32(ddlLote.SelectedValue));
                }
                if (ep != null)
                {
                    ShowMessage("Estoque já cadastrado", MessageType.Info);
                    ddlEmpresa.Focus();

                    return false;
                }
            }
            return true;
        }
        protected void LimpaCampos()
        {
            ddlEmpresa.Text = "";
            ddlLocalizacao.Items.Insert(0, " * Nenhum Selecionado * ");
            ddlLote.Items.Insert(0, "..... SEM LOTE .....");
            txtProduto.Text = "";
            ddlSituacao.Text = "";
            txtquantidade.Text = "0,00";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtLancamento.Focus();

            if (Session["ZoomEstoqueProduto2"] != null)
            {
                if (Session["ZoomEstoqueProduto2"].ToString() == "RELACIONAL")
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
                Session["MensagemTela"] = "";
            }


            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt16(Session["CodModulo"].ToString()),
                                           Convert.ToInt16(Session["CodPflUsuario"].ToString()),
                                           "ConEstoqueProduto.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomEstoqueProduto2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomEstoqueProduto"] != null)
                {
                    string s = Session["ZoomEstoqueProduto"].ToString();
                    Session["ZoomEstoqueProduto"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                        if (txtLancamento.Text == "")
                        {
                            txtLancamento.Text = word;
                            txtLancamento.Enabled = false;

                            EstoqueProdutoDAL r = new EstoqueProdutoDAL();
                            EstoqueProduto p = new EstoqueProduto();

                            CarregaSituacoes();
                            ddlEmpresa_SelectedIndexChanged(sender, e);
                            CarregaLote();

                            p = r.PesquisarEstoqueIndice(Convert.ToInt32(txtLancamento.Text));
                            ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
                            ddlLocalizacao.SelectedValue = p.CodigoIndiceLocalizacao.ToString();

                            if (p.CodigoLote == 0)
                            {
                                //ddlLote.SelectedValue == "..... SEM LOTE .....";
                                ddlLote.Items.Insert(0, "..... SEM LOTE .....");
                            }
                            else
                            {
                                ddlLote.SelectedValue = p.CodigoLote.ToString();
                            }
                            txtProduto.Text = p.CodigoProduto.ToString();
                            txtProduto_TextChanged(sender, e);
                            ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                            txtquantidade.Text = p.Quantidade.ToString();                               

                            ddlEmpresa.Enabled = false;
                            ddlLocalizacao.Enabled = false;
                            txtProduto.Enabled = false;
                            ddlLote.Enabled = false;
                            btnPesquisarItem.Enabled = false;

                            lista.ForEach(delegate (Permissao x)
                            {
                                if (!x.AcessoCompleto)
                                {
                                    if (!x.AcessoAlterar)
                                        btnSalvar.Visible = false;

                                    if (!x.AcessoExcluir)
                                        btnExcluir.Visible = false;
                                }
                            }
                            );
                            return;
                        }
                    }
                }
                else
                {
                    txtLancamento.Text = "Novo";
                    CarregaSituacoes();
                    btnExcluir.Visible = false;

                    ddlEmpresa.Enabled = true;

                    if (Session["CodEmpresa"] != null)
                    {
                        ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
                    }
                    ddlEmpresa_SelectedIndexChanged(sender, e);

                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = true;
                        }
                    });
                }
                if (Session["ZoomProduto"] != null)
                {
                    string[] s = Session["ZoomProduto"].ToString().Split('³');

                    txtProduto.Text = s[0].ToString();

                    txtProduto_TextChanged(sender, e);
                    Session["ZoomProduto"] = null;
                }
            }
            if (Session["CodUsuario"] == "-150380")
            {
                btnSalvar.Visible = true;
            }

            if (txtLancamento.Text == "")
                btnVoltar_Click(sender, e);
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtLancamento.Text.Trim() != "")
                {
                    EstoqueProdutoDAL d = new EstoqueProdutoDAL();
                    d.Excluir(Convert.ToInt16(txtLancamento.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Estoque não identificado.&emsp;&emsp;&emsp;";

            }
            catch (Exception ex)
            {
                strErro = "&emsp;&emsp;&emsp;" + ex.Message.ToString() + "&emsp;&emsp;&emsp;";
            }

            if (strErro != "")
            {
                lblMensagem.Text = strErro;
                pnlMensagem.Visible = true;
            }
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomEstoqueProduto"] = null;
            if (Convert.ToInt16(Request.QueryString["Cad"]) == 1)
            {
                Response.Redirect("~/Pages/Estoque/CadEstoque.aspx");
            }
            if (Session["ZoomEstoqueProduto2"] != null)
            {
                Session["ZoomEstoqueProduto2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadEstoque.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }
            Response.Redirect("~/Pages/Estoque/ConEstoque.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            EstoqueProduto p = new EstoqueProduto();
            EstoqueProdutoDAL d = new EstoqueProdutoDAL();

            p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            p.CodigoIndiceLocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
            if (ddlLote.SelectedValue == "..... SEM LOTE .....")
            {
                EstoqueProduto ep = new EstoqueProduto();

                ep.CodigoLote = 0;
            }
            else
            {
                p.CodigoLote = Convert.ToInt32(ddlLote.SelectedValue);
            }
            p.CodigoProduto = Convert.ToInt32(txtProduto.Text);
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.Quantidade = Convert.ToDecimal(txtquantidade.Text);

            if (txtLancamento.Text == "Novo")
            {

                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p.CodigoIndice = Convert.ToInt32(txtLancamento.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }

            if (Convert.ToInt16(Request.QueryString["cad"]) == 1)
            {
                Session["MensagemTela"] = null;
                Response.Redirect("~/Pages/Produtos/CadEstoqueProduto.aspx");

            }
            else
                btnVoltar_Click(sender, e);
        }
        protected void CarregaSituacoes()
        {
            EmpresaDAL ep = new EmpresaDAL();
            ddlEmpresa.DataSource = ep.ListarEmpresas("", "", "", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();

            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.TipoEstoque();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();
        }
        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            CompactaEstoque(sender, e);
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=2");
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
                            CarregaLote();
                        }
                        else
                        {
                            ddlLote.Items.Insert(0, "..... SEM LOTE .....");
                            ddlLote.Enabled = false;
                        }
                    }
                    else
                    {
                        ShowMessage("Produto do Tipo: Serviço, não pode ser Lançado", MessageType.Info);
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
            txtquantidade.Focus();
            ddlEmpresa_SelectedIndexChanged(sender, e);
        }
        protected void CompactaEstoque(object sender, EventArgs e)
        {
            EstoqueProduto ep = new EstoqueProduto();
            if (txtLancamento.Text == "Novo")
            {
                ep.CodigoIndice = 0;
            }
            else
            {
                ep.CodigoIndice = Convert.ToInt32(txtLancamento.Text);
            }
            if (ddlLocalizacao.SelectedValue == " * Nenhum Selecionado * " || ddlLote.SelectedValue == "..... SEM LOTE .....")
            {
                ep.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                ep.Quantidade = Convert.ToDecimal(txtquantidade.Text);
                Session["EstoqueProduto"] = ep;
            }
            else
            {
                ep.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
                ep.CodigoIndiceLocalizacao = Convert.ToInt32(ddlLocalizacao.SelectedValue);
                ep.CodigoLote = Convert.ToInt32(ddlLote.SelectedValue);
                ep.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                ep.Quantidade = Convert.ToDecimal(txtquantidade.Text);
                Session["EstoqueProduto"] = ep;
            }
        }
        protected void PreencherDados(object sender, EventArgs e)
        {
            EstoqueProduto ep = (EstoqueProduto)Session["EstoqueProduto"];

            if (ep.CodigoIndice == 0)
            {
                txtLancamento.Text = "Novo";
            }
            else
            {
                txtLancamento.Text = ep.CodigoIndice.ToString();
            }
            if (ddlLocalizacao.SelectedValue == null || ddlLote.SelectedValue == null)
            {
                ddlEmpresa.SelectedValue = ep.CodigoEmpresa.ToString();
                ddlSituacao.SelectedValue = ep.CodigoSituacao.ToString();
                txtquantidade.Text = ep.Quantidade.ToString();
                Session["EstoqueProduto"] = null;
            }
            else
            {
                ddlEmpresa.SelectedValue = ep.CodigoEmpresa.ToString();
                ddlLocalizacao.SelectedValue = ep.CodigoIndiceLocalizacao.ToString();
                ddlLote.SelectedValue = ep.CodigoLote.ToString();
                ddlSituacao.SelectedValue = ep.CodigoSituacao.ToString();
                txtquantidade.Text = ep.Quantidade.ToString();
                Session["EstoqueProduto"] = null;
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
        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlLocalizacao.SelectedValue != null)
            {
                LocalizacaoDAL lc = new LocalizacaoDAL();
                ddlLocalizacao.DataSource = lc.ListarLocalizacao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "CD_LOCALIZACAO");
                ddlLocalizacao.DataTextField = "CodigoLocalizacao";
                ddlLocalizacao.DataValueField = "CodigoIndice";
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
        protected void CarregaLote()
        {
            if (txtProduto.Text != "")
            {
                LoteDAL L = new LoteDAL();
                List<Lote> lstlote = new List<Lote>();

                lstlote = L.ListarLoteDisponivel(Convert.ToInt32(ddlEmpresa.SelectedValue), Convert.ToInt32(txtProduto.Text), 1);
                if (lstlote.Count > 0)
                {
                    ddlLote.DataSource = lstlote;
                    ddlLote.DataTextField = "Cpl_DescDDL";
                    ddlLote.DataValueField = "CodigoIndice";
                    ddlLote.DataBind();
                    ddlLote.Items.Insert(0, "..... SEM LOTE .....");
                    ddlLote.Enabled = true;
                }
                else
                {
                    ddlLote.Items.Clear();
                    ddlLote.DataSource = null;
                    ddlLote.Items.Insert(0, "..... SEM LOTE .....");
                    ddlLote.Enabled = true;
                }
            }
        }
    }
}
