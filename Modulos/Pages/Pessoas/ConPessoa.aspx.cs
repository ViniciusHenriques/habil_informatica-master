using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.ComponentModel;
using ClosedXML.Excel;
using System.IO;
using System.IO.Compression;

namespace SoftHabilInformatica.Pages.Pessoas
{
    public partial class ConPessoa : System.Web.UI.Page
    {
        public bool btnSelecionar { get; set; }
        public bool btnOpcoes { get; set; }
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaDropDownList()
        {
            GrupoPessoaDAL sd = new GrupoPessoaDAL();
            ddlGrupoPessoa.DataSource = sd.ObterGrupoPessoa();
            ddlGrupoPessoa.DataTextField = "DescricaoGrupoPessoa";
            ddlGrupoPessoa.DataValueField = "CodigoGrupoPessoa";
            ddlGrupoPessoa.DataBind();
            ddlGrupoPessoa.Items.Insert(0, "*Nenhum Selecionado");

 
        }
        protected void MontaFiltro(object sender, EventArgs e)
        {
            int intI = 1;
            DBTabelaDAL d = new DBTabelaDAL();
            List<DBTabela> Lista = new List<DBTabela>();
            Lista = d.ListarCamposView("VW_PESSOA");

            foreach (DBTabela Lst in Lista)
            {
                if (intI == 1)
                {
                    txtFiltro11.Focus();
                    pnlFiltro1.Visible = true;
                    lblFiltro1.Text = Lst.NomeComum.ToString();
                    lblFiltro1.ToolTip = Lst.Coluna.ToString();
                    txtFiltro11.Text = "";
                    txtFiltro12.Text = "";
                    txtFiltro11.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro12.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }
                if (intI == 2)
                {
                    pnlFiltro2.Visible = true;
                    lblFiltro2.Text = Lst.NomeComum.ToString();
                    lblFiltro2.ToolTip = Lst.Coluna.ToString();
                    txtFiltro21.Text = "";
                    txtFiltro22.Text = "";
                    txtFiltro21.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro22.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }
                if (intI == 3)
                {
                    pnlFiltro3.Visible = true;
                    lblFiltro3.Text = Lst.NomeComum.ToString();
                    lblFiltro3.ToolTip = Lst.Coluna.ToString();
                    txtFiltro31.Text = "";
                    txtFiltro32.Text = "";
                    txtFiltro31.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro32.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }
                if (intI == 4)
                {
                    pnlFiltro4.Visible = true;
                    lblFiltro4.Text = Lst.NomeComum.ToString();
                    lblFiltro4.ToolTip = Lst.Coluna.ToString();
                    txtFiltro41.Text = "";
                    txtFiltro42.Text = "";
                    txtFiltro41.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro42.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }
                if (intI == 5)
                {
                    pnlFiltro5.Visible = true;
                    lblFiltro5.Text = Lst.NomeComum.ToString();
                    lblFiltro5.ToolTip = Lst.Coluna.ToString();
                    txtFiltro51.Text = "";
                    txtFiltro52.Text = "";
                    txtFiltro51.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro52.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }
                if (intI == 6)
                {
                    pnlFiltro6.Visible = true;
                    lblFiltro6.Text = Lst.NomeComum.ToString();
                    lblFiltro6.ToolTip = Lst.Coluna.ToString();
                    txtFiltro61.Text = "";
                    txtFiltro62.Text = "";
                    txtFiltro61.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro62.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }


                if (intI == 7)
                {
                    pnlFiltro7.Visible = true;
                    lblFiltro7.Text = Lst.NomeComum.ToString();
                    lblFiltro7.ToolTip = Lst.Coluna.ToString();
                    txtFiltro71.Text = "";
                    txtFiltro72.Text = "";
                    txtFiltro71.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro72.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }
                if (intI == 8)
                {
                    pnlFiltro8.Visible = true;
                    lblFiltro8.Text = Lst.NomeComum.ToString();
                    lblFiltro8.ToolTip = Lst.Coluna.ToString();
                    txtFiltro81.Text = "";
                    txtFiltro82.Text = "";
                    txtFiltro81.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro82.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }
                if (intI == 9)
                {
                    pnlFiltro9.Visible = true;
                    lblFiltro9.Text = Lst.NomeComum.ToString();
                    lblFiltro9.ToolTip = Lst.Coluna.ToString();
                    txtFiltro91.Text = "";
                    txtFiltro92.Text = "";
                    txtFiltro91.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro92.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }
                if (intI == 10)
                {
                    pnlFiltro10.Visible = true;
                    lblFiltro10.Text = Lst.NomeComum.ToString();
                    lblFiltro10.ToolTip = Lst.Coluna.ToString();
                    txtFiltro101.Text = "";
                    txtFiltro102.Text = "";
                    txtFiltro101.MaxLength = Convert.ToInt32(Lst.Tamanho);
                    txtFiltro102.MaxLength = Convert.ToInt32(Lst.Tamanho);
                }


                intI++;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ListaRepresentantes"] = null;
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");
            Session["TabFocada"] = "home";
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (IsPostBack)
            {
                Session["Ordenacao"] = "ASC";
            }

            if (Session["TabFocadaConPessoa"] != null)
            {
                PanelSelect = Session["TabFocadaConPessoa"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaConPessoa"] = "home";
                txtFiltro11.Focus();
            }


            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Request.QueryString["Cad"] != null)
            {
                //pnlPainel.Visible = false;
                cmdSair.Visible = false;
                btnSair.Visible = false;
                btnNovo.Visible = false;
                btnVoltar.Visible = true;
            }
            else
            {
                btnVoltar.Visible = false;
                //pnlPainel.Visible = true;
                cmdSair.Visible = true;
                btnSair.Visible = true;
                btnNovo.Visible = true;
            }

            if (Session["ZoomPessoa2"] != null)
            {
                if (Session["ZoomPessoa2"].ToString() == "RELACIONAL")
                {
                    //pnlPainel.Visible = false;
                    cmdSair.Visible = false;
                }
                else
                {
                    //pnlPainel.Visible = true;
                    cmdSair.Visible = true;
                }
            }
            else
            {
                //pnlPainel.Visible = true;
                cmdSair.Visible = true;
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                MontaFiltro(sender, e);
                MontaDropDownList();

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConPessoa.aspx");
                lista.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoIncluir)
                            btnNovo.Visible = false;

                        if (!x.AcessoConsulta)
                        {
                            btnConsultar.Visible = false;
                            grdGrid.Enabled = false;
                        }

                    }
                });

                if (Session["CodUsuario"].ToString() == "-150380")
                {
                    btnNovo.Visible = true;
                    btnConsultar.Visible = true;
                    grdGrid.Enabled = true;
                }
                if (Session["ZoomPessoa2"] != null)
                {
                    //btnImprimir.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                {
                    if (Request.QueryString["Cad"] != null)
                        btnVoltar.Visible = true;

                    else
                        btnVoltar.Visible = false;
                }
            }
            if (!lblFiltro1.Visible)
            {
                btnVoltar_Click(sender, e);
            }
            
            if (Convert.ToInt32(Request.QueryString["Cad"]) >= 1 && Convert.ToInt32(Request.QueryString["Cad"]) <= 21)
            {
                btnSelecionar = true;
                btnOpcoes = false;
           
            }
            else
            {
                btnSelecionar = false;
                btnOpcoes = true;
            }

            if (!IsPostBack)
            {
                //SE NÃO TIVER NENHUM MATRICULADO, SOME O BOTÃO
                DataTable dt = new DataTable();
                PessoaDAL p = new PessoaDAL();
                dt = p.RelatorioMatriculados(0);
                if (dt.Rows.Count == 0)
                    btnImprimirRelatorioMatriculados.Visible = false;

                dt = p.ListarPessoasComRepresentantes(null);
                if (dt.Rows.Count == 0)
                    btnImprimirRelatorioRepresentantes.Visible = false;
                //-------------------------------------------------------------------------------


                if (Session["LST_CADPESSOA"] != null)
                {
                    listaT = (List<DBTabelaCampos>)Session["LST_CADPESSOA"];
                    if (listaT.Count != 0)
                    {
                        foreach (var item in listaT)
                        {
                            if (item.Filtro == "CD_GPO_PESSOA")
                            {
                                ddlGrupoPessoa.SelectedValue = item.Inicio;
                                Session["LST_CADPESSOA"] = null;
                            }
                            else if (item.Filtro == "QUANTIDADE_REGISTROS")
                            {
                                ddlRegistros.SelectedValue = item.Inicio;
                                Session["LST_CADPESSOA"] = null;
                            }
                            
                        }
                    }
                    btnConsultar_Click(sender, e);
                }
            }

            Session["ZoomAnexoTimelineCliente"] = null;
            Session["ZoomAnexoTimelineCliente2"] = null;
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {

            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = "";
            Boolean blnCampoValido = false;

            lblTipo.Text = "";
            listaT.Clear();
            
            if (pnlFiltro1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro1.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro1.Text, txtFiltro11.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro11.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro1.Text, txtFiltro12.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro12.Focus();
                        return;
                    }

                if ((txtFiltro11.Text != "") || (txtFiltro12.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro1.ToolTip.ToString();
                    rowp.Inicio = txtFiltro11.Text;
                    rowp.Fim = txtFiltro12.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro2.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro2.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro2.Text, txtFiltro21.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro21.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro2.Text, txtFiltro22.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro22.Focus();
                        return;
                    }

                if ((txtFiltro21.Text != "") || (txtFiltro22.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro2.ToolTip.ToString();
                    rowp.Inicio = txtFiltro21.Text;
                    rowp.Fim = txtFiltro22.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro3.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro3.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro3.Text, txtFiltro31.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro31.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro3.Text, txtFiltro32.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro32.Focus();
                        return;
                    }
                if ((txtFiltro31.Text != "") || (txtFiltro32.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro3.ToolTip.ToString();
                    rowp.Inicio = txtFiltro31.Text;
                    rowp.Fim = txtFiltro32.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro4.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro4.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro4.Text, txtFiltro41.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro41.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro4.Text, txtFiltro42.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro42.Focus();
                        return;
                    }
                if ((txtFiltro41.Text != "") || (txtFiltro42.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro4.ToolTip.ToString();
                    rowp.Inicio = txtFiltro41.Text;
                    rowp.Fim = txtFiltro42.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro5.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro5.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro5.Text, txtFiltro51.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro51.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro5.Text, txtFiltro52.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro52.Focus();
                        return;
                    }
                if ((txtFiltro51.Text != "") || (txtFiltro52.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro5.ToolTip.ToString();
                    rowp.Inicio = txtFiltro51.Text;
                    rowp.Fim = txtFiltro52.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro6.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro6.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro6.Text, txtFiltro61.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro61.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro6.Text, txtFiltro62.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro62.Focus();
                        return;
                    }
                if ((txtFiltro61.Text != "") || (txtFiltro62.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro6.ToolTip.ToString();
                    rowp.Inicio = txtFiltro61.Text;
                    rowp.Fim = txtFiltro62.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            if (pnlFiltro7.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro7.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro7.Text, txtFiltro71.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro71.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro7.Text, txtFiltro72.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro72.Focus();
                        return;
                    }
                if ((txtFiltro71.Text != "") || (txtFiltro72.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro7.ToolTip.ToString();
                    rowp.Inicio = txtFiltro71.Text;
                    rowp.Fim = txtFiltro72.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            if (pnlFiltro8.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro8.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro8.Text, txtFiltro81.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro81.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro8.Text, txtFiltro82.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro82.Focus();
                        return;
                    }
                if ((txtFiltro81.Text != "") || (txtFiltro82.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro8.ToolTip.ToString();
                    rowp.Inicio = txtFiltro81.Text;
                    rowp.Fim = txtFiltro82.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            if (pnlFiltro9.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro9.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro9.Text, txtFiltro91.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro91.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro9.Text, txtFiltro92.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro92.Focus();
                        return;
                    }
                if ((txtFiltro91.Text != "") || (txtFiltro92.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro9.ToolTip.ToString();
                    rowp.Inicio = txtFiltro91.Text;
                    rowp.Fim = txtFiltro92.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            if (pnlFiltro10.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_PESSOA", lblFiltro10.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltro10.Text, txtFiltro101.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro101.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltro10.Text, txtFiltro102.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtFiltro102.Focus();
                        return;
                    }
                if ((txtFiltro101.Text != "") || (txtFiltro62.Text != ""))
                {

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro10.ToolTip.ToString();
                    rowp.Inicio = txtFiltro101.Text;
                    rowp.Fim = txtFiltro102.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }

            }

            short shtQtdReg = Convert.ToInt16(ddlRegistros.SelectedValue);

            if (ddlGrupoPessoa.SelectedValue != "*Nenhum Selecionado")
            {
                DBTabelaCampos rowp6 = new DBTabelaCampos();
                rowp6.Filtro = "CD_GPO_PESSOA";
                rowp6.Inicio = ddlGrupoPessoa.SelectedValue;
                rowp6.Fim = ddlGrupoPessoa.SelectedValue;
                rowp6.Tipo = "INT";
                listaT.Add(rowp6);
            }

            DBTabelaCampos rowp7 = new DBTabelaCampos();
            rowp7.Filtro = "QUANTIDADE_REGISTROS";
            rowp7.Inicio = ddlRegistros.SelectedValue;
            rowp7.Fim = ddlRegistros.SelectedValue;
            rowp7.Tipo = "INT";
            listaT.Add(rowp7);

            PanelSelect = "consulta";
            Session["LST_CADPESSOA"] = listaT;
            Session["ListaRepresentantes"] = null;
            listaT = listaT.Where(x => x.Filtro != "QUANTIDADE_REGISTROS").ToList();

            PessoaDAL r = new PessoaDAL();

            if (Session["Ssn_TipoPessoa"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 3)
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT,1, shtQtdReg);
                grdGrid.DataBind();
            }
            else if (Session["Ssn_CtaReceber"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 5)
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT, 2, shtQtdReg);
                grdGrid.DataBind();
            }
            else if (Session["NotaFiscalServico"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 6)
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT, 2, shtQtdReg);
                grdGrid.DataBind();
            }
            else if (Session["Vendedor"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 11)
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT, 2, shtQtdReg);
                grdGrid.DataBind();
            }
            else if (Session["CadUsuario"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 8)
            {
                grdGrid.DataSource = r.ListarPessoas("IN_USUARIO","SMALLINT","1","", shtQtdReg);
                grdGrid.DataBind();
            }
            else if (Session["Doc_SolicitacaoAtendimento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 9)
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT, 2, shtQtdReg);
                grdGrid.DataBind();
            }
            else if ((Session["Doc_OrdemServico"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 10) || (Session["RelContratos"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 20))
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT, 2, shtQtdReg);
                grdGrid.DataBind();
            }
            else if (Session["Doc_CTe"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 12)
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT, 3, shtQtdReg);
                grdGrid.DataBind();
            }
            else if ((Session["Doc_orcamento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 17) || (Session["Doc_Pedido"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 18))
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT, 3, shtQtdReg);
                grdGrid.DataBind();
            }
            else if ((Session["Doc_orcamento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 7))
            {
                grdGrid.DataSource = r.ListarPessoasCompleto(listaT, 2, shtQtdReg);
                grdGrid.DataBind();
            }
            else
            {
                lblTipo.Text = " >>> " + ddlTipoPessoa.SelectedItem.Text ;
                // sem informações de pesquisa não deixar aceitar todos registro vai dar OVER
                if (listaT.Count == 0)
                {
                    if (shtQtdReg != 32600)
                        grdGrid.DataSource = r.ListarPessoasCompleto(listaT, Convert.ToInt16(ddlTipoPessoa.SelectedValue), shtQtdReg);
                }
                else
                    grdGrid.DataSource = r.ListarPessoasCompleto(listaT, Convert.ToInt16(ddlTipoPessoa.SelectedValue), shtQtdReg);

                grdGrid.DataBind();

            }
            if (grdGrid.Rows.Count == 0)
                ShowMessage("Pessoa(s) não encontrada(s) mediante a pesquisa realizada.", MessageType.Info);
        }
        protected void grdGrid_SelectedIndexChanged(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                PanelSelect = "consulta";

                string x = e.CommandName;

                if (x == "Sort")
                {
                    return;
                }

                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = grdGrid.Rows[index];
                string Codigo = Server.HtmlDecode(row.Cells[0].Text);
                string strNomePessoa = Server.HtmlDecode(row.Cells[2].Text);
                Session["ZoomPessoa"] = Codigo;

                if (x == "Geral")
                {
                    OpGeral_ServerClick(sender, e);
                }
                else if (x == "Financeiro")
                {
                    OpFinanceiro_ServerClick(sender, e);
                }
                else if (x == "Comercial")
                {
                    OpComercial_ServerClick(sender, e);
                }
                else if (x == "Fiscal")
                {
                    OpFiscal_ServerClick(sender, e);
                }
                else if (x == "Historico")
                {
                    MostrarHistorico(Convert.ToInt64(Codigo), strNomePessoa);
                }
                else if (x == "Page")
                {
                }
                else
                {
                    btnSelecionar_Click(sender, e);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }


            
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["InscricaoPessoa"] = null;
            Session["EnderecoPessoa"] = null;
            Session["ContatoPessoa"] = null;
            Session["ZoomPessoa"] = null;
            Session["CadPessoa"] = null;
            Session["ListaRepresentantes"] = null;

            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx?cad=" + Request.QueryString["Cad"]);
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados

            grdGrid.DataBind(); 
            btnConsultar_Click(sender, e);
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {

            Session["Pagina"] = "/Pages/Welcome.aspx";
            PanelSelect = "home";
            Session["TabFocadaConPessoa"] = "home";
            Session["LST_CADPESSOA"] = null;
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
            if (Convert.ToInt32(Request.QueryString["Cad"]) == 4)
                Response.Redirect("~/Pages/Pessoas/CadComprador.aspx");
            else if(Convert.ToInt32(Request.QueryString["Cad"]) == 2)
                Response.Redirect("~/Pages/Financeiros/CadCtaCorrente.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 3)
                Response.Redirect("~/Pages/financeiros/ManCtaPagar.aspx");
            else if(Convert.ToInt32(Request.QueryString["Cad"]) == 1)
                Response.Redirect("~/Pages/Empresas/CadEmpresa.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 5)
                Response.Redirect("~/Pages/financeiros/ManCtaReceber.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 6)
                Response.Redirect("~/Pages/Servicos/ManNotaFiscalServico.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 7)
                Response.Redirect("~/Pages/Vendas/ManOrcamento.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 8)
                Response.Redirect("~/Pages/Usuarios/CadUsuario.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 9)
                Response.Redirect("~/Pages/Servicos/ManSolAtendimento.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 10)
                Response.Redirect("~/Pages/Servicos/ManOrdServico.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 11)
                Response.Redirect("~/Pages/Pessoas/CadVendedor.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) >= 12 && Convert.ToInt32(Request.QueryString["Cad"]) <=16)
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 17 )
                Response.Redirect("~/Pages/Vendas/ManOrcamento.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) >= 18 && Convert.ToInt32(Request.QueryString["Cad"]) <= 19)
                Response.Redirect("~/Pages/Vendas/ManPedido.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 20 )
                Response.Redirect("~/Pages/Vendas/RelContratos.aspx");
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 21)
                Response.Redirect("~/Pages/Compromissos/ManAgendamento.aspx");
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
        }
        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            PanelSelect = "home";
            Session["TabFocadaConPessoa"] = "home";
            Session["LST_CADPESSOA"] = null;
        }

        protected void OpGeral_ServerClick(object sender, EventArgs e)
        {
           
            Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
            
        }
        protected void OpFinanceiro_ServerClick(object sender, EventArgs e)
        {
            
            Response.Redirect("~/Pages/Pessoas/CadPesFinanceiro.aspx");

        }
        protected void OpComercial_ServerClick(object sender, EventArgs e)
        {
            
            Response.Redirect("~/Pages/Pessoas/CadPesComercial.aspx");

        }
        protected void OpFiscal_ServerClick(object sender, EventArgs e)
        {
            
            Response.Redirect("~/Pages/Pessoas/CadPesFiscal.aspx");
   
        }

        protected void btnSelecionar_Click(object sender, GridViewCommandEventArgs e)
        {
            PanelSelect = "consulta";

            string x = e.CommandName;

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = grdGrid.Rows[index];

            string Codigo = Server.HtmlDecode(row.Cells[0].Text);

            Session["ZoomPessoa"] = Codigo;

            UsuarioDAL u = new UsuarioDAL();

            Session["Usuario"] = u.PesquisarUsuarioPorCodPessoa(Convert.ToInt64(Codigo));

            if (Convert.ToInt32(Request.QueryString["Cad"]) == 1)
            {
                Session["IncEmpresa2"] = Server.HtmlDecode(row.Cells[0].Text) + "³" + Server.HtmlDecode(row.Cells[2].Text);
                Response.Redirect("~/Pages/Empresas/CadEmpresa.aspx");
            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 2)
            {
                Session["ContaCorrente2"] = Codigo + "³";
                Response.Redirect("~/Pages/Financeiros/CadCtaCorrente.aspx");
            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 3)
            {
                Session["Ssn_TipoPessoa2"] = Codigo + "³";
                Response.Redirect("~/Pages/Financeiros/ManCtaPagar.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 5)
            {
                Session["Ssn_CtaReceber2"] = Codigo + "³";
                Response.Redirect("~/Pages/Financeiros/ManCtaReceber.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 6)
            {
                Session["NotaFiscalServico2"] = Codigo + "³";
                Response.Redirect("~/Pages/Servicos/ManNotaFiscalServico.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 7)
            {
                Session["ZoomPessoaOrcamento"] = Codigo + "³";
                Response.Redirect("~/Pages/Vendas/ManOrcamento.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 8)
            {
                Session["CadUsuario2"] = Codigo + "³";
                Response.Redirect("~/Pages/Usuarios/CadUsuario.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 9)
            {
                Session["Doc_SolicitacaoAtendimento2"] = Codigo + "³";
                Response.Redirect("~/Pages/Servicos/ManSolAtendimento.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 10)
            {
                Session["Doc_OrdemServico2"] = Codigo + "³";
                Response.Redirect("~/Pages/Servicos/ManOrdServico.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 11)
            {
                Session["ZoomPessoa"] = Codigo;
                Response.Redirect("~/Pages/Pessoas/CadVendedor.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 12)
            {
                Session["ZoomTransportador"] = Codigo; 
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 13)
            {
                Session["ZoomRemetente"] = Codigo;
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 14)
            {
                Session["ZoomDestinatario"] = Codigo;
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 15)
            {
                Session["ZoomTomador"] = Codigo;
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 16)
            {
                Session["ZoomRecebedor"] = Codigo;
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 17)
            {
                Session["ZoomTranspOrcamento"] = Codigo;
                Response.Redirect("~/Pages/Vendas/ManOrcamento.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 18)
            {
                Session["ZoomTranspPedido"] = Codigo;
                Response.Redirect("~/Pages/Vendas/ManPedido.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 19)
            {
                Session["ZoomPessoaPedido"] = Codigo;
                Response.Redirect("~/Pages/Vendas/ManPedido.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 20)
            {
                Session["ZoomClienteRelContratos"] = Codigo;
                Response.Redirect("~/Pages/Vendas/RelContratos.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 21)
            {
                Session["ZoomPessoaAgendamento"] = Codigo;
                Response.Redirect("~/Pages/Compromissos/ManAgendamento.aspx");

            }
            else if (Convert.ToInt32(Request.QueryString["Cad"]) == 4)
            {
                List<Comprador> listCadComprador = new List<Comprador>();
                listCadComprador = (List<Comprador>)Session["IncCompradorPessoa"];
                listCadComprador[0].CodigoPessoa = Convert.ToInt64(Codigo);
                Session["IncCompradorPessoa"] = listCadComprador;
                Response.Redirect("~/Pages/Pessoas/CadComprador.aspx");
            }

            else
            {
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
            }
        }
        protected void grdGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
                

            string sortingDirection = string.Empty;
            if (dir == SortDirection.Ascending)
            {
                dir = SortDirection.Descending;
                sortingDirection = "Desc";
            }
            else
            {
                dir = SortDirection.Ascending;
                sortingDirection = "Asc";
            }

            List<Pessoa> ListOfInt = new List<Pessoa>();
            ListOfInt = (List<Pessoa>)grdGrid.DataSource;
            // populate list
            DataTable ListAsDataTable = BuildDataTable<Pessoa>(ListOfInt);
            DataView ListAsDataView = ListAsDataTable.DefaultView;

            ListAsDataView.Sort = e.SortExpression + " " + sortingDirection;
            grdGrid.DataSource = ListAsDataView;
            grdGrid.DataBind();

            for (int i = 0; i < grdGrid.Columns.Count; i++)
            {
                if (grdGrid.Columns[i].SortExpression == e.SortExpression)
                {
                    var cell = grdGrid.HeaderRow.Cells[i];
                    Image image = new Image();
                    image.Height = 15;
                    image.Width = 15;
                    Literal litespaco = new Literal();
                    litespaco.Text = "&emsp;";
                    cell.Controls.Add(litespaco);

                    if (sortingDirection == "Desc")
                        image.ImageUrl = "../../images/down_arrow.png";
                    else
                        image.ImageUrl = "../../images/up_arrow.png";
                    cell.Controls.Add(image);
                }

            }
        }
        public SortDirection dir
        {
            get {

                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }

                return (SortDirection) ViewState["dirState"];

            }

            set {
                ViewState["dirState"] = value;
            }
        }
        public static DataTable BuildDataTable<T>(IList<T> lst)
        {
            //create DataTable Structure
            DataTable tbl = CreateTable<T>();
            Type entType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            //get the list item and add into the list
            foreach (T item in lst)
            {
                DataRow row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                tbl.Rows.Add(row);
            }
            return tbl;
        }
        private static DataTable CreateTable<T>()
        {
            //T –> ClassName
            Type entType = typeof(T);
            //set the datatable name as class name
            DataTable tbl = new DataTable(entType.Name);
            //get the property list
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                //add property as column

                if ( prop != null)
                    
                {
                    tbl.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

            }
            return tbl;
        }
        protected void MostrarHistorico(Int64 CodigoPessoa, string strNomePessoa)
        {
            txtPesquisar.Text = "";
            litHistorico.Text = "";
            List<AgendamentoCompromisso> Lista = new List<AgendamentoCompromisso>();
            AgendamentoCompromissoDAL agendaDAL = new AgendamentoCompromissoDAL();
            Lista = agendaDAL.ListarAgendamentoCliente(Convert.ToInt32(Session["CodUsuario"].ToString()), CodigoPessoa,"");

            if (Lista.Count == 0)
            {
                ShowMessage("Nenhum histórico de conversa com " + strNomePessoa, MessageType.Info);
                return;
            }
            Lista = Lista.OrderByDescending(x => x.DataHoraAgendamento).ToList();

            litHistorico.Text += "<div class='timeline'>";

            foreach (var item in Lista)
            {
                litHistorico.Text += " <div class='timeline-row'  > " +
                                            "<div class='timeline-time'>" +
                                                item.DataHoraAgendamento.ToString("HH:mm") +
                                                "</br><span style='font-size:15px'>" + item.DataHoraAgendamento.ToString("dd/MM/yyyy") + "</span> " +
                                            "</div> " +
                                            "<div class='timeline-dot fb-bg' ></div> " +
                                            "<div class='timeline-content' style='background-color:" + item.CorLembrete + ";'>" +
                                                "<p class='style-content-h4'>" + item.Anotacao.Replace("\n", "</br>") + "<p> " +
                                                "<p class='style-content-p'>Local: " + item.Local + "!</br> " +
                                                "Contato: " + item.Telefone + " </br> " + item.Contato.Replace("\n", "</br>") + "<p> ";
                List<AnexoAgendamento> ListaAnexo2 = new List<AnexoAgendamento>();
                AnexoAgendamentoDAL anexoDAL = new AnexoAgendamentoDAL();
                ListaAnexo2 = anexoDAL.ObterAnexos(item.CodigoIndex);
                if (ListaAnexo2.Count > 0)
                {
                    litHistorico.Text += "<h4 class='style-content-h4' style='text-align:center'>Anexo(s)" +
                                            "<table class='table table-striped'>" +
                                                "<tr>" +
                                                    "<th>Código</th>" +
                                                    "<th>Data/Hora Lanc.</th>" +
                                                    "<th>Descrição</th>" +
                                                    "<th>Usuário</th>" +
                                                    "<th>Acessar</th>" +
                                                "</tr>";

                    foreach (var anexo in ListaAnexo2)
                    {
                        litHistorico.Text += "<tr style='background-color:white'>" +
                                                "<td>" + anexo.CodigoAnexo + "</td>" +
                                                "<td>" + anexo.DataHoraLancamento + "</td>" +
                                                "<td>" + anexo.DescricaoArquivo + "</td>" +
                                                "<td>" + anexo.Cpl_Usuario + "</td>" +
                                                "<td>"+
                                                    "<button onclick='FazerDownload(" + anexo.CodigoAgendamento + "," + anexo.CodigoAnexo + ")' class='btn btn-default' style='width:40px; height:25px;padding:0'>" +
                                                        "<span class='glyphicon glyphicon-edit ' style='width:17px; height:17px'/> " +
                                                    "</button>"+
                                                "</td>" +
                                            "</tr>";
                    }

                    litHistorico.Text += "</table><h4> ";
                }

                litHistorico.Text += "</div>" +
                                    "</div>";

            }
            litHistorico.Text += "</div>";
            txtPesquisar.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ModalHistorico('Histórico de conversas - " + strNomePessoa + "')", true);
            
        }
        protected void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            if (Session["ZoomPessoa"] != null)
            {
                litHistorico.Text = "";
                List<AgendamentoCompromisso> Lista = new List<AgendamentoCompromisso>();
                AgendamentoCompromissoDAL agendaDAL = new AgendamentoCompromissoDAL();

                if (txtPesquisar.Text.Trim().Length >= 3)
                    Lista = agendaDAL.ListarAgendamentoCliente(Convert.ToInt32(Session["CodUsuario"].ToString()), Convert.ToInt64(Session["ZoomPessoa"]), txtPesquisar.Text);
                else
                    Lista = agendaDAL.ListarAgendamentoCliente(Convert.ToInt32(Session["CodUsuario"].ToString()), Convert.ToInt64(Session["ZoomPessoa"]), "");


                if (Lista.Count == 0)
                {
                    litHistorico.Text = "<p style='text-align:center'>Nenhuma conversa encontrada</p>";
                    return;
                }

                Lista = Lista.OrderByDescending(x => x.DataHoraAgendamento).ToList();


                litHistorico.Text += "<div class='timeline'>";
                foreach (var item in Lista)
                {
                    litHistorico.Text += " <div class='timeline-row' > " +
                                                "<div class='timeline-time'>" +
                                                    item.DataHoraAgendamento.ToString("HH:mm") +
                                                    "</br><span style='font-size:15px'>" + item.DataHoraAgendamento.ToString("dd/MM/yyyy") + "</span> " +
                                                "</div> " +
                                                "<div class='timeline-dot fb-bg'></div> " +
                                                "<div class='timeline-content' style='background-color:" + item.CorLembrete + "' >" +
                                                    "<p class='style-content-h4'>" + item.Anotacao.Replace("\n", "</br>") + "<p> " +
                                                    "<p class='style-content-p'>Local: " + item.Local + "!</br> " +
                                                    "Contato: " + item.Telefone + " </br> " + item.Contato.Replace("\n", "</br>") + "<p> ";

                    List<AnexoAgendamento> ListaAnexo2 = new List<AnexoAgendamento>();
                    AnexoAgendamentoDAL anexoDAL = new AnexoAgendamentoDAL();
                    ListaAnexo2 = anexoDAL.ObterAnexos(item.CodigoIndex);
                    if (ListaAnexo2.Count > 0)
                    {
                        litHistorico.Text += "<h4 class='style-content-h4' style='text-align:center'>Anexo(s)" +
                                                "<table class='table table-striped'>" +
                                                    "<tr>" +
                                                        "<th>Código</th>" +
                                                        "<th>Data/Hora Lanc.</th>" +
                                                        "<th>Descrição</th>" +
                                                        "<th>Usuário</th>" +
                                                        "<th>Acessar</th>" +
                                                    "</tr>";

                        foreach (var anexo in ListaAnexo2)
                        {
                            litHistorico.Text += "<tr style='background-color:white'>" +
                                                    "<td>" + anexo.CodigoAnexo + "</td>" +
                                                    "<td>" + anexo.DataHoraLancamento + "</td>" +
                                                    "<td>" + anexo.DescricaoArquivo + "</td>" +
                                                    "<td>" + anexo.Cpl_Usuario + "</td>" +
                                                    "<td>" +
                                                        "<button onclick='FazerDownload(" + anexo.CodigoAgendamento + "," + anexo.CodigoAnexo + ")' class='btn btn-default' style='width:40px; height:25px;padding:0'>" +
                                                            "<span class='glyphicon glyphicon-edit ' style='width:17px; height:17px'/> " +
                                                        "</button>"+
                                                    "</td>" +
                                                "</tr>";
                        }

                        litHistorico.Text += "</table><h4> ";
                    }

                    litHistorico.Text += "</div>" +
                                        "</div>";

                }
                litHistorico.Text += "</div>";

            }
        }

        protected void btnImprimirRelatorioMatriculados_Click(object sender, EventArgs e)
        {
            try
            {
                PanelSelect = "consulta";
                AnexoDocumentoDAL ad = new AnexoDocumentoDAL();
                DataTable dt = new DataTable();
                PessoaDAL p = new PessoaDAL();

                string NomeArquivoCompactado = ad.GerarGUID("zip");
                string NomeArquivoExcel = "Informações dos Sócios.xlsx";
                string CaminhoPastaParaCompactar = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\ArquivosCompactados\\" + NomeArquivoCompactado.Replace(".zip", "") ;
                string CaminhoPastaDestino = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\ArquivosCompactados\\" + NomeArquivoCompactado;

                if (Directory.Exists(CaminhoPastaParaCompactar))
                    Directory.Delete(CaminhoPastaParaCompactar);

                Directory.CreateDirectory(CaminhoPastaParaCompactar);

                if (ddlGrupoPessoa.SelectedValue != "*Nenhum Selecionado")
                    dt = p.RelatorioMatriculados(Convert.ToInt32(ddlGrupoPessoa.SelectedValue));
                else
                    dt = p.RelatorioMatriculados(0);

                if (dt.Rows.Count == 0 )
                {
                    ShowMessage("Nenhum sócio existente", MessageType.Info);
                    return;
                }

                int LinhaExcel = 1;

                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Planilha 1");

                ws.Cell("A1").Value = "Matrícula";
                ws.Cell("B1").Value = "Nome";
                ws.Cell("C1").Value = "Tipo de Sócio";
                ws.Cell("D1").Value = "Data Filiação";
                ws.Cell("E1").Value = "Data Nascimento";
                            
                foreach (DataRow row in dt.Rows)
                {
                    string strMatricula = "";
                    byte[] byteConteudo = null;
                    LinhaExcel++;
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName == "CD_MATRICULA")
                        {
                            strMatricula = row[column].ToString();
                            ws.Cell("A" + LinhaExcel).Value = "'" + row[column].ToString();
                        }
                        else if (column.ColumnName == "NM_PESSOA")
                            ws.Cell("B" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "DS_GPO_PESSOA")
                            ws.Cell("C" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "DT_FILIACAO")
                            ws.Cell("D" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "DT_NASCIMENTO")
                            ws.Cell("E" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "IM_FOTO")
                            byteConteudo = (byte[])row[column];
                    }

                    if (strMatricula == "")
                        strMatricula = "000";

                    if (byteConteudo != null && byteConteudo.Length > 10)
                    {
                        string CaminhoFoto = CaminhoPastaParaCompactar + "\\" + strMatricula + ".jpg";
                        if (System.IO.File.Exists(CaminhoFoto))
                            System.IO.File.Delete(CaminhoFoto);

                        FileStream file = new FileStream(CaminhoFoto, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(file);
                        bw.Write(byteConteudo);
                        bw.Close();

                        file = new FileStream(CaminhoFoto, FileMode.Open);
                        BinaryReader br = new BinaryReader(file);
                        file.Close();
                    }
                }
                
                ws.Column("A").Width = 10;
                ws.Column("B").Width = 50;

                ws.Columns("C-E").Width = 18;
                ws.Range("A1:E1").Style.Fill.BackgroundColor = XLColor.SkyBlue;

                ws.Range("A1:E" + LinhaExcel).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:E" + LinhaExcel).Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("A1:E" + LinhaExcel).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:E" + LinhaExcel).Style.Border.InsideBorderColor = XLColor.Black;

                ws.Range("A1:E" + LinhaExcel).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("A1:E" + LinhaExcel).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                
                //salvar arquivo em disco
                wb.SaveAs(CaminhoPastaParaCompactar + @"\" + NomeArquivoExcel);
                //liberar objetos
                wb.Dispose();
                
                ZipFile.CreateFromDirectory(CaminhoPastaParaCompactar, CaminhoPastaDestino);
                
                Response.Clear();
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("content-disposition", "filename=" + NomeArquivoCompactado);
                Response.TransmitFile(CaminhoPastaDestino);               
                Response.End();
                
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnImprimirRelatorioRepresentantes_Click(object sender, EventArgs e)
        {
            try
            { 
                PanelSelect = "consulta";
                AnexoDocumentoDAL ad = new AnexoDocumentoDAL();
                DataTable dt = new DataTable();
                PessoaDAL p = new PessoaDAL();

                if (Session["LST_CADPESSOA"] != null)
                    listaT = (List<DBTabelaCampos>)Session["LST_CADPESSOA"];

                string NomeArquivoExcel = ad.GerarGUID("xlsx");
                string CaminhoPastaLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\";

                if (Directory.Exists(NomeArquivoExcel))
                    Directory.Delete(NomeArquivoExcel);

                dt = p.ListarPessoasComRepresentantes(listaT);

                if (dt.Rows.Count == 0)
                {
                    ShowMessage("Nenhuma pessoa com representante", MessageType.Info);
                    return;
                }

                int LinhaExcel = 1;

                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Planilha 1");

                ws.Cell("A1").Value = "Código";
                ws.Cell("B1").Value = "Nome";
                ws.Cell("C1").Value = "Inscrição";
                ws.Cell("D1").Value = "Bairro";
                ws.Cell("E1").Value = "Municipio";
                ws.Cell("F1").Value = "Estado";
                ws.Cell("G1").Value = "Pais";
                ws.Cell("H1").Value = "Nome Contato";
                ws.Cell("I1").Value = "Telefone Principal";
                ws.Cell("J1").Value = "Email Principal";
                ws.Cell("K1").Value = "Grupo da Pessoa";
                ws.Cell("L1").Value = "Representantes";
                ws.Range("L1:M1").Merge();

                foreach (DataRow row in dt.Rows)
                {
                    long CodigoPessoa = 0;
                    LinhaExcel++;
                    int LinhaExcelPessoa = LinhaExcel;
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName == "COD_PESSOA")
                        {
                            CodigoPessoa = Convert.ToInt64(row[column].ToString());
                            ws.Cell("A" + LinhaExcel).Value = "'" + row[column].ToString();
                        }
                        else if (column.ColumnName == "NOME_PESSOA")
                            ws.Cell("B" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "NR_INSCRICAO")
                            ws.Cell("C" + LinhaExcel).Value = "'"+row[column].ToString();
                        else if (column.ColumnName == "DS_BAIRRO")
                            ws.Cell("D" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "DS_MUNICIPIO")
                            ws.Cell("E" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "DS_ESTADO")
                            ws.Cell("F" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "DS_PAIS")
                            ws.Cell("G" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "NM_CONTATO")
                            ws.Cell("H" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "NR_FONE1")
                            ws.Cell("I" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "TX_MAIL1")
                            ws.Cell("J" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "DS_GPO_PESSOA")
                            ws.Cell("K" + LinhaExcel).Value = row[column].ToString();
                    }

                    List<Vendedor> listaRepresentantes = new List<Vendedor>();
                    PessoaDAL pDAL = new PessoaDAL();
                    listaRepresentantes = pDAL.ObterRepresentantes(CodigoPessoa);

                    foreach (var item in listaRepresentantes)
                    {
                        ws.Cell("L" + LinhaExcelPessoa).Value = item.CodigoVendedor;
                        ws.Cell("M" + LinhaExcelPessoa).Value = item.NomePessoa;
                        LinhaExcelPessoa++;
                    }
                    LinhaExcelPessoa--;

                    ws.Range("A" + LinhaExcel + ":A" + LinhaExcelPessoa).Merge();
                    ws.Range("B" + LinhaExcel + ":B" + LinhaExcelPessoa).Merge();
                    ws.Range("C" + LinhaExcel + ":C" + LinhaExcelPessoa).Merge();
                    ws.Range("D" + LinhaExcel + ":D" + LinhaExcelPessoa).Merge();
                    ws.Range("E" + LinhaExcel + ":E" + LinhaExcelPessoa).Merge();
                    ws.Range("F" + LinhaExcel + ":F" + LinhaExcelPessoa).Merge();
                    ws.Range("G" + LinhaExcel + ":G" + LinhaExcelPessoa).Merge();
                    ws.Range("H" + LinhaExcel + ":H" + LinhaExcelPessoa).Merge();
                    ws.Range("I" + LinhaExcel + ":I" + LinhaExcelPessoa).Merge();
                    ws.Range("J" + LinhaExcel + ":J" + LinhaExcelPessoa).Merge();
                    ws.Range("K" + LinhaExcel + ":K" + LinhaExcelPessoa).Merge();
                    LinhaExcel = LinhaExcelPessoa;
                }

                ws.Columns("C-M").Width = 18;
                ws.Column("A").Width = 7;
                ws.Column("B").Width = 50;
                ws.Column("F").Width = 7;
                ws.Column("D").Width = 10;
                ws.Column("L").Width = 5;
                ws.Column("M").Width = 40;

                ws.Range("A1:M1").Style.Fill.BackgroundColor = XLColor.SkyBlue;

                ws.Range("A1:M" + LinhaExcel).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:M" + LinhaExcel).Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("A1:M" + LinhaExcel).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:M" + LinhaExcel).Style.Border.InsideBorderColor = XLColor.Black;

                ws.Range("A1:M" + LinhaExcel).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("A1:M" + LinhaExcel).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                //salvar arquivo em disco
                wb.SaveAs(CaminhoPastaLog + @"\" + NomeArquivoExcel);
                //liberar objetos
                wb.Dispose();

                Response.Clear();
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("content-disposition", "filename=" + NomeArquivoExcel);
                Response.TransmitFile(CaminhoPastaLog + @"\" + NomeArquivoExcel);
                Response.End();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void txtDownloadAnexo_TextChanged(object sender, EventArgs e)
        {

            try
            {
                string strCodigoAgendamento = txtDownloadAnexo.Text.Split('³')[0];
                string strCodigoAnexo = txtDownloadAnexo.Text.Split('³')[1];

                Session["ZoomAnexoTimelineCliente2"] = strCodigoAgendamento + "³" + strCodigoAnexo;
                Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=14");

            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, MessageType.Error);
            }

        }
    }
}