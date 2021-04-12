<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManOrdProducao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.ManOrdProducao" 
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
    <script type="text/javascript" src="../../Scripts/moment-with-locales.js"></script>
    <script src="../../Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="../../Content/bootstrap-datetimepicker.min.css">

    <script type="text/javascript">
       $(document).ready(function (e) {
            $('.datetimepicker1').datetimepicker({ locale: 'pt-br', format: 'DD/MM/YYYY HH:mm' });
        });
    </script>
    <script>    
        function Cancelar()
        {
            $("#CaixaCancelar.modal-title").html("");
            $("#CaixaCancelar").modal("show");
        }
        function CancelarHide()
        {
            $("#CaixaCancelar.modal-title").html("");
            $("#CaixaCancelar").modal("hide");
        }
        function Pessoa()
        {
            $("#CaixaPessoas.modal-title").html("");
            $("#CaixaPessoas").modal("show");
        }
        function Composto()
        {
            $("#CaixaCompostos.modal-title").html("");
            $("#CaixaCompostos").modal("show");
        }
        function Documento()
        {
            $("#CaixaDocumentoOriginal.modal-title").html("");
            $("#CaixaDocumentoOriginal").modal("show");
        }
        function CompostosDocumento()
        {
            $("#CaixaCompostosDocumento.modal-title").html("");
            $("#CaixaCompostosDocumento").modal("show");
        }
        function CompostosDocumentoHide()
        {
            $("#CaixaCompostosDocumento.modal-title").html("");
            $("#CaixaCompostosDocumento").modal("hide");
        }
        function DocumentoHide()
        {
            $("#CaixaDocumentoOriginal.modal-title").html("");
            $("#CaixaDocumentoOriginal").modal("hide");
        }
        function PessoaHide()
        {
            $("#CaixaPessoas.modal-title").html("");
            $("#CaixaPessoas").modal("hide");
        }
        function CompostoHide()
        {
            $("#CaixaCompostos.modal-title").html("");
            $("#CaixaCompostos").modal("hide");
        }
        function expandcollapse(name) {
            var div = document.getElementById(name);
            var img = document.getElementById('img' + name);
            if (div.style.display == "none")
            {
                dix.style.display - "inline";
                img.src = "up_arrow";
            }
            else
            {
                div.style.display = "none";
                img.src = "down_arrow"
            }
        }
    </script>
    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            $('.js-example-basic-single').select2({});

        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});
            $('.datetimepicker1').datetimepicker({ locale: 'pt-br', format: 'DD/MM/YYYY HH:mm' });
        }

        
        var aba = 1;
        $(document).on('keydown', function (e) {
            if ((e.altKey) && (e.which === 83)) { 
                $get('<%=btnSalvar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 86)) { 
                $get('<%=btnVoltar.ClientID%>').click(); 
            }

            else if ((e.altKey) && (e.which === 39)) { 
                
                aba++;
                if (aba == 9)
                    aba = 1;
                else {
                    if (document.getElementById('abaTagLi' + aba).style.display == 'none') {
                        for (var i = aba; i < 9; i++) {

                            if (document.getElementById('abaTagLi' + i).style.display == 'none')
                                aba++
                        }
                    }
                }
                if (aba >= 9)
                    aba = 1;
                $('#myTabs a[href="#aba' + aba + '"]').tab('show');

            }

        });

         $(document).ready(function (e) {
            $('.datetimepicker1').datetimepicker({ locale: 'pt-br', format: 'DD/MM/YYYY HH:mm' });
        });
    </script>
    <style type="text/css">
        @media screen and (max-width: 800px) {
            .noprint{display:none;}  
            .print{display:block!important}
            .scroll-true{overflow-y:scroll!important;}
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
    </style>
   
<div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
    <div class="panel panel-primary">
        <div class="panel-heading">
            Manutenção da Ordem de Produção
            <div class="messagealert" id="alert_container"></div>        
        </div>
        <div class="panel-body" >
            <div class="row" style="background-color: white; border: none;">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <%--BOTÕES--%>
                        <div class="col-md-8" style="padding-top:10px; background-color: white; border: none; text-align: left; font-size: small;">                           
                            <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0"  OnClick="btnVoltar_Click"> 
                                <span aria-hidden="true"  class="glyphicon glyphicon-arrow-left"></span>  Voltar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnSalvar"  runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-success">
                                <span aria-hidden="true" class="glyphicon glyphicon-save" ></span> Salvar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click"> 
                                <span aria-hidden="true" class="glyphicon glyphicon-ban-circle"></span>  Cancelar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnProduzir" Visible="false"  runat="server" Text="Produzir" CssClass="btn btn-primary" OnClick="btnProduzir_Click"> 
                                <span aria-hidden="true" class="glyphicon glyphicon-play"></span>  Produzir
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnEncerrar" Visible="false" runat="server" Text="Encerrar" CssClass="btn btn-success" OnClick="btnEncerrar_Click"> 
                                <span aria-hidden="true" class="glyphicon glyphicon-check"></span>  Encerrar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnImprimir" Visible="false" runat="server" Text="Imprimir" CssClass="btn btn-info" OnClick="btnImprimir_Click"> 
                                <span aria-hidden="true"  class="glyphicon glyphicon-print"></span>  Imprimir
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate> 
                </asp:UpdatePanel>
            </div>
            <div class="container-fluid">
                <div class="row" style="background-color: white; border: none; margin-top:10px!important">
                    <asp:HiddenField ID="TabName" runat="server"/>            
                    <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;" >
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <ul class="nav nav-tabs" role="tablist" id="myTabs">
                                    <li role="presentation" id="abaTagLi1"><a href="#aba1" aria-controls="#aba1" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                                    <li role="presentation" id="abaTagLi2"><a href="#aba2" aria-controls="#aba2" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-text-size"></span>&nbsp;&nbsp;Observações</a></li>
                                    <li role="presentation" id="abaTagLi3" style="<%=Panels%>" ><a href="#aba3" aria-controls="#aba3" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Eventos</a></li>
                                    <li role="presentation" id="abaTagLi4"><a href="#aba4" aria-controls="#aba4" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Componentes</a></li>
                                    <li role="presentation" id="abaTagLi5" style="<%=Panels%>" ><a href="#aba5" aria-controls="#aba5" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-film"></span>&nbsp;&nbsp;Roteiro</a></li>
                                    <li role="presentation" id="abaTagLi6"><a href="#aba6" aria-controls="#aba6" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexos</a></li>
                                    <li role="presentation" id="abaTagLi7" style="<%=Panels%>"><a href="#aba7" aria-controls="#aba7" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-tasks"></span>&nbsp;&nbsp;Logs</a></li>
                                    <li role="presentation" id="abaTagLi8" style="<%=Encerra%>"><a href="#aba8" aria-controls="#aba8" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-saved"></span>&nbsp;&nbsp;Encerramento</a></li>
                                    <li STYLE="float:right!important;" title="Atalho para trocar de aba"><a class="tab-select">Alt + [→] </a></li>
                                </ul>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="tab-content" runat="server" id="PanelContext">                                
                            <div role="tabpanel" class="tab-pane" id="aba1" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size:small;">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-2" style="font-size:x-small; padding-right:0!important">
                                                <label for="usr" style="margin-top:1px;">Lançamento</label>
                                                <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="1" MaxLength="18"/>
                                            </div>
                                            <div class="col-md-4" style="font-size:x-small;padding-right:0!important">
                                                <label for="usr" style="margin-top:1px;">Tipo Operação  </label>
                                                <asp:DropDownList ID="ddlTipoOperacao" runat="server" Width="100%" TabIndex="5" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                            </div>
                                                                                  
                                            <div class="col-md-4" style=" font-size:x-small;padding-right:0!important">
                                                <label for="usr" style="margin-top:1px;">Empresa  </label>
                                                <asp:DropDownList ID="ddlEmpresa" runat="server"   TabIndex="4"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_TextChanged"   AutoPostBack="true"/>
                                            </div>
                                            <div class="col-md-2" style="font-size:x-small;">
                                                <label for="usr" style="margin-top:1px;">Situação</label>
                                                <asp:DropDownList ID="ddlSituacao" runat="server" Width="100%" TabIndex="5" Enabled="false" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                            </div>  
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2" style="font-size: x-small;margin-top:3px;padding-right:0!important">
                                                <label for="usr">N° Ord Produção</label>
                                                <asp:TextBox ID="txtNroDocumento" AutoPostBack="False" CssClass="form-control"  TabIndex="6" runat="server" MaxLength="18" Enabled="false"/>
                                            </div>
                                            <div class="col-md-2" style="font-size: x-small;margin-top:3px;padding-right:0!important">
                                                <label for="usr"> Série </label>
                                                <asp:TextBox ID="txtSerie" AutoPostBack="False" CssClass="form-control"  TabIndex="6" runat="server" MaxLength="18" Enabled="false"/>
                                            </div>
                                            <div class="col-md-2" style="margin-top:3px; padding-right:0!important">
                                                <label for="usr" style="font-size:x-small"> Nº Pedido: </label>
                                                    <div class="input-group" style="width:100%">
                                                        <asp:TextBox ID="txtDocOriginal" Width="80%" OnTextChanged="txtDocOriginal_TextChanged" AutoPostBack="true" CssClass="form-control"  TabIndex="6" runat="server" MaxLength="18" Enabled="false"/>
                                                        <asp:Button runat="server" id="btnDoc" Width="20%" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="btnDoc_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar Documento Original" TabIndex="5"/>
                                                    </div>
                                            </div>
                                            <div class="col-md-3" style="font-size: x-small;margin-top:3px;padding-right:0!important">
                                                <label for="usr">Data Emissão</label>
                                                <asp:TextBox ID="txtdtemissao" CssClass="form-control"  TabIndex="3" runat="server" name="txtdtemissao"  AutoPostBack="true" Enabled="false" MaxLength="15" />
                                            </div> 
                                            <div class="col-md-3" style="font-size: x-small;margin-top:3px">
                                                <label for="usr" >Data Encerramento  </label>
                                                <asp:TextBox ID="txtdtEncerramento" name="txtdtEncerramento" AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="false"/>
                                            </div>              
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6" style="padding-right:0!important">
                                                <label for="usr" style="font-size:x-small;margin-top:3px" >Cliente  </label>
                                                <div class="input-group " style="width:100%">                              
                                                    <asp:TextBox ID="txtCodPessoa" CssClass="form-control" name="txtCodPessoa" OnTextChanged="txtCodPessoa_TextChanged"  runat="server" TabIndex="11" AutoPostBack="true" onFocus="this.select()"                                
                                                    Width="20%" MaxLength="6" />                                                      
                                                    <asp:LinkButton ID="btnPessoa" title="Pesquise os Clientes" OnClick="btnPessoa_Click" runat="server" CssClass="form-control btn btn-primary" Width="10%" AutoPostBack="true"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtNomePessoa" CssClass="form-control" name="txtCodPessoa"  runat="server"  Enabled="false"  Width="70%"  />
                                                </div>
                                            </div> 
                                            <div class="col-md-3" style="font-size:x-small;margin-top:3px;padding-right:0!important">
                                                <label for="usr" > Aplicação do Uso </label>
                                                <asp:DropDownList ID="ddlAppUsoProducao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAppUsoProducao_SelectedIndexChanged" TabIndex="5" Enabled="true" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                            </div>
                                            <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px ">
                                                <label for="usr" style ="margin-top:1px;">Operador<span style="color:red;" title="Campo Obrigátorio">*</span></label>
                                                <asp:DropDownList ID="ddlOperador" runat="server" AutoPostBack="False" Width="100%" TabIndex="13" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                            </div> 
                                        </div>
                                        <div class="row">
                                            <%--COMPOSIÇÃO VAI AQUI, ACHAR JEITO BONITO DE MOSTRAR--%>
                                            <div class="col-md-12" >
                                                <label for="usr" style="font-size:x-small; background-color: white; border: none; text-align: left; margin-top:3px" >Produto Composto: </label>
                                                <div class="input-group " style="width:100%">
                                                    <asp:TextBox ID="txtComposto" CssClass="form-control"  runat="server" TabIndex="7" OnTextChanged="txtComposto_TextChanged" AutoPostBack="true"                                
                                                    Width="10%" MaxLength="6" />                                                      
                                                    <asp:LinkButton ID="btnPesquisarComposto" BorderStyle="Inset" runat="server" CssClass="form-control btn btn-primary" Width="5%" TabIndex="8" OnClick="btnPesquisarComposto_Click" AutoPostBack="true"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtDcrproduto" CssClass="form-control"  runat="server"  Enabled="false"  Width="85%"  AutoPostBack="true"/>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3" style="font-size:x-small;padding-right:0!important">
                                                <label for="usr" >Quantidade a Produzir </label>
                                                <asp:TextBox ID="txtQtPraProduzir" name="txtQtPraProduzir" OnTextChanged="txtQtPraProduzir_TextChanged" AutoPostBack="true" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="true"/>
                                            </div>
                                            <div class="col-md-3" style="font-size:x-small;padding-right:0!important">
                                                <label for="usr" > Quantidade Produzida </label>
                                                <asp:TextBox ID="txtQtJaProduzida" Text="0" name="txtQtJaProduzida" AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="false"/>
                                            </div>
                                            <div class="col-md-3" style="font-size:x-small;padding-right:0!important">
                                                <label for="usr"> Formato  </label>
                                                <asp:TextBox ID="txtFormato" name="txtFormato" AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="true"/>
                                            </div>
                                            <div class="col-md-3" style="font-size:x-small;">
                                                <label for="usr" > Logo Marca  </label>
                                                <asp:TextBox ID="txtLogo" name="txtLogo" AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="true"/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3" style="font-size:x-small;padding-right:0!important">
                                                <label for="usr" > Prazo  </label>
                                                <div class="input-group" style="width:100%">
                                                    <asp:TextBox ID="txtPrazo" Width="25%" style="border-right-style:none" name="txtPrazo" OnTextChanged="txtPrazo_TextChanged" AutoPostBack="true" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="true"/>
                                                    <asp:TextBox ID="txtDiasPrazo" Width="75%" Font-Bold="true" ForeColor="GrayText" style="border-left-style:none" name="txtDiasPrazo" AutoPostBack="true" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="false"/>
                                                </div>
                                            </div>
                                            <div class="col-md-3" style="font-size:x-small;padding-right:0!important">
                                                <label for="usr" > Maquina  </label>
                                                <asp:TextBox ID="txtMaquina" name="txtdtEncerramento"  AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="true"/>
                                            </div>
                                            <div class="col-md-3" style="font-size:x-small;padding-right:0!important">

                                            </div>
                                            <div class="col-md-3" style="font-size: small;padding-top:20px;">
                                                <div class="input-group" title="Valor total da Ordem de Produção">
                                                    <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="border-right:0!important;border:0!important"><b>Total:</b></span>
                                                    <span class="input-group-addon CorPadraoEscolhidaTexto CorPadraoEscolhida" style="font-weight:bold;padding-right:4px!important;font-size:20px;border:0!important;border-right:0!important;">R$</span>
                                                    <asp:TextBox ID="txtValor"  CssClass="form-control CorPadraoEscolhidaTexto  CorPadraoEscolhida" Text="0,00"  runat="server" MaxLength="50" Enabled="false" style="padding-left:0!important;font-weight:bold;font-size:20px;border:0!important;"  />
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate> 
                                </asp:UpdatePanel>
                            </div>                                
                            <div role="tabpanel" class="tab-pane" id="aba2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-12" style="font-size:x-small;">
                                                <label for="usr" style="margin-top:1px;">Observação Composição</label>
                                                <asp:TextBox ID="txtObsComposicao" CssClass="form-control" runat="server" TextMode="multiline" Columns="10" Rows="5" Font-Size="Small" TabIndex="1" MaxLength="18"/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12" style="font-size:x-small;">
                                                <label for="usr" style="margin-top:1px;">Observação Para Produção</label>
                                                <asp:TextBox ID="txtObsProducao" CssClass="form-control" runat="server" TextMode="MultiLine" ViewStateMode="Enabled" Columns="10" Rows="5" Enabled="true" Text ="" Font-Size="Small" TabIndex="1" MaxLength="18"/>
                                            </div>
                                        </div>
                                    </ContentTemplate> 
                                </asp:UpdatePanel>
                            </div> 
                            <div role="tabpanel" class="tab-pane" id="aba3" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
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
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="GrdEventoDocumento" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                    </ContentTemplate> 
                                </asp:UpdatePanel>
                            </div>
                            <div role="tabpanel" class="tab-pane" id="aba4" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px ">
                                            <label for="usr" style ="margin-top:1px;">Localização<span style="color:red;" title="Campo Obrigátorio">*</span></label>
                                            <asp:DropDownList ID="ddlLocalizacao" runat="server" AutoPostBack="False" Width="100%" TabIndex="13" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                        </div> 
                                        <div class="col-md-2" style="font-size:x-small;padding-top:20px; padding-right:0!important">
                                            <asp:LinkButton ID="btnAdicionar" Visible="false" runat="server" Text="Adicionar" CssClass="btn btn-success" OnClick="btnAdicionar_Click"> 
                                                <span aria-hidden="true" class="glyphicon glyphicon-plus"></span> Adicionar/Retirar
                                            </asp:LinkButton>
                                        </div>
                                        <div class="row" style="margin-top: 10px">
                                            <div class="col-md-12" >
                                                <div class="modal-body">
                                                    <asp:GridView ID="GridComponentes" runat="server" Width="100%"
                                                    CssClass="table table-bordered table-hover table-striped"
                                                    GridLines="None" AutoGenerateColumns="False"
                                                    Font-Size="8pt"
                                                    AutoPostBack="true"    
                                                    AllowPaging="true" PageSize="50"
                                                    PagerSettings-Mode="NumericFirstLast">
                                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                            <Columns>
                                                                <asp:BoundField DataField="CodigoRoteiro" HeaderText="Cód. 0" />
                                                                <asp:BoundField DataField="DescRoteiro" HeaderText="Roteiro 1" />
                                                                <asp:BoundField DataField="CodigoComponente" HeaderText="Código 2 " />
                                                                <asp:BoundField DataField="DescricaoComponente" HeaderText="Descrição 3"  />
                                                                <asp:BoundField DataField="Unidade" HeaderText="Unidade 4"  />
                                                                <asp:BoundField DataField="PerQuebraComponente" HeaderText="% Quebra 5"  />
                                                                <asp:BoundField DataField="QuantidadeComponente" HeaderText="Quantidade 6"  />
                                                                <asp:TemplateField HeaderText="Qt Add. 7" ItemStyle-Width="10px" ItemStyle-Height="10px">
                                                                    <ItemTemplate>                                                 
                                                                        <asp:TextBox ID="txtAdd" AutoPostBack="false" runat="server" Width="50px"  CssClass="form-control" Text='<%# Convert.ToDecimal(Eval("QuantidadeAdd").ToString()).ToString()%> '/>
                                                                    </ItemTemplate>                                                                                    
                                                                </asp:TemplateField>                                                                                   
                                                                <asp:TemplateField HeaderText="Qt Ret. 8" ItemStyle-Width="10px" ItemStyle-Height="10px" >                                                              
                                                                    <ItemTemplate>                                                                                     
                                                                        <asp:TextBox ID="txtRet" AutoPostBack="false" runat="server"  Width="50px" CssClass="form-control" Text='<%# Convert.ToDecimal(Eval("QuantidadeRet").ToString()).ToString() %> '/>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>       
                                                                <asp:BoundField DataField="QuantidadeUtil" HeaderText="Qt. Util 9" /> 
                                                                <asp:BoundField DataField="ValorCustoComponente" HeaderText="Custo 10" /> 
                                                                <asp:BoundField DataField="ValorTotal" HeaderText="Custo Total 11" /> 
                                                            </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>                   
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate> 
                                </asp:UpdatePanel>
                            </div>
                            <div role="tabpanel" class="tab-pane" id="aba5" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-1" style="padding-top:10px;padding-left:0!important; font-size:x-small;">
                                                <label for="usr" >Roteiro</label>
                                                <asp:TextBox ID="txtRoteiro"  name="txtLogo" AutoPostBack="False" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="false"/>
                                            </div>
                                            <div class="col-md-4" style="padding-top:10px;padding-left:0!important;font-size:x-small;">
                                                <label for="usr" >Descrição</label>
                                                <asp:TextBox ID="txtDescRoteiro" name="" AutoPostBack="true" CssClass="form-control" TabIndex="8" runat="server" MaxLength="15" enabled="false"/>
                                            </div>
                                            <div class="col-md-2" style="padding-top:10px;padding-left:0!important;font-size:x-small;">
                                                <label for="usr" >Dt. Inicial</label>
                                                <asp:TextBox ID="txtInicio" name="txtInicio" Text="" AutoPostBack="false" CssClass="form-control datetimepicker1" TabIndex="8" runat="server" MaxLength="15" enabled="true"/>
                                            </div>
                                            <div class="col-md-2" style="padding-top:10px;padding-left:0!important;font-size:x-small;">
                                                <label for="usr" >Dt. Final</label>
                                                <asp:TextBox ID="txtFim" name="txtFim" Text="" AutoPostBack="false" CssClass="form-control datetimepicker1" TabIndex="8" runat="server" MaxLength="15" enabled="true"/>
                                            </div>
                                            <div class="col-md-1" style="background-color: white; padding-right:0!important; border: none; text-align: left; font-size: x-small;margin-top:28px;padding-right:0!important">
                                                <asp:Button runat="server" id="BtnAddProduto" OnClientClick="this.disabled = true; this.value = '...';" UseSubmitBehavior="false" OnClick="BtnAddProduto_Click" Text="+" style="width:33px;height:33px;font-size:25px;padding-left:8px!important;padding-top:0px!important;font-weight:bold" CssClass="btn btn-success" ToolTip="Adicionar/Alterar produto" TabIndex="5"/>
                                            </div>
                                        </div>
                                        <div class="row" style="padding-top:20px">
                                            <asp:GridView ID="GridRoteiros" runat="server"
                                                CssClass="table  table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  Visible="true"
                                                OnSelectedIndexChanged="GridRoteiros_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoRoteiro" HeaderText="Cód. Roteiro" />
                                                    <asp:BoundField DataField="DescRoteiro" HeaderText="Descrição"  />
                                                    <asp:BoundField DataField="DataInicio" HeaderText="Dt. Inicio" />
                                                    <asp:BoundField DataField="DataFim" HeaderText="Dt. Fim"  />
                                                     <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate> 
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="BtnNovoAnexo" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="grdAnexo" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>         
                            </div>
                            <div role="tabpanel" class="tab-pane" id="aba6" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="btnNovoAnexo" runat="server" style="padding-left:10px; margin-bottom:10px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovoAnexo_Click" TabIndex="21" ToolTip="Add. Novo anexo ( Alt + N )"> 
                                                <span aria-hidden="true" class="glyphicon glyphicon-edit"></span> Novo Anexo
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
                            <div role="tabpanel" class="tab-pane" id="aba7" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdLogDocumento" runat="server" CssClass="table table-bordered table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  
                                                OnSelectedIndexChanged="grdLogDocumento_SelectedIndexChanged">
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
                            <div role="tabpanel" class="tab-pane" id="aba8" style="padding-left: 20px; padding-top: 0px; padding-right: 20px; font-size: small;">
                                <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:Panel id="pnlEncerramento" runat="server" Visible="false"> 
                                             <div class="row">
                                                  <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <div class="col-md-2" style="font-size:x-small; padding-right:0!important">
                                                            <label for="usr" style="margin-top:3px;">Data de Encerramento</label>
                                                            <asp:TextBox ID="txtDataEncerramento" CssClass="form-control" runat="server" Enabled="false"  TabIndex="1" MaxLength="18"/>
                                                        </div>
                                                        <div class="col-md-3" style="background-color: white;padding-right:0!important; border: none; text-align: left; font-size: x-small;margin-top:3px ">
                                                            <label for="usr" style ="margin-top:1px;">Localização da OP<span  style="color:red;" title="Campo Obrigátorio">*</span></label>
                                                            <asp:DropDownList ID="ddllocalizacaoEncerramento" runat="server" AutoPostBack="False" Width="100%" TabIndex="13" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                        </div> 
                                                         <div class="col-md-2" style="font-size:x-small; padding-right:0!important">
                                                            <label for="usr" style="margin-top:4px;">Quantidade Produzida</label>
                                                            <asp:TextBox ID="txtQtFinal" CssClass="form-control" runat="server" OnTextChanged="QtFinal_TextChanged" AutoPostBack="true" Text =""  TabIndex="1" MaxLength="18"/>
                                                        </div>
                                                          <div class="col-md-2" style="font-size:x-small;padding-top:20px; padding-right:0!important">
                                                             <asp:LinkButton ID="btnEncerramento"  runat="server" Text="Encerrar" CssClass="btn btn-success" OnClick="btnEncerramento_Click"> 
                                                                <span aria-hidden="true" class="glyphicon glyphicon-check"></span>  Encerrar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </ContentTemplate> 
                                                </asp:UpdatePanel> 
                                            </div>
                                        </asp:Panel>
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
    <%--Cancelar--%>
<asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="CaixaCancelar" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" style="height:100%;width:350px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H3">Atenção</h4>
                        </div>
                        <div class="modal-body"> 
                            <%--ou seja, só cancela--%>
                            <asp:Label id="lblMens" Text="" runat="server" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnCfmSim" OnClientClick="this.disabled = true; this.value = 'Cancelando...';" UseSubmitBehavior="false" OnClick="btnCfmSim_Click" CssClass="btn btn-danger" style="color:white" Text="Cancelar" />
                            <%--<button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>--%>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCfmSim"  />
        </Triggers>
    </asp:UpdatePanel>
    <%--pessoas--%>
<div class="modal fade" id="CaixaPessoas"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
    <div class="modal-dialog" role="document" style ="height:300px;width:520px">
        <div class="panel panel-primary">
            <div class="panel-heading" style="height:40px"> 
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <label class="modal-title" id="H1"> Consulta Clientes</label>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Always">
                    <ContentTemplate>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-11" >
                                <label for="usr"> Consultar o nome do Cliente:</label>
                                <asp:TextBox ID="txtPesquisaCliente"  PlaceHolder="No minimo 3 Caracteres ..." OnTextChanged="txtPesquisaCliente_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-11" >
                                <asp:label id="Label1" runat="server" for="usr" ForeColor="OrangeRed"> </asp:label>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="panel-body" style="overflow-x:scroll;height:300px;width:500px">
                                <asp:GridView ID="GridPessoas" runat="server" Width="100%"
                                    CssClass="table table-bordered table-hover table-striped"
                                    GridLines="None" AutoGenerateColumns="False"
                                    Font-Size="8pt"
                                    OnSelectedIndexChanged="GridPessoas_SelectedIndexChanged"
                                    AllowPaging="true" PageSize="50"
                                    PagerSettings-Mode="NumericFirstLast">
                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                        <Columns>
                                            <asp:BoundField DataField="CodigoPessoa" HeaderText="Código"/>
                                            <asp:BoundField DataField="NomePessoa" HeaderText="Nome Pessoa"/>
                                            <asp:BoundField DataField="NomeFantasia" HeaderText="Nome Fantasia"/>
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
    <%--compostos--%>
<div class="modal fade" id="CaixaCompostos"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
    <div class="modal-dialog" role="document" style ="height:300px;width:520px">
        <div class="panel panel-primary">
            <div class="panel-heading" style="height:40px"> 
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <label class="modal-title" id="H2"> Consultar Descrição do Composto</label>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server"  UpdateMode="Always">
                    <ContentTemplate>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-11" >
                                <label for="usr"> Digite a Descrição do Composto:</label>
                                <asp:TextBox ID="txtPesquisaComposto"  PlaceHolder="No minimo 3 Caracteres ..." OnTextChanged="txtPesquisaComposto_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-11" >
                                <asp:label id="Label2" runat="server" for="usr" ForeColor="OrangeRed"> </asp:label>
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
                                    PagerSettings-Mode="NumericFirstLast">
                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                        <Columns>
                                            <asp:BoundField DataField="CodigoProdutoComposto" HeaderText="Código" />
                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição"  />
                                            <asp:BoundField DataField="DescricaoSituacao" HeaderText="Situação"/>
                                            <asp:BoundField DataField="ValorCustoProduto" HeaderText="Valor Compra" DataFormatString="{0:C}"/>
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
    <%--documento original--%>
<div class="modal fade" id="CaixaDocumentoOriginal"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
    <div class="modal-dialog" role="document" style ="height:300px;width:800px">
        <div class="panel panel-primary">
            <div class="panel-heading" style="height:40px"> 
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <label class="modal-title" id="H4"> Consulta de Pedidos de Venda</label>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server"  UpdateMode="Always">
                    <ContentTemplate>
                        <div class="row" style="margin-top: 10px; padding-left:0!important">
                            <div class="col-md-6" >
                                <label for="usr"> Nome do Cliente:</label>
                                <asp:TextBox ID="txtPesquisaClienteDocumento"  PlaceHolder="No minimo 3 Caracteres ..." OnTextChanged="txtPesquisaClienteDocumento_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" />
                            </div>
                            <div class="col-md-6" >
                                <label for="usr"> Nº do Pedido:</label>
                                <asp:TextBox ID="txtPesquisaDocumento" OnTextChanged="txtPesquisaDocumento_TextChanged" CssClass="form-control" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="8" />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-11" >
                                <asp:label id="Label3" runat="server" for="usr" ForeColor="OrangeRed"> </asp:label>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="panel-body" style="overflow-x:scroll;height:300px;width:780px">
                                <asp:GridView ID="gridDocumento" Visible="true" runat="server" Width="100%"
                                    CssClass="table table-bordered table-hover table-striped"
                                    GridLines="None" AutoGenerateColumns="False"
                                    Font-Size="8pt"
                                    OnSelectedIndexChanged="gridDocumento_SelectedIndexChanged"
                                    AllowPaging="true" PageSize="50"
                                    PagerSettings-Mode="NumericFirstLast">
                                        <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                        <Columns>
                                            <asp:BoundField DataField="CodigoDocumento" HeaderText="Código" ItemStyle-CssClass="padding-top-15" />
                                            <asp:BoundField DataField="NumeroDocumento" HeaderText="Pedido"  ItemStyle-CssClass="padding-top-15"/>
                                            <asp:BoundField DataField="DataHoraEmissao" HeaderText="Dt. Emissão"  ItemStyle-CssClass="padding-top-15 "/>   
                                            <asp:BoundField DataField="Cpl_DsTipoOperacao" HeaderText="Tipo de Operação" ItemStyle-CssClass="padding-top-15" />
                                            <asp:BoundField DataField="Cpl_CodigoPessoa" HeaderText="Cód." ItemStyle-CssClass="padding-top-15" />
                                            <asp:BoundField DataField="Cpl_Pessoa" HeaderText="Cliente" ItemStyle-CssClass="padding-top-15" />
                                            <asp:BoundField DataField="CodigoComposto" HeaderText="Composto"  ItemStyle-CssClass="padding-top-15 noprint" HeaderStyle-CssClass="noprint"/>
                                            <asp:BoundField DataField="Cpl_NomeProduto" HeaderText="Descrição"  ItemStyle-CssClass="padding-top-15 noprint" HeaderStyle-CssClass="noprint"/>
                                            <asp:BoundField DataField="Prazo" HeaderStyle-CssClass="noPrint" ItemStyle-CssClass="NoPrint" HeaderText="Prazo"/>
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




</asp:Content>
