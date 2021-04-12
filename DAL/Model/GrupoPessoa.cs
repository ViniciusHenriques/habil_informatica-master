using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class GrupoPessoa
    {
        public int CodigoGrupoPessoa { get; set; }
        public string DescricaoGrupoPessoa { get; set; }
        public int CodigoSituacao { get; set; }
        public bool GerarMatricula { get; set; }
    }
}
