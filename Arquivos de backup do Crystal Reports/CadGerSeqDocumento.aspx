<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadGerSeqDocumento.aspx.cs" Inherits="SoftHabilInformatica.Pages.Empresas.CadGerSeqDocumento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
     <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtValidade']").datepicker({
                todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",
                locate: "pt-BR"
            });

        });

        
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
         $(document).ready(function () {
            $('.js-example-basic-single').select2({
                
            });
        });
        
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <div  class="panel panel-primary" >
            <div class="panel-heading">Cadastro de Geração Sequencial de Documento
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                    <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                    </div> 
                </asp:Panel>

            </div>
            <div class="panel-body">

                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>

                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>
                <asp:LinkButton ID="btnGerarTab" runat="server" Text="Salvar" CssClass="btn btn-primary" UseSubmitBehavior="false" OnClick="btnGerarTab_Click"> 
                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Gerar Tabela
                </asp:LinkButton>

                <div class="row" style="margin-top:30px">
                    <div class="col-md-8">
                        <div class="input-group">
                            <span class="input-group-addon">Código : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtCod" CssClass="form-control" name="txtCodEmpresa" Enabled="false" runat="server" TabIndex="1"/>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="input-group">
                            <span class="input-group-addon">Situação : </span>
                            <asp:DropDownList ID="ddlSituacao" runat="server" CssClass="form-control" />                           
                        </div>
                     </div>
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-addon">Tipo de Documento : </span>
                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-control js-example-basic-single" OnTextChanged="ddlTipoDocumento_TextChanged" AutoPostBack="true"  />                           
                        </div>
                     </div>
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-addon">Empresa : </span>
                            <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-control js-example-basic-single" />                           
                        </div>
                     </div>
                    
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-addon">Série Conteúdo : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtSerieConteudo" CssClass="form-control" runat="server" TabIndex="1"/>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-addon">Série Número : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtSerieNro" CssClass="form-control" runat="server" TabIndex="1"/>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="input-group">
                            <span class="input-group-addon">Descrição : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtDescricao" CssClass="form-control" runat="server" TabIndex="1"/>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-addon">Data Validade : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtValidade" CssClass="form-control" runat="server" TabIndex="1" name="txtValidade"/>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-addon">Número Inicial : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtNroInicial" CssClass="form-control" runat="server" TabIndex="1"/>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="input-group">
                            <span class="input-group-addon">Nome: &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtNome" CssClass="form-control" runat="server" TabIndex="1" Enabled="false"/>
                        </div>
                    </div>
                </div>
                

               

                
            </div>
        </div>                  
    </div>

        <!-- Exclui Empresa -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
            </div>
            <div class="modal-body">
                Deseja Excluir esta Empresa ?
            </div>
            <div class="modal-footer">
            <asp:Button ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
        </div>
    </div>

</asp:Content>
