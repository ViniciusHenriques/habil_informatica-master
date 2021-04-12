<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConModSistema2.aspx.cs" Inherits="GestaoInterna.PagesAdmin.ConModSistema2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="../Content/bootstrap.css" rel="stylesheet" />
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script type="text/javascript" src="../../Scripts/funcoes.js"></script>

    <style type="text/css">
        .auto-style1 {
            width: 129px;
        }
    </style>

</head>

<script src="../../Scripts/jquery-2.1.4.min.js"></script>
<script src="../../Scripts/bootstrap.min.js"></script>

<body>
    <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

    <form id="form1" runat="server">


        <div class="input-group">
          <span class="input-group-addon" id="basic-addon1">Módulo do Sistema Selecionado</span>
          <asp:TextBox ID="txtCodigo" runat="server" Enabled="false" Width="100" />
          <asp:TextBox ID="txtDescricao" runat="server" Width="600" Enabled="false" />
        </div>
        <br />
        <br />
        <div class="input-group">
            <span class="input-group-addon">Pesquisar por:
                <asp:DropDownList ID="ddlPesquisa" AutoPostBack="True" runat="server" OnSelectedIndexChanged="cboSelecionar" /></span>
            <asp:TextBox ID="txtVarchar" CssClass="form-control" Width="150" runat="server" PlaceHolder="Informe Texto" Visible="false" />
            <asp:TextBox ID="txtInt" CssClass="form-control" Width="150" runat="server" PlaceHolder="Informe Valor Numérico" pattern="[0-9]+$" Visible="false" />
            <asp:TextBox ID="txtValor" CssClass="form-control" Width="150" runat="server" PlaceHolder="Informe Valor Moeda" pattern="([0-9]{1,3}\.)?[0-9]{1,3},[0-9]{2}$" Visible="false" />
            Mostrar os Primeiros
                    <asp:DropDownList ID="ddlRegistros" runat="server" Font-Size="Medium" OnSelectedIndexChanged="ddlRegistros_SelectedIndexChanged">
                        <asp:ListItem Value="10" Text="10" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="20" Text="20"></asp:ListItem>
                        <asp:ListItem Value="50" Text="50"></asp:ListItem>
                        <asp:ListItem Value="100" Text="100"></asp:ListItem>
                    </asp:DropDownList>
            Registros
                    <asp:LinkButton ID="btnConsultar" runat="server" UseSubmitBehavior="false" CssClass="btn btn-default" OnClick="btnConsultar_Click"> 
                        <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-search"></span>  Consultar
                    </asp:LinkButton>
        </div>
        <br />
        <asp:GridView ID="grdModSistema" runat="server" Width="100%" 
                    CssClass ="table table-bordered table-hover table-striped" 
                    GridLines="None" AutoGenerateColumns="False" 
                    Font-Size="8pt" BackColor="#99FFCC" 
                    OnSelectedIndexChanged="grdModSistema_SelectedIndexChanged" 
                    AllowPaging="true" PageSize= "10" OnPageIndexChanging="grdModSistema_PageIndexChanging" 
                    PagerSettings-Mode ="NextPrevious"
                    PagerSettings-FirstPageText="/Prim/"
                    PagerSettings-PreviousPageText="/Ant/" 
                    PagerSettings-NextPageText="/Próx/"
                    PagerSettings-LastPageText="/Últ/"  >
                    <Columns>
                        <asp:BoundField DataField="CodigoModulo" HeaderText="Código" />
                        <asp:BoundField DataField="DescricaoModulo" HeaderText="Descrição" />
                        <asp:CommandField HeaderText="Ação"  
                         ButtonType="Button" ShowSelectButton="True" />
                    </Columns>
                    <RowStyle CssClass="cursor-pointer" />
        </asp:GridView>        
        <p>
            <asp:Label ID="lblMensagem" runat="server" />
        </p>
    </form>
</body>
</html>
