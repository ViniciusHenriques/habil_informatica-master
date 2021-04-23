using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_TRANSP_VOLUMES
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal NITEM { get; set; }

        /// <summary></summary>
        public decimal? VOL_QVOL { get; set; }

        /// <summary></summary>
        public string VOL_ESP { get; set; }

        /// <summary></summary>
        public string VOL_MARCA { get; set; }

        /// <summary></summary>
        public string VOL_NVOL { get; set; }

        /// <summary></summary>
        public decimal? VOL_PESOL { get; set; }

        /// <summary></summary>
        public decimal? VOL_PESOB { get; set; }

    }
}