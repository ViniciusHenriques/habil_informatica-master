using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class IntegraDocumentoEletronico
    {
        public decimal Codigo { get; set; }
        public decimal CodigoDocumento { get; set; }
        public int RegistroEnviado { get; set; }
        public int IntegracaoRecebido { get; set; }
        public int IntegracaoProcessando { get; set; }
        public int IntegracaoRetorno { get; set; }
        public int RegistroDevolvido { get; set; }
        public int RegistroMensagem { get; set; }
        public string Mensagem { get; set; }
        public int CodigoAcao { get; set; }
        public int CodigoMaquina { get; set; }
        public int CodigoUsuario { get; set; }
    }
}
