using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class GeracaoSequencialDocumento
    {
        public int CodigoGeracaoSequencial { get; set; }
        public int CodigoTipoDocumento { get; set; }
        public int CodigoEmpresa { get; set; }
        public string SerieConteudo { get; set; }
        public Int64 SerieNumero { get; set; }
        public DateTime Validade { get; set; }
        public decimal NumeroInicial { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int CodigoSituacao { get; set; }

        public string Cpl_Estacao { get; set; }
        public string Cpl_Usuario{ get; set; }

    }
}
