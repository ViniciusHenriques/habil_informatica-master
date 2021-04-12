using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Impostos
{
    public partial class ManRegFisIcms : System.Web.UI.Page
    {
        List<TabIcms> t1 = new List<TabIcms>();
        List<GpoTribPessoa> t2 = new List<GpoTribPessoa>();
        List<GpoTribProduto> t3 = new List<GpoTribProduto>();
        List<Habil_AplicacaoUso> t4 = new List<Habil_AplicacaoUso>();
        List<Habil_TipoOperacaoFiscal> t5 = new List<Habil_TipoOperacaoFiscal>();

        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaTela()
        {

            Session["ListaLocalizacaoIcms"] = null;
            Session["ListaGpoTribPesIcms"] = null;
            Session["ListaGpoTribProIcms"] = null;
            Session["ListaAplicaUsoIcms"] = null;
            Session["ListaOperFiscalIcms"] = null;

            Habil_RegTributarioDAL rtd = new Habil_RegTributarioDAL();
            ddlRegTributario.DataSource = rtd.ListaHabil_RegTributario();
            ddlRegTributario.DataTextField = "DescricaoHabil_RegTributario";
            ddlRegTributario.DataValueField = "CodigoHabil_RegTributario";
            ddlRegTributario.DataBind();
            ddlRegTributario.Items.Insert(0, "*Nenhum Selecionado");

            EstadoDAL ed = new EstadoDAL();
            ddlEstadoOrigem.DataSource = ed.ObterEstadosSiglaDaEmpresa();
            ddlEstadoOrigem.DataTextField = "DescricaoEstado";
            ddlEstadoOrigem.DataValueField = "CodigoEstado";
            ddlEstadoOrigem.DataBind();
            ddlEstadoOrigem.Items.Insert(0, "*Nenhum Selecionado");

            EstadoDAL ed2 = new EstadoDAL();
            ddlEstadoDestino.DataSource = ed2.ObterEstadosSigla();
            ddlEstadoDestino.DataTextField = "DescricaoEstado";
            ddlEstadoDestino.DataValueField = "CodigoEstado";
            ddlEstadoDestino.DataBind();
            ddlEstadoDestino.Items.Insert(0, "*Nenhum Selecionado");

            GpoTribPessoaDAL gtpd = new GpoTribPessoaDAL();
            ddlGpoTribPessoa.DataSource = gtpd.ObterGpoTribPessoas("", "", "", "");
            ddlGpoTribPessoa.DataTextField = "DescricaoGpoTribPessoa";
            ddlGpoTribPessoa.DataValueField = "CodigoGpoTribPessoa";
            ddlGpoTribPessoa.DataBind();
            ddlGpoTribPessoa.Items.Insert(0, "*Nenhum Selecionado");

            GpoTribProdutoDAL gtpd2 = new GpoTribProdutoDAL();
            ddlGpoTribProduto.DataSource = gtpd2.ObterGpoTribProdutos("", "", "", "");
            ddlGpoTribProduto.DataTextField = "DescricaoGpoTribProduto";
            ddlGpoTribProduto.DataValueField = "CodigoGpoTribProduto";
            ddlGpoTribProduto.DataBind();
            ddlGpoTribProduto.Items.Insert(0, "*Nenhum Selecionado");

            Habil_AplicacaoUsoDAL s2 = new Habil_AplicacaoUsoDAL();
            ddlAplicacaoUso.DataSource = s2.ListaHabil_AplicacaoUso();
            ddlAplicacaoUso.DataTextField = "DescricaoHabil_AplicacaoUso";
            ddlAplicacaoUso.DataValueField = "CodigoHabil_AplicacaoUso";
            ddlAplicacaoUso.DataBind();
            ddlAplicacaoUso.Items.Insert(0, "*Nenhum Selecionado");

            Habil_TipoOperacaoFiscalDAL o3 = new Habil_TipoOperacaoFiscalDAL();
            ddlOperFiscal.DataSource = o3.ListaOperacaoFiscal();
            ddlOperFiscal.DataTextField = "DescricaoHabil_TipoOperFiscal";
            ddlOperFiscal.DataValueField = "CodigoHabil_TipoOperFiscal";
            ddlOperFiscal.DataBind();
            ddlOperFiscal.Items.Insert(0, "*Nenhum Selecionado");

            txtMvaEntrada.Text = "0,0000";
            txtdtfinal.Text = "";
            txtdtfinal.Text = "";
            CarregaSituacoes();
            ddlSituacao.SelectedValue = "1";
            txtDataHora.Text = "";
            txtDescricao.Text = "";

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



            ddlAcao.Focus();

        }
        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            ddlModDetBCIcms.DataSource = sd.TipoModDetBCICMS();
            ddlModDetBCIcms.DataTextField = "DescricaoTipo";
            ddlModDetBCIcms.DataValueField = "CodigoTipo";
            ddlModDetBCIcms.DataBind();

            ddlModDetBCIcmsST.DataSource = sd.TipoModDetBCICMSST();
            ddlModDetBCIcmsST.DataTextField = "DescricaoTipo";
            ddlModDetBCIcmsST.DataValueField = "CodigoTipo";
            ddlModDetBCIcmsST.DataBind();

        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            if (pnlAlterar.Visible)
                if (ddlAlteracao.SelectedIndex.ToString() == "0")
                {
                    ShowMessage("Selecione o que deve ser Alterado", MessageType.Info);
                    return false;
                }

            v.CampoValido("Data de Vigência", txtdtinicial.Text,  true, false, false, false,"SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtdtinicial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage (strMensagemR,MessageType.Info);
                    txtdtinicial.Focus();
                }
                return false;
            }

            if (ddlRegTributario.Text == "*Nenhum Selecionado")
            {
                ShowMessage("Regime Tributário deve ser informado.", MessageType.Info);
                ddlRegTributario.Focus();
                return false;

            }

            if (grdLocalizacao.Rows.Count == 0)
            {
                ShowMessage("Localização deve ser informado.", MessageType.Info);
                ddlRegTributario.Focus();
                return false;
            }

            if (grdGpoTribPessoa.Rows.Count == 0)
            {
                ShowMessage("Grupo de Pessoa deve ser informado.", MessageType.Info);
                ddlRegTributario.Focus();
                return false;
            }

            if (grdGpoTribProduto.Rows.Count == 0)
            {
                ShowMessage("Grupo de Produto deve ser informado.", MessageType.Info);
                ddlRegTributario.Focus();
                return false;
            }

            if (grdAplicacaoUso.Rows.Count == 0)
            {
                ShowMessage("Aplicação no Uso deve ser informado.", MessageType.Info);
                ddlRegTributario.Focus();
                return false;
            }

            if (grdOperFiscal.Rows.Count == 0)
            {
                ShowMessage("Tipo de Operação Fiscal deve ser informado.", MessageType.Info);
                ddlRegTributario.Focus();
                return false;
            }


            return true;
        }
        protected void MontaTela(long CodRegra)
        {
            LimpaTela();

            pnlRegra.Enabled = false;
            pnlAplicacao.Enabled = false;
            pnlGrupos.Enabled = false;

            //Montar a Lista dos Estados

            RegFisIcmsLocalizacaoDAL r = new RegFisIcmsLocalizacaoDAL();
            TabIcmsDAL t = new TabIcmsDAL();
            TabIcms ta = new TabIcms();
            List<RegFisIcmsLocalizacao> lista = new List<RegFisIcmsLocalizacao>();

            lista = r.ListarRegFisIcmsLocalizacao(CodRegra);

            foreach (RegFisIcmsLocalizacao item in  lista)
            {
                ta = t.PesquisarTabIcms(item.CodLocalizacaoUF);

                ddlEstadoOrigem.SelectedValue = ta.CodOrigem.ToString();
                ddlEstadoDestino.SelectedValue = ta.CodDestino.ToString();

                BtnAddLocalizacao_Click(null,null);

            }

            ///////////////////////////////////////////////////////////////////////////////////

            //Montar a Grupo de Pessoas

            RegFisIcmsGpoTribPessoaDAL rr2 = new RegFisIcmsGpoTribPessoaDAL();
            List<RegFisIcmsGpoTribPessoa> lista2 = new List<RegFisIcmsGpoTribPessoa>();

            lista2 = rr2.ListarRegFisIcmsGpoTribPessoa(CodRegra);

            foreach (RegFisIcmsGpoTribPessoa item in lista2)
            {
                ddlGpoTribPessoa .SelectedValue = item.CodGpoTribPessoa.ToString();
                BtnGpoTribPessoa_Click(null, null);
            }

            ///////////////////////////////////////////////////////////////////////////////////


            //Montar a Grupo de Produtos

            RegFisIcmsGpoTribProdutoDAL rr3 = new RegFisIcmsGpoTribProdutoDAL();
            List<RegFisIcmsGpoTribProduto> lista3 = new List<RegFisIcmsGpoTribProduto>();

            lista3 = rr3.ListarRegFisIcmsGpoTribProduto(CodRegra);

            foreach (RegFisIcmsGpoTribProduto item in lista3)
            {
                ddlGpoTribProduto.SelectedValue = item.CodGpoTribProduto.ToString();
                BtnGpoTribProduto_Click(null, null);
            }

            ///////////////////////////////////////////////////////////////////////////////////

            //Montar a Aplicação de Uso

            RegFisIcmsHabil_AplicacaoUsoDAL rr4 = new RegFisIcmsHabil_AplicacaoUsoDAL();
            List<RegFisIcmsHabil_AplicacaoUso> lista4 = new List<RegFisIcmsHabil_AplicacaoUso>();

            lista4 = rr4.ListarRegFisIcmsHabil_AplicacaoUso(CodRegra);

            foreach (RegFisIcmsHabil_AplicacaoUso item in lista4)
            {
                ddlAplicacaoUso.SelectedValue = item.CodHabil_AplicacaoUso.ToString();
                BtnAplicacaoUso_Click(null, null);
            }

            ///////////////////////////////////////////////////////////////////////////////////

            //Montar a Operação Fiscal

            RegFisIcmsTipoOperFiscalDAL rr5 = new RegFisIcmsTipoOperFiscalDAL();
            List<RegFisIcmsTipoOperFiscal> lista5 = new List<RegFisIcmsTipoOperFiscal>();

            lista5 = rr5.ListarRegFisIcmsOperFiscal(CodRegra);

            foreach (RegFisIcmsTipoOperFiscal item in lista5)
            {
                ddlOperFiscal.SelectedValue = item.CodTipoOperFiscal.ToString();
                btnOperFiscal_Click(null, null);
            }

            ///////////////////////////////////////////////////////////////////////////////////

            RegFisIcmsDAL rp = new RegFisIcmsDAL();
            RegFisIcms rr = new RegFisIcms();

            rr = rp.PesquisarRegFisIcms(CodRegra);

            ddlSituacao.SelectedValue = rr.CodSituacao.ToString();
            txtDataHora.Text = rr.DataHora.ToString();
            txtDescricao.Text = rr.Descricao;

            if (rr.DataVigencia != null)
                txtdtinicial.Text = rr.DataVigencia.ToString().Substring(0, 10);
            if (rr.DataAtualizacao != null)
                txtdtfinal.Text = rr.DataAtualizacao.ToString().Substring(0, 10);

            ddlRegTributario.SelectedValue = rr.CodHabil_RegTributario.ToString();
            ddlRegTributario_SelectedIndexChanged(null, null);

            txtMsgIcms.Text = rr.MensagemIcms;
            txtCodBnfFiscal.Text = rr.CodBeneficioFiscal;

            if (rr.CodCST_CSOSN.ToString() == "0")
            {
                ddlCSTCSOSN.SelectedValue = "00";
            }
            else
                ddlCSTCSOSN.SelectedValue = rr.CodCST_CSOSN.ToString();

            ddlCSTCSOSN_SelectedIndexChanged(null, null);

            ddlModDetBCIcms.SelectedValue = rr.CodModDetBCIcms.ToString();
            ddlModDetBCIcmsST.SelectedValue = rr.CodModDetBCIcmsST.ToString();
            txtMvaEntrada.Text = rr.MVAEntrada.ToString("###,##0.00000");

            if (rr.CodHabil_RegTributario != 0)
            {
                if (rr.CodHabil_RegTributario == 3)  //REGIME NORMAL
                {
                    switch (rr.CodCST_CSOSN)
                    {
                        case 00:
                            txtCST00Icms.Text = rr.CST00ICMS.ToString("###,##0.00"); ;
                            break;
                        case 10:
                            txtCST10RedBCIcmsST.Text = rr.CST10ReducaoBCICMSST.ToString("###,##0.00000"); ;
                            txtCST10Icms.Text = rr.CST10ICMS.ToString("###,##0.00");
                            txtCST10RedBCIcmsProprio.Text = rr.CST10ReducaoBCICMSSTProprio.ToString("###,##0.00000");
                            txtCST10IcmsProprio.Text = rr.CST10ICMSProprio.ToString("###,##0.00");
                            txtCST10Mva.Text = rr.CST10MVASaida.ToString("###,##0.00000");
                            chkCST10Difal.Checked = rr.CST10CalculaDifal;
                            chkCST10Difal_CheckedChanged(null, null);
                            break;
                        case 20:
                            txtCST20RedBC.Text = rr.CST20ReducaoBCICMS.ToString("###,##0.00000");
                            txtCST20Icms.Text = rr.CST20ICMS.ToString("###,##0.00");
                            if (rr.CST20MotDesoneracao != 0)
                                ddlCST20MovDesoneracao.SelectedValue = rr.CST20MotDesoneracao.ToString();
                            break;
                        case 30:
                            txtCST30RedBCIcmsST.Text = rr.CST30ReducaoBCICMSST.ToString("###,##0.00000");
                            txtCST30Icms.Text = rr.CST30ICMS.ToString("###,##0.00");
                            txtCST30IcmsProprio.Text = rr.CST30ICMSProprio.ToString("###,##0.00");
                            txtCST30Mva.Text = rr.CST30MVASaida.ToString("###,##0.00000");
                            if (rr.CST30MotDesoneracao != 0)
                                ddlCST30MotDesoneracao.SelectedValue = rr.CST30MotDesoneracao.ToString();
                            break;
                        case 40:
                            if (rr.CST404150MotDesoneracao != 0)
                                ddlCST405051MovDesoneracao.SelectedValue = rr.CST404150MotDesoneracao.ToString();
                            break;
                        case 41:
                            if (rr.CST404150MotDesoneracao != 0)
                                ddlCST405051MovDesoneracao.SelectedValue = rr.CST404150MotDesoneracao.ToString();
                            break;
                        case 50:
                            if (rr.CST404150MotDesoneracao != 0)
                                ddlCST405051MovDesoneracao.SelectedValue = rr.CST404150MotDesoneracao.ToString();
                            break;
                        case 51:
                            txtCST51RedBC.Text = rr.CST51ReducaoBCICMS.ToString("###,##0.00000");
                            txtCST51Icms.Text = rr.CST51ICMS.ToString("###,##0.00");
                            txtCST51Diferimento.Text = rr.CST51Diferimento.ToString("###,##0.00");
                            break;
                        case 70:
                            txtCST70RedBCIcmsST.Text = rr.CST70ReducaoBCICMSST.ToString("###,##0.00000");
                            txtCST70Icms.Text = rr.CST70ICMS.ToString("###,##0.00");
                            txtCST70RedBCIcmsProprio.Text = rr.CST70ReducaoBCICMSSTProprio.ToString("###,##0.00000");
                            txtCST70IcmsProprio.Text = rr.CST70ICMSProprio.ToString("###,##0.00");
                            txtCST70Mva.Text = rr.CST70MVASaida.ToString("###,##0.00000");
                            if (rr.CST70MotDesoneracao != 0)
                                ddlCST70MovDesoneracao.Text = rr.CST70MotDesoneracao.ToString();
                            break;
                        case 90:
                            txtCST90RedBCIcmsST.Text = rr.CST90ReducaoBCICMSST.ToString("###,##0.00000");
                            txtCST90Icms.Text = rr.CST90ICMS.ToString("###,##0.00");
                            txtCST90RedBCIcmsProprio.Text = rr.CST90ReducaoBCICMSSTProprio.ToString("###,##0.00000");
                            txtCST90IcmsProprio.Text = rr.CST90ICMSProprio.ToString("###,##0.00");
                            txtCST90Mva.Text = rr.CST90MVASaida.ToString("###,##0.00000");
                            chkCST90CalcDifal.Checked = rr.CST90CalculaDifal;
                            chkCST90CalcDifal_CheckedChanged(null, null);
                            chkCST90CalcDifal.Checked = rr.CST90CalculaDifal;

                            if (rr.CST90MotDesoneracao != 0)
                                ddlCST90MovDesoneracao.SelectedValue = rr.CST90MotDesoneracao.ToString();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (rr.CodCST_CSOSN)
                    {

                        case 101:
                            txtCSOSN101IcmsSimples.Text = rr.CSOSN101_ICMS_SIMPLES.ToString("###,##0.00");
                            break;
                        case 201:
                            txtCSOSN201RedBCST.Text = rr.CSOSN201_ReducaoBCICMSST.ToString("###,##0.00000");
                            txtCSOSN202203Icms.Text = rr.CSOSN201_ICMS.ToString("###,##0.00");
                            txtCSOSN201Mva.Text = rr.CSOSN201_MVASaida.ToString("###,##0.00000");
                            txtCSOSN201IcmsSimples.Text = rr.CSOSN201_ICMS_SIMPLES.ToString("###,##0.00");
                            break;
                        case 202:
                            txtCSOSN202203RedBCST.Text = rr.CSOSN202_203_ReducaoBCICMSST.ToString("###,##0.00000");
                            txtCSOSN202203Icms.Text = rr.CSOSN202_203_ICMS.ToString("###,##0.00");
                            txtCSOSN202203IcmsProprio.Text = rr.CSOSN202_203_ICMS_PROPRIO.ToString("###,##0.00");
                            txtCSOSN202203Mva.Text = rr.CSOSN202_203_MVASaida.ToString("###,##0.000000");
                            break;
                        case 203:
                            txtCSOSN202203RedBCST.Text = rr.CSOSN202_203_ReducaoBCICMSST.ToString("###,##0.00000");
                            txtCSOSN202203Icms.Text = rr.CSOSN202_203_ICMS.ToString("###,##0.00");
                            txtCSOSN202203IcmsProprio.Text = rr.CSOSN202_203_ICMS_PROPRIO.ToString("###,##0.00");
                            txtCSOSN202203Mva.Text = rr.CSOSN202_203_MVASaida.ToString("###,##0.000000");
                            break;
                        case 900:
                            txtCSOSN900RedBCIcmsST.Text = rr.CSOSN900_ReducaoBCICMSST.ToString("###,##0.00000");
                            txtCSOSN900Icms.Text = rr.CSOSN900_ICMS.ToString("###,##0.00");
                            txtCSOSN900IcmsProprio.Text = rr.CSOSN900_ICMS_Proprio.ToString("###,##0.00");
                            txtCSOSN900RedBCIcmsProprio.Text = rr.CSOSN900_ReducaoBCICMSProprio.ToString("###,##0.00000");
                            txtCSOSN900Mva.Text = rr.CSOSN900_MVASaida.ToString("###,##0.00000");
                            txtCSOSN900IcmsSimples.Text = rr.CSOSN900_ICMS_SIMPLES.ToString("###,##0.00");
                            break;
                        default:
                            break;
                    }
                }
            }





            if (rr.CodSituacao == 2)
            {
                btnExcluir.Visible = false;
                btnSalvar.Visible = false;
                ddlSituacao.Enabled = false;
                ddlAcao.Visible = false; 
            }

            List<Habil_Log> habillog = new List<Habil_Log>();
            Habil_LogDAL hl = new Habil_LogDAL();

            habillog = hl.ListarLogs(rr.CodigoRegFisIcms, 10000);
            habillog = habillog.OrderByDescending(x => x.DataHora).ToList();

            grdLog.DataSource = habillog; 
            grdLog.DataBind();




        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(),MessageType.Info);
                Session["MensagemTela"] = "";
            }


            

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConRegFisIcms.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {

                if (Session["ZoomRegFisIcms2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomRegFisIcms"] != null)
                {
                    string s = Session["ZoomRegFisIcms"].ToString();
                    Session["ZoomRegFisIcms"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                        {

                            long IdCodRegFisIcms = Convert.ToInt64(word);
                            txtCodRegra.Text = IdCodRegFisIcms.ToString();
                            MontaTela(IdCodRegFisIcms);


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
                    LimpaTela();
                    txtCodRegra.Text = "Novo";
                    btnExcluir.Visible = false;
                    pnlAlterar.Visible = false; 
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                            if (!p.AcessoExcluir)
                                btnExcluir.Visible = false;
                        }
                    });

                }

            }
            if(txtCodRegra.Text == "")
                btnVoltar_Click(sender, e);
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            DBTabelaDAL tid = new DBTabelaDAL();
            Habil_Log hl = new Habil_Log();
            Habil_LogDAL hldal = new Habil_LogDAL();

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

            RegFisIcmsDAL r = new RegFisIcmsDAL();
            RegFisIcms p = new RegFisIcms();

            if (txtCodRegra.Text != "")
            {
                r.InativarRegra(Convert.ToInt64(txtCodRegra.Text));


                hl = new Habil_Log();

                hl.CodigoIdentificador = Convert.ToInt64(txtCodRegra.Text);
                hl.CodigoTabelaCampo = 0;
                hl.CodigoOperacao = 2; //Inativação da Regra
                hl.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());

                he = hedal.PesquisarNomeHabil_Estacao( Session["Estacao_Logada"].ToString() );
                if (he != null)
                    hl.CodigoEstacao = he.CodigoEstacao;

                hl.DescricaoLog = "Inativação (A Força) da Regra Fiscal Icms: " + Convert.ToString(txtCodRegra.Text);

                hldal.Inserir(hl);

                Session["MensagemTela"] = "Regra Inativada com Sucesso!!!";

                btnVoltar_Click(sender, e);

            }

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Impostos/ConRegFisIcms.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            RegFisIcmsDAL r = new RegFisIcmsDAL();

            List<TabIcms> t1 = new List<TabIcms>();
            List<GpoTribPessoa> t2 = new List<GpoTribPessoa>();
            List<GpoTribProduto> t3 = new List<GpoTribProduto>();
            List<Habil_AplicacaoUso> t4 = new List<Habil_AplicacaoUso>();
            List<Habil_TipoOperacaoFiscal> t5 = new List<Habil_TipoOperacaoFiscal>();

            List<RegFisIcmsLocalizacao> l1 = new List<RegFisIcmsLocalizacao>();
            List<RegFisIcmsGpoTribPessoa> l2 = new List<RegFisIcmsGpoTribPessoa>();
            List<RegFisIcmsGpoTribProduto> l3 = new List<RegFisIcmsGpoTribProduto>();
            List<RegFisIcmsHabil_AplicacaoUso> l4 = new List<RegFisIcmsHabil_AplicacaoUso>();
            List<RegFisIcmsTipoOperFiscal> l5 = new List<RegFisIcmsTipoOperFiscal>();

            RegFisIcmsLocalizacao p1 = new RegFisIcmsLocalizacao();
            RegFisIcmsGpoTribPessoa p2 = new RegFisIcmsGpoTribPessoa();
            RegFisIcmsGpoTribProduto p3 = new RegFisIcmsGpoTribProduto();
            RegFisIcmsHabil_AplicacaoUso p4 = new RegFisIcmsHabil_AplicacaoUso();
            RegFisIcmsTipoOperFiscal p5 = new RegFisIcmsTipoOperFiscal();

            Habil_Log hl = new Habil_Log();
            Habil_LogDAL hldal = new Habil_LogDAL();
            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();


            if (ValidaCampos() == false)
                return;

            if (Session["ListaLocalizacaoIcms"] != null)
                t1 = (List<TabIcms>)Session["ListaLocalizacaoIcms"];
            else
                t1 = new List<TabIcms>();

            foreach (TabIcms item in t1)
            {
                p1 = new RegFisIcmsLocalizacao
                {
                    CodigoRegFisIcms = 1,
                    CodLocalizacaoUF = Convert.ToInt32(item.CodTabAliqIcms)
                };
                l1.Add(p1);
            }
            ///////////////////////////////////////////////////////////////////////////


            if (Session["ListaGpoTribPesIcms"] != null)
                t2= (List<GpoTribPessoa>)Session["ListaGpoTribPesIcms"];
            else
                t2 = new List<GpoTribPessoa>();

            foreach (GpoTribPessoa item in t2)
            {
                p2 = new RegFisIcmsGpoTribPessoa
                {
                    CodigoRegFisIcms = 1,
                    CodGpoTribPessoa = Convert.ToInt32(item.CodigoGpoTribPessoa)
                };
                l2.Add(p2);
            }
            ///////////////////////////////////////////////////////////////////////////


            if (Session["ListaGpoTribProIcms"] != null)
                t3 = (List<GpoTribProduto>)Session["ListaGpoTribProIcms"];
            else
                t3 = new List<GpoTribProduto>();

            foreach (GpoTribProduto item in t3)
            {
                p3 = new RegFisIcmsGpoTribProduto
                {
                    CodigoRegFisIcms = 1,
                    CodGpoTribProduto= Convert.ToInt32(item.CodigoGpoTribProduto)
                };
                l3.Add(p3);
            }
            ///////////////////////////////////////////////////////////////////////////
            

            if (Session["ListaAplicaUsoIcms"] != null)
                t4 = (List<Habil_AplicacaoUso>)Session["ListaAplicaUsoIcms"];
            else
                t4 = new List<Habil_AplicacaoUso>();

            foreach (Habil_AplicacaoUso item in t4)
            {
                p4 = new RegFisIcmsHabil_AplicacaoUso
                {
                    CodigoRegFisIcms = 1,
                    CodHabil_AplicacaoUso = Convert.ToInt32(item.CodigoHabil_AplicacaoUso)
                };
                l4.Add(p4);
            }
            ///////////////////////////////////////////////////////////////////////////


            if (Session["ListaOperFiscalIcms"] != null)
                t5= (List<Habil_TipoOperacaoFiscal>)Session["ListaOperFiscalIcms"];
            else
                t5 = new List<Habil_TipoOperacaoFiscal>();

            foreach (Habil_TipoOperacaoFiscal item in t5)
            {
                p5 = new RegFisIcmsTipoOperFiscal
                {
                    CodigoRegFisIcms = 1,
                    CodTipoOperFiscal = Convert.ToInt32(item.CodigoHabil_TipoOperFiscal)
                };
                l5.Add(p5);
            }

            ///////////////////////////////////////////////////////////////////////////
            //Critica    
            int CodRegra = 0;
            DataTable dt = new DataTable();

            dt.Columns.Add("CodigoRegra", typeof(Int32));
            dt.Columns.Add("DataVigencia", typeof(DateTime));
            dt.Columns.Add("CodigoLocalizacao", typeof(Int32));
            dt.Columns.Add("CodigoGpoPessoa", typeof(Int32));
            dt.Columns.Add("CodigoGpoProduto", typeof(Int32));
            dt.Columns.Add("CodigoAplicacao", typeof(Int32));
            dt.Columns.Add("CodigoTipoOperacao", typeof(Int32));
            string strMsgCritica = "";

            foreach (var item in l1)
            {
                foreach (var item2 in l2)
                {
                    foreach (var item3 in l3)
                    {
                        foreach (var item4 in l4)
                        {
                            foreach (var item5 in l5)
                            {
                                if (txtCodRegra.Text != "Novo")
                                    CodRegra = Convert.ToInt32(txtCodRegra.Text);
                                dt.Clear();
                                dt.Rows.Add(CodRegra,  Convert.ToDateTime(txtdtinicial.Text) , item.CodLocalizacaoUF, item2.CodGpoTribPessoa, item3.CodGpoTribProduto, item4.CodHabil_AplicacaoUso, item5.CodTipoOperFiscal);
                                strMsgCritica = r.ValidarCombinacao(dt);
                                if (strMsgCritica != "")
                                {
                                    ShowMessage(strMsgCritica, MessageType.Info);
                                    return;
                                }
                            }
                        }
                    }
                }
            }


            ///////////////////////////////////////////////////////////////////////////

            DBTabelaDAL tid = new DBTabelaDAL();

            RegFisIcms p = new RegFisIcms();
            p.CodigoRegFisIcmsAnterior = 0;
            p.CodLog = 0;

            if (ddlAcao.SelectedValue == "2")
            {
                p.CodigoRegFisIcmsAnterior = Convert.ToInt64(txtCodRegra.Text);

                if (p.CodigoRegFisIcmsAnterior != 0)
                {
                    r.InativarRegra(p.CodigoRegFisIcmsAnterior);


                    hl = new Habil_Log();
                    hl.CodigoIdentificador = p.CodigoRegFisIcmsAnterior;
                    hl.CodigoTabelaCampo = 0;
                    hl.CodigoOperacao = 2; //Inativação da Regra
                    hl.CodigoUsuario = Convert.ToInt64(Session["CodUsuario"].ToString());


                    he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
                    if (he != null)
                        hl.CodigoEstacao = he.CodigoEstacao;

                    hl.DescricaoLog = "Regra: " + Convert.ToString(p.CodigoRegFisIcmsAnterior);

                    hldal.Inserir(hl);
                    txtCodRegra.Text = "Novo";
                }

            }



            if (txtCodRegra.Text != "Novo")
                p.CodigoRegFisIcms = Convert.ToInt32(txtCodRegra.Text);
            else
                p.CodigoRegFisIcms = 0;

            p.DataVigencia = Convert.ToDateTime(txtdtinicial.Text);
            p.DataAtualizacao = DateTime.Now;
            p.CodSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);

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
            p.Descricao = txtDescricao.Text;

            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            r.SalvarRegra(p, l1, l2, l3, l4, l5, Convert.ToInt32(he.CodigoEstacao), Convert.ToInt32(Session["CodUsuario"].ToString()));

            if (txtCodRegra.Text == "Novo")
            {
                hl = new Habil_Log();
                hl.CodigoIdentificador = p.CodigoRegFisIcms;
                hl.CodigoTabelaCampo = 0;
                hl.CodigoOperacao = 3; //Criação da Regra
                hl.DescricaoLog = "Regra: " + Convert.ToString(p.CodigoRegFisIcms);
                hl.CodigoUsuario = Convert.ToInt64(Session["CodUsuario"].ToString());
                if (he != null)
                    hl.CodigoEstacao = he.CodigoEstacao;
                hldal.Inserir(hl);
            }

            if (ddlAcao.SelectedValue == "0")
            {
                Session["MensagemTela"] = "Regra Criada com Sucesso!!!";
                btnVoltar_Click(sender, e);
            }
            else
            {
                if (txtCodRegra.Text == "Novo")
                    ShowMessage("Regra Criada com Sucesso!!!", MessageType.Success);
                else
                    ShowMessage("Regra Alterada com Sucesso!!!", MessageType.Success);

                txtCodRegra.Text = p.CodigoRegFisIcms.ToString();
                MontaTela(p.CodigoRegFisIcms);
            }
        }
        protected void BtnAddLocalizacao_Click(object sender, EventArgs e)
        {
            TabIcmsDAL r = new TabIcmsDAL();
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();
            List<TabIcms> ListTabIcms = new List<TabIcms>();
            List<TabIcms> ListTabIcmsLocal = new List<TabIcms>();

            if ((ddlEstadoOrigem.Text == "*Nenhum Selecionado") || (ddlEstadoDestino.Text == "*Nenhum Selecionado"))
                return;


            DBTabelaCampos x = new DBTabelaCampos();
            x.Filtro = "CD_EST_ORIGEM";
            x.Tipo = "smallint";
            x.Inicio = ddlEstadoOrigem.SelectedValue;
            x.Fim = ddlEstadoOrigem.SelectedValue;
            lista.Add(x);

            x = new DBTabelaCampos();
            x.Filtro = "CD_EST_DESTINO";
            x.Tipo = "smallint";
            x.Inicio = ddlEstadoDestino.SelectedValue;
            x.Fim = ddlEstadoDestino.SelectedValue;
            lista.Add(x);

            if (Session["ListaLocalizacaoIcms"] != null)
                ListTabIcms = (List<TabIcms>)Session["ListaLocalizacaoIcms"];
            else
                ListTabIcms = new List<TabIcms>();

            ListTabIcmsLocal = r.ListarTabIcmsCompleto(lista);

            TabIcms tabi;

            foreach (TabIcms item in ListTabIcms)
            {
                foreach (TabIcms item2 in ListTabIcmsLocal)
                {
                    if (item2.CodTabAliqIcms == item.CodTabAliqIcms)
                    {
                        goto ItemEncontrado;
                    }
                }

                tabi = new TabIcms();
                tabi.CodTabAliqIcms = item.CodTabAliqIcms;
                tabi.CodDestino = item.CodDestino;
                tabi.CodOrigem = item.CodOrigem;
                tabi.IcmsExterno = item.IcmsExterno;
                tabi.IcmsInterEstadual = item.IcmsInterEstadual;
                tabi.IcmsInterno = item.IcmsInterno;
                tabi.UFDestino = item.UFDestino;
                tabi.UFOrigem = item.UFOrigem;
                ListTabIcmsLocal.Add(tabi);

                ItemEncontrado:;
            }
 
            grdLocalizacao.DataSource = ListTabIcmsLocal;
            grdLocalizacao.DataBind();


            Session["ListaLocalizacaoIcms"] = ListTabIcmsLocal;
        }
        protected void BtnGpoTribPessoa_Click(object sender, EventArgs e)
        {
            GpoTribPessoaDAL r = new GpoTribPessoaDAL();
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();
            List<GpoTribPessoa> ListTabIcms = new List<GpoTribPessoa>();
            List<GpoTribPessoa> ListTabIcmsLocal = new List<GpoTribPessoa>();

            if (ddlGpoTribPessoa.Text == "*Nenhum Selecionado")
                return;


            DBTabelaCampos x = new DBTabelaCampos();
            x.Filtro = "CD_GPO_TRIB_PESSOA";
            x.Tipo = "smallint";
            x.Inicio = ddlGpoTribPessoa.SelectedValue;
            x.Fim = ddlGpoTribPessoa.SelectedValue;
            lista.Add(x);

            if (Session["ListaGpoTribPesIcms"] != null)
                ListTabIcms = (List<GpoTribPessoa>)Session["ListaGpoTribPesIcms"];
            else
                ListTabIcms = new List<GpoTribPessoa>();

            ListTabIcmsLocal = r.ListarGpoTribPessoasCompleto(lista);

            GpoTribPessoa tabi;

            foreach (GpoTribPessoa item in ListTabIcms)
            {
                foreach (GpoTribPessoa item2 in ListTabIcmsLocal)
                {
                    if (item2.CodigoGpoTribPessoa== item.CodigoGpoTribPessoa)
                    {
                        goto ItemEncontrado;
                    }
                }

                tabi = new GpoTribPessoa();
                tabi.CodigoGpoTribPessoa = item.CodigoGpoTribPessoa;
                tabi.DescricaoGpoTribPessoa = item.DescricaoGpoTribPessoa;
                ListTabIcmsLocal.Add(tabi);

                ItemEncontrado:;
            }

            grdGpoTribPessoa.DataSource = ListTabIcmsLocal;
            grdGpoTribPessoa.DataBind();

            Session["ListaGpoTribPesIcms"] = ListTabIcmsLocal;

        }
        protected void BtnGpoTribProduto_Click(object sender, EventArgs e)
        {
            GpoTribProdutoDAL r = new GpoTribProdutoDAL();
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();
            List<GpoTribProduto> ListTabIcms = new List<GpoTribProduto>();
            List<GpoTribProduto> ListTabIcmsLocal = new List<GpoTribProduto>();

            if (ddlGpoTribProduto.Text == "*Nenhum Selecionado")
                return;

            DBTabelaCampos x = new DBTabelaCampos();
            x.Filtro = "CD_GPO_TRIB_Produto";
            x.Tipo = "smallint";
            x.Inicio = ddlGpoTribProduto.SelectedValue;
            x.Fim = ddlGpoTribProduto.SelectedValue;
            lista.Add(x);

            if (Session["ListaGpoTribProIcms"] != null)
                ListTabIcms = (List<GpoTribProduto>)Session["ListaGpoTribProIcms"];
            else
                ListTabIcms = new List<GpoTribProduto>();

            ListTabIcmsLocal = r.ListarGpoTribProdutosCompleto(lista);

            GpoTribProduto tabi;

            foreach (GpoTribProduto item in ListTabIcms)
            {
                foreach (GpoTribProduto item2 in ListTabIcmsLocal)
                {
                    if (item2.CodigoGpoTribProduto == item.CodigoGpoTribProduto)
                    {
                        goto ItemEncontrado;
                    }
                }

                tabi = new GpoTribProduto();
                tabi.CodigoGpoTribProduto = item.CodigoGpoTribProduto;
                tabi.DescricaoGpoTribProduto = item.DescricaoGpoTribProduto;
                ListTabIcmsLocal.Add(tabi);

                ItemEncontrado:;
            }

            grdGpoTribProduto.DataSource = ListTabIcmsLocal;
            grdGpoTribProduto.DataBind();

            Session["ListaGpoTribProIcms"] = ListTabIcmsLocal;

        }
        protected void BtnAplicacaoUso_Click(object sender, EventArgs e)
        {
            Habil_AplicacaoUsoDAL r = new Habil_AplicacaoUsoDAL();
            List<Habil_AplicacaoUso> ListTabIcms = new List<Habil_AplicacaoUso>();
            List<Habil_AplicacaoUso> ListTabIcmsLocal = new List<Habil_AplicacaoUso>();

            if (ddlAplicacaoUso.Text == "*Nenhum Selecionado")
                return;


            if (Session["ListaAplicaUsoIcms"] != null)
                ListTabIcms = (List<Habil_AplicacaoUso>)Session["ListaAplicaUsoIcms"];
            else
                ListTabIcms = new List<Habil_AplicacaoUso>();

            ListTabIcmsLocal = r.ListaHabil_AplicacaoUso(Convert.ToInt32(ddlAplicacaoUso.SelectedValue));

            Habil_AplicacaoUso tabi;

            foreach (Habil_AplicacaoUso item in ListTabIcms)
            {
                foreach (Habil_AplicacaoUso item2 in ListTabIcmsLocal)
                {
                    if (item2.CodigoHabil_AplicacaoUso == item.CodigoHabil_AplicacaoUso)
                    {
                        goto ItemEncontrado;
                    }
                }

                tabi = new Habil_AplicacaoUso();
                tabi.CodigoHabil_AplicacaoUso = item.CodigoHabil_AplicacaoUso;
                tabi.DescricaoHabil_AplicacaoUso = item.DescricaoHabil_AplicacaoUso;
                ListTabIcmsLocal.Add(tabi);

                ItemEncontrado:;
            }

            grdAplicacaoUso.DataSource = ListTabIcmsLocal;
            grdAplicacaoUso.DataBind();

            Session["ListaAplicaUsoIcms"] = ListTabIcmsLocal;


        }
        protected void btnOperFiscal_Click(object sender, EventArgs e)
        {
            Habil_TipoOperacaoFiscalDAL r = new Habil_TipoOperacaoFiscalDAL();
            List<Habil_TipoOperacaoFiscal> ListTabIcms = new List<Habil_TipoOperacaoFiscal>();
            List<Habil_TipoOperacaoFiscal> ListTabIcmsLocal = new List<Habil_TipoOperacaoFiscal>();

            if (ddlOperFiscal.Text == "*Nenhum Selecionado")
                return;


            if (Session["ListaOperFiscalIcms"] != null)
                ListTabIcms = (List<Habil_TipoOperacaoFiscal>)Session["ListaOperFiscalIcms"];
            else
                ListTabIcms = new List<Habil_TipoOperacaoFiscal>();

            ListTabIcmsLocal = r.ListaOperacaoFiscal(Convert.ToInt32(ddlOperFiscal.SelectedValue));

            Habil_TipoOperacaoFiscal tabi;

            foreach (Habil_TipoOperacaoFiscal item in ListTabIcms)
            {
                foreach (Habil_TipoOperacaoFiscal item2 in ListTabIcmsLocal)
                {
                    if (item2.CodigoHabil_TipoOperFiscal == item.CodigoHabil_TipoOperFiscal)
                    {
                        goto ItemEncontrado;
                    }
                }

                tabi = new Habil_TipoOperacaoFiscal();
                tabi.CodigoHabil_TipoOperFiscal = item.CodigoHabil_TipoOperFiscal;
                tabi.DescricaoHabil_TipoOperFiscal = item.DescricaoHabil_TipoOperFiscal;
                ListTabIcmsLocal.Add(tabi);

                ItemEncontrado:;
            }

            grdOperFiscal.DataSource = ListTabIcmsLocal;
            grdOperFiscal.DataBind();

            Session["ListaOperFiscalIcms"] = ListTabIcmsLocal;



        }
        protected void grdLocalizacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<TabIcms> ListTabIcms = new List<TabIcms>();
            List<TabIcms> ListTabIcmsLocal = new List<TabIcms>();


            if (Session["ListaLocalizacaoIcms"] != null)
                ListTabIcms = (List<TabIcms>)Session["ListaLocalizacaoIcms"];
            else
                ListTabIcms = new List<TabIcms>();

            TabIcms tabi;

            foreach (TabIcms item in ListTabIcms)
            {
                
                if (item.CodTabAliqIcms != Convert.ToInt32(HttpUtility.HtmlDecode(grdLocalizacao.SelectedRow.Cells[0].Text)))
                {
                    tabi = new TabIcms();
                    tabi.CodTabAliqIcms = item.CodTabAliqIcms;
                    tabi.CodDestino = item.CodDestino;
                    tabi.CodOrigem = item.CodOrigem;
                    tabi.IcmsExterno = item.IcmsExterno;
                    tabi.IcmsInterEstadual = item.IcmsInterEstadual;
                    tabi.IcmsInterno = item.IcmsInterno;
                    tabi.UFDestino = item.UFDestino;
                    tabi.UFOrigem = item.UFOrigem;
                    ListTabIcmsLocal.Add(tabi);
                }


            }

            grdLocalizacao.DataSource = ListTabIcmsLocal;
            grdLocalizacao.DataBind();
            Session["ListaLocalizacaoIcms"] = ListTabIcmsLocal;
        }
        protected void grdGpoTribPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<GpoTribPessoa> ListGpoTribPessoa = new List<GpoTribPessoa>();
            List<GpoTribPessoa> ListGpoTribPessoaLocal = new List<GpoTribPessoa>();


            if (Session["ListaGpoTribPesIcms"] != null)
                ListGpoTribPessoa = (List<GpoTribPessoa>)Session["ListaGpoTribPesIcms"];
            else
                ListGpoTribPessoa = new List<GpoTribPessoa>();

            GpoTribPessoa tabi;

            foreach (GpoTribPessoa item in ListGpoTribPessoa)
            {

                if (item.CodigoGpoTribPessoa != Convert.ToInt32(HttpUtility.HtmlDecode(grdGpoTribPessoa.SelectedRow.Cells[0].Text)))
                {
                    tabi = new GpoTribPessoa();
                    tabi.CodigoGpoTribPessoa = item.CodigoGpoTribPessoa;
                    tabi.DescricaoGpoTribPessoa = item.DescricaoGpoTribPessoa;
                    ListGpoTribPessoaLocal.Add(tabi);
                }


            }

            grdGpoTribPessoa.DataSource = ListGpoTribPessoaLocal;
            grdGpoTribPessoa.DataBind();
            Session["ListaGpoTribPesIcms"] = ListGpoTribPessoaLocal;

        }
        protected void grdGpoTribProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<GpoTribProduto> ListGpoTribProduto = new List<GpoTribProduto>();
            List<GpoTribProduto> ListGpoTribProdutoLocal = new List<GpoTribProduto>();


            if (Session["ListaGpoTribProIcms"] != null)
                ListGpoTribProduto = (List<GpoTribProduto>)Session["ListaGpoTribProIcms"];
            else
                ListGpoTribProduto = new List<GpoTribProduto>();

            GpoTribProduto tabi;

            foreach (GpoTribProduto item in ListGpoTribProduto)
            {

                if (item.CodigoGpoTribProduto != Convert.ToInt32(HttpUtility.HtmlDecode(grdGpoTribProduto.SelectedRow.Cells[0].Text)))
                {
                    tabi = new GpoTribProduto();
                    tabi.CodigoGpoTribProduto = item.CodigoGpoTribProduto;
                    tabi.DescricaoGpoTribProduto = item.DescricaoGpoTribProduto;
                    ListGpoTribProdutoLocal.Add(tabi);
                }


            }

            grdGpoTribProduto.DataSource = ListGpoTribProdutoLocal;
            grdGpoTribProduto.DataBind();
            Session["ListaGpoTribProIcms"] = ListGpoTribProdutoLocal;

        }
        protected void grdAplicacaoUso_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Habil_AplicacaoUso> ListAplicacaoUso = new List<Habil_AplicacaoUso>();
            List<Habil_AplicacaoUso> ListAplicacaoUsoLocal = new List<Habil_AplicacaoUso>();


            if (Session["ListaAplicaUsoIcms"] != null)
                ListAplicacaoUso = (List<Habil_AplicacaoUso>)Session["ListaAplicaUsoIcms"];
            else
                ListAplicacaoUso = new List<Habil_AplicacaoUso>();

            Habil_AplicacaoUso tabi;

            foreach (Habil_AplicacaoUso item in ListAplicacaoUso)
            {

                if (item.CodigoHabil_AplicacaoUso != Convert.ToInt32(HttpUtility.HtmlDecode(grdAplicacaoUso.SelectedRow.Cells[0].Text)))
                {
                    tabi = new Habil_AplicacaoUso();
                    tabi.CodigoHabil_AplicacaoUso = item.CodigoHabil_AplicacaoUso;
                    tabi.DescricaoHabil_AplicacaoUso = item.DescricaoHabil_AplicacaoUso;
                    ListAplicacaoUsoLocal.Add(tabi);
                }


            }

            grdAplicacaoUso.DataSource = ListAplicacaoUsoLocal;
            grdAplicacaoUso.DataBind();
            Session["ListaAplicaUsoIcms"] = ListAplicacaoUsoLocal;


        }
        protected void grdOperFiscal_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Habil_TipoOperacaoFiscal> ListOperacaoFiscal = new List<Habil_TipoOperacaoFiscal>();
            List<Habil_TipoOperacaoFiscal> ListOperacaoFiscalLocal = new List<Habil_TipoOperacaoFiscal>();


            if (Session["ListaOperFiscalIcms"] != null)
                ListOperacaoFiscal = (List<Habil_TipoOperacaoFiscal>)Session["ListaOperFiscalIcms"];
            else
                ListOperacaoFiscal = new List<Habil_TipoOperacaoFiscal>();

            Habil_TipoOperacaoFiscal tabi;

            foreach (Habil_TipoOperacaoFiscal item in ListOperacaoFiscal)
            {

                if (item.CodigoHabil_TipoOperFiscal != Convert.ToInt32(HttpUtility.HtmlDecode(grdOperFiscal.SelectedRow.Cells[0].Text)))
                {
                    tabi = new Habil_TipoOperacaoFiscal();
                    tabi.CodigoHabil_TipoOperFiscal = item.CodigoHabil_TipoOperFiscal;
                    tabi.DescricaoHabil_TipoOperFiscal = item.DescricaoHabil_TipoOperFiscal;
                    ListOperacaoFiscalLocal.Add(tabi);
                }


            }

            grdOperFiscal.DataSource = ListOperacaoFiscalLocal;
            grdOperFiscal.DataBind();
            Session["ListaOperFiscalIcms"] = ListOperacaoFiscalLocal;

        }
        protected void ddlEstadoOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlEstadoDestino_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlGpoTribPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlGpoTribProduto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlAplicacaoUso_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlOperFiscal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
        protected void CarregaDropDown()
        {
            Habil_RegTributarioDAL rtd = new Habil_RegTributarioDAL();
            ddlRegTributario.DataSource = rtd.ListaHabil_RegTributario();
            ddlRegTributario.DataTextField = "DescricaoHabil_RegTributario";
            ddlRegTributario.DataValueField = "CodigoHabil_RegTributario";
            ddlRegTributario.DataBind();
            ddlRegTributario.Items.Insert(0, "*Nenhum Selecionado");

            CarregaDropDownDesoneracao();


            pnlImpostos.Visible = false;
        }
        protected void CarregaDropDownDesoneracao()
        {
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
        protected void grdLocalizacao_Sorting(object sender, GridViewSortEventArgs e)
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

            List<TabIcms> ListOfInt = new List<TabIcms>();
            ListOfInt = (List<TabIcms>)Session["ListaLocalizacaoIcms"];
            // populate list
            DataTable ListAsDataTable = BuildDataTable<TabIcms>(ListOfInt);
            DataView ListAsDataView = ListAsDataTable.DefaultView;

            ListAsDataView.Sort = e.SortExpression + " " + sortingDirection;
            grdLocalizacao.DataSource = ListAsDataView;
            grdLocalizacao.DataBind();

            for (int i = 0; i < grdLocalizacao.Columns.Count; i++)
            {
                if (grdLocalizacao.Columns[i].SortExpression == e.SortExpression)
                {
                    var cell = grdLocalizacao.HeaderRow.Cells[i];
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
            get
            {

                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["dirState"];

            }

            set
            {
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

                if (prop != null)

                {
                    tbl.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

            }
            return tbl;
        }

        protected void ddlAlteracao_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlAplicacao.Enabled = false;
            pnlGrupos.Enabled = false;
            pnlRegra.Enabled = false;

            if (ddlAlteracao.SelectedIndex.ToString() == "1")
            {
                pnlAplicacao.Enabled = false;
                pnlGrupos.Enabled = false;
                pnlRegra.Enabled = true;
                ddlAlteracao.Enabled = false;
            }

            if (ddlAlteracao.SelectedIndex.ToString() == "2")
            {
                pnlRegra.Enabled = false;
                pnlGrupos.Enabled = true;
                pnlAplicacao.Enabled = false;
                ddlAlteracao.Enabled = false;
            }

            if (ddlAlteracao.SelectedIndex.ToString() == "3")
            {
                pnlGrupos.Enabled = false;
                pnlRegra.Enabled = false;
                pnlAplicacao.Enabled = true;
                ddlAlteracao.Enabled = false;
            }
            txtdtinicial.Enabled = true;

        }
    }
}