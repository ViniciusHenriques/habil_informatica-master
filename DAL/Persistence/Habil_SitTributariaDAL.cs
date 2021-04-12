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
    public class Habil_SitTributariaDAL:Conexao
    {
        public List<Habil_SitTributaria> ListaHabil_SitTributaria(int TipoHabil_RegTributario)
        {
            List<Habil_SitTributaria> lista = new List<Habil_SitTributaria>();
            Habil_SitTributaria p = null;

            if (TipoHabil_RegTributario != 3)
            {
                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "101";
                p.DescricaoHabil_SitTributaria = "101 - Tributada pelo Simples Nacional com permissão de crédito";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "102";
                p.DescricaoHabil_SitTributaria = "102 - Tributada pelo Simples Nacional sem permissão de crédito";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "103";
                p.DescricaoHabil_SitTributaria = "103 - Isenção do ICMS no Simples Nacional para faixa de receita bruta";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "201";
                p.DescricaoHabil_SitTributaria = "201 - Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por substituição tributária";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "202";
                p.DescricaoHabil_SitTributaria = "202 - Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por substituição tributária";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "203";
                p.DescricaoHabil_SitTributaria = "203 - Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "300";
                p.DescricaoHabil_SitTributaria = "300 - Imune";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "400";
                p.DescricaoHabil_SitTributaria = "400 - Não tributada pelo Simples Nacional";                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "500";
                p.DescricaoHabil_SitTributaria = "500 - ICMS cobrado anteriormente por substituição tributária (substituído) ou por antecipação";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "900";
                p.DescricaoHabil_SitTributaria = "900 - Outros";
                lista.Add(p);

            }
            else
            {
                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "00";
                p.DescricaoHabil_SitTributaria = "00 - Tributada integralmente";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "10";
                p.DescricaoHabil_SitTributaria = "10 - Tributada e com cobrança do ICMS por substituição tributária";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "20";
                p.DescricaoHabil_SitTributaria = "20 - Com redução da Base de Cálculo";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "30";
                p.DescricaoHabil_SitTributaria = "30 - Isenta / não tributada e com cobrança do ICMS por substituição tributária";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "40";
                p.DescricaoHabil_SitTributaria = "40 - Isenta";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "41";
                p.DescricaoHabil_SitTributaria = "41 - Não tributada";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "50";
                p.DescricaoHabil_SitTributaria = "50 - Com suspensão";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "51";
                p.DescricaoHabil_SitTributaria = "51 - Com diferimento";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "60";
                p.DescricaoHabil_SitTributaria = "60 - ICMS cobrado anteriormente por substituição tributária";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "70";
                p.DescricaoHabil_SitTributaria = "70 - Com redução da Base de Cálculo e cobrança do ICMS por substituição tributária";
                lista.Add(p);

                p = new Habil_SitTributaria();
                p.CodigoHabil_SitTributaria = "90";
                p.DescricaoHabil_SitTributaria = "90 - Outros";
                lista.Add(p);
            }



            return lista;
        }
    }
}
