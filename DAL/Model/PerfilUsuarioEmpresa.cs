using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class PerfilUsuarioEmpresa
    {
        public Int64 CodigoPflUsuario { get; set; }
        public string DescricaoPflUsuario { get; set; }
        public Int64 CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public Boolean Liberado { get; set; }
    }
}
