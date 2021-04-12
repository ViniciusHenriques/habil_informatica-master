using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Model;
using DAL.Persistence;

namespace DAL.Persistence
{
    public class HabilAtividadeExtraDAL:Conexao
    {
        protected string strSQL = "";

        protected SqlConnection Con2;

        protected SqlCommand Cmd2;

        protected SqlDataReader Dr2;

        protected void AbrirConexao2()
        {
            try
            {
                Con = new SqlConnection(@"Data Source=192.168.0.18\SQLserver2008;Initial Catalog=Fabesul;User ID=sa;Password=habil");

                Con.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void FecharConexao2()
        {
            try
            {
                Con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Inserir(HabilAtividadeExtra p, ref int CodigoAtividade)
        {
            try
            {
                AbrirConexao();

                strSQL = "INSERT INTO [dbo].[HABIL_ATIVIDADE_EXTRA]" +
                           " ([DT_LANCAMENTO]" +
                           " ,[CD_USUARIO]" +
                           " ,[NM_TABELA]" +
                           " ,[DS_ATIVIDADE]" +
                           " ,[DS_FILTRO]" +
                           " ,[CD_SITUACAO])" +
                        "VALUES (@v2,@v3,@v4,@v5,@v6,@v7) SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v2", p.DataHoraLancamento);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v4", p.NomeTabela);
                Cmd.Parameters.AddWithValue("@v5", p.DescricaoAtividade);
                Cmd.Parameters.AddWithValue("@v6", p.DescricaoFiltro);
                Cmd.Parameters.AddWithValue("@v7", p.CodigoSituacao);

                CodigoAtividade = Convert.ToInt32(Cmd.ExecuteScalar().ToString());

                HabilAtividadeExtra ativ = new HabilAtividadeExtra();
                ativ = PesquisarAtividade(CodigoAtividade);

                return ativ.Chave;

                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar HABIL_ATIVIDADE_EXTRA: " + ex.Message.ToString());            
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<HabilAtividadeExtra> ListarAtividades(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT ATIV.*,HT.DS_TIPO FROM HABIL_ATIVIDADE_EXTRA AS ATIV " +
                                    " INNER JOIN HABIL_TIPO AS HT ON HT.CD_TIPO = ATIV.CD_SITUACAO";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<HabilAtividadeExtra> lista = new List<HabilAtividadeExtra>();

                while (Dr.Read())
                {
                    HabilAtividadeExtra p = new HabilAtividadeExtra();

                    if (Dr["CD_USUARIO"] != DBNull.Value)
                        p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    if(Dr["DT_ATUALIZACAO"] != DBNull.Value)
                        p.DataHoraAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Chave = Convert.ToString(Dr["CD_CHAVE"]);
                    p.DescricaoAtividade = Convert.ToString(Dr["DS_ATIVIDADE"]);
                    p.DataHoraLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    p.NomeTabela = Convert.ToString(Dr["NM_TABELA"]);
                    p.Cpl_DsSituacao = Convert.ToString(Dr["DS_TIPO"]);
                    p.DescricaoFiltro = Convert.ToString(Dr["DS_FILTRO"]);

                    if (p.NomeTabela == "TEMP_REL_IMP_CONSUMO")
                        p.VisibleCheckBox = true;

                    if (Dr["IN_IMPOSTOS"] != DBNull.Value)
                        if (Dr["IN_IMPOSTOS"].ToString() == "1")
                            p.Impostos = true;

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar todas atividades extras: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public DataTable ListarTEMP_REL_IMP_CONSUMO(string strGUID, string strNomeTabela)
        {
            try
            {
                AbrirConexao();
                DataTable dt = new DataTable();

                string strSQL = "SELECT * FROM "+ strNomeTabela + " WHERE guid = @v1 ORDER BY vtot desc";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strGUID);

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar consumo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable ListarTEMP_REL_IMP_ENTRADAS(string strGUID, string strNota)
        {
            try
            {
                AbrirConexao();
                DataTable dt = new DataTable();

                string strSQL = "SELECT * FROM [TEMP_REL_IMP_ENTRADAS] WHERE nota = @v1 and guid = @v2";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v2", strGUID);
                Cmd.Parameters.AddWithValue("@v1", strNota);

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar entradas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<string> ListarNotasEntrada(string strGUID)
        {
            try
            {
                AbrirConexao();
                DataTable dt = new DataTable();

                string strSQL = "SELECT  distinct(nota),nlan FROM TEMP_REL_IMP_ENTRADAS WHERE GUID = @v1 order by nlan ";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strGUID);

                Dr = Cmd.ExecuteReader();

                List<string> ListaNota = new List<string>();
                while (Dr.Read())
                {
                    ListaNota.Add(Dr["nota"].ToString());
                }

                return ListaNota;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar notas entrada: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable ListarCFOPNota(string strGUID, string strNota)
        {
            try
            {
                AbrirConexao();
                DataTable dt = new DataTable();

                string strSQL = "SELECT distinct " +
                                    "cfop,  " +
                                    "isnull((select sum(vtot) FROM TEMP_REL_IMP_ENTRADAS WHERE nota = t.nota and guid = t.guid and cfop = t.cfop),0)  as BASE, " +
                                    "isnull((select sum(vtot * (ipi / 100)) FROM TEMP_REL_IMP_ENTRADAS WHERE nota = t.nota and guid = t.guid and cfop = t.cfop),0)  as TOTAL_IPI, " +
                                    "isnull((select sum(vicms) FROM TEMP_REL_IMP_ENTRADAS WHERE nota = t.nota and guid = t.guid and cfop = t.cfop),0)  as TOTAL_ICMS " +
                                "FROM TEMP_REL_IMP_ENTRADAS as t WHERE nota = @v1 and guid = @v2";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strNota);
                Cmd.Parameters.AddWithValue("@v2", strGUID);

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar notas entrada: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTEMP(string strGUID, string strNomeTabela)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("DELETE " + strNomeTabela + " Where [guid] = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", strGUID);
                
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
                            throw new Exception("Erro ao EXCLUIR temp_rel_imp_consumo: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao EXCLUIR temp_rel_imp_consumo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void Excluir(int Codigo, string strGUID, string strNomeTabela)
        {
            try
            {
                ExcluirTEMP(strGUID,strNomeTabela);

                AbrirConexao();
                Cmd = new SqlCommand("DELETE [HABIL_ATIVIDADE_EXTRA] Where [CD_INDEX] = @v1", Con);
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
                            throw new Exception("Erro ao EXCLUIR ATIVIDADE: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao EXCLUIR ATIVIDADE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void AlterarSituacao(int CodigoAtividade, int CodigoSituacao)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("UPDATE [HABIL_ATIVIDADE_EXTRA] SET CD_SITUACAO = @v1 Where [CD_INDEX] = @v2", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v2", CodigoAtividade);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ALTERAR SITUACAO ATIVIDADE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void AlterarDataAtualizacao(int CodigoAtividade)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("UPDATE [HABIL_ATIVIDADE_EXTRA] SET DT_ATUALIZACAO = GETDATE() Where [CD_INDEX] = @v2", Con);
                Cmd.Parameters.AddWithValue("@v2", CodigoAtividade);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ALTERAR SITUACAO ATIVIDADE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public HabilAtividadeExtra PesquisarAtividade(int Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from [HABIL_ATIVIDADE_EXTRA] Where CD_INDEX = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                HabilAtividadeExtra p = null;

                if (Dr.Read())
                {
                    p = new HabilAtividadeExtra();

                    if (Dr["CD_USUARIO"] != DBNull.Value)
                        p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    if (Dr["DT_ATUALIZACAO"] != DBNull.Value)
                        p.DataHoraAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Chave = Convert.ToString(Dr["CD_CHAVE"]);
                    p.DescricaoAtividade = Convert.ToString(Dr["DS_ATIVIDADE"]);
                    p.DataHoraLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    p.NomeTabela = Convert.ToString(Dr["NM_TABELA"]);
                    p.DescricaoFiltro = Convert.ToString(Dr["DS_FILTRO"]);

                    if (Dr["IN_IMPOSTOS"] != DBNull.Value)
                        if (Dr["IN_IMPOSTOS"].ToString() == "1")
                            p.Impostos = true;

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar atividade: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void InserirTEMP_REL_CONTRATOS(DataTable dt, string strGUID)
        {
            try
            {
                AbrirConexao();
                foreach (DataRow row in dt.Rows)
                {
                    strSQL = "insert into TEMP_REL_CONTRATOS(cpro,descr,ncm,unid,prec,guid,cliente) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7)";

                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v1", row[0].ToString());
                    Cmd.Parameters.AddWithValue("@v2", row[1].ToString());
                    Cmd.Parameters.AddWithValue("@v3", row[2].ToString());
                    Cmd.Parameters.AddWithValue("@v4", row[3].ToString());
                    Cmd.Parameters.AddWithValue("@v5", Convert.ToDecimal(row[4].ToString()));
                    Cmd.Parameters.AddWithValue("@v6", strGUID);
                    Cmd.Parameters.AddWithValue("@v7", row[5].ToString());

                    Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar TEMP_REL_CONTRATOS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
