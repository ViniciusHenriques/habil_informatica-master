using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
namespace DAL.Persistence
{
    public class ContaCorrenteDAL:Conexao
    {
        protected string strSQL = "";

        public bool Inserir(ContaCorrente p)
        {

            try
            {
                AbrirConexao();

                strSQL = "insert into CONTA_CORRENTE( DS_CTA_CORRENTE," +
                                                     "CD_SITUACAO," +
                                                     "CD_BANCO," +
                                                     "CD_PESSOA ) values (@v1,@v2,@v3,@v4);";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.DescricaoContaCorrente);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoBanco);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoPessoa);

                Cmd.ExecuteNonQuery();


                return true;
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
                            throw new Exception("Erro ao Incluir Conta corrente: " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar conta corrente: " + ex.Message.ToString());

            }
            finally
            {


                FecharConexao();

            }
        }

        public List<ContaCorrente> ListarContaCorrente(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "Select * from CONTA_CORRENTE";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ContaCorrente> lista = new List<ContaCorrente>();

                while (Dr.Read())
                {
                    ContaCorrente p = new ContaCorrente();
                    p.CodigoContaCorrente = Convert.ToInt32(Dr["CD_CTA_CORRENTE"]);
                    p.DescricaoContaCorrente = Dr["DS_CTA_CORRENTE"].ToString();
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoBanco = Convert.ToInt32(Dr["CD_BANCO"]);

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar  CONTA CORRENTE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public ContaCorrente PesquisarContaCorrente(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [CONTA_CORRENTE] Where CD_CTA_CORRENTE= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                ContaCorrente p = null;

                if (Dr.Read())
                {
                    p = new ContaCorrente();

                    p.CodigoContaCorrente = Convert.ToInt32(Dr["CD_CTA_CORRENTE"]);
                    p.DescricaoContaCorrente = Convert.ToString(Dr["DS_CTA_CORRENTE"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoBanco = Convert.ToInt32(Dr["CD_BANCO"]);


                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar conta corrente: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ContaCorrente> ListarContaCorrenteCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [CONTA_CORRENTE] WHERE [CD_CTA_CORRENTE] IN ( SELECT [CONTA_CORRENTE].CD_CTA_CORRENTE FROM [CONTA_CORRENTE]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                strSQL = strSQL + ")";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ContaCorrente> lista = new List<ContaCorrente>();

                while (Dr.Read())
                {
                    ContaCorrente p = new ContaCorrente();
                    p.CodigoContaCorrente = Convert.ToInt16(Dr["CD_CTA_CORRENTE"]);
                    p.DescricaoContaCorrente = Convert.ToString(Dr["DS_CTA_CORRENTE"]);
                    


                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos bancos: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from CONTA_CORRENTE Where CD_CTA_CORRENTE= @v1", Con);
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
                            throw new Exception("Erro ao excluir CONTA CORRENTE " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir CONTA CORRENTE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(ContaCorrente p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [CONTA_CORRENTE] " +
                         "   set [DS_CTA_CORRENTE] = @v2, " +
                         "[CD_SITUACAO] = @v3," +
                         "[CD_PESSOA] = @v4," +
                         "[CD_BANCO] = @v5" +
                         " Where [CD_CTA_CORRENTE] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoContaCorrente);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoContaCorrente);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoBanco);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar cONTA CORRENTE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
    }
}
