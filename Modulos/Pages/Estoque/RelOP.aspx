<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="RelOP.aspx.cs" 
    Inherits="SoftHabilInformatica.Pages.Estoque.RelOP"  MaintainScrollPositionOnPostback="True" %>

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
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script> 
    
    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
            $('.js-example-basic-single').select2({
            });
        });
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({
            });
        }       
    </script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary" >
            <div  class="panel-heading">Relatório de Ordem de Produção
                <div class="messagealert" id="alert_container"></div>
            </div>
            
            <div class="panel-body">
                    <div class="col-md-1" >
                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false"  OnClick="btnVoltar_Click"> 
                            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-1" >
                        <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                            <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-4" >
                        <asp:DropDownList ID="ddlImpressora" CssClass="form-control js-example-basic-single" runat="server" Font-Size="Small" Width="100%">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4" >
                        <asp:LinkButton ID="btnImprimir" runat="server"  CssClass="btn btn-success" OnClick="btnImprimir_Click" >
                            <span aria-hidden="true" title="Imprimir Orçamento" class="glyphicon glyphicon-print"></span>  Imprimir
                        </asp:LinkButton>                                
                    </div>

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