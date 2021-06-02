using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_INUTILIZACAO_ERP
    {
        /// <summary></summary>
        public decimal ID_INUTILIZACAO_ERP { get; set; }

        /// <summary></summary>
        public decimal CUF { get; set; }

        /// <summary></summary>
        public decimal ANO { get; set; }

        /// <summary></summary>
        public string CNPJ { get; set; }

        /// <summary></summary>
        public decimal MOD { get; set; }

        /// <summary></summary>
        public decimal SERIE { get; set; }

        /// <summary></summary>
        public decimal NNFINI { get; set; }

        /// <summary></summary>
        public decimal NNFFIM { get; set; }

        /// <summary></summary>
        public string XJUST { get; set; }

    }

}