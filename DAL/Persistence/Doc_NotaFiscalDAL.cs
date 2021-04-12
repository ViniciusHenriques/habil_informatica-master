using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Persistence
{
    public class Doc_NotaFiscalDAL : Conexao
    {
        protected string strSQL = "";
        public void Inserir(Doc_NotaFiscal p, List<ProdutoDocumento> listaItens, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento, List<ParcelaDocumento> ListaParcelaDocumento)
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
                                                "DT_HR_SAIDA," +
                                                "CD_SITUACAO," +
                                                "CD_CLASSIFICACAO," +
                                                "CD_GER_SEQ_DOC," +
                                                "CD_NAT_OPERACAO," +
                                                "VL_ST," +
                                                "CD_FINALIDADE," +
                                                "CD_REG_TRIBUTARIO," +
                                                "VL_PESO," +
                                                "CD_INDICADOR_PRESENCA," +
                                                "VL_FRETE," +
                                                "NR_WEB," +
                                                "CD_DOC_ORIGINAL," +
                                                "CD_TIPO_OPERACAO," +
                                                "CD_CONSUMIDOR_FINAL," +
                                                "CD_MOD_FRETE," +
                                                "VL_CUBAGEM) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26,@v27,@v28) SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                decimal CodigoGerado = 0;
                GeradorSequencialDocumentoEmpresaDAL gerDAL = new GeradorSequencialDocumentoEmpresaDAL();
                if (p.Cpl_NomeTabela != null)
                    CodigoGerado = gerDAL.IncluirTabelaGerador(p.Cpl_NomeTabela, Convert.ToInt32(p.CodigoGeracaoSequencialDocumento), p.Cpl_Usuario, p.Cpl_Maquina);

                Cmd.Parameters.AddWithValue("@v1", 9);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v4", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v5", CodigoGerado);
                Cmd.Parameters.AddWithValue("@v6", p.DGNumeroDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.DGSerieDocumento);
                Cmd.Parameters.AddWithValue("@v8", p.ValorTotalDocumento);
                Cmd.Parameters.AddWithValue("@v9", p.DescricaoDocumento);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoCondicaoPagamento);
                if (p.DataHoraSaida.HasValue)
                    Cmd.Parameters.AddWithValue("@v12", p.DataHoraSaida);
                else
                    Cmd.Parameters.AddWithValue("@v12", DBNull.Value);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v14", p.CodigoTipoOrcamento);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoGeracaoSequencialDocumento);
                Cmd.Parameters.AddWithValue("@v16", p.CodigoNaturezaOperacao);
                Cmd.Parameters.AddWithValue("@v17", p.ValorICMSST);
                Cmd.Parameters.AddWithValue("@v18", p.CodigoFinalidadeNF);
                Cmd.Parameters.AddWithValue("@v19", p.CodigoRegimeTributario);
                Cmd.Parameters.AddWithValue("@v20", p.ValorPesoBruto);
                Cmd.Parameters.AddWithValue("@v21", p.CodigoIndicadorPresenca);
                Cmd.Parameters.AddWithValue("@v22", p.ValorFrete);
                Cmd.Parameters.AddWithValue("@v23", p.NumeroWeb);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoDocumentoOriginal);
                Cmd.Parameters.AddWithValue("@v25", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v26", p.CodigoConsumidorFinal);
                Cmd.Parameters.AddWithValue("@v27", p.CodigoModalidadeFrete);
                Cmd.Parameters.AddWithValue("@v28", p.ValorCubagem);

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
                            throw new Exception("Erro ao Incluir nota fiscal: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar nota fiscal: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTransportador, 15);//Pessoa do Documento Transportador
                InserirPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoPessoa, 14);//Pessoa do Documento 

                if (eventoDocumento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
                }
                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);

                ProdutoDocumentoDAL ItemDAL = new ProdutoDocumentoDAL();
                ItemDAL.Inserir(p.CodigoDocumento, listaItens);

                ParcelaDocumentoDAL ParcelaDAL = new ParcelaDocumentoDAL();
                ParcelaDAL.Inserir(p.CodigoDocumento, ListaParcelaDocumento);
            }
        }
        public void Atualizar(Doc_NotaFiscal p, List<ProdutoDocumento> listaItens, EventoDocumento eventoDocumento, List<AnexoDocumento> ListaAnexoDocumento, List<ParcelaDocumento> ListaParcelaDocumento)
        {
            try
            {
                Doc_NotaFiscal p2 = new Doc_NotaFiscal();
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
                                                "DT_HR_SAIDA = @v11," +
                                                "CD_SITUACAO = @v12," +
                                                "CD_CLASSIFICACAO = @v13," +
                                                "CD_NAT_OPERACAO = @v15," +
                                                "VL_ST = @v16," +
                                                "CD_FINALIDADE = @v17," +
                                                "CD_REG_TRIBUTARIO = @v18," +
                                                "VL_PESO = @v19," +
                                                "CD_INDICADOR_PRESENCA = @v20," +
                                                "VL_FRETE = @v21," +
                                                "NR_WEB = @v22," +
                                                "CD_DOC_ORIGINAL = @v23," +
                                                "CD_TIPO_OPERACAO = @v24," +
                                                "CD_CONSUMIDOR_FINAL = @v25," +
                                                "CD_MOD_FRETE = @v26," +
                                                "VL_CUBAGEM = @v27 Where [CD_DOCUMENTO] = @CODIGO";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@CODIGO", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v1", 9);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v3", p.DataHoraEmissao);
                Cmd.Parameters.AddWithValue("@v5", p.DGNumeroDocumento);
                Cmd.Parameters.AddWithValue("@v6", p.DGSerieDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.ValorTotalDocumento);
                Cmd.Parameters.AddWithValue("@v8", p.DescricaoDocumento);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoCondicaoPagamento);
                if(p.DataHoraSaida.HasValue)
                    Cmd.Parameters.AddWithValue("@v11", p.DataHoraSaida);
                else
                    Cmd.Parameters.AddWithValue("@v11", DBNull.Value);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoTipoOrcamento);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoNaturezaOperacao);
                Cmd.Parameters.AddWithValue("@v16", p.ValorICMSST);
                Cmd.Parameters.AddWithValue("@v17", p.CodigoFinalidadeNF);
                Cmd.Parameters.AddWithValue("@v18", p.CodigoRegimeTributario);
                Cmd.Parameters.AddWithValue("@v19", p.ValorPesoBruto);
                Cmd.Parameters.AddWithValue("@v20", p.CodigoIndicadorPresenca);
                Cmd.Parameters.AddWithValue("@v21", p.ValorFrete);
                Cmd.Parameters.AddWithValue("@v22", p.NumeroWeb);
                Cmd.Parameters.AddWithValue("@v23", p.CodigoDocumentoOriginal);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v25", p.CodigoConsumidorFinal);
                Cmd.Parameters.AddWithValue("@v26", p.CodigoModalidadeFrete);
                Cmd.Parameters.AddWithValue("@v27", p.ValorCubagem);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar nota fiscal: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoPessoa, 14);
                AtualizarPessoaDocumento(p.CodigoDocumento, p.Cpl_CodigoTransportador, 15);

                if (eventoDocumento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(eventoDocumento, p.CodigoDocumento);
                }

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, ListaAnexoDocumento);

                ProdutoDocumentoDAL ItemDAL = new ProdutoDocumentoDAL();
                ItemDAL.Inserir(p.CodigoDocumento, listaItens);

                ParcelaDocumentoDAL ParcelaDAL = new ParcelaDocumentoDAL();
                ParcelaDAL.Inserir(p.CodigoDocumento, ListaParcelaDocumento);
            }
        }
        public void Excluir(decimal Codigo)
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
                            throw new Exception("Erro ao excluir nota fiscal: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir nota fiscal: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public Doc_NotaFiscal PesquisarDocumento(decimal Codigo)
        {
            try
            {
                long CodTransportador = PesquisarPessoaDocumento(Codigo, 15);
                long CodCliente = PesquisarPessoaDocumento(Codigo, 14);

                AbrirConexao();
                strSQL = "Select * from [VW_DOC_NF] Where CD_DOCUMENTO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Doc_NotaFiscal p = null;

                if (Dr.Read())
                {
                    p = new Doc_NotaFiscal();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    if(Dr["DT_HR_SAIDA"] != DBNull.Value)
                        p.DataHoraSaida = Convert.ToDateTime(Dr["DT_HR_SAIDA"]);
                    p.ValorTotalDocumento = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoNaturezaOperacao = Convert.ToInt64(Dr["CD_NAT_OPERACAO"]);
                    p.ValorICMSST = Convert.ToDecimal(Dr["VL_ST"]);
                    p.ValorFrete = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.ValorPesoBruto = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CodigoFinalidadeNF = Convert.ToInt32(Dr["CD_FINALIDADE"]);
                    p.CodigoRegimeTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"].ToString());
                    p.CodigoIndicadorPresenca = Convert.ToInt32(Dr["CD_INDICADOR_PRESENCA"]);
                    p.CodigoFinalidadeNF = Convert.ToInt32(Dr["CD_FINALIDADE"]);
                    p.CodigoConsumidorFinal = Convert.ToInt32(Dr["CD_CONSUMIDOR_FINAL"]);
                    p.ChaveAcesso = Convert.ToString(Dr["CD_CHAVE_ACESSO"]);
                    p.CodigoModalidadeFrete = Convert.ToInt32(Dr["CD_MOD_FRETE"]);
                    p.ValorCubagem = Convert.ToDecimal(Dr["VL_CUBAGEM"]);

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
                throw new Exception("Erro ao Pesquisar nota fiscal: " + ex.Message.ToString());
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
        public List<Doc_NotaFiscal> ListarDocumentos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_NF] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_NotaFiscal> lista = new List<Doc_NotaFiscal>();

                while (Dr.Read())
                {
                    Doc_NotaFiscal p = new Doc_NotaFiscal();

                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    if (Dr["DT_HR_SAIDA"] != DBNull.Value)
                        p.DataHoraSaida = Convert.ToDateTime(Dr["DT_HR_SAIDA"]);
                    p.ValorTotalDocumento = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoNaturezaOperacao = Convert.ToInt64(Dr["CD_NAT_OPERACAO"]);
                    p.ValorICMSST = Convert.ToDecimal(Dr["VL_ST"]);
                    p.ValorFrete = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.ValorPesoBruto = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CodigoFinalidadeNF = Convert.ToInt32(Dr["CD_FINALIDADE"]);
                    p.CodigoRegimeTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"].ToString());
                    p.CodigoIndicadorPresenca = Convert.ToInt32(Dr["CD_INDICADOR_PRESENCA"]);
                    p.CodigoFinalidadeNF = Convert.ToInt32(Dr["CD_FINALIDADE"]);
                    p.CodigoConsumidorFinal = Convert.ToInt32(Dr["CD_CONSUMIDOR_FINAL"]);
                    p.ChaveAcesso = Convert.ToString(Dr["CD_CHAVE_ACESSO"]);
                    p.CodigoModalidadeFrete = Convert.ToInt32(Dr["CD_MOD_FRETE"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos notas fiscais: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Doc_NotaFiscal> ListarDocumentosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VW_DOC_NF] ";


                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doc_NotaFiscal> lista = new List<Doc_NotaFiscal>();

                while (Dr.Read())
                {
                    Doc_NotaFiscal p = new Doc_NotaFiscal();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoOrcamento = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.DGSerieDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt64(Dr["NR_DOCUMENTO"]);
                    p.DGNumeroDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataHoraEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    if (Dr["DT_HR_SAIDA"] != DBNull.Value)
                        p.DataHoraSaida = Convert.ToDateTime(Dr["DT_HR_SAIDA"]);
                    p.ValorTotalDocumento = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.DescricaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Cpl_Pessoa = Convert.ToString(Dr["RAZ_SOCIAL"]);
                    p.Cpl_CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoGeracaoSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoNaturezaOperacao = Convert.ToInt64(Dr["CD_NAT_OPERACAO"]);
                    p.ValorICMSST = Convert.ToDecimal(Dr["VL_ST"]);
                    p.ValorFrete = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.ValorPesoBruto = Convert.ToDecimal(Dr["VL_FRETE"]);
                    p.NumeroWeb = Convert.ToDecimal(Dr["NR_WEB"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CodigoFinalidadeNF = Convert.ToInt32(Dr["CD_FINALIDADE"]);
                    p.CodigoRegimeTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"].ToString());
                    p.Cpl_Situacao = Dr["DS_SITUACAO"].ToString();
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CodigoIndicadorPresenca = Convert.ToInt32(Dr["CD_INDICADOR_PRESENCA"]);
                    p.CodigoFinalidadeNF = Convert.ToInt32(Dr["CD_FINALIDADE"]);
                    p.CodigoConsumidorFinal = Convert.ToInt32(Dr["CD_CONSUMIDOR_FINAL"]);
                    if(Dr["CD_CHAVE_ACESSO"] != DBNull.Value)
                        p.ChaveAcesso = Convert.ToString(Dr["CD_CHAVE_ACESSO"]);
                    p.CodigoModalidadeFrete = Convert.ToInt32(Dr["CD_MOD_FRETE"]);

                    if (p.CodigoSituacao != 155)
                        p.PodeImprimir = true;

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar todas notas fiscais: " + ex.Message.ToString());
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

        public void GerarLog(Doc_NotaFiscal p1, Doc_NotaFiscal p2)
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
            if (p1.ValorTotalDocumento != p2.ValorTotalDocumento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOTAL_GERAL"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorTotalDocumento + " para: " + p1.ValorTotalDocumento;
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
            if (p1.DataHoraSaida != p2.DataHoraSaida)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DT_HR_SAIDA"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                if(p1.DataHoraSaida != null && p2.DataHoraSaida != null)
                    log.DescricaoLog = "de: " + p2.DataHoraSaida.Value.ToString("dd/MM/yyyy") + " para: " + p1.DataHoraSaida.Value.ToString("dd/MM/yyyy");
                else if(p2.DataHoraSaida != null && p1.DataHoraSaida == null)
                    log.DescricaoLog = "de: " + p2.DataHoraSaida.Value.ToString("dd/MM/yyyy") + " para: Em branco" ;
                else if (p2.DataHoraSaida == null && p1.DataHoraSaida != null)
                    log.DescricaoLog = "de: Em branco. para: " + p1.DataHoraSaida.Value.ToString("dd/MM/yyyy");
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
            if (p1.ValorICMSST != p2.ValorICMSST)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_ST");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorICMSST + " para: " + p1.ValorICMSST;
            }
            if (p1.ValorPesoBruto != p2.ValorPesoBruto)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_PESO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorPesoBruto + " para: " + p1.ValorPesoBruto;
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
            if (p1.CodigoNaturezaOperacao != p2.CodigoNaturezaOperacao)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_CUBAGEM" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoNaturezaOperacao + " para: " + p1.CodigoNaturezaOperacao;
            }
            if (p1.CodigoFinalidadeNF != p2.CodigoFinalidadeNF)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_DESCONTO_MEDIO" +
                    "");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoFinalidadeNF + " para: " + p1.CodigoFinalidadeNF;
            }
            if (p1.CodigoRegimeTributario != p2.CodigoRegimeTributario)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_COMISSAO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoRegimeTributario + " para: " + p1.CodigoRegimeTributario;
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

        }

        public void EventoDocumento(Doc_NotaFiscal doc, int CodigoSituacao)
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

