namespace DAL.Model
{
    public class DBTabela
    {
        public long IDTabela { get; set; }
        public long IDCampo { get; set; }
        public string Tabela { get; set; }
        public string NomeComum { get; set; }
        public string Coluna { get; set; }
        public string Tipo { get; set; }
        public int Tamanho { get; set; }
        public int Nulo { get; set; }
        public string BancoEstrutura { get; set; }
        public int RegistroUnico { get; set; }
        public string PopulaTabela { get; set; }
    }
}
