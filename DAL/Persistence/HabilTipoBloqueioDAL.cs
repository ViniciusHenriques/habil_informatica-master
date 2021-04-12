using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public     class HabilTipoBloqueioDAL: Conexao
    {
        protected string strSQL = "";


        public void InserirTipoBloqueio(HabilTipoBloqueio p)
        {
            try
            {
                AbrirConexao();
                strSQL = "insert into [HABIL_TIPO_BLOQUEIO] (CD_TIPO_DOCUMENTO,CD_BLOQUEIO,DS_BLOQUEIO)" +
                         " values (@v1, @v2, @v3);";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoBloqueio);
                Cmd.Parameters.AddWithValue("@v3", p.DescricaoBloqueio);
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
                            throw new Exception("Erro ao Incluir Tipo de Bloqueio: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Tipo de Bloqueio: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
       
        public HabilTipoBloqueio PesquisarHabilTipoBloqueio(int i)
        {
            try
            {
                AbrirConexao();
                strSQL = "select distinct  * from HABIL_TIPO_BLOQUEIO Where CD_BLOQUEIO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", i);

                Dr = Cmd.ExecuteReader();

                HabilTipoBloqueio p = null;

                if (Dr.Read())
                {
                    p = new HabilTipoBloqueio();

                    p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                    p.DescricaoBloqueio = Convert.ToString(Dr["DS_BLOQUEIO"]);
                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tipo de Bloqueio: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<HabilTipoBloqueio> ListarTiposDeBloqueios(int i)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT lib.CD_BLOQUEIO, count(*) as [N], DS_BLOQUEIO " +
                                "FROM DOCUMENTO ped " +
                                "inner join LIBERACAO_DO_DOCUMENTO lib on PED.CD_DOCUMENTO = lib.cd_documento " +
                                "left join HABIL_TIPO_BLOQUEIO tb on ped.CD_TIPO_DOCUMENTO = tb.CD_TIPO_DOCUMENTO and lib.CD_BLOQUEIO = TB.CD_BLOQUEIO " +
                                "where ped.CD_EMPRESA = @v1 and lib.DT_LIBERACAO IS NULL and ped.CD_TIPO_DOCUMENTO = 8 " +
                                "GROUP BY lib.CD_BLOQUEIO,DS_BLOQUEIO;";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1",i);
                Dr = Cmd.ExecuteReader();

                List<HabilTipoBloqueio> lista = new List<HabilTipoBloqueio>();

                while (Dr.Read())
                {
                    HabilTipoBloqueio p = new HabilTipoBloqueio();

                    p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                    p.DescricaoBloqueio = Convert.ToString(Dr["DS_BLOQUEIO"]);
                    p.QuantidadeBloqueios = Convert.ToInt32(Dr["N"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos os Bloqueios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public List<HabilTipoBloqueio> ListarHabilTipoBloqueio(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [HABIL_TIPO_BLOQUEIO]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<HabilTipoBloqueio> lista = new List<HabilTipoBloqueio>();

                while (Dr.Read())
                {
                    HabilTipoBloqueio p = new HabilTipoBloqueio();

                    p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                    p.DescricaoBloqueio = Convert.ToString(Dr["DS_BLOQUEIO"]);
                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos os Bloqueios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<HabilTipoBloqueio> ListarTiposDeBloqueiosSolCompra(int intCodEmpresa)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT lib.CD_BLOQUEIO, count(*) as [N], DS_BLOQUEIO " +
                                "FROM DOCUMENTO ped " +
                                "inner join LIBERACAO_DO_DOCUMENTO lib on PED.CD_DOCUMENTO = lib.cd_documento " +
                                "left join HABIL_TIPO_BLOQUEIO tb on ped.CD_TIPO_DOCUMENTO = tb.CD_TIPO_DOCUMENTO and lib.CD_BLOQUEIO = TB.CD_BLOQUEIO " +
                                "where ped.CD_EMPRESA = @v1 and lib.CD_BLOQUEIO = 1 and lib.DT_LIBERACAO IS NULL and ped.CD_TIPO_DOCUMENTO = 12 AND PED.CD_SITUACAO = 201 " +
                                "GROUP BY lib.CD_BLOQUEIO,DS_BLOQUEIO;";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodEmpresa);
                Dr = Cmd.ExecuteReader();

                List<HabilTipoBloqueio> lista = new List<HabilTipoBloqueio>();

                while (Dr.Read())
                {
                    HabilTipoBloqueio p = new HabilTipoBloqueio();

                    p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                    p.DescricaoBloqueio = Convert.ToString(Dr["DS_BLOQUEIO"]);
                    p.QuantidadeBloqueios = Convert.ToInt32(Dr["N"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos os Bloqueios: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public HabilTipoBloqueio PesquisarHabilTipoBloqueioTipoDocumento(int intCodTipoBloqueio, int intCodTipoDocumento)
        {
            try
            {
                AbrirConexao();
                strSQL = "select distinct  * from HABIL_TIPO_BLOQUEIO Where CD_BLOQUEIO = @v1 and CD_TIPO_DOCUMENTO = @v2";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCodTipoBloqueio);
                Cmd.Parameters.AddWithValue("@v2", intCodTipoDocumento);

                Dr = Cmd.ExecuteReader();

                HabilTipoBloqueio p = null;

                if (Dr.Read())
                {
                    p = new HabilTipoBloqueio();

                    p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                    p.DescricaoBloqueio = Convert.ToString(Dr["DS_BLOQUEIO"]);
                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tipo de Bloqueio: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }


    }
}

