using System;
using DAL.Persistence;
using DAL.Model;

namespace FrenteCaixa
{
    public partial class AbeFecSprSan : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            PnlMsg.Visible = false;
            if ((Session["UsuSisCaixa"] == null) || (Session["CodUsuarioCaixa"] == null))
            {
                Response.Redirect("http://localhost:49942/Default.aspx");
                return;
            }
            lblNomefuncionario.Text = Session["UsuSisCaixa"].ToString();
            lblTitulo.Text = Session["OP_CAIXA"].ToString();

            if ((lblTitulo.Text == "Encerramento de Caixa") || (lblTitulo.Text == "Abertura de Caixa"))
                pnlEscondeFormas.Visible=false; 
            else
                pnlEscondeFormas.Visible=true;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Principal.aspx");
        }

        protected void btnFecharMensagem_Click(object sender, EventArgs e)
        {
            PnlMsg.Visible = false; 
        }

        protected void btnConfirma_Click(object sender, EventArgs e)
        {
            bool blnCampoValido = false;
            /*Caixa*/
            if (txtCodCaixa.Text == "")
            {
                PnlMsg.Visible = true;
                lblMensagem.Text = "Caixa deve ser Informado.";
                txtCodCaixa.Focus();
                return;
            }
            else
            {
                v.CampoValido("Caixa", txtCodCaixa.Text, true, true, true, false, "SMALLINT", ref blnCampoValido, ref strMensagemR);
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

            if (txtCodFuncionario.Text == "")
            {
                PnlMsg.Visible = true;
                lblMensagem.Text = "Funcionário deve ser Informado.";
                txtCodFuncionario.Focus();
                return;
            }
            else
            {
                blnCampoValido = false;
                v.CampoValido("Funcionário", txtCodFuncionario.Text, true, true, true, false, "INT", ref blnCampoValido, ref strMensagemR);
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
            if (txtSenha.Text == "")
            {
                PnlMsg.Visible = true;
                lblMensagem.Text = "Senha deve ser Informado.";
                txtSenha.Focus();
                return;
            }

            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            lblMensagem.Text = "";
            UsuarioDAL m = new UsuarioDAL();
            Usuario p = new Usuario();
            Boolean blnLoginOK = false;
            p = m.PesquisarCodLogin(Convert.ToInt64(txtCodFuncionario.Text), txtSenha.Text, out blnLoginOK);

            if (blnLoginOK != true)
            {
                PnlMsg.Visible = true;
                lblMensagem.Text = "Funcionário não autorizado.";
                txtCodFuncionario.Focus();
                return;
            }
            else
            {
                /*Faz o Abre, Sangra, Supri ou Fecha Caixa */

                CaixaDAL cm = new CaixaDAL();
                Caixa c = new Caixa();

                if (lblTitulo.Text == "Abertura de Caixa")
                {

                    /*Verificar se não existe Caixa Aberto*/
                    if (cm.Existe_Caixa_Aberto(Convert.ToInt32(txtCodCaixa.Text)) )
                    {
                        PnlMsg.Visible = true;
                        lblMensagem.Text = "Caixa já Aberto.";
                        txtSenha.Focus();
                        return;
                    }

                    c.CodigoCaixa = Convert.ToInt32(txtCodCaixa.Text);
                    c.CodigoFunAbertura = Convert.ToInt32(txtCodFuncionario.Text);
                    c.NomeMaquina = Environment.MachineName;
                    cm.Abertura_de_Caixa(c);
                    Session["SitCaixa"] = "Caixa Nº " + c.CodigoCaixa.ToString() + " Aberto";
                    Session["CodUsuarioCaixa"] = c.CodigoCaixa;
                }

                if (lblTitulo.Text == "Encerramento de Caixa")
                {

                    /*Verificar se Caixa está Aberto*/
                    if (!cm.Existe_Caixa_Aberto(Convert.ToInt32(txtCodCaixa.Text)))
                    {
                        PnlMsg.Visible = true;
                        lblMensagem.Text = "Caixa não consta Aberto.";
                        txtSenha.Focus();
                        return;
                    }

                    c.CodigoCaixa = Convert.ToInt32(txtCodCaixa.Text);
                    c.CodigoFunFechamento = Convert.ToInt32(txtCodFuncionario.Text);
                    c.NomeMaquina = Environment.MachineName;
                    cm.Encerramento_de_Caixa(c);
                    Session["SitCaixa"] = "Caixa Fechado";
                    Session["CodUsuarioCaixa"] = 0;
                }
                Response.Redirect("~/Principal.aspx");
            }
        }

        protected void txtCodFuncionario_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampoValido = false;
            lblDscFuncionario.Text="";
            if (txtCodFuncionario.Text != "")
            {
                v.CampoValido("Funcionário", txtCodFuncionario.Text, true, true, true, false, "INT", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        PnlMsg.Visible = true;
                        lblMensagem.Text = strMensagemR;
                        return;
                    }
                }
                else
                {
                    UsuarioDAL m = new UsuarioDAL();
                    Usuario p = new Usuario();
                    p = m.PesquisarUsuario(Convert.ToInt64(txtCodFuncionario.Text));

                    if (p == null)
                    {
                        PnlMsg.Visible = true;
                        lblMensagem.Text = "Funcionário não Cadastrado.";
                        txtCodFuncionario.Focus();
                        return;
                    }
                    else
                    {
                        lblDscFuncionario.Text = p.NomeUsuario.ToString();
                        txtSenha.Focus(); 
                    }

                }
            }
        }

        
    }
}