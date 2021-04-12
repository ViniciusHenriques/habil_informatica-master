<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="EncInventario.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.EncInventario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <%--NOVOS LINKS--%>

   <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />

    <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />


    <script type="text/javascript">
        function ShowPopup() {
            $("#btnShowPopup").click();
        }

         function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }

        $(document).ready(function () {
            //Esconde preloader
            $(window).load(function(){
                $('#preloader2').fadeOut(500);//1500 é a duração do efeito (1.5 seg)
            });
        });
    </script>
    <style type="text/css">
        .buttonGrid{
            padding:0!important;           
        }
        
        #preloader2 {
            position: absolute; /* posição absoluta ao elemento pai, neste caso o BODY */
            left: 50%!important; 
            top: 60%;
            width: 300px; /* Largura da DIV */
            height: 200px; /* Altura da DIV */
            opacity:0.8;
            margin-left: -150px;
            margin-top: -125px;
            background-color: #FFF;
            color: #FFF;
            background-color: #FFF;
            text-align: center; /* Centraliza o texto */
            z-index: 1000; /* Faz com que fique sobre todos os elementos da página */
        }
    </style>

<div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
    <div class="panel panel-primary">
        <div class="panel-heading">
            Encerramento de Inventário
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
                            <asp:GridView ID="grdGrid" runat="server" Width="100%" GridView="800" 
                                CssClass="table table-bordered table-hover table-striped"
                                GridLines="None" AutoGenerateColumns="False"
                                Font-Size="8pt" OnRowCommand="grdGrid_SelectedIndexChanged"
                                AllowPaging="true" PageSize="50" PagerSettings-Mode="NumericFirstLast">
                                <Columns>
                                    <asp:BoundField DataField="DtGeracao" HeaderText="Data Lançamento" />
                                    <asp:BoundField DataField="CodigoIndice" HeaderText="Código Lançamento" />
                                    <asp:BoundField DataField="DescInventario" HeaderText="Descrição Inventário"  />
                                    <asp:BoundField DataField="DescSituacao" HeaderText="Situação" />
                                    <asp:BoundField DataField="CodigoUsuario" HeaderText="Usuário" />
                                    <asp:BoundField DataField="CodigoMaquina" HeaderText="Maquina" />
                                    <asp:BoundField DataField="NrContagem" HeaderText="Contagem" />

                                    <asp:TemplateField HeaderText="Ação" >
                                        <ItemTemplate>
                                                <asp:LinkButton ID="btnEncerrar" runat="server" CssClass="btn btn-link buttonGrid" CommandName="EncerrarInventario" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Visible='<%# Eval("BtnEncerrar") %>' title="Encerrar" >
                                                    <img runat="server" src="../../Images/autorizar.png" width="20" height="20"/>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>

                        </div>                  
                    </div>                                
                </div>                                 
            </asp:Panel>



        </div>
    </div>
</div>   

    <img id="preloader2" src="../../Images/aguarde.gif" style="align-content:center;" class="load" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="MyPopup" class="modal fade" role="dialog">
                 <div class="modal-dialog" role="document" style="height: 100%; width: 400px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H4"></h4>
                        </div>
                        <div class="modal-body">

                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnSimEncerrar" runat="server" Text="Sim" CssClass="btn btn-success" TabIndex="-1" AutoPostBack="true" OnClick="btnSimEncerrar_Click"></asp:LinkButton>
                            <button type="button" class="btn btn-danger" data-dismiss="modal" id="btnNao3">Não</button>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger  ControlID="btnSimEncerrar" />

        </Triggers>
    </asp:UpdatePanel>


</asp:Content>

