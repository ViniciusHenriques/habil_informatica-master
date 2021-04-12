<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConEstado.aspx.cs" Inherits="SoftHabilInformatica.Pages.CEPs.ConEstado" %>

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
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    </head>

    <script src="../../Scripts/jquery-2.1.4.min.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <script type="text/javascript">
        function openModal() {
            $('#myModalMsg').modal('show');
        }
    </script>

    <body>
    <form id="form1" runat="server">

        <div class="input-group">
            <span class="input-group-addon" id="basic-addon1">Estado Selecionado</span>
            <asp:TextBox ID="txtCodigo" runat="server" Enabled="false" Width="100" />
            <asp:TextBox ID="txtDescricao" runat="server" Width="600" Enabled="false" />
        </div>
        <br />
        <br />
        <div class="input-group">
            <span class="input-group-addon">Pesquisar por:
                <asp:DropDownList ID="ddlPesquisa" AutoPostBack="True" runat="server" OnSelectedIndexChanged="cboSelecionar" /></span>
            <asp:TextBox ID="txtVarchar" CssClass="form-control" Width="150" runat="server"/>
            &nbsp&nbsp&nbsp&nbsp
            Mostrar os Primeiros
                    <asp:DropDownList ID="ddlRegistros" runat="server" Font-Size="Medium" OnSelectedIndexChanged="ddlRegistros_SelectedIndexChanged">
                        <asp:ListItem Value="10" Text="10" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="20" Text="20"></asp:ListItem>
                        <asp:ListItem Value="50" Text="50"></asp:ListItem>
                        <asp:ListItem Value="100" Text="100"></asp:ListItem>
                    </asp:DropDownList>
            Registros
            &nbsp&nbsp&nbsp&nbsp
                    <asp:LinkButton ID="btnConsultar" runat="server" UseSubmitBehavior="false" CssClass="btn btn-default" OnClick="btnConsultar_Click"> 
                        <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-search"></span>  Consultar
                    </asp:LinkButton>
        </div>

        <asp:LinkButton ID="btnMensagem" runat="server" Text="Mensagem" class="btn btn-default" data-toggle="modal"  data-target="#myModalMsg" OnClick="btnMensagem_Click"> 
        </asp:LinkButton>

        <asp:Panel ID ="pnlMensagem" runat="server" Visible="false" >
                <!-- Modal -->
                <div class="modal fade" id="myModalMsg" role="dialog">
                <div class="modal-dialog modal-sm" style="width:30%;position: fixed; top:30%; left:35%;">
                    <div class="modal-content">
                        <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h5 class="modal-title" id="H3">Atenção</h5>
                        </div>
                        <div class="modal-body">
                            <p><asp:Label ID ="lblMensagem" runat="server" Font-Size="Small"></asp:Label></p>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCfmNao" runat="server" Text="Fechar" CssClass="btn btn-default" data-dismiss="modal" TabIndex="-1" UseSubmitBehavior="false"  OnClick="btnCfmNao_Click"></asp:Button>
                        </div>
                    </div>
                </div>
                </div>
        </asp:Panel>

        <br />

        <asp:GridView ID="grdEstado" runat="server" Width="100%"
            CssClass="table table-bordered table-hover table-striped"
            GridLines="None" AutoGenerateColumns="False"
            Font-Size="8pt" BackColor="#99FFCC"
            OnSelectedIndexChanged="grdEstado_SelectedIndexChanged"
            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdEstado_PageIndexChanging"
            PagerSettings-Mode="NextPrevious"
            PagerSettings-FirstPageText="/Prim/"
            PagerSettings-PreviousPageText="/Ant/"
            PagerSettings-NextPageText="/Próx/"
            PagerSettings-LastPageText="/Últ/">
            <Columns>
                <asp:BoundField DataField="CodigoEstado" HeaderText="Código" />
                <asp:BoundField DataField="Sigla" HeaderText="Sigla" />
                <asp:BoundField DataField="DescricaoEstado" HeaderText="Descrição" />
                <asp:CommandField HeaderText="Ação"
                    ButtonType="Button" ControlStyle-CssClass="btn btn-info" SelectText="Acessar" FooterStyle-Font-Size ="X-Small"  ShowSelectButton="True" />
            </Columns>
            <RowStyle CssClass="cursor-pointer" />
        </asp:GridView>
    </form>
</body>
</html>
