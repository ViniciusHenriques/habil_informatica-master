using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class ParcelaDocumento
    {
        public decimal CodigoDocumento { get; set; }
        public int CodigoParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorParcela { get; set; }
        public decimal CodigoDocumentoPagamento { get; set; }
        public string DGNumeroDocumento { get; set; }

        public ParcelaDocumento() { }
        public ParcelaDocumento(int CodigoParcela, DateTime DataVencimento,decimal ValorParcela, decimal CodigoDocumentoPagamento, string DGNumeroDocumento) {

            this.CodigoParcela= CodigoParcela;

            this.DataVencimento = DataVencimento;
            this.ValorParcela = ValorParcela;
            this.CodigoDocumentoPagamento = CodigoDocumentoPagamento;
            this.DGNumeroDocumento = DGNumeroDocumento;
        }
    }
}
