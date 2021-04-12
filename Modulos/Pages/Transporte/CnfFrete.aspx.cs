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
using System.Windows.Forms;

namespace SoftHabilInformatica.Pages.Transporte
{
    public partial class CnfFrete : System.Web.UI.Page
    {
        public enum MessageType { Success, Error, Info, Warning };

        DataTable ListaArquivos = new DataTable();

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ListaArquivos.Columns.Add("#", typeof(int));
            ListaArquivos.Columns.Add("Arquivo", typeof(string));
            ListaArquivos.Columns.Add("XML", typeof(byte[]));

            if (Session["ListaArquivos"] != null)
            {
                ListaArquivos = (DataTable)Session["ListaArquivos"];
                grdXMLs.DataSource = ListaArquivos;
                grdXMLs.DataBind();
            }
            if (ListaArquivos.Rows.Count == 0)
                BtnLerXML.Visible = false;
            else
                BtnLerXML.Visible = true;
        }

        protected void BtnLerXML_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListaArquivos.Rows.Count == 0)
                {
                    ShowMessage("Necessário existir arquivo(s), adicione!!", MessageType.Info);
                    return;
                }
                
                AnexoDocumentoDAL ad = new AnexoDocumentoDAL();
                List<DataSet> ListDataSet = new List<DataSet>();
                string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\CTe\\";
                foreach (DataRow item in ListaArquivos.Rows)
                {
                    string NomeFile = item[1].ToString();
                    string Extencao = Path.GetExtension(NomeFile);

                    if (Extencao.ToLower() != ".xml")
                    {
                        ShowMessage("Insira um arquivo XML", MessageType.Warning);
                        return;
                    }

                    byte[] XMLBinary = (byte[])item[2];

                    string GUIDXML = ad.GerarGUID("xml");
                    
                    if (System.IO.File.Exists(CaminhoArquivoLog + GUIDXML))
                        System.IO.File.Delete(CaminhoArquivoLog + GUIDXML);

                    FileStream file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Create);

                    BinaryWriter bw = new BinaryWriter(file);
                    bw.Write(XMLBinary);
                    bw.Close();

                    file = new FileStream(CaminhoArquivoLog + GUIDXML, FileMode.Open);
                    BinaryReader br = new BinaryReader(file);
                    file.Close();

                    XmlTextReader xml1 = new XmlTextReader(CaminhoArquivoLog + GUIDXML);

                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(xml1);
                    xml1.Close();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("XML_BINARY", typeof(byte[]));
                    DataRow dtRow = dt.NewRow();

                    dtRow["XML_BINARY"] = XMLBinary;
                    dt.Rows.Add(dtRow);
                    dataSet.Tables.Add(dt);
                    ListDataSet.Add(dataSet);

                    if (System.IO.File.Exists(CaminhoArquivoLog + GUIDXML))
                        System.IO.File.Delete(CaminhoArquivoLog + GUIDXML);
                }
                string NomeXlsxGerado = ad.GerarGUID("xlsx");

                string strErro = "";
                bool GerouExcel = GerarArquivoExcel(ListDataSet, CaminhoArquivoLog, NomeXlsxGerado, ref strErro);

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
                    ShowMessage(strErro, MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Info);
            }
        }
        
        protected bool GerarArquivoExcel(List<DataSet> ListDataSet, string CaminhoArquivo, string NomeArquivo, ref string strErro)
        {
            Habil_LogDAL log = new Habil_LogDAL();
            int ContadorLinhaErro = 0;
            try
            {
                var wb = new XLWorkbook();

                var ws = wb.Worksheets.Add("Planilha 1");
                IXLSheetView view = ws.SheetView;
                view.ZoomScale = 80;
                view.FreezeRows(4);

                //CABEÇALHO DO RELATORIO
                ws.Cell("B3").Value = "Chave Acesso CT-e";
                ws.Range("B3:B4").Merge();
                ws.Columns("B").Width = 23;

                ws.Cell("C3").Value = "Emitente";
                ws.Range("C3:D3").Merge();
                ws.Columns("C").Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.FractionPrecision1);
                ws.Cell("C4").Value = "CNPJ";
                ws.Columns("C").Width = 18;
                ws.Cell("D4").Value = "Nome";
                ws.Columns("D").Width = 20;

                ws.Cell("E3").Value = "Remetente";
                ws.Range("E3:F3").Merge();
                ws.Columns("E").Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.FractionPrecision1);
                ws.Cell("E4").Value = "CNPJ";
                ws.Columns("E").Width = 18;
                ws.Cell("F4").Value = "Nome";
                ws.Columns("F").Width = 20;

                ws.Cell("G3").Value = "Destinatário";
                ws.Range("G3:H3").Merge();
                ws.Columns("G").Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.FractionPrecision1);
                ws.Cell("G4").Value = "CNPJ";
                ws.Columns("G").Width = 18;
                ws.Cell("H4").Value = "Nome";
                ws.Columns("H").Width = 20;

                ws.Cell("I3").Value = "Município Destino";
                ws.Range("I3:I4").Merge();
                ws.Columns("I").Width = 15;

                ws.Cell("J3").Value = "N° Fatura";
                ws.Range("J3:J4").Merge();
                ws.Columns("J").Width = 15;

                ws.Cell("K3").Value = "Valor CT-e";
                ws.Range("K3:K4").Merge();
                ws.Columns("K").Style.NumberFormat.Format = "R$ #,##0.00";

                ws.Cell("L3").Value = "Peso CT-e";
                ws.Range("L3:L4").Merge();

                ws.Cell("M3").Value = "Cubagem    CT-e";
                ws.Range("M3:M4").Merge();

                ws.Columns("N").Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.Text);
                ws.Cell("N3").Value = "Chave Acesso NF-e";
                ws.Range("N3:N4").Merge();
                ws.Columns("N").Width = 23;

                ws.Cell("O3").Value = "Valor NF-e";
                ws.Range("O3:O4").Merge();
                ws.Columns("O").Style.NumberFormat.Format = "R$ #,##0.00";
                ws.Columns("O").Width = 15;

                ws.Cell("P3").Value = "Peso NF-e";
                ws.Range("P3:P4").Merge();

                ws.Cell("Q3").Value = "Cubagem    NF-e";
                ws.Range("Q3:Q4").Merge();

                ws.Cell("R3").Value = "Modalidade NF-e";
                ws.Range("R3:R4").Merge();

                ws.Cell("S3").Value = "BASE CÁLCULO SEM PEDÁGIO";
                ws.Range("S3:S4").Merge();
                ws.Column("S").Style.NumberFormat.Format = "#,##0.00";
                ws.Column("S").Width = 20;

                ws.Cell("T3").Value = "IMPOSTO";
                ws.Range("T3:T4").Merge();
                ws.Columns("T").Style.NumberFormat.Format = "#,##0.00";

                ws.Cell("U3").Value = "Data NF-e";
                ws.Range("U3:U4").Merge();

                ws.Cell("V3").Value = "Frete NF-e";
                ws.Range("V3:V4").Merge();
                ws.Columns("V").Style.NumberFormat.Format = "R$ #,##0.00";
              
                ws.Cell("W3").Value = "N° NFe(s) do cliente na mesma data";
                ws.Range("W3:W4").Merge();

                ws.Cell("X3").Value = "Cálculo do frete";
                ws.Cell("X3").Style.Font.Bold = true;
                ws.Range("X3:X4").Merge();
                ws.Columns("X").Style.NumberFormat.Format = "R$ #,##0.00";

                ws.Cell("Y3").Value = "Cálculo c/ Cubagem Transportadora";
                ws.Cell("Y3").Style.Font.Bold = true ;
                ws.Range("Y3:Y4").Merge();
                ws.Columns("Y").Style.NumberFormat.Format = "R$ #,##0.00";

                //LISTA COM OS NOMES DAS DISCRIMINAÇÕES DO CTE
                List<string> ListXNome = new List<string>();
                ListXNome.Add("FRETE VALOR");
                ListXNome.Add("FRETE PESO");
                ListXNome.Add("GRIS");

                int linha = 5;
                int ColunasExtras = 0;

                char colunaExcel = 'Y';
                string strColunaExcel = "";

                foreach (DataSet dataSet in ListDataSet)
                {
                    ContadorLinhaErro++;
 
                    //LISTA COM OS VALORES DAS DISCRIMINAÇÕES DO CTE
                    List<string> ListXComp = new List<string>();
                    string strBaseCalculoICMS = "",strValorPedagio = "", strPercentualICMS = "";
                    byte[] ArquivoXML = null;

                    string xComp = "",ChaveCTe = "", CNPJRem = "", NomeRem = "", CNPJEmi = "", NomeEmi = "", CNPJDest = "", NomeDest = "", ValorCTe = "0", PesoCTe = "0", Frete = "0",
                        ChaveNota = "", ValorNota = "0", PesoNota = "0", ValorICMS = "0", MunDestino = "", CodMunDestino = "", Modalidade = "", DataHoraNFe = "", CubagemNFe = "0", CubagemCTe = "0";

                    //BUSCA NO DATATABLE AS INFORMAÇÕES NECESSARIAS
                    foreach (DataTable thisTable in dataSet.Tables)
                    {
                        int contadorPeso = 0;
                        int contadorCub = 0;
                        int contadorPedagio = 0;

                        string i = "";
                        bool IsVolume = false;

                        foreach (DataRow row in thisTable.Rows)
                        {
                            foreach (DataColumn column in thisTable.Columns)
                            {
                                if (contadorPeso > 0)
                                {
                                    if (column.ColumnName == "qCarga")
                                    { 
                                        PesoCTe = row[column].ToString();
                                        contadorPeso = 0;
                                    }
                                }
                                if (contadorCub > 0)
                                {
                                    if (column.ColumnName == "qCarga")
                                    {
                                        if(IsVolume)
                                            CubagemCTe = (Convert.ToDecimal(row[column].ToString().Replace('.',',')) /100).ToString();
                                        else
                                            CubagemCTe = row[column].ToString();
                                        contadorCub = 0;
                                    }
                                }
                                if (contadorPedagio > 0)
                                {
                                    if (column.ColumnName == "vComp")
                                    {
                                        strValorPedagio = row[column].ToString();
                                        contadorPedagio = 0;
                                    }
                                }
                                if (thisTable.TableName == "emit" && column.ColumnName == "CNPJ")
                                    CNPJEmi = row[column].ToString();
                                else if (thisTable.TableName == "emit" && column.ColumnName == "xNome")
                                    NomeEmi = row[column].ToString();
                                else if (thisTable.TableName == "rem" && column.ColumnName == "CNPJ")
                                    CNPJRem = row[column].ToString();
                                else if (thisTable.TableName == "rem" && column.ColumnName == "xNome")
                                    NomeRem = row[column].ToString();
                                else if (thisTable.TableName == "dest" && column.ColumnName == "CNPJ")
                                    CNPJDest = row[column].ToString();
                                else if (thisTable.TableName == "dest" && column.ColumnName == "xNome")
                                    NomeDest = row[column].ToString();
                                else if (column.ColumnName == "vRec")
                                    Frete = row[column].ToString();
                                else if (column.ColumnName == "vICMS")
                                    ValorICMS = row[column].ToString();
                                else if (column.ColumnName == "chCTe")
                                    ChaveCTe = row[column].ToString();
                                else if (column.ColumnName == "chave")
                                    ChaveNota += row[column].ToString() + "³";
                                else if (column.ColumnName == "vTPrest")
                                    ValorCTe = row[column].ToString();
                                else if (row[column].ToString().ToUpper() == "PESO REAL" || row[column].ToString().ToUpper() == "PESO DECLARADO" || row[column].ToString().ToUpper() == "PESO BRUTO" || row[column].ToString().ToUpper() == "PESO")
                                    contadorPeso++;
                                else if (row[column].ToString() == "M3")
                                    contadorCub++;
                                else if (row[column].ToString() == "VOLUMES" || row[column].ToString() == "VOLUME")
                                {
                                    contadorCub++;
                                    IsVolume = true;
                                }
                                else if (row[column].ToString() == "CUBAGEM")
                                {
                                    contadorCub++;
                                    IsVolume = false;
                                }

                                else if (column.ColumnName == "xMunFim")
                                    MunDestino = row[column].ToString();
                                else if (column.ColumnName == "pICMS")
                                    strPercentualICMS = row[column].ToString();
                                else if (column.ColumnName == "UFFim")
                                    MunDestino += " - " + row[column].ToString();
                                else if (column.ColumnName == "cMunFim")
                                    CodMunDestino = row[column].ToString();
                                else if (column.ColumnName == "vBC")
                                    strBaseCalculoICMS = row[column].ToString();
                                else if (column.ColumnName == "XML_BINARY")
                                    ArquivoXML = (byte[])row[column];
                                else if (thisTable.TableName == "Comp" && column.ColumnName == "xNome")
                                {
                                    var existe = ListXNome.Where(x => x == row[column].ToString().ToUpper());
                                    if (existe.Count() == 0)
                                    {
                                        ListXNome.Add(row[column].ToString().ToUpper());
                                    }
                                    i = row[column].ToString().ToUpper();
                                }
                                else if (thisTable.TableName == "Comp" && column.ColumnName == "vComp")
                                {
                                    xComp += "³" + row[column].ToString();
                                    ListXComp.Add(i + "³" + row[column].ToString());
                                }


                                if (row[column].ToString() == "PEDÁGIO" || row[column].ToString() == "PEDAGIO")
                                {
                                    contadorPedagio++;
                                }
                            }
                        }
                    }
                    
                    //INSERE AS INFORMAÇÕES DO XML NA PLANILHA
                    ConferenciaFreteDAL ffDAL = new ConferenciaFreteDAL();
                    ws.Cell("C" + linha).Value = CNPJEmi;
                    ws.Cell("B" + linha).Value = "'" + ChaveCTe;
                    ws.Cell("D" + linha).Value = NomeEmi;
                    ws.Cell("E" + linha).Value = CNPJRem;
                    ws.Cell("F" + linha).Value = NomeRem;
                    ws.Cell("G" + linha).Value = CNPJDest;
                    ws.Cell("H" + linha).Value = NomeDest;
                    ws.Cell("I" + linha).Value = MunDestino;
                    ws.Cell("J" + linha).Value = "";// ffDAL.PesquisarNumeroFatura(ChaveCTe);
                    ws.Cell("K" + linha).Value = ValorCTe;
                    ws.Cell("K" + linha).Style.Fill.BackgroundColor = XLColor.FromArgb(221, 217, 195);
                    ws.Cell("L" + linha).Value = PesoCTe;
                    ws.Cell("M" + linha).Value = CubagemCTe;

                    if (ChaveNota != "")
                        ws.Cell("W" + linha).Value = ffDAL.PesquisarNumeroNotasClienteMesmaData(ChaveNota.Split('³')[0], Convert.ToInt32(Session["CodEmpresa"].ToString()).ToString("D2"));

                    ws.Cell("X" + linha).Value = ValorICMS;

                    if (ChaveNota != "")
                    {
                        decimal TotalPesoNF = 0;
                        decimal TotalCubagemNF = 0;
                        decimal TotalPeso = 0;
                        decimal TotalValor = 0;

                        string[] chNota = ChaveNota.Split('³');

                        int linhaMerge = (chNota.Count() - 2) + linha;

                        // MESCLA LINHAS CONFORME O NUMERO DE NOTAS NO CTE
                        ws.Range("B" + linha + ":B" + linhaMerge).Merge();
                        ws.Range("D" + linha + ":D" + linhaMerge).Merge();
                        ws.Range("E" + linha + ":E" + linhaMerge).Merge();
                        ws.Range("F" + linha + ":F" + linhaMerge).Merge();
                        ws.Range("C" + linha + ":C" + linhaMerge).Merge();
                        ws.Range("G" + linha + ":G" + linhaMerge).Merge();
                        ws.Range("H" + linha + ":H" + linhaMerge).Merge();
                        ws.Range("I" + linha + ":I" + linhaMerge).Merge();
                        ws.Range("J" + linha + ":J" + linhaMerge).Merge();
                        ws.Range("K" + linha + ":K" + linhaMerge).Merge();
                        ws.Range("L" + linha + ":L" + linhaMerge).Merge();
                        ws.Range("M" + linha + ":M" + linhaMerge).Merge();
                        ws.Range("S" + linha + ":S" + linhaMerge).Merge();
                        ws.Range("T" + linha + ":T" + linhaMerge).Merge();
                        ws.Range("X" + linha + ":X" + linhaMerge).Merge();
                        ws.Range("W" + linha + ":W" + linhaMerge).Merge();
                        ws.Range("U" + linha + ":U" + linhaMerge).Merge();
                        ws.Range("V" + linha + ":V" + linhaMerge).Merge();
                        ws.Range("W" + linha + ":W" + linhaMerge).Merge();
                        ws.Range("Y" + linha + ":Y" + linhaMerge).Merge();
                        ws.Range(GetColNameFromIndex(24 + ListXNome.Count) + linha + ":" + GetColNameFromIndex(24 + ListXNome.Count) + linhaMerge).Merge();
                        ws.Range(GetColNameFromIndex(25 + ListXNome.Count) + linha + ":" + GetColNameFromIndex(25 + ListXNome.Count) + linhaMerge).Merge();
                        ws.Range(GetColNameFromIndex(26 + ListXNome.Count) + linha + ":" + GetColNameFromIndex(26 + ListXNome.Count) + linhaMerge).Merge();
                        ws.Range(GetColNameFromIndex(27 + ListXNome.Count) + linha + ":" + GetColNameFromIndex(27 + ListXNome.Count) + linhaMerge).Merge();

                        // CONFERENCIA DA BASE DE CALCULO
                        decimal ValorBaseCalculoSemPedagio = 0;
                        if (strBaseCalculoICMS != "" && strValorPedagio != "" && strPercentualICMS != "")
                        {
                            ValorBaseCalculoSemPedagio = (Convert.ToDecimal(strBaseCalculoICMS.Replace('.', ',')) - Convert.ToDecimal(strValorPedagio.Replace('.', ',')));
                            ws.Cell("S" + linha).Value = ValorBaseCalculoSemPedagio.ToString("F");
                            ws.Cell("S" + linha).Style.Fill.BackgroundColor = XLColor.Cyan;
                            ws.Columns("S").Style.NumberFormat.Format = "R$ #,##0.00";

                            ws.Cell("T" + linha).FormulaA1 = ValorBaseCalculoSemPedagio.ToString().Replace(',', '.') + "*(" + strPercentualICMS + "/100)";
                            ws.Cell("T" + linha).Style.Fill.BackgroundColor = XLColor.Cyan;
                        }
                        else if (strBaseCalculoICMS != "" && strPercentualICMS != "")
                        {
                            ValorBaseCalculoSemPedagio = Convert.ToDecimal(strBaseCalculoICMS.Replace('.', ','));
                            ws.Cell("S" + linha).Value = ValorBaseCalculoSemPedagio.ToString("F");
                            ws.Cell("S" + linha).Style.Fill.BackgroundColor = XLColor.Cyan;
                            ws.Columns("S").Style.NumberFormat.Format = "R$ #,##0.00";

                            ws.Cell("T" + linha).FormulaA1 = "=" + ValorBaseCalculoSemPedagio.ToString().Replace(',', '.') + "*(" + strPercentualICMS + "/100)";
                            ws.Cell("T" + linha).Style.Fill.BackgroundColor = XLColor.Cyan;
                        }
                        else if (strBaseCalculoICMS != "" && strValorPedagio != "" && ValorICMS != "")
                        {
                            decimal ValorBaseCalculo = Convert.ToDecimal(strBaseCalculoICMS.Replace('.', ','));
                            ValorBaseCalculoSemPedagio = (Convert.ToDecimal(strBaseCalculoICMS.Replace('.', ',')) - Convert.ToDecimal(strValorPedagio.Replace('.', ',')));
                       
                            ws.Cell("S" + linha).Value = ValorBaseCalculoSemPedagio.ToString("F");
                            ws.Cell("S" + linha).Style.Fill.BackgroundColor = XLColor.Cyan;
                            ws.Columns("S").Style.NumberFormat.Format = "R$ #,##0.00";

                            // SE NÃO TIVER A ALIQUOTA NO XML
                            decimal ValorAliquotaICMS = Convert.ToDecimal(((Convert.ToDecimal(ValorICMS) / ValorBaseCalculoSemPedagio) * 100).ToString("F"));

                            ws.Cell("T" + linha).FormulaA1 = "=" + ValorBaseCalculoSemPedagio.ToString().Replace(',', '.') + "*(" + ValorAliquotaICMS.ToString().Replace(',', '.') + "/100)";
                            ws.Cell("T" + linha).Style.Fill.BackgroundColor = XLColor.Cyan;
                        }
                        else if (strBaseCalculoICMS != "" && ValorICMS != "")
                        {
                            ValorBaseCalculoSemPedagio = Convert.ToDecimal(strBaseCalculoICMS.Replace('.', ','));
                            ws.Cell("S" + linha).Value = ValorBaseCalculoSemPedagio.ToString("F");
                            ws.Cell("S" + linha).Style.Fill.BackgroundColor = XLColor.Cyan;
                            ws.Columns("S").Style.NumberFormat.Format = "R$ #,##0.00";

                            // SE NÃO TIVER A ALIQUOTA NO XML
                            decimal ValorAliquotaICMS = Convert.ToDecimal(((Convert.ToDecimal(ValorICMS)/ ValorBaseCalculoSemPedagio) * 100).ToString("F"));

                            ws.Cell("T" + linha).FormulaA1 = "=" + ValorBaseCalculoSemPedagio.ToString().Replace(',', '.') + "*(" + ValorAliquotaICMS.ToString().Replace(',', '.') + "/100)";
                            ws.Cell("T" + linha).Style.Fill.BackgroundColor = XLColor.Cyan;
                        }
                        else
                        {
                            ws.Cell("S" + linha).Value = "XML sem base de cálculo ou alíquota ICMS ";
                            ws.Cell("T" + linha).Value = "XML sem base de cálculo ou alíquota ICMS ";
                        }

                        //-------------------------------------

                        for (int i = 0; i < chNota.Count(); i++)
                        {
                            string strNota = chNota[i];
                            if (strNota != "")
                            {
                                DataTable dt = new DataTable();
                                DataTable dt2 = new DataTable();

                                dt = ffDAL.PesquisarPorChaveAcessoQuery1(strNota, Convert.ToInt32(Session["CodEmpresa"].ToString()).ToString("D2"));

                                if (dt.Rows.Count == 0)
                                {
                                    dt2 = ffDAL.PesquisarPorChaveAcessoQuery2(strNota, Convert.ToInt32(Session["CodEmpresa"].ToString()).ToString("D2"));
                                    Modalidade = "";
                                }

                                foreach (DataRow row in dt.Rows)
                                {
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        if (column.ColumnName == "pesb" || column.ColumnName == "PesoNF")
                                        {
                                            PesoNota = row[column].ToString();
                                            TotalPeso += Convert.ToDecimal(row[column].ToString());
                                        }
                                        else if (column.ColumnName == "vtnf")
                                        {
                                            ValorNota = row[column].ToString();
                                            TotalValor += Convert.ToDecimal(row[column].ToString());
                                        }
                                        else if (column.ColumnName == "fret")
                                            Frete = row[column].ToString();
                                        else if (column.ColumnName == "tipf")
                                            Modalidade = row[column].ToString();
                                        else if (column.ColumnName == "dtnf")
                                            DataHoraNFe = row[column].ToString();
                                        else if (column.ColumnName == "cubagem")
                                            CubagemNFe = row[column].ToString();

                                    }
                                }
                                foreach (DataRow row in dt2.Rows)
                                {
                                    foreach (DataColumn column in dt2.Columns)
                                    {
                                        if (column.ColumnName == "PesoNF")
                                        {
                                            PesoNota = row[column].ToString();
                                            TotalPeso += Convert.ToDecimal(row[column].ToString());
                                        }
                                        else if (column.ColumnName == "vtnf")
                                        {
                                            ValorNota = row[column].ToString();
                                            TotalValor += Convert.ToDecimal(row[column].ToString());
                                        }
                                        else if (column.ColumnName == "fret")
                                            Frete = row[column].ToString();
                                        else if (column.ColumnName == "dtnf")
                                            DataHoraNFe = row[column].ToString();
                                        else if (column.ColumnName == "cubNF")
                                            CubagemNFe = row[column].ToString();
                                    }
                                }

                                ws.Cell("N" + linha).Value = "'" + strNota;
                                ws.Cell("O" + linha).Value = ValorNota.Replace(',', '.');
                                ws.Cell("P" + linha).Value = PesoNota.Replace(',', '.');
                                ws.Cell("Q" + linha).Value = CubagemNFe.Replace(',', '.');
                                ws.Cell("R" + linha).Value = Modalidade;

                                ws.Cell("U" + linha).Value = DataHoraNFe;
                                ws.Cell("V" + linha).Value = Frete.Replace(',', '.');

                                ws.Range("N" + linha + ":S" + linha).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                ws.Range("N" + linha + ":S" + linha).Style.Border.OutsideBorderColor = XLColor.Black;
                                ws.Range("N" + linha + ":S" + linha).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                                ws.Range("N" + linha + ":S" + linha).Style.Border.InsideBorderColor = XLColor.Black;

                                TotalPesoNF += Convert.ToDecimal(PesoNota);
                                TotalCubagemNF += Convert.ToDecimal(CubagemNFe);
                                //-------------------------------------------------------------------

                                linha++;
                            }
                        }

                        RegraFrete RegFrete = new RegraFrete();
                        RegraFreteDAL RegFreteDAL = new RegraFreteDAL();
                        RegFrete = RegFreteDAL.PesquisarRegraFrete(CNPJEmi, CodMunDestino);

                        if (RegFrete != null)
                        {
                            // CALCULO DE FRETE UTILIZANDO CUBAGEM DA TRASNPORTADOR
                            bool ExisteRegraPorCNPJ = false;

                            decimal ValorFrete = 0;
                            decimal CubagemBaseCalculo = (Convert.ToDecimal(TotalCubagemNF) * 2) * 100;
                            decimal BaseCalculo = TotalPeso;

                            if (RegFrete.IndicadorTipoCalculo == 1)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "TARIFA SEGURO" && array[0].ToUpper() != "FRETE PESO (PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }

                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = TotalValor * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }

                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }

                                if (BaseCalculo <= RegFrete.DePara12)
                                {
                                    if (RegFrete.IndicadorCalcularAdValorDePara1 == 1)
                                    {
                                        //CALCULAR VALOR AD
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                    }

                                    ValorFrete = Convert.ToDecimal((TotalValor * (RegFrete.DeParaPct11 / 100)).ToString("F"));
                                    if (ValorFrete > RegFrete.ValorFreteMinimo)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + (TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes;
                                    else
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = RegFrete.ValorFreteMinimo + ((TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes);
                                }
                                else
                                {
                                    if (RegFrete.ValorPorTonelada > 0)
                                    {
                                        decimal ValorPorTonelada = ((Convert.ToDecimal(PesoCTe.Replace('.', ',')) - 100) / 1000) * RegFrete.ValorPorTonelada;
                                        ValorDiscriminacoes += ValorPorTonelada;
                                    }

                                    //CALCULAR EXCEDENTE
                                    if (RegFrete.DeParaExcedente1 > 0)
                                    {
                                        ValorDiscriminacoes += RegFrete.DeParaExcedente1;
                                    }

                                    ValorFrete = Convert.ToDecimal((BaseCalculo * RegFrete.DeParaPct12).ToString("F"));

                                    //SOMAR VALOR AD
                                    if (RegFrete.IndicadorCalcularAdValorDePara2 == 1)
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));

                                    if (ValorFrete > RegFrete.ValorFreteMinimo)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = (ValorFrete + (TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes).ToString("F");
                                    else
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = (RegFrete.ValorFreteMinimo + ((TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes)).ToString("F");

                                }
                            }
                            else if (RegFrete.IndicadorTipoCalculo == 2)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "FRETE PESO (PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }


                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = Convert.ToDecimal(ValorNota) * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }

                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }

                                // APENAS SE NÃO FOR TRANSPORTADORA SUDOESTE
                                if (CNPJEmi != "02343801000185")
                                {
                                    //CALCULAR VALOR AD
                                    if (RegFrete.ValorAD != 0)
                                        ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                }

                                //CALCULAR GRIS
                                decimal ValorGRIS = 0;
                                if (RegFrete.ValorGRIS > 0)
                                    ValorGRIS = TotalValor * (RegFrete.ValorGRIS / 100);

                                //VERIFICA SE GRIS CALCULADO É MAIOR QUE GRIS MINIMO
                                if (ValorGRIS < RegFrete.ValorGRISMinimo)
                                    ValorGRIS = RegFrete.ValorGRISMinimo;

                                if (BaseCalculo <= RegFrete.DePara11 && TotalValor <= RegFrete.DePara12 && RegFrete.DePara12 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct11;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara21 && TotalValor <= RegFrete.DePara22 && RegFrete.DePara22 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct12;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara31 && TotalValor <= RegFrete.DePara32 && RegFrete.DePara32 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct13;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara41 && TotalValor <= RegFrete.DePara42 && RegFrete.DePara42 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct14;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo > RegFrete.DePara41 || TotalValor > RegFrete.DePara42)
                                {
                                    // APENAS SE FOR TRANSPORTADORA SUDOESTE
                                    if (CNPJEmi == "02343801000185" && TotalValor > RegFrete.DePara12)
                                    {
                                        //CALCULAR VALOR AD
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                    }


                                    //CALCULAR EXCEDENTE POR VALOR
                                    decimal ExcedentePorValor = ((TotalValor - RegFrete.DePara42) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara42 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara32) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara32 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara22) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara22 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara12) * (RegFrete.DeParaExcedente2 / 100));

                                    //CALCULAR EXCEDENTE POR PESO
                                    decimal ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara41) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara41 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara31) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara31 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara21) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara21 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara11) * RegFrete.DeParaExcedente1);

                                    decimal FreteMinimo = RegFrete.DeParaPct14;
                                    if (RegFrete.DeParaPct14 == 0)
                                        FreteMinimo = RegFrete.DeParaPct13;
                                    if (RegFrete.DeParaPct13 == 0)
                                        FreteMinimo = RegFrete.DeParaPct12;
                                    if (RegFrete.DeParaPct12 == 0)
                                        FreteMinimo = RegFrete.DeParaPct11;

                                    if (ExcedentePorValor > 0 && ExcedentePorPeso > 0)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = (ExcedentePorValor + ExcedentePorPeso + ValorGRIS + ValorDiscriminacoes + FreteMinimo);
                                    else if (ExcedentePorPeso > 0)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ExcedentePorPeso + ValorGRIS + ValorDiscriminacoes + FreteMinimo;
                                    else if (ExcedentePorValor > 0)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ExcedentePorValor + ValorGRIS + ValorDiscriminacoes + FreteMinimo;
                                    else
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = "0";
                                }
                            }
                            else if (RegFrete.IndicadorTipoCalculo == 3)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "FRETE PESO (PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }

                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = Convert.ToDecimal(ValorNota) * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }

                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }

                                //CALCULAR VALOR AD
                                if (RegFrete.ValorAD != 0)
                                    ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));

                                //CALCULAR GRIS
                                decimal ValorGRIS = 0;
                                if (RegFrete.ValorGRIS > 0)
                                    ValorGRIS = TotalValor * (RegFrete.ValorGRIS / 100);

                                //VERIFICA SE GRIS CALCULADO É MAIOR QUE GRIS MINIMO
                                if (ValorGRIS < RegFrete.ValorGRISMinimo)
                                    ValorGRIS = RegFrete.ValorGRISMinimo;

                                if (BaseCalculo <= RegFrete.DePara11 && TotalValor <= RegFrete.DePara12 && RegFrete.DePara12 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct11;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara21 && TotalValor <= RegFrete.DePara22 && RegFrete.DePara22 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct12;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara31 && TotalValor <= RegFrete.DePara32 && RegFrete.DePara32 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct13;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara41 && TotalValor <= RegFrete.DePara42 && RegFrete.DePara42 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct14;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara51 && TotalValor <= RegFrete.DePara52 && RegFrete.DePara52 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct15;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara61 && TotalValor <= RegFrete.DePara62 && RegFrete.DePara62 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct16;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara71 && TotalValor <= RegFrete.DePara72 && RegFrete.DePara72 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct17;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo > RegFrete.DePara61 || TotalValor > RegFrete.DePara62)
                                {
                                    //CALCULAR EXCEDENTE POR VALOR
                                    decimal ExcedentePorValor = ((TotalValor - RegFrete.DePara72) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara72 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara62) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara62 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara52) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara52 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara42) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara42 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara32) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara32 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara22) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara22 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara12) * (RegFrete.DeParaExcedente2 / 100));

                                    //CALCULAR EXCEDENTE POR PESO
                                    decimal ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara71) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara71 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara61) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara61 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara51) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara51 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara41) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara41 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara31) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara31 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara21) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara21 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara11) * RegFrete.DeParaExcedente1);

                                    decimal FreteMinimo = RegFrete.DeParaPct17;
                                    if (RegFrete.DeParaPct17 == 0)
                                        FreteMinimo = RegFrete.DeParaPct16;
                                    if (RegFrete.DeParaPct16 == 0)
                                        FreteMinimo = RegFrete.DeParaPct15;
                                    if (RegFrete.DeParaPct15 == 0)
                                        FreteMinimo = RegFrete.DeParaPct14;
                                    if (RegFrete.DeParaPct14 == 0)
                                        FreteMinimo = RegFrete.DeParaPct13;
                                    if (RegFrete.DeParaPct13 == 0)
                                        FreteMinimo = RegFrete.DeParaPct12;
                                    if (RegFrete.DeParaPct12 == 0)
                                        FreteMinimo = RegFrete.DeParaPct11;

                                    if (ExcedentePorValor > 0 && ExcedentePorPeso > 0)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = (ExcedentePorValor + ExcedentePorPeso + ValorGRIS + ValorDiscriminacoes + FreteMinimo);
                                    else if (ExcedentePorPeso > 0)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ExcedentePorPeso + ValorGRIS + ValorDiscriminacoes + FreteMinimo;
                                    else if (ExcedentePorValor > 0)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ExcedentePorValor + ValorGRIS + ValorDiscriminacoes + FreteMinimo;
                                    else
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = "0";
                                }
                            }
                            else if (RegFrete.IndicadorTipoCalculo == 4)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "TARIFA SEGURO" && array[0].ToUpper() != "FRETE PESO (PES" && array[0].ToUpper() != "FRETE PESO/ PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }
                                }

                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = TotalValor * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }

                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }

                                if (BaseCalculo <= RegFrete.DePara12)
                                {
                                    if (RegFrete.IndicadorCalcularAdValorDePara1 == 1)
                                    {
                                        //CALCULAR VALOR AD
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                    }
                                    //CALCULAR EXCEDENTE
                                    if (RegFrete.DeParaExcedente1 > 0 && BaseCalculo > RegFrete.DePara12)
                                    {
                                        ValorDiscriminacoes += RegFrete.DeParaExcedente1;
                                    }



                                    ValorFrete = Convert.ToDecimal((TotalValor * (RegFrete.DeParaPct11 / 100)).ToString("F"));
                                    if (ValorFrete > RegFrete.ValorFreteMinimo)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + (TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes;
                                    else
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = RegFrete.ValorFreteMinimo + ((TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes);
                                }
                                else
                                {
                                    if (RegFrete.ValorPorTonelada > 0)
                                    {
                                        decimal ValorPorTonelada = ((Convert.ToDecimal(PesoCTe.Replace('.', ',')) - 100)) * RegFrete.ValorPorTonelada;
                                        ValorDiscriminacoes += ValorPorTonelada;
                                    }

                                    //CALCULAR EXCEDENTE
                                    if (RegFrete.DeParaExcedente1 > 0 && BaseCalculo > RegFrete.DePara12)
                                    {
                                        ValorDiscriminacoes += RegFrete.DeParaExcedente1;
                                    }


                                    if (RegFrete.IndicadorCalcularAdValorDePara1 == 1)
                                    {
                                        //CALCULAR VALOR AD
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                    }

                                    ValorFrete = Convert.ToDecimal((TotalValor * (RegFrete.DeParaPct11 / 100)).ToString("F"));
                                    if (ValorFrete > RegFrete.ValorFreteMinimo)
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + (TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes;
                                    else
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = RegFrete.ValorFreteMinimo + ((TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes);

                                }
                            }
                            else if (RegFrete.IndicadorTipoCalculo == 5)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "TARIFA SEGURO" && array[0].ToUpper() != "FRETE PESO (PES" && array[0].ToUpper() != "FRETE PESO/ PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }

                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = TotalValor * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }

                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }

                                //CALCULAR EXCEDENTE
                                if (RegFrete.DeParaExcedente1 > 0 && BaseCalculo > RegFrete.DePara12)
                                {
                                    ValorFrete += (BaseCalculo) * RegFrete.DeParaExcedente1;
                                }

                                if (RegFrete.ValorPorTonelada > 0)
                                {
                                    decimal ValorPorTonelada = (BaseCalculo / 1000) * RegFrete.ValorPorTonelada;
                                    ValorFrete += ValorPorTonelada;
                                }
                                if (RegFrete.ValorGRIS > 0)
                                {
                                    decimal ValorGris = ((TotalValor * (RegFrete.ValorGRIS / 100)));
                                    if (ValorGris < RegFrete.ValorGRISMinimo)
                                        ValorDiscriminacoes += RegFrete.ValorGRISMinimo;
                                    else
                                        ValorDiscriminacoes += ((TotalValor * (RegFrete.ValorGRIS / 100)));
                                }
                                if (RegFrete.IndicadorCalcularAdValorDePara1 == 1)
                                {
                                    //CALCULAR VALOR AD

                                    if (RegFrete.ValorAD != 0)
                                    {
                                        decimal ADvalor = (TotalValor * (RegFrete.ValorAD / 100));
                                        if (RegFrete.IndicadorCalcularAdValorDePara2 == 1)
                                            ValorFrete += ADvalor;
                                        else if (ADvalor > ValorFrete)
                                            ValorFrete = ADvalor;
                                    }
                                }
                                if (RegFrete.DeParaPct11 > 0)
                                    ValorFrete = Convert.ToDecimal((TotalValor * (RegFrete.DeParaPct11 / 100)).ToString("F"));

                                if (ValorFrete > RegFrete.ValorFreteMinimo)
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = (ValorFrete + ValorDiscriminacoes).ToString("F");
                                else
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = (RegFrete.ValorFreteMinimo + ValorDiscriminacoes).ToString("F");


                            }

                            if (ExisteRegraPorCNPJ == true)
                            {
                                string cellCalculo = Convert.ToDecimal(ws.Cell("X" + (linha - chNota.Count() + 1)).Value).ToString("F");
                                if (cellCalculo.Replace(',', '.') != ValorCTe)
                                {
                                    decimal Diferenca = Convert.ToDecimal(cellCalculo.Replace(',', '.')) - Convert.ToDecimal(ValorCTe);

                                    if (Diferenca <= Convert.ToDecimal(3) && Diferenca >= Convert.ToDecimal(-3))
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorCTe;
                                    else
                                        ws.Cell("X" + (linha - chNota.Count() + 1)).Style.Fill.BackgroundColor = XLColor.YellowProcess;
                                }
                            }
                            //----------------------------------------------


                            // CALCULO DE FRETE UTILIZANDO CUBAGEM DA FABESUL
                            ValorFrete = 0;
                            ExisteRegraPorCNPJ = false;
                            CubagemBaseCalculo = 0;
                            BaseCalculo = TotalPeso;
                            CubagemBaseCalculo = (Convert.ToDecimal(CubagemCTe.Replace('.', ',')) * 2) * 100;

                            if (RegFrete.IndicadorTipoCalculo == 1)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "TARIFA SEGURO" && array[0].ToUpper() != "FRETE PESO (PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }

                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = TotalValor * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }
                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }

                                if (BaseCalculo <= RegFrete.DePara12)
                                {
                                    //CALCULAR EXCEDENTE
                                    if (RegFrete.DeParaExcedente1 > 0 && BaseCalculo > RegFrete.DePara12)
                                    {
                                        ValorDiscriminacoes += RegFrete.DeParaExcedente1;
                                    }

                                    if (RegFrete.IndicadorCalcularAdValorDePara1 == 1)
                                    {
                                        //CALCULAR VALOR AD
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                    }

                                    ValorFrete = Convert.ToDecimal((TotalValor * (RegFrete.DeParaPct11 / 100)).ToString("F"));
                                    if (ValorFrete > RegFrete.ValorFreteMinimo)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + (TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes;
                                    else
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = RegFrete.ValorFreteMinimo + ((TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes);
                                }
                                else
                                {
                                    //CALCULAR EXCEDENTE
                                    if (RegFrete.DeParaExcedente1 > 0)
                                    {
                                        ValorDiscriminacoes += RegFrete.DeParaExcedente1;
                                    }
                                    if (RegFrete.ValorPorTonelada > 0)
                                    {
                                        decimal ValorPorTonelada = ((Convert.ToDecimal(PesoCTe.Replace('.', ',')) - 100) / 1000) * RegFrete.ValorPorTonelada;
                                        ValorDiscriminacoes += ValorPorTonelada;
                                    }

                                    ValorFrete = Convert.ToDecimal((BaseCalculo * RegFrete.DeParaPct12).ToString("F"));

                                    //SOMAR VALOR AD
                                    if (RegFrete.IndicadorCalcularAdValorDePara2 == 1)
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));

                                    if (ValorFrete > RegFrete.ValorFreteMinimo)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + (TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes;
                                    else
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = RegFrete.ValorFreteMinimo + ((TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes);

                                }
                            }
                            else if (RegFrete.IndicadorTipoCalculo == 2)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "FRETE PESO (PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }

                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }
                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = Convert.ToDecimal(ValorNota) * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }

                                // APENAS SE NÃO FOR TRANSPORTADORA SUDOESTE
                                if (CNPJEmi != "02343801000185")
                                {
                                    //CALCULAR VALOR AD
                                    if (RegFrete.ValorAD != 0)
                                        ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                }

                                //CALCULAR GRIS
                                decimal ValorGRIS = 0;
                                if (RegFrete.ValorGRIS > 0)
                                    ValorGRIS = TotalValor * (RegFrete.ValorGRIS / 100);

                                //VERIFICA SE GRIS CALCULADO É MAIOR QUE GRIS MINIMO
                                if (ValorGRIS < RegFrete.ValorGRISMinimo)
                                    ValorGRIS = RegFrete.ValorGRISMinimo;

                                if (BaseCalculo <= RegFrete.DePara11 && TotalValor <= RegFrete.DePara12 && RegFrete.DePara12 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct11;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara21 && TotalValor <= RegFrete.DePara22 && RegFrete.DePara22 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct12;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara31 && TotalValor <= RegFrete.DePara32 && RegFrete.DePara32 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct13;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara41 && TotalValor <= RegFrete.DePara42 && RegFrete.DePara42 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct14;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo > RegFrete.DePara41 || TotalValor > RegFrete.DePara42)
                                {

                                    // APENAS SE FOR TRANSPORTADORA SUDOESTE
                                    if (CNPJEmi == "02343801000185" && TotalValor > RegFrete.DePara12)
                                    {
                                        //CALCULAR VALOR AD
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                    }

                                    // CALCULO DO EXCEDENTE POR VALOR
                                    decimal ExcedentePorValor = ((TotalValor - RegFrete.DePara42) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara42 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara32) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara32 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara22) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara22 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara12) * (RegFrete.DeParaExcedente2 / 100));

                                    //CALCULO DO EXCEDENTE POR PESO
                                    decimal ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara41) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara41 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara31) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara31 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara21) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara21 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara11) * RegFrete.DeParaExcedente1);

                                    decimal FreteMinimo = RegFrete.DeParaPct14;
                                    if (RegFrete.DeParaPct14 == 0)
                                        FreteMinimo = RegFrete.DeParaPct13;
                                    if (RegFrete.DeParaPct13 == 0)
                                        FreteMinimo = RegFrete.DeParaPct12;
                                    if (RegFrete.DeParaPct12 == 0)
                                        FreteMinimo = RegFrete.DeParaPct11;

                                    if (ExcedentePorValor > 0 && ExcedentePorPeso > 0)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = (ExcedentePorValor + ExcedentePorPeso + ValorGRIS + ValorDiscriminacoes + FreteMinimo);
                                    else if (ExcedentePorPeso > 0)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ExcedentePorPeso + ValorGRIS + ValorDiscriminacoes + FreteMinimo;
                                    else if (ExcedentePorValor > 0)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ExcedentePorValor + ValorGRIS + ValorDiscriminacoes + FreteMinimo;
                                    else
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = "0";
                                }
                            }
                            else if (RegFrete.IndicadorTipoCalculo == 3)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "FRETE PESO (PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }

                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = Convert.ToDecimal(ValorNota) * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }

                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }

                                //CALCULAR VALOR AD
                                if (RegFrete.ValorAD != 0)
                                    ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));

                                //CALCULAR GRIS
                                decimal ValorGRIS = 0;
                                if (RegFrete.ValorGRIS > 0)
                                    ValorGRIS = TotalValor * (RegFrete.ValorGRIS / 100);

                                //VERIFICA SE GRIS CALCULADO É MAIOR QUE GRIS MINIMO
                                if (ValorGRIS < RegFrete.ValorGRISMinimo)
                                    ValorGRIS = RegFrete.ValorGRISMinimo;

                                if (BaseCalculo <= RegFrete.DePara11 && TotalValor <= RegFrete.DePara12 && RegFrete.DePara12 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct11;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara21 && TotalValor <= RegFrete.DePara22 && RegFrete.DePara22 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct12;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara31 && TotalValor <= RegFrete.DePara32 && RegFrete.DePara32 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct13;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara41 && TotalValor <= RegFrete.DePara42 && RegFrete.DePara42 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct14;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara51 && TotalValor <= RegFrete.DePara52 && RegFrete.DePara52 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct15;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara61 && TotalValor <= RegFrete.DePara62 && RegFrete.DePara62 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct16;
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo <= RegFrete.DePara71 && TotalValor <= RegFrete.DePara72 && RegFrete.DePara72 > 0)
                                {
                                    ValorFrete = RegFrete.DeParaPct17;
                                    ws.Cell("X" + (linha - chNota.Count() + 1)).Value = ValorFrete + ValorGRIS + ValorDiscriminacoes;
                                }
                                else if (BaseCalculo > RegFrete.DePara61 || TotalValor > RegFrete.DePara62)
                                {

                                    //CALCULAR EXCEDENTE POR VALOR
                                    decimal ExcedentePorValor = ((TotalValor - RegFrete.DePara72) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara72 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara62) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara62 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara52) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara52 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara42) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara42 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara32) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara32 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara22) * (RegFrete.DeParaExcedente2 / 100));
                                    if (RegFrete.DePara22 == 0)
                                        ExcedentePorValor = ((TotalValor - RegFrete.DePara12) * (RegFrete.DeParaExcedente2 / 100));

                                    //CALCULAR EXCEDENTE POR PESO
                                    decimal ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara71) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara71 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara61) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara61 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara51) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara51 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara41) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara41 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara31) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara31 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara21) * RegFrete.DeParaExcedente1);
                                    if (RegFrete.DePara21 == 0)
                                        ExcedentePorPeso = ((BaseCalculo - RegFrete.DePara11) * RegFrete.DeParaExcedente1);

                                    decimal FreteMinimo = RegFrete.DeParaPct17;
                                    if (RegFrete.DeParaPct17 == 0)
                                        FreteMinimo = RegFrete.DeParaPct16;
                                    if (RegFrete.DeParaPct16 == 0)
                                        FreteMinimo = RegFrete.DeParaPct15;
                                    if (RegFrete.DeParaPct15 == 0)
                                        FreteMinimo = RegFrete.DeParaPct14;
                                    if (RegFrete.DeParaPct14 == 0)
                                        FreteMinimo = RegFrete.DeParaPct13;
                                    if (RegFrete.DeParaPct13 == 0)
                                        FreteMinimo = RegFrete.DeParaPct12;
                                    if (RegFrete.DeParaPct12 == 0)
                                        FreteMinimo = RegFrete.DeParaPct11;

                                    if (ExcedentePorValor > 0 && ExcedentePorPeso > 0)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = (ExcedentePorValor + ExcedentePorPeso + ValorGRIS + ValorDiscriminacoes + FreteMinimo);
                                    else if (ExcedentePorPeso > 0)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ExcedentePorPeso + ValorGRIS + ValorDiscriminacoes + FreteMinimo;
                                    else if (ExcedentePorValor > 0)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ExcedentePorValor + ValorGRIS + ValorDiscriminacoes + FreteMinimo;
                                    else
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = "0";
                                }
                            }
                            else if (RegFrete.IndicadorTipoCalculo == 4)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "TARIFA SEGURO" && array[0].ToUpper() != "FRETE PESO (PES" && array[0].ToUpper() != "FRETE PESO/ PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }

                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = TotalValor * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }
                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }

                                if (BaseCalculo <= RegFrete.DePara12)
                                {
                                    //CALCULAR EXCEDENTE
                                    if (RegFrete.DeParaExcedente1 > 0 && BaseCalculo > RegFrete.DePara12)
                                    {
                                        ValorDiscriminacoes += RegFrete.DeParaExcedente1;
                                    }

                                    if (RegFrete.IndicadorCalcularAdValorDePara1 == 1)
                                    {
                                        //CALCULAR VALOR AD
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                    }

                                    ValorFrete = Convert.ToDecimal((TotalValor * (RegFrete.DeParaPct11 / 100)).ToString("F"));
                                    if (ValorFrete > RegFrete.ValorFreteMinimo)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + (TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes;
                                    else
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = RegFrete.ValorFreteMinimo + ((TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes);
                                }
                                else
                                {
                                    if (RegFrete.ValorPorTonelada > 0)
                                    {
                                        decimal ValorPorTonelada = ((Convert.ToDecimal(PesoCTe.Replace('.', ',')) - 100)) * RegFrete.ValorPorTonelada;
                                        ValorDiscriminacoes += ValorPorTonelada;
                                    }
                                    //CALCULAR EXCEDENTE
                                    if (RegFrete.DeParaExcedente1 > 0 && BaseCalculo > RegFrete.DePara12)
                                    {
                                        ValorDiscriminacoes += RegFrete.DeParaExcedente1;
                                    }

                                    if (RegFrete.IndicadorCalcularAdValorDePara1 == 1)
                                    {
                                        //CALCULAR VALOR AD
                                        if (RegFrete.ValorAD != 0)
                                            ValorDiscriminacoes += (TotalValor * (RegFrete.ValorAD / 100));
                                    }

                                    ValorFrete = Convert.ToDecimal((TotalValor * (RegFrete.DeParaPct11 / 100)).ToString("F"));
                                    if (ValorFrete > RegFrete.ValorFreteMinimo)
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorFrete + (TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes;
                                    else
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = RegFrete.ValorFreteMinimo + ((TotalValor * (RegFrete.ValorGRIS / 100)) + ValorDiscriminacoes);

                                }
                            }
                            else if (RegFrete.IndicadorTipoCalculo == 5)
                            {
                                ExisteRegraPorCNPJ = true;

                                //VERIFICA SE CALCULO SERÁ FEITO POR PESO OU CUBAGEM
                                if (CubagemBaseCalculo > TotalPeso)
                                    BaseCalculo = Convert.ToDecimal(CubagemBaseCalculo.ToString("F"));

                                //SOMAR DISCRIMINAÇÕES
                                decimal ValorDiscriminacoes = 0;
                                for (int valor = 0; valor < ListXComp.Count(); valor++)
                                {
                                    string[] array = ListXComp[valor].Split('³');

                                    if (array[0].ToUpper() != "FRETE VALOR" && array[0].ToUpper() != "FRETE PESO" && array[0].ToUpper() != "GRIS" && array[0].ToUpper() != "PEDAGIO" && array[0].ToUpper() != "TAXA" && array[0].ToUpper() != "TARIFA SEGURO" && array[0].ToUpper() != "FRETE PESO (PES" && array[0].ToUpper() != "FRETE PESO/ PES")
                                    {
                                        ValorDiscriminacoes += Convert.ToDecimal(array[1].Replace('.', ','));
                                    }

                                }

                                //CALCULAR VALOR SEGURO
                                if (RegFrete.ValorSeguro > 0)
                                {
                                    decimal CalculoSeguro = TotalValor * (RegFrete.ValorSeguro / 100);
                                    if (CalculoSeguro < RegFrete.ValorSeguroMinimo)
                                        CalculoSeguro = RegFrete.ValorSeguroMinimo;
                                    ValorDiscriminacoes += CalculoSeguro;
                                }
                                if (RegFrete.ValorPedagio > 0)
                                {
                                    decimal t = (Math.Ceiling(BaseCalculo / 100));
                                    decimal pedagio = (t * RegFrete.ValorPedagio);
                                    if (pedagio < RegFrete.ValorPedagio)
                                        pedagio += RegFrete.ValorPedagio;

                                    if (pedagio > RegFrete.ValorPedagioMaximo && RegFrete.ValorPedagioMaximo > 0)
                                        pedagio = RegFrete.ValorPedagioMaximo;
                                    ValorDiscriminacoes += pedagio;
                                }



                                //CALCULAR EXCEDENTE
                                if (RegFrete.DeParaExcedente1 > 0 && BaseCalculo > RegFrete.DePara12)
                                {
                                    ValorFrete += (BaseCalculo) * RegFrete.DeParaExcedente1;
                                }

                                if (RegFrete.ValorGRIS > 0)
                                {
                                    decimal ValorGris = ((TotalValor * (RegFrete.ValorGRIS / 100)));
                                    if (ValorGris < RegFrete.ValorGRISMinimo)
                                        ValorDiscriminacoes += RegFrete.ValorGRISMinimo;
                                    else
                                        ValorDiscriminacoes += ((TotalValor * (RegFrete.ValorGRIS / 100)));
                                }

                                if (RegFrete.ValorPorTonelada > 0)
                                {
                                    decimal ValorPorTonelada = (BaseCalculo / 1000) * RegFrete.ValorPorTonelada;
                                    ValorFrete += ValorPorTonelada;
                                }

                                if (RegFrete.IndicadorCalcularAdValorDePara1 == 1)
                                {
                                    //CALCULAR VALOR AD
                                    if (RegFrete.ValorAD != 0)
                                    {
                                        decimal ADvalor = (TotalValor * (RegFrete.ValorAD / 100));
                                        if (RegFrete.IndicadorCalcularAdValorDePara2 == 1)
                                            ValorFrete += ADvalor;
                                        else if (ADvalor > ValorFrete)
                                            ValorFrete = ADvalor;
                                    }
                                }
                                if (RegFrete.DeParaPct11 > 0)
                                    ValorFrete = Convert.ToDecimal((TotalValor * (RegFrete.DeParaPct11 / 100)).ToString("F"));

                                if (ValorFrete > RegFrete.ValorFreteMinimo)
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = (ValorFrete + ValorDiscriminacoes).ToString("F");
                                else
                                    ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = (RegFrete.ValorFreteMinimo + ValorDiscriminacoes).ToString("F");

                            }

                            if (ExisteRegraPorCNPJ == true)//IF O CNPJ TIVER NOS IF'S ACIMA
                            {
                                string cellCalculo = Convert.ToDecimal(ws.Cell("Y" + (linha - chNota.Count() + 1)).Value).ToString("F");
                                if (cellCalculo.Replace(',', '.') != ValorCTe)
                                {
                                    decimal Diferenca = Convert.ToDecimal(cellCalculo.Replace(',', '.')) - Convert.ToDecimal(ValorCTe);

                                    if (Diferenca <= Convert.ToDecimal(3) && Diferenca >= Convert.ToDecimal(-3))
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = ValorCTe;
                                    else
                                        ws.Cell("Y" + (linha - chNota.Count() + 1)).Style.Fill.BackgroundColor = XLColor.YellowProcess;
                                }
                            }
                            //-----------------------------------------------
                        }
                        else
                        {
                            ws.Cell("X" + (linha - chNota.Count() + 1)).Value = "Regra não encontrada";
                            ws.Cell("X" + (linha - chNota.Count() + 1)).Style.Fill.BackgroundColor = XLColor.YellowProcess;

                            ws.Cell("Y" + (linha - chNota.Count() + 1)).Value = "Regra não encontrada";
                            ws.Cell("Y" + (linha - chNota.Count() + 1)).Style.Fill.BackgroundColor = XLColor.YellowProcess;
                        }

                        //VERIFICA SE O PESO DO CTE É IGUAL AO PESO NAS NF - SE SIM, COLORIR DE VERMELHO
                        if (Convert.ToDecimal(PesoCTe.Replace('.', ',')).ToString("##,##00.00") != Convert.ToDecimal(TotalPesoNF).ToString("##,##00.00"))
                        {
                            if (chNota.Count() > 1)
                            {
                                ws.Range("P" + (linha - chNota.Count() + 1) + ":P" + (linha - 1)).Style.Fill.BackgroundColor = XLColor.FromArgb(250, 128, 114);
                                ws.Cell("L" + (linha - chNota.Count() + 1)).Style.Fill.BackgroundColor = XLColor.FromArgb(250, 128, 114);
                            }
                            else
                            {
                                ws.Cell("P" + linha).Style.Fill.BackgroundColor = XLColor.FromArgb(250, 128, 114);
                                ws.Cell("L" + linha).Style.Fill.BackgroundColor = XLColor.FromArgb(250, 128, 114);
                            }
                        }

                        //VERIFICA SE A CUBAGEM DO CTE E CUBAGEM DA NF TEM DIFERENCA MAIOR QUE 10%
                        double CalculoCubagem = Convert.ToDouble(CubagemCTe.Replace('.', ',')) * 0.10;
                        decimal DiferencaCubagem = (Convert.ToDecimal(CubagemCTe.Replace('.', ',')) - TotalCubagemNF);
                        if (DiferencaCubagem < 0)
                            DiferencaCubagem = DiferencaCubagem * -1;

                        if (DiferencaCubagem > Convert.ToDecimal(CalculoCubagem))
                        {
                            if (chNota.Count() > 1)
                            {
                                ws.Range("Q" + (linha - chNota.Count() + 1) + ":Q" + (linha - 1)).Style.Fill.BackgroundColor = XLColor.FromArgb(205, 92, 92);
                                ws.Cell("M" + (linha - chNota.Count() + 1)).Style.Fill.BackgroundColor = XLColor.FromArgb(205, 92, 92);
                            }
                            else
                            {
                                ws.Cell("M" + linha).Style.Fill.BackgroundColor = XLColor.FromArgb(205, 92, 92);
                                ws.Cell("Q" + linha).Style.Fill.BackgroundColor = XLColor.FromArgb(205, 92, 92);
                            }
                        }

                        char coluna = colunaExcel;
                        int ColunaSuperiorZLinha = 0;

                        for (int i = 0; i < ListXNome.Count(); i++)
                        {
                            // INTERAÇÕES SOBRE COLUNAS EXCEL DINAMICAS
                            string strColuna = "";
                            if (coluna == 'Z')
                            {
                                coluna = 'A';
                                strColuna = "A" + coluna;
                                ColunaSuperiorZLinha++;
                            }
                            else
                            {
                                coluna = Convert.ToChar(coluna + 1);
                                strColuna = coluna.ToString();

                                if (ColunaSuperiorZLinha == 0)
                                    strColuna = strColuna.ToString();
                                else
                                    strColuna = "A" + strColuna.ToString();
                            }

                            //MONTA AS COLUNAS DE DISCRIMINIAÇÃO DO CTE
                            int contador = 0;
                            string r = ListXNome[i];

                            for (int o = 0; o < ListXComp.Count(); o++)
                            {
                                if (contador == 0)
                                {
                                    string[] array = ListXComp[o].Split('³');
                                    if (array[0] == ListXNome[i])
                                    {

                                        ws.Cell(Convert.ToString(strColuna) + (linha - chNota.Count() + 1)).Value = array[1];

                                        ws.Range(Convert.ToString(strColuna) + (linha - chNota.Count() + 1) + ":" + Convert.ToString(strColuna) + (linha - 1)).Merge();
                                        ws.Column(Convert.ToString(strColuna)).Style.NumberFormat.Format = "R$ #,##0.00";
                                        contador++;
                                    }
                                    else
                                    {
                                        ws.Cell(Convert.ToString(strColuna) + (linha - chNota.Count() + 1)).Value = "  ";
                                        ws.Range(Convert.ToString(strColuna) + (linha - chNota.Count() + 1) + ":" + Convert.ToString(strColuna) + (linha - 1)).Merge();
                                        ws.Column(Convert.ToString(strColuna)).Style.NumberFormat.Format = "R$ #,##0.00";
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        ws.Cell("N" + linha).Value = "Este CT-e não contém NF-e(s)";
                        linha++;
                    }

                    //SALVANDO DOCUMENTO NO BANCO DE DADOS
                    UploadDocumentoEletronico up = new UploadDocumentoEletronico();
                    UploadDocumentoEletronicoDAL upDAL = new UploadDocumentoEletronicoDAL();
                        
                    up.ChaveAcesso = ChaveCTe;

                    if (upDAL.PesquisarUploadDocumentoEletronico(up.ChaveAcesso) == false)
                    {
                        if (strBaseCalculoICMS != "" && strPercentualICMS != "")
                        {
                            up.ValorBaseCalculoICMS = Convert.ToDecimal(strBaseCalculoICMS.Replace('.', ','));
                            up.ValorICMS = Convert.ToDecimal(strBaseCalculoICMS.Replace('.', ',')) * (Convert.ToDecimal(strPercentualICMS.Replace('.', ',')) / 100);
                        }
                        if (strBaseCalculoICMS != "" && ValorICMS != "")
                        {
                            up.ValorBaseCalculoICMS = Convert.ToDecimal(strBaseCalculoICMS.Replace('.', ','));
                            up.ValorICMS = Convert.ToDecimal(ValorICMS.Replace('.', ','));
                        }
                        else
                        {
                            up.ValorBaseCalculoICMS = 0;
                            up.ValorICMS = 0;
                        }


                        up.ArquivoXML = ArquivoXML;
                        up.CodigoTipoDocumento = 7;
                        upDAL.Inserir(up);
                    }
                    //--------------------------------------
                    
                }
                for (int i = 5; i < linha; i++)
                {
                    ws.Cell(GetColNameFromIndex(26 + ListXNome.Count) + i).FormulaA1 = "SUM(Z" + i + ":" + GetColNameFromIndex(25 + ListXNome.Count) + i + ")";
                    ws.Cell(GetColNameFromIndex(26 + ListXNome.Count) + i).Style.Fill.BackgroundColor = XLColor.FromArgb(221, 217, 195);
                }

                //config de cor, alinhamento, etc.
                int ColunaSuperiorZ = 0;
                for (int i = 0; i < ListXNome.Count(); i++)
                {
                    string strColuna = "";
                    if (colunaExcel.ToString() == "Z")
                    {
                        colunaExcel = 'A';
                        strColuna = "A" + colunaExcel;
                        strColunaExcel = strColuna;
                        ColunaSuperiorZ++;
                    }
                    else
                    {
                        colunaExcel = Convert.ToChar(colunaExcel + 1);
                        strColuna = colunaExcel.ToString();

                        if (ColunaSuperiorZ == 0)
                            strColunaExcel = colunaExcel.ToString();
                        else
                            strColunaExcel = "A" + colunaExcel.ToString();
                    }
                    
                    if (ListXNome[i].ToString() == "IMP REPASSADO")
                        ws.Cell(Convert.ToString(strColunaExcel) + 4).Value = "ICMS";
                    else
                        ws.Cell(Convert.ToString(strColunaExcel) + 4).Value = ListXNome[i];

                    ws.Cell(Convert.ToString(strColunaExcel) + 4).Style.Fill.BackgroundColor = XLColor.ForestGreen;
                    ColunasExtras++;
                }

                string ColunaTotal = GetColNameFromIndex(26 + ListXNome.Count);

                ws.Cell(ColunaTotal +"4").Value = "TOTAL";
                ws.Cell(ColunaTotal +"4").Style.Fill.BackgroundColor = XLColor.ForestGreen;
                ws.Column(ColunaTotal).Style.NumberFormat.Format = "R$ #,##0.00";

                
                ws.Cell("Z3").Value = "Discriminação do valor do CT-e";
                ws.Range("Z3:"+ ColunaTotal + "3").Merge();

                ws.Columns("B-"+ GetColNameFromIndex(27 + ListXNome.Count)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Columns("B-"+ GetColNameFromIndex(27 + ListXNome.Count)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int rowSUM = linha + 3;
                linha--;
                ws.Range("J" + rowSUM + ":Y" + rowSUM).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("J" + rowSUM + ":Y" + rowSUM).Style.Border.InsideBorderColor = XLColor.Black;
                 
                ws.Range("J" + rowSUM + ":Y" + rowSUM).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("J" + rowSUM + ":Y" + rowSUM).Style.Border.OutsideBorderColor = XLColor.Black;

                //TOTAIS DAS COLUNAS
                ws.Cell("J" + rowSUM).Value = "Totais: ";
                ws.Cell("J" + rowSUM).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Cell("J" + rowSUM).Style.Fill.BackgroundColor = XLColor.GreenRyb;
                
                ws.Cell("K" + rowSUM).FormulaA1 = "SUM(K5:K" + linha + ")";
                ws.Cell("L" + rowSUM).FormulaA1 = "SUM(L5:L" + linha + ")";
                ws.Cell("M" + rowSUM).FormulaA1 = "SUM(M5:M" + linha + ")";
                ws.Cell("N" + rowSUM).FormulaA1 = "SUM(N5:N" + linha + ")";
                ws.Cell("O" + rowSUM).FormulaA1 = "SUM(O5:O" + linha + ")";
                ws.Cell("P" + rowSUM).FormulaA1 = "SUM(P5:P" + linha + ")";
                ws.Cell("Q" + rowSUM).FormulaA1 = "SUM(Q5:Q" + linha + ")";
                ws.Cell("S" + rowSUM).FormulaA1 = "SUM(S5:S" + linha + ")";
                ws.Cell("T" + rowSUM).FormulaA1 = "SUM(T5:T" + linha + ")";
                ws.Cell("R" + rowSUM).FormulaA1 = "SUM(R5:R" + linha + ")";
                ws.Cell("U" + rowSUM).FormulaA1 = "SUM(U5:U" + linha + ")";
                ws.Cell("V" + rowSUM).FormulaA1 = "SUM(V5:V" + linha + ")";
                ws.Cell("X" + rowSUM).FormulaA1 = "SUM(X5:X" + linha + ")";

                char col = 'X';
                int contadorColuna = 0;
                for (int i = 0; i < ColunasExtras + 2; i++)
                {
                    if (col.ToString() == "Z")
                    {
                        col = 'A';
                        ws.Cell("A" + col.ToString() + rowSUM).FormulaA1 = "SUM(A" + col.ToString() + "5:A" + col.ToString() + linha + ")";
                        ws.Cell("A" + col.ToString() + rowSUM).Style.Border.OutsideBorderColor = XLColor.Black;
                        ws.Cell("A" + col.ToString() + rowSUM).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell("A" + col.ToString() + rowSUM).Style.Border.InsideBorderColor = XLColor.Black;
                        ws.Cell("A" + col.ToString() + rowSUM).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        contadorColuna++;
                    }
                    else
                    {
                        col = Convert.ToChar(col + 1);
                        if (contadorColuna == 0)
                        {
                            ws.Cell(col.ToString() + rowSUM).FormulaA1 = "SUM(" + col.ToString() + "5:" + col.ToString() + linha + ")";
                            ws.Cell(col.ToString() + rowSUM).Style.Border.OutsideBorderColor = XLColor.Black;
                            ws.Cell(col.ToString() + rowSUM).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            ws.Cell(col.ToString() + rowSUM).Style.Border.InsideBorderColor = XLColor.Black;
                            ws.Cell(col.ToString() + rowSUM).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                        else
                        {
                            ws.Cell("A" + col.ToString() + rowSUM).FormulaA1 = "SUM(A" + col.ToString() + "5:A" + col.ToString() + linha + ")";
                            ws.Cell("A" + col.ToString() + rowSUM).Style.Border.OutsideBorderColor = XLColor.Black;
                            ws.Cell("A" + col.ToString() + rowSUM).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            ws.Cell("A" + col.ToString() + rowSUM).Style.Border.InsideBorderColor = XLColor.Black;
                            ws.Cell("A" + col.ToString() + rowSUM).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                    }
                }

                //DESIGN DAS COLUNAS E LINHAS
                ws.Range("B3:" + ColunaTotal + "3").Style.Fill.BackgroundColor = XLColor.GreenRyb;
                ws.Range("C4:H4").Style.Fill.BackgroundColor = XLColor.ForestGreen;

                ws.Range("B4:" + ColunaTotal + "4").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("B4:" + ColunaTotal + "4").Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("B3:"+ ColunaTotal + "3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("B3:" + ColunaTotal + "3").Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("B4:" + ColunaTotal + linha).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("B4:" + ColunaTotal + linha).Style.Border.OutsideBorderColor = XLColor.Black;

                ws.Range("B4:"+ ColunaTotal + "4").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("B4:"+ ColunaTotal + "4").Style.Border.InsideBorderColor = XLColor.Black;

                ws.Range("B3:"+ ColunaTotal + "3").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("B3:"+ ColunaTotal + "3").Style.Border.InsideBorderColor = XLColor.Black;

                ws.Range("B4:"+ ColunaTotal + linha).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("B4:"+ ColunaTotal + linha).Style.Border.InsideBorderColor = XLColor.Black;
                
                ws.Columns("B-" + GetColNameFromIndex(27 + ListXNome.Count)).Style.Alignment.WrapText = true;
                ws.Columns("O-" + GetColNameFromIndex(27 + ListXNome.Count)).Width = 13;

                ws.Column("U").Width = 20;
                ws.Column("A").Width = 0;
                ws.Column(GetColNameFromIndex(26 + ListXNome.Count)).Width = 15;

                ws.Columns("J-M").Width = 13;
                ws.Columns("I-I").Width = 20;

                //copiar Cabeçalho no rodapé
                var firstTableCell = ws.Cell("B3");
                var lastTableCell = ws.Cell(GetColNameFromIndex(27 + ListXNome.Count) + "4");
                var rngData = ws.Range(firstTableCell.Address, lastTableCell.Address);
                ws.Cell("B" + (linha + 1)).Value = rngData;

                //titulo do relatorio
                ws.Cell("B2").Value = "Relatório de CT-e";
                ws.Cell("B2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                var range = ws.Range("B2:"+ GetColNameFromIndex(27 + ListXNome.Count) + "2");
                range.Merge().Style.Font.SetBold().Font.FontSize = 20;

                //salvar arquivo em disco
                wb.SaveAs(CaminhoArquivo + @"\" + NomeArquivo);
                wb.Dispose();

                //LOG DE QUEM TIROU O RELATORIO
                log.GerandoArquivoLog("Planilha de fretes gerada com sucesso por usuário " + Session["UsuSis"] + " / " + Session["CodUsuario"]);
                return true;
            }
            catch (Exception ex)
            {               
                log.GerandoArquivoLog("Linha " +ContadorLinhaErro +": Erro ao gerar planilha de fretes - " + ex.Message + " - tentativa do usuário " + Session["UsuSis"] + " / " + Session["CodUsuario"]);
                ShowMessage(ex.Message, MessageType.Error);
                strErro = ex.Message;
                return false;
            }
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
        protected void btnAdd_Click(string CaminhoArquivo, Stream arquivo)
        {
            try
            {
                BinaryReader br = new BinaryReader(arquivo);

                byte[] ArquivoByte = br.ReadBytes((int)arquivo.Length);
                arquivo.Close();
                string NomeFile = CaminhoArquivo;
                string Extencao = Path.GetExtension(NomeFile);

                if (Extencao.ToLower() != ".xml")
                {
                    return;
                }

                int max = Convert.ToInt32(ListaArquivos.AsEnumerable().Max(row => row[0]));

                string[] arrayCaminho = CaminhoArquivo.Split('\\');
                NomeFile = arrayCaminho[arrayCaminho.Count() - 1];

                ListaArquivos.Rows.Add(max + 1, NomeFile, ArquivoByte);

                grdXMLs.DataSource = ListaArquivos;
                grdXMLs.DataBind();
                Session["ListaArquivos"] = ListaArquivos;
                BtnLerXML.Visible = true;

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Info);
            }
        }

        protected void grdXMLs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
                string CodigoItem = HttpUtility.HtmlDecode(grdXMLs.SelectedRow.Cells[1].Text);
                List<DataRow> rows = new List<DataRow>();
                foreach (DataRow dr in ListaArquivos.Rows)
                {
                    if (dr["#"].ToString() == CodigoItem)
                    {
                        rows.Add(dr);
                    }
                }
                foreach (DataRow item in rows)
                {
                    ListaArquivos.Rows.Remove(item);
                }

                grdXMLs.DataSource = ListaArquivos;
                grdXMLs.DataBind();
                Session["ListaArquivos"] = ListaArquivos;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            Session["ListaArquivos"] = null;
            ListaArquivos.Clear();
            grdXMLs.DataSource = ListaArquivos;
            grdXMLs.DataBind();

            BtnLerXML.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var Arquivos = fileselector.PostedFiles;

                foreach (var file in Arquivos)
                {
                    if (file.InputStream.Length > 0)
                        btnAdd_Click(file.FileName, file.InputStream);
                }
            }catch(Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
    }
}

