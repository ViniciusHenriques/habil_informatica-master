using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class GpoComissaoDAL : Conexao
    {


        protected string strSQL = "";

        public void Inserir(GpoComissao p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [GRUPO_DE_COMISSAO] ( DS_GPO_COMISSAO, CD_SITUACAO) values ( @v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoGpoComissao.ToUpper());
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);

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
                            throw new Exception("Erro ao Incluir Bairro: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar GpoComissao: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(GpoComissao p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [GRUPO_DE_COMISSAO] set [DS_GPO_COMISSAO] = @v2, [CD_SITUACAO] = @v3 Where [CD_GPO_COMISSAO] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoGpoComissao);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoGpoComissao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar GpoComissao: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [GRUPO_DE_COMISSAO] Where [CD_GPO_COMISSAO] = @v1", Con);

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
                            throw new Exception("Erro ao excluir GpoComissao: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir GpoComissao: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public GpoComissao PesquisarGpoComissao(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [GRUPO_DE_COMISSAO] Where CD_GPO_COMISSAO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                GpoComissao p = null;

                if (Dr.Read())
                {
                    p = new GpoComissao();

                    p.CodigoGpoComissao = Convert.ToInt32(Dr["CD_GPO_COMISSAO"]);
                    p.DescricaoGpoComissao = Convert.ToString(Dr["DS_GPO_COMISSAO"]).ToUpper();
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar GRUPO_DE_COMISSAO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public GpoComissao PesquisarGpoComissaoDescricao(string strDescricao)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [GRUPO_DE_COMISSAO] Where DS_GPO_COMISSAO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strDescricao);

                Dr = Cmd.ExecuteReader();

                GpoComissao p = new GpoComissao();

                if (Dr.Read())
                {


                    p.CodigoGpoComissao = Convert.ToInt32(Dr["CD_GPO_COMISSAO"]);
                    p.DescricaoGpoComissao = Convert.ToString(Dr["DS_GPO_COMISSAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar GRUPO_DE_COMISSAO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<GpoComissao> ListarGpoComissao(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [GRUPO_DE_COMISSAO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GpoComissao> lista = new List<GpoComissao>();

                while (Dr.Read())
                {
                    GpoComissao p = new GpoComissao();

                    p.CodigoGpoComissao = Convert.ToInt32(Dr["CD_GPO_COMISSAO"]);
                    p.DescricaoGpoComissao = Convert.ToString(Dr["DS_GPO_COMISSAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas GpoComissaos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterGpoComissao(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [GRUPO_DE_COMISSAO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoGpoComissao", typeof(Int32));
                dt.Columns.Add("DescricaoGpoComissao", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_GPO_COMISSAO"]), Convert.ToString(Dr["DS_GPO_COMISSAO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos as GpoComissaos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<GpoComissao> ListarGpoComissaosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [GRUPO_DE_COMISSAO]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<GpoComissao> lista = new List<GpoComissao>();

                while (Dr.Read())
                {
                    GpoComissao p = new GpoComissao();

                    p.CodigoGpoComissao = Convert.ToInt32(Dr["CD_GPO_COMISSAO"]);
                    p.DescricaoGpoComissao = Convert.ToString(Dr["DS_GPO_COMISSAO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    lista.Add(p);

                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as GpoComissaos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
    }
}