using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class ItemDocumentoDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(decimal CodigoDocumento, List<ItemDocumento> listaItemOrcamento)
        {
            try
            {
                ExcluirTodos(CodigoDocumento);
                
                AbrirConexao();
                foreach (ItemDocumento p in listaItemOrcamento)
                {
                    strSQL = "insert into ITEM_DO_DOCUMENTO(CD_DOCUMENTO,CD_ITEM, CD_USU_ATENDENTE,DT_HR_INICIO,DT_HR_FIM,CD_SITUACAO) values (@v1,@v2,@v3,@v4,@v5,@v6)";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoItem);
                    Cmd.Parameters.AddWithValue("@v3", p.CodigoUsuarioAtendente);
                    Cmd.Parameters.AddWithValue("@v4", p.DataHoraInicio);
                    Cmd.Parameters.AddWithValue("@v5", p.DataHoraFim);
                    Cmd.Parameters.AddWithValue("@v6", p.CodigoSituacao);

                    BodyDocumentoDAL BodyDocumentoDAL = new BodyDocumentoDAL();
                    BodyDocumentoDAL.Excluir(CodigoDocumento,p.CodigoItem);
                    BodyDocumento BodyDocumento = new BodyDocumento();
                    BodyDocumento.CodigoDocumento = CodigoDocumento;
                    BodyDocumento.CodigoItem = p.CodigoItem;
                    BodyDocumento.TextoCorpo = p.DescricaoItem;
                    BodyDocumentoDAL.Inserir(BodyDocumento);

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
                            throw new Exception("Erro ao Incluir Item do Documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Item do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }

        public void Atualizar(ItemDocumento p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [ITEM_DO_DOCUMENTO] " +
                             "set [CD_ITEM] = @v2" +
                             ", CD_USU_ATENDENTE = @v3" +
                             ", DT_HR_INICIO = @v4" +
                             ", DT_HR_FIM = @v5" +
                             ", CD_SITUACAO = @v6" +
                        " Where [CD_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoItem);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoUsuarioAtendente);
                Cmd.Parameters.AddWithValue("@v4", p.DataHoraInicio);
                Cmd.Parameters.AddWithValue("@v5", p.DataHoraFim);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoSituacao);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Item do Documento: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from [ITEM_DO_DOCUMENTO] Where [CD_DOCUMENTO] = @v1", Con);
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
                            throw new Exception("Erro ao excluir Item do Documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Item do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }

        public ItemDocumento PesquisarItemDocumento(decimal CodigoDocumento, int CodigoItem)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [ITEM_DO_DOCUMENTO] Where CD_DOCUMENTO= @v1 and CD_ITEM = @v2";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", CodigoItem);
                Dr = Cmd.ExecuteReader();

                ItemDocumento p = null;

                if (Dr.Read())
                {
                    p = new ItemDocumento();

                    p.CodigoItem = Convert.ToInt32(Dr["CD_ITEM"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoUsuarioAtendente = Convert.ToInt32(Dr["CD_USU_ATENDENTE"]);
                    p.DataHoraInicio = Convert.ToDateTime(Dr["DT_HR_INICIO"]);
                    p.DataHoraFim = Convert.ToDateTime(Dr["DT_HR_FIM"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);

                    Usuario usu = new Usuario();
                    UsuarioDAL usuDAL = new UsuarioDAL();
                    usu = usuDAL.PesquisarUsuario(p.CodigoUsuarioAtendente);
                    p.Cpl_NomeUsuario = usu.NomeUsuario;

                    Habil_Tipo tipo = new Habil_Tipo();
                    Habil_TipoDAL tipoDAL = new Habil_TipoDAL();
                    tipo = tipoDAL.PesquisarHabil_Tipo(p.CodigoSituacao);
                    p.Cpl_DescSituacao = tipo.DescricaoTipo;

                    BodyDocumento Body = new BodyDocumento();
                    BodyDocumentoDAL BodyDAL = new BodyDocumentoDAL();
                    Body = BodyDAL.PesquisarBodyDocumento(p.CodigoDocumento,p.CodigoItem);
                    p.DescricaoItem = Body.TextoCorpo;
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Itens do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<ItemDocumento> ListarItemDocumento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [ITEM_DO_DOCUMENTO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ItemDocumento> lista = new List<ItemDocumento>();

                while (Dr.Read())
                {
                    ItemDocumento p = new ItemDocumento();

                    p.CodigoItem = Convert.ToInt32(Dr["CD_ITEM"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoUsuarioAtendente = Convert.ToInt32(Dr["CD_USU_ATENDENTE"]);
                    p.DataHoraInicio = Convert.ToDateTime(Dr["DT_HR_INICIO"]);
                    p.DataHoraFim = Convert.ToDateTime(Dr["DT_HR_FIM"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    Usuario usu = new Usuario();
                    UsuarioDAL usuDAL = new UsuarioDAL();
                    usu = usuDAL.PesquisarUsuario(p.CodigoUsuarioAtendente);
                    p.Cpl_NomeUsuario = usu.NomeUsuario;

                    Habil_Tipo tipo = new Habil_Tipo();
                    Habil_TipoDAL tipoDAL = new Habil_TipoDAL();
                    tipo = tipoDAL.PesquisarHabil_Tipo(p.CodigoSituacao);
                    p.Cpl_DescSituacao = tipo.DescricaoTipo;
                    lista.Add(p);

                    BodyDocumento Body = new BodyDocumento();
                    BodyDocumentoDAL BodyDAL = new BodyDocumentoDAL();
                    Body = BodyDAL.PesquisarBodyDocumento(p.CodigoDocumento,p.CodigoItem);
                    p.DescricaoItem = Body.TextoCorpo;
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Itens do Documento: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from ITEM_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ", Con);
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
                            throw new Exception("Erro ao excluir Itens do Documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Itens do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<ItemDocumento> ObterItemDocumento(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from ITEM_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Dr = Cmd.ExecuteReader();
                List<ItemDocumento> lista = new List<ItemDocumento>();

                while (Dr.Read())
                {
                    ItemDocumento p = new ItemDocumento();

                    p.CodigoItem = Convert.ToInt32(Dr["CD_ITEM"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoUsuarioAtendente = Convert.ToInt32(Dr["CD_USU_ATENDENTE"]);
                    p.DataHoraInicio = Convert.ToDateTime(Dr["DT_HR_INICIO"]);
                    p.DataHoraFim = Convert.ToDateTime(Dr["DT_HR_FIM"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    Usuario usu = new Usuario();
                    UsuarioDAL usuDAL = new UsuarioDAL();
                    usu = usuDAL.PesquisarUsuario(p.CodigoUsuarioAtendente);
                    p.Cpl_NomeUsuario = usu.NomeUsuario;

                    Habil_Tipo tipo = new Habil_Tipo();
                    Habil_TipoDAL tipoDAL = new Habil_TipoDAL();
                    tipo = tipoDAL.PesquisarHabil_Tipo(p.CodigoSituacao);
                    p.Cpl_DescSituacao = tipo.DescricaoTipo;
                    lista.Add(p);

                    BodyDocumento Body = new BodyDocumento();
                    BodyDocumentoDAL BodyDAL = new BodyDocumentoDAL();
                    Body = BodyDAL.PesquisarBodyDocumento(p.CodigoDocumento, p.CodigoItem);
                    p.DescricaoItem = Body.TextoCorpo;
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter Itens do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelItemDocumento(decimal CodigoDocumento, bool Fatura)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "";
                if (!Fatura)
                    strSQL = "select " +
                                "i.*,USUARIO.NM_COMPLETO AS NM_USUARIO,D.NR_DOCUMENTO" +
                            " FROM " +
                                "DOCUMENTO as D, ITEM_DO_DOCUMENTO as I" +
                                " INNER JOIN USUARIO ON USUARIO.CD_USUARIO = I.CD_USU_ATENDENTE" +
                           " WHERE " +
                               "I.CD_DOCUMENTO = " + CodigoDocumento + "and d.CD_DOCUMENTO = i.CD_DOCUMENTO";
                else
                    strSQL = "select " +
                                "i.*,USUARIO.NM_COMPLETO AS NM_USUARIO,D.NR_DOCUMENTO " +
                             " FROM " +
                                 "DOCUMENTO as D, ITEM_DO_DOCUMENTO as I " +
                                 "INNER JOIN USUARIO ON USUARIO.CD_USUARIO = I.CD_USU_ATENDENTE " +
                             "WHERE " +
                                "D.CD_DOC_ORIGINAL = " + CodigoDocumento + "and I.CD_DOCUMENTO = D.CD_DOCUMENTO";
                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);                
                da.Fill(dt);

                if (dt.Rows.Count != 0)
                    return dt;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Relatório : " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
