using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;
using System.Security.Cryptography;

namespace DAL.Persistence
{
    public class LicencaDAL : Conexao
    {
        public void Inserir(Licenca p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("insert into LICENCA_DE_USO (CD_CLIENTE, NM_CLIENTE, NR_USUARIOS, TX_SERVIDOR, TX_BANCO, TX_USUARIO, TX_SENHA, CD_STR_DATABASE) values (@v3, @v1,@v2,@v4,@v5,@v6,@v7,0); SELECT SCOPE_IDENTITY()", Con);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoDoCliente);
                Cmd.Parameters.AddWithValue("@v1", p.NomeDoCliente);
                Cmd.Parameters.AddWithValue("@v2", p.NumeroDeUsuarios);
                Cmd.Parameters.AddWithValue("@v4", p.ServidorDoCliente);
                Cmd.Parameters.AddWithValue("@v5", p.BancoDoCliente);
                Cmd.Parameters.AddWithValue("@v6", p.UsuarioBancoDoCliente);
                Cmd.Parameters.AddWithValue("@v7", p.SenhaBancoDoCliente);
                //                Cmd.ExecuteNonQuery();
                p.CodigoDaLicenca = Convert.ToInt64(Cmd.ExecuteScalar());
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
                            throw new Exception("Erro ao gravar Licença: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Licença: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Atualizar(Licenca p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update LICENCA_DE_USO set NM_CLIENTE = @v1, NR_USUARIOS = @v2, CD_CLIENTE = @v4, TX_SERVIDOR = @v5, TX_BANCO = @v6, TX_USUARIO = @v7, TX_SENHA = @v8 Where CD_LICENCA = @v3", Con);
                Cmd.Parameters.AddWithValue("@v1", p.NomeDoCliente);
                Cmd.Parameters.AddWithValue("@v2", p.NumeroDeUsuarios);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoDoCliente);
                Cmd.Parameters.AddWithValue("@v3", p.CodigoDaLicenca);

                Cmd.Parameters.AddWithValue("@v5", p.ServidorDoCliente);
                Cmd.Parameters.AddWithValue("@v6", p.BancoDoCliente);
                Cmd.Parameters.AddWithValue("@v7", p.UsuarioBancoDoCliente);
                Cmd.Parameters.AddWithValue("@v8", p.SenhaBancoDoCliente);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Licença: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from LICENCA_DE_USO Where CD_LICENCA = @v1", Con);
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
                            throw new Exception("Erro ao excluir Licença: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Licença: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Licenca PesquisarLicenca(long Codigo)
        {
            try
            {
                AbrirConexao();
                if (Codigo == 0)
                {
                    Cmd = new SqlCommand("Select Top 1 * from LICENCA_DE_USO ", Con);
                }
                else
                {
                    Cmd = new SqlCommand("Select * from LICENCA_DE_USO Where CD_LICENCA = @v1", Con);
                    Cmd.Parameters.AddWithValue("@v1", Codigo);
                }

                Dr = Cmd.ExecuteReader();
                Licenca p = null;
                if (Dr.Read())
                {
                    p = new Licenca();
                    p.CodigoDaLicenca = Convert.ToInt64(Dr["CD_LICENCA"]);
                    p.CodigoDoCliente = Convert.ToInt64(Dr["CD_CLIENTE"]);
                    p.NomeDoCliente = Convert.ToString(Dr["NM_CLIENTE"]);
                    p.NumeroDeUsuarios = Convert.ToInt32(Dr["NR_USUARIOS"]);

                    p.ServidorDoCliente = Convert.ToString(Dr["TX_SERVIDOR"]);
                    p.BancoDoCliente = Convert.ToString(Dr["TX_BANCO"]);
                    p.UsuarioBancoDoCliente = Convert.ToString(Dr["TX_USUARIO"]);
                    p.SenhaBancoDoCliente = Convert.ToString(Dr["TX_SENHA"]);

                    p.CodigoDaAtualizacaoBanco = Convert.ToInt64(Dr["CD_STR_DATABASE"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Licença: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Licenca> ListarLicencas(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();
                string strSQL = "Select * from LICENCA_DE_USO ";
                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);
                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;
                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();
                List<Licenca> lista = new List<Licenca>();
                while (Dr.Read())
                {
                    Licenca p = new Licenca();
                    p.CodigoDaLicenca = Convert.ToInt64(Dr["CD_LICENCA"]);
                    p.CodigoDoCliente = Convert.ToInt64(Dr["CD_CLIENTE"]);
                    p.NomeDoCliente = Convert.ToString(Dr["NM_CLIENTE"]);
                    p.NumeroDeUsuarios = Convert.ToInt32(Dr["NR_USUARIOS"]);

                    p.ServidorDoCliente = Convert.ToString(Dr["TX_SERVIDOR"]);
                    p.BancoDoCliente = Convert.ToString(Dr["TX_BANCO"]);
                    p.UsuarioBancoDoCliente = Convert.ToString(Dr["TX_USUARIO"]);
                    p.SenhaBancoDoCliente = Convert.ToString(Dr["TX_SENHA"]);

                    p.CodigoDaAtualizacaoBanco = Convert.ToInt64(Dr["CD_STR_DATABASE"]);

                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todas Licenças: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Licenca> ListarLicencasCompleto(List<DBTabelaCampos> ListaFiltros)
        {
            try
            {
                AbrirConexao();
                string strValor = "";
                string strSQL = "Select * from [LICENCA_DE_USO]";

                strValor = MontaFiltroIntervalo(ListaFiltros);

                strSQL = strSQL + strValor;
                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Licenca> lista = new List<Licenca>();

                while (Dr.Read())
                {
                    Licenca p = new Licenca();
                    p.CodigoDaLicenca = Convert.ToInt64(Dr["CD_LICENCA"]);
                    p.CodigoDoCliente = Convert.ToInt64(Dr["CD_CLIENTE"]);
                    p.NomeDoCliente = Convert.ToString(Dr["NM_CLIENTE"]);
                    p.NumeroDeUsuarios = Convert.ToInt32(Dr["NR_USUARIOS"]);

                    p.ServidorDoCliente = Convert.ToString(Dr["TX_SERVIDOR"]);
                    p.BancoDoCliente = Convert.ToString(Dr["TX_BANCO"]);
                    p.UsuarioBancoDoCliente = Convert.ToString(Dr["TX_USUARIO"]);
                    p.SenhaBancoDoCliente = Convert.ToString(Dr["TX_SENHA"]);

                    p.CodigoDaAtualizacaoBanco = Convert.ToInt64(Dr["CD_STR_DATABASE"]);

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todas Licenças: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public List<ItemDaLicenca> PesquisarItemLicenca(long Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from LICENCA_DE_USO_ITEM Where CD_LICENCA = @v1 order by CD_ITEM_LICENCA_DE_USO DESC ", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                List<ItemDaLicenca> lista = new List<ItemDaLicenca>();

                while (Dr.Read())
                {
                    ItemDaLicenca p = new ItemDaLicenca();
                    p.CodigoDoItem = Convert.ToInt64(Dr["CD_ITEM_LICENCA_DE_USO"]);
                    p.CodigoDaLicenca = Convert.ToInt64(Dr["CD_LICENCA"]);
                    p.DataDeValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.Guid = Convert.ToString(Dr["TX_GUID"]);
                    p.ChaveDeAutenticacao = Convert.ToString(Dr["CH_AUTENTICACAO"]);
                    lista.Add(p);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Licenças do Cliente: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<ModuloSistema> ListarModulosSistemaPelaLicenca(long Codigo)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select MS.* ";
                strSQL = strSQL + ", ISNULL((Select Top 1 LMS.[CD_MODULO_DO_SISTEMA] FROM [LICENCA_DE_USO_POR_MODULO_DO_SISTEMA] AS LMS WHERE MS.[CD_MODULO_SISTEMA] = LMS.[CD_MODULO_DO_SISTEMA] AND LMS.[CD_LICENCA] = " + Codigo.ToString() + "),0) AS LIBERADO ";
                strSQL = strSQL + "from [MODULO_DO_SISTEMA] AS MS ";
                strSQL = strSQL + "Order By MS.[CD_MODULO_SISTEMA] ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<ModuloSistema> lista = new List<ModuloSistema>();

                while (Dr.Read())
                {
                    ModuloSistema p = new ModuloSistema();

                    p.CodigoModulo = Convert.ToInt32(Dr["CD_MODULO_SISTEMA"]);
                    p.DescricaoModulo = Convert.ToString(Dr["DS_MODULO_SISTEMA"]);
                    if (Dr["LIBERADO"].ToString().Equals("0"))
                        p.Liberar = false;
                    else
                        p.Liberar = true;

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Módulos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public void SalvarLicenca(long CodLicenca, DateTime DataValidade, string strMarcados)
        {
            ItemDaLicenca i = new ItemDaLicenca();

            //////////////////////////////////////////////////////////////////////
            i.CodigoDaLicenca = CodLicenca;
            i.DataDeValidade = DataValidade;
            i.Guid = "12334567890";
            i.ChaveDeAutenticacao = "0987654321";

            InserirLicenca(i, strMarcados);
            //////////////////////////////////////////////////////////////////////

            ModuloSistema m = new ModuloSistema();
            List<ModuloSistema> lista = new List<ModuloSistema>();

            Char delimiter = ',';
            string[] substrings = strMarcados.Split(delimiter);
            int numero = 0;
            foreach (var substring in substrings)
            {
                if (int.TryParse(substring, out numero))
                {
                    m = new ModuloSistema();
                    m.CodigoModulo = Convert.ToInt32(substring);
                    m.Liberar = true;
                    lista.Add(m);
                }
            }

            InserirLicencaModulos(i.CodigoDoItem, lista);
        }
        public void InserirLicenca(ItemDaLicenca p, string strMarcados)
        {

            Guid g;
            // Create and display the value of two GUIDs.
            g = Guid.NewGuid();

            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("insert into LICENCA_DE_USO_ITEM (CD_LICENCA, DT_VALIDADE, CH_AUTENTICACAO, TX_GUID) values (@v1,@v2,@v3,@v4); SELECT SCOPE_IDENTITY()", Con);
                Cmd.Parameters.AddWithValue("@v1", p.CodigoDaLicenca);
                Cmd.Parameters.AddWithValue("@v2", p.DataDeValidade);
                Cmd.Parameters.AddWithValue("@v4", g);
                clsHash clsh = new clsHash(SHA512.Create());
                string teste = clsh.CriptografarSenha(p.CodigoDaLicenca.ToString() + g + strMarcados);
                Cmd.Parameters.AddWithValue("@v3", teste);
                //                Cmd.ExecuteNonQuery();
                p.CodigoDoItem = Convert.ToInt64(Cmd.ExecuteScalar());

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
                            throw new Exception("Erro ao gravar Item da Licença: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Licença: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void InserirLicencaModulos(long CodItem, List<ModuloSistema> p)
        {
            try
            {
                AbrirConexao();

                foreach (var elemento in p)
                {
                    if (elemento.Liberar == true)
                    {
                        Cmd = new SqlCommand("insert into LICENCA_DE_USO_POR_MODULO_DO_SISTEMA (CD_LICENCA, CD_MODULO_DO_SISTEMA) values (@v1,@v2)", Con);
                        Cmd.Parameters.AddWithValue("@v1", CodItem);
                        Cmd.Parameters.AddWithValue("@v2", elemento.CodigoModulo);
                        Cmd.ExecuteNonQuery();
                    }
                }
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
                            throw new Exception("Erro ao gravar Modulos da Licença: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar Licença: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public ItemDaLicenca PesquisarUltimoItemLicenca(long Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select Top 1  * from LICENCA_DE_USO_ITEM Where CD_LICENCA = @v1 order by CD_ITEM_LICENCA_DE_USO DESC ", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                ItemDaLicenca p = new ItemDaLicenca();

                if (Dr.Read())
                {
                    p.CodigoDoItem = Convert.ToInt64(Dr["CD_ITEM_LICENCA_DE_USO"]);
                    p.CodigoDaLicenca = Convert.ToInt64(Dr["CD_LICENCA"]);
                    p.DataDeValidade = Convert.ToDateTime(Dr["DT_VALIDADE"]);
                    p.Guid = Convert.ToString(Dr["TX_GUID"]);
                    p.ChaveDeAutenticacao = Convert.ToString(Dr["CH_AUTENTICACAO"]);
                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Item da Licenças do Cliente: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public bool LicencaEhValida(out string strLicensa, out DateTime dteValidade)
        {
            bool blnValidaLicenca = false;
            Licenca l = new Licenca();
            List<ItemDaLicenca> lista = new List<ItemDaLicenca>();
            List<ModuloSistema> listaMS = new List<ModuloSistema>();
            clsHash clsh = new clsHash(SHA512.Create());
            string strChaveAutenticada = "";
            string strMarcados = "";
            dteValidade = DateTime.Today;
            strLicensa = "";

            l = PesquisarLicenca(0);

            if (l != null)
            {
                strLicensa = l.CodigoDoCliente.ToString() + " - " + l.NomeDoCliente;

                lista = PesquisarItemLicenca(l.CodigoDaLicenca);

                foreach (var itm in lista)
                {
                    if (itm.DataDeValidade >= Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy 00:00:00")))
                    {

                        listaMS = ListarModulosSistemaPelaLicenca(itm.CodigoDoItem);

                        foreach (ModuloSistema itm2 in listaMS)
                        {
                            if (itm2.Liberar == true)
                            {
                                if (strMarcados.Equals(""))
                                    strMarcados = itm2.CodigoModulo.ToString();
                                else
                                    strMarcados = strMarcados + ", " + itm2.CodigoModulo.ToString();
                            }
                        }
                        strChaveAutenticada = clsh.CriptografarSenha(l.CodigoDaLicenca.ToString() + itm.Guid.ToLower() + strMarcados);
                        if (strChaveAutenticada == itm.ChaveDeAutenticacao)
                        {
                            dteValidade = itm.DataDeValidade;
                            blnValidaLicenca = true;
                        }
                    }
                }
            }

            return blnValidaLicenca;
        }
        public void AtualizarLicencaBanco(long lngCod, long lngBanco)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("update LICENCA_DE_USO set CD_STR_DATABASE = @v8 Where CD_LICENCA = @v3", Con);
                Cmd.Parameters.AddWithValue("@v3", lngCod);
                Cmd.Parameters.AddWithValue("@v8", lngBanco);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Licença Banco: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}