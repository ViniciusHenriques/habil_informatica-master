using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;
using System.Linq;

namespace DAL.Persistence
{
	public class PessoaDAL : Conexao
	{
		String strSQL = "";
		public void Inserir(Pessoa p, List<Pessoa_Inscricao> listCadPessoaInscricao, List<Pessoa_Endereco> listCadPessoaEndereco, List<Pessoa_Contato> listCadPessoaContato, ref Int64 CodigoPessoa)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("insert into Pessoa (NM_PESSOA," +
														 "NM_FANTASIA, " +
														 "DT_CADASTRO, " +
														 "DT_ATUALIZACAO, " +
														 "CD_SITUACAO_PESSOA, " +
														 "CD_SITUACAO_FASE, " +
														 "CD_REG_TRIBUTARIO, " +
														 "CD_GPO_TRIB_PESSOA, " +
														 "CD_GPO_PESSOA, " +
														 "IN_EMPRESA, " +
														 "IN_CLIENTE, " +
														 "IN_FORNECEDOR, " +
														 "IN_TRANSPORTADOR, " +
                                                         "IN_VENDEDOR,IN_USUARIO, IN_COMPRADOR, " +
														 "CD_CND_PAGAMENTO," +
														 "CD_PLANO_CONTA," +
														 "CD_TIPO_SERVICO," +
														 "CD_SIS_ANTERIOR," +
														 "NR_PROJECAO," +
														 "VL_LIMITE_CREDITO," +
														 "CD_TRANSPORTADOR," +
														 "CD_TIPO_COBRANCA," +
														 "CD_TIPO_OPERACAO," +
														 "CD_PIS," +
														 "CD_COFINS) " +
                    "values (@v1,@v4, GETDATE(), GETDATE(), @v2, @v3, @v5, @v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@vCpa,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24); SELECT SCOPE_IDENTITY()", Con);
				Cmd.Parameters.AddWithValue("@v1", p.NomePessoa);
				Cmd.Parameters.AddWithValue("@v4", p.NomeFantasia);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacaoPessoa);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacaoFase);
				Cmd.Parameters.AddWithValue("@v5", p.CodHabil_RegTributario);
				Cmd.Parameters.AddWithValue("@v6", p.CodigoGpoTribPessoa);
				Cmd.Parameters.AddWithValue("@v7", p.CodigoGrupoPessoa);
				Cmd.Parameters.AddWithValue("@v8", p.PessoaEmpresa);
				Cmd.Parameters.AddWithValue("@v9", p.PessoaCliente);
				Cmd.Parameters.AddWithValue("@v10", p.PessoaFornecedor);
				Cmd.Parameters.AddWithValue("@v11", p.PessoaTransportador);
				Cmd.Parameters.AddWithValue("@v12", p.PessoaVendedor);
				Cmd.Parameters.AddWithValue("@v13", p.PessoaUsuario);
                Cmd.Parameters.AddWithValue("@vCpa", p.PessoaComprador);
                Cmd.Parameters.AddWithValue("@v14", p.CodigoCondPagamento);
				Cmd.Parameters.AddWithValue("@v15", p.CodigoPlanoContas);
				Cmd.Parameters.AddWithValue("@v16", p.CodigoTipoServico);
				if (string.IsNullOrEmpty(p.CodigoSisAnterior))
					Cmd.Parameters.AddWithValue("@v17", DBNull.Value);
				else
					Cmd.Parameters.AddWithValue("@v17", p.CodigoSisAnterior);
				Cmd.Parameters.AddWithValue("@v18", p.NumeroProjecao);
				Cmd.Parameters.AddWithValue("@v19", p.ValorLimiteCredito);
				Cmd.Parameters.AddWithValue("@v20", p.CodigoTransportador);
				Cmd.Parameters.AddWithValue("@v21", p.CodigoTipoCobranca);
				Cmd.Parameters.AddWithValue("@v22", p.CodigoTipoOperacao);
				Cmd.Parameters.AddWithValue("@v23", p.CodigoPIS);
				Cmd.Parameters.AddWithValue("@v24", p.CodigoCOFINS);
				p.CodigoPessoa = Convert.ToInt64(Cmd.ExecuteScalar());

				CodigoPessoa = p.CodigoPessoa;

			}
			catch (SqlException ex)
			{
				if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
				{
					switch (ex.Errors[0].Number)
					{
						case 2601: // Primary key violation
							throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
						case 2627: // Primary key violation
							throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
						default:
							throw new Exception("Erro ao gravar Pessoa: " + ex.Message.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao gravar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

				PessoaInscricaoDAL pe = new PessoaInscricaoDAL();
				pe.Inserir(p.CodigoPessoa, listCadPessoaInscricao);

				PessoaEnderecoDAL pe2 = new PessoaEnderecoDAL();
				pe2.Inserir(p.CodigoPessoa, listCadPessoaEndereco);

				PessoaContatoDAL pe3 = new PessoaContatoDAL();
				pe3.Inserir(p.CodigoPessoa, listCadPessoaContato);

                InserirRepresentantes(p.CodigoPessoa, p.ListaRepresentantes);
            }
		}
		public void Atualizar(Pessoa p, List<Pessoa_Inscricao> listCadPessoaInscricao, List<Pessoa_Endereco> listCadPessoaEndereco, List<Pessoa_Contato> listCadPessoaContato)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("update Pessoa set NM_PESSOA = @v1, NM_FANTASIA = @v5, DT_ATUALIZACAO = GETDATE(), CD_SITUACAO_PESSOA = @v2, CD_SITUACAO_FASE = @v3," +
					" CD_GPO_PESSOA=@v8, IN_EMPRESA=@v9, IN_CLIENTE=@v10, IN_FORNECEDOR=@v11, IN_TRANSPORTADOR = @v12, IN_VENDEDOR=@v13, IN_USUARIO = @v14 , CD_SIS_ANTERIOR = @v15 " +
                    " , IN_COMPRADOR = @vCpa Where CD_PESSOA = @v4", Con);
				Cmd.Parameters.AddWithValue("@v1", p.NomePessoa);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacaoPessoa);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacaoFase);
				Cmd.Parameters.AddWithValue("@v4", p.CodigoPessoa);
				Cmd.Parameters.AddWithValue("@v5", p.NomeFantasia);
				Cmd.Parameters.AddWithValue("@v8", p.CodigoGrupoPessoa);
				Cmd.Parameters.AddWithValue("@v9", p.PessoaEmpresa);
				Cmd.Parameters.AddWithValue("@v10", p.PessoaCliente);
				Cmd.Parameters.AddWithValue("@v11", p.PessoaFornecedor);
				Cmd.Parameters.AddWithValue("@v12", p.PessoaTransportador);
				Cmd.Parameters.AddWithValue("@v13", p.PessoaVendedor);
				Cmd.Parameters.AddWithValue("@v14", p.PessoaUsuario);

				if (string.IsNullOrEmpty(p.CodigoSisAnterior))
					Cmd.Parameters.AddWithValue("@v15", DBNull.Value);
				else
					Cmd.Parameters.AddWithValue("@v15", p.CodigoSisAnterior);

                Cmd.Parameters.AddWithValue("@vCpa", p.PessoaComprador);


                Cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

				PessoaInscricaoDAL pe = new PessoaInscricaoDAL();
				pe.Inserir(p.CodigoPessoa, listCadPessoaInscricao);

				PessoaEnderecoDAL pe2 = new PessoaEnderecoDAL();
				pe2.Inserir(p.CodigoPessoa, listCadPessoaEndereco);

				PessoaContatoDAL pe3 = new PessoaContatoDAL();
				pe3.Inserir(p.CodigoPessoa, listCadPessoaContato);

                InserirRepresentantes(p.CodigoPessoa, p.ListaRepresentantes);
            }
		}
		public void AtualizarPessoaFinanceiro(Pessoa p)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("update Pessoa set CD_CND_PAGAMENTO = @v2, CD_PLANO_CONTA = @v3, CD_TIPO_SERVICO = @v4, VL_LIMITE_CREDITO = @v5, CD_TIPO_COBRANCA = @v6 Where CD_PESSOA = @v1", Con);
				Cmd.Parameters.AddWithValue("@v1", p.CodigoPessoa);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoCondPagamento);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoPlanoContas);
				Cmd.Parameters.AddWithValue("@v4", p.CodigoTipoServico);
				Cmd.Parameters.AddWithValue("@v5", p.ValorLimiteCredito);
				Cmd.Parameters.AddWithValue("@v6", p.CodigoTipoCobranca);

				Cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar Pessoa financeiro: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public void AtualizarPessoaFiscal(Pessoa p)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("update Pessoa set CD_REG_TRIBUTARIO = @v2," +
														" CD_GPO_TRIB_PESSOA = @v3," +
														" CD_PIS = @v4," +
														" CD_COFINS = @v5 Where CD_PESSOA = @v1", Con);
				Cmd.Parameters.AddWithValue("@v1", p.CodigoPessoa);
				Cmd.Parameters.AddWithValue("@v2", p.CodHabil_RegTributario);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoGpoTribPessoa);
				Cmd.Parameters.AddWithValue("@v4", p.CodigoPIS);
				Cmd.Parameters.AddWithValue("@v5", p.CodigoCOFINS);
				Cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar Pessoa fiscal: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public void AtualizarPessoaComercial(Pessoa p)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("update Pessoa set NR_PROJECAO = @v2, CD_TRANSPORTADOR = @v3 Where CD_PESSOA = @v1", Con);
				Cmd.Parameters.AddWithValue("@v1", p.CodigoPessoa);
				Cmd.Parameters.AddWithValue("@v2", p.NumeroProjecao);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoTransportador);
				Cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar Pessoa COMERCIAL: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public void AtualizarPessoaVendedor(Int64 cd_pessoa, int in_vendedor)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("update Pessoa set IN_VENDEDOR = @v2 Where CD_PESSOA = @v1", Con);
				Cmd.Parameters.AddWithValue("@v1", cd_pessoa);
				Cmd.Parameters.AddWithValue("@v2", in_vendedor);
				Cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar Indicador de Vendedor: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public void Excluir(Int64 Codigo)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("delete from Pessoa_contato where CD_PESSOA=@v1;" +
									 "delete from Pessoa_endereco where CD_PESSOA=@v1;" +
                                     "delete from Pessoa_inscricao where CD_PESSOA=@v1;" +
									 "delete from Pessoa Where CD_PESSOA = @v1 ", Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Cmd.ExecuteNonQuery();
			}
			catch (SqlException ex)
			{
				if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
				{
					switch (ex.Errors[0].Number)
					{
						case 547: // Foreign Key violation
							throw new InvalidOperationException("Exclusão não Permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
						default:
							throw new Exception("Erro ao excluir Pessoa: " + ex.Message.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao excluir Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Pessoa PesquisarCliente(Int64 Codigo)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("Select * from Pessoa Where CD_PESSOA = @v1 AND IN_CLIENTE = 1", Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Dr = Cmd.ExecuteReader();
				Pessoa p = null;
				if (Dr.Read())
				{
					p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
					p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
					p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
					p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
					p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
					p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
					p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
					p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
					p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
					p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
					p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
					p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
					p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
					p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
					p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
					if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
						p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
					p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
					p.ValorLimiteCredito = Convert.ToDecimal(Dr["VL_LIMITE_CREDITO"]);
					p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
					p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
					p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
					p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);

                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador= Convert.ToInt32(Dr["IN_COMPRADOR"]);
                }
                return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Pessoa> ListarCliente(string strNome)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("Select * from Pessoa Where NM_PESSOA like '%" + strNome + "%' AND IN_CLIENTE = 1", Con);
				Dr = Cmd.ExecuteReader();

				List<Pessoa> Lista = new List<Pessoa>();

				Pessoa p = null;

				while (Dr.Read())
				{
					p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					Lista.Add(p);
				}
				return Lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Pessoa> ListarFornecedor(string strNome, Int32 CodFornecedor)
		{
			try
			{
				AbrirConexao();

                strSQL = "Select P.*, SUBSTRING(Pend.DS_ESTADO, 0, 3) AS DS_ESTADO, Pend.DS_MUNICIPIO, CTT.NM_CONTATO, CTT.NR_FONE1, CTT.TX_MAIL1  from Pessoa as P " +
                    "INNER JOIN PESSOA_ENDERECO AS PEND ON PEND.CD_PESSOA = P.CD_PESSOA AND PEND.CD_ENDERECO = 1 " +
                    "INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = P.CD_PESSOA AND INS.CD_INSCRICAO = 1 " +
                    "INNER JOIN PESSOA_CONTATO AS CTT ON CTT.CD_PESSOA = P.CD_PESSOA AND CTT.CD_CONTATO = 1  where IN_FORNECEDOR = 1  ";

                if (strNome != "")
                    strSQL = strSQL + "and NM_PESSOA like '%" + strNome + "%' ";

                if (CodFornecedor != 0)
                    strSQL = strSQL + "and CD_PESSOA = " + CodFornecedor.ToString() ;

                Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Pessoa> Lista = new List<Pessoa>();

				Pessoa p = null;

				while (Dr.Read())
				{
					p = new Pessoa();
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
                    p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
                    p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
                    p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
                    p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
                    p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
                    p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
                    p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
                    p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
                    p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
                    p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
                    p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
                    p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
                        p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
                    p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
                    p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador = Convert.ToInt32(Dr["IN_COMPRADOR"]);

                    p.Cpl_Municipio = Dr["DS_MUNICIPIO"].ToString();
                    p.Cpl_Estado = Dr["DS_ESTADO"].ToString();
                    p.Cpl_Fone = Dr["NR_FONE1"].ToString();
                    p.Cpl_Email = Dr["TX_MAIL1"].ToString();

                    Lista.Add(p);
				}
				return Lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Pessoa PesquisarTransportador(Int64 Codigo)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("Select * from Pessoa Where CD_PESSOA = @v1 AND IN_TRANSPORTADOR = 1", Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Dr = Cmd.ExecuteReader();
				Pessoa p = null;
				if (Dr.Read())
				{
					p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
					p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
					p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
					p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
					p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
					p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
					p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
					p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
					p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
					p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
					p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
					p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
					p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
					p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
					p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
					if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
						p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
					p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
					p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
					p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
					p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
					p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);

                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador = Convert.ToInt32(Dr["IN_COMPRADOR"]);
                }
                return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Pessoa PesquisarPessoaUsuario(Int64 Codigo)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("Select * from Pessoa Where CD_PESSOA = @v1 AND IN_USUARIO = 1", Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Dr = Cmd.ExecuteReader();
				Pessoa p = null;
				if (Dr.Read())
				{
					p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
					p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
					p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
					p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
					p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
					p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
					p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
					p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
					p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
					p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
					p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
					p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
					p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
					p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
					p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
					if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
						p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
					p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
					p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
					p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
					p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
					p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);

                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador = Convert.ToInt32(Dr["IN_COMPRADOR"]);
                }
                return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Pessoa PesquisarPessoa(Int64 Codigo)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("Select * from Pessoa Where CD_PESSOA = @v1", Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Dr = Cmd.ExecuteReader();
				Pessoa p = null;
				if (Dr.Read())
				{
					p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
					p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
					p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
					p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
					p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
					p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
					p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
					p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
					p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
					p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
					p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
					p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
					p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
					p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
					p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
					if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
						p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
					p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
					p.ValorLimiteCredito = Convert.ToDecimal(Dr["VL_LIMITE_CREDITO"]);
					if (Dr["CD_TRANSPORTADOR"] != DBNull.Value)
						p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
					p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
					p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
					p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador = Convert.ToInt32(Dr["IN_COMPRADOR"]);
                }
                return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Pessoa PesquisarViewPessoa(Int64 Codigo)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("Select * from VW_PESSOA Where COD_PESSOA = @v1", Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Dr = Cmd.ExecuteReader();
				Pessoa p = null;
				if (Dr.Read())
				{
					p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["COD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NOME_PESSOA"]);
					p.Cpl_Inscricao = Dr["NR_INSCRICAO"].ToString();
					p.Cpl_Estado = Dr["DS_ESTADO"].ToString();
				}
				return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Pessoa PesquisarFornecedor(Int64 Codigo)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand("Select P.*, SUBSTRING(Pend.DS_ESTADO, 0, 3) AS DS_ESTADO, Pend.DS_MUNICIPIO, CTT.NM_CONTATO, CTT.NR_FONE1, CTT.TX_MAIL1  from Pessoa as P " +
                    "INNER JOIN PESSOA_ENDERECO AS PEND ON PEND.CD_PESSOA = P.CD_PESSOA AND PEND.CD_ENDERECO = 1 " +
                    "INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = P.CD_PESSOA AND INS.CD_INSCRICAO = 1 " +
                    "INNER JOIN PESSOA_CONTATO AS CTT ON CTT.CD_PESSOA = P.CD_PESSOA AND CTT.CD_CONTATO = 1 " +
                    " Where P.CD_PESSOA = @v1 AND P.IN_FORNECEDOR = 1", Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);
				Dr = Cmd.ExecuteReader();
				Pessoa p = null;
				if (Dr.Read())
				{
					p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
					p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
					p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
					p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
					p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
					p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
					p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
					p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
					p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
					p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
					p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
					p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
					p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
					p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
					p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
					if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
						p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
					p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
					p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
					p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
					p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
					p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador = Convert.ToInt32(Dr["IN_COMPRADOR"]);

                    p.Cpl_Municipio = Dr["DS_MUNICIPIO"].ToString();
                    p.Cpl_Estado = Dr["DS_ESTADO"].ToString();
                    p.Cpl_Fone = Dr["NR_FONE1"].ToString();
                    p.Cpl_Email = Dr["TX_MAIL1"].ToString();
                }
                return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Pessoa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Pessoa> ListarPessoas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem, short shtQuantRegistros = 0)
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select ";

				if (shtQuantRegistros != 0)
					strSQL += " Top " + shtQuantRegistros.ToString();

				strSQL += " isnull(dbo.FunPessoaRepresentante(CD_PESSOA),'') as REPRESENTANTES , * from Pessoa ";

				if (strValor != "")
					strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);
				if (strOrdem != "")
					strSQL = strSQL + "Order By " + strOrdem;
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Pessoa> lista = new List<Pessoa>();
				while (Dr.Read())
				{
					Pessoa p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
					p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
					p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
					p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
					p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
					p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
					p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
					p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
					p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
					p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
					p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
					p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
					p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
					p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
					p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
					if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
						p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
					if (Dr["NR_PROJECAO"] != DBNull.Value)
						p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
					p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
					p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
					p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
					p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador = Convert.ToInt32(Dr["IN_COMPRADOR"]);

                    if (Dr["REPRESENTANTES"].ToString() == "")
                        p.MostrarBotaoRepresentantes = false;
                    else
                        p.MostrarBotaoRepresentantes = true;

                    p.Cpl_Representantes = Dr["REPRESENTANTES"].ToString();

                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todas Pessoas: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Pessoa> ListarPessoasCompleto(List<DBTabelaCampos> ListaFiltros, short shtTipoPessoa, short shtQuantRegistros = 0)
		{
			try
			{
				AbrirConexao();
				string strValor = "";

				string strSQL = "Select ";

				if (shtQuantRegistros != 0)
					strSQL += " Top " + shtQuantRegistros.ToString();

				strSQL += " isnull(dbo.FunPessoaRepresentante(CD_PESSOA),'') as REPRESENTANTES , * from Pessoa as P WHERE [CD_PESSOA] IN ( SELECT [VW_PESSOA].COD_PESSOA FROM [VW_PESSOA]  ";


				strValor = MontaFiltroIntervalo(ListaFiltros);

				strSQL = strSQL + strValor;

				strSQL = strSQL + ")";
				if (shtTipoPessoa == 1)
					strSQL = strSQL + " and P.IN_FORNECEDOR = 1";
				else if (shtTipoPessoa == 2)
					strSQL = strSQL + " and P.IN_CLIENTE = 1";
				else if (shtTipoPessoa == 3)
					strSQL = strSQL + " and P.IN_TRANSPORTADOR = 1";
				else if (shtTipoPessoa == 4)
					strSQL = strSQL + " and P.IN_USUARIO = 1";
				else if (shtTipoPessoa == 5)
					strSQL = strSQL + " and P.IN_VENDEDOR = 1";
                else if (shtTipoPessoa == 6)
                    strSQL = strSQL + " and P.IN_COMPRADOR = 1";


                List<Pessoa> lista = new List<Pessoa>();
				List<Pessoa_Inscricao> listaI = new List<Pessoa_Inscricao>();
				List<Pessoa_Contato> listaC = new List<Pessoa_Contato>();
				List<Pessoa_Endereco> listaE = new List<Pessoa_Endereco>();


				PessoaInscricaoDAL insDAL = new PessoaInscricaoDAL();
				listaI = insDAL.ListarPessoaInscricoes(ListaFiltros, shtTipoPessoa);

				PessoaContatoDAL cttDAL = new PessoaContatoDAL();
				listaC = cttDAL.ListarPessoaContatos(ListaFiltros, shtTipoPessoa);

				PessoaEnderecoDAL endDAL = new PessoaEnderecoDAL();
				listaE = endDAL.ListarPessoaEnderecos(ListaFiltros, shtTipoPessoa);

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				while (Dr.Read())
				{

					Pessoa p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
					p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
					p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
					p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
					p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
					p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
					p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
					p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
					p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
					p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
					p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
					p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
					p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
					p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
					p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
					p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
					p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
					p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);

                    if (Dr["REPRESENTANTES"].ToString() == "")
                        p.MostrarBotaoRepresentantes = false;
                    else
                        p.MostrarBotaoRepresentantes = true;

                    p.Cpl_Representantes = Dr["REPRESENTANTES"].ToString();

                    if (Dr["IN_COMPRADOR"] != DBNull.Value)
                        p.PessoaComprador = Convert.ToInt32(Dr["IN_COMPRADOR"]);

                    if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
						p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
					p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);
					if (Dr["CD_TRANSPORTADOR"] != DBNull.Value)
						p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);

					var buscaInscricao = listaI.FirstOrDefault(x => x._CodigoPessoa == p.CodigoPessoa);
					if (buscaInscricao != null)
						p.Cpl_Inscricao = buscaInscricao._NumeroInscricao;

					var buscaEndereco = listaE.FirstOrDefault(x => x._CodigoPessoa == p.CodigoPessoa);
					if (buscaEndereco != null)
					{
						p.Cpl_Estado = buscaEndereco._DescricaoEstado;
						p.Cpl_Municipio = buscaEndereco._DescricaoMunicipio;
						p.Cpl_Bairro = buscaEndereco._DescricaoBairro;
						p.Cpl_Endereco = buscaEndereco._Logradouro + ", " + buscaEndereco._NumeroLogradouro;
					}

					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas Pessoas: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public void PessoaEmpresaUsuario(long CodigoPessoa, int IndicadorPessoa, bool blnGerarComoEmpresa, bool blnGerarComoUsuario)
		{

			//   1 - Empresa
			//   2 - Usuario
			try
			{
				AbrirConexao();
				if (IndicadorPessoa == 1)
				{
					if (blnGerarComoEmpresa)
						strSQL = "update Pessoa set IN_EMPRESA= 1  Where CD_PESSOA = @v2";
					else
						strSQL = "update Pessoa set IN_EMPRESA= 0  Where CD_PESSOA = @v2";
				}
				else if (IndicadorPessoa == 2)
				{
					if (blnGerarComoUsuario)
						strSQL = "update Pessoa set IN_USUARIO= 1  Where CD_PESSOA = @v2";
					else
						strSQL = "update Pessoa set IN_USUARIO = 0  Where CD_PESSOA = @v2";
				}


				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v2", CodigoPessoa);
				Cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao atualizar Pessoa Indicar a ser Empresa: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public decimal VerificaCreditoUsadoCliente(decimal CodigoPessoa, ref decimal EmPedidos, ref decimal decLimiteCredito)
		{
			try
			{
				AbrirConexao();

				string strSQL = "SELECT  " +
									"ISNULL((" +
										"SELECT " +
											"SUM(CT_REC_A_VENCER.VL_TOTAL_GERAL - CT_REC_A_VENCER.VL_PAGO) " +
										"FROM " +
											"VW_DOC_CTA_RECEBER AS CT_REC_A_VENCER " +
										"WHERE " +
											"CT_REC_A_VENCER.CD_PESSOA = P.CD_PESSOA AND CT_REC_A_VENCER.DT_VENCIMENTO > GETDATE()) " +
									",0)  " +
										"AS VL_CONTAS_A_VENCER, " +
									"ISNULL((" +
										"SELECT " +
											"SUM(VL_TOTAL_GERAL) " +
										"FROM " +
											"VW_DOC_PEDIDO " +
										"WHERE " +
									"CD_PESSOA = @v1 AND CD_SITUACAO != 136 AND CD_SITUACAO != 145 AND CD_SITUACAO != 150) " +
									",0) AS VL_PEDIDOS_EM_ANDAMENTO, P.VL_LIMITE_CREDITO " +
								"FROM " +
									"PESSOA AS P " +
							   " WHERE " +
									"P.CD_PESSOA = @v1 ";

				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", CodigoPessoa);

				Dr = Cmd.ExecuteReader();
				if (Dr.Read())
				{
					decLimiteCredito = Convert.ToDecimal(Dr["VL_LIMITE_CREDITO"]);
					EmPedidos = Convert.ToDecimal(Dr["VL_PEDIDOS_EM_ANDAMENTO"]);
					return Convert.ToDecimal(Dr["VL_CONTAS_A_VENCER"]);
				}
				else
				{
					return 0;
				}


			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar verificar credito cliente: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Pessoa> ListarTransportadores(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from VW_TRANSPORTADOR ";

				if (strValor != "")
					strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);
				if (strOrdem != "")
					strSQL = strSQL + "Order By " + strOrdem;

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Pessoa> lista = new List<Pessoa>();
				while (Dr.Read())
				{
					Pessoa p = new Pessoa();
					p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
					p.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);
					p.NomeFantasia = Convert.ToString(Dr["NM_FANTASIA"]);
					p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
					p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.CodigoSituacaoPessoa = Convert.ToInt64(Dr["CD_SITUACAO_PESSOA"]);
					p.CodigoSituacaoFase = Convert.ToInt64(Dr["CD_SITUACAO_FASE"]);
					p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
					p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
					p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
					p.PessoaEmpresa = Convert.ToInt32(Dr["IN_EMPRESA"]);
					p.PessoaCliente = Convert.ToInt32(Dr["IN_CLIENTE"]);
					p.PessoaFornecedor = Convert.ToInt32(Dr["IN_FORNECEDOR"]);
					p.PessoaTransportador = Convert.ToInt32(Dr["IN_TRANSPORTADOR"]);
					p.PessoaVendedor = Convert.ToInt32(Dr["IN_VENDEDOR"]);
					p.PessoaUsuario = Convert.ToInt32(Dr["IN_USUARIO"]);
					p.CodigoCondPagamento = Convert.ToInt32(Dr["CD_CND_PAGAMENTO"]);
					p.CodigoPlanoContas = Convert.ToInt32(Dr["CD_PLANO_CONTA"]);
					p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
					if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
						p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
					if (Dr["NR_PROJECAO"] != DBNull.Value)
						p.NumeroProjecao = Convert.ToDecimal(Dr["NR_PROJECAO"]);


					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todas Pessoas: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
        public void AtualizarPessoaComprador(Int64 cd_pessoa, int in_vendedor)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update Pessoa set IN_COMPRADOR = @v2 Where CD_PESSOA = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", cd_pessoa);
                Cmd.Parameters.AddWithValue("@v2", in_vendedor);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Indicador de Comprador: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public DataTable RelatorioMatriculados(int intCodigoGrupoPessoa)
        {
            try
            {
                AbrirConexao();
                DataTable dt = new DataTable();
                string comando = "";
                if (intCodigoGrupoPessoa == 0)
                {
                    comando = "select P.NM_PESSOA, grup.DS_GPO_PESSOA, format(P.DT_CADASTRO ,'dd/MM/yyyy') AS DT_FILIACAO," +
                                    "ISNULL(P.CD_SIS_ANTERIOR,'') AS CD_MATRICULA,CONVERT(NVARCHAR,PEND.DS_MUNICIPIO + ' - ' + SUBSTRING(PEND.DS_ESTADO,1,2)) AS DS_MUNICIPIO, format(INS.DT_ABERTURA ,'dd/MM/yyyy') AS DT_NASCIMENTO, ISNULL(ctt.IM_FOTO,0) AS IM_FOTO from PESSOA as p " +
                                    "inner join GRUPO_DE_PESSOA as grup on grup.CD_GPO_PESSOA = p.CD_GPO_PESSOA " +
                                    "INNER JOIN PESSOA_ENDERECO AS PEND ON PEND.CD_PESSOA = P.CD_PESSOA AND PEND.CD_ENDERECO = 1 " +
                                    "INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = P.CD_PESSOA AND INS.CD_INSCRICAO = 1 " +
                                    "INNER JOIN PESSOA_CONTATO AS CTT ON CTT.CD_PESSOA = P.CD_PESSOA AND CTT.CD_CONTATO = 1 " +
                                "where grup.IN_GERAR_MATRICULA = 1 ORDER BY CD_SIS_ANTERIOR ";
                }
                else
                {
                    comando = "select P.NM_PESSOA, grup.DS_GPO_PESSOA, format(P.DT_CADASTRO ,'dd/MM/yyyy') AS DT_FILIACAO," +
                                "ISNULL(P.CD_SIS_ANTERIOR,'') AS CD_MATRICULA,CONVERT(NVARCHAR,PEND.DS_MUNICIPIO + ' - ' + SUBSTRING(PEND.DS_ESTADO,1,2)) AS DS_MUNICIPIO, format(INS.DT_ABERTURA ,'dd/MM/yyyy') AS DT_NASCIMENTO, ISNULL(ctt.IM_FOTO,0) AS IM_FOTO from PESSOA as p " +
                                    "inner join GRUPO_DE_PESSOA as grup on grup.CD_GPO_PESSOA = p.CD_GPO_PESSOA " +
                                    "INNER JOIN PESSOA_ENDERECO AS PEND ON PEND.CD_PESSOA = P.CD_PESSOA AND PEND.CD_ENDERECO = 1 " +
                                    "INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = P.CD_PESSOA AND INS.CD_INSCRICAO = 1 " +
                                    "INNER JOIN PESSOA_CONTATO AS CTT ON CTT.CD_PESSOA = P.CD_PESSOA AND CTT.CD_CONTATO = 1 " +
                                "where grup.IN_GERAR_MATRICULA = 1  ORDER BY CD_SIS_ANTERIOR ";
                }

                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigoGrupoPessoa);

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);

                FecharConexao();

                return dt;


            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Indicador de Comprador: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public void InserirRepresentantes(long longCodigoPessoa, List<Vendedor> ListaRepresentantes)
        {
            try
            {
                ExcluirTodosRepresentantes(longCodigoPessoa);
                AbrirConexao();

                if (ListaRepresentantes == null)
                    return;
                foreach (Vendedor p in ListaRepresentantes)
                {
                    Cmd = new SqlCommand("insert into REPRESENTANTE_DA_PESSOA (CD_PESSOA, CD_VENDEDOR) " +
                    "values (@v1,@v2);", Con);
                    Cmd.Parameters.AddWithValue("@v1", longCodigoPessoa);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoVendedor);


                    Cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 2601: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        case 2627: // Primary key violation
                            throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao gravar REPRESENTANTE_DA_PESSOA: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar REPRESENTANTE_DA_PESSOA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodosRepresentantes(long longCodPessoa)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from REPRESENTANTE_DA_PESSOA Where CD_PESSOA = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", longCodPessoa);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir REPRESENTANTE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Vendedor> ObterRepresentantes(long longCodigoPessoa)
        {
            try
            {
                AbrirConexao();
                string comando = "Select R.*,P.NM_PESSOA from REPRESENTANTE_DA_PESSOA as R " +
                                    "inner join VENDEDOR AS V ON V.CD_VENDEDOR = R.CD_VENDEDOR " +
                                    "INNER JOIN PESSOA AS P ON P.CD_PESSOA = V.CD_PESSOA " +
                                "WHERE R.CD_PESSOA = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", longCodigoPessoa);

                Dr = Cmd.ExecuteReader();
                List<Vendedor> anexo = new List<Vendedor>();

                while (Dr.Read())
                {
                    Vendedor p = new Vendedor();
                    p.CodigoVendedor = Convert.ToInt32(Dr["CD_VENDEDOR"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.Pessoa.NomePessoa = Convert.ToString(Dr["NM_PESSOA"]);

                    anexo.Add(p);
                }
                return anexo;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar REPRESENTANTE_DA_PESSOA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public DataTable ListarPessoasComRepresentantes(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                DataTable dt = new DataTable();

                AbrirConexao();

                string SQL = "SELECT P.COD_PESSOA,P.NOME_PESSOA,P.NR_INSCRICAO,P.DS_BAIRRO,P.DS_MUNICIPIO, " +
                                        "SUBSTRING(P.DS_ESTADO, 0, 3) AS DS_ESTADO, PAIS.DS_PAIS, CTT.NM_CONTATO, CTT.NR_FONE1, CTT.TX_MAIL1, G.DS_GPO_PESSOA  FROM " +
                                        "VW_PESSOA AS P " +
                                        "INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = P.COD_PESSOA AND INS.CD_INSCRICAO = 1 " +
                                        "INNER JOIN PAIS ON PAIS.CD_PAIS = INS.CD_PAIS " +
                                        "INNER JOIN PESSOA_CONTATO AS CTT ON CTT.CD_PESSOA = P.COD_PESSOA AND CTT.CD_CONTATO = 1 " +
                                        "INNER JOIN GRUPO_DE_PESSOA AS G ON G.CD_GPO_PESSOA = P.CD_GPO_PESSOA ";

                if (ListaFiltros != null && ListaFiltros.Count > 0)
                    SQL += MontaFiltroIntervalo(ListaFiltros) + " AND (SELECT COUNT(R.CD_PESSOA) FROM REPRESENTANTE_DA_PESSOA AS R WHERE R.CD_PESSOA = P.COD_PESSOA) > 0";
                else
                    SQL += " WHERE (SELECT COUNT(R.CD_PESSOA) FROM REPRESENTANTE_DA_PESSOA AS R WHERE R.CD_PESSOA = P.COD_PESSOA) > 0";

                Cmd = new SqlCommand(SQL, Con);

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);
                FecharConexao();

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar representantes: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
