using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class PessoaEnderecoDAL : Conexao
    {
        Habil_TipoDAL RnHabilTipo = new Habil_TipoDAL();

        public Pessoa_Endereco PesquisarPessoaEndereco(Int64 CodPessoa, Int32 CodItem)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from PESSOA_ENDERECO Where CD_PESSOA = @v1 AND CD_ENDERECO = @v2", Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                Cmd.Parameters.AddWithValue("@v2", CodItem);
                Dr = Cmd.ExecuteReader();
                Pessoa_Endereco p = new Pessoa_Endereco();

                if (Dr.Read())
                {
                    
                    p._CodigoItem = Convert.ToInt32(Dr["CD_ENDERECO"]);
                    p._Logradouro = Dr["TX_LOGRADOURO"].ToString();
                    p._NumeroLogradouro = Dr["NR_ENDERECO"].ToString();
                    p._Complemento = Dr["TX_COMPLEMENTO"].ToString();
                    p._CodigoCEP = Convert.ToInt64( Dr["CD_CEP"]);
                    p._DescricaoEstado = Dr["DS_ESTADO"].ToString();
                    p._DescricaoMunicipio = Dr["DS_MUNICIPIO"].ToString();
                    p._DescricaoBairro = Dr["DS_BAIRRO"].ToString();
                    p._CodigoBairro = Convert.ToInt32(Dr["CD_BAIRRO"].ToString());
                    p._CodigoEstado = Convert.ToInt32(Dr["CD_ESTADO"].ToString());
                    p._CodigoMunicipio = Convert.ToInt64(Dr["CD_MUNICIPIO"].ToString());
                    p._TipoEndereco = Convert.ToInt32(Dr["TP_ENDERECO"].ToString());

                    p._TipoEnderecoD = RnHabilTipo.DescricaoHabil_Tipo(p._TipoEndereco);

                    p._CodigoInscricao = Convert.ToInt32(Dr["CD_INSCRICAO"]);

                    if (Dr["CD_INSCRICAO"] != DBNull.Value)
                    {
                        p._CodigoInscricao = Convert.ToInt32(Dr["CD_INSCRICAO"]);
                    }

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Endereço da Pessoa: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from Pessoa_ENDERECO Where CD_PESSOA = @v1 ", Con);
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
                            throw new Exception("Erro ao excluir Endereço da Pessoa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Endereço da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Inserir(Int64 CodPessoa, List<Pessoa_Endereco> listCadPessoaEndereco)
        {
            try
            {
                ExcluirTodos(CodPessoa);

                AbrirConexao();
                foreach (Pessoa_Endereco p in listCadPessoaEndereco)
                {

                    Cmd = new SqlCommand("insert into PESSOA_ENDERECO (CD_PESSOA, CD_ENDERECO, TX_LOGRADOURO, NR_ENDERECO, TX_COMPLEMENTO, CD_CEP, CD_ESTADO, CD_MUNICIPIO, CD_BAIRRO, DS_ESTADO, DS_MUNICIPIO, DS_BAIRRO, TP_ENDERECO, CD_INSCRICAO) values (@v1,@v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10, @v11, @v12, @v13, @v14)", Con);
                    Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                    Cmd.Parameters.AddWithValue("@v2", p._CodigoItem);
                    Cmd.Parameters.AddWithValue("@v3", p._Logradouro);
                    Cmd.Parameters.AddWithValue("@v4", p._NumeroLogradouro);
                    Cmd.Parameters.AddWithValue("@v5", p._Complemento);
                    Cmd.Parameters.AddWithValue("@v6", p._CodigoCEP);
                    Cmd.Parameters.AddWithValue("@v7", p._CodigoEstado);
                    Cmd.Parameters.AddWithValue("@v8", p._CodigoMunicipio);
                    Cmd.Parameters.AddWithValue("@v9", p._CodigoBairro);
                    Cmd.Parameters.AddWithValue("@v10", p._DescricaoEstado);
                    Cmd.Parameters.AddWithValue("@v11", p._DescricaoMunicipio);
                    Cmd.Parameters.AddWithValue("@v12", p._DescricaoBairro);
                    Cmd.Parameters.AddWithValue("@v13", p._TipoEndereco);
                    Cmd.Parameters.AddWithValue("@v14", p._CodigoInscricao);
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
                            throw new Exception("Erro ao gravar Endereço da Pessoa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Endereço da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Pessoa_Endereco> ObterPessoaEnderecos(Int64 CodPessoa)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from PESSOA_ENDERECO Where CD_PESSOA = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                Dr = Cmd.ExecuteReader();
                List<Pessoa_Endereco> lista = new List<Pessoa_Endereco>();

                while (Dr.Read())
                {
                    Pessoa_Endereco p = new Pessoa_Endereco();
                    p._CodigoItem = Convert.ToInt32(Dr["CD_ENDERECO"]);
                    p._Logradouro = Dr["TX_LOGRADOURO"].ToString();
                    p._NumeroLogradouro = Dr["NR_ENDERECO"].ToString();
                    p._Complemento = Dr["TX_COMPLEMENTO"].ToString();
                    p._CodigoCEP = Convert.ToInt64(Dr["CD_CEP"]);
                    p._DescricaoEstado = Dr["DS_ESTADO"].ToString();
                    p._DescricaoMunicipio = Dr["DS_MUNICIPIO"].ToString();
                    p._DescricaoBairro = Dr["DS_BAIRRO"].ToString();
                    p._CodigoBairro = Convert.ToInt32(Dr["CD_BAIRRO"].ToString());
                    p._CodigoEstado = Convert.ToInt32(Dr["CD_ESTADO"].ToString());
                    p._CodigoMunicipio = Convert.ToInt64(Dr["CD_MUNICIPIO"].ToString());
                    p._TipoEndereco = Convert.ToInt32(Dr["TP_ENDERECO"].ToString());

                    p._TipoEnderecoD = RnHabilTipo.DescricaoHabil_Tipo(p._TipoEndereco);

                    if (Dr["CD_INSCRICAO"] != DBNull.Value)
                    { 
                        p._CodigoInscricao = Convert.ToInt32(Dr["CD_INSCRICAO"]);
                    }

                    lista.Add(p); 
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter Endereços da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Pessoa_Endereco> ListarPessoaEnderecos(List<DBTabelaCampos> ListaFiltros, short shtTipoPessoa)
        {
            try
            {
                AbrirConexao();
                string strValor = "";

                string strSQL = "Select  IP.*, T.DS_TIPO " +
                    " from PESSOA_ENDERECO as IP" +
                    "     Inner Join Pessoa as P" +
                    "         on IP.CD_PESSOA = P.CD_PESSOA" +
                    "     Inner Join HABIL_TIPO as T" +
                    "         on IP.TP_ENDERECO = T.CD_TIPO" +

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
                List<Pessoa_Endereco> lista = new List<Pessoa_Endereco>();

                while (Dr.Read())
                {
                    Pessoa_Endereco p = new Pessoa_Endereco();
                    p._CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p._CodigoItem = Convert.ToInt32(Dr["CD_ENDERECO"]);
                    p._Logradouro = Dr["TX_LOGRADOURO"].ToString();
                    p._NumeroLogradouro = Dr["NR_ENDERECO"].ToString();
                    p._Complemento = Dr["TX_COMPLEMENTO"].ToString();
                    p._CodigoCEP = Convert.ToInt64(Dr["CD_CEP"]);
                    p._DescricaoEstado = Dr["DS_ESTADO"].ToString();
                    p._DescricaoMunicipio = Dr["DS_MUNICIPIO"].ToString();
                    p._DescricaoBairro = Dr["DS_BAIRRO"].ToString();
                    p._CodigoBairro = Convert.ToInt32(Dr["CD_BAIRRO"].ToString());
                    p._CodigoEstado = Convert.ToInt32(Dr["CD_ESTADO"].ToString());
                    p._CodigoMunicipio = Convert.ToInt64(Dr["CD_MUNICIPIO"].ToString());
                    p._TipoEndereco = Convert.ToInt32(Dr["TP_ENDERECO"].ToString());

                    p._TipoEnderecoD = Dr["DS_TIPO"].ToString();

                    if (Dr["CD_INSCRICAO"] != DBNull.Value)
                    {
                        p._CodigoInscricao = Convert.ToInt32(Dr["CD_INSCRICAO"]);
                    }

                    lista.Add(p);
                }
                Dr.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter Endereços da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
    }
}
