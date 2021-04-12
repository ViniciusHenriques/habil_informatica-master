using System;

namespace DAL.Model
{
    public class ItemDoPedido
    {

        public Int32 QtdeItem { get; set; }
        public Int64 CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorTotal { get; set; }

        public ItemDoPedido()
        {
        }

        public ItemDoPedido(Int32 _QtdeItem,
                        Int64 _CodigoProduto,
                        string _DescricaoProduto,
                        double _CodigoCategoria,
                        double _ValorUnitario)
        {
            QtdeItem = _QtdeItem;
            CodigoProduto = _CodigoProduto;
            DescricaoProduto = _DescricaoProduto;
            ValorUnitario  = _ValorUnitario;
            ValorTotal  = _ValorUnitario * _QtdeItem;
        }
    }
}
