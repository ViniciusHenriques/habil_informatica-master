using System;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class CadModSistema : System.Web.UI.Page
    {
        clsMensagem cls = new clsMensagem();

        protected Boolean ValidaCampos()
        {
            Boolean retorno = false;
            if (txtCodigo.Text == "")
            {
                Response.Write(cls.ExibeMensagem("Código do Módulo deve ser informado!!!"));
                txtCodigo.Focus();
                return retorno;
            }

            if (txtDescricao.Text == "")
            {
                Response.Write(cls.ExibeMensagem("Descrição do Módulo deve ser informado!!!"));
                txtDescricao.Focus();
                return retorno;
            }

            return true;
        }

        protected void LimparTela()
        {
            txtCodigo.Text = "";
            txtDescricao.Text = "";

            txtCodigo.Enabled = true;
            txtCodigo.Focus();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomModSistema"] != null)
                {
                    string s = Session["ZoomModSistema"].ToString();
                    Session["ZoomModSistema"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                ModuloSistemaDAL r = new ModuloSistemaDAL();
                                ModuloSistema p = new ModuloSistema();

                                p = r.PesquisarModuloSistema(Convert.ToInt32(txtCodigo.Text));

                                txtDescricao.Text = p.DescricaoModulo;

                                return;
                            }
                    }
                }
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {

            if (ValidaCampos() == false)
                return;

            ModuloSistemaDAL d = new ModuloSistemaDAL();

            d.Excluir(Convert.ToInt32(txtCodigo.Text));
            Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            Session["ZoomModSistema"] = null;
            Response.Redirect("~/PagesAdmin/ConModSistema.aspx");

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomModSistema"] = null;
            Response.Redirect("~/PagesAdmin/ConModSistema.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;
            
            
            ModuloSistemaDAL d = new ModuloSistemaDAL();
            ModuloSistema p = new ModuloSistema();

            p.CodigoModulo = Convert.ToInt32(txtCodigo.Text);
            p.DescricaoModulo = txtDescricao.Text.ToUpper();

            if (d.PesquisarModuloSistema(p.CodigoModulo) == null)
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            Session["ZoomModSistema"] = null;
            Response.Redirect("~/PagesAdmin/ConModSistema.aspx");
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Relatorios/RelModSistema.aspx");
        }

    }
}