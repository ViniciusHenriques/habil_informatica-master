using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class TabIcmsDAL : Conexao
    {
        public void Inserir(TabIcms  p)
        {
            try
            {
                AbrirConexao();
                string strSQL = "insert into HABIL_TABELA_ICMS_ALIQUOTAS (CD_EST_ORIGEM, CD_EST_DESTINO, AL_ICMS_INTERNO, AL_ICMS_INTERESTADUAL, AL_ICMS_PROD_EXTERIOR )";
                strSQL += " values(@v1, @v2, @v3, @v4, @v5); SELECT SCOPE_IDENTITY()";
            
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodOrigem);
                Cmd.Parameters.AddWithValue("@v2", p.CodDestino);
                Cmd.Parameters.AddWithValue("@v3", p.IcmsInterno);
                Cmd.Parameters.AddWithValue("@v4", p.IcmsInterEstadual);
                Cmd.Parameters.AddWithValue("@v5", p.IcmsExterno);
                p.CodTabAliqIcms = Convert.ToInt64(Cmd.ExecuteScalar());
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
                            throw new Exception("Erro ao gravar Movimentação de Acesso: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception ("Erro ao gravar Tabela de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(TabIcms p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update [HABIL_TABELA_ICMS_ALIQUOTAS] set [CD_EST_ORIGEM] = @v2, [CD_EST_DESTINO] = @v3, [AL_ICMS_INTERNO] = @v4, [AL_ICMS_INTERESTADUAL] = @v5, [AL_ICMS_PROD_EXTERIOR] = @v6 Where [CD_TAB_ALIQ_ICMS] = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodTabAliqIcms);

                Cmd.Parameters.AddWithValue("@v2", p.CodOrigem);
                Cmd.Parameters.AddWithValue("@v3", p.CodDestino);
                Cmd.Parameters.AddWithValue("@v4", p.IcmsInterno);
                Cmd.Parameters.AddWithValue("@v5", p.IcmsInterEstadual);
                Cmd.Parameters.AddWithValue("@v6", p.IcmsExterno);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Tabela de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(long Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from [HABIL_TABELA_ICMS_ALIQUOTAS] Where CD_TAB_ALIQ_ICMS = @v1", Con);
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
                            throw new Exception("Erro ao excluir Movimentação de Acesso: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Movimentação de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public TabIcms PesquisarTabIcms(long Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from HABIL_TABELA_ICMS_ALIQUOTAS Where CD_TAB_ALIQ_ICMS = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                TabIcms p = null;
                if (Dr.Read())
                {
                    p = new TabIcms();
                    p.CodTabAliqIcms = Convert.ToInt64(Dr["CD_TAB_ALIQ_ICMS"]);
                    p.CodOrigem = Convert.ToInt32(Dr["CD_EST_ORIGEM"]);
                    p.CodDestino = Convert.ToInt32(Dr["CD_EST_DESTINO"]);
                    p.IcmsInterno = Convert.ToDouble(Dr["AL_ICMS_INTERNO"]);
                    p.IcmsInterEstadual= Convert.ToDouble(Dr["AL_ICMS_INTERESTADUAL"]);
                    p.IcmsExterno = Convert.ToDouble(Dr["AL_ICMS_PROD_EXTERIOR"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tabela de Icms: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<TabIcms> ListarTabIcms(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select * from HABIL_TABELA_ICMS_ALIQUOTAS ";
                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 
                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem; 
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                List<TabIcms> lista = new List<TabIcms>();
                while (Dr.Read())
                {
                    TabIcms p = new TabIcms();
                    p.CodTabAliqIcms = Convert.ToInt64(Dr["CD_TAB_ALIQ_ICMS"]);
                    p.CodOrigem = Convert.ToInt32(Dr["CD_EST_ORIGEM"]);
                    p.CodDestino = Convert.ToInt32(Dr["CD_EST_DESTINO"]);
                    p.IcmsInterno = Convert.ToDouble(Dr["AL_ICMS_INTERNO"]);
                    p.IcmsInterEstadual = Convert.ToDouble(Dr["AL_ICMS_INTERESTADUAL"]);
                    p.IcmsExterno = Convert.ToDouble(Dr["AL_ICMS_PROD_EXTERIOR"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Movimentações de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<TabIcms> ListarTabIcmsCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VW_TAB_ICMS]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor + " Order By UF_Origem, UF_Destino";



                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TabIcms> lista = new List<TabIcms>();

                while (Dr.Read())
                {
                    TabIcms p = new TabIcms();
                    p.CodTabAliqIcms = Convert.ToInt64(Dr["CD_TAB_ALIQ_ICMS"]);
                    p.CodOrigem = Convert.ToInt32(Dr["CD_EST_ORIGEM"]);
                    p.CodDestino = Convert.ToInt32(Dr["CD_EST_DESTINO"]);

                    p.UFOrigem = Convert.ToString(Dr["UF_ORIGEM"]);
                    p.UFDestino = Convert.ToString(Dr["UF_DESTINO"]);

                    p.IcmsInterno = Convert.ToDouble(Dr["AL_ICMS_INTERNO"]);
                    p.IcmsInterEstadual = Convert.ToDouble(Dr["AL_ICMS_INTERESTADUAL"]);
                    p.IcmsExterno = Convert.ToDouble(Dr["AL_ICMS_PROD_EXTERIOR"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Tabela de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable RelTabIcmssCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_TAB_ICMS]  " + MontaFiltroIntervalo(ListaFiltros);

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório de Todas Tabela de Icms: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public void AtualizarAliquotas(TabIcms p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update [HABIL_TABELA_ICMS_ALIQUOTAS] set [AL_ICMS_INTERNO] = @v4, [AL_ICMS_INTERESTADUAL] = @v5, [AL_ICMS_PROD_EXTERIOR] = @v6 Where [CD_TAB_ALIQ_ICMS] = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodTabAliqIcms);
                Cmd.Parameters.AddWithValue("@v4", p. IcmsInterno);
                Cmd.Parameters.AddWithValue("@v5", p.IcmsInterEstadual);
                Cmd.Parameters.AddWithValue("@v6", p.IcmsExterno);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Tabela de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
