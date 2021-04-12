using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ProdutoPorCategoria
    {

        public Int32 QtdeItem { get; set; }
        public Int64 CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public double CodigoCategoria { get; set; }
        public string DescricaoCategoria { get; set; }
        public double ValorVenda { get; set; }

        public ProdutoPorCategoria()
        {
        }

        public ProdutoPorCategoria(Int32 _QtdeItem,
                        Int64 _CodigoProduto,
                        string _DescricaoProduto,
                        double _CodigoCategoria,
                        string _DescricaoCategoria,
                        double _ValorVenda)
        {
            QtdeItem = _QtdeItem;
            CodigoProduto = _CodigoProduto;
            DescricaoProduto = _DescricaoProduto;
            CodigoCategoria = _CodigoCategoria;
            DescricaoCategoria = _DescricaoCategoria;
            ValorVenda = _ValorVenda;
        }
    }
}
