using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
namespace SoftHabilInformatica.Pages.Fiscal
    
{
    public partial class CadNatOperacao : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        List<NatOperacao> listCadNatOperacao = new List<NatOperacao>();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaCampos()
        {

            NatOperacaoDAL d = new NatOperacaoDAL();
            NatOperacao p = new NatOperacao();

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }


            Boolean blnCampoValido = false;

            v.CampoValido("Código da Natureza da Operação", txtCodigo.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodigo.Focus();

                }

                return false;
            }
            else
            {
                p = d.PesquisarNatOperacao(Convert.ToInt64(txtCodigo.Text));

                if ((p != null) && (txtCodigo.Enabled))
                {
                    ShowMessage("Código da Natureza já Cadastrado.", MessageType.Info);
                    txtCodigo.Focus();
                    return false;

                }
            }

            v.CampoValido("Descrição da Natureza da Operação", txtDescricao.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }


            return true;
        }

        protected void CarregaDropDownContraPartida()
        {
            NatOperacaoDAL RnNatOp = new NatOperacaoDAL();
            ddlCodCFOPContra.DataSource = RnNatOp.ListarNatOperacaoContraPartida(Convert.ToInt32(txtCodigo.Text.Substring(0, 1)));
            ddlCodCFOPContra.DataTextField = "DescricaoNaturezaOperacao";
            ddlCodCFOPContra.DataValueField = "CodigoNaturezaOperacao";
            ddlCodCFOPContra.DataBind();
            ddlCodCFOPContra.Items.Insert(0, "..... SELECIONE UMA NATUREZA DE CONTRA-PARTIDA .....");
        }

        protected void CarregaDropDown()
        {
            NatOperacaoDAL RnNatOp = new NatOperacaoDAL();
            ddlCodCFOPContra.DataSource = RnNatOp.ListarNatOperacao("","","","");
            ddlCodCFOPContra.DataTextField = "DescricaoNaturezaOperacao";
            ddlCodCFOPContra.DataValueField = "CodigoNaturezaOperacao";
            ddlCodCFOPContra.DataBind();
            ddlCodCFOPContra.Items.Insert(0, "..... SELECIONE UMA NATUREZA DE CONTRA-PARTIDA .....");

            ddlCodCFOPST.DataSource = RnNatOp.ListarNatOperacao("DS_NAT_OPERACAO","NVARCHAR", "SUBSTIT", "");
            ddlCodCFOPST.DataTextField = "DescricaoNaturezaOperacao";
            ddlCodCFOPST.DataValueField = "CodigoNaturezaOperacao";
            ddlCodCFOPST.DataBind();
            ddlCodCFOPST.Items.Insert(0, "..... SELECIONE UMA NATUREZA DE SUBSTITUIÇÃO TRIBUTÁRIA .....");

            lblCodCFOPST.Visible = false;
            lblCodCFOPST.Text = "";
            lblCodCFOPContra.Visible = false; 
            lblCodCFOPContra.Text = "";
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomNatOperacao2"] != null)
            {
                if (Session["ZoomNatOperacao2"].ToString() == "RELACIONAL")
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

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();

            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConNatOperacao.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomNatOperacao2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomNatOperacao"] != null)
                {
                    string s = Session["ZoomNatOperacao"].ToString();
                    Session["ZoomNatOperacao"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                CarregaDropDown();

                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                NatOperacaoDAL r = new NatOperacaoDAL();
                                NatOperacao p = new NatOperacao();
                               
                                p = r.PesquisarNatOperacao(Convert.ToInt64(txtCodigo.Text));
                                txtCodigo.Text = p.CodigoNaturezaOperacao.ToString();

                                CarregaDropDownContraPartida();

                                txtDescricao.Text = p.DescricaoNaturezaOperacao.ToString();
                                ddlCodCFOPContra.SelectedValue = Convert.ToString(p.CodigoNaturezaOperacaoContraPartida);
                                lblCodCFOPContra.Visible = true;
                                ddlCodCFOPST.SelectedValue = Convert.ToString(p.CodigoNaturezaOperacaoST);
                                lblCodCFOPST.Visible = true;

                                lblCodCFOPST.Text = ddlCodCFOPST.SelectedItem.Text;
                                lblCodCFOPContra.Text = ddlCodCFOPContra.SelectedItem.Text;

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
                    LimpaCampos();
                    txtCodigo.Enabled = true;
                    txtCodigo.Focus();
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
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void LimpaCampos()
        {
            txtCodigo.Text = "";
            txtCodigo.Enabled = false;
            txtDescricao.Text = "";
            CarregaDropDown();
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    NatOperacaoDAL d = new NatOperacaoDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Natureza da Operação excluída com Sucesso!!!";
                    
                    btnVoltar_Click(sender, e);
                    
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Natureza da Operação não identificado.&emsp;&emsp;&emsp;";

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
        protected void btnVoltar_Click(object sender, EventArgs e)
        {

            Session["ZoomNatOperacao"] = null;
            if (Session["ZoomNatOperacao2"] != null)
            {
                Session["ZoomNatOperacao2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadNatOperacao.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Session["InscricaoNatOperacao"] = null;
            
            listCadNatOperacao = null;

            Response.Redirect("~/Pages/Fiscal/ConNatOperacao.aspx");
            

        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
            {
                return;
            }

            NatOperacaoDAL d = new NatOperacaoDAL();
            NatOperacao p = new NatOperacao();
                                 

            p = d.PesquisarNatOperacao(Convert.ToInt64(txtCodigo.Text));

            if (txtCodigo.Enabled)
            {
                p = new NatOperacao();
                p.CodigoNaturezaOperacao = Convert.ToInt64(txtCodigo.Text);
                p.DescricaoNaturezaOperacao = txtDescricao.Text.ToUpper();

                if (ddlCodCFOPContra.SelectedValue != "..... SELECIONE UMA NATUREZA DE CONTRA-PARTIDA .....")
                {
                    p.CodigoNaturezaOperacaoContraPartida = Convert.ToInt64(ddlCodCFOPContra.SelectedValue);
                }

                if (ddlCodCFOPST.SelectedValue != "..... SELECIONE UMA NATUREZA DE SUBSTITUIÇÃO TRIBUTÁRIA .....")
                {
                    p.CodigoNaturezaOperacaoST = Convert.ToInt64(ddlCodCFOPST.SelectedValue);
                }

                d.Inserir(p);
                Session["MensagemTela"] = "Natureza Incluída com Sucesso!!!";

            }
            else
            {
                p = new NatOperacao();
                p.CodigoNaturezaOperacao = Convert.ToInt64(txtCodigo.Text);
                p.DescricaoNaturezaOperacao = txtDescricao.Text.ToUpper();

                if (ddlCodCFOPContra.SelectedValue != "..... SELECIONE UMA NATUREZA DE CONTRA-PARTIDA .....")
                    p.CodigoNaturezaOperacaoContraPartida = Convert.ToInt64(ddlCodCFOPContra.SelectedValue);

                if (ddlCodCFOPST.SelectedValue != "..... SELECIONE UMA NATUREZA DE SUBSTITUIÇÃO TRIBUTÁRIA .....")
                    p.CodigoNaturezaOperacaoST = Convert.ToInt64(ddlCodCFOPST.SelectedValue);

                d.Atualizar(p);

                Session["MensagemTela"] = "Natureza Alterada com Sucesso!!!";
            }

            btnVoltar_Click(sender, e);
            

        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            CarregaDropDownContraPartida();

            lblCodCFOPContra.Visible = false;
            lblCodCFOPContra.Text = "";
        }

        protected void ddlCodCFOPST_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCodCFOPST.Text = ddlCodCFOPST.SelectedItem.Text;
            lblCodCFOPST.Visible = true;
        }

        protected void ddlCodCFOPContra_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCodCFOPContra.Text = ddlCodCFOPContra.SelectedItem.Text;
            lblCodCFOPContra.Visible = true;

        }
    }
}