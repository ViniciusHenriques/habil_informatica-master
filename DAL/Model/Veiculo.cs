using System;

namespace DAL.Model
{
    public class Veiculo
    {
        public long CodigoVeiculo { get; set; }
        public string Placa { get; set; }
        public string NomeVeiculo { get; set; }
        public int CategoriaVeiculo { get; set; }
        public string NomeVeiculoCombo { get; set; }

        public Veiculo()
        {
        }

        public Veiculo(long _CodigoVeiculo,
                        string _Placa,
                        string _NomeVeiculo,
                        int _CategoriaVeiculo)
        {
            CodigoVeiculo = _CodigoVeiculo;
            Placa = _Placa;
            NomeVeiculo = _NomeVeiculo;
            CategoriaVeiculo = _CategoriaVeiculo;
            NomeVeiculoCombo= _Placa + " - " + _NomeVeiculo;
        }
    } 
}
