 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ManAgendamento.aspx.cs" 
     Inherits="SoftHabilInformatica.Pages.Compromissos.ManAgendamento"  MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts//moment.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/moment-with-locales.js"></script>
    <script src="../../Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="../../Content/bootstrap-datetimepicker.min.css">
    <link href='../../Content/Calendar_events_files/main.css' rel='stylesheet' />
    <script src='../../Content/Calendar_events_files/main.js'></script>

    <link rel="stylesheet" href="../../Content/style_timeline.css">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>
        function ModalUsuarios() {
            $("#modalUsers").modal("show");
        }

        function FecharModalUsuarios(){
            $("#modalUsers").modal("hide");
        }

        function ModalHistorico(title) {
            $("#modalHistorico .modal-title-historico").html(title);
            $("#modalHistorico").modal("show");
            
        }

        function FecharModalHistorico(){
            $("#modalHistorico").modal("hide");
        }
        function ReplaceAll() {
            var anotacao = document.getElementById('<%=txtAnotacao.ClientID%>').value;
            document.getElementById('<%=txtAnotacao.ClientID%>').value = anotacao.replaceAll("<", "[").replaceAll(">", "]");
        }
        function MostrarCalendario() {
            $("#modalCalendario").modal("show");

            document.addEventListener('DOMContentLoaded', function () {
                var initialTimeZone = 'local';
                var timeZoneSelectorEl = document.getElementById('time-zone-selector');
                var calendarEl = document.getElementById('calendar');

                var calendar = new FullCalendar.Calendar(calendarEl, {
                    timeZone: initialTimeZone,
                    headerToolbar: {
                        left: 'prev,next today',
                        center: 'title'
                    },
                    initialDate: '<%= DateTime.Now.ToString("yyyy-MM-dd")%>',
                    locale: "pt-br",
                    navLinks: false, // can click day/week names to navigate views
                    selectable: true,
                    selectMirror: true,
                    editable: false,
                    select: function (arg) {
                        var event = new Date(arg.start);
                        document.getElementById("<%=txtData.ClientID%>").value = event.toLocaleDateString("pt-br");
                        $get('<%=txtData.ClientID%>').onchange();
                        $("#modalCalendario").modal("hide");
                    },

                    dayMaxEvents: true, // allow "more" link when too many events
                    events: [
                        <%=Agendamentos%>   
                    ],
                });

                calendar.render();

            });
        }
        function EscolherCor(Cor){
            //alert(Cor);
            $get('<%=btnCor.ClientID%>').click(); 

        }

        $(document).ready(function (e) {
            $('.datetimepicker3').datetimepicker({ locale: 'pt-br', format: 'HH:mm' });
            $('.js-example-basic-single').select2({});
        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.datetimepicker3').datetimepicker({ locale: 'pt-br', format: 'HH:mm' });
            $('.js-example-basic-single').select2({});
        }

        $(document).on('keydown', function (e) {
            if ((e.altKey) && (e.which === 67)) { 
                $get('<%=btnCalendario.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 83)) { 
                $get('<%=btnSalvar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 82)) { 
                $get('<%=BtnReativar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 86)) { 
                $get('<%=btnVoltar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 78)) { 
                $get('<%=btnNovo.ClientID%>').click(); 
            }
            else if ((e.which === 116)) { 
                $get('<%=btnNovo.ClientID%>').click(); 
            }
        });

        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
        function FazerDownload(CodigoAgendamento, CodigoAnexo) {
            $get('<%=txtDownloadAnexo.ClientID%>').value = CodigoAgendamento + "³" + CodigoAnexo;
            $get('<%=txtDownloadAnexo.ClientID%>').onchange();
            
        }
    </script>
    <style>
        .TarefaConcluida{
            opacity:0.4
        }
        .style-content-h4{
            background-color:rgba(000,000,000,0.5);
            border-radius:10px!important;
            margin:5px!important;
            padding:10px;
        }

        .ButtonGrid{
            padding:0!important;           
        }
        .buttonAnotacao{
            padding:4px!important;
        }
        .divCorAnotacao{
            padding:10px!important;
        }
        .ColunaAnotacao{
            width:10px!important;
        }
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, Helvetica Neue, Helvetica, sans-serif;
            font-size: 14px;
        }

        #top {
            background: #eee;
            border-bottom: 1px solid #ddd;
            padding: 0 10px;
            line-height: 40px;
            font-size: 12px;
        }
        .left { float: left }
        .right { float: right }
        .clear { clear: both }

        #script-warning, #loading { display: none }
        #script-warning { font-weight: bold; color: red }

        #calendar {
            max-width: 1100px;
            margin: 40px auto;
            padding: 0 10px;
        }

        .tzo {
            color: #000;
        }
        .arredondamento{
            border-radius: 50%;
        }
        @media screen and (max-width: 500px) {
            .no-print {
                display:none;
            }
        }
        .no-print2{
            display:none;
        }
    </style>
    <body>
        <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
            <div class="panel panel-primary" >
                <div  class="panel-heading">Agendamento de Compromisso
                    <div class="messagealert" id="alert_container"></div>
                </div>            
                <div class="panel-body">
                    <div class="row">   
                        <div class="col-md-12 col-xs-12">
                            <div class="col-md-1 col-xs-5 ">
                                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click" ToolTip="Novo ( Alt + V )" > 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-save"></span>  Voltar
                                </asp:LinkButton>

                            </div>
                            <div class="col-md-3  col-xs-5 " style="padding-right:0!important;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="input-group">
                                            <span class="input-group-addon">Data:</span>
                                            <asp:TextBox ID="txtData" CssClass="form-control" runat="server" OnTextChanged="txtData_TextChanged" AutoPostBack="true" />
                                        </div>
                                    </ContentTemplate> 
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="txtData" EventName="TextChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-5 col-xs-2"  style="padding-left:0!important;">
                                <asp:LinkButton runat="server" OnClick="btnCalendario_Click" ID="btnCalendario" CssClass="btn btn-default" Style="height:33px" ToolTip="Novo ( Alt + C )" TabIndex="1">
                                    <span aria-hidden="true" title="Calendário" class="glyphicon glyphicon-calendar" ></span>            
                                </asp:LinkButton>

                            </div>
                        </div>
                        <div class ="col-md-12 col-xs-12" style="margin-top:15px">
                            <div class="col-md-6">
                                <div class ="panel panel-primary" >
                                    <div class="panel-heading" style="height:42px;padding:4px!important;" >
                                        <div class="col-md-6 col-xs-6" style="padding:6px">Detalhes do agendamento</div>
                                        <div class="col-md-6 col-xs-6" >
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <div class="col-md-10 col-xs-8" style="padding-right:0!important">
                                                        <asp:DropDownList ID="ddlTipoCompromisso" runat="server" Width="100%" TabIndex="9" CssClass="form-control js-example-basic-single"  Font-Size="Small" OnSelectedIndexChanged="ddlTipoCompromisso_SelectedIndexChanged" AutoPostBack="true"/>
                                                    </div>
                                                    <div class="col-md-2 col-xs-4" style="padding-left:0!important">
                                                        <asp:LinkButton runat="server" OnClick="btnUsuario_Click" ID="btnUsuario" CssClass="btn btn-default" Style="height:33px;padding:7px" ToolTip="Usuários com permissão de acesso" TabIndex="1">
                                                            <span class="glyphicon glyphicon-user" ></span>
                                                            <span class="glyphicon glyphicon-user" style="margin-left:-10px;margin-top:-10px!important"></span>
                                                            <span class="glyphicon glyphicon-user" style="margin-left:-10px"></span>          
                                                        </asp:LinkButton>
                                                    </div>
                                                </ContentTemplate> 
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlTipoCompromisso" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnUsuario" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="panel-body" >
                                        <div class="col-md-6" style="margin-top:10px"> 
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="btnNovo" runat="server" CssClass="btn btn-info" TabIndex="0" OnClick="btnNovo_Click" Visible="true" ToolTip="Novo ( Alt + N )"> 
                                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-edit"></span>  Novo
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnSalvar" runat="server" CssClass="btn btn-success" TabIndex="0" OnClick="btnSalvar_Click" Visible="true" ToolTip="Salvar ( Alt + S )"> 
                                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                                    </asp:LinkButton>
                                            
                                                    <asp:LinkButton ID="BtnReativar" runat="server" CssClass="btn btn-warning" TabIndex="0" OnClick="BtnReativar_Click" Visible="false" ToolTip="Reativar ( Alt + R )"> 
                                                        <span aria-hidden="true" title="Reativar" class="glyphicon glyphicon-ok"></span>  Reativar
                                                    </asp:LinkButton> 
                                                    <asp:LinkButton ID="btnConcluir" title="Concluir tarefa" runat="server" CssClass="btn alert-info" TabIndex="0" OnClick="BtnConcluir_Click" Visible="false" height="34"> 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-ok"></span>
                                                    </asp:LinkButton> 
                                                </ContentTemplate> 
                                            </asp:UpdatePanel>

                                        </div>
                                         <div class="col-md-6" style="text-align:center;margin-top:5px" >
                                             <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <div class=" panel panel-primary"  >
                                                        <div class="panel-heading" style="padding:4px;">
                                                                Cor do Lembrete (<asp:Label runat="server" ID="lblCorLembrete" AutoPostBack="true" ></asp:Label>)
                                                            <asp:LinkButton ID="btnCor" OnClick="txtOutraCor_TextChanged"  AutoPostBack="true"  runat="server" />
                                                        </div>
                                                        <div class="panel-body" style="text-align:center;padding:4px;">
                                                            <asp:LinkButton runat="server" ID="btnCinza" CssClass="btn btn-link " Height="25px" BackColor="Gray" OnClick="btnCinza_Click"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="btnVerde" CssClass="btn btn-link " Height="25px" BackColor="Green" OnClick="btnVerde_Click"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="btnAzul" CssClass="btn btn-link " Height="25px" BackColor="SkyBlue" OnClick="btnAzul_Click"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="btnAmarelo" CssClass="btn btn-link " Height="25px" BackColor="Goldenrod" OnClick="btnAmarelo_Click"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="btnVermelho" CssClass="btn btn-link " Height="25px" BackColor="red" OnClick="btnVermelho_Click"></asp:LinkButton>
                                                            <asp:TextBox ID="txtOutraCor" type="color" CssClass="btn btn-link" runat="server" Font-Size="Small" onchange="EscolherCor(this.value);" Height="33px"  Width="28px" Style="padding:0!important;border:0!important" />
                                                        </div>
                                                    </div>
                                                </ContentTemplate> 
                                            </asp:UpdatePanel>
                                        </div>
                                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;">
                                            <ul class="nav nav-tabs" role="tablist" id="myTabs">
                                                <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                                                <li role="presentation"><a href="#consulta4" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexos </a></li>
                                            </ul>
                                            <div class="tab-content" runat="server" id="PanelContext">
                                                <div role="tabpanel" class="tab-pane" id="home" style="font-size: small;">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <div class="col-md-4" style="font-size:x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Lançamento</label>
                                                                <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small"  MaxLength="10"/>
                                                            </div>
                                                            <div class="col-md-4" style="font-size:x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Horário  <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                                <asp:TextBox ID="txtHora" CssClass="form-control datetimepicker3" runat="server" Text ="" Font-Size="Small" TabIndex="2" MaxLength="10"/>
                                                            </div>
                                                            <div class="col-md-4" style="font-size:x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Situação</label>
                                                                <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                            </div> 
                                                
                                                            <div class="col-md-12" >
                                                                <label for="usr" style="font-size: x-small;margin-top:3px">Cliente <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                                <div class="input-group " style="width:100%">                              
                                                                    <asp:TextBox ID="txtCodPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server" TabIndex="3" OnTextChanged="txtCodPessoa_TextChanged"   AutoPostBack="true" onFocus="this.select()"                                
                                                                        Width="15%" MaxLength="6" />                                                      
                                                                    <asp:LinkButton ID="btnPessoa" title="Pesquise os Clientes" runat="server" CssClass="form-control btn btn-primary" Width="10%"  OnClick="btnPessoa_Click" AutoPostBack="true" TabIndex="4"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:TextBox ID="txtPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server"  Enabled="false"  Width="60%"  />
                                                                    <asp:LinkButton ID="btnHistorico" runat="server" CssClass="btn btn-default" TabIndex="0" OnClick="btnHistorico_Click" Visible="true"  Width="15%"  AutoPostBack="true" Height="34px" style="font-size:20px;border-bottom-left-radius:0;border-top-left-radius:0" ToolTip="Histórico do cliente">
                                                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-eye-open"></span>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>  
                                                
                                                            <div class="col-md-12" style="font-size:x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Telefone <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                                <asp:TextBox ID="txtTelefone" CssClass="form-control" runat="server" Text ="" Font-Size="Small" TabIndex="5" MaxLength="50"/>
                                                            </div>
                                               
                                                            <div class="col-md-12" style="font-size:x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Local <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                                <asp:TextBox ID="txtLocal" CssClass="form-control" runat="server" Text ="" Font-Size="Small" TabIndex="6" MaxLength="100" OnTextChanged="txtAnotacao_TextChanged" AutoPostBack="true" > 
                                                                </asp:TextBox>
                                                            </div>
                                                            <div class="col-md-10" style="font-size:x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Contato </label>
                                                                <asp:TextBox ID="txtContato" CssClass="form-control" runat="server"  MaxLength="100"  onFocus="this.select()" />        
                                                            </div>
                                                             <div class="col-md-2" style="font-size:x-small;text-align:center">
                                                                <label for="usr" style ="margin-top:1px;">Enviar e-mail</label>
                                                                <asp:CheckBox ID="chkEmail" runat="server" Enabled="true" CssClass="form-control "  Font-Size="Small" Checked="true"/>
                                                            </div>
                                                 
                                                            <div class="col-md-12" style="font-size:x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Anotação  <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                                <asp:TextBox ID="txtAnotacao" CssClass="form-control" runat="server"  TabIndex="8" TextMode="multiline" Columns="10" Rows="7" MaxLength="50"  onFocus="this.select()" onchange="ReplaceAll()" />        
                                                            </div>
                                                        </ContentTemplate> 
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtCodPessoa" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnCor" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnHistorico" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div role="tabpanel" class="tab-pane" id="consulta4" style="font-size: small;">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
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
                                                                    <asp:BoundField DataField="Cpl_Usuario" HeaderText="Usuário" />
                                                                    <asp:BoundField DataField="DescricaoArquivo" HeaderText="Descrição" />
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
                            <div class="col-md-6">
                                <div class=" panel panel-primary" style="width:100%" >
                                    <div class="panel-heading">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                Agendamentos - <%=txtData.Text %>
                                            </ContentTemplate> 
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="panel-body" style="text-align:center">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:Label ID="lblNenhumAgendamento" runat="server" Visible="false">Nenhum agendamento na data selecionada</asp:Label>
                                                <asp:GridView ID="grdAgendamento" runat="server" Width="100%" 
                                                    AutoGenerateColumns="False" CssClass="table table-hover"
                                                    GridLines="None" 
                                                    Font-Size="8pt" 
                                                    OnRowCommand="grdAgendamento_RowCommand">

                                                <Columns>
                                                    <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText no-print" ItemStyle-CssClass="divCorAnotacao no-print">
                                                        <ItemTemplate>
                                                            <div style="width:20px;padding-top:3px;height:20px;color:white;background-color:<%# Eval("CorLembrete")%>" class="arredondamento" >
                                                                <%# Eval("CodigoIndex")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CodigoIndex" HeaderText="Hora" ItemStyle-Width = "3%" ItemStyle-CssClass="no-print2" HeaderStyle-CssClass="no-print2"/>            
                                                    <asp:BoundField DataField="DataHoraAgendamento" HeaderText="Hora" DataFormatString="{0:HH:mm}" ItemStyle-Width = "3%"/>            
                                                    <asp:BoundField DataField="Cpl_NomeCliente" HeaderText="Cliente" ItemStyle-Width = "10%"/>
                                                    <asp:BoundField DataField="Cpl_NomeUsuario" HeaderText="Usuário" ItemStyle-Width = "10%"/>
                                                    <asp:TemplateField HeaderText="Situação" ItemStyle-CssClass="centerVertical ">
                                                        <ItemTemplate>
                                                            <label class="badge <%# Eval("Cpl_DsSituacao").ToString() %> "><%# Eval("Cpl_DsSituacao").ToString() %></label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Anotação" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText no-print" ItemStyle-CssClass="buttonAnotacao no-print">
                                                        <ItemTemplate>
                                                            <div class="dropdown">  
                                                                <asp:LinkButton runat="server" type="button" class="btn btn-default" data-toggle="dropdown" >
                                                                    <span class="glyphicon glyphicon-eye-open"></span></span></span>
                                                                </asp:LinkButton>
                                                                <div class="dropdown-menu dropdown-menu-right " style="width:300px!important">
                                                                    <div class="col-md-12" >
                                                                        <p><%# Eval("Anotacao").ToString() %></p>
                                                                    </div>
                                                                </div>
                                                            </div> 
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="ButtonGrid" ItemStyle-Width = "10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-link " CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  Visible='<%# Eval("BtnEditar") %>' >
                                                                <img runat="server" src="../../Images/Editar.png" width="20" height="20"/>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="ButtonGrid" ItemStyle-Width = "10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-link " CommandName="Cancelar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  Visible='<%# Eval("BtnCancelar") %>' >
                                                                <img runat="server" src="../../Images/excluir.png" width="20" height="20"/>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                </asp:GridView>
                                            </ContentTemplate> 
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="modal fade" id="modalCalendario" role="dialog" aria-labelledby="myLargeModal">
                    <div class="modal-dialog" role="document" style="width:90%;margin-top:5%">
                        <div class="modal-content" style="width:100%;height:170%">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">Escolha a data do agendamento</h4>
                            </div>
                            <div class="modal-body" >
                                <div id='calendar'></div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate> 
        </asp:UpdatePanel>
        
        <div class="modal fade" id="modalUsers" role="dialog" aria-labelledby="myLargeModal">
            <div class="modal-dialog" role="document" style="width:80%;margin-top:5%">
                <div class="modal-content" style="width:100%;height:100%">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Usuário(s) permitido(s) a acessar esse compromisso</h4>
                        
                    </div>
                    <div class="modal-body" >
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblModal"></asp:Label>
                                <div class="col-md-11" style="font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Usuários</label>
                                    <asp:DropDownList ID="ddlUsuario" runat="server" Width="100%" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                </div> 
                                <div class="col-md-1" style=" margin-top: 20px">
                                    <asp:LinkButton ID="BtnAddUser" title="Adicionar Usuário" runat="server" CssClass="btn btn-success" OnClick="BtnAddUser_Click" Style="height: 33px!important; padding-top: 7px;color:white"> 
                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-md-12" style="font-size:x-small;margin-top:20px">
                                    <asp:GridView ID="grdUsuario" runat="server" Width="100%" 
                                            AutoGenerateColumns="False" CssClass="table table-hover"
                                            GridLines="None" 
                                            Font-Size="8pt" 
                                            OnSelectedIndexChanged="grdUsuario_SelectedIndexChanged">
                                        <Columns>
                                            <asp:BoundField DataField="CodigoUsuario" HeaderText="Código" />
                                            <asp:BoundField DataField="Cpl_NomeUsuario" HeaderText="Nome Completo" />

                                            <asp:CommandField HeaderText="Remover" ShowSelectButton="True" 
                                                            ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                            ButtonType="Image"  SelectImageUrl ="~/Images/Excluir.png" 
                                                            ControlStyle-Width ="25px" ControlStyle-Height ="25px" />                      

                                        </Columns>
                                        <RowStyle CssClass="cursor-pointer" />
                                    </asp:GridView> 
                                </div>  
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnAddUser" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalHistorico" role="dialog" aria-labelledby="myLargeModal">
            <div class="modal-dialog" role="document" style="width:95%;">
                <div class="modal-content" >
                    <div class="modal-header" style="width:100%">
                        <div class="row">
                            <div class="col-md-12" style="font-size:x-small;">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title-historico">Histórico do cliente</h4>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="col-md-12" style="font-size:x-small;">
                                        <asp:TextBox ID="txtPesquisar" CssClass="form-control" runat="server" OnTextChanged="txtPesquisar_TextChanged" TabIndex="8" Placeholder="Pesquisar! No minimo 3 Caracteres ..." MaxLength="50"  onFocus="this.select()" AutoPostBack="true"/>        
                                    </div>
                                </ContentTemplate> 
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtPesquisar" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-body" style="width:100%">
                        <div class="row">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:TextBox runat="server" ID="txtDownloadAnexo" style="display:none" OnTextChanged="txtDownloadAnexo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:literal runat="server" ID="litHistorico">

                                    </asp:literal>
	                            </ContentTemplate> 
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtDownloadAnexo"  />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </body>
</asp:Content>
