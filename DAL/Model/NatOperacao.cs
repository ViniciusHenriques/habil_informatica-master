using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class NatOperacao
    {
        public long CodigoNaturezaOperacao{ get; set; }
        public string DescricaoNaturezaOperacao { get; set; }
        public long CodigoNaturezaOperacaoContraPartida { get; set; }
        public long CodigoNaturezaOperacaoST { get; set; }

        public string Cpl_ComboDescricaoNatOperacao { get; set; }

        public NatOperacao()
        {
        }

        public NatOperacao(long _CodigoNaturezaOperacao,
                         string _DescricaoNaturezaOperacao,
                         long _CodigoNaturezaOperacaoContraPartida,
                         long _CodigoNaturezaOperacaoST)
        {

            this.CodigoNaturezaOperacao = _CodigoNaturezaOperacao;
            this.DescricaoNaturezaOperacao = _DescricaoNaturezaOperacao;
            this.CodigoNaturezaOperacaoContraPartida = _CodigoNaturezaOperacaoContraPartida;
            this.CodigoNaturezaOperacaoST = _CodigoNaturezaOperacaoST;

        }
    }
}
