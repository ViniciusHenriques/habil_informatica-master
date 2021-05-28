<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManCidadesRegraFrete.aspx.cs" Inherits="SoftHabilInformatica.Pages.Transporte.ManCidadesRegraFrete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>

        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});     

            item = '#home';
            $('#myTabs a[href="' + item + '"]').tab('show');
        }
        item = '#home';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
    </script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;" >
         
        <div class="panel panel-primary">
            <div class="panel-heading">Manutenção de cidades da Regra de frete
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                    <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                    </div> 
                </asp:Panel>
            </div>
            <div class="panel-body">        
                <div class="row" >
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="col-md-12" style="margin-top:15px!important;margin-bottom:15px!important">
                                <div class="row">
                                    <div class="col-md-6" style="font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Transportadora</label>
                                        <asp:DropDownList ID="ddlTransportadoras" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" Width="100% " OnSelectedIndexChanged="ddlTransportadoras_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-6" style="font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Região</label>
                                        <asp:DropDownList ID="ddlRegioes" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" Width="100% "  AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" style="margin-top:15px!important">
                                <div class="panel panel-primary">
                                    <div class="panel-heading" style="text-align:center">Cidade da Regra de frete</div>
                                    <div class="panel-body">
                                        <div class="col-md-12">
                                            <div class="col-md-4" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Estado</label>
                                                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true" Width="100%"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-7" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Cidade</label>
                                                <asp:DropDownList ID="ddlCidade" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" Width="100%"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-1" style="font-size:x-small; margin-top:20px;">
                                                <asp:Button runat="server" id="btnAdd" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="btnAdd_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar esta cidade a regra" TabIndex="5"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlEstado" EventName="SelectedIndexChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click"/>
                           
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div> 
    </div>
   
</asp:Content>
