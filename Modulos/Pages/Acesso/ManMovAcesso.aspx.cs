using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;


namespace SoftHabilInformatica.Pages.Acesso
{
    public partial class ManMovAcesso : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        private List<MovAcesso> listMovAcesso = new List<MovAcesso>();
        private clsValidacao v = new clsValidacao();
        private String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected bool ValidaCampos()
        {
            Boolean blnCampoValido = false;

            if (Session["MensagemTela"] != null)
            {
                strMensagemR = Session["MensagemTela"].ToString();
                Session["MensagemTela"] = null;
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }

            if (DdlTipoAcesso.Text == "..... SELECIONE UM TIPO DE ACESSO .....")
            {
                ShowMessage("Tipo de Acesso deve ser Selecionado.", MessageType.Info);
                return false;
            }

            v.CampoValido("Documento", txtDocumento.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtDocumento.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtDocumento.Focus();

                }

                return false;
            }

            if (DdlPessoa.SelectedValue == "..... SELECIONE UMA PESSOA .....")
            {
                ShowMessage("Pessoa deve ser Informada.", MessageType.Info);
                return false;
            }


            return true;
        }
        protected void LimpaCampos()
        {
            DBTabelaDAL t = new DBTabelaDAL();

            txtCodMovAcesso.Text = "Novo";
            txtDocumento.Text = "";
            txtOBS.Text = ConfigurationManager.AppSettings["CaminhoDosRelatorios"].ToString() + "RelMovAcesso.rpt"; ;
            
            txtDataEntrada.Text = t.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm");

            TipoAcessoDAL d = new TipoAcessoDAL();
            DdlTipoAcesso.DataSource = d.ListarTipoAcessos("","","","");
            DdlTipoAcesso.DataTextField = "DescricaoTipoAcesso";
            DdlTipoAcesso.DataValueField = "CodigoTipoAcesso";
            DdlTipoAcesso.DataBind();
            DdlTipoAcesso.Items.Insert(0, "..... SELECIONE UM TIPO DE ACESSO .....");

            PessoaDAL p = new PessoaDAL();
            DdlPessoa.DataSource =  p.ListarPessoas("", "", "", "");
            DdlPessoa.DataTextField = "NomePessoa";
            DdlPessoa.DataValueField = "CodigoPessoa";
            DdlPessoa.DataBind();
            DdlPessoa.Items.Insert(0, "..... SELECIONE UMA PESSOA .....");

            DdlContato.Items.Insert(0, "..... SELECIONE UM CONTATO .....");

            imaFoto.ImageUrl = @"~\Images\Images.jpg?" + DateTime.Now.Ticks.ToString();
            imaFoto.Width = 150;
            imaFoto.Height = 150;


            VeiculoDAL v = new VeiculoDAL();
            DdlVeiculo.DataSource = v.ListarVeiculos("", "", "", "");
            DdlVeiculo.DataTextField = "NomeVeiculoCombo";
            DdlVeiculo.DataValueField = "CodigoVeiculo";
            DdlVeiculo.DataBind();
            DdlVeiculo.Items.Insert(0, "..... SELECIONE UM VEÍCULO .....");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            Panel pnlPainel = (Panel)Master.FindControl("pnlPrincipalMasterPage");
            LinkButton cmdSair = (LinkButton)Master.FindControl("btnSair");

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Error);
                Session["MensagemTela"] = null;
                return;
            }

            if (Session["ZoomMovAcesso2"] != null)
            {
                if (Session["ZoomMovAcesso2"].ToString() == "RELACIONAL")
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
                                            "ConMovAcesso.aspx");

            lista.ForEach(delegate(Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoAlterar)
                        BtnSalvar.Visible = false;

                    if (!x.AcessoExcluir)
                        BtnExcluir.Visible = false;
                }
            });

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                if (Session["ZoomMovAcesso2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["IncMovAcessoTipoAcesso"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomMovAcesso"] != null)
                {
                    string s = Session["ZoomMovAcesso"].ToString();
                    Session["ZoomMovAcesso"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        BtnExcluir.Visible = true;
                        foreach (string word in words)
                            if (txtCodMovAcesso.Text == "")
                            {
                                LimpaCampos();

                                txtCodMovAcesso.Text = word;
                                txtCodMovAcesso.Enabled = false;

                                MovAcessoDAL re = new MovAcessoDAL();
                                MovAcesso p = new MovAcesso();

                                p = re.PesquisarMovAcesso(Convert.ToInt64(txtCodMovAcesso.Text));
                                
                                txtDocumento.Text = p.Documento;
                                txtOBS.Text = p.Observacoes;

                                if (p.DataHoraEntrada != null)
                                    txtDataEntrada.Text = p.DataHoraEntrada.ToString();
                                if (p.DataHoraSaida != null)
                                    txtDataSaida.Text = p.DataHoraSaida.ToString();

                                DdlTipoAcesso.SelectedValue = p.CodTipoAcesso.ToString();
                                DdlPessoa.SelectedValue = p.CodPessoa.ToString();
                                DdlPessoa_SelectedIndexChanged(sender, e);
                                DdlContato.SelectedValue = p.CodContato.ToString();
                                DdlContato_SelectedIndexChanged(sender, e);
                                DdlVeiculo.SelectedValue = p.CodVeiculo.ToString();
                            }
                    }
                }
                else
                {
                    LimpaCampos();
                    BtnExcluir.Visible = false;
                }

                if (Session["IncMovAcessoTipoAcesso"] != null)
                {
                    listMovAcesso = (List<MovAcesso>)Session["IncMovAcessoTipoAcesso"];
                    MoveDados(sender, e);
                    listMovAcesso = null;
                    Session["IncMovAcessoTipoAcesso"] = null;
                }

                if (Session["IncMovAcessoPessoa"] != null)
                {
                    listMovAcesso = (List<MovAcesso>)Session["IncMovAcessoPessoa"];
                    MoveDados(sender, e);
                    listMovAcesso = null;
                    Session["IncMovAcessoPessoa"] = null;
                }

                if (Session["IncMovAcessoContato"] != null)
                {
                    listMovAcesso = (List<MovAcesso>)Session["IncMovAcessoContato"];
                    MoveDados(sender, e);
                    listMovAcesso = null;
                    Session["IncMovAcessoContato"] = null;
                }

                if (Session["IncMovAcessoVeiculo"] != null)
                {
                    listMovAcesso = (List<MovAcesso>)Session["IncMovAcessoVeiculo"];
                    MoveDados(sender, e);
                    listMovAcesso = null;
                    Session["IncMovAcessoVeiculo"] = null;
                }

            }
            if (txtCodMovAcesso.Text == "")
                BtnVoltar_Click(sender, e);
        }
        protected void MoveDados(object sender, EventArgs e)
        {
            LimpaCampos();
            foreach (MovAcesso p in listMovAcesso)
            {

                if (p.CodMovAcesso == 0)
                    txtCodMovAcesso.Text = "Novo";
                else
                    txtCodMovAcesso.Text = p.CodMovAcesso.ToString();

                txtDocumento.Text = p.Documento;
                txtOBS.Text = p.Observacoes;

                if (p.DataHoraEntrada != null)
                    txtDataEntrada.Text = p.DataHoraEntrada.ToString();
                if (p.DataHoraSaida != null)
                    txtDataSaida.Text = p.DataHoraSaida.ToString();

                DdlTipoAcesso.SelectedValue = p.CodTipoAcesso.ToString();

                DdlPessoa.SelectedValue = p.CodPessoa.ToString();
                DdlPessoa_SelectedIndexChanged(sender, e);

                DdlContato.SelectedValue = p.CodContato.ToString();
                DdlContato_SelectedIndexChanged(sender, e);

                DdlVeiculo.SelectedValue = p.CodVeiculo.ToString();

            }

        }
        protected void BtnCfmSim_Click(object sender, EventArgs e)
        {
            String strErro = "";
            try
            {
                if (txtCodMovAcesso.Text.Trim() != "")
                {
                    MovAcessoDAL d = new MovAcessoDAL();
                    d.Excluir(Convert.ToInt64(txtCodMovAcesso.Text));
                    Session["MensagemTela"] = "Movimento Excluído com Sucesso!!!";
                    BtnVoltar_Click(sender, e);
                }
                else
                    strErro = "&emsp;&emsp;&emsp;Exclusão Não Permitida!!! Código do Movimento de Acesso não identificado.&emsp;&emsp;&emsp;";
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
        protected void BtnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomVeiculo"] = null;
            Response.Redirect("~/Pages/Acesso/ConMovAcesso.aspx");
        }
        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            MovAcessoDAL d = new MovAcessoDAL();
            MovAcesso p = new MovAcesso();

            p.Documento = txtDocumento.Text.ToUpper(); 
            p.DataHoraEntrada = Convert.ToDateTime(txtDataEntrada.Text);
            p.Observacoes = txtOBS.Text;

            if (txtDataSaida.Text !="")
                p.DataHoraSaida= Convert.ToDateTime(txtDataSaida.Text);

            if (DdlTipoAcesso.SelectedValue != "..... SELECIONE UM TIPO DE ACESSO .....")
                p.CodTipoAcesso = Convert.ToInt32(DdlTipoAcesso.SelectedValue);

            if (DdlPessoa.SelectedValue != "..... SELECIONE UMA PESSOA .....")
                p.CodPessoa = Convert.ToInt64(DdlPessoa.SelectedValue);

            if (DdlContato.SelectedValue != "..... SELECIONE UM CONTATO .....")
                p.CodContato = Convert.ToInt32(DdlContato.SelectedValue);

            if (DdlVeiculo.SelectedValue != "..... SELECIONE UM VEÍCULO .....")
                p.CodVeiculo = Convert.ToInt64(DdlVeiculo.SelectedValue);

            if (txtCodMovAcesso.Text == "Novo")
            {
                d.Inserir(p);
                Session["MensagemTela"] = "Movimento de Acesso incluso com Sucesso!!!";
            }
            else
            {
                p.CodMovAcesso = Convert.ToInt64(txtCodMovAcesso.Text);
                d.Atualizar(p);

                Session["MensagemTela"] = "Movimento de Acesso alterado com Sucesso!!!";
            }

            BtnVoltar_Click(sender, e);

        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
 
        }
        protected void DdlTipoAcesso_SelectedIndexChanged(object sender, EventArgs e)
        {
            Thread.Sleep(5000);

            txtDocumento.Focus();
        }
        protected void MoveListaPrincipal(object sender, EventArgs e)
        {
            long CodMovAcesso = 0;
            int CodTipoA = 0;
            long CodPessoa = 0;
            int CodContato = 0;
            long CodVeiculo = 0;
            DateTime DteData1;


            if (txtCodMovAcesso.Text != "Novo")
                CodMovAcesso = Convert.ToInt64(txtCodMovAcesso.Text);

            if (DdlTipoAcesso.SelectedValue != "..... SELECIONE UM TIPO DE ACESSO .....")
                CodTipoA = Convert.ToInt32(DdlTipoAcesso.SelectedValue);

            if (DdlPessoa.SelectedValue != "..... SELECIONE UMA PESSOA .....")
                CodPessoa = Convert.ToInt64(DdlPessoa.SelectedValue);

            if (DdlContato.SelectedValue != "..... SELECIONE UM CONTATO .....")
                CodContato = Convert.ToInt32(DdlContato.SelectedValue);

            if (DdlVeiculo.SelectedValue != "..... SELECIONE UM VEÍCULO .....")
                CodVeiculo = Convert.ToInt64(DdlVeiculo.SelectedValue);

            if (txtDataEntrada.Text != "")
                DteData1 = Convert.ToDateTime(txtDataEntrada.Text);

            else
                DteData1 = DateTime.UtcNow;

            MovAcesso x2 = new MovAcesso(CodMovAcesso, DteData1, CodTipoA, txtDocumento.Text, CodPessoa, CodContato, CodVeiculo, txtOBS.Text);

            listMovAcesso = new List<MovAcesso>();
            listMovAcesso.Add(x2);
            Session["IncMovAcessoTipoAcesso"] = listMovAcesso;
        }
        protected void BtnAddTipoAcesso_Click(object sender, EventArgs e)
        {


            MoveListaPrincipal(sender, e);
            Session["IncMovAcessoTipoAcesso"] = listMovAcesso;
            Session["IncMovAcessoPessoa"] = null;
            Session["IncMovAcessoContato"] = null;
            Session["IncMovAcessoVeiculo"] = null;
            Session["ZoomTipoAcesso2"] = "RELACIONAL";
            Response.Redirect("~/Pages/Acesso/CadTipoAcesso.aspx");

        }
        protected void DdlPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlPessoa.SelectedValue != "..... SELECIONE UMA PESSOA .....")
            { 
                PessoaContatoDAL c = new PessoaContatoDAL();
                DdlContato.DataSource = c.ObterPessoaContatos(Convert.ToInt64( DdlPessoa.SelectedValue.ToString()));
                DdlContato.DataTextField = "_NomeContatoCombo";
                DdlContato.DataValueField = "_CodigoItem";
                DdlContato.DataBind();
                DdlContato.Items.Insert(0, "..... SELECIONE UM CONTATO .....");

                imaFoto.ImageUrl = @"~\Images\Images.jpg?" + DateTime.Now.Ticks.ToString();
                imaFoto.Width = 150;
                imaFoto.Height = 150;

                txtCNPJCPF.Text = c.ObterInscricaoPessoaContato(Convert.ToInt64(DdlPessoa.SelectedValue), 0);

            }
        }
        protected void BtnAddPessoa_Click(object sender, EventArgs e)
        {
            MoveListaPrincipal(sender, e);
            Session["IncMovAcessoPessoa"] = listMovAcesso;
            Session["IncMovAcessoTipoAcesso"] = null;
            Session["IncMovAcessoContato"] = null;
            Session["IncMovAcessoTipoAcesso"] = null;

            Session["ZoomCadPessoa3"] = "RELACIONAL";
            Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");

        }
        protected void DdlContato_SelectedIndexChanged(object sender, EventArgs e)
        {

            PessoaContatoDAL re = new PessoaContatoDAL();
            Pessoa_Contato p = new Pessoa_Contato();

            if (DdlContato.SelectedValue != "..... SELECIONE UM CONTATO .....")
            {
                p = re.PesquisarPessoaContato(Convert.ToInt64(DdlPessoa.SelectedValue), Convert.ToInt32(DdlContato.SelectedValue));

                if (p._Foto != null)
                {
                    byteArrayToImage(p._Foto);

                    imaFoto.ImageUrl = "~/Images/CapturaNow.png?" + DateTime.Now.Ticks.ToString();
                }
                else
                    imaFoto.ImageUrl = @"~/Images/Images.jpg?" + DateTime.Now.Ticks.ToString();

                imaFoto.Width = 150;
                imaFoto.Height = 150;

                txtCNPJCPF.Text = re.ObterInscricaoPessoaContato(Convert.ToInt64(DdlPessoa.SelectedValue), Convert.ToInt32(DdlContato.SelectedValue));
            }
            else
            {
                if (DdlPessoa.SelectedValue != "..... SELECIONE UMA PESSOA .....")
                    txtCNPJCPF.Text = re.ObterInscricaoPessoaContato(Convert.ToInt64(DdlPessoa.SelectedValue), 0);
            }
        }
        protected void BtnAddContato_Click(object sender, EventArgs e)
        {
            if (DdlPessoa.SelectedValue != "..... SELECIONE UMA PESSOA .....")
            {
                MoveListaPrincipal(sender, e);
                Session["IncMovAcessoContato"] = listMovAcesso;
                Session["IncMovAcessoPessoa"] = null;
                Session["IncMovAcessoVeiculo"] = null;
                Session["IncMovAcessoTipoAcesso"] = null;

                Session["ZoomPessoa"] = DdlPessoa.SelectedValue.ToString();

                Session["ZoomCadPessoa3"] = "RELACIONAL";
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
            }
        }
        protected void DdlVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOBS.Focus();
        }
        protected void BtnAddVeiculo_Click(object sender, EventArgs e)
        {
            MoveListaPrincipal(sender, e);
            Session["IncMovAcessoVeiculo"] = listMovAcesso;
            Session["IncMovAcessoPessoa"] = null;
            Session["IncMovAcessoContato"] = null;
            Session["IncMovAcessoTipoAcesso"] = null;

            Session["ZoomVeiculo2"] = "RELACIONAL";
            Response.Redirect("~/Pages/Veiculos/CadVeiculo.aspx");

        }
        private void byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                System.Drawing.Image newImage;

                string strFileName = ConfigurationManager.AppSettings["ImagemDaCapturaFisica"].ToString();

                if (byteArrayIn != null)
                {
                    using (MemoryStream stream = new MemoryStream(byteArrayIn))
                    {
                        newImage = System.Drawing.Image.FromStream(stream);
                        newImage.Save(strFileName);
                    }
                }
                else
                {
                    Response.Write("No image data found!");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        protected void BtnCfmSimSaida_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            if (txtDataSaida.Text != "")
            {
                ShowMessage("Somente é permitido Gerar Saída do Movimento 1 Vez. ", MessageType.Warning);
                return;
            }

            DBTabelaDAL t = new DBTabelaDAL();
            MovAcessoDAL d = new MovAcessoDAL();
            MovAcesso p = new MovAcesso();

            txtDataSaida.Text = t.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm");

            if (txtCodMovAcesso.Text == "Novo")
            {
                p.Documento = txtDocumento.Text.ToUpper();
                p.DataHoraEntrada = Convert.ToDateTime(txtDataEntrada.Text);
                p.Observacoes = txtOBS.Text;

                if (txtDataSaida.Text != "")
                {
                    p.DataHoraSaida = Convert.ToDateTime(txtDataSaida.Text);
                }

                if (DdlTipoAcesso.SelectedValue != "..... SELECIONE UM TIPO DE ACESSO .....")
                    p.CodTipoAcesso = Convert.ToInt32(DdlTipoAcesso.SelectedValue);

                if (DdlPessoa.SelectedValue != "..... SELECIONE UMA PESSOA .....")
                    p.CodPessoa = Convert.ToInt64(DdlPessoa.SelectedValue);

                if (DdlContato.SelectedValue != "..... SELECIONE UM CONTATO .....")
                    p.CodContato = Convert.ToInt32(DdlContato.SelectedValue);

                if (DdlVeiculo.SelectedValue != "..... SELECIONE UM VEÍCULO .....")
                    p.CodVeiculo = Convert.ToInt64(DdlVeiculo.SelectedValue);

                if (txtCodMovAcesso.Text == "Novo")
                {
                    d.Inserir(p);
                    Session["MensagemTela"] = "Movimento de Acesso incluso e Gerada Saída com Sucesso!!!";
                }

            }

            else
            {
                p.CodMovAcesso = Convert.ToInt64(txtCodMovAcesso.Text);

                if (txtDataSaida.Text != "")
                {
                    p.DataHoraSaida = Convert.ToDateTime(txtDataSaida.Text);
                }

                d.GerarSaida(p);

                Session["MensagemTela"] = "Movimento de Acesso Gerada Saída com Sucesso!!!";
            }


            BtnVoltar_Click(sender, e);

        }
    }
}