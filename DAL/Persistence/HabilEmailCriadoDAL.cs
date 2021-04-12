using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Persistence;
using DAL.Model;

namespace DAL
{
    public class HabilEmailCriadoDAL : Conexao  
    {
        private string strSQL = "";
        string strSQL2 = "";
        public void Inserir(HabilEmailCriado p, ref long longCodigoIndexEmail)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [HABIL_EMAIL_CRIADO] " +
                    " (CD_USU_REMETENTE, TX_ASSUNTO, IN_HTML, TX_CORPO, CD_SITUACAO) " +
                    " values ( @v1, @v2, @v3, @v4, @v5); SELECT SCOPE_IDENTITY()";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CD_USU_REMETENTE);
                Cmd.Parameters.AddWithValue("@v2", p.TX_ASSUNTO );
                Cmd.Parameters.AddWithValue("@v3", p.IN_HTML);
                Cmd.Parameters.AddWithValue("@v4", p.TX_CORPO);
                Cmd.Parameters.AddWithValue("@v5", p.CD_SITUACAO);

                //                Cmd.ExecuteNonQuery();
                p.CD_INDEX = Convert.ToInt64(Cmd.ExecuteScalar());
                longCodigoIndexEmail = p.CD_INDEX;
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
                            throw new Exception("Erro ao incluir Habil Email Criado: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar Usuário: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public Boolean Gera_Email(List<HabilEmailCriado> lstMail, List<HabilEmailDestinatario> lstDest, List<HabilEmailAnexo> lstAnexo, ref long longCodigoIndexEmail)
        {
            try
            {
                HabilEmailAnexoDAL HeAnexoDal = new HabilEmailAnexoDAL();
                HabilEmailDestinatarioDAL HeDestDal = new HabilEmailDestinatarioDAL();


                foreach (var item in lstMail)
                {
                    Inserir(item, ref longCodigoIndexEmail);

                    foreach (var item2 in lstDest)
                    {
                        item2.CD_INDEX = item.CD_INDEX;
                        HeDestDal.InserirDestinatarios(item2);
                    }

                    foreach (var item3 in lstAnexo)
                    {
                        item3.CD_INDEX = item.CD_INDEX;
                        HeAnexoDal.InserirAnexos(item3);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;

                throw new Exception(ex.Message.ToString());
            }
        }
        public Boolean Grava_ColecaoEmail(HabilEmailCriado lstMail, bool blnEnviaEmail=false)
        {
            try
            {
                HabilEmailAnexoDAL HeAnexoDal = new HabilEmailAnexoDAL();
                HabilEmailDestinatarioDAL HeDestDal = new HabilEmailDestinatarioDAL();

                int intDestinatario = 0;
                long longCodigoIndexEmail = 0;
                if (lstMail.CD_INDEX == 0)
                    Inserir(lstMail,ref longCodigoIndexEmail);
                else
                {
                    Atualizar(lstMail);
                    HeDestDal.ExcluirDestinatarios(lstMail.CD_INDEX);
                    HeAnexoDal.ExcluirAnexos(lstMail.CD_INDEX);
                }
                foreach (var   item2 in lstMail.listaDestinatarios)
                {
                    item2.CD_INDEX = lstMail.CD_INDEX;
                    item2.TP_DESTINATARIO = 1;
                    intDestinatario += 1;
                    item2.CD_EMAIL_DESTINATARIO = intDestinatario;
                    HeDestDal.InserirDestinatarios(item2);
                }

                foreach (var item3 in lstMail.listaDestinatariosComCopia)
                {
                    item3.CD_INDEX = lstMail.CD_INDEX;
                    item3.TP_DESTINATARIO = 2;
                    intDestinatario += 1;
                    item3.CD_EMAIL_DESTINATARIO = intDestinatario;
                    HeDestDal.InserirDestinatarios(item3);
                }

                foreach (var item4 in lstMail.listaDestinatariosComCopiaOculta)
                {
                    item4.CD_INDEX = lstMail.CD_INDEX;
                    item4.TP_DESTINATARIO = 3;
                    intDestinatario += 1;
                    item4.CD_EMAIL_DESTINATARIO = intDestinatario;
                    HeDestDal.InserirDestinatarios(item4);
                }

                foreach (var item5 in lstMail.listaAnexos)
                {
                    item5.CD_INDEX = lstMail.CD_INDEX;
                    HeAnexoDal.InserirAnexos(item5);
                }

                if (blnEnviaEmail)
                {
                    lstMail.CD_SITUACAO = 113; //Envia Imediatamente
                    AtualizaParaEnvio(lstMail);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;

                throw new Exception(ex.Message.ToString());
            }
        }

        public List<HabilEmailCriado> ListarEmails(List<DBTabelaCampos> ListaFiltros)
        {
            List<HabilEmailCriado> lista = new List<HabilEmailCriado>();
            HabilEmailDestinatarioDAL listaDestiDAL = new HabilEmailDestinatarioDAL();
            HabilEmailAnexoDAL listaAnexoDAL = new HabilEmailAnexoDAL();


            try
            {
                AbrirConexao();

                string strSQL = "Select * from [VW_EMAILS] ";
                string strValor = MontaFiltroIntervalo(ListaFiltros);
                strSQL = strSQL + strValor + " ORDER BY DT_LANCAMENTO DESC ";
                

                Cmd = new SqlCommand(strSQL, Con);
                Dr = Cmd.ExecuteReader();


                while (Dr.Read())
                {
                    HabilEmailCriado p = new HabilEmailCriado();

                    foreach (HabilEmailCriado item in lista)
                    {
                        if (item.CD_INDEX == Convert.ToInt64(Dr["CD_INDEX"]))
                        {
                            p.CD_INDEX = item.CD_INDEX;
                            p.DT_LANCAMENTO = item.DT_LANCAMENTO;
                            p.DT_ENVIO = item.DT_ENVIO;
                            p.TX_ASSUNTO = item.TX_ASSUNTO;
                            p.CD_SITUACAO = item.CD_SITUACAO;
                            p.CD_USU_REMETENTE = item.CD_USU_REMETENTE;
                            p.CPL_DESTINATARIOS = item.CPL_DESTINATARIOS;
                            p.CPL_SITUACAO = item.CPL_SITUACAO;
                            p.TX_CORPO = item.TX_CORPO;
                            p.NR_TENTA_ENVIO = item.NR_TENTA_ENVIO;

                            p.listaDestinatarios = item.listaDestinatarios;
                            p.listaDestinatariosComCopia = item.listaDestinatariosComCopia;
                            p.listaDestinatariosComCopiaOculta = item.listaDestinatariosComCopiaOculta;
                            p.listaAnexos = item.listaAnexos;

                            p.TX_ERRO = item.TX_ERRO + "";


                            if (p.CPL_DESTINATARIOS.Contains(Convert.ToString(Dr["TX_EMAIL"])) == false)
                                p.CPL_DESTINATARIOS += "; \n (" + Convert.ToString(Dr["NM_DESTINATARIO"]) + ") " + Convert.ToString(Dr["TX_EMAIL"]);

                            lista.RemoveAll(x => x.CD_INDEX == item.CD_INDEX);
                            goto GeraAlteracao;
                        }
                    }

                    p.CD_INDEX= Convert.ToInt64(Dr["CD_INDEX"]);
                    p.DT_LANCAMENTO = Convert.ToDateTime(Dr["DT_LANCAMENTO"]);

                    if (Dr["DT_ENVIO"] != DBNull.Value) 
                        p.DT_ENVIO = Convert.ToDateTime(Dr["DT_ENVIO"]);

                    if (Dr["NR_TENTA_ENVIO"] != DBNull.Value)
                        p.NR_TENTA_ENVIO = Convert.ToInt32(Dr["NR_TENTA_ENVIO"]);

                    p.TX_ERRO = "";
                    if (Dr["TX_ERRO"] != DBNull.Value)
                        p.TX_ERRO = Convert.ToString(Dr["TX_ERRO"]);

                    p.TX_CORPO = "";
                    if (Dr["TX_CORPO"] != DBNull.Value)
                        p.TX_CORPO = Convert.ToString(Dr["TX_CORPO"]);

                    p.TX_ASSUNTO = Convert.ToString(Dr["TX_ASSUNTO"]);
                    p.CD_USU_REMETENTE = Convert.ToInt64(Dr["CD_USU_REMETENTE"]);
                    p.CPL_DESTINATARIOS = " (" + Convert.ToString(Dr["NM_DESTINATARIO"]) + ") " + Convert.ToString(Dr["TX_EMAIL"]);

                    p.CPL_SITUACAO = Convert.ToString(Dr["DS_SITUACAO"]);
                    p.CD_SITUACAO = Convert.ToInt64(Dr["CD_SITUACAO"]);

                    p.listaDestinatarios = listaDestiDAL.ListarDestinatarioPara(p.CD_INDEX);
                    p.listaDestinatariosComCopia = listaDestiDAL.ListarDestinatarioCC(p.CD_INDEX);
                    p.listaDestinatariosComCopiaOculta = listaDestiDAL.ListarDestinatarioCCO(p.CD_INDEX);
                    p.listaAnexos = listaAnexoDAL.ListarAnexos(p.CD_INDEX);

                GeraAlteracao:

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Todos Emails: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }



        }

        public void AtualizaParaEnvio(HabilEmailCriado p)
        {
            try
            {
                AbrirConexao();

                strSQL = "UPDATE [HABIL_EMAIL_CRIADO] " +
                    " SET CD_SITUACAO = @v5 " +
                    " WHERE CD_INDEX =  @v1";

                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", p.CD_INDEX);
                Cmd.Parameters.AddWithValue("@v5", p.CD_SITUACAO);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao gravar HABIL_EMAIL_CRIADO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }

        public void Atualizar(HabilEmailCriado p)
        {
            try
            {


                AbrirConexao();
                strSQL = "update [HABIL_EMAIL_CRIADO] set [TX_ASSUNTO] = @v2, [CD_SITUACAO] = @v5, [TX_CORPO] = @v4 , [IN_HTML] = @v3   Where [CD_index] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CD_INDEX);
                Cmd.Parameters.AddWithValue("@v2", p.TX_ASSUNTO);
                Cmd.Parameters.AddWithValue("@v3", p.IN_HTML);
                Cmd.Parameters.AddWithValue("@v4", p.TX_CORPO);
                Cmd.Parameters.AddWithValue("@v5", p.CD_SITUACAO);

                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar HABIL_EMAIL_CRIADO: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }

    }

}
