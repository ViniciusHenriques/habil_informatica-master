using System;
using System.Collections.Generic;

namespace DAL.Model
{
    public class Pessoa
    {
        public long CodigoPessoa { get; set; }
        public string NomePessoa { get; set; }
        public string NomeFantasia { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public long CodigoSituacaoPessoa { get; set; }
        public long CodigoSituacaoFase { get; set; }
        public int CodHabil_RegTributario { get; set; }
        public int CodigoGpoTribPessoa { get; set; }
        public int CodigoGrupoPessoa { get; set; }
        public int PessoaEmpresa{get; set;}
        public int PessoaCliente { get; set; }
        public int PessoaFornecedor { get; set; }
        public int PessoaTransportador { get; set; }
        public int PessoaVendedor { get; set; }
        public int PessoaComprador { get; set; }
        public int PessoaUsuario { get; set; }
        public int CodigoCondPagamento { get; set; }
        public int CodigoPlanoContas { get; set; }
        public int CodigoTipoServico { get; set; }
        public string CodigoSisAnterior { get; set; }
        public decimal NumeroProjecao { get; set; }
        public decimal ValorLimiteCredito { get; set; }
        public int CodigoTipoOperacao { get; set; }
        public int CodigoTipoCobranca { get; set; }
        public int CodigoTransportador { get; set; }
        public int CodigoPIS { get; set; }
        public int CodigoCOFINS { get; set; }
        public int CodigoRepresentante { get; set; }
        public List<Vendedor> ListaRepresentantes { get; set; }
        
        //Complementares
        public string Cpl_Inscricao { get; set; }
        public string Cpl_Estado { get; set; }
        public string Cpl_Municipio { get; set; }
        public string Cpl_Bairro { get; set; }
        public string Cpl_Endereco { get; set; }
        public string Cpl_Email { get; set; }
        public string Cpl_Fone { get; set; }
        public string Cpl_Representantes { get; set; }
        public bool MostrarBotaoRepresentantes { get; set; }

        public Pessoa()
        {
        }
        public Pessoa (long _CodigoPessoa ,
                       string CodigoSitAnterior,
                       string _NomePessoa,
                       string _NomeFantasia,
                       DateTime? _DataCadastro, 
                       DateTime? _DataAtualizacao,
                       long _CodigoSituacaoPessoa,
                       long _CodigoSituacaoFase, 
                       int _CodigoGrupoPessoa,
                       int _PessoaEmpresa,
                       int _PessoaCliente,
                       int _PessoaFornecedor,
                       int _PessoaTransportador,
                       int _PessoaVendedor,
                       int _PessoaUsuario, 
                       int _PessoaComprador)
        {
            CodigoPessoa = _CodigoPessoa;
            NomePessoa = _NomePessoa;
            NomeFantasia = _NomeFantasia;
            DataCadastro = _DataCadastro;
            DataAtualizacao = _DataAtualizacao;
            CodigoSituacaoPessoa = _CodigoSituacaoPessoa;
            CodigoSituacaoFase = _CodigoSituacaoFase;
            CodigoGrupoPessoa = _CodigoGrupoPessoa;
            PessoaEmpresa = _PessoaEmpresa;
            PessoaCliente = _PessoaCliente;
            PessoaFornecedor = _PessoaFornecedor;
            PessoaTransportador = _PessoaTransportador;
            PessoaVendedor = _PessoaVendedor;
            PessoaUsuario = _PessoaUsuario;
            PessoaComprador = _PessoaComprador;

            this.CodigoSisAnterior = CodigoSitAnterior;
            this.Cpl_Endereco = Cpl_Endereco;
        }

    }

}
