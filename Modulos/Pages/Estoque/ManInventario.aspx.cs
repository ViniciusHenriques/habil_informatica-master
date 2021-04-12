using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class ManInventario : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocada"] != null)
                PanelSelect = Session["TabFocada"].ToString();
            else

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "RelParaInventario.aspx");
                lista.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {

                    }
                });
            }
            CarregaDados();
        }
        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ZoomAtualizaQtInv"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[1].Text);

            pnlGrid.Visible = false;
            pnlRel.Visible = true;
            btnSair.Visible = false;
            btnSalvar.Visible = true;
            btnVoltar.Visible = true;
            MontaGridAtualiza(Convert.ToDecimal(Session["ZoomAtualizaQtInv"]));
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            InventarioDAL RnInventarioDAL = new InventarioDAL();
            decimal decIndice = 0;
            decimal decQtContagem = 0;
            Boolean blnCampoValido = false;

            foreach (GridViewRow row in grdQtGrid.Rows)
            {
                TextBox txtQtInv = (TextBox)row.FindControl("txtQtInv");

                v.CampoValido("Quantidade", txtQtInv.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtQtInv.Text = "0";

                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtQtInv.Focus();
                    }
                    CarregaBotoesSalvar();
                    return;
                }

                decIndice = Convert.ToDecimal(row.Cells[6].Text);

                decQtContagem = Convert.ToDecimal(txtQtInv.Text);

                RnInventarioDAL.AtualizaQtInventario(decIndice, decQtContagem);
            }
            ShowMessage("Digitação do Inventário Atualizadas com Sucesso.", MessageType.Success);
            CarregaBotoesSalvar();
        }
        protected void CarregaBotoesSalvar()
        {
            btnVoltar.Visible = true;
            btnSalvar.Visible = true;
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void CarregaDados()
        {
            InventarioDAL ep = new InventarioDAL();
            grdGrid.DataSource = ep.ListarGrid();
            grdGrid.DataBind();
            btnVoltar.Visible = false;
            btnSalvar.Visible = false;
        }
        private void MontaGridAtualiza(decimal decInventario)
        {
            InventarioDAL RnInventarioDAL = new InventarioDAL();

            grdQtGrid.DataSource = RnInventarioDAL.ListarItemInventarioMan(decInventario).
                OrderBy(x => x.CodigoLocalizacao).ThenBy(x => x.CodigoProduto).ToList();
            grdQtGrid.DataBind();
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomAtualizaQtInv"] = 0;
            MontaGridAtualiza(Convert.ToDecimal(Session["ZoomAtualizaQtInv"]));

            pnlGrid.Visible = true;
            pnlRel.Visible = false;
            btnVoltar.Visible = false;
            btnSalvar.Visible = false;
            btnSair.Visible = true;
        }
    }
}