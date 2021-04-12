using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_Pedido_Estoque
    {
        public decimal CodigoDocumento { get; set; }
        public decimal NumeroDocumento { get; set; }
        public decimal Saldo { get; set; }
        public decimal QtAtendida { get; set; }
        public decimal QtSolicitada { get; set; }
        public decimal QtColetada { get; set; }
        public decimal QtSumAtendida { get; set; }
        public int CodigoCliente { get; set; }
        public int CodigoProduto { get; set; }
        public int CodigoProdutoDocumento { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoEmpresa{ get; set; }
        public int Count { get; set; }
        public int CodigoDoca { get; set; }
        public short QtEmbalagem{ get; set; }
        public string DescricaoSituacao { get; set; }
        public string DescricaEmpresa { get; set; }
        public string DescricaoProduto { get; set; }
        public string NomeCliente { get; set; }
        public string CodigoBarras { get; set; }
        public string DescricaoDoca{ get; set; }
        public string DescricaoIndentificacao { get; set; }
        public string CodigoBarrasCaixa { get; set; }
    }
}
