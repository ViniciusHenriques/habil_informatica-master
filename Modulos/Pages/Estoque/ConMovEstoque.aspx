<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ConMovEstoque.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.ConMovEstoque" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    
    <script src="../../Scripts/funcoes.js"></script>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>


    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />

    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>

    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    
     <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtDtDe']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
            
        });
         $(document).ready(function () {
            $("input[id*='txtDtAte']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
            
        });
        $(document).ready(function () {
            $('.js-example-basic-single').select2({
                    
            });
              $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({
                    
            });
              $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
            
          
        }
    </script>


<div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
    <div class="panel panel-primary">
        <div class="panel-heading">
            Consulta de Movimentação de Estoque
            <div class="messagealert" id="alert_container"></div>
        </div>
        <div class="panel-body">
            <div id="Tabs" role="tabpanel">
                    <asp:LinkButton ID="btnSair" runat="server" onFocus="this.select()" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                        <span aria-hidden="true" title="Fechar" TabIndex="19" class="glyphicon glyphicon-off"></span>  Fechar
                    </asp:LinkButton>
                   

                <br/>
                <br/>
            <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                    <li role="presentation"><a href="#home"     aria-controls="home"    role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-filter"></span>&nbsp;&nbsp;Seleção dos Dados</a></li>
                    <li role="presentation"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Consulta</a></li>
                </ul>
            <!-- Tab panes -->
                <div class="tab-content" runat="server" onFocus="this.select()" id="PanelContext">
                    <div role="tabpanel" class="tab-pane" id="home" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;"  >
                        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" onFocus="this.select()" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="row" style="margin-top: 10px">
                                    <div class="col-md-6">
                                        <label for="usr"> Empresa: </label>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="1" Width="100%"> </asp:DropDownList>		   
                                                </ContentTemplate> 
                                            <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                                </asp:UpdatePanel>
                                    </div> 
                    
                                    <div class="col-md-3">
                                        <label for="usr">Periodo </label>
                                        <label for="usr">De: </label>
                                        <asp:TextBox ID="txtDtDe" CssClass="form-control" Enabled="true" runat="server" TabIndex="2"/>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="usr">Até: </label>    
                                        <asp:TextBox ID="txtDtAte" CssClass="form-control" Enabled="true" runat="server" TabIndex="3"/>
                                    </div>                                   
                                </div>
                                <div class="row" style="margin-top: 10px">  
                                    <div class="col-md-3">
                                        <label for="usr">Documento: </label>
                                        <asp:TextBox ID="txtDocumento" CssClass="form-control" runat="server" TabIndex="4"  />
                                    </div>
                                    <div class="col-md-9" >
                                        <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Produto: </label>
                                            <div class="input-group " style="width:100%">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtProduto" CssClass="form-control"  runat="server" TabIndex="5" OnTextChanged="txtProduto_TextChanged" OnSelectedIndexChanged="txtProduto_SelectedIndexChanged" AutoPostBack="true"                                
                                                            Width="15%" MaxLength="6" />                                                      
                                                            <asp:LinkButton ID="btnPesquisarItem"  runat="server" CssClass="form-control btn btn-primary" Width="8%" TabIndex="6" OnClick="btnPesquisarItem_Click" AutoPostBack="true"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                            </asp:LinkButton>
                                                        <asp:TextBox ID="txtDcrproduto" CssClass="form-control"  runat="server"  Enabled="false"  Width="77%"  />
                                                    </ContentTemplate> 
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtProduto" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDcrproduto" EventName="TextChanged" />
                                                        </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                    </div>              
                                </div> 
                                <div class="row" style="margin-top: 10px">
                                    <div class="col-md-6">
                                        <label for="usr"> Localização: </label>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlLocalizacao" runat="server" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="7"> </asp:DropDownList>		   
                                                </ContentTemplate> 
                                                    <Triggers> 
                                                        <asp:AsyncPostBackTrigger ControlID="ddlLocalizacao" EventName="TextChanged" />
                                                    </Triggers>
                                            </asp:UpdatePanel>
                                    </div> 
                                    <div class="col-md-6">
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <label for="usr"> Lote: </label>
                                                <asp:DropDownList ID="ddlLote" runat="server" AutoPostBack="true" CssClass="form-control js-example-basic-single" Width="100%" Font-Size="Small" TabIndex="8"> </asp:DropDownList>		   
                                            </ContentTemplate> 
                                            <Triggers> 
                                                <asp:AsyncPostBackTrigger ControlID="ddlLote" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div> 
                                </div>
                                <div class="row" style="margin-top: 10px">
                                    <div class="col-md-12" >
                                        <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Tipo de Operação: </label>
                                            <div class="input-group " style="width:100%">  
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always"> 
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="TxtCdTpOperacao" CssClass="form-control"  runat="server" TabIndex="9" OnTextChanged="TxtCdTpOperacao_TextChanged"  AutoPostBack="true"                                
                                                                Width="15%" MaxLength="4" />                                                      
                                                        <asp:LinkButton ID="btnTpOperacao"  runat="server" CssClass="form-control btn btn-primary" Width="5%" TabIndex="10" OnClick="btnTpOperacao_Click" AutoPostBack="true"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                        <asp:TextBox ID="Dsctpoperacao" CssClass="form-control"  runat="server"  Enabled="false"  Width="80%"  />
                                                    </ContentTemplate> 
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="TxtCdTpOperacao" EventName="TextChanged" /> 
                                                        <asp:AsyncPostBackTrigger ControlID="Dsctpoperacao" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                    </div>
                                </div>
                                <div class="container-fluid">
                                    <div class="row" style="background-color:white;border:none;">
                                        <div class="col-md-2" ><label for="usr" style ="margin-top:10px;">Quantidade de Registros</label></div>
                                            <div class="col-md-2" >
                                                <asp:DropDownList ID="ddlRegistros" CssClass="form-control" runat="server" TabIndex="11" Font-Size="Medium">
                                                    <asp:ListItem Value="100" Text="100" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="500" Text="500"></asp:ListItem>
                                                    <asp:ListItem Value="1000" Text="1000"></asp:ListItem>
                                                    <asp:ListItem Value="32600" Text="Todas"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1" >
                                                <asp:LinkButton ID="btnConsultar" runat="server" onFocus="this.select()" TabIndex="12" CssClass="btn btn-default" UseSubmitBehavior="false"  OnClick="btnConsultar_Click"> 
                                                    <span aria-hidden="true" title="Consultar" class="glyphicon glyphicon-search"></span>Consultar
                                                </asp:LinkButton>
                                            </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnConsultar" />
                                </Triggers>
                            
                        </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;">
                                    <asp:LinkButton ID="btnVoltarSelecao" TabIndex="13" runat="server" onFocus="this.select()" Text="Nova Consulta" CssClass="btn btn-default" UseSubmitBehavior="false" OnClick="btnVoltarSelecao_Click"> 
                                        <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>Nova Consulta
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnImprimir" runat="server" Text="Nova Consulta" CssClass="btn btn-success" OnClick="btnImprimir_Click"> 
                                        <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-print"></span>   Imprimir
                                    </asp:LinkButton>
                                <br/>
                                <br/>
                                <div class="row" style="margin-top: 10px">  
                                    <div class="col-md-3">
                                        <label for="usr" id="est1">Estoque Disponível: </label>
                                        <asp:TextBox ID="txtDisp" CssClass="form-control" runat="server" BackColor="Yellow" ForeColor="Red" Font-Bold="true" Enabled="false" TabIndex="14" />
                                    </div> 
                                    <div class="col-md-3" id="est2">
                                        <label for="usr">Estoque Total: </label>
                                        <asp:TextBox ID="txtTotal" CssClass="form-control" runat="server" BackColor="Yellow" ForeColor="Red" Font-Bold="true" Enabled="false" TabIndex="15"  />
                                    </div> 
                                    <div class="col-md-3" id="est3">
                                        <label for="usr">Estoque Reservado: </label>
                                        <asp:TextBox ID="txtReserv" CssClass="form-control" runat="server" BackColor="Yellow" ForeColor="Red" Font-Bold="true" Enabled="false" TabIndex="16"  />
                                    </div>
                                    <div class="col-md-3" id="est4">
                                        <label for="usr">Estoque Avaria: </label>
                                        <asp:TextBox ID="txtAva" CssClass="form-control" runat="server" BackColor="Yellow" ForeColor="Red" Font-Bold="true" Enabled="false" TabIndex="17"  />
                                    </div>
                                </div>
                                <br>
                                <br>

                                <div style = "overflow-x: auto;" class="row">
                                    <asp:GridView ID="grdGrid" runat="server" CssClass="table table-bordered table-hover table-striped" GridLines="None" AutoGenerateColumns="False"
                                            Font-Size="8pt"  AllowPaging="true" PageSize="100" PagerSettings-Mode="NumericFirstLast">
                                            <PagerStyle HorizontalAlign = "Right"   CssClass = "GridPager" Font-Size="Smaller" />
                                            <Columns>
                                                <asp:BoundField DataField="DtLancamento" HeaderText="Dt Lan" />
                                                <asp:BoundField DataField="NumeroDoc" HeaderText="Nr. Doc." />
                                                <asp:BoundField DataField="TpOperacao" HeaderText="Tp. Op"  />
                                                <asp:BoundField DataField="CodigoTipoOperacao" HeaderText="Cod. Op." />
                                                <asp:BoundField DataField="CodigoLocalizacao" HeaderText="Local" />
                                                <asp:BoundField DataField="CodigoLote" HeaderText="Lt" />
                                                <asp:BoundField DataField="CodigoProduto" HeaderText="Prod" />
                                                <asp:BoundField DataField="ValorSaldoAnterior" HeaderText="Sld. Ant." />
                                                <asp:BoundField DataField="QtMovimentada" HeaderText="Qt. Mov." />
                                                <asp:BoundField DataField="VlSaldoFinal" HeaderText="Sld. Final" />
                                                <asp:BoundField DataField="ValorUnitario" HeaderText="Cst. Unit." />
                                                <asp:BoundField DataField="CodigoDocumento" HeaderText="Cod. Doc." />
                                                <asp:BoundField DataField="CodigoIndice" HeaderText="Codigo Indice" />
                                                <asp:BoundField DataField="NomeMaquina" HeaderText="Usuario" />
                                                <asp:BoundField DataField="NomeUsuario" HeaderText="Maquina" />                                                
                                       
                                            </Columns>
                                        <RowStyle CssClass="cursor-pointer"  />
                                    </asp:GridView>
                                </div>
                                <div class="row" style="margin-top: 10px">  
                                    <div class="col-md-4">
                                        <label for="usr">Qt Saídas: </label>
                                        <asp:TextBox ID="txtQtSaida" CssClass="form-control" Enabled="false" BackColor="Yellow" ForeColor="Red" Font-Bold="true" runat="server" TabIndex="18"  />
                                    </div> 
                                    <div class="col-md-4">
                                        <label for="usr">Qt Entradas: </label>
                                        <asp:TextBox ID="txtQtEntrada" CssClass="form-control" Enabled="false" BackColor="Yellow" ForeColor="Red" Font-Bold="true" runat="server" TabIndex="19"  />
                                    </div> 
                                    <div class="col-md-4">
                                        <label for="usr">Qt Ajuste: </label>
                                        <asp:TextBox ID="txtQtAjuste" CssClass="form-control" Enabled="false" BackColor="Yellow" ForeColor="Red" Font-Bold="true" runat="server" TabIndex="20"  />
                                    </div>
                                </div>
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

   
</asp:Content>
