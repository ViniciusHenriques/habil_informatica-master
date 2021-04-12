using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Persistence
{
    public class Doc_OrdemServicoDAL : Conexao
    {
        protected string strSQL = "";
        public bool Inserir(Doc_OrdemServico p, EventoDocumento evento, List<AnexoDocumento> anexo, List<TipoServico> ListaTipoServico, List<ParcelaDocumento> ListaParcelas,List<ItemTipoServico> listaItemTipoServico,List<ItemDocumento> ListaItemDocumento, List<Doc_OrdemServico> ListaOSsServico)
        {
            try
            {
                AbrirConexao();
                strSQL = "insert into DOCUMENTO (CD_TIPO_DOCUMENTO," +
                                                "NR_DOCUMENTO," +
                                                "DG_SR_DOCUMENTO," +
                                                "CD_SITUACAO," +
                                                "CD_USU_RESPONSAVEL," +
                                                "OB_DOCUMENTO," +
                                                "DT_HR_EMISSAO," +
                                                "CD_SOL_ATENDIMENTO," +
                                                "CD_EMPRESA," +
                                                "CD_CLASSIFICACAO," +
                                                "CD_GER_SEQ_DOC," +
                                                "DT_HR_ENTRADA," +
                                                "CD_CND_PAGAMENTO," +
                                                "CD_TIPO_COBRANCA," +
                                                "VL_TOTAL_GERAL," +
                                                "CD_DOC_ORIGINAL) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16); SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);
                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                decimal CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", 2);
                Cmd.Parameters.AddWithValue("@v2", CodigoGerado);
                Cmd.Parameters.AddWithValue("@v3", p.DGSRDocumento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoUsuarioResponsavel);
                Cmd.Parameters.AddWithValue("@v6", p.ObservacaoDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoSolicAtendimento);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoClassificacao);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v12", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v14", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v15", p.ValorTotal);
                Cmd.Parameters.AddWithValue("@v16", p.CodigoDocumentoOriginal);

                p.CodigoDocumento = Convert.ToDecimal(Cmd.ExecuteScalar());

                BodyDocumentoDAL BodyDocumentoDAL = new BodyDocumentoDAL();
                BodyDocumento BodyDocumento = new BodyDocumento();
                BodyDocumento.CodigoDocumento = p.CodigoDocumento;
                BodyDocumento.CodigoItem = 0;
                BodyDocumento.TextoCorpo = p.DescricaoDocumento;
                BodyDocumentoDAL.Inserir(BodyDocumento);

                if(p.CodigoClassificacao == 98)
                {
                    AtualizarOrdemServicoServico(ListaOSsServico, 108, p.CodigoDocumento);
                }

                if (p.CodigoSolicAtendimento != 0)
                {
                    Doc_SolicitacaoAtendimento sol = new Doc_SolicitacaoAtendimento();
                    Doc_SolicitacaoAtendimentoDAL solDAL = new Doc_SolicitacaoAtendimentoDAL();
                    sol = solDAL.PesquisarDocumento(p.CodigoSolicAtendimento);
                    sol.Cpl_Maquina = p.Cpl_Maquina;
                    sol.Cpl_Usuario = p.Cpl_Usuario;
                    if (sol.CodigoSituacao != 86)
                        solDAL.AtualizarSituacao(sol, 86);
                } 

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao Incluir ordem de serviço: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar ordem de servico: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirPessoaDocumento(p);

                EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                eventoDAL.Inserir(evento, p.CodigoDocumento);

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, anexo);

                if (p.CodigoClassificacao == 97)
                {
                    ItemDocumentoDAL ItemDAL = new ItemDocumentoDAL();
                    ItemDAL.Inserir(p.CodigoDocumento, ListaItemDocumento);

                }else if(p.CodigoClassificacao == 98)
                {
                    InserirServicoDocumento(p.CodigoDocumento, ListaTipoServico);
                    InserirProdutoDocumento(p.CodigoDocumento, listaItemTipoServico);

                    ParcelaDocumentoDAL ParcelaDAL = new ParcelaDocumentoDAL();
                    ParcelaDAL.Inserir(p.CodigoDocumento, ListaParcelas);
                }

            }
        }
        public bool Atualizar(Doc_OrdemServico p, EventoDocumento evento, List<AnexoDocumento> anexo, List<TipoServico> ListaTipoServico, List<ParcelaDocumento> ListaParcelas, List<ItemTipoServico> listaItemTipoServico,List<ItemDocumento> ListaItemDocumento, List<Doc_OrdemServico> ListaOSsServico)
        {
            try
            {
                Doc_OrdemServico p2 = new Doc_OrdemServico();
                p2 = PesquisarDocumento(Convert.ToDecimal(p.CodigoDocumento));
                GerarLog(p, p2);
                AbrirConexao();

                strSQL = "update DOCUMENTO set NR_DOCUMENTO = @v2," +
                                                "DG_SR_DOCUMENTO = @v3," +
                                                "CD_SITUACAO = @v4," +
                                                "CD_USU_RESPONSAVEL = @v5," +
                                                "OB_DOCUMENTO = @v6," +
                                                "DT_HR_EMISSAO = @v7," +
                                                "CD_SOL_ATENDIMENTO = @v8," +
                                                "CD_EMPRESA = @v9," +
                                                "CD_CLASSIFICACAO = @v10," +
                                                "CD_CND_PAGAMENTO = @v12," +
                                                "CD_TIPO_COBRANCA = @v13," +
                                                "VL_TOTAL_GERAL = @v14" +
                                                "  where CD_DOCUMENTO = @v11";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v2", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.DGSRDocumento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoUsuarioResponsavel);
                Cmd.Parameters.AddWithValue("@v6", p.ObservacaoDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoSolicAtendimento);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoClassificacao);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v14", p.ValorTotal);
                Cmd.ExecuteNonQuery();

                BodyDocumentoDAL BodyDocumentoDAL = new BodyDocumentoDAL();
                BodyDocumento BodyDocumento = new BodyDocumento();
                BodyDocumento.CodigoDocumento = p.CodigoDocumento;
                BodyDocumento.CodigoItem = 0;
                BodyDocumento.TextoCorpo = p.DescricaoDocumento;
                BodyDocumentoDAL.Atualizar(BodyDocumento);

                if (p.CodigoClassificacao == 98)
                {
                    AtualizarOrdemServicoServico(ListaOSsServico, 108, p.CodigoDocumento);
                }

                if (p.CodigoSolicAtendimento != 0)
                {
                    Doc_SolicitacaoAtendimento sol = new Doc_SolicitacaoAtendimento();
                    Doc_SolicitacaoAtendimentoDAL solDAL = new Doc_SolicitacaoAtendimentoDAL();
                    sol = solDAL.PesquisarDocumento(p.CodigoSolicAtendimento);
                    sol.Cpl_Maquina = p.Cpl_Maquina;
                    sol.Cpl_Usuario = p.Cpl_Usuario;
                    if(sol.CodigoSituacao != 86)
                        solDAL.AtualizarSituacao(sol, 86);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar ordem de servico: " + ex.Message.ToString());
            }
            finally
            {
                AtualizarPessoaDocumento(p);
                InserirServicoDocumento(p.CodigoDocumento, ListaTipoServico);
                InserirProdutoDocumento(p.CodigoDocumento, listaItemTipoServico);
                FecharConexao();

                if (evento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(evento, p.CodigoDocumento);
                }

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, anexo);

                ParcelaDocumentoDAL ParcelaDAL = new ParcelaDocumentoDAL();
                ParcelaDAL.Inserir(p.CodigoDocumento, ListaParcelas);

                ItemDocumentoDAL ItemDAL = new ItemDocumentoDAL();
                if (p.CodigoClassificacao == 97)
                    ItemDAL.Inserir(p.CodigoDocumento, ListaItemDocumento);

            }
        }
        public List<Doc_OrdemServico> ListarOrdemServico(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_OS] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);



                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_OrdemServico> lista = new List<Doc_OrdemServico>();

                while (Dr.Read())
                {
                    Doc_OrdemServico p = new Doc_OrdemServico();

                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DGSRDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.ObservacaoDocumento = Dr["OB_DOCUMENTO"].ToString();
                    p.CodigoUsuarioResponsavel = Convert.ToInt32(Dr["CD_USU_RESPONSAVEL"]);
                    p.CodigoClassificacao = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.CodigoSolicAtendimento = Convert.ToDecimal(Dr["CD_SOL_ATENDIMENTO"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["TX_DOCUMENTO"]);
                    p.CodigoContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p.Cpl_Pessoa = Dr["RAZ_SOCIAL"].ToString();
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);
                    p.BtnRemove = false;
                    p.BtnAdd = true;
                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ordem de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_OrdemServico> ListarOrdemServicoServico(Int64 intCodPessoa, int CodTipoOS,decimal CodigoDocumentoOriginal)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_OS] where CD_PESSOA = @v1 AND CD_CLASSIFICACAO = @v2 ";
                if (CodigoDocumentoOriginal == 0)
                    strSQL += "AND CD_SITUACAO IN(102,107)";
                else
                    strSQL += "AND (CD_DOC_ORIGINAL = 0 OR CD_DOC_ORIGINAL = @v3) AND CD_SITUACAO != 101 ";

                strSQL += "AND CD_SITUACAO != 37 ORDER BY CD_DOCUMENTO DESC";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodPessoa);
                Cmd.Parameters.AddWithValue("@v2", CodTipoOS);
                Cmd.Parameters.AddWithValue("@v3", CodigoDocumentoOriginal);
                Dr = Cmd.ExecuteReader();

                List<Doc_OrdemServico> lista = new List<Doc_OrdemServico>();

                while (Dr.Read())
                {
                    Doc_OrdemServico p = new Doc_OrdemServico();

                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DGSRDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.ObservacaoDocumento = Dr["OB_DOCUMENTO"].ToString();
                    p.CodigoUsuarioResponsavel = Convert.ToInt32(Dr["CD_USU_RESPONSAVEL"]);
                    p.CodigoClassificacao = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.CodigoSolicAtendimento = Convert.ToDecimal(Dr["CD_SOL_ATENDIMENTO"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["TX_DOCUMENTO"]);
                    p.CodigoContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p.Cpl_Pessoa = Dr["RAZ_SOCIAL"].ToString();
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);

                    if(p.CodigoDocumentoOriginal != 0)
                    {
                        p.BtnRemove = true;
                        p.BtnAdd = false;
                    }
                    else
                    {
                        p.BtnRemove = false;
                        p.BtnAdd = true;
                    }
                    
                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ordem de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_OrdemServico> ListarOrdemServicoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                strSQL = "Select * from [VW_DOC_OS] ";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                    if (strValor == "")
                        strSQL = strSQL + " WHERE CD_SITUACAO != 37";
                    else
                        strSQL = strSQL + " AND CD_SITUACAO != 37";
                

                strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Doc_OrdemServico> lista = new List<Doc_OrdemServico>();

                while (Dr.Read())
                {
                    Doc_OrdemServico p = new Doc_OrdemServico();
                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DGSRDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.ObservacaoDocumento = Dr["OB_DOCUMENTO"].ToString();
                    p.CodigoUsuarioResponsavel = Convert.ToInt32(Dr["CD_USU_RESPONSAVEL"]);
                    p.CodigoClassificacao = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.CodigoSolicAtendimento = Convert.ToDecimal(Dr["CD_SOL_ATENDIMENTO"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["TX_DOCUMENTO"]);
                    p.CodigoContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p.Cpl_Pessoa = Dr["RAZ_SOCIAL"].ToString();
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);

                    Habil_Tipo tp = new Habil_Tipo();
                    Habil_TipoDAL tpDAL = new Habil_TipoDAL();
                    tp = tpDAL.PesquisarHabil_Tipo(p.CodigoClassificacao);
                    p.Cpl_Classificacao = tp.DescricaoTipo;
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar ordem de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Doc_OrdemServico PesquisarDocumento(decimal CodDocumento)
        {
            try
            {

                int CodPessoa = PesquisarPessoaDocumento(CodDocumento);
                AbrirConexao();

                string comando = "Select * from VW_DOC_OS Where CD_DOCUMENTO= @v1 ";

                if (CodDocumento == 0)
                {
                    comando = "SELECT TOP 1 * FROM DOCUMENTO ORDER BY CD_DOCUMENTO DESC ";
                }
                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();
                Doc_OrdemServico p = new Doc_OrdemServico();

                if (Dr.Read())
                {
                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DGSRDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.Cpl_CodigoPessoa = CodPessoa;
                    p.ObservacaoDocumento = Dr["OB_DOCUMENTO"].ToString();
                    p.CodigoUsuarioResponsavel = Convert.ToInt32(Dr["CD_USU_RESPONSAVEL"]);
                    p.CodigoClassificacao = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.CodigoSolicAtendimento = Convert.ToDecimal(Dr["CD_SOL_ATENDIMENTO"]);
                    p.Cpl_MailSolicitante = Convert.ToString(Dr["MAIL_SOLICITANTE"]);
                    p.Cpl_FoneSolicitante = Convert.ToString(Dr["FONE_SOLICITANTE"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["TX_DOCUMENTO"]);
                    p.CodigoContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar ordem de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool InserirPessoaDocumento(Doc_OrdemServico doc)
        {
            try
            {
                AbrirConexao();

                string strCamposPessoa = "CD_DOCUMENTO";
                string strValoresPessoa = "@v25";

                strCamposPessoa += ", TP_PESSOA";
                strValoresPessoa += ", @v26";

                strCamposPessoa += ", CD_PESSOA";
                strValoresPessoa += ", @v27";

                strCamposPessoa += ", RAZ_SOCIAL";
                strValoresPessoa += ", @v28";

                strCamposPessoa += ", INSCRICAO";
                strValoresPessoa += ", @v29";

                strCamposPessoa += ", INS_ESTADUAL";
                strValoresPessoa += ", @v30";

                strCamposPessoa += ", TELEFONE_1";
                strValoresPessoa += ", @v31";

                strCamposPessoa += ", EMAIL_NFE";
                strValoresPessoa += ", @v32";

                strCamposPessoa += ", EMAIL";
                strValoresPessoa += ", @v33";

                strCamposPessoa += ", LOGRADOURO";
                strValoresPessoa += ", @v34";

                strCamposPessoa += ", NR_ENDERECO";
                strValoresPessoa += ", @v35";

                strCamposPessoa += ", COMPLEMENTO";
                strValoresPessoa += ", @v36";

                strCamposPessoa += ", CD_CEP";
                strValoresPessoa += ", @v37";

                strCamposPessoa += ", CD_MUNICIPIO";
                strValoresPessoa += ", @v38";

                strCamposPessoa += ", CD_BAIRRO";
                strValoresPessoa += ", @v39";

                strCamposPessoa += ", DS_BAIRRO";
                strValoresPessoa += ", @v40";

                strCamposPessoa += ", CD_CONTATO";
                strValoresPessoa += ", @v41";


                strSQL = "insert into PESSOA_DO_DOCUMENTO (" + strCamposPessoa + ") values (" + strValoresPessoa + "); SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                PessoaDAL pessoaDAL = new PessoaDAL();
                Pessoa pessoa = new Pessoa();
                pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);

                PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
                Pessoa_Contato pesCtt = new Pessoa_Contato();
                pesCtt = pesCttDAL.PesquisarPessoaContato(doc.Cpl_CodigoPessoa, 1);

                PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
                Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                pesEnd = pesEndDAL.PesquisarPessoaEndereco(doc.Cpl_CodigoPessoa, 1);

                PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
                Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                pesIns = pesInsDAL.PesquisarPessoaInscricao(doc.Cpl_CodigoPessoa, 1);

                Cmd.Parameters.AddWithValue("@v25", doc.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v26", 5);
                Cmd.Parameters.AddWithValue("@v27", doc.Cpl_CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v28", pessoa.NomePessoa);
                Cmd.Parameters.AddWithValue("@v29", pesIns._NumeroInscricao);
                Cmd.Parameters.AddWithValue("@v30", pesIns._NumeroIERG);
                Cmd.Parameters.AddWithValue("@v31", doc.Cpl_FoneSolicitante);
                Cmd.Parameters.AddWithValue("@v32", pesCtt._MailNFE);
                Cmd.Parameters.AddWithValue("@v33", doc.Cpl_MailSolicitante);
                Cmd.Parameters.AddWithValue("@v34", pesEnd._Logradouro);
                Cmd.Parameters.AddWithValue("@v35", pesEnd._NumeroLogradouro);
                Cmd.Parameters.AddWithValue("@v36", pesEnd._Complemento);
                Cmd.Parameters.AddWithValue("@v37", pesEnd._CodigoCEP);
                Cmd.Parameters.AddWithValue("@v38", pesEnd._CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v39", pesEnd._CodigoBairro);
                Cmd.Parameters.AddWithValue("@v40", pesEnd._DescricaoBairro);
                Cmd.Parameters.AddWithValue("@v41", doc.CodigoContato);

                Cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao incluir conta a receber: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Pessoa do documento: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
        public bool AtualizarPessoaDocumento(Doc_OrdemServico doc)
        {
            try
            {
                AbrirConexao();
                strSQL = "update PESSOA_DO_DOCUMENTO set TP_PESSOA = @v26," +
                                                        "CD_PESSOA = @v27," +
                                                        "RAZ_SOCIAL = @v28," +
                                                        "INSCRICAO = @v29," +
                                                        "INS_ESTADUAL = @v30," +
                                                        "TELEFONE_1 = @v31," +
                                                        "EMAIL_NFE = @v32," +
                                                        "EMAIL = @v33," +
                                                        "LOGRADOURO = @v34," +
                                                        "NR_ENDERECO = @v35," +
                                                        "COMPLEMENTO = @v36," +
                                                        "CD_CEP = @v37," +
                                                        "CD_MUNICIPIO = @v38," +
                                                        "CD_BAIRRO = @v39," +
                                                        "DS_BAIRRO = @v40," +
                                                        "CD_CONTATO = @v41 where CD_DOCUMENTO = @v25";
                Cmd = new SqlCommand(strSQL, Con);

                PessoaDAL pessoaDAL = new PessoaDAL();
                Pessoa pessoa = new Pessoa();
                pessoa = pessoaDAL.PesquisarPessoa(doc.Cpl_CodigoPessoa);

                PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
                Pessoa_Contato pesCtt = new Pessoa_Contato();
                pesCtt = pesCttDAL.PesquisarPessoaContato(doc.Cpl_CodigoPessoa, 1);

                PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
                Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                pesEnd = pesEndDAL.PesquisarPessoaEndereco(doc.Cpl_CodigoPessoa, 1);

                PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
                Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                pesIns = pesInsDAL.PesquisarPessoaInscricao(doc.Cpl_CodigoPessoa, 1);

                Cmd.Parameters.AddWithValue("@v25", doc.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v26", 5);
                Cmd.Parameters.AddWithValue("@v27", doc.Cpl_CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v28", pessoa.NomePessoa);
                Cmd.Parameters.AddWithValue("@v29", pesIns._NumeroInscricao);
                Cmd.Parameters.AddWithValue("@v30", pesIns._NumeroIERG);
                Cmd.Parameters.AddWithValue("@v31", doc.Cpl_FoneSolicitante);
                Cmd.Parameters.AddWithValue("@v32", pesCtt._MailNFE);
                Cmd.Parameters.AddWithValue("@v33", doc.Cpl_MailSolicitante);
                Cmd.Parameters.AddWithValue("@v34", pesEnd._Logradouro);
                Cmd.Parameters.AddWithValue("@v35", pesEnd._NumeroLogradouro);
                Cmd.Parameters.AddWithValue("@v36", pesEnd._Complemento);
                Cmd.Parameters.AddWithValue("@v37", pesEnd._CodigoCEP);
                Cmd.Parameters.AddWithValue("@v38", pesEnd._CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v39", pesEnd._CodigoBairro);
                Cmd.Parameters.AddWithValue("@v40", pesEnd._DescricaoBairro);
                Cmd.Parameters.AddWithValue("@v41", doc.CodigoContato);

                Cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao Incluir ordem de serviço: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar ordem de servico " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public int PesquisarPessoaDocumento(decimal CodDocumento)
        {
            try
            {
                AbrirConexao();

                string comando = "Select CD_PESSOA from PESSOA_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                    return Convert.ToInt32(Dr["CD_PESSOA"]);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar PESSOA DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelOrdemServico(decimal CodigoDocumento, bool OSAgrupada)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "";
                if (OSAgrupada == false)
                    strSQL = "Select * from [VW_DOC_OS] where CD_DOCUMENTO = " + CodigoDocumento;
                else
                    strSQL = "Select * from [VW_DOC_OS] where CD_DOC_ORIGINAL = " + CodigoDocumento;

                strSQL += "ORDER BY CD_DOCUMENTO";
                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public DataTable RelServicoDocumento(decimal CodigoDocumento)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "";
                //strSQL = "SELECT * FROM SERVICO_DO_DOCUMENTO as s inner join PRODUTO_DO_DOCUMENTO as p on s.CD_DOCUMENTO = p.CD_DOCUMENTO where s.CD_DOCUMENTO =" + CodigoDocumento;
                strSQL = "SELECT * FROM SERVICO_DO_DOCUMENTO where CD_DOCUMENTO =" + CodigoDocumento ;
                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public DataTable RelProdutoDocumento(decimal CodigoDocumento)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "";
                //strSQL = "SELECT * FROM SERVICO_DO_DOCUMENTO AS S, PRODUTO_DO_DOCUMENTO AS P WHERE S.CD_DOCUMENTO = " + CodigoDocumento + " AND P.CD_DOCUMENTO = " + CodigoDocumento;
                strSQL = "SELECT * FROM PRODUTO_DO_DOCUMENTO where CD_DOCUMENTO =" + CodigoDocumento;
                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int i = dt.Columns.Count;
                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void GerarLog(Doc_OrdemServico p1, Doc_OrdemServico p2)
        {
            Habil_LogDAL logDAL = new Habil_LogDAL();
            DBTabelaDAL db = new DBTabelaDAL();
            long CodIdent = Convert.ToInt64(p1.CodigoDocumento);
            int CodOperacao = 5;

            if (p1.DGSRDocumento != p2.DGSRDocumento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DG_DOCUMENTO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DGSRDocumento + " para: " + p1.DGSRDocumento;

                logDAL.Inserir(log);
            }
            if (p1.Cpl_CodigoPessoa != p2.Cpl_CodigoPessoa)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("PESSOA_DO_DOCUMENTO", "CD_PESSOA");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.Cpl_CodigoPessoa + " para: " + p1.Cpl_CodigoPessoa;
                logDAL.Inserir(log);
            }
            if (p1.DataHoraEmissao != p2.DataHoraEmissao)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DT_HR_EMISSAO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DataHoraEmissao + " para: " + p1.DataHoraEmissao;
                logDAL.Inserir(log);
            }
            if (p1.Cpl_MailSolicitante != p2.Cpl_MailSolicitante)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("PESSOA_DO_DOCUMENTO", "EMAIL"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.Cpl_MailSolicitante + " para: " + p1.Cpl_MailSolicitante;
                logDAL.Inserir(log);
            }
            if (p1.Cpl_FoneSolicitante != p2.Cpl_FoneSolicitante)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("PESSOA_DO_DOCUMENTO", "TELEFONE_1"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de : " + p2.Cpl_FoneSolicitante + " para: " + p1.Cpl_FoneSolicitante;
                logDAL.Inserir(log);
            }
            if (p1.ValorTotal != p2.ValorTotal)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOTAL_GERAL"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorTotal + " para: " + p1.ValorTotal;
                logDAL.Inserir(log);
            }
            if (p1.DescricaoDocumento != p2.DescricaoDocumento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "TX_DOCUMENTO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DescricaoDocumento + " para: " + p1.DescricaoDocumento;
                logDAL.Inserir(log);
            }
            if (p1.ObservacaoDocumento != p2.ObservacaoDocumento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "OB_DOCUMENTO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ObservacaoDocumento + " para: " + p1.ObservacaoDocumento;
                logDAL.Inserir(log);
            }
            if (p1.CodigoEmpresa != p2.CodigoEmpresa)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_EMPRESA"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoEmpresa + " para: " + p1.CodigoEmpresa;
                logDAL.Inserir(log);
            }
            if (p1.CodigoUsuarioResponsavel != p2.CodigoUsuarioResponsavel)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_USU_RESPONSAVEL"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoUsuarioResponsavel + " para: " + p1.CodigoUsuarioResponsavel;
                logDAL.Inserir(log);
            }
            if (p1.CodigoCondicaoPagamento != p2.CodigoCondicaoPagamento)
            {
                if (p2.CodigoCondicaoPagamento != 0)
                {
                    CondPagamento tpDoc = new CondPagamento();
                    CondPagamentoDAL tpDocDAL = new CondPagamentoDAL();
                    tpDoc = tpDocDAL.PesquisarCondPagamento(p1.CodigoCondicaoPagamento);

                    CondPagamento tpDoc2 = new CondPagamento();
                    CondPagamentoDAL tpDocDAL2 = new CondPagamentoDAL();
                    tpDoc2 = tpDocDAL2.PesquisarCondPagamento(p2.CodigoCondicaoPagamento);


                    Habil_Log log = new Habil_Log();

                    log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_CND_PAGAMENTO"); ;
                    log.CodigoEstacao = p1.Cpl_Maquina;
                    log.CodigoIdentificador = CodIdent;
                    log.CodigoOperacao = CodOperacao;
                    log.CodigoUsuario = p1.Cpl_Usuario;

                    log.DescricaoLog = "de: " + tpDoc2.DescricaoCondPagamento + " para: " + tpDoc.DescricaoCondPagamento;
                    logDAL.Inserir(log);
                }

            }
            if (p1.CodigoTipoCobranca != p2.CodigoTipoCobranca)
            {
                if (p2.CodigoTipoCobranca != 0)
                {
                    TipoCobranca tpDoc = new TipoCobranca();
                    TipoCobrancaDAL tpDocDAL = new TipoCobrancaDAL();
                    tpDoc = tpDocDAL.PesquisarTipoCobranca(p1.CodigoTipoCobranca);

                    TipoCobranca tpDoc2 = new TipoCobranca();
                    TipoCobrancaDAL tpDocDAL2 = new TipoCobrancaDAL();
                    tpDoc2 = tpDocDAL2.PesquisarTipoCobranca(p2.CodigoTipoCobranca);


                    Habil_Log log = new Habil_Log();

                    log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_TIPO_COBRANCA"); ;
                    log.CodigoEstacao = p1.Cpl_Maquina;
                    log.CodigoIdentificador = CodIdent;
                    log.CodigoOperacao = CodOperacao;
                    log.CodigoUsuario = p1.Cpl_Usuario;
                    log.DescricaoLog = "de: " + tpDoc2.DescricaoTipoCobranca + " para: " + tpDoc.DescricaoTipoCobranca;
                    logDAL.Inserir(log);
                }

            }
            if (p1.CodigoClassificacao != p2.CodigoClassificacao)
            {
                Habil_Tipo tpDoc = new Habil_Tipo();
                Habil_TipoDAL tpDocDAL = new Habil_TipoDAL();
                tpDoc.DescricaoTipo = tpDocDAL.DescricaoHabil_Tipo(Convert.ToInt32(p1.CodigoClassificacao));

                Habil_Tipo tpDoc2 = new Habil_Tipo();
                Habil_TipoDAL tpDocDAL2 = new Habil_TipoDAL();
                tpDoc2.DescricaoTipo = tpDocDAL2.DescricaoHabil_Tipo(Convert.ToInt32(p2.CodigoClassificacao));


                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_CLASSIFICACAO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + tpDoc.DescricaoTipo + " para: " + tpDoc2.DescricaoTipo;
                logDAL.Inserir(log);
            }
        }
        public bool Excluir(decimal codigo)
        {

            try
            {
                AbrirConexao();

                strSQL = "update DOCUMENTO set CD_SITUACAO = 37 where CD_DOCUMENTO = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", codigo);

                Cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao Incluir ordem de servico: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar ordem de servico: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
        public void InserirServicoDocumento(decimal CodigoDocumento, List<TipoServico> listaTipoServico)
        {
            try
            {
                ExcluirTodosServicosDocumento(CodigoDocumento);
                AbrirConexao();
                foreach (TipoServico p in listaTipoServico)
                {
                    strSQL = "insert into SERVICO_DO_DOCUMENTO (CD_DOCUMENTO,CD_SERV_DOCUMENTO,CD_TIPO_SERVICO,DS_TIPO_SERVICO, CD_CNAE, CD_SERV_LEI) values (@v1,@v2,@v3,@v4,@v5,@v6)";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoServico);
                    Cmd.Parameters.AddWithValue("@v3", p.CodigoTipoServico);
                    Cmd.Parameters.AddWithValue("@v4", p.DescricaoTipoServico);
                    Cmd.Parameters.AddWithValue("@v5", p.CodigoCNAE);

                    Cmd.Parameters.AddWithValue("@v6", p.CodigoServicoLei);

                    Cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao Incluir Servico do Documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar  Servico do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void ExcluirTodosServicosDocumento(decimal CodDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from SERVICO_DO_DOCUMENTO Where CD_DOCUMENTO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodDocumento);
                Cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            throw new InvalidOperationException("Exclusão não Permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao excluir SERVICO DO DOCUMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir SERVICO DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<TipoServico> ObterTipoServico(decimal CodDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from SERVICO_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodDocumento);
                Dr = Cmd.ExecuteReader();
                List<TipoServico> lista = new List<TipoServico>();

                while (Dr.Read())
                {
                    TipoServico p = new TipoServico();
                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    p.CodigoServico = Convert.ToInt32(Dr["CD_SERV_DOCUMENTO"]);
                    p.DescricaoTipoServico = Dr["DS_TIPO_SERVICO"].ToString();
                    p.CodigoCNAE = Convert.ToDecimal(Dr["CD_CNAE"]);

                    p.CodigoServicoLei = Convert.ToDecimal(Dr["CD_SERV_LEI"]);

                    if (Dr["CD_TIPO_SERVICO"] != DBNull.Value)
                    {
                        p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    }

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter SERVICOS DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void InserirProdutoDocumento(decimal CodigoDocumento, List<ItemTipoServico> listaItemTipoServico)
        {
            try
            {
                ExcluirTodosProdutosDocumento(CodigoDocumento);
                AbrirConexao();
                foreach (ItemTipoServico p in listaItemTipoServico)
                {
                    strSQL = "insert into PRODUTO_DO_DOCUMENTO (CD_DOCUMENTO," +
                                                                 "CD_PROD_DOCUMENTO," +
                                                                 "CD_PRODUTO," +
                                                                 "DS_PRODUTO," +
                                                                 "QT_SOLICITADA," +
                                                                 "VL_ITEM," +
                                                                 "VL_TOTAL," +
                                                                 "OB_PROD_DOCUMENTO," +
                                                                 "CD_SERVICO) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9)";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoProdutoDocumento);
                    Cmd.Parameters.AddWithValue("@v3", p.CodigoProduto);

                    Produto produto = new Produto();
                    ProdutoDAL produtoDAL = new ProdutoDAL();
                    produto = produtoDAL.PesquisarProduto(p.CodigoProduto);

                    Cmd.Parameters.AddWithValue("@v4", produto.DescricaoProduto);
                    Cmd.Parameters.AddWithValue("@v5", p.Quantidade);
                    Cmd.Parameters.AddWithValue("@v6", p.PrecoItem);
                    Cmd.Parameters.AddWithValue("@v7", (p.Quantidade * p.PrecoItem));
                    Cmd.Parameters.AddWithValue("@v8", "");
                    Cmd.Parameters.AddWithValue("@v9", p.CodigoServico);
                    Cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao Incluir Servico do Documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar  Servico do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void ExcluirTodosProdutosDocumento(decimal CodDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from PRODUTO_DO_DOCUMENTO Where CD_DOCUMENTO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodDocumento);
                Cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            throw new InvalidOperationException("Exclusão não Permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao excluir PRODUTO DO DOCUMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir PRODUTO DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ItemTipoServico> ObterProdutoDocumento(decimal CodDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from PRODUTO_DO_DOCUMENTO Where CD_DOCUMENTO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodDocumento);
                Dr = Cmd.ExecuteReader();
                List<ItemTipoServico> lista = new List<ItemTipoServico>();

                while (Dr.Read())
                {
                    ItemTipoServico p = new ItemTipoServico();
                    p.CodigoProdutoDocumento = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QT_SOLICITADA"]);
                    p.PrecoItem = Convert.ToDecimal(Dr["VL_ITEM"]);

                    ProdutoDAL produtoDAL = new ProdutoDAL();
                    Produto produto = new Produto();
                    produto = produtoDAL.PesquisarProduto(Convert.ToInt32(Dr["CD_PRODUTO"]));
                    p.Cpl_DscProduto = produto.DescricaoProduto;

                    p.CodigoServico = Convert.ToInt32(Dr["CD_SERVICO"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter Itens do Tipo de Serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void AtualizarOrdemServicoServico( List<Doc_OrdemServico> listaOSs, int CodigoSituacao, decimal CodigoDocumentoOriginal)
        {
            try
            {
                AbrirConexao();
                foreach (Doc_OrdemServico p in listaOSs)
                {

                   
                    if (p.BtnAdd == false && p.BtnRemove == true)
                    {
                        strSQL = "UPDATE DOCUMENTO SET CD_SITUACAO = @v1, CD_DOC_ORIGINAL = @v3 WHERE CD_DOCUMENTO = @v2";

                        Cmd = new SqlCommand(strSQL, Con);
                        Cmd.Parameters.AddWithValue("@v1", CodigoSituacao);
                        Cmd.Parameters.AddWithValue("@v2", p.CodigoDocumento);
                        Cmd.Parameters.AddWithValue("@v3", CodigoDocumentoOriginal);
                        Cmd.ExecuteNonQuery();
                        if (p.CodigoSituacao != CodigoSituacao && p.CodigoDocumento != 0)
                            EventoDocumento(p, CodigoSituacao);
                    }
                    else
                    {
                        if(p.CodigoSituacao == 108)
                        {
                            strSQL = "UPDATE DOCUMENTO SET CD_SITUACAO = @v1, CD_DOC_ORIGINAL = @v3 WHERE CD_DOCUMENTO = @v2";

                            Cmd = new SqlCommand(strSQL, Con);
                            Cmd.Parameters.AddWithValue("@v1", 102);
                            Cmd.Parameters.AddWithValue("@v2", p.CodigoDocumento);
                            Cmd.Parameters.AddWithValue("@v3", 0);
                            Cmd.ExecuteNonQuery();
                            if (p.CodigoSituacao != 102 && p.CodigoDocumento != 0)
                                EventoDocumento(p, 102);
                        }
                       
                    }
                    
                    
                    
                }
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao Incluir OS servico: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar OS servico: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void AtualizarSituacao(Doc_OrdemServico doc, int CodigoSituacao)
        {
            try
            {
                AbrirConexao();

                strSQL = "UPDATE DOCUMENTO SET CD_SITUACAO = @v1 WHERE CD_DOCUMENTO = @v2";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v2", doc.CodigoDocumento);

                EventoDocumento(doc, CodigoSituacao);

                Cmd.ExecuteNonQuery();
                
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao atualizar situcao os: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar situcao os: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void EventoDocumento(Doc_OrdemServico doc, int CodigoSituacao)
        {
            List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
            EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
            ListaEvento = eventoDAL.ObterEventos(doc.CodigoDocumento);
            DBTabelaDAL RnTab = new DBTabelaDAL();

            EventoDocumento eventodoc = new EventoDocumento();
            eventodoc.CodigoDocumento = Convert.ToDecimal(doc.CodigoDocumento);
            eventodoc.CodigoMaquina = doc.Cpl_Maquina;
            eventodoc.CodigoUsuario = doc.Cpl_Usuario;
            eventodoc.CodigoSituacao = CodigoSituacao;
            eventodoc.DataHoraEvento = RnTab.ObterDataHoraServidor();
            eventodoc.CodigoEvento = ListaEvento.Max(x => x.CodigoEvento) + 1;
            eventoDAL.Inserir(eventodoc, doc.CodigoDocumento);

        }
    }
}

