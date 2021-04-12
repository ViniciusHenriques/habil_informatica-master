using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class ModuloSistemaDAL:Conexao
    {
        protected string strSQL ="";
        public void Inserir(ModuloSistema p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [MODULO_DO_SISTEMA] (CD_MODULO_SISTEMA, DS_MODULO_SISTEMA) values (@v1, @v2)";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoModulo);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoModulo);

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
                            throw new Exception("Erro ao Incluir Módulo: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Módulo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(ModuloSistema p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [MODULO_DO_SISTEMA] set [DS_MODULO_SISTEMA] = @v2 Where [CD_MODULO_SISTEMA] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoModulo);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoModulo);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Módulo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Excluir(int Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [MODULO_DO_SISTEMA] Where [CD_MODULO_SISTEMA] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Modulo: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Módulo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public ModuloSistema PesquisarModuloSistema(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [MODULO_DO_SISTEMA] Where CD_MODULO_SISTEMA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                ModuloSistema p = null;

                if (Dr.Read())
                {
                    p = new ModuloSistema();

                    p.CodigoModulo = Convert.ToInt32(Dr["CD_MODULO_SISTEMA"]);
                    p.DescricaoModulo = Convert.ToString(Dr["DS_MODULO_SISTEMA"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Módulo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ModuloSistema> ListarModulosSistema(string strNomeCampo, string strTipoCampo, string strValor,string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [MODULO_DO_SISTEMA]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem; 

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ModuloSistema> lista = new List<ModuloSistema>();

                while (Dr.Read())
                {
                    ModuloSistema p = new ModuloSistema();

                    p.CodigoModulo = Convert.ToInt32(Dr["CD_MODULO_SISTEMA"]);
                    p.DescricaoModulo = Convert.ToString(Dr["DS_MODULO_SISTEMA"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Módulos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public List<ModuloSistema> ListarModulosPermitidos(string CodigoPerfil)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select distinct MOD.*" +
                                    "from[MODULO_DO_SISTEMA] AS MOD INNER JOIN "+
                                            "MENU_DO_SISTEMA AS MENU ON MENU.CD_MODULO_SISTEMA = MOD.CD_MODULO_SISTEMA INNER JOIN "+
                                            "PERMISSAO_DO_USUARIO AS PERMISSAO ON PERMISSAO.CD_MENU_SISTEMA = MENU.CD_MENU_SISTEMA INNER JOIN "+
                                            "USUARIO ON USUARIO.CD_PFL_USUARIO = PERMISSAO.CD_PFL_USUARIO "+
                                    "WHERE USUARIO.CD_PFL_USUARIO = "+ CodigoPerfil;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ModuloSistema> lista = new List<ModuloSistema>();

                while (Dr.Read())
                {
                    ModuloSistema p = new ModuloSistema();

                    p.CodigoModulo = Convert.ToInt32(Dr["CD_MODULO_SISTEMA"]);
                    p.DescricaoModulo = Convert.ToString(Dr["DS_MODULO_SISTEMA"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Módulos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public DataTable  ObterModulosSistema(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [MODULO_DO_SISTEMA]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem; 


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoModulo", typeof(Int32));
                dt.Columns.Add("DescricaoModulo", typeof(string));
                
                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_MODULO_SISTEMA"]), Convert.ToString(Dr["DS_MODULO_SISTEMA"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Módulos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
    }
}
