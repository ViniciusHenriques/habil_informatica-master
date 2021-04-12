using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class BodyDocumentoDAL: Conexao
    {
        protected string strSQL = "";

        public void Inserir(BodyDocumento p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into BODY_DO_DOCUMENTO (CD_DOCUMENTO,CD_ITEM,TX_CORPO) values (@v1,@v2,@v3) SELECT SCOPE_IDENTITY()";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoItem);
                Cmd.Parameters.AddWithValue("@v3", p.TextoCorpo);

                p.CodigoBodyDocumento = Convert.ToInt32(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao Incluir body do Documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar body do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(BodyDocumento p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [BODY_DO_DOCUMENTO] set" +
                             " TX_CORPO = @v4" +
                         " Where [CD_DOCUMENTO] = @v1 AND CD_ITEM = @v3";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                //Cmd.Parameters.AddWithValue("@v2", p.CodigoBodyDocumento);               
                Cmd.Parameters.AddWithValue("@v3", p.CodigoItem);
                Cmd.Parameters.AddWithValue("@v4", p.TextoCorpo);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar body do Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }


        }
        public void Excluir(decimal CodigoDocumento, int CodigoItem)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from [BODY_DO_DOCUMENTO] Where [CD_DOCUMENTO] = @v1 AND CD_ITEM = @v2", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", CodigoItem);
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
                            throw new Exception("Erro ao excluir BODY DO DOCUMENTO: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir BODY DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public BodyDocumento PesquisarBodyDocumento(decimal CodigoDocumento, int CodigoItem)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [BODY_DO_DOCUMENTO] Where CD_DOCUMENTO = @v1 AND CD_ITEM = @v2";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", CodigoItem);
                Dr = Cmd.ExecuteReader();

                BodyDocumento p = null;

                if (Dr.Read())
                {
                    p = new BodyDocumento();

                    p.CodigoBodyDocumento = Convert.ToDecimal(Dr["CD_BODY_DOCUMENTO"]);
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoItem = Convert.ToInt32(Dr["CD_ITEM"]);
                    p.TextoCorpo = Convert.ToString(Dr["TX_CORPO"]);

                }

                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar BODY DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<BodyDocumento> ListarBodyDocumento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [BODY_DO_DOCUMENTO] order by CD_BODY_DOCUMENTO";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<BodyDocumento> lista = new List<BodyDocumento>();

                while (Dr.Read())
                {
                    BodyDocumento p = new BodyDocumento();

                    p.CodigoBodyDocumento = Convert.ToDecimal(Dr["CD_BODY_DOCUMENTO"]);
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoItem = Convert.ToInt32(Dr["CD_ITEM"]);
                    p.TextoCorpo = Convert.ToString(Dr["TX_CORPO"]);
                    
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos BODY DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public List<BodyDocumento> ListarBodyDocumentoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [BODY_DO_DOCUMENTO] ORDER BY CD_BODY_DOCUMENTO DESC ";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                strSQL = strSQL + ")";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<BodyDocumento> lista = new List<BodyDocumento>();

                while (Dr.Read())
                {
                    BodyDocumento p = new BodyDocumento();
                    p.CodigoBodyDocumento = Convert.ToDecimal(Dr["CD_BODY_DOCUMENTO"]);
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoItem = Convert.ToInt32(Dr["CD_ITEM"]);
                    p.TextoCorpo = Dr["TX_CORPO"].ToString();
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos BODY DO DOCUMENTO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
