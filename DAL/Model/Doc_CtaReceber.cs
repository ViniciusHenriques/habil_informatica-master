using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_CtaReceber
    {
        public decimal CodigoDocumento { get; set; }
        public int CodigoTipoDocumento { get; set; }
        public int CodigoEmpresa { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime DataEmissao { get; set; }
        public int NumeroDocumento { get; set; }
        public int NumeroSRDocumento { get; set; }
        public string DGDocumento { get; set; }
        public string DGSRDocumento { get; set; }
        public int CodigoSituacao { get; set; }
        public Decimal ValorDocumento { get; set; }
        public Decimal ValorDesconto { get; set; }
        public Decimal ValorAcrescimo { get; set; }
        public Decimal ValorGeral { get; set; }
        public string ObservacaoDocumento { get; set; }
        public int CodigoCobranca { get; set; }
        public int CodigoCondicaoPagamento { get; set; }
        public DateTime DataEntrega { get; set; }
        public int CodigoVendedor { get; set; }
        public int CodigoComprador { get; set; }
        public int CodigoFormaPagamento { get; set; }
        public int CodigoFrete { get; set; }
        public DateTime DataVencimento { get; set; }
        public int CodigoPlanoContas { get; set; }
        public int CodigoClassificacao { get; set; }
        public decimal CodigoDocumentoOriginal { get; set; }


        //Complementares
        public string Cpl_NomeFornecedor { get; set; }
        public string Cpl_DsSituacao { get; set; }
        public decimal Cpl_vlPagar { get; set; }
        public decimal Cpl_vlAcres { get; set; }
        public decimal Cpl_vlDesc { get; set; }
        public int Cpl_Usuario { get; set; }
        public int Cpl_Maquina { get; set; }
        public int CodigoPessoa { get; set; }
        public int Cpl_Acao { get; set; }
        public decimal Cpl_vlPago { get; set; }
        public int Cpl_CodigoPessoa { get; set; }
        public bool Cpl_PodeReplicar { get; set; }
    }
}
