using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Produto
    {
        private long CodProduto;
        private string p1;
        private string p2;
        private DateTime dateTime1;
        private DateTime dateTime2;
        private double p3;
        private double p4;
        private double p5;
        private int p6;
        private int p7;

        public Int64 CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string CodigoCategoria { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public double ValorCompra { get; set; }
        public double PercentualLucro { get; set; }
        public double ValorVenda { get; set; }
        public int CodigoUnidade { get; set; }
        public int CodigoSituacao { get; set; }
        public int CodigoGpoTribProduto { get; set; }
        public int CodigoTipoProduto { get; set; }
        public string CodigoSisAnterior { get; set; }
        public string CodigoCEST { get; set; }
        public int CodigoFabricante { get; set; }
        public int CodigoMarca { get; set; }
        public int CodigoIndexCEST { get; set; }
        public bool ProdutoInventario { get; set; }
        public bool ControlaLote { get; set; }
        public decimal ValorVolume { get; set; }
        public decimal ValorPeso { get; set; }
        public decimal ValorFatorCubagem { get; set; }
        public int CodigoPIS { get; set; }
        public int CodigoCOFINS { get; set; }
        public string CodigoNCM { get; set; }
        public string CodigoEX { get; set; }
        public string CodigoBarras { get; set; }
        public Int64 CodigoPrdAssociado { get; set; }
        public short QtEmbalagem { get; set; }
        public string DsEmbalagem { get; set; }
        public string DsSigla { get; set; }
        public string LinkProduto { get; set; }
        public byte[] FotoPrincipal { get; set; }
        public byte[] Foto1 { get; set; }
        public byte[] Foto2 { get; set; }
        public byte[] Foto3 { get; set; }
        public byte[] Foto4 { get; set; }
        public byte[] Foto5 { get; set; }
        public string DsMarca { get; set; }

        public Produto()
        {
        }

        public Produto(long _CodigoProduto,
                        string _DescricaoProduto,
                        string _CodigoCategoria, 
                        DateTime _DataCadastro,
                        DateTime _DataAtualizacao,
                        double _ValorCompra,
                        double _PercentualLucro,
                        double _ValorVenda,
                        int _CodigoUnidade,
                        int _CodigoSituacao,
                        int _CodigoGpoTribProduto,
                        int _CodigoTipoProduto,
                        string _CodigoSisAnterior,
                        string _CodigoCEST,
                        int _CodigoMarca,
                        int _CodigoFabricante,
                        bool _ProdutoInventario,
                        bool _ControlaLote,
                        int _CodigoIndexCEST,
                        decimal _ValorVolume,
                        decimal _ValorPeso,
                        decimal _ValorFatorCubagem, 
                        string _CodigoBarras, 
                        int _CodigoPrdAssociado, 
                        short _QtEmbalagem, 
                        string _DsEmbalagem,
                        int _CodigoPIS,
                        int _CodigoCOFINS,
                        string _CodigoNCM,
                        string _CodigoEX)
        {
            CodigoProduto = _CodigoProduto;
            DescricaoProduto = _DescricaoProduto;
            CodigoCategoria = _CodigoCategoria;
            CodigoBarras = _CodigoBarras;
            DataCadastro = _DataCadastro;
            DataAtualizacao = _DataAtualizacao;
            ValorCompra = _ValorCompra;
            PercentualLucro = _PercentualLucro;
            ValorVenda = _ValorVenda;
            CodigoUnidade = _CodigoUnidade;
            CodigoSituacao = _CodigoSituacao;
            CodigoGpoTribProduto = _CodigoGpoTribProduto;
            CodigoTipoProduto = _CodigoTipoProduto;
            CodigoSisAnterior = _CodigoSisAnterior;
            CodigoCEST = _CodigoCEST;
            CodigoMarca = _CodigoMarca;
            CodigoFabricante = _CodigoFabricante;
            CodigoIndexCEST = _CodigoIndexCEST;
            ProdutoInventario = _ProdutoInventario;
            ControlaLote = _ControlaLote;
            ValorVolume = _ValorVolume;
            ValorPeso = _ValorPeso;
            ValorFatorCubagem = _ValorFatorCubagem;
            CodigoBarras = _CodigoBarras;
            CodigoPrdAssociado = _CodigoPrdAssociado;
            QtEmbalagem = _QtEmbalagem;
            DsEmbalagem = _DsEmbalagem;
            CodigoPIS = _CodigoPIS;
            CodigoCOFINS = _CodigoCOFINS;
            CodigoNCM = _CodigoNCM;
            CodigoEX = _CodigoEX;

        }

        public Produto(long CodProduto, string p1, string p2, DateTime dateTime1, DateTime dateTime2, double p3, double p4, double p5, int p6, int p7, string p8)
        {
            // TODO: Complete member initialization
            this.CodProduto = CodProduto;
            this.p1 = p1;
            this.p2 = p2;
            this.dateTime1 = dateTime1;
            this.dateTime2 = dateTime2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
            this.p6 = p6;
            this.p7 = p7;
            //this.p8 = p8;
        }
    }
}
