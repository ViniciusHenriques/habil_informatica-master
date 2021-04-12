using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SoftHabilInformatica.Pages.Compras
{
    public partial class LibSolCompra : System.Web.UI.Page
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
            }
            else
            {
                pnlPainel.Visible = true;
                cmdSair.Visible = true;
                btnSair.Visible = true;
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "LibSolCompra.aspx");
                lista.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        btnAprSolicitacao.Visible = false;
                        btnCanSolicitacao.Visible = false;
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

            Session["LST_SOLCOMPRA"] = listaT;

            LiberacaoDocumentoDAL r = new LiberacaoDocumentoDAL();

            LinkButton t = (LinkButton)sender;

            grdGrid.DataSource = r.ListarLiberacoesSolCompraGrid(listaT, Convert.ToInt32(ddlEmpresa.SelectedIndex), 1);

            HabilTipoBloqueioDAL tipoBloqueio = new HabilTipoBloqueioDAL();

            string texto = tipoBloqueio.PesquisarHabilTipoBloqueioTipoDocumento(Convert.ToInt32(t.ToolTip),12).DescricaoBloqueio;

            lblLiberacao.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto.ToLower());

                       
            grdGrid.DataBind();

            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Solicitações a ser(em) Aprovadas não encontrada(s) mediante a pesquisa realizada.", MessageType.Info);
                Session["LST_ORCAMENTO"] = null;
            }
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
        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            PanelSelect = "home";
        }
        protected void btnAprSolicitacao_Click(object sender, EventArgs e)
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

            p.DataLiberacao = Convert.ToDateTime (new DBTabelaDAL().ObterDataHoraServidor());

            foreach (GridViewRow row in grdGrid.Rows)
            {
                var cell = (CheckBox)row.Cells[0].FindControl("chkLiberar");
                if (cell.Checked)
                {
                    p.CodigoLiberacao = Convert.ToInt32(row.Cells[10].Text);
                    p.CodigoDocumento= Convert.ToInt32(row.Cells[9].Text);
                    libDal.AprovarSolicitacaoCompra(p);
                }
            }


            btnConsultar_Click(Session["sender"], null);
            HabilitaBotoes();
        }
        protected void btnConsulta_Click(object sender, EventArgs e)
        {
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
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void HabilitaBotoes()
        {
            btnConsultar.Enabled = false;
            btnConsultar.Text = "<span aria-hidden='true' title='Consultar'></span> Documentos";

            HabilTipoBloqueioDAL dal = new HabilTipoBloqueioDAL();
            var lista = dal.ListarTiposDeBloqueiosSolCompra(Convert.ToInt32(ddlEmpresa.SelectedIndex));

            foreach (var i in lista)
            {
                if (i.CodigoBloqueio == 1)
                {
                    btnConsultar.Enabled = true;
                    btnConsultar.Text = btnConsultar.Text + " - " + i.QuantidadeBloqueios + " Solicitaçõ(es)";
                }
            }
        }
        protected void btnCanSolicitacao_Click(object sender, EventArgs e)
        {
            LiberacaoDocumentoDAL libDal = new LiberacaoDocumentoDAL();
            LiberacaoDocumento p = new LiberacaoDocumento();
            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

            if (txtMotivo.Text == "" )
            {
                PanelSelect = "consulta";
                ShowMessage("Motivo deve ser Informado!!!", MessageType.Info);
                return;
            }

            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            p.CodigoMaquina = Convert.ToInt32(he.CodigoEstacao);


            foreach (GridViewRow row in grdGrid.Rows)
            {
                var cell = (CheckBox)row.Cells[0].FindControl("chkLiberar");

                if (cell.Checked)
                {
                    LinkButton linkButton = (LinkButton)Session["sender"];

                    p.CodigoBloqueio = Convert.ToInt32(linkButton.ToolTip);
                    p.CodigoLiberacao = Convert.ToInt32(row.Cells[10].Text);
                    p.CodigoDocumento = Convert.ToInt32(row.Cells[9].Text);
                    p.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);
                    p.DataLiberacao = new DBTabelaDAL().ObterDataHoraServidor();

                    libDal.CancelarSolicitacaoCompra(p,txtMotivo.Text);

                }
            }

            txtMotivo.Text = "";
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
                Session["ZoomSolCompra"] = Server.HtmlDecode(row.Cells[9].Text) + "³";
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
                Response.Redirect("~/Pages/Compras/ManSolCompra.aspx?cad=1");

            }
        }
    }
}