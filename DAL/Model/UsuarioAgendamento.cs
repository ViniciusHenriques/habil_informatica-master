using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class UsuarioAgendamento
    {
        public int CodigoAgendamento { get; set; }
        public int CodigoUsuario { get; set; }

        public string Cpl_NomeUsuario { get; set; }
    }
}
