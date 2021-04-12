using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Persistence
{
    public class Doc_PedidoDAL : Conexao
    {
        protected string strSQL = "";
        public void Inserir(Doc_Pedido p, List<ProdutoDocumento> listaItemOrcamento, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento, ref decimal CodigoDocumentoPedido, ref List<LiberacaoDocumento> ListaBloqueios, bool ContinuarAberto,bool CriticarBloqueios, bool FaturarPedido)
        {
            try
            {
                AbrirConexao();
                strSQL = "insert into DOCUMENTO (CD_TIPO_DOCUMENTO," +
                                                "CD_EMPRESA," +
                                                "DT_HR_EMISSAO," +
                                                "DT_HR_ENTRADA," +
                                                "NR_DOCUMENTO," +
                                                "DG_DOCUMENTO," +
                                                "DG_SR_DOCUMENTO," +
                                                "VL_TOTAL_GERAL," +
                                                "OB_DOCUMENTO," +
                                                "CD_TIPO_COBRANCA," +
                                                "CD_CND_PAGAMENTO," +
                                                "DT_VENCIMENTO," +
                                                "CD_SITUACAO," +
                                                "CD_CLASSIFICACAO," +
                                                "CD_GER_SEQ_DOC," +
                                                "CD_VENDEDOR," +
                                                "VL_ST," +
                                                "VL_COMISSAO," +
                                                "VL_CUBAGEM," +
                                                "VL_PESO," +
                                                "VL_DESCONTO_MEDIO," +
                                                "VL_FRETE," +
                                                "NR_WEB," +
                                                "CD_APLICACAO_USO," +
                                                "CD_DOC_ORIGINAL," +
                                                "CD_TIPO_OPERACAO) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26) SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                decimal CodigoGerado = 0;
                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                if (p.Cpl_NomeTabela != null)
                    CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", 8);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v4", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v5", CodigoGerado);
                Cmd.Parameters.AddWithValue("@v6", p.DGNumeroDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.DGSerieDocumento);
                Cmd.Parameters.AddWithValue("@v8", p.ValorTotal);
                Cmd.Parameters.AddWithValue("@v9", p.DescricaoDocumento);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v12", p.DataValidade);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v14", p.CodigoTipoOrcamento);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v16", p.CodigoVendedor);
                Cmd.Parameters.AddWithValue("@v17", p.ValorST);
                Cmd.Parameters.AddWithValue("@v18", p.ValorComissao);
                Cmd.Parameters.AddWithValue("@v19", p.ValorCubagem);
                Cmd.Parameters.AddWithValue("@v20", p.ValorPeso);
                Cmd.Parameters.AddWithValue("@v21", p.ValorDescontoMedio);
                Cmd.Parameters.AddWithValue("@v22", p.ValorFrete);
                Cmd.Parameters.AddWithValue("@v23", p.NumeroWeb);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoAplicacaoUso);
                Cmd.Parameters.AddWithValue("@v25", p.CodigoDocumentoOriginal);
                Cmd.Parameters.AddWithValue("@v26", p.CodigoTipoOperacao);

                p.CodigoDocumento = Convert.ToDecimal(Cmd.ExecuteScalar());

                CodigoDocumentoPedido = p.CodigoDocumento;

                
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
                            throw new Exception("Erro ao Incluir Orcamento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTransportador, 13);//Pessoa do Documento Transportador
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoPessoa, 12);//Pessoa do Documento 

                if (eventoDocumento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
                }
                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);

                ProdutoDocumentoDAL ItemDAL = new ProdutoDocumentoDAL();
                ItemDAL.Inserir(p.CodigoDocumento, listaItemOrcamento);

                if (!FaturarPedido)
                {
                    if (!ContinuarAberto)
                    {
                        if (CriticarBloqueios)
                            ListaBloqueios = VerificarBloqueiosPedido(p);

                        if (ListaBloqueios.Count > 0)
                            AtualizarSituacaoPedido(p, 145, p.CodigoSituacao);
                        else
                            AtualizarSituacaoPedido(p, 146, p.CodigoSituacao);
                    }
                }
                else
                {
                    AtualizarSituacaoPedido(p, 150, p.CodigoSituacao);
                }

        }
        }
        public void Atualizar(Doc_Pedido p, List<ProdutoDocumento> listaItemOrcamento, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento, ref List<LiberacaoDocumento> ListaBloqueios, bool ContinuarAberto, bool CriticarBloqueios, bool FaturarPedido)
        {
            try
            {
                Doc_Pedido p2 = new Doc_Pedido();
                p2 = PesquisarDocumento(Convert.ToInt32(p.CodigoDocumento));
                GerarLog(p, p2);
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_TIPO_DOCUMENTO = @v1," +
                                                "CD_EMPRESA = @v2," +
                                                "DT_HR_EMISSAO = @v3," +
                                                "DG_DOCUMENTO = @v5," +
                                                "DG_SR_DOCUMENTO = @v6," +
                                                "VL_TOTAL_GERAL = @v7," +
                                                "OB_DOCUMENTO = @v8," +
                                                "CD_TIPO_COBRANCA = @v9," +
                                                "CD_CND_PAGAMENTO = @v10," +
                                                "DT_VENCIMENTO = @v11," +
                                                "CD_SITUACAO = @v12," +
                                                "CD_CLASSIFICACAO = @v13," +
                                                //"CD_GER_SEQ_DOC = @v14," +
                                                "CD_VENDEDOR = @v15," +
                                                "VL_ST = @v16," +
                                                "VL_COMISSAO = @v17," +
                                                "VL_CUBAGEM = @v18," +
                                                "VL_PESO = @v19," +
                                                "VL_DESCONTO_MEDIO = @v20," +
                                                "VL_FRETE = @v21," +
                                                "NR_WEB = @v22," +
                                                "CD_APLICACAO_USO = @v23," +
                                                "CD_TIPO_OPERACAO = @v24  Where [CD_DOCUMENTO] = @CODIGO";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@CODIGO", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v1", 8);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v5", p.DGNumeroDocumento);
                Cmd.Parameters.AddWithValue("@v6", p.DGSerieDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.ValorTotal);
                Cmd.Parameters.AddWithValue("@v8", p.DescricaoDocumento);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v11", p.DataValidade);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoTipoOrcamento);
                //Cmd.Parameters.AddWithValue("@v14", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoVendedor);
                Cmd.Parameters.AddWithValue("@v16", p.ValorST);
                Cmd.Parameters.AddWithValue("@v17", p.ValorComissao);
                Cmd.Parameters.AddWithValue("@v18", p.ValorCubagem);
                Cmd.Parameters.AddWithValue("@v19", p.ValorPeso);
                Cmd.Parameters.AddWithValue("@v20", p.ValorDescontoMedio);
                Cmd.Parameters.AddWithValue("@v21", p.ValorFrete);
                Cmd.Parameters.AddWithValue("@v22", p.NumeroWeb);
                Cmd.Parameters.AddWithValue("@v23", p.CodigoAplicacaoUso);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoTipoOperacao);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoPessoa, 12);
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTransportador, 13);

                if (eventoDocumento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
                }

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);

                ProdutoDocumentoDAL ItemDAL = new ProdutoDocumentoDAL();
                ItemDAL.Inserir(p.CodigoDocumento, listaItemOrcamento);
                if (!FaturarPedido)
                {
                    if (!ContinuarAberto)
                    {
                        if (CriticarBloqueios)
                            ListaBloqueios = VerificarBloqueiosPedido(p);

                        if (ListaBloqueios.Count > 0)
                            AtualizarSituacaoPedido(p, 145, p.CodigoSituacao);
                        else
                            AtualizarSituacaoPedido(p, 146, p.CodigoSituacao);
                    }
                }
                else
                {
                    AtualizarSituacaoPedido(p, 150, p.CodigoSituacao);
                }
            }
        }
        public DataTable RelPedido(decimal CodigoDocumento)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "SELECT " +
                                    "DOC.* ,E_END.DS_BAIRRO AS BAIRRO_EMPRESA, " +
                                    "ISNULL(E_END.DS_MUNICIPIO, '') AS MUNICIPIO_EMPRESA, " +
                                    "ISNULL(E_END.TX_LOGRADOURO, '') AS LOGRADOURO_EMPRESA, " +
                                    "ISNULL(ESTADO.SIGLA, '') AS ESTADO_EMPRESA, " +
                                    "ISNULL(E_END.NR_ENDERECO, '') AS NR_ENDERECO_EMPRESA, " +
                                    "ISNULL(E_INS.NR_INSCRICAO, '') AS CNPJ_EMPRESA, " +
                                    "ISNULL(E_INS.NR_IERG, '') AS IE_EMPRESA, " +
                                    "ISNULL(E_END.CD_CEP, '') AS CEP_EMPRESA, " +
                                    "ISNULL(PDOC.TELEFONE_1, '') AS TELEFONE_CLIENTE, " +
                                    "ISNULL(PDOC.EMAIL, '') AS EMAIL_CLIENTE, " +
                                    "ISNULL(PDOC.EMAIL_NFE, '') AS EMAIL_NF_CLIENTE, " +
                                    "ISNULL(P.NM_PESSOA, '') AS NOME_EMPRESA, " +
                                    "ISNULL(CTT_VENDEDOR.NR_FONE1, '') AS TELEFONE_VENDEDOR, " +
                                    "ISNULL(CTT_VENDEDOR.TX_MAIL1, '') AS EMAIL_VENDEDOR , " +
                                    "ISNULL(TP_OP.DS_TIPO_OPERACAO, '') AS DS_TIPO_OPERACAO " +
                                "FROM " +
                                    "VW_DOC_PEDIDO AS DOC " +
                                    "LEFT JOIN EMPRESA AS E ON E.CD_EMPRESA = DOC.CD_EMPRESA " +
                                    "LEFT JOIN PESSOA AS P ON P.CD_PESSOA = E.CD_PESSOA " +
                                    "LEFT JOIN PESSOA_ENDERECO AS E_END ON E_END.CD_PESSOA = E.CD_PESSOA AND E_END.TP_ENDERECO = 5 " +
                                    "LEFT JOIN PESSOA_INSCRICAO AS E_INS ON E_INS.CD_PESSOA = E.CD_PESSOA AND E_INS.TP_INSCRICAO = 3 " +
                                    "LEFT JOIN PESSOA_DO_DOCUMENTO AS PDOC ON PDOC.CD_DOCUMENTO = DOC.CD_DOCUMENTO AND PDOC.TP_PESSOA = 12 " +
                                    "LEFT JOIN ESTADO ON ESTADO.CD_ESTADO = E_END.CD_ESTADO " +
                                    "LEFT JOIN VENDEDOR ON VENDEDOR.CD_VENDEDOR = DOC.CD_VENDEDOR " +
                                    "LEFT JOIN PESSOA_CONTATO AS CTT_VENDEDOR ON CTT_VENDEDOR.CD_PESSOA = VENDEDOR.CD_PESSOA " +
                                    "LEFT JOIN TIPO_DE_OPERACAO AS TP_OP ON TP_OP.CD_TIPO_OPERACAO = DOC.CD_TIPO_OPERACAO " +
                                "WHERE DOC.CD_DOCUMENTO = @v1";

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório de pedido: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public void AtualizarSituacaoPedido(Doc_Pedido doc, int CodigoSituacao, int CodigoSituacaoAnterior)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_SITUACAO = @v1" +
                                                " Where [CD_DOCUMENTO] = @CODIGO";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@CODIGO", doc.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v1", CodigoSituacao);

                Cmd.ExecuteNonQuery();

                if(CodigoSituacao != CodigoSituacaoAnterior)
                {
                    EventoDocumento(doc, CodigoSituacao);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar situação documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void AtualizaOrcamento(decimal CodigoDocumentoOrcamento, int CodigoUsuario, int CodigoMaquina)
        {
            try
            {

                AbrirConexao();
                strSQL = " SELECT " +
                             "ISNULL(( " +
                                "SELECT " +
                                    "SUM(PROD_PED.QT_SOLICITADA) " +
                                "FROM " +
                                    "PRODUTO_DO_DOCUMENTO AS PROD_PED INNER JOIN VW_DOC_PEDIDO AS DOC_PED ON DOC_PED.CD_DOCUMENTO = PROD_PED.CD_DOCUMENTO AND DOC_PED.CD_SITUACAO != 158 " +
                                "WHERE " +
                                    "PROD_PED.CD_PRODUTO = PROD_ORC.CD_PRODUTO AND DOC_PED.CD_DOC_ORIGINAL = PROD_ORC.CD_DOCUMENTO AND PROD_PED.CD_SITUACAO != 134),0) AS QT_SOLICITADA_PEDIDO, " +
                            "ISNULL(PROD_ORC.QT_SOLICITADA, 0) AS QT_SOLICITADA_ORCAMENTO, " +
                             "PROD_ORC.CD_PROD_DOCUMENTO,  " +
                            "ISNULL(DOC_ORC.CD_DOCUMENTO, 0) as CD_DOC_ORIGINAL, ISNULL(DOC_ORC.CD_SITUACAO, 0) AS CD_SITUACAO_ORC, " +
                              "ISNULL(PROD_ORC.CD_PRODUTO, 0) AS CD_PRODUTO " +
                         "FROM " +
                            "PRODUTO_DO_DOCUMENTO AS PROD_ORC " +
                            "INNER JOIN VW_DOC_ORCAMENTO AS DOC_ORC ON DOC_ORC.CD_DOCUMENTO = PROD_ORC.CD_DOCUMENTO " +
                         "WHERE " +
                            "PROD_ORC.CD_DOCUMENTO = @v1 AND PROD_ORC.CD_SITUACAO != 134";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumentoOrcamento);
                Dr = Cmd.ExecuteReader();

                ProdutoDocumentoDAL prodDAL = new ProdutoDocumentoDAL();

                decimal QuantidadePedido = 0;
                decimal QuantidadeOrcamento = 0;
                decimal CodigoDocumentoOriginal = 0;
                decimal CodigoItem = 0;
                int CodigoSituacaoAnteriorOrcamento = 0;
                int Contador = 0;
                while (Dr.Read())
                {
                    QuantidadePedido = Convert.ToDecimal(Dr["QT_SOLICITADA_PEDIDO"]);
                    QuantidadeOrcamento= Convert.ToDecimal(Dr["QT_SOLICITADA_ORCAMENTO"]);
                    CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);
                    CodigoItem = Convert.ToDecimal(Dr["CD_PROD_DOCUMENTO"]);
                    CodigoSituacaoAnteriorOrcamento = Convert.ToInt32(Dr["CD_SITUACAO_ORC"].ToString());

                    if (QuantidadePedido < QuantidadeOrcamento )
                    {
                        if(QuantidadePedido == 0)
                            prodDAL.AtualizarItem(CodigoDocumentoOriginal, CodigoItem, QuantidadePedido, 135);
                        else
                            prodDAL.AtualizarItem(CodigoDocumentoOriginal, CodigoItem, QuantidadePedido,133);
                        Contador++;
                    }
                    else
                    {
                        prodDAL.AtualizarItem(CodigoDocumentoOriginal, CodigoItem, QuantidadePedido, 140);
                    }
                }
                Doc_OrcamentoDAL orcDAL = new Doc_OrcamentoDAL();

                if(CodigoDocumentoOriginal != 0 && CodigoSituacaoAnteriorOrcamento != 0 && CodigoSituacaoAnteriorOrcamento != 144 && CodigoSituacaoAnteriorOrcamento != 138)
                    if (Contador > 0)
                        orcDAL.AtualizarSituacao(CodigoDocumentoOriginal, 136, CodigoSituacaoAnteriorOrcamento,CodigoUsuario,CodigoMaquina);
                    else
                        orcDAL.AtualizarSituacao(CodigoDocumentoOriginal, 137, CodigoSituacaoAnteriorOrcamento, CodigoUsuario, CodigoMaquina);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro AO ATUALIZAR ORCAMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_Pedido> ListarPedidoPessoa(long CodigoPessoa, decimal CodigoDocumentoAtual)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_PEDIDO] where CD_PESSOA = @v1 AND CD_DOCUMENTO != @v2 AND CD_SITUACAO != 139 ORDER BY CD_DOCUMENTO DESC";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v2", CodigoDocumentoAtual);
                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido> lista = new List<Doc_Pedido>();

                while (Dr.Read())
                {
                    Doc_Pedido p = new Doc_Pedido();

                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.ValorST = Convert.ToDecimal(Dr["VL_ST"]);
                    p.ValorComissao = Convert.ToDecimal(Dr["VL_COMISSAO"]);
                    p.ValorCubagem = Convert.ToDecimal(Dr["VL_CUBAGEM"]);
                    p.ValorDescontoMedio = Convert.ToDecimal(Dr["VL_DESCONTO_MEDIO"]);
                    p.ValorFrete = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeVendedor = Dr["NOME_VENDEDOR"].ToString();
                    p.Cpl_DsCondicaoPagamento = Dr["DS_CND_PAGAMENTO"].ToString();
                    p.Cpl_DsAplicacaoUso = Dr["DS_APLICACAO_USO"].ToString();
                    p.Cpl_DsTipoCobranca = Dr["DS_TIPO_COBRANCA"].ToString();
                    p.Cpl_DsTipoOrcamento = Dr["DS_TIPO_COBRANCA"].ToString();
                    p.Cpl_NomeTransportador = Dr["RAZ_SOCIAL_TRANSPORTADOR"].ToString();
                    p.Cpl_DsTipoOperacao = Dr["DS_TIPO_OPERACAO"].ToString();

                    lista.Add(p);
                }
                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Orcamentos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void CancelarPedido(decimal Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update DOCUMENTO set CD_SITUACAO = 158 where CD_DOCUMENTO = @v1", Con);
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
                            throw new InvalidOperationException("Cancelamento não Permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao cancelar documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Cancelar documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public Doc_Pedido PesquisarDocumento(decimal Codigo)
        {
            try
            {
                long CodTransportador = PesquisarPessoaDocumento(Codigo, 13);
                long CodCliente = PesquisarPessoaDocumento(Codigo,12);

                AbrirConexao();
                strSQL = "Select * from [VW_DOC_PEDIDO] Where CD_DOCUMENTO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Doc_Pedido p = null;

                if (Dr.Read())
                {
                    p = new Doc_Pedido();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.ValorST = Convert.ToDecimal(Dr["VL_ST"]);
                    p.ValorComissao = Convert.ToDecimal(Dr["VL_COMISSAO"]);
                    p.ValorCubagem = Convert.ToDecimal(Dr["VL_CUBAGEM"]);
                    p.ValorDescontoMedio = Convert.ToDecimal(Dr["VL_DESCONTO_MEDIO"]);
                    p.ValorFrete = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.Cpl_DsTipoOperacao = Dr["DS_TIPO_OPERACAO"].ToString();

                    if (Dr["CD_DOC_ORIGINAL"] != DBNull.Value)
                        p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);
                    else
                        p.CodigoDocumentoOriginal = 0;

                    p.Cpl_CodigoTransportador = CodTransportador;
                    p.Cpl_CodigoPessoa = CodCliente;
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_CtaReceber> ListarContasAbertasCliente(decimal CodigoPessoa)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT * FROM VW_DOC_CTA_RECEBER WHERE CD_SITUACAO = 31 AND CD_PESSOA = @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoPessoa);

                Dr = Cmd.ExecuteReader();

                List<Doc_CtaReceber> lista = new List<Doc_CtaReceber>();

                while (Dr.Read())
                {
                    Doc_CtaReceber p = new Doc_CtaReceber();

                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.DGDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataEntrada = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.DataVencimento = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoVendedor = Convert.ToInt16(Dr["CD_PESSOA"]);
                    p.Cpl_DsSituacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeFornecedor = Dr["RAZ_SOCIAL"].ToString();
                    p.ValorDocumento = Convert.ToDecimal(Dr["VL_TOTAL_DOCUMENTO"]);
                    p.ValorDesconto = Convert.ToDecimal(Dr["VL_TOTAL_DESCONTO"]);
                    p.ValorAcrescimo = Convert.ToDecimal(Dr["VL_TOTAL_ACRESCIMO"]);
                    p.ValorGeral = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.Cpl_vlPago = Convert.ToDecimal(Dr["VL_PAGO"]);
                    p.Cpl_vlPagar = p.ValorGeral - p.Cpl_vlPago;
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar CONTAS A RECEBER DO documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_Pedido> ListarDocumentos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_PEDIDO] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido> lista = new List<Doc_Pedido>();

                while (Dr.Read())
                {
                    Doc_Pedido p = new Doc_Pedido();

                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.ValorST = Convert.ToDecimal(Dr["VL_ST"]);
                    p.ValorComissao = Convert.ToDecimal(Dr["VL_COMISSAO"]);
                    p.ValorCubagem = Convert.ToDecimal(Dr["VL_CUBAGEM"]);
                    p.ValorDescontoMedio = Convert.ToDecimal(Dr["VL_DESCONTO_MEDIO"]);
                    p.ValorFrete = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.Cpl_DsTipoOperacao = Dr["DS_TIPO_OPERACAO"].ToString();

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos documentos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Doc_Pedido> ListarDocumentosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VW_DOC_PEDIDO] ";


                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido> lista = new List<Doc_Pedido>();

                while (Dr.Read())
                {
                    Doc_Pedido p = new Doc_Pedido();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.ValorST = Convert.ToDecimal(Dr["VL_ST"]);
                    p.ValorComissao = Convert.ToDecimal(Dr["VL_COMISSAO"]);
                    p.ValorCubagem = Convert.ToDecimal(Dr["VL_CUBAGEM"]);
                    p.ValorDescontoMedio = Convert.ToDecimal(Dr["VL_DESCONTO_MEDIO"]);
                    p.ValorFrete = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeVendedor = Dr["NOME_VENDEDOR"].ToString();
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.Cpl_DsTipoOperacao = Dr["DS_TIPO_OPERACAO"].ToString();

                    if (p.CodigoSituacao != 155)
                        p.PodeImprimir = true;

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar TODos documentos: " + ex.Message.ToString());
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

                strCamposPessoa += ", EMAIL_NFSE";
                strValoresPessoa += ", @v41";

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
                            throw new Exception("Erro ao Incluir pessoa do documento: " + ex.Message.ToString());

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
                                                        "DS_BAIRRO = @v40," +
                                                        "EMAIL_NFSE = @v41 where CD_DOCUMENTO = @v25 AND TP_PESSOA = @v26";
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
                            throw new Exception("Erro ao Incluir PESSOA DO DOCUMENTO: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar PESSOA DO DOCUMENTO" + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
        public void GerarLog(Doc_Pedido p1, Doc_Pedido p2)
        {
            Habil_LogDAL logDAL = new Habil_LogDAL();
            DBTabelaDAL db = new DBTabelaDAL();
            long CodIdent = Convert.ToInt64(p1.CodigoDocumento);
            int CodOperacao = 6;

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
            if (p1.CodigoCondicaoPagamento != p2.CodigoCondicaoPagamento)
            {
                if (p2.CodigoCondicaoPagamento != 0)
                {

                    Habil_Log log = new Habil_Log();

                    log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("CONDICAO_DE_PAGAMENTO", "CD_CND_PAGAMENTO");
                    log.CodigoEstacao = p1.Cpl_Maquina;
                    log.CodigoIdentificador = CodIdent;
                    log.CodigoOperacao = CodOperacao;
                    log.CodigoUsuario = p1.Cpl_Usuario;
                    log.DescricaoLog = "de: " + p2.CodigoCondicaoPagamento + " para: " + p1.CodigoCondicaoPagamento;
                    logDAL.Inserir(log);
                }
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
            if (p1.DataValidade != p2.DataValidade)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DT_VENCIMENTO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DataValidade.ToString("dd/MM/yyyy") + " para: " + p1.DataValidade.ToString("dd/MM/yyyy");
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
            if (p1.DGNumeroDocumento != p2.DGNumeroDocumento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DG_DOCUMENTO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DGNumeroDocumento + " para: " + p1.DGNumeroDocumento;

                logDAL.Inserir(log);
            }
            if (p1.NumeroWeb != p2.NumeroWeb)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "NR_WEB");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.NumeroWeb + " para: " + p1.NumeroWeb;

                logDAL.Inserir(log);
            }
            if (p1.ValorST != p2.ValorST)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_ST");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorST + " para: " + p1.ValorST;
            }
            if (p1.ValorPeso != p2.ValorPeso)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_PESO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorPeso + " para: " + p1.ValorPeso;
            }
            if (p1.ValorFrete != p2.ValorFrete)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_FRETE");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorFrete + " para: " + p1.ValorFrete;
            }
            if (p1.ValorCubagem != p2.ValorCubagem)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_CUBAGEM" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorCubagem + " para: " + p1.ValorCubagem;
            }
            if (p1.ValorDescontoMedio != p2.ValorDescontoMedio)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_DESCONTO_MEDIO" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorDescontoMedio + " para: " + p1.ValorDescontoMedio;
            }
            if (p1.ValorComissao != p2.ValorComissao)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_COMISSAO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorComissao + " para: " + p1.ValorComissao;
            }
            if (p1.CodigoTipoOrcamento != p2.CodigoTipoOrcamento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_TIPO_ORCAMENTO" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoTipoOrcamento + " para: " + p1.CodigoTipoOrcamento;
            }
            if (p1.CodigoTipoCobranca != p2.CodigoTipoCobranca)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_TIPO_COBRANCA" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoTipoCobranca + " para: " + p1.CodigoTipoCobranca;
            }
            if (p1.Cpl_CodigoTransportador != p2.Cpl_CodigoTransportador)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_PESSOA_TRANSPORTADOR" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.Cpl_CodigoTransportador + " para: " + p1.Cpl_CodigoTransportador;
            }
            if (p1.CodigoTipoOperacao != p2.CodigoTipoOperacao)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_TIPO_OPERACAO" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoTipoOperacao + " para: " + p1.CodigoTipoOperacao;
            }
            if (p1.CodigoVendedor != p2.CodigoVendedor)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "NOME_VENDEDOR" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoVendedor + " para: " + p1.CodigoVendedor;
            }
            if (p1.CodigoAplicacaoUso != p2.CodigoAplicacaoUso)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_APLICACAO_USO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoAplicacaoUso + " para: " + p1.CodigoAplicacaoUso;
            }
        }
        public List<LiberacaoDocumento> VerificarBloqueiosPedido(Doc_Pedido doc)
        {
            try
            {

                AbrirConexao();
                strSQL = "SELECT " + 
                           "ISNULL(( " +
                                "SELECT " +
                                    "SUM(CT_REC_A_VENCER.VL_TOTAL_GERAL - CT_REC_A_VENCER.VL_PAGO) " +
                                "FROM " +
                                    "VW_DOC_CTA_RECEBER AS CT_REC_A_VENCER " +
                                "WHERE " +
                                    "CT_REC_A_VENCER.CD_PESSOA = P.CD_PESSOA AND CT_REC_A_VENCER.DT_VENCIMENTO > GETDATE()) " +
	                        ",0) " +
                                "AS VL_CONTAS_A_VENCER, " +
                            "ISNULL((" +
                                "SELECT " +
                                    "SUM(CT_REC_VENCIDA.VL_TOTAL_GERAL - CT_REC_VENCIDA.VL_PAGO) " +
                                "FROM " +
                                    "VW_DOC_CTA_RECEBER AS CT_REC_VENCIDA " +
                                "WHERE " +
                                    "CT_REC_VENCIDA.CD_PESSOA = P.CD_PESSOA AND CT_REC_VENCIDA.DT_VENCIMENTO <= GETDATE()) " +
	                        ",0) AS VL_CONTAS_VENCIDAS, " +
                            "ISNULL(( " +
                                "SELECT " +
                                    "SUM(VL_TOTAL_GERAL) " +
                                "FROM " +
                                    "VW_DOC_PEDIDO " +
                                "WHERE " +
                            "CD_PESSOA = @v1 AND CD_SITUACAO != 136 AND CD_SITUACAO != 145 AND CD_SITUACAO != 150 ) " +
	                        ",0) AS VL_PEDIDOS_EM_ANDAMENTO, " +
                            "ISNULL(( " +
                                    "SELECT "+
                                        "COUNT(*) "+
                                    "FROM "+
                                        "VW_DOC_PEDIDO "+
                                    "WHERE "+
                                "CD_PESSOA = @v1 AND CD_SITUACAO = 158 ) " +
	                            ",0) AS QT_PEDIDOS_FATURADOS, " +
                            "ISNULL(P.VL_LIMITE_CREDITO, 0) AS VL_LIMITE_CREDITO_CLIENTE, " +
                            "ISNULL(PSIS.VL_FRETE_MINIMO, 0) AS VL_FRETE_MINIMO, " +
                            "ISNULL(PSIS.VL_PEDIDO_PARA_FRETE_MINIMO, 0) AS VL_PEDIDO_PARA_FRETE_MINIMO " +
                        "FROM " +
                            "PESSOA AS P " +
                            "LEFT JOIN " +
                                "PARAMETROS_DO_SISTEMA AS PSIS ON PSIS.CD_EMPRESA = @v2 " +
                        "WHERE " +
                            "P.CD_PESSOA =  @v1 ";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", doc.Cpl_CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v2", doc.CodigoEmpresa);
                Dr = Cmd.ExecuteReader();

                List<LiberacaoDocumento> lista = new List<LiberacaoDocumento>();
                LiberacaoDocumentoDAL libDAL = new LiberacaoDocumentoDAL();

                decimal ValorPedidosEmAndamento = 0;
                decimal ContasVencidas = 0;
                decimal ContasAVencer = 0;
                decimal LimiteCredito = 0;
                decimal ValorFreteMinimo = 0;
                decimal ValorPedidoParaFreteMinimo = 0;
                decimal QuantidadePedidosFaturados = 0;

                while (Dr.Read())
                {
                    ContasAVencer = Convert.ToDecimal(Dr["VL_CONTAS_A_VENCER"]);
                    ContasVencidas = Convert.ToDecimal(Dr["VL_CONTAS_VENCIDAS"]); 
                    LimiteCredito = Convert.ToDecimal(Dr["VL_LIMITE_CREDITO_CLIENTE"]); 
                    ValorFreteMinimo = Convert.ToDecimal(Dr["VL_FRETE_MINIMO"]); 
                    ValorPedidoParaFreteMinimo = Convert.ToDecimal(Dr["VL_PEDIDO_PARA_FRETE_MINIMO"]);
                    ValorPedidosEmAndamento = Convert.ToDecimal(Dr["VL_PEDIDOS_EM_ANDAMENTO"]);
                    QuantidadePedidosFaturados = Convert.ToDecimal(Dr["QT_PEDIDOS_FATURADOS"]);

                    if((ContasAVencer + doc.ValorTotal + ValorPedidosEmAndamento)  > LimiteCredito)
                    {
                        //bloqueio de credito excedido
                        if (libDAL.PesquisarLiberacaoDocumento(doc.CodigoDocumento, 1) == null)
                        {
                            LiberacaoDocumento lib = new LiberacaoDocumento();
                            lib.CodigoBloqueio = 1;
                            lib.DataLancamento = doc.DataHoraEmissao;
                            lib.CodigoUsuario = doc.Cpl_Usuario;
                            lib.CodigoMaquina = doc.Cpl_Maquina;
                            lib.CodigoDocumento = doc.CodigoDocumento;
                            lib.Cpl_DescricaoBloqueio = "- Limite de " + LimiteCredito.ToString("C") + " e " + (ContasAVencer + ValorPedidosEmAndamento).ToString("C") + " usados...";
                            libDAL.Inserir(lib);
                            lista.Add(lib);
                        }                       
                    }

                    if (ContasVencidas > 0)
                    {
                        //bloqueio de cliente devedor
                        if (libDAL.PesquisarLiberacaoDocumento(doc.CodigoDocumento, 2) != null)
                        {
                            LiberacaoDocumento lib = new LiberacaoDocumento();
                            lib.CodigoBloqueio = 2;
                            lib.DataLancamento = doc.DataHoraEmissao;
                            lib.CodigoUsuario = doc.Cpl_Usuario;
                            lib.CodigoMaquina = doc.Cpl_Maquina;
                            lib.CodigoDocumento = doc.CodigoDocumento;
                            lib.Cpl_DescricaoBloqueio = "- Cliente está devendo " + ContasVencidas.ToString("C") + "...";
                            libDAL.Inserir(lib);
                            lista.Add(lib);
                        }
                    }
                    if (QuantidadePedidosFaturados <= 0)
                    {
                        //bloqueio de frete obrigatório
                        if (libDAL.PesquisarLiberacaoDocumento(doc.CodigoDocumento, 3) != null)
                        {
                            LiberacaoDocumento lib = new LiberacaoDocumento();
                            lib.CodigoBloqueio = 3;
                            lib.DataLancamento = doc.DataHoraEmissao;
                            lib.CodigoUsuario = doc.Cpl_Usuario;
                            lib.CodigoMaquina = doc.Cpl_Maquina;
                            lib.CodigoDocumento = doc.CodigoDocumento;
                            lib.Cpl_DescricaoBloqueio = "- Cliente está fazendo a primeira compra";
                            libDAL.Inserir(lib);
                            lista.Add(lib);
                        }
                    }
                    if (doc.ValorTotal < ValorPedidoParaFreteMinimo && doc.ValorFrete < ValorFreteMinimo && ValorPedidoParaFreteMinimo > 0 && ValorFreteMinimo > 0)//verificar se está correto
                    {
                        //bloqueio de frete obrigatório
                        if (libDAL.PesquisarLiberacaoDocumento(doc.CodigoDocumento, 4) != null)
                        {
                            LiberacaoDocumento lib = new LiberacaoDocumento();
                            lib.CodigoBloqueio = 4;
                            lib.DataLancamento = doc.DataHoraEmissao;
                            lib.CodigoUsuario = doc.Cpl_Usuario;
                            lib.CodigoMaquina = doc.Cpl_Maquina;
                            lib.CodigoDocumento = doc.CodigoDocumento;
                            lib.Cpl_DescricaoBloqueio = "- Frete minímo de " + ValorFreteMinimo + "...";
                            libDAL.Inserir(lib);
                            lista.Add(lib);
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar bloqueios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void EventoDocumento(Doc_Pedido doc, int CodigoSituacao)
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
            if (ListaEvento.Count() > 0)
                eventodoc.CodigoEvento = ListaEvento.Max(x => x.CodigoEvento) + 1;
            else
                eventodoc.CodigoEvento = 1;
            eventoDAL.Inserir(eventodoc, doc.CodigoDocumento);

        }
        public List<Doc_Pedido> ListagemDocumentos(int intCon)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select count(CD_INDEX) AS CONTAGEM, * from [VW_DOC_PEDIDO] ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido> lista = new List<Doc_Pedido>();

                while (Dr.Read())
                {
                    Doc_Pedido p = new Doc_Pedido();

                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.ValorTotal = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.ValorST = Convert.ToDecimal(Dr["VL_ST"]);
                    p.ValorComissao = Convert.ToDecimal(Dr["VL_COMISSAO"]);
                    p.ValorCubagem = Convert.ToDecimal(Dr["VL_CUBAGEM"]);
                    p.ValorDescontoMedio = Convert.ToDecimal(Dr["VL_DESCONTO_MEDIO"]);
                    p.ValorFrete = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.Cpl_DsTipoOperacao = Dr["DS_TIPO_OPERACAO"].ToString();

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos documentos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Doc_Pedido> ListarDocDoca(int intCodIndex, int intCdEmpresa)
        {
            try
            { 
                AbrirConexao();

                string strSQL = "Select * from [VW_LIS_DOCUMENTO] WHERE CD_SITUACAO = 146 AND CD_DOCA_CLIENTE = " + intCodIndex + " AND CD_EMPRESA = " + intCdEmpresa;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido> lista = new List<Doc_Pedido>();

                while (Dr.Read())
                {
                    Doc_Pedido p = new Doc_Pedido();
                    p.DtLancamento = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.NrDocumento = Convert.ToInt32(Dr["NR_DOCUMENTO"]);
                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA_CLIENTE"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoCliente = Convert.ToInt32(Dr["CD_CLIENTE"]);
                    p.NomeCliente = Convert.ToString(Dr["NM_CLIENTE"]);
                    p.NomeCidade = Convert.ToString(Dr["DS_MUNICIPIO"]);
                    p.NomeBairro = Convert.ToString(Dr["DS_BAIRRO"]);
                    p.Transportadora = Convert.ToString(Dr["NM_TRANSPORTE"]);
                    p.Cont = Convert.ToInt32(Dr["QT_PROD_COUNT"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.NomeVendedor = Convert.ToString(Dr["NM_VENDEDOR"]);


                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_Pedido> ListarDocumentosPorPedidos(int intCdEmpresa)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_LIS_DOCUMENTO] WHERE CD_SITUACAO = 146 and CD_EMPRESA = " + intCdEmpresa;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido> lista = new List<Doc_Pedido>();

                while (Dr.Read())
                {
                    Doc_Pedido p = new Doc_Pedido();
                    p.DtLancamento = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.NrDocumento = Convert.ToInt32(Dr["NR_DOCUMENTO"]);
                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoCliente = Convert.ToInt32(Dr["CD_CLIENTE"]);
                    p.NomeCliente = Convert.ToString(Dr["NM_CLIENTE"]);
                    p.NomeCidade = Convert.ToString(Dr["DS_MUNICIPIO"]);
                    p.NomeBairro = Convert.ToString(Dr["DS_BAIRRO"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);
                    p.Transportadora = Convert.ToString(Dr["NM_TRANSPORTE"]);
                    p.Cont = Convert.ToInt32(Dr["QT_PROD_COUNT"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.NomeVendedor = Convert.ToString(Dr["NM_VENDEDOR"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool AtualizarListPedido(decimal decIndice)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL(" UPDATE [DOCUMENTO] SET CD_SITUACAO = 154 Where CD_DOCUMENTO = " + decIndice.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar situação documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void UsuarioPedidosSelecionados(decimal intCodIndex, ref int intUsuario)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select CD_USUARIO from [EVENTO_DO_DOCUMENTO] WHERE CD_SITUACAO = 154 AND CD_DOCUMENTO = " + intCodIndex;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<EventoDocumento> lista = new List<EventoDocumento>();

                if (Dr.Read())
                {
                    intUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos os Eventos do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void CheckedMarcados(ref decimal intCodIndex, ref int intUsuario)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select CD_USUARIO, CD_DOCUMENTO from [EVENTO_DO_DOCUMENTO] WHERE CD_SITUACAO = 154 AND CD_DOCUMENTO = " + intCodIndex;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<EventoDocumento> lista = new List<EventoDocumento>();

                if (Dr.Read())
                {
                    intUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    intCodIndex = Convert.ToInt32(Dr["CD_DOCUMENTO"]);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos os Eventos do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool InserirEventoDocumento(decimal decIndice, int intCdMaquina, int intCdUsuario)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL("insert into EVENTO_DO_DOCUMENTO " +
                    "( CD_DOCUMENTO, CD_EVENTO, CD_SITUACAO, DT_HR_EVENTO, CD_MAQUINA, CD_USUARIO ) " +
                    "values( " + decIndice.ToString() + ", (select max(CD_EVENTO) + 1 from EVENTO_DO_DOCUMENTO where CD_DOCUMENTO = " + decIndice.ToString() + "), 154, getdate(), " + intCdMaquina + ", " + intCdUsuario + " )");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Inserir situação documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public bool DeleteEventoPedido(decimal decIndice, int intCdUsuario)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL(" Delete from [EVENTO_DO_DOCUMENTO] where CD_DOCUMENTO = " + decIndice + " and CD_SITUACAO = 154 and CD_USUARIO = " + intCdUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar situação documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public bool ExecutaSpAtendimentoPedido(int CodMaquina, int CodDoca, string strCdDocumento, ref string strMensagemError)

        {
            bool blnRetorno = false;
            AbrirConexao();
            try
            {
                SqlCommand sqlComand = new SqlCommand("[dbo].[SP_Separacao_Documento]", Con);

                sqlComand.CommandType = CommandType.StoredProcedure;
                sqlComand.Parameters.AddWithValue("@CD_MAQUINA", CodMaquina);
                sqlComand.Parameters.AddWithValue("@lst_cd_documento", strCdDocumento);
                sqlComand.Parameters.AddWithValue("@CD_DOCA", CodDoca);

                SqlDataReader dr = sqlComand.ExecuteReader();

                while (dr.Read())
                {
                    if (dr[1].ToString() != "0")
                    strMensagemError = dr[1].ToString() + " - " + dr[0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Executar Sp Atendimento do Pedido: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                blnRetorno = true;
            }
            return blnRetorno;
        }
        public DataTable RelLisDocumento(string SrtCdDoc)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = "";
                //validada

                strSQL = "Select * from [VW_LIS_ATENDIMENTO] WHERE CD_DOCUMENTO IN (" + SrtCdDoc + ") AND CD_SITUACAO in(155, 157)" +
                    " ORDER BY CD_LOCALIZACAO, CD_LOTE, NM_PRODUTO ";

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar DataSet: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doc_Pedido> ListarPesquisa(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from [VW_LIS_DOCUMENTO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor) + " AND CD_SITUACAO IN (146, 155, 156)";

                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido> lista = new List<Doc_Pedido>();


                Habil_TipoDAL rx = new Habil_TipoDAL();
                Habil_Tipo px = new Habil_Tipo();

                while (Dr.Read())
                {
                    Doc_Pedido p = new Doc_Pedido();

                    p.DtLancamento = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.NrDocumento = Convert.ToInt32(Dr["NR_DOCUMENTO"]);
                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA_DOCUMENTO"]);
                    if (p.CodigoDoca == 0)
                    {
                        p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA_CLIENTE"]);
                    }
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoCliente = Convert.ToInt32(Dr["CD_CLIENTE"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);
                    p.NomeCliente = Convert.ToString(Dr["NM_CLIENTE"]);
                    p.NomeCidade = Convert.ToString(Dr["DS_MUNICIPIO"]);
                    p.NomeBairro = Convert.ToString(Dr["DS_BAIRRO"]);
                    p.Transportadora = Convert.ToString(Dr["NM_TRANSPORTE"]);
                    p.Cont = Convert.ToInt32(Dr["QT_PROD_COUNT"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.NomeVendedor = Convert.ToString(Dr["NM_VENDEDOR"]);

                    lista.Add(p);
                }




                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Pesquisa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }

        }
        public List<Doc_Pedido_Estoque> ListarPedidosEmEstoque(decimal decNrDocumento)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT PD.CD_PROD_DOCUMENTO, pd.CD_PRODUTO, pd.DS_PRODUTO, pd.QT_SOLICITADA, P.CD_BARRAS, P.QT_EMBALAGEM, " +
                    "H.DS_TIPO as DS_SITUACAO, D.CD_SITUACAO , d.NR_DOCUMENTO, D.CD_DOCUMENTO, " +
                    "sum(ISNULL(atd.QT_ATENDIDA, 0)) as QT_ATENDIDA " +
                    "FROM DOCUMENTO AS D " +
                      "INNER JOIN PRODUTO_DO_DOCUMENTO AS PD " +
                        "ON D.CD_DOCUMENTO = PD.CD_DOCUMENTO " +
                        "AND D.CD_TIPO_DOCUMENTO = 8 " +
                      "INNER JOIN PRODUTO AS P " +
                        "ON P.CD_PRODUTO = PD.CD_PRODUTO " +
                      "INNER JOIN HABIL_TIPO AS H " +
                        "ON H.CD_TIPO = D.CD_SITUACAO " +
                      "LEFT JOIN atendimento_do_documento as atd " +
                        "ON atd.cd_documento = d.cd_documento " +
                        "and atd.cd_prod_documento = pd.cd_prod_documento " +
                    "WHERE D.CD_SITUACAO in(155, 156) AND D.NR_DOCUMENTO = " + decNrDocumento +
                    " GROUP BY PD.CD_PROD_DOCUMENTO, pd.CD_PRODUTO, pd.DS_PRODUTO, pd.QT_SOLICITADA, P.CD_BARRAS,  P.QT_EMBALAGEM , H.DS_TIPO, D.CD_SITUACAO, " +
                        "d.NR_DOCUMENTO, D.CD_DOCUMENTO ";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido_Estoque> lista = new List<Doc_Pedido_Estoque>();

                while (Dr.Read())
                {
                    Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();
                    p.CodigoProdutoDocumento = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.QtEmbalagem = Convert.ToInt16(Dr["QT_EMBALAGEM"]);
                    p.DescricaoProduto = Convert.ToString(Dr["DS_PRODUTO"]);
                    p.QtAtendida = Convert.ToDecimal(Dr["QT_ATENDIDA"]);
                    p.CodigoBarras = Convert.ToString(Dr["CD_BARRAS"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Pedidos em Estoques: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void PedidosEmEstoque(decimal decNrDocumento, Doc_Pedido_Estoque p)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT D.CD_DOCUMENTO, D.CD_EMPRESA, D.CD_SITUACAO, H.DS_TIPO AS DS_SITUACAO, PD.RAZ_SOCIAL, d.cd_doca, DC.DS_DOCA " +
                    "from DOCUMENTO AS D " +
                        "INNER JOIN HABIL_TIPO AS H " +
                            "ON H.CD_TIPO = D.CD_SITUACAO " +
                        "INNER JOIN PESSOA_DO_DOCUMENTO AS PD " +
                            "ON PD.CD_DOCUMENTO = D.CD_DOCUMENTO " +
                            "AND TP_PESSOA = 12 " +
                        "INNER JOIN DOCA AS DC " +
                            "ON DC.CD_DOCA = D.CD_DOCA " +
                    "WHERE D.CD_SITUACAO in(155, 156) and d.NR_DOCUMENTO = " + decNrDocumento + " AND d.CD_TIPO_DOCUMENTO = 8";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                {
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);
                    p.NomeCliente = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]);
                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Estoques: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ProdutoPedidosEmEstoque(ref int intproduto, ref string barras, decimal decDoc, ref short ShtQtEmb)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT PD.CD_PRODUTO, P.CD_BARRAS, P.QT_EMBALAGEM FROM PRODUTO_DO_DOCUMENTO AS PD " +
                    "INNER JOIN PRODUTO AS P " +
                    " ON PD.CD_PRODUTO = P.CD_PRODUTO " +
                    "INNER JOIN DOCUMENTO AS D " +
                    " ON D.CD_DOCUMENTO = PD.CD_DOCUMENTO " +
                    "and D.CD_TIPO_DOCUMENTO = 8 " +
                    "WHERE D.NR_DOCUMENTO = " + decDoc;

                if (intproduto != 0)
                    strSQL += " And pd.CD_PRODUTO = " + intproduto;
                if (barras != null)
                    strSQL += " and p.CD_BARRAS = '" + barras + "' ";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                intproduto = 0;
                ShtQtEmb = 0;
                barras = null;
                while (Dr.Read())
                {
                    Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();

                    intproduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    barras = Convert.ToString(Dr["CD_BARRAS"]);
                    ShtQtEmb = Convert.ToInt16(Dr["QT_EMBALAGEM"]);
                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Pedidos em Estoques: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool InserirVolumeDocumento(VolumeDocumento p)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL("insert into VOLUME_DO_DOCUMENTO " +
                    "( CD_DOCUMENTO, CD_ID_VOLUME, CD_EMB_VOLUME, TX_INDENTIFICACAO) " +
                    "values( " + p.CodigoDocumento + ", (select ISNULL(max(CD_ID_VOLUME),0) + 1 from " +
                    "VOLUME_DO_DOCUMENTO where CD_DOCUMENTO = " + p.CodigoDocumento + ")," + p.CodigoEmbalagem + ", '" + p.DescricaoIdentificacao.ToString() + "') "); 
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Inserir Volume do Documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public bool InserirVolumeProdutoDocumento(VolumeDocumento p)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL("insert into VOLUME_PRODUTO_DOCUMENTO " +
                    "( CD_DOCUMENTO, CD_PROD_DOCUMENTO, CD_VOL_DOCUMENTO, QT_EMBALADA) " +
                    "values( " + p.CodigoDocumento + ", " + p.CodigoProdutoDocumento + ", (select ISNULL(max(CD_ID_VOLUME),0) from " +
                    "VOLUME_DO_DOCUMENTO where CD_DOCUMENTO = " + p.CodigoDocumento + ")," + p.QtEmbalagem + ")");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Inserir Volume do Produto do Documento: " + ex.Message.ToString());
            }
            finally 
            {
            }
        }
        public bool AtualizarDocumento(decimal decDoc, short shtQtVol)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL(" UPDATE [DOCUMENTO] SET CD_SITUACAO = 156, QT_VOLUME = " + shtQtVol + " Where CD_DOCUMENTO = " + decDoc.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar situação documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public bool InserirEventoDocumentoConferido(decimal decIndice,int intSituacao, int intCdMaquina, int intCdUsuario)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL("insert into EVENTO_DO_DOCUMENTO " +
                    "( CD_DOCUMENTO, CD_EVENTO, CD_SITUACAO, DT_HR_EVENTO, CD_MAQUINA, CD_USUARIO ) " +
                    "values( " + decIndice.ToString() + ", (select max(CD_EVENTO) + 1 from EVENTO_DO_DOCUMENTO where CD_DOCUMENTO = " + decIndice.ToString() + ")," + intSituacao + ", getdate(), " + intCdMaquina + ", " + intCdUsuario + " )");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Inserir Evento do Documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public List<Doc_Pedido_Estoque> ListarRecontagem(decimal decCodDocumento, int intCont)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select pd.CD_PRODUTO, pd.CD_PROD_DOCUMENTO, pd.DS_PRODUTO, v.CD_EMB_VOLUME, v.TX_INDENTIFICACAO, vd.QT_EMBALADA " +
                    "from DOCUMENTO as d " +
                        "inner join PRODUTO_DO_DOCUMENTO as pd " +
                            "on d.CD_DOCUMENTO = pd.CD_DOCUMENTO " +
                        "inner join VOLUME_DO_DOCUMENTO as v " +
                            "on v.CD_DOCUMENTO = d.CD_DOCUMENTO " +
                        "inner join VOLUME_PRODUTO_DOCUMENTO as vd " +
                            "on d.CD_DOCUMENTO = vd.CD_DOCUMENTO " +
                            "and vd.CD_VOL_DOCUMENTO = v.CD_ID_VOLUME " +
                            "and vd.CD_PROD_DOCUMENTO = pd.CD_PROD_DOCUMENTO " +
                    "where d.CD_DOCUMENTO = " + decCodDocumento + " and v.CD_ID_VOLUME = " + intCont;

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Doc_Pedido_Estoque> lista = new List<Doc_Pedido_Estoque>();

                while (Dr.Read())
                {
                    Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();
                    p.CodigoProdutoDocumento = Convert.ToInt32(Dr["CD_PROD_DOCUMENTO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.QtColetada = Convert.ToInt16(Dr["QT_EMBALADA"]);
                    p.DescricaoProduto = Convert.ToString(Dr["DS_PRODUTO"]);
                    p.DescricaoIndentificacao = Convert.ToString(Dr["TX_INDENTIFICACAO"]);
                    p.CodigoBarrasCaixa = Convert.ToString(Dr["CD_EMB_VOLUME"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Recontagem: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Doc_Pedido_Estoque DocumentoSemDoca(decimal decCodDocumento)
        {
            try
            {
                AbrirConexao();

                strSQL = "SELECT isnull(cd_doca,0) as cd_doca, * FROM DOCUMENTO AS D " +
                    "INNER JOIN PESSOA_DO_DOCUMENTO AS PD " +
                    "ON D.CD_DOCUMENTO = PD.CD_DOCUMENTO " +
                    "AND D.CD_TIPO_DOCUMENTO = 8 " +
                    "AND PD.TP_PESSOA = 12 " +
                    "where d.cd_documento = " + decCodDocumento;

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                Doc_Pedido_Estoque p = new Doc_Pedido_Estoque();

                if (Dr.Read())
                {
                    p = new Doc_Pedido_Estoque();
                    p.CodigoDocumento = Convert.ToInt32(Dr["cd_documento"]);
                    p.CodigoDoca = Convert.ToInt32(Dr["cd_doca"]);
                    p.CodigoCliente = Convert.ToInt32(Dr["cd_pessoa"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Documento sem Doca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public int PessoaSemDoca(int cd_pessoa)
        {
            try
            {
                int intDoca = 0;
                AbrirConexao();
                Dr = new SqlCommand("select * from PESSOA where cd_doca = 0 or cd_doca is null", Con).ExecuteReader();

                if (Dr.Read())
                {
                    intDoca = Convert.ToInt32(Dr["cd_doca"]);
                }
                return intDoca;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Pessoas sem Doca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public int CountCaixas(decimal decCdDocumento)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select count(CD_DOCUMENTO) as count from VOLUME_DO_DOCUMENTO where CD_DOCUMENTO = " + decCdDocumento;
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                int count = 0;

                if (Dr.Read())
                {
                    count = Convert.ToInt32(Dr["count"]);
                }
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Contar Embalagens: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool ExcluirVolumeDoDocumento(decimal decDoc)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL("delete from VOLUME_DO_DOCUMENTO where CD_DOCUMENTO = "+ decDoc);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Volume do Documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public bool ExcluirProdutoDoVolumeDoDocumento(decimal decDoc)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL("delete from VOLUME_PRODUTO_DOCUMENTO where CD_DOCUMENTO = " + decDoc);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Excluir Evento do Documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public bool AtualizarDocumentoRecontagem(decimal decDoc)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL("update documento set cd_situacao = 155 where cd_documento = " + decDoc);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Atualização do Documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public int PesquisarDocumentoEmConferencia (decimal decCdDocumento  )
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("select * from EVENTO_DO_DOCUMENTO " +
                    "where CD_DOCUMENTO = " +  decCdDocumento + " " +
                    "and CD_EVENTO = (select max(CD_EVENTO) from EVENTO_DO_DOCUMENTO " +
                    "where CD_DOCUMENTO = " + decCdDocumento + " )", Con);
                Dr = Cmd.ExecuteReader();

                int SituacaoExistente = 0;

                if(Dr.Read())
                {
                    SituacaoExistente = Convert.ToInt32(Dr["CD_SITUACAO"]);
                }
                return SituacaoExistente;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Evento do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelPedidoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "SELECT * FROM ( " +
                                    " select* from VW_DOC_PEDIDO WHERE CD_SITUACAO != 158 " +
                                    " UNION " +
                                    " select [CD_DOCUMENTO] " +
                                          " ,[NR_DOCUMENTO] " +
                                          " ,[CD_SITUACAO] " +
                                          " ,[DS_SITUACAO] " +
                                          " ,[CD_PESSOA] " +
                                          " ,[RAZ_SOCIAL] " +
                                          " ,[CD_PESSOA_TRANSPORTADOR] " +
                                          " ,[RAZ_SOCIAL_TRANSPORTADOR] " +
                                          " ,[CD_GPO_PESSOA] " +
                                          " ,[DS_GPO_PESSOA] " +
                                          " ,[OB_DOCUMENTO] " +
                                          " ,[CD_EMPRESA] " +
                                          " ,(0) AS VL_TOTAL_GERAL " +
                                          " ,[DT_HR_EMISSAO] " +
                                          " ,[DG_SR_DOCUMENTO] " +
                                          " ,[CD_CND_PAGAMENTO] " +
                                          " ,[DG_DOCUMENTO] " +
                                          " ,[CD_TIPO_COBRANCA] " +
                                          " ,[DT_VENCIMENTO] " +
                                          " ,[CD_CLASSIFICACAO]   " +
                                          " ,[CD_GER_SEQ_DOC]     " +
                                          " ,[CD_VENDEDOR]        " +
                                          " ,[NR_WEB]             " +
                                          " ,[VL_FRETE]           " +
                                          " ,[VL_COMISSAO]        " +
                                          " ,[VL_DESCONTO_MEDIO]  " +
                                          " ,[VL_PESO]            " +
                                          " ,[VL_CUBAGEM]         " +
                                          " ,[VL_ST]              " +
                                          " ,[CD_APLICACAO_USO]   " +
                                          " ,[CD_PESSOA_VENDEDOR] " +
                                          " ,[NOME_VENDEDOR]      " +
                                          " ,[DS_TIPO_COBRANCA]   " +
                                          " ,[DS_CND_PAGAMENTO]   " +
                                          " ,[DS_APLICACAO_USO]   " +
                                          " ,[CD_DOC_ORIGINAL]    " +
                                          " ,[CD_TIPO_OPERACAO]   " +
                                          " ,[DS_TIPO_OPERACAO]   " +
                                    "from VW_DOC_PEDIDO WHERE CD_SITUACAO = 158 " +
                                ") AS AAA ";
                string strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                strSQL = strSQL + " ORDER BY DS_SITUACAO ";
                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório de pedidos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
    }
}

