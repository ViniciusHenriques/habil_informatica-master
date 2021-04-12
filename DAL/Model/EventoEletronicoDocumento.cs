using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class EventoEletronicoDocumento
    {

        private static int CodItem;

        public decimal CodigoDocumento { get; set; }
        public int CodigoEvento { get; set; }
        public int CodigoSituacao { get; set; }
        public DateTime DataHoraEvento { get; set; }
        public long CodigoMaquina { get; set; }
        public int CodigoUsuario { get; set; }
        public int NumeroSequencia { get; set; }
        public string Motivo { get; set; }
        public int CodigoTipoEvento { get; set; }
        public string Retorno { get; set; }
        

        public string Cpl_Situacao { get; set; }
        public string Cpl_NomeUsuario { get; set; }
        public string Cpl_NomeMaquina { get; set; }
        public string Cpl_TipoEvento { get; set; }
        public EventoEletronicoDocumento()
        {
        }
        public EventoEletronicoDocumento(int CodigoEvento,
                              int CodigoSituacao,
                              DateTime DataHoraEvento,
                              long CodigoMaquina,
                              int CodigoUsuario,
                              int NumeroSequencia,
                              string Motivo,
                              string Retorno,
                              int CodigoTipoEvento,
                              string Cpl_TipoEvento,
                              string Cpl_NomeUsuario,
                              string Cpl_NomeMaquina,
                              string Cpl_Situacao)
                              
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
            this.NumeroSequencia = NumeroSequencia;
            this.Motivo = Motivo;
            this.CodigoTipoEvento = CodigoTipoEvento;
            this.Cpl_TipoEvento = Cpl_TipoEvento;
            this.Cpl_NomeUsuario = Cpl_NomeUsuario;
            this.Cpl_NomeMaquina = Cpl_NomeMaquina;
            this.Retorno = Retorno;
            this.Cpl_Situacao = Cpl_Situacao;
        }
    }
}
