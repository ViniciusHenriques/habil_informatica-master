using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class GrupoPessoaDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(GrupoPessoa p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into GRUPO_DE_PESSOA (DS_GPO_PESSOA, CD_SITUACAO, IN_GERAR_MATRICULA) values (@v1,@v2,@v3) SELECT SCOPE_IDENTITY()";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.DescricaoGrupoPessoa);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.GerarMatricula);

                //Cmd.ExecuteNonQuery();


                p.CodigoGrupoPessoa= Convert.ToInt32(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao Incluir Tipo de serviços: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Grupo de pessoa: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from [GRUPO_DE_PESSOA] Where [CD_GPO_PESSOA] = @v1;", Con);
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
                            throw new Exception("Erro ao excluir grupo de pessoas: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Grupo de pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(GrupoPessoa p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [GRUPO_DE_PESSOA] " +
                         "   set [DS_GPO_PESSOA] = @v2 " +
                         "   , [CD_SITUACAO] = @v3" +
                         "   , [IN_GERAR_MATRICULA] = @v4" +
                         " Where [CD_GPO_PESSOA] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoGrupoPessoa);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoGrupoPessoa);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.GerarMatricula);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Grupo de Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public List<GrupoPessoa> ListarGrupoPessoaCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [GRUPO_DE_PESSOA] WHERE [CD_GPO_PESSOA] IN ( SELECT [GRUPO_DE_PESSOA].CD_GPO_PESSOA FROM [GRUPO_DE_PESSOA]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                strSQL = strSQL + ")";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GrupoPessoa> lista = new List<GrupoPessoa>();

                while (Dr.Read())
                {
                    GrupoPessoa p = new GrupoPessoa();
                    p.CodigoGrupoPessoa = Convert.ToInt16(Dr["CD_GPO_PESSOA"]);
                    p.DescricaoGrupoPessoa = Convert.ToString(Dr["DS_GPO_PESSOA"]);
                    p.CodigoSituacao = Convert.ToInt16(Dr["CD_SITUACAO"]);

                    if(Dr["IN_GERAR_MATRICULA"].ToString() == "1")
                        p.GerarMatricula = true;
                    else
                        p.GerarMatricula = false;

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Grupos de pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public GrupoPessoa PesquisarGrupoPessoa(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [GRUPO_DE_PESSOA] Where CD_GPO_PESSOA= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                GrupoPessoa p = null;

                if (Dr.Read())
                {
                    p = new GrupoPessoa();

                    p.CodigoGrupoPessoa = Convert.ToInt32(Dr["CD_GPO_PESSOA"]);
                    p.DescricaoGrupoPessoa = Convert.ToString(Dr["DS_GPO_PESSOA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    if (Dr["IN_GERAR_MATRICULA"].ToString() == "1")
                        p.GerarMatricula = true;
                    else
                        p.GerarMatricula = false;

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar grupo de pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable ObterGrupoPessoa()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from GRUPO_DE_PESSOA  ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoGrupoPessoa", typeof(Int32));
                dt.Columns.Add("DescricaoGrupoPessoa", typeof(string));
                while (Dr.Read())
                    dt.Rows.Add(Dr["CD_GPO_PESSOA"], Dr["DS_GPO_PESSOA"]);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas grupos de pessoas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }

        }
    }
}
