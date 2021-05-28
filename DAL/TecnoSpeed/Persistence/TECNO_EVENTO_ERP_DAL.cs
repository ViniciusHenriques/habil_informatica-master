using System.IO;
using System.Linq;
using System.Text;
using DAL.Model;
using DAL.Persistence;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DAL.TecnoSpeed.Persistence
{
    public class TECNO_EVENTO_ERP_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_EVENTO_ERP> ListarTECNO_EVENTO_ERP(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_EVENTO_ERP Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_EVENTO_ERP> tcn_nf = new List<TECNO_EVENTO_ERP>();

                while (Dr.Read())
                {
                    TECNO_EVENTO_ERP obj = new TECNO_EVENTO_ERP();

                    if (Dr["ID_EVENTO_ERP"] != DBNull.Value)
                        obj.ID_EVENTO_ERP = Convert.ToDecimal(Dr["ID_EVENTO_ERP"]);

                    if (Dr["IMPRESSORA"] != DBNull.Value)
                        obj.IMPRESSORA = Convert.ToString(Dr["IMPRESSORA"]);

                    if (Dr["TP_EVENTO_ERP"] != DBNull.Value)
                        obj.TP_EVENTO_ERP = Convert.ToString(Dr["TP_EVENTO_ERP"]);

                    if (Dr["CHAVE_BUSCA"] != DBNull.Value)
                        obj.CHAVE_BUSCA = Convert.ToString(Dr["CHAVE_BUSCA"]);

                    if (Dr["TP_ACAO"] != DBNull.Value)
                        obj.TP_ACAO = Convert.ToDecimal(Dr["TP_ACAO"]);

                    if (Dr["DH_EVENTO"] != DBNull.Value)
                        obj.DH_EVENTO = Convert.ToDateTime(Dr["DH_EVENTO"]);

                    if (Dr["TP_STATUS"] != DBNull.Value)
                        obj.TP_STATUS = Convert.ToString(Dr["TP_STATUS"]);

                    if (Dr["TP_RETORNO"] != DBNull.Value)
                        obj.TP_RETORNO = Convert.ToDecimal(Dr["TP_RETORNO"]);

                    if (Dr["TP_RETORNO_CONTINGENCIA_LOCAL"] != DBNull.Value)
                        obj.TP_RETORNO_CONTINGENCIA_LOCAL = Convert.ToDecimal(Dr["TP_RETORNO_CONTINGENCIA_LOCAL"]);

                    if (Dr["NPROT"] != DBNull.Value)
                        obj.NPROT = Convert.ToDecimal(Dr["NPROT"]);

                    if (Dr["JUSTIFICATIVA"] != DBNull.Value)
                        obj.JUSTIFICATIVA = Convert.ToString(Dr["JUSTIFICATIVA"]);

                    if (Dr["CHNFE"] != DBNull.Value)
                        obj.CHNFE = Convert.ToString(Dr["CHNFE"]);

                    if (Dr["SERVER_NAME"] != DBNull.Value)
                        obj.SERVER_NAME = Convert.ToString(Dr["SERVER_NAME"]);

                    if (Dr["TPAMB"] != DBNull.Value)
                        obj.TPAMB = Convert.ToDecimal(Dr["TPAMB"]);

                    if (Dr["CNPJ_EVENTO"] != DBNull.Value)
                        obj.CNPJ_EVENTO = Convert.ToString(Dr["CNPJ_EVENTO"]);

                    if (Dr["COD_RETORNO"] != DBNull.Value)
                        obj.COD_RETORNO = Convert.ToDecimal(Dr["COD_RETORNO"]);

                    if (Dr["DESC_RETORNO"] != DBNull.Value)
                        obj.DESC_RETORNO = Convert.ToString(Dr["DESC_RETORNO"]);

                    if (Dr["EMAIL_ENVIO"] != DBNull.Value)
                        obj.EMAIL_ENVIO = Convert.ToString(Dr["EMAIL_ENVIO"]);

                    if (Dr["COPIAS_IMPRESSAO"] != DBNull.Value)
                        obj.COPIAS_IMPRESSAO = Convert.ToDecimal(Dr["COPIAS_IMPRESSAO"]);

                    if (Dr["TP_IMP"] != DBNull.Value)
                        obj.TP_IMP = Convert.ToDecimal(Dr["TP_IMP"]);

                    if (Dr["LINK_PDF"] != DBNull.Value)
                        obj.LINK_PDF = Convert.ToString(Dr["LINK_PDF"]);

                    if (Dr["BO_ATUALIZADO"] != DBNull.Value)
                        obj.BO_ATUALIZADO = Convert.ToString(Dr["BO_ATUALIZADO"]);

                    if (Dr["TP_DESTINO_IMPRESSAO"] != DBNull.Value)
                        obj.TP_DESTINO_IMPRESSAO = Convert.ToString(Dr["TP_DESTINO_IMPRESSAO"]);

                    if (Dr["USUARIO_IMPRESSAO"] != DBNull.Value)
                        obj.USUARIO_IMPRESSAO = Convert.ToString(Dr["USUARIO_IMPRESSAO"]);

                    if (Dr["NR_FORMULARIO_CONTINGENCIA"] != DBNull.Value)
                        obj.NR_FORMULARIO_CONTINGENCIA = Convert.ToDecimal(Dr["NR_FORMULARIO_CONTINGENCIA"]);

                    if (Dr["nome_arquivo_cfg_danfe"] != DBNull.Value)
                        obj.nome_arquivo_cfg_danfe = Convert.ToString(Dr["nome_arquivo_cfg_danfe"]);

                    if (Dr["csitconf"] != DBNull.Value)
                        obj.csitconf = Convert.ToDecimal(Dr["csitconf"]);

                    if (Dr["bo_enviou_email"] != DBNull.Value)
                        obj.bo_enviou_email = Convert.ToString(Dr["bo_enviou_email"]);

                    if (Dr["ID_CLE"] != DBNull.Value)
                        obj.ID_CLE = Convert.ToDecimal(Dr["ID_CLE"]);

                    if (Dr["ID_CONSULTA_DEST"] != DBNull.Value)
                        obj.ID_CONSULTA_DEST = Convert.ToDecimal(Dr["ID_CONSULTA_DEST"]);

                    if (Dr["CTE_ID"] != DBNull.Value)
                        obj.CTE_ID = Convert.ToDecimal(Dr["CTE_ID"]);

                    if (Dr["ID_CTE"] != DBNull.Value)
                        obj.ID_CTE = Convert.ToDecimal(Dr["ID_CTE"]);

                    if (Dr["CONSULTA_STATUS_CUF"] != DBNull.Value)
                        obj.CONSULTA_STATUS_CUF = Convert.ToString(Dr["CONSULTA_STATUS_CUF"]);

                    if (Dr["BO_CONSULTA_STATUS_SVC"] != DBNull.Value)
                        obj.BO_CONSULTA_STATUS_SVC = Convert.ToString(Dr["BO_CONSULTA_STATUS_SVC"]);

                    if (Dr["IDE_MOD"] != DBNull.Value)
                        obj.IDE_MOD = Convert.ToDecimal(Dr["IDE_MOD"]);

                    if (Dr["CHNFEREF"] != DBNull.Value)
                        obj.CHNFEREF = Convert.ToString(Dr["CHNFEREF"]);

                    if (Dr["GMT"] != DBNull.Value)
                        obj.GMT = Convert.ToString(Dr["GMT"]);
                    tcn_nf.Add(obj);
                }
                return tcn_nf;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_EVENTO_ERP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public TECNO_EVENTO_ERP PesquisarTECNO_EVENTO_ERP(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_EVENTO_ERP Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_EVENTO_ERP obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_EVENTO_ERP();

                    if (Dr["ID_EVENTO_ERP"] != DBNull.Value)
                        obj.ID_EVENTO_ERP = Convert.ToDecimal(Dr["ID_EVENTO_ERP"]);

                    if (Dr["IMPRESSORA"] != DBNull.Value)
                        obj.IMPRESSORA = Convert.ToString(Dr["IMPRESSORA"]);

                    if (Dr["TP_EVENTO_ERP"] != DBNull.Value)
                        obj.TP_EVENTO_ERP = Convert.ToString(Dr["TP_EVENTO_ERP"]);

                    if (Dr["CHAVE_BUSCA"] != DBNull.Value)
                        obj.CHAVE_BUSCA = Convert.ToString(Dr["CHAVE_BUSCA"]);

                    if (Dr["TP_ACAO"] != DBNull.Value)
                        obj.TP_ACAO = Convert.ToDecimal(Dr["TP_ACAO"]);

                    if (Dr["DH_EVENTO"] != DBNull.Value)
                        obj.DH_EVENTO = Convert.ToDateTime(Dr["DH_EVENTO"]);

                    if (Dr["TP_STATUS"] != DBNull.Value)
                        obj.TP_STATUS = Convert.ToString(Dr["TP_STATUS"]);

                    if (Dr["TP_RETORNO"] != DBNull.Value)
                        obj.TP_RETORNO = Convert.ToDecimal(Dr["TP_RETORNO"]);

                    if (Dr["TP_RETORNO_CONTINGENCIA_LOCAL"] != DBNull.Value)
                        obj.TP_RETORNO_CONTINGENCIA_LOCAL = Convert.ToDecimal(Dr["TP_RETORNO_CONTINGENCIA_LOCAL"]);

                    if (Dr["NPROT"] != DBNull.Value)
                        obj.NPROT = Convert.ToDecimal(Dr["NPROT"]);

                    if (Dr["JUSTIFICATIVA"] != DBNull.Value)
                        obj.JUSTIFICATIVA = Convert.ToString(Dr["JUSTIFICATIVA"]);

                    if (Dr["CHNFE"] != DBNull.Value)
                        obj.CHNFE = Convert.ToString(Dr["CHNFE"]);

                    if (Dr["SERVER_NAME"] != DBNull.Value)
                        obj.SERVER_NAME = Convert.ToString(Dr["SERVER_NAME"]);

                    if (Dr["TPAMB"] != DBNull.Value)
                        obj.TPAMB = Convert.ToDecimal(Dr["TPAMB"]);

                    if (Dr["CNPJ_EVENTO"] != DBNull.Value)
                        obj.CNPJ_EVENTO = Convert.ToString(Dr["CNPJ_EVENTO"]);

                    if (Dr["COD_RETORNO"] != DBNull.Value)
                        obj.COD_RETORNO = Convert.ToDecimal(Dr["COD_RETORNO"]);

                    if (Dr["DESC_RETORNO"] != DBNull.Value)
                        obj.DESC_RETORNO = Convert.ToString(Dr["DESC_RETORNO"]);

                    if (Dr["EMAIL_ENVIO"] != DBNull.Value)
                        obj.EMAIL_ENVIO = Convert.ToString(Dr["EMAIL_ENVIO"]);

                    if (Dr["COPIAS_IMPRESSAO"] != DBNull.Value)
                        obj.COPIAS_IMPRESSAO = Convert.ToDecimal(Dr["COPIAS_IMPRESSAO"]);

                    if (Dr["TP_IMP"] != DBNull.Value)
                        obj.TP_IMP = Convert.ToDecimal(Dr["TP_IMP"]);

                    if (Dr["LINK_PDF"] != DBNull.Value)
                        obj.LINK_PDF = Convert.ToString(Dr["LINK_PDF"]);

                    if (Dr["BO_ATUALIZADO"] != DBNull.Value)
                        obj.BO_ATUALIZADO = Convert.ToString(Dr["BO_ATUALIZADO"]);

                    if (Dr["TP_DESTINO_IMPRESSAO"] != DBNull.Value)
                        obj.TP_DESTINO_IMPRESSAO = Convert.ToString(Dr["TP_DESTINO_IMPRESSAO"]);

                    if (Dr["USUARIO_IMPRESSAO"] != DBNull.Value)
                        obj.USUARIO_IMPRESSAO = Convert.ToString(Dr["USUARIO_IMPRESSAO"]);

                    if (Dr["NR_FORMULARIO_CONTINGENCIA"] != DBNull.Value)
                        obj.NR_FORMULARIO_CONTINGENCIA = Convert.ToDecimal(Dr["NR_FORMULARIO_CONTINGENCIA"]);

                    if (Dr["nome_arquivo_cfg_danfe"] != DBNull.Value)
                        obj.nome_arquivo_cfg_danfe = Convert.ToString(Dr["nome_arquivo_cfg_danfe"]);

                    if (Dr["csitconf"] != DBNull.Value)
                        obj.csitconf = Convert.ToDecimal(Dr["csitconf"]);

                    if (Dr["bo_enviou_email"] != DBNull.Value)
                        obj.bo_enviou_email = Convert.ToString(Dr["bo_enviou_email"]);

                    if (Dr["ID_CLE"] != DBNull.Value)
                        obj.ID_CLE = Convert.ToDecimal(Dr["ID_CLE"]);

                    if (Dr["ID_CONSULTA_DEST"] != DBNull.Value)
                        obj.ID_CONSULTA_DEST = Convert.ToDecimal(Dr["ID_CONSULTA_DEST"]);

                    if (Dr["CTE_ID"] != DBNull.Value)
                        obj.CTE_ID = Convert.ToDecimal(Dr["CTE_ID"]);

                    if (Dr["ID_CTE"] != DBNull.Value)
                        obj.ID_CTE = Convert.ToDecimal(Dr["ID_CTE"]);

                    if (Dr["CONSULTA_STATUS_CUF"] != DBNull.Value)
                        obj.CONSULTA_STATUS_CUF = Convert.ToString(Dr["CONSULTA_STATUS_CUF"]);

                    if (Dr["BO_CONSULTA_STATUS_SVC"] != DBNull.Value)
                        obj.BO_CONSULTA_STATUS_SVC = Convert.ToString(Dr["BO_CONSULTA_STATUS_SVC"]);

                    if (Dr["IDE_MOD"] != DBNull.Value)
                        obj.IDE_MOD = Convert.ToDecimal(Dr["IDE_MOD"]);

                    if (Dr["CHNFEREF"] != DBNull.Value)
                        obj.CHNFEREF = Convert.ToString(Dr["CHNFEREF"]);

                    if (Dr["GMT"] != DBNull.Value)
                        obj.GMT = Convert.ToString(Dr["GMT"]);

                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_EVENTO_ERP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}



