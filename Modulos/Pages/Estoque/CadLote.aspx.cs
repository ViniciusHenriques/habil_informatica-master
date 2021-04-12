using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class CadLote : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<Lote> listLote = new List<Lote>();
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Número do Lote", txtNumero.Text, true, true, true, false, "NVARCHAR", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtNumero.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;
            }

            if (Convert.ToString(txtNumero.Text) == "")
            {
                ShowMessage("Número não deve estar em Branco", MessageType.Info);
                txtNumero.Focus();
                return false;
            }
            if (txtLancamento.Text == "Novo")
            {
                Lote ep = new Lote();
                LoteDAL epDAL = new LoteDAL();

                ep = epDAL.PesquisarNumeroLote(Convert.ToString(txtNumero.Text), Convert.ToInt32(txtProduto.Text));

                if (ep != null)
                {
                    ShowMessage("Número de Lote já cadastrado", MessageType.Info);
                    txtNumero.Focus();

                    return false;
                }
            }

            v.CampoValido("Série do Lote", txtSerie.Text, true, true, true, false, "NVARCHAR", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtSerie.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;

            }

            if (Convert.ToString(txtSerie.Text) == "")
            {
                ShowMessage("Série não deve estar em Branco", MessageType.Info);
                txtSerie.Focus();
                return false;
            }

            v.CampoValido("Data de Validade", txtdtvalidade.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtdtvalidade.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;
            }
            DateTime data1, data2;
            data1 = Convert.ToDateTime(txtdtvalidade.Text);
            data2 = DateTime.Today;
            if (data1 < data2)
            {
                ShowMessage("Data de Validade deve ser após a data de hoje!", MessageType.Info);
                txtdtvalidade.Focus();
                return false;
            }

            
            v.CampoValido("Data de Fabricacao", txtdtfabricacao.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtdtfabricacao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;
            }
            DateTime data3, data4;
            data3 = Convert.ToDateTime(txtdtfabricacao.Text);
            data4 = DateTime.Today;
            if (data4 < data3)
            {
                ShowMessage("Data de Validade deve ser antes da data de hoje!", MessageType.Info);
                txtdtfabricacao.Focus();
                return false;
            }

            v.CampoValido("Quantidade de lote", txtquantidade.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

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

            return true;
        }

        protected void LimpaCampos()
        {
            ddlEmpresa.Text = "";
            txtSerie.Text = "";
            txtNumero.Text = "";
            txtProduto.Text = "";
            ddlSituacao.Text = "";
            txtquantidade.Text = "0,00";
            txtdtfabricacao.Text = "";
            txtdtvalidade.Text = "";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtLancamento.Focus();

            if (Session["ZoomLote2"] != null)
            {
                if (Session["ZoomLote2"].ToString() == "RELACIONAL")
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
                                           "ConLote.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomLote2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomLote"] != null)
                {
                    string s = Session["ZoomLote"].ToString();
                    Session["ZoomLote"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtLancamento.Text == "")
                            {
                                txtLancamento.Text = word;
                                txtLancamento.Enabled = false;

                                LoteDAL r = new LoteDAL();
                                Lote p = new Lote();

                                CarregaSituacoes();
                                //Indice
                                p = r.PesquisarLoteIndice(Convert.ToInt32(txtLancamento.Text));
                                ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
                                //Produto
                                txtProduto.Text = p.CodigoProduto.ToString();
                                txtProduto_TextChanged(sender, e);
                                //Situação
                                ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                //Numero de Lote
                                txtNumero.Text = p.NumeroLote.ToString();
                                //Série de Lote
                                txtSerie.Text = p.SerieLote.ToString();
                                //Quantidade
                                txtquantidade.Text = p.QuantidadeLote.ToString();
                                //Validade de Lote
                                txtdtvalidade.Text = p.DataValidade.ToString().Substring(0, 10);
                                txtdtfabricacao.Text = p.DataFabricacao.ToString().Substring(0, 10);
                                DateTime data1, data2;
                                data1 = Convert.ToDateTime(txtdtvalidade.Text);
                                data2 = DateTime.Today;
                                if (data1 < data2)
                                {
                                    txtquantidade.Enabled = false;
                                }
                                else
                                {
                                    txtquantidade.Enabled = true;
                                }

                                ddlEmpresa.Enabled = false;
                                txtNumero.Enabled = false;
                                txtSerie.Enabled = false;
                                txtProduto.Enabled = false;
                                txtdtvalidade.Enabled = false;
                                txtdtfabricacao.Enabled = false;
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
                                });

                                return;
                            }
                    }
                }
                else
                {
                    txtLancamento.Text = "Novo";
                    CarregaSituacoes();
                    btnExcluir.Visible = false;
                    txtdtvalidade.Enabled = true;
                    txtdtfabricacao.Enabled = true;
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
                    PreencherDados(sender, e);
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
                    LoteDAL d = new LoteDAL();
                    d.Excluir(Convert.ToInt16(txtLancamento.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Lote não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomLote"] = null;
            if (Convert.ToInt16(Request.QueryString["Cad"]) == 1)
            {
                Response.Redirect("~/Pages/Estoque/CadLote.aspx");

            }
            if (Session["ZoomLote2"] != null)
            {
                Session["ZoomLote2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadLote.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Response.Redirect("~/Pages/Estoque/ConLote.aspx");
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            Lote p = new Lote();
            LoteDAL d = new LoteDAL();
            //string loteNr;


            p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            p.CodigoProduto = Convert.ToInt32(txtProduto.Text);
            p.SerieLote = Convert.ToString(txtSerie.Text);


                p.NumeroLote = Convert.ToString(txtNumero.Text);
            p.QuantidadeLote = Convert.ToDecimal(txtquantidade.Text);
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.DataValidade = Convert.ToDateTime(txtdtvalidade.Text);
            p.DataFabricacao = Convert.ToDateTime(txtdtfabricacao.Text);




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
                Response.Redirect("~/Pages/Estoque/CadLote.aspx");

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
            ddlSituacao.DataSource = sd.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();
        }

        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            CompactaLote(sender, e);
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=3");
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
                if (produto.CodigoTipoProduto != 29)
                {
                    if (produto.ControlaLote == true)
                    {
                        txtDcrproduto.Text = produto.DescricaoProduto;
                    }
                    else
                    {
                        txtDcrproduto.Text = "";
                        txtProduto.Text = "";
                        ShowMessage("Produto não controla Lote", MessageType.Info);
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
                ShowMessage("Produto não cadastrado", MessageType.Info);
            }
            Session["TabFocada"] = null;
            txtquantidade.Focus();
        }

        protected void CompactaLote(object sender, EventArgs e)
        {
            Lote p = new Lote();
            if (txtLancamento.Text == "Novo")
            {
                p.CodigoIndice = 0;
            }
            else
            {
                p.CodigoIndice = Convert.ToInt32(txtLancamento.Text);
            }
            
            p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            p.SerieLote = Convert.ToString(txtSerie.Text);
            p.NumeroLote = Convert.ToString(txtNumero.Text);
            //p.DataValidade = Convert.ToDateTime(txtdtvalidade.Text);
            p.QuantidadeLote = Convert.ToDecimal(txtquantidade.Text);
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.Text);
            Session["Lote"] = p;
        }

        protected void PreencherDados(object sender, EventArgs e)
        {
            Lote p = (Lote)Session["Lote"];

            if (p.CodigoIndice == 0)
            {
                txtLancamento.Text = "Novo";
            }
            else
            {
                txtLancamento.Text = p.CodigoIndice.ToString();
            }
            if (Convert.ToString(p.DataValidade) != "01/01/0001 00:00:00")
                txtdtvalidade.Text = p.DataValidade.ToString().Substring(0, 10);
            if (Convert.ToString(p.DataValidade) != "01/01/0001 00:00:00")
                txtdtfabricacao.Text = p.DataFabricacao.ToString().Substring(0, 10);

            ddlEmpresa.SelectedValue = p.CodigoEmpresa.ToString();
            txtSerie.Text = p.SerieLote.ToString();
            txtNumero.Text = p.NumeroLote.ToString();
            ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
            txtquantidade.Text = p.QuantidadeLote.ToString();
            //txtdtvalidade.Text = p.DataValidade.ToShortDateString();
            Session["Lote"] = null;

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

            //LocalizacaoDAL lc = new LocalizacaoDAL();
            //ddlLocalizacao.DataSource = lc.ListarLocalizacao("CD_EMPRESA", "INT", ddlEmpresa.SelectedValue, "");
            //ddlLocalizacao.DataTextField = "CodigoLocalizacao";
            //ddlLocalizacao.DataValueField = "CodigoIndice";
            //ddlLocalizacao.DataBind();
            //ddlLocalizacao.Items.Insert(0, " * Nenhum Selecionado * ");
            //ddlLocalizacao.Enabled = true;

        }

    }
}
