<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConNotaFiscalServico.aspx.cs" Inherits="SoftHabilInformatica.Pages.Servicos.ConNotaFiscalServico"   MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script type="text/javascript" src="javascript/jquery.js"></script>

    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script>
         
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="'+ item +'"]').tab('show');            

            console.log($(item));
        });


        function ShowPopup() {
            $("#btnShowPopup").click();
        }


        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
        function ShowPopup2(title, body) {
            $("#MyPopup2 .modal-title").html(title);
            $("#MyPopup2 .modal-body").html(body);
            $("#MyPopup2").modal("show");
        }
        function ShowPopup3(title, body) {
            $("#MyPopup3 .modal-title").html(title);
            $("#MyPopup3 .modal-body").html(body);
            $("#MyPopup3").modal("show");
        }

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

        }
        
        
       
    </script>
    <style type="text/css">       
        .buttonGrid{
            padding:0!important;           
        }
        .GridPager{
            text-align:center!important;
            glyph-orientation-horizontal:inherit;
        }
        @media screen and (max-width: 600px) {
            .noprint{display:none;}          
        }
        .listButton{
            background-color:transparent;
            border:0!important;
            padding:1px!important;
            padding-left:20px!important;
            padding-right:20px!important;
        }
        .dropdown-menu{
            padding-top:1px!important;
        }  
   
    </style>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>

     <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtfiltrodata11']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtfiltrodata12']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtfiltrodata21']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtfiltrodata22']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});

         });
        $(document).ready(function(){
            //Esconde preloader
            $(window).load(function(){
                $('#preloader2').fadeOut(500);//1500 é a duração do efeito (1.5 seg)
            });
        });
    </script>
    <style type="text/css">
        
        #preloader2 {
            position: absolute; /* posição absoluta ao elemento pai, neste caso o BODY */
            left: 50%!important; 
            top: 60%;
            width: 300px; /* Largura da DIV */
            height: 200px; /* Altura da DIV */
            opacity:0.8;
            margin-left: -150px;
            margin-top: -125px;
            background-color: #FFF;
            color: #FFF;
            background-color: #FFF;
            text-align: center; /* Centraliza o texto */
            z-index: 1000; /* Faz com que fique sobre todos os elementos da página */
        }
    </style>
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <div class="panel panel-primary" >
            <div  class="panel-heading panel-heading-padrao">Consulta de Nota Fiscal de Serviço
                <div class="messagealert" id="alert_container"></div>
            </div>
            
            <div class="panel-body" >
                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>

                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>

                <br />
                <br />
      
                <div id="Tabs" role="tabpanel" >

                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                        <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-filter"></span>&nbsp;&nbsp;Seleção dos Dados</a></li>
                        <li role="presentation"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Consulta</a></li>
                        <li role="presentation"><a href="#consulta2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-remove-circle"></span>&nbsp;&nbsp;Rejeições</a></li>
                    </ul>
                    <!-- Tab panes -->
                    <div class="tab-content" runat="server" id="PanelContext">
                        
                        
                        <div role="tabpanel" class="tab-pane" id="home" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;"  >
                        
                            
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
                                            <asp:Label ID="lblFiltro2" CssClass="form-control" Text="Filtro 2" name="lblFiltro2" runat="server" Font-Size="Small" Style="white-space: nowrap;" />
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
                                     
                                </div>
                            
                                        
                            <br />
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
                                            <asp:DropDownList ID="ddlSituacao" CssClass="form-control" runat="server" Font-Size="Medium" >
                                                         
                                                <asp:ListItem Value="0" Text="Todos" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Digitação" ></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Autorizado"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Cancelado"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Rejeitado"></asp:ListItem>
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
                        
                        </div>
                        <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;"   >

                            <br/>
                            <asp:LinkButton ID="btnVoltarSelecao" runat="server" Text="Nova Consulta" CssClass="btn btn-default" UseSubmitBehavior="false" OnClick="btnVoltarSelecao_Click"> 
                                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>Nova Consulta
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-info" OnClick="btnConsultar_Click" Height="33"> 
                                <span aria-hidden="true" title="Consultar" class="glyphicon glyphicon-refresh"></span>
                            </asp:LinkButton>
                            <br/>
                            <br/>
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:GridView ID="grdGrid" runat="server" CssClass="table table-bordered table-hover table-striped"
                                        GridLines="Horizontal" AutoGenerateColumns="False"
                                        Font-Size="8pt"  
                                        OnRowCommand="grdGrid_SelectedIndexChanged"
                                        PageSize="50" AllowPaging="true"
                                        PagerSettings-Mode ="NumericFirstLast"   
                                        OnPageIndexChanging="grdGrid_PageIndexChanging" OnRowCreated="grid_RowCreated"  >
                                        <PagerStyle HorizontalAlign = "Center" CssClass = "GridPager" />
                                
                                        <Columns>
                                            <asp:BoundField DataField="CodigoNotaFiscalServico" HeaderText="Código" ItemStyle-CssClass="centerVertical" />
                                            <asp:BoundField DataField="DataEmissao" HeaderText="Data Emissão" DataFormatString="{0: dd/MM/yyyy HH:mm:ss}" ItemStyle-CssClass="centerVertical"/>
                                            <asp:BoundField DataField="NumeroDocumento" HeaderText="Nro Documento" ItemStyle-CssClass="centerVertical"/>
                                            <asp:BoundField DataField="DGSerieDocumento" HeaderText="Série" ItemStyle-CssClass="noprint centerVertical" HeaderStyle-CssClass="noprint" />
                                            <asp:BoundField DataField="Cpl_NomeTomador" HeaderText="Tomador" ItemStyle-CssClass="centerVertical"/>            
                                            <asp:BoundField DataField="ValorTotalNota" HeaderText="Vl. Total Nota" DataFormatString="{0:C}" ItemStyle-CssClass="centerVertical"/>
                                            <asp:TemplateField HeaderText="Situação" ItemStyle-CssClass="centerVertical ">
                                                <ItemTemplate>
                                                    <label  class="badge <%# Eval("Cpl_DsSituacao").ToString() %> " id="situacaoSpan" ><%# Eval("Cpl_DsSituacao").ToString() %></label>
                                            
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ações" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText">
                                            
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnAutorizar" runat="server" CssClass="btn btn-link buttonGrid" CommandName="AutorizarNFSe" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Visible='<%# Eval("BtnAutorizar") %>' title="Autorizar NFS-e" >
                                                        <img runat="server" src="../../Images/autorizar.png" width="20" height="20"/>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnRejeitar" runat="server"  CssClass="btn btn-link buttonGrid" CommandName="CancelarNFSe" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Visible='<%# Eval("BtnCancelar") %>' title="Cancelar NFS-e">
                                                        <img runat="server" src="../../Images/excluir.png" width="20" height="20"/>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Imprimir" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText" ControlStyle-Width="20">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnImprimir" runat="server" CssClass="btn btn-link buttonGrid" CommandName="ImprimirNFSe" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Visible='<%# Eval("BtnImprimir") %>' title="Imprimir NFS-e">
                                                        <img runat="server" src="../../Images/imprimir.png" width="20" height="20"/>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acesso" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText" >
                                                <ItemTemplate>

                                                    <div class="dropdown">  
                                                        <asp:LinkButton runat="server" type="button" class="btn btn-link dropdown-toggle buttonGrid" data-toggle="dropdown" >
                                                            <img runat="server" src="../../Images/configuracao.png" width="20" height="20"/><span class="caret" style="color:black;margin-left:5px"></span>
                                                        </asp:LinkButton>
                                                
                                                        <ul class="dropdown-menu dropdown-menu-right">
                                                            <li><a><asp:linkButton runat="server" ID="opEditar" CssClass="listButton"
                                                                    Text="Editar"
                                                                    CommandName="Editar"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ><span class="glyphicon glyphicon-edit" style="margin-right:10px!important"></span>Editar</asp:linkButton></a></li>
                                                            <li><a><asp:linkButton runat="server" ID="opDuplicar" CssClass="listButton"
                                                                    Text="Duplicar" AutoPostBack="true"
                                                                    CommandName="Duplicar"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ><span class="glyphicon glyphicon-duplicate" style="margin-right:10px!important"></span>Duplicar</asp:linkButton></a></li>
                                                    
                                                            <li><a><asp:linkButton runat="server" ID="opConsultar" CssClass="listButton"
                                                                    Text="Consultar NFS-e" AutoPostBack="true"
                                                                    CommandName="ConsultarNFSe"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ><span class="glyphicon glyphicon-log-in" style="margin-right:10px!important"></span>Consultar NFS-e </asp:linkButton></a></li>
                                                   
                                                            <li><a><asp:linkButton runat="server" ID="opAutorizar" CssClass="listButton" Visible='<%# Eval("BtnAutorizar") %>'
                                                                    Text="Autorizar NFS-e"   AutoPostBack="true"
                                                                    CommandName="AutorizarNFSe"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    ><span class="glyphicon glyphicon-ok-circle" style="margin-right:10px!important"></span> Autorizar NFS-e</asp:linkButton></a></li>
                                                    
                                                            <li><a><asp:linkButton runat="server" ID="opCancelar" CssClass="listButton" Visible='<%# Eval("BtnCancelar") %>'
                                                                    Text="Cancelar NFS-e" AutoPostBack="true"
                                                                    CommandName="CancelarNFSe"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ><span class="glyphicon glyphicon-remove-circle" style="margin-right:10px!important"></span>Cancelar NFS-e</asp:linkButton></a></li>
                                                            <li class="divider"></li>
                                                            <li><a><asp:linkButton runat="server" ID="opImprimirNFSe" CssClass="listButton" Visible='<%# Eval("BtnImprimir") %>'
                                                                    Text="Imprimir NFS-e" AutoPostBack="true"
                                                                    CommandName="ImprimirNFSe"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ><span class="glyphicon glyphicon-print" style="margin-right:10px!important" ></span>Imprimir NFS-e </asp:linkButton></a></li>
                                                    
                                                            <li><a><asp:linkButton runat="server" ID="opDownNFSe" CssClass="listButton" 
                                                                    Text="Download NFS-e" AutoPostBack="true"
                                                                    CommandName="DownloadNFSe"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ><span class="glyphicon glyphicon-save" style="margin-right:10px!important"></span>Download NFS-e</asp:linkButton></a></li>
                                                    
                                                            <li><a> <asp:linkButton runat="server" ID="opDownXML" CssClass="listButton"
                                                                    Text="Download XML" AutoPostBack="true"
                                                                    CommandName="DownloadXML" 
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><span class="glyphicon glyphicon-save" style="margin-right:10px!important"></span>Download XML</asp:linkButton></a></li>
                                                    
                                                            <li><a><asp:linkButton runat="server" ID="opEnviar" CssClass="listButton"
                                                                    Text="Enviar Por Email" AutoPostBack="true"
                                                                    CommandName="EnviarPorEmail"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><span class="glyphicon glyphicon-envelope" style="margin-right:10px!important"></span>Enviar Por Email</asp:linkButton></a></li>

                                                        </ul>
                                                    </div>

                                           
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                   
                                        </Columns>
                                        <RowStyle CssClass="cursor-pointer" />
                                    </asp:GridView>  
                                </ContentTemplate> 
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grdGrid" />                                                    
                                </Triggers>
                            </asp:UpdatePanel>
                                  
                            &emsp;
                        </div> 
                        <div role="tabpanel" class="tab-pane" id="consulta2" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;"  >
                            <asp:GridView ID="grdRejeicoes" runat="server"
                                CssClass="table table-bordered table-hover table-striped"
                                GridLines="None" AutoGenerateColumns="False"
                                Font-Size="8pt" BackColor="#99FFCC" Visible="true"
                                OnSelectedIndexChanged="grdRejeicoes_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="Codigo" HeaderText="Seq."  />
                                    <asp:BoundField DataField="CodigoDocumento" HeaderText="Doc."/>
                                    <asp:BoundField DataField="Mensagem" HeaderText="Mensagem" />
                                    <asp:BoundField DataField="CodigoAcao" HeaderText="Ação" />
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>              
            </div>
        </div>
    </div>
    <img id="preloader2" src="../../Images/aguarde.gif" style="align-content:center;" class="load" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="MyPopup" class="modal fade" role="dialog">
                 <div class="modal-dialog" role="document" style="height: 100%; width: 400px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H4"></h4>
                        </div>
                        <div class="modal-body">

                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnSimAutorizar" runat="server" Text="Sim" CssClass="btn btn-success" TabIndex="-1" AutoPostBack="true" OnClick="btnSimAutorizar_Click"></asp:LinkButton>
                            <button type="button" class="btn btn-danger" data-dismiss="modal" id="btnNao3">Não</button>
                        </div>
                    </div>
                </div>
            </div>

             <div id="MyPopup2" class="modal fade" role="dialog">
                 <div class="modal-dialog" role="document" style="height: 100%; width: 400px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H5"></h4>
                        </div>
                        <div class="modal-body">

                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnSimCancelar" runat="server" Text="Sim" CssClass="btn btn-success" TabIndex="-1" AutoPostBack="true" OnClick="btnSimCancelar_Click"></asp:LinkButton>
                            <button type="button" class="btn btn-danger" data-dismiss="modal" id="btnNao2">Não</button>

                        </div>
                    </div>
                </div>
            </div>

            <div id="MyPopup3" class="modal fade" role="dialog">
                 <div class="modal-dialog" role="document" style="height: 100%; width: 400px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H6"></h4>
                        </div>
                        <div class="modal-body">

                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="BtnSimConsultar" runat="server" Text="Sim" CssClass="btn btn-success" TabIndex="-1" AutoPostBack="true" OnClick="BtnSimConsultar_Click"></asp:LinkButton>
                            <button type="button" class="btn btn-danger" data-dismiss="modal" id="btnNao">Não</button>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSimConsultar" EventName="Click"/>
            <asp:AsyncPostBackTrigger ControlID="btnSimCancelar" EventName="Click"/>
            <asp:AsyncPostBackTrigger ControlID="btnSimAutorizar" EventName="Click"/>

        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
<%--  --%>