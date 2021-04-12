using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_CTe
    {
        public decimal CodigoDocumento { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoEmpresa { get; set; }
        public DateTime DataHoraEmissao { get; set; }
        public DateTime DataHoraLancamento { get; set; }
        public int CodigoTipoOperacao { get; set; }
        public decimal NumeroDocumento { get; set; }
        public string DGSRDocumento { get; set; }
        public decimal ValorTotal { get; set; }
        public string ObservacaoDocumento { get; set; }
        public string ChaveAcesso { get; set; }
        public Int64 CodigoGeracaoSequencialDocumento { get; set; }

        //Complementares
        public string Cpl_Transportador { get; set; }
        public string Cpl_Remetente{ get; set; }
        public string Cpl_Destinatario{ get; set; }

        public string Cpl_DsSituacao { get; set; }
        public int Cpl_Usuario { get; set; }
        public int Cpl_Maquina { get; set; }
        
        public string Cpl_NomeTabela { get; set; }
        public int Cpl_Acao { get; set; }
        public int Cpl_CodigoEstado { get; set; }


        //Complementar pessoas do documento
        public long Cpl_CodigoTomador { get; set; }
        public long Cpl_CodigoTransportador { get; set; }
        public long Cpl_CodigoDestinatario { get; set; }
        public long Cpl_CodigoRecebedor { get; set; }
        public long Cpl_CodigoRemetente { get; set; }
    }
}
