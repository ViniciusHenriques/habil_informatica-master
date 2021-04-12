using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using NFSeX;
using NFSeConverterX;
using System.IO;
using System.Windows.Forms;
namespace SoftHabilInformatica.Pages.Servicos
{
    public partial class ConNotaFiscalServico : System.Web.UI.Page
    {
        NFSeX.spdNFSeX _spdNFSeX = new NFSeX.spdNFSeX();

        NFSeDataSetX.spdNFSeDataSetX _spdNFSeDataSetX = new NFSeDataSetX.spdNFSeDataSetX();

        NFSeX.spdProxyNFSeX ProxyNFSe = new NFSeX.spdProxyNFSeX();

        spdNFSeConverterX _spdNFSeConverterX = new spdNFSeConverterX();

        public string PanelSelect { get; set; }

        clsValidacao v = new clsValidacao();

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
            Lista = d.ListarCamposView("VW_DOC_NFS");

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


            IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
            grdRejeicoes.DataSource = integraDAL.ListarIntegraDocEletronicoRejeitados(0, 1, Convert.ToInt32(Session["CodUsuario"]));
            grdRejeicoes.DataBind();


            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["LST_NOTAFISCALSERVICO"] != null)
            {
                listaT = (List<DBTabelaCampos>)Session["LST_NOTAFISCALSERVICO"];
                
                btnConsultar_Click(sender, e);
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                MontaFiltro(sender, e);


                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConNotaFiscalServico.aspx");
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

                if (Session["ZoomNotaFiscalServico"] != null)
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
            DBTabelaDAL d = new DBTabelaDAL();
            String strTipo = "";
            Boolean blnCampoValido = false;

            if (pnlFiltroData1.Visible)
            {
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltroData1.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltroData2.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltroData3.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro1.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro2.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro3.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro4.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro5.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro6.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro7.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro8.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro9.ToolTip).ToString().ToUpper();

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
                strTipo = d.ListarTipoCampoView("VW_DOC_NFS", lblFiltro10.ToolTip).ToString().ToUpper();

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

            Session["LST_NOTAFISCALSERVICO"] = listaT;
            Doc_NotaFiscalServicoDAL r = new Doc_NotaFiscalServicoDAL();
            int CodSitucao = 0;

            if (Convert.ToInt32(ddlSituacao.SelectedValue) == 1)
            {
                CodSitucao = 42;
            }
            else if (Convert.ToInt32(ddlSituacao.SelectedValue) == 2)
            {
                CodSitucao = 40;
            }
            else if (Convert.ToInt32(ddlSituacao.SelectedValue) == 3)
            {
                CodSitucao = 41;
            }
            else if (Convert.ToInt32(ddlSituacao.SelectedValue) == 4)
            {
                CodSitucao = 39;
            }
            listaT.RemoveAll(x => x.Filtro == "CD_SITUACAO");
            if (CodSitucao != 0)
            {

                DBTabelaCampos rowp2 = new DBTabelaCampos();
                rowp2.Filtro = "CD_SITUACAO";
                rowp2.Inicio = CodSitucao.ToString();
                rowp2.Fim = CodSitucao.ToString();
                rowp2.Tipo = "INT";
                listaT.Add(rowp2);
            }

            grdGrid.DataSource = r.ListarNotaFiscalServicoCompleto(listaT);
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Notas(s) Fiscais de Serviço não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
                Session["LST_NOTAFISCALSERVICO"] = null;
            }
        }

        protected void grdGrid_SelectedIndexChanged(object sender, GridViewCommandEventArgs e)
        {
            PanelSelect = "consulta";
            bool permissaoDownload = true;
            bool permissaoImprimir = true;
            bool permissaoEditar = true;
            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                           Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                           "ConNotaFiscalServico.aspx");
            lista.ForEach(delegate (Permissao x1)
            {
                if (!x1.AcessoCompleto)
                {
                    if (!x1.AcessoDownload)
                        permissaoDownload = false;
                    if (!x1.AcessoImprimir)
                        permissaoImprimir = false;
                    if (!x1.AcessoConsulta)
                        permissaoEditar = false;
                }
            });
            string x = e.CommandName;

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = grdGrid.Rows[index];
            string Codigo = Server.HtmlDecode(row.Cells[0].Text);
            Session["ZoomNotaFiscalServico"] = Codigo;

            if (x == "Editar")
            {
                if (permissaoEditar == false)
                {
                    ShowMessage("Perfil não tem permissão de consulta", MessageType.Info);
                }
                else
                {
                    Response.Redirect("~/Pages/Servicos/ManNotaFiscalServico.aspx");
                }
            }
            else if(x == "Duplicar")
            {
                OpDuplicar_ServerClick(sender, e);
            }
            else if (x == "ConsultarNFSe")
            {
                OpConsultar_ServerClick(sender, e);
            }
            else if (x == "AutorizarNFSe")
            {
                OpAutorizar_ServerClick(sender, e);
            }
            else if (x == "CancelarNFSe")
            {
                OpCancelar_ServerClick(sender, e);
            }
            else if (x == "ImprimirNFSe")
            {
                if (permissaoImprimir == false)
                {
                    ShowMessage("Perfil não tem permissão para imprimir", MessageType.Info);
                }
                else
                {
                    OpImprimir_ServerClick(sender, e);
                }
            }
            else if (x == "DownloadNFSe")
            {
                if(permissaoDownload == false)
                {
                    ShowMessage("Perfil não tem permissão de download", MessageType.Info);
                }
                else
                {
                    OpDownNFSe_ServerClick(sender, e);
                }
                
            }
            else if (x == "DownloadXML")
            {
                if (permissaoDownload == false)
                {
                    ShowMessage("Perfil não tem permissão de download", MessageType.Info);
                }
                else
                {
                    OpDownXML_ServerClick(sender, e);
                }
            }
            else if (x == "EnviarPorEmail")
            {
                OpEmail_ServerClick(sender, e);
            }

        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["Eventos"] = null;
            Session["Logs"] = null;
            Session["NovoAnexo"] = null;
            Session["ManutencaoProduto"] = null;
            Session["NotaFiscalServico"] = null;
            Session["ZoomNotaFiscalServico"] = null;
            Session["TabFocada"] = null;
            Session["ListaParcelaDocumento"] = null;
            Response.Redirect("~/Pages/Servicos/ManNotaFiscalServico.aspx");
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
            Session["LST_NOTAFISCALSERVICO"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }

        protected void btnVoltarSelecao_Click(object sender, EventArgs e)
        {
            PanelSelect = "home";
            Session["LST_NOTAFISCALSERVICO"] = null;
        }

        protected void OpDuplicar_ServerClick(object sender, EventArgs e)
        {
            decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);
            Doc_NotaFiscalServico doc = new Doc_NotaFiscalServico();
            Doc_NotaFiscalServicoDAL docDAL = new Doc_NotaFiscalServicoDAL();

            List<AnexoDocumento> ListaAnexos = new List<AnexoDocumento>();
            List<TipoServico> ListaServicos = new List<TipoServico>();
            List<ItemTipoServico> ListaItemServico = new List<ItemTipoServico>();
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();
            ParcelaDocumentoDAL parDAL = new ParcelaDocumentoDAL();
            doc = docDAL.PesquisarNotaFiscalServico(codigo);
            ListaServicos = docDAL.ObterTipoServico(codigo);
            ListaItemServico = docDAL.ObterProdutoDocumento(codigo);
            ListaParcelaDocumento = parDAL.ObterParcelaDocumento(doc.CodigoNotaFiscalServico);

            GeradorSequencialDocumentoEmpresa gerador = new GeradorSequencialDocumentoEmpresa();
            GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
            gerador = geradorDAL.PesquisarGeradorSequencialEmpresa(Convert.ToInt32(doc.CodigoPrestador), 1);

            DBTabelaDAL db = new DBTabelaDAL();
            GeracaoSequencialDocumento gerDoc = new GeracaoSequencialDocumento();
            GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
            gerDoc = gerDocDAL.PesquisarGeradorSequencial(gerador.CodigoGeradorSequencialDocumento);
            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            doc.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
            doc.Cpl_Usuario = Convert.ToInt32(Session["CodUsuario"]);

            DBTabelaDAL RnTab = new DBTabelaDAL();
            doc.DataEmissao = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"));
            doc.DataLancamento= Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"));

            // Se existe a tabela sequencial
            if (db.BuscaTabelas(gerador.Cpl_Nome) == gerador.Cpl_Nome)
            {
                if (gerDoc.Nome == "" || gerDoc.CodigoSituacao == 2)
                {
                    ShowMessage("Gerador Sequencial não iniciado", MessageType.Warning);
                    return;
                }
                else
                {
                    if (gerDoc.Validade < DateTime.Now)
                    {
                        ShowMessage("Gerador Sequencial venceu em " + gerDoc.Validade.ToString("dd/MM/yyyy"), MessageType.Warning);
                        return;
                    }
                }
                double NroSequencial = geradorDAL.ExibeProximoNroSequencial(gerador.Cpl_Nome);
                if (NroSequencial == 0)
                    doc.NumeroDocumento = Convert.ToInt32(gerDoc.NumeroInicial.ToString());
                else
                    doc.NumeroDocumento = Convert.ToInt32(NroSequencial.ToString());

                doc.DGSerieDocumento = gerDoc.SerieConteudo.ToString();
                doc.Cpl_NomeTabela = gerador.Cpl_Nome;
            }
            else
            {

                doc.NumeroDocumento = Convert.ToInt32(gerDoc.NumeroInicial.ToString());
                doc.DGSerieDocumento = gerDoc.SerieConteudo;
            }
            List<ParcelaDocumento> ListaParcela2 = new List<ParcelaDocumento>();
            foreach (ParcelaDocumento p in ListaParcelaDocumento)
            {
                ParcelaDocumento par = new ParcelaDocumento();
                par.CodigoDocumento = p.CodigoDocumento;
                par.CodigoParcela = p.CodigoParcela;
                par.DataVencimento = p.DataVencimento;
                par.ValorParcela = p.ValorParcela;
                par.DGNumeroDocumento = doc.NumeroDocumento + "/" + p.CodigoParcela;
                ListaParcela2.Add(par);
            }
            doc.CodigoSituacao = 42;
            decimal CodigoDocumento = 0;
            docDAL.Inserir(doc, ListaServicos, ListaItemServico, null, ListaAnexos,ListaParcela2, ref CodigoDocumento);
            btnConsultar_Click(sender, e);
        }

        protected void OpConsultar_ServerClick(object sender, EventArgs e)
        {
            decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);

            Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
            Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
            NFSe = NFSeDAL.PesquisarNotaFiscalServico(codigo);

            if(NFSe.CodigoSituacao == 42)
            {
                ShowMessage("Impossivel Consultar, NFS-e não enviada para autorização", MessageType.Info);
            }
            else
            {
                ShowPopupConsultar(sender, e);
            }

        }

        protected void OpAutorizar_ServerClick(object sender, EventArgs e)
        {
            PanelSelect = "consulta";
            IntegraDocumentoEletronico InDocEle = new IntegraDocumentoEletronico();
            IntegraDocumentoEletronicoDAL InDocEleDAL = new IntegraDocumentoEletronicoDAL();
            decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);
            InDocEle = InDocEleDAL.PesquisarIntegracaoDocEletronico(codigo,0);

            Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
            Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
            NFSe = NFSeDAL.PesquisarNotaFiscalServico(codigo);
            if (InDocEle != null && InDocEle.CodigoAcao == 43 && InDocEle.Mensagem == "")
            {
                ShowMessage("NFS-e já foi enviada para autorização!", MessageType.Info);
                PanelSelect = "consulta";
                return;
            }
            else
            {
                if(NFSe.CodigoSituacao == 41)
                {
                    ShowMessage("Esta NFS-e está cancelada", MessageType.Info);
                }
                else
                {
                    NFSe.ChaveAcesso = "";
                    NFSe.Protocolo = "";
                    InDocEleDAL.AtualizarDocumentoNFSe(NFSe);
                    ShowPopup(sender, e);
                }
                
            }
            
        }

        protected void ShowPopup(object sender, EventArgs e)
        {
            string title = "Atenção";
            string body = "Deseja Autorizar a Nota Fiscal de Serviço de Código "+Session["ZoomNotaFiscalServico"].ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        }

        protected void ShowPopupCancelar(object sender, EventArgs e)
        {
            string title = "Atenção";
            string body = "Deseja Cancelar a Nota Fiscal de Serviço de Código " + Session["ZoomNotaFiscalServico"].ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup2('" + title + "', '" + body + "');", true);
        }

        protected void ShowPopupConsultar(object sender, EventArgs e)
        {
            string title = "Atenção";
            string body = "Deseja consultar a Nota Fiscal de Serviço de Código " + Session["ZoomNotaFiscalServico"].ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "ShowPopup3('" + title + "', '" + body + "');", true);
        }

        protected void OpCancelar_ServerClick(object sender, EventArgs e)
        {
            PanelSelect = "consulta";
            IntegraDocumentoEletronico InDocEle = new IntegraDocumentoEletronico();
            IntegraDocumentoEletronicoDAL InDocEleDAL = new IntegraDocumentoEletronicoDAL();
            decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);

            Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
            Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
            NFSe = NFSeDAL.PesquisarNotaFiscalServico(codigo);

            InDocEle = InDocEleDAL.PesquisarIntegracaoDocEletronico(codigo,0);
            if (InDocEle != null && InDocEle.CodigoAcao == 44)
            {
                ShowMessage("Esta NFS-e já foi enviada para cancelamento!", MessageType.Info);
                PanelSelect = "consulta";
                return;
            }
            else
            {
                if (NFSe.CodigoSituacao == 39)
                {
                    ShowMessage("Esta NFS-e foi rejeitada", MessageType.Info);
                }

                else if (NFSe.CodigoSituacao == 42)
                {
                    ShowMessage("Esta NFS-e não foi enviada para autorizacão", MessageType.Info);
                }
                else
                {
                    ShowPopupCancelar(sender, e);
                }

            }
            
      
        }

        protected void OpImprimir_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string DiretorioEXE = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                _spdNFSeX.LoadConfig(DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\nfseConfig.ini");
                ProxyNFSe.ComponenteNFSe = _spdNFSeX;
                _spdNFSeConverterX.DiretorioEsquemas = _spdNFSeX.DiretorioEsquemas;
                _spdNFSeConverterX.DiretorioScripts = DiretorioEXE + @"\\TecnoSpeed\\NFSe\\Arquivos\\Scripts";
                _spdNFSeConverterX.Cidade = _spdNFSeX.Cidade;

                Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();

                decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);
                NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(codigo));

                if(NFSe.CodigoSituacao == 39)
                {
                    ShowMessage("NFS-e rejeitada", MessageType.Info);
                    return;
                }
                else if(NFSe.CodigoSituacao == 42)
                {
                    ShowMessage("Esta NFS-e não foi enviada para autorização!", MessageType.Info);
                    return;
                }
                List<AnexoDocumento> ListaAnexos = new List<AnexoDocumento>();
                AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
                ListaAnexos = anexoDAL.ObterAnexos(NFSe.CodigoNotaFiscalServico);

                string consulta = "";
                int contador = 0;
                ListaAnexos = ListaAnexos.OrderByDescending(c => c.CodigoAnexo).ToList();
                foreach (AnexoDocumento anexo in ListaAnexos)
                {
                    if(contador == 0)
                    {
                        if (anexo.NaoEditavel == 1)
                        {
                            consulta = System.Text.Encoding.ASCII.GetString(anexo.Arquivo);
                            contador++;

                        }
                    }
                }
                DBTabelaDAL RnTab = new DBTabelaDAL();
                DateTime data = RnTab.ObterDataHoraServidor();

                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                Usuario usu = new Usuario();
                UsuarioDAL usuDAL = new UsuarioDAL();
                usu = usuDAL.PesquisarUsuario(Convert.ToInt32(Session["CodUsuario"]));

                Habil_Log log = new Habil_Log();
                Habil_LogDAL logDAL = new Habil_LogDAL();
                log.DataHora = data;
                log.CodigoEstacao = he.CodigoEstacao ;
                log.CodigoIdentificador = Convert.ToDouble(NFSe.CodigoNotaFiscalServico);
                log.CodigoOperacao = 4;
                log.CodigoUsuario = usu.CodigoUsuario;
                log.DescricaoLog = "NFS-e Impressa";
                logDAL.Inserir(log);

                ProxyNFSe.ComponenteNFSe.ImpressaoModo = ModoImpressao.printNFSe;

                ProxyNFSe.ComponenteNFSe.Impressao_CriarDatasets(consulta);
                ProxyNFSe.ComponenteNFSe.Impressao_Configurar("LogotipoEmitente", DiretorioEXE + @"\Images\LogoDaEmpresa.png");
                ProxyNFSe.ComponenteNFSe.Impressao_ImprimirDocumentoCustom("", "", "");
                //ProxyNFSe.ComponenteNFSe.Impressao_VisualizarDocumento();
                PanelSelect = "consulta";
                NFSeFuncoes nfse = new NFSeFuncoes();
                nfse.GerandoArquivoLog("NFSe " + codigo +" Impressa",0);
            }
            catch (Exception ex)
            {

                NFSeFuncoes nfse = new NFSeFuncoes();

                nfse.GerandoArquivoLog("Erro na impressão da NFSe " + Session["ZoomNotaFiscalServico"].ToString() + " - " + ex.Message.ToString(), 0);
                ShowMessage(ex.ToString(), MessageType.Info);
            }
        }

        protected void OpDownNFSe_ServerClick(object sender, EventArgs e)
        {
            NFSeX.spdNFSeX _spdNFSeX = new NFSeX.spdNFSeX();
            NFSeDataSetX.spdNFSeDataSetX _spdNFSeDataSetX = new NFSeDataSetX.spdNFSeDataSetX();
            NFSeX.spdProxyNFSeX ProxyNFSe = new NFSeX.spdProxyNFSeX();
            spdNFSeConverterX _spdNFSeConverterX = new spdNFSeConverterX();
            try
            {
                string DiretorioEXE = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                _spdNFSeX.LoadConfig(DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\nfseConfig.ini");
                ProxyNFSe.ComponenteNFSe = _spdNFSeX;
                _spdNFSeConverterX.DiretorioEsquemas = DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\Esquemas";
                _spdNFSeConverterX.DiretorioScripts = DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\Scripts";
                _spdNFSeConverterX.Cidade = _spdNFSeX.Cidade;

                Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();

                decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);
                NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(codigo));

                if (NFSe.CodigoSituacao == 39)
                {
                    ShowMessage("NFS-e rejeitada", MessageType.Info);
                    return;
                }
                else if (NFSe.CodigoSituacao == 42)
                {
                    ShowMessage("Esta NFS-e não foi enviada para autorização!", MessageType.Info);
                    return;
                }
                List<AnexoDocumento> ListaAnexos = new List<AnexoDocumento>();
                AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
                ListaAnexos = anexoDAL.ObterAnexos(NFSe.CodigoNotaFiscalServico);

                DBTabelaDAL RnTab = new DBTabelaDAL();
                DateTime data = RnTab.ObterDataHoraServidor();

                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                Usuario usu = new Usuario();
                UsuarioDAL usuDAL = new UsuarioDAL();
                usu = usuDAL.PesquisarUsuario(Convert.ToInt32(Session["CodUsuario"]));

                Habil_Log log = new Habil_Log();
                Habil_LogDAL logDAL = new Habil_LogDAL();
                log.DataHora = data;
                log.CodigoEstacao = he.CodigoEstacao;
                log.CodigoIdentificador = Convert.ToDouble(NFSe.CodigoNotaFiscalServico);
                log.CodigoOperacao = 4;
                log.CodigoUsuario = usu.CodigoUsuario;
                log.DescricaoLog = "Download NFS-e";
                logDAL.Inserir(log);
                ListaAnexos = ListaAnexos.OrderByDescending(c => c.CodigoAnexo).ToList();
                foreach (AnexoDocumento anexo in ListaAnexos)
                {
                    if (anexo.NaoEditavel == 1)
                    {
                        string[] nome = anexo.NomeArquivo.Split('.');

                        string nomeArquivo = nome[0] + ".pdf";

                        string CaminhoArquivo = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Modulos\\Log\\NFSe\\" + nomeArquivo;
                        if (System.IO.File.Exists(CaminhoArquivo))
                            System.IO.File.Delete(CaminhoArquivo);
                        
                        string consulta = System.Text.Encoding.ASCII.GetString(anexo.Arquivo);
                        ProxyNFSe.ComponenteNFSe.ImpressaoModo = ModoImpressao.printNFSe;
                        
                        ProxyNFSe.ComponenteNFSe.Impressao_CriarDatasets(consulta);
                        ProxyNFSe.ComponenteNFSe.Impressao_Configurar("LogotipoEmitente", DiretorioEXE + @"\Images\LogoDaEmpresa.png");
                        ProxyNFSe.ComponenteNFSe.Impressao_ExportarDocumentoParaPDFCustom("", "", CaminhoArquivo);

                        NFSeFuncoes nfse = new NFSeFuncoes();
                        nfse.GerandoArquivoLog("Download da NFSe " + codigo,0);

                        Response.Clear();
                        Response.ContentType = "application/octect-stream";
                        Response.AppendHeader("content-disposition", "filename="+ nomeArquivo);
                        Response.TransmitFile(CaminhoArquivo);
                        Response.End();
                        return;
                    }
                   

                }
            }
            catch (Exception ex)
            {
                NFSeFuncoes nfse = new NFSeFuncoes();

                nfse.GerandoArquivoLog("Erro no Download da NFSe " + Session["ZoomNotaFiscalServico"].ToString(),0);
                ShowMessage(ex.ToString(), MessageType.Info);
            }
        }

        protected void OpDownXML_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();

                decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);
                NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(codigo));

                if (NFSe.CodigoSituacao == 39)
                {
                    ShowMessage("NFS-e rejeitada", MessageType.Info);
                    return;
                }
                else if (NFSe.CodigoSituacao == 42)
                {
                    ShowMessage("Esta NFS-e não foi enviada para autorização!", MessageType.Info);
                    return;
                }
                List<AnexoDocumento> ListaAnexos = new List<AnexoDocumento>();
                AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
                ListaAnexos = anexoDAL.ObterAnexos(NFSe.CodigoNotaFiscalServico);

                DBTabelaDAL RnTab = new DBTabelaDAL();
                DateTime data = RnTab.ObterDataHoraServidor();

                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                Usuario usu = new Usuario();
                UsuarioDAL usuDAL = new UsuarioDAL();
                usu = usuDAL.PesquisarUsuario(Convert.ToInt32(Session["CodUsuario"]));

                Habil_Log log = new Habil_Log();
                Habil_LogDAL logDAL = new Habil_LogDAL();
                log.DataHora = data;
                log.CodigoEstacao = he.CodigoEstacao;
                log.CodigoIdentificador = Convert.ToDouble(NFSe.CodigoNotaFiscalServico);
                log.CodigoOperacao = 4;
                log.CodigoUsuario = usu.CodigoUsuario;
                log.DescricaoLog = "XML da NFS-e Impressa";
                logDAL.Inserir(log);


                ListaAnexos = ListaAnexos.OrderByDescending(c => c.CodigoAnexo).ToList();
                foreach (AnexoDocumento anexo in ListaAnexos)
                {
                    if (anexo.NaoEditavel == 1)
                    {

                        string CaminhoArquivo = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Modulos\\Log\\NFSe\\" + anexo.NomeArquivo;

                        if (System.IO.File.Exists(CaminhoArquivo))
                            System.IO.File.Delete(CaminhoArquivo);


                        FileStream file = new FileStream(CaminhoArquivo, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(file);
                        bw.Write(anexo.Arquivo);
                        bw.Close();

                        file = new FileStream(CaminhoArquivo, FileMode.Open);
                        BinaryReader br = new BinaryReader(file);
                        file.Close();
                        NFSeFuncoes nfse = new NFSeFuncoes();
                        nfse.GerandoArquivoLog("Download XML da NFSe " + codigo,0);
                        Response.Clear();
                        Response.ContentType = "application/octect-stream";
                        Response.AppendHeader("content-disposition", "filename=" + anexo.NomeArquivo);
                        Response.TransmitFile(CaminhoArquivo);
                        Response.End();
                        return;
                    }

                }
            }
            catch
            {
                NFSeFuncoes nfse = new NFSeFuncoes();

                nfse.GerandoArquivoLog("Erro na impressão do XML da NFSe " + Session["ZoomNotaFiscalServico"].ToString(),0);
            }
            
        }

        protected void OpEmail_ServerClick(object sender, EventArgs e)
        {           
            try
            {
                Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();

                decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);
                NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(codigo));

                if (NFSe.CodigoSituacao == 39)
                {
                    ShowMessage("NFS-e rejeitada", MessageType.Info);
                    return;
                }
                else if (NFSe.CodigoSituacao == 42)
                {
                    ShowMessage("Esta NFS-e não foi enviada para autorização!", MessageType.Info);
                    return;
                }

                List<AnexoDocumento> ListaAnexos = new List<AnexoDocumento>();
                AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
                ListaAnexos = anexoDAL.ObterAnexos(NFSe.CodigoNotaFiscalServico);

                DBTabelaDAL RnTab = new DBTabelaDAL();
                DateTime data = RnTab.ObterDataHoraServidor();

                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                Usuario usu = new Usuario();
                UsuarioDAL usuDAL = new UsuarioDAL();
                usu = usuDAL.PesquisarUsuario(Convert.ToInt32(Session["CodUsuario"]));

                Pessoa_Contato Ctt = new Pessoa_Contato();
                PessoaContatoDAL CttDAL = new PessoaContatoDAL();
                Ctt = CttDAL.PesquisarPessoaContato(NFSe.CodigoTomador, 1);

                Habil_Log log = new Habil_Log();
                Habil_LogDAL logDAL = new Habil_LogDAL();
                log.DataHora = data;
                log.CodigoEstacao = he.CodigoEstacao;
                log.CodigoIdentificador = Convert.ToDouble(NFSe.CodigoNotaFiscalServico);
                log.CodigoOperacao = 4;
                log.CodigoUsuario = usu.CodigoUsuario;
                log.DescricaoLog = "NFS-e Enviada por email para " + Ctt._MailNFSE;
                logDAL.Inserir(log);
                ListaAnexos = ListaAnexos.OrderByDescending(c => c.CodigoAnexo).ToList();
                
                foreach (AnexoDocumento anexo in ListaAnexos)
                {
                    if (anexo.NaoEditavel == 1)
                    {
                        string CaminhoArquivo = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Modulos\\Log\\NFSe\\" + anexo.NomeArquivo;

                        if (System.IO.File.Exists(CaminhoArquivo))
                            System.IO.File.Delete(CaminhoArquivo);

                        FileStream file = new FileStream(CaminhoArquivo, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(file);
                        bw.Write(anexo.Arquivo);
                        bw.Close();

                        file = new FileStream(CaminhoArquivo, FileMode.Open);
                        BinaryReader br = new BinaryReader(file);
                        file.Close();

                        string[] nome = anexo.NomeArquivo.Split('.');

                        string nomeArquivo = nome[0] + ".pdf";
                        string CaminhoArquivoPDF = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\NFSe\\" + nomeArquivo;

                        if (System.IO.File.Exists(CaminhoArquivoPDF))
                            System.IO.File.Delete(CaminhoArquivoPDF);

                        string consulta = System.Text.Encoding.ASCII.GetString(anexo.Arquivo);

                        string DiretorioEXE = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                        _spdNFSeX.LoadConfig(DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\nfseConfig.ini");
                        ProxyNFSe.ComponenteNFSe = _spdNFSeX;
                        _spdNFSeConverterX.DiretorioEsquemas = DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\Esquemas";
                        _spdNFSeConverterX.DiretorioScripts = DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\Scripts";
                        _spdNFSeConverterX.Cidade = _spdNFSeX.Cidade;

                        ProxyNFSe.ComponenteNFSe.ImpressaoModo = ModoImpressao.printNFSe;
                        ProxyNFSe.ComponenteNFSe.Impressao_CriarDatasets(consulta);
                        ProxyNFSe.ComponenteNFSe.Impressao_Configurar("LogotipoEmitente", DiretorioEXE + @"\Images\LogoDaEmpresa.png");
                        ProxyNFSe.ComponenteNFSe.Impressao_ExportarDocumentoParaPDFCustom("", "", CaminhoArquivoPDF);
                        ProxyNFSe.ComponenteNFSe.EnviarEmail(Ctt._MailNFSE, CaminhoArquivoPDF + ";" + CaminhoArquivo);
                        
                        ShowMessage("Email enviado para " + Ctt._MailNFSE, MessageType.Info);
                        
                        NFSeFuncoes nfse = new NFSeFuncoes();
                        nfse.GerandoArquivoLog("Email Enviado com sucesso para " + Ctt._MailNFSE, 0);
                        
                        return;
                    }
                }
                PanelSelect = "consulta";
            }
            catch (Exception ex)
            {
                NFSeFuncoes nfse = new NFSeFuncoes();
                nfse.GerandoArquivoLog("Erro ao enviar email - " + ex.ToString(), 0);
                ShowMessage(ex.ToString(), MessageType.Info);
            }
        }

        protected void btnSimAutorizar_Click(object sender, EventArgs e)
        {
            IntegraDocumentoEletronico InDocEle = new IntegraDocumentoEletronico();
            IntegraDocumentoEletronicoDAL InDocEleDAL = new IntegraDocumentoEletronicoDAL();
            decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            InDocEle.CodigoDocumento = codigo;
            InDocEle.RegistroEnviado = 1;
            InDocEle.IntegracaoRecebido = 0;
            InDocEle.IntegracaoProcessando = 0;
            InDocEle.IntegracaoRetorno = 0;
            InDocEle.RegistroDevolvido = 0;
            InDocEle.RegistroMensagem = 0;
            InDocEle.Mensagem = "";
            InDocEle.CodigoAcao = 43;
            InDocEle.CodigoMaquina = Convert.ToInt32(he.CodigoEstacao);
            InDocEle.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);

            InDocEleDAL.Inserir(InDocEle);
            PanelSelect = "consulta";
            ShowMessage("Nota Fiscal de Serviço enviada para Autorização", MessageType.Success);

            Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
            Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
            NFSe = NFSeDAL.PesquisarNotaFiscalServico(codigo);
            NFSe.ChaveAcesso = "";
            NFSe.Protocolo = "";
            NFSeDAL.AtualizarChaveAcesso(NFSe);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#myModal", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#MyPopup').hide();", true);
        }

        protected void btnSimCancelar_Click(object sender, EventArgs e)
        {
            IntegraDocumentoEletronico InDocEle = new IntegraDocumentoEletronico();
            IntegraDocumentoEletronicoDAL InDocEleDAL = new IntegraDocumentoEletronicoDAL();
            decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            InDocEle.CodigoDocumento = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);
            InDocEle.RegistroEnviado = 1;
            InDocEle.IntegracaoRecebido = 0;
            InDocEle.IntegracaoProcessando = 0;
            InDocEle.IntegracaoRetorno = 0;
            InDocEle.RegistroDevolvido = 0;
            InDocEle.RegistroMensagem = 0;
            InDocEle.Mensagem = "";
            InDocEle.CodigoAcao = 44;
            InDocEle.CodigoMaquina = Convert.ToInt32(he.CodigoEstacao);
            InDocEle.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);

            InDocEleDAL.Inserir(InDocEle);
            PanelSelect = "consulta";
            ShowMessage("Nota Fiscal de Serviço enviada para cancelamento", MessageType.Success);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#myModal2", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#MyPopup2').hide();", true);
        }

        protected void BtnSimConsultar_Click(object sender, EventArgs e)
        {
            PanelSelect = "consulta";
            IntegraDocumentoEletronico InDocEle = new IntegraDocumentoEletronico();
            IntegraDocumentoEletronicoDAL InDocEleDAL = new IntegraDocumentoEletronicoDAL();
            decimal codigo = Convert.ToDecimal(Session["ZoomNotaFiscalServico"]);

            Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
            Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
            NFSe = NFSeDAL.PesquisarNotaFiscalServico(codigo);

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            InDocEle.CodigoDocumento = codigo;
            InDocEle.RegistroEnviado = 1;
            InDocEle.IntegracaoRecebido = 0;
            InDocEle.IntegracaoProcessando = 0;
            InDocEle.IntegracaoRetorno = 0;
            InDocEle.RegistroDevolvido = 0;
            InDocEle.RegistroMensagem = 0;
            InDocEle.Mensagem = "";
            InDocEle.CodigoAcao = 45;
            InDocEle.CodigoMaquina = Convert.ToInt32(he.CodigoEstacao);
            InDocEle.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);

            InDocEleDAL.Inserir(InDocEle);
            PanelSelect = "consulta";
            ShowMessage("Nota Fiscal de Serviço enviada para consulta", MessageType.Success);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#myModal3", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#MyPopup3').hide();", true);

        }

        public void grid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].ColumnSpan = 2;
                //now make up for the colspan from cell2
                e.Row.Cells.RemoveAt(8);
                //e.Row.Cells.RemoveAt(9);
            }
        }

        protected void grdRejeicoes_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void opConsultar_Click(object sender, EventArgs e)
        {
            OpConsultar_ServerClick(sender, e);
        }
    }
}