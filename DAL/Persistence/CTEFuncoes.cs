using System;
using System.Collections.Generic;
using DAL.Model;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CTeX;
using CTeDataSetX;

namespace DAL.Persistence
{
    public class CTEFuncoes
    {
        
        List<IntegraDocumentoEletronico> ListaIntegracaoDocEletronico = new List<IntegraDocumentoEletronico>();
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        public CTeX.spdCTeX _spdCTeX = new CTeX.spdCTeX();
        public CTeDataSetX.spdCTeDataSetX _spdCTeDataSetX = new CTeDataSetX.spdCTeDataSetX();
        public void EnviarDesacordo()
        {
            try
            {

                DBTabelaCampos rowp2 = new DBTabelaCampos();
                rowp2.Filtro = "IN_REG_DEVOLVIDO";
                rowp2.Inicio = "0";
                rowp2.Fim = "0";
                rowp2.Tipo = "SMALLINT";
                listaT.Add(rowp2);

                IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
                ListaIntegracaoDocEletronico = integraDAL.ListarIntegracaoDocEletronicoCompleto(listaT);
                int Contador = 0;
                foreach (IntegraDocumentoEletronico integracao in ListaIntegracaoDocEletronico)
                {

                   if (integracao.CodigoAcao == 124 && integracao.IntegracaoProcessando == 0 && integracao.IntegracaoRecebido == 0 && integracao.IntegracaoRetorno == 0 && integracao.RegistroDevolvido == 0 && integracao.RegistroEnviado == 1 && integracao.Mensagem == "")
                   {
                        Doc_CTe doc = new Doc_CTe();
                        Doc_CTeDAL docDAL = new Doc_CTeDAL();
                        doc = docDAL.PesquisarDocumento(integracao.CodigoDocumento);

                        Empresa empresa = new Empresa();
                        EmpresaDAL empresaDAL = new EmpresaDAL();
                        empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);

                        Pessoa pes = new Pessoa();
                        PessoaDAL pesDAL = new PessoaDAL();
                        pes = pesDAL.PesquisarViewPessoa(empresa.CodigoPessoa);

                        Pessoa pesEmitente = new Pessoa();
                        pesEmitente = pesDAL.PesquisarViewPessoa(doc.Cpl_CodigoTransportador);

                        GerandoArquivoLog("Carregando .INI", 1);
                        string DiretorioEXE = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\..\..\..\..\Modulos";
                        _spdCTeX.LoadConfig(DiretorioEXE + @"\TecnoSpeed\CTe\Arquivos\cteConfig.ini");

                        GerandoArquivoLog("Configurando .INI", 1);
                        if(pesEmitente.Cpl_Estado.Length >= 2)
                            _spdCTeX.UF = pesEmitente.Cpl_Estado.Substring(0, 2);//UF do Emitente
                        _spdCTeX.DiretorioEsquemas = DiretorioEXE + @"\TecnoSpeed\CTe\Arquivos\Esquemas";
                        _spdCTeX.DiretorioTemplates = DiretorioEXE + @"\TecnoSpeed\CTe\Arquivos\Templates";
                        _spdCTeX.ArquivoServidoresHom = DiretorioEXE + @"\TecnoSpeed\CTe\Arquivos\cteServidoresHom.ini";
                        _spdCTeX.ArquivoServidoresProd = DiretorioEXE + @"\TecnoSpeed\CTe\Arquivos\cteServidoresProd.ini";
                        _spdCTeX.ImpressaoModeloRetrato = DiretorioEXE + @"\TecnoSpeed\CTe\Arquivos\Templates\3.00\DACTE\Retrato.rtm";
                        _spdCTeX.ImpressaoModeloPaisagem = DiretorioEXE + @"\TecnoSpeed\CTe\Arquivos\Templates\cce\Impressao\modeloCCe.rtm";
                        _spdCTeX.CNPJ = pes.Cpl_Inscricao;
                        string c = _spdCTeX.VersaoManual.ToString();
                        DBTabelaDAL db = new DBTabelaDAL();

                        List<EventoEletronicoDocumento> ListaEventoDocEletronico = new List<EventoEletronicoDocumento>();
                        EventoEletronicoDocumentoDAL eventosEletronicosDAL = new EventoEletronicoDocumentoDAL();
                        ListaEventoDocEletronico = eventosEletronicosDAL.ObterEventosEletronicos(doc.CodigoDocumento);

                        foreach (var evento in ListaEventoDocEletronico)
                        {
                            
                            if (Contador == 0) { 
                                if (evento.CodigoSituacao != 121 && evento.CodigoTipoEvento == 120)
                                {
                                    Contador++;
                                    try
                                    {
                                        integracao.IntegracaoRecebido = 1;
                                        integraDAL.AtualizarIntegraDocEletronico(integracao);
                                        
                                        string consultaCTE = _spdCTeX.ConsultarCT(doc.ChaveAcesso);
                                        string CodigoSituacaoConsulta = BuscarValorTagXML(consultaCTE, "infProt", "cStat");
                                        GerandoArquivoLog("Fazendo consulta do CT-e " + doc.ChaveAcesso, 1);
                                        
                                        if (CodigoSituacaoConsulta == "100")
                                        {
                                           // _spdCTeX.UF = pes.Cpl_Estado.Substring(0, 2);//UF do remetente
                                            GerandoArquivoLog("CT-e está Autorizado...", 1);
                                            doc.CodigoSituacao = 40;

                                            string XML = _spdCTeX.EnviarDesacordo(doc.ChaveAcesso,
                                                            evento.DataHoraEvento.ToString("yyyy-MM-ddTHH:mm:ss") + "-03:00",
                                                            evento.Motivo,
                                                            evento.NumeroSequencia.ToString());

                                            GerandoArquivoLog("Evento Eletronico do documento " + evento.CodigoEvento + " enviado... aguardando retorno ", 1);
                                            integracao.IntegracaoProcessando = 1;
                                            integraDAL.AtualizarIntegraDocEletronico(integracao);

                                            
                                            evento.Retorno = BuscarValorTagXML(XML, "infEvento", "xMotivo");
                                            string CodigoRetorno = BuscarValorTagXML(XML, "infEvento", "cStat");

                                            GerandoArquivoLog("Evento enviado... retorno - " + evento.Retorno, 1);
                                            integracao.IntegracaoRetorno = 1;
                                            integraDAL.AtualizarIntegraDocEletronico(integracao);

                                            if (CodigoRetorno == "135" || CodigoRetorno == "134" || CodigoRetorno == "136")
                                            {
                                                evento.CodigoSituacao = 121;
                                                byte[] XMLRetorno = null;
                                                XMLRetorno = Encoding.UTF8.GetBytes(XML);
                                                DBTabelaDAL dt = new DBTabelaDAL();
                                                SalvarAnexos(doc.CodigoDocumento, XMLRetorno, dt.ObterDataHoraServidor(), integracao, "Retorno do Envio de Desacordo!");
                                                GerandoArquivoLog("Salvando XML em anexo...", 1);
                                            }
                                            else
                                            {
                                                evento.CodigoSituacao = 122;
                                            }


                                        }
                                        else if (CodigoSituacaoConsulta == "218")
                                        {
                                            GerandoArquivoLog("Impossível efetuar o desacordo, CT-e está CANCELADO", 1);
                                            GerandoArquivoLog("CT-e " + doc.ChaveAcesso + " está CANCELADO...", 1);
                                            evento.CodigoSituacao = 122;
                                            doc.CodigoSituacao = 41;
                                        }
                                        else if (CodigoSituacaoConsulta == "")
                                        {
                                            evento.Retorno = BuscarValorTagXML(consultaCTE, "retConsSitCTe", "xMotivo");
                                            evento.CodigoSituacao = 122;
                                            doc.CodigoSituacao = 39;
                                        }
                                        else
                                        {
                                            GerandoArquivoLog("Impossível efetuar o desacordo, CT-e não AUTORIZADO", 1);
                                            GerandoArquivoLog("CT-e " + doc.ChaveAcesso + " não está autorizada...", 1);

                                            evento.CodigoSituacao = 122;
                                            doc.CodigoSituacao = 39;
                                        }
                                        //docDAL.Atualizar,
                                    }
                                    catch (Exception ex)
                                    {
                                        evento.Retorno = ex.ToString();
                                        GerandoArquivoLog("ERRO - " + ex.ToString(), 1);
                                    }

                                    //ListaEventoDocEletronico.RemoveAll(x => x.CodigoEvento == evento.CodigoEvento);
                                    //EventoEletronicoDocumento NovoEvento = new EventoEletronicoDocumento();
                                    //NovoEvento = evento;
                                    //ListaEventoDocEletronico.Add(NovoEvento);


                                }
                            }
                            eventosEletronicosDAL.AtualizarEventoEletronico(evento);
                            GerandoArquivoLog("Evento eletronico do documento " + doc.CodigoDocumento + " atualizado com sucesso", 1);
                            integracao.Mensagem = evento.Retorno;
                            integracao.RegistroDevolvido = 1;
                            integraDAL.AtualizarIntegraDocEletronico(integracao);
                        }

                        integraDAL.AtualizarSituacaoDocumentoCTe(doc);
                        GerandoArquivoLog("Documento " + doc.CodigoDocumento + " Atualizado com sucesso", 1);
                    }
                }  
                if(Contador == 0)
                {
                    GerandoArquivoLog("Nenhum desacordo enviado para CT-e", 1);
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("ERRO: " + ex.ToString(), 1);
            }
        }
        public string BuscarValorTagXML(string XML, string NomeTagPai, string NomeTagFilho)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XML);
            XmlNodeList roll = xmlDoc.GetElementsByTagName(NomeTagPai);

            foreach (XmlNode xn in roll)
            {
                return xn[NomeTagFilho].InnerText;
            }
            return "";
        }
        public void GerandoArquivoLog(string strDescrição, int CodCaminho)
        {
            DateTime data = DateTime.Now;

            string CaminhoArquivoLog = "";
            if (CodCaminho == 1)//HabilServiceNFSe
                CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\..\\..\\..\\..\\Modulos\\Log\\Log-" + data.ToString("dd-MM-yyyy") + ".txt";
            else//HabilInformatica
                CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\Log-" + data.ToString("dd-MM-yyyy") + ".txt";

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
        public void SalvarAnexos(decimal CodigoDocumento, byte[] Arquivo, DateTime data, IntegraDocumentoEletronico integra, string StrDescricao)
        {

            AnexoDocumento anexo = new AnexoDocumento();
            AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
            anexo.CodigoDocumento = Convert.ToDecimal(CodigoDocumento);
            anexo.CodigoMaquina = integra.CodigoMaquina;
            anexo.CodigoUsuario = integra.CodigoUsuario;
            anexo.DataHoraLancamento = data;
            anexo.DescricaoArquivo = StrDescricao;
            anexo.ExtensaoArquivo = "XML";
            anexo.Arquivo = Arquivo;
            anexo.NaoEditavel = 1;
            anexo.NomeArquivo = anexoDAL.GerarGUID(anexo.ExtensaoArquivo);
            anexoDAL.InserirXMLDocumento(anexo);
        }
    }
}
