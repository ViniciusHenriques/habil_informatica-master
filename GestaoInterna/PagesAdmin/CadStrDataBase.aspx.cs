using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.UI;
using DAL.Model;
using DAL.Persistence;

namespace GestaoInterna.PagesAdmin
{
    public partial class CadStrDataBase : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        public enum MessageType { Success, Error, Info, Warning };
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UsuSis"] == null) || (Session["CodUsuario"] == null))
            {
                Response.Redirect("~/Default.aspx");
            }


            if (Session["MensagemTela"] != null)
            {
                ShowMessage(Session["MensagemTela"].ToString(), MessageType.Info);
                Session["MensagemTela"] = "";
            }

            if (Session["Pagina"].ToString() != Request.CurrentExecutionFilePath)
            {
                Session["Pagina"] = Request.CurrentExecutionFilePath;
                LicencaDAL d = new LicencaDAL();
                DdlCliente.DataSource = d.ListarLicencas("", "", "", "");
                DdlCliente.DataTextField = "NomeDoCliente";
                DdlCliente.DataValueField = "CodigoDoCliente";
                DdlCliente.DataBind();
                DdlCliente.Items.Insert(0, "..... SELECIONE UM CLIENTE .....");
            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;

            DBTabela Tab = new DBTabela();
            DBTabelaDAL DalTab = new DBTabelaDAL();


            Tab = new DBTabela(); 

            Tab.BancoEstrutura = txtConteudo.Text;
            Tab.NomeComum = txtDescricao.Text;

            DalTab.GravarEstruturasBanco(Tab);

            ShowMessage("Gravação do Script com Sucesso!!!", MessageType.Success);
            

        }
        protected bool ValidaCampos()
        {
            if (txtDescricao.Text == "" )
            {
               ShowMessage("Descrição deve ser Informada.", MessageType.Info);
               txtDescricao.Focus();
                return false;

            }

            if (txtConteudo.Text == "")
            {
                ShowMessage("Conteúdo deve ser Informado.", MessageType.Info);
                txtConteudo.Focus();
                return false;

            }

            return true;
        }
        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (DdlCliente.Text == "..... SELECIONE UM CLIENTE .....")
            {
                ShowMessage("Cliente deve ser Informado.", MessageType.Info);
                return;
            }

            if (DdlCliente.Text == "..... SELECIONE UM SCRIPT .....")
            {
                ShowMessage("Seleção de Script deve ser Informado.", MessageType.Info);
                return;
            }

            LicencaDAL d = new LicencaDAL();
            DBTabelaDAL RnTab = new DBTabelaDAL();
            List<DBTabela> lstEstrutura = new List<DBTabela>();


            string appPath = ConfigurationManager.AppSettings["CaminhoDaEstrutura"].ToString();
            appPath = appPath + "Struct_DataBase";

            if (!Directory.Exists(appPath))
            {
                //Criamos um com o nome folder
                Directory.CreateDirectory(appPath);
            }

            string nomeArquivo = @"\Banco" + DdlCliente.SelectedItem.Text.Replace(" ", "_").Replace("/","") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".SQL";

            // Cria um novo arquivo e devolve um StreamWriter para ele

            StreamWriter writer = new StreamWriter(appPath + @"\" + nomeArquivo);

            // Agora é só sair escrevendo

            Licenca l = new Licenca();
            l = d.PesquisarLicenca(Convert.ToInt64(DdlCliente.SelectedValue));

            lstEstrutura = RnTab.ListarEstruturasBanco(l.CodigoDaAtualizacaoBanco, Convert.ToInt64(DdlAtuBanco.SelectedValue));
            foreach (DBTabela lstEst in lstEstrutura)
            {
                writer.WriteLine("/*******************************************************************************************************************************");
                writer.WriteLine(lstEst.NomeComum.ToString());
                writer.WriteLine("********************************************************************************************************************************/");

                writer.WriteLine("print '/*******************************************************************************************************************************'");
                writer.WriteLine("print '" + lstEst.NomeComum.ToString() + "'");
                writer.WriteLine("print '********************************************************************************************************************************/'");

                writer.WriteLine("waitfor delay '00:00:02';");


                writer.WriteLine(lstEst.BancoEstrutura.ToString());

                writer.WriteLine("print '/*******************************************************************************************************************************'");
                writer.WriteLine("print 'FINAL COMANDO - " + lstEst.NomeComum.ToString() + "'");
                writer.WriteLine("print '********************************************************************************************************************************/'");

                writer.WriteLine("GO ");
            }

            writer.WriteLine("print '********************************************************************************************************************************/'");

            writer.WriteLine(" if exists (select top 1 1 From Habil_Update) ");
            writer.WriteLine(" begin ");
            writer.WriteLine("   update Habil_Update Set CD_INDEX_VERSAO = " + Convert.ToInt64(DdlAtuBanco.SelectedValue).ToString() + ", DT_UPDATE = GETDATE()") ;
            writer.WriteLine(" end ");
            writer.WriteLine(" else begin ");
            writer.WriteLine("   Insert into Habil_Update (CD_INDEX_VERSAO ) values ( " + Convert.ToInt64(DdlAtuBanco.SelectedValue).ToString() + ")");
            writer.WriteLine(" end ");
            writer.WriteLine(" GO ");
            writer.WriteLine("print '********************************************************************************************************************************/'");


            writer.Close();

            d.AtualizarLicencaBanco(Convert.ToInt64(DdlCliente.SelectedValue), Convert.ToInt64(DdlAtuBanco.SelectedValue));


            ShowMessage("Script Banco do Cliente gerado com Sucesso.", MessageType.Success);


        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = "Inicial";
            Response.Redirect("~/PagesAdmin/WelCome.aspx");
            this.Dispose();
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtConteudo.Text = "";
            txtDescricao.Text = "";
            txtDescricao.Focus();
        }

        protected void DdlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DdlCliente_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void DdlAtuBanco_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if(DdlCliente.Text == "..... SELECIONE UM CLIENTE .....")
            {
                ShowMessage("Cliente deve ser Informado.", MessageType.Info);
                return;
            }
            else
            {
                DBTabelaDAL d = new DBTabelaDAL();
                LicencaDAL ld = new LicencaDAL();
                Licenca l = new Licenca();

                l = ld.PesquisarLicenca(Convert.ToInt64(DdlCliente.SelectedValue));


                DdlAtuBanco.DataSource = d.ListarEstruturasBanco(l.CodigoDaAtualizacaoBanco,0);
                DdlAtuBanco.DataTextField = "NomeComum";
                DdlAtuBanco.DataValueField = "IDTabela";
                DdlAtuBanco.DataBind();
                DdlAtuBanco.Items.Insert(0, "..... SELECIONE UM SCRIPT .....");

            }

        }
    }

}
