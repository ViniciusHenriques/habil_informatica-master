using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class DocaDAL : Conexao
    {


        protected string strSQL = "";

        public void Inserir(Doca p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [DOCA] ( DS_DOCA, CD_SITUACAO, CD_EMPRESA ) values ( @v1, @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoDoca.ToUpper());
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoEmpresa);

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

                throw new Exception("Erro ao gravar Doca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Doca p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DOCA] set [DS_DOCA] = @v2, [CD_SITUACAO] = @v3 Where [CD_DOCA] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoDoca);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoDoca);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Doca: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [DOCA] Where [CD_DOCA] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Doca: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Doca: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Doca PesquisarDoca(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [DOCA] Where CD_DOCA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                Doca p = null;

                if (Dr.Read())
                {
                    p = new Doca();

                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]).ToUpper();
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar DOCA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Doca PesquisarDocaDescricao(string strDescricao)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [DOCA] Where DS_DOCA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strDescricao);

                Dr = Cmd.ExecuteReader();

                Doca p = new Doca();

                if (Dr.Read())
                {


                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar DOCA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doca> ListarDoca(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [DOCA]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doca> lista = new List<Doca>();

                while (Dr.Read())
                {
                    Doca p = new Doca();

                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterDoca()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [DOCA] WHERE CD_SITUACAO = 1";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoDoca", typeof(Int32));
                dt.Columns.Add("DescricaoDoca", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_DOCA"]), Convert.ToString(Dr["DS_DOCA"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos as Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Doca> ListarDocasCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [DOCA]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doca> lista = new List<Doca>();

                while (Dr.Read())
                {
                    Doca p = new Doca();
                    Empresa empresa = new Empresa();
                    EmpresaDAL empresaDAL = new EmpresaDAL();


                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    empresa = empresaDAL.PesquisarEmpresa(Convert.ToInt64(Dr["CD_EMPRESA"]));
                    p.Cpl_NomeEmpresa = empresa.NomeEmpresa;
                    lista.Add(p);

                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<Doca> ListagemDocasPessoa(int intCodEmpresa)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT D.CD_DOCA, D.DS_DOCA, COUNT(1) AS QTD " +
                    "FROM DOCUMENTO AS DOC" +
                        " INNER JOIN PESSOA_DO_DOCUMENTO AS PD " +
                            " ON  DOC.CD_DOCUMENTO = PD.CD_DOCUMENTO AND PD.TP_PESSOA = 12" +
                        " INNER JOIN PESSOA AS P " +
                            " ON PD.CD_PESSOA = P.CD_PESSOA " +
                        " INNER JOIN DOCA AS D " +
                            "ON D.CD_DOCA = P.CD_DOCA " +
                            "AND D.CD_EMPRESA = DOC.CD_EMPRESA " +
                    "WHERE DOC.CD_SITUACAO = 146 and doc.CD_EMPRESA = " + intCodEmpresa +
                        "GROUP BY D.CD_DOCA, D.DS_DOCA";

                //strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doca> lista = new List<Doca>();

                while (Dr.Read())
                {
                    Doca p = new Doca();
                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]);
                    p.Cont = Convert.ToInt32(Dr["QTD"]);

                    lista.Add(p);

                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doca> ListagemDocasPessoaPesquisa(int intCodEmpresa)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT D.CD_DOCA, D.DS_DOCA, COUNT(1) AS QTD " +
                    "FROM DOCUMENTO AS DOC" +
                        " INNER JOIN PESSOA_DO_DOCUMENTO AS PD " +
                            " ON  DOC.CD_DOCUMENTO = PD.CD_DOCUMENTO AND PD.TP_PESSOA = 12" +
                        " INNER JOIN PESSOA AS P " +
                            " ON PD.CD_PESSOA = P.CD_PESSOA " +
                        " INNER JOIN DOCA AS D " +
                            "ON D.CD_DOCA = P.CD_DOCA " +
                            "AND D.CD_EMPRESA = DOC.CD_EMPRESA " +
                    "WHERE DOC.CD_SITUACAO = 146 and doc.CD_EMPRESA = " + intCodEmpresa +
                        "GROUP BY D.CD_DOCA, D.DS_DOCA";

                //strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doca> lista = new List<Doca>();

                while (Dr.Read())
                {
                    Doca p = new Doca();
                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]);
                    p.Cont = Convert.ToInt32(Dr["QTD"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas as Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doca> ListagemDocasEmpresa(int intCodEmpresa)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT D.CD_DOCA, D.DS_DOCA, COUNT(*) AS QTD " +
                    "FROM DOCUMENTO AS DOC" +
                        " INNER JOIN PESSOA_DO_DOCUMENTO AS PD " +
                            " ON  DOC.CD_DOCUMENTO = PD.CD_DOCUMENTO AND PD.TP_PESSOA = 12" +
                        " INNER JOIN PESSOA AS P " +
                            " ON PD.CD_PESSOA = P.CD_PESSOA " +
                        " INNER JOIN DOCA AS D " +
                            "ON D.CD_DOCA = P.CD_DOCA " +
                            "AND D.CD_EMPRESA = DOC.CD_EMPRESA " +
                    "WHERE DOC.CD_SITUACAO = 146 and doc.CD_EMPRESA = " + intCodEmpresa +
                        "GROUP BY D.CD_DOCA, D.DS_DOCA";

                //strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Doca> lista = new List<Doca>();

                while (Dr.Read())
                {
                    Doca p = new Doca();
                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]);
                    p.Cont = Convert.ToInt32(Dr["QTD"]);

                    lista.Add(p);

                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Doca> ListarDocaAtivas(int intCodEmpresa)
        {
            try
            {
                AbrirConexao();

                Dr = new SqlCommand("Select * from [DOCA] where CD_SITUACAO = 1 AND CD_EMPRESA = " + intCodEmpresa, Con).ExecuteReader();

                List<Doca> lista = new List<Doca>();

                while (Dr.Read())
                {
                    Doca p = new Doca();

                    p.CodigoDoca = Convert.ToInt32(Dr["CD_DOCA"]);
                    p.DescricaoDoca = Convert.ToString(Dr["DS_DOCA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CodigoEmpresa = Convert.ToInt32(Dr["CD_EMPRESA"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Docas: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
    }
}