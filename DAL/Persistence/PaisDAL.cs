using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class PaisDAL : Conexao
    {
        protected string strSQL = "";

        public Pais PesquisarMunicipio(int CodigoPais)
        {
            try
            {
                AbrirConexao();
                strSQL = "SELECT * FROM [PAIS] WHERE CD_PAIS = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoPais);

                Dr = Cmd.ExecuteReader();

                Pais p = null;

                if (Dr.Read())
                {
                    p = new Pais();

                    p.CodigoPais = Convert.ToInt32(Dr["CD_PAIS"]);
                    p.DescricaoPais = Convert.ToString(Dr["DS_PAIS"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Pais: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Pais> ListarPaises(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT * FROM [PAIS] ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Pais> lista = new List<Pais>();

                while (Dr.Read())
                {
                    Pais p = new Pais();

                    p.CodigoPais = Convert.ToInt32(Dr["CD_PAIS"]);
                    p.DescricaoPais = Convert.ToString(Dr["DS_PAIS"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Paises: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
    }
}
