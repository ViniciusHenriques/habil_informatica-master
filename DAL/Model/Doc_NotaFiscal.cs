using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Doc_NotaFiscal
    {
        public decimal CodigoDocumento { get; set; }
        public Int64 CodigoEmpresa { get; set; }
        public Int64 CodigoNaturezaOperacao { get; set; }
        public DateTime? DataHoraSaida { get; set; }
        public DateTime DataHoraEmissao { get; set; }
        public string DGSerieDocumento { get; set; }
        public string DGNumeroDocumento { get; set; }
        public string ChaveAcesso { get; set; }
        public string DescricaoDocumento { get; set; }
        public int CodigoTipoOrcamento { get; set; }
        public int CodigoCondicaoPagamento { get; set; }
        public int CodigoTipoCobranca { get; set; }
        public int CodigoTipoDocumento { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoGeracaoSequencialDocumento { get; set; }        
        public int CodigoTipoOperacao { get; set; }
        public int CodigoRegimeTributario { get; set; }
        public int CodigoFinalidadeNF { get; set; }
        public int CodigoIndicadorPresenca { get; set; }
        public int CodigoConsumidorFinal { get; set; }
        public int CodigoModalidadeFrete { get; set; }
        public decimal NumeroDocumento { get; set; }
        public decimal NumeroWeb { get; set; }
        public decimal CodigoDocumentoOriginal { get; set; }//numero documento do pedido
        public decimal ValorBaseCalculoICMS { get; set; }
        public decimal ValorICMS { get; set; }
        public decimal ValorBaseCalculoICMSST { get; set; }
        public decimal ValorICMSST { get; set;}
        public decimal ValorIPI { get; set; }
        public decimal ValorPesoBruto { get; set; }
        public decimal ValorPesoLiquido { get; set; }
        public decimal ValorCubagem { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorDespesasAcessorias { get; set; }
        public decimal ValorTotalItens { get; set; }
        public decimal ValorTotalDocumento { get; set; }

        //Complementos
        public int Cpl_Acao { get; set; }
        public int Cpl_Maquina { get; set; }
        public int Cpl_Usuario { get; set; }
        public long Cpl_CodigoTransportador { get; set; }
        public long Cpl_CodigoPessoa { get; set; }
        public bool PodeImprimir { get; set; }
        public string Cpl_Pessoa { get; set; }
        public string Cpl_NomeTabela { get; set; }
        public string Cpl_Situacao { get; set; }
        public string Cpl_NomeVendedor { get; set; }
        public string Cpl_DsRegimeTributario { get; set; }
        public string Cpl_DsFinalidade { get; set; }
        public string Cpl_DsNatOperacao { get; set; }
        public string Cpl_DsCondicaoPagamento { get; set; }
        public string Cpl_DsTipoCobranca { get; set; }
        public string Cpl_NomeTransportador { get; set; }
        public string Cpl_DsTipoOperacao { get; set; }

    }
}
