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
    public class PerfilUsuarioEmpresaDAL : Conexao
    {
        protected string strSQL = "";

        public List<PerfilUsuarioEmpresa> ListarEmpresaPerfilUsuario(Int64 CodigoEmpresa)
        {
            try
            {
                List<PerfilUsuarioEmpresa> listaPermissao = new List<PerfilUsuarioEmpresa>();
                List<PerfilUsuario> listaPerfis = new List<PerfilUsuario>();
                PerfilUsuarioDAL p1 = new PerfilUsuarioDAL();
                listaPerfis = p1.ListarPerfisUsuario("", "", "", "");

                AbrirConexao();
                foreach (PerfilUsuario p in listaPerfis)
                {
                    PerfilUsuarioEmpresa pue = new PerfilUsuarioEmpresa();

                    pue.CodigoEmpresa = CodigoEmpresa;
                    pue.CodigoPflUsuario = p.CodigoPflUsuario;
                    pue.DescricaoPflUsuario = p.DescricaoPflUsuario;
                    pue.Liberado = false;

                    strSQL = "Select Top 1 1 ";
                    strSQL = strSQL + " From PERFIL_DO_USUARIO_NA_EMPRESA ";
                    strSQL = strSQL + " Where CD_EMPRESA = " + Convert.ToString(CodigoEmpresa);
                    strSQL = strSQL + " and   CD_PFL_USUARIO =  " + Convert.ToString(p.CodigoPflUsuario);

                    Cmd = new SqlCommand(strSQL, Con);
                    Dr = Cmd.ExecuteReader();
                    if (Dr.Read())
                    {
                        pue.Liberado = true;
                    }
                    Dr.Close();

                    listaPermissao.Add(pue);
                }

                return listaPermissao;
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
        public void ExcluirPermissao(Int64 CodigoEmpresa, Int64 CodigoPerfil)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [PERFIL_DO_USUARIO_NA_EMPRESA] Where [CD_EMPRESA] = @v1 AND [CD_PFL_USUARIO] = @v2", Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", CodigoPerfil);

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
        public Boolean PesquisarPermissao(Int64 CodigoEmpresa, Int64 CodigoPerfil)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [PERFIL_DO_USUARIO_NA_EMPRESA] Where [CD_EMPRESA] = @v1 AND [CD_PFL_USUARIO] = @v2";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", CodigoPerfil);


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
        public void SalvarPermissao(Int64 CodigoEmpresa, Int64 CodigoPerfil)
        {
            try
            {

                if (!PesquisarPermissao(CodigoEmpresa, CodigoPerfil))
                {
                    strSQL = "insert into [PERFIL_DO_USUARIO_NA_EMPRESA] ([CD_EMPRESA], [CD_PFL_USUARIO]) VALUES (@v1, @v2)";
                }

                AbrirConexao();

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", CodigoPerfil);
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


        public List<PerfilUsuarioEmpresa> ListarPerfilUsuarioEmpresa(Int64 CodigoPerfil)
        {
            try
            {
                List<PerfilUsuarioEmpresa> listaPermissao = new List<PerfilUsuarioEmpresa>();
                List<Empresa> listaPerfis = new List<Empresa>();
                EmpresaDAL p1 = new EmpresaDAL();
                listaPerfis = p1.ListarEmpresas("", "", "", "");

                AbrirConexao();
                foreach (Empresa p in listaPerfis)
                {
                    PerfilUsuarioEmpresa pue = new PerfilUsuarioEmpresa();

                    pue.CodigoPflUsuario = CodigoPerfil;
                    pue.CodigoEmpresa = p.CodigoEmpresa;
                    pue.NomeEmpresa = p.NomeEmpresa;
                    pue.Liberado = false;

                    strSQL = "Select Top 1 1 ";
                    strSQL = strSQL + " From PERFIL_DO_USUARIO_NA_EMPRESA ";
                    strSQL = strSQL + " Where CD_EMPRESA = " + Convert.ToString(p.CodigoEmpresa);
                    strSQL = strSQL + " and   CD_PFL_USUARIO =  " + Convert.ToString(CodigoPerfil);

                    Cmd = new SqlCommand(strSQL, Con);
                    Dr = Cmd.ExecuteReader();
                    if (Dr.Read())
                    {
                        pue.Liberado = true;
                    }
                    Dr.Close();

                    listaPermissao.Add(pue);
                }

                return listaPermissao;
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

    }
}
