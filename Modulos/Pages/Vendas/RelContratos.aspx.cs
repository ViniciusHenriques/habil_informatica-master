using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DAL.Model;
using DAL.Persistence;
using System.IO;
using System.Xml;
using ClosedXML.Excel;
using System.Data;
using System.Threading;
using System.Windows;
using Ookii.Dialogs.Wpf ;
using Aspose.Cells;
namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class RelContratos : System.Web.UI.Page
    {
        public enum MessageType { Success, Error, Info, Warning };

        String strMensagemR = "";

        public string PanelSelect { get; set; }

        clsValidacao v = new clsValidacao();

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void CarregarDropDownList()
        {
            EmpresaDAL RnEmpresa = new EmpresaDAL();
            ddlEmpresa.DataSource = RnEmpresa.ListarEmpresas("CD_SITUACAO", "INT", "1", "");
            ddlEmpresa.DataTextField = "NomeEmpresa";
            ddlEmpresa.DataValueField = "CodigoEmpresa";
            ddlEmpresa.DataBind();
            if (Session["CodEmpresa"] != null)
                ddlEmpresa.SelectedValue = Session["CodEmpresa"].ToString();
        }

        protected Boolean ValidaCampos()
        {
            if (ddlCliente.SelectedValue == "*Nenhum cliente com contrato" || ddlCliente.SelectedValue == "")
            {
                ShowMessage("Escolha um cliente", MessageType.Info);
                return false;
            }

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            List<Permissao> lista = new List<Permissao>();
            PermissaoDAL r1 = new PermissaoDAL();
            lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                "RelContratos.aspx");
            lista.ForEach(delegate (Permissao x)
            {
                if (!x.AcessoCompleto)
                {
                    if (!x.AcessoAlterar)
                    {
                        btnGerarPlanilha.Visible = false;
                    }
                }
            });
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
                CarregarDropDownList();
                Session["Pagina"] = Request.CurrentExecutionFilePath;
            }


            if (!IsPostBack)
            {
                RelatorioContratosDAL relContr = new RelatorioContratosDAL();
                ddlGpoCliente.DataSource = relContr.ListarGruposCliente();
                ddlGpoCliente.DataTextField = "nome";
                ddlGpoCliente.DataValueField = "matriz_cgru";
                ddlGpoCliente.DataBind();
                ddlGpoCliente_TextChanged(sender, e);

                Session["ListaProdutos"] = null;
            }
            if(ddlEmpresa.Items.Count == 0)
                btnFechar_Click(sender, e);

        }

        protected void btnGerarPlanilha_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidaCampos())
                    return;

                AnexoDocumentoDAL ad = new AnexoDocumentoDAL();

                string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\";
                
                string NomeXlsxGerado = ad.GerarGUID("xlsx");

                string strErro = "";
                bool GerouExcel = GerarArquivoExcel(CaminhoArquivoLog, NomeXlsxGerado, ref strErro);
                string[] arquivos = Directory.GetFiles(CaminhoArquivoLog, "*.xlsx", SearchOption.AllDirectories);

                foreach (string arq in arquivos)
                {
                    if (arq != CaminhoArquivoLog + NomeXlsxGerado)
                    {
                        FileInfo fi = new FileInfo(arq);
                        if (fi.LastWriteTime.ToString("dd/MM/yyyy") != DateTime.Now.ToString("dd/MM/yyyy"))
                        {
                            if (System.IO.File.Exists(arq))
                                System.IO.File.Delete(arq);
                        }
                    }
                }

                if (GerouExcel)
                {
                    Response.Clear();
                    Response.ContentType = "application/octect-stream";
                    Response.AppendHeader("content-disposition", "filename=" + NomeXlsxGerado);
                    Response.TransmitFile(CaminhoArquivoLog + NomeXlsxGerado);
                    Response.End();
                }
                else
                {
                    if(strErro != "")
                        ShowMessage(strErro, MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Info);
            }
        }

        protected List<string> VerificarEstadosSelecionados()
        {
            List<string> lista = new List<string>();

            if (ChkAC.Checked)
                lista.Add("AC");
            if (ChkAL.Checked)
                lista.Add("AL");
            if (ChkAP.Checked)
                lista.Add("AP");
            if (ChkAM.Checked)
                lista.Add("AM");
            if (ChkBA.Checked)
                lista.Add("BA");
            if (ChkCE.Checked)
                lista.Add("CE");
            if (ChkES.Checked)
                lista.Add("ES");
            if (ChkGO.Checked)
                lista.Add("GO");
            if (ChkMA.Checked)
                lista.Add("MA");
            if (ChkMT.Checked)
                lista.Add("MT");
            if (ChkMS.Checked)
                lista.Add("MS");
            if (ChkMG.Checked)
                lista.Add("MG");
            if (ChkPA.Checked)
                lista.Add("PA");
            if (ChkPB.Checked)
                lista.Add("PB");
            if (ChkPR.Checked)
                lista.Add("PR");
            if (ChkPE.Checked)
                lista.Add("PE");
            if (ChkPI.Checked)
                lista.Add("PI");
            if (ChkRJ.Checked)
                lista.Add("RJ");
            if (ChkRN.Checked)
                lista.Add("RN");
            if (ChkRS.Checked)
                lista.Add("RS");
            if (ChkRR.Checked)
                lista.Add("RR");
            if (ChkSC.Checked)
                lista.Add("SC");
            if (ChkSP.Checked)
                lista.Add("SP");
            if (ChkSE.Checked)
                lista.Add("SE");
            if (ChkTO.Checked)
                lista.Add("TO");
            if (ChkDF.Checked)
                lista.Add("DF");


            return lista;
        }

        public static string GetColNameFromIndex(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
        
        protected bool GerarArquivoExcel(string CaminhoArquivo, string NomeArquivo, ref string strErro)
        {
            if (ValidaCampos() == false)
                return false;

            PanelSelect = "home";
            Session["TabFocada"] = "home";

            Habil_LogDAL log = new Habil_LogDAL();
            try
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Planilha 1");

                IXLSheetView view = ws.SheetView;
                view.ZoomScale = 80;
                view.FreezeRows(3);

                //cabeçalho do relatorio
                ws.Range("A1:E1").Style.Font.FontSize = 15;
                
                ws.Cell("A1").Value = DateTime.Now;
                ws.Cell("B1").Value = Convert.ToInt32(ddlEmpresa.SelectedItem.Value).ToString("D2") + " - " + ddlEmpresa.SelectedItem.Text;    
                
                if(ChkCodBarras.Checked)
                    ws.Range("C1:H1").Merge();
                else
                    ws.Range("C1:G1").Merge();

                ws.Cell("A2").Value = "Código Produto";

                ws.Cell("B2").Value = "Descrição Produto";
                int ColunaExcelCabeçalho = 3;

                if (ChkGrupoProdutos.Checked)
                {
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "Grupo Prod.";
                    ColunaExcelCabeçalho++;
                }

                ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "Unid";
                ColunaExcelCabeçalho++;

                ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "Cardex";
                ColunaExcelCabeçalho++;

                if (ChkCodBarras.Checked)
                {
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "Código Barras";
                    ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho) + "1:" + GetColNameFromIndex(8) + "1").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                    ColunaExcelCabeçalho++;
                }
                else
                {
                    ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho) + "1:" + GetColNameFromIndex(7) + "1").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                }

                ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "NCM";
                ColunaExcelCabeçalho++;

                ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "Qtde. Orçada";
                ColunaExcelCabeçalho++;

                ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "Preço s/ ICMS";
                ws.Column(GetColNameFromIndex(ColunaExcelCabeçalho)).Style.NumberFormat.Format = "#,##0.00";
                ws.Range("A1:"+ GetColNameFromIndex(ColunaExcelCabeçalho) + "1").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                ColunaExcelCabeçalho++;

  
                int LinhaExcel = 4;

                if (ChkAdicionarCESTPESO.Checked)
                {
                    ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho) + "1:"+ GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "1").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "Peso";
                    ColunaExcelCabeçalho++;

                    ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho - 1) + "2:"+ GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "CEST";

                    ColunaExcelCabeçalho ++;
                }

                RelatorioContratosDAL relContratos = new RelatorioContratosDAL();
                DataTable dt = new DataTable();

                List<string> ListaSiglaEstadosOrdemAlfabetica = new List<string>();
                List<string> ListaSiglaEstadosOrigemPrimeiro = new List<string>();
                ListaSiglaEstadosOrdemAlfabetica = VerificarEstadosSelecionados();

                if(ListaSiglaEstadosOrdemAlfabetica.Count() == 0)
                {
                    ShowMessage("Selecione algum(ns) estado(s)", MessageType.Info);
                    return false;
                }

                if(ddlCliente.SelectedValue == "")
                {
                    ShowMessage("Este cliente não existe no banco de dados do sistema anterior", MessageType.Info);
                    return false;
                }
                List<Pessoa_Endereco> ListaEnd = new List<Pessoa_Endereco>();
                Pessoa_Endereco pEnd = new Pessoa_Endereco();
                PessoaEnderecoDAL EndDAL = new PessoaEnderecoDAL();
                ListaEnd = EndDAL.ObterPessoaEnderecos(Convert.ToInt64(ddlEmpresa.SelectedValue));
                pEnd = ListaEnd.Where(x => x._TipoEndereco == 5).ToList()[0];
                string strEstadosConcatenados = "";

                for (int i = 0; i < ListaSiglaEstadosOrdemAlfabetica.Count; i++)
                {
                    if (ListaSiglaEstadosOrdemAlfabetica[i] == pEnd._DescricaoEstado.Substring(0, 2))
                    {
                        strEstadosConcatenados = ListaSiglaEstadosOrdemAlfabetica[i];
                        ListaSiglaEstadosOrigemPrimeiro.Add(ListaSiglaEstadosOrdemAlfabetica[i]);
                    }
                }
                for (int i = 0; i < ListaSiglaEstadosOrdemAlfabetica.Count; i++)
                {
                    if (ListaSiglaEstadosOrdemAlfabetica[i] != pEnd._DescricaoEstado.Substring(0, 2))
                    {
                        strEstadosConcatenados += ListaSiglaEstadosOrdemAlfabetica[i];
                        ListaSiglaEstadosOrigemPrimeiro.Add(ListaSiglaEstadosOrdemAlfabetica[i]);
                    }
                }

                decimal CodigoSisAnterior = 0;
                if (!decimal.TryParse(ddlCliente.SelectedValue, out CodigoSisAnterior))
                {
                    CodigoSisAnterior = Convert.ToDecimal(ddlCliente.SelectedValue.Substring(3));
                }
                else
                {
                    CodigoSisAnterior = Convert.ToDecimal(ddlCliente.SelectedValue);
                }

                if (ChkSomarST.Checked)
                    dt = relContratos.ExecutaSpImpCliCTm(Convert.ToInt32(ddlEmpresa.SelectedValue).ToString("D2"), CodigoSisAnterior.ToString(), ChkPrecoUnico.Checked, ChkSomarST.Checked, ChkOrdemItem.Checked, strEstadosConcatenados);
                else
                    dt = relContratos.ExecutaSpImpCliCT(Convert.ToInt32(ddlEmpresa.SelectedValue).ToString("D2"), CodigoSisAnterior.ToString(), ChkPrecoUnico.Checked, ChkSomarST.Checked,ChkOrdemItem.Checked, strEstadosConcatenados);

                for (int i = 0; i < ListaSiglaEstadosOrigemPrimeiro.Count(); i++)
                {
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "1").Value = ListaSiglaEstadosOrigemPrimeiro[i];
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "1").Style.Font.FontSize = 20;
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "1").Style.Font.Bold = true;

                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho) + "2").Value = "CST";
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "2").Value = "% ICMS";
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho + 2) + "2").Value = "% Redução";
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho + 3) + "2").Value = "% MVA";
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho + 4) + "2").Value = "% FCP";
                    ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho + 5) + "2").Value = "Preço Contrato";

                    ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 5) + "1").Merge();

                    if ((i % 2) == 0)
                        ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 5) + "2").Style.Fill.BackgroundColor = XLColor.YellowGreen;
                    else
                        ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 5) + "2").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                    ColunaExcelCabeçalho = Convert.ToChar(ColunaExcelCabeçalho + 6);
                }

                if(dt.Rows.Count == 0)
                {
                    ShowMessage("Não existe nenhum contrato com este cliente", MessageType.Warning);
                    return false;
                }

                foreach (DataRow row in dt.Rows)
                {

                    bool AdicionarLinha = true;
                    string strOrigem = "";

                    int ColunaExcelDados = 9;
                    int ColunaExcelComCodBarras = 3;

                    if (ChkAdicionarCESTPESO.Checked)
                        ColunaExcelDados += 2;

                    if (!ChkCodBarras.Checked)
                        ColunaExcelDados--;

                    string strCodigoProduto = "";
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (ChkGrupoProdutos.Checked && strCodigoProduto != "")
                        {
                            ws.Cell(GetColNameFromIndex(ColunaExcelComCodBarras) + LinhaExcel).Value = relContratos.PesquisarGrupoDoProduto(strCodigoProduto);
                            ColunaExcelComCodBarras++;
                            ColunaExcelDados ++;
                            strCodigoProduto = "";
                        }
                        if (AdicionarLinha)
                        {
                            if (column.ColumnName == "CodProduto")
                            {
                                if (ChkAdicionarCESTPESO.Checked || ChkProdutosInativos.Checked || ChkOrigem.Checked)
                                {
                                    DataTable dtInfoProduto = new DataTable();
                                    dtInfoProduto = relContratos.PesquisarProduto(row[column].ToString());

                                    foreach (DataRow rowProduto in dtInfoProduto.Rows)
                                    {
                                        foreach (DataColumn columnProduto in dtInfoProduto.Columns)
                                        {
                                            if (columnProduto.ColumnName == "situ" && rowProduto[columnProduto].ToString() == "Inativo" && ChkProdutosInativos.Checked)
                                                AdicionarLinha = true;
                                            else if (columnProduto.ColumnName == "situ" && rowProduto[columnProduto].ToString() != "Inativo" && ChkProdutosInativos.Checked)
                                                AdicionarLinha = false;
                                            else if (columnProduto.ColumnName == "peso" && ChkAdicionarCESTPESO.Checked && AdicionarLinha)
                                            {
                                                if (ChkCodBarras.Checked)
                                                    ws.Cell("I" + LinhaExcel).Value = rowProduto[columnProduto].ToString();
                                                else
                                                    ws.Cell("H" + LinhaExcel).Value = rowProduto[columnProduto].ToString();
                                            }
                                            else if (columnProduto.ColumnName == "CD_CEST" && ChkAdicionarCESTPESO.Checked && AdicionarLinha)
                                            {
                                                if (ChkCodBarras.Checked)
                                                    ws.Cell("J" + LinhaExcel).Value = rowProduto[columnProduto].ToString();
                                                else
                                                    ws.Cell("I" + LinhaExcel).Value = rowProduto[columnProduto].ToString();
                                            }
                                            else if (columnProduto.ColumnName == "orig" && ChkOrigem.Checked)
                                                strOrigem = rowProduto[columnProduto].ToString();
                                        }
                                    }
                                }
                                if (AdicionarLinha)
                                    ws.Cell("A" + LinhaExcel).Value = row[column].ToString();

                                if (ChkHyperlink.Checked)
                                    ws.Cell("A" + LinhaExcel).Hyperlink = new XLHyperlink(@"http://www.fabesul.com.br/fabesul/WebForms/produto.aspx?cpro=" + row[column].ToString());

                                strCodigoProduto = row[column].ToString();
                            }
                            else if (column.ColumnName == "DcrProduto")
                                ws.Cell("B" + LinhaExcel).Value = row[column].ToString();
                            
                            else if (column.ColumnName == "Unidade")
                            {
                                ws.Cell(GetColNameFromIndex(ColunaExcelComCodBarras) + LinhaExcel).Value = row[column].ToString();
                                ColunaExcelComCodBarras++;
                            }
                            else if (column.ColumnName == "Cardex")
                            {
                                ws.Cell(GetColNameFromIndex(ColunaExcelComCodBarras) + LinhaExcel).Value = row[column].ToString();
                                ColunaExcelComCodBarras++;
                            }
                            else if (column.ColumnName == "CodBarras" && ChkCodBarras.Checked)
                            {
                                ws.Cell(GetColNameFromIndex(ColunaExcelComCodBarras) + LinhaExcel).Value = "'" + row[column].ToString();
                                ColunaExcelComCodBarras++;
                            }
                            else if (column.ColumnName == "ClfFiscal")
                            {
                                ws.Cell(GetColNameFromIndex(ColunaExcelComCodBarras) + LinhaExcel).Value = row[column].ToString();
                                ColunaExcelComCodBarras++;
                            }
                            else if (column.ColumnName == "QtdeOrcada")
                            {
                                string TEST = row[column].ToString();
                                if (!DBNull.Value.Equals(row[column]))
                                {
                                    ws.Cell(GetColNameFromIndex(ColunaExcelComCodBarras) + LinhaExcel).Value = row[column].ToString();
                                }
                                ColunaExcelComCodBarras++;
                            }
                            else if (column.ColumnName == "PrcSemIcms")
                            {
                                ws.Cell(GetColNameFromIndex(ColunaExcelComCodBarras) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString()).ToString("F").Replace(',', '.');
                                ColunaExcelComCodBarras++;
                            }
                            else if (column.ColumnName == "NomeCliente")
                                ws.Cell("C1").Value = ddlCliente.SelectedValue + " - " + row[column].ToString();
                            else if (column.ColumnName == "PrTab")
                                ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString()).ToString("F").Replace(',', '.');
                            else
                            {
                                
                                for (int i = 1; i <= ListaSiglaEstadosOrigemPrimeiro.Count(); i++)
                                {
                                    if (column.ColumnName == "Sitt" + i.ToString("D2"))
                                    {
                                        if (ChkOrigem.Checked)
                                        {
                                            ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Style.NumberFormat.Format = "###000";
                                            ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = strOrigem + row[column].ToString();
                                        }
                                        else
                                        {
                                            ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Style.NumberFormat.Format = "##00";
                                            ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = row[column].ToString();
                                        }

                                        ColunaExcelDados = Convert.ToChar(ColunaExcelDados + 1);
                                    }
                                    else if (column.ColumnName == "Icms" + i.ToString("D2"))
                                    {
                                        string ValorST = ws.Cell(GetColNameFromIndex(ColunaExcelDados - 1) + LinhaExcel).Value.ToString();
                                        string UF = ws.Cell(GetColNameFromIndex(ColunaExcelDados - 1) + 5).Value.ToString();
                                        if (ChkOrigem.Checked && ValorST.Length == 3)
                                            ValorST = ValorST.Substring(1);
                                        if (ValorST == "60")
                                            ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = "00";
                                        else if (ValorST == "51" || ValorST == "20")
                                        {
                                            if (UF == "SC")
                                                ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = "17";
                                            else if (UF == "RS" || UF == "PR" || UF == "SP")
                                                ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = "18";
                                            else
                                                ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = row[column].ToString();
                                        }
                                        else
                                            ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = row[column].ToString();
                                        ColunaExcelDados = Convert.ToChar(ColunaExcelDados + 1);
                                    }
                                    else if (column.ColumnName == "Redu" + i.ToString("D2"))
                                    {
                                        ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString()).ToString("F").Replace(',', '.');
                                        ws.Column(GetColNameFromIndex(ColunaExcelDados)).Style.NumberFormat.Format = "#,##0.00";
                                        ColunaExcelDados = Convert.ToChar(ColunaExcelDados + 1);
                                    }
                                    else if (column.ColumnName == "MvaE" + i.ToString("D2"))
                                    {
                                        ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString()).ToString("F").Replace(',', '.');
                                        ws.Column(GetColNameFromIndex(ColunaExcelDados)).Style.NumberFormat.Format = "#,##0.00";
                                        ColunaExcelDados = Convert.ToChar(ColunaExcelDados + 1);
                                    }
                                    else if (column.ColumnName == "PFCP" + i.ToString("D2"))
                                    {
                                        decimal FCP = Convert.ToDecimal(row[column].ToString());
                                        ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = FCP.ToString("F");
                                        ColunaExcelDados = Convert.ToChar(ColunaExcelDados + 1);
                                        if (FCP > 0)
                                        {
                                            decimal ICMS = Convert.ToDecimal(ws.Cell(GetColNameFromIndex(ColunaExcelDados - 4) + (LinhaExcel)).Value.ToString());
                                            ws.Cell(GetColNameFromIndex(ColunaExcelDados - 4) + (LinhaExcel)).Value = (ICMS - FCP).ToString("F");
                                        }
                                    }
                                    else if (column.ColumnName == "PrFi" + i.ToString("D2"))
                                    {
                                        
                                        if (row["TPCu"].ToString() == "C")
                                        {
                                            if (row["PreCT"] != DBNull.Value)
                                                ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = Convert.ToDecimal(row["PreCT"].ToString()).ToString("F").Replace(',', '.');
                                        }
                                        else
                                            ws.Cell(GetColNameFromIndex(ColunaExcelDados) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString()).ToString("F").Replace(',', '.');

                                        ws.Column(GetColNameFromIndex(ColunaExcelDados)).Style.NumberFormat.Format = "#,##0.00";
                                        ColunaExcelDados = Convert.ToChar(ColunaExcelDados + 1);
                                        
                                    }
                                }
                            }

                        }
                    }

                    if (AdicionarLinha)
                        LinhaExcel++;
                }

                ws.Cell(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "2").Value = "Preço de lista";
                ws.Column(GetColNameFromIndex(ColunaExcelCabeçalho + 1)).Style.NumberFormat.Format = "#,##0.00";

                ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "2").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154); 
                ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 1) + (LinhaExcel - 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 1) + (LinhaExcel - 1)).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 1) + (LinhaExcel - 1)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 1) + (LinhaExcel - 1)).Style.Border.InsideBorderColor = XLColor.Black;
                ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 1) + (LinhaExcel - 1)).Style.Border.InsideBorderColor = XLColor.Black;
                ws.Range(GetColNameFromIndex(ColunaExcelCabeçalho + 1) + "1:" + GetColNameFromIndex(ColunaExcelCabeçalho + 1) + (LinhaExcel - 1)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                int ColunaSemCodBarras = 7;
                if (ChkCodBarras.Checked)
                    ColunaSemCodBarras = 8;

                if(ChkGrupoProdutos.Checked)
                    ws.Range("A2:" + GetColNameFromIndex(ColunaSemCodBarras + 1) + "2").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                else
                    ws.Range("A2:" + GetColNameFromIndex(ColunaSemCodBarras) + "2").Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                ws.Range("A1:" + GetColNameFromIndex(ColunaSemCodBarras) + "2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:" + GetColNameFromIndex(ColunaSemCodBarras) + "2").Style.Border.OutsideBorderColor = XLColor.Black;
              
                ws.Range("A1:" + GetColNameFromIndex(ColunaExcelCabeçalho - 1) + (LinhaExcel - 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:" + GetColNameFromIndex(ColunaExcelCabeçalho - 1) + (LinhaExcel - 1)).Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("A1:" + GetColNameFromIndex(ColunaSemCodBarras) + "2").Style.Border.InsideBorderColor = XLColor.Black;
                ws.Range("A1:" + GetColNameFromIndex(ColunaSemCodBarras) + "2").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                                      
                ws.Range("A1:" + GetColNameFromIndex(ColunaExcelCabeçalho - 1) + (LinhaExcel - 1)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:" + GetColNameFromIndex(ColunaExcelCabeçalho - 1) + (LinhaExcel - 1)).Style.Border.InsideBorderColor = XLColor.Black;

                ws.Columns("A-" + GetColNameFromIndex(ColunaExcelCabeçalho + 1)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Columns("A-" + GetColNameFromIndex(ColunaExcelCabeçalho + 1)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Range("A1:C1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column("B").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Columns("C-"+GetColNameFromIndex(ColunaExcelCabeçalho + 1)).Width = 9;
                //ws.Column(GetColNameFromIndex(ColunaSemCodBarras)).Width = 20;

                ws.Column("A").Width = 10;
                ws.Column("B").Width = 20;
                ws.Column("G").Width = 7;
                ws.Column("E").Width = 12;
                ws.Columns("C-D").Width = 7;

                ws.Row(2).Style.Alignment.WrapText = true;
                ws.Row(2).Height = 35;

                //salvar arquivo em disco
                wb.SaveAs(CaminhoArquivo + @"\" + NomeArquivo);
                //liberar objetos
                wb.Dispose();

                log.GerandoArquivoLog("Relatório de contratos gerado pelo usuário " + Session["UsuSis"] + " / " + Session["CodUsuario"]);
                return true;
            }
            catch (Exception ex)
            {
                log.GerandoArquivoLog("Erro ao gerar relatório de contratos - " + ex.Message + " - tentativa do usuário " + Session["UsuSis"] + " / " + Session["CodUsuario"]);
                ShowMessage(ex.Message, MessageType.Error);
                strErro = ex.Message;
                return false;
            }
        }

        protected void btnFechar_Click(object sender, EventArgs e)
        {
            Session["ListaProdutos"] = null;
            Response.Redirect("~/Pages/Welcome.aspx");
        }

        protected void ddlGpoCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {

                RelatorioContratosDAL relContr = new RelatorioContratosDAL();
                DataTable dt = new DataTable();
                
                dt = relContr.PesquisarClientePorGrupo(ddlGpoCliente.SelectedValue.Split('³')[1]);
                ddlCliente.DataSource = dt;
                ddlCliente.DataTextField = "nomecli";
                ddlCliente.DataValueField = "ccli";
                ddlCliente.DataBind();
                ddlCliente.SelectedValue = ddlGpoCliente.SelectedValue.Split('³')[0];
                if (dt.Rows.Count == 0)
                    ddlCliente.Items.Insert(0, "*Nenhum cliente com contrato");
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                PanelSelect = "profile";
                Session["TabFocada"] = "profile";

                if (fileselector.HasFile )
                {
                    string ex = fileselector.FileName;
                    string ex2 = Path.GetExtension(ex);

                    if (ex2 == ".xlsx" || ex2 == ".xls")
                    {
                        DataTable dt = new DataTable();
                        AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();
                        dt.Columns.Add("Codigo_Produto");
                        dt.Columns.Add("Descricao_Produto");
                        dt.Columns.Add("Cardex");
                        dt.Columns.Add("Unidade");
                        dt.Columns.Add("Preco_Tabela");
                        dt.Columns.Add("Cliente");


                        string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\";
                        string fileName = anexo.GerarGUID(ex2.Replace(".",""));

                        BinaryReader br = new BinaryReader(fileselector.PostedFile.InputStream);

                        byte[] ArquivoByte = fileselector.FileBytes;

                        if (System.IO.File.Exists(CaminhoArquivoLog + fileName))
                            System.IO.File.Delete(CaminhoArquivoLog + fileName);

                        FileStream file = new FileStream(CaminhoArquivoLog + fileName, FileMode.Create);

                        BinaryWriter bw = new BinaryWriter(file);
                        bw.Write(ArquivoByte);
                        bw.Close();

                        if (ex2 == ".xls")//CONVERTER ARQUIVO XLS PARA XLSX
                        {
                            Workbook work = new Workbook(CaminhoArquivoLog + fileName);
                            fileName = fileName.Replace(".xls", ".xlsx");
                            work.Save(CaminhoArquivoLog + fileName, SaveFormat.Xlsx);
                        }

                        var workbook = new XLWorkbook(CaminhoArquivoLog + fileName);
                        var ws = workbook.Worksheet(1);
                        int linha = 1;

                        foreach (IXLRow row in ws.RowsUsed())
                        {
                            linha++;
                            if (linha > 4 && ws.Cell("B" + linha).Value.ToString() != "")
                            {
                                int ContadorErros = 0;

                                DataRow dtrow = dt.NewRow();

                                decimal s2 = 0;
                                if (decimal.TryParse(ws.Cell("A" + linha).Value.ToString(), out s2))
                                {
                                    dtrow["Codigo_Produto"] = ws.Cell("A" + linha).Value;
                                }
                                else
                                    ContadorErros++;

                                dtrow["Descricao_Produto"] = ws.Cell("B" + linha).Value;
                                dtrow["Cardex"] = ws.Cell("C" + linha).Value;
                                dtrow["Unidade"] = ws.Cell("D" + linha).Value;
                                
                                decimal s1 = 0;
                                if (decimal.TryParse(ws.Cell("E" + linha).Value.ToString(), out s1))
                                {
                                    dtrow["Preco_Tabela"] = Convert.ToDecimal(ws.Cell("E" + linha).Value).ToString("F");
                                }
                                else
                                    ContadorErros++;

                                if (ws.Cell("C2").Value.ToString() != "")
                                    dtrow["Cliente"] = ws.Cell("C2").Value;
                                else if (ws.Cell("A2").Value.ToString() != "")
                                    dtrow["Cliente"] = ws.Cell("A2").Value;
                                else if (ws.Cell("B2").Value.ToString() != "")
                                    dtrow["Cliente"] = ws.Cell("B2").Value;

                                if(ContadorErros == 0 )
                                    dt.Rows.Add(dtrow);
                            }
                        }
                        grdXMLs.DataSource = dt;
                        grdXMLs.DataBind();
                        file.Close();

                        Session["ListaProdutos"] = dt;

                        
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Popup", "MostrarDadosExcel('Atenção! Deseja importar esses dados?</br> No total foram validados "+dt.Rows.Count+" itens');", true);
                    }
                    else
                    {
                        ShowMessage("Importação apenas com arquivo xlsx", MessageType.Info);
                    }
                }
                else
                {
                    ShowMessage("Selecione um arquivo para importação", MessageType.Info);
                }
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Info);
            }
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ListaProdutos"];

                string strGUID = "";

                HabilAtividadeExtraDAL ativDAL = new HabilAtividadeExtraDAL();
                HabilAtividadeExtra ativ = new HabilAtividadeExtra();
                int CodigoAtividade = 0;

                ativ.DataHoraLancamento = DateTime.Now;
                ativ.DescricaoAtividade = "Importação de preços";
                ativ.NomeTabela = "TEMP_REL_CONTRATOS";
                ativ.DescricaoFiltro = "";
                ativ.CodigoSituacao = 173;
                ativ.CodigoUsuario = Convert.ToInt32(Session["CodUsuario"]);

                strGUID = ativDAL.Inserir(ativ, ref CodigoAtividade);

                ativDAL.InserirTEMP_REL_CONTRATOS(dt, strGUID);

                ativDAL.AlterarSituacao(CodigoAtividade, 172);
                ShowMessage("Importação gerada com sucesso", MessageType.Info);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Info);
            }

            Session["ListaProdutos"] = null;
        }
    }
}
