<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManRegFrete.aspx.cs" Inherits="SoftHabilInformatica.Pages.Transporte.ManRegFrete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>

        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});           
        }

    </script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;" >
         
        <div class="panel panel-primary">
            <div class="panel-heading">Manutenção da Regra de frete
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                    <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                    </div> 
                </asp:Panel>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-12">

                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click"> 
                            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>

                        <asp:LinkButton runat="server" CssClass=" btn btn-success link-button-glyphicon" ID="btnSalvar" >
                            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-save" ></span>
                            <asp:Button runat="server"  OnClientClick="this.disabled = true; this.value = 'Salvando...';" UseSubmitBehavior="false" OnClick="btnSalvar_Click" CssClass=" button-glyphicon" Text="Salvar" />
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcluir" runat="server" onFocus="this.select()" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes"> 
                            <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>
                        
                    </div>
                </div>
                <div class="row" >
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="col-md-12" style="margin-top:15px!important">
                                <div class="row">
                                    <div class="col-md-2" style="font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Código</label>
                                        <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control "  Font-Size ="Small" Enabled="false" ></asp:TextBox>
                                    </div>
                                    <div class="col-md-2" style="font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Região</label>
                                        <asp:TextBox ID="txtRegiao" runat="server" CssClass="form-control "  Font-Size ="Small" AutoPostBack="true" onFocus="this.select()" MaxLength="30"></asp:TextBox>
                                    </div>
                                    <div class="col-md-8" style="font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Transportador</label>
                                        <asp:DropDownList ID="ddlTransp" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" Width="100% " ></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6" style="font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Frete Minimo</label>
                                <asp:TextBox ID="txtFreteMinimo" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtFreteMinimo_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                            </div>
                            <div class="col-md-6" style="font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">GRIS</label>
                                <asp:TextBox ID="txtGRIS" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtGRIS_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                            </div>
                            <div class="col-md-6" style="padding:0!important">
                                <div class="col-md-12" >
                                    <div class="panel panel-primary" style="margin-top:15px!important">
                                        <div class="panel-heading">Regra para Frete Valor</div>
                                        <div class="panel-body">
                                            <div class="col-md-12" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">% - Calcular sobre valor NF-e </label>
                                                <asp:TextBox ID="txtDeParaPct11" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct11_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                            </div>
                                            <div class="col-md-6" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">De</label>
                                                <asp:TextBox ID="txtDePara11" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara11_TextChanged" AutoPostBack="true"  Text="0,00000"  onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                            </div>
                                            <div class="col-md-6" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Até</label>
                                                <asp:TextBox ID="txtDePara12" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara12_TextChanged" AutoPostBack="true" Text="0,00000"  onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12" >
                                    <div class="panel panel-primary" >
                                        <div class="panel-heading">Regra para Frete Peso</div>
                                        <div class="panel-body">
                                            <div class="col-md-12" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">R$ - Calcular sobre peso NF-e</label>
                                                <asp:TextBox ID="txtDeParaPct12" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct12_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">De</label>
                                                <asp:TextBox ID="txtDePara21" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara21_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Até</label>
                                                <asp:TextBox ID="txtDePara22" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara22_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12" style="display:none">
                                    <div class="panel panel-primary" style="margin-top:15px!important">
                                        <div class="panel-heading">Regra 3
                                        </div>
                                        <div class="panel-body">
                                            <div class="col-md-6" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">De </label>
                                                <asp:TextBox ID="txtDePara31" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara31_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Para </label>
                                                <asp:TextBox ID="txtDePara32" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara32_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6" style="margin-top:15px!important">
                                <div class="panel panel-primary">
                                    <div class="panel-heading" style="text-align:center">Cidades da Regra de frete</div>
                                    <div class="panel-body">
                                        <div class="col-md-12">
                                            <div class="col-md-4" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Estado</label>
                                                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-7" style="font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Cidade</label>
                                                <asp:DropDownList ID="ddlCidade" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" ></asp:DropDownList>
                                            </div>
                                            <div class="col-md-1" style="font-size:x-small; margin-top:20px;">
                                                <asp:Button runat="server" id="btnAdd" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="btnAdd_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar esta cidade a regra" TabIndex="5"/>
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="margin-top:15px">
                                            <asp:GridView ID="grdCidades" runat="server"
                                                    CssClass="table table-striped"
                                                    GridLines="None" AutoGenerateColumns="False" OnSelectedIndexChanged="grdCidades_SelectedIndexChanged"
                                                    Font-Size="8pt"  
                                                    AllowPaging="true" PageSize= "100" 
                                                    OnPageIndexChanging="grdCidades_PageIndexChanging"
                                                    PagerSettings-Mode ="NumericFirstLast" >
                                                <PagerStyle HorizontalAlign = "LEFT"   CssClass = "GridPager" />
                                                
                                                <Columns>
                                                    <asp:BoundField DataField="Cpl_Sigla" HeaderText="Estado" />
                                                    <asp:BoundField DataField="CodigoIBGE" HeaderText="IBGE" />
                                                    <asp:BoundField DataField="Cpl_DescricaoCidade" HeaderText="Cidade" />
                                                    <asp:CommandField HeaderText="Remover" ShowSelectButton="True" HeaderStyle-CssClass="col-md-1"
                                                            ItemStyle-Height ="15px" ItemStyle-Width ="20px" 
                                                            ButtonType="Image"  SelectImageUrl ="~/Images/Excluir.png" 
                                                            ControlStyle-Width ="20px" ControlStyle-Height ="20px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlEstado" EventName="SelectedIndexChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID="grdCidades" EventName="SelectedIndexChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtRegiao" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara11" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara12" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara21" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara22" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara31" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara32" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtFreteMinimo" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtGRIS" EventName="TextChanged"/>
                        </Triggers>
                    </asp:UpdatePanel>
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
                            Deseja Excluir esta regra de frete?
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
</asp:Content>
