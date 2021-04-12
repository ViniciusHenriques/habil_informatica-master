using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DAL.Persistence;
using DAL.Model;

namespace FrenteCaixa
{
    public partial class Principal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if ((Session["UsuSisCaixa"] == null) || (Session["CodUsuarioCaixa"] == null))
            {
                ///Response.Redirect("http://localhost:49942/Default.aspx");
                Response.Redirect("http://localhost:1478/login.aspx");
                return;
            }

            pnlCxsAbertos.Visible = false;


            lblNomefuncionario.Text = Session["UsuSisCaixa"].ToString();

            lblTitulo.Text = Session["SitCaixa"].ToString() ;
            lblTitulo.Font.Size = 50; 
        }

        protected void btnSuprimento_Click(object sender, EventArgs e)
        {
            if (lblTitulo.Text.EndsWith("Aberto"))
            {
                Session["OP_CAIXA"] = "Suprimento de Caixa";
                Response.Redirect("~/AbeFecSprSan.aspx");
            }
        }

        protected void btnSangria_Click(object sender, EventArgs e)
        {
            if (lblTitulo.Text.EndsWith("Aberto"))            {
                Session["OP_CAIXA"] = "Sangria de Caixa";
                Response.Redirect("~/AbeFecSprSan.aspx");
            }
        }

        protected void btnAbertura_Click(object sender, EventArgs e)
        {
            if (lblTitulo.Text != "Caixa Aberto")
            {
                Session["OP_CAIXA"] = "Abertura de Caixa";
                Response.Redirect("~/AbeFecSprSan.aspx");
            }        
        }

        protected void btnEncerra_Click(object sender, EventArgs e)
        {
            if (lblTitulo.Text.EndsWith("Aberto"))
            {
                Session["OP_CAIXA"] = "Encerramento de Caixa";
                Response.Redirect("~/AbeFecSprSan.aspx");
            }
        }

        protected void btnCxAbertos_Click(object sender, EventArgs e)
        {
            lblTitulo.Visible =false ;
            pnlDash.Visible = false;
            pnlCxsAbertos.Visible = true;

        }

        protected void btnVoltarPrinc_Click(object sender, EventArgs e)
        {
            lblTitulo.Visible = true;
            pnlDash.Visible = true;
            pnlCxsAbertos.Visible = false;
        }
    }
}