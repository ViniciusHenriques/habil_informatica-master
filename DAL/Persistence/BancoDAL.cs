using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
namespace DAL.Persistence


{
    public class BancoDAL:Conexao
    {
        string strSQL;
        public bool Inserir(Banco p)
        {

            try
            {
                AbrirConexao();

                strSQL = "insert into BANCO( CD_BANCO," +
                                            "DS_BANCO ) values (@v1,@v2);";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoBanco);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoBanco);


                Cmd.ExecuteNonQuery();



                return true;
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
                            throw new Exception("Erro ao Incluir BANCO  : " + ex.Message.ToString());

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar BANCO   : " + ex.Message.ToString());

            }
            finally
            {


                FecharConexao();

            }
        }

        public List<Banco> ListarBancos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "Select * from BANCO";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Banco> lista = new List<Banco>();

                while (Dr.Read())
                {
                    Banco p = new Banco();
                    p.CodigoBanco = Convert.ToInt32(Dr["CD_BANCO"]);
                    p.DescricaoBanco = Dr["DS_BANCO"].ToString();
                    p.DescricaoBanco = Dr["DS_BANCO"].ToString();


                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar  CONTA CORRENTE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Banco> ListarBancosCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [BANCO] WHERE [CD_BANCO] IN ( SELECT [BANCO].CD_BANCO FROM [BANCO]  ";

                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;
                strSQL = strSQL + ")";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Banco> lista = new List<Banco>();

                while (Dr.Read())
                {
                    Banco p = new Banco();
                    p.CodigoBanco = Convert.ToInt16(Dr["CD_BANCO"]);
                    p.DescricaoBanco = Convert.ToString(Dr["DS_BANCO"]);


                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos bancos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public Banco PesquisarBanco(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [BANCO] Where CD_BANCO= @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Banco p = null;

                if (Dr.Read())
                {
                    p = new Banco();

                    p.CodigoBanco = Convert.ToInt32(Dr["CD_BANCO"]);
                    p.DescricaoBanco= Convert.ToString(Dr["DS_BANCO"]);


                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Bancos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(Banco p)
        {
            try
            {
                AbrirConexao();


                strSQL = "update [BANCO] " +
                         "   set [DS_BANCO] = @v2 " +
                         " Where [CD_BANCO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoBanco);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoBanco);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Banco: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }

        public void Excluir(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from BANCO Where CD_BANCO= @v1", Con);
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
                            throw new Exception("Erro ao excluir BANCO " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir BANCO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }


    }
}
