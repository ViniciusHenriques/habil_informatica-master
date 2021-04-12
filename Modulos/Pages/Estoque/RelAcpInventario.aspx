﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="RelAcpInventario.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.RelAcpInventario" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">

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
            Relatório de Acompanhamento de Inventário
            <div class="messagealert" id="alert_container"></div>
        </div>
        <div class="panel-body">
            <asp:LinkButton ID="btnSair" runat="server" onFocus="this.select()" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                        <span aria-hidden="true" title="Fechar" TabIndex="19" class="glyphicon glyphicon-off"></span>  Fechar
            </asp:LinkButton>
            <asp:Panel id="pnlGrid" runat="server">
                <div class="row" style="margin-top: 25px; padding-left: 15px; padding-right:15px;">
                    <div class="col-md-12">
                        <div style = "overflow-x: auto;" class="row">
                            <asp:GridView ID="grdGrid" runat="server" Width="100% " GridView1="800" 
                                CssClass="table table-bordered table-hover table-striped"
                                GridLines="None" AutoGenerateColumns="False"
                                Font-Size="8pt"
                                OnSelectedIndexChanged="grdGrid_SelectedIndexChanged"
                                AllowPaging="true" PageSize="50"
                                PagerSettings-Mode="NumericFirstLast">
                                    <Columns>
                                        <asp:BoundField DataField="DtGeracao" HeaderText="Descrição do Produto" />
                                        <asp:BoundField DataField="CodigoIndice" HeaderText="Código Lançamento" />
                                        <asp:BoundField DataField="DescInventario" HeaderText="Descrição de Inventario"  />
                                        <asp:BoundField DataField="DescSituacao" HeaderText="Situação" />
                                        <asp:BoundField DataField="CodigoUsuario" HeaderText="Usuario" />
                                        <asp:BoundField DataField="CodigoMaquina" HeaderText="Maquina" />
                                        <asp:BoundField DataField="NrContagem" HeaderText="Contagem" />
                                        <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                            ItemStyle-Height="15px" ItemStyle-Width="50px"
                                            ButtonType="Image" SelectImageUrl="~/Images/Acessar.svg"
                                            ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                    </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>
                        </div>                  
                    </div>                                
                </div>                                 
            </asp:Panel>
            <asp:Panel id="pnlRel" runat="server" Visible="false">
                <div class="col-md-1">
                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>
                    </div>                
                <br />
                <br />  
                <div class="row" style="margin-top: 20px; padding-left: 30px; padding-right:50px;">
                    <div class="col-md-12">
                        <CR:CrystalReportViewer ID="CRViewer" Visible="false" runat="server" AutoDataBind="True" HasRefreshButton="True"
                                EnableDatabaseLogonPrompt="False" Height="100%" ToolPanelWidth="200px" Width="100%" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
</asp:Content>

