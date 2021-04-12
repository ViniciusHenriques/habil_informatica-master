using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class BIConsumoClienteProduto
    {

        public decimal CodigoIndex { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int CodigoGrupoPessoa { get; set; }
        public Int64 CodigoPessoa { get; set; }
        public Int64 CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public Int64 CodigoVendedor { get; set; }
        public decimal ValorMes1 { get; set; }
        public decimal QuantidadeMes1 { get; set; }
        public string DescricaoMes1 { get; set; }
        public decimal ValorMes2 { get; set; }
        public decimal QuantidadeMes2 { get; set; }
        public string DescricaoMes2 { get; set; }
        public decimal ValorMes3 { get; set; }
        public decimal QuantidadeMes3 { get; set; }
        public string DescricaoMes3 { get; set; }
        public decimal ValorMes4 { get; set; }
        public decimal QuantidadeMes4 { get; set; }
        public string DescricaoMes4 { get; set; }
        public decimal ValorMes5 { get; set; }
        public decimal QuantidadeMes5 { get; set; }
        public string DescricaoMes5 { get; set; }
        public decimal ValorMes6 { get; set; }
        public decimal QuantidadeMes6 { get; set; }
        public string DescricaoMes6 { get; set; }
        public decimal QuantidadeMedia { get; set; }
        public decimal NumeroProjecao { get; set; }
        public decimal QuantidadeComprar { get; set; }
        public decimal PrecoVenda { get; set; }
        public int CodigoDepartamento { get; set; }


        //COMPLEMENTOS
        public string strCpl_Mes1 { get; set; }
        public string strCpl_Mes2 { get; set; }
        public string strCpl_Mes3 { get; set; }
        public string strCpl_Mes4 { get; set; }
        public string strCpl_Mes5 { get; set; }
        public string strCpl_Mes6 { get; set; }
        public string Cpl_DescricaoDepartamento { get; set; }

    }
}
