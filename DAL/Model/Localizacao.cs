namespace DAL.Model
{
    public class Localizacao
    {
        public int CodigoIndice { get; set; }
        public int CodigoEmpresa { get; set; }
        public string CodigoLocalizacao { get; set; }
        public string DescricaoLocalizacao{ get; set; }
        public long CodigoSituacao { get; set; }
        public long CodigoTipoLocalizacao { get; set; }
        public string Cpl_DescDDL { get; set; }

        public Localizacao()
        {
        }

        public Localizacao(int codigoIndice, int codigoEmpresa, string codigoLocalizacao, string descricaoLocalizacao, long codigoSituacao, long codigoTipoLocalizacao, string cpl_DescDDL)
        {
            CodigoIndice = codigoIndice;
            CodigoEmpresa = codigoEmpresa;
            CodigoLocalizacao = codigoLocalizacao;
            DescricaoLocalizacao = descricaoLocalizacao;
            CodigoSituacao = codigoSituacao;
            CodigoTipoLocalizacao = codigoTipoLocalizacao;
            Cpl_DescDDL = codigoLocalizacao.ToString() + " - " + descricaoLocalizacao.ToString();
        }
    }
}
