using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Persistence;
using DAL.Model;

namespace GestaoInterna
{
    public partial class Default : System.Web.UI.Page
    {
        clsMensagem cls = new clsMensagem();

        protected void Page_Load(object sender, EventArgs e)
        {
           try
           {

               Session["UsuSis"] = null;
               Session["CodEmpresa"] = null;
               Session["QtasEmpresas"] = null;
               Session["NomeEmpresa"] = null;

               txtUsuario1.Attributes.Add("onKeyPress", "javascript:passaCampo(event, pwdSenha1);");
               pwdSenha1.Attributes.Add("onKeyPress", "javascript:passaCampo(event, btnEntrar);");


               if (!Page.IsPostBack)
               {
                   txtUsuario1.Text = "";
                   pwdSenha1.Text = "";
                   txtUsuario1.Focus();
               }
               
           }
           catch (Exception)
           {
               Response.Redirect("~/SistemaEmManut.aspx");
           }



        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {

            if (txtUsuario1.Text == "")
            {
                lblMensagem.Text = "Usuário deve ser Informado.";
                txtUsuario1.Focus();
                return;
            }
            if (pwdSenha1.Text == "")
            {
                lblMensagem.Text = "Senha deve ser Informado.";
                pwdSenha1.Focus();
                return;
            }

            if ((txtUsuario1.Text == "habil") && (pwdSenha1.Text == "a"))
            {
                Session["UsuSis"] = "ENTRADA INICIAL DO SISTEMA";
                Session["CodUsuario"] = "0";
                Session["CodPflUsuario"] = 1;
                Response.Redirect("~/PagesAdmin/Welcome.aspx");
                return;
            }
            else
            {
                lblMensagem.Text = "Usuário e/ou Senha não autorizados.";
                txtUsuario1.Focus();

            }


        }

        protected void ddlPesquisa_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUsuario1.Focus();
        }

        protected void button1_Click(object sender, EventArgs e)
        {

        }

        protected void btnNada_Click(object sender, EventArgs e)
        {

        }
    }
}