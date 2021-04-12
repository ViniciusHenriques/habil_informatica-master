<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="RelComposicao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Produtos.RelComposicao" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">

<div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
    <div class="panel panel-primary">
        <div class="panel-heading">
            Relatório de Composição
            <div class="messagealert" id="alert_container"></div>
        </div>
        <div class="panel-body">
            <asp:LinkButton ID="LinkButton1" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false"  OnClick="btnVoltar_Click"> 
                <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
            </asp:LinkButton>
            <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
            </asp:LinkButton>              
            <br />
            <br />  
            <div class="row" style="margin-top: 20px; padding-left: 30px; padding-right:50px;">
                <div class="col-md-12">
                    <CR:CrystalReportViewer ID="CRViewer" Visible="true" runat="server" AutoDataBind="True" ReuseParameterValuesOnRefresh="true" HasRefreshButton="True"
                            EnableDatabaseLogonPrompt="False" Height="100%" ToolPanelWidth="200px" Width="100%" />
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>

