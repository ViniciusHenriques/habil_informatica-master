using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SoftHabilInformatica.Pages.Compromissos
{
    public partial class ManAgendamento : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();

        public enum MessageType { Success, Error, Info, Warning };

        public string Agendamentos;

        List<UsuarioAgendamento> ListaUsuarios = new List<UsuarioAgendamento>();

        String strMensagemR = "";

        clsValidacao v = new clsValidacao();

        protected void LimpaCampos(bool LimparSession)
        {          
            txtCodigo.Text = "Novo";
            txtContato.Text = "";
            txtCodPessoa.Text = "";
            txtPessoa.Text = "";
            txtLocal.Text = "";
            txtTelefone.Text = "";
            txtHora.Text = "";
            txtTelefone.Text = "";
            txtAnotacao.Text = "";
            ddlSituacao.SelectedValue = "168";
            lblCorLembrete.Text = "";
            chkEmail.Checked = true;

            if (LimparSession)
            {
                Session["ListaUsuariosPermitidos"] = null;
                Session["NovoAnexo"] = null;
                ListaAnexo.Clear();
                grdAnexo.DataSource = ListaAnexo;
                grdAnexo.DataBind();
            }

            CarregaSituacao();
        }

        protected bool ValidaCampos()
        {
            try
            { 
                Boolean blnCampoValido = false;

                if (txtData.Text == "")
                {
                    ShowMessage("Insira a data", MessageType.Info);
                    return false;
                }

                if (txtHora.Text == "")
                {
                    ShowMessage("Insira o horário", MessageType.Info);
                    return false;
                }

                v.CampoValido("Data e Hora", txtData.Text + " " + txtHora.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtData.Focus();
                    }
                    return false;
                }

                v.CampoValido("Código Cliente", txtCodPessoa.Text, true, true, true, false, "INT", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtCodPessoa.Focus();
                    }
                    return false;
                }

                if (txtAnotacao.Text.Length < 5)
                {
                    ShowMessage("Insira uma anotação", MessageType.Info);
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

        protected void CarregaSituacao()
        {
            Habil_TipoDAL sd = new Habil_TipoDAL();
            ddlSituacao.DataSource = sd.SituacaoAgenda();
            ddlSituacao.DataTextField = "DescricaoTipo";
            ddlSituacao.DataValueField = "CodigoTipo";
            ddlSituacao.DataBind();
            ddlSituacao.SelectedValue = "168";

            ddlTipoCompromisso.DataSource = sd.TipoCompromisso();
            ddlTipoCompromisso.DataTextField = "DescricaoTipo";
            ddlTipoCompromisso.DataValueField = "CodigoTipo";
            ddlTipoCompromisso.DataBind();
            btnUsuario.Visible = false;
        }

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected string ShowMessageModal(string Message)
        {
            return "<div id = 'alert_div' style = 'margin: 0 0.5 %; -webkit - box - shadow: 3px 4px 6px #999;' class='alert fade in alert-info'><a href = '#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Informação !</strong> <span>" + Message + "</span></div>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { 
                if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                    return;

                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();

                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "ConAgendamento.aspx");

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
                
                if (!IsPostBack)
                {
                    if (Request.QueryString["date"] != null)
                    {
                        txtData.Text = Request.QueryString["date"].ToString();
                        txtData_TextChanged(sender, e);
                    }
                    
                }

                if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                {
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                    LimpaCampos(false);

                    lista.ForEach(delegate (Permissao p)
                    {
                        if (!p.AcessoCompleto)
                        {
                            if (!p.AcessoIncluir)
                                btnSalvar.Visible = false;
                        }
                    });                   
                }


                if (Session["AgendamentoCompromisso"] != null)
                {
                    CarregaSituacao();
                    PreencheDados(sender, e);
                }
                else
                {
                    if(!IsPostBack)
                        txtCodigo.Text = "Novo";

                }
                if (Session["ZoomPessoaAgendamento"] != null)
                {
                    txtCodPessoa.Text = Session["ZoomPessoaAgendamento"].ToString().Split('³')[0];
                    txtCodPessoa_TextChanged(sender, e);
                    Session["ZoomPessoaAgendamento"] = null;
                }

                if (Session["NovoAnexo"] != null)
                {
                    ListaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];
                    grdAnexo.DataSource = ListaAnexo;
                    grdAnexo.DataBind();
                }

                Session["ZoomAnexoTimelineCliente"] = null;
                Session["ZoomAnexoTimelineCliente2"] = null;

                DateTime resultado = DateTime.MinValue;
                if (DateTime.TryParse(txtData.Text, out resultado))
                {                    
                    if (ddlSituacao.SelectedValue == "169" )
                    {
                        BtnReativar.Visible = true;
                        btnSalvar.Visible = false;
                        btnConcluir.Visible = false;
                    }
                    else if (ddlSituacao.SelectedValue == "206")
                    {
                        BtnReativar.Visible = false;
                        btnSalvar.Visible = true;
                        btnConcluir.Visible = false;
                    }
                    else
                    {
                        BtnReativar.Visible = false;
                        btnSalvar.Visible = true;
                        if(ddlSituacao.SelectedValue == "167")
                            btnConcluir.Visible = true;
                    }                  
                }
                
                if (grdAgendamento.Rows.Count == 0)
                    lblNenhumAgendamento.Visible = true;
                else
                    lblNenhumAgendamento.Visible = false;

                if (ddlTipoCompromisso.SelectedValue == "170")
                    btnUsuario.Visible = false;
                else
                    btnUsuario.Visible = true;

                if (txtCodigo.Text == "")
                    LimpaCampos(true);
               
                if(ddlTipoCompromisso.Items.Count == 0 )
                    CarregaSituacao();

            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos() == false)
                {
                    return;
                }

                AgendamentoCompromissoDAL d = new AgendamentoCompromissoDAL();
                AgendamentoCompromisso p = new AgendamentoCompromisso();
                
                p.CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);
                p.DataHoraAgendamento = Convert.ToDateTime(txtData.Text + " " + txtHora.Text);
                p.Anotacao = txtAnotacao.Text;
                p.Telefone = txtTelefone.Text;
                p.Contato = txtContato.Text;
                p.Local = txtLocal.Text;
                p.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                p.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);
                p.CodigoEmpresa = Convert.ToInt32(Session["CodEmpresa"]);
                p.ListaUsuario = (List<UsuarioAgendamento>)Session["ListaUsuariosPermitidos"];
                p.CodigoTipoAgendamento = Convert.ToInt32(ddlTipoCompromisso.SelectedValue);
                p.EnviarEmail = chkEmail.Checked;

                if (lblCorLembrete.Text == "Azul")
                    p.CorLembrete = "Skyblue";
                else if (lblCorLembrete.Text == "Cinza")
                    p.CorLembrete = "Gray";
                else if (lblCorLembrete.Text == "Verde")
                    p.CorLembrete = "Green";
                else if (lblCorLembrete.Text == "Amarelo")
                    p.CorLembrete = "Goldenrod";
                else if (lblCorLembrete.Text == "Vermelho")
                    p.CorLembrete = "Red";
                else
                    p.CorLembrete = txtOutraCor.Text;

                //CONVERTER ANEXO DO DOCUMENTO PARA ANEXO DO AGENDAMENTO
                List<AnexoAgendamento> ListaAnexoAgendamento = new List<AnexoAgendamento>();
                foreach (AnexoDocumento item in ListaAnexo)
                {
                    AnexoAgendamento anexo = new AnexoAgendamento();
                    anexo.CodigoAnexo = item.CodigoAnexo;
                    anexo.DataHoraLancamento = item.DataHoraLancamento;
                    anexo.CodigoMaquina = item.CodigoMaquina;
                    anexo.CodigoUsuario = item.CodigoUsuario;
                    anexo.NomeArquivo = item.NomeArquivo;
                    anexo.ExtensaoArquivo = item.ExtensaoArquivo;
                    anexo.Arquivo = item.Arquivo;
                    anexo.DescricaoArquivo = item.DescricaoArquivo;
                    anexo.NaoEditavel = item.NaoEditavel;
                    ListaAnexoAgendamento.Add(anexo);
                }

                if (txtCodigo.Text == "Novo" )
                {
                    p.CodigoSituacao = 167;
                    d.Inserir(p, ListaAnexoAgendamento);
                    ShowMessage("Agendamento Incluído com Sucesso!!!", MessageType.Success);
                }
                else
                {
                    p.CodigoIndex = Convert.ToInt32(txtCodigo.Text);

                    if (p.CodigoSituacao == 206)
                    {
                        AgendamentoCompromisso p2 = new AgendamentoCompromisso();
                        p2 = d.PesquisarAgendamento(p.CodigoIndex);
                        if (p2.DataHoraAgendamento != p.DataHoraAgendamento)
                        {
                            p.CodigoSituacao = 167;
                            d.Inserir(p, ListaAnexoAgendamento);
                        }
                        else
                        {
                            d.Atualizar(p, ListaAnexoAgendamento);
                        }
                    }
                    else
                    {
                        d.Atualizar(p, ListaAnexoAgendamento);
                    }

                    ShowMessage("Agendamento Alterado com Sucesso!!!", MessageType.Success);
                }
                
                LimpaCampos(true);
                btnConcluir.Visible = false;
                BtnReativar.Visible = false;
                Session["ZoomAgendamento"] = null;
                txtData_TextChanged(sender, e);
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void txtCodPessoa_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Boolean blnCampo = false;

                if (txtCodPessoa.Text.Equals(""))
                {
                    txtPessoa.Text = "";
                    return;
                }
                else
                {
                    v.CampoValido("Codigo Cliente", txtCodPessoa.Text, true, true, true, false, "", ref blnCampo, ref strMensagemR);
                    if (!blnCampo)
                    {
                        txtCodPessoa.Text = "";
                        txtPessoa.Text = "";
                        return;
                    }
                }

                Int64 codigoPessoa = Convert.ToInt64(txtCodPessoa.Text);
                PessoaDAL pessoa = new PessoaDAL();
                Pessoa p2 = new Pessoa();

                p2 = pessoa.PesquisarPessoa(codigoPessoa);

                if (p2 == null)
                {
                    ShowMessage("Pessoa não existente!", MessageType.Warning);
                    txtCodPessoa.Text = "";
                    txtPessoa.Text = "";
     
                    txtCodPessoa.Focus();

                    return;
                }

                Pessoa_Contato ctt = new Pessoa_Contato();
                PessoaContatoDAL cttDAL = new PessoaContatoDAL();
                ctt = cttDAL.PesquisarPessoaContato(Convert.ToInt64(txtCodPessoa.Text), 1);
                if (txtContato.Text == "")
                    txtContato.Text = ctt._Mail1;
                if (txtTelefone.Text == "")
                    txtTelefone.Text = ctt._Fone1;
                txtPessoa.Text = p2.NomePessoa;

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnPessoa_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx?Cad=21");
        }

        protected void txtData_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Boolean blnCampoValido = false;
                v.CampoValido("Data", txtData.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        txtData.Text = "";
                        txtData.Focus();
                    }
                }

                DateTime resultado = DateTime.MinValue;
                if (DateTime.TryParse(txtData.Text, out resultado))
                {
                    AgendamentoCompromissoDAL d = new AgendamentoCompromissoDAL();
                    grdAgendamento.DataSource = d.ListarAgendamentoPorDataUsuario(Convert.ToInt32(Session["CodUsuario"]), Convert.ToDateTime(txtData.Text));
                    grdAgendamento.DataBind();

                    if (grdAgendamento.Rows.Count == 0)
                        lblNenhumAgendamento.Visible = true;
                    else
                        lblNenhumAgendamento.Visible = false;
                }
                else
                {
                    lblNenhumAgendamento.Visible = true;
                    grdAgendamento.DataSource = null;
                    grdAgendamento.DataBind();
                }
                
                Session["ZoomAgendamento"] = null;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdAgendamentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void PreencheDados(object sender, EventArgs e)
        {
            try
            {
                AgendamentoCompromisso agenda = (AgendamentoCompromisso)Session["AgendamentoCompromisso"];
              
                if (agenda.CodigoIndex == 0)
                    txtCodigo.Text = "Novo";
                else
                    txtCodigo.Text = agenda.CodigoIndex.ToString();


                txtHora.Text = agenda.DataHoraAgendamento.ToString("HH:mm");

                if (agenda.DataHoraAgendamento.ToString("dd/MM/yyyy") != "01/01/0001" && agenda.DataHoraAgendamento.ToString("dd/MM/yyyy") != "01/01/2000")
                {
                    txtData.Text = agenda.DataHoraAgendamento.ToString("dd/MM/yyyy");
                    txtData_TextChanged(sender, e);
                }

                txtAnotacao.Text = agenda.Anotacao;
                txtTelefone.Text = agenda.Telefone;
                txtContato.Text = agenda.Contato;
                txtLocal.Text = agenda.Local;
                ddlSituacao.SelectedValue = agenda.CodigoSituacao.ToString();
                ddlTipoCompromisso.SelectedValue = agenda.CodigoTipoAgendamento.ToString();

                if (agenda.CodigoPessoa != 0)
                {
                    txtCodPessoa.Text = agenda.CodigoPessoa.ToString();
                    txtCodPessoa_TextChanged(sender, e);
                }

                if (ddlTipoCompromisso.SelectedValue == "170")
                    btnUsuario.Visible = false;
                else
                    btnUsuario.Visible = true;

                if (agenda.CorLembrete == "Skyblue")
                    lblCorLembrete.Text = "Blue";
                else if (agenda.CorLembrete == "Gray")
                    lblCorLembrete.Text = "Cinza";
                else if (agenda.CorLembrete == "Green")
                    lblCorLembrete.Text = "Verde";
                else if (agenda.CorLembrete == "Goldenrod")
                    lblCorLembrete.Text = "Amarelo";
                else if (agenda.CorLembrete == "Red")
                    lblCorLembrete.Text = "Vermelho";
                else
                    lblCorLembrete.Text = agenda.CorLembrete;

                txtOutraCor.Text = agenda.CorLembrete;

                Session["AgendamentoCompromisso"] = null;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void CompactaDocumento()
        {
            try
            {
                if (txtCodigo.Text == "")
                    return;
                
                AgendamentoCompromisso agenda = new AgendamentoCompromisso();

                if (txtCodigo.Text != "Novo")
                    agenda.CodigoIndex = Convert.ToInt32(txtCodigo.Text);

                if(txtCodPessoa.Text != "")
                    agenda.CodigoPessoa = Convert.ToInt32(txtCodPessoa.Text);

                if (txtHora.Text != "" && txtData.Text != "")
                    agenda.DataHoraAgendamento = Convert.ToDateTime(txtData.Text + " " + txtHora.Text);
                else if (txtHora.Text != "")
                    agenda.DataHoraAgendamento = Convert.ToDateTime("01/01/0001 " + txtHora.Text);
                else if (txtData.Text != "")
                    agenda.DataHoraAgendamento = Convert.ToDateTime(txtData.Text);

                agenda.Anotacao = txtAnotacao.Text;
                agenda.Telefone = txtTelefone.Text;
                agenda.Contato = txtContato.Text;
                agenda.Local = txtLocal.Text;
                agenda.CodigoSituacao = Convert.ToInt32(ddlSituacao.SelectedValue);
                agenda.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);
                agenda.CodigoTipoAgendamento = Convert.ToInt32(ddlTipoCompromisso.SelectedValue);
                agenda.EnviarEmail = chkEmail.Checked;

                if (lblCorLembrete.Text == "Azul")
                    agenda.CorLembrete = "Skyblue";
                else if (lblCorLembrete.Text == "Cinza")
                    agenda.CorLembrete = "Gray";
                else if (lblCorLembrete.Text == "Verde")
                    agenda.CorLembrete = "Green";
                else if (lblCorLembrete.Text == "Amarelo")
                    agenda.CorLembrete = "Goldenrod";
                else if (lblCorLembrete.Text == "Vermelho")
                    agenda.CorLembrete = "Red";
                else
                    agenda.CorLembrete = txtOutraCor.Text;

                Session["ZoomPessoaAgendamento"] = null;
                Session["AgendamentoCompromisso"] = agenda;
                
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Compromissos/ConAgendamento.aspx");
        }

        protected void grdAgendamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            { 
                string x = e.CommandName;

                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = grdAgendamento.Rows[index];
                string Codigo = Server.HtmlDecode(row.Cells[1].Text);
                Session["ZoomAgendamento"] = Codigo;

                AgendamentoCompromissoDAL agendaDAL = new AgendamentoCompromissoDAL();
                AgendamentoCompromisso agenda = new AgendamentoCompromisso();

                if (x == "Editar")
                {
                    CarregaSituacao();
                    Session["ListaUsuariosPermitidos"] = null;
                    agenda = agendaDAL.PesquisarAgendamento(Convert.ToInt32(Codigo));

                    txtCodigo.Text = agenda.CodigoIndex.ToString();
                    txtCodPessoa.Text = agenda.CodigoPessoa.ToString();

                    PessoaDAL pessoa = new PessoaDAL();
                    Pessoa p2 = new Pessoa();

                    p2 = pessoa.PesquisarPessoa(Convert.ToInt64(txtCodPessoa.Text));

                    if (p2 == null)
                    {
                        txtCodPessoa.Text = "";
                        txtPessoa.Text = "";
                        return;
                    }
                    else
                        txtPessoa.Text = p2.NomePessoa;

                    ListaAnexo.Clear();
                    txtHora.Text = agenda.DataHoraAgendamento.ToString("HH:mm");
                    txtAnotacao.Text = agenda.Anotacao;
                    txtTelefone.Text = agenda.Telefone;
                    txtContato.Text = agenda.Contato;
                    txtLocal.Text = agenda.Local;
                    ddlSituacao.SelectedValue = agenda.CodigoSituacao.ToString();
                    ddlTipoCompromisso.SelectedValue = agenda.CodigoTipoAgendamento.ToString();
                    chkEmail.Checked = agenda.EnviarEmail;

                    //CONVERTER ANEXO DO AGENDAMENTO PARA ANEXO DO DOCUMENTO
                    AnexoAgendamentoDAL anexoDAL = new AnexoAgendamentoDAL();
                    List<AnexoAgendamento> ListaAnexoAgendamento = new List<AnexoAgendamento>();
                    ListaAnexoAgendamento = anexoDAL.ObterAnexos(Convert.ToInt32(txtCodigo.Text));

                    foreach (AnexoAgendamento item in ListaAnexoAgendamento)
                    {
                        AnexoDocumento anexo = new AnexoDocumento();
                        anexo.CodigoAnexo = item.CodigoAnexo;
                        anexo.DataHoraLancamento = item.DataHoraLancamento;
                        anexo.CodigoMaquina = item.CodigoMaquina;
                        anexo.CodigoUsuario = item.CodigoUsuario;
                        anexo.NomeArquivo = item.NomeArquivo;
                        anexo.ExtensaoArquivo = item.ExtensaoArquivo;
                        anexo.Arquivo = item.Arquivo;
                        anexo.DescricaoArquivo = item.DescricaoArquivo;
                        anexo.NaoEditavel = item.NaoEditavel;
                        anexo.Cpl_Usuario = item.Cpl_Usuario;
                        anexo.Cpl_Maquina = item.Cpl_Maquina;
                        ListaAnexo.Add(anexo);
                    }

                    Session["NovoAnexo"] = ListaAnexo;
                    grdAnexo.DataSource = ListaAnexo;
                    grdAnexo.DataBind();

                    if (ddlTipoCompromisso.SelectedValue == "170")
                        btnUsuario.Visible = false;
                    else
                    {
                        btnUsuario.Visible = true;
                        Session["ListaUsuariosPermitidos"] = agenda.ListaUsuario;
                    }

                    if (agenda.CorLembrete == "Skyblue")
                        lblCorLembrete.Text = "Azul";
                    else if (agenda.CorLembrete == "Gray")
                        lblCorLembrete.Text = "Cinza";
                    else if (agenda.CorLembrete == "Green")
                        lblCorLembrete.Text = "Verde";
                    else if (agenda.CorLembrete == "Goldenrod")
                        lblCorLembrete.Text = "Amarelo";
                    else if (agenda.CorLembrete == "Red")
                        lblCorLembrete.Text = "Vermelho";
                    else
                        lblCorLembrete.Text = agenda.CorLembrete;

                    txtOutraCor.Text = agenda.CorLembrete;

                    if (agenda.CodigoSituacao == 169)
                    {
                        BtnReativar.Visible = true;
                        btnSalvar.Visible = false;
                        btnConcluir.Visible = false;
                    }
                    else if(agenda.CodigoSituacao == 206)
                    {
                        BtnReativar.Visible = false;
                        btnSalvar.Visible = true;
                        btnConcluir.Visible = false;
                    }
                    else
                    {
                        btnConcluir.Visible = true;
                    }

                }
                else if (x == "Cancelar")
                {
                    agendaDAL.Cancelar(Convert.ToInt32(Codigo));
                    txtData_TextChanged(sender, e);

                    if (txtCodigo.Text == Codigo)
                        LimpaCampos(true);

                    ShowMessage("Agendamento cancelado com sucesso", MessageType.Success);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnCalendario_Click(object sender, EventArgs e)
        {
            try
            {
                List<AgendamentoCompromisso> Lista = new List<AgendamentoCompromisso>();
                AgendamentoCompromissoDAL agendaDAL = new AgendamentoCompromissoDAL();
                Lista = agendaDAL.ListarAgendamento("USU.CD_USUARIO", "INT", Session["CodUsuario"].ToString(), "");

                for (int i = 0; i < Lista.Count; i++)
                {
                    if (Lista[i].CodigoSituacao == 206)
                    {
                        Agendamentos += "{" +
                                            "title: '" + Lista[i].DataHoraAgendamento.ToString("HH:mm") + " - " + Lista[i].Anotacao.Replace("\n", " ").Replace("\r", " ") + "'," +
                                            "start: '" + Lista[i].DataHoraAgendamento.ToString("yyyy-MM-dd") + "'," +
                                            "color: '" + Lista[i].CorLembrete + "'," +
                                            "className: 'TarefaConcluida'" +
                                        "},";
                    }
                    else
                    {
                        Agendamentos += "{" +
                                            "title: '" + Lista[i].DataHoraAgendamento.ToString("HH:mm") + " - " + Lista[i].Anotacao.Replace("\n", " ").Replace("\r", " ") + "'," +
                                            "start: '" + Lista[i].DataHoraAgendamento.ToString("yyyy-MM-dd") + "'," +
                                            "color: '" + Lista[i].CorLembrete + "'" +
                                        "},";
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "MostrarCalendario()", true);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void txtAnotacao_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnVerde_Click(object sender, EventArgs e)
        {
            lblCorLembrete.Text = "Verde";
        }

        protected void btnAmarelo_Click(object sender, EventArgs e)
        {
            lblCorLembrete.Text = "Amarelo";
        }
            
        protected void btnVermelho_Click(object sender, EventArgs e)
        {
            lblCorLembrete.Text = "Vermelho";
        }

        protected void BtnReativar_Click(object sender, EventArgs e)
        {
            ddlSituacao.SelectedValue = "167";
            BtnReativar.Visible = false;
            btnSalvar.Visible = true;
            btnConcluir.Visible = false;
            btnSalvar_Click(sender, e);
            
        }

        protected void btnCinza_Click(object sender, EventArgs e)
        {
            lblCorLembrete.Text = "Cinza";
        }

        protected void btnAzul_Click(object sender, EventArgs e)
        {
            lblCorLembrete.Text = "Azul";
        }

        protected void txtOutraCor_TextChanged(object sender, EventArgs e)
        {
            lblCorLembrete.Text = txtOutraCor.Text;
        }

        protected void ddlTipoCompromisso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoCompromisso.SelectedValue == "171")
                {
                    List<Usuario> ListaUsuarios2 = new List<Usuario>();
                    UsuarioDAL usuDAL = new UsuarioDAL();

                    if (Session["ListaUsuariosPermitidos"] == null)
                    {
                        ListaUsuarios2 = usuDAL.ListarUsuarios("CD_PFL_USUARIO", "INT", Session["CodPflUsuario"].ToString(), "");
                        foreach (var item in ListaUsuarios2)
                        {
                            UsuarioAgendamento usuAgenda = new UsuarioAgendamento();
                            usuAgenda.CodigoUsuario = item.CodigoUsuario;
                            usuAgenda.Cpl_NomeUsuario = item.NomeUsuario;
                            ListaUsuarios.Add(usuAgenda);
                        }
                        Session["ListaUsuariosPermitidos"] = ListaUsuarios;
                    }
                    else
                        ListaUsuarios = (List<UsuarioAgendamento>)Session["ListaUsuariosPermitidos"];

                    grdUsuario.DataSource = ListaUsuarios;
                    grdUsuario.DataBind();

                    ddlUsuario.DataSource = usuDAL.ListarUsuarios("", "", "", "");
                    ddlUsuario.DataTextField = "NomeUsuario";
                    ddlUsuario.DataValueField = "CodigoUsuario";
                    ddlUsuario.DataBind();

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ModalUsuarios()", true);
                    btnUsuario.Visible = true;
                }
                else if (ddlTipoCompromisso.SelectedValue == "170")
                {
                    Session["ListaUsuariosPermitidos"] = null;
                    btnUsuario.Visible = false;
                }
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["ListaUsuariosPermitidos"] != null)
                ListaUsuarios = (List<UsuarioAgendamento>)Session["ListaUsuariosPermitidos"];
                else
                    ListaUsuarios = new List<UsuarioAgendamento>();

                ListaUsuarios.RemoveAll(x => x.CodigoUsuario == Convert.ToInt32(HttpUtility.HtmlDecode(grdUsuario.SelectedRow.Cells[0].Text)));

                Session["ListaUsuariosPermitidos"] = ListaUsuarios;
                grdUsuario.DataSource = ListaUsuarios;
                grdUsuario.DataBind();
                ddlTipoCompromisso_SelectedIndexChanged(sender, e);
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void BtnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ListaUsuariosPermitidos"] != null)
                    ListaUsuarios = (List<UsuarioAgendamento>)Session["ListaUsuariosPermitidos"];
                else
                    ListaUsuarios = new List<UsuarioAgendamento>();

                if(ListaUsuarios.Where(x => x.CodigoUsuario == Convert.ToInt32(ddlUsuario.SelectedValue)).Count() > 0)
                {
                    lblModal.Text = ShowMessageModal("Usuário já incluído no agendamento");
                    return;
                }

                UsuarioAgendamento usuAgenda = new UsuarioAgendamento();
                usuAgenda.CodigoUsuario = Convert.ToInt32(ddlUsuario.SelectedValue);
                usuAgenda.Cpl_NomeUsuario = ddlUsuario.SelectedItem.Text;
                ListaUsuarios.Add(usuAgenda);
                
                Session["ListaUsuariosPermitidos"] = ListaUsuarios;
                grdUsuario.DataSource = ListaUsuarios;
                grdUsuario.DataBind();

                ddlTipoCompromisso_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnFechaModalUser_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "FecharModalUsuarios()", true);
        }

        protected void btnUsuario_Click(object sender, EventArgs e)
        {
            lblModal.Text = "";
            ddlTipoCompromisso_SelectedIndexChanged(sender, e);
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            LimpaCampos(true);
        }

        protected void btnHistorico_Click(object sender, EventArgs e)
        {
            txtPesquisar.Text = "";
            litHistorico.Text = "";
            if (txtCodPessoa.Text != "")
            {
                List<AgendamentoCompromisso> Lista = new List<AgendamentoCompromisso>();
                AgendamentoCompromissoDAL agendaDAL = new AgendamentoCompromissoDAL();
                Lista = agendaDAL.ListarAgendamentoCliente(Convert.ToInt32(Session["CodUsuario"].ToString()),Convert.ToInt64(txtCodPessoa.Text),"");
                Lista = Lista.OrderByDescending(x => x.DataHoraAgendamento).ToList();
                if (Lista.Count == 0)
                {
                    ShowMessage("Nenhum histórico de conversa com " + txtPessoa.Text, MessageType.Info);
                    return;
                }
                
                litHistorico.Text += "<div class='timeline'>";

                foreach (var item in Lista)
                {
                    litHistorico.Text += " <div class='timeline-row'  > " +
                                                "<div class='timeline-time'>" +
                                                    item.DataHoraAgendamento.ToString("HH:mm") +
                                                    "</br><span style='font-size:15px'>" + item.DataHoraAgendamento.ToString("dd/MM/yyyy") + "</span> " +
                                                "</div> " +
                                                "<div class='timeline-dot fb-bg' ></div> " +
                                                "<div class='timeline-content' style='background-color:" + item.CorLembrete + ";'>" +
                                                    "<p class='style-content-h4'>" + item.Anotacao.Replace("\n", "</br>") + "<p> " +
                                                    "<p class='style-content-p'>Local: " + item.Local + "!</br> " +
                                                    "Contato: " + item.Telefone + " </br> " + item.Contato.Replace("\n", "</br>") + "<p> ";

                    List<AnexoAgendamento> ListaAnexo2 = new List<AnexoAgendamento>();
                    AnexoAgendamentoDAL anexoDAL = new AnexoAgendamentoDAL();
                    ListaAnexo2 = anexoDAL.ObterAnexos(item.CodigoIndex);
                    if(ListaAnexo2.Count > 0 )
                    { 
                        litHistorico.Text += "<h4 class='style-content-h4' style='text-align:center'>Anexo(s) " +
                                                "<table class='table table-striped'>" +
                                                    "<tr>" +
                                                        "<th>Código</th>" +
                                                        "<th>Data/Hora Lanc.</th>" +
                                                        "<th>Descrição</th>" +
                                                        "<th>Usuário</th>" + 
                                                        "<th>Acessar</th>" +
                                                    "</tr>";

                        foreach (var anexo in ListaAnexo2)
                        {
                            litHistorico.Text += "<tr style='background-color:white'>" +
                                                    "<td>" + anexo.CodigoAnexo + "</td>" +
                                                    "<td>" + anexo.DataHoraLancamento + "</td>" +
                                                    "<td>" + anexo.DescricaoArquivo + "</td>" +
                                                    "<td>" + anexo.Cpl_Usuario + "</td>" +
                                                    "<td>" +
                                                        "<button onclick='FazerDownload(" + anexo.CodigoAgendamento + "," + anexo.CodigoAnexo + ")' class='btn btn-default' style='width:40px; height:25px;padding:0'>" +
                                                            "<span class='glyphicon glyphicon-edit ' style='width:17px; height:17px'/> " +
                                                        "</button>" +
                                                    "</td>" +
                                                "</tr>";
                        }

                        litHistorico.Text += "</table><h4>";
                    }

                    litHistorico.Text += "</div>" +
                                        "</div>";

                }

                litHistorico.Text += "</div>";
                txtPesquisar.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ModalHistorico('Histórico de conversas - " + txtPessoa.Text +"')", true);
            }
        }

        protected void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            if (txtCodPessoa.Text != "")
            {
                litHistorico.Text = "";
                List<AgendamentoCompromisso> Lista = new List<AgendamentoCompromisso>();
                AgendamentoCompromissoDAL agendaDAL = new AgendamentoCompromissoDAL();

                if (txtPesquisar.Text.Trim().Length >= 3)
                    Lista = agendaDAL.ListarAgendamentoCliente(Convert.ToInt32(Session["CodUsuario"].ToString()), Convert.ToInt64(txtCodPessoa.Text), txtPesquisar.Text);
                else
                    Lista = agendaDAL.ListarAgendamentoCliente(Convert.ToInt32(Session["CodUsuario"].ToString()), Convert.ToInt64(txtCodPessoa.Text), "");

                if (Lista.Count == 0)
                {
                    litHistorico.Text = "<p style='text-align:center'>Nenhuma conversa encontrada</p>";
                    return;
                }


                Lista = Lista.OrderByDescending(x => x.DataHoraAgendamento).ToList();
                
                litHistorico.Text += "<div class='timeline'>";
                foreach (var item in Lista)
                {
                    litHistorico.Text += " <div class='timeline-row' > " +
                                                "<div class='timeline-time'>" +
                                                    item.DataHoraAgendamento.ToString("HH:mm") +
                                                    "</br><span style='font-size:15px'>" + item.DataHoraAgendamento.ToString("dd/MM/yyyy") + "</span> " +
                                                "</div> " +
                                                "<div class='timeline-dot fb-bg' ></div> " +
                                                "<div class='timeline-content' style='background-color:" + item.CorLembrete + "' >" +
                                                    "<p class='style-content-h4'>" + item.Anotacao.Replace("\n", "</br>") + "<p> " +
                                                    "<p class='style-content-p'>Local: " + item.Local + "!</br> " +
                                                    "Contato: " + item.Telefone + " </br> " + item.Contato.Replace("\n", "</br>") + "<p> ";
                    List<AnexoAgendamento> ListaAnexo2 = new List<AnexoAgendamento>();
                    AnexoAgendamentoDAL anexoDAL = new AnexoAgendamentoDAL();
                    ListaAnexo2 = anexoDAL.ObterAnexos(item.CodigoIndex);
                    if (ListaAnexo2.Count > 0)
                    {
                        litHistorico.Text += "<h4 class='style-content-h4' style='text-align:center'>Anexo(s)" +
                                                "<table class='table table-striped'>" +
                                                    "<tr>" +
                                                        "<th>Código</th>" +
                                                        "<th>Data/Hora Lanc.</th>" +
                                                        "<th>Descrição</th>" +
                                                        "<th>Usuário</th>" +
                                                        "<th>Acessar</th>" +
                                                    "</tr>";

                        foreach (var anexo in ListaAnexo2)
                        {
                            litHistorico.Text += "<tr style='background-color:white'>" +
                                                    "<td>" + anexo.CodigoAnexo + "</td>" +
                                                    "<td>" + anexo.DataHoraLancamento + "</td>" +
                                                    "<td>" + anexo.DescricaoArquivo + "</td>" +
                                                    "<td>" + anexo.Cpl_Usuario + "</td>" +
                                                    "<td>" +
                                                        "<button onclick='FazerDownload(" + anexo.CodigoAgendamento + "," + anexo.CodigoAnexo + ")' class='btn btn-default' style='width:40px; height:25px;padding:0'>" +
                                                            "<span class='glyphicon glyphicon-edit ' style='width:17px; height:17px'/> " +
                                                        "</button>" +
                                                    "</td>" +
                                                "</tr>";
                        }

                        litHistorico.Text += "</table><h4> ";
                    }

                    litHistorico.Text += "</div>" +
                                        "</div>";
                                            
                }
                litHistorico.Text += "</div>";
                

            }
        }

        protected void grdAnexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompactaDocumento();
            Session["ZoomDocCtaAnexo"] = HttpUtility.HtmlDecode(grdAnexo.SelectedRow.Cells[0].Text);
            Response.Redirect("~/Pages/financeiros/ManAnexo.aspx?cad=14");
        }
        protected void btnNovoAnexo_Click(object sender, EventArgs e)
        {
            CompactaDocumento();
            Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=14");
        }
        protected void txtDownloadAnexo_TextChanged(object sender, EventArgs e)
        {
            try
            {                
                string strCodigoAgendamento = txtDownloadAnexo.Text.Split('³')[0];
                string strCodigoAnexo = txtDownloadAnexo.Text.Split('³')[1];
                CompactaDocumento();

                Session["ZoomAnexoTimelineCliente"] = strCodigoAgendamento + "³" + strCodigoAnexo;
                Response.Redirect("~/Pages/Financeiros/ManAnexo.aspx?cad=14");
                //txtDownloadAnexo.Text = "";
                //List<AnexoAgendamento> ListaAnexo2 = new List<AnexoAgendamento>();
                //AnexoAgendamentoDAL anexoDAL = new AnexoAgendamentoDAL();
                //ListaAnexo2 = anexoDAL.ObterAnexos(Convert.ToDecimal(strCodigoAgendamento));

                //AnexoAgendamento anexoDownload = null;
                //foreach (var anexo in ListaAnexo2)
                //{
                //    if (anexo.CodigoAnexo.ToString() == strCodigoAnexo)
                //    {
                //        anexoDownload = new AnexoAgendamento();
                //        anexoDownload = anexo;
                //    }
                //}
                //if (anexoDownload != null)
                //{
                //    string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\";

                //    if (System.IO.File.Exists(CaminhoArquivoLog + anexoDownload.NomeArquivo))
                //        System.IO.File.Delete(CaminhoArquivoLog + anexoDownload.NomeArquivo);

                //    FileStream file = new FileStream(CaminhoArquivoLog + anexoDownload.NomeArquivo, FileMode.Create);

                //    BinaryWriter bw = new BinaryWriter(file);
                //    bw.Write(anexoDownload.Arquivo);
                //    bw.Close();

                //    file = new FileStream(CaminhoArquivoLog + anexoDownload.NomeArquivo, FileMode.Open);
                //    BinaryReader br = new BinaryReader(file);
                //    file.Close();
                //    Response.Clear();
                //    Response.ContentType = "application/octect-stream";
                //    Response.AppendHeader("content-disposition", "filename=" + anexoDownload.NomeArquivo);
                //    Response.TransmitFile(CaminhoArquivoLog + anexoDownload.NomeArquivo);
                //    HttpContext.Current.Response.Flush();
                //    HttpContext.Current.Response.SuppressContent = true;
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //}
            }
            catch (Exception ex)
            {
                
                ShowMessage(ex.Message, MessageType.Error);
            }
            
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {         
            //CompactaDocumento();
            //Response.Redirect("~/Pages/Compromissos/ManAgendamento.aspx");
        }

        protected void BtnConcluir_Click(object sender, EventArgs e)
        {
            ddlSituacao.SelectedValue = "206";
            BtnReativar.Visible = false;
            btnSalvar.Visible = true;
            btnSalvar_Click(sender, e);
        }

        protected void txtAnotacao_TextChanged1(object sender, EventArgs e)
        {
            txtAnotacao.Text = txtAnotacao.Text.Replace("<", "[").Replace(">", "]");
        }
    }
}