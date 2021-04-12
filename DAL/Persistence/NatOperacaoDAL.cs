using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class NatOperacaoDAL : Conexao
    {
        public void Inserir(NatOperacao  p)
        {
            try
            {
                AbrirConexao();
                string strSQL = "insert into NATUREZA_DA_OPERACAO(CD_NAT_OPERACAO, DS_NAT_OPERACAO, CD_NAT_OPER_CONTRA, [CD_NAT_OPER_ST]";

                strSQL += ") values(@v1, @v2, @v3, @v4";

                strSQL += ") ";
            
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoNaturezaOperacao);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoNaturezaOperacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoNaturezaOperacaoContraPartida);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoNaturezaOperacaoST);

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
                            throw new Exception("Erro ao gravar Natureza da Operação: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception ("Erro ao gravar Natureza da Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(NatOperacao p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update [NATUREZA_DA_OPERACAO] set DS_NAT_OPERACAO = @v2, CD_NAT_OPER_CONTRA= @v3, [CD_NAT_OPER_ST]= @v4 Where [CD_NAT_OPERACAO] = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoNaturezaOperacao);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoNaturezaOperacao);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoNaturezaOperacaoContraPartida);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoNaturezaOperacaoST);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Natureza da Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(long Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from NATUREZA_DA_OPERACAO Where CD_NAT_OPERACAO = @v1", Con);
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
                            throw new Exception("Erro ao excluir Natureza da Operação: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Natureza da Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public NatOperacao PesquisarNatOperacao(long Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from NATUREZA_DA_OPERACAO Where CD_NAT_OPERACAO = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                NatOperacao p = null;
                if (Dr.Read())
                {
                    p = new NatOperacao();
                    p.CodigoNaturezaOperacao = Convert.ToInt64(Dr["CD_NAT_OPERACAO"]);
                    p.DescricaoNaturezaOperacao = Convert.ToString(Dr["DS_NAT_OPERACAO"]);
                    p.CodigoNaturezaOperacaoContraPartida = Convert.ToInt64(Dr["CD_NAT_OPER_CONTRA"]);
                    p.CodigoNaturezaOperacaoST = Convert.ToInt64(Dr["CD_NAT_OPER_ST"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Natureza da Operação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<NatOperacao> ListarNatOperacao(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select * from NATUREZA_DA_OPERACAO ";
                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 
                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem; 
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                List<NatOperacao> lista = new List<NatOperacao>();
                while (Dr.Read())
                {
                    NatOperacao p = new NatOperacao();
                    p.CodigoNaturezaOperacao = Convert.ToInt64(Dr["CD_NAT_OPERACAO"]);
                    p.DescricaoNaturezaOperacao = Convert.ToString(Dr["DS_NAT_OPERACAO"]);
                    p.CodigoNaturezaOperacaoContraPartida = Convert.ToInt64(Dr["CD_NAT_OPER_CONTRA"]);
                    p.CodigoNaturezaOperacaoST = Convert.ToInt64(Dr["CD_NAT_OPER_ST"]);
                    p.Cpl_ComboDescricaoNatOperacao = p.CodigoNaturezaOperacao + " | " + p.DescricaoNaturezaOperacao;
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Natureza de Operações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<NatOperacao> ListarNatOperacoesCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [VW_NAT_OPERACAO]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;



                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<NatOperacao> lista = new List<NatOperacao>();

                while (Dr.Read())
                {
                    NatOperacao p = new NatOperacao();
                    p.CodigoNaturezaOperacao = Convert.ToInt64(Dr["CD_NAT_OPERACAO"]);
                    p.DescricaoNaturezaOperacao = Convert.ToString(Dr["DS_NAT_OPERACAO"]);
                    p.CodigoNaturezaOperacaoContraPartida = Convert.ToInt64(Dr["CD_NAT_OPER_CONTRA"]);
                    p.CodigoNaturezaOperacaoST = Convert.ToInt64(Dr["CD_NAT_OPER_ST"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Natureza de Operações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable RelNatOperacoesCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_NAT_OPERACAO]  " + MontaFiltroIntervalo(ListaFiltros);

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Relatório de Todas Natureza de Operações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public List<NatOperacao> ListarNatOperacaoContraPartida(int intNroInicial)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select * from NATUREZA_DA_OPERACAO ";

                if (intNroInicial == 1)
                    strSQL = strSQL + " Where CD_NAT_OPERACAO >= 5000 AND CD_NAT_OPERACAO < 6000 ";
                if (intNroInicial == 2)
                    strSQL = strSQL + " Where CD_NAT_OPERACAO >= 6000 AND CD_NAT_OPERACAO < 7000 ";
                if (intNroInicial == 3)
                    strSQL = strSQL + " Where CD_NAT_OPERACAO >= 7000 AND CD_NAT_OPERACAO < 8000 ";

                if (intNroInicial == 5)
                    strSQL = strSQL + " Where CD_NAT_OPERACAO >= 1000 AND CD_NAT_OPERACAO < 2000 ";
                if (intNroInicial == 6)
                    strSQL = strSQL + " Where CD_NAT_OPERACAO >= 2000 AND CD_NAT_OPERACAO < 3000 ";
                if (intNroInicial == 7)
                    strSQL = strSQL + " Where CD_NAT_OPERACAO >= 3000 AND CD_NAT_OPERACAO < 4000 ";


                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                List<NatOperacao> lista = new List<NatOperacao>();
                while (Dr.Read())
                {
                    NatOperacao p = new NatOperacao();
                    p.CodigoNaturezaOperacao = Convert.ToInt64(Dr["CD_NAT_OPERACAO"]);
                    p.DescricaoNaturezaOperacao = Convert.ToString(Dr["DS_NAT_OPERACAO"]);
                    p.CodigoNaturezaOperacaoContraPartida = Convert.ToInt64(Dr["CD_NAT_OPER_CONTRA"]);
                    p.CodigoNaturezaOperacaoST = Convert.ToInt64(Dr["CD_NAT_OPER_ST"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Natureza de Operações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
