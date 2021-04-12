using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class TipoServicoDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(TipoServico p, List<ItemTipoServico> listaItemTipoServico)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into TIPO_DE_SERVICO (DS_TIPO_SERVICO, CD_SITUACAO , VL_ISSQN, CD_CNAE, CD_SERV_LEI) values (@v1,@v2,@v3,@v4,@v5) SELECT SCOPE_IDENTITY()";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.DescricaoTipoServico.ToUpper());
                Cmd.Parameters.AddWithValue("@v2", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v3", p.ValorISSQN);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoCNAE);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoServicoLei);
                //Cmd.ExecuteNonQuery();

                
                p.CodigoTipoServico = Convert.ToInt32(Cmd.ExecuteScalar());
                
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
                            throw new Exception("Erro ao Incluir Tipo de serviços: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Tipo de serviços: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                ItemTipoServicoDAL pe3 = new ItemTipoServicoDAL();
                pe3.Inserir(p.CodigoTipoServico, listaItemTipoServico);
            }


        }
        public void Atualizar(TipoServico p, List<ItemTipoServico> listaItemTipoServico)
        {
            try
            {
                AbrirConexao();

                               
                strSQL = "update [TIPO_DE_SERVICO] " +
                         "   set [DS_TIPO_SERVICO] = @v2 "  +
                         "   , [CD_SITUACAO] = @v3, VL_ISSQN= @v4, CD_CNAE = @v5, CD_SERV_LEI = @v6" +
                         " Where [CD_TIPO_SERVICO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoServico);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoTipoServico.ToUpper());
                Cmd.Parameters.AddWithValue("@v3", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", p.ValorISSQN);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoCNAE);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoServicoLei);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Tipo de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                ItemTipoServicoDAL pe3 = new ItemTipoServicoDAL();
                pe3.Inserir(p.CodigoTipoServico, listaItemTipoServico);
            }


        }
        public void Excluir(Int32 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from [TIPO_DE_SERVICO] Where [CD_TIPO_SERVICO] = @v1;" +
                    "delete from ITEM_DO_TIPO_DE_SERVICO where CD_TIPO_SERVICO = @v1", Con);
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
                            throw new Exception("Erro ao excluir Tipo de serviço: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Tipo de serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public TipoServico PesquisarTipoServico(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [TIPO_DE_SERVICO] Where CD_TIPO_SERVICO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                TipoServico p = null;

                if (Dr.Read())
                {
                    p = new TipoServico ();

                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    p.DescricaoTipoServico = Convert.ToString(Dr["DS_TIPO_SERVICO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.ValorISSQN = Convert.ToDecimal(Dr["VL_ISSQN"]);
                    p.CodigoCNAE = Convert.ToDecimal(Dr["CD_CNAE"]);
                    p.CodigoServicoLei = Convert.ToDecimal(Dr["CD_SERV_LEI"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tipo de Serviço: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<TipoServico> ListarTipoServico(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [TIPO_DE_SERVICO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoServico> lista = new List<TipoServico>();

                while (Dr.Read())
                {
                    TipoServico p = new TipoServico();

                    p.CodigoTipoServico = Convert.ToInt32(Dr["CD_TIPO_SERVICO"]);
                    p.DescricaoTipoServico = Convert.ToString(Dr["DS_TIPO_SERVICO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.ValorISSQN  = Convert.ToDecimal(Dr["VL_ISSQN"]);
                    p.CodigoCNAE = Convert.ToDecimal(Dr["CD_CNAE"]);
                    p.CodigoServicoLei = Convert.ToDecimal(Dr["CD_SERV_LEI"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Tipos de Serviços: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public List<TipoServico> ListarTipoServicoCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [TIPO_DE_SERVICO] WHERE [CD_TIPO_SERVICO] IN ( SELECT [TIPO_DE_SERVICO].CD_TIPO_SERVICO FROM [TIPO_DE_SERVICO]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                strSQL = strSQL + ")";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoServico> lista = new List<TipoServico>();

                while (Dr.Read())
                {
                    TipoServico p = new TipoServico();
                    p.CodigoTipoServico = Convert.ToInt16(Dr["CD_TIPO_SERVICO"]);
                    p.DescricaoTipoServico = Convert.ToString(Dr["DS_TIPO_SERVICO"]);
                    p.CodigoSituacao = Convert.ToInt16(Dr["CD_SITUACAO"]);
                    
                    p.ValorISSQN = Convert.ToDecimal(Dr["VL_ISSQN"]);
                    p.CodigoCNAE = Convert.ToDecimal(Dr["CD_CNAE"]);
                    p.CodigoServicoLei = Convert.ToDecimal(Dr["CD_SERV_LEI"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Tipos de serviços: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
