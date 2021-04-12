﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConModSistema.aspx.cs" Inherits="GestaoInterna.PagesAdmin.ConModSistema" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <div class="panel panel-primary">
            <div  class="panel-heading">Consulta de Módulos do Sistema</div>
            <div class="panel-body" >

                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>

                <asp:LinkButton ID="btnImprimir" runat="server"  UseSubmitBehavior="false" CssClass="btn btn-warning" OnClick="btnImprimir_Click"> 
                    <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-print"></span>  Imprimir
                </asp:LinkButton>
                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
                <br />
                <br />
                <div class="input-group">
                    <span class="input-group-addon">Pesquisar por: <asp:DropDownlist id="ddlPesquisa" AutoPostBack="True" runat="server" OnSelectedIndexChanged="cboSelecionar"/></span>
                    <asp:TextBox ID="txtVarchar" CssClass="form-control" Width="150" runat="server" PlaceHolder="Informe Texto"  Visible ="false" />
                    <asp:TextBox ID="txtInt" CssClass="form-control" Width="150" runat="server" PlaceHolder="Informe Valor Numérico" pattern="[0-9]+$" Visible ="false"  />
                    <asp:TextBox ID="txtValor" CssClass="form-control" Width="150" runat="server" PlaceHolder="Informe Valor Moeda" pattern="([0-9]{1,3}\.)?[0-9]{1,3},[0-9]{2}$" Visible ="false"  />
                    Mostrar os Primeiros
                    <asp:DropDownList ID="ddlRegistros" runat="server" Font-Size="Medium" OnSelectedIndexChanged="ddlRegistros_SelectedIndexChanged">
                        <asp:ListItem Value="10" Text="10" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="20" Text="20" ></asp:ListItem>
                        <asp:ListItem Value="50" Text="50" ></asp:ListItem>
                        <asp:ListItem Value="100" Text="100" ></asp:ListItem>
                    </asp:DropDownList>              
                    Registros
                    <asp:LinkButton ID="btnConsultar" runat="server"  UseSubmitBehavior="false" CssClass="btn btn-default" OnClick="btnConsultar_Click"> 
                        <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-search"></span>  Consultar
                    </asp:LinkButton>
                </div>
                <br />    
                <asp:GridView ID="grdModSistema" runat="server" Width="100%" 
                    CssClass ="table table-hover table-striped" 
                    GridLines="None" AutoGenerateColumns="False" 
                    Font-Size="8pt" BackColor="#99FFCC" 
                    OnSelectedIndexChanged="grdModSistema_SelectedIndexChanged" 
                    AllowPaging="true" PageSize= "100" OnPageIndexChanging="grdModSistema_PageIndexChanging" 
                    PagerSettings-Mode ="NextPrevious"
                    PagerSettings-FirstPageText="/Prim/"
                    PagerSettings-PreviousPageText="/Ant/" 
                    PagerSettings-NextPageText="/Próx/"
                    PagerSettings-LastPageText="/Últ/"  >
                    <Columns>
                        <asp:BoundField DataField="CodigoModulo" HeaderText="Código" />
                        <asp:BoundField DataField="DescricaoModulo" HeaderText="Descrição" />
                        <asp:CommandField HeaderText="Ação" ButtonType="Button" ShowSelectButton="True" />
                    </Columns>
                    <RowStyle CssClass="cursor-pointer" />
                </asp:GridView>        
                <p>
                    <asp:Label ID="lblMensagem" runat="server" />
                </p>
            </div>
        </div>
    </div>   
    </asp:Content>
