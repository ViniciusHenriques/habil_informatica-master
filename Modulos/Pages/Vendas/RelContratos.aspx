 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="RelContratos.aspx.cs" Inherits="SoftHabilInformatica.Pages.Vendas.RelContratos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
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
        function MostrarDadosExcel(title) {
            $("#ModalExcel .modal-title").html(title);
            $("#ModalExcel").modal("show");
        }

        $(document).ready(function (e) {
            $('.js-example-basic-single').select2({
            });
        });
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({
                    
            });
        }

        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
    </script>
    <body>
        <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
            <div class="panel panel-primary" >
                <div  class="panel-heading">Relatório de Contratos de cliente
                    <div class="messagealert" id="alert_container"></div>
                </div>            
                <div class="panel-body">
                    <div class="row">   
                        <div class="col-md-12">
                            <div class="col-md-7">
                                <asp:LinkButton ID="btnFechar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnFechar_Click" > 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-off"></span>  Fechar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnGerarPlanilha" runat="server" CssClass="btn btn-success" TabIndex="0" OnClick="btnGerarPlanilha_Click" AutoPostBack="true" title="Transforme seus XMLs em uma Planilha excel"> 
                                    <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span>  Gerar planilha
                                </asp:LinkButton>
                            </div>
                            
                        </div>
                        <div class="col-md-12" style="margin-top:20px">
                            <div id="Tabs" role="tabpanel" >
                                <!-- Nav tabs -->
                                <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                                    <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-filter"></span>&nbsp;&nbsp;Geração relatório</a></li>
                                    <li role="presentation"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Importação planilha</a></li>
                                </ul>
                                <!-- Tab panes -->
                            <div class="tab-content" runat="server" id="PanelContext">
                                <div role="tabpanel" class="tab-pane" id="home">
                                    <div class ="col-md-12" style="margin-top:15px">
                                        <div class="col-md-4" style=" font-size:x-small;">
                                            <label for="usr" style ="margin-top:3px;">Empresa</label>
                                            <asp:DropDownList ID="ddlEmpresa" runat="server"  TabIndex="0"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                        </div>
                                         <div class="col-md-8" style="padding:0!important" >
                                             <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                                 <ContentTemplate>
                                                    <div class="col-md-4" style=" font-size:x-small;">
                                                        <label for="usr" style ="margin-top:3px;">Grupo de Cliente</label>
                                                        <asp:DropDownList ID="ddlGpoCliente" runat="server"  TabIndex="1"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlGpoCliente_TextChanged" AutoPostBack="true"/>
                                                    </div>
                                                     <div class="col-md-8" style=" font-size:x-small;">
                                                        <label for="usr" style ="margin-top:3px;">Cliente</label>
                                                        <asp:DropDownList ID="ddlCliente" runat="server"  TabIndex="2"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                                    </div>
                                                    </ContentTemplate> 
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlGpoCliente" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel> 
                                        </div>
                                        <div class="col-md-6" style="margin-top:15px;padding:0!important" >
                                            <div class="col-md-12" >
                                                <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding: 5px!important">Somar ST quando não for estado origem</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkSomarST" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                                 <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding: 5px!important">Ordem de item do orçamento</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkOrdemItem" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding: 5px!important">Adicionar (Peso/CEST) produto</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkAdicionarCESTPESO" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding: 5px!important">Usar <br/> HyperLink</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkHyperlink" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12" >
                                                <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding: 5px!important">Adicionar <br/> Código Barras</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkCodBarras" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding: 5px!important">Preço <br/> único</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkPrecoUnico" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding: 5px!important">Apenas produtos<br/> inativos</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkProdutosInativos" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding: 5px!important">Adicionar origem <br/> produto</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkOrigem" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12" >
                                                <div class="panel panel-primary col-md-3 checkbox-opcoes">
                                                    <div class="panel-heading" style="padding:5px!important">Grupo de <br/> produtos</div>
                                                    <div class="panel-body checkbox">
                                                        <label style="padding: 0!important; margin: 0!important">
                                                            <asp:CheckBox runat="server" ID="ChkGrupoProdutos" />
                                                            <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6" style="margin-top:15px;">
                                             <div class="panel panel-primary " style="text-align:center;">
                                                <div class="panel-heading" style="padding: 5px!important">Estados</div>
                                                <div class="panel-body">
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">AC</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkAC" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">AL</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkAL" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">AP</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkAP" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">AM</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkAM" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">BA</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkBA" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">CE</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkCE" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">ES</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkES" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">GO</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkGO" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">MA</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkMA" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">MT</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkMT" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">MS</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkMS" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">MG</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkMG" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">PA</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkPA" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">PB</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkPB" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">PR</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkPR" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">PE</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkPE" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">PI</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkPI" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">RJ</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkRJ" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">RN</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkRN" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">RS</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkRS" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                     <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">RO</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkRO" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">RR</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkRR" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">SC</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkSC" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">SP</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkSP" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">SE</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkSE" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="panel panel-primary checkbox-dia-semana col-md-1" >
                                                        <div class="panel-heading" style="padding: 5px!important">TO</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkTO" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="panel panel-primary checkbox-dia-semana col-md-1">
                                                        <div class="panel-heading" style="padding: 5px!important">DF</div>
                                                        <div class="panel-body checkbox">
                                                            <label style="padding: 0!important; margin: 0!important">
                                                                <asp:CheckBox runat="server" ID="ChkDF" />
                                                                <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="profile">
                                    <div class="col-md-5">
                                        <asp:LinkButton runat="server" id="btnArquivo"  CssClass="btn btn-default" ToolTip="Escolha o arquivo xlsx">
                                            <asp:FileUpload  runat="server" id="fileselector" type="file" style="margin:0"  />
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnImportar" runat="server" CssClass="btn alert-info" TabIndex="0" OnClick="btnImportar_Click"> 
                                            <span aria-hidden="true" class="glyphicon glyphicon-open"></span>  Importar planilha
                                        </asp:LinkButton>                                
                                    </div>
                                     <div class="col-md-7">
                                         <div class="panel panel-primary ">
                                            <div class="panel-heading" >Layout de Importação</div>
                                            <div class="panel-body ">
                                                <img src="../../Images/ModeloImportacaoContratos.png" width="100%"/>                              
                                                    
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
        <div class="modal fade" id="ModalExcel" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
            <div class="modal-dialog" role="document" style="width:800px" >
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Atenção! Deseja importar esses dados?</h4>
                        
                    </div>
                    <div class="modal-body">   
                        <div style="float:right;margin-top:10px">
                            <asp:Button runat="server" ID="btnSim" OnClientClick="this.disabled = true; this.value = 'Importando...';" UseSubmitBehavior="false" OnClick="btnSim_Click" CssClass="btn btn-danger" style="color:white" Text="Confirmar" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                        <asp:GridView ID="grdXMLs" runat="server"
                                CssClass="table  table-hover table-striped"
                                GridLines="None" 
                                Font-Size="8pt"  >
                            <PagerStyle HorizontalAlign = "left"   CssClass = "GridPager" />
                            <Columns>   
                                            
                            </Columns>
                            <RowStyle CssClass="cursor-pointer" />
                        </asp:GridView>
                    </div>
                    
                </div>
            </div>
        </div>
    </body>
</asp:Content>
