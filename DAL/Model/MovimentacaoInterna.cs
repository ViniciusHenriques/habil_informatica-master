using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class MovimentacaoInterna
    {
        public decimal CodigoIndice { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoIndiceLocalizacao { get; set; }
        public int CodigoLote { get; set; }
        public int CodigoProduto { get; set; }
        public int CodigoTipoOperacao { get; set; }
        public string TpOperacao { get; set; }
        public int CodigoUsuario { get; set; }
        public long CodigoMaquina { get; set; }
        public int CodigoDocumento { get; set; }
        public DateTime DtLancamento { get; set; }
        public DateTime DtValidade{ get; set; }
        public string  NumeroDoc{ get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorSaldoAnterior { get; set; }
        public decimal QtMovimentada { get; set; }
        public decimal VlSaldoFinal { get; set; }
        public decimal VlSaldoAjuste { get; set; }

        //view movimentação de estoque
        public string NrLote { get; set; }
        public string NomeEmpresa { get; set; }
        public string CodigoLocalizacao { get; set; }
        public string NomeProduto { get; set; }
        public string NomeUsuario { get; set; }
        public string NomeMaquina { get; set; }

        //ConMOvInterna
        public DateTime DtDe { get; set; }
        public DateTime DtAte { get; set; }
    }
}

