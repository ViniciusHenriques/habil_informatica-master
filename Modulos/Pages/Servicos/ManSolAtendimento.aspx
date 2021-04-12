<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManSolAtendimento.aspx.cs" Inherits="SoftHabilInformatica.Pages.Servicos.ManSolAtendimento" 
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

            $('.js-example-basic-single').select2({
            });
        });
        
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
             $('.js-example-basic-single').select2({
                language: $.extend({},
                $.fn.select2.defaults.defaults.language, {
                  noResults: function() {
                    var term = id_categoria
                      .data('select2')
                      .$dropdown.find("input[type='search']")
                      .val();

                    return $("Nenhum resultado");
                  }
                })
            });
            item = '#<%=PanelSelect%>';
            $(document).ready(function (e) {
                $('#myTabs a[href="' + item + '"]').tab('show');
                console.log($(item));


            });

        }
        
    </script>

    <style type="text/css">
        @media screen and (max-width: 600px) {
            .noprint{display:none;}  
            .total{
                display:none;
            }
            .real{
                border-bottom-left-radius:4px!important;
                border-top-left-radius:4px!important;
            }
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

         .NivelReacao{
             width:50px;
             height:50px;
             margin-right:10px;
         }
         .NivelReacao:hover{
             height:55px;
             width:55px;
         }
    </style>
   
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary   ">
            <div class="panel-heading  panel-heading-padrao " >
                Solicitação de Atendimento
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
                                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click" ToolTip="Ao voltar, TODAS alterações feitas no documento será perdida!"> 
                                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
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
                            <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                                <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                                <li role="presentation"><a href="#consulta2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Eventos</a></li>
                                <li role="presentation"><a href="#consulta3" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-check"></span>&nbsp;&nbsp;Logs</a></li>
                                <li role="presentation"><a href="#consulta4" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexos</a></li>
                            </ul>
                            <div class="tab-content" runat="server" id="PanelContext">                                
                                <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Lançamento</label>
                                                    <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="1"/>
                                                </div>
                                                <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Tipo de Solicitação</label>
                                                    <asp:DropDownList ID="ddlTipoSolicitacao" runat="server" Width="100%" TabIndex="2" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>
                                                <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Data Emissão</label>
                                                    <asp:TextBox ID="txtdtemissao" CssClass="form-control"  TabIndex="3" runat="server" name="txtdtemissao"  AutoPostBack="true" Enabled="false" />
                                                </div>                                   
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                    <asp:DropDownList ID="ddlEmpresa" runat="server"   TabIndex="4"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_TextChanged"   AutoPostBack="true"/>
                                                </div>
                                                <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Situação</label>
                                                    <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" TabIndex="5" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>  
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8" style="margin:0!important;padding:0!important">
                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtNroDocumento" AutoPostBack="False" CssClass="form-control"  TabIndex="6" runat="server" />
                                                    </div>
                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                        <label for="usr" style ="margin-top:1px;">Série</label>
                                                        <asp:TextBox ID="txtNroSerie" AutoPostBack="False" CssClass="form-control"  TabIndex="7" runat="server" />
                                                    </div>
                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                        <label for="usr" style ="margin-top:1px;">Data Conclusão</label>
                                                        <asp:TextBox ID="txtDtConclusao" AutoPostBack="False" CssClass="form-control" Enabled="false" TabIndex="8" runat="server" />
                                                    </div>
                                                     <div class="col-md-12" >
                                                        <label for="usr" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">Cliente</label>
                                                        <div class="input-group " style="width:100%">                              
                                                            <asp:TextBox ID="txtCodPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server" TabIndex="9" OnTextChanged="SelectedPessoa"  onFocus="this.select()" AutoPostBack="true"                                
                                                            Width="15%" MaxLength="6" />                                                      
                                                            <asp:LinkButton ID="btnPessoa" title="Pesquise os Clientes" runat="server" CssClass="form-control btn btn-primary " Width="10%"  OnClick="ConPessoa" AutoPostBack="true" TabIndex="10"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                            </asp:LinkButton>
                                                            <asp:TextBox ID="txtPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server"  Enabled="false"  Width="75%" />
                                                        </div>
                                                    </div>                                           
                                                </div>
                                                <div class="col-md-4" style="padding-top:15px">
                                                    <div class="panel panel-primary  " style="margin-bottom:0!important">
                                                        <div class="panel-heading panel-heading-padrao " style="text-align:center;height:30px;padding:5px!important">
                                                            Nível de Prioridade
                                                            <asp:Label runat="server" ID="lblNivel"></asp:Label>
                                                        </div>
                                                         <div class="panel-body" style="text-align:center;height:70px;padding-top:0!important">
                                                             <div class="col-md-1"></div>
                                                             <div class="col-md-10">
                                                                <div class="col-md-4 col-xs-4" style="background-color: white; border: none; text-align: left; font-size:x-small;margin-top:10px" title="Baixa">
                                                                    <asp:LinkButton runat="server" id="btnNivelBaixo" CssClass="btn-link" OnClick="btnNivelBaixo_Click"><img src="../../Images/nivelBaixo.png"  class="NivelReacao"/></asp:LinkButton>
                                                                </div>
                                                                <div class="col-md-4 col-xs-4" style="background-color: white; border: none; text-align: left; font-size:x-small;margin-top:10px" title="Média">
                                                                    <asp:LinkButton runat="server" ID="btnNivelMedio" CssClass="btn-link" OnClick="btnNivelMedio_Click"><img src="../../Images/nivelMedio.png" class="NivelReacao" /></asp:LinkButton>
                                                                </div>
                                                                <div class="col-md-4 col-xs-4" style="background-color: white; border: none; text-align: left; font-size:x-small;margin-top:10px" title="Alta">
                                                                    <asp:LinkButton runat="server" ID="btnNivelAlto" CssClass="btn-link" OnClick="btnNivelAlto_Click"><img src="../../Images/nivelAlto.png" class="NivelReacao"/></asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px;padding-left:0!important">
                                                    <div class="col-md-11">
                                                        <label for="usr" style ="margin-top:1px;">Contato</label>
                                                        <asp:DropDownList ID="ddlContato" runat="server" AutoPostBack="true" TabIndex="11" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnSelectedIndexChanged="ddlContato_SelectedIndexChanged"/>
                                                    </div>
                                                    <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small; margin-top:19px" title="Adicionar Contato">
                                               
                                                        <asp:LinkButton ID="BtnAddContato" runat="server" Text="Adicionar" TabIndex="12" AutoPostBack="true" CssClass="btn btn-success" OnClick="BtnAddContato_Click" style="height:33px!important;padding-top:7px" > 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Email Solicitante</label>
                                                    <asp:TextBox ID="txtEmail" AutoPostBack="False" CssClass="form-control"  TabIndex="13" runat="server" MaxLength="50" onFocus="this.select()"/>
                                                </div>
                                                <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Telefone Solicitante</label>
                                                    <div class="input-group ">
                                                        <span class="input-group-addon">+55</span>
                                                        <asp:TextBox ID="txtTelefone" AutoPostBack="False" CssClass="form-control"  TabIndex="14" MaxLength="11" runat="server" onFocus="this.select()"/>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="panel panel-primary   " style="margin-bottom:0!important">
                                                        <div class="panel-heading panel-heading-padrao" style="text-align:center;height:30px;padding:5px!important">
                                                            Orçamento
                                                        </div>
                                                        <div class="panel-body" style="text-align:center;height:70px;padding-top:0!important">
                                                            <div class="col-md-12">
                                                                    <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                                    <label for="usr" style ="margin-top:1px;">Horas Previstas</label>
                                                                    <asp:TextBox ID="txtHRSPrevistas" AutoPostBack="true" CssClass="form-control"  TabIndex="15" runat="server" MaxLength="50" OnTextChanged="txtHRSPrevistas_TextChanged" onFocus="this.select()"/>
                                                                </div>
                                                                    <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                                    <label for="usr" style ="margin-top:1px;">Valor Total</label>
                                                                    <asp:TextBox ID="txtValorTotal" AutoPostBack="true" CssClass="form-control"  TabIndex="16" runat="server" MaxLength="50" OnTextChanged="txtValorTotal_TextChanged" onFocus="this.select()"/>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                           
                                           <div class="row">         
                                               <div class="col-md-12" style="margin-top:15px">
                                                   <div class="panel panel-default">
                                                        <div class="panel-heading" >
                                                            
                                                            <asp:LinkButton runat="server" title="Editar Descrição" ID="BtnEditarDS" OnClick="BtnEditarDS_Click" CssClass="btn btn-info" style="height:33px; margin-right:20px;float:left;margin-top:-8px" TabIndex="17"><span class=" glyphicon glyphicon-pencil"></span></asp:LinkButton>                                  
                                                            Descrição da Solicitação
                                                        </div>
                                                        <div class="panel-body" >
                                                           <asp:Literal runat="server" ID="litDescricao" ></asp:Literal>
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
                                            <asp:AsyncPostBackTrigger ControlID="ddlContato" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtHRSPrevistas" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtValorTotal" EventName="TextChanged" />

                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>                                
                               
                                <div role="tabpanel" class="tab-pane" id="consulta2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:GridView ID="GrdEventoDocumento" runat="server" AutoPostBack="false"
                                                CssClass="table table-hover table-striped"
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
                                            <asp:GridView ID="grdLogDocumento" runat="server" CssClass="table  table-hover table-striped"
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
                                            <asp:LinkButton ID="btnNovoAnexo" runat="server" style="padding-left:20px; margin-bottom:10px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovoAnexo_Click" TabIndex="21"> 
                                                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span> Novo Anexo &nbsp;
                                            </asp:LinkButton>
                                            <br />
                                            <asp:GridView ID="grdAnexo" runat="server"
                                                CssClass="table  table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt" Visible="true"
                                                OnSelectedIndexChanged="grdAnexo_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoAnexo" HeaderText="#"  />
                                                    <asp:BoundField DataField="DataHoraLancamento" HeaderText="Dt. Lanc"  />
                                                    <asp:BoundField DataField="Cpl_Maquina" HeaderText="Estação" />
                                                    <asp:BoundField DataField="Cpl_Usuario" HeaderText="Usuário"  />
                                                    <asp:BoundField DataField="DescricaoArquivo" HeaderText="Descrição"/>
                                                    <asp:BoundField DataField="NomeArquivo" HeaderText="Nome"/>
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
                            Deseja Excluir este registro ?
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
