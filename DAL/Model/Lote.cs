using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Lote
    {
        public int CodigoIndice { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoProduto { get; set; }
        public int CodigoSituacao { get; set; }
        public string NumeroLote { get; set; }
        public string SerieLote { get; set; }
        public DateTime DataValidade { get; set; }
        public DateTime DataFabricacao { get; set; }
        public decimal QuantidadeLote { get; set; }
        public string NomeEmpresa { get; set; }
        public string NomeProduto { get; set; }
        public string DescricaoSituacao { get; set; }
        public string Cpl_DescDDL { get; set; }

        public Lote()
        {
        }

        public Lote(int codigoIndice, int codigoEmpresa, int codigoProduto, int codigoSituacao, string descricaoSituacao, string numeroLote, string serieLote, DateTime dataValidade, DateTime dataFabricacao, decimal quantidadeLote, string nomeEmpresa, string nomeProduto, string cpl_DescDDL)
        {
            CodigoIndice = codigoIndice;
            CodigoEmpresa = codigoEmpresa;
            CodigoProduto = codigoProduto;
            CodigoSituacao = codigoSituacao;
            DescricaoSituacao = descricaoSituacao;
            NumeroLote = numeroLote;
            SerieLote = serieLote;
            DataValidade = dataValidade;
            DataFabricacao = dataFabricacao;
            QuantidadeLote = quantidadeLote; 
            NomeEmpresa = nomeEmpresa;
            NomeProduto = nomeProduto;
            Cpl_DescDDL = NumeroLote.ToString() + " - " + DataFabricacao.ToString() + " - " + DataValidade.ToString();

            
        }

    }
}
