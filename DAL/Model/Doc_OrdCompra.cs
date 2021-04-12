using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_OrdCompra
    {

        public decimal CodigoDocumento { get; set; }
        public Int64 CodigoEmpresa { get; set; }
        public int CodigoTipoOrcamento { get; set; }
        public string DGSerieDocumento { get; set; }
        public decimal NumeroDocumento { get; set; }
        public string DGNumeroDocumento { get; set; }
        public DateTime DataHoraEmissao { get; set; }
        public DateTime DataValidade { get; set; }
        public int CodigoTipoOperacao { get; set; }
        public decimal ValorTotal { get; set; }
        public string DescricaoDocumento { get; set; }
        public int CodigoCondicaoPagamento { get; set; }
        public int CodigoTipoCobranca { get; set; }
        public int CodigoTipoDocumento { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoGeracaoSequencialDocumento { get; set; }
        public long CodigoVendedor { get; set; }
        public decimal ValorComissao { get; set; }
        public decimal ValorDescontoMedio { get; set; }
        public decimal ValorPesoOrcamento { get; set; }
        public decimal ValorCubagem { get; set; }
        public decimal NumeroWeb { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorST { get; set; }
        public int CodigoAplicacaoUso { get; set; }
        public string MotivoBaixaSemVenda { get; set; }
        //Complementos
        public long Cpl_CodigoTransportador { get; set; }
        public long Cpl_CodigoPessoa { get; set; }
        public int Cpl_Acao { get; set; }
        public int Cpl_Maquina { get; set; }
        public int Cpl_Usuario { get; set; }
        public string Cpl_Pessoa { get; set; }
        public string Cpl_NomeTabela { get; set; }
        public string Cpl_Situacao { get; set; }
        public string Cpl_NomeVendedor { get; set; }
        public string Cpl_DsCondicaoPagamento { get; set; }
        public string Cpl_DsAplicacaoUso { get; set; }
        public string Cpl_DsTipoCobranca { get; set; }
        public string Cpl_DsTipoOrcamento { get; set; }
        public string Cpl_NomeTransportador { get; set; }
        public int Cpl_QuantidadePedidosVinculados { get; set; }
        public bool IsVendedor { get; set; }
        public bool PodeGerarPedido { get; set; }
                                                                                                                                                                              
    }
}
