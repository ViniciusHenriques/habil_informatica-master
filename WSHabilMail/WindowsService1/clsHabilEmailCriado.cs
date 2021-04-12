using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSHabilMail
{
    public class clsHabilEmailCriado
    {
        public long CD_INDEX = 0;
        public DateTime DT_LANCAMENTO = DateTime.Now;  
        public long CD_USU_REMETENTE = 0;
        public string TX_CORPO = "";
        public long CD_SITUACAO = 0;
        public DateTime? DT_ENVIO  = null;
        public string TX_ERRO = "";
        public int NR_TENTA_ENVIO = 0;
        public int IN_HTML = 1;
        public string TX_ASSUNTO = "";

        public string  EMAIL_REMETENTE = "";
        public string  SENHA_REMETENTE = "";
    }


}
