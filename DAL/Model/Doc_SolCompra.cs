using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class Doc_SolCompra
    {

        public decimal CodigoDocumento { get; set; }
		public decimal NumeroDocumento { get; set; }
		public decimal ValorTotal { get; set; }
		public decimal NumeroWeb { get; set; }

		public int CodigoEmpresa { get; set; }
		public int CodigoTipoDocumento { get; set; }
		public int CodigoSituacao { get; set; }
		public int CodigoComprador { get; set; }
		public int CodigoUsuario { get; set; }
		public int CodigoGeracaoSequencialDocumento { get; set; }


		public DateTime DataHoraEmissao { get; set; }
        public DateTime DataValidade { get; set; }

        public string MotivoCancelamento { get; set; }
        public string DescricaoDocumento { get; set; }
        public string Cpl_NomeTabela { get; set; }
		public int Cpl_Maquina { get; set; }
		public int Cpl_Usuario { get; set; }
		public string Cpl_NomeUsuario { get; set; }
		public string Cpl_Pessoa { get; set; }
		public long Cpl_CodigoPessoa { get; set; }
		public string Cpl_Situacao { get; set; }
		public int CodigoFornecedor { get; set; }

	}
}
