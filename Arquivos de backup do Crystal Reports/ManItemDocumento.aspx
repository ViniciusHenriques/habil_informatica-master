<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManItemDocumento.aspx.cs" Inherits="SoftHabilInformatica.Pages.Vendas.ManItemDocumento" 
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server" >

    <meta charset="utf-8">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link rel="stylesheet" href="../../Content/bootstrap-datetimepicker.min.css">
    <script src="../../Scripts//moment.min.js" type="text/javascript"></script>
    <script src="../../Scripts/bootstrap-datetimepicker.min.js"></script>
    <script type="text/javascript" src="../../Scripts/moment-with-locales.js"></script>

    <script type="text/javascript" charset="utf-8">


        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            $('.js-example-basic-single').select2({
            });
            $('.datetimepicker1').datetimepicker({ locale: 'pt-br', format: 'DD/MM/YYYY HH:mm' });
        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
             $('.js-example-basic-single').select2({
                language: $.extend({},
                $.fn.select2.defaults.defaults.language, {
                  noResults: function() {
                    var term = id_categoria
                      .data('select2')
                      .$dropdown.find("input[type='search']")
                      .val();

                    return $("Nenhum resultado");
                  }
                })
            });
            $('.datetimepicker1').datetimepicker({ locale: 'pt-br', format: 'DD/MM/YYYY hh:mm' });
        }
    </script>
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary ">
            <div class="panel-heading panel-heading-padrao" >
                Manutenção dos Itens do Documento
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body" >
                <div class="row" style="background-color: white; border: none;">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                <asp:LinkButton ID="btnVoltar" runat="server"  CssClass="btn btn-info" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnVoltar_Click"> 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span> Voltar
                                </asp:LinkButton>
                                    <asp:LinkButton ID="btnSalvar" runat="server"  CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-save"></span> Salvar
                                </asp:LinkButton>
                                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                                    <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate> 
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnVoltar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none; margin-top:10px!important">                      
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                            <ContentTemplate>                                       
                                <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; margin-top:20px">                                                                                                                                                                                                                             
                                    <div class="row" style="margin-bottom:15px">                                                                
                                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr">Item </label>
                                            <asp:TextBox ID="txtCodItem"  CssClass="form-control" Enabled="false" TabIndex="1" runat="server"  MaxLength="50" AutoPostBack="true"/>
                                        </div>

                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Situação</label>
                                            <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" TabIndex="2" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                        </div>
                                        
                                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small; " >
                                            <label for="usr">Data/Hora Inicio </label>
                                            <div class='input-group' >
                                            <asp:TextBox ID="txtDtInicio" name="txtDtInicio" CssClass="form-control datetimepicker1" TabIndex="3" runat="server" MaxLength="50" onFocus="this.select()" />
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar "></span>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small; " >
                                            <label for="usr">Data/Hora Fim </label>
                                            <div class='input-group ' > 
                                                <asp:TextBox ID="txtDtFim" name="txtDtFim" CssClass="form-control datetimepicker1" TabIndex="4" runat="server" MaxLength="50" />
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar "></span>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-12" >
                                            <label for="usr" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">Usuário Responsável</label>
                                            <div class="input-group " style="width:100%">                              
                                                <asp:TextBox ID="txtCodUsu" CssClass="form-control" name="txtCodPessoa"  runat="server" TabIndex="5" OnTextChanged="txtCodUsu_TextChanged"   AutoPostBack="true"                                
                                                Width="15%" MaxLength="6" />                                                      
                                                <asp:LinkButton ID="btnUsu" title="Pesquise os Usuarios" runat="server" CssClass="form-control btn btn-primary" TabIndex="6" Width="10%"  OnClick="btnUsu_Click" AutoPostBack="true"> 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                </asp:LinkButton>
                                                <asp:TextBox ID="txtUsu" CssClass="form-control" name="txtCodPessoa"  runat="server"  Enabled="false"  Width="75%" />
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="margin-top:15px">
                                            <div class="panel panel-default">
                                                <div class="panel-heading" >
                                                            
                                                    <asp:LinkButton runat="server" title="Editar Descrição" ID="BtnEditarDS" TabIndex="7" OnClick="BtnEditarDS_Click" CssClass="btn btn-info" style="height:33px; margin-right:20px;float:left;margin-top:-8px"><span class=" glyphicon glyphicon-pencil"></span></asp:LinkButton>                                  
                                                    Descrição da Solicitação
                                                </div>
                                                <div class="panel-body" >
                                                    <asp:Literal runat="server" ID="litDescricao" ></asp:Literal>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                                                                                
                                </div>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtCodUsu" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>                                 
                </div>
            </div>
        </div>
    </div>
</asp:Content>
