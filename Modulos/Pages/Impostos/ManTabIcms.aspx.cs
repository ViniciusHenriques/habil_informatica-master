using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Persistence;
using DAL.Model;
using System.Collections.Generic;

namespace SoftHabilInformatica.Pages.Impostos
{
    public partial class ManTabIcms : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
//            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
//                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
                return;
            }

            if (!IsPostBack)
            {

                EstadoDAL ed = new EstadoDAL();
                ddlEstadoOrigem.DataSource = ed.ObterEstadosSiglaDaEmpresa();
                ddlEstadoOrigem.DataTextField = "DescricaoEstado";
                ddlEstadoOrigem.DataValueField = "CodigoEstado";
                ddlEstadoOrigem.DataBind();
                ddlEstadoOrigem.Items.Insert(0, "..... SELECIONE UM ESTADO DE ORIGEM .....");

                grdTabIcms.Visible = false;


            }


        }
        protected void grdPermissao_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void grdPermissao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void btnConfirma_Click(object sender, EventArgs e)
        {
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Boolean blnCampoValido = false;
            TabIcmsDAL r = new TabIcmsDAL();
            TabIcms p;

            foreach (GridViewRow row in grdTabIcms.Rows)
            {
                TextBox txtIcms = (TextBox)row.FindControl("txtIcmsInterno");
                TextBox txtIcms1 = (TextBox)row.FindControl("txtIcmsInterEstadual");
                TextBox txtIcms2 = (TextBox)row.FindControl("txtIcmsExterno");

                v.CampoValido("ICMS Interno", txtIcms.Text, true, true, false, true, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        strMensagemR += "<br/>";
                        strMensagemR += "Estado Origem: " + row.Cells[1].Text;
                        strMensagemR += "<br/>";
                        strMensagemR += "Estado Destino: " + row.Cells[2].Text;

                        ShowMessage(strMensagemR, MessageType.Error);
                        txtIcms.Focus();
                        return;
                    }
                }


                v.CampoValido("ICMS InterEstadual", txtIcms1.Text, true, true, false, true, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        strMensagemR += "<br/>";
                        strMensagemR += "Estado Origem: " + row.Cells[1].Text;
                        strMensagemR += "<br/>";
                        strMensagemR += "Estado Destino: " + row.Cells[2].Text;

                        ShowMessage(strMensagemR, MessageType.Error);

                        txtIcms1.Focus();
                        return;
                    }
                }

                v.CampoValido("ICMS Externo", txtIcms2.Text, true, true, false, true, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        strMensagemR += "<br/>";
                        strMensagemR += "Estado Origem: " + row.Cells[1].Text;
                        strMensagemR += "<br/>";
                        strMensagemR += "Estado Destino: " + row.Cells[2].Text;

                        ShowMessage(strMensagemR, MessageType.Error);
                        txtIcms2.Focus();
                        return;
                    }
                }

                p = new TabIcms();

                p.CodTabAliqIcms = Convert.ToInt64(row.Cells[0].Text);
                p.IcmsInterno = Convert.ToDouble(txtIcms.Text);
                p.IcmsInterEstadual = Convert.ToDouble(txtIcms1.Text);
                p.IcmsExterno = Convert.ToDouble(txtIcms2.Text);

                r.AtualizarAliquotas(p);
            }


            ShowMessage("Permissões Atualizadas com Sucesso.", MessageType.Success);
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }

        protected void btnProcurar_Click(object sender, EventArgs e)
        {
            if(ddlEstadoOrigem.SelectedValue == "..... SELECIONE UM ESTADO DE ORIGEM .....")
            {
                ShowMessage("Selecione um estado de origem", MessageType.Info);
                return;
            }
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();
            TabIcmsDAL r = new TabIcmsDAL();
            DBTabelaCampos x = new DBTabelaCampos();
            x.Filtro = "CD_EST_ORIGEM";
            x.Tipo = "smallint";
            x.Inicio = ddlEstadoOrigem.SelectedValue;
            x.Fim = ddlEstadoOrigem.SelectedValue;
            lista.Add(x);

            btnSalvar.Visible = false;
            grdTabIcms.Visible = true;   
            grdTabIcms.DataSource = r.ListarTabIcmsCompleto(lista);

            grdTabIcms.DataBind();
            if (grdTabIcms.Rows.Count == 0)
                ShowMessage("Tabela de ICMS não cadastrado.", MessageType.Info);
            else
                btnSalvar.Visible = true;

        }

    }
}