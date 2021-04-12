using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DAL.Persistence;
using DAL.Model;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using System.Web.Services;
using System.Linq;
using System.Web;
using System.Web.Caching;
namespace HabilInformatica
{
    public partial class MasterPrincipal : System.Web.UI.MasterPage
    {
        public string TamanhoDivCorpo { get; set; } 
        public string TamanhoDivMenu { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    if (Session["CodModulo"] != null && Session["CodEmpresa"] != null && Session["CodPflUsuario"] != null)
            //    {
            //        string[] strURL = HttpContext.Current.Request.Url.AbsoluteUri.Split('/');
            //        List<Permissao> lista = new List<Permissao>();
            //        PermissaoDAL r1 = new PermissaoDAL();
            //        lista = r1.ListarPerfilUsuario(Convert.ToInt32(Session["CodModulo"].ToString()),
            //                                            Convert.ToInt32(Session["CodPflUsuario"].ToString()),
            //                                            strURL[strURL.Count() - 1]);
            //        lista.ForEach(delegate (Permissao x)
            //        {
            //            if (!x.Liberado)
            //            {
            //                Response.Redirect("~/Pages/Welcome.aspx");
            //            }
            //        });
            //    }
            //    else
            //    {
            //        if (Session["CodUsuario"] != null && !IsPostBack)
            //        {
            //            if (Session["CodUsuario"].ToString() != "-150380")
            //                Response.Redirect("~/Selecao.aspx");
            //        }
            //    }
            //}

            if (Session["CodigoModuloEspecifico"] != null)
                btnVoltar.Visible = false;

            if ((Session["UsuSis"] == null) || (Session["CodModulo"] == null))
            {
                Menu1.Visible = false;
                pnlAuttenticado.Visible = false;
                cttCorpo.Visible = false;
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                Menu1.Dispose();
                Menu1.Visible = true;

                DataTable dt2 = new DataTable();

                if (Session["CodUsuario"] != null)
                {
                    dt2 = this.GetData(Convert.ToInt32(Session["CodModulo"]), 0, Convert.ToInt32(Session["CodPflUsuario"].ToString()));
                    MontaMenu(Convert.ToInt32(Session["CodModulo"]), dt2,  0,  null, Convert.ToInt32(Session["CodPflUsuario"].ToString()));
                }

                if(Session["MenuAberto"] != null)
                    if (Session["MenuAberto"].ToString() == "NAO")
                    {
                        Session["MenuAberto"] = "SIM";
                    }
                                        
                pnlAuttenticado.Visible = true;
                cttCorpo.Visible = true;


                lblUsuario.Text = Session["UsuSis"].ToString();
                lblModulo.Text = "Você está Logado no Módulo: " + Session["DesModulo"].ToString();
                if (Session["NomeEmpresa"] != null)
                    lblEmpresa.Text = "Empresa: " + Session["CodEmpresa"].ToString() + " - " + Session["NomeEmpresa"].ToString();


                string CaminhoFotoUsuario = @"\Scripts\CapturaWebcamASPNET\uploads\" + Session["CodPessoaLogada"] + "-" + Session["UsuSis"] + "-" + 1 +".jpg";
                string CaminhoProjeto = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                if (System.IO.File.Exists(CaminhoProjeto + CaminhoFotoUsuario))
                {
                    imgUsuario.ImageUrl = CaminhoFotoUsuario + "?cache" + DateTime.Now.Ticks.ToString();
                    imgFotoModal.ImageUrl = CaminhoFotoUsuario + "?cache" + DateTime.Now.Ticks.ToString();
                }
            }

            if (pnlPrincipalMasterPageVertical.Visible == true)
                
                if (Cache["MenuMaximizadoUsuario" + Session["CodUsuario"]] == null  ||
                    Convert.ToBoolean(Cache["MenuMaximizadoUsuario" + Session["CodUsuario"]]) == false)
                {
                    btnMinMenu_Click(sender, e);
                    btnMaxMenu.Visible = true;
                    btnMinMenu.Visible = false;
                }
                else
                {
                    btnMaxMenu_Click(sender, e);
                    btnMaxMenu.Visible = false;
                    btnMinMenu.Visible = true;
                }

            if (Session["CodUsuario"].ToString() != "-150380")
                VerificaAutorizacaoAcesso();
        }

        private DataTable GetData(int CodMod, int paiMenu, int CodPfl)
        {
            MenuSistemaDAL m = new MenuSistemaDAL();

            if (Session["CodUsuario"].ToString() == "-150380")
            {
                return m.ObterMenuInicial("CD_PAI_MENU_SISTEMA", "INT", paiMenu.ToString(), "CD_ORDEM");
            }
            else
            {
                return m.ObterMenuSistema(CodMod, "CD_PAI_MENU_SISTEMA", "INT", paiMenu.ToString(), "CD_ORDEM", CodPfl);
            }
        }

        private void MontaMenu(int intCodMod, DataTable dt, int intMenuPai, MenuItem intMenuFilho, int intCodPfl)
        {
            
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
            foreach (DataRow row in dt.Rows)
            {
                MenuItem menuItem = new MenuItem();
                if (row["UrlIcone"].ToString() == "")
                {
                    menuItem = new MenuItem
                    {
                        Value = row["CodigoMenu"].ToString(),
                        Text = " " + row["NomeMenu"].ToString(),
                        NavigateUrl = row["UrlPrograma"].ToString(),
                        Selected = row["UrlPrograma"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
                    };
                }
                else
                {
                    menuItem = new MenuItem
                    {
                        Value = row["CodigoMenu"].ToString(),
                        Text = " " + row["NomeMenu"].ToString(),
                        NavigateUrl = row["UrlPrograma"].ToString(),
                        Selected = row["UrlPrograma"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase),
                        ImageUrl = row["UrlIcone"].ToString() + "?cache" + DateTime.Now.Ticks.ToString()
                    };
                }
                menuItem.ToolTip = menuItem.Text;
                if (intMenuPai == 0)
                {
                    List<ParSistema> ListPar = new List<ParSistema>();
                    ParSistemaDAL ParDAL = new ParSistemaDAL();

                    if (Session["CodEmpresa"] == null)
                        Session["CodEmpresa"] = -1;

                    ListPar = ParDAL.ListarParSistemas("CD_EMPRESA", "INT", Session["CodEmpresa"].ToString(), "");
                    
                    if(ListPar.Count > 0)
                        if (ListPar[0].TipoMenu == 1)
                        {
                            pnlPrincipalMasterPage.Visible = true;
                            pnlPrincipalMasterPageVertical.Visible = false;
                            TamanhoDivCorpo = "col-md-12";
                            Menu1.Items.Add(menuItem);
                        }
                        else
                        {
                            pnlPrincipalMasterPageVertical.Visible = true;
                            pnlPrincipalMasterPage.Visible = false;
                            TamanhoDivCorpo = "col-md-10";
                            TamanhoDivMenu = "col-md-2";
                            Menu2.Items.Add(menuItem);
                        }
                    else
                    {
                        pnlPrincipalMasterPage.Visible = true;
                        pnlPrincipalMasterPageVertical.Visible = false;
                        TamanhoDivCorpo = "col-md-12";
                        Menu1.Items.Add(menuItem);
                    }
                    
                    DataTable dtChild = this.GetData(intCodMod,  int.Parse(menuItem.Value), intCodPfl);
                    MontaMenu(intCodMod, dtChild, int.Parse(menuItem.Value), menuItem, intCodPfl);
                }
                else
                {                   
                    intMenuFilho.ChildItems.Add(menuItem);
                    DataTable dtChild2 = this.GetData(intCodMod, int.Parse(menuItem.Value), intCodPfl);
                    MontaMenu(intCodMod, dtChild2, int.Parse(menuItem.Value), menuItem, intCodPfl);                    
                }
                if ((menuItem.NavigateUrl == "~/Pages/Welcome.aspx" || menuItem.NavigateUrl == "~/") && menuItem.Text != " " && menuItem.Text != "  ")
                {
                    menuItem.Selected = false;
                    menuItem.Selectable = false;
                    menuItem.Text += " <span class='caret'/></span>";
                }
                else
                {
                    menuItem.Selected= true;
                    menuItem.Selectable = true;
                }              
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            Session["Pagina"] = "/Pages/Welcome.aspx";

        }
        [WebMethod]
        public void btnOK_Click(object sender, EventArgs e)
        {

            if (Session["CD_DOC_INTEGRA_VISUALIZADO"] != null)
            {
                string teste = Session["CD_DOC_INTEGRA_VISUALIZADO"].ToString();
                IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();
                IntegraDocumentoEletronico integra = new IntegraDocumentoEletronico();
                integra = integraDAL.PesquisarIntegracaoDocEletronicoPorCodigo(Convert.ToDecimal(Session["CD_DOC_INTEGRA_VISUALIZADO"]), 0);
                integra.RegistroMensagem = 6;

                integraDAL.AtualizarIntegraDocEletronico(integra);
                Session["CD_DOC_INTEGRA_VISUALIZADO"] = null;
            }
        }

        protected void btnVerMais_Click(object sender, EventArgs e)
        {
            btnOK_Click(sender, e);
            Session["TabFocadaConNFSe"] = "consulta2";
            Response.Redirect("~/pages/Servicos/ConNotaFiscalServico.aspx");
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            List<string> SessionsParaSerAnulada = new List<string>();
            foreach (var i in Session.Keys)
            {
                if (i.ToString() != "CodUsuario" && i.ToString() != "CodPessoaLogada" && i.ToString() != "CodEmpresa"
                    && i.ToString() != "DataHoraVerificaPendencias" && i.ToString() != "Estacao_Logada" && i.ToString() != "IP_Logado"
                    && i.ToString() != "CodEmpresa" && i.ToString() != "QtasEmpresas" && i.ToString() != "NomeEmpresa" && i.ToString() != "UsuSis" && i.ToString() != "CodPflUsuario"
                    && i.ToString() != "CodModulo" && i.ToString() != "DesModulo" && i.ToString() != "sid" && i.ToString() != "PwdSis" && i.ToString() != "ZoomModSistema" 
                    && i.ToString() != "ZoomPflUsuario" && i.ToString() != "Operacao" && i.ToString() != "Pagina" && i.ToString() != "MenuAberto")
                {
                    SessionsParaSerAnulada.Add(i.ToString());
                }
            }
            for (int i = 0; i < SessionsParaSerAnulada.Count(); i++)
            {
                Session[SessionsParaSerAnulada[i]] = null;
            }
            Response.Redirect("~/Selecao.aspx");
        }

        protected void VerificaAutorizacaoAcesso()
        {
            DBTabelaDAL RnTab = new DBTabelaDAL();
            DateTime DataHoraAtual = RnTab.ObterDataHoraServidor();
            DateTime UltimaVerificacao = Convert.ToDateTime(Session["DataHoraVerificaPendencias"]).AddMinutes(1);

            //Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar em breve !!!', 'Atenção');", true);
            
            if (UltimaVerificacao <= DataHoraAtual)
            {
                bool Autorizado = false;
                PerfilUsuario Perfil = new PerfilUsuario();
                PerfilUsuarioDAL PerfilDAL = new PerfilUsuarioDAL();
                Perfil = PerfilDAL.PesquisarPerfilUsuario(Convert.ToInt64(Session["CodPflUsuario"]));

                DateTime EncerramentoSessao = UltimaVerificacao.AddMinutes(10);

                if (Perfil != null)
                {
                    List<String> listaDiaSemanaAutorizado = new List<String>();
                    if (Perfil.Domingo)
                        listaDiaSemanaAutorizado.Add("Sunday");
                    if (Perfil.Segunda)
                        listaDiaSemanaAutorizado.Add("Monday");
                    if (Perfil.Terca)
                        listaDiaSemanaAutorizado.Add("Tuesday");
                    if (Perfil.Quarta)
                        listaDiaSemanaAutorizado.Add("Wednesday");
                    if (Perfil.Quinta)
                        listaDiaSemanaAutorizado.Add("Thursday");
                    if (Perfil.Sexta)
                        listaDiaSemanaAutorizado.Add("Friday");
                    if (Perfil.Sabado)
                        listaDiaSemanaAutorizado.Add("Saturday");

                    foreach (string DiaSemanaAutorizado in listaDiaSemanaAutorizado)
                    {
                        if (DataHoraAtual.DayOfWeek.ToString() == DiaSemanaAutorizado)
                        {
                            if (Perfil.HoraInicial < Perfil.HoraFinal)
                            {
                                if (DataHoraAtual.TimeOfDay >= Perfil.HoraInicial.TimeOfDay && DataHoraAtual.TimeOfDay < Perfil.HoraFinal.TimeOfDay)
                                {
                                    Autorizado = true;
                                    if (EncerramentoSessao >= Perfil.HoraFinal)
                                    {
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.HoraFinal.ToString("HH:mm") + " !!!', 'Atenção');", true);
                                    }
                                }
                            }
                            else if (Perfil.HoraInicial > Perfil.HoraFinal)
                            {
                                DateTime DataFinalMaisUm = Perfil.HoraFinal.AddDays(1);
                                DateTime DataHoraAtualMaisUm = DataHoraAtual;
                                if (DataHoraAtual <= Perfil.HoraInicial)
                                    DataHoraAtualMaisUm = DataHoraAtual.AddDays(1);
                                if (DataHoraAtualMaisUm >= Perfil.HoraInicial && DataHoraAtualMaisUm <= DataFinalMaisUm)
                                {
                                    Autorizado = true;
                                    if (EncerramentoSessao >= Perfil.HoraFinal)
                                    {
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.HoraFinal.ToShortTimeString() + "  !!!', 'Atenção');", true);
                                    }
                                }
                            }
                            if (Autorizado)
                            {
                                if (Perfil.IntervaloInicio1.HasValue)
                                {
                                    if (Perfil.IntervaloInicio1 < Perfil.IntervaloFim1)
                                    {
                                        if (DataHoraAtual >= Perfil.IntervaloInicio1 && DataHoraAtual < Perfil.IntervaloFim1)
                                        {
                                            Autorizado = false;
                                        }
                                        else
                                        {
                                            if (EncerramentoSessao >= Perfil.IntervaloInicio1 && EncerramentoSessao <= Perfil.IntervaloFim1)
                                            {
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.IntervaloInicio1.Value.ToShortTimeString() + " e retornar as " + Perfil.IntervaloFim1.Value.ToShortTimeString() + " !!!', 'Atenção');", true);
                                            }
                                        }
                                    }
                                    else if (Perfil.IntervaloInicio1 > Perfil.IntervaloFim1)
                                    {
                                        DateTime? IntevaloFimMaisUm = Perfil.IntervaloFim1;
                                        DateTime DataHoraAtualMaisUm = DataHoraAtual;
                                        if (DataHoraAtual <= Perfil.IntervaloFim1)
                                            DataHoraAtualMaisUm = DataHoraAtual.AddDays(1);
                                        if (DataHoraAtualMaisUm >= Perfil.IntervaloInicio1 && DataHoraAtualMaisUm <= IntevaloFimMaisUm)
                                        {
                                            Autorizado = false;
                                        }
                                        else
                                        {
                                            if (EncerramentoSessao >= Perfil.IntervaloInicio1 && EncerramentoSessao <= Perfil.IntervaloFim1)
                                            {
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.IntervaloInicio1.Value.ToShortTimeString() + " e retornar as " + Perfil.IntervaloFim1.Value.ToShortTimeString() + " !!!', 'Atenção');", true);
                                            }
                                        }
                                    }
                                }
                                if (Perfil.IntervaloInicio2.HasValue)
                                {
                                    if (Perfil.IntervaloInicio2 < Perfil.IntervaloFim2)
                                    {
                                        if (DataHoraAtual >= Perfil.IntervaloInicio2 && DataHoraAtual < Perfil.IntervaloFim2)
                                        {
                                            Autorizado = false;
                                        }
                                        else
                                        {
                                            if (EncerramentoSessao >= Perfil.IntervaloInicio2 && EncerramentoSessao <= Perfil.IntervaloFim2)
                                            {
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.IntervaloInicio2.Value.ToShortTimeString() + " e retornar as " + Perfil.IntervaloFim2.Value.ToShortTimeString() + " !!!', 'Atenção');", true);
                                            }
                                        }
                                    }
                                    else if (Perfil.IntervaloInicio2 > Perfil.IntervaloFim2)
                                    {
                                        DateTime? IntevaloFimMaisUm = Perfil.IntervaloFim2;
                                        DateTime DataHoraAtualMaisUm = DataHoraAtual;
                                        if (DataHoraAtual <= Perfil.IntervaloFim2)
                                            DataHoraAtualMaisUm = DataHoraAtual.AddDays(1);
                                        if (DataHoraAtualMaisUm >= Perfil.IntervaloInicio2 && DataHoraAtualMaisUm <= IntevaloFimMaisUm)
                                        {
                                            Autorizado = false;
                                        }
                                        else
                                        {
                                            if (EncerramentoSessao >= Perfil.IntervaloInicio2 && EncerramentoSessao <= Perfil.IntervaloFim2)
                                            {
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.IntervaloInicio2.Value.ToShortTimeString() + " e retornar as " + Perfil.IntervaloFim2.Value.ToShortTimeString() + " !!!', 'Atenção');", true);
                                            }
                                        }
                                    }
                                }
                                if (Perfil.IntervaloInicio3.HasValue)
                                {
                                    if (Perfil.IntervaloInicio3 < Perfil.IntervaloFim3)
                                    {
                                        if (DataHoraAtual >= Perfil.IntervaloInicio3 && DataHoraAtual < Perfil.IntervaloFim3)
                                        {
                                            Autorizado = false;
                                        }
                                        else
                                        {
                                            if (EncerramentoSessao >= Perfil.IntervaloInicio3 && EncerramentoSessao <= Perfil.IntervaloFim3)
                                            {
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.IntervaloInicio3.Value.ToShortTimeString() + " e retornar as " + Perfil.IntervaloFim3.Value.ToShortTimeString() + "!!!', 'Atenção');", true);
                                            }
                                        }
                                    }
                                    else if (Perfil.IntervaloInicio3 > Perfil.IntervaloFim3)
                                    {
                                        DateTime? IntevaloFimMaisUm = Perfil.IntervaloFim3;
                                        DateTime DataHoraAtualMaisUm = DataHoraAtual;
                                        if (DataHoraAtual <= Perfil.IntervaloFim3)
                                            DataHoraAtualMaisUm = DataHoraAtual.AddDays(1);
                                        if (DataHoraAtualMaisUm >= Perfil.IntervaloInicio3 && DataHoraAtualMaisUm <= IntevaloFimMaisUm)
                                        {
                                            Autorizado = false;
                                        }
                                        else
                                        {
                                            if (EncerramentoSessao >= Perfil.IntervaloInicio3 && EncerramentoSessao <= Perfil.IntervaloFim3)
                                            {
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.IntervaloInicio3.Value.ToShortTimeString() + " e retornar as " + Perfil.IntervaloFim3.Value.ToShortTimeString() + " !!!', 'Atenção');", true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }//fecha foreach
                    if (!Autorizado)
                    {
                        if (Perfil.DataHoraInicio.HasValue && DataHoraAtual >= Perfil.DataHoraInicio && DataHoraAtual <= Perfil.DataHoraFim)
                        {
                            Autorizado = true;
                        }
                        else
                        {
                            if (EncerramentoSessao >= Perfil.DataHoraFim)
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.info('Sua Sessão no sistema irá se encerrar as " + Perfil.DataHoraFim.Value.ToShortTimeString() + " !!!', 'Atenção');", true);
                            }
                        }
                    }
                }
                if (Autorizado)
                {
                    Session["DataHoraVerificaPendencias"] = DataHoraAtual;
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }

                VerificaIntegracaoDocumentoEletronico();
            }
        }

        protected void VerificaIntegracaoDocumentoEletronico()
        {
            List<IntegraDocumentoEletronico> ListaIntegra = new List<IntegraDocumentoEletronico>();
            IntegraDocumentoEletronicoDAL integraDAL = new IntegraDocumentoEletronicoDAL();

            ListaIntegra = integraDAL.ListarIntegraDocEletronicoRejeitados(1, 0, Convert.ToInt32(Session["CodUsuario"]));
            foreach (IntegraDocumentoEletronico integraDoc in ListaIntegra)
            {
                int contador = 0;
                if (integraDoc.CodigoAcao == 124)
                {
                    if (contador != 0)
                        return;

                    List<EventoEletronicoDocumento> ListEventos = new List<EventoEletronicoDocumento>();
                    EventoEletronicoDocumentoDAL EventoEleDAL = new EventoEletronicoDocumentoDAL();
                    ListEventos = EventoEleDAL.ObterEventosEletronicos(integraDoc.CodigoDocumento);
                    IEnumerable<EventoEletronicoDocumento> EventosInviados = ListEventos.Where((EventoEletronicoDocumento c) => { return c.CodigoSituacao != 124; }).OrderByDescending(x => x.CodigoEvento).ToList(); ;
                    foreach (var evento in EventosInviados)
                    {
                        contador++;
                        if (evento.CodigoSituacao == 121)
                            Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.success(' " + evento.Retorno + "','Documento n° " + integraDoc.CodigoDocumento + ", Evento n° " + evento.CodigoEvento + "');", true);
                        else
                            Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.error(' " + evento.Retorno + "','Documento n° " + integraDoc.CodigoDocumento + ", Evento n° " + evento.CodigoEvento + "');", true);
                    }
                }
                else
                {
                    string mensagem = "";
                    if (integraDoc.Mensagem.Length <= 75)
                        mensagem = integraDoc.Mensagem.Substring(0, integraDoc.Mensagem.Length);
                    else
                        mensagem = integraDoc.Mensagem.Substring(0, 72) + "...";

                    Page.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "toastr.error(' " + mensagem + "','Documento n° " + integraDoc.CodigoDocumento + "');", true);
                }

                Session["CD_DOC_INTEGRA_VISUALIZADO"] = integraDoc.Codigo;
                integraDoc.RegistroMensagem++;

                integraDAL.AtualizarIntegraDocEletronico(integraDoc);
            }
        }

        protected void btnMaxMenu_Click(object sender, EventArgs e)
        {
            TamanhoDivCorpo = "col-md-10";
            TamanhoDivMenu = "col-md-2";
            btnMinMenu.Visible = true;
            btnMaxMenu.Visible = false;
            MenuItemCollection menuCollection = new MenuItemCollection();
            menuCollection = (MenuItemCollection)Menu2.Items;
            for (int i = 0; i < menuCollection.Count; i++)
            {
                if (menuCollection[i].ChildItems.Count > 0)
                    menuCollection[i].Text = menuCollection[i].ToolTip + "<span class='caret'/></span>";
                else
                    menuCollection[i].Text = menuCollection[i].ToolTip;
                Menu2.Items.AddAt(i,menuCollection[i]);

            }
            // HttpContext.Current.Application["MenuMaximizado"] = true;
            Cache["MenuMaximizadoUsuario" + Session["CodUsuario"]] = true;
        }

        protected void btnMinMenu_Click(object sender, EventArgs e)
        {
            TamanhoDivCorpo = "col-md-11";
            TamanhoDivMenu = "col-md-1";
            btnMaxMenu.Visible = true;
            btnMinMenu.Visible = false;
            MenuItemCollection menuCollection = new MenuItemCollection();
            menuCollection = (MenuItemCollection)Menu2.Items;
            int t = menuCollection.Count;
            for (int i = 0; i < menuCollection.Count; i++)
            {
                menuCollection[i].Text = "";
                Menu2.Items.AddAt(i, menuCollection[i]);
            }
            // HttpContext.Current.Application["MenuMaximizado"] = false;
            Cache["MenuMaximizadoUsuario" + Session["CodUsuario"]] = false;

        }
    }
}