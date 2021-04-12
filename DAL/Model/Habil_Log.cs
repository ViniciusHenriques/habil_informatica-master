using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Habil_Log
    {
        public double CodigoLog { get; set; }
        public DateTime DataHora { get; set; }
        public double CodigoTabelaCampo { get; set; }
        public long CodigoTabela { get; set; }
        public long CodigoCampo { get; set; }
        public long CodigoOperacao { get; set; }
        public long CodigoEstacao { get; set; }
        public long CodigoUsuario { get; set; }
        public String DescricaoLog { get; set; }
        public String CodigoChave { get; set; }

        //Complementares
        public double CodigoIdentificador { get; set; }

        public string EstacaoNome { get; set; }
        public string UsuarioNome { get; set; }
        public string Cpl_NomeCampo { get; set; }
        public string Cpl_DescricaoOperacao { get; set; }



    }
}
