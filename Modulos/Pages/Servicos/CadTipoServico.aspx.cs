using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
namespace SoftHabilInformatica.Pages.Servicos
    
{
    public partial class CadTipoServico : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        List<ItemTipoServico> ListaItemTipoServicoLocal = new List<ItemTipoServico>();
        List<TipoServico> listCadTipoServico = new List<TipoServico>();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected Boolean ValidaCampos()
        {

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }


            Boolean blnCampoValido = false;
            v.CampoValido("Valor ISSQN", TxtVlrISSQN.Text,true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    TxtVlrISSQN.Focus();

                }

                return false;
            }

            v.CampoValido("Descrição", txtDescricao.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDescricao.Focus();

                }

                return false;
            }
            v.CampoValido("Código CNAE", txtCodigoCNAE.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodigoCNAE.Focus();

                }

                return false;
            }
            v.CampoValido("Código servico Conforme lei 116", txtCodigoLei.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodigoLei.Focus();

                }

                return false;
            }



            return true;
        }
        protected void MontarValorTotal(List<ItemTipoServico> ListaItens)
        {

            decimal ValorTotal = 0;
            txtVlrTotal.Text = Convert.ToDouble("0,00").ToString("C");

            foreach (ItemTipoServico itens in ListaItens)
            {
                ValorTotal += (itens.Quantidade * itens.PrecoItem);

            }
            txtVlrTotal.Text = Convert.ToString(ValorTotal);
            txtVlrTotal.Text = Convert.ToDouble(txtVlrTotal.Text).ToString("C");

        }
        protected void LimparTelaItens()
        {
            txtPreco.Text = "0,00";
            txtQtde.Text = "0,00";
            ProdutoDAL ed = new ProdutoDAL();
            ddlProduto.DataSource = ed.ObterTiposServicos();
            ddlProduto.DataTextField = "DescricaoProduto";
            ddlProduto.DataValueField = "CodigoProduto";
            ddlProduto.DataBind();
            ddlProduto.Items.Insert(0, "..... SELECIONE UM PRODUTO.....");
        }
        protected void CarregaSituacoes()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            txtCodSituacao.DataSource = sd.Atividade();
            txtCodSituacao.DataTextField = "DescricaoTipo";
            txtCodSituacao.DataValueField = "CodigoTipo";
            txtCodSituacao.DataBind();

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["ZoomTipoServico2"] != null)
            {
                if (Session["ZoomTipoServico2"].ToString() == "RELACIONAL")
                {
                    pnlPainel.Visible = false;
                    cmdSair.Visible = false;
                }
                else
                {
                    pnlPainel.Visible = true;
                    cmdSair.Visible = true;
                }
            }
            else
            {
                pnlPainel.Visible = true;
                cmdSair.Visible = true;
            }

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();

            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                            "ConTipoServico.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomTipoServico2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomTipoServico"] != null)
                {
                    string s = Session["ZoomTipoServico"].ToString();
                    Session["ZoomTipoServico"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                TipoServicoDAL r = new TipoServicoDAL();
                                TipoServico p = new TipoServico();
                               
                                CarregaSituacoes();
                                p = r.PesquisarTipoServico(Convert.ToInt32(txtCodigo.Text));
                                txtCodigo.Text = p.CodigoTipoServico.ToString();                               
                                txtDescricao.Text = p.DescricaoTipoServico.ToString();
                                txtCodSituacao.SelectedValue = p.CodigoSituacao.ToString();
                                TxtVlrISSQN.Text = Convert.ToString(p.ValorISSQN);
                                txtCodigoCNAE.Text = Convert.ToString(p.CodigoCNAE);
                                txtCodigoLei.Text = Convert.ToString(p.CodigoServicoLei);
                                txtVlrTotal.Enabled = false;
              

                                ItemTipoServicoDAL pe2 = new ItemTipoServicoDAL();
                                ListaItemTipoServicoLocal = pe2.ObterItemTipoServico(Convert.ToInt32(txtCodigo.Text));
                                
                                //MCM - Olha como deveria ficar
                                MontarValorTotal(ListaItemTipoServicoLocal);
                                
                                Session["ListaItemTipoServico"] = ListaItemTipoServicoLocal;
                                grdItens.DataSource = ListaItemTipoServicoLocal;
                                grdItens.DataBind();

                                LimparTelaItens();

                                lista.ForEach(delegate (Permissao x)
                                {
                                    if (!x.AcessoCompleto)
                                    {
                                        if (!x.AcessoAlterar)
                                            btnSalvar.Visible = false;

                                        if (!x.AcessoExcluir)
                                            btnExcluir.Visible = false;
                                    }
                                });

                                return;
                            }
                    }
                }
                else
                {
                    LimpaCampos();
                                       
                    btnExcluir.Visible = false;
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                        }
                    });

                }
            }
            if(txtCodigo.Text == "")
                Response.Redirect("~/Pages/Servicos/ConTipoServico.aspx");
            
                       
            if (Session["ListaItemTipoServico"] != null)
            {
                ListaItemTipoServicoLocal = (List<ItemTipoServico>)Session["ListaItemTipoServico"];
                MontarValorTotal(ListaItemTipoServicoLocal);
                grdItens.DataSource = ListaItemTipoServicoLocal;
                grdItens.DataBind();               
            }                                  
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void LimpaCampos()
        {
            txtPreco.Text = "0,00";
            txtQtde.Text = "0,00";
            txtVlrTotal.Enabled = false;
            txtCodigo.Text = "Novo";
            txtCodigo.Enabled = false;
            txtDescricao.Text = "";
            TxtVlrISSQN.Text =  "0,00";
            txtCodigoLei.Text = "";
            txtVlrTotal.Text = Convert.ToDouble("0,00").ToString("C");
            txtCodigoCNAE.Text = "";
;
            CarregaSituacoes();
            LimparTelaItens();

            txtCodSituacao.SelectedValue = "1";
            Session["ListaItemTipoServico"] = null;

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    TipoServicoDAL d = new TipoServicoDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Tipo de Serviço excluído com Sucesso!!!";
                    
                    btnVoltar_Click(sender, e);
                    
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Tipo de Serviço não identificado.&emsp;&emsp;&emsp;";

            }
            catch (Exception ex)
            {
                strErro = "&emsp;&emsp;&emsp;" + ex.Message.ToString() + "&emsp;&emsp;&emsp;";
               
            }

            if (strErro != "")
            {
                lblMensagem.Text = strErro;
                pnlMensagem.Visible = true;
            }

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {

            Session["ZoomTipoServico"] = null;
            if (Session["ZoomTipoServico2"] != null)
            {
                Session["ZoomTipoServico2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadTipoServico.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Session["InscricaoTipoServico"] = null;
            
            listCadTipoServico = null;

            Response.Redirect("~/Pages/Servicos/ConTipoServico.aspx");
            

        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
            {
                return;
            }

            TipoServicoDAL d = new TipoServicoDAL();
            TipoServico p = new TipoServico();
            ItemTipoServico i = new ItemTipoServico();
            List<ItemTipoServico> ItensTipoServico = new List<ItemTipoServico>();

                                 
            p.CodigoSituacao = Convert.ToInt16(txtCodSituacao.SelectedValue);
            p.DescricaoTipoServico = txtDescricao.Text.ToUpper();
            p.ValorISSQN = Convert.ToDecimal(TxtVlrISSQN.Text);
            p.CodigoCNAE = Convert.ToInt64(txtCodigoCNAE.Text);
            p.CodigoServicoLei = Convert.ToDecimal(txtCodigoLei.Text);
            //txtCodigo.Text = p.codigoTipoServico.ToString();

            if (txtCodigo.Text == "Novo")
            {
                d.Inserir(p,ListaItemTipoServicoLocal);
                Session["MensagemTela"] = "Tipo de Serviço Incluído com Sucesso!!!";

            }
            else
            {
                p.CodigoTipoServico = Convert.ToInt32(txtCodigo.Text);
                d.Atualizar(p,ListaItemTipoServicoLocal);

                Session["MensagemTela"] = "Tipo de Serviço Alterado com Sucesso!!!";
            }

            btnVoltar_Click(sender, e);
            

        }
        protected void BtnAddProduto_Click(object sender, EventArgs e)
        {
            int intEndItem = 0;
            ItemTipoServicoDAL r = new ItemTipoServicoDAL();
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();

            
            if (ValidaCamposItemTipoServico() == false)
                return;
           

            if (ListaItemTipoServicoLocal.Count != 0)
                intEndItem = Convert.ToInt32(ListaItemTipoServicoLocal.Max(x => x.CodigoItemTipoServico).ToString());

            intEndItem = intEndItem + 1;

            ItemTipoServico ListaItem = new ItemTipoServico(intEndItem,0,
                                                            Convert.ToDecimal(txtQtde.Text),
                                                            Convert.ToDecimal(txtPreco.Text),
                                                            ddlProduto.SelectedItem.Text, 
                                                            Convert.ToInt32(ddlProduto.SelectedValue),0,0);

            if (intEndItem != 0)
                ListaItemTipoServicoLocal.RemoveAll(x => x.CodigoItemTipoServico == intEndItem);

            ListaItemTipoServicoLocal.Add(ListaItem);

            Session["ListaItemTipoServico"] = ListaItemTipoServicoLocal;
            grdItens.DataSource = ListaItemTipoServicoLocal;
            grdItens.DataBind();

            MontarValorTotal(ListaItemTipoServicoLocal);
            LimparTelaItens();


        }
        protected void grdItens_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ItemTipoServico> ListaItemTipoServico = new List<ItemTipoServico>();
            List<ItemTipoServico> ListaItemTipo= new List<ItemTipoServico>();

            if (Session["ListaItemTipoServico"] != null)
                ListaItemTipoServico = (List<ItemTipoServico>)Session["ListaItemTipoServico"];
            else
                ListaItemTipoServico = new List<ItemTipoServico>();

            ItemTipoServico tabi;
            foreach (ItemTipoServico item in ListaItemTipoServico)
            {               
                if (item.CodigoItemTipoServico != Convert.ToInt32(HttpUtility.HtmlDecode(grdItens.SelectedRow.Cells[0].Text)))
                {                
                    tabi = new ItemTipoServico();
                    tabi.CodigoItemTipoServico = item.CodigoItemTipoServico;
                    tabi.Cpl_DscProduto = item.Cpl_DscProduto;
                    tabi.CodigoProduto = item.CodigoProduto;
                    tabi.Quantidade = item.Quantidade;
                    tabi.PrecoItem = item.PrecoItem;
                    ListaItemTipo.Add(tabi);
                }
            }

            MontarValorTotal(ListaItemTipo);
            grdItens.DataSource = ListaItemTipo;
            grdItens.DataBind();
            Session["ListaItemTipoServico"] = ListaItemTipo;
        }
        protected Boolean ValidaCamposItemTipoServico()
        {

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }


            if ((ddlProduto.Text == "..... SELECIONE UM PRODUTO....."))
            {
                ShowMessage("Escolha um Produto", MessageType.Info);
                ddlProduto.Focus();
                return false;
            }

            Boolean blnCampoValido = false;

            v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            
            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtQtde.Focus();

                }

                return false;
            }
            else
            {
                if (Convert.ToDouble(txtQtde.Text) == 0)
                {
                    ShowMessage("Digite a Quantidade!", MessageType.Info);
                    txtQtde.Focus();
                    return false;
                }
            }



            v.CampoValido("Preço", txtPreco.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);

            
            if (!blnCampoValido)
            {
                //txtRazSocial.Text = "";
                
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtPreco.Focus();

                }

                return false;
            }
            else
            {                
                if (Convert.ToDouble(txtPreco.Text) == 0)
                {
                    ShowMessage("Digite o preço!", MessageType.Info);
                    txtPreco.Focus();
                    return false;
                }
            }
            
            return true;
        }
    }

    


}