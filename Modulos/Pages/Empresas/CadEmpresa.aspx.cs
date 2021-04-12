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
    public partial class CadEmpresa : System.Web.UI.Page
    {
        List<GeradorSequencialDocumentoEmpresa> ListaGeracaoSequencial= new List<GeradorSequencialDocumentoEmpresa>();

        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void MontaDropDownList()
        {
            Habil_RegTributarioDAL rtd = new Habil_RegTributarioDAL();
            ddlRegTributario.DataSource = rtd.ListaHabil_RegTributario();
            ddlRegTributario.DataTextField = "DescricaoHabil_RegTributario";
            ddlRegTributario.DataValueField = "CodigoHabil_RegTributario";
            ddlRegTributario.DataBind();
            ddlRegTributario.Items.Insert(0, "..... SELECIONE UM REGIME DE TRIBUTAÇÃO .....");

            Habil_TipoDAL RnSituacao = new Habil_TipoDAL();
            txtCodSituacao.DataSource = RnSituacao.Atividade();
            txtCodSituacao.DataTextField = "DescricaoTipo";
            txtCodSituacao.DataValueField = "CodigoTipo";
            txtCodSituacao.DataBind();

            if (txtCodEmpresa.Text != "Novo")
            {
                GeracaoSequencialDocumentoDAL tipoDoc = new GeracaoSequencialDocumentoDAL();
                ddlGerSeq.DataSource = tipoDoc.ListarGeracaoSequencial("CD_EMPRESA", "INT", txtCodEmpresa.Text, "");
                ddlGerSeq.DataTextField = "Descricao";
                ddlGerSeq.DataValueField = "CodigoGeracaoSequencial";
                ddlGerSeq.DataBind();
                ddlGerSeq.Items.Insert(0, "..... SELECIONE UM GERADOR SEQUENCIAL .....");
            }
        }
        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código da Pessoa", txtCodPessoa.Text, true, true, true, false, "INT", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                    ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }

            EmpresaDAL e = new EmpresaDAL();
            Empresa e1 = new Empresa();

            e1 = e.PesquisarEmpresaPessoa(Convert.ToInt64(txtCodPessoa.Text));

            if (e1 != null)
            {
                if (e1.CodigoEmpresa.ToString() != txtCodEmpresa.Text)
                {
                    ShowMessage("Pessoa já consta na Empresa: " + e1.CodigoEmpresa.ToString(), MessageType.Error);
                    return false;
                }

            }

            if (ddlRegTributario.Text == "..... SELECIONE UM REGIME DE TRIBUTAÇÃO .....")
            {
                ShowMessage("Selecione um Regime de Tributação.", MessageType.Info);
                return false;
            }

            return true;
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

            if (Session["ZoomEmpresa2"] != null)
            {
                if (Session["ZoomEmpresa2"].ToString() == "RELACIONAL")
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
            if (Session["TabFocadaEmpresa"] != null)
            {
                PanelSelect = Session["TabFocadaEmpresa"].ToString();
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocadaEmpresa"] = "home";
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConEmpresa.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomEmpresa2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomEmpresa"] != null)
                {
                    string s = Session["ZoomEmpresa"].ToString();
                    Session["ZoomEmpresa"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
 
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodEmpresa.Text == "")
                            {
                                txtCodEmpresa.Text = word;
                                txtCodEmpresa.Enabled = false;

                                MontaDropDownList();
                                EmpresaDAL r = new EmpresaDAL();
                                Empresa p = new Empresa();

                                p = r.PesquisarEmpresa(Convert.ToInt32(txtCodEmpresa.Text));

                                txtNomeEmpresa.Text = p.NomeEmpresa;
                                txtCodPessoa.Text = p.CodigoPessoa.ToString();
                                ddlRegTributario.SelectedValue = p.CodHabil_RegTributario.ToString();
                                txtCodSituacao.SelectedValue = p.CodigoSituacao.ToString();

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


                                PerfilUsuarioEmpresaDAL s1 = new PerfilUsuarioEmpresaDAL();
                                grdPermissao.DataSource = s1.ListarEmpresaPerfilUsuario(p.CodigoEmpresa);
                                grdPermissao.DataBind();

                                GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
                                ListaGeracaoSequencial = geradorDAL.ObterGeradorSequencialDocumentoEmpresa(Convert.ToInt32(txtCodEmpresa.Text));
                                Session["ListaGeracaoSequencial"] = ListaGeracaoSequencial;

                            }
                    }
                }
                else
                {
                    txtCodEmpresa.Text = "Novo";
                    btnExcluir.Visible = false;

                    MontaDropDownList();
                    ddlGerSeq.Visible = false;
                    BtnAddGerSeq.Visible = false;
                    lblGerSeq.Visible = false;
                    Session["ListaGeracaoSequencial"] = null;
                    
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

            if (Session["IncEmpresa"] != null)
            {
                String strIncEmpresa = Session["IncEmpresa"].ToString();
                Session["IncEmpresa"] = null;
                string[] wordsEmp = strIncEmpresa.Split('³');
                if (strIncEmpresa != "³")
                {
                    txtCodEmpresa.Text = wordsEmp[0];
                    txtCodPessoa.Text = wordsEmp[1];
                    txtNomeEmpresa.Text = wordsEmp[2];
                }
            }
            if (Session["IncEmpresa2"] != null)
            {
                String strIncEmpresa2 = Session["IncEmpresa2"].ToString();
                Session["IncEmpresa2"] = null;
                string[] wordsEmp2 = strIncEmpresa2.Split('³');
                if (strIncEmpresa2 != "³")
                {
                    txtCodPessoa.Text = wordsEmp2[0];
                    txtNomeEmpresa.Text = wordsEmp2[1];
                }
            }
            if (txtCodEmpresa.Text == "")
                btnVoltar_Click(sender, e);

            if (Session["ListaGeracaoSequencial"] != null)
            {
                ListaGeracaoSequencial= (List<GeradorSequencialDocumentoEmpresa>)Session["ListaGeracaoSequencial"];
                grdGerSeq.DataSource = ListaGeracaoSequencial;
                grdGerSeq.DataBind();
                
            }
           
            if (Session["CodUsuario"].ToString() == "-150380")
            {
                btnSalvar.Visible = true;
                if (txtCodEmpresa.Text != "Novo")
                    btnExcluir.Visible = true;
            }

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            string strErro = "";
            try
            {
                if (txtCodEmpresa.Text.Trim() != "")
                {
                    PessoaDAL pe = new PessoaDAL();
                    pe.PessoaEmpresaUsuario(Convert.ToInt64(txtCodPessoa.Text),1, false, false);
                    EmpresaDAL d = new EmpresaDAL();
                    d.Excluir(Convert.ToInt32(txtCodEmpresa.Text));
                    Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código da Empresa não identificado.&emsp;&emsp;&emsp;";

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
            Session["ZoomEmpresa"] = null;
            if (Session["ZoomEmpresa2"] != null)
            {
                Session["ZoomEmpresa2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadEmpresa.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return; 
            }
            Session["TabFocadaEmpresa"] = null;
            Response.Redirect("~/Pages/Empresas/ConEmpresa.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            EmpresaDAL d = new EmpresaDAL();
            Empresa p = new Empresa();

            PessoaDAL pe = new PessoaDAL();
            pe.PessoaEmpresaUsuario(Convert.ToInt64(txtCodPessoa.Text),1, true,false);

            p.CodigoPessoa = Convert.ToInt64(txtCodPessoa.Text);
            p.NomeEmpresa = txtNomeEmpresa.Text.ToUpper();
            p.CodHabil_RegTributario = Convert.ToInt32(ddlRegTributario.SelectedValue);

            p.CodigoSituacao = Convert.ToInt32(txtCodSituacao.SelectedValue);


            if (txtCodEmpresa.Text == "Novo")
            {
                d.Inserir(p);
                
                Session["MensagemTela"] = "Registro Incluído com Sucesso!!!";
            }
            else
            {
                p.CodigoEmpresa = Convert.ToInt32(txtCodEmpresa.Text);
                d.Atualizar(p,ListaGeracaoSequencial);
                Session["MensagemTela"] = "Registro Alterado com Sucesso!!!";
            }

            //
            PerfilUsuarioEmpresaDAL pue = new PerfilUsuarioEmpresaDAL();

            foreach (GridViewRow row in grdPermissao.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLiberado");

                if (chk.Checked == false)
                    pue.ExcluirPermissao(Convert.ToInt64(txtCodEmpresa.Text), Convert.ToInt64(row.Cells[1].Text));
                else
                {
                    pue.SalvarPermissao(Convert.ToInt32(txtCodEmpresa.Text), Convert.ToInt64(row.Cells[1].Text));
                }
            }
            btnVoltar_Click(sender, e);

        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Session["IncEmpresa"] = txtCodEmpresa.Text + "³" + txtCodPessoa.Text + "³" + txtNomeEmpresa.Text;
            Session["LST_CADPESSOA"] = null;
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=1");
        }

        protected void grdGerSeq_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["TabFocadaEmpresa"] = "consulta";
            PanelSelect = "consulta";
            List<GeradorSequencialDocumentoEmpresa> ListaGerSeq= new List<GeradorSequencialDocumentoEmpresa>();

            if (Session["ListaGeracaoSequencial"] != null)
                ListaGeracaoSequencial = (List<GeradorSequencialDocumentoEmpresa>)Session["ListaGeracaoSequencial"];
            else
                ListaGeracaoSequencial = new List<GeradorSequencialDocumentoEmpresa>();

            GeradorSequencialDocumentoEmpresa tabi;
            foreach (GeradorSequencialDocumentoEmpresa item in ListaGeracaoSequencial)
            {
                if (item.CodigoGeradorSequencialDocumento != Convert.ToInt32(HttpUtility.HtmlDecode(grdGerSeq.SelectedRow.Cells[0].Text)))
                {
                    tabi = new GeradorSequencialDocumentoEmpresa();
                    tabi.CodigoGeradorSequencialDocumento = item.CodigoGeradorSequencialDocumento;
                    tabi.CodigoEmpresa = item.CodigoEmpresa;
                    tabi.Cpl_TipoDocumento = item.Cpl_TipoDocumento;
                    tabi.Cpl_SerieNumero = item.Cpl_SerieNumero;
                    tabi.Cpl_SerieConteudo = item.Cpl_SerieConteudo;
                    tabi.Cpl_Nome = item.Cpl_Nome;
                    tabi.Cpl_Descricao = item.Cpl_Descricao;
                    ListaGerSeq.Add(tabi);
                }
            }

            grdGerSeq.DataSource = ListaGerSeq;
            grdGerSeq.DataBind();
            Session["ListaGeracaoSequencial"] = ListaGerSeq;
        }


        protected void BtnAddGerSeq_Click(object sender, EventArgs e)
        {

            Session["TabFocadaEmpresa"] = "consulta";
            PanelSelect = "consulta";
            foreach (GeradorSequencialDocumentoEmpresa item in ListaGeracaoSequencial)
            {
                if (ddlGerSeq.SelectedValue == item.CodigoGeradorSequencialDocumento.ToString())
                {
                    ShowMessage("Gerador Sequencial já adicionado", MessageType.Info);
                    return;
                }
            }
            if (ddlGerSeq.SelectedValue == "..... SELECIONE UM GERADOR SEQUENCIAL .....")
            {
                
                ShowMessage("Selecione um Gerador Sequencial", MessageType.Info);
                return;
            }
            GeracaoSequencialDocumento gerSeq = new GeracaoSequencialDocumento();
            GeracaoSequencialDocumentoDAL gerSeqDAL = new GeracaoSequencialDocumentoDAL();
            gerSeq = gerSeqDAL.PesquisarGeradorSequencial(Convert.ToInt32(ddlGerSeq.SelectedValue));

            foreach(GeradorSequencialDocumentoEmpresa ItemGerador in ListaGeracaoSequencial)
            {
                GeracaoSequencialDocumento gerSeq2 = new GeracaoSequencialDocumento();
                gerSeq2 = gerSeqDAL.PesquisarGeradorSequencial(ItemGerador.CodigoGeradorSequencialDocumento);

                if (gerSeq.CodigoTipoDocumento == gerSeq2.CodigoTipoDocumento)
                {
                    ShowMessage("Tipo de Documento já Existente.", MessageType.Info);
                    return;
                }
            }
            GeradorSequencialDocumentoEmpresa gerSeqEmpresa = new GeradorSequencialDocumentoEmpresa(Convert.ToInt32(txtCodEmpresa.Text),
                                                                                                    Convert.ToInt32(ddlGerSeq.SelectedValue),
                                                                                                    gerSeq.SerieConteudo,
                                                                                                    gerSeq.SerieNumero,
                                                                                                    gerSeq.Descricao,
                                                                                                    gerSeq.CodigoTipoDocumento);


            ListaGeracaoSequencial.Add(gerSeqEmpresa);


            Session["ListaGeracaoSequencial"] = ListaGeracaoSequencial;
            grdGerSeq.DataSource = ListaGeracaoSequencial;
            grdGerSeq.DataBind();

            Session["TabFocadaEmpresa"] = "consulta";
            PanelSelect = "consulta";
        }

        protected void btnAddTipoDoc_Click(object sender, EventArgs e)
        {

        }

       
    }
}