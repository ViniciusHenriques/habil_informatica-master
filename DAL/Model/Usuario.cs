using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Usuario
    {
        public int CodigoUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string GUID { get; set; }
        public int CodigoPerfil { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoPessoa { get; set; }
        public Pessoa Pessoa { get; set; }
        public int CodigoCargo { get; set; }
        public string ResetarSenha { get; set; }


    }
}
