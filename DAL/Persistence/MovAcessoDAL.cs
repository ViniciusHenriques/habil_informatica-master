using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class MovAcessoDAL : Conexao
    {
        public void Inserir(MovAcesso  p)
        {
            try
            {
                AbrirConexao();
                string strSQL = "insert into MOVIMENTACAO_DE_ACESSO(DT_HR_ENTRADA, CD_TIPO_ACESSO, TX_DOCUMENTO, [CD_PESSOA], CD_CONTATO, [CD_VEICULO], [TX_OBS]";

                if (p.DataHoraSaida != null)
                {
                    strSQL += ",DT_HR_SAIDA";
                }

                strSQL += ") values(@v1, @v2, @v3, @v4, @v5, @v6, @v7";

                if (p.DataHoraSaida != null)
                {
                    strSQL += ",@v8";
                }

                strSQL += "); SELECT SCOPE_IDENTITY()";
            
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DataHoraEntrada);
                Cmd.Parameters.AddWithValue("@v2", p.CodTipoAcesso);
                Cmd.Parameters.AddWithValue("@v3", p.Documento);
                Cmd.Parameters.AddWithValue("@v4", p.CodPessoa);
                Cmd.Parameters.AddWithValue("@v5", p.CodContato);
                Cmd.Parameters.AddWithValue("@v6", p.CodVeiculo);
                Cmd.Parameters.AddWithValue("@v7", p.Observacoes);
                if (p.DataHoraSaida != null)
                {
                    Cmd.Parameters.AddWithValue("@v8", p.DataHoraSaida);
                }

                    //Cmd.ExecuteNonQuery();
                    p.CodMovAcesso = Convert.ToInt64(Cmd.ExecuteScalar());
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
                throw new Exception ("Erro ao gravar Movimentação de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(MovAcesso p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update [MOVIMENTACAO_DE_ACESSO] set [CD_TIPO_ACESSO] = @v2, [TX_DOCUMENTO] = @v3, [CD_PESSOA] = @v4, CD_CONTATO = @v5, [CD_VEICULO] = @v6, [TX_OBS] = @v7 Where [CD_MOV_ACESSO] = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodMovAcesso);
                Cmd.Parameters.AddWithValue("@v2", p.CodTipoAcesso);
                Cmd.Parameters.AddWithValue("@v3", p.Documento);
                Cmd.Parameters.AddWithValue("@v4", p.CodPessoa);
                Cmd.Parameters.AddWithValue("@v5", p.CodContato);
                Cmd.Parameters.AddWithValue("@v6", p.CodVeiculo);
                Cmd.Parameters.AddWithValue("@v7", p.Observacoes);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Movimentação de Acesso: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from MOVIMENTACAO_DE_ACESSO Where CD_MOV_ACESSO = @v1", Con);
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
        public MovAcesso PesquisarMovAcesso(long Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from MOVIMENTACAO_DE_ACESSO Where CD_MOV_ACESSO = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                MovAcesso p = null;
                if (Dr.Read())
                {
                    p = new MovAcesso();
                    p.CodMovAcesso = Convert.ToInt64(Dr["CD_MOV_ACESSO"]);
                    p.DataHoraEntrada = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.Documento = Convert.ToString(Dr["TX_DOCUMENTO"]);
                    p.Observacoes = Convert.ToString(Dr["TX_OBS"]);

                    if (Dr["DT_HR_SAIDA"] != DBNull.Value)
                    {
                        p.DataHoraSaida = Convert.ToDateTime(Dr["DT_HR_SAIDA"]);
                    }

                    if (Dr["CD_TIPO_ACESSO"] != DBNull.Value)
                    {
                        p.CodTipoAcesso = Convert.ToInt32(Dr["CD_TIPO_ACESSO"]);
                    }

                    if (Dr["CD_PESSOA"] != DBNull.Value)
                    {
                        p.CodPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    }

                    if (Dr["CD_CONTATO"] != DBNull.Value)
                    {
                        p.CodContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    }

                    if (Dr["CD_VEICULO"] != DBNull.Value)
                    {
                        p.CodVeiculo = Convert.ToInt64(Dr["CD_VEICULO"]);
                    }

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Movimentação de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<MovAcesso> ListarMovAcessos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select * from MOVIMENTACAO_DE_ACESSO ";
                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 
                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem; 
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                List<MovAcesso> lista = new List<MovAcesso>();
                while (Dr.Read())
                {
                    MovAcesso p = new MovAcesso();
                    p.CodMovAcesso = Convert.ToInt64(Dr["CD_MOV_ACESSO"]);
                    p.DataHoraEntrada = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);
                    p.Documento = Convert.ToString(Dr["TX_DOCUMENTO"]);
                    p.Observacoes = Convert.ToString(Dr["TX_OBS"]);

                    if (Dr["DT_HR_SAIDA"] != DBNull.Value)
                    {
                        p.DataHoraSaida = Convert.ToDateTime(Dr["DT_HR_SAIDA"]);
                    }

                    if (Dr["CD_TIPO_ACESSO"] != DBNull.Value)
                    {
                        p.CodTipoAcesso = Convert.ToInt32(Dr["CD_TIPO_ACESSO"]);
                    }

                    if (Dr["CD_PESSOA"] != DBNull.Value)
                    {
                        p.CodPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    }

                    if (Dr["CD_CONTATO"] != DBNull.Value)
                    {
                        p.CodContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                    }

                    if (Dr["CD_VEICULO"] != DBNull.Value)
                    {
                        p.CodVeiculo = Convert.ToInt64(Dr["CD_VEICULO"]);
                    }

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
        public List<MovAcesso> ListarMovAcessosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VW_MOV_ACESSO]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;



                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<MovAcesso> lista = new List<MovAcesso>();

                while (Dr.Read())
                {
                    MovAcesso p = new MovAcesso();
                    p.CodMovAcesso = Convert.ToInt64(Dr["CD_MOV_ACESSO"]);
                    p.Observacoes = Convert.ToString(Dr["TX_OBS"]);
                    p.Documento = Convert.ToString(Dr["TX_DOCUMENTO"]);

                    p.DataHoraEntrada = Convert.ToDateTime(Dr["DT_HR_ENTRADA"]);

                    if (Dr["DT_HR_SAIDA"] != DBNull.Value)
                    {
                        p.DataHoraSaida = Convert.ToDateTime(Dr["DT_HR_SAIDA"]);
                    }

                    if (Dr["CD_TIPO_ACESSO"] != DBNull.Value)
                    {
                        p.CodTipoAcesso = Convert.ToInt32(Dr["CD_TIPO_ACESSO"]);
                        p.TipoAcesso = Convert.ToString(Dr["DS_TIPO_ACESSO"]);
                    }

                    if (Dr["CD_PESSOA"] != DBNull.Value)
                    {
                        p.CodPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                        p.Pessoa = Convert.ToString(Dr["NM_PESSOA"]) + " - " + Convert.ToString(Dr["NR_INSCRICAO"]);
                    }

                    if (Dr["CD_CONTATO"] != DBNull.Value)
                    {
                        p.CodContato = Convert.ToInt32(Dr["CD_CONTATO"]);
                        p.Contato = Convert.ToString(Dr["NM_CONTATO"]);
                    }

                    if (Dr["CD_VEICULO"] != DBNull.Value)
                    {
                        p.CodVeiculo = Convert.ToInt64(Dr["CD_VEICULO"]);
                        p.Veiculo = Convert.ToString(Dr["NM_VEICULO"]);
                    }

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
        public DataTable RelMovAcessosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_MOV_ACESSO]  " + MontaFiltroIntervalo(ListaFiltros);

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório de Todas Movimentações de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }

        public void GerarSaida(MovAcesso p)
        {
            try
            {
                if (p.DataHoraSaida != null)
                {
                    AbrirConexao();

                    Cmd = new SqlCommand("update [MOVIMENTACAO_DE_ACESSO] set [DT_HR_SAIDA] = @v2 Where [CD_MOV_ACESSO] = @v1", Con);
                    Cmd.Parameters.AddWithValue("@v1", p.CodMovAcesso);
                    Cmd.Parameters.AddWithValue("@v2", p.DataHoraSaida);
                    Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao GerarSaida Movimentação de Acesso: " + ex.Message.ToString());
            }
            finally
            {
                if (p.DataHoraSaida != null)
                {
                    FecharConexao();
                }
            }
        }



    }
}
