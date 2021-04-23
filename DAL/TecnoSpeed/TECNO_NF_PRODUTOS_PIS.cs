using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_PRODUTOS_PIS
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal PROD_NITEM { get; set; }

        /// <summary></summary>
        public decimal PIS_CST { get; set; }

        /// <summary></summary>
        public decimal? PIS_VBC { get; set; }

        /// <summary></summary>
        public decimal? PIS_PPIS { get; set; }

        /// <summary></summary>
        public decimal? PIS_VPIS { get; set; }

        /// <summary></summary>
        public decimal? PIS_QBCPROD { get; set; }

        /// <summary></summary>
        public decimal? PIS_VALIQPROD { get; set; }

        /// <summary></summary>
        public decimal? PISST_VBC { get; set; }

        /// <summary></summary>
        public decimal? PISST_PPIS { get; set; }

        /// <summary></summary>
        public decimal? PISST_QBCPROD { get; set; }

        /// <summary></summary>
        public decimal? PISST_VALIQPROD { get; set; }

        /// <summary></summary>
        public decimal? PISST_VPIS { get; set; }

    }
}