using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NFeX;
using NFeDataSetX;
using DAL.TecnoSpeed.Persistence;
using DAL.TecnoSpeed;
using DAL.Model;
using System.Xml;

namespace DAL.Persistence
{
    public class NFeFuncoes
    {
        public NFeX.spdNFeX spdNFe = new NFeX.spdNFeX();
        public NFeDataSetX.spdNFeDataSetX dt = new spdNFeDataSetX();
        public NFeX.spdNFeSCAN spdSCAN = new NFeX.spdNFeSCAN();
        public NFeX.spdNFeDPECX spdDPEC = new spdNFeDPECX();

        private static string LerTagXML(string pTexto, string pTag, int pTamanho)
        {
            string vReturn = "";
            int pIni = 0;

            pIni = (pTexto.IndexOf(pTag, 10) + (pTag.Length + 1));
            if (pIni > 0)
            {
                vReturn = pTexto.Substring(pIni, pTamanho);
            }

            return vReturn;
        }

        public bool ConfigurandoComponente(string strCNPJ, decimal decUF)
        {
            try
            {

                Estado est = new Estado();
                EstadoDAL estDAL = new EstadoDAL();
                est = estDAL.PesquisarEstado(Convert.ToInt32(decUF));

                string strVersaoManual = "5.0", strCertificadoDigital = "";

                spdNFe.ConfigurarSoftwareHouse("02048638000128", "");

                string[] strCertificados = spdNFe.ListarCertificados("|").Split('|');
                foreach (string certificado in strCertificados)
                {
                    string[] ArrayCertificado = certificado.Split(':');
                    if (ArrayCertificado.Length >= 2)
                    {
                        string[] ArrayCertificado2 = ArrayCertificado[1].Split(',');

                        foreach (string cert in ArrayCertificado2)
                        {
                            string Cnpj = cert;
                            if (Cnpj == strCNPJ)
                            {
                                strCertificadoDigital = certificado;
                            }
                        }
                    }
                }
                if (strCertificadoDigital != "")
                {
                    string CaminhoArquivo = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\..\\..\\..\\..\\Modulos\\TecnoSpeed\\NFe\\Arquivos";
                    
                    spdNFe.UF = est.Sigla.Substring(0, 2);
                    spdNFe.CNPJ = strCNPJ;
                    spdNFe.NomeCertificado = strCertificadoDigital;
                    spdNFe.ArquivoServidoresHom = CaminhoArquivo + @"\nfeServidoresHom.ini";
                    spdNFe.ArquivoServidoresProd = CaminhoArquivo + @"\nfeServidoresProd.ini";
                    spdNFe.DiretorioEsquemas = CaminhoArquivo + @"\Esquemas\";
                    spdNFe.DiretorioTemplates = CaminhoArquivo + @"\Templates\";
                    spdNFe.DiretorioLog = CaminhoArquivo + @"\Log\";
                    spdNFe.DiretorioXmlDestinatario = CaminhoArquivo + @"\XML_Destinatario";
                    spdNFe.TipoCertificado = TipoCertificado.ckFile;
                    spdNFe.VersaoManual = strVersaoManual;
                    spdNFe.Ambiente = Ambiente.akHomologacao;
                    spdNFe.ModoOperacao = ModoOperacaoNFe.moNormal;
                    spdNFe.HttpLibs = "wininet,sbb";
                    spdNFe.ValidarEsquemaAntesEnvio = true;

                    //[SCAN]
                    spdSCAN.UF = est.Sigla.Substring(0, 2);
                    spdSCAN.CNPJ = strCNPJ;
                    spdSCAN.ArquivoServidoresHom = "nfeServidoresHomSCAN.ini";
                    spdSCAN.DiretorioEsquemas = CaminhoArquivo + @"\Esquemas\";
                    spdSCAN.DiretorioTemplates = CaminhoArquivo + @"\Templates\";
                    spdSCAN.DiretorioLog = CaminhoArquivo + @"\Log\";
                    spdSCAN.TipoCertificado = TipoCertificado.ckFile;
                    spdSCAN.NomeCertificado = strCertificadoDigital;
                    spdSCAN.VersaoManual = strVersaoManual;

                    //[DPEC]
                    spdDPEC.UF = est.Sigla.Substring(0, 2);
                    spdDPEC.CNPJ = strCNPJ;
                    spdDPEC.ArquivoServidoresHom = "nfeServidoresHomDPEC.ini";
                    spdDPEC.DiretorioEsquemas = CaminhoArquivo + @"\Esquemas\";
                    spdDPEC.DiretorioTemplates = CaminhoArquivo + @"\Templates\";
                    spdDPEC.DiretorioLog = CaminhoArquivo + @"\Log\";
                    spdDPEC.TipoCertificado = TipoCertificado.ckFile;
                    spdDPEC.NomeCertificado = "";
                    spdDPEC.VersaoManual = strVersaoManual;

                    //[MAIL]
                    spdNFe.EmailServidor = "smtp.gmail.com";
                    spdNFe.EmailRemetente = "testedanfe@gmail.com";
                    spdNFe.EmailDestinatario = "testedanfe@gmail.com";
                    spdNFe.EmailAssunto = "Exemplo de envio de DANFE por email";
                    spdNFe.EmailMensagem = "O arquivo está anexo.";
                    spdNFe.Usuario = "testedanfe@gmail.com";
                    spdNFe.Senha = "123teste";
                    spdNFe.EmailAutenticacao = true;
                    spdNFe.EmailPorta = 587;


                    //[DANFE]
                    spdNFe.LogotipoEmitente = "";
                    spdNFe.FraseContingencia = "";
                    spdNFe.FraseHomologacao = "";
                    spdNFe.ModeloRetrato = "";
                    spdNFe.ModeloPaisagem = "";
                    spdNFe.QtdeCopias = 1;

                    return true;
                }
                else
                {
                    GerandoArquivoLog("Certificado digital não encontrado para CNPJ " + strCNPJ, 1);
                    return false;
                }
            }
            catch(Exception ex)
            {
                GerandoArquivoLog("Erro ao configurar componente..." +strCNPJ +" - "+ ex.Message, 1);
                return false;
            }
        }

        public void EnviarNFe()
        {
            try
            {
                List<TECNO_NF> ListaNF = new List<TECNO_NF>();

                TECNO_NF_DAL nfDAL = new TECNO_NF_DAL();

                ListaNF = nfDAL.ListarTECNO_NF();

                foreach (var nf in ListaNF)
                {
                    string XML = "", XMLAssinado = "", XMLRetornoEnvio = "", NumeroRecibo = "", StatusRetorno = "", MotivoRetorno = "" ;

                    GerandoArquivoLog("Configurando componente...", 1);

                    if (ConfigurandoComponente(nf.EMIT_CNPJ, nf.IDE_CUF))
                    {

                        GerandoArquivoLog("Componente configuradao... Preparando para gerar XML da Nota Fiscal ", 1);

                        bool blnGerouXML = false;

                        XML = MontarXML(nf, ref blnGerouXML);

                        if (blnGerouXML)
                        {    
                            GerandoArquivoLog("Gerou XML da nota fiscal com sucesso... Preparando para assinar XML", 1);

                            XMLAssinado = spdNFe.AssinarNota(XML);
                             
                            GerandoArquivoLog("XML assinado com sucesso, preparando para enviar NFe", 1);

                            XMLRetornoEnvio = spdNFe.EnviarNF("1", XMLAssinado, false);

                            GerandoArquivoLog("NFe enviada, verificando status...", 1);

                            StatusRetorno = LerTagXML(XMLRetornoEnvio, "<cStat", 3);
                            
                            if (StatusRetorno == "100")
                            {
                                GerandoArquivoLog("NFe Autorizada com sucesso... Consultando recibo", 1);

                                NumeroRecibo = LerTagXML(XMLRetornoEnvio, "<nRec", 15);

                                GerandoArquivoLog("Recibo Verificado, consultando número de recibo", 1);

                                ConsultarRecibo(NumeroRecibo);
                            }
                            else if (StatusRetorno == "105")
                            {
                                GerandoArquivoLog("NFe enviada, lote ainda em processamento... preparando uma consulta", 1);
                                // LOTE EM PROCESSAMENTO
                            }
                            else
                            {
                                MotivoRetorno = LerTagXML(XMLRetornoEnvio, "<xMotivo", 15);

                                GerandoArquivoLog("NFe rejeitada, motivo... " + MotivoRetorno, 1);
                            }
                        }
                        else
                        {
                            GerandoArquivoLog("Erro ao gerar XML de NF-e - " + XML, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro no envio de NF-e - " + ex.Message, 1);
            }
        }

        private string ConsultarRecibo(string strNumeroRecibo)
        {
            if (strNumeroRecibo != "")
                return LerTagXML(spdNFe.ConsultarRecibo(strNumeroRecibo), "<nProt", 20);
            else
                return "";
        }

        private string ConsultarNFe(string strLoteNotas)
        {
            if (strLoteNotas != "")
            {
                return LerTagXML(spdNFe.ConsultarRecibo(strLoteNotas), "<nProt", 20);
            }
            else
                return "";
        }

        private string PreverDanfe(string strLoteNotas)
        {
            if (strLoteNotas != "")
            {
                return spdNFe.PreverDanfe(strLoteNotas, "");
            }
            else
                return "";
        }

        private string EditarDanfe(string strLoteNotas)
        {
            if (strLoteNotas != "")
            {
                return spdNFe.EditarModeloDanfe("1", strLoteNotas, "");
            }
            else
            {
                return "";
            }
        }

        private string VisualizarDanfe(string strLoteNotas)
        {
            if (strLoteNotas != "")
            {
                return spdNFe.VisualizarDanfe("1", strLoteNotas, "");
            }
            else
                return "";
        }

        private string ImprimirDanfe(string strLoteNotas)
        {
            if (strLoteNotas != "")
            {
                return spdNFe.ImprimirDanfe("1", strLoteNotas, "", "");
            }
            else
                return "";
        }

        private string ExportarPDF(string strLoteNotas)
        {
            if (strLoteNotas != "")
            {
                return spdNFe.ExportarDanfe("1", strLoteNotas, "", 1, "");
            }
            else
                return "";
        }

        private string EnviarEmail(string strAldNf)
        {
            if (strAldNf != "")
            {
                return spdNFe.EnviarNotaDestinatario(strAldNf, "", "");
            }
            else
                return "";
        }

        public void GerandoInsertDoDicionarioTecnoSpeed()
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\vinic\Desktop\teste.txt");
            System.IO.TextWriter arquivo = System.IO.File.AppendText(@"C:\Users\vinic\Desktop\teste2.txt");
            string[] t = text.Split('³');
            string INSERT = "";
            for (int i = 0; i < t.Length; i++)
            {
                string n = t[i];
                if (n != "")
                {
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(n);
                        string  nome="", tipo="",tamanho="",requerido="",display="",dica="",enum1="";
                        XmlElement root = doc.DocumentElement;
                        if (root.Attributes["nome"] != null)
                            nome =  root.Attributes["nome"].Value;
                        if (root.Attributes["tipo"] != null)
                            tipo = root.Attributes["tipo"].Value;
                        if (root.Attributes["tamanho"] != null)
                            tamanho = root.Attributes["tamanho"].Value;
                        if (root.Attributes["requerido"] != null)
                            requerido = root.Attributes["requerido"].Value;
                        if (root.Attributes["display"] != null)
                            display = root.Attributes["display"].Value;
                        if (root.Attributes["dica"] != null)
                            dica = root.Attributes["dica"].Value;
                        if (root.Attributes["enum"] != null)
                            enum1 = root.Attributes["enum"].Value;

                        arquivo.WriteLine("INSERT INTO DICIONARIO_NF_TECNOSPEED VALUES('" +nome+"','"+tipo+"','"+tamanho+"','"+requerido+"','"+display+"','"+dica+"','"+enum1 + "');");
                       
                    }
                    catch
                    {

                    }
                }
            }
            arquivo.Close();
            Console.WriteLine(INSERT);
        }

        public string MontarXML(TECNO_NF nf, ref bool blnGerouXML)
        {
            try
            {
                if (nf != null)
                {
                    spdNFe.VersaoManual = "6.0";
                    dt.VersaoEsquema = "pl_009";
                    dt.DiretorioTemplates = spdNFe.DiretorioTemplates + "Conversor\\NFeDataSets.xml";
                    dt.DicionarioXML = spdNFe.DiretorioTemplates + "Conversor\\NFeDataSets.xml";
                    dt.Incluir();

                    
                    dt.SetCampo("versao_A02=4.00");//Versão do leiaute	
                    //dt.SetCampo("id_A03=");//Chave da NFe a ser assinada	
                    dt.SetCampo("cUF_B02=" + nf.IDE_CUF);//Código da UF do emitente do Documento Fiscal	
                    dt.SetCampo("cNF_B03=" + nf.IDE_CNF);//Código Numérico que compõe a Chave de Acesso	
                    dt.SetCampo("natOp_B04=" + nf.IDE_NATOP);//Descrição da Natureza da Operação	
                    dt.SetCampo("mod_B06=" + nf.IDE_MOD);//Código do Modelo do Documento Fiscal	
                    dt.SetCampo("serie_B07=" + nf.IDE_SERIE);//Série do Documento Fiscal	
                    dt.SetCampo("nNF_B08=" + nf.IDE_NNF);//Número do documento fiscal	
                    dt.SetCampo("dhEmi_B09=" + nf.IDE_DEMI.ToString("yyyy-MM-ddThh:mm:ss") + "-03:00");//Data e hora de emissão do Documento Fiscal	
                    dt.SetCampo("dhSaiEnt_B10=" + nf.IDE_DSAIENT.ToString("yyyy-MM-ddThh:mm:ss") + "-03:00");//Data e hora de Saída ou da Entrada da Mercadoria/Produto	
                    dt.SetCampo("tpNF_B11=" + nf.IDE_TPNF);//Tipo de Operação	
                    dt.SetCampo("idDest_B11a=" + nf.IDE_DEST);//Identificador de local de destino da operação	
                    dt.SetCampo("cMunFG_B12=" + nf.IDE_CMUNFG);//Código do Município de Ocorrência do Fato Gerador
                    dt.SetCampo("tpImp_B21=" + nf.IDE_TPIMP);//Formato de Impressão do DANFE	
                    dt.SetCampo("tpEmis_B22=" + nf.IDE_TPEMIS);//Tipo de Emissão da NF-e	
                    //dt.SetCampo("cDV_B23=" );//Dígito Verificador da Chave de Acesso da NF-e	

                    if (spdNFe.Ambiente == Ambiente.akProducao)
                        dt.SetCampo("tpAmb_B24=1");//Identificação do Ambiente	
                    else
                        dt.SetCampo("tpAmb_B24=2");//Identificação do Ambiente	

                    dt.SetCampo("finNFe_B25=" + nf.IDE_FINNFE);//Versão do Processo de emissão da NF-e	
                    dt.SetCampo("indFinal_B25a=" + nf.IDE_INDFINAL);//Indica operação com Consumidor final	
                    dt.SetCampo("indPres_B25b=" + nf.IDE_INDPRES);//Indicador de presença do comprador no estabelecimento comercial no momento da operação	

                    if(nf.IDE_INDINTERMED != null)
                        dt.SetCampo("indIntermed_B25c=" + nf.IDE_INDINTERMED);//Indicador de intermediador/marketplace	

                    dt.SetCampo("procEmi_B26=" + nf.IDE_PROCEMI);//	Processo de emissão da NF-e
                    dt.SetCampo("verProc_B27=");//	Versão do Processo de emissão da NF-e

                    //Sequência XML	
                    dt.SetCampo("dhCont_B28=" + nf.IDE_DHCONT);//Data e Hora da entrada em contingência
                    dt.SetCampo("xJust_B29=" + nf.IDE_XJUST);//Justificativa da entrada em contingência
                    //Fim Sequência XML

                    //Informação de Documentos Fiscais referenciados
                    List<TECNO_NF_REFERENCIA> ListaNf_ref = new List<TECNO_NF_REFERENCIA>();
                    TECNO_NF_REFERENCIA_DAL nf_refDAL = new TECNO_NF_REFERENCIA_DAL();
                    ListaNf_ref = nf_refDAL.ListarTECNO_NF_REFERENCIA(nf.ID_NOTA_FISCAL);
                    foreach (var nf_ref in ListaNf_ref)
                    {
                        //Informação da NF-e referenciada
                        dt.IncluirParte("NREF");
                        dt.SetCampo("refNFe_BA02=" + nf_ref.NFREF_REFNFE);//Chave de acesso da NF-e referenciada	
                        dt.SalvarParte("NREF");

                        //Informação da NF modelo 1/1A ou NF modelo 2 referenciada
                        dt.IncluirParte("NREF");
                        dt.SetCampo("cUF_BA04=" + nf_ref.REFNF_CUF);//Código da UF do emitente	
                        dt.SetCampo("AAMM_BA05=" + nf_ref.REFNF_AAMM);//Ano e Mês de emissão da NF-e	
                        dt.SetCampo("CNPJ_BA06=" + nf_ref.REFNF_CNPJ);//CNPJ do emitente	
                        dt.SetCampo("mod_BA07=" + nf_ref.REFNF_MOD);//Modelo do Documento Fiscal	
                        dt.SetCampo("serie_BA08=" + nf_ref.REFNF_SERIE);//Série do Documento Fiscal	
                        dt.SetCampo("nNF_BA09=" + nf_ref.REFNF_NNF);//Número do Documento Fiscal
                        dt.SalvarParte("NREF");

                    }

                    //Informações da NF de produtor rural referenciada
                    //dt.IncluirParte("NREF");
                    //dt.SetCampo("cUF_BA11");//Código da UF do emitente	
                    //dt.SetCampo("AAMM_BA12");//Ano e Mês de emissão da NF-e	
                    //dt.SetCampo("CNPJ_BA13");//CNPJ do emitente	
                    //dt.SetCampo("CPF_BA14");//CPF do emitente	
                    //dt.SetCampo("IE_BA15");//IE do emitente	
                    //dt.SetCampo("mod_BA16");//Modelo do Documento Fiscal	
                    //dt.SetCampo("serie_BA17");//Série do Documento Fiscal	
                    //dt.SetCampo("nNF_BA18");//Número do Documento Fiscal	
                    //dt.SetCampo("refCTe_BA19");//Chave de acesso do CT-e referenciada	
                    //dt.SalvarParte("NREF");

                    //Informações do Cupom Fiscal referenciado	
                    //dt.IncluirParte("NREF");
                    //dt.SetCampo("mod_BA21");//Modelo do Documento Fiscal	
                    //dt.SetCampo("nECF_BA22");//Número de ordem Sequência l do ECF	
                    //dt.SetCampo("nCOO_BA23");//Número do Contador de Ordem de Operação - COO	
                    //dt.SalvarParte("NREF");

                    //Identificação do emitente da NF-e
                    if (nf.DEST_CNPJ_CPF.Length > 11)
                        dt.SetCampo("CNPJ_C02=" + nf.DEST_CNPJ_CPF);//CNPJ do emitente	
                    else
                        dt.SetCampo("CPF_C02a=" + nf.DEST_CNPJ_CPF);//CPF do emitente

                    dt.SetCampo("xNome_C03=" + nf.EMIT_XNOME);//Razão social ou nome do emitente	
                    dt.SetCampo("xFant_C04=" + nf.EMIT_XFANT);//Nome fantasia	
                    dt.SetCampo("xLgr_C06=" + nf.ENDEREMIT_XLOG); //Logradouro
                    dt.SetCampo("nro_C07=" + nf.ENDEREMIT_NRO);//Número
                    dt.SetCampo("xCpl_C08=" + nf.ENDEREMIT_XCPL);//Complemento	
                    dt.SetCampo("xBairro_C09=" + nf.ENDEREMIT_XBAIRRO);//Bairro
                    dt.SetCampo("cMun_C10=" + nf.ENDEREMIT_CMUN);//Código do município	
                    dt.SetCampo("xMun_C11=" + nf.ENDEREMIT_XMUN);//Nome do município	
                    dt.SetCampo("UF_C12=" + nf.ENDEREMIT_UF);//Sigla da UF	
                    dt.SetCampo("CEP_C13=" + nf.ENDEREMIT_CEP);//Código do CEP	
                    dt.SetCampo("cPais_C14=" + nf.ENDEREMIT_CPAIS);//Código do País	
                    dt.SetCampo("xPais_C15=" + nf.ENDEREMIT_XPAIS);//Nome do País	
                    dt.SetCampo("fone_C16=" + nf.ENDEREMIT_FONE);//Telefone
                    dt.SetCampo("IE_C17=" + nf.EMIT_IE);//Inscrição Estadual do Emitente	
                    dt.SetCampo("IEST_C18=" + nf.EMIT_IEST);//IE do Substituto Tributário	

                    //Sequência XML
                    dt.SetCampo("IM_C19=" + nf.EMIT_IM);//Inscrição Municipal do Prestador de Serviço	
                    dt.SetCampo("CNAE_C20=" + nf.EMIT_CNAE);//CNAE Fiscal	
                                                            //Fim Sequência XML

                    dt.SetCampo("CRT_C21=" + nf.IDE_CRT);//Código do Regime Tribtário	

                    //Identificação do Destinatário da NF-e
                    if (nf.DEST_CNPJ_CPF.Length > 11)
                        dt.SetCampo("CNPJ_E02=" + nf.DEST_CNPJ_CPF);//CNPJ do destinatário	
                    else
                        dt.SetCampo("CPF_E03=" + nf.DEST_CNPJ_CPF);//CPF do destinatário	

                    dt.SetCampo("idEstrangeiro_E03a=" + nf.DEST_IDESTRANG);//Identificação do destinatário no caso de comprador estrangeiro	
                    dt.SetCampo("xNome_E04=" + nf.DEST_XNOME);//Razão social ou nome do destinatário	
                    dt.SetCampo("xLgr_E06=" + nf.ENDERDEST_XLGR);//Logradouro
                    dt.SetCampo("nro_E07=" + nf.ENDERDEST_NRO);//Número
                    dt.SetCampo("xCpl_E08=" + nf.ENDERDEST_XCPL);//Complemento
                    dt.SetCampo("xBairro_E09=" + nf.ENDERDEST_XBAIRRO);//Bairro
                    dt.SetCampo("cMun_E10=" + nf.ENDERDEST_CMUN);//Código do município	
                    dt.SetCampo("xMun_E11=" + nf.ENDERDEST_XMUN);//Nome do município	
                    dt.SetCampo("UF_E12=" + nf.ENDERDEST_UF);//Sigla da UF	
                    dt.SetCampo("CEP_E13=" + nf.ENDERDEST_CEP);//Código do CEP	
                    dt.SetCampo("cPais_E14=" + nf.ENDERDEST_CPAIS);//Código do País	
                    dt.SetCampo("xPais_E15=" + nf.ENDERDEST_XPAIS);//Nome do País	
                    dt.SetCampo("fone_E16=" + nf.ENDERDEST_FONE);//Telefone
                    dt.SetCampo("indIEDest_E16a=" + nf.DEST_INDIE);//Indicador da IE do Destinatário	
                    dt.SetCampo("IE_E17=" + nf.DEST_IE);//Inscrição Estadual do Destinatário	
                    dt.SetCampo("ISUF_E18=" + nf.DEST_ISUF);//Inscrição na SUFRAMA	
                    dt.SetCampo("IM_E18a=" + nf.DEST_IM);//Inscrição Municipal do Tomador do Serviço	
                    dt.SetCampo("email_E19=" + nf.DEST_EMAIL);//Email do destinatário	

                    //Identificação do Local de Retirada (Informar somente se diferente do endereço do remetente)
                    TECNO_NF_LOCAL_RETIRADA nf_retirada = new TECNO_NF_LOCAL_RETIRADA();
                    TECNO_NF_LOCAL_RETIRADA_DAL nf_retiradaDAL = new TECNO_NF_LOCAL_RETIRADA_DAL();
                    nf_retirada = nf_retiradaDAL.PesquisarTECNO_NF_LOCAL_RETIRADA(nf.ID_NOTA_FISCAL);

                    if (nf_retirada != null)
                    {
                        if (nf_retirada.RETIRADA_CNPJ.Length > 11)
                            dt.SetCampo("CNPJ_F02=" + nf_retirada.RETIRADA_CNPJ);//CNPJ
                        else
                            dt.SetCampo("CPF_F02a=" + nf_retirada.RETIRADA_CNPJ);//CPF

                        dt.SetCampo("xNome_F02b=" + nf_retirada.RETIRADA_XNOME);//Razão Social ou Nome do Expedidor	
                        dt.SetCampo("xLgr_F03=" + nf_retirada.RETIRADA_XLGR);//Logradouro
                        dt.SetCampo("nro_F04=" + nf_retirada.RETIRADA_NRO);//Número
                        dt.SetCampo("xCpl_F05=" + nf_retirada.RETIRADA_XCPL);//Complemento
                        dt.SetCampo("xBairro_F06=" + nf_retirada.RETIRADA_XBAIRRO);//Bairro
                        dt.SetCampo("cMun_F07=" + nf_retirada.RETIRADA_CMUN);//Código do município	
                        dt.SetCampo("xMun_F08=" + nf_retirada.RETIRADA_XMUN);//Nome do município	
                        dt.SetCampo("UF_F09=" + nf_retirada.RETIRADA_UF);//Sigla da UF	
                        dt.SetCampo("CEP_F10=" + nf_retirada.RETIRADA_CEP);//Código do CEP	
                        dt.SetCampo("cPais_F11=" + nf_retirada.RETIRADA_CPAIS);//Código do País	
                        dt.SetCampo("xPais_F12=" + nf_retirada.RETIRADA_XPAIS);//Nome do País	
                        dt.SetCampo("fone_F13=" + nf_retirada.RETIRADA_FONE);//Telefone
                        dt.SetCampo("email_F14=" + nf_retirada.RETIRADA_EMAIL);//Endereço de e-mail do Expedidor	
                        dt.SetCampo("IE_F15=" + nf_retirada.RETIRADA_IE);//Inscrição Estadual do Estabelecimento Expedidor	
                    }

                    //Identificação do Local de Entrega (Informar somente se diferente do endereço do destinatário)
                    TECNO_NF_LOCAL_ENTREGA nf_entrega = new TECNO_NF_LOCAL_ENTREGA();
                    TECNO_NF_LOCAL_ENTREGA_DAL nf_entregaDAL = new TECNO_NF_LOCAL_ENTREGA_DAL();
                    nf_entrega = nf_entregaDAL.PesquisarTECNO_NF_LOCAL_ENTREGA(nf.ID_NOTA_FISCAL);

                    if (nf_entrega != null)
                    {
                        if (nf_entrega.ENTREGA_CNPJ.Length > 11)
                            dt.SetCampo("CNPJ_G02=" + nf_entrega.ENTREGA_CNPJ);//CNPJ
                        else
                            dt.SetCampo("CPF_G02a=" + nf_entrega.ENTREGA_CNPJ);//CPF

                        dt.SetCampo("xNome_G02b=" + nf_entrega.ENTREGA_XNOME);//Razão Social ou Nome do Recebedor	
                        dt.SetCampo("xLgr_G03=" + nf_entrega.ENTREGA_XLGR);//Logradouro
                        dt.SetCampo("nro_G04=" + nf_entrega.ENTREGA_NRO);//Número
                        dt.SetCampo("xCpl_G05=" + nf_entrega.ENTREGA_XCPL);//Complemento
                        dt.SetCampo("xBairro_G06=" + nf_entrega.ENTREGA_XBAIRRO);//Bairro
                        dt.SetCampo("cMun_G07=" + nf_entrega.ENTREGA_CMUN);//Código do município	
                        dt.SetCampo("xMun_G08=" + nf_entrega.ENTREGA_XMUN);//Nome do município	
                        dt.SetCampo("UF_G09=" + nf_entrega.ENTREGA_UF);//Sigla da UF	
                        dt.SetCampo("CEP_G10=" + nf_entrega.ENTREGA_CEP);//Código do CEP	
                        dt.SetCampo("cPais_G11=" + nf_entrega.ENTREGA_CPAIS);//Código do País	
                        dt.SetCampo("xPais_G12=" + nf_entrega.ENTREGA_XPAIS);//Nome do País	
                        dt.SetCampo("fone_G13=" + nf_entrega.ENTREGA_FONE);//Telefone
                        dt.SetCampo("email_G14=" + nf_entrega.ENTREGA_EMAIL);//Endereço de e-mail do Recebedor	
                        dt.SetCampo("IE_G15=" + nf_entrega.ENTREGA_IE);//Inscrição Estadual do Estabelecimento Recebedor	
                    }

                    //Autorização para obter XML
                    dt.IncluirParte("autXML");
                    dt.SetCampo("CNPJ_GA02=" + nf.EMIT_CNPJ);//CNPJ Autorizado	
                    dt.SalvarParte("autXML");

                    dt.IncluirParte("autXML");
                    if (nf.DEST_CNPJ_CPF.Count() > 11)
                        dt.SetCampo("CNPJ_GA02=" + nf.DEST_CNPJ_CPF);//CNPJ Autorizado	
                    else
                        dt.SetCampo("CPF_GA03" + nf.DEST_CNPJ_CPF);//CPF Autorizado	
                    dt.SalvarParte("autXML");

                    //Produtos e Serviços da NF-e
                    List<TECNO_NF_PRODUTOS> nf_produtos = new List<TECNO_NF_PRODUTOS>();
                    TECNO_NF_PRODUTOS_DAL nf_produtosDAL = new TECNO_NF_PRODUTOS_DAL();
                    nf_produtos = nf_produtosDAL.ListarTECNO_NF_PRODUTOS(nf.ID_NOTA_FISCAL);

                    foreach (var p in nf_produtos)
                    {
                        dt.IncluirItem();
                        dt.SetCampo("nItem_H02=" + p.PROD_NITEM);//Número do Item	
                        dt.SetCampo("cProd_I02=" + p.PROD_CPROD);//Código do produto ou serviço	
                        dt.SetCampo("cEAN_I03=" + p.PROD_CEAN);//GTIN (Global Trade Item Number) do produto, antigo código EAN ou código de barras	
                        dt.SetCampo("xProd_I04=" + p.PROD_XPROD);//Descrição do produto ou serviço	
                        dt.SetCampo("NCM_I05=" + p.PROD_NCM);//Código NCM com 8 digitos	

                        if (p.prod_nve != "" && p.prod_nve != null)
                        {
                            dt.IncluirParte("NVE");
                            dt.SetCampo("NVE_I05a=" + p.prod_nve);//Codificação NVE - Nomenclatura de Valor Aduaneiro e Estatística.	
                            dt.SalvarParte("NVE");
                        }

                        //Sequência XML	
                        dt.SetCampo("CEST_I05c=" + p.PROD_CEST);//Código CEST	
                        dt.SetCampo("indEscala_I05d=" + p.PROD_INDESCALA);//Indicador de Escala Relevante	
                        dt.SetCampo("CNPJFab_I05e=" + p.PROD_CNPJFAB);//CNPJ do Fabricante da Mercadoria	
                                                                      //Fim Sequência XML	

                        dt.SetCampo("cBenef_I05f=" + p.PROD_CBENEF);//Código de Benefício Fiscal na UF aplicado ao item	
                        dt.SetCampo("EXTIPI_I06=" + p.PROD_EXTIPI);//EX_TIPI	
                        dt.SetCampo("CFOP_I08=" + p.PROD_CFOP);//Código Fiscal de Operações e Prestações	
                        dt.SetCampo("uCom_I09=" + p.PROD_UCOM);//Unidade Comercial	
                        dt.SetCampo("qCom_I10=" + p.PROD_QCOM);//Quantidade Comercial	
                        dt.SetCampo("vUnCom_I10a=" + p.PROD_VUNCOM);//Valor Unitário de Comercialização	
                        dt.SetCampo("vProd_I11=" + p.PROD_VPROD);//Valor Total Bruto dos Produtos ou Serviços	
                        dt.SetCampo("cEANTrib_I12=" + p.PROD_CEANTRIB);//GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras	
                        dt.SetCampo("uTrib_I13=" + p.PROD_UTRIB);//Unidade Tributável	
                        dt.SetCampo("qTrib_I14=" + p.PROD_QTRIB);//Quantidade Tributável	
                        dt.SetCampo("vUnTrib_I14a=" + p.PROD_VUNTRIB);//Valor Unitário de tributação	
                        dt.SetCampo("vFrete_I15=" + p.PROD_VFRETE);//Valor Total do Frete	
                        dt.SetCampo("vSeg_I16=" + p.PROD_VSEG);//Valor Total do Seg	
                        dt.SetCampo("vDesc_I17=" + p.PROD_VDESC);//Valor Total do Desconto	
                        dt.SetCampo("vOutro_I17a=" + p.PROD_VOUTRO);//Outras despesas	
                        dt.SetCampo("indTot_I17b=" + p.PROD_INDTOT);//Indica se valor do Item (vProd) entra no valor total da NF-e (vProd)	
                        dt.SetCampo("xPed_I60=" + p.PROD_XPED);//Número do Pedido de Compra	
                        dt.SetCampo("nItemPed_I61=" + p.PROD_NITEMPED);//Item do Pedido de Compra	
                        dt.SetCampo("nFCI_I70=" + p.PROD_NFCI);//Número de controle da FCI - Ficha de Conteúdo de Importação	

                        /*
                        //Declaração de Importação	 
                        dt.IncluirParte("DI");
                        dt.SetCampo("nDI_I19=");//Número do Documento de Importação (DI, DSI, DIRE, ...)	
                        dt.SetCampo("dDI_I20");//Data de Registro do documento	
                        dt.SetCampo("xLocDesemb_I21");//Local de desembaraço	
                        dt.SetCampo("UFDesemb_I22");// Sigla da UF onde ocorreu o Desembaraço Aduaneiro
                        dt.SetCampo("dDesemb_I23");//Data do Desembaraço Aduaneiro	
                        dt.SetCampo("tpViaTransp_I23a");//Via de transporte internacional informada na Declaração de Importação (DI)	
                        dt.SetCampo("vAFRMM_I23b");//Valor da AFRMM - Adicional ao Frete para Renovação da Marinha Mercante	
                        dt.SetCampo("tpIntermedio_I23c");//Forma de importação quanto a intermediação	
                        dt.SetCampo("CNPJ_I23d");//CNPJ do adquirente ou do encomendante	
                        dt.SetCampo("UFTerceiro_I23e");//Sigla da UF do adquirente ou do encomendante	
                        dt.SetCampo("cExportador_I24");//Código do Exportador	
                        
                        //Adições
                        dt.IncluirParte("ADI");
                        dt.SetCampo("nAdicao_I26=");//Numero da Adição	
                        dt.SetCampo("nSeqAdic_I27");//Numero Sequência l do item dentro da Adição	
                        dt.SetCampo("cFabricante_I28");//Código do fabricante estrangeiro	
                        dt.SetCampo("vDescDI_I29");//Valor do desconto do item da DI – Adição	
                        dt.SetCampo("nDraw_I29a");//Número do ato concessório de Drawback	
                        dt.SalvarParte("ADI");
                        
                        dt.SalvarParte("DI");

                        //Grupo de informações de exportação para o item
                        dt.IncluirParte("DETEXPORT");
                        dt.SetCampo("nDraw_I51=");//Número do ato concessório de Drawback	
                        dt.SalvarParte("DETEXPORT");

                        //Grupo sobre exportação indireta
                        dt.IncluirParte("EXPORTIND");
                        dt.SetCampo("nRE_I53=");//Número do Registro de Exportação	
                        dt.SetCampo("chNFe_I54=");//Chave de Acesso da NF-e recebida para exportação	
                        dt.SetCampo("qExport_I55=");//Quantidade do item realmente exportado	
                        dt.SalvarParte("EXPORTIND");

                        //Detalhamento de produto sujeito a rastreabilidade
                        dt.IncluirParte("I80");
                        dt.SetCampo("nLote_I81=");//Número do Lote do produto	
                        dt.SetCampo("qLote_I82=");//Quantidade de produto no Lote	
                        dt.SetCampo("dFab_I83=");//Data de fabricação/ Produção	
                        dt.SetCampo("dVal_I84=");//Data de validade	
                        dt.SetCampo("cAgreg_I85=");//Código de Agregação	
                        dt.SalvarParte("I80");

                        //Sequência XML	
                        //Detalhamento de Medicamentos e de matérias-primas farmacêuticas
                        dt.SetCampo("cProdANVISA_K01a=");//Código de Produto da ANVISA	
                        dt.SetCampo("xMotivoIsencao_K01b=");//Motivo da isenção da ANVISA	
                        dt.SetCampo("vPMC_K06=");//Preço máximo consumidor	

                        //Detalhamento Específico de Armamentos
                        dt.IncluirParte("L");
                        dt.SetCampo("tpArma_L02=");//Indicador do tipo de arma de fogo	
                        dt.SetCampo("nSerie_L03=");//Número de série da arma	
                        dt.SetCampo("nCano_L04=");//Número de série do cano	
                        dt.SetCampo("descr_L05=");//Descrição
                        dt.SalvarParte("L");

                        //Detalhamento Específico de Veículos novos
                        dt.SetCampo("tpOp_J02=");//Tipo da operação	
                        dt.SetCampo("chassi_J03=");//Chassi do veículo	
                        dt.SetCampo("cCor_J04=");//Codigo da Cor	
                        dt.SetCampo("xCor_J05=");//Descrição da Cor	
                        dt.SetCampo("pot_J06=");//Potência Motor (CV)	
                        dt.SetCampo("cilin_J07=");//Cilindradas	
                        dt.SetCampo("pesoL_J08=");//Peso Líquido	
                        dt.SetCampo("pesoB_J09=");//Peso Bruto	
                        dt.SetCampo("nSerie_J10=");//Serial (série)	
                        dt.SetCampo("tpComb_J11=");//Tipo de combustível	
                        dt.SetCampo("nMotor_J12=");//Número de Motor	
                        dt.SetCampo("CMT_J13=");//Capacidade Máxima de Tração	
                        dt.SetCampo("dist_J14=");//Distância entre eixos	
                        dt.SetCampo("anoMod_J16=");//Ano Modelo de Fabricação	
                        dt.SetCampo("anoFab_J17=");//Ano de Fabricação	
                        dt.SetCampo("tpPint_J18=");//Tipo de Pintura	
                        dt.SetCampo("tpVeic_J19=");//Tipo de Veículo	
                        dt.SetCampo("espVeic_J20=");//Espécie de Veículo	
                        dt.SetCampo("VIN_J21=");//Condição do VIN	
                        dt.SetCampo("condVeic_J22=");//Condição do Veículo	
                        dt.SetCampo("cMod_J23=");//Código Marca Modelo	
                        dt.SetCampo("cCorDENATRAN_J24=");//Código da Cor	
                        dt.SetCampo("lota_J25=");//Capacidade máxima de lotação	
                        dt.SetCampo("tpRest_J26=");//Restrição

                        //Informações específicas para combustíveis líquidos e lubrificantes
                        dt.SetCampo("cProdANP_LA02=");//Código de produto da ANP	
                        dt.SetCampo("descANP_LA03=");//Descrição do produto conforme ANP	
                        dt.SetCampo("pGLP_LA03a=");//Percentual do GLP derivado do petróleo no produto GLP (cProdANP=210203001)	
                        dt.SetCampo("pGNn_LA03b=");//Percentual de Gás Natural Nacional – GLGNn para o produto GLP (cProdANP=210203001)	
                        dt.SetCampo("pGNi_LA03c=");//Percentual de Gás Natural Importado – GLGNi para o produto GLP (cProdANP=210203001)	
                        dt.SetCampo("vPart_LA03d=");//Valor de partida (cProdANP=210203001)	
                        dt.SetCampo("CODIF_LA04=");//Código de autorização / registro do CODIF	
                        dt.SetCampo("qTemp_LA05=");//Quantidade de combustível faturada à temperatura ambiente.	
                        dt.SetCampo("UFCons_LA06=");//Sigla da UF de consumo	
                                                    //Informações da CIDE	
                        dt.SetCampo("qBCProd_LA08=");//BC da CIDE	
                        dt.SetCampo("vAliqProd_LA09=");//Valor da alíquota da CIDE	
                        dt.SetCampo("vCIDE_LA10=");//Valor da CIDE	
                                                   //Informações do grupo de “encerrante”
                        dt.SetCampo("nBico_LA12=");//Número de identificação do bico utilizado no abastecimento	
                        dt.SetCampo("nBomba_LA13=");//Número de identificação da bomba ao qual o bico está interligado	
                        dt.SetCampo("nTanque_LA14=");//Número de identificação do tanque ao qual o bico está interligado	
                        dt.SetCampo("vEncIni_LA15=");//Valor do Encerrante no início do abastecimento	
                        dt.SetCampo("vEncFin_LA16=");//Valor do Encerrante no final do abastecimento	

                        //Detalhamento Específico para Operação com Papel Imune
                        dt.SetCampo("nRECOPI_LB01=");//Número do RECOPI	
                                                     //Fim Sequência XML	
                        */

                        //Tributos incidentes no Produto ou Serviço
                        dt.SetCampo("vTotTrib_M02=" +nf.VTOTTRIB);//Valor aproximado total de tributos federais, estaduais e municipais.	

                        TECNO_NF_PRODUTOS_ICMS icms = new TECNO_NF_PRODUTOS_ICMS();
                        TECNO_NF_PRODUTOS_ICMS_DAL icmsDAL = new TECNO_NF_PRODUTOS_ICMS_DAL();
                        icms = icmsDAL.PesquisarTECNO_NF_PRODUTOS_ICMS(p.ID_NOTA_FISCAL, p.PROD_NITEM);
                        if (icms != null)
                        {
                            if (nf.IDE_CRT == 3)
                            {
                                //ICMS
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=00
                                if (icms.ICMS_CST == 0)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS = 00	
                                    dt.SetCampo("modBC_N13=" + icms.ICMS_MODBC);//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("vBC_N15=" + icms.ICMS_VBC);//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + icms.ICMS_PICMS);//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + icms.ICMS_VICMS);//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("pFCP_N17b=" + icms.ICMS_PFCP);//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + icms.ICMS_VFCP);//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS = 10
                                else if (icms.ICMS_CST == 10)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS = 10	
                                    dt.SetCampo("modBC_N13=" + icms.ICMS_MODBC);//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("vBC_N15=" + icms.ICMS_VBC);//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + icms.ICMS_PICMS);//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + icms.ICMS_VICMS);//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + icms.ICMS_VBCFCP);//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + icms.ICMS_PFCP);//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + icms.ICMS_VFCP);//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                    dt.SetCampo("modBCST_N18=" + icms.ICMS_MODBCST);//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + icms.ICMS_PMVAST);//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + icms.ICMS_PREDBCST);//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + icms.ICMS_VBCST);//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + icms.ICMS_PICMSST);//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + icms.ICMS_VICMSST);//Valor do ICMS ST	
                                                                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPST_N23a=" + icms.ICMS_VBCFCPST);//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + icms.ICMS_PFCPST);//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + icms.ICMS_VFCPST);//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=20
                                else if (icms.ICMS_CST == 20)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS = 20	
                                    dt.SetCampo("modBC_N13=" + icms.ICMS_MODBC);//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + icms.ICMS_PREDBC);//Percentual da Redução de BC		
                                    dt.SetCampo("vBC_N15=" + icms.ICMS_VBC);//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + icms.ICMS_PICMS);//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + icms.ICMS_VICMS);//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + icms.ICMS_VBCFCP);//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + icms.ICMS_PFCP);//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + icms.ICMS_VFCP);//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                                                               //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + icms.ICMS_VICMSDESON);//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + icms.ICMS_MOTDESICMS);//Motivo da desoneração do ICMS
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=30
                                else if (icms.ICMS_CST == 30)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS = 30	
                                    dt.SetCampo("pMVAST_N19=" + icms.ICMS_PMVAST);//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + icms.ICMS_PREDBCST);//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + icms.ICMS_VBCST);//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + icms.ICMS_PICMSST);//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + icms.ICMS_VICMSST);//Valor do ICMS ST	
                                                                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPST_N23a=" + icms.ICMS_VBCFCPST);//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + icms.ICMS_PFCPST);//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + icms.ICMS_VFCPST);//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML	

                                    //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + icms.ICMS_VICMSDESON);//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + icms.ICMS_MOTDESICMS);//Motivo da desoneração do ICMS	
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação ICMS = 40, 41, 50
                                else if (icms.ICMS_CST == 40 || icms.ICMS_CST == 41 || icms.ICMS_CST == 50)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS 
                                                                            //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + icms.ICMS_VICMSDESON);//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + icms.ICMS_MOTDESICMS);//Motivo da desoneração do ICMS	
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação ICMS=51
                                else if (icms.ICMS_CST == 51)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS 
                                    dt.SetCampo("modBC_N13=" + icms.ICMS_MODBC);//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + icms.ICMS_PREDBC);//Percentual da Redução de BC	
                                    dt.SetCampo("vBC_N15=" + icms.ICMS_VBC);//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + icms.ICMS_PICMS);//Alíquota do imposto	
                                    dt.SetCampo("vICMSOp_N16a=" + icms.ICMS_VICMSOP);//Valor do ICMS da Operação	
                                    dt.SetCampo("pDif_N16b=" + icms.ICMS_PDIF);//Percentual do diferimento	
                                    dt.SetCampo("vICMSDif_N16c=" + icms.ICMS_VICMSDIF);//Valor do ICMS diferido	
                                    dt.SetCampo("vICMS_N17=" + icms.ICMS_VICMS);//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + icms.ICMS_VBCFCP);//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + icms.ICMS_PFCP);//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + icms.ICMS_VFCP);//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=60
                                else if (icms.ICMS_CST == 60)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS
                                                                            //Sequência XML	
                                    dt.SetCampo("vBCSTRet_N26=" + icms.ICMS_VBCSTRET);//Valor da BC do ICMS ST retido	
                                    dt.SetCampo("pST_N26a=" + icms.ICMS_PST);//Alíquota suportada pelo Consumidor Final	
                                    dt.SetCampo("vICMSSubstituto_N26b=" + icms.ICMS_VICMSSUBSTITUTO);//Valor do ICMS próprio do Substituto	
                                    dt.SetCampo("vICMSSTRet_N27=" + icms.ICMS_VICMSSTRET);//Valor do ICMS ST retido	
                                                                                          //Fim da Sequência XML	

                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPSTRet_N27a=" + icms.ICMS_VBCFCPSTRET);//Valor da Base de Cálculo do FCP retido anteriormente por ST	
                                    dt.SetCampo("pFCPSTRet_N27b=" + icms.ICMS_PFCPSTRET);//Percentual do FCP retido anteriormente por Substituição Tributária	
                                    dt.SetCampo("vFCPSTRet_N27d=" + icms.ICMS_VFCPSTRET);//Valor do FCP retido por Substituição Tributária	
                                                                                         //Fim da Sequência XML	

                                    //Sequência XML	
                                    dt.SetCampo("pRedBCEfet_N34=" + icms.ICMS_REDBCEFET);//Percentual de redução da base de cálculo efetiva	
                                    dt.SetCampo("vBCEfet_N35=" + icms.ICMS_VBCEFET);//Valor da base de cálculo efetiva	
                                    dt.SetCampo("pICMSEfet_N36=" + icms.ICMS_PEFET);//Alíquota do ICMS efetiva	
                                    dt.SetCampo("vICMSEfet_N37=" + icms.ICMS_VEFET);//Valor do ICMS efetivo	
                                                                                    //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=70
                                else if (icms.ICMS_CST == 70)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS 
                                    dt.SetCampo("modBC_N13=" + icms.ICMS_MODBC);//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + icms.ICMS_PREDBC);//Percentual da Redução de BC	
                                    dt.SetCampo("vBC_N15=" + icms.ICMS_VBC);//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + icms.ICMS_PICMS);//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + icms.ICMS_VICMS);//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + icms.ICMS_VBCFCP);//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + icms.ICMS_PFCP);//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + icms.ICMS_VFCP);//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                                                               //Sequência XML	
                                    dt.SetCampo("modBCST_N18=" + icms.ICMS_MODBCST);//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + icms.ICMS_PMVAST);//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + icms.ICMS_PREDBCST);//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + icms.ICMS_VBCST);//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + icms.ICMS_PICMSST);//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + icms.ICMS_VICMSST);//vICMSST_N23
                                                                                    //Fim da Sequência XML	
                                                                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPST_N23a=" + icms.ICMS_VBCFCPST);//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + icms.ICMS_PFCPST);//Percentual do FCP retido por Substituição Tributária		
                                    dt.SetCampo("vFCPST_N23d=" + icms.ICMS_VFCPST);//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML	
                                                                                   //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + icms.ICMS_VICMSDESON);//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + icms.ICMS_MOTDESICMS);//Motivo da desoneração do ICMS	
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=90
                                else if (icms.ICMS_CST == 90)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS
                                    dt.SetCampo("modBC_N13=" + icms.ICMS_MODBC);//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + icms.ICMS_PREDBC);//Percentual da Redução de BC	
                                    dt.SetCampo("vBC_N15=" + icms.ICMS_VBC);//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + icms.ICMS_PICMS);//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + icms.ICMS_VICMS);//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + icms.ICMS_VBCFCP);//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + icms.ICMS_PFCP);//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + icms.ICMS_VFCP);//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                                                               //Sequência XML	
                                    dt.SetCampo("modBCST_N18=" + icms.ICMS_MODBCST);//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + icms.ICMS_PMVAST);//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + icms.ICMS_PREDBCST);//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + icms.ICMS_VBCST);//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + icms.ICMS_PICMSST);//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + icms.ICMS_VICMSST);//vICMSST_N23
                                                                                    //Fim da Sequência XML	
                                                                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPST_N23a=" + icms.ICMS_VBCFCPST);//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + icms.ICMS_PFCPST);//Percentual do FCP retido por Substituição Tributária		
                                    dt.SetCampo("vFCPST_N23d=" + icms.ICMS_VFCPST);//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML
                                                                                   //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + icms.ICMS_VICMSDESON);//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + icms.ICMS_MOTDESICMS);//Motivo da desoneração do ICMS	
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------

                                //Grupo de Repasse de ICMS ST retido anteriormente em operações interestaduais com repasses através do Substituto Tributário
                                dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                dt.SetCampo("CST_N12=" + icms.ICMS_CST);//Tributação do ICMS
                                dt.SetCampo("vBCSTRet_N26=" + icms.ICMS_VBCSTRET);//Valor do BC do ICMS ST retido na UF remetente	
                                dt.SetCampo("pST_N26a=" + icms.ICMS_PST);//Alíquota suportada pelo Consumidor Final	
                                dt.SetCampo("vICMSSubstituto_N26b=" + icms.ICMS_VICMSSUBSTITUTO);//Valor do ICMS próprio do Substituto	
                                dt.SetCampo("vICMSSTRet_N27=" + icms.ICMS_VICMSSTRET);//Valor do ICMS ST retido na UF remetente	
                                                                                      //Sequência XML	
                                dt.SetCampo("vBCFCPSTRet_N27a=" + icms.ICMS_VBCFCPSTRET);//Valor da Base de Cálculo do FCP retido anteriormente por ST	
                                dt.SetCampo("pFCPSTRet_N27b=" + icms.ICMS_PFCPSTRET);//Percentual do FCP retido anteriormente por Substituição Tributária	
                                dt.SetCampo("vFCPSTRet_N27d=" + icms.ICMS_VFCPSTRET);//Valor do FCP retido por Substituição Tributária	
                                                                                     //Fim da Sequência XML	
                                dt.SetCampo("vBCSTDest_N31=" + icms.ICMS_VBCSTDEST);//Valor da BC do ICMS ST da UF destino	
                                dt.SetCampo("vICMSSTDest_N32=" + icms.ICMS_VICMSSTDEST);//Valor do ICMS ST da UF destino	
                                                                                        //Sequência XML	
                                dt.SetCampo("pRedBCEfet_N34=" + icms.ICMS_REDBCEFET);//Percentual de redução da base de cálculo efetiva	
                                dt.SetCampo("vBCEfet_N35=" + icms.ICMS_VBCEFET);//Valor da base de cálculo efetiva	
                                dt.SetCampo("pICMSEfet_N36=" + icms.ICMS_PEFET);//Alíquota do ICMS efetiva	
                                dt.SetCampo("vICMSEfet_N37=" + icms.ICMS_VEFET);//Valor do ICMS efetivo	
                                                                                //Fim da Sequência XML	
                            }
                            else
                            {                           
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT = 1 – Simples Nacional e CSOSN = 101
                                if (icms.ICMS_CSOSN == 101)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + icms.ICMS_CSOSN);//Código de Situação da Operação – Simples Nacional	
                                    dt.SetCampo("pCredSN_N29=" + icms.ICMS_PCREDSN);//Alíquota aplicável de cálculo do crédito (Simples Nacional).	
                                    dt.SetCampo("vCredICMSSN_N30=" + icms.ICMS_VCREDICMSSN);//Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (Simples Nacional)
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN=102, 103, 300 ou 400
                                else if (icms.ICMS_CSOSN == 102 || icms.ICMS_CSOSN == 103 || icms.ICMS_CSOSN == 300 || icms.ICMS_CSOSN == 400)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + icms.ICMS_CSOSN);//Código de Situação da Operação – Simples Nacional	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN=201
                                else if (icms.ICMS_CSOSN == 201)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + icms.ICMS_CSOSN);//Código de Situação da Operação – Simples Nacional	
                                    dt.SetCampo("modBCST_N18=" + icms.ICMS_MODBCST);//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + icms.ICMS_PMVAST);//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + icms.ICMS_PREDBCST);//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + icms.ICMS_VBCST);//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + icms.ICMS_PICMSST);//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + icms.ICMS_VICMSST);//Valor do ICMS ST	
                                                                                    //Sequência XML
                                    dt.SetCampo("vBCFCPST_N23a=" + icms.ICMS_VBCFCPST);//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + icms.ICMS_PFCPST);//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + icms.ICMS_VFCPST);//Valor do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pCredSN_N29=" + icms.ICMS_PCREDSN);//Alíquota aplicável de cálculo do crédito (SIMPLES NACIONAL).	
                                    dt.SetCampo("vCredICMSSN_N30=" + icms.ICMS_VCREDICMSSN);//Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (SIMPLES NACIONAL)
                                                                                            //Fim da Sequência XML
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN=202 ou 203
                                else if (icms.ICMS_CSOSN == 202 || icms.ICMS_CSOSN == 203)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + icms.ICMS_CSOSN);//Código de Situação da Operação – Simples Nacional	
                                    dt.SetCampo("modBCST_N18=" + icms.ICMS_MODBCST);//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + icms.ICMS_PMVAST);//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + icms.ICMS_PREDBCST);//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + icms.ICMS_VBCST);//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + icms.ICMS_PICMSST);//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + icms.ICMS_VICMSST);//Valor do ICMS ST	
                                                                                    //Sequência XML
                                    dt.SetCampo("vBCFCPST_N23a=" + icms.ICMS_VBCFCPST);//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + icms.ICMS_PFCPST);//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + icms.ICMS_VFCPST);//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN = 500
                                else if (icms.ICMS_CSOSN == 500)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + icms.ICMS_CSOSN);//Código de Situação da Operação – Simples Nacional
                                                                                 //Sequência XML
                                    dt.SetCampo("vBCSTRet_N26=" + icms.ICMS_VBCSTRET);//Valor da BC do ICMS ST retido	
                                    dt.SetCampo("pST_N26a=" + icms.ICMS_PST);//Alíquota suportada pelo Consumidor Final	
                                    dt.SetCampo("vICMSSubstituto_N26b=" + icms.ICMS_VICMSSUBSTITUTO);//Valor do ICMS próprio do Substituto	
                                    dt.SetCampo("vICMSSTRet_N27=" + icms.ICMS_VICMSSTRET);//Valor do ICMS ST retido	
                                                                                          //Fim da Sequência XML
                                                                                          //Sequência XML
                                    dt.SetCampo("vBCFCPSTRet_N27a=" + icms.ICMS_VBCFCPSTRET);//Valor da Base de Cálculo do FCP retido anteriormente por ST	
                                    dt.SetCampo("pFCPSTRet_N27b=" + icms.ICMS_PFCPSTRET);//Percentual do FCP retido anteriormente por Substituição Tributária	
                                    dt.SetCampo("vFCPSTRet_N27d=" + icms.ICMS_VFCPSTRET);//Valor do FCP retido por Substituição Tributária	
                                                                                         //Fim da Sequência XML
                                                                                         //Sequência XML
                                    dt.SetCampo("pRedBCEfet_N34=" + icms.ICMS_REDBCEFET);//Percentual de redução da base de cálculo efetiva	
                                    dt.SetCampo("vBCEfet_N35=" + icms.ICMS_VBCEFET);//Valor da base de cálculo efetiva	
                                    dt.SetCampo("pICMSEfet_N36=" + icms.ICMS_PEFET);//Alíquota do ICMS efetiva	
                                    dt.SetCampo("vICMSEfet_N37=" + icms.ICMS_VEFET);//Valor do ICMS efetivo	
                                                                                    //Fim da Sequência XML
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN=900
                                else if (icms.ICMS_CSOSN == 900)
                                {
                                    dt.SetCampo("orig_N11=" + icms.ICMS_ORIG);//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + icms.ICMS_CSOSN);//Código de Situação da Operação – Simples Nacional	
                                                                                 //Sequência XML
                                    dt.SetCampo("modBC_N13=" + icms.ICMS_MODBC);//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + icms.ICMS_PREDBC);//Percentual da Redução de BC	
                                    dt.SetCampo("vBC_N15=" + icms.ICMS_VBC);//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + icms.ICMS_PICMS);//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + icms.ICMS_VICMS);//Valor do ICMS	
                                                                                //Fim da Sequência XML
                                                                                //Sequência XML
                                    dt.SetCampo("modBCST_N18=" + icms.ICMS_MODBCST);//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + icms.ICMS_PMVAST);//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + icms.ICMS_PREDBCST);//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + icms.ICMS_VBCST);//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + icms.ICMS_PICMSST);//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + icms.ICMS_VICMSST);//Valor do ICMS ST	
                                    dt.SetCampo("pBCOp_N25=" + icms.ICMS_PBCOP);//Percentual da BC operação própria	
                                    dt.SetCampo("UFST_N24=" + icms.ICMS_UFST);//UF para qual é devido o ICMS ST	
                                                                              //Fim da Sequência XML
                                                                              //Sequência XML
                                    dt.SetCampo("vBCFCPST_N23a=" + icms.ICMS_VBCFCPST);//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + icms.ICMS_PFCPST);//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + icms.ICMS_VFCPST);//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML
                                                                                   //Sequência XML
                                    dt.SetCampo("pCredSN_N29=" + icms.ICMS_PCREDSN);//Alíquota aplicável de cálculo do crédito (SIMPLES NACIONAL).	
                                    dt.SetCampo("vCredICMSSN_N30=" + icms.ICMS_VCREDICMSSN);//Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (SIMPLES NACIONAL)	
                                                                                            //Fim da Sequência XML
                                }
                                //-----------------------------------------------------------------------------------------------------------------------------
                            }

                            //ICMS para a UF de destino (Partilha de ICMS)
                            TECNO_NF_PRODUTOS_ICMS_UFDEST icmsDest = new TECNO_NF_PRODUTOS_ICMS_UFDEST();
                            TECNO_NF_PRODUTOS_ICMS_UFDEST_DAL icmsDestDAL = new TECNO_NF_PRODUTOS_ICMS_UFDEST_DAL();
                            icmsDest = icmsDestDAL.PesquisarTECNO_NF_PRODUTOS_ICMS_UFDEST(p.ID_NOTA_FISCAL, p.PROD_NITEM);
                            if (icmsDest != null)
                            {
                                dt.SetCampo("vBCUFDest_NA03=" + icmsDest.VBCFCPUFDEST);//Valor da BC do ICMS na UF de destino	
                                dt.SetCampo("vBCFCPUFDest_NA04=" + icmsDest.VBCFCPUFDEST);//Valor da BC do FCP na UF de destino	
                                dt.SetCampo("pFCPUFDest_NA05=" + icmsDest.PFCPUFDEST);//Percentual do ICMS relativo ao Fundo de Combate à Pobreza (FCP) na UF de destino	
                                dt.SetCampo("pICMSUFDest_NA07=" + icmsDest.PICMSUFDEST);//Alíquota interna da UF de destino	
                                dt.SetCampo("pICMSInter_NA09=" + icmsDest.PICMSINTER);//Alíquota interestadual das UF envolvidas	
                                dt.SetCampo("pICMSInterPart_NA11=" + icmsDest.PICMSINTERPART);//Percentual provisório de partilha do ICMS Interestadual	
                                dt.SetCampo("vFCPUFDest_NA13=" + icmsDest.VFCPUFDEST);//Valor do ICMS relativo ao Fundo de Combate à Pobreza (FCP) da UF de destino	
                                dt.SetCampo("vICMSUFDest_NA15=" + icmsDest.VICMSUFDEST);//Valor do ICMS Interestadual para a UF de destino	
                                dt.SetCampo("vICMSUFRemet_NA17=" + icmsDest.VICMSUFREMET);//Valor do ICMS Interestadual para a UF do remetente	
                            }
                        }

                        

                        //-------------------------------------------IPI----------------------------------------------------

                        TECNO_NF_PRODUTOS_IPI ipi = new TECNO_NF_PRODUTOS_IPI();
                        TECNO_NF_PRODUTOS_IPI_DAL ipiDAL = new TECNO_NF_PRODUTOS_IPI_DAL();
                        ipi = ipiDAL.PesquisarTECNO_NF_PRODUTOS_IPI(p.ID_NOTA_FISCAL, p.PROD_NITEM);
                        if (ipi != null)
                        {
                            //Imposto sobre Produtos Industrializados
                            dt.SetCampo("CNPJProd_O03=" + ipi.IPI_CNPJPROD);//CNPJ do produtor da mercadoria, quando diferente do emitente. Somente para os casos de exportação direta ou indireta.	
                            dt.SetCampo("cSelo_O04=" + ipi.IPI_CSELO);//Código do selo de controle IPI	
                            dt.SetCampo("qSelo_O05=" + ipi.IPI_QSELO);//Quantidade de selo de controle	
                            dt.SetCampo("cEnq_O06=" + ipi.IPI_CENQ);//Código de Enquadramento Legal do IPI	

                            //IPI Trib	Grupo do CST 00, 49, 50 e 99
                            if (ipi.IPI_CST == 0 || ipi.IPI_CST == 49 || ipi.IPI_CST == 50 || ipi.IPI_CST == 99)
                            {
                                dt.SetCampo("CST_O09=" + ipi.IPI_CST);//Código da situação tributária do IPI	
                                                                      //Sequência XML
                                dt.SetCampo("vBC_O10=" + ipi.IPI_VBC);//Valor da BC do IPI	
                                dt.SetCampo("pIPI_O13=" + ipi.IPI_PIPI);//Alíquota do IPI	
                                                                        //Fim da Sequência XML
                                                                        //Sequência XML
                                dt.SetCampo("qUnid_O11=" + ipi.IPI_QUNID);//Quantidade total na unidade padrão para tributação (somente para os produtos tributados por unidade)	
                                dt.SetCampo("vUnid_O12=" + ipi.IPI_VUNID);//Valor por Unidade Tributável	
                                                                          //Fim da Sequência XML
                                dt.SetCampo("vIPI_O14=" + ipi.IPI_VIPI);//Valor do IPI	
                            }
                            //IPINT	Grupo CST 01, 02, 03, 04, 51, 52, 53, 54 e 55
                            else if (ipi.IPI_CST == 01 || ipi.IPI_CST == 02 || ipi.IPI_CST == 03 || ipi.IPI_CST == 04 || ipi.IPI_CST == 51 || ipi.IPI_CST == 52 || ipi.IPI_CST == 53 || ipi.IPI_CST == 54 || ipi.IPI_CST == 55)
                            {
                                dt.SetCampo("CST_O09=" + ipi.IPI_CST);//Código da situação tributária do IPI	
                            }


                            //Imposto de Importação	
                            TECNO_NF_PRODUTOS_II ii = new TECNO_NF_PRODUTOS_II();
                            TECNO_NF_PRODUTOS_II_DAL iiDAL = new TECNO_NF_PRODUTOS_II_DAL();
                            ii = iiDAL.PesquisarTECNO_NF_PRODUTOS_II(ipi.ID_NOTA_FISCAL, ipi.PROD_NITEM);
                            if (ii != null)
                            {
                                dt.SetCampo("vBC_P02=" + ii.II_VBC);//Valor BC do Imposto de Importação	
                                dt.SetCampo("vDespAdu_P03=" + ii.II_VDESPADU);//Valor despesas aduaneiras	
                                dt.SetCampo("vII_P04=" + ii.II_VII);//Valor Imposto de Importação	
                                dt.SetCampo("vIOF_P05=" + ii.II_VIOF);//Valor Imposto sobre Operações Financeiras	
                            }
                        }

                        //-------------------------------------------PIS----------------------------------------------------

                        TECNO_NF_PRODUTOS_PIS pis = new TECNO_NF_PRODUTOS_PIS();
                        TECNO_NF_PRODUTOS_PIS_DAL pisDAL = new TECNO_NF_PRODUTOS_PIS_DAL();
                        pis = pisDAL.PesquisarTECNO_NF_PRODUTOS_PIS(p.ID_NOTA_FISCAL, p.PROD_NITEM);
                        if (pis != null)
                        {
                            if (pis.PIS_CST == 1 || pis.PIS_CST == 2)
                            {
                                //dt.SetCampo("PISAliq=" + pis.PIS_VALIQPROD);//Grupo PIS tributado pela alíquota	
                                dt.SetCampo("CST_Q06=" + pis.PIS_CST);//Código de Situação Tributária do PIS	
                                dt.SetCampo("vBC_Q07=" + pis.PIS_VBC);//Valor da Base de Cálculo do PIS	
                                dt.SetCampo("pPIS_Q08=" + pis.PIS_PPIS);//Alíquota do PIS (em percentual)	
                                dt.SetCampo("vPIS_Q09=" + pis.PIS_VPIS);//Valor do PIS	
                            }
                            else if (pis.PIS_CST == 3)
                            {
                                //Grupo PIS tributado por Qtde	
                                dt.SetCampo("CST_Q06=" + pis.PIS_CST);//Código de Situação Tributária do PIS	
                                dt.SetCampo("qBCProd_Q10=" + pis.PIS_QBCPROD);//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_Q11=" + pis.PIS_VALIQPROD);//Alíquota do PIS (em reais)	
                                dt.SetCampo("vPIS_Q09=" + pis.PIS_VPIS);//Valor do PIS	
                            }
                            else if (pis.PIS_CST == 4 || pis.PIS_CST == 5 || pis.PIS_CST == 6 || pis.PIS_CST == 7 || pis.PIS_CST == 8 || pis.PIS_CST == 9)
                            {
                                //PISNT	Grupo PIS não tributado
                                dt.SetCampo("CST_Q06=" + pis.PIS_CST);//Código de Situação Tributária do PIS	
                                                                      //Sequência XML 
                            }
                            else if (pis.PIS_CST > 49 && pis.PIS_CST < 99)
                            {
                                //Grupo PIS outras Operações
                                dt.SetCampo("CST_Q06=" + pis.PIS_CST);//Código de Situação Tributária do PIS	
                                dt.SetCampo("vBC_Q07=" + pis.PIS_VBC);//Valor da Base de Cálculo do PIS	
                                dt.SetCampo("pPIS_Q08=" + pis.PIS_PPIS);//Alíquota do PIS (em percentual)	
                                                                        //Fim da Sequência XML
                                                                        //Sequência XML - Informar os campos Q10 e Q11 se o cálculo do PIS for em valor
                                dt.SetCampo("qBCProd_Q10=" + pis.PIS_QBCPROD);//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_Q11=" + pis.PIS_VALIQPROD);//Alíquota do PIS (em reais)	
                                                                                  //Fim da Sequência XML
                            }
                            else
                            {
                                //PISST	Grupo PIS Substituição Tributária

                                //Sequência XML	 	1-1	 	Informar os campos R02 e R03 para cálculo do PIS em percentual
                                dt.SetCampo("vBC_R02=" + pis.PIS_VBC);//Valor da Base de Cálculo do PIS	
                                dt.SetCampo("pPIS_R03=" + pis.PIS_PPIS);//Alíquota do PIS (em percentual)	
                                                                        //Fim da Sequência XML
                                                                        //Sequência XML	 	1-1	 	Informar os campos R04 e R05 para cálculo do PIS em valor
                                dt.SetCampo("qBCProd_R04=" + pis.PIS_QBCPROD);//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_R05=" + pis.PIS_VALIQPROD);//Alíquota do PIS (em reais)	
                                                                                  //Fim da Sequência XML
                                dt.SetCampo("vPIS_R06=" + pis.PIS_VPIS);//Valor do PIS	
                            }
                        }

                        //-------------------------------------------cofins-------------------------------------------------

                        TECNO_NF_PRODUTOS_COFINS cofins = new TECNO_NF_PRODUTOS_COFINS();
                        TECNO_NF_PRODUTOS_COFINS_DAL cofinsDAL = new TECNO_NF_PRODUTOS_COFINS_DAL();
                        cofins = cofinsDAL.PesquisarTECNO_NF_PRODUTOS_COFINS(p.ID_NOTA_FISCAL, p.PROD_NITEM);
                        if (cofins != null)
                        {
                            if (cofins.COFINS_CST == 1 || cofins.COFINS_CST == 2)
                            {
                                //Informar apenas um dos grupos COFINSAliq, COFINSQtde, COFINSNT ou COFINSOutr com base valor atribuído ao campo CST_S06
                                //dt.SetCampo("COFINSAliq=");//Grupo COFINS tributado pela alíquota	
                                dt.SetCampo("CST_S06=" + cofins.COFINS_CST);//Código de Situação Tributária do COFINS	
                                dt.SetCampo("vBC_S07=" + cofins.COFINS_VBC);//Valor da Base de Cálculo do COFINS	
                                dt.SetCampo("pCOFINS_S08=" + cofins.COFINS_PCOFINS);//Alíquota do COFINS (em percentual)	
                                dt.SetCampo("vCOFINS_S11=" + cofins.COFINS_VCOFINS);//Valor do COFINS	
                            }
                            if (cofins.COFINS_CST == 3)
                            {
                                //dt.SetCampo("COFINSQtde=");//Grupo COFINS tributado por Qtde	
                                dt.SetCampo("CST_S06=" + cofins.COFINS_CST);//Código de Situação Tributária do COFINS	
                                dt.SetCampo("qBCProd_S09=" + cofins.COFINS_QBCPROD);//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_S10=" + cofins.COFINS_VALIQPROD);//Alíquota do COFINS (em reais)	
                                dt.SetCampo("vCOFINS_S11=" + cofins.COFINS_VCOFINS);//Valor do COFINS	
                            }

                            else if (pis.PIS_CST == 4 || pis.PIS_CST == 5 || pis.PIS_CST == 6 || pis.PIS_CST == 7 || pis.PIS_CST == 8 || pis.PIS_CST == 9)
                            {
                                //COFINSNT	Grupo COFINS não tributado
                                dt.SetCampo("CST_S06=" + cofins.COFINS_CST);//Código de Situação Tributária do COFINS	
                            }
                            else if (pis.PIS_CST > 49 && pis.PIS_CST < 99)
                            {
                                //Grupo COFINS outras Operações	
                                //Sequência XML	 	1-1	 	Informar os campos S07 e S08 se o cálculo do COFINS em percentual
                                dt.SetCampo("vBC_S07=" + cofins.COFINS_VBC);//Valor da Base de Cálculo do COFINS	
                                dt.SetCampo("pCOFINS_S08=" + cofins.COFINS_PCOFINS);//Alíquota do COFINS (em percentual)	
                                                                                    //Fim da Sequência XML	
                                                                                    //Sequência XML	 	1-1	 	Informar os campos S10 e S11 se o cálculo do COFINS for em valor
                                dt.SetCampo("qBCProd_S09=" + cofins.COFINS_QBCPROD);//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_S10=" + cofins.COFINS_VALIQPROD);//vAliqProd_S10
                                                                                        //Fim da Sequência XML	
                                dt.SetCampo("vCOFINS_S11=" + cofins.COFINS_VCOFINS);//Valor do COFINS	
                            }
                            else
                            {
                                //COFINSST	Grupo COFINS Substituição Tributária

                                //Sequência XML	 	1-1	 	Informar os campos T02 e T03 para cálculo do COFINS em percentual
                                dt.SetCampo("vBC_T02=" + cofins.COFINS_VBC);//Valor da Base de Cálculo do COFINS	
                                dt.SetCampo("pCOFINS_T03=" + cofins.COFINS_PCOFINS);//Alíquota do COFINS (em percentual)
                                                                                    //Fim da Sequência XML	

                                //Sequência XML	 	1-1	 	Informar os campos T04 e T05 para cálculo do COFINS em valor
                                dt.SetCampo("qBCProd_T04=" + cofins.COFINS_QBCPROD);//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_T05=" + cofins.COFINS_VALIQPROD);//Alíquota do COFINS (em reais)	
                                                                                        //Fim da Sequência XML	

                                dt.SetCampo("vCOFINS_T06=" + cofins.COFINS_VCOFINS);//Valor do COFINS	
                            }
                        }

                        //----------------ISSQN (Imposto sobre Serviços de Qualquer Natureza)-------------------------------
                        /*
                        dt.SetCampo("ISSQN=");//Grupo ISSQN	
                        dt.SetCampo("vBC_U02=");//Valor da Base de Cálculo do ISSQN	
                        dt.SetCampo("vAliq_U03=");//Alíquota do ISSQN	
                        dt.SetCampo("vISSQN_U04=");//Valor do ISSQN	
                        dt.SetCampo("cMunFG_U05=");//Código do município de ocorrência do fato gerador do ISSQN	
                        dt.SetCampo("cListServ_U06=");//Item da Lista de Serviços	
                        dt.SetCampo("vDeducao_U07=");//Valor dedução para redução da Base de Cálculo	
                        dt.SetCampo("vOutro_U08=");//Valor outras retenções	
                        dt.SetCampo("vDescIncond_U09=");//Valor desconto incondicionado	
                        dt.SetCampo("vDescCond_U10=");//Valor desconto condicionado	
                        dt.SetCampo("vISSRet_U11=");//Valor retenção ISS	
                        dt.SetCampo("indISS_U12=");//Indicador da exigibilidade do ISS	
                        dt.SetCampo("cServico_U13=");//Código do serviço prestado dentro do município	
                        dt.SetCampo("cMun_U14=");//Código do Município de incidência do imposto	
                        dt.SetCampo("cPais_U15=");//Código do País onde o serviço foi prestado	
                        dt.SetCampo("nProcesso_U16=");//Número do processo judicial ou administrativo de suspensão da exigibilidade	
                        dt.SetCampo("indIncentivo_U17=");//Indicador de incentivo Fiscal
                        */

                        //Tributos Devolvidos
                        //Informação do Imposto devolvido.O motivo da devolução deverá ser informado pela empresa no campo de Informações Adicionais do Produto (tag:infAdProd)
                        dt.SetCampo("pDevol_UA02=" + p.PDEVOL);//Percentual da mercadoria devolvida	
                        dt.SetCampo("vIPIDevol_UA04=" + p.VIPIDEVOL);//Valor do IPI devolvido	

                        //Informações Adicionais
                        if(p.PROD_INFADPROD != null && p.PROD_INFADPROD != "")
                            dt.SetCampo("infAdProd_V01=" + p.PROD_INFADPROD);//Informações Adicionais do Produto	
                        dt.SalvarItem();
                    }

                    //------------------------Total da NF-e	--------------------------------
                    //Grupo Totais referentes ao ICMS
                    dt.SetCampo("vBC_W03=" + nf.ICMSTOT_VBC);//Base de Cálculo do ICMS	
                    dt.SetCampo("vICMS_W04=" + nf.ICMSTOT_VICMS);//Valor Total do ICMS	
                    dt.SetCampo("vICMSDeson_W04a=" + nf.ICMStot_VICMSDESON);//Valor Total do ICMS desonerado	
                    dt.SetCampo("vFCPUFDest_W04c=" + nf.ICMSTOT_VFCPUFDEST);//Valor total do ICMS relativo Fundo de Combate à Pobreza (FCP) da UF de destino	
                    dt.SetCampo("vICMSUFDest_W04e=" + nf.ICMSTOT_VICMSUFDEST);//vICMSUFDest_W04e
                    dt.SetCampo("vICMSUFRemet_W04g=" + nf.ICMSTOT_VICMSUFREMET);//Valor total do ICMS Interestadual para a UF do remetente	
                    dt.SetCampo("vFCP_W04h=" + nf.ICMSTOT_VFCP);//Valor Total do FCP (Fundo de Combate à Pobreza)	
                    dt.SetCampo("vBCST_W05=" + nf.ICMSTOT_VBCST);//Base de Cálculo do ICMS ST	
                    dt.SetCampo("vST_W06=" + nf.ICMSTOT_VST);//Valor Total do ICMS ST	
                    dt.SetCampo("vFCPST_W06a=" + nf.ICMSTOT_VFCPST);//Valor Total do FCP (Fundo de Combate à Pobreza) retido por substituição tributária	
                    dt.SetCampo("vFCPSTRet_W06b=" + nf.ICMSTOT_VFCPSTRET);//Valor Total do FCP retido anteriormente por Substituição Tributária	
                    dt.SetCampo("vProd_W07=" + nf.ICMSTOT_VPROD);//Valor Total dos produtos e serviços	
                    dt.SetCampo("vFrete_W08=" + nf.ICMSTOT_VFRETE);//Valor Total do Frete	
                    dt.SetCampo("vSeg_W09=" + nf.ICMSTOT_VSEG);//Valor Total do Seguro	
                    dt.SetCampo("vDesc_W10=" + nf.ICMSTOT_VDESC);//Valor Total do Desconto	
                    dt.SetCampo("vII_W11=" + nf.ICMSTOT_VII);//Valor Total do II	
                    dt.SetCampo("vIPI_W12=" + nf.ICMSTOT_VIPI);//Valor Total do IPI	
                    dt.SetCampo("vIPIDevol_W12a=" + nf.ICMSTOT_VIPIDEVOL);//Valor Total do IPI Devolvido	
                    dt.SetCampo("vPIS_W13=" + nf.ICMSTOT_VPIS);//Valor Total do PIS	
                    dt.SetCampo("vCOFINS_W14=" + nf.ICMSTOT_VCOFINS);//Valor Total do PIS	
                    dt.SetCampo("vOutro_W15=" + nf.ICMSTOT_VOUTRO);//Valor total de Outras despesas	
                    dt.SetCampo("vNF_W16=" + nf.ICMSTOT_VNF);//Valor Total da NF-e	
                    dt.SetCampo("vTotTrib_W16a=" + nf.VTOTTRIB);//Valor aproximado total de tributos federais, estaduais e municipais.	

                    /*
                    //Grupo Totais referentes ao ISSQN
                    dt.SetCampo("vServ_W18=" + nf.ISSQNTOT_VSERV);//Valor total dos Serviços sob não-incidência ou não tributados pelo ICMS	
                    dt.SetCampo("vBC_W19=" + nf.ISSQNTOT_VBC);//Valor total Base de Cálculo do ISS	
                    dt.SetCampo("vISS_W20=" + nf.ISSQNTOT_VISS);//Valor total do ISS	
                    dt.SetCampo("vPIS_W21=" + nf.ISSQNTOT_VPIS);//Valor total do PIS sobre serviços	
                    dt.SetCampo("vCOFINS_W22=" + nf.ISSQNTOT_VCOFINS);//Valor total da COFINS sobre serviços	
                    dt.SetCampo("dCompet_W22a=" + nf.ISSQNTOT_DCOMPET);//Data da prestação do serviço	
                    dt.SetCampo("vDeducao_W22b=" + nf.ISSQNTOT_VDEDUCAO);//Valor total dedução para redução da Base de Cálculo	
                    dt.SetCampo("vOutro_W22c=" + nf.ISSQNTOT_VOUTRO);//Valor total outras retenções	
                    dt.SetCampo("vDescIncond_W22d=" + nf.ISSQNTOT_VDESCINCOND);//Valor total desconto incondicionado	
                    dt.SetCampo("vDescCond_W22e=" + nf.ISSQNTOT_VDESCCOND);//Valor total desconto condicionado
                    dt.SetCampo("vISSRet_W22f=" + nf.ISSQNTOT_VISSRET);//Valor total retenção ISS	
                    dt.SetCampo("cRegTrib_W22g=" + nf.ISSQNTOT_CREGTRIB);//Código do Regime Especial de Tributação	
                    */

                    //Grupo Retenções de Tributos	
                    dt.SetCampo("vRetPIS_W24=" + nf.RETTRIB_VRETPIS);//Valor Retido de PIS	
                    dt.SetCampo("vRetCOFINS_W25=" + nf.RETTRIB_VRETCOFINS);//Valor Retido de COFINS	
                    dt.SetCampo("vRetCSLL_W26=" + nf.RETTRIB_VRETCSLL);//Valor Retido de CSLL	
                    dt.SetCampo("vBCIRRF_W27=" + nf.RETTRIB_VBCIRRF);//Base de Cálculo do IRRF	
                    dt.SetCampo("vIRRF_W28=" + nf.RETTRIB_VIRRF);//Valor Retido do IRRF	
                    dt.SetCampo("vBCRetPrev_W29=" + nf.RETTRIB_VBCRETPREV);//Base de Cálculo da Retenção da Previdência Social	
                    dt.SetCampo("vRetPrev_W30=" + nf.RETTRIB_VRETPREV);//Valor da Retenção da Previdência Social	

                    //-------------------------------------------------------------------------------------------------------------
                    //Informações do Transporte da NF-e
                    TECNO_NF_TRANSP_TRANSPORTA transp = new TECNO_NF_TRANSP_TRANSPORTA();
                    TECNO_NF_TRANSP_TRANSPORTA_DAL transpDAL = new TECNO_NF_TRANSP_TRANSPORTA_DAL();
                    transp = transpDAL.PesquisarTECNO_NF_TRANSP_TRANSPORTA(nf.ID_NOTA_FISCAL);
                    if (transp != null)
                    {
                        dt.SetCampo("modFrete_X02=" + nf.TRANSP_MODFRETE);//Modalidade do frete	
                                                                          //Grupo Transportador	
                        if (transp.TRANSPORTA_CNPJ_CPF.Length > 11)
                            dt.SetCampo("CNPJ_X04=" + transp.TRANSPORTA_CNPJ_CPF);//CNPJ do Transportador	
                        else
                            dt.SetCampo("CPF_X05=" + transp.TRANSPORTA_CNPJ_CPF);//CPF do Transportador	
                        dt.SetCampo("xNome_X06=" + transp.TRANSPORTA_XNOME);//Razão Social ou nome	
                        dt.SetCampo("IE_X07=" + transp.TRANSPORTA_IE);//Inscrição Estadual do Transportador	
                        dt.SetCampo("xEnder_X08=" + transp.TRANSPORTA_XENDER);//Endereço Completo	
                        dt.SetCampo("xMun_X09=" + transp.TRANSPORTA_XMUN);//Nome do município	
                        dt.SetCampo("UF_X10=" + transp.TRANSPORTA_UF);//Sigla da UF	
                    }

                    //-------------------------------------------------------------------------------------------------------------
                    //Grupo Retenção ICMS transporte
                    TECNO_NF_TRANSP_RETTRANSP RetTransp = new TECNO_NF_TRANSP_RETTRANSP();
                    TECNO_NF_TRANSP_RETTRANSP_DAL RetTranspDAL = new TECNO_NF_TRANSP_RETTRANSP_DAL();
                    RetTransp = RetTranspDAL.PesquisarTECNO_NF_TRANSP_RETTRANSP(nf.ID_NOTA_FISCAL);
                    if (RetTransp != null)
                    {
                        dt.SetCampo("vServ_X12=" + RetTransp.RETTRANSP_VSERV);//Valor do Serviço	
                        dt.SetCampo("vBCRet_X13=" + RetTransp.RETTRANSP_VBCRET);//BC da Retenção do ICMS	
                        dt.SetCampo("pICMSRet_X14=" + RetTransp.RETTRANSP_PICMSRET);//Alíquota da Retenção	
                        dt.SetCampo("vICMSRet_X15=" + RetTransp.RETTRANSP_VICMSRET);//Valor do ICMS Retido	
                        dt.SetCampo("CFOP_X16=" + RetTransp.RETTRANSP_CFOP);//CFOP
                        dt.SetCampo("cMunFG_X17=" + RetTransp.RETTRANSP_CMUNFG);//Código do município de ocorrência do fato gerador do ICMS do transporte	
                    }

                    //-------------------------------------------------------------------------------------------------------------
                    //Grupo Veículo Transporte	
                    TECNO_NF_TRANSP_VEIC veic = new TECNO_NF_TRANSP_VEIC();
                    TECNO_NF_TRANSP_VEIC_DAL veicDAL = new TECNO_NF_TRANSP_VEIC_DAL();
                    veic = veicDAL.PesquisarTECNO_NF_TRANSP_VEIC(nf.ID_NOTA_FISCAL);
                    if (veic != null)
                    {
                        dt.SetCampo("placa_X19=" + veic.VEICTRANSP_PLACA);//Placa do Veículo	
                        dt.SetCampo("UF_X20=" + veic.VEICTRANSP_UF);//Sigla da URegistro Nacional de Transportador de Carga (ANTT)	F	
                        dt.SetCampo("RNTC_X21=" + veic.VEICTRANSP_RNTC);//Registro Nacional de Transportador de Carga (ANTT)	
                    }

                    //-------------------------------------------------------------------------------------------------------------
                    //Grupo Reboque	
                    TECNO_NF_TRANSP_REBOQUE reboque = new TECNO_NF_TRANSP_REBOQUE();
                    TECNO_NF_TRANSP_REBOQUE_DAL reboqueDAL = new TECNO_NF_TRANSP_REBOQUE_DAL();
                    reboque = reboqueDAL.PesquisarTECNO_NF_TRANSP_REBOQUE(nf.ID_NOTA_FISCAL);
                    if (reboque != null)
                    {
                        dt.IncluirParte("REBOQUE");
                        dt.SetCampo("placa_X23=" + reboque.REBOQUE_PLACA);//Placa do Veículo	
                        dt.SetCampo("UF_X24=" + reboque.REBOQUE_UF);//Sigla da UF	
                        dt.SetCampo("RNTC_X25=" + reboque.REBOQUE_RNTC);//Registro Nacional de Transportador de Carga (ANTT)	
                                                                        //dt.SetCampo("vagao_X25a=" + reboque.);//Identificação do vagão	
                                                                        //dt.SetCampo("balsa_X25b=");//Identificação da balsa	
                        dt.SalvarParte("REBOQUE");
                    }

                    //-------------------------------------------------------------------------------------------------------------
                    //Grupo Volumes	

                    TECNO_NF_TRANSP_VOLUMES vol = new TECNO_NF_TRANSP_VOLUMES();
                    TECNO_NF_TRANSP_VOLUMES_DAL volDAL = new TECNO_NF_TRANSP_VOLUMES_DAL();
                    vol = volDAL.PesquisarTECNO_NF_TRANSP_VOLUMES(nf.ID_NOTA_FISCAL);
                    if (vol != null)
                    {
                        dt.IncluirParte("VOL");
                        dt.SetCampo("qVol_X27=" + vol.VOL_QVOL);//Quantidade de volumes transportados	
                        dt.SetCampo("esp_X28=" + vol.VOL_ESP);//Espécie dos volumes transportados	
                        dt.SetCampo("marca_X29=" + vol.VOL_MARCA);//Marca dos volumes transportados	
                        dt.SetCampo("nVol_X30=" + vol.VOL_NVOL);//Numeração dos volumes transportados	
                        dt.SetCampo("pesoL_X31=" + vol.VOL_PESOL);//Peso Líquido (em kg)	
                        dt.SetCampo("pesoB_X32=" + vol.VOL_PESOB);//Peso Bruto (em kg)	
                        dt.SalvarParte("VOL");
                    }

                    //-------------------------------------------------------------------------------------------------------------
                    //Grupo Lacres
                    dt.IncluirParte("LACRE");
                    dt.SetCampo("nLacre_X34=");//Número dos Lacres	
                    dt.SalvarParte("LACRE");


                    //-------------------------------------------------------------------------------------------------------------
                    //Dados da Cobrança
                    //Grupo Fatura
                    dt.SetCampo("nFat_Y03=" + nf.FAT_NFAT);//Número da Fatura	
                    dt.SetCampo("vOrig_Y04=" + nf.FAT_VORIG);//Valor Original da Fatura	
                    dt.SetCampo("vDesc_Y05=" + nf.FAT_VDESC);//Valor do desconto	
                    dt.SetCampo("vLiq_Y06=" + nf.FAT_VLIQ);//Valor Líquido da Fatura	

                    TECNO_NF_DUPLICATA dup = new TECNO_NF_DUPLICATA();
                    TECNO_NF_DUPLICATA_DAL dupDAL = new TECNO_NF_DUPLICATA_DAL();
                    dup = dupDAL.PesquisarTECNO_NF_DUPLICATA(nf.ID_NOTA_FISCAL);
                    if (dup != null)
                    {
                        //Grupo Parcelas
                        dt.IncluirParte("DUP");
                        dt.SetCampo("nDup_Y08=" + dup.DUP_NDUP);//Número da Parcela	
                        dt.SetCampo("dVenc_Y09=" + dup.DUP_DVENC);//Data de vencimento	
                        dt.SetCampo("vDup_Y10=" + dup.DUP_VDUP);//Valor da Parcela	
                        dt.SalvarParte("DUP");
                    }

                    TECNO_NF_FORMA_PGTO pgto = new TECNO_NF_FORMA_PGTO();
                    TECNO_NF_FORMA_PGTO_DAL pgtoDAL = new TECNO_NF_FORMA_PGTO_DAL();
                    pgto = pgtoDAL.PesquisarTECNO_NF_FORMA_PGTO(nf.ID_NOTA_FISCAL);
                    if (pgto != null)
                    {
                        //Informações de Pagamento
                        //Grupo Detalhamento do Pagamento
                        dt.IncluirParte("YA");
                        dt.SetCampo("indPag_YA01b=" + pgto.INDPAG);//Indicador da Forma de Pagamento	
                        dt.SetCampo("tPag_YA02=" + pgto.TPAG);//Meio de pagamento	
                        dt.SetCampo("vPag_YA03=" + pgto.VPAG);//Valor do Pagamento

                        TECNO_NF_FORMA_PGTO_CARTAO pgtoCartao = new TECNO_NF_FORMA_PGTO_CARTAO();
                        TECNO_NF_FORMA_PGTO_CARTAO_DAL pgtoCartaoDAL = new TECNO_NF_FORMA_PGTO_CARTAO_DAL();
                        pgtoCartao = pgtoCartaoDAL.PesquisarTECNO_NF_FORMA_PGTO_CARTAO(nf.ID_NOTA_FISCAL);
                        if (pgtoCartao != null)
                        {
                            //Grupo de Cartões	
                            dt.SetCampo("tpIntegra_YA04a=" + pgtoCartao.TPINTEGRA);//Tipo de Integração para pagamento	
                            dt.SetCampo("CNPJ_YA05=" + pgtoCartao.CNPJ);//CNPJ da instituição de pagamento	
                            dt.SetCampo("tBand_YA06=" + pgtoCartao.TBAND);//Código da bandeira da operadora de cartão de crédito e/ou débito	
                            dt.SetCampo("cAut_YA07=" + pgtoCartao.CAUT);//Número de autorização da operação cartão de crédito e/ou débito	
                        }
                        dt.SalvarParte("YA");
                    }

                    dt.SetCampo("vTroco_YA09=" + nf.VTROCO);//Valor do troco	

                    //Informações do Intermediador da Transação
                    if(nf.INFINTERMED_CNPJ != null && nf.INFINTERMED_CNPJ != "")
                        dt.SetCampo("CNPJ_YB02=" + nf.INFINTERMED_CNPJ);//CNPJ do Intermediador da Transação (agenciador, plataforma de delivery, marketplace e similar) de serviços e de negócios.	
                    if (nf.INFINTERMED_IDCADINTTRAN != null && nf.INFINTERMED_IDCADINTTRAN != "")
                        dt.SetCampo("idCadIntTran_YB03=" + nf.INFINTERMED_IDCADINTTRAN);//Identificador cadastrado no intermediador

                    //Informações Adicionais da NF-e
                    dt.SetCampo("infAdFisco_Z02=" + nf.INFADIC_INFADFISCO);//Informações Adicionais de Interesse do Fisco	
                    dt.SetCampo("infCpl_Z03=" + nf.INFADIC_INFCPL);//Informações Complementares de interesse do Contribuinte	

                    //Grupo Campo de uso livre do contribuinte
                    dt.IncluirParte("OBSCONT");
                    dt.SetCampo("xCampo_Z05=");//Identificação do campo	
                    dt.SetCampo("xTexto_Z06=");//Conteúdo do campo	
                    dt.SalvarParte("OBSCONT");

                    //Grupo Campo de uso livre do fisco
                    dt.IncluirParte("OBSFISCO");
                    dt.SetCampo("xCampo_Z08=");//Identificação do campo	
                    dt.SetCampo("xTexto_Z09=");//Conteúdo do campo	
                    dt.SalvarParte("OBSFISCO");

                    //Grupo Processo Referenciado
                    dt.IncluirParte("PROCREF");
                    dt.SetCampo("nProc_Z11=");//Identificador do processo ou ato concessório	
                    dt.SetCampo("indProc_Z12=");//Indicador da origem do processo	
                    dt.SalvarParte("PROCREF");

                    //Grupo Exportação	
                    dt.SetCampo("UFSaidaPais_ZA02=" + nf.EXPORTA_UFSAIDAPAIS);//Sigla da UF de Embarque ou de transposição de fronteira	
                    dt.SetCampo("xLocExporta_ZA03=" + nf.EXPORTA_XLOCEXPORTA);//Descrição do Local de Embarque ou de transposição de fronteira	
                    dt.SetCampo("xLocDespacho_ZA04=" + nf.EXPORTA_XLOCDESPACHO);//Descrição do local de despacho	

                    //Grupo Compra	
                    dt.SetCampo("xNEmp_ZB02=" + nf.COMPRA_XNEMP);//Nota de Empenho	
                    dt.SetCampo("xPed_ZB03=" + nf.COMPRA_XPED);//Pedido
                    dt.SetCampo("xCont_ZB04=" + nf.COMPRA_XCONT);//Contrato

                    //Informações do Registro de Aquisição de Cana	
                    //dt.SetCampo("safra_ZC02=");//Identificação da safra	
                    //dt.SetCampo("ref_ZC03=");//Mês e ano de referência	

                    //Grupo Fornecimento diário de cana	
                    /*
                    dt.IncluirParte("FORDIA");
                    dt.SetCampo("dia_ZC05=");//Dia
                    dt.SetCampo("qtde_ZC06=");//Quantidade
                    dt.SetCampo("qTotMes_ZC07=");//Quantidade Total do Mês	
                    dt.SetCampo("qTotAnt_ZC08=");//Quantidade Total Anterior	
                    dt.SetCampo("qTotGer_ZC09=");//Quantidade Total Geral	
                    dt.SalvarParte("FORDIA");
                    */
                    /*
                    //Grupo Deduções – Taxas e Contribuições	
                    dt.IncluirParte("DEDUC");
                    dt.SetCampo("xDed_ZC11=");//Descrição da Dedução	
                    dt.SetCampo("vDed_ZC12=");//Valor da Dedução	
                    dt.SetCampo("vFor_ZC13=");//Valor dos Fornecimentos	
                    dt.SetCampo("vTotDed_ZC14=");//Valor Total da Dedução	
                    dt.SetCampo("vLiqFor_ZC15=");//Valor Líquido dos Fornecimentos	
                    dt.SalvarParte("DEDUC");
                    */

                    //Informações do Responsável Técnico
                    dt.SetCampo("CNPJ_ZD02=");//CNPJ da pessoa jurídica responsável pelo sistema utilizado na emissão do documento fiscal eletrônico	
                    dt.SetCampo("xContato_ZD04=");//Nome da pessoa a ser contatada	
                    dt.SetCampo("email_ZD05=");//E-mail da pessoa jurídica a ser contatada	
                    dt.SetCampo("fone_ZD06=");//Telefone da pessoa jurídica/física a ser contatada	

                    //Sequência XML	- Grupo de informações do Código de Segurança do Responsável Técnico -CSTR
                    dt.SetCampo("idCSRT_ZD08=");//Identificador do CSRT	
                    dt.SetCampo("hashCSRT_ZD09=");//Hash do CSRT	
                    //Fim da Sequência XML
                    dt.Salvar();

                    blnGerouXML = true;
                    return dt.LoteNFe;
                    
                }

                blnGerouXML = false;
                return "";
            }
            catch(Exception ex)
            {
                blnGerouXML = false;
                return ex.Message;

            }

        }

        public void GerandoArquivoLog(string strDescrição, int CodCaminho)
        {
            try
            {
                DateTime data = DateTime.Now;

                string CaminhoArquivoLog = "";
                if (CodCaminho == 1)//HabilServiceNFSe
                    CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\..\\..\\..\\..\\Modulos\\Log\\NFe\\";
                else//HabilInformatica
                    CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\NFe\\";

                if (!Directory.Exists(CaminhoArquivoLog))
                    Directory.CreateDirectory(CaminhoArquivoLog);

                CaminhoArquivoLog += "Log - " + data.ToString("dd-MM-yyyy") + ".txt";

                if (!System.IO.File.Exists(CaminhoArquivoLog))
                {
                    FileStream file = new FileStream(CaminhoArquivoLog, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(file);
                    bw.Close();
                }

                string nomeArquivo = CaminhoArquivoLog;
                System.IO.TextWriter arquivo = System.IO.File.AppendText(nomeArquivo);

                // Agora é só sair escrevendo
                arquivo.WriteLine(data.ToString("HH:mm:ss") + " - " + strDescrição);

                arquivo.Close();
            }
            catch (Exception ex)
            {
                GerandoArquivoLog(ex.Message, 1);
            }
        }
    }
}