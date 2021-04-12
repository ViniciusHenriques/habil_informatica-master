using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;
using System.Drawing;
using System.Text;
using NFSeX;
using NFSeConverterX;
using System.IO;
using System.Xml;


namespace SoftHabilInformatica.HabilEletronico
{
    public partial class MonMDe : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        List<IntegraDocumentoEletronico> ListaIntegracaoDocEletronico = new List<IntegraDocumentoEletronico>();
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["TabFocada"] != null)
                PanelSelect = Session["TabFocada"].ToString();
            else
                if (!IsPostBack)
            {
                PanelSelect = "home";
                Session["TabFocadaNFS"] = "home";
            }




            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            if (Session["LST_ACESSO_MDE"] != null)
            {
                listaT = (List<DBTabelaCampos>)Session["LST_ACESSO_MDE"];
                if (listaT.Count != 0)
                    btnConsultar_Click(sender, e);
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;

                


                List<Permissao> lista = new List<Permissao>();
                PermissaoDAL r1 = new PermissaoDAL();
                lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                               Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                               "MonMDe.aspx");
                lista.ForEach(delegate (Permissao x)
                {
                    if (!x.AcessoCompleto)
                    {

                        if (!x.AcessoConsulta)
                        {
                            grdGrid.Enabled = false;
                        }

                    }
                });

            }
            
            btnConsultar_Click(sender, e);
            

        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {

            IntegraDocumentoEletronicoDAL r = new IntegraDocumentoEletronicoDAL();

            ListaIntegracaoDocEletronico = r.ListarIntegracaoDocEletronicoCompleto(listaT);
            grdGrid.DataSource = ListaIntegracaoDocEletronico;
            grdGrid.DataBind();
            if (grdGrid.Rows.Count == 0)
            {
                ShowMessage("Notas(s) Fiscais de Serviço não encontrado(s) mediante a pesquisa realizada.", MessageType.Info);
                Session["LST_ACESSO_MDE"] = null;
            }

        }


        protected void grdGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // adiciona o novo indice
            grdGrid.PageIndex = e.NewPageIndex;
            // Carrega os dados
            btnConsultar_Click(sender, e);
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["LST_ACESSO_MDE"] = null;
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/Pages/WelCome.aspx");
            this.Dispose();
        }


        public NFSeX.spdNFSeX _spdNFSeX = new NFSeX.spdNFSeX();
        public NFSeDataSetX.spdNFSeDataSetX _spdNFSeDataSetX = new NFSeDataSetX.spdNFSeDataSetX();
        public NFSeX.spdProxyNFSeX ProxyNFSe = new NFSeX.spdProxyNFSeX();
        public spdNFSeConverterX _spdNFSeConverterX = new spdNFSeConverterX();

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime data = RnTab.ObterDataHoraServidor();
            IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();

            //GerandoArquivoLog("Submetendo ao Enviar Nota... ");
            string DiretorioEXE = @"C:\HabilWeb\Habil_Informatica\Habil_Informatica\Habil_Informatica\";

            ListaIntegracaoDocEletronico = integraDAL.ListarIntegracaoDocEletronicoCompleto(listaT);

            //--------------------------Configuracao do .INI------------------------------------------
            _spdNFSeX.LoadConfig(DiretorioEXE + "\\TecnoSpeed\\NFSe\\Arquivos\\nfseConfig.ini");
            ProxyNFSe.ComponenteNFSe = _spdNFSeX;
            _spdNFSeConverterX.DiretorioEsquemas = _spdNFSeX.DiretorioEsquemas;
            _spdNFSeConverterX.DiretorioScripts = DiretorioEXE + @"\\TecnoSpeed\\NFSe\\Arquivos\\Scripts";
            _spdNFSeConverterX.Cidade = _spdNFSeX.Cidade;
            //------------------------------------------------------------------------------------------

            GerandoArquivoLog("Listagem de Certificados: " + _spdNFSeX.ListarCertificados());
            //Console.Write(_spdNFSeX.ListarCertificados());

            decimal CodigoNFSe = 0;
            try
            {
                int Contador = 0;
                foreach (IntegraDocumentoEletronico integracao in ListaIntegracaoDocEletronico)
                {

                    if (integracao.CodigoAcao == 43)
                    {
                        Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                        Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
                        NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(integracao.CodigoDocumento));




                        if (NFSe.ChaveAcesso == "" && NFSe.Protocolo == "")
                        {
                            Contador++;
                            GerandoArquivoLog("Solicitação de autorização da NFS-e " + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico);
                            //SOLICITACAO DE AUTORIZADA DE NFS-e X
                            //integracao recebeu a nota para autorizacao
                            integracao.IntegracaoRecebido = 1;
                            GerandoArquivoLog("gerando XML da NFS-e" + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico);
                            string RPS = GerarRPS(integracao);
                            CodigoNFSe = NFSe.CodigoNotaFiscalServico;
                            GerandoArquivoLog(RPS);

                            GerandoArquivoLog("Assinando XML da NFS-e " + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico);
                            RPS = ProxyNFSe.Assinar(RPS, "");//Assinar XML gerado
                            GerandoArquivoLog(RPS);
                            byte[] Arquivo2 = Encoding.UTF8.GetBytes(RPS);
                            SalvarAnexos(NFSe, Arquivo2, data, integracao, "Envio NFS-e");

                            GerandoArquivoLog("Enviando XML da NFS-e " + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico);
                            string protocolo = ProxyNFSe.Enviar(RPS, "");// Envio XML assinado

                            decimal numero;
                            if (decimal.TryParse(protocolo, out numero))
                            {
                                integracao.IntegracaoProcessando = 1;
                                //txt - nota enviado com sucesso... aguardando resposta
                                GerandoArquivoLog("NFS-e Enviada...");
                            }
                            else
                            {
                                GerandoArquivoLog("Erro ao enviar NFS-e");
                            }


                            GerandoArquivoLog("Consultando o envio de lote - protocolo n° " + protocolo);
                            string consultaLote = ProxyNFSe.ConsultarLote(protocolo, "");// Consultar lote
                                                                                         //txt - Fazendo consulta do envio de lote

                            GerandoArquivoLog("Convertendo retorno da consulta de lote");

                            //------------------------------------Converter XML-----------------------------------------
                            NFSeConverterX.spdRetConsultaLoteNFSe retorno2 = new NFSeConverterX.spdRetConsultaLoteNFSe();
                            retorno2 = _spdNFSeConverterX.ConverterRetConsultarLoteNFSeTipo(consultaLote);
                            String motivo = retorno2.Motivo;//Mostra situacao da nota
                                                            //------------------------------------------------------------------------------------------
                                                            //CONVERSAO DO RETORNO DE CONSULTA DE LOTE


                            integracao.IntegracaoRetorno = 1;

                            string retornoChaveCancelamento = _spdNFSeConverterX.ConverterRetConsultarLoteNFSe(consultaLote, "ChaveCancelamento");//retorna chave de cancelamento

                            String[] StrChaveAcesso = retornoChaveCancelamento.Split(';');
                            string chaveAcesso = StrChaveAcesso[0];// chave de acesso                   

                            GerandoArquivoLog("Obtendo Chave de acesso " + chaveAcesso);
                            if (chaveAcesso != "EMPROCESSAMENTO")
                            {
                                //em processamento
                                integracao.RegistroDevolvido = 1;

                                //FAZER CONSULTA
                                //CHAVE DE ACESSO GERADA IMEDIATAMENTO
                                GerandoArquivoLog("Chave de acesso retornada com sucesso...");
                            }
                            else
                            {

                                //CHAVE DE ACESSO NÃO RETORNADA
                                GerandoArquivoLog("Chave de acesso não retornada...");
                                IntegraDocumentoEletronico ReenvioDoc = new IntegraDocumentoEletronico();
                                ReenvioDoc.CodigoDocumento = NFSe.CodigoNotaFiscalServico;
                                ReenvioDoc.CodigoAcao = 45;
                                ReenvioDoc.RegistroEnviado = 1;
                                ReenvioDoc.IntegracaoRecebido = 0;
                                ReenvioDoc.IntegracaoProcessando = 0;
                                ReenvioDoc.IntegracaoRetorno = 0;
                                ReenvioDoc.RegistroDevolvido = 0;
                                ReenvioDoc.RegistroMensagem = 0;
                                ReenvioDoc.Mensagem = "";
                                ReenvioDoc.CodigoMaquina = integracao.CodigoMaquina;
                                ReenvioDoc.CodigoUsuario = integracao.CodigoUsuario;
                                integraDAL.Inserir(ReenvioDoc);
                            }

                            GerandoArquivoLog("Consultando da NFS-e" + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico);
                            string consulta = ProxyNFSe.ConsultarNota(chaveAcesso, "");//consultar nota
                            byte[] Arquivo = Encoding.UTF8.GetBytes(consulta);

                            SalvarAnexos(NFSe, Arquivo, data, integracao, "Envio Autorização NFS-e");
                            GerandoArquivoLog("Anexo salvo com sucesso...");
                            //------------------------------------Converter XML-----------------------------------------
                            NFSeConverterX.spdRetConsultaNFSe retorno = new NFSeConverterX.spdRetConsultaNFSe();
                            retorno = _spdNFSeConverterX.ConverterRetConsultarNFSeTipo(consulta);
                            String Situacao = retorno.Situacao;//Mostra situacao da nota
                                                               //------------------------------------------------------------------------------------------

                            NFSe.ChaveAcesso = chaveAcesso;
                            NFSe.Protocolo = Convert.ToString(protocolo);


                            if (Situacao == "AUTORIZADA")
                            {
                                integracao.RegistroMensagem = 1;
                                NFSe.CodigoSituacao = 40;
                                //NOTA AUTORIZADA COM SUCESSO
                                GerandoArquivoLog("NFS-e autorizada com sucesso...");
                            }
                            else if (Situacao == "CANCELADO")
                            {
                                integracao.RegistroMensagem = 1;
                                NFSe.CodigoSituacao = 41;
                                //NOTA CANCELADA COM SUCESSO
                                GerandoArquivoLog("NFS-e cancelada com sucesso...");
                            }
                            else
                            {
                                NFSe.CodigoSituacao = 39;
                                //NOTA REJEITADA
                                //MOTIVO DA REJEIÇÃO
                                GerandoArquivoLog("NFS-e Rejeitada..." + motivo);

                            }
                            integracao.Mensagem = Situacao + motivo + retorno.Motivo;

                            integraDAL.AtualizarIntegraDocEletronico(integracao);
                            integraDAL.AtualizarDocumentoNFSe(NFSe);
                            int io = integracao.CodigoUsuario;
                            EventoDocumento(NFSe, data, integracao);
                            CodigoNFSe = 0;
                            if (Situacao == "AUTORIZADA" || Situacao == "CANCELADA")
                            {
                                List<IntegraDocumentoEletronico> ListaRejeitadas = new List<IntegraDocumentoEletronico>();
                                ListaRejeitadas = integraDAL.ListarIntegraDocEletronico("CD_DOCUMENTO", "NUMERIC", NFSe.CodigoNotaFiscalServico.ToString(), "", 43);
                                foreach (IntegraDocumentoEletronico i in ListaRejeitadas)
                                {
                                    integraDAL.Excluir(Convert.ToInt64(i.CodigoDocumento));
                                }
                                integraDAL.Excluir(Convert.ToInt64(integracao.Codigo));
                                GerandoArquivoLog("Registro Apagado do integrador de documentos eletrônicos!");
                            }
                        }
                    }

                }
                if (Contador == 0)
                {
                    GerandoArquivoLog("Nenhuma NFSe para ser Autorizada");
                }

            }
            catch (Exception ex)
            {

                GerandoArquivoLog("Ocorreu erro durante o processo de autorização - " + ex.ToString());
                Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
                IntegraDocumentoEletronico integra = new IntegraDocumentoEletronico();
                integra = integraDAL.PesquisarIntegracaoDocEletronico(CodigoNFSe, 43);
                if (integra != null)
                {
                    
                    NFSe = NFSeDAL.PesquisarNotaFiscalServico(integra.CodigoDocumento);
                    NFSe.CodigoSituacao = 39;
                    NFSe.ChaveAcesso = "ERRO";
                    NFSe.CodigoNotaFiscalServico = Convert.ToInt64(integra.CodigoDocumento);
                    NFSe.Protocolo = "";
                    integraDAL.AtualizarDocumentoNFSe(NFSe);
                    EventoDocumento(NFSe, data, integra);
                    integra.Mensagem = "Ocorreu um erro no processo de envio NFS-e... Verifique os dados informados!";
                    integraDAL.AtualizarIntegraDocEletronico(integra);
                    GerandoArquivoLog("Nota rejeitada:" + ex.ToString());
                }
                btnEnviar_Click(sender,e);
                CodigoNFSe = 0;
            }

        }
        public void GerandoArquivoLog(string strDescrição)
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime data = RnTab.ObterDataHoraServidor();


            string CaminhoArquivoLog = @"C:\HabilWeb\Habil_Informatica\Habil_Informatica\Habil_Informatica\Log\Log-" + data.ToString("dd-MM-yyyy") + ".txt";

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
        public void EventoDocumento(Doc_NotaFiscalServico NFSe, DateTime data, IntegraDocumentoEletronico integra)
        {
            List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
            EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
            ListaEvento = eventoDAL.ObterEventos(NFSe.CodigoNotaFiscalServico);
            int contador = 0;
            foreach (EventoDocumento evento in ListaEvento)
            {
                contador++;
            }
            EventoDocumento eventodoc = new EventoDocumento();
            eventodoc.CodigoDocumento = Convert.ToDecimal(NFSe.CodigoNotaFiscalServico);
            eventodoc.CodigoMaquina = integra.CodigoMaquina;
            eventodoc.CodigoUsuario = integra.CodigoUsuario;
            eventodoc.CodigoSituacao = NFSe.CodigoSituacao;
            eventodoc.DataHoraEvento = data;
            eventodoc.CodigoEvento = contador + 1;
            eventoDAL.Inserir(eventodoc, NFSe.CodigoNotaFiscalServico);


        }
        public void SalvarAnexos(Doc_NotaFiscalServico NFSe, byte[] Arquivo, DateTime data, IntegraDocumentoEletronico integra, string StrDescricao)
        {

            AnexoDocumento anexo = new AnexoDocumento();
            AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
            anexo.CodigoDocumento = Convert.ToInt32(NFSe.CodigoNotaFiscalServico);
            anexo.CodigoMaquina = integra.CodigoMaquina;
            anexo.CodigoUsuario = integra.CodigoUsuario;
            anexo.DataHoraLancamento = data;
            anexo.DescricaoArquivo = StrDescricao;
            anexo.ExtensaoArquivo = "XML";
            anexo.Arquivo = Arquivo;
            anexo.NomeArquivo = anexoDAL.GerarGUID(anexo.ExtensaoArquivo);
            anexoDAL.InserirXMLDocumento(anexo);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            NFSeFuncoes fun = new NFSeFuncoes();
            fun.btnCancelar_Click();
        }

        protected void btnConsultarNota_Click(object sender, EventArgs e)
        {
            
                DBTabelaDAL RnTab = new DBTabelaDAL();
                DateTime data = RnTab.ObterDataHoraServidor();
                IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
                ListaIntegracaoDocEletronico = integraDAL.ListarIntegracaoDocEletronicoCompleto(listaT);
            //--------------------------Configuracao do .INI------------------------------------------
            string DiretorioEXE = @"C:\HabilWeb\Habil_Informatica\Habil_Informatica\Habil_Informatica\";

                _spdNFSeX.LoadConfig(DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\nfseConfig.ini");
                ProxyNFSe.ComponenteNFSe = _spdNFSeX;
                _spdNFSeConverterX.DiretorioEsquemas = _spdNFSeX.DiretorioEsquemas;
                _spdNFSeConverterX.DiretorioScripts = DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\Scripts";
                _spdNFSeConverterX.Cidade = _spdNFSeX.Cidade;
                //------------------------------------------------------------------------------------------
                try
                {

                    int Contador = 0;
                    foreach (IntegraDocumentoEletronico integracao in ListaIntegracaoDocEletronico)
                    {
                        if (integracao.CodigoAcao == 45 && integracao.RegistroDevolvido != 1)
                        {

                            //SOLITCITACAO DE CONSULTA DE NOTA

                            Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                            Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
                            NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(integracao.CodigoDocumento));


                            if ((NFSe.ChaveAcesso != "" || NFSe.Protocolo != ""))
                            {
                                Contador++;
                                GerandoArquivoLog("Solicitação de consulta da NFS-e N° " + NFSe.NumeroDocumento);
                                integracao.RegistroEnviado = 1;
                                integracao.IntegracaoRecebido = 1;


                                string StrChaveAcesso = "";
                                if (NFSe.ChaveAcesso == "EMPROCESSAMENTO")
                                {

                                    string consultaLote = ProxyNFSe.ConsultarLote(NFSe.Protocolo.ToString(), "");
                                    //txt - Fazendo consulta do envio de lote
                                    GerandoArquivoLog("Consultando o envio de lote - protocolo n° " + NFSe.Protocolo.ToString());


                                    //------------------------------------Converter XML-----------------------------------------
                                    NFSeConverterX.spdRetConsultaLoteNFSe retorno2 = new NFSeConverterX.spdRetConsultaLoteNFSe();
                                    string retornoChaveCancelamento = _spdNFSeConverterX.ConverterRetConsultarLoteNFSe(consultaLote, "ChaveCancelamento");
                                    string motivo = retorno2.Motivo;//Mostra situacao da nota
                                                                    //------------------------------------------------------------------------------------------


                                    string[] ChaveAcesso = retornoChaveCancelamento.Split(';');
                                    StrChaveAcesso = ChaveAcesso[0];// chave de acesso   

                                    List<IntegraDocumentoEletronico> ListaIntegraAutorizar = new List<IntegraDocumentoEletronico>();
                                    ListaIntegraAutorizar = integraDAL.ListarIntegraDocEletronico("CD_DOCUMENTO", "NUMERIC", integracao.CodigoDocumento.ToString(), "", 43);
                                    foreach (IntegraDocumentoEletronico integraAutorizar in ListaIntegraAutorizar)
                                    {
                                        integraDAL.Excluir(Convert.ToInt64(integraAutorizar.Codigo));
                                    }


                                    if (StrChaveAcesso == "EMPROCESSAMENTO")
                                    {
                                        List<IntegraDocumentoEletronico> ListaIntegraConsultar = new List<IntegraDocumentoEletronico>();
                                        ListaIntegraConsultar = integraDAL.ListarIntegraDocEletronico("CD_DOCUMENTO", "NUMERIC", NFSe.CodigoNotaFiscalServico.ToString(), "", 45);
                                        if (ListaIntegraConsultar.Count >= 5)
                                        {
                                            GerandoArquivoLog("Chave de acesso não retornada... Nota Rejeitada");
                                        }
                                        else
                                        {
                                            GerandoArquivoLog("Chave de acesso não retornada...");
                                            IntegraDocumentoEletronico ReenvioDoc = new IntegraDocumentoEletronico();
                                            ReenvioDoc.CodigoDocumento = NFSe.CodigoNotaFiscalServico;
                                            ReenvioDoc.CodigoAcao = 45;
                                            ReenvioDoc.RegistroEnviado = 1;
                                            ReenvioDoc.IntegracaoRecebido = 0;
                                            ReenvioDoc.IntegracaoProcessando = 0;
                                            ReenvioDoc.IntegracaoRetorno = 0;
                                            ReenvioDoc.RegistroDevolvido = 0;
                                            ReenvioDoc.RegistroMensagem = 0;
                                            ReenvioDoc.Mensagem = "";
                                            ReenvioDoc.CodigoMaquina = integracao.CodigoMaquina;
                                            ReenvioDoc.CodigoUsuario = integracao.CodigoUsuario;
                                            integraDAL.Inserir(ReenvioDoc);
                                        }
                                    }
                                }
                                else
                                {
                                    StrChaveAcesso = NFSe.ChaveAcesso;
                                }
                                GerandoArquivoLog("Chave de acesso " + StrChaveAcesso);
                                string consulta = ProxyNFSe.ConsultarNota(StrChaveAcesso, "");
                                integracao.IntegracaoProcessando = 1;
                                integracao.IntegracaoRetorno = 1;
                                //GerarArquivoXMLs(consulta, "ConsultaNota");
                                //CONSULTANDO NOTA...
                                NFSe.ChaveAcesso = StrChaveAcesso;

                                byte[] Arquivo = Encoding.UTF8.GetBytes(consulta);

                                SalvarAnexos(NFSe, Arquivo, data, integracao, "Consulta NFS-e");
                                GerandoArquivoLog("Anexo salvo com sucesso...");


                                NFSeConverterX.spdNFSeConverterX spdNFSeConverterX = new NFSeConverterX.spdNFSeConverterX();
                                NFSeConverterX.spdRetConsultaNFSe retorno = new NFSeConverterX.spdRetConsultaNFSe();
                                retorno = _spdNFSeConverterX.ConverterRetConsultarNFSeTipo(consulta);
                                string Situacao = retorno.Situacao;



                                integracao.RegistroDevolvido = 1;
                                GerandoArquivoLog("Convertendo XML retornado...");
                                int situacaoAnterior = NFSe.CodigoSituacao;
                                //-------------------------------------------------------------------------------------------
                                if (Situacao == "AUTORIZADA")
                                {
                                    integracao.RegistroMensagem = 1;
                                    NFSe.CodigoSituacao = 40;
                                    //NOTA AUTORIZADA
                                    GerandoArquivoLog("Nota Autorizada...");
                                }
                                else if (Situacao == "CANCELADA")
                                {
                                    integracao.RegistroMensagem = 1;
                                    NFSe.CodigoSituacao = 41;
                                    //NOTA CANCELADA
                                    GerandoArquivoLog("Nota cancelada...");
                                }
                                else
                                {
                                    Situacao = "REJEITADA";
                                    NFSe.CodigoSituacao = 39;
                                    //NOTA REJEITADA
                                    GerandoArquivoLog("Nota Rejeitada..." + retorno.Motivo);
                                }
                                if (NFSe.CodigoSituacao != situacaoAnterior)
                                {
                                    EventoDocumento(NFSe, data, integracao);
                                }
                                integracao.Mensagem = Situacao + " - " + retorno.Motivo;
                                integraDAL.AtualizarIntegraDocEletronico(integracao);
                                integraDAL.AtualizarDocumentoNFSe(NFSe);

                                EventoDocumento(NFSe, data, integracao);
                                if (Situacao == "AUTORIZADA" || Situacao == "CANCELADA")
                                {
                                    List<IntegraDocumentoEletronico> ListaRejeitadas = new List<IntegraDocumentoEletronico>();
                                    ListaRejeitadas = integraDAL.ListarIntegraDocEletronico("CD_DOCUMENTO", "NUMERIC", NFSe.CodigoNotaFiscalServico.ToString(), "", 43);
                                    foreach (IntegraDocumentoEletronico i in ListaRejeitadas)
                                    {
                                        integraDAL.Excluir(Convert.ToInt64(i.CodigoDocumento));
                                    }
                                    integraDAL.Excluir(Convert.ToInt64(integracao.Codigo));
                                    GerandoArquivoLog("Registro Apagado do integrador de documentos eletrônicos!");
                                }
                            }
                        }

                    }
                    if (Contador == 0)
                    {
                        GerandoArquivoLog("Nenhuma NFSe para ser Consultada");
                    }
                }
                catch (Exception ex)
                {
                    GerandoArquivoLog("Ocorreu erro durante o processo de Consulta - " + ex.ToString());
                    throw new Exception(ex.Message.ToString());
                }
            

        }

        protected void tmrFillAlerts_Tick(object sender, EventArgs e)
        {
            btnConsultar_Click(sender, e);

        }
        protected string GerarRPS(IntegraDocumentoEletronico integracao)
        {
            Doc_NotaFiscalServico doc = new Doc_NotaFiscalServico();
            Doc_NotaFiscalServicoDAL docDAL = new Doc_NotaFiscalServicoDAL();
            doc = docDAL.PesquisarNotaFiscalServico(Convert.ToInt32(integracao.CodigoDocumento));
            //-------------------------------------Instância DALs-------------------------------------
            PessoaDAL pDAL = new PessoaDAL();
            PessoaContatoDAL cttDAL = new PessoaContatoDAL();
            PessoaEnderecoDAL endDAL = new PessoaEnderecoDAL();
            PessoaInscricaoDAL insDAL = new PessoaInscricaoDAL();
            EmpresaDAL empresaDAL = new EmpresaDAL();
            //-------------------------------------Pessoa Tomador-------------------------------------

            Pessoa pTomador = new Pessoa();
            Pessoa_Inscricao pInsTomador = new Pessoa_Inscricao();
            Pessoa_Contato pCttTomador = new Pessoa_Contato();
            Pessoa_Endereco pEndTomador = new Pessoa_Endereco();

            pTomador = pDAL.PesquisarPessoa(doc.CodigoTomador);
            pInsTomador = insDAL.PesquisarPessoaInscricao(pTomador.CodigoPessoa, 1);
            pCttTomador = cttDAL.PesquisarPessoaContato(pTomador.CodigoPessoa, 1);
            pEndTomador = endDAL.PesquisarPessoaEndereco(pTomador.CodigoPessoa, 1);
            
            //----------------------------------Pessoa Prestador---------------------------------------
            Empresa empresaPrestador = new Empresa();
            empresaPrestador = empresaDAL.PesquisarEmpresa(doc.CodigoPrestador);
            Pessoa pPrestador = new Pessoa();
            Pessoa_Inscricao pInsPrestador = new Pessoa_Inscricao();
            Pessoa_Contato pCttPrestador = new Pessoa_Contato();
            Pessoa_Endereco pEndPrestador = new Pessoa_Endereco();
            if (empresaPrestador != null)
            {
                pPrestador = pDAL.PesquisarPessoa(empresaPrestador.CodigoPessoa);
                pInsPrestador = insDAL.PesquisarPessoaInscricao(pPrestador.CodigoPessoa, 1);
                pCttPrestador = cttDAL.PesquisarPessoaContato(pPrestador.CodigoPessoa, 1);
                pEndPrestador = endDAL.PesquisarPessoaEndereco(pPrestador.CodigoPessoa, 1);
            }
            else
            {
                pPrestador = pDAL.PesquisarPessoa(doc.CodigoPrestador);
                pInsPrestador = insDAL.PesquisarPessoaInscricao(pPrestador.CodigoPessoa, 1);
                pCttPrestador = cttDAL.PesquisarPessoaContato(pPrestador.CodigoPessoa, 1);
                pEndPrestador = endDAL.PesquisarPessoaEndereco(pPrestador.CodigoPessoa, 1);
            }
            int numItemFatura = 0; //<nItem></nItem><serv>

            Municipio mun = new Municipio();
            MunicipioDAL munDAL = new MunicipioDAL();
            mun = munDAL.PesquisarMunicipio(Convert.ToInt64(doc.CodigoMunicipioPrestado));

            Estado est = new Estado();
            EstadoDAL estDAL = new EstadoDAL();
            est = estDAL.PesquisarEstado(mun.CodigoEstado);
            //-----------------------------------------------SERVICOS----------------------------------------------------
            List<TipoServico> ListaTipoServico = new List<TipoServico>();
            List<ItemTipoServico> ListaItemTipoServico = new List<ItemTipoServico>();
            ListaTipoServico = docDAL.ObterTipoServico(doc.CodigoNotaFiscalServico);
            ListaItemTipoServico = docDAL.ObterProdutoDocumento(doc.CodigoNotaFiscalServico);
            string nfse = "" +
            "<envioLote versao=\"1.0\">" +
                "<CNPJ>" + pInsPrestador._NumeroInscricao + "</CNPJ>" +
                "<dhTrans>" + doc.DataEmissao.ToString("yyyy-MM-dd HH:mm:ss") + "</dhTrans>" +
                "<NFS-e>" +
                    "<infNFSe versao=\"1.1\">" +
                        "<Id>" +
                            "<cNFS-e>" + doc.NumeroDocumento.ToString() + "</cNFS-e>" +
                            "<mod>98</mod>" +
                            "<serie>" + doc.DGSerieDocumento.ToUpper() + "</serie>" +
                            "<nNFS-e>" + doc.NumeroDocumento.ToString() + "</nNFS-e>" +
                            "<dEmi>" + doc.DataEmissao.ToString("yyyy-MM-dd") + "</dEmi>" +
                            "<hEmi>" + doc.DataEmissao.ToString("HH:mm") + "</hEmi>" +
                            "<tpNF>1</tpNF> " +
                            "<refNF>000000000000000000000000000000000000000</refNF>" +
                            "<tpImp>1</tpImp>" +
                            "<tpEmis>N</tpEmis>" +
                            "<ambienteEmi>2</ambienteEmi>" +
                            "<formaEmi>2</formaEmi>" +
                            "<empreitadaGlobal>2</empreitadaGlobal>" +
                        "</Id>" +
                        "<prest>" +
                            "<CNPJ>" + pInsPrestador._NumeroInscricao + "</CNPJ>" +
                            "<xNome>" + pPrestador.NomePessoa + "</xNome>" +
                            "<xFant>" + pPrestador.NomeFantasia + "</xFant>" +
                            "<IM>" + pInsPrestador._NumeroIM + "</IM>" +
                            "<xEmail>" + pCttPrestador._MailNFSE + "</xEmail>" +
                            "<end>" +
                                "<xLgr>" + pEndPrestador._Logradouro.ToUpper() + "</xLgr>" +
                                "<nro>" + pEndPrestador._NumeroLogradouro + "</nro>" +
                                "<xCpl>" + pEndPrestador._Complemento + "</xCpl>" +
                                "<xBairro>" + pEndPrestador._DescricaoBairro + "</xBairro>" +
                                "<cMun>" + pEndPrestador._CodigoMunicipio.ToString() + "</cMun>" +
                                "<xMun>" + pEndPrestador._DescricaoMunicipio + "</xMun>" +
                                "<UF>" + pEndPrestador._DescricaoEstado.Substring(0, 2) + "</UF>" +
                                "<CEP>" + pEndPrestador._CodigoCEP.ToString() + "</CEP>" +
                                "<cPais>1058</cPais>" +
                                "<xPais>Brasil</xPais>" +
                            "</end>" +
                            "<fone>" + pCttPrestador._Fone1 + "</fone>" +
                            "<IE>" + pInsPrestador._NumeroIERG + "</IE>" +
                            "<regimeTrib>1</regimeTrib>" +
                        "</prest>" +
                        "<TomS>";
            if (pInsTomador._NumeroInscricao.Length > 12)
            {
                nfse = nfse + "<CNPJ>" + pInsTomador._NumeroInscricao + "</CNPJ>";
            }
            else
            {
                nfse = nfse + "<CPF>" + pInsTomador._NumeroInscricao + "</CPF>";
            }
            nfse = nfse + "<xNome>" + pTomador.NomePessoa + "</xNome>" +
                          "<ender>" +
                              "<xLgr>" + pEndTomador._Logradouro + "</xLgr>" +
                              "<nro>" + pEndTomador._NumeroLogradouro + "</nro>" +
                              "<xBairro>" + pEndTomador._DescricaoBairro + "</xBairro>" +
                              "<cMun>" + pEndTomador._CodigoMunicipio.ToString() + "</cMun>" +
                              "<xMun>" + pEndTomador._DescricaoMunicipio + "</xMun>" +
                              "<UF>" + pEndTomador._DescricaoEstado.Substring(0, 2) + "</UF>" +
                              "<CEP>" + pEndTomador._CodigoCEP.ToString() + "</CEP>" +
                              "<cPais>1058</cPais>" +
                              "<xPais>Brasil</xPais>" +
                          "</ender>" +
                          "<xEmail>" + pCttTomador._MailNFSE + "</xEmail>" +
                          "<IE>" + pInsTomador._NumeroIERG + "</IE>" +
                          "<IM>" + pInsTomador._NumeroIM + "</IM>" +
                          "<fone>" + pCttTomador._Fone1 + "</fone>" +
                          "<fone2>" + pCttTomador._Fone2 + "</fone2>" +
                        "</TomS>";

            foreach (TipoServico tipo in ListaTipoServico)
            {
                foreach (ItemTipoServico produto in ListaItemTipoServico)
                {

                    if (produto.CodigoServico == tipo.CodigoServico)
                    {
                        numItemFatura++;
                        nfse = nfse + 
                            "<det>" +
                           "<nItem>" + numItemFatura + "</nItem>" +
                           "<serv>" +
                               "<cServ>" + tipo.CodigoCNAE + "</cServ>" +
                               "<cLCServ>1406</cLCServ>" +
                               "<xServ>" + tipo.DescricaoTipoServico + "</xServ>" +
                               "<localTributacao>4305108</localTributacao>" +
                               "<localVerifResServ>1</localVerifResServ>";
                        decimal valorServico = (produto.Quantidade * produto.PrecoItem);

                        nfse = nfse + "<uTrib>" + produto.CodigoItemTipoServico + "</uTrib>" +
                              "<qTrib>" + Math.Round(produto.Quantidade, 2).ToString().Replace(",", ".") + "</qTrib>" +
                              "<vUnit>" + Math.Round(produto.PrecoItem, 2).ToString().Replace(",", ".") + "</vUnit>" +
                              "<vServ>" + Math.Round(valorServico, 2).ToString().Replace(",", ".") + "</vServ>" +
                              "<vDesc>" + doc.ValorAliquotaISSQN.ToString("F").Replace(",", ".") + "</vDesc>" +
                              "<vBCISS>" + 0.00 + "</vBCISS>" +
                              "<pISS>" + 0.00 + "</pISS>" +
                              "<vISS>" + doc.ValorAliquotaISSQN.ToString("F").Replace(",", ".") + "</vISS>" +
                              "<vBCINSS>" + 0.00 + "</vBCINSS>" +
                              "<pRetINSS>" + 0.00 + "</pRetINSS>" +
                              "<vRetINSS>" + doc.ValorINSS.ToString("F").Replace(",", ".") + "</vRetINSS>" +
                              "<vRed>" + 0.00 + "</vRed>" +
                              "<vBCRetIR>" + 0.00 + "</vBCRetIR>" +
                              "<pRetIR>" + doc.ValorIRRF.ToString("F").Replace(",", ".") + "</pRetIR>" +
                              "<vRetIR>" + 0.00 + "</vRetIR>" +
                              "<vBCCOFINS>" + 0.00 + "</vBCCOFINS>" +
                              "<pRetCOFINS>" + doc.ValorCofins.ToString("F").Replace(",", ".") + "</pRetCOFINS>" +
                              "<vRetCOFINS>" + 0.00 + "</vRetCOFINS>" +
                              "<vBCCSLL>" + 0.00 + "</vBCCSLL>" +
                              "<pRetCSLL>" + doc.ValorCSLL.ToString("F").Replace(",", ".") + "</pRetCSLL>" +
                              "<vRetCSLL>" + 0.00 + "</vRetCSLL>" +
                              "<vBCPISPASEP>" + 0.00 + "</vBCPISPASEP>" +
                              "<pRetPISPASEP>" + 0.00 + "</pRetPISPASEP>" +
                              "<vRetPISPASEP>" + 0.00 + "</vRetPISPASEP>" +
                         "</serv>" +
                      "</det>";
                    }

                }
            }
            nfse = nfse + "<total>" +
                           "<vServ>" + doc.ValorTotalNota.ToString("F").Replace(",", ".") + "</vServ>" +
                           "<vtNF>" + doc.ValorTotalNota.ToString("F").Replace(",", ".") + "</vtNF>" +
                           "<vtLiq>" + doc.ValorTotalNota.ToString("F").Replace(",", ".") + "</vtLiq>" +
                           "<ISS>" +
                               "<vBCISS>" + doc.ValorAliquotaISSQN.ToString("F").Replace(",", ".") + "</vBCISS>" +
                               "<vISS>" + doc.ValorAliquotaISSQN.ToString("F").Replace(",", ".") + "</vISS>" +
                           "</ISS>" +
                       "</total>" +
                       "<infAdicLT>4305108</infAdicLT>" +
                       "<infAdic>Cidade do Serviço: "+mun.DescricaoMunicipio+" - "+est.Sigla+"</infAdic>" +
                   "</infNFSe>" +
               "</NFS-e>" +
           "</envioLote>\r\n";
            return nfse;
        }

    }
}