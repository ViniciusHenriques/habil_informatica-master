using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class RegFisIcmsTipoOperFiscalDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(RegFisIcmsTipoOperFiscal p)
        {
            try
            {
                Excluir(p.CodigoRegFisIcms, p.CodTipoOperFiscal);
                AbrirConexao();
                strSQL = "insert into [REGRA_FISCAL_ICMS_TIPO_OPERACAO_FISCAL] (CD_REGRA_FISCAL_ICMS, CD_TIPO_OP_FISCAL) values (@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoRegFisIcms);
                Cmd.Parameters.AddWithValue("@v2", p.CodTipoOperFiscal);
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
                            throw new Exception("Erro ao Incluir Regra Fiscal Icms Operação Fiscal: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Fiscal Icms Regra Fiscal Icms Operação Fiscal: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(long CodigoRegra, int CodigoHabil_TipoOperFiscal)
        {
            try
            {
                AbrirConexao();

                if (CodigoHabil_TipoOperFiscal != 0 )
                    strSQL = "delete from [REGRA_FISCAL_ICMS_TIPO_OPERACAO_FISCAL] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_TIPO_OP_FISCAL] = @v2 ";
                else
                    strSQL = "delete from [REGRA_FISCAL_ICMS_TIPO_OPERACAO_FISCAL] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                if (CodigoHabil_TipoOperFiscal != 0)
                    Cmd.Parameters.AddWithValue("@v2", CodigoHabil_TipoOperFiscal);

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
                            throw new Exception("Erro ao excluir Regra Fiscal Icms Operação Fiscal: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Regra Fiscal Icms Operação Fiscal: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegFisIcmsTipoOperFiscal PesquisarRegFisIcmsOperFiscal(long CodigoRegra, int CodigoHabil_TipoOperFiscal)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_TIPO_OPERACAO_FISCAL] Where [CD_REGRA_FISCAL_ICMS] = @v1 and [CD_TIPO_OP_FISCAL] = @v2 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
                Cmd.Parameters.AddWithValue("@v2", CodigoHabil_TipoOperFiscal);


                Dr = Cmd.ExecuteReader();

                RegFisIcmsTipoOperFiscal p = null;

                if (Dr.Read())
                {
                    p = new RegFisIcmsTipoOperFiscal
                    {
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"]),
                        CodTipoOperFiscal = Convert.ToInt32(Dr["CD_TIPO_OP_FISCAL"])
                    };
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Operação Fiscal: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegFisIcmsTipoOperFiscal> ListarRegFisIcmsOperFiscal(long CodigoRegra)
        {
            List<RegFisIcmsTipoOperFiscal> lista = new List<RegFisIcmsTipoOperFiscal>();
            try
            {
                AbrirConexao();

                strSQL = "Select * from [REGRA_FISCAL_ICMS_TIPO_OPERACAO_FISCAL] Where [CD_REGRA_FISCAL_ICMS] = @v1 ";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);

                Dr = Cmd.ExecuteReader();

                RegFisIcmsTipoOperFiscal p = null;

                while (Dr.Read())
                {
                    p = new RegFisIcmsTipoOperFiscal
                    {
                        CodigoRegFisIcms = Convert.ToInt64(Dr["CD_REGRA_FISCAL_ICMS"]),
                        CodTipoOperFiscal = Convert.ToInt32(Dr["CD_TIPO_OP_FISCAL"])
                    };

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Regra Fiscal Icms Operação Fiscal: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public DataTable ObterRegFisIcmsOperFiscal(double CodRegra)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [REGRA_FISCAL_ICMS_TIPO_OPERACAO_FISCAL] WHERE  CD_REGRA_FISCAL_ICMS = " + CodRegra.ToString();

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
