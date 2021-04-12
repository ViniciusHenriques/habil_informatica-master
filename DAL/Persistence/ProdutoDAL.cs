using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class ProdutoDAL : Conexao
    {
        public void Inserir(Produto  p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("insert into PRODUTO (NM_PRODUTO, " +
                                                            "CD_CATEGORIA, " +
                                                            " DT_CADASTRO," +
                                                            " DT_ATUALIZACAO," +
                                                            " VL_COMPRA," +
                                                            " PC_LUCRO," +
                                                            " VL_VENDA," +
                                                            " CD_UNIDADE," +
                                                            " CD_GPO_TRIB_PRODUTO," +
                                                            " CD_TIPO_PRODUTO," +
                                                            " CD_SIS_ANTERIOR," +
                                                            " CD_FABRICANTE," +
                                                            " CD_MARCA," +
                                                            " CD_SITUACAO," +
                                                            " IN_PRD_INVENTARIO," +
                                                            " IN_CTR_LOTE," +
                                                            " CD_CEST," +
                                                            " VL_VOLUME," +
                                                            " VL_PESO," +
                                                            " VL_FATOR_CUBAGEM," +
                                                            " DS_EMBALAGEM, " +
                                                            " QT_EMBALAGEM," +
                                                            " CD_BARRAS, " +
                                                            " CD_PRD_ASSOCIADO," +
                                                            " CD_PIS," +
                                                            " CD_COFINS," +
                                                            " CD_NCM," +
                                                            " CD_EX ," +
                                                            " TX_LNK_PRODUTO ," +
                                                            " IM_FOTO_PRINCIPAL ," +
                                                            " IM_FOTO1 ," +
                                                            " IM_FOTO2 ," +
                                                            " IM_FOTO3 ," +
                                                            " IM_FOTO4 ," +
                                                            " IM_FOTO5) values (@v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10, @v11, @v12, @v13, @v14, @v15, @v16, @v17, @v18, @v19, @v20, @v21, @v22,'', 0, @v25, @v26, @v27, @v28, @v29, @v30, @v31, @v32, @v33, @v34, @v35); SELECT SCOPE_IDENTITY()", Con);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoProduto.ToUpper());

                CategoriaDAL r = new CategoriaDAL();
                Categoria p1 = new Categoria();

                p1 = r.PesquisarCategoria(p.CodigoCategoria);
                if(p1 != null)
                    Cmd.Parameters.AddWithValue("@v2", p1.CodigoIndice);
                else
                    Cmd.Parameters.AddWithValue("@v2", 0);

                Cmd.Parameters.AddWithValue("@v3", p.DataCadastro);
                Cmd.Parameters.AddWithValue("@v4", p.DataAtualizacao);
                Cmd.Parameters.AddWithValue("@v5", p.ValorCompra);
                Cmd.Parameters.AddWithValue("@v6", p.PercentualLucro);
                Cmd.Parameters.AddWithValue("@v7", p.ValorVenda);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoUnidade);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoGpoTribProduto);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoTipoProduto);

                if (string.IsNullOrEmpty(p.CodigoSisAnterior))
                    Cmd.Parameters.AddWithValue("@v11", DBNull.Value);
                else
                    Cmd.Parameters.AddWithValue("@v11", p.CodigoSisAnterior);

                Cmd.Parameters.AddWithValue("@v12", p.CodigoFabricante);
                Cmd.Parameters.AddWithValue("@v13", p.CodigoMarca);                
                Cmd.Parameters.AddWithValue("@v14", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v15", p.ProdutoInventario);
                Cmd.Parameters.AddWithValue("@v16", p.ControlaLote);
                Cmd.Parameters.AddWithValue("@v17", p.CodigoIndexCEST);
                Cmd.Parameters.AddWithValue("@v18", p.ValorVolume);
                Cmd.Parameters.AddWithValue("@v19", p.ValorPeso);
                Cmd.Parameters.AddWithValue("@v20", p.ValorFatorCubagem);
                Cmd.Parameters.AddWithValue("@v21", p.DsEmbalagem);
                Cmd.Parameters.AddWithValue("@v22", p.QtEmbalagem);
                Cmd.Parameters.AddWithValue("@v25", p.CodigoPIS);
                Cmd.Parameters.AddWithValue("@v26", p.CodigoCOFINS);
                Cmd.Parameters.AddWithValue("@v27", p.CodigoNCM);
                Cmd.Parameters.AddWithValue("@v28", p.CodigoEX);
                Cmd.Parameters.AddWithValue("@v29", p.LinkProduto);
                if (p.FotoPrincipal != null)
                    Cmd.Parameters.AddWithValue("@v30", p.FotoPrincipal);
                else
                    Cmd.Parameters.AddWithValue("@v30", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto1 != null)
                    Cmd.Parameters.AddWithValue("@v31", p.Foto1);
                else
                    Cmd.Parameters.AddWithValue("@v31", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto2 != null)
                    Cmd.Parameters.AddWithValue("@v32", p.Foto2);
                else
                    Cmd.Parameters.AddWithValue("@v32", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto3 != null)
                    Cmd.Parameters.AddWithValue("@v33", p.Foto3);
                else
                    Cmd.Parameters.AddWithValue("@v33", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto4 != null)
                    Cmd.Parameters.AddWithValue("@v34", p.Foto4);
                else
                    Cmd.Parameters.AddWithValue("@v34", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto5 != null)
                    Cmd.Parameters.AddWithValue("@v35", p.Foto5);
                else
                    Cmd.Parameters.AddWithValue("@v35", System.Data.SqlTypes.SqlBinary.Null);

                p.CodigoProduto = Convert.ToInt64(Cmd.ExecuteScalar());


                if (p.CodigoPrdAssociado == 0)
                    p.CodigoPrdAssociado = p.CodigoProduto;

                if (p.CodigoBarras == "")
                    p.CodigoBarras = p.CodigoProduto.ToString();



                Atualizar(p);
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
                            throw new Exception("Erro ao gravar Produto: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception ("Erro ao gravar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void Atualizar(Produto p)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update Produto set NM_PRODUTO = @v1," +
                                                        " CD_CATEGORIA = @v2," +
                                                        " DT_CADASTRO = @v3," +
                                                        " DT_ATUALIZACAO = @v4," +
                                                        " VL_COMPRA = @v5," +
                                                        " PC_LUCRO = @v6," +
                                                        " VL_VENDA = @v7," +
                                                        " CD_UNIDADE = @v8," +
                                                        " CD_GPO_TRIB_PRODUTO = @v9," +
                                                        " CD_TIPO_PRODUTO=@v11," +
                                                        " CD_SIS_ANTERIOR = @v12," +
                                                        " CD_FABRICANTE = @v13, " +
                                                        " CD_MARCA = @v14," +
                                                        " CD_CEST = @v15, " +
                                                        " CD_SITUACAO = @v16," +
                                                        " IN_PRD_INVENTARIO = @v17," +
                                                        " IN_CTR_LOTE = @v18," +
                                                        " VL_VOLUME = @v19," +
                                                        " VL_PESO = @v20," +
                                                        " VL_FATOR_CUBAGEM = @v21, " +
                                                        " DS_EMBALAGEM = @v22, " +
                                                        " QT_EMBALAGEM = @v23, " +
                                                        " CD_PRD_ASSOCIADO = @v24, " +
                                                        " CD_BARRAS = @v25," +
                                                        " CD_PIS = @v26," +
                                                        " CD_COFINS = @v27," +
                                                        " CD_NCM = @v28," +
                                                        " CD_EX = @v29, " +
                                                        " TX_LNK_PRODUTO = @v30, " +
                                                        " IM_FOTO_PRINCIPAL = @v31, " +
                                                        " IM_FOTO1 = @v32, " +
                                                        " IM_FOTO2 = @v33, " +
                                                        " IM_FOTO3 = @v34, " +
                                                        " IM_FOTO4 = @v35, " +
                                                        " IM_FOTO5 = @v36 " +
                                                    "Where CD_Produto = @v10", Con);
                Cmd.Parameters.AddWithValue("@v10", p.CodigoProduto);
                Cmd.Parameters.AddWithValue("@v1", p.DescricaoProduto.ToUpper());

                CategoriaDAL r = new CategoriaDAL();
                Categoria p1 = new Categoria();
                if (p1 != null)
                    Cmd.Parameters.AddWithValue("@v2", p1.CodigoIndice);
                else
                    Cmd.Parameters.AddWithValue("@v2", 0);

                //Cmd.Parameters.AddWithValue("@v2", p1.CodigoIndice);

                Cmd.Parameters.AddWithValue("@v3", p.DataCadastro);
                Cmd.Parameters.AddWithValue("@v4", p.DataAtualizacao);
                Cmd.Parameters.AddWithValue("@v5", p.ValorCompra);
                Cmd.Parameters.AddWithValue("@v6", p.PercentualLucro);
                Cmd.Parameters.AddWithValue("@v7", p.ValorVenda);
                Cmd.Parameters.AddWithValue("@v8", p.CodigoUnidade);
                Cmd.Parameters.AddWithValue("@v9", p.CodigoGpoTribProduto);
                Cmd.Parameters.AddWithValue("@v11", p.CodigoTipoProduto);

                if (string.IsNullOrEmpty(p.CodigoSisAnterior))
                    Cmd.Parameters.AddWithValue("@v12", DBNull.Value);
                else
                    Cmd.Parameters.AddWithValue("@v12", p.CodigoSisAnterior);

                Cmd.Parameters.AddWithValue("@v13", p.CodigoFabricante);
                Cmd.Parameters.AddWithValue("@v14", p.CodigoMarca);
                Cmd.Parameters.AddWithValue("@v15", p.CodigoIndexCEST);
                Cmd.Parameters.AddWithValue("@v16", p.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v17", p.ProdutoInventario);
                Cmd.Parameters.AddWithValue("@v18", p.ControlaLote);
                Cmd.Parameters.AddWithValue("@v19", p.ValorVolume);
                Cmd.Parameters.AddWithValue("@v20", p.ValorPeso);
                Cmd.Parameters.AddWithValue("@v21", p.ValorFatorCubagem);
                Cmd.Parameters.AddWithValue("@v22", p.DsEmbalagem);//ds
                Cmd.Parameters.AddWithValue("@v23", p.QtEmbalagem);//qt
                Cmd.Parameters.AddWithValue("@v24", p.CodigoPrdAssociado);//ass
                Cmd.Parameters.AddWithValue("@v25", p.CodigoBarras);//br
                Cmd.Parameters.AddWithValue("@v26", p.CodigoPIS);
                Cmd.Parameters.AddWithValue("@v27", p.CodigoCOFINS);
                Cmd.Parameters.AddWithValue("@v28", p.CodigoNCM);
                Cmd.Parameters.AddWithValue("@v29", p.CodigoEX);
                Cmd.Parameters.AddWithValue("@v30", p.LinkProduto);

                if(p.FotoPrincipal != null)
                    Cmd.Parameters.AddWithValue("@v31", p.FotoPrincipal);
                else
                    Cmd.Parameters.AddWithValue("@v31", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto1 != null)
                    Cmd.Parameters.AddWithValue("@v32", p.Foto1);
                else
                    Cmd.Parameters.AddWithValue("@v32", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto2 != null)
                    Cmd.Parameters.AddWithValue("@v33", p.Foto2);
                else
                    Cmd.Parameters.AddWithValue("@v33", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto3 != null)
                    Cmd.Parameters.AddWithValue("@v34", p.Foto3);
                else
                    Cmd.Parameters.AddWithValue("@v34", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto4 != null)
                    Cmd.Parameters.AddWithValue("@v35", p.Foto4);
                else
                    Cmd.Parameters.AddWithValue("@v35", System.Data.SqlTypes.SqlBinary.Null);

                if (p.Foto5 != null)
                    Cmd.Parameters.AddWithValue("@v36", p.Foto5);
                else
                    Cmd.Parameters.AddWithValue("@v36", System.Data.SqlTypes.SqlBinary.Null);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void AtualizarIndicadorInventario(int intCodProduto, Boolean blnMarcado)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("update Produto set IN_PRD_INVENTARIO = @v1 Where CD_Produto = @v2", Con);

                if(blnMarcado)
                    Cmd.Parameters.AddWithValue("@v1", 1);
                else
                    Cmd.Parameters.AddWithValue("@v1", 0);

                Cmd.Parameters.AddWithValue("@v2", intCodProduto);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Produto: " + ex.Message.ToString());
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
                Cmd = new SqlCommand("delete from Produto Where CD_Produto = @v1", Con);
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
                            throw new Exception("Erro ao excluir Produto: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Produto PesquisarProduto(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from Produto as p inner join unidade as u on p.CD_UNIDADE = u.CD_UNIDADE  inner join Marca as m on p.CD_MARCA = m.CD_MARCA Where CD_Produto = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Produto p = null;
                if (Dr.Read())
                {
                    p = new Produto();
                    p.CodigoProduto = Convert.ToInt64(Dr["CD_Produto"]);
                    p.DescricaoProduto = Dr["NM_PRODUTO"].ToString();
                    p.CodigoCategoria = Dr["CD_CATEGORIA"].ToString();
                    p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.ValorCompra = Convert.ToDouble(Dr["VL_COMPRA"]);
                    p.PercentualLucro = Convert.ToDouble(Dr["PC_LUCRO"]);
                    p.ValorVenda = Convert.ToDouble(Dr["VL_VENDA"]);
                    p.CodigoUnidade = Convert.ToInt32(Dr["CD_Unidade"]);
                    p.CodigoGpoTribProduto = Convert.ToInt32(Dr["CD_GPO_TRIB_PRODUTO"]);
                    p.CodigoTipoProduto = Convert.ToInt32(Dr["CD_TIPO_PRODUTO"]);
                    if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
                        p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();

                    p.DsSigla = Convert.ToString(Dr["SIGLA"]);
                    p.CodigoFabricante = Convert.ToInt32(Dr["CD_FABRICANTE"]);
                    p.CodigoMarca = Convert.ToInt32(Dr["CD_MARCA"]);
                    p.CodigoIndexCEST = Convert.ToInt32(Dr["CD_CEST"]);
                    //ALTERAÇÃO MATEUS//
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.ProdutoInventario = Convert.ToBoolean(Dr["IN_PRD_INVENTARIO"]);
                    p.ControlaLote = Convert.ToBoolean(Dr["IN_CTR_LOTE"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_PESO"]);
                    p.ValorVolume = Convert.ToDecimal(Dr["VL_VOLUME"]);
                    p.ValorFatorCubagem = Convert.ToDecimal(Dr["VL_FATOR_CUBAGEM"]);
                    p.QtEmbalagem = Convert.ToInt16(Dr["QT_EMBALAGEM"]);
                    p.DsEmbalagem = Convert.ToString(Dr["DS_EMBALAGEM"]);
                    p.CodigoBarras = Convert.ToString(Dr["CD_BARRAS"]);
                    p.CodigoPrdAssociado = Convert.ToInt32(Dr["CD_PRD_ASSOCIADO"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.CodigoNCM = Dr["CD_NCM"].ToString();
                    p.CodigoEX = Dr["CD_EX"].ToString();

                    if(Dr["TX_LNK_PRODUTO"] != DBNull.Value)
                        p.LinkProduto = Dr["TX_LNK_PRODUTO"].ToString();

                    if (Dr["IM_FOTO_PRINCIPAL"] != DBNull.Value)
                        p.FotoPrincipal = (byte[])Dr["IM_FOTO_PRINCIPAL"];

                    if (Dr["IM_FOTO1"] != DBNull.Value)
                        p.Foto1 = (byte[])Dr["IM_FOTO1"];

                    if (Dr["IM_FOTO2"] != DBNull.Value)
                        p.Foto2 = (byte[])Dr["IM_FOTO2"];

                    if (Dr["IM_FOTO3"] != DBNull.Value)
                        p.Foto3 = (byte[])Dr["IM_FOTO3"];

                    if (Dr["IM_FOTO4"] != DBNull.Value)
                        p.Foto4 = (byte[])Dr["IM_FOTO4"];

                    if (Dr["IM_FOTO5"] != DBNull.Value)
                        p.Foto5 = (byte[])Dr["IM_FOTO5"];

                    if (Dr["DS_MARCA"] != DBNull.Value)
                        p.DsMarca = Dr["DS_MARCA"].ToString();

                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Produto> ListarProdutos(string strNomeCampo, string strTipoCampo, string strValor, string strOrdem)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "select * from produto as p inner join unidade as u on p.CD_UNIDADE = u.CD_UNIDADE  inner join marca as m on p.CD_MARCA = m.CD_MARCA ";

                if (strValor != "")
                    strSQL = strSQL + " Where " + MontaFiltro(strNomeCampo, strTipoCampo, strValor);

                if (strOrdem != "")
                    strSQL = strSQL + "Order By " + strOrdem;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Produto> lista = new List<Produto>();

                while (Dr.Read())
                {
                    Produto p = new Produto();
                    p.CodigoProduto = Convert.ToInt64(Dr["CD_Produto"]);
                    p.DescricaoProduto = Dr["NM_PRODUTO"].ToString();
                    p.CodigoCategoria = Dr["CD_CATEGORIA"].ToString();
                    p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.ValorCompra = Convert.ToDouble(Dr["VL_COMPRA"]);
                    p.PercentualLucro = Convert.ToDouble(Dr["PC_LUCRO"]);
                    p.ValorVenda = Convert.ToDouble(Dr["VL_VENDA"]);

                    p.CodigoUnidade = Convert.ToInt32(Dr["CD_Unidade"]);

                    p.CodigoGpoTribProduto = Convert.ToInt32(Dr["CD_GPO_TRIB_PRODUTO"]);
                    p.CodigoTipoProduto = Convert.ToInt32(Dr["CD_TIPO_PRODUTO"]);
                    if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
                        p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();
                    p.CodigoFabricante = Convert.ToInt32(Dr["CD_FABRICANTE"]);
                    p.CodigoMarca = Convert.ToInt32(Dr["CD_MARCA"]);
                    p.CodigoIndexCEST = Convert.ToInt32(Dr["CD_CEST"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.ProdutoInventario = Convert.ToBoolean(Dr["IN_PRD_INVENTARIO"]);
                    p.ControlaLote = Convert.ToBoolean(Dr["IN_CTR_LOTE"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_PESO"]);
                    p.ValorVolume = Convert.ToDecimal(Dr["VL_VOLUME"]);
                    p.ValorFatorCubagem = Convert.ToDecimal(Dr["VL_FATOR_CUBAGEM"]);
                    p.QtEmbalagem = Convert.ToInt16(Dr["QT_EMBALAGEM"]);
                    p.DsEmbalagem = Convert.ToString(Dr["DS_EMBALAGEM"]);
                    p.CodigoBarras = Convert.ToString(Dr["CD_BARRAS"]);
                    p.DsSigla = Convert.ToString(Dr["DS_UNIDADE"]);
                    p.CodigoPrdAssociado = Convert.ToInt32(Dr["CD_PRD_ASSOCIADO"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.CodigoNCM = Dr["CD_NCM"].ToString();
                    p.CodigoEX = Dr["CD_EX"].ToString();

                    p.DsMarca = Convert.ToString(Dr["DS_MARCA"]);


                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public string RetornaNomeProduto(Int64 Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select NM_PRODUTO from Produto Where CD_Produto = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                string StrNome = "";
                if (Dr.Read())
                {
                    StrNome = Dr["NM_PRODUTO"].ToString();
                }
                return StrNome;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public DataTable ObterTiposServicos()
        {
            try
            {
                AbrirConexao();

                string strSQL = "Select * from PRODUTO Where CD_TIPO_PRODUTO=29 ";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                // Cria DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("CodigoProduto", typeof(Int32));
                dt.Columns.Add("DescricaoProduto", typeof(string));
                while (Dr.Read())
                    dt.Rows.Add(Dr["CD_PRODUTO"], Dr["NM_PRODUTO"]);

                return dt;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos produtos: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public int PesquisaCodigoBarrasInformado(string CdBarras, int intCdprod)
        {
            try
            { 
                AbrirConexao();

                string strSQL = "SELECT CD_BARRAS, CD_PRODUTO FROM PRODUTO WHERE CD_BARRAS = '" + CdBarras + "' AND CD_PRODUTO <> " + intCdprod;

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();

                int Cdprod1 = 0;
                int Cdprod2 = 0;

                if (Dr.Read())
                {
                    Cdprod1 = Convert.ToInt32(Dr["CD_PRODUTO"]);
                    if (Cdprod1 != 0)
                        Cdprod2 = Cdprod1;
                }
                return Cdprod2;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar Estoques: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Produto> ListarEmbalagens(int intcdprod)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "Select CD_PRODUTO, CD_PRD_ASSOCIADO, QT_EMBALAGEM, DS_EMBALAGEM from [PRODUTO] where CD_PRD_ASSOCIADO = " + intcdprod;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Produto> lista = new List<Produto>();

                while (Dr.Read())
                {
                    Produto p = new Produto();
                    p.CodigoProduto = Convert.ToInt64(Dr["CD_PRODUTO"]);
                    p.CodigoPrdAssociado = Convert.ToInt32(Dr["CD_PRD_ASSOCIADO"]);
                    p.QtEmbalagem = Convert.ToInt16(Dr["QT_EMBALAGEM"]);
                    p.DsEmbalagem = Convert.ToString(Dr["DS_EMBALAGEM"]);

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void UpdateEmbalagem(long intProd, string strEmb)
        {
            try
            {
                //
                AbrirConexao();

                string strSQL = "UPDATE PRODUTO SET DS_EMBALAGEM = '" + strEmb + "' WHERE CD_PRODUTO = " + intProd;

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Produto> ListarProdutosParaComposicao(string strDescricao)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from produto where CD_TIPO_PRODUTO <> 30";
                if (strDescricao != "")
                    strSQL += " AND NM_PRODUTO like '%" + strDescricao + "%'";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Produto> lista = new List<Produto>();

                while (Dr.Read())
                {
                    Produto p = new Produto();
                    p.CodigoProduto = Convert.ToInt64(Dr["CD_Produto"]);
                    p.DescricaoProduto = Dr["NM_PRODUTO"].ToString();
                    p.ValorCompra = Convert.ToDouble(Dr["VL_COMPRA"]);

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public Produto PesquisarProdutoParaComposicao (int Codigo)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("Select * from Produto Where CD_Produto = @v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Codigo);
                Dr = Cmd.ExecuteReader();
                Produto p = null;
                if (Dr.Read())
                {
                    p = new Produto();
                    p.CodigoProduto = Convert.ToInt64(Dr["CD_Produto"]);
                    p.DescricaoProduto = Dr["NM_PRODUTO"].ToString();
                    p.CodigoCategoria = Dr["CD_CATEGORIA"].ToString();
                    p.DataCadastro = Convert.ToDateTime(Dr["DT_CADASTRO"]);
                    p.DataAtualizacao = Convert.ToDateTime(Dr["DT_ATUALIZACAO"]);
                    p.ValorCompra = Convert.ToDouble(Dr["VL_COMPRA"]);
                    p.PercentualLucro = Convert.ToDouble(Dr["PC_LUCRO"]);
                    p.ValorVenda = Convert.ToDouble(Dr["VL_VENDA"]);
                    p.CodigoUnidade = Convert.ToInt32(Dr["CD_Unidade"]);
                    p.CodigoGpoTribProduto = Convert.ToInt32(Dr["CD_GPO_TRIB_PRODUTO"]);
                    p.CodigoTipoProduto = Convert.ToInt32(Dr["CD_TIPO_PRODUTO"]);
                    if (Dr["CD_SIS_ANTERIOR"] != DBNull.Value)
                        p.CodigoSisAnterior = Dr["CD_SIS_ANTERIOR"].ToString();

                    p.CodigoFabricante = Convert.ToInt32(Dr["CD_FABRICANTE"]);
                    p.CodigoMarca = Convert.ToInt32(Dr["CD_MARCA"]);
                    p.CodigoIndexCEST = Convert.ToInt32(Dr["CD_CEST"]);
                    //ALTERAÇÃO MATEUS//
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.ProdutoInventario = Convert.ToBoolean(Dr["IN_PRD_INVENTARIO"]);
                    p.ControlaLote = Convert.ToBoolean(Dr["IN_CTR_LOTE"]);
                    p.ValorPeso = Convert.ToDecimal(Dr["VL_PESO"]);
                    p.ValorVolume = Convert.ToDecimal(Dr["VL_VOLUME"]);
                    p.ValorFatorCubagem = Convert.ToDecimal(Dr["VL_FATOR_CUBAGEM"]);
                    p.QtEmbalagem = Convert.ToInt16(Dr["QT_EMBALAGEM"]);
                    p.DsEmbalagem = Convert.ToString(Dr["DS_EMBALAGEM"]);
                    p.CodigoBarras = Convert.ToString(Dr["CD_BARRAS"]);
                    p.CodigoPrdAssociado = Convert.ToInt32(Dr["CD_PRD_ASSOCIADO"]);
                    p.CodigoPIS = Convert.ToInt32(Dr["CD_PIS"]);
                    p.CodigoCOFINS = Convert.ToInt32(Dr["CD_COFINS"]);
                    p.CodigoNCM = Dr["CD_NCM"].ToString();
                    p.CodigoEX = Dr["CD_EX"].ToString();



                }
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public List<Produto> ListarProdutosPesquisa(string strDescricao)
        {
            try
            {
                AbrirConexao();

                string strSQL = "select * from produto as p inner join unidade as u on p.CD_UNIDADE = u.CD_UNIDADE where cd_situacao = 1 ";
                if (strDescricao != "")
                    strSQL += " and NM_PRODUTO like '%" + strDescricao + "%'";

                Cmd = new SqlCommand(strSQL, Con);

                Dr = Cmd.ExecuteReader();

                List<Produto> lista = new List<Produto>();

                while (Dr.Read())
                {
                    Produto p = new Produto();
                    p.CodigoProduto = Convert.ToInt64(Dr["CD_Produto"]);
                    p.DescricaoProduto = Dr["NM_PRODUTO"].ToString();
                    p.ValorCompra = Convert.ToDouble(Dr["VL_COMPRA"]);
					p.DsSigla = Convert.ToString(Dr["SIGLA"]);
                    p.DsEmbalagem = Convert.ToString(Dr["DS_EMBALAGEM"]);
                    p.QtEmbalagem  = Convert.ToInt16 (Dr["QT_EMBALAGEM"]);
                    p.CodigoBarras = Convert.ToString(Dr["CD_BARRAS"]);
                    p.CodigoNCM = Convert.ToString(Dr["CD_NCM"]);

                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Produto: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
