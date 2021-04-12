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
    //Teste
    public class PessoaProdutoDocumentoDAL : Conexao
    {
        protected string strSQL = "";
        protected string strSQL2 = "";
        public void ExcluirTodos(decimal CodigoDocumento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("delete from PESSOA_DO_PRODUTO_DO_DOCUMENTO Where CD_DOCUMENTO= @v1 ", Con);
                Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
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
                            throw new Exception("Erro ao excluir PESSOA produtos do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir PESSOA produtos do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public void InserirItem(decimal CodigoDocumento, List<PessoaProdutoDocumento> listaItemCotacao)
        {
            try
            {
                ExcluirTodos(CodigoDocumento);
                AbrirConexao();
                foreach (PessoaProdutoDocumento p in listaItemCotacao)
                {
                    strSQL = "insert into PESSOA_DO_PRODUTO_DO_DOCUMENTO (CD_DOCUMENTO, ";

                    strSQL += " CD_PES_PRD_DOCUMENTO, ";
                    strSQL += " CD_PESSOA, ";
                    strSQL += " CD_PRODUTO, ";
                    strSQL += " CD_PRD_PESSOA, ";
                    strSQL += " VL_PRECO_COMPRA, ";
                    strSQL += " IN_NAO_ATENDE, ";
                    strSQL += " OB_FINANCEIRA, ";
                    strSQL += " OB_IMPOSTO, ";
                    strSQL += " DT_DIA_ENTREGA, ";
                    strSQL += " CD_SITUACAO ";

                    if (p.DataResposta != null)
                        strSQL += ", DT_RESPOSTA";

                    if (p.DataAprovacao != null)
                        strSQL += ", DT_APROVACAO";

                    strSQL += ") values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11";

                    if (p.DataResposta != null)
                        strSQL += ",@v12";

                    if (p.DataAprovacao != null)
                        strSQL += ",@v13";
                    
                    strSQL += ")";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoItem);
                    Cmd.Parameters.AddWithValue("@v3", p.CodigoPessoa);
                    Cmd.Parameters.AddWithValue("@v4", p.CodigoProduto);
                    Cmd.Parameters.AddWithValue("@v5", p.CodigoProdutoPessoa);
                    Cmd.Parameters.AddWithValue("@v6", p.PrecoItem);
                    Cmd.Parameters.AddWithValue("@v7", p.NaoAtendeItem);
                    Cmd.Parameters.AddWithValue("@v8", p.OBSFinanceira);
                    Cmd.Parameters.AddWithValue("@v9", p.OBSImposto);
                    Cmd.Parameters.AddWithValue("@v10", p.DataDiaEntrega);
                    Cmd.Parameters.AddWithValue("@v11", p.CodigoSituacao );

                    if (p.DataResposta != null)
                        Cmd.Parameters.AddWithValue("@v12", p.DataResposta);

                    if (p.DataAprovacao != null)
                        Cmd.Parameters.AddWithValue("@v13", p.DataAprovacao);

                    Cmd.ExecuteNonQuery();

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
                            throw new Exception("Erro ao Incluir PESSOA produtos do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar PESSOA produtos do documento: " + ex.Message.ToString());
            }
            finally
            {
                
                FecharConexao();
            }
        }
        public void AtualizarItem(List<PessoaProdutoDocumento> listaItemCotacao)
        {
            try
            {
                AbrirConexao();

                foreach (PessoaProdutoDocumento p in listaItemCotacao)
                {
                    strSQL = "UPDATE PESSOA_DO_PRODUTO_DO_DOCUMENTO";
                    strSQL += " SET CD_PESSOA = @v3, CD_PRODUTO = @v4, PRC_COMPRA = @v5, IN_NAO_ATENDE = @v6, OB_FINANCEIRA = @v7, " +
                        "OB_IMPOSTO = @v8, DT_DIA_ENTREGA = @v9, CD_SITUACAO = @v10, CD_PRD_PESSOA = @v11 ";

                    if (p.DataResposta != null)
                        strSQL += " DT_RESPOSTA = @v12";

                    if (p.DataAprovacao != null)
                        strSQL += " DT_APROVACAO = @v13";

                    strSQL += "WHERE CD_DOCUMENTO = @v1 AND CD_PROD_DOCUMENTO = @v2";

                    Cmd = new SqlCommand(strSQL, Con);

                    Cmd.Parameters.AddWithValue("@v1", p.CodigoDocumento);
                    Cmd.Parameters.AddWithValue("@v2", p.CodigoItem);
                    Cmd.Parameters.AddWithValue("@v3", p.CodigoPessoa);
                    Cmd.Parameters.AddWithValue("@v4", p.CodigoProduto);
                    Cmd.Parameters.AddWithValue("@v5", p.PrecoItem);
                    Cmd.Parameters.AddWithValue("@v6", p.NaoAtendeItem);
                    Cmd.Parameters.AddWithValue("@v7", p.OBSFinanceira);
                    Cmd.Parameters.AddWithValue("@v8", p.OBSImposto);
                    Cmd.Parameters.AddWithValue("@v9", p.DataDiaEntrega);
                    Cmd.Parameters.AddWithValue("@v10", p.CodigoSituacao);
                    Cmd.Parameters.AddWithValue("@v11", p.CodigoProdutoPessoa);

                    if (p.DataResposta != null)
                        Cmd.Parameters.AddWithValue("@v12", p.DataResposta);

                    if (p.DataAprovacao != null)
                        Cmd.Parameters.AddWithValue("@v13", p.DataAprovacao);

                    Cmd.ExecuteNonQuery();

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
                            throw new Exception("Erro ao atualizar produtos do documento: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar produtos do documento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
		public List<PessoaProdutoDocumento> ListarPessoaProdutosDocumento(decimal CodigoDocumento)
		{
			try
			{
				AbrirConexao();
				Cmd = new SqlCommand(" select PD.*, P.NM_PRODUTO, TP.DS_TIPO AS DS_SITUACAO " +
					"from PESSOA_DO_PRODUTO_DO_DOCUMENTO as PD " +
                         "INNER JOIN PRODUTO AS P " +
                              "ON PD.CD_PRODUTO = P.CD_PRODUTO " +
                         "INNER JOIN HABIL_TIPO AS TP " +
                              "ON TP.CD_TIPO = p.CD_SITUACAO " +
                      "Where PD.CD_DOCUMENTO = @v1 ", Con);

				Cmd.Parameters.AddWithValue("@v1", CodigoDocumento);
				Dr = Cmd.ExecuteReader();
				List<PessoaProdutoDocumento> lista = new List<PessoaProdutoDocumento>();

				while (Dr.Read())
				{
                    PessoaProdutoDocumento p = new PessoaProdutoDocumento();
                    p.CodigoProdutoPessoa = Dr["CD_PRD_PESSOA"].ToString();
                    p.CodigoProduto = Convert.ToInt32(Dr["CD_PRODUTO"]);
					p.Cpl_DscProduto = Dr["NM_PRODUTO"].ToString();
                    p.Cpl_DscSituacao = Dr["DS_SITUACAO"].ToString();
                    p.PrecoItem = Convert.ToDecimal(Dr["VL_PRECO_COMPRA"]);
                    p.CodigoDocumento = Convert.ToInt64(Dr["CD_DOCUMENTO"]);
                    p.CodigoItem = Convert.ToInt32(Dr["CD_PES_PRD_DOCUMENTO"]);
                    p.CodigoPessoa= Convert.ToInt32(Dr["CD_PESSOA"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DataDiaEntrega = Dr["DT_DIA_ENTREGA"].ToString();
                    p.OBSFinanceira = Dr["OB_FINANCEIRA"].ToString();
                    p.OBSImposto = Dr["OB_IMPOSTO"].ToString();
                    p.DataDiaEntrega = Dr["DT_DIA_ENTREGA"].ToString();
                    p.NaoAtendeItem = Convert.ToInt16(Dr["IN_NAO_ATENDE"]);

                    if (Dr["DT_APROVACAO"] != DBNull.Value)
						p.DataAprovacao= Convert.ToDateTime(Dr["DT_APROVACAO"]);

					if (Dr["DT_RESPOSTA"] != DBNull.Value)
						p.DataResposta = Convert.ToDateTime(Dr["DT_RESPOSTA"]);

					lista.Add(p);
				}
				return lista;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao Obter Pessoa Produtos do Documento: " + ex.Message.ToString());
			}
			finally
			{
				FecharConexao();
			}
		}


        //public int CodigoItem { get; set; }
        //public decimal CodigoDocumento { get; set; }
        //public decimal PrecoItem { get; set; }
        //public int CodigoProduto { get; set; }
        //public int CodigoPessoa { get; set; }
        //public int CodigoSituacao { get; set; }
        //public string OBSFinanceira { get; set; }
        //public string OBSImposto { get; set; }
        //public string DataDiaEntrega { get; set; }
        //public short NaoAtendeItem { get; set; }
        //public DateTime? DataResposta { get; set; }
        //public DateTime? DataAprovacao { get; set; }

    }
}
