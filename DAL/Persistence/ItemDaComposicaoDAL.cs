using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
	public class ItemDaComposicaoDAL : Conexao
	{
		protected string strSQL = "";

		public void Inserir(ItemDaComposicao p)
		{
			try
			{
				AbrirConexao();
				strSQL = "insert into [ITEM_DA_COMPOSICAO] (CD_COMPOSTO, CD_COMPONENTE, VL_CUSTO_COMPONENTE, QT_COMPONENTE, VL_PCT_QUEBRA, TX_OBS, CD_ROTEIRO, DS_ROTEIRO) " +
					"values ( @v1, @v2, @v3, @v4, @v5, @v6 , @v7 , @v8 )";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", p.CodigoComposto);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoComponente);
				Cmd.Parameters.AddWithValue("@v3", p.ValorCustoComponente);
				Cmd.Parameters.AddWithValue("@v4", p.QuantidadeComponente);
				Cmd.Parameters.AddWithValue("@v5", p.PerQuebraComponente);
				Cmd.Parameters.AddWithValue("@v6", p.Observacao);
				Cmd.Parameters.AddWithValue("@v7", p.CodigoRoteiro);
				Cmd.Parameters.AddWithValue("@v8", p.DescRoteiro);

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
		public void Atualizar(ItemDaComposicao p)
		{
			try
			{
				AbrirConexao();
				strSQL = "update [ITEM_DA_COMPOSICAO] set [CD_COMPONENTE] = @v2, [VL_CUSTO_COMPONENTE] = @v3, [QT_COMPONENTE] = @v4 " +
					"Where [CD_COMPOSTO] = @v1";
				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v1", p.CodigoComposto);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoComponente);
				Cmd.Parameters.AddWithValue("@v3", p.ValorCustoComponente);
				Cmd.Parameters.AddWithValue("@v4", p.QuantidadeComponente);
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

				Cmd = new SqlCommand("delete from [ITEM_DA_COMPOSICAO] Where [CD_COMPOSTO] = @v1", Con);

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
		public List<ItemDaComposicao> ListarItemDaComposicao(int cod)
		{
			try
			{
				AbrirConexao();

				string strSQL = "select * from [ITEM_DA_COMPOSICAO] AS I " +
					"INNER JOIN PRODUTO AS P " +
						"ON P.CD_PRODUTO = I.CD_COMPONENTE " +
					"INNER JOIN UNIDADE AS U " +
						"ON U.CD_UNIDADE = P.CD_UNIDADE " +
					" where CD_COMPOSTO = " + cod;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<ItemDaComposicao> lista = new List<ItemDaComposicao>();

				while (Dr.Read())
				{
					ItemDaComposicao p = new ItemDaComposicao();

					p.CodigoComponente = Convert.ToInt32(Dr["CD_COMPONENTE"]);
					p.CodigoComposto = Convert.ToInt32(Dr["CD_COMPOSTO"]);
					p.DescricaoComponente = Convert.ToString(Dr["NM_PRODUTO"]);
					p.PerQuebraComponente = Convert.ToDecimal(Dr["VL_PCT_QUEBRA"]);
					p.QuantidadeComponente = Convert.ToDecimal(Dr["QT_COMPONENTE"]);
					p.ValorCustoComponente = Convert.ToDecimal(Dr["VL_CUSTO_COMPONENTE"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
					p.CodigoRoteiro = Convert.ToInt16(Dr["CD_ROTEIRO"]);
					p.DescRoteiro = Convert.ToString(Dr["DS_ROTEIRO"]);
					p.Unidade = Convert.ToString(Dr["SIGLA"]);

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
		public List<ItemDaComposicao> ListarRoteiros(int intCod)
		{
			try
			{
				AbrirConexao();

				string strSQL = "SELECT DISTINCT CD_ROTEIRO, DS_ROTEIRO FROM ITEM_DA_COMPOSICAO WHERE CD_COMPOSTO = " + intCod;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<ItemDaComposicao> lista = new List<ItemDaComposicao>();

				while (Dr.Read())
				{
					ItemDaComposicao p = new ItemDaComposicao();

					p.CodigoRoteiro = Convert.ToInt16(Dr["CD_ROTEIRO"]);
					p.DescRoteiro = Convert.ToString(Dr["DS_ROTEIRO"]);

					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Roteiros: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public ItemDaComposicao ValidacaoParaComposicaoNaoCircular(int intCodComposto, int intCodComponente)
		{
			try
			{
				AbrirConexao();

				string strSQL = "SELECT * FROM ITEM_DA_COMPOSICAO WHERE CD_COMPOSTO = " + intCodComponente + " AND CD_COMPONENTE = " + intCodComposto;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				ItemDaComposicao p = new ItemDaComposicao();

				while (Dr.Read())
				{
					p = new ItemDaComposicao();

					p.CodigoComposto = Convert.ToInt32(Dr["CD_COMPOSTO"]);
					p.CodigoComponente = Convert.ToInt32(Dr["CD_COMPONENTE"]);

				}
				return p;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Roteiros: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<ItemDaComposicao> ListarRoteirosComDistinctParaDocumento(decimal decCodDocumento)
		{
			try
			{
				AbrirConexao();

				string strSQL = "select distinct(i.CD_ROTEIRO) , i.DS_ROTEIRO, p.DT_INICIO_ROTEIRO,p.DT_FIM_ROTEIRO from ITEM_DA_COMPOSICAO as i " +
									"inner join PRODUTO_DO_DOCUMENTO as p " +
										"on i.CD_COMPONENTE = p.CD_PRODUTO " +
								"Where CD_DOCUMENTO = " + decCodDocumento;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				ItemDaComposicao p = new ItemDaComposicao();
				List<ItemDaComposicao> Lista = new List<ItemDaComposicao>();

				while (Dr.Read())
				{
					p = new ItemDaComposicao();

					if (Dr["DT_INICIO_ROTEIRO"] != DBNull.Value)
						p.DataInicio = Convert.ToDateTime(Dr["DT_INICIO_ROTEIRO"]);
					if (Dr["DT_FIM_ROTEIRO"] != DBNull.Value)
						p.DataFim = Convert.ToDateTime(Dr["DT_FIM_ROTEIRO"]);
					p.CodigoRoteiro = Convert.ToInt16(Dr["CD_ROTEIRO"]);
					p.DescRoteiro = (Dr["DS_ROTEIRO"]).ToString();
					Lista.Add(p);
				}
				return Lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Roteiros: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
	}
}
