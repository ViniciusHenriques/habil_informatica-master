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
using System.Data;

namespace DAL.Persistence
{
    public class NFeFuncoes
    {
        private NFeX.spdNFeX spdNFe = new NFeX.spdNFeX();

        private NFeDataSetX.spdNFeDataSetX dt = new spdNFeDataSetX();

        private NFeX.spdNFeSCAN spdSCAN = new NFeX.spdNFeSCAN();

        private NFeX.spdNFeDPECX spdDPEC = new spdNFeDPECX();

        private bool ConfigurandoComponente(TECNO_EVENTO_ERP TCN_EVENTO)
        {
            try
            {
                string strVersaoManual = "", strVersaoManualDPEC_SCAN = "";

                if (TCN_EVENTO.VERSAO_MANUAL == "" || TCN_EVENTO.VERSAO_MANUAL == null)
                    strVersaoManual = "6.0";
                else
                    strVersaoManual = TCN_EVENTO.VERSAO_MANUAL;

                if (TCN_EVENTO.VERSAO_MANUAL_DPEC_SCAN == "" || TCN_EVENTO.VERSAO_MANUAL_DPEC_SCAN == null)
                    strVersaoManualDPEC_SCAN = "5.0";
                else
                    strVersaoManualDPEC_SCAN = TCN_EVENTO.VERSAO_MANUAL_DPEC_SCAN;

                spdNFe.ConfigurarSoftwareHouse("02048638000128", "");
               
                if (TCN_EVENTO.CERTIFICADO_CAMINHO != "")
                {
                    Estado est = new Estado();
                    EstadoDAL estDAL = new EstadoDAL();
                    est = estDAL.PesquisarEstado(Convert.ToInt32(TCN_EVENTO.CODIGO_UF));

                    string CaminhoArquivo = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\..\\..\\..\\..\\Modulos\\TecnoSpeed\\NFe\\Arquivos";

                    spdNFe.UF = est.Sigla;
                    spdNFe.CNPJ = TCN_EVENTO.CNPJ_EVENTO;
                    spdNFe.CaminhoCertificado = TCN_EVENTO.CERTIFICADO_CAMINHO;
                    spdNFe.SenhaCertificado = TCN_EVENTO.CERTIFICADO_SENHA;
                    spdNFe.ArquivoServidoresHom = CaminhoArquivo + @"\nfeServidoresHom.ini";
                    spdNFe.ArquivoServidoresProd = CaminhoArquivo + @"\nfeServidoresProd.ini";
                    spdNFe.DiretorioEsquemas = CaminhoArquivo + @"\Esquemas\";
                    spdNFe.DiretorioTemplates = CaminhoArquivo + @"\Templates\";
                    spdNFe.DiretorioLog = CaminhoArquivo + @"\Log\";
                    spdNFe.DiretorioXmlDestinatario = CaminhoArquivo + @"\XML_Destinatario";
                    spdNFe.TipoCertificado = TipoCertificado.ckLocalMashine;
                    spdNFe.VersaoManual = strVersaoManual;
                    spdNFe.DiretorioLogErro = CaminhoArquivo + @"\LogErro\";
                    spdNFe.HttpLibs = "wininet,sbb";
                    
                    //-------------AMBIENTE----------------
                    if (TCN_EVENTO.TPAMB == 1)
                        spdNFe.Ambiente = Ambiente.akHomologacao;
                    else
                        spdNFe.Ambiente = Ambiente.akHomologacao;

                    //-----------MODO OPERACAO-------------
                    if (TCN_EVENTO.TP_MODO_OPERACAO == 3)
                        spdNFe.ModoOperacao = ModoOperacaoNFe.moEPEC;
                    else if (TCN_EVENTO.TP_MODO_OPERACAO == 1)
                        spdNFe.ModoOperacao = ModoOperacaoNFe.moSVCAN;
                    else if (TCN_EVENTO.TP_MODO_OPERACAO == 2)
                        spdNFe.ModoOperacao = ModoOperacaoNFe.moSVCRS;
                    else
                        spdNFe.ModoOperacao = ModoOperacaoNFe.moNormal;


                    //-----------VALIDAR ESQUEMA-----------
                    if (TCN_EVENTO.IN_VALIDAR_ESQUEMA == 1)
                        spdNFe.ValidarEsquemaAntesEnvio = true;
                    else
                        spdNFe.ValidarEsquemaAntesEnvio = false;


                    //-----------REMOVER ACENTOS-----------
                    if (TCN_EVENTO.IN_REMOVER_ACENTOS == 1)
                        spdNFe.CaracteresRemoverAcentos = "áéíóúàèìòùâêîôûäëïöüãõñçÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÄËÏÖÜÃÕÑÇºª";

                    //[SCAN]
                    spdSCAN.UF = est.Sigla;
                    spdSCAN.CNPJ = TCN_EVENTO.CNPJ_EVENTO;
                    spdSCAN.ArquivoServidoresHom = "nfeServidoresHomSCAN.ini";
                    spdSCAN.DiretorioEsquemas = CaminhoArquivo + @"\Esquemas\";
                    spdSCAN.DiretorioTemplates = CaminhoArquivo + @"\Templates\";
                    spdSCAN.DiretorioLog = CaminhoArquivo + @"\Log\";
                    spdSCAN.TipoCertificado = TipoCertificado.ckLocalMashine;
                    spdSCAN.CaminhoCertificado = TCN_EVENTO.CERTIFICADO_CAMINHO;
                    spdSCAN.SenhaCertificado = TCN_EVENTO.CERTIFICADO_SENHA;
                    spdSCAN.VersaoManual = strVersaoManualDPEC_SCAN;

                    //[DPEC]
                    spdDPEC.UF = est.Sigla;
                    spdDPEC.CNPJ = TCN_EVENTO.CNPJ_EVENTO;
                    spdDPEC.ArquivoServidoresHom = "nfeServidoresHomDPEC.ini";
                    spdDPEC.DiretorioEsquemas = CaminhoArquivo + @"\Esquemas\";
                    spdDPEC.DiretorioTemplates = CaminhoArquivo + @"\Templates\";
                    spdDPEC.DiretorioLog = CaminhoArquivo + @"\Log\";
                    spdDPEC.TipoCertificado = TipoCertificado.ckLocalMashine;
                    spdDPEC.CaminhoCertificado = TCN_EVENTO.CERTIFICADO_CAMINHO;
                    spdDPEC.SenhaCertificado = TCN_EVENTO.CERTIFICADO_SENHA;
                    spdDPEC.VersaoManual = strVersaoManualDPEC_SCAN;

                    //[MAIL]                    
                    spdNFe.EmailServidor = TCN_EVENTO.CONFIG_EMAIL_SERVIDOR;
                    spdNFe.EmailRemetente = TCN_EVENTO.CONFIG_EMAIL_REMETENTE;
                    spdNFe.EmailDestinatario = TCN_EVENTO.CONFIG_EMAIL_DESTINATARIO;
                    spdNFe.EmailAssunto = TCN_EVENTO.CONFIG_EMAIL_ASSUNTO;
                    spdNFe.EmailMensagem = TCN_EVENTO.CONFIG_EMAIL_MENSAGEM;
                    spdNFe.EmailUsuario = TCN_EVENTO.CONFIG_EMAIL_USUARIO;
                    spdNFe.EmailSenha = TCN_EVENTO.CONFIG_EMAIL_SENHA;

                    if (TCN_EVENTO.CONFIG_EMAIL_AUTENTICACAO == 1)
                        spdNFe.EmailAutenticacao = true;
                    else
                        spdNFe.EmailAutenticacao = false;

                    spdNFe.EmailPorta = TCN_EVENTO.CONFIG_EMAIL_PORTA;
                    spdNFe.EmailTimeOut = 600000;
                    
                    
                    //[DANFE]
                    spdNFe.LogotipoEmitente = CaminhoArquivo + "\\Logotipo\\LogoDaEmpresa.jpg";
                    spdNFe.FraseContingencia = "DANFE em contingência";
                    spdNFe.FraseHomologacao = "SEM VALOR FISCAL";
                    spdNFe.ModeloRetrato = spdNFe.DiretorioTemplates + "\\Danfe\\retrato.rtm";
                    spdNFe.ModeloPaisagem = spdNFe.DiretorioTemplates + "\\Danfe\\paisagem.rtm";
                    spdNFe.QtdeCopias = Convert.ToInt32(TCN_EVENTO.COPIAS_IMPRESSAO);

                    return true;
                }
                else
                {
                    GerandoArquivoLog("Caminho do certificado não foi informado CNPJ " + TCN_EVENTO.CNPJ_EVENTO, 1);
                    return false;
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro ao configurar componente..." + TCN_EVENTO.CNPJ_EVENTO + " - " + ex.Message, 1);
                return false;
            }
        }

        public void EnviarEvento()
        {
            List<TECNO_EVENTO_ERP> LISTA_TCN_EVENTO = new List<TECNO_EVENTO_ERP>();

            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            LISTA_TCN_EVENTO = TCN_EVENTO_DAL.ListarTECNO_EVENTO_ERP();

            if (LISTA_TCN_EVENTO.Count() > 0)
            {
                GerandoArquivoLog("", 1);

                GerandoArquivoLog("--------------" + LISTA_TCN_EVENTO.Count() + " solicitação(ções)--------------", 1);
            }

            foreach (var TCN_EVENTO in LISTA_TCN_EVENTO)
            {
                GerandoArquivoLog("-----------------------------------------------", 1);

                GerandoArquivoLog("Configurando componente... CNPJ: " + TCN_EVENTO.CNPJ_EVENTO + "; UF: " + TCN_EVENTO.CODIGO_UF, 1);

                if (ConfigurandoComponente(TCN_EVENTO))
                {
                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "L", TCN_EVENTO.ID_EVENTO_ERP);

                    GerandoArquivoLog("Componente configurado...", 1);

                    if (TCN_EVENTO.TP_ACAO == 1)
                    {
                        GerandoArquivoLog("Solicitação de envio de NF-e -  CHAVE_BUSCA: " + TCN_EVENTO.CHAVE_BUSCA, 1);

                        EnviarNFe(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA),TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 2)
                    {
                        GerandoArquivoLog("Solicitação de cancelamento de NF-e, chave: " + TCN_EVENTO.CHNFE, 1);

                        CancelarNFe(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 3)
                    {
                        GerandoArquivoLog("Solicitação de inutilização de NF-e, chave: " + TCN_EVENTO.CHNFE, 1);

                        InutilizarNFe(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 4)
                    {
                        GerandoArquivoLog("Solicitação de consulta de NF-e, chave: " + TCN_EVENTO.CHNFE, 1);

                        ConsultarNFe(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 5)
                    {
                        GerandoArquivoLog("Solicitação de Consulta Contribuinte de ICMS", 1);

                        ConsultarContribuinteICMS(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 6)
                    {
                        GerandoArquivoLog("Solicitação de autorização Nota Enviada em Contingência ", 1);

                        //InutilizarNFe(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 7)
                    {
                        GerandoArquivoLog("Solicitação de Reimpressao NF-e " + TCN_EVENTO.CHNFE, 1);

                        ImprimirDANFE(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 8)
                    {
                        GerandoArquivoLog("Solicitação de envio nota em contingência", 1);

                        //EnviarNFe_DPEC(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 9)
                    {
                        GerandoArquivoLog("Solicitação de envio em contingência DPEC ", 1);

                        EnviarNFe_DPEC(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 12)
                    {
                        GerandoArquivoLog("Solicitação de reenviar email", 1);

                        EnviarEmailDestinatario(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 1000)
                    {
                        GerandoArquivoLog("Solicitação de envio de carta de correção", 1);

                        EnviarCartaCorrecao(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 1001)
                    {
                        GerandoArquivoLog("Solicitação de envio de pedido de prorrogação", 1);

                        EnviarPedidoProrrogacao(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 1002)
                    {
                        GerandoArquivoLog("Solicitação de envio de manifestação de destinatário", 1);

                        EnviarManifestacaoDestinatario(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 1003)
                    {
                        GerandoArquivoLog("Solicitação de salvar anexos", 1);

                        SalvarAnexoXML_PDF(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA),TCN_EVENTO, spdNFe.DiretorioXmlDestinatario + "\\" + TCN_EVENTO.CHNFE + "-nfe.xml", "XML AUTORIZAÇÃO NF-E", "PDF AUTORIZAÇÃO NF-E", true);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 1004)
                    {
                        GerandoArquivoLog("Solicitação de consulta de notas emitidas contra CNPJ " + TCN_EVENTO.CNPJ_EVENTO, 1);

                        ConsultarNFEmitidasContraCNPJ(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                    else if (TCN_EVENTO.TP_ACAO == 1005)
                    {
                        GerandoArquivoLog("Solicitação de consulta de nota emitida contra CNPJ "+TCN_EVENTO.CNPJ_EVENTO+", NF - " + TCN_EVENTO.CHNFE, 1);

                        ConsultarNFEspecificaEmitidaContraCNPJ(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO);
                    }
                }
            }           
        }

        // TP_ACAO = 1
        private void EnviarNFe(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            bool blnAutorizouNF = false;

            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                TECNO_NF nf = new TECNO_NF();

                TECNO_NF_DAL nfDAL = new TECNO_NF_DAL();

                nf = nfDAL.ListarTECNO_NF(decID_NOTA_FISCAL);

                blnAutorizouNF = false;

                GerandoArquivoLog("Preparando envio da NF-e ID_NOTA_FISCAL: " + nf.ID_NOTA_FISCAL + "; NRO: " + nf.IDE_NNF + "; SÉRIE: " + nf.IDE_SERIE + ";", 1);

                string strXML = "", strXMLAssinado = "", strXMLRetornoEnvio = "", strNumeroRecibo = "", strStatusRetornoRecibo = "", strStatusRetornoNF = "", strMensagemRetorno = "", strChaveAcessoNF = "", strXMLRetornoConsulta, strProtocolo = "" ;

                bool blnGerouXML = false;

                strXML = MontarXML(nf, TCN_EVENTO, ref blnGerouXML);

                if (blnGerouXML)
                {    
                    GerandoArquivoLog("Gerou XML da nota fiscal com sucesso... Preparando para assinar XML", 1);

                    strXMLAssinado = spdNFe.AssinarNota(strXML);

                    SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_EVENTO.CHAVE_BUSCA + "-xml-assinado.xml", "ENVIO NF ASSINADO", TCN_EVENTO);

                    GerandoArquivoLog("XML assinado com sucesso, preparando para enviar NFe", 1);

                    strXMLRetornoEnvio = spdNFe.EnviarNF(TCN_EVENTO.NR_LOTE.ToString(), strXMLAssinado, false);

                    SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_EVENTO.CHAVE_BUSCA + "-xml-envio.xml", "RETORNO ENVIO NF", TCN_EVENTO);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                    GerandoArquivoLog("NFe enviada, verificando status...", 1);

                    strStatusRetornoRecibo = LerTagXML(strXMLRetornoEnvio, "cStat", "retEnviNFe");
                            
                    if (strStatusRetornoRecibo == "103")
                    {
                        strNumeroRecibo = LerTagXML(strXMLRetornoEnvio, "nRec", "infRec");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("NREC", strNumeroRecibo, TCN_EVENTO.ID_EVENTO_ERP);

                        TCN_EVENTO.NREC = Convert.ToDecimal(strNumeroRecibo);

                        GerandoArquivoLog("Recibo Verificado (" + strNumeroRecibo + "), consultando número de recibo", 1);

                        strXMLRetornoEnvio = ConsultarRecibo(strNumeroRecibo, TCN_EVENTO);

                        strChaveAcessoNF = LerTagXML(strXMLRetornoEnvio, "chNFe", "infProt");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("CHNFE", strChaveAcessoNF, TCN_EVENTO.ID_EVENTO_ERP);

                        TCN_EVENTO.CHNFE = strChaveAcessoNF;

                        GerandoArquivoLog("Recibo consultado, chave de acesso " + strChaveAcessoNF, 1);

                        strStatusRetornoNF = LerTagXML(strXMLRetornoEnvio, "cStat", "infProt");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetornoNF, TCN_EVENTO.ID_EVENTO_ERP);

                        strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "infProt");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

                        if (strStatusRetornoNF == "100")
                        {
                            blnAutorizouNF = true;
                            
                            strProtocolo = LerTagXML(strXMLRetornoEnvio, "nProt", "infProt");

                            GerandoArquivoLog("Protocolo: " + strProtocolo, 1);

                            TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("NPROT", strProtocolo, TCN_EVENTO.ID_EVENTO_ERP);

                            TCN_EVENTO.NPROT = Convert.ToDecimal(strProtocolo);

                            GerandoArquivoLog(strMensagemRetorno, 1);

                            TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "1", TCN_EVENTO.ID_EVENTO_ERP);

                            strXMLRetornoConsulta = spdNFe.ConsultarNF(strChaveAcessoNF);

                            GerandoArquivoLog("Preparando para enviar e-mail para destinatário " + TCN_EVENTO.CONFIG_EMAIL_DESTINATARIO, 1);

                            EnviarEmailDestinatario(decID_NOTA_FISCAL, TCN_EVENTO);

                            GerandoArquivoLog("Email enviado, imprimindo DANFE..." , 1);

                            ImprimirDANFE(decID_NOTA_FISCAL, TCN_EVENTO);

                            SalvarAnexoXML_PDF(decID_NOTA_FISCAL, TCN_EVENTO, spdNFe.DiretorioXmlDestinatario + "\\" + TCN_EVENTO.CHNFE + "-nfe.xml", "XML AUTORIZAÇÃO NF-E", "PDF AUTORIZAÇÃO NF-E", true);
                        }
                        else if (strStatusRetornoNF == "110")
                        { 
                            GerandoArquivoLog("NF-e com uso denegado " + strMensagemRetorno, 1);

                            TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "9", TCN_EVENTO.ID_EVENTO_ERP);
                        }
                        else
                        {
                            GerandoArquivoLog("NF-e rejeitada, motivo: " + strMensagemRetorno, 1);

                            TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "4", TCN_EVENTO.ID_EVENTO_ERP);
                        }
                    }
                    else if (strChaveAcessoNF == "105")
                    {
                        GerandoArquivoLog("NFe enviada, lote ainda em processamento... preparando uma consulta do recibo", 1);

                        strXMLRetornoEnvio = ConsultarRecibo(strNumeroRecibo, TCN_EVENTO);

                        ConsultarNFe(decID_NOTA_FISCAL, TCN_EVENTO);
                    }
                    else
                    {
                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

                        strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retEnviNFe");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "4", TCN_EVENTO.ID_EVENTO_ERP);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                        GerandoArquivoLog("NFe rejeitada, motivo... " + strMensagemRetorno, 1);
                    }
                }
                else
                {
                    GerandoArquivoLog("Erro ao gerar XML de NF-e - " + strXML, 1);
                } 
                
            }
            catch (Exception ex)
            {
                if (blnAutorizouNF)
                {
                    GerandoArquivoLog("NF-e autorizada, mas ocorreu um erro - " + ex.Message, 1);
                }
                else
                {
                    GerandoArquivoLog("Erro no envio de NF-e - " + ex.Message, 1);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
                }
            }
        }

        // TP_ACAO = 2
        private void CancelarNFe(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                string strXMLRetornoEnvio = "", strStatusRetorno = "", strMensagemRetorno, strProtocolo = "";

                if (TCN_EVENTO.NPROT != null)
                {
                    strXMLRetornoEnvio = spdNFe.CancelarNFeEvento(TCN_EVENTO.CHNFE, TCN_EVENTO.NPROT.ToString(), TCN_EVENTO.JUSTIFICATIVA, TCN_EVENTO.DH_EVENTO.ToString("yyyy-MM-ddThh:mm:ss"), TCN_EVENTO_DAL.RetornarSequenciaEvento(TCN_EVENTO.CHNFE), TCN_EVENTO.GMT);

                    strProtocolo = LerTagXML(strXMLRetornoEnvio, "nProt", "infProt");

                    strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "infEvento");

                    strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "infEvento");

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("NPROT", strProtocolo, TCN_EVENTO.ID_EVENTO_ERP);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno,  TCN_EVENTO.ID_EVENTO_ERP);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R",TCN_EVENTO.ID_EVENTO_ERP);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                    GerandoArquivoLog(strMensagemRetorno, 1);

                    if(strStatusRetorno == "101")
                    {
                        GerandoArquivoLog("NF-e cancelada com sucesso", 1);
                    }

                    GerandoArquivoLog(strMensagemRetorno, 1);

                    SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_EVENTO.CHNFE + "-cancelamento.xml","CANCELAMENTO DE NF", TCN_EVENTO);

                    EnviarNFCanceladaDestinatario(decID_NOTA_FISCAL, TCN_EVENTO);

                    SalvarAnexoXML_PDF(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA), TCN_EVENTO, spdNFe.DiretorioXmlDestinatario + "\\" + TCN_EVENTO.CHNFE + "-caneve.xml", "XML CANCELAMENTO NF-E", "", false);
                }
                else
                {
                    GerandoArquivoLog("Campo protocolo estava nulo, impossível cancelar NF-e... " + TCN_EVENTO.CHNFE, 1);
                }               
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro no cancelamento de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 3
        private void InutilizarNFe(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            TECNO_INUTILIZACAO_ERP TCN_INUT = new TECNO_INUTILIZACAO_ERP();

            TECNO_INUTILIZACAO_ERP_DAL TCN_INUT_DAL = new TECNO_INUTILIZACAO_ERP_DAL();

            try
            {

                string strXMLRetornoEnvio = "", strStatusRetorno = "", strMensagemRetorno = "", strProtocolo = "";

                TCN_INUT = TCN_INUT_DAL.PesquisarTECNO_INUTILIZACAO_ERP(TCN_EVENTO.CHAVE_BUSCA);
                
                strXMLRetornoEnvio = spdNFe.InutilizarNF("",TCN_INUT.ANO.ToString(), TCN_INUT.CNPJ, TCN_INUT.MOD.ToString(), TCN_INUT.SERIE.ToString(), TCN_INUT.NNFINI.ToString(), TCN_INUT.NNFFIM.ToString(), TCN_INUT.XJUST);

                strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "retEnviNFe");

                strProtocolo = LerTagXML(strXMLRetornoEnvio, "nProt", "retEnviNFe");

                strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retEnviNFe");

                if (strStatusRetorno == "102")
                {
                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "3", TCN_EVENTO.ID_EVENTO_ERP);
                }
                else
                {
                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "4", TCN_EVENTO.ID_EVENTO_ERP);
                }

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("NPROT", strProtocolo, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

                SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_INUT.NNFINI + "" + TCN_INUT.NNFFIM + "-inutilizacao.xml","INUTILIZAÇÃO DE NUMERAÇÃO NF", TCN_EVENTO);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro na inutilização de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 4
        private void ConsultarNFe(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                if (TCN_EVENTO.CHNFE != "")
                {
                    string strXMLRetornoEnvio = "", strStatusRetorno = "", strMensagemRetorno = "";

                    strXMLRetornoEnvio = spdNFe.ConsultarNF(TCN_EVENTO.CHNFE);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                    strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "retConsSitNFe");

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                    strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retConsSitNFe");

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                    SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_EVENTO.CHNFE + "-consulta.xml", "CONSULTA DE NOTA FISCAL", TCN_EVENTO);

                    if (strStatusRetorno == "100")
                    {
                        GerandoArquivoLog(strMensagemRetorno, 1);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "1", TCN_EVENTO.ID_EVENTO_ERP);
                    }
                    else if (strStatusRetorno == "101")
                    {
                        GerandoArquivoLog(strMensagemRetorno, 1);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "2", TCN_EVENTO.ID_EVENTO_ERP);
                    }
                    else if (strStatusRetorno == "102")
                    {
                        GerandoArquivoLog(strMensagemRetorno, 1);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "3", TCN_EVENTO.ID_EVENTO_ERP);
                    }
                    else if (strStatusRetorno == "110")
                    {
                        GerandoArquivoLog(strMensagemRetorno, 1);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "9", TCN_EVENTO.ID_EVENTO_ERP);
                    }
                    else
                    {
                        GerandoArquivoLog(strMensagemRetorno, 1);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "4", TCN_EVENTO.ID_EVENTO_ERP);
                    }

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);
                }
                else
                {
                    GerandoArquivoLog("Necessário chave de acesso para consulta de NF-e", 1);
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro na inutilização de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }            
        }

        // TP_ACAO = 5
        private void ConsultarContribuinteICMS(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                string strXMLRetornoEnvio = "", strStatusRetorno = "", strMensagemRetorno = "", strTipoDocumento = "";

                if (TCN_EVENTO.CONTRIB_CNPJ_CPF_IE.Length == 11)
                    strTipoDocumento = "CPF";

                else if (TCN_EVENTO.CONTRIB_CNPJ_CPF_IE.Length == 14)
                    strTipoDocumento = "CNPJ";

                else
                    strTipoDocumento = "IE";

                strXMLRetornoEnvio = spdNFe.ConsultarCadastro(TCN_EVENTO.CONTRIB_CNPJ_CPF_IE, strTipoDocumento, spdNFe.UF);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "infCons");

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "infCons");

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

                SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_EVENTO.CONTRIB_CNPJ_CPF_IE + "-consulta-contribuinte.xml","CONSULTA DE CONTRIBUINTE DE ICMS", TCN_EVENTO);
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro na consulta de contribuinte de ICMS - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 6
        private void EnviarNFe_DPEC(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            bool blnAutorizouNF = false;

            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                TECNO_NF nf = new TECNO_NF();

                TECNO_NF_DAL nfDAL = new TECNO_NF_DAL();

                nf = nfDAL.ListarTECNO_NF(decID_NOTA_FISCAL);

                blnAutorizouNF = false;

                GerandoArquivoLog("Preparando envio da NF-e ID_NOTA_FISCAL: " + nf.ID_NOTA_FISCAL + "; NRO: " + nf.IDE_NNF + "; SÉRIE: " + nf.IDE_SERIE + ";", 1);

                string strXML = "", strXMLAssinado = "", strXMLRetornoEnvio = "", strNumeroRecibo = "", strStatusRetornoRecibo = "", strStatusRetornoNF = "", strMensagemRetorno = "", strChaveAcessoNF = "", strXMLRetornoConsulta, strProtocolo = "";

                bool blnGerouXML = false;

                strXML = MontarXML(nf, TCN_EVENTO, ref blnGerouXML);

                if (blnGerouXML)
                {
                    GerandoArquivoLog("Gerou XML da nota fiscal com sucesso... Preparando para assinar XML", 1);

                    strXMLAssinado = spdDPEC.AssinarDPEC(strXML);

                    GerandoArquivoLog("XML assinado com sucesso, preparando para enviar NFe", 1);

                    strXMLRetornoEnvio = spdDPEC.EnviarDPEC(strXMLAssinado);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                    GerandoArquivoLog("NFe enviada, verificando status...", 1);

                    strStatusRetornoRecibo = LerTagXML(strXMLRetornoEnvio, "cStat", "retEnviNFe");

                    if (strStatusRetornoRecibo == "103")
                    {
                        strNumeroRecibo = LerTagXML(strXMLRetornoEnvio, "nRec", "infRec");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("NREC", strNumeroRecibo, TCN_EVENTO.ID_EVENTO_ERP);

                        TCN_EVENTO.NREC = Convert.ToDecimal(strNumeroRecibo);

                        GerandoArquivoLog("Recibo Verificado (" + strNumeroRecibo + "), consultando número de recibo", 1);

                        strXMLRetornoEnvio = ConsultarRecibo(strNumeroRecibo, TCN_EVENTO);

                        strChaveAcessoNF = LerTagXML(strXMLRetornoEnvio, "chNFe", "infProt");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("CHNFE", strChaveAcessoNF, TCN_EVENTO.ID_EVENTO_ERP);

                        TCN_EVENTO.CHNFE = strChaveAcessoNF;

                        GerandoArquivoLog("Recibo consultado, chave de acesso " + strChaveAcessoNF, 1);

                        strStatusRetornoNF = LerTagXML(strXMLRetornoEnvio, "cStat", "infProt");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetornoNF, TCN_EVENTO.ID_EVENTO_ERP);

                        strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "infProt");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

                        if (strStatusRetornoNF == "100")
                        {
                            blnAutorizouNF = true;

                            strProtocolo = LerTagXML(strXMLRetornoEnvio, "nProt", "infProt");

                            GerandoArquivoLog("Protocolo: " + strProtocolo, 1);

                            TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("NPROT", strProtocolo, TCN_EVENTO.ID_EVENTO_ERP);

                            TCN_EVENTO.NPROT = Convert.ToDecimal(strProtocolo);

                            GerandoArquivoLog(strMensagemRetorno, 1);

                            TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "1", TCN_EVENTO.ID_EVENTO_ERP);

                            strXMLRetornoConsulta = spdNFe.ConsultarNF(strChaveAcessoNF);

                            GerandoArquivoLog("Preparando para enviar e-mail para destinatário " + TCN_EVENTO.CONFIG_EMAIL_DESTINATARIO, 1);

                            EnviarEmailDestinatario(decID_NOTA_FISCAL, TCN_EVENTO);

                            GerandoArquivoLog("Email enviado, imprimindo DANFE...", 1);

                            ImprimirDANFE(decID_NOTA_FISCAL, TCN_EVENTO);

                            SalvarAnexoXML_PDF(decID_NOTA_FISCAL, TCN_EVENTO, spdNFe.DiretorioXmlDestinatario + "\\" + TCN_EVENTO.CHNFE + "-nfe.xml","XML AUTORIZAÇÃO NF-E DPEC","PDF AUTORIZAÇÃO NF-E DPEC",true);
                        }
                        else if (strStatusRetornoNF == "110")
                        {
                            GerandoArquivoLog("NF-e com uso denegado " + strMensagemRetorno, 1);

                            TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "9", TCN_EVENTO.ID_EVENTO_ERP);
                        }
                        else
                        {
                            GerandoArquivoLog("NF-e rejeitada, motivo: " + strMensagemRetorno, 1);

                            TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "4", TCN_EVENTO.ID_EVENTO_ERP);
                        }
                    }
                    else if (strChaveAcessoNF == "105")
                    {
                        GerandoArquivoLog("NFe enviada, lote ainda em processamento... preparando uma consulta do recibo", 1);

                        strXMLRetornoEnvio = ConsultarRecibo(strNumeroRecibo, TCN_EVENTO);

                        ConsultarNFe(decID_NOTA_FISCAL, TCN_EVENTO);
                    }
                    else
                    {
                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

                        strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retEnviNFe");

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "4", TCN_EVENTO.ID_EVENTO_ERP);

                        TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                        GerandoArquivoLog("NFe rejeitada, motivo... " + strMensagemRetorno, 1);
                    }
                }
                else
                {
                    GerandoArquivoLog("Erro ao gerar XML de NF-e - " + strXML, 1);
                }

            }
            catch (Exception ex)
            {
                if (blnAutorizouNF)
                {
                    GerandoArquivoLog("NF-e autorizada, mas ocorreu um erro - " + ex.Message, 1);
                }
                else
                {
                    GerandoArquivoLog("Erro no envio de NF-e - " + ex.Message, 1);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
                }
            }
        }

        // TP_ACAO = 7
        private void ImprimirDANFE(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                if (TCN_EVENTO.IMPRESSORA != "")
                {
                    GerandoArquivoLog("Preparando para imprimir DANFE", 1);

                    string strXMLRetornoEnvio = System.IO.File.ReadAllText(spdNFe.DiretorioXmlDestinatario + "\\" + TCN_EVENTO.CHNFE + "-nfe.xml");
                    
                    if (TCN_EVENTO.NR_LOTE > 0)
                    {
                        if (TCN_EVENTO.TP_IMP == 2)
                            spdNFe.ImprimirDanfe(TCN_EVENTO.NR_LOTE.ToString(), strXMLRetornoEnvio, spdNFe.ModeloPaisagem, TCN_EVENTO.IMPRESSORA);
                        else
                            spdNFe.ImprimirDanfe(TCN_EVENTO.NR_LOTE.ToString(), strXMLRetornoEnvio, spdNFe.ModeloRetrato, TCN_EVENTO.IMPRESSORA);
                    }
                    else
                    {
                        if (TCN_EVENTO.TP_IMP == 2)
                            spdNFe.ImprimirDanfe("1", strXMLRetornoEnvio, spdNFe.ModeloPaisagem, TCN_EVENTO.IMPRESSORA);
                        else
                            spdNFe.ImprimirDanfe("1", strXMLRetornoEnvio, spdNFe.ModeloRetrato, TCN_EVENTO.IMPRESSORA);
                    }

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

                    GerandoArquivoLog("DANFE enviado para impressão...", 1);
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro na impressão de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 12
        private void EnviarEmailDestinatario(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                if (TCN_EVENTO.CONFIG_EMAIL_DESTINATARIO != "")
                {
                    GerandoArquivoLog("Preparando para enviar email para o destinatário...", 1);

                    string strXMLRetornoEnvio = System.IO.File.ReadAllText(spdNFe.DiretorioXmlDestinatario + "\\" + TCN_EVENTO.CHNFE + "-nfe.xml");

                    spdNFe.EnviarNotaDestinatario(TCN_EVENTO.CHNFE, "", "");

                    TCN_DAL.AtualizarTECNO_EVENTO_ERP("IN_ENVIOU_EMAIL", "1", TCN_EVENTO.ID_EVENTO_ERP);

                    GerandoArquivoLog("Email enviado com sucesso para " + TCN_EVENTO.CONFIG_EMAIL_DESTINATARIO, 1);

                    TCN_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);
                }
                else
                {
                    GerandoArquivoLog("Nenhum email destinatário informado", 1);
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro no envio de email para o destinatário - " + ex.Message, 1);

                TCN_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 1000
        private void EnviarCartaCorrecao(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                string strXMLRetornoEnvio = "", strStatusRetorno = "", strMensagemRetorno = "";

                strXMLRetornoEnvio = spdNFe.EnviarCCe(TCN_EVENTO.CHNFE, TCN_EVENTO.TX_CORRECAO, TCN_EVENTO.DH_EVENTO.ToString("yyyy-MM-ddThh:mm:ss"), TCN_EVENTO.CODIGO_UF, TCN_EVENTO.NR_LOTE.ToString(), TCN_EVENTO_DAL.RetornarSequenciaEvento(TCN_EVENTO.CHNFE), TCN_EVENTO.GMT);

                SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_EVENTO.CHNFE + "-carta-correcao.xml","ENVIO DE CARTA DE CORREÇÃO", TCN_EVENTO);

                strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "retEnvEvento");

                strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retEnvEvento");

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                GerandoArquivoLog(strMensagemRetorno, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro no envio de carta de correção de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 1001
        private void EnviarPedidoProrrogacao(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                string strXMLRetornoEnvio = "", strStatusRetorno = "", strMensagemRetorno = "";

                if(TCN_EVENTO.TP_EVENTO == 1)
                    strXMLRetornoEnvio = spdNFe.EnviarPedidoProrrogacao(TCN_EVENTO.NR_LOTE.ToString(), TCN_EVENTO.CHNFE, TCN_EVENTO.NPROT.ToString(), TCN_EVENTO.DH_EVENTO.ToString("yyyy-MM-ddThh:mm:ss"), TCN_EVENTO.CODIGO_UF.ToString(), TipoPedidoProrrogacao.tEPP2, TCN_EVENTO_DAL.RetornarSequenciaEvento(TCN_EVENTO.CHNFE), TCN_EVENTO.GMT, "");
                else
                    strXMLRetornoEnvio = spdNFe.EnviarPedidoProrrogacao(TCN_EVENTO.NR_LOTE.ToString(),TCN_EVENTO.CHNFE, TCN_EVENTO.NPROT.ToString(), TCN_EVENTO.DH_EVENTO.ToString("yyyy-MM-ddThh:mm:ss"), TCN_EVENTO.CODIGO_UF.ToString(),TipoPedidoProrrogacao.tEPP1 , TCN_EVENTO_DAL.RetornarSequenciaEvento(TCN_EVENTO.CHNFE), TCN_EVENTO.GMT, "");

                SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_EVENTO.CHNFE + "-pedido-prorrogacao.xml","ENVIO DE PEDIDO DE PRORROGAÇÃO", TCN_EVENTO);

                strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "retEnvEvento");

                strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retEnvEvento");

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                GerandoArquivoLog(strMensagemRetorno, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro no envio de pedido de prorrogação de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 1002
        private void EnviarManifestacaoDestinatario(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                string strXMLRetornoEnvio = "", strStatusRetorno = "", strMensagemRetorno = "";

                strXMLRetornoEnvio = spdNFe.EnviarManifestacaoDestinatario(0, TCN_EVENTO.CHNFE, TCN_EVENTO.JUSTIFICATIVA, TCN_EVENTO.DH_EVENTO.ToString("yyyy-MM-ddThh:mm:ss"), "", TCN_EVENTO_DAL.RetornarSequenciaEvento(TCN_EVENTO.CHNFE), TCN_EVENTO.GMT, TCN_EVENTO.CODIGO_UF.ToString());

                SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioLog + TCN_EVENTO.CHNFE + "-manifes-destinatario.xml", "ENVIO DE MANIFESTACAO DE DESTINATÁRIO",TCN_EVENTO);

                strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "retEnvEvento");

                strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retEnvEvento");

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                GerandoArquivoLog(strMensagemRetorno, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro na manifestação de destinatário de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 1003
        private void SalvarAnexoXML_PDF(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO, string strCaminhoArquivoXML, string strDescricaoArquivoXML, string strDescricaoArquivoPDF, bool blnSalvarPDF)
        {       
            try
            {
                int intCodigoAnexo = 0;

                TECNO_ANEXO_DAL TCN_ANEXO_DAL = new TECNO_ANEXO_DAL();

                List<TECNO_ANEXO> LISTA_TCN_ANEXO = new List<TECNO_ANEXO>();

                LISTA_TCN_ANEXO = TCN_ANEXO_DAL.ListarTECNO_ANEXO(decID_NOTA_FISCAL);

                intCodigoAnexo = LISTA_TCN_ANEXO.Count();

                TECNO_ANEXO TCN_ANEXO_XML = new TECNO_ANEXO();

                string strXMLRetornoEnvio = System.IO.File.ReadAllText(strCaminhoArquivoXML);

                FileStream fileXML = new FileStream(strCaminhoArquivoXML, FileMode.Open);

                BinaryReader br = new BinaryReader(fileXML);

                byte[] byteArquivoXML = br.ReadBytes((int)fileXML.Length);

                fileXML.Close();

                TCN_ANEXO_XML.CHAVE_BUSCA = decID_NOTA_FISCAL.ToString();

                TCN_ANEXO_XML.CD_ANEXO = intCodigoAnexo + 1;

                TCN_ANEXO_XML.DS_ARQUIVO = strDescricaoArquivoXML;

                TCN_ANEXO_XML.TX_CONTEUDO = byteArquivoXML;

                TCN_ANEXO_XML.EX_ARQUIVO = "XML";

                TCN_ANEXO_XML.TP_ACAO = Convert.ToInt32(TCN_EVENTO.TP_ACAO);

                TCN_ANEXO_DAL.InserirTECNO_ANEXO(TCN_ANEXO_XML);

                if (blnSalvarPDF)
                {
                    TECNO_ANEXO TCN_ANEXO_PDF = new TECNO_ANEXO();

                    string strCaminhoArquivoPDF = spdNFe.DiretorioXmlDestinatario + TCN_EVENTO.CHNFE + "-nfe.pdf";

                    FileStream filePDF = new FileStream(strCaminhoArquivoPDF, FileMode.Create);

                    filePDF.Close();

                    if (TCN_EVENTO.NR_LOTE > 0)
                    {
                        if (TCN_EVENTO.TP_IMP == 2)
                            spdNFe.ExportarDanfe(TCN_EVENTO.NR_LOTE.ToString(), strXMLRetornoEnvio, spdNFe.ModeloPaisagem, 1, strCaminhoArquivoPDF);
                        else
                            spdNFe.ExportarDanfe(TCN_EVENTO.NR_LOTE.ToString(), strXMLRetornoEnvio, spdNFe.ModeloRetrato, 1, strCaminhoArquivoPDF);
                    }
                    else
                    {
                        if (TCN_EVENTO.TP_IMP == 2)
                            spdNFe.ExportarDanfe("1", strXMLRetornoEnvio, spdNFe.ModeloPaisagem, 1, strCaminhoArquivoPDF);
                        else
                            spdNFe.ExportarDanfe("1", strXMLRetornoEnvio, spdNFe.ModeloRetrato, 1, strCaminhoArquivoPDF);
                    }

                    filePDF = new FileStream(strCaminhoArquivoPDF, FileMode.Open);

                    br = new BinaryReader(filePDF);

                    byte[] byteArquivoPDF = br.ReadBytes((int)filePDF.Length);

                    filePDF.Close();

                    TCN_ANEXO_PDF.CHAVE_BUSCA = decID_NOTA_FISCAL.ToString();

                    TCN_ANEXO_PDF.CD_ANEXO = intCodigoAnexo + 2;

                    TCN_ANEXO_PDF.DS_ARQUIVO = strDescricaoArquivoPDF;

                    TCN_ANEXO_PDF.TX_CONTEUDO = byteArquivoPDF;

                    TCN_ANEXO_PDF.EX_ARQUIVO = "PDF";

                    TCN_ANEXO_PDF.TP_ACAO = Convert.ToInt32(TCN_EVENTO.TP_ACAO);

                    TCN_ANEXO_DAL.InserirTECNO_ANEXO(TCN_ANEXO_PDF);

                    GerandoArquivoLog("XML e PDF da NF-e salvo com sucesso", 1);
                }
            }
            catch(Exception ex)
            {
                GerandoArquivoLog("Erro ao salvar anexo - " + ex.Message, 1);
            }
        }

        // TP_ACAO = 1004
        private void ConsultarNFEmitidasContraCNPJ(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                int intCodigoAnexo = 0;

                string strXMLRetornoEnvio = "", strCaminhoDiretorio = spdNFe.DiretorioLog + "\\" + TCN_EVENTO.CNPJ_EVENTO + " - " + TCN_EVENTO.NSU, strStatusRetorno = "", strMensagemRetorno = "";

                if(TCN_EVENTO.TP_NSU == 1)
                    strXMLRetornoEnvio = spdNFe.ConsultarDistribuicaoDFe(TCN_EVENTO.CODIGO_UF, TCN_EVENTO.CNPJ_EVENTO, TCN_EVENTO.NSU, TipoNSU.nkEspecifico);
                else
                    strXMLRetornoEnvio = spdNFe.ConsultarDistribuicaoDFe(TCN_EVENTO.CODIGO_UF, TCN_EVENTO.CNPJ_EVENTO, TCN_EVENTO.NSU, TipoNSU.nkUltimo);

                if (!Directory.Exists(strCaminhoDiretorio))
                    Directory.CreateDirectory(strCaminhoDiretorio);

                spdNFe.TratarXMLRetornoDFe(strXMLRetornoEnvio, strCaminhoDiretorio);

                System.IO.Compression.ZipFile.CreateFromDirectory(strCaminhoDiretorio, strCaminhoDiretorio);

                FileStream file = new FileStream(strCaminhoDiretorio, FileMode.Open);

                BinaryReader br = new BinaryReader(file);

                byte[] byteArquivo = br.ReadBytes((int)file.Length);

                file.Close();

                TECNO_ANEXO_DAL TCN_ANEXO_DAL = new TECNO_ANEXO_DAL();

                List<TECNO_ANEXO> LISTA_TCN_ANEXO = new List<TECNO_ANEXO>();

                LISTA_TCN_ANEXO = TCN_ANEXO_DAL.ListarTECNO_ANEXO(decID_NOTA_FISCAL);

                intCodigoAnexo = LISTA_TCN_ANEXO.Count();

                TECNO_ANEXO TCN_ANEXO = new TECNO_ANEXO();

                TCN_ANEXO.CHAVE_BUSCA = decID_NOTA_FISCAL.ToString();

                TCN_ANEXO.CD_ANEXO = intCodigoAnexo + 2;

                TCN_ANEXO.DS_ARQUIVO = "NOTAS EMITIDAS CONTRA O CNPJ";

                TCN_ANEXO.TX_CONTEUDO = byteArquivo;

                TCN_ANEXO.EX_ARQUIVO = "ZIP";

                TCN_ANEXO.TP_ACAO = Convert.ToInt32(TCN_EVENTO.TP_ACAO);

                TCN_ANEXO_DAL.InserirTECNO_ANEXO(TCN_ANEXO);

                GerandoArquivoLog("Consulta de notas emitidas contra CNPJ feita com sucesso", 1);

                strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "retEnvEvento");

                strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retEnvEvento");

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                GerandoArquivoLog(strMensagemRetorno, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro na manifestação de destinatário de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        // TP_ACAO = 1005
        private void ConsultarNFEspecificaEmitidaContraCNPJ(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                string strXMLRetornoEnvio = "", strStatusRetorno = "", strMensagemRetorno = "";

                strXMLRetornoEnvio = spdNFe.ConsultarDistribuicaoDFeChave(TCN_EVENTO.CODIGO_UF, TCN_EVENTO.CNPJ_EVENTO,TCN_EVENTO.CHNFE);

                SalvarXML(strXMLRetornoEnvio, spdNFe.DiretorioXmlDestinatario + TCN_EVENTO.CHNFE + "-nf-emitida-conta-"+TCN_EVENTO.CNPJ_EVENTO+".xml","CONSULTA DE NF EMITIDA CONTRA CNPJ " + TCN_EVENTO.CNPJ_EVENTO, TCN_EVENTO);

                GerandoArquivoLog("Consulta da NF emitida contra CNPJ feita com sucesso", 1);

                strStatusRetorno = LerTagXML(strXMLRetornoEnvio, "cStat", "retDistDFeInt");

                strMensagemRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retDistDFeInt");

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "S", TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", strMensagemRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("COD_RETORNO", strStatusRetorno, TCN_EVENTO.ID_EVENTO_ERP);

                GerandoArquivoLog(strMensagemRetorno, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro na manifestação de destinatário de NF-e - " + ex.Message, 1);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        private void EnviarNFCanceladaDestinatario(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            TECNO_EVENTO_ERP_DAL TCN_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                if (TCN_EVENTO.CONFIG_EMAIL_DESTINATARIO != "")
                {
                    GerandoArquivoLog("Preparando para enviar NF-e cancelada para o destinatário...", 1);

                    string strXML_log_env = System.IO.File.ReadAllText(spdNFe.DiretorioLog + "\\" + TCN_EVENTO.CHNFE + "-caneve-env.xml");

                    string strXML_log_ret = System.IO.File.ReadAllText(spdNFe.DiretorioLog + "\\" + TCN_EVENTO.CHNFE + "-caneve-ret.xml");

                    spdNFe.EnviarNotaCanceladaDestinatario(TCN_EVENTO.CHNFE, strXML_log_env, strXML_log_ret, TCN_EVENTO.CONFIG_EMAIL_DESTINATARIO);

                    TCN_DAL.AtualizarTECNO_EVENTO_ERP("IN_ENVIOU_EMAIL", "1", TCN_EVENTO.ID_EVENTO_ERP);

                    GerandoArquivoLog("Email enviado com sucesso para " + TCN_EVENTO.CONFIG_EMAIL_DESTINATARIO, 1);

                    TCN_DAL.AtualizarTECNO_EVENTO_ERP("TP_STATUS", "R", TCN_EVENTO.ID_EVENTO_ERP);

                }
                else
                {
                    GerandoArquivoLog("Nenhum email destinatário informado", 1);
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Erro no envio de email para o destinatário - " + ex.Message, 1);

                TCN_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                TCN_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
            }
        }

        private void EnviarNFeSCAN(decimal decID_NOTA_FISCAL, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            bool blnAutorizouNF = false;

            TECNO_EVENTO_ERP_DAL TCN_EVENTO_DAL = new TECNO_EVENTO_ERP_DAL();

            try
            {
                TECNO_NF nf = new TECNO_NF();

                TECNO_NF_DAL nfDAL = new TECNO_NF_DAL();

                nf = nfDAL.ListarTECNO_NF(decID_NOTA_FISCAL);

                blnAutorizouNF = false;

                GerandoArquivoLog("Preparando envio da NF-e ID_NOTA_FISCAL: " + nf.ID_NOTA_FISCAL + "; NRO: " + nf.IDE_NNF + "; SÉRIE: " + nf.IDE_SERIE + ";", 1);

                string strXML = "", strXMLAssinado = "", strXMLRetornoEnvio = "", strNumeroRecibo = "", strStatusRetornoRecibo = "", strStatusRetornoNF = "", strMotivoRetorno = "", strChaveAcessoNF = "";

                bool blnGerouXML = false;

                strXML = MontarXML(nf, TCN_EVENTO, ref blnGerouXML);

                if (blnGerouXML)
                {
                    GerandoArquivoLog("Gerou XML da nota fiscal com sucesso... Preparando para assinar XML", 1);

                    strXMLAssinado = spdNFe.AssinarNota(strXML);

                    GerandoArquivoLog("XML assinado com sucesso, preparando para enviar NFe", 1);

                    strXMLRetornoEnvio = spdNFe.EnviarNF("1", strXMLAssinado, false);

                    GerandoArquivoLog("NFe enviada, verificando status...", 1);

                    strStatusRetornoRecibo = LerTagXML(strXMLRetornoEnvio, "cStat", "retEnviNFe");

                    if (strStatusRetornoRecibo == "103")
                    {
                        strNumeroRecibo = LerTagXML(strXMLRetornoEnvio, "nRec", "infRec");

                        GerandoArquivoLog("Recibo Verificado (" + strNumeroRecibo + "), consultando número de recibo", 1);

                        strXMLRetornoEnvio = ConsultarRecibo(strNumeroRecibo, TCN_EVENTO);

                        strChaveAcessoNF = LerTagXML(strXMLRetornoEnvio, "chNFe", "infProt");

                        GerandoArquivoLog("Recibo consultado, chave de acesso " + strChaveAcessoNF, 1);

                        strStatusRetornoNF = LerTagXML(strXMLRetornoEnvio, "cStat", "infProt");

                        if (strStatusRetornoNF == "100")
                        {
                            blnAutorizouNF = true;

                            GerandoArquivoLog("NF-e autorizada com sucesso... " + strMotivoRetorno, 1);
                        }
                        if (strStatusRetornoNF == "101")
                        {
                            GerandoArquivoLog("Esta NF-e está cancelada... Cancelamento de NF-e Homologado " + strMotivoRetorno, 1);
                        }
                        else if (strStatusRetornoNF == "110")
                        {
                            GerandoArquivoLog("NF-e com uso denegado " + strMotivoRetorno, 1);
                        }
                        else
                        {
                            strMotivoRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "infProt");

                            GerandoArquivoLog("NF-e rejeitada, motivo: " + strMotivoRetorno, 1);
                        }
                    }
                    else if (strChaveAcessoNF == "105")
                    {
                        GerandoArquivoLog("NFe enviada, lote ainda em processamento... preparando uma consulta do recibo", 1);

                        strXMLRetornoEnvio = ConsultarRecibo(strNumeroRecibo, TCN_EVENTO);

                        //CRIAR EVENTO DE CONSULTA DE RECIBO
                    }
                    else
                    {
                        strMotivoRetorno = LerTagXML(strXMLRetornoEnvio, "xMotivo", "retEnviNFe");

                        GerandoArquivoLog("NFe rejeitada, motivo... " + strMotivoRetorno, 1);
                    }
                }
                else
                {
                    GerandoArquivoLog("Erro ao gerar XML de NF-e - " + strXML, 1);
                }
            }
            catch (Exception ex)
            {
                if (blnAutorizouNF)
                {
                    GerandoArquivoLog("NF-e autorizada, mas ocorreu um erro - " + ex.Message, 1);
                }
                else
                {
                    GerandoArquivoLog("Erro no envio de NF-e - " + ex.Message, 1);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("DESC_RETORNO", ex.Message, TCN_EVENTO.ID_EVENTO_ERP);

                    TCN_EVENTO_DAL.AtualizarTECNO_EVENTO_ERP("TP_RETORNO", "-1", TCN_EVENTO.ID_EVENTO_ERP);
                }
            }
        }

        private static string LerTagXML(string pTexto, string pTag, string pTagPai)
        {
            if (pTexto != null)
            {
                StringReader stream = new StringReader(pTexto);
                XmlTextReader reader = new XmlTextReader(stream);
                DataSet dataSet = new DataSet();
 
                dataSet.ReadXml(reader);

                foreach (DataTable thisTable in dataSet.Tables)
                {
                    foreach (DataRow row in thisTable.Rows)
                    {
                        foreach (DataColumn column in thisTable.Columns)
                        {
                            if (thisTable.TableName == pTagPai)
                            {
                                return row[pTag].ToString();
                            }
                        }
                    }
                }
            }
            return "";
        }

        private string ConverterNumeroParaPadraoXML(decimal? decValor)
        {
            return decValor.ToString().Replace(",",".");
        }

        private string ConsultarRecibo(string strNumeroRecibo, TECNO_EVENTO_ERP TCN_EVENTO)
        {
            if (strNumeroRecibo != "")
                return spdNFe.ConsultarRecibo(strNumeroRecibo);
            else
                return "";
        }

        private void GerandoInsertDicionarioTecnoSpeed()
        {
            string ARQUIVO_ORIGEM = System.IO.File.ReadAllText(@"C:\Users\vinic\Desktop\teste.txt");
            System.IO.TextWriter ARQUIVO_DESTINO = System.IO.File.AppendText(@"C:\Users\vinic\Desktop\teste2.txt");

            string[] t = ARQUIVO_ORIGEM.Split('³');

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

                        ARQUIVO_DESTINO.WriteLine("INSERT INTO DICIONARIO_NF_TECNOSPEED VALUES('" +nome+"','"+tipo+"','"+tamanho+"','"+requerido+"','"+display+"','"+dica+"','"+enum1 + "');");
                       
                    }
                    catch
                    {

                    }
                }
            }
            ARQUIVO_DESTINO.Close();
        }

        private string MontarXML(TECNO_NF nf, TECNO_EVENTO_ERP TCN_EVENTO, ref bool blnGerouXML)
        {
            try
            {
                if (nf != null)
                {
                    dt.VersaoEsquema = "pl_009";
                    dt.DiretorioTemplates = spdNFe.DiretorioTemplates + "Conversor\\NFeDataSets.xml";
                    dt.DicionarioXML = spdNFe.DiretorioTemplates + "Conversor\\NFeDataSets.xml";
                    dt.Incluir();
                   
                    dt.SetCampo("versao_A02=" + ConverterNumeroParaPadraoXML(nf.IDE_VERSAO));//Versão do leiaute	
                    //dt.SetCampo("id_A03=");//Chave da NFe a ser assinada	
                    dt.SetCampo("cUF_B02=" + ConverterNumeroParaPadraoXML(nf.IDE_CUF));//Código da UF do emitente do Documento Fiscal	
                    dt.SetCampo("cNF_B03=" + ConverterNumeroParaPadraoXML(nf.IDE_CNF));//Código Numérico que compõe a Chave de Acesso	
                    dt.SetCampo("natOp_B04=" + nf.IDE_NATOP);//Descrição da Natureza da Operação	
                    dt.SetCampo("mod_B06=" + ConverterNumeroParaPadraoXML(nf.IDE_MOD));//Código do Modelo do Documento Fiscal	
                    dt.SetCampo("serie_B07=" + ConverterNumeroParaPadraoXML(nf.IDE_SERIE));//Série do Documento Fiscal	
                    dt.SetCampo("nNF_B08=" + ConverterNumeroParaPadraoXML(nf.IDE_NNF));//Número do documento fiscal	
                    dt.SetCampo("dhEmi_B09=" + nf.IDE_DEMI.ToString("yyyy-MM-ddThh:mm:ss") + TCN_EVENTO.GMT);//Data e hora de emissão do Documento Fiscal	
                    dt.SetCampo("dhSaiEnt_B10=" + nf.IDE_DSAIENT.ToString("yyyy-MM-ddThh:mm:ss") + TCN_EVENTO.GMT);//Data e hora de Saída ou da Entrada da Mercadoria/Produto	
                    dt.SetCampo("tpNF_B11=" + nf.IDE_TPNF);//Tipo de Operação	
                    dt.SetCampo("idDest_B11a=" + ConverterNumeroParaPadraoXML(nf.IDE_DEST));//Identificador de local de destino da operação	
                    dt.SetCampo("cMunFG_B12=" + ConverterNumeroParaPadraoXML(nf.IDE_CMUNFG));//Código do Município de Ocorrência do Fato Gerador
                    dt.SetCampo("tpImp_B21=" + nf.IDE_TPIMP);//Formato de Impressão do DANFE	
                    dt.SetCampo("tpEmis_B22=" + nf.IDE_TPEMIS);//Tipo de Emissão da NF-e	
                    //dt.SetCampo("cDV_B23=" + nf.);//Dígito Verificador da Chave de Acesso da NF-e	

                    if (spdNFe.Ambiente == Ambiente.akProducao)
                        dt.SetCampo("tpAmb_B24=1");//Identificação do Ambiente	
                    else
                        dt.SetCampo("tpAmb_B24=2");//Identificação do Ambiente	

                    dt.SetCampo("finNFe_B25=" + nf.IDE_FINNFE);//Versão do Processo de emissão da NF-e	
                    dt.SetCampo("indFinal_B25a=" + ConverterNumeroParaPadraoXML(nf.IDE_INDFINAL));//Indica operação com Consumidor final	
                    dt.SetCampo("indPres_B25b=" + ConverterNumeroParaPadraoXML(nf.IDE_INDPRES));//Indicador de presença do comprador no estabelecimento comercial no momento da operação	

                    if (nf.IDE_INDPRES == 2 || nf.IDE_INDPRES == 3 || nf.IDE_INDPRES == 4 || nf.IDE_INDPRES == 9)
                    {
                        if (nf.IDE_INDINTERMED == null)
                            dt.SetCampo("indIntermed_B25c=0");//Indicador de intermediador/marketplace	
                        else
                            dt.SetCampo("indIntermed_B25c=" + ConverterNumeroParaPadraoXML(nf.IDE_INDINTERMED));//Indicador de intermediador/marketplace	
                    }

                    dt.SetCampo("procEmi_B26=" + nf.IDE_PROCEMI);//	Processo de emissão da NF-e
                    dt.SetCampo("verProc_B27=" + ConverterNumeroParaPadraoXML(nf.IDE_VERSAO));//	Versão do Processo de emissão da NF-e

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
                        dt.SetCampo("cUF_BA04=" + ConverterNumeroParaPadraoXML(nf_ref.REFNF_CUF));//Código da UF do emitente	
                        dt.SetCampo("AAMM_BA05=" + ConverterNumeroParaPadraoXML(nf_ref.REFNF_AAMM));//Ano e Mês de emissão da NF-e	
                        dt.SetCampo("CNPJ_BA06=" + nf_ref.REFNF_CNPJ);//CNPJ do emitente	
                        dt.SetCampo("mod_BA07=" + ConverterNumeroParaPadraoXML(nf_ref.REFNF_MOD));//Modelo do Documento Fiscal	
                        dt.SetCampo("serie_BA08=" + nf_ref.REFNF_SERIE);//Série do Documento Fiscal	
                        dt.SetCampo("nNF_BA09=" + ConverterNumeroParaPadraoXML(nf_ref.REFNF_NNF));//Número do Documento Fiscal
                        dt.SalvarParte("NREF");

                    }

                    /*
                    //Informações da NF de produtor rural referenciada
                    dt.IncluirParte("NREF");
                    dt.SetCampo("cUF_BA11");//Código da UF do emitente	
                    dt.SetCampo("AAMM_BA12");//Ano e Mês de emissão da NF-e	
                    dt.SetCampo("CNPJ_BA13");//CNPJ do emitente	
                    dt.SetCampo("CPF_BA14");//CPF do emitente	
                    dt.SetCampo("IE_BA15");//IE do emitente	
                    dt.SetCampo("mod_BA16");//Modelo do Documento Fiscal	
                    dt.SetCampo("serie_BA17");//Série do Documento Fiscal	
                    dt.SetCampo("nNF_BA18");//Número do Documento Fiscal	
                    dt.SetCampo("refCTe_BA19");//Chave de acesso do CT-e referenciada	
                    dt.SalvarParte("NREF");

                    //Informações do Cupom Fiscal referenciado	
                    dt.IncluirParte("NREF");
                    dt.SetCampo("mod_BA21");//Modelo do Documento Fiscal	
                    dt.SetCampo("nECF_BA22");//Número de ordem Sequência l do ECF	
                    dt.SetCampo("nCOO_BA23");//Número do Contador de Ordem de Operação - COO	
                    dt.SalvarParte("NREF");
                    */

                    //Identificação do emitente da NF-e
                    if (nf.DEST_CNPJ_CPF.Length > 11)
                        dt.SetCampo("CNPJ_C02=" + nf.EMIT_CNPJ);//CNPJ do emitente	
                    else
                        dt.SetCampo("CPF_C02a=" + nf.EMIT_CNPJ);//CPF do emitente

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

                    if(nf.DEST_IM != "0.00")
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

                    /*
                    //Autorização para obter XML
                    dt.IncluirParte("autXML");
                        dt.SetCampo("CNPJ_GA02=" + nf.DEST_CNPJ_CPF);//CNPJ Autorizado	
                        dt.SetCampo("CPF_GA03" + nf.DEST_CNPJ_CPF);//CPF Autorizado	
                    dt.SalvarParte("autXML");
                    */

                    //Produtos e Serviços da NF-e
                    List<TECNO_NF_PRODUTOS> nf_produtos = new List<TECNO_NF_PRODUTOS>();
                    TECNO_NF_PRODUTOS_DAL nf_produtosDAL = new TECNO_NF_PRODUTOS_DAL();
                    nf_produtos = nf_produtosDAL.ListarTECNO_NF_PRODUTOS(nf.ID_NOTA_FISCAL);

                    foreach (var p in nf_produtos)
                    {
                        dt.IncluirItem();
                        dt.SetCampo("nItem_H02=" + ConverterNumeroParaPadraoXML(p.PROD_NITEM));//Número do Item	
                        dt.SetCampo("cProd_I02=" + p.PROD_CPROD);//Código do produto ou serviço	

                        if(p.PROD_CEAN !="")
                            dt.SetCampo("cEAN_I03=" + p.PROD_CEAN);//GTIN (Global Trade Item Number) do produto, antigo código EAN ou código de barras	
                        else
                            dt.SetCampo("cEAN_I03=SEM GTIN");//GTIN (Global Trade Item Number) do produto, antigo código EAN ou código de barras	

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
                        dt.SetCampo("CFOP_I08=" + ConverterNumeroParaPadraoXML(p.PROD_CFOP));//Código Fiscal de Operações e Prestações	
                        dt.SetCampo("uCom_I09=" + p.PROD_UCOM);//Unidade Comercial	
                        dt.SetCampo("qCom_I10=" + ConverterNumeroParaPadraoXML(p.PROD_QCOM));//Quantidade Comercial	
                        dt.SetCampo("vUnCom_I10a=" + ConverterNumeroParaPadraoXML(p.PROD_VUNCOM));//Valor Unitário de Comercialização	
                        dt.SetCampo("vProd_I11=" + ConverterNumeroParaPadraoXML(p.PROD_VPROD));//Valor Total Bruto dos Produtos ou Serviços
                        if(p.PROD_CEANTRIB != "")
                            dt.SetCampo("cEANTrib_I12=" + p.PROD_CEANTRIB);//GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras	
                        else
                            dt.SetCampo("cEANTrib_I12=SEM GTIN");//GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras	
                        dt.SetCampo("uTrib_I13=" + p.PROD_UTRIB);//Unidade Tributável	
                        dt.SetCampo("qTrib_I14=" + ConverterNumeroParaPadraoXML(p.PROD_QTRIB));//Quantidade Tributável	
                        dt.SetCampo("vUnTrib_I14a=" + ConverterNumeroParaPadraoXML(p.PROD_VUNTRIB));//Valor Unitário de tributação	

                        if(p.PROD_VFRETE != null && p.PROD_VFRETE != 0)
                            dt.SetCampo("vFrete_I15=" + ConverterNumeroParaPadraoXML(p.PROD_VFRETE));//Valor Total do Frete	

                        if (p.PROD_VSEG != null && p.PROD_VSEG != 0)
                            dt.SetCampo("vSeg_I16=" + ConverterNumeroParaPadraoXML(p.PROD_VSEG));//Valor Total do Seg	

                        if (p.PROD_VDESC != null && p.PROD_VDESC != 0)
                            dt.SetCampo("vDesc_I17=" + ConverterNumeroParaPadraoXML(p.PROD_VDESC));//Valor Total do Desconto	

                        if (p.PROD_VOUTRO != null && p.PROD_VOUTRO != 0)
                            dt.SetCampo("vOutro_I17a=" + ConverterNumeroParaPadraoXML(p.PROD_VOUTRO));//Outras despesas	

                        dt.SetCampo("indTot_I17b=" + ConverterNumeroParaPadraoXML(p.PROD_INDTOT));//Indica se valor do Item (vProd) entra no valor total da NF-e (vProd)	
                        dt.SetCampo("xPed_I60=" + p.PROD_XPED);//Número do Pedido de Compra	
                        dt.SetCampo("nItemPed_I61=" + ConverterNumeroParaPadraoXML(p.PROD_NITEMPED));//Item do Pedido de Compra	
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
                        dt.SetCampo("vTotTrib_M02=" + ConverterNumeroParaPadraoXML(p.VTOTTRIB));//Valor aproximado total de tributos federais, estaduais e municipais.	

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
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS = 00	
                                    dt.SetCampo("modBC_N13=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBC));//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("vBC_N15=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBC));//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMS));//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMS));//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("pFCP_N17b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCP));//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCP));//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS = 10
                                else if (icms.ICMS_CST == 10)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS = 10	
                                    dt.SetCampo("modBC_N13=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBC));//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("vBC_N15=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBC));//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMS));//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMS));//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCP));//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCP));//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCP));//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                    dt.SetCampo("modBCST_N18=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBCST));//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + ConverterNumeroParaPadraoXML(icms.ICMS_PMVAST));//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBCST));//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCST));//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMSST));//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSST));//Valor do ICMS ST	
                                                                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPST_N23a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPST));//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPST));//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPST));//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=20
                                else if (icms.ICMS_CST == 20)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS = 20	
                                    dt.SetCampo("modBC_N13=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBC));//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBC));//Percentual da Redução de BC		
                                    dt.SetCampo("vBC_N15=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBC));//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMS));//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMS));//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCP));//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCP));//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCP));//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                                                               //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSDESON));//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + ConverterNumeroParaPadraoXML(icms.ICMS_MOTDESICMS));//Motivo da desoneração do ICMS
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=30
                                else if (icms.ICMS_CST == 30)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS = 30	
                                    dt.SetCampo("pMVAST_N19=" + ConverterNumeroParaPadraoXML(icms.ICMS_PMVAST));//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBCST));//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCST));//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMSST));//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSST));//Valor do ICMS ST	
                                                                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPST_N23a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPST));//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPST));//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPST));//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML	

                                    //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSDESON));//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + ConverterNumeroParaPadraoXML(icms.ICMS_MOTDESICMS));//Motivo da desoneração do ICMS	
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação ICMS = 40, 41, 50
                                else if (icms.ICMS_CST == 40 || icms.ICMS_CST == 41 || icms.ICMS_CST == 50)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS 
                                                                            //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSDESON));//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + ConverterNumeroParaPadraoXML(icms.ICMS_MOTDESICMS));//Motivo da desoneração do ICMS	
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação ICMS=51
                                else if (icms.ICMS_CST == 51)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS 
                                    dt.SetCampo("modBC_N13=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBC));//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBC));//Percentual da Redução de BC	
                                    dt.SetCampo("vBC_N15=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBC));//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMS));//Alíquota do imposto	
                                    dt.SetCampo("vICMSOp_N16a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSOP));//Valor do ICMS da Operação	
                                    dt.SetCampo("pDif_N16b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PDIF));//Percentual do diferimento	
                                    dt.SetCampo("vICMSDif_N16c=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSDIF));//Valor do ICMS diferido	
                                    dt.SetCampo("vICMS_N17=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMS));//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCP));//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCP));//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCP));//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=60
                                else if (icms.ICMS_CST == 60)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS
                                                                            //Sequência XML	
                                    dt.SetCampo("vBCSTRet_N26=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCSTRET));//Valor da BC do ICMS ST retido	
                                    dt.SetCampo("pST_N26a=" + ConverterNumeroParaPadraoXML(icms.ICMS_PST));//Alíquota suportada pelo Consumidor Final	
                                    dt.SetCampo("vICMSSubstituto_N26b=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSSUBSTITUTO));//Valor do ICMS próprio do Substituto	
                                    dt.SetCampo("vICMSSTRet_N27=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSSTRET));//Valor do ICMS ST retido	
                                                                                          //Fim da Sequência XML	

                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPSTRet_N27a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPSTRET));//Valor da Base de Cálculo do FCP retido anteriormente por ST	
                                    dt.SetCampo("pFCPSTRet_N27b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPSTRET));//Percentual do FCP retido anteriormente por Substituição Tributária	
                                    dt.SetCampo("vFCPSTRet_N27d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPSTRET));//Valor do FCP retido por Substituição Tributária	
                                                                                         //Fim da Sequência XML	

                                    //Sequência XML	
                                    dt.SetCampo("pRedBCEfet_N34=" + ConverterNumeroParaPadraoXML(icms.ICMS_REDBCEFET));//Percentual de redução da base de cálculo efetiva	
                                    dt.SetCampo("vBCEfet_N35=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCEFET));//Valor da base de cálculo efetiva	
                                    dt.SetCampo("pICMSEfet_N36=" + ConverterNumeroParaPadraoXML(icms.ICMS_PEFET));//Alíquota do ICMS efetiva	
                                    dt.SetCampo("vICMSEfet_N37=" + ConverterNumeroParaPadraoXML(icms.ICMS_VEFET));//Valor do ICMS efetivo	
                                                                                    //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=70
                                else if (icms.ICMS_CST == 70)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS 
                                    dt.SetCampo("modBC_N13=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBC));//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBC));//Percentual da Redução de BC	
                                    dt.SetCampo("vBC_N15=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBC));//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMS));//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMS));//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCP));//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("vFCP_N17c=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCP));//Valor do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("pFCP_N17b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCP));//Percentual do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                                                               //Sequência XML	
                                    dt.SetCampo("modBCST_N18=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBCST));//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + ConverterNumeroParaPadraoXML(icms.ICMS_PMVAST));//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBCST));//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCST));//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMSST));//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSST));//vICMSST_N23
                                                                                    //Fim da Sequência XML	
                                                                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPST_N23a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPST));//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPST));//Percentual do FCP retido por Substituição Tributária		
                                    dt.SetCampo("vFCPST_N23d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPST));//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML	
                                                                                   //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSDESON));//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + ConverterNumeroParaPadraoXML(icms.ICMS_MOTDESICMS));//Motivo da desoneração do ICMS	
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo Tributação do ICMS=90
                                else if (icms.ICMS_CST == 90)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CST_N12=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS
                                    dt.SetCampo("modBC_N13=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBC));//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBC));//Percentual da Redução de BC	
                                    dt.SetCampo("vBC_N15=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBC));//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMS));//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMS));//Valor do ICMS	
                                                                                //Sequência XML	
                                    dt.SetCampo("vBCFCP_N17a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCP));//Valor da Base de Cálculo do FCP	
                                    dt.SetCampo("pFCP_N17b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCP));//Percentual do Fundo de Combate à Pobreza (FCP)	
                                    dt.SetCampo("vFCP_N17c=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCP));//Valor do Fundo de Combate à Pobreza (FCP)	
                                                                               //Fim da Sequência XML	
                                                                               //Sequência XML	
                                    dt.SetCampo("modBCST_N18=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBCST));//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + ConverterNumeroParaPadraoXML(icms.ICMS_PMVAST));//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBCST));//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCST));//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMSST));//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSST));//vICMSST_N23
                                                                                    //Fim da Sequência XML	
                                                                                    //Sequência XML	
                                    dt.SetCampo("vBCFCPST_N23a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPST));//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPST));//Percentual do FCP retido por Substituição Tributária		
                                    dt.SetCampo("vFCPST_N23d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPST));//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML
                                                                                   //Sequência XML	
                                    dt.SetCampo("vICMSDeson_N28a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSDESON));//Valor do ICMS desonerado	
                                    dt.SetCampo("motDesICMS_N28=" + ConverterNumeroParaPadraoXML(icms.ICMS_MOTDESICMS));//Motivo da desoneração do ICMS	
                                                                                          //Fim da Sequência XML	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------

                                //Grupo de Repasse de ICMS ST retido anteriormente em operações interestaduais com repasses através do Substituto Tributário
                                dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                dt.SetCampo("CST_N12=" + Convert.ToDecimal(ConverterNumeroParaPadraoXML(icms.ICMS_CST)).ToString("D2"));//Tributação do ICMS
                                dt.SetCampo("vBCSTRet_N26=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCSTRET));//Valor do BC do ICMS ST retido na UF remetente	
                                dt.SetCampo("pST_N26a=" + ConverterNumeroParaPadraoXML(icms.ICMS_PST));//Alíquota suportada pelo Consumidor Final	
                                dt.SetCampo("vICMSSubstituto_N26b=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSSUBSTITUTO));//Valor do ICMS próprio do Substituto	
                                dt.SetCampo("vICMSSTRet_N27=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSSTRET));//Valor do ICMS ST retido na UF remetente	
                                                                                      //Sequência XML	
                                dt.SetCampo("vBCFCPSTRet_N27a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPSTRET));//Valor da Base de Cálculo do FCP retido anteriormente por ST	
                                dt.SetCampo("vFCPSTRet_N27d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPSTRET));//Valor do FCP retido por Substituição Tributária	
                                dt.SetCampo("pFCPSTRet_N27b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPSTRET));//Percentual do FCP retido anteriormente por Substituição Tributária	
                                                                                     //Fim da Sequência XML	
                                dt.SetCampo("vBCSTDest_N31=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCSTDEST));//Valor da BC do ICMS ST da UF destino	
                                dt.SetCampo("vICMSSTDest_N32=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSSTDEST));//Valor do ICMS ST da UF destino	
                                                                                        //Sequência XML	
                                dt.SetCampo("pRedBCEfet_N34=" + ConverterNumeroParaPadraoXML(icms.ICMS_REDBCEFET));//Percentual de redução da base de cálculo efetiva	
                                dt.SetCampo("vBCEfet_N35=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCEFET));//Valor da base de cálculo efetiva	
                                dt.SetCampo("vICMSEfet_N37=" + ConverterNumeroParaPadraoXML(icms.ICMS_VEFET));//Valor do ICMS efetivo	
                                dt.SetCampo("pICMSEfet_N36=" + ConverterNumeroParaPadraoXML(icms.ICMS_PEFET));//Alíquota do ICMS efetiva	
                                                                                //Fim da Sequência XML	
                            }
                            else
                            {                           
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT = 1 – Simples Nacional e CSOSN = 101
                                if (icms.ICMS_CSOSN == 101)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + ConverterNumeroParaPadraoXML(icms.ICMS_CSOSN));//Código de Situação da Operação – Simples Nacional	
                                    dt.SetCampo("pCredSN_N29=" + ConverterNumeroParaPadraoXML(icms.ICMS_PCREDSN));//Alíquota aplicável de cálculo do crédito (Simples Nacional).	
                                    dt.SetCampo("vCredICMSSN_N30=" + ConverterNumeroParaPadraoXML(icms.ICMS_VCREDICMSSN));//Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (Simples Nacional)
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN=102, 103, 300 ou 400
                                else if (icms.ICMS_CSOSN == 102 || icms.ICMS_CSOSN == 103 || icms.ICMS_CSOSN == 300 || icms.ICMS_CSOSN == 400)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + ConverterNumeroParaPadraoXML(icms.ICMS_CSOSN));//Código de Situação da Operação – Simples Nacional	
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN=201
                                else if (icms.ICMS_CSOSN == 201)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + ConverterNumeroParaPadraoXML(icms.ICMS_CSOSN));//Código de Situação da Operação – Simples Nacional	
                                    dt.SetCampo("modBCST_N18=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBCST));//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + ConverterNumeroParaPadraoXML(icms.ICMS_PMVAST));//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBCST));//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCST));//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMSST));//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSST));//Valor do ICMS ST	
                                                                                    //Sequência XML
                                    dt.SetCampo("vBCFCPST_N23a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPST));//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPST));//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPST));//Valor do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pCredSN_N29=" + ConverterNumeroParaPadraoXML(icms.ICMS_PCREDSN));//Alíquota aplicável de cálculo do crédito (SIMPLES NACIONAL).	
                                    dt.SetCampo("vCredICMSSN_N30=" + ConverterNumeroParaPadraoXML(icms.ICMS_VCREDICMSSN));//Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (SIMPLES NACIONAL)
                                                                                            //Fim da Sequência XML
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN=202 ou 203
                                else if (icms.ICMS_CSOSN == 202 || icms.ICMS_CSOSN == 203)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + ConverterNumeroParaPadraoXML(icms.ICMS_CSOSN));//Código de Situação da Operação – Simples Nacional	
                                    dt.SetCampo("modBCST_N18=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBCST));//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + ConverterNumeroParaPadraoXML(icms.ICMS_PMVAST));//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBCST));//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCST));//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMSST));//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSST));//Valor do ICMS ST	
                                                                                    //Sequência XML
                                    dt.SetCampo("vBCFCPST_N23a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPST));//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" +ConverterNumeroParaPadraoXML( icms.ICMS_PFCPST));//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPST));//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN = 500
                                else if (icms.ICMS_CSOSN == 500)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + ConverterNumeroParaPadraoXML(icms.ICMS_CSOSN));//Código de Situação da Operação – Simples Nacional
                                                                                 //Sequência XML
                                    dt.SetCampo("vBCSTRet_N26=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCSTRET));//Valor da BC do ICMS ST retido	
                                    dt.SetCampo("pST_N26a=" + ConverterNumeroParaPadraoXML(icms.ICMS_PST));//Alíquota suportada pelo Consumidor Final	
                                    dt.SetCampo("vICMSSubstituto_N26b=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSSUBSTITUTO));//Valor do ICMS próprio do Substituto	
                                    dt.SetCampo("vICMSSTRet_N27=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSSTRET));//Valor do ICMS ST retido	
                                                                                          //Fim da Sequência XML
                                                                                          //Sequência XML
                                    dt.SetCampo("vBCFCPSTRet_N27a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPSTRET));//Valor da Base de Cálculo do FCP retido anteriormente por ST	
                                    dt.SetCampo("pFCPSTRet_N27b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPSTRET));//Percentual do FCP retido anteriormente por Substituição Tributária	
                                    dt.SetCampo("vFCPSTRet_N27d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPSTRET));//Valor do FCP retido por Substituição Tributária	
                                                                                         //Fim da Sequência XML
                                                                                         //Sequência XML
                                    dt.SetCampo("pRedBCEfet_N34=" + ConverterNumeroParaPadraoXML(icms.ICMS_REDBCEFET));//Percentual de redução da base de cálculo efetiva	
                                    dt.SetCampo("vBCEfet_N35=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCEFET));//Valor da base de cálculo efetiva	
                                    dt.SetCampo("pICMSEfet_N36=" + ConverterNumeroParaPadraoXML(icms.ICMS_PEFET));//Alíquota do ICMS efetiva	
                                    dt.SetCampo("vICMSEfet_N37=" + ConverterNumeroParaPadraoXML(icms.ICMS_VEFET));//Valor do ICMS efetivo	
                                                                                    //Fim da Sequência XML
                                }
                                //---------------------------------------------------------------------------------------------------------------------------
                                //Grupo CRT=1 – Simples Nacional e CSOSN=900
                                else if (icms.ICMS_CSOSN == 900)
                                {
                                    dt.SetCampo("orig_N11=" + ConverterNumeroParaPadraoXML(icms.ICMS_ORIG));//Origem da mercadoria	
                                    dt.SetCampo("CSOSN_N12a=" + ConverterNumeroParaPadraoXML(icms.ICMS_CSOSN));//Código de Situação da Operação – Simples Nacional	
                                                                                 //Sequência XML
                                    dt.SetCampo("modBC_N13=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBC));//Modalidade de determinação da BC do ICMS	
                                    dt.SetCampo("pRedBC_N14=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBC));//Percentual da Redução de BC	
                                    dt.SetCampo("vBC_N15=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBC));//Valor da BC do ICMS	
                                    dt.SetCampo("pICMS_N16=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMS));//Alíquota do imposto	
                                    dt.SetCampo("vICMS_N17=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMS));//Valor do ICMS	
                                                                                //Fim da Sequência XML
                                                                                //Sequência XML
                                    dt.SetCampo("modBCST_N18=" + ConverterNumeroParaPadraoXML(icms.ICMS_MODBCST));//Modalidade de determinação da BC do ICMS ST	
                                    dt.SetCampo("pMVAST_N19=" + ConverterNumeroParaPadraoXML(icms.ICMS_PMVAST));//Percentual da margem de valor Adicionado do ICMS ST	
                                    dt.SetCampo("pRedBCST_N20=" + ConverterNumeroParaPadraoXML(icms.ICMS_PREDBCST));//Percentual da Redução de BC do ICMS ST	
                                    dt.SetCampo("vBCST_N21=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCST));//Valor da BC do ICMS ST	
                                    dt.SetCampo("pICMSST_N22=" + ConverterNumeroParaPadraoXML(icms.ICMS_PICMSST));//Alíquota do imposto do ICMS ST	
                                    dt.SetCampo("vICMSST_N23=" + ConverterNumeroParaPadraoXML(icms.ICMS_VICMSST));//Valor do ICMS ST	
                                    dt.SetCampo("pBCOp_N25=" + ConverterNumeroParaPadraoXML(icms.ICMS_PBCOP));//Percentual da BC operação própria	
                                    dt.SetCampo("UFST_N24=" + icms.ICMS_UFST);//UF para qual é devido o ICMS ST	
                                                                              //Fim da Sequência XML
                                                                              //Sequência XML
                                    dt.SetCampo("vBCFCPST_N23a=" + ConverterNumeroParaPadraoXML(icms.ICMS_VBCFCPST));//Valor da Base de Cálculo do FCP retido por Substituição Tributária	
                                    dt.SetCampo("pFCPST_N23b=" + ConverterNumeroParaPadraoXML(icms.ICMS_PFCPST));//Percentual do FCP retido por Substituição Tributária	
                                    dt.SetCampo("vFCPST_N23d=" + ConverterNumeroParaPadraoXML(icms.ICMS_VFCPST));//Valor do FCP retido por Substituição Tributária	
                                                                                   //Fim da Sequência XML
                                                                                   //Sequência XML
                                    dt.SetCampo("pCredSN_N29=" + ConverterNumeroParaPadraoXML(icms.ICMS_PCREDSN));//Alíquota aplicável de cálculo do crédito (SIMPLES NACIONAL).	
                                    dt.SetCampo("vCredICMSSN_N30=" + ConverterNumeroParaPadraoXML(icms.ICMS_VCREDICMSSN));//Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (SIMPLES NACIONAL)	
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
                                dt.SetCampo("vBCUFDest_NA03=" + ConverterNumeroParaPadraoXML(icmsDest.VBCFCPUFDEST));//Valor da BC do ICMS na UF de destino	
                                dt.SetCampo("vBCFCPUFDest_NA04=" + ConverterNumeroParaPadraoXML(icmsDest.VBCFCPUFDEST));//Valor da BC do FCP na UF de destino	
                                dt.SetCampo("pFCPUFDest_NA05=" + ConverterNumeroParaPadraoXML(icmsDest.PFCPUFDEST));//Percentual do ICMS relativo ao Fundo de Combate à Pobreza (FCP) na UF de destino	
                                dt.SetCampo("pICMSUFDest_NA07=" + ConverterNumeroParaPadraoXML(icmsDest.PICMSUFDEST));//Alíquota interna da UF de destino	
                                dt.SetCampo("pICMSInter_NA09=" + ConverterNumeroParaPadraoXML(icmsDest.PICMSINTER));//Alíquota interestadual das UF envolvidas	
                                dt.SetCampo("pICMSInterPart_NA11=" + ConverterNumeroParaPadraoXML(icmsDest.PICMSINTERPART));//Percentual provisório de partilha do ICMS Interestadual	
                                dt.SetCampo("vFCPUFDest_NA13=" + ConverterNumeroParaPadraoXML(icmsDest.VFCPUFDEST));//Valor do ICMS relativo ao Fundo de Combate à Pobreza (FCP) da UF de destino	
                                dt.SetCampo("vICMSUFDest_NA15=" + ConverterNumeroParaPadraoXML(icmsDest.VICMSUFDEST));//Valor do ICMS Interestadual para a UF de destino	
                                dt.SetCampo("vICMSUFRemet_NA17=" + ConverterNumeroParaPadraoXML(icmsDest.VICMSUFREMET));//Valor do ICMS Interestadual para a UF do remetente	
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
                            dt.SetCampo("qSelo_O05=" + ConverterNumeroParaPadraoXML(ipi.IPI_QSELO));//Quantidade de selo de controle	
                            dt.SetCampo("cEnq_O06=" + ipi.IPI_CENQ);//Código de Enquadramento Legal do IPI	

                            //IPI Trib	Grupo do CST 00, 49, 50 e 99
                            if (ipi.IPI_CST == 0 || ipi.IPI_CST == 49 || ipi.IPI_CST == 50 || ipi.IPI_CST == 99)
                            {
                                dt.SetCampo("CST_O09=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(ipi.IPI_CST)).ToString("D2"));//Código da situação tributária do IPI	
                                                                      //Sequência XML
                                dt.SetCampo("vBC_O10=" + ConverterNumeroParaPadraoXML(ipi.IPI_VBC));//Valor da BC do IPI	
                                dt.SetCampo("pIPI_O13=" + ConverterNumeroParaPadraoXML(ipi.IPI_PIPI));//Alíquota do IPI	
                                                                        //Fim da Sequência XML
                                                                        //Sequência XML
                                dt.SetCampo("qUnid_O11=" + ConverterNumeroParaPadraoXML(ipi.IPI_QUNID));//Quantidade total na unidade padrão para tributação (somente para os produtos tributados por unidade)	
                                dt.SetCampo("vUnid_O12=" + ConverterNumeroParaPadraoXML(ipi.IPI_VUNID));//Valor por Unidade Tributável	
                                                                          //Fim da Sequência XML
                                dt.SetCampo("vIPI_O14=" + ConverterNumeroParaPadraoXML(ipi.IPI_VIPI));//Valor do IPI	
                            }
                            //IPINT	Grupo CST 01, 02, 03, 04, 51, 52, 53, 54 e 55
                            else if (ipi.IPI_CST == 01 || ipi.IPI_CST == 02 || ipi.IPI_CST == 03 || ipi.IPI_CST == 04 || ipi.IPI_CST == 51 || ipi.IPI_CST == 52 || ipi.IPI_CST == 53 || ipi.IPI_CST == 54 || ipi.IPI_CST == 55)
                            {
                                dt.SetCampo("CST_O09=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(ipi.IPI_CST)).ToString("D2"));//Código da situação tributária do IPI	
                            }


                            //Imposto de Importação	
                            TECNO_NF_PRODUTOS_II ii = new TECNO_NF_PRODUTOS_II();
                            TECNO_NF_PRODUTOS_II_DAL iiDAL = new TECNO_NF_PRODUTOS_II_DAL();
                            ii = iiDAL.PesquisarTECNO_NF_PRODUTOS_II(ipi.ID_NOTA_FISCAL, ipi.PROD_NITEM);
                            if (ii != null)
                            {
                                dt.SetCampo("vBC_P02=" + ConverterNumeroParaPadraoXML(ii.II_VBC));//Valor BC do Imposto de Importação	
                                dt.SetCampo("vDespAdu_P03=" + ConverterNumeroParaPadraoXML(ii.II_VDESPADU));//Valor despesas aduaneiras	
                                dt.SetCampo("vII_P04=" + ConverterNumeroParaPadraoXML(ii.II_VII));//Valor Imposto de Importação	
                                dt.SetCampo("vIOF_P05=" + ConverterNumeroParaPadraoXML(ii.II_VIOF));//Valor Imposto sobre Operações Financeiras	
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
                                dt.SetCampo("CST_Q06=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(pis.PIS_CST)).ToString("D2"));//Código de Situação Tributária do PIS	
                                dt.SetCampo("vBC_Q07=" + ConverterNumeroParaPadraoXML(pis.PIS_VBC));//Valor da Base de Cálculo do PIS	
                                dt.SetCampo("pPIS_Q08=" + ConverterNumeroParaPadraoXML(pis.PIS_PPIS));//Alíquota do PIS (em percentual)	
                                dt.SetCampo("vPIS_Q09=" + ConverterNumeroParaPadraoXML(pis.PIS_VPIS));//Valor do PIS	
                            }
                            else if (pis.PIS_CST == 3)
                            {
                                //Grupo PIS tributado por Qtde	
                                dt.SetCampo("CST_Q06=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(pis.PIS_CST)).ToString("D2"));//Código de Situação Tributária do PIS	
                                dt.SetCampo("qBCProd_Q10=" + ConverterNumeroParaPadraoXML(pis.PIS_QBCPROD));//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_Q11=" + ConverterNumeroParaPadraoXML(pis.PIS_VALIQPROD));//Alíquota do PIS (em reais)	
                                dt.SetCampo("vPIS_Q09=" + ConverterNumeroParaPadraoXML(pis.PIS_VPIS));//Valor do PIS	
                            }
                            else if (pis.PIS_CST == 4 || pis.PIS_CST == 5 || pis.PIS_CST == 6 || pis.PIS_CST == 7 || pis.PIS_CST == 8 || pis.PIS_CST == 9)
                            {
                                //PISNT	Grupo PIS não tributado
                                dt.SetCampo("CST_Q06=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(pis.PIS_CST)).ToString("D2"));//Código de Situação Tributária do PIS	
                                //Sequência XML 
                            }
                            else if (pis.PIS_CST > 49 && pis.PIS_CST < 99)
                            {
                                //Grupo PIS outras Operações
                                dt.SetCampo("CST_Q06=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(pis.PIS_CST)).ToString("D2"));//Código de Situação Tributária do PIS	
                                dt.SetCampo("vBC_Q07=" + ConverterNumeroParaPadraoXML(pis.PIS_VBC));//Valor da Base de Cálculo do PIS	
                                dt.SetCampo("pPIS_Q08=" + ConverterNumeroParaPadraoXML(pis.PIS_PPIS));//Alíquota do PIS (em percentual)	
                                //Fim da Sequência XML
                                //Sequência XML - Informar os campos Q10 e Q11 se o cálculo do PIS for em valor
                                dt.SetCampo("qBCProd_Q10=" + ConverterNumeroParaPadraoXML(pis.PIS_QBCPROD));//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_Q11=" + ConverterNumeroParaPadraoXML(pis.PIS_VALIQPROD));//Alíquota do PIS (em reais)	
                                //Fim da Sequência XML
                                dt.SetCampo("vPIS_Q09=" + ConverterNumeroParaPadraoXML(pis.PIS_VPIS));//Valor do PIS
                            }
                            else
                            {
                                //PISST	Grupo PIS Substituição Tributária

                                //Sequência XML	 	1-1	 	Informar os campos R02 e R03 para cálculo do PIS em percentual
                                dt.SetCampo("vBC_R02=" + ConverterNumeroParaPadraoXML(pis.PIS_VBC));//Valor da Base de Cálculo do PIS	
                                dt.SetCampo("pPIS_R03=" + ConverterNumeroParaPadraoXML(pis.PIS_PPIS));//Alíquota do PIS (em percentual)	
                                                                        //Fim da Sequência XML
                                                                        //Sequência XML	 	1-1	 	Informar os campos R04 e R05 para cálculo do PIS em valor
                                dt.SetCampo("qBCProd_R04=" + ConverterNumeroParaPadraoXML(pis.PIS_QBCPROD));//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_R05=" + ConverterNumeroParaPadraoXML(pis.PIS_VALIQPROD));//Alíquota do PIS (em reais)	
                                                                                  //Fim da Sequência XML
                                dt.SetCampo("vPIS_R06=" + ConverterNumeroParaPadraoXML(pis.PIS_VPIS));//Valor do PIS	
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
                                dt.SetCampo("CST_S06=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(cofins.COFINS_CST)).ToString("D2"));//Código de Situação Tributária do COFINS	
                                dt.SetCampo("vBC_S07=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VBC));//Valor da Base de Cálculo do COFINS	
                                dt.SetCampo("pCOFINS_S08=" + ConverterNumeroParaPadraoXML(cofins.COFINS_PCOFINS));//Alíquota do COFINS (em percentual)	
                                dt.SetCampo("vCOFINS_S11=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VCOFINS));//Valor do COFINS	
                            }
                            if (cofins.COFINS_CST == 3)
                            {
                                //dt.SetCampo("COFINSQtde=");//Grupo COFINS tributado por Qtde	
                                dt.SetCampo("CST_S06=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(cofins.COFINS_CST)).ToString("D2"));//Código de Situação Tributária do COFINS	
                                dt.SetCampo("qBCProd_S09=" + ConverterNumeroParaPadraoXML(cofins.COFINS_QBCPROD));//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_S10=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VALIQPROD));//Alíquota do COFINS (em reais)	
                                dt.SetCampo("vCOFINS_S11=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VCOFINS));//Valor do COFINS	
                            }

                            else if (pis.PIS_CST == 4 || pis.PIS_CST == 5 || pis.PIS_CST == 6 || pis.PIS_CST == 7 || pis.PIS_CST == 8 || pis.PIS_CST == 9)
                            {
                                //COFINSNT	Grupo COFINS não tributado
                                dt.SetCampo("CST_S06=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(cofins.COFINS_CST)).ToString("D2"));//Código de Situação Tributária do COFINS	
                            }
                            else if (pis.PIS_CST > 49 && pis.PIS_CST < 99)
                            {
                                //Grupo COFINS outras Operações	
                                //Sequência XML	 	1-1	 	Informar os campos S07 e S08 se o cálculo do COFINS em percentual
                                dt.SetCampo("CST_S06=" + Convert.ToInt32(ConverterNumeroParaPadraoXML(cofins.COFINS_CST)).ToString("D2"));//Código de Situação Tributária do COFINS	
                                dt.SetCampo("vBC_S07=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VBC));//Valor da Base de Cálculo do COFINS	
                                dt.SetCampo("pCOFINS_S08=" + ConverterNumeroParaPadraoXML(cofins.COFINS_PCOFINS));//Alíquota do COFINS (em percentual)	
                                //Fim da Sequência XML	
                                //Sequência XML	 	1-1	 	Informar os campos S10 e S11 se o cálculo do COFINS for em valor
                                dt.SetCampo("qBCProd_S09=" + ConverterNumeroParaPadraoXML(cofins.COFINS_QBCPROD));//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_S10=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VALIQPROD));//vAliqProd_S10
                                //Fim da Sequência XML	
                                dt.SetCampo("vCOFINS_S11=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VCOFINS));//Valor do COFINS	
                            }
                            else
                            {
                                //COFINSST	Grupo COFINS Substituição Tributária

                                //Sequência XML	 	1-1	 	Informar os campos T02 e T03 para cálculo do COFINS em percentual
                                dt.SetCampo("vBC_T02=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VBC));//Valor da Base de Cálculo do COFINS	
                                dt.SetCampo("pCOFINS_T03=" + ConverterNumeroParaPadraoXML(cofins.COFINS_PCOFINS));//Alíquota do COFINS (em percentual)
                                                                                    //Fim da Sequência XML	

                                //Sequência XML	 	1-1	 	Informar os campos T04 e T05 para cálculo do COFINS em valor
                                dt.SetCampo("qBCProd_T04=" + ConverterNumeroParaPadraoXML(cofins.COFINS_QBCPROD));//Quantidade Vendida	
                                dt.SetCampo("vAliqProd_T05=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VALIQPROD));//Alíquota do COFINS (em reais)	
                                                                                        //Fim da Sequência XML	

                                dt.SetCampo("vCOFINS_T06=" + ConverterNumeroParaPadraoXML(cofins.COFINS_VCOFINS));//Valor do COFINS	
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
                        if(p.PDEVOL != null && p.PDEVOL > 0)
                            dt.SetCampo("pDevol_UA02=" + ConverterNumeroParaPadraoXML(p.PDEVOL));//Percentual da mercadoria devolvida	

                        if (p.VIPIDEVOL != null && p.VIPIDEVOL > 0)
                            dt.SetCampo("vIPIDevol_UA04=" + ConverterNumeroParaPadraoXML(p.VIPIDEVOL));//Valor do IPI devolvido	

                        //Informações Adicionais
                        if(p.PROD_INFADPROD != null && p.PROD_INFADPROD != "")
                            dt.SetCampo("infAdProd_V01=" + p.PROD_INFADPROD);//Informações Adicionais do Produto	

                        dt.SalvarItem();
                    }

                    //------------------------Total da NF-e	--------------------------------
                    //Grupo Totais referentes ao ICMS
                    dt.SetCampo("vBC_W03=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VBC));//Base de Cálculo do ICMS	
                    dt.SetCampo("vICMS_W04=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VICMS));//Valor Total do ICMS	
                    dt.SetCampo("vICMSDeson_W04a=" + ConverterNumeroParaPadraoXML(nf.ICMStot_VICMSDESON));//Valor Total do ICMS desonerado	
                    dt.SetCampo("vFCPUFDest_W04c=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VFCPUFDEST));//Valor total do ICMS relativo Fundo de Combate à Pobreza (FCP) da UF de destino	
                    dt.SetCampo("vICMSUFDest_W04e=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VICMSUFDEST));//vICMSUFDest_W04e
                    dt.SetCampo("vICMSUFRemet_W04g=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VICMSUFREMET));//Valor total do ICMS Interestadual para a UF do remetente	
                    dt.SetCampo("vFCP_W04h=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VFCP));//Valor Total do FCP (Fundo de Combate à Pobreza)	
                    dt.SetCampo("vBCST_W05=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VBCST));//Base de Cálculo do ICMS ST	
                    dt.SetCampo("vST_W06=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VST));//Valor Total do ICMS ST	
                    dt.SetCampo("vFCPST_W06a=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VFCPST));//Valor Total do FCP (Fundo de Combate à Pobreza) retido por substituição tributária	
                    dt.SetCampo("vFCPSTRet_W06b=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VFCPSTRET));//Valor Total do FCP retido anteriormente por Substituição Tributária	
                    dt.SetCampo("vProd_W07=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VPROD));//Valor Total dos produtos e serviços	
                    dt.SetCampo("vFrete_W08=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VFRETE));//Valor Total do Frete	
                    dt.SetCampo("vSeg_W09=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VSEG));//Valor Total do Seguro	
                    dt.SetCampo("vDesc_W10=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VDESC));//Valor Total do Desconto	
                    dt.SetCampo("vII_W11=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VII));//Valor Total do II	
                    dt.SetCampo("vIPI_W12=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VIPI));//Valor Total do IPI	
                    dt.SetCampo("vIPIDevol_W12a=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VIPIDEVOL));//Valor Total do IPI Devolvido	
                    dt.SetCampo("vPIS_W13=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VPIS));//Valor Total do PIS	
                    dt.SetCampo("vCOFINS_W14=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VCOFINS));//Valor Total do PIS	
                    dt.SetCampo("vOutro_W15=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VOUTRO));//Valor total de Outras despesas	
                    dt.SetCampo("vNF_W16=" + ConverterNumeroParaPadraoXML(nf.ICMSTOT_VNF));//Valor Total da NF-e	
                    dt.SetCampo("vTotTrib_W16a=" + ConverterNumeroParaPadraoXML(nf.VTOTTRIB));//Valor aproximado total de tributos federais, estaduais e municipais.	

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
                    dt.SetCampo("vRetPIS_W24=" + ConverterNumeroParaPadraoXML(nf.RETTRIB_VRETPIS));//Valor Retido de PIS	
                    dt.SetCampo("vRetCOFINS_W25=" + ConverterNumeroParaPadraoXML(nf.RETTRIB_VRETCOFINS));//Valor Retido de COFINS	
                    dt.SetCampo("vRetCSLL_W26=" + ConverterNumeroParaPadraoXML(nf.RETTRIB_VRETCSLL));//Valor Retido de CSLL	
                    dt.SetCampo("vBCIRRF_W27=" + ConverterNumeroParaPadraoXML(nf.RETTRIB_VBCIRRF));//Base de Cálculo do IRRF	
                    dt.SetCampo("vIRRF_W28=" + ConverterNumeroParaPadraoXML(nf.RETTRIB_VIRRF));//Valor Retido do IRRF	
                    dt.SetCampo("vBCRetPrev_W29=" + ConverterNumeroParaPadraoXML(nf.RETTRIB_VBCRETPREV));//Base de Cálculo da Retenção da Previdência Social	
                    dt.SetCampo("vRetPrev_W30=" + ConverterNumeroParaPadraoXML(nf.RETTRIB_VRETPREV));//Valor da Retenção da Previdência Social	
                    
                    //-------------------------------------------------------------------------------------------------------------
                    //Informações do Transporte da NF-e
                    TECNO_NF_TRANSP_TRANSPORTA transp = new TECNO_NF_TRANSP_TRANSPORTA();
                    TECNO_NF_TRANSP_TRANSPORTA_DAL transpDAL = new TECNO_NF_TRANSP_TRANSPORTA_DAL();
                    transp = transpDAL.PesquisarTECNO_NF_TRANSP_TRANSPORTA(nf.ID_NOTA_FISCAL);
                    if (transp != null)
                    {
                        dt.SetCampo("modFrete_X02=" + ConverterNumeroParaPadraoXML(nf.TRANSP_MODFRETE));//Modalidade do frete	
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
                        dt.SetCampo("vServ_X12=" + ConverterNumeroParaPadraoXML(RetTransp.RETTRANSP_VSERV));//Valor do Serviço	
                        dt.SetCampo("vBCRet_X13=" + ConverterNumeroParaPadraoXML(RetTransp.RETTRANSP_VBCRET));//BC da Retenção do ICMS	
                        dt.SetCampo("pICMSRet_X14=" + ConverterNumeroParaPadraoXML(RetTransp.RETTRANSP_PICMSRET));//Alíquota da Retenção	
                        dt.SetCampo("vICMSRet_X15=" + ConverterNumeroParaPadraoXML(RetTransp.RETTRANSP_VICMSRET));//Valor do ICMS Retido	
                        dt.SetCampo("CFOP_X16=" + ConverterNumeroParaPadraoXML(RetTransp.RETTRANSP_CFOP));//CFOP
                        dt.SetCampo("cMunFG_X17=" + ConverterNumeroParaPadraoXML(RetTransp.RETTRANSP_CMUNFG));//Código do município de ocorrência do fato gerador do ICMS do transporte	
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
                        dt.SetCampo("vagao_X25a=" + nf.TRANSP_VAGAO);//Identificação do vagão	
                        dt.SetCampo("balsa_X25b=" + nf.TRANSP_BALSA);//Identificação da balsa	
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
                        dt.SetCampo("qVol_X27=" + ConverterNumeroParaPadraoXML(vol.VOL_QVOL));//Quantidade de volumes transportados	
                        dt.SetCampo("esp_X28=" + vol.VOL_ESP);//Espécie dos volumes transportados	
                        dt.SetCampo("marca_X29=" + vol.VOL_MARCA);//Marca dos volumes transportados	
                        dt.SetCampo("nVol_X30=" + vol.VOL_NVOL);//Numeração dos volumes transportados	

                        if(vol.VOL_PESOL != null && vol.VOL_PESOL > 0 )
                            dt.SetCampo("pesoL_X31=" + ConverterNumeroParaPadraoXML(vol.VOL_PESOL));//Peso Líquido (em kg)	

                        if (vol.VOL_PESOB != null && vol.VOL_PESOB > 0)
                            dt.SetCampo("pesoB_X32=" + ConverterNumeroParaPadraoXML(vol.VOL_PESOB));//Peso Bruto (em kg)	

                        dt.SalvarParte("VOL");
                    }

                    //-------------------------------------------------------------------------------------------------------------
                    //Grupo Lacres
                    //dt.IncluirParte("LACRE");
                    //dt.SetCampo("nLacre_X34=");//Número dos Lacres	
                    //dt.SalvarParte("LACRE");

                    //-------------------------------------------------------------------------------------------------------------
                    //Dados da Cobrança
                    //Grupo Fatura
                    dt.SetCampo("nFat_Y03=" + nf.FAT_NFAT);//Número da Fatura	
                    dt.SetCampo("vOrig_Y04=" + ConverterNumeroParaPadraoXML(nf.FAT_VORIG));//Valor Original da Fatura	
                    dt.SetCampo("vDesc_Y05=" + ConverterNumeroParaPadraoXML(nf.FAT_VDESC));//Valor do desconto	
                    dt.SetCampo("vLiq_Y06=" + ConverterNumeroParaPadraoXML(nf.FAT_VLIQ));//Valor Líquido da Fatura	

                    TECNO_NF_DUPLICATA dup = new TECNO_NF_DUPLICATA();
                    TECNO_NF_DUPLICATA_DAL dupDAL = new TECNO_NF_DUPLICATA_DAL();
                    dup = dupDAL.PesquisarTECNO_NF_DUPLICATA(nf.ID_NOTA_FISCAL);
                    if (dup != null)
                    {
                        //Grupo Parcelas
                        dt.IncluirParte("DUP");
                        dt.SetCampo("nDup_Y08=" + dup.DUP_NDUP);//Número da Parcela	
                        dt.SetCampo("dVenc_Y09=" + dup.DUP_DVENC.ToString("yyyy-MM-dd"));//Data de vencimento	
                        dt.SetCampo("vDup_Y10=" + ConverterNumeroParaPadraoXML(dup.DUP_VDUP));//Valor da Parcela	
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
                        dt.SetCampo("vPag_YA03=" + ConverterNumeroParaPadraoXML(pgto.VPAG));//Valor do Pagamento

                        TECNO_NF_FORMA_PGTO_CARTAO pgtoCartao = new TECNO_NF_FORMA_PGTO_CARTAO();
                        TECNO_NF_FORMA_PGTO_CARTAO_DAL pgtoCartaoDAL = new TECNO_NF_FORMA_PGTO_CARTAO_DAL();
                        pgtoCartao = pgtoCartaoDAL.PesquisarTECNO_NF_FORMA_PGTO_CARTAO(nf.ID_NOTA_FISCAL);
                        if (pgtoCartao != null)
                        {
                            //Grupo de Cartões	
                            dt.SetCampo("tpIntegra_YA04a=" + ConverterNumeroParaPadraoXML(pgtoCartao.TPINTEGRA));//Tipo de Integração para pagamento	
                            dt.SetCampo("CNPJ_YA05=" + pgtoCartao.CNPJ);//CNPJ da instituição de pagamento	
                            dt.SetCampo("tBand_YA06=" + pgtoCartao.TBAND);//Código da bandeira da operadora de cartão de crédito e/ou débito	
                            dt.SetCampo("cAut_YA07=" + pgtoCartao.CAUT);//Número de autorização da operação cartão de crédito e/ou débito	
                        }
                        dt.SalvarParte("YA");
                    }

                    dt.SetCampo("vTroco_YA09=" + ConverterNumeroParaPadraoXML(nf.VTROCO));//Valor do troco	

                    //Informações do Intermediador da Transação
                    dt.SetCampo("CNPJ_YB02=" + nf.INFINTERMED_CNPJ);//CNPJ do Intermediador da Transação (agenciador, plataforma de delivery, marketplace e similar) de serviços e de negócios.	
                    dt.SetCampo("idCadIntTran_YB03=" + nf.INFINTERMED_IDCADINTTRAN);//Identificador cadastrado no intermediador

                    //Informações Adicionais da NF-e
                    dt.SetCampo("infAdFisco_Z02=" + nf.INFADIC_INFADFISCO);//Informações Adicionais de Interesse do Fisco	
                    dt.SetCampo("infCpl_Z03=" + nf.INFADIC_INFCPL);//Informações Complementares de interesse do Contribuinte	

                    /*
                    //Grupo Campo de uso livre do contribuinte
                    dt.IncluirParte("OBSCONT");
                    dt.SetCampo("xCampo_Z05=");//Identificação do campo	
                    dt.SetCampo("xTexto_Z06=");//Conteúdo do campo	
                    dt.SalvarParte("OBSCONT");
                    */

                    //Grupo Campo de uso livre do fisco
                    if (nf.INFADIC_INFADFISCO != null)
                    {
                        dt.IncluirParte("OBSFISCO");
                        dt.SetCampo("xCampo_Z08=" + nf.INFADIC_INFCPL);//Identificação do campo	
                        dt.SetCampo("xTexto_Z09=" + nf.INFADIC_INFADFISCO);//Conteúdo do campo	
                        dt.SalvarParte("OBSFISCO");
                    }

                    /*
                    //Grupo Processo Referenciado
                    dt.IncluirParte("PROCREF");
                    dt.SetCampo("nProc_Z11=" );//Identificador do processo ou ato concessório	
                    dt.SetCampo("indProc_Z12=");//Indicador da origem do processo	
                    dt.SalvarParte("PROCREF");
                    */

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

                    /*
                    //Grupo Fornecimento diário de cana	
                    dt.IncluirParte("FORDIA");
                    dt.SetCampo("dia_ZC05=");//Dia
                    dt.SetCampo("qtde_ZC06=");//Quantidade
                    dt.SetCampo("qTotMes_ZC07=");//Quantidade Total do Mês	
                    dt.SetCampo("qTotAnt_ZC08=");//Quantidade Total Anterior	
                    dt.SetCampo("qTotGer_ZC09=");//Quantidade Total Geral	
                    dt.SalvarParte("FORDIA");
                    */
                    
                    //Grupo Deduções – Taxas e Contribuições	
                    //dt.IncluirParte("DEDUC");
                    //dt.SetCampo("xDed_ZC11=" );//Descrição da Dedução	
                    //dt.SetCampo("vDed_ZC12=");//Valor da Dedução	
                    //dt.SetCampo("vFor_ZC13=");//Valor dos Fornecimentos	
                    //dt.SetCampo("vTotDed_ZC14=");//Valor Total da Dedução	
                    //dt.SetCampo("vLiqFor_ZC15=");//Valor Líquido dos Fornecimentos	
                    //dt.SalvarParte("DEDUC");
                   

                    //Informações do Responsável Técnico
                    dt.SetCampo("CNPJ_ZD02=" + nf.INFTEC_CNPJ);//CNPJ da pessoa jurídica responsável pelo sistema utilizado na emissão do documento fiscal eletrônico	
                    dt.SetCampo("xContato_ZD04=" + nf.INFTEC_XCONTATO);//Nome da pessoa a ser contatada	
                    dt.SetCampo("email_ZD05=" + nf.INFTEC_EMAIL);//E-mail da pessoa jurídica a ser contatada	
                    dt.SetCampo("fone_ZD06=" + nf.INFTEC_FONE);//Telefone da pessoa jurídica/física a ser contatada	

                    //Sequência XML	- Grupo de informações do Código de Segurança do Responsável Técnico -CSTR
                    dt.SetCampo("idCSRT_ZD08=" + nf.INFTEC_IDCSRT);//Identificador do CSRT	
                    dt.SetCampo("hashCSRT_ZD09=" + nf.INFTEC_HASHCSRT);//Hash do CSRT	
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

        private void GerandoArquivoLog(string strDescrição, int CodCaminho)
        {
            try
            {
                DateTime data = DateTime.Now;

                string CaminhoArquivoLog = "";
                if (CodCaminho == 1)//HabilService
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

        private void SalvarXML(string strXML, string strNomeArquivo, string strDescricaoAnexo,TECNO_EVENTO_ERP TCN_EVENTO)
        {
            try
            {
                int intCodigoAnexo = 0;

                string strCaminhoDiretorio = strNomeArquivo;

                if (Directory.Exists(strCaminhoDiretorio))
                    Directory.Delete(strCaminhoDiretorio);

                FileStream file = new FileStream(strCaminhoDiretorio, FileMode.Create);

                StreamWriter sw = new StreamWriter(file);

                sw.WriteLine(strXML);

                sw.Close(); file.Close();

                file = new FileStream(strCaminhoDiretorio, FileMode.Open);

                BinaryReader br = new BinaryReader(file);

                byte[] byteArquivo = br.ReadBytes((int)file.Length);

                file.Close();

                TECNO_ANEXO_DAL TCN_ANEXO_DAL = new TECNO_ANEXO_DAL();

                List<TECNO_ANEXO> LISTA_TCN_ANEXO = new List<TECNO_ANEXO>();

                LISTA_TCN_ANEXO = TCN_ANEXO_DAL.ListarTECNO_ANEXO(Convert.ToDecimal(TCN_EVENTO.CHAVE_BUSCA));

                intCodigoAnexo = LISTA_TCN_ANEXO.Count();

                TECNO_ANEXO TCN_ANEXO = new TECNO_ANEXO();

                TCN_ANEXO.CHAVE_BUSCA = TCN_EVENTO.CHAVE_BUSCA.ToString();

                TCN_ANEXO.CD_ANEXO = intCodigoAnexo + 2;

                TCN_ANEXO.DS_ARQUIVO = strDescricaoAnexo;

                TCN_ANEXO.TX_CONTEUDO = byteArquivo;

                TCN_ANEXO.EX_ARQUIVO = "XML";

                TCN_ANEXO.TP_ACAO = Convert.ToInt32(TCN_EVENTO.TP_ACAO);

                TCN_ANEXO_DAL.InserirTECNO_ANEXO(TCN_ANEXO);
            }
            catch (Exception ex)
            {
                GerandoArquivoLog(ex.Message, 1);
            }
        }
    }
}