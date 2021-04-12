using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
	public class Habil_TipoDAL : Conexao
	{
		protected string strSQL = "";
		public void Inserir(Habil_Tipo p)
		{
			try
			{
				AbrirConexao();

				strSQL = "insert into [HABIL_TIPO] (CD_TIPO, DS_TIPO, TP_TIPO, TX_OBS) values (@v1, @v2,@v3,@v4)";
				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v1", p.CodigoTipo);
				Cmd.Parameters.AddWithValue("@v2", p.DescricaoTipo);
				Cmd.Parameters.AddWithValue("@v3", p.TipoTipo);
				Cmd.Parameters.AddWithValue("@v4", p.Observacao);

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
							throw new Exception("Erro ao Incluir tipo: " + ex.Message.ToString());
					}
				}

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao gravar Tipo e Situação: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public void Atualizar(Habil_Tipo p)
		{
			try
			{
				AbrirConexao();
				strSQL = "update [HABIL_TIPO] set [DS_TIPO] = @v2, TP_TIPO = @v3, TX_OBS = @v4 Where [CD_TIPO] = @v1";
				Cmd = new SqlCommand(strSQL, Con);

				Cmd.Parameters.AddWithValue("@v1", p.CodigoTipo);
				Cmd.Parameters.AddWithValue("@v2", p.DescricaoTipo);
				Cmd.Parameters.AddWithValue("@v3", p.TipoTipo);
				Cmd.Parameters.AddWithValue("@v4", p.Observacao);
				Cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao atualizar Tipo: " + ex.Message.ToString());
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

				Cmd = new SqlCommand("delete from [HABIL_TIPO] Where [CD_TIPO] = @v1", Con);

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
							throw new Exception("Erro ao excluir Tipos e Situação: " + ex.Message.ToString());
					}
				}
			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao excluir Tipo: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public String DescricaoHabil_Tipo(int Codigo)
		{
			try
			{
				AbrirConexao();
				strSQL = "Select DS_TIPO from [HABIL_TIPO] Where CD_TIPO = @v1";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);

				Dr = Cmd.ExecuteReader();


				if (Dr.Read())
					return Convert.ToString(Dr["DS_TIPO"]);
				else
					return "";

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Tipo: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public Habil_Tipo PesquisarHabil_Tipo(long Codigo)
		{
			try
			{
				AbrirConexao();
				strSQL = "Select * from [HABIL_TIPO] Where CD_TIPO = @v1";
				Cmd = new SqlCommand(strSQL, Con);
				Cmd.Parameters.AddWithValue("@v1", Codigo);

				Dr = Cmd.ExecuteReader();

				Habil_Tipo p = null;

				if (Dr.Read())
				{
					p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                }

				return p;

			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Pesquisar Tipo: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> ListarHabilTipos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO]";

				if (strValor != "")
					strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

				if (strOrdem != "")
					strSQL = strSQL + "Order By " + strOrdem;

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public DataTable ObterHabilTipos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO]";

				if (strValor != "")
					strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

				if (strOrdem != "")
					strSQL = strSQL + "Order By " + strOrdem;


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				// Cria DataTable
				DataTable dt = new DataTable();
				dt.Columns.Add("CodigoTipo", typeof(Int32));
				dt.Columns.Add("DescricaoTipo", typeof(string));
				dt.Columns.Add("TipoTipo", typeof(Int32));
				dt.Columns.Add("Observacao", typeof(string));
                dt.Columns.Add("CodigoReferencia", typeof(Int32));

                while (Dr.Read())
					dt.Rows.Add(Convert.ToInt32(Dr["CD_TIPO"]), Convert.ToString(Dr["DS_TIPO"]), Convert.ToInt32(Dr["TP_TIPO"]), Convert.ToString(Dr["TX_OBS"]), Convert.ToInt32(Dr["CD_REFERENCIA"]));
				return dt;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> Atividade()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(1,2)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
					lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoPessoa()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(3,4)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> TipoEndereco()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(5,6,7,8,9)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> TipoContato()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(10,11,12,13,14)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> EtapasCadPessoa()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(15,16,17,18)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> Existencia()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(19,20)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public List<Habil_Tipo> TipoDocumento()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(21,22)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}


		}
		public List<Habil_Tipo> CondPagamento()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(23,24,25,26,27)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> TipoProduto()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(28,29,30)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.TipoTipo = Convert.ToInt32(Dr["TP_TIPO"]);
					p.Observacao = Convert.ToString(Dr["TX_OBS"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> ClassificaCtaPagar()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(32)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> SituacaoCtaPagar()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(31,36,37,38)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> SituacaoNotaFiscalServico()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(42,39,40,41)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> TipoOrcamento()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(85)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}



		}
		public List<Habil_Tipo> TipoBaixa()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(33,34,35)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}

		}
		public List<Habil_Tipo> TipoLocalizacao()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(46,47,48,49)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoModDetBCICMS()
		{
			try
			{
				AbrirConexao();
				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(50,51,52,53)";
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Dr["CD_REFERENCIA"].ToString() + ". " + Convert.ToString(Dr["DS_TIPO"]);
					p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> TipoModDetBCICMSST()
		{
			try
			{
				AbrirConexao();
				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(54,55,56,57,58,59,78)";
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Dr["CD_REFERENCIA"].ToString() + ". " + Convert.ToString(Dr["DS_TIPO"]);
					p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> TipoMovimentacaoEstoque()
		{
			try
			{
				AbrirConexao();
				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(60,61,62)";
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> MotivoDesoneracaoCST207090()
		{
			try
			{
				AbrirConexao();
				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(63,64,65)";
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Dr["CD_REFERENCIA"].ToString() + ". " + Convert.ToString(Dr["DS_TIPO"]);
					p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> MotivoDesoneracaoCST30()
		{
			try
			{
				AbrirConexao();
				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(66,67,68)";
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Dr["CD_REFERENCIA"].ToString() + ". " + Convert.ToString(Dr["DS_TIPO"]);
					p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> MotivoDesoneracaoCST404150()
		{
			try
			{
				AbrirConexao();
				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(69, 70, 71, 72, 73, 74, 75, 76, 77,79)";
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Dr["CD_REFERENCIA"].ToString() + ". " + Convert.ToString(Dr["DS_TIPO"]);
					p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> SituacaoSolicitacaoAtendimento()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(86,87,88,89,94)";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoOrdemFatura()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(103,104,105)";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoOrdemServico()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(101,102,106,107,108,109)";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoItemOrdemServico()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(99,100)";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoOrdemServico()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(97,98)";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoSolicitacaoAtendimento()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(93,95,96)";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoGeradorEmail()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(110,111,112,113)";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoDeVendedor()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(114,115,116,117,118)";

				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}

				return lista;

			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoCTe()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(40,41,42,39)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoEvento()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(120)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoEventoEletronico()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(119,121,122,123)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}

		public List<Habil_Tipo> TipoAplicacaoUso()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(141,142,143)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoTributacao()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(125,126,127)";


				Cmd = new SqlCommand(strSQL, Con);

				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();

				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();

					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{

				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoEstoque()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(128,129)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoOrcamento()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(136,137,138,139,144)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoPedido()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(136,137,145,146,158,150,154,155,156,157)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoOrdemGeracaoNFe()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(147,148,149,209)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoListagemPedido()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(151, 152, 153)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> TipoOperacaoPrecedenciaICMS()
		{
			try
			{
				AbrirConexao();
				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(159)";
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> TipoOperacaoPrecedenciaPIS_COFINS()
		{
			try
			{
				AbrirConexao();
				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(159)";
				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();
				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				while (Dr.Read())
				{
					Habil_Tipo p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}
		public List<Habil_Tipo> SituacaoEmbalagem()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(160, 161)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoAgenda()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(167, 168,169)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}

		public List<Habil_Tipo> TipoCompromisso()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(170, 171)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoComposicao()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(165, 166)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoAplicaUsoOrdemProducao()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(174, 175) order by cd_tipo desc";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
		public List<Habil_Tipo> SituacaoOrdemDeProducao()
		{
			try
			{
				AbrirConexao();

				string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(177,178,179,180)";

				Cmd = new SqlCommand(strSQL, Con);
				Dr = Cmd.ExecuteReader();

				List<Habil_Tipo> lista = new List<Habil_Tipo>();
				Habil_Tipo p;

				while (Dr.Read())
				{
					p = new Habil_Tipo();
					p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
					p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();

			}
		}
        public List<Habil_Tipo> TipoIndicadorPresenca()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(184,185,186,187,188,189)";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Habil_Tipo> lista = new List<Habil_Tipo>();
                Habil_Tipo p;

                while (Dr.Read())
                {
                    p = new Habil_Tipo();
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Habil_Tipo> TipoFinalidade()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(196,197,198,199)";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Habil_Tipo> lista = new List<Habil_Tipo>();
                Habil_Tipo p;

                while (Dr.Read())
                {
                    p = new Habil_Tipo();
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Habil_Tipo> TipoConsumidorFinal()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(182,183)";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Habil_Tipo> lista = new List<Habil_Tipo>();
                Habil_Tipo p;

                while (Dr.Read())
                {
                    p = new Habil_Tipo();
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Habil_Tipo> TipoModalidadeFrete()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(190,191,192,193,194,195)";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Habil_Tipo> lista = new List<Habil_Tipo>();
                Habil_Tipo p;

                while (Dr.Read())
                {
                    p = new Habil_Tipo();
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Habil_Tipo> SituacaoSolCompra()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(200,201,202,203,204,205)";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Habil_Tipo> lista = new List<Habil_Tipo>();
                Habil_Tipo p;

                while (Dr.Read())
                {
                    p = new Habil_Tipo();
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Habil_Tipo> SituacaoCotPreco()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [HABIL_TIPO] where CD_TIPO in(206,207,208)";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Habil_Tipo> lista = new List<Habil_Tipo>();
                Habil_Tipo p;

                while (Dr.Read())
                {
                    p = new Habil_Tipo();
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.CodigoReferencia = Convert.ToInt32(Dr["CD_REFERENCIA"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos tipos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
