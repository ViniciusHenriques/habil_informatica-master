 using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class COFINSDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(COFINS p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [COFINS] (CD_COFINS, CD_TIPO, VL_COFINS, DS_COFINS) values (@v1, @v2, @v3, @v4)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoCOFINS);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoTipo);
                Cmd.Parameters.AddWithValue("@v3", p.ValorCOFINS);
                Cmd.Parameters.AddWithValue("@v4", p.DescricaoCOFINS);
                p.CodigoIndice = Convert.ToInt32(Cmd.ExecuteScalar());


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
                            throw new Exception("Erro ao Incluir COFINS: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar COFINS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(COFINS p)
        {
            try
            {
                AbrirConexao();

                strSQL = "update [COFINS] set [CD_TIPO] = @v2, [VL_COFINS] = @v3, [DS_COFINS] = @v4 Where [CD_INDEX] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoIndice);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoTipo);
                Cmd.Parameters.AddWithValue("@v3", p.ValorCOFINS);
                Cmd.Parameters.AddWithValue("@v4", p.DescricaoCOFINS);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar COFINS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int intCodigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [COFINS] Where [CD_index] = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", intCodigo);

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
                            throw new Exception("Erro ao excluir COFINS: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir COFINS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public COFINS PesquisarCOFINS(string strCOFINS)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [COFINS] Where CD_COFINS = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strCOFINS);

                Dr = Cmd.ExecuteReader();

                COFINS p = null;

                if (Dr.Read())
                {
                    p = new COFINS();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.ValorCOFINS = Convert.ToDouble(Dr["VL_COFINS"]);

                    //Habil_TipoDAL rx = new Habil_TipoDAL();
                    //Habil_Tipo px = new Habil_Tipo();

                    //p.NomeTipo = rx.DescricaoHabil_Tipo(Convert.ToInt32 (p.CodigoTipo));

                    //p.ValorCOFINS = Convert.ToDouble(Dr["VL_COFINS"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar COFINS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public COFINS PesquisarCOFINSIndice(int lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [COFINS] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                COFINS p = null;

                if (Dr.Read())
                {
                    p = new COFINS();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.ValorCOFINS = Convert.ToDouble(Dr["VL_COFINS"]);
                    p.DescricaoCOFINS = Convert.ToString(Dr["DS_COFINS"]);
                    //Habil_TipoDAL rx = new Habil_TipoDAL();
                    //Habil_Tipo px = new Habil_Tipo();

                    //p.NomeTipo = rx.DescricaoHabil_Tipo(Convert.ToInt32(p.CodigoTipo));


                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar COFINS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<COFINS> ListarCOFINS(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select C.CD_INDEX, C.CD_COFINS, C.VL_COFINS, C.DS_COFINS, HT.DS_TIPO" +
                    " from [COFINS] as C" +
                    " inner join habil_tipo ht on ht.CD_TIPO = C.CD_TIPO ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<COFINS> lista = new List<COFINS>();
                
                while (Dr.Read())
                {
                    COFINS p = new COFINS();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);
                    p.ValorCOFINS = Convert.ToDouble(Dr["VL_COFINS"]);
                    p.DescricaoCOFINS = Convert.ToString(Dr["DS_COFINS"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos os COFINS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }



        }
        public DataTable ObterCOFINS(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [COFINS]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoIndice", typeof(int));
                dt.Columns.Add("CodigoCOFINS", typeof(Int32));
                dt.Columns.Add("VL_COFINS", typeof(Double));
                dt.Columns.Add("CodigoTipo", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_INDEX"]),
                        Convert.ToInt32(Dr["CD_COFINS"]),
                        Convert.ToInt32(Dr["CD_TIPO"]),
                        Convert.ToDouble(Dr["VL_COFINS"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas COFINSs: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
