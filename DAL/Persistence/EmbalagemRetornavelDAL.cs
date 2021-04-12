using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class EmbalagemRetornavelDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(EmbalagemRetornavel p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [EMBALAGEM_RETORNAVEL] (CD_SITUACAO, CD_EMBALAGEM, NR_LACRES) values ( @v1, @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmbalagem.ToUpper());
                Cmd.Parameters.AddWithValue("@v3", p.NrLacres);

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
                            throw new Exception("Erro ao Incluir Bairro: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Embalagem Retornavel: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(EmbalagemRetornavel p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [EMBALAGEM_RETORNAVEL] set CD_SITUACAO = @v1, CD_EMBALAGEM = @v2, NR_LACRES = @v3 Where [CD_INDEX] = @v4";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v4", p.CodigoIndice);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEmbalagem);
                Cmd.Parameters.AddWithValue("@v3", p.NrLacres);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Embalagem Retornavel: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [EMBALAGEM_RETORNAVEL] Where [CD_INDEX] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Bairro: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Bairro: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public EmbalagemRetornavel CaixasAtivas(string strCdEmbalagem)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from EMBALAGEM_RETORNAVEL where CD_EMBALAGEM = '" + strCdEmbalagem + "' ";
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                EmbalagemRetornavel p = new EmbalagemRetornavel();

                if (Dr.Read())
                {
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmbalagem = Convert.ToString(Dr["CD_EMBALAGEM"]);
                    p.NrLacres = Convert.ToInt16(Dr["NR_LACRES"]);

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Embalagem retornavel: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public EmbalagemRetornavel EmbalagemExistente(int intCodIndice, string strCdEmbalagem)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from EMBALAGEM_RETORNAVEL where CD_EMBALAGEM = '" + strCdEmbalagem + "' ";
                if (intCodIndice != 0)
                    strSQL += " and CD_INDEX <> " + intCodIndice;

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                EmbalagemRetornavel p = new EmbalagemRetornavel();

                if (Dr.Read())
                {
                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmbalagem = Convert.ToString(Dr["CD_EMBALAGEM"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Embalagem retornavel: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<EmbalagemRetornavel> ListarEmbalagens(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();

                string strValor = "";
                string strSQL = "select * from [EMBALAGEM_RETORNAVEL] AS Em " +
                    "INNER JOIN HABIL_TIPO AS HT " +
                    "ON HT.CD_TIPO = EM.CD_SITUACAO";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<EmbalagemRetornavel> lista = new List<EmbalagemRetornavel>();

                while (Dr.Read())
                {
                    EmbalagemRetornavel p = new EmbalagemRetornavel();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoEmbalagem = Convert.ToString(Dr["CD_EMBALAGEM"]);
                    p.DescricaoSituacao = Convert.ToString(Dr["DS_TIPO"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Embalagens: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool AtualizarCaixas(int CdIndex)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL("UPDATE [EMBALAGEM_RETORNAVEL] SET CD_SITUACAO = 160 Where CD_INDEX = " + CdIndex.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar situação documento: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public EmbalagemRetornavel PesquisarEmbalagem(int intCodIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from EMBALAGEM_RETORNAVEL where CD_INDEX = " + intCodIndice;
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                EmbalagemRetornavel p = new EmbalagemRetornavel();

                if (Dr.Read())
                {
                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoEmbalagem = Convert.ToString(Dr["CD_EMBALAGEM"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Embalagem retornavel: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
