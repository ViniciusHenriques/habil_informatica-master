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
    public class TipoDocumentoDAL:Conexao
    {
        protected string strSQL = "";
        public void Inserir(TipoDocumento p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [TIPO_DE_DOCUMENTO] (DS_TIPO_DOCUMENTO, [TP_CAMPO], NM_TABELA , [INC_POR_EMPRESA], [ABRE_SERIE], [CD_SITUACAO]) values (@v2, @v3,@v4,@v5,@v6,@v7)";

                Cmd = new SqlCommand(strSQL, Con);


                Cmd.Parameters.AddWithValue("@v2", p.DescricaoTipoDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.TipoDeCampo);
                Cmd.Parameters.AddWithValue("@v4", "");
                Cmd.Parameters.AddWithValue("@v5", p.IncrementalPorEmpresa);
                Cmd.Parameters.AddWithValue("@v6", p.AberturaDeSerie);
                Cmd.Parameters.AddWithValue("@v7", p.CodigoSituacao);
                

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
                            throw new Exception("Erro ao Incluir Tipo de Documentos: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Tipo de Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void Atualizar(TipoDocumento p)
        {
            try
            {
                AbrirConexao();

                string strSQL1 = "";

                if ((p.CodigoSituacao == 1) && (p.TipoDeCampo == 10))
                {
                    strSQL1 = " , NM_TABELA = '";
                    strSQL1 += "INS_DOC_" + p.CodigoTipoDocumento.ToString() + "_";

                    if (p.IncrementalPorEmpresa == 7)
                    {
                        if (p.AberturaDeSerie == 7)
                            strSQL1 += "EMPRESA_SERIE";
                        else
                            strSQL1 += "EMPRESA";
                    }
                    else
                    {
                        if (p.AberturaDeSerie == 7)
                            strSQL1 += "SERIE";
                        else
                            strSQL1 += "SEQ";
                    }

                    strSQL1 += "'";
                }

                strSQL = "update [TIPO_DE_DOCUMENTO] " +
                         "   set [DS_TIPO_DOCUMENTO] = @v2 " + strSQL1 +
                         "   , [INC_POR_EMPRESA] = @v3, [ABRE_SERIE] = @v4, CD_SITUACAO = @v5, TP_CAMPO = @v6  " +
                         " Where [CD_TIPO_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoTipoDocumento);
                Cmd.Parameters.AddWithValue("@v2", p.DescricaoTipoDocumento);
                Cmd.Parameters.AddWithValue("@v3", p.IncrementalPorEmpresa);
                Cmd.Parameters.AddWithValue("@v4", p.AberturaDeSerie);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v6", p.TipoDeCampo);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Tipo de Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public void InativaTipoDocumento(int CodigoTipoDocumento)
        {
            try
            {
                AbrirConexao();

                string strSQL1 = "";

                strSQL = "update [TIPO_DE_DOCUMENTO] " +
                         "   set [CD_SITUACAO] = 2 " +
                         " Where [CD_TIPO_DOCUMENTO] = @v1";

                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", CodigoTipoDocumento);
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Tipo de Documento: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from [TIPO_DE_DOCUMENTO] Where [CD_TIPO_DOCUMENTO] = @v1", Con);
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
                            throw new Exception("Erro ao excluir Tipo de Documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir Tipo de Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }
        public TipoDocumento PesquisarTipoDocumento(int Codigo)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [TIPO_DE_DOCUMENTO] Where CD_TIPO_DOCUMENTO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);

                Dr = Cmd.ExecuteReader();

                TipoDocumento p = null;

                if (Dr.Read())
                {
                    p = new TipoDocumento();

                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);
                    p.DescricaoTipoDocumento = Convert.ToString(Dr["DS_TIPO_DOCUMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.TipoDeCampo = Convert.ToInt32(Dr["TP_CAMPO"]);
                    p.IncrementalPorEmpresa = Convert.ToInt32(Dr["INC_POR_EMPRESA"]);
                    p.AberturaDeSerie= Convert.ToInt32(Dr["ABRE_SERIE"]);
                    p.NomeDaTabela = Convert.ToString(Dr["NM_TABELA"]);
                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Tipo de Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<TipoDocumento> ListarTipoDocumento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from [TIPO_DE_DOCUMENTO]";

                if (strValor != "")
                {
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);
                    strSQL = strSQL + " and CD_SITUACAO > 0 ";
                }
                else
                    strSQL = strSQL + " where CD_SITUACAO > 0 ";


                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<TipoDocumento> lista = new List<TipoDocumento>();

                while (Dr.Read())
                {
                    TipoDocumento p = new TipoDocumento();

                    p.CodigoTipoDocumento = Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]);
                    p.DescricaoTipoDocumento = Convert.ToString(Dr["DS_TIPO_DOCUMENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.TipoDeCampo = Convert.ToInt32(Dr["TP_CAMPO"]);
                    p.IncrementalPorEmpresa = Convert.ToInt32(Dr["INC_POR_EMPRESA"]);
                    p.AberturaDeSerie = Convert.ToInt32(Dr["ABRE_SERIE"]);
                    p.NomeDaTabela = Convert.ToString(Dr["NM_TABELA"]);
                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Tipos de Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }
        public DataTable ObterTiposDocumento(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                AbrirConexao();



                string strSQL = "Select * from [TIPO_DE_DOCUMENTO]";

                if (strValor != "")
                {
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);
                    strSQL = strSQL + " and CD_SITUACAO > 0 ";
                }
                else
                    strSQL = strSQL + " where CD_SITUACAO > 0 ";

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoTipoDocumento", typeof(Int64));
                dt.Columns.Add("DescricaoTipoDocumento", typeof(string));
                dt.Columns.Add("CodigoSituacao", typeof(Int64));
                dt.Columns.Add("IncrementalPorEmpresa", typeof(Int64));
                dt.Columns.Add("TipoDeCampo", typeof(Int64));
                dt.Columns.Add("AberturaDeSerie", typeof(Int64));

                while (Dr.Read())
                    dt.Rows.Add(Convert.ToInt32(Dr["CD_TIPO_DOCUMENTO"]),
                        Convert.ToString(Dr["DS_TIPO_DOCUMENTO"]),
                        Convert.ToInt32(Dr["CD_SITUACAO"]),
                        Convert.ToInt32(Dr["INC_POR_EMPRESA"]),
                        Convert.ToInt32(Dr["TP_CAMPO"]),
                        Convert.ToInt32(Dr["ABRE_SERIE"]),
                        Convert.ToString(Dr["NM_TABELA"]));

                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Todos Tipos de Documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void LiberaTipoDocumento(int CodModuloSistema)
        {
            try
            {
                switch (CodModuloSistema)
                {

                    case 4: //Financeiro
                        InativaTipoDocumento(3);
                        InativaTipoDocumento(4);
                        break;

                    case 5://Serviço
                        InativaTipoDocumento(1);
                        InativaTipoDocumento(2);
                        InativaTipoDocumento(6);
                        break;

                    case 6://Vendas
                        InativaTipoDocumento(5);
                        InativaTipoDocumento(8);
                        break;

                    case 7://Transporte
                        InativaTipoDocumento(7);
                        break;

                    case 14://Compras
                        InativaTipoDocumento(11);
                        InativaTipoDocumento(12);
                        InativaTipoDocumento(13);
                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar Tipo de Documento: " + ex.Message.ToString());
            }
            finally
            {
                switch (CodModuloSistema)
                {

                    case 4: //Financeiro
                        FecharConexao();
                        break;

                    case 5://Serviço
                        FecharConexao();
                        break;

                    case 6://Vendas
                        FecharConexao();
                        break;

                    case 7://Transporte
                        FecharConexao();
                        break;

                    case 14://Transporte
                        FecharConexao();
                        break;

                    default:
                        break;
                }

            }


        }
    }
}
