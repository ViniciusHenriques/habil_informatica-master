using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
	public class ComposicaoDAL : Conexao
	{
		protected string strSQL = "";

		public void Inserir(Composicao p)
		{
			try
			{
				AbrirConexao();

				strSQL = "insert into [COMPOSICAO] (CD_COMPOSTO, CD_SITUACAO, CD_TIPO, VL_CUSTO_PRODUTO, VL_RENDIMENTO, VL_PCT_UMIDADE, VL_PCT_QUEBRA, TX_OBS, DT_ATUALIZACAO) " +
					"values (@v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8, getdate())";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", p.CodigoProdutoComposto);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoTipo);
				Cmd.Parameters.AddWithValue("@v4", p.ValorCustoProduto);
				Cmd.Parameters.AddWithValue("@v5", p.Rendimento);
				Cmd.Parameters.AddWithValue("@v6", p.PercentualUmidade);
				Cmd.Parameters.AddWithValue("@v7", p.PercentualQuebra);
				Cmd.Parameters.AddWithValue("@v8", p.Observacao);

				Cmd.ExecuteNonQuery();

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
							throw new Exception("Erro ao Incluir Bairro: " + ex.Message.ToString());
					}
				}

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao gravar Bairro: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public void Atualizar(Composicao p)
		{
			try
			{
				AbrirConexao();
				strSQL = "update [COMPOSICAO] set [CD_SITUACAO] = @v2, [CD_TIPO] = @v3 " +
					", [VL_CUSTO_PRODUTO] = @v4, [VL_RENDIMENTO] = @v5 , [VL_PCT_UMIDADE] = @v6, [VL_PCT_QUEBRA] = @v7, [TX_OBS] = @v8, [DT_ATUALIZACAO] = getdate() " +
					" Where [CD_COMPOSTO] = @v1";
				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v1", p.CodigoProdutoComposto);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
				Cmd.Parameters.AddWithValue("@v3", p.CodigoTipo);
				Cmd.Parameters.AddWithValue("@v4", p.ValorCustoProduto);
				Cmd.Parameters.AddWithValue("@v5", p.Rendimento);
				Cmd.Parameters.AddWithValue("@v6", p.PercentualUmidade);
				Cmd.Parameters.AddWithValue("@v7", p.PercentualQuebra);
				Cmd.Parameters.AddWithValue("@v8", p.Observacao);
				Cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao atualizar Bairro: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public void AtualizarObservacao(Composicao p)
		{
			try
			{
				AbrirConexao();
				strSQL = "update [COMPOSICAO] set [TX_OBS] = @v2 " +
					" Where [CD_COMPOSTO] = @v1";
				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v1", p.CodigoProdutoComposto);
				Cmd.Parameters.AddWithValue("@v2", p.Observacao);
				Cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao atualizar Bairro: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public void Excluir(int Codigo)
		{
			try
			{
				AbrirConexao();

				Cmd = new SqlCommand("delete from [COMPOSICAO] Where [CD_COMPOSTO] = @v1", Con);

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
							throw new Exception("Erro ao excluir Bairro: " + ex.Message.ToString());
					}
				}
			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao excluir Bairro: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Composicao> ListarComposicao(List<DBTabelaCampos> ListaFiltros)
		{
			try
			{
				AbrirConexao();

				string strValor = "";
				string strSQL = "select * from VW_COMPOSICAO";

				strValor = MontaFiltroIntervalo(ListaFiltros);

				strSQL = strSQL + strValor;
				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Composicao> lista = new List<Composicao>();

				while (Dr.Read())
				{
					Composicao p = new Composicao();

					p.CodigoProdutoComposto = Convert.ToInt32(Dr["CD_COMPOSTO"]);
					p.DescricaoProduto = Convert.ToString(Dr["NM_COMPOSTO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.DescricaoSituacao = Convert.ToString(Dr["DS_SITUACAO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.ValorCustoProduto = Convert.ToDecimal(Dr["VL_CUSTO_PRODUTO"]);
					p.Data = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Composições: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Composicao PesquisarComposicao(int cod)
		{
			try
			{
				AbrirConexao();

				string strSQL = "select C.*, P.NM_PRODUTO from [Composicao] as c" +
					" inner join produto as p" +
					" on p.cd_produto = c.cd_composto" +
					" where CD_COMPOSTO = " + cod;


				Dr = new SqlCommand(strSQL, Con).ExecuteReader();

				Composicao p = new Composicao();

				if (Dr.Read())
				{
					p = new Composicao();

					p.CodigoProdutoComposto = Convert.ToInt32(Dr["CD_COMPOSTO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.ValorCustoProduto = Convert.ToDecimal(Dr["VL_CUSTO_PRODUTO"]);
					p.Rendimento = Convert.ToDecimal(Dr["VL_RENDIMENTO"]);
					p.PercentualUmidade = Convert.ToDecimal(Dr["VL_PCT_UMIDADE"]);
					p.PercentualQuebra = Convert.ToDecimal(Dr["VL_PCT_QUEBRA"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
					p.Data = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
					p.DescricaoProduto = Convert.ToString(Dr["NM_PRODUTO"]);

				}
				return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Composições: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public DataTable RelComposicao(List<DBTabelaCampos> ListaFiltros)
		{
			try
			{
				DataTable dt = new DataTable();
				string strValor = "";
				strSQL = "SELECT * FROM VW_COMPOSICAO";

				strValor = MontaFiltroIntervalo(ListaFiltros);

				strSQL = strSQL + strValor;

				AbrirConexao();
				SqlCommand cmd = new SqlCommand(strSQL, Con);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);

				return dt;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar DataSet: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Composicao> PesquisarListaComposicao(string strDescricao)
		{
			try
			{
				AbrirConexao();

				strSQL = "select * from COMPOSICAO AS C " +
					"INNER JOIN PRODUTO AS P " +
						"ON C.CD_COMPOSTO = P.CD_PRODUTO " +
					"INNER JOIN HABIL_TIPO AS H " +
						"ON H.CD_TIPO = C.CD_SITUACAO " +
					"where P.NM_PRODUTO like '%" + strDescricao + "%'";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Composicao> lista = new List<Composicao>();

				while (Dr.Read())
				{
					Composicao p = new Composicao();

					p.CodigoProdutoComposto = Convert.ToInt32(Dr["CD_COMPOSTO"]);
					p.DescricaoProduto = Convert.ToString(Dr["NM_PRODUTO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.DescricaoSituacao = Convert.ToString(Dr["DS_TIPO"]);
					p.ValorCustoProduto = Convert.ToDecimal(Dr["VL_CUSTO_PRODUTO"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Composições: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public ItemDaComposicao PesquisarProdutoComRoteiro(int intcod, int intComp)
		{
			try
			{
				AbrirConexao();

				strSQL = "select CD_COMPONENTE, CD_ROTEIRO, DS_ROTEIRO from item_da_composicao where CD_COMPOSTO = " + intcod + " and CD_COMPONENTE = " + intComp;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				ItemDaComposicao p = new ItemDaComposicao();

				if (Dr.Read())
				{
					p = new ItemDaComposicao();

					p.CodigoComponente = Convert.ToInt32(Dr["CD_COMPONENTE"]);
					p.CodigoRoteiro = Convert.ToInt16(Dr["CD_ROTEIRO"]);
					p.DescRoteiro = Convert.ToString(Dr["DS_ROTEIRO"]);
				}
				return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Composições: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
	}
}
