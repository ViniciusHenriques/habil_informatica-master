using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class PermissaoDAL : Conexao
    {
        protected string strSQL = "";

        public List<Permissao> ListarPerfilUsuario(int CodigoModulo, int CodigoPerfil, string strPrograma)
        {
            try
            {
                AbrirConexao();

                strSQL = " WITH cteMenuNivel(CD_ORDEM , ID, NM_MENU_SISTEMA, Nivel, NomeCompleto , CD_ORD_PAI)  AS " +
                         " (SELECT CD_ORDEM, CD_MENU_SISTEMA, NM_MENU_SISTEMA, 1 AS 'Nivel', UPPER(CAST(NM_MENU_SISTEMA AS VARCHAR(255))) AS 'NomeCompleto', CD_ORDEM AS CD_ORD_PAI " +
                         " FROM MENU_DO_SISTEMA WHERE CD_PAI_MENU_SISTEMA = 0 UNION ALL " +
                         " SELECT m.CD_ORDEM, m.CD_MENU_SISTEMA, m.NM_MENU_SISTEMA, c.Nivel + 1 AS 'Nivel', UPPER(CAST((c.NomeCompleto + ' => ' + m.NM_MENU_SISTEMA) AS VARCHAR(255))) 'NomeCompleto', c.CD_ORD_PAI " +
                         " FROM MENU_DO_SISTEMA m INNER JOIN cteMenuNivel c ON m.CD_PAI_MENU_SISTEMA = c.ID) " +
                         " SELECT c.CD_ORDEM, c.ID, c.NOMECOMPLETO, m.TP_Formulario, m.TX_AJUDA  , 1 AS LIBERADO, 1 AS ACESSO_COMPLETO " +
                         " FROM cteMenuNivel as c inner join MENU_DO_SISTEMA as m on c.id = m.CD_MENU_SISTEMA " +
                         "";

                if (CodigoPerfil <= 0)
                    strSQL = strSQL + " Where m.CD_MENU_SISTEMA = -1 ";
                else
                {
                    PerfilUsuario p1 = new PerfilUsuario();
                    PerfilUsuarioDAL pd1 = new PerfilUsuarioDAL();

                    p1 = pd1.PesquisarPerfilUsuario(CodigoPerfil);
                    
                    if (p1 == null)
                        strSQL = strSQL + " Where m.CD_MENU_SISTEMA = -1";
                    else
                        strSQL = strSQL + " Where m.CD_MODULO_SISTEMA = " + Convert.ToString(CodigoModulo);

                    if (strPrograma.Trim() != "")
                        strSQL = strSQL + " and m.[URL] LIKE '%"+ strPrograma +"%'";
                }

                strSQL = strSQL + " order by C.CD_ORD_PAI, c.NomeCompleto ";


                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                Permissao p = null;
                List<Permissao> lista = new List<Permissao>();

                while (Dr.Read())
                {
                    p = new Permissao();
                    p.CodigoItem = Convert.ToInt32(Dr["ID"]);
                    p.DescricaoDoMenu = Convert.ToString(Dr["NOMECOMPLETO"]);
                    p.TipoFormulario = Convert.ToString(Dr["TP_FORMULARIO"]);

                    p.Liberado = false;
                    p.AcessoCompleto = false;
                    p.AcessoConsulta = false;
                    p.AcessoRelatorio = false;
                    p.AcessoImprimir = false;
                    p.AcessoIncluir = false;
                    p.AcessoAlterar = false;
                    p.AcessoExcluir = false;
                    p.AcessoDownload = false;
                    p.AcessoUpload = false;
                    p.AcessoExclusaoAnexo = false;
                    p.AcessoEspecial1 = false;
                    p.AcessoEspecial2 = false;
                    p.AcessoEspecial3 = false;

                    p.TextoAjuda = Convert.ToString(Dr["TX_AJUDA"]);
                    lista.Add(p);
                }
                Dr.Close();

                //Ler as Permissões do Usuário
                foreach (var permusu in lista)
                {
                    strSQL = "";
                    strSQL = strSQL + " Select CD_MENU_SISTEMA, CD_PFL_USUARIO, isnull(IN_BOTOES,'00000000000000') as TX_INDICADORES ";
                    strSQL = strSQL + " From PERMISSAO_DO_USUARIO ";
                    strSQL = strSQL + " Where CD_MENU_SISTEMA = " + Convert.ToString(permusu.CodigoItem);
                    strSQL = strSQL + " and   CD_PFL_USUARIO =  " + Convert.ToString(CodigoPerfil);

                    Cmd = new SqlCommand(strSQL, Con);
                    Dr = Cmd.ExecuteReader();

                    while (Dr.Read())
                    {

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(0, 1) == "1")
                            permusu.Liberado = true;
                        else
                            permusu.Liberado = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(1, 1) == "1")
                            permusu.AcessoCompleto = true;
                        else
                            permusu.AcessoCompleto = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(2, 1) == "1")
                            permusu.AcessoConsulta = true;
                        else
                            permusu.AcessoConsulta = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(3, 1) == "1")
                            permusu.AcessoRelatorio = true;
                        else
                            permusu.AcessoRelatorio = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(4, 1) == "1")
                            permusu.AcessoImprimir = true;
                        else
                            permusu.AcessoImprimir = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(5, 1) == "1")
                            permusu.AcessoIncluir = true;
                        else
                            permusu.AcessoIncluir = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(6, 1) == "1")
                            permusu.AcessoAlterar = true;
                        else
                            permusu.AcessoAlterar = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(7, 1) == "1")
                            permusu.AcessoExcluir = true;
                        else
                            permusu.AcessoExcluir = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(8, 1) == "1")
                            permusu.AcessoDownload = true;
                        else
                            permusu.AcessoDownload = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(9, 1) == "1")
                            permusu.AcessoUpload = true;
                        else
                            permusu.AcessoUpload = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(10, 1) == "1")
                            permusu.AcessoExclusaoAnexo = true;
                        else
                            permusu.AcessoExclusaoAnexo = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(11, 1) == "1")
                            permusu.AcessoEspecial1 = true;
                        else
                            permusu.AcessoEspecial1 = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(12, 1) == "1")
                            permusu.AcessoEspecial2 = true;
                        else
                            permusu.AcessoEspecial2 = false;

                        if (Convert.ToString(Dr["TX_INDICADORES"]).Substring(13, 1) == "1")
                            permusu.AcessoEspecial3 = true;
                        else
                            permusu.AcessoEspecial3 = false;

                       

                    }
                    Dr.Close();

                }


                return lista;
            }

            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Perfil de Usuário: " + ex.Message.ToString());
            }

            finally
            {
                FecharConexao();
            }
        }

        public void ExcluirPermissao(int CodigoDoMenu, int CodigoDoPerfil)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [PERMISSAO_DO_USUARIO] Where [CD_MENU_SISTEMA] = @v1 AND [CD_PFL_USUARIO] = @v2", Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDoMenu);
                Cmd.Parameters.AddWithValue("@v2", CodigoDoPerfil);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Permissão: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public Boolean PesquisarPermissao(int CodigoDoMenu, int CodigoDoPerfil)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [PERMISSAO_DO_USUARIO] Where [CD_MENU_SISTEMA] = @v1 AND [CD_PFL_USUARIO] = @v2";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDoMenu);
                Cmd.Parameters.AddWithValue("@v2", CodigoDoPerfil);


                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Permissão: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void SalvarPermissao(int CodigoDoMenu, int CodigoDoPerfil, string strIndicadores)
        {
            try
            {
                if (PesquisarPermissao(CodigoDoMenu, CodigoDoPerfil))
                {
                    strSQL = "update [PERMISSAO_DO_USUARIO] set IN_BOTOES = @v3  Where [CD_MENU_SISTEMA] = @v1 AND [CD_PFL_USUARIO] = @v2";
                }
                else
                {
                    strSQL = "insert into [PERMISSAO_DO_USUARIO] ([CD_MENU_SISTEMA], [CD_PFL_USUARIO], [IN_BOTOES]) VALUES (@v1, @v2, @v3)";
                }

                AbrirConexao();

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoDoMenu);
                Cmd.Parameters.AddWithValue("@v2", CodigoDoPerfil);
                Cmd.Parameters.AddWithValue("@v3", strIndicadores);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Situação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }

        }


    }
}
