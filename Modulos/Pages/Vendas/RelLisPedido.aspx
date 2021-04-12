<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="RelLisPedido.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.RelLisPedido" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <%--NOVOS LINKS--%>

   <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />

    <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

<div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
    <div class="panel panel-primary">
        <div class="panel-heading">
            Relatório para Listagem de Pedido
            <div class="messagealert" id="alert_container"></div>
        </div>
        <div class="panel-body">
            <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar"  TabIndex="2" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
            </asp:LinkButton>
            <asp:LinkButton ID="btnImprimir" runat="server" Visible="true" Text="Nova Consulta" CssClass="btn btn-success" OnClick="btnImprimir_Click"> 
                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-print"></span> Imprimir
            </asp:LinkButton>
            <asp:Panel id="pnlRel" runat="server" Visible="true">
                <div class="row" style="margin-top: 20px; padding-left: 30px; padding-right:50px;">
                    <div class="col-md-12">
                        <CR:CrystalReportViewer ID="CRViewer" Visible="true"  runat="server" AutoDataBind="True" HasRefreshButton="True" HasPrintButton="True" HasExportButton="True"     
                                EnableDatabaseLogonPrompt="true" Height="100%" ToolPanelWidth="200px" Width="100%"  />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>

        


</asp:Content>

