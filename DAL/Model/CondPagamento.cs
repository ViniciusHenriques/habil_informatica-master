namespace DAL.Model
{
    public class CondPagamento
    {
        int CodItem = 0;
        public int CodigoCondPagamento { get; set; }
        public string DescricaoCondPagamento { get; set; }
        public int CodigoFmaPagamento { get; set; }
        public int CodigoTipoCobranca { get; set; }
        public int CodigoSituacao { get; set; }

        public int CodigoTipoPagamento { get; set; }

        public int DiaFixo { get; set; }

        public int QtdeParcelas { get; set; }

        public int Parcelas1 { get; set; }
        public int Parcelas2 { get; set; }
        public int Parcelas3 { get; set; }
        public int Parcelas4 { get; set; }
        public int Parcelas5 { get; set; }
        public int Parcelas6 { get; set; }
        public int Parcelas7 { get; set; }
        public int Parcelas8 { get; set; }
        public int Parcelas9 { get; set; }
        public int Parcelas10 { get; set; }

        public decimal Proporcao1 { get; set; }
        public decimal Proporcao2 { get; set; }
        public decimal Proporcao3 { get; set; }
        public decimal Proporcao4 { get; set; }
        public decimal Proporcao5 { get; set; }
        public decimal Proporcao6 { get; set; }
        public decimal Proporcao7 { get; set; }
        public decimal Proporcao8 { get; set; }
        public decimal Proporcao9 { get; set; }
        public decimal Proporcao10 { get; set; }

        public CondPagamento() { }
        public CondPagamento(int CodigoCondPagamento, string DescricaoCondPagamento) {

            CodItem++;
            if (CodigoCondPagamento == 0)
                this.CodigoCondPagamento= CodItem;
            else
                this.CodigoCondPagamento= CodigoCondPagamento;

            this.DescricaoCondPagamento = DescricaoCondPagamento;
        }

    }
}
