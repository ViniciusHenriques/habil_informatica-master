<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadProduto.aspx.cs" Inherits="SoftHabilInformatica.Pages.Produtos.CadProduto" 
    MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
        <script>
        $(document).ready(function (e) {
            $('.js-example-basic-single').select2({
                    
            });
            
            item = '#<%=PanelSelect%>';
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));


        });
        
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {

            
            $('.js-example-basic-single').select2({
                    
            });
        }
        

    </script>
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;" >

            <div class="panel panel-primary">
                <div class="panel-heading">Cadastro de Produtos
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
                    

                    <div class="row" style="background-color:white;border:none;">
                        <br/>
                        <div class="col-md-2 col-xs-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Código Produto</label>
                            <asp:TextBox ID="txtCodProduto" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-xs-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Cód. Sistema Anterior</label>
                            <asp:TextBox ID="txtCodSisAnterior" runat="server" CssClass="form-control"  Font-Size ="Small"  MaxLength="40"></asp:TextBox>
                        </div>
                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr">Dt. Cadastro</label>
                            <asp:TextBox ID="txtDataCadastro" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr">Dt. Atualização</label>
                            <asp:TextBox ID="txtDataAtualizacao" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr">Situação</label>
                            <asp:DropDownList ID="txtCodSituacao" runat="server" CssClass="form-control" >
                                <asp:ListItem Text="ATIVO" Value="1" />
                                <asp:ListItem Text="INATIVO" Value="2" />
                            </asp:DropDownList>
                        </div>
                <%-----------------------------------------------------------------------------------------------------------------%>

                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                                                
                                <div class="col-md-2" style="background-color:white;border:none;font-size:x-small; text-align=left">

                                    <label for="usr">Tipo de Produto: </label>
                                    <asp:DropDownList ID="ddlGpoPessoa" runat="server" TabIndex="2" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlGpoPessoa_SelectedIndexChanged" >
                                            

                                    </asp:DropDownList>
                                                
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlGpoPessoa" EventName ="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>


                        <div class="form-group col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:10px">
                            <label for="exampleInputPassword1">Descrição do Produto</label>
                            <asp:TextBox ID="txtDscProduto" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                

                    <asp:HiddenField ID="TabName" runat="server"/>
                
                    <div id="Tabs" role="tabpanel" >

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                            <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                            <li role="presentation"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-euro"></span>&nbsp;&nbsp;Fiscal</a></li>
                            <li role="presentation"><a href="#contact" aria-controls="contact" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Indicadores</a></li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content" runat="server" id="PanelContext" style="margin-bottom:150px!important">
                            <div role="tabpanel" class="tab-pane" id="home" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;margin-bottom:100px!important"  >
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" style="padding:0!important">
                                        <ContentTemplate>
                                            <div class="form-inline">
                                                <label for="usr">Unidade</label>
                                                <div class="form-inline">
                                                    <asp:DropDownList ID="ddlUnidade" AutoPostBack="true" runat="server" CssClass="form-control" 
                                                        OnSelectedIndexChanged="ddlUnidade_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;
                                                    <asp:LinkButton ID="btnAddUnidade" runat="server" Text="Adicionar" CssClass="btn btn-success" width="10%" UseSubmitBehavior="false" OnClick="btnAddUnidade_Click" style="height:33px;margin-top:0!important"> 
                                                    <span aria-hidden="true" title="Adicionar Unidades" class="glyphicon glyphicon-plus"></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                        
                                <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px; ">Categoria</label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" style="padding:0!important">
                                            <ContentTemplate>
                                                <div class="row" >
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <asp:TextBox ID="txtCodCategoria" name ="txtCodCategoria" CssClass="form-control" runat="server"
                                                            OnTextChanged="txtCodCategoria_TextChanged" AutoPostBack="true" MaxLength="20" Font-Size ="Medium" 
                                                            Font-Bold ="true"  ForeColor="Blue"   ClientIDMode="Static" />
                                                        </div>
                                                    <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <asp:LinkButton ID="btnConCategoria" runat="server" CssClass="form-control btn btn-primary"  TabIndex="-1" OnClick="btnConCategoria_Click"> 
                                                            <span aria-hidden="true" title="Categoria" class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-7" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <asp:TextBox ID="txtDcrCategoria" runat="server" CssClass="form-control" Enabled="false" />
                                                    </div>
                                                </div>                                                    
                                            </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCodCategoria" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="btnConCategoria" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:10px">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="form-group">
                                                <label for="usr">Valor de Compra</label>
                                                <asp:TextBox ID="txtCompra" runat="server" CssClass="form-control" 
                                                    AutoPostBack="true" OnTextChanged="txtCompra_TextChanged"></asp:TextBox>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCompra" EventName ="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                        
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:10px">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="form-group">
                                                <label for="usr">% Lucro</label>
                                                <asp:TextBox ID="txtLucro" runat="server" CssClass="form-control" AutoPostBack ="true" OnTextChanged="txtLucro_TextChanged"></asp:TextBox>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtLucro" EventName ="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>                                       
                                </div>
                                        
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:10px">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="form-group">
                                                <label for="usr">Valor de Venda</label>
                                                <asp:TextBox ID="txt_Venda" runat="server" CssClass="form-control" AutoPostBack="true"  OnTextChanged="txt_Venda_TextChanged"></asp:TextBox>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txt_Venda" EventName ="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                        
                                </div>
                                <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr">Marca: </label>
                                    <asp:DropDownList ID="ddlMarca" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                </div>
                                <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr">Fabricante: </label>
                                    <asp:DropDownList ID="ddlFabricante" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                </div>
                            </div>

                            <div role="tabpanel" class="tab-pane" id="profile" >
                                <div class="row" style="background-color:white;border:none;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="col-md-12 " style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                <label for="usr">Grupo de Tributação : </label>
                                                <asp:DropDownList ID="ddlGpoTribProduto" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-12 " style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:5px;">
                                                <label for="usr">CEST: </label>
                                                <asp:DropDownList ID="ddlCodigoCEST" runat="server" AutoPostBack="true" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%" OnSelectedIndexChanged="ddlCodigoCEST_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-12 TextBoxFormatado" >
                                                <label for="usr" style="margin-top: 1px;">Descrição CEST</label>
                                                <asp:TextBox ID="txtDescricaoCEST" CssClass="form-control" runat="server"  TextMode="multiline" Columns="10" Rows="5" MaxLength="255" Enabled="false" />        
                                            </div>
                                            </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCodigoCEST" EventName ="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtDescricaoCEST" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
 
                            <div role="tabpanel" class="tab-pane" id="contact" >
                                <br/>
                                &emsp;
                            </div>
                        </div>
                    </div>
                </div>
            </div> 
        </div>



       <!-- Exclui Produto -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir este Produto ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>
