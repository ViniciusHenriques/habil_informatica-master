<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="RelEstoque.aspx.cs" 
    Inherits="SoftHabilInformatica.Pages.Estoque.RelEstoque"  MaintainScrollPositionOnPostback="True" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />

    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
        
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary" >
            <div  class="panel-heading">Relatório de Estoque
                <div class="messagealert" id="alert_container"></div>
            </div>
            
            <div class="panel-body">
                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false"  OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                        <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                    </asp:LinkButton>


                <br />
                <br />
                <div class="col-md-2"></div>
                <asp:HiddenField ID="TabName" runat="server"/>
                    <CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="True" HasRefreshButton="True"
                                EnableDatabaseLogonPrompt="true"  ToolPanelWidth="0" Width="100%" CssClass="col-md-8 "  />
            </div>
        </div>
    </div>

</asp:Content>
<%--  --%>