using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.TecnoSpeed
{
    public class TECNO_EVENTO_ERP
    {
        /// <summary></summary>
        public decimal ID_EVENTO_ERP { get; set; }

        /// <summary></summary>
        public string IMPRESSORA { get; set; }

        /// <summary></summary>
        public string TP_EVENTO_ERP { get; set; }

        /// <summary></summary>
        public string CHAVE_BUSCA { get; set; }

        /// <summary></summary>
        public decimal TP_ACAO { get; set; }

        /// <summary></summary>
        public DateTime DH_EVENTO { get; set; }

        /// <summary></summary>
        public string TP_STATUS { get; set; }

        /// <summary></summary>
        public decimal? TP_RETORNO { get; set; }

        /// <summary></summary>
        public decimal? TP_RETORNO_CONTINGENCIA_LOCAL { get; set; }

        /// <summary></summary>
        public decimal? NPROT { get; set; }

        /// <summary></summary>
        public decimal? NREC { get; set; }

        /// <summary></summary>
        public string JUSTIFICATIVA { get; set; }

        /// <summary></summary>
        public string CHNFE { get; set; }

        /// <summary></summary>
        public string SERVER_NAME { get; set; }

        /// <summary></summary>
        public decimal TPAMB { get; set; }

        /// <summary></summary>
        public string CNPJ_EVENTO { get; set; }

        /// <summary></summary>
        public decimal? COD_RETORNO { get; set; }

        /// <summary></summary>
        public string DESC_RETORNO { get; set; }

        /// <summary></summary>
        public string CONFIG_EMAIL_DESTINATARIO { get; set; }

        /// <summary></summary>
        public decimal COPIAS_IMPRESSAO { get; set; }

        /// <summary></summary>
        public decimal? TP_IMP { get; set; }

        /// <summary></summary>
        public string LINK_PDF { get; set; }

        /// <summary></summary>
        public string BO_ATUALIZADO { get; set; }

        /// <summary></summary>
        public string TP_DESTINO_IMPRESSAO { get; set; }

        /// <summary></summary>
        public string USUARIO_IMPRESSAO { get; set; }

        /// <summary></summary>
        public decimal? NR_FORMULARIO_CONTINGENCIA { get; set; }

        /// <summary></summary>
        public string nome_arquivo_cfg_danfe { get; set; }

        /// <summary></summary>
        public decimal? csitconf { get; set; }

        /// <summary></summary>
        public int IN_ENVIOU_EMAIL { get; set; }

        /// <summary></summary>
        public decimal? ID_CLE { get; set; }

        /// <summary></summary>
        public decimal? ID_CONSULTA_DEST { get; set; }

        /// <summary></summary>
        public decimal? CTE_ID { get; set; }

        /// <summary></summary>
        public decimal? ID_CTE { get; set; }

        /// <summary></summary>
        public string CODIGO_UF { get; set; }

        /// <summary></summary>
        public string BO_CONSULTA_STATUS_SVC { get; set; }

        /// <summary></summary>
        public decimal? IDE_MOD { get; set; }

        /// <summary></summary>
        public string CHNFEREF { get; set; }

        /// <summary></summary>
        public string GMT { get; set; }

        /// <summary></summary>
        public string CONFIG_EMAIL_SERVIDOR { get; set; }

        /// <summary></summary>
        public string CONFIG_EMAIL_REMETENTE { get; set; }

        /// <summary></summary>
        public string CONFIG_EMAIL_ASSUNTO { get; set; }

        /// <summary></summary>
        public string CONFIG_EMAIL_MENSAGEM { get; set; }

        /// <summary></summary>
        public string CONFIG_EMAIL_USUARIO { get; set; }

        /// <summary></summary>
        public string CONFIG_EMAIL_SENHA { get; set; }

        /// <summary></summary>
        public int CONFIG_EMAIL_AUTENTICACAO { get; set; }

        /// <summary></summary>
        public int CONFIG_EMAIL_PORTA { get; set; }

        /// <summary></summary>
        public int IN_REMOVER_ACENTOS { get; set; }

        /// <summary></summary>
        public int IN_VALIDAR_ESQUEMA { get; set; }

        /// <summary></summary>
        public int TP_MODO_OPERACAO { get; set; }

        /// <summary></summary>
        public string CERTIFICADO_CAMINHO { get; set; }

        /// <summary></summary>
        public string CERTIFICADO_SENHA { get; set; }

        /// <summary></summary>
        public string VERSAO_MANUAL { get; set; }

        /// <summary></summary>
        public string VERSAO_MANUAL_DPEC_SCAN { get; set; }

        /// <summary></summary>
        public int NR_LOTE { get; set; }

        /// <summary></summary>
        public int TP_EVENTO { get; set; }

        /// <summary></summary>
        public string NSU { get; set; }

        /// <summary></summary>
        public int TP_NSU { get; set; }

        /// <summary></summary>
        public string TX_CORRECAO { get; set; }

        /// <summary></summary>
        public string CONTRIB_CNPJ_CPF_IE { get; set; }

    }

}