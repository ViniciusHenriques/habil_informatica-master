using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class CategoriaVeiculoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(CategoriaVeiculo p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [Categoria_do_Veiculo] (DS_CAT_VEICULO) values (@v1)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoCategoriaVeiculo);

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
                            throw new Exception("Erro ao Incluir Categoria do Veiculo: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Categoria do Veiculo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(CategoriaVeiculo p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [Categoria_do_Veiculo] set [DS_CAT_CATEGORIA] = @v2  Where [CD_CAT_VEICULO] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoCategoriaVeiculo);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoCategoriaVeiculo);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Categoria do Veiculo: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [Categoria_do_Veiculo] Where [CD_Cat_Veiculo] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Categoria do Veiculo: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Categoria do Veiculo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public CategoriaVeiculo  PesquisarCategoriaVeiculo(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Categoria_do_Veiculo] Where CD_Cat_Veiculo = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                CategoriaVeiculo  p = null;

                if (Dr.Read())
                {
                    p = new CategoriaVeiculo ();

                    p.CodigoCategoriaVeiculo= Convert.ToInt32(Dr["CD_Cat_Veiculo"]);
                    p.DescricaoCategoriaVeiculo = Convert.ToString(Dr["DS_Cat_Veiculo"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Categoria do Veiculo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<CategoriaVeiculo> ListarCategoriaVeiculos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Categoria_do_Veiculo]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<CategoriaVeiculo> lista = new List<CategoriaVeiculo>();

                while (Dr.Read())
                {
                    CategoriaVeiculo p = new CategoriaVeiculo();

                    p.CodigoCategoriaVeiculo = Convert.ToInt32(Dr["CD_Cat_Veiculo"]);
                    p.DescricaoCategoriaVeiculo= Convert.ToString(Dr["DS_Cat_Veiculo"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Categoria do Veiculo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterCategoriaVeiculos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Categoria_do_Veiculo]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoCategoriaVeiculo", typeof(Int32));
                dt.Columns.Add("DescricaoCategoriaVeiculo", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_CategoriaVeiculo"]), Convert.ToString(Dr["DS_CategoriaVeiculo"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Categoria do Veiculos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
