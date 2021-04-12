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
    public class BandCartaoNFEDAL : Conexao
    {
        public List<BandCartaoNFE> ListaBandCartaoNFE()
        {
            List<BandCartaoNFE> lista = new List<BandCartaoNFE>();
            BandCartaoNFE p = null;

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "01";
            p.DescricaoBandCartaoNFE = "VISA";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "02";
            p.DescricaoBandCartaoNFE = "MASTERCARD";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "03";
            p.DescricaoBandCartaoNFE = "AMERICAN EXPRESS";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "04";
            p.DescricaoBandCartaoNFE = "SOROCRED";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "05";
            p.DescricaoBandCartaoNFE = "DINERS CLUB";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "06";
            p.DescricaoBandCartaoNFE = "ELO";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "07";
            p.DescricaoBandCartaoNFE = "HIPERCARD";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "08";
            p.DescricaoBandCartaoNFE = "AURA";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "09";
            p.DescricaoBandCartaoNFE = "CABAL";
            lista.Add(p);

            p = new BandCartaoNFE();
            p.CodigoBandCartaoNFE = "99";
            p.DescricaoBandCartaoNFE = "OUTROS";
            lista.Add(p);

            return lista;
        }
        public List<BandCartaoNFE> ListaBandCartaoNFE(String CodListaBandCartaoNFE)
        {
            List<BandCartaoNFE> lista = new List<BandCartaoNFE>();
            BandCartaoNFE p = null;

            if (CodListaBandCartaoNFE == "01")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "01";
                p.DescricaoBandCartaoNFE = "VISA";
                lista.Add(p);
            }
            if (CodListaBandCartaoNFE == "02")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "02";
                p.DescricaoBandCartaoNFE = "MASTERCARD";
                lista.Add(p);
            }
            if (CodListaBandCartaoNFE == "03")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "03";
                p.DescricaoBandCartaoNFE = "AMERICAN EXPRESS";
                lista.Add(p);
            }
            if (CodListaBandCartaoNFE == "04")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "04";
                p.DescricaoBandCartaoNFE = "SOROCRED";
                lista.Add(p);
            }
            if (CodListaBandCartaoNFE == "05")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "05";
                p.DescricaoBandCartaoNFE = "DINERS CLUB";
                lista.Add(p);
            }

            if (CodListaBandCartaoNFE == "06")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "06";
                p.DescricaoBandCartaoNFE = "ELO";
                lista.Add(p);
            }
            if (CodListaBandCartaoNFE == "07")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "07";
                p.DescricaoBandCartaoNFE = "HIPERCARD";
                lista.Add(p);
            }
            if (CodListaBandCartaoNFE == "08")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "08";
                p.DescricaoBandCartaoNFE = "AURA";
                lista.Add(p);
            }
            if (CodListaBandCartaoNFE == "09")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "09";
                p.DescricaoBandCartaoNFE = "CABAL";
                lista.Add(p);
            }

            if (CodListaBandCartaoNFE == "99")
            {
                p = new BandCartaoNFE();
                p.CodigoBandCartaoNFE = "99";
                p.DescricaoBandCartaoNFE = "OUTROS";
                lista.Add(p);
            }
            return lista;
        }
        public String  ListaDescricaoBandCartaoNFE(String CodListaBandCartaoNFE)
        {
            if (CodListaBandCartaoNFE == "01")
                return "VISA";
            if (CodListaBandCartaoNFE == "02")
                return "MASTERCARD";
            if (CodListaBandCartaoNFE == "03")
                return "AMERICAN EXPRESS";
            if (CodListaBandCartaoNFE == "04")
                return "SOROCRED";
            if (CodListaBandCartaoNFE == "05")
                return "DINERS CLUB";
            if (CodListaBandCartaoNFE == "06")
                return "ELO";
            if (CodListaBandCartaoNFE == "07")
                return "HIPERCARD";
            if (CodListaBandCartaoNFE == "08")
                return "AURA";
            if (CodListaBandCartaoNFE == "09")
                return "CABAL";
            if (CodListaBandCartaoNFE == "99")
                return "OUTROS";

            return "Nada";
        }
    }
}
