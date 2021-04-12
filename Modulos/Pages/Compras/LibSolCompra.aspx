<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="LibSolCompra.aspx.cs"
    Inherits="SoftHabilInformatica.Pages.Compras.LibSolCompra" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });


    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <style type="text/css">
        .buttonGrid {
            padding: 0 !important;
        }

        .GridPager {
            text-align: center !important;
            glyph-orientation-horizontal: inherit;
        }

        @media screen and (max-width: 800px) {
            .noprint {
                display: none;
            }
        }

        .listButton {
            background-color: transparent;
            border: 0 !important;
            padding: 1px !important;
            padding-left: 20px !important;
            padding-right: 20px !important;
        }

        .dropdown-menu {
            padding-top: 1px !important;
        }

        .padding-top-15 {
            padding-top: 14px !important;
        }

        .padding-top-10 {
            padding-top: 10px !important;
        }
    </style>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>



    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Aprovação de Solicitações de Compra 
               
                <div class="messagealert" id="alert_container"></div>
            </div>
            <br />

            <div class="col-md-3">
                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                        <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
            </div>

            <div class="col-md-12" style="margin-top: 15px">

                <br />

            </div>

            <div id="Tabs" role="tabpanel">

                <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist" id="myTabs">
                    <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-filter"></span>&nbsp;&nbsp;Seleção dos Dados</a></li>
                    <li role="presentation"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Liberação</a></li>
                </ul>

                <!-- Tab panes -->

                <div class="tab-content" runat="server" id="PanelContext">
                    <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">

                        <div class="container-fluid">

                            <div class="row">

                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-6" style="font-size: medium;">
                                            <label for="usr" style="margin-top: 1px;">Empresa <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                            <asp:DropDownList ID="ddlEmpresa" runat="server" TabIndex="4" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_SelectedIndexChanged" AutoPostBack="true" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="col-md-2" style="margin-top: 28px; margin-left: -20px;">
                                    <asp:Button ID="btnAtualiza" runat="server" Text="Consultar" CssClass="btn btn-default" UseSubmitBehavior="false" Width="100%" Height="33" OnClick="btnAtualiza_Click" />
                                </div>
                                <div class="col-md-4" style="font-size: medium;">
                                    <asp:LinkButton ID="btnConsultar" style="margin-top: 25px;" runat="server" ToolTip="1" Enabled="false" Text="Documentos" CssClass="btn alert-info col-md-12" UseSubmitBehavior="false" OnClick="btnConsultar_Click"> 
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">

                        <div class="row">
                            <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Comprador</label>
                                <asp:DropDownList ID="ddlComprador" runat="server" Width="100%" TabIndex="8" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                            </div>
                            <div class="col-md-9" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style="font-size: x-small;">Motivo da Situação</label>
                                <asp:TextBox ID="txtMotivo" CssClass="form-control" runat="server"  Font-Size="Small" TabIndex="10" />
                            </div> 
                        </div> 
                        <br/>
                        <div class="row">
                            <div class="col-md-5">
                                <asp:LinkButton ID="btnVoltarSelecao" runat="server" Text="Nova Consulta" CssClass="btn btn-default" usesubmitbehavior="false" OnClick="btnVoltarSelecao_Click"> 
                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>Nova Consulta
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnAprSolicitacao" runat="server" Text="Aprovar Solicitação" CssClass="btn btn-success" usesubmitbehavior="false" OnClick="btnAprSolicitacao_Click"> 
                            <span aria-hidden="true" title="Aprovar" ></span>   Aprovar Solicitação
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnCanSolicitacao" runat="server" Text="Devolver ao Vendedor" CssClass="btn btn-warning" usesubmitbehavior="false" OnClick="btnCanSolicitacao_Click"> 
                            <span aria-hidden="true" title="Cancelar" ></span>   Cancelar Solicitação
                                </asp:LinkButton>

                            </div>
                            <div class="col-md-5">
                                <asp:Label ID="lblLiberacao" runat="server" Font-Bold="true" ForeColor="DarkRed" Font-Size="Large" Text=""></asp:Label>
                            </div>
                        </div>

                        <div class="row"></div>
                        <br />

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:GridView ID="grdGrid" runat="server" OnRowCommand="grdGrid_RowCommand" CssClass="table table-bordered table-striped" Width="100%"
                                    GridLines="None" AutoGenerateColumns="False"
                                    Font-Size="8pt"
                                    PageSize="50" AllowPaging="true"
                                    PagerSettings-Mode="NumericFirstLast">
                                    <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="Ação" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkLiberar" runat="server" Checked='<%# Eval("Oper1").ToString().Equals("True") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CodigoSolicitacao" HeaderText="NºSolicitação" ItemStyle-CssClass="padding-top-15" />
                                        <asp:BoundField DataField="DataEmissao" HeaderText="Dt. Emissão" DataFormatString="{0:dd/MM/yyyy dd:mm}" ItemStyle-CssClass="padding-top-15 noprint" HeaderStyle-CssClass="noprint" />
                                        <asp:BoundField DataField="DataValidade" HeaderText="Dt. Validade" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="padding-top-15 noprint" HeaderStyle-CssClass="noprint" />
                                        <asp:BoundField DataField="CodigoFornecedor" HeaderText="Cód.For" ItemStyle-CssClass="padding-top-15" />
                                        <asp:BoundField DataField="NomeFornecedor" HeaderText="Fornecedor" ItemStyle-CssClass="padding-top-15" />
                                        <asp:BoundField DataField="ValorVerba" HeaderText="Valor Verba" DataFormatString="{0:C}" ItemStyle-CssClass="padding-top-15" />
                                        <asp:BoundField DataField="CodigoUsuario" HeaderText="Cód.Usu" ItemStyle-CssClass="padding-top-15" />
                                        <asp:BoundField DataField="NomeUsuario" HeaderText="Usuário" ItemStyle-CssClass="padding-top-15" />
                                        <asp:BoundField DataField="CodigoDocumento" HeaderText="Documento" ItemStyle-CssClass="padding-top-15" />
                                        <asp:BoundField DataField="CodigoIndice" HeaderText="Indice" ItemStyle-CssClass="padding-top-15" />

                                        <asp:TemplateField HeaderText="Acessar">
                                            <ItemTemplate>
                                                <div class="dropdown">
                                                    <asp:LinkButton runat="server" type="button" class="btn btn-link dropdown-toggle buttonGrid" data-toggle="dropdown">
                                                            <img runat="server" src="../../Images/configuracao.png" width="20" height="20"/><span class="caret" style="color:black;margin-left:5px"></span>
                                                    </asp:LinkButton>

                                                    <ul class="dropdown-menu dropdown-menu-right">
                                                        <li><a>
                                                            <asp:LinkButton runat="server" ID="opPedido" CssClass="listButton"
                                                                Text="Ver Solicitação"
                                                                CommandName="Pedido"
                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><span class="glyphicon glyphicon-edit" style="margin-right:10px!important"></span>Pedido</asp:LinkButton></a></li>
                                                    </ul>
                                                </div>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <RowStyle CssClass="cursor-pointer" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<%--  --%>