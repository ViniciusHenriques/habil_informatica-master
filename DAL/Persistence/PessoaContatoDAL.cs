using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class PessoaContatoDAL : Conexao
    {
        Habil_TipoDAL RnHabilTipo = new Habil_TipoDAL();

        public Pessoa_Contato PesquisarPessoaContato(long CodPessoa, Int32 CodItem)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from PESSOA_CONTATO Where CD_PESSOA = @v1 AND CD_CONTATO = @v2", Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                Cmd.Parameters.AddWithValue("@v2", CodItem);
                Dr = Cmd.ExecuteReader();
                Pessoa_Contato p = new Pessoa_Contato();

                if (Dr.Read())
                {
                    p._CodigoItem = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p._TipoContato = Convert.ToInt32(Dr["TP_CONTATO"].ToString());
                    p._NomeContato = Dr["NM_CONTATO"].ToString();
                    p._Fone1 = Dr["NR_FONE1"].ToString();
                    p._Fone2 = Dr["NR_FONE2"].ToString();
                    p._Fone3 = Dr["NR_FONE3"].ToString();
                    p._MailNFE = Dr["TX_MAILNFE"].ToString();
                    p._MailNFSE = Dr["TX_MAILNFSE"].ToString();
                    p._Mail1 = Dr["TX_MAIL1"].ToString();
                    p._Mail2 = Dr["TX_MAIL2"].ToString();
                    p._Mail3 = Dr["TX_MAIL3"].ToString();
                    p._EmailSenha = Dr["TX_MAIL_SENHA"].ToString();
                    p._FoneWhatsApp = Convert.ToInt32(Dr["IN_WHATS"]);
                    p._CodigoPais = Convert.ToInt32(Dr["CD_PAIS"]);

                    p._TipoContatoD = RnHabilTipo.DescricaoHabil_Tipo(p._TipoContato);

                    p._NomeContatoCombo = p._NomeContato + "(" + p._CodigoItem.ToString() + ")";

                    if ((Dr["IM_FOTO"]) != DBNull.Value)
                        p._Foto = (byte[])(Dr["IM_FOTO"]);

                    if (Dr["CD_INSCRICAO"] != DBNull.Value)
                    {
                        p._CodigoInscricao = Convert.ToInt32(Dr["CD_INSCRICAO"]);
                    }
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Contato da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodos(long CodPessoa)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from Pessoa_CONTATO Where CD_PESSOA = @v1 ", Con);
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
                            throw new Exception("Erro ao excluir Contato da Pessoa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Contato da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Inserir(long CodPessoa, List<Pessoa_Contato> listCadPessoaContato)
        {
            try
            {
                ExcluirTodos(CodPessoa);

                AbrirConexao();

                string strSQL = "";
                foreach (Pessoa_Contato p in listCadPessoaContato)
                {
                    if (p._Foto != null)
                        strSQL = "insert into PESSOA_CONTATO (CD_PESSOA, CD_CONTATO, TP_CONTATO, NM_CONTATO, NR_FONE1, NR_FONE2, NR_FONE3, TX_MAILNFE,TX_MAILNFSE, TX_MAIL1, TX_MAIL2, TX_MAIL3, IM_FOTO, CD_INSCRICAO,TX_MAIL_SENHA,IN_WHATS,CD_PAIS) values (@v1,@v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10, @v11, @v12, @v13, @v14, @v15,@v16,@v17)";
                    else
                        strSQL = "insert into PESSOA_CONTATO (CD_PESSOA, CD_CONTATO, TP_CONTATO, NM_CONTATO, NR_FONE1, NR_FONE2, NR_FONE3, TX_MAILNFE,TX_MAILNFSE, TX_MAIL1, TX_MAIL2, TX_MAIL3, CD_INSCRICAO,TX_MAIL_SENHA,IN_WHATS,CD_PAIS) values (@v1,@v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10, @v11, @v12,@v14, @v15,@v16,@v17)";

                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                    Cmd.Parameters.AddWithValue("@v2", p._CodigoItem);
                    Cmd.Parameters.AddWithValue("@v3", p._TipoContato);
                    Cmd.Parameters.AddWithValue("@v4", p._NomeContato);
                    Cmd.Parameters.AddWithValue("@v5", p._Fone1);
                    Cmd.Parameters.AddWithValue("@v6", p._Fone2);
                    Cmd.Parameters.AddWithValue("@v7", p._Fone3);
                    Cmd.Parameters.AddWithValue("@v8", p._MailNFE);
                    Cmd.Parameters.AddWithValue("@v9", p._MailNFSE);
                    Cmd.Parameters.AddWithValue("@v10", p._Mail1);
                    Cmd.Parameters.AddWithValue("@v11", p._Mail2);
                    Cmd.Parameters.AddWithValue("@v12", p._Mail3);
                    

                    if (p._Foto != null)
                    {
                        Cmd.Parameters.AddWithValue("@v13", p._Foto);
                    }

                    Cmd.Parameters.AddWithValue("@v14", p._CodigoInscricao);
                    Cmd.Parameters.AddWithValue("@v15", p._EmailSenha);
                    Cmd.Parameters.AddWithValue("@v16", p._FoneWhatsApp);
                    Cmd.Parameters.AddWithValue("@v17", p._CodigoPais);
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
                            throw new Exception("Erro ao gravar Contato da Pessoa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Contato da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Pessoa_Contato> ObterPessoaContatos(long CodPessoa)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from PESSOA_CONTATO Where CD_PESSOA = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);
                Dr = Cmd.ExecuteReader();
                List<Pessoa_Contato> lista = new List<Pessoa_Contato>();

                while (Dr.Read())
                {
                    Pessoa_Contato p = new  Pessoa_Contato();
                    p._CodigoItem = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p._TipoContato = Convert.ToInt32(Dr["TP_CONTATO"].ToString());
                    p._NomeContato = Dr["NM_CONTATO"].ToString();
                    p._Fone1 = Dr["NR_FONE1"].ToString();
                    p._Fone2 = Dr["NR_FONE2"].ToString();
                    p._Fone3 = Dr["NR_FONE3"].ToString();
                    p._MailNFE = Dr["TX_MAILNFE"].ToString();
                    p._MailNFSE = Dr["TX_MAILNFSE"].ToString();
                    p._Mail1 = Dr["TX_MAIL1"].ToString();
                    p._Mail2 = Dr["TX_MAIL2"].ToString();
                    p._Mail3 = Dr["TX_MAIL3"].ToString();
                    p._EmailSenha = Dr["TX_MAIL_SENHA"].ToString();
                    p._FoneWhatsApp = Convert.ToInt32(Dr["IN_WHATS"]);
                    p._CodigoPais = Convert.ToInt32(Dr["CD_PAIS"]);

                    p._TipoContatoD = RnHabilTipo.DescricaoHabil_Tipo(p._TipoContato);

                    p._NomeContatoCombo = p._NomeContato + "(" + p._CodigoItem.ToString() + ")";

                    if ((Dr["IM_FOTO"]) != DBNull.Value)
                        p._Foto = (byte[])(Dr["IM_FOTO"]);

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
                throw new Exception("Erro ao Obter Contatos da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public string ObterInscricaoPessoaContato(long CodPessoa, Int32 CodItem)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select CD_INSCRICAO from PESSOA_CONTATO Where CD_PESSOA = @v1 ";
                string strInscricao = "";

                if (CodItem == 0)
                    strSQL += " and TP_Contato = 5";
                else
                    strSQL += " AND CD_CONTATO = @v2";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodPessoa);

                if (CodItem != 0)
                    Cmd.Parameters.AddWithValue("@v2", CodItem);

                Dr = Cmd.ExecuteReader();
                Pessoa_Contato p = new Pessoa_Contato();

                if (Dr.Read())
                {
                    if (Dr["CD_INSCRICAO"] != DBNull.Value)
                    {
                        PessoaInscricaoDAL p_i = new PessoaInscricaoDAL();
                        strInscricao = p_i.ObterInscricao(CodPessoa, Convert.ToInt32(Dr["CD_INSCRICAO"]));
                    }
                }

                String strValor = strInscricao;

                strValor = strValor.Replace(".", "");
                strValor = strValor.Replace("/", "");
                strValor = strValor.Replace("-", "");

                if (strValor.Length < 14)
                {
                    if (strValor.Length < 11)
                        strInscricao = "";
                    else
                        strInscricao = strValor.Substring(0, 3) + "." + strValor.Substring(3, 3) + "." + strValor.Substring(6, 3) + "-" + strValor.Substring(9, 2);
                }
                else
                    strInscricao = strValor.Substring(0, 2) + "." + strValor.Substring(2, 3) + "." + strValor.Substring(5, 3) + "/" + strValor.Substring(8, 4) + "-" + strValor.Substring(12, 2);


                return strInscricao;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Contato da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Pessoa_Contato> ListarPessoaContatos(List<DBTabelaCampos> ListaFiltros, short shtTipoPessoa)
        {
            try
            {
                AbrirConexao();
                string strValor = "";

                string strSQL = "Select  IP.*, T.DS_TIPO " +
                    " from PESSOA_CONTATO as IP" +
                    "     Inner Join Pessoa as P" +
                    "         on IP.CD_PESSOA = P.CD_PESSOA" +
                    "     Inner Join HABIL_TIPO as T" +
                    "         on IP.TP_CONTATO = T.CD_TIPO" +

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
                List<Pessoa_Contato> lista = new List<Pessoa_Contato>();

                while (Dr.Read())
                {
                    Pessoa_Contato p = new Pessoa_Contato();
                    p._CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p._CodigoItem = Convert.ToInt32(Dr["CD_CONTATO"]);
                    p._TipoContato = Convert.ToInt32(Dr["TP_CONTATO"].ToString());
                    p._NomeContato = Dr["NM_CONTATO"].ToString();
                    p._Fone1 = Dr["NR_FONE1"].ToString();
                    p._Fone2 = Dr["NR_FONE2"].ToString();
                    p._Fone3 = Dr["NR_FONE3"].ToString();
                    p._MailNFE = Dr["TX_MAILNFE"].ToString();
                    p._MailNFSE = Dr["TX_MAILNFSE"].ToString();
                    p._Mail1 = Dr["TX_MAIL1"].ToString();
                    p._Mail2 = Dr["TX_MAIL2"].ToString();
                    p._Mail3 = Dr["TX_MAIL3"].ToString();
                    p._EmailSenha = Dr["TX_MAIL_SENHA"].ToString();
                    p._FoneWhatsApp = Convert.ToInt32(Dr["IN_WHATS"]);
                    p._TipoContatoD = Dr["DS_TIPO"].ToString();
                    p._CodigoPais = Convert.ToInt32(Dr["CD_PAIS"]);

                    if ((Dr["IM_FOTO"]) != DBNull.Value)
                        p._Foto = (byte[])(Dr["IM_FOTO"]);

                    if (Dr["CD_INSCRICAO"] != DBNull.Value)
                        p._CodigoInscricao = Convert.ToInt32(Dr["CD_INSCRICAO"]);

                    p._NomeContatoCombo = p._NomeContato + "(" + p._CodigoItem.ToString() + ")";

                    lista.Add(p);
                }
                Dr.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter Contatos da Pessoa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
