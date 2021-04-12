using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_SolicitacaoAtendimento
    {
        public decimal CodigoDocumento{ get; set; }
        public int CodigoTipoSolicitacao { get; set; }
        public int CodigoSituacao { get; set; }
        public DateTime DataHoraEmissao { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoNivelPrioridade { get; set; }
        public decimal NumeroDocumento { get; set; }
        public string DGSerieDocumento { get; set; }
        public string DescricaoDocumento { get; set; }
        public DateTime DataConclusao { get; set; }
        public int CodigoGeracaoSequencialDocumento { get; set; }
        public int CodigoTipoDocumento { get; set; }
        public int CodigoContato { get; set; }
        public decimal HorasPrevistas { get; set; }
        public decimal ValorTotal { get; set; }
        //complementares

        public int Cpl_CodigoPessoa { get; set; }
        public string Cpl_MailSolicitante { get; set; }
        public string Cpl_FoneSolicitante { get; set; }
        public int Cpl_Maquina { get; set; }
        public int Cpl_Usuario { get; set; }
        public string Cpl_NomeTabela { get; set; }
        public string Cpl_Pessoa { get; set; }
        public string Cpl_Situacao { get; set; }
    }
}
