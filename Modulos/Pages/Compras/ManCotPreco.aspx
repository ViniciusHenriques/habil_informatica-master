<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManCotPreco.aspx.cs" Inherits="SoftHabilInformatica.Pages.Compras.ManCotPreco" 
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

    <%--
        --%>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtdtvalidade']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
        });
    </script>


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

        function CaixaSolicitacaoHide()
        {
            $("#CaixaSolicitacao.modal-title").html("");
            $("#CaixaSolicitacao").modal("hide");
        }

        $(document).on('keydown', function (e) {
            if ((e.altKey) && (e.which === 83)) { 
                $get('<%=btnSalvar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 82)) { 
                $get('<%=btnEncerrar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 86)) { 
                $get('<%=btnVoltar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 75)) { 
                $get('<%=btnExcluir.ClientID%>').click(); 
            }
        });
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
                Manutenção de Cotação de Preços                
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

                                    <asp:LinkButton ID="btnEncerrar" runat="server" Text="Encerrar" CssClass="btn btn-success link-button-glyphicon" data-toggle="modal" data-target="#myexcpes2" title="Encerrar ( Alt + R )" > 
                                        <span aria-hidden="true" title="Encerrar ( Alt + E )" class="glyphicon glyphicon-check"></span>  Encerrar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnEnviarEmail" runat="server" Text="Encerrar" CssClass="btn btn-warning link-button-glyphicon" data-toggle="modal" data-target="#myexcpes3" title="Enviar Email" > 
                                        <span aria-hidden="true" title="Enviar Email" class="glyphicon glyphicon-check"></span>  Enviar Email
                                    </asp:LinkButton>


                                </div>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSalvar"/>
                                <asp:PostBackTrigger ControlID="btnEncerrar"/>
                                <asp:PostBackTrigger ControlID="btnEnviarEmail"/>
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
                                        <li role="presentation" id="abaTagLi2" ><a href="#aba2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Fornecedores Participantes</a></li>
                                        <li role="presentation" id="abaTagLi8" ><a href="#aba8" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Cotações de Produtos</a></li>
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
                                                    <asp:TextBox ID="txtCodigo" CssClass="form-control" style="text-align:right;" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="1" MaxLength="18"/>
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
                                                    <asp:TextBox ID="txtdtvalidade" name="txtdtvalidade" AutoPostBack="false" CssClass="form-control" TabIndex="4" runat="server" MaxLength="15" Enabled="true"/>
                                                </div>              
                                                <div class="col-md-2" style="font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Situação</label>
                                                    <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" TabIndex="5" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>  
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">N° Documento</label>
                                                    <asp:TextBox ID="txtNroDocumento" style="text-align:right;" AutoPostBack="False" CssClass="form-control"  TabIndex="6" runat="server" MaxLength="18" Enabled="false"/>
                                                </div>
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Série</label>
                                                    <asp:TextBox ID="txtNroSerie" AutoPostBack="False" CssClass="form-control"  TabIndex="7" runat="server" MaxLength="18" Enabled="false"/>
                                                </div>
                                                <div class="col-md-2" style=" font-size: x-small;margin-top:3px">
                                                    <label for="usr" style="font-size:x-small; background-color: white; border: none; text-align: left; margin-top:3px" >
                                                        Nr.Solicitação: &emsp;</label><asp:Label runat="server" ID="lblNrSolicitacao" Text="" Font-Bold ="true" ></asp:Label>
                                                    <div class="input-group " style="width:100%">
                                                        <asp:TextBox ID="txtNrSolicitacao" CssClass="form-control"  runat="server" TabIndex="7" OnTextChanged="txtNrSolicitacao_TextChanged" AutoPostBack="true"                                
                                                        Width="80%" MaxLength="6" />                                                      
                                                        <asp:LinkButton ID="btnPesSolicitacao" BorderStyle="Inset" runat="server" CssClass="form-control btn btn-primary" Width="20%" TabIndex="8" OnClick="btnPesSolicitacao_Click" AutoPostBack="true"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div> 
                                                <div class="col-md-6" style=" font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Comprador</label>
                                                    <asp:DropDownList ID="ddlUsuario" runat="server" Width="100%" TabIndex="8" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>              
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style="font-size: x-small;">Observações</label>
                                                    <asp:TextBox ID="txtObs" CssClass="form-control" runat="server" TextMode="multiline" Columns="10" Rows="3" Font-Size="Small" TabIndex="10" MaxLength="18"/>
                                                </div> 
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12 " style="margin-top:6px">
                                                    <div class="panel panel-primary">
                                                        <div class="panel-heading" style="text-align:center;padding:13px!important;height:45px">
                                                             Produto(s) da Cotação de Preço
                                                        </div>
                                                        <div class="panel-body scroll-true" style="padding-left:0px!important;padding-bottom:0px!important">
                                                            <div class="row" style="margin-left:20px; margin-bottom:15px">                                                                
                                                                <div class="col-md-1 " style="background-color:white;border:none;text-align:right;font-size:x-small;">
                                                                    <label for="usr">Produto</label>
                                                                    <asp:TextBox ID="txtProduto" style="text-align:right;" OnTextChanged="txtProduto_TextChanged" CssClass="form-control" name="txtCodItem" TabIndex="11" runat="server"  MaxLength="50" AutoPostBack="true" />
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
                                                                    <asp:TextBox ID="txtMarca" runat="server" Width="100%" CssClass="form-control" Font-Size="Small" Enabled="false"/>
                                                                </div>
                                                                
                                                                <div class="col-md-1" style="background-color:white;border:none;text-align:right;font-size:x-small;">
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
                                                                    Width="100%" 
                                                                    CssClass="table table-hover table-striped" AllowSorting="true"  
                                                                    GridLines="None" AutoGenerateColumns="False" AllowPaging="true" 
                                                                    OnSelectedIndexChanged="GridItemProdutos_SelectedIndexChanged"
                                                                    Font-Size="8pt" >
                                                                <Columns>
                                                                    <asp:BoundField DataField="CodigoItem" HeaderText="#"  SortExpression="CodigoItem" />
                                                                    <asp:BoundField DataField="CodigoProduto" HeaderText="Código do Produto"  SortExpression="CodigoProduto" />
                                                                    <asp:BoundField DataField="DsMarca" HeaderText="Marca"  SortExpression="DsMarca" />
                                                                    <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Descrição"  SortExpression="Cpl_DscProduto" />
                                                                    <asp:BoundField DataField="Unidade" HeaderText="Unidade"  SortExpression="Unidade" />
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

                                            <div class="row">
                                                
                                                <div class="col-md-9" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style="font-size: x-small;">Fornecedor <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <div class="input-group " style="width:100%;text-align:right;">                              
                                                        <asp:TextBox ID="txtFornecedor" style="text-align:right;" CssClass="form-control" OnTextChanged="txtFornecedor_TextChanged"  runat="server" TabIndex="9" AutoPostBack="true"  onFocus="this.select()"                              
                                                        Width="15%" MaxLength="6" />                                                      
                                                        <asp:LinkButton ID="btnFornecedor" OnClick="btnFornecedor_Click" title="Pesquise os Fornecedor" runat="server" CssClass="form-control btn btn-primary" Width="10%"  AutoPostBack="true"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                        <asp:TextBox ID="txtNomeFornecedor" CssClass="form-control" runat="server"  Enabled="false"  Width="75%"  />
                                                    </div>
                                                </div> 
                                                <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                    <div class="input-group " style="width:100%">                              
                                                        <label for="usr" style="font-size: x-small;">Fone</span></label>
                                                        <asp:TextBox ID="txtFone" CssClass="form-control" runat="server"  Enabled="false"    />
                                                    </div>
                                                </div> 


                                                <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                    <div class="input-group " style="width:100%">                              
                                                        <label for="usr" style="font-size: x-small;">Estado</span></label>
                                                        <asp:TextBox ID="txtEstado" CssClass="form-control" runat="server"  Enabled="false"    />
                                                    </div>
                                                </div> 
                                                <div class="col-md-4" style="font-size: x-small;margin-top:3px">
                                                    <div class="input-group " style="width:100%">                              
                                                        <label for="usr" style="font-size: x-small;">Município</span></label>
                                                        <asp:TextBox ID="txtCidade" CssClass="form-control" runat="server"  Enabled="false"    />
                                                    </div>
                                                </div> 
                                                <div class="col-md-6" style="font-size: x-small;margin-top:3px">
                                                    <div class="input-group " style="width:100%">                              
                                                        <label for="usr" style="font-size: x-small;">E-Mail</span></label>
                                                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"  Enabled="false"    />
                                                    </div>
                                                </div> 
                                                <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:19px;padding-right:0!important">
                                                    <asp:Button runat="server" id="btnAddForn" OnClick="btnAddForn_Click"  OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false"  Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar/Alterar Fornecedor" TabIndex="14"/>
                                                    <asp:Button runat="server" id="btnExcForn" OnClick ="btnExcForn_Click"  OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false"  Text="×" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-danger" ToolTip="Excluir Fornecedor" TabIndex="15"/>
                                                </div>


                                                <asp:GridView ID="grdFornParticipantes" runat="server" Width="100%"
                                                    CssClass="table table-bordered table-hover table-striped"
                                                    GridLines="None" AutoGenerateColumns="False"
                                                    Font-Size="8pt"
                                                    OnSelectedIndexChanged="grdFornParticipantes_SelectedIndexChanged"
                                                    AllowPaging="true" PageSize="50"
                                                    PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoPessoa" HeaderText="Código" />
                                                            <asp:BoundField DataField="NomePessoa" HeaderText="Descrição"  />
                                                            <asp:BoundField DataField="Cpl_Fone" HeaderText="Fone" />
                                                            <asp:BoundField DataField="Cpl_Estado" HeaderText="Estado" />
                                                            <asp:BoundField DataField="Cpl_Municipio" HeaderText="Município"  />
                                                            <asp:BoundField DataField="Cpl_Email" HeaderText="E-Mail"  />
                                                            <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                                                ItemStyle-Height="15px" ItemStyle-Width="50px"
                                                                ButtonType="Image" SelectImageUrl="~/Images/Acessar.svg"
                                                                ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                                        </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                </asp:GridView>   

                                            </div>


                                        </ContentTemplate> 
                                    </asp:UpdatePanel>
                                </div>

                                <div role="tabpanel" class="tab-pane" id="aba8" style="padding-left: 1px; padding-top: 1px; padding-right: 1px; font-size: small;">

                                    <div class="row">

                                        <div class="col-md-12 " style="margin-top:1px">

                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
                                            <ContentTemplate>

                                                <div class="row" style="margin-left:20px; margin-bottom:15px;text-align:right">                                                                
                                                    <div class="col-md-1 " style="background-color:white;border:none;text-align:right;font-size:x-small;">
                                                        <label for="usr">Fornecedor</label>
                                                        <asp:TextBox ID="txtFornec" style="text-align:right;" OnTextChanged="txtProduto_TextChanged" CssClass="form-control" name="txtFornec" TabIndex="11" runat="server"  MaxLength="50" Enabled="false" AutoPostBack="true" />
                                                    </div>
                                                    <div class="col-md-1 " style="background-color:white;border:none;text-align:right;font-size:x-small;">
                                                        <label for="usr">Produto</label>
                                                        <asp:TextBox ID="txtPrdFornec" style="text-align:right;" OnTextChanged="txtProduto_TextChanged" CssClass="form-control" name="txtPrdFornec" TabIndex="11" runat="server"  MaxLength="50" Enabled="false" AutoPostBack="true" />
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr">Cod.Prd.Fornecedor</label>
                                                        <asp:TextBox ID="txtCodPrdFornecedor"  CssClass="form-control" name="txtCodPrdFornecedor"  runat="server" MaxLength="50" AutoPostBack="true" />
                                                    </div>
                                                    <div class="col-md-5" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr">Descrição </label>
                                                        <asp:TextBox ID="txtNomePrdFornec"  CssClass="form-control" name="txtDescricao"  runat="server" MaxLength="50" Enabled="false"  AutoPostBack="true" />
                                                    </div>

                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:small;padding-top:20px;">
                                                        <asp:CheckBox ID="chkBoxNaoAtende" Text ="&emsp;Não Trabalha com Produto" name="chkBoxNaoAtende" TabIndex="13" runat="server"  onFocus="this.select()" AutoPostBack="true"/>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left:20px; margin-bottom:15px">
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:right;font-size:x-small;">
                                                        <label for="usr">Preço Fornecedor </label>
                                                        <asp:TextBox ID="txtPrcCompra" style="text-align:right;"  CssClass="form-control" name="txtPrcCompra" TabIndex="13" runat="server" Text="0,00" MaxLength="18" onFocus="this.select()" OnTextChanged="txtQtde_TextChanged"  AutoPostBack="true"/>
                                                    </div>
                                                                
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr">(Data / Prazo em Dias) Entrega </label>
                                                        <asp:TextBox ID="txtFornDtDiaEntrega" PlaceHolder="01/01/1950 ou 10/15/20/30 DIAS"  CssClass="form-control" name="txtDescricao"  runat="server" AutoPostBack="true" />
                                                    </div>

                                                    <div class="col-md-5" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr">Obs. Financeira</label>
                                                        <asp:TextBox ID="txtFornObsFinanceira" PlaceHolder="DINHEIRO, DEP.CONTA=BANCO OU BOLETO=30/60/90"   CssClass="form-control" name="txtDescricao"  runat="server" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left:20px; margin-bottom:15px">

                                                    <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr">Obs. Impostos</label>
                                                        <asp:TextBox ID="txtFornObsImposto"
                                                            PlaceHolder="ICMS.: .... - IPI.: .... - PIS.: .... - COFINS.: .... - ST.: .... - MVA.: .... - DIFAL.: ...."
                                                            Text="ICMS.: .... - IPI.: .... - PIS.: .... - COFINS.: .... - ST.: .... - MVA.: .... - DIFAL.: ...."
                                                            CssClass="form-control" name="txtDescricao"  runat="server" AutoPostBack="true" />
                                                    </div>

                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr">Dt. Resposta</label>
                                                        <asp:TextBox ID="txtFornDtResposta"  CssClass="form-control" name="txtDescricao" Text="Sem Resposta..."  runat="server" MaxLength="50" Enabled="false"  AutoPostBack="true" />
                                                    </div>

                                                    <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:19px;padding-right:0!important">
                                                        <asp:Button runat="server" id="BtnAddItemCotPreco" OnClick="BtnAddItemCotPreco_Click" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false"  Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar/Alterar produto" TabIndex="14"/>
                                                    </div>
                                                </div>       
                                            

                                                <asp:GridView ID="grdPrdCotacao" runat="server" TabIndex="16"
                                                        Width="100%"  
                                                        OnSelectedIndexChanged  ="grdPrdCotacao_SelectedIndexChanged"
                                                        CssClass="table table-hover table-striped" AllowSorting="true"  
                                                        GridLines="None" AutoGenerateColumns="False"  
                                                        AutoPostBack="true"                                                   
                                                        Font-Size="8pt" >
                                                    <Columns>

                                                        <asp:BoundField DataField="CodigoItem" HeaderText="#" SortExpression="CodigoItem" />
                                                        <asp:BoundField DataField="CodigoProduto" HeaderText="Cód.Prod."  SortExpression="CodigoProduto" />
                                                        <asp:BoundField DataField="CodigoProdutoPessoa" HeaderText="Cód.Prod.Fornecedor"  SortExpression="CodigoProdutoPessoa" />
                                                        <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Produto"  SortExpression="Cpl_DscProduto" />
                                                        <asp:BoundField DataField="CodigoPessoa" HeaderText="Cód.Forn."  SortExpression="CodigoPessoa" />
                                                        <asp:BoundField DataField="PrecoItem" HeaderText="Preço Fornec."  DataFormatString="{0:F}"  SortExpression="PrecoItem" />
                                                        <asp:BoundField DataField="DataDiaEntrega" HeaderText="Dt.Entrega"  SortExpression="DataDiaEntrega" />
                                                        <asp:BoundField DataField="ObsFinanceira" HeaderText="OBS Financeira" SortExpression="ObsFinanceira" />
                                                        <asp:BoundField DataField="OBSImposto" HeaderText="OBS Impostos"  SortExpression="OBSImposto" />

                                                        <asp:TemplateField HeaderText="Não Trabalha com Produto">
                                                            <ItemTemplate >
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkTrabProd" runat="server" Enabled="false" AutoPostBack="true" Checked='<%# Eval("NaoAtendeItem").ToString().Equals("1") %>' />
                                                                </ContentTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="DataResposta" HeaderText="Dt.Resposta"  SortExpression="DataResposta" />


                                                        <asp:CommandField HeaderText="Editar" ShowSelectButton="True" 
                                                                ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                                ButtonType="Image"  SelectImageUrl ="~/Images/Editar.png" 
                                                                ControlStyle-Width ="25px" ControlStyle-Height ="25px" />                                                                     
                                                    </Columns>
                                                    <RowStyle CssClass="cursor-pointer" />
                                                </asp:GridView>
                                            </ContentTemplate> 

                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdPrdCotacao" />
                                                <asp:AsyncPostBackTrigger ControlID="BtnAddItemCotPreco" />

                                                
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        </div>
                                    </div>
                                        
                                </div>
                                <div role="tabpanel" class="tab-pane" id="aba5" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
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
                            Deseja Cancelar esta Cotação de Preço ?
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
                            Deseja Encerrar esta Cotação de Preço ?
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnEncerrarSim" OnClientClick="this.disabled = true; this.value = 'Encerrando...';" UseSubmitBehavior="false" OnClick="btnEncerrarSim_Click" CssClass="btn btn-success" style="color:white" Text="Encerrar" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEncerrarSim"  />
        </Triggers>
    </asp:UpdatePanel> 
    
    <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="myexcpes3" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" style="height: 100%; width: 500px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H3">Atenção</h4>
                        </div>
                        <div class="modal-body">
                            Deseja Enviar Cotações aos Fornecedores ?
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnEnviarSim" OnClientClick="this.disabled = true; this.value = 'Enviando E-mails...';" UseSubmitBehavior="false" OnClick="btnEnviarSim_Click" CssClass="btn btn-warning" style="color:white" Text="Enviar" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEnviarSim"  />
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
                                                <asp:BoundField DataField="DsMarca" HeaderText="Marca"  />
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
<div class="modal fade" id="CaixaSolicitacao"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:300px;width:720px">
            <div class="panel panel-primary">
                <div class="panel-heading" style="height:40px"> 
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="H1"> Consultar Solicitações de Compra</label>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server"  UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="margin-top: 10px">
                                <div class="col-md-11" >
                                    <label for="usr"> Digite a Descrição do Produto:</label>
                                    <asp:TextBox ID="TextBox1"  PlaceHolder="No minimo 3 Caracteres ..." OnTextChanged="txtPesquisaProduto_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="18" />
                                </div>
                            </div>
                            <div class="row" style="margin-top: 10px">
                                <div class="col-md-11" >
                                    <asp:label id="Label3" runat="server" for="usr" ForeColor="OrangeRed"> </asp:label>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 10px">
                                <div class="panel-body" style="overflow-x:scroll;height:300px;width:700px">
                                    <asp:GridView ID="GridSolCompra" runat="server" Width="100%"
                                        CssClass="table table-bordered table-hover table-striped"
                                        GridLines="None" AutoGenerateColumns="False"
                                        Font-Size="8pt"
                                        OnSelectedIndexChanged="GridSolCompra_SelectedIndexChanged"
                                        AllowPaging="true" PageSize="50"
                                        PagerSettings-Mode="NumericFirstLast">
                                            <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                            <Columns>
                                            <asp:BoundField DataField="CodigoDocumento" HeaderText="Código" ItemStyle-CssClass="padding-top-15" />
                                            <asp:BoundField DataField="NumeroDocumento" HeaderText="Documento"  ItemStyle-CssClass="padding-top-15"/>
                                            <asp:BoundField DataField="Cpl_Pessoa" HeaderText="Fornecedor"  ItemStyle-CssClass="padding-top-15 "/>   
                                            <asp:BoundField DataField="DatahoraEmissao" HeaderText="Data/Hora Emissão" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-CssClass="padding-top-15" />
                                            <asp:BoundField DataField="DataValidade" HeaderText="Data Validade" DataFormatString="{0:dd/MM/yyyy}"  ItemStyle-CssClass="padding-top-15 noprint" HeaderStyle-CssClass="noprint"/>
                                            <asp:BoundField DataField="Cpl_NomeUsuario" HeaderText="Solicitante"  ItemStyle-CssClass="padding-top-15 "/>   
                                            <asp:BoundField DataField="ValorTotal" HeaderText="Total Verba" DataFormatString="{0:C}" ItemStyle-CssClass="padding-top-15"/>
                                    
                                            <asp:TemplateField HeaderText="Situação" ItemStyle-CssClass="centerVertical col-md-1 padding-top-15">
                                                <ItemTemplate>
                                                    <label class="badge <%# Eval("Cpl_Situacao").ToString().Replace(' ','-') %>" id="situacaoSpan"><%# Eval("Cpl_Situacao").ToString() %></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

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
                                                <asp:BoundField DataField="Cpl_Estado" HeaderText="Est."  />
                                                <asp:BoundField DataField="Cpl_Municipio" HeaderText="Município"  />
                                                <asp:BoundField DataField="Cpl_Fone" HeaderText="Fone"  />
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


<%--                                                    <div class="row">
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Cod.Forn.</label>
                                                                <asp:TextBox ID="TextBox4" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Prç Compra</label>
                                                                <asp:TextBox ID="TextBox8" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Datas de Entrega</label>
                                                                <asp:TextBox ID="TextBox5" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Financeira</label>
                                                                <asp:TextBox ID="TextBox6" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Impostos</label>&emsp;&emsp;&emsp;
                                                                <label for="usr" style="font-size: x-small;">Data da Resposta: 15/15/9999 15:12:99</label>
                                                                <asp:TextBox ID="TextBox7" Font-Size="X-Small" Font-Bold ="true" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:1px">
                                                            <asp:CheckBox runat="server"  Font-Size="XX-Small" CssClass="form-control-static CorPadraoEscolhidaTexto" /><strong> Não Atende</strong> 
                                                            <asp:LinkButton ID="btnAprovar1"  Visible ="false"  Font-Size="X-Small" runat="server" Height="20"  Text="Encaminhar para Compra" CssClass="btn btn-success link-button-glyphicon" title="Aprovar" > 
                                                                <span aria-hidden="true" title="Aprovar" class="glyphicon glyphicon-check"></span>  Aprovar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Cod.Forn.</label>
                                                                <asp:TextBox ID="TextBox1" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Prç Compra</label>
                                                                <asp:TextBox ID="TextBox2" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Datas de Entrega</label>
                                                                <asp:TextBox ID="TextBox3" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Financeira</label>
                                                                <asp:TextBox ID="TextBox9" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Impostos</label>&emsp;&emsp;&emsp;
                                                                <label for="usr" style="font-size: x-small;">Data da Resposta: 15/15/9999 15:12:99</label>
                                                                <asp:TextBox ID="TextBox10" Font-Size="X-Small" Font-Bold ="true" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:1px">
                                                            <asp:CheckBox runat="server"  Font-Size="XX-Small" CssClass="form-control-static CorPadraoEscolhidaTexto" /><strong> Não Atende</strong> 
                                                            <asp:LinkButton ID="LinkButton1"  Visible ="false"  Font-Size="X-Small" runat="server" Height="20"  Text="Encaminhar para Compra" CssClass="btn btn-success link-button-glyphicon" title="Aprovar" > 
                                                                <span aria-hidden="true" title="Aprovar" class="glyphicon glyphicon-check"></span>  Aprovar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Cod.Forn.</label>
                                                                <asp:TextBox ID="TextBox11" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Prç Compra</label>
                                                                <asp:TextBox ID="TextBox12" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Datas de Entrega</label>
                                                                <asp:TextBox ID="TextBox13" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Financeira</label>
                                                                <asp:TextBox ID="TextBox14" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Impostos</label>&emsp;&emsp;&emsp;
                                                                <label for="usr" style="font-size: x-small;">Data da Resposta: 15/15/9999 15:12:99</label>
                                                                <asp:TextBox ID="TextBox15" Font-Size="X-Small" Font-Bold ="true" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:1px">
                                                            <asp:CheckBox runat="server"  Font-Size="XX-Small" CssClass="form-control-static CorPadraoEscolhidaTexto" /><strong> Não Atende</strong> 
                                                            <asp:LinkButton ID="LinkButton2"  Visible ="false"  Font-Size="X-Small" runat="server" Height="20"  Text="Encaminhar para Compra" CssClass="btn btn-success link-button-glyphicon" title="Aprovar" > 
                                                                <span aria-hidden="true" title="Aprovar" class="glyphicon glyphicon-check"></span>  Aprovar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Cod.Forn.</label>
                                                                <asp:TextBox ID="TextBox16" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Prç Compra</label>
                                                                <asp:TextBox ID="TextBox17" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Datas de Entrega</label>
                                                                <asp:TextBox ID="TextBox18" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Financeira</label>
                                                                <asp:TextBox ID="TextBox19" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Impostos</label>&emsp;&emsp;&emsp;
                                                                <label for="usr" style="font-size: x-small;">Data da Resposta: 15/15/9999 15:12:99</label>
                                                                <asp:TextBox ID="TextBox20" Font-Size="X-Small" Font-Bold ="true" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:1px">
                                                            <asp:CheckBox runat="server"  Font-Size="XX-Small" CssClass="form-control-static CorPadraoEscolhidaTexto" /><strong> Não Atende</strong> 
                                                            <asp:LinkButton ID="LinkButton3"  Visible ="false"  Font-Size="X-Small" runat="server" Height="20"  Text="Encaminhar para Compra" CssClass="btn btn-success link-button-glyphicon" title="Aprovar" > 
                                                                <span aria-hidden="true" title="Aprovar" class="glyphicon glyphicon-check"></span>  Aprovar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Cod.Forn.</label>
                                                                <asp:TextBox ID="TextBox21" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Prç Compra</label>
                                                                <asp:TextBox ID="TextBox22" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">Datas de Entrega</label>
                                                                <asp:TextBox ID="TextBox23" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Financeira</label>
                                                                <asp:TextBox ID="TextBox24" Font-Bold ="true"  Font-Size="X-Small" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4" style="font-size: x-small;margin-top:3px">
                                                            <div class="input-group " style="width:100%">                              
                                                                <label for="usr" style="font-size: x-small;">OBS. Impostos</label>&emsp;&emsp;&emsp;
                                                                <label for="usr" style="font-size: x-small;">Data da Resposta: 15/15/9999 15:12:99</label>
                                                                <asp:TextBox ID="TextBox25" Font-Size="X-Small" Font-Bold ="true" Height="20" CssClass="form-control" runat="server"  Enabled="True"    />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1" style="font-size: x-small;margin-top:1px">
                                                            <asp:CheckBox runat="server"  Font-Size="XX-Small" CssClass="form-control-static CorPadraoEscolhidaTexto" /><strong> Não Atende</strong> 
                                                            <asp:LinkButton ID="LinkButton4" Visible ="false" Font-Size="X-Small" runat="server" Height="20"  Text="Encaminhar para Compra" CssClass="btn btn-success link-button-glyphicon" title="Aprovar" > 
                                                                <span aria-hidden="true" title="Aprovar" class="glyphicon glyphicon-check"></span>  Aprovar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>--%>
