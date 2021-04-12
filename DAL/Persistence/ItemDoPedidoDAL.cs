using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class ItemDoPedidoDAL : Conexao
    {

        public List<ItemDoPedido> ListarItensDoPedido(Int64 NroPedido)
        {
            try
            {
                //
                AbrirConexao();
                string strSQL = "Select * from [Produto] Where CD_CATEGORIA = " + Convert.ToString(NroPedido) ;
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                List<ItemDoPedido> lista = new List<ItemDoPedido>();

                while (Dr.Read())
                {
                    ItemDoPedido p = new ItemDoPedido();
                    //p.CodigoProduto = Convert.ToInt64(Dr["CD_Produto"]);
                    //p.DescricaoProduto = Dr["NM_PRODUTO"].ToString();
                    //p.CodigoCategoria = Convert.ToDouble(Dr["CD_CATEGORIA"]);
                    //p.DescricaoCategoria = "TESTE";
                    //p.ValorVenda = Convert.ToDouble(Dr["VL_VENDA"]);
                    //p.QtdeItem = 1;

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Listar Produto por Categoria: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }



    }
}
