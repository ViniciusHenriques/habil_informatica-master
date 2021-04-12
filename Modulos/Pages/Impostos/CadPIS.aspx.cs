using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Impostos
{
    public partial class CadPIS : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<PIS> listPIS = new List<PIS>();
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código do PIS", txtCodigo.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

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

                PISDAL r = new PISDAL();
                PIS p = new PIS();

                p = r.PesquisarPIS(txtCodigo.Text);

                if (p != null)
                {
                    ShowMessage("Código do PIS já cadastrado",MessageType.Info );
                    txtCodigo.Focus();

                    return false;
                }
            }

            v.CampoValido("Valor do PIS", txtVLPIS.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtVLPIS.Text = "0,00";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtVLPIS.Focus();

                }

                return false;
            }
            if (txtDescricaoPIS.Text.Length > 150)
            {
                ShowMessage("Descricao PIS excedeu o número de caracteres", MessageType.Info);
                return false;
            }
            return true;
        }
        protected void LimpaCampos()
        {
            txtCodigo.Text = "";
            txtLancamento.Text = "";
            ddlPIS.Text = "";
            txtDescricaoPIS.Text = "";
            txtVLPIS.Text = "0,00";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            txtCodigo.Focus();

            if (Session["ZoomPIS2"] != null)
            {
                if (Session["ZoomPIS2"].ToString() == "RELACIONAL")
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
                                           "ConPIS.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomPIS2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomPIS"] != null)
                {
                    string s = Session["ZoomPIS"].ToString();
                    Session["ZoomPIS"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                txtLancamento.Text = word;
                                txtLancamento.Enabled = false;

                                PISDAL r = new PISDAL();
                                PIS p = new PIS();

                                CarregaSituacoes();

                                p = r.PesquisarPISIndice(Convert.ToInt32(txtLancamento.Text));
                                ddlPIS.Text = p.CodigoTipo.ToString();
                                txtDescricaoPIS.Text = p.DescricaoPIS.ToString();
                                txtVLPIS.Text = p.ValorPIS.ToString();
                                txtCodigo.Text = p.CodigoPIS.ToString();
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
        
       
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    PISDAL d = new PISDAL();
                    d.Excluir(Convert.ToInt16(txtCodigo.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do PIS não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomPIS"] = null;
            if (Convert.ToInt16(Request.QueryString["Cad"]) == 1)
            {
                Response.Redirect("~/Pages/Impostos/CadPIS.aspx");

            }
            if (Session["ZoomPIS2"] != null)
            {
                Session["ZoomPIS2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadPIS.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Response.Redirect("~/Pages/Impostos/ConPIS.aspx");
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            PIS p = new PIS();
            PISDAL d = new PISDAL();

            

            p.CodigoPIS = Convert.ToInt16(txtCodigo.Text);
            p.CodigoTipo = Convert.ToInt16(ddlPIS.SelectedValue);
            p.ValorPIS = Convert.ToDouble(txtVLPIS.Text);
            p.DescricaoPIS = Convert.ToString(txtDescricaoPIS.Text);

            if (txtLancamento.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p.CodigoIndice = Convert.ToInt16(txtLancamento.Text);
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }

            if (Convert.ToInt16(Request.QueryString["cad"]) == 1)
            {
                Session["MensagemTela"] = null;
                Response.Redirect("~/Pages/Impostos/CadPIS.aspx");

            }
            else
                btnVoltar_Click(sender, e);
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            
        }
         
        protected void CarregaSituacoes()
        {
          
            Habil_TipoDAL ep = new Habil_TipoDAL();
            ddlPIS.DataSource = ep.TipoTributacao();
            ddlPIS.DataTextField = "DescricaoTipo";
            ddlPIS.DataValueField = "CodigoTipo";
            ddlPIS.DataBind(); 
        }


    }
}