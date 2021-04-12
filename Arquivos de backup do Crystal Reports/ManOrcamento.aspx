<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManOrcamento.aspx.cs" Inherits="SoftHabilInformatica.Pages.Vendas.ManOrcamento" 
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
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            $('.js-example-basic-single').select2({});
            $("input[id*='txtdtValidade']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});

        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            
            $('.js-example-basic-single').select2({});           
            $("input[id*='txtdtValidade']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });

            item = '#<%=PanelSelect%>';
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

        }
        function MontaGraficoCredito(Disponivel, Usado)
        {
            CanvasJS.addColorSet("greenShades",["#40E0D0","#B22222"]);
            var credito = {
	            colorSet: "greenShades",
                animationEnabled: true,
                height:300,
	            data: [{
		            type: "pie",
		            indexLabelFontSize: 18,
		            radius: 80,
		            indexLabel: "{label} - {y}%",
		            yValueFormatString: "###0.0\"\"",
		            click: explodePie,
                    dataPoints: [
                        { y: Disponivel, label: "Disponível" },
                        { y: Usado, label: "Utilizado" },


		            ]
	            }]
            };
           
            $("#chartContainer").CanvasJSChart(credito);
            chart.render();

            function explodePie(e) {
	            for(var i = 0; i < e.dataSeries.dataPoints.length; i++) {
		            if(i !== e.dataPointIndex)
			            e.dataSeries.dataPoints[i].exploded = false;
	            }
            }       
        }      
    </script>
    <style type="text/css">
        @media screen and (max-width: 600px) {
            .noprint{display:none;}  

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
         .canvasjs-chart-credit,.canvasjs-chart-tooltip{
	        display:none!important;
         }
         
    </style>
   
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary">
            <div class="panel-heading" >
                Manutenção do Orçamento
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body" >
                    <div class="row" style="background-color: white; border: none;">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">                           
                                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                                    </asp:LinkButton>
                                     <asp:LinkButton ID="btnGerarPedido" runat="server" Text="Gerar pedido" CssClass="btn btn-warning" TabIndex="1"  OnClick="btnGerarPedido_Click"> 
                                        <span aria-hidden="true" class="glyphicon glyphicon-pencil"></span>  Gerar pedido
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnSalvar" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none; margin-top:10px!important">
                        
                        <asp:HiddenField ID="TabName" runat="server"/>            
                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;" >
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <ul class="nav nav-tabs " role="tablist" id="myTabs">
                                        <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                                        <li role="presentation"  style="<%=PanelInfoCliente%>" title="Informações do cliente"><a href="#consulta1" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-user"></span>&nbsp;&nbsp;Dados do cliente</a></li>
                                        <li role="presentation" style="<%=PanelInfoCliente%>"><a href="#consulta7" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-briefcase"></span>&nbsp;&nbsp;Financeiro</a></li>
                                        <li role="presentation" style="<%=PanelInfoCliente%>"><a href="#consulta8" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-hourglass"></span>&nbsp;&nbsp;Consumo</a></li>
                                        <li role="presentation" style="<%=Panels%>"><a href="#consulta2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Eventos</a></li>
                                        <li role="presentation" style="<%=TabLogs%>" title="Alterações feitas no Documento"><a href="#consulta3" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-check"></span>&nbsp;&nbsp;Logs</a></li>
                                        <li role="presentation" style="<%=Panels%>"><a href="#consulta4" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexos</a></li>
                                        <li role="presentation"><a href="#consulta5" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-text-size"></span>&nbsp;&nbsp;Observações</a></li>
                                    </ul>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="tab-content" runat="server" id="PanelContext">                                
                                <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-2" style="font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Lançamento</label>
                                                    <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="1" MaxLength="18"/>
                                                </div>
                                                <div class="col-md-2" style=" font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Tipo de Orçamento <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlTipoOrcamento" runat="server" Width="100%" TabIndex="2" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>
                                                <div class="col-md-2" style=" font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Data Emissão</label>
                                                    <asp:TextBox ID="txtdtemissao" CssClass="form-control"  TabIndex="3" runat="server" name="txtdtemissao"  AutoPostBack="true" Enabled="false" MaxLength="15" />
                                                </div>                                   
                                                <div class="col-md-4" style=" font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Empresa <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlEmpresa" runat="server"   TabIndex="4"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_TextChanged"   AutoPostBack="true"/>
                                                </div>
                                                <div class="col-md-2" style="font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Situação</label>
                                                    <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" TabIndex="5" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>  
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">N° Documento</label>
                                                    <asp:TextBox ID="txtNroDocumento" AutoPostBack="False" CssClass="form-control"  TabIndex="6" runat="server" MaxLength="18"/>
                                                </div>
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Série</label>
                                                    <asp:TextBox ID="txtNroSerie" AutoPostBack="False" CssClass="form-control"  TabIndex="7" runat="server" MaxLength="18"/>
                                                </div>
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Data Validade <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:TextBox ID="txtdtValidade" name="txtdtValidade" AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15"/>
                                                </div> 
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Aplicação de uso <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlAplicacaoUso" runat="server" Width="100%" TabIndex="9" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div> 
                                                <div class="col-md-4" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Cobrança <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlTipoCobranca" runat="server" AutoPostBack="False" Width="100%" TabIndex="10" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6" >
                                                    <label for="usr" style="font-size: x-small;margin-top:3px">Cliente <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <div class="input-group " style="width:100%">                              
                                                        <asp:TextBox ID="txtCodPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server" TabIndex="11" OnTextChanged="txtCodPessoa_TextChanged"   AutoPostBack="true" onFocus="this.select()"                                
                                                        Width="15%" MaxLength="6" />                                                      
                                                        <asp:LinkButton ID="btnPessoa" title="Pesquise os Clientes" runat="server" CssClass="form-control btn btn-primary" Width="10%"  OnClick="ConPessoa" AutoPostBack="true"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                        <asp:TextBox ID="txtPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server"  Enabled="false"  Width="75%"  />
                                                    </div>
                                                </div>  
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Número Web </label>
                                                    <asp:TextBox ID="txtNroWeb" CssClass="form-control"  TabIndex="21" runat="server" AutoPostBack="true"  Text="0,00" MaxLength="18" onFocus="this.select()" OnTextChanged="txtNroWeb_TextChanged"/>
                                                </div>
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Condição de Pagamento <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlPagamento" runat="server" AutoPostBack="False" Width="100%" TabIndex="12" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div> 
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6" >
                                                    <label for="usr" style="font-size: x-small;">Transportador <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <div class="input-group " style="width:100%">                              
                                                        <asp:TextBox ID="txtCodTransportador" CssClass="form-control"  runat="server" TabIndex="13" OnTextChanged="txtCodTransportador_TextChanged"   AutoPostBack="true"  onFocus="this.select()"                              
                                                        Width="15%" MaxLength="6" />                                                      
                                                        <asp:LinkButton ID="btnTransportador" title="Pesquise os Transportadores" runat="server" CssClass="form-control btn btn-primary" Width="10%"  OnClick="btnTransportador_Click" AutoPostBack="true"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                        <asp:TextBox ID="txtTransportador" CssClass="form-control" runat="server"  Enabled="false"  Width="75%"  />
                                                    </div>
                                                </div> 
                                                 <div class="col-md-6" style="font-size: x-small;">
                                                    <label for="usr" >Vendedor <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlVendedor" runat="server" AutoPostBack="False" Width="100%" TabIndex="14" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div>
                                                
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12 " style="margin-top:6px">
                                                    <div class="panel panel-primary">
                                                        <div class="panel-heading" style="text-align:center;padding:13px!important;height:45px">
                                                            <div class="col-md-2">
                                                                 <asp:LinkButton ID="btnAddItem" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="15" UseSubmitBehavior="false"  OnClick="btnAddItem_Click" style="float:left!important;margin-top:-7px!important"> 
                                                                    <span aria-hidden="true" class="glyphicon glyphicon-pencil"></span> Editar
                                                                </asp:LinkButton>
                                                            </div>
                                                            <div class="col-md-8">Produtos do Orçamento</div>
                                                          
                                                        </div>
                                                        <div class="panel-body scroll-true" style="padding-left:0px!important;padding-right:0px!important;padding-bottom:0px!important">
                                                            <asp:GridView ID="grdProduto" runat="server" AllowSorting="true"  OnSorting="grdProduto_Sorting"
                                                                    CssClass="table table-hover table-striped"
                                                                    GridLines="None" AutoGenerateColumns="False" PagerSettings-Mode ="NumericFirstLast" OnPageIndexChanging="grdItens_PageIndexChanging" 
                                                                    Font-Size="8pt"  Visible="true" AllowPaging="true" PageSize="10"
                                                                    OnSelectedIndexChanged="grdPagamento_SelectedIndexChanged">
                                                                <PagerStyle HorizontalAlign = "left"   CssClass = "GridPager" />
                                                            <Columns>
                                                                <asp:BoundField DataField="CodigoItem" HeaderText="#"  SortExpression="CodigoItem" />
                                                                <asp:BoundField DataField="CodigoProduto" HeaderText="Código do Produto"  SortExpression="CodigoProduto" />
                                                                <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Descrição"  SortExpression="Cpl_DscProduto" />
                                                                <asp:BoundField DataField="Cpl_DscSituacao" HeaderText="Situação"  SortExpression="Cpl_DscSituacao" />
                                                                <asp:BoundField DataField="Unidade" HeaderText="Unidade"  DataFormatString="{0:F}"  SortExpression="Unidade" />
                                                                <asp:BoundField DataField="Quantidade" HeaderText="Quantidade"  DataFormatString="{0:F}"  SortExpression="Quantidade" />
                                                                <asp:BoundField DataField="PrecoItem" HeaderText="Preço"  DataFormatString="{0:C}"  SortExpression="PrecoItem" />
                                                                <asp:BoundField DataField="ValorDesconto" HeaderText="% Desconto"  DataFormatString="{0:F}%"  SortExpression="ValorDesconto" />
                                                                <asp:BoundField DataField="ValorTotalItem" HeaderText="Valor Total"  DataFormatString="{0:C}"  SortExpression="ValorTotalItem" />

                                                            </Columns>
                                                            <RowStyle CssClass="cursor-pointer" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div> 
                                             <div class="row">
                                                <div class="col-md-12" style="padding:0!important">
                                                    <div class="col-md-3" style="font-size: x-small;">
                                                        <label for="usr" style ="margin-top:1px;">ST </label>
                                                        <asp:TextBox ID="txtST" CssClass="form-control"  TabIndex="16" runat="server" AutoPostBack="true" Text="0,00" Enabled="false" MaxLength="18" />
                                                    </div> 
                                                    <div class="col-md-3" style="font-size: x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cubagem</label>
                                                        <asp:TextBox ID="txtCubagem" CssClass="form-control"  TabIndex="17" runat="server" AutoPostBack="true" Text="0,00" Enabled="false" MaxLength="18"  />
                                                    </div> 
                                                    <div class="col-md-3" style="font-size: x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Peso </label>
                                                        <asp:TextBox ID="txtPeso" CssClass="form-control"  TabIndex="18" runat="server" AutoPostBack="true" Text="0,00" MaxLength="18" Enabled="false"  />
                                                    </div> 
                                                    <div class="col-md-3" style="font-size: x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Desconto Médio </label>
                                                        <asp:TextBox ID="txtDescontoMedio" CssClass="form-control"  TabIndex="19" runat="server" AutoPostBack="true" Text="R$0,00" MaxLength="18" Enabled="false" />
                                                    </div>
                                                                                           
                                                     <div class="col-md-4" style="font-size: x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Comissão</label>
                                                        <asp:TextBox ID="txtComissao" CssClass="form-control"  TabIndex="20" runat="server" AutoPostBack="true" Text="0,00" MaxLength="18" Enabled="false"  />
                                                    </div> 
                                                     <div class="col-md-4" style="font-size: x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Frete</label>
                                                        <asp:TextBox ID="txtFrete" CssClass="form-control"  TabIndex="22" runat="server" AutoPostBack="true" Text="0,00" MaxLength="18" onFocus="this.select()" OnTextChanged="txtFrete_TextChanged"/>
                                                    </div> 
                                                    <div class="col-md-4" style="font-size: small;padding-top:20px;">
                                                        <div class="input-group" title="Valor total do Orçamento">
                                                           <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="border-right:0!important;border:0!important"><b>Total:</b></span>
                                                            <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="font-weight:bold;padding-right:4px!important;font-size:20px;border:0!important;border-right:0!important;">R$</span>
                                                            <asp:TextBox ID="txtVlrTotal"  CssClass="form-control CorPadraoEscolhidaTexto  CorPadraoEscolhida" Text="0,00"  runat="server" MaxLength="50" Enabled="false" style="padding-left:0!important;font-weight:bold;font-size:20px;border:0!important;"  />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCodigo" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCodPessoa" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtPessoa" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtST" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCubagem" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtPeso" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtNroWeb" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtComissao" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>                                
                                <div role="tabpanel" class="tab-pane" id="consulta1" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">CNPJ/CPF</label>
                                                <asp:TextBox ID="txtCNPJCPFCredor" CssClass="form-control" runat="server" Enabled="false" Text =""  />
                                            </div>                               
                                            <div class="col-md-10" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Razão Social</label>
                                                <asp:TextBox ID="txtRazSocial" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>                                    
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Endereço</label>
                                                <asp:TextBox ID="txtEndereco" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>                                    
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">CEP</label>
                                                <asp:TextBox ID="txtCEP" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Estado</label>
                                                <asp:TextBox ID="txtEstado" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Cidade</label>
                                                <asp:TextBox ID="txtCidade" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Bairro</label>
                                                <asp:TextBox ID="txtBairro" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">E-mail</label>
                                                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCNPJCPFCredor" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtRazSocial" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtEndereco" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCEP" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtEstado" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCidade" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtBairro" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta7" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <div class="row" >
                                        <div class="col-md-8">
                                            <div id="chartContainer" style="height: 50px;"></div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="panel panel-primary" >
                                                <div class="panel-heading" style="text-align:center;">
                                                    Valores
                                                </div>
                                                <div class="panel-body" >
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <div class="col-md-12" style="height:10%">
                                                                 <div class="input-group" style="height:80%">
                                                                    <span class="input-group-addon"><b>Crédito:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b></span>
                                                                    <span class="input-group-addon" style="border-right:0;padding-right:0;color:black;font-weight:bold;font-size:20px" >R$</span>
                                                                    <asp:TextBox ID="txtCredito" name="txtCredito" AutoPostBack="False" Height="100%" Enabled="false" CssClass="form-control" TabIndex="8" runat="server" style="border-left:0;color:black;font-weight:bold;padding-left:5px;font-size:20px"/>
                                                                </div>                                                
                                                            </div> 
                                                            <div class="col-md-12" style="height:10%">
                                                                <div class="input-group" style="height:80%">
                                                                    <span class="input-group-addon"><b>Utilizado:&nbsp;&nbsp;&nbsp;</b></span>
                                                                    <span class="input-group-addon" style="border-right:0;padding-right:0;color:black;font-weight:bold;font-size:20px" >R$</span>
                                                                    <asp:TextBox ID="txtVlrUsado" name="txtVlrUsado" AutoPostBack="False" Enabled="false" Height="100%"  CssClass="form-control" TabIndex="8" runat="server" style="border-left:0;color:black;font-weight:bold;padding-left:5px;font-size:20px" />
                                                                </div>                                                  
                                                            </div>
                                                            <br /><br />
                                                            <div class="col-md-12" style="height:10%">
                                                                <div class="input-group" style="height:80%">
                                                                    <span class="input-group-addon" ><b>Disponível:</b></span>
                                                                    <span class="input-group-addon" style="border-right:0;padding-right:0;color:black;font-weight:bold;font-size:20px" >R$</span>
                                                                    <asp:TextBox ID="txtVlrDisponivel" name="txtVlrDisponivel" Enabled="false" AutoPostBack="False" Height="100%"  CssClass="form-control" TabIndex="8" runat="server" style="border-left:0;color:black;font-weight:bold;padding-left:5px;font-size:20px" />
                                                                </div>                                                 
                                                            </div>
                                                        </ContentTemplate> 
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div> 
                                    </div> 
                                    <div class="row">
                                        <div class="col-md-12">
                                             <div class="panel panel-primary">
                                                <div class="panel-heading" style="text-align:center;">
                                                    Detalhes de pagamento
                                                </div>
                                                <div class="panel-body" style="padding-left:0px!important;padding-right:0px!important;padding-bottom:0px!important">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="grdFinanceiro" runat="server"
                                                                    CssClass="table  table-hover table-striped"
                                                                    GridLines="None" AutoGenerateColumns="False" PagerSettings-Mode ="NumericFirstLast" OnPageIndexChanging="grdFinanceiro_PageIndexChanging" 
                                                                    Font-Size="8pt" Visible="true" AllowPaging="true" PageSize="10"
                                                                    OnSelectedIndexChanged="grdFinanceiro_SelectedIndexChanged">
                                                                <PagerStyle HorizontalAlign = "left"   CssClass = "GridPager" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="DGDocumento" HeaderText="Documento" />
                                                                    <asp:BoundField DataField="DataVencimento" HeaderText="Data Vencimento" />
                                                                    <asp:BoundField DataField="DataEmissao" HeaderText="Data Emissão"  />
                                                                    <asp:BoundField DataField="ValorGeral" HeaderText="Unidade"  DataFormatString="{0:C}"/>
                                                                </Columns>
                                                                <RowStyle CssClass="cursor-pointer" />
                                                            </asp:GridView>
                                                        </ContentTemplate> 
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>    
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta8" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div id="chartContainer2" style="height: 450px;"></div>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
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
                                <div role="tabpanel" class="tab-pane" id="consulta3" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdLogDocumento" runat="server" CssClass="table table-bordered table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  
                                                OnSelectedIndexChanged="grdLogDocumento_SelectedIndexChanged"

                                                PageSize="10" AllowPaging="true"
                                                OnPageIndexChanging="grdLogDocumento_PageIndexChanging"
                                                PagerSettings-Mode ="NumericFirstLast" >
                                                <PagerStyle HorizontalAlign = "Right"   CssClass = "GridPager" />
                                                <Columns>
                                                    <asp:BoundField DataField="DataHora" HeaderText="Data" />
                                                    <asp:BoundField DataField="EstacaoNome" HeaderText="Máquina" />
                                                    <asp:BoundField DataField="UsuarioNome" HeaderText="Usuário" />
                                                    <asp:BoundField DataField="Cpl_DescricaoOperacao" HeaderText="Operação" />
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
                                <div role="tabpanel" class="tab-pane" id="consulta4" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btnNovoAnexo" runat="server" style="padding-left:10px; margin-bottom:10px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovoAnexo_Click" TabIndex="21"> 
                                                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span> Novo Anexo
                                            </asp:LinkButton>
                                            <br />
                                            <asp:GridView ID="grdAnexo" runat="server"
                                                CssClass="table  table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  Visible="true"
                                                OnSelectedIndexChanged="grdAnexo_SelectedIndexChanged">
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
                                <div role="tabpanel" class="tab-pane" id="consulta5" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                             <div class="row">
                                                <div class="col-md-12" style="font-size: x-small;margin-top:0px">
                                                    <label for="usr" style ="margin-top:1px;">Observação</label>
                                                    <asp:TextBox ID="txtDescricao" AutoPostBack="False" CssClass="form-control" MaxLength="300" TabIndex="23" runat="server" onFocus="this.select()" />
                                                </div>
                                            </div>
                                        </ContentTemplate> 
                                        <Triggers>
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
                <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H3">Atenção</h4>
                        </div>
                        <div class="modal-body">
                            Deseja Excluir este documento?
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:LinkButton>
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
</asp:Content>
