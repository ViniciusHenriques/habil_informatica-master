using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Impostos
{
    public partial class ConRegFisIcms : System.Web.UI.Page
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
        protected void MontaFiltro(object sender, EventArgs e)
        {
            int intI = 1;
            int intJ = 1;
            int intZ = 1;
            DBTabelaDAL d = new DBTabelaDAL();
            List<DBTabela> Lista = new List<DBTabela>();
            Lista = d.ListarCamposView("VW_REG_FIS_ICMS");

            foreach (DBTabela Lst in Lista)
            {

                if ((Lst.Tipo.ToUpper() == "SMALLDATETIME") || (Lst.Tipo.ToUpper() == "DATETIME"))
                {
                    if (intJ == 1)
                    {
                        txtfiltrodata11.Focus();
                        pnlFiltroData1.Visible = true;
                        lblFiltroData1.Text = Lst.NomeComum.ToString();
                        lblFiltroData1.ToolTip = Lst.Coluna.ToString();
                        txtfiltrodata11.Text = "";
                        txtfiltrodata12.Text = "";
                        txtfiltrodata11.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtfiltrodata12.MaxLength = Convert.ToInt32(Lst.Tamanho);

                    }

                    if (intJ == 2)
                    {
                        pnlFiltroData2.Visible = true;
                        lblFiltroData2.Text = Lst.NomeComum.ToString();
                        lblFiltroData2.ToolTip = Lst.Coluna.ToString();
                        txtfiltrodata21.Text = "";
                        txtfiltrodata22.Text = "";
                        txtfiltrodata21.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtfiltrodata22.MaxLength = Convert.ToInt32(Lst.Tamanho);

                    }

                    if (intJ == 3)
                    {
                        pnlFiltroData3.Visible = true;
                        lblFiltroData3.Text = Lst.NomeComum.ToString();
                        lblFiltroData3.ToolTip = Lst.Coluna.ToString();
                        txtfiltrodata31.Text = "";
                        txtfiltrodata32.Text = "";
                        txtfiltrodata31.MaxLength = Convert.ToInt32(Lst.Tamanho);
                        txtfiltrodata32.MaxLength = Convert.ToInt32(Lst.Tamanho);

                    }


                    intJ++;
                }
                else
                {
                    if (Lst.RegistroUnico == 0)
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
                    else //Registro Unico Sim
                    {
                        if (intZ == 1)
                        {
                            pnlFiltroDrop1.Visible = true;
                            lblFiltroDrop1.Text = Lst.NomeComum.ToString();
                            lblFiltroDrop1.ToolTip = Lst.Coluna.ToString();
                            ddlFiltroDrop1.DataSource = d.PopularTabela(Lst.PopulaTabela);
                            ddlFiltroDrop1.DataTextField = "DescricaoDrop";
                            ddlFiltroDrop1.DataValueField = "CodigoDrop";
                            ddlFiltroDrop1.DataBind();
                            ddlFiltroDrop1.Items.Insert(0, "*Nenhum Selecionado");
                        }
                        if (intZ == 2)
                        {
                            pnlFiltroDrop2.Visible = true;
                            lblFiltroDrop2.Text = Lst.NomeComum.ToString();
                            lblFiltroDrop2.ToolTip = Lst.Coluna.ToString();
                            ddlFiltroDrop2.DataSource = d.PopularTabela(Lst.PopulaTabela);
                            ddlFiltroDrop2.DataTextField = "DescricaoDrop";
                            ddlFiltroDrop2.DataValueField = "CodigoDrop";
                            ddlFiltroDrop2.DataBind();
                            ddlFiltroDrop2.Items.Insert(0, "*Nenhum Selecionado");
                        }
                        if (intZ == 3)
                        {
                            pnlFiltroDrop3.Visible = true;
                            lblFiltroDrop3.Text = Lst.NomeComum.ToString();
                            lblFiltroDrop3.ToolTip = Lst.Coluna.ToString();
                            ddlFiltroDrop3.DataSource = d.PopularTabela(Lst.PopulaTabela);
                            ddlFiltroDrop3.DataTextField = "DescricaoDrop";
                            ddlFiltroDrop3.DataValueField = "CodigoDrop";
                            ddlFiltroDrop3.DataBind();
                            ddlFiltroDrop3.Items.Insert(0, "*Nenhum Selecionado");
                        }
                        if (intZ == 4)
                        {
                            pnlFiltroDrop4.Visible = true;
                            lblFiltroDrop4.Text = Lst.NomeComum.ToString();
                            lblFiltroDrop4.ToolTip = Lst.Coluna.ToString();
                            ddlFiltroDrop4.DataSource = d.PopularTabela(Lst.PopulaTabela);
                            ddlFiltroDrop4.DataTextField = "DescricaoDrop";
                            ddlFiltroDrop4.DataValueField = "CodigoDrop";
                            ddlFiltroDrop4.DataBind();
                            ddlFiltroDrop4.Items.Insert(0, "*Nenhum Selecionado");
                        }
                        if (intZ == 5)
                        {
                            pnlFiltroDrop5.Visible = true;
                            lblFiltroDrop5.Text = Lst.NomeComum.ToString();
                            lblFiltroDrop5.ToolTip = Lst.Coluna.ToString();
                            ddlFiltroDrop5.DataSource = d.PopularTabela(Lst.PopulaTabela);
                            ddlFiltroDrop5.DataTextField = "DescricaoDrop";
                            ddlFiltroDrop5.DataValueField = "CodigoDrop";
                            ddlFiltroDrop5.DataBind();
                            ddlFiltroDrop5.Items.Insert(0, "*Nenhum Selecionado");
                        }
                        if (intZ == 6)
                        {
                            pnlFiltroDrop6.Visible = true;
                            lblFiltroDrop6.Text = Lst.NomeComum.ToString();
                            lblFiltroDrop6.ToolTip = Lst.Coluna.ToString();
                            ddlFiltroDrop6.DataSource = d.PopularTabela(Lst.PopulaTabela);
                            ddlFiltroDrop6.DataTextField = "DescricaoDrop";
                            ddlFiltroDrop6.DataValueField = "CodigoDrop";
                            ddlFiltroDrop6.DataBind();
                            ddlFiltroDrop6.Items.Insert(0, "*Nenhum Selecionado");
                        }

                        intZ++;
                    }

                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocadaRegFisIcms"] != null)
            {
                PanelSelect = Session["TabFocadaRegFisIcms"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaRegFisIcms"] = "home";
                txtFiltro11.Focus();
            }

            if (Session["LST_REGFISICMS"] != null)
            {
                listaT = (List<DBTabelaCampos>)Session["LST_REGFISICMS"];
                if (listaT.Count != 0)
                    btnConsultar_Click(sender, e);
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
                btnNovo.Visible = false;
                btnVoltar.Visible = true;
            }
            else
            {
                btnVoltar.Visible = false;
                pnlPainel.Visible = true;
                cmdSair.Visible = true;
                btnSair.Visible = true;
                btnNovo.Visible = true;
            }

            if (Session["ZoomAcesso2"] != null)
            {
                if (Session["ZoomAcesso2"].ToString() == "RELACIONAL")
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

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) 
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                MontaFiltro(sender, e);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "RegFisIcms.aspx");
                lista.ForEach(delegate(Permissao x)
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

                        if (!x.AcessoRelatorio)
                            btnImprimir.Visible = false;

                    }
                });

                if (Session["ZoomAcesso2"] != null)
                {
                    //btnImprimir.Visible = false;
                    btnSair.Visible = false;
                    btnVoltar.Visible = true;
                }
                else
                    btnVoltar.Visible = false;

            }
            //if (!lblFiltro1.Visible)
            //{
            //    Response.Redirect("~/Pages/Welcome.aspx");
            //}
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {

            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = "";
            Boolean blnCampoValido = false;

            if (pnlFiltroDrop1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroDrop1.ToolTip).ToString().ToUpper();

                if ((ddlFiltroDrop1.Text != "") && (ddlFiltroDrop1.Text != "*Nenhum Selecionado"))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroDrop1.ToolTip.ToString();
                    rowp.Inicio = ddlFiltroDrop1.SelectedItem.Text;
                    rowp.Fim = "";
                    rowp.Tipo = strTipo;
                    rowp.SemLike = true;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltroDrop2.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroDrop2.ToolTip).ToString().ToUpper();

                if ((ddlFiltroDrop2.Text != "") && (ddlFiltroDrop2.Text != "*Nenhum Selecionado"))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroDrop2.ToolTip.ToString();
                    rowp.Inicio = ddlFiltroDrop2.SelectedItem.Text;
                    rowp.Fim = "";
                    rowp.Tipo = strTipo;
                    rowp.SemLike = true;
                    listaT.Add(rowp);
                }
            }
            if (pnlFiltroDrop3.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroDrop3.ToolTip).ToString().ToUpper();

                if ((ddlFiltroDrop3.Text != "") && (ddlFiltroDrop3.Text != "*Nenhum Selecionado"))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroDrop3.ToolTip.ToString();
                    rowp.Inicio = ddlFiltroDrop3.SelectedItem.Text;
                    rowp.Fim = "";
                    rowp.Tipo = strTipo;
                    rowp.SemLike = true;
                    listaT.Add(rowp);
                }
            }
            if (pnlFiltroDrop4.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroDrop4.ToolTip).ToString().ToUpper();

                if ((ddlFiltroDrop4.Text != "") && (ddlFiltroDrop4.Text != "*Nenhum Selecionado"))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroDrop4.ToolTip.ToString();
                    rowp.Inicio = ddlFiltroDrop4.SelectedItem.Text;
                    rowp.Fim = "";
                    rowp.Tipo = strTipo;
                    rowp.SemLike = true;
                    listaT.Add(rowp);
                }
            }
            if (pnlFiltroDrop5.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroDrop5.ToolTip).ToString().ToUpper();

                if ((ddlFiltroDrop5.Text != "") && (ddlFiltroDrop5.Text != "*Nenhum Selecionado"))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroDrop5.ToolTip.ToString();
                    rowp.Inicio = ddlFiltroDrop5.SelectedItem.Text;
                    rowp.Fim = "";
                    rowp.Tipo = strTipo;
                    rowp.SemLike = true;
                    listaT.Add(rowp);
                }
            }
            if (pnlFiltroDrop6.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroDrop6.ToolTip).ToString().ToUpper();

                if ((ddlFiltroDrop6.Text != "") && (ddlFiltroDrop6.Text != "*Nenhum Selecionado"))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroDrop6.ToolTip.ToString();
                    rowp.Inicio = ddlFiltroDrop6.SelectedItem.Text;
                    rowp.Fim = "";
                    rowp.Tipo = strTipo;
                    rowp.SemLike = true;
                    listaT.Add(rowp);
                }
            }


            if (pnlFiltroData1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroData1.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltroData1.Text, txtfiltrodata11.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata11.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltroData1.Text, txtfiltrodata12.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata12.Focus();
                        return;
                    }

                if ((txtfiltrodata11.Text != "") || (txtfiltrodata12.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroData1.ToolTip.ToString();
                    rowp.Inicio = txtfiltrodata11.Text;
                    rowp.Fim = txtfiltrodata12.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }
            if (pnlFiltroData2.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroData2.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltroData2.Text, txtfiltrodata21.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata21.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltroData2.Text, txtfiltrodata22.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata22.Focus();
                        return;
                    }

                if ((txtfiltrodata21.Text != "") || (txtfiltrodata22.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroData2.ToolTip.ToString();
                    rowp.Inicio = txtfiltrodata21.Text;
                    rowp.Fim = txtfiltrodata22.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }
            if (pnlFiltroData3.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltroData3.ToolTip).ToString().ToUpper();

                v.CampoValido(lblFiltroData3.Text, txtfiltrodata31.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata31.Focus();
                        return;
                    }
                }

                v.CampoValido(lblFiltroData3.Text, txtfiltrodata32.Text, false, false, false, false, strTipo, ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtfiltrodata32.Focus();
                        return;
                    }

                if ((txtfiltrodata31.Text != "") || (txtfiltrodata32.Text != ""))
                {
                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltroData3.ToolTip.ToString();
                    rowp.Inicio = txtfiltrodata31.Text;
                    rowp.Fim = txtfiltrodata32.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }

            if (pnlFiltro1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro1.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro2.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro3.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro4.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro5.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro6.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro7.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro8.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro9.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_REG_FIS_ICMS", lblFiltro10.ToolTip).ToString().ToUpper();

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

            PanelSelect = "consulta";

            Session["LST_REGFISICMS"] = listaT;
            RegFisIcmsDAL r = new RegFisIcmsDAL();
            grdGrid.DataSource = r.ListarRegFisIcmsCompleto(listaT, Convert.ToInt16 (ddlRegistros.SelectedValue));
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Regras de Icms não encontrada(s) mediante a pesquisa realizada.", MessageType.Info);
                Session["LST_REGFISICMS"] = null;
            }
        }
        protected void grdGrid_SelectedIndexChanged(object sender, GridViewCommandEventArgs e)
        {

            string x = e.CommandName ;

            if (x == "Page")
            {
                return;
            }

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = grdGrid.Rows[index];
            string Codigo = Server.HtmlDecode(row.Cells[0].Text);
            Session["ZoomRegFisIcms"] = Codigo + "³" ;

            Response.Redirect("~/Pages/Impostos/ManRegFisIcms.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomRegFisIcms"] = null;
            Session["CadRegFisIcms"] = null;
            Response.Redirect("~/Pages/Impostos/ManRegFisIcms.aspx");
        }
        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
         //   grdGrid.PageSize = Convert.ToInt32(ddlRegistros.SelectedValue.ToString());
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
            Session["LST_REGFISICMS"] = null;
            PanelSelect = "home";
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Impostos/RelRegFisIcms.aspx");
        }

        protected void grdGrid_Sorting(object sender, GridViewSortEventArgs e)
        {

        }
    }
}