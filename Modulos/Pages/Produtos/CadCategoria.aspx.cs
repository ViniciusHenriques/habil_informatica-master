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
    public partial class CadCategoria : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        public string MaskCategoria { get; set; }

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void CarregaDropDown()
        {
            DepartamentoDAL sd = new DepartamentoDAL();
            ddlDepartamento.DataSource = sd.ListarDepartamento("", "", "", "");
            ddlDepartamento.DataTextField = "DescricaoDepartamento"; 
            ddlDepartamento.DataValueField = "CodigoDepartamento";
            ddlDepartamento.DataBind();
            ddlDepartamento.Items.Insert(0, "*Nenhum Selecionado");
            GpoComissaoDAL ep = new GpoComissaoDAL();
            ddlGpoComissao.DataSource = ep.ListarGpoComissao("", "", "", "");
            ddlGpoComissao.DataTextField = "DescricaoGpoComissao"; 
            ddlGpoComissao.DataValueField = "CodigoGpoComissao";
            ddlGpoComissao.DataBind();
            ddlGpoComissao.Items.Insert(0, "*Nenhum Selecionado");
        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            // Validar conforme a critica data pela caracterista

            v.CampoValido("Código da Categoria",  txtCodigo.Text,  true, false, false, false, "NVARCHAR", ref blnCampoValido, ref strMensagemR);

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
                CategoriaDAL d = new CategoriaDAL();
                Categoria p = new Categoria();
                p = d.PesquisarCategoria(txtCodigo.Text);

                if ((txtLancamento.Text == "Novo") && (p != null))
                {
                    ShowMessage("Código da Categoria já Cadastrado.", MessageType.Info);
                    txtDescricao.Focus();

                    return false;
                }
            }

            v.CampoValido("Descrição da Categoria", txtDescricao.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDescricao.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR,MessageType.Info);
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

            ParSistemaDAL RnParSis = new ParSistemaDAL();
            MaskCategoria = @RnParSis.FormataCategoria(1);

            txtDescricao.Focus();

            if (Session["ZoomCategoria2"] != null)
            {
                if (Session["ZoomCategoria2"].ToString() == "RELACIONAL")
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
                ShowMessage(Session["MensagemTela"].ToString(),MessageType.Info);
                Session["MensagemTela"] = "";
            }

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConCategoria.aspx");

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    if (Session["ZoomCategoria2"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomCategoria"] != null)
                    {
                        string s = Session["ZoomCategoria"].ToString();
                        Session["ZoomCategoria"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            btnExcluir.Visible = true;
                            foreach (string word in words)
                                if (txtCodigo.Text == "")
                                {
                                    CategoriaDAL r = new CategoriaDAL();
                                    Categoria p = new Categoria();
                                    string strCod = word;

                                    p = r.PesquisarCategoriaIndice(Convert.ToInt32(strCod));

                                    txtLancamento.Text = p.CodigoIndice.ToString();
                                    txtDescricao.Text = p.DescricaoCategoria;
                                    txtCodigo.Text = p.CodigoCategoria;
                                    CarregaDropDown();
                                    ddlDepartamento.SelectedValue = p.CodigoDepartamento.ToString();
                                    ddlGpoComissao.SelectedValue = p.CodigoGpoComissao.ToString();



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
                        if (Session["IncProdutoCategoria"] != null)
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
                        CarregaDropDown();

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
                    CategoriaDAL d = new CategoriaDAL();
                    d.Excluir(Convert.ToInt32(txtLancamento.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Categoria não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomCategoria"] = null;
            Response.Redirect("~/Pages/Produtos/ConCategoria.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (ValidaCampos() == false)
                    return;


                CategoriaDAL d = new CategoriaDAL();
                Categoria p = new Categoria();

                p.DescricaoCategoria = txtDescricao.Text.ToUpper();
                p.CodigoCategoria = txtCodigo.Text;

                if (ddlDepartamento.Text != "*Nenhum Selecionado")
                    p.CodigoDepartamento = Convert.ToInt32(ddlDepartamento.SelectedValue);

                if (ddlGpoComissao.Text != "*Nenhum Selecionado")
                        p.CodigoGpoComissao = Convert.ToInt32(ddlGpoComissao.SelectedValue);

                if (txtLancamento.Text == "Novo")
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Categoria inclusa com Sucesso!!!";
                }
                else
                {
                    p.CodigoIndice= Convert.ToInt32(txtLancamento.Text);
                    d.Atualizar(p);
                    Session["MensagemTela"] = "Categoria alterada com Sucesso!!!";
                }

                if (Session["IncProdutoCategoria"] != null)
                {
                    List<Produto> listCadProduto = new List<Produto>();
                    listCadProduto = (List<Produto>)Session["IncProdutoCategoria"];
                    listCadProduto[0].CodigoCategoria = txtCodigo.Text;
                    Session["IncProdutoCategoria"] = listCadProduto;
                    Session["ZoomCategoria2"] = null;

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