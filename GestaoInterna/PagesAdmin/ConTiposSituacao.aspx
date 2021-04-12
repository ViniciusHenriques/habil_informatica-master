<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConTiposSituacao.aspx.cs" Inherits="GestaoInterna.PagesAdmin.ConTiposSituacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script type="text/javascript" src="../../Scripts/funcoes.js"></script>


    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">

        <div class="panel panel-primary">
            <div class="panel-heading">Consulta de Tipos e Situações</div>
            <div class="panel-body" >

                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>

                <asp:LinkButton ID="btnImprimir" runat="server" UseSubmitBehavior="false" CssClass="btn btn-warning" OnClick="btnImprimir_Click"> 
                    <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-print"></span>  Imprimir
                </asp:LinkButton>
                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
                <br />
                <br />
                <div class="input-group">
                    <span class="input-group-addon">Pesquisar por:
                        <asp:DropDownList ID="ddlPesquisa" AutoPostBack="True" runat="server" OnSelectedIndexChanged="cboSelecionar" /></span>
                    <asp:TextBox ID="txtVarchar" CssClass="form-control" Width="200" runat="server" PlaceHolder="Informe Texto" Visible="false" />
                    <asp:TextBox ID="txtInt" CssClass="form-control" Width="200" runat="server" PlaceHolder="Informe Valor Numérico" pattern="[0-9]+$" Visible="false" />
                    <asp:TextBox ID="txtValor" CssClass="form-control" Width="200" runat="server" PlaceHolder="Informe Valor Moeda" pattern="([0-9]{1,3}\.)?[0-9]{1,3},[0-9]{2}$" Visible="false" />

                    <asp:LinkButton ID="btnConsultar" runat="server" UseSubmitBehavior="false" CssClass="btn btn-default" OnClick="btnConsultar_Click"> 
                        <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-search"></span>  Consultar
                    </asp:LinkButton>
                </div>
                <br />
                <asp:GridView ID="grdSituacao" runat="server" Width="100%"
                    CssClass="table table-bordered table-hover table-striped"
                    GridLines="None" AutoGenerateColumns="False"
                    Font-Size="8pt" BackColor="#99FFCC"
                    OnSelectedIndexChanged="grdSituacao_SelectedIndexChanged"
                    AllowPaging="true" PageSize="50" OnPageIndexChanging="grdSituacao_PageIndexChanging"
                    PagerSettings-Mode ="NumericFirstLast" >

                    <PagerStyle HorizontalAlign = "Right" CssClass = "GridPager" />
                    <Columns>
                        <asp:BoundField DataField="CodigoTipo" HeaderText="Código" />
                        <asp:BoundField DataField="DescricaoTipo" HeaderText="Descrição" />
                        <asp:BoundField DataField="Observacao" HeaderText="OBS" />
                        <asp:CommandField HeaderText="Ação" ButtonType="Button" ShowSelectButton="True" />
                    </Columns>
                    <RowStyle CssClass="cursor-pointer" />
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
