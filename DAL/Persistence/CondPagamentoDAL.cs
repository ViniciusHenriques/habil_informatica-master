using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class CondPagamentoDAL : Conexao
    {
        protected string strSQL = "";
        public void Inserir(CondPagamento p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [CONDICAO_DE_PAGAMENTO] ([DS_CND_PAGAMENTO],[CD_SITUACAO],[CD_FmaPagamento],[CD_TIPO_COBRANCA],[TP_CND_PAGAMENTO],[DIA_FIXO],[QT_PARCELAS]";
                strSQL += " ,[NR_PARCELA_1] ,[NR_PARCELA_2] ,[NR_PARCELA_3] ,[NR_PARCELA_4] ,[NR_PARCELA_5] ,[NR_PARCELA_6] ,[NR_PARCELA_7] ,[NR_PARCELA_8] ,[NR_PARCELA_9] ,[NR_PARCELA_10] ";
                strSQL += " ,[NR_PROPORCAO_1] ,[NR_PROPORCAO_2] ,[NR_PROPORCAO_3] ,[NR_PROPORCAO_4] ,[NR_PROPORCAO_5] ,[NR_PROPORCAO_6] ,[NR_PROPORCAO_7] ,[NR_PROPORCAO_8] ,[NR_PROPORCAO_9] ,[NR_PROPORCAO_10]) ";
                strSQL += " values ( @v1,  @v2,  @v3,  @v4, @v5,  @v6,  @v7,  @v8, @v9,  @v10,  @v11,  @v12, @v13,  @v14,  @v15,  @v16, @v17,  @v18,  @v19,  @v20, @v21,  @v22,  @v23,  @v24, @v25,  @v26,  @v27)";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoCondPagamento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoFmaPagamento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoTipoPagamento);
                Cmd.Parameters.AddWithValue("@v6", p.DiaFixo);
                Cmd.Parameters.AddWithValue("@v7", p.QtdeParcelas);
                Cmd.Parameters.AddWithValue("@v8", p.Parcelas1);
                Cmd.Parameters.AddWithValue("@v9", p.Parcelas2);
                Cmd.Parameters.AddWithValue("@v10", p.Parcelas3);
                Cmd.Parameters.AddWithValue("@v11", p.Parcelas4);
                Cmd.Parameters.AddWithValue("@v12", p.Parcelas5);
                Cmd.Parameters.AddWithValue("@v13", p.Parcelas6);
                Cmd.Parameters.AddWithValue("@v14", p.Parcelas7);
                Cmd.Parameters.AddWithValue("@v15", p.Parcelas8);
                Cmd.Parameters.AddWithValue("@v16", p.Parcelas9);
                Cmd.Parameters.AddWithValue("@v17", p.Parcelas10);
                Cmd.Parameters.AddWithValue("@v18", p.Proporcao1);
                Cmd.Parameters.AddWithValue("@v19", p.Proporcao2);
                Cmd.Parameters.AddWithValue("@v20", p.Proporcao3);
                Cmd.Parameters.AddWithValue("@v21", p.Proporcao4);
                Cmd.Parameters.AddWithValue("@v22", p.Proporcao5);
                Cmd.Parameters.AddWithValue("@v23", p.Proporcao6);
                Cmd.Parameters.AddWithValue("@v24", p.Proporcao7);
                Cmd.Parameters.AddWithValue("@v25", p.Proporcao8);
                Cmd.Parameters.AddWithValue("@v26", p.Proporcao9);
                Cmd.Parameters.AddWithValue("@v27", p.Proporcao10);

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
                            throw new Exception("Erro ao Incluir Condições de Pagamento: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Condições de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(CondPagamento p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [CONDICAO_DE_PAGAMENTO] set [DS_CND_PAGAMENTO] = @v1,[CD_SITUACAO] = @v2,[CD_FmaPagamento] = @v3,[CD_TIPO_COBRANCA] = @v4,[TP_CND_PAGAMENTO] = @v5,[DIA_FIXO] = @v6,[QT_PARCELAS] = @v7";
                strSQL += " ,[NR_PARCELA_1]  = @v8,[NR_PARCELA_2] = @v9 ,[NR_PARCELA_3] = @v10 ,[NR_PARCELA_4] = @v11 ,[NR_PARCELA_5] = @v12 ,[NR_PARCELA_6] = @v13 ,[NR_PARCELA_7] = @v14 ,[NR_PARCELA_8] = @v15 ,[NR_PARCELA_9] = @v16 ,[NR_PARCELA_10] = @v17 ";
                strSQL += " ,[NR_PROPORCAO_1] = @v18 ,[NR_PROPORCAO_2] = @v19 ,[NR_PROPORCAO_3] = @v20 ,[NR_PROPORCAO_4] = @v21 ,[NR_PROPORCAO_5] = @v22 ,[NR_PROPORCAO_6] = @v23 ,[NR_PROPORCAO_7] = @v24 ,[NR_PROPORCAO_8] = @v25 ,[NR_PROPORCAO_9] = @v26 ,[NR_PROPORCAO_10] = @v27 ";
                strSQL += "Where [CD_CND_PAGAMENTO] = @v28";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.DescricaoCondPagamento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoFmaPagamento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoTipoPagamento);
                Cmd.Parameters.AddWithValue("@v6", p.DiaFixo);
                Cmd.Parameters.AddWithValue("@v7", p.QtdeParcelas);
                Cmd.Parameters.AddWithValue("@v8", p.Parcelas1);
                Cmd.Parameters.AddWithValue("@v9", p.Parcelas2);
                Cmd.Parameters.AddWithValue("@v10", p.Parcelas3);
                Cmd.Parameters.AddWithValue("@v11", p.Parcelas4);
                Cmd.Parameters.AddWithValue("@v12", p.Parcelas5);
                Cmd.Parameters.AddWithValue("@v13", p.Parcelas6);
                Cmd.Parameters.AddWithValue("@v14", p.Parcelas7);
                Cmd.Parameters.AddWithValue("@v15", p.Parcelas8);
                Cmd.Parameters.AddWithValue("@v16", p.Parcelas9);
                Cmd.Parameters.AddWithValue("@v17", p.Parcelas10);
                Cmd.Parameters.AddWithValue("@v18", p.Proporcao1);
                Cmd.Parameters.AddWithValue("@v19", p.Proporcao2);
                Cmd.Parameters.AddWithValue("@v20", p.Proporcao3);
                Cmd.Parameters.AddWithValue("@v21", p.Proporcao4);
                Cmd.Parameters.AddWithValue("@v22", p.Proporcao5);
                Cmd.Parameters.AddWithValue("@v23", p.Proporcao6);
                Cmd.Parameters.AddWithValue("@v24", p.Proporcao7);
                Cmd.Parameters.AddWithValue("@v25", p.Proporcao8);
                Cmd.Parameters.AddWithValue("@v26", p.Proporcao9);
                Cmd.Parameters.AddWithValue("@v27", p.Proporcao10);
                Cmd.Parameters.AddWithValue("@v28", p.CodigoCondPagamento);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Condições de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [CONDICAO_DE_PAGAMENTO] Where [CD_CND_PAGAMENTO] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Condições de Pagamento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Condições de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public CondPagamento  PesquisarCondPagamento(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [CONDICAO_DE_PAGAMENTO] Where CD_CND_PAGAMENTO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                CondPagamento  p = null;

                if (Dr.Read())
                {
                    p = new CondPagamento ();

                    p.CodigoCondPagamento= Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.DescricaoCondPagamento= Convert.ToString(Dr["DS_CND_PAGAMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoFmaPagamento = Convert.ToInt32(Dr["CD_FMAPAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoTipoPagamento= Convert.ToInt32(Dr["TP_CND_PAGAMENTO"]);
                    p.DiaFixo = Convert.ToInt32(Dr["DIA_FIXO"]);
                    p.QtdeParcelas = Convert.ToInt32(Dr["QT_PARCELAS"]);

                    p.Parcelas1 = Convert.ToInt32(Dr["NR_PARCELA_1"]);
                    p.Proporcao1 = Convert.ToDecimal(Dr["NR_PROPORCAO_1"]);
                    p.Parcelas2 = Convert.ToInt32(Dr["NR_PARCELA_2"]);
                    p.Proporcao2 = Convert.ToDecimal(Dr["NR_PROPORCAO_2"]);
                    p.Parcelas3 = Convert.ToInt32(Dr["NR_PARCELA_3"]);
                    p.Proporcao3 = Convert.ToDecimal(Dr["NR_PROPORCAO_3"]);
                    p.Parcelas4 = Convert.ToInt32(Dr["NR_PARCELA_4"]);
                    p.Proporcao4 = Convert.ToDecimal(Dr["NR_PROPORCAO_4"]);
                    p.Parcelas5 = Convert.ToInt32(Dr["NR_PARCELA_5"]);
                    p.Proporcao5 = Convert.ToDecimal(Dr["NR_PROPORCAO_5"]);
                    p.Parcelas6 = Convert.ToInt32(Dr["NR_PARCELA_6"]);
                    p.Proporcao6 = Convert.ToDecimal(Dr["NR_PROPORCAO_6"]);
                    p.Parcelas7 = Convert.ToInt32(Dr["NR_PARCELA_7"]);
                    p.Proporcao7 = Convert.ToDecimal(Dr["NR_PROPORCAO_7"]);
                    p.Parcelas8 = Convert.ToInt32(Dr["NR_PARCELA_8"]);
                    p.Proporcao8 = Convert.ToDecimal(Dr["NR_PROPORCAO_8"]);
                    p.Parcelas9 = Convert.ToInt32(Dr["NR_PARCELA_9"]);
                    p.Proporcao9 = Convert.ToDecimal(Dr["NR_PROPORCAO_9"]);
                    p.Parcelas10 = Convert.ToInt32(Dr["NR_PARCELA_10"]);
                    p.Proporcao10 = Convert.ToDecimal(Dr["NR_PROPORCAO_10"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Condições de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<CondPagamento> ListarCondPagamento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [CONDICAO_DE_PAGAMENTO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<CondPagamento> lista = new List<CondPagamento>();

                while (Dr.Read())
                {
                    CondPagamento p = new CondPagamento();

                    p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.DescricaoCondPagamento = Convert.ToString(Dr["DS_CND_PAGAMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoFmaPagamento = Convert.ToInt32(Dr["CD_FMAPAGAMENTO"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoTipoPagamento = Convert.ToInt32(Dr["TP_CND_PAGAMENTO"]);
                    p.DiaFixo = Convert.ToInt32(Dr["DIA_FIXO"]);
                    p.QtdeParcelas = Convert.ToInt32(Dr["QT_PARCELAS"]);

                    p.Parcelas1 = Convert.ToInt32(Dr["NR_PARCELA_1"]);
                    p.Proporcao1 = Convert.ToDecimal(Dr["NR_PROPORCAO_1"]);
                    p.Parcelas2 = Convert.ToInt32(Dr["NR_PARCELA_2"]);
                    p.Proporcao2 = Convert.ToDecimal(Dr["NR_PROPORCAO_2"]);
                    p.Parcelas3 = Convert.ToInt32(Dr["NR_PARCELA_3"]);
                    p.Proporcao3 = Convert.ToDecimal(Dr["NR_PROPORCAO_3"]);
                    p.Parcelas4 = Convert.ToInt32(Dr["NR_PARCELA_4"]);
                    p.Proporcao4 = Convert.ToDecimal(Dr["NR_PROPORCAO_4"]);
                    p.Parcelas5 = Convert.ToInt32(Dr["NR_PARCELA_5"]);
                    p.Proporcao5 = Convert.ToDecimal(Dr["NR_PROPORCAO_5"]);
                    p.Parcelas6 = Convert.ToInt32(Dr["NR_PARCELA_6"]);
                    p.Proporcao6 = Convert.ToDecimal(Dr["NR_PROPORCAO_6"]);
                    p.Parcelas7 = Convert.ToInt32(Dr["NR_PARCELA_7"]);
                    p.Proporcao7 = Convert.ToDecimal(Dr["NR_PROPORCAO_7"]);
                    p.Parcelas8 = Convert.ToInt32(Dr["NR_PARCELA_8"]);
                    p.Proporcao8 = Convert.ToDecimal(Dr["NR_PROPORCAO_8"]);
                    p.Parcelas9 = Convert.ToInt32(Dr["NR_PARCELA_9"]);
                    p.Proporcao9 = Convert.ToDecimal(Dr["NR_PROPORCAO_9"]);
                    p.Parcelas10 = Convert.ToInt32(Dr["NR_PARCELA_10"]);
                    p.Proporcao10 = Convert.ToDecimal(Dr["NR_PROPORCAO_10"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Condições de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterCondPagamentos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [CONDICAO_DE_PAGAMENTO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoCondPagamento", typeof(Int32));
                dt.Columns.Add("DescricaoCondPagamento", typeof(string));
                dt.Columns.Add("CodigoSituacao", typeof(Int32));
                dt.Columns.Add("CodigoFmaPagamento", typeof(Int32));
                dt.Columns.Add("CodigoTipoCobranca", typeof(Int32));
                dt.Columns.Add("CodigoTipoPagamento", typeof(Int32));
                dt.Columns.Add("DiaFixo", typeof(Int32));
                dt.Columns.Add("QtdeParcelas", typeof(Int32));

                dt.Columns.Add("Parcela1", typeof(Int32));
                dt.Columns.Add("Proporcao1", typeof(Decimal));
                dt.Columns.Add("Parcela2", typeof(Int32));
                dt.Columns.Add("Proporcao2", typeof(Decimal));
                dt.Columns.Add("Parcela3", typeof(Int32));
                dt.Columns.Add("Proporcao3", typeof(Decimal));
                dt.Columns.Add("Parcela4", typeof(Int32));
                dt.Columns.Add("Proporcao4", typeof(Decimal));
                dt.Columns.Add("Parcela5", typeof(Int32));
                dt.Columns.Add("Proporcao5", typeof(Decimal));
                dt.Columns.Add("Parcela6", typeof(Int32));
                dt.Columns.Add("Proporcao6", typeof(Decimal));
                dt.Columns.Add("Parcela7", typeof(Int32));
                dt.Columns.Add("Proporcao7", typeof(Decimal));
                dt.Columns.Add("Parcela8", typeof(Int32));
                dt.Columns.Add("Proporcao8", typeof(Decimal));
                dt.Columns.Add("Parcela9", typeof(Int32));
                dt.Columns.Add("Proporcao9", typeof(Decimal));
                dt.Columns.Add("Parcela10", typeof(Int32));
                dt.Columns.Add("Proporcao10", typeof(Decimal));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]),
                                Convert.ToString(Dr["DS_CND_PAGAMENTO"]),
                                Convert.ToInt32(Dr["CD_SITUACAO"]),
                                Convert.ToInt32(Dr["CD_FMAPAGAMENTO"]),
                                Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]),
                                Convert.ToInt32(Dr["TP_CND_PAGAMENTO"]),
                                Convert.ToInt32(Dr["DIA_FIXO"]),
                                Convert.ToInt32(Dr["QT_PARCELAS"]),
                                Convert.ToInt32(Dr["NR_PARCELA1"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO1"]),
                                Convert.ToInt32(Dr["NR_PARCELA2"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO2"]),
                                Convert.ToInt32(Dr["NR_PARCELA3"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO3"]),
                                Convert.ToInt32(Dr["NR_PARCELA4"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO4"]),
                                Convert.ToInt32(Dr["NR_PARCELA5"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO5"]),
                                Convert.ToInt32(Dr["NR_PARCELA6"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO6"]),
                                Convert.ToInt32(Dr["NR_PARCELA7"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO7"]),
                                Convert.ToInt32(Dr["NR_PARCELA8"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO8"]),
                                Convert.ToInt32(Dr["NR_PARCELA9"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO9"]),
                                Convert.ToInt32(Dr["NR_PARCELA10"]),
                                Convert.ToDecimal(Dr["NR_PROPORCAO10"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Condições de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
