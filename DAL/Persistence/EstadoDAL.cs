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
    public class EstadoDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(Estado p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [ESTADO] (CD_ESTADO, SIGLA, DS_SITUACAO) values (@v1, @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoEstado);
                Cmd.Parameters.AddWithValue("@v2", p.Sigla);
                Cmd.Parameters.AddWithValue("@v3", p.DescricaoEstado);

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
                            throw new Exception("Erro ao gravar Estado: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Estado: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void Atualizar(Estado p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [Estado] set [Sigla] = @v2, [DS_Estado] = @v3, Where [CD_Estado] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoEstado);
                Cmd.Parameters.AddWithValue("@v2", p.Sigla);
                Cmd.Parameters.AddWithValue("@v3", p.DescricaoEstado);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Estado: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [Estado] Where [CD_Estado] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Estado: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Estado: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public Estado PesquisarEstado(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Estado] Where CD_ESTADO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Estado p = null;

                if (Dr.Read())
                {
                    p = new Estado();

                    p.CodigoEstado = Convert.ToInt32(Dr["CD_Estado"]);
                    p.Sigla = Convert.ToString(Dr["Sigla"]);
                    p.DescricaoEstado = Convert.ToString(Dr["DS_Estado"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Estado: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Estado PesquisarEstadoUF(string Sigla)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Estado] Where Sigla = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Sigla);

                Dr = Cmd.ExecuteReader();


                Estado p = new Estado();
                if (Dr.Read())
                {

                    
                    p.CodigoEstado = Convert.ToInt32(Dr["CD_Estado"]);
                    p.Sigla = Convert.ToString(Dr["Sigla"]);
                    p.DescricaoEstado = Convert.ToString(Dr["DS_Estado"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Estado: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Estado> ListarEstados(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Estado]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Estado> lista = new List<Estado>();

                while (Dr.Read())
                {
                    Estado p = new Estado();

                    p.CodigoEstado = Convert.ToInt32(Dr["CD_Estado"]);
                    p.DescricaoEstado = Convert.ToString(Dr["DS_Estado"]);
                    p.Sigla = Convert.ToString(Dr["Sigla"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Estados: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterEstados(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Estado]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoEstado", typeof(Int32));
                dt.Columns.Add("Sigla", typeof(string));
                dt.Columns.Add("DescricaoEstado", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_Estado"]), Convert.ToString(Dr["Sigla"]), Convert.ToString(Dr["DS_Estado"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Estados: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public DataTable ObterEstadosSigla()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Estado]";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoEstado", typeof(Int32));
                dt.Columns.Add("DescricaoEstado", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_Estado"]), Convert.ToString(Dr["Sigla"]) + " - " + Convert.ToString(Dr["DS_Estado"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Estados: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public DataTable ObterEstadosSiglaDaEmpresa()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Estado] Where CD_ESTADO in (Select CD_ESTADO FROM VW_EMPRESA) ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoEstado", typeof(Int32));
                dt.Columns.Add("DescricaoEstado", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_Estado"]), Convert.ToString(Dr["Sigla"]) + " - " + Convert.ToString(Dr["DS_Estado"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Estados: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
