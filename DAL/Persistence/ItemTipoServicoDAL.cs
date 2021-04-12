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
    public class ItemTipoServicoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(int CodigoTipoServico, List<ItemTipoServico> listaItemTipoServico)
        {
            try
            {
                ExcluirTodos(CodigoTipoServico);
                AbrirConexao();
                foreach (ItemTipoServico p in listaItemTipoServico)
                {
                    strSQL = "insert into ITEM_DO_TIPO_DE_SERVICO (CD_TIPO_SERVICO, CD_PRODUTO, QT_ITENS, VL_PRECO) values (@v1,@v2,@v3,@v4)";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1", CodigoTipoServico);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoProduto);
                    Cmd.Parameters.AddWithValue("@v3", p.Quantidade);
                    Cmd.Parameters.AddWithValue("@v4", p.PrecoItem);

                    Cmd.ExecuteNonQuery();
                }
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
                            throw new Exception("Erro ao Incluir Tipo de serviços: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Item do Tipo de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(ItemTipoServico p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [ITEM_DO_TIPO_DE_SERVICO] " +
                         "   set [CD_TIPO_SERVICO] = @v2 " +
                         "   , [CD_PRODUTO] = @v3, QT_ITENS= @v4, VL_PRECO = @v5" +
                         " Where [CD_ITEM_TIPO_SERVICO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoItemTipoServico);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoTipoServico);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v4", p.Quantidade);
                Cmd.Parameters.AddWithValue("@v5", p.PrecoItem);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Item do Tipo de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(Int32 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from [ITEM_DO_TIPO_DE_SERVICO] Where [CD_ITEM_TIPO_SERVICO] = @v1", Con);
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
                            throw new Exception("Erro ao excluir Item do Tipo de serviço: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Item do Tipo de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public ItemTipoServico PesquisarItemTipoServico(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [ITEM_DO_TIPO_DE_SERVICO] Where CD_ITEM_TIPO_SERVICO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                ItemTipoServico p = null;

                if (Dr.Read())
                {
                    p = new ItemTipoServico();

                    p.CodigoItemTipoServico = Convert.ToInt32(Dr["CD_ITEM_TIPO_SERVICO"]);
                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QT_ITENS"]);
                    p.PrecoItem = Convert.ToDecimal(Dr["VL_PRECO"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Itens do Tipo de Serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ItemTipoServico> ListarItemTipoServico(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [ITEM_DO_TIPO_DE_SERVICO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ItemTipoServico> lista = new List<ItemTipoServico>();

                while (Dr.Read())
                {
                    ItemTipoServico p = new ItemTipoServico();

                    p.CodigoItemTipoServico = Convert.ToInt32(Dr["CD_DO_TIPO_SERVICO"]);
                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QT_ITENS"]);
                    p.PrecoItem = Convert.ToDecimal(Dr["VL_PRECO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Itens do Tipo de Serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public List<ItemTipoServico> ListarItemTipoServicoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [ITEM_DO_TIPO_DE_SERVICO] WHERE [CD_ITEM_TIPO_SERVICO] IN ( SELECT [ITEM_DO_TIPO_DE_SERVICO].CD_ITEM_TIPO_SERVICO FROM [ITEM_DO_TIPO_DE_SERVICO]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;

                strSQL = strSQL + ")";


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ItemTipoServico> lista = new List<ItemTipoServico>();

                while (Dr.Read())
                {
                    ItemTipoServico p = new ItemTipoServico();
                    p.CodigoItemTipoServico = Convert.ToInt16(Dr["CD_ITEM_TIPO_SERVICO"]);
                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    p.CodigoProduto = Convert.ToInt16(Dr["CD_PRODUTO"]);

                    p.Quantidade = Convert.ToDecimal(Dr["QT_ITENS"]);
                    p.PrecoItem = Convert.ToDecimal(Dr["VL_PRECO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Itens do Tipo de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public void ExcluirTodos(Int64 CodTipoServico)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from ITEM_DO_TIPO_DE_SERVICO Where CD_TIPO_SERVICO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodTipoServico);
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
                            throw new Exception("Erro ao excluir Item do tipo de Servico: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Item do tipo de Servico: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<ItemTipoServico> ObterItemTipoServico(Int32 CodTipoServico)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from ITEM_DO_TIPO_DE_SERVICO Where CD_TIPO_SERVICO= @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodTipoServico);
                Dr = Cmd.ExecuteReader();
                List<ItemTipoServico> lista = new List<ItemTipoServico>();

                while (Dr.Read())
                {
                    ItemTipoServico p = new ItemTipoServico();
                    p.CodigoItemTipoServico = Convert.ToInt32(Dr["CD_ITEM_TIPO_SERVICO"]);
                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    p.Quantidade = Convert.ToDecimal(Dr["QT_ITENS"]);
                    p.PrecoItem = Convert.ToDecimal(Dr["VL_PRECO"]);

                    ProdutoDAL produtoDAL = new ProdutoDAL();
                    Produto produto = new Produto();
                    produto = produtoDAL.PesquisarProduto(Convert.ToInt32(Dr["CD_PRODUTO"]));
                    p.Cpl_DscProduto = produto.DescricaoProduto;


                    if (Dr["CD_ITEM_TIPO_SERVICO"] != DBNull.Value)
                    {
                        p.CodigoItemTipoServico= Convert.ToInt32(Dr["CD_ITEM_TIPO_SERVICO"]);
                    }

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter Itens do Tipo de Serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
