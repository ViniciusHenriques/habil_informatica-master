using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class LisPedido : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        string strMensagemR = "";
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocadaLiberacao"] != null)
            {
                PanelSelect = Session["TabFocadaLiberacao"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaLiberacao"] = "home";
            }

            if (Session["CodEmpresa"] != null)
            {
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
            }

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                MontaSelecionar(sender, e);
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
            int intTipoMenu = 0;
            ParSistemaDAL RnPar = new ParSistemaDAL();

            RnPar.TipoDeListagemDePedidos(ref intTipoMenu, Convert.ToInt32(Session["CodEmpresa"]));
            if (intTipoMenu == 151)
            {
                if (Session["LISTAGEMPORPEDIDO"] != null)
                {
                    Habil_Estacao he = new Habil_Estacao();
                    Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                    Doc_PedidoDAL RnDoc = new Doc_PedidoDAL();
                    int intMaquina = 0;
                    string strMensagemErro = "";

                    he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                    if (he != null)
                    {
                        intMaquina = Convert.ToInt32(he.CodigoEstacao);
                    }
                    
                    RnDoc.InserirEventoDocumento(Convert.ToDecimal(Session["LISTAGEMPORPEDIDO"].ToString()), intMaquina, Convert.ToInt32(Session["CodUsuario"].ToString()));

                    int intCodDoca = 0;

                    if (Session["CodDoca"] != null)
                    {
                        intCodDoca = Convert.ToInt32(Session["CodDoca"].ToString());
                    }

                    RnDoc.ExecutaSpAtendimentoPedido(intMaquina, intCodDoca, Session["LISTAGEMPORPEDIDO"].ToString(), ref strMensagemErro);
                    if (strMensagemErro != "")
                        ShowMessage(strMensagemErro, MessageType.Error);

                    Response.Redirect("~/Pages/Vendas/RelLisPedido.aspx");
                }

                Session["Pagina"] = "Inicial";
                Response.Redirect("~/Pages/WelCome.aspx");
                this.Dispose();
            }
            CarregaEmpresas();
            if (Session["PaginaIniciada"] == null)
            {
                Session["PaginaIniciada"] = "a";
                EntradaTela();
            }
            if (Session["VoltaDoRel"] != null)
            {
                Session["LST_SP"] = null;
                Session["LST_IMPRIMIR"] = null;
                Session["VoltaDoRel"] = null;
                btnPesquisarItem_Click(sender, e); 
            }
        }
        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            ParSistemaDAL RnPar = new ParSistemaDAL();
            ddlEmpresa.Enabled = false;
            int intTipoMenu = 0;

            RnPar.TipoDeListagemDePedidos(ref intTipoMenu, Convert.ToInt32(Session["CodEmpresa"]));

            if (txtVarchar.Text != "")
            {
                DBTabelaDAL db = new DBTabelaDAL();
                String strTipo = db.ListarTipoCampoView("VW_LIS_DOCUMENTO", ddlPesquisa.SelectedValue).ToString().ToUpper();

                Boolean blnCampoValido = false;
                v.CampoValido(ddlPesquisa.SelectedValue.ToString().ToUpper(), txtVarchar.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtVarchar.Focus();
                        return;
                    }
                }
                Doc_PedidoDAL r = new Doc_PedidoDAL();
                grdGrid.DataSource = r.ListarPesquisa(ddlPesquisa.Text, strTipo, txtVarchar.Text, ddlPesquisa.Text);
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(ddlPesquisa.Text + ": " + ddlPesquisa.Text + ", não encontrado para Listagem.", MessageType.Info);
                    pnlBtn.Visible = false;
                    return;
                }
                if (intTipoMenu == 152)
                    ddlDoca.Enabled = false;

                pnlBtn.Visible = true;
                pnlGrid.Visible = true;
                Tp153();
            }
            else
            {
                if (intTipoMenu == 152)
                {
                    pnlGrid.Visible = false;
                    int intI = 1;
                    DocaDAL d = new DocaDAL();
                    List<Doca> Lista = new List<Doca>();
                    Lista = d.ListagemDocasPessoa(Convert.ToInt32(ddlEmpresa.SelectedValue));

                    foreach (Doca Lst in Lista)
                    {
                        if (intI == 1)
                        {
                            btnDoca1.Focus();
                            Doca1.Visible = true;
                            btnDoca1.Visible = true;
                            btnDoca1.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca1.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 2)
                        {
                            btnDoca2.Focus();
                            Doca2.Visible = true;
                            btnDoca2.Visible = true;
                            btnDoca2.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca2.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 3)
                        {
                            btnDoca3.Focus();
                            Doca3.Visible = true;
                            btnDoca3.Visible = true;
                            btnDoca3.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca3.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 4)
                        {
                            btnDoca4.Focus();
                            Doca4.Visible = true;
                            btnDoca4.Visible = true;
                            btnDoca4.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca4.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 5)
                        {
                            btnDoca5.Focus();
                            btnDoca5.Visible = true;
                            btnDoca5.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca5.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 6)
                        {
                            btnDoca6.Focus();
                            Doca6.Visible = true;
                            btnDoca6.Visible = true;
                            btnDoca6.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca6.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 7)
                        {
                            btnDoca7.Focus();
                            Doca7.Visible = true;
                            btnDoca7.Visible = true;
                            btnDoca7.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca7.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 8)
                        {
                            btnDoca8.Focus();
                            Doca8.Visible = true;
                            btnDoca8.Visible = true;
                            btnDoca8.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca8.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 9)
                        {
                            btnDoca9.Focus();
                            Doca9.Visible = true;
                            btnDoca9.Visible = true;
                            btnDoca9.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca9.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 10)
                        {
                            btnDoca10.Focus();
                            Doca10.Visible = true;
                            btnDoca10.Visible = true;
                            btnDoca10.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca10.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 11)
                        {
                            btnDoca11.Focus();
                            Doca11.Visible = true;
                            btnDoca11.Visible = true;
                            btnDoca11.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca11.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 12)
                        {
                            btnDoca12.Focus();
                            Doca12.Visible = true;
                            btnDoca12.Visible = true;
                            btnDoca12.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca12.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 13)
                        {
                            btnDoca13.Focus();
                            Doca13.Visible = true;
                            btnDoca13.Visible = true;
                            btnDoca13.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca13.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 14)
                        {
                            btnDoca14.Focus();
                            Doca14.Visible = true;
                            btnDoca14.Visible = true;
                            btnDoca14.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca14.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 15)
                        {
                            btnDoca15.Focus();
                            Doca15.Visible = true;
                            btnDoca15.Visible = true;
                            btnDoca15.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca15.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 16)
                        {
                            btnDoca16.Focus();
                            Doca16.Visible = true;
                            btnDoca16.Visible = true;
                            btnDoca16.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca16.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 17)
                        {
                            btnDoca17.Focus();
                            Doca17.Visible = true;
                            btnDoca17.Visible = true;
                            btnDoca17.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca17.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 18)
                        {
                            btnDoca18.Focus();
                            Doca18.Visible = true;
                            btnDoca18.Visible = true;
                            btnDoca18.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca18.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 19)
                        {
                            btnDoca19.Focus();
                            Doca19.Visible = true;
                            btnDoca19.Visible = true;
                            btnDoca19.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca19.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        if (intI == 20)
                        {
                            btnDoca20.Focus();
                            Doca20.Visible = true;
                            btnDoca20.Visible = true;
                            btnDoca20.Text = "Doca: " + Lst.CodigoDoca.ToString() + " - " + Lst.DescricaoDoca.ToString() + " - " + Lst.Cont.ToString() + " Pedidos";
                            btnDoca20.ToolTip = Lst.CodigoDoca.ToString();
                        }
                        intI++;
                    }
                    if (Lista.Count == 0)
                    {
                        ShowMessage("Empresa não possui Pedidos a serem Listados!", MessageType.Info);
                        pnlGrid.Visible = false;
                        pnlEscolhasDocas.Visible = false;
                        ddlPesquisa.Enabled = true;
                        txtVarchar.Enabled = true;
                        return;
                    }
                    pnlGrid.Visible = false;
                    pnlEscolhasDocas.Visible = true;
                    Tp152();
                    ddlDoca.Enabled = false;

                }
                if (intTipoMenu == 153)
                {
                    Tp153();
                    Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
                    grdGrid.DataSource = RnPedido.ListarDocumentosPorPedidos(Convert.ToInt32(ddlEmpresa.SelectedValue));
                    grdGrid.DataBind();
                    if (grdGrid.Rows.Count == 0)
                    {
                        ShowMessage("Não existem Pedidos para serem listados", MessageType.Info);
                        ddlPesquisa.Enabled = true;
                        txtVarchar.Enabled = true;
                    }
                    
                }
            }
            ObtemDoca();
        }
        protected void MontaSelecionar(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            ddlPesquisa.DataSource = d.ListarCamposView("VW_LIS_ATENDIMENTO");
            ddlPesquisa.DataTextField = "NomeComum";
            ddlPesquisa.DataValueField = "Coluna";
            ddlPesquisa.DataBind();
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            ddlEmpresa.Enabled = true;
            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            Doc_PedidoDAL RnDoc = new Doc_PedidoDAL();

            int intSituacao = 0;
            int intCdDocumento = 0;
            int intDoca = 0;
            string strMensagemErro = "";
            int intMaquina = 0;
            int Int = 0;
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            if (he != null)
            {
                intMaquina = Convert.ToInt32(he.CodigoEstacao);
            }
            foreach (GridViewRow row in grdGrid.Rows)
            {
                CheckBox chk = (CheckBox)row.Cells[0].FindControl("chkLiberar");
                CheckBox chkInd = (CheckBox)row.Cells[12].FindControl("chkLiberar");
                intSituacao = Convert.ToInt32(row.Cells[12].Text);
                intDoca = Convert.ToInt32(row.Cells[10].Text);
                if (chk.Checked == false)
                {
                    Int += 1;
                }
                if (chk.Checked == true)
                {
                    if (intSituacao == 146)
                    {
                        intCdDocumento = Convert.ToInt32(row.Cells[13].Text);
                        if (Session["LST_SP"] == null)
                            Session["LST_SP"] = intCdDocumento.ToString();
                        else
                            Session["LST_SP"] += ", " + intCdDocumento.ToString();
                    }
                    else
                    {
                        intCdDocumento = Convert.ToInt32(row.Cells[13].Text);
                        if (Session["LST_IMPRIMIR"] == null)
                            Session["LST_IMPRIMIR"] = intCdDocumento.ToString();
                        else
                            Session["LST_IMPRIMIR"] += ", " + intCdDocumento.ToString();
                    }
                }
            }
            if (grdGrid.Rows.Count == Int)
            {
                ShowMessage("Selecione Algum Documento!", MessageType.Info);
                btnImprimir.Visible = true;
                return;
            }
            if (intSituacao == 146)
            {
                if (ddlDoca.Enabled == true)
                {
                    if (ddlDoca.SelectedItem.Text == " * Escolha uma Doca * ")
                    {
                        ShowMessage("Selecione uma Doca", MessageType.Info);
                        btnImprimir.Visible = true;
                        return;
                    }
                    else
                    {
                        intDoca = Convert.ToInt32(ddlDoca.SelectedValue);
                    }
                }
                RnDoc.ExecutaSpAtendimentoPedido(intMaquina, intDoca, Session["LST_SP"].ToString(), ref strMensagemErro);
            }

            

            if (strMensagemErro != "")
                ShowMessage(strMensagemErro, MessageType.Error);
            else
            {
                int intTipoMenu = 0;
                ParSistemaDAL RnPar = new ParSistemaDAL();

                RnPar.TipoDeListagemDePedidos(ref intTipoMenu, Convert.ToInt32(Session["CodEmpresa"]));

                if (chkImprimir.Checked == true)
                {
                    Session["PaginaIniciada"] = null;
                    if (Session["LST_IMPRIMIR"] != null)
                    {
                        Session["LST_REL"] = Session["LST_IMPRIMIR"];
                        Session["IMPRIMIR_DIRETO"] = null;
                    }
                    else
                    {
                        Session["LST_REL"] = Session["LST_SP"];
                        Session["Doca"] = intDoca.ToString();
                        Session["IMPRIMIR_DIRETO"] = null;
                    }
                    Response.Redirect("~/Pages/Vendas/RelLisPedido.aspx");
                }
                else
                {
                    if (Session["LST_IMPRIMIR"] != null)
                    {
                        Session["LST_REL"] = Session["LST_IMPRIMIR"];
                        Session["IMPRIMIR_DIRETO"] = "";
                    }
                    else
                    {
                        Session["Doca"] = intDoca.ToString();
                        Session["LST_REL"] = Session["LST_SP"];
                        Session["IMPRIMIR_DIRETO"] = "";
                    }
                    Response.Redirect("~/Pages/Vendas/RelLisPedido.aspx");
                }
                
                if (intTipoMenu == 152)
                {
                    //btnPesquisarItem_Click(sender, e);
                    pnlEscolhasDocas.Visible = false;
                    ddlDoca.Visible = false;
                    btnPesquisarItem_Click(sender, e);
                    Loop152();
                }
                if (intTipoMenu == 153)
                {
                    Loop153();
                    ObtemDoca();
                }

                Session["LST_IMPRIMIR"] = null;
                Session["LST_SP"] = null;
                ShowMessage("Listagem Gerada com Sucesso.", MessageType.Success);
            }
        }
        protected void Loop152()
        {
            int intI = 1;
            DocaDAL d = new DocaDAL();
            List<Doca> Lista = new List<Doca>();
            Lista = d.ListagemDocasPessoa(Convert.ToInt32(ddlEmpresa.SelectedValue));

            foreach (Doca Lst in Lista)
            {
                if (intI == 1)
                {
                    btnDoca1_Click(null, null);
                }
                if (intI == 2)
                {
                    btnDoca2_Click(null, null);
                }
                if (intI == 3)
                {
                    btnDoca3_Click(null, null);
                }
                if (intI == 4)
                {
                    btnDoca4_Click(null, null);
                }
                if (intI == 5)
                {
                    btnDoca5_Click(null, null);
                }
                if (intI == 6)
                {
                    btnDoca6_Click(null, null);
                }
                if (intI == 7)
                {
                    btnDoca7_Click(null, null);
                }
                if (intI == 8)
                {
                    btnDoca8_Click(null, null);
                }
                if (intI == 9)
                {
                    btnDoca9_Click(null, null);
                }
                if (intI == 10)
                {
                    btnDoca10_Click(null, null);
                }
                if (intI == 11)
                {
                    btnDoca11_Click(null, null);
                }
                if (intI == 12)
                {
                    btnDoca12_Click(null, null);
                }
                if (intI == 13)
                {
                    btnDoca13_Click(null, null);
                }
                if (intI == 14)
                {
                    btnDoca14_Click(null, null);
                }
                if (intI == 15)
                {
                    btnDoca15_Click(null, null);
                }
                if (intI == 16)
                {
                    btnDoca16_Click(null, null);
                }
                if (intI == 17)
                {
                    btnDoca17_Click(null, null);
                }
                if (intI == 18)
                {
                    btnDoca18_Click(null, null);
                }
                if (intI == 19)
                {
                    btnDoca19_Click(null, null);
                }
                if (intI == 20)
                {
                    btnDoca20_Click(null, null);
                }
                intI++;

                pnlGrid.Visible = true;
                pnlEscolhasDocas.Visible = true;
                Tp152();

            }
        }
        protected void Loop153()
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
            grdGrid.DataSource = RnPedido.ListarDocumentosPorPedidos(Convert.ToInt32(ddlEmpresa.SelectedValue));
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Não existem Pedidos para serem listados", MessageType.Info);
            }
            ObtemDoca();
            
        }
        protected void carregagrid()
        {
            pnlEscolhasDocas.Visible = false;
            pnlGrid.Visible = true;
            pnlBtn.Visible = true;
        }
        protected void chkLiberar_CheckedChanged(object sender, EventArgs e)
        {
            ddlEmpresa.Enabled = false;
            Doc_PedidoDAL RnDoc = new Doc_PedidoDAL();
            EventoDocumento p = new EventoDocumento();

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

            decimal decIndice = 0;
            int intCdSituacao = 0;

            int intMaquina = 0;

            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            if (he != null)
            {
                intMaquina = Convert.ToInt32(he.CodigoEstacao);
            }
            foreach (GridViewRow row in grdGrid.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLiberar");
                decIndice = Convert.ToDecimal(row.Cells[13].Text);
                intCdSituacao = Convert.ToInt32(row.Cells[12].Text);
                int intUsuario = 0;

                if (intCdSituacao != 146)
                {
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                    carregagrid();
                    btnImprimir.Visible = true;
                    return;
                }
                else
                {
                    if (chk.Checked == true)
                    {
                        RnDoc.UsuarioPedidosSelecionados(decIndice, ref intUsuario);

                        if (intUsuario != Convert.ToInt32(Session["CodUsuario"].ToString()) && intUsuario > 0)
                        {
                            chk.Checked = false;
                            ShowMessage("Pedido Já Consta como Selecionado", MessageType.Info);
                            carregagrid();
                        }
                        else
                        {
                            if (intUsuario == Convert.ToInt32(Session["CodUsuario"].ToString()) && intUsuario > 0)
                            {

                                carregagrid();
                            }
                            else
                            {
                                RnDoc.InserirEventoDocumento(decIndice, intMaquina, Convert.ToInt32(Session["CodUsuario"].ToString()));
                            }
                        }
                    }
                    else
                    {
                        RnDoc.DeleteEventoPedido(decIndice, Convert.ToInt32(Session["CodUsuario"].ToString()));
                        carregagrid();
                    }
                }
            }
            btnImprimir.Visible = true;
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnDoc = new Doc_PedidoDAL();
            foreach (GridViewRow row in grdGrid.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLiberar");
                if (chk.Checked == true)
                {
                    RnDoc.DeleteEventoPedido(Convert.ToDecimal(row.Cells[13].Text), Convert.ToInt32(Session["CodUsuario"].ToString()));
                    chk.Checked = false;
                }
            }
            ddlPesquisa.Enabled = true;
            txtVarchar.Enabled = true;
            txtVarchar.Text = "";
            pnlEscolhasDocas.Visible = false;
            pnlGrid.Visible = false;
            pnlBtn.Visible = false;
            btnSair.Visible = true;
            Session["PaginaIniciada"] = null;
            ddlEmpresa.Enabled = true;
        }
        protected void OnClick()
        {
            pnlGrid.Visible = true;
            pnlEscolhasDocas.Visible = false;
            pnlBtn.Visible = true;
        }
        protected void Tp152()
        {
            btnSair.Visible = true;
            pnlBtn.Visible = false;
        }
        protected void Tp153()
        {
            btnSair.Visible = false;
            pnlBtn.Visible = true;
            pnlGrid.Visible = true;
            pnlEscolhasDocas.Visible = false;
        }
        protected void ObtemDoca()
        {
            DocaDAL RnDoca = new DocaDAL();
            ddlDoca.DataSource = RnDoca.ObterDoca();
            ddlDoca.DataTextField = "DescricaoDoca";
            ddlDoca.DataValueField = "CodigoDoca";
            ddlDoca.SelectedValue = null;
            ddlDoca.DataBind();
            ddlDoca.Items.Insert(0, " * Escolha uma Doca * ");
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["PaginaIniciada"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void CarregaEmpresas()
        {
            EmpresaDAL RnEmpresa = new EmpresaDAL();
            ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("", "", "", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();
        }
        protected void btnDoca1_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
            if (Doca1.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca1.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca2_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca2.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca2.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)

                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca3_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca3.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca3.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca4_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca4.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca4.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca5_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca5.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca5.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca6_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca6.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca6.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca7_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca7.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca7.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca8_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca8.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca8.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca9_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca9.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca9.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca10_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca10.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca10.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca11_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca11.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca11.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca12_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca12.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca12.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca13_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca13.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca13.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca14_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca14.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca14.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca15_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca15.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca15.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca16_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca16.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca16.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca17_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca17.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca17.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca18_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca18.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca18.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca19_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca19.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca19.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void btnDoca20_Click(object sender, EventArgs e)
        {
            Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();

            if (Doca20.Visible != false)
            {

                grdGrid.DataSource = RnPedido.ListarDocDoca(Convert.ToInt32(btnDoca20.ToolTip), Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                OnClick();
            }
        }
        protected void EntradaTela()
        {
            Doc_PedidoDAL RnDoc = new Doc_PedidoDAL();

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

            decimal decIndice1 = 0;
            decimal decNrDocumento = 0;
            string StrPedidos = "";
            int intTipoMenu = 0;
            ParSistemaDAL RnPar = new ParSistemaDAL();

            RnPar.TipoDeListagemDePedidos(ref intTipoMenu, Convert.ToInt32(Session["CodEmpresa"]));
            if (intTipoMenu == 151)
            {
                Session["Pagina"] = "Inicial";
                Response.Redirect("~/Pages/WelCome.aspx");
                this.Dispose();
            }
            if (intTipoMenu == 152)
            {
                //doca
                pnlBtn.Visible = false;
                pnlEscolhasDocas.Visible = true;
                pnlGrid.Visible = false;
            }
            if (intTipoMenu == 153)
            {
                pnlEscolhasDocas.Visible = false;
                pnlGrid.Visible = false;
                pnlBtn.Visible = false;
                Doc_PedidoDAL RnPedido = new Doc_PedidoDAL();
                grdGrid.DataSource = RnPedido.ListarDocumentosPorPedidos(Convert.ToInt32(ddlEmpresa.SelectedValue));
                grdGrid.DataBind();
                if (grdGrid.Rows.Count == 0)
                {
                    ShowMessage(" Não existem Pedidos para serem listados", MessageType.Info);
                }
                ObtemDoca();
            }
            foreach (GridViewRow row in grdGrid.Rows)
            {
                decIndice1 = Convert.ToDecimal(row.Cells[13].Text);
                CheckBox chk = (CheckBox)row.FindControl("chkLiberar");
                int intUsuario1 = Convert.ToInt32(Session["CodUsuario"].ToString());
                int intUsuario2 = 0;

                RnDoc.CheckedMarcados(ref decIndice1, ref intUsuario2);

                if (intUsuario2 != intUsuario1 && intUsuario2 > 0)
                {
                    chk.Enabled = false;
                    decNrDocumento = Convert.ToDecimal(row.Cells[1].Text);
                    StrPedidos += decNrDocumento.ToString() + ", ";
                }
                else
                {
                    if (decIndice1 > 0 && intUsuario2 == intUsuario1)
                        chk.Checked = true;
                }
            }
            if (StrPedidos != "")
                ShowMessage("Pedido(s) " + StrPedidos + " selecionado(s) por outro Usuário ", MessageType.Info);
        }
    }
}