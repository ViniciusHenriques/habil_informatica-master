using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_DUPLICATA
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal NITEM { get; set; }

        /// <summary></summary>
        public string DUP_NDUP { get; set; }

        /// <summary></summary>
        public DateTime DUP_DVENC { get; set; }

        /// <summary></summary>
        public decimal DUP_VDUP { get; set; }

    }
}