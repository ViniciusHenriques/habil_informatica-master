<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManSolCompra.aspx.cs" Inherits="SoftHabilInformatica.Pages.Compras.ManSolCompra" 
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server" >
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>
    <script src="../../Scripts/jquery.canvasjs.min.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            $('.js-example-basic-single').select2({});

        });

        function FloatingBox(idmodal, idMostrar)
        {
            $("#" + idmodal + ".modal-title").html("");
            $("#" + idmodal).modal(idMostrar);
        }

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});           
        }

        function CaixaFornecedorHide()
        {
            $("#CaixaFornecedor.modal-title").html("");
            $("#CaixaFornecedor").modal("hide");
        }

        function CaixaProdutoHide()
        {
            $("#CaixaProduto.modal-title").html("");
            $("#CaixaProduto").modal("hide");
        }
    </script>
    <style type="text/css">
        @media screen and (max-width: 800px) {
            .noprint{display:none;}  
            .print{display:block!important}
            .scroll-true{overflow-y:scroll!important;}
        }
        @media screen and (max-width: 1000px) {
            .acao{
                margin-top:20px!important;
                
            }  
            #consulta3,#consulta1,#consulta2{
                padding-left:0px!important;
            }
        }
        .valor{
            border-left:0!important;
        }
         .centerHeaderText{
            text-align: center!important;
         }
         
    </style>
   
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary">
            <div class="panel-heading" >
                Manutenção de Solicitação de Compra                
                <div class="messagealert" id="alert_container"></div>

            </div>
            <div class="panel-body" >
                    <div class="row" style="background-color: white; border: none;">
                       <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">                           
                                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0"  OnClick="btnVoltar_Click" title="Voltar ( Alt + V )"> 
                                        <span aria-hidden="true" title="Voltar ( Alt + V )" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" CssClass=" btn btn-success link-button-glyphicon" ID="btnSalvar" title="Salvar ( Alt + S )">
                                        <span aria-hidden="true" title="Salvar ( Alt + S )" class="glyphicon glyphicon-save" ></span>
                                        <asp:Button runat="server"  OnClientClick="this.disabled = true; this.value = 'Salvando...';" UseSubmitBehavior="false" OnClick="btnSalvar_Click" CssClass=" button-glyphicon" Text="Salvar" ID="btnSalvar2" />
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Cancelar"                  CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" title="Cancelar ( Alt + K )" > 
                                        <span aria-hidden="true" title="Excluir ( Alt + K )" class="glyphicon glyphicon-remove"></span>  Cancelar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnEncCompra" runat="server" Text="Encaminhar para Compra" CssClass="btn btn-warning link-button-glyphicon" data-toggle="modal" data-target="#myexcpes2" title="Excluir ( Alt + E )" > 
                                        <span aria-hidden="true" title="Encaminhar para Compra ( Alt + E )" class="glyphicon glyphicon-check"></span>  Encaminhar para Compra
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSalvar"/>
                                <asp:PostBackTrigger ControlID="btnEncCompra"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none; margin-top:10px!important">
                        <asp:HiddenField ID="TabName" runat="server"/>            
                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;" >
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <ul class="nav nav-tabs" role="tablist" id="myTabs">
                                        <li role="presentation" id="abaTagLi1"><a href="#aba1" aria-controls="#aba1" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                                        <li role="presentation" id="abaTagLi5" ><a href="#aba5" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Eventos</a></li>
                                        <li role="presentation" id="abaTagLi6" ><a href="#aba6" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-check"></span>&nbsp;&nbsp;Logs</a></li>
                                        <li role="presentation" id="abaTagLi7" ><a href="#aba7" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexos</a></li>
                                        <li STYLE="float:right!important;" title="Atalho para trocar de aba"><a class="tab-select">Alt + [→] </a></li>
                                    </ul>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="tab-content" runat="server" id="PanelContext">                                
                                <div role="tabpanel" class="tab-pane" id="aba1" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-2" style="font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Lançamento</label>
                                                    <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="1" MaxLength="18"/>
                                                </div>
                                                <div class="col-md-4" style=" font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Empresa </label>
                                                    <asp:DropDownList ID="ddlEmpresa" runat="server"   TabIndex="2"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_TextChanged"   AutoPostBack="true"/>
                                                </div>
                                                <div class="col-md-2" style=" font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Data Emissão</label>
                                                    <asp:TextBox ID="txtdtemissao" CssClass="form-control"  TabIndex="3" runat="server" name="txtdtemissao"  AutoPostBack="true" Enabled="false" MaxLength="15" />
                                                </div> 
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Data Validade <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:TextBox ID="txtdtValidade" name="txtdtValidade" AutoPostBack="False" CssClass="form-control" TabIndex="4" runat="server" MaxLength="15" enabled="false"/>
                                                </div>              
                                                <div class="col-md-2" style="font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Situação</label>
                                                    <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" TabIndex="5" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>  
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">N° Documento</label>
                                                    <asp:TextBox ID="txtNroDocumento" AutoPostBack="False" CssClass="form-control"  TabIndex="6" runat="server" MaxLength="18" Enabled="false"/>
                                                </div>
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Série</label>
                                                    <asp:TextBox ID="txtNroSerie" AutoPostBack="False" CssClass="form-control"  TabIndex="7" runat="server" MaxLength="18" Enabled="false"/>
                                                </div>
                                                <div class="col-md-2" style=" font-size: x-small;margin-top:3px">
                                                </div> 
                                                <div class="col-md-6" style=" font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Usuário Solicitante</label>
                                                    <asp:DropDownList ID="ddlUsuario" runat="server" Width="100%" TabIndex="8" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>              
                                            </div>
                                            <div class="row">
                                                
                                                <div class="col-md-9" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style="font-size: x-small;">Fornecedor <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <div class="input-group " style="width:100%">                              
                                                        <asp:TextBox ID="txtFornecedor" CssClass="form-control" OnTextChanged="txtFornecedor_TextChanged"  runat="server" TabIndex="9" AutoPostBack="true"  onFocus="this.select()"                              
                                                        Width="15%" MaxLength="6" />                                                      
                                                        <asp:LinkButton ID="btnFornecedor" OnClick="btnFornecedor_Click" title="Pesquise os Fornecedor" runat="server" CssClass="form-control btn btn-primary" Width="10%"  AutoPostBack="true"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                        <asp:TextBox ID="txtNomeFornecedor" CssClass="form-control" runat="server"  Enabled="false"  Width="75%"  />
                                                    </div>
                                                </div> 
                                                <div class="col-md-3" style="font-size: small;padding-top:20px;">
                                                    <div class="input-group" title="Valor Total da Verba">
                                                        <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="border-right:0!important;border:0!important"><b>Total da Verba:</b></span>
                                                        <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="font-weight:bold;padding-right:4px!important;font-size:20px;border:0!important;border-right:0!important;">R$</span>
                                                        <asp:TextBox ID="txtValorTotal"  CssClass="form-control CorPadraoEscolhidaTexto  CorPadraoEscolhida" Text="0,00"  runat="server" MaxLength="50" Enabled="true" AutoPostBack="true" style="padding-left:0!important;font-weight:bold;font-size:20px;border:0!important;" OnTextChanged="txtValorTotal_TextChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style="font-size: x-small;">Motivo da Situação</label>
                                                    <asp:TextBox ID="txtMotivo" CssClass="form-control" runat="server"  Font-Size="Small" TabIndex="10" />
                                                </div> 
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style="font-size: x-small;">Observações</label>
                                                    <asp:TextBox ID="txtObs" CssClass="form-control" runat="server" TextMode="multiline" Columns="10" Rows="2" Font-Size="Small" TabIndex="10" MaxLength="18"/>
                                                </div> 
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12 " style="margin-top:6px">
                                                    <div class="panel panel-primary">
                                                        <div class="panel-heading" style="text-align:center;padding:13px!important;height:45px">
                                                             Produtos da Solicitação de Compra                                                               
                                                        </div>
                                                        <div class="panel-body scroll-true" style="padding-left:0px!important;padding-bottom:0px!important">
                                                            <div class="row" style="margin-left:20px; margin-bottom:15px">                                                                
                                                                <div class="col-md-1 " style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <label for="usr">Produto</label>
                                                                    <asp:TextBox ID="txtProduto" OnTextChanged="txtProduto_TextChanged" CssClass="form-control" name="txtCodItem" TabIndex="11" runat="server"  MaxLength="50" AutoPostBack="true" />
                                                                </div>
                                                                <div class="col-md-1 " style="background-color: white; border: none; font-size: x-small;margin-top:18px;">
                                                                    <asp:LinkButton ID="btnProduto" runat="server" OnClick="btnProduto_Click" title="Pesquisar produtos" TabIndex="12" CssClass="btn btn-primary" Width="100%" style="height:35px!important;padding-top:7px"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                                    </asp:LinkButton>                                                                                                              
                                                                </div>
                                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <label for="usr">Descrição </label>
                                                                    <asp:TextBox ID="txtDescProduto"  CssClass="form-control" name="txtDescricao"  runat="server" MaxLength="50" Enabled="false"  AutoPostBack="true" />
                                                                </div>
                                                                <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <label for="usr">Unidade</label>
                                                                    <asp:DropDownList ID="ddlUnidade" runat="server" Width="100%" CssClass="form-control" Font-Size="Small" Enabled="false"/>
                                                                </div>

                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <label for="usr">Marca</label>
                                                                    <asp:TextBox ID="txtMarca" runat="server" Width="90%" CssClass="form-control" Font-Size="Small" Enabled="false"/>
                                                                </div>
                                                                <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <label for="usr">Qtde. </label>
                                                                    <asp:TextBox ID="txtQtde"  CssClass="form-control" name="txtQtde" TabIndex="13" runat="server" Text="0,00" Width ="120%" MaxLength="18" onFocus="this.select()" OnTextChanged="txtQtde_TextChanged"  AutoPostBack="true"/>
                                                                </div>

                                                                <div class="col-md-9" style="font-size: x-small;margin-top:3px">
                                                                    <label for="usr" style="font-size: x-small;">Especificações Técnicas</label>
                                                                    <asp:TextBox ID="txtOBSItem" CssClass="form-control" runat="server" TextMode="multiline" Columns="10" Rows="3" Font-Size="Small" Width="97%" TabIndex="10"/>
                                                                </div> 

                                                                <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:19px;padding-right:0!important">
                                                                    <asp:Button runat="server" id="BtnAddProduto" OnClick="BtnAddProduto_Click" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false"  Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar/Alterar produto" TabIndex="14"/>
                                                                    <asp:Button runat="server" id="BtnExcluirProduto" OnClick="BtnExcluirProduto_Click" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false"  Text="×" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-danger" ToolTip="Excluir Produto" TabIndex="15"/>
                                                                </div> 
                                                            </div>       
                                            
                                                            <asp:GridView ID="GridItemProdutos" runat="server" TabIndex="16"
                                                                    Width="100%" PagerSettings-Mode ="NumericFirstLast"  
                                                                    CssClass="table table-hover table-striped" AllowSorting="true"  
                                                                    GridLines="None" AutoGenerateColumns="False" AllowPaging="true" PageSize="5"
                                                                    OnSelectedIndexChanged="GridItemProdutos_SelectedIndexChanged"
                                                                    Font-Size="8pt" >
                                                                <Columns>
                                                                    <asp:BoundField DataField="CodigoItem" HeaderText="#"  SortExpression="CodigoItem" />
                                                                    <asp:BoundField DataField="CodigoProduto" HeaderText="Cód.Produto"  SortExpression="CodigoProduto" />
                                                                    <asp:BoundField DataField="DsMarca" HeaderText="Marca"  SortExpression="DsMarca" />
                                                                    <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Descrição do Produto"  SortExpression="Cpl_DscProduto" />
                                                                    <asp:BoundField DataField="Unidade" HeaderText="Unidade"  DataFormatString="{0:F}"  SortExpression="Unidade" />
                                                                    <asp:BoundField DataField="Quantidade" HeaderText="Quantidade"  DataFormatString="{0:F}"  SortExpression="Quantidade" />
                                                                    <asp:CommandField HeaderText="Editar" ShowSelectButton="True" 
                                                                            ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                                            ButtonType="Image"  SelectImageUrl ="~/Images/Editar.png" 
                                                                            ControlStyle-Width ="25px" ControlStyle-Height ="25px" />                                                                     
                                                                </Columns>
                                                                <RowStyle CssClass="cursor-pointer" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>                                
                                <div role="tabpanel" class="tab-pane" id="aba2" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                        <ContentTemplate>

                                        </ContentTemplate> 
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="aba5" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                                <asp:GridView ID="GrdEventoDocumento" runat="server" AutoPostBack="false"
                                                    CssClass="table table-bordered table-hover table-striped"
                                                    GridLines="None" AutoGenerateColumns="False"
                                                    Font-Size="8pt" Visible="true">
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoEvento" HeaderText="Código" />
                                                    <asp:BoundField DataField="DataHoraEvento" HeaderText="Data" />
                                                    <asp:BoundField DataField="Cpl_NomeMaquina" HeaderText="Máquina" />
                                                    <asp:BoundField DataField="Cpl_NomeUsuario" HeaderText="Usuário" />
                                                    <asp:BoundField DataField="Cpl_Situacao" HeaderText="Situação" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>                                                                                                
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="GrdEventoDocumento" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                        </ContentTemplate> 
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="aba6" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdLogDocumento" runat="server" CssClass="table table-bordered table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt" >
                                                <Columns>
                                                    <asp:BoundField DataField="DataHora" HeaderText="Data" />
                                                    <asp:BoundField DataField="EstacaoNome" HeaderText="Máquina" />
                                                    <asp:BoundField DataField="UsuarioNome" HeaderText="Usuário" />
                                                    <asp:BoundField DataField="Cpl_DescricaoOperacao" HeaderText="Operação" />
                                                    <asp:BoundField DataField="CodigoChave" HeaderText="Chave" />
                                                    <asp:BoundField DataField="DescricaoLog" HeaderText="Descrição" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="grdLogDocumento"  />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="aba7" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="btnNovoAnexo" runat="server" style="padding-left:10px; margin-bottom:10px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovoAnexo_Click" TabIndex="17" ToolTip="Add. Novo anexo ( Alt + N )"> 
                                                <span aria-hidden="true" class="glyphicon glyphicon-edit"></span> Novo Anexo
                                            </asp:LinkButton>
                                            <br />
                                            <asp:GridView ID="grdAnexo" runat="server"
                                                CssClass="table  table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  Visible="true"
                                                >
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoAnexo" HeaderText="#"  />
                                                    <asp:BoundField DataField="DataHoraLancamento" HeaderText="Data/Hora Lançamento"  />
                                                    <asp:BoundField DataField="Cpl_Maquina" HeaderText="Estação" />
                                                    <asp:BoundField DataField="Cpl_Usuario" HeaderText="Usuário"  />
                                                    <asp:BoundField DataField="DescricaoArquivo" HeaderText="Descrição"/>
                                                    <asp:BoundField DataField="NomeArquivo" HeaderText="Arquivo"/>
                                                    <asp:BoundField DataField="ExtensaoArquivo" HeaderText="Extensão" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>                                           
                                         </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnNovoAnexo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="grdAnexo" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>         
                                </div> 
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="myexcpes" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" style="height: 100%; width: 500px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H3">Atenção</h4>
                        </div>
                        <div class="modal-body">
                            Deseja Cancelar esta Solicitação de Compra?
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnCfmSim" OnClientClick="this.disabled = true; this.value = 'Excluindo...';" UseSubmitBehavior="false" OnClick="btnCfmSim_Click" CssClass="btn btn-danger" style="color:white" Text="Excluir" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCfmSim"  />
        </Triggers>
    </asp:UpdatePanel> 

    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="myexcpes2" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" style="height: 100%; width: 500px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H3">Atenção</h4>
                        </div>
                        <div class="modal-body">
                            Deseja Encaminhar esta Solicitação para Compra?
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnEncaminhar" OnClientClick="this.disabled = true; this.value = 'Encaminhando...';" UseSubmitBehavior="false" OnClick="btnEncaminhar_Click" CssClass="btn btn-warning" style="color:white" Text="Encaminhar" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEncaminhar"  />
        </Triggers>
    </asp:UpdatePanel> 
    
    <div class="modal fade" id="CaixaProduto"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:300px;width:720px">
            <div class="panel panel-primary">
                <div class="panel-heading" style="height:40px"> 
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="H1"> Consultar Produto </label>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server"  UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="margin-top: 10px">
                                <div class="col-md-11" >
                                    <label for="usr"> Digite a Descrição do Produto:</label>
                                    <asp:TextBox ID="txtPesquisaProduto"  PlaceHolder="No minimo 3 Caracteres ..." OnTextChanged="txtPesquisaProduto_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="18" />
                                </div>
                            </div>
                            <div class="row" style="margin-top: 10px">
                                <div class="col-md-11" >
                                    <asp:label id="Label2" runat="server" for="usr" ForeColor="OrangeRed"> </asp:label>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 10px">
                                <div class="panel-body" style="overflow-x:scroll;height:300px;width:700px">
                                    <asp:GridView ID="GridProdutos" runat="server" Width="100%"
                                        CssClass="table table-bordered table-hover table-striped"
                                        GridLines="None" AutoGenerateColumns="False"
                                        Font-Size="8pt"
                                        OnSelectedIndexChanged="GridProdutos_SelectedIndexChanged"
                                        AllowPaging="true" PageSize="50"
                                        PagerSettings-Mode="NumericFirstLast">
                                            <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                            <Columns>
                                                <asp:BoundField DataField="CodigoProduto" HeaderText="Código" />
                                                <asp:BoundField DataField="CodigoBarras" HeaderText="Cód.Barras"  />
                                                <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição"  />
                                                <asp:BoundField DataField="DsSigla" HeaderText="Und"  />
                                                <asp:BoundField DataField="DsEmbalagem" HeaderText="Complemento"  />
                                                <asp:BoundField DataField="QtEmbalagem" HeaderText="Qtd.Emb"  />
                                                <asp:BoundField DataField="CodigoNCM" HeaderText="NCM"  />

                                                <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                                    ItemStyle-Height="15px" ItemStyle-Width="50px"
                                                    ButtonType="Image" SelectImageUrl="~/Images/Acessar.svg"
                                                    ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                            </Columns>
                                            <RowStyle CssClass="cursor-pointer" />
                                    </asp:GridView>   
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="CaixaFornecedor"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:300px;width:720px">
            <div class="panel panel-primary">
                <div class="panel-heading" style="height:40px"> 
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="H2"> Consultar Fornecedor</label>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server"  UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="margin-top: 10px">
                                <div class="col-md-11" >
                                    <label for="usr"> Digite a Descrição do Fornecedor:</label>
                                    <asp:TextBox ID="txtPesquisaFornecedor"  PlaceHolder="No minimo 3 Caracteres ..." OnTextChanged="txtPesquisaFornecedor_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="19" />
                                </div>
                            </div>
                            <div class="row" style="margin-top: 10px">
                                <div class="col-md-11" >
                                    <asp:label id="Label1" runat="server" for="usr" ForeColor="OrangeRed"> </asp:label>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 10px">
                                <div class="panel-body" style="overflow-x:scroll;height:300px;width:700px">
                 
                                    <asp:GridView ID="GridFornecedor" runat="server" Width="100%"
                                        CssClass="table table-bordered table-hover table-striped"
                                        GridLines="None" AutoGenerateColumns="False"
                                        Font-Size="8pt"
                                        OnSelectedIndexChanged="GridFornecedor_SelectedIndexChanged"
                                        AllowPaging="true" PageSize="50"
                                        PagerSettings-Mode="NumericFirstLast">
                                            <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                            <Columns>
                                                <asp:BoundField DataField="CodigoPessoa" HeaderText="Código" />
                                                <asp:BoundField DataField="NomePessoa" HeaderText="Descrição"  />
                                                <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                                    ItemStyle-Height="15px" ItemStyle-Width="50px"
                                                    ButtonType="Image" SelectImageUrl="~/Images/Acessar.svg"
                                                    ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                            </Columns>
                                            <RowStyle CssClass="cursor-pointer" />
                                    </asp:GridView>   
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
