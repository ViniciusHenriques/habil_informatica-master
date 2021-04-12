using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class HabilTipoBloqueio
    {
        public int CodigoTipoDocumento { get; set; }
        public int CodigoBloqueio { get; set; }
        public string DescricaoBloqueio { get; set; }
        public int QuantidadeBloqueios { get; set; }
    }
}
