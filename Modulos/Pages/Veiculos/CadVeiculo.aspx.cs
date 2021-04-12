using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Veiculos
{
    public partial class CadVeiculo : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        List<Veiculo> listCadVeiculo = new List<Veiculo>();
        protected bool ValidaCampos()
        {

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }


            Boolean blnCampoValido = false;

            v.CampoValido("Placa do Veículo", txtPlaca.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDscVeiculo.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtPlaca.Focus();

                }

                return false;
            }

            if (ddlCatVeiculo.Text == "..... SELECIONE UMA CATEGORIA DO VEÍCULO .....")
            {
                ShowMessage("Categoria deve ser Selecionado.", MessageType.Info);
                return false;
            }

            v.CampoValido("Descrição do Veículo", txtDscVeiculo.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDscVeiculo.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDscVeiculo.Focus();

                }

                return false;
            }


            return true;
        }
        protected void LimpaCampos()
        {
            txtCodVeiculo.Text = "Novo";
            txtPlaca.Text = "";
            txtDscVeiculo.Text = "";

            CategoriaVeiculoDAL d = new CategoriaVeiculoDAL();
            ddlCatVeiculo.DataSource = d.ListarCategoriaVeiculos("","","","");
            ddlCatVeiculo.DataTextField = "DescricaoCategoriaVeiculo";
            ddlCatVeiculo.DataValueField = "CodigoCategoriaVeiculo";
            ddlCatVeiculo.DataBind();
            ddlCatVeiculo.Items.Insert(0, "..... SELECIONE UMA CATEGORIA DO VEÍCULO .....");

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

            if (Session["ZoomVeiculo2"] != null)
            {
                if (Session["ZoomVeiculo2"].ToString() == "RELACIONAL")
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
                                               "ConVeiculo.aspx");

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

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    if (Session["ZoomVeiculo2"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;

                   if (Session["IncVeiculoCategoria"] == null)
                      Session["Pagina"] = Request.CurrentExecutionFilePath;


                if (Session["ZoomVeiculo"] != null)
                    {
                        string s = Session["ZoomVeiculo"].ToString();
                        Session["ZoomVeiculo"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            btnExcluir.Visible = true;
                            foreach (string word in words)
                                if (txtCodVeiculo.Text == "")
                                {
                                    LimpaCampos();

                                    txtCodVeiculo.Text = word;
                                    txtCodVeiculo.Enabled = false;

                                    VeiculoDAL re = new VeiculoDAL();
                                    Veiculo p = new Veiculo();

                                    p = re.PesquisarVeiculo(Convert.ToInt64(txtCodVeiculo.Text));

                                txtDscVeiculo.Text = p.NomeVeiculo;
                                txtPlaca.Text = p.Placa;
                                ddlCatVeiculo.SelectedValue = p.CategoriaVeiculo.ToString();
                                }
                        }
                    }
                    else
                    {
                        LimpaCampos();
                        btnExcluir.Visible = false;
                    }

                    if (Session["IncVeiculoCategoria"] != null) 
                    {

                        if (Session["IncVeiculoCategoria"] != null)
                            listCadVeiculo = (List<Veiculo>)Session["IncVeiculoCategoria"];

                        
                        foreach (Veiculo p in listCadVeiculo)
                        {

                            if (p.CodigoVeiculo == 0)
                                txtCodVeiculo.Text = "Novo";
                            else
                                txtCodVeiculo.Text = p.CodigoVeiculo.ToString();

                            txtDscVeiculo.Text = p.NomeVeiculo;
                            ddlCatVeiculo.SelectedValue = p.CategoriaVeiculo.ToString();
                            txtPlaca.Text = p.Placa;
                        }
                        listCadVeiculo = null;
                        Session["IncVeiculoCategoria"] = null;
                        
                    }
                }
                if(txtCodVeiculo.Text == "")
                    btnVoltar_Click(sender, e);
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodVeiculo.Text.Trim() != "")
                {
                    VeiculoDAL d = new VeiculoDAL();
                    d.Excluir(Convert.ToInt64(txtCodVeiculo.Text));
                    Session["MensagemTela"] = "Veículo Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Veículo não identificado.&emsp;&emsp;&emsp;";

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

            if (Session["IncMovAcessoVeiculo"] != null)
            {
                Session["MensagemTela"] = null;
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                Response.Redirect("~/Pages/Acesso/ManMovAcesso.aspx");
                return;
            }

            Session["ZoomVeiculo"] = null;
            if (Session["ZoomVeiculo2"] != null)
            {
                Session["ZoomVeiculo2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadVeiculo.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return; 
            }

            Response.Redirect("~/Pages/Veiculos/ConVeiculo.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            VeiculoDAL d = new VeiculoDAL();
            Veiculo p = new Veiculo();

            p.NomeVeiculo= txtDscVeiculo.Text.ToUpper();
            p.Placa = txtPlaca.Text.ToUpper(); ;
            p.CategoriaVeiculo = Convert.ToInt32(ddlCatVeiculo.SelectedValue);

            if (txtCodVeiculo.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Veículo incluso com Sucesso!!!";
            }
            else
            {
                p.CodigoVeiculo = Convert.ToInt64(txtCodVeiculo.Text);
                d.Atualizar(p);

                Session["MensagemTela"] = "Veículo alterado com Sucesso!!!";
            }

            btnVoltar_Click(sender, e);

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
 
        }
        protected void ddlCatVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void btnAddCatVeiculo_Click(object sender, EventArgs e)
        {
            long CodVeiculo = 0;
            int CodTipo = 0;

            if (txtCodVeiculo.Text != "Novo")
                CodVeiculo = Convert.ToInt64(txtCodVeiculo.Text);

            if (ddlCatVeiculo.SelectedValue != "..... SELECIONE UMA CATEGORIA DO VEÍCULO .....")
                CodTipo = Convert.ToInt32(ddlCatVeiculo.SelectedValue);


            Veiculo x1 = new Veiculo(CodVeiculo,
                                     txtPlaca.Text,
                                     txtDscVeiculo.Text,
                                     CodTipo);

            listCadVeiculo = new List<Veiculo>();
            listCadVeiculo.Add(x1);
            Session["IncVeiculoCategoria"] = listCadVeiculo;
            Session["ZoomVeiculoCategoria2"] = "RELACIONAL";
            Response.Redirect("~/Pages/Veiculos/CadCatVeiculo.aspx");

        }
    }
}