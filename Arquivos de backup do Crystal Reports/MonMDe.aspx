<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="MonMDe.aspx.cs" Inherits="SoftHabilInformatica.HabilEletronico.MonMDe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <meta http-equiv="refresh" content="180">

    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script>
    item = '#<%=PanelSelect%>';
    $(document).ready(function (e) {
        $('#myTabs a[href="' + item + '"]').tab('show');
        console.log($(item));
    });
        
    </script>

    <style type="text/css">
        
        @media screen and (max-width: 600px) {
            .noprint{display:none;}          
        }
        .input-group{
            margin-bottom:10px!important;
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
    </script>
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <div class="panel panel-primary" >
            <div  class="panel-heading">Monitoramento Documento Eletrônico
                <div class="messagealert" id="alert_container"></div>
            </div>
            
            <div class="panel-body">


                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
                <asp:LinkButton ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" UseSubmitBehavior="false" OnClick="btnEnviar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-save"></span>  Enviar
                </asp:LinkButton>
                <asp:LinkButton ID="btnCancelar" runat="server" Text="Teste" CssClass="btn btn-primary" UseSubmitBehavior="false" OnClick="btnCancelar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-save"></span>  Cancelar 
                </asp:LinkButton>
                <asp:LinkButton ID="btnConsultarNota" runat="server" Text="Teste" CssClass="btn btn-primary" UseSubmitBehavior="false" OnClick="btnConsultarNota_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-save"></span>  Consultar
                </asp:LinkButton>
                <br />
                <br />
            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:HiddenField ID="TabName" runat="server"/>
                            <asp:GridView ID="grdGrid" runat="server" CssClass="table table-bordered table-hover table-striped"
                                GridLines="None" AutoGenerateColumns="False"
                                Font-Size="8pt"  
                                PageSize="50" AllowPaging="true"
                                PagerSettings-Mode ="NumericFirstLast"   >
                                <PagerStyle HorizontalAlign = "Right"   CssClass = "GridPager" />
                                <Columns>
                                    <asp:BoundField DataField="Codigo" HeaderText="Código" />
                                    <asp:BoundField DataField="CodigoDocumento" HeaderText="Código Documento" />
                                    <asp:BoundField DataField="Mensagem" HeaderText="Mensagem" />                                                                     
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>        
                            <asp:Timer ID="tmrFillAlerts" runat="server" OnTick="tmrFillAlerts_Tick" Interval="5000" Enabled="true"/>

                                        &emsp;
                        </ContentTemplate> 
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="tmrFillAlerts" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
            </div>
        </div>
    </div>


</asp:Content>
