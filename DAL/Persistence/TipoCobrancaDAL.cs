using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class TipoCobrancaDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(TipoCobranca p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [Tipo_de_Cobranca] (DS_TIPO_COBRANCA, CD_SITUACAO) values (@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);

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
                            throw new Exception("Erro ao Incluir Tipo de Acesso: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Tipo de Cobrança: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(TipoCobranca p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [Tipo_de_Cobranca] set [DS_TIPO_COBRANCA] = @v2, [CD_SITUACAO] = @v3   Where [CD_TIPO_COBRANCA] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoTipoCobranca);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Tipo De Cobrança: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [Tipo_de_Cobranca] Where [CD_TIPO_COBRANCA] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Tipo de Acesso: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Tipo de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public TipoCobranca  PesquisarTipoCobranca(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Tipo_de_Cobranca] Where CD_TIPO_COBRANCA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                TipoCobranca  p = null;

                if (Dr.Read())
                {
                    p = new TipoCobranca ();

                    p.CodigoTipoCobranca= Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.DescricaoTipoCobranca = Convert.ToString(Dr["DS_TIPO_COBRANCA"]);
                    p.CodigoSituacao= Convert.ToInt32(Dr["CD_SITUACAO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tipo de Cobrança: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<TipoCobranca> ListarTipoCobrancas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Tipo_de_Cobranca]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoCobranca> lista = new List<TipoCobranca>();

                while (Dr.Read())
                {
                    TipoCobranca p = new TipoCobranca();

                    p.CodigoTipoCobranca = Convert.ToInt32(Dr["CD_TIPO_COBRANCA"]);
                    p.DescricaoTipoCobranca = Convert.ToString(Dr["DS_TIPO_COBRANCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Tipo de Cobranca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterTipoCobrancas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Tipo_de_Cobranca]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoTipoCobranca", typeof(Int32));
                dt.Columns.Add("DescricaoTipoCobranca", typeof(string));
                dt.Columns.Add("CodigoSituacao", typeof(Int32));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_Tipo_Cobranca"]), Convert.ToString(Dr["DS_Tipo_Cobranca"]), Convert.ToInt32(Dr["CD_Situacao"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Tipo de Cobranca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
