<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="CadTipoServico.aspx.cs" Inherits="SoftHabilInformatica.Pages.Servicos.CadTipoServico" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script>
        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $(document).ready(function () {
                $('.js-example-basic-single').select2();
            });

        }
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            
            <div  class="panel panel-primary  " >
                <div class="panel-heading  panel-heading-padrao ">Cadastro de Tipo de serviço
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

                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>

                   <br/><br/>

                    
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Código : &nbsp;&nbsp;&nbsp; </span>
                                <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" Enabled="false" runat="server" TabIndex="1" 
                                onkeypress="return PermiteSomenteNumeros(event);" MaxLength="2"  />
                            </div>
                        </div>

                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                        </div>

                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Situação : &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                <asp:DropDownList ID="txtCodSituacao" runat="server" AutoPostBack="True"  CssClass="form-control"  Font-Size ="Small"  TabIndex="2" >
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>
                       
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Descrição : </span>
                                <asp:TextBox ID="txtDescricao"  CssClass="form-control" name="txtDescricao" TabIndex="2" runat="server" MaxLength="50" />
                            </div>
                        </div>
                    </div>
                    <div class="row" style="background-color:white;border:none;">

                         <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">CNAE: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    <asp:TextBox ID="txtCodigoCNAE"  CssClass="form-control" name="txtCodigoCNAE" TabIndex="2" runat="server" MaxLength="18" />
                            </div>
                        </div>
                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Código Lei Complementar do serviço: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    <asp:TextBox ID="txtCodigoLei"  CssClass="form-control" name="txtCodigoLei" TabIndex="2" runat="server" MaxLength="18" />
                            </div>
                        </div>

                    </div>
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">% ISSQN: &nbsp;</span>
                                    <asp:TextBox ID="TxtVlrISSQN"  CssClass="form-control" name="txtVlrISSQN" TabIndex="2" runat="server" MaxLength="50" />
                            </div>
                        </div>
                       
                        
                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                        </div>
                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Valor Total: &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    <asp:TextBox ID="txtVlrTotal"  CssClass="form-control" name="txtVlrTotal"  TabIndex="2" runat="server" MaxLength="50" Text=""/>

                            </div>
                        </div>

                    </div>
                    
                    <br/>
                    <div>
                    <asp:HiddenField ID="TabName" runat="server"/>
                
                    <div id="Tabs" role="tabpanel">

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist" id="myTabs">
                            <li role="presentation" class="active"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" >Itens</a></li>
                        </ul>
                        <!-- Tab panes -->                                                                                                 
                            <div class="tab-pane fade in active" runat="server" id="PanelContext">
                                <div role="tabpanel" class="tab-pane" id="home"  >
                                    
                                    <div class="container-fluid">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                        <div class="row" style="background-color:white;border:none;">

                                            <br/>
                                            <div class="col-md-7" style="background-color:white;border:none;text-align:left;font-size:x-small;">

                                                 <label for="usr">Descrição: </label>
                                                  <asp:DropDownList ID="ddlProduto" runat="server" TabIndex="2" CssClass="form-control js-example-basic-single" Width="100%" ></asp:DropDownList>
                                                
                                            </div>
                                        
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">

                                                 <label for="usr">Quantidade: </label>
                                                  <asp:TextBox ID="txtQtde"  CssClass="form-control" name="txtQtde" TabIndex="2" runat="server" MaxLength="50"   />

                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">

                                                 <label for="usr">Preço: </label>
                                                  <asp:TextBox ID="txtPreco"  CssClass="form-control" name="txtPreco" TabIndex="2" runat="server" MaxLength="50" />

                                            </div>
                                            <br />
                                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <asp:LinkButton ID="BtnAddProduto" runat="server" Text="Adicionar" TabIndex="10" CssClass="btn btn-success" OnClick="BtnAddProduto_Click" style="height:33px!important;padding-top:7px" > 
                                                <span aria-hidden="true" title="Adicionar Produto" class="glyphicon glyphicon-plus"></span>
                                                </asp:LinkButton>
                                            </div>
                                       

                                     
                                        </div>
                                        <div style="margin-top:10px">
                                            <asp:GridView ID="grdItens" runat="server" AutoPostBack="false"
                                                CssClass="table table-bordered table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt" 
                                                OnSelectedIndexChanged="grdItens_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoItemTipoServico" HeaderText="Item" />
                                                    <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Descrição"  />
                                                    <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" DataFormatString="{0:F}" />
                                                    <asp:BoundField DataField="PrecoItem" HeaderText="Preço" DataFormatString="{0:C}" />
                                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                                                                    ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                                                    ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                    ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="ddlProduto" />
                                                <asp:PostBackTrigger ControlID="BtnAddProduto" />
                                                <asp:PostBackTrigger ControlID="grdItens" />
                                            </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>                               
                            </div>
                            <div role="tabpanel" class="tab-pane" id="indications">

                            </div>
                        </div>
                    </div>
                </div>
            </div>           
            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnSalvar" />
                <asp:PostBackTrigger ControlID="btnExcluir" />
            </Triggers>

        </asp:UpdatePanel>
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
              Deseja Excluir este Tipo Serviço ?
          </div>
          <div class="modal-footer">
            <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:LinkButton>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>
</asp:Content>
