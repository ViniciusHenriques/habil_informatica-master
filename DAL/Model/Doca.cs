using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Doca
    {
        
            public int CodigoDoca { get; set; }
            public string DescricaoDoca { get; set; }
            public int CodigoSituacao { get; set; }
            public int CodigoCliente { get; set; }
            public long CodigoEmpresa { get; set; }
            public string Cpl_NomeEmpresa { get; set; }
            public int Cont { get; set; }
            public DateTime DtLancamento { get; set; }
            public int CodigoDocumento { get; set; }
            public int NrDocumento { get; set; }
            public string NomePessoa { get; set; }
            public string NomeCliente { get; set; }
            public string NomeCidade { get; set; }
            public string NomeBairro{ get; set; }
            public string Transportadora { get; set; }
            public int ContagemdeItens { get; set; }
            public string NomeVendedor { get; set; }
    }
}

