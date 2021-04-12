using System;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class CadMenu : System.Web.UI.Page
    {
        clsMensagem cls = new clsMensagem();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath) 
            {
                Session["ZoomModSistema"] = null;
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                LimpaTela();

                if (Session["ZoomMenu"] != null)
                {
                    string s = Session["ZoomMenu"].ToString();
                    Session["ZoomMenu"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                            if (txtCodMenu.Text == "Novo")
                            {
                                txtCodMenu.Text = word;
                                txtCodMenu.Enabled = false;

                                MenuSistemaDAL m = new MenuSistemaDAL();
                                MenuSistema p = new MenuSistema();

                                p = m.PesquisarMenuSistema(Convert.ToInt32(txtCodMenu.Text));

                                txtCodModulo.Text = p.CodigoModulo.ToString();
                                txtCodModulo_TextChanged(sender, e);
                                txtNomeMenu.Text = p.NomeMenu;
                                txtDcrMenu.Text = p.DescricaoMenu;
                                txtOrdApresentacao.Text = p.CodigoOrdem.ToString();
                                txtCodPaiMenu.Text = p.CodigoPaiMenu.ToString();
                                txtUrlPrograma.Text = p.UrlPrograma;
                                ddlTipoFormulario.Text = p.TipoFormulario;
                                txtAjuda.Text = p.TextoAjuda;
                                TxtUrlIcone.Text = p.UrlIcone;
                                btnExcluir.Visible = true; 
                                return;
                            }
                    }
                }
                
            }




            if ((Session["ZoomModSistema"] != null) && (Session["Operacao"].ToString() == "EDICAO"))
            {
                string s = Session["ZoomModSistema"].ToString();
                string[] words = s.Split('²');

                if (s != "³")
                {
                    txtCodModulo.Text = "";
                    txtDcrModulo.Text = "";

                    foreach (string word in words)
                    {
                        if (txtCodModulo.Text == "")
                            txtCodModulo.Text = word;
                        else
                            if (txtDcrModulo.Text == "")
                                txtDcrModulo.Text = word;
                    }
                    Session["ZoomModSistema"] = null;
                }
            }

        }



        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PagesAdmin/ConMenu.aspx");
            Session["Operacao"] = "CONSULTA";
            Session["ZoomMenu"] = null;
        }

        protected void LimpaTela()
        {

            txtCodMenu.Text = "Novo";
            txtCodMenu.Enabled = false;
            txtCodModulo .Text = "";
            txtDcrModulo.Text = "";
            txtNomeMenu.Text = "";
            txtDcrMenu.Text = "";
            txtOrdApresentacao.Text = "0";
            txtCodPaiMenu.Text = "0";
            txtUrlPrograma.Text = @"~/";
            ddlTipoFormulario.Text = "Nenhum";
            txtAjuda.Text = "";
            TxtUrlIcone.Text = "~/images/nulo.png";
            btnExcluir.Visible = false;
            Session["Operacao"] = "EDICAO";
            txtCodModulo.Focus();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

            if (ValidaCampos() == false)
                return;
            

            MenuSistemaDAL d = new MenuSistemaDAL();
            MenuSistema p = new MenuSistema();

            p.CodigoModulo = Convert.ToInt32(txtCodModulo.Text);
            p.CodigoOrdem = Convert.ToInt32(txtOrdApresentacao.Text);
            p.CodigoPaiMenu = Convert.ToInt32(txtCodPaiMenu.Text);
            p.NomeMenu = txtNomeMenu.Text;
            p.DescricaoMenu= txtDcrMenu.Text;
            p.UrlPrograma = txtUrlPrograma.Text;
            p.TextoAjuda = txtAjuda.Text;
            p.TipoFormulario = ddlTipoFormulario.Text;
            p.UrlIcone = TxtUrlIcone.Text;


            if (txtCodMenu.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] ="Registro Incluído com Sucesso";
            }
            else
            {
                p.CodigoMenu = Convert.ToInt32(txtCodMenu.Text) ;
                d.Atualizar(p);
                 Session["MensagemTela"] ="Registro Alterado com Sucesso";
            }

            btnVoltar_Click(sender, e);

        }


        protected void txtCodModulo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodModulo.Text != "") 
            {
                ModuloSistemaDAL d = new ModuloSistemaDAL();
                ModuloSistema p = new ModuloSistema();
                p = d.PesquisarModuloSistema(Convert.ToInt32(txtCodModulo.Text));

                if (p == null)
                    txtDcrModulo.Text = "";
                else
                    txtDcrModulo.Text = p.DescricaoModulo;
            }
            else
                txtDcrModulo.Text = "";
                      
        }

        protected Boolean ValidaCampos()
        {
            Boolean retorno = false;
            if (txtCodModulo.Text == "")
            {
                txtCodModulo.Focus();
                return retorno;
            }

            if (txtCodPaiMenu.Text == "")
            {
                txtCodPaiMenu.Focus();
                return retorno;
            }

            if (txtOrdApresentacao.Text == "")
            {
                txtOrdApresentacao.Focus();
                return retorno;
            }

            return true;
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (txtCodMenu.Text.Trim() != "")
                pnlExcluir.Visible = true; 
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {

            MenuSistemaDAL d = new MenuSistemaDAL();

            if (txtCodMenu.Text.Trim() != "")
            {

                d.Excluir(Convert.ToInt32(txtCodMenu.Text));
                Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                pnlExcluir.Visible = false;
                btnVoltar_Click(sender, e);
            }
        }

        protected void btnCfmNao_Click(object sender, EventArgs e)
        {
            pnlExcluir.Visible = false; 

        }

        protected void btnConfirma_Click(object sender, EventArgs e)
        {

        }
    }
}