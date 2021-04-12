using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Persistence;
using DAL.Model;

namespace HabilInformatica
{
    public partial class Selecao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["CodUsuario"] != null)
                if (Session["CodUsuario"].ToString() == "-150380")
                    Response.Redirect("~/Pages/Welcome.aspx");


            Session["CodigoModuloEspecifico"] = null;

            List<ModuloSistema> MS = new List<ModuloSistema>();
            ModuloSistemaDAL m = new ModuloSistemaDAL();

            if (Session["CodPflUsuario"] != null)
            {
                MS = m.ListarModulosPermitidos(Session["CodPflUsuario"].ToString());
                PerfilUsuario pu = new PerfilUsuario();
                PerfilUsuarioDAL puDAL = new PerfilUsuarioDAL();
                pu = puDAL.PesquisarPerfilUsuario(Convert.ToInt64(Session["CodPflUsuario"]));

                if (pu != null && pu.CodigoModuloEspecifico != 0 )
                {
                    if (MS.Where(x => x.CodigoModulo == pu.CodigoModuloEspecifico).ToList().Count > 0)
                    {
                        Session["CodigoModuloEspecifico"] = pu.CodigoModuloEspecifico.ToString();
                        Session["CodModulo"] = pu.CodigoModuloEspecifico.ToString();
                        Session["DesModulo"] = MS.Where(x => x.CodigoModulo == pu.CodigoModuloEspecifico).ToList()[0].DescricaoModulo;
                        Response.Redirect("~/Pages/Welcome.aspx");
                    }
                    else
                    {
                        Session["MensagemTela"] = "Usuário sem permissão ao módulo específico selecionado";
                        Response.Redirect("~/Default.aspx");
                    }
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }


            if(MS.Count() == 0)
            {
                Session["MensagemTela"] = "Usuário sem permissão a qualquer módulo, entre em contato com um administrador";
                Response.Redirect("~/Default.aspx");
            }

            grdConsulta.DataSource = MS;
            grdConsulta.DataBind();

        }

        protected void grdConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CodModulo"] = HttpUtility.HtmlDecode(grdConsulta.SelectedRow.Cells[2].Text);
            Session["DesModulo"] = HttpUtility.HtmlDecode(grdConsulta.SelectedRow.Cells[1].Text);

            if (Convert.ToInt32(Session["CodModulo"].ToString()) > 1 && Convert.ToInt32(Session["CodModulo"].ToString()) < 9)
            {
                TipoDocumentoDAL t = new TipoDocumentoDAL();
                t.LiberaTipoDocumento(Convert.ToInt32(Session["CodModulo"].ToString()));
            }

            if (HttpUtility.HtmlDecode(grdConsulta.SelectedRow.Cells[2].Text).ToUpper().Equals ( "GESTÃO DE CAIXA"))
                Response.Redirect("http://localhost:1478/Login.aspx");
            else
                Response.Redirect("~/Pages/Welcome.aspx");
        }

        protected void grdConsulta_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}