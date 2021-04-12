using System;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class CadTiposSituacao : System.Web.UI.Page
    {
        clsMensagem cls = new clsMensagem();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomHabilTipo"] != null)
                    {
                        string s = Session["ZoomHabilTipo"].ToString();
                        Session["ZoomHabilTipo"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            foreach (string word in words)
                                if (txtCodigo.Text == "")
                                {
                                    txtCodigo.Text = word;
                                    txtCodigo.Enabled = false;

                                    Habil_TipoDAL r = new Habil_TipoDAL();
                                    Habil_Tipo p = new Habil_Tipo();

                                    p = r.PesquisarHabil_Tipo(Convert.ToInt32(txtCodigo.Text));

                                    txtDescricao.Text = p.DescricaoTipo;
                                    txtObs.Text = p.Observacao;
                                    ddlTipoTipo.SelectedValue= p.TipoTipo.ToString();

                                    return;
                                }
                        }
                    }
                    else
                    {
                        txtCodigo.Focus();
                    }

                }



            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {

            if (ValidaCampos() == false)
                return;

            Habil_TipoDAL d = new Habil_TipoDAL();

            d.Excluir(Convert.ToInt32(txtCodigo.Text));
            Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
            btnVoltar_Click(sender, e);

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomHabilTipo"] = null;
            Response.Redirect("~/PagesAdmin/ConTiposSituacao.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            Habil_TipoDAL d = new Habil_TipoDAL();
            Habil_Tipo p = new Habil_Tipo();

            p.CodigoTipo = Convert.ToInt32(txtCodigo.Text);
            p.DescricaoTipo = txtDescricao.Text.ToUpper();
            p.TipoTipo = Convert.ToInt32(ddlTipoTipo.SelectedValue);
            p.Observacao = txtObs.Text.ToUpper();

            if (d.PesquisarHabil_Tipo(p.CodigoTipo) == null)
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                d.Atualizar(p);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }


            btnVoltar_Click(sender, e);

        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Relatorios/RelTiposSituacao.aspx");

        }
        protected bool ValidaCampos()
        {
            Boolean retorno = false;
            if (txtCodigo.Text == "")
            {
                txtCodigo.Focus();
                 Response.Write(cls.ExibeMensagem("Código da Situação é Obrigatório"));
                return retorno;
            }

            if (txtDescricao.Text == "")
            {
                txtDescricao.Focus();
                 Response.Write(cls.ExibeMensagem("Descrição da Situação é Obrigatório"));
                return retorno;
            }

            return true;
        }
    }
}