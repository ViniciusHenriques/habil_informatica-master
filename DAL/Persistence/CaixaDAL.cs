using System;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;
  
namespace DAL.Persistence
{
    public class CaixaDAL : Conexao
    {

        public void Abertura_de_Caixa(Caixa  p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("insert into CAIXA  (CD_CAIXA, DT_CAIXA, CD_USU_ABERTURA, TX_MAQUINA) values (@v1, GETDATE(), @v2, @v3)", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoCaixa);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoFunAbertura);
                Cmd.Parameters.AddWithValue("@v3", p.NomeMaquina);
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
                            throw new Exception("Erro ao gravar Caixa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception ("Erro ao gravar Caixa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        /******************************************************************************************************************************************/

        public Caixa Obtem_Caixa_Aberto()
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select * from [CAIXA] Where TX_MAQUINA = '" + Environment.MachineName + "' AND DT_FECHAMENTO IS NULL";
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                Caixa p = null;

                if (Dr.Read())
                {
                    p = new Caixa();

                    p.CodigoCaixa = Convert.ToInt32(Dr["CD_CAIXA"]);
                    p.DataAbertura = Convert.ToDateTime(Dr["DT_CAIXA"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obtem_Caixa_Aberto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        /******************************************************************************************************************************************/

        public void Encerramento_de_Caixa(Caixa p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update CAIXA set DT_FECHAMENTO = GETDATE(), CD_USU_FECHAMENTO = @v2 WHERE CD_CAIXA = @v1 and DT_FECHAMENTO IS NULL ", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoCaixa);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoFunFechamento);
                Cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                         throw new Exception("Erro ao gravar Caixa: " + ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Encerramento_de_Caixa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        /******************************************************************************************************************************************/

        public bool Existe_Caixa_Aberto(int CodCaixa)
        {
            try
            {
                    AbrirConexao();
                    string strSQL = "Select * from [CAIXA] Where CD_CAIXA = " + CodCaixa.ToString() + " AND DT_FECHAMENTO IS NULL";
                    Cmd = new SqlCommand(strSQL, Con);

                    Dr = Cmd.ExecuteReader();

                    if (Dr.Read())

                        return true;
                    else
                        return false;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Existe_Caixa_Aberto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        /******************************************************************************************************************************************/

    }
}
