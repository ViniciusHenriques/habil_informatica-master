using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.CEPs
{
    public partial class CadMunicipio : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        protected void ApresentaMensagem(String strMensagem)
        {http://localhost:59900/Pages/CEPs/CadMunicipio.aspx.cs
            lblMensagem.Text = strMensagem;
            pnlMensagem.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        protected Boolean ValidaEstado()
        {

            Boolean blnCampoValido = false;
            v.CampoValido("Código do Estado", txtCodEstado.Text, true, true, true, false,"", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDcrEstado.Text = "";
                if (strMensagemR != "")
                    ApresentaMensagem(strMensagemR);
                return false;
            }
            EstadoDAL d = new EstadoDAL();
            Estado p = new Estado();
            p = d.PesquisarEstado(Convert.ToInt32(txtCodEstado.Text));
            txtDcrEstado.Text = "";

            if (p != null)
            {
                txtDcrEstado.Text = p.DescricaoEstado;
                return true;
            }
            else
            {
                ApresentaMensagem("Estado não Cadastrado.");
                txtDcrEstado.Text = "";
                return false;
            }
        }
        protected Boolean ValidaMunicipio()
        {
            if (txtCodigo.Enabled)
            {
                Boolean blnCampoValido = false;
                v.CampoValido("Código do Município", txtCodigo.Text, true, true, true, false,"", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                        ApresentaMensagem(strMensagemR);
                    return false;
                }
                MunicipioDAL d = new MunicipioDAL();
                Municipio p = new Municipio();
                p = d.PesquisarMunicipio(Convert.ToInt64(txtCodigo.Text));

                if (p == null)
                {
                    return true;
                }
                else
                {
                    ApresentaMensagem("Código do Município já Cadastrado.");
                    return false;
                }
            }
            else
                return true;
        }
        protected Boolean ValidaDescricao()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Descrição do Município", txtDescricao.Text,  true, false, false, false,"", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                    ApresentaMensagem(strMensagemR);
                return false;
            }
            return true;
        }
        protected Boolean ValidaCampos()
        {
            if (!ValidaMunicipio()) return false;
            if (!ValidaEstado()) return false;
            if (!ValidaDescricao()) return false;
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["ZoomMunicipio2"] != null)
            {
                if (Session["ZoomMunicipio2"].ToString() == "RELACIONAL")
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
                cmdSair.Visible = true;
            }


            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomEstado"] != null)
            {
                string s = Session["ZoomEstado"].ToString();
                Session["ZoomEstado"] = null;

                string[] words = s.Split('²');
                if (s != "³")
                {
                    txtCodEstado.Text = "";
                    txtDcrEstado.Text = "";
                    foreach (string word in words)
                    {
                        if (txtCodEstado.Text == "")
                            txtCodEstado.Text = word;
                        else
                        {
                            if (txtDcrEstado.Text == "")
                                txtDcrEstado.Text = word;
                        }
                    }
                    txtDescricao.Focus();
                    return;
                }
            }
            else
            {
                txtCodigo.Focus();
            }

            try
            {
                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConMunicipio.aspx");

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                   if (Session["ZoomMunicipio2"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;

                   if (Session["ZoomMunicipio"] != null)
                    {
                        string s = Session["ZoomMunicipio"].ToString();
                        Session["ZoomMunicipio"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            btnExcluir.Visible = true;
                            foreach (string word in words)
                                if (txtCodigo.Text == "")
                                {
                                    txtCodigo.Text = word;
                                    txtCodigo.Enabled = false;

                                    MunicipioDAL r = new MunicipioDAL();
                                    Municipio p = new Municipio();

                                    p = r.PesquisarMunicipio(Convert.ToInt64(txtCodigo.Text));

                                    txtDescricao.Text = p.DescricaoMunicipio;

                                    txtCodEstado.Text = p.CodigoEstado.ToString();

                                    EstadoDAL r2 = new EstadoDAL();
                                    Estado p2 = new Estado();

                                    p2 = r2.PesquisarEstado(Convert.ToInt32(txtCodEstado.Text));

                                    txtDcrEstado.Text = p2.Sigla + " - " + p2.DescricaoEstado;


                                    lista.ForEach(delegate(Permissao x)
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
                        btnExcluir.Visible = false;
                        lista.ForEach(delegate(Permissao p)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoIncluir) 
                                    btnSalvar.Visible = false;
                            }
                        });

                    }
                }



            }
            catch (Exception )
            {
            }
            
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    MunicipioDAL d = new MunicipioDAL();
                    d.Excluir(Convert.ToInt64(txtCodigo.Text));
                    Session["MensagemTela"] = "Município excluído com Sucesso.";
                    btnVoltar_Click(sender, e);
                }

            }
            catch (Exception ex)
            {
                btnCfmNao_Click(sender, e);
                ApresentaMensagem(ex.Message.ToString());
            }
        }
        protected void btnCfmNao_Click(object sender, EventArgs e)
        {
            pnlExcluir.Visible = false;
            pnlMensagem.Visible = false;
        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text.Trim() != "")
                pnlExcluir.Visible = true; 
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomMunicipio"] = null;
            if (Session["ZoomMunicipio2"] != null)
            {
                Session["ZoomMunicipio2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadMunicipio.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return; 
            }

            Response.Redirect("~/Pages/CEPs/ConMunicipio.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos() == false)
                    return;


                MunicipioDAL d = new MunicipioDAL();
                Municipio p = new Municipio();

                p.CodigoMunicipio = Convert.ToInt32(txtCodigo.Text);
                p.CodigoEstado = Convert.ToInt32(txtCodEstado.Text);

                p.DescricaoMunicipio = txtDescricao.Text.ToUpper();

                if (txtCodigo.Enabled)
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Município incluído com Sucesso.";
                }
                else
                {
                    d.Atualizar(p);
                    Session["MensagemTela"] = "Município alterado com Sucesso.";
                }

                btnVoltar_Click(sender, e);

            }
            catch (Exception ex)
            {
                btnCfmNao_Click(sender, e);
                ApresentaMensagem(ex.Message.ToString());
            }

        }
        protected void btnMensagem_Click(object sender, EventArgs e)
        {
            if (Session["FocoTela"] == null)
            {
                Session["FocoTela"] = 1;

            }
            //Botão do OK da Mensagem
            if ((Convert.ToInt32(Session["FocoTela"]) <= 0) || (Convert.ToInt32(Session["FocoTela"])) >= 3)
            {
                Session["FocoTela"] = 1;
                txtCodigo.Focus();
            }
            else
            {
                if (Convert.ToInt32(Session["FocoTela"]) == 1) 
                    txtCodEstado.Focus();
                if (Convert.ToInt32(Session["FocoTela"]) == 2)
                    txtDescricao.Focus();

                Session["FocoTela"] = Convert.ToInt32(Session["FocoTela"]) + 1;
            }



        }
        protected void btnCfmEstado_Click(object sender, EventArgs e)
        {
        }
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
           ValidaMunicipio();
        }
        protected void txtCodEstado_TextChanged(object sender, EventArgs e)
        {
            ValidaEstado();
        }

    }
}