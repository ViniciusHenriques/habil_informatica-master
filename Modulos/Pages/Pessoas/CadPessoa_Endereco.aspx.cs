using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using DAL.Model;
using DAL.Persistence;
using System.Data;


namespace SoftHabilInformatica.Pages.Pessoas
{
    public partial class CadPessoa_Endereco : System.Web.UI.Page
    {
        private List<Pessoa> listCadPessoa = new List<Pessoa>();
        private List<Pessoa_Endereco> listCadPessoaEndereco = new List<Pessoa_Endereco>();
        private List<Pessoa_Inscricao> listCadPessoaInscricao = new List<Pessoa_Inscricao>();
        private List<Pessoa_Contato> listCadPessoaContato = new List<Pessoa_Contato>();
        private clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void btnCanEndereco_Click(object sender, EventArgs e)

        {
            if (txtCodPessoa.Text == "0")
                Session["TabFocada"] = "contact";
            Session["CARREGOUCNPJ"] = null;
            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx?cad=" + Request.QueryString["Cad"]);
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
        }
        protected void BtnOkExcluir_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            pnlMensagem.Visible = false;
        }
        protected void txtcodcep_TextChanged(object sender, EventArgs e)
        {
            CEPDAL c = new CEPDAL();
            CEP c1 = new CEP();

            if (txtcodcep.Text == "")
                txtcodcep.Text = "0";
            Boolean blnCampoValido = false;
            v.CampoValido("CEP", txtcodcep.Text.Replace("-", "").Replace(".", "").Replace("/", ""), true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtcodcep.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtcodcep.Focus();
                }
                return;
            }

            c1 = c.PesquisarCEP(Convert.ToInt64(txtcodcep.Text.Replace("-", "").Replace(".", "")));

            if (c1 == null)
            {
                ShowMessage("CEP não cadastrado, clique no [+] para cadastrar", MessageType.Info);
                txtLogradouro.Text = "";
                txtComplemento.Text = "";

                lblCodEstado.Text = "";
                txtEstado.Text = "";

                lblCodMunicipio.Text = "";
                txtMunicipio.Text = "";

                lblCodBairro.Text ="";
                txtBairro.Text = "";
            }
            else
            {
                txtLogradouro.Text = c1.Logradouro;
                txtComplemento.Text = c1.Complemento;

                lblCodEstado.Text = c1.CodigoEstado.ToString();
                txtEstado.Text = c1.Sigla + " - " + c1.DescricaoEstado;

                lblCodMunicipio.Text = c1.CodigoMunicipio.ToString();
                txtMunicipio.Text = c1.DescricaoMunicipio;

                lblCodBairro.Text = c1.CodigoBairro.ToString();
                txtBairro.Text = c1.DescricaoBairro;
                txtNroEndereco.Focus();
            }
            
        }
        protected void btnExcEndereco_Click(object sender, EventArgs e)
        {
            int intEndItem = 0;
            
            if (txtEndItem.Text != "Novo")
            {
                intEndItem = Convert.ToInt32(txtEndItem.Text);

                if (intEndItem != 0)
                    listCadPessoaEndereco.RemoveAll(x => x._CodigoItem == intEndItem);

                Session["EnderecoPessoa"] = listCadPessoaEndereco;

                btnCanEndereco_Click(sender, e);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["EnderecoPessoa"] != null)
                listCadPessoaEndereco = (List<Pessoa_Endereco>)Session["EnderecoPessoa"];

            if (Session["CadPessoa"] != null)
                listCadPessoa = (List<Pessoa>)Session["CadPessoa"];

            if (Session["ContatoPessoa"] != null)
                listCadPessoaContato = (List<Pessoa_Contato>)Session["ContatoPessoa"];

            //if (Request.QueryString["cep"] != null)
            //{
            //    txtcodcep.Text = Request.QueryString["cep"].ToString();
            //    txtcodcep_TextChanged(sender, e);
            //}
            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = null;
            }


            foreach (Pessoa p in listCadPessoa)
            {
                txtCodPessoa.Text = p.CodigoPessoa.ToString();
                txtRazSocial.Text = p.NomePessoa;
            }

            if (Session["CARREGOUCNPJ"] == null)
            {

                if (Session["InscricaoPessoa"] != null)
                {
                    listCadPessoaInscricao = (List<Pessoa_Inscricao>)Session["InscricaoPessoa"];
                }
                else
                {
                    PessoaInscricaoDAL pi = new PessoaInscricaoDAL();
                    listCadPessoaInscricao = pi.ObterPessoaInscricoes(Convert.ToInt64(txtCodPessoa.Text));
                }


                DdlTipoInscricao.DataSource = listCadPessoaInscricao;
                DdlTipoInscricao.DataTextField = "_DcrInscricao";
                DdlTipoInscricao.DataValueField = "_CodigoItem";
                DdlTipoInscricao.DataBind();
                Session["CARREGOUCNPJ"] = "SIM";
            }
            if (Session["CadPessoa_Endereco"] != null)
            {
                string s = Session["CadPessoa_Endereco"].ToString();
                string[] words = s.Split('³');

                txtEndItem.Text = "";
                foreach (string word in words)
                {
                    if (txtEndItem.Text == "")
                        txtEndItem.Text = word;
                }

                Habil_TipoDAL RnSituacao = new Habil_TipoDAL();
                ddlTipoEndereco.DataSource = RnSituacao.TipoEndereco();
                ddlTipoEndereco.DataTextField = "DescricaoTipo";
                ddlTipoEndereco.DataValueField = "CodigoTipo";
                ddlTipoEndereco.DataBind();
                
                if (txtEndItem.Text != "Novo")
                {
                    btnExcEndereco.Visible = true;
                    foreach (Pessoa_Endereco p in listCadPessoaEndereco)
                    {
                        if (txtEndItem.Text == p._CodigoItem.ToString())
                        {
                            txtEndItem.Text = p._CodigoItem.ToString(); 
                            ddlTipoEndereco.SelectedValue = p._TipoEndereco.ToString();
                            txtcodcep.Text = p._CodigoCEP.ToString();
                            txtEstado.Text = p._DescricaoEstado;
                            txtBairro.Text = p._DescricaoBairro;
                            txtMunicipio.Text = p._DescricaoMunicipio;
                            txtLogradouro.Text = p._Logradouro;
                            txtNroEndereco.Text = p._NumeroLogradouro;
                            txtComplemento.Text = p._Complemento;
                            lblCodBairro.Text= p._CodigoBairro.ToString();
                            lblCodEstado.Text = p._CodigoEstado.ToString();
                            lblCodMunicipio.Text = p._CodigoMunicipio.ToString();

                            DdlTipoInscricao.SelectedValue = p._CodigoInscricao.ToString();
                            if(p._TipoEndereco == 5)
                            {
                                ddlTipoEndereco.Enabled = false;
                                btnExcEndereco.Visible = false;
                            }
                            else
                            {
                                RetirarTipoEnderecoEmpresa();
                            }
                        }
                    }
                }
                else
                {
                    ddlTipoEndereco_SelectedIndexChanged(sender, e);
                }
                Session["CadPessoa_Endereco"] = null;
            }
            if(Session["Endereco"] != null)
            {
                if (Session["InscricaoPessoa"] != null)
                {
                    listCadPessoaInscricao = (List<Pessoa_Inscricao>)Session["InscricaoPessoa"];
                }
                else
                {
                    PessoaInscricaoDAL pi = new PessoaInscricaoDAL();
                    listCadPessoaInscricao = pi.ObterPessoaInscricoes(Convert.ToInt64(txtCodPessoa.Text));
                }
                Habil_TipoDAL RnSituacao = new Habil_TipoDAL();
                ddlTipoEndereco.DataSource = RnSituacao.TipoEndereco();
                ddlTipoEndereco.DataTextField = "DescricaoTipo";
                ddlTipoEndereco.DataValueField = "CodigoTipo";
                ddlTipoEndereco.DataBind();

                DdlTipoInscricao.DataSource = listCadPessoaInscricao;
                DdlTipoInscricao.DataTextField = "_DcrInscricao";
                DdlTipoInscricao.DataValueField = "_CodigoItem";
                DdlTipoInscricao.DataBind();
                Pessoa_Endereco p = (Pessoa_Endereco)Session["Endereco"];
                if (p._CodigoItem != 0)
                    txtEndItem.Text = p._CodigoItem.ToString();
                else
                    txtEndItem.Text = "Novo";

                ddlTipoEndereco.SelectedValue = p._TipoEndereco.ToString();
                DdlTipoInscricao.SelectedValue = p._CodigoInscricao.ToString();
                Session["Endereco"] = null;
            }
            txtcodcep.Focus();
            if (txtEndItem.Text == "")
                btnCanEndereco_Click(sender, e);
        }
        protected Boolean ValidaTela()
        {

            Boolean blnCampoValido = false;
            strMensagemR = "";

            foreach (Pessoa_Endereco valor in listCadPessoaEndereco)
            {
                string[] ins = DdlTipoInscricao.SelectedItem.Text.Split('(', ')');

                if (valor._CodigoInscricao.ToString().Equals(DdlTipoInscricao.SelectedValue) && (valor._CodigoItem.ToString() != txtEndItem.Text) && ins[1] == "4")
                {
                    ShowMessage("Inscrição já consta em outro Endereço.", MessageType.Info);
                    return false;
                }
            }

            v.CampoValido("CEP", txtcodcep.Text.Replace("-", "").Replace(".", "").Replace("/", ""), true, true, true, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtcodcep.Focus();
                }
                return false;
            }

            v.CampoValido("Estado", lblCodEstado.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtEstado.Focus();
                }
                return false;
            }

            v.CampoValido("Estado", txtEstado.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtEstado.Focus();
                }
                return false;
            }

            v.CampoValido("Município", lblCodMunicipio.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtMunicipio.Focus();
                }
                return false;
            }

            v.CampoValido("Município", txtMunicipio.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtMunicipio.Focus();
                }
                return false;
            }

            v.CampoValido("Bairro", lblCodBairro.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtBairro.Focus();
                }
                return false;
            }

            v.CampoValido("Bairro", txtBairro.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtBairro.Focus();
                }
                return false;
            }


            v.CampoValido("Logradouro", txtLogradouro.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtLogradouro.Focus();
                }
                return false;
            }

            v.CampoValido("Número do Endereço", txtNroEndereco.Text, true, false, false, false, "", ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                txtRazSocial.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtNroEndereco.Focus();
                }
                return false;
            }

            return true;
        }
        protected void btnSlvEndereco_Click(object sender, EventArgs e)
        {
            int intEndItem = 0;
       


            if (!ValidaTela())
                return;

            if (txtEndItem.Text != "Novo")
                intEndItem = Convert.ToInt32(txtEndItem.Text);
            else
            {
                if (listCadPessoaEndereco.Count != 0)
                    intEndItem = Convert.ToInt32(listCadPessoaEndereco.Max(x => x._CodigoItem).ToString());

                intEndItem = intEndItem + 1;
            }

            String strValor = txtcodcep.Text;
            strValor = strValor.Replace(".", "");
            strValor = strValor.Replace("/", "");
            strValor = strValor.Replace("-", "");


            Pessoa_Endereco x1 = new Pessoa_Endereco(intEndItem, Convert.ToInt32(ddlTipoEndereco.Text), 
                txtLogradouro.Text, txtNroEndereco.Text, txtComplemento.Text,  
                Convert.ToInt64(strValor), Convert.ToInt32(lblCodEstado.Text), txtEstado.Text, 
                Convert.ToInt64(lblCodMunicipio.Text) , txtMunicipio.Text, 
                Convert.ToInt32(lblCodBairro.Text), txtBairro.Text, Convert.ToInt32(DdlTipoInscricao.SelectedValue));

            if (intEndItem != 0)
                listCadPessoaEndereco.RemoveAll(x => x._CodigoItem == intEndItem);

            listCadPessoaEndereco.Add(x1);

            Session["EnderecoPessoa"] = listCadPessoaEndereco;
            btnCanEndereco_Click(sender, e);


        }
        protected void btnAddUnidade_Click(object sender, EventArgs e)
        {
            if (txtcodcep.Text != "")
            {
                CEP cep2 = new CEP();
                CEPDAL cepDAL = new CEPDAL();
                cep2 = cepDAL.PesquisarCEP(Convert.ToInt64(txtcodcep.Text.Replace("-", "").Replace(".", "")));

                if (cep2 == null)
                {
                    CEP cep = new CEP();
                    ViaCEP(sender, e);
                    if (txtMunicipio.Text == "")
                        return;
                    Municipio muni = new Municipio();
                    MunicipioDAL muniDAL = new MunicipioDAL();
                    muni = muniDAL.PesquisarMunicipio(Convert.ToInt32(lblCodMunicipio.Text));

                    if (muni == null)
                    {
                        Municipio muni2 = new Municipio();
                        muni2.CodigoMunicipio = Convert.ToInt32(lblCodMunicipio.Text);
                        muni2.DescricaoMunicipio = txtMunicipio.Text;

                        string[] valor = txtEstado.Text.Split(" ".ToCharArray());

                        Estado est = new Estado();
                        EstadoDAL estDAL = new EstadoDAL();

                        est = estDAL.PesquisarEstadoUF(valor[1]);

                        muni.CodigoEstado = est.CodigoEstado;
                        muni2.Sigla = valor[1];
                        muniDAL.Inserir(muni2);

                    }
                    muni = muniDAL.PesquisarMunicipio(Convert.ToInt32(lblCodMunicipio.Text));
                    cep.CodigoCEP = Convert.ToInt64(txtcodcep.Text.Replace("-", "").Replace(".", ""));
                    cep.DescricaoEstado = txtEstado.Text;
                    cep.DescricaoMunicipio = txtMunicipio.Text;
                    cep.DescricaoBairro = txtBairro.Text;
                    cep.Logradouro = txtLogradouro.Text;
                    cep.Complemento = txtComplemento.Text;

                    lblCodEstado.Text = muni.CodigoEstado.ToString();
                    lblCodMunicipio.Text = muni.CodigoMunicipio.ToString();

                    cep.CodigoEstado = Convert.ToInt32(lblCodEstado.Text);
                    cep.CodigoMunicipio = Convert.ToInt32(lblCodMunicipio.Text);

                    Bairro bairro = new Bairro();
                    BairroDAL bairroDAL = new BairroDAL();

                    bairro.DescricaoBairro = txtBairro.Text;
                    if (txtBairro.Text != " ")
                    {
                        Bairro bairro2 = new Bairro();
                        bairro2.DescricaoBairro = txtBairro.Text;
                        bairro2 = bairroDAL.PesquisarBairroDescricao(bairro2.DescricaoBairro.ToUpper());
                        if (bairro2.CodigoBairro == 0)
                            bairroDAL.Inserir(bairro);

                        bairro = bairroDAL.PesquisarBairroDescricao(bairro.DescricaoBairro);
                        cep.CodigoBairro = bairro.CodigoBairro;

                    }

                    cepDAL.Inserir(cep);
                    txtcodcep_TextChanged(sender, e);
                }
                else
                {
                    ShowMessage("CEP já Cadastrado", MessageType.Info);
                    txtcodcep_TextChanged(sender, e);
                }
            }
            else
            {
                ShowMessage("Digite seu CEP", MessageType.Info);
            }
        }
        protected void ViaCEP(object sender, EventArgs e)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + txtcodcep.Text + "/json/");
                request.AllowAutoRedirect = false;
                HttpWebResponse ChecaServidor = ChecaServidor = (HttpWebResponse)request.GetResponse();
                 

                if (ChecaServidor.StatusCode != HttpStatusCode.OK)
                {
                    ShowMessage("Servidor indisponível", MessageType.Info);
                    return; // Sai da rotina
                }

                using (Stream webStream = ChecaServidor.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            response = Regex.Replace(response, "[{},]", string.Empty);
                            response = response.Replace("\"", "");

                            String[] substrings = response.Split('\n');

                            int cont = 0;
                            foreach (var substring in substrings)
                            {
                                if (cont == 1)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    if (valor[0] == "  erro")
                                    {
                                        ShowMessage("CEP não encontrado", MessageType.Info);
                                        txtcodcep.Focus();
                                        return;
                                    }
                                }

                                //Logradouro
                                if (cont == 2)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txtLogradouro.Text = valor[1];
                                }

                                //Complemento
                                if (cont == 3)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txtComplemento.Text = valor[1];
                                }

                                //Bairro
                                if (cont == 4)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    
                                    txtBairro.Text = valor[1];
                                    if (txtBairro.Text == " ")
                                        txtBairro.Text = "SEM BAIRRO";
                                }

                                //Localidade (Cidade)
                                if (cont == 5)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    
                                    txtMunicipio.Text = valor[1];
                                   
                                }

                                //Estado (UF)
                                if (cont == 6)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    
                                    txtEstado.Text = valor[1];
                                    



                                }
                                if (cont == 7)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());

                                    lblCodMunicipio.Text = valor[1];

                                }
                                if (cont == 8)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    if(lblCodMunicipio.Text == " ")
                                        lblCodMunicipio.Text = valor[1];




                                }

                                cont++;
                            }
                            
                        }
                    }
                }
                

            }
            catch (WebException ex)
            {

                ShowMessage("CEP Inexistente. " + ex.Message.ToString(), MessageType.Info);
                txtBairro.Text = "";
                txtcodcep.Text = "";
                txtEstado.Text = "";
                txtMunicipio.Text = "";
                txtLogradouro.Text = "";
            }

        }

        protected void ddlTipoEndereco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlTipoInscricao.SelectedItem.Text == null)
                return;
            string[] array = DdlTipoInscricao.SelectedItem.Text.Split('(', ')');

            if (array[1] == "3")
            {
                IEnumerable<Pessoa_Endereco> SelectEndereco = listCadPessoaEndereco.Where((Pessoa_Endereco c) => { return c._CodigoInscricao == Convert.ToInt32(DdlTipoInscricao.SelectedValue); });
                if (SelectEndereco.Count() == 0)
                {
                    ddlTipoEndereco.SelectedValue = "14";
                    ddlTipoEndereco.Enabled = false;
                }
                else
                {
                    RetirarTipoEnderecoEmpresa();
                }               
            }
        }
        protected void RetirarTipoEnderecoEmpresa()
        {
            List<Habil_Tipo> list = (List<Habil_Tipo>)ddlTipoEndereco.DataSource;
            IEnumerable<Habil_Tipo> TipoEndereco = list.Where((Habil_Tipo c) => { return c.CodigoTipo != 5; });

            ddlTipoEndereco.DataSource = TipoEndereco;
            ddlTipoEndereco.DataBind();
        }
    }
}