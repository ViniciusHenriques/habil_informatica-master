using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class DepartamentoDAL : Conexao
    {


        protected string strSQL = "";

        public void Inserir(Departamento p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [DEPARTAMENTO] ( DS_DEPARTAMENTO, CD_SITUACAO) values ( @v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoDepartamento.ToUpper());
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);

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
                            throw new Exception("Erro ao Incluir Departamento: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Departamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Departamento p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DEPARTAMENTO] set [DS_DEPARTAMENTO] = @v2, [CD_SITUACAO] = @v3 Where [CD_DEPARTAMENTO] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoDepartamento);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoDepartamento);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Departamento: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [DEPARTAMENTO] Where [CD_DEPARTAMENTO] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Departamento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Departamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao(); 
            }
        }
        public Departamento PesquisarDepartamento(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [DEPARTAMENTO] Where CD_DEPARTAMENTO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                Departamento p = null;

                if (Dr.Read())
                {
                    p = new Departamento();

                    p.CodigoDepartamento = Convert.ToInt32(Dr["CD_DEPARTAMENTO"]);
                    p.DescricaoDepartamento = Convert.ToString(Dr["DS_DEPARTAMENTO"]).ToUpper();
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar DEPARTAMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Departamento PesquisarDepartamentoDescricao(string strDescricao)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [DEPARTAMENTO] Where DS_DEPARTAMENTO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strDescricao);

                Dr = Cmd.ExecuteReader();

                Departamento p = new Departamento();

                if (Dr.Read())
                {


                    p.CodigoDepartamento = Convert.ToInt32(Dr["CD_DEPARTAMENTO"]);
                    p.DescricaoDepartamento = Convert.ToString(Dr["DS_DEPARTAMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar DEPARTAMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Departamento> ListarDepartamento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [DEPARTAMENTO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Departamento> lista = new List<Departamento>();

                while (Dr.Read())
                {
                    Departamento p = new Departamento();

                    p.CodigoDepartamento = Convert.ToInt32(Dr["CD_DEPARTAMENTO"]);
                    p.DescricaoDepartamento = Convert.ToString(Dr["DS_DEPARTAMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos os Departamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }

            

        }
        public DataTable ObterDepartamento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [DEPARTAMENTO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoDepartamento", typeof(Int32));
                dt.Columns.Add("DescricaoDepartamento", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_DEPARTAMENTO"]),
                        Convert.ToString(Dr["DS_DEPARTAMENTO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos os Departamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao(); 

            }
        }
        public List<Departamento> ListarDepartamentoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [DEPARTAMENTO]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Departamento> lista = new List<Departamento>(); 

                while (Dr.Read())
                {
                    Departamento p = new Departamento();

                    p.CodigoDepartamento = Convert.ToInt32(Dr["CD_DEPARTAMENTO"]);
                    p.DescricaoDepartamento = Convert.ToString(Dr["DS_DEPARTAMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    lista.Add(p);

                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as Departamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
    }
}