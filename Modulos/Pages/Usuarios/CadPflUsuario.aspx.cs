using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Usuarios
{
    public partial class CadPflUsuario : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";

        protected void ApresentaMensagem(String strMensagem)
        {
            lblMensagem.Text = strMensagem;
            pnlMensagem.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        protected void CarregaDropDownList()
        {
            ModuloSistemaDAL ms = new ModuloSistemaDAL();
            ddlModuloEspecifico.DataSource = ms.ListarModulosSistema("","","","");
            ddlModuloEspecifico.DataTextField = "DescricaoModulo";
            ddlModuloEspecifico.DataValueField = "CodigoModulo";
            ddlModuloEspecifico.DataBind();
            ddlModuloEspecifico.Items.Insert(0, "* Nenhum específico (Acesso a todos disponíveis)");
        }
        protected Boolean ValidaPerfilUsuario()
        {
            Boolean blnCampoValido = false;
            if (txtCodigo.Enabled)
            {
                
                v.CampoValido("Código do Perfil de Usuário", txtCodigo.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    txtCodigo.Text = "";
                    if (strMensagemR != "")
                        ApresentaMensagem(strMensagemR);
                    return false;
                }
                PerfilUsuarioDAL d = new PerfilUsuarioDAL();
                PerfilUsuario p = new PerfilUsuario();
                p = d.PesquisarPerfilUsuario(Convert.ToInt32(txtCodigo.Text));

                if (p != null)
                {
                    ApresentaMensagem("Código do Perfil já Cadastrado.");
                    return false;
                }

            }
            v.CampoValido("Hora Inicial", txtHorarioInicial.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ApresentaMensagem(strMensagemR);
                    txtHorarioInicial.Focus();
                }
                return false;
            }
            v.CampoValido("Hora Final", txtHorarioFinal.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ApresentaMensagem(strMensagemR);
                    txtHorarioFinal.Focus();
                }
                return false;
            }
            
            v.CampoValido("Hora inicio intervalo 1", txtIntervaloInicio.Text, false, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ApresentaMensagem(strMensagemR);
                    txtIntervaloInicio.Focus();
                }
                return false;
            }
            else if (txtIntervaloInicio.Text != "")
            {
                v.CampoValido("Hora final intervalo 1", txtIntervaloFim.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ApresentaMensagem(strMensagemR);
                        txtIntervaloFim.Focus();
                    }
                    return false;
                }
            }
            else
            {
                v.CampoValido("Hora final intervalo 1", txtIntervaloFim.Text, false, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ApresentaMensagem(strMensagemR);
                        txtIntervaloFim.Focus();
                    }
                    return false;
                }
            }
            v.CampoValido("Hora inicio intervalo 2", txtIntervaloInicio2.Text, false, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ApresentaMensagem(strMensagemR);
                    txtIntervaloInicio2.Focus();
                }
                return false;
            }
            else if (txtIntervaloInicio2.Text != "")
            {
                v.CampoValido("Hora final intervalo 2", txtIntervaloFim2.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ApresentaMensagem(strMensagemR);
                        txtIntervaloFim2.Focus();
                    }
                    return false;
                }
            }
            else
            {
                v.CampoValido("Hora final intervalo 2", txtIntervaloFim2.Text, false, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ApresentaMensagem(strMensagemR);
                        txtIntervaloFim2.Focus();
                    }
                    return false;
                }
            }
            v.CampoValido("Hora inicio intervalo 3", txtIntervaloInicio3.Text, false, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ApresentaMensagem(strMensagemR);
                    txtIntervaloInicio3.Focus();
                }
                return false;
            }
            else if (txtIntervaloInicio3.Text != "")
            {
                v.CampoValido("Hora final intervalo 3", txtIntervaloFim3.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ApresentaMensagem(strMensagemR);
                        txtIntervaloFim3.Focus();
                    }
                    return false;
                }
            }
            else
            {
                v.CampoValido("Hora final intervalo 3", txtIntervaloFim3.Text, false, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ApresentaMensagem(strMensagemR);
                        txtIntervaloFim3.Focus();
                    }
                    return false;
                }
            }


            v.CampoValido("Data/Hora inicio acesso Diário", txtDtHrInicio.Text, false, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ApresentaMensagem(strMensagemR);
                    txtDtHrInicio.Focus();
                }
                return false;
            }
            else if(txtDtHrInicio.Text != "")
            {
                v.CampoValido("Data/Hora fim acesso Diário", txtDtHrFim.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ApresentaMensagem(strMensagemR);
                        txtDtHrFim.Focus();
                    }
                    return false;
                }
            }
            else
            {

                v.CampoValido("Data/Hora Fim Diário", txtDtHrFim.Text, false, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {

                    if (strMensagemR != "")
                    {
                        ApresentaMensagem(strMensagemR);
                        txtDtHrFim.Focus();
                    }
                    return false;
                }
            }
            
            if(txtHorarioInicial.Text == txtHorarioFinal.Text)
            {
                ApresentaMensagem("Horário inicial e final devem ser diferentes");
                return false;
            }
            if (txtDtHrInicio.Text == txtDtHrFim.Text && txtDtHrInicio.Text != "")
            {
                ApresentaMensagem("Horário inicial e final do acesso diário devem ser diferentes");
                return false;
            }
            if (txtIntervaloInicio.Text == txtIntervaloFim.Text && txtIntervaloInicio.Text != "")
            {
                ApresentaMensagem("Horário inicial e final do intervalo 1 devem ser diferentes");
                return false;
            }
            if (txtIntervaloInicio2.Text == txtIntervaloFim2.Text && txtIntervaloInicio2.Text != "")
            {
                ApresentaMensagem("Horário inicial e final do intervalo 2 devem ser diferentes");
                return false;
            }
            if (txtIntervaloInicio3.Text == txtIntervaloFim3.Text && txtIntervaloInicio3.Text != "")
            {
                ApresentaMensagem("Horário inicial e final do intervalo 3 devem ser diferentes");
                return false;
            }
            if (!ChkDomingo.Checked && !ChkSegunda.Checked && !ChkTerca.Checked && !ChkQuarta.Checked && !ChkQuinta.Checked && !ChkSexta.Checked && !ChkSabado.Checked)
            {
                ApresentaMensagem("Marque pelo menos um dia da semana");
                return false;
            }
            return true;

        }
        protected Boolean ValidaDescricao()
        {
            Boolean blnCampoValido = false;
            v.CampoValido("Descrição do Perfil de Usuário", txtDescricao.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                    ApresentaMensagem(strMensagemR);
                return false;
            }
            return true;
        }
        protected Boolean ValidaCampos()
        {
            if (!ValidaPerfilUsuario()) return false;
            if (!ValidaDescricao()) return false;
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["TabFocada"] != null)
            {
                PanelSelect = Session["TabFocada"].ToString();
                Session["TabFocada"] = null;
            }
            else
            {
                PanelSelect = "home";
                Session["TabFocada"] = "home";
            }
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                           Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                           "ConPflUsuario.aspx");

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomPflUsuario2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomPflUsuario"] != null)
                {
                    string s = Session["ZoomPflUsuario"].ToString();
                    Session["ZoomPflUsuario"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodigo.Text == "")
                            {
                                txtCodigo.Text = word;
                                txtCodigo.Enabled = false;

                                CarregaDropDownList();
                                PerfilUsuarioDAL r = new PerfilUsuarioDAL();
                                PerfilUsuario p = new PerfilUsuario();

                                p = r.PesquisarPerfilUsuario(Convert.ToInt32(txtCodigo.Text));

                                txtDescricao.Text = p.DescricaoPflUsuario;

                                txtHorarioInicial.Text = p.HoraInicial.ToString("HH:mm");
                                txtHorarioFinal.Text = p.HoraFinal.ToString("HH:mm");

                                if (p.DataHoraInicio.HasValue)
                                    txtDtHrInicio.Text = p.DataHoraInicio.Value.ToShortTimeString();

                                if (p.DataHoraFim.HasValue)
                                    txtDtHrFim.Text = p.DataHoraFim.Value.ToShortTimeString();

                                if (p.IntervaloInicio1.HasValue)
                                    txtIntervaloInicio.Text = p.IntervaloInicio1.Value.ToShortTimeString();

                                if (p.IntervaloFim1.HasValue)
                                    txtIntervaloFim.Text = p.IntervaloFim1.Value.ToShortTimeString();

                                if (p.IntervaloInicio2.HasValue)
                                    txtIntervaloInicio2.Text = p.IntervaloInicio2.Value.ToShortTimeString();

                                if (p.IntervaloFim2.HasValue)
                                    txtIntervaloFim2.Text = p.IntervaloFim2.Value.ToShortTimeString();

                                if (p.IntervaloInicio3.HasValue)
                                    txtIntervaloInicio3.Text = p.IntervaloInicio3.Value.ToShortTimeString();

                                if (p.IntervaloFim3.HasValue)
                                    txtIntervaloFim3.Text = p.IntervaloFim3.Value.ToShortTimeString();

                                ChkDomingo.Checked = p.Domingo;
                                ChkSegunda.Checked = p.Segunda;
                                ChkTerca.Checked = p.Terca;
                                ChkQuarta.Checked = p.Quarta;
                                ChkQuinta.Checked = p.Quinta;
                                ChkSexta.Checked = p.Sexta;
                                ChkSabado.Checked = p.Sabado;
                                ddlModuloEspecifico.SelectedValue = p.CodigoModuloEspecifico.ToString();

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
                                PerfilUsuarioEmpresaDAL s1 = new PerfilUsuarioEmpresaDAL();
                                grdPermissao.DataSource = s1.ListarPerfilUsuarioEmpresa(p.CodigoPflUsuario);
                                grdPermissao.DataBind();
                                if (Session["CodUsuario"].ToString() == "-150380")
                                {
                                    btnSalvar.Visible = true;
                                    if (txtCodigo.Text != "")
                                        btnExcluir.Visible = true;

                                }
                                PanelSelect = "home";
                                return;
                            }
                    }
                }
                else
                {
                    txtCodigo.Focus();
                    CarregaDropDownList();
                    btnExcluir.Visible = false;
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                        }
                    });
                    if (Session["CodUsuario"].ToString() == "-150380")
                    {
                        btnSalvar.Visible = true;
                        if (txtCodigo.Text != "")
                            btnExcluir.Visible = true;

                    }
                }
            }
            if (ddlModuloEspecifico.Items.Count == 0)
                btnVoltar_Click(sender, e);

        }
        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    PerfilUsuarioDAL d = new PerfilUsuarioDAL();
                    d.Excluir(Convert.ToInt32(txtCodigo.Text));
                    Session["MensagemTela"] = "Perfil Excluído com Sucesso!!!";
                    btnVoltar_Click(sender, e);
                }

            }
            catch (Exception ex)
            {
                btnCfmNao_Click(sender, e);
                ApresentaMensagem(ex.Message.ToString());
            }

        }
        protected void btnCfmNao_Click(object sender, EventArgs e)
        {
            pnlExcluir.Visible = false;
            pnlMensagem.Visible = false;
        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCodigo.Text.Trim() != "")
                    pnlExcluir.Visible = true;
            }
            catch (Exception ex)
            {
                btnCfmNao_Click(sender, e);
                ApresentaMensagem(ex.Message.ToString());
            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomPflUsuario"] = null;
            if (Session["ZoomPflUsuario2"] != null)
            {
                Session["ZoomPflUsuario2"] = null;
                Session["MensagemTela"] = null;
                string _open = "window.close('CadPflUsuario.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                return;
            }

            Response.Redirect("~/Pages/Usuarios/ConPflUsuario.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos() == false)
                    return;


                PerfilUsuarioDAL d = new PerfilUsuarioDAL();
                PerfilUsuario p = new PerfilUsuario();

                p.CodigoPflUsuario = Convert.ToInt32(txtCodigo.Text);
                p.DescricaoPflUsuario = txtDescricao.Text.ToUpper();

                p.HoraInicial = Convert.ToDateTime(txtHorarioInicial.Text);
                p.HoraFinal= Convert.ToDateTime(txtHorarioFinal.Text);

                if(txtIntervaloInicio.Text != "")
                    p.IntervaloInicio1 = Convert.ToDateTime(txtIntervaloInicio.Text);

                if (txtIntervaloFim.Text != "")
                    p.IntervaloFim1 = Convert.ToDateTime(txtIntervaloFim.Text);

                if (txtIntervaloInicio2.Text != "")
                    p.IntervaloInicio2 = Convert.ToDateTime(txtIntervaloInicio2.Text);

                if (txtIntervaloFim2.Text != "")
                    p.IntervaloFim2 = Convert.ToDateTime(txtIntervaloFim2.Text);

                if (txtIntervaloInicio3.Text != "")
                    p.IntervaloInicio3 = Convert.ToDateTime(txtIntervaloInicio3.Text);

                if (txtIntervaloFim3.Text != "")
                    p.IntervaloFim3 = Convert.ToDateTime(txtIntervaloFim3.Text);

                if (txtDtHrInicio.Text != "")
                    p.DataHoraInicio = Convert.ToDateTime(txtDtHrInicio.Text);

                if (txtDtHrFim.Text != "")
                    p.DataHoraFim = Convert.ToDateTime(txtDtHrFim.Text);

                p.Domingo = ChkDomingo.Checked;
                p.Segunda = ChkSegunda.Checked;
                p.Terca = ChkTerca.Checked;
                p.Quarta = ChkQuarta.Checked;
                p.Quinta = ChkQuinta.Checked;
                p.Sexta = ChkSexta.Checked;
                p.Sabado = ChkSabado.Checked;


                if (ddlModuloEspecifico.SelectedValue != "* Nenhum específico (Acesso a todos disponíveis)")
                {
                    p.CodigoModuloEspecifico = Convert.ToInt32(ddlModuloEspecifico.SelectedValue);
                    if (Session["CodPflUsuario"].ToString() == txtCodigo.Text)
                    {
                        Session["CodigoModuloEspecifico"] = p.CodigoModuloEspecifico.ToString();
                        Session["CodModulo"] = p.CodigoModuloEspecifico.ToString();
                        Session["DesModulo"] = ddlModuloEspecifico.SelectedItem.Text;
                    }
                }
                else
                {
                    if (Session["CodPflUsuario"].ToString() == txtCodigo.Text)
                    {
                        Session["CodigoModuloEspecifico"] = null;
                    }
                }

                if (d.PesquisarPerfilUsuario(p.CodigoPflUsuario) == null)
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Perfil Incluído com Sucesso!!!";
                }
                else
                {
                    d.Atualizar(p);
                    Session["MensagemTela"] = "Perfil Alterado com Sucesso!!!";
                }
                //
                PerfilUsuarioEmpresaDAL pue = new PerfilUsuarioEmpresaDAL();

                foreach (GridViewRow row in grdPermissao.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkLiberado");

                    if (chk.Checked == false)
                        pue.ExcluirPermissao(Convert.ToInt64(row.Cells[1].Text), Convert.ToInt64(txtCodigo.Text));
                    else
                    {
                        pue.SalvarPermissao(Convert.ToInt64(row.Cells[1].Text), Convert.ToInt64(txtCodigo.Text));
                    }
                }

                btnVoltar_Click(sender, e);


            }
            catch (Exception ex)
            {
                btnCfmNao_Click(sender, e);
                ApresentaMensagem(ex.Message.ToString());
            }

        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Relatorios/RelPflUsuario.aspx");

        }
        protected void btnMensagem_Click(object sender, EventArgs e)
        {
            //Botão do OK da Mensagem

        }
    }
}