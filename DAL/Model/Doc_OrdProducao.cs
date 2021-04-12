using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_OrdProducao
	{

        public decimal CodigoDocumento { get; set; }
        public decimal CodigoDocumentoOriginal { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoTipoOrcamento { get; set; }
        public int CodigoOperador { get; set; }
        public int CodigoProduto { get; set; }
        public string DGSerieDocumento { get; set; }
        public decimal NumeroDocumento { get; set; }
        public string DGNumeroDocumento { get; set; }
        public DateTime? DataHoraEmissao { get; set; }
		public DateTime? DataEncerramento { get; set; }
		public int CodigoTipoOperacao { get; set; }
        public decimal ValorTotal { get; set; }
        public string DescricaoDocumento { get; set; }
        public int CodigoTipoDocumento { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoGeracaoSequencialDocumento { get; set; }
        public decimal NumeroWeb { get; set; }
        public int CodigoAplicacaoUso { get; set; }
        //Complementos
        public long Cpl_CodigoTransportador { get; set; }
        public long Cpl_CodigoPessoa { get; set; }
        public int Cpl_Acao { get; set; }
        public int Cpl_Maquina { get; set; }
        public int Cpl_Usuario { get; set; }
        public string Cpl_Pessoa { get; set; }
        public string Cpl_NomeTabela { get; set; }
        public string Cpl_Situacao { get; set; }
        public string Cpl_DsAplicacaoUso { get; set; }
        public string Cpl_DsDescricao { get; set; }
        public string Cpl_DsTipoOperacao { get; set; }
        public string Cpl_NomeProduto { get; set; }
        public string Cpl_NomeEmpresa { get; set; }
        public string ObsComposicao { get; set; }

		public string formato { get; set; }
		public string LogoMarca { get; set; }
		public string Maquina { get; set; }
		public int Prazo{ get; set; }
		public decimal QtProduzir { get; set; }
		public decimal QtProduzida { get; set; }
		public decimal Quantidade { get; set; }
		public decimal QtAtendida { get; set; }
		public int CodigoComposto { get; set; }
		public int CodigoComponente { get; set; }
		public int CodigoLocalizacao { get; set; }
		public int CodigoLote { get; set; }



		//Complementos
		

	}
}
