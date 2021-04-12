using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Model;
using System.Data;

namespace DAL.Persistence
{
    public class EventoDocumentoDAL:Conexao
    {

        public void Inserir(EventoDocumento doc, decimal documento)
        {
            try
            {
                AbrirConexao();
                Cmd = new SqlCommand("insert into EVENTO_DO_DOCUMENTO (CD_DOCUMENTO, " +
                                                                    "CD_EVENTO," +
                                                                    "CD_SITUACAO," +
                                                                    "DT_HR_EVENTO," +
                                                                    "CD_MAQUINA," +
                                                                    "CD_USUARIO) " +
                "values (@v1,@v2,@v3,@v4, @v5, @v6);", Con);
                Cmd.Parameters.AddWithValue("@v1", documento);
                Cmd.Parameters.AddWithValue("@v2", doc.CodigoEvento);
                Cmd.Parameters.AddWithValue("@v3", doc.CodigoSituacao);
                Cmd.Parameters.AddWithValue("@v4", doc.DataHoraEvento);
                Cmd.Parameters.AddWithValue("@v5", doc.CodigoMaquina);
                Cmd.Parameters.AddWithValue("@v6", doc.CodigoUsuario);

                Cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar evento: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<EventoDocumento> ObterEventos(decimal CodDocumento)
        {
            try
            {

                AbrirConexao();
                string comando = "Select EVE.*, USUARIO.NM_COMPLETO,ESTACAO.NM_ESTACAO,TP.DS_TIPO" +
                                     " from EVENTO_DO_DOCUMENTO AS EVE LEFT JOIN" +
                                         " USUARIO ON USUARIO.CD_USUARIO = EVE.CD_USUARIO LEFT JOIN" +
                                        " HABIL_ESTACAO AS ESTACAO ON ESTACAO.CD_ESTACAO = EVE.CD_MAQUINA INNER JOIN"+
                                        " HABIL_TIPO AS TP ON TP.CD_TIPO = EVE.CD_SITUACAO"+
                                      " Where CD_DOCUMENTO = @v1 ";  

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", CodDocumento);

                Dr = Cmd.ExecuteReader();
                List<EventoDocumento> baixa = new List<EventoDocumento>();

                while (Dr.Read())
                {
                    EventoDocumento p = new EventoDocumento();
                    p.CodigoDocumento = Convert.ToDecimal(Dr["CD_DOCUMENTO"]);
                    p.CodigoEvento = Convert.ToInt32(Dr["CD_EVENTO"]);
                    p.CodigoSituacao = Convert.ToInt32(Dr["CD_SITUACAO"]);
                    p.DataHoraEvento = Convert.ToDateTime(Dr["DT_HR_EVENTO"]);
                    p.CodigoMaquina = Convert.ToInt32(Dr["CD_MAQUINA"]);
                    p.CodigoUsuario = Convert.ToInt32(Dr["CD_USUARIO"]);
                    p.Cpl_NomeUsuario = Convert.ToString(Dr["NM_COMPLETO"]);
                    p.Cpl_NomeMaquina = Convert.ToString(Dr["NM_ESTACAO"]);
                    p.Cpl_Situacao = Dr["DS_TIPO"].ToString();

                    baixa.Add(p);
                }
                return baixa;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar EVENTOS: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}
