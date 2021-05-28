using System;
using System.Collections.Generic;
using DAL.Model;
using System.Text;
using NFSeX;
using NFSeConverterX;
using NFSeDataSetX;
using System.IO;
using System.Xml;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace DAL.Persistence
{
    public class NFSeFuncoes
    {
        public string PanelSelect { get; set; }
        clsValidacao v = new clsValidacao();
        List<IntegraDocumentoEletronico> ListaIntegracaoDocEletronico = new List<IntegraDocumentoEletronico>();
        List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
           
        public decimal CodigoNFSe = 0;

        public NFSeX.spdNFSeX _spdNFSeX = new NFSeX.spdNFSeX();
        public NFSeDataSetX.spdNFSeDataSetX _spdNFSeDataSetX = new NFSeDataSetX.spdNFSeDataSetX();
        public NFSeX.spdProxyNFSeX ProxyNFSe = new NFSeX.spdProxyNFSeX();
        public spdNFSeConverterX _spdNFSeConverterX = new spdNFSeConverterX();
        
        
        public void IntegracaoPedido()
        {
            List<IntegracaoPedido> ListaIntegracaoPedidos = new List<IntegracaoPedido>();
            IntegracaoPedidoDAL intePedidoDAL = new IntegracaoPedidoDAL();
            ListaIntegracaoPedidos = intePedidoDAL.ListarIntegracaoPedido("CD_SITUACAO", "INT", "80", "");
            foreach(IntegracaoPedido p in ListaIntegracaoPedidos)
            {
                try
                {
                    int CodPessoa = 0;
                    PessoaInscricaoDAL pessoaInscricaoDAL = new PessoaInscricaoDAL();
                    CodPessoa = pessoaInscricaoDAL.PesquisarPessoaInscricao(p.NumeroInscricao);
                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    DBTabelaDAL RnTab = new DBTabelaDAL();
                    List<Pessoa_Inscricao> listaInscricoes = new List<Pessoa_Inscricao>();
                    List<Pessoa_Endereco> listaEnderecos = new List<Pessoa_Endereco>();
                    List<Pessoa_Contato> listaContatos = new List<Pessoa_Contato>();
                    List<Habil_Log> listaLog = new List<Habil_Log>();
                    List<AnexoDocumento> listaAnexo = new List<AnexoDocumento>();
                    List<ParcelaDocumento> listaParcelas = new List<ParcelaDocumento>();

                    if (CodPessoa != 0)
                    {
                        PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
                        Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                        pesIns = pesInsDAL.PesquisarPessoaInscricao(CodPessoa, 1);
                        pesIns._NumeroIERG = p.NumeroIERG;
                        listaInscricoes.Add(pesIns);

                        PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
                        Pessoa_Contato pesCtt = new Pessoa_Contato();
                        pesCtt = pesCttDAL.PesquisarPessoaContato(CodPessoa, 1);
                        pesCtt._MailNFSE = p.Mail_NFSe;
                        listaContatos.Add(pesCtt);

                        Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                        PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
                        pesEnd = pesEndDAL.PesquisarPessoaEndereco(CodPessoa, 1);

                        CEP cep = new CEP();
                        CEPDAL cepDAL = new CEPDAL();
                        MunicipioDAL munDAL = new MunicipioDAL();
                        EstadoDAL estDAL = new EstadoDAL();
                        BairroDAL bairroDAL = new BairroDAL();
                        cep = cepDAL.PesquisarCEP(p.CodigoCEP);
                        if (cep == null)

                        {
                            CEP cep2 = new CEP();
                            Municipio mun2 = new Municipio();
                            Estado est2 = new Estado();

                            mun2 = munDAL.PesquisarMunicipio(Convert.ToInt64(p.CodigoMunicipio));

                            est2 = estDAL.PesquisarEstado(mun2.CodigoEstado);

                            Bairro bairro2 = new Bairro();
                            bairro2 = bairroDAL.PesquisarBairroDescricao(p.DescricaoBairro);
                            if (bairro2.CodigoBairro == 0)
                            {
                                bairro2.DescricaoBairro = p.DescricaoBairro;

                                bairroDAL.Inserir(bairro2);
                                bairro2 = bairroDAL.PesquisarBairroDescricao(p.DescricaoBairro);
                            }
                            cep2.CodigoCEP = p.CodigoCEP;
                            cep2.CodigoMunicipio = Convert.ToInt64(p.CodigoMunicipio);
                            cep2.Complemento = "";
                            cep2.Logradouro = p.Logradouro;
                            cep2.DescricaoBairro = p.DescricaoBairro;
                            cep2.CodigoBairro = bairro2.CodigoBairro;
                            cep2.CodigoEstado = mun2.CodigoEstado;
                            cep2.DescricaoEstado = est2.Sigla + " - " + est2.DescricaoEstado;
                            cep2.DescricaoMunicipio = mun2.DescricaoMunicipio;
                            cepDAL.Inserir(cep2);

                        }
                        pesEnd._CodigoCEP = p.CodigoCEP;
                        pesEnd._CodigoMunicipio = Convert.ToInt64(p.CodigoMunicipio);
                        pesEnd._CodigoItem = 1;
                        Municipio mun = new Municipio();

                        Estado est = new Estado();

                        mun = munDAL.PesquisarMunicipio(Convert.ToInt64(p.CodigoMunicipio));
                        if(mun.CodigoMunicipio == 0 || mun == null)
                        {
                            GerandoArquivoLog("Codigo de municipio inexistente.",1);
                        }
                        est = estDAL.PesquisarEstado(mun.CodigoEstado);

                        Bairro bairro = new Bairro();

                        bairro = bairroDAL.PesquisarBairroDescricao(p.DescricaoBairro);
                        if (bairro.CodigoBairro == 0)
                        {
                            bairro.DescricaoBairro = p.DescricaoBairro;

                            bairroDAL.Inserir(bairro);
                            bairro = bairroDAL.PesquisarBairroDescricao(p.DescricaoBairro);
                        }

                        pesEnd._DescricaoMunicipio = mun.DescricaoMunicipio;
                        pesEnd._CodigoEstado = mun.CodigoEstado;
                        pesEnd._CodigoBairro = bairro.CodigoBairro;
                        pesEnd._DescricaoBairro = p.DescricaoBairro;
                        pesEnd._Logradouro = p.Logradouro;
                        pesEnd._NumeroLogradouro = p.NumeroEndereco;
                        pesEnd._DescricaoBairro = p.DescricaoBairro;
                        pesEnd._TipoEndereco = 5;
                        pesEnd._CodigoInscricao = 1;
                        pesEnd._Complemento = "";


                        pesEnd._DescricaoEstado = est.Sigla + " - " + est.DescricaoEstado;
                        listaEnderecos.Add(pesEnd);

                        pessoa = pessoaDAL.PesquisarPessoa(CodPessoa);

                        pessoaDAL.Atualizar(pessoa, listaInscricoes, listaEnderecos, listaContatos);
                    }
                    else
                    {

                        Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                        pesIns._CodigoItem = 1;
                        pesIns._NumeroInscricao = p.NumeroInscricao;
                        pesIns._NumeroIERG = p.NumeroIERG;
                        pesIns._NumeroIM = "";
                        pesIns._OBS = "";
                        pesIns._DataDeAbertura = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"));
                        if (p.NumeroInscricao.Length > 12)
                            pesIns._TipoInscricao = 3;
                        else
                            pesIns._TipoInscricao = 4;
                        listaInscricoes.Add(pesIns);

                        Pessoa_Contato pesCtt = new Pessoa_Contato();
                        pesCtt._MailNFSE = p.Mail_NFSe;
                        pesCtt._NomeContato = "Principal";
                        pesCtt._TipoContato = 10;
                        pesCtt._Fone1 = "";
                        pesCtt._Fone2 = "";
                        pesCtt._Fone3 = "";
                        pesCtt._Mail1 = "";
                        pesCtt._Mail2 = "";
                        pesCtt._Mail3 = "";
                        pesCtt._MailNFE = "";
                        pesCtt._CodigoItem = 1;
                        pesCtt._EmailSenha = "";
                        pesCtt._CodigoPais = 1058;
                        listaContatos.Add(pesCtt);

                        Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                        pesEnd._CodigoCEP = p.CodigoCEP;
                        pesEnd._CodigoMunicipio = Convert.ToInt64(p.CodigoMunicipio);
                        Municipio mun = new Municipio();
                        MunicipioDAL munDAL = new MunicipioDAL();
                        mun = munDAL.PesquisarMunicipio(Convert.ToInt64(p.CodigoMunicipio));
                        pesEnd._DescricaoMunicipio = mun.DescricaoMunicipio;

                        Estado est = new Estado();
                        EstadoDAL estDAL = new EstadoDAL();
                        est = estDAL.PesquisarEstado(mun.CodigoEstado);
                        Bairro bairro = new Bairro();
                        BairroDAL bairroDAL = new BairroDAL();
                        bairro = bairroDAL.PesquisarBairroDescricao(p.DescricaoBairro);
                        if (bairro.CodigoBairro == 0)
                        {
                            bairro.DescricaoBairro = p.DescricaoBairro;

                            bairroDAL.Inserir(bairro);
                            bairro = bairroDAL.PesquisarBairroDescricao(p.DescricaoBairro);
                        }
                        CEP cep = new CEP();
                        CEPDAL cepDAL = new CEPDAL();
                        cep = cepDAL.PesquisarCEP(p.CodigoCEP);
                        if (cep == null)
                        {
                            CEP cep2 = new CEP();
                            Municipio mun2 = new Municipio();
                            Estado est2 = new Estado();

                            mun2 = munDAL.PesquisarMunicipio(Convert.ToInt64(p.CodigoMunicipio));
                            est2 = estDAL.PesquisarEstado(mun2.CodigoEstado);

                            Bairro bairro2 = new Bairro();
                            bairro2 = bairroDAL.PesquisarBairroDescricao(p.DescricaoBairro);
                            if (bairro.CodigoBairro == 0)
                            {
                                bairro2.DescricaoBairro = p.DescricaoBairro;

                                bairroDAL.Inserir(bairro2);
                                bairro2 = bairroDAL.PesquisarBairroDescricao(p.DescricaoBairro);
                            }
                            cep2.CodigoCEP = p.CodigoCEP;
                            cep2.CodigoMunicipio = Convert.ToInt64(p.CodigoMunicipio);
                            cep2.Logradouro = p.Logradouro;
                            cep2.Complemento = "";
                            cep2.DescricaoBairro = p.DescricaoBairro;
                            cep2.CodigoBairro = bairro2.CodigoBairro;
                            cep2.CodigoEstado = mun2.CodigoEstado;
                            cep2.DescricaoEstado = est2.Sigla + " - " + est2.DescricaoEstado;
                            cep2.DescricaoMunicipio = mun2.DescricaoMunicipio;
                            cepDAL.Inserir(cep2);

                        }


                        pesEnd._CodigoEstado = mun.CodigoEstado;
                        pesEnd._DescricaoEstado = est.Sigla + " - " + est.DescricaoEstado;
                        pesEnd._CodigoBairro = bairro.CodigoBairro;
                        pesEnd._CodigoEstado = mun.CodigoEstado;
                        pesEnd._DescricaoBairro = "";
                        pesEnd._Logradouro = p.Logradouro;
                        pesEnd._NumeroLogradouro = p.NumeroEndereco;
                        pesEnd._DescricaoBairro = p.DescricaoBairro;


                        pesEnd._TipoEndereco = 5;
                        pesEnd._Complemento = "";
                        pesEnd._CodigoInscricao = 1;
                        pesEnd._CodigoItem = 1;

                        listaEnderecos.Add(pesEnd);


                        pessoa.NomeFantasia = p.NomeTomador;
                        pessoa.NomePessoa = p.NomeTomador;
                        pessoa.PessoaCliente = 1;
                        pessoa.CodigoSituacaoPessoa = 1;
                        pessoa.CodigoSituacaoFase = 15;
                        pessoa.CodigoGrupoPessoa = 1;

                        Int64 CodigoPessoa = 0;
                        pessoaDAL.Inserir(pessoa, listaInscricoes, listaEnderecos, listaContatos, ref CodigoPessoa);


                    }
                    CodPessoa = pessoaInscricaoDAL.PesquisarPessoaInscricao(p.NumeroInscricao);

                    List<TipoServico> listaTipoServico = new List<TipoServico>();
                    TipoServico tipoServico = new TipoServico();
                    tipoServico.CodigoSituacao = 1;
                    tipoServico.DescricaoTipoServico = p.DescricaoServico;
                    tipoServico.CodigoCNAE = p.CodigoCNAE;
                    tipoServico.CodigoServicoLei = p.CodigoServicoLei;
                    tipoServico.CodigoServico = 1;


                    ItemTipoServico itemServico = new ItemTipoServico();
                    List<ItemTipoServico> listaItemServico = new List<ItemTipoServico>();
                    itemServico.CodigoServico = 1;
                    itemServico.CodigoProdutoDocumento = 1;
                    itemServico.Quantidade = p.Quantidade;
                    itemServico.PrecoItem = p.PrecoItem;
                    itemServico.CodigoProduto = 1;
                    listaTipoServico.Add(tipoServico);
                    listaItemServico.Add(itemServico);


                    Doc_NotaFiscalServico nfse = new Doc_NotaFiscalServico();
                    Doc_NotaFiscalServicoDAL nfseDAL = new Doc_NotaFiscalServicoDAL();
                    nfse.CodigoPrestador = p.CodigoEmpresa;
                    nfse.CodigoTomador = CodPessoa;
                    nfse.CodigoTipoOperacao = 1;
                    nfse.DataEmissao = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"));
                    nfse.DataLancamento = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"));
                    nfse.DescricaoGeralServico = p.DescricaoNFSE;
                    nfse.CodigoSituacao = 42;
                    nfse.CodigoMunicipioPrestado = Convert.ToInt32(p.CodigoMunicipio);
                    nfse.ValorTotalNota = p.ValorTotalNFSe;

                    GeradorSequencialDocumentoEmpresa gerador = new GeradorSequencialDocumentoEmpresa();
                    GeradorSequencialDocumentoEmpresaDAL geradorDAL = new GeradorSequencialDocumentoEmpresaDAL();
                    gerador = geradorDAL.PesquisarGeradorSequencialEmpresa(Convert.ToInt32(nfse.CodigoPrestador), 1);
                    nfse.CodigoGeracaoSequencialDocumento = gerador.CodigoGeradorSequencialDocumento;
                    DBTabelaDAL db = new DBTabelaDAL();
                    GeracaoSequencialDocumento gerDoc = new GeracaoSequencialDocumento();
                    GeracaoSequencialDocumentoDAL gerDocDAL = new GeracaoSequencialDocumentoDAL();
                    gerDoc = gerDocDAL.PesquisarGeradorSequencial(gerador.CodigoGeradorSequencialDocumento);


                    // Se existe a tabela sequencial
                    if (db.BuscaTabelas(gerador.Cpl_Nome) == gerador.Cpl_Nome)
                    {
                        double NroSequencial = geradorDAL.ExibeProximoNroSequencial(gerador.Cpl_Nome);
                        if (NroSequencial == 0)
                            nfse.NumeroDocumento = Convert.ToInt32(gerDoc.NumeroInicial.ToString());
                        else
                            nfse.NumeroDocumento = Convert.ToInt32(NroSequencial.ToString());

                        nfse.DGSerieDocumento = gerDoc.SerieConteudo.ToString();
                        nfse.Cpl_NomeTabela = gerador.Cpl_Nome;
                    }
                    else
                    {

                        nfse.NumeroDocumento = Convert.ToInt32(gerDoc.NumeroInicial.ToString());
                        nfse.DGSerieDocumento = gerDoc.SerieConteudo;
                    }
                    nfse.Cpl_Maquina = 1;
                    nfse.Cpl_Usuario = 1;

                    decimal CodigoDocumento = 0;

                    Doc_NotaFiscalServicoDAL doc = new Doc_NotaFiscalServicoDAL();
                    List<Doc_NotaFiscalServico> ListaDocs = new List<Doc_NotaFiscalServico>();
                    ListaDocs = doc.ListarNotaFiscalServico("CD_SITUACAO", "SMALLINT", "39", "CD_DOCUMENTO DESC");
                    int i = 0;
                    foreach (Doc_NotaFiscalServico d in ListaDocs)
                    {

                        if (i == 0 && d.CodigoSituacao != 37)
                        {
                            i++;
                            doc.Excluir(d.CodigoNotaFiscalServico);

                            GeradorSequencialDocumentoEmpresa gerador2 = new GeradorSequencialDocumentoEmpresa();
                            GeradorSequencialDocumentoEmpresaDAL gerador2DAL = new GeradorSequencialDocumentoEmpresaDAL();
                            gerador2 = gerador2DAL.PesquisarGeradorSequencialEmpresa(d.CodigoPrestador, 1);

                            GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                            gerDAL.AtualizarTabelaGeradora(d.NumeroDocumento, gerador2.Cpl_Nome);
                        }

                    }




                    nfseDAL.Inserir(nfse, listaTipoServico, listaItemServico, null, listaAnexo, listaParcelas, ref CodigoDocumento);
                    intePedidoDAL.AtualizarDocumento(CodigoDocumento, p.Codigo);

                    intePedidoDAL.AtualizarRetornoPedido(84, "", p.Codigo, 0);

                    IntegraDocumentoEletronico InDocEle = new IntegraDocumentoEletronico();
                    IntegraDocumentoEletronicoDAL InDocEleDAL = new IntegraDocumentoEletronicoDAL();

                    InDocEle.CodigoDocumento = CodigoDocumento;
                    InDocEle.RegistroEnviado = 1;
                    InDocEle.IntegracaoRecebido = 0;
                    InDocEle.IntegracaoProcessando = 0;
                    InDocEle.IntegracaoRetorno = 0;
                    InDocEle.RegistroDevolvido = 0;
                    InDocEle.RegistroMensagem = 0;
                    InDocEle.Mensagem = "";
                    InDocEle.CodigoAcao = 43;
                    InDocEle.CodigoMaquina = 1;
                    InDocEle.CodigoUsuario = 1;

                    InDocEleDAL.Inserir(InDocEle);

                    nfse.ChaveAcesso = "";
                    nfse.Protocolo = "";
                    nfseDAL.AtualizarChaveAcesso(nfse);
                }
                catch (Exception e)
                {
                    GerandoArquivoLog(e.ToString(),1);
                    intePedidoDAL.AtualizarRetornoPedido(82, "Ocorreu algum erro no processo. Verifique Arquivo de Log", p.Codigo, 0);
                }
            }

            List<IntegracaoPedido> ListaIntegracaoPedidos2 = new List<IntegracaoPedido>();
            ListaIntegracaoPedidos2 = intePedidoDAL.ListarIntegracaoPedido("CD_SITUACAO", "INT", "81", "");
            foreach(IntegracaoPedido p2 in ListaIntegracaoPedidos2)
            {
                try { 
                    if(p2.CodigoDocumento != 0)
                    {
                        Doc_NotaFiscalServico nfse = new Doc_NotaFiscalServico();
                        Doc_NotaFiscalServicoDAL nfseDAL = new Doc_NotaFiscalServicoDAL();
                        nfse = nfseDAL.PesquisarNotaFiscalServico(p2.CodigoDocumento);

                        if (nfse.CodigoSituacao == 40)
                        {
                            IntegraDocumentoEletronico InDocEle = new IntegraDocumentoEletronico();
                            IntegraDocumentoEletronicoDAL InDocEleDAL = new IntegraDocumentoEletronicoDAL();

                            InDocEle.CodigoDocumento = p2.CodigoDocumento;
                            InDocEle.RegistroEnviado = 1;
                            InDocEle.IntegracaoRecebido = 0;
                            InDocEle.IntegracaoProcessando = 0;
                            InDocEle.IntegracaoRetorno = 0;
                            InDocEle.RegistroDevolvido = 0;
                            InDocEle.RegistroMensagem = 0;
                            InDocEle.Mensagem = "";
                            InDocEle.CodigoAcao = 44;
                            InDocEle.CodigoMaquina = 1;
                            InDocEle.CodigoUsuario = 1;

                            InDocEleDAL.Inserir(InDocEle);
                            intePedidoDAL.AtualizarRetornoPedido(84, "", nfse.CodigoIndexIntegraPedido, p2.CodigoDocumento);
                        }
                    }
                }
                    catch (Exception e)
                {
                    GerandoArquivoLog(e.ToString(),1);
                    intePedidoDAL.AtualizarRetornoPedido(82, "Ocorreu algum erro no processo. Verifique Arquivo de Log", p2.Codigo, p2.CodigoDocumento);
                }
            }
        }

        public void ExecutarFuncoes()
        {
            IntegracaoPedido();
            btnEnviar_Click();
            btnCancelar_Click();
            btnConsultarNota_Click();
        }
        public void btnEnviar_Click()
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime data = RnTab.ObterDataHoraServidor();
            
            IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
            List<ParcelaDocumento> ListaParcelaDocumento = new List<ParcelaDocumento>();

            ListaIntegracaoDocEletronico = integraDAL.ListarIntegracaoDocEletronicoCompleto(listaT);
            Thread.Sleep(1000);
            string chaveAcesso = "";
            try
            {
                int Contador = 0;
                foreach (IntegraDocumentoEletronico integracao in ListaIntegracaoDocEletronico)
                {                    
                    if (integracao.CodigoAcao == 43 && integracao.IntegracaoRecebido == 0)
                    {
                        Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                        Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
                        NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(integracao.CodigoDocumento));

                        if (NFSe.ChaveAcesso == "" && NFSe.Protocolo == "" && NFSe.CodigoSituacao != 40 && NFSe.CodigoSituacao != 41)
                        {
                            string certificados = _spdNFSeX.ListarCertificados();
                            GerandoArquivoLog(certificados,1);
                            if (certificados != null)
                            {
                                string[] ListaCertificado = certificados.Split('|');
                                Empresa empresa = new Empresa();
                                EmpresaDAL empresaDAL = new EmpresaDAL();
                                empresa = empresaDAL.PesquisarEmpresa(NFSe.CodigoPrestador);

                                Pessoa pessoa = new Pessoa();
                                PessoaDAL pessoaDAL = new PessoaDAL();
                                pessoa = pessoaDAL.PesquisarPessoa(empresa.CodigoPessoa);

                                Pessoa_Inscricao Inscricao = new Pessoa_Inscricao();
                                PessoaInscricaoDAL inscricaoDAL = new PessoaInscricaoDAL();
                                Inscricao = inscricaoDAL.PesquisarPessoaInscricao(pessoa.CodigoPessoa, 1);

                                Pessoa_Endereco endereco = new Pessoa_Endereco();
                                PessoaEnderecoDAL enderecoDAL = new PessoaEnderecoDAL();
                                endereco = enderecoDAL.PesquisarPessoaEndereco(pessoa.CodigoPessoa, 1);

                                int contador = 0;
                                foreach (string certificado in ListaCertificado)
                                {
                                    string[] ArrayCertificado = certificado.Split(':');
                                    if (ArrayCertificado.Length >= 2)
                                    {
                                        string[] ArrayCertificado2 = ArrayCertificado[1].Split(',');

                                        foreach (string cert in ArrayCertificado2)
                                        {
                                            string Cnpj = cert;
                                            if (Cnpj == Inscricao._NumeroInscricao)
                                            {
                                                contador++;
                                                //----------------------------------------------------Configuracao do .INI----------------------------------------------------------                                               
                                                string DiretorioEXE = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\\..\\..\\..\\..\\Modulos";
                                                _spdNFSeX.LoadConfig(DiretorioEXE + "\\TecnoSpeed\\NFSe\\Arquivos\\nfseConfig.ini");
                                                _spdNFSeX.CNPJ = Inscricao._NumeroInscricao;
                                                _spdNFSeX.InscricaoMunicipal = Inscricao._NumeroIM;
                                                _spdNFSeX.NomeCertificado = certificado;
                                                _spdNFSeX.ArquivoLocais = DiretorioEXE + @"\\TecnoSpeed\\NFSe\\Arquivos\\nfseLocais.ini";
                                                _spdNFSeX.ArquivoServidoresHom = DiretorioEXE + @"\\TecnoSpeed\\NFSe\\Arquivos\\nfseServidoresHom.ini";
                                                _spdNFSeX.ArquivoServidoresProd = DiretorioEXE + @"\\TecnoSpeed\\NFSe\\Arquivos\\nfseServidoresProd.ini";
                                                _spdNFSeX.DiretorioLogErro = DiretorioEXE + @"\\TecnoSpeed\\NFSe\\Arquivos\\NFSeDiretorioLogErro";

                                                ProxyNFSe.ComponenteNFSe = _spdNFSeX;
                                                _spdNFSeConverterX.DiretorioEsquemas = _spdNFSeX.DiretorioEsquemas;
                                                _spdNFSeConverterX.DiretorioScripts = DiretorioEXE + @"\\TecnoSpeed\\NFSe\\Arquivos\\Scripts";
                                                _spdNFSeConverterX.Cidade = _spdNFSeX.Cidade;
                                                //-----------------------------------------------------------------------------------------------------------------------------------
                                            }
                                        }
                                    }
                                }
                                if (contador == 0)
                                {
                                    GerandoArquivoLog("Empresa Prestador não tem Certificado Digital instalada",1);
                                    NFSe.CodigoSituacao = 39;
                                    integraDAL.AtualizarDocumentoNFSe(NFSe);

                                    return;
                                }
                            }
                            else
                            {
                                GerandoArquivoLog("Nenhum certificado instalado na máquina",1);
                                NFSe.CodigoSituacao = 39;
                                integraDAL.AtualizarDocumentoNFSe(NFSe);
                                return ;
                            }
                            
                            Contador++;
                            GerandoArquivoLog("Solicitação de autorização da NFS-e " + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico,1);
                            //SOLICITACAO DE AUTORIZADA DE NFS-e X
                            //integracao recebeu a nota para autorizacao
                            integracao.IntegracaoRecebido = 1;
                            Thread.Sleep(1000);
                            GerandoArquivoLog("gerando XML da NFS-e" + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico,1);

                            string RPS = "";
                            if (_spdNFSeX.CNPJ == "92575851000180")
                                RPS = GerarRPS(integracao);
                            else
                            {
                                string caminhoTX2 = gerarXMLPorTX2(integracao);
                                RPS = _spdNFSeConverterX.ConverterEnvioNFSe(caminhoTX2, "");
                                if (System.IO.File.Exists(caminhoTX2))
                                    System.IO.File.Delete(caminhoTX2);
                            }

                           
                           
                            CodigoNFSe = NFSe.CodigoNotaFiscalServico;
                            GerandoArquivoLog(RPS,1);
                            Thread.Sleep(1000);
                           // string teste = _spdNFSeX.NomeCertificado;
                            GerandoArquivoLog("Assinando XML da NFS-e " + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico,1);
                            RPS = ProxyNFSe.Assinar(RPS, "");//Assinar XML gerado
                            GerandoArquivoLog(RPS,1);
                            Thread.Sleep(1000);
                            byte[] Arquivo2 = Encoding.UTF8.GetBytes(RPS);
                            SalvarAnexos(NFSe, Arquivo2, data, integracao, "Envio NFS-e");

                            GerandoArquivoLog("Enviando XML da NFS-e " + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico,1);
                            string protocolo = ProxyNFSe.Enviar(RPS, "");// Envio XML assinado 
                            NFSe.Protocolo = protocolo;
                            integraDAL.AtualizarDocumentoNFSe(NFSe);

                            Thread.Sleep(1000);
                            
                            if (protocolo.Length < 35)
                            {
                                integracao.IntegracaoProcessando = 1;
                                //txt - nota enviado com sucesso... aguardando resposta
                                GerandoArquivoLog("NFS-e Enviada...",1);
                            }
                            else
                            {
                                NFSe.CodigoSituacao = 39;
                                integracao.Mensagem = "Erro ao Enviar NFSe, protocolo não retornado!";
                                integraDAL.AtualizarIntegraDocEletronico(integracao);
                                integraDAL.AtualizarDocumentoNFSe(NFSe);
                                GerandoArquivoLog("Erro ao enviar NFS-e",1);
                            }
                            

                            GerandoArquivoLog("Consultando o envio de lote - protocolo n° " + protocolo,1);
                            string consultaLote = ProxyNFSe.ConsultarLote(protocolo, "");// Consultar lote
                                                                                         //txt - Fazendo consulta do envio de lote
                            Thread.Sleep(1000);
                            GerandoArquivoLog("Convertendo retorno da consulta de lote",1);

                            //------------------------------------Converter XML-----------------------------------------
                            NFSeConverterX.spdRetConsultaLoteNFSe retorno2 = new NFSeConverterX.spdRetConsultaLoteNFSe();
                            retorno2 = _spdNFSeConverterX.ConverterRetConsultarLoteNFSeTipo(consultaLote);
                            String motivo = retorno2.Motivo;//Mostra situacao da nota
                            //------------------------------------------------------------------------------------------
                            //CONVERSAO DO RETORNO DE CONSULTA DE LOTE

                            Thread.Sleep(1000);

                            integracao.IntegracaoRetorno = 1;

                            string retornoChaveCancelamento = _spdNFSeConverterX.ConverterRetConsultarLoteNFSe(consultaLote, "ChaveCancelamento");//retorna chave de cancelamento

                            String[] StrChaveAcesso = retornoChaveCancelamento.Split(';');
                            chaveAcesso = StrChaveAcesso[0];// chave de acesso                   

                            GerandoArquivoLog("Obtendo Chave de acesso " + chaveAcesso,1);
                            if (chaveAcesso != "EMPROCESSAMENTO")
                            {
                                //em processamento
                                integracao.RegistroDevolvido = 1;

                                //FAZER CONSULTA
                                //CHAVE DE ACESSO GERADA IMEDIATAMENTO
                                GerandoArquivoLog("Chave de acesso retornada com sucesso...",1);
                            }
                            else
                            {
                                //CHAVE DE ACESSO NÃO RETORNADA
                                GerandoArquivoLog("Chave de acesso não retornada...",1);
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

                            GerandoArquivoLog("Consultando da NFS-e" + NFSe.NumeroDocumento + " / " + NFSe.CodigoNotaFiscalServico,1);
                            string consulta = ProxyNFSe.ConsultarNota(chaveAcesso, "");//consultar nota
                            byte[] Arquivo = Encoding.UTF8.GetBytes(consulta);

                            SalvarAnexos(NFSe, Arquivo, data, integracao,"Envio Autorização NFS-e");
                            GerandoArquivoLog("Anexo salvo com sucesso...",1);
                            //------------------------------------Converter XML-----------------------------------------
                            NFSeConverterX.spdRetConsultaNFSe retorno = new NFSeConverterX.spdRetConsultaNFSe();
                            retorno = _spdNFSeConverterX.ConverterRetConsultarNFSeTipo(consulta);
                            String Situacao = retorno.Situacao;//Mostra situacao da nota

                            //------------------------------------------------------------------------------------------

                            NFSe.ChaveAcesso = chaveAcesso;
                            NFSe.Protocolo = protocolo;
                            integraDAL.AtualizarDocumentoNFSe(NFSe);

                            IntegracaoPedidoDAL intePedidoDAL = new IntegracaoPedidoDAL();

                            if (Situacao == "AUTORIZADA")
                            {
                                integracao.RegistroMensagem = 1;
                                NFSe.CodigoSituacao = 40;
                                //NOTA AUTORIZADA COM SUCESSO
                                GerandoArquivoLog("NFS-e autorizada com sucesso...",1);

                                TipoOperacao TipoOP = new TipoOperacao();
                                TipoOperacaoDAL TipoOPDAL = new TipoOperacaoDAL();
                                TipoOP = TipoOPDAL.PesquisarTipoOperacao(NFSe.CodigoTipoOperacao);
                                if(TipoOP.AtualizaFinanceiro == true)
                                {
                                    ParcelaDocumentoDAL parcelaDAL = new ParcelaDocumentoDAL();
                                    ListaParcelaDocumento = parcelaDAL.ObterParcelaDocumento(NFSe.CodigoNotaFiscalServico);
                                    if (ListaParcelaDocumento.Count != 0)
                                    {
                                        Doc_CtaReceberDAL ContaReceberDAL = new Doc_CtaReceberDAL();
                                        ContaReceberDAL.InserirParcelas(ListaParcelaDocumento, NFSe, TipoOP.BaixaFinanceiro, integracao);
                                    }
                                    
                                   
                                }
                                intePedidoDAL.AtualizarRetornoPedido(83, "AUTORIZADO", NFSe.CodigoIndexIntegraPedido, NFSe.CodigoNotaFiscalServico);

                                if (NFSe.CodigoDocumentoOriginal != 0)
                                {
                                    List<Doc_OrdemServico> ListaOSs = new List<Doc_OrdemServico>();
                                    Doc_OrdemServico osFat = new Doc_OrdemServico();
                                    Doc_OrdemServicoDAL osDAL = new Doc_OrdemServicoDAL();
                                    
                                    osFat = osDAL.PesquisarDocumento(NFSe.CodigoDocumentoOriginal);
                                    osFat.Cpl_Maquina = integracao.CodigoMaquina;
                                    osFat.Cpl_Usuario = integracao.CodigoUsuario;
                                    
                                    osDAL.AtualizarSituacao(osFat, 104);
                                    ListaOSs = osDAL.ListarOrdemServicoServico(osFat.Cpl_CodigoPessoa, 97, NFSe.CodigoDocumentoOriginal);

                                    List<Doc_OrdemServico> ListaNovaOSs = new List<Doc_OrdemServico>();
                                    foreach(Doc_OrdemServico os in ListaOSs)
                                    {
                                        os.Cpl_Maquina = integracao.CodigoMaquina;
                                        os.Cpl_Usuario = integracao.CodigoUsuario;
                                        ListaNovaOSs.Add(os);
                                    }
                                    osDAL.AtualizarOrdemServicoServico(ListaNovaOSs, 109, NFSe.CodigoDocumentoOriginal);
                                }
                            }
                            else if (Situacao == "CANCELADO")
                            {
                                integracao.RegistroMensagem = 1;
                                NFSe.CodigoSituacao = 41;
                                //NOTA CANCELADA COM SUCESSO
                                GerandoArquivoLog("NFS-e cancelada ...",1);
                                intePedidoDAL.AtualizarRetornoPedido(83, "CANCELADO", NFSe.CodigoIndexIntegraPedido, NFSe.CodigoNotaFiscalServico);
                            }
                            else
                            {
                                NFSe.CodigoSituacao = 39;
                                //NOTA REJEITADA
                                //MOTIVO DA REJEIÇÃO
                                GerandoArquivoLog("NFS-e Rejeitada..." + motivo,1);
                                intePedidoDAL.AtualizarRetornoPedido(82, "REJEITADA: "+motivo, NFSe.CodigoIndexIntegraPedido, NFSe.CodigoNotaFiscalServico);
                            }

                            integracao.Mensagem = Situacao + motivo + retorno.Motivo;
                            integraDAL.AtualizarIntegraDocEletronico(integracao);
                            integraDAL.AtualizarDocumentoNFSe(NFSe);
                            EventoDocumento(NFSe, data, integracao);
                            CodigoNFSe = 0;
                            if (Situacao == "AUTORIZADA" || Situacao == "CANCELADA")
                            {
                                List<IntegraDocumentoEletronico> ListaRejeitadas = new List<IntegraDocumentoEletronico>();
                                ListaRejeitadas = integraDAL.ListarIntegraDocEletronico("CD_DOCUMENTO", "NUMERIC", NFSe.CodigoNotaFiscalServico.ToString(), "", 43);
                                foreach(IntegraDocumentoEletronico i in ListaRejeitadas)
                                {
                                    integraDAL.ExcluirPorDocumento(Convert.ToInt64(i.CodigoDocumento));
                                }
                                integraDAL.Excluir(Convert.ToInt64(integracao.Codigo));
                                GerandoArquivoLog("Registro Apagado do integrador de documentos eletrônicos!",1);
                            }
                        }
                    }                    
                }
                if (Contador == 0)
                {
                    GerandoArquivoLog("Nenhuma NFSe para ser Autorizada", 1);
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Ocorreu erro durante o processo de autorização - " + ex.ToString(),1);
                Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();

                IntegraDocumentoEletronico integra = new IntegraDocumentoEletronico();
                IntegracaoPedidoDAL intePedidoDAL = new IntegracaoPedidoDAL();
                integra = integraDAL.PesquisarIntegracaoDocEletronico(CodigoNFSe , 43);

                if (integra != null)
                {
                    NFSe = NFSeDAL.PesquisarNotaFiscalServico(integra.CodigoDocumento);
                    NFSe.CodigoSituacao = 39;

                    if (chaveAcesso == "")
                        NFSe.ChaveAcesso = "ERRO";
                    else
                        NFSe.ChaveAcesso = chaveAcesso;

                    string MSGErro = "";
                    if (ex.Message.Length >= 900)
                        MSGErro = ex.Message.Substring(0, 900);
                    else
                        MSGErro = ex.Message;

                    intePedidoDAL.AtualizarRetornoPedido(82, "Ocorreu erro durante o processo de autorização - " + MSGErro, NFSe.CodigoIndexIntegraPedido, NFSe.CodigoNotaFiscalServico);
                    NFSe.CodigoNotaFiscalServico = Convert.ToInt64(integra.CodigoDocumento);
                    NFSe.Protocolo = "";
                    integraDAL.AtualizarDocumentoNFSe(NFSe);
                    integra.Mensagem = "Ocorreu um erro no processo de envio NFS-e... Verifique os dados informados! - "  + MSGErro;
                    integraDAL.AtualizarIntegraDocEletronico(integra);
                    EventoDocumento(NFSe, data, integra);
                    GerandoArquivoLog("Nota rejeitada:" + ex.ToString(),1);
                }
                btnEnviar_Click();
                CodigoNFSe = 0;
            }
        }
        public void GerandoArquivoLog(string strDescrição, int CodCaminho)
        {
            try
            {
                DateTime data = DateTime.Now;

                string CaminhoArquivoLog = "";
                if (CodCaminho == 1)//HabilService
                    CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\..\\..\\..\\..\\Modulos\\Log\\NFSe\\";
                else//HabilInformatica
                    CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\NFSe\\";

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
        public void EventoDocumento(Doc_NotaFiscalServico NFSe, DateTime data,IntegraDocumentoEletronico integra)
        {
            List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
            EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
            ListaEvento = eventoDAL.ObterEventos(NFSe.CodigoNotaFiscalServico);
            EventoDocumento eventodoc = new EventoDocumento();
            eventodoc.CodigoDocumento = Convert.ToDecimal(NFSe.CodigoNotaFiscalServico);
            eventodoc.CodigoMaquina = integra.CodigoMaquina;
            eventodoc.CodigoUsuario = integra.CodigoUsuario;
            eventodoc.CodigoSituacao = NFSe.CodigoSituacao;
            eventodoc.DataHoraEvento = data;
            if(ListaEvento.Count > 0)
                eventodoc.CodigoEvento = ListaEvento.Max(x => x.CodigoEvento) + 1;
            else
                eventodoc.CodigoEvento = 1;
            eventoDAL.Inserir(eventodoc, NFSe.CodigoNotaFiscalServico);
        }

        public void SalvarAnexos(Doc_NotaFiscalServico NFSe, byte[] Arquivo, DateTime data,IntegraDocumentoEletronico integra, string StrDescricao)
        {
            AnexoDocumento anexo = new AnexoDocumento();
            AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();
            anexo.CodigoDocumento = Convert.ToDecimal(NFSe.CodigoNotaFiscalServico);
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

        public void btnCancelar_Click()
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime data = RnTab.ObterDataHoraServidor();

            IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
            ListaIntegracaoDocEletronico = integraDAL.ListarIntegracaoDocEletronicoCompleto(listaT);

            //--------------------------Configuracao do .INI------------------------------------------
            string DiretorioEXE = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\..\..\..\..\Modulos";
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
                    
                    if (integracao.CodigoAcao == 44 && integracao.IntegracaoRecebido == 0)
                    {
                        Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                        Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
                        NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(integracao.CodigoDocumento));

                        if (NFSe.ChaveAcesso != "" && NFSe.Protocolo != "" && NFSe.CodigoSituacao != 42)
                        {
                            Contador++;
                            //SOLICITACAO DE CANCELAMENTO DE NOTA X
                            integracao.IntegracaoRecebido = 1;
                            GerandoArquivoLog("Solicitação de cancelamento da NFS-e n° " + NFSe.NumeroDocumento,1);
                            string chaveAcesso = NFSe.ChaveAcesso;//NFSe.ChaveAcesso;

                            integracao.RegistroEnviado = 1;

                            GerandoArquivoLog("Cancelando NFSe... Chave de acesso " + chaveAcesso,1);
                            string retornoCancelamento = ProxyNFSe.CancelarNota(chaveAcesso, "MotivoCancelamento=2");//cancela nota
                            
                            Thread.Sleep(2000);

                            integracao.IntegracaoProcessando = 1;
                            //NOTA ENVIADA PARA CANCELAMENTO
                            GerandoArquivoLog("Gerando XML's da NFS-e "+chaveAcesso,1);

                            GerarArquivoXMLs(retornoCancelamento, "Cancelamento");//gerar Arquivo Xml do cancelamento

                            string consulta = ProxyNFSe.ConsultarNota(chaveAcesso, "");
                            Thread.Sleep(2000);
                            integracao.IntegracaoRetorno = 1;
                            //CONSULTA DA NOTA
                            GerandoArquivoLog("Consultando NFS-e... ",1);

                            GerarArquivoXMLs(consulta, "ConsultaNota");
                            byte[] Arquivo = Encoding.UTF8.GetBytes(consulta); 
                            SalvarAnexos(NFSe, Arquivo, data, integracao, "Consulta Cancelamento NFS-e");
                            GerandoArquivoLog("Anexo salvo com sucesso...",1);

                            //----------------------------------------Converter XML--------------------------------------
                            NFSeConverterX.spdNFSeConverterX spdNFSeConverterX = new NFSeConverterX.spdNFSeConverterX();
                            NFSeConverterX.spdRetConsultaNFSe retorno = new NFSeConverterX.spdRetConsultaNFSe();
                            retorno = _spdNFSeConverterX.ConverterRetConsultarNFSeTipo(consulta);
                            string Situacao = retorno.Situacao;
                            integracao.RegistroDevolvido = 1;
                            //-------------------------------------------------------------------------------------------
                            IntegracaoPedidoDAL intePedidoDAL = new IntegracaoPedidoDAL();
                            if (Situacao == "AUTORIZADA")
                            {
                                integracao.RegistroMensagem = 1;
                                NFSe.CodigoSituacao = 40;
                                //NOTA AUTORIZADA COM SUCESSO
                                GerandoArquivoLog("Nota Autorizada...",1);
                                intePedidoDAL.AtualizarRetornoPedido(83, "AUTORIZADO", NFSe.CodigoIndexIntegraPedido, NFSe.CodigoNotaFiscalServico);

                            }
                            else if (Situacao == "CANCELADA")
                            {
                                integracao.RegistroMensagem = 1;
                                NFSe.CodigoSituacao = 41;
                                //NOTA CANCELADA COM SUCESSO
                                GerandoArquivoLog("Nota Cancelada...",1);

                                Doc_CtaReceberDAL ContaReceberDAL = new Doc_CtaReceberDAL();
                                ContaReceberDAL.AtualizarParcelas(NFSe.CodigoNotaFiscalServico);
                                intePedidoDAL.AtualizarRetornoPedido(83, "CANCELADA", NFSe.CodigoIndexIntegraPedido, NFSe.CodigoNotaFiscalServico);

                                if (NFSe.CodigoDocumentoOriginal != 0)
                                {
                                    List<Doc_OrdemServico> ListaOSs = new List<Doc_OrdemServico>();
                                    Doc_OrdemServico osFat = new Doc_OrdemServico();
                                    Doc_OrdemServicoDAL osDAL = new Doc_OrdemServicoDAL();
                                    osFat = osDAL.PesquisarDocumento(NFSe.CodigoDocumentoOriginal);
                                    osFat.Cpl_Maquina = integracao.CodigoMaquina;
                                    osFat.Cpl_Usuario = integracao.CodigoUsuario;
                                    osDAL.AtualizarSituacao(osFat, 103);
                                    ListaOSs = osDAL.ListarOrdemServicoServico(osFat.Cpl_CodigoPessoa, 97, NFSe.CodigoDocumentoOriginal);
                                    List<Doc_OrdemServico> ListaNovaOSs = new List<Doc_OrdemServico>();
                                    foreach (Doc_OrdemServico os in ListaOSs)
                                    {
                                        os.Cpl_Maquina = integracao.CodigoMaquina;
                                        os.Cpl_Usuario = integracao.CodigoUsuario;
                                        ListaNovaOSs.Add(os);
                                    }
                                    osDAL.AtualizarOrdemServicoServico(ListaNovaOSs, 108, NFSe.CodigoDocumentoOriginal);
                                }
                            }
                            else
                            {
                                NFSe.CodigoSituacao = 39;
                                //NOTA REJEITADA
                                //MOTIVO DA REJEIÇÃO
                                GerandoArquivoLog("Nota Rejeitada...",1);
                                intePedidoDAL.AtualizarRetornoPedido(82, "ERRO AO CANCELAR", NFSe.CodigoIndexIntegraPedido, NFSe.CodigoNotaFiscalServico);
                            }

                            integracao.Mensagem = Situacao + retorno.Motivo;
                            integraDAL.AtualizarIntegraDocEletronico(integracao);
                            integraDAL.AtualizarDocumentoNFSe(NFSe);
                            EventoDocumento(NFSe, data, integracao);

                            if (Situacao == "AUTORIZADA" || Situacao == "CANCELADA")
                            {
                                integraDAL.Excluir(Convert.ToInt64(integracao.Codigo));
                                GerandoArquivoLog("Registro Apagado do integrador de documentos eletrônicos!",1);
                            }
                        }
                        else
                        {
                            integraDAL.Excluir(integracao.Codigo);
                        }
                    }                   
                }
                if (Contador == 0)
                {
                    GerandoArquivoLog("Nenhuma NFSe para ser Cancelada",1);
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Ocorreu erro durante o processo de cancelamento - "+ex.ToString(),1);
            }
        }
        
        public void btnConsultarNota_Click()
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime data = RnTab.ObterDataHoraServidor();
            IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
            ListaIntegracaoDocEletronico = integraDAL.ListarIntegracaoDocEletronicoCompleto(listaT);
            //-------------------------------------------------------Configuracao do .INI-------------------------------------------------------
            string DiretorioEXE = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\\..\\..\\..\\..\\Modulos";

            _spdNFSeX.LoadConfig(DiretorioEXE + @"\TecnoSpeed\NFSe\Arquivos\nfseConfig.ini");

            //----------------------------------------------------------------------------------------------------------------------------------
            try
            {
                int Contador = 0;
                foreach (IntegraDocumentoEletronico integracao in ListaIntegracaoDocEletronico)
                {
                    if (integracao.CodigoAcao == 45 && integracao.RegistroDevolvido != 1 &&  integracao.IntegracaoRecebido == 0)
                    {
                        //SOLITCITACAO DE CONSULTA DE NOTA
                        Doc_NotaFiscalServico NFSe = new Doc_NotaFiscalServico();
                        Doc_NotaFiscalServicoDAL NFSeDAL = new Doc_NotaFiscalServicoDAL();
                        NFSe = NFSeDAL.PesquisarNotaFiscalServico(Convert.ToInt64(integracao.CodigoDocumento));

                        if ((NFSe.Protocolo != "" ))
                        {
                            Contador++;
                            GerandoArquivoLog("Solicitação de consulta da NFS-e N° " + NFSe.NumeroDocumento,1);
                            integracao.RegistroEnviado = 1;
                            integracao.IntegracaoRecebido = 1;

                            if (NFSe.ChaveAcesso == "ERRO" && NFSe.Protocolo == "")
                            {
                                integracao.Mensagem = "NFS-e sem Chave de Acesso";
                                integraDAL.AtualizarIntegraDocEletronico(integracao);
                                return;
                            }

                            string StrChaveAcesso = "";
                            if (NFSe.ChaveAcesso == "EMPROCESSAMENTO" || NFSe.ChaveAcesso == "ERRO")
                            {

                                string consultaLote = ProxyNFSe.ConsultarLote(NFSe.Protocolo.ToString(), "");
                                Thread.Sleep(2000);
                                //txt - Fazendo consulta do envio de lote
                                GerandoArquivoLog("Consultando o envio de lote - protocolo n° " + NFSe.Protocolo.ToString(),1);


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
                                        GerandoArquivoLog("Chave de acesso não retornada... Nota Rejeitada",1);
                                    }
                                    else
                                    {
                                        GerandoArquivoLog("Chave de acesso não retornada...",1);
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
                            GerandoArquivoLog("Chave de acesso " + StrChaveAcesso,1);                          

                            string consulta = ProxyNFSe.ConsultarNota(StrChaveAcesso, "");
                            
                            integracao.IntegracaoProcessando = 1;
                            integracao.IntegracaoRetorno = 1;
                            //CONSULTANDO NOTA...

                            NFSe.ChaveAcesso = StrChaveAcesso;

                            byte[] Arquivo = Encoding.UTF8.GetBytes(consulta);

                            SalvarAnexos(NFSe, Arquivo, data, integracao, "Consulta NFS-e");
                            GerandoArquivoLog("Anexo salvo com sucesso...",1);

                            NFSeConverterX.spdNFSeConverterX spdNFSeConverterX = new NFSeConverterX.spdNFSeConverterX();
                            NFSeConverterX.spdRetConsultaNFSe retorno = new NFSeConverterX.spdRetConsultaNFSe();
                            retorno = _spdNFSeConverterX.ConverterRetConsultarNFSeTipo(consulta);
                            string Situacao = retorno.Situacao;

                            integracao.RegistroDevolvido = 1;
                            GerandoArquivoLog("Convertendo XML retornado...", 1);
                            int situacaoAnterior = NFSe.CodigoSituacao;
                            //-------------------------------------------------------------------------------------------
                            if (Situacao == "AUTORIZADA")
                            {
                                integracao.RegistroMensagem = 1;
                                NFSe.CodigoSituacao = 40;
                                //NOTA AUTORIZADA
                                GerandoArquivoLog("Nota Autorizada...",1);

                                if (NFSe.CodigoDocumentoOriginal != 0)
                                {
                                    List<Doc_OrdemServico> ListaOSs = new List<Doc_OrdemServico>();
                                    Doc_OrdemServico osFat = new Doc_OrdemServico();
                                    Doc_OrdemServicoDAL osDAL = new Doc_OrdemServicoDAL();
                                    osDAL.AtualizarSituacao(osFat, 104);
                                    osFat = osDAL.PesquisarDocumento(NFSe.CodigoDocumentoOriginal);
                                    ListaOSs = osDAL.ListarOrdemServicoServico(osFat.Cpl_CodigoPessoa, 97, NFSe.CodigoDocumentoOriginal);
                                    foreach (Doc_OrdemServico os in ListaOSs)
                                    {
                                        os.Cpl_Maquina = integracao.CodigoMaquina;
                                        os.Cpl_Usuario = integracao.CodigoUsuario;
                                    }
                                    osDAL.AtualizarOrdemServicoServico(ListaOSs, 109, NFSe.CodigoDocumentoOriginal);
                                }

                            }
                            else if (Situacao == "CANCELADA")
                            {
                                integracao.RegistroMensagem = 1;
                                NFSe.CodigoSituacao = 41;
                                //NOTA CANCELADA
                                GerandoArquivoLog("Nota cancelada...",1);

                                if (NFSe.CodigoDocumentoOriginal != 0)
                                {
                                    List<Doc_OrdemServico> ListaOSs = new List<Doc_OrdemServico>();
                                    Doc_OrdemServico osFat = new Doc_OrdemServico();
                                    Doc_OrdemServicoDAL osDAL = new Doc_OrdemServicoDAL();
                                    
                                    osFat = osDAL.PesquisarDocumento(NFSe.CodigoDocumentoOriginal);
                                    osFat.Cpl_Maquina = integracao.CodigoMaquina;
                                    osFat.Cpl_Usuario = integracao.CodigoUsuario;
                                    osDAL.AtualizarSituacao(osFat, 105);
                                    ListaOSs = osDAL.ListarOrdemServicoServico(osFat.Cpl_CodigoPessoa, 97, NFSe.CodigoDocumentoOriginal);
                                    List<Doc_OrdemServico> ListaNovaOSs = new List<Doc_OrdemServico>();
                                    foreach (Doc_OrdemServico os in ListaOSs)
                                    {
                                        os.Cpl_Maquina = integracao.CodigoMaquina;
                                        os.Cpl_Usuario = integracao.CodigoUsuario;
                                        ListaNovaOSs.Add(os);
                                    }
                                    osDAL.AtualizarOrdemServicoServico(ListaNovaOSs, 108, NFSe.CodigoDocumentoOriginal);
                                }
                            }
                            else
                            {
                                Situacao = "REJEITADA";
                                NFSe.CodigoSituacao = 39;
                                //NOTA REJEITADA
                                GerandoArquivoLog("Nota Rejeitada..." + retorno.Motivo,1);

                                if (NFSe.CodigoDocumentoOriginal != 0)
                                {
                                    List<Doc_OrdemServico> ListaOSs = new List<Doc_OrdemServico>();
                                    Doc_OrdemServico osFat = new Doc_OrdemServico();
                                    Doc_OrdemServicoDAL osDAL = new Doc_OrdemServicoDAL();
                                    
                                    osFat = osDAL.PesquisarDocumento(NFSe.CodigoDocumentoOriginal);
                                    osFat.Cpl_Maquina = integracao.CodigoMaquina;
                                    osFat.Cpl_Usuario = integracao.CodigoUsuario;
                                    osDAL.AtualizarSituacao(osFat, 103);
                                    ListaOSs = osDAL.ListarOrdemServicoServico(osFat.Cpl_CodigoPessoa, 97, NFSe.CodigoDocumentoOriginal);
                                    List<Doc_OrdemServico> ListaNovaOSs = new List<Doc_OrdemServico>();
                                    foreach (Doc_OrdemServico os in ListaOSs)
                                    {
                                        os.Cpl_Maquina = integracao.CodigoMaquina;
                                        os.Cpl_Usuario = integracao.CodigoUsuario;
                                        ListaNovaOSs.Add(os);
                                    }
                                    osDAL.AtualizarOrdemServicoServico(ListaNovaOSs, 108, NFSe.CodigoDocumentoOriginal);
                                }
                            }
                            if (NFSe.CodigoSituacao != situacaoAnterior)
                            {
                                EventoDocumento(NFSe, data, integracao);
                            }
                            integracao.Mensagem = Situacao + " - " + retorno.Motivo;
                            integraDAL.AtualizarIntegraDocEletronico(integracao);
                            integraDAL.AtualizarDocumentoNFSe(NFSe);

                            if (Situacao == "AUTORIZADA" || Situacao == "CANCELADA")
                            {
                                List<IntegraDocumentoEletronico> ListaRejeitadas = new List<IntegraDocumentoEletronico>();
                                ListaRejeitadas = integraDAL.ListarIntegraDocEletronico("CD_DOCUMENTO", "NUMERIC", NFSe.CodigoNotaFiscalServico.ToString(), "", 43);
                                foreach (IntegraDocumentoEletronico i in ListaRejeitadas)
                                {
                                    integraDAL.Excluir(Convert.ToInt64(i.CodigoDocumento));
                                }
                                integraDAL.Excluir(Convert.ToInt64(integracao.Codigo));
                                GerandoArquivoLog("Registro Apagado do integrador de documentos eletrônicos!",1);
                            }
                        }
                        else
                        {
                            integraDAL.Excluir(integracao.Codigo);
                        }
                    }                   
                }

                if (Contador == 0)
                {
                    GerandoArquivoLog("Nenhuma NFSe para ser Consultada",1);
                }
            }
            catch (Exception ex)
            {
                GerandoArquivoLog("Ocorreu erro durante o processo de Consulta - " + ex.ToString(),1);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void GerarArquivoXMLs(string XML, string Tipo)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XML);

            doc.PreserveWhitespace = true;
        }

        protected string gerarXMLPorTX2(IntegraDocumentoEletronico integracao)
        {
            string caminho = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\\..\\..\\..\\..\\Modulos\\Log\\NFSe\\NFSeCod"+integracao.CodigoDocumento + ".tx2";
            if (System.IO.File.Exists(caminho))
                System.IO.File.Delete(caminho);

            System.IO.TextWriter arquivo = System.IO.File.AppendText(caminho);

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
            decimal ValorISS = doc.ValorTotalNota * (doc.ValorAliquotaISSQN / 100);

            arquivo.WriteLine("formato=tx2");
            arquivo.WriteLine("padrao=TecnoNFSe");
            arquivo.WriteLine("NomeCidade="+pEndPrestador._DescricaoMunicipio);
            arquivo.WriteLine("");
            arquivo.WriteLine("INCLUIR" );
            arquivo.WriteLine("NumeroLote=" + doc.NumeroDocumento);
            arquivo.WriteLine("CPFCNPJRemetente=" + pInsPrestador._NumeroInscricao);
            arquivo.WriteLine("InscricaoMunicipalRemetente=" + pInsPrestador._NumeroIM);
            arquivo.WriteLine("ValorTotalServicos=" + doc.ValorTotalNota.ToString("F"));
            arquivo.WriteLine("ValorTotalDeducoes=0.00");
            arquivo.WriteLine("ValorTotalBaseCalculo=" + doc.ValorTotalNota.ToString("F"));
            arquivo.WriteLine("SALVAR");
            arquivo.WriteLine("");
            arquivo.WriteLine("");
            arquivo.WriteLine("INCLUIRRPS");
            arquivo.WriteLine("SituacaoNota=1");
            arquivo.WriteLine("TipoRps=1");
            arquivo.WriteLine("SerieRps="+doc.DGSerieDocumento);
            arquivo.WriteLine("NumeroRps=" + doc.NumeroDocumento);
            arquivo.WriteLine("DataEmissao=" + doc.DataEmissao.ToString("yyyy-MM-ddTHH:mm:ss")); 
            arquivo.WriteLine("Competencia=" + doc.DataEmissao.ToString("yyyy-MM-dd"));
            arquivo.WriteLine("CpfCnpjPrestador="+pInsPrestador._NumeroInscricao);
            arquivo.WriteLine("InscricaoMunicipalPrestador="+pInsPrestador._NumeroIM);
            arquivo.WriteLine("RazaoSocialPrestador="+pPrestador.NomePessoa);
            arquivo.WriteLine("InscricaoEstadualPrestador="+pInsPrestador._NumeroIERG);
            arquivo.WriteLine("TipoLogradouroPrestador=");
            arquivo.WriteLine("EnderecoPrestador="+ pEndPrestador._Logradouro);
            arquivo.WriteLine("NumeroPrestador="+ pEndPrestador._NumeroLogradouro);
            arquivo.WriteLine("ComplementoPrestador=" + pEndPrestador._Complemento);
            arquivo.WriteLine("TipoBairroPrestador=Bairro");
            arquivo.WriteLine("BairroPrestador=" + pEndPrestador._DescricaoBairro);
            arquivo.WriteLine("CodigoCidadePrestador=" + pEndPrestador._CodigoMunicipio);
            arquivo.WriteLine("DescricaoCidadePrestador="+pEndPrestador._DescricaoMunicipio);
            arquivo.WriteLine("TelefonePrestador="+pCttPrestador._Fone1);
            arquivo.WriteLine("EmailPrestador="+pCttPrestador._MailNFSE);
            arquivo.WriteLine("CepPrestador="+pEndPrestador._CodigoCEP);
            arquivo.WriteLine("");
            arquivo.WriteLine("");
            arquivo.WriteLine("OptanteSimplesNacional=1");
            arquivo.WriteLine("IncentivadorCultural=2");
            arquivo.WriteLine("RegimeEspecialTributacao=0");
            arquivo.WriteLine("NaturezaTributacao=1");
            arquivo.WriteLine("IncentivoFiscal=2");
            arquivo.WriteLine("TipoTributacao=6");
            arquivo.WriteLine("ExigibilidadeISS=1");
            arquivo.WriteLine("Operacao=A");
            arquivo.WriteLine("");
            arquivo.WriteLine("CodigoItemListaServico=" + ListaTipoServico[0].CodigoServicoLei);
            arquivo.WriteLine("CodigoTributacaoMunicipio=" + ListaTipoServico[0].CodigoServicoLei);
            arquivo.WriteLine("CodigoCnae=" + ListaTipoServico[0].CodigoCNAE);
            string DiscriminacaoServico = "";
            foreach (TipoServico tipo in ListaTipoServico)
            {
                foreach (ItemTipoServico produto in ListaItemTipoServico)
                {

                    if (produto.CodigoServico == tipo.CodigoServico)
                    {
                        numItemFatura++;
                        if (numItemFatura > 1)
                            DiscriminacaoServico += "|";
                        DiscriminacaoServico += produto.Cpl_DscProduto + " - R$ " + (produto.Quantidade * produto.PrecoItem).ToString("F");

                        //if (numItemFatura > 1)
                        //    arquivo.WriteLine("INCLUIRSERVICO");
                        //arquivo.WriteLine("ValorUnitarioServico=" + produto.PrecoItem);
                        //arquivo.WriteLine("DiscriminacaoServico=" + produto.Cpl_DscProduto);
                        //arquivo.WriteLine("QuantidadeServicos=" + produto.Quantidade);
                        //arquivo.WriteLine("UnidadeServico=UN");
                        //arquivo.WriteLine("ValorServicos=" + produto.Quantidade * produto.PrecoItem);
                        //arquivo.WriteLine("ValorLiquidoServico=" + produto.Quantidade * produto.PrecoItem);
                        //arquivo.WriteLine("Tributavel=N");
                        //arquivo.WriteLine("CodigoItemListaServico=" + DiscriminacaoServico);
                        //arquivo.WriteLine("CodigoCnae=" + tipo.CodigoCNAE);
                        //arquivo.WriteLine("ValorIss=" + tipo.ValorISSQN);///calculo
                        //arquivo.WriteLine("CodigoCidadePrestacao=" + doc.CodigoMunicipioPrestado);
                        //arquivo.WriteLine("AliquotaServico=" + tipo.ValorISSQN);
                        //arquivo.WriteLine("BaseCalculo=0.00");
                        //arquivo.WriteLine("ValorIssRetido=0.00");
                        //if (numItemFatura > 1)
                        //    arquivo.WriteLine("SALVARSERVICO");

                    }
                }
            }
            arquivo.WriteLine("DiscriminacaoServico=" + DiscriminacaoServico);

            arquivo.WriteLine("OutrasInformacoes=" + doc.DescricaoGeralServico);
            arquivo.WriteLine("MunicipioIncidencia="+pEndPrestador._CodigoMunicipio);
            arquivo.WriteLine("CodigoCidadePrestacao=" + doc.CodigoMunicipioPrestado);
            arquivo.WriteLine("DescricaoCidadePrestacao=" + doc.Cpl_DescricaoMunicipio);
            arquivo.WriteLine("");
            arquivo.WriteLine("");
            arquivo.WriteLine("CpfCnpjTomador="+pInsTomador._NumeroInscricao);
            arquivo.WriteLine("RazaoSocialTomador="+pTomador.NomeFantasia);
            arquivo.WriteLine("InscricaoEstadualTomador="+pInsTomador._NumeroIERG);
            arquivo.WriteLine("InscricaoMunicipalTomador="+pInsTomador._NumeroIM);
            arquivo.WriteLine("TipoLogradouroTomador=");
            arquivo.WriteLine("EnderecoTomador="+pEndTomador._Logradouro);
            arquivo.WriteLine("NumeroTomador="+pEndTomador._NumeroLogradouro);
            arquivo.WriteLine("ComplementoTomador="+pEndTomador._Complemento);
            arquivo.WriteLine("BairroTomador=" + pEndTomador._DescricaoBairro);
            arquivo.WriteLine("CodigoCidadeTomador=" + pEndTomador._CodigoMunicipio);
            arquivo.WriteLine("DescricaoCidadeTomador=" + pEndTomador._DescricaoMunicipio);
            arquivo.WriteLine("UfTomador=" + pEndTomador._DescricaoEstado.Substring(0,2));
            arquivo.WriteLine("CepTomador=" + pEndTomador._CodigoCEP);
            arquivo.WriteLine("PaisTomador=1058");
            arquivo.WriteLine("DDDTomador=" + pCttTomador._Fone1.Substring(0,2));
            arquivo.WriteLine("TelefoneTomador="+pCttTomador._Fone1.Substring(2));
            arquivo.WriteLine("EmailTomador="+pCttTomador._MailNFSE);
            arquivo.WriteLine("");
            arquivo.WriteLine("");
            arquivo.WriteLine("AliquotaPIS=0.00");
            arquivo.WriteLine("AliquotaCOFINS=0.00");
            arquivo.WriteLine("AliquotaINSS=0.00");
            arquivo.WriteLine("AliquotaIR=0.00");
            arquivo.WriteLine("AliquotaCSLL=0.00");
            arquivo.WriteLine("ValorPIS=0.00");
            arquivo.WriteLine("ValorCOFINS=0.00");
            arquivo.WriteLine("ValorINSS=0.00");
            arquivo.WriteLine("ValorIR=0.00");
            arquivo.WriteLine("ValorCSLL=0.00");
            arquivo.WriteLine("OutrasRetencoes=0.00");
            arquivo.WriteLine("DescontoIncondicionado=0.00");
            arquivo.WriteLine("DescontoCondicionado=0.00");
            arquivo.WriteLine("ValorDeducoes=0.00");
            arquivo.WriteLine("");
            arquivo.WriteLine("");

            arquivo.WriteLine("ValorServicos=" + (doc.ValorTotalNota).ToString("F"));
            arquivo.WriteLine("BaseCalculo=" + (doc.ValorTotalNota).ToString("F"));
            arquivo.WriteLine("AliquotaISS=3");
            arquivo.WriteLine("ValorIss="+ (ValorISS).ToString("F"));

            arquivo.WriteLine("IssRetido=false");
            arquivo.WriteLine("ValorISSRetido=0.00");

            arquivo.WriteLine("ValorLiquidoNfse="+(doc.ValorTotalNota).ToString("F"));
            arquivo.WriteLine("SALVARRPS");
            arquivo.Close();
            return caminho;
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
                               "<cLCServ>"+ tipo.CodigoServicoLei + "</cLCServ>" +
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
                       "<infAdic>Cidade do Serviço: " + mun.DescricaoMunicipio + " - " + est.Sigla + " / OBS: "+doc.DescricaoGeralServico+"</infAdic>" +
                   "</infNFSe>" +
               "</NFS-e>" +
           "</envioLote>";
            
            return nfse;
        }


    }

}


