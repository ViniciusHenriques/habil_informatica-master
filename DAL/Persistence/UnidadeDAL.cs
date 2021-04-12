using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class UnidadeDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(Unidade p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [UNIDADE] (CD_UNIDADE,DS_UNIDADE, SIGLA) values ( (SELECT (ISNULL(Max(CD_UNIDADE),0) + 1) FROM UNIDADE ) ,@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoUnidade);
                Cmd.Parameters.AddWithValue("@v2", p.SiglaUnidade);

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
                            throw new Exception("Erro ao Incluir Unidade: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Unidade: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Unidade p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [Unidade] set [DS_Unidade] = @v2, [Sigla] = @v3  Where [CD_Unidade] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoUnidade);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoUnidade);
                Cmd.Parameters.AddWithValue("@v3", p.SiglaUnidade);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Unidade: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [Unidade] Where [CD_Unidade] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Unidade: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Unidade: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Unidade  PesquisarUnidade(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Unidade] Where CD_Unidade = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                Unidade  p = null;

                if (Dr.Read())
                {
                    p = new Unidade ();

                    p.CodigoUnidade= Convert.ToInt32(Dr["CD_Unidade"]);
                    p.DescricaoUnidade = Convert.ToString(Dr["DS_Unidade"]);
                    p.SiglaUnidade = Convert.ToString(Dr["SIGLA"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Unidade: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Unidade> ListarUnidades(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Unidade]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Unidade> lista = new List<Unidade>();

                while (Dr.Read())
                {
                    Unidade p = new Unidade();

                    p.CodigoUnidade = Convert.ToInt32(Dr["CD_Unidade"]);
                    p.DescricaoUnidade= Convert.ToString(Dr["DS_Unidade"]);
                    p.SiglaUnidade = Convert.ToString(Dr["SIGLA"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Unidade: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterUnidades(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Unidade]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoUnidade", typeof(Int32));
                dt.Columns.Add("DescricaoUnidade", typeof(string));
                dt.Columns.Add("SiglaUnidade", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_Unidade"]), Convert.ToString(Dr["DS_Unidade"]), Convert.ToString(Dr["SIGLA"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Unidades: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
