using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Produtos
{
    public partial class CadCEST : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        public string MaskCEST { get; set; }

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            // Validar conforme a critica data pela caracterista

            v.CampoValido("Código da CEST", txtCodigo.Text, true, false, false, false, "NVARCHAR", ref blnCampoValido, ref strMensagemR);
         
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
            else
            {
                CESTDAL d = new CESTDAL();
                CEST p = new CEST();
                p = d.PesquisarCEST(txtCodigo.Text);

                if ((txtLancamento.Text == "Novo") && (p != null))
                {
                    ShowMessage("Código da CEST já Cadastrado.", MessageType.Info);
                    txtDescricao.Focus();

                    return false;
                }
            }
            v.CampoValido("Código da NCM", txtNCM.Text, true, false, false, false, "NVARCHAR", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtNCM.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtNCM.Focus();

                }

                return false;
            }
           
            v.CampoValido("Descrição da CEST", txtDescricao.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

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

            MaskCEST = "99.999.99";

            txtDescricao.Focus();

            if (Session["ZoomCEST2"] != null)
            {
                if (Session["ZoomCEST2"].ToString() == "RELACIONAL")
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
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                           Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                           "ConCEST.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomCEST2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomCEST"] != null)
                {
                    string s = Session["ZoomCEST"].ToString();
                    Session["ZoomCEST"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                CESTDAL r = new CESTDAL();
                                CEST p = new CEST();
                                string strCod = word;

                                p = r.PesquisarCESTIndice(Convert.ToInt32(strCod));

                                txtLancamento.Text = p.CodigoIndice.ToString();
                                txtDescricao.Text = p.DescricaoCEST;
                                txtCodigo.Text = p.CodigoCEST;
                                txtNCM.Text = p.CodigoNCM;

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
                    if (Session["IncProdutoCEST"] != null)
                    {
                        txtLancamento.Text = "Novo";
                        txtLancamento.Enabled = false;

                        txtCodigo.Enabled = true;
                        txtCodigo.Focus();
                        btnExcluir.Visible = false;
                        return;
                    }
                    else
                    {
                        txtLancamento.Text = "Novo";
                        txtLancamento.Enabled = false;

                        txtCodigo.Text = "";
                        txtCodigo.Enabled = true;
                        txtCodigo.Focus();
                        btnExcluir.Visible = false;
                    }

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
                    CESTDAL d = new CESTDAL();
                    d.Excluir(Convert.ToInt32(txtLancamento.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da CEST não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomCEST"] = null;
            Response.Redirect("~/Pages/Produtos/ConCEST.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (ValidaCampos() == false)
                    return;


                CESTDAL d = new CESTDAL();
                CEST p = new CEST();

                p.DescricaoCEST = txtDescricao.Text.ToUpper();
                p.CodigoCEST = txtCodigo.Text;
                p.CodigoNCM = txtNCM.Text; 

                if (txtLancamento.Text == "Novo")
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "CEST inclusa com Sucesso!!!";
                }
                else
                {
                    p.CodigoIndice = Convert.ToInt32(txtLancamento.Text);
                    d.Atualizar(p);
                    Session["MensagemTela"] = "CEST alterada com Sucesso!!!";
                }

                if (Session["IncProdutoCEST"] != null)
                {
                    List<Produto> listCadProduto = new List<Produto>();
                    listCadProduto = (List<Produto>)Session["IncProdutoCEST"];
                    listCadProduto[0].CodigoCEST = txtCodigo.Text;
                    Session["IncProdutoCEST"] = listCadProduto;
                    Session["ZoomCEST2"] = null;

                    Session["MensagemTela"] = null;
                    Response.Redirect("~/Pages/Produtos/CadProduto.aspx");
                    return;
                }
                else
                    btnVoltar_Click(sender, e);

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
    }
}