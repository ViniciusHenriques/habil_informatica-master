using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace GestaoInterna
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["sid"] = Session.SessionID;
            Session["sid"] = "Test";
            Session["UsuSis"] = null;
            Session["CodUsuario"] = null;
            Session["PwdSis"] = null;
            Session["ZoomSituacao"] = null;
            Session["ZoomModSistema"] = null;
            Session["ZoomPflUsuario"] = null;
            Session["Operacao"] = "CONSULTA";
            Session["Pagina"] = "Inicial";
            Session["MenuAberto"] = "NAO";
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}