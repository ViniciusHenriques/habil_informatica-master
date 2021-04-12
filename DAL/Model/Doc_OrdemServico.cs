using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_OrdemServico
    {
        public decimal CodigoDocumento { get; set; }
        public decimal CodigoDocumentoOriginal { get; set; }
        public int CodigoTipoDocumento { get; set; }
        public decimal NumeroDocumento { get; set; }
        public string DGSRDocumento { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoUsuarioResponsavel { get; set; }
        public string ObservacaoDocumento { get; set; }
        public DateTime DataHoraEmissao { get; set; }
        public decimal CodigoSolicAtendimento { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoClassificacao { get; set; }
        public int CodigoContato { get; set; }
        public int CodigoGeracaoSequencialDocumento { get; set; }

        public int CodigoCondicaoPagamento { get; set; }
        public int CodigoTipoCobranca { get; set; }
        public decimal ValorTotal { get; set; }
        //Complementares
        public int Cpl_CodigoPessoa { get; set; }
        public int Cpl_Maquina { get; set; }
        public int Cpl_Usuario { get; set; }
        public string Cpl_NomeTabela { get; set; }
        public string Cpl_Pessoa { get; set; }
        public string Cpl_Situacao { get; set; }
        public int Cpl_Acao { get; set; }
        public string DescricaoDocumento { get; set; }
        public string Cpl_MailSolicitante { get; set; }
        public string Cpl_FoneSolicitante { get; set; }
        public string Cpl_Classificacao { get; set; }

        public bool BtnAdd { get; set; }
        public bool BtnRemove { get; set; }
    }
}
