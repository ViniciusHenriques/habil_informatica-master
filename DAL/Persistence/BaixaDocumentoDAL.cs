using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;
namespace DAL.Persistence
{
    public class BaixaDocumentoDAL:Conexao
    {
        String strSQL = "";
        public void Inserir(decimal doc, List<BaixaDocumento> listaBaixa)
        {
            try
            {
                ExcluirTodos(doc);
                AbrirConexao();
                foreach (BaixaDocumento p in listaBaixa)
                {
                    Cmd = new SqlCommand("insert into BAIXA_DO_DOCUMENTO (CD_DOCUMENTO, " +
                                                                     "CD_BAIXA," +
                                                                     " DT_BAIXA," +
                                                                     " VL_BAIXA," +
                                                                     " DT_LANCAMENTO," +
                                                                     " VL_DESCONTO," +
                                                                     " VL_ACRESCIMO," +
                                                                     " VL_TOTAL_BAIXA," +
                                                                     " CD_TIPO_COBRANCA," +
                                                                     " CD_CTA_CORRENTE," +
                                                                     " TX_OBS," +
                                                                     " TP_BAIXA ) " +
                    "values (@v1,@v2,@v3,@v4, @v5, @v6, @v7, @v8,@v9,@v10,@v11,@v12);", Con);
                    Cmd.Parameters.AddWithValue("@v1", doc);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoBaixa);
                    Cmd.Parameters.AddWithValue("@v3", p.DataBaixa);
                    Cmd.Parameters.AddWithValue("@v4", p.ValorBaixa);
                    Cmd.Parameters.AddWithValue("@v5", p.DataLancamento);
                    Cmd.Parameters.AddWithValue("@v6", p.ValorDesconto);
                    Cmd.Parameters.AddWithValue("@v7", p.ValorAcrescimo);
                    Cmd.Parameters.AddWithValue("@v8", p.ValorTotalBaixa);
                    Cmd.Parameters.AddWithValue("@v9", p.CodigoTipoCobranca);
                    Cmd.Parameters.AddWithValue("@v10", p.CodigoContaCorrente);
                    Cmd.Parameters.AddWithValue("@v11", p.Observacao);
                    Cmd.Parameters.AddWithValue("@v12", p.TipoBaixa);

                    //Cmd.ExecuteNonQuery();
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
                            throw new Exception("Erro ao gravar baixa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar baixa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodos(decimal CodDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from BAIXA_DO_DOCUMENTO Where CD_DOCUMENTO = @v1 ", Con);
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
                            throw new Exception("Erro ao excluir baixa do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir baixa do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<BaixaDocumento> ObterBaixas(decimal CodDocumento)
        {
            try
            {

                AbrirConexao();
                string comando = "Select * from BAIXA_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);
                
                Dr = Cmd.ExecuteReader();
                List<BaixaDocumento> baixa = new List<BaixaDocumento>();

                while (Dr.Read())
                {
                    BaixaDocumento p = new BaixaDocumento();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoBaixa = Convert.ToInt32(Dr["CD_BAIXA"]);
                    p.DataBaixa = Convert.ToDateTime(Dr["DT_BAIXA"]);
                    p.ValorBaixa = Convert.ToDecimal(Dr["VL_BAIXA"]);
                    p.DataLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    p.ValorDesconto = Convert.ToDecimal(Dr["VL_DESCONTO"]);
                    p.ValorAcrescimo = Convert.ToDecimal(Dr["VL_ACRESCIMO"]);
                    p.ValorTotalBaixa = Convert.ToDecimal(Dr["VL_TOTAL_BAIXA"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoContaCorrente = Convert.ToInt32(Dr["CD_CTA_CORRENTE"]);
                    p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.TipoBaixa = Convert.ToInt32(Dr["TP_BAIXA"]);

                    TipoCobrancaDAL cobrancaDAL = new TipoCobrancaDAL();
                    TipoCobranca cobranca = new TipoCobranca();
                    cobranca = cobrancaDAL.PesquisarTipoCobranca(Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]));
                    p.Cpl_Cobranca = cobranca.DescricaoTipoCobranca;

                    baixa.Add(p);
                }
                return baixa;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar baixas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
