using System;

namespace DAL.Model
{
    public class Caixa
    {
        public Int32  CodigoCaixa { get; set; }
        public DateTime DataAbertura { get; set; }
        public Int64 CodigoFunAbertura { get; set; }
        public Int64 CodigoFunFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public string NomeMaquina { get; set; }

    }

}
