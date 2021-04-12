using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
namespace DAL.Persistence
{
    public class PerfilUsuarioDAL:Conexao
    {
        protected string strSQL = "";

        public void Inserir(PerfilUsuario p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [PERFIL_DO_USUARIO] (" +
                                            "CD_PFL_USUARIO," +
                                            "DS_PFL_USUARIO," +
                                            "HR_INICIAL_SEMANAL," +
                                            "HR_FINAL_SEMANAL," +
                                            "HR_INICIO_INTERVALO_1," +
                                            "HR_FIM_INTERVALO_1," +
                                            "HR_INICIO_INTERVALO_2," +
                                            "HR_FIM_INTERVALO_2," +
                                            "HR_INICIO_INTERVALO_3," +
                                            "HR_FIM_INTERVALO_3," +
                                            "DT_HR_INICIO_DIARIO," +
                                            "DT_HR_FIM_DIARIO," +
                                            "IN_DOMINGO," +
                                            "IN_SEGUNDA," +
                                            "IN_TERCA," +
                                            "IN_QUARTA," +
                                            "IN_QUINTA," +
                                            "IN_SEXTA," +
                                            "IN_SABADO," +
                                            "CD_MODULO_ESPECIFICO) " +
                                "values (@v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10, @v11, @v12, @v13, @v14, @v15, @v16, @v17, @v18, @v19,@v20)";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoPflUsuario);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoPflUsuario);
                Cmd.Parameters.AddWithValue("@v3", p.HoraInicial.ToString("HH:mm"));
                Cmd.Parameters.AddWithValue("@v4", p.HoraFinal.ToString("HH:mm"));
                if (p.IntervaloInicio1.HasValue)
                {
                    Cmd.Parameters.AddWithValue("@v5", p.IntervaloInicio1.Value.ToShortTimeString());
                    Cmd.Parameters.AddWithValue("@v6", p.IntervaloFim1.Value.ToShortTimeString());
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@v5", DBNull.Value);
                    Cmd.Parameters.AddWithValue("@v6", DBNull.Value);
                }

                if (p.IntervaloInicio2.HasValue)
                {
                    Cmd.Parameters.AddWithValue("@v7", p.IntervaloInicio2.Value.ToShortTimeString());
                    Cmd.Parameters.AddWithValue("@v8", p.IntervaloFim2.Value.ToShortTimeString());
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@v7", DBNull.Value);
                    Cmd.Parameters.AddWithValue("@v8", DBNull.Value);
                }

                if (p.IntervaloInicio3.HasValue)
                {
                    Cmd.Parameters.AddWithValue("@v9", p.IntervaloInicio3.Value.ToShortTimeString());
                    Cmd.Parameters.AddWithValue("@v10", p.IntervaloFim3.Value.ToShortTimeString());
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@v9", DBNull.Value);
                    Cmd.Parameters.AddWithValue("@v10", DBNull.Value);
                }
                if (p.DataHoraInicio.HasValue)
                {
                    Cmd.Parameters.AddWithValue("@v11", p.DataHoraInicio.Value.ToShortTimeString());
                    Cmd.Parameters.AddWithValue("@v12", p.DataHoraFim.Value.ToShortTimeString());
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@v11", DBNull.Value);
                    Cmd.Parameters.AddWithValue("@v12", DBNull.Value);
                }
                Cmd.Parameters.AddWithValue("@v13", p.Domingo);
                Cmd.Parameters.AddWithValue("@v14", p.Segunda);
                Cmd.Parameters.AddWithValue("@v15", p.Terca);
                Cmd.Parameters.AddWithValue("@v16", p.Quarta);
                Cmd.Parameters.AddWithValue("@v17", p.Quinta);
                Cmd.Parameters.AddWithValue("@v18", p.Sexta);
                Cmd.Parameters.AddWithValue("@v19", p.Sabado);
                Cmd.Parameters.AddWithValue("@v20", p.CodigoModuloEspecifico);

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
                            throw new Exception("Erro ao Incluir Perfil de Usuários: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Perfil de Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(PerfilUsuario p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [PERFIL_DO_USUARIO] set [DS_PFL_USUARIO] = @v2," +
                                            "HR_INICIAL_SEMANAL = @v3," +
                                            "HR_FINAL_SEMANAL = @v4," +
                                            "HR_INICIO_INTERVALO_1 = @v5," +
                                            "HR_FIM_INTERVALO_1 = @v6," +
                                            "HR_INICIO_INTERVALO_2 = @v7," +
                                            "HR_FIM_INTERVALO_2 = @v8," +
                                            "HR_INICIO_INTERVALO_3 = @v9," +
                                            "HR_FIM_INTERVALO_3 = @v10," +
                                            "DT_HR_INICIO_DIARIO = @v11," +
                                            "DT_HR_FIM_DIARIO = @v12," +
                                            "IN_DOMINGO = @v13," +
                                            "IN_SEGUNDA = @v14," +
                                            "IN_TERCA = @v15," +
                                            "IN_QUARTA = @v16," +
                                            "IN_QUINTA = @v17," +
                                            "IN_SEXTA = @v18," +
                                            "IN_SABADO = @v19," +
                                            "CD_MODULO_ESPECIFICO = @v20 " +
                            " Where [CD_PFL_USUARIO] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoPflUsuario);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoPflUsuario);
                Cmd.Parameters.AddWithValue("@v3", p.HoraInicial.ToString("HH:mm"));
                Cmd.Parameters.AddWithValue("@v4", p.HoraFinal.ToString("HH:mm"));

                if (p.IntervaloInicio1.HasValue)
                {
                    Cmd.Parameters.AddWithValue("@v5", p.IntervaloInicio1.Value.ToShortTimeString());
                    Cmd.Parameters.AddWithValue("@v6", p.IntervaloFim1.Value.ToShortTimeString());
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@v5", DBNull.Value);
                    Cmd.Parameters.AddWithValue("@v6", DBNull.Value);
                }

                if (p.IntervaloInicio2.HasValue)
                {
                    Cmd.Parameters.AddWithValue("@v7", p.IntervaloInicio2.Value.ToShortTimeString());
                    Cmd.Parameters.AddWithValue("@v8", p.IntervaloFim2.Value.ToShortTimeString());
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@v7", DBNull.Value);
                    Cmd.Parameters.AddWithValue("@v8", DBNull.Value);
                }

                if (p.IntervaloInicio3.HasValue)
                {
                    Cmd.Parameters.AddWithValue("@v9", p.IntervaloInicio3.Value.ToShortTimeString());
                    Cmd.Parameters.AddWithValue("@v10", p.IntervaloFim3.Value.ToShortTimeString());
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@v9", DBNull.Value);
                    Cmd.Parameters.AddWithValue("@v10", DBNull.Value);
                }
                if (p.DataHoraInicio.HasValue)
                {
                    Cmd.Parameters.AddWithValue("@v11", p.DataHoraInicio.ToString());
                    Cmd.Parameters.AddWithValue("@v12", p.DataHoraFim.ToString());
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@v11", DBNull.Value);
                    Cmd.Parameters.AddWithValue("@v12", DBNull.Value);
                }
                Cmd.Parameters.AddWithValue("@v13", p.Domingo);
                Cmd.Parameters.AddWithValue("@v14", p.Segunda);
                Cmd.Parameters.AddWithValue("@v15", p.Terca);
                Cmd.Parameters.AddWithValue("@v16", p.Quarta);
                Cmd.Parameters.AddWithValue("@v17", p.Quinta);
                Cmd.Parameters.AddWithValue("@v18", p.Sexta);
                Cmd.Parameters.AddWithValue("@v19", p.Sabado);
                Cmd.Parameters.AddWithValue("@v20", p.CodigoModuloEspecifico);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Perfil de Usuário: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from [PERFIL_DO_USUARIO] Where [CD_PFL_USUARIO] = @v1", Con);
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
                            throw new Exception("Erro ao excluir Perfil de Usuário: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Perfil de Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public PerfilUsuario PesquisarPerfilUsuario(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [PERFIL_DO_USUARIO] Where CD_PFL_USUARIO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                PerfilUsuario p = null;

                if (Dr.Read())
                {
                    p = new PerfilUsuario();

                    p.CodigoPflUsuario = Convert.ToInt32(Dr["CD_PFL_USUARIO"]);
                    p.DescricaoPflUsuario = Convert.ToString(Dr["DS_PFL_USUARIO"]);

                    p.HoraInicial = Convert.ToDateTime(Dr["HR_INICIAL_SEMANAL"].ToString());
                    p.HoraFinal = Convert.ToDateTime(Dr["HR_FINAL_SEMANAL"].ToString());

                    if (Dr["HR_INICIO_INTERVALO_1"] != DBNull.Value)
                        p.IntervaloInicio1 = Convert.ToDateTime(Dr["HR_INICIO_INTERVALO_1"].ToString());
                    if (Dr["HR_FIM_INTERVALO_1"] != DBNull.Value)
                        p.IntervaloFim1 = Convert.ToDateTime(Dr["HR_FIM_INTERVALO_1"].ToString());
                    if (Dr["HR_INICIO_INTERVALO_2"] != DBNull.Value)
                        p.IntervaloInicio2 = Convert.ToDateTime(Dr["HR_INICIO_INTERVALO_2"].ToString());
                    if (Dr["HR_FIM_INTERVALO_2"] != DBNull.Value)
                        p.IntervaloFim2 = Convert.ToDateTime(Dr["HR_FIM_INTERVALO_2"].ToString());
                    if (Dr["HR_INICIO_INTERVALO_3"] != DBNull.Value)
                        p.IntervaloInicio3 = Convert.ToDateTime(Dr["HR_INICIO_INTERVALO_3"].ToString());
                    if (Dr["HR_FIM_INTERVALO_3"] != DBNull.Value)
                        p.IntervaloFim3 = Convert.ToDateTime(Dr["HR_FIM_INTERVALO_3"].ToString());
                    if (Dr["DT_HR_INICIO_DIARIO"] != DBNull.Value)
                        p.DataHoraInicio = Convert.ToDateTime(Dr["DT_HR_INICIO_DIARIO"].ToString());
                    if (Dr["DT_HR_FIM_DIARIO"] != DBNull.Value)
                        p.DataHoraFim = Convert.ToDateTime(Dr["DT_HR_FIM_DIARIO"].ToString());

                    if (Dr["IN_DOMINGO"].ToString() == "1")
                        p.Domingo = true;
                    else
                        p.Domingo = false;

                    if (Dr["IN_SEGUNDA"].ToString() == "1")
                        p.Segunda = true;
                    else
                        p.Segunda = false;

                    if (Dr["IN_TERCA"].ToString() == "1")
                        p.Terca = true;
                    else
                        p.Terca = false;

                    if (Dr["IN_QUARTA"].ToString() == "1")
                        p.Quarta = true;
                    else
                        p.Quarta = false;

                    if (Dr["IN_QUINTA"].ToString() == "1")
                        p.Quinta = true;
                    else
                        p.Quinta = false;
                    if (Dr["IN_SEXTA"].ToString() == "1")
                        p.Sexta = true;
                    else
                        p.Sexta = false;

                    if (Dr["IN_SABADO"].ToString() == "1")
                        p.Sabado = true;
                    else
                        p.Sabado = false;

                    p.CodigoModuloEspecifico = Convert.ToInt32(Dr["CD_MODULO_ESPECIFICO"].ToString());
                }

                return p;

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
        public List<PerfilUsuario> ListarPerfisUsuario(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [PERFIL_DO_USUARIO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor); 


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<PerfilUsuario> lista = new List<PerfilUsuario>();

                while (Dr.Read())
                {
                    PerfilUsuario p = new PerfilUsuario();

                    p.CodigoPflUsuario = Convert.ToInt64(Dr["CD_PFL_USUARIO"]);
                    p.DescricaoPflUsuario = Convert.ToString(Dr["DS_PFL_USUARIO"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Perfis de Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterPerfisUsuario(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [PERFIL_DO_USUARIO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;


                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoPflUsuario", typeof(Int64));
                dt.Columns.Add("DescricaoPflUsuario", typeof(string));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt64(Dr["CD_PFL_USUARIO"]), Convert.ToString(Dr["DS_PFL_USUARIO"]));

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Perfis de Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
    }
}
