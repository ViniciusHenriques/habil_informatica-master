using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DAL.Persistence
{
    public class RelatorioContratosDAL
    {
        protected SqlConnection Con;

        protected SqlCommand Cmd;

        protected SqlDataReader Dr;

        protected void AbrirConexao()
        {
            try
            {
                Con = new SqlConnection(@"Data Source=192.168.0.18\SQLserver2008;Initial Catalog=Fabesul;User ID=sa;Password=habil");

                Con.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void FecharConexao()
        {
            try
            {
                Con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    
        public DataTable PesquisarProduto(string strCodigoProduto)
        {
            try
            {
                DataTable dt = new DataTable();

                AbrirConexao();

                string comando = "select situ,sitt,orig,peso,ISNULL(CD_CEST,' ') AS CD_CEST from produt where cpro = @v1 ";


                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", strCodigoProduto);

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);

                FecharConexao();

                return dt;

                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable ListarGruposCliente()
        {
            try
            {
                DataTable dt = new DataTable();

                AbrirConexao();

                string comando = "select " +
                                    "	(convert(nvarchar,isnull(CD_CLI_MTZ,0)) +'³'+ convert(nvarchar,cgru)) AS matriz_cgru, (cgru + ' | ' + nome) as nome " +
                                "from " +
                                    "cliengr " +
                                "where (select " +
                                       "count(cli.ccli) " +
                                    "from " +
                                        "cliente as cli " +
                                    "where " +
                                        "cli.ativ = 'Sim' AND(" +
                                            "select " +
                                                "count(ct.ccli)  " +
                                            "from " +
                                                "ClienPr as ct " +
                                            "where " +
                                                "tipo = 'CT' and ct.ccli = cli.ccli " +
                                        ") > 0 " +
                                        "AND cli.cgru = cliengr.cgru) > 0 order by cgru";


                Cmd = new SqlCommand(comando, Con);

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);

                FecharConexao();

                return dt;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string PesquisarGrupoDoProduto(string strCodigoProduto)
        {
            try
            {
                DataTable dt = new DataTable();

                AbrirConexao();

                string comando = "SELECT g.cgru + ' - ' + g.dgru as dgru FROM Produt as p " +
                                "inner join grupo as g on g.cgru = p.cgru where p.cpro = @v1";

                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", strCodigoProduto);
                Dr = Cmd.ExecuteReader();

                string DescricaoGrupoProduto = "";

                if (Dr.Read())
                    DescricaoGrupoProduto = Convert.ToString(Dr["dgru"]);
                else
                    DescricaoGrupoProduto  = "";

                FecharConexao();

                return DescricaoGrupoProduto;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable PesquisarClientePorGrupo(string strCodigoGrupo)
        {
            try
            {
                DataTable dt = new DataTable();

                AbrirConexao();

                string comando = "select " +
                                    "cli.ccli,(CONVERT(varchar(18), cli.ccli) + ' | ' + cli.nomecli ) as nomecli " +
                                "from " +
                                    "cliente as cli " +
                                "where " +
                                    "cli.ativ = 'Sim' AND( " +
                                    "select " +
                                        "count(ct.ccli) " +
                                    "from " +
                                        "ClienPr as ct " +
                                    "where " +
                                        "tipo = 'CT' and ct.ccli = cli.ccli) > 0 AND cli.cgru = @v1 order by ccli ";

                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", strCodigoGrupo);
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);

                FecharConexao();

                return dt;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable ExecutaSpImpCliCT(string CodigoEmpresa, string CodigoCliente, bool PrecoUnico, bool CalculaST, bool CodigoProdDocumento, string strEstados)
        {
            DataTable dtSP = new DataTable();
            AbrirConexao();
            try
            {
                SqlCommand sqlComand = new SqlCommand("SP_Impclict ", Con);

                sqlComand.CommandType = CommandType.StoredProcedure;
                sqlComand.Parameters.AddWithValue("@CodEmpresa", CodigoEmpresa);
                sqlComand.Parameters.AddWithValue("@CodCliente", CodigoCliente);
                sqlComand.Parameters.AddWithValue("@Grupo", "");
                sqlComand.Parameters.AddWithValue("@ESTADOS", strEstados);

                if(PrecoUnico)
                    sqlComand.Parameters.AddWithValue("@PrecoUnico", "S");
                else
                    sqlComand.Parameters.AddWithValue("@PrecoUnico", "N");

                if (CalculaST)
                    sqlComand.Parameters.AddWithValue("@SomarSTQdoNaoEstRS", "S");
                else
                    sqlComand.Parameters.AddWithValue("@SomarSTQdoNaoEstRS", "N");

                if (CodigoProdDocumento)
                    sqlComand.Parameters.AddWithValue("@OrdItemLancamento", "S");
                else
                    sqlComand.Parameters.AddWithValue("@OrdItemLancamento", "N");

                SqlDataAdapter da = new SqlDataAdapter(sqlComand);
                sqlComand.CommandTimeout = 100000;
                da.Fill(dtSP);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Executar Sp ImpCliCT: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
            return dtSP;
        }
        public DataTable ExecutaSpImpCliCTm(string CodigoEmpresa, string CodigoCliente, bool PrecoUnico, bool CalculaST, bool CodigoProdDocumento, string strEstados)
        {
            DataTable dtSP = new DataTable();
            AbrirConexao();
            try
            {
                SqlCommand sqlComand = new SqlCommand("SP_Impclictm ", Con);

                sqlComand.CommandType = CommandType.StoredProcedure;
                sqlComand.Parameters.AddWithValue("@CodEmpresa", CodigoEmpresa);
                sqlComand.Parameters.AddWithValue("@CodCliente", CodigoCliente);
                sqlComand.Parameters.AddWithValue("@Grupo", "");
                sqlComand.Parameters.AddWithValue("@ESTADOS", strEstados);

                if (PrecoUnico)
                    sqlComand.Parameters.AddWithValue("@PrecoUnico", "S");
                else
                    sqlComand.Parameters.AddWithValue("@PrecoUnico", "N");

                if (CalculaST)
                    sqlComand.Parameters.AddWithValue("@SomarSTQdoNaoEstRS", "S");
                else
                    sqlComand.Parameters.AddWithValue("@SomarSTQdoNaoEstRS", "N");

                if (CodigoProdDocumento)
                    sqlComand.Parameters.AddWithValue("@OrdItemLancamento", "S");
                else
                    sqlComand.Parameters.AddWithValue("@OrdItemLancamento", "N");
                sqlComand.Parameters.AddWithValue("@CodProdutoPesquisa", 0);

                SqlDataAdapter da = new SqlDataAdapter(sqlComand);
                sqlComand.CommandTimeout = 100000;
                da.Fill(dtSP);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Executar Sp ImpCliCT: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
            return dtSP;
        }
    }
}
