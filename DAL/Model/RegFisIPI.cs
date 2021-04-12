using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class RegFisIPI
    {
        public decimal CodigoRegraFiscalIPI { get; set; }
        public DateTime DtVigencia { get; set; }
        public DateTime DtAtualizacao { get; set; }
        public string CodigoEx { get; set; }
        public decimal PercentualIPI { get; set; }
        public short CodigoEnquadramento { get; set; }
        public short CodigoSituacaoTributaria{ get; set; }
        public int CodigoTipo { get; set; }
        public int CodigoSituacao { get; set; }
        public string CodigoNCM { get; set; }
        public string DescricaoNCM { get; set; }
        public string DescricaoTipo{ get; set; }
        public string DescricaoSituacao { get; set; }
    }
}