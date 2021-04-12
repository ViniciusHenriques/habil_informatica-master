using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class EventoDocumento
    {


        private static int CodItem;


        public decimal CodigoDocumento { get; set; }
        public int CodigoEvento { get; set; }
        public int CodigoSituacao { get; set; }
        public DateTime DataHoraEvento { get; set; }
        public long CodigoMaquina { get; set; }
        public int CodigoUsuario { get; set; }

        public string Cpl_Situacao { get; set; }
        public string Cpl_NomeUsuario { get; set; }
        public string Cpl_NomeMaquina { get; set; }

        public EventoDocumento()
        {
        }
        public EventoDocumento(int CodigoEvento,
                              int CodigoSituacao,
                              DateTime DataHoraEvento,
                              long CodigoMaquina,
                              int CodigoUsuario)
                              
        {
            CodItem++;
            if (CodigoEvento == 0)
                this.CodigoEvento= CodItem;
            else
                this.CodigoEvento = CodigoEvento;
            this.CodigoDocumento = CodigoDocumento;
            this.CodigoEvento = CodigoEvento;
            this.CodigoSituacao = CodigoSituacao;
            this.DataHoraEvento = DataHoraEvento;
            this.CodigoMaquina = CodigoMaquina;
            this.CodigoUsuario = CodigoUsuario;

        }
    }
}
