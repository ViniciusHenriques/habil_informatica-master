using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class ItemDocumento
    {
        private static int CodItem;
        public decimal CodigoDocumento { get; set; }
        public int CodigoItem { get; set; }
        public int CodigoUsuarioAtendente { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public int CodigoSituacao { get; set; }
        public string DescricaoItem { get; set; }


        public string Cpl_NomeUsuario { get; set; }
        public string Cpl_DescSituacao { get; set; }
        public ItemDocumento() { }
        public ItemDocumento(int CodigoItem,
                             decimal CodigoDocumento,
                             int CodigoUsuarioAtendente,
                             DateTime DataHoraInicio,
                             DateTime DataHoraFim,
                             int CodigoSituacao,
                             string DescricaoItem,
                             string Cpl_NomeUsuario,
                             string Cpl_DescSituacao)
        {
            CodItem++;
            if (CodigoItem == 0)
                this.CodigoItem = CodItem;
            else
                this.CodigoItem = CodigoItem;
            this.CodigoDocumento = CodigoDocumento;
            this.CodigoUsuarioAtendente = CodigoUsuarioAtendente;
            this.DataHoraInicio = DataHoraInicio;
            this.DataHoraFim = DataHoraFim;
            this.CodigoSituacao = CodigoSituacao;
            this.DescricaoItem = DescricaoItem;
            this.Cpl_NomeUsuario = Cpl_NomeUsuario;
            this.Cpl_DescSituacao = Cpl_DescSituacao;
        }
    }
}
