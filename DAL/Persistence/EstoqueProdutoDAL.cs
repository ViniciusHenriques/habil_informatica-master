using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class EstoqueProdutoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(EstoqueProduto p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [ESTOQUE] (CD_EMPRESA, CD_LOCALIZACAO, CD_PRODUTO, CD_LOTE, QUANTIDADE, CD_SITUACAO )" +
                         " values (@v1, @v2, @v3, @v4, @v5, @v6)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoIndiceLocalizacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoLote);
                Cmd.Parameters.AddWithValue("@v5", p.Quantidade);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoSituacao);
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

                throw new Exception("Erro ao gravar PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(EstoqueProduto p)
        {
            try
            {
                AbrirConexao();

                strSQL = "update [ESTOQUE] set [CD_EMPRESA] = @v1, [CD_LOCALIZACAO] = @v2, [CD_PRODUTO] = @v3, [CD_LOTE] = @v4, [QUANTIDADE] = @v5, [CD_SITUACAO] = @v6 Where [CD_INDEX] = @v7";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v7", p.CodigoIndice);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoIndiceLocalizacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoLote);
                Cmd.Parameters.AddWithValue("@v5", p.Quantidade);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoSituacao);
                p.CodigoIndice = Convert.ToInt32(Cmd.ExecuteScalar());
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar o Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int intCodigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [ESTOQUE] Where [CD_INDEX] = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", intCodigo);

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
                            throw new Exception("Erro ao excluir PIS: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir o Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public decimal PesquisarEstoqueDisponivelProduto(Int64 CodigoProduto, int CodigoEmpresa)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_ESTOQUE_TOTAL_DISPONIVEL] Where CD_PRODUTO = @v1 AND CD_EMPRESA = @v2";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoProduto);
                Cmd.Parameters.AddWithValue("@v2", CodigoEmpresa);

                Dr = Cmd.ExecuteReader();

                decimal QuantidadeDisponivel = 0;
                if (Dr.Read())
                {
                    QuantidadeDisponivel = Convert.ToDecimal(Dr["QT_DISPONIVEL"]);
                }

                return QuantidadeDisponivel;

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
        public EstoqueProduto PesquisarEstoqueProduto(string strEstoqueProduto)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_ESTOQUE] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strEstoqueProduto);

                Dr = Cmd.ExecuteReader();

                EstoqueProduto p = null;

                if (Dr.Read())
                {
                    p = new EstoqueProduto();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDEX_LOCALIZACAO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QUANTIDADE"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NomeEmpresa = Convert.ToString(Dr["CD_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);




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
        public EstoqueProduto PesquisarEstoqueIndice(int lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_ESTOQUE] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                EstoqueProduto p = null;

                if (Dr.Read())
                {
                    p = new EstoqueProduto();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDEX_LOCALIZACAO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QUANTIDADE"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NomeEmpresa = Convert.ToString(Dr["CD_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);

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
        public List<EstoqueProduto> ListarEstoqueProduto(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from [VW_ESTOQUE]";
              
                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<EstoqueProduto> lista = new List<EstoqueProduto>();


                Habil_TipoDAL rx = new Habil_TipoDAL();
                Habil_Tipo px = new Habil_Tipo();

                while (Dr.Read())
                {
                    EstoqueProduto p = new EstoqueProduto();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDEX_LOCALIZACAO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QUANTIDADE"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NomeEmpresa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);

                    lista.Add(p);
                }

                


                return lista;

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
        public DataTable ObterEstoqueProduto(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_ESTOQUE]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoIndice", typeof(Int32));
                dt.Columns.Add("CodigoEmpresa", typeof(Int32));
                dt.Columns.Add("CodigoIndiceLocalizacao", typeof(Int32));
                dt.Columns.Add("CodigoLocalizacao", typeof(String));
                dt.Columns.Add("CodigoProduto", typeof(Int32));
                dt.Columns.Add("CodigoLote", typeof(Int32));
                dt.Columns.Add("Quantidade", typeof(Decimal));
                dt.Columns.Add("CodigoSituacao", typeof(Int32));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_INDEX"]),
                        Convert.ToInt32(Dr["CD_EMPRESA"]),
                        Convert.ToInt32(Dr["CD_INDEX_LOCALIZACAO"]),
                        Convert.ToInt32(Dr["CD_LOCALIZACAO"]),
                        Convert.ToInt32(Dr["CD_PRODUTO"]),
                        Convert.ToInt32(Dr["CD_LOTE"]),
                        Convert.ToDecimal(Dr["QUANTIDADE"]),
                        Convert.ToInt32(Dr["CD_SITUACAO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos os Estoques: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public EstoqueProduto LerEstoque(int empresa, string localizacao, int produto, int lote)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_ESTOQUE] Where CD_EMPRESA = @V1 AND CD_INDEX_LOCALIZACAO = @v2 AND CD_PRODUTO = @v3 AND CD_LOTE = @v4";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", empresa);
                Cmd.Parameters.AddWithValue("@v2", localizacao);
                Cmd.Parameters.AddWithValue("@v3", produto);
                Cmd.Parameters.AddWithValue("@v4", lote);

                Dr = Cmd.ExecuteReader();

                EstoqueProduto p = null;

                if (Dr.Read())
                {
                    p = new EstoqueProduto();


                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);

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
        public List<EstoqueProduto> ListarEstoque2(int strNomeEmpresa, int strNomeProtuto)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select CD_LOTE from [VW_ESTOQUE] Where CD_EMPRESA = @V1 AND CD_PRODUTO = @v2";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strNomeEmpresa);
                Cmd.Parameters.AddWithValue("@v2", strNomeProtuto);

                Dr = Cmd.ExecuteReader();

                List<EstoqueProduto> lista = new List<EstoqueProduto>();

                while (Dr.Read())
                {
                    EstoqueProduto p = new EstoqueProduto();

                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Estoque : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public EstoqueProduto SituacaoProtudo (int lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [PRODUTO] Where CD_SITUACAO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                EstoqueProduto p = null;

                if (Dr.Read())
                {
                    p = new EstoqueProduto();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDEX_LOCALIZACAO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QUANTIDADE"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NomeEmpresa = Convert.ToString(Dr["CD_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);

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
        public void ObterQuantidadeAvaria(int intCodEmpresa, int intCodProduto, int intCodLocalizacao, int intCodLote, ref decimal refQtdAvaria)
        {
            try
            {
                AbrirConexao();

                strSQL = "select ISNULL(SUM(QUANTIDADE),0) AS QUANTIDADE FROM [VW_ESTOQUE] WHERE CD_SITUACAO = 128  AND CD_TIPO_LOCALIZACAO = 48 ";
                strSQL = strSQL + "AND CD_EMPRESA = @v1";
                if (intCodProduto != 0)
                    strSQL = strSQL + " AND CD_PRODUTO = @v2";
                if (intCodLocalizacao != 0)
                    strSQL = strSQL + " AND CD_INDEX_LOCALIZACAO = @v3";
                if (intCodLote != 0)
                    strSQL = strSQL + " AND CD_LOTE = @v4";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodEmpresa);
                if (intCodProduto != 0)
                    Cmd.Parameters.AddWithValue("@v2", intCodProduto);
                if (intCodLocalizacao != 0)
                    Cmd.Parameters.AddWithValue("@v3", intCodLocalizacao);
                if (intCodLote != 0)
                    Cmd.Parameters.AddWithValue("@v4", intCodLote);

                Dr = Cmd.ExecuteReader();

                EstoqueProduto p = null;

                if (Dr.Read())
                {
                    p = new EstoqueProduto();

                    refQtdAvaria = Convert.ToDecimal(Dr["QUANTIDADE"]);

                }
                return;
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
        public void ObterQuantidadeTotal(int intCodEmpresa, int intCodProduto, int intCodLocalizacao, int intCodLote, ref decimal refQtdTotal)
        {
            try
            {
                AbrirConexao();

                strSQL = "select ISNULL(SUM(QUANTIDADE),0) AS QUANTIDADE from VW_ESTOQUE  WHERE CD_SITUACAO = 128";
                strSQL = strSQL + "AND CD_EMPRESA = @v1";
                if (intCodProduto != 0)
                    strSQL = strSQL + " AND CD_PRODUTO = @v2";
                if (intCodLocalizacao != 0)
                    strSQL = strSQL + " AND CD_INDEX_LOCALIZACAO = @v3";
                if (intCodLote != 0)
                    strSQL = strSQL + " AND CD_LOTE = @v4";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodEmpresa);
                if (intCodProduto != 0)
                    Cmd.Parameters.AddWithValue("@v2", intCodProduto);
                if (intCodLocalizacao != 0)
                    Cmd.Parameters.AddWithValue("@v3", intCodLocalizacao);
                if (intCodLote != 0)
                    Cmd.Parameters.AddWithValue("@v4", intCodLote);

                Dr = Cmd.ExecuteReader();

                EstoqueProduto p = null;

                if (Dr.Read())
                {
                    p = new EstoqueProduto();

                    refQtdTotal = Convert.ToDecimal(Dr["QUANTIDADE"]);

                }
                return;
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
        public void EstoqueIndiceProduto(int intCodEmpresa, int intCodProduto, int intCodLocalizacao, int intCodLote, ref decimal refQtd)
        {
            try
            {
                AbrirConexao();

                strSQL = "select QUANTIDADE from [VW_ESTOQUE] WHERE CD_EMPRESA = @v1 AND CD_PRODUTO = @v2 AND CD_INDEX_LOCALIZACAO = @v3 AND CD_LOTE = @v4";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodEmpresa);
                Cmd.Parameters.AddWithValue("@v2", intCodProduto);
                Cmd.Parameters.AddWithValue("@v3", intCodLocalizacao);
                Cmd.Parameters.AddWithValue("@v4", intCodLote);

                Dr = Cmd.ExecuteReader();

                EstoqueProduto p;

                while (Dr.Read())
                {
                    p = new EstoqueProduto();

                    refQtd = Convert.ToDecimal(Dr["QUANTIDADE"]);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Estoque : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public EstoqueProduto PesquisarEstoqueParaInventario(decimal decIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_ESTOQUE] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", decIndice);

                Dr = Cmd.ExecuteReader();

                EstoqueProduto p = null;

                if (Dr.Read())
                {
                    p = new EstoqueProduto();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoIndiceLocalizacao = Convert.ToInt32(Dr["CD_INDEX_LOCALIZACAO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoLote = Convert.ToInt32(Dr["CD_LOTE"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QUANTIDADE"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NomeEmpresa = Convert.ToString(Dr["CD_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);

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
        public DataTable RelEstoqueProduto(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = "";
                //validada

                AbrirConexao();

                strSQL = "select * from [VW_ESTOQUE]";

                if (strValor != "")
                    strSQL += " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL += " Order By " + strOrdem;

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

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
        public DataTable RelContagemPorLocalizacao (string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = "";
                //validada

                AbrirConexao();

                strSQL = "select * from [VW_ESTOQUE]";

                if (strValor != "")
                    strSQL += " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL += " Order By " + strOrdem;

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;
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
    }
}