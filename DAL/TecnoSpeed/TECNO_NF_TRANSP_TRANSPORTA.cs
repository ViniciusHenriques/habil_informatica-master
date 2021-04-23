using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_TRANSP_TRANSPORTA
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public string TRANSPORTA_CNPJ_CPF { get; set; }

        /// <summary></summary>
        public string TRANSPORTA_XNOME { get; set; }

        /// <summary></summary>
        public string TRANSPORTA_IE { get; set; }

        /// <summary></summary>
        public string TRANSPORTA_XENDER { get; set; }

        /// <summary></summary>
        public string TRANSPORTA_XMUN { get; set; }

        /// <summary></summary>
        public string TRANSPORTA_UF { get; set; }

    }
}