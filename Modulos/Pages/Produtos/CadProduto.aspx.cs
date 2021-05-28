using System;
using System.Collections.Generic;
using DAL.Model;
using DAL.Persistence;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;
using System.IO;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class CadProduto : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        public string MaskCategoria { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<Produto> listCadProduto = new List<Produto>();
        protected Boolean ValidaCampos()
        {

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }


            Boolean blnCampoValido = false;

            v.CampoValido("Descrição do Produto", txtDscProduto.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDscProduto.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDscProduto.Focus();

                }

                return false;
            }
            v.CampoValido("Código Sistema Anterior", txtCodSisAnterior.Text, false, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodSisAnterior.Focus();
                }
                return false;
            }
            v.CampoValido("Valor Volume", txtVolume.Text, false, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtVolume.Focus();
                }
                return false;
            }
            v.CampoValido("Valor peso", txtPeso.Text, false, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtPeso.Focus();
                }
                return false;
            }
            v.CampoValido("Valor fator de cubagem", txtFatorCubagem.Text, false, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtFatorCubagem.Focus();
                }
                return false;
            }
            if (ddlMarca.SelectedValue == "..... SELECIONE A MARCA .....")
            {
                ShowMessage("Selecione a Marca...", MessageType.Info);
                ddlMarca.Focus();
                PanelSelect = "home";
                return false;
            }
            if (ddlFabricante.SelectedValue == "..... SELECIONE O FABRICANTE .....")
            {
                ShowMessage("Selecione o Fabricante...", MessageType.Info);
                ddlFabricante.Focus();
                PanelSelect = "home";
                return false;
            }

            if (ddlGpoTribProduto.SelectedValue == "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PRODUTOS .....")
            {
                ShowMessage("Selecione um grupo de tributação de produtos.", MessageType.Info);
                ddlGpoTribProduto.Focus();
                PanelSelect = "profile";
                return false;
            }

            //if (Convert.ToDecimal(txtVolume.Text) <= 0)
            //{
            //    ShowMessage("Digite um volume válido", MessageType.Info);
            //    txtVolume.Focus();
            //    return false;
            //}
            //if (Convert.ToDecimal(txtPeso.Text) <= 0)
            //{
            //    ShowMessage("Digite um peso válido", MessageType.Info);
            //    txtPeso.Focus();
            //    return false;
            //}
            //if (Convert.ToDecimal(txtFatorCubagem.Text) <= 0)
            //{
            //    ShowMessage("Digite um fator de cubagem válido", MessageType.Info);
            //    txtFatorCubagem.Focus();
            //    return false;
            //}
            if (txtQtEmb.Text == "")
            {
                ShowMessage("Digite a quantidade da embalagem", MessageType.Info);
                txtQtEmb.Focus();
                return false;
            }
            if (txtQtEmb.Text == "1")
            {
                if (txtEmbalagem.Text == "")
                {
                    txtEmbalagem.Text = ddlUnidade.SelectedItem.Text;
                    return true;
                }
            }
            if (txtQtEmb.Text == "0")
            {
                ShowMessage("Digite a quantidade da embalagem", MessageType.Info);
                txtQtEmb.Focus();
                return false;
            }
            v.CampoValido("Quantidade na Embalagem", txtQtEmb.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtQtEmb.Focus();
                }
                return false;
            }
            if (txtAssociado.Text == "")
            {
                txtAssociado.Text = txtCodProduto.Text;
            }
            if (txtAssociado.Text == "0")
            {
                txtAssociado.Text = txtCodProduto.Text;
            }
            if (txtBarras.Text == "")
            {
                txtBarras.Text = txtCodProduto.Text;
            }
            if (txtBarras.Text == "0")
            {
                txtBarras.Text = txtCodProduto.Text;
            }
            if(txtNCM.Text.Length != 10 && txtNCM.Text.Length > 0)
            {
                ShowMessage("NCM Inválido, deve ter formatação '9999.99.99'", MessageType.Info);
                return false;
            }
            return true;
        }
        protected Boolean ValidaCategoria()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código da Categoria", txtCodCategoria.Text, true, false, false, false, "NVARCHAR", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodCategoria.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodCategoria.Focus();
                }

                return false;
            }
            return blnCampoValido;
        }
        protected void LimpaCampos()
        {
            chkInventario.Enabled = false;
            txtCodProduto.Text = "Novo";
            txtDscProduto.Text = "";
            txt_Venda.Text = "0,00";
            txtLucro.Text = "0,00";
            txtCompra.Text = "0,00";
            txtQtEmb.Text = "1";
            txtEmbalagem.Text = "";
            txtBarras.Text = "";
            txtAssociado.Text = "";
            txtDcrCategoria.Text = "";

            chkCrtLote.Checked = false;

            UnidadeDAL d = new UnidadeDAL();
            ddlUnidade.DataSource = d.ListarUnidades("", "", "", "");
            ddlUnidade.DataTextField = "DescricaoUnidade";
            ddlUnidade.DataValueField = "CodigoUnidade";
            ddlUnidade.DataBind();

            GpoTribProdutoDAL gtpd = new GpoTribProdutoDAL();
            ddlGpoTribProduto.DataSource = gtpd.ObterGpoTribProdutos("", "", "", "");
            ddlGpoTribProduto.DataTextField = "DescricaoGpoTribProduto";
            ddlGpoTribProduto.DataValueField = "CodigoGpoTribProduto";
            ddlGpoTribProduto.DataBind();
            ddlGpoTribProduto.Items.Insert(0, "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PRODUTOS .....");

            Habil_TipoDAL RnSituacao = new Habil_TipoDAL();
            ddlGpoPessoa.DataSource = RnSituacao.TipoProduto();
            ddlGpoPessoa.DataTextField = "DescricaoTipo";
            ddlGpoPessoa.DataValueField = "CodigoTipo";
            ddlGpoPessoa.DataBind();

            Habil_TipoDAL Ht = new Habil_TipoDAL();
            ddlSituacao.DataSource = Ht.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            MarcaDAL marcas = new MarcaDAL();
            ddlMarca.DataSource = marcas.ListarMarcas("", "", "", "");
            ddlMarca.DataTextField = "DescricaoMarca";
            ddlMarca.DataValueField = "CodigoMarca";
            ddlMarca.DataBind();
            ddlMarca.Items.Insert(0, "..... SELECIONE A MARCA .....");

            FabricanteDAL fabricantes = new FabricanteDAL();
            ddlFabricante.DataSource = fabricantes.ListarFabricantes("", "", "", "");
            ddlFabricante.DataTextField = "DescricaoFabricante";
            ddlFabricante.DataValueField = "CodigoFabricante";
            ddlFabricante.DataBind();
            ddlFabricante.Items.Insert(0, "..... SELECIONE O FABRICANTE .....");

            CESTDAL cests = new CESTDAL();
            ddlCodigoCEST.DataSource = cests.ListarCESTs("", "", "", "");
            ddlCodigoCEST.DataTextField = "CodigoCEST";
            ddlCodigoCEST.DataValueField = "CodigoIndice";
            ddlCodigoCEST.DataBind();
            ddlCodigoCEST.Items.Insert(0, "* Nenhum selecionado");

            PISDAL pis = new PISDAL();
            ddlPIS.DataSource = pis.ListarPIS("", "", "", "");
            ddlPIS.DataTextField = "DescricaoPIS";
            ddlPIS.DataValueField = "CodigoIndice";
            ddlPIS.DataBind();
            ddlPIS.Items.Insert(0, "* Nenhum selecionado");

            COFINSDAL cofins = new COFINSDAL();
            ddlCOFINS.DataSource = cofins.ListarCOFINS("", "", "", "");
            ddlCOFINS.DataTextField = "DescricaoCOFINS";
            ddlCOFINS.DataValueField = "CodigoIndice";
            ddlCOFINS.DataBind();
            ddlCOFINS.Items.Insert(0, "* Nenhum selecionado");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ParSistemaDAL RnParSis = new ParSistemaDAL();
            MaskCategoria = @RnParSis.FormataCategoria(1);

            txtCodProduto.Enabled = false;
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");


            if (Session["TabFocada"] != null)
            {
                PanelSelect = Session["TabFocada"].ToString();
                Session["TabFocada"] = null;
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Error);
                Session["MensagemTela"] = null;
                return;
            }

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomProduto2"] != null)
            {
                if (Session["ZoomProduto2"].ToString() == "RELACIONAL")
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
                cmdSair.Visible = true;
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();

            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                           Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                           "ConProduto.aspx");

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

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomProduto2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomProduto"] != null)
                {
                    PanelSelect = "home";
                    Session["TabFocada"] = "home";

                    string s = Session["ZoomProduto"].ToString();
                    Session["ZoomProduto"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodProduto.Text == "")
                            {
                                LimpaCampos();

                                txtCodProduto.Text = word;
                                txtCodProduto.Enabled = false;

                                ProdutoDAL r = new ProdutoDAL();
                                Produto p = new Produto();
                                ProdutoDAL rn = new ProdutoDAL();
                                Produto pd = new Produto();

                                p = r.PesquisarProduto(Convert.ToInt64(txtCodProduto.Text));
                                if (p.CodigoPrdAssociado != 0)
                                {
                                    pd = rn.PesquisarProduto(Convert.ToInt64(p.CodigoPrdAssociado));
                                    Session["EmbalagemText"] = pd.DsEmbalagem.ToString();
                                }
                                txtEmbalagem.Text = p.DsEmbalagem.ToString();
                               
                                txtEmbalagem.Enabled = false;
                                txtQtEmb.Text = p.QtEmbalagem.ToString();
                                txtAssociado.Text = p.CodigoPrdAssociado.ToString();
                                txtBarras.Text = p.CodigoBarras.ToString();
                                txtDscProduto.Text = p.DescricaoProduto;
                                txtDataCadastro.Text = p.DataCadastro.ToString("d").PadLeft(10);
                                txtDataAtualizacao.Text = p.DataAtualizacao.ToString("d").PadLeft(10);
                                ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                ddlGpoPessoa.SelectedValue = p.CodigoTipoProduto.ToString();
                                ddlUnidade.SelectedValue = p.CodigoUnidade.ToString();
                                txtCompra.Text = Convert.ToDouble(p.ValorCompra).ToString("###,##0.00");
                                txtLucro.Text = Convert.ToDouble(p.PercentualLucro).ToString("###,##0.00");
                                txt_Venda.Text = Convert.ToDouble(p.ValorVenda).ToString("###,##0.00");
                                ddlGpoTribProduto.SelectedValue = p.CodigoGpoTribProduto.ToString();
                                ddlCodigoCEST.SelectedValue = p.CodigoIndexCEST.ToString();
                                ddlFabricante.SelectedValue = p.CodigoFabricante.ToString();
                                ddlMarca.SelectedValue = p.CodigoMarca.ToString();
                                chkInventario.Checked = p.ProdutoInventario;
                                chkCrtLote.Checked = p.ControlaLote;
                                txtVolume.Text = p.ValorVolume.ToString();
                                txtPeso.Text = p.ValorPeso.ToString();
                                txtFatorCubagem.Text = p.ValorFatorCubagem.ToString();
                                ddlCOFINS.SelectedValue = p.CodigoCOFINS.ToString();
                                ddlPIS.SelectedValue = p.CodigoPIS.ToString();
                                txtNCM.Text = p.CodigoNCM.ToString();
                                txtEX.Text = p.CodigoEX.ToString();
                                txtHyperLinkProduto.Text = p.LinkProduto;

                                if (p.CodigoSisAnterior != "" && p.CodigoSisAnterior != null)
                                    txtCodSisAnterior.Text = p.CodigoSisAnterior.ToString();

                                if (txtAssociado.Text == txtCodProduto.Text)
                                {
                                    txtEmbalagem.Enabled = true;
                                }


                                if (p.FotoPrincipal != null)
                                    SalvarFotoNaPasta(p.FotoPrincipal, p.CodigoProduto, 0);
                                if (p.Foto1 != null)
                                    SalvarFotoNaPasta(p.Foto1, p.CodigoProduto, 1);
                                if (p.Foto2 != null)
                                    SalvarFotoNaPasta(p.Foto2, p.CodigoProduto, 2);
                                if (p.Foto3 != null)
                                    SalvarFotoNaPasta(p.Foto3, p.CodigoProduto, 3);
                                if (p.Foto4 != null)
                                    SalvarFotoNaPasta(p.Foto4, p.CodigoProduto, 4);
                                if (p.Foto5 != null)
                                    SalvarFotoNaPasta(p.Foto5, p.CodigoProduto, 5);

                                CategoriaDAL c = new CategoriaDAL();
                                if (p.CodigoCategoria != "" && p.CodigoCategoria != "0")
                                {
                                    txtCodCategoria.Text = c.PesquisarCategoriaIndice(Convert.ToInt32(p.CodigoCategoria)).CodigoCategoria;
                                    txtCodCategoria_TextChanged(sender, e);
                                    ddlCodigoCEST_SelectedIndexChanged(sender, e);
                                    PanelSelect = "home";
                                }
                            }
                    }
                }
                else
                {
                    LimpaCampos();
                    btnExcluir.Visible = false;
                }

                if ((Session["IncProdutoUnidade"] != null) || (Session["IncProdutoCategoria"] != null))
                {

                    if (Session["IncProdutoUnidade"] != null)
                        listCadProduto = (List<Produto>)Session["IncProdutoUnidade"];

                    if (Session["IncProdutoCategoria"] != null)
                        listCadProduto = (List<Produto>)Session["IncProdutoCategoria"];

                    foreach (Produto p in listCadProduto)
                    {

                        if (p.CodigoProduto == 0)
                            txtCodProduto.Text = "Novo";
                        else
                            txtCodProduto.Text = p.CodigoProduto.ToString();
                        if (p.CodigoSisAnterior != "")
                            txtCodSisAnterior.Text = p.CodigoSisAnterior.ToString();

                        txtDscProduto.Text = p.DescricaoProduto;
                        txtDataCadastro.Text = p.DataCadastro.ToString("d").PadLeft(10);
                        txtDataAtualizacao.Text = p.DataAtualizacao.ToString("d").PadLeft(10);
                        ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                        ddlGpoPessoa.SelectedValue = p.CodigoTipoProduto.ToString();
                        ddlUnidade.SelectedValue = p.CodigoUnidade.ToString();
                        txtCompra.Text = Convert.ToDouble(p.ValorCompra).ToString("###,##0.00");
                        txtLucro.Text = Convert.ToDouble(p.PercentualLucro).ToString("###,##0.00");
                        txt_Venda.Text = Convert.ToDouble(p.ValorVenda).ToString("###,##0.00");
                        ddlGpoTribProduto.SelectedValue = Convert.ToString(p.CodigoGpoTribProduto);
                        ddlCodigoCEST.SelectedValue = p.CodigoIndexCEST.ToString();
                        ddlFabricante.SelectedValue = p.CodigoFabricante.ToString();
                        ddlMarca.SelectedValue = p.CodigoMarca.ToString();
                        txtCodCategoria.Text = p.CodigoCategoria;
                        txtCodCategoria_TextChanged(sender, e);
                        ddlCodigoCEST_SelectedIndexChanged(sender, e);
                        txtVolume.Text = p.ValorVolume.ToString();
                        txtPeso.Text = p.ValorPeso.ToString();
                        txtFatorCubagem.Text = p.ValorFatorCubagem.ToString();
                        txtEmbalagem.Text = p.DsEmbalagem.ToString();
                        txtQtEmb.Text = p.QtEmbalagem.ToString();
                        txtAssociado.Text = p.CodigoPrdAssociado.ToString();
                        txtBarras.Text = p.CodigoBarras.ToString();
                        ddlCOFINS.SelectedValue = p.CodigoCOFINS.ToString();
                        ddlPIS.SelectedValue = p.CodigoPIS.ToString();
                        txtNCM.Text = p.CodigoNCM.ToString();
                        txtEX.Text = p.CodigoEX.ToString();
                        txtHyperLinkProduto.Text = p.LinkProduto;

                    }
                    PanelSelect = "home";
                    listCadProduto = null;
                    Session["IncProdutoUnidade"] = null;
                    Session["IncProdutoCategoria"] = null;

                }
            }
            if (txtCodProduto.Text == "")
                btnVoltar_Click(sender, e);

            if (Session["ZoomCategoria"] != null)
            {
                txtCodCategoria.Text = Session["ZoomCategoria"].ToString().Split('³')[0];
                txtCodCategoria_TextChanged(sender, e);
                Session["ZoomCategoria"] = null;
            }

            if (!IsPostBack)
            {
                if (Session["CaminhoProdutoFoto0"] != null)
                {
                    imgFotoPrincipal.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoProdutoFoto0"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
                    btnRemoverFotoPrincipal.Visible = true;
                }
                if (Session["CaminhoProdutoFoto1"] != null)
                {
                    imgFoto1.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoProdutoFoto1"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
                    btnRemoverFoto1.Visible = true;
                }
                if (Session["CaminhoProdutoFoto2"] != null)
                {
                    imgFoto2.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoProdutoFoto2"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
                    btnRemoverFoto2.Visible = true;
                }
                if (Session["CaminhoProdutoFoto3"] != null)
                {
                    imgFoto3.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoProdutoFoto3"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
                    btnRemoverFoto3.Visible = true;
                }
                if (Session["CaminhoProdutoFoto4"] != null)
                {
                    imgFoto4.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoProdutoFoto4"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
                    btnRemoverFoto4.Visible = true;
                }
                if (Session["CaminhoProdutoFoto5"] != null)
                {
                    imgFoto5.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoProdutoFoto5"].ToString() + "?cache" + DateTime.Now.Ticks.ToString();
                    btnRemoverFoto5.Visible = true;
                }
            }
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodProduto.Text.Trim() != "")
                {
                    ProdutoDAL d = new ProdutoDAL();
                    d.Excluir(Convert.ToInt32(txtCodProduto.Text));
                    Session["MensagemTela"] = "Produto Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Produto não identificado.&emsp;&emsp;&emsp;";

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
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["EmbalagemText"] = null;

            Session["CaminhoProdutoFoto0"] = null;
            Session["CaminhoProdutoFoto1"] = null;
            Session["CaminhoProdutoFoto2"] = null;
            Session["CaminhoProdutoFoto3"] = null;
            Session["CaminhoProdutoFoto4"] = null;
            Session["CaminhoProdutoFoto5"] = null;

            Session["ZoomProduto"] = null;
            if (Session["ZoomProduto2"] != null)
            {
                Session["ZoomProduto2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadProduto.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Response.Redirect("~/Pages/Produtos/ConProduto.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            if (ValidaCategoria() == false)
                return;

            ProdutoDAL d = new ProdutoDAL();
            Produto p = new Produto();

            if (Convert.ToInt32(ddlGpoPessoa.SelectedValue) == 0)
            {
                ShowMessage("Escolha um Tipo de produto", MessageType.Info);
                return;
            }

            p.CodigoNCM = txtNCM.Text;
            p.CodigoEX = txtEX.Text;
            p.DescricaoProduto = txtDscProduto.Text.ToUpper();
            p.DataCadastro = DateTime.Today;
            p.DataAtualizacao = DateTime.Today;
            p.CodigoUnidade = Convert.ToInt32(ddlUnidade.SelectedValue);
            p.CodigoCategoria = txtCodCategoria.Text;
            p.ValorCompra = Convert.ToDouble(txtCompra.Text);
            p.PercentualLucro = Convert.ToDouble(txtLucro.Text);
            p.ValorVenda = Convert.ToDouble(txt_Venda.Text);
            if (txtCodSisAnterior.Text != "")
                p.CodigoSisAnterior = txtCodSisAnterior.Text;
            p.CodigoTipoProduto = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            p.CodigoFabricante = Convert.ToInt32(ddlFabricante.SelectedValue);
            p.CodigoMarca = Convert.ToInt32(ddlMarca.SelectedValue);
            p.CodigoGpoTribProduto = Convert.ToInt32(ddlGpoTribProduto.SelectedValue);
            p.ValorVolume = Convert.ToDecimal(txtVolume.Text);
            p.ValorPeso = Convert.ToDecimal(txtPeso.Text);
            p.ValorFatorCubagem = Convert.ToDecimal(txtFatorCubagem.Text);
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.ProdutoInventario = chkInventario.Checked;
            p.ControlaLote = chkCrtLote.Checked;
            p.DsEmbalagem = txtEmbalagem.Text;
            p.QtEmbalagem = Convert.ToInt16(txtQtEmb.Text);
            p.LinkProduto = txtHyperLinkProduto.Text;
            if(Session["CaminhoProdutoFoto0"] != null)
                p.FotoPrincipal = BuscarFotoDaPasta(Session["CaminhoProdutoFoto0"].ToString());
            if (Session["CaminhoProdutoFoto1"] != null)
                p.Foto1 = BuscarFotoDaPasta(Session["CaminhoProdutoFoto1"].ToString());
            if (Session["CaminhoProdutoFoto2"] != null)
                p.Foto2 = BuscarFotoDaPasta(Session["CaminhoProdutoFoto2"].ToString());
            if (Session["CaminhoProdutoFoto3"] != null)
                p.Foto3 = BuscarFotoDaPasta(Session["CaminhoProdutoFoto3"].ToString());
            if (Session["CaminhoProdutoFoto4"] != null)
                p.Foto4 = BuscarFotoDaPasta(Session["CaminhoProdutoFoto4"].ToString());
            if (Session["CaminhoProdutoFoto5"] != null)
                p.Foto5 = BuscarFotoDaPasta(Session["CaminhoProdutoFoto5"].ToString());


            if (txtBarras.Text == "")
                p.CodigoBarras = "";
            else
                p.CodigoBarras = txtBarras.Text;

            if (txtCodProduto.Text == txtAssociado.Text)
            {
                d.ListarProdutos("", "", "", "");
            }

            if (ddlCodigoCEST.SelectedValue != "* Nenhum selecionado")
                p.CodigoIndexCEST = Convert.ToInt32(ddlCodigoCEST.SelectedValue);
            if (ddlCOFINS.SelectedValue != "* Nenhum selecionado")
                p.CodigoCOFINS = Convert.ToInt32(ddlCOFINS.SelectedValue);
            if (ddlPIS.SelectedValue != "* Nenhum selecionado")
                p.CodigoPIS = Convert.ToInt32(ddlPIS.SelectedValue);

            if (txtCodProduto.Text == "Novo")
            {
                p.CodigoPrdAssociado = 0;
                d.Inserir(p);
                Session["MensagemTela"] = "Produto incluso com Sucesso!!!";
            }
            else
            {
                p.CodigoPrdAssociado = Convert.ToInt32(txtAssociado.Text);
                p.CodigoProduto = Convert.ToInt64(txtCodProduto.Text);
                d.Atualizar(p);

                Session["MensagemTela"] = "Produto alterado com Sucesso!!!";
            }
            List<Produto> lstprod = new List<Produto>();

            if (txtCodProduto.Text == txtAssociado.Text)
            {
                if (Session["EmbalagemText"] != null)
                {
                    if (txtEmbalagem.Text != Session["EmbalagemText"].ToString())
                    {
                        lstprod = d.ListarEmbalagens(Convert.ToInt32(txtAssociado.Text));

                        foreach (Produto item in lstprod)
                        {
                            string emb = "";
                            if (item.CodigoProduto != Convert.ToInt32(txtCodProduto.Text))
                            {
                                if (item.QtEmbalagem != 0)
                                {
                                    emb = item.QtEmbalagem.ToString() + " X " + txtEmbalagem.Text;
                                    d.UpdateEmbalagem(item.CodigoProduto, emb);
                                }
                            }
                        }
                    }
                }
            }


            Session["EmbalagemText"] = null;
            btnVoltar_Click(sender, e);

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void ddlUnidade_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void btnAddUnidade_Click(object sender, EventArgs e)
        {
            try
            {
                long CodProduto = 0;

                if (txtCodProduto.Text != "Novo")
                    CodProduto = Convert.ToInt64(txtCodProduto.Text);

                int CodigoMarca = 0;
                int CodigoFabricante = 0;
                int CodigoIndexCEST = 0;
                int CodigoGpoTripProd = 0;
                int CodigoPIS = 0;
                int CodigoCOFINS = 0;

                if (ddlMarca.SelectedValue != "..... SELECIONE A MARCA .....")
                    CodigoMarca = Convert.ToInt32(ddlMarca.SelectedValue);
                if (ddlFabricante.SelectedValue != "..... SELECIONE O FABRICANTE .....")
                    CodigoFabricante = Convert.ToInt32(ddlFabricante.SelectedValue);
                if (ddlGpoTribProduto.SelectedValue != "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PRODUTOS .....")
                    CodigoGpoTripProd = Convert.ToInt32(ddlGpoTribProduto.SelectedValue);
                if (ddlCodigoCEST.SelectedValue != "* Nenhum selecionado")
                    CodigoIndexCEST = Convert.ToInt32(ddlCodigoCEST.SelectedValue);
                if (ddlCOFINS.SelectedValue != "* Nenhum selecionado")
                    CodigoCOFINS = Convert.ToInt32(ddlCOFINS.SelectedValue);
                if (ddlPIS.SelectedValue != "* Nenhum selecionado")
                    CodigoPIS = Convert.ToInt32(ddlPIS.SelectedValue);

                Produto x1 = new Produto(CodProduto,
                    txtDscProduto.Text,
                    txtCodCategoria.Text,
                    DateTime.Today,
                    DateTime.Today,
                    Convert.ToDouble(txtCompra.Text),
                    Convert.ToDouble(txtLucro.Text),
                    Convert.ToDouble(txt_Venda.Text),
                    Convert.ToInt32(ddlUnidade.SelectedValue),
                    Convert.ToInt32(ddlSituacao.SelectedValue),
                    CodigoGpoTripProd,
                    Convert.ToInt32(ddlGpoPessoa.SelectedValue),
                    txtCodSisAnterior.Text, "",
                    CodigoMarca,
                    CodigoFabricante,
                    Convert.ToBoolean(chkInventario.Checked),
                    Convert.ToBoolean(chkCrtLote.Checked),
                    CodigoIndexCEST,
                    Convert.ToDecimal(txtVolume.Text),
                    Convert.ToDecimal(txtPeso.Text),
                    Convert.ToDecimal(txtFatorCubagem.Text),
                    Convert.ToString(txtBarras.Text),
                    Convert.ToInt32(txtAssociado.Text),
                    Convert.ToInt16(txtQtEmb.Text),
                    Convert.ToString(txtEmbalagem.Text), 
                    CodigoPIS,
                    CodigoCOFINS,
                    txtNCM.Text,
                    txtEX.Text
                );
                


                listCadProduto = new List<Produto>();
                listCadProduto.Add(x1);
                Session["IncProdutoUnidade"] = listCadProduto;
                Session["ZoomUnidade2"] = "RELACIONAL";
                Response.Redirect("~/Pages/Produtos/CadUnidade.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void txtCompra_TextChanged(object sender, EventArgs e)
        {
            if (txtCompra.Text.Equals(""))
                txtCompra.Text = "0,00";
            else
            {
                txtCompra.Text = Convert.ToDouble(txtCompra.Text).ToString("###,##0.00");
                txtLucro.Focus();
            }
        }
        protected void txtLucro_TextChanged(object sender, EventArgs e)
        {
            if (txtLucro.Text.Equals(""))
                txtLucro.Text = "0,00";
            else
            {
                txtLucro.Text = Convert.ToDouble(txtLucro.Text).ToString("###,##0.00");
                txt_Venda.Focus();
            }
        }
        protected void txt_Venda_TextChanged(object sender, EventArgs e)
        {
            if (txt_Venda.Text.Equals(""))
                txt_Venda.Text = "0,00";
            else
            {
                txt_Venda.Text = Convert.ToDouble(txt_Venda.Text).ToString("###,##0.00");
                ddlMarca.Focus();
            }
        }
        protected void txtCodCategoria_TextChanged(object sender, EventArgs e)
        {
            CategoriaDAL r = new CategoriaDAL();
            Categoria p = new Categoria();
            if (ValidaCategoria())
            {
                p = r.PesquisarCategoria(txtCodCategoria.Text);
                txtDcrCategoria.Text = "";
                if (p != null)
                    txtDcrCategoria.Text = p.DescricaoCategoria;
                else
                    ShowMessage("Categoria não Cadastrada", MessageType.Info);
            }
            else
                txtDcrCategoria.Text = "";

        }
        protected void btnConCategoria_Click(object sender, EventArgs e)
        {
            CompactaDados(sender, e);
            Response.Redirect("~/Pages/Produtos/ConCategoria.aspx?cad=1");
        }
        protected void CompactaDados(object sender, EventArgs e)
        {
            Produto p = new Produto();

            long CodProduto = 0;

            if (txtCodProduto.Text != "Novo")
                CodProduto = Convert.ToInt64(txtCodProduto.Text);

            int CodigoMarca = 0;
            int CodigoFabricante = 0;
            int CodigoIndexCEST = 0;
            int CodigoGpoTripProd = 0;

            if (ddlMarca.SelectedValue != "..... SELECIONE A MARCA .....")
                CodigoMarca = Convert.ToInt32(ddlMarca.SelectedValue);
            if (ddlFabricante.SelectedValue != "..... SELECIONE O FABRICANTE .....")
                CodigoFabricante = Convert.ToInt32(ddlFabricante.SelectedValue);
            if (ddlGpoTribProduto.SelectedValue != "..... SELECIONE UM GRUPO DE TRIBUTAÇÃO DE PRODUTOS .....")
                CodigoGpoTripProd = Convert.ToInt32(ddlGpoTribProduto.SelectedValue);
            if (ddlCodigoCEST.SelectedValue != "* Nenhum selecionado")
                CodigoIndexCEST = Convert.ToInt32(ddlCodigoCEST.SelectedValue);
            if (ddlCOFINS.SelectedValue != "* Nenhum selecionado")
                p.CodigoCOFINS = Convert.ToInt32(ddlCOFINS.SelectedValue);
            if (ddlPIS.SelectedValue != "* Nenhum selecionado")
                p.CodigoPIS = Convert.ToInt32(ddlPIS.SelectedValue);

            p.CodigoNCM = txtNCM.Text;
            p.CodigoEX = txtEX.Text;
            p.CodigoProduto = CodProduto;
            p.DescricaoProduto = txtDscProduto.Text;
            p.DataCadastro = DateTime.Today;
            p.DataAtualizacao = DateTime.Today;
            p.ValorCompra = Convert.ToDouble(txtCompra.Text);
            p.PercentualLucro = Convert.ToDouble(txtLucro.Text);
            p.ValorVenda = Convert.ToDouble(txt_Venda.Text);
            p.CodigoUnidade = Convert.ToInt32(ddlUnidade.SelectedValue);
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.CodigoGpoTribProduto = CodigoGpoTripProd;
            p.CodigoTipoProduto = Convert.ToInt32(ddlGpoPessoa.SelectedValue);
            p.CodigoSisAnterior = txtCodSisAnterior.Text;
            p.CodigoCEST = "";
            p.CodigoMarca = CodigoMarca;
            p.CodigoFabricante = CodigoFabricante;
            p.ValorFatorCubagem = Convert.ToDecimal(txtFatorCubagem.Text);
            p.ValorVolume = Convert.ToDecimal(txtVolume.Text);
            p.ValorPeso = Convert.ToDecimal(txtPeso.Text);
            p.CodigoBarras = Convert.ToString(txtBarras.Text);
            p.LinkProduto = txtHyperLinkProduto.Text;
            if(txtAssociado.Text == "")
                p.CodigoPrdAssociado = 0;
            else
                p.CodigoPrdAssociado = Convert.ToInt32(txtAssociado.Text);

            p.QtEmbalagem = Convert.ToInt16(txtQtEmb.Text);
            p.DsEmbalagem = Convert.ToString(txtEmbalagem.Text);
            p.ControlaLote = Convert.ToBoolean(chkCrtLote.Checked);
            p.CodigoIndexCEST = CodigoIndexCEST;
            listCadProduto.Add(p);
            
            Session["IncProdutoCategoria"] = listCadProduto;
            Session["ZoomCategoria2"] = "RELACIONAL";
        }
        protected void PreencheDados(object sender, EventArgs e)
        {

        }
        protected void ddlGpoPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlCodigoCEST_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCodigoCEST.SelectedValue == "* Nenhum selecionado")
            {
                txtDescricaoCEST.Text = "";
            }
            else
            {
                CEST cest = new CEST();
                CESTDAL cestDAL = new CESTDAL();
                cest = cestDAL.PesquisarCESTIndice(Convert.ToInt32(ddlCodigoCEST.SelectedValue));
                if (cest != null)
                    txtDescricaoCEST.Text = cest.DescricaoCEST;
                else
                    txtDescricaoCEST.Text = "";
            }
            PanelSelect = "profile";
        }
        protected void txtVolume_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtVolume.Text.Equals(""))
            {
                txtVolume.Text = "0,000";
            }
            else
            {
                v.CampoValido("Volume", txtVolume.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtVolume.Text = Convert.ToDecimal(txtVolume.Text).ToString("###,###0.000");
                }
                else
                    txtVolume.Text = "0,000";

            }
        }
        protected void txtPeso_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtPeso.Text.Equals(""))
            {
                txtPeso.Text = "0,000";
            }
            else
            {
                v.CampoValido("Volume", txtPeso.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtPeso.Text = Convert.ToDecimal(txtPeso.Text).ToString("###,###0.000");
                }
                else
                    txtPeso.Text = "0,000";

            }
        }
        protected void txtFatorCubagem_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtFatorCubagem.Text.Equals(""))
            {
                txtFatorCubagem.Text = "0,000";
            }
            else
            {
                v.CampoValido("Volume", txtFatorCubagem.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtFatorCubagem.Text = Convert.ToDecimal(txtFatorCubagem.Text).ToString("###,###0.000");
                }
                else
                    txtFatorCubagem.Text = "0,000";

            }
        }
        protected void txtAssociado_TextChanged(object sender, EventArgs e)
        {
            //Boolean blnCampo = false;

            if (txtAssociado.Text != "")
            {
                txtEmbalagem.Enabled = false;
            }
            else
            {
                txtAssociado.Text = "";
                return;
            }
            int num;
            bool isNum = Int32.TryParse(txtAssociado.Text, out num);
            if (!isNum)
            {
                ShowMessage("Código do Produto Invalido", MessageType.Info);
                return;
            }

            txtCodProduto.Enabled = false;
            txtDscProduto.Enabled = false;
            txtEmbalagem.Enabled = false;

            ProdutoDAL r = new ProdutoDAL();
            Produto p = new Produto();
            if (txtAssociado.Text == "")
            {
                LimpaCampos();
                return;
            }
            p = r.PesquisarProduto(Convert.ToInt64(txtAssociado.Text));

            if (p.CodigoSituacao == 2)
            {
                ShowMessage("Produto Associado conta como Inativo", MessageType.Warning);
                return;
            }
            txtEmbalagem.Text = p.DsEmbalagem.ToString();
            Session["EmbalagemText"] = p.DsEmbalagem.ToString();
            txtQtEmb.Text = p.QtEmbalagem.ToString("f");
            txtAssociado.Text = p.CodigoProduto.ToString();
            txtDscProduto.Text = p.DescricaoProduto;
            ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
            ddlGpoPessoa.SelectedValue = p.CodigoTipoProduto.ToString();
            ddlUnidade.SelectedValue = p.CodigoUnidade.ToString();
            txtCompra.Text = Convert.ToDouble(p.ValorCompra).ToString("###,##0.00");
            txtLucro.Text = Convert.ToDouble(p.PercentualLucro).ToString("###,##0.00");
            txt_Venda.Text = Convert.ToDouble(p.ValorVenda).ToString("###,##0.00");
            ddlGpoTribProduto.SelectedValue = p.CodigoGpoTribProduto.ToString();
            ddlCodigoCEST.SelectedValue = p.CodigoIndexCEST.ToString();
            ddlFabricante.SelectedValue = p.CodigoFabricante.ToString();
            ddlMarca.SelectedValue = p.CodigoMarca.ToString();
            chkInventario.Checked = p.ProdutoInventario;
            chkCrtLote.Checked = p.ControlaLote;
            txtVolume.Text = p.ValorVolume.ToString();
            txtPeso.Text = p.ValorPeso.ToString();
            txtFatorCubagem.Text = p.ValorFatorCubagem.ToString();
            if (p.CodigoSisAnterior != "" && p.CodigoSisAnterior != null)
                txtCodSisAnterior.Text = p.CodigoSisAnterior.ToString();

            CategoriaDAL c = new CategoriaDAL();
            txtCodCategoria.Text = c.PesquisarCategoriaIndice(Convert.ToInt32(p.CodigoCategoria)).CodigoCategoria;
            txtCodCategoria_TextChanged(sender, e);
            ddlCodigoCEST_SelectedIndexChanged(sender, e);
            PanelSelect = "home";

            if (txtAssociado.Text == txtCodProduto.Text)
            {
                txtEmbalagem.Enabled = true;
            }

        }
        protected void txtBarras_TextChanged(object sender, EventArgs e)
        {
            ProdutoDAL RnprodutoDAL = new ProdutoDAL();
            Produto p = new Produto();

            int lngProduto = 0;

            if(txtCodProduto.Text == "")
                lngProduto = RnprodutoDAL.PesquisaCodigoBarrasInformado(txtBarras.Text, 0);
            else
                lngProduto = RnprodutoDAL.PesquisaCodigoBarrasInformado(txtBarras.Text, Convert.ToInt32(txtCodProduto.Text));

            if (lngProduto != 0)
            {
                txtBarras.Text = "";
                ShowMessage("Código de Barras já consta em outro Produto", MessageType.Info);
            }
        }
        protected void txtQtEmb_TextChanged(object sender, EventArgs e)
        {
            //Boolean blnCampo = false;

            if (txtAssociado.Text == txtCodProduto.Text)
                return;
            if (txtAssociado.Text != "")
            {
                if (Session["EmbalagemText"] != null)
                    txtEmbalagem.Text = " " + txtQtEmb.Text + " x " + Session["EmbalagemText"].ToString();
                else
                {
                    if (txtEmbalagem.Text != "")
                        txtEmbalagem.Text = " " + txtQtEmb.Text + " x " + txtEmbalagem.Text;
                    else
                        txtEmbalagem.Text = txtQtEmb.Text;
                }
            }
            else
            {
                txtQtEmb.Text = "1";
            }
        }
        protected void BtnAddProduto_Click(object sender, EventArgs e)
        { 
            RegraFisIPIDAL RnIPI = new RegraFisIPIDAL();
            RegFisIPI p = new RegFisIPI();
            List<RegFisIPI> Lista = new List<RegFisIPI>();
            List<DBTabelaCampos> ListaFiltros = new List<DBTabelaCampos>();
            DBTabelaDAL dbTDAL = new DBTabelaDAL();

            txtVigencia.Text = Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("dd/MM/yyyy"));
            Lista = RnIPI.ListarRegrasIPI(ListaFiltros);
            GridNCM.DataSource = Lista;
            GridNCM.DataBind();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "NCMShow();", true);
        }
        protected void txtNCM_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtNCM.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo NCM", txtNCM.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtNCM.Text = "";
                    return;
                }
            }

            RegraFisIPIDAL RnIPI = new RegraFisIPIDAL();
            RegFisIPI p = new RegFisIPI();

            p = RnIPI.PesquisarIPIPorCodNcm(txtNCM.Text);

            if (p.CodigoRegraFiscalIPI > 0)
            {
                if (p.CodigoSituacao == 1)
                {
                    txtNCM.Text = p.CodigoNCM;
                }
                else
                {
                    txtNCM.Text = "";
                    ShowMessage("Regra Fiscal de IPI está inativo!!!", MessageType.Info);
                }
            }
            else
            {
                txtNCM.Text = "";
                ShowMessage("Regra Fiscal de IPI não cadastrada!!!", MessageType.Info);
            }
            PanelSelect = "profile";
        }
        protected void GridNCM_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNCM.Text = HttpUtility.HtmlDecode(GridNCM.SelectedRow.Cells[2].Text);
            txtNCM_TextChanged(sender, e);
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "NCMhide();", true);
        }
        protected void GridNCM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void txtPesquisaNCM_TextChanged(object sender, EventArgs e)
        {
            RegraFisIPIDAL RnIPI = new RegraFisIPIDAL();
            RegFisIPI p = new RegFisIPI();
            List<RegFisIPI> Lista = new List<RegFisIPI>();

            if (txtPesquisaNCM.Text.Length >= 8 && txtVigencia.Text != "")
            {
                //.ToString("yyyy/MM/dd")
                Lista = RnIPI.ListarIPIPorCodNcm(txtPesquisaNCM.Text, Convert.ToDateTime(txtVigencia.Text));

                GridNCM.DataSource = Lista;
                GridNCM.DataBind();

                txtPesquisaNCM.Text = "";
                labelmens.Text = "";
                if (GridNCM.Rows.Count == 0)
                    labelmens.Text = "Não existem Produto(s) mediante a pesquisa informada!";
            }
            else
            {
                if (txtPesquisaNCM.Text == "")
                {
                    labelmens.Text = "NCM Incompleto!";
                }
                if (txtVigencia.Text == "")
                {
                    labelmens.Text = "Data de Vigência deve ser informada";
                }
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "NCMShow();", true);

        }
        protected void BtnAssociado_Click(object sender, EventArgs e)
        {
            ProdutoDAL RnProd = new ProdutoDAL();

            GridAssociados.DataSource = RnProd.ListarProdutosParaComposicao("");
            GridAssociados.DataBind();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "AssociadoShow();", true);
        }
        protected void GridAssociados_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAssociado.Text = HttpUtility.HtmlDecode(GridAssociados.SelectedRow.Cells[0].Text);
            txtAssociado_TextChanged(sender, e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "AssociadoHide();", true);
        }
        protected void GridAssociados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void txtPesquisaDescricao_TextChanged(object sender, EventArgs e)
        { 
            string texto = txtPesquisaDescricao.Text;
            if (texto.Length > 2)
            {
                ProdutoDAL RnProd = new ProdutoDAL();

                GridAssociados.DataSource = RnProd.ListarProdutosParaComposicao(txtPesquisaDescricao.Text);
                GridAssociados.DataBind();
                txtPesquisaDescricao.Text = "";
                lblMensagem.Text = "";
                if (GridAssociados.Rows.Count == 0)
                    lblMensagem.Text = "Não existem Produto(s) mediante a pesquisa informada!";
            }
            else
            {
                lblMensagem.Text = "Pesquisa Permitida acima de 3 caracteres ";
                txtPesquisaDescricao.Text = "";
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "AssociadoShow();", true);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Session["TabFocada"] = "galeria";
            PanelSelect = "galeria";

            if (base_img.Text.Length < 9)
                return;
            try
            {
                // Convert byte[] to Image
                AnexoDocumentoDAL ad = new AnexoDocumentoDAL();
                string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Scripts\CapturaWebcamASPNET\uploads\";

                string CodigoFoto = base_img.Text.Split('³')[0];
                byte[] XMLBinary = Convert.FromBase64String(base_img.Text.Replace("data:image/jpeg;base64,", "").Split('³')[1]);


                string GUIDXML = ad.GerarGUID(".jpg");

                if (System.IO.File.Exists(CaminhoArquivoLog + GUIDXML))
                    System.IO.File.Delete(CaminhoArquivoLog + GUIDXML);

                FileStream file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Create);

                BinaryWriter bw = new BinaryWriter(file);
                bw.Write(XMLBinary);
                bw.Close();

                file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Open);
                BinaryReader br = new BinaryReader(file);
                file.Close();

                Session["CaminhoProdutoFoto" + CodigoFoto] = GUIDXML;
                if (CodigoFoto == "0")
                {
                    imgFotoPrincipal.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + GUIDXML + "?cache" + DateTime.Now.Ticks.ToString(); 
                    btnRemoverFotoPrincipal.Visible = true;
                }
                else if (CodigoFoto == "1")
                {
                    imgFoto1.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + GUIDXML + "?cache" + DateTime.Now.Ticks.ToString(); 
                    btnRemoverFoto1.Visible = true;
                }
                else if (CodigoFoto == "2")
                {
                    imgFoto2.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + GUIDXML + "?cache" + DateTime.Now.Ticks.ToString(); 
                    btnRemoverFoto2.Visible = true;
                }
                else if (CodigoFoto == "3")
                {
                    imgFoto3.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + GUIDXML + "?cache" + DateTime.Now.Ticks.ToString(); 
                    btnRemoverFoto3.Visible = true;
                }
                else if (CodigoFoto == "4")
                {
                    imgFoto4.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + GUIDXML + "?cache" + DateTime.Now.Ticks.ToString(); 
                    btnRemoverFoto4.Visible = true;
                }
                else if (CodigoFoto == "5")
                {
                    imgFoto5.ImageUrl = @"..\..\Scripts\CapturaWebcamASPNET\uploads\" + GUIDXML + "?cache" + DateTime.Now.Ticks.ToString();
                    btnRemoverFoto5.Visible = true;
                }
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        private void SalvarFotoNaPasta(byte[] byteArrayIn, Int64 intCodigoProduto, int intCodigoFoto)
        {
            try
            {
                AnexoDocumentoDAL ad = new AnexoDocumentoDAL();

                string FileName = "ProdutoCod³" + intCodigoProduto + "Foto³" + intCodigoFoto + ".jpg";

                Session["CaminhoProdutoFoto" + intCodigoFoto] = FileName;
                string CaminhoArquivoLog = Server.MapPath(@"\Scripts\CapturaWebcamASPNET\uploads\" + FileName);

                if (System.IO.File.Exists(CaminhoArquivoLog))
                    System.IO.File.Delete(CaminhoArquivoLog);

                FileStream file = new FileStream(CaminhoArquivoLog, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(file);
                bw.Write(byteArrayIn);
                bw.Close();

                file = new FileStream(CaminhoArquivoLog, FileMode.Open);
                BinaryReader br = new BinaryReader(file);
                file.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private byte[] BuscarFotoDaPasta(string CaminhoFoto)
        {
            try
            {
                byte[] arquivoBinario = null;
                if (System.IO.File.Exists(Server.MapPath(@"\Scripts\CapturaWebcamASPNET\uploads\" + CaminhoFoto)))
                {
                    FileStream file = new FileStream(Server.MapPath(@"\Scripts\CapturaWebcamASPNET\uploads\" + CaminhoFoto), FileMode.Open);
                    BinaryReader reader = new BinaryReader(file);
                    arquivoBinario = reader.ReadBytes(Convert.ToInt32(file.Length));
                    file.Close();
                    return arquivoBinario;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void RemoverFoto(int intCodigoFoto)
        {
            try
            {
                Session["TabFocada"] = "galeria";
                PanelSelect = "galeria";
                if (Session["CaminhoProdutoFoto" + intCodigoFoto] != null)
                {
                    string CaminhoFoto = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Scripts\CapturaWebcamASPNET\uploads\" + Session["CaminhoProdutoFoto" + intCodigoFoto];

                    if (intCodigoFoto == 0)
                    {
                        imgFotoPrincipal.ImageUrl = "../../Images/sem-foto.jpg" + "?cache" + DateTime.Now.Ticks.ToString();
                        btnRemoverFotoPrincipal.Visible = false;
                    }
                    else if (intCodigoFoto == 1)
                    {
                        imgFoto1.ImageUrl = "../../Images/sem-foto.jpg" + "?cache" + DateTime.Now.Ticks.ToString();
                        btnRemoverFoto1.Visible = false;
                    }
                    else if (intCodigoFoto == 2)
                    {
                        imgFoto2.ImageUrl = "../../Images/sem-foto.jpg" + "?cache" + DateTime.Now.Ticks.ToString();
                        btnRemoverFoto2.Visible = false;
                    }
                    else if (intCodigoFoto == 3)
                    {
                        imgFoto3.ImageUrl = "../../Images/sem-foto.jpg" + "?cache" + DateTime.Now.Ticks.ToString();
                        btnRemoverFoto3.Visible = false;
                    }
                    else if (intCodigoFoto == 4)
                    {
                        imgFoto4.ImageUrl = "../../Images/sem-foto.jpg" + "?cache" + DateTime.Now.Ticks.ToString();
                        btnRemoverFoto4.Visible = false;
                    }
                    else if (intCodigoFoto == 5)
                    {
                        imgFoto5.ImageUrl = "../../Images/sem-foto.jpg" + "?cache" + DateTime.Now.Ticks.ToString();
                        btnRemoverFoto5.Visible = false;
                    }

                    if (System.IO.File.Exists(CaminhoFoto))
                        System.IO.File.Delete(CaminhoFoto);

                    Session["CaminhoProdutoFoto" + intCodigoFoto] = null;
                }
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnRemoverFoto5_Click(object sender, EventArgs e)
        {
            RemoverFoto(5);
        }

        protected void btnRemoverFoto4_Click(object sender, EventArgs e)
        {
            RemoverFoto(4);
        }

        protected void btnRemoverFoto3_Click(object sender, EventArgs e)
        {
            RemoverFoto(3);
        }

        protected void btnRemoverFoto2_Click(object sender, EventArgs e)
        {
            RemoverFoto(2);
        }

        protected void btnRemoverFoto1_Click(object sender, EventArgs e)
        {
            RemoverFoto(1);
        }

        protected void btnRemoverFotoPrincipal_Click(object sender, EventArgs e)
        {
            RemoverFoto(0);
        }
    }
}