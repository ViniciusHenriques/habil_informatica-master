using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DAL.Persistence
{
    public class Habil_LogDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(Habil_Log p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [Habil_Log] (CD_TABELA_CAMPO, CD_OPR_LOG, CD_ESTACAO, CD_USUARIO, DESCRICAO, CD_IDENTIFICADOR, CHAVE) values (@v1, @v2, @v3, @v4, @v5, @v6,@v7); SELECT SCOPE_IDENTITY() ";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTabelaCampo);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoOperacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoEstacao);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v5", p.DescricaoLog);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoIdentificador);

                if (p.CodigoChave == null)
                    p.CodigoChave = "";

                Cmd.Parameters.AddWithValue("@v7", p.CodigoChave);

                
                p.CodigoLog = Convert.ToDouble(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao Incluir Log: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Log: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public Habil_Log PesquisarCodigo(double Codigo)
        {
            try
            {
                UsuarioDAL u = new  UsuarioDAL();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();

                AbrirConexao();
                strSQL = "Select * from [Habil_Log] Where CD_Log = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Habil_Log p = null;

                if (Dr.Read())
                {
                    p = new Habil_Log();

                    p.CodigoLog = Convert.ToDouble(Dr["CD_Log"]);
                    p.CodigoIdentificador = Convert.ToDouble(Dr["CD_IDENTIFICADOR"]);
                    p.CodigoTabelaCampo= Convert.ToDouble(Dr["CD_TABELA_CAMPO"]);
                    p.CodigoOperacao = Convert.ToInt32(Dr["CD_OPR_LOG"]);
                    p.CodigoEstacao = Convert.ToInt64(Dr["CD_ESTACAO"]);
                    p.CodigoUsuario = Convert.ToInt64(Dr["CD_USUARIO"]);
                    p.DataHora = Convert.ToDateTime(Dr["DT_GERACAO"]);
                    p.UsuarioNome = u.PesquisarUsuario(Convert.ToInt64(Dr["CD_USUARIO"])).NomeUsuario;
                    p.EstacaoNome = hedal.PesquisarCodigoHabil_Estacao(Convert.ToInt64(Dr["CD_ESTACAO"])).NomeEstacao;
                    p.CodigoChave = Dr["CHAVE"].ToString();
                    //Completa
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Habil_Log: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Habil_Log> ListarLogs(double CodigoIdentificador, int intQtdRegistros)
        {
            try
            {
                UsuarioDAL u = new UsuarioDAL();
                Habil_EstacaoDAL hedal = new Habil_EstacaoDAL();
                List<Habil_Log> lista = new List<Habil_Log>();

                AbrirConexao();
                strSQL = "Select ";

                if(intQtdRegistros != 0)
                {
                    strSQL += "top " + intQtdRegistros.ToString();
                }

                strSQL += " * from VW_LOG_DOCUMENTO Where CD_Identificador = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoIdentificador);

                Dr = Cmd.ExecuteReader();

                Habil_Log p;

                while (Dr.Read())
                {
                    p = new Habil_Log();

                    p.CodigoIdentificador = Convert.ToDouble(Dr["CD_IDENTIFICADOR"]);
                    p.DataHora = Convert.ToDateTime(Dr["DT_GERACAO"]);
                    p.UsuarioNome = Convert.ToString(Dr["NM_USUARIO"]);
                    p.EstacaoNome = Convert.ToString(Dr["DS_ESTACAO"]);
                    p.Cpl_DescricaoOperacao = Convert.ToString(Dr["DS_OPERACAO"]);

                    p.DescricaoLog = Dr["DESCRICAO"].ToString();
                    p.CodigoChave = Dr["CHAVE"].ToString();
                    //Completa
                    lista.Add(p);
                }

                return lista;

            }
           catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Habil_Log: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Habil_Log> ComparaDataTables(DataTable tbanterior, DataTable tbatual, 
                                                 double CodigoIdentificador, 
                                                 int CodUsuario, 
                                                 int CodMaquina,
                                                 int intOpLog,
                                                 string strNomeTabela)
        {
            DBTabelaDAL Rn_DBTabela = new DBTabelaDAL();
            List<Habil_Log> lista = new List<Habil_Log>();
            Habil_Log Rn_Log = new Habil_Log();


            foreach (DataRow linha in tbatual.Rows)
            {
                foreach (DataRow linhaAnterior in tbanterior.Rows)
                {

                    foreach (DataColumn item in tbatual.Columns)
                    {
                        string x = item.ColumnName;

                        if ((linha[x] == null) && (linhaAnterior[x] != null))
                        {
                            Rn_Log = new Habil_Log();
                            Rn_Log.CodigoIdentificador = CodigoIdentificador;
                            Rn_Log.CodigoTabelaCampo = Rn_DBTabela.PesquisaIDTabelaCampoSQL(strNomeTabela, item.ColumnName);
                            Rn_Log.CodigoOperacao = intOpLog; 
                            Rn_Log.CodigoUsuario = CodUsuario;
                            Rn_Log.CodigoEstacao = CodMaquina;
                            Rn_Log.CodigoChave = "Cód.Chave: " + CodigoIdentificador.ToString();
                            Rn_Log.DescricaoLog = " De:  " + linhaAnterior[x].ToString() + " Para: [Nulo] ";
                            lista.Add(Rn_Log);

                        }

                        if ((linha[x] != null) && (linhaAnterior[x] == null))
                        {
                            Rn_Log = new Habil_Log();
                            Rn_Log.CodigoIdentificador = CodigoIdentificador;
                            Rn_Log.CodigoTabelaCampo = Rn_DBTabela.PesquisaIDTabelaCampoSQL(strNomeTabela, item.ColumnName);
                            Rn_Log.CodigoOperacao = intOpLog; 
                            Rn_Log.CodigoUsuario = CodUsuario;
                            Rn_Log.CodigoEstacao = CodMaquina;
                            Rn_Log.CodigoChave = "Cód.Chave: " + CodigoIdentificador.ToString();
                            Rn_Log.DescricaoLog = " De: [Nulo]  Para:  " + linha[x].ToString();
                            lista.Add(Rn_Log);
                        }

                        if ((linha[x] != null) && (linhaAnterior[x] != null))
                        {
                            if (linha[x].ToString() != linhaAnterior[x].ToString())
                            {
                                Rn_Log = new Habil_Log();
                                Rn_Log.CodigoIdentificador = CodigoIdentificador;
                                Rn_Log.CodigoTabelaCampo = Rn_DBTabela.BuscaIDTabelaCampo(strNomeTabela, item.ColumnName);
                                Rn_Log.CodigoOperacao = intOpLog; 
                                Rn_Log.CodigoUsuario = CodUsuario;
                                Rn_Log.CodigoEstacao = CodMaquina;
                                Rn_Log.CodigoChave = "Cód.Chave: " + CodigoIdentificador.ToString();
                                Rn_Log.DescricaoLog = " De:  " + linhaAnterior[x].ToString() + " Para:  " + linha[x].ToString();
                                lista.Add(Rn_Log);
                            }

                        }
                    }
                }
            }
            

            return lista;
        }

        public List<Habil_Log> ComparaDataTablesRelacional(DataTable tbanterior, DataTable tbatual,
                                                          double CodigoIdentificador,
                                                          int CodUsuario,
                                                          int CodMaquina,
                                                          int intOpLogInclusao,
                                                          int intOpLogExclusao,
                                                          string strNomeTabela, string strCampoChave)
        {
            DBTabelaDAL Rn_DBTabela = new DBTabelaDAL();
            List<Habil_Log> lista = new List<Habil_Log>();
            Habil_Log Rn_Log = new Habil_Log();

            bool blnAchou = false;

            lista.Clear();

            foreach (DataRow linhaAnterior in tbanterior.Rows)
            {
                blnAchou = false;

                foreach (DataRow linhaAtual in tbatual.Rows)
                {
                    if (linhaAtual[strCampoChave].ToString() == linhaAnterior[strCampoChave].ToString())
                        blnAchou = true;
                }

                if (!blnAchou)
                {
                    Rn_Log = new Habil_Log();
                    Rn_Log.CodigoIdentificador = CodigoIdentificador;
                    Rn_Log.CodigoTabelaCampo = 0;
                    Rn_Log.CodigoOperacao = intOpLogExclusao;
                    Rn_Log.CodigoUsuario = CodUsuario;
                    Rn_Log.CodigoEstacao = CodMaquina;
                    Rn_Log.DescricaoLog = " Exclusão:  " + linhaAnterior[strCampoChave].ToString();
                    Rn_Log.CodigoChave = "Cód.Chave: " + CodigoIdentificador.ToString();
                    lista.Add(Rn_Log);
                }

            }

            foreach (DataRow linhaAtual in tbatual.Rows)
            {
                blnAchou = false;
                foreach (DataRow linhaAnterior in tbanterior.Rows)
                {
                    if (linhaAtual[strCampoChave].ToString() == linhaAnterior[strCampoChave].ToString())
                        blnAchou = true;
                }

                if (!blnAchou)
                {
                    Rn_Log = new Habil_Log();
                    Rn_Log.CodigoIdentificador = CodigoIdentificador;
                    Rn_Log.CodigoTabelaCampo = 0;
                    Rn_Log.CodigoOperacao = intOpLogInclusao;
                    Rn_Log.CodigoUsuario = CodUsuario;
                    Rn_Log.CodigoEstacao = CodMaquina;
                    Rn_Log.DescricaoLog = " Inclusão:  " + linhaAtual[strCampoChave].ToString();
                    Rn_Log.CodigoChave = "Cód.Chave: " + CodigoIdentificador.ToString();
                    lista.Add(Rn_Log);
                }

            }



            return lista;
        }

        public void GerandoArquivoLog(string strDescrição)
        {
            try
            {
                DateTime data = DateTime.Now;

                string CaminhoArquivoLog = "";
                CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\Log\\Log-HABIL-" + data.ToString("dd-MM-yyyy") + ".txt";

                if (!System.IO.File.Exists(CaminhoArquivoLog))
                {
                    FileStream file = new FileStream(CaminhoArquivoLog, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(file);
                    bw.Close();
                }

                string nomeArquivo = CaminhoArquivoLog;
                System.IO.TextWriter arquivo = System.IO.File.AppendText(nomeArquivo);

                // Agora é só sair escrevendo
                arquivo.WriteLine(data.ToString("HH:mm:ss") + " - " + strDescrição);

                arquivo.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public void GerandoArquivoLogPeloServico(string strDescrição)
        {
            try
            {
                DateTime data = DateTime.Now;

                string CaminhoArquivoLog = "";
                CaminhoArquivoLog = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\..\\..\\..\\..\\Modulos\\Log\\Log-" + data.ToString("dd-MM-yyyy") + ".txt";
                
                if (!System.IO.File.Exists(CaminhoArquivoLog))
                {
                    FileStream file = new FileStream(CaminhoArquivoLog, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(file);
                    bw.Close();
                }

                string nomeArquivo = CaminhoArquivoLog;
                System.IO.TextWriter arquivo = System.IO.File.AppendText(nomeArquivo);

                // Agora é só sair escrevendo
                arquivo.WriteLine(data.ToString("HH:mm:ss") + " - " + strDescrição);

                arquivo.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


		public List<Habil_Log> ComparaDataTablesRelacionalProduto_do_Documento(DataTable tbanterior, 
														  DataTable tbatual,
														  double CodigoIdentificador,
														  int CodUsuario,
														  int CodMaquina,
														  int intOpLogInclusao,
														  int intOpLogExclusao,
														  int intOpLogAlteracao,
														  string strNomeTabela, string strCampoChave, string strCampoChave2)
		{
			DBTabelaDAL Rn_DBTabela = new DBTabelaDAL();
			List<Habil_Log> lista = new List<Habil_Log>();
			Habil_Log Rn_Log = new Habil_Log();

			bool blnAchou = false;

            try
            {
                lista.Clear();

                foreach (DataRow linhaAnterior in tbanterior.Rows)
                {
                    blnAchou = false;

                    foreach (DataRow linhaAtual in tbatual.Rows)
                    {
                        if (linhaAtual[strCampoChave2].ToString() == linhaAnterior[strCampoChave2].ToString())
                            blnAchou = true;
                    }

                    if (!blnAchou)
                    {
                        Rn_Log = new Habil_Log();
                        Rn_Log.CodigoIdentificador = CodigoIdentificador;
                        Rn_Log.CodigoTabelaCampo = 0;
                        Rn_Log.CodigoOperacao = intOpLogExclusao;
                        Rn_Log.CodigoUsuario = CodUsuario;
                        Rn_Log.CodigoEstacao = CodMaquina;
                        Rn_Log.DescricaoLog = " Exclusão do Produto: " + linhaAnterior["CD_PRODUTO"].ToString() + " - " + linhaAnterior["DS_PRODUTO"].ToString();
                        Rn_Log.CodigoChave = "Cód.Chave: " + linhaAnterior["CD_PROD_DOCUMENTO"].ToString();
                        lista.Add(Rn_Log);
                    }

                }

                foreach (DataRow linhaAtual in tbatual.Rows)
                {
                    blnAchou = false;
                    foreach (DataRow linhaAnterior in tbanterior.Rows)
                    {
                        if (linhaAtual[strCampoChave2].ToString() == linhaAnterior[strCampoChave2].ToString())
                            blnAchou = true;
                    }

                    if (!blnAchou)
                    {
                        Rn_Log = new Habil_Log();
                        Rn_Log.CodigoIdentificador = CodigoIdentificador;
                        Rn_Log.CodigoTabelaCampo = 0;
                        Rn_Log.CodigoOperacao = intOpLogInclusao;
                        Rn_Log.CodigoUsuario = CodUsuario;
                        Rn_Log.CodigoEstacao = CodMaquina;
                        Rn_Log.DescricaoLog = " Inclusão do Produto: " + linhaAtual["CD_PRODUTO"].ToString() + " - " + linhaAtual["DS_PRODUTO"].ToString();
                        Rn_Log.CodigoChave = "Cód.Chave: " + linhaAtual["CD_PROD_DOCUMENTO"].ToString();
                        lista.Add(Rn_Log);
                    }

                }

                //	if (tbatual.Rows.Count != tbanterior.Rows.Count)
                //	return lista;

                foreach (DataRow linha in tbatual.Rows)
                {
                    foreach (DataRow linhaAnterior in tbanterior.Rows)
                    {

                        if (linha["CD_PROD_DOCUMENTO"].ToString() == linhaAnterior["CD_PROD_DOCUMENTO"].ToString())
                        {
                            foreach (DataColumn item in tbatual.Columns)
                            {
                                string x = item.ColumnName;

                                if ((linha[x] != null) && (linhaAnterior[x] != null))
                                {
                                    if ((x.ToUpper() == "CD_SITUACAO") && (linha[x].ToString() == "134"))
                                    {
                                        Rn_Log = new Habil_Log();
                                        Rn_Log.CodigoIdentificador = CodigoIdentificador;
                                        Rn_Log.CodigoTabelaCampo = Rn_DBTabela.BuscaIDTabelaCampo(strNomeTabela, item.ColumnName);
                                        Rn_Log.CodigoOperacao = intOpLogAlteracao;
                                        Rn_Log.CodigoUsuario = CodUsuario;
                                        Rn_Log.CodigoEstacao = CodMaquina;
                                        Rn_Log.DescricaoLog = " Exclusão do Item  " + linha["CD_PRODUTO"].ToString() + " - " + linha["DS_PRODUTO"].ToString();
                                        Rn_Log.CodigoChave = "Cód.Chave: " + linha["CD_PROD_DOCUMENTO"].ToString();
                                        lista.Add(Rn_Log);

                                    }
                                    else
                                    {
                                        if (linha[x].ToString() != linhaAnterior[x].ToString())
                                        {
                                            Rn_Log = new Habil_Log();
                                            Rn_Log.CodigoIdentificador = CodigoIdentificador;
                                            Rn_Log.CodigoTabelaCampo = Rn_DBTabela.BuscaIDTabelaCampo(strNomeTabela, item.ColumnName);
                                            Rn_Log.CodigoOperacao = intOpLogAlteracao;
                                            Rn_Log.CodigoUsuario = CodUsuario;
                                            Rn_Log.CodigoEstacao = CodMaquina;
                                            Rn_Log.DescricaoLog = " - De:  " + linhaAnterior[x].ToString() + " Para:  " + linha[x].ToString();
                                            Rn_Log.CodigoChave = "Cód.Produto: " + linha["CD_PRODUTO"].ToString();
                                            lista.Add(Rn_Log);
                                        }
                                    }

                                }
                            }
                        }
                    }


                }



                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception (ex.Message);
            }

		}

		public List<Habil_Log> ComparaDataTablesRelacionalPessoa_do_Documento(DataTable tbanterior,
														  DataTable tbatual,
														  double CodigoIdentificador,
														  int CodUsuario,
														  int CodMaquina,
														  int intOpLogInclusao,
														  int intOpLogExclusao,
														  int intOpLogAlteracao,
														  string strNomeTabela, string strCampoChave, string strCampoChave2)
		{
			DBTabelaDAL Rn_DBTabela = new DBTabelaDAL();
			List<Habil_Log> lista = new List<Habil_Log>();
			Habil_Log Rn_Log = new Habil_Log();

			bool blnAchou = false;

			lista.Clear();

			foreach (DataRow linhaAnterior in tbanterior.Rows)
			{
				blnAchou = false;

				foreach (DataRow linhaAtual in tbatual.Rows)
				{
					if ((linhaAtual[strCampoChave2].ToString() == linhaAnterior[strCampoChave2].ToString()) && (linhaAtual["TP_PESSOA"] == linhaAnterior["TP_PESSOA"]))
                        blnAchou = true;
				}

				if (!blnAchou)
				{
					Rn_Log = new Habil_Log();
					Rn_Log.CodigoIdentificador = CodigoIdentificador;
					Rn_Log.CodigoTabelaCampo = 0;
					Rn_Log.CodigoOperacao = intOpLogExclusao;
					Rn_Log.CodigoUsuario = CodUsuario;
					Rn_Log.CodigoEstacao = CodMaquina;
					Rn_Log.DescricaoLog = "Exclusão da Pessoa: " + linhaAnterior["CD_PESSOA"].ToString() + " - " + linhaAnterior["RAZ_SOCIAL"].ToString();
                    Rn_Log.CodigoChave = "Cód.Pessoa: " + linhaAnterior["CD_PESSOA"].ToString();
                    lista.Add(Rn_Log);
				}

			}

			foreach (DataRow linhaAtual in tbatual.Rows)
			{
				blnAchou = false;
				foreach (DataRow linhaAnterior in tbanterior.Rows)
				{
                    if ((linhaAtual[strCampoChave2].ToString() == linhaAnterior[strCampoChave2].ToString()) && (linhaAtual["TP_PESSOA"] == linhaAnterior["TP_PESSOA"]))
						blnAchou = true;
				}

				if (!blnAchou)
				{
					Rn_Log = new Habil_Log();
					Rn_Log.CodigoIdentificador = CodigoIdentificador;
					Rn_Log.CodigoTabelaCampo = 0;
					Rn_Log.CodigoOperacao = intOpLogInclusao;
					Rn_Log.CodigoUsuario = CodUsuario;
					Rn_Log.CodigoEstacao = CodMaquina;
					Rn_Log.DescricaoLog = " Inclusão da Pessoa: " + linhaAtual["CD_PESSOA"].ToString() + " - " + linhaAtual["RAZ_SOCIAL"].ToString();
                    Rn_Log.CodigoChave = "Cód.Pessoa: " + linhaAtual["CD_PESSOA"].ToString();
                    lista.Add(Rn_Log);
				}

			}

			//	if (tbatual.Rows.Count != tbanterior.Rows.Count)
			//	return lista;

			foreach (DataRow linha in tbatual.Rows)
			{
				foreach (DataRow linhaAnterior in tbanterior.Rows)
				{

					if (linha["TP_PESSOA"].ToString() == linhaAnterior["TP_PESSOA"].ToString())
					{
						foreach (DataColumn item in tbatual.Columns)
						{
							string x = item.ColumnName;

							if ((linha[x] != null) && (linhaAnterior[x] != null))
							{
								if (linha[x].ToString() != linhaAnterior[x].ToString())
								{
									Rn_Log = new Habil_Log();
									Rn_Log.CodigoIdentificador = CodigoIdentificador;
									Rn_Log.CodigoTabelaCampo = Rn_DBTabela.BuscaIDTabelaCampo(strNomeTabela, item.ColumnName);
									Rn_Log.CodigoOperacao = intOpLogAlteracao;
									Rn_Log.CodigoUsuario = CodUsuario;
									Rn_Log.CodigoEstacao = CodMaquina;
									Rn_Log.DescricaoLog = " De:  " + linhaAnterior[x].ToString() + " Para:  " + linha[x].ToString();
                                    Rn_Log.CodigoChave = "Cód.Pessoa: " + linha["CD_PESSOA"].ToString();
                                    lista.Add(Rn_Log);
								}
							}
						}
					}
				}


			}



			return lista;
		}

	}
}
