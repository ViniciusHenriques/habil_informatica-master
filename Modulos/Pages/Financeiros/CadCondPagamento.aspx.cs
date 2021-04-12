using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class CadCondPagamento : System.Web.UI.Page
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

            v.CampoValido("Descrição da Condição de Pagamento", txtDescricao.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDescricao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR,MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }

            if (DdlFormaPagamento.Text == "..... SELECIONE UMA FORMA DE PAGAMENTO .....")
            {
                ShowMessage("Selecione uma Forma de Pagamento.", MessageType.Info);
                return false;
            }

            if (DdlTipoCobranca.Text =="..... SELECIONE UM TIPO DE COBRANÇA .....")
            {
                ShowMessage("Selecione um Tipo de Cobrança.", MessageType.Info);
                return false;
            }

            if (txtParc1.Enabled)
            {
                v.CampoValido("Parcela 1", txtParc1.Text, true, true, true,  false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc1.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc1.Focus();
                    }
                    return false;
                }
            }

            if (txtParc2.Enabled)
            {
                v.CampoValido("Parcela 2", txtParc2.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc2.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc2.Focus();
                    }
                    return false;
                }
            }

            if (txtParc3.Enabled)
            {
                v.CampoValido("Parcela 3", txtParc3.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc3.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc3.Focus();
                    }
                    return false;
                }
            }

            if (txtParc4.Enabled)
            {
                v.CampoValido("Parcela 4", txtParc4.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc4.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc4.Focus();
                    }
                    return false;
                }
            }
            if (txtParc5.Enabled)
            {
                v.CampoValido("Parcela 5", txtParc5.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc5.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc5.Focus();
                    }
                    return false;
                }
            }
            if (txtParc6.Enabled)
            {
                v.CampoValido("Parcela 6", txtParc6.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc6.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc6.Focus();
                    }
                    return false;
                }
            }
            if (txtParc7.Enabled)
            {
                v.CampoValido("Parcela 7", txtParc7.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc7.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc7.Focus();
                    }
                    return false;
                }
            }
            if (txtParc8.Enabled)
            {
                v.CampoValido("Parcela 8", txtParc8.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc8.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc8.Focus();
                    }
                    return false;
                }
            }
            if (txtParc9.Enabled)
            {
                v.CampoValido("Parcela 9", txtParc9.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc9.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc9.Focus();
                    }
                    return false;
                }
            }
            if (txtParc10.Enabled)
            {
                v.CampoValido("Parcela 10", txtParc10.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtParc10.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtParc10.Focus();
                    }
                    return false;
                }
            }
            
            // A Prazo 
            if (DdlTipoPagamento.SelectedValue == "2")
            {
                if ((Convert.ToDecimal(txtProp1.Text)
                 + Convert.ToDecimal(txtProp2.Text)
                 + Convert.ToDecimal(txtProp3.Text)
                 + Convert.ToDecimal(txtProp4.Text)
                 + Convert.ToDecimal(txtProp5.Text)
                 + Convert.ToDecimal(txtProp6.Text)
                 + Convert.ToDecimal(txtProp7.Text)
                 + Convert.ToDecimal(txtProp8.Text)
                 + Convert.ToDecimal(txtProp9.Text)
                 + Convert.ToDecimal(txtProp10.Text)) != 100)
                {
                    ShowMessage("Percentual de Proporcionalização deve ser 100 %. Verifique", MessageType.Info);
                    return false;
                }

            }

            return true;



        }
        protected void LimpaCampos()
        {
            txtCodigo.Text = "Novo";
            txtDescricao.Text = "";

            FmaPagamentoDAL d = new FmaPagamentoDAL();
            DdlFormaPagamento.DataSource = d.ListarFmasPagamento("","","","");
            DdlFormaPagamento.DataTextField = "DescricaoFmaPagamento";
            DdlFormaPagamento.DataValueField = "CodigoFmaPagamento";
            DdlFormaPagamento.DataBind();
            DdlFormaPagamento.Items.Insert(0, "..... SELECIONE UMA FORMA DE PAGAMENTO .....");

            TipoCobrancaDAL zd = new TipoCobrancaDAL();
            DdlTipoCobranca.DataSource = zd.ListarTipoCobrancas("", "", "", "");
            DdlTipoCobranca.DataTextField = "DescricaoTipoCobranca";
            DdlTipoCobranca.DataValueField = "CodigoTipoCobranca";
            DdlTipoCobranca.DataBind();
            DdlTipoCobranca.Items.Insert(0, "..... SELECIONE UM TIPO DE COBRANÇA .....");

            Habil_TipoDAL sd = new Habil_TipoDAL();
            txtCodSituacao.DataSource = sd.Atividade();
            txtCodSituacao.DataTextField = "DescricaoTipo";
            txtCodSituacao.DataValueField = "CodigoTipo";
            txtCodSituacao.DataBind();


            DdlTipoPagamento.DataSource = sd.CondPagamento();
            DdlTipoPagamento.DataTextField = "DescricaoTipo";
            DdlTipoPagamento.DataValueField = "CodigoTipo";
            DdlTipoPagamento.DataBind();

            DdlTipoPagamento_SelectedIndexChanged(null, null);
            RotinaAbreParcelas("0");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtCodigo.Enabled = false;

            if (Session["ZoomCondPagamento2"] != null)
            {
                if (Session["ZoomCondPagamento2"].ToString() == "RELACIONAL")
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
                ShowMessage(Session["MensagemTela"].ToString(),MessageType.Info);
                Session["MensagemTela"] = "";
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConCondPagamento.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomCondPagamento2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomCondPagamento"] != null)
                {
                    string s = Session["ZoomCondPagamento"].ToString();
                    Session["ZoomCondPagamento"] = null;

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

                                CondPagamentoDAL r = new CondPagamentoDAL();
                                CondPagamento p = new CondPagamento();

                                p = r.PesquisarCondPagamento(Convert.ToInt32(txtCodigo.Text));

                                txtDescricao.Text = p.DescricaoCondPagamento;
                                DdlFormaPagamento.SelectedValue = p.CodigoFmaPagamento.ToString();
                                txtCodSituacao.SelectedValue = p.CodigoSituacao.ToString();

                                DdlTipoPagamento.SelectedValue = p.CodigoTipoPagamento.ToString();
                                DdlTipoPagamento_SelectedIndexChanged(null, null);
                                txtDiaFixo.Text = p.DiaFixo.ToString();

                                DdlTipoCobranca.SelectedValue = p.CodigoTipoCobranca.ToString();
                                txtQtdeParcelas.Text = p.QtdeParcelas.ToString();

                                RotinaAbreParcelas(p.QtdeParcelas.ToString());

                                txtParc1.Text = p.Parcelas1.ToString();
                                txtParc2.Text = p.Parcelas2.ToString();
                                txtParc3.Text = p.Parcelas3.ToString();
                                txtParc4.Text = p.Parcelas4.ToString();
                                txtParc5.Text = p.Parcelas5.ToString();
                                txtParc6.Text = p.Parcelas6.ToString();
                                txtParc7.Text = p.Parcelas7.ToString();
                                txtParc8.Text = p.Parcelas8.ToString();
                                txtParc9.Text = p.Parcelas9.ToString();
                                txtParc10.Text = p.Parcelas10.ToString();

                                txtProp1.Text = p.Proporcao1.ToString("##0.00");
                                txtProp2.Text = p.Proporcao2.ToString("##0.00");
                                txtProp3.Text = p.Proporcao3.ToString("##0.00");
                                txtProp4.Text = p.Proporcao4.ToString("##0.00");
                                txtProp5.Text = p.Proporcao5.ToString("##0.00");
                                txtProp6.Text = p.Proporcao6.ToString("##0.00");
                                txtProp7.Text = p.Proporcao7.ToString("##0.00");
                                txtProp8.Text = p.Proporcao8.ToString("##0.00");
                                txtProp9.Text = p.Proporcao9.ToString("##0.00");
                                txtProp10.Text = p.Proporcao10.ToString("##0.00");
                                                                                             
                                lista.ForEach(delegate(Permissao x)
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
                txtDescricao.Focus();

                    if (Session["IncProdutoCondPagamento"] != null)
                    {
                        txtCodigo.Enabled = true;
                        txtCodigo.Focus();
                        btnExcluir.Visible = false;
                        return;
                    }
                    else
                    {
                        txtCodigo.Text = "Novo";
                        txtCodigo.Enabled = false ;
                        txtDescricao.Focus();
                        btnExcluir.Visible = false;
                    }

                    lista.ForEach(delegate(Permissao p)
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
            if (txtCodigo.Text == "")
                btnVoltar_Click(sender, e);

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    CondPagamentoDAL d = new CondPagamentoDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text.Replace(".", "")));
                    Session["MensagemTela"] = "Condição de Pagamento excluída com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Condição de Pagamento não identificado.&emsp;&emsp;&emsp;";

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
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomCondPagamento"] = null;
            Response.Redirect("~/Pages/Financeiros/ConCondPagamento.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (ValidaCampos() == false)
                    return;


                CondPagamentoDAL d = new CondPagamentoDAL();
                CondPagamento p = new CondPagamento();

                p.DescricaoCondPagamento = txtDescricao.Text.ToUpper();
                p.CodigoFmaPagamento= Convert.ToInt32(DdlFormaPagamento.SelectedValue);
                p.CodigoSituacao = Convert.ToInt32(txtCodSituacao.SelectedValue);

                p.CodigoTipoPagamento = Convert.ToInt32(DdlTipoPagamento.SelectedValue);

                p.DiaFixo = Convert.ToInt32(txtDiaFixo.Text);
                p.CodigoTipoCobranca = Convert.ToInt32(DdlTipoCobranca.SelectedValue);
                p.QtdeParcelas =Convert.ToInt32(txtQtdeParcelas.Text);
                p.Parcelas1 = Convert.ToInt32(txtParc1.Text);
                p.Parcelas2 = Convert.ToInt32(txtParc2.Text);
                p.Parcelas3 = Convert.ToInt32(txtParc3.Text);
                p.Parcelas4 = Convert.ToInt32(txtParc4.Text);
                p.Parcelas5 = Convert.ToInt32(txtParc5.Text);
                p.Parcelas6 = Convert.ToInt32(txtParc6.Text);
                p.Parcelas7 = Convert.ToInt32(txtParc7.Text);
                p.Parcelas8 = Convert.ToInt32(txtParc8.Text);
                p.Parcelas9 = Convert.ToInt32(txtParc9.Text);
                p.Parcelas10 = Convert.ToInt32(txtParc10.Text);

                p.Proporcao1 = Convert.ToDecimal(txtProp1.Text);
                p.Proporcao2 = Convert.ToDecimal(txtProp2.Text);
                p.Proporcao3 = Convert.ToDecimal(txtProp3.Text);
                p.Proporcao4 = Convert.ToDecimal(txtProp4.Text);
                p.Proporcao5 = Convert.ToDecimal(txtProp5.Text);
                p.Proporcao6 = Convert.ToDecimal(txtProp6.Text);
                p.Proporcao7 = Convert.ToDecimal(txtProp7.Text);
                p.Proporcao8 = Convert.ToDecimal(txtProp8.Text);
                p.Proporcao9 = Convert.ToDecimal(txtProp9.Text);
                p.Proporcao10 = Convert.ToDecimal(txtProp10.Text);

                if (txtCodigo.Text == "Novo")
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Condição de Pagamento inclusa com Sucesso!!!";
                }
                else
                {
                    p.CodigoCondPagamento = Convert.ToInt32(txtCodigo.Text);
                    d.Atualizar(p);
                    Session["MensagemTela"] = "Condição de Pagamento alterada com Sucesso!!!";
                }

                btnVoltar_Click(sender, e);

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
        protected void DdlTipoPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (DdlTipoPagamento.SelectedValue)
            {
                case "23":
                    txtQtdeParcelas.Text = "1";
                    txtQtdeParcelas.Enabled = false;

                    txtDiaFixo.Enabled = false;
                    txtDiaFixo.Text = "0";

                    RotinaAbreParcelas("0");
                    btnMais.Enabled = false;
                    btnMenos.Enabled = false;
                    break;

                case "24":
                    txtQtdeParcelas.Text = "1";
                    txtQtdeParcelas.Enabled = false;

                    txtDiaFixo.Enabled = false;
                    txtDiaFixo.Text = "0";

                    RotinaAbreParcelas("1");
                    btnMais.Enabled = true;
                    btnMenos.Enabled = true;

                    break;

                case "25":
                    txtQtdeParcelas.Text = "1";
                    txtQtdeParcelas.Enabled = false;

                    txtDiaFixo.Enabled = true;
                    txtDiaFixo.Text = "0";

                    RotinaAbreParcelas("0");
                    btnMais.Enabled = false;
                    btnMenos.Enabled = false;


                    break;

                case "26":
                    txtQtdeParcelas.Text = "1";
                    txtQtdeParcelas.Enabled = false;
                    txtDiaFixo.Enabled = false;
                    txtDiaFixo.Text = "0";
                    RotinaAbreParcelas("0");
                    btnMais.Enabled = false;
                    btnMenos.Enabled = false;
                    break;

                case "27":
                    txtQtdeParcelas.Text = "1";
                    txtQtdeParcelas.Enabled = false;

                    txtDiaFixo.Enabled = true;
                    txtDiaFixo.Text = "0";
                    btnMais.Enabled = true;
                    btnMenos.Enabled = true; 

                    RotinaAbreParcelas("0");


                    break;

                default:
                    break;
            }

        }
        protected void btnMenos_Click(object sender, EventArgs e)
        {
            if (DdlTipoPagamento.SelectedValue == "2")
            {
                if (txtQtdeParcelas.Text == "")
                    txtQtdeParcelas.Text = "1";
                else
                    txtQtdeParcelas.Text = (Convert.ToInt32(txtQtdeParcelas.Text) - 1).ToString();

                if (txtQtdeParcelas.Text == "0")
                    txtQtdeParcelas.Text = "1";

                RotinaAbreParcelas(txtQtdeParcelas.Text);
                CalculaProporcao(Convert.ToInt32(txtQtdeParcelas.Text));
            }
            else
            {
                if (txtQtdeParcelas.Text == "")
                    txtQtdeParcelas.Text = "1";
                else
                    txtQtdeParcelas.Text = (Convert.ToInt32(txtQtdeParcelas.Text) - 1).ToString();

                if (txtQtdeParcelas.Text == "0")
                    txtQtdeParcelas.Text = "1";

            }
        }
        protected void btnMais_Click(object sender, EventArgs e)
        {
            if (DdlTipoPagamento.SelectedValue == "2")
            {
                if (txtQtdeParcelas.Text == "")
                    txtQtdeParcelas.Text = "1";
                if (txtQtdeParcelas.Text == "10")
                    txtQtdeParcelas.Text = "10";
                else
                    txtQtdeParcelas.Text = (Convert.ToInt32(txtQtdeParcelas.Text) + 1).ToString();

                RotinaAbreParcelas(txtQtdeParcelas.Text);
                CalculaProporcao(Convert.ToInt32(txtQtdeParcelas.Text));
            }
            else
            {
                if (txtQtdeParcelas.Text == "")
                    txtQtdeParcelas.Text = "1";
                else
                    txtQtdeParcelas.Text = (Convert.ToInt32(txtQtdeParcelas.Text) + 1).ToString();

                if (txtQtdeParcelas.Text == "0")
                    txtQtdeParcelas.Text = "1";
            }
        }
        protected void RotinaAbreParcelas (string QtdParcelas)
        {
            switch (QtdParcelas)
            {
                case "1":
                    if (DdlTipoPagamento.SelectedValue  == "2")
                    {
                        txtParc1.Enabled = true;
                        txtProp1.Enabled = true;
                        txtParc1.Text = "0";
                        txtProp1.Text = "0,00";

                    }
                    else
                    {
                        txtParc1.Enabled = false;
                        txtProp1.Enabled = false;
                        txtParc1.Text = "0";
                        txtProp1.Text = "0,00";
                    }

                    txtParc2.Enabled = false;
                    txtProp2.Enabled = false;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = false;
                    txtProp3.Enabled = false;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = false;
                    txtProp4.Enabled = false;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = false;
                    txtProp5.Enabled = false;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = false;
                    txtProp6.Enabled = false;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = false;
                    txtProp7.Enabled = false;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = false;
                    txtProp8.Enabled = false;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "2":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = false;
                    txtProp3.Enabled = false;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = false;
                    txtProp4.Enabled = false;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = false;
                    txtProp5.Enabled = false;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = false;
                    txtProp6.Enabled = false;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = false;
                    txtProp7.Enabled = false;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = false;
                    txtProp8.Enabled = false;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "3":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = true;
                    txtProp3.Enabled = true;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = false;
                    txtProp4.Enabled = false;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = false;
                    txtProp5.Enabled = false;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = false;
                    txtProp6.Enabled = false;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = false;
                    txtProp7.Enabled = false;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = false;
                    txtProp8.Enabled = false;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "4":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = true;
                    txtProp3.Enabled = true;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = true;
                    txtProp4.Enabled = true;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = false;
                    txtProp5.Enabled = false;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = false;
                    txtProp6.Enabled = false;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = false;
                    txtProp7.Enabled = false;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = false;
                    txtProp8.Enabled = false;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "5":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = true;
                    txtProp3.Enabled = true;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = true;
                    txtProp4.Enabled = true;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = true;
                    txtProp5.Enabled = true;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = false;
                    txtProp6.Enabled = false;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = false;
                    txtProp7.Enabled = false;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = false;
                    txtProp8.Enabled = false;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "6":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = true;
                    txtProp3.Enabled = true;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = true;
                    txtProp4.Enabled = true;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = true;
                    txtProp5.Enabled = true;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = true;
                    txtProp6.Enabled = true;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = false;
                    txtProp7.Enabled = false;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = false;
                    txtProp8.Enabled = false;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "7":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = true;
                    txtProp3.Enabled = true;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = true;
                    txtProp4.Enabled = true;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = true;
                    txtProp5.Enabled = true;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = true;
                    txtProp6.Enabled = true;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = true;
                    txtProp7.Enabled = true;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = false;
                    txtProp8.Enabled = false;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "8":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = true;
                    txtProp3.Enabled = true;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = true;
                    txtProp4.Enabled = true;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = true;
                    txtProp5.Enabled = true;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = true;
                    txtProp6.Enabled = true;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = true;
                    txtProp7.Enabled = true;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = true;
                    txtProp8.Enabled = true;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "9":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = true;
                    txtProp3.Enabled = true;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = true;
                    txtProp4.Enabled = true;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = true;
                    txtProp5.Enabled = true;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = true;
                    txtProp6.Enabled = true;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = true;
                    txtProp7.Enabled = true;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = true;
                    txtProp8.Enabled = true;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = true;
                    txtProp9.Enabled = true;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                case "10":
                    txtParc1.Enabled = true;
                    txtProp1.Enabled = true;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = true;
                    txtProp2.Enabled = true;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = true;
                    txtProp3.Enabled = true;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = true;
                    txtProp4.Enabled = true;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = true;
                    txtProp5.Enabled = true;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = true;
                    txtProp6.Enabled = true;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = true;
                    txtProp7.Enabled = true;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = true;
                    txtProp8.Enabled = true;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = true;
                    txtProp9.Enabled = true;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = true;
                    txtProp10.Enabled = true;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";

                    break;

                default:
                    txtParc1.Enabled = false;
                    txtProp1.Enabled = false;
                    txtParc1.Text = "0";
                    txtProp1.Text = "0,00";

                    txtParc2.Enabled = false;
                    txtProp2.Enabled = false;
                    txtParc2.Text = "0";
                    txtProp2.Text = "0,00";

                    txtParc3.Enabled = false;
                    txtProp3.Enabled = false;
                    txtParc3.Text = "0";
                    txtProp3.Text = "0,00";

                    txtParc4.Enabled = false;
                    txtProp4.Enabled = false;
                    txtParc4.Text = "0";
                    txtProp4.Text = "0,00";

                    txtParc5.Enabled = false;
                    txtProp5.Enabled = false;
                    txtParc5.Text = "0";
                    txtProp5.Text = "0,00";

                    txtParc6.Enabled = false;
                    txtProp6.Enabled = false;
                    txtParc6.Text = "0";
                    txtProp6.Text = "0,00";

                    txtParc7.Enabled = false;
                    txtProp7.Enabled = false;
                    txtParc7.Text = "0";
                    txtProp7.Text = "0,00";

                    txtParc8.Enabled = false;
                    txtProp8.Enabled = false;
                    txtParc8.Text = "0";
                    txtProp8.Text = "0,00";

                    txtParc9.Enabled = false;
                    txtProp9.Enabled = false;
                    txtParc9.Text = "0";
                    txtProp9.Text = "0,00";

                    txtParc10.Enabled = false;
                    txtProp10.Enabled = false;
                    txtParc10.Text = "0";
                    txtProp10.Text = "0,00";
                    break;
            }
        }
        protected void CalculaProporcao (int QualParcela)
        {
            decimal Proporcao = 100;

            if (QualParcela == 1)
            {
                txtProp1.Text = Proporcao.ToString("##0.00");
            }

            if (QualParcela == 2)
            {
                txtProp1.Text = (Proporcao / 2).ToString("##0.00");
                txtProp2.Text = (Proporcao / 2).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text)
                    + 100 -
                    (Convert.ToDecimal((Proporcao / 2).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 2).ToString("##0.00"))
                    )).ToString("##0.00");

            }

            if (QualParcela == 3)
            {
                txtProp1.Text = (Proporcao / 3).ToString("##0.00");
                txtProp2.Text = (Proporcao / 3).ToString("##0.00");
                txtProp3.Text = (Proporcao / 3).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text) 
                    + 100 - 
                    (Convert.ToDecimal((Proporcao / 3).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 3).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 3).ToString("##0.00"))
                    )).ToString("##0.00");

            }
            if (QualParcela == 4)
            {
                txtProp1.Text = (Proporcao / 4).ToString("##0.00");
                txtProp2.Text = (Proporcao / 4).ToString("##0.00");
                txtProp3.Text = (Proporcao / 4).ToString("##0.00");
                txtProp4.Text = (Proporcao / 4).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text)
                    + 100 -
                    (Convert.ToDecimal((Proporcao / 4).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 4).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 4).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 4).ToString("##0.00"))
                    )).ToString("##0.00");

            }

            if (QualParcela == 5)
            {
                txtProp1.Text = (Proporcao / 5).ToString("##0.00");
                txtProp2.Text = (Proporcao / 5).ToString("##0.00");
                txtProp3.Text = (Proporcao / 5).ToString("##0.00");
                txtProp4.Text = (Proporcao / 5).ToString("##0.00");
                txtProp5.Text = (Proporcao / 5).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text)
                    + 100 -
                    (Convert.ToDecimal((Proporcao / 5).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 5).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 5).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 5).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 5).ToString("##0.00"))
                    )).ToString("##0.00");
            }

            if (QualParcela == 6)
            {
                txtProp1.Text = (Proporcao / 6).ToString("##0.00");
                txtProp2.Text = (Proporcao / 6).ToString("##0.00");
                txtProp3.Text = (Proporcao / 6).ToString("##0.00");
                txtProp4.Text = (Proporcao / 6).ToString("##0.00");
                txtProp5.Text = (Proporcao / 6).ToString("##0.00");
                txtProp6.Text = (Proporcao / 6).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text)
                    + 100 -
                    (Convert.ToDecimal((Proporcao / 6).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 6).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 6).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 6).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 6).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 6).ToString("##0.00"))
                    )).ToString("##0.00");
            }

            if (QualParcela == 7)
            {
                txtProp1.Text = (Proporcao / 7).ToString("##0.00");
                txtProp2.Text = (Proporcao / 7).ToString("##0.00");
                txtProp3.Text = (Proporcao / 7).ToString("##0.00");
                txtProp4.Text = (Proporcao / 7).ToString("##0.00");
                txtProp5.Text = (Proporcao / 7).ToString("##0.00");
                txtProp6.Text = (Proporcao / 7).ToString("##0.00");
                txtProp7.Text = (Proporcao / 7).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text)
                    + 100 -
                    (Convert.ToDecimal((Proporcao / 7).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 7).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 7).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 7).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 7).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 7).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 7).ToString("##0.00"))
                    )).ToString("##0.00");
            }

            if (QualParcela == 8)
            {
                txtProp1.Text = (Proporcao / 8).ToString("##0.00");
                txtProp2.Text = (Proporcao / 8).ToString("##0.00");
                txtProp3.Text = (Proporcao / 8).ToString("##0.00");
                txtProp4.Text = (Proporcao / 8).ToString("##0.00");
                txtProp5.Text = (Proporcao / 8).ToString("##0.00");
                txtProp6.Text = (Proporcao / 8).ToString("##0.00");
                txtProp7.Text = (Proporcao / 8).ToString("##0.00");
                txtProp8.Text = (Proporcao / 8).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text)
                    + 100 -
                    (Convert.ToDecimal((Proporcao / 8).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 8).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 8).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 8).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 8).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 8).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 8).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 8).ToString("##0.00"))
                    )).ToString("##0.00");
            }

            if (QualParcela == 9)
            {
                txtProp1.Text = (Proporcao / 9).ToString("##0.00");
                txtProp2.Text = (Proporcao / 9).ToString("##0.00");
                txtProp3.Text = (Proporcao / 9).ToString("##0.00");
                txtProp4.Text = (Proporcao / 9).ToString("##0.00");
                txtProp5.Text = (Proporcao / 9).ToString("##0.00");
                txtProp6.Text = (Proporcao / 9).ToString("##0.00");
                txtProp7.Text = (Proporcao / 9).ToString("##0.00");
                txtProp8.Text = (Proporcao / 9).ToString("##0.00");
                txtProp9.Text = (Proporcao / 9).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text)
                    + 100 -
                    (Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 9).ToString("##0.00"))
                    )).ToString("##0.00");
            }

            if (QualParcela == 10)
            {
                txtProp1.Text = (Proporcao / 10).ToString("##0.00");
                txtProp2.Text = (Proporcao / 10).ToString("##0.00");
                txtProp3.Text = (Proporcao / 10).ToString("##0.00");
                txtProp4.Text = (Proporcao / 10).ToString("##0.00");
                txtProp5.Text = (Proporcao / 10).ToString("##0.00");
                txtProp6.Text = (Proporcao / 10).ToString("##0.00");
                txtProp7.Text = (Proporcao / 10).ToString("##0.00");
                txtProp8.Text = (Proporcao / 10).ToString("##0.00");
                txtProp9.Text = (Proporcao / 10).ToString("##0.00");
                txtProp10.Text = (Proporcao / 10).ToString("##0.00");

                txtProp1.Text = (Convert.ToDecimal(txtProp1.Text)
                    + 100 -
                    (Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    + Convert.ToDecimal((Proporcao / 10).ToString("##0.00"))
                    )).ToString("##0.00");
            }



        }
        protected void txtParc1_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc1.Text.Equals(""))
            {
                txtParc1.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 1", txtParc1.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc1.Text = Convert.ToDecimal(txtParc1.Text).ToString("###,##0");
                    CalculaProporcao(1);

                }
                else
                    txtParc1.Text = "0";
            }
        }

        protected void txtProp1_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp1.Text.Equals(""))
            {
                txtProp1.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 1", txtProp1.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp1.Text = Convert.ToDecimal(txtProp1.Text).ToString("###,##0.00");
                else
                    txtProp1.Text = "0,00";

            }

        }

        protected void txtParc2_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc2.Text.Equals(""))
            {
                txtParc2.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 2", txtParc2.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc2.Text = Convert.ToDecimal(txtParc2.Text).ToString("###,##0");
                    CalculaProporcao(2);

                }
                else
                    txtParc2.Text = "0";
            }

        }

        protected void txtProp2_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp2.Text.Equals(""))
            {
                txtProp2.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 2", txtProp2.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp2.Text = Convert.ToDecimal(txtProp2.Text).ToString("###,##0.00");
                else
                    txtProp2.Text = "0,00";

            }
        }

        protected void txtParc3_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc3.Text.Equals(""))
            {
                txtParc3.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 3", txtParc3.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc3.Text = Convert.ToDecimal(txtParc3.Text).ToString("###,##0");
                    CalculaProporcao(3);

                }
                else
                    txtParc3.Text = "0";
            }

        }

        protected void txtProp3_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp3.Text.Equals(""))
            {
                txtProp3.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 3", txtProp3.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp3.Text = Convert.ToDecimal(txtProp3.Text).ToString("###,##0.00");
                else
                    txtProp3.Text = "0,00";

            }

        }

        protected void txtParc4_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc4.Text.Equals(""))
            {
                txtParc4.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 4", txtParc4.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc4.Text = Convert.ToDecimal(txtParc4.Text).ToString("###,##0");
                    CalculaProporcao(4);

                }
                else
                    txtParc4.Text = "0";
            }

        }

        protected void txtProp4_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp4.Text.Equals(""))
            {
                txtProp4.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 4", txtProp4.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp4.Text = Convert.ToDecimal(txtProp4.Text).ToString("###,##0.00");
                else
                    txtProp4.Text = "0,00";

            }

        }

        protected void txtParc5_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc5.Text.Equals(""))
            {
                txtParc5.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 5", txtParc5.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc5.Text = Convert.ToDecimal(txtParc5.Text).ToString("###,##0");
                    CalculaProporcao(5);

                }
                else
                    txtParc5.Text = "0";
            }


        }

        protected void txtProp5_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp5.Text.Equals(""))
            {
                txtProp5.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 5", txtProp5.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp5.Text = Convert.ToDecimal(txtProp5.Text).ToString("###,##0.00");
                else
                    txtProp5.Text = "0,00";

            }

        }

        protected void txtParc6_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc6.Text.Equals(""))
            {
                txtParc6.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 6", txtParc6.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc6.Text = Convert.ToDecimal(txtParc6.Text).ToString("###,##0");
                    CalculaProporcao(6);

                }
                else
                    txtParc6.Text = "0";
            }

        }

        protected void txtProp6_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp6.Text.Equals(""))
            {
                txtProp6.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 6", txtProp6.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp6.Text = Convert.ToDecimal(txtProp6.Text).ToString("###,##0.00");
                else
                    txtProp6.Text = "0,00";

            }

        }

        protected void txtParc7_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc7.Text.Equals(""))
            {
                txtParc7.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 7", txtParc7.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc7.Text = Convert.ToDecimal(txtParc7.Text).ToString("###,##0");
                    CalculaProporcao(7);

                }
                else
                    txtParc7.Text = "0";
            }

        }

        protected void txtProp7_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp7.Text.Equals(""))
            {
                txtProp7.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 7", txtProp7.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp7.Text = Convert.ToDecimal(txtProp7.Text).ToString("###,##0.00");
                else
                    txtProp7.Text = "0,00";

            }

        }

        protected void txtParc8_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc8.Text.Equals(""))
            {
                txtParc8.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 8", txtParc8.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc8.Text = Convert.ToDecimal(txtParc8.Text).ToString("###,##0");
                    CalculaProporcao(8);

                }
                else
                    txtParc8.Text = "0";
            }

        }

        protected void txtProp8_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp8.Text.Equals(""))
            {
                txtProp8.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 8", txtProp8.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp8.Text = Convert.ToDecimal(txtProp8.Text).ToString("###,##0.00");
                else
                    txtProp8.Text = "0,00";

            }

        }

        protected void txtParc9_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc9.Text.Equals(""))
            {
                txtParc9.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 9", txtParc9.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc9.Text = Convert.ToDecimal(txtParc9.Text).ToString("###,##0");
                    CalculaProporcao(9);

                }
                else
                    txtParc9.Text = "0";
            }

        }

        protected void txtProp9_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp9.Text.Equals(""))
            {
                txtProp9.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 9", txtProp9.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp9.Text = Convert.ToDecimal(txtProp9.Text).ToString("###,##0.00");
                else
                    txtProp9.Text = "0,00";

            }

        }

        protected void txtParc10_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtParc10.Text.Equals(""))
            {
                txtParc10.Text = "0";
            }
            else
            {
                v.CampoValido("Parcela 10", txtParc10.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtParc10.Text = Convert.ToDecimal(txtParc10.Text).ToString("###,##0");
                    CalculaProporcao(10);

                }
                else
                    txtParc10.Text = "0";
            }
        }

        protected void txtProp10_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtProp10.Text.Equals(""))
            {
                txtProp10.Text = "0,00";
            }
            else
            {

                v.CampoValido("Proporcionalização 10", txtProp10.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                    txtProp10.Text = Convert.ToDecimal(txtProp10.Text).ToString("###,##0.00");
                else
                    txtProp10.Text = "0,00";

            }


        }
    }
}