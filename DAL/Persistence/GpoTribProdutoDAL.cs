using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class GpoTribProdutoDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(GpoTribProduto p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [GRUPO_TRIB_Produto] (DS_GPO_TRIB_Produto, IN_ICMS, IN_IPI) values (@v1, @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoGpoTribProduto);
                Cmd.Parameters.AddWithValue("@v2", p.ICMS);
                Cmd.Parameters.AddWithValue("@v3", p.IPI);

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
                            throw new Exception("Erro ao Incluir GpoTribProduto: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar GpoTribProduto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(GpoTribProduto p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [GRUPO_TRIB_Produto] set [DS_GPO_TRIB_Produto] = @v2, [IN_ICMS] = @v3, [IN_IPI] = @v4  Where [CD_GPO_TRIB_Produto] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoGpoTribProduto);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoGpoTribProduto);
                Cmd.Parameters.AddWithValue("@v3", p.ICMS);
                Cmd.Parameters.AddWithValue("@v4", p.IPI);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar GpoTribProduto: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [GRUPO_TRIB_Produto] Where [CD_GPO_TRIB_Produto] = @v1", Con);

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
                            throw new Exception("Erro ao excluir GpoTribProduto: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir GpoTribProduto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public GpoTribProduto  PesquisarGpoTribProduto(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [GRUPO_TRIB_Produto] Where CD_GPO_TRIB_Produto = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                GpoTribProduto  p = null;

                if (Dr.Read())
                {
                    p = new GpoTribProduto ();

                    p.CodigoGpoTribProduto= Convert.ToInt32(Dr["CD_GPO_TRIB_Produto"]);
                    p.DescricaoGpoTribProduto = Convert.ToString(Dr["DS_GPO_TRIB_Produto"]);
                    p.ICMS= Convert.ToInt32(Dr["IN_ICMS"]);
                    p.IPI = Convert.ToInt32(Dr["IN_IPI"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar GpoTribProduto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<GpoTribProduto> ListarGpoTribProdutos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [GRUPO_TRIB_Produto]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GpoTribProduto> lista = new List<GpoTribProduto>();

                while (Dr.Read())
                {
                    GpoTribProduto p = new GpoTribProduto();

                    p.CodigoGpoTribProduto = Convert.ToInt32(Dr["CD_GPO_TRIB_Produto"]);
                    p.DescricaoGpoTribProduto = Convert.ToString(Dr["DS_GPO_TRIB_Produto"]);
                    p.ICMS = Convert.ToInt32(Dr["IN_ICMS"]);
                    p.IPI = Convert.ToInt32(Dr["IN_IPI"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas GpoTribProduto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterGpoTribProdutos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [GRUPO_TRIB_Produto]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoGpoTribProduto", typeof(Int32));
                dt.Columns.Add("DescricaoGpoTribProduto", typeof(string));
                dt.Columns.Add("ICMS", typeof(Int32));
                dt.Columns.Add("IPI", typeof(Int32));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_GPO_TRIB_Produto"]), Convert.ToString(Dr["DS_GPO_TRIB_Produto"]), Convert.ToInt32(Dr["IN_ICMS"]), Convert.ToInt32(Dr["IN_IPI"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas GpoTribProdutos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<GpoTribProduto> ListarGpoTribProdutosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [GRUPO_TRIB_Produto]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GpoTribProduto> lista = new List<GpoTribProduto>();

                while (Dr.Read())
                {
                    GpoTribProduto p = new GpoTribProduto();

                    p.CodigoGpoTribProduto = Convert.ToInt32(Dr["CD_GPO_TRIB_Produto"]);
                    p.DescricaoGpoTribProduto = Convert.ToString(Dr["DS_GPO_TRIB_Produto"]);
                    p.ICMS = Convert.ToInt32(Dr["IN_ICMS"]);
                    p.IPI = Convert.ToInt32(Dr["IN_IPI"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas GpoTribProduto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
    }
}
