using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class PlanoContasDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(PlanoContas p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [PLANO_DE_CONTAS] (DS_PLANO_CONTA, CD_REDUZIDO, CD_SITUACAO) values (@v1, @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoPlanoConta);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoReduzido);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);

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
                            throw new Exception("Erro ao Incluir PLANO DE CONTAS: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar PLANO DE CONTAS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(PlanoContas p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [PLANO_DE_CONTAS] set [DS_PLANO_CONTA] = @v2, [CD_REDUZIDO] = @v3, [CD_SITUACAO] = @v4  Where [CD_PLANO_CONTA] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoPlanoConta);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoPlanoConta);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoReduzido);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar PLANO DE CONTAS: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [PLANO_DE_CONTAS] Where [CD_PLANO_CONTA] = @v1", Con);

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
                            throw new Exception("Erro ao excluir PLANO DE CONTAS: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir PLANO DE CONTAS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public PlanoContas  PesquisarPlanoConta(long intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [PLANO_DE_CONTAS] Where CD_PLANO_CONTA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                PlanoContas  p = null;

                if (Dr.Read())
                {
                    p = new PlanoContas ();

                    p.CodigoPlanoConta= Convert.ToInt64(Dr["CD_PLANO_CONTA"]);
                    p.DescricaoPlanoConta = Convert.ToString(Dr["DS_PLANO_CONTA"]);
                    p.CodigoReduzido = Convert.ToString(Dr["CD_REDUZIDO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar PLANO DE CONTAS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<PlanoContas> ListarPlanoContas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [PLANO_DE_CONTAS]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<PlanoContas> lista = new List<PlanoContas>();

                while (Dr.Read())
                {
                    PlanoContas p = new PlanoContas();

                    p.CodigoPlanoConta = Convert.ToInt64(Dr["CD_PLANO_CONTA"]);
                    p.DescricaoPlanoConta = Convert.ToString(Dr["DS_PLANO_CONTA"]);
                    p.CodigoReduzido = Convert.ToString(Dr["CD_REDUZIDO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas PLANO DE CONTAS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterPlanoContas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [PLANO_DE_CONTAS]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoPlanoConta", typeof(Int64));
                dt.Columns.Add("DescricaoPlanoConta", typeof(string));
                dt.Columns.Add("CodigoReduzido", typeof(string));
                dt.Columns.Add("CodigoSituacao", typeof(Int32));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt64(Dr["CD_PLANO_CONTA"]), 
                                Convert.ToString(Dr["DS_PLANO_CONTA"]),
                                Convert.ToString(Dr["CD_REDUZIDO"]),
                                Convert.ToInt32(Dr["CD_SITUACAO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas PLANO DE CONTAS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
