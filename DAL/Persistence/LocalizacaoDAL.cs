using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
	public class LocalizacaoDAL : Conexao
	{
		protected string strSQL = "";

		public void Inserir(Localizacao p)
		{
			try
			{
				AbrirConexao();

				strSQL = "insert into [LOCALIZACAO] (CD_EMPRESA, CD_LOCALIZACAO, DS_LOCALIZACAO, CD_SITUACAO, CD_TIPO_LOCALIZACAO) values (@v1, @v2, @v3, @v4, @v5)";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
				Cmd.Parameters.AddWithValue("@v2", p.CodigoLocalizacao.ToUpper());
				Cmd.Parameters.AddWithValue("@v3", p.DescricaoLocalizacao);
				Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);
				Cmd.Parameters.AddWithValue("@v5", p.CodigoTipoLocalizacao);
				p.CodigoIndice = Convert.ToInt32(Cmd.ExecuteScalar());


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
							throw new Exception("Erro ao Incluir LOCALIZAÇÃO: " + ex.Message.ToString());
					}
				}

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao gravar LOCALIZAÇÃO: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public void Atualizar(Localizacao p)
		{
			try
			{
				AbrirConexao();
				strSQL = "update [LOCALIZACAO] set [DS_LOCALIZACAO] = @v2,  [CD_SITUACAO] = @v4,  [CD_TIPO_LOCALIZACAO] = @v5  Where [CD_EMPRESA] = @v1 AND [CD_LOCALIZACAO] = @v3";
				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v3", p.CodigoLocalizacao);
				Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
				Cmd.Parameters.AddWithValue("@v2", p.DescricaoLocalizacao);
				Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);
				Cmd.Parameters.AddWithValue("@v5", p.CodigoTipoLocalizacao);
				Cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao atualizar LOCALIZAÇÃO: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public void Excluir(long CodEmpresa, string strCodigo)
		{
			try
			{
				AbrirConexao();

				Cmd = new SqlCommand("delete from [LOCALIZACAO] Where [CD_EMPRESA] = @v1 and [CD_LOCALIZACAO] = @v2", Con);

				Cmd.Parameters.AddWithValue("@v1", CodEmpresa);
				Cmd.Parameters.AddWithValue("@v2", strCodigo);

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
							throw new Exception("Erro ao excluir LOCALIZAÇÃO: " + ex.Message.ToString());
					}
				}
			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao excluir LOCALIZAÇÃO: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Localizacao PesquisarLocalizacao(long CodEmpresa, string strLocalizacao)
		{
			try
			{
				AbrirConexao();
				strSQL = "Select * from [LOCALIZACAO] Where CD_EMPRESA = @v1 and CD_LOCALIZACAO = @v2";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", CodEmpresa);
				Cmd.Parameters.AddWithValue("@v2", strLocalizacao);

				Dr = Cmd.ExecuteReader();

				Localizacao p = null;

				if (Dr.Read())
				{
					p = new Localizacao();

					p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
					p.DescricaoLocalizacao = Convert.ToString(Dr["DS_LOCALIZACAO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.CodigoTipoLocalizacao = Convert.ToInt32(Dr["CD_TIPO_LOCALIZACAO"]);
					p.Cpl_DescDDL = p.CodigoLocalizacao.ToString() + " - " + p.DescricaoLocalizacao.ToString();
				}

				return p;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar LOCALIZACAO: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Localizacao PesquisarLocalizacaoIndice(long lngIndice)
		{
			try
			{
				AbrirConexao();
				strSQL = "Select * from [LOCALIZACAO] Where CD_Index = @v1";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", lngIndice);

				Dr = Cmd.ExecuteReader();

				Localizacao p = null;

				if (Dr.Read())
				{
					p = new Localizacao();

					p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
					p.DescricaoLocalizacao = Convert.ToString(Dr["DS_LOCALIZACAO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.CodigoTipoLocalizacao = Convert.ToInt32(Dr["CD_TIPO_LOCALIZACAO"]);
					p.Cpl_DescDDL = p.CodigoLocalizacao.ToString() + " - " + p.DescricaoLocalizacao.ToString();
				}

				return p;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar LOCALIZACAO: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Localizacao> ListarLocalizacao(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [LOCALIZACAO]";

				if (strValor != "")
					strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

				if (strOrdem != "")
					strSQL = strSQL + "Order By " + strOrdem;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Localizacao> lista = new List<Localizacao>();

				while (Dr.Read())
				{
					Localizacao p = new Localizacao();

					p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
					p.DescricaoLocalizacao = Convert.ToString(Dr["DS_LOCALIZACAO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.CodigoTipoLocalizacao = Convert.ToInt32(Dr["CD_TIPO_LOCALIZACAO"]);
					p.Cpl_DescDDL = p.CodigoLocalizacao.ToString() + " - " + p.DescricaoLocalizacao.ToString();

					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas LOCALIZACAO: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Localizacao> ListarLocalizacaoProducao(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [LOCALIZACAO] where CD_TIPO_LOCALIZACAO = 181 ";

				if (strValor != "")
					strSQL = strSQL + " and  " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

				if (strOrdem != "")
					strSQL = strSQL + "Order By " + strOrdem;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Localizacao> lista = new List<Localizacao>();

				while (Dr.Read())
				{
					Localizacao p = new Localizacao();

					p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
					p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
					p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
					p.DescricaoLocalizacao = Convert.ToString(Dr["DS_LOCALIZACAO"]);
					p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
					p.CodigoTipoLocalizacao = Convert.ToInt32(Dr["CD_TIPO_LOCALIZACAO"]);
					p.Cpl_DescDDL = p.CodigoLocalizacao.ToString() + " - " + p.DescricaoLocalizacao.ToString();

					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas LOCALIZACAO: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public DataTable ObterLocalizacao(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [LOCALIZACAO]";

				if (strValor != "")
					strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

				if (strOrdem != "")
					strSQL = strSQL + "Order By " + strOrdem;


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				// Cria DataTable
				DataTable dt = new DataTable();
				dt.Columns.Add("CodigoIndice", typeof(int));
				dt.Columns.Add("CodigoEmpresa", typeof(int));
				dt.Columns.Add("CodigoLocalizacao", typeof(string));
				dt.Columns.Add("DescricaoLocalizacao", typeof(string));
				dt.Columns.Add("CodigoSituacao", typeof(int));
				dt.Columns.Add("CodigoTipoLocalizacao", typeof(int));

				while (Dr.Read())
					dt.Rows.Add(Convert.ToInt32(Dr["CD_INDEX"]), Convert.ToInt32(Dr["CD_EMPRESA"]), Convert.ToString(Dr["CD_LOCALIZACAO"]),
						Convert.ToString(Dr["DS_LOCALIZACAO"]), Convert.ToInt32(Dr["CD_SITUACAO"]), Convert.ToInt32(Dr["CD_TIPO_LOCALIZACAO"]));

				return dt;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todas Localizações: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}

	}
}
