using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Persistence;

namespace DAL.Model
{
    public class Pessoa_Endereco : Habil_TipoDAL
    {
        private static int CodItem;
        public int _CodigoPessoa { get; set; }
        public int _CodigoItem { get; set; }
        public int _TipoEndereco { get; set; }
        public string _TipoEnderecoD { get; set; }
        public string _Logradouro { get; set; }
        public string _NumeroLogradouro { get; set; }
        public string _Complemento { get; set; }
        public Int64 _CodigoCEP { get; set; }
        public int _CodigoEstado { get; set; }
        public string _DescricaoEstado { get; set; }
        public Int64 _CodigoMunicipio { get; set; }
        public string _DescricaoMunicipio { get; set; }
        public int _CodigoBairro { get; set; }
        public string _DescricaoBairro { get; set; }
        public int _CodigoInscricao { get; set; }

        public Pessoa_Endereco()
        {
        }
        public Pessoa_Endereco (int CodigoItem, int TipoEndereco, string Logradouro, string NumeroLogradouro, string Complemento, Int64 CodigoCEP, int CodigoEstado, string DescricaoEstado , Int64 CodigoMunicipio, string DescricaoMunicipio, int CodigoBairro, string DescricaoBairro, int CodigoInscricao)
        {
            _CodigoPessoa = 0;
            CodItem++;
            if (CodigoItem == 0)
                _CodigoItem = CodItem;
            else
                _CodigoItem = CodigoItem;

            _TipoEndereco = TipoEndereco;
            _Logradouro = Logradouro;
            _NumeroLogradouro = NumeroLogradouro;
            _CodigoCEP = CodigoCEP;
            _Complemento = Complemento;
            _CodigoEstado = CodigoEstado;
            _DescricaoEstado = DescricaoEstado;
            _CodigoMunicipio = CodigoMunicipio;
            _DescricaoMunicipio = DescricaoMunicipio;
            _CodigoBairro = CodigoBairro;
            _DescricaoBairro = DescricaoBairro;
            _TipoEnderecoD = DescricaoHabil_Tipo(_TipoEndereco);
            _CodigoInscricao = CodigoInscricao;
        }
    }
}
