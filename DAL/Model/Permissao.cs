using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Permissao
    {
        public int CodigoItem { get; set; }
        public string DescricaoDoMenu { get; set; }
        public string TipoFormulario { get; set; }
        public Boolean Liberado { get; set; }
        public Boolean AcessoCompleto { get; set; }
        public Boolean AcessoConsulta { get; set; }
        public Boolean AcessoRelatorio { get; set; }
        public Boolean AcessoImprimir { get; set; }
        public Boolean AcessoIncluir { get; set; }
        public Boolean AcessoAlterar { get; set; }
        public Boolean AcessoExcluir { get; set; }
        public Boolean AcessoEspecial1 { get; set; }
        public Boolean AcessoEspecial2 { get; set; }
        public Boolean AcessoEspecial3 { get; set; }
        public Boolean AcessoDownload { get; set; }
        public Boolean AcessoUpload { get; set; }
        public Boolean AcessoExclusaoAnexo { get; set; }
        public string TextoAjuda { get; set; }
    }




}
