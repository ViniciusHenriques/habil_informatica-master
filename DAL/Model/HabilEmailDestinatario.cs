using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class HabilEmailDestinatario
    {
        public long CD_INDEX { get; set; }
        public int CD_EMAIL_DESTINATARIO { get; set; }
        public int TP_DESTINATARIO { get; set; }
        public string NM_DESTINATARIO { get; set; }
        public string TX_EMAIL { get; set; }
        public bool  IN_EMAIL_VALIDADO { get; set; }

    }

}
