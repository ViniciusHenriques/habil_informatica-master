using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class Doc_CtaReceberDAL : Conexao
    {
        protected string strSQL = "";
        public bool Inserir(Doc_CtaReceber p, List<BaixaDocumento> listaBaixa, EventoDocumento evento, List<AnexoDocumento> anexo)
        {
            try
            {
                AbrirConexao();
                string strCampos = "CD_TIPO_DOCUMENTO";
                string strValores = "@v1";

                strCampos += ", CD_EMPRESA";
                strValores += ", @v2";

                strCampos += ", DT_HR_ENTRADA";
                strValores += ", @v3";

                strCampos += ", DT_HR_EMISSAO";
                strValores += ", @v4";

                strCampos += ", NR_DOCUMENTO";
                strValores += ", @v5";

                strCampos += ", NR_SR_DOCUMENTO";
                strValores += ", @v6";

                strCampos += ", DG_DOCUMENTO";
                strValores += ", @v7";

                strCampos += ", DG_SR_DOCUMENTO";
                strValores += ", @v8";

                strCampos += ", CD_SITUACAO";
                strValores += ", @v9";

                strCampos += ", VL_TOTAL_DOCUMENTO";
                strValores += ", @v10";

                strCampos += ", VL_TOTAL_DESCONTO";
                strValores += ", @v11";

                strCampos += ", VL_TOTAL_ACRESCIMO";
                strValores += ", @v12";

                strCampos += ", VL_TOTAL_GERAL";
                strValores += ", @v13";

                strCampos += ", OB_DOCUMENTO";
                strValores += ", @v14";

                strCampos += ", CD_TIPO_COBRANCA";
                strValores += ", @v15";

                strCampos += ", CD_CND_PAGAMENTO";
                strValores += ", @v16";

                strCampos += ", DT_ENTREGA";
                strValores += ", @v17";

                strCampos += ", CD_VENDEDOR";
                strValores += ", @v18";

                strCampos += ", CD_COMPRADOR";
                strValores += ", @v19";

                strCampos += ", CD_FMA_PAGAMENTO";
                strValores += ", @v20";

                strCampos += ", CD_MOD_FRETE";
                strValores += ", @v21";

                strCampos += ", DT_VENCIMENTO";
                strValores += ", @v22";

                strCampos += ", CD_PLANO_CONTA";
                strValores += ", @v23";

                strCampos += ", CD_CLASSIFICACAO";
                strValores += ", @v24";

                strCampos += ", CD_DOC_ORIGINAL";
                strValores += ", @v25";

                strSQL = "insert into DOCUMENTO (" + strCampos + ") values (" + strValores + "); SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v3", p.DataEntrada);
                Cmd.Parameters.AddWithValue("@v4", p.DataEmissao);
                Cmd.Parameters.AddWithValue("@v5", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v6", p.NumeroSRDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.DGDocumento);
                Cmd.Parameters.AddWithValue("@v8", p.DGSRDocumento);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v10", p.ValorDocumento);
                Cmd.Parameters.AddWithValue("@v11", p.ValorDesconto);
                Cmd.Parameters.AddWithValue("@v12", p.ValorAcrescimo);
                Cmd.Parameters.AddWithValue("@v13", p.ValorGeral);
                Cmd.Parameters.AddWithValue("@v14", p.ObservacaoDocumento);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoCobranca);
                Cmd.Parameters.AddWithValue("@v16", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v17", p.DataVencimento);
                Cmd.Parameters.AddWithValue("@v18", p.CodigoVendedor);
                Cmd.Parameters.AddWithValue("@v19", p.CodigoComprador);
                Cmd.Parameters.AddWithValue("@v20", p.CodigoFormaPagamento);
                Cmd.Parameters.AddWithValue("@v21", p.CodigoFrete);
                Cmd.Parameters.AddWithValue("@v22", p.DataVencimento);
                Cmd.Parameters.AddWithValue("@v23", p.CodigoPlanoContas);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoClassificacao);
                Cmd.Parameters.AddWithValue("@v25", p.CodigoDocumentoOriginal);

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
                            throw new Exception("Erro ao Incluir conta a receber: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar conta a receber: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirPessoaDocumento(p.CodigoPessoa, p.CodigoDocumento);

                BaixaDocumentoDAL baixaDAL = new BaixaDocumentoDAL();
                baixaDAL.Inserir(p.CodigoDocumento, listaBaixa);

                EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                eventoDAL.Inserir(evento, p.CodigoDocumento);

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, anexo);
            }
        }
        public bool Atualizar(Doc_CtaReceber p, List<BaixaDocumento> listaBaixa, EventoDocumento evento, List<AnexoDocumento> anexo)
        {
            try
            {
                Doc_CtaReceber p2 = new Doc_CtaReceber();
                p2 = PesquisarDocumento(Convert.ToDecimal(p.CodigoDocumento));
                GerarLog(p, p2);
                AbrirConexao();


                strSQL = "update DOCUMENTO set CD_TIPO_DOCUMENTO = @v1," +
                                              "CD_EMPRESA = @v2," +
                                              "DT_HR_ENTRADA = @v3," +
                                              "DT_HR_EMISSAO = @v4," +
                                              "NR_DOCUMENTO = @v5," +
                                              "NR_SR_DOCUMENTO = @v6," +
                                              "DG_DOCUMENTO = @v7," +
                                              "DG_SR_DOCUMENTO = @v8," +
                                              "CD_SITUACAO = @v9," +
                                              "VL_TOTAL_DOCUMENTO = @v10," +
                                              "VL_TOTAL_DESCONTO = @v11," +
                                              "VL_TOTAL_ACRESCIMO = @v12," +
                                              "VL_TOTAL_GERAL = @v13," +
                                              "OB_DOCUMENTO = @v14," +
                                              "CD_TIPO_COBRANCA = @v15," +
                                              "CD_CND_PAGAMENTO = @v16," +
                                              "DT_ENTREGA = @v17," +
                                              "CD_VENDEDOR = @v18," +
                                              "CD_COMPRADOR = @v19," +
                                              "CD_FMA_PAGAMENTO = @v20," +
                                              "CD_MOD_FRETE = @v21," +
                                              "DT_VENCIMENTO = @v22," +
                                              "CD_PLANO_CONTA = @v23," +
                                              "CD_CLASSIFICACAO = @v24," +
                                              "CD_DOC_ORIGINAL = @v26  where CD_DOCUMENTO = @v25;";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v25", p.CodigoDocumento);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v3", p.DataEntrada);
                Cmd.Parameters.AddWithValue("@v4", p.DataEmissao);
                Cmd.Parameters.AddWithValue("@v5", p.NumeroDocumento);
                Cmd.Parameters.AddWithValue("@v6", p.NumeroSRDocumento);
                Cmd.Parameters.AddWithValue("@v7", p.DGDocumento);
                Cmd.Parameters.AddWithValue("@v8", p.DGSRDocumento);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v10", p.ValorDocumento);
                Cmd.Parameters.AddWithValue("@v11", p.ValorDesconto);
                Cmd.Parameters.AddWithValue("@v12", p.ValorAcrescimo);
                Cmd.Parameters.AddWithValue("@v13", p.ValorGeral);
                Cmd.Parameters.AddWithValue("@v14", p.ObservacaoDocumento);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoCobranca);
                Cmd.Parameters.AddWithValue("@v16", p.CodigoCondicaoPagamento);
                Cmd.Parameters.AddWithValue("@v17", p.DataVencimento);
                Cmd.Parameters.AddWithValue("@v18", p.CodigoVendedor);
                Cmd.Parameters.AddWithValue("@v19", p.CodigoComprador);
                Cmd.Parameters.AddWithValue("@v20", p.CodigoFormaPagamento);
                Cmd.Parameters.AddWithValue("@v21", p.CodigoFrete);
                Cmd.Parameters.AddWithValue("@v22", p.DataVencimento);
                Cmd.Parameters.AddWithValue("@v23", p.CodigoPlanoContas);
                Cmd.Parameters.AddWithValue("@v24", p.CodigoClassificacao);
                Cmd.Parameters.AddWithValue("@v26", p.CodigoDocumentoOriginal);
                Cmd.ExecuteNonQuery();
                return true;
            }

            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar cta receber: " + ex.Message.ToString());

            }
            finally
            {
                AtualizarPessoaDocumento(p.CodigoPessoa, p.CodigoDocumento);
                FecharConexao();
                BaixaDocumentoDAL baixaDAL = new BaixaDocumentoDAL();
                baixaDAL.Inserir(p.CodigoDocumento, listaBaixa);

                if (evento != null)
                {
                    EventoDocumentoDAL eventoDAL = new EventoDocumentoDAL();
                    eventoDAL.Inserir(evento, p.CodigoDocumento);
                }

                AnexoDocumentoDAL AnexoDAL = new AnexoDocumentoDAL();
                AnexoDAL.Inserir(p.CodigoDocumento, anexo);
            }
        }
        public List<Doc_CtaReceber> ListarCtaReceber(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_DOC_CTA_RECEBER] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

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
                    p.CodigoVendedor = Convert.ToInt16(Dr["CD_FORNECEDOR"]);
                    //
                    p.Cpl_DsSituacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeFornecedor = Dr["RAZ_SOCIAL"].ToString();
                    p.ValorDocumento = Convert.ToDecimal(Dr["VL_TOTAL_DOCUMENTO"]);
                    p.ValorDesconto = Convert.ToDecimal(Dr["VL_TOTAL_DESCONTO"]);
                    p.ValorAcrescimo = Convert.ToDecimal(Dr["VL_TOTAL_ACRESCIMO"]);
                    p.ValorGeral = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar cONTAS A RECEBER: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public List<Doc_CtaReceber> ListarDoc_CtaReceberCompleto(List<DBTabelaCampos> ListaFiltros, int TpBaixa, int TipoCobranca, int PlanoConta)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select * from [VW_DOC_CTA_RECEBER] ";

                string strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                if (TpBaixa != 0)
                {
                    if (strValor == "")
                        strSQL = strSQL + " WHERE CD_SITUACAO = " + TpBaixa;
                    else
                        strSQL = strSQL + " AND CD_SITUACAO = " + TpBaixa;
                }
                else
                {
                    if (strValor == "")
                        strSQL = strSQL + " WHERE CD_SITUACAO != 37";
                    else
                        strSQL = strSQL + " AND CD_SITUACAO != 37";
                }

                if (TipoCobranca != 0)
                {
                    if (strSQL == "Select * from [VW_DOC_CTA_RECEBER] ")
                        strSQL = strSQL + " WHERE CD_TIPO_COBRANCA = " + TipoCobranca;
                    else
                        strSQL = strSQL + " AND CD_TIPO_COBRANCA = " + TipoCobranca;
                }

                if (PlanoConta != 0)
                {
                    if (strSQL == "Select * from [VW_DOC_CTA_RECEBER] ")
                        strSQL = strSQL + " WHERE CD_PLANO_CONTA = " + PlanoConta;
                    else
                        strSQL = strSQL + " AND CD_PLANO_CONTA= " + PlanoConta;
                }

                strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Doc_CtaReceber> lista = new List<Doc_CtaReceber>();

                while (Dr.Read())
                {
                    Doc_CtaReceber p = new Doc_CtaReceber();

                    foreach (Doc_CtaReceber item in lista)
                    {
                        if (item.CodigoDocumento == Convert.ToInt64(Dr["CD_DOCUMENTO"]))
                        {
                            p.CodigoDocumento = item.CodigoDocumento;
                            p.NumeroDocumento = item.NumeroDocumento;
                            p.DGDocumento = item.DGDocumento;
                            p.DataEmissao = item.DataEmissao;
                            p.DataEntrada = item.DataEntrada;
                            p.DataVencimento = item.DataVencimento;
                            p.CodigoSituacao = item.CodigoSituacao;
                            p.CodigoVendedor = item.CodigoVendedor;
                            p.Cpl_DsSituacao = item.Cpl_DsSituacao;
                            p.Cpl_NomeFornecedor = item.Cpl_NomeFornecedor;
                            p.ValorDocumento = item.ValorDocumento;
                            p.ValorAcrescimo = item.ValorAcrescimo;
                            p.ValorGeral = item.ValorGeral;

                            lista.RemoveAll(x => x.CodigoDocumento == item.CodigoDocumento);
                            goto GeraAlteracao;
                        }
                    }



                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.DGDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.DataEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataEntrada = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.DataVencimento = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    //p.CodigoVendedor = Convert.ToInt16(Dr["CD_FORNECEDOR"]);
                    //
                    p.Cpl_DsSituacao = Dr["DS_SITUACAO"].ToString();
                    p.Cpl_NomeFornecedor = Dr["RAZ_SOCIAL"].ToString();
                    p.ValorDocumento = Convert.ToDecimal(Dr["VL_TOTAL_DOCUMENTO"]);
                    p.ValorDesconto = Convert.ToDecimal(Dr["VL_TOTAL_DESCONTO"]);
                    p.ValorAcrescimo = Convert.ToDecimal(Dr["VL_TOTAL_ACRESCIMO"]);
                    p.ValorGeral = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);

                    p.Cpl_vlPago = Convert.ToDecimal(Dr["VL_PAGO"]);
                    p.Cpl_vlPagar = p.ValorGeral - p.Cpl_vlPago;

                    GeraAlteracao:

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Contas a pagar: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable RelCtaReceberCompleto(List<DBTabelaCampos> ListaFiltros, List<int> CodigoFiltro)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_DOC_CTA_RECEBER]  ";
                string strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                int tps = 0;
                foreach (int cd in CodigoFiltro)
                {
                    tps++;
                    if (tps == 1)
                    {
                        if (cd != 0)
                        {
                            if (strSQL == "Select * from [VW_DOC_CTA_RECEBER]  ")
                                strSQL = strSQL + " WHERE CD_TIPO_COBRANCA = " + cd;
                            else
                                strSQL = strSQL + " AND CD_TIPO_COBRANCA = " + cd;
                        }

                    }
                    else if (tps == 2)
                    {
                        if (cd != 0)
                        {
                            if (strSQL == "Select * from [VW_DOC_CTA_RECEBER]  ")
                                strSQL = strSQL + " WHERE CD_PLANO_CONTA = " + cd;
                            else
                                strSQL = strSQL + " AND CD_PLANO_CONTA= " + cd;
                        }
                    }
                    else if (tps == 3)
                    {
                        if (cd != 0)
                        {
                            if (strSQL == "Select * from [VW_DOC_CTA_RECEBER]  ")
                                strSQL = strSQL + " WHERE CD_SITUACAO = " + cd;
                            else
                                strSQL = strSQL + " AND CD_SITUACAO = " + cd;
                        }
                        else
                        {
                            if (strValor == "")
                                strSQL = strSQL + " WHERE CD_SITUACAO != 37";
                            else
                                strSQL = strSQL + " AND CD_SITUACAO != 37";
                        }

                    }
                }

                strSQL = strSQL + " ORDER BY CD_DOCUMENTO DESC ";
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
        public void InserirParcelas(List<ParcelaDocumento> lista, Doc_NotaFiscalServico doc, bool BaixaFinanceiro, IntegraDocumentoEletronico integra)
        {
            try
            {
                foreach (ParcelaDocumento p in lista)
                {
                    AbrirConexao();
                    string strCampos = "CD_TIPO_DOCUMENTO";
                    string strValores = "@v1";

                    strCampos += ", CD_EMPRESA";
                    strValores += ", @v2";

                    strCampos += ", DT_HR_ENTRADA";
                    strValores += ", @v3";

                    strCampos += ", DT_HR_EMISSAO";
                    strValores += ", @v4";

                    strCampos += ", NR_DOCUMENTO";
                    strValores += ", @v5";

                    strCampos += ", NR_SR_DOCUMENTO";
                    strValores += ", @v6";

                    strCampos += ", DG_DOCUMENTO";
                    strValores += ", @v7";

                    strCampos += ", DG_SR_DOCUMENTO";
                    strValores += ", @v8";

                    strCampos += ", CD_SITUACAO";
                    strValores += ", @v9";

                    strCampos += ", VL_TOTAL_DOCUMENTO";
                    strValores += ", @v10";

                    strCampos += ", VL_TOTAL_DESCONTO";
                    strValores += ", @v11";

                    strCampos += ", VL_TOTAL_ACRESCIMO";
                    strValores += ", @v12";

                    strCampos += ", VL_TOTAL_GERAL";
                    strValores += ", @v13";

                    strCampos += ", OB_DOCUMENTO";
                    strValores += ", @v14";

                    strCampos += ", CD_TIPO_COBRANCA";
                    strValores += ", @v15";

                    strCampos += ", CD_CND_PAGAMENTO";
                    strValores += ", @v16";

                    strCampos += ", DT_ENTREGA";
                    strValores += ", @v17";

                    strCampos += ", CD_VENDEDOR";
                    strValores += ", @v18";

                    strCampos += ", CD_COMPRADOR";
                    strValores += ", @v19";

                    strCampos += ", CD_FMA_PAGAMENTO";
                    strValores += ", @v20";

                    strCampos += ", CD_MOD_FRETE";
                    strValores += ", @v21";

                    strCampos += ", DT_VENCIMENTO";
                    strValores += ", @v22";

                    strCampos += ", CD_PLANO_CONTA";
                    strValores += ", @v23";

                    strCampos += ", CD_CLASSIFICACAO";
                    strValores += ", @v24";

                    strCampos += ", CD_DOC_ORIGINAL";
                    strValores += ", @v25";
                    strSQL = "insert into DOCUMENTO (" + strCampos + ") values (" + strValores + "); SELECT SCOPE_IDENTITY();";

                    Cmd = new SqlCommand(strSQL, Con);

                    CondPagamento cond = new CondPagamento();
                    CondPagamentoDAL condDAL = new CondPagamentoDAL();
                    cond = condDAL.PesquisarCondPagamento(doc.CodigoCondicaoPagamento);

                    Pessoa pes = new Pessoa();
                    PessoaDAL pesDAL = new PessoaDAL();
                    pes = pesDAL.PesquisarPessoa(doc.CodigoTomador);

                    Cmd.Parameters.AddWithValue("@v1", 3);
                    Cmd.Parameters.AddWithValue("@v2", doc.CodigoPrestador);
                    Cmd.Parameters.AddWithValue("@v3", doc.DataEmissao);
                    Cmd.Parameters.AddWithValue("@v4", doc.DataEmissao);
                    Cmd.Parameters.AddWithValue("@v5", 0);
                    Cmd.Parameters.AddWithValue("@v6", 0);
                    Cmd.Parameters.AddWithValue("@v7", p.DGNumeroDocumento);
                    Cmd.Parameters.AddWithValue("@v8", doc.DGSerieDocumento);
                    if(cond.CodigoTipoPagamento == 23 && BaixaFinanceiro)
                    { 
                        Cmd.Parameters.AddWithValue("@v9", 36);
                        
                    }
                    else
                    {
                        Cmd.Parameters.AddWithValue("@v9", 31);
                    }
                    
                    Cmd.Parameters.AddWithValue("@v10", p.ValorParcela);
                    Cmd.Parameters.AddWithValue("@v11", 0);
                    Cmd.Parameters.AddWithValue("@v12", 0);
                    Cmd.Parameters.AddWithValue("@v13", p.ValorParcela);
                    Cmd.Parameters.AddWithValue("@v14", doc.DescricaoGeralServico);
                    Cmd.Parameters.AddWithValue("@v15", cond.CodigoTipoCobranca);
                    Cmd.Parameters.AddWithValue("@v16", doc.CodigoCondicaoPagamento);
                    Cmd.Parameters.AddWithValue("@v17", p.DataVencimento);
                    Cmd.Parameters.AddWithValue("@v18", 0);
                    Cmd.Parameters.AddWithValue("@v19", 0);
                    Cmd.Parameters.AddWithValue("@v20", 0);
                    Cmd.Parameters.AddWithValue("@v21", 0);
                    Cmd.Parameters.AddWithValue("@v22", p.DataVencimento);
                    Cmd.Parameters.AddWithValue("@v23", pes.CodigoPlanoContas);
                    Cmd.Parameters.AddWithValue("@v24", 0);
                    Cmd.Parameters.AddWithValue("@v25", doc.CodigoNotaFiscalServico);

                    p.CodigoDocumentoPagamento = Convert.ToInt64(Cmd.ExecuteScalar());


                    EventoDocumento Evento = new EventoDocumento();
                    EventoDocumentoDAL EventoDAL = new EventoDocumentoDAL();
                    Evento.CodigoDocumento = p.CodigoDocumentoPagamento;
                    Evento.CodigoMaquina = integra.CodigoMaquina;
                    Evento.CodigoUsuario = integra.CodigoUsuario;


                    if (cond.CodigoTipoPagamento == 23 && BaixaFinanceiro)
                    {
                        List<BaixaDocumento> ListaBaixa = new List<BaixaDocumento>();
                        BaixaDocumento Baixa = new BaixaDocumento();
                        BaixaDocumentoDAL BaixaDAL = new BaixaDocumentoDAL();
                        Baixa.CodigoDocumento = p.CodigoDocumentoPagamento;
                        Baixa.CodigoTipoCobranca = cond.CodigoTipoCobranca;
                        Baixa.ValorBaixa = p.ValorParcela;
                        Baixa.ValorTotalBaixa = p.ValorParcela;
                        Baixa.DataLancamento = doc.DataLancamento;
                        Baixa.Observacao = "";
                        Baixa.DataBaixa = doc.DataEmissao;
                        Baixa.CodigoBaixa = 1;
                        ListaBaixa.Add(Baixa);
                        BaixaDAL.Inserir(p.CodigoDocumentoPagamento, ListaBaixa);

                        Evento.CodigoSituacao = 36;
                    }
                    else
                    {
                        Evento.CodigoSituacao = 31;
                    }
                    Evento.DataHoraEvento = doc.DataEmissao;
                    Evento.CodigoEvento = 1;
                    EventoDAL.Inserir(Evento, p.CodigoDocumentoPagamento);
                    
                    ParcelaDocumento par = new ParcelaDocumento();
                    ParcelaDocumentoDAL parDAL = new ParcelaDocumentoDAL();

                    par.CodigoDocumento = p.CodigoDocumento;
                    par.CodigoParcela = p.CodigoParcela;
                    par.ValorParcela = p.ValorParcela;
                    par.DataVencimento = p.DataVencimento;
                    par.DGNumeroDocumento = p.DGNumeroDocumento;
                    par.CodigoDocumentoPagamento = p.CodigoDocumentoPagamento;

                    parDAL.Atualizar(par);
                    InserirPessoaDocumento(doc.CodigoTomador, p.CodigoDocumentoPagamento);

                    
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar conta a receber: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }
        public void AtualizarParcelas(decimal doc)
        {

            try
            {

                AbrirConexao();


                strSQL = "update [DOCUMENTO] " +
                         "   set [CD_SITUACAO] = 38 " +
                         " Where [CD_DOC_ORIGINAL] = @v1 AND CD_TIPO_DOCUMENTO = 3";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", doc);


                Cmd.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar conta a receber: " + ex.Message.ToString());

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
                Cmd.Parameters.AddWithValue("@v26", 2);
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
        public Doc_CtaReceber PesquisarDocumentoOriginal(decimal CodDocumento)
        {
            try
            {
                AbrirConexao();

                string comando = "Select * from DOCUMENTO Where CD_DOCUMENTO= @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();
                Doc_CtaReceber p = new Doc_CtaReceber();

                if (Dr.Read())
                {
                    p.CodigoDocumentoOriginal = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt32(Dr["NR_DOCUMENTO"]);
                    p.DGSRDocumento = Convert.ToString(Dr["DG_SR_DOCUMENTO"]);
                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar documento original: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Doc_CtaReceber PesquisarDocumento(decimal CodDocumento)
        {
            try
            {

                int CodPessoa = PesquisarPessoaDocumento(CodDocumento);
                AbrirConexao();

                string comando = "Select * from DOCUMENTO Where CD_DOCUMENTO= @v1 ";

                if (CodDocumento == 0)
                {
                    comando = "SELECT TOP 1 * FROM DOCUMENTO ORDER BY CD_DOCUMENTO DESC ";
                }
                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();
                Doc_CtaReceber p = new Doc_CtaReceber();

                if (Dr.Read())
                {
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.NumeroDocumento = Convert.ToInt32(Dr["NR_DOCUMENTO"]);
                    p.DGDocumento = Convert.ToString(Dr["DG_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.DataEmissao = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.DataEntrada = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.DataVencimento = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoPessoa = CodPessoa;

                    p.ValorDocumento = Convert.ToDecimal(Dr["VL_TOTAL_DOCUMENTO"]);
                    p.ValorDesconto = Convert.ToDecimal(Dr["VL_TOTAL_DESCONTO"]);
                    p.ValorAcrescimo = Convert.ToDecimal(Dr["VL_TOTAL_ACRESCIMO"]);
                    p.ValorGeral = Convert.ToDecimal(Dr["VL_TOTAL_GERAL"]);
                    p.ObservacaoDocumento = Convert.ToString(Dr["OB_DOCUMENTO"]);
                    p.CodigoCondicaoPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.DataEntrega = Convert.ToDateTime(Dr["DT_ENTREGA"]);
                    p.CodigoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.DataVencimento = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
                    p.CodigoClassificacao = Convert.ToInt32(Dr["CD_CLASSIFICACAO"]);
                    p.CodigoDocumentoOriginal = Convert.ToDecimal(Dr["CD_DOC_ORIGINAL"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Conta a receber: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void GerarLog(Doc_CtaReceber p1, Doc_CtaReceber p2)
        {
            Habil_LogDAL logDAL = new Habil_LogDAL();
            DBTabelaDAL db = new DBTabelaDAL();
            long CodIdent = Convert.ToInt64(p1.CodigoDocumento);
            int CodOperacao = 5;

            if (p1.DGDocumento != p2.DGDocumento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DG_DOCUMENTO");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DGDocumento + " para: " + p1.DGDocumento;

                logDAL.Inserir(log);
            }
            if (p1.CodigoPessoa != p2.CodigoPessoa)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("PESSOA_DO_DOCUMENTO", "CD_PESSOA");
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.CodigoPessoa + " para: " + p1.CodigoPessoa;
                logDAL.Inserir(log);
            }
            if (p1.DataEmissao != p2.DataEmissao)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DT_HR_EMISSAO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DataEmissao + " para: " + p1.DataEmissao;
                logDAL.Inserir(log);
            }
            if (p1.DataVencimento != p2.DataVencimento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "DT_HR_VENCIMENTO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.DataVencimento + " para: " + p1.DataVencimento;
                logDAL.Inserir(log);
            }
            if (p1.ValorDocumento != p2.ValorDocumento)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOTAL_DOCUMENTO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorDocumento + " para: " + p1.ValorDocumento;
                logDAL.Inserir(log);
            }
            if (p1.ValorDesconto != p2.ValorDesconto)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOTAL_DESCONTO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de : " + p2.ValorDesconto + " para: " + p1.ValorDesconto;
                logDAL.Inserir(log);
            }
            if (p1.ValorAcrescimo != p2.ValorAcrescimo)
            {
                Habil_Log log = new Habil_Log();

                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOTAL_ACRESCIMO"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorAcrescimo + " para: " + p1.ValorAcrescimo;
                logDAL.Inserir(log);
            }
            if (p1.ValorGeral != p2.ValorGeral)
            {
                Habil_Log log = new Habil_Log();
                ;
                log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "VL_TOTAL_GERAL"); ;
                log.CodigoEstacao = p1.Cpl_Maquina;
                log.CodigoIdentificador = CodIdent;
                log.CodigoOperacao = CodOperacao;
                log.CodigoUsuario = p1.Cpl_Usuario;
                log.DescricaoLog = "de: " + p2.ValorGeral + " para: " + p1.ValorGeral;
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
            if (p1.CodigoPlanoContas != p2.CodigoPlanoContas)
            {
                if(p2.CodigoPlanoContas != 0)
                {
                    PlanoContas tpDoc = new PlanoContas();
                    PlanoContasDAL tpDocDAL = new PlanoContasDAL();
                    tpDoc = tpDocDAL.PesquisarPlanoConta(p1.CodigoPlanoContas);

                    PlanoContas tpDoc2 = new PlanoContas();
                    PlanoContasDAL tpDocDAL2 = new PlanoContasDAL();
                    tpDoc2 = tpDocDAL2.PesquisarPlanoConta(p2.CodigoPlanoContas);


                    Habil_Log log = new Habil_Log();

                    log.CodigoTabelaCampo = db.BuscaIDTabelaCampo("DOCUMENTO", "CD_PLANO_CONTA"); ;
                    log.CodigoEstacao = p1.Cpl_Maquina;
                    log.CodigoIdentificador = CodIdent;
                    log.CodigoOperacao = CodOperacao;
                    log.CodigoUsuario = p1.Cpl_Usuario;

                    log.DescricaoLog = "de: " + tpDoc2.DescricaoPlanoConta + " para: " + tpDoc.DescricaoPlanoConta;
                    logDAL.Inserir(log);
                }
                
            }
            if (p1.CodigoCobranca != p2.CodigoCobranca)
            {
                if(p2.CodigoCobranca != 0)
                {
                    TipoCobranca tpDoc = new TipoCobranca();
                    TipoCobrancaDAL tpDocDAL = new TipoCobrancaDAL();
                    tpDoc = tpDocDAL.PesquisarTipoCobranca(p1.CodigoCobranca);

                    TipoCobranca tpDoc2 = new TipoCobranca();
                    TipoCobrancaDAL tpDocDAL2 = new TipoCobrancaDAL();
                    tpDoc2 = tpDocDAL2.PesquisarTipoCobranca(p2.CodigoCobranca);


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
                tpDoc.DescricaoTipo = tpDocDAL.DescricaoHabil_Tipo(Convert.ToInt32(p2.CodigoClassificacao));

                Habil_Tipo tpDoc2 = new Habil_Tipo();
                Habil_TipoDAL tpDocDAL2 = new Habil_TipoDAL();
                tpDoc2.DescricaoTipo = tpDocDAL2.DescricaoHabil_Tipo(Convert.ToInt32(p1.CodigoClassificacao));


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
                                                        "DS_BAIRRO = @v40 where CD_DOCUMENTO = @v25";
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
                Cmd.Parameters.AddWithValue("@v26", 2);
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
                            throw new Exception("Erro ao Incluir conta a receber: " + ex.Message.ToString());

                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar conta a receber: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }        
    }
}
