using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_TRANSP_RETTRANSP
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal RETTRANSP_VSERV { get; set; }

        /// <summary></summary>
        public decimal RETTRANSP_VBCRET { get; set; }

        /// <summary></summary>
        public decimal? RETTRANSP_PICMSRET { get; set; }

        /// <summary></summary>
        public decimal RETTRANSP_VICMSRET { get; set; }

        /// <summary></summary>
        public decimal RETTRANSP_CFOP { get; set; }

        /// <summary></summary>
        public decimal RETTRANSP_CMUNFG { get; set; }

    }
}