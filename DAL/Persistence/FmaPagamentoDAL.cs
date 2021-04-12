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
    public class FmaPagamentoDAL : Conexao
    {
        protected string strSQL = "";
        public void Inserir(FmaPagamento p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [FORMA_DE_PAGAMENTO] (DS_FmaPagamento, CD_FMA_PGTO_NFE, CD_BAND_CARTAO_NFE, CD_SITUACAO) values ( @v1,  @v2,  @v3,  @v4 )";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoFmaPagamento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoFmaPagamentoNFE);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoBandeiraNFE);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);

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
                            throw new Exception("Erro ao Incluir Formas de Pagamento: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Formas de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(FmaPagamento p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [FORMA_DE_PAGAMENTO] set [DS_FmaPagamento] = @v2, CD_FMA_PGTO_NFE = @v3, CD_BAND_CARTAO_NFE = @v4, CD_SITUACAO= @v5 Where [CD_FmaPagamento] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoFmaPagamento);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoFmaPagamento);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoFmaPagamentoNFE);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoBandeiraNFE);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoSituacao);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Formas de Pagamento: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [FORMA_DE_PAGAMENTO] Where [CD_FmaPagamento] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Formas de Pagamento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Formas de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public FmaPagamento  PesquisarFmasPagamento(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [FORMA_DE_PAGAMENTO] Where CD_FmaPagamento = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                FmaPagamento  p = null;

                if (Dr.Read())
                {
                    p = new FmaPagamento ();

                    p.CodigoFmaPagamento= Convert.ToInt32(Dr["CD_FmaPagamento"]);
                    p.DescricaoFmaPagamento= Convert.ToString(Dr["DS_FmaPagamento"]);
                    p.CodigoFmaPagamentoNFE = Convert.ToString(Dr["CD_FMA_PGTO_NFE"]);
                    p.CodigoBandeiraNFE= Convert.ToString(Dr["CD_BAND_CARTAO_NFE"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Formas de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<FmaPagamento> ListarFmasPagamento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [FORMA_DE_PAGAMENTO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<FmaPagamento> lista = new List<FmaPagamento>();

                while (Dr.Read())
                {
                    FmaPagamento p = new FmaPagamento();

                    p.CodigoFmaPagamento = Convert.ToInt32(Dr["CD_FmaPagamento"]);
                    p.DescricaoFmaPagamento= Convert.ToString(Dr["DS_FmaPagamento"]);
                    p.CodigoFmaPagamentoNFE = Convert.ToString(Dr["CD_FMA_PGTO_NFE"]);
                    p.CodigoBandeiraNFE = Convert.ToString(Dr["CD_BAND_CARTAO_NFE"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Formas de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterFmaPagamentos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [FORMA_DE_PAGAMENTO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoFmaPagamento", typeof(Int32));
                dt.Columns.Add("DescricaoFmaPagamento", typeof(string));
                dt.Columns.Add("CodigoFmaPagamentoNFE", typeof(string));
                dt.Columns.Add("CodigoBandeiraNFE", typeof(string));
                dt.Columns.Add("CodigoSituacao", typeof(Int32));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_FmaPagamento"]),
                                Convert.ToString(Dr["DS_FmaPagamento"]),
                                Convert.ToString(Dr["CD_FMA_PGTO_NFE"]),
                                Convert.ToString(Dr["CD_BAND_CARTAO_NFE"]),
                                Convert.ToInt32(Dr["CD_SITUACAO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Formas de Pagamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
