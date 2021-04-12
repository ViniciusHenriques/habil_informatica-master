using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class ItemTipoServico
    {
        int CodItem = 0;
        int CodProd = 0;
        public int CodigoItemTipoServico { get; set; }
        public int CodigoProdutoDocumento { get; set; }
        public int CodigoServico { get; set; }
        public int CodigoProduto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoItem { get; set; }
        public int CodigoTipoServico { get; set; }
        //Complementares
        public string Cpl_DscProduto { get; set; }

        public ItemTipoServico() { }
      
        public ItemTipoServico(int CodigoItemTipoServico,int CodigoProdutoDocumento,  decimal Quantidade, decimal PrecoItem,  string Cpl_DscProduto, int CodigoProduto, int CodigoServico,int CodigoTipoServico)
        {
            CodItem++;
            if (CodigoItemTipoServico == 0)
                this.CodigoItemTipoServico = CodItem;
            else
                this.CodigoItemTipoServico = CodigoItemTipoServico;

            CodProd++;
            if (CodigoProdutoDocumento == 0)
                this.CodigoProduto = CodProd;
            else
                this.CodigoProdutoDocumento = CodigoProdutoDocumento;

            this.CodigoServico = CodigoServico;
            this.Quantidade= Quantidade;
            this.PrecoItem = PrecoItem;
            this.Cpl_DscProduto = Cpl_DscProduto;
            this.CodigoProduto = CodigoProduto;
            this.CodigoTipoServico = CodigoTipoServico;
        }
    }
}
