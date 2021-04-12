using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Comprador
    {
        public Int64 CodigoComprador { get; set; }
        public string NomeComprador { get; set; }
        public Int64 CodigoPessoa { get; set; }
        public Int64 CodigoUsuario { get; set; }

        public Comprador()
        {
        }

        public Comprador(Int64 _CodigoComprador,
                         Int64 _CodigoPessoa,
                         Int64 _CodigoUsuario)
        {
            CodigoComprador = _CodigoComprador;
            CodigoPessoa = _CodigoPessoa;
            CodigoUsuario = _CodigoUsuario;
        }
}

}
