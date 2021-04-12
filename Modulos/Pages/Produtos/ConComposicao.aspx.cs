using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class ConComposicao : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaFiltro(object sender, EventArgs e)
        {
            PanelSelect = "home";
            int intI = 1;
            DBTabelaDAL d = new DBTabelaDAL();
            List<DBTabela> Lista = new List<DBTabela>();
            Lista = d.ListarCamposView("VW_COMPOSICAO");

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
                intI++;
            }
            PanelSelect = "home";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocada"] != null)
                PanelSelect = Session["TabFocada"].ToString();
            else
                if (!IsPostBack)
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

            txtFiltro11.Focus();



            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }



            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                MontaFiltro(sender, e);

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConComposicao.aspx");
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

                if (Session["ZoomComposicao2"] != null)
                {
                    btnSair.Visible = false;
                }
            }
            if (!lblFiltro1.Visible)
            {
                Response.Redirect("~/Pages/Welcome.aspx");
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            string Descricao = "";
            btnImprimir.Visible = true;
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = "";
            Boolean blnCampoValido = false;
            List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
            if (pnlFiltroData1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltroData1.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltroData1.Text + " De:" + txtfiltrodata11.Text;
                    if (txtfiltrodata12.Text != "")
                        Descricao += " Até:" + txtfiltrodata12.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltroData2.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltroData2.Text + " De:" + txtfiltrodata21.Text;
                    if (txtfiltrodata12.Text != "")
                        Descricao += " Até:" + txtfiltrodata22.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltroData3.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltroData3.Text + " De:" + txtfiltrodata31.Text;
                    if (txtfiltrodata12.Text != "")
                        Descricao += " Até:" + txtfiltrodata32.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro1.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro1.Text + " De:" + txtFiltro11.Text;
                    if (txtFiltro12.Text != "")
                        Descricao += " Até:" + txtFiltro12.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro2.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro2.Text + " De:" + txtFiltro21.Text;
                    if (txtFiltro22.Text != "")
                        Descricao += " Até:" + txtFiltro22.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro3.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro3.Text + " De:" + txtFiltro31.Text;
                    if (txtFiltro32.Text != "")
                        Descricao += " Até:" + txtFiltro32.Text;


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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro4.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro4.Text + " De:" + txtFiltro41.Text;
                    if (txtFiltro42.Text != "")
                        Descricao += " Até:" + txtFiltro42.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro5.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro5.Text + " De:" + txtFiltro51.Text;
                    if (txtFiltro52.Text != "")
                        Descricao += " Até:" + txtFiltro52.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro6.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro6.Text + " De:" + txtFiltro61.Text;
                    if (txtFiltro62.Text != "")
                        Descricao += " Até:" + txtFiltro62.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro7.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro7.Text + " De:" + txtFiltro71.Text;
                    if (txtFiltro72.Text != "")
                        Descricao += " Até:" + txtFiltro72.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro8.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro8.Text + " De:" + txtFiltro81.Text;
                    if (txtFiltro82.Text != "")
                        Descricao += " Até:" + txtFiltro82.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro9.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro9.Text + " De:" + txtFiltro91.Text;
                    if (txtFiltro92.Text != "")
                        Descricao += " Até:" + txtFiltro92.Text;

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
                strTipo = d.ListarTipoCampoView("VW_COMPOSICAO", lblFiltro10.ToolTip).ToString().ToUpper();

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
                    Descricao += " " + lblFiltro10.Text + " De:" + txtFiltro101.Text;
                    if (txtFiltro62.Text != "")
                        Descricao += " Até:" + txtFiltro62.Text;

                    DBTabelaCampos rowp = new DBTabelaCampos();
                    rowp.Filtro = lblFiltro10.ToolTip.ToString();
                    rowp.Inicio = txtFiltro101.Text;
                    rowp.Fim = txtFiltro102.Text;
                    rowp.Tipo = strTipo;
                    listaT.Add(rowp);
                }
            }
            PanelSelect = "consulta";
            ComposicaoDAL r = new ComposicaoDAL();
            List<Composicao> ListaComposicao = new List<Composicao>();
            //List<Composicao> Lista2 = new List<Composicao>();

            Session["DescricaoComposicao"] = Descricao;

            ListaComposicao = r.ListarComposicao(listaT);

            var  ListaFinal = (from t in ListaComposicao
                group t by new { t.CodigoProdutoComposto, t.DescricaoProduto, t.DescricaoTipo, t.DescricaoSituacao, t.Data, t.ValorCustoProduto}
                                into grp
            select new
            {
                grp.Key.CodigoProdutoComposto,
                grp.Key.DescricaoProduto,
                grp.Key.DescricaoTipo,
                grp.Key.DescricaoSituacao,
                grp.Key.Data,
                grp.Key.ValorCustoProduto
            }).ToList();

            Session["ListComposicao"] = listaT;

            grdGrid.DataSource = ListaFinal;
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
                ShowMessage("Composição(ções) não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
            PanelSelect = "consulta";
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomComposicao"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[0].Text) + "³";
            Response.Redirect("~/Pages/Produtos/CadComposicao.aspx");
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            btnImprimir.Visible = false;
            Session["ZoomComposicao"] = null;
            Response.Redirect("~/Pages/Produtos/CadComposicao.aspx");
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
            grdGrid.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            btnImprimir.Visible = false;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            btnImprimir.Visible = false;
            PanelSelect = "home";
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            btnImprimir.Visible = false;
            Response.Redirect("~/Pages/Produtos/RelComposicao.aspx");
        }
    }
}