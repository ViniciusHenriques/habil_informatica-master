using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class LibDocumento : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();

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

            if (Session["IncEmpresa"] != null)
            {
                pnlPainel.Visible = false;
                cmdSair.Visible = false;
                btnSair.Visible = false;
                //btnNovo.Visible = false;
                btnVoltar.Visible = true;
            }
            else
            {
                btnVoltar.Visible = false;
                pnlPainel.Visible = true;
                cmdSair.Visible = true;
                btnSair.Visible = true;
                //btnNovo.Visible = true;
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
                        btnLibCliente.Visible = false;
                        btnDevolver.Visible = false;
                        if (!x.AcessoIncluir)
                            //  btnNovo.Visible = false;

                            if (!x.AcessoConsulta)
                            {
                                // btnConsultar.Visible = false;
                                //grdGrid.Enabled = false;
                            }

                        if (!x.AcessoRelatorio)
                            btnImprimir.Visible = false;
                    }
                });

                LimpaCampos();
            }

            if (Session["SsnLibPedido"] != null)
            {
                string[] words = Session["SsnLibPedido"].ToString().Split('³');
                if (words[0] != "")
                {
                    ddlEmpresa.SelectedValue = words[0].ToString();
                    btnConsultar_Click(Session["sender"], null);
                }
                Session["SsnLibPedido"] = null;
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            Session["sender"] = sender;

            DBTabelaDAL d = new DBTabelaDAL();

            PanelSelect = "consulta";

            listaT.RemoveAll(x => x.Filtro == "CD_SITUACAO");

            Session["LST_ORCAMENTO"] = listaT;

            LiberacaoDocumentoDAL r = new LiberacaoDocumentoDAL();

            LinkButton t = (LinkButton)sender;

            grdGrid.DataSource = r.ListarLiberacoesGrid(listaT, Convert.ToInt32(ddlEmpresa.SelectedIndex), Convert.ToInt32(t.ToolTip));

            HabilTipoBloqueioDAL tipoBloqueio = new HabilTipoBloqueioDAL();

            string texto = tipoBloqueio.PesquisarHabilTipoBloqueio(Convert.ToInt32(t.ToolTip)).DescricaoBloqueio;

            lblLiberacao.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto.ToLower());

                       
            grdGrid.DataBind();

            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Boqueio(s) não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
                Session["LST_ORCAMENTO"] = null;
            }
        }


        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["Eventos"] = null;
            Session["Logs"] = null;
            Session["NovoAnexo"] = null;
            Session["TabFocada"] = null;

            Response.Redirect("~/Pages/Vendas/LibDocumento.aspx");
        }

        /*  protected */
        void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            //    grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";

            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void btnMensagem_Click(object sender, EventArgs e)
        {

        }
        protected void btnCfmNao_Click(object sender, EventArgs e)
        {

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            //    Response.Redirect("~/Pages/Acessos/CadComprador.aspx");
            //else
            //    Response.Redirect("~/Pages/Empresas/CadEmpresa.aspx");

        }
        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            PanelSelect = "home";
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {

        }
        protected void btnFreteObrigatorio_Click(object sender, EventArgs e)
        {

        }
        protected void btnLibCliente_Click(object sender, EventArgs e)
        {

            LiberacaoDocumento p = new LiberacaoDocumento();

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            p.CodigoMaquina = Convert.ToInt32(he.CodigoEstacao);
            p.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);

            LiberacaoDocumentoDAL libDal = new LiberacaoDocumentoDAL();

            LinkButton lb = (LinkButton)Session["sender"];

            p.CodigoBloqueio = Convert.ToInt32(lb.ToolTip);

            p.DataLiberacao = DateTime.Now;

            foreach (GridViewRow row in grdGrid.Rows)
            {
                var cell = (CheckBox)row.Cells[0].FindControl("chkLiberar");
                if (cell.Checked)
                {
                    p.CodigoDocumento = Convert.ToInt32(row.Cells[11].Text);
                    libDal.LiberarDocumento(p);
                }
            }


            btnConsultar_Click(Session["sender"], null);
            HabilitaBotoes();
        }
        protected void btnCreditoExcedido_Click(object sender, EventArgs e)
        {
        }
        protected void btnPrimeiraCompra_Click(object sender, EventArgs e)
        {

        }
        protected void btnClienteInadimplente_Click(object sender, EventArgs e)
        {
        }
        protected void btnOpcao1_Click(object sender, EventArgs e)
        {

        }
        protected void btnOpcao2_Click(object sender, EventArgs e)
        {

        }
        protected void btnOpcao3_Click(object sender, EventArgs e)
        {

        }
        protected void btnOpcao4_Click(object sender, EventArgs e)
        {

        }
        protected bool ValidaCampos()
        {
            if (ddlEmpresa.Text == "..... SELECIONE UMA EMPRESA .....")
            {
                ShowMessage("Selecione uma Empresa.", MessageType.Info);
                PanelSelect = "parameter";
                Session["TabFocada"] = "parameter";
                return false;
            }


            return true;
        }
        protected void CarregaEmpresas()
        {
            EmpresaDAL RnEmpresa = new EmpresaDAL();
            ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("", "", "", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();
            ddlEmpresa.Items.Insert(0, "*Nenhum Selecionado");
        }
        protected void CarregaSituacoes()
        {
            CarregaEmpresas();
            HabilitaBotoes();

        }

        protected void LimpaCampos()
        {
            CarregaSituacoes();
        }
        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {

            //btnConsultar_Click(Session["sender"], null);
            //HabilitaBotoes();
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void HabilitaBotoes()
        {
            btnCreditoExcedido.Enabled = false;
            btnCreditoExcedido.Text = "<span aria-hidden='true' title='Crédito Excedido'></span> Crédito Excedido";

            btnClienteInadimplente.Enabled = false;
            btnClienteInadimplente.Text = "<span aria-hidden='true' title='Inadimplente'></span> Inadimplente";

            btnPrimeiraCompra.Enabled = false;
            btnPrimeiraCompra.Text = "<span aria-hidden='true' title='Primeira Compra'></span> Primeira Compra";

            btnFreteObrigatorio.Enabled = false;
            btnFreteObrigatorio.Text = "<span aria-hidden='true' title='Frete Obrigatório'></span> Frete Obrigatório";


            HabilTipoBloqueioDAL dal = new HabilTipoBloqueioDAL();
            var lista = dal.ListarTiposDeBloqueios(Convert.ToInt32(ddlEmpresa.SelectedIndex));

            foreach (var i in lista)
            {
                if (i.CodigoBloqueio == 1)
                {
                    btnCreditoExcedido.Enabled = true;
                    btnCreditoExcedido.Text = btnCreditoExcedido.Text + " - " + i.QuantidadeBloqueios + " Pedido(s)";
                }

                if (i.CodigoBloqueio == 2)
                {
                    btnClienteInadimplente.Enabled = true;
                    btnClienteInadimplente.Text = btnClienteInadimplente.Text + " - " + i.QuantidadeBloqueios + " Pedido(s)";
                }

                if (i.CodigoBloqueio == 3)
                {
                    btnPrimeiraCompra.Enabled = true;
                    btnPrimeiraCompra.Text = btnPrimeiraCompra.Text + " - " + i.QuantidadeBloqueios + " Pedido(s)";
                }

                if (i.CodigoBloqueio == 4)
                {
                    btnFreteObrigatorio.Enabled = true;
                    btnFreteObrigatorio.Text = btnFreteObrigatorio.Text + " - " + i.QuantidadeBloqueios + " Pedido(s)";
                }
            }
        }
        protected void Devolver_Click(object sender, EventArgs e)
        {
            LiberacaoDocumentoDAL libDal = new LiberacaoDocumentoDAL();

            LiberacaoDocumento p = new LiberacaoDocumento();

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            p.CodigoMaquina = Convert.ToInt32(he.CodigoEstacao);
            p.CodigoUsuario = Convert.ToInt32(Session["CodPflUsuario"]);



            foreach (GridViewRow row in grdGrid.Rows)
            {
                var cell = (CheckBox)row.Cells[0].FindControl("chkLiberar");
                if (cell.Checked)
                {
                    LinkButton linkButton = (LinkButton)Session["sender"];

                    p.CodigoBloqueio = Convert.ToInt32(linkButton.ToolTip);
                    p.CodigoDocumento = Convert.ToInt32(row.Cells[11].Text);
                    p.DataLiberacao = DateTime.Now;

                    libDal.DevolverDocumentoPedido(p);

                }
            }
            btnConsultar_Click(Session["sender"], null);
            HabilitaBotoes();
        }
        protected void chkLiberar_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void btnAtualiza_Click(object sender, EventArgs e)
        {
            HabilitaBotoes();
        }
        protected void grdGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string x = e.CommandName;

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = grdGrid.Rows[index];

            LinkButton lb = (LinkButton)Session["sender"];

            if (x == "Pedido")
            {
                Session["ZoomPedido"] = Server.HtmlDecode(row.Cells[11].Text) + "³";
                Session["Eventos"] = null;
                Session["Logs"] = null;
                Session["NovoAnexo"] = null;
                Session["Ssn_TipoPessoa"] = null;
                Session["Ssn_ctaReceber"] = null;
                Session["NotaFiscalServico"] = null;
                Session["Doc_orcamento"] = null;
                Session["ListaItemOrcamento"] = null;
                Session["NovaBaixa"] = null;
                Session["CadOrcamento"] = null;
                Session["SsnLibPedido"] = ddlEmpresa.SelectedValue + "³" + lb.ToolTip;
                Response.Redirect("~/Pages/Vendas/ManPedido.aspx?cad=1");
            }

            if (x == "Orcamento")
            {
                string Codigo = Server.HtmlDecode(row.Cells[11].Text);

                Doc_PedidoDAL RnPed= new Doc_PedidoDAL();
                Doc_Pedido p = new Doc_Pedido();

                p = RnPed.PesquisarDocumento(Convert.ToDecimal(Codigo));

                Session["ZoomOrcamento"] = Codigo + "³";
                if (p != null)
                {
                    if (p.CodigoDocumentoOriginal != 0)
                        Session["ZoomOrcamento"] = p.CodigoDocumentoOriginal + "³";

                }

                Session["Eventos"] = null;
                Session["Logs"] = null;
                Session["NovoAnexo"] = null;
                Session["Ssn_TipoPessoa"] = null;
                Session["Ssn_ctaReceber"] = null;
                Session["NotaFiscalServico"] = null;
                Session["Doc_orcamento"] = null;
                Session["ListaItemOrcamento"] = null;
                Session["NovaBaixa"] = null;
                Session["CadOrcamento"] = null;
                Session["ListaOutrosOrcamentos"] = null;
                Session["ListaContaReceber"] = null;
                Session["SsnLibPedido"] = ddlEmpresa.SelectedValue + "³" + lb.ToolTip;
                Response.Redirect("~/Pages/Vendas/ManOrcamento.aspx?cad=1");


                //btnAcessar_Click(sender , e);
            }
        }
    }
}