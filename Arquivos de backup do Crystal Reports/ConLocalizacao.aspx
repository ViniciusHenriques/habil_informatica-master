<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConLocalizacao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.ConLocalizacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
        <div class="panel panel-primary" >
            <div  class="panel-heading">Consulta de Localizações - Endereçamento do Estoque
                <div class="messagealert" id="alert_container"></div>
                
            </div>
            <div class="panel-body">
                <br />
                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>

                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>

                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
                <br />
                <br />
                <div class="input-group">
                    <span class="input-group-addon">Pesquisar por: <asp:DropDownlist id="ddlPesquisa" AutoPostBack="True" runat="server" OnSelectedIndexChanged="cboSelecionar"/></span>
                    <asp:TextBox ID="txtVarchar" CssClass="form-control" Width="150" runat="server" TabIndex="1"  />
                    &nbsp&nbsp&nbsp&nbsp
                    Mostrar os Primeiros
                    <asp:DropDownList ID="ddlRegistros" runat="server" Font-Size="Medium" OnSelectedIndexChanged="ddlRegistros_SelectedIndexChanged">
                        <asp:ListItem Value="10" Text="10" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="20" Text="20" ></asp:ListItem>
                        <asp:ListItem Value="50" Text="50" ></asp:ListItem>
                        <asp:ListItem Value="100" Text="100" ></asp:ListItem>
                    </asp:DropDownList>              
                    Registros
                    &nbsp&nbsp&nbsp&nbsp    
                    <asp:LinkButton ID="btnConsultar" runat="server"  UseSubmitBehavior="false" CssClass="btn btn-default" OnClick="btnConsultar_Click"> 
                        <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-search"></span>  Consultar
                    </asp:LinkButton>
                </div>
                <br />    
                <asp:GridView ID="grdGrid" runat="server" Width="100%" 
                    CssClass ="table table-bordered table-hover table-striped" 
                    GridLines="None" AutoGenerateColumns="False" 
                    Font-Size="8pt" BackColor="#99FFCC" 
                    OnSelectedIndexChanged="grdGrid_SelectedIndexChanged" 
                    AllowPaging="true" PageSize= "10" 
                    OnPageIndexChanging="grdGrid_PageIndexChanging" 
                    PagerSettings-Mode ="NextPrevious"
                    PagerSettings-FirstPageText="/Prim/"
                    PagerSettings-PreviousPageText="/Ant/" 
                    PagerSettings-NextPageText="/Próx/"
                    PagerSettings-LastPageText="/Últ/"  >
                    <Columns>
                        <asp:BoundField DataField="CodigoIndice" HeaderText="Código" />
                        <asp:BoundField DataField="CodigoEmpresa" HeaderText="Código da Empresa" />
                        <asp:BoundField DataField="CodigoLocalizacao" HeaderText="Código da Localização" />
                        <asp:BoundField DataField="DescricaoLocalizacao" HeaderText="Descrição da Localização" />
                        <asp:CommandField HeaderText="Ação" ButtonType="Button" ControlStyle-CssClass="btn btn-info" SelectText="Acessar" FooterStyle-Font-Size ="X-Small"  ShowSelectButton="True" />
                    </Columns>
                    <RowStyle CssClass="cursor-pointer" />
                </asp:GridView>        
        </div>
    </div>
    </div>

</asp:Content>
<%--  --%>