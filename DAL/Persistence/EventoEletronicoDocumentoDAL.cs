using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class EventoEletronicoDocumentoDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(List<EventoEletronicoDocumento> ListaEventoDocumento, decimal CodigoDocumento)
        {
            try
            {
                ExcluirTodos(CodigoDocumento);
                AbrirConexao();
                foreach (EventoEletronicoDocumento doc in ListaEventoDocumento)
                {
                    AbrirConexao();
                    Cmd = new SqlCommand("insert into EVENTO_ELETRONICO_DO_DOCUMENTO (CD_DOCUMENTO, " +
                                                                        "CD_EVENTO," +
                                                                        "CD_SITUACAO," +
                                                                        "DT_HR_EVENTO," +
                                                                        "CD_MAQUINA," +
                                                                        "CD_USUARIO," +
                                                                        "NM_SEQUENCIA," +
                                                                        "TX_MOTIVO," +
                                                                        "CD_TP_EVENTO," +
                                                                        "TX_RETORNO) " +
                                                                    "values (@v1,@v2,@v3,@v4, @v5, @v6,@v7,@v8,@v9,@v10);", Con);
                    Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                    Cmd.Parameters.AddWithValue("@v2", doc.CodigoEvento);
                    Cmd.Parameters.AddWithValue("@v3", doc.CodigoSituacao);
                    Cmd.Parameters.AddWithValue("@v4", doc.DataHoraEvento);
                    Cmd.Parameters.AddWithValue("@v5", doc.CodigoMaquina);
                    Cmd.Parameters.AddWithValue("@v6", doc.CodigoUsuario);
                    Cmd.Parameters.AddWithValue("@v7", doc.NumeroSequencia);
                    Cmd.Parameters.AddWithValue("@v8", doc.Motivo);
                    Cmd.Parameters.AddWithValue("@v9", doc.CodigoTipoEvento);
                    Cmd.Parameters.AddWithValue("@v10", doc.Retorno);
                    Cmd.ExecuteNonQuery();
                }
               
                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar evento eletronico: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void ExcluirTodos(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from EVENTO_ELETRONICO_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
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
                            throw new Exception("Erro ao excluir EVENTO ELETRONICO DO DOCUMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir EVENTO ELETRONICO DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<EventoEletronicoDocumento> ObterEventosEletronicos(decimal CodDocumento)
        {
            try
            {

                AbrirConexao();
                string comando = "Select * from EVENTO_ELETRONICO_DO_DOCUMENTO Where CD_DOCUMENTO = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();
                List<EventoEletronicoDocumento> evento = new List<EventoEletronicoDocumento>();

                while (Dr.Read())
                {
                    EventoEletronicoDocumento p = new EventoEletronicoDocumento();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEvento = Convert.ToInt32(Dr["CD_EVENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DataHoraEvento = Convert.ToDateTime(Dr["DT_HR_EVENTO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.NumeroSequencia = Convert.ToInt32(Dr["NM_SEQUENCIA"]);
                    p.Motivo = Dr["TX_MOTIVO"].ToString();
                    p.CodigoTipoEvento = Convert.ToInt32(Dr["CD_TP_EVENTO"]);
                    p.Retorno = Dr["TX_RETORNO"].ToString();

                    UsuarioDAL usuarioDAL = new UsuarioDAL();
                    Usuario usuario = new Usuario();
                    usuario = usuarioDAL.PesquisarUsuario(Convert.ToInt32(Dr["CD_USUARIO"]));
                    p.Cpl_NomeUsuario = usuario.NomeUsuario;

                    Habil_EstacaoDAL EstacaoDAL = new Habil_EstacaoDAL();
                    Habil_Estacao Estacao = new Habil_Estacao();
                    Estacao = EstacaoDAL.PesquisarCodigoHabil_Estacao(Convert.ToInt32(Dr["CD_MAQUINA"]));
                    p.Cpl_NomeMaquina= Estacao.NomeEstacao;

                    Habil_Tipo situ = new Habil_Tipo();
                    Habil_TipoDAL situDAL = new Habil_TipoDAL();
                    situ = situDAL.PesquisarHabil_Tipo(Convert.ToInt32(Dr["CD_SITUACAO"]));
                    if (situ != null)
                        p.Cpl_Situacao = situ.DescricaoTipo;

                    Habil_Tipo tpEvento = new Habil_Tipo();
                    tpEvento = situDAL.PesquisarHabil_Tipo(Convert.ToInt32(Dr["CD_TP_EVENTO"]));
                    if (tpEvento != null)
                        p.Cpl_TipoEvento = tpEvento.DescricaoTipo;

                    evento.Add(p);
                }
                return evento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar EVENTOS ELETRONICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void AtualizarEventoEletronico(EventoEletronicoDocumento evento)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [EVENTO_ELETRONICO_DO_DOCUMENTO] set CD_SITUACAO = @v3, TX_RETORNO = @v4 Where [CD_DOCUMENTO] = @v1 and [CD_EVENTO] = @v2";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", evento.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", evento.CodigoEvento);
                Cmd.Parameters.AddWithValue("@v3", evento.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", evento.Retorno);

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
    }
}
