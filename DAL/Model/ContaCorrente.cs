using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class ContaCorrente
    {
        public int CodigoContaCorrente { get; set; }
        public string DescricaoContaCorrente { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoBanco { get; set; }
        public int CodigoPessoa { get; set; }
    }
}
