using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ItemDoInventario
    {
        public decimal CodigoIndice { get; set; }
        public decimal CodigoIndiceInventario { get; set; }
        public decimal CodigoIndiceEstoque { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoProduto { get; set; }
        public int CodigoIndiceLocalizacao1 { get; set; }
        public int CodigoIndiceLocalizacao2 { get; set; }
        public int CodigoCategoria1 { get; set; }
        public int CodigoCategoria2 { get; set; }
        public decimal QtContagem { get; set; }
        public decimal QtContagem2 { get; set; }
        public decimal QtContagem3 { get; set; }
        public decimal QtContagem4 { get; set; }
        public decimal QtContagem5 { get; set; }
        public decimal QtInventario { get; set; }

        public string CodigoLocalizacao { get; set; }
        public string CodigoCategoria { get; set; }
        public string CplLote { get; set; }
        public string NomeProduto { get; set; }
        public string Und { get; set; }

    }
}

