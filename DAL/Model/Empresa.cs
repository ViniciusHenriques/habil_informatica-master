using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Empresa
    {
        public Int64 CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string NomeFantasia { get; set; }

        public Int64 CodigoPessoa { get; set; }
        public int CodHabil_RegTributario { get; set; }
        public int CodigoSituacao { get; set; }

        public Empresa()
        {
        }
    }

}
