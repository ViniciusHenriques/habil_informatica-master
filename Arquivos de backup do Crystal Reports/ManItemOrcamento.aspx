<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManItemOrcamento.aspx.cs" Inherits="SoftHabilInformatica.Pages.Vendas.ManItemOrcamento" 
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server"  >

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jquery.canvasjs.min.js"></script>
    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            $('.js-example-basic-single').select2({});            
        });


        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});
        }

        function Departamento(ValoresDep6, Cor){
            CanvasJS.addColorSet("greenShades",[Cor,"#B22222"]);
            var consumo = {
	            colorSet: "greenShades",
                animationEnabled: true,               
                height:160,
	            data: [{
		            type: "area",
		            indexLabelFontSize: 12,
		            radius: 160,
		            indexLabel: "{y}",
		            yValueFormatString: "R$ ###0.0\"\"",
		            click: explodePie,
		            dataPoints: [
                        { y: ValoresDep6[0], label: '<%=MesesAnteriores[0].ToString()%>'}, // "Nov;Dez;Jan;Fev;Mar;Abr"
                        { y: ValoresDep6[1], label: '<%=MesesAnteriores[1].ToString()%>'},
                        { y: ValoresDep6[2], label: '<%=MesesAnteriores[2].ToString()%>'},
                        { y: ValoresDep6[3], label: '<%=MesesAnteriores[3].ToString()%>'},
                        { y: ValoresDep6[4], label: '<%=MesesAnteriores[4].ToString()%>'},
                        { y: ValoresDep6[5], label: '<%=MesesAnteriores[5].ToString()%>'},

		            ]
	            }]
            };
            function explodePie(e) {
	            for(var i = 0; i < e.dataSeries.dataPoints.length; i++) {
		            if(i !== e.dataPointIndex)
			            e.dataSeries.dataPoints[i].exploded = false;
	            }
             }   
            $("#chartContainer").CanvasJSChart(consumo);
            chart.render();
        }
       
        window.onload = function () {
            Departamento([<%=ValoresDep1[0]%>,<%=ValoresDep1[1]%>,<%=ValoresDep1[2]%>,<%=ValoresDep1[3]%>,<%=ValoresDep1[4]%>,<%=ValoresDep1[5]%>],'#000000');           
        }
        
    </script>
    <style>
        @media screen and (max-width: 1000px) {
            .noprint {display: none;}
            .scroll-true{overflow-y:scroll!important;}
        }
        .NoPaddingMarginRight{padding:0!important;margin:0!important;}
        .padding-top-15{padding-top:15px!important;}
        .text-align-center{text-align:center;}
        .padding-top-3{padding-top:5px!important;}
        .no-print{display:none;}
    </style>
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary">
            <div class="panel-heading" >
                Manutenção dos Itens do Orçamento
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
                            <div class="col-md-9 col-xs-4" style="background-color: white; border: none; text-align: left; font-size: small;">
                                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                </asp:LinkButton>
                            </div>
                            <div class="col-md-3 col-xs-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                <div class="input-group" style="margin:0!important;width:80%;float:right" title="Valor total do Orçamento">
                                   <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="border-right:0!important;border:1px solid black!important"><b>Total:</b></span>
                                    <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="font-weight:bold;padding-right:4px!important;font-size:20px;border:1px solid black!important;border-right:0!important;" >R$</span>
                                    <asp:TextBox ID="txtVlrTotal"  CssClass="form-control CorPadraoEscolhidaTexto  CorPadraoEscolhida" Text="0,00"  name="txtPreco" TabIndex="21" runat="server" MaxLength="50" Enabled="false" style="padding-left:0!important;font-weight:bold;font-size:20px;height:36px;border:1px solid black!important;border-left:0!important;"  />
                                </div>
                            </div>
                        </ContentTemplate> 
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnVoltar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="row" style="background-color: white; border: none; margin-top:10px!important">
                    <asp:HiddenField ID="TabName" runat="server"/>            
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                            <ContentTemplate>                                       
                                <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; margin-top:5px">                                                                                                 
                                    <div class="panel panel-default" >
                                        <div class="panel-heading" style="padding-left:25px" >
                                            Itens do Orçamento       
                                        </div>
                                        <div class="panel-body">  
                                            <div class="row" style="margin-bottom:15px">                                                                
                                                <div class="col-md-1 " style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <label for="usr"> Produto <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:TextBox ID="txtCodItem"  CssClass="form-control" name="txtCodItem" TabIndex="1" runat="server"  MaxLength="50" OnTextChanged="txtCodItem_TextChanged"  AutoPostBack="true" />
                                                    </div>
                                                    <div class="col-md-1 " style="background-color: white; border: none; font-size: x-small;margin-top:18px;">
                                                    <asp:LinkButton ID="btnPesquisarItem" runat="server" title="Pesquisar produtos" TabIndex="20" CssClass="btn btn-primary" Width="100%"  OnClick="btnPesquisarItem_Click" style="height:35px!important;padding-top:7px"> 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>                                                                                                              
                                                </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <label for="usr">Descrição </label>
                                                    <asp:TextBox ID="txtDescricao"  CssClass="form-control" name="txtDescricao"  runat="server" MaxLength="50" Enabled="false"  AutoPostBack="true" />
                                                </div>
                                                <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <label for="usr">Unidade</label>
                                                    <asp:DropDownList ID="ddlUnidade" runat="server"   Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" Enabled="false"/>
                                                </div>
                                                <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <label for="usr">Qtde.<span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:TextBox ID="txtQtde"  CssClass="form-control" name="txtQtde" TabIndex="3" runat="server" Text="0,00" MaxLength="18" onFocus="this.select()" OnTextChanged="txtQtde_TextChanged" AutoPostBack="true"/>
                                                </div>
                                                <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small; " >
                                                    <label for="usr">Preço <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:TextBox ID="txtPreco" Text="0,00" CssClass="form-control" name="txtPreco" TabIndex="4" runat="server" MaxLength="18" onFocus="this.select()" OnTextChanged="txtPreco_TextChanged" AutoPostBack="true"/>
                                                </div>
                                                 <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;" >
                                                    <label for="usr">% Desconto <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:TextBox ID="txtDesconto" Text="0,00" CssClass="form-control" TabIndex="5" runat="server" MaxLength="18" onFocus="this.select()" OnTextChanged="txtDesconto_TextChanged" AutoPostBack="true"/>
                                                </div>
                                                <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small; " >
                                                    <label for="usr">Valor Total</label>
                                                    <asp:TextBox ID="txtVlrTotalItem" Text="0,00" CssClass="form-control" TabIndex="4" runat="server" MaxLength="18"  Enabled="false"/>
                                                </div>
                                                <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:19px;padding-right:0!important">
                                                    <asp:LinkButton ID="BtnAddProduto" runat="server" title="Adicionar/Alterar Produto" TabIndex="6" CssClass="btn btn-success"  OnClick="BtnAddProduto_Click" style="height:33px!important;padding-top:7px" > 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="BtnExcluirProduto" runat="server" Text="Adicionar" title="Excluir Produto" TabIndex="7" CssClass="btn btn-danger"  OnClick="BtnExcluirProduto_Click" style="height:33px!important;padding-top:7px"> 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                    </asp:LinkButton>                                                                                                               
                                                </div> 
                                            </div>       
                                            
                                            <asp:GridView ID="grdProduto" runat="server" TabIndex="7"
                                                    Width="100%" PagerSettings-Mode ="NumericFirstLast" OnPageIndexChanging="grdProduto_PageIndexChanging" 
                                                    CssClass="table table-hover table-striped" AllowSorting="true"  
                                                    GridLines="None" AutoGenerateColumns="False" AllowPaging="true" PageSize="5"
                                                    Font-Size="8pt" 
                                                    OnSelectedIndexChanged="grdProduto_SelectedIndexChanged"
                                                    OnSorting="grdProduto_Sorting"
                                                    SortedAscendingHeaderStyle-CssClass=".ascending" SortedDescendingHeaderStyle-CssClass =".descending"
                                                    CurrentSortDirection="ASC">
                                                <PagerStyle HorizontalAlign = "left"   CssClass = "GridPager" />
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoItem" HeaderText="#"  SortExpression="CodigoItem" />
                                                    <asp:BoundField DataField="CodigoProduto" HeaderText="Código do Produto"  SortExpression="CodigoProduto" />
                                                    <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Descrição"  SortExpression="Cpl_DscProduto" />
                                                    <asp:BoundField DataField="Cpl_DscSituacao" HeaderText="Situação"  SortExpression="Cpl_DscSituacao" />
                                                    <asp:BoundField DataField="Unidade" HeaderText="Unidade"  DataFormatString="{0:F}"  SortExpression="Unidade" />
                                                    <asp:BoundField DataField="Quantidade" HeaderText="Quantidade"  DataFormatString="{0:F}"  SortExpression="Quantidade" />
                                                    <asp:BoundField DataField="QuantidadeAtendida" HeaderText="Qtda. Atendida"  DataFormatString="{0:F}"  SortExpression="QuantidadeAtendida" />
                                                    <asp:BoundField DataField="PrecoItem" HeaderText="Preço"  DataFormatString="{0:C}"  SortExpression="PrecoItem" />
                                                    <asp:BoundField DataField="ValorDesconto" HeaderText="% Desconto"  DataFormatString="{0:F}%"  SortExpression="ValorDesconto"/>
                                                    <asp:BoundField DataField="ValorTotalItem" HeaderText="Valor Total"  DataFormatString="{0:C}"  SortExpression="ValorTotalItem" />
                                                    
                                                                                                                      
                                                    <asp:CommandField HeaderText="Editar" ShowSelectButton="True" 
                                                            ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                            ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                                            ControlStyle-Width ="25px" ControlStyle-Height ="25px" />                                                                     
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtDescricao"  />
                                <asp:AsyncPostBackTrigger ControlID="txtCodItem"  EventName="TextChanged"/>
                                <asp:AsyncPostBackTrigger ControlID="txtQtde" EventName="TextChanged"/>
                                <asp:AsyncPostBackTrigger ControlID="ddlUnidade" EventName="TextChanged"/>
                                <asp:AsyncPostBackTrigger ControlID="txtPreco" EventName="TextChanged"/>
                                <asp:AsyncPostBackTrigger ControlID="grdProduto"  EventName="SelectedIndexChanged"/>
                                <asp:AsyncPostBackTrigger ControlID="BtnAddProduto"  EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="BtnExcluirProduto"  EventName="Click"/>
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; margin-top:5px">                                                                                                 
                            <div class="panel panel-default" >
                                <div class="panel-heading" style="padding-left:25px" >
                                    Sugestões de Compra <asp:label runat="server" ID="lblProjecao"></asp:label>
                                </div>
                                <div class="panel-body">  
                                    <div class="row">
                                        <div class="col-md-1 " style="margin-bottom:20px;background-color: white; border: none; text-align: left; font-size: small;">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                <ContentTemplate> 
                                                    <asp:LinkButton ID="btnAddBIConsumoClienteProduto" runat="server"  CssClass="btn btn-success" UseSubmitBehavior="false"  OnClick="btnAddBIConsumoClienteProduto_Click" Style="height:60px;width:100%;margin-top:13px;font-size:25px;padding-top:5px" title="Adicionar item escolhidos no Orçamento novo"> 
                                                        <img src="../../images/arrow.gif" width="30" height="50" />
                                                    </asp:LinkButton>
                                                    </ContentTemplate> 
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddBIConsumoClienteProduto"  EventName="Click"/>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-11">
                                            <div id="chartContainer" style="height: 160px;"></div>
                                        </div>
                                        <div class="col-md-12" style="margin-top:20px">
                                            <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;" >
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                                    <ContentTemplate> 
                                                        <ul class="nav nav-tabs " role="tablist" id="myTabs" >
                                                            <li role="presentation" onclick="Departamento([<%=ValoresDep1[0]%>,<%=ValoresDep1[1]%>,<%=ValoresDep1[2]%>,<%=ValoresDep1[3]%>,<%=ValoresDep1[4]%>,<%=ValoresDep1[5]%>],'#000000')"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select">&nbsp;&nbsp;<%=Departamentos[0].ToString() %></a></li>
                                                            <li role="presentation" onclick="Departamento([<%=ValoresDep2[0]%>,<%=ValoresDep2[1]%>,<%=ValoresDep2[2]%>,<%=ValoresDep2[3]%>,<%=ValoresDep2[4]%>,<%=ValoresDep2[5]%>],'#87CEFA')"><a href="#dep2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select">&nbsp;&nbsp;<%=Departamentos[1].ToString() %></a></li>
                                                            <li role="presentation" onclick="Departamento([<%=ValoresDep3[0]%>,<%=ValoresDep3[1]%>,<%=ValoresDep3[2]%>,<%=ValoresDep3[3]%>,<%=ValoresDep3[4]%>,<%=ValoresDep3[5]%>],'#6B8E23')"><a href="#dep3" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select">&nbsp;&nbsp;<%=Departamentos[2].ToString() %></a></li>
                                                            <li role="presentation" onclick="Departamento([<%=ValoresDep4[0]%>,<%=ValoresDep4[1]%>,<%=ValoresDep4[2]%>,<%=ValoresDep4[3]%>,<%=ValoresDep4[4]%>,<%=ValoresDep4[5]%>],'#B22222')"><a href="#dep4" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select">&nbsp;&nbsp;<%=Departamentos[3].ToString() %></a></li>
                                                            <li role="presentation" onclick="Departamento([<%=ValoresDep5[0]%>,<%=ValoresDep5[1]%>,<%=ValoresDep5[2]%>,<%=ValoresDep5[3]%>,<%=ValoresDep5[4]%>,<%=ValoresDep5[5]%>],'#FF8C00')"><a href="#dep5" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select">&nbsp;&nbsp;<%=Departamentos[4].ToString() %></a></li>
                                                            <li role="presentation" onclick="Departamento([<%=ValoresDep6[0]%>,<%=ValoresDep6[1]%>,<%=ValoresDep6[2]%>,<%=ValoresDep6[3]%>,<%=ValoresDep6[4]%>,<%=ValoresDep6[5]%>],'#BDB76B')"><a href="#dep6" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select">&nbsp;&nbsp;<%=Departamentos[5].ToString() %></a></li>
                                                        </ul>
                                                        </ContentTemplate> 
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdBIConsumoPessoaProduto"  EventName="SelectedIndexChanged"/>
                                                    </Triggers> 
                                                </asp:UpdatePanel>  
                                                <div class="tab-content" runat="server" id="PanelContext">                                
                                                    <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                                        <div class="col-md-12 scroll-true" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                <ContentTemplate> 
                                                                    <asp:GridView ID="grdBIConsumoPessoaProduto" runat="server" Width="100%" 
                                                                            CssClass ="table table-hover table-striped" 
                                                                            GridLines="None" AutoGenerateColumns="false" 
                                                                            Font-Size="8pt"  AllowSorting ="true" 
                                                                            OnSorting="grdBIConsumoPessoaProduto_Sorting"
                                                                            SortedAscendingHeaderStyle-CssClass=".ascending" SortedDescendingHeaderStyle-CssClass =".descending"
                                                                            CurrentSortDirection="ASC">
                                                                        <PagerStyle HorizontalAlign="left"   CssClass = "GridPager" />
                                                                        <Columns>    
                                                                            
                                                                            <asp:BoundField DataField="CodigoIndex" HeaderText="#" SortExpression="CodigoIndex" ItemStyle-CssClass="no-print" HeaderStyle-CssClass="no-print" />
                                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Produto"  SortExpression="CodigoProduto" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição do Produto"  SortExpression="DescricaoProduto" ItemStyle-CssClass="padding-top-15"/> 
                                                                            <asp:BoundField DataField="strCpl_Mes1" HeaderText=""  SortExpression="strCpl_Mes1" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes2" HeaderText="" SortExpression="strCpl_Mes2" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes3" HeaderText="" SortExpression="strCpl_Mes3" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes4" HeaderText="" SortExpression="strCpl_Mes4" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes5" HeaderText="" SortExpression="strCpl_Mes5" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes6" HeaderText="" SortExpression="strCpl_Mes6" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>                                    
                                                                            <asp:BoundField DataField="NumeroProjecao" HeaderText="Estoque" DataFormatString="{0:G2}"  SortExpression="NumeroProjecao" ItemStyle-CssClass="padding-top-15"/>
                                                                            <asp:BoundField DataField="QuantidadeMedia" HeaderText="Qtde. Média" DataFormatString="{0:F}"  SortExpression="QuantidadeMedia" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="PrecoVenda" HeaderText="Preço Venda" DataFormatString="{0:F}"  SortExpression="PrecoVenda" ItemStyle-CssClass="padding-top-15" />
                                                                           
                                                                            <asp:TemplateField HeaderText="Qtde. Comprar"  ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="text-align-center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtQtdaComprarDep1" CssClass="form-control"  runat="server" Text='<%# Eval("QuantidadeComprar").ToString() %>' OnTextChanged="txtQtdaComprarDep1_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="cursor-pointer" />
                                                                    </asp:GridView>
                                                                </ContentTemplate> 
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="grdBIConsumoPessoaProduto"  EventName="SelectedIndexChanged"/>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div role="tabpanel" class="tab-pane" id="dep2" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                                         <div class="col-md-12 scroll-true" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                <ContentTemplate> 
                                                                    <asp:GridView ID="grdBIConsumoPessoaProduto2" runat="server" Width="100%" 
                                                                            CssClass ="table table-hover table-striped" 
                                                                            GridLines="None" AutoGenerateColumns="false" 
                                                                            Font-Size="8pt"  AllowSorting ="true" 
                                                                            OnSorting="grdBIConsumoPessoaProduto_Sorting"
                                                                            SortedAscendingHeaderStyle-CssClass=".ascending" SortedDescendingHeaderStyle-CssClass =".descending"
                                                                            CurrentSortDirection="ASC">
                                                                        <PagerStyle HorizontalAlign="left"   CssClass = "GridPager" />
                                                                        <Columns>    
                                                                            
                                                                            <asp:BoundField DataField="CodigoIndex" HeaderText="#" SortExpression="CodigoIndex" ItemStyle-CssClass="no-print" HeaderStyle-CssClass="no-print" />
                                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Produto"  SortExpression="CodigoProduto" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição do Produto"  SortExpression="DescricaoProduto" ItemStyle-CssClass="padding-top-15" /> 
                                                                            <asp:BoundField DataField="strCpl_Mes1" HeaderText=""  SortExpression="strCpl_Mes1" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes2" HeaderText="" SortExpression="strCpl_Mes2" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes3" HeaderText="" SortExpression="strCpl_Mes3" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes4" HeaderText="" SortExpression="strCpl_Mes4" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes5" HeaderText="" SortExpression="strCpl_Mes5" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes6" HeaderText="" SortExpression="strCpl_Mes6" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>                                    
                                                                            <asp:BoundField DataField="NumeroProjecao" HeaderText="Estoque" DataFormatString="{0:G2}"  SortExpression="NumeroProjecao" ItemStyle-CssClass="padding-top-15"/>
                                                                            <asp:BoundField DataField="QuantidadeMedia" HeaderText="Qtde. Média" DataFormatString="{0:F}"  SortExpression="QuantidadeMedia" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="PrecoVenda" HeaderText="Preço Venda" DataFormatString="{0:F}"  SortExpression="PrecoVenda" ItemStyle-CssClass="padding-top-15" />
                                                                           
                                                                            <asp:TemplateField HeaderText="Qtde. Comprar"  ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="text-align-center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtQtdaComprarDep2" CssClass="form-control"  runat="server" Text='<%# Eval("QuantidadeComprar").ToString() %>' OnTextChanged="txtQtdaComprarDep2_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="cursor-pointer" />
                                                                    </asp:GridView>
                                                                </ContentTemplate> 
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="grdBIConsumoPessoaProduto2"  EventName="SelectedIndexChanged"/>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div role="tabpanel" class="tab-pane" id="dep3" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                                         <div class="col-md-12 scroll-true" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                                                <ContentTemplate> 
                                                                    <asp:GridView ID="grdBIConsumoPessoaProduto3" runat="server" Width="100%" 
                                                                            CssClass ="table table-hover table-striped" 
                                                                            GridLines="None" AutoGenerateColumns="false" 
                                                                            Font-Size="8pt"  AllowSorting ="true" 
                                                                            OnSorting="grdBIConsumoPessoaProduto_Sorting"
                                                                            SortedAscendingHeaderStyle-CssClass=".ascending" SortedDescendingHeaderStyle-CssClass =".descending"
                                                                            CurrentSortDirection="ASC">
                                                                        <PagerStyle HorizontalAlign="left"   CssClass = "GridPager" />
                                                                        <Columns>    
                                                                            
                                                                            <asp:BoundField DataField="CodigoIndex" HeaderText="#" SortExpression="CodigoIndex" ItemStyle-CssClass="no-print" HeaderStyle-CssClass="no-print" />
                                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Produto"  SortExpression="CodigoProduto" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição do Produto"  SortExpression="DescricaoProduto" ItemStyle-CssClass="padding-top-15"/> 
                                                                            <asp:BoundField DataField="strCpl_Mes1" HeaderText=""  SortExpression="strCpl_Mes1" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes2" HeaderText="" SortExpression="strCpl_Mes2" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes3" HeaderText="" SortExpression="strCpl_Mes3" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes4" HeaderText="" SortExpression="strCpl_Mes4" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes5" HeaderText="" SortExpression="strCpl_Mes5" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes6" HeaderText="" SortExpression="strCpl_Mes6" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>                                    
                                                                            <asp:BoundField DataField="NumeroProjecao" HeaderText="Estoque" DataFormatString="{0:G2}"  SortExpression="NumeroProjecao" ItemStyle-CssClass="padding-top-15"/>
                                                                            <asp:BoundField DataField="QuantidadeMedia" HeaderText="Qtde. Média" DataFormatString="{0:F}"  SortExpression="QuantidadeMedia" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="PrecoVenda" HeaderText="Preço Venda" DataFormatString="{0:F}"  SortExpression="PrecoVenda" ItemStyle-CssClass="padding-top-15" />
                                                                           
                                                                            <asp:TemplateField HeaderText="Qtde. Comprar"  ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="text-align-center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtQtdaComprarDep3" CssClass="form-control"  runat="server" Text='<%# Eval("QuantidadeComprar").ToString() %>' OnTextChanged="txtQtdaComprarDep3_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="cursor-pointer" />
                                                                    </asp:GridView>
                                                                </ContentTemplate> 
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="grdBIConsumoPessoaProduto3"  EventName="SelectedIndexChanged"/>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div role="tabpanel" class="tab-pane" id="dep4" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                                         <div class="col-md-12 scroll-true" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                                                <ContentTemplate> 
                                                                    <asp:GridView ID="grdBIConsumoPessoaProduto4" runat="server" Width="100%" 
                                                                            CssClass ="table table-hover table-striped" 
                                                                            GridLines="None" AutoGenerateColumns="false" 
                                                                            Font-Size="8pt"  AllowSorting ="true" 
                                                                            OnSorting="grdBIConsumoPessoaProduto_Sorting"
                                                                            SortedAscendingHeaderStyle-CssClass=".ascending" SortedDescendingHeaderStyle-CssClass =".descending"
                                                                            CurrentSortDirection="ASC">
                                                                        <PagerStyle HorizontalAlign="left"   CssClass = "GridPager" />
                                                                        <Columns>    
                                                                            <asp:BoundField DataField="CodigoIndex" HeaderText="#" SortExpression="CodigoIndex" ItemStyle-CssClass="no-print" HeaderStyle-CssClass="no-print" />
                                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Produto"  SortExpression="CodigoProduto" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição do Produto"  SortExpression="DescricaoProduto" ItemStyle-CssClass="padding-top-15"/> 
                                                                            <asp:BoundField DataField="strCpl_Mes1" HeaderText=""  SortExpression="strCpl_Mes1" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes2" HeaderText="" SortExpression="strCpl_Mes2" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes3" HeaderText="" SortExpression="strCpl_Mes3" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes4" HeaderText="" SortExpression="strCpl_Mes4" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes5" HeaderText="" SortExpression="strCpl_Mes5" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes6" HeaderText="" SortExpression="strCpl_Mes6" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>                                    
                                                                            <asp:BoundField DataField="NumeroProjecao" HeaderText="Estoque" DataFormatString="{0:G2}"  SortExpression="NumeroProjecao" ItemStyle-CssClass="padding-top-15"/>
                                                                            <asp:BoundField DataField="QuantidadeMedia" HeaderText="Qtde. Média" DataFormatString="{0:F}"  SortExpression="QuantidadeMedia" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="PrecoVenda" HeaderText="Preço Venda" DataFormatString="{0:F}"  SortExpression="PrecoVenda" ItemStyle-CssClass="padding-top-15" />
                                                                           
                                                                            <asp:TemplateField HeaderText="Qtde. Comprar"  ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="text-align-center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtQtdaComprarDep4" CssClass="form-control"  runat="server" Text='<%# Eval("QuantidadeComprar").ToString() %>' OnTextChanged="txtQtdaComprarDep4_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="cursor-pointer" />
                                                                    </asp:GridView>
                                                                </ContentTemplate> 
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="grdBIConsumoPessoaProduto4"  EventName="SelectedIndexChanged"/>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div role="tabpanel" class="tab-pane" id="dep5" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                                         <div class="col-md-12 scroll-true" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                                                <ContentTemplate> 
                                                                    <asp:GridView ID="grdBIConsumoPessoaProduto5" runat="server" Width="100%" 
                                                                            CssClass ="table table-hover table-striped" 
                                                                            GridLines="None" AutoGenerateColumns="false" 
                                                                            Font-Size="8pt"  AllowSorting ="true" 
                                                                            OnSorting="grdBIConsumoPessoaProduto_Sorting"
                                                                            SortedAscendingHeaderStyle-CssClass=".ascending" SortedDescendingHeaderStyle-CssClass =".descending"
                                                                            CurrentSortDirection="ASC">
                                                                        <PagerStyle HorizontalAlign="left"   CssClass = "GridPager" />
                                                                        <Columns>    
                                                                            
                                                                            <asp:BoundField DataField="CodigoIndex" HeaderText="#" SortExpression="CodigoIndex" ItemStyle-CssClass="no-print" HeaderStyle-CssClass="no-print" />
                                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Produto"  SortExpression="CodigoProduto" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição do Produto"  SortExpression="DescricaoProduto" ItemStyle-CssClass="padding-top-15"/> 
                                                                            <asp:BoundField DataField="strCpl_Mes1" HeaderText=""  SortExpression="strCpl_Mes1" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes2" HeaderText="" SortExpression="strCpl_Mes2" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes3" HeaderText="" SortExpression="strCpl_Mes3" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes4" HeaderText="" SortExpression="strCpl_Mes4" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes5" HeaderText="" SortExpression="strCpl_Mes5" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes6" HeaderText="" SortExpression="strCpl_Mes6" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>                                    
                                                                            <asp:BoundField DataField="NumeroProjecao" HeaderText="Estoque" DataFormatString="{0:G2}"  SortExpression="NumeroProjecao" ItemStyle-CssClass="padding-top-15"/>
                                                                            <asp:BoundField DataField="QuantidadeMedia" HeaderText="Qtde. Média" DataFormatString="{0:F}"  SortExpression="QuantidadeMedia" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="PrecoVenda" HeaderText="Preço Venda" DataFormatString="{0:F}"  SortExpression="PrecoVenda" ItemStyle-CssClass="padding-top-15" />
                                                                           
                                                                            <asp:TemplateField HeaderText="Qtde. Comprar"  ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="text-align-center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtQtdaComprarDep5" CssClass="form-control"  runat="server" Text='<%# Eval("QuantidadeComprar").ToString() %>' OnTextChanged="txtQtdaComprarDep5_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="cursor-pointer" />
                                                                    </asp:GridView>
                                                                </ContentTemplate> 
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="grdBIConsumoPessoaProduto5"  EventName="SelectedIndexChanged"/>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div role="tabpanel" class="tab-pane" id="dep6" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                                         <div class="col-md-12 scroll-true" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                                                <ContentTemplate> 
                                                                    <asp:GridView ID="grdBIConsumoPessoaProduto6" runat="server" Width="100%" 
                                                                            CssClass ="table table-hover table-striped" 
                                                                            GridLines="None" AutoGenerateColumns="false" 
                                                                            Font-Size="8pt"  AllowSorting ="true" 
                                                                            OnSorting="grdBIConsumoPessoaProduto_Sorting"
                                                                            SortedAscendingHeaderStyle-CssClass=".ascending" SortedDescendingHeaderStyle-CssClass =".descending"
                                                                            CurrentSortDirection="ASC">
                                                                        <PagerStyle HorizontalAlign="left"   CssClass = "GridPager" />
                                                                        <Columns>    
                                                                            
                                                                            <asp:BoundField DataField="CodigoIndex" HeaderText="#" SortExpression="CodigoIndex" ItemStyle-CssClass="no-print" HeaderStyle-CssClass="no-print" />
                                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Produto"  SortExpression="CodigoProduto" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição do Produto"  SortExpression="DescricaoProduto" ItemStyle-CssClass="padding-top-15"/> 
                                                                            <asp:BoundField DataField="strCpl_Mes1" HeaderText=""  SortExpression="strCpl_Mes1" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes2" HeaderText="" SortExpression="strCpl_Mes2" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes3" HeaderText="" SortExpression="strCpl_Mes3" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes4" HeaderText="" SortExpression="strCpl_Mes4" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes5" HeaderText="" SortExpression="strCpl_Mes5" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>
                                                                            <asp:BoundField DataField="strCpl_Mes6" HeaderText="" SortExpression="strCpl_Mes6" ItemStyle-CssClass="padding-top-15 text-align-center"  HeaderStyle-CssClass="text-align-center"/>                                    
                                                                            <asp:BoundField DataField="NumeroProjecao" HeaderText="Estoque" DataFormatString="{0:G2}"  SortExpression="NumeroProjecao" ItemStyle-CssClass="padding-top-15"/>
                                                                            <asp:BoundField DataField="QuantidadeMedia" HeaderText="Qtde. Média" DataFormatString="{0:F}"  SortExpression="QuantidadeMedia" ItemStyle-CssClass="padding-top-15" />
                                                                            <asp:BoundField DataField="PrecoVenda" HeaderText="Preço Venda" DataFormatString="{0:F}"  SortExpression="PrecoVenda" ItemStyle-CssClass="padding-top-15" />
                                                                           
                                                                            <asp:TemplateField HeaderText="Qtde. Comprar"  ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="text-align-center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtQtdaComprarDep6" CssClass="form-control"  runat="server" Text='<%# Eval("QuantidadeComprar").ToString() %>' OnTextChanged="txtQtdaComprarDep6_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="cursor-pointer" />
                                                                    </asp:GridView>
                                                                </ContentTemplate> 
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="grdBIConsumoPessoaProduto6"  EventName="SelectedIndexChanged"/>
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
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
