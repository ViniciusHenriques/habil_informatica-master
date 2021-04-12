using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Composicao
    {
        public int CodigoProdutoComposto{ get; set; }
        public int CodigoSituacao{ get; set; }
        public int CodigoTipo{ get; set; }
        public DateTime Data { get; set; }
        public decimal ValorCustoProduto { get; set; }
        public decimal PercentualQuebra { get; set; }
        public decimal PercentualUmidade { get; set; }
        public decimal Rendimento { get; set; }
        public string DescricaoProduto { get; set; }
        public string DescricaoSituacao { get; set; }
        public string DescricaoTipo { get; set; }
        public string Observacao { get; set; }
    }
}
