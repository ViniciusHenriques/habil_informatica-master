using System;

namespace DAL.Model
{
    public class ItemDaLicenca
    {
        public long     CodigoDoItem { get; set; }
        public long     CodigoDaLicenca { get; set; }
        public DateTime DataDeValidade { get; set; }
        public string   Guid { get; set; }
        public string   ChaveDeAutenticacao { get; set; }
    }

}
