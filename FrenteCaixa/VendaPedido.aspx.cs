using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DAL.Persistence;
using DAL.Model;

namespace FrenteCaixa
{
    public partial class VendaPedido : System.Web.UI.Page
    {
        List<ProdutoPorCategoria> lista = new List<ProdutoPorCategoria>();
        List<ItemDoPedido> listaItens = new List<ItemDoPedido>();

        protected void Page_Load(object sender, EventArgs e)
        {


            if ((Session["UsuSisCaixa"] == null) || (Session["CodUsuarioCaixa"] == null))
            {
                ///Response.Redirect("http://localhost:49942/Default.aspx");
                Response.Redirect("http://localhost:1478/login.aspx");
                return;
            }
            lblNomefuncionario.Text = Session["UsuSisCaixa"].ToString();

        }

        protected void btnAguardar_Click(object sender, EventArgs e)
        {
        }

        protected void btnItens_Click(object sender, EventArgs e)
        {

        }

        protected void btnPagamento_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void btnSubGrupo1_Click(object sender, EventArgs e)
        {
            ProdutoPorCategoriaDAL X = new ProdutoPorCategoriaDAL();
            lista = X.ListarProdutosPorCategoria(0101010101);
            grdGrid.DataSource = lista;
            grdGrid.DataBind();

        }

        protected void btnSubGrupo2_Click(object sender, EventArgs e)
        {
            ProdutoPorCategoriaDAL X = new ProdutoPorCategoriaDAL();
            lista = X.ListarProdutosPorCategoria(0202020202);
            grdGrid.DataSource = lista;
            grdGrid.DataBind();

        }

        protected void btnSubGrupo2_Click1(object sender, EventArgs e)
        {
            ProdutoPorCategoriaDAL X = new ProdutoPorCategoriaDAL();
            lista = X.ListarProdutosPorCategoria(0303030303);
            grdGrid.DataSource = lista;
            grdGrid.DataBind();

        }

        protected void btnSubGrupo3_Click(object sender, EventArgs e)
        {
            ProdutoPorCategoriaDAL X = new ProdutoPorCategoriaDAL();
            lista = X.ListarProdutosPorCategoria(0404040404);
            grdGrid.DataSource = lista;
            grdGrid.DataBind();

        }

        protected void btnSubGrupo4_Click(object sender, EventArgs e)
        {
            ProdutoPorCategoriaDAL X = new ProdutoPorCategoriaDAL();
            lista = X.ListarProdutosPorCategoria(0505050505);
            grdGrid.DataSource = lista;
            grdGrid.DataBind();

        }

        protected void grdGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["ItensDoPedido"] != null)
                listaItens = (List<ItemDoPedido>)(Session["ItensDoPedido"]);

            ItemDoPedido p = new ItemDoPedido();
            p.CodigoProduto = 100;
            p.DescricaoProduto = "COCA PRA MIM";
            p.QtdeItem  = 1;
            p.ValorUnitario  = 10;
            p.ValorTotal  = 10;

            listaItens.Add(p);

            grdGridItens.DataSource = listaItens;
            grdGridItens.DataBind();

            Session["ItensDoPedido"] = listaItens; 
        }
    }
}