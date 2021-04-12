using System;


namespace DAL.Model
{
    public class TipoServico
    {
        int CodItem = 0;
        public int CodigoServico { get; set; }
        public int CodigoTipoServico { get; set; }
        public string DescricaoTipoServico { get; set; }
        public int CodigoSituacao { get; set; }
        public decimal ValorISSQN { get; set; }
        public decimal CodigoCNAE { get; set; }
        public decimal CodigoServicoLei { get; set; }


        public TipoServico() { }

        public TipoServico(int CodigoServico,int CodigoTipoServico, string DescricaoTipoServico, int CodigoSituacao, decimal ValorISSQN, decimal CodigoCNAE, decimal CodigoServicoLei)
        {
            CodItem++;
            if (CodigoServico == 0)
                this.CodigoServico = CodItem;
            else
                this.CodigoServico = CodigoServico;

            this.CodigoTipoServico = CodigoTipoServico;
            this.DescricaoTipoServico = DescricaoTipoServico;
            this.CodigoSituacao = CodigoSituacao;
            this.ValorISSQN = ValorISSQN;
            this.CodigoCNAE = CodigoCNAE;
            this.CodigoServicoLei = CodigoServicoLei;
        }
    }
}
