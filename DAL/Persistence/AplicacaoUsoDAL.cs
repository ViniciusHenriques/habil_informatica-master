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
    public class Habil_AplicacaoUsoDAL:Conexao
    {
        public List<Habil_AplicacaoUso> ListaHabil_AplicacaoUso()
        {
            List<Habil_AplicacaoUso> lista = new List<Habil_AplicacaoUso>();
            Habil_AplicacaoUso p = null;

            p = new Habil_AplicacaoUso();
            p.CodigoHabil_AplicacaoUso = 1;
            p.DescricaoHabil_AplicacaoUso = "Consumo";
            lista.Add(p);

            p = new Habil_AplicacaoUso();
            p.CodigoHabil_AplicacaoUso = 2;
            p.DescricaoHabil_AplicacaoUso = "Revenda";
            lista.Add(p);

            p = new Habil_AplicacaoUso();
            p.CodigoHabil_AplicacaoUso = 3;
            p.DescricaoHabil_AplicacaoUso = "Industrialização";
            lista.Add(p);

            return lista;
        }

        public List<Habil_AplicacaoUso> ListaHabil_AplicacaoUso(int CodAplicacao)
        {
            List<Habil_AplicacaoUso> lista = new List<Habil_AplicacaoUso>();
            Habil_AplicacaoUso p = null;

            if (CodAplicacao ==1 )
            {
                p = new Habil_AplicacaoUso();
                p.CodigoHabil_AplicacaoUso = 1;
                p.DescricaoHabil_AplicacaoUso = "Consumo";
                lista.Add(p);
            }

            if (CodAplicacao == 2)
            {
                p = new Habil_AplicacaoUso();
                p.CodigoHabil_AplicacaoUso = 2;
                p.DescricaoHabil_AplicacaoUso = "Revenda";
                lista.Add(p);
            }
            if (CodAplicacao == 3)
            {
                p = new Habil_AplicacaoUso();
                p.CodigoHabil_AplicacaoUso = 3;
                p.DescricaoHabil_AplicacaoUso = "Industrialização";
                lista.Add(p);
            }

            return lista;
        }

        public String ListaDescricaoHabil_AplicacaoUso(int CodAplicacao)
        {
            if (CodAplicacao == 1)
                return "Consumo";
            if (CodAplicacao == 2)
                return "Revenda";
            if (CodAplicacao == 3)
                return "Industrialização";

            return "Nada";
        }

    }
}
