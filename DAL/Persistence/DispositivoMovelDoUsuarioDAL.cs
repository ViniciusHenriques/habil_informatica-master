using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class DispositivoMovelDoUsuarioDAL : Conexao
    {
        protected string strSQL = "";

        public List<DispositivoMovelDoUsuario> GetAll()
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select * from DISPOSITIVO_MOVEL_DO_USUARIO", Con);
                Dr = Cmd.ExecuteReader();

                List<DispositivoMovelDoUsuario> lista = new List<DispositivoMovelDoUsuario>();

                while (Dr.Read())
                {
                    DispositivoMovelDoUsuario p = new DispositivoMovelDoUsuario();

                    p.CD_INDEX = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CD_SITUACAO = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.CD_USUARIO = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.TX_KEY = Convert.ToString(Dr["TX_KEY"]);
                    p.NR_FONE = Convert.ToString(Dr["NR_FONE"]);
                    p.ID_DISPOSITIVO = Convert.ToString(Dr["ID_DISPOSITIVO"]);
                    p.NM_DISPOSITIVO = Convert.ToString(Dr["NM_DISPOSITIVO"]);
                    p.NM_MODELO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_FABRICANTE = Convert.ToString(Dr["NM_FABRICANTE"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar os Dispositivos Móveis dos Usuários: " + ex.Message);
            }
            finally
            {
                FecharConexao();
            }
        }

        public void Inserir(DispositivoMovelDoUsuario p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [DISPOSITIVO_MOVEL_DO_USUARIO] (" +
                    "CD_USUARIO," +
                    "CD_SITUACAO," +
                    "TX_KEY," +
                    "NR_FONE," +
                    "ID_DISPOSITIVO," +
                    "NM_DISPOSITIVO," +
                    "NM_MODELO," +
                    "NM_FABRICANTE) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CD_USUARIO);
                Cmd.Parameters.AddWithValue("@v2", p.CD_SITUACAO);
                Cmd.Parameters.AddWithValue("@v3", p.TX_KEY);
                Cmd.Parameters.AddWithValue("@v4", p.NR_FONE);
                Cmd.Parameters.AddWithValue("@v5", p.ID_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@v6", p.NM_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@v7", p.NM_MODELO);
                Cmd.Parameters.AddWithValue("@v8", p.NM_FABRICANTE);

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
                            throw new Exception("Erro ao Incluir Dispositivo: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Dispositivo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(DispositivoMovelDoUsuario p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DISPOSITIVO_MOVEL_DO_USUARIO] set " +
                            "[CD_USUARIO] = @v2," +
                            "[CD_SITUACAO] = @v3," +
                            "[TX_KEY] = @v4," +
                            "[NR_FONE] = @v5," +
                            "[ID_DISPOSITIVO] = @v6," +
                            "[NM_DISPOSITIVO] = @v7," +
                            "[NM_MODELO] = @v8," +
                            "[NM_FABRICANTE] = @v9 " +
                          "Where [CD_INDEX] = @v1";


                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CD_INDEX);
                Cmd.Parameters.AddWithValue("@v2", p.CD_USUARIO);
                Cmd.Parameters.AddWithValue("@v3", p.CD_SITUACAO);
                Cmd.Parameters.AddWithValue("@v4", p.TX_KEY);
                Cmd.Parameters.AddWithValue("@v5", p.NR_FONE);
                Cmd.Parameters.AddWithValue("@v6", p.ID_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@v7", p.NM_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@v8", p.NM_MODELO);
                Cmd.Parameters.AddWithValue("@v9", p.NM_FABRICANTE);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Dispositivo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }

        }



        public void AtualizarTX_KEY(string tx_key, DispositivoMovelDoUsuario p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [DISPOSITIVO_MOVEL_DO_USUARIO] set " +
                            "[NR_FONE] = @v2, " +
                            "[ID_DISPOSITIVO] = @v3, " +
                            "[NM_DISPOSITIVO] = @v4, " +
                            "[NM_MODELO] = @v5," +
                            "[NM_FABRICANTE] = @v6 " +
                          "Where [TX_KEY] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", tx_key);
                Cmd.Parameters.AddWithValue("@v2", p.NR_FONE);
                Cmd.Parameters.AddWithValue("@v3", p.ID_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@v4", p.NM_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@v5", p.NM_MODELO);
                Cmd.Parameters.AddWithValue("@v6", p.NM_FABRICANTE);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Dispositivo: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [DISPOSITIVO_MOVEL_DO_USUARIO] Where [CD_INDEX] = @v1", Con);

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
                            throw new Exception("Erro ao excluir Dispositivo: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Dispositovo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DispositivoMovelDoUsuario PesquisarDispositivoMovelDoUsuario(int intCodigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [DISPOSITIVO_MOVEL_DO_USUARIO] Where CD_INDEX = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);

                Dr = Cmd.ExecuteReader();

                DispositivoMovelDoUsuario p = null;

                if (Dr.Read())
                {
                    p = new DispositivoMovelDoUsuario();

                    p.CD_INDEX = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CD_USUARIO = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CD_USUARIO = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.TX_KEY = Convert.ToString(Dr["TX_KEY"]);
                    p.NR_FONE = Convert.ToString(Dr["NR_FONE"]);
                    p.ID_DISPOSITIVO = Convert.ToString(Dr["ID_DISPOSITIVO"]);
                    p.NM_DISPOSITIVO = Convert.ToString(Dr["NM_DISPOSITIVO"]);
                    p.NM_MODELO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_FABRICANTE = Convert.ToString(Dr["NM_FABRICANTE"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Dispositivo Móvel: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public DispositivoMovelDoUsuario PesquisarDispositivoMovelPorTX_KEY(string strTX_KEY)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [DISPOSITIVO_MOVEL_DO_USUARIO] Where TX_KEY = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strTX_KEY);

                Dr = Cmd.ExecuteReader();

                DispositivoMovelDoUsuario p = new DispositivoMovelDoUsuario();

                if (Dr.Read())
                {
                    p.CD_INDEX = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CD_USUARIO = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CD_USUARIO = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.TX_KEY = Convert.ToString(Dr["TX_KEY"]);
                    p.NR_FONE = Convert.ToString(Dr["NR_FONE"]);
                    p.ID_DISPOSITIVO = Convert.ToString(Dr["ID_DISPOSITIVO"]);
                    p.NM_DISPOSITIVO = Convert.ToString(Dr["NM_DISPOSITIVO"]);
                    p.NM_MODELO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_FABRICANTE = Convert.ToString(Dr["NM_FABRICANTE"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Dispositivo Móvel do Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<DispositivoMovelDoUsuario> ListarCargos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [DISPOSITIVO_MOVEL_DO_USUARIO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<DispositivoMovelDoUsuario> lista = new List<DispositivoMovelDoUsuario>();

                while (Dr.Read())
                {
                    DispositivoMovelDoUsuario p = new DispositivoMovelDoUsuario();

                    p.CD_INDEX = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CD_USUARIO = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CD_USUARIO = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.TX_KEY = Convert.ToString(Dr["TX_KEY"]);
                    p.NR_FONE = Convert.ToString(Dr["NR_FONE"]);
                    p.ID_DISPOSITIVO = Convert.ToString(Dr["ID_DISPOSITIVO"]);
                    p.NM_DISPOSITIVO = Convert.ToString(Dr["NM_DISPOSITIVO"]);
                    p.NM_MODELO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_FABRICANTE = Convert.ToString(Dr["NM_FABRICANTE"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Cargo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }

        //public DataTable ObterCargos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        //{
        //    try
        //    {
        //        AbrirConexao();

        //        string strSQL = "Select * from [CARGO]";

        //        if (strValor != "")
        //            strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

        //        if (strOrdem != "")
        //            strSQL = strSQL + "Order By " + strOrdem;


        //        Cmd = new SqlCommand(strSQL, Con);

        //        Dr = Cmd.ExecuteReader();

        //        // Cria DataTable
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("CodigoCargo", typeof(Int32));
        //        dt.Columns.Add("DescricaoCargo", typeof(string));

        //        while (Dr.Read())
        //            dt.Rows.Add(Convert.ToInt32(Dr["CD_CARGO"]), Convert.ToString(Dr["DS_CARGO"]));

        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("Erro ao Listar Todas Cargos: " + ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        FecharConexao();

        //    }
        //}

        //public List<Cargo> ListarCargosCompleto(List<DBTabelaCampos> ListaFiltros)
        //{
        //    try
        //    {
        //        AbrirConexao();
        //        string strValor = "";
        //        string strSQL = "Select * from [CARGO]";

        //        strValor = MontaFiltroIntervalo(ListaFiltros);

        //        strSQL = strSQL + strValor;
        //        Cmd = new SqlCommand(strSQL, Con);

        //        Dr = Cmd.ExecuteReader();

        //        List<Cargo> lista = new List<Cargo>();

        //        while (Dr.Read())
        //        {
        //            Cargo p = new Cargo();

        //            p.CodigoCargo = Convert.ToInt32(Dr["CD_CARGO"]);
        //            p.DescricaoCargo = Convert.ToString(Dr["DS_CARGO"]);
        //            lista.Add(p);
        //        }
        //        return lista;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao Listar Todas Cargo: " + ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        FecharConexao();
        //    }
        //}

    }
}
