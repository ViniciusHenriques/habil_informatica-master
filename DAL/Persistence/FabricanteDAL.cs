using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class FabricanteDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(Fabricante p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [FABRICANTE] ( DS_Fabricante) values ( @v1)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoFabricante.ToUpper());

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
                            throw new Exception("Erro ao Incluir Fabricante: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Fabricante: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }


        }
        public void Atualizar(Fabricante p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [FABRICANTE] set [DS_FABRICANTE] = @v2 Where [CD_FABRICANTE] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoFabricante);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoFabricante);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Fabricante: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(short Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [FABRICANTE] Where [CD_FABRICANTE] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Fabricante: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Fabricante: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Fabricante PesquisarFabricante(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [FABRICANTE] Where CD_FABRICANTE = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                Fabricante p = null;

                if (Dr.Read())
                {
                    p = new Fabricante();

                    p.CodigoFabricante = Convert.ToInt16(Dr["CD_FABRICANTE"]);
                    p.DescricaoFabricante = Convert.ToString(Dr["DS_FABRICANTE"]).ToUpper();
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Fabricante: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Fabricante PesquisarFabricanteDescricao(string strDescricao)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [FABRICANTE] Where DS_FABRICANTE = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strDescricao);

                Dr = Cmd.ExecuteReader();

                Fabricante p = new Fabricante();

                if (Dr.Read())
                {
                    p.CodigoFabricante = Convert.ToInt16(Dr["CD_FABRICANTE"]);
                    p.DescricaoFabricante = Convert.ToString(Dr["DS_FABRICANTE"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Fabricante: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Fabricante> ListarFabricantes(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [FABRICANTE]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Fabricante> lista = new List<Fabricante>();

                while (Dr.Read())
                {
                    Fabricante p = new Fabricante();

                    p.CodigoFabricante = Convert.ToInt16(Dr["CD_FABRICANTE"]);
                    p.DescricaoFabricante = Convert.ToString(Dr["DS_FABRICANTE"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas F: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable ObterFabricante(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [FABRICANTE]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoFabricante", typeof(short));
                dt.Columns.Add("DescricaoFabricante", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt16(Dr["CD_FABRICANTE"]), Convert.ToString(Dr["DS_FABRICANTE"]));

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Fabricante: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Fabricante> ListarFabricanteCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [FABRICANTE]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Fabricante> lista = new List<Fabricante>();

                while (Dr.Read())
                {
                    Fabricante p = new Fabricante();

                    p.CodigoFabricante = Convert.ToInt16(Dr["CD_FABRICANTE"]);
                    p.DescricaoFabricante = Convert.ToString(Dr["DS_FABRICANTE"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Fabricante: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}