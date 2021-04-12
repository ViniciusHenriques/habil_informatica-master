using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class CESTDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(CEST p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [CEST] (CD_CEST, DS_CEST, CD_NCM) values (@v1, @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoCEST.ToUpper());
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoCEST);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoNCM.ToUpper());
                p.CodigoIndice = Convert.ToInt32(Cmd.ExecuteScalar());


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
                            throw new Exception("Erro ao Incluir CEST: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar CEST: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(CEST p)
        {
            try
            {
                AbrirConexao();

                strSQL = "update [CEST] set [DS_CEST] = @v2, [CD_NCM] = @v3 Where [CD_CEST] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoCEST);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoCEST);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoNCM);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar CEST: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int intCodigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [CEST] Where [CD_index] = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", intCodigo );

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
                            throw new Exception("Erro ao excluir CEST: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir CEST: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public CEST PesquisarCEST(string strCEST)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [CEST] Where CD_CEST = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strCEST);

                Dr = Cmd.ExecuteReader();

                CEST p = null;

                if (Dr.Read())
                {
                    p = new CEST();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCEST = Convert.ToString(Dr["CD_CEST"]);
                    p.DescricaoCEST = Convert.ToString(Dr["DS_CEST"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar CEST: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public CEST PesquisarCESTIndice(int lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [CEST] Where CD_Index = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                CEST p = null;

                if (Dr.Read())
                {
                    p = new CEST();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCEST = Convert.ToString(Dr["CD_CEST"]);
                    p.DescricaoCEST = Convert.ToString(Dr["DS_CEST"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar CEST: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<CEST> ListarCESTs(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [CEST]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<CEST> lista = new List<CEST>();

                while (Dr.Read())
                {
                    CEST p = new CEST();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCEST = Convert.ToString(Dr["CD_CEST"]);
                    p.DescricaoCEST = Dr["DS_CEST"].ToString();
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas CEST: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterCESTs(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [CEST]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoIndice", typeof(int));
                dt.Columns.Add("CodigoCEST", typeof(string));
                dt.Columns.Add("DescricaoCEST", typeof(string));
                dt.Columns.Add("CodigoNCM", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_INDEX"]) , 
                        Convert.ToString(Dr["CD_CEST"]), 
                        Convert.ToString(Dr["DS_CEST"]), 
                        Convert.ToString(Dr["CD_NCM"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas CESTs: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
