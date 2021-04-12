using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class VeiculoDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(Veiculo p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [VEICULO] (PLACA, NM_VEICULO, CD_CAT_VEICULO) values (@v1, @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.Placa.ToUpper());
                Cmd.Parameters.AddWithValue("@v2", p.NomeVeiculo);
                Cmd.Parameters.AddWithValue("@v3", p.CategoriaVeiculo);

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
                            throw new Exception("Erro ao Incluir Veiculo: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Veiculo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Veiculo p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [VEICULO] set [NM_VEICULO] = @v2, [PLACA] = @v3, [CD_CAT_VEICULO] = @v4 Where [CD_VEICULO] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoVeiculo);
                Cmd.Parameters.AddWithValue("@v2", p.NomeVeiculo);
                Cmd.Parameters.AddWithValue("@v3", p.Placa);
                Cmd.Parameters.AddWithValue("@v4", p.CategoriaVeiculo);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Veiculo: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [VEICULO] Where [CD_VEICULO] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Veiculo: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Veiculo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Veiculo  PesquisarVeiculo(long intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Veiculo] Where CD_Veiculo = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                Veiculo  p = null;

                if (Dr.Read())
                {
                    p = new Veiculo();

                    p.CodigoVeiculo = Convert.ToInt64(Dr["CD_VEICULO"]);
                    p.NomeVeiculo = Convert.ToString(Dr["NM_VEICULO"]);
                    p.Placa = Convert.ToString(Dr["PLACA"]);
                    p.CategoriaVeiculo = Convert.ToInt32(Dr["CD_CAT_VEICULO"]);
                    p.NomeVeiculoCombo = p.Placa + " - " + p.NomeVeiculo;
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Veiculo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Veiculo> ListarVeiculos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VEICULO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Veiculo> lista = new List<Veiculo>();

                while (Dr.Read())
                {
                    Veiculo p = new Veiculo();

                    p.CodigoVeiculo = Convert.ToInt64(Dr["CD_VEICULO"]);
                    p.NomeVeiculo = Convert.ToString(Dr["NM_VEICULO"]);
                    p.Placa = Convert.ToString(Dr["PLACA"]);
                    p.CategoriaVeiculo = Convert.ToInt32(Dr["CD_CAT_VEICULO"]);
                    p.NomeVeiculoCombo = p.Placa + " - " + p.NomeVeiculo;
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Veiculos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public List<Veiculo> ListarVeiculoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VEICULO]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Veiculo> lista = new List<Veiculo>();

                while (Dr.Read())
                {
                    Veiculo p = new Veiculo();

                    p.CodigoVeiculo = Convert.ToInt64(Dr["CD_VEICULO"]);
                    p.NomeVeiculo = Convert.ToString(Dr["NM_VEICULO"]);
                    p.Placa = Convert.ToString(Dr["PLACA"]);
                    p.CategoriaVeiculo = Convert.ToInt32(Dr["CD_CAT_VEICULO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Veiculos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterVeiculos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VEICULO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoVeiculo", typeof(long));
                dt.Columns.Add("Placa", typeof(string));
                dt.Columns.Add("NomeVeiculo", typeof(string));
                dt.Columns.Add("CategoriaVeiculo", typeof(int));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt64(Dr["CD_VEICULO"]), Convert.ToString(Dr["PLACA"]), Convert.ToString(Dr["NM_VEICULO"]), Convert.ToInt32(Dr["CD_CAT_VEICULO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Veiculos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
    }
}
