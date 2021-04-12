using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class MarcaDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(Marca p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [MARCA] ( DS_Marca) values ( @v1)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoMarca.ToUpper());

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
                            throw new Exception("Erro ao Incluir Marca: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Marca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }


        }
        public void Atualizar(Marca p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [MARCA] set [DS_MARCA] = @v2 Where [CD_MARCA] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoMarca);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoMarca);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Marca: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [MARCA] Where [CD_MARCA] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Marca: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Marca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Marca PesquisarMarca(short shtCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [MARCA] Where CD_MARCA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", shtCodigo);

                Dr = Cmd.ExecuteReader();

                Marca p = null;

                if (Dr.Read())
                {
                    p = new Marca();

                    p.CodigoMarca = Convert.ToInt16(Dr["CD_MARCA"]);
                    p.DescricaoMarca = Convert.ToString(Dr["DS_MARCA"]).ToUpper();
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Marca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Marca PesquisarMarcaDescricao(string strDescricao)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [MARCA] Where DS_MARCA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strDescricao);

                Dr = Cmd.ExecuteReader();

                Marca p = new Marca();

                if (Dr.Read())
                {
                    p.CodigoMarca = Convert.ToInt16(Dr["CD_MARCA"]);
                    p.DescricaoMarca = Convert.ToString(Dr["DS_MARCA"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Marca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Marca> ListarMarcas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Marca]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Marca> lista = new List<Marca>();

                while (Dr.Read())
                {
                    Marca p = new Marca();

                    p.CodigoMarca = Convert.ToInt16(Dr["CD_MARCA"]);
                    p.DescricaoMarca = Convert.ToString(Dr["DS_MARCA"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Marca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable ObterMarcas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [MARCA]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoMarca", typeof(short));
                dt.Columns.Add("DescricaoMarca", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt16(Dr["CD_MARCA"]), Convert.ToString(Dr["DS_MARCA"]));

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Marcas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Marca> ListarMarcaCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [MARCA]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Marca> lista = new List<Marca>();

                while (Dr.Read())
                {
                    Marca p = new Marca();

                    p.CodigoMarca = Convert.ToInt16(Dr["CD_MARCA"]);
                    p.DescricaoMarca = Convert.ToString(Dr["DS_MARCA"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Marca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}