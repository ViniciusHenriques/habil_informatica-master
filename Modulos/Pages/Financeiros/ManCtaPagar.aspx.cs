using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class ManCtaPagar : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }

        List<BaixaDocumento> ListaBaixa = new List<BaixaDocumento>();
        List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();

        List<Habil_Log> ListaLog = new List<Habil_Log>();

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


            Session["codigoCredor"] = null;
            ListaBaixa.RemoveAll(x => x.CodigoBaixa >= 0);
            grdBxCtaPagar.DataSource = ListaBaixa;
            grdBxCtaPagar.DataBind();

            Session["NovoEvento"] = null;
            ListaAnexo.RemoveAll(x => x.CodigoAnexo >= 0);
            grdAnexo.DataSource = ListaAnexo;
            grdAnexo.DataBind();

            DBTabelaDAL RnTab = new DBTabelaDAL();
            txtdtentrada.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm");
            
            txtVlrDocumento.Text = "0,00";
            txtVlrAcres.Text = "0,00";
            txtVlrDesc.Text = "0,00";
            txtVlrTotal.Text = "0,00";
            txtVlrPagar.Text = "0,00";
            txtVlrPago.Text = "0,00";
            txtCNPJCPFCredor.Text = "";
            txtRazSocial.Text = "";
            txtEndereco.Text = "";
            txtEstado.Text = "";
            txtCEP.Text = "";
            txtCidade.Text = "";
            txtBairro.Text = "";

            txtNroDocumento.Text = "";
            txtdtemissao.Text = "";
            txtdtvencimento.Text = "";
            txtOBS.Text = "";

            ddlAcao.Focus();

        }
        protected void CarregaTiposSituacoes()
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

            ddlTipoDocumento.DataSource = sd.ClassificaCtaPagar();
            ddlTipoDocumento.DataTextField = "DescricaoTipo";
            ddlTipoDocumento.DataValueField = "CodigoTipo";
            ddlTipoDocumento.DataBind();
            ddlTipoDocumento.Items.Insert(0, "SEM ORIGEM");

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
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Número Documento", txtNroDocumento.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtNroDocumento.Focus();

                }

                return false;
            }

            v.CampoValido("Data de Emissão", txtdtemissao.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtdtemissao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;
            }
            v.CampoValido("Data de Vencimento", txtdtvencimento.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtdtvencimento.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;
            }



            if ((ddlTipoCobranca.Text == "..... SELECIONE UM TIPO DE COBRANÇA ....."))
            {
                ShowMessage("Escolha um Tipo de Cobrança", MessageType.Info);
                ddlTipoCobranca.Focus();
                return false;
            }

            v.CampoValido("Código Credor", txtCodCredor.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);


            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodCredor.Focus();

                }

                return false;
            }
            Int64 numero;
            if (!Int64.TryParse(txtCodCredor.Text, out numero))
            {
                ShowMessage("Codigo do Credor incorreto", MessageType.Info);
                return false;
            }
            if ((ddlPlanoConta.Text == "..... SELECIONE UM PLANO DE CONTAS ....."))
            {
                ShowMessage("Escolha um Plano de contas", MessageType.Info);
                ddlPlanoConta.Focus();
                return false;
            }

            if (Convert.ToDecimal(txtVlrDocumento.Text) == 0)
            {
                ShowMessage("Valor documento deve ser maior que zero!", MessageType.Info);
                txtVlrDocumento.Focus();
                return false;
            }

            DateTime data1, data2;
            data1 = Convert.ToDateTime(txtdtvencimento.Text);
            data2 = DateTime.Today;
            if (data1 < data2)
            {
                ShowMessage("Data de Vencimento deve ser após a data de hoje!", MessageType.Info);
                txtdtvencimento.Focus();
                return false;
            }
            decimal totalAcrescimo = 0;
            decimal totalDesconto = 0;
            if (grdBxCtaPagar.Rows.Count > 0)
            {
                foreach (BaixaDocumento baixa in ListaBaixa)
                {
                    totalDesconto += baixa.ValorDesconto;
                    totalAcrescimo += baixa.ValorAcrescimo;

                }

                if (Convert.ToDecimal(txtVlrDesc.Text) < totalDesconto)
                {
                    ShowMessage("Valor de Descontos das Baixas não são coerentes com o Desconto do Documento", MessageType.Info);
                    return false;
                }

                if (Convert.ToDecimal(txtVlrAcres.Text) < totalAcrescimo)
                {
                    ShowMessage("Valor de Acréscimos das Baixas não são coerentes com o Acréscimo do Documento", MessageType.Info);
                    return false;
                }
            }
            

            return true;
        }
        protected void MontaTela(long CodRegra)
        {
            LimpaTela();
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


            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConCtaPagar.aspx");
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
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                btnReativar.Visible = false;
                if (Session["ZoomDocCtaPagar2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomDocCtaPagar"] != null)
                {
                    string s = Session["ZoomDocCtaPagar"].ToString();
                    Session["ZoomDocCtaPagar"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                txtCodDocumento.Text = word;
                                txtCodDocumento.Enabled = false;
                                CarregaTiposSituacoes();
                                Doc_CtaPagar doc = new Doc_CtaPagar();
                                Doc_CtaPagarDAL docDAL = new Doc_CtaPagarDAL();

                                doc = docDAL.PesquisarDocumento(Convert.ToDecimal(txtCodDocumento.Text));
                                txtdtemissao.Text = doc.DataEmissao.ToString().Substring(0, 10);
                                txtdtvencimento.Text = doc.DataVencimento.ToString().Substring(0, 10);
                                txtdtentrada.Text = doc.DataEntrada.ToString().Substring(0, 10);
                                ddlEmpresa.SelectedValue = Convert.ToString(doc.CodigoEmpresa);
                                ddlClassificacao.SelectedValue = Convert.ToString(doc.CodigoClassificacao);                               
                                txtNroDocumento.Text = Convert.ToString(doc.DGDocumento);
                                ddlTipoCobranca.SelectedValue = Convert.ToString(doc.CodigoCobranca);
                                ddlPlanoConta.SelectedValue = Convert.ToString(doc.CodigoPlanoContas);
                                txtOBS.Text = doc.ObservacaoDocumento;
                                txtVlrDesc.Text = Convert.ToString(doc.ValorDesconto);
                                txtVlrAcres.Text = Convert.ToString(doc.ValorAcrescimo);
                                txtVlrDocumento.Text = Convert.ToString(doc.ValorDocumento);
                                txtVlrTotal.Text = Convert.ToString(doc.ValorGeral);
                                txtCodCredor.Text = Convert.ToString(doc.CodigoPessoa);


                                SelectedCredor(sender, e);
                                btnNovBaixa.Visible = true;
                                ddlSituacao.SelectedValue = Convert.ToString(doc.CodigoSituacao);

                                if (doc.CodigoDocumentoOriginal != 0)
                                {
                                    Doc_CtaPagar docOriginal = new Doc_CtaPagar();
                                    docOriginal = docDAL.PesquisarDocumentoOriginal(doc.CodigoDocumentoOriginal);

                                    txtNroOriginal.Text = docOriginal.NumeroDocumento.ToString();
                                    txtSerieOriginal.Text = docOriginal.DGSRDocumento.ToString();
                                    txtCodOriginal.Text = docOriginal.CodigoDocumentoOriginal.ToString();
                                    ddlTipoDocumento.SelectedValue = docOriginal.CodigoTipoDocumento.ToString();


                                }

                                BaixaDocumentoDAL pe3 = new BaixaDocumentoDAL();
                                ListaBaixa = pe3.ObterBaixas(Convert.ToDecimal(txtCodDocumento.Text));
                                Session["NovaBaixa"] = ListaBaixa;

                                EventoDocumentoDAL eve = new EventoDocumentoDAL();
                                ListaEvento = eve.ObterEventos(Convert.ToDecimal(txtCodDocumento.Text));
                                Session["Eventos"] = ListaEvento;

                                Habil_LogDAL log = new Habil_LogDAL();
                                ListaLog = log.ListarLogs(Convert.ToDouble(txtCodDocumento.Text),100);
                                Session["Logs"] = ListaLog;

                                AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
                                ListaAnexo = anexo.ObterAnexos(Convert.ToDecimal(txtCodDocumento.Text));
                                Session["NovoAnexo"] = ListaAnexo;

                                PanelSelect = "home";
                                Session["TabFocadaManDocCtaPagar"] = "home";
                            }
                        }
                    }
                }
                else
                {
                    LimpaTela();
                    txtCodDocumento.Text = "Novo";
                    
                    if (Session["Ssn_TipoPessoa"] == null)
                    {
                        Session["NovaBaixa"] = null;
                        Session["Eventos"] = null;
                        Session["NovoAnexo"] = null;
                        Session["Logs"] = null;
                        btnCancelar.Visible = false;
                        btnExcluir.Visible = false;
                    }

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
                
                if (Session["Ssn_TipoPessoa"] != null)
                {

                    CarregaTiposSituacoes();
                    if (Session["Ssn_TipoPessoa2"] != null)
                    {
                        string s = Session["Ssn_TipoPessoa2"].ToString();
                        Session["Ssn_TipoPessoa2"] = null;

                        string[] words = s.Split('³');

                        if (s != "³")
                        {
                            foreach (string word in words)
                            {
                                if (word != "")
                                {
                                    txtCodCredor.Text = word;
                                    PreencheDados(Convert.ToInt32(txtCodCredor.Text));
                                    SelectedCredor(sender, e);
                                }

                            }
                        }
                    }
                    else
                    {
                        btnExcluir.Visible = true;
                        PreencheDados(0);
                        SelectedCredor(sender, e);
                    }

                    Session["Ssn_TipoPessoa"] = null;
                }

            }

            if (Session["NovaBaixa"] != null)
            {
                ListaBaixa = (List<BaixaDocumento>)Session["NovaBaixa"];
                grdBxCtaPagar.DataSource = ListaBaixa;
                grdBxCtaPagar.DataBind();
                

            }
            if (Session["Eventos"] != null)
            {
                ListaEvento = (List<EventoDocumento>)Session["Eventos"];
                GrdEventoDocumento.DataSource = ListaEvento;
                GrdEventoDocumento.DataBind();
                

            }
            if (Session["Logs"] != null)
            {
                ListaLog = (List<Habil_Log>)Session["Logs"];
                grdLogDocumento.DataSource = ListaLog;
                grdLogDocumento.DataBind();


            }
            if (Session["NovoAnexo"] != null)
            {
                ListaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];
                grdAnexo.DataSource = ListaAnexo;
                grdAnexo.DataBind();


            }
            if (ddlSituacao.SelectedValue == "38")
            {
                btnReativar.Visible = true;
                btnCancelar.Visible = false;
                btnNovBaixa.Visible = false;
                btnNovoAnexo.Visible = false;

            }
            if (txtCodDocumento.Text == "")
            {
                btnVoltar_Click(sender, e);
            }
            CalculaTotal();


        }
        protected void PreencheDados(int credor)
        {
            Doc_CtaPagar doc = new Doc_CtaPagar();
            doc = (Doc_CtaPagar)Session["Ssn_TipoPessoa"];

            if (doc.CodigoDocumento == 0)
                txtCodDocumento.Text = "Novo";
            else
                txtCodDocumento.Text = Convert.ToString(doc.CodigoDocumento);

            if (Convert.ToString(doc.DataEmissao) != "01/01/0001 00:00:00")
                txtdtemissao.Text = doc.DataEmissao.ToString().Substring(0, 10);
            if (Convert.ToString(doc.DataVencimento) != "01/01/0001 00:00:00")
                txtdtvencimento.Text = doc.DataVencimento.ToString().Substring(0, 10);
            txtdtentrada.Text = doc.DataEntrada.ToString().Substring(0, 10);
            ddlEmpresa.SelectedValue = Convert.ToString(doc.CodigoEmpresa);
            ddlClassificacao.SelectedValue = Convert.ToString(doc.CodigoClassificacao);
            txtNroDocumento.Text = Convert.ToString(doc.DGDocumento);
            ddlTipoCobranca.SelectedValue = Convert.ToString(doc.CodigoCobranca);
            ddlSituacao.SelectedValue = Convert.ToString(doc.CodigoSituacao);
            ddlAcao.SelectedValue = doc.Cpl_Acao.ToString();
            ddlPlanoConta.SelectedValue = Convert.ToString(doc.CodigoPlanoContas);
            txtOBS.Text = doc.ObservacaoDocumento;
            txtVlrDesc.Text = Convert.ToString(doc.ValorDesconto);
            txtVlrAcres.Text = Convert.ToString(doc.ValorAcrescimo);
            txtVlrDocumento.Text = Convert.ToString(doc.ValorDocumento);
            txtVlrTotal.Text = Convert.ToString(doc.ValorGeral);

            if (doc.CodigoDocumentoOriginal != 0)
            {
                Doc_CtaPagarDAL docDAL = new Doc_CtaPagarDAL();
                Doc_CtaPagar docOriginal = new Doc_CtaPagar();
                docOriginal = docDAL.PesquisarDocumentoOriginal(doc.CodigoDocumentoOriginal);

                txtNroOriginal.Text = docOriginal.NumeroDocumento.ToString();
                txtSerieOriginal.Text = docOriginal.DGSRDocumento.ToString();
                txtCodOriginal.Text = docOriginal.CodigoDocumentoOriginal.ToString();
                ddlTipoDocumento.SelectedValue = docOriginal.CodigoTipoDocumento.ToString();
            }
            
            if (credor == 0)
            {
                if (doc.CodigoPessoa == 0)
                    txtCodCredor.Text = "";
                else
                    txtCodCredor.Text = Convert.ToString(doc.CodigoPessoa);
            }
            CalculaTotal();

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            Doc_CtaPagarDAL doc = new Doc_CtaPagarDAL();
            doc.Excluir(Convert.ToDecimal(txtCodDocumento.Text));
            Session["MensagemTela"] = "Documento Excluído com sucesso!";
            ddlSituacao.SelectedValue = Convert.ToString(37);
            Response.Redirect("~/Pages/financeiros/ConCtaPagar.aspx");

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            btnCancelar.Visible = false;
            btnReativar.Visible = true;
            btnNovBaixa.Visible = false;
            btnNovoAnexo.Visible = false;

            
            List<BaixaDocumento> bx = new List<BaixaDocumento>();
           
            
            foreach (BaixaDocumento b in ListaBaixa)
            {

                BaixaDocumento novaBx = new BaixaDocumento(b.CodigoBaixa,
                                                           b.DataBaixa,
                                                           b.ValorBaixa,
                                                           b.DataLancamento,
                                                           b.ValorDesconto,
                                                           b.ValorAcrescimo,
                                                           b.ValorTotalBaixa,
                                                           b.CodigoTipoCobranca,
                                                           b.CodigoContaCorrente,
                                                           b.Observacao, 
                                                           35,
                                                           b.Cpl_Cobranca);
                    
                bx.Add(novaBx);
            }
            
            grdBxCtaPagar.DataSource =bx;
            grdBxCtaPagar.DataBind();
            CalculaTotal();
            Session["NovaBaixa"] = bx;
            ShowMessage("Documento foi Baixado por Cancelamento!", MessageType.Info);


        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Financeiros/ConCtaPagar.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;
            Doc_CtaPagar p = new Doc_CtaPagar();
            Doc_CtaPagarDAL pe = new Doc_CtaPagarDAL();
            

            // p.CodigoDocumento = Convert.ToInt32(txtCodDocumento.Text);
            p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            p.DataEntrada = DateTime.Today;
            p.CodigoTipoDocumento = 4;
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.DGDocumento = txtNroDocumento.Text;
            p.DataEmissao = Convert.ToDateTime(txtdtemissao.Text);
            p.DataVencimento = Convert.ToDateTime(txtdtvencimento.Text);
            p.CodigoPessoa = Convert.ToInt32(txtCodCredor.Text);
            p.DGSRDocumento = " ";
            p.CodigoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);
            p.CodigoPlanoContas = Convert.ToInt32(ddlPlanoConta.SelectedValue);
            p.ObservacaoDocumento = txtOBS.Text;
            p.ValorDocumento = Convert.ToDecimal(txtVlrDocumento.Text);
            p.ValorAcrescimo = Convert.ToDecimal(txtVlrAcres.Text);
            p.ValorDesconto = Convert.ToDecimal(txtVlrDesc.Text);
            p.ValorGeral = Convert.ToDecimal(txtVlrTotal.Text);
            p.CodigoClassificacao = Convert.ToInt32(ddlClassificacao.SelectedValue);

            if (txtCodOriginal.Text != "")
                p.CodigoDocumentoOriginal = Convert.ToDecimal(txtCodOriginal.Text);

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
            p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
            p.Cpl_Usuario = Convert.ToInt32(Session["CodPflUsuario"]);

            if (txtCodDocumento.Text == "Novo")
            {
                pe.Inserir(p,ListaBaixa, EventoDocumento(),ListaAnexo);
            }
            else
            {
                Doc_CtaPagar p2 = new Doc_CtaPagar();

                p.CodigoDocumento = Convert.ToDecimal(txtCodDocumento.Text);
                p2 = pe.PesquisarDocumento(Convert.ToInt32(txtCodDocumento.Text));

                if (Convert.ToInt32(ddlSituacao.SelectedValue) != p2.CodigoSituacao)
                    pe.Atualizar(p, ListaBaixa, EventoDocumento(),ListaAnexo);
                else
                    pe.Atualizar(p,ListaBaixa,null,ListaAnexo);
            }


            if (Convert.ToInt32(ddlAcao.SelectedValue) == 0)
            {
                Session["MensagemTela"] = null;
                if (txtCodDocumento.Text == "Novo")
                    Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
                else
                    Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
                btnVoltar_Click(sender, e);
            }

            if (Convert.ToInt32(ddlAcao.SelectedValue) == 1)
            {
                p = pe.PesquisarDocumento(Convert.ToInt32(p.CodigoDocumento));
                CarregaTiposSituacoes();
                

                EventoDocumentoDAL eve = new EventoDocumentoDAL();
                ListaEvento = eve.ObterEventos(p.CodigoDocumento);
                Session["Eventos"] = ListaEvento;
                GrdEventoDocumento.DataSource = ListaEvento;
                GrdEventoDocumento.DataBind();

                Habil_LogDAL log = new Habil_LogDAL();
                ListaLog = log.ListarLogs(Convert.ToDouble(p.CodigoDocumento), 100);
                Session["Logs"] = ListaLog;
                grdLogDocumento.DataSource = ListaLog;
                grdLogDocumento.DataBind();


                Session["MensagemTela"] = null;
                if (txtCodDocumento.Text == "Novo")
                    ShowMessage("Registro Incluído com Sucesso!!! Continue Editando...", MessageType.Info);
                else
                    ShowMessage("Registro Alterado com Sucesso!!! Continue Editando...", MessageType.Info);

                txtCodDocumento.Text = Convert.ToString(p.CodigoDocumento);
                txtdtentrada.Text = p.DataEntrada.ToString().Substring(0, 10);
                ddlEmpresa.SelectedValue = Convert.ToString(p.CodigoEmpresa);
                ddlClassificacao.SelectedValue = Convert.ToString(p.CodigoClassificacao);
                ddlSituacao.SelectedValue = Convert.ToString(p.CodigoSituacao);
                txtNroDocumento.Text = Convert.ToString(p.DGDocumento);
                txtdtemissao.Text = p.DataEmissao.ToString().Substring(0, 10);
                txtdtvencimento.Text = p.DataVencimento.ToString().Substring(0, 10);
                ddlTipoCobranca.Text = Convert.ToString(p.CodigoCobranca);
                txtCodCredor.Text = Convert.ToString(p.CodigoPessoa);               
                ddlPlanoConta.Text = Convert.ToString(p.CodigoPlanoContas);
                txtOBS.Text = p.ObservacaoDocumento;
                txtVlrTotal.Text = Convert.ToString(p.ValorGeral);
                txtVlrAcres.Text = Convert.ToString(p.ValorAcrescimo);
                txtVlrDesc.Text = Convert.ToString(p.ValorDesconto);
                txtVlrDocumento.Text = Convert.ToString(p.ValorDocumento);
                
                CalculaTotal();
                SelectedCredor(sender, e);

                btnNovBaixa.Visible = true;
                btnNovoAnexo.Visible = true;
            }

            if (Convert.ToInt32(ddlAcao.SelectedValue) == 2)
            {
                
                Session["MensagemTela"] = null;

                if (txtCodDocumento.Text == "Novo")
                    ShowMessage("Registro Incluído com Sucesso!!! Continue incluindo...", MessageType.Info);
                else
                    ShowMessage("Registro Alterado com Sucesso!!! Continue incluindo...", MessageType.Info);

                txtCodDocumento.Text = "Novo";

                txtCodOriginal.Text = "";
                txtNroOriginal.Text = "";
                txtSerieOriginal.Text = "";
                ddlTipoDocumento.SelectedValue = "SEM ORIGEM";


                Session["NovaBaixa"] = null;
                Session["Eventos"] = null;
                Session["Logs"] = null;
                
                grdBxCtaPagar.DataSource = null;
                grdBxCtaPagar.DataBind();

                GrdEventoDocumento.DataSource = null;
                GrdEventoDocumento.DataBind();

                grdLogDocumento.DataSource = null;
                grdLogDocumento.DataBind();

                grdAnexo.DataSource = null;
                grdAnexo.DataBind();


                btnNovBaixa.Visible = false;
                btnNovoAnexo.Visible = false;

                CalculaTotal();  
            }
        }
        protected void txtVlrDesc_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtVlrDesc.Text.Equals(""))
            {
                txtVlrDesc.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor do Desconto", txtVlrDesc.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtVlrDesc.Text = Convert.ToDecimal(txtVlrDesc.Text).ToString("###,##0.00");
                    
                }
                else
                    txtVlrDesc.Text = "0,00";

            }
            CalculaTotal();

        }
        protected void txtVlrAcres_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtVlrAcres.Text.Equals(""))
            {
                txtVlrAcres.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor do Acréscimo", txtVlrAcres.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    
                    txtVlrAcres.Text = Convert.ToDecimal(txtVlrAcres.Text).ToString("###,##0.00");
                    
                }
                else
                    txtVlrAcres.Text = "0,00";

            }
            CalculaTotal();
        }
        protected void txtVlrDocumento_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtVlrDocumento.Text.Equals(""))
            {
                txtVlrDocumento.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor do Documento", txtVlrDocumento.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtVlrDocumento.Text = Convert.ToDecimal(txtVlrDocumento.Text).ToString("###,##0.00");
                    
                }
                else
                    txtVlrDocumento.Text = "0,00";
            }
            CalculaTotal();
        }
        protected void btnNovBaixa_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;
            CompactaDocumento();
            Session["NovaBaixa"] = ListaBaixa;
            Response.Redirect("~/Pages/Financeiros/ManCtaBaixa.aspx?cad=1");
        }
        protected void btnNovAnexo_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;
            CompactaDocumento();
            Session["NovaBaixa"] = ListaBaixa;
            Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=1");
        }
        protected void btnReativar_Click(object sender, EventArgs e)
        {
            btnCancelar.Visible = true;
            btnReativar.Visible = false;
            btnNovBaixa.Visible = true;
            btnNovoAnexo.Visible = true;
            List<BaixaDocumento> bx = new List<BaixaDocumento>();
            ListaBaixa = (List<BaixaDocumento>)Session["NovaBaixa"];

            foreach (BaixaDocumento b in ListaBaixa)
            {

                BaixaDocumento novaBx = new BaixaDocumento(b.CodigoBaixa,
                                                           b.DataBaixa,
                                                           b.ValorBaixa,
                                                           b.DataLancamento,
                                                           b.ValorDesconto,
                                                           b.ValorAcrescimo,
                                                           b.ValorTotalBaixa,
                                                           b.CodigoTipoCobranca,
                                                           b.CodigoContaCorrente,
                                                           b.Observacao,
                                                           33,
                                                           b.Cpl_Cobranca);

                bx.Add(novaBx);


            }
            grdBxCtaPagar.DataSource = bx;
            grdBxCtaPagar.DataBind();
            CalculaTotal();
            Session["NovaBaixa"] = bx;
            
            
            
            ShowMessage("Lançamento Reativado com sucesso!!!", MessageType.Info);


        }
        protected void grdBxCtaPagar_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["ZoomDocCtaBaixa"] = HttpUtility.HtmlDecode(grdBxCtaPagar.SelectedRow.Cells[0].Text);
            Response.Redirect("~/Pages/financeiros/ManCtaBaixa.aspx?cad=1");
        }
        protected void CompactaDocumento()
        {
            Doc_CtaPagar contasPgar = new Doc_CtaPagar();
            contasPgar.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            contasPgar.DataEntrada = Convert.ToDateTime(txtdtentrada.Text);
            contasPgar.CodigoTipoDocumento = 5;
            contasPgar.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            contasPgar.DGDocumento = txtNroDocumento.Text;
            if (txtCodDocumento.Text == "Novo")
                contasPgar.CodigoDocumento = 0;
            else
                contasPgar.CodigoDocumento = Convert.ToDecimal(txtCodDocumento.Text);

            if (txtdtemissao.Text != "")
                contasPgar.DataEmissao = Convert.ToDateTime(txtdtemissao.Text);
            if (txtdtvencimento.Text != "")
                contasPgar.DataVencimento = Convert.ToDateTime(txtdtvencimento.Text);

            contasPgar.CodigoCobranca = 0;
            if (ddlTipoCobranca.Text != "..... SELECIONE UM TIPO DE COBRANÇA .....")
                contasPgar.CodigoCobranca = Convert.ToInt32(ddlTipoCobranca.SelectedValue);

            contasPgar.CodigoPlanoContas = 0;
            if (ddlPlanoConta.Text != "..... SELECIONE UM PLANO DE CONTAS .....")
                contasPgar.CodigoPlanoContas = Convert.ToInt32(ddlPlanoConta.SelectedValue);

            contasPgar.DGSRDocumento = "";
            contasPgar.ObservacaoDocumento = txtOBS.Text;
            contasPgar.ValorDocumento = Convert.ToDecimal(txtVlrDocumento.Text);
            contasPgar.ValorAcrescimo = Convert.ToDecimal(txtVlrAcres.Text);
            contasPgar.ValorDesconto = Convert.ToDecimal(txtVlrDesc.Text);
            contasPgar.ValorGeral = Convert.ToDecimal(txtVlrTotal.Text);
            contasPgar.CodigoClassificacao = Convert.ToInt32(ddlClassificacao.SelectedValue);
            contasPgar.Cpl_Acao = Convert.ToInt32(ddlAcao.SelectedValue);
            if (txtCodCredor.Text == "")
                contasPgar.CodigoPessoa = 0;
            else
                contasPgar.CodigoPessoa = Convert.ToInt32(txtCodCredor.Text);

            if (txtCodOriginal.Text != "")
                contasPgar.CodigoDocumentoOriginal = Convert.ToDecimal(txtCodOriginal.Text);

            decimal vlAcres = Convert.ToDecimal(txtVlrAcres.Text);
            decimal vlDesc = Convert.ToDecimal(txtVlrDesc.Text);
            decimal vlDoc = Convert.ToDecimal(txtVlrDocumento.Text);
            if (grdBxCtaPagar.Rows.Count > 0)
            {

                foreach (BaixaDocumento baixa in ListaBaixa)
                {
                    vlAcres =  vlAcres - baixa.ValorAcrescimo;
                    vlDesc = vlDesc - baixa.ValorDesconto;
                    vlDoc = vlDoc - baixa.ValorBaixa;

                }
            }
            contasPgar.Cpl_vlPagar = vlDoc;
            contasPgar.Cpl_vlAcres = vlAcres;
            contasPgar.Cpl_vlDesc = vlDesc;

            Session["Ssn_TipoPessoa"] = contasPgar;
        }
        protected void ConCredor(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=3");

        }
        protected void SelectedCredor(object sender, EventArgs e)
        {

            Boolean blnCampo = false;

            if (txtCodCredor.Text.Equals(""))
            {

                return;
            }
            else
            {
                v.CampoValido("Codigo Credor", txtCodCredor.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (!blnCampo)
                {
                    txtCodCredor.Text = "";
                    return;

                }


            }
            Int64 numero;
            if (!Int64.TryParse(txtCodCredor.Text, out numero))
            {
                return;
            }

            if (txtCodDocumento.Text != "Novo")
            {
                Doc_CtaPagar doc = new Doc_CtaPagar();
                Doc_CtaPagarDAL docDAL = new Doc_CtaPagarDAL();
                doc = docDAL.PesquisarDocumento(Convert.ToInt32(txtCodDocumento.Text));
                if (doc.CodigoPessoa != Convert.ToInt32(txtCodCredor.Text))
                {
                    ShowMessage("Você alterou o credor! A Pessoa do Documento irá mudar...", MessageType.Info);
                }
            }


            Int64 codigoCredor = Convert.ToInt64(txtCodCredor.Text);
            PessoaDAL pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();

            PessoaInscricaoDAL ins = new PessoaInscricaoDAL();
            List<Pessoa_Inscricao> ins2 = new List<Pessoa_Inscricao>();
            ins2 = ins.ObterPessoaInscricoes(codigoCredor);
            p2 = pessoa.PesquisarFornecedor(codigoCredor);

            if (p2 == null)
            {
                ShowMessage("Credor não existente!", MessageType.Info);
                txtCodCredor.Text = "";
                txtCredor.Text = "";
                txtCNPJCPFCredor.Text = "";
                txtCidade.Text = "";
                txtBairro.Text = "";
                txtRazSocial.Text = "";
                txtEndereco.Text = "";
                txtEstado.Text = "";
                txtCEP.Text = "";
                txtCodCredor.Focus();

                return;
            }
            txtRazSocial.Text = p2.NomePessoa;
            txtCredor.Text = p2.NomePessoa;

            foreach (Pessoa_Inscricao inscricao in ins2)
            {
                if (inscricao._CodigoItem == 1)
                {
                    txtCNPJCPFCredor.Text = Convert.ToString(inscricao._NumeroInscricao);
                }
            }

            PessoaEnderecoDAL end = new PessoaEnderecoDAL();
            List<Pessoa_Endereco> end2 = new List<Pessoa_Endereco>();
            end2 = end.ObterPessoaEnderecos(codigoCredor);
            foreach (Pessoa_Endereco endereco in end2)
            {
                if (endereco._CodigoInscricao == 1)
                {
                    txtEndereco.Text = Convert.ToString(endereco._Logradouro + ", " + endereco._NumeroLogradouro + " - " + endereco._Complemento);

                    txtCEP.Text = Convert.ToString(endereco._CodigoCEP);
                    txtEstado.Text = endereco._DescricaoEstado.Substring(0, 2);
                    txtCidade.Text = endereco._DescricaoMunicipio;
                    txtBairro.Text = endereco._DescricaoBairro;

                }
            }


            Session["TabFocadaManDocCtaPagar"] = null;
            Session["Ssn_TipoPessoa"] = null;
        }
        protected void CalculaTotal()
        {
            decimal doc = Convert.ToDecimal(txtVlrDocumento.Text);
            decimal acr = Convert.ToDecimal(txtVlrAcres.Text);
            decimal des = Convert.ToDecimal(txtVlrDesc.Text);
            decimal total = doc + acr - des;

            if (total < 0)
            {
                ShowMessage("Valor total deve ser maior que zero", MessageType.Info);
                txtVlrDesc.Text = "0,00";

                CalculaTotal();

            }
            else
            {
                txtVlrTotal.Text = Convert.ToString(total);
                decimal totalPagar = total;
                decimal totalPago = total - totalPagar;

                {
                                     
                    foreach (BaixaDocumento baixa in ListaBaixa)
                    {
                        totalPagar = totalPagar - baixa.ValorTotalBaixa;
                        totalPago = totalPago + baixa.ValorTotalBaixa;

                    }
                }
                

                txtVlrPagar.Text = Convert.ToString(totalPagar);
                txtVlrPago.Text = Convert.ToString(totalPago);

                if (txtCodDocumento.Text == "Novo")
                {
                    txtVlrPago.Text = "0,00";
                    txtVlrPagar.Text = txtVlrTotal.Text;
                    ddlSituacao.SelectedValue = "31";
                    btnNovBaixa.Visible = false;
                    btnNovoAnexo.Visible = false;
                    btnExcluir.Visible = false;
                    btnCancelar.Visible = false;
                    btnReativar.Visible = false;
                    txtVlrAcres.Enabled = true;
                    txtVlrDesc.Enabled = true;
                }
                else
                {
                    btnNovBaixa.Visible = true;
                    btnExcluir.Visible = true;
                    
                    if (btnReativar.Visible == true)
                    {
                        ddlSituacao.SelectedValue = "38";
                        btnNovBaixa.Visible = false;
                        btnNovoAnexo.Visible = false;

                    }
                    else
                    {

                         if (Convert.ToDecimal(txtVlrTotal.Text) == Convert.ToDecimal(txtVlrPago.Text) && Convert.ToDecimal(txtVlrPago.Text) > 0)
                        {
                            ddlSituacao.SelectedValue = "36";
                            txtVlrAcres.Enabled = false;
                            txtVlrDesc.Enabled = false;
                        }
                        else 
                        {
                            ddlSituacao.SelectedValue = "31";
                            txtVlrAcres.Enabled = true;
                            txtVlrDesc.Enabled = true;
                        }
                        btnReativar.Visible = false;
                        btnCancelar.Visible = true;
                        btnNovBaixa.Visible = true;
                        btnNovoAnexo.Visible = true;
                        

                    }
                    


                }
            }          
        }
        protected EventoDocumento EventoDocumento()
        {          

            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime DataHoraEvento = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm"));

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            int intCttItem = 0;

            if (GrdEventoDocumento.Rows.Count != 0)
                intCttItem = Convert.ToInt32(ListaEvento.Max(x => x.CodigoEvento).ToString());


            intCttItem = intCttItem + 1;
            
            if (intCttItem != 0)
                ListaEvento.RemoveAll(x => x.CodigoEvento == intCttItem);
            EventoDocumento evento = new EventoDocumento(intCttItem,
                                                       Convert.ToInt32(ddlSituacao.SelectedValue),
                                                       DataHoraEvento,
                                                       he.CodigoEstacao,
                                                       Convert.ToInt32(Session["CodUsuario"]));
            return evento;

        }
        protected void grdLogDocumento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdLogDocumento.PageIndex = e.NewPageIndex;
            PanelSelect = "consulta4";
            // Carrega os dados

        }
        protected void grdLogDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void GrdEventoDocumento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void grdAnexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["ZoomDocCtaAnexo"] = HttpUtility.HtmlDecode(grdAnexo.SelectedRow.Cells[0].Text);
            Response.Redirect("~/Pages/financeiros/ManAnexo.aspx?cad=1");
        }
    }
}