using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
namespace SoftHabilInformatica.Pages.Fiscal
    
{
    public partial class CadTipoOperacao: System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        List<TipoOperacao> listCadTipoOperacao = new List<TipoOperacao>();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
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
            v.CampoValido("Descrição do Tipo de Operação", txtDescricao.Text,true, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }

            if (ddlTipoMovimentacao.SelectedValue == "*Nenhum Selecionado")
            {
                ShowMessage("Tipo de Movimentação deve ser informado.", MessageType.Info);
                ddlTipoMovimentacao.Focus();
                return false;
            }

            if (ddlTipoOprFiscal.SelectedValue == "*Nenhum Selecionado")
            {
                ShowMessage("Operação Fiscal deve ser informado.", MessageType.Info);
                ddlTipoOprFiscal.Focus();
                return false;
            }

            if (ddlCFOPEstadual.SelectedValue == "*Nenhum Selecionado")
            {
                ShowMessage("Natureza Operação deve ser informado.", MessageType.Info);
                ddlCFOPEstadual.Focus();
                return false;
            }

            if (ddlCFOPInterestadual.SelectedValue == "*Nenhum Selecionado")
            {
                ShowMessage("Natureza Operação deve ser informado.", MessageType.Info);
                ddlCFOPInterestadual.Focus();
                return false;
            }

            if (ddlCFOPExterior.SelectedValue == "*Nenhum Selecionado")
            {
                ShowMessage("Natureza Operação deve ser informado.", MessageType.Info);
                ddlCFOPExterior.Focus();
                return false;
            }
            if (ddlPIS.SelectedValue == "*Nenhum selecionado")
            {
                ShowMessage("PIS deve ser informado.", MessageType.Info);
                ddlPIS.Focus();
                return false;
            }
            if (ddlCOFINS.SelectedValue == "*Nenhum selecionado")
            {
                ShowMessage("COFINS deve ser informado.", MessageType.Info);
                ddlCOFINS.Focus();
                return false;
            }
            if (chkMovLocOrigemDest.Checked != false)
                if(ddlTipoOprCtPartida.SelectedValue == "*Nenhum Selecionado")
                {
                    ShowMessage("Tipo de Operação de Contra Partida deve ser informado.", MessageType.Info);
                    ddlTipoOprCtPartida.Focus();
                    return false;
                }

            return true;
        }
        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlCodSituacao.DataSource = sd.Atividade();
            ddlCodSituacao.DataTextField = "DescricaoTipo";
            ddlCodSituacao.DataValueField = "CodigoTipo";
            ddlCodSituacao.DataBind();

            ddlModDetBCIcms.DataSource = sd.TipoModDetBCICMS();
            ddlModDetBCIcms.DataTextField = "DescricaoTipo";
            ddlModDetBCIcms.DataValueField = "CodigoTipo";
            ddlModDetBCIcms.DataBind();

            ddlModDetBCIcmsST.DataSource = sd.TipoModDetBCICMSST();
            ddlModDetBCIcmsST.DataTextField = "DescricaoTipo";
            ddlModDetBCIcmsST.DataValueField = "CodigoTipo";
            ddlModDetBCIcmsST.DataBind();

            ddlPrecedenciaICMS.DataSource = sd.TipoOperacaoPrecedenciaICMS();
            ddlPrecedenciaICMS.DataTextField = "DescricaoTipo";
            ddlPrecedenciaICMS.DataValueField = "CodigoTipo";
            ddlPrecedenciaICMS.DataBind();

            ddlPrecedenciaPISCOFINS.DataSource = sd.TipoOperacaoPrecedenciaPIS_COFINS();
            ddlPrecedenciaPISCOFINS.DataTextField = "DescricaoTipo";
            ddlPrecedenciaPISCOFINS.DataValueField = "CodigoTipo";
            ddlPrecedenciaPISCOFINS.DataBind();

            PISDAL pis = new PISDAL();
            ddlPIS.DataSource = pis.ListarPIS("", "", "", "");
            ddlPIS.DataTextField = "DescricaoPIS";
            ddlPIS.DataValueField = "CodigoIndice";
            ddlPIS.DataBind();
            ddlPIS.Items.Insert(0, "*Nenhum selecionado");

            COFINSDAL cofins = new COFINSDAL();
            ddlCOFINS.DataSource = cofins.ListarCOFINS("", "", "", "");
            ddlCOFINS.DataTextField = "DescricaoCOFINS";
            ddlCOFINS.DataValueField = "CodigoIndice";
            ddlCOFINS.DataBind();
            ddlCOFINS.Items.Insert(0, "*Nenhum selecionado");
        }
        protected void CarregaDropDownDesoneracao()
        {
            ddlTipoOprCtPartida.Items.Insert(0, "*Nenhum Selecionado");
            ddlTipoOprCtPartida.Enabled = true;

            Habil_TipoDAL RnTp = new Habil_TipoDAL();
            ddlCST20MovDesoneracao.DataSource = RnTp.MotivoDesoneracaoCST207090();
            ddlCST20MovDesoneracao.DataTextField = "DescricaoTipo";
            ddlCST20MovDesoneracao.DataValueField = "CodigoTipo";
            ddlCST20MovDesoneracao.DataBind();
            ddlCST20MovDesoneracao.Items.Insert(0, "*Nenhum Selecionado");

            ddlCST70MovDesoneracao.DataSource = RnTp.MotivoDesoneracaoCST207090();
            ddlCST70MovDesoneracao.DataTextField = "DescricaoTipo";
            ddlCST70MovDesoneracao.DataValueField = "CodigoTipo";
            ddlCST70MovDesoneracao.DataBind();
            ddlCST70MovDesoneracao.Items.Insert(0, "*Nenhum Selecionado");

            ddlCST90MovDesoneracao.Items.Clear();
            ddlCST90MovDesoneracao.DataSource = RnTp.MotivoDesoneracaoCST207090();
            ddlCST90MovDesoneracao.DataTextField = "DescricaoTipo";
            ddlCST90MovDesoneracao.DataValueField = "CodigoTipo";
            ddlCST90MovDesoneracao.DataBind();
            ddlCST90MovDesoneracao.Items.Insert(0, "*Nenhum Selecionado");

            ddlCST30MotDesoneracao.DataSource = RnTp.MotivoDesoneracaoCST30();
            ddlCST30MotDesoneracao.DataTextField = "DescricaoTipo";
            ddlCST30MotDesoneracao.DataValueField = "CodigoTipo";
            ddlCST30MotDesoneracao.DataBind();
            ddlCST30MotDesoneracao.Items.Insert(0, "*Nenhum Selecionado");

            ddlCST405051MovDesoneracao.DataSource = RnTp.MotivoDesoneracaoCST404150();
            ddlCST405051MovDesoneracao.DataTextField = "DescricaoTipo";
            ddlCST405051MovDesoneracao.DataValueField = "CodigoTipo";
            ddlCST405051MovDesoneracao.DataBind();
            ddlCST405051MovDesoneracao.Items.Insert(0, "*Nenhum Selecionado");

        }
        protected void CarregaDropDown()
        {
            NatOperacaoDAL RnNatOp = new NatOperacaoDAL();
            Habil_TipoOperacaoFiscalDAL RnTpOpFis = new Habil_TipoOperacaoFiscalDAL();
            Habil_TipoDAL RnTp = new Habil_TipoDAL();

            ddlCFOPEstadual.DataSource = RnNatOp.ListarNatOperacao("", "", "", "");
            ddlCFOPEstadual.DataTextField = "DescricaoNaturezaOperacao";
            ddlCFOPEstadual.DataValueField = "CodigoNaturezaOperacao";
            ddlCFOPEstadual.DataBind();
            ddlCFOPEstadual.Items.Insert(0, "*Nenhum Selecionado");

            ddlCFOPInterestadual.DataSource = ddlCFOPEstadual.DataSource;
            ddlCFOPInterestadual.DataTextField = "DescricaoNaturezaOperacao";
            ddlCFOPInterestadual.DataValueField = "CodigoNaturezaOperacao";
            ddlCFOPInterestadual.DataBind();
            ddlCFOPInterestadual.Items.Insert(0, "*Nenhum Selecionado");

            ddlCFOPExterior.DataSource = ddlCFOPEstadual.DataSource;
            ddlCFOPExterior.DataTextField = "DescricaoNaturezaOperacao";
            ddlCFOPExterior.DataValueField = "CodigoNaturezaOperacao";
            ddlCFOPExterior.DataBind();
            ddlCFOPExterior.Items.Insert(0, "*Nenhum Selecionado");

            ddlTipoOprFiscal.DataSource = RnTpOpFis.ListaOperacaoFiscal();
            ddlTipoOprFiscal.DataTextField = "DescricaoHabil_TipoOperFiscal";
            ddlTipoOprFiscal.DataValueField = "CodigoHabil_TipoOperFiscal";
            ddlTipoOprFiscal.DataBind();
            ddlTipoOprFiscal.Items.Insert(0, "*Nenhum Selecionado");

            ddlTipoMovimentacao.DataSource = RnTp.TipoMovimentacaoEstoque();
            ddlTipoMovimentacao.DataTextField = "DescricaoTipo";
            ddlTipoMovimentacao.DataValueField = "CodigoTipo";
            ddlTipoMovimentacao.DataBind();
            ddlTipoMovimentacao.Items.Insert(0, "*Nenhum Selecionado");

            Habil_RegTributarioDAL rtd = new Habil_RegTributarioDAL();
            ddlRegTributario.DataSource = rtd.ListaHabil_RegTributario();
            ddlRegTributario.DataTextField = "DescricaoHabil_RegTributario";
            ddlRegTributario.DataValueField = "CodigoHabil_RegTributario";
            ddlRegTributario.DataBind();
            ddlRegTributario.Items.Insert(0, "*Nenhum Selecionado");

            CarregaDropDownDesoneracao();


            pnlImpostos.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomTipoOperacao2"] != null)
            {
                if (Session["ZoomTipoOperacao2"].ToString() == "RELACIONAL")
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

            PanelSelect = "home";

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();

            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConTipoOperacao.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomTipoOperacao2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomTipoOperacao"] != null)
                {
                    string s = Session["ZoomTipoOperacao"].ToString();
                    Session["ZoomTipoOperacao"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                LimpaCampos();

                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                TipoOperacaoDAL r = new TipoOperacaoDAL();
                                TipoOperacao p = new TipoOperacao();
                               
                                p = r.PesquisarTipoOperacao(Convert.ToInt32(txtCodigo.Text));
                                txtCodigo.Text = p.CodigoTipoOperacao.ToString();                               
                                txtDescricao.Text = p.DescricaoTipoOperacao.ToString();
                                ddlCodSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                ddlCFOPEstadual.SelectedValue = Convert.ToString(p.CodCFOPEstadual);
                                ddlCFOPInterestadual.SelectedValue = Convert.ToString(p.CodCFOPInterestadual);
                                ddlCFOPExterior.SelectedValue = Convert.ToString(p.CodCFOPExterior);
                                ddlTipoMovimentacao.SelectedValue = Convert.ToString(p.CodigoTipoMovimentacao);
                                ddlTipoOprFiscal.SelectedValue = Convert.ToString(p.CodTipoOperFiscal);
                                chkAtuEstoque.Checked = p.MovimentaEstoque;
                                chkAtuFinanceiro.Checked = p.AtualizaFinanceiro;
                                chkMovInterna.Checked = p.MovimentacaoInterna;
                                chkBaixaFinanceiro.Checked = p.BaixaFinanceiro;
                                chkMovLocOrigemDest.Checked = p.MovLocOrigemDestino;
                                ddlPIS.SelectedValue = p.CodigoPIS.ToString();
                                ddlCOFINS.SelectedValue = p.CodigoCOFINS.ToString();
                                ddlPrecedenciaICMS.SelectedValue = p.CodigoPrecedenciaImpostoICMS.ToString();
                                ddlPrecedenciaPISCOFINS.SelectedValue = p.CodigoPrecedenciaImpostoPIS_COFINS.ToString();
                                ddlRegTributario.SelectedValue = p.CodHabil_RegTributario.ToString();
                                ddlRegTributario_SelectedIndexChanged(sender, e);

                                txtMsgIcms.Text  = p.MensagemIcms;
                                txtCodBnfFiscal.Text = p.CodBeneficioFiscal;
                                ddlCSTCSOSN.SelectedValue = p.CodCST_CSOSN.ToString();
                                ddlCSTCSOSN_SelectedIndexChanged(sender, e);

                                ddlModDetBCIcms.SelectedValue = p.CodModDetBCIcms.ToString();
                                ddlModDetBCIcmsST.SelectedValue = p.CodModDetBCIcmsST.ToString();
                                txtMvaEntrada.Text = p.MVAEntrada.ToString("###,##0.00000");

                                
                                CarregaTipoContraPartida();
                                if (p.CodTipoOperCtPartida != 0)
                                    ddlTipoOprCtPartida.SelectedValue = Convert.ToString(p.CodTipoOperCtPartida);

                                if (p.CodHabil_RegTributario != 0)
                                {
                                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                                    {
                                        switch (p.CodCST_CSOSN)
                                        {
                                            case 00:
                                                txtCST00Icms.Text = p.CST00ICMS.ToString("###,##0.00"); ;
                                                break;
                                            case 10:
                                                txtCST10RedBCIcmsST.Text = p.CST10ReducaoBCICMSST.ToString("###,##0.00000"); ;
                                                txtCST10Icms.Text = p.CST10ICMS.ToString("###,##0.00");
                                                txtCST10RedBCIcmsProprio.Text = p.CST10ReducaoBCICMSSTProprio.ToString("###,##0.00000");
                                                txtCST10IcmsProprio.Text = p.CST10ICMSProprio.ToString("###,##0.00");
                                                txtCST10Mva.Text = p.CST10MVASaida.ToString("###,##0.00000");
                                                chkCST10Difal.Checked = p.CST10CalculaDifal;
                                                chkCST10Difal_CheckedChanged(sender, e);
                                                break;
                                            case 20:
                                                txtCST20RedBC.Text = p.CST20ReducaoBCICMS.ToString("###,##0.00000"); 
                                                txtCST20Icms.Text = p.CST20ICMS.ToString("###,##0.00");
                                                if (p.CST20MotDesoneracao != 0)
                                                    ddlCST20MovDesoneracao.SelectedValue = p.CST20MotDesoneracao.ToString();
                                                break;
                                            case 30:
                                                txtCST30RedBCIcmsST.Text = p.CST30ReducaoBCICMSST.ToString("###,##0.00000");
                                                txtCST30Icms.Text = p.CST30ICMS.ToString("###,##0.00");
                                                txtCST30IcmsProprio.Text = p.CST30ICMSProprio.ToString("###,##0.00");
                                                txtCST30Mva.Text = p.CST30MVASaida.ToString("###,##0.00000");
                                                if (p.CST30MotDesoneracao != 0)
                                                    ddlCST30MotDesoneracao.SelectedValue = p.CST30MotDesoneracao.ToString();
                                                break;
                                            case 40:
                                                if (p.CST404150MotDesoneracao != 0)
                                                    ddlCST405051MovDesoneracao.SelectedValue = p.CST404150MotDesoneracao.ToString();
                                                break;
                                            case 41:
                                                if (p.CST404150MotDesoneracao != 0)
                                                    ddlCST405051MovDesoneracao.SelectedValue = p.CST404150MotDesoneracao.ToString();
                                                break;
                                            case 50:
                                                if (p.CST404150MotDesoneracao != 0)
                                                    ddlCST405051MovDesoneracao.SelectedValue = p.CST404150MotDesoneracao.ToString();
                                                break;
                                            case 51:
                                                txtCST51RedBC.Text = p.CST51ReducaoBCICMS.ToString("###,##0.00000");
                                                txtCST51Icms.Text = p.CST51ICMS.ToString("###,##0.00");
                                                txtCST51Diferimento.Text = p.CST51Diferimento.ToString("###,##0.00");
                                                break;
                                            case 70:
                                                txtCST70RedBCIcmsST.Text =p.CST70ReducaoBCICMSST.ToString("###,##0.00000");
                                                txtCST70Icms.Text = p.CST70ICMS.ToString("###,##0.00");
                                                txtCST70RedBCIcmsProprio.Text = p.CST70ReducaoBCICMSSTProprio.ToString("###,##0.00000");
                                                txtCST70IcmsProprio.Text = p.CST70ICMSProprio.ToString("###,##0.00");
                                                txtCST70Mva.Text = p.CST70MVASaida.ToString("###,##0.00000");
                                                if (p.CST70MotDesoneracao != 0)
                                                    ddlCST70MovDesoneracao.Text = p.CST70MotDesoneracao.ToString();
                                                break;
                                            case 90:
                                                txtCST90RedBCIcmsST.Text = p.CST90ReducaoBCICMSST.ToString("###,##0.00000");
                                                txtCST90Icms.Text = p.CST90ICMS.ToString("###,##0.00");
                                                txtCST90RedBCIcmsProprio.Text = p.CST90ReducaoBCICMSSTProprio.ToString("###,##0.00000");
                                                txtCST90IcmsProprio.Text = p.CST90ICMSProprio.ToString("###,##0.00");
                                                txtCST90Mva.Text = p.CST90MVASaida.ToString("###,##0.00000");
                                                chkCST90CalcDifal.Checked = p.CST90CalculaDifal;
                                                chkCST90CalcDifal_CheckedChanged(sender, e);
                                                if (p.CST90MotDesoneracao !=0)
                                                    ddlCST90MovDesoneracao.SelectedValue = p.CST90MotDesoneracao.ToString();
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        switch (p.CodCST_CSOSN)
                                        {

                                            case 101:
                                                txtCSOSN101IcmsSimples.Text = p.CSOSN101_ICMS_SIMPLES.ToString("###,##0.00");
                                                break;
                                            case 201:
                                                txtCSOSN201RedBCST.Text = p.CSOSN201_ReducaoBCICMSST.ToString("###,##0.00000");
                                                txtCSOSN202203Icms.Text =p.CSOSN201_ICMS.ToString("###,##0.00");
                                                txtCSOSN201Mva.Text = p.CSOSN201_MVASaida.ToString("###,##0.00000");
                                                txtCSOSN201IcmsSimples.Text = p.CSOSN201_ICMS_SIMPLES.ToString("###,##0.00");
                                                break;
                                            case 202:
                                                txtCSOSN202203RedBCST.Text =p.CSOSN202_203_ReducaoBCICMSST.ToString("###,##0.00000");
                                                txtCSOSN202203Icms.Text = p.CSOSN202_203_ICMS.ToString("###,##0.00");
                                                txtCSOSN202203IcmsProprio.Text = p.CSOSN202_203_ICMS_PROPRIO.ToString("###,##0.00");
                                                txtCSOSN202203Mva.Text = p.CSOSN202_203_MVASaida.ToString("###,##0.000000");
                                                break;
                                            case 203:
                                                txtCSOSN202203RedBCST.Text = p.CSOSN202_203_ReducaoBCICMSST.ToString("###,##0.00000");
                                                txtCSOSN202203Icms.Text = p.CSOSN202_203_ICMS.ToString("###,##0.00");
                                                txtCSOSN202203IcmsProprio.Text = p.CSOSN202_203_ICMS_PROPRIO.ToString("###,##0.00");
                                                txtCSOSN202203Mva.Text = p.CSOSN202_203_MVASaida.ToString("###,##0.000000");
                                                break;
                                            case 900:
                                                txtCSOSN900RedBCIcmsST.Text = p.CSOSN900_ReducaoBCICMSST.ToString("###,##0.00000");
                                                txtCSOSN900Icms.Text = p.CSOSN900_ICMS.ToString("###,##0.00");
                                                txtCSOSN900IcmsProprio.Text = p.CSOSN900_ICMS_Proprio.ToString("###,##0.00");
                                                txtCSOSN900RedBCIcmsProprio.Text = p.CSOSN900_ReducaoBCICMSProprio.ToString("###,##0.00000");
                                                txtCSOSN900Mva.Text = p.CSOSN900_MVASaida.ToString("###,##0.00000");
                                                txtCSOSN900IcmsSimples.Text = p.CSOSN900_ICMS_SIMPLES.ToString("###,##0.00");
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                chkCST90CalcDifal.Checked = p.CST90CalculaDifal;
                                chkCST10Difal.Checked = p.CST10CalculaDifal;

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

                                return;
                            }
                    }
                }
                else
                {
                    LimpaCampos();
                                       
                    btnExcluir.Visible = false;
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                        }
                    });

                }
            }
            if(txtCodigo.Text == "")
                Response.Redirect("~/Pages/Fiscal/ConTipoOperacao.aspx");

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void LimpaCampos()
        {
            txtCodigo.Text = "Novo";
            txtCodigo.Enabled = false;
            txtDescricao.Text = "";

            CarregaSituacoes();
            ddlCodSituacao.SelectedValue = "1";
            CarregaDropDown();

            txtMvaEntrada.Text = "0,00000";

            txtCST00Icms.Text = "0,00";
            txtCST10RedBCIcmsST.Text = "0,00000";
            txtCST10Icms.Text = "0,00";
            txtCST10RedBCIcmsProprio.Text = "0,00000";
            txtCST10IcmsProprio.Text = "0,00";
            txtCST10Mva.Text = "0,00000";
            chkCST10Difal.Checked = false;
            txtCST10Difal.Text = "0,00";
            txtCST20RedBC.Text = "0,00000";
            txtCST20Icms.Text = "0,00";
            txtCST30RedBCIcmsST.Text = "0,00000";
            txtCST30Icms.Text = "0,00";
            txtCST30IcmsProprio.Text = "0,00000";
            txtCST30Mva.Text = "0,00000";
            txtCST51Diferimento.Text = "100,00";
            txtCST51Icms.Text = "0,00";
            txtCST51RedBC.Text = "0,00000";
            txtCST70RedBCIcmsST.Text = "0,00000";
            txtCST70Icms.Text = "0,00";
            txtCST70RedBCIcmsProprio.Text = "0,00000";
            txtCST70IcmsProprio.Text = "0,00";
            txtCST70Mva.Text = "0,00000";
            txtCST90RedBCIcmsST.Text = "0,00000";
            txtCST90Icms.Text = "0,00";
            txtCST90RedBCIcmsProprio.Text = "0,00000";
            txtCST90IcmsProprio.Text = "0,00";
            txtCST90Mva.Text = "0,00000";
            chkCST90CalcDifal.Checked = false;
            txtCST90Difal.Text = "0,00";
            txtCSOSN101IcmsSimples.Text = "0,00";
            txtCSOSN201RedBCST.Text = "0,00000";
            txtCSOSN201Icms.Text = "0,00";
            txtCSOSN201Mva.Text = "0,00000";
            txtCSOSN201IcmsSimples.Text = "0,00";
            txtCSOSN202203RedBCST.Text = "0,00000";
            txtCSOSN202203Icms.Text = "0,00";
            txtCSOSN202203IcmsProprio.Text = "0,00";
            txtCSOSN202203Mva.Text = "0,00000";
            txtCSOSN900RedBCIcmsST.Text = "0,00000";
            txtCSOSN900Icms.Text = "0,00";
            txtCSOSN900RedBCIcmsProprio.Text = "0,00000";
            txtCSOSN900IcmsProprio.Text = "0,00";
            txtCSOSN900Mva.Text = "0,00000";
            txtCSOSN900IcmsSimples.Text = "0,00";
            txtCodBnfFiscal.Text = "";
            txtMsgIcms.Text = "";
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    TipoOperacaoDAL d = new TipoOperacaoDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Tipo de Operação excluído com Sucesso!!!";
                    
                    btnVoltar_Click(sender, e);
                    
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Tipo de Operação não identificado.&emsp;&emsp;&emsp;";

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

            Session["ZoomTipoOperacao"] = null;
            if (Session["ZoomTipoOperacao2"] != null)
            {
                Session["ZoomTipoOperacao2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadTipoOperacao.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Session["InscricaoTipoOperacao"] = null;
            
            listCadTipoOperacao = null;

            Response.Redirect("~/Pages/Fiscal/ConTipoOperacao.aspx");
            

        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
            {
                return;
            }

            TipoOperacaoDAL d = new TipoOperacaoDAL();
            TipoOperacao p = new TipoOperacao();

                                 
            p.CodigoSituacao = Convert.ToInt16(ddlCodSituacao.SelectedValue);
            p.DescricaoTipoOperacao = txtDescricao.Text.ToUpper();

            p.CodigoSituacao= Convert.ToInt32 (ddlCodSituacao.SelectedValue);

            p.CodCFOPEstadual = Convert.ToInt64(ddlCFOPEstadual.SelectedValue);
            p.CodCFOPInterestadual = Convert.ToInt64(ddlCFOPInterestadual.SelectedValue);
            p.CodCFOPExterior = Convert.ToInt64(ddlCFOPExterior.SelectedValue);
            if(ddlTipoOprCtPartida.SelectedValue != "*Nenhum Selecionado")
               p.CodTipoOperCtPartida = Convert.ToInt32(ddlTipoOprCtPartida.SelectedValue);

            p.CodigoTipoMovimentacao = Convert.ToInt32(ddlTipoMovimentacao.SelectedValue);
            p.CodTipoOperFiscal = Convert.ToInt32(ddlTipoOprFiscal.SelectedValue);
            p.MovimentaEstoque = chkAtuEstoque.Checked;
            p.AtualizaFinanceiro = chkAtuFinanceiro.Checked;
            p.MovimentacaoInterna = chkMovInterna.Checked;
            p.BaixaFinanceiro = chkBaixaFinanceiro.Checked;
            p.MovLocOrigemDestino = chkMovLocOrigemDest.Checked;
            p.CodigoPIS = Convert.ToInt32(ddlPIS.SelectedValue);
            p.CodigoCOFINS = Convert.ToInt32(ddlCOFINS.SelectedValue);
            p.CodigoPrecedenciaImpostoICMS = Convert.ToInt32(ddlPrecedenciaICMS.SelectedValue);
            p.CodigoPrecedenciaImpostoPIS_COFINS = Convert.ToInt32(ddlPrecedenciaPISCOFINS.SelectedValue);

            p.MensagemIcms = txtMsgIcms.Text;
            p.CodBeneficioFiscal = txtCodBnfFiscal.Text;
            p.CodHabil_RegTributario = 0;
            if (ddlRegTributario.SelectedItem.Text != "*Nenhum Selecionado")
            {
                p.CodHabil_RegTributario = Convert.ToInt32(ddlRegTributario.SelectedValue);
                p.CodCST_CSOSN = Convert.ToInt32(ddlCSTCSOSN.SelectedValue);
                p.CodModDetBCIcms = Convert.ToInt32(ddlModDetBCIcms.SelectedValue);
                p.CodModDetBCIcmsST = Convert.ToInt32(ddlModDetBCIcmsST.SelectedValue);
                p.MVAEntrada = Convert.ToDecimal(txtMvaEntrada.Text);
            
            
                p.CST90CalculaDifal = chkCST90CalcDifal.Checked;

                if (p.CodHabil_RegTributario != 0)
                {
                    if (p.CodHabil_RegTributario == 3)  //REGIME NORMAL
                    {
                        switch (p.CodCST_CSOSN)
                        {
                            case 00:
                                p.CST00ICMS = Convert.ToDecimal(txtCST00Icms.Text);
                                break;
                            case 10:
                                p.CST10ReducaoBCICMSST = Convert.ToDecimal(txtCST10RedBCIcmsST.Text);
                                p.CST10ICMS = Convert.ToDecimal(txtCST10Icms.Text);
                                p.CST10ReducaoBCICMSSTProprio = Convert.ToDecimal(txtCST10RedBCIcmsProprio.Text);
                                p.CST10ICMSProprio = Convert.ToDecimal(txtCST10IcmsProprio.Text);
                                p.CST10MVASaida = Convert.ToDecimal(txtCST10Mva.Text);
                                p.CST10CalculaDifal = Convert.ToBoolean(chkCST10Difal.Checked);
                                break;
                            case 20:
                                p.CST20ReducaoBCICMS = Convert.ToDecimal(txtCST20RedBC.Text);
                                p.CST20ICMS = Convert.ToDecimal(txtCST20Icms.Text);

                                if (ddlCST20MovDesoneracao.SelectedItem.Text != "* Nenhum Selecionado")
                                    p.CST20MotDesoneracao = Convert.ToInt32(ddlCST20MovDesoneracao.SelectedValue);
                                break;
                            case 30:
                                p.CST30ReducaoBCICMSST = Convert.ToDecimal(txtCST30RedBCIcmsST.Text);
                                p.CST30ICMS = Convert.ToDecimal(txtCST30Icms.Text);
                                p.CST30ICMSProprio = Convert.ToDecimal(txtCST30IcmsProprio.Text);
                                p.CST30MVASaida = Convert.ToDecimal(txtCST30Mva.Text);
                                if (ddlCST30MotDesoneracao.SelectedItem.Text != "* Nenhum Selecionado")
                                    p.CST30MotDesoneracao = Convert.ToInt32(ddlCST30MotDesoneracao.SelectedValue);
                                break;
                            case 40:
                                if (ddlCST405051MovDesoneracao.SelectedItem.Text != "* Nenhum Selecionado")

                                    p.CST404150MotDesoneracao = Convert.ToInt32(ddlCST405051MovDesoneracao.SelectedValue);
                                break;
                            case 41:
                                p.CST404150MotDesoneracao = Convert.ToInt32(ddlCST405051MovDesoneracao.SelectedValue);
                                break;
                            case 50:
                                p.CST404150MotDesoneracao = Convert.ToInt32(ddlCST405051MovDesoneracao.SelectedValue);
                                break;
                            case 51:
                                p.CST51ReducaoBCICMS = Convert.ToDecimal(txtCST51RedBC.Text);
                                p.CST51ICMS = Convert.ToDecimal(txtCST51Icms.Text);
                                p.CST51Diferimento = Convert.ToDecimal(txtCST51Diferimento.Text);
                                break;
                            case 70:
                                p.CST70ReducaoBCICMSST = Convert.ToDecimal(txtCST70RedBCIcmsST.Text);
                                p.CST70ICMS = Convert.ToDecimal(txtCST70Icms.Text);
                                p.CST70ReducaoBCICMSSTProprio = Convert.ToDecimal(txtCST70RedBCIcmsProprio.Text);
                                p.CST70ICMSProprio = Convert.ToDecimal(txtCST70IcmsProprio.Text);
                                p.CST70MVASaida = Convert.ToDecimal(txtCST70Mva.Text);
                                if (ddlCST70MovDesoneracao.SelectedItem.Text != "* Nenhum Selecionado")
                                    p.CST70MotDesoneracao = Convert.ToInt32(ddlCST70MovDesoneracao.Text);
                                break;
                            case 90:
                                p.CST90ReducaoBCICMSST = Convert.ToDecimal(txtCST90RedBCIcmsST.Text);
                                p.CST90ICMS = Convert.ToDecimal(txtCST90Icms.Text);
                                p.CST90ReducaoBCICMSSTProprio = Convert.ToDecimal(txtCST90RedBCIcmsProprio.Text);
                                p.CST90ICMSProprio = Convert.ToDecimal(txtCST90IcmsProprio.Text);
                                p.CST90MVASaida = Convert.ToDecimal(txtCST90Mva.Text);
                                p.CST90CalculaDifal = Convert.ToBoolean(chkCST90CalcDifal.Checked);
                                if (ddlCST90MovDesoneracao.SelectedItem.Text != "* Nenhum Selecionado")
                                    p.CST90MotDesoneracao = Convert.ToInt32(ddlCST90MovDesoneracao.SelectedValue);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (p.CodCST_CSOSN)
                        {

                            case 101:
                                p.CSOSN101_ICMS_SIMPLES = Convert.ToDecimal(txtCSOSN101IcmsSimples.Text);
                                break;

                            case 201:
                                p.CSOSN201_ReducaoBCICMSST = Convert.ToDecimal(txtCSOSN201RedBCST.Text);
                                p.CSOSN201_ICMS = Convert.ToDecimal(txtCSOSN201Icms.Text);
                                p.CSOSN201_MVASaida = Convert.ToDecimal(txtCSOSN201Mva.Text);
                                p.CSOSN201_ICMS_SIMPLES = Convert.ToDecimal(txtCSOSN201IcmsSimples.Text);
                                break;

                            case 202:
                                p.CSOSN202_203_ReducaoBCICMSST = Convert.ToDecimal(txtCSOSN202203RedBCST.Text);
                                p.CSOSN202_203_ICMS = Convert.ToDecimal(txtCSOSN202203Icms.Text);
                                p.CSOSN202_203_ICMS_PROPRIO = Convert.ToDecimal(txtCSOSN202203IcmsProprio.Text);
                                p.CSOSN202_203_MVASaida = Convert.ToDecimal(txtCSOSN202203Mva.Text);
                                break;

                            case 203:
                                p.CSOSN202_203_ReducaoBCICMSST = Convert.ToDecimal(txtCSOSN202203RedBCST.Text);
                                p.CSOSN202_203_ICMS = Convert.ToDecimal(txtCSOSN202203Icms.Text);
                                p.CSOSN202_203_ICMS_PROPRIO = Convert.ToDecimal(txtCSOSN202203IcmsProprio.Text);
                                p.CSOSN202_203_MVASaida = Convert.ToDecimal(txtCSOSN202203Mva.Text);
                                break;

                            case 900:
                                p.CSOSN900_ReducaoBCICMSST = Convert.ToDecimal(txtCSOSN900RedBCIcmsST.Text);
                                p.CSOSN900_ICMS = Convert.ToDecimal(txtCSOSN900Icms.Text);
                                p.CSOSN900_ICMS_Proprio = Convert.ToDecimal(txtCSOSN900IcmsProprio.Text);
                                p.CSOSN900_ReducaoBCICMSProprio = Convert.ToDecimal(txtCSOSN900RedBCIcmsProprio.Text);
                                p.CSOSN900_MVASaida = Convert.ToDecimal(txtCSOSN900Mva.Text);
                                p.CSOSN900_ICMS_SIMPLES = Convert.ToDecimal(txtCSOSN900IcmsSimples.Text);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            if (txtCodigo.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Tipo de Operação Incluído com Sucesso!!!";

            }
            else
            {
                p.CodigoTipoOperacao = Convert.ToInt64(txtCodigo.Text);
                d.Atualizar(p);

                Session["MensagemTela"] = "Tipo de Operação Alterado com Sucesso!!!";
            }

            btnVoltar_Click(sender, e);
            

        }

        protected void ddlRegTributario_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                Habil_SitTributariaDAL rtd = new Habil_SitTributariaDAL();

                ddlCSTCSOSN.DataSource = rtd.ListaHabil_SitTributaria(Convert.ToInt32(ddlRegTributario.SelectedValue));
                ddlCSTCSOSN.DataTextField = "DescricaoHabil_SitTributaria";
                ddlCSTCSOSN.DataValueField = "CodigoHabil_SitTributaria";
                ddlCSTCSOSN.DataBind();
                ddlCSTCSOSN_SelectedIndexChanged(sender, e);
                pnlImpostos.Visible = true;
            }
            catch (Exception ex)
            {
                pnlImpostos.Visible = false;
            }
        }
        protected void ddlCSTCSOSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblCSTCSOSN.Text = ddlCSTCSOSN.SelectedItem.Text;

                pnlCST00.Visible = false;
                pnlCST10.Visible = false;
                pnlCST20.Visible = false;
                pnlCST30.Visible = false;
                pnlCST404150.Visible = false;
                pnlCST51.Visible = false;
                pnlCST70.Visible = false;
                pnlCST90.Visible = false;
                pnlCSOSN101.Visible = false;
                pnlCSOSN201.Visible = false;
                pnlCSOSN202203.Visible = false;
                pnlCSOSN900.Visible = false;
                CarregaDropDownDesoneracao();

                switch (ddlCSTCSOSN.SelectedValue)
                {
                    case "00":
                        pnlCST00.Visible = true;
                        break;

                    case "10":
                        pnlCST10.Visible = true;
                        break;

                    case "20":
                        pnlCST20.Visible = true;
                        break;

                    case "30":
                        pnlCST30.Visible = true;
                        break;

                    case "40":
                        pnlCST404150.Visible = true;
                        break;

                    case "41":
                        pnlCST404150.Visible = true;
                        break;

                    case "50":
                        pnlCST404150.Visible = true;
                        break;

                    case "51":
                        pnlCST51.Visible = true;
                        break;

                    case "70":
                        pnlCST70.Visible = true;
                        break;

                    case "90":
                        pnlCST90.Visible = true;
                        break;

                    case "101":
                        pnlCSOSN101.Visible = true;
                        break;

                    case "201":
                        pnlCSOSN201.Visible = true;
                        break;

                    case "202":
                        pnlCSOSN202203.Visible = true;
                        break;

                    case "203":
                        pnlCSOSN202203.Visible = true;
                        break;

                    case "900":
                        pnlCSOSN900.Visible = true;
                        break;

                    default:
                        break;
                }

                    lblRegTributario.Text = ddlRegTributario.SelectedItem.Text;

            }
            catch (Exception ex)
            {
            }
        }
        protected void txtCST00Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST00Icms.Text.Equals(""))
            {
                txtCST00Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Interna da Operação", txtCST00Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST00Icms.Text = Convert.ToDecimal(txtCST00Icms.Text).ToString("###,##0.00");
                else
                    txtCST00Icms.Text = "0,00";

            }

        }
        protected void txtMvaEntrada_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtMvaEntrada.Text.Equals(""))
            {
                txtMvaEntrada.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Margem de Valor Agregado(MVA) Entrada", txtMvaEntrada.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtMvaEntrada.Text = Convert.ToDecimal(txtMvaEntrada.Text).ToString("###,##0.00000");
                else
                    txtMvaEntrada.Text = "0,00000";

            }
        }
        protected void txtCST10RedBCIcmsST_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST10RedBCIcmsST.Text.Equals(""))
            {
                txtCST10RedBCIcmsST.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo ST", txtCST10RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST10RedBCIcmsST.Text = Convert.ToDecimal(txtCST10RedBCIcmsST.Text).ToString("###,##0.00000");
                else
                    txtCST10RedBCIcmsST.Text = "0,00000";

            }

        }
        protected void txtCST10Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST10Icms.Text.Equals(""))
            {
                txtCST10Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota da Operação", txtCST10Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST10Icms.Text = Convert.ToDecimal(txtCST10Icms.Text).ToString("###,##0.00");
                else
                    txtCST10Icms.Text = "0,00";

            }

        }
        protected void txtCST10RedBCIcmsProprio_TextChanged(object sender, EventArgs e)
        {
            
            Boolean blnCampo = false;
            if (txtCST10RedBCIcmsProprio.Text.Equals(""))
            {
                txtCST10RedBCIcmsProprio.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo Icms Próprio", txtCST10RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST10RedBCIcmsProprio.Text = Convert.ToDecimal(txtCST10RedBCIcmsProprio.Text).ToString("###,##0.00000");
                else
                    txtCST10RedBCIcmsProprio.Text = "0,00000";

            }
        }
        protected void txtCST10IcmsProprio_TextChanged(object sender, EventArgs e)
        {
            
            Boolean blnCampo = false;
            if (txtCST10IcmsProprio.Text.Equals(""))
            {
                txtCST10IcmsProprio.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Icms Próprio", txtCST10IcmsProprio.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST10IcmsProprio.Text = Convert.ToDecimal(txtCST10IcmsProprio.Text).ToString("###,##0.00");
                else
                    txtCST10IcmsProprio.Text = "0,00";

            }

        }
        protected void txtCST10Mva_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST10Mva.Text.Equals(""))
            {
                txtCST10Mva.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Margem de Valor Agregado(MVA) Saída", txtCST10Mva.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST10Mva.Text = Convert.ToDecimal(txtCST10Mva.Text).ToString("###,##0.00000");
                else
                    txtCST10Mva.Text = "0,00000";

            }

        }
        protected void chkCST10Difal_CheckedChanged(object sender, EventArgs e)
        {
            decimal decValor = 0;
            if (chkCST10Difal.Checked)
            {
                decValor = Convert.ToDecimal(txtCST10IcmsProprio.Text) - Convert.ToDecimal(txtCST10Icms.Text);
                txtCST10Difal.Text = decValor.ToString("##0.00");
            }
            else
                txtCST10Difal.Text = "0,00";
        }
        protected void txtCST10Difal_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtCST20RedBC_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST20RedBC.Text.Equals(""))
            {
                txtCST20RedBC.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo Icms", txtCST20RedBC.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST20RedBC.Text = Convert.ToDecimal(txtCST20RedBC.Text).ToString("###,##0.00000");
                else
                    txtCST20RedBC.Text = "0,00000";

                decimal decIcmsEfetivo = 0;
                decIcmsEfetivo = Convert.ToDecimal(txtCST20Icms.Text) * Convert.ToDecimal(txtCST20RedBC.Text) / 100;
                txtCST20Efetiva.Text = decIcmsEfetivo.ToString("###,##0.00");

            }

        }
        protected void txtCST20Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST20Icms.Text.Equals(""))
                txtCST20Icms.Text = "0,00";
            else
            {
                v.CampoValido("Alíquota Interna da Operação", txtCST20Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                    txtCST20Icms.Text = Convert.ToDecimal(txtCST20Icms.Text).ToString("###,##0.00");
                else
                    txtCST20Icms.Text = "0,00";
            }

            decimal decIcmsEfetivo = 0;
            decIcmsEfetivo = Convert.ToDecimal(txtCST20Icms.Text) * Convert.ToDecimal(txtCST20RedBC.Text) / 100;
            txtCST20Efetiva.Text = decIcmsEfetivo.ToString("###,##0.00");

        }
        protected void txtCST30RedBCIcmsST_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST30RedBCIcmsST.Text.Equals(""))
            {
                txtCST30RedBCIcmsST.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo ST", txtCST30RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST30RedBCIcmsST.Text = Convert.ToDecimal(txtCST30RedBCIcmsST.Text).ToString("###,##0.00000");
                else
                    txtCST30RedBCIcmsST.Text = "0,00000";

            }

        }
        protected void txtCST30Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST30Icms.Text.Equals(""))
            {
                txtCST30Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota da Operação", txtCST30Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST30Icms.Text = Convert.ToDecimal(txtCST30Icms.Text).ToString("###,##0.00");
                else
                    txtCST30Icms.Text = "0,00";

            }


        }
        protected void txtCST30IcmsProprio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST30IcmsProprio.Text.Equals(""))
            {
                txtCST30IcmsProprio.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Icms Próprio", txtCST30IcmsProprio.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST30IcmsProprio.Text = Convert.ToDecimal(txtCST30IcmsProprio.Text).ToString("###,##0.00");
                else
                    txtCST30IcmsProprio.Text = "0,00";

            }

        }
        protected void txtCST30Mva_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST30Mva.Text.Equals(""))
            {
                txtCST30Mva.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Margem de Valor Agregado(MVA) Saída", txtCST30Mva.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST30Mva.Text = Convert.ToDecimal(txtCST30Mva.Text).ToString("###,##0.00000");
                else
                    txtCST30Mva.Text = "0,00000";

            }

        }
        protected void txtCST51Diferimento_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST51Diferimento.Text.Equals(""))
            {
                txtCST51Diferimento.Text = "100,00";
            }
            else
            {
                v.CampoValido("% Diferimento", txtCST51Diferimento.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST51Diferimento.Text = Convert.ToDecimal(txtCST51Diferimento.Text).ToString("###,##0.00");
                else
                    txtCST51Diferimento.Text = "100,00";

            }

        }
        protected void txtCST51Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST51Icms.Text.Equals(""))
            {
                txtCST51Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Interna da Operação", txtCST51Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST51Icms.Text = Convert.ToDecimal(txtCST51Icms.Text).ToString("###,##0.00");
                else
                    txtCST51Icms.Text = "0,00";

            }


        }
        protected void txtCST51RedBC_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST51RedBC.Text.Equals(""))
            {
                txtCST51RedBC.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo Icms", txtCST51RedBC.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST51RedBC.Text = Convert.ToDecimal(txtCST51RedBC.Text).ToString("###,##0.00000");
                else
                    txtCST51RedBC.Text = "0,00000";

            }

        }
        protected void txtCST70RedBCIcmsST_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST70RedBCIcmsST.Text.Equals(""))
            {
                txtCST70RedBCIcmsST.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo ST", txtCST70RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST70RedBCIcmsST.Text = Convert.ToDecimal(txtCST70RedBCIcmsST.Text).ToString("###,##0.00000");
                else
                    txtCST70RedBCIcmsST.Text = "0,00000";

            }

        }
        protected void txtCST70Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST70Icms.Text.Equals(""))
            {
                txtCST70Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota da Operação", txtCST70Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST70Icms.Text = Convert.ToDecimal(txtCST70Icms.Text).ToString("###,##0.00");
                else
                    txtCST70Icms.Text = "0,00";

            }


        }
        protected void txtCST70RedBCIcmsProprio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST70RedBCIcmsProprio.Text.Equals(""))
            {
                txtCST70RedBCIcmsProprio.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo Icms Próprio", txtCST70RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST70RedBCIcmsProprio.Text = Convert.ToDecimal(txtCST70RedBCIcmsProprio.Text).ToString("###,##0.00000");
                else
                    txtCST70RedBCIcmsProprio.Text = "0,00000";

            }

        }
        protected void txtCST70IcmsProprio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST70IcmsProprio.Text.Equals(""))
            {
                txtCST70IcmsProprio.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Icms Próprio", txtCST70IcmsProprio.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST70IcmsProprio.Text = Convert.ToDecimal(txtCST70IcmsProprio.Text).ToString("###,##0.00");
                else
                    txtCST70IcmsProprio.Text = "0,00";

            }

        }
        protected void txtCST70Mva_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST70Mva.Text.Equals(""))
            {
                txtCST70Mva.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Margem de Valor Agregado(MVA) Saída", txtCST70Mva.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST70Mva.Text = Convert.ToDecimal(txtCST70Mva.Text).ToString("###,##0.00000");
                else
                    txtCST70Mva.Text = "0,00000";

            }

        }
        protected void txtCST90RedBCIcmsST_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST90RedBCIcmsST.Text.Equals(""))
            {
                txtCST90RedBCIcmsST.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo ST", txtCST90RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST90RedBCIcmsST.Text = Convert.ToDecimal(txtCST90RedBCIcmsST.Text).ToString("###,##0.00000");
                else
                    txtCST90RedBCIcmsST.Text = "0,00000";

            }

        }
        protected void txtCST90Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST90Icms.Text.Equals(""))
            {
                txtCST90Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota da Operação", txtCST90Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST90Icms.Text = Convert.ToDecimal(txtCST90Icms.Text).ToString("###,##0.00");
                else
                    txtCST90Icms.Text = "0,00";

            }


        }
        protected void txtCST90RedBCIcmsProprio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST90RedBCIcmsProprio.Text.Equals(""))
            {
                txtCST90RedBCIcmsProprio.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo Icms Próprio", txtCST90RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST90RedBCIcmsProprio.Text = Convert.ToDecimal(txtCST90RedBCIcmsProprio.Text).ToString("###,##0.00000");
                else
                    txtCST90RedBCIcmsProprio.Text = "0,00000";

            }

        }
        protected void txtCST90IcmsProprio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST90IcmsProprio.Text.Equals(""))
            {
                txtCST90IcmsProprio.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Icms Próprio", txtCST90IcmsProprio.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST90IcmsProprio.Text = Convert.ToDecimal(txtCST90IcmsProprio.Text).ToString("###,##0.00");
                else
                    txtCST90IcmsProprio.Text = "0,00";

            }

        }
        protected void txtCST90Mva_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCST90Mva.Text.Equals(""))
            {
                txtCST90Mva.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Margem de Valor Agregado(MVA) Saída", txtCST90Mva.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCST90Mva.Text = Convert.ToDecimal(txtCST90Mva.Text).ToString("###,##0.00000");
                else
                    txtCST90Mva.Text = "0,00000";

            }

        }
        protected void chkCST90CalcDifal_CheckedChanged(object sender, EventArgs e)
        {
            decimal decValor = 0;
            if (chkCST90CalcDifal.Checked)
            {
                decValor = Convert.ToDecimal(txtCST90IcmsProprio.Text) - Convert.ToDecimal(txtCST90Icms.Text);
                txtCST90Difal.Text = decValor.ToString("##0.00");
            }
            else
                txtCST90Difal.Text = "0,00";

        }
        protected void txtCST90Difal_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtCSOSN101IcmsSimples_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN101IcmsSimples.Text.Equals(""))
            {
                txtCSOSN101IcmsSimples.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Interna da Operação do Simples Nacional", txtCSOSN101IcmsSimples.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN101IcmsSimples.Text = Convert.ToDecimal(txtCSOSN101IcmsSimples.Text).ToString("###,##0.00");
                else
                    txtCSOSN101IcmsSimples.Text = "0,00";

            }

        }
        protected void txtCSOSN201RedBCST_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN201RedBCST.Text.Equals(""))
            {
                txtCSOSN201RedBCST.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo ST", txtCSOSN201RedBCST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN201RedBCST.Text = Convert.ToDecimal(txtCSOSN201RedBCST.Text).ToString("###,##0.00000");
                else
                    txtCSOSN201RedBCST.Text = "0,00000";

            }

        }
        protected void txtCSOSN201Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN201Icms.Text.Equals(""))
            {
                txtCSOSN201Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota da Operação", txtCSOSN201Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN201Icms.Text = Convert.ToDecimal(txtCSOSN201Icms.Text).ToString("###,##0.00");
                else
                    txtCSOSN201Icms.Text = "0,00";

            }


        }
        protected void txtCSOSN201Mva_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN201Mva.Text.Equals(""))
            {
                txtCSOSN201Mva.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Margem de Valor Agregado(MVA) Saída", txtCSOSN201Mva.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN201Mva.Text = Convert.ToDecimal(txtCSOSN201Mva.Text).ToString("###,##0.00000");
                else
                    txtCSOSN201Mva.Text = "0,00000";

            }

        }
        protected void txtCSOSN201IcmsSimples_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN201IcmsSimples.Text.Equals(""))
            {
                txtCSOSN201IcmsSimples.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Interna da Operação do Simples Nacional", txtCSOSN201IcmsSimples.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN201IcmsSimples.Text = Convert.ToDecimal(txtCSOSN201IcmsSimples.Text).ToString("###,##0.00");
                else
                    txtCSOSN201IcmsSimples.Text = "0,00";

            }

        }
        protected void txtCSOSN202203RedBCST_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN202203RedBCST.Text.Equals(""))
            {
                txtCSOSN202203RedBCST.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo ST", txtCST10RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN202203RedBCST.Text = Convert.ToDecimal(txtCSOSN202203RedBCST.Text).ToString("###,##0.00000");
                else
                    txtCSOSN202203RedBCST.Text = "0,00000";

            }

        }
        protected void txtCSOSN202203Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN202203Icms.Text.Equals(""))
            {
                txtCSOSN202203Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota da Operação", txtCSOSN202203Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN202203Icms.Text = Convert.ToDecimal(txtCSOSN202203Icms.Text).ToString("###,##0.00");
                else
                    txtCSOSN202203Icms.Text = "0,00";

            }

        }
        protected void txtCSOSN202203IcmsProprio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN202203IcmsProprio.Text.Equals(""))
            {
                txtCSOSN202203IcmsProprio.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Icms Próprio", txtCSOSN202203IcmsProprio.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN202203IcmsProprio.Text = Convert.ToDecimal(txtCSOSN202203IcmsProprio.Text).ToString("###,##0.00");
                else
                    txtCSOSN202203IcmsProprio.Text = "0,00";

            }

        }
        protected void txtCSOSN202203Mva_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN202203Mva.Text.Equals(""))
            {
                txtCSOSN202203Mva.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Margem de Valor Agregado(MVA) Saída", txtCSOSN202203Mva.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN202203Mva.Text = Convert.ToDecimal(txtCSOSN202203Mva.Text).ToString("###,##0.00000");
                else
                    txtCSOSN202203Mva.Text = "0,00000";

            }

        }
        protected void txtCSOSN900RedBCIcmsST_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN900RedBCIcmsST.Text.Equals(""))
            {
                txtCSOSN900RedBCIcmsST.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo ST", txtCSOSN900RedBCIcmsST.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN900RedBCIcmsST.Text = Convert.ToDecimal(txtCSOSN900RedBCIcmsST.Text).ToString("###,##0.00000");
                else
                    txtCSOSN900RedBCIcmsST.Text = "0,00000";

            }

        }
        protected void txtCSOSN900Icms_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN900Icms.Text.Equals(""))
            {
                txtCSOSN900Icms.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota da Operação", txtCSOSN900Icms.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN900Icms.Text = Convert.ToDecimal(txtCSOSN900Icms.Text).ToString("###,##0.00");
                else
                    txtCSOSN900Icms.Text = "0,00";

            }

        }
        protected void txtCSOSN900RedBCIcmsProprio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN900RedBCIcmsProprio.Text.Equals(""))
            {
                txtCSOSN900RedBCIcmsProprio.Text = "0,00000";
            }
            else
            {
                v.CampoValido("% Redução da Base de Cálculo Icms Próprio", txtCSOSN900RedBCIcmsProprio.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN900RedBCIcmsProprio.Text = Convert.ToDecimal(txtCSOSN900RedBCIcmsProprio.Text).ToString("###,##0.00000");
                else
                    txtCSOSN900RedBCIcmsProprio.Text = "0,00000";

            }

        }
        protected void txtCSOSN900IcmsProprio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN900IcmsProprio.Text.Equals(""))
            {
                txtCSOSN900IcmsProprio.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Icms Próprio", txtCSOSN900IcmsProprio.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN900IcmsProprio.Text = Convert.ToDecimal(txtCSOSN900IcmsProprio.Text).ToString("###,##0.00");
                else
                    txtCSOSN900IcmsProprio.Text = "0,00";

            }

        }
        protected void txtCSOSN900Mva_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN900Mva.Text.Equals(""))
            {
                txtCSOSN900Mva.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Margem de Valor Agregado(MVA) Saída", txtCSOSN900Mva.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN900Mva.Text = Convert.ToDecimal(txtCSOSN900Mva.Text).ToString("###,##0.00000");
                else
                    txtCSOSN900Mva.Text = "0,00000";

            }

        }
        protected void txtCSOSN900IcmsSimples_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCSOSN900IcmsSimples.Text.Equals(""))
            {
                txtCSOSN900IcmsSimples.Text = "0,00";
            }
            else
            {
                v.CampoValido("Alíquota Interna da Operação do Simples Nacional", txtCSOSN900IcmsSimples.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtCSOSN900IcmsSimples.Text = Convert.ToDecimal(txtCSOSN900IcmsSimples.Text).ToString("###,##0.00");
                else
                    txtCSOSN900IcmsSimples.Text = "0,00";

            }

        }
        protected void ddlTipoMovimentacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaTipoContraPartida();
        }
        protected void CarregaTipoContraPartida()
        {
            TipoOperacaoDAL RnTpDal = new TipoOperacaoDAL();

            int tp = 0;

            if (ddlTipoMovimentacao.SelectedIndex == 3)
            {
                ddlTipoOprCtPartida.Items.Clear();
                ddlTipoOprCtPartida.Items.Insert(0, "*Nenhum Selecionado");
                ddlTipoOprCtPartida.Enabled = false;
                chkMovLocOrigemDest.Enabled = false;
            }
            else
            {
                if (ddlTipoMovimentacao.SelectedIndex == 1)
                    tp = 61;
                else
                    tp = 60;

                ddlTipoOprCtPartida.DataSource = RnTpDal.ListarTpOperContraPartida(tp);
                ddlTipoOprCtPartida.DataTextField = "DescricaoTipoOperacao";
                ddlTipoOprCtPartida.DataValueField = "CodigoTipoOperacao";
                ddlTipoOprCtPartida.SelectedValue = null;
                ddlTipoOprCtPartida.DataBind();
                ddlTipoOprCtPartida.Items.Insert(0, "*Nenhum Selecionado");
                ddlTipoOprCtPartida.Enabled = true;
            }
        }
        protected void ddlCodSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void txtDescricao_TextChanged(object sender, EventArgs e)
        {

        }

        protected void chkAtuFinanceiro_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAtuFinanceiro.Checked == false)
                chkBaixaFinanceiro.Checked = false;
            else
                chkBaixaFinanceiro.Checked = true;

        }

        protected void chkBaixaFinanceiro_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBaixaFinanceiro.Checked == true)
                chkAtuFinanceiro.Checked = true;
            else
                chkAtuFinanceiro.Checked = true;
        }

        protected void chkMovLocOrigemDest_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkMovInterna_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMovInterna.Checked == true)
                chkMovLocOrigemDest.Enabled = true;
            else
                chkMovLocOrigemDest.Enabled = false;
        }
    }
}