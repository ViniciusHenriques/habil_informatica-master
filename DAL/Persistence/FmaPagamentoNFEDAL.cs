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
    public class FmaPagamentoNFEDAL : Conexao
    {
        public List<FmaPagamentoNFE> ListaFmaPagamentoNFE()
        {
            List<FmaPagamentoNFE> lista = new List<FmaPagamentoNFE>();
            FmaPagamentoNFE p = null;

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "01";
            p.DescricaoFmaPagamentoNFE = "DINHEIRO";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "02";
            p.DescricaoFmaPagamentoNFE = "CHEQUE";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "03";
            p.DescricaoFmaPagamentoNFE = "CARTÃO DE CRÉDITO";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "04";
            p.DescricaoFmaPagamentoNFE = "CARTÃO DE DÉBITO";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "05";
            p.DescricaoFmaPagamentoNFE = "CRÉDITO LOJA";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "10";
            p.DescricaoFmaPagamentoNFE = "VALE ALIMENTAÇÃO";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "11";
            p.DescricaoFmaPagamentoNFE = "VALE REFEIÇÃO";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "12";
            p.DescricaoFmaPagamentoNFE = "VALE PRESENTE";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "13";
            p.DescricaoFmaPagamentoNFE = "VALE COMBUSTÍVEL";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "14";
            p.DescricaoFmaPagamentoNFE = "DUPLICADA MERCANTIL";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "15";
            p.DescricaoFmaPagamentoNFE = "BOLETO BANCÁRIO";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "90";
            p.DescricaoFmaPagamentoNFE = "SEM PAGAMENTO";
            lista.Add(p);

            p = new FmaPagamentoNFE();
            p.CodigoFmaPagamentoNFE = "99";
            p.DescricaoFmaPagamentoNFE = "OUTROS";
            lista.Add(p);

            return lista;
        }
        public List<FmaPagamentoNFE> ListaFmaPagamentoNFE(String CodListaFmaPagamentoNFE)
        {
            List<FmaPagamentoNFE> lista = new List<FmaPagamentoNFE>();
            FmaPagamentoNFE p = null;

            if (CodListaFmaPagamentoNFE == "01")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "01";
                p.DescricaoFmaPagamentoNFE = "DINHEIRO";
                lista.Add(p);
            }
            if (CodListaFmaPagamentoNFE == "02")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "02";
                p.DescricaoFmaPagamentoNFE = "CHEQUE";
                lista.Add(p);
            }
            if (CodListaFmaPagamentoNFE == "03")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "03";
                p.DescricaoFmaPagamentoNFE = "CARTÃO DE CRÉDITO";
                lista.Add(p);
            }
            if (CodListaFmaPagamentoNFE == "04")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "04";
                p.DescricaoFmaPagamentoNFE = "CARTÃO DE DÉBITO";
                lista.Add(p);
            }
            if (CodListaFmaPagamentoNFE == "05")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "05";
                p.DescricaoFmaPagamentoNFE = "CRÉDITO LOJA";
                lista.Add(p);
            }

            if (CodListaFmaPagamentoNFE == "10")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "10";
                p.DescricaoFmaPagamentoNFE = "VALE ALIMENTAÇÃO";
                lista.Add(p);
            }
            if (CodListaFmaPagamentoNFE == "11")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "11";
                p.DescricaoFmaPagamentoNFE = "VALE REFEIÇÃO";
                lista.Add(p);
            }
            if (CodListaFmaPagamentoNFE == "12")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "12";
                p.DescricaoFmaPagamentoNFE = "VALE PRESENTE";
                lista.Add(p);
            }
            if (CodListaFmaPagamentoNFE == "13")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "13";
                p.DescricaoFmaPagamentoNFE = "VALE COMBUSTÍVEL";
                lista.Add(p);
            }

            if (CodListaFmaPagamentoNFE == "14")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "14";
                p.DescricaoFmaPagamentoNFE = "DUPLICADA MERCANTIL";
                lista.Add(p);
            }

            if (CodListaFmaPagamentoNFE == "15")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "15";
                p.DescricaoFmaPagamentoNFE = "BOLETO BANCÁRIO";
                lista.Add(p);
            }

            if (CodListaFmaPagamentoNFE == "90")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "90";
                p.DescricaoFmaPagamentoNFE = "SEM PAGAMENTO";
                lista.Add(p);
            }

            if (CodListaFmaPagamentoNFE == "99")
            {
                p = new FmaPagamentoNFE();
                p.CodigoFmaPagamentoNFE = "99";
                p.DescricaoFmaPagamentoNFE = "OUTROS";
                lista.Add(p);
            }
            return lista;
        }
        public String ListaDescricaoFmaPagamentoNFE(String CodListaFmaPagamentoNFE)
        {
            if (CodListaFmaPagamentoNFE == "01")
                return "DINHEIRO";
            if (CodListaFmaPagamentoNFE == "02")
                return "CHEQUE";
            if (CodListaFmaPagamentoNFE == "03")
                return "CARTÃO DE CRÉDITO";
            if (CodListaFmaPagamentoNFE == "04")
                return "CARTÃO DE DÉBITO";
            if (CodListaFmaPagamentoNFE == "05")
                return "CRÉDITO LOJA";
            if (CodListaFmaPagamentoNFE == "10")
                return "VALE ALIMENTAÇÃO";
            if (CodListaFmaPagamentoNFE == "11")
                return "VALE REFEIÇÃO";
            if (CodListaFmaPagamentoNFE == "12")
                return "VALE PRESENTE";
            if (CodListaFmaPagamentoNFE == "13")
                return "VALE COMBUSTÍVEL";
            if (CodListaFmaPagamentoNFE == "14")
                return "DUPLICADA MERCANTIL";
            if (CodListaFmaPagamentoNFE == "15")
                return "BOLETO BANCÁRIO";
            if (CodListaFmaPagamentoNFE == "90")
                return "SEM PAGAMENTO";
            if (CodListaFmaPagamentoNFE == "99")
                return "OUTROS";

            return "Nada";
        }

    }
}
