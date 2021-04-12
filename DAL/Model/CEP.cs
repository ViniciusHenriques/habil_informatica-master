using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class CEP
    {
        public Int64 CodigoCEP { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public int CodigoEstado { get; set; }
        public string DescricaoEstado { get; set; }
        public string Sigla { get; set; }
        public Int64 CodigoMunicipio { get; set; }
        public string DescricaoMunicipio { get; set; }
        public int CodigoBairro { get; set; }
        public string DescricaoBairro { get; set; }
        
    }
}
