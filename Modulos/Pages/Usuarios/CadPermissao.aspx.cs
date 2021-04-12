using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Persistence;
using DAL.Model;
using System.Data.Common;
namespace SoftHabilInformatica.Pages.Usuarios
{
    public partial class CadPermissao : System.Web.UI.Page
    {
        
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void ApresentaMensagem(String strMensagem)
        {
            lblMensagem.Text = strMensagem;
            pnlMensagem.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        protected Boolean ValidaPerfil()
        {

            Boolean blnCampoValido = false;
            v.CampoValido("Código do Perfil", txtCodPerfil.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                    ApresentaMensagem(strMensagemR);
                return false;
            }
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
                return;
            }



            if ((DdlModulo.Text == "") || (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath))
            {

                if (Session["CodModulo"] != null)
                {
                    if  (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                    {
                        Session["ZoomPflUsuario"] = null;
                        Session["Pagina"] = Request.CurrentExecutionFilePath;
                    }
                    txtCodPerfil.Focus();

                    btnSalvar.Visible = false;

                    ModuloSistemaDAL msdal = new ModuloSistemaDAL();

                    DdlModulo.DataSource = msdal.ListarModulosSistema("", "", "", "CD_MODULO_SISTEMA");
                    DdlModulo.DataTextField = "DescricaoModulo";
                    DdlModulo.DataValueField = "CodigoModulo";
                    DdlModulo.DataBind();

                    Session["Operacao"] = "EDICAO";
                }

            }

            else
            {

                if ((Session["ZoomPflUsuario"] != null) && (Session["Operacao"].ToString() == "EDICAO"))
                {

                    string s = Session["ZoomPflUsuario"].ToString();
                    string[] words = s.Split('²');

                    if (s != "³")
                    {
                        txtCodPerfil.Text = "";
                        txtDcrPerfil.Text = "";

                        foreach (string word in words)
                        {
                            if (txtCodPerfil.Text == "")
                                txtCodPerfil.Text = word;
                            else
                                if (txtDcrPerfil.Text == "")
                                    txtDcrPerfil.Text = word;
                        }

                        txtCodPerfil_TextChanged (sender, e);

                        Session["ZoomPflUsuario"] = null;
                    }
                }
            }


        }
        protected void txtCodPerfil_TextChanged(object sender, EventArgs e)
        {
            btnSalvar.Visible = false;
            PermissaoDAL r = new PermissaoDAL();
            grdPermissao.DataSource = r.ListarPerfilUsuario(-1, -1,"");
            grdPermissao.DataBind();

            if (ValidaPerfil())
            {
                PerfilUsuarioDAL d = new PerfilUsuarioDAL();
                PerfilUsuario p = new PerfilUsuario();
                p = d.PesquisarPerfilUsuario(Convert.ToInt32(txtCodPerfil.Text));
                txtDcrPerfil.Text = "";

                if (p != null)
                    txtDcrPerfil.Text = p.DescricaoPflUsuario;
                else
                    ApresentaMensagem("Perfil não cadastrado");
            }
            else
            {
                txtDcrPerfil.Text = "";
            }
        }
        protected void grdPermissao_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblHistorico.Text = "";
            string strCodigo = HttpUtility.HtmlDecode(grdPermissao.SelectedRow.Cells[1].Text);

            MenuSistemaDAL p = new MenuSistemaDAL();
            MenuSistema u = new MenuSistema();

            u = p.PesquisarMenuSistema(Convert.ToInt32(strCodigo));

            lblHistorico.Text = u.TextoAjuda;

            pnlHistorico.Visible = true;
            //pnlConsulta.Visible = false; 

        }
        protected void grdPermissao_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkBox = (CheckBox)e.Row.FindControl("chkLiberado");
                CheckBox chkBox1 = (CheckBox)e.Row.FindControl("chkAcessoCompleto");

                if (chkBox.Checked == false)
                {
                    chkBox1.Enabled = false;
                }
                else
                {
                    chkBox1.Enabled = true;
                }

            }
        }
        protected void btnConfirma_Click(object sender, EventArgs e)
        {

        }
        protected void chkAcessoCompleto_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow row in grdPermissao.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLiberado");
                CheckBox chk2 = (CheckBox)row.FindControl("chkAcessoCompleto");
                if (chk.Checked == false)
                    chk2.Checked = chk.Checked; 
                
            }

        }
        protected void chkLiberado_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdPermissao.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLiberado");
                CheckBox chk2 = (CheckBox)row.FindControl("chkAcessoCompleto");
                CheckBox chk3 = (CheckBox)row.FindControl("chkConsulta");
                CheckBox chk4 = (CheckBox)row.FindControl("chkRelatorio");
                CheckBox chk5 = (CheckBox)row.FindControl("chkImprimir");
                CheckBox chk6 = (CheckBox)row.FindControl("chkIncluir");
                CheckBox chk7 = (CheckBox)row.FindControl("chkAlterar");
                CheckBox chk8 = (CheckBox)row.FindControl("chkExcluir");
                CheckBox chk9 = (CheckBox)row.FindControl("chkDownload");
                CheckBox chk10 = (CheckBox)row.FindControl("chkUpload");
                CheckBox chk11 = (CheckBox)row.FindControl("chkExclusaoAnexo");
                CheckBox chk12 = (CheckBox)row.FindControl("chkEspecial1");
                CheckBox chk13 = (CheckBox)row.FindControl("chkEspecial2");
                CheckBox chk14 = (CheckBox)row.FindControl("chkEspecial3");

                if (chk.Checked == false)
                {
                    chk2.Checked = chk.Checked;
                    chk2.Enabled = chk.Checked;
                    chk3.Enabled = chk.Checked;
                    chk4.Enabled = chk.Checked;
                    chk5.Enabled = chk.Checked;
                    chk6.Enabled = chk.Checked;
                    chk7.Enabled = chk.Checked;
                    chk8.Enabled = chk.Checked;
                    chk9.Enabled = chk.Checked;
                    chk10.Enabled = chk.Checked;
                    chk11.Enabled = chk.Checked;
                    chk12.Enabled = chk.Checked;
                    chk13.Enabled = chk.Checked;
                    chk14.Enabled = chk.Checked;
                }
                else
                {
                    chk2.Enabled = chk.Checked;

                    if (  row.Cells[3].Text  == "Cadastro")
                    {
                        chk3.Enabled = true;
                        chk4.Enabled = true;
                        chk5.Enabled = true;
                        chk6.Enabled = true;
                        chk7.Enabled = true;
                        chk8.Enabled = true;
                        chk9.Enabled = true;
                        chk10.Enabled = true;
                        chk11.Enabled = true;
                        chk12.Enabled = true;
                        chk13.Enabled = true;
                        chk14.Enabled = true;
                    }
                    else
                    {
                        chk3.Enabled = false;
                        chk4.Enabled = false;
                        chk5.Enabled = false;
                        chk6.Enabled = false;
                        chk7.Enabled = false;
                        chk8.Enabled = false;
                        chk9.Enabled = false;
                        chk10.Enabled = false;
                        chk11.Enabled = false;
                        chk12.Enabled = false;
                        chk13.Enabled = false;
                        chk14.Enabled = false;
                    }
                }
            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            PermissaoDAL p = new PermissaoDAL();
            String strIndicadores = "";

            foreach (GridViewRow row in grdPermissao.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLiberado");
                CheckBox chk2 = (CheckBox)row.FindControl("chkAcessoCompleto");
                CheckBox chk3 = (CheckBox)row.FindControl("chkConsulta");
                CheckBox chk4 = (CheckBox)row.FindControl("chkRelatorio");
                CheckBox chk5 = (CheckBox)row.FindControl("chkImprimir");
                CheckBox chk6 = (CheckBox)row.FindControl("chkIncluir");
                CheckBox chk7 = (CheckBox)row.FindControl("chkAlterar");
                CheckBox chk8 = (CheckBox)row.FindControl("chkExcluir");
                CheckBox chk9 = (CheckBox)row.FindControl("chkDownload");
                CheckBox chk10 = (CheckBox)row.FindControl("chkUpload");
                CheckBox chk11 = (CheckBox)row.FindControl("chkExclusaoAnexo");
                CheckBox chk12 = (CheckBox)row.FindControl("chkEspecial1");
                CheckBox chk13 = (CheckBox)row.FindControl("chkEspecial2");
                CheckBox chk14 = (CheckBox)row.FindControl("chkEspecial3");


                if (chk.Checked == false)
                {
                    p.ExcluirPermissao(Convert.ToInt32(row.Cells[1].Text), Convert.ToInt32(txtCodPerfil.Text));
                }
                else
                {
                    if (chk.Checked == true)
                        strIndicadores = "1";
                    else
                        strIndicadores = "0";

                    if (chk2.Checked == true)
                        strIndicadores = strIndicadores  + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk3.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk4.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk5.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk6.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk7.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk8.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk9.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk10.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk11.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk12.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk13.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    if (chk14.Checked == true)
                        strIndicadores = strIndicadores + "1";
                    else
                        strIndicadores = strIndicadores + "0";

                    p.SalvarPermissao(Convert.ToInt32(row.Cells[1].Text), Convert.ToInt32(txtCodPerfil.Text), strIndicadores);
                }

            }
            ShowMessage("Permissões Atualizadas com Sucesso.", MessageType.Success);
       }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            pnlHistorico.Visible = false;
            pnlConsulta.Visible = true; 

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            btnSalvar.Visible = false;
            PermissaoDAL r = new PermissaoDAL();

            if (txtCodPerfil.Text != "")
                grdPermissao.DataSource = r.ListarPerfilUsuario(Convert.ToInt32(DdlModulo.SelectedValue), Convert.ToInt32(txtCodPerfil.Text),"");
            else
                grdPermissao.DataSource = r.ListarPerfilUsuario(-1, -1, "");

            grdPermissao.DataBind();
            if (grdPermissao.Rows.Count == 0)
            {
                ShowMessage("Perfil não cadastrado.", MessageType.Info);
                lblChkAll.Visible = false;
                chkAll.Visible = false;
                lblAcessoAll.Visible = false;
                chkAcessoAll.Visible = false;
            }
            else
            {
                btnSalvar.Visible = true;
                lblChkAll.Visible = true;
                chkAll.Visible = true;
                lblAcessoAll.Visible = true;
                chkAcessoAll.Visible = true;

            }


        }
        protected void btnCfmNao2_Click(object sender, EventArgs e)
        {
            pnlMensagem.Visible = false;
        }
        protected void btnMensagem_Click(object sender, EventArgs e)
        {

        }
        protected void DdlModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(txtCodPerfil.Text != "")
                btnBuscar_Click(sender, e);
            chkAll.Checked = false;
            chkAcessoAll.Checked = false;
        }
        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkMes;

            foreach (GridViewRow grvRow in grdPermissao.Rows)
            {
                chkMes = (CheckBox)grvRow.FindControl("chkLiberado");
                if (chkAll.Checked)
                {
                    chkMes.Checked = true;
                    if (chkAcessoAll.Checked)
                    {
                        chkAcessoAll_CheckedChanged(sender, e);
                    }
                }
                else
                {
                    chkMes.Checked = false;
                }
                chkLiberado_CheckedChanged(sender, e);

            }
        }

        protected void chkAcessoAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkMes;

            foreach (GridViewRow grvRow in grdPermissao.Rows)
            {
                chkMes = (CheckBox)grvRow.FindControl("chkAcessoCompleto");
                if (chkAcessoAll.Checked)
                {
                    chkMes.Checked = true;
                    
                }
                else
                {
                    chkMes.Checked = false;
                }

            }
        }
    }
}