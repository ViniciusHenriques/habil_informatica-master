using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_PRODUTOS_COFINS
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal PROD_NITEM { get; set; }

        /// <summary></summary>
        public decimal COFINS_CST { get; set; }

        /// <summary></summary>
        public decimal? COFINS_VBC { get; set; }

        /// <summary></summary>
        public decimal? COFINS_PCOFINS { get; set; }

        /// <summary></summary>
        public decimal? COFINS_VCOFINS { get; set; }

        /// <summary></summary>
        public decimal? COFINS_QBCPROD { get; set; }

        /// <summary></summary>
        public decimal? COFINS_VALIQPROD { get; set; }

        /// <summary></summary>
        public decimal? COFINSST_VBC { get; set; }

        /// <summary></summary>
        public decimal? COFINSST_PCOFINS { get; set; }

        /// <summary></summary>
        public decimal? COFINSST_QBCPROD { get; set; }

        /// <summary></summary>
        public decimal? COFINSST_VALIQPROD { get; set; }

        /// <summary></summary>
        public decimal? COFINSST_VCOFINS { get; set; }

    }
}