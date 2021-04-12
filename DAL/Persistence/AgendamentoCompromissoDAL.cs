using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class AgendamentoCompromissoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(AgendamentoCompromisso p, List<AnexoAgendamento> ListaAnexo)
        {
            try
            {
                AbrirConexao();
                strSQL = "INSERT INTO [dbo].[AGENDAMENTO_DE_COMPROMISSO]" +
                               " ([DT_HR_AGENDAMENTO]" +
                               " ,[DS_ANOTACAO]" +
                               " ,[CD_SITUACAO]" +
                               " ,[DS_FONE]" +
                               " ,[DS_CONTATO]" +
                               " ,[CD_USUARIO]" +
                               " ,[TX_COR_LEMBRETE]" +
                               " ,[DS_LOCAL]" +
                               " ,[CD_TIPO_AGENDAMENTO]" +
                               " ,[CD_PESSOA]" +
                               " ,[IN_EMAIL]" +
                               " ,[CD_EMPRESA]" +
                               " ,[IN_EMAIL_ENVIADO])" +
                         " VALUES(@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13) SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DataHoraAgendamento);
                Cmd.Parameters.AddWithValue("@v2", p.Anotacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.Telefone);
                Cmd.Parameters.AddWithValue("@v5", p.Contato);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v7", p.CorLembrete);
                Cmd.Parameters.AddWithValue("@v8", p.Local);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoTipoAgendamento);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v11", p.EnviarEmail);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v13", 0);
                p.CodigoIndex = Convert.ToInt32(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao Incluir agendamento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar agendamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirUsuarioAgendamento(p.CodigoIndex, p.ListaUsuario);

                AnexoAgendamentoDAL AnexoDAL = new AnexoAgendamentoDAL();
                AnexoDAL.Inserir(p.CodigoIndex, ListaAnexo);
            }
        }
        public void Atualizar(AgendamentoCompromisso p, List<AnexoAgendamento> ListaAnexo)
        {
            try
            {
                AbrirConexao();


                strSQL = "UPDATE [dbo].[AGENDAMENTO_DE_COMPROMISSO] " +
                           " SET[DT_HR_AGENDAMENTO] = @v1 " +
                              " ,[DS_ANOTACAO] = @v2 " +
                              " ,[CD_SITUACAO] = @v3 " +
                              " ,[DS_FONE] = @v4 " +
                              " ,[DS_CONTATO] = @v5 " +
                              " ,[CD_USUARIO] = @v7 " +
                              " ,[TX_COR_LEMBRETE] = @v8 " +
                              " ,[DS_LOCAL] = @v9 " +
                              " ,[CD_PESSOA] = @v10 " +
                              " ,[CD_TIPO_AGENDAMENTO] = @v11 " +
                              " ,[IN_EMAIL] = @v12 " +
                              " ,[IN_EMAIL_ENVIADO] = @v13 " +
                              " ,[CD_EMPRESA] = @v14 " +
                         " WHERE CD_INDEX = @v6";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.DataHoraAgendamento);
                Cmd.Parameters.AddWithValue("@v2", p.Anotacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.Telefone);
                Cmd.Parameters.AddWithValue("@v5", p.Contato);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoIndex);
                Cmd.Parameters.AddWithValue("@v7", p.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v8", p.CorLembrete);
                Cmd.Parameters.AddWithValue("@v9", p.Local);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoTipoAgendamento);
                Cmd.Parameters.AddWithValue("@v12", p.EnviarEmail);
                Cmd.Parameters.AddWithValue("@v13", 0);
                Cmd.Parameters.AddWithValue("@v14", p.CodigoEmpresa);
                Cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar agendamento de compromissos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                InserirUsuarioAgendamento(p.CodigoIndex, p.ListaUsuario);

                AnexoAgendamentoDAL AnexoDAL = new AnexoAgendamentoDAL();
                AnexoDAL.Inserir(p.CodigoIndex, ListaAnexo);
            }
        }
        public void Cancelar(int Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("UPDATE [AGENDAMENTO_DE_COMPROMISSO] SET CD_SITUACAO = 169 Where [CD_INDEX] = @v1", Con);
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
                            throw new Exception("Erro ao CANCELAR agendamento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao CANCELAR agendamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public AgendamentoCompromisso PesquisarAgendamento(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [AGENDAMENTO_DE_COMPROMISSO] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                AgendamentoCompromisso p = null;

                if (Dr.Read())
                {
                    p = new AgendamentoCompromisso();

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.DataHoraAgendamento = Convert.ToDateTime(Dr["DT_HR_AGENDAMENTO"]);
                    p.Anotacao = Convert.ToString(Dr["DS_ANOTACAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Telefone = Convert.ToString(Dr["DS_FONE"]);
                    p.Contato = Convert.ToString(Dr["DS_CONTATO"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.Local = Convert.ToString(Dr["DS_LOCAL"]);
                    p.CorLembrete = Convert.ToString(Dr["TX_COR_LEMBRETE"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoTipoAgendamento = Convert.ToInt32(Dr["CD_TIPO_AGENDAMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);

                    if (Dr["IN_EMAIL"].ToString() == "1")
                        p.EnviarEmail = true;
                    else
                        p.EnviarEmail = false;
                    p.ListaUsuario = ObterUsuarioAgendamento(Codigo);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar agendamento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<AgendamentoCompromisso> ListarAgendamentoPorDataUsuario(int CodigoUsuario, DateTime DataAgendamento)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select ag.*,HT.DS_TIPO,P.NM_PESSOA,P_USU.NM_PESSOA as NM_USUARIO from AGENDAMENTO_DE_COMPROMISSO  as ag " +
                                    "inner join habil_tipo as ht on ht.CD_TIPO = AG.CD_SITUACAO " +
                                    "INNER JOIN USUARIO AS U ON U.CD_USUARIO = AG.CD_USUARIO " +
                                    "INNER JOIN PESSOA AS P_USU ON P_USU.CD_PESSOA = U.CD_PESSOA " +
                                    "INNER JOIN PESSOA AS P ON P.CD_PESSOA = AG.CD_PESSOA  " +
                                    "left join usuario_do_agendamento as usu on usu.CD_AGENDAMENTO = AG.CD_INDEX " +
                                "WHERE (usu.CD_USUARIO = @v1 or usu.cd_usuario is null ) AND CONVERT(VARCHAR(10), DT_HR_AGENDAMENTO, 111) = Convert(smalldatetime,@v2)";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v2", DataAgendamento.ToString("yyyy-MM-dd"));
                Dr = Cmd.ExecuteReader();

                List<AgendamentoCompromisso> lista = new List<AgendamentoCompromisso>();

                while (Dr.Read())
                {
                    AgendamentoCompromisso p = new AgendamentoCompromisso();

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.DataHoraAgendamento = Convert.ToDateTime(Dr["DT_HR_AGENDAMENTO"]);
                    p.Anotacao = Convert.ToString(Dr["DS_ANOTACAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Telefone = Convert.ToString(Dr["DS_FONE"]);
                    p.Contato = Convert.ToString(Dr["DS_CONTATO"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.Local = Convert.ToString(Dr["DS_LOCAL"]);
                    p.CorLembrete = Convert.ToString(Dr["TX_COR_LEMBRETE"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.Cpl_NomeCliente = Dr["NM_PESSOA"].ToString();
                    p.Cpl_NomeUsuario = Dr["NM_USUARIO"].ToString();
                    p.Cpl_DsSituacao = Dr["DS_TIPO"].ToString();
                    p.CodigoTipoAgendamento = Convert.ToInt32(Dr["CD_TIPO_AGENDAMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);

                    if (Dr["IN_EMAIL"].ToString() == "1")
                        p.EnviarEmail = true;
                    else
                        p.EnviarEmail = false;

                    p.BtnEditar = true;
                    if(p.CodigoSituacao != 169)
                    {
                        p.BtnCancelar = true;
                    }

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos agendamentos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<AgendamentoCompromisso> ListarAgendamento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select AGE.*, USU.CD_USUARIO AS CD_USUARIO_PERMITIDO from [AGENDAMENTO_DE_COMPROMISSO] AS AGE " +
                                    "LEFT join USUARIO_DO_AGENDAMENTO AS USU ON USU.CD_AGENDAMENTO = AGE.CD_INDEX ";


                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor) + " OR USU.CD_USUARIO IS NULL AND CD_SITUACAO != 169 ";
                else
                    strSQL += " WHERE CD_SITUACAO != 169 ";

                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<AgendamentoCompromisso> lista = new List<AgendamentoCompromisso>();

                while (Dr.Read())
                {
                    AgendamentoCompromisso p = new AgendamentoCompromisso();

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.DataHoraAgendamento = Convert.ToDateTime(Dr["DT_HR_AGENDAMENTO"]);
                    p.Anotacao = Convert.ToString(Dr["DS_ANOTACAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Telefone = Convert.ToString(Dr["DS_FONE"]);
                    p.Contato = Convert.ToString(Dr["DS_CONTATO"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.Local = Convert.ToString(Dr["DS_LOCAL"]);
                    p.CorLembrete = Convert.ToString(Dr["TX_COR_LEMBRETE"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoTipoAgendamento = Convert.ToInt32(Dr["CD_TIPO_AGENDAMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);

                    if (Dr["IN_EMAIL"].ToString() == "1")
                        p.EnviarEmail = true;
                    else
                        p.EnviarEmail = false;

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos agendamentos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<AgendamentoCompromisso> ListarAgendamentoCliente(int CodigoUsuario, Int64 CodigoCliente, string strFiltro)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select AGE.*, USU.CD_USUARIO AS CD_USUARIO_PERMITIDO from [AGENDAMENTO_DE_COMPROMISSO] AS AGE " +
                                    "LEFT join USUARIO_DO_AGENDAMENTO AS USU ON USU.CD_AGENDAMENTO = AGE.CD_INDEX " +
                                " WHERE  (USU.CD_USUARIO = @v1 OR USU.CD_USUARIO IS NULL) AND AGE.CD_PESSOA = @v2 AND CD_SITUACAO != 169 ";
                if(strFiltro != "")
                    strSQL += "AND (DS_ANOTACAO LIKE '%" + strFiltro + "%' OR DS_CONTATO LIKE '%" + strFiltro + "%' OR DS_LOCAL LIKE '%" + strFiltro + "%')";

                strSQL += " ORDER BY DT_HR_AGENDAMENTO DESC";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v2", CodigoCliente);

                Dr = Cmd.ExecuteReader();

                List<AgendamentoCompromisso> lista = new List<AgendamentoCompromisso>();

                while (Dr.Read())
                {
                    AgendamentoCompromisso p = new AgendamentoCompromisso();

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.DataHoraAgendamento = Convert.ToDateTime(Dr["DT_HR_AGENDAMENTO"]);
                    p.Anotacao = Convert.ToString(Dr["DS_ANOTACAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Telefone = Convert.ToString(Dr["DS_FONE"]);
                    p.Contato = Convert.ToString(Dr["DS_CONTATO"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.Local = Convert.ToString(Dr["DS_LOCAL"]);
                    p.CorLembrete = Convert.ToString(Dr["TX_COR_LEMBRETE"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoTipoAgendamento = Convert.ToInt32(Dr["CD_TIPO_AGENDAMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);

                    if (Dr["IN_EMAIL"].ToString() == "1")
                        p.EnviarEmail = true;
                    else
                        p.EnviarEmail = false;

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos agendamentos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<AgendamentoCompromisso> ListarAgendamentoCompromissoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [AGENDAMENTO_DE_COMPROMISSO] ";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                //strSQL = strSQL + ")";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<AgendamentoCompromisso> lista = new List<AgendamentoCompromisso>();

                while (Dr.Read())
                {
                    AgendamentoCompromisso p = new AgendamentoCompromisso();
                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.DataHoraAgendamento = Convert.ToDateTime(Dr["DT_HR_AGENDAMENTO"]);
                    p.Anotacao = Convert.ToString(Dr["DS_ANOTACAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.Telefone = Convert.ToString(Dr["DS_FONE"]);
                    p.Contato = Convert.ToString(Dr["DS_CONTATO"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.Local = Convert.ToString(Dr["DS_LOCAL"]);
                    p.CorLembrete = Convert.ToString(Dr["TX_COR_LEMBRETE"]);
                    p.CodigoPessoa = Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoTipoAgendamento = Convert.ToInt32(Dr["CD_TIPO_AGENDAMENTO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);

                    if (Dr["IN_EMAIL"].ToString() == "1")
                        p.EnviarEmail = true;
                    else
                        p.EnviarEmail = false;

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos AGENDAMENTOS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void InserirUsuarioAgendamento(int CodigoAgendamento, List<UsuarioAgendamento> listaUsuario)
        {
            try
            {
                ExcluirTodosUsuarioAgendamento(CodigoAgendamento);

                AbrirConexao();

                if(listaUsuario != null)
                    foreach (UsuarioAgendamento p in listaUsuario)
                    {
                        strSQL = "insert into USUARIO_DO_AGENDAMENTO(CD_AGENDAMENTO,CD_USUARIO) values (@v1,@v2)";

                        Cmd = new SqlCommand(strSQL, Con);

                        Cmd.Parameters.AddWithValue("@v1", CodigoAgendamento);
                        Cmd.Parameters.AddWithValue("@v2", p.CodigoUsuario);
 
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
                            throw new Exception("Erro ao Incluir usuario do agendamento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar usuario do agendamento " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodosUsuarioAgendamento(int CodigoAgendamento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from USUARIO_DO_AGENDAMENTO Where CD_AGENDAMENTO = @v1 ", Con);
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
                            throw new Exception("Erro ao excluir usuario do agendamento : " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir usuario do agendamento : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<UsuarioAgendamento> ObterUsuarioAgendamento(int CodigoAgendamento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select U.*, USU.NM_COMPLETO from USUARIO_DO_AGENDAMENTO AS U INNER JOIN USUARIO AS USU ON USU.CD_USUARIO = U.CD_USUARIO Where CD_AGENDAMENTO = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoAgendamento);
                Dr = Cmd.ExecuteReader();
                List<UsuarioAgendamento> lista = new List<UsuarioAgendamento>();

                while (Dr.Read())
                {
                    UsuarioAgendamento p = new UsuarioAgendamento();

                    p.CodigoAgendamento = Convert.ToInt32(Dr["CD_AGENDAMENTO"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.Cpl_NomeUsuario = Dr["NM_COMPLETO"].ToString();
                    lista.Add(p);

                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter usuario do agendamento : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
