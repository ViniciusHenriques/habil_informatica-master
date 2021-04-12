using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DAL;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using System.Drawing.Imaging;
namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class RelOrcamento : System.Web.UI.Page
    {
        ReportDocument RptDoc;

        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if(Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if ((Session["CodModulo"] != null) && (Session["CodPflUsuario"] != null))
            {
                List<Permissao> lista1 = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista1 = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "ConOrcamento.aspx");
                lista1.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {
                        if (!x.AcessoImprimir)
                        {
                            CRViewer.HasExportButton = false;
                            CRViewer.HasPrintButton = false;
                            CRViewer.HasToggleGroupTreeButton = false;
                            btnVisualizar.Visible = false;
                            btnImprimir.Visible = false;
                            ddlImpressora.Visible = false;
                        }
                    }
                });
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                if (Session["ZoomOrcamento"] == null)
                    btnVoltar_Click(sender, e);
            }

            if (!IsPostBack)
            {
                if (Session["ZoomOrcamento"] != null)
                {
                    Doc_Orcamento doc = new Doc_Orcamento();
                    Doc_OrcamentoDAL docDAL = new Doc_OrcamentoDAL();
                    doc = docDAL.PesquisarOrcamento(Convert.ToDecimal(Session["ZoomOrcamento"].ToString().Split('³')[0]));
                    if (doc == null)
                        btnVoltar_Click(sender, e);
                    txtCodigoDocumento.Text = doc.CodigoDocumento.ToString();
                    txtNroDocumento.Text = doc.NumeroDocumento.ToString();

                    Pessoa p = new Pessoa();
                    PessoaDAL pDAL = new PessoaDAL();
                    p = pDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
                    txtCodigoCliente.Text = p.CodigoPessoa.ToString();
                    txtCliente.Text = p.NomePessoa;

                    PessoaContatoDAL CttDAL = new PessoaContatoDAL();
                    ddlContatos.DataSource = CttDAL.ObterPessoaContatos(p.CodigoPessoa);
                    ddlContatos.DataValueField = "_CodigoItem";
                    ddlContatos.DataTextField = "_NomeContatoCombo";
                    ddlContatos.DataBind();
                    ddlContatos_SelectedIndexChanged(sender, e);


                }
                List<string> ListaImpressoras = new List<string>();
                foreach (string impressora in PrinterSettings.InstalledPrinters)
                {

                    ListaImpressoras.Add(impressora);
                }
                ddlImpressora.DataSource = ListaImpressoras ;
                ddlImpressora.DataBind();
            }
            

            if (ddlContatos.Items.Count == 0)
                btnVoltar_Click(sender, e);
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["ZoomOrcamento"] = null;
            Response.Redirect("~/Pages/Vendas/ConOrcamento.aspx");

        }
        protected void btnSair_Click(object sender, EventArgs e)
        {

        }

        private void MontaCrystal()
        {
            try
            { 
                if (Session["ZoomOrcamento"] == null)
                    btnVoltar_Click(null, null);

                RptDoc = new ReportDocument();
                string CaminhoExe = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

                List<DBTabelaCampos> lista = new List<DBTabelaCampos>();
                ProdutoDocumentoDAL prodDAL = new ProdutoDocumentoDAL();
                Doc_OrcamentoDAL orc = new Doc_OrcamentoDAL();
                clsValidacao v = new clsValidacao();

                DataTable dt = new DataTable();
                
                dt = orc.RelOrcamento(Convert.ToDecimal(Session["ZoomOrcamento"].ToString().Split('³')[0]));
                if(dt.Rows.Count == 0)
                    btnVoltar_Click(null, null);

                RptDoc.Load(Server.MapPath("~/Pages/Vendas/RPT/RelOrcamento.rpt"));

                RptDoc.DataDefinition.FormulaFields["LogoDaEmpresa"].Text = "'" + CaminhoExe + @"\Images\LogoDaEmpresa.jpg'";
                RptDoc.DataDefinition.FormulaFields["Data"].Text = "'" + v.RetornaDataPorExtenso(DateTime.Now) + "'";
                RptDoc.DataDefinition.FormulaFields["Consideracao_inicial_do_documento"].Text = "'" + txtConsInicial.Text.Replace('\n',' ') + "'";
                RptDoc.DataDefinition.FormulaFields["Prazo_Entrega"].Text = "'" + txtPrazoEntrega.Text.ToUpper() + "'";
                RptDoc.DataDefinition.FormulaFields["Telefone_cliente"].Text = "'" + txtFoneCliente.Text.ToUpper() + "'";

                RptDoc.Subreports["RelProdutoDocumento.rpt"].SetDataSource(prodDAL.RelProdutoDocumento(Convert.ToDecimal(Session["ZoomOrcamento"].ToString().Split('³')[0])));
                RptDoc.Subreports["RelProdutoDocumento.rpt"].DataDefinition.FormulaFields["Frete"].Text = "'" + dt.Rows[0][23].ToString() + "'";
                RptDoc.Subreports["RelProdutoDocumento.rpt"].DataDefinition.FormulaFields["Valor_ST"].Text = "'" + dt.Rows[0][28].ToString() + "'";

                RptDoc.SetDataSource(dt);
                CRViewer.ReportSource = RptDoc;
                Session["RptDoc"] = RptDoc;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }   
        }

        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            MontaCrystal();
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            
            btnVisualizar.Visible = false;
            btnEnviarEmail.Visible = true;
            pnlInfos.Visible = false;
            pnlCRViewer.Visible = true;
            btnEditar.Visible = true;
            
            if (IsPostBack)
            {
                Session["RptDoc"] = null;
                RptDoc = null;
                MontaCrystal();
            }
            if (Session["RptDoc"] != null)
            {
                CRViewer.ReportSource = (ReportDocument)Session["RptDoc"];
                CRViewer.DataBind();
            }
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                HabilEmailCriado Hec_Mail = new HabilEmailCriado();
                HabilEmailCriadoDAL Hec_Mail2 = new HabilEmailCriadoDAL();
                HabilEmailDestinatario Hec_MailDest = new HabilEmailDestinatario();
                HabilEmailAnexo Hec_MailAnexo = new HabilEmailAnexo();

                List<HabilEmailCriado> listMails = new List<HabilEmailCriado>();
                List<HabilEmailAnexo> listAnexos = new List<HabilEmailAnexo>();
                List<HabilEmailDestinatario> listDestinatarios = new List<HabilEmailDestinatario>();
                AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
                
                ReportDocument rpt = (ReportDocument)Session["RptDoc"];
                Hec_Mail.CD_USU_REMETENTE = 0;
                Hec_Mail.IN_HTML = 1;
                Hec_Mail.TX_ASSUNTO = Session["NomeEmpresa"].ToString() + " - Orçamento Código " + txtCodigoDocumento.Text + @"\ N° "+txtNroDocumento.Text;
                Hec_Mail.TX_CORPO = "";
                Hec_Mail.CD_SITUACAO = 110;
                Hec_Mail.CD_USU_REMETENTE = Convert.ToInt64(Session["CodUsuario"]);
                listMails.Add(Hec_Mail);
                
                Hec_MailDest = new HabilEmailDestinatario();
                Hec_MailDest.CD_EMAIL_DESTINATARIO = 1;
                Hec_MailDest.TP_DESTINATARIO = 1;
                Hec_MailDest.NM_DESTINATARIO = txtCliente.Text;
                Hec_MailDest.TX_EMAIL = txtEmailCliente.Text;
                listDestinatarios.Add(Hec_MailDest);

                Stream stre = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                BinaryReader br = new BinaryReader(stre);
                Hec_MailAnexo = new HabilEmailAnexo();
                Hec_MailAnexo.CD_EXTENSAO = 2;
                Hec_MailAnexo.DS_ARQUIVO = anexo.GerarGUIDPorEmpresa(Session["NomeEmpresa"].ToString().Split(' ')[0], "pdf");
                Hec_MailAnexo.TX_CONTEUDO = br.ReadBytes(Convert.ToInt32(stre.Length));
                Hec_MailAnexo.CD_ANEXO = 1;
                listAnexos.Add(Hec_MailAnexo);

                long longCodigoIndexEmail = 0;
                Hec_Mail2.Gera_Email(listMails, listDestinatarios, listAnexos, ref longCodigoIndexEmail);
                Session["ZoomGeracaiDosEmails"] = longCodigoIndexEmail + "³";

                Response.Redirect("~/Pages/HabilUtilitarios/ManGerEmails.aspx?IN=1");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void ddlContatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Pessoa_Contato ctt = new Pessoa_Contato();
                PessoaContatoDAL cttDAL = new PessoaContatoDAL();
                ctt = cttDAL.PesquisarPessoaContato(Convert.ToInt64(txtCodigoCliente.Text), Convert.ToInt32(ddlContatos.SelectedValue));
                txtFoneCliente.Text = ctt._Fone1;
                txtEmailCliente.Text = ctt._Mail1;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnImprimir.Visible)
                    btnImprimir_Click(sender, e);

                RptDoc.PrintOptions.PrinterName = (@ddlImpressora.SelectedValue);
                RptDoc.PrintToPrinter(1, false, 0, 0);
                ShowMessage("Orçamento enviado para impressora, aguarde...", MessageType.Success);
            }
            catch(Exception ex)
            {
                ShowMessage("Erro ao se conectar com a impressora. " + ex.Message, MessageType.Error);
            }
            pnlInfos.Visible = true;
            pnlCRViewer.Visible = false;
            btnVisualizar.Visible = true;
            btnEnviarEmail.Visible = false;
            btnEditar.Visible = false;
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            pnlInfos.Visible = true;
            pnlCRViewer.Visible = false;
            btnVisualizar.Visible = true;
            btnEnviarEmail.Visible = false;
            btnEditar.Visible = false;
        }
    }
}