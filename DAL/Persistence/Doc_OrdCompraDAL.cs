using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Persistence
{
    public class Doc_OrdCompraDAL : Conexao
    {
        protected string strSQL = "";
        public void Inserir(Doc_OrdCompra p, List<ProdutoDocumento> listaItemOrcamento, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento)
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
                                                "TX_MOTIVO_BAIXA," +
                                                "CD_TIPO_OPERACAO) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26) SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                decimal CodigoGerado = 0;
                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                if(p.Cpl_NomeTabela != null)
                    CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", 11);
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
                Cmd.Parameters.AddWithValue("@v20", p.ValorPesoOrcamento);
                Cmd.Parameters.AddWithValue("@v21", p.ValorDescontoMedio);
                Cmd.Parameters.AddWithValue("@v22", p.ValorFrete);
                Cmd.Parameters.AddWithValue("@v23", p.NumeroWeb);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoAplicacaoUso);
                Cmd.Parameters.AddWithValue("@v25", p.MotivoBaixaSemVenda);
                Cmd.Parameters.AddWithValue("@v26", p.CodigoTipoOperacao);

                p.CodigoDocumento = Convert.ToDecimal(Cmd.ExecuteScalar());
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
                            throw new Exception("Erro ao Incluir Ordem de Compra: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Ordem de Compra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTransportador, 13);//Pessoa do Documento Transportador
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoPessoa, 6);//Pessoa do Documento Transportador

                if (eventoDocumento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
                }
                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);

                ProdutoDocumentoDAL ItemDAL = new ProdutoDocumentoDAL();
                ItemDAL.Inserir(p.CodigoDocumento, listaItemOrcamento);
            }
        }
        public void Atualizar(Doc_OrdCompra p, List<ProdutoDocumento> listaItemOrcamento, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento)
        {
            try
            {
                Doc_OrdCompra p2 = new Doc_OrdCompra();
                p2 = PesquisarOrdemCompra(Convert.ToInt32(p.CodigoDocumento));
                GerarLog(p, p2);
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_TIPO_DOCUMENTO = @v1," +
                                                "CD_EMPRESA = @v2," +
                                                "DT_HR_EMISSAO = @v3," +
                                                "NR_DOCUMENTO = @v4," +
                                                "DG_DOCUMENTO = @v5," +
                                                "DG_SR_DOCUMENTO = @v6," +
                                                "VL_TOTAL_GERAL = @v7," +
                                                "OB_DOCUMENTO = @v8," +
                                                "CD_TIPO_COBRANCA = @v9," +
                                                "CD_CND_PAGAMENTO = @v10," +
                                                "DT_VENCIMENTO = @v11," +
                                                "CD_SITUACAO = @v12," +
                                                "CD_CLASSIFICACAO = @v13," +
                                                "CD_TIPO_OPERACAO = @v14," +
                                                "CD_VENDEDOR = @v15," +
                                                "VL_ST = @v16," +
                                                "VL_COMISSAO = @v17," +
                                                "VL_CUBAGEM = @v18," +
                                                "VL_PESO = @v19," +
                                                "VL_DESCONTO_MEDIO = @v20," +
                                                "VL_FRETE = @v21," +
                                                "NR_WEB = @v22," +
                                                "CD_APLICACAO_USO = @v23," +
                                                "TX_MOTIVO_BAIXA = @v24  Where [CD_DOCUMENTO] = @CODIGO";

                Cmd = new SqlCommand(strSQL, Con);


                Cmd.Parameters.AddWithValue("@CODIGO", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v1", 11);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v4", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v5", p.DGNumeroDocumento);
                Cmd.Parameters.AddWithValue("@v6", p.DGSerieDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.ValorTotal);
                Cmd.Parameters.AddWithValue("@v8", p.DescricaoDocumento);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v11", p.DataValidade);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoSituacao);           
                Cmd.Parameters.AddWithValue("@v13", p.CodigoTipoOrcamento);
                Cmd.Parameters.AddWithValue("@v14", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoVendedor);
                Cmd.Parameters.AddWithValue("@v16", p.ValorST);
                Cmd.Parameters.AddWithValue("@v17", p.ValorComissao);
                Cmd.Parameters.AddWithValue("@v18", p.ValorCubagem);
                Cmd.Parameters.AddWithValue("@v19", p.ValorPesoOrcamento);
                Cmd.Parameters.AddWithValue("@v20", p.ValorDescontoMedio);
                Cmd.Parameters.AddWithValue("@v21", p.ValorFrete);
                Cmd.Parameters.AddWithValue("@v22", p.NumeroWeb);
                Cmd.Parameters.AddWithValue("@v23", p.CodigoAplicacaoUso);
                Cmd.Parameters.AddWithValue("@v24", p.MotivoBaixaSemVenda);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a Ordem de Compra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoPessoa, 6);
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
            }
        }
        public void AtualizarSituacao(decimal CodigoDocumento, int CodigoSituacaoNova, int CodigoSituacaoAnterior, int CodigoUsuario, int CodigoMaquina)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_SITUACAO = @v1 WHERE CD_DOCUMENTO = @v2 ";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoSituacaoNova);
                Cmd.Parameters.AddWithValue("@v2", CodigoDocumento);

                if(CodigoSituacaoAnterior != 0 && CodigoSituacaoAnterior != CodigoSituacaoNova)
                {
                    Doc_OrdCompra doc = new Doc_OrdCompra();
					Doc_OrdCompraDAL docDAL = new Doc_OrdCompraDAL();
                    doc = docDAL.PesquisarOrdemCompra(CodigoDocumento);
                    doc.Cpl_Maquina = CodigoMaquina;
                    doc.Cpl_Usuario = CodigoUsuario;
                    if(doc != null)
                        EventoDocumento(doc,CodigoSituacaoNova);
                }


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar situacao da Ordem de Compra: " + ex.Message.ToString());
            }

        }
        public void Excluir(decimal Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update DOCUMENTO set CD_SITUACAO = 139 where CD_DOCUMENTO = @v1", Con);
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
        public Doc_OrdCompra PesquisarOrdemCompra(decimal Codigo)
        {
            try
            {
                long CodTransportador = PesquisarPessoaDocumento(Codigo, 13);
                long CodCliente = PesquisarPessoaDocumento(Codigo, 6);

                AbrirConexao();
                strSQL = "Select * from [VW_DOC_ORD_COMPRA] Where CD_DOCUMENTO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Doc_OrdCompra p = null;

                if (Dr.Read())
                {
                    p = new Doc_OrdCompra();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento= Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
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
                    p.ValorPesoOrcamento = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeVendedor = Dr["NOME_VENDEDOR"].ToString();
                    p.Cpl_DsCondicaoPagamento = Dr["DS_CND_PAGAMENTO"].ToString();
                    p.Cpl_DsAplicacaoUso = Dr["DS_APLICACAO_USO"].ToString();
                    p.Cpl_DsTipoCobranca = Dr["DS_TIPO_COBRANCA"].ToString();
                    p.Cpl_DsTipoOrcamento = Dr["DS_TIPO_COBRANCA"].ToString();
                    p.Cpl_NomeTransportador = Dr["RAZ_SOCIAL_TRANSPORTADOR"].ToString();
                    p.Cpl_QuantidadePedidosVinculados = Convert.ToInt32(Dr["QT_PEDIDOS_VINCULADOS"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.MotivoBaixaSemVenda = Dr["TX_MOTIVO_BAIXA"].ToString();
                    p.Cpl_CodigoTransportador = CodTransportador;
                    p.Cpl_CodigoPessoa = CodCliente;
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Doc_OrdCompra: " + ex.Message.ToString());
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
                    p.Cpl_CodigoPessoa = Convert.ToInt16(Dr["CD_PESSOA"]);
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

                throw new Exception("Erro ao Listar Contas a receber da Ordem de Compra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Doc_OrdCompra> ListarOrdemCompra(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_ORD_COMPRA] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_OrdCompra> lista = new List<Doc_OrdCompra>();

                while (Dr.Read())
                {
                    Doc_OrdCompra p = new Doc_OrdCompra();

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
                    p.ValorPesoOrcamento = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeVendedor = Dr["NOME_VENDEDOR"].ToString();

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as Ordens de Compra:  " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Doc_OrdCompra> ListarOrdemCompraPessoa(long CodigoPessoa, decimal CodigoDocumentoAtual)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_ORD_COMPRA] where CD_PESSOA = @v1 AND CD_DOCUMENTO != @v2 ORDER BY CD_DOCUMENTO DESC";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v2", CodigoDocumentoAtual);
                Dr = Cmd.ExecuteReader();

                List<Doc_OrdCompra> lista = new List<Doc_OrdCompra>();

                while (Dr.Read())
                {
                    Doc_OrdCompra p = new Doc_OrdCompra();

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
                    p.ValorPesoOrcamento = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeVendedor = Dr["NOME_VENDEDOR"].ToString();
                    p.Cpl_DsCondicaoPagamento = Dr["DS_CND_PAGAMENTO"].ToString();
                    p.Cpl_DsAplicacaoUso = Dr["DS_APLICACAO_USO"].ToString();
                    p.Cpl_DsTipoCobranca = Dr["DS_TIPO_COBRANCA"].ToString();
                    p.Cpl_DsTipoOrcamento = Dr["DS_TIPO_COBRANCA"].ToString();
                    p.Cpl_NomeTransportador = Dr["RAZ_SOCIAL_TRANSPORTADOR"].ToString();
                    p.MotivoBaixaSemVenda = Dr["TX_MOTIVO_BAIXA"].ToString();
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as Ordens de Compra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Doc_OrdCompra> ListarOrdemComprasCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VW_DOC_ORD_COMPRA] ";


                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_OrdCompra> lista = new List<Doc_OrdCompra>();

                while (Dr.Read())
                {
                    Doc_OrdCompra p = new Doc_OrdCompra();
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
                    p.ValorPesoOrcamento = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoAplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO_USO"]);
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeVendedor = Dr["NOME_VENDEDOR"].ToString();
                    if (!string.IsNullOrEmpty(Dr["TX_MOTIVO_BAIXA"].ToString()))
                    {
                        p.MotivoBaixaSemVenda = Dr["TX_MOTIVO_BAIXA"].ToString();
                    }

                    if (p.CodigoSituacao == 137 || p.CodigoSituacao == 138 || p.CodigoSituacao == 144)
                        p.PodeGerarPedido = false;
                    else
                        p.PodeGerarPedido = true;

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as Ordens de Compra: " + ex.Message.ToString());
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
        public bool AtualizarPessoaDocumento(decimal CodigoDocumento,  long CodigoPessoa, int TipoPessoa)
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
        public DataTable RelOrdemCompra(decimal CodigoDocumento)
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
                                    "ISNULL(CTT_VENDEDOR.TX_MAIL1, '') AS EMAIL_VENDEDOR " +
                                "FROM " +
                                    "VW_DOC_ORD_COMPRA AS DOC " +
                                    "LEFT JOIN EMPRESA AS E ON E.CD_EMPRESA = DOC.CD_EMPRESA " +
                                    "LEFT JOIN PESSOA AS P ON P.CD_PESSOA = E.CD_PESSOA " +
                                    "LEFT JOIN PESSOA_ENDERECO AS E_END ON E_END.CD_PESSOA = E.CD_PESSOA AND E_END.TP_ENDERECO = 5 " +
                                    "LEFT JOIN PESSOA_INSCRICAO AS E_INS ON E_INS.CD_PESSOA = E.CD_PESSOA AND E_INS.TP_INSCRICAO = 3 " +
                                    "LEFT JOIN PESSOA_DO_DOCUMENTO AS PDOC ON PDOC.CD_DOCUMENTO = DOC.CD_DOCUMENTO AND PDOC.TP_PESSOA = 6 " +
                                    "LEFT JOIN ESTADO ON ESTADO.CD_ESTADO = E_END.CD_ESTADO " +
                                    "LEFT JOIN VENDEDOR ON VENDEDOR.CD_VENDEDOR = DOC.CD_VENDEDOR " +
                                    "LEFT JOIN PESSOA_CONTATO AS CTT_VENDEDOR ON CTT_VENDEDOR.CD_PESSOA = VENDEDOR.CD_PESSOA " +
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

                throw new Exception("Erro ao Listar Relatório de Ordem de Compra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public void GerarLog(Doc_OrdCompra p1, Doc_OrdCompra p2)
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
                    log.DescricaoLog = "de: " + p2.Cpl_DsCondicaoPagamento+ " para: " + p1.Cpl_DsCondicaoPagamento;
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
            if (p1.ValorPesoOrcamento != p2.ValorPesoOrcamento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_PESO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorPesoOrcamento + " para: " + p1.ValorPesoOrcamento;
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
                log.DescricaoLog = "de: " + p2.Cpl_DsTipoOrcamento + " para: " + p1.Cpl_DsTipoOrcamento;
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
                log.DescricaoLog = "de: " + p2.Cpl_DsTipoCobranca + " para: " + p1.Cpl_DsTipoCobranca;
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
                log.DescricaoLog = "de: " + p2.Cpl_NomeTransportador + " para: " + p1.Cpl_NomeTransportador;
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
                log.DescricaoLog = "de: " + p2.Cpl_NomeVendedor + " para: " + p1.Cpl_NomeVendedor;
            }
            if (p1.CodigoAplicacaoUso != p2.CodigoAplicacaoUso)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_APLICACAO_USO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.Cpl_DsAplicacaoUso + " para: " + p1.Cpl_DsAplicacaoUso;
            }
        }
        public void EventoDocumento(Doc_OrdCompra doc, int CodigoSituacao)
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
    }
}
