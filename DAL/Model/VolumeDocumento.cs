namespace DAL.Model
{
    public class VolumeDocumento
    {
        public decimal CodigoDocumento { get; set; }
        public int CodigoIdVolume{ get; set; }
        public int CodigoEmbalagem { get; set; }
        public string DescricaoIdentificacao{ get; set; }

        public int CodigoProdutoDocumento { get; set; }
        public int CodigoVolume { get; set; }
        public decimal QtEmbalagem { get; set; }
    }
}
