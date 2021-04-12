using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class AcaoDoDispositivo
    {
        public string CD_KEY { get; set; }
        public decimal NR_FONE { get; set; }
        public string ID_DISPOSITIVO { get; set; }
        public string NM_DISPOSITIVO { get; set; }
        public string NM_MODELO { get; set; }
        public string NM_FABRICANTE { get; set; }
        public DateTime DT_LANCAMENTO { get; set; }
    }
}
