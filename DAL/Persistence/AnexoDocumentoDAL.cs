using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;
using System.Linq;

namespace DAL.Persistence
{
    public class AnexoDocumentoDAL:Conexao
    {

        String strSQL = "";
        public string GerarGUID(string extensao)
        {
            try
            {
                AbrirConexao();

                    Cmd = new SqlCommand("Declare @SQLTeste as nVarchar(50) " +
                                         "Set @SQLTeste = LEFT(REPLACE((SELECT NEWID()), '-', '_'), 8)" +
                                         " SELECT 'DCTO_HABIL_' + @SQLTeste + '."+extensao+"'; ", Con);



                    return Convert.ToString( Cmd.ExecuteScalar());


      
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
                            throw new Exception("Erro ao gerar GUID: " + ex.Message.ToString());
                    }
                }
                return "";
            }
            finally
            {
                FecharConexao();
            }
        }
        public string GerarGUIDPorEmpresa(string NomeEmpresa,string extensao)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("Declare @SQLTeste as nVarchar(50) " +
                                     "Set @SQLTeste = LEFT(REPLACE((SELECT NEWID()), '-', '_'), 8)" +
                                     " SELECT 'DCTO_" + NomeEmpresa + "_' + @SQLTeste + '." + extensao + "'; ", Con);



                return Convert.ToString(Cmd.ExecuteScalar());
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
                            throw new Exception("Erro ao gerar GUID: " + ex.Message.ToString());
                    }
                }
                return "";
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Inserir(decimal doc, List<AnexoDocumento> listaAnexo)
        {
            try
            {
                ExcluirTodos(doc);
                AbrirConexao();

				if (listaAnexo == null)
					return;
				foreach (AnexoDocumento p in listaAnexo)
                {
                    Cmd = new SqlCommand("insert into ANEXO_DO_DOCUMENTO (CD_DOCUMENTO, " +
                                                                     "CD_ANEXO," +
                                                                     " DT_HR_LANCAMENTO," +
                                                                     " CD_MAQUINA," +
                                                                     " CD_USUARIO," +
                                                                     " NM_ARQUIVO," +
                                                                     " EX_ARQUIVO," +
                                                                     " TX_CONTEUDO," +
                                                                     " DS_ARQUIVO," +
                                                                     "IN_NAO_EDITAVEL ) " +
                                            "values (@v1,@v2,@v3,@v4, @v5, @v6, @v7, @v8,@v9,@v10);", Con);

                    Cmd.Parameters.AddWithValue("@v1", doc);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoAnexo);
                    Cmd.Parameters.AddWithValue("@v3", p.DataHoraLancamento);
                    Cmd.Parameters.AddWithValue("@v4", p.CodigoMaquina);
                    Cmd.Parameters.AddWithValue("@v5", p.CodigoUsuario);
                    Cmd.Parameters.AddWithValue("@v6", p.NomeArquivo);
                    Cmd.Parameters.AddWithValue("@v7", p.ExtensaoArquivo);
                    Cmd.Parameters.AddWithValue("@v8", p.Arquivo);
                    Cmd.Parameters.AddWithValue("@v9", p.DescricaoArquivo);
                    Cmd.Parameters.AddWithValue("@v10",p.NaoEditavel);
					
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
                            throw new Exception("Erro ao gravar ANEXO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar ANEXO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodos(decimal CodDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from ANEXO_DO_DOCUMENTO Where CD_DOCUMENTO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodDocumento);
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
                            throw new Exception("Erro ao excluir ANEXOS: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir ANEXOS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<AnexoDocumento> ObterAnexos(decimal CodDocumento)
        {
            try
            {

                AbrirConexao();
                string comando = "Select * from ANEXO_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();
                List<AnexoDocumento> anexo = new List<AnexoDocumento>();

                while (Dr.Read())
                {
                    AnexoDocumento p = new AnexoDocumento();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoAnexo = Convert.ToInt32(Dr["CD_ANEXO"]);
                    p.DataHoraLancamento = Convert.ToDateTime(Dr["DT_HR_LANCAMENTO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.NomeArquivo = Convert.ToString(Dr["NM_ARQUIVO"]);
                    p.ExtensaoArquivo= Convert.ToString(Dr["EX_ARQUIVO"]);
                    p.Arquivo = (byte[])(Dr["TX_CONTEUDO"]);
                    p.DescricaoArquivo = Convert.ToString(Dr["DS_ARQUIVO"]);
                    p.NaoEditavel = Convert.ToInt32(Dr["IN_NAO_EDITAVEL"]);

                    UsuarioDAL usuarioDAL = new UsuarioDAL();
                    Usuario usuario = new Usuario();
                    usuario = usuarioDAL.PesquisarUsuario(Convert.ToInt32(Dr["CD_USUARIO"]));
                    p.Cpl_Usuario = usuario.NomeUsuario;

                    Habil_EstacaoDAL EstacaoDAL = new Habil_EstacaoDAL();
                    Habil_Estacao Estacao = new Habil_Estacao();
                    Estacao = EstacaoDAL.PesquisarCodigoHabil_Estacao(Convert.ToInt32(Dr["CD_MAQUINA"]));
                    p.Cpl_Maquina = Estacao.NomeEstacao;



                    anexo.Add(p);
                }
                return anexo;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar anexos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public int PesquisaExtensaoPorNome(string strNomeExtensao)
        {
            try
            {

                AbrirConexao();
                string comando = "Select * from EXTENSAO_DO_ARQUIVO WHERE DS_EXTENSAO = @v1";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strNomeExtensao);
                Dr = Cmd.ExecuteReader();
                int intCodigoExtensao = 0;

                while (Dr.Read())
                {
                    intCodigoExtensao = Convert.ToInt32(Dr["CD_EXTENSAO"].ToString());
                    
                }
                return intCodigoExtensao;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar extensões: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<String> ListarExtensao()
        {
            try
            {

                AbrirConexao();
                string comando = "Select * from EXTENSAO_DO_ARQUIVO ";

                Cmd = new SqlCommand(comando, Con);


                Dr = Cmd.ExecuteReader();
                List<String> extensao = new List<String>();

                while (Dr.Read())
                {
                    String p = Dr["DS_EXTENSAO"].ToString();

                    


                    extensao.Add(p);
                }
                return extensao;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar extensões: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void InserirXMLDocumento(AnexoDocumento anexo)
        {
            try
            {
                
                List<AnexoDocumento> listaAnexosExistentes = new List<AnexoDocumento>();
                listaAnexosExistentes = ObterAnexos(anexo.CodigoDocumento);

                int intCttItem = 0;
                if (listaAnexosExistentes.Count != 0)
                    intCttItem = Convert.ToInt32(listaAnexosExistentes.Max(x => x.CodigoAnexo).ToString());

                AbrirConexao();
                Cmd = new SqlCommand("insert into ANEXO_DO_DOCUMENTO (CD_DOCUMENTO, " +
                                                                    "CD_ANEXO," +
                                                                    " DT_HR_LANCAMENTO," +
                                                                    " CD_MAQUINA," +
                                                                    " CD_USUARIO," +
                                                                    " NM_ARQUIVO," +
                                                                    " EX_ARQUIVO," +
                                                                    " TX_CONTEUDO," +
                                                                    " DS_ARQUIVO," +
                                                                    " IN_NAO_EDITAVEL ) " +
                "values (@v1,@v2,@v3,@v4, @v5, @v6, @v7, @v8,@v9,@v10);", Con);
                Cmd.Parameters.AddWithValue("@v1", anexo.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", intCttItem + 1);
                Cmd.Parameters.AddWithValue("@v3", anexo.DataHoraLancamento);
                Cmd.Parameters.AddWithValue("@v4", anexo.CodigoMaquina);
                Cmd.Parameters.AddWithValue("@v5", anexo.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v6", anexo.NomeArquivo);
                Cmd.Parameters.AddWithValue("@v7", anexo.ExtensaoArquivo);
                Cmd.Parameters.AddWithValue("@v8", anexo.Arquivo);
                Cmd.Parameters.AddWithValue("@v9", anexo.DescricaoArquivo);
                Cmd.Parameters.AddWithValue("@v10",anexo.NaoEditavel);


                //Cmd.ExecuteNonQuery();
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
                            throw new Exception("Erro ao gravar ANEXO 2: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar ANEXO: " + ex.Message.ToString());

            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
