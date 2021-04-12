using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class MenuSistema
    {
        public int CodigoMenu { get; set; }
        public int CodigoModulo { get; set; }
        public string NomeMenu { get; set; }
        public string DescricaoMenu { get; set; }
        public int CodigoOrdem { get; set; }
        public int CodigoPaiMenu { get; set; }
        public string UrlPrograma { get; set; }
        public string TipoFormulario { get; set; }
        public string TextoAjuda { get; set; }
        public string UrlIcone { get; set; }


    }
}
