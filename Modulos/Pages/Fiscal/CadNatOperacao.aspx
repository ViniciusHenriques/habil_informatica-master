<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="CadNatOperacao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Fiscal.CadNatOperacao" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script>
        function pageload(sender, args)  {
            $(".js-example-basic-single").select2();
        }
        
        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <style type="text/css">
       
        .informacao{
            border-top-left-radius:0!important;
            border-top-right-radius:0!important;
        }
        .informacao2{
            border-bottom-left-radius:0!important;
            border-bottom-right-radius:0!important;
        }
    </style>
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">

        <div class="panel panel-primary">
            <div class="panel-heading">
                Cadastro de Naturezas de Operação
                    <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
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

                <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                </asp:LinkButton>

                <br />
                <br />

                <div class="container-fluid" style="background-color: white; border: none;">
                    <div class="row" style="background-color: white; border: none;">
                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">CFOP : &nbsp;&nbsp;&nbsp; </span>
                                <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" Enabled="false" runat="server" TabIndex="0"
                                    onkeypress="return PermiteSomenteNumeros(event);" MaxLength="6" Text="" AutoPostBack="true" OnTextChanged="txtCodigo_TextChanged" />
                            </div>
                        </div>

                        <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                        </div>

                        <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                        </div>

                    </div>

                    <br />

                    <div class="row" style="background-color: white; border: none;">
                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Descrição : </span>
                                <asp:TextBox ID="txtDescricao" CssClass="form-control" name="txtDescricao" TabIndex="1" runat="server" MaxLength="1000" />
                            </div>
                        </div>
                    </div>
                    <div class="row" style="background-color: white; border: none;">

                        <br />

                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                            <div class="input-group" >
                                <span class="input-group-addon informacao2">CFOP Contra: &nbsp;</span>
                                <asp:DropDownList ID="ddlCodCFOPContra" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single informacao2" 
                                    Font-Size="Small" TabIndex="2" OnSelectedIndexChanged="ddlCodCFOPContra_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <asp:Label ID="lblCodCFOPContra" runat="server" CssClass="form-control informacao" ForeColor="Black" BackColor="#FFD700" Height="50px"></asp:Label>
                        </div>

                    </div>
                    <div class="row" style="background-color: white; border: none;height:105%;">

                        <br />

                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                            <div class="input-group ">
                                <span class="input-group-addon  informacao2"  >CFOP ST: &nbsp;</span>
                                <asp:DropDownList ID="ddlCodCFOPST" runat="server" AutoPostBack="true"
                                    CssClass="form-control js-example-basic-single informacao2" Font-Size="Small" TabIndex="3"
                                    OnSelectedIndexChanged="ddlCodCFOPST_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <asp:Label ID="lblCodCFOPST" runat="server" CssClass="form-control informacao" ForeColor="Black" BackColor="#FFD700" Height="50px"></asp:Label>

                        </div>
                        <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                        </div>
                    </div>
                </div>
                <br />
                <div>
                </div>
            </div>
        </div>
    </div>

       <!-- Exclui Grupo -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir esta Natureza da Operação ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>
</asp:Content>
