using CrystalDecisions.CrystalReports.Engine;
using DAL.Model;
using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class RelCtaReceber: System.Web.UI.Page
    {
        ReportDocument RptDoc;

        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        string strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
            }
            else if (!IsPostBack)
            {
                Session["RptDoc"] = null;
                btnVoltar_Click(sender, e);
            }

            if ((Session["CodModulo"] != null) && (Session["CodPflUsuario"] != null))
            {
                List<Permissao> lista1 = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista1 = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "RelCtaReceber.aspx");
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
            Response.Redirect("~/Pages/Financeiros/ConCtaReceber.aspx");

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
            RptDoc = new ReportDocument();

            RptDoc.Load(Server.MapPath("~/Pages/Financeiros/RPT/RelCtaReceber.rpt"));

            Doc_CtaReceberDAL r = new Doc_CtaReceberDAL();
            List<DBTabelaCampos> lista = new List<DBTabelaCampos>();

            if (Session["LST_DOCCTARECEBER"] != null)
                lista = (List<DBTabelaCampos>)Session["LST_DOCCTARECEBER"];

            if (Session["NomeEmpresa"] != null)
                RptDoc.DataDefinition.FormulaFields["Empresa"].Text = "'" + Session["NomeEmpresa"].ToString() + "'";

            if (Session["UsuSis"] != null)
                RptDoc.DataDefinition.FormulaFields["Emissor"].Text = "'" + Session["UsuSis"].ToString() + "'";

            int i = 0;
            int filtro1 = 0;
            int filtro2 = 0;
            int filtro3 = 0;
            int filtro4 = 0;
            int filtro5 = 0;
            int filtro6 = 0;
            int filtro7 = 0;
            int filtro8 = 0;
            int filtro9 = 0;
            int filtro10 = 0;
            int filtro11 = 0;

            foreach (DBTabelaCampos tab in lista)
            {
                if (filtro1 == 0)
                {
                    if (tab.Filtro == "DT_HR_EMISSAO")
                    {
                        filtro1++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Emissão: " + filtro + " '";
                    }
                }
                if (filtro2 == 0)
                {
                    if (tab.Filtro == "DT_VENCIMENTO")
                    {
                        filtro2++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Venc.: " + filtro + " '";
                    }
                }
                if (filtro3 == 0)
                {
                    if (tab.Filtro == "CD_DOCUMENTO")
                    {
                        filtro3++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Código: " + filtro + " '";
                    }
                }
                if (filtro4 == 0)
                {
                    if (tab.Filtro == "DT_HR_ENTRADA")
                    {
                        filtro4++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Entrada: " + filtro + " '";

                    }
                }
                if (filtro5 == 0)
                {
                    if (tab.Filtro == "CD_FORNECEDOR")
                    {
                        filtro5++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Código Credor: " + filtro + " '";

                    }
                }
                if (filtro6 == 0)
                {
                    if (tab.Filtro == "RAZ_SOCIAL")
                    {
                        filtro6++;
                        i++;
                        string filtro;
                        if (tab.Inicio.Length > 9 && tab.Fim.Length > 9)
                        {

                            if (tab.Inicio == "")
                                filtro = tab.Fim.Substring(0, 9).ToUpper();
                            else if (tab.Fim == "")
                                filtro = tab.Inicio.Substring(0, 9).ToUpper();
                            else if (tab.Inicio == tab.Fim)
                                filtro = tab.Inicio.Substring(0, 9).ToUpper();
                            else
                                filtro = tab.Inicio.Substring(0, 9).ToUpper() + " a " + tab.Fim.Substring(0, 9).ToUpper();


                        }
                        else if (tab.Inicio.Length > 9)
                        {
                            if (tab.Inicio == "")
                                filtro = tab.Fim.ToUpper();
                            else if (tab.Fim == "")
                                filtro = tab.Inicio.Substring(0, 9).ToUpper();
                            else if (tab.Inicio == tab.Fim)
                                filtro = tab.Inicio.Substring(0, 9).ToUpper();
                            else
                                filtro = tab.Inicio.Substring(0, 9).ToUpper() + " a " + tab.Fim.ToUpper();
                        }
                        else if (tab.Fim.Length > 10)
                        {
                            if (tab.Inicio == "")
                                filtro = tab.Fim.Substring(0, 9).ToUpper();
                            else if (tab.Fim == "")
                                filtro = tab.Inicio.ToUpper();
                            else if (tab.Inicio == tab.Fim)
                                filtro = tab.Inicio.ToUpper();
                            else
                                filtro = tab.Inicio.ToUpper() + " a " + tab.Fim.Substring(0, 9).ToUpper();
                        }
                        else
                        {
                            if (tab.Inicio == "")
                                filtro = tab.Fim.ToUpper();
                            else if (tab.Fim == "")
                                filtro = tab.Inicio.ToUpper();
                            else if (tab.Inicio == tab.Fim)
                                filtro = tab.Inicio.ToUpper();
                            else
                                filtro = tab.Inicio.ToUpper() + " a " + tab.Fim.ToUpper();
                        }




                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Credor: " + filtro + " '";

                    }
                }
                if (filtro7 == 0)
                {
                    if (tab.Filtro == "CD_GPO_PESSOA")
                    {
                        filtro7++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Cod. Grupo Pessoa: " + filtro + " '";

                    }
                }
                if (filtro8 == 0)
                {
                    if (tab.Filtro == "DS_GPO_PESSOA")
                    {
                        filtro8++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Grupo Pessoa: " + filtro.ToUpper() + " '";

                    }
                }
                if (filtro9 == 0)
                {
                    if (tab.Filtro == "VL_TOTAL_DOCUMENTO")
                    {
                        filtro9++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a R$ " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Vl. Titulo: R$ " + filtro + " '";

                    }
                }
                if (filtro10 == 0)
                {
                    if (tab.Filtro == "NR_INSCRICAO")
                    {
                        filtro10++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Inscrição: " + filtro + " '";

                    }
                }
                if (filtro11 == 0)
                {
                    if (tab.Filtro == "DG_DOCUMENTO")
                    {
                        filtro11++;
                        i++;
                        string filtro;
                        if (tab.Inicio == "")
                            filtro = tab.Fim;
                        else if (tab.Fim == "")
                            filtro = tab.Inicio;
                        else if (tab.Inicio == tab.Fim)
                            filtro = tab.Inicio;
                        else
                            filtro = tab.Inicio + " a " + tab.Fim;
                        RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Doc. Digitado: " + filtro + " '";

                    }
                }


            }
            if (Session["FILTROS"] != null && Session["COD_FILTROS"] != null)
            {
                List<string> filtros = (List<string>)Session["FILTROS"];
                int tipos = 0;
                int tipos2 = 0;
                foreach (string f in filtros)
                {
                    tipos++;
                    if (tipos == 2)
                    {
                        if (f != "TODOS")
                        {
                            string cobranca = "";
                            if (f.Length > 15) { 
                                
                                string[] words = f.Split(' ');
                                foreach (string word in words)
                                {
                                    if (word == "BRASIL")
                                    {
                                        cobranca = cobranca + " BR";
                                    }
                                    else if (word == "BANRISUL")
                                    {
                                        cobranca = cobranca + " BANRI";
                                    }
                                    else if (word == "DEPOSITO" || word == "DEPÓSITO")
                                    {
                                        cobranca = cobranca + " DEP.";
                                    }
                                    else if (word == "CONTA")
                                    {
                                        cobranca = cobranca + " CTA";
                                    }
                                    else if (word == "BANCO")
                                    {
                                        cobranca = cobranca + " BNCO";
                                    }
                                    else if (word.Length > 5)
                                    {
                                        cobranca = cobranca + " " + word.Substring(0, 5) + ".";
                                    }
                                    else if (word.Length == 2)
                                    {
                                        cobranca = cobranca + "";
                                    }
                                    else
                                    {
                                        cobranca = cobranca + " " + word;
                                    }
                                }
                            }
                            else
                            {
                                cobranca = f;
                            }
                            i++;
                            tipos2++;
                            if (i == 1)
                                RptDoc.DataDefinition.FormulaFields["Filtro" + 1].Text = "'Cobrança: " + cobranca + "'";
                            else
                                RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Cobrança: " + cobranca + "'";

                        }
                    }
                    else if (tipos == 1)
                    {
                        if (f != "TODOS")
                        {
                            i++;
                            if (i == 1)
                            {
                                if (tipos2 == 0)
                                    RptDoc.DataDefinition.FormulaFields["Filtro" + 1].Text = "'Plano Conta: " + f + "'";
                                else
                                    RptDoc.DataDefinition.FormulaFields["Filtro" + 2].Text = "'Plano Conta: " + f + "'";

                            }
                            else
                                RptDoc.DataDefinition.FormulaFields["Filtro" + i].Text = "'Plano Conta: " + f + "'";
                        }
                    }
                    else if (tipos == 3)
                    {

                        RptDoc.DataDefinition.FormulaFields["Situacao"].Text = "' " + f + "'";
                    }
                }
                List<int> CodigoFiltros = (List<int>)Session["COD_FILTROS"];
                RptDoc.SetDataSource(r.RelCtaReceberCompleto(lista, CodigoFiltros));

            }

            CRViewer.ReportSource = RptDoc;

            Session["RptDoc"] = RptDoc;
            Session["LST_DOCCTARECEBER"] = null;
            Session["COD_FILTROS"] = null;
            Session["FILTROS"] = null;
        }

        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            MontaCrystal();
        }
    }
}