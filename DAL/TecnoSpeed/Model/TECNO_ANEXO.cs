using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_ANEXO
    {
        /// <summary></summary>
        public string CHAVE_BUSCA { get; set; }

        /// <summary></summary>
        public int CD_ANEXO { get; set; }

        /// <summary></summary>
        public string DS_ARQUIVO { get; set; }

        /// <summary></summary>
        public byte[] TX_CONTEUDO { get; set; }

        /// <summary></summary>
        public string EX_ARQUIVO { get; set; }

        /// <summary></summary>
        public int TP_ACAO { get; set; }

    }

}