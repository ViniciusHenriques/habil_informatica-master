using System;

using DAL.Persistence;

namespace DAL.Model
{
    public class Pessoa_Contato : Habil_TipoDAL
    {
        private static int CodItem;
        public string _NomeContatoCombo { get; set; }

        public int _CodigoPessoa { get; set; }

        public int _CodigoItem { get; set; }
        public int _TipoContato { get; set; }
        public string _TipoContatoD { get; set; }
        public string _NomeContato { get; set; }
        public string _Fone1 { get; set; }
        public string _Fone2 { get; set; }
        public string _Fone3 { get; set; }
        public string _MailNFE { get; set; }
        public string _MailNFSE { get; set; }
        public string _Mail1 { get; set; }
        public string _Mail2 { get; set; }
        public string _Mail3 { get; set; }
        public byte[] _Foto { get; set; }
        public int _CodigoInscricao { get; set; }
        public string _EmailSenha { get; set; }
        public int _FoneWhatsApp { get; set; }
        public int _CodigoPais { get; set; }

        public Pessoa_Contato()
        {
        }
        public Pessoa_Contato (int CodigoItem, int TipoContato, string NomeContato, string Fone1, string Fone2, string Fone3, string MailNFE,string MailNFSE, string Mail1 , string Mail2, string Mail3, byte[] Foto, int CodigoInscricao,string EmailSenha, int FoneWhatsApp, int CodigoPais)
        {
            _CodigoPessoa = 0;
            CodItem++;
            if (CodigoItem == 0)
                _CodigoItem = CodItem;
            else
                _CodigoItem = CodigoItem;

            _TipoContato = TipoContato;
            _NomeContato = NomeContato;
            _Fone1 = Fone1;
            _Fone2 = Fone2;
            _Fone3 = Fone3;
            _MailNFE = MailNFE;
            _MailNFSE = MailNFSE;
            _Mail1 = Mail1;
            _Mail2 = Mail2;
            _Mail3 = Mail3;

            _NomeContatoCombo = _NomeContato + "(" + _CodigoItem.ToString() + ")";

            _TipoContatoD = DescricaoHabil_Tipo(_TipoContato);


            _Foto = Foto;

            _CodigoInscricao = CodigoInscricao;

            _EmailSenha = EmailSenha;
            _FoneWhatsApp = FoneWhatsApp;
            _CodigoPais = CodigoPais;
        }
    }
}
