<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadProduto.aspx.cs" Inherits="SoftHabilInformatica.Pages.Produtos.CadProduto" 
    MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script src="../../Scripts/jquery.maskedinput.min.js"></script>

        <%--meus--%>

    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>

        <script type="text/javascript">
            $(document).ready(function(e) {
                $("input[id*='txtVigencia']").datepicker({
                    todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR"
                });
            });
        </script>
        <script>

        $(document).ready(function (e) {
            $('.js-example-basic-single').select2({
                    
            });
            
            item = '#<%=PanelSelect%>';
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            jQuery(function ($) {
                $("#txtNCM").mask("9999.99.99");
            });
            jQuery(function ($) {
                $("#txtPesquisaNCM").mask("9999.99.99");
            });
        });
        function NCMShow()
        {
            $("#CodigoNCM").modal("show");
        }
        function NCMhide()
        {
            $("#CodigoNCM").modal("hide");
        }
        function AssociadoShow()
        {
            $("#Associado").modal("show");
        }
        function AssociadoHide()
        {
            $("#Associado").modal("hide");
        }
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {            
            $('.js-example-basic-single').select2({
                    
            });
            jQuery(function ($) {
                $("#txtNCM").mask("9999.99.99");
            });
            jQuery(function ($) {
                $("#txtPesquisaNCM").mask("9999.99.99");
            });
            $("input[id*='txtVigencia']").datepicker({
                todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR"
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
                    <asp:TextBox ID="txtCodSisAnterior" runat="server" CssClass="form-control"  Font-Size ="Small"  MaxLength="40" onFocus="this.select()" ></asp:TextBox>
                </div>
                <div class="col-md-2 col-xs-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                    <label for="usr" style ="margin-top:1px;">Cód. Produto Associado</label>
                    <div class="input-group " style="width:100%">
                        <asp:TextBox ID="txtAssociado" Width="80%" AutoPostBack="true" runat="server" CssClass="form-control"  Font-Size ="Small"  MaxLength="40" OnTextChanged="txtAssociado_TextChanged" ></asp:TextBox>
                        <asp:Button runat="server" Width="20%" id="BtnAssociado" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="BtnAssociado_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold;border-bottom-left-radius:0!important; border-top-left-radius:0!important" CssClass="btn btn-success" ToolTip="Adicionar/Alterar produto" TabIndex="5"/>
                    </div>
                </div>
                <div class="col-md-2 col-xs-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                    <label for="usr" style ="margin-top:1px;">Cód. Barras</label>
                    <asp:TextBox ID="txtBarras" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtBarras_TextChanged"  Font-Size ="Small"  MaxLength="40" ></asp:TextBox>
                </div>
                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                    <label for="usr"> Situação: </label>
                    <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="3"> </asp:DropDownList>		   
                </div> 
                <%-----------------------------------------------------------------------------------------------------------------%>

                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="col-md-2" style="background-color:white;border:none;font-size:x-small; text-align:left">
                            <label for="usr">Tipo de Produto: </label>
                            <asp:DropDownList ID="ddlGpoPessoa" runat="server" TabIndex="2" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlGpoPessoa_SelectedIndexChanged" ></asp:DropDownList>                                               
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlGpoPessoa" EventName ="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <div class="row" style="margin-top: 30px">

                <div class="form-group col-md-10" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:10px">
                    <label for="exampleInputPassword1">Descrição do Produto</label>
                    <asp:TextBox ID="txtDscProduto" runat="server"  CssClass="form-control" onFocus="this.select()" MaxLength="100"></asp:TextBox>
                </div>
                <div class="form-group col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:10px">
                    <label for="usr"> Embalagem </label>
                    <asp:TextBox ID="txtEmbalagem" CssClass="form-control" name="txtLancamento" Enabled="true" runat="server" TabIndex="1"  />
                </div>
            </div>               

        <asp:HiddenField ID="TabName" runat="server"/>
            <div id="Tabs" role="tabpanel" >
                <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                    <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                    <li role="presentation"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-euro"></span>&nbsp;&nbsp;Fiscal</a></li>
                    <li role="presentation"><a href="#contact" aria-controls="contact" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Indicadores</a></li>
                    <li role="presentation"><a href="#galeria" aria-controls="contact" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Galeria de fotos</a></li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content" runat="server" id="PanelContext" style="margin-bottom:150px!important">
                    <div role="tabpanel" class="tab-pane" id="home" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;margin-bottom:100px!important"  >
                        <div class="row" >
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" style="padding:0!important">
                                    <ContentTemplate>
                                        <div class="form-inline">
                                            <label for="usr" style="margin-left:40px">Unidade</label>
                                            <div class="form-inline">
                                                <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <asp:DropDownList ID="ddlUnidade" Width="100%" AutoPostBack="true" runat="server" CssClass="form-control" 
                                                        OnSelectedIndexChanged="ddlUnidade_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;
                                                </div>
                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <asp:LinkButton ID="btnAddUnidade" runat="server" Width="100%" Text="Adicionar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnAddUnidade_Click" style="height:33px;margin-top:0!important"> 
                                                        <span aria-hidden="true" title="Adicionar Unidades" class="glyphicon glyphicon-plus"></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr"> Qt. Embalagem</label>
                                <asp:TextBox ID="txtQtEmb" runat="server" AutoPostBack="true" OnTextChanged="txtQtEmb_TextChanged" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr">Marca: </label>
                                <asp:DropDownList ID="ddlMarca" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                            </div>
                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr">Dt. Cadastro</label>
                                <asp:TextBox ID="txtDataCadastro" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr">Dt. Atualização</label>
                                <asp:TextBox ID="txtDataAtualizacao" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row" >
                            <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-left:15px">
                                <label for="usr" style ="margin-top:1px; ">Categoria</label>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" style="padding:0!important">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <asp:TextBox ID="txtCodCategoria" name ="txtCodCategoria" CssClass="form-control" runat="server"
                                                        OnTextChanged="txtCodCategoria_TextChanged" AutoPostBack="true" MaxLength="20" Font-Size ="Medium" 
                                                        Font-Bold ="true"  ForeColor="Blue"   ClientIDMode="Static" onFocus="this.select()" />
                                                </div>
                                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <asp:LinkButton ID="btnConCategoria" runat="server" CssClass="form-control btn btn-primary"  TabIndex="-1" OnClick="btnConCategoria_Click"> 
                                                        <span aria-hidden="true" title="Categoria" class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                </div>
                                                <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
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
                            <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:10px;margin-right:30px;margin-left:15px"">
                                <label for="usr">Fabricante: </label>
                                <asp:DropDownList ID="ddlFabricante" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:10px">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label for="usr">Valor de Compra</label>
                                        <asp:TextBox ID="txtCompra" runat="server" CssClass="form-control" 
                                            AutoPostBack="true" OnTextChanged="txtCompra_TextChanged" onFocus="this.select()" ></asp:TextBox>
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
                                        <asp:TextBox ID="txtLucro" runat="server" CssClass="form-control" AutoPostBack ="true" OnTextChanged="txtLucro_TextChanged" onFocus="this.select()" ></asp:TextBox>
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
                                        <asp:TextBox ID="txt_Venda" runat="server" CssClass="form-control" AutoPostBack="true"  OnTextChanged="txt_Venda_TextChanged" onFocus="this.select()" ></asp:TextBox>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txt_Venda" EventName ="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                                        
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:0px">
                                    <label for="usr">Volume: </label>
                                    <asp:TextBox ID="txtVolume" runat="server" AutoPostBack="true" TabIndex="12" CssClass="form-control " Width="100%" OnTextChanged="txtVolume_TextChanged" Text="0,000" onFocus="this.select()" ></asp:TextBox>
                                </div>
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:0px">
                                    <label for="usr">Peso: </label>
                                    <asp:TextBox ID="txtPeso" runat="server" AutoPostBack="true" TabIndex="13" CssClass="form-control" Width="100%" OnTextChanged="txtPeso_TextChanged" Text="0,000" onFocus="this.select()" ></asp:TextBox>
                                </div>
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:0px">
                                    <label for="usr">Fator de Cubagem: </label>
                                    <asp:TextBox ID="txtFatorCubagem" runat="server" AutoPostBack="true" TabIndex="14" CssClass="form-control" Width="100%" OnTextChanged="txtFatorCubagem_TextChanged" Text="0,000" onFocus="this.select()" ></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtVolume" EventName ="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtPeso" EventName ="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtFatorCubagem" EventName ="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>  
                    </div>

                    <div role="tabpanel" class="tab-pane" id="profile" >
                        <div class="row" style="background-color:white;border:none;">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="col-md-12" style="margin-top: 8px">
                                        <div class="col-md-6" style="font-size:x-small;">
                                            <label for="usr">NCM: </label>
                                            <div class="input-group " style="width:100%">
                                                <asp:TextBox ID="txtNCM" Width="94%" runat="server" OnTextChanged="txtNCM_TextChanged" AutoPostBack="true" name="txtCodigo"  ClientIDMode="Static"  MaxLength="10" CssClass="form-control " ></asp:TextBox>
                                                <asp:Button runat="server" Width="6%" id="BtnAddProduto" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="BtnAddProduto_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold;border-bottom-left-radius:0!important; border-top-left-radius:0!important" CssClass="btn btn-success" ToolTip="Adicionar/Alterar produto" TabIndex="5"/>
                                            </div>
                                        </div>
                                        <div class="col-md-6" style="font-size:x-small;">
                                            <label for="usr">EX: </label>
                                            <asp:TextBox ID="txtEX" runat="server" AutoPostBack="true" MaxLength="5" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="margin-top: 8px">
                                        <div class="col-md-6" style="font-size:x-small;">
                                            <label for="usr">PIS: </label>
                                            <asp:DropDownList ID="ddlPIS" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-6" style="font-size:x-small;">
                                            <label for="usr">COFINS: </label>
                                            <asp:DropDownList ID="ddlCOFINS" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="margin-top: 8px">
                                        <div class="col-md-12 " style="font-size:x-small;">
                                            <label for="usr">Grupo de Tributação : </label>
                                            <asp:DropDownList ID="ddlGpoTribProduto" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="margin-top: 8px">
                                        <div class="col-md-12 " style="font-size:x-small;margin-top:5px;">
                                            <label for="usr">CEST: </label>
                                            <asp:DropDownList ID="ddlCodigoCEST" runat="server" AutoPostBack="true" TabIndex="11" CssClass="form-control js-example-basic-single" Width="100%" OnSelectedIndexChanged="ddlCodigoCEST_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="margin-top: 8px">
                                        <div class="col-md-12 TextBoxFormatado" >
                                            <label for="usr" style="margin-top: 1px;">Descrição CEST</label>
                                            <asp:TextBox ID="txtDescricaoCEST" CssClass="form-control" runat="server"  TextMode="multiline" Columns="10" Rows="5" MaxLength="255" Enabled="false" />        
                                        </div>
                                    </div>
                                    </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtNCM" EventName ="TextChanged"/>
                                    <asp:AsyncPostBackTrigger ControlID="txtDescricaoCEST" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
 
                    <div role="tabpanel" class="tab-pane" id="contact" >

                        <div id="divindicadores" style="padding-left: 100px; border: none; border-radius: 0px;">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:CheckBox ID="chkInventario" Text="&emsp;Produto em Inventário" CssClass="form-control-static" runat="server" onFocus="this.select()" Enabled="true" Font-Size="Small" value="1" />
                                    <br />
                                    <asp:CheckBox ID="chkCrtLote" Text="&emsp;Produto Controla Lote" CssClass="form-control-static" runat="server" onFocus="this.select()" Enabled="true" Font-Size="Small" value="1" />
                                    <br />
                                    </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger  ControlID="chkInventario" EventName="CheckedChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div role="tabpanel" class="tab-pane" id="galeria" >
                        <style>
                            .classFotosGaleria{
                                width:100%;
                                margin-bottom:30px!important;
                                border-radius:5px;
                            }
                            .classFotoPrincipalGaleira{
                                width:100%;
                                border-radius:5px;
                            }
                            .classBotaoRemover {
                                color: white;
                                padding: 5px;
                                font-size: 30px;
                                background-color: rgba(0,0,0,0.5);
                                position: absolute;
                                border-top-left-radius:5px;
                                border-bottom-right-radius:5px;
                            }
                        </style>
                        <div class="col-md-12" style="font-size:x-small;margin-bottom:15px;">
                             <label for="usr">HyperLink do produto: </label>
                            <asp:TextBox ID="txtHyperLinkProduto" runat="server" AutoPostBack="true" MaxLength="100" CssClass="form-control" Width="100%"></asp:TextBox>
                        </div>
                        <div class="col-md-5" style="margin-bottom:10px"><!--Foto Principal-->
                            <asp:LinkButton runat="server" ID="btnRemoverFotoPrincipal" Visible="false" class="classBotaoRemover" OnClick="btnRemoverFotoPrincipal_Click">
                                <span class="glyphicon glyphicon-trash"></span>
                            </asp:LinkButton>
                            <a onclick="AbrirModalWebcam(0)"><asp:Image ImageUrl="../../Images/sem-foto.jpg" runat="server" ID="imgFotoPrincipal" CssClass="classFotoPrincipalGaleira"/></a>
                        </div>
                        <div class="col-md-7"><!--Outras Fotos-->
                            <div class="col-md-4">
                                <asp:LinkButton runat="server" ID="btnRemoverFoto1" Visible="false" class="classBotaoRemover" OnClick="btnRemoverFoto1_Click">
                                    <span class="glyphicon glyphicon-trash"></span>
                                </asp:LinkButton>
                                <a onclick="AbrirModalWebcam(1)">
                                    <asp:Image ImageUrl="../../Images/sem-foto.jpg" runat="server" ID="imgFoto1" CssClass="classFotosGaleria"/>
                                </a>
                            </div>

                            <div class="col-md-4">
                                <asp:LinkButton runat="server" ID="btnRemoverFoto2" Visible="false" class="classBotaoRemover" OnClick="btnRemoverFoto2_Click">
                                    <span class="glyphicon glyphicon-trash"></span>
                                </asp:LinkButton>
                                <a onclick="AbrirModalWebcam(2)">
                                    <asp:Image ImageUrl="../../Images/sem-foto.jpg" runat="server" ID="imgFoto2" CssClass="classFotosGaleria"/>
                                </a>
                            </div>

                            <div class="col-md-4">
                                <asp:LinkButton runat="server" ID="btnRemoverFoto3" Visible="false" class="classBotaoRemover" OnClick="btnRemoverFoto3_Click">
                                    <span class="glyphicon glyphicon-trash"></span>
                                </asp:LinkButton>
                                <a onclick="AbrirModalWebcam(3)">
                                    <asp:Image ImageUrl="../../Images/sem-foto.jpg" runat="server" ID="imgFoto3" CssClass="classFotosGaleria"/>
                                </a>
                            </div>

                            <div class="col-md-4">
                                <asp:LinkButton runat="server" ID="btnRemoverFoto4" Visible="false" class="classBotaoRemover" OnClick="btnRemoverFoto4_Click">
                                    <span class="glyphicon glyphicon-trash"></span>
                                </asp:LinkButton>
                                <a onclick="AbrirModalWebcam(4)">
                                    <asp:Image ImageUrl="../../Images/sem-foto.jpg" runat="server" ID="imgFoto4" CssClass="classFotosGaleria"/>
                                </a>
                            </div>

                            <div class="col-md-4">
                                <asp:LinkButton runat="server" ID="btnRemoverFoto5" Visible="false" class="classBotaoRemover" OnClick="btnRemoverFoto5_Click">
                                    <span class="glyphicon glyphicon-trash"></span>
                                </asp:LinkButton>
                                <a onclick="AbrirModalWebcam(5)">
                                    <asp:Image ImageUrl="../../Images/sem-foto.jpg" runat="server" ID="imgFoto5" CssClass="classFotosGaleria"/>
                                </a>
                            </div>

                        </div>
                        <script>
                            function errorMessage(message, e) {
                                $("#ErroWebcam").html(message);
                                document.getElementById('btnSnap').style.display = 'none';
                            }
                            var CodFoto = 0;
                            function AbrirModalWebcam(CodigoFoto) {

                                $("#ErroWebcam").html("");
                                document.getElementById('btnSnap').style.display = 'block';

                                CodFoto = CodigoFoto;
                                $("#modalWebCam .modal-title").html("Tire a foto do produto");
                                $("#modalWebCam").modal("show");
                                //environment ou user
                                //if (location.protocol === 'https:') {
                                    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
                                    if (navigator.getUserMedia) {
                                        navigator.getUserMedia({
                                            video: true,
                                            facingMode: {
                                              exact: 'environment'
                                            }
                                        }, function (stream) {
                                            document.getElementById("vid").srcObject = stream;
                                            var mediaStreamTrack = stream.getVideoTracks()[0];
                                            if (typeof mediaStreamTrack != "undefined") {
                                                mediaStreamTrack.onended = function () {//for Chrome.
                                                    errorMessage('Sua webcam está ocupada')
                                                }
                                            } else errorMessage('Permissão negada!');
                                        }, function (e) {
                                            var message;
                                            switch (e.name) {
                                                case 'NotFoundError':
                                                case 'DevicesNotFoundError':
                                                    message = 'Por favor, configure sua webcam primeiro.';
                                                    break;
                                                case 'SourceUnavailableError':
                                                    message = 'Sua webcam está ocupada';
                                                    break;
                                                case 'PermissionDeniedError':
                                                case 'SecurityError':
                                                    message = 'Permissão negada!';
                                                    break;
                                                default: errorMessage('Acesso a Webcam rejeitado!!', e);
                                                    return;
                                            }
                                            errorMessage(message);
                                        });
                                    } else errorMessage('Navegador incompatível!');
                                
                                //} else errorMessage('Use o protocolo https para abrir esta página.')

                            }

                            function snap() {

                                var video = document.querySelector("#vid");
	                            //Criando um canvas que vai guardar a imagem temporariamente
	                            var canvas = document.createElement('canvas');
	                            canvas.width = video.videoWidth;
	                            canvas.height = video.videoHeight;
	                            var ctx = canvas.getContext('2d');
	
	                            //Desenhando e convertendo as dimensões
	                            ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
	
	                            //Criando o JPG
                                var dataURI = canvas.toDataURL('image/jpeg'); //O resultado é um BASE64 de uma imagem.

                                if (CodFoto == 0) {
                                    $get('<%=imgFotoPrincipal.ClientID%>').src = dataURI;
                                }
                                else if (CodFoto == 1) {
                                    $get('<%=imgFoto1.ClientID%>').src = dataURI;
                                }
                                else if (CodFoto == 2) {
                                    $get('<%=imgFoto2.ClientID%>').src = dataURI;
                                }
                                else if (CodFoto == 3) {
                                    $get('<%=imgFoto3.ClientID%>').src = dataURI;
                                }
                                else if (CodFoto == 4) {
                                    $get('<%=imgFoto4.ClientID%>').src = dataURI;
                                }
                                else if (CodFoto == 5) {
                                    $get('<%=imgFoto5.ClientID%>').src = dataURI;
                                }
                                
                                document.getElementById("<%=base_img.ClientID%>").value = CodFoto + "³" + dataURI;
                                $get('<%=btnRefresh.ClientID%>').click(); 
                                
                            }
                        </script>
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
        
<div class="modal fade" id="CodigoNCM"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
    <div class="modal-dialog" role="document" style ="height:300px;width:720px">
        <div class="panel panel-primary">
            <div class="panel-heading" style="height:40px"> 
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <label class="modal-title" id="H6"> Consulta de Regra fiscal IPI</label>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel10" runat="server"  UpdateMode="Always">
                    <ContentTemplate>  
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-3" style="padding-right:0!important" >
                                <label for="usr"> Dt Vigência:</label>
                                <asp:TextBox ID="txtVigencia" MaxLength="8" CssClass="form-control" Font-Size ="Medium" runat="server" TabIndex="8" />
                            </div>
                            <div class="col-md-8" >
                                <label for="usr"> Digite o Código NCM:</label>
                                <asp:TextBox ID="txtPesquisaNCM" MaxLength="8" name="txtCodigo"  ClientIDMode="Static" OnTextChanged="txtPesquisaNCM_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-11" >
                                <asp:label id="labelmens" runat="server" for="usr" AutoPostBack="true"  ForeColor="OrangeRed"> </asp:label>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="panel-body" style="overflow-x:scroll;height:300px;width:700px">
                                <asp:GridView ID="GridNCM" runat="server" Width="100%"
                                    CssClass="table table-bordered table-hover table-striped"
                                    GridLines="None" AutoGenerateColumns="False"
                                    Font-Size="8pt"
                                    OnSelectedIndexChanged="GridNCM_SelectedIndexChanged"
                                    AllowPaging="true" PageSize="50"
                                    OnPageIndexChanging="GridNCM_PageIndexChanging"
                                    PagerSettings-Mode="NumericFirstLast">
                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                        <Columns>
                                            <asp:BoundField DataField="DtVigencia" HeaderText="Dt. Vigência" />
                                            <asp:BoundField DataField="CodigoRegraFiscalIPI" HeaderText="Código" />
                                            <asp:BoundField DataField="CodigoNCM" HeaderText="NCM"  />
                                            <asp:BoundField DataField="DescricaoNCM" HeaderText="Descrição"  />
                                            <asp:BoundField DataField="CodigoEx" HeaderText="Cód. Ex"  />
                                            <asp:BoundField DataField="CodigoEnquadramento" HeaderText="Enquadramento"  />
                                            <asp:BoundField DataField="PercentualIPI" HeaderText="% IPI"  />
                                            <asp:BoundField DataField="DescricaoTipo" HeaderText="Tipo"  />
                                            <asp:BoundField DataField="DescricaoSituacao" HeaderText="Situação"  />
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
                    <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="txtVigencia" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger  ControlID="txtPesquisaNCM" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>
              
<div class="modal fade" id="Associado"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
    <div class="modal-dialog" role="document" style ="height:300px;width:520px">
        <div class="panel panel-primary">
            <div class="panel-heading" style="height:40px"> 
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <label class="modal-title" id="H7"> Consulta de Produtos Associados</label>
            </div>
            <div class="panel-body">
                <div class="row" style="margin-top: 10px">
                    <div class="col-md-11" >
                        <label for="usr"> Digite a Descrição Produto</label>
                        <asp:TextBox ID="txtPesquisaDescricao"  PlaceHolder="No minimo 3 Caracteres ..." OnTextChanged="txtPesquisaDescricao_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" />
                    </div>                                 
                </div>
                <div class="row" style="margin-top: 10px">
                    <div class="col-md-11" >
                        <asp:label id="Label1" runat="server" for="usr" ForeColor="OrangeRed"> </asp:label>
                    </div>
                </div>
                <div class="row" style="margin-top: 10px">
                    <div class="panel-body" style="overflow-x:scroll;height:300px;width:500px">
                        <asp:GridView ID="GridAssociados" runat="server" Width="100%"
                            CssClass="table table-bordered table-hover table-striped"
                            GridLines="None" AutoGenerateColumns="False"
                            Font-Size="8pt" 
                            OnSelectedIndexChanged="GridAssociados_SelectedIndexChanged"
                            AllowPaging="true" PageSize="50"
                            OnPageIndexChanging="GridAssociados_PageIndexChanging"
                            PagerSettings-Mode="NumericFirstLast">
                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                <Columns>
                                    <asp:BoundField DataField="CodigoProduto" HeaderText="Código" />
                                    <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição"  />
                                    <asp:BoundField DataField="ValorCompra" HeaderText="Valor Compra" DataFormatString="{0:C}"/>
                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                        ItemStyle-Height="15px" ItemStyle-Width="50px"
                                        ButtonType="Image" SelectImageUrl="~/Images/Acessar.svg"
                                        ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                        </asp:GridView>   
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
    <div class="modal fade" id="modalWebCam" tabindex="-1" role="dialog" aria-labelledby="myLargeModal" >
        <div class="modal-dialog" role="document" style="width:60%!important;margin-top:10px!important">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="CodigoFoto"></h4>
                </div>
                <div class="modal-body" style="text-align:center">
                     
                    <video id="vid" autoplay muted playsinline></video>
                    <asp:textbox runat="server"  type="text" id="base_img" name="base_img" Style="display:none"></asp:textbox>

                    <p id="caminhoImagem" class="caminho-imagem"><a href="" target="_blank"></a></p>
                    <div style="margin-top:15px">
                        <p id="ErroWebcam"></p>
                        <asp:Linkbutton class="btn btn-default" runat="server" onclick="btnRefresh_Click" ID="btnRefresh" style="height:35px;font-size:18px;display:none">
                            <span aria-hidden="true" title="Tirar Foto" class="glyphicon glyphicon-refresh"></span>
                        </asp:Linkbutton>
                                
                        <input type="button" id="btnSnap" class="btn btn-success"  value="Capturar" onclick="snap();" />

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

