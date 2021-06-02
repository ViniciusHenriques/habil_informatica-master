using System;
using System.Windows;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Linq;
using NFSeX;
using System.Data;
using System.ComponentModel;


namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class ManItemOrcamento : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }

        public decimal[] ValoresDep1 = new decimal[] { 0, 0, 0, 0, 0, 0 };
        public decimal[] ValoresDep2 = new decimal[] { 0, 0, 0, 0, 0, 0 };
        public decimal[] ValoresDep3 = new decimal[] { 0, 0, 0, 0, 0, 0 };
        public decimal[] ValoresDep4 = new decimal[] { 0, 0, 0, 0, 0, 0 };
        public decimal[] ValoresDep5 = new decimal[] { 0, 0, 0, 0, 0, 0 };
        public decimal[] ValoresDep6 = new decimal[] { 0, 0, 0, 0, 0, 0 };
        public string[] Departamentos = new string[6] {"","","","","",""};
        public string[] MesesAnteriores;

        List<BIConsumoClienteProduto> ListaBiConsumoDep1, ListaBiConsumoDep2, ListaBiConsumoDep3, ListaBiConsumoDep4, ListaBiConsumoDep5, ListaBiConsumoDep6;

        List<ProdutoDocumento> ListaItemDocumento = new List<ProdutoDocumento>();

        List<BIConsumoClienteProduto> ListaBIConsumoClienteProduto = new List<BIConsumoClienteProduto>();

        clsValidacao v = new clsValidacao();

        String strMensagemR = "";

        public enum MessageType { Success, Error, Info, Warning };

        protected void CarregaTiposSituacoes()
        {
            UnidadeDAL RnUnidade = new UnidadeDAL();
            ddlUnidade.DataSource = RnUnidade.ListarUnidades("", "", "", "");
            ddlUnidade.DataTextField = "SiglaUnidade";
            ddlUnidade.DataValueField = "CodigoUnidade";
            ddlUnidade.DataBind();
        }       

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void LimpaTela()
        {
            txtCodItem.Text = "";
            txtDescricao.Text = "";
            txtPreco.Text = "0,00";
            txtQtde.Text = "0,00";
            txtVlrTotalItem.Text = "0,00";
            txtDesconto.Text = "0,00";
        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código Item", txtCodItem.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodItem.Focus();
                }
                return false;
            }
            Produto prod = new Produto();
            ProdutoDAL prodDAL = new ProdutoDAL();
            prod = prodDAL.PesquisarProduto(Convert.ToInt64(txtCodItem.Text));
            if(prod == null)
            {
                ShowMessage("Este Produto não existe!", MessageType.Info);
                return false;
            }

            v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                    return;

                if (Session["MensagemTela"] != null)
                {
                    ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                    Session["MensagemTela"] = null;
                }
                if (Session["TabFocada"] != null)
                {
                    PanelSelect = Session["TabFocada"].ToString();
                }
                else
                {
                    PanelSelect = "home";
                    Session["TabFocada"] = "home";
                }

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    CarregaTiposSituacoes();
                    if (Session["ZoomProduto2"] == null)
                        Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomProduto"] != null)
                    {
                        string s = Session["ZoomProduto"].ToString();
                        Session["ZoomProduto"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            foreach (string word in words)
                            {
                                if (word != "")
                                {
                                    txtCodItem.Text = word;
                                    txtCodItem_TextChanged(sender, e);
                                }
                            }
                        }
                    }
                    else
                    {
                        LimpaTela();
                    }

                    if (Session["Doc_orcamento"] != null)
                    {
                        MontaMesesAnteriores();

                        if (Session["ListaBIConsumoClienteProduto"] != null)
                        {
                            MontaGridsBiConsumoClienteProduto(true, true);
                        }
                        Doc_Orcamento doc = new Doc_Orcamento();
                        doc = (Doc_Orcamento)Session["Doc_orcamento"];


                        Pessoa p = new Pessoa();
                        PessoaDAL pDAL = new PessoaDAL();
                        p = pDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
                        lblProjecao.Text = " - Projeção de " + p.NumeroProjecao + " dias";
                    }
                    else if (Session["Doc_Pedido"] != null)
                    {
                        MontaMesesAnteriores();

                        if (Session["ListaBIConsumoClienteProduto"] != null)
                        {
                            MontaGridsBiConsumoClienteProduto(true, true);

                        }
                        Doc_Pedido doc = new Doc_Pedido();
                        doc = (Doc_Pedido)Session["Doc_Pedido"];

                        Pessoa p = new Pessoa();
                        PessoaDAL pDAL = new PessoaDAL();
                        p = pDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
                        lblProjecao.Text = " - Projeção de " + p.NumeroProjecao + " dias";
                    }
                    else
                    {
                        if (Request.QueryString["Cad"] == "1")
                        {
                            Response.Redirect("~/Pages/Vendas/ConOrcamento.aspx");
                        }
                        else if (Request.QueryString["Cad"] == "2")
                        {
                            Response.Redirect("~/Pages/Vendas/ConPedido.aspx");
                        }
                    }

                    if (Session["ListaItemDocumento"] != null)
                    {
                        ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemDocumento"];
                        ListaItemDocumento = ListaItemDocumento.OrderByDescending(x => x.CodigoItem).ToList();
                        Session["ListaItemDocumento"] = ListaItemDocumento;
                        grdProduto.DataSource = ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList();
                        grdProduto.DataBind();
                    }
                }
                else
                {
                    if (Session["ListaItemDocumento"] != null)
                    {
                        ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemDocumento"];
                        grdProduto.DataSource = ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList();
                        grdProduto.DataBind();
                    }
                    if (Session["ListaBIConsumoClienteProduto"] != null)
                    {
                        List<BIConsumoClienteProduto> ListaBIConsumo = new List<BIConsumoClienteProduto>();
                        ListaBIConsumo = (List<BIConsumoClienteProduto>)Session["ListaBIConsumoClienteProduto"];

                    }

                    if (Session["Doc_orcamento"] == null && Request.QueryString["Cad"] == "1")
                    {
                        Response.Redirect("~/Pages/Vendas/ConOrcamento.aspx");
                    }
                    else if (Session["Doc_Pedido"] == null && Request.QueryString["Cad"] == "2")
                    {
                        Response.Redirect("~/Pages/Vendas/ConPedido.aspx");
                    }

                    MontaMesesAnteriores();
                    MontaGridsBiConsumoClienteProduto(true, false);
                }

                MontarValorTotal(ListaItemDocumento);
                CamposProduto();
                if (!IsPostBack)
                    txtCodItem.Focus();
                if (ddlUnidade.Items.Count == 0)
                    btnVoltar_Click(sender, e);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ManutencaoProduto"] = null;
            if(Session["Doc_orcamento"] != null && Request.QueryString["Cad"] == "1")
                Response.Redirect("~/Pages/Vendas/ManOrcamento.aspx");
            else if (Session["Doc_Pedido"] != null && Request.QueryString["Cad"] == "2")
                Response.Redirect("~/Pages/Vendas/ManPedido.aspx");
        }

        protected void MontaGridsBiConsumoClienteProduto(bool AtualizaGrafico, bool AtualizaGrid)
        {
            List<BIConsumoClienteProduto> ListaBIConsumo = new List<BIConsumoClienteProduto>();
            ListaBIConsumo = (List<BIConsumoClienteProduto>)Session["ListaBIConsumoClienteProduto"];
            
            List<int> ListaDepartamentoExistente = new List<int>();// verifica quais os departamento diferentes existem na lista
            List<string> ListaDescricaoDepartamento = new List<string>();//lista a descricao dos departamentos presentes na lista anterior
            List<string> DepartamentoConcatenado = new List<string>();//concatena o codigo do departamento, a descrição e a quantidade existente do dep.

            if (ListaBIConsumo == null)
                return;
            for (int i = 0; i < ListaBIConsumo.Count; i++)//percorre a lista total, buscando departamento diferentes
            {
                ListaBIConsumo[i].strCpl_Mes1 = ListaBIConsumo[i].QuantidadeMes1 + " / R$" + ListaBIConsumo[i].ValorMes1;
                ListaBIConsumo[i].strCpl_Mes2 = ListaBIConsumo[i].QuantidadeMes2 + " / R$" + ListaBIConsumo[i].ValorMes2;
                ListaBIConsumo[i].strCpl_Mes3 = ListaBIConsumo[i].QuantidadeMes3 + " / R$" + ListaBIConsumo[i].ValorMes3;
                ListaBIConsumo[i].strCpl_Mes4 = ListaBIConsumo[i].QuantidadeMes4 + " / R$" + ListaBIConsumo[i].ValorMes4;
                ListaBIConsumo[i].strCpl_Mes5 = ListaBIConsumo[i].QuantidadeMes5 + " / R$" + ListaBIConsumo[i].ValorMes5;
                ListaBIConsumo[i].strCpl_Mes6 = ListaBIConsumo[i].QuantidadeMes6 + " / R$" + ListaBIConsumo[i].ValorMes6;

                int contador = 0;
                for (int s = 0; s < ListaDepartamentoExistente.Count(); s++)
                {
                    if (ListaBIConsumo[i].CodigoDepartamento == ListaDepartamentoExistente[s])
                    {
                        contador++;
                    }
                }
                if (contador == 0)
                {
                    ListaDepartamentoExistente.Add(ListaBIConsumo[i].CodigoDepartamento);
                    ListaDescricaoDepartamento.Add(ListaBIConsumo[i].Cpl_DescricaoDepartamento);
                }
            }

            for (int i = 0; i < ListaDepartamentoExistente.Count; i++)// concatena e adiciona na lista DepertamentoConcatenado
            {
                string strDep = ListaBIConsumo.Where(x => x.CodigoDepartamento == ListaDepartamentoExistente[i]).ToList().Count + "³" + ListaDepartamentoExistente[i] + "³" + ListaDescricaoDepartamento[i];
                DepartamentoConcatenado.Add(strDep);
            }

            DepartamentoConcatenado = DepartamentoConcatenado.OrderByDescending(y => y.ToString()).ToList();

            string t2 = "";
            for (int i = 0; i < DepartamentoConcatenado.Count; i++)
            {
                t2 += DepartamentoConcatenado[i] + " - ";
                if (i < 5)
                {
                    string[] splitDepartamentoConcatenado = DepartamentoConcatenado[i].ToString().Split('³');
                    Departamentos[i] = splitDepartamentoConcatenado[2].ToUpper().First() + splitDepartamentoConcatenado[2].Substring(1).ToLower();
                }
                else
                {
                    Departamentos[5] = "Outros";// se existir mais de 6 departamentos, os que tiverem menos ficarão juntos 
                }

            }
                //{
                // preenchendo o header do gridView
                grdBIConsumoPessoaProduto.Columns[3].HeaderText = MesesAnteriores[0];
                grdBIConsumoPessoaProduto.Columns[4].HeaderText = MesesAnteriores[1];
                grdBIConsumoPessoaProduto.Columns[5].HeaderText = MesesAnteriores[2];
                grdBIConsumoPessoaProduto.Columns[6].HeaderText = MesesAnteriores[3];
                grdBIConsumoPessoaProduto.Columns[7].HeaderText = MesesAnteriores[4];
                grdBIConsumoPessoaProduto.Columns[8].HeaderText = MesesAnteriores[5];

                grdBIConsumoPessoaProduto2.Columns[3].HeaderText = MesesAnteriores[0];
                grdBIConsumoPessoaProduto2.Columns[4].HeaderText = MesesAnteriores[1];
                grdBIConsumoPessoaProduto2.Columns[5].HeaderText = MesesAnteriores[2];
                grdBIConsumoPessoaProduto2.Columns[6].HeaderText = MesesAnteriores[3];
                grdBIConsumoPessoaProduto2.Columns[7].HeaderText = MesesAnteriores[4];
                grdBIConsumoPessoaProduto2.Columns[8].HeaderText = MesesAnteriores[5];

                grdBIConsumoPessoaProduto3.Columns[3].HeaderText = MesesAnteriores[0];
                grdBIConsumoPessoaProduto3.Columns[4].HeaderText = MesesAnteriores[1];
                grdBIConsumoPessoaProduto3.Columns[5].HeaderText = MesesAnteriores[2];
                grdBIConsumoPessoaProduto3.Columns[6].HeaderText = MesesAnteriores[3];
                grdBIConsumoPessoaProduto3.Columns[7].HeaderText = MesesAnteriores[4];
                grdBIConsumoPessoaProduto3.Columns[8].HeaderText = MesesAnteriores[5];

                grdBIConsumoPessoaProduto4.Columns[3].HeaderText = MesesAnteriores[0];
                grdBIConsumoPessoaProduto4.Columns[4].HeaderText = MesesAnteriores[1];
                grdBIConsumoPessoaProduto4.Columns[5].HeaderText = MesesAnteriores[2];
                grdBIConsumoPessoaProduto4.Columns[6].HeaderText = MesesAnteriores[3];
                grdBIConsumoPessoaProduto4.Columns[7].HeaderText = MesesAnteriores[4];
                grdBIConsumoPessoaProduto4.Columns[8].HeaderText = MesesAnteriores[5];

                grdBIConsumoPessoaProduto5.Columns[3].HeaderText = MesesAnteriores[0];
                grdBIConsumoPessoaProduto5.Columns[4].HeaderText = MesesAnteriores[1];
                grdBIConsumoPessoaProduto5.Columns[5].HeaderText = MesesAnteriores[2];
                grdBIConsumoPessoaProduto5.Columns[6].HeaderText = MesesAnteriores[3];
                grdBIConsumoPessoaProduto5.Columns[7].HeaderText = MesesAnteriores[4];
                grdBIConsumoPessoaProduto5.Columns[8].HeaderText = MesesAnteriores[5];

                grdBIConsumoPessoaProduto6.Columns[3].HeaderText = MesesAnteriores[0];
                grdBIConsumoPessoaProduto6.Columns[4].HeaderText = MesesAnteriores[1];
                grdBIConsumoPessoaProduto6.Columns[5].HeaderText = MesesAnteriores[2];
                grdBIConsumoPessoaProduto6.Columns[6].HeaderText = MesesAnteriores[3];
                grdBIConsumoPessoaProduto6.Columns[7].HeaderText = MesesAnteriores[4];
                grdBIConsumoPessoaProduto6.Columns[8].HeaderText = MesesAnteriores[5];
                //--------------------------------
            //}
            
        
            if (DepartamentoConcatenado.Count >= 1)
            { 
                ListaBiConsumoDep1 = ListaBIConsumo.Where(y => y.CodigoDepartamento.ToString() == DepartamentoConcatenado[0].ToString().Split('³')[1]).ToList();
                if(AtualizaGrafico)
                    for (int i = 0; i < ListaBiConsumoDep1.Count; i++)
                    {
                        ValoresDep1 = new decimal[]{
                            ValoresDep1[0] + ListaBiConsumoDep1[i].ValorMes1,
                            ValoresDep1[1] + ListaBiConsumoDep1[i].ValorMes2,
                            ValoresDep1[2] + ListaBiConsumoDep1[i].ValorMes3,
                            ValoresDep1[3] + ListaBiConsumoDep1[i].ValorMes4,
                            ValoresDep1[4] + ListaBiConsumoDep1[i].ValorMes5,
                            ValoresDep1[5] + ListaBiConsumoDep1[i].ValorMes6
                        };
                    }
                if (AtualizaGrid)
                {
                    grdBIConsumoPessoaProduto.DataSource = ListaBiConsumoDep1;
                    grdBIConsumoPessoaProduto.DataBind();
                }
            }// se existir pelo menos 1 dep;
            else
            {
                grdBIConsumoPessoaProduto.DataSource = null;
                grdBIConsumoPessoaProduto.DataBind();
            }
            if (DepartamentoConcatenado.Count >= 2)
            {
                ListaBiConsumoDep2 = ListaBIConsumo.Where(y => y.CodigoDepartamento.ToString() == DepartamentoConcatenado[1].ToString().Split('³')[1]).ToList();
                if (AtualizaGrafico)
                    for (int i = 0; i < ListaBiConsumoDep2.Count; i++)
                    {
                        ValoresDep2 = new decimal[]{
                            ValoresDep2[0] + ListaBiConsumoDep2[i].ValorMes1,
                            ValoresDep2[1] + ListaBiConsumoDep2[i].ValorMes2,
                            ValoresDep2[2] + ListaBiConsumoDep2[i].ValorMes3,
                            ValoresDep2[3] + ListaBiConsumoDep2[i].ValorMes4,
                            ValoresDep2[4] + ListaBiConsumoDep2[i].ValorMes5,
                            ValoresDep2[5] + ListaBiConsumoDep2[i].ValorMes6
                        };
                    }
                if (AtualizaGrid)
                {
                    grdBIConsumoPessoaProduto2.DataSource = ListaBiConsumoDep2;
                    grdBIConsumoPessoaProduto2.DataBind();
                }
            }// se existir pelo menos 2 dep;
            else
            {
                grdBIConsumoPessoaProduto2.DataSource = null;
                grdBIConsumoPessoaProduto2.DataBind();
            }
            if (DepartamentoConcatenado.Count >= 3)
            { 
                ListaBiConsumoDep3 = ListaBIConsumo.Where(y => y.CodigoDepartamento.ToString() == DepartamentoConcatenado[2].ToString().Split('³')[1]).ToList();
                if (AtualizaGrafico)
                    for (int i = 0; i < ListaBiConsumoDep3.Count; i++)
                    {
                        ValoresDep3 = new decimal[]{
                            ValoresDep3[0] + ListaBiConsumoDep3[i].ValorMes1,
                            ValoresDep3[1] + ListaBiConsumoDep3[i].ValorMes2,
                            ValoresDep3[2] + ListaBiConsumoDep3[i].ValorMes3,
                            ValoresDep3[3] + ListaBiConsumoDep3[i].ValorMes4,
                            ValoresDep3[4] + ListaBiConsumoDep3[i].ValorMes5,
                            ValoresDep3[5] + ListaBiConsumoDep3[i].ValorMes6
                        };
                    }
                if (AtualizaGrid)
                {
                    grdBIConsumoPessoaProduto3.DataSource = ListaBiConsumoDep3;
                    grdBIConsumoPessoaProduto3.DataBind();
                }
            }// se existir pelo menos 3 dep;
            else
            {
                grdBIConsumoPessoaProduto3.DataSource = null;
                grdBIConsumoPessoaProduto3.DataBind();
            }
            if (DepartamentoConcatenado.Count >= 4)
            {
                ListaBiConsumoDep4 = ListaBIConsumo.Where(y => y.CodigoDepartamento.ToString() == DepartamentoConcatenado[3].ToString().Split('³')[1]).ToList();
                if (AtualizaGrafico)
                    for (int i = 0; i < ListaBiConsumoDep4.Count; i++)
                    {
                        ValoresDep4 = new decimal[]{
                            ValoresDep4[0] + ListaBiConsumoDep4[i].ValorMes1,
                            ValoresDep4[1] + ListaBiConsumoDep4[i].ValorMes2,
                            ValoresDep4[2] + ListaBiConsumoDep4[i].ValorMes3,
                            ValoresDep4[3] + ListaBiConsumoDep4[i].ValorMes4,
                            ValoresDep4[4] + ListaBiConsumoDep4[i].ValorMes5,
                            ValoresDep4[5] + ListaBiConsumoDep4[i].ValorMes6
                        };
                    }
                if (AtualizaGrid)
                {
                    grdBIConsumoPessoaProduto4.DataSource = ListaBiConsumoDep4;
                    grdBIConsumoPessoaProduto4.DataBind();
                }
            }// se existir pelo menos 4 dep;
            else
            {
                grdBIConsumoPessoaProduto4.DataSource = null;
                grdBIConsumoPessoaProduto4.DataBind();
            }
            if (DepartamentoConcatenado.Count >= 5)
            {
                ListaBiConsumoDep5 = ListaBIConsumo.Where(y => y.CodigoDepartamento.ToString() == DepartamentoConcatenado[4].ToString().Split('³')[1]).ToList();
                if (AtualizaGrafico)
                    for (int i = 0; i < ListaBiConsumoDep5.Count; i++)
                    {
                        ValoresDep5 = new decimal[]{
                            ValoresDep5[0] + ListaBiConsumoDep5[i].ValorMes1,
                            ValoresDep5[1] + ListaBiConsumoDep5[i].ValorMes2,
                            ValoresDep5[2] + ListaBiConsumoDep5[i].ValorMes3,
                            ValoresDep5[3] + ListaBiConsumoDep5[i].ValorMes4,
                            ValoresDep5[4] + ListaBiConsumoDep5[i].ValorMes5,
                            ValoresDep5[5] + ListaBiConsumoDep5[i].ValorMes6
                        };
                    }
                if (AtualizaGrid)
                {
                    grdBIConsumoPessoaProduto5.DataSource = ListaBiConsumoDep5;
                    grdBIConsumoPessoaProduto5.DataBind();
                }
            }// se existir pelo menos 5 dep;
            else
            {
                grdBIConsumoPessoaProduto5.DataSource = null;
                grdBIConsumoPessoaProduto5.DataBind();
            }
            if (DepartamentoConcatenado.Count >= 6)
            {
                ListaBiConsumoDep6 = ListaBIConsumo.Where(y => y.CodigoDepartamento.ToString() != DepartamentoConcatenado[0].ToString().Split('³')[1] &&
                                                    y.CodigoDepartamento.ToString() != DepartamentoConcatenado[1].ToString().Split('³')[1] &&
                                                    y.CodigoDepartamento.ToString() != DepartamentoConcatenado[2].ToString().Split('³')[1] &&
                                                    y.CodigoDepartamento.ToString() != DepartamentoConcatenado[3].ToString().Split('³')[1] &&
                                                    y.CodigoDepartamento.ToString() != DepartamentoConcatenado[4].ToString().Split('³')[1]).ToList();

                for (int i = 0; i < ListaBiConsumoDep6.Count; i++)
                {
                    ValoresDep6 = new decimal[] {
                        ValoresDep6[0] + ListaBiConsumoDep6[i].ValorMes1,
                        ValoresDep6[1] + ListaBiConsumoDep6[i].ValorMes2,
                        ValoresDep6[2] + ListaBiConsumoDep6[i].ValorMes3,
                        ValoresDep6[3] + ListaBiConsumoDep6[i].ValorMes4,
                        ValoresDep6[4] + ListaBiConsumoDep6[i].ValorMes5,
                        ValoresDep6[5] + ListaBiConsumoDep6[i].ValorMes6
                   };
                }
                if (AtualizaGrid)
                {
                    grdBIConsumoPessoaProduto6.DataSource = ListaBiConsumoDep6;
                    grdBIConsumoPessoaProduto6.DataBind();
                }
            }// se existir pelo menos 6 dep;
            else
            {
                grdBIConsumoPessoaProduto6.DataSource = null;
                grdBIConsumoPessoaProduto6.DataBind();
            }

            Session["ListaBIConsumoClienteProduto"] = ListaBIConsumo;          
        }

        protected void MontarValorTotal(List<ProdutoDocumento> ListaItens)
        {
            decimal ValorTotal = 0;
            txtVlrTotal.Text = Convert.ToDouble("0,00").ToString("F");

            long CodigoPessoa = 0;
            int CodigoTipoOperacao = 0;
            decimal decValorFrete = 0;
            long CodigoEmpresa = 0;
            int CodigoAplicacaoUso = 0;

            if (Session["Doc_orcamento"] != null)
            {
                Doc_Orcamento doc = new Doc_Orcamento();
                doc = (Doc_Orcamento)Session["Doc_orcamento"];
                CodigoPessoa = doc.Cpl_CodigoPessoa;
                CodigoTipoOperacao = 1;
                decValorFrete = doc.ValorFrete;
                CodigoEmpresa = doc.CodigoEmpresa;
                CodigoAplicacaoUso = doc.CodigoAplicacaoUso;
            }
            else if (Session["Doc_Pedido"] != null)
            {
                Doc_Pedido doc = new Doc_Pedido();
                doc = (Doc_Pedido)Session["Doc_Pedido"];
                CodigoPessoa = doc.Cpl_CodigoPessoa;
                CodigoTipoOperacao = doc.CodigoTipoOperacao;
                decValorFrete = doc.ValorFrete;
                CodigoEmpresa = doc.CodigoEmpresa;
                CodigoAplicacaoUso = doc.CodigoAplicacaoUso;
            }
            else
            {
                return;
            }

            List<ProdutoDocumento> NovaListaItens = new List<ProdutoDocumento>();

            decimal decValorFreteRatiado = 0;
            decimal decDiferencaValorFreteRatiado = 0;
            if (ListaItens.Where(x => x.CodigoSituacao != 134).Count() > 0)
            {
                if (decValorFrete > 0)
                    decValorFreteRatiado = decValorFrete / ListaItens.Where(x => x.CodigoSituacao != 134).Count();

                if ((decValorFreteRatiado * ListaItens.Where(x => x.CodigoSituacao != 134).Count()) != decValorFrete)
                {
                    decDiferencaValorFreteRatiado = decValorFrete - (decValorFreteRatiado * ListaItens.Where(x => x.CodigoSituacao != 134).Count());
                }


                foreach (ProdutoDocumento itens in ListaItens)
                {
                    if (itens.CodigoSituacao != 134)
                    {
                        if (decDiferencaValorFreteRatiado != 0)
                        {
                            itens.Impostos = ImpostoProdutoDocumentoDAL.PreencherImpostosProdutoDocumento(itens, CodigoEmpresa, CodigoPessoa, CodigoTipoOperacao, CodigoAplicacaoUso, decValorFreteRatiado - (decDiferencaValorFreteRatiado), false);
                            decDiferencaValorFreteRatiado = 0;
                        }
                        else
                            itens.Impostos = ImpostoProdutoDocumentoDAL.PreencherImpostosProdutoDocumento(itens, CodigoEmpresa, CodigoPessoa, CodigoTipoOperacao, CodigoAplicacaoUso, decValorFreteRatiado, false);

                        ValorTotal += itens.ValorTotalItem;
                    }
                    NovaListaItens.Add(itens);
                }
            }
            txtVlrTotal.Text = Convert.ToString(ValorTotal + decValorFrete);
            txtVlrTotal.Text = Convert.ToDouble(txtVlrTotal.Text).ToString("F");
        }

        protected void grdProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ProdutoDocumento item in ListaItemDocumento)
            {
                if (item.CodigoItem == Convert.ToInt32(HttpUtility.HtmlDecode(grdProduto.SelectedRow.Cells[0].Text)))
                {
                    Session["ManutencaoProduto"] = item;
                    txtCodItem.Text = item.CodigoProduto.ToString();
                    txtQtde.Text = item.Quantidade.ToString("###,##0.00");
                    txtPreco.Text = item.PrecoItem.ToString("###,##0.00");
                    txtDesconto.Text = item.ValorDesconto.ToString("###,##0.00");
                    ddlUnidade.SelectedItem.Text = item.Unidade.ToString();
                    txtDescricao.Text = item.Cpl_DscProduto;
                    txtVlrTotalItem.Text = item.ValorTotalItem.ToString("###,##0.00");
                    BtnExcluirProduto.Visible = true;
                }
            }
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }

        protected void grdLogDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void CamposProduto()
        {
            if (Session["ManutencaoProduto"] != null)
            {
                BtnExcluirProduto.Visible = true;
            }
            else
            {
                BtnExcluirProduto.Visible = false;
            }
        }

        protected void MontaMesesAnteriores()
        {
            DateTime DataDia = DateTime.Now.Date;
            MesesAnteriores = new string[] {
                RetornaDescricaoMes(DataDia.Month - 6),//Mes 1
                RetornaDescricaoMes(DataDia.Month - 5),//Mes 2
                RetornaDescricaoMes(DataDia.Month - 4),//Mes 3
                RetornaDescricaoMes(DataDia.Month - 3),//Mes 4
                RetornaDescricaoMes(DataDia.Month - 2),//Mes 5
                RetornaDescricaoMes(DataDia.Month - 1) //Mes 6
            };
        }

        protected string RetornaDescricaoMes(int intMes)
        {

            if (intMes <= 0)
                intMes = intMes - 1;

            if (intMes == 1 || intMes == -12)
                return "Jan.";
            if (intMes == 2 || intMes == -11)
                return "Fev.";
            if (intMes == 3 || intMes == -10)
                return "Mar.";
            if (intMes == 4 || intMes == -9)
                return "Abr.";
            if (intMes == 5 || intMes == -8)
                return "Mai.";
            if (intMes == 6 || intMes == -7)
                return "Jun.";
            if (intMes == 7 || intMes == -6)
                return "Jul.";
            if (intMes == 8 || intMes == -5)
                return "Ago.";
            if (intMes == 9 || intMes == -4)
                return "Set.";
            if (intMes == 10 || intMes == -3)
                return "Out.";
            if (intMes == 11 || intMes == -2)
                return "Nov.";
            if (intMes == 12 || intMes == -1)
                return "Dez.";

            return "";
        }

        protected void BtnAddProduto_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos() == false)
                    return;
                int CodigoTipoOperacao = 0;
                if (Session["Doc_orcamento"] != null)
                {
                    Doc_Orcamento doc = new Doc_Orcamento();
                    doc = (Doc_Orcamento)Session["Doc_orcamento"];
                    CodigoTipoOperacao = doc.CodigoTipoOperacao;
                }
                else if (Session["Doc_Pedido"] != null)
                {
                    Doc_Pedido doc = new Doc_Pedido();
                    doc = (Doc_Pedido)Session["Doc_Pedido"];
                    CodigoTipoOperacao = doc.CodigoTipoOperacao;
                }
                if(CodigoTipoOperacao == 0)
                {
                    ShowMessage("Tipo de operação é necessário, volte na tela anterior e selecione um tipo de operação", MessageType.Info);
                    return;
                }

                TipoOperacao tpOP = new TipoOperacao();
                TipoOperacaoDAL tpOPDAL = new TipoOperacaoDAL();
                tpOP = tpOPDAL.PesquisarTipoOperacao(CodigoTipoOperacao);
                if (tpOP.MovimentaEstoque)
                {
                    EstoqueProdutoDAL estoqueDAL = new EstoqueProdutoDAL();
                    decimal decQuantidadeDisponivel = estoqueDAL.PesquisarEstoqueDisponivelProduto(Convert.ToInt64(txtCodItem.Text), Convert.ToInt32(Session["CodEmpresa"]));
                    if (Convert.ToDecimal(txtQtde.Text) > decQuantidadeDisponivel)
                    {
                        if (decQuantidadeDisponivel == 0)
                            ShowMessage("Este produto não tem em estoque", MessageType.Warning);
                        else
                            ShowMessage("Produto possui apenas " + decQuantidadeDisponivel + " " + ddlUnidade.SelectedItem.Text + " disponível em estoque", MessageType.Warning);
                        return;
                    }
                }
                ProdutoDAL produtoDAL = new ProdutoDAL();
                Produto p = new Produto();
                p = produtoDAL.PesquisarProduto(Convert.ToInt64(txtCodItem.Text));

                if (Session["ManutencaoProduto"] != null)
                {
                    ProdutoDocumento produto = (ProdutoDocumento)Session["ManutencaoProduto"];
                    Session["ManutencaoProduto"] = null;

                    List<ProdutoDocumento> ListaItem = new List<ProdutoDocumento>();

                    if (Session["ListaItemDocumento"] != null)
                        ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemDocumento"];
                    else
                        ListaItemDocumento = new List<ProdutoDocumento>();

                    ProdutoDocumento tabi;
                    foreach (ProdutoDocumento item in ListaItemDocumento)
                    {
                        if (item.CodigoItem != produto.CodigoItem)
                        {
                            tabi = new ProdutoDocumento();
                            tabi.CodigoItem = item.CodigoItem;
                            tabi.CodigoDocumento = item.CodigoDocumento;
                            tabi.Unidade = item.Unidade;
                            tabi.Cpl_DscProduto = item.Cpl_DscProduto;
                            tabi.CodigoProduto = item.CodigoProduto;
                            tabi.Quantidade = item.Quantidade;
                            tabi.QuantidadeAtendida = item.QuantidadeAtendida;
                            tabi.PrecoItem = item.PrecoItem;
                            tabi.ValorDesconto = item.ValorDesconto;
                            tabi.ValorTotalItem = item.ValorTotalItem;
                            tabi.CodigoSituacao = item.CodigoSituacao;
                            tabi.ValorFatorCubagem = item.ValorFatorCubagem;
                            tabi.ValorPeso = item.ValorPeso;
                            tabi.ValorVolume = item.ValorVolume;
                            tabi.Impostos = item.Impostos;
                            tabi.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            tabi.Cpl_PrecoVenda = Convert.ToDecimal(p.ValorVenda);
                            ListaItem.Add(tabi);
                        }
                        else
                        {
                            tabi = new ProdutoDocumento();
                            tabi.CodigoItem = item.CodigoItem;
                            tabi.CodigoDocumento = item.CodigoDocumento;
                            tabi.Unidade = ddlUnidade.SelectedItem.Text;
                            tabi.Cpl_DscProduto = Convert.ToString(txtDescricao.Text);
                            tabi.CodigoProduto = Convert.ToInt32(txtCodItem.Text);
                            tabi.Quantidade = Convert.ToDecimal(txtQtde.Text);
                            tabi.QuantidadeAtendida = 0;
                            tabi.ValorDesconto = Convert.ToDecimal(txtDesconto.Text);
                            tabi.PrecoItem = Convert.ToDecimal(txtPreco.Text);
                            tabi.ValorTotalItem = (Convert.ToDecimal(txtPreco.Text) * Convert.ToDecimal(txtQtde.Text));
                            tabi.CodigoSituacao = item.CodigoSituacao;
                            tabi.ValorFatorCubagem = item.ValorFatorCubagem;
                            tabi.ValorPeso = item.ValorPeso;
                            tabi.ValorVolume = item.ValorVolume;
                            tabi.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            tabi.Cpl_PrecoVenda = Convert.ToDecimal(p.ValorVenda);
                            //tabi.Impostos = CalcularImpostosProduto(item);
                            ListaItem.Add(tabi);
                        }
                    }

                    MontarValorTotal(ListaItem);
                    BtnExcluirProduto.Visible = false;
                    Session["ListaItemDocumento"] = ListaItem;

                    grdProduto.DataSource = ListaItem.Where(x => x.CodigoSituacao != 134).ToList();
                    grdProduto.DataBind();

                }//inserir produto novo
                else//alterar produto existente
                {

                    int intEndItem = 0;
                    ProdutoDocumentoDAL r = new ProdutoDocumentoDAL();
                    List<DBTabelaCampos> lista = new List<DBTabelaCampos>();

                    if (ListaItemDocumento.Count != 0)
                        intEndItem = Convert.ToInt32(ListaItemDocumento.Max(x => x.CodigoItem).ToString());

                    intEndItem = intEndItem + 1;

                    ProdutoDocumento ListaItem = new ProdutoDocumento();
                    ListaItem.CodigoItem = intEndItem;
                    ListaItem.CodigoProduto = Convert.ToInt32(txtCodItem.Text);
                    ListaItem.Cpl_DscProduto = txtDescricao.Text;
                    ListaItem.Quantidade = Convert.ToDecimal(txtQtde.Text);
                    ListaItem.QuantidadeAtendida = 0;
                    ListaItem.Unidade = ddlUnidade.SelectedItem.Text;
                    ListaItem.ValorDesconto = Convert.ToDecimal(txtDesconto.Text);
                    ListaItem.PrecoItem = Convert.ToDecimal(txtPreco.Text);
                    ListaItem.ValorTotalItem = (Convert.ToDecimal(txtPreco.Text) * Convert.ToDecimal(txtQtde.Text));
                    ListaItem.CodigoSituacao = 135;
                    ListaItem.ValorVolume = p.ValorVolume;
                    ListaItem.ValorPeso = p.ValorPeso;
                    ListaItem.ValorFatorCubagem = p.ValorFatorCubagem;
                    ListaItem.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                    ListaItem.Cpl_PrecoVenda = Convert.ToDecimal(p.ValorVenda);


                    if (intEndItem != 0)
                        ListaItemDocumento.RemoveAll(x => x.CodigoItem == intEndItem);

                    //ListaItem.Impostos = CalcularImpostosProduto(ListaItem);
                    ListaItemDocumento.Add(ListaItem);

                    Session["ListaItemDocumento"] = ListaItemDocumento;

                    var ListaAtivos = ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList();

                    if ((ListaAtivos.Count % 5) == 0)
                        ShowMessage("Salve o documento para garantir os itens já inseridos", MessageType.Warning);

                    grdProduto.DataSource = ListaAtivos;
                    grdProduto.DataBind();

                    MontarValorTotal(ListaItemDocumento);
                }

                LimpaTela();
                txtCodItem.Focus();
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        
        protected void BtnExcluirProduto_Click(object sender, EventArgs e)
        {
            List<ProdutoDocumento> ListaItem = new List<ProdutoDocumento>();

            if (Session["ListaItemDocumento"] != null)
                ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemDocumento"];
            else
                ListaItemDocumento = new List<ProdutoDocumento>();

            ProdutoDocumento tabi;
            foreach (ProdutoDocumento item in ListaItemDocumento)
            {
                tabi = new ProdutoDocumento();
                tabi.CodigoItem = item.CodigoItem;
                tabi.CodigoDocumento = item.CodigoDocumento;
                tabi.Unidade = item.Unidade;
                tabi.Cpl_DscProduto = item.Cpl_DscProduto;
                tabi.CodigoProduto = item.CodigoProduto;
                tabi.Quantidade = item.Quantidade;
                tabi.QuantidadeAtendida = item.QuantidadeAtendida;
                tabi.PrecoItem = item.PrecoItem;
                tabi.ValorDesconto = item.ValorDesconto;
                tabi.ValorTotalItem = item.ValorTotalItem;
                tabi.Cpl_DscSituacao = item.Cpl_DscSituacao;
                tabi.ValorFatorCubagem = item.ValorFatorCubagem;
                tabi.ValorPeso = item.ValorPeso;
                tabi.ValorVolume = item.ValorVolume;

                if (item.CodigoItem == Convert.ToInt32(HttpUtility.HtmlDecode(grdProduto.SelectedRow.Cells[0].Text)))
                    tabi.CodigoSituacao = 134;
                else
                    tabi.CodigoSituacao = item.CodigoSituacao;

                ListaItem.Add(tabi);
            }
            

            BtnExcluirProduto.Visible = false;
            MontarValorTotal(ListaItem);

            grdProduto.DataSource = ListaItem.Where(x => x.CodigoSituacao != 134).ToList(); 
            grdProduto.DataBind();

            Session["ListaItemDocumento"] = ListaItem;
            Session["ManutencaoProduto"] = null;

            LimpaTela();
            txtCodItem.Focus();
        }

        protected void btnPesquisarItem_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Produtos/ConProduto.aspx?cad=1");
        }

        protected void txtCodItem_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;
            if (txtCodItem.Text.Equals(""))
            {
                return;
            }
            else
            {
                v.CampoValido("Codigo Produto", txtCodItem.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (!blnCampo)
                {
                    txtCodItem.Text = "";
                    return;
                }
            }

            Int64 codigoItem = Convert.ToInt64(txtCodItem.Text);
            ProdutoDAL produtoDAL = new ProdutoDAL();
            Produto produto = new Produto();
            produto = produtoDAL.PesquisarProduto(codigoItem);//PESQUISAR PRODUTO DO TIPO PRODUTO ACABADO
            txtQtde.Focus();
            
            if (produto != null)
            {
                var ItemExiste = ListaItemDocumento.Where(x => x.CodigoProduto == produto.CodigoProduto && x.CodigoSituacao != 134);
                if (ItemExiste.Count() > 0)
                {
                    txtCodItem.Text = "";
                    txtCodItem.Focus();
                    txtDescricao.Text = "";
                    ShowMessage("Este produto já foi adicionado", MessageType.Info);
                }
                else
                {
                    if (produto.ValorVenda != 0)
                    {
                        txtDescricao.Text = produto.DescricaoProduto;
                        ddlUnidade.SelectedValue = produto.CodigoUnidade.ToString();
                        txtPreco.Text = produto.ValorVenda.ToString("F");
                        txtQtde.Focus();
                    }
                    else
                    {
                        txtCodItem.Text = "";
                        txtCodItem.Focus();
                        txtDescricao.Text = "";
                        txtPreco.Text = "0,00";
                        ShowMessage("Preço de venda do produto está zerado", MessageType.Info);
                    }
                }
            }
            else
            {
                txtCodItem.Text = "";
                txtCodItem.Focus();
                txtDescricao.Text = "";
                txtPreco.Text = "0,00";
                ShowMessage("Este produto não existe", MessageType.Info);
            }
            Session["TabFocada"] = null;
        }

        protected void txtPreco_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtPreco.Text.Equals(""))
            {
                txtPreco.Text = "0,00";
            }
            else
            {
                v.CampoValido("Preço", txtPreco.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    ProdutoDAL produtoDAL = new ProdutoDAL();
                    Produto p = new Produto();
                    p = produtoDAL.PesquisarProduto(Convert.ToInt64(txtCodItem.Text));

                    txtPreco.Text = Convert.ToDecimal(txtPreco.Text).ToString("###,##0.00");
                    txtDesconto.Focus();
                    txtDesconto.Text = (100 - ((Convert.ToDecimal(txtPreco.Text) / (Convert.ToDecimal(p.ValorVenda)))) * 100).ToString("###,##0.00");

                }
                else
                    txtPreco.Text = "0,00";

                txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * Convert.ToDecimal(txtPreco.Text)).ToString("###,##0.00");
            }
        }

        protected void txtQtde_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtQtde.Text.Equals(""))
            {
                txtQtde.Text = "0,00";
            }
            else
            {
                v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtQtde.Text = Convert.ToDecimal(txtQtde.Text).ToString("###,##0.00");
                    txtPreco.Focus();

                }
                else
                    txtQtde.Text = "0,00";

                txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * Convert.ToDecimal(txtPreco.Text)).ToString("###,##0.00");
            }
        }

        protected void grdProduto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdProduto.PageIndex = e.NewPageIndex;
            // Carrega os dados
            if (Session["ListaItemDocumento"] != null)
            {
                ListaItemDocumento = (List<ProdutoDocumento>)Session["ListaItemDocumento"];
                grdProduto.DataSource = ListaItemDocumento.Where(x => x.CodigoSituacao != 134).ToList();
                grdProduto.DataBind();

            }
        }

        protected void grdProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            if (dir == SortDirection.Ascending)
            {
                dir = SortDirection.Descending;
                sortingDirection = "Desc";
            }
            else
            {
                dir = SortDirection.Ascending;
                sortingDirection = "Asc";
            }

            List<ProdutoDocumento> ListOfInt = new List<ProdutoDocumento>();
            ListOfInt = (List<ProdutoDocumento>)grdProduto.DataSource;
            // populate list
            DataTable ListAsDataTable = BuildDataTable<ProdutoDocumento>(ListOfInt);
            DataView ListAsDataView = ListAsDataTable.DefaultView;

            ListAsDataView.Sort = e.SortExpression + " " + sortingDirection;

            grdProduto.DataSource = ListAsDataView;
            grdProduto.DataBind();

            ListaItemDocumento = ListAsDataView.ToTable().Rows.Cast<DataRow>().Select(t => new ProdutoDocumento()
            {
                CodigoItem = t.Field<int>("CodigoItem"),
                CodigoProduto = t.Field<int>("CodigoProduto"),
                Cpl_DscProduto = t.Field<string>("Cpl_DscProduto"),
                Unidade = t.Field<string>("Unidade"),
                Quantidade = t.Field<decimal>("Quantidade"),
                PrecoItem = t.Field<decimal>("PrecoItem"),
                ValorTotalItem = t.Field<decimal>("ValorTotalItem"),
                Cpl_DscSituacao = t.Field<string>("Cpl_DscSituacao")
            }).ToList();

            Session["ListaItemDocumento"] = ListaItemDocumento;

            for (int i = 0; i < grdProduto.Columns.Count; i++)
            {
                if (grdProduto.Columns[i].SortExpression == e.SortExpression)
                {
                    var cell = grdProduto.HeaderRow.Cells[i];
                    Image image = new Image();
                    image.Height = 15;
                    image.Width = 15;
                    Literal litespaco = new Literal();
                    litespaco.Text = "&emsp;";
                    cell.Controls.Add(litespaco);

                    if (sortingDirection == "Desc")
                        image.ImageUrl = "../../images/down_arrow.png";
                    else
                        image.ImageUrl = "../../images/up_arrow.png";

                    cell.Controls.Add(image);
                }

            }
        }

        protected void grdBIConsumoPessoaProduto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grdBIConsumoPessoaProduto_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grdBIConsumoPessoaProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            if (dir == SortDirection.Ascending)
            {
                dir = SortDirection.Descending;
                sortingDirection = "Desc";
            }
            else
            {
                dir = SortDirection.Ascending;
                sortingDirection = "Asc";
            }

            List<BIConsumoClienteProduto> ListOfInt2 = new List<BIConsumoClienteProduto>();

            ListOfInt2 = (List<BIConsumoClienteProduto>)Session["ListaBIConsumoClienteProduto"];
            // populate list
            DataTable ListAsDataTable = BuildDataTable<BIConsumoClienteProduto>(ListOfInt2);
            DataView ListAsDataView = ListAsDataTable.DefaultView;

            ListAsDataView.Sort = e.SortExpression + " " + sortingDirection;
            grdBIConsumoPessoaProduto.DataSource = ListAsDataView;
            grdBIConsumoPessoaProduto.DataBind();


            for (int i = 0; i < grdBIConsumoPessoaProduto.Columns.Count; i++)
            {
                if (grdBIConsumoPessoaProduto.Columns[i].SortExpression == e.SortExpression)
                {
                    var cell = grdBIConsumoPessoaProduto.HeaderRow.Cells[i];
                    Image image = new Image();
                    image.Height = 15;
                    image.Width = 15;
                    Literal litespaco = new Literal();
                    litespaco.Text = "&emsp;";
                    cell.Controls.Add(litespaco);

                    if (sortingDirection == "Desc")
                        image.ImageUrl = "../../images/down_arrow.png";
                    else
                        image.ImageUrl = "../../images/up_arrow.png";

                    cell.Controls.Add(image);
                }

            }
        }

        public SortDirection dir
        {
            get
            {
                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["dirState"];
            }
            set
            {
                ViewState["dirState"] = value;
            }
        }

        public static DataTable BuildDataTable<T>(IList<T> lst)
        {
            //create DataTable Structure
            DataTable tbl = CreateTable<T>();
            Type entType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            //get the list item and add into the list
            foreach (T item in lst)
            {
                DataRow row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                tbl.Rows.Add(row);
            }
            return tbl;
        }
        
        private static DataTable CreateTable<T>()
        {
            //T –> ClassName
            Type entType = typeof(T);
            //set the datatable name as class name
            DataTable tbl = new DataTable(entType.Name);
            //get the property list
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                //add property as column

                if (prop != null)

                {
                    tbl.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

            }
            return tbl;
        }

        protected void btnAddBIConsumoClienteProduto_Click(object sender, EventArgs e)
        {
            try
            {


                int NroSelecionados = 0;
                ListaBIConsumoClienteProduto = (List<BIConsumoClienteProduto>)Session["ListaBIConsumoClienteProduto"];

                foreach (GridViewRow row in grdBIConsumoPessoaProduto.Rows)
                {
                    TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep1");

                    if (Convert.ToDecimal(txtQtdeComprar.Text) > 0)
                    {
                        IEnumerable<ProdutoDocumento> ItensOrcamento = ListaItemDocumento.Where((ProdutoDocumento c) =>
                        {
                            return c.Cpl_CodigoBIConsumoClienteProduto == Convert.ToInt32(row.Cells[0].Text);
                        });

                        if (ItensOrcamento.Count() == 0)
                        {
                            ProdutoDocumento NovoItem = new ProdutoDocumento();
                            NovoItem.CodigoProduto = Convert.ToInt32(row.Cells[1].Text);

                            if (ListaItemDocumento.Count == 0 || ListaItemDocumento == null)
                                NovoItem.CodigoItem = 1;
                            else
                                NovoItem.CodigoItem = ListaItemDocumento.Max(x => x.CodigoItem) + 1;

                            NovoItem.Cpl_DscProduto = row.Cells[2].Text;
                            NovoItem.Quantidade = Convert.ToDecimal(txtQtdeComprar.Text);
                            NovoItem.Cpl_CodigoBIConsumoClienteProduto = Convert.ToInt32(row.Cells[0].Text);

                            Produto produto = new Produto();
                            ProdutoDAL produtoDAL = new ProdutoDAL();
                            produto = produtoDAL.PesquisarProduto(Convert.ToInt32(row.Cells[1].Text));

                            Unidade unidade = new Unidade();
                            UnidadeDAL unidadeDAL = new UnidadeDAL();
                            unidade = unidadeDAL.PesquisarUnidade(produto.CodigoUnidade);


                            NovoItem.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            NovoItem.Unidade = unidade.SiglaUnidade;
                            NovoItem.PrecoItem = Convert.ToDecimal(produto.ValorVenda);
                            NovoItem.ValorDesconto = 0;
                            NovoItem.ValorTotalItem = (NovoItem.Quantidade * NovoItem.PrecoItem) * (1 - (NovoItem.ValorDesconto / 100));
                            ListaItemDocumento.Add(NovoItem);

                            ListaBIConsumoClienteProduto.RemoveAll(x => x.CodigoIndex == Convert.ToInt32(row.Cells[0].Text));
                            Session["ListaBIConsumoClienteProduto"] = ListaBIConsumoClienteProduto;


                            NroSelecionados++;
                        }
                    }
                }
                foreach (GridViewRow row in grdBIConsumoPessoaProduto2.Rows)
                {
                    TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep2");

                    if (Convert.ToDecimal(txtQtdeComprar.Text) > 0)
                    {
                        IEnumerable<ProdutoDocumento> ItensOrcamento = ListaItemDocumento.Where((ProdutoDocumento c) =>
                        {
                            return c.Cpl_CodigoBIConsumoClienteProduto == Convert.ToInt32(row.Cells[0].Text);
                        });

                        if (ItensOrcamento.Count() == 0)
                        {
                            ProdutoDocumento NovoItem = new ProdutoDocumento();
                            NovoItem.CodigoProduto = Convert.ToInt32(row.Cells[1].Text);

                            if (ListaItemDocumento.Count == 0 || ListaItemDocumento == null)
                                NovoItem.CodigoItem = 1;
                            else
                                NovoItem.CodigoItem = ListaItemDocumento.Max(x => x.CodigoItem) + 1;

                            NovoItem.Cpl_DscProduto = row.Cells[2].Text;
                            NovoItem.Quantidade = Convert.ToDecimal(txtQtdeComprar.Text);
                            NovoItem.Cpl_CodigoBIConsumoClienteProduto = Convert.ToInt32(row.Cells[0].Text);

                            Produto produto = new Produto();
                            ProdutoDAL produtoDAL = new ProdutoDAL();
                            produto = produtoDAL.PesquisarProduto(Convert.ToInt32(row.Cells[1].Text));

                            Unidade unidade = new Unidade();
                            UnidadeDAL unidadeDAL = new UnidadeDAL();
                            unidade = unidadeDAL.PesquisarUnidade(produto.CodigoUnidade);
                            NovoItem.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            NovoItem.Unidade = unidade.SiglaUnidade;
                            NovoItem.PrecoItem = Convert.ToDecimal(produto.ValorVenda);
                            NovoItem.ValorTotalItem = NovoItem.Quantidade * NovoItem.PrecoItem;
                            ListaItemDocumento.Add(NovoItem);

                            ListaBIConsumoClienteProduto.RemoveAll(x => x.CodigoIndex == Convert.ToInt32(row.Cells[0].Text));

                            NroSelecionados++;

                        }
                    }
                }
                foreach (GridViewRow row in grdBIConsumoPessoaProduto3.Rows)
                {
                    TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep3");

                    if (Convert.ToDecimal(txtQtdeComprar.Text) > 0)
                    {
                        IEnumerable<ProdutoDocumento> ItensOrcamento = ListaItemDocumento.Where((ProdutoDocumento c) =>
                        {
                            return c.Cpl_CodigoBIConsumoClienteProduto == Convert.ToInt32(row.Cells[0].Text);
                        });

                        if (ItensOrcamento.Count() == 0)
                        {
                            ProdutoDocumento NovoItem = new ProdutoDocumento();
                            NovoItem.CodigoProduto = Convert.ToInt32(row.Cells[1].Text);

                            if (ListaItemDocumento.Count == 0 || ListaItemDocumento == null)
                                NovoItem.CodigoItem = 1;
                            else
                                NovoItem.CodigoItem = ListaItemDocumento.Max(x => x.CodigoItem) + 1;

                            NovoItem.Cpl_DscProduto = row.Cells[2].Text;
                            NovoItem.Quantidade = Convert.ToDecimal(txtQtdeComprar.Text);
                            NovoItem.Cpl_CodigoBIConsumoClienteProduto = Convert.ToInt32(row.Cells[0].Text);

                            Produto produto = new Produto();
                            ProdutoDAL produtoDAL = new ProdutoDAL();
                            produto = produtoDAL.PesquisarProduto(Convert.ToInt32(row.Cells[1].Text));

                            Unidade unidade = new Unidade();
                            UnidadeDAL unidadeDAL = new UnidadeDAL();
                            unidade = unidadeDAL.PesquisarUnidade(produto.CodigoUnidade);
                            NovoItem.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            NovoItem.Unidade = unidade.SiglaUnidade;
                            NovoItem.PrecoItem = Convert.ToDecimal(produto.ValorVenda);
                            NovoItem.ValorTotalItem = NovoItem.Quantidade * NovoItem.PrecoItem;
                            ListaItemDocumento.Add(NovoItem);

                            ListaBIConsumoClienteProduto.RemoveAll(x => x.CodigoIndex == Convert.ToInt32(row.Cells[0].Text));

                            NroSelecionados++;

                        }
                    }
                }
                foreach (GridViewRow row in grdBIConsumoPessoaProduto4.Rows)
                {
                    TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep4");

                    if (Convert.ToDecimal(txtQtdeComprar.Text) > 0)
                    {
                        IEnumerable<ProdutoDocumento> ItensOrcamento = ListaItemDocumento.Where((ProdutoDocumento c) =>
                        {
                            return c.Cpl_CodigoBIConsumoClienteProduto == Convert.ToInt32(row.Cells[0].Text);
                        });

                        if (ItensOrcamento.Count() == 0)
                        {
                            ProdutoDocumento NovoItem = new ProdutoDocumento();
                            NovoItem.CodigoProduto = Convert.ToInt32(row.Cells[1].Text);

                            if (ListaItemDocumento.Count == 0 || ListaItemDocumento == null)
                                NovoItem.CodigoItem = 1;
                            else
                                NovoItem.CodigoItem = ListaItemDocumento.Max(x => x.CodigoItem) + 1;

                            NovoItem.Cpl_DscProduto = row.Cells[2].Text;
                            NovoItem.Quantidade = Convert.ToDecimal(txtQtdeComprar.Text);
                            NovoItem.Cpl_CodigoBIConsumoClienteProduto = Convert.ToInt32(row.Cells[0].Text);

                            Produto produto = new Produto();
                            ProdutoDAL produtoDAL = new ProdutoDAL();
                            produto = produtoDAL.PesquisarProduto(Convert.ToInt32(row.Cells[1].Text));

                            Unidade unidade = new Unidade();
                            UnidadeDAL unidadeDAL = new UnidadeDAL();
                            unidade = unidadeDAL.PesquisarUnidade(produto.CodigoUnidade);
                            NovoItem.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            NovoItem.Unidade = unidade.SiglaUnidade;
                            NovoItem.PrecoItem = Convert.ToDecimal(produto.ValorVenda);
                            NovoItem.ValorTotalItem = NovoItem.Quantidade * NovoItem.PrecoItem;

                            ListaItemDocumento.Add(NovoItem);

                            ListaBIConsumoClienteProduto.RemoveAll(x => x.CodigoIndex == Convert.ToInt32(row.Cells[0].Text));

                            NroSelecionados++;

                        }
                    }
                }
                foreach (GridViewRow row in grdBIConsumoPessoaProduto5.Rows)
                {
                    TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep5");

                    if (Convert.ToDecimal(txtQtdeComprar.Text) > 0)
                    {
                        IEnumerable<ProdutoDocumento> ItensOrcamento = ListaItemDocumento.Where((ProdutoDocumento c) =>
                        {
                            return c.Cpl_CodigoBIConsumoClienteProduto == Convert.ToInt32(row.Cells[0].Text);
                        });

                        if (ItensOrcamento.Count() == 0)
                        {
                            ProdutoDocumento NovoItem = new ProdutoDocumento();
                            NovoItem.CodigoProduto = Convert.ToInt32(row.Cells[1].Text);

                            if (ListaItemDocumento.Count == 0 || ListaItemDocumento == null)
                                NovoItem.CodigoItem = 1;
                            else
                                NovoItem.CodigoItem = ListaItemDocumento.Max(x => x.CodigoItem) + 1;

                            NovoItem.Cpl_DscProduto = row.Cells[2].Text;
                            NovoItem.Quantidade = Convert.ToDecimal(txtQtdeComprar.Text);
                            NovoItem.Cpl_CodigoBIConsumoClienteProduto = Convert.ToInt32(row.Cells[0].Text);

                            Produto produto = new Produto();
                            ProdutoDAL produtoDAL = new ProdutoDAL();
                            produto = produtoDAL.PesquisarProduto(Convert.ToInt32(row.Cells[1].Text));

                            Unidade unidade = new Unidade();
                            UnidadeDAL unidadeDAL = new UnidadeDAL();
                            unidade = unidadeDAL.PesquisarUnidade(produto.CodigoUnidade);
                            NovoItem.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            NovoItem.Unidade = unidade.SiglaUnidade;
                            NovoItem.PrecoItem = Convert.ToDecimal(produto.ValorVenda);
                            NovoItem.ValorTotalItem = NovoItem.Quantidade * NovoItem.PrecoItem;
                            ListaItemDocumento.Add(NovoItem);

                            ListaBIConsumoClienteProduto.RemoveAll(x => x.CodigoIndex == Convert.ToInt32(row.Cells[0].Text));

                            NroSelecionados++;

                        }
                    }
                }
                foreach (GridViewRow row in grdBIConsumoPessoaProduto6.Rows)
                {
                    TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep6");

                    if (Convert.ToDecimal(txtQtdeComprar.Text) > 0)
                    {
                        IEnumerable<ProdutoDocumento> ItensOrcamento = ListaItemDocumento.Where((ProdutoDocumento c) =>
                        {
                            return c.Cpl_CodigoBIConsumoClienteProduto == Convert.ToInt32(row.Cells[0].Text);
                        });

                        if (ItensOrcamento.Count() == 0)
                        {
                            ProdutoDocumento NovoItem = new ProdutoDocumento();
                            NovoItem.CodigoProduto = Convert.ToInt32(row.Cells[1].Text);

                            if (ListaItemDocumento.Count == 0 || ListaItemDocumento == null)
                                NovoItem.CodigoItem = 1;
                            else
                                NovoItem.CodigoItem = ListaItemDocumento.Max(x => x.CodigoItem) + 1;

                            NovoItem.Cpl_DscProduto = row.Cells[2].Text;
                            NovoItem.Quantidade = Convert.ToDecimal(txtQtdeComprar.Text);
                            NovoItem.Cpl_CodigoBIConsumoClienteProduto = Convert.ToInt32(row.Cells[0].Text);

                            Produto produto = new Produto();
                            ProdutoDAL produtoDAL = new ProdutoDAL();
                            produto = produtoDAL.PesquisarProduto(Convert.ToInt32(row.Cells[1].Text));

                            Unidade unidade = new Unidade();
                            UnidadeDAL unidadeDAL = new UnidadeDAL();
                            unidade = unidadeDAL.PesquisarUnidade(produto.CodigoUnidade);
                            NovoItem.Cpl_DscSituacao = "AGUARDANDO INCLUSÃO";
                            NovoItem.Unidade = unidade.SiglaUnidade;
                            NovoItem.PrecoItem = Convert.ToDecimal(produto.ValorVenda);
                            NovoItem.ValorTotalItem = NovoItem.Quantidade * NovoItem.PrecoItem;
                            ListaItemDocumento.Add(NovoItem);

                            ListaBIConsumoClienteProduto.RemoveAll(x => x.CodigoIndex == Convert.ToInt32(row.Cells[0].Text));

                            NroSelecionados++;

                        }
                    }
                }

                Session["ListaItemDocumento"] = ListaItemDocumento;
                grdProduto.DataSource = ListaItemDocumento;
                grdProduto.DataBind();

                Session["ListaBIConsumoClienteProduto"] = ListaBIConsumoClienteProduto;
                MontaGridsBiConsumoClienteProduto(false, true);
                MontarValorTotal(ListaItemDocumento);

                if (NroSelecionados == 0)
                    ShowMessage("Nenhum item foi adicionado ao documento!", MessageType.Warning);
                else if (NroSelecionados == 1)
                    ShowMessage(NroSelecionados + " item foi adicionado ao documento!", MessageType.Success);
                else
                    ShowMessage(NroSelecionados + " itens foram adicionados ao documento!", MessageType.Success);
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void txtQtdaComprarDep1_TextChanged(object sender, EventArgs e)
        {
            int contador = 0;
            Boolean blnCampo = false;
            foreach (GridViewRow row in grdBIConsumoPessoaProduto.Rows)
            {
                if (contador == 1)
                {
                    GridViewSetFocus(row);
                    return;
                }                    

                TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep1");
                if (txtQtdeComprar.Text.Equals(""))
                {
                    txtQtdeComprar.Text = "0,00";
                }
                else
                {
                    v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                    if (blnCampo)
                    {
                        string txtQtdeComprar2 = Convert.ToDecimal(txtQtdeComprar.Text).ToString("###,##0.00");

                        if (txtQtdeComprar.Text != txtQtdeComprar2)
                        {
                            txtQtdeComprar.Text = txtQtdeComprar2;                           
                            contador++;
                        }                        
                    }
                    else
                        txtQtdeComprar.Text = "0,00";
                }                
            }                     
        }

        protected void txtQtdaComprarDep2_TextChanged(object sender, EventArgs e)
        {
            int contador = 0;
            Boolean blnCampo = false;
            foreach (GridViewRow row in grdBIConsumoPessoaProduto2.Rows)
            {
                if (contador == 1)
                {
                    GridViewSetFocus(row);
                    return;
                }

                TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep2");
                if (txtQtdeComprar.Text.Equals(""))
                {
                    txtQtdeComprar.Text = "0,00";
                }
                else
                {
                    v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                    if (blnCampo)
                    {
                        string txtQtdeComprar2 = Convert.ToDecimal(txtQtdeComprar.Text).ToString("###,##0.00");

                        if (txtQtdeComprar.Text != txtQtdeComprar2)
                        {
                            txtQtdeComprar.Text = txtQtdeComprar2;
                            contador++;
                        }
                    }
                    else
                        txtQtdeComprar.Text = "0,00";
                }
            }
        }

        protected void txtQtdaComprarDep3_TextChanged(object sender, EventArgs e)
        {
            int contador = 0;
            Boolean blnCampo = false;
            foreach (GridViewRow row in grdBIConsumoPessoaProduto3.Rows)
            {
                if (contador == 1)
                {
                    GridViewSetFocus(row);
                    return;
                }

                TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep3");
                if (txtQtdeComprar.Text.Equals(""))
                {
                    txtQtdeComprar.Text = "0,00";
                }
                else
                {
                    v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                    if (blnCampo)
                    {
                        string txtQtdeComprar2 = Convert.ToDecimal(txtQtdeComprar.Text).ToString("###,##0.00");

                        if (txtQtdeComprar.Text != txtQtdeComprar2)
                        {
                            txtQtdeComprar.Text = txtQtdeComprar2;
                            contador++;
                        }
                    }
                    else
                        txtQtdeComprar.Text = "0,00";
                }
            }
        }

        protected void txtQtdaComprarDep4_TextChanged(object sender, EventArgs e)
        {
            int contador = 0;
            Boolean blnCampo = false;
            foreach (GridViewRow row in grdBIConsumoPessoaProduto4.Rows)
            {
                if (contador == 1)
                {
                    GridViewSetFocus(row);
                    return;
                }

                TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep4");
                if (txtQtdeComprar.Text.Equals(""))
                {
                    txtQtdeComprar.Text = "0,00";
                }
                else
                {
                    v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                    if (blnCampo)
                    {
                        string txtQtdeComprar2 = Convert.ToDecimal(txtQtdeComprar.Text).ToString("###,##0.00");

                        if (txtQtdeComprar.Text != txtQtdeComprar2)
                        {
                            txtQtdeComprar.Text = txtQtdeComprar2;
                            contador++;
                        }
                    }
                    else
                        txtQtdeComprar.Text = "0,00";
                }
            }
        }

        protected void txtQtdaComprarDep5_TextChanged(object sender, EventArgs e)
        {
            int contador = 0;
            Boolean blnCampo = false;
            foreach (GridViewRow row in grdBIConsumoPessoaProduto5.Rows)
            {
                if (contador == 1)
                {
                    GridViewSetFocus(row);
                    return;
                }

                TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep5");
                if (txtQtdeComprar.Text.Equals(""))
                {
                    txtQtdeComprar.Text = "0,00";
                }
                else
                {
                    v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                    if (blnCampo)
                    {
                        string txtQtdeComprar2 = Convert.ToDecimal(txtQtdeComprar.Text).ToString("###,##0.00");

                        if (txtQtdeComprar.Text != txtQtdeComprar2)
                        {
                            txtQtdeComprar.Text = txtQtdeComprar2;
                            contador++;
                        }
                    }
                    else
                        txtQtdeComprar.Text = "0,00";
                }
            }
        }

        protected void txtQtdaComprarDep6_TextChanged(object sender, EventArgs e)
        {
            int contador = 0;
            Boolean blnCampo = false;
            foreach (GridViewRow row in grdBIConsumoPessoaProduto6.Rows)
            {
                if (contador == 1)
                {
                    GridViewSetFocus(row);
                    return;
                }

                TextBox txtQtdeComprar = (TextBox)row.FindControl("txtQtdaComprarDep6");
                if (txtQtdeComprar.Text.Equals(""))
                {
                    txtQtdeComprar.Text = "0,00";
                }
                else
                {
                    v.CampoValido("Quantidade", txtQtde.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                    if (blnCampo)
                    {
                        string txtQtdeComprar2 = Convert.ToDecimal(txtQtdeComprar.Text).ToString("###,##0.00");

                        if (txtQtdeComprar.Text != txtQtdeComprar2)
                        {
                            txtQtdeComprar.Text = txtQtdeComprar2;
                            contador++;
                        }
                    }
                    else
                        txtQtdeComprar.Text = "0,00";
                }
            }
        }

        public static void GridViewSetFocus(GridViewRow row)
        {
            bool found = false;
            foreach (TableCell cell in row.Cells)
            {
                foreach (Control control in cell.Controls)
                {
                    if (control.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                    {
                        found = true;
                        control.Focus();
                        break;
                    }
                }
                if (found)
                    break;
            }
        }

        protected void txtDesconto_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDesconto.Text.Equals(""))
            {
                txtDesconto.Text = "0,00";
            }
            else
            {
                v.CampoValido("Desconto", txtDesconto.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    BtnAddProduto.Focus();
                    decimal ValorDigitado = Convert.ToDecimal(txtPreco.Text);
                    ProdutoDAL produtoDAL = new ProdutoDAL();
                    Produto p = new Produto();
                    p = produtoDAL.PesquisarProduto(Convert.ToInt64(txtCodItem.Text));

                    txtDesconto.Text = Convert.ToDecimal(txtDesconto.Text).ToString("###,##0.00");
                    decimal Desconto = Convert.ToDecimal(txtDesconto.Text);

                    if (Desconto > 0)
                        txtPreco.Text = (Convert.ToDecimal(p.ValorVenda) * (1-(Desconto / 100))).ToString("###,##0.00");
                    else
                        txtPreco.Text = ((Convert.ToDecimal(txtQtde.Text) * Convert.ToDecimal(p.ValorVenda))).ToString("###,##0.00");

                }
                else
                    txtDesconto.Text = "0,00";

                txtVlrTotalItem.Text = (Convert.ToDecimal(txtQtde.Text) * Convert.ToDecimal(txtPreco.Text)).ToString("###,##0.00"); 
            }
        }
   
    }
}