using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Persistence;

namespace DAL.Model
{
    public class Pessoa_Inscricao:Habil_TipoDAL
    {
        private static int CodItem;
        public int _CodigoPessoa { get; set; }
        public int _CodigoItem { get; set; }
        public int _TipoInscricao { get; set; }
        public string _TipoInscricaoD { get; set; }
        public string _NumeroInscricao { get; set; }
        public string _NumeroIERG { get; set; }
        public string _NumeroIM { get; set; }
        public DateTime? _DataDeAbertura { get; set; }
        public DateTime? _DataDeEncerramento { get; set; }
        public string _OBS { get; set; }
        public int CodigoPais { get; set; }

        public string _DcrInscricao { get; set; }

        public Pessoa_Inscricao()
        {
        }

        public Pessoa_Inscricao (int CodigoItem, int TipoInscricao, string NumeroInscricao, string NumeroIERG, string NumeroIM, DateTime? DataDeAbertura , DateTime? DataDeEncerramento ,  string OBS , int _CodigoPais)
        {
            CodItem++;
            _CodigoPessoa = 0;
            if (CodigoItem == 0)
                _CodigoItem = CodItem;
            else
                _CodigoItem = CodigoItem;

            _TipoInscricao = TipoInscricao;
            _NumeroInscricao = NumeroInscricao ;
            _NumeroIERG = NumeroIERG ;
            _NumeroIM = NumeroIM;
            _DataDeAbertura = DataDeAbertura;
            _DataDeEncerramento = DataDeEncerramento;
            _OBS = OBS;

            _TipoInscricaoD = DescricaoHabil_Tipo(TipoInscricao);

            _DcrInscricao = NumeroInscricao.ToString() + " (" + TipoInscricao.ToString() + ") " + NumeroIERG;
            CodigoPais = _CodigoPais;
        }
    }
}
