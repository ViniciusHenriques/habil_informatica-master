<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="Welcome.aspx.cs" Inherits="SoftHabilInformatica.Pages.Welcome" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link rel="stylesheet" href='../Content/bootstrap3/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='../Content/bootstrap3/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='../Content/bootstrap3/bootstrap.min.js'></script>

    <asp:Panel ID="pnlFigura" runat ="server" HorizontalAlign ="Center">
        <div class="PapelDeParedePadrao"  title="MSoftGate" Style="height:100%; width:100%; margin-top:0px!important;" ></div>

    </asp:Panel>     

    <asp:Panel ID="pnlSelecao" runat="server" Visible="false" >
        <div id="divNavTeste" style="padding-left: 300px; padding-top: 100px; padding-right: 300px">
            <div class="panel panel-default">
                <div class="panel-heading">Selecione a Empresa para Acesso</div>
                <div class="panel-body">
                    <asp:GridView ID="grdGrid" runat="server" Width="100%"
                        CssClass="table table-bordered table-hover table-striped"
                        GridLines="None" AutoGenerateColumns="False"
                        Font-Size="8pt" 
                        OnSelectedIndexChanged="grdGrid_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="CodigoEmpresa" HeaderText="Código" />
                            <asp:BoundField DataField="NomeEmpresa" HeaderText="Razão Social" />
                            <asp:BoundField DataField="NomeFantasia" HeaderText="Fantasia" />
                            <asp:CommandField HeaderText="Acesso" ButtonType="Button" ControlStyle-CssClass="btn btn-info" SelectText="Acessar" FooterStyle-Font-Size ="X-Small"  ShowSelectButton="True" />
                        </Columns>
                        <RowStyle CssClass="cursor-pointer" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
