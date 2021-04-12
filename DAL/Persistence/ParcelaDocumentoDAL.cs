using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class ParcelaDocumentoDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(decimal CodDoc, List<ParcelaDocumento> listaParcela)
        {
            try
            {
                ExcluirTodos(CodDoc);
                AbrirConexao();
                foreach (ParcelaDocumento p in listaParcela)
                {
                    strSQL = "insert into PARCELA_DO_DOCUMENTO (CD_DOCUMENTO, CD_PARCELA, DT_VENCIMENTO, VL_PARCELA, CD_DOC_PAGAMENTO,DG_DOCUMENTO) values (@v1,@v2,@v3,@v4,@v5,@v6)";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1", CodDoc);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoParcela);
                    Cmd.Parameters.AddWithValue("@v3", p.DataVencimento);
                    Cmd.Parameters.AddWithValue("@v4", p.ValorParcela);
                    Cmd.Parameters.AddWithValue("@v5", p.CodigoDocumentoPagamento);
                    Cmd.Parameters.AddWithValue("@v6", p.DGNumeroDocumento);

                    Cmd.ExecuteNonQuery();
                }
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
                            throw new Exception("Erro ao Incluir parcela do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar parcela do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(ParcelaDocumento p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [PARCELA_DO_DOCUMENTO] set" +
                         " CD_PARCELA = @v2" +
                         ", DT_VENCIMENTO = @v3" +
                         ", CD_DOC_PAGAMENTO = @v5" +
                         ", DG_DOCUMENTO = @v6" +
                         " Where [CD_DOCUMENTO] = @v1 and CD_PARCELA = @v2";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoParcela);
                Cmd.Parameters.AddWithValue("@v3", p.DataVencimento);
                Cmd.Parameters.AddWithValue("@v4", p.ValorParcela);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoDocumentoPagamento);
                Cmd.Parameters.AddWithValue("@v6", p.DGNumeroDocumento);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar parcela do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
       
        public ParcelaDocumento PesquisarItemTipoServico(decimal Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [PARCELA_DO_DOCUMENTO] Where CD_DOCUMENTO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                ParcelaDocumento p = null;

                if (Dr.Read())
                {
                    p = new ParcelaDocumento();

                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoParcela = Convert.ToInt32(Dr["CD_PARCELA"]);
                    p.DataVencimento = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.ValorParcela = Convert.ToDecimal(Dr["VL_PARCELA"]);
                    p.CodigoDocumentoPagamento = Convert.ToDecimal(Dr["CD_DOC_PAGAMENTO"]);
                    p.DGNumeroDocumento = Dr["DG_DOCUMENTO"].ToString();

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar parcela do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ParcelaDocumento> ObterParcelaDocumento(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from PARCELA_DO_DOCUMENTO Where CD_DOCUMENTO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Dr = Cmd.ExecuteReader();
                List<ParcelaDocumento> lista = new List<ParcelaDocumento>();

                while (Dr.Read())
                {
                    ParcelaDocumento p = new ParcelaDocumento();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoParcela = Convert.ToInt32(Dr["CD_PARCELA"]);
                    p.DataVencimento = Convert.ToDateTime(Dr["DT_VENCIMENTO"]);
                    p.ValorParcela = Convert.ToDecimal(Dr["VL_PARCELA"]);
                    p.CodigoDocumentoPagamento = Convert.ToDecimal(Dr["CD_DOC_PAGAMENTO"]);
                    p.DGNumeroDocumento = Dr["DG_DOCUMENTO"].ToString() ;

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter PARCELA DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        

        public void ExcluirTodos(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from PARCELA_DO_DOCUMENTO Where CD_DOCUMENTO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
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
                            throw new Exception("Erro ao excluir parcela do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir parcela do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelParcelaDocumento(decimal CodigoDocumento)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "";
                strSQL = "SELECT * FROM PARCELA_DO_DOCUMENTO where CD_DOCUMENTO =" + CodigoDocumento;
                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
    }
}
