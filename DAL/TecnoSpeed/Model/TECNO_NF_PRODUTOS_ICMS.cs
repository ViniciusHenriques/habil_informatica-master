
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF_PRODUTOS_ICMS
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal PROD_NITEM { get; set; }

        /// <summary></summary>
        public decimal? ICMS_CST { get; set; }

        /// <summary></summary>
        public decimal ICMS_ORIG { get; set; }

        /// <summary></summary>
        public decimal? ICMS_MODBC { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VBC { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PICMS { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VICMS { get; set; }

        /// <summary></summary>
        public decimal? ICMS_MODBCST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PMVAST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PREDBCST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VBCST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PICMSST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VICMSST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PREDBC { get; set; }

        /// <summary></summary>
        public decimal? ICMS_MOTDESICMS { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VBCSTRET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VICMSSTRET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_ICMSPART { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PBCOP { get; set; }

        /// <summary></summary>
        public string ICMS_UFST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_ICMSST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VBCSTDEST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VICMSSTDEST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_CSOSN { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PCREDSN { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VCREDICMSSN { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VICMSDESON { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VICMSOP { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PDIF { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VICMSDIF { get; set; }

        /// <summary></summary>
        public string BO_MONTAR_VICMS { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VBCFCP { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PFCP { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VFCP { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VBCFCPST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PFCPST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VFCPST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VBCFCPSTRET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PFCPSTRET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VFCPSTRET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PST { get; set; }

        /// <summary></summary>
        public decimal? ICMS_REDBCEFET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VBCEFET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_PEFET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VEFET { get; set; }

        /// <summary></summary>
        public decimal? ICMS_VICMSSUBSTITUTO { get; set; }

    }
}