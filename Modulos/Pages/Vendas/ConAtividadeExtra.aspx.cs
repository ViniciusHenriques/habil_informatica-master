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
using System.Web.UI.WebControls;

namespace SoftHabilInformatica.Pages.Vendas
{
    public partial class ConAtividadeExtra : System.Web.UI.Page
    {
        public enum MessageType { Success, Error, Info, Warning };

        String strMensagemR = "";

        clsValidacao v = new clsValidacao();

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
                Session["Pagina"] = Request.CurrentExecutionFilePath;

            if(!IsPostBack)
                AtualizarGridView();
        }

        protected void AtualizarGridView()
        {
            HabilAtividadeExtraDAL ativDAL = new HabilAtividadeExtraDAL();
            grdAtividades.DataSource = ativDAL.ListarAtividades("CD_USUARIO", "INT", Session["CodUsuario"].ToString(), "CD_INDEX DESC");
            grdAtividades.DataBind();

            if (grdAtividades.Rows.Count > 0)
                lblNenhumAtividade.Visible = false;
            else
                lblNenhumAtividade.Visible = true;
        }

        protected void btnGerarPlanilha_Click(string strGUID, bool Impostos, int CodigoAtividade)
        {
            try
            {
                AnexoDocumentoDAL ad = new AnexoDocumentoDAL();

                string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\";

                string NomeXlsxGerado = ad.GerarGUID("xlsx");

                string strErro = "";

                HabilAtividadeExtraDAL ativDAL = new HabilAtividadeExtraDAL();
                HabilAtividadeExtra ativ = new HabilAtividadeExtra();
                ativ = ativDAL.PesquisarAtividade(CodigoAtividade);

                bool GerouExcel = false;

                if (ativ.NomeTabela == "TEMP_REL_IMP_CONSUMO")
                    GerouExcel = GerarArquivoExcel_REL_IMP_CONSUMO(CaminhoArquivoLog, ref NomeXlsxGerado, CodigoAtividade, Impostos, ref strErro);
                else if (ativ.NomeTabela == "TEMP_REL_IMP_ENTRADAS")
                    GerouExcel = GerarArquivoExcel_REL_IMP_ENTRADAS(CaminhoArquivoLog, NomeXlsxGerado, CodigoAtividade, ref strErro);

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
                    if (strErro != "")
                        ShowMessage(strErro, MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Info);
            }
        }

        public static string GetColuna(int columnNumber)
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

        protected bool GerarArquivoExcel_REL_IMP_CONSUMO(string CaminhoArquivo,ref string NomeArquivo, int CodigoAtividade,bool Impostos, ref string strErro)
        {
            Habil_LogDAL log = new Habil_LogDAL();

            HabilAtividadeExtraDAL ativDAL = new HabilAtividadeExtraDAL();
            HabilAtividadeExtra ativ = new HabilAtividadeExtra();
            
            try
            {
                ativ = ativDAL.PesquisarAtividade(CodigoAtividade);

                if (ativ.CodigoSituacao == 173)
                {
                    AtualizarGridView();
                    ShowMessage("Relatório sendo processado", MessageType.Info);
                    return false;
                }
                
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Planilha 1");

                IXLSheetView view = ws.SheetView;
                view.ZoomScale = 80;
                view.FreezeRows(4);

                int ColunaCabecalho = 4;
                int ColunaDados = 2;
                
                ws.Cell("B1").Value = "Empresa: ";
                ws.Range("B1:E1").Merge();

                ws.Cell("F1").Value = ativ.DescricaoAtividade;
                ws.Range("F1:J1").Merge();

                ws.Cell("B2").Value = "Cliente: ";
                ws.Range("B2:E2").Merge();

                ws.Cell("F2").Value = "Período: ";
                ws.Range("F2:I2").Merge();

                ws.Cell("A4").Value = "Código Produto";

                ws.Cell("B4").Value = "Descrição Produto";

                ws.Cell("C4").Value = "Cardex";

                ws.Cell("D4").Value = "NCM";
              
                if (Impostos)
                {
                    ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "NCM";
                    ColunaCabecalho++;

                    ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "C.S.T";
                    ColunaCabecalho++;

                    ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "ICMS";
                    ColunaCabecalho++;

                    ColunaDados += 3;
                }

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "CT";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "Unid.";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "Qtde";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "Vlr. Venda";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "Total Venda";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "% Part";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "Valor Época";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "% Desc";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "Valor lista Atual";
                ColunaCabecalho++;

                ws.Cell(GetColuna(ColunaCabecalho) + "4").Value = "% Desc. Atual";

                ws.Column(GetColuna(ColunaDados + 4)).Style.NumberFormat.Format = "#0";
                ws.Columns(GetColuna(ColunaDados + 5) + "-" + GetColuna(ColunaDados + 11)).Style.NumberFormat.Format = "#,##0.00";

                string Cliente = "";
                string Empresa = "";

                decimal TotalDcU = 0, TotalPrea = 0, TotalPdsc = 0, TotalVlta = 0, TotalPrec = 0;
                decimal qtde = 0, prec = 0, prea = 0;

                DateTime DataInicial = DateTime.MinValue, DataFinal = DateTime.MinValue;
                DataTable dt = new DataTable();
                dt = ativDAL.ListarTEMP_REL_IMP_CONSUMO(ativ.Chave,ativ.NomeTabela);
               
                int LinhaExcel = 4;
                
                if(dt.Rows.Count == 0)
                {
                    ShowMessage("Relatório sem registro(s)", MessageType.Info);
                    ativDAL.AlterarDataAtualizacao(CodigoAtividade);
                    ativDAL.AlterarSituacao(CodigoAtividade, 172);
                    AtualizarGridView();
                    return false;
                }

                foreach (DataRow row in dt.Rows)
                {
                    LinhaExcel++;
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName == "cpro")
                            ws.Cell("A" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "descr" && row[column] != DBNull.Value)
                            ws.Cell("B" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "cardex" && row[column] != DBNull.Value)
                            ws.Cell("C" + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "ncm" && row[column] != DBNull.Value && Impostos)
                            ws.Cell(GetColuna(ColunaDados - 1) + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "cst" && row[column] != DBNull.Value && Impostos)
                        {
                            ws.Cell(GetColuna(ColunaDados) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            ws.Cell(GetColuna(ColunaDados) + LinhaExcel).Style.NumberFormat.Format = "###000";
                        }
                        else if (column.ColumnName == "icms" && row[column] != DBNull.Value && Impostos)
                            ws.Cell(GetColuna(ColunaDados + 1) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "contratox" && row[column] != DBNull.Value)
                            ws.Cell(GetColuna(ColunaDados + 2) + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "unid" && row[column] != DBNull.Value)
                            ws.Cell(GetColuna(ColunaDados + 3) + LinhaExcel).Value = row[column].ToString();
                        else if (column.ColumnName == "qtde" && row[column] != DBNull.Value)
                        {
                            ws.Cell(GetColuna(ColunaDados + 4) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            qtde = Convert.ToDecimal(row[column].ToString());
                        }
                        else if (column.ColumnName == "prec" && row[column] != DBNull.Value)
                        {
                            ws.Cell(GetColuna(ColunaDados + 5) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            prec = Convert.ToDecimal(row[column].ToString());
                        }
                        else if (column.ColumnName == "vtot" && row[column] != DBNull.Value)
                            ws.Cell(GetColuna(ColunaDados + 6) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "part" && row[column] != DBNull.Value)
                            ws.Cell(GetColuna(ColunaDados + 7) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "vlta" && row[column] != DBNull.Value)
                        {
                            if ((Convert.ToDecimal(row[column].ToString()) * qtde) == 0)
                                ws.Cell(GetColuna(ColunaDados + 8) + LinhaExcel).Value = "0";
                            else
                                ws.Cell(GetColuna(ColunaDados + 8) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString()) / qtde;
                        }
                        else if (column.ColumnName == "pdsc" && row[column] != DBNull.Value)
                            ws.Cell(GetColuna(ColunaDados + 9) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "prea" && row[column] != DBNull.Value)
                        {
                            ws.Cell(GetColuna(ColunaDados + 10) + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            prea = Convert.ToDecimal(row[column].ToString());
                        }
                        else if (column.ColumnName == "dcu")
                        {
                            if (prea * qtde == 0)
                                ws.Cell(GetColuna(ColunaDados + 11) + LinhaExcel).Value = "0";
                            else
                                ws.Cell(GetColuna(ColunaDados + 11) + LinhaExcel).Value = 100 - ((prec * qtde) * 100 / (prea * qtde));
                        }
                        else if (column.ColumnName == "tot_dcu" && row[column] != DBNull.Value)
                            TotalDcU = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "tot_prea" && row[column] != DBNull.Value)
                            TotalPrea = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "tot_pdsc" && row[column] != DBNull.Value)
                            TotalPdsc = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "tot_vlta" && row[column] != DBNull.Value)
                            TotalVlta = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "tot_prec" && row[column] != DBNull.Value)
                            TotalPrec = Convert.ToDecimal(row[column].ToString());
                        else if (column.ColumnName == "dt_inicial" && row[column] != DBNull.Value)
                            DataInicial = Convert.ToDateTime(row[column].ToString());
                        else if (column.ColumnName == "dt_final" && row[column] != DBNull.Value)
                            DataFinal = Convert.ToDateTime(row[column].ToString());
                        else if (column.ColumnName == "nome_emp" && row[column] != DBNull.Value)
                            Empresa = row[column].ToString();
                        else if (column.ColumnName == "nomecli" && row[column] != DBNull.Value)
                            Cliente = row[column].ToString();
                    }                                         
                }
                
                ws.Cell(GetColuna(ColunaDados + 4) + (LinhaExcel + 2)).FormulaA1 = "SUM("+ GetColuna(ColunaDados + 4) + "5:"+ GetColuna(ColunaDados + 4) + LinhaExcel + ")"; ;
                ws.Cell(GetColuna(ColunaDados + 5) + (LinhaExcel + 2)).Value = TotalPrec;
                ws.Cell(GetColuna(ColunaDados + 6) + (LinhaExcel + 2)).FormulaA1 = "SUM(" + GetColuna(ColunaDados + 6) + "5:" + GetColuna(ColunaDados + 6) + + LinhaExcel + ")";
                ws.Cell(GetColuna(ColunaDados + 8) + (LinhaExcel + 2)).Value = TotalVlta;
                ws.Cell(GetColuna(ColunaDados + 9) + (LinhaExcel + 2)).Value = TotalPdsc;
                ws.Cell(GetColuna(ColunaDados + 10) + (LinhaExcel + 2)).Value = TotalPrea;
                ws.Cell(GetColuna(ColunaDados + 11) + (LinhaExcel + 2)).Value = TotalDcU;
                ws.Range("G" + (LinhaExcel + 2) + ":" + GetColuna(ColunaDados + 11) + (LinhaExcel + 2)).Style.Font.Bold = true;
                
                ws.Range("G" + (LinhaExcel + 2) + ":" + GetColuna(ColunaDados + 11) + (LinhaExcel + 2)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("G" + (LinhaExcel + 2) + ":" + GetColuna(ColunaDados + 11) + (LinhaExcel + 2)).Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("G" + (LinhaExcel + 2) + ":" + GetColuna(ColunaDados + 11) + (LinhaExcel + 2)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("G" + (LinhaExcel + 2) + ":" + GetColuna(ColunaDados + 11) + (LinhaExcel + 2)).Style.Border.InsideBorderColor = XLColor.Black;

                ws.Range("A4:" + GetColuna(ColunaCabecalho) + LinhaExcel).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A4:" + GetColuna(ColunaCabecalho) + LinhaExcel).Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("A4:" + GetColuna(ColunaCabecalho) + LinhaExcel).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A4:" + GetColuna(ColunaCabecalho) + LinhaExcel).Style.Border.InsideBorderColor = XLColor.Black;

                ws.Range("A4:" + GetColuna(ColunaCabecalho) + "4").Style.Fill.BackgroundColor = XLColor.GreenRyb;
                ws.Range("A4:" + GetColuna(ColunaCabecalho) + "4").Style.Font.Bold = true;
                ws.Range("A4:" + GetColuna(ColunaCabecalho) + "4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Range("A4:" + GetColuna(ColunaCabecalho) + (LinhaExcel + 2)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("A4:" + GetColuna(ColunaCabecalho) + (LinhaExcel + 2)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                
                ws.Cell("B1").Value = "Empresa: " + Empresa;
                ws.Cell("B2").Value = "Grupo/Cliente: " + Cliente;
                ws.Cell("F2").Value = "Período: " + DataInicial.ToString("dd/MM/yyyy") + " a " + DataFinal.ToString("dd/MM/yyyy");

                ws.Range("A1:F2").Style.Font.Bold = true;
                ws.Column("D").Width = 10;
                ws.Columns("A").Width = 7;
                ws.Columns(GetColuna(ColunaDados) + "-"+ GetColuna(ColunaDados + 4)).Width = 6;
                ws.Columns(GetColuna(ColunaDados + 5) + "-" + GetColuna(ColunaDados + 6)).Width = 10;
                ws.Columns(GetColuna(ColunaDados + 7) + "-" + GetColuna(ColunaDados + 11)).Width = 8;
                ws.Column( GetColuna(ColunaDados + 11)).Width = 9;
                ws.Column("C").Width = 7;
                ws.Column("B").Width = 20;

                ws.Columns("B-C").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Row(4).Style.Alignment.WrapText = true;
                ws.Row(4).Height = 30;
                
                
                NomeArquivo = "HABIL_DCTO_" + Cliente.ToUpper().Replace(" ","").Replace(".","-").Replace("-","_").Replace(":","_").Replace(",", "-").Replace(";", "-").Replace("/", "-").Replace(",", "-").Replace("|", "-")+ ".xlsx";
                wb.SaveAs(CaminhoArquivo + @"\" + NomeArquivo);
                wb.Dispose();

                log.GerandoArquivoLog("Relatório gerado pelo usuário " + Session["UsuSis"] + " / " + Session["CodUsuario"]);
                ativDAL.AlterarDataAtualizacao(CodigoAtividade);
                AtualizarGridView();
                return true;
            }
            catch (Exception ex)
            {
                log.GerandoArquivoLog("Erro ao gerar relatório - " + ex.Message + " - tentativa do usuário " + Session["UsuSis"] + " / " + Session["CodUsuario"]);

                ativDAL.AlterarDataAtualizacao(CodigoAtividade);
                ativDAL.AlterarSituacao(CodigoAtividade, 172);
                ShowMessage(ex.Message, MessageType.Error);
                strErro = ex.Message;
                return false;
            }
        }

        protected bool GerarArquivoExcel_REL_IMP_ENTRADAS(string CaminhoArquivo, string NomeArquivo, int CodigoAtividade, ref string strErro)
        {
            Habil_LogDAL log = new Habil_LogDAL();

            HabilAtividadeExtraDAL ativDAL = new HabilAtividadeExtraDAL();
            HabilAtividadeExtra ativ = new HabilAtividadeExtra();

            try
            {
                ativ = ativDAL.PesquisarAtividade(CodigoAtividade);

                if (ativ.CodigoSituacao == 173)
                {
                    AtualizarGridView();
                    ShowMessage("Relatório sendo processado", MessageType.Info);
                    return false;
                }

                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Planilha 1");

                IXLSheetView view = ws.SheetView;
                view.ZoomScale = 80;

                ws.Column("D").Style.NumberFormat.Format = "#,##0.00";

                ws.Cell("A1").Value = "";
                ws.Range("A1:E1").Merge();

                ws.Cell("F1").Value = ativ.DescricaoAtividade;
                ws.Range("F1:J1").Merge();

                ws.Cell("A2").Value = "....";
                ws.Range("A2:E2").Merge();

                ws.Cell("A4").Value = "Entrada";

                ws.Cell("B4").Value = "Empresa";

                ws.Cell("C4").Value = "Nota";

                ws.Cell("D4").Value = "Emissão";

                ws.Cell("E4").Value = "Valor Nota";

                ws.Cell("F4").Value = "Fornecedor";
                ws.Range("F4:H4").Merge();

                ws.Cell("I4").Value = "Chave Acesso";
                ws.Range("I4:L4").Merge();
                DateTime DataInicial = DateTime.MinValue, DataFinal = DateTime.MinValue;

                List<string> ListaNota = new List<string>();
                ListaNota = ativDAL.ListarNotasEntrada(ativ.Chave);
                string NomeEmpresa = "";

                int LinhaExcel = 4;

                if (ListaNota.Count == 0)
                {
                    ShowMessage("Relatório sem registro(s)", MessageType.Info);
                    ativDAL.AlterarDataAtualizacao(CodigoAtividade);
                    ativDAL.AlterarSituacao(CodigoAtividade, 172);
                    AtualizarGridView();
                    return false;
                }
                for (int i = 0; i < ListaNota.Count; i++)
                {
                    LinhaExcel++;
                    LinhaExcel++;
                    LinhaExcel++;
                    
                    ws.Cell("A" + LinhaExcel).Value = "Código";
                    ws.Cell("B" + LinhaExcel).Value = "Descrição do Produto";
                    ws.Cell("C" + LinhaExcel).Value = "Un";
                    ws.Cell("D" + LinhaExcel).Value = "Qtde";
                    ws.Cell("E" + LinhaExcel).Value = "Vl. Unitário";
                    ws.Cell("F" + LinhaExcel).Value = "Vl. Total Item";
                    ws.Cell("G" + LinhaExcel).Value = "Vl. IPI";
                    ws.Cell("H" + LinhaExcel).Value = "CFOP";
                    ws.Cell("I" + LinhaExcel).Value = "CST da Nota";
                    ws.Cell("J" + LinhaExcel).Value = "Situação do GF";
                    ws.Cell("K" + LinhaExcel).Value = "% ICMS";
                    ws.Cell("L" + LinhaExcel).Value = "Vl. Icms";
                    ws.Cell("M" + LinhaExcel).Value = "% ST";
                    ws.Cell("N" + LinhaExcel).Value = "Clf.Fiscal";
                    ws.Cell("O" + LinhaExcel).Value = "Base Substituição Tributária";
                    ws.Cell("P" + LinhaExcel).Value = "Valor Substituição Tributária";
                    ws.Cell("Q" + LinhaExcel).Value = "Redução de Substituição Tributária";
                    ws.Cell("R" + LinhaExcel).Value = "Redução de Icms para Substituição Tributária";
                    ws.Cell("S" + LinhaExcel).Value = "Base Substituição Tributária Retida";
                    ws.Cell("T" + LinhaExcel).Value = "Valor Substituição Tributária Retida";

                    ws.Row(LinhaExcel).Style.Alignment.WrapText = true;

                    ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Border.InsideBorderColor = XLColor.Black;

                    if ((i % 2) == 0)
                        ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Fill.BackgroundColor = XLColor.GreenRyb;
                    else
                        ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                    DataTable dt = new DataTable();
                    dt = ativDAL.ListarTEMP_REL_IMP_ENTRADAS(ativ.Chave, ListaNota[i]);

                    DataTable dtCFOPNota = new DataTable();
                    dtCFOPNota = ativDAL.ListarCFOPNota(ativ.Chave, ListaNota[i]);

                    string DataEntrada = "", Empresa = "", Nota = "", ValorNota = "", Fornecedor = "", ChaveAcesso = "", DataEmissao = "";

                    foreach (DataRow row in dt.Rows)
                    {
                        LinhaExcel++;

                        ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        ws.Range("A" + LinhaExcel + ":T" + LinhaExcel).Style.Border.InsideBorderColor = XLColor.Black;

                        decimal ValorTotalItem = 0;
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName == "cpro" && row[column] != DBNull.Value)
                                ws.Cell("A" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "deno" && row[column] != DBNull.Value)
                                ws.Cell("B" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "unid" && row[column] != DBNull.Value)
                                ws.Cell("C" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "qtde" && row[column] != DBNull.Value)
                                ws.Cell("D" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "vlun" && row[column] != DBNull.Value)
                                ws.Cell("E" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "vtot" && row[column] != DBNull.Value)
                            {
                                ws.Cell("F" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                                ValorTotalItem = Convert.ToDecimal(row[column].ToString());
                            }
                            else if (column.ColumnName == "ipi" && row[column] != DBNull.Value)
                                ws.Cell("G" + LinhaExcel).Value = Convert.ToDecimal(ValorTotalItem * (Convert.ToDecimal(row[column].ToString()) / 100));
                            else if (column.ColumnName == "cfop" && row[column] != DBNull.Value)
                                ws.Cell("H" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "sitnf" && row[column] != DBNull.Value)
                                ws.Cell("I" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "sitgr" && row[column] != DBNull.Value)
                                ws.Cell("J" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "icms" && row[column] != DBNull.Value)
                                ws.Cell("K" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "vicms" && row[column] != DBNull.Value)
                                ws.Cell("L" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "psub" && row[column] != DBNull.Value)
                                ws.Cell("M" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "claf" && row[column] != DBNull.Value)
                                ws.Cell("N" + LinhaExcel).Value = row[column].ToString();
                            else if (column.ColumnName == "bicmsst" && row[column] != DBNull.Value)
                                ws.Cell("O" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "vicmsst" && row[column] != DBNull.Value)
                                ws.Cell("P" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "redust" && row[column] != DBNull.Value)
                                ws.Cell("Q" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "reduicmsst" && row[column] != DBNull.Value)
                                ws.Cell("R" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "bicmsstret" && row[column] != DBNull.Value)
                                ws.Cell("S" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "vicmsstret" && row[column] != DBNull.Value)
                                ws.Cell("T" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "dten" && row[column] != DBNull.Value)
                                DataEntrada = row[column].ToString();
                            else if (column.ColumnName == "cemp" && row[column] != DBNull.Value)
                                Empresa = row[column].ToString();
                            else if (column.ColumnName == "nota" && row[column] != DBNull.Value)
                                Nota = row[column].ToString();
                            else if (column.ColumnName == "nome" && row[column] != DBNull.Value)
                                Fornecedor = row[column].ToString();
                            else if (column.ColumnName == "chavenfe" && row[column] != DBNull.Value)
                                ChaveAcesso = row[column].ToString();
                            else if (column.ColumnName == "dtnf" && row[column] != DBNull.Value)
                                DataEmissao = row[column].ToString();
                            else if (column.ColumnName == "vtnf" && row[column] != DBNull.Value)
                                ValorNota = row[column].ToString();
                            else if (column.ColumnName == "dt_inicial" && row[column] != DBNull.Value)
                                DataInicial = Convert.ToDateTime(row[column].ToString());
                            else if (column.ColumnName == "dt_final" && row[column] != DBNull.Value)
                                DataFinal = Convert.ToDateTime(row[column].ToString());
                            else if (column.ColumnName == "nome_emp" && row[column] != DBNull.Value)
                                NomeEmpresa = row[column].ToString();
                        }
                    }
                    ws.Cell("A" + (LinhaExcel - dt.Rows.Count - 1)).Value = DataEntrada;
                    ws.Cell("B" + (LinhaExcel - dt.Rows.Count - 1)).Value = "'" + Empresa;
                    ws.Cell("C" + (LinhaExcel - dt.Rows.Count - 1)).Value = Nota;
                    ws.Cell("D" + (LinhaExcel - dt.Rows.Count - 1)).Value = DataEmissao;
                    ws.Cell("D" + (LinhaExcel - dt.Rows.Count - 1)).Style.DateFormat.Format = "dd/MM/yyyy";
                    ws.Cell("E" + (LinhaExcel - dt.Rows.Count - 1)).Value = Convert.ToDecimal(ValorNota);
                    ws.Cell("E" + (LinhaExcel - dt.Rows.Count - 1)).Style.NumberFormat.Format = "#,##0.00";

                    ws.Cell("F" + (LinhaExcel - dt.Rows.Count - 1)).Value = Fornecedor;
                    ws.Range("F" + (LinhaExcel - dt.Rows.Count - 1) + ":H" + (LinhaExcel - dt.Rows.Count - 1)).Merge();

                    ws.Cell("I" + (LinhaExcel - dt.Rows.Count - 1)).Value = "'" + ChaveAcesso;
                    ws.Range("I" + (LinhaExcel - dt.Rows.Count - 1) + ":L" + (LinhaExcel - dt.Rows.Count - 1)).Merge();

                    ws.Range("A" + (LinhaExcel - dt.Rows.Count - 1) + ":L" + (LinhaExcel - dt.Rows.Count - 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A" + (LinhaExcel - dt.Rows.Count - 1) + ":L" + (LinhaExcel - dt.Rows.Count - 1)).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Range("A" + (LinhaExcel - dt.Rows.Count - 1) + ":L" + (LinhaExcel - dt.Rows.Count - 1)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A" + (LinhaExcel - dt.Rows.Count - 1) + ":L" + (LinhaExcel - dt.Rows.Count - 1)).Style.Border.InsideBorderColor = XLColor.Black;

                    LinhaExcel++;
                    ws.Cell("F" + LinhaExcel).FormulaA1 = "SUM(F" + (LinhaExcel - dt.Rows.Count) + ":F" + (LinhaExcel - 1) + ")";
                    ws.Cell("G" + LinhaExcel).FormulaA1 = "SUM(G" + (LinhaExcel - dt.Rows.Count) + ":G" + (LinhaExcel - 1) + ")";
                    ws.Cell("L" + LinhaExcel).FormulaA1 = "SUM(L" + (LinhaExcel - dt.Rows.Count) + ":L" + (LinhaExcel - 1) + ")";
                    ws.Cell("O" + LinhaExcel).FormulaA1 = "SUM(O" + (LinhaExcel - dt.Rows.Count) + ":O" + (LinhaExcel - 1) + ")";
                    ws.Cell("P" + LinhaExcel).FormulaA1 = "SUM(P" + (LinhaExcel - dt.Rows.Count) + ":P" + (LinhaExcel - 1) + ")";
                    ws.Cell("S" + LinhaExcel).FormulaA1 = "SUM(S" + (LinhaExcel - dt.Rows.Count) + ":S" + (LinhaExcel - 1) + ")";
                    ws.Cell("T" + LinhaExcel).FormulaA1 = "SUM(T" + (LinhaExcel - dt.Rows.Count) + ":T" + (LinhaExcel - 1) + ")";

                    ws.Range("F" + LinhaExcel + ":T" + LinhaExcel).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("F" + LinhaExcel + ":T" + LinhaExcel).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Range("F" + LinhaExcel + ":T" + LinhaExcel).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("F" + LinhaExcel + ":T" + LinhaExcel).Style.Border.InsideBorderColor = XLColor.Black;

                    ws.Range("F" + LinhaExcel + ":T" + LinhaExcel).Style.Font.Bold = true;

                    LinhaExcel++;
                    ws.Cell("A" + LinhaExcel).Value = "CFOP";
                    ws.Cell("B" + LinhaExcel).Value = "BASE";
                    ws.Cell("C" + LinhaExcel).Value = "ICMS";
                    ws.Cell("D" + LinhaExcel).Value = "IPI";

                    if ((i % 2) == 0)
                        ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Fill.BackgroundColor = XLColor.GreenRyb;
                    else
                        ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                    ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Border.InsideBorderColor = XLColor.Black;


                    foreach (DataRow row in dtCFOPNota.Rows)
                    {
                        LinhaExcel++;
                        string CFOP = "";

                        foreach (DataColumn column in dtCFOPNota.Columns)
                        {
                            if (column.ColumnName == "cfop" && row[column] != DBNull.Value)
                            {
                                ws.Cell("A" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                                CFOP = row[column].ToString();
                            }
                            else if (column.ColumnName == "BASE" && row[column] != DBNull.Value)
                                ws.Cell("B" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                            else if (column.ColumnName == "TOTAL_ICMS" && row[column] != DBNull.Value)
                            {
                                if (CFOP == "1102" || CFOP == "2102" || CFOP == "2152")
                                    ws.Cell("C" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                                else
                                    ws.Cell("C" + LinhaExcel).Value = "0";

                                ws.Cell("C" + LinhaExcel).Style.NumberFormat.Format = "#,##0.00";
                            }
                            else if (column.ColumnName == "TOTAL_IPI" && row[column] != DBNull.Value)
                                ws.Cell("D" + LinhaExcel).Value = Convert.ToDecimal(row[column].ToString());
                        }

                        ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Border.OutsideBorderColor = XLColor.Black;
                     
                        ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        ws.Range("A" + LinhaExcel + ":D" + LinhaExcel).Style.Border.InsideBorderColor = XLColor.Black;

                    }
                    LinhaExcel++;
                }

                ws.Range("A4:L4").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A4:L4").Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("A4:L4").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A4:L4").Style.Border.InsideBorderColor = XLColor.Black;

                ws.Range("A4:L4").Style.Fill.BackgroundColor = XLColor.GreenRyb;
                ws.Range("A4:T4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Range("A4:T" + (LinhaExcel + 2)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("A4:T" + (LinhaExcel + 2)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell("A2").Value = "Período: " + DataInicial.ToString("dd/MM/yyyy") + " a " + DataFinal.ToString("dd/MM/yyyy");
                ws.Cell("A1").Value = NomeEmpresa;
                
                ws.Range("A4:T4").Style.Font.Bold = true;

                ws.Columns("A-N").Width = 13;
                ws.Columns("O-T").Width = 23;
                ws.Column("B").Width = 15;
                ws.Column("B").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Row(4).Height = 20;

                ws.Column("B").Style.NumberFormat.Format = "#,##0.00";
                ws.Column("G").Style.NumberFormat.Format = "#,##0.00";
                ws.Column("F").Style.NumberFormat.Format = "#,##0.00";
                ws.Column("L").Style.NumberFormat.Format = "#,##0.00";
                ws.Columns("P-T").Style.NumberFormat.Format = "#,##0.00";

                wb.SaveAs(CaminhoArquivo + @"\" + NomeArquivo);
                wb.Dispose();

                log.GerandoArquivoLog("Relatório gerado pelo usuário " + Session["UsuSis"] + " / " + Session["CodUsuario"]);
                ativDAL.AlterarDataAtualizacao(CodigoAtividade);
                AtualizarGridView();
                return true;
            }
            catch (Exception ex)
            {
                log.GerandoArquivoLog("Erro ao gerar relatório - " + ex.Message + " - tentativa do usuário " + Session["UsuSis"] + " / " + Session["CodUsuario"]);

                ativDAL.AlterarDataAtualizacao(CodigoAtividade);
                ativDAL.AlterarSituacao(CodigoAtividade, 172);
                ShowMessage(ex.Message, MessageType.Error);
                strErro = ex.Message;
                return false;
            }
        }

        protected void btnFechar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Welcome.aspx");
        }

        protected void grdAtividades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string x = e.CommandName;
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = grdAtividades.Rows[index];
                CheckBox chk = (CheckBox)grdAtividades.Rows[index].FindControl("chkImpostos");

                int Codigo = Convert.ToInt32(Server.HtmlDecode(row.Cells[0].Text));
                string strGUID = Server.HtmlDecode(row.Cells[2].Text);
                
                HabilAtividadeExtraDAL ativDAL = new HabilAtividadeExtraDAL();
                HabilAtividadeExtra ativ = new HabilAtividadeExtra();
                ativ = ativDAL.PesquisarAtividade(Codigo);

                if (x == "Gerar")
                {
                    if (ativ.CodigoSituacao == 173)
                    {
                        AtualizarGridView();
                        ShowMessage("Relatório sendo processado", MessageType.Info);
                        return ;
                    }

                    btnGerarPlanilha_Click(strGUID, chk.Checked, Codigo);

                    grdAtividades.DataSource = ativDAL.ListarAtividades("CD_USUARIO", "INT", Session["CodUsuario"].ToString(), "CD_INDEX DESC");
                    grdAtividades.DataBind();
                }
                else if (x == "Cancelar")
                {
                    ativDAL.Excluir(Codigo, strGUID, ativ.NomeTabela);

                    ShowMessage("Atividade cancelada com sucesso", MessageType.Success);
                    grdAtividades.DataSource = ativDAL.ListarAtividades("CD_USUARIO", "INT", Session["CodUsuario"].ToString(), "CD_INDEX DESC");
                    grdAtividades.DataBind();
                }
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
    }
}
