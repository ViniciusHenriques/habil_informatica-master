using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;
using System.Linq;

namespace DAL.Persistence
{
    public class AnexoAgendamentoDAL:Conexao
    {

        String strSQL = "";
        
        public void Inserir(decimal CodigoAgendamento, List<AnexoAgendamento> listaAnexo)
        {
            try
            {
                ExcluirTodos(CodigoAgendamento);
                AbrirConexao();

				if (listaAnexo == null)
					return;
				foreach (AnexoAgendamento p in listaAnexo)
                {
                    Cmd = new SqlCommand("insert into ANEXO_DO_AGENDAMENTO (CD_AGENDAMENTO, " +
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

                    Cmd.Parameters.AddWithValue("@v1", CodigoAgendamento);
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
                            throw new Exception("Erro ao gravar ANEXO DO AGENDAMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar ANEXO DO AGENDAMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodos(decimal CodigoAgendamento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from ANEXO_DO_AGENDAMENTO Where CD_AGENDAMENTO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoAgendamento);
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
                            throw new Exception("Erro ao excluir ANEXOS DO AGENDAMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir ANEXOS AGENDAMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<AnexoAgendamento> ObterAnexos(decimal CodigoAgendamento)
        {
            try
            {

                AbrirConexao();
                string comando = "Select * from ANEXO_DO_AGENDAMENTO Where CD_AGENDAMENTO = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoAgendamento);

                Dr = Cmd.ExecuteReader();
                List<AnexoAgendamento> anexo = new List<AnexoAgendamento>();

                while (Dr.Read())
                {
                    AnexoAgendamento p = new AnexoAgendamento();
                    p.CodigoAgendamento = Convert.ToDecimal(Dr["CD_AGENDAMENTO"]);
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
                throw new Exception("Erro ao Pesquisar anexos do agendamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
