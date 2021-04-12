<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="RelOrcamento.aspx.cs" 
    Inherits="SoftHabilInformatica.Pages.Vendas.RelOrcamento"  MaintainScrollPositionOnPostback="True" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
            $('.js-example-basic-single').select2({
            });
        });
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({
            });
        }       
    </script>

    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary" >
            <div  class="panel-heading">Visualização do Orçamento
                <div class="messagealert" id="alert_container"></div>
            </div>

            <div class="panel-body">
                <div class="col-md-12" style="margin-bottom:15px;padding:0!important">
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-4" >
                                <asp:LinkButton ID="btnVoltar" runat="server" CssClass="btn btn-info" UseSubmitBehavior="false"    OnClick="btnVoltar_Click"> 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnVisualizar" runat="server" CssClass="btn alert-info" OnClick="btnVisualizar_Click">
                                    <span aria-hidden="true" title="Vizualizar a impressão" class="glyphicon glyphicon-eye-open"></span>  Visualizar impressão
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn alert-info" OnClick="btnEditar_Click" Visible="false">
                                    <span aria-hidden="true" title="Vizualizar a impressão" class="glyphicon glyphicon-edit"></span>  Editar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnEnviarEmail" runat="server" CssClass="btn alert-info" OnClick="btnEnviarEmail_Click" Visible="false">
                                    <span aria-hidden="true" title="Enviar para o email do cliente" class="glyphicon glyphicon-envelope"></span>  Enviar por e-mail
                                </asp:LinkButton>
                            </div>
                             <div class="col-md-4" >
                                <asp:DropDownList ID="ddlImpressora" CssClass="form-control js-example-basic-single" runat="server" Font-Size="Small" Width="100%">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4" >
                                <asp:LinkButton ID="btnImprimir" runat="server"  CssClass="btn btn-success" OnClick="btnImprimir_Click" >
                                    <span aria-hidden="true" title="Imprimir Orçamento" class="glyphicon glyphicon-print"></span>  Imprimir
                                </asp:LinkButton>                                
                            </div>
                        </ContentTemplate> 
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnVisualizar" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnEnviarEmail"  />
                            <asp:AsyncPostBackTrigger ControlID="btnImprimir" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>              
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlInfos" runat="server">
                            <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Código Documento<span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                <asp:TextBox ID="txtCodigoDocumento" AutoPostBack="False" CssClass="form-control" runat="server" MaxLength="15" Text="" Enabled="false"/>
                            </div>
                            <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Número Documento<span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                <asp:TextBox ID="txtNroDocumento" AutoPostBack="False" CssClass="form-control" runat="server" MaxLength="15" Text="" Enabled="false"/>
                            </div>
                            <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Código Cliente<span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                <asp:TextBox ID="txtCodigoCliente" AutoPostBack="False" CssClass="form-control"  runat="server" MaxLength="15" Text="" Enabled="false"/>
                            </div>
                            <div class="col-md-6" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Cliente<span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                <asp:TextBox ID="txtCliente" AutoPostBack="False" CssClass="form-control"  runat="server" MaxLength="50" Text="" Enabled="false"/>
                            </div>
                            <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Contato(s) Cliente<span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                <asp:DropDownList ID="ddlContatos" AutoPostBack="true" CssClass="form-control js-example-basic-single" TabIndex="1" runat="server" OnSelectedIndexChanged="ddlContatos_SelectedIndexChanged"/>
                            </div>
                            <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">E-mail Cliente<span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                <asp:TextBox ID="txtEmailCliente" AutoPostBack="False" CssClass="form-control" TabIndex="3" runat="server" MaxLength="50" Text=""/>
                            </div>
                            <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Telefone Cliente</label>
                                <asp:TextBox ID="txtFoneCliente" AutoPostBack="False" CssClass="form-control" TabIndex="3" runat="server" MaxLength="50" Text=""/>
                            </div>
                            <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Prazo de entrega </label>
                                <asp:TextBox ID="txtPrazoEntrega" AutoPostBack="False" CssClass="form-control" TabIndex="4" runat="server" MaxLength="30" Text="" />
                            </div> 
                            
                            <div class="col-md-12" style="font-size: x-small;margin-top:3px">
                                <label for="usr" style ="margin-top:1px;">Consideração inicial do documento</label>
                                <asp:TextBox ID="txtConsInicial" CssClass="form-control" runat="server"  TabIndex="5" TextMode="multiline" Columns="10" Rows="5" MaxLength="50"  onFocus="this.select()" />        
                            </div>
                        </asp:Panel>
                    </ContentTemplate> 
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlContatos" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                            
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlCRViewer" runat="server" >
                            <div class="col-md-2"></div>
                            <div class="col-md-8">
                                <asp:HiddenField ID="TabName" runat="server"/>           
                                <CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="True" HasRefreshButton="True"
                                    EnableDatabaseLogonPrompt="true"  ToolPanelWidth="0" Width="100%" />
                            </div>
                        </asp:Panel>               
                    </ContentTemplate> 
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>
<%--  --%>