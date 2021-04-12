using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Impostos
{
    public partial class CadCOFINS : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<COFINS> listCOFINS = new List<COFINS>();
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código do COFINS", txtCodigo.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodigo.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodigo.Focus();

                }

                return false;
            }

            if (txtLancamento.Text == "Novo")
            {

                COFINSDAL r = new COFINSDAL();
                COFINS p = new COFINS();

                p = r.PesquisarCOFINS(txtCodigo.Text);

                if (p != null)
                {
                    ShowMessage("Código do COFINS já cadastrado", MessageType.Info);
                    txtCodigo.Focus();

                    return false;
                }
            }


            v.CampoValido("Código do TIPO", txtCodigo.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodigo.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodigo.Focus();

                }

                return false;
            }


            v.CampoValido("Valor do COFINS", txtVLCOFINS.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtVLCOFINS.Text = "0,00";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtVLCOFINS.Focus();

                }

                return false;
            }
            if (txtDescricaoCOFINS.Text.Length > 150)
            {
                ShowMessage("Descricao COFINS excedeu o número de caracteres", MessageType.Info);
                return false;
            }

            return true;
        }
        protected void LimpaCampos()
        {
            txtCodigo.Text = "";
            txtLancamento.Text = "";
            ddlCOFINS.Text = "";
            txtDescricaoCOFINS.Text = "";
            txtVLCOFINS.Text = "0,00";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtCodigo.Focus();

            if (Session["ZoomCOFINS2"] != null)
            {
                if (Session["ZoomCOFINS2"].ToString() == "RELACIONAL")
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
                                           "ConCOFINS.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomCOFINS2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomCOFINS"] != null)
                {
                    string s = Session["ZoomCOFINS"].ToString();
                    Session["ZoomCOFINS"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                txtLancamento.Text = word;
                                txtLancamento.Enabled = false;

                                COFINSDAL r = new COFINSDAL();
                                COFINS p = new COFINS();

                                CarregaSituacoes();

                                p = r.PesquisarCOFINSIndice(Convert.ToInt32(txtLancamento.Text));
                                txtCodigo.Text = p.CodigoCOFINS.ToString();
                                txtDescricaoCOFINS.Text = p.DescricaoCOFINS.ToString();
                                txtVLCOFINS.Text = p.ValorCOFINS.ToString();
                                ddlCOFINS.Text = p.CodigoTipo.ToString();
                                txtCodigo.Enabled = false;

                                lista.ForEach(delegate (Permissao x)
                                {
                                    if (!x.AcessoCompleto)
                                    {
                                        if (!x.AcessoAlterar)
                                            btnSalvar.Visible = false;

                                        if (!x.AcessoExcluir)
                                            btnExcluir.Visible = false;
                                    }
                                }
                                );

                                return;
                            }
                    }
                }
                else
                {
                    txtLancamento.Text = "Novo";
                    CarregaSituacoes();
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

            if (txtLancamento.Text == "")
                btnVoltar_Click(sender, e);

        }
        protected Boolean ValidaTipo()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código do tipo", txtCodigo.Text, true, true, true, false, "NUMERIC", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtCodigo.Text = "";
                txtDescricaoCOFINS.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodigo.Focus();
                }

                return false;
            }
            return blnCampoValido;
        }
        protected void txtCodTipo_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    COFINSDAL d = new COFINSDAL();
                    d.Excluir(Convert.ToInt16(txtCodigo.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do COFINS não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomCOFINS"] = null;
            if (Convert.ToInt16(Request.QueryString["Cad"]) == 1)
            {
                Response.Redirect("~/Pages/Impostos/CadCOFINS.aspx");

            }
            if (Session["ZoomCOFINS2"] != null)
            {
                Session["ZoomCOFINS2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadCOFINS.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Response.Redirect("~/Pages/Impostos/ConCOFINS.aspx");
        }

        protected void BtnConTipo_Click(object sender, EventArgs e)
        {
            long CodTipo = 0;

            if (txtCodigo.Text != "")
                CodTipo = Convert.ToInt64(txtCodigo.Text);

            COFINS x1 = new COFINS();

            listCOFINS  = new List<COFINS>();
            listCOFINS.Add(x1);
            Session["IncHabil_Tipo"] = listCOFINS;
            Session["ZoomHabil_Tipo"] = "RELACIONAL";
            Response.Redirect("~/Pages/Pessoas/ConHabil_Tipo.aspx?Cad=4");
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            COFINSDAL d = new COFINSDAL();
            COFINS p = new COFINS();

            p.CodigoCOFINS = Convert.ToInt16(txtCodigo.Text);
            p.CodigoTipo = Convert.ToInt16(ddlCOFINS.SelectedValue);
            p.ValorCOFINS = Convert.ToDouble(txtVLCOFINS.Text);
            p.DescricaoCOFINS = Convert.ToString(txtDescricaoCOFINS.Text);

            if (txtLancamento.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p.CodigoIndice = Convert.ToInt16(txtCodigo.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }

            if (Convert.ToInt16(Request.QueryString["cad"]) == 1)
            {
                Session["MensagemTela"] = null;
                Response.Redirect("~/Pages/Impostos/CadCOFINS.aspx");

            }
            else
                btnVoltar_Click(sender, e);
        }


        protected void CarregaSituacoes()
        {

            Habil_TipoDAL ep = new Habil_TipoDAL();
            ddlCOFINS.DataSource = ep.TipoTributacao();
            ddlCOFINS.DataTextField = "DescricaoTipo";
            ddlCOFINS.DataValueField = "CodigoTipo";
            ddlCOFINS.DataBind();
        }
    }
}