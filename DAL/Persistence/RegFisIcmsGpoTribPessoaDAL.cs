using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class RegFisIcmsGpoTribPessoaDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(RegFisIcmsGpoTribPessoa p)
        {
            try
            {
                Excluir(p.CodigoRegFisIcms, p.CodGpoTribPessoa);

                AbrirConexao();


                strSQL = "insert into [REGRA_FISCAL_ICMS_GRUPO_PESSOA] (CD_REGRA_FISCAL_ICMS, CD_GPO_TRIB_PESSOA) values (@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoRegFisIcms);
                Cmd.Parameters.AddWithValue("@v2", p.CodGpoTribPessoa);

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
                            throw new Exception("Erro ao Incluir Regra Fiscal Icms Gpo Trib Pessoa: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Fiscal Icms Gpo Trib Pessoa: " + ex.Message.ToString());
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

                if(CodigoGrupo == 0)
                    strSQL = "delete from [REGRA_FISCAL_ICMS_GRUPO_PESSOA] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";
                else
                    strSQL = "delete from [REGRA_FISCAL_ICMS_GRUPO_PESSOA] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_GPO_TRIB_PESSOA] = @v2 ";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);

                if (CodigoGrupo != 0)
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
                            throw new Exception("Erro ao excluir Regra Fiscal Icms Gpo Trib Pessoa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Regra Fiscal Icms Gpo Trib Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIcmsGpoTribPessoa PesquisarRegFisIcmsGpoTribPessoa(long CodigoRegra, int CodigoGrupo)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_GRUPO_PESSOA] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_GPO_TRIB_PESSOA] = @v2 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Cmd.Parameters.AddWithValue("@v2", CodigoGrupo);
                Dr = Cmd.ExecuteReader();

                RegFisIcmsGpoTribPessoa p = null;

                if (Dr.Read())
                {
                    p = new RegFisIcmsGpoTribPessoa
                    {
                        CodGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]),
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"])
                    };
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Gpo Trib Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegFisIcmsGpoTribPessoa> ListarRegFisIcmsGpoTribPessoa(long CodigoRegra)
        {
            List<RegFisIcmsGpoTribPessoa> lista = new List<RegFisIcmsGpoTribPessoa>();
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_GRUPO_PESSOA] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Dr = Cmd.ExecuteReader();

                RegFisIcmsGpoTribPessoa p = null;

                while (Dr.Read())
                {
                    p = new RegFisIcmsGpoTribPessoa
                    {
                        CodGpoTribPessoa = Convert.ToInt32(Dr["CD_GPO_TRIB_PESSOA"]),
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"])
                    };

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Gpo Trib Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public DataTable ObterRegFisIcmsGpoPessoa(double CodRegra)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [REGRA_FISCAL_ICMS_GRUPO_PESSOA] WHERE  CD_REGRA_FISCAL_ICMS = " + CodRegra.ToString();

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
