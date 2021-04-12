<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ManMovAcesso.aspx.cs" Inherits="SoftHabilInformatica.Pages.Acesso.ManMovAcesso" MaintainScrollPositionOnPostback="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script> 
    $(document).ready(function() {
        $('.js-example-basic-single').select2();
    });

     </script>



    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;" >

            <div class="panel panel-primary">
                <div class="panel-heading">Movimentação de Acessos
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                            <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                        </div> 
                    </asp:Panel>
                </div>
                <div class="panel-body">

                    <asp:LinkButton ID="BtnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="BtnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                    </asp:LinkButton>

                    <asp:LinkButton ID="BtnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="BtnSalvar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                    </asp:LinkButton>

                    <asp:LinkButton ID="BtnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>

                    <asp:LinkButton ID="BtnSaida" runat="server" Text="Gerar Saída" CssClass="btn btn-warning" data-toggle="modal" data-target="#mysaipes" > 
                        <span aria-hidden="true" title="Saida" class="glyphicon glyphicon-ok"></span>  Gerar Saída
                    </asp:LinkButton>

                    <br/>
                    <br/>

                    <div class="container-fluid" style="background-color:white;border:none;"> <%--ListraCinza--%>
                        <div class="row" style="background-color:white;border:none;">
                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Código</label>
                                <asp:TextBox ID="txtCodMovAcesso" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Data de Entrada</label>
                                <asp:TextBox ID="txtDataEntrada" runat="server" Enabled ="false" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Data de Saída</label>
                                <asp:TextBox ID="txtDataSaida" runat="server" Enabled ="false" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            </div>
                            
                            <div class="form-group col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <div class="form-group">
                                    <label for="usr">Tipo de Acesso</label>
                                    <div class="form-inline">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" >
                                            <ContentTemplate>
                                                <asp:DropDownList ID="DdlTipoAcesso" runat="server"  AutoPostBack="True" Width ="700"  CssClass="form-control js-example-basic-single" 
                                                    OnSelectedIndexChanged="DdlTipoAcesso_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:LinkButton ID="BtnAddTipoAcesso" runat="server" Text="Adicionar" CssClass="btn btn-success" OnClick="BtnAddTipoAcesso_Click"> 
                                                <span aria-hidden="true" title="Adicionar Tipos de Acesso" class="glyphicon glyphicon-plus"></span>
                                                </asp:LinkButton>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="DdlTipoAcesso" />
                                                <asp:PostBackTrigger ControlID="BtnAddTipoAcesso" />
                                            </Triggers>

                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Documento</label>
                                <asp:TextBox ID="txtDocumento" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                            </div>


                            <div class="form-group col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <div class="form-group">
                                    <label for="usr">Pessoa</label>
                                    <div class="form-inline">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                            <ContentTemplate>
                                                <asp:DropDownList ID="DdlPessoa" AutoPostBack="True" runat="server" Width ="700" CssClass="form-control js-example-basic-single" 
                                                    OnSelectedIndexChanged="DdlPessoa_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:LinkButton ID="BtnAddPessoa" runat="server" Text="Adicionar" CssClass="btn btn-success"  OnClick="BtnAddPessoa_Click"> 
                                                <span aria-hidden="true" title="Adicionar Pessoas" class="glyphicon glyphicon-plus"></span>
                                                </asp:LinkButton>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="DdlPessoa" />
                                                <asp:PostBackTrigger ControlID="BtnAddPessoa" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">CPF/CNPJ</label>
                                <asp:TextBox ID="txtCNPJCPF" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <div class="form-group">
                                    <label for="usr">Contato</label>
                                    <div class="form-inline">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="DdlContato" runat="server" AutoPostBack="True" Width ="700"  CssClass="form-control js-example-basic-single" 
                                                    OnSelectedIndexChanged="DdlContato_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:LinkButton ID="BtnAddContato" runat="server" Text="Adicionar" CssClass="btn btn-success" OnClick="BtnAddContato_Click"> 
                                                <span aria-hidden="true" title="Adicionar Contatos da Pessoa" class="glyphicon glyphicon-plus"></span>
                                                </asp:LinkButton>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="DdlContato" />
                                                <asp:PostBackTrigger ControlID="BtnAddContato" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                            </div>
                            <div class="form-group col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <div class="form-group">
                                    <div class="form-inline">
                                        <asp:Image ID="imaFoto" ImageUrl=""  runat="server" BorderStyle ="Dashed"  BorderWidth ="2" ImageAlign ="Right" Width="350" />
                                    </div>
                                </div>

                            </div>
     
                            <div class="form-group col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <div class="form-group">
                                    <label for="usr">Veículo</label>
                                    <div class="form-inline">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="DdlVeiculo" runat="server" AutoPostBack="True" Width ="700"  CssClass="form-control js-example-basic-single" 
                                                    OnSelectedIndexChanged="DdlVeiculo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:LinkButton ID="BtnAddVeiculo" runat="server" Text="Adicionar" CssClass="btn btn-success" OnClick="BtnAddVeiculo_Click"> 
                                                <span aria-hidden="true" title="Adicionar Contatos da Pessoa" class="glyphicon glyphicon-plus"></span>
                                                </asp:LinkButton>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="DdlVeiculo" />
                                                <asp:PostBackTrigger ControlID="BtnAddVeiculo" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                            </div>

                            <div class="form-group col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Observações</label>
                                <asp:TextBox ID="txtOBS" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>


                        </div>
                    </div>
                </div>
            </div> 
    </div>

     <!-- Exclui MovAcesso -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir esta Movimentação de Acesso ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="BtnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="false" OnClick="BtnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Promover Saída -->
    <div class="modal fade" id="mysaipes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:400px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Gerar Saída desta Movimentação de Acesso ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="BtnCfmSimSaida" runat="server" Text="Gerar Movimentação" CssClass="btn btn-warning" TabIndex="-1" AutoPostBack="false" OnClick="BtnCfmSimSaida_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>
</asp:Content>
