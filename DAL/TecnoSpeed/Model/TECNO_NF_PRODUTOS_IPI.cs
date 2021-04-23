using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_PRODUTOS_IPI
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal PROD_NITEM { get; set; }

        /// <summary></summary>
        public string IPI_CLENQ { get; set; }

        /// <summary></summary>
        public string IPI_CNPJPROD { get; set; }

        /// <summary></summary>
        public string IPI_CSELO { get; set; }

        /// <summary></summary>
        public decimal? IPI_QSELO { get; set; }

        /// <summary></summary>
        public string IPI_CENQ { get; set; }

        /// <summary></summary>
        public decimal IPI_CST { get; set; }

        /// <summary></summary>
        public decimal? IPI_VBC { get; set; }

        /// <summary></summary>
        public decimal? IPI_QUNID { get; set; }

        /// <summary></summary>
        public decimal? IPI_VUNID { get; set; }

        /// <summary></summary>
        public decimal? IPI_PIPI { get; set; }

        /// <summary></summary>
        public decimal? IPI_VIPI { get; set; }

    }
}