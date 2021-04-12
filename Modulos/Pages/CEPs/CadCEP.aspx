<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadCEP.aspx.cs" Inherits="SoftHabilInformatica.Pages.CEPs.CadCEP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsCep.js" type="text/javascript"></script>
    <script src="../../Scripts/funcoes.js" type="text/javascript"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
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
        });
    </script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">

        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Cadastro de CEP
                        <div class="messagealert" id="alert_container"></div>
                        <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                            <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                                <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                                <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                            </div> 
                        </asp:Panel>
                    </div>

                    <div class="panel-body">
                        <div class="input-group">
                            <span class="input-group-addon">Código do CEP : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtcodcep"
                                name="txtcodcep"
                                CssClass="form-control"
                                runat="server"
                                TabIndex="1" Width="100" MaxLength="10"
                                AutoPostBack="true"
                                onKeyUp="javascript:mascara_cep(this);"
                                onKeyDown="javascript:checa_value_cep(this);"
                                OnTextChanged="txtcodcep_TextChanged" />

                            <asp:LinkButton ID="btnCorreios" name="btnCorreios" runat="server" Text="Consultar Correios" CssClass="btn btn-primary" UseSubmitBehavior="false" OnClick="btnCorreios_Click"> 
                            <span aria-hidden="true" title="Consultar Correios" class="glyphicon glyphicon-save"></span>  Consultar Correios
                            </asp:LinkButton>
                        </div>
                        <br />

                        <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click" Visible ="false" > 
                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnCancelar" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnCancelar_Click" Visible ="false" > 
                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Cancelar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click" Visible="false"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" UseSubmitBehavior="false" TabIndex="-1" data-toggle="modal" data-target="#myexccep" Visible="false"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                            <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                        </asp:LinkButton>

                        <asp:Panel ID="pnlPanelCEP" runat="server" Enabled="false">
                            <br />
                            <br />
                            <br />
                            <div class="input-group ">
                                <span class="input-group-addon ">Logradouro : &nbsp;&nbsp;&nbsp</span>
                                <asp:TextBox ID="txtLogradouro" CssClass="form-control " name="txtLogradouro" runat="server"
                                    TabIndex="2"  MaxLength="100" />
                            </div>
                            <br />
                            <div class="input-group ">
                                <span class="input-group-addon ">Complemento : </span>
                                <asp:TextBox ID="txtComplemento" CssClass="form-control" name="txtComplemento" runat="server"
                                    TabIndex="3" MaxLength="100" />
                            </div>
                            <br />
                            <div class="col-md-3 col-xs-12" style="padding:0!important;margin-bottom:20px;margin-right:10px"">
                            
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="input-group">
                                            <span class="input-group-addon">Estado: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                            <asp:DropDownList
                                                ID="ddlEstado"
                                                AutoPostBack="true"
                                                TabIndex="4"
                                                runat="server"
                                                CssClass="form-control js-example-basic-single"
                                                OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                            <div class="col-md-4 col-xs-12 " style="padding:0!important;margin-bottom:20px;margin-right:10px"">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="input-group ">
                                            <span class="input-group-addon">Município: </span>
                                            <asp:DropDownList ID="ddlMunicipio" AutoPostBack="true" runat="server" CssClass="form-control js-example-basic-single" TabIndex="5"
                                                OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-3 col-xs-10" style="padding:0!important;margin-bottom:20px;margin-right:10px">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="input-group">
                                            <span class="input-group-addon">Bairro: </span>
                                            <asp:DropDownList ID="ddlBairro" AutoPostBack="true" runat="server" CssClass="form-control js-example-basic-single" TabIndex="6"
                                                OnSelectedIndexChanged="ddlBairro_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-1 col-xs-1" style="margin-top:2px">
                                <asp:LinkButton ID="btnAddBairro" runat="server" Text="Adicionar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnAddBairro_Click"> 
                                <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-plus"></span>
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

   <!-- Exclui CEP -->
    <div class="modal fade" id="myexccep"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir este CEP ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="Button2" runat="server" Text="Confirma" CssClass="btn btn-primary"  AutoPostBack="true"   OnClick="btnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>
</asp:Content>

