using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;
using NFSeX;

namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class ManItemDocumento : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }

        List<ItemDocumento> ListaItemDocumento = new List<ItemDocumento>();
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";

        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void LimpaTela()
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            
            txtCodItem.Text = "";
            txtDtFim.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"); 
            txtDtInicio.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"); 
            txtCodUsu.Text = Session["CodUsuario"].ToString();
        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

           
            v.CampoValido("Data/Hora de Inicio", txtDtInicio.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDtInicio.Focus();
                }
                return false;
            }
            v.CampoValido("Data/Hora de Fim", txtDtFim.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDtFim.Focus();
                }
                return false;
            }
            v.CampoValido("Descrição da Solicitação", litDescricao.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);

                }

                return false;
            }
            v.CampoValido("Código Usuario responsável", txtCodUsu.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            Int64 numero;
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodUsu.Focus();
                }
                return false;
            }

            if (!Int64.TryParse(txtCodUsu.Text, out numero))
            {
                ShowMessage("Codigo do Usuário responsavel incorreto", MessageType.Info);
                return false;
            }
            DateTime DataHoraInicio = Convert.ToDateTime(txtDtInicio.Text);
            DateTime DataHoraFim = Convert.ToDateTime(txtDtFim.Text);
            if(!(DataHoraInicio.Day == DataHoraFim.Day && DataHoraInicio.Month == DataHoraFim.Month && DataHoraInicio.Year == DataHoraFim.Year && DataHoraInicio.TimeOfDay < DataHoraFim.TimeOfDay))
            {
                ShowMessage("Datas de Inicio/Fim devem ser iguais e Hora de fim maior que Hora de início", MessageType.Info);
                return false;
            }
            return true;
        }

        protected void MontaTela(long CodRegra)
        {

            LimpaTela();

        }

        protected void CarregaTiposSituacoes()
        {

            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.SituacaoItemOrdemServico();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;
            if (Session["ListaItemDocumento"] != null)
            {
                ListaItemDocumento = (List<ItemDocumento>)Session["ListaItemDocumento"];
            }
            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }
            if (Session["TabFocada"] != null)
            {
                PanelSelect = Session["TabFocada"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomItemDocumento2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomItemDocumento"] != null)
                {
                    string s = Session["ZoomItemDocumento"].ToString();
                    Session["ZoomItemDocumento"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                txtCodItem.Text = word;
                                foreach (ItemDocumento p in ListaItemDocumento)
                                {
                                    if (txtCodItem.Text == p.CodigoItem.ToString())
                                    {
                                        CarregaTiposSituacoes();
                                        txtDtFim.Text = p.DataHoraFim.ToString();
                                        txtDtInicio.Text = p.DataHoraInicio.ToString();
                                        ddlSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                        litDescricao.Text = p.DescricaoItem;
                                        txtCodUsu.Text = p.CodigoUsuarioAtendente.ToString();
                                        txtCodUsu_TextChanged(sender, e);

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    CarregaTiposSituacoes();
                    LimpaTela();
                    txtCodUsu_TextChanged(sender, e);
                    txtCodItem.Text = "Novo";

                }            
            }
            if(Session["ItemDocumento"] != null)
            {
                CarregaTiposSituacoes();
                PreencheDados();
                if (Session["ItemDocumentoUsuario"] != null)
                {
                    txtCodUsu.Text = Session["ItemDocumentoUsuario"].ToString();
                    Session["ItemDocumentoUsuario"] = null;
                }
                txtCodUsu_TextChanged(sender, e);
            }
            if(Session["Doc_OrdemServico"] == null)
            {
                Response.Redirect("~/Pages/Servicos/ConOrdServico.aspx");
            }
            Doc_OrdemServico doc = (Doc_OrdemServico)Session["Doc_OrdemServico"];
            if(doc.CodigoClassificacao == 98)
            {
                btnExcluir.Visible = false;
                btnSalvar.Visible = false;
            }
            if (txtCodItem.Text == "")
                btnVoltar_Click(sender, e);
            
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["TabFocada"] = "consulta6";
            Response.Redirect("~/Pages/Servicos/ManOrdServico.aspx");

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;



            int intCttItem = 0;
            if (txtCodItem.Text != "Novo")
                intCttItem = Convert.ToInt32(txtCodItem.Text);
            else
            {
                if (ListaItemDocumento.Count != 0)
                    intCttItem = Convert.ToInt32(ListaItemDocumento.Max(x => x.CodigoItem).ToString());


                intCttItem = intCttItem + 1;
            }
            if (intCttItem != 0)
                ListaItemDocumento.RemoveAll(x => x.CodigoItem == intCttItem);
            ItemDocumento item = new ItemDocumento(intCttItem,0,                                                    
                                                   Convert.ToInt32(txtCodUsu.Text),
                                                   Convert.ToDateTime(txtDtInicio.Text),
                                                   Convert.ToDateTime(txtDtFim.Text),
                                                   Convert.ToInt32(ddlSituacao.SelectedValue),
                                                   litDescricao.Text,
                                                   txtUsu.Text,
                                                   ddlSituacao.SelectedItem.Text);

            ListaItemDocumento.Add(item);
            Session["ListaItemDocumento"] = ListaItemDocumento;

            if (txtCodItem.Text == "Novo")
                Session["MensagemTela"] = "Item do documento criado com sucesso!";
            else
                Session["MensagemTela"] = "Item do documento alterada com sucesso!";
            btnVoltar_Click(sender, e);

        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }
   
        protected void chkSelecionado_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void PreencheDados()
        {
            ItemDocumento doc = (ItemDocumento)Session["ItemDocumento"];

            if (doc.CodigoItem == 0)
            {
                txtCodItem.Text = "Novo";
                btnExcluir.Visible = false;
            }
            else
            {
                txtCodItem.Text = Convert.ToString(doc.CodigoItem);
                btnExcluir.Visible = true;
            }

            if (Convert.ToString(doc.DataHoraInicio) != "01/01/0001 00:00:00")
                txtDtInicio.Text = doc.DataHoraInicio.ToString();
            if (Convert.ToString(doc.DataHoraFim) != "01/01/0001 00:00:00")
                txtDtFim.Text = doc.DataHoraFim.ToString();

            ddlSituacao.SelectedValue = doc.CodigoSituacao.ToString();


            if (doc.CodigoUsuarioAtendente == 0)
                txtCodUsu.Text = "";
            else
                txtCodUsu.Text = Convert.ToString(doc.CodigoUsuarioAtendente);
        


            if (Session["RascunhoDocumentoItem"] != null)
            {
                string str = Server.HtmlDecode(Session["RascunhoDocumentoItem"].ToString());
                litDescricao.Text = str;
                Session["RascunhoDocumentoItem"] = null;
            }
            Session["ItemDocumento"] = null;
            Session["TabFocada"] = "consulta6";
        }

        protected void CompactaDocumento()
        {
            ItemDocumento doc = new ItemDocumento();

            if (txtCodItem.Text == "Novo")
                doc.CodigoItem = 0;
            else
                doc.CodigoItem = Convert.ToInt32(txtCodItem.Text);

            Boolean blnCampoValido = false;
            v.CampoValido("Data/Hora de Inicio", txtDtInicio.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (blnCampoValido)
            {
                doc.DataHoraInicio = Convert.ToDateTime(txtDtInicio.Text);
            }
            v.CampoValido("Data/Hora de Fim", txtDtFim.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (blnCampoValido)
            {
                doc.DataHoraFim = Convert.ToDateTime(txtDtFim.Text);
            }



            doc.CodigoSituacao = Convert.ToInt32(ddlSituacao.Text);

            if (txtCodUsu.Text == "")
                doc.CodigoUsuarioAtendente = 0;
            else
                doc.CodigoUsuarioAtendente = Convert.ToInt32(txtCodUsu.Text);
            //ckeditor
            string str = litDescricao.Text;
            string str2 = Server.HtmlDecode(str);
            Session["RascunhoDocumentoItem"] = str2;
            doc.DescricaoItem = str2;         

            Session["ItemDocumento"] = doc;

        }

        protected void BtnEditarDS_Click(object sender, EventArgs e)
        {
            CompactaDocumento();

            Response.Redirect("~/Pages/Servicos/ManRasDocumento.aspx?cad=3");
        }
    
        protected void txtCodUsu_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCodUsu.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Usuário", txtCodUsu.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodUsu.Text = "";
                    return;
                }
            }
            Int64 numero;
            if (!Int64.TryParse(txtCodUsu.Text, out numero))
            {
                return;
            }

            Int64 codigoUsuario = Convert.ToInt64(txtCodUsu.Text);
            UsuarioDAL Usuario = new UsuarioDAL();
            Usuario p2 = new Usuario();

            p2 = Usuario.PesquisarUsuario(codigoUsuario);

            if (p2 == null)
            {
                ShowMessage("Usuário não existente!", MessageType.Info);
                txtCodUsu.Text = "";
                txtUsu.Text = "";
                txtCodUsu.Focus();

                return;
            }


            txtUsu.Text = p2.NomeUsuario;
            //txtDescricao.Focus();
            Session["TabFocada"] = null;
        }

        protected void btnUsu_Click(object sender, EventArgs e)
        {
            CompactaDocumento();

            Response.Redirect("~/Pages/Usuarios/ConUsuario.aspx?Cad=2");
        }
    }
}