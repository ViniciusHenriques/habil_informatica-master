namespace DAL.Model
{
    public class TipoOperacao
    {
        public long CodigoTipoOperacao { get; set; }
        public string DescricaoTipoOperacao { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoTipoMovimentacao { get; set; }
        public int CodTipoOperFiscal { get; set; }
        public int CodTipoOperCtPartida{ get; set; }
        public long CodCFOPEstadual { get; set; }
        public long CodCFOPInterestadual { get; set; }
        public long CodCFOPExterior { get; set; }
        public bool MovimentaEstoque { get; set; }
        public bool AtualizaFinanceiro { get; set; }
        public bool MovimentacaoInterna { get; set; }
        public bool BaixaFinanceiro { get; set; }
        public bool MovLocOrigemDestino{ get; set; }
        public int CodigoPIS { get; set; }
        public int CodigoCOFINS { get; set; }
        public int CodigoPrecedenciaImpostoICMS { get; set; }
        public int CodigoPrecedenciaImpostoPIS_COFINS { get; set; }
        //ICMS
        public int CodHabil_RegTributario { get; set; }
        public int CodCST_CSOSN { get; set; }
        public int CodModDetBCIcms{ get; set; }
        public int CodModDetBCIcmsST { get; set; }
        public decimal MVAEntrada { get; set; }
        public decimal CST00ICMS { get; set; }
        public decimal CST10ReducaoBCICMSST { get; set; }
        public decimal CST10ICMS { get; set; }
        public decimal CST10ReducaoBCICMSSTProprio { get; set; }
        public decimal CST10ICMSProprio { get; set; }
        public decimal CST10MVASaida { get; set; }
        public bool CST10CalculaDifal { get; set; }
        public decimal CST20ReducaoBCICMS { get; set; }
        public decimal CST20ICMS { get; set; }
        public decimal CST30ReducaoBCICMSST { get; set; }
        public decimal CST30ICMS { get; set; }
        public decimal CST30ICMSProprio { get; set; }
        public decimal CST30MVASaida { get; set; }
        public decimal CST51ReducaoBCICMS { get; set; }
        public decimal CST51ICMS { get; set; }
        public decimal CST51Diferimento { get; set; }
        public decimal CST70ReducaoBCICMSST { get; set; }
        public decimal CST70ICMS { get; set; }
        public decimal CST70ReducaoBCICMSSTProprio { get; set; }
        public decimal CST70ICMSProprio { get; set; }
        public decimal CST70MVASaida { get; set; }
        public decimal CST90ReducaoBCICMSST { get; set; }
        public decimal CST90ICMS { get; set; }
        public decimal CST90ReducaoBCICMSSTProprio { get; set; }
        public decimal CST90ICMSProprio { get; set; }
        public decimal CST90MVASaida { get; set; }
        public bool CST90CalculaDifal { get; set; }
        public int CST90MotDesoneracao { get; set; }
        //Simples
        public decimal CSOSN101_ICMS_SIMPLES { get; set; }
        public decimal CSOSN201_ReducaoBCICMSST { get; set; }
        public decimal CSOSN201_ICMS { get; set; }
        public decimal CSOSN201_MVASaida { get; set; }
        public decimal CSOSN201_ICMS_SIMPLES { get; set; }
        public decimal CSOSN202_203_ReducaoBCICMSST { get; set; }
        public decimal CSOSN202_203_ICMS { get; set; }
        public decimal CSOSN202_203_ICMS_PROPRIO { get; set; }
        public decimal CSOSN202_203_MVASaida { get; set; }

        public decimal CSOSN900_ReducaoBCICMSST { get; set; }
        public decimal CSOSN900_ICMS { get; set; }
        public decimal CSOSN900_ReducaoBCICMSProprio{ get; set; }
        public decimal CSOSN900_ICMS_Proprio { get; set; }
        public decimal CSOSN900_MVASaida { get; set; }
        public decimal CSOSN900_ICMS_SIMPLES { get; set; }

        public int CST20MotDesoneracao { get; set; }
        public int CST30MotDesoneracao { get; set; }
        public int CST70MotDesoneracao { get; set; }
        public int CST404150MotDesoneracao { get; set; }
        public string CodBeneficioFiscal { get; set; }
        public string MensagemIcms { get; set; }


        public string Cpl_ComboDescricaoTipoOperacao { get; set; }
    }
}
