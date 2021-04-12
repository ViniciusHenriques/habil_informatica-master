<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManNotaFiscal.aspx.cs" Inherits="SoftHabilInformatica.Pages.Faturamento.ManNotaFiscal" 
    MaintainScrollPositionOnPostback="true"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server" >
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>
    <script src="../../Scripts/jquery.canvasjs.min.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <script type="text/javascript" src="../../Scripts/moment-with-locales.js"></script>
    <script src="../../Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="../../Content/bootstrap-datetimepicker.min.css">
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    <script>

        function browseResult2(e) {
             var verificadorArquivosIncosistentes = false;
            for (let i = 0; i < arquivos.length; i++) {
                var arquivo = arquivos[i];
                if (arquivo.size == 0) {
                   verificadorArquivosIncosistentes  = true;
                   break;                        
                }
            }
            return verificadorArquivosIncosistentes
        }
        function browseResult(e)
        {
            try
            {
                var fi = document.getElementById('<%= fileselector.ClientID %>');

                var maxFileSize = 10480760 // 4MB -> 4 * 1024 * 1024

                if (fi.files.length > 0) {

                    var fsize = 0;
                    for (var i = 0; i < fi.files.length; i++) {

                        fsize += fi.files.item(i).size;

                    }
                    if (fsize > maxFileSize) {

                        alert("Tamanho total dos arquivos deve ser no maximo 10MB");

                        fi.value = null;
                    }
                }
            }
            catch (e)
            {
                alert(e);
            }
        }
        item = '#<%= PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            $('.js-example-basic-single').select2({});
            $('.datetimepicker3').datetimepicker({ locale: 'pt-br', format: 'HH:mm' });
            $("input[id*='txtDtSaida']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt1']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt2']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt3']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt4']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt5']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt6']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt7']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt8']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt9']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt10']").datepicker({todayBtn: "linked",clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            
            $('.js-example-basic-single').select2({});   
            $('.datetimepicker3').datetimepicker({ locale: 'pt-br', format: 'HH:mm' });
            $("input[id*='txtDtSaida']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt1']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt2']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt3']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt4']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt5']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt6']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt7']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt8']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt9']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt10']").datepicker({todayBtn: "linked",clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
        }
        function TabFocada(aba) {
            $('#myTabs a[href="#aba2"]').tab('show');
        }
        function MontaGraficoCredito(Disponivel, Usado)
        {
            CanvasJS.addColorSet("greenShades",["#40E0D0","#B22222"]);
            var credito = {
	            colorSet: "greenShades",
                animationEnabled: true,
                height:300,
	            data: [{
		            type: "pie",
		            indexLabelFontSize: 18,
		            radius: 80,
		            indexLabel: "{label} - {y}%",
		            yValueFormatString: "###0.0\"\"",
		            click: explodePie,
                    dataPoints: [
                        { y: Disponivel, label: "Disponível" },
                        { y: Usado, label: "Utilizado" },


		            ]
	            }]
            };
           
            $("#chartContainer").CanvasJSChart(credito);
            chart.render();

            function explodePie(e) {
	            for(var i = 0; i < e.dataSeries.dataPoints.length; i++) {
		            if(i !== e.dataPointIndex)
			            e.dataSeries.dataPoints[i].exploded = false;
	            }
            }       
        }
        function MostrarItens(title) {
            $("#modalItens .modal-title").html(title);
            $("#modalItens").modal("show");
        }
        function MostrarResultadoXML() {
            $("#modalResultadoXML").modal("show");
        }

        var aba = 1;
        $(document).on('keydown', function (e) {
            if ((e.altKey) && (e.which === 65)) { 
                $get('<%=btnAddItem.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 83)) { 
                $get('<%=btnSalvar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 86)) { 
                $get('<%=btnVoltar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 75)) { 
                $get('<%=btnCancelar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 73)) { 
                $get('<%=btnImportacao.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 39)) { 
                
                 aba++;
                if (aba == 7)
                    aba = 1;
                else {
                    if (document.getElementById('abaTagLi' + aba).style.display == 'none') {
                        for (var i = aba; i < 7; i++) {

                            if (document.getElementById('abaTagLi' + i).style.display == 'none')
                                aba++
                        }
                    }
                }
                if (aba >= 7)
                    aba = 1;
                $('#myTabs a[href="#aba' + aba + '"]').tab('show');

            }
        });

    </script>
    <style>
        .checkbox .cr .cr-icon,
        .radio .cr .cr-icon {
            font-size: 0.8em;
        }
        .checkbox-estados{
            width:4%!important;
            margin-top:10px!important;

        }
        .checkbox .cr,.radio .cr {
            width: 1.5em!important;
            height: 1.5em!important;
        }
        .checkbox-opcoes{
            text-align:center;
            padding:0!important;
            
        }
    </style>
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary">
          <div class="panel-heading" >
                <div class="col-md-4" style="margin-top:5px">Manutenção da Nota Fiscal</div>
                <div class="input-group col-md-6 " style="padding:0!important;margin:0px!important;">
                    <span class="input-group-addon">Tipo de Operação :</span>
                    <asp:DropDownList ID="ddlTipoOperacao" runat="server" Width="100%" TabIndex="5" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                </div>
                <div class="messagealert" id="alert_container" ></div>
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
                                <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: small;">                           
                                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" OnClick="btnVoltar_Click" title="Voltar ( Alt + V )" > 
                                        <span aria-hidden="true" title="Voltar ( Alt + V )" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass=" btn btn-success" ID="btnSalvar" OnClick="btnSalvar_Click" title="Salvar ( Alt + S )">
                                        <span aria-hidden="true" title="Salvar ( Alt + S )" class="glyphicon glyphicon-save" ></span> Salvar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes"  title="Cancelar ( Alt + K )"> 
                                        <span aria-hidden="true" title="Cancelar ( Alt + K )" class="glyphicon glyphicon-remove"></span>  Cancelar
                                    </asp:LinkButton>
                                </div>
                                <asp:panel ID="pnlChaveAcesso" runat="server">
                                    <div class="col-md-6 " style="font-size:x-small;">
                                        <div class="input-group">
                                            <span class="input-group-addon">Chave de acesso:</span>
                                            <asp:TextBox ID="txtChaveAcesso" CssClass="form-control" runat="server" Text ="" Font-Size="Small" TabIndex="1" MaxLength="45" Enabled="false"/>
                                        </div>
                                    </div>
                                </asp:panel>
                                
                                <asp:panel ID="pnlImportacaoNF" runat="server">
                                    <div class="col-md-6" style="text-align:right">
                                        <asp:LinkButton ID="btnImportacao" runat="server" Text="Cancelar" CssClass="btn alert-info" data-toggle="modal" data-target="#modalImportarXML"  title="Importar ( Alt + I )"> 
                                             <span aria-hidden="true" title="Importar ( Alt + I )" class="glyphicon glyphicon-open"></span>  Importar
                                        </asp:LinkButton>
                                    </div>
                                </asp:panel>
                                </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnSalvar"  EventName="Click"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none; margin-top:10px!important">
                        
                        <asp:HiddenField ID="TabName" runat="server"/>            
                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;" >
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <ul class="nav nav-tabs " role="tablist" id="myTabs" >
                                        <li role="presentation" id="abaTagLi1"><a href="#aba1" aria-controls="aba1'" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                                        <li role="presentation" id="abaTagLi2" title="Informações do cliente"><a href="#aba2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-user"></span>&nbsp;&nbsp;Destinatário</a></li>
                                        <li role="presentation" id="abaTagLi3" ><a href="#aba3" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-road"></span>&nbsp;&nbsp;Transportador</a></li>
                                        <li role="presentation" id="abaTagLi4" ><a href="#aba4" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Itens da NF</a></li>
                                        <li role="presentation" id="abaTagLi5" ><a href="#aba5" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-briefcase"></span>&nbsp;&nbsp;Financeiro</a></li>
                                        <li role="presentation" id="abaTagLi6"><a href="#aba6" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-asterisk"></span>&nbsp;&nbsp;Dados Adicionais</a></li>
                                        <li STYLE="float:right!important;" title="Atalho para trocar de aba"><a class="tab-select">Alt + [→] </a></li>
                                    </ul>                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="tab-content" runat="server" id="PanelContext">                                
                                <div role="tabpanel" class="tab-pane" id="aba1" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-1" style="font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Lançamento</label>
                                                    <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="1" MaxLength="18"/>
                                                </div>
                                                <div class="col-md-2" style="margin:0!important;padding:0!important">
                                                    <div class="col-md-8" style="font-size:x-small;padding-right:0!important;">
                                                        <label for="usr" style ="margin-top:1px;">N° pedido</label>
                                                        <asp:TextBox ID="txtNroPedido" CssClass="form-control" runat="server" Text ="" Font-Size="Small" TabIndex="1" MaxLength="18" OnTextChanged="txtNroPedido_TextChanged"/>
                                                    </div>
                                                    <div class="col-md-4" style="font-size:x-small;padding-left:0!important;margin-top:20px">
                                                        <asp:LinkButton ID="btnPesquisarPedido" title="Pesquise os Pedidos" runat="server" CssClass="form-control btn btn-info" OnClick="btnPesquisarPedido_Click" AutoPostBack="true" data-toggle="modal" data-target="#modalPedidos" > 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="col-md-3" style="font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Natureza de Operação <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlNatOperacao" runat="server" Width="100%" TabIndex="5" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>
                                                <div class="col-md-4" style=" font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Empresa <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlEmpresa" runat="server"   TabIndex="4"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_TextChanged"   AutoPostBack="true"/>
                                                </div>
                                                <div class="col-md-2" style="font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Situação</label>
                                                    <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" TabIndex="5" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                                </div>  
                                                
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">N° NF-e</label>
                                                    <asp:TextBox ID="txtNroDocumento" AutoPostBack="False" CssClass="form-control"  TabIndex="6" runat="server" MaxLength="18" Enabled="false"/>
                                                </div>
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Série NF-e</label>
                                                    <asp:TextBox ID="txtNroSerie" AutoPostBack="False" CssClass="form-control"  TabIndex="7" runat="server" MaxLength="18" Enabled="false"/>
                                                </div>
                                                <div class="col-md-2" style=" font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Data Emissão</label>
                                                    <asp:TextBox ID="txtdtemissao" CssClass="form-control"  TabIndex="3" runat="server" name="txtdtemissao"  AutoPostBack="true" Enabled="false" MaxLength="15" />
                                                </div>  
                                                <div class="col-md-2" style=" font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Hora Emissão</label>
                                                    <asp:TextBox ID="txtHrEmissao" CssClass="form-control"  TabIndex="3" runat="server" name="txtdtemissao"  AutoPostBack="true" Enabled="false" MaxLength="15" />
                                                </div>  
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Data Saída <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:TextBox ID="txtDtSaida"  AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" />
                                                </div> 
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Hora Saída <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:TextBox ID="txtHrSaida"  AutoPostBack="False" CssClass="form-control datetimepicker3" TabIndex="8" runat="server" MaxLength="15" />
                                                </div>                                              
                                            </div>
                                            <div class="row" style="margin-top:5px;">
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Finalidade da NF <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlFinalidade" runat="server" Width="100%" TabIndex="9" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div> 
                                                <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Modalidade da NF <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlModalidadeFrete" runat="server" Width="100%" TabIndex="9" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div> 
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Regime tributário <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlRegime" runat="server" Width="100%" TabIndex="9" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div> 
                                                <div class="col-md-2" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Consumidor final <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlConsumidorFinal" runat="server" Width="100%" TabIndex="9" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div> 
                                                <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                    <label for="usr" style ="margin-top:1px;">Indicador de presença <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                    <asp:DropDownList ID="ddlIndicadorPresenca" runat="server" Width="100%" TabIndex="9" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                </div> 
                                            </div>
     
                                            <div class="row">
                                                <div class="col-md-12 " style="margin-top:15px">
                                                    <div class="panel panel-primary">
                                                        <div class="panel-heading" style="text-align:center;padding:13px!important;height:45px">
                                                            Valores Totais da Nota
                                                        </div>
                                                        <div class="panel-body scroll-true" style="padding-left:0px!important;padding-right:0px!important;padding-bottom:0px!important">
                                                             <div class="col-md-12" style="padding:0!important">
                                                               
                                                                <div class="col-md-2" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Base de ICMS</label>
                                                                    <asp:TextBox ID="txtBaseICMS" CssClass="form-control"  TabIndex="17" runat="server" AutoPostBack="true" Text="0,00" Enabled="false" MaxLength="18"  />
                                                                </div> 
                                                                 <div class="col-md-2" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Valor de ICMS</label>
                                                                    <asp:TextBox ID="txtVlrICMS" CssClass="form-control"  TabIndex="18" runat="server" AutoPostBack="true" Text="0,00" MaxLength="18" Enabled="false"  />
                                                                </div> 
                                                                 <div class="col-md-2" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Base de ICMS ST</label>
                                                                    <asp:TextBox ID="txtBaseICMSST" CssClass="form-control"  TabIndex="20" runat="server" AutoPostBack="true" Text="0,00" MaxLength="18" Enabled="false"  />
                                                                </div> 
                                                                 <div class="col-md-2" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Valor de ICMS ST</label>
                                                                    <asp:TextBox ID="txtVlrICMSST" CssClass="form-control"  TabIndex="20" runat="server" AutoPostBack="true" Text="0,00" MaxLength="18" Enabled="false"  />
                                                                </div>  
                                                                 <div class="col-md-4" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Valor dos Produtos</label>
                                                                    <asp:TextBox ID="txtTotalItens" CssClass="form-control"  TabIndex="20" runat="server" AutoPostBack="true" Text="0,00" MaxLength="18" Enabled="false"  />
                                                                </div> 
                                                                                           
                                                                 <div class="col-md-3" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Frete</label>
                                                                    <asp:TextBox ID="txtFrete" CssClass="form-control"  TabIndex="22" runat="server" AutoPostBack="true" Text="0,00" MaxLength="18" onFocus="this.select()" OnTextChanged="txtFrete_TextChanged"/>
                                                                </div> 
                                                                <div class="col-md-3" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Valor de Desconto </label>
                                                                    <asp:TextBox ID="txtDesconto" CssClass="form-control"  TabIndex="16" runat="server" AutoPostBack="true" Text="0,00" Enabled="false" MaxLength="18" />
                                                                </div> 
                                                                 <div class="col-md-3" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Valor despesas acessorias</label>
                                                                    <asp:TextBox ID="txtDespesas" CssClass="form-control"  TabIndex="16" runat="server" AutoPostBack="true" Text="0,00" Enabled="false" MaxLength="18" />
                                                                </div> 
                                                                 <div class="col-md-3" style="font-size: x-small;">
                                                                    <label for="usr" style ="margin-top:1px;">Valor de IPI</label>
                                                                    <asp:TextBox ID="txtVlrIPI" CssClass="form-control"  TabIndex="16" runat="server" AutoPostBack="true" Text="0,00" Enabled="false" MaxLength="18" />
                                                                </div> 
                                                                <div class="col-md-8"></div>
                                                                <div class="col-md-4" style="font-size: small;padding-top:20px;">
                                                                    <div class="input-group" title="Valor total do seu Orçamento">
                                                                       <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="border-right:0!important;border:0!important"><b>Total:</b></span>
                                                                        <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="font-weight:bold;padding-right:4px!important;font-size:20px;border:0!important;border-right:0!important;">R$</span>
                                                                        <asp:TextBox ID="txtVlrTotal"  CssClass="form-control CorPadraoEscolhidaTexto  CorPadraoEscolhida" Text="0,00"  runat="server" MaxLength="50" Enabled="false" style="padding-left:0!important;font-weight:bold;font-size:20px;border:0!important;"  />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div> 
                                          
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCodigo" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtPessoa" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>                                
                                <div role="tabpanel" class="tab-pane" id="aba2" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="col-md-12" >
                                                <label for="usr" style="font-size: x-small;">Cliente <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <div class="input-group " style="width:100%">                              
                                                    <asp:TextBox ID="txtCodPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server" TabIndex="11" OnTextChanged="txtCodPessoa_TextChanged"   AutoPostBack="true" onFocus="this.select()"                                
                                                    Width="15%" MaxLength="6" />                                                      
                                                    <asp:LinkButton ID="btnPessoa" title="Pesquise os Clientes" runat="server" CssClass="form-control btn btn-primary" Width="10%"  OnClick="ConPessoa" AutoPostBack="true"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server"  Enabled="false"  Width="75%"  />
                                                </div>
                                            </div>
                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">CNPJ/CPF</label>
                                                <asp:TextBox ID="txtCNPJCPFCredor" CssClass="form-control" runat="server" Enabled="false" Text =""  />
                                            </div>                                                                 
                                            <div class="col-md-10" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Endereço</label>
                                                <asp:TextBox ID="txtEndereco" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>                                    
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">CEP</label>
                                                <asp:TextBox ID="txtCEP" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Estado</label>
                                                <asp:TextBox ID="txtEstado" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Cidade</label>
                                                <asp:TextBox ID="txtCidade" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Bairro</label>
                                                <asp:TextBox ID="txtBairro" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">E-mail</label>
                                                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCNPJCPFCredor" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtEndereco" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCEP" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtEstado" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCidade" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtBairro" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCodPessoa" EventName="TextChanged" />

                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="aba3" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                             <div class="col-md-12" >
                                                <label for="usr" style="font-size: x-small;">Transportador <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                <div class="input-group " style="width:100%">                              
                                                    <asp:TextBox ID="txtCodTransportador" CssClass="form-control"  runat="server" TabIndex="13" OnTextChanged="txtCodTransportador_TextChanged"   AutoPostBack="true"  onFocus="this.select()"                              
                                                    Width="15%" MaxLength="6" />                                                      
                                                    <asp:LinkButton ID="btnTransportador" title="Pesquise os Transportadores" runat="server" CssClass="form-control btn btn-primary" Width="10%"  OnClick="btnTransportador_Click" AutoPostBack="true"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtTransportador" CssClass="form-control" runat="server"  Enabled="false"  Width="75%"  />
                                                </div>
                                            </div> 
                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">CNPJ/CPF</label>
                                                <asp:TextBox ID="txtCNPJTransp" CssClass="form-control" runat="server" Enabled="false" Text =""  />
                                            </div>                                                                 
                                            <div class="col-md-10" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Endereço</label>
                                                <asp:TextBox ID="txtEnderecoTransp" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>                                    
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">CEP</label>
                                                <asp:TextBox ID="txtCEPTransp" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Estado</label>
                                                <asp:TextBox ID="txtUFTransp" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Cidade</label>
                                                <asp:TextBox ID="txtCidadeTransp" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Bairro</label>
                                                <asp:TextBox ID="txtBairroTransp" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">E-mail</label>
                                                <asp:TextBox ID="txtEmailTransp" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-4" style="font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Peso Liquido</label>
                                                <asp:TextBox ID="txtPesoLiquido" CssClass="form-control"  TabIndex="18" runat="server" AutoPostBack="true" Text="0,000" MaxLength="18" Enabled="false"  />
                                            </div>
                                            <div class="col-md-4" style="font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Peso Bruto</label>
                                                <asp:TextBox ID="txtPesoBruto" CssClass="form-control"  TabIndex="18" runat="server" AutoPostBack="true" Text="0,000" MaxLength="18" Enabled="false"  />
                                            </div>
                                            <div class="col-md-4" style="font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Cubagem</label>
                                                <asp:TextBox ID="txtCubagem" CssClass="form-control"  TabIndex="18" runat="server" AutoPostBack="true" Text="0,00000" MaxLength="18" Enabled="false"  />
                                            </div>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCodTransportador" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="aba4" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                    <div class="row">
                                        <div class="col-md-12 " style="margin-top:6px">
                                            <div class="panel panel-primary">
                                                <div class="panel-heading" style="text-align:center;padding:13px!important;height:45px">
                                                    <div class="col-md-1">
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="btnAddItem" runat="server" CssClass="btn btn-info" TabIndex="15" UseSubmitBehavior="false"  OnClick="btnAddItem_Click" style="float:left!important;margin-top:-7px!important" title="Editar ( Alt + A )"> 
                                                                    <span aria-hidden="true" class="glyphicon glyphicon-pencil"></span> Editar
                                                                </asp:LinkButton>
                                                                </ContentTemplate> 
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnAddItem" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>                                                                     
                                                    <div class="col-md-10">Itens da Nota Fiscal
                                                        
                                                    </div>
                                                    <div class="col-md-1" >
                                                        <asp:Label runat="server" ID="lblTotalItens" Font-Bold="true" Font-Size="Medium" ></asp:Label>    
                                                    </div>  
                                                </div>
                                                <div class="panel-body scroll-true" style="padding-left:0px!important;padding-right:0px!important;padding-bottom:0px!important">
                                                    <asp:GridView ID="grdProduto" runat="server" AllowSorting="true"  OnSorting="grdProduto_Sorting"
                                                            CssClass="table table-hover table-striped"
                                                            GridLines="None" AutoGenerateColumns="False" PagerSettings-Mode ="NumericFirstLast" OnPageIndexChanging="grdItens_PageIndexChanging" 
                                                            Font-Size="8pt"  Visible="true" AllowPaging="true" PageSize="10"
                                                            OnSelectedIndexChanged="grdPagamento_SelectedIndexChanged">
                                                        <PagerStyle HorizontalAlign = "left"   CssClass = "GridPager" />
                                                    <Columns>
                                                        <asp:BoundField DataField="CodigoItem" HeaderText="#"  SortExpression="CodigoItem" />
                                                        <asp:BoundField DataField="CodigoProduto" HeaderText="Código do Produto"  SortExpression="CodigoProduto" />
                                                        <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Descrição"  SortExpression="Cpl_DscProduto" />
                                                        <asp:BoundField DataField="Unidade" HeaderText="Unidade"  DataFormatString="{0:F}"  SortExpression="Unidade" />
                                                        <asp:BoundField DataField="Quantidade" HeaderText="Quantidade"  DataFormatString="{0:F}"  SortExpression="Quantidade" />
                                                        <asp:BoundField DataField="PrecoItem" HeaderText="Preço"  DataFormatString="{0:C}"  SortExpression="PrecoItem" />
                                                        <asp:BoundField DataField="ValorDesconto" HeaderText="% Desconto"  DataFormatString="{0:F}%"  SortExpression="ValorDesconto" />
                                                        <asp:BoundField DataField="ValorTotalItem" HeaderText="Valor Total"  DataFormatString="{0:C}"  SortExpression="ValorTotalItem" />
                                                    </Columns>
                                                    <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div> 
                                </div>
                                <div role="tabpanel" class="tab-pane" id="aba5" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">    
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-lg-4 col-md-4 col-xs-12" style="font-size: x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cobrança <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                                                        <asp:DropDownList ID="ddlTipoCobranca" runat="server" AutoPostBack="False" Width="100%" TabIndex="10" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                    </div>
    
                                                    <div class="col-lg-5 col-md-4 col-xs-12" style=" font-size: x-small;">
                                                        <label for="usr">Condição de Pagamento: </label>
                                                        <asp:DropDownList ID="ddlPagamento" runat="server" TabIndex="16" CssClass="form-control js-example-basic-single " Width="100%" OnTextChanged="ddlPagamento_TextChanged" AutoPostBack="true" ></asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-1 col-md-2 col-xs-6">
                                                        <asp:LinkButton ID="btnLimpar" runat="server" style=" margin-top:19px"  CssClass="btn btn-default" UseSubmitBehavior="false" OnClick="btnLimpar_Click" TabIndex="21"> 
                                                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-repeat"></span>  Limpar
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-xs-6">
                                                        <asp:LinkButton ID="btnGerarParcela" runat="server" style=" margin-top:19px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnGerarParcela_Click" TabIndex="21"> 
                                                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Gerar Parcelas
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-top:10px;" >
                                                    <div class="col-lg-2 col-md-3 col-xs-6">
                                                        <asp:panel runat="server" id="pnlAddTodos" Visible="false" Width="100%">
                                                            <asp:LinkButton ID="btnAddTodas" runat="server" title="Adicionar/Alterar Todas Parcelas" TabIndex="20" Width="100%" CssClass="btn btn-success"  OnClick="btnAddTodas_Click" > 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span> Adicionar Todas
                                                            </asp:LinkButton>
                                                        </asp:panel>
                                                    </div>
                                                    <div class="col-lg-2 col-md-3 col-xs-6">
                                                        <asp:panel runat="server" id="pnlRemoverTodos" Visible="false" Width="100%">
                                                            <asp:LinkButton ID="btnRemoverTodas" runat="server" title="Remover Todas Parcelas" TabIndex="20" CssClass="btn btn-danger"  Width="100%" OnClick="btnRemoverTodas_Click" > 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span> Remover Todas
                                                            </asp:LinkButton>
                                                        </asp:panel>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-8 col-md-12" style="margin-top:20px;">
                                                        <asp:Panel ID="pnlParcela1" runat="server" Visible="false">
                                                            <div class="row" >                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:18px">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:18px">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt1" name="txtDt1" AutoPostBack="False" CssClass="form-control" Enabled="true" TabIndex="3" runat="server" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:18px">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor: </span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor"  CssClass="form-control" style="border-left:0!important"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" />
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:18px;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela" runat="server"  title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela2" runat="server" Visible="false">
                                                            <div class="row">                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro2"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt2" name="txtDt2" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor:</span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor2"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela2" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela2_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela2" runat="server"  title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela2_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela3" runat="server" Visible="false">
                                                            <div class="row">                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro3"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt3" name="txtDt3" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor:</span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor3"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela3" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela3_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela3" runat="server"  title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela3_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela4" runat="server" Visible="false">
                                                            <div class="row">                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro4"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt4" name="txtDt4" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor:</span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor4"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela4" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela4_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela4" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela4_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela5" runat="server" Visible="false">
                                                            <div class="row">                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro5"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt5" name="txtDt5" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor:</span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor5"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela5" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela5_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela5" runat="server"  title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela5_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela6" runat="server" Visible="false">
                                                            <div class="row">                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro6"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt6" name="txtDt6" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor:</span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor6"   CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela6" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela6_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela6" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela6_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela7" runat="server" Visible="false">
                                                            <div class="row">                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro7"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt7" name="txtDt7"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor:</span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor7"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela7" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela7_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela7" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela7_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela8" runat="server" Visible="false">
                                                            <div class="row">                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro8"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt8" name="txtDt8"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor:</span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor8"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela8" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela8_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela8" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela8_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela9" runat="server" Visible="false">
                                                            <div class="row">                                                
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">N° Doc:</span>
                                                                        <asp:TextBox ID="txtNro9"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Data Vencimento:</span>
                                                                        <asp:TextBox ID="txtDt9" name="txtDt9" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">Valor:</span>
                                                                        <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                        <asp:TextBox ID="txtValor9"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                    </div>
                                                                </div>
                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                    <asp:LinkButton ID="btnAddParcela9" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela9_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExcluirParcela9" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela9_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                        <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                    </asp:LinkButton>                                                   
                                                            
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlParcela10" runat="server" Visible="false">
                                                    <div class="row">                                                
                                                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">N° Doc:</span>
                                                                <asp:TextBox ID="txtNro10"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">Data Vencimento:</span>
                                                                <asp:TextBox ID="txtDt10" name="txtDt10" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">Valor:</span>
                                                                <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                <asp:TextBox ID="txtValor10"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                            </div>
                                                        </div>
                                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                            <asp:LinkButton ID="btnAddParcela10" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela10_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnExcluirParcela10" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela10_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                            </asp:LinkButton>                                                   
                                                            
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                    </div>
                                                    <asp:Panel ID="pnlParcelas" runat="server" Visible="false">
                                                        <div class="col-md-12 col-lg-4" style="margin-top:37px">
                                                            <div class="panel panel-primary">
                                                                <div class="panel-heading" style="text-align:center;">Parcelas</div>
                                                                <div class="panel-body" style="padding-left:0px!important;padding-right:0px!important;padding-bottom:0px!important">
                                                                        <asp:GridView ID="grdPagamento" runat="server"
                                                                                CssClass="table  table-striped"
                                                                                GridLines="None" AutoGenerateColumns="False"
                                                                                Font-Size="8pt"  Visible="true"
                                                                                OnSelectedIndexChanged="grdPagamento_SelectedIndexChanged">
                                                                            <PagerStyle HorizontalAlign = "Center" CssClass = "GridPager" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="CodigoParcela" HeaderText="#"   HeaderStyle-Width ="15%" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="centerHeaderText"/>
                                                                            <asp:BoundField DataField="DGNumeroDocumento" HeaderText="Nro Doc."   HeaderStyle-Width ="25%" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="centerHeaderText"/>
                                                                            <asp:BoundField DataField="DataVencimento" HeaderText="Data Venc."  DataFormatString="{0: dd/MM/yyyy}" HeaderStyle-Width ="30%"  HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="centerHeaderText"/>
                                                                            <asp:BoundField DataField="ValorParcela" HeaderText="Valor Parcela" DataFormatString="{0:C}"   HeaderStyle-Width ="30%" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="centerHeaderText"/>

                                                                        </Columns>
                                                                        <RowStyle CssClass="cursor-pointer" />
                                                                        </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>   
                                                </div>
                                            </ContentTemplate> 
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAddItem" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-12" style="margin-top:10px">
                                            <div class="panel panel-primary">
                                            <div class="panel-heading" style="text-align:center;">
                                                Detalhes de pagamento
                                            </div>
                                            <div class="panel-body" style="padding-left:0px!important;padding-right:0px!important;padding-bottom:0px!important">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdFinanceiro" runat="server"
                                                                CssClass="table  table-hover table-striped"
                                                                GridLines="None" AutoGenerateColumns="False" PagerSettings-Mode ="NumericFirstLast" OnPageIndexChanging="grdFinanceiro_PageIndexChanging" 
                                                                Font-Size="8pt" Visible="true" AllowPaging="true" PageSize="10"
                                                                OnSelectedIndexChanged="grdFinanceiro_SelectedIndexChanged">
                                                            <PagerStyle HorizontalAlign = "left"   CssClass = "GridPager" />
                                                            <Columns>
                                                                <asp:BoundField DataField="DGDocumento" HeaderText="Documento" />
                                                                <asp:BoundField DataField="DataVencimento" HeaderText="Data Vencimento" DataFormatString="{0:dd/MM/yyyy}"/>
                                                                <asp:BoundField DataField="DataEmissao" HeaderText="Data Emissão"  />
                                                                <asp:BoundField DataField="ValorGeral" HeaderText="Valor Total"  DataFormatString="{0:C}"/>
                                                                <asp:BoundField DataField="Cpl_vlPago" HeaderText="Valor Pago"  DataFormatString="{0:C}"/>
                                                                <asp:BoundField DataField="Cpl_vlPagar" HeaderText="Em Aberto"  DataFormatString="{0:C}"/>
                                                            </Columns>
                                                            <RowStyle CssClass="cursor-pointer" />
                                                        </asp:GridView>
                                                    </ContentTemplate> 
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>    
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane" id="aba6" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                    <div class="col-md-2" style="font-size: x-small;">
                                        <label for="usr" style ="margin-top:1px;">Número Web </label>
                                        <asp:TextBox ID="txtNroWeb" CssClass="form-control"  TabIndex="21" enabled="false" runat="server" AutoPostBack="true" MaxLength="18" onFocus="this.select()" OnTextChanged="txtNroWeb_TextChanged"/>
                                    </div>
                                    <div class="col-md-10" style="font-size: x-small;margin-top:0px">
                                        <label for="usr" style ="margin-top:1px;">Observação</label>
                                        <asp:TextBox ID="txtDescricao" AutoPostBack="False" CssClass="form-control" MaxLength="300" TabIndex="23" runat="server" onFocus="this.select()" />
                                    </div>
                                    <div class="col-md-12" style="margin-top:15px">
                                        <div class="panel panel-primary">
                                        <div class="panel-heading" style="text-align:center;">Anexo(s) do documento</div>
                                        <div class="panel-body" >
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="btnNovoAnexo" runat="server" style="padding-left:10px; margin-bottom:10px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovoAnexo_Click" TabIndex="21" ToolTip="Add. Novo anexo ( Alt + N )"> 
                                                        <span aria-hidden="true"  class="glyphicon glyphicon-edit"></span> Novo Anexo
                                                    </asp:LinkButton>
                                                    <br />
                                                    <asp:GridView ID="grdAnexo" runat="server"
                                                        CssClass="table  table-hover table-striped"
                                                        GridLines="None" AutoGenerateColumns="False"
                                                        Font-Size="8pt"  Visible="true"
                                                        OnSelectedIndexChanged="grdAnexo_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoAnexo" HeaderText="#"  />
                                                            <asp:BoundField DataField="DataHoraLancamento" HeaderText="Data/Hora Lançamento"  />
                                                            <asp:BoundField DataField="Cpl_Maquina" HeaderText="Estação" />
                                                            <asp:BoundField DataField="Cpl_Usuario" HeaderText="Usuário"  />
                                                            <asp:BoundField DataField="DescricaoArquivo" HeaderText="Descrição"/>
                                                            <asp:BoundField DataField="NomeArquivo" HeaderText="Arquivo"/>
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
                                    <div class="col-md-12">
                                        <div class="panel panel-primary">
                                        <div class="panel-heading" style="text-align:center;">Log(s) do documento</div>
                                        <div class="panel-body" >
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdLogDocumento" runat="server" CssClass="table table-bordered table-hover table-striped"
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
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="panel panel-primary">
                                        <div class="panel-heading" style="text-align:center;">Evento(s) do documento</div>
                                        <div class="panel-body" >
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                <ContentTemplate>            
                                                    <asp:GridView ID="GrdEventoDocumento" runat="server" AutoPostBack="false"
                                                            CssClass="table table-bordered table-hover table-striped"
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
                            Deseja cancelar este documento?
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnCfmSim" OnClientClick="this.disabled = true; this.value = 'Cancelando...';" UseSubmitBehavior="false" OnClick="btnCfmSim_Click" CssClass="btn btn-danger" Text="Sim" style="color:white"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Não</button>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCfmSim"  />
        </Triggers>
    </asp:UpdatePanel> 
   

     <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="modalItens" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" >
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Atenção</h4>
                        </div>
                        <div class="modal-body">
                            <asp:GridView ID="grdItemOutrosOrcamentos" runat="server" 
                                AutoGenerateColumns="False" CssClass="table  table-hover"
                                GridLines="None"  
                                Font-Size="8pt" >
                                <Columns>
                                    <asp:BoundField DataField="CodigoItem" HeaderText="#"  SortExpression="CodigoItem" />
                                    <asp:BoundField DataField="CodigoProduto" HeaderText="Produto"  SortExpression="CodigoProduto" />
                                    <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Descrição"  SortExpression="Cpl_DscProduto" />
                                    <asp:BoundField DataField="Unidade" HeaderText="Un."  DataFormatString="{0:F}"  SortExpression="Unidade" />
                                    <asp:BoundField DataField="Quantidade" HeaderText="Qtde"  DataFormatString="{0:F}"  SortExpression="Quantidade" />
                                    <asp:BoundField DataField="PrecoItem" HeaderText="Preço"  DataFormatString="{0:C}"  SortExpression="PrecoItem" />
                                    <asp:BoundField DataField="ValorDesconto" HeaderText="Desc."  DataFormatString="{0:F}%"  SortExpression="ValorDesconto" />
                                    <asp:BoundField DataField="ValorTotalItem" HeaderText="Valor Total"  DataFormatString="{0:C}"  SortExpression="ValorTotalItem" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCfmSim"  />
        </Triggers>
    </asp:UpdatePanel>  
    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="modalPedidos" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" >
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Pedidos</h4>
                        </div>
                        <div class="modal-body">
                            
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCfmSim"  />
        </Triggers>
    </asp:UpdatePanel> 
  
    <div class="modal fade" id="modalResultadoXML" tabindex="-1" role="dialog" aria-labelledby="myLargeModal" >
        <div class="modal-dialog" role="document" style="width:60%">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Observações sobre o XML</h4>
                </div>
                <div class="modal-body">
                    <asp:Label runat="server" ID="lblResultadoXML"></asp:Label>
                </div>
                <div class="modal-footer">
                    <p><span class='glyphicon glyphicon-remove' style='color:red'></span> - Não existe cadastrado
                        <span class='glyphicon glyphicon-ok' style='color:green'></span> - Existe cadastrado</p>
                    <h4><asp:label runat="server" ID="lblSalvarItens">Deseja salvar os itens não cadastrados?   </asp:label>
                    <asp:Button runat="server" ID="btnSalvarItensModal" OnClientClick="this.disabled = true; this.value = 'Salvando...';" UseSubmitBehavior="false" OnClick="btnSalvarItensModal_Click" CssClass="btn btn-success" Text="Sim" style="color:white"/>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Não</button></h4>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modalImportarXML" tabindex="-1" role="dialog" aria-labelledby="myLargeModal" >
        <div class="modal-dialog" role="document" style="width:60%">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Importacão de NF-e, escolha um arquivo XML</h4>
                </div>
                <div class="modal-body">
                    <asp:LinkButton runat="server" id="btnArquivo"  CssClass="btn btn-default" ToolTip="Escolha o arquivo xlsx">
                        <asp:FileUpload  runat="server" id="fileselector" type="file" style="margin:0"  />
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnImportar" runat="server" CssClass="btn alert-info" TabIndex="0" OnClick="btnImportar_Click"> 
                        <span aria-hidden="true" class="glyphicon glyphicon-open"></span>  Importar XML
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
