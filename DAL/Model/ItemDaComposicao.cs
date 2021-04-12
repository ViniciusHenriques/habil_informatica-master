using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
	public class ItemDaComposicao
	{
		public int CodigoComposto { get; set; }
		public int CodigoComponente { get; set; }
		public decimal ValorCustoComponente { get; set; }
		public decimal ValorTotal { get; set; }
		public decimal PerQuebraComponente { get; set; }
		public decimal QuantidadeComponente { get; set; }
		public string DescricaoComponente { get; set; }
		public string Observacao { get; set; }
		public short CodigoRoteiro { get; set; }
		public string DescRoteiro { get; set; }
		public string Unidade { get; set; }

		public decimal QuantidadeAdd { get; set; }
		public decimal QuantidadeRet { get; set; }
		public decimal QuantidadeUtil { get; set; }
		public decimal Quantidade{ get; set; }
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
	}
}
