using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.UI;
using DAL.Model;
using DAL.Persistence;
using System.Runtime.Serialization.Formatters.Binary;

namespace SoftHabilInformatica.Pages.Transporte
{
    public partial class ManEvtDocEletronico : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        
        private List<EventoEletronicoDocumento> ListaEventoEletronico = new List<EventoEletronicoDocumento>();

        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaTela()
        {
            txtSequencia.Text = "Novo";
            txtMotivo.Text = "";
            ddlSituacao.SelectedValue = "123";
            DBTabelaDAL RnTab = new DBTabelaDAL();
            txtDtHrEmissao.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm");
        }

        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            if (Session["Doc_CTe"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
            {
                ddlTipoEvento.DataSource = sd.TipoEvento();
                ddlTipoEvento.DataTextField = "DescricaoTipo";
                ddlTipoEvento.DataValueField = "CodigoTipo";
                ddlTipoEvento.DataBind();
                ddlTipoEvento.Items.Insert(0, "..... SELECIONE UM TIPO DE EVENTO.....");
            }
            else
            {
                Response.Redirect("~/Pages/Default.aspx");
            }

            ddlSituacao.DataSource = sd.SituacaoEventoEletronico();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();
            
        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Data de Emissão", txtDtHrEmissao.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDtHrEmissao.Focus();
                }
                return false;
            }
            if (txtChaveAcesso.Text.Length != 44)
            {
                ShowMessage("A chave de acesso deve ter 44 dígitos.", MessageType.Info);
                return false;
            }
            if (ddlTipoEvento.SelectedValue == "..... SELECIONE UM TIPO DE EVENTO.....")
            {
                ShowMessage("Selecione um tipo de evento", MessageType.Info);
                ddlTipoEvento.Focus();
                return false;
            }

            string motivo = txtMotivo.Text.Trim(' ');
            if (motivo.Length <= 15 || motivo.Length > 200)
            {
                ShowMessage("Motivo deve ter mais de 15 caracteres!", MessageType.Info);
                txtMotivo.Focus();
                return false;
            }
            return true;
        }
        protected void MontaTela()
        {
            Doc_CTe doc = (Doc_CTe)Session["Doc_CTe"];
            if(doc == null) {
                Response.Redirect("~/Pages/Default.aspx");
            }
            txtChaveAcesso.Text = doc.ChaveAcesso;

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
            if (Session["ListaEventoDocEletronico"] != null)
                ListaEventoEletronico = (List<EventoEletronicoDocumento>)Session["ListaEventoDocEletronico"];

            PanelSelect = "home";
            List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
            Doc_CtaPagarDAL r = new Doc_CtaPagarDAL();
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                if (Session["ZoomEvtDocEletronico"] != null)
                {                    
                    string s = Session["ZoomEvtDocEletronico"].ToString();

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;

                        foreach (string word in words)
                        {
                            if (txtSequencia.Text == "")
                                txtSequencia.Text = word;
                        }

                        if (txtSequencia.Text != "Novo")
                        {
                            btnExcluir.Visible = true;
                            foreach (EventoEletronicoDocumento p in ListaEventoEletronico)
                            {
                                if (txtSequencia.Text == p.CodigoEvento.ToString())
                                {
                                    CarregaSituacoes();
                                    MontaTela();
                                    ddlTipoEvento.SelectedValue = p.CodigoTipoEvento.ToString();
                                    ddlTipoEvento.Enabled = false;
                                    txtSequencia.Text = p.NumeroSequencia.ToString();
                                    txtMotivo.Text = p.Motivo;
                                    txtDtHrEmissao.Text = p.DataHoraEvento.ToString();
                                    ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                    if(p.CodigoSituacao == 121)
                                    {
                                        btnSalvar.Visible = false;
                                        btnExcluir.Visible = false;

                                    }
                                    //if (p.Retorno != "")
                                    //{
                                    //    btnSalvar.Visible = false;
                                    //    btnExcluir.Visible = false;
                                    //}
                                }
                            }
                        }
                    }
                }
                else
                {
                    CarregaSituacoes();
                    MontaTela();
                    LimpaTela();
                    btnExcluir.Visible = false;              
                }
            }    
            if (txtSequencia.Text == "")
            {
                btnVoltar_Click(sender, e);
            }
            if (ddlTipoEvento.SelectedValue != "..... SELECIONE UM TIPO DE EVENTO.....")
                ddlTipoEvento.Enabled = false;
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            int intInsItem = 0;
            if (txtSequencia.Text != "Novo")
            {
                intInsItem = Convert.ToInt32(txtSequencia.Text);

                if (intInsItem != 0)
                    ListaEventoEletronico.RemoveAll(x => x.CodigoEvento == intInsItem);

                Session["ZoomEvtDocEletronico"] = null;
                Session["EventoDocEletronico"] = ListaEventoEletronico;
                btnExcluir.Visible = false;
                btnSalvar.Visible = true;
                Session["MensagemTela"] = "Evento Eletrônico Excluído com sucesso!";
                btnVoltar_Click(sender,e);
            }
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomEvtDocEletronico"] = null;
            Session["TabFocada"] = "consulta";
            if (Session["Doc_CTe"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");
            else
            {
                Session["ListaEventoDocEletronico"] = null;
                Session["Doc_CTe"] = null;
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");
            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            Habil_Estacao he = new Habil_Estacao();
            Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
            he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

            Usuario usu = new Usuario();
            UsuarioDAL usuDAL = new UsuarioDAL();
            usu = usuDAL.PesquisarUsuario(Convert.ToInt32(Session["CodUsuario"]));

            int intCttItem = 0;
            if (Session["ZoomEvtDocEletronico"] != null)
            {
                string s = Session["ZoomEvtDocEletronico"].ToString();
                Session["ZoomEvtDocEletronico"] = null;

                string[] words = s.Split('³');
                intCttItem = Convert.ToInt32(words[0]);
                
            }
            else
            {
                if (ListaEventoEletronico.Count != 0)
                    intCttItem = Convert.ToInt32(ListaEventoEletronico.Max(x => x.CodigoEvento).ToString());

                intCttItem = intCttItem + 1;
            }
            if (intCttItem != 0)
                ListaEventoEletronico.RemoveAll(x => x.CodigoEvento == intCttItem);

            string strMotivo = txtMotivo.Text.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Trim(' ');
            EventoEletronicoDocumentoDAL eventoDAL = new EventoEletronicoDocumentoDAL();         
            EventoEletronicoDocumento evento = new EventoEletronicoDocumento(intCttItem,
                                                       Convert.ToInt32(ddlSituacao.SelectedValue),
                                                       Convert.ToDateTime(txtDtHrEmissao.Text),
                                                       he.CodigoEstacao,
                                                       Convert.ToInt32(Session["CodUsuario"]),
                                                       Convert.ToInt32(txtSequencia.Text),
                                                       strMotivo, "",
                                                       Convert.ToInt32(ddlTipoEvento.SelectedValue),
                                                       ddlTipoEvento.SelectedItem.Text,
                                                       usu.NomeUsuario,
                                                       he.NomeEstacao,
                                                       ddlSituacao.SelectedItem.Text);

            ListaEventoEletronico.Add(evento);
            Session["ListaEventoDocEletronico"] = ListaEventoEletronico;

            if (txtSequencia.Text == "Novo")
                Session["MensagemTela"] = "Evento Eletrônico do documento feita com sucesso!";
            else
                Session["MensagemTela"] = "Evento Eletrônico do documento alterada com sucesso!";

            Session["TabFocada"] = "consulta2";
            btnVoltar_Click(sender, e);
        }

        protected void ddlTipoEvento_TextChanged(object sender, EventArgs e)
        {
            List<EventoEletronicoDocumento> ListaNovaEvento = new List<EventoEletronicoDocumento>();
            //ListaNovaEvento = ListaEventoEletronico.Where((EventoEletronicoDocumento x) => { return x.CodigoTipoEvento > 2000; })); 
            IEnumerable<EventoEletronicoDocumento> SelectListaEventosTipoEvento = ListaEventoEletronico.Where((EventoEletronicoDocumento c) => { return c.CodigoTipoEvento == Convert.ToInt32(ddlTipoEvento.SelectedValue); });

            txtSequencia.Text = (SelectListaEventosTipoEvento.Count() + 1).ToString();
        }
    }
}