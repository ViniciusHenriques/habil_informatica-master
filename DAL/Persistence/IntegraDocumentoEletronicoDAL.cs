using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class IntegraDocumentoEletronicoDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(IntegraDocumentoEletronico p)
        {
            try
            {
                AbrirConexao();
                strSQL = "insert into INTEGRA_DOCUMENTO_ELETRONICO (CD_DOCUMENTO," +
                                                                   "IN_REG_ENVIADO," +
                                                                   "IN_INTEGRA_RECEBIDO," +
                                                                   "IN_INTEGRA_PROCESSANDO," +
                                                                   "IN_INTEGRA_RETORNO," +
                                                                   "IN_REG_DEVOLVIDO," +
                                                                   "IN_REG_MENSAGEM," +
                                                                   "TX_MSG," +
                                                                   "CD_ACAO," +
                                                                   "CD_MAQUINA," +
                                                                   "CD_USUARIO) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11) ";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.RegistroEnviado);
                Cmd.Parameters.AddWithValue("@v3", p.IntegracaoRecebido);
                Cmd.Parameters.AddWithValue("@v4", p.IntegracaoProcessando);
                Cmd.Parameters.AddWithValue("@v5", p.IntegracaoRetorno);
                Cmd.Parameters.AddWithValue("@v6", p.RegistroDevolvido);
                Cmd.Parameters.AddWithValue("@v7", p.RegistroMensagem);
                Cmd.Parameters.AddWithValue("@v8", p.Mensagem);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoAcao);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoMaquina);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoUsuario);

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
                            throw new Exception("Erro ao Incluir INTEGRA DOCUMENTO ELETRONICO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar INTEGRA DOCUMENTO ELETRONICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(decimal Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from INTEGRA_DOCUMENTO_ELETRONICO Where CD_INDEX= @v1", Con);
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
                            throw new Exception("Erro ao excluir INTEGRA DOCUMENTO ELETRONICO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir INTEGRA DOCUMENTO ELETRONICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirPorDocumento(decimal Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from INTEGRA_DOCUMENTO_ELETRONICO Where CD_DOCUMENTO = @v1", Con);
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
                            throw new Exception("Erro ao excluir INTEGRA DOCUMENTO ELETRONICO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir INTEGRA DOCUMENTO ELETRONICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<IntegraDocumentoEletronico> ListarIntegracaoDocEletronicoCompleto(List<DBTabelaCampos> ListaFiltros)

        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [INTEGRA_DOCUMENTO_ELETRONICO]";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                strSQL += " order by CD_INDEX  desc ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<IntegraDocumentoEletronico> lista = new List<IntegraDocumentoEletronico>();

                while (Dr.Read())
                {
                    IntegraDocumentoEletronico p = new IntegraDocumentoEletronico();
                    p.Codigo = Convert.ToDecimal(Dr["CD_INDEX"]);
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.RegistroEnviado = Convert.ToInt32(Dr["IN_REG_ENVIADO"]);
                    p.IntegracaoRecebido = Convert.ToInt32(Dr["IN_INTEGRA_RECEBIDO"]);
                    p.IntegracaoProcessando = Convert.ToInt32(Dr["IN_INTEGRA_PROCESSANDO"]);
                    p.IntegracaoRetorno = Convert.ToInt32(Dr["IN_INTEGRA_RETORNO"]);
                    p.RegistroDevolvido = Convert.ToInt32(Dr["IN_REG_DEVOLVIDO"]);
                    p.RegistroMensagem = Convert.ToInt32(Dr["IN_REG_MENSAGEM"]);
                    p.CodigoAcao = Convert.ToInt32(Dr["CD_ACAO"]);
                    p.Mensagem = Dr["TX_MSG"].ToString();
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"].ToString());
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"].ToString());

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar TODAS NOTAS FISCAIS DE SERVICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void AtualizarDocumentoNFSe(Doc_NotaFiscalServico p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_CHAVE_ACESSO = @v2, NR_PROTOCOLO = @v3, CD_SITUACAO = @v4  Where [CD_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoNotaFiscalServico);
                Cmd.Parameters.AddWithValue("@v2", p.ChaveAcesso);
                Cmd.Parameters.AddWithValue("@v3", p.Protocolo);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar NOTA FISCAL DE SERVIÇO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void AtualizarSituacaoDocumentoCTe(Doc_CTe p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCUMENTO] set CD_SITUACAO = @v3 Where [CD_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar CTE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void AtualizarIntegraDocEletronico(IntegraDocumentoEletronico p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [INTEGRA_DOCUMENTO_ELETRONICO] set IN_REG_ENVIADO = @v2," +
                                                                   "IN_INTEGRA_RECEBIDO = @v3," +
                                                                   "IN_INTEGRA_PROCESSANDO = @v4," +
                                                                   "IN_INTEGRA_RETORNO = @v5," +
                                                                   "IN_REG_DEVOLVIDO = @v6," +
                                                                   "IN_REG_MENSAGEM = @v7," +
                                                                   "TX_MSG = @v8," +
                                                                   "CD_ACAO = @v9," +
                                                                   "CD_MAQUINA = @v10," +
                                                                   "CD_USUARIO = @v11  Where [CD_INDEX] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.Codigo);
                Cmd.Parameters.AddWithValue("@v2", p.RegistroEnviado);
                Cmd.Parameters.AddWithValue("@v3", p.IntegracaoRecebido);
                Cmd.Parameters.AddWithValue("@v4", p.IntegracaoProcessando);
                Cmd.Parameters.AddWithValue("@v5", p.IntegracaoRetorno);
                Cmd.Parameters.AddWithValue("@v6", p.RegistroDevolvido);
                Cmd.Parameters.AddWithValue("@v7", p.RegistroMensagem);
                Cmd.Parameters.AddWithValue("@v8", p.Mensagem);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoAcao);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoMaquina);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoUsuario);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar integra documento eletronico: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public IntegraDocumentoEletronico PesquisarIntegracaoDocEletronico(decimal Codigo, int CodigoAcao)
        {
            try
            {

                AbrirConexao();
                strSQL = "Select * from [INTEGRA_DOCUMENTO_ELETRONICO] Where CD_DOCUMENTO= @v1";
                if(CodigoAcao != 0)
                {
                    strSQL = strSQL + " and CD_ACAO = "+CodigoAcao;
                }
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);


                Dr = Cmd.ExecuteReader();

                IntegraDocumentoEletronico integraDocumento = null;

                if (Dr.Read())
                {
                    integraDocumento = new IntegraDocumentoEletronico(); 
                    integraDocumento.Codigo = Convert.ToInt64(Dr["CD_INDEX"]);
                    integraDocumento.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    integraDocumento.RegistroEnviado = Convert.ToInt32(Dr["IN_REG_ENVIADO"]);
                    integraDocumento.IntegracaoRecebido = Convert.ToInt32(Dr["IN_INTEGRA_RECEBIDO"]);
                    integraDocumento.IntegracaoProcessando = Convert.ToInt32(Dr["IN_INTEGRA_PROCESSANDO"]);
                    integraDocumento.IntegracaoRetorno = Convert.ToInt32(Dr["IN_INTEGRA_RETORNO"]);
                    integraDocumento.RegistroDevolvido = Convert.ToInt32(Dr["IN_REG_DEVOLVIDO"]);
                    integraDocumento.RegistroMensagem = Convert.ToInt32(Dr["IN_REG_MENSAGEM"]);
                    integraDocumento.Mensagem = Dr["TX_MSG"].ToString();
                    integraDocumento.CodigoAcao = Convert.ToInt32(Dr["CD_ACAO"]);
                    integraDocumento.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    integraDocumento.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);

                }

                return integraDocumento;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar NOTA FISCAL DE SERVICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public IntegraDocumentoEletronico PesquisarIntegracaoDocEletronicoPorCodigo(decimal Codigo, int CodigoAcao)
        {
            try
            {

                AbrirConexao();
                strSQL = "Select * from [INTEGRA_DOCUMENTO_ELETRONICO] Where CD_INDEX = @v1";
                if (CodigoAcao != 0)
                {
                    strSQL = strSQL + " and CD_ACAO = " + CodigoAcao;
                }
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);


                Dr = Cmd.ExecuteReader();

                IntegraDocumentoEletronico integraDocumento = null;

                if (Dr.Read())
                {
                    integraDocumento = new IntegraDocumentoEletronico();
                    integraDocumento.Codigo = Convert.ToInt64(Dr["CD_INDEX"]);
                    integraDocumento.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    integraDocumento.RegistroEnviado = Convert.ToInt32(Dr["IN_REG_ENVIADO"]);
                    integraDocumento.IntegracaoRecebido = Convert.ToInt32(Dr["IN_INTEGRA_RECEBIDO"]);
                    integraDocumento.IntegracaoProcessando = Convert.ToInt32(Dr["IN_INTEGRA_PROCESSANDO"]);
                    integraDocumento.IntegracaoRetorno = Convert.ToInt32(Dr["IN_INTEGRA_RETORNO"]);
                    integraDocumento.RegistroDevolvido = Convert.ToInt32(Dr["IN_REG_DEVOLVIDO"]);
                    integraDocumento.RegistroMensagem = Convert.ToInt32(Dr["IN_REG_MENSAGEM"]);
                    integraDocumento.Mensagem = Dr["TX_MSG"].ToString();
                    integraDocumento.CodigoAcao = Convert.ToInt32(Dr["CD_ACAO"]);
                    integraDocumento.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    integraDocumento.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);

                }

                return integraDocumento;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar NOTA FISCAL DE SERVICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<IntegraDocumentoEletronico> ListarIntegraDocEletronico(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem, int intCodigoAcao)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "Select * from [INTEGRA_DOCUMENTO_ELETRONICO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);
                if (intCodigoAcao != 0)
                    strSQL = strSQL + " and CD_ACAO =" + intCodigoAcao;
                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<IntegraDocumentoEletronico> lista = new List<IntegraDocumentoEletronico>();

                while (Dr.Read())
                {
                    IntegraDocumentoEletronico p = new IntegraDocumentoEletronico();
                    p.Codigo = Convert.ToDecimal(Dr["CD_INDEX"]);
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.RegistroEnviado = Convert.ToInt32(Dr["IN_REG_ENVIADO"]);
                    p.IntegracaoRecebido = Convert.ToInt32(Dr["IN_INTEGRA_RECEBIDO"]);
                    p.IntegracaoProcessando = Convert.ToInt32(Dr["IN_INTEGRA_PROCESSANDO"]);
                    p.IntegracaoRetorno = Convert.ToInt32(Dr["IN_INTEGRA_RETORNO"]);
                    p.RegistroDevolvido = Convert.ToInt32(Dr["IN_REG_DEVOLVIDO"]);
                    p.RegistroMensagem = Convert.ToInt32(Dr["IN_REG_MENSAGEM"]);
                    p.CodigoAcao = Convert.ToInt32(Dr["CD_ACAO"]);
                    p.Mensagem = Dr["TX_MSG"].ToString();
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao LISTA INTEGRA DOCUMENTO ELETRONICO : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<IntegraDocumentoEletronico> ListarIntegraDocEletronicoRejeitados(int RegistroMensagem, int Decrescente, int CodigoUsuario)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "Select * from [INTEGRA_DOCUMENTO_ELETRONICO] ";

                if(RegistroMensagem != 0)
                {
                    strSQL = strSQL + "where IN_REG_MENSAGEM <= 5 and TX_MSG != '' and TX_MSG != 'AUTORIZADA' and TX_MSG != 'CANCELADA' and CD_USUARIO = " + CodigoUsuario;
                }
                else
                {
                    strSQL = strSQL + "where TX_MSG != '' and TX_MSG != 'AUTORIZADA' and TX_MSG != 'CANCELADA' and CD_USUARIO = " + CodigoUsuario;
                }
                if(Decrescente == 1)
                    strSQL = strSQL + " ORDER BY CD_INDEX DESC";
                else
                    strSQL = strSQL + " ORDER BY CD_INDEX ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<IntegraDocumentoEletronico> lista = new List<IntegraDocumentoEletronico>();

                while (Dr.Read())
                {
                    IntegraDocumentoEletronico p = new IntegraDocumentoEletronico();
                    p.Codigo = Convert.ToDecimal(Dr["CD_INDEX"]);
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.RegistroEnviado = Convert.ToInt32(Dr["IN_REG_ENVIADO"]);
                    p.IntegracaoRecebido = Convert.ToInt32(Dr["IN_INTEGRA_RECEBIDO"]);
                    p.IntegracaoProcessando = Convert.ToInt32(Dr["IN_INTEGRA_PROCESSANDO"]);
                    p.IntegracaoRetorno = Convert.ToInt32(Dr["IN_INTEGRA_RETORNO"]);
                    p.RegistroDevolvido = Convert.ToInt32(Dr["IN_REG_DEVOLVIDO"]);
                    p.RegistroMensagem = Convert.ToInt32(Dr["IN_REG_MENSAGEM"]);
                    p.CodigoAcao = Convert.ToInt32(Dr["CD_ACAO"]);
                    p.Mensagem = Dr["TX_MSG"].ToString();
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao LISTA INTEGRA DOCUMENTO ELETRONICO : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }


    }
}
