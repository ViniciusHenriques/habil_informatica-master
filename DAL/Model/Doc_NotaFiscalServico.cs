using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_NotaFiscalServico
    {
        public decimal CodigoNotaFiscalServico{get;set;}
        public Int64 CodigoGeracaoSequencialDocumento { get; set; }
        public DateTime DataLancamento { get; set; }
        public int CodigoPrestador { get; set; }
        public DateTime DataEmissao { get; set; }
        public decimal ValorPIS { get; set; }
        public decimal ValorCofins { get; set; }
        public decimal ValorCSLL { get; set; }
        public decimal ValorIRRF { get; set; }
        public decimal ValorINSS { get; set; }
        public decimal ValorOutrasRetencoes { get; set; }
        public string DescricaoGeralServico { get; set; }
        public int CodigoSituacao { get; set; }
        public decimal NumeroDocumento { get; set; }
        public decimal NumeroSerie { get; set; }
        public int CodigoTipoServico { get; set; }
        public decimal ValorAliquotaISSQN { get; set; }
        public decimal CodigoMunicipioPrestado { get; set; }
        public Decimal ValorTotalNota { get; set; }
        public string ChaveAcesso { get; set; }
        public string Protocolo { get; set; }
        public string DGSerieDocumento { get; set; }
        public int CodigoCondicaoPagamento { get; set; }
        public int CodigoTipoOperacao { get; set; }
        public decimal CodigoIndexIntegraPedido { get; set; }
        public decimal CodigoDocumentoOriginal { get; set; }
        //Complementares
        public string Cpl_NomeTomador{ get; set; }
        public string Cpl_DsSituacao { get; set; }
        public int Cpl_Usuario { get; set; }
        public int Cpl_Maquina { get; set; }
        public int CodigoTomador { get; set; }
        public string Cpl_NomeTabela { get; set; }
        public int Cpl_Acao { get; set; }
        public int Cpl_CodigoEstado { get; set; }
        public string Cpl_DescricaoMunicipio { get; set; }
        public bool BtnAutorizar { get; set; }
        public bool BtnCancelar{ get; set; }
        public bool BtnImprimir { get; set; }
        

    }
}
