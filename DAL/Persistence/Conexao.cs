using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Configuration;
using System.Reflection;
namespace DAL.Persistence
{
    public class Conexao
    {
        protected SqlConnection Con, ConDbMaster;

        protected SqlCommand Cmd, CmdDbMaster;

        protected SqlDataReader Dr, DrMaster;
        protected string SQLLEGAL = "";
        protected void AbrirConexao()
        {
            try
            {
                Con = new SqlConnection(ObterConexaoString());
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
        public string MontaFiltro(string strNomeCampo, string strTipoCampo, string strValor)
        {
            string strFiltro = "";

            if (strValor != "")
            {
                if ((strTipoCampo == "INT") || (strTipoCampo == "SMALLINT") || (strTipoCampo == "NUMERIC"))
                    strFiltro = strNomeCampo + " = " + strValor;

                if ((strTipoCampo == "VARCHAR") || (strTipoCampo == "NVARCHAR") || (strTipoCampo == "CHAR"))
                    strFiltro = strNomeCampo + " LIKE '%" + strValor + "%'";
            }
            return strFiltro;
        }
        public string MontaFiltroIntervalo(List<DBTabelaCampos> ListaFiltros)
        {
            string strFiltro = "";

            foreach (DBTabelaCampos p in ListaFiltros)
            {

                if  ((p.Fim != "*Nenhum Selecionado") && (p.Inicio != "*Nenhum Selecionado"))
                { 
                    if ( (p.Tipo.ToUpper() == "DATETIME") || (p.Tipo.ToUpper() == "SMALLDATETIME") )
                    {
                        if (strFiltro == "")
                            strFiltro = strFiltro + " WHERE ";
                        else
                            strFiltro = strFiltro + " AND ";

                        if ((p.Fim != "") && (p.Inicio != ""))
                        {
                            strFiltro = strFiltro + p.Filtro + " >= '" + Convert.ToDateTime(p.Inicio).ToString("yyyy-MM-dd") + " 00:00:00' AND " + p.Filtro + " <= '" + Convert.ToDateTime(p.Fim).ToString("yyyy-MM-dd") + " 23:59:59.999'";
                        }
                        else
                        {
                            if (p.Inicio != "")
                                strFiltro = strFiltro + p.Filtro + " >= '" + Convert.ToDateTime(p.Inicio).ToString("yyyy-MM-dd") + " 00:00:00' AND " + p.Filtro + " <= '" + Convert.ToDateTime(p.Inicio).ToString("yyyy-MM-dd") + " 23:59:59.999'";
                            if (p.Fim != "")
                                strFiltro = strFiltro + p.Filtro + " >= '" + Convert.ToDateTime(p.Fim).ToString("yyyy-MM-dd") + " 00:00:00' AND " + p.Filtro + " <= '" + Convert.ToDateTime(p.Fim).ToString("yyyy-MM-dd") + " 23:59:59.999'";
                        }
                    }

                    if ((p.Tipo.ToUpper() == "INT") || (p.Tipo.ToUpper() == "SMALLINT") || (p.Tipo.ToUpper() == "NUMERIC"))
                    {
                        if (strFiltro == "")
                            strFiltro = strFiltro + " WHERE ";
                        else
                            strFiltro = strFiltro + " AND ";

                        if ((p.Fim != "") && (p.Inicio != ""))
                        {
                            strFiltro = strFiltro + p.Filtro + " >= " + p.Inicio + " AND " + p.Filtro + "<= " + p.Fim;
                        }
                        else
                        {
                            if (p.Inicio != "")
                                strFiltro = strFiltro + p.Filtro + " = " + p.Inicio;
                            if (p.Fim != "")
                                strFiltro = strFiltro + p.Filtro + " = " + p.Fim;
                        }
                    }

                    if ((p.Tipo.ToUpper() == "VARCHAR") || (p.Tipo.ToUpper() == "NVARCHAR") || (p.Tipo.ToUpper() == "CHAR"))
                    {
                        if (strFiltro == "")
                            strFiltro = strFiltro + " WHERE ";
                        else
                            strFiltro = strFiltro + " AND ";


                        if ((p.Fim != "") && (p.Inicio != ""))
                        {
                            strFiltro = strFiltro + p.Filtro + " >= '" + p.Inicio + "' AND " + p.Filtro + " <= '" + p.Fim + "'";
                        }
                        else
                        {
                            if (p.SemLike == true)
                            {
                                if (p.Inicio != "")
                                    strFiltro = strFiltro + p.Filtro + " = '" + p.Inicio + "'";
                                if (p.Fim != "")
                                    strFiltro = strFiltro + p.Filtro + " = '" + p.Fim + "'";
                            }
                            else
                            {

                                if (p.Inicio != "")
                                    strFiltro = strFiltro + p.Filtro + " LIKE '%" + p.Inicio + "%'";
                                if (p.Fim != "")
                                    strFiltro = strFiltro + p.Filtro + " LIKE '%" + p.Fim + "%'";
                            }
                        }
                    }
                }
            }
            return strFiltro;
        }
        protected void AbrirConexaoMaster(string ServidorInstancia, string NomeDoBanco, string UsuarioSQLServer, string SenhaSQLServer)

        {

            try

            {

                ConDbMaster = new SqlConnection("Data Source=" + ServidorInstancia + ";Initial Catalog=" + NomeDoBanco + ";User ID=" + UsuarioSQLServer + ";Password=" + SenhaSQLServer);

                ConDbMaster.Open();

            }

            catch (Exception ex)

            {

                throw new Exception(ex.Message);

            }

        }
        protected void FecharConexaoMaster()

        {

            try

            {

                ConDbMaster.Close();

            }

            catch (Exception ex)

            {

                throw new Exception(ex.Message);

            }

        }

        protected string ObterConexaoString()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings["HabilInfoCS"].ConnectionString; /// + ";Connection Timeout=90;";
            }
            catch (Exception ex)
            {
                /*string serviceName = string.Empty;
                Assembly executingAssembly = Assembly.GetAssembly(typeof(ProjectInstaller));
                string targetDir = executingAssembly.Location;
                Configuration config = ConfigurationManager.OpenExeConfiguration(targetDir);
                serviceName = config.AppSettings.Settings["ServiceName"].Value.ToString();
                
                return serviceName*/
                return ConfigurationManager.AppSettings["HabilInfoCS"].ToString(); /// + ";Connection Timeout=90;";
            }
        }

    }
}