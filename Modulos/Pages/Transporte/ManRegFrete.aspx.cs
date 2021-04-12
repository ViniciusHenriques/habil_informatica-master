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
                                    txtDePara11.Text = reg.DePara11.ToString();
                                    txtDePara12.Text = reg.DePara12.ToString();
                                    txtDePara21.Text = reg.DePara21.ToString();
                                    txtDePara22.Text = reg.DePara22.ToString();
                                    txtDePara31.Text = reg.DePara31.ToString();
                                    txtDePara32.Text = reg.DePara32.ToString();
                                    txtDeParaPct11.Text = reg.DeParaPct11.ToString();
                                    txtDeParaPct12.Text = reg.DeParaPct12.ToString();
                                    txtFreteMinimo.Text = reg.ValorFreteMinimo.ToString();
                                    txtGRIS.Text = reg.ValorGRIS.ToString();
                                    txtRegiao.Text = reg.Regiao.ToString();

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

                reg.DePara11 = Convert.ToDecimal(txtDePara11.Text);
                reg.DePara12 = Convert.ToDecimal(txtDePara12.Text);
                reg.DeParaPct11 = Convert.ToDecimal(txtDeParaPct11.Text);

                reg.DePara21 = Convert.ToDecimal(txtDePara21.Text);
                reg.DePara22 = Convert.ToDecimal(txtDePara22.Text);
                reg.DeParaPct12 = Convert.ToDecimal(txtDeParaPct12.Text);
                
                reg.DePara31 = Convert.ToDecimal(txtDePara31.Text);
                reg.DePara32 = Convert.ToDecimal(txtDePara32.Text);

                if(txtCodigo.Text == "Novo")
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
    }
}