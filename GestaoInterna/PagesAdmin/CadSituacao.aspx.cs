using System;
using DAL.Model;
using DAL.Persistence;

namespace HabilInformaticaOriginal.PagesAdmin
{
    public partial class CadSituacao : System.Web.UI.Page
    {
        clsMensagem cls = new clsMensagem();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomSituacao"] != null)
                    {
                        string s = Session["ZoomSituacao"].ToString();
                        Session["ZoomSituacao"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            foreach (string word in words)
                                if (txtCodigo.Text == "")
                                {
                                    txtCodigo.Text = word;
                                    txtCodigo.Enabled = false;

                                    SituacaoDAL r = new SituacaoDAL();
                                    Situacao p = new Situacao();

                                    p = r.PesquisarSituacao(Convert.ToInt32(txtCodigo.Text));

                                    txtDescricao.Text = p.DescricaoSituacao;

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

            SituacaoDAL d = new SituacaoDAL();

            d.Excluir(Convert.ToInt32(txtCodigo.Text));
            Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
            btnVoltar_Click(sender, e);

        }


        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomSituacao"] = null;
            Response.Redirect("~/PagesAdmin/ConSituacao.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            SituacaoDAL d = new SituacaoDAL();
            Situacao p = new Situacao();

            p.CodigoSituacao = Convert.ToInt32(txtCodigo.Text);
            p.DescricaoSituacao = txtDescricao.Text.ToUpper();

            if (d.PesquisarSituacao(p.CodigoSituacao) == null)
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
            Response.Redirect("~/Relatorios/RelSituacao.aspx");

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