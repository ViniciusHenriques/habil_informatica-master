using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class TipoAcessoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(TipoAcesso p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [Tipo_de_Acesso] (DS_TIPO_ACESSO) values (@v1)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoTipoAcesso);

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

                throw new Exception("Erro ao gravar Tipo de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(TipoAcesso p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [Tipo_de_Acesso] set [DS_TIPO_ACESSO] = @v2  Where [CD_TIPO_ACESSO] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoAcesso);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoTipoAcesso);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Tipo de Acesso: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [Tipo_de_Acesso] Where [CD_TIPO_ACESSO] = @v1", Con);

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
        public TipoAcesso  PesquisarTipoAcesso(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [Tipo_de_Acesso] Where CD_TIPO_ACESSO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                TipoAcesso  p = null;

                if (Dr.Read())
                {
                    p = new TipoAcesso ();

                    p.CodigoTipoAcesso= Convert.ToInt32(Dr["CD_TIPO_ACESSO"]);
                    p.DescricaoTipoAcesso = Convert.ToString(Dr["DS_TIPO_ACESSO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tipo de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<TipoAcesso> ListarTipoAcessos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Tipo_de_Acesso]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoAcesso> lista = new List<TipoAcesso>();

                while (Dr.Read())
                {
                    TipoAcesso p = new TipoAcesso();

                    p.CodigoTipoAcesso = Convert.ToInt32(Dr["CD_TIPO_ACESSO"]);
                    p.DescricaoTipoAcesso= Convert.ToString(Dr["DS_TIPO_ACESSO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Tipo de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterTipoAcessos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [Tipo_de_Acesso]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoTipoAcesso", typeof(Int32));
                dt.Columns.Add("DescricaoTipoAcesso", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_TipoAcesso"]), Convert.ToString(Dr["DS_TipoAcesso"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Tipo de Acessos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
