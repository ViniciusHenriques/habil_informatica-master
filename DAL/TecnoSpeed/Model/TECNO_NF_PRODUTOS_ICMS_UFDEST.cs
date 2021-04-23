using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_PRODUTOS_ICMS_UFDEST
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal PROD_NITEM { get; set; }

        /// <summary></summary>
        public decimal? VBCUFDEST { get; set; }

        /// <summary></summary>
        public decimal? PICMSUFDEST { get; set; }

        /// <summary></summary>
        public decimal? PICMSINTER { get; set; }

        /// <summary></summary>
        public decimal? PICMSINTERPART { get; set; }

        /// <summary></summary>
        public decimal? VICMSUFDEST { get; set; }

        /// <summary></summary>
        public decimal? VICMSUFREMET { get; set; }

        /// <summary></summary>
        public decimal? PFCPUFDEST { get; set; }

        /// <summary></summary>
        public decimal? VFCPUFDEST { get; set; }

        /// <summary></summary>
        public decimal? VBCFCPUFDEST { get; set; }

    }
}