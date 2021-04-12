using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Security.Cryptography;

namespace GestaoInterna.PagesAdmin
{
    public partial class ConModSistema2 : System.Web.UI.Page
    {

        clsMensagem cls = new clsMensagem();
        
        protected void Page_Load(object sender, EventArgs e)
        {

             clsHash clsh = new clsHash(SHA512.Create());
             string teste =  clsh.CriptografarSenha("lucas");
             teste = "";
            
            try
            {
                if (Session["Operacao"].ToString() == "CONSULTA") {
                    return;
                }


                if (!Page.IsPostBack)
                {
                    Session["ZoomModSistema"] = "³";

                    DBTabelaDAL d = new DBTabelaDAL();
                    ModuloSistemaDAL r = new ModuloSistemaDAL();

                    ddlPesquisa.DataSource = d.ListarCamposXML("MODULO_DO_SISTEMA");
                    ddlPesquisa.DataTextField = "Coluna";
                    ddlPesquisa.DataValueField = "Coluna";
                    ddlPesquisa.DataBind();

                    grdModSistema.DataSource = r.ListarModulosSistema("", "", "", "");
                    grdModSistema.DataBind();
                    if (grdModSistema.Rows.Count == 0)
                         Response.Write(cls.ExibeMensagem("Módulos do Sistema Não Encontrados"));
                }



            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            ModuloSistemaDAL r = new ModuloSistemaDAL();
            DBTabelaDAL d = new DBTabelaDAL();

            String strTipo = d.ListarTipoCampoXML("MODULO_DO_SISTEMA", ddlPesquisa.SelectedValue).ToString().ToUpper();
            string strValor = txtVarchar.Text + txtInt.Text + txtValor.Text;
            grdModSistema.DataSource = r.ListarModulosSistema(ddlPesquisa.Text, strTipo, strValor, ddlPesquisa.Text);
            grdModSistema.DataBind();
            if (grdModSistema.Rows.Count == 0)
                 Response.Write(cls.ExibeMensagem("Módulos do Sistema Não Encontrados"));
            txtCodigo.Text = "";
            txtDescricao.Text = "";
        }

        protected void cboSelecionar(object sender, EventArgs e)
        {
            try
            {
                txtCodigo.Text = "";
                txtDescricao.Text = "";

                txtVarchar.Visible = false;
                txtInt.Visible = false;
                txtValor.Visible = false;

                DBTabelaDAL d = new DBTabelaDAL();
                String strTipo = d.ListarTipoCampoXML("MODULO_DO_SISTEMA", ddlPesquisa.SelectedValue).ToString().ToUpper();

                switch (strTipo)
                {
                    case "VARCHAR":
                        txtVarchar.Visible = true;
                        txtVarchar.Text = "";
                        txtVarchar.Focus();
                        break;

                    case "INT":
                        txtInt.Visible = true;
                        txtInt.Text = "";
                        txtInt.Focus();
                        break;

                    case "NUMERIC":
                        txtValor.Visible = true;
                        txtValor.Text = "";
                        txtValor.Focus();
                        break;

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }


        protected void grdModSistema_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdModSistema.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }

        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdModSistema.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }


        protected void btnNada_Click(object sender, EventArgs e)
        {

        }

        protected void grdModSistema_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtCodigo.Text = HttpUtility.HtmlDecode(grdModSistema.SelectedRow.Cells[0].Text);
            txtDescricao.Text = HttpUtility.HtmlDecode(grdModSistema.SelectedRow.Cells[1].Text);
            Session["ZoomModSistema"] = HttpUtility.HtmlDecode(grdModSistema.SelectedRow.Cells[0].Text) 
                   + "²" + HttpUtility.HtmlDecode(grdModSistema.SelectedRow.Cells[1].Text);

        }

    }

}