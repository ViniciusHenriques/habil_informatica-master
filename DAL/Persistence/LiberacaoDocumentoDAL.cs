using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Persistence
{
    public class LiberacaoDocumentoDAL : Conexao
    {
        public void Inserir(LiberacaoDocumento p)
        {
            try
            {

                AbrirConexao();
                Cmd = new SqlCommand("insert into LIBERACAO_DO_DOCUMENTO (CD_DOCUMENTO, CD_BLOQUEIO, CD_USUARIO, CD_MAQUINA,DT_LANCAMENTO) values (@v2, @v3,@v4,@v5,@v7); SELECT SCOPE_IDENTITY()", Con);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoBloqueio);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoMaquina);
                Cmd.Parameters.AddWithValue("@v7", p.DataLancamento);
                //                Cmd.ExecuteNonQuery();
                p.CodigoLiberacao = Convert.ToInt32(Cmd.ExecuteScalar());
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
                            throw new Exception("Erro ao gravar a Liberação: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar a Liberação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(LiberacaoDocumento p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update LIBERACAO_DO_DOCUMENTO set CD_DOCUMENTO = @v2, CD_BLOQUEIO = @v3, CD_USUARIO = @v4, CD_MAQUINA = @v5, DT_LIBERACAO = @v6, DT_LANCAMENTO = @v7" +
                    "where CD_INDEX = @v1) " +
                                      "SELECT SCOPE_IDENTITY(), Con);");
                Cmd.Parameters.AddWithValue("@v1", p.CodigoLiberacao);
                Cmd.Parameters.AddWithValue("@v2", p.CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoBloqueio);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoUsuario);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoMaquina);
                Cmd.Parameters.AddWithValue("@v6", p.DataLiberacao);
                Cmd.Parameters.AddWithValue("@v7", p.DataLancamento);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a Liberação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Excluir(long Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from LIBERACAO_DO_DOCUMENTO Where CD_INDEX = @v1", Con);
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
                            throw new Exception("Erro ao excluir a Liberação: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Liberação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public LiberacaoDocumento PesquisarLiberacaoId(int Codigo)
        {
            try
            {
                AbrirConexao();
                if (Codigo == 0)
                {
                    Cmd = new SqlCommand("Select * from LIBERACAO_DO_DOCUMENTO ", Con);
                }
                else
                {
                    Cmd = new SqlCommand("Select Top 1 * from LIBERACAO_DO_DOCUMENTO Where CD_INDEX = @v1", Con);
                    Cmd.Parameters.AddWithValue("@v1", Codigo);
                }

                Dr = Cmd.ExecuteReader();
                LiberacaoDocumento p = null;
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        p = new LiberacaoDocumento();
                        p.CodigoLiberacao = Convert.ToInt32(Dr["CD_INDEX"]);
                        p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                        p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                        p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                        p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                        if (!string.IsNullOrWhiteSpace(Dr["DT_LIBERACAO"].ToString()))
                            p.DataLiberacao = Convert.ToDateTime(Dr["DT_LIBERACAO"]);
                        if (!string.IsNullOrWhiteSpace(Dr["DT_LANCAMENTO"].ToString()))
                            p.DataLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    }
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Liberação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public LiberacaoDocumento PesquisarLiberacaoDocumento(decimal CodigoDocumento, int CodigoBloqueio)
        {
            try
            {
                AbrirConexao();
                if (CodigoBloqueio != 0)
                {
                    Cmd = new SqlCommand("Select * from LIBERACAO_DO_DOCUMENTO where CD_DOCUMENTO = @v1 AND CD_BLOQUEIO = @v2", Con);
                }
                else
                {
                    Cmd = new SqlCommand("Select * from LIBERACAO_DO_DOCUMENTO where CD_DOCUMENTO = @v1", Con);
                }

                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                Cmd.Parameters.AddWithValue("@v2", CodigoBloqueio);
                Dr = Cmd.ExecuteReader();
                LiberacaoDocumento p = null;
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        p = new LiberacaoDocumento();
                        p.CodigoLiberacao = Convert.ToInt32(Dr["CD_INDEX"]);
                        p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                        p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                        p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                        p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                        if (!string.IsNullOrWhiteSpace(Dr["DT_LIBERACAO"].ToString()))
                            p.DataLiberacao = Convert.ToDateTime(Dr["DT_LIBERACAO"]);
                        if (!string.IsNullOrWhiteSpace(Dr["DT_LANCAMENTO"].ToString()))
                            p.DataLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    }
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Liberação: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<LiberacaoDocumento> ListarLiberacao(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();
                string strSQL = "SELECT LIB.* ,ISNULL(U.NM_COMPLETO,' ') AS NM_COMPLETO, ISNULL(E.NM_ESTACAO,' ') AS NM_ESTACAO,PES.RAZ_SOCIAL,PES.CD_PESSOA,TP.DS_BLOQUEIO AS DS_BLOQUEIO " +
                                "FROM " +
                                "LIBERACAO_DO_DOCUMENTO AS LIB " +
                                "INNER JOIN PESSOA_DO_DOCUMENTO AS PES ON PES.CD_DOCUMENTO = LIB.CD_DOCUMENTO AND PES.TP_PESSOA = 12 " +
                                "LEFT JOIN USUARIO AS U ON U.CD_USUARIO = LIB.CD_USUARIO " +
                                "LEFT JOIN HABIL_ESTACAO AS E ON E.CD_ESTACAO = LIB.CD_MAQUINA " +
                                "INNER JOIN HABIL_TIPO_BLOQUEIO AS TP ON TP.CD_BLOQUEIO = LIB.CD_BLOQUEIO";
                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);
                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                List<LiberacaoDocumento> lista = new List<LiberacaoDocumento>();

                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {


                        LiberacaoDocumento p = new LiberacaoDocumento();
                        p.CodigoLiberacao = Convert.ToInt32(Dr["CD_INDEX"]);
                        p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                        p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                        p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                        p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                        if (Dr["DT_LIBERACAO"] != DBNull.Value)
                            p.DataLiberacao = Convert.ToDateTime(Dr["DT_LIBERACAO"]);
                        p.DataLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                        p.Cpl_NomeMaquina = Dr["NM_ESTACAO"].ToString();
                        p.Cpl_NomeUsuario = Dr["NM_COMPLETO"].ToString();
                        p.Cpl_DescricaoBloqueio = Dr["DS_BLOQUEIO"].ToString();
                        lista.Add(p);
                    }

                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Liberaçãoes: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<LiberacaoDocumentoGrid> ListarLiberacoesGrid(List<DBTabelaCampos> ListaFiltros, int cd_Empresa, int cd_Bloqueio)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "select * from VW_LIBERACAO_DO_PEDIDO where CD_EMPRESA = @v1 and CD_BLOQUEIO = @v2 and DT_LIBERACAO IS NULL;";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", cd_Empresa);
                Cmd.Parameters.AddWithValue("@v2", cd_Bloqueio);

                Dr = Cmd.ExecuteReader();

                List<LiberacaoDocumentoGrid> lista = new List<LiberacaoDocumentoGrid>();

                while (Dr.Read())
                {
                    LiberacaoDocumentoGrid p = new LiberacaoDocumentoGrid();

                    p.CodigoLiberacao = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoOrcamento = Convert.ToInt32(Dr["NR_ORCAMENTO"]);
                    p.CodigoPedido = Convert.ToInt32(Dr["NR_PEDIDO"]);
                    p.Valor = Convert.ToInt32(Dr["VL_TOTAL_GERAL"]);
                    p.DataLancamento = Convert.ToDateTime(Dr["DT_HR_EMISSAO"]);
                    p.Oper1 = Convert.ToString(Dr["OPER1"]);
                    p.Oper2 = Convert.ToString(Dr["OPER2"]);
                    p.Oper3 = Convert.ToString(Dr["OPER3"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(Dr["NM_CLIENTE"])))
                    {
                        p.CodigoCliente = Convert.ToInt32(Dr["CD_CLIENTE"]);
                        p.NomeCliente = Convert.ToString(Dr["NM_CLIENTE"]);

                        PessoaDAL pessoaDAL = new PessoaDAL();

                        decimal decEmPedidos = 0; //fazer busca nos pedidos

                        decimal decLimiteCredito = 0;

                        decimal decCreditoUsado = pessoaDAL.VerificaCreditoUsadoCliente(p.CodigoCliente, ref decEmPedidos, ref decLimiteCredito);

                        p.ValorAberto = decCreditoUsado + decEmPedidos;
                        p.LimiteCredito = decLimiteCredito;
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(Dr["NM_USUARIO"])))
                        p.NomeUsuario = Convert.ToString(Dr["NM_USUARIO"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(Dr["CD_BLOQUEIO"])) && Convert.ToInt32(Dr["CD_BLOQUEIO"]) > 0)
                    {
                        //p.LiberacaoStatus.CodigoStatus = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                    }

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Liberações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public class Bloqueio
        {
            public int CodigoBloqueio { get; private set; }
            public string DescricaoBloqueio { get; private set; }


            public Bloqueio ObterBloqueio(int i)
            {
                List<Bloqueio> bloqueios = new List<Bloqueio>();

                bloqueios.Add(new Bloqueio { CodigoBloqueio = 1, DescricaoBloqueio = "Liberado" });
                bloqueios.Add(new Bloqueio { CodigoBloqueio = 2, DescricaoBloqueio = "Pendente" });
                bloqueios.Add(new Bloqueio { CodigoBloqueio = 3, DescricaoBloqueio = "Bloquado" });
                bloqueios.Add(new Bloqueio { CodigoBloqueio = 4, DescricaoBloqueio = "Em LIberação" });

                var ret = bloqueios.Where(x => x.CodigoBloqueio.Equals(i)).FirstOrDefault();

                return ret;
            }
        }

        //
        public void LiberarDocumento(LiberacaoDocumento p)
        {



            AbrirConexao();

            SqlCommand cmd1 = new SqlCommand();
            SqlCommand cmd2 = new SqlCommand();
            SqlCommand cmd3 = new SqlCommand();


            SqlTransaction transaction = Con.BeginTransaction();

            cmd1 = new SqlCommand("update LIBERACAO_DO_DOCUMENTO set DT_LIBERACAO = @v3, CD_USUARIO = @v4, CD_MAQUINA = @v5 " +
                                  "where CD_DOCUMENTO = @v1 and CD_BLOQUEIO = @v2;", Con, transaction);
            cmd1.Parameters.AddWithValue("@v1", p.CodigoDocumento);
            cmd1.Parameters.AddWithValue("@v2", p.CodigoBloqueio);
            cmd1.Parameters.AddWithValue("@v3", p.DataLiberacao);
            cmd1.Parameters.AddWithValue("@v4", p.CodigoUsuario);
            cmd1.Parameters.AddWithValue("@v5", p.CodigoMaquina);

            try
            {
                cmd1.ExecuteNonQuery();

                if (!ExisteBoqueio(p.CodigoDocumento, Con, transaction))
                {
                    cmd2 = new SqlCommand("update DOCUMENTO set CD_SITUACAO = 146 Where CD_DOCUMENTO = @v1;", Con, transaction);
                    cmd2.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                    cmd2.ExecuteNonQuery();

                    cmd3 = new SqlCommand("insert into EVENTO_DO_DOCUMENTO(CD_DOCUMENTO,CD_EVENTO,CD_SITUACAO,DT_HR_EVENTO,CD_MAQUINA,CD_USUARIO) " +
                                          "values(@v1,(select max(CD_EVENTO) + 1 from EVENTO_DO_DOCUMENTO where CD_DOCUMENTO = @v1),146,@v3,@v4,@v5)", Con, transaction);
                    cmd3.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                    cmd3.Parameters.AddWithValue("@v3", p.DataLiberacao);
                    cmd3.Parameters.AddWithValue("@v4", p.CodigoMaquina);
                    cmd3.Parameters.AddWithValue("@v5", p.CodigoUsuario);

                    cmd3.ExecuteNonQuery();

                }

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();

                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            throw new InvalidOperationException("Alteração do Documento não permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao alterar documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao alterar documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        private bool ExisteBoqueio(decimal decCodDocumento, SqlConnection con, SqlTransaction transaction)
        {
            Cmd = new SqlCommand();
            Cmd.CommandType = System.Data.CommandType.Text;
            Cmd.CommandText = "select count(0) from LIBERACAO_DO_DOCUMENTO where CD_DOCUMENTO = @v1 and DT_LIBERACAO is null";
            Cmd.Connection = con;
            Cmd.Parameters.AddWithValue("@v1", decCodDocumento);
            Cmd.Transaction = transaction;
            int Quantidade = (int)Cmd.ExecuteScalar();


            if (Quantidade == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DevolverDocumentoPedido(LiberacaoDocumento p)
        {
            AbrirConexao();

            SqlCommand cmd1 = new SqlCommand();
            SqlCommand cmd2 = new SqlCommand();
            SqlCommand cmd3 = new SqlCommand();

            SqlTransaction transaction = Con.BeginTransaction();

            cmd1 = new SqlCommand("delete from LIBERACAO_DO_DOCUMENTO where CD_DOCUMENTO = @v1;", Con, transaction);
            cmd1.Parameters.AddWithValue("@v1", p.CodigoDocumento);

            cmd2 = new SqlCommand("update DOCUMENTO set CD_SITUACAO = 136 Where CD_DOCUMENTO = @v1;", Con, transaction);
            cmd2.Parameters.AddWithValue("@v1", p.CodigoDocumento);

            cmd3 = new SqlCommand("insert into EVENTO_DO_DOCUMENTO(CD_DOCUMENTO,CD_EVENTO,CD_SITUACAO,DT_HR_EVENTO,CD_MAQUINA,CD_USUARIO) " +
                                          "values(@v1,(select max(CD_EVENTO) + 1 from EVENTO_DO_DOCUMENTO where CD_DOCUMENTO = @v1),136,@v3,@v4,@v5)", Con, transaction);
            cmd3.Parameters.AddWithValue("@v1", p.CodigoDocumento);
            cmd3.Parameters.AddWithValue("@v3", p.DataLiberacao);
            cmd3.Parameters.AddWithValue("@v4", p.CodigoMaquina);
            cmd3.Parameters.AddWithValue("@v5", p.CodigoUsuario);

            try
            {
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (SqlException ex)
            {
                transaction.Rollback();

                if (ex.Errors.Count > 0)
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547:
                            throw new InvalidOperationException("Devolução do Documento não permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao devolver Documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao devolver Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }


        public List<LiberacaoDocumentoGrid> ListarLiberacoesSolCompraGrid(List<DBTabelaCampos> ListaFiltros, int cd_Empresa, int cd_Bloqueio)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "select * from [VW_LIBERACAO_SOL_COMPRA] where CD_EMPRESA = @v1 and CD_BLOQUEIO = @v2 and DT_LIBERACAO IS NULL;";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", cd_Empresa);
                Cmd.Parameters.AddWithValue("@v2", cd_Bloqueio);

                Dr = Cmd.ExecuteReader();

                List<LiberacaoDocumentoGrid> lista = new List<LiberacaoDocumentoGrid>();

                while (Dr.Read())
                {
                    LiberacaoDocumentoGrid p = new LiberacaoDocumentoGrid();

                    p.Oper1 = "0";

                    p.CodigoIndice = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoSolicitacao = Convert.ToInt32(Dr["NR_DOCUMENTO"]);

                    if (Dr["NR_SR_DOCUMENTO"] != DBNull.Value)
                        p.CodigoSerieSolicitacao = Convert.ToInt32(Dr["NR_SR_DOCUMENTO"]);

                    p.ValorVerba = Convert.ToInt32(Dr["VL_TOTAL_GERAL"]);
                    p.DataEmissao = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);
                    p.DataValidade= Convert.ToDateTime(Dr["DT_VENCIMENTO"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(Dr["NM_FORNECEDOR"])))
                    {
                        p.CodigoFornecedor = Convert.ToInt32(Dr["CD_FORNECEDOR"]);
                        p.NomeFornecedor = Convert.ToString(Dr["NM_FORNECEDOR"]);
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(Dr["NM_USUARIO"])))
                        p.NomeUsuario = Convert.ToString(Dr["NM_USUARIO"]);

                    p.CodigoUsuario = Dr["CD_USUARIO"].ToString();

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Liberações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }



        public List<LiberacaoDocumento> ListarLiberacoesCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [LIBERACAO_DO_DOCUMENTO]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<LiberacaoDocumento> lista = new List<LiberacaoDocumento>();

                while (Dr.Read())
                {
                    LiberacaoDocumento p = new LiberacaoDocumento();

                    p.CodigoLiberacao = Convert.ToInt32(Dr["CD_INDEX"]);
                    p.CodigoDocumento = Convert.ToInt32(Dr["CD_DOCUMENTO"]);
                    p.CodigoBloqueio = Convert.ToInt32(Dr["CD_BLOQUEIO"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    //  p.DataDeLiberacao = Convert.ToDateTime(Dr["DT_LIBERACAO"]);
                    //  p.DataDeLancamento = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Liberações: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

            //}
            //public List<ItemDaLicenca> PesquisarItemLicenca(long Codigo)
            //{
            //    try
            //    {
            //        AbrirConexao();
            //        Cmd = new SqlCommand("Select * from LICENCA_DE_USO_ITEM Where CD_LICENCA = @v1 order by CD_ITEM_LICENCA_DE_USO DESC ", Con);
            //        Cmd.Parameters.AddWithValue("@v1", Codigo);
            //        Dr = Cmd.ExecuteReader();
            //        List<ItemDaLicenca> lista = new List<ItemDaLicenca>();

            //        while (Dr.Read())
            //        {
            //            ItemDaLicenca p = new ItemDaLicenca();
            //            p.CodigoDoItem = Convert.ToInt64(Dr["CD_ITEM_LICENCA_DE_USO"]);
            //            p.CodigoDaLicenca = Convert.ToInt64(Dr["CD_LICENCA"]);
            //            p.DataDeValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
            //            p.Guid = Convert.ToString(Dr["TX_GUID"]);
            //            p.ChaveDeAutenticacao = Convert.ToString(Dr["CH_AUTENTICACAO"]);
            //            lista.Add(p);
            //        }
            //        return lista;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Erro ao Pesquisar Licenças do Cliente: " + ex.Message.ToString());
            //    }
            //    finally
            //    {
            //        FecharConexao();
            //    }
            //}
            //public List<ModuloSistema> ListarModulosSistemaPelaLicenca(long Codigo)
            //{
            //    try
            //    {
            //        AbrirConexao();

            //        string strSQL = "Select MS.* ";
            //        strSQL = strSQL + ", ISNULL((Select Top 1 LMS.[CD_MODULO_DO_SISTEMA] FROM [LICENCA_DE_USO_POR_MODULO_DO_SISTEMA] AS LMS WHERE MS.[CD_MODULO_SISTEMA] = LMS.[CD_MODULO_DO_SISTEMA] AND LMS.[CD_LICENCA] = " + Codigo.ToString() + "),0) AS LIBERADO ";
            //        strSQL = strSQL + "from [MODULO_DO_SISTEMA] AS MS ";
            //        strSQL = strSQL + "Order By MS.[CD_MODULO_SISTEMA] ";

            //        Cmd = new SqlCommand(strSQL, Con);

            //        Dr = Cmd.ExecuteReader();

            //        List<ModuloSistema> lista = new List<ModuloSistema>();

            //        while (Dr.Read())
            //        {
            //            ModuloSistema p = new ModuloSistema();

            //            p.CodigoModulo = Convert.ToInt32(Dr["CD_MODULO_SISTEMA"]);
            //            p.DescricaoModulo = Convert.ToString(Dr["DS_MODULO_SISTEMA"]);
            //            if (Dr["LIBERADO"].ToString().Equals("0"))
            //                p.Liberar = false;
            //            else
            //                p.Liberar = true;

            //            lista.Add(p);
            //        }

            //        return lista;

            //    }
            //    catch (Exception ex)
            //    {

            //        throw new Exception("Erro ao Listar Todos Módulos: " + ex.Message.ToString());
            //    }
            //    finally
            //    {
            //        FecharConexao();

            //    }



            //}
            //public void SalvarLicenca(long CodLicenca, DateTime DataValidade, string strMarcados)
            //{
            //    ItemDaLicenca i = new ItemDaLicenca();

            //    //////////////////////////////////////////////////////////////////////
            //    i.CodigoDaLicenca = CodLicenca;
            //    i.DataDeValidade = DataValidade;
            //    i.Guid = "12334567890";
            //    i.ChaveDeAutenticacao = "0987654321";

            //    InserirLicenca(i, strMarcados);
            //    //////////////////////////////////////////////////////////////////////

            //    ModuloSistema m = new ModuloSistema();
            //    List<ModuloSistema> lista = new List<ModuloSistema>();

            //    Char delimiter = ',';
            //    string[] substrings = strMarcados.Split(delimiter);
            //    int numero = 0;
            //    foreach (var substring in substrings)
            //    {
            //        if (int.TryParse(substring, out numero))
            //        {
            //            m = new ModuloSistema();
            //            m.CodigoModulo = Convert.ToInt32(substring);
            //            m.Liberar = true;
            //            lista.Add(m);
            //        }
            //    }

            //    InserirLicencaModulos(i.CodigoDoItem, lista);
            //}
            //public void InserirLicenca(ItemDaLicenca p, string strMarcados)
            //{

            //    Guid g;
            //    // Create and display the value of two GUIDs.
            //    g = Guid.NewGuid();

            //    try
            //    {
            //        AbrirConexao();
            //        Cmd = new SqlCommand("insert into LICENCA_DE_USO_ITEM (CD_LICENCA, DT_VALIDADE, CH_AUTENTICACAO, TX_GUID) values (@v1,@v2,@v3,@v4); SELECT SCOPE_IDENTITY()", Con);
            //        Cmd.Parameters.AddWithValue("@v1", p.CodigoDaLicenca);
            //        Cmd.Parameters.AddWithValue("@v2", p.DataDeValidade);
            //        Cmd.Parameters.AddWithValue("@v4", g);
            //        clsHash clsh = new clsHash(SHA512.Create());
            //        string teste = clsh.CriptografarSenha(p.CodigoDaLicenca.ToString() + g + strMarcados);
            //        Cmd.Parameters.AddWithValue("@v3", teste);
            //        //                Cmd.ExecuteNonQuery();
            //        p.CodigoDoItem = Convert.ToInt64(Cmd.ExecuteScalar());

            //    }
            //    catch (SqlException ex)
            //    {
            //        if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
            //        {
            //            switch (ex.Errors[0].Number)
            //            {
            //                case 2601: // Primary key violation
            //                    throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
            //                case 2627: // Primary key violation
            //                    throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
            //                default:
            //                    throw new Exception("Erro ao gravar Item da Licença: " + ex.Message.ToString());
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Erro ao gravar Licença: " + ex.Message.ToString());
            //    }
            //    finally
            //    {
            //        FecharConexao();
            //    }
            //}
            //public void InserirLicencaModulos(long CodItem, List<ModuloSistema> p)
            //{
            //    try
            //    {
            //        AbrirConexao();

            //        foreach (var elemento in p)
            //        {
            //            if (elemento.Liberar == true)
            //            {
            //                Cmd = new SqlCommand("insert into LICENCA_DE_USO_POR_MODULO_DO_SISTEMA (CD_LICENCA, CD_MODULO_DO_SISTEMA) values (@v1,@v2)", Con);
            //                Cmd.Parameters.AddWithValue("@v1", CodItem);
            //                Cmd.Parameters.AddWithValue("@v2", elemento.CodigoModulo);
            //                Cmd.ExecuteNonQuery();
            //            }
            //        }
            //    }
            //    catch (SqlException ex)
            //    {
            //        if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
            //        {
            //            switch (ex.Errors[0].Number)
            //            {
            //                case 2601: // Primary key violation
            //                    throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
            //                case 2627: // Primary key violation
            //                    throw new DuplicateNameException("Inclusão não Permitida!!! Chave já consta no Banco de Dados. Mensagem :" + ex.Message.ToString(), ex);
            //                default:
            //                    throw new Exception("Erro ao gravar Modulos da Licença: " + ex.Message.ToString());
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Erro ao gravar Licença: " + ex.Message.ToString());
            //    }
            //    finally
            //    {
            //        FecharConexao();
            //    }
            //}
            //public ItemDaLicenca PesquisarUltimoItemLicenca(long Codigo)
            //{
            //    try
            //    {
            //        AbrirConexao();
            //        Cmd = new SqlCommand("Select Top 1  * from LICENCA_DE_USO_ITEM Where CD_LICENCA = @v1 order by CD_ITEM_LICENCA_DE_USO DESC ", Con);
            //        Cmd.Parameters.AddWithValue("@v1", Codigo);
            //        Dr = Cmd.ExecuteReader();
            //        ItemDaLicenca p = new ItemDaLicenca();

            //        if (Dr.Read())
            //        {
            //            p.CodigoDoItem = Convert.ToInt64(Dr["CD_ITEM_LICENCA_DE_USO"]);
            //            p.CodigoDaLicenca = Convert.ToInt64(Dr["CD_LICENCA"]);
            //            p.DataDeValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
            //            p.Guid = Convert.ToString(Dr["TX_GUID"]);
            //            p.ChaveDeAutenticacao = Convert.ToString(Dr["CH_AUTENTICACAO"]);
            //        }
            //        return p;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Erro ao Pesquisar Item da Licenças do Cliente: " + ex.Message.ToString());
            //    }
            //    finally
            //    {
            //        FecharConexao();
            //    }
            //}

            //public bool LicencaEhValida(out string strLicensa, out DateTime dteValidade)
            //{
            //    bool blnValidaLicenca = false;
            //    Licenca l = new Licenca();
            //    List<ItemDaLicenca> lista = new List<ItemDaLicenca>();
            //    List<ModuloSistema> listaMS = new List<ModuloSistema>();
            //    clsHash clsh = new clsHash(SHA512.Create());
            //    string strChaveAutenticada = "";
            //    string strMarcados = "";
            //    dteValidade = DateTime.Today;
            //    strLicensa = "";

            //    l = PesquisarLicenca(0);

            //    if (l != null)
            //    {
            //        strLicensa = l.CodigoDoCliente.ToString() + " - " + l.NomeDoCliente;

            //        lista = PesquisarItemLicenca(l.CodigoDaLicenca);

            //        foreach (var itm in lista)
            //        {
            //            if (itm.DataDeValidade >= Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy 00:00:00")))
            //            {

            //                listaMS = ListarModulosSistemaPelaLicenca(itm.CodigoDoItem);

            //                foreach (ModuloSistema itm2 in listaMS)
            //                {
            //                    if (itm2.Liberar == true)
            //                    {
            //                        if (strMarcados.Equals(""))
            //                            strMarcados = itm2.CodigoModulo.ToString();
            //                        else
            //                            strMarcados = strMarcados + ", " + itm2.CodigoModulo.ToString();
            //                    }
            //                }
            //                strChaveAutenticada = clsh.CriptografarSenha(l.CodigoDaLicenca.ToString() + itm.Guid.ToLower() + strMarcados);
            //                if (strChaveAutenticada == itm.ChaveDeAutenticacao)
            //                {
            //                    dteValidade = itm.DataDeValidade;
            //                    blnValidaLicenca = true;
            //                }
            //            }
            //        }
            //    }

            //    return blnValidaLicenca;
            //}
            //public void AtualizarLicencaBanco(long lngCod, long lngBanco)
            //{
            //    try
            //    {
            //        AbrirConexao();

            //        Cmd = new SqlCommand("update LICENCA_DE_USO set CD_STR_DATABASE = @v8 Where CD_LICENCA = @v3", Con);
            //        Cmd.Parameters.AddWithValue("@v3", lngCod);
            //        Cmd.Parameters.AddWithValue("@v8", lngBanco);
            //        Cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Erro ao atualizar Licença Banco: " + ex.Message.ToString());
            //    }
            //    finally
            //    {
            //        FecharConexao();
            //    }
            //}

        }

        public void CancelarSolicitacaoCompra(LiberacaoDocumento LibDocumento, string strMotivo)
        {


            try
            {
                AbrirConexao();

                SqlCommand cmd1 = new SqlCommand();


                cmd1 = new SqlCommand("delete from LIBERACAO_DO_DOCUMENTO where CD_INDEX = @v1;", Con);
                cmd1.Parameters.AddWithValue("@v1", LibDocumento.CodigoDocumento);
                cmd1.ExecuteNonQuery();

                //Monta Tela
                List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
                List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
                List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
                ProdutoDocumentoDAL RnPd = new ProdutoDocumentoDAL();
                Doc_SolCompra p = new Doc_SolCompra();
                Doc_SolCompraDAL docDAL = new Doc_SolCompraDAL();
                EventoDocumentoDAL eve = new EventoDocumentoDAL();
                AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();

                p = docDAL.PesquisarSolCompra(LibDocumento.CodigoDocumento);
                ListaProdutos = RnPd.ObterItemSolCompra(p.CodigoDocumento);
                ListaProdutos = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList(); ;
                ListaEvento = eve.ObterEventos(LibDocumento.CodigoDocumento);
                ListaAnexo = anexo.ObterAnexos(LibDocumento.CodigoDocumento);

                //Gravar Documento

                p.Cpl_Maquina = LibDocumento.CodigoMaquina;
                p.Cpl_Usuario = LibDocumento.CodigoUsuario;

                p.CodigoSituacao = 203;
                p.MotivoCancelamento = strMotivo; 
                docDAL.SalvarSolicitacao(p, ListaProdutos, EventoDocumento(ListaEvento, p.CodigoSituacao, p.Cpl_Usuario , p.Cpl_Maquina), ListaAnexo);





            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0)
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547:
                            throw new InvalidOperationException("Devolução do Documento não permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao CancelarSolicitacaoCompra: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao CancelarSolicitacaoCompra: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void AprovarSolicitacaoCompra(LiberacaoDocumento p)
        {

            

            AbrirConexao();

            SqlCommand cmd1 = new SqlCommand();
            SqlCommand cmd2 = new SqlCommand();
            SqlCommand cmd3 = new SqlCommand();


            SqlTransaction transaction = Con.BeginTransaction();

            cmd1 = new SqlCommand("update LIBERACAO_DO_DOCUMENTO set DT_LIBERACAO = @v3, CD_USUARIO = @v4, CD_MAQUINA = @v5 " +
                                  "where CD_INDEX = @v1 ", Con, transaction);
            cmd1.Parameters.AddWithValue("@v1", p.CodigoLiberacao);
            cmd1.Parameters.AddWithValue("@v3", p.DataLiberacao);
            cmd1.Parameters.AddWithValue("@v4", p.CodigoUsuario);
            cmd1.Parameters.AddWithValue("@v5", p.CodigoMaquina);

            try
            {
                cmd1.ExecuteNonQuery();

                //Monta Tela
                List<EventoDocumento> ListaEvento = new List<EventoDocumento>();
                List<AnexoDocumento> ListaAnexo = new List<AnexoDocumento>();
                List<ProdutoDocumento> ListaProdutos = new List<ProdutoDocumento>();
                ProdutoDocumentoDAL RnPd = new ProdutoDocumentoDAL();
                Doc_SolCompra p1 = new Doc_SolCompra();
                Doc_SolCompraDAL docDAL = new Doc_SolCompraDAL();
                EventoDocumentoDAL eve = new EventoDocumentoDAL();
                AnexoDocumentoDAL anexo = new AnexoDocumentoDAL();

                p1 = docDAL.PesquisarSolCompra(p.CodigoDocumento);
                ListaProdutos = RnPd.ObterItemSolCompra(p.CodigoDocumento);
                ListaProdutos = ListaProdutos.Where(x => x.CodigoSituacao != 134).ToList();
                ListaEvento = eve.ObterEventos(p.CodigoDocumento);
                ListaAnexo = anexo.ObterAnexos(p.CodigoDocumento);

                //Gravar Documento
                p1.Cpl_Maquina = p.CodigoMaquina;
                p1.Cpl_Usuario = p.CodigoUsuario;

                p1.CodigoSituacao = 202;
                p1.MotivoCancelamento = "";
                docDAL.SalvarSolicitacao(p1, ListaProdutos, EventoDocumento(ListaEvento, p1.CodigoSituacao, p1.Cpl_Usuario, p1.Cpl_Maquina), ListaAnexo);

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();

                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            throw new InvalidOperationException("Alteração do Documento não permitida!!! Existe Relacionamentos Obrigatórios com a Tabela. Mensagem :" + ex.Message.ToString(), ex);
                        default:
                            throw new Exception("Erro ao alterar documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao alterar documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        protected EventoDocumento EventoDocumentoCancelamento(List<EventoDocumento> ListaEvento, LiberacaoDocumento p)
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime DataHoraEvento = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm"));

            int intCttItem = 0;

            if (ListaEvento.Count != 0)
                intCttItem = Convert.ToInt32(ListaEvento.Max(x => x.CodigoEvento).ToString());

            intCttItem = intCttItem + 1;

            if (intCttItem != 0)
                ListaEvento.RemoveAll(x => x.CodigoEvento == intCttItem);

            EventoDocumento evento = new EventoDocumento(intCttItem,
                                                       999, //situacao
                                                       DataHoraEvento,
                                                       p.CodigoMaquina,
                                                       p.CodigoUsuario);
            return evento;
        }

        protected EventoDocumento EventoDocumento(List<EventoDocumento> ListaEvento, int CodSituacao, int CodUsuario, int intMaquina)
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime DataHoraEvento = Convert.ToDateTime(RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm"));

            int intCttItem = 0;
            if (ListaEvento.Count > 0)
                intCttItem = Convert.ToInt32(ListaEvento.Max(x => x.CodigoEvento).ToString());

            intCttItem = intCttItem + 1;
            if (intCttItem != 0)
                ListaEvento.RemoveAll(x => x.CodigoEvento == intCttItem);

            EventoDocumento evento = new EventoDocumento(intCttItem, CodSituacao, DataHoraEvento, intMaquina, CodUsuario);
            return evento;
        }
    }
}
