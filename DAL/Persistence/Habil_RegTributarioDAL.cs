using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class Habil_RegTributarioDAL:Conexao
    {
        public List<Habil_RegTributario> ListaHabil_RegTributario()
        {
            List<Habil_RegTributario> lista = new List<Habil_RegTributario>();
            Habil_RegTributario p = null;

            p = new Habil_RegTributario();
            p.CodigoHabil_RegTributario = 1;
            p.DescricaoHabil_RegTributario = "Simples Nacional";
            lista.Add(p);

            p = new Habil_RegTributario();
            p.CodigoHabil_RegTributario = 2;
            p.DescricaoHabil_RegTributario = "Simples Nacional - Acima do Limite (v2.0)";
            lista.Add(p);

            p = new Habil_RegTributario();
            p.CodigoHabil_RegTributario = 3;
            p.DescricaoHabil_RegTributario = "Regime Normal";
            lista.Add(p);

            return lista;
        }
    }
}
