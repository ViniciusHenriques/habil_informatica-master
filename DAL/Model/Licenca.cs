namespace DAL.Model
{
    public class Licenca
    {
        public long CodigoDaLicenca { get; set; }
        public long CodigoDoCliente { get; set; }
        public string NomeDoCliente { get; set; }
        public int NumeroDeUsuarios { get; set; }
        public string ServidorDoCliente { get; set; }
        public string BancoDoCliente { get; set; }
        public string UsuarioBancoDoCliente { get; set; }
        public string SenhaBancoDoCliente { get; set; }
        public long CodigoDaAtualizacaoBanco { get; set; }
    }

}