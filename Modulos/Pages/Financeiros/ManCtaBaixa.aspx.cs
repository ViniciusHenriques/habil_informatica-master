using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class ManCtaBaixa : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }

        private List<BaixaDocumento> listaBaixas = new List<BaixaDocumento>();

        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaTela()
        {
            
            CarregaTiposSituacoes();
            
            DBTabelaDAL RnTab = new DBTabelaDAL();
            txtdtBaixa.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm");
            ddlCtaCorrente.SelectedValue = "..... SELECIONE UMA CONTA CORRENTE .....";
            txtCodBaixa.Text = "Novo";     

            


        }
        protected void CarregaSituacoesDocumento()
        {


            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.SituacaoCtaPagar();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();

            ddlClassificacao.DataSource = sd.ClassificaCtaPagar();
            ddlClassificacao.DataTextField = "DescricaoTipo";
            ddlClassificacao.DataValueField = "CodigoTipo";
            ddlClassificacao.DataBind();


            EmpresaDAL RnEmpresa = new EmpresaDAL();
            ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("", "", "", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();


            PlanoContasDAL RnPlanoConta = new PlanoContasDAL();
            ddlPlanoConta.DataSource = RnPlanoConta.ListarPlanoContas("", "", "", "");
            ddlPlanoConta.DataTextField = "DescricaoPlanoConta";
            ddlPlanoConta.DataValueField = "CodigoPlanoConta";
            ddlPlanoConta.DataBind();
            ddlPlanoConta.Items.Insert(0, "..... SELECIONE UM PLANO DE CONTAS .....");

            TipoCobrancaDAL RnCobranca = new TipoCobrancaDAL();
            ddlTipoCobranca.DataSource = RnCobranca.ListarTipoCobrancas("", "", "", "");
            ddlTipoCobranca.DataTextField = "DescricaoTipoCobranca";
            ddlTipoCobranca.DataValueField = "CodigoTipoCobranca";
            ddlTipoCobranca.DataBind();
            ddlTipoCobranca.Items.Insert(0, "..... SELECIONE UM TIPO DE COBRANÇA .....");


        }
        protected void CarregaTiposSituacoes()
        {
            ContaCorrenteDAL conta = new ContaCorrenteDAL();
            ddlCtaCorrente.DataSource = conta.ListarContaCorrente("", "", "", "");
            ddlCtaCorrente.DataTextField = "DescricaoContaCorrente";
            ddlCtaCorrente.DataValueField = "CodigoContaCorrente";
            ddlCtaCorrente.DataBind();
            ddlCtaCorrente.Items.Insert(0, "..... SELECIONE UMA CONTA CORRENTE .....");

            Habil_TipoDAL sd = new Habil_TipoDAL();

            ddlTipoBaixa.DataSource = sd.TipoBaixa();
            ddlTipoBaixa.DataTextField = "DescricaoTipo";
            ddlTipoBaixa.DataValueField = "CodigoTipo";
            ddlTipoBaixa.DataBind();


            TipoCobrancaDAL RnCobranca = new TipoCobrancaDAL();

            ddlTpCobranca.DataSource = RnCobranca.ListarTipoCobrancas("", "", "", "");
            ddlTpCobranca.DataTextField = "DescricaoTipoCobranca";
            ddlTpCobranca.DataValueField = "CodigoTipoCobranca";
            ddlTpCobranca.DataBind();
            ddlTpCobranca.Items.Insert(0, "..... SELECIONE UM TIPO DE COBRANÇA .....");
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Valor da Baixa", txtVlrBaixa.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);


            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtVlrBaixa.Focus();

                }

                return false;
            }
            if (ddlTpCobranca.Text == "..... SELECIONE UM TIPO DE COBRANÇA .....")
            {
                ShowMessage("Selecione um Tipo de cobrança", MessageType.Info);
                return false;
            }

            

            if (Convert.ToDecimal(txtVlrBaixa.Text) <= 0)
            {
                ShowMessage("Digite um valor válido!", MessageType.Info);
                return false;
            }

            return true;
        }
        protected void MontaTela(long CodRegra)
        {
            LimpaTela();

            //    //Montar a Lista dos Estados

            //    DocCtaPagarLocalizacaoDAL r = new DocCtaPagarLocalizacaoDAL();
            //    TabIcmsDAL t = new TabIcmsDAL();
            //    TabIcms ta = new TabIcms();
            //    List<DocCtaPagarLocalizacao> lista = new List<DocCtaPagarLocalizacao>();

            //    lista = r.ListarDocCtaPagarLocalizacao(CodRegra);

            //    foreach (DocCtaPagarLocalizacao item in  lista)
            //    {
            //        ta = t.PesquisarTabIcms(item.CodLocalizacaoUF);

            //        ddlEstadoOrigem.SelectedValue = ta.CodOrigem.ToString();
            //        ddlEstadoDestino.SelectedValue = ta.CodDestino.ToString();

            //        BtnAddLocalizacao_Click(null,null);
            //    }

            //    ///////////////////////////////////////////////////////////////////////////////////

            //    //Montar a Grupo de Pessoas

            //    DocCtaPagarGpoTribPessoaDAL rr2 = new DocCtaPagarGpoTribPessoaDAL();
            //    List<DocCtaPagarGpoTribPessoa> lista2 = new List<DocCtaPagarGpoTribPessoa>();

            //    lista2 = rr2.ListarDocCtaPagarGpoTribPessoa(CodRegra);

            //    foreach (DocCtaPagarGpoTribPessoa item in lista2)
            //    {
            //        ddlGpoTribPessoa .SelectedValue = item.CodGpoTribPessoa.ToString();
            //        BtnGpoTribPessoa_Click(null, null);
            //    }

            //    ///////////////////////////////////////////////////////////////////////////////////


            //    //Montar a Grupo de Produtos

            //    DocCtaPagarGpoTribProdutoDAL rr3 = new DocCtaPagarGpoTribProdutoDAL();
            //    List<DocCtaPagarGpoTribProduto> lista3 = new List<DocCtaPagarGpoTribProduto>();

            //    lista3 = rr3.ListarDocCtaPagarGpoTribProduto(CodRegra);

            //    foreach (DocCtaPagarGpoTribProduto item in lista3)
            //    {
            //        ddlGpoTribProduto.SelectedValue = item.CodGpoTribProduto.ToString();
            //        BtnGpoTribProduto_Click(null, null);
            //    }

            //    ///////////////////////////////////////////////////////////////////////////////////

            //    //Montar a Aplicação de Uso

            //    DocCtaPagarAplicacaoUsoDAL rr4 = new DocCtaPagarAplicacaoUsoDAL();
            //    List<DocCtaPagarAplicacaoUso> lista4 = new List<DocCtaPagarAplicacaoUso>();

            //    lista4 = rr4.ListarDocCtaPagarAplicacaoUso(CodRegra);

            //    foreach (DocCtaPagarAplicacaoUso item in lista4)
            //    {
            //        ddlAplicacaoUso.SelectedValue = item.CodAplicacaoUso.ToString();
            //        BtnAplicacaoUso_Click(null, null);
            //    }

            //    ///////////////////////////////////////////////////////////////////////////////////

            //    //Montar a Operação Fiscal

            //    DocCtaPagarTipoOperFiscalDAL rr5 = new DocCtaPagarTipoOperFiscalDAL();
            //    List<DocCtaPagarTipoOperFiscal> lista5 = new List<DocCtaPagarTipoOperFiscal>();

            //    lista5 = rr5.ListarDocCtaPagarOperFiscal(CodRegra);

            //    foreach (DocCtaPagarTipoOperFiscal item in lista5)
            //    {
            //        ddlOperFiscal.SelectedValue = item.CodTipoOperFiscal.ToString();
            //        btnOperFiscal_Click(null, null);
            //    }

            //    ///////////////////////////////////////////////////////////////////////////////////

            //    DocCtaPagarDAL rp = new DocCtaPagarDAL();
            //    DocCtaPagar rr = new DocCtaPagar();

            //    rr = rp.PesquisarDocCtaPagar(CodRegra);

            //    txtAliquota.Text = Convert.ToDouble(rr.AliquotaICMS).ToString("###,##0.00");
            //    txtRedBC.Text = Convert.ToDouble(rr.ReducaoBCICMS).ToString("###,##0.0000");
            //    txtRedBCIcmsPropST.Text = Convert.ToDouble(rr.ReducaoBCSTICMSProprio).ToString("###,##0.0000");
            //    txtRedBCST.Text = Convert.ToDouble(rr.ReducaoBCST).ToString("###,##0.0000");
            //    txtMvaSaida.Text = Convert.ToDouble(rr.MVASaida).ToString("###,##0.0000");
            //    txtMvaEntrada.Text = Convert.ToDouble(rr.MVAEntrada).ToString("###,##0.0000");

            //    if (rr.DataInicial != null)
            //        txtdtinicial.Text = rr.DataInicial.ToString().Substring(0, 10);
            //    if (rr.DataFinal != null)
            //        txtdtfinal.Text = rr.DataFinal.ToString().Substring(0, 10);

            //    ddlRegTributario.SelectedValue = rr.CodRegTributario.ToString();
            //    ddlRegTributario_SelectedIndexChanged(null, null);

            //    if (rr.CodCST >= 0)
            //        if (rr.CodCST == 0)
            //            ddlCSTCSOSN.SelectedValue = "00";
            //        else
            //            ddlCSTCSOSN.SelectedValue = rr.CodCST.ToString();

            //    if (rr.CodCSOSN >= 0)
            //        ddlCSTCSOSN.SelectedValue = rr.CodCSOSN.ToString();

            //    chkCalcDifal.Checked = rr.CalculaDifal == true;
            //    chkSomaIPInaST.Checked = rr.SomaIPInaBaseDaST == true;

            //    ddlSituacao.SelectedValue = rr.CodSituacao.ToString();


            //    txtDiferimento.Text = Convert.ToDouble(rr.Diferimento).ToString("###,##0.0000");
            //    txtDifal.Text = Convert.ToDouble(rr.AliquotaDIFAL).ToString("###,##0.0000");
            //    txtDataHora.Text = rr.DataHora.ToString();
            //    txtMensagem.Text = rr.Mensagem;
            //    txtDescricao.Text = rr.Descricao;


            //    if (rr.CodSituacao ==2)
            //    {
            //        btnExcluir.Visible = false;
            //        btnSalvar.Visible = false;
            //        ddlSituacao.Enabled = false;
            //        ddlAcao.Visible = false; 
            //    }

            //    Habil_LogDAL hl = new Habil_LogDAL();

            //    grdLog.DataSource = hl.ListarLogs(rr.CodigoDocCtaPagar); 
            //    grdLog.DataBind();




        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["TabFocadaManDocCtaPagar"] != null)
            {
                PanelSelect = Session["TabFocadaManDocCtaPagar"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaManDocCtaPagar"] = "home";
            }

            if (Session["NovaBaixa"] != null)
                listaBaixas = (List<BaixaDocumento>)Session["NovaBaixa"];

            //teste
            List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
            Doc_CtaPagarDAL r = new Doc_CtaPagarDAL();
            //grdBxCtaPagar.DataSource = r.ListarDoc_CtaPagarCompleto(listaT);
            //grdBxCtaPagar.DataBind();
            ///////////////////////////////////////////////////////////////////////////



            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                

                if (Session["ZoomDocCtaBaixa2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomDocCtaBaixa"] != null)
                {
                    string s = Session["ZoomDocCtaBaixa"].ToString();
                    Session["ZoomDocCtaBaixa"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                         
                        foreach (string word in words)
                        {
                            if (txtCodBaixa.Text == "")
                                txtCodBaixa.Text = word;
                        }

                        if (txtCodBaixa.Text != "Novo")
                        {
                            btnExcluir.Visible = true;
                            foreach (BaixaDocumento p in listaBaixas)
                            {
                                if (txtCodBaixa.Text == p.CodigoBaixa.ToString())
                                {
                                    btnSalvar.Visible = false;
                                    CarregaTiposSituacoes();
                                    txtCodBaixa.Text = p.CodigoBaixa.ToString();
                                    txtdtBaixa.Text = p.DataBaixa.ToString();
                                    txtVlrBaixa.Text = p.ValorBaixa.ToString();
                                    TxtVlrAcrescimo.Text = p.ValorAcrescimo.ToString();
                                    TxtVlrDesconto.Text = p.ValorDesconto.ToString();
                                    ddlTipoBaixa.SelectedValue = p.TipoBaixa.ToString();
                                    ddlCtaCorrente.SelectedValue = p.CodigoContaCorrente.ToString();
                                    ddlTipoCobranca.SelectedValue = p.CodigoTipoCobranca.ToString();
                                    TxtOb.Text = p.Observacao;



                                }
                            }
                        }
                                             
                        
                    }
                }
                else
                {
                    LimpaTela();
                    txtCodDocumento.Text = "Novo";
                    btnExcluir.Visible = false;


                }
                if (Session["Ssn_TipoPessoa"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                {
                    Doc_CtaPagar doc = new Doc_CtaPagar();
                    doc = (Doc_CtaPagar)Session["Ssn_TipoPessoa"];
                    
                    CarregaSituacoesDocumento();
                    txtCodDocumento.Text = Convert.ToString(doc.CodigoDocumento);
                    if (doc.CodigoDocumento == 0)
                        txtCodDocumento.Text = "Novo";

                    txtCodDocumento.Enabled = false;

                    txtdtemissao.Text = doc.DataEmissao.ToString().Substring(0, 10);
                    if (txtdtemissao.Text == "01/01/0001")
                        txtdtemissao.Text = "";

                    txtdtvencimento.Text = doc.DataVencimento.ToString().Substring(0, 10);
                    if (txtdtvencimento.Text == "01/01/0001")
                        txtdtvencimento.Text = "";

                    txtdtentrada.Text = doc.DataEntrada.ToString().Substring(0, 10);
                    ddlEmpresa.SelectedValue = Convert.ToString(doc.CodigoEmpresa);
                    ddlClassificacao.SelectedValue = Convert.ToString(doc.CodigoClassificacao);
                    ddlSituacao.SelectedValue = Convert.ToString(doc.CodigoSituacao);
                    txtNroDocumento.Text = Convert.ToString(doc.DGDocumento);
                    if (doc.CodigoCobranca != 0)
                        ddlTipoCobranca.SelectedValue = Convert.ToString(doc.CodigoCobranca);
                    if (doc.CodigoPlanoContas != 0)
                        ddlPlanoConta.SelectedValue = Convert.ToString(doc.CodigoPlanoContas);
                    txtOBS.Text = doc.ObservacaoDocumento;
                    
                    if (doc.CodigoPessoa == 0)
                        txtCodCredor.Text = "";
                    else
                    {
                        PessoaDAL pesDAL = new PessoaDAL();
                        Pessoa pes = new Pessoa(); 
                        pes = pesDAL.PesquisarPessoa(doc.CodigoPessoa);
                        txtCodCredor.Text = pes.NomePessoa;
                    }

                    txtVlrDocumento.Text = Convert.ToString(doc.ValorDocumento);
                    txtVlrAcres.Text = Convert.ToString(doc.ValorAcrescimo);
                    txtVlrDesc.Text = Convert.ToString(doc.ValorDesconto);
                    txtVlrTotal.Text = Convert.ToString(doc.ValorGeral);
                    if (txtCodBaixa.Text == "Novo")
                    {
                        txtVlrBaixa.Text = Convert.ToString(doc.Cpl_vlPagar);
                        TxtVlrAcrescimo.Text = doc.Cpl_vlAcres.ToString();
                        TxtVlrDesconto.Text = doc.Cpl_vlDesc.ToString();
                    }
                    
                    txtdtVencimentoBaixa.Text =doc.DataVencimento.ToString().Substring(0, 10);
                    ddlTpCobranca.SelectedValue = doc.CodigoCobranca.ToString();

                    if (ddlSituacao.SelectedValue == "38")
                    {
                        btnExcluir.Visible = false;
                        btnSalvar.Visible = false;
                    }
                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConCtaPagar.aspx");
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
                else if (Session["Ssn_CtaReceber"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 2)
                {
                    Doc_CtaReceber doc = new Doc_CtaReceber();
                    doc = (Doc_CtaReceber)Session["Ssn_CtaReceber"];

                    CarregaSituacoesDocumento();
                    txtCodDocumento.Text = Convert.ToString(doc.CodigoDocumento);
                    if (doc.CodigoDocumento == 0)
                        txtCodDocumento.Text = "Novo";

                    txtCodDocumento.Enabled = false;

                    txtdtemissao.Text = doc.DataEmissao.ToString().Substring(0, 10);
                    if (txtdtemissao.Text == "01/01/0001")
                        txtdtemissao.Text = "";

                    txtdtvencimento.Text = doc.DataVencimento.ToString().Substring(0, 10);
                    if (txtdtvencimento.Text == "01/01/0001")
                        txtdtvencimento.Text = "";

                    txtdtentrada.Text = doc.DataEntrada.ToString().Substring(0, 10);
                    ddlEmpresa.SelectedValue = Convert.ToString(doc.CodigoEmpresa);
                    ddlClassificacao.SelectedValue = Convert.ToString(doc.CodigoClassificacao);
                    ddlSituacao.SelectedValue = Convert.ToString(doc.CodigoSituacao);
                    txtNroDocumento.Text = Convert.ToString(doc.DGDocumento);
                    if (doc.CodigoCobranca != 0)
                        ddlTipoCobranca.SelectedValue = Convert.ToString(doc.CodigoCobranca);
                    if (doc.CodigoPlanoContas != 0)
                        ddlPlanoConta.SelectedValue = Convert.ToString(doc.CodigoPlanoContas);
                    txtOBS.Text = doc.ObservacaoDocumento;

                    if (doc.CodigoPessoa == 0)
                        txtCodCredor.Text = "";
                    else
                    {
                        PessoaDAL pesDAL = new PessoaDAL();
                        Pessoa pes = new Pessoa();
                        pes = pesDAL.PesquisarPessoa(doc.CodigoPessoa);
                        txtCodCredor.Text = pes.NomePessoa;
                    }

                    txtVlrDocumento.Text = Convert.ToString(doc.ValorDocumento);
                    txtVlrAcres.Text = Convert.ToString(doc.ValorAcrescimo);
                    txtVlrDesc.Text = Convert.ToString(doc.ValorDesconto);
                    txtVlrTotal.Text = Convert.ToString(doc.ValorGeral);
                    if (txtCodBaixa.Text == "Novo")
                    {
                        txtVlrBaixa.Text = Convert.ToString(doc.Cpl_vlPagar);
                        TxtVlrAcrescimo.Text = doc.Cpl_vlAcres.ToString();
                        TxtVlrDesconto.Text = doc.Cpl_vlDesc.ToString();
                    }

                    txtdtVencimentoBaixa.Text = doc.DataVencimento.ToString().Substring(0, 10);
                    ddlTpCobranca.SelectedValue = doc.CodigoCobranca.ToString();

                    if (ddlSituacao.SelectedValue == "38")
                    {
                        btnExcluir.Visible = false;
                        btnSalvar.Visible = false;
                    }
                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConCtaReceber.aspx");
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
                if (txtCodBaixa.Text == "")
                {
                    btnVoltar_Click(sender, e);
                }



                CalculaTotal();
            }
            if (Session["Baixa"] != null)
            {
                BaixaDocumento baixa = (BaixaDocumento)Session["Baixa"];
                if (baixa.CodigoBaixa == 0)
                    txtCodBaixa.Text = "Novo";
                else
                    txtCodBaixa.Text =Convert.ToString(baixa.CodigoBaixa);
                txtdtBaixa.Text = Convert.ToString(baixa.DataBaixa);
                if (txtdtBaixa.Text == "01/01/0001")
                    txtdtBaixa.Text = "";

                txtVlrBaixa.Text = baixa.ValorBaixa.ToString();

                ddlCtaCorrente.SelectedValue = Convert.ToString(baixa.CodigoContaCorrente);
                Session["Baixa"] = null;

            }
            if (txtCodBaixa.Text == "")
            {
                btnVoltar_Click(sender, e);
            }

        }
        
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            int intInsItem = 0;


            if (txtCodBaixa.Text != "Novo")
            {
                intInsItem = Convert.ToInt32(txtCodBaixa.Text);
                
                intInsItem = Convert.ToInt32(txtCodBaixa.Text);

                if (intInsItem != 0)
                    listaBaixas.RemoveAll(x => x.CodigoBaixa == intInsItem);

                Session["NovaBaixa"] = listaBaixas;
                btnExcluir.Visible = false;
                btnSalvar.Visible = true;
                ShowMessage( "Baixa Excluída com sucesso!",MessageType.Info);
                LimpaTela();
                if (Session["Ssn_TipoPessoa"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                {
                    Doc_CtaPagar doc = (Doc_CtaPagar)Session["Ssn_TipoPessoa"];
                    ddlTpCobranca.SelectedValue = doc.CodigoCobranca.ToString();
                }
                else if (Session["Ssn_CtaReceber"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 2)
                {
                    Doc_CtaReceber doc = (Doc_CtaReceber)Session["Ssn_CtaReceber"];
                    ddlTpCobranca.SelectedValue = doc.CodigoCobranca.ToString();
                }
            }

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["TabFocadaManDocCtaPagar"] = "consulta1";
            if (Session["Ssn_TipoPessoa"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                Response.Redirect("~/Pages/Financeiros/ManCtaPagar.aspx");
            if (Session["Ssn_CtaReceber"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 2)
                Response.Redirect("~/Pages/Financeiros/ManCtaReceber.aspx");
            else
            {
                Session["Ssn_TipoPessoa"] = null;
                Session["Ssn_CtaReceber"] = null;
                Response.Redirect("~/Pages/welcome.aspx");

            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;



            int ContaCorrente=0;
            if (ddlCtaCorrente.Text != "..... SELECIONE UMA CONTA CORRENTE .....")
            {
                ContaCorrente = Convert.ToInt32(ddlCtaCorrente.SelectedValue);
            }
            


            int intCttItem = 0;
            if (txtCodBaixa.Text != "Novo")
                intCttItem = Convert.ToInt32(txtCodBaixa.Text);
            else
            {
                if (listaBaixas.Count != 0)
                    intCttItem = Convert.ToInt32(listaBaixas.Max(x => x.CodigoBaixa).ToString());
                

                intCttItem = intCttItem + 1;
            }
            if (intCttItem != 0)
                listaBaixas.RemoveAll(x => x.CodigoBaixa == intCttItem);
            
            BaixaDocumento baixa = new BaixaDocumento(intCttItem,
                                                      Convert.ToDateTime(txtdtBaixa.Text),
                                                      Convert.ToDecimal(txtVlrBaixa.Text),
                                                      Convert.ToDateTime(txtdtentrada.Text),
                                                      Convert.ToDecimal(TxtVlrDesconto.Text),
                                                      Convert.ToDecimal(TxtVlrAcrescimo.Text),
                                                      Convert.ToDecimal(TxtVlTotalbaixa.Text),
                                                      Convert.ToInt32(ddlTpCobranca.SelectedValue),
                                                      ContaCorrente,
                                                      txtOBS.Text,
                                                      Convert.ToInt32(ddlTipoBaixa.SelectedValue),
                                                      ddlTpCobranca.SelectedItem.Text);

            listaBaixas.Add(baixa);
            Session["NovaBaixa"] = listaBaixas;
            
            if(txtCodBaixa.Text == "Novo")
                Session["MensagemTela"] = "Baixa do documento feita com sucesso!";
            else
                Session["MensagemTela"] = "Baixa do documento alterada com sucesso!";
            Session["TabFocadaManDocCtaPagar"] = "consulta1";
            btnVoltar_Click(sender,e);
        }
        
        protected void btnNovBaixa_Click(object sender, EventArgs e)
        {

        }
        protected void btnReativar_Click(object sender, EventArgs e)
        {

        }
        protected void grdBxCtaPagar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void BtnAddCtaCorrente_Click(object sender, EventArgs e)
        {
            BaixaDocumento baixa = new BaixaDocumento();
            if (txtCodBaixa.Text == "Novo")
                baixa.CodigoBaixa = 0;
            else
                baixa.CodigoBaixa = Convert.ToInt32(txtCodBaixa.Text);
            baixa.ValorBaixa = Convert.ToDecimal(txtVlrBaixa.Text);
            baixa.DataBaixa = Convert.ToDateTime(txtdtBaixa.Text);
            if (ddlCtaCorrente.SelectedValue == "..... SELECIONE UMA CONTA CORRENTE .....")
                baixa.CodigoContaCorrente = 0;
            else
                baixa.CodigoContaCorrente = Convert.ToInt32(ddlCtaCorrente.SelectedValue);
            Session["Baixa"] = baixa;
            Response.Redirect("~/Pages/Financeiros/CadCtaCorrente.aspx?cad="+ Request.QueryString["cad"]);
        }
        protected void txtVlrDesc_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (TxtVlrDesconto.Text.Equals(""))
            {
                TxtVlrDesconto.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor do Desconto", TxtVlrDesconto.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    TxtVlrDesconto.Text = Convert.ToDecimal(TxtVlrDesconto.Text).ToString("###,##0.00");
                    
                }
                else
                    TxtVlrDesconto.Text = "0,00";

            }
            CalculaTotal();

        }
        protected void txtVlrAcres_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (TxtVlrAcrescimo.Text.Equals(""))
            {
                TxtVlrAcrescimo.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor do Acréscimo", TxtVlrAcrescimo.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    TxtVlrAcrescimo.Text = Convert.ToDecimal(TxtVlrAcrescimo.Text).ToString("###,##0.00");
                   
                }
                else
                    TxtVlrAcrescimo.Text = "0,00";

            }
            CalculaTotal();
        }
        protected void txtVlrDocumento_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtVlrBaixa.Text.Equals(""))
            {
                txtVlrBaixa.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor do Documento", txtVlrBaixa.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtVlrBaixa.Text = Convert.ToDecimal(txtVlrBaixa.Text).ToString("###,##0.00");
                    
                }
                else
                    txtVlrBaixa.Text = "0,00";
            }
            CalculaTotal();
        }
        protected void CalculaTotal()
        {
            decimal doc = Convert.ToDecimal(txtVlrBaixa.Text);
            decimal acr = Convert.ToDecimal(TxtVlrAcrescimo.Text);
            decimal des = Convert.ToDecimal(TxtVlrDesconto.Text);
            decimal total = doc + acr - des;

            if (total < 0)
            {
                ShowMessage("Valor total deve ser maior que zero", MessageType.Info);
                TxtVlrDesconto.Text = "0,00";

                CalculaTotal();

            }
            else
            {
                TxtVlTotalbaixa.Text = Convert.ToString(total);
                

            }


        }


    }
}