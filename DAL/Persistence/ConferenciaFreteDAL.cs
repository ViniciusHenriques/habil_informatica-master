using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DAL.Persistence
{
    public class ConferenciaFreteDAL
    {
        protected SqlConnection Con;

        protected SqlCommand Cmd;

        protected SqlDataReader Dr;

        protected void AbrirConexao()
        {
            try
            {
                Con = new SqlConnection(@"Data Source=192.168.0.18\SQLserver2008;Initial Catalog=Fabesul;User ID=sa;Password=habil");
                
                Con.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void FecharConexao()
        {
            try
            {
                Con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Busca NFe(nfiscal) por chave de acesso 
        /// </summary>
        /// <param name="ChaveAcesso"></param>
        /// <returns></returns>
        public DataTable PesquisarPorChaveAcessoQuery1(string strChaveAcesso, string strCodigoEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();

                AbrirConexao();

                string comando = "select nume, vtnf, pesb, cubagem, fret,tipf,dtnf from nfiscal where nume= @v1 and cemp = @v2 and seri = '03' ";

            
                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", strChaveAcesso.Substring(25, 9));

                if(strChaveAcesso.Substring(0, 2) == "41")
                    Cmd.Parameters.AddWithValue("@v2", "04");
                else if (strChaveAcesso.Substring(0, 2) == "42")
                    Cmd.Parameters.AddWithValue("@v2", "02");
                else
                    Cmd.Parameters.AddWithValue("@v2", "01");
                Cmd.CommandTimeout = 10000;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);

                FecharConexao();

                return dt;

                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Busca NFe(entrada) por chave de acesso
        /// </summary>
        /// <param name="ChaveAcesso"></param>
        /// <returns></returns>
        public DataTable PesquisarPorChaveAcessoQuery2(string strChaveAcesso, string strCodigoEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();

                AbrirConexao();

                string comando = "select e.nlan, e.vtnf, sum(i.qtde * p.peso) as PesoNF, Sum(i.qtde * p.cubagem) as CubNF ,e.fret , e.dtnf from entrada as e " +
                                     "inner join entitem as i " +
                                       "on e.nlan = i.nlan " +
                                    "inner join produt as p " +
                                       "on p.cpro = i.cpro " +
                                    "where chavenfe = @v1 and cemp =@v2 and seri = '03'" +
                                    "group by e.nlan, e.vtnf,e.fret,e.dtnf";

                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", strChaveAcesso);
                if (strChaveAcesso.Substring(0, 2) == "41")
                    Cmd.Parameters.AddWithValue("@v2", "04");
                else if (strChaveAcesso.Substring(0, 2) == "42")
                    Cmd.Parameters.AddWithValue("@v2", "02");
                else
                    Cmd.Parameters.AddWithValue("@v2", "01");
                Cmd.CommandTimeout = 10000;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dt);
                FecharConexao();
                return dt;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Busca Fatura pela chave de acesso do CT-e
        /// </summary>
        /// <param name="ChaveAcesso"> Chave de Acesso do CT-e</param>
        /// <returns></returns>
        public string PesquisarNumeroFatura(string ChaveAcesso)
        {
            try
            {
                string NumeroFatura = "";
                AbrirConexao();

                string comando = "select Fatura.nume from fatitem " + 
                                    "inner join Fatura on Fatura.nlan = Fatitem.nlan " +
                                 "where chavecte = @v1 ";

                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", ChaveAcesso);
                Cmd.CommandTimeout = 10000;
                Dr = Cmd.ExecuteReader();

                int contador = 0;
                if (Dr.Read())
                {
                    contador++;
                    if(contador == 1)
                        NumeroFatura += Dr["nume"].ToString();
                    else
                        NumeroFatura += ", " + Dr["nume"].ToString();
                }
                FecharConexao();
                return NumeroFatura;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string PesquisarNumeroNotasClienteMesmaData(string strChaveAcesso, string strCodigoEmpresa)
        {
            try
            {
                string QuantidadeNFsClienteEmbarque = "";
                AbrirConexao();

                string comando = "select " +
                                        "count(distinct n2.nume) as contagem " +
                                 "from nfiscal as n with(nolock) " +
                                        "left join NFEmbarque as em with(nolock) on em.nume = n.nume and em.cemp= n.cemp and em.seri= n.seri " +
                                        "inner join nfiscal as n2 with(nolock) on n2.ccli = n.ccli " +
                                            "and n2.cemp = n.cemp " +
                                            "and n2.ctra = n.ctra " +
                                        "inner join NFEmbarque as em2 with(nolock) on em2.nume = n2.nume and em2.seri = n2.seri and em2.cemp = n2.cemp " +
                                            "and em2.ccli = n2.ccli " +
                                            "and em2.cemp = n2.cemp " +
                                 "where n.nume= @v1 "+
                                        "and n.cemp = @v2 "+
                                        "and n.seri = '03' " +
                                        "and em2.dtsa = em.dtsa ";

                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", strChaveAcesso.Substring(25,9));

                if (strChaveAcesso.Substring(0, 2) == "41")
                    Cmd.Parameters.AddWithValue("@v2", "04");
                else if (strChaveAcesso.Substring(0, 2) == "42")
                    Cmd.Parameters.AddWithValue("@v2", "02");
                else
                    Cmd.Parameters.AddWithValue("@v2", "01");
                Cmd.CommandTimeout = 10000;
                Dr = Cmd.ExecuteReader();

                if (Dr.Read())
                {
                    if (Convert.ToInt32(Dr["contagem"].ToString()) > 1)
                        QuantidadeNFsClienteEmbarque += Dr["contagem"].ToString();
                    else
                        QuantidadeNFsClienteEmbarque = "-";

                }
                FecharConexao();
                return QuantidadeNFsClienteEmbarque;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
