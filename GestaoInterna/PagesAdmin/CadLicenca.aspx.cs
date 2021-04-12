using System;
using System.Web.UI;
using DAL.Model;
using DAL.Persistence;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace GestaoInterna.PagesAdmin
{
    public partial class CadLicenca : System.Web.UI.Page
    {
        clsValidacao v = new clsValidacao();
        String strMensagemR = "";
        bool blnCampoValido = false;
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

                if (Session["ZoomLicenca"] != null)
                {

                    string s = Session["ZoomLicenca"].ToString();
                    Session["ZoomLicenca"] = null;

                    string[] words = s.Split('³');
                    if (s != "³")
                    {
                        foreach (string word in words)
                            if (txtCodLicenca.Text == "")
                            {
                                txtCodLicenca.Text = word;
                                txtCodLicenca.Enabled = false;

                                LicencaDAL r = new LicencaDAL();
                                Licenca p = new Licenca();

                                p = r.PesquisarLicenca(Convert.ToInt64(txtCodLicenca.Text));

                                txtCodCliente.Text = p.CodigoDoCliente.ToString();
                                txtNomeCliente.Text = p.NomeDoCliente;
                                txtServidor.Text = p.ServidorDoCliente;
                                txtBanco.Text = p.BancoDoCliente;
                                txtUsuario.Text = p.UsuarioBancoDoCliente;
                                txtSenha.Text = p.SenhaBancoDoCliente;
                                txtAtuaBanco.Text = p.CodigoDaAtualizacaoBanco.ToString();
                                pnlLicenca.Visible = true;

                            }
                        LicencaDAL s1 = new LicencaDAL();
                        grdConsulta.DataSource = s1.PesquisarItemLicenca(Convert.ToInt64(txtCodLicenca.Text));
                        grdConsulta.DataBind();

                        return;
                        }
                    }
                else
                {
                    pnlLicenca.Visible = false;
                    txtCodLicenca.Text = "Novo";
                    txtCodCliente.Focus(); 
                }

            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PagesAdmin/ConLicenca.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaCampos() == false)
                return;


            LicencaDAL d = new LicencaDAL();
            Licenca p = new Licenca();

            p.CodigoDoCliente = Convert.ToInt64( txtCodCliente.Text);
            p.NomeDoCliente = txtNomeCliente.Text.ToUpper();
            p.ServidorDoCliente = txtServidor.Text.ToUpper();
            p.BancoDoCliente  = txtBanco.Text.ToUpper();
            p.UsuarioBancoDoCliente = txtUsuario.Text;
            p.SenhaBancoDoCliente = txtSenha.Text;


            if (txtCodLicenca.Text == "Novo")
            {
                p.CodigoDaAtualizacaoBanco = 0;
                d.Inserir(p);
                txtCodLicenca.Text = p.CodigoDaLicenca.ToString();
                ShowMessage("Cliente Incluído com Sucesso!!!",MessageType.Info);
                pnlLicenca.Visible = true;
            }
            else
            {
                if (txtAtuaBanco.Text == "")
                    txtAtuaBanco.Text = "0";

                p.CodigoDaAtualizacaoBanco = Convert.ToInt64(txtAtuaBanco.Text);
                p.CodigoDaLicenca= Convert.ToInt64(txtCodLicenca.Text);
                d.Atualizar(p);
                ShowMessage("Cliente Alterado com Sucesso!!!", MessageType.Info);
            }

        }
        protected bool ValidaCampos()
        {
            Boolean blnCampoValido = false;

            v.CampoValido("Código do Cliente", txtCodCliente.Text, true, true, true, false, "", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtNomeCliente.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtCodCliente.Focus();

                }

                return false;
            }

            v.CampoValido("Nome do Cliente", txtNomeCliente.Text, true, false, false, true,"", ref blnCampoValido, ref strMensagemR);

            if (!blnCampoValido)
            {
                txtNomeCliente.Text = "";
                if (strMensagemR != "")
                {
                    ShowMessage(strMensagemR, MessageType.Info);
                    txtNomeCliente.Focus();

                }

                return false;
            }

            return true;
        }
        protected void btnNovaChave_Click(object sender, EventArgs e)
        {
            pnlMexerChave.Visible = true;
            pnlGrid.Visible = false;

            ModuloSistemaDAL s1 = new ModuloSistemaDAL();
            grdPermissao.DataSource = s1.ListarModulosSistema("","","","");
            grdPermissao.DataBind();
            txtChave.Text = "";
            txtToDoDate.Text = "";
            txtToDoDate.Enabled = true;
            txtChave.Enabled = true;
            grdPermissao.Enabled = true;
            btnGravaItem.Text = "Gravar";
            txtToDoDate.Focus(); 
        }
        protected void grdConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlMexerChave.Visible = true;
            pnlGrid.Visible = false;

            Session["ZoomItemLicenca"] = HttpUtility.HtmlDecode(grdConsulta.SelectedRow.Cells[0].Text);
            txtToDoDate.Text = HttpUtility.HtmlDecode(grdConsulta.SelectedRow.Cells[1].Text);
            txtChave.Text= HttpUtility.HtmlDecode(grdConsulta.SelectedRow.Cells[3].Text);
            
            LicencaDAL s1 = new LicencaDAL();
            grdPermissao.DataSource = s1.ListarModulosSistemaPelaLicenca(Convert.ToInt64(HttpUtility.HtmlDecode(grdConsulta.SelectedRow.Cells[0].Text)));
            grdPermissao.DataBind();
            txtToDoDate.Enabled = false;
            txtChave.Enabled = false;
            grdPermissao.Enabled = false;
            btnGravaItem.Text = "Gerar Chave";
        }
        protected void btnVoltarItem_Click(object sender, EventArgs e)
        {
            pnlMexerChave.Visible = false;
            pnlGrid.Visible = true;

            LicencaDAL s1 = new LicencaDAL();
            grdConsulta.DataSource = s1.PesquisarItemLicenca(Convert.ToInt64(txtCodLicenca.Text));
            grdConsulta.DataBind();

        }
        protected void btnGravaItem_Click(object sender, EventArgs e)
        {
            if (btnGravaItem.Text.Equals("Gerar Chave"))
            {
                LicencaDAL d = new LicencaDAL();
                Licenca p = new Licenca();
                ItemDaLicenca z = new ItemDaLicenca();
                MenuSistemaDAL MSDAL = new MenuSistemaDAL();
                DBTabelaDAL RnTab = new DBTabelaDAL();
                List<DBTabela> lstEstrutura = new List<DBTabela>();

                string appPath = ConfigurationManager.AppSettings["CaminhoDaLicenca"].ToString() ;

                string strAjuda = "";
                appPath = appPath + "Licenca";

                if  (!Directory.Exists(appPath))
                {
                    //Criamos um com o nome folder
                    Directory.CreateDirectory(appPath);
                }

                string nomeArquivo = @"\GerVer" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".SQL";

                // Cria um novo arquivo e devolve um StreamWriter para ele

                StreamWriter writer = new StreamWriter(appPath + @"\" + nomeArquivo);

                // Agora é só sair escrevendo

                writer.WriteLine("TRUNCATE TABLE LICENCA_DE_USO ");
                writer.WriteLine("GO ");
                writer.WriteLine("TRUNCATE TABLE LICENCA_DE_USO ");
                writer.WriteLine("GO ");
                writer.WriteLine("TRUNCATE TABLE LICENCA_DE_USO_ITEM ");
                writer.WriteLine("GO ");
                writer.WriteLine("TRUNCATE TABLE LICENCA_DE_USO_POR_MODULO_DO_SISTEMA ");
                writer.WriteLine("GO ");
                writer.WriteLine("TRUNCATE TABLE MENU_DO_SISTEMA ");
                writer.WriteLine("GO ");
                writer.WriteLine("TRUNCATE TABLE MODULO_DO_SISTEMA ");
                writer.WriteLine("GO ");

                p = d.PesquisarLicenca(Convert.ToInt64(txtCodLicenca.Text));
                
                string strSQL = "INSERT INTO LICENCA_DE_USO (CD_LICENCA, CD_CLIENTE, NM_CLIENTE, NR_USUARIOS, TX_SERVIDOR, TX_BANCO, TX_USUARIO, TX_SENHA, CD_STR_DATABASE) ";
                strSQL = strSQL + "VALUES(" + p.CodigoDaLicenca.ToString() +", " + p.CodigoDoCliente.ToString() + ", " + "'" + p.NomeDoCliente + "', " + "0, '" + p.ServidorDoCliente + "', '" + p.BancoDoCliente + "', '" + p.UsuarioBancoDoCliente+ "', '" + p.SenhaBancoDoCliente+ "', " + p.CodigoDaAtualizacaoBanco.ToString() + ")";
                writer.WriteLine(strSQL);
                writer.WriteLine("GO ");

                z = d.PesquisarUltimoItemLicenca(Convert.ToInt64(txtCodLicenca.Text));

                strSQL = "INSERT INTO LICENCA_DE_USO_ITEM (CD_ITEM_LICENCA_DE_USO, CD_LICENCA, DT_VALIDADE, CH_AUTENTICACAO, TX_GUID) ";
                strSQL = strSQL + "VALUES ("+ z.CodigoDoItem.ToString() +", "+ z.CodigoDaLicenca.ToString() + ", '"+ z.DataDeValidade.ToString("yyyy-MM-dd") +"', '"+ z.ChaveDeAutenticacao +"', '"+ z.Guid  +"')";
                writer.WriteLine(strSQL);
                writer.WriteLine("GO ");


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                List<ModuloSistema>lista = new List<ModuloSistema>();
                lista = d.ListarModulosSistemaPelaLicenca(Convert.ToInt64(Session["ZoomItemLicenca"]));

                foreach (ModuloSistema pepe in lista)
                {
                    if (pepe.Liberar)
                    {


                        strSQL = "INSERT INTO LICENCA_DE_USO_POR_MODULO_DO_SISTEMA (CD_MODULO_LICENCA, CD_LICENCA, CD_MODULO_DO_SISTEMA) ";
                        strSQL = strSQL + "VALUES  (" + z.CodigoDoItem.ToString() + ", " + z.CodigoDoItem.ToString() + ", "+ pepe.CodigoModulo.ToString() +")";
                        writer.WriteLine(strSQL);
                        writer.WriteLine("GO ");

                        strSQL = "INSERT INTO MODULO_DO_SISTEMA (CD_MODULO_SISTEMA, DS_MODULO_SISTEMA) ";
                        strSQL = strSQL + "VALUES (" + pepe.CodigoModulo.ToString() + ", '" + pepe.DescricaoModulo + "')";
                        writer.WriteLine(strSQL);
                        writer.WriteLine("GO ");

                        //Menu do Sistema

                        List<MenuSistema> listaM = new List<MenuSistema>();
                        listaM = MSDAL.ListarMenuSistema("CD_MODULO_SISTEMA", "INT", pepe.CodigoModulo.ToString(), "CD_MENU_SISTEMA");

                        foreach (MenuSistema meni in listaM)
                        {
                            strAjuda = meni.TextoAjuda.Replace("\r\n", " - ");
                            strSQL = " insert into [MENU_DO_SISTEMA] (CD_MENU_SISTEMA, CD_MODULO_SISTEMA, NM_MENU_SISTEMA, DS_MENU_SISTEMA, CD_ORDEM, CD_PAI_MENU_SISTEMA, URL, TP_FORMULARIO, TX_AJUDA, TX_CAMINHO_IMG ) ";
                            strSQL = strSQL + " Values(" + meni.CodigoMenu.ToString() + ", " + meni.CodigoModulo.ToString() + ", '" + meni.NomeMenu + "', '" + meni.DescricaoMenu + "', " + meni.CodigoOrdem.ToString() + ", " + meni.CodigoPaiMenu.ToString() + ", '" + meni.UrlPrograma + "', '" + meni.TipoFormulario  + "', '"+ strAjuda +"','"+meni.UrlIcone+"')";
                            writer.WriteLine(strSQL);
                            writer.WriteLine("GO ");

                        }

                    }
                }

                Habil_TipoDAL RnHabilTipoDal = new Habil_TipoDAL();
                List<Habil_Tipo> listaT = new List<Habil_Tipo>();
                listaT = RnHabilTipoDal.ListarHabilTipos( "", "", "", "CD_TIPO");


                foreach (Habil_Tipo situacao in listaT)
                {

                    strSQL = "if NOT exists(select 1 from habil_tipo where cd_tipo = " + situacao.CodigoTipo.ToString() +") ";
                    writer.WriteLine(strSQL);

                    strSQL = "  insert into habil_tipo values("+ situacao.CodigoTipo.ToString() +", '"+ situacao.DescricaoTipo +"', " + situacao.TipoTipo + ", '"+ situacao.Observacao +"', " + situacao.CodigoReferencia + " )";
                    writer.WriteLine(strSQL);

                    strSQL = "else";
                    writer.WriteLine(strSQL);

                    strSQL = " update habil_tipo Set DS_TIPO = '" + situacao.DescricaoTipo + "', TP_TIPO = " + situacao.TipoTipo + ", TX_OBS = '" + situacao.Observacao + "', CD_REFERENCIA = " + situacao.CodigoReferencia + "   where cd_tipo = " + situacao.CodigoTipo.ToString();
                    writer.WriteLine(strSQL);
                    writer.WriteLine("GO ");
                    writer.WriteLine(" ");

                }

                DataTable dt;

                dt = RnTab.ListarPesquisaFiltro("", "");

                foreach (DataRow item in dt.Rows)
                {
                    strSQL = "if exists(select 1 from habil_pesquisa_filtro where cd_pesquisa_filtro = " + item["CD_PESQUISA_FILTRO"].ToString() + ") ";
                    writer.WriteLine(strSQL);
                    strSQL = "  Delete from habil_pesquisa_filtro where cd_pesquisa_filtro = " + item["CD_PESQUISA_FILTRO"].ToString() ;
                    writer.WriteLine(strSQL);
                    writer.WriteLine("GO ");
                    writer.WriteLine("SET IDENTITY_INSERT[dbo].[HABIL_PESQUISA_FILTRO] ON");
                    strSQL = " insert into habil_pesquisa_filtro(CD_PESQUISA_FILTRO, DS_TABELA, DS_CAMPO, ORDEM, VW_DS_ALIAS, REG_UNICO, POP_TABELA) values(" + item["CD_PESQUISA_FILTRO"].ToString() + ", '"+ item["DS_TABELA"].ToString() + "', '"+ item["DS_CAMPO"].ToString() + "', "+ item["ORDEM"].ToString() + ", '"+ item["VW_DS_ALIAS"].ToString() + "', "+ item["REG_UNICO"].ToString() + ", '"+ item["POP_TABELA"].ToString() + "') ";
                    writer.WriteLine(strSQL);
                    writer.WriteLine("SET IDENTITY_INSERT[dbo].[HABIL_PESQUISA_FILTRO] OFF");
                    writer.WriteLine("GO ");

                }

                // Não esqueça de fechar o arquivo ao terminar

                writer.Close();

                ShowMessage("Arquivo Gerado com Sucesso!!!", MessageType.Info);

                return;
            }
            else
            {
                v.CampoValido("Data de Validade", txtToDoDate.Text, true, false, false, false, "SMALLDATETIME", ref blnCampoValido, ref strMensagemR);
                if (!blnCampoValido)
                {
                    if (strMensagemR != "")
                    {
                        ShowMessage(strMensagemR, MessageType.Info);
                        txtToDoDate.Focus();
                        return;
                    }
                }
                else
                {
                    if (Convert.ToDateTime(txtToDoDate.Text) <= DateTime.Now)
                    {
                        ShowMessage("Data de Validade deve ser superior a Data de Hoje.", MessageType.Info);
                        txtToDoDate.Focus();
                        return;
                    }

                }

            }

            /*Verificação dos Módulos Marcados*/
            bool blnMarcado = false;
            string strMarcados = "";
            foreach (GridViewRow row in grdPermissao.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLiberar");
                CheckBox lbl = (CheckBox)row.FindControl("chkLiberar");
                if (chk.Checked.Equals(true))
                {
                    blnMarcado = chk.Checked;

                    if (strMarcados.Equals(""))
                        strMarcados = row.Cells[1].Text;
                    else
                        strMarcados = strMarcados + ", " + row.Cells[1].Text;

                }
            }

            if (!blnMarcado)
            {
                ShowMessage("Nenhum Módulo do Sistema foi Selecionado. Verifique!!!", MessageType.Info);
                return;
            }

            //Gravação dos Dados
            LicencaDAL s2 = new LicencaDAL();
            s2.SalvarLicenca(Convert.ToInt64(txtCodLicenca.Text), Convert.ToDateTime(txtToDoDate.Text), strMarcados);

            pnlMexerChave.Visible = false;
            pnlGrid.Visible = true;

            grdConsulta.DataSource = s2.PesquisarItemLicenca(Convert.ToInt64(txtCodLicenca.Text));
            grdConsulta.DataBind();

        }

        protected void grdConsulta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)

            {

                //e.Row.Cells[3].Text = FormatarTexto(e.Row.Cells[3].Text, 70);
                e.Row.Cells[2].Text = FormatarTexto(e.Row.Cells[2].Text, 7);
                e.Row.Cells[3].Text = FormatarTexto(e.Row.Cells[3].Text, 7);


            }
        }
        public string FormatarTexto(string texto, int tamanho)

        {

            string str_retorno = string.Empty;

            if (texto.Length > tamanho)

            {

                str_retorno = texto.Substring(0, tamanho - 3) + "...";

                return str_retorno;

            }

            else

            {

                return texto;

            }

        }
    }

}
