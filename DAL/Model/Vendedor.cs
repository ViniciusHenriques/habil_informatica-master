using System;

namespace DAL.Model
{
    public class Vendedor
    {
        public Vendedor()
        {
            this.Pessoa = new Pessoa();
        }

        public Vendedor(Int64 _CodigoVendedor,
                         Int64 _CodigoPessoa
                         )
        {
            CodigoVendedor = _CodigoVendedor;
            CodigoPessoa = _CodigoPessoa;
            this.Pessoa = new Pessoa();
        }

        public long CodigoVendedor { get; set; }
        public long CodigoPessoa { get; set; }
        public long CodigoUsuario { get; set; }
        public long CodigoSituacao { get; set; }
        public long CodigoTipoVendedor { get; set; }
        public long CodigoEmpresa { get; set; }
        public decimal PercentualComissao { get; set; }
        public Pessoa Pessoa { get; set; }
        public string NomePessoa { get { return Pessoa.NomePessoa; } }
    }
}
