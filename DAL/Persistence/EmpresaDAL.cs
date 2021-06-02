using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class EmpresaDAL : Conexao
    {
        public void Inserir(Empresa  p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("insert into Empresa (CD_PESSOA, CD_REG_TRIBUTARIO, CD_SITUACAO) values (@v2, @v3, @v4) SELECT SCOPE_IDENTITY();", Con);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v3", p.CodHabil_RegTributario);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoSituacao);
                p.CodigoEmpresa = Convert.ToInt32(Cmd.ExecuteScalar());

                ParSistema par = new ParSistema();
                ParSistemaDAL parDAL = new ParSistemaDAL();
                par.CaracteristaCategoria = "999-999-999-999-999";
                par.CaracteristaLocalizacao = "999-999-999-999-999";
                par.LocalizacaoEspelhada = false;
                par.CodigoTipoOperacao = 1;
                par.CorPadrao = "#7c7979";
                par.CorFundo = "#ffffff";
                par.TipoMenu = 1;
                par.TipoAjusteInventario = 0;
                par.DiasValidadeOrc = 0;
                par.ValorPedidoParaFreteMinimo = 0;
                par.ValorFreteMinimo = 0;
                par.CodigoSequenciaGeracaoNFe = 209;
                par.TipoListagemPedido = 153;
                par.CriticaRegras = false;
                par.ConferePedidos = false;
                par.NumeroHorasEnvioAlerta = 1;
                par.CodigoEmpresa = p.CodigoEmpresa;
                parDAL.Inserir(par);

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
                            throw new Exception("Erro ao gravar Empresa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception ("Erro ao gravar Empresa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void Atualizar(Empresa p, List<GeradorSequencialDocumentoEmpresa> ListaGeradorSequencialDocumentoEmpresas)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update Empresa set CD_PESSOA = @v4, CD_REG_TRIBUTARIO = @v2 Where CD_EMPRESA = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoEmpresa);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoPessoa);
                Cmd.Parameters.AddWithValue("@v2", p.CodHabil_RegTributario);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Empresa: " + ex.Message.ToString());
            }
            finally
            {
                
                
                FecharConexao();
                GeradorSequencialDocumentoEmpresaDAL pe3 = new GeradorSequencialDocumentoEmpresaDAL();
                pe3.Inserir(p.CodigoEmpresa, ListaGeradorSequencialDocumentoEmpresas);
            }
        }
        public void Excluir(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from Empresa Where CD_EMPRESA = @v1;", Con);
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
                            throw new Exception("Erro ao excluir Empresa: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Empresa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Empresa PesquisarEmpresa(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select E.[CD_EMPRESA], P.[NM_PESSOA], E.[CD_PESSOA], E.CD_REG_TRIBUTARIO from EMPRESA as E Inner Join Pessoa as P On E.CD_PESSOA = P.CD_PESSOA  Where E.CD_EMPRESA = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Empresa p = null;
                if (Dr.Read())
                {
                    p = new Empresa();
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.NomeEmpresa = Dr["NM_PESSOA"].ToString();
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);
                    
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Empresa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Empresa> ListarEmpresas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "SELECT dbo.EMPRESA.CD_EMPRESA, dbo.EMPRESA.CD_REG_TRIBUTARIO, dbo.EMPRESA.CD_PESSOA, dbo.PESSOA.NM_PESSOA, dbo.PESSOA.NM_FANTASIA " +
                                    "FROM dbo.EMPRESA INNER JOIN dbo.PESSOA ON dbo.EMPRESA.CD_PESSOA = dbo.PESSOA.CD_PESSOA";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Empresa> lista = new List<Empresa>();

                while (Dr.Read())
                {
                    Empresa p = new Empresa();
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.NomeEmpresa = Dr["NM_PESSOA"].ToString();
                    p.NomeFantasia = Dr["NM_FANTASIA"].ToString();

                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Empresa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public Empresa PesquisarEmpresaPessoa(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select E.[CD_EMPRESA], P.[NM_PESSOA], P.[NM_FANTASIA], E.[CD_PESSOA], E.CD_REG_TRIBUTARIO from EMPRESA as E Inner Join Pessoa as P On E.CD_PESSOA = P.CD_PESSOA  Where E.CD_PESSOA = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Empresa p = null;
                if (Dr.Read())
                {
                    p = new Empresa();
                    p.CodigoEmpresa = Convert.ToInt64(Dr["CD_EMPRESA"]);
                    p.CodigoPessoa = Convert.ToInt64(Dr["CD_PESSOA"]);
                    p.NomeEmpresa = Dr["NM_PESSOA"].ToString();
                    p.NomeFantasia = Dr["NM_FANTASIA"].ToString();
                    p.CodHabil_RegTributario = Convert.ToInt32(Dr["CD_REG_TRIBUTARIO"]);

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Empresa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        
        public string RetornaNomeEmpresa(int intCodigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select P.[NM_PESSOA] from EMPRESA as E Inner Join Pessoa as P On E.CD_PESSOA = P.CD_PESSOA  Where E.CD_EMPRESA = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);
                Dr = Cmd.ExecuteReader();
                string StrNome = "";
                if (Dr.Read())
                {
                    StrNome = Dr["NM_PESSOA"].ToString();
                }
                return StrNome;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Empresa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public string RetornaNomeFantasia(int intCodigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select P.[NM_FANTASIA] from EMPRESA as E Inner Join Pessoa as P On E.CD_PESSOA = P.CD_PESSOA  Where E.CD_EMPRESA = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", intCodigo);
                Dr = Cmd.ExecuteReader();
                string StrNome = "";
                if (Dr.Read())
                {
                    StrNome = Dr["NM_FANTASIA"].ToString();
                }
                return StrNome;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Empresa: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

    }
}
