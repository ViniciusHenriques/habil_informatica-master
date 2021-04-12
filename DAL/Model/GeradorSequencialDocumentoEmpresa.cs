using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class GeradorSequencialDocumentoEmpresa
    {


        public int CodigoEmpresa { get; set; }
        public int CodigoGeradorSequencialDocumento { get; set; }
        public string Cpl_SerieConteudo { get; set; }
        public Int64 Cpl_SerieNumero { get; set; }
        public string Cpl_Descricao { get; set; }
        public int Cpl_TipoDocumento { get; set; }
        public string Cpl_Nome { get; set; }
        public int Cpl_CodigoTipoDocumento { get; set; }
        public GeradorSequencialDocumentoEmpresa() { }

        public GeradorSequencialDocumentoEmpresa(int CodigoEmpresa, int CodigoGeradorSequencialDocumento, string Cpl_SerieConteudo, Int64 Cpl_SerieNumero, string Cpl_Descricao, int Cpl_CodigoTipoDocumento)
        {

            this.CodigoEmpresa = CodigoEmpresa;
            this.CodigoGeradorSequencialDocumento = CodigoGeradorSequencialDocumento;
            this.Cpl_Descricao = Cpl_Descricao;
            this.Cpl_SerieConteudo = Cpl_SerieConteudo;
            this.Cpl_SerieNumero = Cpl_SerieNumero;
            this.Cpl_CodigoTipoDocumento = Cpl_CodigoTipoDocumento;
        }
    }
}
