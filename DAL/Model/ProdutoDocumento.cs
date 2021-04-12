using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
	public class ProdutoDocumento
	{
		int CodItem = 0;
		public int CodigoItem { get; set; }
		public decimal CodigoDocumento { get; set; }
		public string Unidade { get; set; }
		public decimal Quantidade { get; set; }
		public decimal PrecoItem { get; set; }
		public int CodigoProduto { get; set; }
        public int CodigoSistemaAnterior { get; set; }
		public decimal ValorTotalItem { get; set; }
		public decimal ValorDesconto { get; set; }
		public decimal QuantidadeAtendida { get; set; }
		public decimal QuantidadePendente { get; set; }
		public int CodigoSituacao { get; set; }
		public int CodigoLocalizacao { get; set; }
		public decimal ValorVolume { get; set; }
		public decimal ValorFatorCubagem { get; set; }
		public decimal ValorPeso { get; set; }
		public ImpostoProdutoDocumento Impostos { get; set; }
		//Complementares
		public string Cpl_DscSituacao { get; set; }
		public string Cpl_DscProduto { get; set; }
		public int Cpl_CodigoBIConsumoClienteProduto { get; set; }
		public decimal Cpl_PrecoVenda { get; set; }

		public decimal PerQuebraComponente { get; set; }
		public short Cpl_CodigoRoteiro { get; set; }
		public string Cpl_DescRoteiro { get; set; }
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public int LocalizacoaProducao { get; set; }
        public decimal CodigoCFOP { get; set; }
        public string DsMarca { get; set; }


        public ProdutoDocumento()
		{
			Impostos = new ImpostoProdutoDocumento();

		}

		public ProdutoDocumento(int CodigoItem, decimal CodigoDocumento, decimal Quantidade, decimal PrecoItem, string Cpl_DscProduto, string Unidade, int CodigoProduto, decimal ValorTotalItem, int Cpl_CodigoBIConsumoClienteProduto)
		{
			CodItem++;
			if (CodigoItem == 0)
				this.CodigoItem = CodItem;
			else
				this.CodigoItem = CodigoItem;


			this.Unidade = Unidade;
			this.Quantidade = Quantidade;
			this.PrecoItem = PrecoItem;
			this.Cpl_DscProduto = Cpl_DscProduto;
			this.CodigoDocumento = CodigoDocumento;
			this.CodigoProduto = CodigoProduto;
			this.ValorTotalItem = ValorTotalItem;
			this.Cpl_CodigoBIConsumoClienteProduto = Cpl_CodigoBIConsumoClienteProduto;
		}
	}
}
