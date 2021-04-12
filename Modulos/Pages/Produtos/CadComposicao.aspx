<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="CadComposicao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Produtos.CadComposicao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <script>
        function Excluir()
        {
            $("#Excluir .modal-title").html("");
            $("#Excluir").modal("show");
        }
        function GridProdutoComposto()
        {
            $("#CdProdutosParaComposicao").modal("show");
        }
        function GridDesaparece()
        {
            $("#CdProdutosParaComposicao").modal("hide");
        }
        function Roteiro()
        {
            $("#Roteiro.modal-title").html("");
            $("#Roteiro").modal("show");
        }
        $("#<%= txtRoteiro.ClientID%>").OnChanged(function (e) {

            e.preventDefault();
            if ($(".slide").is(":hidden")) {
                $(".slide").slideUp("slow");
            }
            else {
                $(".slide").slideDown("slow");
            }
        });
    </script>
    <style> 
        .slide
        {
            font-size: 12px;
            font-family: Arial,sans-serif;
            display: none;
            height: 100px;
            background-color: #148A21;
            color:#000000;
        }
        .noPrint 
        {
            display:none
        }
    </style>

<div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
    <div class="panel panel-primary">
        <div class="panel-heading">
            Composição do Produto
            <div class="messagealert" id="alert_container"></div>        
        </div>
        <div class="panel-body">
            <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click" TabIndex="11"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
            </asp:LinkButton>
            <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" TabIndex="12"> 
                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
            </asp:LinkButton>
            <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" OnClick="btnExcluir_Click" TabIndex="13"> 
                    <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
            </asp:LinkButton>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Always">
                <ContentTemplate>
                    <div class="row" style="margin-top: 10px">
                        <div class="col-md-10" Style="padding-right:0!important">
                            <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Produto Composto: </label>
                            <div class="input-group " style="width:100%">
                                <asp:TextBox ID="txtProdutoComposto" CssClass="form-control"  runat="server" TabIndex="7" OnTextChanged="txtProduto_TextChanged" AutoPostBack="true"                                
                                Width="15%" MaxLength="6" />                                                      
                                <asp:LinkButton ID="btnPesquisarComposto" BorderStyle="Inset" runat="server" CssClass="form-control btn btn-primary" Width="10%" TabIndex="8" OnClick="btnPesquisarComposto_Click" AutoPostBack="true"> 
                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                </asp:LinkButton>
                                <asp:TextBox ID="txtDcrproduto" CssClass="form-control"  runat="server"  Enabled="false"  Width="75%"  AutoPostBack="true"/>
                            </div>
                        </div>
                        <div class="col-md-2" style="margin-top:3px;">
                            <label for="usr"> Data da Atualização:</label>
                            <asp:TextBox ID="txtData" CssClass="form-control" Enabled="false" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px;">
                        <div class="col-md-2" Style="padding-right:0!important">
                            <label for="usr"> Tipo: </label>
                            <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="3"> </asp:DropDownList>		   
                        </div>
                        <div class="col-md-2" Style="padding-right:0!important">
                            <label for="usr">Rendimento:</label>
                            <asp:TextBox ID="txtRendimento" OnTextChanged="txtRendimento_TextChanged" CssClass="form-control" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                        </div>
                        <div class="col-md-2" Style="padding-right:0!important">
                            <label for="usr">% Quebra:</label>
                            <asp:TextBox ID="txtQuebra" CssClass="form-control" OnTextChanged="txtQuebra_TextChanged" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                        </div>
                        <div class="col-md-2" Style="padding-right:0!important">
                            <label for="usr">% Umidade:</label>
                            <asp:TextBox ID="txtUmidade" CssClass="form-control" OnTextChanged="txtUmidade_TextChanged" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                        </div>
                        <div class="col-md-2" Style="padding-right:0!important; width:190px">
                            <label for="usr"> Custo Composto:  </label>
                            <div class="input-group" style="width:100%">
                                <asp:TextBox ID="txtValor" Enabled="false" Width="28%" style="border-right:unset;text-align:end" CssClass="form-control" Text="R$:" Font-Size="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                                <asp:TextBox ID="txtvalorProduto" Enabled="false" Width="72%" style="border-left:unset; text-align:start" CssClass="form-control" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                            </div>
                        </div>
                        <div class="col-md-2"  style="padding-right:0!important" >
                            <label for="usr"> Situação: </label>
                            <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="3"> </asp:DropDownList>		   
                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px; padding-right:0!important">
                        <div class="col-md-12">
                            <label for="usr">OBS Composto:</label>
                            <asp:TextBox ID="txtObs" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="50"/>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px">
                        <div class="col-md-12" >
                            <div class="modal-content"> 
                                <div class="panel-heading"> 
                                    <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Itens da Composição</label>
                                </div>
                                <div class="panel-body">
                                    <div class="row" style="margin-top: 8px">
                                        <div class="col-md-2" style="margin-top:3px; padding-right:0!important">
                                            <label for="usr"> Roteiro: </label>
                                                <div class="input-group" style="width:100%">
                                                    <asp:DropDownList ID="ddlRoteiro" Width="75%" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="3"> </asp:DropDownList>		   
                                                    <asp:Button runat="server" id="btnRoteiro" Width="25%" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="btnRoteiro_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar Roteiro" TabIndex="5"/>
                                                </div>
                                        </div>
                                        <div class="col-md-7" Style="padding-right:0!important">
                                            <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Produto Componente:</label>
                                            <div class="input-group " style="width:100%">
                                                <asp:TextBox ID="txtProdutoComponente" CssClass="form-control"  runat="server" TabIndex="7" OnTextChanged="txtProdutoComponente_TextChanged" AutoPostBack="true"                                
                                                Width="15%" MaxLength="6" />                                                      
                                                <asp:LinkButton ID="btnPesquisarComponente" runat="server" CssClass="form-control btn btn-primary" Width="10%" TabIndex="8" OnClick="btnPesquisarComponente_Click" AutoPostBack="true"> 
                                                    <span aria-hidden="true" class="glyphicon glyphicon-search"></span>
                                                </asp:LinkButton>
                                                <asp:TextBox ID="txtDescComposto" CssClass="form-control"  runat="server"  Enabled="false"  Width="75%"  AutoPostBack="true"/>
                                            </div>
                                        </div>
                                        <div class="col-md-1" Style="padding-top:2px ;padding-right:0!important">
                                            <label for="usr">Und:</label>
                                            <asp:TextBox ID="txtUnd" CssClass="form-control" Enabled="false" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                                        </div>
                                        <div class="col-md-2 "Style="padding-top:1px">
                                            <label for="usr">Embalagem:</label>
                                            <asp:TextBox ID="txtEmb" CssClass="form-control" Enabled="false" AutoPostBack="true" Text="Embalagem" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 10px">
                                        <div class="col-md-2" Style="padding-right:0!important">
                                            <label for="usr"> Quantidade:  </label>
                                            <asp:TextBox ID="txtquantidade" OnTextChanged="txtquantidade_TextChanged" CssClass="form-control" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                                        </div>
                                        <div class="col-md-2" Style="padding-right:0!important">
                                            <label for="usr"> Custo Componente:  </label>
                                            <div class="input-group" style="width:100%">
                                                <asp:TextBox ID="txtV" Enabled="false" Width="30%" Font-Bold="true" style="border-right:unset;text-align:end" BackColor="Transparent" CssClass="form-control" Text="R$:" Font-Size="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                                                <asp:TextBox ID="txtValorComponente" Enabled="false" Width="70%" style="border-left:unset; text-align:initial" CssClass="form-control" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                                            </div>
                                        </div>
                                        <div class="col-md-1" Style="padding-right:0!important">
                                            <label for="usr"> %Quebra:</label>
                                            <asp:TextBox ID="txtQuebraComponente" OnTextChanged="txtQuebraComponente_TextChanged" CssClass="form-control" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4"/>
                                        </div>
                                        <div class="col-md-6" Style="padding-right:0!important">
                                            <label for="usr"> OBS Componente:</label>
                                            <asp:TextBox ID="txtObsComponente" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="100"/>
                                        </div>
                                        <div class="col-md-1" style="background-color: white; padding-right:0!important; border: none; text-align: left; font-size: x-small;margin-top:22px;padding-right:0!important">
                                            <asp:Button runat="server" id="BtnAddProduto" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="BtnAddProduto_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar/Alterar produto" TabIndex="5"/>
                                            <asp:Button runat="server" id="BtnRemProduto" Visible="false" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="BtnRemProduto_Click" Text="×" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-danger" ToolTip="Excluir Produto" TabIndex="6"/>
                                        </div> 
                                    </div>
                                    <div class="row" style="margin-top: 10px">
                                        
                                    </div>
                                    <div class="row" style="margin-top: 10px">
                                        <div class="col-md-12" >
                                            <div class="modal-body">
                                                <asp:GridView ID="GridComposicao" runat="server" Width="100%"
                                                CssClass="table table-bordered table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"
                                                OnSelectedIndexChanged="GridComposicao_SelectedIndexChanged"
                                                AllowPaging="true" PageSize="50"
                                                PagerSettings-Mode="NumericFirstLast">
                                                    <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoRoteiro" HeaderText="Cód." />
                                                            <asp:BoundField DataField="DescRoteiro" HeaderText="Roteiro" />
                                                            <asp:BoundField DataField="CodigoComponente" HeaderText="Código" />
                                                            <asp:BoundField DataField="DescricaoComponente" HeaderText="Descrição"  />
                                                            <asp:BoundField DataField="QuantidadeComponente" HeaderText="Quantidade"  />
                                                            <asp:BoundField DataField="ValorCustoComponente" HeaderText="Valor Custo"  />
                                                            <asp:BoundField DataField="PerQuebraComponente" HeaderText="% Quebra"  />
                                                            <asp:BoundField DataField="Observacao" HeaderStyle-CssClass="noPrint" ItemStyle-CssClass="NoPrint" HeaderText="Observação"  /> 
                                                            <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                                            ItemStyle-Height="15px" ItemStyle-Width="50px"
                                                            ButtonType="Image" SelectImageUrl="~/Images/Acessar.svg"
                                                            ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                                        </Columns>
                                                    <RowStyle CssClass="cursor-pointer" />
                                                </asp:GridView>                   
                                            </div>
                                        </div>
<asp:TextBox ID="TXTSAIR" CssClass="form-control" Enabled="false" Visible="false" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="100"/>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>       
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Always">
    <ContentTemplate>
        <div class="modal fade" id="Excluir" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
            <div class="modal-dialog" role="document" style ="height:600px;width:300px">
                <div class="modal-content" >
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="H3">Atenção</h4>
                    </div>
                    <div class="modal-body">
                      Deseja Excluir esta Composição?
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click" > 
                            <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>

</asp:UpdatePanel>

<div class="modal fade" id="CdProdutosParaComposicao"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
    <div class="modal-dialog" role="document" style ="height:300px;width:520px">
        <div class="panel panel-primary">
            <div class="panel-heading" style="height:40px"> 
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <label class="modal-title" id="H6"> Consulta de Produtos Componentes</label>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server"  UpdateMode="Always">
                    <ContentTemplate>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-11" >
                                <label for="usr"> Digite a Descrição Produto Componente:</label>
                                <asp:TextBox ID="txtPesquisaDescricao"  PlaceHolder="No minimo 3 Caracteres ..." OnTextChanged="txtPesquisaDescricao_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-11" >
                                <asp:label id="lblMensagem" runat="server" for="usr" ForeColor="OrangeRed"> </asp:label>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="panel-body" style="overflow-x:scroll;height:300px;width:500px">
                                <asp:GridView ID="GridCompostos" runat="server" Width="100%"
                                    CssClass="table table-bordered table-hover table-striped"
                                    GridLines="None" AutoGenerateColumns="False"
                                    Font-Size="8pt"
                                    OnSelectedIndexChanged="GridCompostos_SelectedIndexChanged"
                                    AllowPaging="true" PageSize="50"
                                    OnPageIndexChanging="GridCompostos_PageIndexChanging"
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>
<div class="modal fade" id="Roteiro" style="padding-top:530px;padding-right:975px!important" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
    <div class="modal-dialog" role="document" style ="height:600px;width:175px">
        <div class="modal-content" style="border:unset" >
            <div class="modal-footer" style="padding:2px!important; margin:2px!important; text-align:left">
                <asp:TextBox ID="txtRoteiro" PlaceHolder="Roteiro..." OnTextChanged="txtRoteiro_TextChanged"  CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="20"/>
            </div>
        </div>
    </div>
</div>
</asp:Content>

