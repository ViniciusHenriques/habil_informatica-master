using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.IO;
using System.Windows.Forms;

namespace SoftHabilInformatica.Pages.Estoque
{
    public partial class CanInventario : System.Web.UI.Page
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
        protected void grdGrid_SelectedIndexChanged(object sender, GridViewCommandEventArgs e)
        {

            string x = e.CommandName;

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = grdGrid.Rows[index];
            string Codigo = Server.HtmlDecode(row.Cells[1].Text);
            Session["ZoomindiceInv"] = Codigo;

            ShowPopupCancelar(sender, e);
        }
        protected void ShowPopupCancelar(object sender, EventArgs e)
        {
            string title = "Atenção";
            string body = "Deseja Cancelar o Inventário. Indice: " + Session["ZoomindiceInv"].ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
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
            
        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            //string strErro = "";
            //try
            //{
            //    decIndice = Convert.ToDecimal(row.Cells[7].Text);

            //    if (txtLancamento.Text.Trim() != "")
            //    {
            //        InventarioDAL d = new InventarioDAL();
            //        d.Excluir(Convert.ToInt16(txtLancamento.Text));
            //        Session["MensagemTela"] = "Registro Excluído com Sucesso!!!";
            //        btnVoltar_Click(sender, e);
            //    }
            //    else
            //        strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Estoque não identificado.&emsp;&emsp;&emsp;";

            //}
            //catch (Exception ex)
            //{
            //    strErro = "&emsp;&emsp;&emsp;" + ex.Message.ToString() + "&emsp;&emsp;&emsp;";
            //}

            //if (strErro != "")
            //{
            //    lblMensagem.Text = strErro;
            //    pnlMensagem.Visible = true;
            //}
        }

        protected void btnSimCancelar_Click(object sender, EventArgs e)
        {
            InventarioDAL RnInvenario = new InventarioDAL();



            bool blnResultado = RnInvenario.CancelarInventario(Convert.ToInt32(Session["ZoomindiceInv"].ToString()), Convert.ToInt32(Session["CodUsuario"].ToString()));

            if (blnResultado)
                ShowMessage("Inventário Cancelado com Sucesso !!!",MessageType.Success);
            else
                ShowMessage("Erro ao cancelar Inventário. ", MessageType.Error);

            CarregaDados();
            UpdatePanel2.Update();
        }
    }
}