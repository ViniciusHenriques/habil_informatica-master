using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Model
{
    public class AnexoDocumento
    {

        private static int CodItem;

        public decimal CodigoDocumento { get;set; }
        public int CodigoAnexo { get; set; }
        public DateTime DataHoraLancamento { get; set; }
        public int CodigoMaquina { get; set; }
        public int CodigoUsuario{ get; set; }
        public string NomeArquivo { get; set; }
        public string ExtensaoArquivo { get; set; }
        public byte[] Arquivo { get; set; }
        public string DescricaoArquivo { get; set; }
        public int NaoEditavel { get; set; }

        public string Cpl_Maquina { get; set; }
        public string Cpl_Usuario { get; set; }
        public AnexoDocumento()
        {

        }
        public AnexoDocumento(int CodigoAnexo,
                              DateTime DataHoraLancamento,
                              int CodigoMaquina,
                              int CodigoUsuario,
                              string NomeArquivo,
                              string ExtensaoArquivo,
                              string Cpl_Maquina,
                              string Cpl_Usuario,
                              byte[] Arquivo,
                              string DescricaoArquivo,
                              int NaoEditavel)
        {
            CodItem++;
            if (CodigoAnexo == 0)
                this.CodigoAnexo= CodItem;
            else
                this.CodigoAnexo = CodigoAnexo;

            this.DataHoraLancamento = DataHoraLancamento;
            this.CodigoMaquina = CodigoMaquina;
            this.CodigoUsuario = CodigoUsuario;
            this.NomeArquivo = NomeArquivo;
            this.ExtensaoArquivo = ExtensaoArquivo;
            this.Cpl_Maquina = Cpl_Maquina;
            this.Cpl_Usuario = Cpl_Usuario;
            this.Arquivo = Arquivo;
            this.DescricaoArquivo = DescricaoArquivo;
            this.NaoEditavel = NaoEditavel;
        }

    }
}
