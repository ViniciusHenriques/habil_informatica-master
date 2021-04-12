using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;

namespace SoftHabilInformatica.Pages.Servicos
{
    public partial class ManRasDocumento : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";

        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;



            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConRasAtendimento.aspx");
            lista.ForEach(delegate (Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoAlterar)
                        btnSalvar.Visible = false;

                }
            });

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                if(Session["RascunhoDocumento"] != null && (Convert.ToInt32(Request.QueryString["Cad"]) == 1 || Convert.ToInt32(Request.QueryString["Cad"]) == 2))
                {
                    string Texto = Convert.ToString(Session["RascunhoDocumento"]);
                    //string str = CKEditor1.Text;
                    string str2 = Server.HtmlDecode(Texto);
                    CKEditor1.Text = str2;
                }
                if (Session["RascunhoDocumentoItem"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 3)
                {
                    string Texto = Convert.ToString(Session["RascunhoDocumentoItem"]);
                    //string str = CKEditor1.Text;
                    string str2 = Server.HtmlDecode(Texto);
                    CKEditor1.Text = str2;
                }
                if (Session["RascunhoEmail"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 4)
                {
                    string Texto = Convert.ToString(Session["RascunhoEmail"]);
                    //string str = CKEditor1.Text;
                    string str2 = Server.HtmlDecode(Texto);
                    CKEditor1.Text = str2;
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
            if(Session["Doc_OrdemServico"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 2)
                Response.Redirect("~/Pages/Servicos/ManOrdServico.aspx");
            else if (Session["Doc_SolicitacaoAtendimento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 1)
                Response.Redirect("~/Pages/Servicos/ManSolAtendimento.aspx");
            else if (Session["ItemDocumento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 3)
                Response.Redirect("~/Pages/Servicos/ManItemDocumento.aspx");
            else if (Session["GeracaoDosEmails"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 4)
            {
                if (Request.QueryString["IN"] != null)
                    Response.Redirect("~/Pages/HabilUtilitarios/ManGerEmails.aspx?cad=" + Request.QueryString["Cad"] + "&IN=" + Request.QueryString["IN"]);
                else
                    Response.Redirect("~/Pages/HabilUtilitarios/ManGerEmails.aspx?cad=" + Request.QueryString["Cad"]);
            }
            else
                Response.Redirect("~/Pages/welcome.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string str = CKEditor1.Text;
            string str2 = Server.HtmlDecode(str);
            if ((Session["Doc_SolicitacaoAtendimento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 1) || (Session["Doc_OrdemServico"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 2))
                Session["RascunhoDocumento"] = str2;
            if (Session["ItemDocumento"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 3)
                Session["RascunhoDocumentoItem"] = str2;
            if (Session["GeracaoDosEmails"] != null && Convert.ToInt32(Request.QueryString["Cad"]) == 4)
                Session["RascunhoEmail"] = str2;
            btnVoltar_Click(sender, e);

        }
    }
}