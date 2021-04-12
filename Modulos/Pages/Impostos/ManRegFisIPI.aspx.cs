using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Impostos
{
    public partial class ManRegFisIPI : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Data de Vigência", txtDtVigencia.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodNcm.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodNcm.Focus();

                }
                return false;
            }
            if (txtDtVigencia.Text == "")
            {
                ShowMessage("Data de Vigência deve ser Informado", MessageType.Info);
                txtDtVigencia.Focus();
                return false;
            }
            if (txtCodTrib.Text != "")
            {
                v.CampoValido("Código de Situaçãp de Tributação", txtCodTrib.Text, false, true, true, false, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    txtCodTrib.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtCodTrib.Focus();
                    }
                    return false;
                }
            }
            if (txtCodTrib.Text == "")
            {
                ShowMessage("Código de Situação Tributária deve ser Informado", MessageType.Info);
                txtCodTrib.Focus();
                return false;
            }
            v.CampoValido("Código do NCM", txtCodNcm.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodNcm.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodNcm.Focus();

                }
                return false;
            }
            if (txtCodNcm.Text == "")
            {
                ShowMessage("Código de NCM deve ser Informado", MessageType.Info);
                txtCodNcm.Focus();
                return false;
            }
            if (txtDsNcm.Text.Length > 255)
            {
                ShowMessage("Descrição de NCM com número de caracteres Excedido", MessageType.Info);
                txtDsNcm.Focus();
            }
            v.CampoValido("Porcentagem do IPI", txtVlPctIpi.Text, true, false, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtVlPctIpi.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtVlPctIpi.Focus();
                }
                return false;
            }
            if (txtVlPctIpi.Text == "")
            {
                ShowMessage("Porcentagem do IPI deve ser Informado", MessageType.Info);
                txtVlPctIpi.Focus();
                return false;
            }
            if (txtCodEx.Text != "")
            {
                v.CampoValido("Código de Excessão", txtCodEx.Text, false, true, true, false, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    txtCodEx.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtCodEx.Focus();
                    }
                    return false;
                }
            }
            return true;
        }
        protected void LimpaCampos()
        {
            DBTabelaDAL dbTDAL = new DBTabelaDAL();
            txtDtAtualizacao.Text = Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("dd/MM/yyyy"));
            txtDtVigencia.Text = Convert.ToDateTime(txtDtAtualizacao.Text).ToString("dd/MM/yyyy");
            txtCodNcm.Text = "";
            txtCodRegra.Text = "Novo";
            TxtEx.Text = "Ex:";
            txtCodEx.Text = "";
            txtDsNcm.Text = "";
            txtEnquadramento.Text = "";
            txtVlPctIpi.Text = "0,00";

            CarregaSituacoes();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["ZoomRegraIPI2"] != null)
            {
                if (Session["ZoomRegraIPI2"].ToString() == "RELACIONAL")
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
                cmdSair.Visible = false;
            }

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = "";
            }

            

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt16(Session["CodModulo"].ToString()),
                                           Convert.ToInt16(Session["CodPflUsuario"].ToString()),
                                           "ConRegFisIPI.aspx");
            if (Session["PrimeiraVezIPI"] == null)
            {
                Session["PrimeiraVezIPI"] = "";
                LimpaCampos();
            }
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomRegraIPI2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomRegraIPI"] != null)
                {
                    
                    string s = Session["ZoomRegraIPI"].ToString();
                    Session["ZoomRegraIPI"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        txtCodRegra.Text = "";
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodRegra.Text == "")
                            {
                                txtCodRegra.Text = word;
                                txtCodRegra.Enabled = false;

                                RegraFisIPIDAL r = new RegraFisIPIDAL();
                                RegFisIPI p = new RegFisIPI();

                                CarregaSituacoes();

                                p = r.PesquisarIPIPorRegra(Convert.ToDecimal(txtCodRegra.Text));
                                
                                txtDtVigencia.Text = p.DtVigencia.ToString();
                                txtDtAtualizacao.Text = p.DtAtualizacao.ToString();
                                txtVlPctIpi.Text = p.PercentualIPI.ToString("###,##0.00");
                                txtEnquadramento.Text = p.CodigoEnquadramento.ToString();
                                ddlTipo.SelectedValue = p.CodigoTipo.ToString();
                                ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                txtCodNcm.Text = p.CodigoNCM.ToString();
                                txtDsNcm.Text = p.DescricaoNCM.ToString();
                                txtCodTrib.Text = p.CodigoSituacaoTributaria.ToString();


                                lista.ForEach(delegate (Permissao x)
                                {
                                    if (!x.AcessoCompleto)
                                    {
                                        if (!x.AcessoAlterar)
                                            btnSalvar.Visible = false;

                                        if (!x.AcessoExcluir)
                                            btnExcluir.Visible = false;
                                    }
                                }
                                );

                                return;
                            }
                        }
                    }
                else
                {

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
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            RegFisIPI p = new RegFisIPI();
            RegFisIPI ep = new RegFisIPI();
            RegraFisIPIDAL RnReg = new RegraFisIPIDAL(); 

            if (txtCodTrib.Text != "")
                p.CodigoSituacaoTributaria = Convert.ToInt16(txtCodTrib.Text);

            if (txtCodRegra.Text != "Novo")
                p.CodigoRegraFiscalIPI = Convert.ToInt32(txtCodRegra.Text);

            p.DtVigencia = Convert.ToDateTime(txtDtVigencia.Text);
            p.PercentualIPI = Convert.ToDecimal(txtVlPctIpi.Text);
            p.CodigoEnquadramento = Convert.ToInt16(txtEnquadramento.Text);
            p.CodigoTipo = Convert.ToInt32(ddlTipo.SelectedValue);
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.CodigoNCM = txtCodNcm.Text;
            p.DescricaoNCM = txtDsNcm.Text;
            p.CodigoEx = txtCodEx.Text;

            ep = RnReg.PesquisarIPI(p.DtVigencia, p.CodigoNCM, p.CodigoEx, p.CodigoRegraFiscalIPI);

            if (ep.CodigoRegraFiscalIPI > 0)
            {
                ShowMessage("Regra Fiscal IPI já Cadastrada", MessageType.Info);
                return;
            }

            if (txtCodRegra.Text == "Novo")
            {
                RnReg.Inserir(p);
                Session["MensagemTela"] = "Regra Fiscal Inserida com Sucesso!";
            }
            else
            {
                RnReg.Atualizar(p);
                Session["MensagemTela"] = "Regra Fiscal Atualizada com Sucesso!";
            }
            btnVoltar_Click(sender, e);
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["PrimeiraVezIPI"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/Impostos/ConRegFisIPI.aspx");
            this.Dispose();
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            RegraFisIPIDAL RnReg = new RegraFisIPIDAL();

            RnReg.Excluir(Convert.ToInt32(txtCodRegra.Text));
            Session["MensagemTela"] = "Regra Fiscal Excluída com Sucesso!";
            btnVoltar_Click(sender, e);
        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (txtCodRegra.Text == "Novo")
                return;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "abc();", true);
        }
        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            ddlTipo.DataSource = sd.TipoTributacao();
            ddlTipo.DataTextField = "DescricaoTipo";
            ddlTipo.DataValueField = "CodigoTipo";
            ddlTipo.DataBind();
            
        }
        protected void txtVlPctIpi_TextChanged(object sender, EventArgs e)
        {
            if (txtVlPctIpi.Text.Equals(""))
                txtVlPctIpi.Text = "0,00";
            else
            {
                txtVlPctIpi.Text = Convert.ToDouble(txtVlPctIpi.Text).ToString("###,##0.00");
                txtVlPctIpi.Focus();
            }
        }
    }
}