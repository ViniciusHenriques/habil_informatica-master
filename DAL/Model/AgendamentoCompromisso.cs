using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class AgendamentoCompromisso
    {
        public int CodigoIndex { get; set; }
        public int CodigoPessoa { get; set; }
        public DateTime DataHoraAgendamento { get; set; }
        public string Anotacao { get; set; }
        public string Telefone { get; set; }
        public string Contato { get; set; }
        public int CodigoSituacao{ get; set; }
        public int CodigoUsuario { get; set; }
        public string Local { get; set; }
        public string CorLembrete { get; set; }
        public bool BtnEditar { get; set; }
        public bool BtnCancelar { get; set; }
        public int CodigoTipoAgendamento { get; set; }
        public List<UsuarioAgendamento> ListaUsuario { get; set; }
        public bool EnviarEmail { get; set; }
        public int CodigoEmpresa { get; set; }

        public string Cpl_DsSituacao { get; set; }
        public string Cpl_NomeCliente { get; set; }
        public string Cpl_NomeUsuario { get; set; }

        public AgendamentoCompromisso()
        {
        }
    }
}
