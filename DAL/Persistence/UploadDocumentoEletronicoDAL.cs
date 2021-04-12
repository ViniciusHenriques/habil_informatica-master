using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Persistence
{
    public class UploadDocumentoEletronicoDAL : Conexao
    {
        protected string strSQL = "";
        public void Inserir(UploadDocumentoEletronico doc)
        {
            try
            {
                AbrirConexao();

                Cmd = new SqlCommand("insert into UPLOAD_DO_DOCUMENTO_ELETRONICO" +
                                                               "([CH_ACESSO]" +
                                                               ",[VL_BASE_ICMS]" +
                                                               ",[VL_ICMS]" +
                                                               ",[XML_DOC]" +
                                                               ",[CD_TIPO_DOCUMENTO])" +
                                                         "VALUES(@v1,@v2,@v3,@v4, @v5);", Con);

                Cmd.Parameters.AddWithValue("@v1", doc.ChaveAcesso);
                Cmd.Parameters.AddWithValue("@v2", doc.ValorBaseCalculoICMS);
                Cmd.Parameters.AddWithValue("@v3", doc.ValorICMS);
                Cmd.Parameters.AddWithValue("@v4", doc.ArquivoXML);
                Cmd.Parameters.AddWithValue("@v5", doc.CodigoTipoDocumento);


                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar UPLOAD DO DOCUMENTO ELETRONICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool PesquisarUploadDocumentoEletronico(string strChaveAcesso)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select CD_INDEX from [UPLOAD_DO_DOCUMENTO_ELETRONICO] with(nolock) Where CH_ACESSO = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", strChaveAcesso);

                Dr = Cmd.ExecuteReader();

                bool blnExiste = false;

                if (Dr.Read())
                {
                    blnExiste = true;
                }
                
                return blnExiste;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar UPLOAD_DO_DOCUMENTO_ELETRONICO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}

