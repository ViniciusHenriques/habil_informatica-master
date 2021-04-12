using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class ConTiposSituacao2 : System.Web.UI.Page
    {

        clsMensagem cls = new clsMensagem();
        
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["Operacao"].ToString() == "CONSULTA") {
                    return;
                }


                if (!Page.IsPostBack)
                {
                    Session["ZoomHabilTipo"] = "³";

                    DBTabelaDAL d = new DBTabelaDAL();
                    Habil_TipoDAL r = new Habil_TipoDAL();

                    ddlPesquisa.DataSource = d.ListarCamposXML("HABIL_TIPO");
                    ddlPesquisa.DataTextField = "Coluna";
                    ddlPesquisa.DataValueField = "Coluna";
                    ddlPesquisa.DataBind();

                    grdSituacao.DataSource = r.ListarHabilTipos("", "", "", "");
                    grdSituacao.DataBind();
                    if (grdSituacao.Rows.Count == 0)
                         Response.Write(cls.ExibeMensagem("Tipos e Situação Não Encontrados"));
                }



            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            Habil_TipoDAL r = new Habil_TipoDAL();
            DBTabelaDAL d = new DBTabelaDAL();

            String strTipo = d.ListarTipoCampoXML("HABIL_TIPO", ddlPesquisa.SelectedValue).ToString().ToUpper();
            string strValor = txtVarchar.Text + txtInt.Text + txtValor.Text;
            grdSituacao.DataSource = r.ListarHabilTipos(ddlPesquisa.Text, strTipo, strValor, ddlPesquisa.Text);
            grdSituacao.DataBind();
            if (grdSituacao.Rows.Count == 0)
                 Response.Write(cls.ExibeMensagem("Tipos e Situação Não Encontrados"));
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
                String strTipo = d.ListarTipoCampoXML("HABIL_SITUACAO", ddlPesquisa.SelectedValue).ToString().ToUpper();

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


        protected void grdSituacao_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdSituacao.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }

        protected void ddlRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdSituacao.PageSize = Convert.ToInt16(ddlRegistros.Text);
        }


        protected void btnNada_Click(object sender, EventArgs e)
        {

        }

        protected void grdSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtCodigo.Text = HttpUtility.HtmlDecode(grdSituacao.SelectedRow.Cells[0].Text);
            txtDescricao.Text = HttpUtility.HtmlDecode(grdSituacao.SelectedRow.Cells[1].Text);
            Session["ZoomHabilTipo"] = HttpUtility.HtmlDecode(grdSituacao.SelectedRow.Cells[0].Text)
                   + "²" + HttpUtility.HtmlDecode(grdSituacao.SelectedRow.Cells[1].Text);

        }

    }

}