using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    //teste
	public class PessoaProdutoDocumento
	{
		int CodItem = 0;
		public int CodigoItem { get; set; }
		public decimal CodigoDocumento { get; set; }
		public decimal PrecoItem { get; set; }
		public long CodigoProduto { get; set; }
        public string CodigoProdutoPessoa { get; set; }
        public long CodigoPessoa { get; set; }
        public int CodigoSituacao { get; set; }
        public string OBSFinanceira { get; set; }
        public string OBSImposto { get; set; }
        public string DataDiaEntrega { get; set; }
        public short NaoAtendeItem { get; set; }
        public DateTime? DataResposta { get; set; }
        public DateTime? DataAprovacao { get; set; }

        //Complementares
        public string Cpl_DscSituacao { get; set; }
		public string Cpl_DscProduto { get; set; }


        public PessoaProdutoDocumento()
		{

		}

		public PessoaProdutoDocumento(int CodigoItem, decimal CodigoDocumento, decimal PrecoItem, string Cpl_DscProduto, int CodigoProduto,
            int CodigoPessoa, int CodigoSituacao, string OBSFinanceira, string OBSImposto, string DataDiaEntrega, short NaoAtendeItem,
            DateTime? DataResposta, DateTime? DataAprovacao)
		{
			CodItem++;
			if (CodigoItem == 0)
				this.CodigoItem = CodItem;
			else
				this.CodigoItem = CodigoItem;


			this.PrecoItem = PrecoItem;
			this.Cpl_DscProduto = Cpl_DscProduto;
			this.CodigoDocumento = CodigoDocumento;
			this.CodigoProduto = CodigoProduto;
			this.CodigoPessoa= CodigoPessoa;
            this.CodigoSituacao = CodigoSituacao;
            this.OBSFinanceira = OBSFinanceira;
            this.OBSImposto = OBSImposto;
            this.DataDiaEntrega = DataDiaEntrega;
            this.NaoAtendeItem = NaoAtendeItem;
            this.DataResposta = DataResposta;
            this.DataAprovacao = DataAprovacao;

        }
    }
}
