using System;
using System.Collections.Generic;

namespace DAL
{
    public class HabilEmailCriado
    {
        public long CD_INDEX { get; set; }
        public DateTime DT_LANCAMENTO { get; set; }  
        public long CD_USU_REMETENTE { get; set; }
        public string TX_CORPO { get; set; }
        public long CD_SITUACAO { get; set; }
        public DateTime? DT_ENVIO { get; set; }
        public string TX_ERRO { get; set; }
        public int NR_TENTA_ENVIO { get; set; }
        public int IN_HTML { get; set; }
        public string TX_ASSUNTO { get; set; }

        public string  EMAIL_REMETENTE { get; set; }
        public string  SENHA_REMETENTE { get; set; }

        //Complementos
        public string CPL_DESTINATARIOS { get; set; }
        public string CPL_SITUACAO { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public List<HabilEmailDestinatario> listaDestinatarios { get; set; }
        public List<HabilEmailDestinatario> listaDestinatariosComCopia { get; set; }
        public List<HabilEmailDestinatario> listaDestinatariosComCopiaOculta { get; set; }
        public List<HabilEmailAnexo> listaAnexos { get; set; }


        public HabilEmailCriado()
        {
            this.listaDestinatarios = new List<HabilEmailDestinatario>();
            this.listaDestinatariosComCopia = new List<HabilEmailDestinatario>();
            this.listaDestinatariosComCopiaOculta = new List<HabilEmailDestinatario>();
            this.listaAnexos = new List<HabilEmailAnexo>();
        }
    }

}
