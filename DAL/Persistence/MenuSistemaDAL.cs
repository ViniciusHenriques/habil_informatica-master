using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data.SqlClient;
using System.Data;

namespace DAL.Persistence
{
    public class MenuSistemaDAL : Conexao
    {
        protected string strSQL = "";

        public void Inserir(MenuSistema p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [MENU_DO_SISTEMA] (CD_MENU_SISTEMA, CD_MODULO_SISTEMA, NM_MENU_SISTEMA, DS_MENU_SISTEMA, CD_ORDEM, CD_PAI_MENU_SISTEMA, URL, TP_FORMULARIO, TX_AJUDA,TX_CAMINHO_IMG ) values ((SELECT ISNULL(MAX(CD_MENU_SISTEMA),0) + 1 FROM [MENU_DO_SISTEMA]), @v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8,@v9)";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoModulo);
                Cmd.Parameters.AddWithValue("@v2", p.NomeMenu);
                Cmd.Parameters.AddWithValue("@v3", p.DescricaoMenu);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoOrdem);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoPaiMenu);
                Cmd.Parameters.AddWithValue("@v6", p.UrlPrograma);
                Cmd.Parameters.AddWithValue("@v7", p.TipoFormulario);
                Cmd.Parameters.AddWithValue("@v8", p.TextoAjuda);
                Cmd.Parameters.AddWithValue("@v9", p.UrlIcone);

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
                            throw new Exception("Erro ao gravar Menu: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Menu: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(MenuSistema p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [MENU_DO_SISTEMA] set [CD_MODULO_SISTEMA] = @v2";
                strSQL = strSQL + ", [NM_MENU_SISTEMA] = @v3 ";
                strSQL = strSQL + ", [DS_MENU_SISTEMA] = @v4 ";
                strSQL = strSQL + ", [CD_ORDEM] = @v5 ";
                strSQL = strSQL + ", [CD_PAI_MENU_SISTEMA] = @v6 ";
                strSQL = strSQL + ", [URL] = @v7 ";
                strSQL = strSQL + ", [TP_FORMULARIO] = @v8 ";
                strSQL = strSQL + ", [TX_AJUDA] = @v9 ";
                strSQL = strSQL + ", [TX_CAMINHO_IMG] = @v10 ";
                strSQL = strSQL + " Where [CD_MENU_SISTEMA] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoMenu);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoModulo);
                Cmd.Parameters.AddWithValue("@v3", p.NomeMenu);
                Cmd.Parameters.AddWithValue("@v4", p.DescricaoMenu);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoOrdem);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoPaiMenu);
                Cmd.Parameters.AddWithValue("@v7", p.UrlPrograma);
                Cmd.Parameters.AddWithValue("@v8", p.TipoFormulario);
                Cmd.Parameters.AddWithValue("@v9", p.TextoAjuda);
                Cmd.Parameters.AddWithValue("@v10", p.UrlIcone);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Menu: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [MENU_DO_SISTEMA] Where [CD_MENU_SISTEMA] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Menu: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Menu: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public MenuSistema PesquisarMenuSistema(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [MENU_DO_SISTEMA] Where CD_MENU_SISTEMA = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                MenuSistema p = null;

                if (Dr.Read())
                {
                    p = new MenuSistema();

                    p.CodigoMenu = Convert.ToInt32(Dr["CD_MENU_SISTEMA"]);
                    p.CodigoModulo = Convert.ToInt32(Dr["CD_MODULO_SISTEMA"]);
                    p.NomeMenu = Convert.ToString(Dr["NM_MENU_SISTEMA"]);
                    p.DescricaoMenu = Convert.ToString(Dr["DS_MENU_SISTEMA"]);
                    p.CodigoOrdem = Convert.ToInt32(Dr["CD_ORDEM"]);
                    p.CodigoPaiMenu = Convert.ToInt32(Dr["CD_PAI_MENU_SISTEMA"]);
                    p.UrlPrograma = Convert.ToString(Dr["URL"]);
                    p.TipoFormulario = Convert.ToString(Dr["TP_FORMULARIO"]);
                    p.TextoAjuda = Convert.ToString(Dr["TX_AJUDA"]);
                    p.UrlIcone = Convert.ToString(Dr["TX_CAMINHO_IMG"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Menu: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<MenuSistema> ListarMenuSistema(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [MENU_DO_SISTEMA]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<MenuSistema> lista = new List<MenuSistema>();

                while (Dr.Read())
                {
                    MenuSistema p = new MenuSistema();

                    p.CodigoMenu = Convert.ToInt32(Dr["CD_MENU_SISTEMA"]);
                    p.CodigoModulo = Convert.ToInt32(Dr["CD_MODULO_SISTEMA"]);
                    p.NomeMenu = Convert.ToString(Dr["NM_MENU_SISTEMA"]);
                    p.DescricaoMenu = Convert.ToString(Dr["DS_MENU_SISTEMA"]);
                    p.CodigoOrdem = Convert.ToInt32(Dr["CD_ORDEM"]);
                    p.CodigoPaiMenu = Convert.ToInt32(Dr["CD_PAI_MENU_SISTEMA"]);
                    p.UrlPrograma = Convert.ToString(Dr["URL"]);
                    p.TipoFormulario = Convert.ToString(Dr["TP_FORMULARIO"]);
                    p.TextoAjuda = Convert.ToString(Dr["TX_AJUDA"]);
                    p.UrlIcone = Convert.ToString(Dr["TX_CAMINHO_IMG"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Menu: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterMenuSistema(int CodigoModulo, string strNomeCampo, string strTipoCampo, string strValor, string strOrdem, int CodigoPerfil)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [MENU_DO_SISTEMA]";

                if (CodigoPerfil > 0)
                    strSQL = strSQL + " Inner Join [PERMISSAO_DO_USUARIO] on [MENU_DO_SISTEMA].[CD_MENU_SISTEMA] = [PERMISSAO_DO_USUARIO].[CD_MENU_SISTEMA] ";

                if (strValor != "")
                    strSQL = strSQL + " Where [MENU_DO_SISTEMA]." + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strSQL.IndexOf("Where") > 0)
                {
                    if (CodigoModulo > 0)
                        strSQL = strSQL + " and [MENU_DO_SISTEMA].[CD_MODULO_SISTEMA] = " + CodigoModulo.ToString();
                }
                else
                {
                    if (CodigoModulo > 0)
                        strSQL = strSQL + " Where [MENU_DO_SISTEMA].[CD_MODULO_SISTEMA] = " + CodigoModulo.ToString();
                }

                if (CodigoPerfil > 0)
                    strSQL = strSQL + " and [PERMISSAO_DO_USUARIO].[CD_PFL_USUARIO] = " + CodigoPerfil.ToString();

                if (strOrdem != "")
                    strSQL = strSQL + "Order By [MENU_DO_SISTEMA]." + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoMenu", typeof(Int32));
                dt.Columns.Add("CodigoModulo", typeof(Int32));
                dt.Columns.Add("NomeMenu", typeof(string));
                dt.Columns.Add("DescricaoMenu", typeof(string));
                dt.Columns.Add("CodigoOrdem", typeof(Int32));
                dt.Columns.Add("CodigoPaiMenu", typeof(Int32));
                dt.Columns.Add("UrlPrograma", typeof(string));
                dt.Columns.Add("TipoFormulario", typeof(string));
                dt.Columns.Add("TextoAjuda", typeof(string));
                dt.Columns.Add("UrlIcone", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_MENU_SISTEMA"]),
                                Convert.ToString(Dr["CD_MODULO_SISTEMA"]),
                                Convert.ToString(Dr["NM_MENU_SISTEMA"]),
                                Convert.ToString(Dr["DS_MENU_SISTEMA"]),
                                Convert.ToString(Dr["CD_ORDEM"]),
                                Convert.ToString(Dr["CD_PAI_MENU_SISTEMA"]),
                                Convert.ToString(Dr["URL"]),
                                Convert.ToString(Dr["TP_FORMULARIO"]),
                                Convert.ToString(Dr["TX_AJUDA"]),
                                Convert.ToString(Dr["TX_CAMINHO_IMG"])
                                );

                return dt;

                

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Menu: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterMenuInicial(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [MENU_DO_SISTEMA_INICIAL]";

                if (strValor != "")
                {
                    strSQL = strSQL + " Where [MENU_DO_SISTEMA_INICIAL]." + MontaFiltro(strNomeCampo, strTipoCampo, strValor);
                }

                if (strOrdem != "")
                {
                    strSQL = strSQL + "Order By [MENU_DO_SISTEMA_INICIAL]." + strOrdem;
                }

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoMenu", typeof(Int32));
                dt.Columns.Add("CodigoModulo", typeof(Int32));
                dt.Columns.Add("NomeMenu", typeof(string));
                dt.Columns.Add("DescricaoMenu", typeof(string));
                dt.Columns.Add("CodigoOrdem", typeof(Int32));
                dt.Columns.Add("CodigoPaiMenu", typeof(Int32));
                dt.Columns.Add("UrlPrograma", typeof(string));
                dt.Columns.Add("TipoFormulario", typeof(string));
                dt.Columns.Add("TextoAjuda", typeof(string));
                dt.Columns.Add("UrlIcone", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_MENU_SISTEMA"]),
                                Convert.ToString(Dr["CD_MODULO_SISTEMA"]),
                                Convert.ToString(Dr["NM_MENU_SISTEMA"]),
                                Convert.ToString(Dr["DS_MENU_SISTEMA"]),
                                Convert.ToString(Dr["CD_ORDEM"]),
                                Convert.ToString(Dr["CD_PAI_MENU_SISTEMA"]),
                                Convert.ToString(Dr["URL"]),
                                Convert.ToString(Dr["TP_FORMULARIO"]),
                                Convert.ToString(Dr["TX_AJUDA"]),
                                Convert.ToString(Dr["TX_CAMINHO_IMG"])
                                );
                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Menu: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }

    }
}