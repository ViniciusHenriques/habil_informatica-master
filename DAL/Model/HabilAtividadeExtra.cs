using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class HabilAtividadeExtra
    {
        public int CodigoIndex { get; set; }
        public int CodigoUsuario { get; set; }
        public string NomeTabela { get; set; }
        public string DescricaoAtividade { get; set; }
        public string Chave { get; set; }
        public DateTime DataHoraLancamento { get; set; }
        public DateTime DataHoraAtualizacao { get; set; }
        public string DescricaoFiltro { get; set; }
        public int CodigoSituacao { get; set; }
        public bool VisibleCheckBox { get; set; }
        public bool Impostos { get; set; }
        public string Cpl_DsSituacao { get; set; }
    }
}
