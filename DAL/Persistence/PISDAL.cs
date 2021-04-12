using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class PISDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(PIS p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [PIS] (CD_PIS, CD_TIPO, VL_PIS, DS_PIS) values (@v1, @v2, @v3, @v4)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoPIS);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoTipo );
                Cmd.Parameters.AddWithValue("@v3", p.ValorPIS);
                Cmd.Parameters.AddWithValue("@v4", p.DescricaoPIS);
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
                            throw new Exception("Erro ao Incluir PIS: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(PIS p)
        {
            try
            {
                AbrirConexao();

                strSQL = "update [PIS] set [CD_TIPO] = @v2,[CD_PIS] = @v5, [VL_PIS] = @v3, [DS_PIS] = @v4 Where [CD_INDEX] = @v1;";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoIndice);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoTipo);
                Cmd.Parameters.AddWithValue("@v3", p.ValorPIS);
                Cmd.Parameters.AddWithValue("@v4", p.DescricaoPIS);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoPIS);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar PIS: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [PIS] Where [CD_INDEX] = @v1", Con);

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
                            throw new Exception("Erro ao excluir PIS: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public PIS PesquisarPIS(string strPIS)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_PIS] Where CD_PIS = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strPIS);

                Dr = Cmd.ExecuteReader();

                PIS p = null;

                if (Dr.Read())
                {
                    p = new PIS();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoTipo= Convert.ToInt32(Dr["CD_TIPO"]);
                    p.ValorPIS= Convert.ToDouble(Dr["VL_PIS"]);
                    p.DescricaoPIS = Convert.ToString(Dr["DS_PIS"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);

                        
                    //Habil_TipoDAL rx = new Habil_TipoDAL();
                    //Habil_Tipo px = new Habil_Tipo();

                    //p.DescricaoCOFINS = rx.DescricaoHabil_Tipo(Convert.ToInt32(p.CodigoTipo));

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public PIS PesquisarPISIndice(int lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [VW_PIS] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                PIS p = null;

                if (Dr.Read())
                {
                    p = new PIS();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoTipo= Convert.ToInt32(Dr["CD_TIPO"]);
                    p.ValorPIS = Convert.ToDouble(Dr["VL_PIS"]);
                    p.DescricaoPIS = Convert.ToString(Dr["DS_PIS"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);

                    //    Habil_TipoDAL rx = new Habil_TipoDAL();
                    //    Habil_Tipo px = new Habil_Tipo();

                    //    p.NomeTipo = rx.DescricaoHabil_Tipo(Convert.ToInt32(p.CodigoTipo));
                   }

                    return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<PIS> ListarPIS(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from [VW_PIS]";
                ///                
                ///string strSQL = "select p.*,ht.DS_TIPO " +
                ///                "from pis as p " +
                ///                "    inner join habil_tipo ht on ht.DS_TIPO = p.CD_TIPO ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<PIS> lista = new List<PIS>();


                Habil_TipoDAL rx = new Habil_TipoDAL();
                Habil_Tipo px = new Habil_Tipo();

                while (Dr.Read())
                {
                    PIS p = new PIS();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoTipo = Convert.ToInt32(Dr["CD_TIPO"]);
                    p.DescricaoPIS = Convert.ToString(Dr["DS_PIS"]);
                    p.ValorPIS= Convert.ToDouble(Dr["VL_PIS"]);
                    p.DescricaoTipo = Convert.ToString(Dr["DS_TIPO"]);

                    //p.NomeTipo = rx.DescricaoHabil_Tipo(Convert.ToInt32(p.CodigoTipo));
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterPIS(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_PIS]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoIndice", typeof(int));
                dt.Columns.Add("CodigoPIS", typeof(Int32));
                dt.Columns.Add("VL_PIS", typeof(Double));
                dt.Columns.Add("CodigoTipo", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_INDEX"]),
                        Convert.ToInt32(Dr["CD_PIS"]),
                        Convert.ToInt32(Dr["CD_TIPO"]),
                        Convert.ToDouble(Dr["VL_PIS"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas PIS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

    }
}
