using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class VendedorDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(Vendedor p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [VENDEDOR] (CD_PESSOA, CD_SITUACAO, CD_EMPRESA, PC_COMISSAO,CD_TIPO_VENDEDOR) " +
                         "values (@CD_PESSOA, @CD_SITUACAO, @CD_EMPRESA, @PC_COMISSAO, @CD_TIPO_VENDEDOR)";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@CD_PESSOA", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@CD_SITUACAO", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@CD_EMPRESA", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@PC_COMISSAO", p.PercentualComissao);
                Cmd.Parameters.AddWithValue("@CD_TIPO_VENDEDOR", p.CodigoTipoVendedor);
                Cmd.ExecuteNonQuery();


                PessoaDAL x = new PessoaDAL();
                x.AtualizarPessoaVendedor(p.CodigoPessoa,1);


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
                            throw new Exception("Erro ao gravar Vendedor: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Vendedor: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void Atualizar(Vendedor p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [VENDEDOR] set " +
                                "[CD_PESSOA]    = @CD_PESSOA," +
                                "[CD_SITUACAO]  = @CD_SITUACAO," +
                                "[CD_EMPRESA]   = @CD_EMPRESA," +
                                "[CD_TIPO_VENDEDOR]  = @CD_TIPO_VENDEDOR, " +
                                "[PC_COMISSAO]  = @PC_COMISSAO " +
                                "where [CD_VENDEDOR] = @CD_VENDEDOR";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@CD_VENDEDOR", p.CodigoVendedor);
                Cmd.Parameters.AddWithValue("@CD_PESSOA", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@CD_SITUACAO", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@CD_EMPRESA", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@PC_COMISSAO", p.PercentualComissao);
                Cmd.Parameters.AddWithValue("@CD_TIPO_VENDEDOR", p.CodigoTipoVendedor);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Vendedor: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void Excluir(long Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [VENDEDOR] Where [CD_VENDEDOR] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Vendedor: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Vendedor: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public Vendedor PesquisarVendedor(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "select * from [VENDEDOR] inner join [PESSOA] on VENDEDOR.CD_PESSOA = PESSOA.CD_PESSOA where VENDEDOR.CD_VENDEDOR = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Vendedor p = null;

                if (Dr.Read())
                {
                    p = new Vendedor(0,0);
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoSituacao = Convert.ToInt64(Dr["CD_SITUACAO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.PercentualComissao = Convert.ToDecimal(Dr["PC_COMISSAO"]);
                    p.Pessoa.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.CodigoTipoVendedor = Convert.ToInt64(Dr["CD_TIPO_VENDEDOR"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Vendedor: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public Vendedor PesquisarVendedorPessoa(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "select VENDEDOR.*,PESSOA.NM_PESSOA from [VENDEDOR] inner join [PESSOA] on VENDEDOR.CD_PESSOA = PESSOA.CD_PESSOA where VENDEDOR.CD_PESSOA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Vendedor p = null;

                if (Dr.Read())
                {
                    p = new Vendedor(0, 0);
                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_VENDEDOR"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoSituacao = Convert.ToInt64(Dr["CD_SITUACAO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.PercentualComissao = Convert.ToDecimal(Dr["PC_COMISSAO"]);
                    p.Pessoa.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.CodigoTipoVendedor = Convert.ToInt64(Dr["CD_TIPO_VENDEDOR"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Vendedor: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Vendedor> ListarRepresentantes()
        {
            try
            {
                AbrirConexao();

                string strSQL = "select v.CD_VENDEDOR, v.CD_PESSOA, p.NM_PESSOA,ENDE.DS_MUNICIPIO + '/' +"+
                                    "SUBSTRING(ENDE.DS_ESTADO, 0, 3) + ' - ' + PAIS.DS_PAIS AS MUNICIPIO_PAIS " +
                                "from VENDEDOR AS V " +
                                    "INNER JOIN PESSOA AS P " +
                                    "INNER JOIN PESSOA_ENDERECO AS ENDE ON ENDE.CD_PESSOA = P.CD_PESSOA AND ENDE.CD_ENDERECO = 1 " +
                                        "ON P.CD_PESSOA = V.CD_PESSOA " +
                                    "INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = P.CD_PESSOA AND INS.CD_INSCRICAO = 1 "+
                                    "INNER JOIN PAIS ON PAIS.CD_PAIS = INS.CD_PAIS " +
                                "WHERE V.CD_TIPO_VENDEDOR = 118";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Vendedor> lista = new List<Vendedor>();

                while (Dr.Read())
                {
                    Vendedor p = new Vendedor();

                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_Vendedor"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.Pessoa.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]) + " - " + Dr["MUNICIPIO_PAIS"].ToString().ToUpper(); 

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos representantes: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Vendedor> ListarVendedores(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = " select V.CD_VENDEDOR, V.CD_EMPRESA, V.CD_SITUACAO AS CD_SIT_VENDEDOR, V.PC_COMISSAO, V.CD_TIPO_VENDEDOR, P.*" +
                                " from VENDEDOR AS V " +
                                " INNER JOIN PESSOA AS P " +
                                "   ON P.CD_PESSOA = V.CD_PESSOA  ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Vendedor> lista = new List<Vendedor>();

                while (Dr.Read())
                {
                    Vendedor p = new Vendedor();

                    p.CodigoVendedor = Convert.ToInt64(Dr["CD_Vendedor"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoSituacao = Convert.ToInt64(Dr["CD_SIT_VENDEDOR"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoTipoVendedor = Convert.ToInt64(Dr["CD_TIPO_VENDEDOR"]);

                    p.Pessoa.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.PercentualComissao = Convert.ToDecimal(Dr["PC_COMISSAO"]);
                    p.Pessoa.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.Pessoa.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
                    p.Pessoa.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
                    p.Pessoa.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.Pessoa.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]); 
                    p.Pessoa.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
                    p.Pessoa.CodHabil_RegTributario= Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.Pessoa.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
                    p.Pessoa.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
                    p.Pessoa.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
                    p.Pessoa.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
                    p.Pessoa.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
                    p.Pessoa.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
                    p.Pessoa.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
                    p.Pessoa.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.Pessoa.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
                    p.Pessoa.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
                    p.Pessoa.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                           //CD_SIS_ANTERIOR

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Vendedores: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable ObterVendedores(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Vendedor]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                SqlDataAdapter da = new SqlDataAdapter(strSQL, Con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Vendedores: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
    }
}
