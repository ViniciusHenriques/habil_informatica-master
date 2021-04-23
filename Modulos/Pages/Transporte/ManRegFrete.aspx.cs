using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
namespace SoftHabilInformatica.Pages.Transporte
{
    public partial class ManRegFrete : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();

        List<CidadeRegraFrete> ListaCidades = new List<CidadeRegraFrete>();

        String strMensagemR = "";

        public enum MessageType { Success, Error, Info, Warning };

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected Boolean ValidaCampos()
        {
            try { 
                if(ListaCidades.Count == 0)
                {
                    ShowMessage("Adicione cidades a regra", MessageType.Info);
                    return false;
                }
                if(txtRegiao.Text == "")
                {
                    ShowMessage("Insira uma região válida", MessageType.Info);
                    return false;
                }
                if (ddlTransp.SelectedValue == "* Nenhum selecionado")
                {
                    ShowMessage("Insira um transportador", MessageType.Info);
                    return false;
                }
                RegraFrete reg = new RegraFrete();
                RegraFreteDAL regDAL = new RegraFreteDAL();
                if(txtCodigo.Text != "Novo")
                    reg = regDAL.PesquisarRegraFreteRegiaoETransportador(txtRegiao.Text, Convert.ToInt64(ddlTransp.SelectedValue), Convert.ToInt32(txtCodigo.Text));
                else
                    reg = regDAL.PesquisarRegraFreteRegiaoETransportador(txtRegiao.Text, Convert.ToInt64(ddlTransp.SelectedValue), 0);
                if (reg != null)
                {
                    ShowMessage("Já existe uma regra com esta região e transportador", MessageType.Info);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                return false;
            }
        }

        protected void CarregaSituacoes()
        {
            try
            {
                EstadoDAL est = new EstadoDAL();
                ddlEstado.DataSource = est.ListarEstados("", "", "", "");
                ddlEstado.DataTextField = "Sigla";
                ddlEstado.DataValueField = "CodigoEstado";
                ddlEstado.DataBind();

                MunicipioDAL mun = new MunicipioDAL();
                ddlCidade.DataSource = mun.ListarMunicipios("M.CD_ESTADO", "SMALLINT", ddlEstado.SelectedValue, "");
                ddlCidade.DataTextField = "DescricaoMunicipio";
                ddlCidade.DataValueField = "CodigoMunicipio";
                ddlCidade.DataBind();

                PessoaDAL p = new PessoaDAL();
                ddlTransp.DataSource = p.ListarTransportadores("", "", "", "");
                ddlTransp.DataTextField = "NomePessoa";
                ddlTransp.DataValueField = "CodigoPessoa";
                ddlTransp.DataBind();
                ddlTransp.Items.Insert(0,"* Nenhum selecionado");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
                LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

                if (Session["MensagemTela"] != null)
                {
                    ShowMessage(Session["MensagemTela"].ToString(), MessageType.Error);
                    Session["MensagemTela"] = null;
                    return;
                }

                if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                    return;

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();

                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConRegFrete.aspx");

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

                if (Session["CodUsuario"].ToString() == "-150380")
                {
                    btnSalvar.Visible = true;
                }

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    if (Session["ZoomRegFrete2"] == null)
                        Session["Pagina"] = Request.CurrentExecutionFilePath;

                    if (Session["ZoomRegFrete"] != null)
                    {
                        string s = Session["ZoomRegFrete"].ToString();
                        Session["ZoomRegFrete"] = null;

                        string[] words = s.Split('³');
                        if (s != "³")
                        {
                            foreach (string word in words)
                            {
                                if (txtCodigo.Text == "")
                                {
                                    txtCodigo.Text = word;
                                    CarregaSituacoes();
                                    RegraFrete reg = new RegraFrete();
                                    RegraFreteDAL regDAL = new RegraFreteDAL();
                                    reg = regDAL.PesquisarRegraFreteIndex(Convert.ToInt32(word));
                                    ddlTransp.SelectedValue = reg.CodigoTransportador.ToString();
                                    txtFreteMinimo.Text = reg.ValorFreteMinimo.ToString();
                                    txtGRIS.Text = reg.ValorGRIS.ToString();
                                    txtRegiao.Text = reg.Regiao.ToString();

                                    txtGRISMinimo.Text = reg.ValorGRISMinimo.ToString();
                                    txtPedagio.Text = reg.ValorPedagio.ToString();
                                    txtPedagioMaximo.Text = reg.ValorPedagioMaximo.ToString();
                                    txtADValor.Text = reg.ValorAD.ToString();
                                    txtPorTonelada.Text = reg.ValorPorTonelada.ToString();
                                    txtSeguro.Text = reg.ValorSeguro.ToString();
                                    txtSeguroMinimo.Text = reg.ValorSeguroMinimo.ToString();
                                    txtExcedente1.Text = reg.DeParaExcedente1.ToString();
                                    txtExcedente2.Text = reg.DeParaExcedente2.ToString();
                                    txtCalcularADValor1.Text = reg.IndicadorCalcularAdValorDePara1.ToString();
                                    txtCalcularADValor2.Text = reg.IndicadorCalcularAdValorDePara2.ToString();
                                    txtTipoCalculo.Text = reg.IndicadorTipoCalculo.ToString();
                                    txtPesoCubado.Text = reg.ValorPesoCubado.ToString();

                                    txtDePara11.Text = reg.DePara11.ToString();
                                    txtDePara12.Text = reg.DePara12.ToString();
                                    txtDeParaPct11.Text = reg.DeParaPct11.ToString();

                                    txtDePara21.Text = reg.DePara21.ToString();
                                    txtDePara22.Text = reg.DePara22.ToString();
                                    txtDeParaPct12.Text = reg.DeParaPct12.ToString();

                                    txtDePara31.Text = reg.DePara31.ToString();
                                    txtDePara32.Text = reg.DePara32.ToString();
                                    txtDeParaPct31.Text = reg.DeParaPct13.ToString();

                                    txtDePara41.Text = reg.DePara41.ToString();
                                    txtDePara42.Text = reg.DePara42.ToString();
                                    txtDeParaPct41.Text = reg.DeParaPct14.ToString();

                                    txtDePara51.Text = reg.DePara51.ToString();
                                    txtDePara52.Text = reg.DePara52.ToString();
                                    txtDeParaPct51.Text = reg.DeParaPct15.ToString();

                                    txtDePara61.Text = reg.DePara61.ToString();
                                    txtDePara62.Text = reg.DePara62.ToString();
                                    txtDeParaPct61.Text = reg.DeParaPct16.ToString();

                                    txtDePara71.Text = reg.DePara71.ToString();
                                    txtDePara72.Text = reg.DePara72.ToString();
                                    txtDeParaPct71.Text = reg.DeParaPct17.ToString();

                                    CidadeRegraFreteDAL cityDAL = new CidadeRegraFreteDAL();
                                    ListaCidades = cityDAL.ObterCidadesRegraFrete(Convert.ToInt32(txtCodigo.Text));
                                    Session["ListaCidadesRegra"] = ListaCidades;
                                }
                            }
                        }
                    }
                    else
                    {
                        txtCodigo.Text = "Novo";
                        CarregaSituacoes();
                    }
                }
                if (Session["ListaCidadesRegra"] != null)
                {
                    ListaCidades = (List<CidadeRegraFrete>)Session["ListaCidadesRegra"];
                    grdCidades.DataSource = ListaCidades;
                    grdCidades.DataBind();
                }

                if(txtCodigo.Text != "Novo")
                {
                    ddlTransp.Enabled = false;
                    txtRegiao.Enabled = false;
                }
                else
                {
                    btnExcluir.Visible = false;
                }

                if (txtCodigo.Text == "")
                    btnVoltar_Click(sender, e);

            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Transporte/ConRegFrete.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            { 
                if (ValidaCampos() == false)
                    return;

                RegraFrete reg = new RegraFrete();
                RegraFreteDAL regDAL = new RegraFreteDAL();

                reg.CodigoTransportador = Convert.ToInt32(ddlTransp.SelectedValue);
                reg.Regiao = txtRegiao.Text;
                reg.ValorFreteMinimo = Convert.ToDecimal(txtFreteMinimo.Text);
                reg.ValorGRIS = Convert.ToDecimal(txtGRIS.Text);
                reg.ValorGRISMinimo = Convert.ToDecimal(txtGRISMinimo.Text);
                reg.ValorPedagio = Convert.ToDecimal(txtPedagio.Text);
                reg.ValorPedagioMaximo = Convert.ToDecimal(txtPedagioMaximo.Text);
                reg.ValorAD = Convert.ToDecimal(txtADValor.Text);
                reg.ValorPorTonelada = Convert.ToDecimal(txtPorTonelada.Text);
                reg.ValorSeguro = Convert.ToDecimal(txtSeguro.Text);
                reg.ValorSeguroMinimo = Convert.ToDecimal(txtSeguroMinimo.Text);
                reg.DeParaExcedente1 = Convert.ToDecimal(txtExcedente1.Text);
                reg.DeParaExcedente2 = Convert.ToDecimal(txtExcedente2.Text);
                reg.IndicadorCalcularAdValorDePara1 = Convert.ToInt32(txtCalcularADValor1.Text);
                reg.IndicadorCalcularAdValorDePara2 = Convert.ToInt32(txtCalcularADValor2.Text);
                reg.IndicadorTipoCalculo = Convert.ToInt32(txtTipoCalculo.Text);
                reg.ValorPesoCubado = Convert.ToInt32(txtPesoCubado.Text);

                reg.DePara11 = Convert.ToDecimal(txtDePara11.Text);
                reg.DePara12 = Convert.ToDecimal(txtDePara12.Text);
                reg.DeParaPct11 = Convert.ToDecimal(txtDeParaPct11.Text);

                reg.DePara21 = Convert.ToDecimal(txtDePara21.Text);
                reg.DePara22 = Convert.ToDecimal(txtDePara22.Text);
                reg.DeParaPct12 = Convert.ToDecimal(txtDeParaPct12.Text);
                
                reg.DePara31 = Convert.ToDecimal(txtDePara31.Text);
                reg.DePara32 = Convert.ToDecimal(txtDePara32.Text);
                reg.DeParaPct13 = Convert.ToDecimal(txtDeParaPct31.Text);

                reg.DePara41 = Convert.ToDecimal(txtDePara41.Text);
                reg.DePara42 = Convert.ToDecimal(txtDePara42.Text);
                reg.DeParaPct14 = Convert.ToDecimal(txtDeParaPct41.Text);

                reg.DePara51 = Convert.ToDecimal(txtDePara51.Text);
                reg.DePara52 = Convert.ToDecimal(txtDePara52.Text);
                reg.DeParaPct15 = Convert.ToDecimal(txtDeParaPct51.Text);

                reg.DePara61 = Convert.ToDecimal(txtDePara61.Text);
                reg.DePara62 = Convert.ToDecimal(txtDePara62.Text);
                reg.DeParaPct16 = Convert.ToDecimal(txtDeParaPct61.Text);

                reg.DePara71 = Convert.ToDecimal(txtDePara71.Text);
                reg.DePara72 = Convert.ToDecimal(txtDePara72.Text);
                reg.DeParaPct17 = Convert.ToDecimal(txtDeParaPct71.Text);

                if (txtCodigo.Text == "Novo")
                {
                    regDAL.Inserir(reg, ListaCidades);
                    Session["MensagemTela"] = "Regra de frete cadastrada com sucesso!";
                }
                else
                {
                    reg.CodigoIndex = Convert.ToInt32(txtCodigo.Text);
                    regDAL.Atualizar(reg, ListaCidades);
                    Session["MensagemTela"] = "Regra de frete alterada com sucesso!";
                }

                btnVoltar_Click(sender, e);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            RegraFreteDAL doc = new RegraFreteDAL();
            doc.Excluir(Convert.ToInt32(txtCodigo.Text));

            Session["MensagemTela"] = "Regra de frete excluída com sucesso!";
            btnVoltar_Click(sender, e);
        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }

        protected void txtDePara11_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara11.Text.Equals(""))
            {
                txtDePara11.Text = "0,00000";
            }
            else
            {
                v.CampoValido("De 1", txtDePara11.Text, true,true , false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara11.Text = Convert.ToDecimal(txtDePara11.Text).ToString("###,#####0.00000");
                    txtDePara12.Focus();
                }
                else
                    txtDePara11.Text = "0,00000";
            }
        }

        protected void txtDePara12_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara12.Text.Equals(""))
            {
                txtDePara12.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para", txtDePara12.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara12.Text = Convert.ToDecimal(txtDePara12.Text).ToString("###,#####0.00000");
                    txtDeParaPct12.Focus();
                }
                else
                    txtDePara12.Text = "0,00000";

            }
        }

        protected void txtDePara21_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara21.Text.Equals(""))
            {
                txtDePara21.Text = "0,00000";
            }
            else
            {
                v.CampoValido("De 2", txtDePara21.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara21.Text = Convert.ToDecimal(txtDePara21.Text).ToString("###,#####0.00000");
                    txtDePara22.Focus();
                }
                else
                    txtDePara21.Text = "0,00000";
            }
        }

        protected void txtDePara22_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara22.Text.Equals(""))
            {
                txtDePara22.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para 2", txtDePara22.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara22.Text = Convert.ToDecimal(txtDePara22.Text).ToString("###,#####0.00000");
                    txtDePara31.Focus();
                }
                else
                    txtDePara22.Text = "0,00000";

            }
        }

        protected void txtDePara31_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara31.Text.Equals(""))
            {
                txtDePara31.Text = "0,00000";
            }
            else
            {
                v.CampoValido("De 3", txtDePara31.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara31.Text = Convert.ToDecimal(txtDePara31.Text).ToString("###,#####0.00000");
                    txtDePara32.Focus();
                }
                else
                    txtDePara31.Text = "0,00000";

            }
        }

        protected void txtDePara32_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara32.Text.Equals(""))
            {
                txtDePara32.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para 3", txtDePara32.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara32.Text = Convert.ToDecimal(txtDePara32.Text).ToString("###,#####0.00000");
                    ddlEstado.Focus();
                }
                else
                    txtDePara32.Text = "0,00000";

            }
        }

        protected void txtFreteMinimo_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtFreteMinimo.Text.Equals(""))
            {
                txtFreteMinimo.Text = "0,00";
            }
            else
            {
                v.CampoValido("Frete minimo", txtFreteMinimo.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtFreteMinimo.Text = Convert.ToDecimal(txtFreteMinimo.Text).ToString("#,##0.00");
                    txtGRIS.Focus();
                }
                else
                    txtFreteMinimo.Text = "0,00";


            }
        }

        protected void txtGRIS_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtGRIS.Text.Equals(""))
            {
                txtGRIS.Text = "0,00";
            }
            else
            {
                v.CampoValido("GRIS", txtGRIS.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtGRIS.Text = Convert.ToDecimal(txtGRIS.Text).ToString("###,##0.00");
                    txtDeParaPct11.Focus();
                }
                else
                    txtGRIS.Text = "0,00";

            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            MunicipioDAL mun = new MunicipioDAL();
            ddlCidade.DataSource = mun.ListarMunicipios("M.CD_ESTADO", "SMALLINT", ddlEstado.SelectedValue, "");
            ddlCidade.DataTextField = "DescricaoMunicipio";
            ddlCidade.DataValueField = "CodigoMunicipio";
            ddlCidade.DataBind();

        }

        protected void txtDeParaPct11_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDeParaPct11.Text.Equals(""))
            {
                txtDeParaPct11.Text = "0,00";
            }
            else
            {
                v.CampoValido("De Percentual", txtDeParaPct11.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDeParaPct11.Text = Convert.ToDecimal(txtDeParaPct11.Text).ToString("###,##0.00");
                    txtDePara11.Focus();
                }
                else
                    txtDeParaPct11.Text = "0,00";

            }
        }

        protected void txtDeParaPct12_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDeParaPct12.Text.Equals(""))
            {
                txtDeParaPct12.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDeParaPct12.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDeParaPct12.Text = Convert.ToDecimal(txtDeParaPct12.Text).ToString("###,#####0.00000");
                    txtDePara21.Focus();
                }
                else
                    txtDeParaPct12.Text = "0,00000";

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var query = ListaCidades.Where(x => x.CodigoIBGE == Convert.ToInt32(ddlCidade.SelectedItem.Value));
            if(query.Count() == 0)
            {
                if(ddlTransp.SelectedValue == "* Nenhum selecionado")
                {
                    ShowMessage("Selecione uma transportadora", MessageType.Info);
                    return;
                }
                RegraFrete reg = new RegraFrete();
                RegraFreteDAL regDAL = new RegraFreteDAL();
                Pessoa_Inscricao ins = new Pessoa_Inscricao();
                PessoaInscricaoDAL insDAL = new PessoaInscricaoDAL();
                ins = insDAL.PesquisarPessoaInscricao(Convert.ToInt64(ddlTransp.SelectedValue), 1);
                reg = regDAL.PesquisarRegraFrete(ins._NumeroInscricao, ddlCidade.SelectedValue);
                if (reg == null || reg.CodigoIndex.ToString() == txtCodigo.Text)
                {
                    CidadeRegraFrete city = new CidadeRegraFrete();
                    city.CodigoIBGE = Convert.ToInt32(ddlCidade.SelectedItem.Value);
                    city.Cpl_DescricaoCidade = ddlCidade.SelectedItem.Text;
                    city.Cpl_CodigoEstado = Convert.ToInt32(ddlEstado.SelectedItem.Value);
                    city.Cpl_Sigla = ddlEstado.SelectedItem.Text;
                    ListaCidades.Add(city);

                    Session["ListaCidadesRegra"] = ListaCidades;
                    grdCidades.DataSource = ListaCidades;
                    grdCidades.DataBind();
                }
                else
                {
                    ShowMessage("Cidade já inclusa na região " + reg.Regiao, MessageType.Info);
                    return;
                }
            }
            else
            {
                ShowMessage("Cidades já inserida", MessageType.Info);
            }
        }

        protected void grdCidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCidades.PageIndex = e.NewPageIndex;
            // Carrega os dados
            grdCidades.DataBind();
            if (Session["ListaCidadesRegra"] != null)
            {
                ListaCidades = (List<CidadeRegraFrete>)Session["ListaCidadesRegra"];
                grdCidades.DataSource = ListaCidades;
                grdCidades.DataBind();
            }
        }

        protected void grdCidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CodigoItem = HttpUtility.HtmlDecode(grdCidades.SelectedRow.Cells[1].Text);
            List<CidadeRegraFrete> ListaNova = new List<CidadeRegraFrete>();
            foreach (CidadeRegraFrete item in ListaCidades)
            {
                if (item.CodigoIBGE != Convert.ToInt32(CodigoItem))
                {
                    ListaNova.Add(item);
                }
            }

            grdCidades.DataSource = ListaNova;
            grdCidades.DataBind();
            Session["ListaCidadesRegra"] = ListaNova;
        }

        protected void txtTipoCalculo_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtTipoCalculo.Text.Equals(""))
            {
                txtTipoCalculo.Text = "0";
            }
            else
            {
                v.CampoValido("Para percentual", txtTipoCalculo.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtTipoCalculo.Text = Convert.ToDecimal(txtTipoCalculo.Text).ToString("#0");
                }
                else
                    txtTipoCalculo.Text = "0";

            }
        }

        protected void txtPorTonelada_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtPorTonelada.Text.Equals(""))
            {
                txtPorTonelada.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtPorTonelada.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtPorTonelada.Text = Convert.ToDecimal(txtPorTonelada.Text).ToString("###,#####0.00000");
                    txtPorTonelada.Focus();
                }
                else
                    txtPorTonelada.Text = "0,00000";

            }
        }

        protected void txtCalcularADValor2_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCalcularADValor2.Text.Equals(""))
            {
                txtCalcularADValor2.Text = "0";
            }
            else
            {
                v.CampoValido("Para percentual", txtCalcularADValor2.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtCalcularADValor2.Text = Convert.ToDecimal(txtCalcularADValor2.Text).ToString("#0");
                    txtCalcularADValor2.Focus();
                }
                else
                    txtCalcularADValor2.Text = "0";

            }
        }

        protected void txtCalcularADValor1_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtCalcularADValor1.Text.Equals(""))
            {
                txtCalcularADValor1.Text = "0";
            }
            else
            {
                v.CampoValido("Para percentual", txtCalcularADValor1.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtCalcularADValor1.Text = Convert.ToDecimal(txtCalcularADValor1.Text).ToString("#0");
                    txtCalcularADValor1.Focus();
                }
                else
                    txtCalcularADValor1.Text = "0";

            }
        }

        protected void txtSeguroMinimo_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtSeguroMinimo.Text.Equals(""))
            {
                txtSeguroMinimo.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtSeguroMinimo.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtSeguroMinimo.Text = Convert.ToDecimal(txtSeguroMinimo.Text).ToString("###,#####0.00000");
                    txtSeguroMinimo.Focus();
                }
                else
                    txtSeguroMinimo.Text = "0,00000";

            }
        }

        protected void txtSeguro_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtSeguro.Text.Equals(""))
            {
                txtSeguro.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtSeguro.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtSeguro.Text = Convert.ToDecimal(txtSeguro.Text).ToString("###,#####0.00000");
                    txtSeguro.Focus();
                }
                else
                    txtSeguro.Text = "0,00000";

            }
        }

        protected void txtExcedente2_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtExcedente2.Text.Equals(""))
            {
                txtExcedente2.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtExcedente2.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtExcedente2.Text = Convert.ToDecimal(txtExcedente2.Text).ToString("###,#####0.00000");
                    txtExcedente2.Focus();
                }
                else
                    txtExcedente2.Text = "0,00000";

            }
        }

        protected void txtExcedente1_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtExcedente1.Text.Equals(""))
            {
                txtExcedente1.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtExcedente1.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtExcedente1.Text = Convert.ToDecimal(txtExcedente1.Text).ToString("###,#####0.00000");
                    txtExcedente1.Focus();
                }
                else
                    txtExcedente1.Text = "0,00000";

            }
        }

        protected void txtPedagioMaximo_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtPedagioMaximo.Text.Equals(""))
            {
                txtPedagioMaximo.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtPedagioMaximo.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtPedagioMaximo.Text = Convert.ToDecimal(txtPedagioMaximo.Text).ToString("###,#####0.00000");
                    txtPedagioMaximo.Focus();
                }
                else
                    txtPedagioMaximo.Text = "0,00000";

            }

        }

        protected void txtADValor_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtADValor.Text.Equals(""))
            {
                txtADValor.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtADValor.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtADValor.Text = Convert.ToDecimal(txtADValor.Text).ToString("###,#####0.00000");
                    txtADValor.Focus();
                }
                else
                    txtADValor.Text = "0,00000";

            }
        }

        protected void txtPedagio_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtPedagio.Text.Equals(""))
            {
                txtPedagio.Text = "0,00";
            }
            else
            {
                v.CampoValido("Para percentual", txtPedagio.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtPedagio.Text = Convert.ToDecimal(txtPedagio.Text).ToString("###,##0.00");
                    txtPedagio.Focus();
                }
                else
                    txtPedagio.Text = "0,00";

            }
        }

        protected void txtGRISMinimo_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtGRISMinimo.Text.Equals(""))
            {
                txtGRISMinimo.Text = "0,00";
            }
            else
            {
                v.CampoValido("Para percentual", txtGRISMinimo.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtGRISMinimo.Text = Convert.ToDecimal(txtGRISMinimo.Text).ToString("###,##0.00");
                    txtGRISMinimo.Focus();
                }
                else
                    txtGRISMinimo.Text = "0,00";

            }
        }

        protected void txtDeParaPct31_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDeParaPct31.Text.Equals(""))
            {
                txtDeParaPct31.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDeParaPct31.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDeParaPct31.Text = Convert.ToDecimal(txtDeParaPct31.Text).ToString("###,#####0.00000");
                    txtDeParaPct31.Focus();
                }
                else
                    txtDeParaPct31.Text = "0,00000";

            }
        }

        protected void txtDeParaPct41_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDeParaPct41.Text.Equals(""))
            {
                txtDeParaPct41.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDeParaPct41.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDeParaPct41.Text = Convert.ToDecimal(txtDeParaPct41.Text).ToString("###,#####0.00000");
                    txtDeParaPct41.Focus();
                }
                else
                    txtDeParaPct41.Text = "0,00000";

            }
        }

        protected void txtDePara41_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara41.Text.Equals(""))
            {
                txtDePara41.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDePara41.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara41.Text = Convert.ToDecimal(txtDePara41.Text).ToString("###,#####0.00000");
                    txtDePara41.Focus();
                }
                else
                    txtDePara41.Text = "0,00000";

            }
        }

        protected void txtDePara42_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara42.Text.Equals(""))
            {
                txtDePara42.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDePara42.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara42.Text = Convert.ToDecimal(txtDePara42.Text).ToString("###,#####0.00000");
                    txtDePara42.Focus();
                }
                else
                    txtDePara42.Text = "0,00000";

            }
        }

        protected void txtDeParaPct51_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDeParaPct51.Text.Equals(""))
            {
                txtDeParaPct51.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDeParaPct51.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDeParaPct51.Text = Convert.ToDecimal(txtDeParaPct51.Text).ToString("###,#####0.00000");
                    txtDeParaPct51.Focus();
                }
                else
                    txtDeParaPct51.Text = "0,00000";

            }
        }

        protected void txtDePara51_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara51.Text.Equals(""))
            {
                txtDePara51.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDePara51.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara51.Text = Convert.ToDecimal(txtDePara51.Text).ToString("###,#####0.00000");
                    txtDePara51.Focus();
                }
                else
                    txtDePara51.Text = "0,00000";

            }
        }

        protected void txtDePara52_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara52.Text.Equals(""))
            {
                txtDePara52.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDePara52.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara52.Text = Convert.ToDecimal(txtDePara52.Text).ToString("###,#####0.00000");
                    txtDePara52.Focus();
                }
                else
                    txtDePara52.Text = "0,00000";

            }
        }

        protected void txtDeParaPct61_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDeParaPct61.Text.Equals(""))
            {
                txtDeParaPct61.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDeParaPct61.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDeParaPct61.Text = Convert.ToDecimal(txtDeParaPct61.Text).ToString("###,#####0.00000");
                    txtDeParaPct61.Focus();
                }
                else
                    txtDeParaPct61.Text = "0,00000";

            }
        }

        protected void txtDePara61_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara61.Text.Equals(""))
            {
                txtDePara61.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDePara61.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara61.Text = Convert.ToDecimal(txtDePara61.Text).ToString("###,#####0.00000");
                    txtDePara61.Focus();
                }
                else
                    txtDePara61.Text = "0,00000";

            }
        }

        protected void txtDePara62_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara62.Text.Equals(""))
            {
                txtDePara62.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDePara62.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara62.Text = Convert.ToDecimal(txtDePara62.Text).ToString("###,#####0.00000");
                    txtDePara62.Focus();
                }
                else
                    txtDePara62.Text = "0,00000";

            }
        }

        protected void txtDeParaPct71_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDeParaPct71.Text.Equals(""))
            {
                txtDeParaPct71.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDeParaPct71.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDeParaPct71.Text = Convert.ToDecimal(txtDeParaPct71.Text).ToString("###,#####0.00000");
                    txtDeParaPct71.Focus();
                }
                else
                    txtDeParaPct71.Text = "0,00000";

            }
        }

        protected void txtDePara71_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara71.Text.Equals(""))
            {
                txtDePara71.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDePara71.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara71.Text = Convert.ToDecimal(txtDePara71.Text).ToString("###,#####0.00000");
                    txtDePara71.Focus();
                }
                else
                    txtDePara71.Text = "0,00000";

            }
        }

        protected void txtDePara72_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtDePara72.Text.Equals(""))
            {
                txtDePara72.Text = "0,00000";
            }
            else
            {
                v.CampoValido("Para percentual", txtDePara72.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtDePara72.Text = Convert.ToDecimal(txtDePara72.Text).ToString("###,#####0.00000");
                    txtDePara72.Focus();
                }
                else
                    txtDePara72.Text = "0,00000";

            }
        }

        protected void txtPesoCubado_TextChanged(object sender, EventArgs e)
        {
            Boolean blnCampo = false;

            if (txtPesoCubado.Text.Equals(""))
            {
                txtPesoCubado.Text = "000";
            }
            else
            {
                v.CampoValido("Para percentual", txtPesoCubado.Text, true, true, false, false, "", ref blnCampo, ref strMensagemR);
                if (blnCampo)
                {
                    txtPesoCubado.Text = Convert.ToDecimal(txtPesoCubado.Text).ToString("###000");
                    txtPesoCubado.Focus();
                }
                else
                    txtPesoCubado.Text = "000";

            }
        }
    }
}