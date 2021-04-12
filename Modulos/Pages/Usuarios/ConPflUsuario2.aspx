<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConPflUsuario2.aspx.cs" Inherits="SoftHabilInformatica.Pages.Usuarios.ConPflUsuario2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="../Content/bootstrap.css" rel="stylesheet" />
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script type="text/javascript" src="../../Scripts/funcoes.js"></script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

</head>

<script src="../../Scripts/jquery-2.1.4.min.js"></script>
<script src="../../Scripts/bootstrap.min.js"></script>
<script src="../../Scripts/funcoes.js"></script>
<script type="text/javascript">
    function openModal() {
        $('#myModalMsg').modal('show');
    }
</script>

<body>
    <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

    <form id="form1" runat="server">


        <asp:LinkButton ID="btnMensagem" runat="server" Text="Mensagem"  Visible="false" class="btn btn-default" data-toggle="modal"  data-target="#myModalMsg" OnClick="btnMensagem_Click"> 
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
                            <asp:Button ID="btnCfmNao2" runat="server" Text="Fechar" CssClass="btn btn-default" data-dismiss="modal" TabIndex="-1" UseSubmitBehavior="false"  OnClick="btnCfmNao2_Click"></asp:Button>
                        </div>
                    </div>
                </div>
                </div>
        </asp:Panel>


        <div class="container-fluid">
            <div class="row">
                <div class="col-md-3 col-xs-6">
                    <span class=" form-control" id="basic-addon1" >Perfil do Usuário Selecionado</span>
                </div>
                <div class="col-md-3 col-xs-6">
                    <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false"  />
                </div>
                <div class="col-md-6 col-xs-12">
                    <asp:TextBox ID="txtDescricao" CssClass="form-control" runat="server"  Enabled="false" />
                </div>
            </div>
        </div>
        <br />
        <div class="container-fluid">
            <div class="col-md-1 col-xs-12">
                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>Incluir
                </asp:LinkButton>
            </div>
            <div class="col-md-4 col-xs-7">
                 <span class="input-group-addon">Pesquisar por:
                    <asp:DropDownList ID="ddlPesquisa"  AutoPostBack="True" runat="server" width="60%" OnSelectedIndexChanged="cboSelecionar" /></span>
            </div>
            <div class="col-md-2 col-xs-5">
                <asp:TextBox ID="txtVarchar" CssClass="form-control"  runat="server" TabIndex="1"/>
            </div>
            <div class="col-md-5 col-xs-12">
                <div class="col-md-8 col-xs-7">
                     <span class="input-group-addon">Mostrar os Primeiros
                    <asp:DropDownList ID="ddlRegistros" runat="server" Font-Size="Medium" OnSelectedIndexChanged="ddlRegistros_SelectedIndexChanged" >
                        <asp:ListItem Value="10" Text="10" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="20" Text="20"></asp:ListItem>
                        <asp:ListItem Value="50" Text="50"></asp:ListItem>
                        <asp:ListItem Value="100" Text="100"></asp:ListItem>
                    </asp:DropDownList></span>
                </div>
               <div class="col-md-2 col-xs-5">
                    <asp:LinkButton ID="btnConsultar" runat="server" UseSubmitBehavior="false" CssClass="btn btn-default" OnClick="btnConsultar_Click" > 
                        <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-search"></span>  Consultar
                    </asp:LinkButton>
               </div>
               
            </div>
            <br />
            <br />
            <br />

            <asp:GridView ID="grdPflUsuario" runat="server" Width="100%"
                    CssClass="table table-bordered table-hover table-striped"
                    GridLines="None" AutoGenerateColumns="False"
                    Font-Size="8pt" BackColor="#99FFCC"
                    OnSelectedIndexChanged="grdPflUsuario_SelectedIndexChanged"
                    AllowPaging="true" PageSize="10" OnPageIndexChanging="grdPflUsuario_PageIndexChanging"
                    PagerSettings-Mode="NextPrevious"
                    PagerSettings-FirstPageText="/Prim/"
                    PagerSettings-PreviousPageText="/Ant/"
                    PagerSettings-NextPageText="/Próx/"
            PagerSettings-LastPageText="/Últ/">
            <Columns>
                <asp:BoundField DataField="CodigoPflUsuario" HeaderText="Código" />
                <asp:BoundField DataField="DescricaoPflUsuario" HeaderText="Descrição" />
                <asp:CommandField HeaderText="Ação"
                    ButtonType="Button" ControlStyle-CssClass="btn btn-info" SelectText="Acessar" FooterStyle-Font-Size ="X-Small"  ShowSelectButton="True" />
            </Columns>
            <RowStyle CssClass="cursor-pointer" />
        </asp:GridView>
        </div>
        
        

        
    </form>
</body>
</html>
