<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="CnfPedido.aspx.cs"
    Inherits="SoftHabilInformatica.Pages.Vendas.CnfPedido" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">

    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
        function abc()
        {
            $("#CdCaixas .modal-title").html("");
            $("#CdCaixas").modal("show");
        }
        var aba = 1;
        $(document).on('keydown', function (e) {
            if ((e.altKey) && (e.which === 80)) { // p = novo produto 
                $get('<%=txtCod.ClientID%>').focus(); 
            }
            else if ((e.altKey) && (e.which === 81)) { // q =  sair
                $get('<%=btnSair.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 78)) { // n = novo
                $get('<%=btnNovo.ClientID%>').click(); 
             }
            else if ((e.altKey) && (e.which === 66)) { // b = barras/produtos
                $get('<%=txtNrPedido.ClientID%>').click(); 
             }
            else if ((e.altKey) && (e.which === 67)) { // c = caixa abrir/fechar
                $get('<%=btnAbrirFechar.ClientID%>').click(); 
             }
            else if ((e.altKey) && (e.which === 49)) { // nr 1 = caixa empresa
                $get('<%=BtnCaixaEmpresa.ClientID%>').click(); 
             }
            else if ((e.altKey) && (e.which === 50)) { // nr 2 = caixa outros
                $get('<%=BtnCaixaOutros.ClientID%>').click(); 
             }
            else if ((e.altKey) && (e.which === 51)) { // nr 3 = caixa Retornavel
                $get('<%=BtnCaixaRetornavel.ClientID%>').click(); 
             }
            else if ((e.altKey) && (e.which === 82)) { // R = recontar
                $get('<%=btnRecontar.ClientID%>').click(); 
             }
            else if ((e.altKey) && (e.which === 83)) { // s = finalizar
                $get('<%=btnFinalizar.ClientID%>').click(); 
             }
            else if ((e.altKey) && (e.which === 66)) { // b = texto retornavel
                $get('<%=txtCaixaRet.ClientID%>').focus(); 
            }
            else if ((e.altKey) && (e.which === 86)) { // v = texto volume
                $get('<%=txtVolume.ClientID%>').focus(); 
            }
            else if ((e.altKey) && (e.which === 84)) { // t = texto quantidade
                $get('<%=txtVolume.ClientID%>').focus(); 
            }
            else if ((e.altKey) && (e.which === 39)) { 
                
                aba++;
                if (aba == 3)
                    aba = 1;
                $('#myTabs a[href="#aba' + aba + '"]').tab('show');

            }
  
        });
    </script>
    
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Conferência de Pedido
                <div class="messagealert" id="alert_container"></div>
            </div>
            <div class="panel-body">
                <div class="row" style="margin-top:1px">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
			            <ContentTemplate>
                            <asp:Panel ID="pnlNovo" runat="server"> 
                                <div class="col-md-1" style="padding-top:25px; padding-left:10px">
                                    <asp:LinkButton ID="btnNovo" ToolTip="(alt + N)" runat="server" Text="Novo" Width="85px" CssClass="btn btn-info" OnClick="btnNovo_Click"> 
                                        <span aria-hidden="true" TabIndex="18" title="Novo" class="glyphicon glyphicon-edit"></span> Novo 
                                    </asp:LinkButton>
                                </div>
                            </asp:Panel>  
                            <asp:Panel ID="pnlRecontar" runat="server" Visible="false"> 
                                <div class="col-md-1" style="padding-top:25px; padding-left:5px">
                                    <asp:LinkButton ID="btnRecontar" ToolTip="(alt + R)" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnRecontar_Click">
                                        <span aria-hidden="true" title="Fechar" TabIndex="19" class="glyphicon glyphicon-repeat"></span>  Recontar
                                    </asp:LinkButton>
                                </div>
                            </asp:Panel>   
                            <asp:Panel id="pnlSair" runat="server"> 
                                <div class="col-md-1" style="padding-top:25px; padding-right:0!important ">
                                    <asp:LinkButton ID="btnSair" runat="server" ToolTip="(alt + Q)"  Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                                        <span aria-hidden="true" title="Fechar" TabIndex="19" class="glyphicon glyphicon-off"></span>  Fechar
                                    </asp:LinkButton>
                                </div>
                            </asp:Panel>  
                            <asp:Panel id="pnlConferente" runat="server"> 
                            <div class="col-md-7" style="">
                                <label for="usr"> Conferente: </label>
                                <asp:TextBox ID="txtConferente" CssClass="form-control" Enabled="false" runat="server" TabIndex="1"/>
                            </div>      
                        </asp:Panel>
                        </ContentTemplate> 
					</asp:UpdatePanel>
                </div>
                <div class="row" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
                    <div class="row" style="padding-top:1px">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
						    <ContentTemplate>
                                <div class="col-md-2" style="padding-right:0!important">
                                    <label for="usr">NrºPedido: </label>
                                    <asp:Button ID="btnNada2" Text="btnNada" runat="server" Visible="false" OnClick="btnNada2_Click"/>
                                    <asp:Panel ID="PanelA" runat="server" DefaultButton="btnNada2"> </asp:Panel>
                                    <asp:TextBox ID="txtNrPedido" ToolTip="(alt + N)" CssClass="form-control" AutoPostBack="true" Enabled="true" OnTextChanged="txtNrPedido_TextChanged" runat="server" TabIndex="0"/>    
                                </div>
                            </ContentTemplate>
                            <Triggers>
								<asp:AsyncPostBackTrigger ControlID ="txtNrPedido" EventName="TextChanged"/>
							</Triggers>
					    </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
						    <ContentTemplate>
                                <div class="col-md-6" style="padding-right:0!important">
                                    <label for="usr">Cliente</label>
                                    <asp:TextBox ID="txtCliente" CssClass="form-control" name="txt" Enabled="false" runat="server" TabIndex="10"  />
                                </div>
                            </ContentTemplate> 
					    </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
						    <ContentTemplate>
                                <div class="col-md-1" style="padding-right:0!important">
                                    <label for="usr">NrºVolumes:</label>
                                    <asp:TextBox ID="txtVolume" CssClass="form-control" AutoPostBack="true" Text="0" Enabled="false" runat="server" TabIndex="2"/>
                                </div>
                            </ContentTemplate> 
					    </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
						    <ContentTemplate>
                                <div class="col-md-3" style="padding-right:0!important;width:275px">
                            <label for="usr"> Situação: </label>
                            <asp:TextBox ID="txtSituacao" CssClass="form-control" Enabled="false" runat="server" TabIndex="3"/>
                        </div>
                            </ContentTemplate> 
					    </asp:UpdatePanel>
                    </div>
                </div>
                <br />
                
                <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                    <li role="presentation"><a href="#aba1" aria-controls="geral" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-filter"></span>&nbsp;&nbsp;Geral</a></li>
                    <li role="presentation"><a href="#aba2" aria-controls="final" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Finalização</a></li>
                    <li STYLE="float:right!important;" title="Atalho para trocar de aba"><a class="tab-select">Alt + [→] </a></li>
                </ul>
                <div class="tab-content" runat="server"  id="PanelContext">
                    <div role="tabpanel" class="tab-pane" id="aba1" style="padding-left:20px;padding-right:20px;font-size:small;" >
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-2" style="padding-right:0!important">
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
						            <ContentTemplate>
                                        <label for="usr">Quantidade: </label>
                                        <asp:TextBox ID="txtQtd" ToolTip="(alt + T)" CssClass="form-control" AutoPostBack="true" MaxLength="10" Enabled="true" runat="server" TabIndex="4"  />
                                    </ContentTemplate> 
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-2" style="padding-right:0!important">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
						            <ContentTemplate>
                                        <label for="usr">Codígo/Barras </label>
                                        <asp:TextBox ID="txtCod" ToolTip="(alt + P)" CssClass="form-control" Font-Bold="true" BorderStyle="Solid" MaxLength="20" AutoPostBack="true" Enabled="true" OnTextChanged="txtCod_TextChanged" runat="server" TabIndex="5"  />
                                        <asp:Button ID="btnNada" Text="btnNada" runat="server" Visible="false" OnClick="btnNada_Click"/>
                                        <asp:Panel ID="pnlNada" runat="server" DefaultButton="btnNada">  </asp:Panel>
                                    </ContentTemplate> 
                                    <Triggers>
								        <asp:AsyncPostBackTrigger ControlID ="txtCod" EventName="TextChanged"/>
							        </Triggers>
            					</asp:UpdatePanel>
                            </div>
                            <div class="col-md-8" style="padding-right:0!important; width:758px">
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
						            <ContentTemplate>
                                        <label for="usr"> Descrição</label>
                                        <asp:TextBox ID="txtDesc" CssClass="form-control" name="txtLancamento" Font-Bold="true" ForeColor="#333333"  BorderStyle="Solid" Enabled="false" runat="server" TabIndex="1"/>
                                    </ContentTemplate> 
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
						    <ContentTemplate>
                                <asp:Panel id="pnlPrincipal" BackColor="Red" runat="server" Visible="false"> 
                            <div class="col-md-8" style="margin-top:15px;padding:0!important">
                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        <asp:Label Text="text" runat="server"> Produtos do Pedido </asp:Label>
                                    </div>
                                    <div class="panel-body" style="overflow:scroll;height:415px">
                                        <div class="col-md-12">
                                            <asp:GridView ID="grdGrid" runat="server" CssClass="table table-bordered table-striped"
                                                GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                PagerSettings-Mode="NumericFirstLast">
                                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                    <Columns>
                                                        <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="#" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:BoundField DataField="CodigoBarras" HeaderText="Código Barras" ItemStyle-Width ="5%"  />
                                                        <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                        <asp:BoundField DataField="QtAtendida" ItemStyle-Width ="5%" HeaderText="Atendida" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:TemplateField HeaderText="Saldo" ItemStyle-HorizontalAlign="Right" >
                                                            <ItemTemplate>  
                                                                <asp:Label runat="server" ID="lblSaldo" Font-Bold="true" Text='<%# Eval("Saldo").ToString() %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>            
                                                    </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4" style="margin-top:15px;padding-right:0!important">
                            <div class="col-md-6" style="padding-right:0!important">
                                <asp:RadioButton ID="BtnCaixaEmpresa" ToolTip="(alt + 1)" Width="100%" GroupName="RegularMenu" runat="server" AutoPostBack="true" OnCheckedChanged="BtnCaixa_CheckedChanged" Text="Caixa Papel/Empresa" BorderColor="Silver" BorderStyle="Groove" CssClass="btn btn-info" BackColor ="Silver"/>
                            </div>
                            <div class="col-md-6" style="padding-right:0!important">
                                <asp:RadioButton ID="BtnCaixaOutros" Width="100%" ToolTip="(alt + 2)" GroupName="RegularMenu" runat="server" AutoPostBack="true" OnCheckedChanged="BtnCaixaOutros_CheckedChanged" Text="Caixa Papel/Outro"  BorderColor="Silver" BorderStyle="Groove" CssClass="btn btn-info" BackColor ="Silver"/>
                            </div>
                            <div class="col-md-12" style="padding-right:0!important;padding-top:10px;">
                                <asp:RadioButton ID="BtnCaixaRetornavel" Width="100%" ToolTip="(alt + 3)" GroupName="RegularMenu" runat="server" AutoPostBack="true" OnCheckedChanged="BtnCaixaRetornavel_CheckedChanged" Text="Caixa Retornável"  BorderColor="Silver" BorderStyle="Groove"  CssClass="btn btn-info" BackColor ="Silver"/>
                            </div>
                            <br />
                            <br />
                            <div class="col-md-12" style="padding-right:0!important;padding-top:10px;">
                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        <label>Produtos Embalados</label>
                                        <asp:LinkButton ID="btnAbrirFechar" ToolTip="(alt + C)" runat="server" AutoPostBack="true" Text="Fechar" CssClass="btn btn-info" OnClick="btnAbrirFechar_Click"> Abrir </asp:LinkButton>
                                    </div>
                                    <div class="panel-body" style="overflow:scroll;height:300px;width:345px">
                                        <asp:Panel id="Panel1" runat="server" Visible="false"> 
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 01 <asp:Label id="Label1" runat="server" /> <asp:Label id="Label11" runat="server" /> </div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa1" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel2" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 02 <asp:Label id="Label2" runat="server" /> <asp:Label id="Label12" runat="server" /> </div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa2" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel3" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 03 <asp:Label id="Label3" runat="server" /><asp:Label id="Label13" runat="server" /> </div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa3" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel4" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 04 <asp:Label id="Label4" runat="server" /> <asp:Label id="Label14" runat="server" /> </div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa4" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel5" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 05 <asp:Label id="Label5" runat="server" /> <asp:Label id="Label15" runat="server" /> </div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa5" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel6" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 06 <asp:Label id="Label6" runat="server" /> <asp:Label id="Label16" runat="server" /></div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa6" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel7" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 07 <asp:Label id="Label7" runat="server" /><asp:Label id="Label17" runat="server" /></div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa7" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel8" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 08 <asp:Label id="Label8" runat="server" /> <asp:Label id="Label18" runat="server" /></div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa8" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel9" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 09 <asp:Label id="Label9" runat="server" /><asp:Label id="Label19" runat="server" /></div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa9" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel id="Panel10" runat="server" Visible="false">  
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">Caixa 10 <asp:Label id="Label10" runat="server" /><asp:Label id="Label20" runat="server" /></div>
                                                <div class="panel-body">
                                                        <asp:GridView ID="grdCaixa10" runat="server" CssClass="table table-bordered table-striped"
                                                        GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                                        PageSize="500" AllowPaging="true" Font-Size ="Smaller"
                                                        PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Cód" ItemStyle-Width ="5%" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" ItemStyle-Width ="40%"  />
                                                            <asp:BoundField DataField="QtColetada" ItemStyle-Width ="5%" HeaderText="Coletada" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                    </div>
                                </div>
                            </div>
                        </div>
                                    </asp:Panel>
                            </ContentTemplate> 
					    </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="aba2" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;">
                                <div class="row" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
						                <ContentTemplate>
                                            <div class="col-md-3" style="padding-right:0!important;width:275px">
                                                <label for="usr"> Conferente: </label>
                                                <asp:TextBox ID="txtConferenteFim" AutoPostBack="true" CssClass="form-control" Enabled="false" runat="server" TabIndex="3"/>
                                            </div>
                                            <div class="col-md-3" style="padding-right:0!important;width:275px">
                                                <label for="usr"> Doca: </label>
                                                <asp:TextBox ID="txtDoca" CssClass="form-control" AutoPostBack="true" Enabled="false" runat="server" TabIndex="3"/>
                                            </div>
                                            <div class="col-md-3" style="padding-right:0!important;width:275px">
                                                <label for="usr"> Volume: </label>
                                                <asp:TextBox ID="txtConfereVolume" ToolTip="(alt + V)" CssClass="form-control"  runat="server" TabIndex="3"/>
                                            </div>
                                            <div class="col-md-1" style="padding-top:25px; padding-left:10px">
                                                <asp:LinkButton ID="btnFinalizar" runat="server" ToolTip="(alt + S)" Text="Fechar" CssClass="btn btn-success" OnClick="btnFinalizar_Click">
                                                    <span aria-hidden="true" title="Fechar" TabIndex="19" class="glyphicon glyphicon-ok"></span>  Finaizar
                                                </asp:LinkButton>
                                            </div>
                                            <div class="col-md-1" >
                                        <asp:TextBox ID="txtDocumento" CssClass="form-control" Visible="false" Enabled="false" runat="server" TabIndex="3"/>
                                    </div>
                                        </ContentTemplate> 
					                </asp:UpdatePanel>
                                </div>
                            </div>
                </div>
            </div>  
        </div>
    </div>
        
      <!-- Codigo caixa -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Always">
            <ContentTemplate>
                <div class="modal fade" id="CdCaixas" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                    <div class="modal-dialog" role="document" style ="height:100%;width:300px">
                        <div class="modal-content" >
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title" id="H3">Caixa Retornavel</h4>
                            </div>
                            <div class="modal-body">
                                <label for="usr"> Código/Barras </label>
                                <asp:TextBox ID="txtCaixaRet" ToolTip="(alt + B)" OnTextChanged="txtCaixaRet_TextChanged" CssClass="form-control" name="txtCaixaRet" Enabled="true" AutoPostBack="true" runat="server" TabIndex="-1"/>                   
                            </div>
                            <div class="modal-footer">
                                <asp:Label ID="lbnMens" runat="server" Font-Bold="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="txtCaixaRet"/>
            </Triggers>
    </asp:UpdatePanel>
</asp:Content>
