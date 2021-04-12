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
    public partial class DigInventario : System.Web.UI.Page
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
            Session["ZoomAtualizaInv"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[1].Text);            

            pnlGrid.Visible = false;
            pnlRel.Visible = true;
            btnVoltar.Visible = true;
            btnSalvar.Visible = false;
            btnSair.Visible = false;
            ddlContagem.Visible = true;

            Session["ddlContagem"] = HttpUtility.HtmlDecode(grdGrid.SelectedRow.Cells[6].Text);
            MontaDll(null, null);
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {           
            InventarioDAL RnInventarioDAL = new InventarioDAL();
            decimal decIndice = 0;
            decimal decQtContagem = 0;
            short QtContagem = 0;
            Boolean blnCampoValido = false;
            ItemDoInventario p;

            if (ddlContagem.SelectedValue == "Selecione uma Contagem")
            {
                ShowMessage("Selecione uma Contagem", MessageType.Info);
                ddlContagem.Focus();
                return;
            }
            foreach (GridViewRow row in grdQtGrid.Rows)
            {
                TextBox txtQtMov1 = (TextBox)row.FindControl("txtQtMov1");
                CheckBox chkZerar = (CheckBox)row.FindControl("chkZerar");

                v.CampoValido("Quantidade", txtQtMov1.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    txtQtMov1.Text = "0";

                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtQtMov1.Focus();
                    }
                    btnVoltar.Visible = true;
                    btnSalvar.Visible = true;
                    return;
                }
                decIndice = Convert.ToDecimal(row.Cells[7].Text);                
                decQtContagem = Convert.ToDecimal(txtQtMov1.Text);
                QtContagem = Convert.ToInt16(Session["ddlContagem"]);
 
                RnInventarioDAL.AtualizaContagem(decIndice, Convert.ToInt16(ddlContagem.SelectedItem.Text), decQtContagem, chkZerar.Checked);
                RnInventarioDAL.PesquisaEAtualizaInventario(decIndice, QtContagem , Convert.ToInt16(ddlContagem.SelectedItem.Text), decQtContagem);
                p = new ItemDoInventario();

                decIndice = 0;
            }
            ShowMessage("Digitação do Inventario Atualizadas com Sucesso.", MessageType.Success);
            CarregaBotoesSalvar();
        }
        protected void CarregaBotoesSalvar()
        {
            btnVoltar.Visible = true;
            btnSalvar.Visible = true;
            ddlContagem.Visible = false;
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
            ddlContagem.Visible = false;
        }
        private void MontaGridAtualiza(decimal decInventario)
        {
            int Con = 0;
            InventarioDAL RnInventarioDAL = new InventarioDAL();
            List<ItemDoInventario> ItInv = new List<ItemDoInventario>();
            if (ddlContagem.SelectedValue != "Selecione uma Contagem")
                Con = Convert.ToInt32(ddlContagem.SelectedValue);
            else
                Con = 1;

            ItInv = RnInventarioDAL.ListarItemInventario(decInventario, Con).ToList();
            var itemGrid = ItInv.OrderBy(x => x.CodigoLocalizacao).ThenBy(x => x.NomeProduto).ToList();
            grdQtGrid.DataSource = itemGrid;
            grdQtGrid.Columns[5].HeaderText = "Quantidade " + ddlContagem.SelectedValue + "ª Contagem";
            grdQtGrid.DataBind();
            
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            
                Session["ZoomAtualizaInv"] = 0;
                MontaGridAtualiza(Convert.ToDecimal(Session["ZoomAtualizaInv"]));
            
            
            pnlGrid.Visible = true;
            pnlRel.Visible = false;
            btnVoltar.Visible = false;
            btnSalvar.Visible = false;
            btnSair.Visible = true;
            ddlContagem.Visible = false;
        }
        protected void MontaDll(object sender, EventArgs e)
        {
            ddlContagem.Items.Clear();
            ddlContagem.Items.Insert(0, "Selecione uma Contagem");
            for (int i = 1; i < Convert.ToInt16(Session["ddlContagem"]) + 1; i++)
            {
                ddlContagem.Items.Insert(i, i.ToString());
            }
        }
        protected void ddlContagem_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSalvar.Visible = true;
            btnVoltar.Visible = true;
            MontaGridAtualiza(Convert.ToDecimal(Session["ZoomAtualizaInv"]));
        }
    }
}