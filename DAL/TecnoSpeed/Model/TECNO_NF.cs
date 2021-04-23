using System;
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

        /// <summary>cUF_B02 - Código da UF do emitente do Documento Fiscal	</summary>
        public decimal IDE_CUF { get; set; }

        /// <summary>vServ_W18 - Valor total dos Serviços sob não-incidência ou não tributados pelo ICMS	</summary>
        public decimal? ISSQNTOT_VSERV { get; set; }

        /// <summary>cNF_B03 - Código Numérico que compõe a Chave de Acesso	</summary>
        public decimal IDE_CNF { get; set; }

        /// <summary>vBC_W19 - Valor da Base de Cálculo do ISSQN	</summary>
        public decimal? ISSQNTOT_VBC { get; set; }

        /// <summary>natOp_B04 - Descrição da Natureza da Operação</summary>
        public string IDE_NATOP { get; set; }

        /// <summary>vISS_W20 - Valor total do ISS</summary>
        public decimal? ISSQNTOT_VISS { get; set; }

        /// <summary>indPag_YA01b - Indicador da Forma de Pagamento	</summary>
        public decimal? IDE_INDPAG { get; set; }

        /// <summary>vPIS_W21 - Valor total do PIS sobre serviços	</summary>
        public decimal? ISSQNTOT_VPIS { get; set; }

        /// <summary>mod_B06 - Código do Modelo do Documento Fiscal</summary>
        public decimal IDE_MOD { get; set; }

        /// <summary>vCOFINS_W22 - Valor total da COFINS sobre serviços	</summary>
        public decimal? ISSQNTOT_VCOFINS { get; set; }

        /// <summary>serie_B07 - Série do Documento Fiscal	</summary>
        public decimal IDE_SERIE { get; set; }

        /// <summary> nNF_B08	-	Número do documento fiscal.</summary>
        public decimal IDE_NNF { get; set; }

        /// <summary>dhEmi_B09 - Data e hora de emissão do Documento Fiscal</summary>
        public DateTime IDE_DEMI { get; set; }

        /// <summary>dhSaiEnt_B10 - Data e hora de Saída ou da Entrada da Mercadoria/Produto</summary>
        public DateTime? IDE_DSAIENT { get; set; }

        /// <summary>tpNF_B11 - Tipo de Operação</summary>
        public string IDE_TPNF { get; set; }

        /// <summary>cMunFG_B12	- 	Código do Município de Ocorrência do Fato Gerador</summary>
        public decimal IDE_CMUNFG { get; set; }

        /// <summary>finNFe_B25 - Versão do Processo de emissão da NF-e</summary>
        public string IDE_FINNFE { get; set; }

        /// <summary>CNPJ_C02 - CNPJ do emitente</summary>
        public string EMIT_CNPJ { get; set; }

        /// <summary>IEST_C18 - IE do Substituto Tributário</summary>
        public string EMIT_IEST { get; set; }

        /// <summary>IM_C19 - 	Inscrição Municipal do Prestador de Serviço</summary>
        public string EMIT_IM { get; set; }

        /// <summary>CNAE_C20 - CNAE Fiscal</summary>
        public string EMIT_CNAE { get; set; }

        /// <summary>CNPJ_E02/CPF_E03 - CNPJ/CPF do destinatário</summary>
        public string DEST_CNPJ_CPF { get; set; }

        /// <summary>xNome_E04 - Razão social ou nome do destinatário</summary>
        public string DEST_XNOME { get; set; }

        /// <summary>xLgr_E06 - Logradouro	</summary>
        public string ENDERDEST_XLGR { get; set; }

        /// <summary>nro_E07 - Número</summary>
        public string ENDERDEST_NRO { get; set; }

        /// <summary>xCpl_E08 - Complemento	</summary>
        public string ENDERDEST_XCPL { get; set; }

        /// <summary>xBairro_E09 - Bairro</summary>
        public string ENDERDEST_XBAIRRO { get; set; }

        /// <summary>cMun_E10 - Código do município	</summary>
        public decimal? ENDERDEST_CMUN { get; set; }

        /// <summary>xMun_E11 - 	Nome do município</summary>
        public string ENDERDEST_XMUN { get; set; }

        /// <summary>UF_E12 - Sigla da UF	</summary>
        public string ENDERDEST_UF { get; set; }

        /// <summary>CEP_E13 - Código do CEP	</summary>
        public decimal? ENDERDEST_CEP { get; set; }

        /// <summary>cPais_E14 - Código do País	</summary>
        public string ENDERDEST_CPAIS { get; set; }

        /// <summary>xPais_E15	 -  Nome do País	</summary>
        public string ENDERDEST_XPAIS { get; set; }

        /// <summary>fone_E16 - Telefone</summary>
        public decimal? ENDERDEST_FONE { get; set; }

        /// <summary>IE_E17 - Inscrição Estadual do Destinatário	</summary>
        public string DEST_IE { get; set; }

        /// <summary>ISUF_E18 - Inscrição na SUFRAMA</summary>
        public string DEST_ISUF { get; set; }

        /// <summary>Valor base calculo</summary>
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

        /// <summary>xNome_G02b - Razão Social ou Nome do Recebedor	</summary>
        public string NOME_RECEBEDOR { get; set; }

        /// <summary></summary>
        public string OBS_IMPRIMIR_NO_CORPO { get; set; }

        /// <summary></summary>
        public DateTime? IDE_DHCONT { get; set; }

        /// <summary></summary>
        public string IDE_XJUST { get; set; }

        /// <summary>email_E19 - Email do destinatário	</summary>
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

        /// <summary>indFinal_B25a - Indica operação com Consumidor final</summary>
        public decimal? IDE_INDFINAL { get; set; }

        /// <summary> indPres_B25b - Indicador de presença do comprador no estabelecimento comercial no momento da operação</summary>
        public decimal? IDE_INDPRES { get; set; }

        /// <summary></summary>
        public string DEST_IDESTRANG { get; set; }

        /// <summary></summary>
        public string IDE_HEMI { get; set; }

        /// <summary></summary>
        public decimal? ICMStot_VICMSDESON { get; set; }

        /// <summary>IM_E18a - Inscrição Municipal do Tomador do Serviço	</summary>
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