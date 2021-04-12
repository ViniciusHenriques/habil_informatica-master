using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class IntegracaoPedido
    {
        public decimal Codigo { get; set; }
        public string NumeroInscricao { get; set; }
        public string NomeTomador{get;set;}
        public decimal CodigoMunicipio { get; set; }
        public decimal CodigoCNAE { get; set; }
        public string DescricaoServico { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoItem { get; set; }
        public decimal ValorTotalServico { get; set; }
        public decimal ValorTotalNFSe { get; set; }
        public decimal ValorAliquota { get; set; }
        public string CodigoMunicipioServico { get; set; }
        public int CodigoSituacao { get; set; }
        public string Mensagem { get; set; }
        public int CodigoEmpresa { get; set; }
        public decimal CodigoDocumento { get; set; }
        public string Mail_NFSe { get; set; }
        public Int64 CodigoCEP { get; set; }
        public string Logradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string DescricaoBairro { get; set; }
        public string NumeroIERG { get; set; }
        public string DescricaoNFSE { get; set; }
        public decimal CodigoServicoLei { get; set; }
    }
}
