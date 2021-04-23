using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_FORMA_PGTO
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal FORMA_PGTO_NITEM { get; set; }

        /// <summary></summary>
        public string TPAG { get; set; }

        /// <summary></summary>
        public decimal VPAG { get; set; }

        /// <summary></summary>
        public decimal? VTROCO { get; set; }

        /// <summary></summary>
        public string INDPAG { get; set; }

    }
}