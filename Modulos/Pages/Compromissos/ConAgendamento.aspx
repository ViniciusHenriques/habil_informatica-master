 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConAgendamento.aspx.cs" 
     Inherits="SoftHabilInformatica.Pages.Compromissos.ConAgendamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link href='../../Content/Calendar_events_files/main.css' rel='stylesheet' />
    <script src='../../Content/Calendar_events_files/main.js'></script>

    <script>

        document.addEventListener('DOMContentLoaded', function ()
        {
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
                        window.location.assign("ManAgendamento.aspx?date=" + event.toLocaleDateString("pt-br"));
                    },

                    dayMaxEvents: true, // allow "more" link when too many events
                    events: [
                        <%=Agendamentos%>   
                    ],
                });

                calendar.render();
        });

        $(document).on('keydown', function (e) {
            if ((e.altKey) && (e.which === 78)) { 
                $get('<%=btnNovo.ClientID%>').click(); 
            }
        });
    </script>
    <style>
        .TarefaConcluida{
            opacity:0.4
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

    </style>
    <body>
        <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
            <div class="panel panel-primary" >
                <div class="panel-heading">Agendamento de Compromisso
                    <div class="messagealert" id="alert_container"></div>
                </div>            
                <div class="panel-body">
                    <div class="row">   
                        <div class="col-md-12">
                            <div class="col-md-2">
                                <asp:LinkButton ID="btnNovo" runat="server" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnNovo_Click" ToolTip="Novo ( Alt + N )"> 
                                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Editar
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class ="col-md-12" style="margin-top:15px">
                             <div id='top'>
                                <div class='right'>
                                  <span id='loading'></span>
                                </div>
                                <div class='clear'></div>
                            </div>

                            <div id='calendar'></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </body>
</asp:Content>
