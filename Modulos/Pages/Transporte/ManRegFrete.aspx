<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManRegFrete.aspx.cs" Inherits="SoftHabilInformatica.Pages.Transporte.ManRegFrete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script src="../../Scripts/TratamentoINPUTs.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>

        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});     

            item = '#home';
            $('#myTabs a[href="' + item + '"]').tab('show');
        }
        item = '#home';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            
        });
        

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
                            <div class="col-md-12" style="margin-top:15px!important;margin-bottom:15px!important">
                                <div class="row">
                                    <div class="col-md-2" style="font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Código</label>
                                        <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control "  Font-Size ="Small" Enabled="false" ></asp:TextBox>
                                    </div>
                                    <div class="col-md-2" style="font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Região</label>
                                        <asp:TextBox ID="txtRegiao" runat="server" CssClass="form-control "  Font-Size ="Small" AutoPostBack="true" onFocus="this.select()" MaxLength="30" ></asp:TextBox>
                                    </div>
                                    <div class="col-md-8" style="font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Transportador</label>
                                        <asp:DropDownList ID="ddlTransp" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" Width="100% " ></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;margin:10px;">
                                <ul class="nav nav-tabs" role="tablist" id="myTabs">
                                    <li role="presentation"><a href="#home" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-cog"></span>&nbsp;&nbsp;Configurações&nbsp;&nbsp;</a></li>
                                    <li role="presentation"><a href="#consulta" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-plane"></span>&nbsp;&nbsp;Cidades</a></li>
                                </ul>
                                <div class="tab-content" runat="server" id="PanelContext">
                                    <div role="tabpanel" class="tab-pane" id="home" style="font-size: small;">
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Frete Minimo</label>
                                            <asp:TextBox ID="txtFreteMinimo" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtFreteMinimo_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">GRIS</label>
                                            <asp:TextBox ID="txtGRIS" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtGRIS_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Valor GRIS minimo </label>
                                            <asp:TextBox ID="txtGRISMinimo" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtGRISMinimo_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Valor Pedágio</label>
                                            <asp:TextBox ID="txtPedagio" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtPedagio_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Valor Pedágio Máximo</label>
                                            <asp:TextBox ID="txtPedagioMaximo" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtPedagioMaximo_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">AD Valor</label>
                                            <asp:TextBox ID="txtADValor" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtADValor_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Valor Seguro</label>
                                            <asp:TextBox ID="txtSeguro" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtSeguro_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Valor Seguro Minimo</label>
                                            <asp:TextBox ID="txtSeguroMinimo" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtSeguroMinimo_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Valor Por Tonelada</label>
                                            <asp:TextBox ID="txtPorTonelada" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtPorTonelada_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Valor Peso Cubado (ex.:300,250,200)</label>
                                            <asp:TextBox ID="txtPesoCubado" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtPesoCubado_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>   
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Excedente (R$)</label>
                                            <asp:TextBox ID="txtExcedente1" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtExcedente1_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Excedente (%)</label>
                                            <asp:TextBox ID="txtExcedente2" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtExcedente2_TextChanged" AutoPostBack="true" Text="0,00" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                        </div>
                                    <div class="col-md-12" style="padding:0!important">
                                        <asp:Panel runat="server" ID="pnlRegra1" Visible ="false">
                                            <div class="col-md-3" >
                                                <div class="panel panel-primary" style="margin-top:15px!important">
                                                    <div class="panel-heading">Regra 1</div>
                                                    <div class="panel-body">
                                                        <div class="col-md-12" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl1Regra1" for="usr" style ="margin-top:1px;">Valor</asp:label>
                                                            <asp:TextBox ID="txtDeParaPct11" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct11_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl2Regra1" for="usr" style ="margin-top:1px;">De</asp:label>
                                                            <asp:TextBox ID="txtDePara11" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara11_TextChanged" AutoPostBack="true"  Text="0,00000"  onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl3Regra1" for="usr" style ="margin-top:1px;">Até</asp:label>
                                                            <asp:TextBox ID="txtDePara12" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara12_TextChanged" AutoPostBack="true" Text="0,00000"  onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlRegra2" Visible ="false">
                                            <div class="col-md-3" >
                                                <div class="panel panel-primary" style="margin-top:15px!important">
                                                    <div class="panel-heading">Regra 2</div>
                                                    <div class="panel-body">
                                                        <div class="col-md-12" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl1Regra2" for="usr" style ="margin-top:1px;">Valor</asp:label>
                                                            <asp:TextBox ID="txtDeParaPct12" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct12_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl2Regra2" for="usr" style ="margin-top:1px;">De</asp:label>
                                                            <asp:TextBox ID="txtDePara21" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara21_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl3Regra2" for="usr" style ="margin-top:1px;">Até</asp:label>
                                                            <asp:TextBox ID="txtDePara22" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara22_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlRegra3" Visible ="false">
                                            <div class="col-md-3" >
                                                <div class="panel panel-primary" style="margin-top:15px!important">
                                                    <div class="panel-heading">Regra 3
                                                    </div>
                                                    <div class="panel-body">
                                                        <div class="col-md-12" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl1Regra3" for="usr" style ="margin-top:1px;">Valor</asp:label>
                                                            <asp:TextBox ID="txtDeParaPct31" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct31_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl2Regra3" for="usr" style ="margin-top:1px;">De </asp:label>
                                                            <asp:TextBox ID="txtDePara31" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara31_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl3Regra3" for="usr" style ="margin-top:1px;">Para </asp:label>
                                                            <asp:TextBox ID="txtDePara32" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara32_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlRegra4" Visible ="false">
                                            <div class="col-md-3" >
                                                <div class="panel panel-primary" style="margin-top:15px!important">
                                                    <div class="panel-heading">Regra 4
                                                    </div>
                                                    <div class="panel-body">
                                                        <div class="col-md-12" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl1Regra4" for="usr" style ="margin-top:1px;">Valor</asp:label>
                                                            <asp:TextBox ID="txtDeParaPct41" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct41_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl2Regra4" for="usr" style ="margin-top:1px;">De </asp:label>
                                                            <asp:TextBox ID="txtDePara41" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara41_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl3Regra4" for="usr" style ="margin-top:1px;">Para </asp:label>
                                                            <asp:TextBox ID="txtDePara42" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara42_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlRegra5" Visible ="false">
                                            <div class="col-md-3" >
                                                <div class="panel panel-primary" style="margin-top:15px!important">
                                                    <div class="panel-heading">Regra 5
                                                    </div>
                                                    <div class="panel-body">
                                                        <div class="col-md-12" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl1Regra5" for="usr" style ="margin-top:1px;">Valor</asp:label>
                                                            <asp:TextBox ID="txtDeParaPct51" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct51_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl2Regra5" for="usr" style ="margin-top:1px;">De </asp:label>
                                                            <asp:TextBox ID="txtDePara51" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara51_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl3Regra5" for="usr" style ="margin-top:1px;">Para </asp:label>
                                                            <asp:TextBox ID="txtDePara52" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara52_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlRegra6" Visible ="false">
                                            <div class="col-md-3" >
                                                <div class="panel panel-primary" style="margin-top:15px!important">
                                                    <div class="panel-heading">Regra 6
                                                    </div>
                                                    <div class="panel-body">
                                                        <div class="col-md-12" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl1Regra6" for="usr" style ="margin-top:1px;">Valor</asp:label>
                                                            <asp:TextBox ID="txtDeParaPct61" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct61_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl2Regra6" for="usr" style ="margin-top:1px;">De </asp:label>
                                                            <asp:TextBox ID="txtDePara61" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara61_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl3Regra6" for="usr" style ="margin-top:1px;">Para </asp:label>
                                                            <asp:TextBox ID="txtDePara62" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara62_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlRegra7" Visible ="false">
                                             <div class="col-md-3" >
                                                <div class="panel panel-primary" style="margin-top:15px!important">
                                                    <div class="panel-heading">Regra 7
                                                    </div>
                                                    <div class="panel-body">
                                                        <div class="col-md-12" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl1Regra7" for="usr" style ="margin-top:1px;">Valor</asp:label>
                                                            <asp:TextBox ID="txtDeParaPct71" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDeParaPct71_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl2Regra7" for="usr" style ="margin-top:1px;">De </asp:label>
                                                            <asp:TextBox ID="txtDePara71" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara71_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6" style="font-size:x-small;">
                                                            <asp:label runat="server" ID="lbl3Regra7" for="usr" style ="margin-top:1px;">Para </asp:label>
                                                            <asp:TextBox ID="txtDePara72" runat="server" CssClass="form-control"  Font-Size ="Small" OnTextChanged="txtDePara72_TextChanged" AutoPostBack="true" Text="0,00000" onFocus="this.select()" MaxLength="13" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                    <div role="tabpanel" class="tab-pane" id="consulta" style="font-size: small;">
                                        <div class="col-md-12" style="margin-top:15px!important">
                                            <div class="panel panel-primary">
                                                <div class="panel-heading" style="text-align:center">Cidades da Regra de frete</div>
                                                <div class="panel-body">
                                                    <div class="col-md-12">
                                                        <div class="col-md-4" style="font-size:x-small;">
                                                            <label for="usr" style ="margin-top:1px;">Estado</label>
                                                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true" Width="100%"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-7" style="font-size:x-small;">
                                                            <label for="usr" style ="margin-top:1px;">Cidade</label>
                                                            <asp:DropDownList ID="ddlCidade" runat="server" CssClass="form-control js-example-basic-single" Font-Size ="Small" Width="100%"></asp:DropDownList>
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
                            <asp:AsyncPostBackTrigger ControlID="txtDeParaPct11" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara21" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara22" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDeParaPct12" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara31" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara32" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDeParaPct31" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara41" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara42" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDeParaPct41" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara51" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara52" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDeParaPct51" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara61" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara62" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDeParaPct61" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara71" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDePara72" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtDeParaPct71" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtFreteMinimo" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtGRIS" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtGRISMinimo" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtPedagio" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtPedagioMaximo" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtSeguro" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtSeguroMinimo" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtADValor" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtPorTonelada" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtExcedente1" EventName="TextChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="txtExcedente2" EventName="TextChanged"/>

                            <asp:AsyncPostBackTrigger ControlID="txtPesoCubado" EventName="TextChanged"/>
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
