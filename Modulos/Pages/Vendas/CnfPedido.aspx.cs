using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class CnfPedido : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        //string strMensagemR = "";
        List<Doc_Pedido_Estoque> Lst1Grid = new List<Doc_Pedido_Estoque>();
        List<Doc_Pedido_Estoque> Lst2Man = new List<Doc_Pedido_Estoque>();
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void TabSelecionada()
        {
            PanelSelect = "aba1";
            Session["TabFocadaConf"] = "aba1";
        }
        protected void LimpaCampos()
        {
            Session["PedidoJaConferido"] = null;
            btnFinalizar.Enabled = true;
            Session["LstGridCnf"] = null;
            grdGrid.DataSource = null;
            grdGrid.DataBind();
            Session["StrLbn"] = null;

            btnAbrirFechar.Text = "Abrir";
            txtCod.Enabled = false;
            txtNrPedido.Enabled = true;
            txtSituacao.Text = "";
            txtVolume.Text = "0";
            txtNrPedido.Text = "";
            txtConferenteFim.Text = "";
            txtConfereVolume.Text = "";
            txtDoca.Text = "";
            txtDocumento.Text = "";
            txtDesc.Text = "";
            txtCod.Text = "";
            txtQtd.Text = "1";
            txtCliente.Text = "";
            PanelA.Visible = false;
            pnlPrincipal.Visible = false;
            txtCod.Enabled = false;
            VoltarCaixas();
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel5.Visible = false;
            Panel6.Visible = false;
            Panel7.Visible = false;
            Panel8.Visible = false;
            Panel9.Visible = false;
            Panel10.Visible = false;
            pnlRecontar.Visible = false;

            LimpaGrids();
            LimpaLabels();
            LimpaSessions();

            if (txtNrPedido.Text == "")
                txtNrPedido.Focus();
            else
            {
                if (txtCod.Text == "")
                    txtCod.Focus();
            }
            PanelSelect = "aba1";
            Session["TabFocadaConf"] = "aba1";
        }
        protected void DesabilitaPanels()
        {
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel5.Visible = false;
            Panel6.Visible = false;
            Panel7.Visible = false;
            Panel8.Visible = false;
            Panel9.Visible = false;
            Panel10.Visible = false;
        }
        protected void LimpaGrids()
        {
            grdCaixa1.DataSource = null;
            grdCaixa1.DataBind();
            grdCaixa2.DataSource = null;
            grdCaixa2.DataBind();
            grdCaixa3.DataSource = null;
            grdCaixa3.DataBind();
            grdCaixa4.DataSource = null;
            grdCaixa4.DataBind();
            grdCaixa5.DataSource = null;
            grdCaixa5.DataBind();
            grdCaixa6.DataSource = null;
            grdCaixa6.DataBind();
            grdCaixa7.DataSource = null;
            grdCaixa7.DataBind();
            grdCaixa8.DataSource = null;
            grdCaixa8.DataBind();
            grdCaixa9.DataSource = null;
            grdCaixa9.DataBind();
            grdCaixa10.DataSource = null;
            grdCaixa10.DataBind();
        }
        protected void LimpaLabels()
        {
            Label1.Text = "";
            Label2.Text = "";
            Label3.Text = "";
            Label4.Text = "";
            Label5.Text = "";
            Label6.Text = "";
            Label7.Text = "";
            Label8.Text = "";
            Label9.Text = "";
            Label10.Text = "";
            Label11.Text = "";
            Label12.Text = "";
            Label13.Text = "";
            Label14.Text = "";
            Label15.Text = "";
            Label16.Text = "";
            Label17.Text = "";
            Label18.Text = "";
            Label19.Text = "";
            Label20.Text = "";
        }
        protected void LimpaSessions()
        {
            Session["Lst1"] = null;
            Session["Lst2"] = null;
            Session["Lst3"] = null;
            Session["Lst4"] = null;
            Session["Lst5"] = null;
            Session["Lst6"] = null;
            Session["Lst7"] = null;
            Session["Lst8"] = null;
            Session["Lst9"] = null;
            Session["Lst10"] = null;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocadaConf"] != null)
            {
                PanelSelect = Session["TabFocadaConf"].ToString();
            }
            else
            {
                PanelSelect = "aba1";
                Session["TabFocadaConf"] = "aba1";
            }
            if (Session["CodEmpresa"] != null)
            {
                txtConferente.Text = Session["UsuSis"].ToString();
                txtConferenteFim.Text = Session["UsuSis"].ToString();
            }
            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "LibDocumento.aspx");
                lista.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoConsulta)
                        {

                        }
                    }
                });
            }
            PanelSelect = "aba1";
            Session["TabFocadaConf"] = "aba1";
            if (Session["PrimeiraVez"] == null)
            {
                int intTipoMenu = 0;
                ParSistemaDAL RnPar = new ParSistemaDAL();
                intTipoMenu = RnPar.ConferePedido(intTipoMenu, Convert.ToInt32(Session["CodEmpresa"]));
                if (intTipoMenu == 0)
                    Response.Redirect("~/Pages/WelCome.aspx");
                LimpaCampos();
                Session["PrimeiraVez"] = "a";
            }
            txtCod.Focus();
            if (txtNrPedido.Text == "")
                txtNrPedido.Focus();
            else
            {
                if (txtCod.Text == "")
                    txtCod.Focus();
            }
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            LimpaCampos();
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Lst1"] = null;
            Session["Lst2"] = null;
            Session["Lst3"] = null;
            Session["Lst4"] = null;
            Session["Lst5"] = null;
            Session["Lst6"] = null;
            Session["Lst7"] = null;
            Session["Lst8"] = null;
            Session["Lst9"] = null;
            Session["Lst10"] = null;
            Session["StrLbn"] = null;
            Session["TabFocadaConf"] = null;
            Session["PrimeiraVez"] = null;
            Session["PedidoJaConferido"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void txtNrPedido_TextChanged(object sender, EventArgs e)
        {
            string strMensagemR = "";
            Boolean blnCampo = false;
            v.CampoValido("Pedido ", txtNrPedido.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
            if (!blnCampo)
            {
                txtNrPedido.Text = "";
                ShowMessage("Digite um Pedido Válido", MessageType.Info);
                return;
            }              

            decimal decNrPedido = Convert.ToDecimal(txtNrPedido.Text);
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
            Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();

            RnPedido.PedidosEmEstoque(decNrPedido, p);

            if (p.CodigoDocumento != 0)
            {
                if (p.CodigoEmpresa == Convert.ToInt32(Session["CodEmpresa"]))
                {
                    txtDocumento.Text = p.CodigoDocumento.ToString();

                    pnlRecontar.Visible = false;

                    if (p.CodigoSituacao == 156)
                    {
                        RemontagemCaixas(p.CodigoDocumento);
                        pnlRecontar.Visible = true;
                        btnFinalizar.Enabled = false;
                        ShowMessage("Pedido já Conferido!", MessageType.Info);
                        Session["PedidoJaConferido"] = "";
                    }

                    txtCliente.Text = p.NomeCliente.ToString();
                    txtSituacao.Text = p.DescricaoSituacao.ToString();
                    txtNrPedido.Text = decNrPedido.ToString();
                    txtDoca.Text = p.DescricaoDoca;

                    Lst1Grid = RnPedido.ListarPedidosEmEstoque(decNrPedido);
                    if (Session["PedidoJaConferido"] != null)
                    {
                        foreach (Doc_Pedido_Estoque item in Lst1Grid)
                        {
                            p = new Doc_Pedido_Estoque();


                            p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                            p.CodigoProduto = item.CodigoProduto;
                            p.DescricaoProduto = item.DescricaoProduto;
                            p.QtAtendida = item.QtAtendida;
                            p.CodigoBarras = item.CodigoBarras;
                            p.QtColetada = item.QtAtendida;

                            Lst2Man.Add(p);
                        }
                        Lst1Grid.Clear();
                        Lst1Grid = Lst2Man;
                        Lst1Grid = Lst1Grid.OrderByDescending(x => x.Saldo).ToList();
                        Session["LstGridCnf"] = Lst1Grid;
                    }
                    else
                    {
                        Session["LstGridCnf"] = Lst1Grid;
                    }
                    grdGrid.DataSource = Lst1Grid;
                    grdGrid.DataBind();
                    if (grdGrid.Rows.Count == 0)
                    {
                        ShowMessage("Não existem Produtos para serem Conferidos", MessageType.Info);
                        LimpaCampos();
                        return;
                    }
                    else
                    {
                        Habil_Estacao he = new Habil_Estacao();
                        Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

                        int intMaquina = 0;

                        he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
                        if (he != null)
                        {
                            intMaquina = Convert.ToInt32(he.CodigoEstacao);
                        }

                        int intSituacaoExistente = 0;

                        intSituacaoExistente = RnPedido.PesquisarDocumentoEmConferencia(p.CodigoDocumento);
                        if (intSituacaoExistente != 162)
                        {
                            RnPedido.InserirEventoDocumentoConferido(p.CodigoDocumento, 162, intMaquina, Convert.ToInt32(Session["CodUsuario"].ToString()));
                        }
                        txtNrPedido.Enabled = false;
                    }
                    txtCod.Focus();
                }
                else
                {
                    txtNrPedido.Text = "";
                    ShowMessage("Empresa do Documento/Pedido incoerente com Empresa informada", MessageType.Info);
                }
            }
            else
            {
                txtNrPedido.Text = "";
                ShowMessage("Pedido não Encontrado", MessageType.Info);
                return;
            }

            PanelSelect = "aba1";
            Session["TabFocadaConf"] = "aba1";
            btnNada_Click(sender, e);
            pnlPrincipal.Visible = true;
        }
        protected void btnAbrirFechar_Click(object sender, EventArgs e)
        {
            if (Session["PedidoJaConferido"] != null)
            {
                ShowMessage("Pedido Já Conferido", MessageType.Info);
                return;
            }
            if (btnAbrirFechar.Text == "Abrir")
            {
                List<Doc_Pedido_Estoque> LstFim = new List<Doc_Pedido_Estoque>();

                LstFim = (List<Doc_Pedido_Estoque>)Session["LstGridCnf"];

                int j = 0;
                foreach (Doc_Pedido_Estoque item in LstFim)
                {
                    if (item.QtAtendida == item.QtColetada)
                        j = j + 1;
                }
                if (j == LstFim.Count())
                {
                    ShowMessage("Produto do Pedido já Concluídos", MessageType.Warning);
                    return;
                }
            }

            txtCod.Enabled = true;
            string strlbl = "";
            if (txtNrPedido.Text == "")
            {
                ShowMessage("Selecione um pedido!", MessageType.Info);
                return;
            }
            if (Session["StrLbn"] != null)
            {
                BtnCaixaRetornavel.Checked = true;
            }
            int i = 0;
            if (BtnCaixaEmpresa.Checked == false)
            {
                i = i + 1;
            }
            else
            {
                strlbl = "CPE";
            }
            if (BtnCaixaOutros.Checked == false)
            {
                i = i + 1;
            }
            else
            {
                strlbl = "CPO";
            }
            if (BtnCaixaRetornavel.Checked == false)
            {
                i = i + 1;
            }
            else
            {
                strlbl = "RET ";
            }
            if (i == 3)
            {
                ShowMessage("Selecione um tipo de Embalagem", MessageType.Info);
                return;
            }

            if (btnAbrirFechar.Text == "Fechar")
            {
                if (Panel10.Visible == true)
                {
                    if(grdCaixa10.Rows.Count > 0)
                    { 
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
            }
                if (Panel9.Visible == true)
                {
                    if (grdCaixa9.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
                if (Panel8.Visible == true)
                {
                    if (grdCaixa8.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
                if (Panel7.Visible == true)
                {
                    if (grdCaixa7.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
                if (Panel6.Visible == true)
                {
                    if (grdCaixa6.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
                if (Panel5.Visible == true)
                {
                    if (grdCaixa5.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
                if (Panel4.Visible == true)
                {
                    if (grdCaixa4.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
                if (Panel3.Visible == true)
                {
                    if (grdCaixa3.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
                if (Panel2.Visible == true)
                {
                    if (grdCaixa2.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
                if (Panel1.Visible == true)
                {
                    if (grdCaixa1.Rows.Count > 0)
                    {
                        VoltarCaixas();
                        btnAbrirFechar.Text = "Abrir";
                        return;
                    }
                    else
                    {
                        ShowMessage("Embalagem deve Conter pelo menos um Produto", MessageType.Info);
                        return;
                    }
                }
            }
            else
            {
                if (Panel1.Visible == false)
                {                    
                    btnAbrirFechar.Text = "Fechar";
                    Label1.Text = strlbl;
                    Panel1.Visible = true;
                    if (Session["StrLbn"] != null)
                    {
                        Label11.Text = Session["StrLbn"].ToString();
                    }
                    btnNada_Click(sender, e);
                    txtVolume.Text = "1";
                    return;
                }
                if (Panel2.Visible == false)
                {
                    txtVolume.Text = "2";
                    btnAbrirFechar.Text = "Fechar";
                    Label2.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label12.Text = Session["StrLbn"].ToString();
                    }
                    Panel2.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
                if (Panel3.Visible == false)
                {
                    txtVolume.Text = "3";
                    btnAbrirFechar.Text = "Fechar";
                    Label3.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label13.Text = Session["StrLbn"].ToString();
                    }
                    Panel3.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
                if (Panel4.Visible == false)
                {
                    txtVolume.Text = "4";
                    btnAbrirFechar.Text = "Fechar";
                    Label4.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label14.Text = Session["StrLbn"].ToString();
                    }
                    Panel4.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
                if (Panel5.Visible == false)
                {
                    txtVolume.Text = "5";
                    btnAbrirFechar.Text = "Fechar";
                    Label5.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label15.Text = Session["StrLbn"].ToString();
                    }
                    Panel5.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
                if (Panel6.Visible == false)
                {
                    txtVolume.Text = "6";
                    btnAbrirFechar.Text = "Fechar";
                    Label6.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label16.Text = Session["StrLbn"].ToString();
                    }
                    Panel6.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
                if (Panel7.Visible == false)
                {
                    txtVolume.Text = "7";
                    btnAbrirFechar.Text = "Fechar";
                    Label7.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label17.Text = Session["StrLbn"].ToString();
                    }
                    Panel7.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
                if (Panel8.Visible == false)
                {
                    txtVolume.Text = "8";
                    btnAbrirFechar.Text = "Fechar";
                    Label8.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label18.Text = Session["StrLbn"].ToString();
                    }
                    Panel8.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
                if (Panel9.Visible == false)
                {
                    txtVolume.Text = "9";
                    btnAbrirFechar.Text = "Fechar";
                    Label9.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label19.Text = Session["StrLbn"].ToString();
                    }
                    Panel9.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
                if (Panel10.Visible == false)
                {
                    txtVolume.Text = "10";
                    btnAbrirFechar.Text = "Fechar";
                    Label10.Text = strlbl;
                    if (Session["StrLbn"] != null)
                    {
                        Label20.Text = Session["StrLbn"].ToString();
                    }
                    Panel10.Visible = true;
                    btnNada_Click(sender, e);
                    return;
                }
            }
            Session["StrLbn"] = null;
        }
        protected void txtCod_TextChanged(object sender, EventArgs e)
        {
            if (Session["PedidoJaConferido"] != null)
            {
                ShowMessage("Pedido Já Conferido", MessageType.Info);
                return;
            }
            Lst1Grid = (List<Doc_Pedido_Estoque>)Session["LstGridCnf"];

            int i = 0;
            if (txtNrPedido.Text == "")
            {
                ShowMessage("Digite um Pedido", MessageType.Info);
                LimpaCampos();
                return;
            }
            if (BtnCaixaEmpresa.Checked == false)
            {
                i = i + 1;
            }
            if (BtnCaixaOutros.Checked == false)
            {
                i = i + 1;
            }
            if (BtnCaixaRetornavel.Checked == false)
            {
                i = i + 1;
            }
            if (i == 3)
            {
                txtCod.Text = "";
                ShowMessage("Selecione um tipo de Embalagem", MessageType.Info);
                return;
            }
            if (btnAbrirFechar.Text == "Abrir")
            {
                txtCod.Text = "";
                ShowMessage("Abra um tipo de Embalagem", MessageType.Info);
                return;
            }
            if (txtCod.Text == "")
            {
                txtCod.Text = "";
                ShowMessage("Digite um Produto", MessageType.Info);
                return;
            }
            if (txtCod.Text == "0")
            {
                txtCod.Text = "";
                ShowMessage("Digite um Produto Valido", MessageType.Info);
                return;
            }
            Boolean blnCampoValido = false;
            string strMensagemR = "";
            v.CampoValido("Quantidade", txtQtd.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtQtd.Text = "1";
                txtCod.Text = "";
                ShowMessage("Quantidade deve ser Válida", MessageType.Info);
                txtQtd.Focus();

                return;
            }

            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
            Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();

            short decQtColetada = Convert.ToInt16(txtQtd.Text);
            decimal decNrPedido = Convert.ToDecimal(txtNrPedido.Text);
            string strCodProduto = txtCod.Text;
            string strCodProduto2 = txtCod.Text;
            string strBarrasCorreta = "";
            int CodProduto = 0;
            int CodProdutoCorreto = 0;
            int CodProdutoDocumento = 0;
            short sthQtEmb = 0;
            short sthLimite = 0;

            int num;
            bool isNum = Int32.TryParse(strCodProduto, out num);
            if (isNum)
            {
                if (strCodProduto2.Length > 7)
                {
                    RnPedido.ProdutoPedidosEmEstoque(ref CodProduto, ref strCodProduto2, decNrPedido, ref sthQtEmb);
                }
                else
                {
                    CodProduto = Convert.ToInt32(txtCod.Text);
                    strCodProduto2 = null;
                    RnPedido.ProdutoPedidosEmEstoque(ref CodProduto, ref strCodProduto2, decNrPedido, ref sthQtEmb);
                }
                if (CodProduto == 0)
                {
                    txtDesc.Text = "";
                    txtCod.Text = "";
                    ShowMessage("Produto/Barras não encontrado", MessageType.Warning);
                    return;
                }
            }
            else
            {
                RnPedido.ProdutoPedidosEmEstoque(ref CodProduto, ref strCodProduto2, decNrPedido, ref sthQtEmb);
                if (CodProduto == 0)
                {
                    txtDesc.Text = "";
                    txtCod.Text = "";
                    ShowMessage("Produto/Barras não encontrado", MessageType.Info);
                    return;
                }
            }
            
            Lst2Man = new List<Doc_Pedido_Estoque>();

            foreach (Doc_Pedido_Estoque item in Lst1Grid)
            {
                p = new Doc_Pedido_Estoque();

                if (item.QtEmbalagem > 1)
                {
                    decQtColetada = item.QtEmbalagem;
                    p.QtEmbalagem = item.QtEmbalagem;
                    txtQtd.Text = p.QtEmbalagem.ToString();
                }

                if (strCodProduto.Length > 7)
                {
                    p.QtColetada = item.QtColetada;

                    if (item.CodigoBarras == strCodProduto)
                    {

                        p.QtColetada += +decQtColetada;
                        txtDesc.Text = item.DescricaoProduto;
                        CodProdutoCorreto = item.CodigoProduto;
                        CodProdutoDocumento = item.CodigoProdutoDocumento;
                    }
                }
                else
                {
                    p.QtColetada = item.QtColetada;
                    if (item.CodigoProduto == Convert.ToInt32(strCodProduto))
                    {
                        p.QtColetada += +decQtColetada;
                        txtDesc.Text = item.DescricaoProduto;
                        CodProdutoCorreto = item.CodigoProduto;
                        CodProdutoDocumento = item.CodigoProdutoDocumento;
                    }
                }
                p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                p.CodigoProduto = item.CodigoProduto;
                strBarrasCorreta = item.CodigoBarras;
                p.DescricaoProduto = item.DescricaoProduto;
                p.QtAtendida = item.QtAtendida;
                p.CodigoBarras = item.CodigoBarras;
                if (p.QtColetada > p.QtAtendida)
                {
                    p.QtColetada += -decQtColetada;
                    ShowMessage("Quantidade Coletada ultrapassa Limite Atendido.", MessageType.Info);
                    decQtColetada = Convert.ToInt16(p.QtColetada);
                    sthLimite = 1;
                }

                p.Saldo = (item.QtAtendida - p.QtColetada);

                Lst2Man.Add(p);
            }
            Lst1Grid.Clear();
            Lst1Grid = Lst2Man;
            Lst1Grid = Lst1Grid.OrderByDescending(x => x.Saldo).ToList(); 
            Session["LstGridCnf"] = Lst1Grid;
            grdGrid.DataSource = Lst1Grid;
            grdGrid.DataBind();

            foreach (GridViewRow row in grdGrid.Rows)
            {
                Label lblSaldo = (Label)row.Cells[6].FindControl("lblSaldo");

                if(lblSaldo.Text == "0,000")
                    lblSaldo.ForeColor = Color.Black;
                else
                    lblSaldo.ForeColor = Color.Red;
            }

            if (sthLimite == 0)
            {
                InjetaGrid(CodProdutoCorreto, CodProdutoDocumento);
            }
            btnNada_Click(sender, e);
            txtQtd.Text = "1";
        }
        protected void InjetaGrid(int intCodProduto, int CodProdutoDocumento)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
            Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();

            List<Doc_Pedido_Estoque> LstGridSession = new List<Doc_Pedido_Estoque>();

            if (Panel10.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst10 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst10"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst10"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst10.Clear();
                Lst10 = Lst;
                Lst10 = Lst10.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa10.DataSource = Lst10;
                grdCaixa10.DataBind();
                Session["Lst10"] = Lst10;
                txtVolume.Text = "10";
                return;
            }
            if (Panel9.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst9 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst9"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst9"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst9.Clear();
                Lst9 = Lst;
                Lst9 = Lst9.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa9.DataSource = Lst9;
                grdCaixa9.DataBind();
                Session["Lst9"] = Lst9;
                txtVolume.Text = "9";
                return;
            }
            if (Panel8.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst8 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst8"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst8"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst8.Clear();
                Lst8 = Lst;
                Lst8 = Lst8.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa8.DataSource = Lst8;
                grdCaixa8.DataBind();
                Session["Lst8"] = Lst8;
                txtVolume.Text = "8";
                return;
            }
            if (Panel7.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst7 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst7"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst7"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst7.Clear();
                Lst7 = Lst;
                Lst7 = Lst7.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa7.DataSource = Lst7;
                grdCaixa7.DataBind();
                Session["Lst7"] = Lst7;
                txtVolume.Text = "7";
                return;
            }
            if (Panel6.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst6 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst6"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst6"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst6.Clear();
                Lst6 = Lst;
                Lst6 = Lst6.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa6.DataSource = Lst6;
                grdCaixa6.DataBind();
                Session["Lst6"] = Lst6;
                txtVolume.Text = "6";
                return;
            }
            if (Panel5.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst5 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst5"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst5"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst5.Clear();
                Lst5 = Lst;
                Lst5 = Lst5.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa5.DataSource = Lst5;
                grdCaixa5.DataBind();
                Session["Lst5"] = Lst5;
                txtVolume.Text = "5";
                return;
            }
            if (Panel4.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst4 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst4"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst4"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst4.Clear();
                Lst4 = Lst;
                Lst4 = Lst4.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa4.DataSource = Lst4;
                grdCaixa4.DataBind();
                Session["Lst4"] = Lst4;
                txtVolume.Text = "4";
                return;
            }
            if (Panel3.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst3 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst3"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst3"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst3.Clear();
                Lst3 = Lst;
                Lst3 = Lst3.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa3.DataSource = Lst3;
                grdCaixa3.DataBind();
                Session["Lst3"] = Lst3;
                txtVolume.Text = "3";
                return;
            }
            if (Panel2.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst2 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst2"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst2"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst2.Clear();
                Lst2 = Lst;
                Lst2 = Lst2.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa2.DataSource = Lst2;
                grdCaixa2.DataBind();
                Session["Lst2"] = Lst2;
                txtVolume.Text = "2";
                return;
            }
            if (Panel1.Visible == true)
            {
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst1 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                if (Session["Lst1"] != null)
                {
                    Lst.Clear();
                    LstGridSession = (List<Doc_Pedido_Estoque>)Session["Lst1"];

                    bool blnProdutoJaLancado = false;

                    foreach (Doc_Pedido_Estoque item in LstGridSession)
                    {
                        p = new Doc_Pedido_Estoque();

                        p.CodigoProduto = item.CodigoProduto;
                        p.DescricaoProduto = item.DescricaoProduto;
                        p.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                        p.QtColetada = item.QtColetada;
                        if (item.CodigoProduto == intCodProduto)
                        {
                            p.QtColetada += Convert.ToInt16(txtQtd.Text);
                            blnProdutoJaLancado = true;
                        }

                        Lst.Add(p);
                    }
                    // Se for produto coleta nova
                    if (blnProdutoJaLancado == false)
                    {
                        p = new Doc_Pedido_Estoque();
                        p.CodigoProdutoDocumento = CodProdutoDocumento;
                        p.CodigoProduto = intCodProduto;
                        p.DescricaoProduto = txtDesc.Text;
                        p.QtColetada = Convert.ToInt16(txtQtd.Text);
                        Lst.Add(p);
                    }
                }
                else
                {
                    // primeira embalagem
                    p.CodigoProdutoDocumento = 0;
                    p.CodigoProduto = intCodProduto;
                    p.DescricaoProduto = txtDesc.Text;
                    p.CodigoProdutoDocumento = CodProdutoDocumento;
                    p.QtColetada = Convert.ToInt16(txtQtd.Text);
                    Lst.Add(p);
                }

                Lst1.Clear();
                Lst1 = Lst;
                Lst1 = Lst1.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa1.DataSource = Lst1;
                grdCaixa1.DataBind();
                Session["Lst1"] = Lst1;
                txtVolume.Text = "1";
                return;
            }
        }
        protected void btnNada_Click(object sender, EventArgs e)
        {
            if (txtNrPedido.Text == "")
                txtNrPedido.Focus();
            else
            {
                if (txtCod.Text == "")
                    txtCod.Focus();
            }
            txtCod.Text = "";
            txtQtd.Text = "1";
        }
        protected void btnNada2_Click(object sender, EventArgs e)
        {
            if (txtNrPedido.Text == "")
                txtNrPedido.Focus();
            else
            {
                if (txtCod.Text == "")
                    txtCod.Focus();
            }
            txtCod.Text = "";
            txtQtd.Text = "1";
        }
        protected void txtVolume_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtQtd_TextChanged(object sender, EventArgs e)
        {

        }
        protected void BtnCaixaRetornavel_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["PedidoJaConferido"] != null)
            {
                ShowMessage("Pedido Já Conferido", MessageType.Info);
                return;
            }
            BtnCaixaEmpresa.BackColor = Color.Silver;
            BtnCaixaOutros.BackColor = Color.Silver;
            BtnCaixaRetornavel.BackColor = Color.Magenta;


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "abc();", true);
        }
        protected void BtnCaixaOutros_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["PedidoJaConferido"] != null)
            {
                ShowMessage("Pedido Já Conferido", MessageType.Info);
                return;
            }
            BtnCaixaEmpresa.BackColor = Color.Silver;
            BtnCaixaOutros.BackColor = Color.Magenta;
            BtnCaixaRetornavel.BackColor = Color.Silver;
        }
        protected void BtnCaixa_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["PedidoJaConferido"] != null)
            {
                ShowMessage("Pedido Já Conferido", MessageType.Info);
                return;
            }
            BtnCaixaEmpresa.BackColor = Color.Magenta;
            BtnCaixaOutros.BackColor = Color.Silver;
            BtnCaixaRetornavel.BackColor = Color.Silver;
        }
        protected void txtCaixaRet_TextChanged(object sender, EventArgs e)
        {
            EmbalagemRetornavelDAL RnEmb = new EmbalagemRetornavelDAL();
            EmbalagemRetornavel p = new EmbalagemRetornavel();

            p = RnEmb.CaixasAtivas(txtCaixaRet.Text);

            if (p.CodigoIndice == 0)
            {
                BtnCaixaRetornavel_CheckedChanged(sender, e);
                lbnMens.Text = "Embalagem não Existe";
                lbnMens.ForeColor = Color.Red;
            }
            else
            {
                if (p.CodigoSituacao != 161)
                {
                    BtnCaixaRetornavel_CheckedChanged(sender, e);
                    lbnMens.Text = "Embalagem Está em Situação Ocupada";
                    lbnMens.ForeColor = Color.OrangeRed;
                }
                else
                {
                    txtCaixaRet.Text = "";
                    Session["StrLbn"] = p.CodigoEmbalagem;
                    btnAbrirFechar_Click(sender, e);
                }
            }
            txtCaixaRet.Text = "";
        }
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            PanelSelect = "aba1";
            Session["TabFocadaConf"] = "aba1";
            if (Session["PedidoJaConferido"] != null)
            {
                ShowMessage("Pedido Já Conferido", MessageType.Info);
                return;
            }
            if (txtNrPedido.Text == "")
            {
                ShowMessage("Selecione um Pedido!", MessageType.Info);
                return;
            }
            List<Doc_Pedido_Estoque> LstFim = new List<Doc_Pedido_Estoque>();
            LstFim = (List<Doc_Pedido_Estoque>)Session["LstGridCnf"];
            int i = 0;
            foreach (Doc_Pedido_Estoque item in LstFim)
            {
                if (item.QtAtendida == item.QtColetada)
                    i = i + 1;
            }
            if (i != LstFim.Count())
            {
                ShowMessage("Produto do Pedido ainda com itens a serem Concluídos",MessageType.Warning);
                return;
            }
            if (txtConfereVolume.Text != txtVolume.Text)
            {
                ShowMessage("Quantidade de Volumes incoerente com o Sistema", MessageType.Info);
                return;
            }
            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

            EmbalagemRetornavelDAL RnEmb = new EmbalagemRetornavelDAL();
            EmbalagemRetornavel d = new EmbalagemRetornavel();

            Doc_PedidoDAL RnDoc = new Doc_PedidoDAL();
            
            VolumeDocumento ep = new VolumeDocumento();
            VolumeDocumento ap = new VolumeDocumento();            

            ap.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);

            if (Label1.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();
                RnDoc = new Doc_PedidoDAL();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label1.Text;
                if (Label11.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label11.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += " [" + Label11.Text + "]";
                }
                RnDoc.InserirVolumeDocumento(ep);

                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst1"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label2.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();
                RnDoc = new Doc_PedidoDAL();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label2.Text;
                if (Label12.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label12.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += " [" + Label12.Text + "]";
                }
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst2"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label3.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label3.Text;
                if (Label13.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label13.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += "[" + Label13.Text +"]";
                }
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst3"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label4.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label14.Text;
                if (Label14.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label14.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += "[" + Label14.Text + "]";
                }
                
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst4"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label5.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label5.Text;
                if (Label15.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label15.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += "[" + Label15.Text + "]";
                }
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst5"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label6.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label6.Text;
                if (Label16.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label16.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += "[" + Label16.Text + "]";
                }
                
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst6"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label7.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label7.Text;
                if (Label17.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label17.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += "[" + Label17.Text + "]";
                }
                
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst7"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label8.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label8.Text;
                if (Label18.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label18.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += "[" + Label18.Text + "]";
                }
                
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst8"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label9.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label9.Text;
                if (Label19.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label19.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += "[" + Label19.Text + "]";
                }
                
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst9"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }
            if (Label10.Text != "")
            {
                d = new EmbalagemRetornavel();
                ep = new VolumeDocumento();

                ep.CodigoDocumento = ap.CodigoDocumento;

                ep.DescricaoIdentificacao = Label10.Text;
                if (Label20.Text != "")
                {
                    d = RnEmb.CaixasAtivas(Label20.Text);
                    RnEmb.AtualizarCaixas(d.CodigoIndice);
                    ep.CodigoEmbalagem = d.CodigoIndice;
                    ep.DescricaoIdentificacao += "[" + Label20.Text + "]";
                }
                RnDoc.InserirVolumeDocumento(ep);
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();

                Lst = (List<Doc_Pedido_Estoque>)Session["Lst10"];

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    ep.QtEmbalagem = item.QtColetada;
                    ep.CodigoDocumento = Convert.ToDecimal(txtDocumento.Text);
                    ep.CodigoProdutoDocumento = item.CodigoProdutoDocumento;
                    RnDoc.InserirVolumeProdutoDocumento(ep);
                }
            }

           

            int intMaquina = 0;

            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            if (he != null)
            {
                intMaquina = Convert.ToInt32(he.CodigoEstacao);
            }
            RnDoc.AtualizarDocumento(ep.CodigoDocumento, Convert.ToInt16(txtConfereVolume.Text));
            RnDoc.InserirEventoDocumentoConferido(ep.CodigoDocumento, 156 , intMaquina, Convert.ToInt32(Session["CodUsuario"].ToString()));

            ShowMessage("Conferência de Pedido Concluida!", MessageType.Success);
            btnFinalizar.Enabled = false;
            btnNovo_Click(sender, e);
            LimpaSessions();
            LimpaGrids();
            LimpaLabels();
            DesabilitaPanels();
            PanelSelect = "aba1";
            Session["TabFocadaConf"] = "aba1";
        }
        protected void VoltarCaixas()
        {
            TabSelecionada();
            BtnCaixaEmpresa.BackColor = Color.Silver;
            BtnCaixaEmpresa.Checked = false;
            BtnCaixaOutros.BackColor = Color.Silver;
            BtnCaixaOutros.Checked = false;
            BtnCaixaRetornavel.BackColor = Color.Silver;
            BtnCaixaRetornavel.Checked = false;
        }
        protected void RemontagemCaixas(decimal decDocPedido)
        {
            List<Doc_Pedido_Estoque> LstGridSession = new List<Doc_Pedido_Estoque>();
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
            Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();

            int count = 0;

            count = RnPedido.CountCaixas(decDocPedido);

            txtVolume.Text = count.ToString();

            if (count == 10)
            {
                Panel10.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst10 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label10.Text = item.DescricaoIndentificacao;
                }

                Lst10.Clear();
                Lst10 = Lst;
                Lst10 = Lst10.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa10.DataSource = Lst10;
                grdCaixa10.DataBind();
                Session["Lst10"] = Lst10;
                
                count += - 1;
            }
            if (count == 9)
            {
                Panel9.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst9 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label9.Text = item.DescricaoIndentificacao;
                }

                Lst9.Clear();
                Lst9 = Lst;
                Lst9 = Lst9.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa9.DataSource = Lst9;
                grdCaixa9.DataBind();
                Session["Lst9"] = Lst9;

                count += -1;
            }
            if (count == 8)
            {
                Panel8.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst8 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label8.Text = item.DescricaoIndentificacao;
                }

                Lst8.Clear();
                Lst8 = Lst;
                Lst8 = Lst8.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa8.DataSource = Lst8;
                grdCaixa8.DataBind();
                Session["Lst8"] = Lst8;

                count += -1;
            }
            if (count == 7)
            {
                Panel7.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst7 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label7.Text = item.DescricaoIndentificacao;
                }

                Lst7.Clear();
                Lst7 = Lst;
                Lst7 = Lst7.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa7.DataSource = Lst7;
                grdCaixa7.DataBind();
                Session["Lst7"] = Lst7;

                count += -1;
            }
            if (count == 6)
            {
                Panel6.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst6 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label6.Text = item.DescricaoIndentificacao;
                }

                Lst6.Clear();
                Lst6 = Lst;
                Lst6 = Lst6.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa6.DataSource = Lst6;
                grdCaixa6.DataBind();
                Session["Lst6"] = Lst6;

                count += -1;
            }
            if (count == 5)
            {
                Panel5.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst5 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label5.Text = item.DescricaoIndentificacao;
                }

                Lst5.Clear();
                Lst5 = Lst;
                Lst5 = Lst5.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa5.DataSource = Lst5;
                grdCaixa5.DataBind();
                Session["Lst5"] = Lst5;

                count += -1;
            }
            if (count == 4)
            {
                Panel4.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst4 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label4.Text = item.DescricaoIndentificacao;
                }

                Lst4.Clear();
                Lst4 = Lst;
                Lst4 = Lst4.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa4.DataSource = Lst4;
                grdCaixa4.DataBind();
                Session["Lst4"] = Lst4;

                count += -1;
            }
            if (count == 3)
            {
                Panel3.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst3 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label3.Text = item.DescricaoIndentificacao;
                }

                Lst3.Clear();
                Lst3 = Lst;
                Lst3 = Lst3.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa3.DataSource = Lst3;
                grdCaixa3.DataBind();
                Session["Lst3"] = Lst3;
                count += -1;
            }
            if (count == 2)
            {
                Panel2.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst2 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label2.Text = item.DescricaoIndentificacao;
                }

                Lst2.Clear();
                Lst2 = Lst;
                Lst2 = Lst2.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa2.DataSource = Lst2;
                grdCaixa2.DataBind();
                Session["Lst2"] = Lst2;

                count += -1;
            }
            if (count == 1)
            {
                Panel1.Visible = true;
                List<Doc_Pedido_Estoque> Lst = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> Lst1 = new List<Doc_Pedido_Estoque>();
                List<Doc_Pedido_Estoque> LstReserva = new List<Doc_Pedido_Estoque>();

                Lst = RnPedido.ListarRecontagem(Convert.ToDecimal(txtDocumento.Text), count);

                foreach (Doc_Pedido_Estoque item in Lst)
                {
                    Label1.Text = item.DescricaoIndentificacao;
                }

                Lst1.Clear();
                Lst1 = Lst;
                Lst1 = Lst1.OrderByDescending(x => x.Saldo).ToList();
                grdCaixa1.DataSource = Lst1;
                grdCaixa1.DataBind();
                Session["Lst1"] = Lst1;

                count += -1;
            }
        }
        protected void btnRecontar_Click(object sender, EventArgs e)
        {
            Session["PedidoJaConferido"] = null;
            btnFinalizar.Enabled = true;
            Doc_PedidoDAL RnDoc = new Doc_PedidoDAL();
            Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();

            RnDoc.AtualizarDocumentoRecontagem(Convert.ToDecimal(txtDocumento.Text));
            RnDoc.ExcluirVolumeDoDocumento(Convert.ToDecimal(txtDocumento.Text));
            RnDoc.ExcluirProdutoDoVolumeDoDocumento(Convert.ToDecimal(txtDocumento.Text));

            LimpaSessions();
            LimpaGrids();
            LimpaLabels();
            DesabilitaPanels();
            txtVolume.Text = "";
            txtNrPedido_TextChanged(sender, e);
            PanelSelect = "aba1";
            Session["TabFocadaConf"] = "aba1";

        }
    }
}