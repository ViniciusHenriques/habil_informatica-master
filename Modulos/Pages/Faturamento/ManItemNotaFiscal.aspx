<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManItemNotaFiscal.aspx.cs" Inherits="SoftHabilInformatica.Pages.Faturamento.ManItemNotaFiscal" 
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server"  >
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>

    <script src="../../Scripts/jquery.canvasjs.min.js"></script>
    <script>
        item = '#home';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });

        
        $(document).on('keydown', function (e) {

             if ((e.altKey) && (e.which === 86)) { 
                $get('<%=btnVoltar.ClientID%>').click(); 
            }
        });
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
                Manutenção dos Itens da Nota Fiscal
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
                                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" OnClick="btnVoltar_Click"> 
                                    <span aria-hidden="true" title="Voltar ( Alt + V ou F5 )" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                </asp:LinkButton>
                            </div>
                            <div class="col-md-3 col-xs-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                <div class="input-group" style="margin:0!important;width:80%;float:right" title="Valor total do Orçamento">
                                   <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="border-right:0!important;border:0!important"><b>Total:</b></span>
                                    <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="font-weight:bold;padding-right:4px!important;font-size:20px;border:0!important;border-right:0!important;" >R$</span>
                                    <asp:TextBox ID="txtVlrTotal"  CssClass="form-control CorPadraoEscolhidaTexto  CorPadraoEscolhida" Text="0,00"  name="txtPreco" TabIndex="21" runat="server" MaxLength="50" Enabled="false" style="padding-left:0!important;font-weight:bold;font-size:20px;height:36px;border:0;border-left:0!important;"  />
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
                                                    <asp:DropDownList ID="ddlUnidade" runat="server"   Width="100%" CssClass="form-control" Font-Size="Small" Enabled="false"/>
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
                                                    <asp:Button runat="server" id="BtnAddProduto" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="BtnAddProduto_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar/Alterar produto" TabIndex="5"/>
                                                    <asp:Button runat="server" id="BtnExcluirProduto" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="BtnExcluirProduto_Click" Text="×" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-danger" ToolTip="Excluir Produto" TabIndex="6"/>
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
                                                    <asp:BoundField DataField="Unidade" HeaderText="Unidade"  DataFormatString="{0:F}"  SortExpression="Unidade" />
                                                    <asp:BoundField DataField="Quantidade" HeaderText="Quantidade"  DataFormatString="{0:F}"  SortExpression="Quantidade" />
                                                    <asp:BoundField DataField="PrecoItem" HeaderText="Preço"  DataFormatString="{0:C}"  SortExpression="PrecoItem" />
                                                    <asp:BoundField DataField="ValorDesconto" HeaderText="% Desconto"  DataFormatString="{0:F}%"  SortExpression="ValorDesconto"/>
                                                    <asp:BoundField DataField="ValorTotalItem" HeaderText="Valor Total"  DataFormatString="{0:C}"  SortExpression="ValorTotalItem" />
                                                    <asp:BoundField DataField="Cpl_DscSituacao" HeaderText="Situação"  SortExpression="Cpl_DscSituacao" />
                                                    
                                                                                                                      
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
               
                    </div>
                </div>
            </div>
        </div>

</asp:Content>
