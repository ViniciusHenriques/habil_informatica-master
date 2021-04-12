using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class CargoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(Cargo p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [CARGO] (DS_CARGO) values (@v1)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoCargo.ToUpper());

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
                            throw new Exception("Erro ao Incluir Cargo: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Cargo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Cargo p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [CARGO] set [DS_CARGO] = @v2 Where [CD_CARGO] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoCargo);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoCargo);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Cargo: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [CARGO] Where [CD_CARGO] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Cargo: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Cargo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Cargo PesquisarCargo(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [CARGO] Where CD_CARGO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                Cargo p = null;

                if (Dr.Read())
                {
                    p = new Cargo();

                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);
                    p.DescricaoCargo = Convert.ToString(Dr["DS_CARGO"]).ToUpper();
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Cargo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Cargo PesquisarCargoDescricao(string strDescricao)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [CARGO] Where DS_CARGO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strDescricao);

                Dr = Cmd.ExecuteReader();

                Cargo p = new Cargo();

                if (Dr.Read())
                {


                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);
                    p.DescricaoCargo = Convert.ToString(Dr["DS_CARGO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Cargo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Cargo> ListarCargos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [CARGO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Cargo> lista = new List<Cargo>();

                while (Dr.Read())
                {
                    Cargo p = new Cargo();

                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);
                    p.DescricaoCargo = Convert.ToString(Dr["DS_CARGO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Cargo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public DataTable ObterCargos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [CARGO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoCargo", typeof(Int32));
                dt.Columns.Add("DescricaoCargo", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_CARGO"]), Convert.ToString(Dr["DS_CARGO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Cargos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Cargo> ListarCargosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [CARGO]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Cargo> lista = new List<Cargo>();

                while (Dr.Read())
                {
                    Cargo p = new Cargo();

                    p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);
                    p.DescricaoCargo = Convert.ToString(Dr["DS_CARGO"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Cargo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
