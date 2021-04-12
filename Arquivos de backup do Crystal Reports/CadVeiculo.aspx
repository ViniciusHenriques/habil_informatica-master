<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadVeiculo.aspx.cs" Inherits="SoftHabilInformatica.Pages.Veiculos.CadVeiculo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">

        <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;" >

            <div class="panel panel-primary">
                <div class="panel-heading">Cadastro de Veículos
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                            <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                        </div> 
                    </asp:Panel>
                </div>
                <div class="panel-body">
                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>
                    
                    <br />
                    <br />
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Código</label>
                            <asp:TextBox ID="txtCodVeiculo" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Placa</label>
                            <asp:TextBox ID="txtPlaca" runat="server" MaxLength="7" CssClass="form-control"></asp:TextBox>
                        </div>


                        <div class="form-group col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label for="usr">Categoria do Veículo</label>
                                        <div class="form-inline">
                                            <asp:DropDownList ID="ddlCatVeiculo" AutoPostBack="true" runat="server" CssClass="form-control" 
                                                OnSelectedIndexChanged="ddlCatVeiculo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:LinkButton ID="btnAddCatVeiculo" runat="server" Text="Adicionar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnAddCatVeiculo_Click" Height="33"> 
                                            <span aria-hidden="true" title="Adicionar Categorias de Veículo" class="glyphicon glyphicon-plus"></span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="form-group col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small">
                            <label for="exampleInputPassword1">Descrição do Veículo</label>
                            <asp:TextBox ID="txtDscVeiculo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
            </div> 
    </div>

       <!-- Exclui Veiculo -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir este Veículo ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>
