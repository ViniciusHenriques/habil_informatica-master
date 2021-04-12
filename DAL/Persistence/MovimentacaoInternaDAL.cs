using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class MovimentacaoInternaDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(MovimentacaoInterna p)
        {
            DBTabelaDAL dbTDAL = new DBTabelaDAL();

            try
            {
                AbrirConexao();

                strSQL = "insert into [MOVIMENTACAO_DE_ESTOQUE] (CD_EMPRESA, CD_LOCALIZACAO, CD_LOTE, CD_PRODUTO, CD_TIPO_OPERACAO, TP_OPER, CD_USUARIO, CD_MAQUINA, CD_DOCUMENTO,VL_AJUSTE , NR_DOCUMENTO, VL_UNITARIO, VL_SALDO_ANTERIOR, QT_MOVIMENTADA, VL_SALDO_FINAL )" +
                         " values (@v1, @v2, @v3, @v4, @v5, @v6, @v7,@v8, @v9, @v10, @v12, @v13, @v14, @v15, @v16 ); SELECT SCOPE_IDENTITY()";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoIndiceLocalizacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoLote);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v6", p.TpOperacao);
                Cmd.Parameters.AddWithValue("@v7", p.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoMaquina);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v10", p.VlSaldoAjuste);
                Cmd.Parameters.AddWithValue("@v12", p.NumeroDoc);
                Cmd.Parameters.AddWithValue("@v13", p.ValorUnitario);
                Cmd.Parameters.AddWithValue("@v14", p.ValorSaldoAnterior);
                Cmd.Parameters.AddWithValue("@v15", p.QtMovimentada);
                Cmd.Parameters.AddWithValue("@v16", p.VlSaldoFinal);
                p.CodigoIndice = Convert.ToInt32(Cmd.ExecuteScalar());


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
                            throw new Exception("Erro ao Incluir Estoque: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Movimentação de Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                ExecutaSpAtualizaEstoque(p.CodigoIndice, Convert.ToDateTime(dbTDAL.ObterDataHoraServidor().ToString("yyyy-MM-dd")));
            }
        }
        public MovimentacaoInterna PesquisarMovimentacao(string strMovimentacaoEstoque)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_MOV_ESTOQUE] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strMovimentacaoEstoque);

                Dr = Cmd.ExecuteReader();

                MovimentacaoInterna p = null;

                if (Dr.Read())
                {
                    p = new MovimentacaoInterna();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDICE_LOCALIZACAO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.TpOperacao = Convert.ToString(Dr["TP_OPER"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.DtLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    p.DtValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.NumeroDoc = Convert.ToString(Dr["NR_DOCUMENTO"]);
                    p.ValorUnitario = Convert.ToDecimal(Dr["VL_UNITARIO"]);
                    p.ValorSaldoAnterior = Convert.ToDecimal(Dr["VL_SALDO_ANTERIOR"]);
                    p.QtMovimentada = Convert.ToDecimal(Dr["QT_MOVIMENTADA"]);
                    p.VlSaldoFinal = Convert.ToDecimal(Dr["VL_SALDO_FINAL"]);
                    p.NrLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.NomeEmpresa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_ESTACAO"]);
                    p.NomeMaquina = Convert.ToString(Dr["LOGIN"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Movimentação de Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public MovimentacaoInterna PesquisarMovIndice(decimal lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_MOV_ESTOQUE] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                MovimentacaoInterna p = null;

                if (Dr.Read())
                {
                    p = new MovimentacaoInterna();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDICE_LOCALIZACAO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.TpOperacao = Convert.ToString(Dr["TP_OPER"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.DtLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    p.DtValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.NumeroDoc = Convert.ToString(Dr["NR_DOCUMENTO"]);
                    p.ValorUnitario = Convert.ToDecimal(Dr["VL_UNITARIO"]);
                    p.ValorSaldoAnterior = Convert.ToDecimal(Dr["VL_SALDO_ANTERIOR"]);
                    p.QtMovimentada = Convert.ToDecimal(Dr["QT_MOVIMENTADA"]);
                    p.VlSaldoFinal = Convert.ToDecimal(Dr["VL_SALDO_FINAL"]);
                    p.NrLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.NomeEmpresa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_ESTACAO"]);
                    p.NomeMaquina = Convert.ToString(Dr["LOGIN"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Movimentação de Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<MovimentacaoInterna> ListarMovimentacaoEstoque(int intTop,int intCodEmpresa, DateTime dteDataInicio, DateTime dtrDataFim,
            int intCodProduto, int intCodOperacao, int intCodLocalizacao, int intCodLote, string strNrDocumento)
        {
            try
            {
                AbrirConexao();
                if (intTop != 0)
                    strSQL = "Select TOP " + intTop + " * from [VW_MOV_ESTOQUE]";
                else
                {
                    strSQL = "Select * from [VW_MOV_ESTOQUE]";
                }

                strSQL = strSQL + "where CD_EMPRESA = @v1" +
                    " AND DT_LANCAMENTO >= @v2 and DT_LANCAMENTO <= @v3";
                if (intCodProduto != 0)
                    strSQL = strSQL + " AND CD_PRODUTO = @v4";
                if (intCodOperacao != 0)
                    strSQL = strSQL + " AND CD_TIPO_OPERACAO = @v5";
                if (intCodLocalizacao != 0)
                    strSQL = strSQL + " AND CD_INDICE_LOCALIZACAO = @v6";
                if (intCodLote != 0)
                    strSQL = strSQL + " AND CD_LOTE = @v7";
                if (strNrDocumento != "")
                    strSQL = strSQL + " AND NR_DOCUMENTO LIKE '%" + strNrDocumento + "%'";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodEmpresa);
                Cmd.Parameters.AddWithValue("@v2", Convert.ToDateTime(dteDataInicio.ToString("dd/MM/yyyy 00:00:00")));
                Cmd.Parameters.AddWithValue("@v3", Convert.ToDateTime(dtrDataFim.ToString("dd/MM/yyyy 23:59:59")));
                if (intCodProduto != 0)
                    Cmd.Parameters.AddWithValue("@v4", intCodProduto);
                if (intCodOperacao != 0)
                    Cmd.Parameters.AddWithValue("@v5", intCodOperacao);
                if (intCodLocalizacao != 0)
                    Cmd.Parameters.AddWithValue("@v6", intCodLocalizacao);
                if (intCodLote != 0)
                    Cmd.Parameters.AddWithValue("@v7", intCodLote);

                Dr = Cmd.ExecuteReader();

                List<MovimentacaoInterna> lista = new List<MovimentacaoInterna>();

                while (Dr.Read())
                {
                    MovimentacaoInterna p = new MovimentacaoInterna();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDICE_LOCALIZACAO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.TpOperacao = Convert.ToString(Dr["TP_OPER"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.DtLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);

                    //p.DtValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);

                    p.NumeroDoc = Convert.ToString(Dr["NR_DOCUMENTO"]);
                    p.ValorUnitario = Convert.ToDecimal(Dr["VL_UNITARIO"]);
                    p.ValorSaldoAnterior = Convert.ToDecimal(Dr["VL_SALDO_ANTERIOR"]);
                    p.QtMovimentada = Convert.ToDecimal(Dr["QT_MOVIMENTADA"]);
                    p.VlSaldoFinal = Convert.ToDecimal(Dr["VL_SALDO_FINAL"]);
                    p.NrLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.NomeEmpresa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_ESTACAO"]);
                    p.NomeMaquina = Convert.ToString(Dr["LOGIN"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Movimentações de Estoques: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void ObterQuantidadeTpOperacoes(int intTop,int intCodEmpresa, DateTime dteDataInicio, DateTime dtrDataFim,
            int intCodProduto, int intCodOperacao, int intCodLocalizacao, int intCodLote, string strNrDocumento, ref decimal decQtdSaida,
            ref decimal decQtdEntrada, ref decimal decQtdAjuste)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select TP_OPER, ISNULL(SUM(QT_MOVIMENTADA), 0)AS QT_MOVIMENTADA from[VW_MOV_ESTOQUE]";
                    
                strSQL = strSQL + "where CD_EMPRESA = @v1" +
                " AND DT_LANCAMENTO >= @v2 and DT_LANCAMENTO <= @v3";
                if (intCodProduto != 0)
                    strSQL = strSQL + " AND CD_PRODUTO = @v4";
                if (intCodOperacao != 0)
                    strSQL = strSQL + " AND CD_TIPO_OPERACAO = @v5";
                if (intCodLocalizacao != 0)
                    strSQL = strSQL + " AND CD_INDICE_LOCALIZACAO = @v6";
                if (intCodLote != 0)
                    strSQL = strSQL + " AND CD_LOTE = @v7";
                if (strNrDocumento != "")
                    strSQL = strSQL + " AND NR_DOCUMENTO LIKE '%" + strNrDocumento + "%'";

                strSQL = strSQL +  " GROUP BY TP_OPER";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodEmpresa);
                Cmd.Parameters.AddWithValue("@v2", Convert.ToDateTime(dteDataInicio.ToString("dd/MM/yyyy 00:00:00")));
                Cmd.Parameters.AddWithValue("@v3", Convert.ToDateTime(dtrDataFim.ToString("dd/MM/yyyy 23:59:59")));
                if (intCodProduto != 0)
                    Cmd.Parameters.AddWithValue("@v4", intCodProduto);
                if (intCodOperacao != 0)
                    Cmd.Parameters.AddWithValue("@v5", intCodOperacao);
                if (intCodLocalizacao != 0)
                    Cmd.Parameters.AddWithValue("@v6", intCodLocalizacao);
                if (intCodLote != 0)
                    Cmd.Parameters.AddWithValue("@v7", intCodLote);

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    string TpOperacao = Convert.ToString(Dr["TP_OPER"]);

                    if (TpOperacao == "E")
                    {
                        decQtdEntrada = Convert.ToDecimal(Dr["QT_MOVIMENTADA"]);
                    }
                    if (TpOperacao == "A")
                    {
                        decQtdAjuste = Convert.ToDecimal(Dr["QT_MOVIMENTADA"]);
                    }
                    if (TpOperacao == "S")
                    {
                        decQtdSaida = Convert.ToDecimal(Dr["QT_MOVIMENTADA"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Movimentações de Estoques: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<MovimentacaoInterna> ListarMovimentacaoEstoque2(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from [MV_MOV_ESTOQUE]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<MovimentacaoInterna> lista = new List<MovimentacaoInterna>();


                Habil_TipoDAL rx = new Habil_TipoDAL();
                Habil_Tipo px = new Habil_Tipo();

                while (Dr.Read())
                {
                    MovimentacaoInterna p = new MovimentacaoInterna();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDICE_LOCALIZACAO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.TpOperacao = Convert.ToString(Dr["TP_OPER"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.DtLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    p.DtValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.NumeroDoc = Convert.ToString(Dr["NR_DOCUMENTO"]);
                    p.ValorUnitario = Convert.ToDecimal(Dr["VL_UNITARIO"]);
                    p.ValorSaldoAnterior = Convert.ToDecimal(Dr["VL_SALDO_ANTERIOR"]);
                    p.QtMovimentada = Convert.ToDecimal(Dr["QT_MOVIMENTADA"]);
                    p.VlSaldoFinal = Convert.ToDecimal(Dr["VL_SALDO_FINAL"]);
                    p.NrLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.NomeEmpresa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.NomeUsuario = Convert.ToString(Dr["NM_ESTACAO"]);
                    p.NomeMaquina = Convert.ToString(Dr["LOGIN"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Movimentações de Estoques: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public DataTable ObterMovimentacaoEstoque(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_MOV_ESTOQUE]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                //Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoIndice", typeof(Decimal));
                dt.Columns.Add("CodigoEmpresa", typeof(Int32));
                dt.Columns.Add("CodigoIndiceLocalizacao", typeof(Int32));
                dt.Columns.Add("CodigoLote", typeof(Int32));
                dt.Columns.Add("CodigoProduto", typeof(Int32));
                dt.Columns.Add("CodigoTipoOperacao", typeof(Int32));
                dt.Columns.Add("TpOperacao", typeof(String));
                dt.Columns.Add("CodigoUsuario", typeof(Int32));
                dt.Columns.Add("CodigoMaquina", typeof(Int32));
                dt.Columns.Add("CodigoDocumento", typeof(Int32));
                dt.Columns.Add("DtLancamento", typeof(DateTime));
                dt.Columns.Add("DtValidade", typeof(DateTime));
                dt.Columns.Add("NumeroDoc", typeof(String));
                dt.Columns.Add("ValorUnitario", typeof(Decimal));
                dt.Columns.Add("ValorSaldoAnterior", typeof(Decimal));
                dt.Columns.Add("QtMovimentada", typeof(Decimal));
                dt.Columns.Add("VlSaldoFinal", typeof(Decimal));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToDecimal(Dr["CD_INDEX"]),
                        Convert.ToInt32(Dr["CD_EMPRESA"]),
                        Convert.ToInt32(Dr["CD_INDICE_LOCALIZACAO"]),
                        Convert.ToInt32(Dr["CD_LOTE"]),
                        Convert.ToInt32(Dr["CD_PRODUTO"]),
                        Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]),
                        Convert.ToString(Dr["TP_OPER"]),
                        Convert.ToInt32(Dr["CD_USUARIO"]),
                Convert.ToInt32(Dr["CD_MAQUINA"]),
                Convert.ToInt32(Dr["CD_DOCUMENTO"]),
                Convert.ToDateTime(Dr["DT_LANCAMENTO"]),
                Convert.ToDateTime(Dr["DT_VALIDADE"]),
                Convert.ToString(Dr["NR_DOCUMENTO"]),
                Convert.ToDecimal(Dr["VL_UNITARIO"]),
                Convert.ToDecimal(Dr["VL_SALDO_ANTERIOR"]),
                Convert.ToDecimal(Dr["QT_MOVIMENTADA"]),
                Convert.ToDecimal(Dr["VL_SALDO_FINAL"]));

                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos as movimentações de estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public MovimentacaoInterna LerSaldoAnterior(int empresa, int localizacao, int produto, int lote)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select TOP 1 * from [MOVIMENTACAO_DE_ESTOQUE] Where CD_EMPRESA = @V1 AND CD_LOCALIZACAO = @v2 AND CD_PRODUTO = @v3 AND CD_LOTE = @v4 AND DT_LANCAMENTO < GETDATE() ORDER BY DT_LANCAMENTO DESC";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", empresa);
                Cmd.Parameters.AddWithValue("@v2", localizacao);
                Cmd.Parameters.AddWithValue("@v3", produto);
                Cmd.Parameters.AddWithValue("@v4", lote);

                Dr = Cmd.ExecuteReader();

                MovimentacaoInterna p = null;

                if (Dr.Read())
                {
                    p = new MovimentacaoInterna();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_LOCALIZACAO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.TpOperacao = Convert.ToString(Dr["TP_OPER"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.DtLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    //p.DtValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.NumeroDoc = Convert.ToString(Dr["NR_DOCUMENTO"]);
					if(Dr["VL_UNITARIO"] != DBNull.Value)
						p.ValorUnitario = Convert.ToDecimal(Dr["VL_UNITARIO"]);

                    p.ValorSaldoAnterior = Convert.ToDecimal(Dr["VL_SALDO_ANTERIOR"]);
                    p.QtMovimentada = Convert.ToDecimal(Dr["QT_MOVIMENTADA"]);
                    p.VlSaldoFinal = Convert.ToDecimal(Dr["VL_SALDO_FINAL"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool ExecutaSpAtualizaEstoque(decimal CodIndice, DateTime DtRef)
        {
            bool blnRetorno = false;
            AbrirConexao();
            try
            {
                SqlCommand sqlComand = new SqlCommand("[dbo].[SP_Atualiza_Estoque]", Con);

                sqlComand.CommandType = CommandType.StoredProcedure;
                sqlComand.Parameters.AddWithValue("@CD_INDEX", CodIndice);
                sqlComand.Parameters.AddWithValue("@DT_REFERENCIA", DtRef);


                
                sqlComand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Executar Sp Atualiza Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                blnRetorno = true;
            }
            return blnRetorno;
        }
        public DataTable RelMovEstoque(int intCodEmpresa, DateTime dteDataInicio, DateTime dtrDataFim,
            int intCodProduto, int intCodOperacao, int intCodLocalizacao, int intCodLote, string strNrDocumento, int tpOper)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = "";
                //validada
                
                strSQL = "Select * from [VW_MOV_ESTOQUE]";

                strSQL = strSQL + " where CD_EMPRESA = " + intCodEmpresa + " " +
                    " AND DT_LANCAMENTO >= '" + dteDataInicio.ToString("yyyy-MM-dd 00:00:00") + "' " +
                    " AND DT_LANCAMENTO <= '" + dtrDataFim.ToString("yyyy-MM-dd 23:59:59") + "' ";
                if (intCodProduto != 0)
                    strSQL = strSQL + " AND CD_PRODUTO = " + intCodProduto + " ";
                if (tpOper == 1)
                {
                    if (intCodOperacao != 0)
                        strSQL = strSQL + " AND CD_TIPO_OPERACAO = " + intCodOperacao + " ";
                }
                if(tpOper == 2)
                    strSQL = strSQL + " AND TP_OPER = 'E' ";
                if (tpOper == 3)
                    strSQL = strSQL + " AND TP_OPER = 'S' ";
                if (intCodLocalizacao != 0)
                    strSQL = strSQL + " AND CD_INDICE_LOCALIZACAO = " + intCodLocalizacao + " ";
                if (intCodLote != 0)
                    strSQL = strSQL + " AND CD_LOTE = " + intCodLote + " ";
                if (strNrDocumento != "")
                    strSQL = strSQL + " AND NR_DOCUMENTO LIKE '%" + strNrDocumento + "%'";
                
                strSQL += " ORDER BY CD_EMPRESA, CD_LOCALIZACAO, CD_PRODUTO, CD_LOTE, DT_LANCAMENTO ";

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
        public DataTable RelPosicaoEstoque(int intCodEmpresa, DateTime dteDataRef, int intCodProduto, int intCodLocalizacao1, int intCodLocalizacao2)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = "";
                //validada

                strSQL = "SELECT * FROM VW_MOV_ESTOQUE WHERE CD_INDEX IN " +
                    " (SELECT MAX(CD_INDEX) AS CD_INDEX " +
                    " FROM VW_MOV_ESTOQUE WHERE DT_LANCAMENTO <= '" + dteDataRef.ToString("yyyy-MM-dd 23:59:59.999") + "' " +
                    " AND CD_EMPRESA = " + intCodEmpresa; 

                if(intCodLocalizacao1 != 0)
                    strSQL += " AND CD_INDICE_LOCALIZACAO >= " + intCodLocalizacao1.ToString();

                if (intCodLocalizacao2 != 0)
                    strSQL += " AND CD_INDICE_LOCALIZACAO <= " + intCodLocalizacao2.ToString();

                if (intCodProduto != 0)
                    strSQL += " AND CD_PRODUTO = " + intCodProduto.ToString();

                    strSQL += " GROUP BY CD_EMPRESA, CD_INDICE_LOCALIZACAO, CD_PRODUTO, CD_LOTE) ";

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
    }
}