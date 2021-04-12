using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class BodyDocumento
    {
        public decimal CodigoBodyDocumento { get; set; }
        public decimal CodigoDocumento { get; set; }
        public int CodigoItem { get; set; }
        public string TextoCorpo { get; set; }
    }
}
