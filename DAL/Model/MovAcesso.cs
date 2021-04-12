using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class MovAcesso
    {
        public long CodMovAcesso { get; set; }
        public DateTime? DataHoraEntrada { get; set; }
        public DateTime? DataHoraSaida { get; set; }
        public int CodTipoAcesso { get; set; }
        public string TipoAcesso { get; set; }
        public string Documento { get; set; }
        public long CodPessoa { get; set; }
        public string Pessoa { get; set; }
        public int CodContato { get; set; }
        public string Contato { get; set; }
        public long CodVeiculo { get; set; }
        public string Veiculo { get; set; }
        public string Observacoes { get; set; }

        public MovAcesso()
        {
        }

        public MovAcesso(long _CodMovAcesso,
                         DateTime _DataHoraEntrada,
                         int _CodTipoAcesso,
                         string _Documento,
                         long _CodPessoa,
                         int _CodContato,
                         long _CodVeiculo,
                         string _Observacoes)
        {

            this.CodMovAcesso = _CodMovAcesso;
            this.DataHoraEntrada = _DataHoraEntrada;
            this.CodTipoAcesso = _CodTipoAcesso;
            this.Documento = _Documento;
            this.CodPessoa = _CodPessoa;
            this.CodContato = _CodContato;
            this.CodVeiculo = _CodVeiculo;
            this.Observacoes = _Observacoes;

        }
    }
}
