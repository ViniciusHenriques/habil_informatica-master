using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class CategoriaDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(Categoria p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [Categoria] (CD_Categoria, DS_Categoria, CD_GPO_COMISSAO, CD_DEPARTAMENTO) values (@v1, @v2, @v3, @v4)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoCategoria.ToUpper());
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoCategoria);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoGpoComissao);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoDepartamento);

                p.CodigoIndice = Convert.ToInt32(Cmd.ExecuteScalar());


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
                            throw new Exception("Erro ao Incluir Categoria: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Categoria: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Categoria p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [Categoria] set [DS_Categoria] = @v2, [CD_GPO_COMISSAO] = @v3 ,[CD_DEPARTAMENTO] = @v4 Where [CD_Categoria] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoCategoria);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoCategoria);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoGpoComissao);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoDepartamento);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Categoria: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int intCodigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [Categoria] Where [CD_index] = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", intCodigo);

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
                            throw new Exception("Erro ao excluir Categoria: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Categoria: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Categoria PesquisarCategoria(string strCategoria)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Vw_Categoria] Where CD_Categoria = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strCategoria);

                Dr = Cmd.ExecuteReader();

                Categoria p = null;

                if (Dr.Read())
                {
                    p = new Categoria();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCategoria = Convert.ToString(Dr["CD_Categoria"]);
                    p.DescricaoCategoria = Convert.ToString(Dr["DS_Categoria"]);
                    p.CodigoDepartamento = Convert.ToInt16(Dr["CD_DEPARTAMENTO"]);
                    p.CodigoGpoComissao = Convert.ToInt16(Dr["CD_GPO_COMISSAO"]);
                    p.DescricaoDepartamento = Dr["DS_DEPARTA"].ToString();
                    p.DescricaoGpoComissao = Dr["DS_GPO"].ToString();
                    p.Cpl_DescDDL = p.CodigoCategoria.ToString() + " - " + p.DescricaoCategoria.ToString();
                }
            

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Categoria: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Categoria PesquisarCategoriaIndice(int lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_Categoria] Where CD_Index = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                Categoria p = null;

                if (Dr.Read())
                {
                    p = new Categoria();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCategoria = Convert.ToString(Dr["CD_Categoria"]);
                    p.DescricaoCategoria = Convert.ToString(Dr["DS_Categoria"]);
                    p.CodigoDepartamento = Convert.ToInt16(Dr["CD_DEPARTAMENTO"]);
                    p.CodigoGpoComissao = Convert.ToInt16(Dr["CD_GPO_COMISSAO"]);
                    p.DescricaoDepartamento = Dr["DS_DEPARTA"].ToString();
                    p.DescricaoGpoComissao = Dr["DS_GPO"].ToString();
                    p.Cpl_DescDDL = p.CodigoCategoria.ToString() + " - " + p.DescricaoCategoria.ToString();
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Categoria: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Categoria> ListarCategorias(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Vw_Categoria]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Categoria> lista = new List<Categoria>();

                while (Dr.Read())
                {
                    Categoria p = new Categoria();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCategoria = Convert.ToString(Dr["CD_CATEGORIA"]);
                    p.DescricaoCategoria= Dr["DS_Categoria"].ToString();
                    p.CodigoDepartamento = Convert.ToInt16(Dr["CD_DEPARTAMENTO"]);
                    p.CodigoGpoComissao = Convert.ToInt16(Dr["CD_GPO_COMISSAO"]);
                    p.DescricaoDepartamento = Dr["DS_DEPARTA"].ToString();
                    p.DescricaoGpoComissao = Dr["DS_GPO"].ToString();
                    p.Cpl_DescDDL = p.CodigoCategoria.ToString() + " - " + p.DescricaoCategoria.ToString();
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Categoria: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterCategorias(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Vw_Categoria]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoIndice", typeof(int));
                dt.Columns.Add("CodigoCategoria", typeof(string));
                dt.Columns.Add("DescricaoCategoria", typeof(string));
                dt.Columns.Add("CodiggoDepartamento", typeof(int));
                dt.Columns.Add("CodigoGpoComissao", typeof(int));
                dt.Columns.Add("DescricaoDepartamento", typeof(string));
                dt.Columns.Add("DescricaoGpoComissao", typeof(string));
                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_INDEX"]),
                        Convert.ToString(Dr["CD_Categoria"]),
                        Convert.ToString(Dr["DS_Categoria"]),
                        Convert.ToInt32(Dr["CD_DEPARTAMENTO"]),
                        Convert.ToInt32(Dr["CD_GPO_COMISSAO"]),
                        Convert.ToString(Dr["DS_DEPARTA"]),
                        Convert.ToString(Dr["DS_GPO"])); 

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Categorias: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
