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
    public class RegraFreteDAL : Conexao
    {
        protected string strSQL = "";
        public void Inserir(RegraFrete p, List<CidadeRegraFrete> listaCidades)
        {
            try
            {
                AbrirConexao();
                strSQL = "INSERT INTO [dbo].[REGRA_DE_FRETE] " +
                                       "([CD_TRANSPORTADOR] " +
                                       ",[DE_PARA_11] " +
                                       ",[DE_PARA_12] " +
                                       ",[DE_PARA_21] " +
                                       ",[DE_PARA_22] " +
                                       ",[DE_PARA_PCT_11] " +
                                       ",[DE_PARA_PCT_12] " +
                                       ",[VL_FRETE_MINIMO] " +
                                       ",[VL_GRIS] " +
                                       ",[DE_PARA_31] " +
                                       ",[DE_PARA_32] " +
                                       ",[REGIAO]) " +
                "values(@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12) SELECT SCOPE_IDENTITY();";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTransportador);
                Cmd.Parameters.AddWithValue("@v2", p.DePara11);
                Cmd.Parameters.AddWithValue("@v3", p.DePara12);
                Cmd.Parameters.AddWithValue("@v4", p.DePara21);
                Cmd.Parameters.AddWithValue("@v5", p.DePara22);
                Cmd.Parameters.AddWithValue("@v6", p.DeParaPct11);
                Cmd.Parameters.AddWithValue("@v7", p.DeParaPct12);
                Cmd.Parameters.AddWithValue("@v8", p.ValorFreteMinimo);
                Cmd.Parameters.AddWithValue("@v9", p.ValorGRIS);
                Cmd.Parameters.AddWithValue("@v10", p.DePara31);
                Cmd.Parameters.AddWithValue("@v11", p.DePara32);
                Cmd.Parameters.AddWithValue("@v12", p.Regiao);

                p.CodigoIndex = Convert.ToInt32(Cmd.ExecuteScalar());
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
                            throw new Exception("Erro ao Incluir REGRA DE FRETE: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar REGRA DE FRETE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

                CidadeRegraFreteDAL citDAL = new CidadeRegraFreteDAL();
                citDAL.Inserir(listaCidades, p.CodigoIndex);
            }
        }
        public void Atualizar(RegraFrete p, List<CidadeRegraFrete> listaCidades)
        {
            try
            {

                AbrirConexao();
                strSQL = "UPDATE [dbo].[REGRA_DE_FRETE] " +
                           "SET[CD_TRANSPORTADOR] = @v1 " +
                              ",[DE_PARA_11] = @v2 " +
                              ",[DE_PARA_12] = @v3 " +
                              ",[DE_PARA_21] = @v4 " +
                              ",[DE_PARA_22] = @v5 " +
                              ",[DE_PARA_PCT_11] = @v6 " +
                              ",[DE_PARA_PCT_12] = @v7 " +
                              ",[VL_FRETE_MINIMO] = @v8 " +
                              ",[VL_GRIS] = @v9 " +
                              ",[DE_PARA_31] = @v10 " +
                              ",[DE_PARA_32] = @v11 " +
                              ",[REGIAO] = @v12 " +
                         "WHERE [CD_INDEX] = @CODIGO";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@CODIGO", p.CodigoIndex);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoTransportador);
                Cmd.Parameters.AddWithValue("@v2", p.DePara11);
                Cmd.Parameters.AddWithValue("@v3", p.DePara12);
                Cmd.Parameters.AddWithValue("@v4", p.DePara21);
                Cmd.Parameters.AddWithValue("@v5", p.DePara22);
                Cmd.Parameters.AddWithValue("@v6", p.DeParaPct11);
                Cmd.Parameters.AddWithValue("@v7", p.DeParaPct12);
                Cmd.Parameters.AddWithValue("@v8", p.ValorFreteMinimo);
                Cmd.Parameters.AddWithValue("@v9", p.ValorGRIS);
                Cmd.Parameters.AddWithValue("@v10", p.DePara31);
                Cmd.Parameters.AddWithValue("@v11", p.DePara32);
                Cmd.Parameters.AddWithValue("@v12", p.Regiao);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar REGRA DE FRETE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
                CidadeRegraFreteDAL citDAL = new CidadeRegraFreteDAL();
                citDAL.Inserir(listaCidades, p.CodigoIndex);
            }
        }
        public void Excluir(int Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("DELETE REGRA_DE_FRETE WHERE CD_INDEX = @v1", Con);
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
                            throw new Exception("Erro ao excluir REGRA DE FRETE: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir REGRA DE FRETE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<RegraFrete> ListarRegraFrete(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "SELECT * FROM VW_REGRA_DE_FRETE ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);


                if (strOrdem != "")
                    strSQL = strSQL + " Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<RegraFrete> lista = new List<RegraFrete>();

                while (Dr.Read())
                {
                    RegraFrete p = new RegraFrete();

                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
                    p.DePara11 = Convert.ToDecimal(Dr["DE_PARA_11"]);
                    p.DePara12 = Convert.ToDecimal(Dr["DE_PARA_12"]);
                    p.DePara21 = Convert.ToDecimal(Dr["DE_PARA_21"]);
                    p.DePara22 = Convert.ToDecimal(Dr["DE_PARA_22"]);
                    p.DePara31 = Convert.ToDecimal(Dr["DE_PARA_31"]);
                    p.DePara32 = Convert.ToDecimal(Dr["DE_PARA_32"]);
                    p.DeParaPct11 = Convert.ToDecimal(Dr["DE_PARA_PCT_11"]);
                    p.DeParaPct12 = Convert.ToDecimal(Dr["DE_PARA_PCT_12"]);
                    p.ValorFreteMinimo= Convert.ToDecimal(Dr["VL_FRETE_MINIMO"]);
                    p.ValorGRIS = Convert.ToDecimal(Dr["VL_GRIS"]);
                    p.Regiao = Dr["REGIAO"].ToString();
                    p.Cpl_InscricaoTransportador = Dr["NR_INSCRICAO"].ToString();
                    p.Cpl_NomeTransportador = Dr["NM_TRANSPORTADOR"].ToString();

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos REGRAS DE FRETE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public List<RegraFrete> ListarRegraFreteCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "SELECT * ,P.NM_PESSOA AS NM_TRANSPORTADOR,INS.NR_INSCRICAO "+
                                "FROM REGRA_DE_FRETE AS REG " +
                                "INNER JOIN PESSOA AS P ON P.CD_PESSOA = REG.CD_TRANSPORTADOR " +
                                "INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = P.CD_PESSOA ";


                strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor;

                strSQL = strSQL + " ORDER BY CD_INDEX DESC ";
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<RegraFrete> lista = new List<RegraFrete>();

                while (Dr.Read())
                {
                    RegraFrete p = new RegraFrete();
                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
                    p.DePara11 = Convert.ToDecimal(Dr["DE_PARA_11"]);
                    p.DePara12 = Convert.ToDecimal(Dr["DE_PARA_12"]);
                    p.DePara21 = Convert.ToDecimal(Dr["DE_PARA_21"]);
                    p.DePara22 = Convert.ToDecimal(Dr["DE_PARA_22"]);
                    p.DePara31 = Convert.ToDecimal(Dr["DE_PARA_31"]);
                    p.DePara32 = Convert.ToDecimal(Dr["DE_PARA_32"]);
                    p.DeParaPct11 = Convert.ToDecimal(Dr["DE_PARA_PCT_11"]);
                    p.DeParaPct12 = Convert.ToDecimal(Dr["DE_PARA_PCT_12"]);
                    p.ValorFreteMinimo = Convert.ToDecimal(Dr["VL_FRETE_MINIMO"]);
                    p.ValorGRIS = Convert.ToDecimal(Dr["VL_GRIS"]);
                    p.Regiao = Dr["REGIAO"].ToString();
                    p.Cpl_InscricaoTransportador = Dr["NR_INSCRICAO"].ToString();
                    p.Cpl_NomeTransportador = Dr["NM_TRANSPORTADOR"].ToString();
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar TODAS REGRAS DE FRETE: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public RegraFrete PesquisarRegraFrete(string strInscricao, string strCodigoCidade)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select " +
                                        "RF.*, INS.NR_INSCRICAO AS NR_INSCRICAO_TRANSP, CITY.CD_IBGE " +
                                    "from " +
                                        "REGRA_DE_FRETE AS RF INNER JOIN PESSOA_INSCRICAO AS INS ON INS.CD_PESSOA = RF.CD_TRANSPORTADOR " +
                                        "INNER JOIN CIDADE_REGRA_DE_FRETE AS CITY ON CITY.CD_REGRA_DE_FRETE = RF.CD_INDEX " +
                                    "where " +
                                        "NR_INSCRICAO = @v1 AND CD_IBGE = @v2", Con);

                Cmd.Parameters.AddWithValue("@v1", strInscricao);
                Cmd.Parameters.AddWithValue("@v2", strCodigoCidade);

                Dr = Cmd.ExecuteReader();

                RegraFrete p = null;
                if (Dr.Read())
                {
                    p = new RegraFrete();
                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
                    if(Dr["DE_PARA_11"] != DBNull.Value)
                        p.DePara11 = Convert.ToDecimal(Dr["DE_PARA_11"]);
                    if (Dr["DE_PARA_12"] != DBNull.Value)
                        p.DePara12 = Convert.ToDecimal(Dr["DE_PARA_12"]);
                    if (Dr["DE_PARA_21"] != DBNull.Value)
                        p.DePara21 = Convert.ToDecimal(Dr["DE_PARA_21"]);
                    if (Dr["DE_PARA_22"] != DBNull.Value)
                        p.DePara22 = Convert.ToDecimal(Dr["DE_PARA_22"]);
                    if (Dr["DE_PARA_31"] != DBNull.Value)
                        p.DePara31 = Convert.ToDecimal(Dr["DE_PARA_31"]);
                    if (Dr["DE_PARA_32"] != DBNull.Value)
                        p.DePara32 = Convert.ToDecimal(Dr["DE_PARA_32"]);
                    if (Dr["DE_PARA_41"] != DBNull.Value)
                        p.DePara41 = Convert.ToDecimal(Dr["DE_PARA_41"]);
                    if (Dr["DE_PARA_42"] != DBNull.Value)
                        p.DePara42 = Convert.ToDecimal(Dr["DE_PARA_42"]);
                    if (Dr["DE_PARA_51"] != DBNull.Value)
                        p.DePara51 = Convert.ToDecimal(Dr["DE_PARA_51"]);
                    if (Dr["DE_PARA_52"] != DBNull.Value)
                        p.DePara52 = Convert.ToDecimal(Dr["DE_PARA_52"]);
                    if (Dr["DE_PARA_61"] != DBNull.Value)
                        p.DePara61 = Convert.ToDecimal(Dr["DE_PARA_61"]);
                    if (Dr["DE_PARA_62"] != DBNull.Value)
                        p.DePara62 = Convert.ToDecimal(Dr["DE_PARA_62"]);
                    if (Dr["DE_PARA_71"] != DBNull.Value)
                        p.DePara71 = Convert.ToDecimal(Dr["DE_PARA_71"]);
                    if (Dr["DE_PARA_72"] != DBNull.Value)
                        p.DePara72 = Convert.ToDecimal(Dr["DE_PARA_72"]);

                    if (Dr["DE_PARA_PCT_11"] != DBNull.Value)
                        p.DeParaPct11 = Convert.ToDecimal(Dr["DE_PARA_PCT_11"]);
                    if (Dr["DE_PARA_PCT_12"] != DBNull.Value)
                        p.DeParaPct12 = Convert.ToDecimal(Dr["DE_PARA_PCT_12"]);
                    if (Dr["DE_PARA_PCT_13"] != DBNull.Value)
                        p.DeParaPct13 = Convert.ToDecimal(Dr["DE_PARA_PCT_13"]);
                    if (Dr["DE_PARA_PCT_14"] != DBNull.Value)
                        p.DeParaPct14 = Convert.ToDecimal(Dr["DE_PARA_PCT_14"]);
                    if (Dr["DE_PARA_PCT_51"] != DBNull.Value)
                        p.DeParaPct15 = Convert.ToDecimal(Dr["DE_PARA_PCT_51"]);
                    if (Dr["DE_PARA_PCT_61"] != DBNull.Value)
                        p.DeParaPct16 = Convert.ToDecimal(Dr["DE_PARA_PCT_61"]);
                    if (Dr["DE_PARA_PCT_71"] != DBNull.Value)
                        p.DeParaPct17 = Convert.ToDecimal(Dr["DE_PARA_PCT_71"]);

                    if (Dr["DE_PARA_EXCEDENTE_11"] != DBNull.Value)
                        p.DeParaExcedente1 = Convert.ToDecimal(Dr["DE_PARA_EXCEDENTE_11"]);

                    if (Dr["DE_PARA_EXCEDENTE_12"] != DBNull.Value)
                        p.DeParaExcedente2 = Convert.ToDecimal(Dr["DE_PARA_EXCEDENTE_12"]);

                    if (Dr["VL_GRIS_MINIMO"] != DBNull.Value)
                        p.ValorGRISMinimo = Convert.ToDecimal(Dr["VL_GRIS_MINIMO"]);

                    if (Dr["VL_PEDAGIO"] != DBNull.Value)
                        p.ValorPedagio = Convert.ToDecimal(Dr["VL_PEDAGIO"]);

                    if (Dr["VL_PEDAGIO_MAXIMO"] != DBNull.Value)
                        p.ValorPedagioMaximo = Convert.ToDecimal(Dr["VL_PEDAGIO_MAXIMO"]);

                    if (Dr["VL_AD_VALOR"] != DBNull.Value)
                        p.ValorAD = Convert.ToDecimal(Dr["VL_AD_VALOR"]);

                    if (Dr["VL_SEGURO"] != DBNull.Value)
                        p.ValorSeguro = Convert.ToDecimal(Dr["VL_SEGURO"]);

                    if (Dr["VL_SEGURO_MINIMO"] != DBNull.Value)
                        p.ValorSeguroMinimo = Convert.ToDecimal(Dr["VL_SEGURO_MINIMO"]);

                    if (Dr["IN_CALCULA_ADVALOR_DE_PARA1"] != DBNull.Value)
                        p.IndicadorCalcularAdValorDePara1 = Convert.ToInt32(Dr["IN_CALCULA_ADVALOR_DE_PARA1"]);

                    if (Dr["IN_CALCULA_ADVALOR_DE_PARA2"] != DBNull.Value)
                        p.IndicadorCalcularAdValorDePara2 = Convert.ToInt32(Dr["IN_CALCULA_ADVALOR_DE_PARA2"]);

                    if (Dr["VL_POR_TONELADA"] != DBNull.Value)
                        p.ValorPorTonelada = Convert.ToDecimal(Dr["VL_POR_TONELADA"]);

                    p.IndicadorTipoCalculo = Convert.ToDecimal(Dr["IN_TP_CALCULO"]);
                    p.ValorFreteMinimo = Convert.ToDecimal(Dr["VL_FRETE_MINIMO"]);
                    p.ValorGRIS = Convert.ToDecimal(Dr["VL_GRIS"]);
                    p.Inscricao = Dr["NR_INSCRICAO_TRANSP"].ToString();
                    p.Regiao = Dr["REGIAO"].ToString();
                }

                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar regra de frete: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegraFrete Pesquisarteste(string regiao)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select * FROM REGRA_DE_FRETE WHERE REGIAO = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", regiao);
                

                Dr = Cmd.ExecuteReader();

                RegraFrete p = null;
                if (Dr.Read())
                {
                    p = new RegraFrete();
                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    
                    p.Regiao = Dr["REGIAO"].ToString();
                }

                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar regra de frete: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegraFrete PesquisarRegraFreteIndex(int Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select * from REGRA_DE_FRETE where CD_INDEX = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                RegraFrete p = null;
                if (Dr.Read())
                {
                    p = new RegraFrete();
                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
                    p.DePara11 = Convert.ToDecimal(Dr["DE_PARA_11"]);
                    p.DePara12 = Convert.ToDecimal(Dr["DE_PARA_12"]);
                    p.DePara21 = Convert.ToDecimal(Dr["DE_PARA_21"]);
                    p.DePara22 = Convert.ToDecimal(Dr["DE_PARA_22"]);
                    p.DePara31 = Convert.ToDecimal(Dr["DE_PARA_31"]);
                    p.DePara32 = Convert.ToDecimal(Dr["DE_PARA_32"]);
                    p.DeParaPct11 = Convert.ToDecimal(Dr["DE_PARA_PCT_11"]);
                    p.DeParaPct12 = Convert.ToDecimal(Dr["DE_PARA_PCT_12"]);
                    p.ValorFreteMinimo = Convert.ToDecimal(Dr["VL_FRETE_MINIMO"]);
                    p.ValorGRIS = Convert.ToDecimal(Dr["VL_GRIS"]);
                    p.Regiao = Dr["REGIAO"].ToString();
                }

                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar regra de frete por index: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public RegraFrete PesquisarRegraFreteRegiaoETransportador(string Regiao, Int64 CodigoTransportador, int CodigoRegraAtual)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("select * from REGRA_DE_FRETE where REGIAO = @v1 AND CD_TRANSPORTADOR = @v2 AND CD_INDEX != @v3", Con);

                Cmd.Parameters.AddWithValue("@v1", Regiao);
                Cmd.Parameters.AddWithValue("@v2", CodigoTransportador);
                Cmd.Parameters.AddWithValue("@v3", CodigoRegraAtual);

                Dr = Cmd.ExecuteReader();

                RegraFrete p = null;
                if (Dr.Read())
                {
                    p = new RegraFrete();
                    p.CodigoIndex = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoTransportador = Convert.ToInt32(Dr["CD_TRANSPORTADOR"]);
                    p.DePara11 = Convert.ToDecimal(Dr["DE_PARA_11"]);
                    p.DePara12 = Convert.ToDecimal(Dr["DE_PARA_12"]);
                    p.DePara21 = Convert.ToDecimal(Dr["DE_PARA_21"]);
                    p.DePara22 = Convert.ToDecimal(Dr["DE_PARA_22"]);
                    p.DePara31 = Convert.ToDecimal(Dr["DE_PARA_31"]);
                    p.DePara32 = Convert.ToDecimal(Dr["DE_PARA_32"]);
                    p.DeParaPct11 = Convert.ToDecimal(Dr["DE_PARA_PCT_11"]);
                    p.DeParaPct12 = Convert.ToDecimal(Dr["DE_PARA_PCT_12"]);
                    p.ValorFreteMinimo = Convert.ToDecimal(Dr["VL_FRETE_MINIMO"]);
                    p.ValorGRIS = Convert.ToDecimal(Dr["VL_GRIS"]);
                    p.Regiao = Dr["REGIAO"].ToString();
                }

                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar regra de frete por index: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
