<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="RelMovEstoque.aspx.cs" 
    Inherits="SoftHabilInformatica.Pages.Estoque.RelMovEstoque"  MaintainScrollPositionOnPostback="True" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />

    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
        
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary" >
            <div  class="panel-heading">Relatório de Movimentação de Estoque
                <div class="messagealert" id="alert_container"></div>
            </div>
            
            <div class="panel-body">
                <div class="col-md-1">
                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" TabIndex="1" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                    </asp:LinkButton>
                </div>
                <div class="col-md-1">
                    <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click" TabIndex="2" >
                        <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                    </asp:LinkButton>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlRegistros" Visible="false" CssClass="form-control" runat="server" TabIndex="3" Font-Size="Medium" >
                        <asp:ListItem Value="1" Text="Relatório de Estoque" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Relatórios de Entradas"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Relatórios de Saídas"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:LinkButton ID="btnGerar" runat="server" Visible="false" Text="Salvar" CssClass="btn btn-success" TabIndex="4"  OnClick="BtnGerar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Gerar
                    </asp:LinkButton>
                </div>
                <br />
                <br />
                <div class="col-md-2"></div>
                <asp:HiddenField ID="TabName" runat="server"/>
                    <CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="True" HasRefreshButton="True"
                                EnableDatabaseLogonPrompt="true"  ToolPanelWidth="0" Width="100%" CssClass="col-md-8"  />
            </div>
        </div>
    </div>

</asp:Content>
<%--  --%>