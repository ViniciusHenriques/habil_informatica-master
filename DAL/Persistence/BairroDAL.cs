using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class BairroDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(Bairro p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [BAIRRO] ( DS_BAIRRO) values ( @v1)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoBairro.ToUpper());

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
                            throw new Exception("Erro ao Incluir Bairro: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Bairro: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Bairro p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [BAIRRO] set [DS_BAIRRO] = @v2 Where [CD_BAIRRO] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoBairro);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoBairro);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Bairro: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [BAIRRO] Where [CD_BAIRRO] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Bairro: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Bairro: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Bairro  PesquisarBairro(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [BAIRRO] Where CD_BAIRRO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                Bairro  p = null;

                if (Dr.Read())
                {
                    p = new Bairro ();

                    p.CodigoBairro= Convert.ToInt32(Dr["CD_BAIRRO"]);
                    p.DescricaoBairro= Convert.ToString(Dr["DS_BAIRRO"]).ToUpper();
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar BAIRRO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Bairro PesquisarBairroDescricao(string strDescricao)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [BAIRRO] Where DS_BAIRRO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strDescricao);

                Dr = Cmd.ExecuteReader();

                Bairro p = new Bairro();

                if (Dr.Read())
                {
                    

                    p.CodigoBairro = Convert.ToInt32(Dr["CD_BAIRRO"]);
                    p.DescricaoBairro = Convert.ToString(Dr["DS_BAIRRO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar BAIRRO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Bairro> ListarBairros(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [BAIRRO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Bairro> lista = new List<Bairro>();

                while (Dr.Read())
                {
                    Bairro p = new Bairro();

                    p.CodigoBairro = Convert.ToInt32(Dr["CD_BAIRRO"]);
                    p.DescricaoBairro= Convert.ToString(Dr["DS_BAIRRO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Bairro: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterBairros(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [BAIRRO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoBairro", typeof(Int32));
                dt.Columns.Add("DescricaoBairro", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_BAIRRO"]), Convert.ToString(Dr["DS_BAIRRO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Bairros: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Bairro> ListarBairrosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [BAIRRO]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Bairro> lista = new List<Bairro>();

                while (Dr.Read())
                {
                    Bairro p = new Bairro();

                    p.CodigoBairro = Convert.ToInt32(Dr["CD_BAIRRO"]);
                    p.DescricaoBairro = Convert.ToString(Dr["DS_BAIRRO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Bairro: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
    }
}
