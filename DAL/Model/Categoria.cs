namespace DAL.Model
{
    public class Categoria
    {
        public int CodigoIndice { get; set; }
        public string CodigoCategoria { get; set; }
        public string DescricaoCategoria { get; set; }
        public int CodigoDepartamento { get; set; }
        public string DescricaoDepartamento { get; set; }
        public int CodigoGpoComissao { get; set; }
        public string DescricaoGpoComissao { get; set; }
        public string Cpl_DescDDL { get; set; }

        public Categoria()
        {
        }

        public Categoria(int codigoIndice, string codigoCategoria, string descricaoCategoria, int codigoDepartamento, string descricaoDepartamento, int codigoGpoComissao, string descricaoGpoComissao, string cpl_DescDDL)
        {
            CodigoIndice = codigoIndice;
            CodigoCategoria = codigoCategoria;
            DescricaoCategoria = descricaoCategoria;
            CodigoDepartamento = codigoDepartamento;
            DescricaoDepartamento = descricaoDepartamento;
            CodigoGpoComissao = codigoGpoComissao;
            DescricaoGpoComissao = descricaoGpoComissao;
            Cpl_DescDDL = codigoCategoria.ToString() + " - " + descricaoCategoria.ToString();
        }
    }
}
