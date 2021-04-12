using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Municipio
    {
        public Int64 CodigoMunicipio { get; set; }
        public int CodigoEstado { get; set; }
        public string Sigla { get; set; }
        public string DescricaoMunicipio { get; set; }
        
    }
}
