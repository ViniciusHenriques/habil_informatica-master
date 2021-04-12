<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="CadTipoOperacao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Fiscal.CadTipoOperacao" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server" onFocus="this.select()">
    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
    </script>
       
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>

    <script type="text/javascript">
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $(".js-example-basic-single").select2();
        }
        $(document).ready(function () {
            $('.js-example-basic-single').select2();

        });

        
    </script>
    <style type="text/css">

        .row{
            margin-bottom:5px!important;
        }
    </style>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;">

        <div class="panel panel-primary">
            <div class="panel-heading">
                Cadastro de Tipos de Operação
            <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server" onFocus="this.select()">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" onFocus="this.select()" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" onFocus="this.select()" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>

            </div>
            <div class="panel-body">

                <asp:UpdatePanel ID="UpdatePanel5" runat="server" onFocus="this.select()" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:LinkButton ID="btnVoltar" runat="server" onFocus="this.select()" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnSalvar" runat="server" onFocus="this.select()" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcluir" runat="server" onFocus="this.select()" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="btnVoltar" EventName="Click" />
                        <asp:AsyncPostBackTrigger  ControlID="btnSalvar"  EventName ="Click"  />
                    </Triggers>
                </asp:UpdatePanel>

                <br />
                <br />

                <div class="container-fluid" style="background-color: white;">
                    <div class="row" style="background-color: white; border: none;">
                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Código : &nbsp;&nbsp;&nbsp; </span>
                                <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" Enabled="false" runat="server" onFocus="this.select()" 
                                    onkeypress="return PermiteSomenteNumeros(event);" MaxLength="6" />
                            </div>
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Always">
                            <ContentTemplate>

                                <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: small;">
                                    <div class="input-group">
                                        <span class="input-group-addon">Tipo de Movimentação: &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                        <asp:DropDownList ID="ddlTipoMovimentacao" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single" Font-Size="Small" OnSelectedIndexChanged="ddlTipoMovimentacao_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                    </div>

                                </div>

                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                    <div class="input-group">
                                        <span class="input-group-addon">Situação : &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                        <asp:DropDownList ID="ddlCodSituacao" runat="server" onFocus="this.select()" AutoPostBack="True" CssClass="form-control" Font-Size="Small" OnSelectedIndexChanged ="ddlCodSituacao_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger  ControlID="ddlTipoMovimentacao" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger  ControlID="ddlCodSituacao"  EventName ="SelectedIndexChanged"  />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="background-color: white; border: none;">
                                <div class="col-md-12" style="font-size: x-small;">
                                    <div class="input-group">
                                        <span class="input-group-addon">Descrição : </span>
                                        <asp:TextBox ID="txtDescricao" CssClass="form-control" name="txtDescricao"  AutoPostBack="true"  runat="server" onFocus="this.select()" MaxLength="100" OnTextChanged="txtDescricao_TextChanged" />
                                    </div>
                                </div>
                                <div class="col-md-6" style="font-size: x-small;">
                                    <div class="input-group">
                                        <span class="input-group-addon">PIS : </span>
                                        <asp:DropDownList ID="ddlPIS" runat="server" AutoPostBack="false" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6" style="font-size: x-small;">
                                    <div class="input-group">
                                        <span class="input-group-addon">COFINS : </span>
                                        <asp:DropDownList ID="ddlCOFINS" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6" style="font-size: x-small;">
                                    <div class="input-group">
                                        <span class="input-group-addon">Precedência do ICMS : </span>
                                        <asp:DropDownList ID="ddlPrecedenciaICMS" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6" style="font-size: x-small;">
                                    <div class="input-group">
                                        <span class="input-group-addon">Precedência do PIS/COFINS : </span>
                                        <asp:DropDownList ID="ddlPrecedenciaPISCOFINS" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger  ControlID="txtDescricao" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <br />
                    <div id="Tabs" role="tabpanel">

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist" id="myTabs">
                            <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-check"></span>&nbsp;&nbsp;Natureza da Operação</a></li>
                            <li role="presentation"><a href="#contrato" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Indicadores</a></li>
                            <li role="presentation"><a href="#contrato2" aria-controls="profile2" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-resize-full"></span>&nbsp;&nbsp;ICMS</a></li>
                        </ul>

                        <asp:HiddenField ID="TabName" runat="server" />

                        <!-- Tab panes -->
                        <div class="tab-content" runat="server"  id="PanelContext">

                            <div role="tabpanel " class="tab-pane" id="home">

                                <div class="container-fluid">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="row" style="background-color: white; border: none;">
                                        <br />
                                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                            <br />
                                            <div class="input-group">
                                                <span class="input-group-addon">Tipo de Operação Fiscal: &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                <asp:DropDownList ID="ddlTipoOprFiscal" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single" Font-Size="Small" width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <br />
                                                <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                            <br />
                                            <div class="input-group">
                                                <span class="input-group-addon">Tipo de Operação de Contra Partida: &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                <asp:DropDownList ID="ddlTipoOprCtPartida" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single" Font-Size="Small" width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <br />
                                       
                                        
                                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                            <br />
                                            <div class="input-group">
                                                <span class="input-group-addon">Natureza da Operação Estadual: &nbsp;</span>
                                                <asp:DropDownList ID="ddlCFOPEstadual" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single " Font-Size="Small" width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <br />

                                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                            <br />
                                            
                                            <div class="input-group">
                                                <span class="input-group-addon">Natureza da Operação Interestadual: &nbsp;</span>
                                                <asp:DropDownList ID="ddlCFOPInterestadual" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single " Font-Size="Small" width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <br />

                                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                            <br />
                                            
                                            <div class="input-group">
                                                <span class="input-group-addon">Natureza da Operação Exterior: &nbsp;</span>
                                                <asp:DropDownList ID="ddlCFOPExterior" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single " Font-Size="Small" width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger  ControlID="ddlTipoOprFiscal" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger  ControlID="ddlTipoOprCtPartida" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger  ControlID="ddlCFOPEstadual"  EventName ="SelectedIndexChanged"  />
                                            <asp:AsyncPostBackTrigger  ControlID="ddlCFOPInterestadual" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger  ControlID="ddlCFOPExterior" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>
                            </div>

                            <div role="tabpanel" class="tab-pane" id="contrato">
                                <br />
                                <div id="divindicadores" style="padding-left: 100px; border: none; border-radius: 0px;">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkAtuEstoque" Text="&emsp;Atualiza Estoque" CssClass="form-control-static" runat="server" onFocus="this.select()" Enabled="true" Font-Size="Small" value="1" />
                                            <br />
                                            <asp:CheckBox ID="chkMovInterna" Text="&emsp;Movimentação Interna" CssClass="form-control-static" OnCheckedChanged="chkMovInterna_CheckedChanged" runat="server" onFocus="this.select()" Enabled="true" Font-Size="Small" value="2" AutoPostBack="true" />
                                            <br />
                                            <asp:CheckBox ID="chkAtuFinanceiro" Text="&emsp;Atualiza Financeiro" CssClass="form-control-static" runat="server" onFocus="this.select()" Enabled="true" Font-Size="Small" value="3" OnCheckedChanged="chkAtuFinanceiro_CheckedChanged" AutoPostBack="true" />
                                            <br />
                                            <asp:CheckBox ID="chkBaixaFinanceiro" Text="&emsp;Baixa Financeiro" CssClass="form-control-static" runat="server" onFocus="this.select()" Enabled="true" Font-Size="Small" value="4" OnCheckedChanged="chkBaixaFinanceiro_CheckedChanged" AutoPostBack="true" />
                                            <br />  
                                            <asp:CheckBox ID="chkMovLocOrigemDest" Text="&emsp;Movimentação de Localização Origem Destino" CssClass="form-control-static" runat="server" onFocus="this.select()" Enabled="true" Font-Size="Small" value="4" OnCheckedChanged="chkMovLocOrigemDest_CheckedChanged" AutoPostBack="true" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger  ControlID="chkAtuFinanceiro" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger  ControlID="chkBaixaFinanceiro" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger  ControlID="chkMovInterna" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger  ControlID="chkMovLocOrigemDest" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger  ControlID="chkAtuEstoque" EventName="CheckedChanged" />

                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                            <div role="tabpanel" class="tab-pane" id="contrato2">
                                <br />

                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                <label for="usr">Regime Tributário : </label>
                                            </div>
                                            <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                <asp:DropDownList ID="ddlRegTributario" Width="100%"  runat="server" onFocus="this.select()" AutoPostBack="true" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlRegTributario_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>



                                        <asp:Panel ID ="pnlImpostos" runat="server" Visible ="false">
                                            <div class="row">
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                    <label for="usr">CST/CSOSN : </label>
                                                </div>
                                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                    <asp:DropDownList ID="ddlCSTCSOSN" runat="server"  AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlCSTCSOSN_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                    <label for="usr">Modalidade de determinação da BC do ICMS : </label>
                                                </div>
                                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                    <asp:DropDownList ID="ddlModDetBCIcms" runat="server"  AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                    <label for="usr">Modalidade de determinação da BC do ICMS ST : </label>
                                                </div>
                                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                    <asp:DropDownList ID="ddlModDetBCIcmsST" runat="server"  AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-bottom:5px">
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                    <label for="usr">Margem de Valor Agregado (MVA) Entrada : </label>
                                                </div>
                                                <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <asp:TextBox ID="txtMvaEntrada" CssClass="form-control" AutoPostBack="true" name="txtMvaEntrada" runat="server" onFocus="this.select()" 
                                                        MaxLength="50" Style="text-align: right;" OnTextChanged="txtMvaEntrada_TextChanged" />
                                                </div>
                                                <div class="col-md-6"></div>
                                            </div>

                                            <div class="row" style="margin-bottom:5px">
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                    <label for="usr">Mensagem do Icms : </label>
                                                </div>
                                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                     <asp:TextBox ID="txtMsgIcms" CssClass="form-control" AutoPostBack="true" name="txtMsgIcms" runat="server" MaxLength="1000" TextMode="MultiLine"/>
                                                </div>
                                            </div>

                                            <div class="row" style="margin-bottom:5px">
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                    <label for="usr">Código do Benefício Fiscal : </label>
                                                </div>
                                                <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                     <asp:TextBox ID="txtCodBnfFiscal" CssClass="form-control" AutoPostBack="true" name="txtCodBnfFiscal" runat="server" MaxLength="15"/>
                                                </div>
                                                <div class="col-md-6"></div>
                                            </div>

                                            <div class="row">

                                                <div class="col-md-3"></div>

                                                <div class="col-md-6">

                                                    <div class="panel panel-info">
                                                        <div class="panel-heading" style="text-align: center;">
                                                            <asp:Label ID="lblRegTributario" Text="" runat="server" onFocus="this.select()" />
                                                            <br />
                                                            <asp:Label ID="lblCSTCSOSN" Text="" runat="server" onFocus="this.select()" />
                                                        </div>
                                                        <div class="panel-body">

                                                            <asp:Panel ID="pnlCST00" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Interna da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST00Icms" CssClass="form-control" AutoPostBack="true" name="txtAliquota" runat="server" onFocus="this.select()" Text ="0,00"  MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST00Icms_TextChanged" />
                                                                    </div>
                                                                    <div class="col-md-1"></div>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCST10" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST10RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCST10RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST10RedBCIcmsST_TextChanged" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST10Icms" CssClass="form-control" AutoPostBack="true" name="txtCST10Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST10Icms_TextChanged" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST10RedBCIcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST10RedBCIcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST10RedBCIcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST10IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST10IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50"  Text="0,00" Style="text-align: right;" OnTextChanged="txtCST10IcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST10Mva" CssClass="form-control" AutoPostBack="true" name="txtCST10Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST10Mva_TextChanged" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <div id="divindicadores1" style="padding-left: 0px; border: none; border-radius: 0px;">
                                                                            <asp:CheckBox ID="chkCST10Difal" Text="&emsp;Calcula Difal: " CssClass="form-control-static" runat="server" onFocus="this.select()" AutoPostBack="true" Enabled="true" Font-Size="Small" value="1" OnCheckedChanged="chkCST10Difal_CheckedChanged"  />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                    </div>
                                                                    <br />
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Diferencial de Alíquota : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST10Difal" CssClass="form-control" AutoPostBack="true" name="txtCST10Difal" runat="server" onFocus="this.select()" MaxLength="50" Enabled ="false"  Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST10Difal_TextChanged" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCST20" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST20RedBC" CssClass="form-control" AutoPostBack="true" name="txtCST20RedBC" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged ="txtCST20RedBC_TextChanged" />
                                                                    </div>
                                                                    <div class="col-md-2"></div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Interna da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST20Icms" CssClass="form-control" AutoPostBack="true" name="txtCST20Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST20Icms_TextChanged" />
                                                                    </div>
                                                                    <div class="col-md-2"></div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Alíquota Efetiva : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST20Efetiva" CssClass="form-control" AutoPostBack="true" name="txtCST20Efetiva" runat="server" onFocus="this.select()" MaxLength="50" Enabled="false"  Text="0,00" Style="text-align: right;" />
                                                                    </div>
                                                                    <div class="col-md-2"></div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Motivo da Desoneração: </label>
                                                                    </div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:DropDownList ID="ddlCST20MovDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCST30" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST30RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCST30RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged ="txtCST30RedBCIcmsST_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST30Icms" CssClass="form-control" AutoPostBack="true" name="txtCST30Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged ="txtCST30Icms_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST30IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST30IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50"  Text="0,00" Style="text-align: right;" OnTextChanged="txtCST30IcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST30Mva" CssClass="form-control" AutoPostBack="true" name="txtCST30Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged ="txtCST30Mva_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Motivo da Desoneração: </label>
                                                                    </div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:DropDownList ID="ddlCST30MotDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCST404150" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <label for="usr">Motivo da Desoneração: </label>
                                                                    <asp:DropDownList ID="ddlCST405051MovDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                </div>

                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCST51" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST51RedBC" CssClass="form-control" AutoPostBack="true" name="txtCST51RedBC" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST51RedBC_TextChanged" />
                                                                    </div>
                                                                    <div class="col-md-2"></div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Interna da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST51Icms" CssClass="form-control" AutoPostBack="true" name="txtCST51Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST51Icms_TextChanged" />
                                                                    </div>
                                                                    <div class="col-md-2"></div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Diferimento (Total ou Parcial) : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST51Diferimento" CssClass="form-control" AutoPostBack="true" name="txtCST51Diferimento" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" OnTextChanged="txtCST51Diferimento_TextChanged" />
                                                                    </div>
                                                                    <div class="col-md-2"></div>
                                                                </div>

                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCST70" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-2"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST70RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCST10RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST70RedBCIcmsST_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST70Icms" CssClass="form-control" AutoPostBack="true" name="txtCST70Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged ="txtCST70Icms_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST70RedBCIcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST70RedBCIcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST70RedBCIcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST70IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST10IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50"  Text="0,00" Style="text-align: right;" OnTextChanged ="txtCST70IcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST70Mva" CssClass="form-control" AutoPostBack="true" name="txtCST70Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST70Mva_TextChanged" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Motivo da Desoneração: </label>
                                                                    </div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:DropDownList ID="ddlCST70MovDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCST90" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST90RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCST90RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged ="txtCST90RedBCIcmsST_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST90Icms" CssClass="form-control" AutoPostBack="true" name="txtCST90Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST90Icms_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST90RedBCIcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST90RedBCIcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST90RedBCIcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST90IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST90IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50"  Text="0,00" Style="text-align: right;" OnTextChanged ="txtCST90IcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST90Mva" CssClass="form-control" AutoPostBack="true" name="txtCST90Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged ="txtCST90Mva_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <div id="divindicadores2" style="padding-left: 0px; border: none; border-radius: 0px;">
                                                                            <asp:CheckBox ID="chkCST90CalcDifal" Text="&emsp;Calcula Difal: " CssClass="form-control-static" runat="server" onFocus="this.select()" AutoPostBack="true"  Enabled="true" Font-Size="Small" value="1" OnCheckedChanged="chkCST90CalcDifal_CheckedChanged" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                    </div>
                                                                    <br />
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Diferencial de Alíquota : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCST90Difal" CssClass="form-control" AutoPostBack="true" name="txtCST90Difal" runat="server" onFocus="this.select()" MaxLength="50" Enabled ="false"  Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST90Difal_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Motivo da Desoneração: </label>
                                                                    </div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:DropDownList ID="ddlCST90MovDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single" ></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>

                                                            <asp:Panel ID="pnlCSOSN101" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Interna da Operação do Simples Nacional : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN101IcmsSimples" CssClass="form-control" AutoPostBack="true" name="txtCSOSN101IcmsSimples" runat="server" onFocus="this.select()" Text ="0,00"  MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN101IcmsSimples_TextChanged" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCSOSN201" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN201RedBCST" CssClass="form-control" AutoPostBack="true" name="txtCSOSN201RedBCST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN201RedBCST_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN201Icms" CssClass="form-control" AutoPostBack="true" name="txtCSOSN201Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN201Icms_TextChanged" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN201IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST10IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50"  Text="0,00" Style="text-align: right;" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN201Mva" CssClass="form-control" AutoPostBack="true" name="txtCSOSN201Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN201Mva_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Interna da Operação do Simples Nacional : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN201IcmsSimples" CssClass="form-control" AutoPostBack="true" name="txtCSOSN201IcmsSimples" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN201IcmsSimples_TextChanged" />
                                                                    </div>
                                                                </div>



                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCSOSN202203" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN202203RedBCST" CssClass="form-control" AutoPostBack="true" name="txtCSOSN202203RedBCST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN202203RedBCST_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN202203Icms" CssClass="form-control" AutoPostBack="true" name="txtCSOSN202203Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged ="txtCSOSN202203Icms_TextChanged" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN202203IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCSOSN202203IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50"  Text="0,00" Style="text-align: right;" OnTextChanged ="txtCSOSN202203IcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN202203Mva" CssClass="form-control" AutoPostBack="true" name="txtCSOSN202203Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN202203Mva_TextChanged" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCSOSN900" runat="server" onFocus="this.select()" Visible="false">
                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN900RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN900RedBCIcmsST_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota da Operação : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN900Icms" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900Icms" runat="server" onFocus="this.select()" Text ="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN900Icms_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">% Redução da Base de Cálculo Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN900RedBCIcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900RedBCIcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged ="txtCSOSN900RedBCIcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Icms Próprio : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN900IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50"  Text="0,00" Style="text-align: right;" OnTextChanged ="txtCSOSN900IcmsProprio_TextChanged" />
                                                                    </div>
                                                                </div>


                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN900Mva" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged ="txtCSOSN900Mva_TextChanged" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-1"></div>
                                                                    <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                        <label for="usr">Alíquota Interna da Operação do Simples Nacional : </label>
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        <asp:TextBox ID="txtCSOSN900IcmsSimples" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900IcmsSimples" runat="server" onFocus="this.select()" Text ="0,00"  MaxLength="50" Style="text-align: right;" OnTextChanged ="txtCSOSN900IcmsSimples_TextChanged" />
                                                                    </div>
                                                                </div>

                                                            </asp:Panel>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </asp:Panel>

                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger  ControlID="ddlRegTributario" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger  ControlID="ddlCSTCSOSN"  EventName ="SelectedIndexChanged"  />
                                        <asp:AsyncPostBackTrigger  ControlID="ddlModDetBCIcms" EventName ="SelectedIndexChanged"  />
                                        <asp:AsyncPostBackTrigger  ControlID="ddlModDetBCIcmsST" EventName ="SelectedIndexChanged"  />

                                        <asp:AsyncPostBackTrigger  ControlID="txtMvaEntrada" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST00Icms" EventName ="TextChanged"/>

                                        <asp:AsyncPostBackTrigger  ControlID="txtCST10RedBCIcmsST" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST10Icms" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST10IcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST10RedBCIcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST10Mva" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="chkCST10Difal" EventName ="CheckedChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST10Difal" EventName ="TextChanged"/>

                                        <asp:AsyncPostBackTrigger  ControlID="txtCST20RedBC" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST20Icms" EventName ="TextChanged"/>
                                        
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST30RedBCIcmsST" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST30Icms" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST30IcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST30Mva" EventName ="TextChanged"/>

                                        <asp:AsyncPostBackTrigger  ControlID="txtCST51RedBC" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST51Icms" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST51Diferimento" EventName ="TextChanged"/>

                                        <asp:AsyncPostBackTrigger  ControlID="txtCST70RedBCIcmsST" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST70Icms" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST70IcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST70RedBCIcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST70Mva" EventName ="TextChanged"/>

                                        <asp:AsyncPostBackTrigger  ControlID="txtCST90RedBCIcmsST" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST90Icms" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST90IcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST90RedBCIcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST90Mva" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="chkCST90CalcDifal" EventName ="CheckedChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCST90Difal" EventName ="TextChanged"/>
                                        
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN101IcmsSimples" EventName ="TextChanged"/>

                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN201RedBCST" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN201Icms" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN201Mva" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN201IcmsSimples" EventName ="TextChanged"/>

                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN202203RedBCST" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN202203Icms" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN202203IcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN202203Mva" EventName ="TextChanged"/>

                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN900RedBCIcmsST" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN900Icms" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN900IcmsProprio" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN900Mva" EventName ="TextChanged"/>
                                        <asp:AsyncPostBackTrigger  ControlID="txtCSOSN900IcmsSimples" EventName ="TextChanged"/>
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Exclui Grupo -->
    <div class="modal fade" id="myexcpes" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h2 class="modal-title" id="H3">Atenção</h2>
                </div>
                <div class="modal-body">Deseja Excluir este Tipo de Operação ?</div>
                <div class="modal-footer">
                    <asp:Button ID="btnCfmSim" runat="server" onFocus="this.select()" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
