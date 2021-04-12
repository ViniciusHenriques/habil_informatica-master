using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class Doc_NotaFiscalServicoDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(Doc_NotaFiscalServico p, List<TipoServico> ListaTipoServico, List<ItemTipoServico> listaItemTipoServico,EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento, List<ParcelaDocumento> ListaParcelaDocumento, ref decimal CodigoDocumento)
        {            
            try
            {
                AbrirConexao();
                strSQL = "insert into DOCUMENTO (OB_DOCUMENTO," +
                                                "CD_GER_SEQ_DOC," +
                                                "DT_HR_ENTRADA," +
                                                "CD_EMPRESA," +
                                                "VL_TOT_PIS," +
                                                "VL_TOT_COFINS," +
                                                "VL_TOT_CSLL," +
                                                "VL_TOT_IRRF," +
                                                "VL_TOT_INSS," +
                                                "VL_TOT_OUTRAS," +
                                                "CD_SITUACAO," +
                                                "NR_DOCUMENTO," +
                                                "NR_SR_DOCUMENTO," +
                                                "VL_ALIQ_ISSQN," +
                                                "CD_MUN_SERV_PRESTADO," +
                                                "CD_TIPO_DOCUMENTO," +
                                                "DT_HR_EMISSAO," +
                                                "VL_TOTAL_GERAL," +
                                                "CD_CHAVE_ACESSO," +
                                                "NR_PROTOCOLO," +
                                                "DG_SR_DOCUMENTO," +
                                                "CD_TIPO_OPERACAO," +
                                                "CD_CND_PAGAMENTO," +
                                                "CD_INDEX_INTEGRACAO_PEDIDO," +
                                                "CD_DOC_ORIGINAL) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25) SELECT SCOPE_IDENTITY();" +
                                                "";

                Cmd = new SqlCommand(strSQL, Con);

                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                decimal CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", p.DescricaoGeralServico.ToUpper());
                Cmd.Parameters.AddWithValue("@v2", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.DataLancamento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoPrestador);
                Cmd.Parameters.AddWithValue("@v5", p.ValorPIS);
                Cmd.Parameters.AddWithValue("@v6", p.ValorCofins);
                Cmd.Parameters.AddWithValue("@v7", p.ValorCSLL);
                Cmd.Parameters.AddWithValue("@v8", p.ValorIRRF);
                Cmd.Parameters.AddWithValue("@v9", p.ValorINSS);
                Cmd.Parameters.AddWithValue("@v10", p.ValorOutrasRetencoes);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v12", CodigoGerado);
                Cmd.Parameters.AddWithValue("@v13", p.NumeroSerie);
                Cmd.Parameters.AddWithValue("@v14", p.ValorAliquotaISSQN);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoMunicipioPrestado);
                Cmd.Parameters.AddWithValue("@v16", 1);
                Cmd.Parameters.AddWithValue("@v17", p.DataEmissao);
                Cmd.Parameters.AddWithValue("@v18", p.ValorTotalNota);
                Cmd.Parameters.AddWithValue("@v19", "");
                Cmd.Parameters.AddWithValue("@v20", 0);
                Cmd.Parameters.AddWithValue("@v21", p.DGSerieDocumento);
                Cmd.Parameters.AddWithValue("@v22", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v23", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v24", 0);
                Cmd.Parameters.AddWithValue("@v25", p.CodigoDocumentoOriginal);

                p.CodigoNotaFiscalServico = Convert.ToDecimal(Cmd.ExecuteScalar());

                CodigoDocumento = p.CodigoNotaFiscalServico;
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
                            throw new Exception("Erro ao Incluir nota fiscal de serviço: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar nota fiscal de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirPessoaDocumento(p.CodigoTomador, p.CodigoNotaFiscalServico);
                InserirServicoDocumento(p.CodigoNotaFiscalServico, ListaTipoServico);
                InserirProdutoDocumento(p.CodigoNotaFiscalServico, listaItemTipoServico);
                if (eventoDocumento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(eventoDocumento, p.CodigoNotaFiscalServico);
                }
                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoNotaFiscalServico, ListaAnexoDocumento);

                ParcelaDocumentoDAL ParcelaDAL = new ParcelaDocumentoDAL();
                ParcelaDAL.Inserir(p.CodigoNotaFiscalServico, ListaParcelaDocumento);
            }
        }
        public void Atualizar(Doc_NotaFiscalServico p, List<TipoServico> ListaTipoServico, List<ItemTipoServico> listaItemTipoServico, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento, List<ParcelaDocumento> ListaParcelaDocumento)
        {
            try
            {
                Doc_NotaFiscalServico p2 = new Doc_NotaFiscalServico();
                p2 = PesquisarNotaFiscalServico(Convert.ToInt32(p.CodigoNotaFiscalServico));
                GerarLog(p, p2);
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set " +
                                                "OB_DOCUMENTO = @v2," +
                                                //"CD_GER_SEQ_DOC = @v3," +
                                                "DT_HR_ENTRADA = @v4, " +
                                                "CD_EMPRESA = @v5," +
                                                "VL_TOT_PIS = @v6," +
                                                "VL_TOT_COFINS = @v7," +
                                                "VL_TOT_CSLL = @v8," +
                                                "VL_TOT_IRRF = @v9," +
                                                "VL_TOT_INSS = @v10," +
                                                "VL_TOT_OUTRAS = @v11," +
                                                "CD_SITUACAO = @v12," +
                                                "NR_DOCUMENTO = @v13," +
                                                "NR_SR_DOCUMENTO = @v14," +
                                                "VL_ALIQ_ISSQN = @v15," +
                                                "CD_MUN_SERV_PRESTADO = @v16," +
                                                "CD_TIPO_DOCUMENTO = @v17," +
                                                "DT_HR_EMISSAO = @v18," +
                                                "VL_TOTAL_GERAL = @v19 ," +
                                                "CD_CHAVE_ACESSO = @v20," +
                                                "NR_PROTOCOLO = @v21," +
                                                "DG_SR_DOCUMENTO = @v22," +
                                                "CD_TIPO_OPERACAO = @v23," +
                                                "CD_CND_PAGAMENTO = @v24," +
                                                "CD_INDEX_INTEGRACAO_PEDIDO = @v25," +
                                                "CD_DOC_ORIGINAL = @v26 Where [CD_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoNotaFiscalServico);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoGeralServico.ToUpper());
                //Cmd.Parameters.AddWithValue("@v3", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v4", p.DataLancamento);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoPrestador);
                Cmd.Parameters.AddWithValue("@v6", p.ValorPIS);
                Cmd.Parameters.AddWithValue("@v7", p.ValorCofins);
                Cmd.Parameters.AddWithValue("@v8", p.ValorCSLL);
                Cmd.Parameters.AddWithValue("@v9", p.ValorIRRF);
                Cmd.Parameters.AddWithValue("@v10", p.ValorINSS);
                Cmd.Parameters.AddWithValue("@v11", p.ValorOutrasRetencoes);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v13", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v14", p.NumeroSerie);
                Cmd.Parameters.AddWithValue("@v15", p.ValorAliquotaISSQN);
                Cmd.Parameters.AddWithValue("@v16", p.CodigoMunicipioPrestado);
                Cmd.Parameters.AddWithValue("@v17", 1);
                Cmd.Parameters.AddWithValue("@v18", p.DataEmissao);
                Cmd.Parameters.AddWithValue("@v19", p.ValorTotalNota);
                Cmd.Parameters.AddWithValue("@v20", "");
                Cmd.Parameters.AddWithValue("@v21", 0);
                Cmd.Parameters.AddWithValue("@v22", p.DGSerieDocumento);
                Cmd.Parameters.AddWithValue("@v23", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v25", p.CodigoIndexIntegraPedido);
                Cmd.Parameters.AddWithValue("@v26", p.CodigoDocumentoOriginal);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar NOTA FISCAL DE SERVIÇO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                AtualizarPessoaDocumento(p.CodigoTomador, p.CodigoNotaFiscalServico);
                InserirServicoDocumento(p.CodigoNotaFiscalServico, ListaTipoServico);
                InserirProdutoDocumento(p.CodigoNotaFiscalServico, listaItemTipoServico);
                if (eventoDocumento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(eventoDocumento, p.CodigoNotaFiscalServico);
                }

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoNotaFiscalServico, ListaAnexoDocumento);

                ParcelaDocumentoDAL ParcelaDAL = new ParcelaDocumentoDAL();
                ParcelaDAL.Inserir(p.CodigoNotaFiscalServico, ListaParcelaDocumento);
            }
        }
        public void Excluir(decimal Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update DOCUMENTO set CD_SITUACAO = 37 where CD_DOCUMENTO = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
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
                            throw new Exception("Erro ao excluir NOTA FISCAL DE SERVICO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir NOTA FISCAL DE SERVICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public Doc_NotaFiscalServico PesquisarNotaFiscalServico(decimal Codigo)
        {
            try
            {
                int CodTomador = PesquisarPessoaDocumento(Codigo);

                AbrirConexao();

                strSQL = "Select * from [VW_DOC_NFS] Where CD_DOCUMENTO= @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();

                Doc_NotaFiscalServico p = new Doc_NotaFiscalServico();

                if (Dr.Read())
                {                   
                    p.CodigoNotaFiscalServico = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.DescricaoGeralServico = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.DataLancamento = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.CodigoPrestador = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.ValorPIS = Convert.ToDecimal(Dr["VL_TOT_PIS"]);
                    p.ValorCofins = Convert.ToDecimal(Dr["VL_TOT_COFINS"]);
                    p.ValorCSLL = Convert.ToDecimal(Dr["VL_TOT_CSLL"]);
                    p.ValorIRRF = Convert.ToDecimal(Dr["VL_TOT_IRRF"]);
                    p.ValorINSS = Convert.ToDecimal(Dr["VL_TOT_INSS"]);
                    p.ValorOutrasRetencoes = Convert.ToDecimal(Dr["VL_TOT_OUTRAS"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.NumeroSerie = Convert.ToDecimal(Dr["NR_SR_DOCUMENTO"]);
                    p.CodigoTomador = CodTomador;
                    p.ValorAliquotaISSQN = Convert.ToDecimal(Dr["VL_ALIQ_ISSQN"]);
                    p.CodigoMunicipioPrestado= Convert.ToDecimal(Dr["CD_MUN_SERV_PRESTADO"]);
                    p.ValorTotalNota = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DataEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.Cpl_NomeTomador = Dr["RAZ_SOCIAL"].ToString();
                    p.ChaveAcesso = Dr["CD_CHAVE_ACESSO"].ToString();
                    p.Protocolo = Convert.ToString(Dr["NR_PROTOCOLO"]);
                    p.DGSerieDocumento = Dr["DG_SR_DOCUMENTO"].ToString();
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoIndexIntegraPedido = Convert.ToDecimal(Dr["CD_INDEX_INTEGRACAO_PEDIDO"]);
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);
                    p.Cpl_DescricaoMunicipio = Dr["DS_MUN_SERV_PRESTADO"].ToString();
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar NOTA FISCAL DE SERVICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_NotaFiscalServico> ListarNotaFiscalServico(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_NFS] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_NotaFiscalServico> lista = new List<Doc_NotaFiscalServico>();

                while (Dr.Read())
                {
                    Doc_NotaFiscalServico p = new Doc_NotaFiscalServico();

                    p.CodigoNotaFiscalServico = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.DescricaoGeralServico = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.DataLancamento = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.CodigoPrestador = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.ValorPIS = Convert.ToDecimal(Dr["VL_TOT_PIS"]);
                    p.ValorCofins = Convert.ToDecimal(Dr["VL_TOT_COFINS"]);
                    p.ValorCSLL = Convert.ToDecimal(Dr["VL_TOT_CSLL"]);
                    p.ValorIRRF = Convert.ToDecimal(Dr["VL_TOT_IRRF"]);
                    p.ValorINSS = Convert.ToDecimal(Dr["VL_TOT_INSS"]);
                    p.ValorOutrasRetencoes = Convert.ToDecimal(Dr["VL_TOT_OUTRAS"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.NumeroSerie = Convert.ToDecimal(Dr["NR_SR_DOCUMENTO"]);
                    p.ValorAliquotaISSQN = Convert.ToDecimal(Dr["VL_ALIQ_ISSQN"]);
                    p.CodigoMunicipioPrestado = Convert.ToDecimal(Dr["CD_MUN_SERV_PRESTADO"]);
                    p.ValorTotalNota = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DataEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.Cpl_NomeTomador = Dr["RAZ_SOCIAL"].ToString();
                    p.Cpl_DsSituacao = Dr["DS_SITUACAO"].ToString();
                    p.DGSerieDocumento = Dr["DG_SR_DOCUMENTO"].ToString();
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoIndexIntegraPedido = Convert.ToDecimal(Dr["CD_INDEX_INTEGRA_PEDIDO"]);
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos NOTAS FISCAIS DE SERVICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_NotaFiscalServico> ListarNotaFiscalServicoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VW_DOC_NFS] ";
                
                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                if (strValor == "")
                    strSQL = strSQL + " WHERE CD_SITUACAO != 37";
                else
                    strSQL = strSQL + " AND CD_SITUACAO != 37";
               

                strSQL = strSQL + "order by CD_DOCUMENTO DESC";
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_NotaFiscalServico> lista = new List<Doc_NotaFiscalServico>();

                while (Dr.Read())
                {
                    Doc_NotaFiscalServico p = new Doc_NotaFiscalServico();
                    p.CodigoNotaFiscalServico = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.DescricaoGeralServico = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.DataLancamento = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.CodigoPrestador = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.ValorPIS = Convert.ToDecimal(Dr["VL_TOT_PIS"]);
                    p.ValorCofins = Convert.ToDecimal(Dr["VL_TOT_COFINS"]);
                    p.ValorCSLL = Convert.ToDecimal(Dr["VL_TOT_CSLL"]);
                    p.ValorIRRF = Convert.ToDecimal(Dr["VL_TOT_IRRF"]);
                    p.ValorINSS = Convert.ToDecimal(Dr["VL_TOT_INSS"]);
                    p.ValorOutrasRetencoes = Convert.ToDecimal(Dr["VL_TOT_OUTRAS"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NumeroDocumento = Convert.ToDecimal(Dr["NR_DOCUMENTO"]);
                    p.NumeroSerie = Convert.ToDecimal(Dr["NR_SR_DOCUMENTO"]);
                    p.ValorAliquotaISSQN = Convert.ToDecimal(Dr["VL_ALIQ_ISSQN"]);
                    p.CodigoMunicipioPrestado = Convert.ToDecimal(Dr["CD_MUN_SERV_PRESTADO"]);
                    p.ValorTotalNota = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DataEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.Cpl_NomeTomador = Dr["RAZ_SOCIAL"].ToString();
                    p.Cpl_DsSituacao = Dr["DS_SITUACAO"].ToString();
                    p.DGSerieDocumento = Dr["DG_SR_DOCUMENTO"].ToString();
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoIndexIntegraPedido = Convert.ToDecimal(Dr["CD_INDEX_INTEGRACAO_PEDIDO"]);
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);

                    p.BtnImprimir = true;
                    if(p.CodigoSituacao == 42)//DIGITACAO
                    {
                        p.BtnAutorizar = true;
                        p.BtnCancelar = false;
                        p.BtnImprimir = false;
                    }
                    else if(p.CodigoSituacao == 41)//CANCELADO
                    {
                        p.BtnAutorizar = false;
                        p.BtnCancelar = false;
                    }
                    else if (p.CodigoSituacao == 39)//REJEITADO
                    {
                        p.BtnAutorizar = true;
                        p.BtnCancelar = false;
                    }
                    else//40 - AUTORIZADO
                    {
                        p.BtnAutorizar = false;
                        p.BtnCancelar = true;
                    }

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar TODAS NOTAS FISCAIS DE SERVICO: " + ex.Message.ToString());
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
        public bool InserirPessoaDocumento(int p, decimal doc)
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

                strCamposPessoa += ", EMAIL_NFSE";
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

                Cmd.Parameters.AddWithValue("@v25", doc);
                Cmd.Parameters.AddWithValue("@v26", 3);
                Cmd.Parameters.AddWithValue("@v27", p);
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
                Cmd.Parameters.AddWithValue("@v41", pesCtt._MailNFSE);
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
                throw new Exception("Erro ao gravar Pessoa do documento: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
        public bool AtualizarPessoaDocumento(int p, decimal doc)
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
                                                        "EMAIL_NFSE = @v41 where CD_DOCUMENTO = @v25";
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

                Cmd.Parameters.AddWithValue("@v25", doc);
                Cmd.Parameters.AddWithValue("@v26", 3);
                Cmd.Parameters.AddWithValue("@v27", p);
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
                Cmd.Parameters.AddWithValue("@v41", pesCtt._MailNFSE);

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
                            throw new Exception("Erro ao Incluir conta a receber: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar conta a receber " + ex.Message.ToString());

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
        public List<ItemTipoServico> ObterProdutoDocumento(Decimal CodDocumento)
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
        public List<TipoServico> ObterTipoServico(Decimal CodDocumento)
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
        public void GerarLog(Doc_NotaFiscalServico p1, Doc_NotaFiscalServico p2)
        {
            Habil_LogDAL logDAL = new Habil_LogDAL();
            DBTabelaDAL db = new DBTabelaDAL();
            long CodIdent = Convert.ToInt64(p1.CodigoNotaFiscalServico);
            int CodOperacao = 6;

            
            if (p1.CodigoTomador != p2.CodigoTomador)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("PESSOA_DO_DOCUMENTO", "CD_PESSOA");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoTomador+ " para: " + p1.CodigoTomador;
                logDAL.Inserir(log);
            }
            if (p1.CodigoCondicaoPagamento != p2.CodigoCondicaoPagamento)
            {
                if (p2.CodigoCondicaoPagamento != 0)
                {

                    CondPagamento cond = new CondPagamento();
                    CondPagamentoDAL condDAL = new CondPagamentoDAL();
                    cond = condDAL.PesquisarCondPagamento(p1.CodigoCondicaoPagamento);

                    CondPagamento cond2 = new CondPagamento();

                    cond2 = condDAL.PesquisarCondPagamento(p2.CodigoCondicaoPagamento);
                    Habil_Log log = new Habil_Log();

                    log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_CND_PAGAMENTO");
                    log.CodigoEstacao = p1.Cpl_Maquina;
                    log.CodigoIdentificador = CodIdent;
                    log.CodigoOperacao = CodOperacao;
                    log.CodigoUsuario = p1.Cpl_Usuario;
                    log.DescricaoLog = "de: " + cond2.DescricaoCondPagamento + " para: " + cond.DescricaoCondPagamento;
                    logDAL.Inserir(log);
                }
            }
            if (p1.CodigoTipoOperacao != p2.CodigoTipoOperacao)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_TIPO_OPERACAO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoTipoOperacao + " para: " + p1.CodigoTipoOperacao;
                logDAL.Inserir(log);
            }
            if (p1.ValorAliquotaISSQN != p2.ValorAliquotaISSQN)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_ALIQ_ISSQN");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorAliquotaISSQN + " para: " + p1.ValorAliquotaISSQN;
                logDAL.Inserir(log);
            }
            if (p1.ValorTotalNota != p2.ValorTotalNota)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOTAL_GERAL"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorTotalNota + " para: " + p1.ValorTotalNota;
                logDAL.Inserir(log);
            }
            if (p1.ValorPIS != p2.ValorPIS)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOT_PIS"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de : " + p2.ValorPIS + " para: " + p1.ValorPIS;
                logDAL.Inserir(log);
            }
            if (p1.ValorCofins != p2.ValorCofins)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOT_COFINS"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorCofins + " para: " + p1.ValorCofins;
                logDAL.Inserir(log);
            }
            if (p1.ValorCSLL != p2.ValorCSLL)
            {
                Habil_Log log = new Habil_Log();
                ;
                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOT_CSLL"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorCSLL + " para: " + p1.ValorCSLL;
                logDAL.Inserir(log);
            }
            if (p1.ValorIRRF != p2.ValorIRRF)
            {
                Habil_Log log = new Habil_Log();
                ;
                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOT_IRRF"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorIRRF + " para: " + p1.ValorIRRF;
                logDAL.Inserir(log);
            }
            if (p1.ValorINSS != p2.ValorINSS)
            {
                Habil_Log log = new Habil_Log();
                ;
                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOT_INSS"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorINSS + " para: " + p1.ValorINSS;
                logDAL.Inserir(log);
            }
            if (p1.ValorOutrasRetencoes != p2.ValorOutrasRetencoes)
            {
                Habil_Log log = new Habil_Log();
                ;
                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOT_OUTRAS"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorOutrasRetencoes + " para: " + p1.ValorOutrasRetencoes;
                logDAL.Inserir(log);
            }
            if (p1.CodigoPrestador != p2.CodigoPrestador)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_EMPRESA"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoPrestador + " para: " + p1.CodigoPrestador;
                logDAL.Inserir(log);
            }
            if (p1.DescricaoGeralServico != p2.DescricaoGeralServico)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "OB_DOCUMENTO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DescricaoGeralServico + " para: " + p1.DescricaoGeralServico;
                logDAL.Inserir(log);
            }
            if (p1.CodigoMunicipioPrestado != p2.CodigoMunicipioPrestado)
            {
                Municipio mun = new Municipio();
                MunicipioDAL munDAL = new MunicipioDAL();
                mun = munDAL.PesquisarMunicipio(Convert.ToInt64(p1.CodigoMunicipioPrestado));

                Municipio mun2 = new Municipio();
                MunicipioDAL munDAL2 = new MunicipioDAL();
                mun2 = munDAL2.PesquisarMunicipio(Convert.ToInt64(p2.CodigoMunicipioPrestado));


                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_MUN_SERV_PRESTADO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + mun2.DescricaoMunicipio+ " para: " + mun.DescricaoMunicipio;
                logDAL.Inserir(log);
            }
        }
        public void AtualizarDocumento(decimal CodigoDocumento, decimal CodigoIntegraPedido)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_INDEX_REFERENCIAL = @v2," +
                                             " Where [CD_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", CodigoIntegraPedido);               
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar integracao pedido: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void AtualizarChaveAcesso(Doc_NotaFiscalServico p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_CHAVE_ACESSO = @v2," +
                                                "NR_PROTOCOLO = @v3" +
                                             " Where [CD_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoNotaFiscalServico);
                Cmd.Parameters.AddWithValue("@v2", p.ChaveAcesso);
                Cmd.Parameters.AddWithValue("@v3", p.Protocolo);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar NOTA FISCAL DE SERVIÇO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
    }
}
