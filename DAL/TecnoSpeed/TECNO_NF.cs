﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_NF
    {
        /// <summary></summary>
        public decimal ID_NOTA_FISCAL { get; set; }

        /// <summary></summary>
        public decimal IDE_CUF { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VSERV { get; set; }

        /// <summary></summary>
        public decimal IDE_CNF { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VBC { get; set; }

        /// <summary></summary>
        public string IDE_NATOP { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VISS { get; set; }

        /// <summary></summary>
        public decimal? IDE_INDPAG { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VPIS { get; set; }

        /// <summary></summary>
        public decimal IDE_MOD { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VCOFINS { get; set; }

        /// <summary></summary>
        public decimal IDE_SERIE { get; set; }

        /// <summary></summary>
        public decimal IDE_NNF { get; set; }

        /// <summary></summary>
        public DateTime IDE_DEMI { get; set; }

        /// <summary></summary>
        public DateTime? IDE_DSAIENT { get; set; }

        /// <summary></summary>
        public string IDE_TPNF { get; set; }

        /// <summary></summary>
        public decimal IDE_CMUNFG { get; set; }

        /// <summary></summary>
        public string IDE_FINNFE { get; set; }

        /// <summary></summary>
        public string EMIT_CNPJ { get; set; }

        /// <summary></summary>
        public string EMIT_IEST { get; set; }

        /// <summary></summary>
        public string EMIT_IM { get; set; }

        /// <summary></summary>
        public string EMIT_CNAE { get; set; }

        /// <summary></summary>
        public string DEST_CNPJ_CPF { get; set; }

        /// <summary></summary>
        public string DEST_XNOME { get; set; }

        /// <summary></summary>
        public string ENDERDEST_XLGR { get; set; }

        /// <summary></summary>
        public string ENDERDEST_NRO { get; set; }

        /// <summary></summary>
        public string ENDERDEST_XCPL { get; set; }

        /// <summary></summary>
        public string ENDERDEST_XBAIRRO { get; set; }

        /// <summary></summary>
        public decimal? ENDERDEST_CMUN { get; set; }

        /// <summary></summary>
        public string ENDERDEST_XMUN { get; set; }

        /// <summary></summary>
        public string ENDERDEST_UF { get; set; }

        /// <summary></summary>
        public decimal? ENDERDEST_CEP { get; set; }

        /// <summary></summary>
        public string ENDERDEST_CPAIS { get; set; }

        /// <summary></summary>
        public string ENDERDEST_XPAIS { get; set; }

        /// <summary></summary>
        public decimal? ENDERDEST_FONE { get; set; }

        /// <summary></summary>
        public string DEST_IE { get; set; }

        /// <summary></summary>
        public string DEST_ISUF { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VBC { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VICMS { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VBCST { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VST { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VPROD { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VFRETE { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VSEG { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VDESC { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VII { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VIPI { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VPIS { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VCOFINS { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VOUTRO { get; set; }

        /// <summary></summary>
        public decimal ICMSTOT_VNF { get; set; }

        /// <summary></summary>
        public decimal? RETTRIB_VRETPIS { get; set; }

        /// <summary></summary>
        public decimal? RETTRIB_VRETCOFINS { get; set; }

        /// <summary></summary>
        public decimal? RETTRIB_VRETCSLL { get; set; }

        /// <summary></summary>
        public decimal? RETTRIB_VBCIRRF { get; set; }

        /// <summary></summary>
        public decimal? RETTRIB_VIRRF { get; set; }

        /// <summary></summary>
        public decimal? RETTRIB_VBCRETPREV { get; set; }

        /// <summary></summary>
        public decimal? RETTRIB_VRETPREV { get; set; }

        /// <summary></summary>
        public decimal TRANSP_MODFRETE { get; set; }

        /// <summary></summary>
        public string FAT_NFAT { get; set; }

        /// <summary></summary>
        public decimal? FAT_VORIG { get; set; }

        /// <summary></summary>
        public decimal? FAT_VDESC { get; set; }

        /// <summary></summary>
        public decimal? FAT_VLIQ { get; set; }

        /// <summary></summary>
        public string INFADIC_INFADFISCO { get; set; }

        /// <summary></summary>
        public string EXPORTA_UFEMBARQ { get; set; }

        /// <summary></summary>
        public string EXPORTA_XLOCEMBARQ { get; set; }

        /// <summary></summary>
        public string COMPRA_XNEMP { get; set; }

        /// <summary></summary>
        public string COMPRA_XPED { get; set; }

        /// <summary></summary>
        public string COMPRA_XCONT { get; set; }

        /// <summary></summary>
        public string INFADIC_INFCPL { get; set; }

        /// <summary></summary>
        public string NOME_RECEBEDOR { get; set; }

        /// <summary></summary>
        public string OBS_IMPRIMIR_NO_CORPO { get; set; }

        /// <summary></summary>
        public DateTime? IDE_DHCONT { get; set; }

        /// <summary></summary>
        public string IDE_XJUST { get; set; }

        /// <summary></summary>
        public string DEST_EMAIL { get; set; }

        /// <summary></summary>
        public string TRANSP_VAGAO { get; set; }

        /// <summary></summary>
        public string TRANSP_BALSA { get; set; }

        /// <summary></summary>
        public decimal? gera_ideDsaient_automatico { get; set; }

        /// <summary></summary>
        public string ide_hcont { get; set; }

        /// <summary></summary>
        public decimal? VTOTTRIB { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_DCOMPET_OLD { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VDEDUCAO { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VOUTRO { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VDESCINCOND { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VDESCCOND { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_VISSRET { get; set; }

        /// <summary></summary>
        public decimal? ISSQNTOT_CREGTRIB { get; set; }

        /// <summary></summary>
        public string EXPORTA_UFSAIDAPAIS { get; set; }

        /// <summary></summary>
        public string EXPORTA_XLOCEXPORTA { get; set; }

        /// <summary></summary>
        public string EXPORTA_XLOCDESPACHO { get; set; }

        /// <summary></summary>
        public decimal? DEST_INDIE { get; set; }

        /// <summary></summary>
        public decimal? IDE_DEST { get; set; }

        /// <summary></summary>
        public decimal? IDE_INDFINAL { get; set; }

        /// <summary></summary>
        public decimal? IDE_INDPRES { get; set; }

        /// <summary></summary>
        public string DEST_IDESTRANG { get; set; }

        /// <summary></summary>
        public string IDE_HEMI { get; set; }

        /// <summary></summary>
        public decimal? ICMStot_VICMSDESON { get; set; }

        /// <summary></summary>
        public string DEST_IM { get; set; }

        /// <summary></summary>
        public string IDE_HSAIENT { get; set; }

        /// <summary></summary>
        public decimal? ICMSTOT_VICMSUFDEST { get; set; }

        /// <summary></summary>
        public decimal? ICMSTOT_VICMSUFREMET { get; set; }

        /// <summary></summary>
        public decimal? ICMSTOT_VFCPUFDEST { get; set; }

        /// <summary></summary>
        public decimal? ICMSTOT_VFCP { get; set; }

        /// <summary></summary>
        public decimal? ICMSTOT_VFCPST { get; set; }

        /// <summary></summary>
        public decimal? ICMSTOT_VFCPSTRET { get; set; }
    }
}