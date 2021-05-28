using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
namespace SoftHabilInformatica.Pages.Transporte
{
    public partial class ManCidadesRegraFrete : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();

        List<CidadeRegraFrete> ListaCidades = new List<CidadeRegraFrete>();

        String strMensagemR = "";

        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected Boolean ValidaCampos()
        {
            try
            {
                if (ListaCidades.Count == 0)
                {
                    ShowMessage("Adicione cidades a regra", MessageType.Info);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                return false;
            }
        }

        protected void CarregaSituacoes()
        {
            try
            {
                EstadoDAL est = new EstadoDAL();
                ddlEstado.DataSource = est.ListarEstados("", "", "", "");
                ddlEstado.DataTextField = "Sigla";
                ddlEstado.DataValueField = "CodigoEstado";
                ddlEstado.DataBind();

                MunicipioDAL mun = new MunicipioDAL();
                ddlCidade.DataSource = mun.ListarMunicipios("M.CD_ESTADO", "SMALLINT", ddlEstado.SelectedValue, "");
                ddlCidade.DataTextField = "DescricaoMunicipio";
                ddlCidade.DataValueField = "CodigoMunicipio";
                ddlCidade.DataBind();
                ddlEstado.SelectedValue = "43";
                ddlEstado_SelectedIndexChanged(null, null);

                RegraFreteDAL p = new RegraFreteDAL();
                ddlTransportadoras.DataSource = p.ListarTransportadoresRegraFrete();
                ddlTransportadoras.DataTextField = "Cpl_ComboRegras";
                ddlTransportadoras.DataValueField = "CodigoTransportador";
                ddlTransportadoras.DataBind();
                ddlTransportadoras_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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

                if (!IsPostBack)
                {
                    CarregaSituacoes();
                }

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Transporte/ConRegFrete.aspx");
        }
        
        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEstado.SelectedValue != "")
            {
                MunicipioDAL mun = new MunicipioDAL();
                ddlCidade.DataSource = mun.ListarMunicipios("M.CD_ESTADO", "SMALLINT", ddlEstado.SelectedValue, "");
                ddlCidade.DataTextField = "DescricaoMunicipio";
                ddlCidade.DataValueField = "CodigoMunicipio";
                ddlCidade.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlCidade.SelectedValue != "")
            {
                CidadeRegraFreteDAL cityDAL = new CidadeRegraFreteDAL();
                RegraFrete reg = new RegraFrete();
                RegraFreteDAL regDAL = new RegraFreteDAL();

                cityDAL.InserirCidades(Convert.ToInt32(ddlCidade.SelectedValue), Convert.ToInt32(ddlRegioes.SelectedValue));

                ShowMessage(ddlCidade.SelectedItem.Text + " incluso na região " + ddlRegioes.SelectedItem.Text, MessageType.Info);
            }
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }

        protected void ddlTransportadoras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlTransportadoras.SelectedValue != "* Nenhum selecionado")
            {
                RegraFreteDAL reg = new RegraFreteDAL();
                ddlRegioes.DataSource = reg.ListarRegiãoRegraFrete(Convert.ToDecimal(ddlTransportadoras.SelectedValue));
                ddlRegioes.DataTextField = "Regiao";
                ddlRegioes.DataValueField = "CodigoIndex";
                ddlRegioes.DataBind();

            } 
        }
    }
}