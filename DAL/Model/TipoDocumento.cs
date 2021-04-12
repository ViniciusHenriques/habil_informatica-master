using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class TipoDocumento
    {
        public int CodigoTipoDocumento  { get; set;}
        public string DescricaoTipoDocumento { get; set; }
        public int CodigoSituacao { get; set; }
        public int TipoDeCampo { get; set; }
        public int IncrementalPorEmpresa { get; set; }
        public int AberturaDeSerie { get; set; }
        public string NomeDaTabela { get; set; }
    }

}
