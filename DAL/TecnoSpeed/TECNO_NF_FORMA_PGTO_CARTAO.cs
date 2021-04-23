using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_FORMA_PGTO_CARTAO
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal FORMA_PGTO_NITEM { get; set; }

        /// <summary></summary>
        public string CNPJ { get; set; }

        /// <summary></summary>
        public string TBAND { get; set; }

        /// <summary></summary>
        public string CAUT { get; set; }

        /// <summary></summary>
        public decimal? TPINTEGRA { get; set; }

    }
}