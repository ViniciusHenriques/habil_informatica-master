using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Persistence;
using DAL.Model;

namespace FrenteCaixa
{
    public partial class Login : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            PnlMsg.Visible = false;
            txtUsuario1.Focus();  
        }
        protected void pwdSenha_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btnConfirma_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = false;
            if (txtUsuario1.Text == "")
            {
                PnlMsg.Visible = true; 
                lblMensagem.Text = "Funcionário deve ser Informado.";
                txtUsuario1.Focus();
                return;
            }
            else
            {
                v.CampoValido("Funcionário", txtUsuario1.Text, true, true, true, false, "INT", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        PnlMsg.Visible = true;
                        lblMensagem.Text = strMensagemR;
                        return;
                    }
                }
            }
            if (pwdSenha1.Text == "")
            {
                PnlMsg.Visible = true;
                lblMensagem.Text = "Senha deve ser Informado.";
                pwdSenha1.Focus();
                return;
            }
            
            lblMensagem.Text = "";
            UsuarioDAL m = new UsuarioDAL();
            Usuario p = new Usuario();
            Boolean blnLoginOK = false;
            p = m.PesquisarCodLogin(Convert.ToInt64 (txtUsuario1.Text), pwdSenha1.Text, out blnLoginOK);

            if (blnLoginOK == true)
            {
                Session["UsuSisCaixa"] = p.NomeUsuario;
                Session["CodUsuarioCaixa"] = p.CodigoUsuario;
                Session["CodPflUsuarioCaixa"] = p.CodigoPerfil;

                CaixaDAL CD = new CaixaDAL();
                Caixa C = new Caixa();

                C = CD.Obtem_Caixa_Aberto();

                if (C == null)
                    Session["SitCaixa"] = "Caixa Fechado";
                else
                {
                    Session["SitCaixa"] = "Caixa Nº " + C.CodigoCaixa.ToString() + " Aberto";
                    Session["CodUsuarioCaixa"] = C.CodigoCaixa;
                }



                Response.Redirect("http://localhost:1478/Principal.aspx");
            }
            else
            {
                PnlMsg.Visible = true;
                lblMensagem.Text = "Funcionário e/ou Senha não autorizados.";
                txtUsuario1.Focus();
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://localhost:59942/Selecao.aspx");

        }

        protected void btnFecharMensagem_Click(object sender, EventArgs e)
        {
            PnlMsg.Visible = false; 
        }
    }
}