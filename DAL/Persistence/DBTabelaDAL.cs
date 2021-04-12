using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data.SqlClient;
using System.Xml;
using System.Data;

namespace DAL.Persistence
{
    public class DBTabelaDAL : Conexao
    {
        public string ListarTipoCampoSQL(string strNomeDaTabela, string strNomeDoCampo)
        {
            try
            {
                AbrirConexao();

                string strCampo = "";

                string strSQL = "SELECT TIPOS.NAME AS TIPO ";
                strSQL = strSQL + " FROM SYSOBJECTS TABELAS ";
                strSQL = strSQL + "   INNER JOIN SYSCOLUMNS COLUNAS ";
                strSQL = strSQL + "     ON (TABELAS.ID = COLUNAS.ID) ";
                strSQL = strSQL + "   INNER JOIN SYSTYPES TIPOS ";
                strSQL = strSQL + "     ON (COLUNAS.USERTYPE = TIPOS.USERTYPE) and (COLUNAS.xtype = TIPOS.xtype)  ";
                strSQL = strSQL + "   INNER JOIN HABIL_PESQUISA_FILTRO PF ";
                strSQL = strSQL + "     ON (COLUNAS.NAME = PF.DS_CAMPO) and (TABELAS.NAME = PF.DS_TABELA)  ";
                strSQL = strSQL + " WHERE TABELAS.XTYPE = 'U' AND TABELAS.NAME = '" + strNomeDaTabela + "' AND COLUNAS.NAME = '" + strNomeDoCampo + "' AND COLUNAS.xtype <> 99 ORDER BY TABELAS.NAME ";


                


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                {
                    strCampo = Convert.ToString(Dr["Tipo"]);
                }

                return strCampo;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Campo da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public string ListarTipoCampoXML(string strNomeDaTabela, string strNomeDoCampo)
        {
            try
            {

                string strCampo = "";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@"D:\HabilWeb\Habil_Informatica\Habil_Informatica\Habil_Informatica\XML\XMLColumnsTables.xml"); //Carregando o arquivo

                //Pegando elemento pelo nome da TAG
                XmlNodeList xnList = xmlDoc.GetElementsByTagName("row");

                //Usando for para imprimir na tela
                for (int i = 0; i < xnList.Count; i++)
                {
                    if ((strNomeDaTabela == xnList[i]["TABELA"].InnerText) && (strNomeDoCampo == xnList[i]["COLUNA"].InnerText))
                        strCampo = Convert.ToString(xnList[i]["TIPO"].InnerText);

                }
                return strCampo;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar ListarTipoCampo: " + ex.Message.ToString());
            }
            finally
            {
            }

        }
        public string ListarTipoCampoView(string strNomeDaView, string strNomeDoCampo)
        {
            try
            {
                AbrirConexao();

                string strCampo = "";

                string strSQL = " SELECT TIPOS.NAME AS TIPO ";
                strSQL = strSQL + " FROM sys.all_views TABELAS INNER JOIN sys.all_columns COLUNAS ON (TABELAS.object_id = COLUNAS.object_id) ";
                strSQL = strSQL + " INNER JOIN SYSTYPES TIPOS ON (COLUNAS.system_type_id = TIPOS.xtype ) Where TABELAS.object_id > 0 and tipos.name <> 'sysname' ";
                strSQL = strSQL + " AND TABELAS.NAME = '" + strNomeDaView + "' AND COLUNAS.NAME = '" + strNomeDoCampo + "' ";
                strSQL = strSQL + " order BY COLUNAS.column_id ";


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                {
                    strCampo = Convert.ToString(Dr["Tipo"]);
                }

                return strCampo;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Campo da Tabela VIEW: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public List<DBTabela> ListarCamposView(string strNomeDaView)
        {
            try
            {
                AbrirConexao();

                string strSQL = " SELECT TABELAS.NAME AS TABELA, COLUNAS.NAME AS COLUNA, TIPOS.NAME AS TIPO, COLUNAS.IS_NULLABLE AS EH_NULO, PF.VW_DS_ALIAS, REG_UNICO, POP_TABELA   ";
                strSQL = strSQL + " FROM sys.all_views TABELAS ";
                strSQL = strSQL + "   INNER JOIN sys.all_columns COLUNAS ";
                strSQL = strSQL + "      ON (TABELAS.object_id = COLUNAS.object_id) ";
                strSQL = strSQL + "   INNER JOIN SYSTYPES TIPOS ";
                strSQL = strSQL + "      ON (COLUNAS.system_type_id = TIPOS.xtype ) ";
                strSQL = strSQL + "   INNER JOIN HABIL_PESQUISA_FILTRO PF ";
                strSQL = strSQL + "     ON (COLUNAS.NAME = PF.DS_CAMPO) and (TABELAS.NAME = PF.DS_TABELA)  ";
                strSQL = strSQL + "  Where TABELAS.object_id > 0 and tipos.name <> 'sysname' ";
                strSQL = strSQL + "    and TABELAS.NAME = '" + strNomeDaView + "'";
                strSQL = strSQL + " ORDER BY TABELAS.NAME, PF.ORDEM ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<DBTabela> lista = new List<DBTabela>();

                while (Dr.Read())
                {
                    DBTabela rowp = new DBTabela();

                    rowp.Tabela = Convert.ToString(Dr["Tabela"]);
                    rowp.Coluna = Convert.ToString(Dr["Coluna"]);
                    rowp.Tipo = Convert.ToString(Dr["Tipo"]);
                    rowp.Tamanho = 50;
                    rowp.Nulo = Convert.ToInt32(Dr["Eh_Nulo"]);
                    rowp.NomeComum = Convert.ToString(Dr["VW_DS_ALIAS"]);
                    rowp.RegistroUnico = Convert.ToInt32(Dr["REG_UNICO"]);
                    rowp.PopulaTabela = (Dr["POP_TABELA"].ToString());
                    lista.Add(rowp);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public List<DBTabela> ListarCamposSQL(string strNomeDaTabela)
        {
            try
            {
                AbrirConexao();

                string strSQL = " SELECT VW.MAJOR_ID, VW.MINOR_ID, TABELAS.NAME AS TABELA, COLUNAS.NAME AS COLUNA, TIPOS.NAME AS TIPO, COLUNAS.LENGTH AS TAMANHO, COLUNAS.ISNULLABLE AS EH_NULO, VW.DESCRICAOCAMPO ";
                strSQL = strSQL + " FROM SYSOBJECTS TABELAS ";
                strSQL = strSQL + "   INNER JOIN SYSCOLUMNS COLUNAS ";
                strSQL = strSQL + "     ON (TABELAS.ID = COLUNAS.ID) ";
                strSQL = strSQL + "   INNER JOIN Vw_Table_Columns_Description AS VW ";
                strSQL = strSQL + "     ON TABELAS.NAME = VW.NOMETABELA AND COLUNAS.NAME = VW.NOMECOLUNA ";
                strSQL = strSQL + "   INNER JOIN SYSTYPES TIPOS ON (COLUNAS.USERTYPE = TIPOS.USERTYPE) AND (COLUNAS.xtype = TIPOS.xtype ) ";
                strSQL = strSQL + "   INNER JOIN HABIL_PESQUISA_FILTRO PF ";
                strSQL = strSQL + "     ON (COLUNAS.NAME = PF.DS_CAMPO) and (TABELAS.NAME = PF.DS_TABELA)  ";
                strSQL = strSQL + " WHERE TABELAS.XTYPE = 'U' AND TABELAS.NAME = '" + strNomeDaTabela + "' AND COLUNAS.XTYPE <> 99 ";
                strSQL = strSQL + " ORDER BY TABELAS.NAME, PF.ORDEM ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<DBTabela> lista = new List<DBTabela>();

                while (Dr.Read())
                {
                    DBTabela rowp = new DBTabela();

                    rowp.IDTabela = Convert.ToInt64(Dr["MAJOR_ID"]);
                    rowp.IDCampo = Convert.ToInt64(Dr["MINOR_ID"]);
                    rowp.Tabela = Convert.ToString(Dr["Tabela"]);
                    rowp.Coluna = Convert.ToString(Dr["Coluna"]);
                    rowp.NomeComum = Convert.ToString(Dr["DescricaoCampo"]);
                    rowp.Tipo = Convert.ToString(Dr["Tipo"]);

                    switch (Convert.ToString(Dr["Tipo"]).ToUpper())
                    {
                        case "INT":
                            rowp.Tamanho = 8;
                            break;

                        case "SMALLINT":
                            rowp.Tamanho = 4;
                            break;

                        default:
                            rowp.Tamanho = Convert.ToInt32(Dr["Tamanho"]);
                            break;
                    }

                    rowp.Nulo = Convert.ToInt32(Dr["Eh_Nulo"]);

                    lista.Add(rowp);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public List<DBTabela> ListarCamposXML(string strNomeDaTabela)
        {
            try
            {
                List<DBTabela> lista = new List<DBTabela>();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@"D:\Web\Habil_Informatica\Habil_Informatica_Original\Habil_Informatica_Original\XML\XMLColumnsTables.xml"); //Carregando o arquivo
                

                //Pegando elemento pelo nome da TAG
                XmlNodeList xnList = xmlDoc.GetElementsByTagName("row");

                //Usando for para imprimir na tela
                for (int i = 0; i < xnList.Count; i++)
                {
                    if  (strNomeDaTabela == xnList[i]["TABELA"].InnerText)
                    {
                        DBTabela rowp = new DBTabela();

                        rowp.Tabela = Convert.ToString(xnList[i]["TABELA"].InnerText);
                        rowp.NomeComum = Convert.ToString(xnList[i]["DESCRICAO"].InnerText);
                        rowp.Coluna = Convert.ToString(xnList[i]["COLUNA"].InnerText);
                        rowp.Tipo = Convert.ToString(xnList[i]["TIPO"].InnerText);

                        switch (Convert.ToString(xnList[i]["TIPO"].InnerText).ToUpper())
                        {
                            case  "INT" :
                                rowp.Tamanho = 8;
                                break;

                            case "SMALLINT":
                                rowp.Tamanho = 4;
                                break;

                            default :
                                rowp.Tamanho = Convert.ToInt32(xnList[i]["TAMANHO"].InnerText);
                                break;
                        }

                        rowp.Nulo = Convert.ToInt32(xnList[i]["EH_NULO"].InnerText);

                        lista.Add(rowp);

                    }

                }
                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
            }

        }
        public bool ExecutaComandoSQL(string strComando)

        {

            try

            {

                AbrirConexao();



                Cmd = new SqlCommand(strComando, Con);



                Dr = Cmd.ExecuteReader();



                return true;



            }

            catch (Exception ex)

            {

                return false;



                throw new Exception("Erro ao Executar Comando: " + strComando + "Erro: " + ex.Message.ToString());

            }

            finally

            {

                FecharConexao();

            }



        }
        public long PesquisaIDTabelaSQL(string strNomeDaTabela)
        {
            try
            {
                AbrirConexao();

                string strSQL = " SELECT VW.MAJOR_ID";
                strSQL = strSQL + " FROM Vw_Table_Columns_Description AS VW ";
                strSQL = strSQL + " WHERE NOMETABELA = '" + strNomeDaTabela + "' ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                    return Convert.ToInt64(Dr["MAJOR_ID"]);
                else
                    return 0;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public long PesquisaIDTabelaCampoSQL(string strNomeDaTabela, string strNomeDaCampo)
        {
            try
            {
                AbrirConexao();

                string strSQL = " SELECT VW.MINOR_ID";
                strSQL = strSQL + " FROM Vw_Table_Columns_Description AS VW ";
                strSQL = strSQL + " WHERE NOMETABELA = '" + strNomeDaTabela + "' AND NOMECOLUNA = '" + strNomeDaCampo + "' ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                    return Convert.ToInt64(Dr["MINOR_ID"]);
                else
                    return 0;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public bool CriaBancoInicial(string ServidorInstancia, string NomeDoBanco, string UsuarioSQLServer, string SenhaSQLServer)

        {

            try

            {

                ExecutaComandoSQLMaster("CREATE DATABASE [" + NomeDoBanco + "] ON(NAME = N'" + NomeDoBanco + "', FILENAME = N'D:\\MSSQL\\" + NomeDoBanco + ".mdf', SIZE = 1024MB, FILEGROWTH = 256MB) LOG ON(NAME = N'" + NomeDoBanco  + "_log', FILENAME = N'D:\\MSSQL\\" + NomeDoBanco  + "_log.ldf', SIZE = 512MB, FILEGROWTH = 125MB)                                                      ", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET COMPATIBILITY_LEVEL = 100", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("IF(1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')) begin EXEC [" + NomeDoBanco + "].[dbo].[sp_fulltext_database] @action = 'enable' end", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET ANSI_NULL_DEFAULT OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET ANSI_NULLS OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET ANSI_PADDING OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET ANSI_WARNINGS OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET ARITHABORT OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET AUTO_CLOSE OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET AUTO_SHRINK OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET AUTO_UPDATE_STATISTICS ON", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET CURSOR_CLOSE_ON_COMMIT OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET CURSOR_DEFAULT  GLOBAL", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET CONCAT_NULL_YIELDS_NULL OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET NUMERIC_ROUNDABORT OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET QUOTED_IDENTIFIER OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET RECURSIVE_TRIGGERS OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET DISABLE_BROKER", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET AUTO_UPDATE_STATISTICS_ASYNC OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET DATE_CORRELATION_OPTIMIZATION OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET TRUSTWORTHY OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET ALLOW_SNAPSHOT_ISOLATION OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET PARAMETERIZATION SIMPLE", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET READ_COMMITTED_SNAPSHOT OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET HONOR_BROKER_PRIORITY OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET RECOVERY SIMPLE", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET  MULTI_USER", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET PAGE_VERIFY CHECKSUM", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET DB_CHAINING OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF)", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET TARGET_RECOVERY_TIME = 0 SECONDS", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET DELAYED_DURABILITY = DISABLED", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                ExecutaComandoSQLMaster("ALTER DATABASE [" + NomeDoBanco + "] SET QUERY_STORE = OFF", ServidorInstancia, NomeDoBanco, UsuarioSQLServer, SenhaSQLServer);



                return true;



            }

            catch (Exception ex)

            {

                return false;



                throw new Exception("Erro ao Criar Banco. Erro: " + ex.Message.ToString());

            }

            finally

            {

            }



        }
        public bool ExecutaComandoSQLMaster(string strComando, string ServidorInstancia, string NomeDoBanco, string UsuarioSQLServer, string SenhaSQLServer)

        {

            try

            {

                AbrirConexaoMaster(ServidorInstancia, "MASTER", UsuarioSQLServer, SenhaSQLServer);



                CmdDbMaster = new SqlCommand(strComando, ConDbMaster);



                DrMaster = CmdDbMaster.ExecuteReader();



                return true;



            }

            catch (Exception ex)

            {

                return false;



                throw new Exception("Erro ao Executar Comando: " + strComando + "Erro: " + ex.Message.ToString());

            }

            finally

            {

                FecharConexaoMaster();

            }



        }
        public Int32 BuscaIDTabelaCampo(string nomeTabela, string nomeCampo){

            try
            {
                AbrirConexao();

                string strSQL = " SELECT *";
                strSQL = strSQL + " FROM HABIL_ESTRUTURA_BANCO ";
                strSQL = strSQL + " WHERE TX_TABELA = '" + nomeTabela+ "' AND TX_CAMPO= '" + nomeCampo+ "' ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();


                if (Dr.Read())
                    return Convert.ToInt32(Dr["CD_INDEX"]);
                else
                    return 0;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DateTime ObterDataHoraServidor()
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("SELECT GETDATE()", Con);
                return (DateTime)Cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Data/Hora Servidor: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public string BuscaTabelas(string nomeTabela)
        {

            try
            {
                AbrirConexao();

                string strSQL = " SELECT *";
                strSQL = strSQL + " FROM VW_TB_CL";
                strSQL = strSQL + " WHERE TABELA = '" + nomeTabela+"'";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();


                if (Dr.Read())
                    return (Dr["TABELA"].ToString());
                else
                    return "";

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<DBTabela> ListarEstruturasBanco(long lngDe, long lngAte)
        {
            try
            {
                AbrirConexao();

                string strSQL = " SELECT * FROM STRUCT_DATABASE ";

                if (lngDe > 0)
                {
                    strSQL = strSQL + " Where CD_STR_DATABASE > " + lngDe.ToString();
                    if (lngAte > 0)
                    {
                        strSQL = strSQL + " and CD_STR_DATABASE <= " + lngAte.ToString();
                    }
                }
                else
                {
                    if (lngAte > 0)
                    {
                        strSQL = strSQL + " Where CD_STR_DATABASE <= " + lngAte.ToString();
                    }

                }

                strSQL = strSQL + " Order by CD_STR_DATABASE ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<DBTabela> lista = new List<DBTabela>();

                while (Dr.Read())
                {
                    DBTabela rowp = new DBTabela();
                    rowp.BancoEstrutura = Convert.ToString(Dr["TX_SCRIPT"]);
                    rowp.NomeComum = Convert.ToInt64(Dr["CD_STR_DATABASE"]) + " - " + Convert.ToString(Dr["DS_STR_DATABASE"]);
                    rowp.IDTabela = Convert.ToInt64(Dr["CD_STR_DATABASE"]);
                    lista.Add(rowp);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public void GravarEstruturasBanco(DBTabela p)
        {
            try
            {
                AbrirConexao();

                string strSQL = " INSERT INTO STRUCT_DATABASE (DS_STR_DATABASE, TX_SCRIPT) VALUES (@v2, @v3)";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v2", p.NomeComum);
                Cmd.Parameters.AddWithValue("@v3", p.BancoEstrutura);

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
                            throw new Exception("Erro ao gravar Empresa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Script: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }

        public String PesquisaBancoExiste(string strDoBanco, string ServidorInstancia, string UsuarioSQLServer, string SenhaSQLServer)
        {
            try
            {

                AbrirConexaoMaster(ServidorInstancia, "MASTER", UsuarioSQLServer, SenhaSQLServer);
                string strSQL = " SELECT * FROM sys.databases WHERE name = '" + strDoBanco + "'";

                CmdDbMaster = new SqlCommand(strSQL, ConDbMaster);

                DrMaster = CmdDbMaster.ExecuteReader();

                if (DrMaster.Read())
                    return DrMaster["name"].ToString();
                else
                    return "";

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Banco Existe: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexaoMaster();
            }

        }

        public DataTable PopularTabela(string strNomeTabela)
        {
            strNomeTabela = strNomeTabela.ToUpper();

            DataTable dt = new DataTable();

            dt.Columns.Add("CodigoDrop", typeof(Int32));
            dt.Columns.Add("DescricaoDrop", typeof(string));



            switch (strNomeTabela)
            { 
                case "ESTADO":
                {
                        RegFisIcmsDAL eDAL = new RegFisIcmsDAL();
                        DataTable dt2 = new DataTable();
                        dt2 = (DataTable)eDAL.ListarRegFisIcmsLocalizacoes();
                        dt = dt2;
                        break;
                }
                case "GRUPO_DE_PESSOA":
                {
                        GrupoPessoaDAL eDAL = new GrupoPessoaDAL();
                        DataTable dt2 = new DataTable();
                        dt2 = (DataTable)eDAL.ObterGrupoPessoa();
                        dt = dt2;
                        break;
                }
                case "GPO_PESSOA":
                    {
                        RegFisIcmsDAL eDAL = new RegFisIcmsDAL();
                        DataTable dt2 = new DataTable();
                        dt2 = (DataTable)eDAL.ListarRegFisIcmsGrupoPessoas();
                        dt = dt2;
                        break;
                    }
                case "GPO_PRODUTO":
                    {
                        RegFisIcmsDAL eDAL = new RegFisIcmsDAL();
                        DataTable dt2 = new DataTable();
                        dt2 = (DataTable)eDAL.ListarRegFisIcmsGrupoProdutos();
                        dt = dt2;
                        break;
                    }
                case "APLICACAO":
                    {
                        RegFisIcmsDAL eDAL = new RegFisIcmsDAL();
                        DataTable dt2 = new DataTable();
                        dt2 = (DataTable)eDAL.ListarRegFisIcmsAplicacao();
                        dt = dt2;
                        break;
                    }
                case "TIPO_OPER_FISCAL":
                    {
                        RegFisIcmsDAL eDAL = new RegFisIcmsDAL();
                        DataTable dt2 = new DataTable();
                        dt2 = (DataTable)eDAL.ListarRegFisIcmsTpOpFiscal();
                        dt = dt2;
                        break;
                    }
                default:
                    {
                        break;
                }
            }

            return dt;
        }

        public DataTable ListarPesquisaFiltro(string strTabela, string strCampo)
        {
            try
            {
                DataTable dt = new DataTable();

                string strSQL = " SELECT * FROM HABIL_PESQUISA_FILTRO ";

                if (strTabela != "")
                {
                    strSQL = strSQL + " Where DS_TABELA  '" + strTabela + "'";
                    if (strCampo != "")
                    {
                        strSQL = strSQL + " and DS_CAMPO = '" + strCampo + "'";
                    }
                }
                else
                {
                    if (strCampo != "")
                    {
                        strSQL = strSQL + " WHERE DS_CAMPO = '" + strCampo + "'";
                    }

                }

                strSQL = strSQL + " Order by CD_PESQUISA_FILTRO ";


                AbrirConexao();
                SqlCommand cmd = new SqlCommand(strSQL, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Dados da Tabela: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }

    }
}
