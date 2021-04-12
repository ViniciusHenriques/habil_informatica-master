<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConParSistema.aspx.cs" Inherits="SoftHabilInformatica.Pages.Empresas.ConParSistema" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
        <div class="panel panel-primary" >
            <div  class="panel-heading">Manutenção de Parâmetros do Sistema
                <div class="messagealert" id="alert_container"></div>
            </div>
            <div class="panel-body">             
                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>

                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
                <br />
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
                        <asp:BoundField DataField="CodigoEmpresa" HeaderText="Código" />
                        <asp:BoundField DataField="NomeEmpresa" HeaderText="Nome da Empresa" />
                         <asp:CommandField HeaderText="Entrar" ShowSelectButton="True" 
                                        ItemStyle-Height ="15px" ItemStyle-Width ="40px" ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                        ControlStyle-Width ="20px" ControlStyle-Height ="20px" HeaderStyle-Width ="5%" />
                    </Columns>
                    <RowStyle CssClass="cursor-pointer" />
                </asp:GridView>        
        </div>
    </div>
    </div>

</asp:Content>
<%--  --%>