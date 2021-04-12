<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" 
    CodeBehind="ManEvtDocEletronico.aspx.cs" Inherits="SoftHabilInformatica.Pages.Transporte.ManEvtDocEletronico"  
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

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
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary">
            <div class="panel-heading">Evento Eletrônico do Documento
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body">
                <div class="row" style="background-color: white; border: none;">
                    <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">                           
                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click" > 
                            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnExcluir" runat="server" Text="Inativar" CssClass="btn btn-danger" TabIndex="3" data-toggle="modal" data-target="#myexcpes"> 
                            <span aria-hidden="true" title="Inativar" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>
                    </div>                      
                </div>
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none;">                      
                        <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 0px; padding-top: 20px; padding-right: 20px; font-size: small;">
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                <ContentTemplate>  
                                    <div class="col-md-10" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr" style="margin-top: 1px;">Tipo Evento</label>
                                        <asp:DropDownList ID="ddlTipoEvento" runat="server" TabIndex="1" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlTipoEvento_TextChanged" AutoPostBack="true"/>
                                    </div>  
                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr" style="margin-top: 1px;">Situação</label>
                                        <asp:DropDownList ID="ddlSituacao" runat="server" TabIndex="2" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" Enabled="false"/>
                                    </div> 
                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Sequência</label>
                                        <asp:TextBox ID="txtSequencia" CssClass="form-control" runat="server" Text ="" Font-Size="Small" enabled="false" TabIndex="3"/>
                                    </div>
                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Data/Hora Emissão</label>
                                        <asp:TextBox ID="txtDtHrEmissao" CssClass="form-control" runat="server" Text ="" Font-Size="Small" enabled="false" TabIndex="4"/>
                                    </div>
                                                             
                                    <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Chave Acesso</label>
                                        <asp:TextBox ID="txtChaveAcesso" CssClass="form-control" runat="server" Font-Size="Small" MaxLength="100" Enabled="false" TabIndex="5"/>
                                    </div>  
                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Motivo</label>
                                        <asp:TextBox ID="txtMotivo" CssClass="form-control" runat="server"  TabIndex="6" TextMode="multiline" Columns="10" Rows="5" MaxLength="200" onFocus="this.select()" />   
                                    </div> 
                                    </ContentTemplate> 
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTipoEvento" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>                                                            
                    </div>                           
                </div>
            </div>
        </div>
    </div>
    <!-- Excluir-->
    <div class="modal fade" id="myexcpes" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir este Evento eletrônico do Documento ?
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="False" OnClick="btnCfmSim_Click"></asp:LinkButton>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
