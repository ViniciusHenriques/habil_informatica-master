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
    public class CompradorDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(Comprador p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [COMPRADOR] (CD_PESSOA, CD_USUARIO) values ( @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v2", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoUsuario);

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
                            throw new Exception("Erro ao Incluir Comprador: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Comprador: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

                PessoaDAL x = new PessoaDAL();
                x.AtualizarPessoaComprador(p.CodigoPessoa, 1);

            }


        }
        public void Atualizar(Comprador p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [COMPRADOR] set [CD_PESSOA] = @v2, [CD_USUARIO] = @v3 Where [CD_COMPRADOR] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoComprador);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoUsuario);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Comprador: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

                PessoaDAL x = new PessoaDAL();
                x.AtualizarPessoaComprador(p.CodigoPessoa, 1);

            }


        }
        public void Excluir(Int64 Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [COMPRADOR] Where [CD_COMPRADOR] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Situação: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Situação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public Comprador PesquisarComprador(Int64  Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select [COMPRADOR].*, PESSOA.NM_PESSOA, USUARIO.NM_COMPLETO from [COMPRADOR] INNER JOIN PESSOA ON COMPRADOR.CD_PESSOA = PESSOA.CD_PESSOA INNER JOIN USUARIO ON USUARIO.CD_USUARIO = COMPRADOR.CD_USUARIO Where [COMPRADOR].CD_COMPRADOR = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Comprador p = null;

                if (Dr.Read())
                {
                    p = new Comprador();

                    p.CodigoComprador = Convert.ToInt64(Dr["CD_COMPRADOR"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoUsuario = Convert.ToInt64(Dr["CD_USUARIO"]);
                    p.NomeComprador = Convert.ToString(Dr["NM_PESSOA"]);
                
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Comprador: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Comprador> ListarCompradores(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select [COMPRADOR].*, PESSOA.NM_PESSOA, USUARIO.NM_COMPLETO from [COMPRADOR] INNER JOIN PESSOA ON COMPRADOR.CD_PESSOA = PESSOA.CD_PESSOA INNER JOIN USUARIO ON USUARIO.CD_USUARIO = COMPRADOR.CD_USUARIO ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Comprador> lista = new List<Comprador>();

                while (Dr.Read())
                {
                    Comprador p = new Comprador();

                    p.CodigoComprador = Convert.ToInt64(Dr["CD_COMPRADOR"]);
                    p.NomeComprador= Convert.ToString(Dr["NM_PESSOA"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.CodigoUsuario = Convert.ToInt64(Dr["CD_USUARIO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Compradores: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterCompradores(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select [COMPRADOR].*, PESSOA.NM_PESSOA, USUARIO.NM_COMPLETO from [COMPRADOR] INNER JOIN PESSOA ON COMPRADOR.CD_PESSOA = PESSOA.CD_PESSOA INNER JOIN USUARIO ON USUARIO.CD_USUARIO = COMPRADOR.CD_USUARIO ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoComprador", typeof(Int64));
                dt.Columns.Add("NomeComprador", typeof(string));
                dt.Columns.Add("CodigoPessoa", typeof(Int64));
                dt.Columns.Add("CodigoUsuario", typeof(Int64));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt64(Dr["CD_COMPRADOR"]), Convert.ToString(Dr["DS_COMPRADOR"]), Convert.ToInt64(Dr["CD_PESSOA"]), Convert.ToInt64(Dr["CD_USUARIO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Compradores: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
    }
}
