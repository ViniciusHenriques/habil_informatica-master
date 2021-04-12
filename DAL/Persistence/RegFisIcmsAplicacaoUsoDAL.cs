using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class RegFisIcmsHabil_AplicacaoUsoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(RegFisIcmsHabil_AplicacaoUso p)
        {
            try
            {
                Excluir(p.CodigoRegFisIcms, p.CodHabil_AplicacaoUso);
                AbrirConexao();
                strSQL = "insert into [REGRA_FISCAL_ICMS_APLICACAO] (CD_REGRA_FISCAL_ICMS, CD_APLICACAO) values (@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoRegFisIcms);
                Cmd.Parameters.AddWithValue("@v2", p.CodHabil_AplicacaoUso);
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
                            throw new Exception("Erro ao Incluir Regra Fiscal Icms Aplicação Uso: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Fiscal Icms Regra Fiscal Icms Localização UF: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void Excluir(long CodigoRegra, int CodigoAplicacao)
        {
            try
            {
                AbrirConexao();

                if (CodigoAplicacao == 0)
                    strSQL = "delete from [REGRA_FISCAL_ICMS_APLICACAO] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";
                else
                    strSQL = "delete from [REGRA_FISCAL_ICMS_APLICACAO] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_APLICACAO] = @v2 ";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                if (CodigoAplicacao != 0)
                    Cmd.Parameters.AddWithValue("@v2", CodigoAplicacao);
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
                            throw new Exception("Erro ao excluir Regra Fiscal Icms Aplicação Uso: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Regra Fiscal Icms Aplicação Uso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIcmsHabil_AplicacaoUso PesquisarRegFisIcmsHabil_AplicacaoUso(long CodigoRegra, int CodigoAplicacao)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_APLICACAO] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_APLICACAO] = @v2 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Cmd.Parameters.AddWithValue("@v2", CodigoAplicacao);


                Dr = Cmd.ExecuteReader();

                RegFisIcmsHabil_AplicacaoUso p = null;

                if (Dr.Read())
                {
                    p = new RegFisIcmsHabil_AplicacaoUso
                    {
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"]),
                        CodHabil_AplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO"])
                    };
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Aplicação Uso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegFisIcmsHabil_AplicacaoUso> ListarRegFisIcmsHabil_AplicacaoUso(long CodigoRegra)
        {
            List<RegFisIcmsHabil_AplicacaoUso> lista = new List<RegFisIcmsHabil_AplicacaoUso>();

            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_APLICACAO] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);

                Dr = Cmd.ExecuteReader();

                RegFisIcmsHabil_AplicacaoUso p = null;

                while (Dr.Read())
                {
                    p = new RegFisIcmsHabil_AplicacaoUso
                    {
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"]),
                        CodHabil_AplicacaoUso = Convert.ToInt32(Dr["CD_APLICACAO"])
                    };
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Aplicação Uso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public DataTable ObterRegFisIcmsHabil_AplicacaoUso(double CodRegra)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [REGRA_FISCAL_ICMS_APLICACAO] WHERE  CD_REGRA_FISCAL_ICMS = " + CodRegra.ToString();

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