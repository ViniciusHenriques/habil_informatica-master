using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using DAL;


using System.Linq;


namespace SoftHabilInformatica.Pages.HabilUtilitarios
{
    public partial class ConGerEmails : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        clsValidacao v1 = new clsValidacao();
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaFiltro(object sender, EventArgs e)
        {
            int intI = 1;
            int intJ = 1;
            DBTabelaDAL d = new DBTabelaDAL();
            List<DBTabela> Lista = new List<DBTabela>();
            Lista = d.ListarCamposView("VW_EMAILS");

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


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;


            if (Session["TabFocadaConNFSe"] != null)
            {
                PanelSelect = Session["TabFocadaConNFSe"].ToString();
                Session["TabFocadaConNFSe"] = null;
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaConNFSe"] = "home";
                txtFiltro11.Focus();
            }


            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }


            Session["RascunhoEmail"] = null;

            if (Session["LST_EMAILS"] != null)
            {
                listaT = (List<DBTabelaCampos>)Session["LST_EMAILS"];
                
                btnConsultar_Click(sender, e);
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                Habil_TipoDAL sd = new Habil_TipoDAL();
                ddlSituacao.DataSource = sd.SituacaoGeradorEmail();
                ddlSituacao.DataTextField = "DescricaoTipo";
                ddlSituacao.DataValueField = "CodigoTipo";
                ddlSituacao.DataBind();
                ddlSituacao.Items.Insert(0, "TODOS");

                MontaFiltro(sender, e);


                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConGerEmails.aspx");
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

                if (Session["ZoomGeracaiDosEmails"] != null)
                {
                    btnSair.Visible = false;
                }
            }
            if (!lblFiltro1.Visible)
            {
               // Response.Redirect("~/Pages/Welcome.aspx");
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = "";
            Boolean blnCampoValido = false;

            if (pnlFiltroData1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltroData1.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltroData2.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltroData3.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro1.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro2.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro3.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro4.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro5.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro6.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro7.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro8.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro9.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_EMAILS", lblFiltro10.ToolTip).ToString().ToUpper();

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

            DBTabelaCampos rowp21 = new DBTabelaCampos();
            rowp21.Filtro = "CD_USU_REMETENTE";
            rowp21.Inicio = Session["CodUsuario"].ToString();
            rowp21.Fim = rowp21.Inicio;
            rowp21.Tipo = "INT";
            listaT.Add(rowp21);

            if (ddlSituacao.Text != "TODOS")
            {
                rowp21 = new DBTabelaCampos();
                rowp21.Filtro = "CD_SITUACAO";
                rowp21.Inicio = ddlSituacao.SelectedValue;
                rowp21.Fim = ddlSituacao.SelectedValue;
                rowp21.Tipo = "INT";
                listaT.Add(rowp21);
            }


            PanelSelect = "consulta";

            Session["LST_EMAILS"] = listaT;
            HabilEmailCriadoDAL HeEmailDal = new HabilEmailCriadoDAL();
            List<HabilEmailCriado> listaNova = new List<HabilEmailCriado>();
            listaNova = HeEmailDal.ListarEmails(listaT);
            listaNova = listaNova.OrderByDescending(x => x.CD_INDEX).ToList();
            //listaNova.Sort(delegate (HabilEmailCriado p1, HabilEmailCriado p2)
            //{
            //    return p2.CD_INDEX.CompareTo(p1.CD_INDEX);
            //});

            grdGrid.DataSource = listaNova;
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Email(s) não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
                Session["LST_EMAILS"] = null;
            }
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/HabilUtilitarios/ManGerEmails.aspx");
        }
        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  grdGrid.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["LST_EMAILS"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            PanelSelect = "home";
            Session["LST_EMAILS"] = null;
        }

        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                           Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                           "ConGerEmails.aspx");


            string Codigo = Server.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text);
            Session["ZoomGeracaiDosEmails"] = Codigo;





            Response.Redirect("~/Pages/HabilUtilitarios/ManGerEmails.aspx");

        }




        //private void byteArrayToAnexo(byte[] strConteudo)
        //{

        //    if (System.IO.File.Exists("C:\\sql\\" + txtGUID.Text))
        //        System.IO.File.Delete("C:\\sql\\" + txtGUID.Text);


        //    FileStream file = new FileStream("C:\\sql\\" + txtGUID.Text, FileMode.Create);
        //    BinaryWriter bw = new BinaryWriter(file);
        //    bw.Write(strConteudo);
        //    bw.Close();

        //    file = new FileStream("C:\\sql\\" + txtGUID.Text, FileMode.Open);
        //    BinaryReader br = new BinaryReader(file);
        //    file.Close();

        //    Response.Clear();
        //    Response.ContentType = "application/octect-stream";
        //    Response.AppendHeader("content-disposition", "filename=" + txtGUID.Text);
        //    Response.TransmitFile("C:\\sql\\" + txtGUID.Text);
        //    Response.End();


        //    //            System.Net.WebClient client = new System.Net.WebClient();
        //    //            client.
        //    //            client.DownloadFile("http://localhost:59900/arquivo6.pdf", @"C:\sql\arquivo6.pdf");
        //}
    }
}