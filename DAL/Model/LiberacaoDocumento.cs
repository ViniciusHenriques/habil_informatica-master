using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class LiberacaoDocumento
    {
        public int CodigoLiberacao { get; set; }
        public decimal CodigoDocumento { get; set; }
        public int CodigoBloqueio { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoMaquina { get; set; }
        public DateTime DataLiberacao { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Cpl_DescricaoBloqueio { get; set; }
        public string Cpl_NomeUsuario { get; set; }
        public string Cpl_NomeMaquina { get; set; }
    }
    public class LiberacaoDocumentoGrid
    {
        public int CodigoLiberacao { get; set; }
        public int CodigoOrcamento { get; set; }
        public int CodigoPedido { get; set; }
        public DateTime DataLiberacao { get; set; }
        public DateTime DataLancamento { get; set; }
        public decimal Valor { get; set; }
        public string CodigoUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string Oper1{ get; set; }
        public string Oper2 { get; set; }
        public string Oper3 { get; set; }
        public int CodigoCliente { get; set; }
        public string NomeCliente { get; set; }
        public string Logradouro { get; set; }
        public string Municipio { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorAberto { get; set; }
        public decimal CreditoUsado { get; set; }
        public decimal LimiteCredito { get; set; }

        //Solicitação de Compra
        public int CodigoIndice { get; set; }
        public int CodigoDocumento { get; set; }
        public int CodigoSolicitacao { get; set; }
        public int CodigoSerieSolicitacao { get; set; }
        public decimal ValorVerba { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataValidade { get; set; }
        public int CodigoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }

        

}
}
