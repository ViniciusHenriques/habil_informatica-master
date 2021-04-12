using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.CEPs
{
    public partial class CadCEP : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        public string strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaLogradouro()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Logradouro", txtLogradouro.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                    ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }
            return true;
        }
        protected Boolean ValidaCampos()
        {
            if (!ValidaLogradouro()) return false;

            if (ddlMunicipio.Text == "..... SELECIONE UM MUNICÍPIO .....")
            {
                ShowMessage("Município deve ser Selecionado.", MessageType.Info);
                return false;
            }

            if (ddlBairro.Text == "..... SELECIONE UM BAIRRO .....")
            {
                ShowMessage("Bairro deve ser Selecionado.", MessageType.Info);
                return false;
            }
            return true;
        }
        protected void btnCorreios_Click(object sender, EventArgs e)
        {

            string _open = "window.open('http://www.buscacep.correios.com.br/sistemas/buscacep/');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
            return;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                btnSalvar.Visible = false;
                btnExcluir.Visible = false;
                txtcodcep.Focus();
            }

            if (Session["IncCEP"] != null)
            {
                txtcodcep.Text = "";
                txtLogradouro.Text = "";
                txtComplemento.Text = "";
                btnCancelar.Visible = true;
                btnSalvar.Visible = true;
                btnExcluir.Visible = false;
                txtcodcep.Enabled = false;
                btnNovo.Visible = false;
                pnlPanelCEP.Enabled = true;
                txtLogradouro.Text = "";
                txtComplemento.Text = "";
                txtLogradouro.Focus();

                EstadoDAL r1000 = new EstadoDAL();
                ddlEstado.Items.Clear();
                ddlEstado.DataSource = r1000.ListarEstados("", "", "", "");
                ddlEstado.DataTextField = "DescricaoEstado";
                ddlEstado.DataValueField = "CodigoEstado";
                ddlEstado.DataBind();
                ddlEstado_SelectedIndexChanged(sender, e);
                BairroDAL b1000 = new BairroDAL();
                ddlBairro.DataSource = b1000.ListarBairros("", "", "", "");
                ddlBairro.DataTextField = "DescricaoBairro";
                ddlBairro.DataValueField = "CodigoBairro";
                ddlBairro.DataBind();
                ddlMunicipio.Items.Clear();
                ddlBairro.Items.Insert(0, "..... SELECIONE UM BAIRRO .....");

                String strIncCEP = Session["IncCEP"].ToString();
                Session["IncCEP"] = null;
                
                string[] words = strIncCEP.Split('³');
                if (strIncCEP != "³")
                {
                    txtcodcep.Text = words[0];
                    txtLogradouro.Text = words[1];
                    txtComplemento.Text = words[2];
                    ddlEstado.SelectedValue = words[3];
                    ddlEstado_SelectedIndexChanged(sender, e);
                    ddlMunicipio.SelectedValue = words[4];

                }
                

            }
            

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {

            String strErro = "";
            try
            {
                if (txtcodcep.Text.Trim() != "")
                {
                    CEPDAL d = new CEPDAL();
                    d.Excluir(Convert.ToInt64(txtcodcep.Text.Replace("-", "").Replace(".","")));
                    ShowMessage("CEP Excluído com Sucesso.", MessageType.Success);

                    txtcodcep.Enabled = true;
                    txtcodcep.Focus();
                    txtcodcep.Text = "";
                    txtComplemento.Text = "";
                    txtLogradouro.Text = "";
                    btnNovo.Visible = false;
                    btnSalvar.Visible = false;
                    btnExcluir.Visible = false;
                    ddlEstado.Items.Clear();
                    ddlMunicipio.Items.Clear();
                    ddlBairro.Items.Clear();
                    pnlPanelCEP.Enabled = false; 
                      

                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do CEP não identificado.&emsp;&emsp;&emsp;";

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
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {

                if (ValidaCampos() == false)
                    return;


                CEPDAL d = new CEPDAL();
                CEP p = new CEP();

                p.CodigoCEP = Convert.ToInt64(txtcodcep.Text.Replace("-", "").Replace(".", ""));
                p.Logradouro = txtLogradouro.Text;
                p.Complemento = txtComplemento.Text;
                p.CodigoEstado = Convert.ToInt32(ddlEstado.SelectedValue);
                p.CodigoMunicipio = Convert.ToInt64(ddlMunicipio.SelectedValue);
                p.CodigoBairro = Convert.ToInt32(ddlBairro.SelectedValue);

                if (!btnExcluir.Visible)
                {
                    
                    d.Inserir(p);
                    ShowMessage("Registro Incluído com Sucesso.", MessageType.Success);
                    
                }
                else
                {
                    d.Atualizar(p);
                    ShowMessage("Registro Alterado com Sucesso.", MessageType.Success);
                }

                btnCancelar.Visible = false;
                btnExcluir.Visible = true; 

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
        protected void txtcodcep_TextChanged(object sender, EventArgs e)
        {

            CEPDAL c = new CEPDAL();
            CEP c1 = new CEP();

            if (txtcodcep.Text == "")
                txtcodcep.Text = "0";

            c1 = c.PesquisarCEP(Convert.ToInt64(txtcodcep.Text.Replace("-", "").Replace(".", "")));

            if (c1 == null)
            {
                ShowMessage("CEP não cadastrado.", MessageType.Info);
                btnNovo.Visible = true;
                pnlPanelCEP.Enabled = false;
                btnExcluir.Visible = false;
                btnSalvar.Visible = false;

            }
            else 
            {
                EstadoDAL r1000 = new EstadoDAL();
                ddlEstado.Items.Clear();
                ddlEstado.DataSource = r1000.ListarEstados("", "", "", "");
                ddlEstado.DataTextField = "DescricaoEstado";
                ddlEstado.DataValueField = "CodigoEstado";
                ddlEstado.DataBind();
                BairroDAL b1000 = new BairroDAL();
                ddlBairro.DataSource = b1000.ListarBairros("", "", "", "");
                ddlBairro.DataTextField = "DescricaoBairro";
                ddlBairro.DataValueField = "CodigoBairro";
                ddlBairro.DataBind();
                ddlMunicipio.Items.Clear();

                txtLogradouro.Text = c1.Logradouro;
                txtComplemento.Text = c1.Complemento;

                ddlEstado.Text = c1.CodigoEstado.ToString();
                ddlEstado_SelectedIndexChanged(sender, e);
                ddlMunicipio.Text = c1.CodigoMunicipio.ToString();

                //verifica pelo valor
                ListItem li = ddlBairro.Items.FindByValue(c1.CodigoBairro.ToString());
                if (li != null)
                    ddlBairro.Text = c1.CodigoBairro.ToString();

                pnlPanelCEP.Enabled = true; 
                btnExcluir.Visible = true;
                btnSalvar.Visible = true;
                btnNovo.Visible = false; 
            }
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

            MunicipioDAL m1000 = new MunicipioDAL();

            ddlMunicipio.Items.Clear();

            ddlMunicipio.DataSource = m1000.ListarMunicipios("E.CD_ESTADO", "SMALLINT", ddlEstado.SelectedValue, "M.DS_MUNICIPIO");
            ddlMunicipio.DataTextField = "DescricaoMunicipio";
            ddlMunicipio.DataValueField = "CodigoMunicipio";
            ddlMunicipio.DataBind();

            ddlMunicipio.Items.Insert(0, "..... SELECIONE UM MUNICÍPIO .....");
        }
        protected void ddlBairro_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void ddlMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            btnCancelar.Visible = true;
            btnSalvar.Visible = true;
            btnExcluir.Visible = false;
            txtcodcep.Enabled = false;
            btnNovo.Visible = false; 
            pnlPanelCEP.Enabled = true;
            txtLogradouro.Text = "";
            txtComplemento.Text = "";
            txtLogradouro.Focus();

            EstadoDAL r1000 = new EstadoDAL();
            ddlEstado.Items.Clear();
            ddlEstado.DataSource = r1000.ListarEstados("", "", "", "");
            ddlEstado.DataTextField = "DescricaoEstado";
            ddlEstado.DataValueField = "CodigoEstado";
            ddlEstado.DataBind();
            ddlEstado_SelectedIndexChanged(sender, e);
            BairroDAL b1000 = new BairroDAL();
            ddlBairro.DataSource = b1000.ListarBairros("", "", "", "");
            ddlBairro.DataTextField = "DescricaoBairro";
            ddlBairro.DataValueField = "CodigoBairro";
            ddlBairro.DataBind();
            

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            btnCancelar.Visible = false;
            btnSalvar.Visible = false ;
            btnExcluir.Visible =false ;
            pnlPanelCEP.Enabled = false;
            btnNovo.Visible = true;
            txtcodcep.Enabled = true;
            txtcodcep.Focus(); 
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
       }
        protected void btnAddBairro_Click(object sender, EventArgs e)
        {
            Session["IncCEP"] = txtcodcep.Text + "³" + txtLogradouro.Text + "³" + txtComplemento.Text + "³" + ddlEstado.SelectedValue + "³" + ddlMunicipio.SelectedValue;   
            Response.Redirect("~/Pages/CEPs/CadBairro.aspx?cad=1");
        }
    }
}