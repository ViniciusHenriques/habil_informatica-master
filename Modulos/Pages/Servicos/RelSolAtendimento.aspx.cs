using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;

namespace SoftHabilInformatica.Pages.Servicos
{
    public partial class RelSolAtendimento: System.Web.UI.Page
    {
        ReportDocument RptDoc;

        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        public enum MessageType { Success, Error, Info, Warning };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
            }

            if ((Session["CodModulo"] != null) && (Session["CodPflUsuario"] != null))
            {
                List<Permissao> lista1 = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista1 = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "RelSolAtendimento.aspx");
                lista1.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {

                        if (!x.AcessoImprimir)
                        {

                            CRViewer.HasExportButton = false;
                            CRViewer.HasPrintButton = false;
                            CRViewer.HasToggleGroupTreeButton = false;

                        }

                    }
                });

            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Servicos/ConSolAtendimento.aspx");

        }
        protected void btnSair_Click(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
        private void MontaCrystal()
        {
            if (Session["ZoomSolicitacaoAtendimento"] == null)
                Response.Redirect("~/Pages/Servicos/ConSolAtendimento.aspx");

            RptDoc = new ReportDocument();

            RptDoc.Load(Server.MapPath("~/Pages/Servicos/RPT/RelSolAtendimento.rpt"));

            Doc_SolicitacaoAtendimentoDAL r = new Doc_SolicitacaoAtendimentoDAL();

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            RptDoc.DataDefinition.FormulaFields["Imagem"].Text = "'" + System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Log\\ImagemRelatorio\\RelSolicAtendimento" + Session["ZoomSolicitacaoAtendimento"] +".jpg'";

            Doc_SolicitacaoAtendimento doc = new Doc_SolicitacaoAtendimento();
            doc = r.PesquisarDocumento(Convert.ToDecimal(Session["ZoomSolicitacaoAtendimento"]));
            //FORMULA TIPO SOLICITACAO

            if (doc.CodigoTipoSolicitacao == 93)
                RptDoc.DataDefinition.FormulaFields["Tipo_Solicitacao"].Text = "'NÃO INFORMADO'";
            else if (doc.CodigoTipoSolicitacao == 95)
            {
                RptDoc.DataDefinition.FormulaFields["Tipo_Solicitacao"].Text = "'SOLICITAÇÃO DO CLIENTE'";
                PessoaContatoDAL CttDAL = new PessoaContatoDAL();
                Pessoa_Contato Ctt = new Pessoa_Contato();
                Ctt = CttDAL.PesquisarPessoaContato(doc.Cpl_CodigoPessoa, doc.CodigoContato);
                RptDoc.Subreports["ModeloGenerico.rpt"].DataDefinition.FormulaFields["Solicitante"].Text = "'"+Ctt._NomeContato+"'";
            }
            else if (doc.CodigoTipoSolicitacao == 96)
                RptDoc.DataDefinition.FormulaFields["Tipo_Solicitacao"].Text = "'ORÇAMENTO'";
            else
                RptDoc.DataDefinition.FormulaFields["Tipo_Solicitacao"].Text = "'-'";

            //FORMULA NIVEL PRIORIDADE

            if (doc.CodigoNivelPrioridade == 90)
                RptDoc.DataDefinition.FormulaFields["Prioridade"].Text = "'BAIXO'";
            else if (doc.CodigoNivelPrioridade == 91)
                RptDoc.DataDefinition.FormulaFields["Prioridade"].Text = "'MÉDIO'";
            else if (doc.CodigoNivelPrioridade == 92)
                RptDoc.DataDefinition.FormulaFields["Prioridade"].Text = "'ALTO'";
            else
                RptDoc.DataDefinition.FormulaFields["Prioridade"].Text = "'-'";


            RptDoc.SetDataSource(r.RelSolicitacaoAtendimento(Convert.ToDecimal(Session["ZoomSolicitacaoAtendimento"])));

            
            CRViewer.ReportSource = RptDoc;
            Session["RptDoc"] = RptDoc;
            Session["LST_DOCCTARECEBER"] = null;
            Session["ZoomSolicitacaoAtendimento"] = null;
        }

        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            MontaCrystal();
        }
    }
}