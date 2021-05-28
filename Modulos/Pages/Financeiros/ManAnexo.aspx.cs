using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.UI;
using DAL.Model;
using DAL.Persistence;
using System.Runtime.Serialization.Formatters.Binary;
using DAL;

namespace SoftHabilInformatica.Pages.Financeiros
{
    public partial class ManAnexo : System.Web.UI.Page
    {
        public string PanelSelect { get; set; }
        public string DisplayInformacoes = "display:block";
        private List<AnexoDocumento> listaAnexo = new List<AnexoDocumento>();

        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void LimpaTela()
        {
            txtSequencia.Text = "Novo";
            txtNome.Text = "";
        }

        protected Boolean ValidaCampos()
        {
            Boolean blnCampoValido = false;

           
            v.CampoValido("Nome Anexo", txtNome.Text, true, false, false, true, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {

                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtNome.Focus();

                }

                return false;
            }
            return true;
        }
        protected void MontaTela(long CodRegra)
        {
            LimpaTela();

           
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }

            PanelSelect = "home";
            if (Session["NovoAnexo"] != null)
                listaAnexo = (List<AnexoDocumento>)Session["NovoAnexo"];

            //teste
            List<DBTabelaCampos> listaT = new List<DBTabelaCampos>();
            Doc_CtaPagarDAL r = new Doc_CtaPagarDAL();
            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {


                if (Session["ZoomDocCtaAnexo2"] == null)
                    Session["Pagina"] = Request.CurrentExecutionFilePath;

                if (Session["ZoomDocCtaAnexo"] != null)
                {
                    string s = Session["ZoomDocCtaAnexo"].ToString();
                    Session["ZoomDocCtaAnexo"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        btnExcluir.Visible = true;

                        foreach (string word in words)
                        {
                            if (txtSequencia.Text == "")
                                txtSequencia.Text = word;
                        }

                        if (txtSequencia.Text != "Novo")
                        {
                            btnExcluir.Visible = true;
                            if (Session["GeracaoDosEmails"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 9)
                            {
                                List<HabilEmailAnexo> listaAnexosEmail = new List<HabilEmailAnexo>();

                                if (Session["LST_ANEXO"] != null)
                                    listaAnexosEmail = (List<HabilEmailAnexo>)Session["LST_ANEXO"];

                                foreach (HabilEmailAnexo p in listaAnexosEmail)
                                {
                                    if (txtSequencia.Text == p.CD_ANEXO.ToString())
                                    {
                                        btnSalvar.Visible = false;
                                        txtSequencia.Text = p.CD_ANEXO.ToString();
                                        txtNome.Text = p.DS_ARQUIVO.ToString();

                                        Session["arquivo"] = p.TX_CONTEUDO;

                                        arquivo.Visible = false;
                                        titulo.Visible = false;
                                        txtGUID.Visible = true;
                                        txtDSNome.Visible = true;

                                    }
                                }
                            }
                            else
                            {
                                foreach (AnexoDocumento p in listaAnexo)
                                {
                                    if (txtSequencia.Text == p.CodigoAnexo.ToString())
                                    {
                                        btnSalvar.Visible = false;
                                        txtSequencia.Text = p.CodigoAnexo.ToString();
                                        txtNome.Text = p.DescricaoArquivo.ToString();

                                        Session["arquivo"] = p.Arquivo;
                                        txtGUID.Text = p.NomeArquivo;

                                        arquivo.Visible = false;
                                        titulo.Visible = false;
                                        txtGUID.Visible = true;
                                        txtDSNome.Visible = true;

                                        if (p.NaoEditavel == 1)
                                        {
                                            btnSalvar.Visible = false;
                                            btnExcluir.Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Session["ZoomAnexoTimelineCliente"] != null || Session["ZoomAnexoTimelineCliente2"] != null)
                {
                    string strCodigoAgendamento = "";
                    string strCodigoAnexo = "";
                    DisplayInformacoes = "display:none";
                    if (Session["ZoomAnexoTimelineCliente"] != null)
                    {
                        strCodigoAgendamento = Session["ZoomAnexoTimelineCliente"].ToString().Split('³')[0];
                        strCodigoAnexo = Session["ZoomAnexoTimelineCliente"].ToString().Split('³')[1];
                    }
                    else
                    {
                        strCodigoAgendamento = Session["ZoomAnexoTimelineCliente2"].ToString().Split('³')[0];
                        strCodigoAnexo = Session["ZoomAnexoTimelineCliente2"].ToString().Split('³')[1];
                    }

                    List<AnexoAgendamento> ListaAnexo2 = new List<AnexoAgendamento>();
                    AnexoAgendamentoDAL anexoDAL = new AnexoAgendamentoDAL();
                    ListaAnexo2 = anexoDAL.ObterAnexos(Convert.ToDecimal(strCodigoAgendamento));

                    AnexoAgendamento anexoDownload = null;
                    foreach (var anexo in ListaAnexo2)
                    {
                        if (anexo.CodigoAnexo.ToString() == strCodigoAnexo)
                        {
                            anexoDownload = new AnexoAgendamento();
                            anexoDownload = anexo;
                        }
                    }
                    if (anexoDownload != null)
                    {
                        txtSequencia.Text = anexoDownload.CodigoAnexo.ToString();
                        txtNome.Text = anexoDownload.DescricaoArquivo.ToString();
                        
                        Session["arquivo"] = anexoDownload.Arquivo;
                        txtGUID.Text = anexoDownload.NomeArquivo;
                        arquivo.Visible = false;
                        titulo.Visible = false;
                        txtGUID.Visible = true;
                        txtDSNome.Visible = true;
                    }
                    btnSalvar.Visible = false;
                    btnExcluir.Visible = false;
                }
                else
                {
                    LimpaTela();
                    btnExcluir.Visible = false;
                    btnDownload.Visible = false;
                    txtGUID.Visible = false;
                    txtDSNome.Visible = false;                 
                }
                if (Session["Ssn_TipoPessoa"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                {
                    Doc_CtaPagar doc = new Doc_CtaPagar();
                    doc = (Doc_CtaPagar)Session["Ssn_TipoPessoa"];
                    txtPnlCtaPagarCodigo.Text = doc.CodigoDocumento.ToString();
                    txtPnlCtaPagarLancamento.Text = doc.DataEntrada.ToString().Substring(0, 10);
                    txtPnlCtaPagarEmissao.Text = doc.DataEmissao.ToString().Substring(0, 10);

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
                    txtPnlCtaPagarSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
                    txtPnlCtaPagarEmpresa.Text = empresa.NomeEmpresa;

                    txtPnlCtaPagarNroDoc.Text = doc.DGDocumento.ToString();
                    Habil_Tipo habilTipo2 = new Habil_Tipo();
                    habilTipo2 = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoClassificacao);
                    txtPnlCtaPagarClassificacao.Text = habilTipo2.DescricaoTipo.ToString();
                    txtPnlCtaPagarVencimento.Text = doc.DataVencimento.ToString();

                    TipoCobranca tpCobranca = new TipoCobranca();
                    TipoCobrancaDAL tpCobrancaDAL = new TipoCobrancaDAL();
                    tpCobranca = tpCobrancaDAL.PesquisarTipoCobranca(doc.CodigoCobranca);
                    txtPnlCtaPagarCobranca.Text = tpCobranca.DescricaoTipoCobranca;

                    PlanoContas planoConta = new PlanoContas();
                    PlanoContasDAL planoContaDAL = new PlanoContasDAL();
                    planoConta = planoContaDAL.PesquisarPlanoConta(doc.CodigoPlanoContas);
                    txtPnlCtaPagarPlanoConta.Text = planoConta.DescricaoPlanoConta;
                    txtPnlCtaPagarValorDoc.Text = doc.ValorDocumento.ToString();
                    txtPnlCtaPagarValorDesc.Text = doc.ValorDesconto.ToString();
                    txtPnlCtaPagarValorAcres.Text = doc.ValorAcrescimo.ToString();

                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    pessoa = pessoaDAL.PesquisarPessoa(doc.CodigoPessoa);
                    txtPnlCtaPagarCredor.Text = pessoa.NomePessoa;
                    txtPnlCtaPagarOBS.Text = doc.ObservacaoDocumento.ToString();


                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConCtaPagar.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    pnlCtaReceber.Visible = false;
                    pnlOrcamentoPedido.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
                    pnlCTe.Visible = false;
                    pnlCtaPagar.Visible = true;
					pnlOrdCompra.Visible = false;
                    pnlNF.Visible = false;
                    pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = false;
				}
                else if (Session["Ssn_CtaReceber"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 2)
                {
                    Doc_CtaReceber doc = new Doc_CtaReceber();
                    doc = (Doc_CtaReceber)Session["Ssn_CtaReceber"];
                    txtPnlCtaReceberCodigo.Text = doc.CodigoDocumento.ToString();
                    txtPnlCtaReceberLancamento.Text = doc.DataEntrada.ToString().Substring(0, 10);
                    txtPnlCtaReceberEmissao.Text = doc.DataEmissao.ToString().Substring(0, 10);

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
                    txtPnlCtaReceberSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
                    txtPnlCtaReceberEmpresa.Text = empresa.NomeEmpresa;

                    txtPnlCtaReceberNroDoc.Text = doc.DGDocumento.ToString();
                    Habil_Tipo habilTipo2 = new Habil_Tipo();
                    habilTipo2 = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoClassificacao);
                    txtPnlCtaReceberClassificacao.Text = habilTipo2.DescricaoTipo.ToString();
                    txtPnlCtaReceberVencimento.Text = doc.DataVencimento.ToString();

                    TipoCobranca tpCobranca = new TipoCobranca();
                    TipoCobrancaDAL tpCobrancaDAL = new TipoCobrancaDAL();
                    tpCobranca = tpCobrancaDAL.PesquisarTipoCobranca(doc.CodigoCobranca);
                    txtPnlCtaReceberCobranca.Text = tpCobranca.DescricaoTipoCobranca;

                    PlanoContas planoConta = new PlanoContas();
                    PlanoContasDAL planoContaDAL = new PlanoContasDAL();
                    planoConta = planoContaDAL.PesquisarPlanoConta(doc.CodigoPlanoContas);
                    txtPnlCtaReceberPlanoConta.Text = planoConta.DescricaoPlanoConta;
                    txtPnlCtaReceberValorDoc.Text = doc.ValorDocumento.ToString();
                    txtPnlCtaReceberValorDesc.Text = doc.ValorDesconto.ToString();
                    txtPnlCtaReceberValorAcres.Text = doc.ValorAcrescimo.ToString();

                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    pessoa = pessoaDAL.PesquisarPessoa(doc.CodigoPessoa);
                    txtPnlCtaReceberDevedor.Text = pessoa.NomePessoa;
                    txtPnlCtaReceberOBS.Text = doc.ObservacaoDocumento.ToString();


                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConCtaReceber.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });

                    pnlCtaPagar.Visible = false;
                    pnlNF.Visible = false;
                    pnlOrcamentoPedido.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
                    pnlCTe.Visible = false;
                    pnlCtaReceber.Visible = true;
					pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = false;

				}
                else if (Session["NotaFiscalServico"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 3)
                {

                    Doc_NotaFiscalServico nota = (Doc_NotaFiscalServico)Session["NotaFiscalServico"];
                    txtPnlNFSeCodigo.Text = nota.CodigoNotaFiscalServico.ToString();
                    txtPnlNFSeLancamento.Text = nota.DataLancamento.ToString().Substring(0, 10);
                    txtPnlNFSeEmissao.Text = nota.DataEmissao.ToString().Substring(0, 10);

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(nota.CodigoSituacao);
                    txtPnlNFSeSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(nota.CodigoPrestador);
                    txtPnlNFSeEmpresa.Text = empresa.NomeEmpresa;
                    txtPnlNFSeNroDoc.Text = nota.NumeroDocumento.ToString();
                    txtPnlNFSeSerie.Text = nota.DGSerieDocumento.ToString();
                    txtPnlNFSePIS.Text = nota.ValorPIS.ToString();
                    txtPnlNFSeCofins.Text = nota.ValorCofins.ToString();
                    txtPnlNFSeCSLL.Text = nota.ValorCSLL.ToString();
                    txtPnlNFSeIRRF.Text = nota.ValorIRRF.ToString();
                    txtPnlNFSeINSS.Text = nota.ValorINSS.ToString();
                    txtPnlNFSeOutras.Text = nota.ValorOutrasRetencoes.ToString();

                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    pessoa = pessoaDAL.PesquisarPessoa(nota.CodigoTomador);
                    txtPnlNFSeTomador.Text = pessoa.NomePessoa;
                    txtPnlNFSeAliquota.Text = nota.ValorAliquotaISSQN.ToString();

                    Municipio municipio = new Municipio();
                    MunicipioDAL municipioDAL = new MunicipioDAL();
                    municipio = municipioDAL.PesquisarMunicipio(Convert.ToInt64(nota.CodigoMunicipioPrestado));
                    txtPnlNFSeMun.Text = municipio.DescricaoMunicipio;
                    txtPnlNFSeDescricao.Text = nota.DescricaoGeralServico.ToString();

                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConNotaFiscalServico.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    pnlCtaPagar.Visible = false;
                    pnlOrcamentoPedido.Visible = false;
                    pnlNF.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
                    pnlCTe.Visible = false;
					pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = false;
					pnlNFSe.Visible = true;
                }
                else if (Session["Doc_orcamento"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 4)
                {

                    Doc_Orcamento doc = (Doc_Orcamento)Session["Doc_orcamento"];
                    txtPnlOrcamentoCodigo.Text = doc.CodigoDocumento.ToString();

                    txtPnlOrcamentoValidade.Text = doc.DataValidade.ToString().Substring(0, 10);
                    txtPnlOrcamentoEmissao.Text = doc.DataHoraEmissao.ToString().Substring(0, 10);

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
                    txtPnlOrcamentoSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
                    txtPnlOrcamentoEmpresa.Text = empresa.NomeEmpresa;

                    txtPnlOrcamentoNroDoc.Text = doc.NumeroDocumento.ToString();
                    txtPnlOrcamentoSerie.Text = doc.DGSerieDocumento.ToString();

                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
                    txtPnlOrcamentoPessoa.Text = pessoa.NomePessoa;

                    TipoCobranca tpCobranca = new TipoCobranca();
                    TipoCobrancaDAL tpCobrancaDAL = new TipoCobrancaDAL();
                    tpCobranca = tpCobrancaDAL.PesquisarTipoCobranca(doc.CodigoTipoCobranca);

                    txtPnlOrcamentoCobranca.Text = tpCobranca.DescricaoTipoCobranca;
                    txtPnlOrcamentoDescricao.Text = doc.DescricaoDocumento.ToString();

                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConOrcamento.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    pnlCtaPagar.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
                    pnlNF.Visible = false;
                    pnlCTe.Visible = false;
					pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = false;

					pnlOrcamentoPedido.Visible = true;
                }
                else if (Session["Doc_SolicitacaoAtendimento"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 5)
                {

                    Doc_SolicitacaoAtendimento doc = (Doc_SolicitacaoAtendimento)Session["Doc_SolicitacaoAtendimento"];
                    txtPnlSolCodigo.Text = doc.CodigoDocumento.ToString();
                    txtPnlSolEntrada.Text = doc.DataHoraEmissao.ToString().Substring(0, 10);
                    txtPnlSolEmissao.Text = doc.DataHoraEmissao.ToString().Substring(0, 10);

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
                    txtPnlSolSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);

                    txtPnlSolEmpresa.Text = empresa.NomeEmpresa;
                    txtPnlSolNroDoc.Text = doc.NumeroDocumento.ToString();
                    txtPnlSolSerie.Text = doc.DGSerieDocumento.ToString();

                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
                    txtPnlSolPessoa.Text = pessoa.NomePessoa;

                    Habil_Tipo habilTipo2 = new Habil_Tipo();
                    habilTipo2 = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoNivelPrioridade);
                    txtPnlSolNvlPrioridade.Text = habilTipo2.DescricaoTipo;
                    txtPnlSolDescricao.Text = doc.DescricaoDocumento.ToString();
                    txtPnlSolMailSolicitante.Text = doc.Cpl_MailSolicitante;
                    txtPnlSolFoneSolicitante.Text = doc.Cpl_FoneSolicitante;

                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConSolAtendimento.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    pnlCtaPagar.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlOrcamentoPedido.Visible = false;
                    pnlOS.Visible = false;
                    pnlCTe.Visible = false;
					pnlOrdCompra.Visible = false;
                    pnlNF.Visible = false;
                    pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = false;

					pnlSolAtendimento.Visible = true;
                }
                else if (Session["Doc_OrdemServico"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 6)
                {

                    Doc_OrdemServico doc = (Doc_OrdemServico)Session["Doc_OrdemServico"];
                    txtPnlOSCodigo.Text = doc.CodigoDocumento.ToString();

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
                    txtPnlOSSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    Habil_Tipo habilTipo2 = new Habil_Tipo();
                    habilTipo2 = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoClassificacao);
                    txtPnlOSClassificacao.Text = habilTipo2.DescricaoTipo.ToString();

                    txtPnlOSEmissao.Text = doc.DataHoraEmissao.ToString();

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
                    txtPnlOSEmpresa.Text = empresa.NomeEmpresa;

                    txtPnlOSNroDoc.Text = doc.NumeroDocumento.ToString();
                    txtPnlOSSerie.Text = doc.DGSRDocumento;
                    txtPnlOSSol.Text = doc.CodigoSolicAtendimento.ToString();


                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
                    txtPnlOSPessoa.Text = pessoa.NomePessoa;

                    Usuario usu = new Usuario();
                    UsuarioDAL UsuDAL = new UsuarioDAL();
                    usu = UsuDAL.PesquisarUsuario(doc.CodigoUsuarioResponsavel);
                    txtPnlOSUSu.Text = usu.NomeUsuario;

                    txtPnlOSFoneSolicitante.Text = doc.Cpl_FoneSolicitante;
                    txtPnlOSMailSolicitante.Text = doc.Cpl_MailSolicitante;
                    txtPnlOSOBS.Text = doc.ObservacaoDocumento.ToString();

                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConOrdServico.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    pnlCtaPagar.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlOrcamentoPedido.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlCTe.Visible = false;
                    pnlOS.Visible = true;
                    pnlNF.Visible = false;
                    pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = false;
				}
                else if (Session["Doc_CTe"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 7)
                {

                    Doc_CTe doc = (Doc_CTe)Session["Doc_CTe"];
                    txtPnlCTeCodigo.Text = doc.CodigoDocumento.ToString();

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
                    txtPnlCTeSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    txtPnlCTeEmissao.Text = doc.DataHoraEmissao.ToString();
                    txtPnlCTeDtLancamento.Text = doc.DataHoraEmissao.ToString();

                    TipoOperacao TpOperacao = new TipoOperacao();
                    TipoOperacaoDAL TpOperacaoDAL = new TipoOperacaoDAL();
                    TpOperacao = TpOperacaoDAL.PesquisarTipoOperacao(doc.CodigoTipoOperacao);
                    txtPnlCTeTipoOperacao.Text = TpOperacao.DescricaoTipoOperacao;

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
                    txtPnlCTeEmpresa.Text = empresa.NomeEmpresa;

                    txtPnlCTeNroDoc.Text = doc.NumeroDocumento.ToString();
                    txtPnlCTeSerieDoc.Text = doc.DGSRDocumento;
                    txtPnlCTeChaveAcesso.Text = doc.ChaveAcesso;

                    PessoaDAL pessoaDAL = new PessoaDAL();

                    Pessoa pessoaTransportador = new Pessoa();
                    pessoaTransportador = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoTransportador);
                    txtPnlCTeTransportador.Text = pessoaTransportador.NomePessoa;

                    Pessoa pessoaRemetente = new Pessoa();
                    pessoaRemetente = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoRemetente);
                    txtPnlCTeRemetente.Text = pessoaRemetente.NomePessoa;

                    Pessoa pessoaDestinatario = new Pessoa();
                    pessoaDestinatario = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoDestinatario);
                    txtPnlCTeDestinatario.Text = pessoaDestinatario.NomePessoa;

                    Pessoa pessoaTomador = new Pessoa();
                    pessoaTomador = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoTomador);
                    txtPnlCTeTomador.Text = pessoaTomador.NomePessoa;

                    Pessoa pessoaRecebedor = new Pessoa();
                    pessoaRecebedor = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoRecebedor);
                    txtPnlCTeRecebedor.Text = pessoaRecebedor.NomePessoa;
                    txtPnlCTeVlTotal.Text = doc.ValorTotal.ToString();
                    txtPnlCTeOBS.Text = doc.ObservacaoDocumento;

                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConCTe.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    pnlNF.Visible = false;
                    pnlCtaPagar.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlOrcamentoPedido.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
					pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = false;

					pnlCTe.Visible = true;
                }
                else if (Session["Doc_Pedido"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 8)
                {

                    Doc_Pedido doc = (Doc_Pedido)Session["Doc_Pedido"];
                    txtPnlOrcamentoCodigo.Text = doc.CodigoDocumento.ToString();

                    txtPnlOrcamentoValidade.Text = doc.DataValidade.ToString().Substring(0, 10);
                    txtPnlOrcamentoEmissao.Text = doc.DataHoraEmissao.ToString().Substring(0, 10);

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
                    txtPnlOrcamentoSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
                    txtPnlOrcamentoEmpresa.Text = empresa.NomeEmpresa;

                    txtPnlOrcamentoNroDoc.Text = doc.NumeroDocumento.ToString();
                    txtPnlOrcamentoSerie.Text = doc.DGSerieDocumento.ToString();

                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
                    txtPnlOrcamentoPessoa.Text = pessoa.NomePessoa;

                    TipoCobranca tpCobranca = new TipoCobranca();
                    TipoCobrancaDAL tpCobrancaDAL = new TipoCobrancaDAL();
                    tpCobranca = tpCobrancaDAL.PesquisarTipoCobranca(doc.CodigoTipoCobranca);

                    txtPnlOrcamentoCobranca.Text = tpCobranca.DescricaoTipoCobranca;
                    txtPnlOrcamentoDescricao.Text = doc.DescricaoDocumento.ToString();

                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConPedido.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    pnlCtaPagar.Visible = false;
                    pnlNF.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
                    pnlCTe.Visible = false;
					pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = false;
					pnlOrcamentoPedido.Visible = true;
                }
                else if (Session["GeracaoDosEmails"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 9)
                {
                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConGerEmails.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });

                    DisplayInformacoes = "display:none";
                    pnlCtaPagar.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
                    pnlCTe.Visible = false;
                    pnlOrcamentoPedido.Visible = false;
					pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = false;
                    pnlNF.Visible = false;
                    pnlSolCompra.Visible = false;
				}
				else if (Session["Doc_ordproducao"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 10)
				{

					Doc_OrdProducao doc = (Doc_OrdProducao)Session["Doc_ordproducao"];
					txtCodigo.Text = doc.CodigoDocumento.ToString();

					txtDtEmissao.Text = doc.DataHoraEmissao.ToString();
					txtDtEncerramento.Text = doc.DataEncerramento.ToString();

					Habil_Tipo habilTipo = new Habil_Tipo();
					Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
					habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
					txtSituacao.Text = habilTipo.DescricaoTipo.ToString();

					Empresa empresa = new Empresa();
					EmpresaDAL empresaDAL = new EmpresaDAL();
					empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
					txtEmpresa.Text = empresa.NomeEmpresa;

					txtNrDocumento.Text = doc.NumeroDocumento.ToString();
					txtSrDocumento.Text = doc.DGSerieDocumento.ToString();

					Pessoa pessoa = new Pessoa();
					PessoaDAL pessoaDAL = new PessoaDAL();
					pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
					txtPessoa.Text = pessoa.NomePessoa;

					txtDesc.Text = doc.DescricaoDocumento.ToString();

					List<Permissao> lista = new List<Permissao>();
					PermissaoDAL r1 = new PermissaoDAL();
					lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
														Convert.ToInt32(Session["CodPflUsuario"].ToString()),
														"ConOrdProducao.aspx");
					lista.ForEach(delegate (Permissao p)
					{
						if (p.Liberado)
						{
							if (!p.AcessoCompleto)
							{
								if (!p.AcessoUpload)
									btnSalvar.Visible = false;
								if (!p.AcessoExclusaoAnexo)
									btnExcluir.Visible = false;
								if (!p.AcessoDownload)
									btnDownload.Visible = false;
							}
						}
						else
							btnVoltar_Click(sender, e);
					});
					pnlCtaPagar.Visible = false;
					pnlNFSe.Visible = false;
					pnlCtaReceber.Visible = false;
					pnlSolAtendimento.Visible = false;
					pnlOS.Visible = false;
					pnlCTe.Visible = false;

					pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = true;
					pnlSolCompra.Visible = false;

					pnlOrcamentoPedido.Visible = false;
				}
				else if (Session["Doc_OrdCompra"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 11)
				{

					Doc_OrdCompra doc = (Doc_OrdCompra)Session["Doc_OrdCompra"];
					txtPnlOrcamentoCodigo.Text = doc.CodigoDocumento.ToString();

					txtPnlOrcamentoValidade.Text = doc.DataValidade.ToString().Substring(0, 10);
					txtPnlOrcamentoEmissao.Text = doc.DataHoraEmissao.ToString().Substring(0, 10);

					Habil_Tipo habilTipo = new Habil_Tipo();
					Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
					habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
					txtPnlOrcamentoSituacao.Text = habilTipo.DescricaoTipo.ToString();

					Empresa empresa = new Empresa();
					EmpresaDAL empresaDAL = new EmpresaDAL();
					empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
					txtPnlOrcamentoEmpresa.Text = empresa.NomeEmpresa;

					txtPnlOrcamentoNroDoc.Text = doc.NumeroDocumento.ToString();
					txtPnlOrcamentoSerie.Text = doc.DGSerieDocumento.ToString();

					Pessoa pessoa = new Pessoa();
					PessoaDAL pessoaDAL = new PessoaDAL();
					pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
					txtPnlOrcamentoPessoa.Text = pessoa.NomePessoa;

					TipoCobranca tpCobranca = new TipoCobranca();
					TipoCobrancaDAL tpCobrancaDAL = new TipoCobrancaDAL();
					tpCobranca = tpCobrancaDAL.PesquisarTipoCobranca(doc.CodigoTipoCobranca);

					txtPnlOrcamentoCobranca.Text = tpCobranca.DescricaoTipoCobranca;
					txtPnlOrcamentoDescricao.Text = doc.DescricaoDocumento.ToString();

					List<Permissao> lista = new List<Permissao>();
					PermissaoDAL r1 = new PermissaoDAL();
					lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
														Convert.ToInt32(Session["CodPflUsuario"].ToString()),
														"ConOrdCompra.aspx");
					lista.ForEach(delegate (Permissao p)
					{
						if (p.Liberado)
						{
							if (!p.AcessoCompleto)
							{
								if (!p.AcessoUpload)
									btnSalvar.Visible = false;
								if (!p.AcessoExclusaoAnexo)
									btnExcluir.Visible = false;
								if (!p.AcessoDownload)
									btnDownload.Visible = false;
							}
						}
						else
							btnVoltar_Click(sender, e);
					});
					pnlCtaPagar.Visible = false;
					pnlNFSe.Visible = false;
					pnlCtaReceber.Visible = false;
					pnlSolAtendimento.Visible = false;
                    pnlNF.Visible = false;
                    pnlOS.Visible = false;
					pnlCTe.Visible = false;
					pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = true;
					pnlSolCompra.Visible = false;

					pnlOrcamentoPedido.Visible = false;
				}
				else if (Session["Doc_SolCompra"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 12)
				{

					Doc_SolCompra doc = (Doc_SolCompra)Session["Doc_SolCompra"];
					txtPnlOrcamentoCodigo.Text = doc.CodigoDocumento.ToString();

					txtPnlOrcamentoValidade.Text = doc.DataValidade.ToString().Substring(0, 10);
					txtPnlOrcamentoEmissao.Text = doc.DataHoraEmissao.ToString().Substring(0, 10);

					Habil_Tipo habilTipo = new Habil_Tipo();
					Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
					habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
					txtPnlOrcamentoSituacao.Text = habilTipo.DescricaoTipo.ToString();

					Empresa empresa = new Empresa();
					EmpresaDAL empresaDAL = new EmpresaDAL();
					empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
					txtPnlOrcamentoEmpresa.Text = empresa.NomeEmpresa;

					txtPnlOrcamentoNroDoc.Text = doc.NumeroDocumento.ToString();
					txtPnlOrcamentoDescricao.Text = doc.DescricaoDocumento.ToString();

					List<Permissao> lista = new List<Permissao>();
					PermissaoDAL r1 = new PermissaoDAL();
					lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
														Convert.ToInt32(Session["CodPflUsuario"].ToString()),
														"ConSolCompra.aspx");
					lista.ForEach(delegate (Permissao p)
					{
						if (p.Liberado)
						{
							if (!p.AcessoCompleto)
							{
								if (!p.AcessoUpload)
									btnSalvar.Visible = false;
								if (!p.AcessoExclusaoAnexo)
									btnExcluir.Visible = false;
								if (!p.AcessoDownload)
									btnDownload.Visible = false;
							}
						}
						else
							btnVoltar_Click(sender, e);
					});
					pnlCtaPagar.Visible = false;
					pnlNFSe.Visible = false;
					pnlCtaReceber.Visible = false;
					pnlSolAtendimento.Visible = false;
					pnlOS.Visible = false;
					pnlCTe.Visible = false;
                    pnlNF.Visible = false;
                    pnlOrdCompra.Visible = false;
					pnlOrdProducao.Visible = false;
					pnlSolCompra.Visible = true;

					pnlOrcamentoPedido.Visible = false;
				}
                else if (Session["Doc_NotaFiscal"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 13)
                {

                    Doc_NotaFiscal doc = (Doc_NotaFiscal)Session["Doc_NotaFiscal"];
                    txtPnlNFCodigoDocumento.Text = doc.CodigoDocumento.ToString();

                    txtPnlNFEmissao.Text = doc.DataHoraEmissao.ToString().Substring(0, 10);

                    Habil_Tipo habilTipo = new Habil_Tipo();
                    Habil_TipoDAL habilTipoDAL = new Habil_TipoDAL();
                    habilTipo = habilTipoDAL.PesquisarHabil_Tipo(doc.CodigoSituacao);
                    txtPnlNFSituacao.Text = habilTipo.DescricaoTipo.ToString();

                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();
                    empresa = empresaDAL.PesquisarEmpresa(doc.CodigoEmpresa);
                    txtPnlNFEmpresa.Text = empresa.NomeEmpresa;

                    txtPnlNFNroDocumento.Text = doc.NumeroDocumento.ToString();
                    txtPnlNFSerie.Text = doc.DGSerieDocumento.ToString();

                    Pessoa pessoa = new Pessoa();
                    PessoaDAL pessoaDAL = new PessoaDAL();
                    pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);
                    txtPnlNFPessoa.Text = pessoa.NomePessoa;

                    TipoCobranca tpCobranca = new TipoCobranca();
                    TipoCobrancaDAL tpCobrancaDAL = new TipoCobrancaDAL();
                    tpCobranca = tpCobrancaDAL.PesquisarTipoCobranca(doc.CodigoTipoCobranca);

                    txtPnlNFCobranca.Text = tpCobranca.DescricaoTipoCobranca;
                    txtPnlNFObservacao.Text = doc.DescricaoDocumento.ToString();

                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConNotaFiscal.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    pnlCtaPagar.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
                    pnlCTe.Visible = false;
                    pnlOrdCompra.Visible = false;
                    pnlOrdProducao.Visible = false;
                    pnlSolCompra.Visible = false;
                    pnlNF.Visible = true;

                    pnlOrcamentoPedido.Visible = false;
                }
                else if (Session["AgendamentoCompromisso"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 14)
                {
                    List<Permissao> lista = new List<Permissao>();
                    PermissaoDAL r1 = new PermissaoDAL();
                    lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
                                                        Convert.ToInt32(Session["CodPflUsuario"].ToString()),
                                                        "ConAgendamento.aspx");
                    lista.ForEach(delegate (Permissao p)
                    {
                        if (p.Liberado)
                        {
                            if (!p.AcessoCompleto)
                            {
                                if (!p.AcessoUpload)
                                    btnSalvar.Visible = false;
                                if (!p.AcessoExclusaoAnexo)
                                    btnExcluir.Visible = false;
                                if (!p.AcessoDownload)
                                    btnDownload.Visible = false;
                            }
                        }
                        else
                            btnVoltar_Click(sender, e);
                    });
                    DisplayInformacoes = "display:none";
                    pnlCtaPagar.Visible = false;
                    pnlNFSe.Visible = false;
                    pnlCtaReceber.Visible = false;
                    pnlSolAtendimento.Visible = false;
                    pnlOS.Visible = false;
                    pnlCTe.Visible = false;
                    pnlOrdCompra.Visible = false;
                    pnlOrdProducao.Visible = false;
                    pnlSolCompra.Visible = false;
                    pnlNF.Visible = false;

                    pnlOrcamentoPedido.Visible = false;
                }
            }
            
            if (txtSequencia.Text == "")
            {
                btnVoltar_Click(sender, e);
            }
        }

        protected void btnCfmSim_Click(object sender, EventArgs e)
        {
            int intInsItem = 0;


            if (txtSequencia.Text != "Novo")
            {
                intInsItem = Convert.ToInt32(txtSequencia.Text);

                intInsItem = Convert.ToInt32(txtSequencia.Text);

                if (intInsItem != 0)
                    listaAnexo.RemoveAll(x => x.CodigoAnexo == intInsItem);

                Session["NovoAnexo"] = listaAnexo;
                btnExcluir.Visible = false;
                btnSalvar.Visible = true;
                Session["TabFocadaManDocCtaPagar"] = "consulta3";
                Session["MensagemTela"] = "Anexo Excluído com sucesso!";
                btnVoltar_Click(sender,e);


            }

        }

        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;

        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session["TabFocadaNFS"] = "consulta4";
            Session["TabFocada"] = "consulta4";
            Session["TabFocadaManDocCtaPagar"] = "consulta3";
            if(Session["ZoomAnexoTimelineCliente"] != null || Session["ZoomAnexoTimelineCliente2"] != null)
                Session["TabFocada"] = "home";

            Session["ZoomAnexoTimelineCliente"] = null;
            
            if (Session["Ssn_TipoPessoa"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 1)
                Response.Redirect("~/Pages/Financeiros/ManCtaPagar.aspx");
            if (Session["Ssn_CtaReceber"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 2)
                Response.Redirect("~/Pages/Financeiros/ManCtaReceber.aspx");
            if (Session["NotaFiscalServico"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 3)
                Response.Redirect("~/Pages/Servicos/ManNotaFiscalServico.aspx");
            if (Session["Doc_orcamento"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 4)
                Response.Redirect("~/Pages/Vendas/ManOrcamento.aspx");
            if (Session["Doc_SolicitacaoAtendimento"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 5)
                Response.Redirect("~/Pages/Servicos/ManSolAtendimento.aspx");
            if (Session["Doc_OrdemServico"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 6)
                Response.Redirect("~/Pages/Servicos/ManOrdServico.aspx");
            if (Session["Doc_CTe"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 7)
                Response.Redirect("~/Pages/Transporte/ManCTe.aspx");
            if (Session["Doc_Pedido"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 8)
                Response.Redirect("~/Pages/Vendas/ManPedido.aspx");
            if (Session["GeracaoDosEmails"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 9)
            {
                if (Request.QueryString["IN"] != null)
                    Response.Redirect("~/Pages/HabilUtilitarios/ManGerEmails.aspx?IN=" + Request.QueryString["IN"]);
                else
                    Response.Redirect("~/Pages/HabilUtilitarios/ManGerEmails.aspx");
            }
			if (Session["Doc_ordproducao"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 10)
				Response.Redirect("~/Pages/Estoque/ManOrdProducao.aspx");
			if (Session["Doc_OrdCompra"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 11)
				Response.Redirect("~/Pages/Compras/ManOrdCompra.aspx");
            if (Session["Doc_SolCompra"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 12)
                Response.Redirect("~/Pages/Compras/ManSolCompra.aspx");
            else if (Session["Doc_NotaFiscal"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 13)
                Response.Redirect("~/Pages/Faturamento/ManNotaFiscal.aspx");
            else if (Session["AgendamentoCompromisso"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 14)
                Response.Redirect("~/Pages/Compromissos/ManAgendamento.aspx");
            else if (Session["ZoomAnexoTimelineCliente2"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 14)
            {
                Session["ZoomAnexoTimelineCliente2"] = null;
                Response.Redirect("~/Pages/Pessoas/ConPessoa.aspx");
            }
            else
            {
                Session["Ssn_TipoPessoa"] = null;
                Session["Ssn_CtaReceber"] = null;
                Session["Doc_orcamento"] = null;
                Session["NotaFiscalServico"] = null;
                Session["Doc_SolicitacaoAtendimento"] = null;
                Session["Doc_OrdemServico"] = null;
                Session["Doc_CTe"] = null;
                Session["Doc_ordproducao"] = null;
                Session["Doc_OrdCompra"] = null;
                Session["Doc_SolCompra"] = null;
                Session["Doc_NotaFiscal"] = null;
                Session["AgendamentoCompromisso"] = null;

                Response.Redirect("~/Pages/welcome.aspx");

            }
            

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos() == false)
                    return;

                DBTabelaDAL RnTab = new DBTabelaDAL();
                DateTime data = RnTab.ObterDataHoraServidor();

                Habil_Estacao he = new Habil_Estacao();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                he = hedal.PesquisarNomeHabil_Estacao(Session["Estacao_Logada"].ToString());

                Usuario usu = new Usuario();
                UsuarioDAL usuDAL = new UsuarioDAL();
                usu = usuDAL.PesquisarUsuario(Convert.ToInt32(Session["CodUsuario"]));

                if (!arquivo.HasFile)
                {
                    ShowMessage("Anexe arquivo(s)", MessageType.Info);
                    return;
                }

                int ContadorErros = 0;
                string strMensagemErros = "";

                var Arquivos = arquivo.PostedFiles;

                foreach (var file in Arquivos)
                {
                    if (file.InputStream.Length > 0)
                    {
                        int intCttItem = 0;
                        if (txtSequencia.Text != "Novo")
                            intCttItem = Convert.ToInt32(txtSequencia.Text);
                        else
                        {
                            if (listaAnexo.Count != 0)
                                intCttItem = Convert.ToInt32(listaAnexo.Max(x => x.CodigoAnexo).ToString());


                            intCttItem = intCttItem + 1;
                        }
                        if (intCttItem != 0)
                            listaAnexo.RemoveAll(x => x.CodigoAnexo == intCttItem);


                        byte[] ArquivoAnexo;
                        Guid meuNovoGuid;
                        meuNovoGuid = Guid.NewGuid();

                        string ex = file.FileName;
            
                        string ex2 = Path.GetExtension(ex);
                
                        AnexoDocumentoDAL a = new AnexoDocumentoDAL();
                        List<String> str = new List<String>();
                        str = a.ListarExtensao();
                        ArquivoAnexo = null;

                        
                        ex2 = ex2.Substring(1).ToUpper();
                        foreach (String str2 in str)
                        {                    
                            if (ex2 == str2)
                            {
                                BinaryReader br = new BinaryReader(file.InputStream);

                                ArquivoAnexo = br.ReadBytes((int)file.InputStream.Length);
                            }              
                        }
                        

                        if(ArquivoAnexo==null)
                        {
                            ContadorErros++;
                          
                            strMensagemErros += "Arquivo com formato " + ex2 + " indiponível</br>";
                            
                        }
                        else
                        {
                            if (Session["GeracaoDosEmails"] != null && Convert.ToInt32(Request.QueryString["cad"]) == 9)
                            {

                                List<HabilEmailAnexo> listaAnexosEmail = new List<HabilEmailAnexo>();

                                if (Session["LST_ANEXO"] != null)
                                    listaAnexosEmail = (List<HabilEmailAnexo>)Session["LST_ANEXO"];

                                HabilEmailAnexo anexoEmail = new HabilEmailAnexo();
                                anexoEmail.DS_ARQUIVO = txtNome.Text + "." + ex2;
                                anexoEmail.CD_EXTENSAO = a.PesquisaExtensaoPorNome(ex2);
                                anexoEmail.TX_CONTEUDO = ArquivoAnexo;
                                if (listaAnexosEmail.Count == 0)
                                    anexoEmail.CD_ANEXO = 1;
                                else
                                    anexoEmail.CD_ANEXO = listaAnexosEmail.Max(x => x.CD_ANEXO) + 1;

                                listaAnexosEmail.Add(anexoEmail);
                                Session["LST_ANEXO"] = listaAnexosEmail;

                                if (txtSequencia.Text == "Novo")
                                    Session["MensagemTela"] = "Anexo adicionado com sucesso!";
                                else
                                    Session["MensagemTela"] = "Anexo alterada com sucesso!";

                                btnVoltar_Click(sender, e);
                            }

                            AnexoDocumentoDAL anexoDAL = new AnexoDocumentoDAL();

                            AnexoDocumento anexo = new AnexoDocumento(intCttItem,
                                                                    data,
                                                                    Convert.ToInt32(he.CodigoEstacao),
                                                                    usu.CodigoUsuario,
                                                                    anexoDAL.GerarGUID(ex2),
                                                                    ex2,
                                                                    he.NomeEstacao,
                                                                    usu.NomeUsuario,
                                                                    ArquivoAnexo,
                                                                    txtNome.Text + " - " + ex, 0);
                            listaAnexo.Add(anexo);
                        }                                               
                    }
                }
                if (ContadorErros > 0)
                {
                    ShowMessage(strMensagemErros, MessageType.Info);
                    return;
                }
                Session["NovoAnexo"] = listaAnexo;

                if (txtSequencia.Text == "Novo")
                    Session["MensagemTela"] = "Anexo do documento feita com sucesso!";
                else
                    Session["MensagemTela"] = "Anexo do documento alterada com sucesso!";

                

                Session["TabFocada"] = "consulta3";
                btnVoltar_Click(sender, e);
            }
            catch
            {
                btnVoltar_Click(sender, e);
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            byteArrayToAnexo((byte[])(Session["arquivo"]));
        }

        protected void grdBxCtaPagar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void byteArrayToAnexo( byte[] strConteudo)
        {
            string CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\NFSe\\"+ txtGUID.Text;
            if  (System.IO.File.Exists(CaminhoArquivoLog))
                System.IO.File.Delete(CaminhoArquivoLog);

            FileStream file = new FileStream(CaminhoArquivoLog, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(file);
            bw.Write(strConteudo);
            bw.Close();

            file = new FileStream(CaminhoArquivoLog, FileMode.Open);
            BinaryReader br = new BinaryReader(file);
            file.Close();

            Response.Clear();
            Response.ContentType = "application/octect-stream";
            Response.AppendHeader("content-disposition", "filename=" + txtGUID.Text);
            Response.TransmitFile(CaminhoArquivoLog);
            Response.End();


            //            System.Net.WebClient client = new System.Net.WebClient();
            //            client.
            //            client.DownloadFile("http://localhost:59900/arquivo6.pdf", @"C:\sql\arquivo6.pdf");
        }


    }
}