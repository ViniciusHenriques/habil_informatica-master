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
    public class Habil_TipoOperacaoFiscalDAL:Conexao
    {
        public List<Habil_TipoOperacaoFiscal> ListaOperacaoFiscal()
        {
            List<Habil_TipoOperacaoFiscal> lista = new List<Habil_TipoOperacaoFiscal>();
            Habil_TipoOperacaoFiscal p = null;

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 1;
            p.DescricaoHabil_TipoOperFiscal = "Compra";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 2;
            p.DescricaoHabil_TipoOperFiscal = "Devolução de Compra";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 3;
            p.DescricaoHabil_TipoOperFiscal = "Venda";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 4;
            p.DescricaoHabil_TipoOperFiscal = "Devolução de Venda";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 5;
            p.DescricaoHabil_TipoOperFiscal = "Transferência";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 6;
            p.DescricaoHabil_TipoOperFiscal = "Devolução de Transferência ";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 7;
            p.DescricaoHabil_TipoOperFiscal = "Simples Remessa";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 8;
            p.DescricaoHabil_TipoOperFiscal = "Ativo Imobilizado";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 9;
            p.DescricaoHabil_TipoOperFiscal = "Bonificação";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 10;
            p.DescricaoHabil_TipoOperFiscal = "Outras Operações Entradas";
            lista.Add(p);

            p = new Habil_TipoOperacaoFiscal();
            p.CodigoHabil_TipoOperFiscal = 11;
            p.DescricaoHabil_TipoOperFiscal = "Outras Operações Saídas";
            lista.Add(p);

            return lista;
        }

        public List<Habil_TipoOperacaoFiscal> ListaOperacaoFiscal(int CodTipoOperFiscal)
        {
            List<Habil_TipoOperacaoFiscal> lista = new List<Habil_TipoOperacaoFiscal>();
            Habil_TipoOperacaoFiscal p = null;

            if (CodTipoOperFiscal == 1)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 1;
                p.DescricaoHabil_TipoOperFiscal = "Compra";
                lista.Add(p);
            }
            if (CodTipoOperFiscal == 2)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 2;
                p.DescricaoHabil_TipoOperFiscal = "Devolução de Compra";
                lista.Add(p);
            }
            if (CodTipoOperFiscal == 3)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 3;
                p.DescricaoHabil_TipoOperFiscal = "Venda";
                lista.Add(p);
            }
            if (CodTipoOperFiscal == 4)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 4;
                p.DescricaoHabil_TipoOperFiscal = "Devolução de Venda";
                lista.Add(p);
            }
            if (CodTipoOperFiscal == 5)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 5;
                p.DescricaoHabil_TipoOperFiscal = "Transferência";
                lista.Add(p);
            }

            if (CodTipoOperFiscal == 6)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 6;
                p.DescricaoHabil_TipoOperFiscal = "Devolução de Transferência";
                lista.Add(p);
            }
            if (CodTipoOperFiscal == 7)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 7;
                p.DescricaoHabil_TipoOperFiscal = "Simples Remessa";
                lista.Add(p);
            }
            if (CodTipoOperFiscal == 8)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 8;
                p.DescricaoHabil_TipoOperFiscal = "Ativo Imobilizado";
                lista.Add(p);
            }
            if (CodTipoOperFiscal == 9)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 9;
                p.DescricaoHabil_TipoOperFiscal = "Bonificação";
                lista.Add(p);

            }
            if (CodTipoOperFiscal == 10)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 10;
                p.DescricaoHabil_TipoOperFiscal = "Outras Operações Entradas";
                lista.Add(p);

            }
            if (CodTipoOperFiscal == 11)
            {
                p = new Habil_TipoOperacaoFiscal();
                p.CodigoHabil_TipoOperFiscal = 11;
                p.DescricaoHabil_TipoOperFiscal = "Outras Operações Saídas";
                lista.Add(p);

            }
            return lista;
        }

        public String ListaDescricaoOperacaoFiscal(int CodTipoOperFiscal)
        {
            if (CodTipoOperFiscal == 1)
                return  "Venda";
            if (CodTipoOperFiscal == 2)
                return "Devolução de Compra";
            if (CodTipoOperFiscal == 3)
                return "Venda";
            if (CodTipoOperFiscal == 4)
                return "Devolução de Transferência";
            if (CodTipoOperFiscal == 5)
                return "Transferência";
            if (CodTipoOperFiscal == 6)
                return "Devolução de Transferência";
            if (CodTipoOperFiscal == 7)
                return "Simples Remessa";
            if (CodTipoOperFiscal == 8)
                return "Ativo Imobilizado";
            if (CodTipoOperFiscal == 9)
                return "Bonificação";
            if (CodTipoOperFiscal == 10)
                return "Outras Operações Entradas";
            if (CodTipoOperFiscal == 11)
                return "Outras Operações Saídas";

            return "Nada";
        }

    }
}
