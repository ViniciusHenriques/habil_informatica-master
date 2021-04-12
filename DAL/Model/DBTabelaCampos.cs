using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class DBTabelaCampos
    {
        public string Filtro { get; set; }
        public string Inicio { get; set; }
        public string Fim { get; set; }
        public string Tipo { get; set; }
        public Boolean SemLike { get; set; }

    }
}
