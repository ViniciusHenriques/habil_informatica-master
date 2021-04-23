using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftHabilInformatica.Pages.Compromissos
{
    public partial class ConAgendamento : System.Web.UI.Page
    {
        public enum MessageType { Success, Error, Info, Warning };
        public string Agendamentos;
        String strMensagemR = "";

        clsValidacao v = new clsValidacao();

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["NovoAnexo"] = null;
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();

            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConAgendamento.aspx");

            lista.ForEach(delegate (Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoIncluir)
                        btnNovo.Visible = false;
                }
            });

            List<AgendamentoCompromisso> Lista = new List<AgendamentoCompromisso>();
            AgendamentoCompromissoDAL agendaDAL = new AgendamentoCompromissoDAL();
            Lista = agendaDAL.ListarAgendamento("USU.CD_USUARIO","INT",Session["CodUsuario"].ToString(),"");

            for (int i = 0; i < Lista.Count; i++)
            {
                if (Lista[i].CodigoSituacao == 206)
                {
                    Agendamentos += "{" +
                                        "title: '" + Lista[i].DataHoraAgendamento.ToString("HH:mm") + " - " + Lista[i].Anotacao.Replace("\n", " ").Replace("\r", " ") + "'," +
                                        "start: '" + Lista[i].DataHoraAgendamento.ToString("yyyy-MM-dd") + "'," +
                                        "color: '" + Lista[i].CorLembrete + "'," +
                                        "className: 'TarefaConcluida'" +
                                    "},";
                }
                else
                {
                    Agendamentos += "{" +
                                        "title: '" + Lista[i].DataHoraAgendamento.ToString("HH:mm") + " - " + Lista[i].Anotacao.Replace("\n", " ").Replace("\r", " ") + "'," +
                                        "start: '" + Lista[i].DataHoraAgendamento.ToString("yyyy-MM-dd") + "'," +
                                        "color: '" + Lista[i].CorLembrete + "'" +
                                    "},";
                }

            }
            Session["ListaUsuariosPermitidos"] = null;
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["ZoomAgendamento"] = null;
            Session["NovoAnexo"] = null;
            Session["ListaUsuariosPermitidos"] = null;
            Response.Redirect("~/Pages/Compromissos/ManAgendamento.aspx");

        }
    }
}