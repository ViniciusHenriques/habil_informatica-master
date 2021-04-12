using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class RegFisIcmsLocalizacaoDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(RegFisIcmsLocalizacao p)
        {
            try
            {
                Excluir(p.CodigoRegFisIcms, p.CodLocalizacaoUF);
                AbrirConexao();

                strSQL = "insert into [REGRA_FISCAL_ICMS_LOCALIZACAO] (CD_REGRA_FISCAL_ICMS, CD_TAB_ALIQ_ICMS) values (@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoRegFisIcms);
                Cmd.Parameters.AddWithValue("@v2", p.CodLocalizacaoUF);

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
                            throw new Exception("Erro ao Incluir Regra Fiscal Icms Localização UF: " + ex.Message.ToString());
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

        public void Excluir(long CodigoRegra, int CodigoLocalizacao)
        {
            try
            {
                AbrirConexao();

                if (CodigoLocalizacao == 0)
                    strSQL = "delete from [REGRA_FISCAL_ICMS_LOCALIZACAO] Where [CD_REGRA_FISCAL_ICMS] = @v1";
                else
                    strSQL = "delete from [REGRA_FISCAL_ICMS_LOCALIZACAO] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_TAB_ALIQ_ICMS] = @v2 ";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                if (CodigoLocalizacao != 0)
                    Cmd.Parameters.AddWithValue("@v2", CodigoLocalizacao);

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
                            throw new Exception("Erro ao excluir Regra Fiscal Icms Localização UF: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Regra Fiscal Icms Localização UF: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIcmsLocalizacao PesquisarRegFisIcmsLocalizacao(long CodigoRegra, int CodigoLocalizacao)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_LOCALIZACAO] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_TAB_ALIQ_ICMS] = @v2 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Cmd.Parameters.AddWithValue("@v2", CodigoLocalizacao);
                Dr = Cmd.ExecuteReader();

                RegFisIcmsLocalizacao p = null;

                if (Dr.Read())
                {
                    p = new RegFisIcmsLocalizacao
                    {
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"]),
                        CodLocalizacaoUF = Convert.ToInt32(Dr["CD_TAB_ALIQ_ICMS"])
                    };
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Localização UF: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegFisIcmsLocalizacao> ListarRegFisIcmsLocalizacao(long CodigoRegra)
        {
            List<RegFisIcmsLocalizacao> lista = new List<RegFisIcmsLocalizacao>();
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_LOCALIZACAO] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Dr = Cmd.ExecuteReader();

                RegFisIcmsLocalizacao p;

                while (Dr.Read())
                {
                    p = new RegFisIcmsLocalizacao
                    {
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"]),
                        CodLocalizacaoUF = Convert.ToInt32(Dr["CD_TAB_ALIQ_ICMS"])
                    };
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Localização UF: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public DataTable ObterRegFisIcmsLocalizacao(long CodRegra)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [REGRA_FISCAL_ICMS_LOCALIZACAO] WHERE  CD_REGRA_FISCAL_ICMS = " + CodRegra.ToString();

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
