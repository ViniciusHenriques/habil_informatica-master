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

        public List<TECNO_EVENTO_ERP> ListarTECNO_EVENTO_ERP()
        {
            try
            {
                AbrirConexao();

                string comando = "Select * from TECNO_EVENTO_ERP WHERE TP_STATUS = 'C' AND TP_EVENTO_ERP = 'E'";

                Cmd = new SqlCommand(comando, Con);

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

                    if (Dr["NREC"] != DBNull.Value)
                        obj.NREC = Convert.ToDecimal(Dr["NREC"]);

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

                    if (Dr["CONFIG_EMAIL_DESTINATARIO"] != DBNull.Value)
                        obj.CONFIG_EMAIL_DESTINATARIO = Convert.ToString(Dr["CONFIG_EMAIL_DESTINATARIO"]);

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

                    if (Dr["ID_CLE"] != DBNull.Value)
                        obj.ID_CLE = Convert.ToDecimal(Dr["ID_CLE"]);

                    if (Dr["ID_CONSULTA_DEST"] != DBNull.Value)
                        obj.ID_CONSULTA_DEST = Convert.ToDecimal(Dr["ID_CONSULTA_DEST"]);

                    if (Dr["CTE_ID"] != DBNull.Value)
                        obj.CTE_ID = Convert.ToDecimal(Dr["CTE_ID"]);

                    if (Dr["ID_CTE"] != DBNull.Value)
                        obj.ID_CTE = Convert.ToDecimal(Dr["ID_CTE"]);

                    if (Dr["CODIGO_UF"] != DBNull.Value)
                        obj.CODIGO_UF = Convert.ToString(Dr["CODIGO_UF"]);

                    if (Dr["BO_CONSULTA_STATUS_SVC"] != DBNull.Value)
                        obj.BO_CONSULTA_STATUS_SVC = Convert.ToString(Dr["BO_CONSULTA_STATUS_SVC"]);

                    if (Dr["IDE_MOD"] != DBNull.Value)
                        obj.IDE_MOD = Convert.ToDecimal(Dr["IDE_MOD"]);

                    if (Dr["CHNFEREF"] != DBNull.Value)
                        obj.CHNFEREF = Convert.ToString(Dr["CHNFEREF"]);

                    if (Dr["GMT"] != DBNull.Value)
                        obj.GMT = Convert.ToString(Dr["GMT"]);

                    if (Dr["CONFIG_EMAIL_SERVIDOR"] != DBNull.Value)
                        obj.CONFIG_EMAIL_SERVIDOR = Convert.ToString(Dr["CONFIG_EMAIL_SERVIDOR"]);

                    if (Dr["CONFIG_EMAIL_REMETENTE"] != DBNull.Value)
                        obj.CONFIG_EMAIL_REMETENTE = Convert.ToString(Dr["CONFIG_EMAIL_REMETENTE"]);

                    if (Dr["CONFIG_EMAIL_ASSUNTO"] != DBNull.Value)
                        obj.CONFIG_EMAIL_ASSUNTO = Convert.ToString(Dr["CONFIG_EMAIL_ASSUNTO"]);

                    if (Dr["CONFIG_EMAIL_MENSAGEM"] != DBNull.Value)
                        obj.CONFIG_EMAIL_MENSAGEM = Convert.ToString(Dr["CONFIG_EMAIL_MENSAGEM"]);

                    if (Dr["CONFIG_EMAIL_USUARIO"] != DBNull.Value)
                        obj.CONFIG_EMAIL_USUARIO = Convert.ToString(Dr["CONFIG_EMAIL_USUARIO"]);

                    if (Dr["CONFIG_EMAIL_SENHA"] != DBNull.Value)
                        obj.CONFIG_EMAIL_SENHA = Convert.ToString(Dr["CONFIG_EMAIL_SENHA"]);

                    if (Dr["CONFIG_EMAIL_AUTENTICACAO"] != DBNull.Value)
                        obj.CONFIG_EMAIL_AUTENTICACAO = Convert.ToInt32(Dr["CONFIG_EMAIL_AUTENTICACAO"]);

                    if (Dr["CONFIG_EMAIL_PORTA"] != DBNull.Value)
                        obj.CONFIG_EMAIL_PORTA = Convert.ToInt32(Dr["CONFIG_EMAIL_PORTA"]);

                    if (Dr["IN_ENVIOU_EMAIL"] != DBNull.Value)
                        obj.IN_ENVIOU_EMAIL = Convert.ToInt32(Dr["IN_ENVIOU_EMAIL"]);

                    if (Dr["IN_REMOVER_ACENTOS"] != DBNull.Value)
                        obj.IN_REMOVER_ACENTOS = Convert.ToInt32(Dr["IN_REMOVER_ACENTOS"]);

                    if (Dr["IN_VALIDAR_ESQUEMA"] != DBNull.Value)
                        obj.IN_VALIDAR_ESQUEMA = Convert.ToInt32(Dr["IN_VALIDAR_ESQUEMA"]);

                    if (Dr["TP_MODO_OPERACAO"] != DBNull.Value)
                        obj.TP_MODO_OPERACAO = Convert.ToInt32(Dr["TP_MODO_OPERACAO"]);

                    if (Dr["CERTIFICADO_CAMINHO"] != DBNull.Value)
                        obj.CERTIFICADO_CAMINHO = Convert.ToString(Dr["CERTIFICADO_CAMINHO"]);

                    if (Dr["CERTIFICADO_SENHA"] != DBNull.Value)
                        obj.CERTIFICADO_SENHA = Convert.ToString(Dr["CERTIFICADO_SENHA"]);

                    if (Dr["VERSAO_MANUAL"] != DBNull.Value)
                        obj.VERSAO_MANUAL = Convert.ToString(Dr["VERSAO_MANUAL"]);

                    if (Dr["VERSAO_MANUAL_DPEC_SCAN"] != DBNull.Value)
                        obj.VERSAO_MANUAL_DPEC_SCAN = Convert.ToString(Dr["VERSAO_MANUAL_DPEC_SCAN"]);

                    if (Dr["NR_LOTE"] != DBNull.Value)
                        obj.NR_LOTE = Convert.ToInt32(Dr["NR_LOTE"]);

                    if (Dr["TP_EVENTO"] != DBNull.Value)
                        obj.TP_EVENTO = Convert.ToInt32(Dr["TP_EVENTO"]);

                    if (Dr["NSU"] != DBNull.Value)
                        obj.NSU = Convert.ToString(Dr["NSU"]);

                    if (Dr["TP_NSU"] != DBNull.Value)
                        obj.TP_NSU = Convert.ToInt32(Dr["TP_NSU"]);

                    if (Dr["TX_CORRECAO"] != DBNull.Value)
                        obj.TX_CORRECAO = Convert.ToString(Dr["TX_CORRECAO"]);

                    if (Dr["CONTRIB_CNPJ_CPF_IE"] != DBNull.Value)
                        obj.CONTRIB_CNPJ_CPF_IE = Convert.ToString(Dr["CONTRIB_CNPJ_CPF_IE"]);

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

        public int RetornarSequenciaEvento(string strCHNFE)
        {
            try
            {
                AbrirConexao();

                string comando = "SELECT (COUNT(CHAVE_BUSCA) + 1) AS SEQUENCIA_DO_EVENTO " +
                                "FROM TECNO_EVENTO_ERP  " +
                                "WHERE TP_ACAO = 2 AND TP_ACAO = 1000 AND TP_ACAO = 1001 AND TP_ACAO = 1002 AND CHNFE = @v1 AND TP_RETORNO = 1";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strCHNFE);

                Dr = Cmd.ExecuteReader();
                if (Dr.Read())
                {
                    if (Dr["SEQUENCIA_DO_EVENTO"] != DBNull.Value)
                        return Convert.ToInt32(Dr["SEQUENCIA_DO_EVENTO"]);
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao procurar eventos : " + ex.Message.ToString());
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

                string comando = "Select * from TECNO_EVENTO_ERP Where CHAVE_BUSCA = @v1 ";

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

                    if (Dr["CONFIG_EMAIL_DESTINATARIO"] != DBNull.Value)
                        obj.CONFIG_EMAIL_DESTINATARIO = Convert.ToString(Dr["CONFIG_EMAIL_DESTINATARIO"]);

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

                    if (Dr["ID_CLE"] != DBNull.Value)
                        obj.ID_CLE = Convert.ToDecimal(Dr["ID_CLE"]);

                    if (Dr["ID_CONSULTA_DEST"] != DBNull.Value)
                        obj.ID_CONSULTA_DEST = Convert.ToDecimal(Dr["ID_CONSULTA_DEST"]);

                    if (Dr["CTE_ID"] != DBNull.Value)
                        obj.CTE_ID = Convert.ToDecimal(Dr["CTE_ID"]);

                    if (Dr["ID_CTE"] != DBNull.Value)
                        obj.ID_CTE = Convert.ToDecimal(Dr["ID_CTE"]);

                    if (Dr["CODIGO_UF"] != DBNull.Value)
                        obj.CODIGO_UF = Convert.ToString(Dr["CODIGO_UF"]);

                    if (Dr["BO_CONSULTA_STATUS_SVC"] != DBNull.Value)
                        obj.BO_CONSULTA_STATUS_SVC = Convert.ToString(Dr["BO_CONSULTA_STATUS_SVC"]);

                    if (Dr["IDE_MOD"] != DBNull.Value)
                        obj.IDE_MOD = Convert.ToDecimal(Dr["IDE_MOD"]);

                    if (Dr["CHNFEREF"] != DBNull.Value)
                        obj.CHNFEREF = Convert.ToString(Dr["CHNFEREF"]);

                    if (Dr["GMT"] != DBNull.Value)
                        obj.GMT = Convert.ToString(Dr["GMT"]);

                    if (Dr["CONFIG_EMAIL_SERVIDOR"] != DBNull.Value)
                        obj.CONFIG_EMAIL_SERVIDOR = Convert.ToString(Dr["CONFIG_EMAIL_SERVIDOR"]);

                    if (Dr["CONFIG_EMAIL_REMETENTE"] != DBNull.Value)
                        obj.CONFIG_EMAIL_REMETENTE = Convert.ToString(Dr["CONFIG_EMAIL_REMETENTE"]);

                    if (Dr["CONFIG_EMAIL_ASSUNTO"] != DBNull.Value)
                        obj.CONFIG_EMAIL_ASSUNTO = Convert.ToString(Dr["CONFIG_EMAIL_ASSUNTO"]);

                    if (Dr["CONFIG_EMAIL_MENSAGEM"] != DBNull.Value)
                        obj.CONFIG_EMAIL_MENSAGEM = Convert.ToString(Dr["CONFIG_EMAIL_MENSAGEM"]);

                    if (Dr["CONFIG_EMAIL_USUARIO"] != DBNull.Value)
                        obj.CONFIG_EMAIL_USUARIO = Convert.ToString(Dr["CONFIG_EMAIL_USUARIO"]);

                    if (Dr["CONFIG_EMAIL_SENHA"] != DBNull.Value)
                        obj.CONFIG_EMAIL_SENHA = Convert.ToString(Dr["CONFIG_EMAIL_SENHA"]);

                    if (Dr["CONFIG_EMAIL_AUTENTICACAO"] != DBNull.Value)
                        obj.CONFIG_EMAIL_AUTENTICACAO = Convert.ToInt32(Dr["CONFIG_EMAIL_AUTENTICACAO"]);

                    if (Dr["CONFIG_EMAIL_PORTA"] != DBNull.Value)
                        obj.CONFIG_EMAIL_PORTA = Convert.ToInt32(Dr["CONFIG_EMAIL_PORTA"]);

                    if (Dr["IN_ENVIOU_EMAIL"] != DBNull.Value)
                        obj.IN_ENVIOU_EMAIL = Convert.ToInt32(Dr["IN_ENVIOU_EMAIL"]);

                    if (Dr["IN_REMOVER_ACENTOS"] != DBNull.Value)
                        obj.IN_REMOVER_ACENTOS = Convert.ToInt32(Dr["IN_REMOVER_ACENTOS"]);

                    if (Dr["IN_VALIDAR_ESQUEMA"] != DBNull.Value)
                        obj.IN_VALIDAR_ESQUEMA = Convert.ToInt32(Dr["IN_VALIDAR_ESQUEMA"]);

                    if (Dr["TP_MODO_OPERACAO"] != DBNull.Value)
                        obj.TP_MODO_OPERACAO = Convert.ToInt32(Dr["TP_MODO_OPERACAO"]);

                    if (Dr["CERTIFICADO_CAMINHO"] != DBNull.Value)
                        obj.CERTIFICADO_CAMINHO = Convert.ToString(Dr["CERTIFICADO_CAMINHO"]);

                    if (Dr["CERTIFICADO_SENHA"] != DBNull.Value)
                        obj.CERTIFICADO_SENHA = Convert.ToString(Dr["CERTIFICADO_SENHA"]);

                    if (Dr["VERSAO_MANUAL"] != DBNull.Value)
                        obj.VERSAO_MANUAL = Convert.ToString(Dr["VERSAO_MANUAL"]);

                    if (Dr["VERSAO_MANUAL_DPEC_SCAN"] != DBNull.Value)
                        obj.VERSAO_MANUAL_DPEC_SCAN = Convert.ToString(Dr["VERSAO_MANUAL_DPEC_SCAN"]);

                    if (Dr["NR_LOTE"] != DBNull.Value)
                        obj.NR_LOTE = Convert.ToInt32(Dr["NR_LOTE"]);

                    if (Dr["TP_EVENTO"] != DBNull.Value)
                        obj.TP_EVENTO = Convert.ToInt32(Dr["TP_EVENTO"]);

                    if (Dr["NSU"] != DBNull.Value)
                        obj.NSU = Convert.ToString(Dr["NSU"]);

                    if (Dr["TP_NSU"] != DBNull.Value)
                        obj.TP_NSU = Convert.ToInt32(Dr["TP_NSU"]);

                    if (Dr["TX_CORRECAO"] != DBNull.Value)
                        obj.TX_CORRECAO = Convert.ToString(Dr["TX_CORRECAO"]);

                    if (Dr["CONTRIB_CNPJ_CPF_IE"] != DBNull.Value)
                        obj.CONTRIB_CNPJ_CPF_IE = Convert.ToString(Dr["CONTRIB_CNPJ_CPF_IE"]);
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

        public void InserirTECNO_EVENTO_ERP(TECNO_EVENTO_ERP obj)
        {
            try
            {

                AbrirConexao();
                Cmd = new SqlCommand("INSERT INTO[dbo].[TECNO_EVENTO_ERP] " +
                                       "([IMPRESSORA] " +
                                       ",[TP_EVENTO_ERP] " +
                                       ",[CHAVE_BUSCA] " +
                                       ",[TP_ACAO] " +
                                       ",[DH_EVENTO] " +
                                       ",[TP_STATUS] " +
                                       ",[TP_RETORNO] " +
                                       ",[TP_RETORNO_CONTINGENCIA_LOCAL] " +
                                       ",[NPROT] " +
                                       ",[JUSTIFICATIVA] " +
                                       ",[CHNFE] " +
                                       ",[SERVER_NAME] " +
                                       ",[TPAMB] " +
                                       ",[CNPJ_EVENTO] " +
                                       ",[COD_RETORNO] " +
                                       ",[DESC_RETORNO] " +
                                       ",[EMAIL_ENVIO] " +
                                       ",[COPIAS_IMPRESSAO] " +
                                       ",[TP_IMP] " +
                                       ",[LINK_PDF] " +
                                       ",[BO_ATUALIZADO] " +
                                       ",[TP_DESTINO_IMPRESSAO] " +
                                       ",[USUARIO_IMPRESSAO] " +
                                       ",[NR_FORMULARIO_CONTINGENCIA] " +
                                       ",[nome_arquivo_cfg_danfe] " +
                                       ",[csitconf] " +
                                       ",[ID_CLE] " +
                                       ",[ID_CONSULTA_DEST] " +
                                       ",[CTE_ID] " +
                                       ",[ID_CTE] " +
                                       ",[CONSULTA_STATUS_CUF] " +
                                       ",[BO_CONSULTA_STATUS_SVC] " +
                                       ",[IDE_MOD] " +
                                       ",[CHNFEREF] " +
                                       ",[GMT]" +
                                       ",[NREC]" +
                                       ",[CONFIG_EMAIL_SERVIDOR]" +
                                       ",[CONFIG_EMAIL_REMETENTE]" +
                                       ",[CONFIG_EMAIL_ASSUNTO]" +
                                       ",[CONFIG_EMAIL_MENSAGEM]" +
                                       ",[CONFIG_EMAIL_USUARIO]" +
                                       ",[CONFIG_EMAIL_SENHA]" +
                                       ",[CONFIG_EMAIL_AUTENTICACAO]" +
                                       ",[CONFIG_EMAIL_PORTA]" +
                                       ",[IN_ENVIOU_EMAIL]" +
                                       ",[IN_REMOVER_ACENTOS]" +
                                       ",[IN_VALIDAR_ESQUEMA]" +
                                       ",[TP_MODO_OPERACAO]" +
                                       ",[CERTIFICADO_CAMINHO]" +
                                       ",[CERTIFICADO_SENHA]" +
                                       ",[VERSAO_MANUAL]" +
                                       ",[VERSAO_MANUAL_DPEC_SCAN]" +
                                       ",[NR_LOTE]" +
                                       ",[TP_EVENTO]" +
                                       ",[NSU]" +
                                       ",[TP_NSU]" +
                                       ",[TX_CORRECAO]" +
                                       ",[CONTRIB_CNPJ_CPF_IE]) " +
                                 "VALUES " +
                                       "(@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26,@v27,@v29,@v30,@v31,@v32,@v33,@v34,@v35,@v36,@v37,@v38,@v39,@v40,@v41,@v42,@v43,@v44,@v45,@v46,@v47,@v48,@v49,@v50,@v51,@v52,@v53,@v54,@v55,@v56,@v57,@v58,@v59,@v60) ", Con);

                Cmd.Parameters.AddWithValue("@v2", obj.IMPRESSORA);
                Cmd.Parameters.AddWithValue("@v3", obj.TP_EVENTO_ERP);
                Cmd.Parameters.AddWithValue("@v4", obj.CHAVE_BUSCA);
                Cmd.Parameters.AddWithValue("@v5", obj.TP_ACAO);
                Cmd.Parameters.AddWithValue("@v6", obj.DH_EVENTO);
                Cmd.Parameters.AddWithValue("@v7", obj.TP_STATUS);
                Cmd.Parameters.AddWithValue("@v8", obj.TP_RETORNO);
                Cmd.Parameters.AddWithValue("@v9", obj.TP_RETORNO_CONTINGENCIA_LOCAL);
                Cmd.Parameters.AddWithValue("@v10", obj.NPROT);
                Cmd.Parameters.AddWithValue("@v11", obj.JUSTIFICATIVA);
                Cmd.Parameters.AddWithValue("@v12", obj.CHNFE);
                Cmd.Parameters.AddWithValue("@v13", obj.SERVER_NAME);
                Cmd.Parameters.AddWithValue("@v14", obj.TPAMB);
                Cmd.Parameters.AddWithValue("@v15", obj.CNPJ_EVENTO);
                Cmd.Parameters.AddWithValue("@v16", obj.COD_RETORNO);
                Cmd.Parameters.AddWithValue("@v17", obj.DESC_RETORNO);
                Cmd.Parameters.AddWithValue("@v18", obj.CONFIG_EMAIL_DESTINATARIO);
                Cmd.Parameters.AddWithValue("@v19", obj.COPIAS_IMPRESSAO);
                Cmd.Parameters.AddWithValue("@v20", obj.TP_IMP);
                Cmd.Parameters.AddWithValue("@v21", obj.LINK_PDF);
                Cmd.Parameters.AddWithValue("@v22", obj.BO_ATUALIZADO);
                Cmd.Parameters.AddWithValue("@v23", obj.TP_DESTINO_IMPRESSAO);
                Cmd.Parameters.AddWithValue("@v24", obj.USUARIO_IMPRESSAO);
                Cmd.Parameters.AddWithValue("@v25", obj.NR_FORMULARIO_CONTINGENCIA);
                Cmd.Parameters.AddWithValue("@v26", obj.nome_arquivo_cfg_danfe);
                Cmd.Parameters.AddWithValue("@v27", obj.csitconf);
                Cmd.Parameters.AddWithValue("@v29", obj.ID_CLE);
                Cmd.Parameters.AddWithValue("@v30", obj.ID_CONSULTA_DEST);
                Cmd.Parameters.AddWithValue("@v31", obj.CTE_ID);
                Cmd.Parameters.AddWithValue("@v32", obj.ID_CTE);
                Cmd.Parameters.AddWithValue("@v33", obj.CODIGO_UF);
                Cmd.Parameters.AddWithValue("@v34", obj.BO_CONSULTA_STATUS_SVC);
                Cmd.Parameters.AddWithValue("@v35", obj.IDE_MOD);
                Cmd.Parameters.AddWithValue("@v36", obj.CHNFEREF);
                Cmd.Parameters.AddWithValue("@v37", obj.GMT);
                Cmd.Parameters.AddWithValue("@v38", obj.NREC);
                Cmd.Parameters.AddWithValue("@v39", obj.CONFIG_EMAIL_SERVIDOR);
                Cmd.Parameters.AddWithValue("@v40", obj.CONFIG_EMAIL_REMETENTE);
                Cmd.Parameters.AddWithValue("@v41", obj.CONFIG_EMAIL_ASSUNTO);
                Cmd.Parameters.AddWithValue("@v42", obj.CONFIG_EMAIL_MENSAGEM);
                Cmd.Parameters.AddWithValue("@v43", obj.CONFIG_EMAIL_USUARIO);
                Cmd.Parameters.AddWithValue("@v44", obj.CONFIG_EMAIL_SENHA);
                Cmd.Parameters.AddWithValue("@v45", obj.CONFIG_EMAIL_AUTENTICACAO);
                Cmd.Parameters.AddWithValue("@v46", obj.CONFIG_EMAIL_PORTA);
                Cmd.Parameters.AddWithValue("@v47", obj.IN_ENVIOU_EMAIL);
                Cmd.Parameters.AddWithValue("@v48", obj.IN_REMOVER_ACENTOS);
                Cmd.Parameters.AddWithValue("@v49", obj.IN_VALIDAR_ESQUEMA);
                Cmd.Parameters.AddWithValue("@v50", obj.TP_MODO_OPERACAO);
                Cmd.Parameters.AddWithValue("@v51", obj.CERTIFICADO_CAMINHO);
                Cmd.Parameters.AddWithValue("@v52", obj.CERTIFICADO_SENHA);
                Cmd.Parameters.AddWithValue("@v53", obj.VERSAO_MANUAL);
                Cmd.Parameters.AddWithValue("@v54", obj.VERSAO_MANUAL_DPEC_SCAN);
                Cmd.Parameters.AddWithValue("@v55", obj.NR_LOTE);
                Cmd.Parameters.AddWithValue("@v56", obj.TP_EVENTO);
                Cmd.Parameters.AddWithValue("@v57", obj.NSU);
                Cmd.Parameters.AddWithValue("@v58", obj.TP_NSU);
                Cmd.Parameters.AddWithValue("@v59", obj.TX_CORRECAO);
                Cmd.Parameters.AddWithValue("@v60", obj.CONTRIB_CNPJ_CPF_IE);
                Cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar TECNO_EVENTO_ERP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public void AtualizarTECNO_EVENTO_ERP(string strNOME_CAMPO, string strVALOR_CAMPO, decimal decID_EVENTO_ERP)
        {
            try
            {
                if (strNOME_CAMPO != "" && strVALOR_CAMPO != "")
                {

                    AbrirConexao();
                    Cmd = new SqlCommand("UPDATE TECNO_EVENTO_ERP SET " +
                                            strNOME_CAMPO + "= @v2" + 
                                        " WHERE ID_EVENTO_ERP = @v3", Con);

                    Cmd.Parameters.AddWithValue("@v2", strVALOR_CAMPO);
                    Cmd.Parameters.AddWithValue("@v3", decID_EVENTO_ERP);
                    Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar TECNO_EVENTO_ERP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}



