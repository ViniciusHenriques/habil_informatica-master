using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class Doc_CTeDAL : Conexao
    {
        protected string strSQL = "";
        public bool Inserir(Doc_CTe p, List<AnexoDocumento> ListaAnexos, List<EventoEletronicoDocumento> ListaEventoEletronico) { 
            try
            {
                AbrirConexao();
                strSQL = "insert into DOCUMENTO (CD_TIPO_DOCUMENTO," +
                                                "NR_DOCUMENTO," +
                                                "DG_SR_DOCUMENTO," +
                                                "CD_SITUACAO," +
                                                "OB_DOCUMENTO," +
                                                "DT_HR_EMISSAO," +
                                                "DT_HR_ENTRADA," +
                                                "CD_EMPRESA," +
                                                "CD_GER_SEQ_DOC," +
                                                "CD_TIPO_OPERACAO," +
                                                "VL_TOTAL_GERAL," +
                                                "CD_CHAVE_ACESSO) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12); SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);
                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                decimal CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", 7);
                Cmd.Parameters.AddWithValue("@v2", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.DGSRDocumento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v5", p.ObservacaoDocumento);
                Cmd.Parameters.AddWithValue("@v6", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v7", p.DataHoraLancamento);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v11", p.ValorTotal);
                Cmd.Parameters.AddWithValue("@v12", p.ChaveAcesso);

                p.CodigoDocumento = Convert.ToDecimal(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao Incluir CTe: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar CTe: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTransportador, 7);//Pessoa do Documento Transportador
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoRemetente, 8);//Pessoa do Documento Remetente
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoDestinatario, 9);//Pessoa do Documento Destinatario
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTomador, 10);//Pessoa do Documento Tomador
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoRecebedor, 11);//Pessoa do Documento Recebedor

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexos);

                EventoEletronicoDocumentoDAL EventoEletronicoDAL = new EventoEletronicoDocumentoDAL();
                EventoEletronicoDAL.Inserir(ListaEventoEletronico, p.CodigoDocumento);
            }
        }
        public bool Atualizar(Doc_CTe p, List<AnexoDocumento> ListaAnexos, List<EventoEletronicoDocumento> ListaEventoEletronico)
        {
            try
            {
                Doc_CTe p2 = new Doc_CTe();
                p2 = PesquisarDocumento(Convert.ToDecimal(p.CodigoDocumento));
                //GerarLog(p, p2);
                AbrirConexao();

                strSQL = "update DOCUMENTO set NR_DOCUMENTO = @v2," +
                                                "DG_SR_DOCUMENTO = @v3," +
                                                "CD_SITUACAO = @v4," +
                                                "OB_DOCUMENTO = @v5," +
                                                "DT_HR_EMISSAO = @v6," +
                                                "DT_HR_ENTRADA = @v7," +
                                                "CD_EMPRESA = @v8," +
                                               // "CD_GER_SEQ_DOC = @v9," +
                                                "CD_TIPO_OPERACAO = @v10," +
                                                "VL_TOTAL_GERAL = @v11," +
                                                "CD_CHAVE_ACESSO = @v12 where CD_DOCUMENTO = @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.DGSRDocumento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v5", p.ObservacaoDocumento);
                Cmd.Parameters.AddWithValue("@v6", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v7", p.DataHoraLancamento);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoEmpresa);
                //Cmd.Parameters.AddWithValue("@v9", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v11", p.ValorTotal);
                Cmd.Parameters.AddWithValue("@v12", p.ChaveAcesso);

                Cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar CTe: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTransportador, 7);//Pessoa do Documento Transportador
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoRemetente, 8);//Pessoa do Documento Remetente
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoDestinatario, 9);//Pessoa do Documento Destinatario
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTomador, 10);//Pessoa do Documento Tomador
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoRecebedor, 11);//Pessoa do Documento Recebedor
                

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexos);

                EventoEletronicoDocumentoDAL EventoEletronicoDAL = new EventoEletronicoDocumentoDAL();
                EventoEletronicoDAL.Inserir(ListaEventoEletronico, p.CodigoDocumento);
            }
        }
        public List<Doc_CTe> ListarCTe(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_CTE] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_CTe> lista = new List<Doc_CTe>();

                while (Dr.Read())
                {
                    Doc_CTe p = new Doc_CTe();

                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataHoraLancamento = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.DGSRDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.ObservacaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.ChaveAcesso = Convert.ToString(Dr["CD_CHAVE_ACESSO"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt64(Dr["CD_GER_SEQ_DOC"]);
                    p.Cpl_DsSituacao = Dr["DS_SITUACAO"].ToString();

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar CTe: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_CTe> ListarCTEsCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                strSQL = "Select * from [VW_DOC_CTE] ";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                if (strValor == "")
                    strSQL = strSQL + " WHERE CD_SITUACAO != 37";
                else
                    strSQL = strSQL + " AND CD_SITUACAO != 37";


                strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Doc_CTe> lista = new List<Doc_CTe>();

                while (Dr.Read())
                {
                    Doc_CTe p = new Doc_CTe();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataHoraLancamento = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.DGSRDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.ObservacaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.ChaveAcesso = Convert.ToString(Dr["CD_CHAVE_ACESSO"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt64(Dr["CD_GER_SEQ_DOC"]);
                    p.Cpl_DsSituacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_Transportador = Dr["RAZ_SOCIAL_TRANSPORTADOR"].ToString();
                    p.Cpl_Remetente = Dr["RAZ_SOCIAL_REMETENTE"].ToString();
                    p.Cpl_Destinatario = Dr["RAZ_SOCIAL_DESTINATARIO"].ToString();

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar CTe: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Doc_CTe PesquisarDocumento(decimal CodDocumento)
        {
            try
            {

                long CodTransportador = PesquisarPessoaDocumento(CodDocumento, 7);
                long CodRemetente = PesquisarPessoaDocumento(CodDocumento, 8);
                long CodDestinatario = PesquisarPessoaDocumento(CodDocumento, 9);
                long CodTomador = PesquisarPessoaDocumento(CodDocumento, 10);
                long CodRecebedor = PesquisarPessoaDocumento(CodDocumento, 11);
                AbrirConexao();

                string comando = "Select * from VW_DOC_CTE Where CD_DOCUMENTO= @v1 ";

                if (CodDocumento == 0)
                {
                    comando = "SELECT TOP 1 * FROM DOCUMENTO ORDER BY CD_DOCUMENTO DESC ";
                }
                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();
                Doc_CTe p = new Doc_CTe();

                if (Dr.Read())
                {
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataHoraLancamento = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.DGSRDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.ObservacaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.ChaveAcesso = Convert.ToString(Dr["CD_CHAVE_ACESSO"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt64(Dr["CD_GER_SEQ_DOC"]);
                    p.Cpl_CodigoTransportador = CodTransportador;
                    p.Cpl_CodigoDestinatario = CodDestinatario;
                    p.Cpl_CodigoRecebedor = CodRecebedor;
                    p.Cpl_CodigoTomador = CodTomador;
                    p.Cpl_CodigoRemetente = CodRemetente;

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar CTe: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool InserirPessoaDocumento(decimal CodigoDocumento, long CodigoPessoa, int TipoPessoa)
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



                strSQL = "insert into PESSOA_DO_DOCUMENTO (" + strCamposPessoa + ") values (" + strValoresPessoa + "); SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                PessoaDAL pessoaDAL = new PessoaDAL();
                Pessoa pessoa = new Pessoa();
                pessoa = pessoaDAL.PesquisarPessoa(CodigoPessoa);

                PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
                Pessoa_Contato pesCtt = new Pessoa_Contato();
                pesCtt = pesCttDAL.PesquisarPessoaContato(CodigoPessoa, 1);

                PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
                Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                pesEnd = pesEndDAL.PesquisarPessoaEndereco(CodigoPessoa, 1);

                PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
                Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                pesIns = pesInsDAL.PesquisarPessoaInscricao(CodigoPessoa, 1);

                Cmd.Parameters.AddWithValue("@v25", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v26", TipoPessoa);
                Cmd.Parameters.AddWithValue("@v27", CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v28", pessoa.NomePessoa);
                Cmd.Parameters.AddWithValue("@v29", pesIns._NumeroInscricao);
                Cmd.Parameters.AddWithValue("@v30", pesIns._NumeroIERG);
                Cmd.Parameters.AddWithValue("@v31", pesCtt._Fone1);
                Cmd.Parameters.AddWithValue("@v32", pesCtt._MailNFE);
                Cmd.Parameters.AddWithValue("@v33", pesCtt._Mail1);
                Cmd.Parameters.AddWithValue("@v34", pesEnd._Logradouro);
                Cmd.Parameters.AddWithValue("@v35", pesEnd._NumeroLogradouro);
                Cmd.Parameters.AddWithValue("@v36", pesEnd._Complemento);
                Cmd.Parameters.AddWithValue("@v37", pesEnd._CodigoCEP);
                Cmd.Parameters.AddWithValue("@v38", pesEnd._CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v39", pesEnd._CodigoBairro);
                Cmd.Parameters.AddWithValue("@v40", pesEnd._DescricaoBairro);

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
        public bool AtualizarPessoaDocumento(decimal CodigoDocumento, long CodigoPessoa, int TipoPessoa)
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
                                                        "DS_BAIRRO = @v40 where CD_DOCUMENTO = @v25 AND TP_PESSOA = @v26";
                Cmd = new SqlCommand(strSQL, Con);

                PessoaDAL pessoaDAL = new PessoaDAL();
                Pessoa pessoa = new Pessoa();
                pessoa = pessoaDAL.PesquisarPessoa(CodigoPessoa);

                PessoaContatoDAL pesCttDAL = new PessoaContatoDAL();
                Pessoa_Contato pesCtt = new Pessoa_Contato();
                pesCtt = pesCttDAL.PesquisarPessoaContato(CodigoPessoa, 1);

                PessoaEnderecoDAL pesEndDAL = new PessoaEnderecoDAL();
                Pessoa_Endereco pesEnd = new Pessoa_Endereco();
                pesEnd = pesEndDAL.PesquisarPessoaEndereco(CodigoPessoa, 1);

                PessoaInscricaoDAL pesInsDAL = new PessoaInscricaoDAL();
                Pessoa_Inscricao pesIns = new Pessoa_Inscricao();
                pesIns = pesInsDAL.PesquisarPessoaInscricao(CodigoPessoa, 1);

                Cmd.Parameters.AddWithValue("@v25", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v26", TipoPessoa);
                Cmd.Parameters.AddWithValue("@v27", CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v28", pessoa.NomePessoa);
                Cmd.Parameters.AddWithValue("@v29", pesIns._NumeroInscricao);
                Cmd.Parameters.AddWithValue("@v30", pesIns._NumeroIERG);
                Cmd.Parameters.AddWithValue("@v31", pesCtt._Fone1);
                Cmd.Parameters.AddWithValue("@v32", pesCtt._MailNFE);
                Cmd.Parameters.AddWithValue("@v33", pesCtt._Mail1);
                Cmd.Parameters.AddWithValue("@v34", pesEnd._Logradouro);
                Cmd.Parameters.AddWithValue("@v35", pesEnd._NumeroLogradouro);
                Cmd.Parameters.AddWithValue("@v36", pesEnd._Complemento);
                Cmd.Parameters.AddWithValue("@v37", pesEnd._CodigoCEP);
                Cmd.Parameters.AddWithValue("@v38", pesEnd._CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v39", pesEnd._CodigoBairro);
                Cmd.Parameters.AddWithValue("@v40", pesEnd._DescricaoBairro);

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
        public long PesquisarPessoaDocumento(decimal CodDocumento, int TipoPessoa)
        {
            try
            {
                AbrirConexao();

                string comando = "Select CD_PESSOA from PESSOA_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 and TP_PESSOA = @v2 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);
                Cmd.Parameters.AddWithValue("@v2", TipoPessoa);

                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                    return Convert.ToInt64(Dr["CD_PESSOA"]);
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

        //public void GerarLog(Doc_CTe p1, Doc_CTe p2)
        //{
        //    Habil_LogDAL logDAL = new Habil_LogDAL();
        //    DBTabelaDAL db = new DBTabelaDAL();
        //    long CodIdent = Convert.ToInt64(p1.CodigoDocumento);
        //    int CodOperacao = 5;

        //    if (p1.DGSRDocumento != p2.DGSRDocumento)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DG_DOCUMENTO");
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.DGSRDocumento + " para: " + p1.DGSRDocumento;

        //        logDAL.Inserir(log);
        //    }
        //    if (p1.Cpl_CodigoPessoa != p2.Cpl_CodigoPessoa)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("PESSOA_DO_DOCUMENTO", "CD_PESSOA");
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.Cpl_CodigoPessoa + " para: " + p1.Cpl_CodigoPessoa;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.DataHoraEmissao != p2.DataHoraEmissao)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DT_HR_EMISSAO"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.DataHoraEmissao + " para: " + p1.DataHoraEmissao;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.Cpl_MailSolicitante != p2.Cpl_MailSolicitante)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("PESSOA_DO_DOCUMENTO", "EMAIL"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.Cpl_MailSolicitante + " para: " + p1.Cpl_MailSolicitante;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.Cpl_FoneSolicitante != p2.Cpl_FoneSolicitante)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("PESSOA_DO_DOCUMENTO", "TELEFONE_1"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de : " + p2.Cpl_FoneSolicitante + " para: " + p1.Cpl_FoneSolicitante;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.ValorTotal != p2.ValorTotal)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOTAL_GERAL"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.ValorTotal + " para: " + p1.ValorTotal;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.DescricaoDocumento != p2.DescricaoDocumento)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "TX_DOCUMENTO"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.DescricaoDocumento + " para: " + p1.DescricaoDocumento;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.ObservacaoDocumento != p2.ObservacaoDocumento)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "OB_DOCUMENTO"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.ObservacaoDocumento + " para: " + p1.ObservacaoDocumento;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.CodigoEmpresa != p2.CodigoEmpresa)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_EMPRESA"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.CodigoEmpresa + " para: " + p1.CodigoEmpresa;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.CodigoUsuarioResponsavel != p2.CodigoUsuarioResponsavel)
        //    {
        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_USU_RESPONSAVEL"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + p2.CodigoUsuarioResponsavel + " para: " + p1.CodigoUsuarioResponsavel;
        //        logDAL.Inserir(log);
        //    }
        //    if (p1.CodigoCondicaoPagamento != p2.CodigoCondicaoPagamento)
        //    {
        //        if (p2.CodigoCondicaoPagamento != 0)
        //        {
        //            CondPagamento tpDoc = new CondPagamento();
        //            CondPagamentoDAL tpDocDAL = new CondPagamentoDAL();
        //            tpDoc = tpDocDAL.PesquisarCondPagamento(p1.CodigoCondicaoPagamento);

        //            CondPagamento tpDoc2 = new CondPagamento();
        //            CondPagamentoDAL tpDocDAL2 = new CondPagamentoDAL();
        //            tpDoc2 = tpDocDAL2.PesquisarCondPagamento(p2.CodigoCondicaoPagamento);


        //            Habil_Log log = new Habil_Log();

        //            log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_CND_PAGAMENTO"); ;
        //            log.CodigoEstacao = p1.Cpl_Maquina;
        //            log.CodigoIdentificador = CodIdent;
        //            log.CodigoOperacao = CodOperacao;
        //            log.CodigoUsuario = p1.Cpl_Usuario;

        //            log.DescricaoLog = "de: " + tpDoc2.DescricaoCondPagamento + " para: " + tpDoc.DescricaoCondPagamento;
        //            logDAL.Inserir(log);
        //        }

        //    }
        //    if (p1.CodigoTipoCobranca != p2.CodigoTipoCobranca)
        //    {
        //        if (p2.CodigoTipoCobranca != 0)
        //        {
        //            TipoCobranca tpDoc = new TipoCobranca();
        //            TipoCobrancaDAL tpDocDAL = new TipoCobrancaDAL();
        //            tpDoc = tpDocDAL.PesquisarTipoCobranca(p1.CodigoTipoCobranca);

        //            TipoCobranca tpDoc2 = new TipoCobranca();
        //            TipoCobrancaDAL tpDocDAL2 = new TipoCobrancaDAL();
        //            tpDoc2 = tpDocDAL2.PesquisarTipoCobranca(p2.CodigoTipoCobranca);


        //            Habil_Log log = new Habil_Log();

        //            log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_TIPO_COBRANCA"); ;
        //            log.CodigoEstacao = p1.Cpl_Maquina;
        //            log.CodigoIdentificador = CodIdent;
        //            log.CodigoOperacao = CodOperacao;
        //            log.CodigoUsuario = p1.Cpl_Usuario;
        //            log.DescricaoLog = "de: " + tpDoc2.DescricaoTipoCobranca + " para: " + tpDoc.DescricaoTipoCobranca;
        //            logDAL.Inserir(log);
        //        }

        //    }
        //    if (p1.CodigoClassificacao != p2.CodigoClassificacao)
        //    {
        //        Habil_Tipo tpDoc = new Habil_Tipo();
        //        Habil_TipoDAL tpDocDAL = new Habil_TipoDAL();
        //        tpDoc.DescricaoTipo = tpDocDAL.DescricaoHabil_Tipo(Convert.ToInt32(p1.CodigoClassificacao));

        //        Habil_Tipo tpDoc2 = new Habil_Tipo();
        //        Habil_TipoDAL tpDocDAL2 = new Habil_TipoDAL();
        //        tpDoc2.DescricaoTipo = tpDocDAL2.DescricaoHabil_Tipo(Convert.ToInt32(p2.CodigoClassificacao));


        //        Habil_Log log = new Habil_Log();

        //        log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_CLASSIFICACAO"); ;
        //        log.CodigoEstacao = p1.Cpl_Maquina;
        //        log.CodigoIdentificador = CodIdent;
        //        log.CodigoOperacao = CodOperacao;
        //        log.CodigoUsuario = p1.Cpl_Usuario;
        //        log.DescricaoLog = "de: " + tpDoc.DescricaoTipo + " para: " + tpDoc2.DescricaoTipo;
        //        logDAL.Inserir(log);
        //    }
        //}
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
                            throw new Exception("Erro ao Incluir CTe: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar CTe: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
    }
}

