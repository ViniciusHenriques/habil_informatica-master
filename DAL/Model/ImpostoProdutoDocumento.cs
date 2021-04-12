using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ImpostoProdutoDocumento
    {
        public decimal CodigoDocumento { get; set; }
        public int CodigoProdutoDocumento { get; set; }

        public decimal CodigoCST_ICMS { get; set; }
        public decimal ValorBaseCalculoICMS { get; set; }
        public decimal PercentualICMS { get; set; }
        public decimal ValorICMS { get; set; }
        public decimal PercentualICMS_Interno { get; set; }
        public decimal ValorReducaoBaseCalculoICMS { get; set; }
        public decimal ValorReducaoBaseCalculoICMS_Interno { get; set; }
        public decimal PercentualICMS_ST { get; set; }

        public decimal ValorMVA_Entrada { get; set; }
        public decimal ValorMVA_Saida { get; set; }

        public decimal CodigoCST_IPI { get; set; }
        public decimal ValorBaseCalculoIPI{ get; set; }
        public decimal PercentualIPI { get; set; }
        public decimal ValorIPI { get; set; }
        public int CodigoEnquadramento { get; set; }

        public decimal CodigoCST_PIS { get; set; }
        public decimal ValorBaseCalculoPIS{ get; set; }
        public decimal PercentualPIS { get; set; }
        public decimal ValorPIS { get; set; }

        public decimal CodigoCST_COFINS { get; set; }
        public decimal ValorBaseCalculoCOFINS{ get; set; }
        public decimal PercentualCOFINS { get; set; }
        public decimal ValorCOFINS{ get; set; }

        public ImpostoProdutoDocumento()
        {
        }

        public ImpostoProdutoDocumento(decimal codigoDocumento, int codigoProdutoDocumento, decimal codigoCST_ICMS, decimal valorBaseCalculoICMS, decimal percentualICMS, decimal valorICMS, decimal percentualICMS_Interno, decimal valorReducaoBaseCalculoICMS, decimal valorReducaoBaseCalculoICMS_Interno, decimal percentualICMS_ST, decimal valorMVA_Entrada, decimal valorMVA_Saida, decimal codigoCST_IPI, decimal valorBaseCalculoIPI, decimal percentualIPI, decimal valorIPI, int codigoEnquadramento, decimal codigoCST_PIS, decimal valorBaseCalculoPIS, decimal percentualPIS, decimal valorPIS, decimal codigoCST_COFINS, decimal valorBaseCalculoCOFINS, decimal percentualCOFINS, decimal valorCOFINS)
        {
            CodigoDocumento = codigoDocumento;
            CodigoProdutoDocumento = codigoProdutoDocumento;
            CodigoCST_ICMS = codigoCST_ICMS;
            ValorBaseCalculoICMS = valorBaseCalculoICMS;
            PercentualICMS = percentualICMS;
            ValorICMS = valorICMS;
            PercentualICMS_Interno = percentualICMS_Interno;
            ValorReducaoBaseCalculoICMS = valorReducaoBaseCalculoICMS;
            ValorReducaoBaseCalculoICMS_Interno = valorReducaoBaseCalculoICMS_Interno;
            PercentualICMS_ST = percentualICMS_ST;
            ValorMVA_Entrada = valorMVA_Entrada;
            ValorMVA_Saida = valorMVA_Saida;
            CodigoCST_IPI = codigoCST_IPI;
            ValorBaseCalculoIPI = valorBaseCalculoIPI;
            PercentualIPI = percentualIPI;
            ValorIPI = valorIPI;
            CodigoEnquadramento = codigoEnquadramento;
            CodigoCST_PIS = codigoCST_PIS;
            ValorBaseCalculoPIS = valorBaseCalculoPIS;
            PercentualPIS = percentualPIS;
            ValorPIS = valorPIS;
            CodigoCST_COFINS = codigoCST_COFINS;
            ValorBaseCalculoCOFINS = valorBaseCalculoCOFINS;
            PercentualCOFINS = percentualCOFINS;
            ValorCOFINS = valorCOFINS;
        }
    }
}
