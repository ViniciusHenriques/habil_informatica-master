using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class CadBanco : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }

        List<BaixaDocumento> ListaBaixa = new List<BaixaDocumento>();

        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaTela()
        {

            //CarregaTiposSituacoes();

            txtDescricao.Text = "";
            txtCod.Text = "";




        }
        //protected void CarregaTiposSituacoes()
        //{
        //    Habil_TipoDAL sd = new Habil_TipoDAL();
        //    ddlSituacao.DataSource = sd.Atividade();
        //    ddlSituacao.DataTextField = "DescricaoTipo";
        //    ddlSituacao.DataValueField = "CodigoTipo";
        //    ddlSituacao.DataBind();


        //}
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Codigo Banco", txtCod.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);


            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCod.Focus();

                }

                return false;
            }
            v.CampoValido("Descrição", txtDescricao.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);


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
        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["TabFocadaManDocCtaPagar"] != null)
            {
                PanelSelect = Session["TabFocadaManDocCtaPagar"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaManDocCtaPagar"] = "home";
            }


            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "Conbanco.aspx");
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
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {

                if (Session["ZoomBanco2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomBanco"] != null)
                {
                    string s = Session["ZoomBanco"].ToString();
                    Session["ZoomBanco"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                txtCod.Text = word;
                                Banco banco = new Banco();
                                BancoDAL bancoDAL = new BancoDAL();
                                banco = bancoDAL.PesquisarBanco(Convert.ToInt32(txtCod.Text));
                                txtDescricao.Text = Convert.ToString(banco.DescricaoBanco);

                                


                            }
                        }
                    }
                }
                else
                {
                    LimpaTela();
                    btnExcluir.Visible = false;
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                            if (!p.AcessoExcluir)
                                btnExcluir.Visible = false;
                        }
                    });

                }


            }


        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCod.Text.Trim() != "")
                {
                    BancoDAL d = new BancoDAL();
                    d.Excluir(Convert.ToInt32(txtCod.Text));
                    Session["MensagemTela"] = "Banco excluído com Sucesso!!!";

                    btnVoltar_Click(sender, e);

                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Tipo de Serviço não identificado.&emsp;&emsp;&emsp;";

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
            Response.Redirect("~/Pages/Financeiros/ConBanco.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {


            if (ValidaCampos() == false)
                return;

            Banco ban = new Banco();
            BancoDAL banDAL = new BancoDAL();
            List<Banco> listBan = new List<Banco>();
            

                ban.DescricaoBanco = txtDescricao.Text.ToUpper();
                ban.CodigoBanco = Convert.ToInt32(txtCod.Text);


            listBan = banDAL.ListarBancos("CD_BANCO","INT",txtCod.Text,"");
            if(listBan.Count !=0)
            { 
                banDAL.Atualizar(ban);
                Session["MensagemTela"] = "Banco Alterado com sucesso!";
            }
            else
            {
                banDAL.Inserir(ban);
                Session["MensagemTela"] = "Banco cadastrada com sucesso!";
            }
            

            Response.Redirect("~/Pages/Financeiros/ConBanco.aspx");


            //else
            //{
            //    p.CodigoGrupoPessoa = Convert.ToInt32(txtCodigo.Text);
            //    d.Atualizar(p);

            //    Session["MensagemTela"] = "Grupo de Pessoa Alterado com Sucesso!!!";
            //}


            btnVoltar_Click(sender, e);




        }
        



    }
}