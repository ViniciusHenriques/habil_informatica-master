using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Drawing;
using System.Text;
using System.IO;
using System.Xml;

namespace SoftHabilInformatica.Pages.Transporte
{
    public partial class ManCTe : System.Web.UI.Page
    {
        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
        List<EventoEletronicoDocumento> ListaEventoDocEletronico = new List<EventoEletronicoDocumento>();
        public string Panels { get; set; }
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaCampos()
        {
            CarregaSituacoes();

            DBTabelaDAL RnTab = new DBTabelaDAL();
            txtdtemissao.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss");
            txtdtLancamento.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss");
            //txtDescricao.Text = "";
            txtNroDocumento.Text = "";
            txtNroSerie.Text = "";
            txtCodigo.Text = "Novo";
            txtOBS.Text = "";
            txtValor.Text = "0,00";
        }

        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.SituacaoCTe();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();
            ddlSituacao.SelectedValue = "42";

            EmpresaDAL RnEmpresa = new EmpresaDAL();
            ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("CD_SITUACAO", "INT", "1", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();
            if (Session["CodEmpresa"] != null)
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();

            TipoOperacaoDAL TipoOP = new TipoOperacaoDAL();
            ddlTipoOperacao.DataSource = TipoOP.ListarTipoOperacoes("CD_SITUACAO", "INT", "1", "");
            ddlTipoOperacao.DataTextField = "DescricaoTipoOperacao";
            ddlTipoOperacao.DataValueField = "CodigoTipoOperacao";
            ddlTipoOperacao.DataBind();
            ddlTipoOperacao.Items.Insert(0, "..... SELECIONE UM TIPO DE OPERAÇÃO.....");

        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;
            if (ddlTipoOperacao.SelectedValue == "..... SELECIONE UM TIPO DE OPERAÇÃO.....")
            {
                ShowMessage("Selecione um tipo de operação", MessageType.Info);
                ddlTipoOperacao.Focus();
                return false;
            }
            if (txtChaveAcesso.Text.Length != 44)
            {
                ShowMessage("A chave de acesso deve ter 44 dígitos.", MessageType.Info);
                txtChaveAcesso.Focus();
                return false;
            }
            v.CampoValido("Data de Emissão", txtdtemissao.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtdtemissao.Focus();
                }
                return false;
            }
            v.CampoValido("Data de Lançamento", txtdtLancamento.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtdtLancamento.Focus();
                }
                return false;
            }

            v.CampoValido("Código Transportadora", txtCodTransportador.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodTransportador.Focus();
                }
                return false;
            }
            v.CampoValido("Código Tomador", txtCodTomador.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodTomador.Focus();
                }
                return false;
            }
            v.CampoValido("Código Recebedor", txtCodRecebedor.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodRecebedor.Focus();
                }
                return false;
            }
            v.CampoValido("Código Remetente", txtCodRemetente.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodRemetente.Focus();
                }
                return false;
            }

            v.CampoValido("Código Destinatário", txtCodDestinatario.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodDestinatario.Focus();
                }
                return false;
            }
            
            v.CampoValido("Valor Total", txtValor.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtValor.Focus();
                }
                return false;
            }
            if(Convert.ToDecimal(txtValor.Text) == 0)
            {
                ShowMessage("Digite um valor válido", MessageType.Info);
                txtValor.Focus();
                return false;
            }
            if(txtOBS.Text.Length > 300)
            {
                ShowMessage("Observação excedeu o número de caracteres!", MessageType.Info);
                return false;
            }

            Pessoa_Endereco endTransp = new Pessoa_Endereco();
            PessoaEnderecoDAL endDAL = new PessoaEnderecoDAL();
            endTransp = endDAL.PesquisarPessoaEndereco(Convert.ToInt64(txtCodTransportador.Text), 1);
            if(endTransp._DescricaoEstado == "" || endTransp._DescricaoEstado == " " || endTransp._DescricaoEstado == "   ")
            {
                ShowMessage("Atualize cadastro do transportador, UF inexistente", MessageType.Info);
                return false;
            }
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocada"] != null)
            {
                PanelSelect = Session["TabFocada"].ToString();
                Session["TabFocada"] = null;
            }
            else
                if (!IsPostBack)
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConCTe.aspx");
            lista.ForEach(delegate (Permissao x)
            {
                if (x.Liberado)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoAlterar)
                            btnSalvar.Visible = false;
                        if (!x.AcessoExcluir)
                            btnExcluir.Visible = false;
                        if (!x.AcessoIncluir)
                            BtnEnviarDesacordo.Visible = false;
                    }
                }
                else
                    Response.Redirect("~/Pages/Welcome.aspx");
            });

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomCTe2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomCTe"] != null)
                {
                    string s = Session["ZoomCTe"].ToString();
                    Session["ZoomCTe"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                CarregaSituacoes();
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                Doc_CTe doc = new Doc_CTe();
                                Doc_CTeDAL docDAL = new Doc_CTeDAL();
                                doc = docDAL.PesquisarDocumento(Convert.ToDecimal(txtCodigo.Text));

                                txtdtemissao.Text = doc.DataHoraEmissao.ToString();
                                txtdtLancamento.Text = doc.DataHoraLancamento.ToString();
                                ddlSituacao.SelectedValue = doc.CodigoSituacao.ToString();
                                ddlTipoOperacao.SelectedValue = doc.CodigoTipoOperacao.ToString();
                                ddlEmpresa.SelectedValue = doc.CodigoEmpresa.ToString();
                                txtNroDocumento.Text = doc.NumeroDocumento.ToString();
                                txtNroSerie.Text = doc.DGSRDocumento.ToString();
                                txtChaveAcesso.Text = doc.ChaveAcesso;

                                txtCodTransportador.Text = doc.Cpl_CodigoTransportador.ToString();
                                txtCodTransportador_TextChanged(sender, e);                         

                                txtCodRemetente.Text = doc.Cpl_CodigoRemetente.ToString();
                                txtCodRemetente_TextChanged(sender, e);

                                txtCodDestinatario.Text = doc.Cpl_CodigoDestinatario.ToString();
                                txtCodDestinatario_TextChanged(sender, e);

                                txtCodTomador.Text = doc.Cpl_CodigoTomador.ToString();
                                txtCodTomador_TextChanged(sender, e);

                                txtCodRecebedor.Text = doc.Cpl_CodigoRecebedor.ToString();
                                txtCodRecebedor_TextChanged(sender, e);

                                txtValor.Text = doc.ValorTotal.ToString();
                                txtOBS.Text = doc.ObservacaoDocumento;


                                AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
                                ListaAnexo = anexoDAL.ObterAnexos(Convert.ToDecimal(txtCodigo.Text));
                                Session["NovoAnexo"] = ListaAnexo;

                                EventoEletronicoDocumentoDAL eventoDAL = new EventoEletronicoDocumentoDAL();
                                ListaEventoDocEletronico = eventoDAL.ObterEventosEletronicos(Convert.ToInt32(txtCodigo.Text));
                                Session["ListaEventoDocEletronico"] = ListaEventoDocEletronico;

                                lista.ForEach(delegate (Permissao x)
                                {
                                    if (!x.AcessoCompleto)
                                    {
                                        if (!x.AcessoAlterar)
                                            btnSalvar.Visible = false;
                                        if (!x.AcessoExcluir)
                                            btnExcluir.Visible = false;
                                        if (!x.AcessoIncluir)
                                            BtnEnviarDesacordo.Visible = false;
                                    }
                                });
                            }
                        }
                    }
                }
                else
                {
                    LimpaCampos();
                    CarregaSituacoes();
                    
                    ddlEmpresa_TextChanged(sender, e);
                    
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                            {
                                btnSalvar.Visible = false;
                                BtnEnviarDesacordo.Visible = false;
                            }
                        }
                    });
                }              
            }
            if (Session["Doc_CTe"] != null)
            {
                CarregaSituacoes();
                PreencheDados(sender, e);
            }
            if (Session["ZoomTransportador"] != null)
            {
                txtCodTransportador.Text = Session["ZoomTransportador"].ToString();
                txtCodTransportador_TextChanged(sender, e);
                Session["ZoomTransportador"] = null;
            }
            else if (Session["ZoomRemetente"] != null)
            {
                txtCodRemetente.Text = Session["ZoomRemetente"].ToString();
                txtCodRemetente_TextChanged(sender, e);
                Session["ZoomRemetente"] = null;
            }
            else if (Session["ZoomDestinatario"] != null)
            {
                txtCodDestinatario.Text = Session["ZoomDestinatario"].ToString();
                txtCodDestinatario_TextChanged(sender, e);
                Session["ZoomDestinatario"] = null;
            }
            else if (Session["ZoomTomador"] != null)
            {
                txtCodTomador.Text = Session["ZoomTomador"].ToString();
                txtCodTomador_TextChanged(sender, e);
                Session["ZoomTomador"] = null;
            }
            else if (Session["ZoomRecebedor"] != null)
            {
                txtCodRecebedor.Text = Session["ZoomRecebedor"].ToString();
                txtCodRecebedor_TextChanged(sender, e);
                Session["ZoomRecebedor"] = null;
            }
            if (Session["NovoAnexo"] != null)
            {
                ListaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];
                grdAnexo.DataSource = ListaAnexo;
                grdAnexo.DataBind();
            }
            if (Session["ListaEventoDocEletronico"] != null)
            {
                ListaEventoDocEletronico = (List<EventoEletronicoDocumento>)Session["ListaEventoDocEletronico"];
                ListaEventoDocEletronico = ListaEventoDocEletronico.OrderBy(x => x.CodigoEvento).ToList();
                grdEventoEletronico.DataSource = ListaEventoDocEletronico;
                grdEventoEletronico.DataBind();
            }

            if (txtCodigo.Text == "")
                Response.Redirect("~/Pages/Transporte/ConCTe.aspx");
            else if (txtCodigo.Text == "Novo")
            {
                Panels = "display:none";
                btnNovoAnexo.Visible = false;
                BtnEnviarDesacordo.Visible = false;
                btnExcluir.Visible = false;
            }
            else
            {
                Panels = "display:block";
                btnNovoAnexo.Visible = true;
            }

        }

        protected void PreencheDados(object sender, EventArgs e)
        {
            if (Session["Doc_CTe"] == null)
                return;
            Doc_CTe doc = (Doc_CTe)Session["Doc_CTe"];
            if (doc.CodigoDocumento == 0)
                txtCodigo.Text = "Novo";
            else
                txtCodigo.Text = doc.CodigoDocumento.ToString();

            txtdtemissao.Text = doc.DataHoraEmissao.ToString();
            txtdtLancamento.Text = doc.DataHoraLancamento.ToString();
            ddlSituacao.SelectedValue = doc.CodigoSituacao.ToString();
            ddlEmpresa.SelectedValue = doc.CodigoEmpresa.ToString();
            ddlTipoOperacao.SelectedValue = doc.CodigoTipoOperacao.ToString();
            txtNroDocumento.Text = doc.NumeroDocumento.ToString();
            txtNroSerie.Text = doc.DGSRDocumento;
            txtChaveAcesso.Text = doc.ChaveAcesso;
            txtOBS.Text = doc.ObservacaoDocumento;
            txtValor.Text = doc.ValorTotal.ToString();
            
            if (doc.Cpl_CodigoTransportador != 0)
            {
                txtCodTransportador.Text = doc.Cpl_CodigoTransportador.ToString();
                txtCodTransportador_TextChanged(sender, e);
            }
            if (doc.Cpl_CodigoRemetente != 0)
            {
                txtCodRemetente.Text = doc.Cpl_CodigoRemetente.ToString();
                txtCodRemetente_TextChanged(sender, e);
            }
            if (doc.Cpl_CodigoDestinatario != 0)
            {
                txtCodDestinatario.Text = doc.Cpl_CodigoDestinatario.ToString();
                txtCodDestinatario_TextChanged(sender, e);
            }
            if (doc.Cpl_CodigoTomador != 0)
            {
                txtCodTomador.Text = doc.Cpl_CodigoTomador.ToString();
                txtCodTomador_TextChanged(sender, e);
            }
            if (doc.Cpl_CodigoRecebedor != 0)
            {
                txtCodRecebedor.Text = doc.Cpl_CodigoRecebedor.ToString();
                txtCodRecebedor_TextChanged(sender, e);
            }
            Session["LST_CADPESSOA"] = null;
            Session["Doc_CTe"] = null;
        }

        protected void CompactaDocumento()
        {
            
            Doc_CTe doc = new Doc_CTe();
            if (txtCodigo.Text == "Novo")
                doc.CodigoDocumento = 0;
            else
                doc.CodigoDocumento = Convert.ToDecimal(txtCodigo.Text);
            doc.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
            doc.DataHoraLancamento = Convert.ToDateTime(txtdtLancamento.Text);
            doc.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            if (ddlTipoOperacao.SelectedValue == "..... SELECIONE UM TIPO DE OPERAÇÃO.....")
                doc.CodigoTipoOperacao = 0;
            else
                doc.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);

            doc.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            doc.ChaveAcesso = txtChaveAcesso.Text;
            doc.ObservacaoDocumento = txtOBS.Text;
            doc.ValorTotal = Convert.ToDecimal(txtValor.Text);
            doc.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
            doc.DGSRDocumento = txtNroSerie.Text;

            if(txtCodTransportador.Text != "")
                doc.Cpl_CodigoTransportador = Convert.ToInt64(txtCodTransportador.Text);

            if (txtCodRemetente.Text != "")
                doc.Cpl_CodigoRemetente = Convert.ToInt64(txtCodRemetente.Text);

            if (txtCodDestinatario.Text != "")
                doc.Cpl_CodigoDestinatario = Convert.ToInt64(txtCodDestinatario.Text);

            if (txtCodTomador.Text != "")
                doc.Cpl_CodigoTomador = Convert.ToInt64(txtCodTomador.Text);

            if (txtCodRecebedor.Text != "")
                doc.Cpl_CodigoRecebedor = Convert.ToInt64(txtCodRecebedor.Text);

            Session["Doc_CTe"] = doc;

        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["LST_CTE"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }

        public CTeX.spdCTeX _spdCTeX = new CTeX.spdCTeX();  

        public CTeDataSetX.spdCTeDataSetX _spdCTeDataSetX = new CTeDataSetX.spdCTeDataSetX();

        protected string BuscarValorTagXML(string XML, string NomeTagPai, string NomeTagFilho)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XML);
            XmlNodeList roll = xmlDoc.GetElementsByTagName(NomeTagPai);

            foreach (XmlNode xn in roll)
            {
                return xn[NomeTagFilho].InnerText;
            }
            return "";
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "Novo")
            {
                EventoEletronicoDocumentoDAL eveDAL = new EventoEletronicoDocumentoDAL();
                List<EventoEletronicoDocumento> ListaEventos = new List<EventoEletronicoDocumento>();
                ListaEventos = eveDAL.ObterEventosEletronicos(Convert.ToDecimal(txtCodigo.Text));
                foreach (var item in ListaEventos)
                {
                    if (item.CodigoSituacao == 119)
                        btnRefresh_Click(sender, e);
                }


                int i = 0;
                foreach (var item in ListaEventoDocEletronico)
                {
                    if (item.CodigoSituacao == 119)
                        i++;
                }
                if (i > 0)
                {
                    ShowMessage("Já existe um evento sendo enviado! Aguarde...", MessageType.Info);
                    btnRefresh_Click(sender, e);
                    return;
                }
            }
            
            SalvarDocumento(sender, e, false);
        }

        protected void SalvarDocumento(object sender, EventArgs e, bool VoltarParaConsulta)
        {
            if(txtCodigo.Text != "Novo" && ListaEventoDocEletronico.Count > 0)
            { 
                IEnumerable<EventoEletronicoDocumento> EventosNaoInviados = ListaEventoDocEletronico.Where((EventoEletronicoDocumento c) => { return c.CodigoSituacao != 121 && c.CodigoSituacao != 119; });
                if (EventosNaoInviados.Count() == 0)
                {
                    ShowMessage("Adicione um novo evento eletrônico para efetuar o desacordo!", MessageType.Info);
                    PanelSelect = "consulta";
                    return;
                }

                List<IntegraDocumentoEletronico> ListaIntegracaoDocEletronico = new List<IntegraDocumentoEletronico>();
                List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();

                DBTabelaCampos rowp3 = new DBTabelaCampos();
                rowp3.Filtro = "CD_DOCUMENTO";
                rowp3.Inicio = txtCodigo.Text;
                rowp3.Fim = txtCodigo.Text;
                rowp3.Tipo = "NUMERIC";
                listaT.Add(rowp3);

                IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
                ListaIntegracaoDocEletronico = integraDAL.ListarIntegracaoDocEletronicoCompleto(listaT);
                int Contador = 0;
                foreach (IntegraDocumentoEletronico integracao in ListaIntegracaoDocEletronico)
                {
                    if (integracao.CodigoAcao == 124 && integracao.IntegracaoProcessando == 0 && integracao.IntegracaoRecebido == 0
                        && integracao.IntegracaoRetorno == 0 && integracao.RegistroDevolvido == 0 && integracao.RegistroEnviado == 1 && integracao.Mensagem == "")
                    {
                        Contador++;
                    }
                    else if (integracao.CodigoAcao == 124 && integracao.Mensagem == "")
                    {
                        Contador++;
                    }
                }
                if (Contador != 0)
                {
                    ShowMessage("Já existe um evento sendo enviado! Aguarde...", MessageType.Info);
                    return;
                }

                EventoEletronicoDocumentoDAL eveDAL = new EventoEletronicoDocumentoDAL();
                List<EventoEletronicoDocumento> ListaEventos = new List<EventoEletronicoDocumento>();
                ListaEventos = eveDAL.ObterEventosEletronicos(Convert.ToDecimal(txtCodigo.Text));
                int i = 0;
                foreach (var item in ListaEventos)
                {
                    if (i == 0)
                        if (item.CodigoSituacao != 121 && item.CodigoTipoEvento != 119)
                        {
                            EventoEletronicoDocumento eve = new EventoEletronicoDocumento();
                            eve = item;
                            eve.CodigoSituacao = 119;
                            eve.Retorno = "";
                            eveDAL.AtualizarEventoEletronico(eve);
                            btnRefresh_Click(sender, e);
                            i++;
                        }
                }
                if (i == 0 && ListaEventos.Count > 0)
                {
                    ShowMessage("Evento sendo enviado", MessageType.Info);
                    return;
                }
            }
            if (ValidaCampos() == false)
                return;

            Doc_CTe p = new Doc_CTe();
            Doc_CTeDAL pe = new Doc_CTeDAL();

            p.DataHoraEmissao = Convert.ToDateTime(txtdtemissao.Text);
            p.DataHoraLancamento = Convert.ToDateTime(txtdtLancamento.Text);
            p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            p.CodigoTipoOperacao = Convert.ToInt32(ddlTipoOperacao.SelectedValue);
            p.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            p.NumeroDocumento = Convert.ToDecimal(txtNroDocumento.Text);
            p.DGSRDocumento = txtNroSerie.Text;
            p.ObservacaoDocumento = txtOBS.Text;
            p.ChaveAcesso = txtChaveAcesso.Text;
            p.ValorTotal = Convert.ToDecimal(txtValor.Text);
            p.Cpl_CodigoTransportador = Convert.ToInt32(txtCodTransportador.Text);
            p.Cpl_CodigoRemetente = Convert.ToInt32(txtCodRemetente.Text);
            p.Cpl_CodigoDestinatario = Convert.ToInt32(txtCodDestinatario.Text);
            p.Cpl_CodigoTomador = Convert.ToInt32(txtCodTomador.Text);
            p.Cpl_CodigoRecebedor = Convert.ToInt32(txtCodRecebedor.Text);
            
            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            p.Cpl_Maquina = Convert.ToInt32(he.CodigoEstacao);
            p.Cpl_Usuario = Convert.ToInt32(Session["CodPflUsuario"]);

            if (txtCodigo.Text == "Novo")
            {
                ddlEmpresa_TextChanged(sender, e);
                p.CodigoGeracaoSequencialDocumento = Convert.ToInt64(Session["CodigoGeradorSequencial"]);
                p.Cpl_NomeTabela = Session["NomeTabela"].ToString();

                Session["CodigoGeradorSequencial"] = null;
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
                pe.Inserir(p,ListaAnexo,ListaEventoDocEletronico);
            }
            else
            { 
                Doc_CTe p2 = new Doc_CTe();
                p.CodigoDocumento = Convert.ToInt64(txtCodigo.Text);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
                p2 = pe.PesquisarDocumento(Convert.ToDecimal(txtCodigo.Text));
                if (Convert.ToInt32(ddlSituacao.SelectedValue) != p2.CodigoSituacao)
                    pe.Atualizar(p, ListaAnexo,ListaEventoDocEletronico);
                else
                    pe.Atualizar(p, ListaAnexo, ListaEventoDocEletronico);
            }

            Session["NomeTabela"] = null;

            if(!VoltarParaConsulta)
                btnVoltar_Click(sender, e);
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["TabFocada"] = "home";
            Response.Redirect("~/Pages/Transporte/ConCTe.aspx");
        }

        protected void ddlEmpresa_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "Novo")
                return;

            DBTabelaDAL db = new DBTabelaDAL();
            List<GeracaoSequencialDocumento> ListaGerDoc = new List<GeracaoSequencialDocumento>();
            GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
            GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
            ListaGerDoc = gerDocDAL.ListarGeracaoSequencial("CD_TIPO_DOCUMENTO", "INT", "7", "");

            // Se existe a tabela sequencial
            if (ListaGerDoc.Count == 0)
            {
                Session["MensagemTela"] = "Gerador Sequencial não iniciado";
                btnVoltar_Click(sender, e);
            }
            foreach (GeracaoSequencialDocumento ger in ListaGerDoc)
            {
                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
                if (ger.Nome == "" || ger.CodigoSituacao == 2)
                {
                    Session["MensagemTela"] = "Gerador Sequencial não iniciado";
                    btnVoltar_Click(sender, e);
                }
                else
                {
                    if (ger.Validade < DateTime.Now)
                    {
                        Session["MensagemTela"] = "Gerador Sequencial venceu em " + ger.Validade.ToString("dd/MM/yyyy");
                        btnVoltar_Click(sender, e);
                    }
                }
                Session["NomeTabela"] = ger.Nome;
                Session["CodigoGeradorSequencial"] = ger.CodigoGeracaoSequencial;

                double NroSequencial = geradorDAL.ExibeProximoNroSequencial(ger.Nome);
                if (NroSequencial == 0)
                    txtNroDocumento.Text = ger.NumeroInicial.ToString();
                else
                    txtNroDocumento.Text = NroSequencial.ToString();

                txtNroSerie.Text = ger.SerieConteudo.ToString();

            }
        }

        protected void txtCodTransportador_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodTransportador.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Transportador", txtCodTransportador.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodTransportador.Text = "";
                    return;
                }
            }

            Int64 CodigoPessoa = Convert.ToInt64(txtCodTransportador.Text);
            PessoaDAL Pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();
            p2 = Pessoa.PesquisarTransportador(CodigoPessoa);

            if (p2 == null)
            {
                ShowMessage("Transportador não existente!", MessageType.Info);
                txtCodTransportador.Text = "";
                txtTransportador.Text = "";
                txtCodTransportador.Focus();
                return;
            }

            txtTransportador.Text = p2.NomePessoa;
            Session["TabFocada"] = null;
        }

        protected void btnTransportador_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?cad=12");
        }

        protected void txtCodRemetente_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodRemetente.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Remetente", txtCodRemetente.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodRemetente.Text = "";
                    return;
                }
            }

            Int64 CodigoPessoa = Convert.ToInt64(txtCodRemetente.Text);
            PessoaDAL Pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();
            p2 = Pessoa.PesquisarPessoa(CodigoPessoa);

            if (p2 == null)
            {
                ShowMessage("Remetente não existente!", MessageType.Info);
                txtCodRemetente.Text = "";
                txtRemetente.Text = "";
                txtCodRemetente.Focus();
                return;
            }

            txtRemetente.Text = p2.NomePessoa;
            Session["TabFocada"] = null;
        }

        protected void btnRemetente_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?cad=13");
        }

        protected void txtCodDestinatario_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodDestinatario.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Destinatário", txtCodDestinatario.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodDestinatario.Text = "";
                    return;
                }
            }

            Int64 CodigoPessoa = Convert.ToInt64(txtCodDestinatario.Text);
            PessoaDAL Pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();
            p2 = Pessoa.PesquisarPessoa(CodigoPessoa);

            if (p2 == null)
            {
                ShowMessage("Destinatário não existente!", MessageType.Info);
                txtCodDestinatario.Text = "";
                txtDestinatario.Text = "";
                txtCodDestinatario.Focus();
                return;
            }

            txtDestinatario.Text = p2.NomePessoa;
            Session["TabFocada"] = null;
        }

        protected void btnDestinatario_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?cad=14");
        }

        protected void txtCodTomador_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodTomador.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Tomador", txtCodTomador.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodDestinatario.Text = "";
                    return;
                }
            }

            Int64 CodigoPessoa = Convert.ToInt64(txtCodTomador.Text);
            PessoaDAL Pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();
            p2 = Pessoa.PesquisarPessoa(CodigoPessoa);

            if (p2 == null)
            {
                ShowMessage("Tomador não existente!", MessageType.Info);
                txtCodTomador.Text = "";
                txtTomador.Text = "";
                txtCodTomador.Focus();
                return;
            }

            txtTomador.Text = p2.NomePessoa;
            Session["TabFocada"] = null;
        }

        protected void btnTomador_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?cad=15");
        }

        protected void txtCodRecebedor_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodRecebedor.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Recebedor", txtCodRecebedor.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodRecebedor.Text = "";
                    return;
                }
            }

            Int64 CodigoPessoa = Convert.ToInt64(txtCodRecebedor.Text);
            PessoaDAL Pessoa = new PessoaDAL();
            Pessoa p2 = new Pessoa();
            p2 = Pessoa.PesquisarPessoa(CodigoPessoa);

            if (p2 == null)
            {
                ShowMessage("Recebedor não existente!", MessageType.Info);
                txtCodRecebedor.Text = "";
                txtRecebedor.Text = "";
                txtCodRecebedor.Focus();
                return;
            }

            txtRecebedor.Text = p2.NomePessoa;
            Session["TabFocada"] = null;
        }

        protected void btnRecebedor_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?cad=16");
        }

        protected void txtValor_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtValor.Text.Equals(""))
            {
                txtValor.Text = "0,00";
            }
            else
            {
                v.CampoValido("Valor Total", txtValor.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);

                if (blnCampo)
                {
                    txtValor.Text = Convert.ToDecimal(txtValor.Text).ToString("###,##0.00");
                    txtValor.Focus();

                }
                else
                    txtValor.Text = "0,00";

            }
        }

        protected void grdAnexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["ZoomDocCtaAnexo"] = HttpUtility.HtmlDecode(grdAnexo.SelectedRow.Cells[0].Text);
            Response.Redirect("~/Pages/financeiros/ManAnexo.aspx?cad=7");
        }

        protected void btnNovoAnexo_Click(object sender, EventArgs e)
        {
            if (!ValidaCampos() || txtCodigo.Text == "Novo")
                return;
            CompactaDocumento();
            Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=7");
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            Doc_CTeDAL doc = new Doc_CTeDAL();
            doc.Excluir(Convert.ToInt32(txtCodigo.Text));


            Session["MensagemTela"] = "Documento Excluído com sucesso!";
            btnVoltar_Click(sender, e);
        }

        protected void btnAddEvento_Click(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
            int i = 0;
            foreach (var item in ListaEventoDocEletronico)
            {
                if (item.CodigoSituacao == 119)
                    i++;
            }
            if (i > 0)
            {
                ShowMessage("Já existe um evento sendo enviado! Aguarde...", MessageType.Info);
                btnRefresh_Click(sender, e);
                return;
            }

            if (!ValidaCampos())
                return;
            Session["ZoomEvtDocEletronico"] = null;
            CompactaDocumento();
            Response.Redirect("~/Pages/Transporte/ManEvtDocEletronico.aspx?cad=1");
        }

        protected void grdEventoEletronico_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
            int i = 0;
            foreach (var item in ListaEventoDocEletronico)
            {
                if (item.CodigoSituacao == 119)
                    i++;
            }
            if (i > 0)
            {
                ShowMessage("Já existe um evento sendo enviado! Aguarde...", MessageType.Info);
                btnRefresh_Click(sender, e);
                return;
            }
            if (!ValidaCampos())
                return;
            CompactaDocumento();
            Session["ZoomEvtDocEletronico"] = HttpUtility.HtmlDecode(grdEventoEletronico.SelectedRow.Cells[0].Text);
            Response.Redirect("~/Pages/Transporte/ManEvtDocEletronico.aspx?cad=1");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!ValidaCampos())
                return;
            Doc_CTe doc = new Doc_CTe();
            Doc_CTeDAL docDAL = new Doc_CTeDAL();
            doc = docDAL.PesquisarDocumento(Convert.ToDecimal(txtCodigo.Text));

            txtdtemissao.Text = doc.DataHoraEmissao.ToString();
            txtdtLancamento.Text = doc.DataHoraLancamento.ToString();
            ddlSituacao.SelectedValue = doc.CodigoSituacao.ToString();
            ddlTipoOperacao.SelectedValue = doc.CodigoTipoOperacao.ToString();
            ddlEmpresa.SelectedValue = doc.CodigoEmpresa.ToString();
            txtNroDocumento.Text = doc.NumeroDocumento.ToString();
            txtNroSerie.Text = doc.DGSRDocumento.ToString();
            txtChaveAcesso.Text = doc.ChaveAcesso;

            txtCodTransportador.Text = doc.Cpl_CodigoTransportador.ToString();
            txtCodTransportador_TextChanged(sender, e);

            txtCodRemetente.Text = doc.Cpl_CodigoRemetente.ToString();
            txtCodRemetente_TextChanged(sender, e);

            txtCodDestinatario.Text = doc.Cpl_CodigoDestinatario.ToString();
            txtCodDestinatario_TextChanged(sender, e);

            txtCodTomador.Text = doc.Cpl_CodigoTomador.ToString();
            txtCodTomador_TextChanged(sender, e);

            txtCodRecebedor.Text = doc.Cpl_CodigoRecebedor.ToString();
            txtCodRecebedor_TextChanged(sender, e);

            txtValor.Text = doc.ValorTotal.ToString();
            txtOBS.Text = doc.ObservacaoDocumento;

            AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
            ListaAnexo = anexo.ObterAnexos(Convert.ToInt32(txtCodigo.Text));
            Session["NovoAnexo"] = ListaAnexo;
            grdAnexo.DataSource = ListaAnexo;
            grdAnexo.DataBind();

            EventoEletronicoDocumentoDAL eventoDAL = new EventoEletronicoDocumentoDAL();
            ListaEventoDocEletronico = eventoDAL.ObterEventosEletronicos(Convert.ToInt32(txtCodigo.Text));
            Session["ListaEventoDocEletronico"] = ListaEventoDocEletronico;
            grdEventoEletronico.DataSource = ListaEventoDocEletronico;
            grdEventoEletronico.DataBind();

            IEnumerable<EventoEletronicoDocumento> EventosNaoInviados = ListaEventoDocEletronico.Where((EventoEletronicoDocumento c) => { return c.CodigoSituacao != 121; });
            if (EventosNaoInviados.Count() == 0)
                BtnEnviarDesacordo.Visible = false;
            else
                BtnEnviarDesacordo.Visible = true;
        }

        protected void btnSimSalvar_Click(object sender, EventArgs e)
        {
            IEnumerable<EventoEletronicoDocumento> EventosNaoInviados = ListaEventoDocEletronico.Where((EventoEletronicoDocumento c) => { return c.CodigoSituacao != 121 && c.CodigoSituacao != 119; });
            if (EventosNaoInviados.Count() == 0)
            {
                ShowMessage("Adicione um novo evento eletrônico para efetuar o desacordo!", MessageType.Info);
                PanelSelect = "consulta";
                return;
            }

            List<IntegraDocumentoEletronico> ListaIntegracaoDocEletronico = new List<IntegraDocumentoEletronico>();
            List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();

            DBTabelaCampos rowp3 = new DBTabelaCampos();
            rowp3.Filtro = "CD_DOCUMENTO";
            rowp3.Inicio = txtCodigo.Text;
            rowp3.Fim = txtCodigo.Text;
            rowp3.Tipo = "NUMERIC";
            listaT.Add(rowp3);

            IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
            ListaIntegracaoDocEletronico = integraDAL.ListarIntegracaoDocEletronicoCompleto(listaT);
            int Contador = 0;
            foreach (IntegraDocumentoEletronico integracao in ListaIntegracaoDocEletronico)
            {
                if (integracao.CodigoAcao == 124 && integracao.IntegracaoProcessando == 0 && integracao.IntegracaoRecebido == 0 
                    && integracao.IntegracaoRetorno == 0 && integracao.RegistroDevolvido == 0 && integracao.RegistroEnviado == 1 && integracao.Mensagem == "")
                {
                    Contador++;
                }
                else if (integracao.CodigoAcao == 124  && integracao.Mensagem == "")
                {
                    Contador++;
                }
            }
            if(Contador != 0)
            {
                ShowMessage("Já existe um evento sendo enviado! Aguarde...", MessageType.Info);
                return;
            }

            EventoEletronicoDocumentoDAL eveDAL = new EventoEletronicoDocumentoDAL();
            List<EventoEletronicoDocumento> ListaEventos = new List<EventoEletronicoDocumento>();
            ListaEventos = eveDAL.ObterEventosEletronicos(Convert.ToDecimal(txtCodigo.Text));
            int i = 0;
            foreach (var item in ListaEventos)
            {
                if (i == 0)
                    if (item.CodigoSituacao != 121 && item.CodigoTipoEvento != 119)
                    {
                        EventoEletronicoDocumento eve = new EventoEletronicoDocumento();
                        eve = item;
                        eve.CodigoSituacao = 119;
                        eve.Retorno = "";
                        eveDAL.AtualizarEventoEletronico(eve);
                        btnRefresh_Click(sender, e);
                        i++;
                    }
            }
            if (i == 0)
            {
                ShowMessage("Evento sendo enviado", MessageType.Info);
                return;
            }


            SalvarDocumento(sender, e, true);
            ShowMessage("Documento salvo e desacordo enviado com sucesso! Aguarde o retorno...", MessageType.Info);
            PanelSelect = "home";

            IntegraDocumentoEletronico InDocEle = new IntegraDocumentoEletronico();
            IntegraDocumentoEletronicoDAL InDocEleDAL = new IntegraDocumentoEletronicoDAL();

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            InDocEle.CodigoDocumento = Convert.ToDecimal(txtCodigo.Text);
            InDocEle.RegistroEnviado = 1;
            InDocEle.IntegracaoRecebido = 0;
            InDocEle.IntegracaoProcessando = 0;
            InDocEle.IntegracaoRetorno = 0;
            InDocEle.RegistroDevolvido = 0;
            InDocEle.RegistroMensagem = 0;
            InDocEle.Mensagem = "";
            InDocEle.CodigoAcao = 124;
            InDocEle.CodigoMaquina = Convert.ToInt32(he.CodigoEstacao);
            InDocEle.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);

            InDocEleDAL.Inserir(InDocEle);

        }
    }
}