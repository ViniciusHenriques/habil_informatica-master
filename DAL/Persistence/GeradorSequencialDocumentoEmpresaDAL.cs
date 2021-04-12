using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
namespace DAL.Persistence
{
    public class GeradorSequencialDocumentoEmpresaDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(Int64 Codigo, List<GeradorSequencialDocumentoEmpresa> listaGeradorSequencialDocumento)
        {
            try
            {
                ExcluirTodos(Codigo);
                AbrirConexao();
                foreach (GeradorSequencialDocumentoEmpresa p in listaGeradorSequencialDocumento)
                {
                    strSQL = "insert into GERADOR_SEQ_DOC_EMPRESA (CD_EMPRESA, CD_GER_SEQ_DOC) values (@v1,@v2)";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1",p.CodigoEmpresa);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoGeradorSequencialDocumento);


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
                            throw new Exception("Erro ao Incluir GERADOR SEQUENCIAL DOCUMENTO EMPRESA: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar GERADOR SEQUENCIAL DOCUMENTO EMPRESA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
       
        
        public void ExcluirTodos(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from GERADOR_SEQ_DOC_EMPRESA Where CD_EMPRESA = @v1 ", Con);
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
                            throw new Exception("Erro ao excluir GERADOR SEQUENCIAL DOCUMENTO EMPRESA: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir GERADOR SEQUENCIAL DOCUMENTO EMPRESA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<GeradorSequencialDocumentoEmpresa> ObterGeradorSequencialDocumentoEmpresa(Int32 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from GERADOR_SEQ_DOC_EMPRESA Where CD_EMPRESA = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                List<GeradorSequencialDocumentoEmpresa> lista = new List<GeradorSequencialDocumentoEmpresa>();

                while (Dr.Read())
                {
                    GeradorSequencialDocumentoEmpresa p = new GeradorSequencialDocumentoEmpresa();
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.CodigoGeradorSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);

                    GeracaoSequencialDocumento gerador = new GeracaoSequencialDocumento();
                    GeracaoSequencialDocumentoDAL geradorDAL = new GeracaoSequencialDocumentoDAL();
                    gerador = geradorDAL.PesquisarGeradorSequencial(p.CodigoGeradorSequencialDocumento);

                    p.Cpl_Descricao = gerador.Descricao;
                    p.Cpl_SerieConteudo = gerador.SerieConteudo;
                    p.Cpl_SerieNumero = gerador.SerieNumero;


                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter GERADOR SEQUENCIAL DOCUMENTO EMPRESA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public GeradorSequencialDocumentoEmpresa PesquisarGeradorSequencialEmpresa(int Codigo, int CodigoTipoDocumento)
        {
            try
            {
                AbrirConexao();
                strSQL = "SELECT dbo.GERADOR_SEQ_DOC_EMPRESA.CD_EMPRESA, dbo.GERADOR_SEQ_DOC_EMPRESA.CD_GER_SEQ_DOC, dbo.GERADOR_SEQUENCIAL_DOCUMENTO.CD_TIPO_DOCUMENTO, dbo.GERADOR_SEQUENCIAL_DOCUMENTO.NOME " +
                                "FROM dbo.GERADOR_SEQ_DOC_EMPRESA INNER JOIN " +
                                    "dbo.GERADOR_SEQUENCIAL_DOCUMENTO ON dbo.GERADOR_SEQUENCIAL_DOCUMENTO.CD_GER_SEQ_DOC = dbo.GERADOR_SEQ_DOC_EMPRESA.CD_GER_SEQ_DOC " +
                                    "WHERE dbo.GERADOR_SEQ_DOC_EMPRESA.CD_EMPRESA = @v1  and GERADOR_SEQUENCIAL_DOCUMENTO.CD_TIPO_DOCUMENTO = @v2";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Cmd.Parameters.AddWithValue("@v2", CodigoTipoDocumento);

                Dr = Cmd.ExecuteReader();

                GeradorSequencialDocumentoEmpresa p = new GeradorSequencialDocumentoEmpresa();

                if (Dr.Read())
                {


                    p.CodigoGeradorSequencialDocumento = Convert.ToInt32(Dr["CD_GER_SEQ_DOC"]);
                    p.Cpl_TipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    p.Cpl_Nome = Dr["NOME"].ToString();



                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar GERADOR SEQUENCIAL DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public double ExibeProximoNroSequencial(string nomeTabela)
        {

            try
            {

                AbrirConexao();

                string strSQL = " SELECT (ISNULL(MAX(NR_SEQUENCIAL + 1),0) ) AS NR_SEQ_NOW  FROM " + nomeTabela ;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();


                if (Dr.Read() && Dr["NR_SEQ_NOW"].ToString() != null)
                    
                    return Convert.ToDouble(Dr["NR_SEQ_NOW"]);
                else
                    return 0;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public decimal ListaNrosVagos(string strNomeTabela, int CodigoUsuario, int CodigoMaquina)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "Select * from "+strNomeTabela+" where CD_USADO = @v1 and CD_USUARIO = @v2 and CD_ESTACAO = @v3";


                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", 0);
                Cmd.Parameters.AddWithValue("@v2", CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v3", CodigoMaquina);
                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    return Convert.ToDecimal(Dr["NR_SEQUENCIAL"]);
                    
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Empresa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public decimal IncluirTabelaGerador(string nomeTabela, int intCodigoSeq, int intCodUsuario, int intCodMaquina)
        {

            try
            {
                decimal NroVago = ListaNrosVagos(nomeTabela, intCodUsuario, intCodMaquina);

                AbrirConexao();
                if (NroVago != 0)
                {
                    string strSQL = " UPDATE " + nomeTabela + " SET CD_USADO = @v2 WHERE NR_SEQUENCIAL = @v1";

                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v1", NroVago);
                    Cmd.Parameters.AddWithValue("@v2", 1);
                    Cmd.ExecuteNonQuery();
                    return NroVago;
                }
                else
                {
                    string strSQL = " INSERT INTO " + nomeTabela + " VALUES(@v1,@v2,@v3,@v4) SELECT SCOPE_IDENTITY();";

                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v1", intCodigoSeq);
                    Cmd.Parameters.AddWithValue("@v2", intCodUsuario);
                    Cmd.Parameters.AddWithValue("@v3", intCodMaquina);
                    Cmd.Parameters.AddWithValue("@v4", 1);
                    return Convert.ToDecimal(Cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao incluir Dados na Tabela Geradora: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void AtualizarTabelaGeradora(decimal NumeroSequencial, string strNomeTabela)
        {

            try
            {
                AbrirConexao();

                string strSQL = " UPDATE " + strNomeTabela + " SET CD_USADO = @v2 WHERE NR_SEQUENCIAL = @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", NumeroSequencial);
                Cmd.Parameters.AddWithValue("@v2", 0);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao incluir Dados na Tabela Geradora: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
