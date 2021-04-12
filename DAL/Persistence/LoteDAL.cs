using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class LoteDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(Lote p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [LOTE] (CD_EMPRESA, CD_PRODUTO, CD_SITUACAO, NR_LOTE, SR_LOTE, DT_VALIDADE, DT_FABRICACAO , QT_LOTE )" +
                         " values (@v1, @v2, @v3, @v4, @v5, @v6, @v7,@v8)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.NumeroLote);
                Cmd.Parameters.AddWithValue("@v5", p.SerieLote);
                Cmd.Parameters.AddWithValue("@v6", p.DataValidade);
                Cmd.Parameters.AddWithValue("@v7", p.DataFabricacao);
                Cmd.Parameters.AddWithValue("@v8", p.QuantidadeLote);
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
                            throw new Exception("Erro ao Incluir Lote: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Lote p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [LOTE] set [CD_EMPRESA] = @v1, [CD_PRODUTO] = @v2, [CD_SITUACAO] = @v3, [NR_LOTE] = @v4, [SR_LOTE] = @v5, [DT_VALIDADE] = @v6 , [DT_FABRICACAO] = @v9, [QT_LOTE] = @v7 where [CD_INDEX] = @v8";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.NumeroLote);
                Cmd.Parameters.AddWithValue("@v5", p.SerieLote);
                Cmd.Parameters.AddWithValue("@v6", p.DataValidade);
                Cmd.Parameters.AddWithValue("@v9", p.DataFabricacao);
                Cmd.Parameters.AddWithValue("@v7", p.QuantidadeLote);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoIndice);
                p.CodigoIndice = Convert.ToInt32(Cmd.ExecuteScalar());
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar o Lote: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [LOTE] Where [CD_INDEX] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Lote: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir o Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Lote PesquisarLote(string strLote)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_LOTE] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strLote);

                Dr = Cmd.ExecuteReader();

                Lote p = null;

                if (Dr.Read())
                {
                    p = new Lote();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NumeroLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.SerieLote = Convert.ToString(Dr["SR_LOTE"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.DataFabricacao = Convert.ToDateTime(Dr["DT_FABRICACAO"]);
                    p.QuantidadeLote = Convert.ToDecimal(Dr["QT_LOTE"]);
                    p.Cpl_DescDDL = "LT: " + p.NumeroLote.ToString() + " - DFAB: " + p.DataFabricacao.ToString("dd/MM/yyyy") + " - DVAL: " + p.DataValidade.ToString("dd/MM/yyyy");
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Lote PesquisarLoteIndice(int lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_LOTE] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                Lote p = null;

                if (Dr.Read())
                {
                    p = new Lote();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NumeroLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.SerieLote = Convert.ToString(Dr["SR_LOTE"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.DataFabricacao = Convert.ToDateTime(Dr["DT_FABRICACAO"]);
                    p.QuantidadeLote = Convert.ToDecimal(Dr["QT_LOTE"]);
                    p.Cpl_DescDDL = "LT: " + p.NumeroLote.ToString() + " - DFAB: " + p.DataFabricacao.ToString("dd/MM/yyyy") + " - DVAL: " + p.DataValidade.ToString("dd/MM/yyyy");
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Lote> ListarLote(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from [VW_LOTE]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Lote> lista = new List<Lote>();


                Habil_TipoDAL rx = new Habil_TipoDAL();
                Habil_Tipo px = new Habil_Tipo();

                while (Dr.Read())
                {
                    Lote p = new Lote();



                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.NumeroLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.SerieLote = Convert.ToString(Dr["SR_LOTE"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.DataFabricacao = Convert.ToDateTime(Dr["DT_FABRICACAO"]);
                    p.QuantidadeLote = Convert.ToDecimal(Dr["QT_LOTE"]);
                    p.Cpl_DescDDL = "LT: " + p.NumeroLote.ToString() + " - DFAB: " + p.DataFabricacao.ToString("dd/MM/yyyy") + " - DVAL: " + p.DataValidade.ToString("dd/MM/yyyy");


                    p.NomeEmpresa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);

                    Habil_Tipo ht = new Habil_Tipo();
                    Habil_TipoDAL htDAL = new Habil_TipoDAL();

                    ht = htDAL.PesquisarHabil_Tipo(Convert.ToInt32(Dr["CD_SITUACAO"]));
                    p.DescricaoSituacao = ht.DescricaoTipo;

                    lista.Add(p);
                }




                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterLote(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
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

                //Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoIndice", typeof(Int32));
                dt.Columns.Add("CodigoEmpresa", typeof(Int32));
                dt.Columns.Add("CodigoProduto", typeof(Int32));
                dt.Columns.Add("CodigoSituacao", typeof(Int32));
                dt.Columns.Add("NumeroLote", typeof(Int32));
                dt.Columns.Add("SerieLote", typeof(Int32));
                dt.Columns.Add("DataValidade", typeof(DateTime));
                dt.Columns.Add("DataFabricacao", typeof(DateTime));
                dt.Columns.Add("QuantidadeLote", typeof(Decimal));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_INDEX"]),
                        Convert.ToInt32(Dr["CD_EMPRESA"]),
                        Convert.ToInt32(Dr["CD_PRODUTO"]),
                        Convert.ToInt32(Dr["CD_SITUACAO"]),
                        Convert.ToString(Dr["NR_LOTE"]),
                        Convert.ToString(Dr["SR_LOTE"]),
                        Convert.ToDateTime(Dr["DT_VALIDADE"]),
                        Convert.ToDateTime(Dr["DT_FABRICACAO"]),
                        Convert.ToDecimal(Dr["QT_LOTE"]));

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
        public List<Lote> ListarLote2(int strNomeEmpresa, int strNomeProtuto)

        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from [VW_LOTE] where CD_EMPRESA = @v1 and CD_PRODUTO = @v2";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strNomeEmpresa);
                Cmd.Parameters.AddWithValue("@v2", strNomeProtuto);

                Dr = Cmd.ExecuteReader();

                List<Lote> lista = new List<Lote>();


               
                while (Dr.Read())
                {
                    Lote p = new Lote();
                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.NumeroLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.DataFabricacao = Convert.ToDateTime(Dr["DT_FABRICACAO"]);
                    
                    p.Cpl_DescDDL = "LT: " + p.NumeroLote.ToString() + " - DFAB: " + p.DataFabricacao.ToString("dd/MM/yyyy") + " - DVAL: " + p.DataValidade.ToString("dd/MM/yyyy");


                    lista.Add(p);
                }




                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }

        }
        public List<Lote> ListarLoteDisponivel(int intNomeEmpresa, int intNomeProtuto, int intCodSituacao)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from [VW_LOTE] where CD_EMPRESA = @v1 and CD_PRODUTO = @v2 and CD_SITUACAO = @v3";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intNomeEmpresa);
                Cmd.Parameters.AddWithValue("@v2", intNomeProtuto);
                Cmd.Parameters.AddWithValue("@v3", intCodSituacao);

                Dr = Cmd.ExecuteReader();

                List<Lote> lista = new List<Lote>();
                while (Dr.Read())
                {
                    Lote p = new Lote();
                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.NumeroLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.DataFabricacao = Convert.ToDateTime(Dr["DT_FABRICACAO"]);

                    p.Cpl_DescDDL = "LT: " + p.NumeroLote.ToString() + " - DFAB: " + p.DataFabricacao.ToString("dd/MM/yyyy") + " - DVAL: " + p.DataValidade.ToString("dd/MM/yyyy");


                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Lote> ListarLoteDisponivelContraPartida(int intNomeEmpresa, int intNomeProtuto, int intCodSituacao)
        {
            try
            {
                AbrirConexao();

                    strSQL = "select * from [VW_LOTE] where CD_EMPRESA = @v1 and CD_SITUACAO = @v3";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intNomeEmpresa);
                Cmd.Parameters.AddWithValue("@v2", intNomeProtuto);
                Cmd.Parameters.AddWithValue("@v3", intCodSituacao);

                Dr = Cmd.ExecuteReader();

                List<Lote> lista = new List<Lote>();
                while (Dr.Read())
                {
                    Lote p = new Lote();
                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.NumeroLote = Convert.ToString(Dr["NR_LOTE"]);
                    p.DataValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.DataFabricacao = Convert.ToDateTime(Dr["DT_FABRICACAO"]);

                    p.Cpl_DescDDL = "LT: " + p.NumeroLote.ToString() + " - DFAB: " + p.DataFabricacao.ToString("dd/MM/yyyy") + " - DVAL: " + p.DataValidade.ToString("dd/MM/yyyy");

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Lote PesquisarNumeroLote(string strLote, int intProduto)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select NR_LOTE from [VW_LOTE] Where NR_LOTE = @v1 and CD_PRODUTO = @v2";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strLote);
                Cmd.Parameters.AddWithValue("@v2", intProduto);

                Dr = Cmd.ExecuteReader();

                Lote p = null;

                if (Dr.Read())
                {
                    p = new Lote();
                    p.NumeroLote = Convert.ToString(Dr["NR_LOTE"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Lote: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}