using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Empresas
{
    public partial class CadTipoDocumento : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";

        protected void ApresentaMensagem(String strMensagem)
        {
            lblMensagem.Text = strMensagem;
            pnlMensagem.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }


        protected void LimpaCampos()
        {
            txtCodigo.Text = "Novo";
            txtDescricao.Text = "";
 
            Habil_TipoDAL sd = new Habil_TipoDAL();
            txtCodSituacao.DataSource = sd.Atividade();
            txtCodSituacao.DataTextField = "DescricaoTipo";
            txtCodSituacao.DataValueField = "CodigoTipo";
            txtCodSituacao.DataBind();

            txtCodSituacao.SelectedValue = "1";


            DdlDigitavel.DataSource = sd.TipoDocumento();
            DdlDigitavel.DataTextField = "DescricaoTipo";
            DdlDigitavel.DataValueField = "CodigoTipo";
            DdlDigitavel.DataBind();

            DdlDigitavel_SelectedIndexChanged(null, null);

            DdlIncEmpresa.DataSource = sd.Existencia();
            DdlIncEmpresa.DataTextField = "DescricaoTipo";
            DdlIncEmpresa.DataValueField = "CodigoTipo";
            DdlIncEmpresa.DataBind();
            

            DdlAbeSerie.DataSource = sd.Existencia();
            DdlAbeSerie.DataTextField = "DescricaoTipo";
            DdlAbeSerie.DataValueField = "CodigoTipo";
            DdlAbeSerie.DataBind();

        }

        protected Boolean ValidaTipoDocumento()
        {
            if (txtCodigo.Enabled)
            {
                Boolean blnCampoValido = false;
                v.CampoValido("Código do Tipo de Documento", txtCodigo.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    txtCodigo.Text = "";
                    if (strMensagemR != "")
                        ApresentaMensagem(strMensagemR);
                    return false;
                }
                TipoDocumentoDAL d = new TipoDocumentoDAL();
                TipoDocumento p = new TipoDocumento();
                p = d.PesquisarTipoDocumento(Convert.ToInt32(txtCodigo.Text));

                if (p == null)
                {
                    return true;
                }
                else
                {
                    ApresentaMensagem("Código do Tipo de Documento já Cadastrado.");
                    return false;
                }
            }
            else
                return true;

        }
        protected Boolean ValidaDescricao()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Descrição do Tipo de Documento", txtDescricao.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
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
            if (!ValidaTipoDocumento()) return false;
            if (!ValidaDescricao()) return false;
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["ZoomTipoDocumento2"] != null)
            {
                if (Session["ZoomTipoDocumento2"].ToString() == "RELACIONAL")
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

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConTipoDocumento.aspx");

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                   if (Session["ZoomTipoDocumento2"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomTipoDocumento"] != null)
                    {
                        string s = Session["ZoomTipoDocumento"].ToString();
                        Session["ZoomTipoDocumento"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            btnExcluir.Visible = true;
                            foreach (string word in words)
                                if (txtCodigo.Text == "")
                                {
                                    LimpaCampos();

                                    txtCodigo.Text = word;
                                    txtCodigo.Enabled = false;

                                    TipoDocumentoDAL r = new TipoDocumentoDAL();
                                    TipoDocumento p = new TipoDocumento();

                                    p = r.PesquisarTipoDocumento(Convert.ToInt32(txtCodigo.Text));

                                    txtDescricao.Text = p.DescricaoTipoDocumento;
                                    txtCodSituacao.Text = p.CodigoSituacao.ToString();
                                    DdlDigitavel.SelectedValue = p.TipoDeCampo.ToString();
                                    DdlDigitavel_SelectedIndexChanged(sender, e);

                                    DdlIncEmpresa.SelectedValue = p.IncrementalPorEmpresa.ToString();
                                    DdlAbeSerie.SelectedValue = p.AberturaDeSerie.ToString();
                                    tx_tabela.Text = p.NomeDaTabela;


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
                        txtCodigo.Enabled=false;
                        LimpaCampos();

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
                if (txtCodigo.Text == "")
                    btnVoltar_Click(sender, e);


        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    TipoDocumentoDAL d = new TipoDocumentoDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Tipo de Documento Excluído com Sucesso!!!";
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
            try
            {
                if (txtCodigo.Text.Trim() != "")
                    pnlExcluir.Visible = true; 
            }
            catch (Exception ex)
            {
                btnCfmNao_Click(sender, e);
                ApresentaMensagem(ex.Message.ToString());
            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomTipoDocumento"] = null;
            if (Session["ZoomTipoDocumento2"] != null)
            {
                Session["ZoomTipoDocumento2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadTipoDocumento.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return; 
            }

            Response.Redirect("~/Pages/Empresas/ConTipoDocumento.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos() == false)
                    return;


                TipoDocumentoDAL d = new TipoDocumentoDAL();
                TipoDocumento p = new TipoDocumento();

                p.DescricaoTipoDocumento = txtDescricao.Text.ToUpper();
                p.CodigoSituacao = Convert.ToInt32(txtCodSituacao.SelectedValue);


                if (txtCodigo.Text == "Novo")
                {
                    //Inativo
                    p.CodigoSituacao = 2;
                    p.TipoDeCampo = Convert.ToInt32(DdlDigitavel.SelectedValue);
                    p.IncrementalPorEmpresa = Convert.ToInt32(DdlIncEmpresa.SelectedValue);
                    p.AberturaDeSerie = Convert.ToInt32(DdlAbeSerie.SelectedValue);
                    d.Inserir(p);
                    Session["MensagemTela"] = "Tipo de Documento Incluído com Sucesso!!!";
                }
                else
                {
                    p.CodigoTipoDocumento = Convert.ToInt32(txtCodigo.Text);
                    p.TipoDeCampo = Convert.ToInt32(DdlDigitavel.SelectedValue);
                    p.IncrementalPorEmpresa = Convert.ToInt32(DdlIncEmpresa.SelectedValue);
                    p.AberturaDeSerie = Convert.ToInt32(DdlAbeSerie.SelectedValue);

                    d.Atualizar(p);
                    Session["MensagemTela"] = "Tipo de Documento Alterado com Sucesso!!!";
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
            //Botão do OK da Mensagem

        }

        protected void DdlDigitavel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlDigitavel.SelectedValue == "21")
            {
                DdlAbeSerie.SelectedValue = "20";
                DdlIncEmpresa.SelectedValue = "20";

                DdlAbeSerie.Enabled = false;
                DdlIncEmpresa.Enabled = false; 
            }
            else
            {
                DdlAbeSerie.SelectedValue = "20";
                DdlIncEmpresa.SelectedValue = "20";

                DdlAbeSerie.Enabled = true;
                DdlIncEmpresa.Enabled = true;

            }
        }
    }
}