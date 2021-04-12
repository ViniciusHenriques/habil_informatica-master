<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ManCTe.aspx.cs" Inherits="SoftHabilInformatica.Pages.Transporte.ManCTe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            $('.js-example-basic-single').select2({
            });
        });
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({
                    
            });
            $('#myexcpes2').modal('hide');
        }
    </script>
    <body>
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
        <div class="panel panel-primary" >
            <div  class="panel-heading">Manutenção CT-e
                <div class="messagealert" id="alert_container"></div>
            </div>            
            <div class="panel-body">
                <div class="row" style="background-color: white; border: none;">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click" ToolTip="Ao voltar, TODAS alterações feitas neste documento será perdida, inclusive novos eventos e anexos!"> 
                                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes"> 
                                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                                </asp:LinkButton>     
                                <asp:LinkButton ID="BtnEnviarDesacordo" runat="server"  CssClass="btn btn-warning" data-toggle="modal" data-target="#myexcpes2"  title="Enviar Desacordo do CTe" > 
                                    <span aria-hidden="true"  class="glyphicon glyphicon-upload"></span> Enviar Desacordo
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSalvar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none; margin-top: 15px!important">
                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;">
                            <ul class="nav nav-tabs" role="tablist" id="myTabs">
                                <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                                <li role="presentation" style="<%=Panels%>"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Eventos Eletrônicos&nbsp;&nbsp;</a></li>
                                <li role="presentation" style="<%=Panels%>"><a href="#consulta4" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexos </a></li>
                            </ul>
                            <div class="tab-content" runat="server" id="PanelContext">
                                <div role="tabpanel" class="tab-pane" id="home" style="font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                        <ContentTemplate>  
                                            <div class="col-md-3 TextBoxFormatado" >
                                                <label for="usr" style="margin-top: 1px;">Código</label>
                                                <asp:TextBox ID="txtCodigo" CssClass="form-control" TabIndex="1" runat="server"  Enabled="false" />               
                                            </div>
                                            <div class="col-md-3 TextBoxFormatado">
                                                <label for="usr" style="margin-top: 1px;">Data/Hora Lançamento</label>
                                                <asp:TextBox ID="txtdtLancamento" CssClass="form-control" TabIndex="2" runat="server" name="txtdtemissao" Enabled="false" />
                                            </div>
                                            <div class="col-md-3 TextBoxFormatado">
                                                <label for="usr" style="margin-top: 1px;">Data/Hora Emissão</label>
                                                <asp:TextBox ID="txtdtemissao" CssClass="form-control" TabIndex="3" runat="server" name="txtdtemissao" Enabled="false" />
                                            </div>
                                            <div class="col-md-3 TextBoxFormatado">
                                                <label for="usr" style="margin-top: 1px;">Situação</label>
                                                <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" TabIndex="4" CssClass="form-control js-example-basic-single" Font-Size="Small" Enabled="false"/>
                                            </div>
                                            <div class="col-md-6" style="background-color:white; border: none; text-align: left; font-size:x-small;margin-top:3px">
                                                <label for="usr" style ="margin-top:1px;">Tipo Operação</label>
                                                <asp:DropDownList ID="ddlTipoOperacao" runat="server"   TabIndex="5"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" />
                                            </div>
                                            <div class="col-md-6 TextBoxFormatado">
                                                <label for="usr" style="margin-top: 1px;">Empresa</label>
                                                <asp:DropDownList ID="ddlEmpresa" runat="server" TabIndex="6" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_TextChanged" AutoPostBack="true" />
                                            </div>                                                   
                                            <div class="col-md-3 TextBoxFormatado" >
                                                <label for="usr" style="margin-top: 1px;">Número Documento</label>
                                                <asp:TextBox ID="txtNroDocumento" AutoPostBack="False" CssClass="form-control" TabIndex="7" runat="server" Enabled="false" />
                                            </div>
                                            <div class="col-md-3 TextBoxFormatado" >
                                                <label for="usr" style="margin-top: 1px;">Série</label>
                                                <asp:TextBox ID="txtNroSerie" AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" Enabled="false" />
                                            </div>
                                            <div class="col-md-6">
                                                <label for="usr" class="TextBoxFormatado">Transportador <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <div class="input-group " style="width: 100%">
                                                    <asp:TextBox ID="txtCodTransportador" CssClass="form-control"  runat="server" TabIndex="9" OnTextChanged="txtCodTransportador_TextChanged" AutoPostBack="true"
                                                        Width="15%" MaxLength="6" onFocus="this.select()" />
                                                    <asp:LinkButton ID="btnTransportador" title="Pesquise os Transportadores" runat="server" CssClass="form-control btn btn-primary" Width="10%" OnClick="btnTransportador_Click" AutoPostBack="true" TabIndex="10"> 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtTransportador" CssClass="form-control" runat="server" Enabled="false" Width="75%" />
                                                </div>
                                            </div>
                                            <div class="col-md-12 TextBoxFormatado" style="margin-top:-5px">
                                                <label for="usr" style="margin-top: 1px;">Chave de Acesso <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <asp:TextBox ID="txtChaveAcesso" CssClass="form-control" TabIndex="11" runat="server"  onFocus="this.select()" MaxLength="45" />               
                                            </div> 
                                            <div class="col-md-6">
                                                <label for="usr" class="TextBoxFormatado">Remetente <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <div class="input-group " style="width: 100%">
                                                    <asp:TextBox ID="txtCodRemetente" CssClass="form-control" runat="server" TabIndex="12" OnTextChanged="txtCodRemetente_TextChanged" AutoPostBack="true"
                                                        Width="15%" MaxLength="6" onFocus="this.select()" />
                                                    <asp:LinkButton ID="btnRemetente" title="Pesquise as Pessoas" runat="server" CssClass="form-control btn btn-primary " Width="10%" OnClick="btnRemetente_Click" AutoPostBack="true" TabIndex="13"> 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtRemetente" CssClass="form-control" runat="server" Enabled="false" Width="75%" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label for="usr" class="TextBoxFormatado">Destinatário <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <div class="input-group " style="width: 100%">
                                                    <asp:TextBox ID="txtCodDestinatario" CssClass="form-control" runat="server" TabIndex="14" OnTextChanged="txtCodDestinatario_TextChanged" AutoPostBack="true"
                                                        Width="15%" MaxLength="6" onFocus="this.select()" />
                                                    <asp:LinkButton ID="btnDestinatario" title="Pesquise as Pessoas" runat="server" CssClass="form-control btn btn-primary " Width="10%" OnClick="btnDestinatario_Click" AutoPostBack="true" TabIndex="15"> 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtDestinatario" CssClass="form-control" runat="server" Enabled="false" Width="75%" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label for="usr" class="TextBoxFormatado" style="margin-top:-5px">Tomador <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <div class="input-group " style="width: 100%">
                                                    <asp:TextBox ID="txtCodTomador" CssClass="form-control" runat="server" TabIndex="16" OnTextChanged="txtCodTomador_TextChanged" AutoPostBack="true"
                                                        Width="15%" MaxLength="6" onFocus="this.select()" />
                                                    <asp:LinkButton ID="btnTomador" title="Pesquise as Pessoas" runat="server" CssClass="form-control btn btn-primary " Width="10%" OnClick="btnTomador_Click" AutoPostBack="true" TabIndex="17"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtTomador" CssClass="form-control" runat="server" Enabled="false" Width="75%" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label for="usr" class="TextBoxFormatado" style="margin-top:-5px">Recebedor <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <div class="input-group " style="width: 100%">
                                                    <asp:TextBox ID="txtCodRecebedor" CssClass="form-control" runat="server" TabIndex="18" OnTextChanged="txtCodRecebedor_TextChanged" AutoPostBack="true"
                                                        Width="15%" MaxLength="6" onFocus="this.select()" />
                                                    <asp:LinkButton ID="btnRecebedor" title="Pesquise as Pessoas" runat="server" CssClass="form-control btn btn-primary " Width="10%" OnClick="btnRecebedor_Click" AutoPostBack="true" TabIndex="19"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtRecebedor" CssClass="form-control" runat="server" Enabled="false" Width="75%" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-offset-8 TextBoxFormatado" style="margin-top:-5px">
                                                <label for="usr" style="margin-top: 1px;">Valor do CT-e <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <div class="input-group ">
                                                    <span class="input-group-addon">R$</span>
                                                    <asp:TextBox ID="txtValor" CssClass="form-control" TabIndex="20" runat="server" onFocus="this.select()" OnTextChanged="txtValor_TextChanged" MaxLength="18"/>    
                                                </div>                                       
                                            </div>
                                            <div class="col-md-12 TextBoxFormatado" style="margin-top:-5px">
                                                <label for="usr" style="margin-top: 1px;">Observação</label>
                                                <asp:TextBox ID="txtOBS" CssClass="form-control" runat="server"  TabIndex="21" TextMode="multiline" Columns="10" Rows="5"  onFocus="this.select()" />        
                                            </div>
                                            </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCodigo"  />
                                            <asp:AsyncPostBackTrigger ControlID="ddlSituacao" />
                                            <asp:AsyncPostBackTrigger ControlID="txtValor" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtOBS" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCodTransportador" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtTransportador" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCodDestinatario" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtDestinatario" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCodTomador" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtTomador" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCodRecebedor" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCodRemetente" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtRemetente" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtRecebedor" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtChaveAcesso" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta" style="font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                        <ContentTemplate> 
                                            <%--<asp:LinkButton ID="btnEnviarDesacordo" runat="server" Text="Excluir" CssClass="btn btn-warning" OnClick="btnEnviarDesacordo_Click"> 
                                                <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-barcode"></span>  Enviar Desacordo
                                            </asp:LinkButton>--%>
                                            <asp:LinkButton ID="btnAddEvento" runat="server" Style="margin-bottom: 10px" Text="Excluir" CssClass="btn btn-success" OnClick="btnAddEvento_Click" title="Adicionar evento eletrônico ao CT-e"> 
                                                <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>  Adicionar Evento
                                            </asp:LinkButton>
                                            
                                            <asp:LinkButton ID="btnRefresh" runat="server" Style="margin-bottom: 10px" CssClass="btn btn-default" OnClick="btnRefresh_Click" Height="34"  title="Atualizar" > 
                                                <span aria-hidden="true" title="Atualizar" class="glyphicon glyphicon-refresh"></span> 
                                            </asp:LinkButton>

                                            <br />
                                            <asp:GridView ID="grdEventoEletronico" runat="server"
                                                    CssClass="table table-striped"
                                                    GridLines="None" AutoGenerateColumns="False"
                                                    Font-Size="8pt"  Visible="true"
                                                    OnSelectedIndexChanged="grdEventoEletronico_SelectedIndexChanged">
                                                
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoEvento" HeaderText="#" />
                                                    <asp:BoundField DataField="Cpl_TipoEvento" HeaderText="Tipo Evento" />
                                                    <asp:BoundField DataField="NumeroSequencia" HeaderText="Sequência" />
                                                    <asp:BoundField DataField="DataHoraEvento" HeaderText="Data" />
                                                    <asp:BoundField DataField="Cpl_NomeMaquina" HeaderText="Máquina" />
                                                    <asp:BoundField DataField="Cpl_NomeUsuario" HeaderText="Usuário" />
                                                    <asp:BoundField DataField="Motivo" HeaderText="Motivo" />     
                                                    <asp:BoundField DataField="Retorno" HeaderText="Retorno" />   
                                                    <asp:TemplateField HeaderText="Situação" ItemStyle-CssClass="centerVertical col-md-1">
                                                        <ItemTemplate>
                                                            <label class="badge <%# Eval("Cpl_Situacao").ToString() %>" id="situacaoSpan"><%# Eval("Cpl_Situacao").ToString() %></label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                                        ItemStyle-Height="15px" ItemStyle-Width="50px"
                                                        ButtonType="Image" SelectImageUrl="~/Images/Acessar.svg"
                                                        ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>                              
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta4" style="font-size: small;">
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btnNovoAnexo" runat="server" Style="margin-bottom: 10px" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovoAnexo_Click"> 
                                                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span> Novo Anexo 
                                            </asp:LinkButton>
                                            <br />
                                            <asp:GridView ID="grdAnexo" runat="server"
                                                    CssClass="table table-striped"
                                                    GridLines="None" AutoGenerateColumns="False"
                                                    Font-Size="8pt"  Visible="true"
                                                    OnSelectedIndexChanged="grdAnexo_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoAnexo" HeaderText="#" />
                                                    <asp:BoundField DataField="DataHoraLancamento" HeaderText="Data/Hota Lançamento" />
                                                    <asp:BoundField DataField="Cpl_Maquina" HeaderText="Estação" />
                                                    <asp:BoundField DataField="Cpl_Usuario" HeaderText="Usuário" />
                                                    <asp:BoundField DataField="DescricaoArquivo" HeaderText="Descrição" />
                                                    <asp:BoundField DataField="NomeArquivo" HeaderText="Nome" />
                                                    <asp:BoundField DataField="ExtensaoArquivo" HeaderText="Extensão" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" />
                                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                                        ItemStyle-Height="15px" ItemStyle-Width="50px"
                                                        ButtonType="Image" SelectImageUrl="~/Images/Acessar.svg"
                                                        ControlStyle-Width="25px" ControlStyle-Height="25px" />
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
                            Deseja Excluir este Registro ?
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:LinkButton>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="myexcpes2" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" style="height: 100%; width: 500px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" >Atenção</h4>
                        </div>
                        <div class="modal-body">
                            Para o envio do evento, o documento precisa ser salvo!!!</br>
                            Deseja salvar?
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSimSalvar" OnClientClick="this.disabled = true; this.value = 'Salvando...';" UseSubmitBehavior="false" OnClick="btnSimSalvar_Click" CssClass="btn btn-danger" style="color:white" Text="Sim" />
<%--                            <asp:LinkButton ID="btnSimSalvar" runat="server" Text="Sim" CssClass="btn btn-success" TabIndex="-1" AutoPostBack="true" OnClick="btnSimSalvar_Click" ></asp:LinkButton>--%>
                            <button type="button" class="btn btn-default" data-dismiss="modal" id="NaoSalvar">Não</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCfmSim" />
            <asp:PostBackTrigger ControlID="btnSimSalvar" />
        </Triggers>
    </asp:UpdatePanel>

</body>
</asp:Content>
