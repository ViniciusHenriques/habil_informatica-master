<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" 
    CodeBehind="ManRasDocumento.aspx.cs" Inherits="SoftHabilInformatica.Pages.Servicos.ManRasDocumento" 
    MaintainScrollPositionOnPostback="true" %>


<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server" >
    <script src="../../Scripts/jquery.canvasjs.min.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    <script src="../../Content/ckeditor/ckeditor.js"></script>
	<script src="../../Content/ckeditor/samples/js/sample.js"></script>
	<link rel="stylesheet" href="../../Content/ckeditor/samples/toolbarconfigurator/lib/codemirror/neo.css">
    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

        });
        
        
    </script>

   
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary ">
            <div class="panel-heading panel-heading-padrao" >
                Rascunho do Documento
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
                                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                </asp:LinkButton>
                            </div>                               
                        </ContentTemplate> 
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSalvar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="row" style="background-color: white; border: none; margin-top:10px!important">                                                       
                    <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                        <label for="usr" style ="margin-top:1px;">Descrição da Solicitação</label>
                        <CKEditor:CKEditorControl runat="server" id="CKEditor1" BasePath="/ckeditor/"></CKEditor:CKEditorControl>
                        <script>
                            CKEDITOR.config.height = 750;
                            CKEDITOR.config.maxCharCount = 10;
                        </script>                                                                         
                    </div>                                                                                                         
                </div>
            </div>
        </div>
    </div>
</asp:Content>
