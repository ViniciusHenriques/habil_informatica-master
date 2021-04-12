using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Model;
using System.Data;
using System.Data.SqlClient;


namespace DAL.Persistence
{
    public class CEPDAL:Conexao
    {
        protected string strSQL = "";
        public CEP PesquisarCEP(Int64 intCEP)
        {
            try
            {
                AbrirConexao();
                strSQL = "Select * from [CEP] Where CD_CEP = @v1";
                Cmd = new SqlCommand(strSQL, Con);
                Cmd.Parameters.AddWithValue("@v1", intCEP);

                Dr = Cmd.ExecuteReader();

                CEP p = null;

                if (Dr.Read())
                {
                    p = new CEP();

                    p.CodigoCEP = Convert.ToInt64(Dr["CD_CEP"]);
                    p.Logradouro = Convert.ToString(Dr["DS_Logradouro"]);
                    p.Complemento = Convert.ToString(Dr["DS_Complemento"]);

                    p.CodigoEstado = Convert.ToInt32(Dr["CD_Estado"]);
                    EstadoDAL e = new EstadoDAL();
                    Estado e1 = new Estado();
                    e1 = e.PesquisarEstado(p.CodigoEstado);
                    if (e1 != null)
                    {
                        p.DescricaoEstado = e1.DescricaoEstado;
                        p.Sigla = e1.Sigla;
                    }
                    else
                    {
                        p.DescricaoEstado = "";
                        p.Sigla = "";
                    }

                    p.CodigoMunicipio = Convert.ToInt64(Dr["CD_MUNICIPIO"]);
                    MunicipioDAL m = new MunicipioDAL();
                    Municipio m1 = new Municipio();
                    m1 = m.PesquisarMunicipio(p.CodigoMunicipio);
                    if (e1 != null)
                        p.DescricaoMunicipio = m1.DescricaoMunicipio;
                    else
                        p.DescricaoMunicipio = "";

                    p.CodigoBairro = Convert.ToInt32(Dr["CD_BAIRRO"]);
                    BairroDAL b = new BairroDAL();
                    Bairro b1 = new Bairro();
                    b1 = b.PesquisarBairro(p.CodigoBairro);
                    if (b1 != null)
                        p.DescricaoBairro = b1.DescricaoBairro;
                    else
                        p.DescricaoBairro = "";

                }

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar CEP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }

        }
        public void Inserir(CEP p)
        {
            try
            {
                AbrirConexao();

                strSQL = "insert into [CEP] (CD_CEP, DS_LOGRADOURO, DS_COMPLEMENTO, CD_ESTADO, CD_MUNICIPIO, CD_BAIRRO) values (@v1, @v2, @v3, @v4, @v5, @v6)";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoCEP);
                Cmd.Parameters.AddWithValue("@v2", p.Logradouro);
                Cmd.Parameters.AddWithValue("@v3", p.Complemento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoEstado);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoBairro);

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
                            throw new Exception("Erro ao gravar CEP: " + ex.Message.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar CEP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }
        }
        public void Atualizar(CEP p)
        {
            try
            {
                AbrirConexao();
                strSQL = "update [CEP] set [DS_Logradouro] = @v2, [DS_Complemento] = @v3, [CD_ESTADO] = @v4, [CD_MUNICIPIO] = @v5, [CD_BAIRRO] = @v6 Where [CD_CEP] = @v1";
                Cmd = new SqlCommand(strSQL, Con);

                Cmd.Parameters.AddWithValue("@v1", p.CodigoCEP);
                Cmd.Parameters.AddWithValue("@v2", p.Logradouro);
                Cmd.Parameters.AddWithValue("@v3", p.Complemento);
                Cmd.Parameters.AddWithValue("@v4", p.CodigoEstado);
                Cmd.Parameters.AddWithValue("@v5", p.CodigoMunicipio);
                Cmd.Parameters.AddWithValue("@v6", p.CodigoBairro);
                
                Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar CEP: " + ex.Message.ToString());
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

                Cmd = new SqlCommand("delete from [CEP] Where [CD_CEP] = @v1", Con);

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
                            throw new Exception("Erro ao excluir CEP: " + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao excluir CEP: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();

            }


        }

    }
}
