
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class UploadDocumentoEletronico
    {
        public int CodigoIndex { get; set; }
        public string ChaveAcesso { get; set; }
        public decimal ValorBaseCalculoICMS { get; set; }
        public decimal ValorICMS { get; set; }
        public byte[] ArquivoXML { get; set; }
        public int CodigoTipoDocumento { get; set; }
    }
}