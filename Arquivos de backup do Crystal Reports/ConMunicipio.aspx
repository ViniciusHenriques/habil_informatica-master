<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConMunicipio.aspx.cs" Inherits="SoftHabilInformatica.Pages.CEPs.ConMunicipio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script type="text/javascript">
      function openModal() {
          $('#myModalMsg').modal('show');
      }
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
        <div class="panel panel-primary" >
            <div  class="panel-heading">Consulta de Municípios</div>
            <div class="panel-body">
                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>
                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>

                <asp:LinkButton ID="btnMensagem" runat="server" Text="Mensagem" Visible="false" class="btn btn-default" data-toggle="modal"  data-target="#myModalMsg" OnClick="btnMensagem_Click"> 
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

                <br />
                <br />
                <div class="input-group">
                    <span class="input-group-addon">Pesquisar por: <asp:DropDownlist id="ddlPesquisa" AutoPostBack="True" runat="server" OnSelectedIndexChanged="cboSelecionar"/></span>
                    <asp:TextBox ID="txtVarchar" CssClass="form-control" Width="150" runat="server" TabIndex="1" />
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
                <asp:GridView ID="grdGrid" runat="server" Width="100%" 
                    CssClass ="table table-bordered table-hover table-striped" 
                    GridLines="None" AutoGenerateColumns="False" 
                    Font-Size="8pt" 
                    OnSelectedIndexChanged="grdGrid_SelectedIndexChanged" 
                    AllowPaging="true" PageSize= "10" 
                    OnPageIndexChanging="grdGrid_PageIndexChanging" 
                    PagerSettings-Mode ="NextPrevious"
                    PagerSettings-FirstPageText="/Prim/"
                    PagerSettings-PreviousPageText="/Ant/" 
                    PagerSettings-NextPageText="/Próx/"
                    PagerSettings-LastPageText="/Últ/"  >
                    <Columns>
                        <asp:BoundField DataField="CodigoMunicipio" HeaderText="Cód. Município" />
                        <asp:BoundField DataField="CodigoEstado" HeaderText="Cód. Estado" />
                        <asp:BoundField DataField="Sigla" HeaderText="Sigla" />
                        <asp:BoundField DataField="DescricaoMunicipio" HeaderText="Descrição do Município" />
                        <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />                      

                    </Columns>
                    <RowStyle CssClass="cursor-pointer" />
                </asp:GridView>        
        </div>
    </div>
    </div>

</asp:Content>
<%--  --%>