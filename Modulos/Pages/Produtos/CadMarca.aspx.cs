using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class CadMarca : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Descrição da Marca", txtDescricao.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDescricao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }

            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtDescricao.Focus();

            if (Session["ZoomMarca2"] != null)
            {
                if (Session["ZoomMarca2"].ToString() == "RELACIONAL")
                {
                    pnlPainel.Visible = false;
                    cmdSair.Visible = false;
                }
                else
                {
                    pnlPainel.Visible = true;
                    cmdSair.Visible = true;
                }
            }
            else
            {
                pnlPainel.Visible = true;
                cmdSair.Visible = false;
            }

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = "";
            }


            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt16(Session["CodModulo"].ToString()),
                                           Convert.ToInt16(Session["CodPflUsuario"].ToString()),
                                           "ConMarca.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomMarca2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomMarca"] != null)
                {
                    string s = Session["ZoomMarca"].ToString();
                    Session["ZoomMarca"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                MarcaDAL r = new MarcaDAL();
                                Marca p = new Marca();

                                p = r.PesquisarMarca(Convert.ToInt16(txtCodigo.Text));

                                txtDescricao.Text = p.DescricaoMarca;

                                lista.ForEach(delegate (Permissao x)
                                {
                                    if (!x.AcessoCompleto)
                                    {
                                        if (!x.AcessoAlterar)
                                            btnSalvar.Visible = false;

                                        if (!x.AcessoExcluir)
                                            btnExcluir.Visible = false;
                                    }
                                });

                                return;
                            }
                    }
                }
                else
                {
                    txtCodigo.Text = "Novo";
                    btnExcluir.Visible = false;
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                        }
                    });

                }
            }

            if (Session["CodUsuario"] == "-150380")
            {
                btnSalvar.Visible = true;
            }

            if (txtCodigo.Text == "")
                btnVoltar_Click(sender, e);

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    MarcaDAL d = new MarcaDAL();
                    d.Excluir(Convert.ToInt16(txtCodigo.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Marca não identificado.&emsp;&emsp;&emsp;";

            }
            catch (Exception ex)
            {
                strErro = "&emsp;&emsp;&emsp;" + ex.Message.ToString() + "&emsp;&emsp;&emsp;";
            }

            if (strErro != "")
            {
                lblMensagem.Text = strErro;
                pnlMensagem.Visible = true;
            }
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomMarca"] = null;
            if (Convert.ToInt16(Request.QueryString["Cad"]) == 1)
            {
                Response.Redirect("~/Pages/Produtos/CadMarca.aspx");

            }
            if (Session["ZoomMarca2"] != null)
            {
                Session["ZoomMarca2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadMarca.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Response.Redirect("~/Pages/Produtos/ConMarca.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            MarcaDAL d = new MarcaDAL();
            Marca p = new Marca();

            p.DescricaoMarca = txtDescricao.Text.ToUpper();

            if (txtCodigo.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p.CodigoMarca = Convert.ToInt16(txtCodigo.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }

            if (Convert.ToInt16(Request.QueryString["cad"]) == 1)
            {
                Session["MensagemTela"] = null;
                Response.Redirect("~/Pages/Produtos/CadMarca.aspx");

            }
            else
                btnVoltar_Click(sender, e);
        }

    }
}