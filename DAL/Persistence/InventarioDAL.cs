using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class InventarioDAL : Conexao
    {
        protected string strSQL = "";

        public void InserirInventario(Inventario p, ref decimal decIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "insert into [INVENTARIO] (CD_SITUACAO, NR_CONTAGEM, DS_INVENTARIO, CD_MAQUINA, CD_USUARIO)" +
                         " values (@v1, @v2, @v3, @v4, @v5) SELECT SCOPE_IDENTITY();";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", 130);
                Cmd.Parameters.AddWithValue("@v2", p.NrContagem);
                Cmd.Parameters.AddWithValue("@v3", p.DescInventario);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoMaquina);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoUsuario);
                p.CodigoIndice = Convert.ToInt32(Cmd.ExecuteScalar());
                decIndice = p.CodigoIndice; 
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
                            throw new Exception("Erro ao Incluir Inventario: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Iventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public void InserirItemInventario(ItemDoInventario p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [ITEM_DO_INVENTARIO] (CD_INDEX_INVENTARIO, CD_INDEX_ESTOQUE)" +
                    " values (@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoIndiceInventario);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoIndiceEstoque);
                
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
                            throw new Exception("Erro ao Incluir Inventario: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Item do Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Boolean InventarioComProdutoJaAberto(int empresa, int localizacao1, int localizacao2, int produto, int Categoria1, int Categoria2)
        {
            try
            {
                AbrirConexao();
                strSQL = "SELECT TOP 1 1 FROM ITEM_DO_INVENTARIO as it" +
                    " INNER JOIN [INVENTARIO] as i ON it.CD_INDEX_INVENTARIO = i.CD_INDEX " +
                    "where i.CD_SITUACAO = 130 and it.CD_INDEX_ESTOQUE in ";

                if (localizacao1 != 0 && localizacao2 != 0)
                {
                    strSQL += "(select isnull(e.CD_INDEX,0) as CD_INDEX " +
                        "from [VW_ESTOQUE] as e " +
                        "inner join LOCALIZACAO as l on l.CD_INDEX = e.CD_INDEX_LOCALIZACAO " +
                        "where e.CD_EMPRESA = @v3 " +
                        "and l.CD_LOCALIZACAO >= (select CD_LOCALIZACAO FROM LOCALIZACAO WHERE CD_INDEX = @v1) " +
                        "and l.CD_LOCALIZACAO <= (select CD_LOCALIZACAO FROM LOCALIZACAO WHERE CD_INDEX = @v2) " +
                        "AND e.CD_SITUACAO = 128)";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v1", localizacao1);
                    Cmd.Parameters.AddWithValue("@v2", localizacao2);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                }

                if (produto != 0)
                {
                    strSQL += "(select ISNULL(CD_INDEX,0) AS CD_INDEX " +
                        "from [VW_ESTOQUE]" +
                        " where CD_EMPRESA = @v3 " +
                        "and CD_PRODUTO = @v4 " +
                        "AND CD_SITUACAO = 128)";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                    Cmd.Parameters.AddWithValue("@v4", produto);
                }

                if (Categoria1 != 0 && Categoria2 != 0)
                {

                    strSQL += "(select isnull(e.CD_INDEX,0) AS CD_INDEX" +
                        " from [VW_ESTOQUE] as e" +
                        " inner join PRODUTO as p on e.CD_PRODUTO = p.CD_PRODUTO" +
                        " inner join CATEGORIA as c on c.CD_INDEX = p.CD_CATEGORIA" +
                        " where e.CD_EMPRESA = @v3" +
                        " and c.CD_CATEGORIA >= (select CD_CATEGORIA FROM CATEGORIA WHERE CD_INDEX = @v5) " +
                        " and c.CD_CATEGORIA <= (select CD_CATEGORIA FROM CATEGORIA WHERE CD_INDEX = @v6) " +
                        "AND e.CD_SITUACAO = 128)";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v5", Categoria1);
                    Cmd.Parameters.AddWithValue("@v6", Categoria2);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                }
                Dr = Cmd.ExecuteReader();
                if (Dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public void PopulaItemDoInventario(int empresa, int localizacao1, int localizacao2, int produto, int Categoria1, int Categoria2, decimal decIndice)
        {
            try
            {
                AbrirConexao();

                if (localizacao1 != 0 && localizacao2 != 0)
                {
                    strSQL = "select isnull(e.CD_INDEX,0) as CD_INDEX, e.CD_PRODUTO " +
                        "from [VW_ESTOQUE] as e " +
                        "inner join LOCALIZACAO as l on l.CD_INDEX = e.CD_INDEX_LOCALIZACAO " +
                        "where e.CD_EMPRESA = @v3 " +
                        "and l.CD_LOCALIZACAO >= (select CD_LOCALIZACAO FROM LOCALIZACAO WHERE CD_INDEX = @v1) " +
                        "and l.CD_LOCALIZACAO <= (select CD_LOCALIZACAO FROM LOCALIZACAO WHERE CD_INDEX = @v2) " +
                        "AND e.CD_SITUACAO = 128";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v1", localizacao1);
                    Cmd.Parameters.AddWithValue("@v2", localizacao2);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                }

                if (produto != 0)
                {
                    strSQL = "select ISNULL(CD_INDEX,0) AS CD_INDEX, CD_PRODUTO " +
                        "from [VW_ESTOQUE]" +
                        " where CD_EMPRESA = @v3 " +
                        "and CD_PRODUTO = @v4 " +
                        "AND CD_SITUACAO = 128";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                    Cmd.Parameters.AddWithValue("@v4", produto);
                }

                if (Categoria1 != 0 && Categoria2 != 0)
                {

                    strSQL = "select isnull(e.CD_INDEX,0) AS CD_INDEX, e.CD_PRODUTO " +
                        " from [VW_ESTOQUE] as e" +
                        " inner join PRODUTO as p on e.CD_PRODUTO = p.CD_PRODUTO" +
                        " inner join CATEGORIA as c on c.CD_INDEX = p.CD_CATEGORIA" +
                        " where e.CD_EMPRESA = @v3" +
                        " and c.CD_CATEGORIA >= (select CD_CATEGORIA FROM CATEGORIA WHERE CD_INDEX = @v5) " +
                        " and c.CD_CATEGORIA <= (select CD_CATEGORIA FROM CATEGORIA WHERE CD_INDEX = @v6) " +
                        "AND e.CD_SITUACAO = 128";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v5", Categoria1);
                    Cmd.Parameters.AddWithValue("@v6", Categoria2);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                }

                ProdutoDAL RnProdutoDAL = new ProdutoDAL();

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    ItemDoInventario p = new ItemDoInventario();                  

                    p.CodigoIndiceEstoque = Convert.ToDecimal(Dr["CD_INDEX"]);
                    p.CodigoIndiceInventario = decIndice;

                    InserirItemInventario(p);

                    RnProdutoDAL.AtualizarIndicadorInventario(Convert.ToInt32(Dr["CD_PRODUTO"]), true);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Boolean IndiceEstoqueExistente(int empresa, int localizacao1, int localizacao2, int produto, int Categoria1, int Categoria2)
        {
            try
            {
                AbrirConexao();

                if (localizacao1 != 0 && localizacao2 != 0)
                {
                    strSQL = "select isnull(e.CD_INDEX,0) as CD_INDEX " +
                        "from [VW_ESTOQUE] as e " +
                        "inner join LOCALIZACAO as l on l.CD_INDEX = e.CD_INDEX_LOCALIZACAO " +
                        "where e.CD_EMPRESA = @v3 " +
                        "and l.CD_LOCALIZACAO >= (select CD_LOCALIZACAO FROM LOCALIZACAO WHERE CD_INDEX = @v1) " +
                        "and l.CD_LOCALIZACAO <= (select CD_LOCALIZACAO FROM LOCALIZACAO WHERE CD_INDEX = @v2) " +
                        "AND e.CD_SITUACAO = 128";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v1", localizacao1);
                    Cmd.Parameters.AddWithValue("@v2", localizacao2);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                }

                if (produto != 0)
                {
                    strSQL = "select ISNULL(CD_INDEX,0) AS CD_INDEX " +
                        "from [VW_ESTOQUE]" +
                        " where CD_EMPRESA = @v3 " +
                        "and CD_PRODUTO = @v4 " +
                        "AND CD_SITUACAO = 128";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                    Cmd.Parameters.AddWithValue("@v4", produto);
                }

                if (Categoria1 != 0 && Categoria2 != 0)
                {

                    strSQL = "select isnull(e.CD_INDEX,0) AS CD_INDEX" +
                        " from [VW_ESTOQUE] as e" +
                        " inner join PRODUTO as p on e.CD_PRODUTO = p.CD_PRODUTO" +
                        " inner join CATEGORIA as c on c.CD_INDEX = p.CD_CATEGORIA" +
                        " where e.CD_EMPRESA = @v3" +
                        " and c.CD_CATEGORIA >= (select CD_CATEGORIA FROM CATEGORIA WHERE CD_INDEX = @v5) " +
                        " and c.CD_CATEGORIA <= (select CD_CATEGORIA FROM CATEGORIA WHERE CD_INDEX = @v6) " +
                        "AND e.CD_SITUACAO = 128";
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v5", Categoria1);
                    Cmd.Parameters.AddWithValue("@v6", Categoria2);
                    Cmd.Parameters.AddWithValue("@v3", empresa);
                }

                Dr = Cmd.ExecuteReader();
                if (Dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Estoque: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Inventario PesquisarEstoqueIndice(int lngIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [INVENTARIO] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", lngIndice);

                Dr = Cmd.ExecuteReader();

                Inventario p = null;

                if (Dr.Read())
                {
                    p = new Inventario();

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DtGeracao = Convert.ToDateTime(Dr["DT_GERACAO"]);

                    p.NrContagem = Convert.ToInt16(Dr["NR_CONTAGEM"]);
                    p.DescInventario = Convert.ToString(Dr["DS_INVENTARIO"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Inventario> ListarGrid()
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT *, " +
                                 " case CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX )) When 0  then 0 ELSE" +
                                 " (SELECT ROUND((CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX AND QT_CONTAGEM_1 > 0 )) /" +
                                 "CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX ))) * 100.00,0) ) END AS  PCT1," +
                                 "Case CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX )) When 0  then 0" +
                                 " ELSE " +
                                 "(SELECT ROUND(" +
                                 "(CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX AND QT_CONTAGEM_2 > 0 ))/" +
                                 "CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX ))) * 100.00,0) ) END AS  PCT2," +
                                 "Case CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX )) When 0 then 0 " +
                                 "ELSE (SELECT ROUND(" +
                                 "(CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX AND QT_CONTAGEM_3 > 0))/" +
                                 "CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX ))) * 100.00,0) ) END AS  PCT3, " +
                                 "Case CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX )) When 0  then 0" +
                                 " ELSE (SELECT ROUND(" +
                                 "(CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX AND QT_CONTAGEM_4 > 0 ))/" +
                                 "CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX ))) * 100.00,0) ) END AS  PCT4," +
                                 "Case CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX )) When 0  then 0" +
                                 " ELSE (SELECT ROUND(" +
                                 "(CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX AND QT_CONTAGEM_5 > 0))/" +
                                 "CONVERT(money, (select COUNT(1) from ITEM_DO_INVENTARIO WHERE CD_INDEX_INVENTARIO = I.CD_INDEX ))) * 100.00,0)) END AS  PCT5" +
                                 " FROM INVENTARIO AS I INNER JOIN HABIL_TIPO AS HT ON I.CD_SITUACAO = HT.CD_TIPO WHERE I.CD_SITUACAO = 130  ";
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Inventario> lista = new List<Inventario>();

                while (Dr.Read())
                {
                    Inventario p = new Inventario();
                    
                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.DescInventario = Convert.ToString(Dr["DS_INVENTARIO"]);
                    p.DtGeracao = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    p.DescSituacao = Convert.ToString(Dr["DS_TIPO"]);
                    p.NrContagem = Convert.ToInt16(Dr["NR_CONTAGEM"]);
                    p.PctCont1 = Convert.ToDecimal(Dr["PCT1"]);
                    p.PctCont2 = Convert.ToDecimal(Dr["PCT2"]);
                    p.PctCont3 = Convert.ToDecimal(Dr["PCT3"]);
                    p.PctCont4 = Convert.ToDecimal(Dr["PCT4"]);
                    p.PctCont5 = Convert.ToDecimal(Dr["PCT5"]);
                    p.BtnCancelar = true;
                    p.BtnEncerrar = true;

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void MontaSession(decimal CodInventario, ref string desc, ref string dtlancamento)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT * FROM INVENTARIO" +
                    " WHERE CD_SITUACAO = 130 and CD_INDEX = " + CodInventario.ToString();
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    Inventario p = new Inventario();

                    desc = Convert.ToString(Dr["DS_INVENTARIO"]);
                    p.DtGeracao = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    dtlancamento = p.DtGeracao.ToString("dd/MM/yyyy hh:mm:ss");

                    p.NrContagem = Convert.ToInt16(Dr["NR_CONTAGEM"]);
                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable RelParaInventario(double CodInventario)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_INVENTARIO] WHERE  CD_INDEX_INVENTARIO = " + CodInventario.ToString() + "ORDER BY CD_LOCALIZACAO, NM_PRODUTO" ;

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Obter Regra Fiscal de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public DataTable RelInventario(double CodInventario , short shtitem, short shCon)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "";
                //validada
                if (shtitem == 1)
                {
                    strSQL = "Select * from [VW_INVENTARIO] WHERE  CD_INDEX_INVENTARIO = " + CodInventario.ToString() + "ORDER BY CD_LOCALIZACAO, NM_PRODUTO";
                }
                
                if (shtitem == 2)
                {
                    strSQL = "select* from VW_INVENTARIO where CD_INDEX_INVENTARIO = " + CodInventario.ToString() + " AND (";

                    if (shCon == 2)
                    {
                        strSQL += "(QT_CONTAGEM_1 = QT_CONTAGEM_2)";
                    }
                    if (shCon == 3)
                    {
                        strSQL += "(QT_CONTAGEM_1 = QT_CONTAGEM_2)" +
                            " OR (QT_CONTAGEM_1 = QT_CONTAGEM_3)" +
                            " OR (QT_CONTAGEM_2 = QT_CONTAGEM_3)";
                    }
                    if (shCon == 4)
                    {
                        strSQL += "(QT_CONTAGEM_1 = QT_CONTAGEM_2)" +
                            " OR (QT_CONTAGEM_1 = QT_CONTAGEM_3" +
                            ") OR (QT_CONTAGEM_1 = QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_2 = QT_CONTAGEM_3)" +
                            " OR (QT_CONTAGEM_2 = QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_3 = QT_CONTAGEM_4)";
                    }
                    if (shCon == 5)
                    {
                        strSQL += "(QT_CONTAGEM_1 = QT_CONTAGEM_2)" +
                            " OR (QT_CONTAGEM_1 = QT_CONTAGEM_3)" +
                            " OR (QT_CONTAGEM_1 = QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_1 = QT_CONTAGEM_5)" +
                            " OR (QT_CONTAGEM_2 = QT_CONTAGEM_3)" +
                            " OR (QT_CONTAGEM_2 = QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_2 = QT_CONTAGEM_5)" +
                            " OR (QT_CONTAGEM_3 = QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_3 = QT_CONTAGEM_5)";
                    }
                    strSQL += ") ORDER BY CD_LOCALIZACAO, NM_PRODUTO";

                }
                //null
                if (shtitem == 3)
                {
                    strSQL = "select* from VW_INVENTARIO where CD_INDEX_INVENTARIO = " + CodInventario.ToString() + " AND (";

                    if (shCon == 2)
                    {
                        strSQL += "(QT_CONTAGEM_1 IS null) ";
                    }
                    if (shCon == 3)
                    {
                        strSQL += "(QT_CONTAGEM_1 IS null)" +
                            " OR (QT_CONTAGEM_2 IS null)" +
                            " OR (QT_CONTAGEM_3 IS null)";
                    }
                    if (shCon == 4)
                    {
                        strSQL += "(QT_CONTAGEM_1 IS null)" +
                            " OR (QT_CONTAGEM_4 IS null)" +
                            " OR (QT_CONTAGEM_2 IS null)" +
                            " OR (QT_CONTAGEM_2 IS null)";
                    }
                    if (shCon == 5)
                    {
                        strSQL += "(QT_CONTAGEM_1 IS null)" +
                            " OR (QT_CONTAGEM_2 IS null)" +
                            " OR (QT_CONTAGEM_3 IS null)" +
                            " OR (QT_CONTAGEM_4 IS null)" +
                            " OR (QT_CONTAGEM_5 IS null)";
                    }
                    strSQL += ") ORDER BY CD_LOCALIZACAO, NM_PRODUTO";
                }
                if (shtitem == 4)
                {
                    strSQL = "select* from VW_INVENTARIO where CD_INDEX_INVENTARIO = " + CodInventario.ToString() + " AND (";

                    if (shCon == 2)
                    {
                        strSQL += "(QT_CONTAGEM_1 <> QT_CONTAGEM_2)";
                    }
                    if (shCon == 3)
                    {
                        strSQL += "(QT_CONTAGEM_1 <> QT_CONTAGEM_2)" +
                            " OR (QT_CONTAGEM_1 <> QT_CONTAGEM_3)" +
                            " OR (QT_CONTAGEM_2 <> QT_CONTAGEM_3)";
                    }
                    if (shCon == 4)
                    {
                        strSQL += "(QT_CONTAGEM_1 <> QT_CONTAGEM_2)" +
                            " OR (QT_CONTAGEM_1 <> QT_CONTAGEM_3" +
                            ") OR (QT_CONTAGEM_1 <> QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_2 <> QT_CONTAGEM_3)" +
                            " OR (QT_CONTAGEM_2 <> QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_3 <> QT_CONTAGEM_4)";
                    }
                    if (shCon == 5)
                    {
                        strSQL += "(QT_CONTAGEM_1 <> QT_CONTAGEM_2)" +
                            " OR (QT_CONTAGEM_1 <> QT_CONTAGEM_3)" +
                            " OR (QT_CONTAGEM_1 <> QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_1 <> QT_CONTAGEM_5)" +
                            " OR (QT_CONTAGEM_2 <> QT_CONTAGEM_3)" +
                            " OR (QT_CONTAGEM_2 <> QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_2 <> QT_CONTAGEM_5)" +
                            " OR (QT_CONTAGEM_3 <> QT_CONTAGEM_4)" +
                            " OR (QT_CONTAGEM_3 <> QT_CONTAGEM_5)";
                    }
                    strSQL += ") ORDER BY CD_LOCALIZACAO, NM_PRODUTO";
                }




                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Grid: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public DataTable RelAcompanhamentoInventario(double CodInventario)
        {
            try
            {
                //// Cria DataTable
                DataTable dt = new DataTable();
                string strSQL = "Select * from [VW_INVENTARIO] WHERE  CD_INDEX_INVENTARIO = " + CodInventario.ToString() + "ORDER BY CD_LOCALIZACAO, NM_PRODUTO";

                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Obter Regra Fiscal de ICMS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void AtualizaContagem(decimal decIndice,short nrMov, decimal QtdCon, Boolean blnZerarQuantidade)
        {
            try
            {
                AbrirConexao();
                strSQL = "update ITEM_DO_INVENTARIO set QT_CONTAGEM_" + nrMov + " = @v1 Where CD_INDEX = @v2";


                if (blnZerarQuantidade == true)
                {
                    strSQL = "update ITEM_DO_INVENTARIO set QT_CONTAGEM_" + nrMov + " = 0 Where CD_INDEX = @v2";
                    Cmd = new SqlCommand(strSQL, Con);
                }
                else
                {
                    if (QtdCon == 0)
                    {
                        strSQL = "update ITEM_DO_INVENTARIO set QT_CONTAGEM_" + nrMov + " = NULL Where CD_INDEX = @v2";
                    }
                    Cmd = new SqlCommand(strSQL, Con);
                    Cmd.Parameters.AddWithValue("@v1", QtdCon);
                }

                Cmd.Parameters.AddWithValue("@v2", decIndice);

                

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void PesquisaEAtualizaInventario(decimal decIndice, short TlContagens, short nrMov, decimal QtdCon)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select * from ITEM_DO_INVENTARIO Where CD_INDEX = @v2", Con);
                //Cmd.Parameters.AddWithValue("@v1", decIndice);
                Cmd.Parameters.AddWithValue("@v2", decIndice);

                Dr = Cmd.ExecuteReader();
                if(Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ItemDoInventario p = new ItemDoInventario();
                        InventarioDAL RninventarioDAL = new InventarioDAL();

                        if (Dr["QT_CONTAGEM_1"] != DBNull.Value)
                        {
                            p.QtContagem = Convert.ToDecimal(Dr["QT_CONTAGEM_1"]);
                        }
                        if (Dr["QT_CONTAGEM_2"] != DBNull.Value)
                        {
                            p.QtContagem2 = Convert.ToDecimal(Dr["QT_CONTAGEM_2"]);
                        }
                        if (Dr["QT_CONTAGEM_3"] != DBNull.Value)
                        {
                            p.QtContagem3 = Convert.ToDecimal(Dr["QT_CONTAGEM_3"]);
                        }
                        if (Dr["QT_CONTAGEM_4"] != DBNull.Value)
                        {
                            p.QtContagem4 = Convert.ToDecimal(Dr["QT_CONTAGEM_4"]);
                        }
                        if (Dr["QT_CONTAGEM_5"] != DBNull.Value)
                        {
                            p.QtContagem5 = Convert.ToDecimal(Dr["QT_CONTAGEM_5"]);
                        }
                        if (TlContagens > 1)
                        {
                            if (nrMov == 1)
                            {
                                if (p.QtContagem2 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem3 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem4 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem5 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                            }
                            if (nrMov == 2)
                            {
                                if (p.QtContagem == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem3 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem4 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem5 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                            }
                            if (nrMov == 3)
                            {
                                if (p.QtContagem2 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem4 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem5 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                            }
                            if (nrMov == 4)
                            {
                                if (p.QtContagem2 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem3 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem5 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                            }
                            if (nrMov == 5)
                            {
                                if (p.QtContagem2 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem3 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem4 == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                if (p.QtContagem == QtdCon)
                                {
                                    RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                                    return;
                                }
                                return;
                            }
                        }
                        else
                        {
                            RninventarioDAL.AtualizaQtInventario(decIndice, QtdCon);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void AtualizaQtInventario(decimal decIndice, decimal QtdCon)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update ITEM_DO_INVENTARIO set QT_INVENTARIO = @v1 Where CD_INDEX = @v2", Con);
                Cmd.Parameters.AddWithValue("@v1", QtdCon);
                Cmd.Parameters.AddWithValue("@v2", decIndice);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ItemDoInventario> ListarItemInventario(decimal decIndice, int intNrCon)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT * FROM VW_INVENTARIO  WHERE CD_INDEX_INVENTARIO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", decIndice);

                Dr = Cmd.ExecuteReader();

                List<ItemDoInventario> lista = new List<ItemDoInventario>();

                while (Dr.Read())
                {
                    ItemDoInventario p = new ItemDoInventario();

                    p.CodigoIndice = Convert.ToDecimal(Dr["CD_INDEX"]);
                    p.CodigoIndiceEstoque = Convert.ToDecimal(Dr["CD_INDEX_ESTOQUE"]);
                    p.CodigoIndiceInventario = Convert.ToDecimal(Dr["CD_INDEX_INVENTARIO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.CodigoCategoria = Convert.ToString(Dr["CD_CATEGORIA"]);
                    if (Dr["QT_CONTAGEM_" + intNrCon + ""] != DBNull.Value)
                    {
                        p.QtContagem = Convert.ToDecimal(Dr["QT_CONTAGEM_" + intNrCon + ""]);
                    }
                    p.CplLote = Convert.ToString(Dr["CPL_LOTE"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.Und = Convert.ToString(Dr["SIGLA"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ItemDoInventario> ListarItemInventarioMan(decimal decIndice)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT * FROM VW_INVENTARIO  WHERE CD_INDEX_INVENTARIO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", decIndice);

                Dr = Cmd.ExecuteReader();

                List<ItemDoInventario> lista = new List<ItemDoInventario>();

                while (Dr.Read())
                {
                    ItemDoInventario p = new ItemDoInventario();

                    p.CodigoIndice = Convert.ToDecimal(Dr["CD_INDEX"]);
                    p.CodigoIndiceEstoque = Convert.ToDecimal(Dr["CD_INDEX_ESTOQUE"]);
                    p.CodigoIndiceInventario = Convert.ToDecimal(Dr["CD_INDEX_INVENTARIO"]);
                    p.CodigoLocalizacao = Convert.ToString(Dr["CD_LOCALIZACAO"]);
                    p.CodigoCategoria = Convert.ToString(Dr["CD_CATEGORIA"]);
                    p.QtInventario = Convert.ToDecimal(Dr["QT_INVENTARIO"]);
                    p.CplLote = Convert.ToString(Dr["CPL_LOTE"]);
                    p.NomeProduto = Convert.ToString(Dr["NM_PRODUTO"]);
                    p.Und = Convert.ToString(Dr["SIGLA"]);
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Qt_inventario(decimal decIndice, short shortCon, decimal valor)
        {
            try
            {
                AbrirConexao();

                string strCampos = " CD_INDEX, QT_CONTAGEM_1, QT_CONTAGEM_2, QT_CONTAGEM_3, QT_CONTAGEM_4, QT_CONTAGEM_5, CD_INDEX_INVENTARIO ";
                string strSQL = "SELECT " + strCampos +
                    "FROM ITEM_DO_INVENTARIO " +
                    "WHERE CD_INDEX_INVENTARIO = @v1" +
                    " AND(QT_CONTAGEM_1 > 0 " +
                    "OR QT_CONTAGEM_2 > 0 " +
                    "OR QT_CONTAGEM_3 > 0 " +
                    "OR QT_CONTAGEM_4 > 0 " +
                    "OR QT_CONTAGEM_5 > 0)" +
                    "GROUP BY " + strCampos + 
                    "HAVING(QT_CONTAGEM_1 = QT_CONTAGEM_2 OR " +
                    "QT_CONTAGEM_1 = QT_CONTAGEM_3 OR " +
                    "QT_CONTAGEM_1 = QT_CONTAGEM_4 OR " +
                    "QT_CONTAGEM_1 = QT_CONTAGEM_5) OR " +
                    "(QT_CONTAGEM_2 = QT_CONTAGEM_3 OR " +
                    "QT_CONTAGEM_2 = QT_CONTAGEM_4 OR " +
                    "QT_CONTAGEM_2 = QT_CONTAGEM_5) OR " +
                    "(QT_CONTAGEM_3 = QT_CONTAGEM_4 OR " +
                    "QT_CONTAGEM_3 = QT_CONTAGEM_5) OR" +
                    "(QT_CONTAGEM_4 = QT_CONTAGEM_5)";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", decIndice);

                Dr = Cmd.ExecuteReader();

                while (Dr.Read())
                {
                    ItemDoInventario p = new ItemDoInventario();


                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void AtualizaSituacaoCancelamento(decimal decIndice)
        {
            try
            {
                AbrirConexao();
                strSQL = "update ITEM_DO_INVENTARIO set CD_SITUACAO = 131 Where CD_INDEX = @v2";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v2", decIndice);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool  CancelarInventario(int lngIndice, int cdusu)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL(" UPDATE [INVENTARIO] SET CD_SITUACAO = 131, DT_CANCELAMENTO = GETDATE(), CD_USU_CANCELAMENTO = " + cdusu + "  Where CD_INDEX = " + lngIndice.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Cancelar Inventario: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
        public bool EncerrarInventario(int lngIndice, int cdusu)
        {
            try
            {
                return new DBTabelaDAL().ExecutaComandoSQL(" UPDATE [INVENTARIO] SET CD_SITUACAO = 132, DT_ENCERRAMENTO = GETDATE(), CD_USU_ENCERRAMENTO = " + cdusu + "  Where CD_INDEX = " + lngIndice.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Cancelar Inventario: " + ex.Message.ToString());
            }
            finally
            {
            }
        }
    }
}