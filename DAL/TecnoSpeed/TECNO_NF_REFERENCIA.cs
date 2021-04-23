using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_REFERENCIA
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal NITEM { get; set; }

        /// <summary></summary>
        public string NFREF_REFNFE { get; set; }

        /// <summary></summary>
        public decimal? REFNF_CUF { get; set; }

        /// <summary></summary>
        public decimal? REFNF_AAMM { get; set; }

        /// <summary></summary>
        public string REFNF_CNPJ { get; set; }

        /// <summary></summary>
        public decimal? REFNF_MOD { get; set; }

        /// <summary></summary>
        public decimal? REFNF_NNF { get; set; }

        /// <summary></summary>
        public string REFNF_IE { get; set; }

        /// <summary></summary>
        public string REFNF_CTE { get; set; }

        /// <summary></summary>
        public string REFNF_MODECF { get; set; }

        /// <summary></summary>
        public decimal? REFNF_NECF { get; set; }

        /// <summary></summary>
        public decimal? REFNF_NCOO { get; set; }

        /// <summary></summary>
        public string REFNF_SERIE { get; set; }

    }
}