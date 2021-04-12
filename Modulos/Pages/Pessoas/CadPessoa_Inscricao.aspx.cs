using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Model;
using DAL.Persistence;

namespace SoftHabilInformatica.Pages.Pessoas
{
    public partial class CadPessoa_Inscricao : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        public string strMensagemR = "";
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void CarregaDropDownList()
        {
            PaisDAL paisDAL = new PaisDAL();
            ddlPaises.DataSource = paisDAL.ListarPaises("", "", "", "");
            ddlPaises.DataTextField = "DescricaoPais";
            ddlPaises.DataValueField = "CodigoPais";
            ddlPaises.DataBind();

            ddlPaises.SelectedValue = "1058";
        }
        private List<Pessoa> listCadPessoa = new List<Pessoa>();
        private List<Pessoa_Inscricao> listCadPessoaInscricao = new List<Pessoa_Inscricao>();
        private List<Pessoa_Endereco> listCadPessoaEndereco = new List<Pessoa_Endereco>();
        private List<Pessoa_Contato> listCadPessoaContato = new List<Pessoa_Contato>();
        protected bool ValidaTela()
        {

            Boolean blnCampoValido = false;
            strMensagemR = "";

            if (ddlPaises.SelectedValue == "1058")
            {
                v.CampoValido("C.N.P.J ou C.P.F", txtCNPJCPF.Text.Replace("-", "").Replace(".", "").Replace("/", ""), true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

                if (!blnCampoValido)
                {
                    txtCNPJCPF.Text = "";
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtCNPJCPF.Focus();

                    }
                    return false;
                }
            
                if (ddlTipoInscricao.SelectedValue == "3")
                {
                    blnCampoValido = v.ValidaCNPJ(txtCNPJCPF.Text);
                    if (!blnCampoValido)
                        strMensagemR = "C.N.P.J incorreto !!!";
                }
                else
                {
                    blnCampoValido = v.ValidaCPF(txtCNPJCPF.Text);
                    if (!blnCampoValido)
                        strMensagemR = "C.P.F incorreto !!!";
                }
            }

            

            if (strMensagemR != "")
            {
                ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }

            blnCampoValido = false;
            v.CampoDataValido("Data da Abertura / Data de Nascimento", txtdtabertura.Text, true, ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                    ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }

            blnCampoValido = false;
            v.CampoDataValido("Data de Encerramento", txtdtencerramento.Text, false, ref blnCampoValido, ref strMensagemR);
            if (!blnCampoValido)
            {
                if (strMensagemR != "")
                    ShowMessage(strMensagemR, MessageType.Error);
                return false;
            }

            return true;
        }
        protected bool ValidaExclusao()
        {
            foreach (Pessoa_Endereco valor in listCadPessoaEndereco)
            {
                if (valor._CodigoInscricao == Convert.ToInt32(txtInsItem.Text))
                {
                    return false;
                }
            }

            foreach (Pessoa_Contato valor1 in listCadPessoaContato)
            {
                if (valor1._CodigoInscricao == Convert.ToInt32(txtInsItem.Text))
                {
                    return false;
                }
            }


            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
                return;

            if (Session["InscricaoPessoa"] != null)
                listCadPessoaInscricao = (List<Pessoa_Inscricao>)Session["InscricaoPessoa"];

            if (Session["CadPessoa"] != null)
                listCadPessoa = (List<Pessoa>)Session["CadPessoa"];

            if (Session["EnderecoPessoa"] != null)
                listCadPessoaEndereco = (List<Pessoa_Endereco>)Session["EnderecoPessoa"];

            if (Session["ContatoPessoa"] != null)
                listCadPessoaContato  = (List<Pessoa_Contato>)Session["ContatoPessoa"];

            foreach (Pessoa p in listCadPessoa)
            {
                txtCodPessoa.Text= p.CodigoPessoa.ToString();
                txtRazSocial.Text = p.NomePessoa;
            }


            if (Session["CadPessoa_Inscricao"] != null)
            {
                string s = Session["CadPessoa_Inscricao"].ToString();
                string[] words = s.Split('³');

                txtInsItem.Text = "";
                foreach (string word in words)
                {
                    if (txtInsItem.Text == "")
                        txtInsItem.Text = word;
                }

                Habil_TipoDAL RnSituacao = new Habil_TipoDAL();
                ddlTipoInscricao.DataSource = RnSituacao.TipoPessoa();
                ddlTipoInscricao.DataTextField = "DescricaoTipo";
                ddlTipoInscricao.DataValueField = "CodigoTipo";
                ddlTipoInscricao.DataBind();

                CarregaDropDownList();

                DBTabelaDAL RnTab = new DBTabelaDAL();
                txtdtabertura.Text = RnTab.ObterDataHoraServidor().ToString("dd/MM/yyyy HH:mm");
                if (txtInsItem.Text != "Novo")
                {
                    btnExcInscricao.Visible = true;
                    foreach (Pessoa_Inscricao  p in listCadPessoaInscricao )
                    {
                        if (txtInsItem.Text == p._CodigoItem.ToString())
                        {
                            txtCNPJCPF.Text = p._NumeroInscricao.ToString();
                            ddlTipoInscricao.SelectedValue = p._TipoInscricao.ToString();
                            txtdtabertura.Text = String.Format("{0:dd/MM/yyyy}", p._DataDeAbertura);
                            
                            txtdtencerramento.Text = String.Format("{0:dd/MM/yyyy}", p._DataDeEncerramento);
                            txtIERG.Text = p._NumeroIERG.ToString();
                            txtOBSInscr.Text = p._OBS;
                            txtIM.Text = p._NumeroIM;
                            ddlPaises.SelectedValue = p.CodigoPais.ToString();

                        }
                    }
                }

                Session["CadPessoa_Inscricao"] = null;
                txtCNPJCPF.Focus();
            }
            if (txtInsItem.Text == "")
                btnCanInscricao_Click(sender, e);
        }
        protected void txtCNPJCPF_TextChanged(object sender, EventArgs e)
        {
            if (ddlPaises.SelectedValue == "1058")
            {


                String strValor = txtCNPJCPF.Text;

                strValor = strValor.Replace(".", "");
                strValor = strValor.Replace("/", "");
                strValor = strValor.Replace("-", "");

                if (strValor.Length == 14)
                {
                    txtCNPJCPF.Text = strValor.Substring(0, 2) + "." + strValor.Substring(2, 3) + "." + strValor.Substring(5, 3) + "/" + strValor.Substring(8, 4) + "-" + strValor.Substring(12, 2);
                    ddlTipoInscricao.SelectedValue = "3";
                }
                else if (strValor.Length == 11)
                {
                    txtCNPJCPF.Text = strValor.Substring(0, 3) + "." + strValor.Substring(3, 3) + "." + strValor.Substring(6, 3) + "-" + strValor.Substring(9, 2);
                    ddlTipoInscricao.SelectedValue = "4";
                }
                else
                    txtCNPJCPF.Text = "";
            }
        }
        protected void btnSlvInscricao_Click(object sender, EventArgs e)
        {
            int intInsItem = 0;

            if (!ValidaTela())
                return;

            if (txtInsItem.Text != "Novo")
                intInsItem = Convert.ToInt32(txtInsItem.Text);
            else
            {
                if (listCadPessoaInscricao.Count != 0)
                    intInsItem = Convert.ToInt32(listCadPessoaInscricao.Max(x => x._CodigoItem).ToString());

                intInsItem = intInsItem + 1;
            }

            String strValor = txtCNPJCPF.Text;

            strValor = strValor.Replace(".", "");
            strValor = strValor.Replace("/", "");
            strValor = strValor.Replace("-", "");

            DateTime? dataabertura;
            dataabertura = Convert.ToDateTime(txtdtabertura.Text);

            DateTime? dataencerra = null;
            if (txtdtencerramento.Text.Trim() != "")
                dataencerra = Convert.ToDateTime(txtdtencerramento.Text);
            
            Pessoa_Inscricao x1 = new Pessoa_Inscricao(intInsItem, Convert.ToInt32(ddlTipoInscricao.Text), strValor, txtIERG.Text, txtIM.Text, dataabertura, dataencerra, txtOBSInscr.Text, Convert.ToInt32(ddlPaises.SelectedValue));

            if (intInsItem != 0)
                listCadPessoaInscricao.RemoveAll(x => x._CodigoItem == intInsItem);

            listCadPessoaInscricao.Add(x1);

            Session["InscricaoPessoa"] = listCadPessoaInscricao;
            Session["CARREGOUCNPJ"] = null;
            btnCanInscricao_Click(sender, e);

        }
        protected void btnCanInscricao_Click(object sender, EventArgs e)
        {
            if(txtCodPessoa.Text == "0")
                Session["TabFocada"] = "profile";
            if (Request.QueryString["Cad"] != null)
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx?cad=" + Request.QueryString["Cad"]);
            else
                Response.Redirect("~/Pages/Pessoas/CadPessoa.aspx");
        }
        protected void btnExcInscricao_Click(object sender, EventArgs e)
        {
            int intInsItem = 0;


            if (txtInsItem.Text != "Novo")
            {
                intInsItem = Convert.ToInt32(txtInsItem.Text);
                if (!ValidaExclusao())
                {
                    ShowMessage("Inscrição não pode ser Excluída. Existe(m) Contato(s) ou Endereço(s) vinculados ao mesmo.", MessageType.Info);
                    return;
                }

                intInsItem = Convert.ToInt32(txtInsItem.Text);

                if (intInsItem != 0)
                    listCadPessoaInscricao.RemoveAll(x => x._CodigoItem == intInsItem);

                Session["InscricaoPessoa"] = listCadPessoaInscricao;

                btnCanInscricao_Click(sender, e);
            }
        }
        protected void ddlTipoInscricao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaises.SelectedValue == "1058")
            {
                txtCNPJCPF.Text = "";
                txtCNPJCPF.Focus();
            }
        }
    }
}