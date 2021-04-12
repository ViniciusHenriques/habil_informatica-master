using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class MunicipioDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(Municipio p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [Municipio] (CD_Municipio, CD_Estado, DS_Municipio) values (@v1, @v2, @v3)";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEstado);
                Cmd.Parameters.AddWithValue("@v3", p.DescricaoMunicipio);

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
                            throw new Exception("Erro ao incluir Município: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Município: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(Municipio p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [Municipio] set [CD_Estado] = @v2, [DS_Municipio] = @v3 Where [CD_Municipio] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoEstado);
                Cmd.Parameters.AddWithValue("@v3", p.DescricaoMunicipio);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Municipio: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [Municipio] Where [CD_Municipio] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Município: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Municipio: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public Municipio PesquisarMunicipio(long Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select M.*, E.SIGLA from [Municipio] as M Inner Join [Estado] as E On M.CD_ESTADO = E.CD_ESTADO Where M.CD_Municipio = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                Municipio p = null;

                if (Dr.Read())
                {
                    p = new Municipio();

                    p.CodigoMunicipio = Convert.ToInt64(Dr["CD_Municipio"]);
                    p.CodigoEstado = Convert.ToInt32(Dr["CD_Estado"]);
                    p.Sigla = Convert.ToString(Dr["Sigla"]);
                    p.DescricaoMunicipio = Convert.ToString(Dr["DS_Municipio"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Municipio: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Municipio> ListarMunicipios(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select M.*, E.SIGLA from [Municipio] as M Inner Join [Estado] as E On M.CD_ESTADO = E.CD_ESTADO ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Municipio> lista = new List<Municipio>();

                while (Dr.Read())
                {
                    Municipio p = new Municipio();

                    p.CodigoMunicipio = Convert.ToInt64(Dr["CD_Municipio"]);
                    p.CodigoEstado = Convert.ToInt32(Dr["CD_Estado"]);
                    p.DescricaoMunicipio = Convert.ToString(Dr["DS_Municipio"]);
                    p.Sigla = Convert.ToString(Dr["Sigla"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Municipios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterMunicipios(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select M.*, E.SIGLA from [Municipio] as M Inner Join [Estado] as E On M.CD_ESTADO = E.CD_ESTADO";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoMunicipio", typeof(Int64));
                dt.Columns.Add("CodigoEstado", typeof(Int32));
                dt.Columns.Add("Sigla", typeof(string));
                dt.Columns.Add("DescricaoMunicipio", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt64(Dr["CD_Municipio"]), Convert.ToInt32(Dr["CD_Estado"]), Convert.ToString(Dr["Sigla"]), Convert.ToString(Dr["DS_Municipio"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Municipios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public Municipio ObterMunicipio(string strDescricaoMunicipio, string siglaEstado)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select M.*, E.SIGLA from [Municipio] as M Inner Join [Estado] as E On M.CD_ESTADO = E.CD_ESTADO " +
                    "where ' '+M.DS_MUNICIPIO = @v1 AND  ' '+e.SIGLA = @v2";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", " " + strDescricaoMunicipio);
                Cmd.Parameters.AddWithValue("@v2", " " + siglaEstado);
                Dr = Cmd.ExecuteReader();

                Municipio p = new Municipio();

                while (Dr.Read())
                {
                    

                    p.CodigoMunicipio = Convert.ToInt64(Dr["CD_Municipio"]);
                    p.CodigoEstado = Convert.ToInt32(Dr["CD_Estado"]);
                    p.DescricaoMunicipio = Convert.ToString(Dr["DS_Municipio"]);
                    p.Sigla = Convert.ToString(Dr["Sigla"]);
                    
                }

                return p;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Municipios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }

        }
    }
}
