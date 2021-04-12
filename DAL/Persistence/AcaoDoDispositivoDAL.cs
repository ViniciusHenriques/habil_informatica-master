using System;
using System.Collections.Generic;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class AcaoDoDispositivoDAL : Conexao
    {
        protected string strSQL = "";

        public List<AcaoDoDispositivo> GetAll()
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("select * from [ACAO_DO_DISPOSITIVO]", Con);

                Dr = Cmd.ExecuteReader();

                List<AcaoDoDispositivo> lista = new List<AcaoDoDispositivo>();

                AcaoDoDispositivo p = null;

                while (Dr.Read())
                {
                    p = new AcaoDoDispositivo();

                    p.CD_KEY = Convert.ToString(Dr["CD_KEY"]);
                    p.NR_FONE = Convert.ToDecimal(Dr["NR_FONE"]);
                    p.ID_DISPOSITIVO = Convert.ToString(Dr["ID_DISPOSITIVO"]);
                    p.NM_DISPOSITIVO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_MODELO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_FABRICANTE = Convert.ToString(Dr["NM_FABRICANTE"]);
                    p.DT_LANCAMENTO = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar Ações dos dispositivos: " + ex.Message);
            }
            finally
            {
                FecharConexao();
            }
        }


        public void Inserir(AcaoDoDispositivo p)
        {
            strSQL = "insert into [ACAO_DO_DISPOSITIVO] " +
                            "(CD_KEY," +
                            "NR_FONE," +
                            "ID_DISPOSITIVO," +
                            "NM_DISPOSITIVO,NM_MODELO," +
                            "NM_FABRICANTE) " +
                      "values " +
                            "(@cd_key," +
                            "@nr_fone,@id_dispositivo,@nm_dispositivo,@nm_modelo,@nm_fabricante)";
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@cd_key", p.CD_KEY.ToUpper());
                Cmd.Parameters.AddWithValue("@nr_fone", p.NR_FONE);
                Cmd.Parameters.AddWithValue("@id_dispositivo", p.ID_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@nm_dispositivo", p.NM_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@nm_modelo", p.NM_MODELO);
                Cmd.Parameters.AddWithValue("@nm_fabricante", p.NM_FABRICANTE);

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
                            throw new Exception("Erro ao Incluir Ação do Dispositivo: " + ex.Message.ToString());
                    }
                }
            }

            finally
            {
                FecharConexao();
            }

        }
        public void Atualizar(AcaoDoDispositivo p)
        {
            try
            {
                AbrirConexao();

                strSQL = "update ACAO_DO_DISPOSITIVO set " +
                            "NR_FONE        = @nr_fone, " +
                            "ID_DISPOSITIVO = @id_dispositivo, " +
                            "NM_DISPOSITIVO = @nm_dispositivo, " +
                            "NM_MODELO      = @nm_modelo, " +
                            "NM_FABRICANTE  = @nm_fabricante, " +
                            "DT_LANCAMENTO  = @dt_lancamento " +
                         "Where [CD_KEY] = @cd_key;";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@cd_key", p.CD_KEY);
                Cmd.Parameters.AddWithValue("@nr_fone", Convert.ToInt64(p.NR_FONE));
                Cmd.Parameters.AddWithValue("@id_dispositivo", p.ID_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@nm_dispositivo", p.NM_DISPOSITIVO);
                Cmd.Parameters.AddWithValue("@nm_modelo", p.NM_MODELO);
                Cmd.Parameters.AddWithValue("@nm_fabricante", p.NM_FABRICANTE);
                Cmd.Parameters.AddWithValue("@dt_lancamento", p.DT_LANCAMENTO);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Ação do Dispositivo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(string Codigo)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("delete from [ACAO_DO_DISPOSITIVO] Where [CD_KEY] = @v1", Con);
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
                            throw new Exception("Erro ao excluir Ação do Dispositivo: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Ação do Dispositivo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public AcaoDoDispositivo PesquisarAcaoDoDispositivo(string strCodigo)
        {

            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("Select * from [ACAO_DO_DISPOSITIVO] Where CD_KEY = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", strCodigo);

                Dr = Cmd.ExecuteReader();

                AcaoDoDispositivo p = null;

                if (Dr.Read())
                {
                    p = new AcaoDoDispositivo();

                    p.CD_KEY = Convert.ToString(Dr["CD_KEY"]);
                    p.NR_FONE = Convert.ToDecimal(Dr["NR_FONE"]);
                    p.ID_DISPOSITIVO = Convert.ToString(Dr["ID_DISPOSITIVO"]);
                    p.NM_DISPOSITIVO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_MODELO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_FABRICANTE = Convert.ToString(Dr["NM_FABRICANTE"]);
                    p.DT_LANCAMENTO = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Ação do Dispositivo: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<AcaoDoDispositivo> ListarAcaoDoDispositivo(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [ACAO_DO_DISPOSITIVO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<AcaoDoDispositivo> lista = new List<AcaoDoDispositivo>();

                while (Dr.Read())
                {
                    AcaoDoDispositivo p = new AcaoDoDispositivo();

                    p.CD_KEY = Convert.ToString(Dr["CD_KEY"]);
                    p.NR_FONE = Convert.ToInt32(Dr["NR_FONE"]);
                    p.ID_DISPOSITIVO = Convert.ToString(Dr["ID_DISPOSITIVO"]);
                    p.NM_DISPOSITIVO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_MODELO = Convert.ToString(Dr["NM_MODELO"]);
                    p.NM_FABRICANTE = Convert.ToString(Dr["NM_FABRICANTE"]);
                    p.DT_LANCAMENTO = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas as Ações dos Dispositivos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }

    }
}
