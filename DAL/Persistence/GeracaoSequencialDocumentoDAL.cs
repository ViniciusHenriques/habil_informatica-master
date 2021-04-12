using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
namespace DAL.Persistence
{
    public class GeracaoSequencialDocumentoDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(GeracaoSequencialDocumento p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into GERADOR_SEQUENCIAL_DOCUMENTO (CD_TIPO_DOCUMENTO," +
                                                                    "CD_EMPRESA," +
                                                                    "SR_CONTEUDO," +
                                                                    "SR_NUMERO," +
                                                                    "VALIDADE," +
                                                                    "NR_INICIAL," +
                                                                    "NOME," +
                                                                    "DS_GER_SEQ_DOC," +
                                                                    "CD_SITUACAO)" +
                                                     " values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9) SELECT SCOPE_IDENTITY()";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v3", p.SerieConteudo);
                Cmd.Parameters.AddWithValue("@v4", p.SerieNumero);
                Cmd.Parameters.AddWithValue("@v5", p.Validade);
                Cmd.Parameters.AddWithValue("@v6", p.NumeroInicial);
                Cmd.Parameters.AddWithValue("@v7", p.Nome);
                Cmd.Parameters.AddWithValue("@v8", p.Descricao.ToUpper());
                Cmd.Parameters.AddWithValue("@v9", p.CodigoSituacao);
                //Cmd.ExecuteNonQuery();


                p.CodigoGeracaoSequencial = Convert.ToInt32(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao Incluir GERACAO SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar GERACAO SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }


        }
        public void CriarTabelaGeracaoSequencial(GeracaoSequencialDocumento p)
        {
            try
            {
                AbrirConexao();

                string strCod = p.CodigoGeracaoSequencial.ToString();

                string strSQL1 = "CONSTRAINT [PK_HABIL_ERP_0000] ";
                strSQL1 = strSQL1.Replace("0000", strCod.PadLeft(4, '0'));

                strSQL = "CREATE TABLE [dbo].[HABIL_ERP_0000] ([NR_SEQUENCIAL] [numeric](18, 0) IDENTITY(";

                strSQL = strSQL.Replace("0000", strCod.PadLeft(4, '0'));

                strSQL = strSQL + Convert.ToString(p.NumeroInicial);

                strSQL = strSQL + ",1) NOT NULL, " +
                                         "[CD_GER_SEQ_DOC] [int] null,  " +
                                         "[CD_USUARIO] [int] NULL, " +
                                         "[CD_ESTACAO]  [int] NULL, "+
                                         "[CD_USADO] [smallint] NULL, ";


               strSQL += strSQL1;
                                         
                strSQL +=  "PRIMARY KEY CLUSTERED ([NR_SEQUENCIAL] ASC ) " +
                                         "WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) " +
                                         "ON [PRIMARY] ) ON [PRIMARY];";
                
                

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoGeracaoSequencial);
                Cmd.Parameters.AddWithValue("@v2", p.NumeroInicial);


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
                            throw new Exception("Erro ao Incluir GERACAO SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar GERACAO SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }


        }
        public void Atualizar(GeracaoSequencialDocumento p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update GERADOR_SEQUENCIAL_DOCUMENTO set " +
                                 "CD_TIPO_DOCUMENTO = @v2, " +
                                 "CD_EMPRESA = @v3, " +
                                 "SR_CONTEUDO = @v4, " +
                                 "SR_NUMERO = @v5," +
                                 "VALIDADE = @v6, " +
                                 "NR_INICIAL = @v7, " +
                                 "NOME = @v8, " +
                                 "DS_GER_SEQ_DOC = @v9," +
                                 "CD_SITUACAO = @v10" +
                         " Where [CD_GER_SEQ_DOC] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoGeracaoSequencial);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoTipoDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v4", p.SerieConteudo);
                Cmd.Parameters.AddWithValue("@v5", p.SerieNumero);
                Cmd.Parameters.AddWithValue("@v6", p.Validade);
                Cmd.Parameters.AddWithValue("@v7", p.NumeroInicial);
                Cmd.Parameters.AddWithValue("@v8", p.Nome);
                Cmd.Parameters.AddWithValue("@v9", p.Descricao.ToUpper());
                Cmd.Parameters.AddWithValue("@v10", p.CodigoSituacao);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar GERACAO SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(Int32 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from GERADOR_SEQUENCIAL_DOCUMENTO Where [CD_GER_SEQ_DOC] = @v1", Con);
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
                            throw new Exception("Erro ao excluir GERACAO SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir GERACAO SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public List<GeracaoSequencialDocumento> ListarGeracaoSequencialDocumentoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from  GERADOR_SEQUENCIAL_DOCUMENTO";
                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GeracaoSequencialDocumento> lista = new List<GeracaoSequencialDocumento>();

                while (Dr.Read())
                {
                    GeracaoSequencialDocumento p = new GeracaoSequencialDocumento();
                    p.CodigoGeracaoSequencial = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.SerieConteudo = Convert.ToString(Dr["SR_CONTEUDO"]);
                    p.SerieNumero = Convert.ToInt64(Dr["SR_NUMERO"]);
                    p.Validade = Convert.ToDateTime(Dr["VALIDADE"]);
                    p.NumeroInicial = Convert.ToDecimal(Dr["NR_INICIAL"]);
                    p.Descricao = Convert.ToString(Dr["DS_GER_SEQ_DOC"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos GERACAO SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public GeracaoSequencialDocumento PesquisarGeradorSequencial(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [GERADOR_SEQUENCIAL_DOCUMENTO] Where CD_GER_SEQ_DOC = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                GeracaoSequencialDocumento p = new GeracaoSequencialDocumento();

                if (Dr.Read())
                {
                    

                    p.CodigoGeracaoSequencial = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.NumeroInicial = Convert.ToDecimal(Dr["NR_INICIAL"]);
                    p.Validade = Convert.ToDateTime(Dr["VALIDADE"]);
                    p.SerieConteudo = Convert.ToString(Dr["SR_CONTEUDO"]);
                    p.Nome = Convert.ToString(Dr["NOME"]);
                    p.SerieNumero = Convert.ToInt64(Dr["SR_NUMERO"]);
                    p.Descricao = Convert.ToString(Dr["DS_GER_SEQ_DOC"]);



                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar GERADOR SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<GeracaoSequencialDocumento> ListarGeracaoSequencial(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [GERADOR_SEQUENCIAL_DOCUMENTO] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GeracaoSequencialDocumento> lista = new List<GeracaoSequencialDocumento>();

                while (Dr.Read())
                {
                    GeracaoSequencialDocumento p = new GeracaoSequencialDocumento();

                    p.CodigoGeracaoSequencial = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.SerieConteudo = Convert.ToString(Dr["SR_CONTEUDO"]);
                    p.SerieNumero = Convert.ToInt64(Dr["SR_NUMERO"]);
                    p.Validade = Convert.ToDateTime(Dr["VALIDADE"]);
                    p.NumeroInicial = Convert.ToDecimal(Dr["NR_INICIAL"]);
                    p.Descricao = Convert.ToString(Dr["DS_GER_SEQ_DOC"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Nome = Convert.ToString(Dr["NOME"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos GERADORES SEQUENCIAIS DE DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }


    }
}
