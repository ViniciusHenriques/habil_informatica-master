using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class RegraFrete
    {
        public int CodigoIndex { get; set; }
        public int CodigoTransportador { get; set; }
        public decimal DePara11 { get; set; }
        public decimal DePara12 { get; set; }
        public decimal DePara21 { get; set; }
        public decimal DePara22 { get; set; }
        public decimal DePara31 { get; set; }
        public decimal DePara32 { get; set; }
        public decimal DePara41 { get; set; }
        public decimal DePara42 { get; set; }
        public decimal DePara51 { get; set; }
        public decimal DePara52 { get; set; }
        public decimal DePara61 { get; set; }
        public decimal DePara62 { get; set; }
        public decimal DePara71 { get; set; }
        public decimal DePara72 { get; set; }
        public decimal DeParaPct11 { get; set; }
        public decimal DeParaPct12 { get; set; }
        public decimal DeParaPct13 { get; set; }
        public decimal DeParaPct14 { get; set; }
        public decimal DeParaPct15 { get; set; }
        public decimal DeParaPct16 { get; set; }
        public decimal DeParaPct17 { get; set; }
        public decimal DeParaExcedente1 { get; set; }
        public decimal DeParaExcedente2 { get; set; }


        public decimal ValorSeguro { get; set; }
        public decimal ValorSeguroMinimo { get; set; }
        public decimal ValorPedagioMaximo { get; set; }
        public decimal IndicadorTipoCalculo { get; set; }
        public int IndicadorCalcularAdValorDePara1 { get; set; }
        public int IndicadorCalcularAdValorDePara2 { get; set; }
        public decimal ValorFreteMinimo { get; set; }
        public decimal ValorGRIS { get; set; }
        public string Inscricao { get; set; }
        public string Regiao { get; set; }
        public decimal ValorGRISMinimo { get; set; }
        public decimal ValorPedagio { get; set; }
        public decimal ValorAD { get; set; }
        public decimal ValorPorTonelada { get; set; }
        public decimal ValorPesoCubado { get; set; }
        //COMPLEMENTARES
        public string Cpl_NomeTransportador { get; set; }
        public string Cpl_InscricaoTransportador { get; set; }
        public string Cpl_ComboRegras { get; set; }
    }
}
