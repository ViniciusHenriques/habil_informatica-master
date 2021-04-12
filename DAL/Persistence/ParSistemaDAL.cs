using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class ParSistemaDAL : Conexao
    {
        string strSQL = "";
        public void Inserir(ParSistema  p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("insert into PARAMETROS_DO_SISTEMA (CD_EMPRESA," +
                                            " CD_CARAC_CATEGORIA," +
                                            " CD_CARAC_LOCALIZACAO," +
                                            " IN_CAT_LOC_ESPELHADA," +
                                            "CD_TIPO_OPERACAO," +
                                            "TX_COR_PADRAO," +
                                            " TX_COR_FUNDO," +
                                            "TP_MENU, " +
                                            "NR_DIAS_ORC_VALIDADE, " +
                                            "VL_PEDIDO_PARA_FRETE_MINIMO," +
                                            " VL_FRETE_MINIMO," +
                                            " CD_SEQUENCIA_GERACAO_NFE, " +
                                            "IN_PED_CRT_REGRAS ," +
                                            " IN_CNF_PEDIDO , " +
                                            "TP_LIST_PEDIDO, " +
                                            "NR_HR_ENVIO_ALERTA)" +
                    " values (@v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10, @v11 ,@v12, @v13, @v14, @v15, @v16) ", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", p.CaracteristaCategoria);
                Cmd.Parameters.AddWithValue("@v3", p.CaracteristaLocalizacao);

                if (p.LocalizacaoEspelhada)
                    Cmd.Parameters.AddWithValue("@v4", 1);
                else
                    Cmd.Parameters.AddWithValue("@v4", 0);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v6", p.CorPadrao);
                Cmd.Parameters.AddWithValue("@v7", p.CorFundo);
                Cmd.Parameters.AddWithValue("@v8", p.TipoMenu);
                Cmd.Parameters.AddWithValue("@v9", p.DiasValidadeOrc);
                Cmd.Parameters.AddWithValue("@v10", p.ValorPedidoParaFreteMinimo);
                Cmd.Parameters.AddWithValue("@v11", p.ValorFreteMinimo);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoSequenciaGeracaoNFe);
                Cmd.Parameters.AddWithValue("@v13", p.CriticaRegras);
                Cmd.Parameters.AddWithValue("@v14", p.ConferePedidos);
                Cmd.Parameters.AddWithValue("@v15", p.TipoListagemPedido);
                Cmd.Parameters.AddWithValue("@v16", p.NumeroHorasEnvioAlerta);
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
                            throw new Exception("Erro ao gravar Parâmetros do Sistema: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception ("Erro ao gravar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void Atualizar(ParSistema p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update PARAMETROS_DO_SISTEMA set CD_CARAC_CATEGORIA = @v2, " +
                                                                        "CD_CARAC_LOCALIZACAO = @v3, " +
                                                                        "IN_CAT_LOC_ESPELHADA = @v4, " +
                                                                        "CD_TIPO_OPERACAO = @v5, " +
                                                                        "TX_COR_PADRAO = @v6, " +
                                                                        "TX_COR_FUNDO = @v7, " +
                                                                        "TP_MENU = @v8, " +
                                                                        "NR_DIAS_ORC_VALIDADE = @v9," +
                                                                        "VL_PEDIDO_PARA_FRETE_MINIMO = @v10," +
                                                                        "VL_FRETE_MINIMO = @v11," +
                                                                        "CD_SEQUENCIA_GERACAO_NFE = @v12, " +
                                                                        "IN_PED_CRT_REGRAS = @v13, " +
                                                                        "IN_CNF_PEDIDO = @v14, " +
                                                                        "TP_LIST_PEDIDO = @v15," +
                                                                        "NR_HR_ENVIO_ALERTA = @v16 Where CD_EMPRESA = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v2", p.CaracteristaCategoria);
                Cmd.Parameters.AddWithValue("@v3", p.CaracteristaLocalizacao);

                if (p.LocalizacaoEspelhada)
                    Cmd.Parameters.AddWithValue("@v4", 1);
                else
                    Cmd.Parameters.AddWithValue("@v4", 0);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoTipoOperacao);
                Cmd.Parameters.AddWithValue("@v6", p.CorPadrao);
                Cmd.Parameters.AddWithValue("@v7", p.CorFundo);
                Cmd.Parameters.AddWithValue("@v8", p.TipoMenu);
                Cmd.Parameters.AddWithValue("@v9", p.DiasValidadeOrc);
                Cmd.Parameters.AddWithValue("@v10", p.ValorPedidoParaFreteMinimo);
                Cmd.Parameters.AddWithValue("@v11", p.ValorFreteMinimo);
                Cmd.Parameters.AddWithValue("@v12", p.CodigoSequenciaGeracaoNFe);
                Cmd.Parameters.AddWithValue("@v13", p.CriticaRegras);
                Cmd.Parameters.AddWithValue("@v14", p.ConferePedidos);
                Cmd.Parameters.AddWithValue("@v15", p.TipoListagemPedido);
                Cmd.Parameters.AddWithValue("@v16", p.NumeroHorasEnvioAlerta);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar ParSistema: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from PARAMETROS_DO_SISTEMA Where CD_EMPRESA = @v1;", Con);
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
                            throw new Exception("Erro ao excluir PARAMETROS_DO_SISTEMA: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir PARAMETROS_DO_SISTEMA: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public ParSistema PesquisarParSistema(Int64 CodigoEmpresa)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select PS.*, P.[NM_PESSOA]" +
                                    " from PARAMETROS_DO_SISTEMA as PS " +
                                    "    Inner Join Empresa as E " +
                                    "      On E.CD_EMPRESA = PS.CD_EMPRESA  " +
                                    "    Inner Join Pessoa as P " +
                                    "      On E.CD_PESSOA = P.CD_PESSOA  " +
                                    " Where E.CD_EMPRESA = @v1", Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoEmpresa);
                Dr = Cmd.ExecuteReader();
                ParSistema p = null;
                if (Dr.Read())
                {
                    p = new ParSistema();
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.NomeEmpresa = Dr["NM_PESSOA"].ToString();
                    p.CaracteristaCategoria = Dr["CD_CARAC_CATEGORIA"].ToString();
                    p.CaracteristaLocalizacao = Dr["CD_CARAC_LOCALIZACAO"].ToString();
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CorPadrao = Dr["TX_COR_PADRAO"].ToString();
                    p.CorFundo= Dr["TX_COR_FUNDO"].ToString();
                    p.TipoMenu = Convert.ToInt32(Dr["TP_MENU"]);
                    if (Dr["CD_TP_INV_AJUSTE"] != DBNull.Value)
                        p.TipoAjusteInventario = Convert.ToInt32(Dr["CD_TP_INV_AJUSTE"]);
                    p.DiasValidadeOrc = Convert.ToInt16(Dr["NR_DIAS_ORC_VALIDADE"]);
                    p.ValorPedidoParaFreteMinimo = Convert.ToDecimal(Dr["VL_PEDIDO_PARA_FRETE_MINIMO"]);
                    p.ValorFreteMinimo = Convert.ToDecimal(Dr["VL_FRETE_MINIMO"]);
                    p.CodigoSequenciaGeracaoNFe = Convert.ToInt32(Dr["CD_SEQUENCIA_GERACAO_NFE"]);
                    p.NumeroHorasEnvioAlerta = Convert.ToInt32(Dr["NR_HR_ENVIO_ALERTA"]);

                    p.ConferePedidos = Convert.ToBoolean(Dr["IN_CNF_PEDIDO"]);
                    p.CriticaRegras = Convert.ToBoolean(Dr["IN_PED_CRT_REGRAS"]);

                    if (Dr["TP_LIST_PEDIDO"] != DBNull.Value)
                        p.TipoListagemPedido = Convert.ToInt32(Dr["TP_LIST_PEDIDO"]);


                    if (Dr["IN_CAT_LOC_ESPELHADA"].ToString() == "1")
                        p.LocalizacaoEspelhada = true;
                    else
                        p.LocalizacaoEspelhada = false;
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ParSistema> ListarParSistemas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "Select * from [VW_PAR_SISTEMA]";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ParSistema> lista = new List<ParSistema>();

                while (Dr.Read())
                {
                    ParSistema p = new ParSistema();
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.NomeEmpresa = Dr["NM_PESSOA"].ToString();
                    p.CaracteristaCategoria = Dr["CD_CARAC_CATEGORIA"].ToString();
                    p.CaracteristaLocalizacao = Dr["CD_CARAC_LOCALIZACAO"].ToString();
                    p.CodigoTipoOperacao = Convert.ToInt32(Dr["CD_TIPO_OPERACAO"]);
                    p.CorPadrao = Dr["TX_COR_PADRAO"].ToString();
                    p.CorFundo = Dr["TX_COR_FUNDO"].ToString();
                    p.TipoMenu = Convert.ToInt32(Dr["TP_MENU"]);
                    p.DiasValidadeOrc = Convert.ToInt16(Dr["NR_DIAS_ORC_VALIDADE"]);
                    p.ValorPedidoParaFreteMinimo = Convert.ToDecimal(Dr["VL_PEDIDO_PARA_FRETE_MINIMO"]);
                    p.ValorFreteMinimo = Convert.ToDecimal(Dr["VL_FRETE_MINIMO"]);
                    p.NumeroHorasEnvioAlerta = Convert.ToInt32(Dr["NR_HR_ENVIO_ALERTA"]);
                    p.CodigoSequenciaGeracaoNFe = Convert.ToInt32(Dr["CD_SEQUENCIA_GERACAO_NFE"]);
                    if (Dr["TP_LIST_PEDIDO"] != DBNull.Value)
                        p.TipoListagemPedido = Convert.ToInt32(Dr["TP_LIST_PEDIDO"]);
                    if (Dr["IN_CNF_PEDIDO"].ToString() == "1")
                        p.ConferePedidos = true;
                    else
                        p.ConferePedidos = false;

                    if (Dr["IN_PED_CRT_REGRAS"].ToString() == "1")
                        p.CriticaRegras = true;
                    else
                        p.CriticaRegras = false;

                    if (Dr["IN_CAT_LOC_ESPELHADA"].ToString() == "1")
                        p.LocalizacaoEspelhada = true;
                    else
                        p.LocalizacaoEspelhada = false;

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ParSistema>  ListarParSistemasInclusao()
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand(" Select E.[CD_EMPRESA], P.[NM_FANTASIA] AS NM_PESSOA " +
                                    "  from Empresa as E " +
                                    "     Inner Join Pessoa as P " +
                                    "       On E.CD_PESSOA = P.CD_PESSOA  " +
                                    "  Where E.CD_EMPRESA NOT IN (SELECT CD_EMPRESA FROM PARAMETROS_DO_SISTEMA) ", Con);
                List<ParSistema> lista = new List<ParSistema>();
                Dr = Cmd.ExecuteReader();

                ParSistema p = null;
                while (Dr.Read())
                {
                    p = new ParSistema();
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.NomeEmpresa = Dr["NM_PESSOA"].ToString();
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ParSistema> ListarParSistemasAlteracao()
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand(" Select E.[CD_EMPRESA], P.[NM_FANTASIA] AS [NM_PESSOA] " +
                                    "  from PARAMETROS_DO_SISTEMA as PS " +
                                    "     Inner Join Empresa as E " +
                                    "       On E.CD_EMPRESA = PS.CD_EMPRESA " +
                                    "     Inner Join Pessoa as P " +
                                    "       On E.CD_PESSOA = P.CD_PESSOA  " +
                                    "   ", Con);
                List<ParSistema> lista = new List<ParSistema>();
                Dr = Cmd.ExecuteReader();
                ParSistema p = null;
                if (Dr.Read())
                {
                    p = new ParSistema();
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.NomeEmpresa = Dr["NM_PESSOA"].ToString();
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public string FormataCategoria(long CodEmpresa)
        {
            try
            {
                AbrirConexao();

                strSQL = " Select PS.[CD_CARAC_CATEGORIA]" +
                         "  from PARAMETROS_DO_SISTEMA as PS " +
                         "     Inner Join Empresa as E " +
                         "       On E.CD_EMPRESA = PS.CD_EMPRESA " +
                         "     Inner Join Pessoa as P " +
                         "       On E.CD_PESSOA = P.CD_PESSOA  ";

                if (CodEmpresa > 0)
                    strSQL = strSQL + " WHERE PS.[CD_EMPRESA] = " + CodEmpresa.ToString(); 

                Cmd = new SqlCommand(strSQL, Con);
                List<ParSistema> lista = new List<ParSistema>();
                Dr = Cmd.ExecuteReader();
                ParSistema p = null;
                if (Dr.Read())
                {
                    return Dr[0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }


        public string FormataLocalizacao(long CodEmpresa)
        {
            try
            {
                AbrirConexao();
                strSQL = " Select PS.[CD_CARAC_LOCALIZACAO]" +
                         "  from PARAMETROS_DO_SISTEMA as PS " +
                         "     Inner Join Empresa as E " +
                         "       On E.CD_EMPRESA = PS.CD_EMPRESA " +
                         "     Inner Join Pessoa as P " +
                         "       On E.CD_PESSOA = P.CD_PESSOA  ";

                if (CodEmpresa > 0)
                    strSQL = strSQL + " WHERE PS.[CD_EMPRESA] = " + CodEmpresa.ToString();

                Cmd = new SqlCommand(strSQL, Con);
                List<ParSistema> lista = new List<ParSistema>();
                Dr = Cmd.ExecuteReader();
                ParSistema p = null;
                if (Dr.Read())
                {
                    return Dr[0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void InserirTpAjuste(int intTpAjuste, int intEmpresa)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("UPDATE [PARAMETROS_DO_SISTEMA] SET CD_TP_INV_AJUSTE = " + intTpAjuste  + "  Where CD_EMPRESA = " + intEmpresa, Con);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Inserir Tipo de Ajuste de Inventario: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void TipoDeListagemDePedidos(ref int intMenu, int intEmpresa)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "Select TP_LIST_PEDIDO from [PARAMETROS_DO_SISTEMA] where CD_EMPRESA = " + intEmpresa;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ParSistema> lista = new List<ParSistema>();

                if(Dr.Read())
                {
                    ParSistema p = new ParSistema();
                    intMenu = Convert.ToInt32(Dr["TP_LIST_PEDIDO"]);
                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public int ConferePedido(int intMenu, int intEmpresa)
        {
            try
            {
                AbrirConexao();

                strSQL = "Select IN_CNF_PEDIDO from [PARAMETROS_DO_SISTEMA] where CD_EMPRESA = " + intEmpresa;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ParSistema> lista = new List<ParSistema>();

                if (Dr.Read())
                {
                    ParSistema p = new ParSistema();
                    intMenu = Convert.ToInt32(Dr["IN_CNF_PEDIDO"]);
                }
                return intMenu;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Parâmetros do Sistema: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
