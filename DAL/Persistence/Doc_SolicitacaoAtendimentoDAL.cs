using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class Doc_SolicitacaoAtendimentoDAL : Conexao
    {
        protected string strSQL = "";       
        public bool Inserir(Doc_SolicitacaoAtendimento p, EventoDocumento evento, List<AnexoDocumento> anexo)
        {
            try
            {
                AbrirConexao();               
                strSQL = "insert into DOCUMENTO (CD_CLASSIFICACAO," +
                                                "CD_SITUACAO," +
                                                "DT_HR_EMISSAO," +
                                                "DT_HR_ENTRADA," +
                                                "CD_EMPRESA," +
                                                "CD_NIVEL_PRIORIDADE," +
                                                "NR_DOCUMENTO," +
                                                "DG_SR_DOCUMENTO," +
                                                "DT_ENTREGA," +
                                                "CD_TIPO_DOCUMENTO," +
                                                "CD_GER_SEQ_DOC," +
                                                "NR_HRS_PREVISTAS," +
                                                "VL_TOTAL_GERAL) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v10,@v11,@v12,@v13,@v14); SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);
                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                decimal CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoSolicitacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v4", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoNivelPrioridade);
                Cmd.Parameters.AddWithValue("@v7", CodigoGerado);
                Cmd.Parameters.AddWithValue("@v8", p.DGSerieDocumento);

                if(p.DataConclusao.ToString() == "01/01/0001 00:00:00")
                    Cmd.Parameters.AddWithValue("@v10", "");
                else
                    Cmd.Parameters.AddWithValue("@v10",p.DataConclusao);

                Cmd.Parameters.AddWithValue("@v11", 6);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v13", p.HorasPrevistas);
                Cmd.Parameters.AddWithValue("@v14", p.ValorTotal);

                p.CodigoDocumento = Convert.ToDecimal(Cmd.ExecuteScalar());

                BodyDocumentoDAL BodyDocumentoDAL = new BodyDocumentoDAL();
                BodyDocumento BodyDocumento = new BodyDocumento();
                BodyDocumento.CodigoDocumento = p.CodigoDocumento;
                BodyDocumento.CodigoItem = 0;
                BodyDocumento.TextoCorpo = p.DescricaoDocumento;
                BodyDocumentoDAL.Inserir(BodyDocumento);

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
                            throw new Exception("Erro ao Incluir Solicitação atendimento: " + ex.Message.ToString());
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Solicitação atendimento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirPessoaDocumento(p.Cpl_CodigoPessoa, p);

                EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                eventoDAL.Inserir(evento, p.CodigoDocumento);

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, anexo);         
            }
        }
        public bool Atualizar(Doc_SolicitacaoAtendimento p, EventoDocumento evento, List<AnexoDocumento> anexo)
        {
            try
            {
                Doc_SolicitacaoAtendimento p2 = new Doc_SolicitacaoAtendimento();
                p2 = PesquisarDocumento(Convert.ToDecimal(p.CodigoDocumento));
                GerarLog(p, p2);

                AbrirConexao();
                strSQL = "update DOCUMENTO set CD_CLASSIFICACAO = @v1," +
                                                "CD_SITUACAO = @v2," +
                                                "DT_HR_EMISSAO = @v3," +
                                                "CD_EMPRESA = @v4," +
                                                "CD_NIVEL_PRIORIDADE = @v5," +
                                                "NR_DOCUMENTO = @v6," +
                                                "DG_SR_DOCUMENTO = @v7," +
                                                "DT_ENTREGA = @v9," +
                                                "CD_TIPO_DOCUMENTO = @v10," +
                                                "NR_HRS_PREVISTAS = @v11," +
                                                "VL_TOTAL_GERAL = @v12  where CD_DOCUMENTO = @v13;";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoSolicitacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoNivelPrioridade);
                Cmd.Parameters.AddWithValue("@v6", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.DGSerieDocumento);

                if (p.DataConclusao.ToString() == "01/01/0001 00:00:00")
                    Cmd.Parameters.AddWithValue("@v9", "");
                else
                    Cmd.Parameters.AddWithValue("@v9", p.DataConclusao);

                Cmd.Parameters.AddWithValue("@v10", 6);
                Cmd.Parameters.AddWithValue("@v11", p.HorasPrevistas);
                Cmd.Parameters.AddWithValue("@v12", p.ValorTotal);
                Cmd.ExecuteNonQuery();

                BodyDocumentoDAL BodyDocumentoDAL = new BodyDocumentoDAL();
                BodyDocumento BodyDocumento = new BodyDocumento();
                BodyDocumento.CodigoDocumento = p.CodigoDocumento;
                BodyDocumento.CodigoItem = 0;
                BodyDocumento.TextoCorpo = p.DescricaoDocumento;
                BodyDocumentoDAL.Atualizar(BodyDocumento);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar solicitacao atendimento: " + ex.Message.ToString());
            }
            finally
            {
                AtualizarPessoaDocumento(p.Cpl_CodigoPessoa, p);
                FecharConexao();

                if (evento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(evento, p.CodigoDocumento);
                }

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, anexo);
            }
        }
        public List<Doc_SolicitacaoAtendimento> ListarSolicitacaoAtendimento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select * from [VW_DOC_SOLIC_ATENDIMENTO] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_SolicitacaoAtendimento> lista = new List<Doc_SolicitacaoAtendimento>();

                while (Dr.Read())
                {
                    Doc_SolicitacaoAtendimento p = new Doc_SolicitacaoAtendimento();

                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.CodigoNivelPrioridade = Convert.ToInt32(Dr["CD_NIVEL_PRIORIDADE"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoSolicitacao = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.Cpl_MailSolicitante = Dr["MAIL_SOLICITANTE"].ToString();
                    p.Cpl_FoneSolicitante = Dr["FONE_SOLICITANTE"].ToString();
                    p.DGSerieDocumento = Dr["DG_SR_DOCUMENTO"].ToString();
                    p.DataConclusao = Convert.ToDateTime(Dr["DT_ENTREGA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p.HorasPrevistas = Convert.ToDecimal(Dr["NR_HRS_PREVISTAS"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);

                    BodyDocumento Body = new BodyDocumento();
                    BodyDocumentoDAL BodyDAL = new BodyDocumentoDAL();
                    Body = BodyDAL.PesquisarBodyDocumento(p.CodigoDocumento,0);
                    p.DescricaoDocumento = Body.TextoCorpo;
                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar SOLICITACAO ATENDIMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_SolicitacaoAtendimento> ListarSolicitacaoAtendimentoCompleto(List<DBTabelaCampos> ListaFiltros, int CodSituacao)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select * from [VW_DOC_SOLIC_ATENDIMENTO] ";

                string strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                if (CodSituacao != 0)
                {
                    if (strValor == "")
                        strSQL = strSQL + " WHERE CD_SITUACAO = " + CodSituacao;
                    else
                        strSQL = strSQL + " AND CD_SITUACAO = " + CodSituacao;
                }
                else
                {
                    if (strValor == "")
                        strSQL = strSQL + " WHERE CD_SITUACAO != 37";
                    else
                        strSQL = strSQL + " AND CD_SITUACAO != 37";
                }
                strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Doc_SolicitacaoAtendimento> lista = new List<Doc_SolicitacaoAtendimento>();

                while (Dr.Read())
                {
                    Doc_SolicitacaoAtendimento p = new Doc_SolicitacaoAtendimento();
                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.CodigoNivelPrioridade = Convert.ToInt32(Dr["CD_NIVEL_PRIORIDADE"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoSolicitacao = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.Cpl_MailSolicitante = Dr["MAIL_SOLICITANTE"].ToString();
                    p.Cpl_FoneSolicitante = Dr["FONE_SOLICITANTE"].ToString();
                    p.Cpl_Pessoa = Dr["RAZ_SOCIAL"].ToString();
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.DGSerieDocumento = Dr["DG_SR_DOCUMENTO"].ToString();
                    p.DataConclusao = Convert.ToDateTime(Dr["DT_ENTREGA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p.HorasPrevistas = Convert.ToDecimal(Dr["NR_HRS_PREVISTAS"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar solicitacao atendimento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Doc_SolicitacaoAtendimento PesquisarDocumento(decimal CodDocumento)
        {
            try
            {
                int CodPessoa = PesquisarPessoaDocumento(CodDocumento);
                AbrirConexao();

                string comando = "Select * from VW_DOC_SOLIC_ATENDIMENTO Where CD_DOCUMENTO= @v1 ";

                if (CodDocumento == 0)
                {
                    comando = "SELECT TOP 1 * FROM DOCUMENTO ORDER BY CD_DOCUMENTO DESC ";
                }
                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();
                Doc_SolicitacaoAtendimento p = null;

                if (Dr.Read())
                {
                    p = new Doc_SolicitacaoAtendimento();
                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.CodigoNivelPrioridade = Convert.ToInt32(Dr["CD_NIVEL_PRIORIDADE"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoSolicitacao = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.Cpl_MailSolicitante = Dr["MAIL_SOLICITANTE"].ToString();
                    p.Cpl_FoneSolicitante = Dr["FONE_SOLICITANTE"].ToString();
                    p.Cpl_Pessoa = Dr["RAZ_SOCIAL"].ToString();
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.DescricaoDocumento = Dr["OB_DOCUMENTO"].ToString();
                    p.DGSerieDocumento = Dr["DG_SR_DOCUMENTO"].ToString();
                    p.DataConclusao = Convert.ToDateTime(Dr["DT_ENTREGA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p.HorasPrevistas = Convert.ToDecimal(Dr["NR_HRS_PREVISTAS"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);

                    BodyDocumento Body = new BodyDocumento();
                    BodyDocumentoDAL BodyDAL = new BodyDocumentoDAL();
                    Body = BodyDAL.PesquisarBodyDocumento(p.CodigoDocumento,0);
                    p.DescricaoDocumento = Body.TextoCorpo;
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar solicitacao atendimento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool InserirPessoaDocumento(int p, Doc_SolicitacaoAtendimento doc)
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
                pessoa = pessoaDAL.PesquisarPessoa(p);

                PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
                Pessoa_Contato pesCtt = new Pessoa_Contato();
                pesCtt = pesCttDAL.PesquisarPessoaContato(p, 1);

                PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
                Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                pesEnd = pesEndDAL.PesquisarPessoaEndereco(p, 1);

                PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
                Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                pesIns = pesInsDAL.PesquisarPessoaInscricao(p, 1);

                Cmd.Parameters.AddWithValue("@v25", doc.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v26", 4);
                Cmd.Parameters.AddWithValue("@v27", p);
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
                            throw new Exception("Erro ao incluir Pessoa do documento: " + ex.Message.ToString());
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
        public bool AtualizarPessoaDocumento(int p, Doc_SolicitacaoAtendimento doc)
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
                pessoa = pessoaDAL.PesquisarPessoa(p);

                PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
                Pessoa_Contato pesCtt = new Pessoa_Contato();
                pesCtt = pesCttDAL.PesquisarPessoaContato(p, 1);

                PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
                Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                pesEnd = pesEndDAL.PesquisarPessoaEndereco(p, 1);

                PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
                Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                pesIns = pesInsDAL.PesquisarPessoaInscricao(p, 1);

                Cmd.Parameters.AddWithValue("@v25", doc.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v26", 4);
                Cmd.Parameters.AddWithValue("@v27", p);
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
                            throw new Exception("Erro ao Incluir Pessoa do documento: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Pessoa do documento" + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelSolicitacaoAtendimento(decimal CodigoDocumento)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_DOC_SOLIC_ATENDIMENTO] where CD_DOCUMENTO = " + CodigoDocumento;

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
        public void GerarLog(Doc_SolicitacaoAtendimento p1, Doc_SolicitacaoAtendimento p2)
        {
            Habil_LogDAL logDAL = new Habil_LogDAL();
            DBTabelaDAL db = new DBTabelaDAL();
            long CodIdent = Convert.ToInt64(p1.CodigoDocumento);
            int CodOperacao = 5;

            if (p1.NumeroDocumento != p2.NumeroDocumento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "NR_DOCUMENTO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.NumeroDocumento + " para: " + p1.NumeroDocumento;

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
            if (p1.DataConclusao != p2.DataConclusao && p1.DataConclusao.ToString() != "01/01/0001 00:00:00" && p2.DataConclusao.ToString() != "01/01/0001 00:00:00" && p1.DataConclusao.ToString() != "01/01/1900 00:00:00" && p2.DataConclusao.ToString() != "01/01/1900 00:00:00")
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DT_ENTRADA"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DataConclusao + " para: " + p1.DataConclusao;
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
            
            if (p1.DescricaoDocumento != p2.DescricaoDocumento)
            {
                Habil_Log log = new Habil_Log();
                
                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "OB_DOCUMENTO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DescricaoDocumento + " para: " + p1.DescricaoDocumento;
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
            if (p1.HorasPrevistas != p2.HorasPrevistas)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "NR_HRS_PREVISTAS"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.HorasPrevistas + " para: " + p1.HorasPrevistas;
                logDAL.Inserir(log);
            }
            if (p1.CodigoNivelPrioridade != p2.CodigoNivelPrioridade)
            {
                Habil_Tipo tpDoc = new Habil_Tipo();
                Habil_TipoDAL tpDocDAL = new Habil_TipoDAL();
                tpDoc.DescricaoTipo = tpDocDAL.DescricaoHabil_Tipo(Convert.ToInt32(p2.CodigoNivelPrioridade));

                Habil_Tipo tpDoc2 = new Habil_Tipo();
                Habil_TipoDAL tpDocDAL2 = new Habil_TipoDAL();
                tpDoc2.DescricaoTipo = tpDocDAL2.DescricaoHabil_Tipo(Convert.ToInt32(p1.CodigoNivelPrioridade));


                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_NIVEL_PRIORIDADE"); ;
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
                            throw new Exception("Erro ao deletar documento: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar Documento: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
        public void AtualizarSituacao(Doc_SolicitacaoAtendimento doc, int CodigoSituacao)
        {
            try
            {
                AbrirConexao();

                strSQL = "UPDATE DOCUMENTO SET CD_SITUACAO = @v1 WHERE CD_DOCUMENTO = @v2";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v2", doc.CodigoDocumento);
                Cmd.ExecuteNonQuery();

                EventoDocumento(doc, CodigoSituacao);

                

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
        public void EventoDocumento(Doc_SolicitacaoAtendimento doc, int CodigoSituacao)
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
            eventodoc.CodigoEvento = ListaEvento.Count + 1;
            eventoDAL.Inserir(eventodoc, doc.CodigoDocumento);

        }
    }
}

