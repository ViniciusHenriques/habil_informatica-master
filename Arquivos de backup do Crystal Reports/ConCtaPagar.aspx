﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConCtaPagar.aspx.cs" 
    Inherits="SoftHabilInformatica.Pages.Financeiros.ConCtaPagar"  MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });

        
        $(document).ready(function () {
            $('.js-example-basic-single').select2({

            });
        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
             $('.js-example-basic-single').select2({
                });

        }
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <style type="text/css">

        @media screen and (max-width: 600px) {
            .noprint{display:none;}          
        }

    </style>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />



    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtfiltrodata11']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtfiltrodata12']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtfiltrodata21']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtfiltrodata22']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtfiltrodata31']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtfiltrodata32']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
        });
    </script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary" >
            <div  class="panel-heading">Consulta do Contas a Pagar
                <div class="messagealert" id="alert_container"></div>
            </div>
            
            <div class="panel-body">
                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" Visible="false" OnClick="btnVoltar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>
                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>
                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
                <br />
                <br />
                <asp:HiddenField ID="TabName" runat="server"/>
            
                <div id="Tabs" role="tabpanel" >

                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                        <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-filter"></span>&nbsp;&nbsp;Seleção dos Dados</a></li>
                        <li role="presentation"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Consulta</a></li>
                    </ul>
                    <!-- Tab panes -->
                    <div class="tab-content" runat="server" id="PanelContext">
                        <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                            <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
                                <ContentTemplate>

                                    
                                    <div class="row" style="background-color:white;border:none;">
                                        
                                        <asp:Panel ID="pnlFiltroData1" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltroData1" CssClass="form-control" Text="Filtro 1" name="lblFiltroData1" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtfiltrodata11" CssClass="form-control" name="txtfiltrodata11" runat="server"  MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtfiltrodata12" CssClass="form-control" name="txtfiltrodata12" runat="server"  MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>
                                            
                                        <asp:Panel ID="pnlFiltroData2" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltroData2" CssClass="form-control" Text="Filtro 2" name="lblFiltroData2" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtfiltrodata21" CssClass="form-control" name="txtfiltrodata21" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtfiltrodata22" CssClass="form-control" name="txtfiltrodata22" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltroData3" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltroData3" CssClass="form-control" Text="Filtro 1" name="lblFiltroData3" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtfiltrodata31" CssClass="form-control" name="txtfiltrodata31" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtfiltrodata32" CssClass="form-control" name="txtfiltrodata23" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro1" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro1" CssClass="form-control" Text="Filtro 1" name="lblFiltro1" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro11" CssClass="form-control" name="txtFiltro11" runat="server"  MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro12" CssClass="form-control" name="txtFiltro12" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro2" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro2" CssClass="form-control" Text="Filtro 2" name="lblFiltro2" runat="server" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro21" CssClass="form-control" name="txtFiltro21" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro22" CssClass="form-control" name="txtFiltro22" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro3" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro3" CssClass="form-control" Text="Filtro 3: " name="lblFiltro3" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro31" CssClass="form-control" name="txtFiltro31" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro32" CssClass="form-control" name="txtFiltro32" runat="server" MaxLength="200" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro4" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro4" CssClass="form-control" Text="Filtro 4: " name="lblFiltro4" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro41" CssClass="form-control" name="txtFiltro41" runat="server"  MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro42" CssClass="form-control" name="txtFiltro42" runat="server"  MaxLength="200" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro5" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro5" CssClass="form-control" Text="Filtro 5: " name="lblFiltro5" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro51" CssClass="form-control" name="txtFiltro51" runat="server"  MaxLength="100" />

                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro52" CssClass="form-control" name="txtFiltro52" runat="server"  MaxLength="200" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro6" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro6" CssClass="form-control" Text="Filtro 6: " name="lblDescricao2" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro61" CssClass="form-control" name="txtFiltro61" runat="server"  MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro62" CssClass="form-control" name="txtFiltro62" runat="server"  MaxLength="200" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro7" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro7" CssClass="form-control" Text="Filtro 7: " name="lblDescricao2" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro71" CssClass="form-control" name="txtFiltro71" runat="server"  MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro72" CssClass="form-control" name="txtFiltro72" runat="server"  MaxLength="200" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro8" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro8" CssClass="form-control" Text="Filtro 8: " name="lblDescricao2" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro81" CssClass="form-control" name="txtFiltro81" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro82" CssClass="form-control" name="txtFiltro82" runat="server" MaxLength="200" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro9" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro9" CssClass="form-control" Text="Filtro 9: " name="lblDescricao2" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro91" CssClass="form-control" name="txtFiltro91" runat="server" MaxLength="100" />

                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro92" CssClass="form-control" name="txtFiltro92" runat="server" MaxLength="200" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>

                                        <asp:Panel ID="pnlFiltro10" runat="server" Visible="false">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <asp:Label ID="lblFiltro10" CssClass="form-control" Text="Filtro 10: " name="lblDescricao2" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
                                            </div>
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">De :</span>
                                                    <asp:TextBox ID="txtFiltro101" CssClass="form-control" name="txtFiltro101" runat="server" MaxLength="100" />
                                                </div>
                                            </div>                                                    
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;">
                                                <div class="input-group">
                                                    <span class="input-group-addon">Até: </span>
                                                    <asp:TextBox ID="txtFiltro102" CssClass="form-control" name="txtFiltro102" runat="server" MaxLength="200" />
                                                </div>
                                            </div>                                                    
                                        </asp:Panel>
                                        <div class="col-md-12"></div>
                                        <div class="col-md-6" >
                                                <div class="input-group">
                                                    <span class="input-group-addon">Tipo Cobrança : &nbsp;&nbsp;</span>
                                                    <asp:DropDownList ID="ddlTipoCobranca" CssClass="form-control js-example-basic-single" runat="server" Font-Size="Small" Width="100%" >
                                                         
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        <div class="col-md-6" >
                                                <div class="input-group">
                                                    <span class="input-group-addon">Plano de Conta : &nbsp;&nbsp;</span>
                                                    <asp:DropDownList ID="ddlPlanoConta" CssClass="form-control js-example-basic-single" runat="server" Font-Size="Small" Width="100%">
                                                         
                                                    </asp:DropDownList>
                                                </div>
                                            </div>                                                                           
                                    </div>

                                    <br/>
                                    <div class="container-fluid">
                                        
                                        <div class="row" style="background-color:white;border:none;">
                                            <div class="col-md-2" ><label for="usr" style ="margin-top:10px;">Quantidade de Registros</label></div>
                                            
                                            <div class="col-md-1" >
                                                <asp:DropDownList ID="ddlRegistros" CssClass="form-control" runat="server" Font-Size="Medium"   OnSelectedIndexChanged="ddlRegistros_SelectedIndexChanged">
                                                    <asp:ListItem Value="50" Text="50" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="100" Text="100"></asp:ListItem>
                                                    <asp:ListItem Value="1000" Text="1000"></asp:ListItem>
                                                    <asp:ListItem Value="32600" Text="Todas"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3" >
                                                <div class="input-group">
                                                    <span class="input-group-addon">Situação : &nbsp;&nbsp;</span>
                                                    <asp:DropDownList ID="ddlTipoBaixa" CssClass="form-control" runat="server" Font-Size="Medium" >
                                                         
                                                        <asp:ListItem Value="0" Text="Todos" ></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Aberto" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Baixado"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="Cancelado"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-1" >
                                                <asp:LinkButton ID="btnConsultar" runat="server" CssClass="btn btn-default" OnClick="btnConsultar_Click"> 
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

                        <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                            <br />
                            <asp:LinkButton ID="btnVoltarSelecao" runat="server" Text="Nova Consulta" CssClass="btn btn-default" UseSubmitBehavior="false" OnClick="btnVoltarSelecao_Click"> 
                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>Nova Consulta
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnImprimir" runat="server" Text="Nova Consulta" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnImprimir_Click"> 
                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-print"></span>   Imprimir
                            </asp:LinkButton>
                            <br />
                            <br />
                            <asp:GridView ID="grdGrid" runat="server" CssClass="table table-bordered table-striped" Width="100%" 
                                GridLines="None" AutoGenerateColumns="False"
                                Font-Size="8pt"  
                                OnSelectedIndexChanged="grdGrid_SelectedIndexChanged"
                                PageSize="50" AllowPaging="true"
                                OnPageIndexChanging="grdGrid_PageIndexChanging"
                                PagerSettings-Mode ="NumericFirstLast"  >
                                <PagerStyle HorizontalAlign = "Right"   CssClass = "GridPager" />
                                <Columns>
                                    <asp:BoundField DataField="CodigoDocumento" HeaderText="Código"  />
                                    <asp:BoundField DataField="DGDocumento" HeaderText="Documento" />
                                    <asp:BoundField DataField="DataEmissao" HeaderText="Dt. Emissão" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="DataVencimento" HeaderText="Dt. Vencimento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="Cpl_NomeFornecedor" HeaderText="Credor" />
                                    
                                    <asp:BoundField DataField="ValorDocumento" HeaderText="Vl. Titulo"  ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="ValorDesconto" HeaderText="Vl. Desc." ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="ValorAcrescimo" HeaderText="Vl. Acrésc." ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="ValorGeral" HeaderText="Vl. Total" />
                                    <asp:BoundField DataField="Cpl_vlPago" HeaderText="Vl. Pago" />
                                    <asp:BoundField DataField="Cpl_vlPagar" HeaderText="Vl. Aberto" />
                                    <asp:BoundField DataField="Cpl_DsSituacao" HeaderText="Situação" />
                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<%--  --%>