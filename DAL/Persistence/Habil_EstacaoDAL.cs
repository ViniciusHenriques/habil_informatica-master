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
    public class Habil_EstacaoDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(Habil_Estacao p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [Habil_Estacao] (NM_Estacao, IP_Estacao) values (@v2, @v3); SELECT SCOPE_IDENTITY() ";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoEstacao);
                Cmd.Parameters.AddWithValue("@v2", p.NomeEstacao);
                Cmd.Parameters.AddWithValue("@v3", p.IPEstacao);

                p.CodigoEstacao = Convert.ToInt64(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao Incluir Estação: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Estação: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [Habil_Estacao] Where [CD_Estacao] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Estação: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Estação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public Habil_Estacao PesquisarCodigoHabil_Estacao(long Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Habil_Estacao] Where CD_Estacao = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Habil_Estacao p = null;

                if (Dr.Read())
                {
                    p = new Habil_Estacao();

                    p.CodigoEstacao= Convert.ToInt64(Dr["CD_Estacao"]);
                    p.NomeEstacao = Convert.ToString(Dr["NM_Estacao"]);
                    p.IPEstacao = Convert.ToString(Dr["IP_Estacao"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Habil_Estacao: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Habil_Estacao PesquisarNomeHabil_Estacao(string Nome)
        {
            try
            {

                AbrirConexao();
                strSQL = "Select * from [Habil_Estacao] Where NM_Estacao = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Nome);

                Dr = Cmd.ExecuteReader();

                Habil_Estacao p = null;

                if (Dr.Read())
                {
                    p = new Habil_Estacao();

                    p.CodigoEstacao = Convert.ToInt64(Dr["CD_Estacao"]);
                    p.NomeEstacao = Convert.ToString(Dr["NM_Estacao"]);
                    p.IPEstacao = Convert.ToString(Dr["IP_Estacao"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Habil_Estacao: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
