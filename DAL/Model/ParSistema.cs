using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ParSistema
    {
        public Int64 CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string CaracteristaCategoria { get; set; }
        public string CaracteristaLocalizacao { get; set; }
        public bool   LocalizacaoEspelhada { get; set; }
        public int CodigoTipoOperacao { get; set; }
        public string CorPadrao { get; set; }
        public string CorFundo { get; set; }
        public int TipoMenu { get; set; }
        public int TipoAjusteInventario{ get; set; }
        public short DiasValidadeOrc{ get; set; }
        public decimal ValorPedidoParaFreteMinimo { get; set; }
        public decimal ValorFreteMinimo { get; set; }
        public int CodigoSequenciaGeracaoNFe { get; set; }
        public int TipoListagemPedido { get; set; }
        public bool ConferePedidos { get; set; }
        public bool CriticaRegras { get; set; }
        public int NumeroHorasEnvioAlerta { get; set; }


        public ParSistema()
        {
        }
    }

}
