using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class PessoaInscricaoDAL : Conexao
    {
        Habil_TipoDAL RnHabilTipo = new Habil_TipoDAL();
        protected string strSQL = "";
        public Pessoa_Inscricao PesquisarPessoaInscricao(Int64 CodPessoa, Int32 CodItem)
        {
            try
            {

                AbrirConexao();
                Cmd = new SqlCommand("Select * from PESSOA_INSCRICAO Where CD_PESSOA = @v1 AND CD_INSCRICAO = @v2", Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                Cmd.Parameters.AddWithValue("@v2", CodItem);
                Dr = Cmd.ExecuteReader();
                Pessoa_Inscricao p = new Pessoa_Inscricao();

                if (Dr.Read())
                {
                    p._CodigoItem = Convert.ToInt32(Dr["CD_INSCRICAO"]);
                    p._TipoInscricao = Convert.ToInt32(Dr["TP_INSCRICAO"]);
                    p._NumeroInscricao = Convert.ToString(Dr["NR_INSCRICAO"]);

                    if (!Dr.IsDBNull(Dr.GetOrdinal("DT_ABERTURA")))
                        p._DataDeAbertura = Convert.ToDateTime(Dr["DT_ABERTURA"]);

                    if (!Dr.IsDBNull(Dr.GetOrdinal("DT_ENCERRAMENTO")))
                        p._DataDeEncerramento = Convert.ToDateTime(Dr["DT_ENCERRAMENTO"]);

                    p._OBS = Dr["TX_OBS"].ToString();
                    p._NumeroIERG = Convert.ToString(Dr["NR_IERG"]);
                    p._NumeroIM = Dr["NR_IM"].ToString();

                    p._TipoInscricaoD = RnHabilTipo.DescricaoHabil_Tipo(p._TipoInscricao);
                    p._DcrInscricao = p._NumeroInscricao.ToString() + " (" + p._TipoInscricao.ToString() + ") " + p._NumeroIERG;
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Inscrição da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodos(Int64 CodPessoa)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from Pessoa_Inscricao Where CD_PESSOA = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);
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
                            throw new Exception("Erro ao excluir Inscrição da Pessoa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Inscrição da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Inserir(Int64 CodPessoa, List<Pessoa_Inscricao> listCadPessoaInscricao)
        {
            try
            {
                ExcluirTodos(CodPessoa);

                AbrirConexao();
                foreach (Pessoa_Inscricao p in listCadPessoaInscricao)
                {

                    Cmd = new SqlCommand("insert into PESSOA_INSCRICAO (CD_PESSOA, CD_INSCRICAO, TP_INSCRICAO, NR_INSCRICAO, NR_IERG, NR_IM, DT_ABERTURA, DT_ENCERRAMENTO, TX_OBS,CD_PAIS) values (@v1,@v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10)", Con);
                    Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                    Cmd.Parameters.AddWithValue("@v2", p._CodigoItem);
                    Cmd.Parameters.AddWithValue("@v3", p._TipoInscricao );
                    Cmd.Parameters.AddWithValue("@v4", p._NumeroInscricao);
                    Cmd.Parameters.AddWithValue("@v5", p._NumeroIERG);
                    Cmd.Parameters.AddWithValue("@v6", p._NumeroIM);
                    Cmd.Parameters.AddWithValue("@v7", p._DataDeAbertura);
                    if (p._DataDeEncerramento != null)
                        Cmd.Parameters.AddWithValue("@v8", p._DataDeEncerramento);
                    else
                        Cmd.Parameters.AddWithValue("@v8", DBNull.Value);

                    Cmd.Parameters.AddWithValue("@v9", p._OBS);
                    Cmd.Parameters.AddWithValue("@v10", p.CodigoPais);
                    Cmd.ExecuteNonQuery();


                }
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
                            throw new Exception("Erro ao gravar Pessoa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception ("Erro ao gravar Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Pessoa_Inscricao> ObterPessoaInscricoes(Int64 CodPessoa)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from PESSOA_INSCRICAO Where CD_PESSOA = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                Dr = Cmd.ExecuteReader();
                List<Pessoa_Inscricao> lista = new List<Pessoa_Inscricao>();

                while (Dr.Read())
                {
                    Pessoa_Inscricao  p = new  Pessoa_Inscricao();
                    p._CodigoItem = Convert.ToInt32(Dr["CD_INSCRICAO"]);
                    p._TipoInscricao = Convert.ToInt32(Dr["TP_INSCRICAO"]);
                    p._NumeroInscricao = Convert.ToString(Dr["NR_INSCRICAO"]);

                    if (! Dr.IsDBNull(Dr.GetOrdinal("DT_ABERTURA")))
                        p._DataDeAbertura =  Convert.ToDateTime(Dr["DT_ABERTURA"]);

                    if (!Dr.IsDBNull(Dr.GetOrdinal("DT_ENCERRAMENTO")))
                        p._DataDeEncerramento = Convert.ToDateTime(Dr["DT_ENCERRAMENTO"]);
                    
                    p._OBS = Dr["TX_OBS"].ToString() ;
                    p._NumeroIERG = Convert.ToString(Dr["NR_IERG"]);
                    p._NumeroIM = Dr["NR_IM"].ToString();
                    p.CodigoPais= Convert.ToInt32(Dr["CD_PAIS"].ToString());

                    p._TipoInscricaoD = RnHabilTipo.DescricaoHabil_Tipo(p._TipoInscricao);

                    p._DcrInscricao = p._NumeroInscricao.ToString() + " (" + p._TipoInscricao.ToString() + ") " + p._NumeroIERG;

                    lista.Add(p); 
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Inscrição da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public string ObterInscricao(Int64 CodPessoa, Int32 CodItem)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select NR_INSCRICAO from PESSOA_INSCRICAO Where CD_PESSOA = @v1 AND CD_INSCRICAO = @v2";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                Cmd.Parameters.AddWithValue("@v2", CodItem);
                Dr = Cmd.ExecuteReader();
                if (Dr.Read())
                    return Convert.ToString(Dr["NR_INSCRICAO"]);
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Inscrição da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public int PesquisarPessoaInscricao(string strInscricao)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [PESSOA_INSCRICAO] Where NR_INSCRICAO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strInscricao);

                Dr = Cmd.ExecuteReader();

                int p = 0;

                if (Dr.Read())
                {
                    p = Convert.ToInt32(Dr["CD_PESSOA"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tipo de Serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Pessoa_Inscricao> ListarPessoaInscricoes(List<DBTabelaCampos> ListaFiltros, short shtTipoPessoa)
        {
            try
            {
                AbrirConexao();
                string strValor = "";

                string strSQL = "Select  IP.*, T.DS_TIPO " +
                    " from PESSOA_INSCRICAO as IP" +
                    "     Inner Join Pessoa as P" +
                    "         on IP.CD_PESSOA = P.CD_PESSOA" +
                    "     Inner Join HABIL_TIPO as T" +
                    "         on IP.TP_INSCRICAO = T.CD_TIPO" +

                    " WHERE P.[CD_PESSOA] IN ( SELECT [VW_PESSOA].COD_PESSOA FROM [VW_PESSOA]  ";
                
                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;

                strSQL = strSQL + ")";
                if (shtTipoPessoa == 1)
                    strSQL = strSQL + " and P.IN_FORNECEDOR = 1";
                else if (shtTipoPessoa == 2)
                    strSQL = strSQL + " and P.IN_CLIENTE = 1";
                else if (shtTipoPessoa == 3)
                    strSQL = strSQL + " and P.IN_TRANSPORTADOR = 1";

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                List<Pessoa_Inscricao> lista = new List<Pessoa_Inscricao>();

                while (Dr.Read())
                {
                    Pessoa_Inscricao p = new Pessoa_Inscricao();

                    p._CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p._CodigoItem = Convert.ToInt32(Dr["CD_INSCRICAO"]);
                    p._TipoInscricao = Convert.ToInt32(Dr["TP_INSCRICAO"]);
                    p._NumeroInscricao = Convert.ToString(Dr["NR_INSCRICAO"]);

                    if (!Dr.IsDBNull(Dr.GetOrdinal("DT_ABERTURA")))
                        p._DataDeAbertura = Convert.ToDateTime(Dr["DT_ABERTURA"]);

                    if (!Dr.IsDBNull(Dr.GetOrdinal("DT_ENCERRAMENTO")))
                        p._DataDeEncerramento = Convert.ToDateTime(Dr["DT_ENCERRAMENTO"]);

                    p._OBS = Dr["TX_OBS"].ToString();
                    p._NumeroIERG = Convert.ToString(Dr["NR_IERG"]);
                    p._NumeroIM = Dr["NR_IM"].ToString();

                    //p._TipoInscricaoD = RnHabilTipo.DescricaoHabil_Tipo(p._TipoInscricao);
                    p._TipoInscricaoD = Dr["DS_TIPO"].ToString();

                    p._DcrInscricao = p._NumeroInscricao.ToString() + " (" + p._TipoInscricao.ToString() + ") " + p._NumeroIERG;

                    lista.Add(p);
                }
                Dr.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Inscrição da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }


    }
}
