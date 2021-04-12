using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL 
{
    public class HabilEmailUsuarioContato
    {
        public long CD_INDEX { get; set; }
        public long CD_USUARIO { get; set; }
        public string NM_CONTATO { get; set; }
        public string TX_EMAIL { get; set; }
        public string NM_CONTATO_EMAIL { get; set; }
    }
}
