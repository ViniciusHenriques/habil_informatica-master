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
    public partial class EncInventario : System.Web.UI.Page
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

            ShowPopupEncerrar(sender, e);
        }
        protected void ShowPopupEncerrar(object sender, EventArgs e)
        {
            string title = "Atenção";
            string body = "Deseja Encerrar o Inventário. Indice: " + Session["ZoomindiceInv"].ToString();
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

        protected void btnSimEncerrar_Click(object sender, EventArgs e)
        {
            List<ItemDoInventario> lista = new List<ItemDoInventario>();
            InventarioDAL RnInventario = new InventarioDAL();
            DBTabelaDAL dbTDAL = new DBTabelaDAL();
            lista = RnInventario.ListarItemInventarioMan(Convert.ToDecimal(Session["ZoomindiceInv"]));

            foreach (var item in lista)
            {
                int intCodTp = 0;

                EstoqueProduto p = new EstoqueProduto();
                EstoqueProdutoDAL Rn_Ep = new EstoqueProdutoDAL();

                p = Rn_Ep.PesquisarEstoqueParaInventario(item.CodigoIndiceEstoque);

                MovimentacaoInterna ep = new MovimentacaoInterna();
                MovimentacaoInterna ep2 = new MovimentacaoInterna();
                MovimentacaoInternaDAL d = new MovimentacaoInternaDAL();
                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                TipoOperacao TpOper = new TipoOperacao();
                TipoOperacaoDAL RnTpOper = new TipoOperacaoDAL();

                RnTpOper.TipoAjusteInventario(p.CodigoEmpresa, ref intCodTp);

                ep2 = d.LerSaldoAnterior(p.CodigoEmpresa, p.CodigoIndiceLocalizacao, p.CodigoProduto, p.CodigoLote);

                ep.ValorUnitario = ep2.ValorUnitario;
                ep.VlSaldoAjuste = item.QtInventario;
                ep.ValorSaldoAnterior = 0;
                ep.QtMovimentada = 0;
                ep.NumeroDoc = " INV [" + item.CodigoIndiceInventario + "]" + Convert.ToString(dbTDAL.ObterDataHoraServidor().ToString("ddMMyyyy"));
                ep.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"].ToString());
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());
                if (he != null)
                {
                    ep.CodigoMaquina = he.CodigoEstacao;
                }
                ep.CodigoTipoOperacao = intCodTp;
                ep.CodigoEmpresa = p.CodigoEmpresa;
                ep.CodigoIndiceLocalizacao = p.CodigoIndiceLocalizacao;
                ep.CodigoProduto = p.CodigoProduto;
                ep.CodigoLote = p.CodigoLote;
                ep.TpOperacao = "A";

                if(ep != null)
                    d.Inserir(ep);
                
            }
            bool blnResultado = RnInventario.EncerrarInventario(Convert.ToInt32(Session["ZoomindiceInv"].ToString()), Convert.ToInt32(Session["CodUsuario"].ToString()));

            if (blnResultado)
                ShowMessage("Inventário Encerrado com Sucesso !!!",MessageType.Success);
            else
                ShowMessage("Erro ao encerrar Inventário. ", MessageType.Error);

            CarregaDados();
            UpdatePanel2.Update();
        }
    }
}