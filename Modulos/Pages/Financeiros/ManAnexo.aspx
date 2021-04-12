<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" 
    CodeBehind="ManAnexo.aspx.cs" Inherits="SoftHabilInformatica.Pages.Financeiros.ManAnexo"  
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $('.js-example-basic-single').select2({
                
            });
        });
        function VerificaTamanhoArquivo() {

            var fi = document.getElementById('<%= arquivo.ClientID %>'); 
            var maxFileSize = 10480760 // 4MB -> 4 * 1024 * 1024
       
            if (fi.files.length > 0) {
       
                for (var i = 0; i <= fi.files.length - 1; i++) {
                
                    var fsize = fi.files.item(i).size;

                    if (fsize > maxFileSize) {

                        alert("Tamanho do Arquivo deve ser no maximo 10MB");
                    
                        fi.value = null;
                    }

                }
            }
        }

        $(document).on('keydown', function (e) {

            if ((e.altKey) && (e.which === 83)) { 
                $get('<%=btnSalvar.ClientID%>').click(); 
            }
            else if ((e.altKey) && (e.which === 86)) { 
                $get('<%=btnVoltar.ClientID%>').click(); 
            }

        });
    </script>


    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtdtemissao']").datepicker({
                todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",
                locate: "pt-BR"
            });
            $("input[id*='txtdtvencimento']").datepicker({
                todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",
                locate: "pt-BR"
            });
            $("input[id*='txtdtentrada']").datepicker({
                todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",
                locate: "pt-BR"
            });
            $("input[id*='txtdtBaixa']").datepicker({
                todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",
                locate: "pt-BR"
            });
        });
    </script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Anexo
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>

            </div>
            <div class="panel-body">

                <div class="row" style="background-color: white; border: none;">
                    <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                            
                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcluir" runat="server" Text="Inativar" CssClass="btn btn-danger" TabIndex="3" data-toggle="modal" data-target="#myexcpes"> 
                            <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnDownload" runat="server" Text="Download" CssClass="btn btn-primary" TabIndex="3" OnClick="btnDownload_Click" UseSubmitBehavior="false" >
                            <span aria-hidden="true" class="glyphicon glyphicon-save"></span>  Download
                        </asp:LinkButton>
                    </div>                      
                </div>
                    <br />

                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none;">
                      
                        <asp:HiddenField ID="TabName" runat="server"/>
            
                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;margin-left:15px" >

                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                                <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexo</a></li>
                                <li role="presentation" style='<%= DisplayInformacoes %>'><a href="#consulta" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-file"></span>&nbsp;&nbsp;Informações</a></li>
                            </ul>

                            <!-- Tab panes -->
                            <div class="tab-content" runat="server" id="PanelContext">
                                <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 0px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Sequência</label>
                                        <asp:TextBox ID="txtSequencia" CssClass="form-control" runat="server" Text ="" Font-Size="Small" enabled="false"/>
                                    </div>
                                   
                                    <div class="col-md-9" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Descrição do Arquivo</label>
                                        <asp:TextBox ID="txtNome" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100"/>
                                    </div>  
                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <asp:label runat="server" for="usr" style ="margin-top:1px; font-weight:bold;" ID="txtDSNome">Nome do Arquivo</asp:label>
                                        <asp:TextBox ID="txtGUID" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" enabled="false" />
                                    </div>
                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:5px" >
                                        <asp:label runat="server" for="usr" style ="margin-top:1px; font-weight:bold; " id="titulo">Importação do Arquivo</asp:label>
                                       
                                        <asp:FileUpload ID="arquivo" runat="server"  CssClass="form-control" Text="" multiple="multiple"  onchange="Javascript: VerificaTamanhoArquivo();" />
                                    </div> 
                                    
                                    
                                    
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">Informações do Documento</div>
                                            <div class="panel-body">
                                                <asp:Panel runat="server" ID="pnlCtaPagar">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarCodigo" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Lançamento</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarLancamento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarNroDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Classificação</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarClassificacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Data Vencimento</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarVencimento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cobrança</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarCobranca" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Plano Contas</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarPlanoConta" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor Documento</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarValorDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor Acréscimo</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarValorAcres" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor Desconto</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarValorDesc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                     <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Credor</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarCredor" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Observação</label>
                                                        <asp:TextBox ID="txtPnlCtaPagarOBS" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>   
                                                <asp:Panel runat="server" ID="pnlCtaReceber">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberCodigo" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Lançamento</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberLancamento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberNroDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Classificação</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberClassificacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Data Vencimento</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberVencimento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cobrança</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberCobranca" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Plano Contas</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberPlanoConta" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor Documento</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberValorDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor Acréscimo</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberValorAcres" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor Desconto</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberValorDesc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                     <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Devedor</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberDevedor" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Observação</label>
                                                        <asp:TextBox ID="txtPnlCtaReceberOBS" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>   
                                                <asp:Panel runat="server" ID="pnlOrcamentoPedido">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoCodigo" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Validade</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoValidade" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoNroDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoSerie" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    
                                                     <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Pessoa</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoPessoa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cobranca</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoCobranca" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Descrição</label>
                                                        <asp:TextBox ID="txtPnlOrcamentoDescricao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>   
                                                <asp:Panel runat="server" ID="pnlNFSe">
                                                     <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtPnlNFSeCodigo" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="txtPnlNFSeEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Lançamento</label>
                                                        <asp:TextBox ID="txtPnlNFSeLancamento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtPnlNFSeSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtPnlNFSeEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtPnlNFSeNroDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="txtPnlNFSeSerie" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor PIS</label>
                                                        <asp:TextBox ID="txtPnlNFSePIS" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor Cofins</label>
                                                        <asp:TextBox ID="txtPnlNFSeCofins" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor CSLL</label>
                                                        <asp:TextBox ID="txtPnlNFSeCSLL" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor IRRF</label>
                                                        <asp:TextBox ID="txtPnlNFSeIRRF" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor INSS</label>
                                                        <asp:TextBox ID="txtPnlNFSeINSS" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor outras retenções</label>
                                                        <asp:TextBox ID="txtPnlNFSeOutras" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                     <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Tomador</label>
                                                        <asp:TextBox ID="txtPnlNFSeTomador" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Alíquota</label>
                                                        <asp:TextBox ID="txtPnlNFSeAliquota" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Municipio Prestador</label>
                                                        <asp:TextBox ID="txtPnlNFSeMun" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Descrição</label>
                                                        <asp:TextBox ID="txtPnlNFSeDescricao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>   
                                                <asp:Panel runat="server" ID="pnlSolAtendimento">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtPnlSolCodigo" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="txtPnlSolEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Entrada</label>
                                                        <asp:TextBox ID="txtPnlSolEntrada" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtPnlSolSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtPnlSolEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtPnlSolNroDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="txtPnlSolSerie" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                  
                                                     <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Pessoa</label>
                                                        <asp:TextBox ID="txtPnlSolPessoa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Email Solicitante</label>
                                                        <asp:TextBox ID="txtPnlSolMailSolicitante" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Telefone Solicitante</label>
                                                        <asp:TextBox ID="txtPnlSolFoneSolicitante" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Nível Prioridade</label>
                                                        <asp:TextBox ID="txtPnlSolNvlPrioridade" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Descrição</label>
                                                        <asp:TextBox ID="txtPnlSolDescricao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>   
                                                <asp:Panel runat="server" ID="pnlOS">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtPnlOSCodigo" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Classificação</label>
                                                        <asp:TextBox ID="txtPnlOSClassificacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissao</label>
                                                        <asp:TextBox ID="txtPnlOSEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtPnlOSSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtPnlOSEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtPnlOSNroDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="txtPnlOSSerie" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Solicitação Atendimento</label>
                                                        <asp:TextBox ID="txtPnlOSSol" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Usuario Responsável</label>
                                                        <asp:TextBox ID="txtPnlOSUSu" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Email Solicitante</label>
                                                        <asp:TextBox ID="txtPnlOSMailSolicitante" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Telefone Solicitante</label>
                                                        <asp:TextBox ID="txtPnlOSFoneSolicitante" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                     <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cliente</label>
                                                        <asp:TextBox ID="txtPnlOSPessoa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Observação</label>
                                                        <asp:TextBox ID="txtPnlOSOBS" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel> 
                                                <asp:Panel runat="server" ID="pnlCTe">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtPnlCTeCodigo" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Data/Hora Lançamemento</label>
                                                        <asp:TextBox ID="txtPnlCTeDtLancamento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Data/Hora Emissao</label>
                                                        <asp:TextBox ID="txtPnlCTeEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtPnlCTeSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">TipoOperação</label>
                                                        <asp:TextBox ID="txtPnlCTeTipoOperacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtPnlCTeEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtPnlCTeNroDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="txtPnlCTeSerieDoc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Transportador</label>
                                                        <asp:TextBox ID="txtPnlCTeTransportador" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Chave Acesso</label>
                                                        <asp:TextBox ID="txtPnlCTeChaveAcesso" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Remetente</label>
                                                        <asp:TextBox ID="txtPnlCTeRemetente" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Destinatário</label>
                                                        <asp:TextBox ID="txtPnlCTeDestinatario" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                     <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Tomador</label>
                                                        <asp:TextBox ID="txtPnlCTeTomador" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Recebedor</label>
                                                        <asp:TextBox ID="txtPnlCTeRecebedor" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Valor Total</label>
                                                        <asp:TextBox ID="txtPnlCTeVlTotal" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Observação</label>
                                                        <asp:TextBox ID="txtPnlCTeOBS" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlOrdProducao">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="txtDtEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Encerramento</label>
                                                        <asp:TextBox ID="txtDtEncerramento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Nº Documento</label>
                                                        <asp:TextBox ID="txtNrDocumento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="txtSrDocumento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    
                                                     <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Pessoa</label>
                                                        <asp:TextBox ID="txtPessoa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Descrição</label>
                                                        <asp:TextBox ID="txtDesc" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlOrdCompra">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="TextBox11" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="TextBox12" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Validade</label>
                                                        <asp:TextBox ID="TextBox13" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="TextBox14" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="TextBox15" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="TextBox16" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="TextBox17" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    
                                                     <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Pessoa</label>
                                                        <asp:TextBox ID="TextBox18" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cobranca</label>
                                                        <asp:TextBox ID="TextBox19" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Descrição</label>
                                                        <asp:TextBox ID="TextBox20" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlSolCompra">
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="TextBox21" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="TextBox22" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Validade</label>
                                                        <asp:TextBox ID="TextBox23" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="TextBox24" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="TextBox25" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="TextBox26" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="TextBox27" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    
                                                     <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Pessoa</label>
                                                        <asp:TextBox ID="TextBox28" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cobranca</label>
                                                        <asp:TextBox ID="TextBox29" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Descrição</label>
                                                        <asp:TextBox ID="TextBox30" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlNF">
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Documento</label>
                                                        <asp:TextBox ID="txtPnlNFCodigoDocumento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                        <asp:TextBox ID="txtPnlNFEmissao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Situação</label>
                                                        <asp:TextBox ID="txtPnlNFSituacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                        <asp:TextBox ID="txtPnlNFEmpresa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                        <asp:TextBox ID="txtPnlNFNroDocumento" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Série Documento</label>
                                                        <asp:TextBox ID="txtPnlNFSerie" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    
                                                     <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Pessoa</label>
                                                        <asp:TextBox ID="txtPnlNFPessoa" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Cobranca</label>
                                                        <asp:TextBox ID="txtPnlNFCobranca" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <label for="usr" style ="margin-top:1px;">Observação</label>
                                                        <asp:TextBox ID="txtPnlNFObservacao" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" MaxLength="100" Enabled="false"/>
                                                    </div>
                                                </asp:Panel>
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


    <!-- Exclui Grupo -->
    <div class="modal fade" id="myexcpes" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir este Anexo do Documento ?
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="False" OnClick="btnCfmSim_Click"></asp:LinkButton>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
    

</asp:Content>
