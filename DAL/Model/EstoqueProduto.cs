using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class EstoqueProduto
    {
        public int CodigoIndice { get; set; }
        public int CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string NomeProduto { get; set; }
        public string CodigoLocalizacao { get; set; }
        public int CodigoIndiceLocalizacao { get; set; }
        public int CodigoProduto { get; set; }
        public int CodigoLote { get; set; }
        public decimal Quantidade { get; set; }
        public int CodigoSituacao { get; set; }
        public string DescricaoSituacao { get; set; }
        //public string Cpl_DescDDL { get; set; }
        

    }
}
