namespace DAL.Model
{
    public class TabIcms
    {
        public long CodTabAliqIcms { get; set; }
        public int CodOrigem { get; set; }
        public int CodDestino { get; set; }
        public string UFOrigem { get; set; }
        public string UFDestino { get; set; }
        public double IcmsInterno { get; set; }
        public double IcmsInterEstadual { get; set; }
        public double IcmsExterno { get; set; }

        public TabIcms()
        {
        }

        public TabIcms(long _CodTabAliqIcms,
                         int _CodOrigem,
                         int _CodDestino,
                         string _UFOrigem,
                         string _UFDestino,
                         double _IcmsInterno,
                         double _IcmsInterEstadual,
                         double _IcmsExterno)
        {

            this.CodTabAliqIcms = _CodTabAliqIcms;
            this.CodOrigem = _CodOrigem;
            this.CodDestino = _CodDestino;
            this.UFOrigem = _UFOrigem;
            this.UFDestino = _UFDestino;
            this.IcmsInterno = _IcmsInterno;
            this.IcmsInterEstadual = _IcmsInterEstadual;
            this.IcmsExterno = _IcmsExterno;
        }
    }
}
