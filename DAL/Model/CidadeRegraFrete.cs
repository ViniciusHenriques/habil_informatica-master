using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class CidadeRegraFrete
    {
        public int CodigoIBGE { get; set; }
        public int CodigoRegraFrete { get; set; }

        //complementares
        public string Cpl_DescricaoCidade { get; set; }
        public int Cpl_CodigoEstado { get; set; }
        public string Cpl_Sigla { get; set; }

        public CidadeRegraFrete()
        {
        }

        public CidadeRegraFrete(int codigoIBGE, int codigoRegraFrete, string cpl_DescricaoCidade, int cpl_CodigoEstado, string cpl_Sigla)
        {
            CodigoIBGE = codigoIBGE;
            CodigoRegraFrete = codigoRegraFrete;
            Cpl_DescricaoCidade = cpl_DescricaoCidade;
            Cpl_CodigoEstado = cpl_CodigoEstado;
            Cpl_Sigla = cpl_Sigla;
        }
    }
}
