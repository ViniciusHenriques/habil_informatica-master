using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Empresas
{
    public partial class CadGerSeqDocumento : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            if (ddlTipoDocumento.Text== "")
                return false;


            if (txtSerieNro.Enabled == true)
            {
                v.CampoValido("Série Conteúdo", txtSerieConteudo.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtSerieConteudo.Focus();

                    }

                    return false;
                }
            }
            if (txtSerieNro.Enabled == true)
            {
                v.CampoValido("Série Número", txtSerieNro.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtSerieNro.Focus();

                    }

                    return false;
                }
            }
            if(ddlTipoDocumento.Enabled == true)
            {
                if(ddlEmpresa.SelectedValue == "..... SELECIONE UMA EMPRESA .....")
                {
                    ShowMessage("Selecione uma empresa!", MessageType.Info);
                    return false;
                }
            }
            v.CampoValido("Descrição", txtDescricao.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }
            v.CampoValido("Data de Validade", txtValidade.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtValidade.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                }
                return false;
            }
            v.CampoValido("Número Inicial", txtNroInicial.Text, true, true, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtNroInicial.Focus();

                }

                return false;
            }
            return true;
        }
        protected void CarregaSituacoes()
        {
            Habil_TipoDAL RnSituacao = new Habil_TipoDAL();
            ddlSituacao.DataSource = RnSituacao.Atividade();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();


            TipoDocumentoDAL RnTipoDocumento = new TipoDocumentoDAL();
            ddlTipoDocumento.DataSource = RnTipoDocumento.ListarTipoDocumento("TP_CAMPO", "INT", "22", "");
            ddlTipoDocumento.DataTextField = "DescricaoTipoDocumento";
            ddlTipoDocumento.DataValueField = "CodigoTipoDocumento";
            ddlTipoDocumento.DataBind();
        }
        protected void LimpaTela()
        {
            CarregaSituacoes();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Error);
                Session["MensagemTela"] = null;
                return;
            }

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomGeracaoSequencial2"] != null)
            {
                if (Session["ZoomGeracaoSequencial2"].ToString() == "RELACIONAL")
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
                                            "ConGerSeqDocumento.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomGeracaoSequencial2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomGeracaoSequencial"] != null)
                {
                    string s = Session["ZoomGeracaoSequencial"].ToString();
                    Session["ZoomGeracaoSequencial"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {

                        foreach(string word in words)
                        {
                            if (word != "")
                            {
                                txtCod.Text = word;
                                CarregaSituacoes();

                                GeracaoSequencialDocumento ger = new GeracaoSequencialDocumento();
                                GeracaoSequencialDocumentoDAL gerDAL = new GeracaoSequencialDocumentoDAL();
                                ger = gerDAL.PesquisarGeradorSequencial(Convert.ToInt32(txtCod.Text));
                                ddlSituacao.SelectedValue = ger.CodigoSituacao.ToString();
                                txtDescricao.Text = ger.Descricao;
                                txtNome.Text = ger.Nome;
                                txtNroInicial.Text = ger.NumeroInicial.ToString();
                                txtSerieConteudo.Text = ger.SerieConteudo;
                                txtSerieNro.Text = ger.SerieNumero.ToString();
                                txtValidade.Text = ger.Validade.ToString().Substring(0, 10);
                                ddlTipoDocumento.SelectedValue = ger.CodigoTipoDocumento.ToString();
                                ddlTipoDocumento_TextChanged(sender, e);
                                if (ger.CodigoEmpresa != 0)
                                    ddlEmpresa.SelectedValue = Convert.ToString(ger.CodigoEmpresa);
                                if (ger.SerieConteudo != "")
                                    txtSerieConteudo.Text = ger.SerieConteudo;
                                if (ger.SerieNumero != 0)
                                    txtSerieNro.Text = ger.SerieNumero.ToString();
                                txtNome.Text = "HABIL_ERP_" + txtCod.Text.PadLeft(4,'0');
                                DBTabelaDAL db = new DBTabelaDAL();
                                string strRetorno = db.BuscaTabelas(txtNome.Text);

                                if (strRetorno == txtNome.Text)
                                    btnGerarTab.Visible = false;
                            }


                        }


                        lista.ForEach(delegate (Permissao x)
                        {
                            if (!x.AcessoCompleto)
                            {
                                if (!x.AcessoAlterar)
                                    btnSalvar.Visible = false;

                                
                            }
                        });
                        
                    }
                    
                }
                else
                {
                    
                    txtCod.Text = "Novo";
                    LimpaTela(); 
                    ddlTipoDocumento_TextChanged(sender, e);
                    btnGerarTab.Visible = false;
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


            if (txtCod.Text == "")
                btnVoltar_Click(sender, e);




        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                //if (txtCodEmpresa.Text.Trim() != "")
                //{
                //    PessoaDAL pe = new PessoaDAL();
                //    pe.PessoaEmpresa(Convert.ToInt64(txtCodPessoa.Text), false);
                //    EmpresaDAL d = new EmpresaDAL();
                //    d.Excluir(Convert.ToInt32(txtCodEmpresa.Text));
                //    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                //    btnVoltar_Click(sender, e);
                //}
                //else
                //    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Empresa não identificado.&emsp;&emsp;&emsp;";

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

            Response.Redirect("~/Pages/Empresas/ConGerSeqDocumento.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            GeracaoSequencialDocumento ger = new GeracaoSequencialDocumento();
            GeracaoSequencialDocumentoDAL gerDAL = new GeracaoSequencialDocumentoDAL();

            ger.CodigoTipoDocumento = Convert.ToInt32(ddlTipoDocumento.SelectedValue);

            if (ddlEmpresa.SelectedValue == "Tipo Documento não Incremental por empresa")
                ger.CodigoEmpresa = 0;
            else
                ger.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);

            if (txtSerieConteudo.Enabled == true)
                ger.SerieConteudo = txtSerieConteudo.Text;
            else
                ger.SerieConteudo = "";

            if (txtSerieNro.Enabled == true)
                ger.SerieNumero = Convert.ToInt64(txtSerieNro.Text);
            else
                ger.SerieNumero = 0;


            ger.Validade = Convert.ToDateTime(txtValidade.Text);
            ger.NumeroInicial = Convert.ToDecimal(txtNroInicial.Text);
            ger.Nome = txtNome.Text;
            ger.Descricao = txtDescricao.Text;
            ger.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            if(txtCod.Text == "Novo")
            {
                gerDAL.Inserir(ger);
                Session["MensagemTela"] = "Gerador Sequencial do documento cadastrado com sucesso";
            }
            else
            {
                ger.CodigoGeracaoSequencial = Convert.ToInt32(txtCod.Text);
                gerDAL.Atualizar(ger);
                Session["MensagemTela"] = "Gerador Sequencial do documento alterado com sucesso";
            }




          
            
           
            btnVoltar_Click(sender, e);

        }

        protected void btnVoltar_Click1(object sender, EventArgs e)
        {

        }

        protected void ddlTipoDocumento_TextChanged(object sender, EventArgs e)
        {
            TipoDocumento tpDoc = new TipoDocumento();
            TipoDocumentoDAL tpDocDAL = new TipoDocumentoDAL();

            if (ddlTipoDocumento.Text !="" )
                tpDoc = tpDocDAL.PesquisarTipoDocumento(Convert.ToInt32(ddlTipoDocumento.SelectedValue));

            if(tpDoc.IncrementalPorEmpresa == 19)
            {
                EmpresaDAL RnEmpresa = new EmpresaDAL();
                ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("", "", "", "");
                ddlEmpresa.DataTextField = "NomeEmpresa";
                ddlEmpresa.DataValueField = "CodigoEmpresa";
                ddlEmpresa.DataBind();
                ddlEmpresa.Items.Insert(0, "..... SELECIONE UMA EMPRESA .....");
                ddlEmpresa.Enabled = true;
            }
            else
            {

                ddlEmpresa.DataSource = "";
                ddlEmpresa.DataTextField = "";
                ddlEmpresa.DataValueField = "";
                ddlEmpresa.DataBind();
                ddlEmpresa.Items.Insert(0, "Tipo Documento não Incremental por empresa");
                ddlEmpresa.Enabled = false;
            }

            if(tpDoc.AberturaDeSerie == 19)
            {
                txtSerieConteudo.Enabled = true;
                txtSerieNro.Enabled = true;
                txtSerieNro.Text = "";
                txtSerieConteudo.Text = "";
            }
            else
            {
                txtSerieNro.Enabled = false;
                txtSerieConteudo.Enabled = false;
                txtSerieNro.Text = "Tipo Documento Sem abertura de Série";
                txtSerieConteudo.Text = "Tipo Documento Sem abertura de Série";
            }
        }

        protected void btnGerarTab_Click(object sender, EventArgs e)
        {
            GeracaoSequencialDocumento ger = new GeracaoSequencialDocumento();
            GeracaoSequencialDocumentoDAL gerDAL = new GeracaoSequencialDocumentoDAL();

            ger.CodigoTipoDocumento = Convert.ToInt32(ddlTipoDocumento.SelectedValue);

            if (ddlEmpresa.SelectedValue == "Tipo Documento não Incremental por empresa")
                ger.CodigoEmpresa = 0;
            else
                ger.CodigoEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);

            if (txtSerieConteudo.Enabled == true)
                ger.SerieConteudo = txtSerieConteudo.Text;
            else
                ger.SerieConteudo = "";

            if (txtSerieNro.Enabled == true)
                ger.SerieNumero = Convert.ToInt64(txtSerieNro.Text);
            else
                ger.SerieNumero = 0;


            ger.Validade = Convert.ToDateTime(txtValidade.Text);
            ger.NumeroInicial = Convert.ToDecimal(txtNroInicial.Text);
            ger.Nome = txtNome.Text;
            ger.Descricao = txtDescricao.Text;
            ger.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
            ger.CodigoGeracaoSequencial = Convert.ToInt32(txtCod.Text);

           
            gerDAL.CriarTabelaGeracaoSequencial(ger);
            ShowMessage("Processo concluído com sucesso", MessageType.Success);
            btnGerarTab.Visible = false;
            btnSalvar_Click(sender, e);
        }
    }
}