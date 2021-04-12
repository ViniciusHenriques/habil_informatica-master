using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class GpoTribPessoaDAL: Conexao
    {
        protected string strSQL = "";

        public void Inserir(GpoTribPessoa p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [GRUPO_TRIB_PESSOA] (DS_GPO_TRIB_PESSOA, IN_ICMS, IN_IPI) values (@v1, @v2, @v3)";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoGpoTribPessoa);
                Cmd.Parameters.AddWithValue("@v2", p.Icms);
                Cmd.Parameters.AddWithValue("@v3", p.IPI);

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
                            throw new Exception("Erro ao Incluir Grupo Tributação da Pessoa: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Grupo Tributação da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(GpoTribPessoa p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [GRUPO_TRIB_Pessoa] set [DS_GPO_TRIB_Pessoa] = @v2, [IN_ICMS] = @v3, [IN_IPI] = @v4  Where [CD_GPO_TRIB_PESSOA] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoGpoTribPessoa);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoGpoTribPessoa);
                Cmd.Parameters.AddWithValue("@v3", p.Icms);
                Cmd.Parameters.AddWithValue("@v4", p.IPI);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar GpoTribPessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }

        public void Excluir(long Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [GRUPO_TRIB_Pessoa] Where [CD_GPO_TRIB_Pessoa] = @v1", Con);

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
                            throw new Exception("Erro ao excluir GpoTribPessoa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir GpoTribPessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public GpoTribPessoa  PesquisarGpoTribPessoa(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [GRUPO_TRIB_PESSOA] Where CD_GPO_TRIB_PESSOA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                GpoTribPessoa  p = null;

                if (Dr.Read())
                {
                    p = new GpoTribPessoa ();

                    p.CodigoGpoTribPessoa= Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
                    p.DescricaoGpoTribPessoa= Convert.ToString(Dr["DS_GPO_TRIB_PESSOA"]);
                    p.Icms= Convert.ToInt32(Dr["IN_ICMS"]);
                    p.IPI = Convert.ToInt32(Dr["IN_IPI"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar GpoTribPessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<GpoTribPessoa> ListarGpoTribPessoas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [GRUPO_TRIB_PESSOA]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GpoTribPessoa> lista = new List<GpoTribPessoa>();

                while (Dr.Read())
                {
                    GpoTribPessoa p = new GpoTribPessoa();

                    p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
                    p.DescricaoGpoTribPessoa = Convert.ToString(Dr["DS_GPO_TRIB_PESSOA"]);
                    p.Icms = Convert.ToInt32(Dr["IN_ICMS"]);
                    p.IPI = Convert.ToInt32(Dr["IN_IPI"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas RegFisIcmsGpoTribPessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterGpoTribPessoas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [GRUPO_TRIB_PESSOA]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoGpoTribPessoa", typeof(Int32));
                dt.Columns.Add("DescricaoGpoTribPessoa", typeof(string));
                dt.Columns.Add("Icms", typeof(Int32));
                dt.Columns.Add("IPI", typeof(Int32));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]), Convert.ToString(Dr["DS_GPO_TRIB_PESSOA"]), Convert.ToInt32(Dr["IN_ICMS"]), Convert.ToInt32(Dr["IN_IPI"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas GpoTribPessoas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<GpoTribPessoa> ListarGpoTribPessoasCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [GRUPO_TRIB_PESSOA]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GpoTribPessoa> lista = new List<GpoTribPessoa>();

                while (Dr.Read())
                {
                    GpoTribPessoa p = new GpoTribPessoa();

                    p.CodigoGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]);
                    p.DescricaoGpoTribPessoa = Convert.ToString(Dr["DS_GPO_TRIB_PESSOA"]);
                    p.Icms = Convert.ToInt32(Dr["IN_ICMS"]);
                    p.IPI = Convert.ToInt32(Dr["IN_IPI"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas GpoTribPessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
    }
}
