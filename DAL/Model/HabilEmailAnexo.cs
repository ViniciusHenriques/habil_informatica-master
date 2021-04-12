using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL 
{
    public class HabilEmailAnexo
    {
        public long CD_INDEX { get; set; }
        public int CD_ANEXO { get; set; }
        public string DS_ARQUIVO { get; set; }
        public long CD_EXTENSAO { get; set; }
        public byte[] TX_CONTEUDO { get; set; }
    }
}
