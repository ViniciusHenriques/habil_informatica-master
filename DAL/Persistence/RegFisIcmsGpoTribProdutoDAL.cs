using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class RegFisIcmsGpoTribProdutoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(RegFisIcmsGpoTribProduto p)
        {
            try
            {
                Excluir(p.CodigoRegFisIcms, p.CodGpoTribProduto);

                AbrirConexao();


                strSQL = "insert into [REGRA_FISCAL_ICMS_GRUPO_Produto] (CD_REGRA_FISCAL_ICMS, CD_GPO_TRIB_Produto) values (@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoRegFisIcms);
                Cmd.Parameters.AddWithValue("@v2", p.CodGpoTribProduto);

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
                            throw new Exception("Erro ao Incluir Regra Fiscal Icms Gpo Trib Produto: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Fiscal Icms Gpo Trib Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public void Excluir(long CodigoRegra, int CodigoGrupo)
        {
            try
            {
                AbrirConexao();

                if (CodigoGrupo==0)
                    strSQL = "delete from [REGRA_FISCAL_ICMS_GRUPO_Produto] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";
                else
                    strSQL = "delete from [REGRA_FISCAL_ICMS_GRUPO_Produto] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_GPO_TRIB_Produto] = @v2 ";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Cmd.Parameters.AddWithValue("@v2", CodigoGrupo);

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
                            throw new Exception("Erro ao excluir Regra Fiscal Icms Gpo Trib Produto: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Regra Fiscal Icms Gpo Trib Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIcmsGpoTribProduto PesquisarRegFisIcmsGpoTribProduto(long CodigoRegra, int CodigoGrupo)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_GRUPO_Produto] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_GPO_TRIB_Produto] = @v2 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Cmd.Parameters.AddWithValue("@v2", CodigoGrupo);

                Dr = Cmd.ExecuteReader();

                RegFisIcmsGpoTribProduto p = null;

                if (Dr.Read())
                {
                    p = new RegFisIcmsGpoTribProduto
                    {
                        CodGpoTribProduto = Convert.ToInt32(Dr["CD_GPO_TRIB_Produto"]),
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"])
                    };
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Gpo Trib Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegFisIcmsGpoTribProduto> ListarRegFisIcmsGpoTribProduto(long CodigoRegra)
        {
            List<RegFisIcmsGpoTribProduto> lista = new List<RegFisIcmsGpoTribProduto>();
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_GRUPO_Produto] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);


                Dr = Cmd.ExecuteReader();

                RegFisIcmsGpoTribProduto p = null;

                while (Dr.Read())
                {
                    p = new RegFisIcmsGpoTribProduto
                    {
                        CodGpoTribProduto = Convert.ToInt32(Dr["CD_GPO_TRIB_Produto"]),
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"])
                    };
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Gpo Trib Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public DataTable ObterRegFisIcmsGpoProduto(double CodRegra)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [REGRA_FISCAL_ICMS_GRUPO_PRODUTO] WHERE  CD_REGRA_FISCAL_ICMS = " + CodRegra.ToString();

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Obter Regra Fiscal de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }

    }
}
