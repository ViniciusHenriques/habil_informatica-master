using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL.Model
{
    public class PerfilUsuario
    {
        public Int64 CodigoPflUsuario { get; set; }
        public string DescricaoPflUsuario { get; set; }

        public DateTime HoraInicial { get; set; }
        public DateTime HoraFinal { get; set; }

        public Boolean Domingo { get; set; }
        public Boolean Segunda { get; set; }
        public Boolean Terca { get; set; }
        public Boolean Quarta { get; set; }
        public Boolean Quinta { get; set; }
        public Boolean Sexta { get; set; }
        public Boolean Sabado { get; set; }

        public DateTime? IntervaloInicio1 { get; set; }
        public DateTime? IntervaloFim1 { get; set; }

        public DateTime? IntervaloInicio2 { get; set; }
        public DateTime? IntervaloFim2 { get; set; }

        public DateTime? IntervaloInicio3 { get; set; }
        public DateTime? IntervaloFim3 { get; set; }

        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }

        public int CodigoModuloEspecifico { get; set; }
    }
}
