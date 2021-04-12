using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class CidadeRegraFreteDAL : Conexao
    {

        public void Inserir(List<CidadeRegraFrete> ListaCidades, int CodigoRegra)
        {
            try
            {
                ExcluirTodos(CodigoRegra);

                AbrirConexao();

                foreach (CidadeRegraFrete cit in ListaCidades)
                {
                    Cmd = new SqlCommand("insert into CIDADE_REGRA_DE_FRETE (CD_IBGE, CD_REGRA_DE_FRETE) " +
                                                        "values (@v1,@v2);", Con);
                    Cmd.Parameters.AddWithValue("@v1", cit.CodigoIBGE);
                    Cmd.Parameters.AddWithValue("@v2", CodigoRegra);

                    Cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar cidade da regra de frete: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void ExcluirTodos(decimal CodigoRegra)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from CIDADE_REGRA_DE_FRETE Where CD_REGRA_DE_FRETE = @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);
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
                            throw new Exception("Erro ao excluir cidades da regra: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir cidades da regra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<CidadeRegraFrete> ObterCidadesRegraFrete(decimal CodigoRegra)
        {
            try
            {

                AbrirConexao();
                string comando = "SELECT CIT.*,M.*,E.SIGLA "+
                                    "FROM CIDADE_REGRA_DE_FRETE AS CIT "+
                                        "INNER JOIN MUNICIPIO AS M ON M.CD_MUNICIPIO = CIT.CD_IBGE "+
                                        "INNER JOIN ESTADO AS E ON E.CD_ESTADO = M.CD_ESTADO "+
                                    "WHERE CD_REGRA_DE_FRETE = @v1";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoRegra);

                Dr = Cmd.ExecuteReader();
                List<CidadeRegraFrete> lista = new List<CidadeRegraFrete>();

                while (Dr.Read())
                {
                    CidadeRegraFrete p = new CidadeRegraFrete();
                    p.CodigoIBGE = Convert.ToInt32(Dr["CD_IBGE"]);
                    p.CodigoRegraFrete = Convert.ToInt32(Dr["CD_REGRA_DE_FRETE"]);
                    p.Cpl_CodigoEstado = Convert.ToInt32(Dr["CD_ESTADO"]);
                    p.Cpl_DescricaoCidade = Dr["DS_MUNICIPIO"].ToString();
                    p.Cpl_Sigla = Dr["SIGLA"].ToString();

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar cidades da regra de frete: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
