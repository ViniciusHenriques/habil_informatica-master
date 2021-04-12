using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class BaixaDocumento
    {
        private static int CodItem;

        public decimal CodigoDocumento { get; set; }
        public int CodigoBaixa { get; set; }
        public DateTime DataBaixa { get; set; }
        public decimal ValorBaixa { get; set; }
        public DateTime DataLancamento { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorAcrescimo { get; set; }
        public decimal ValorTotalBaixa { get; set; }
        public int CodigoTipoCobranca { get; set; }
        public int CodigoContaCorrente { get; set; }
        public string Observacao { get; set; }
        public int TipoBaixa { get; set; }

        public string Cpl_Cobranca { get; set; }

        public BaixaDocumento()
        {
        }
        public BaixaDocumento(int CodigoBaixa,
                              DateTime DataBaixa,
                              decimal ValorBaixa,
                              DateTime DataLancamento,
                              decimal ValorDesconto,
                              decimal ValorAcrescimo,
                              decimal ValorTotalBaixa,
                              int CodigoTipoCobranca,
                              int CodigoContaCorrente,
                              string Observacao,
                              int TipoBaixa ,
                              string Cpl_Cobranca)
        {
            CodItem++;
            if (CodigoBaixa == 0)
                this.CodigoBaixa = CodItem;
            else
                this.CodigoBaixa = CodigoBaixa;
            this.DataBaixa = DataBaixa;
            this.ValorBaixa = ValorBaixa;
            this.DataLancamento = DataLancamento;
            this.ValorDesconto = ValorDesconto;
            this.ValorAcrescimo = ValorAcrescimo;
            this.ValorTotalBaixa = ValorTotalBaixa;
            this.CodigoTipoCobranca = CodigoTipoCobranca;
            this.CodigoContaCorrente = CodigoContaCorrente;
            this.Observacao = Observacao;
            this.TipoBaixa = TipoBaixa;
            this.Cpl_Cobranca = Cpl_Cobranca;
        }
    }
}

