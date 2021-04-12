using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Inventario
    {
        public decimal CodigoIndice { get; set; }
        public int CodigoSituacao { get; set; }
        public long CodigoMaquina { get; set; }
        public int CodigoUsuario { get; set; }
        public DateTime DtGeracao { get; set; }
        public short NrContagem { get; set; }
        public string DescInventario { get; set; }
        public string DescSituacao { get; set; }
        public decimal PctCont1 { get; set; }
        public decimal PctCont2 { get; set; }
        public decimal PctCont3 { get; set; }
        public decimal PctCont4 { get; set; }
        public decimal PctCont5 { get; set; }
        //Complementares
        public bool BtnCancelar { get; set; }
        public bool BtnEncerrar { get; set; }
    }
}
