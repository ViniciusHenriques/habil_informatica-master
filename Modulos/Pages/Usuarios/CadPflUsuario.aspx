<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadPflUsuario.aspx.cs" Inherits="SoftHabilInformatica.Pages.Usuarios.CadPflUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link rel="stylesheet" href="../../Content/EstiloDefinidoPorParametro.css">
    <script src="../../Scripts//moment.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/moment-with-locales.js"></script>
    <script src="../../Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="../../Content/bootstrap-datetimepicker.min.css">
    <script type="text/javascript">
        function openModal() {
            $('#myModalMsg').modal('show');
        }
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.datetimepicker3').datetimepicker({ locale: 'pt-br', format: 'HH:mm' });
            $('.datetimepicker1').datetimepicker({ locale: 'pt-br', format: 'DD/MM/YYYY HH:mm' });

            item = '#<%=PanelSelect%>';
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

        
            item2 = '#home2';
                $('#myTabs2 a[href="' + item2 + '"]').tab('show');
                console.log($(item2));

        }
        $(document).ready(function (e) {
            item = '#<%=PanelSelect%>';
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

        });
        
        $(document).ready(function (e) {
            item2 = '#home2';
            $('#myTabs2 a[href="' + item2 + '"]').tab('show');
            console.log($(item2));

        });

        $(document).ready(function (e) {
            $('.datetimepicker3').datetimepicker({ locale: 'pt-br', format: 'HH:mm' });
            $('.datetimepicker1').datetimepicker({ locale: 'pt-br', format: 'DD/MM/YYYY HH:mm' });
        });
    </script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="panel panel-primary">
                    <div class="panel-heading">Cadastro de Perfis de Usuários</div>
                    <div class="panel-body">
                        <asp:Panel ID="pnlExcluir" runat="server" Visible="false">
                            <div class="modal-dialog" role="document" style="height: 30%; width: 60%">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title" id="H2">Atenção</h4>
                                    </div>
                                    <div class="modal-body">Deseja Excluir Perfil do Usuário ?</div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnCfmSim" runat="server" Text="Sim" CssClass="btn btn-danger" TabIndex="-1" UseSubmitBehavior="false" OnClick="btnCfmSim_Click"></asp:Button>
                                        <asp:Button ID="btnCfmNao" runat="server" Text="Não" CssClass="btn btn-default" TabIndex="-1" UseSubmitBehavior="false" OnClick="btnCfmNao_Click"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" UseSubmitBehavior="false" OnClick="btnExcluir_Click"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnMensagem" runat="server" Text="Mensagem" Visible="false" class="btn btn-default" data-toggle="modal" data-target="#myModalMsg" OnClick="btnMensagem_Click"> 
                        </asp:LinkButton>

                        <asp:Panel ID="pnlMensagem" runat="server" Visible="false">
                            <!-- Modal -->
                            <div class="modal fade" id="myModalMsg" role="dialog">
                                <div class="modal-dialog modal-sm" style="width: 30%; position: fixed; top: 30%; left: 35%;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                            <h5 class="modal-title" id="H3">Atenção</h5>
                                        </div>
                                        <div class="modal-body">
                                            <p>
                                                <asp:Label ID="lblMensagem" runat="server" Font-Size="Small"></asp:Label>
                                            </p>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:LinkButton ID="Button1" runat="server" Text="Fechar" CssClass="btn btn-default" data-dismiss="modal" TabIndex="-1" UseSubmitBehavior="false" OnClick="btnCfmNao_Click"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <br />
                        <br />
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-addon">Código do Perfil de Usuário : &nbsp;&nbsp;&nbsp; </span>
                                <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" runat="server" TabIndex="1"
                                    onkeypress="return PermiteSomenteNumeros(event);"
                                    MaxLength="5" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-addon">Descrição do Perfil de Usuário : </span>
                                <asp:TextBox ID="txtDescricao" CssClass="form-control" name="txtDescricao" TabIndex="2" runat="server" MaxLength="50" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="input-group">
                                <span class="input-group-addon">Módulo específico: </span>
                                <asp:DropDownList ID="ddlModuloEspecifico" CssClass="form-control" TabIndex="3" runat="server" />
                            </div>
                        </div>

                        <br />
                        <div id="Tabs" role="tabpanel">

                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs" role="tablist" id="myTabs">
                                <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-briefcase"></span>&nbsp;&nbsp;Empresas</a></li>
                                <li role="presentation" title="Configuração do Acesso ao sistema"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-log-in"></span>&nbsp;&nbsp;Acesso</a></li>
                            </ul>
                            <!-- Tab panes -->
                            <div class="tab-content" runat="server" id="PanelContext">
                                <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:GridView ID="grdPermissao" runat="server"
                                        Width="100%" AutoPostBack="true"
                                        CssClass="table table-bordered table-hover table-striped"
                                        GridLines="None" AutoGenerateColumns="False"
                                        Font-Size="8pt">

                                        <Columns>
                                            <asp:TemplateField HeaderText="Liberado">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkLiberado" runat="server" Checked='<%# Eval("Liberado").ToString().Equals("True") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CodigoEmpresa" HeaderText="Empresa" />
                                            <asp:BoundField DataField="NomeEmpresa" HeaderText="Nome da Empresa" />
                                        </Columns>
                                        <RowStyle CssClass="cursor-pointer" />
                                    </asp:GridView>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;margin-bottom:400px!important">
                                    <div id="Tabs2" role="tabpanel">

                                        <!-- Nav tabs -->
                                        <ul class="nav nav-tabs" role="tablist" id="myTabs2">
                                            <li role="presentation" title="Configuração da jornada de trabalho normal do funcionário"><a href="#home2" aria-controls="home2" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Semanal</a></li>
                                            <li role="presentation" title="Eventuais acessos ao Sistema fora do expediente "><a href="#consulta2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-time"></span>&nbsp;&nbsp;Diário</a></li>
                                        </ul>
                                        <!-- Tab panes -->
                                        <div class="tab-content" runat="server" id="Div1">
                                            <div role="tabpanel" class="tab-pane" id="home2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                                <div class="col-md-6">
                                                    <div class="col-md-12" style="margin-bottom:10px!important;">
                                                        <div class="panel panel-primary checkbox-dia-semana">
                                                            <div class="panel-heading" style="padding: 5px!important">Dom</div>
                                                            <div class="panel-body checkbox">
                                                                <label style="padding: 0!important; margin: 0!important">
                                                                    <asp:CheckBox runat="server" ID="ChkDomingo" />
                                                                    <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-primary checkbox-dia-semana">
                                                            <div class="panel-heading" style="padding: 5px!important">Seg</div>
                                                            <div class="panel-body checkbox">
                                                                <label style="padding: 0!important; margin: 0!important">
                                                                    <asp:CheckBox runat="server" ID="ChkSegunda" />
                                                                    <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-primary checkbox-dia-semana ">
                                                            <div class="panel-heading" style="padding: 5px!important">Ter</div>
                                                            <div class="panel-body checkbox">
                                                                <label style="padding: 0!important; margin: 0!important">
                                                                    <asp:CheckBox runat="server" ID="ChkTerca" />
                                                                    <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-primary checkbox-dia-semana">
                                                            <div class="panel-heading" style="padding: 5px!important">Qua</div>
                                                            <div class="panel-body checkbox">
                                                                <label style="padding: 0!important; margin: 0!important">
                                                                    <asp:CheckBox runat="server" ID="ChkQuarta" />
                                                                    <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-primary checkbox-dia-semana">
                                                            <div class="panel-heading" style="padding: 5px!important">Qui</div>
                                                            <div class="panel-body checkbox">
                                                                <label style="padding: 0!important; margin: 0!important">
                                                                    <asp:CheckBox runat="server" ID="ChkQuinta" />
                                                                    <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-primary checkbox-dia-semana">
                                                            <div class="panel-heading" style="padding: 5px!important">Sex</div>
                                                            <div class="panel-body checkbox">
                                                                <label style="padding: 0!important; margin: 0!important">
                                                                    <asp:CheckBox runat="server" ID="ChkSexta" />
                                                                    <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-primary checkbox-dia-semana">
                                                            <div class="panel-heading" style="padding: 5px!important">Sab</div>
                                                            <div class="panel-body checkbox">
                                                                <label style="padding: 0!important; margin: 0!important">
                                                                    <asp:CheckBox runat="server" ID="ChkSabado" />
                                                                    <span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>
                                                                </label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="margin-bottom: 15px; margin-top: 10px">
                                                        <div class="form-group">
                                                            <div class='input-group date '>
                                                                <span class="input-group-addon">Horário Inicial: &nbsp;&nbsp; </span>
                                                                <asp:TextBox ID="txtHorarioInicial" CssClass="form-control datetimepicker3" TabIndex="2" runat="server" MaxLength="6" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-time"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="margin-bottom: 15px; margin-top: 10px">
                                                        <div class="form-group">
                                                            <div class='input-group date '>
                                                                <span class="input-group-addon">Horário Final: &nbsp;&nbsp;&nbsp; </span>
                                                                <asp:TextBox ID="txtHorarioFinal" CssClass="form-control datetimepicker3" TabIndex="2" runat="server" MaxLength="6" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-time"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="col-md-12">
                                                        <div class="panel panel-default" title="Configuração do intervalo, impedindo acesso ao Sistema">
                                                            <div class="panel-heading"><span class="glyphicon glyphicon-eye-close"></span>&nbsp;&nbsp;Intervalo(s)</div>
                                                            <div class="panel-body ">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <div class='input-group date '>
                                                                            <span class="input-group-addon">Início: &nbsp;&nbsp;&nbsp; </span>
                                                                            <asp:TextBox ID="txtIntervaloInicio" CssClass="form-control datetimepicker3" TabIndex="2" runat="server" MaxLength="6" />
                                                                            <span class="input-group-addon">
                                                                                <span class="glyphicon glyphicon-time"></span>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <div class='input-group date '>
                                                                            <span class="input-group-addon">Fim: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                                                                            <asp:TextBox ID="txtIntervaloFim" CssClass="form-control datetimepicker3" TabIndex="2" runat="server" MaxLength="6" />
                                                                            <span class="input-group-addon">
                                                                                <span class="glyphicon glyphicon-time"></span>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <div class='input-group date '>
                                                                            <span class="input-group-addon">Início: &nbsp;&nbsp;&nbsp; </span>
                                                                            <asp:TextBox ID="txtIntervaloInicio2" CssClass="form-control datetimepicker3" TabIndex="2" runat="server" MaxLength="6" />
                                                                            <span class="input-group-addon">
                                                                                <span class="glyphicon glyphicon-time"></span>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <div class='input-group date '>
                                                                            <span class="input-group-addon">Fim: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                                                                            <asp:TextBox ID="txtIntervaloFim2" CssClass="form-control datetimepicker3" TabIndex="2" runat="server" MaxLength="6" />
                                                                            <span class="input-group-addon">
                                                                                <span class="glyphicon glyphicon-time"></span>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <div class='input-group date '>
                                                                            <span class="input-group-addon">Início: &nbsp;&nbsp;&nbsp; </span>
                                                                            <asp:TextBox ID="txtIntervaloInicio3" CssClass="form-control datetimepicker3" TabIndex="2" runat="server" MaxLength="6" />
                                                                            <span class="input-group-addon">
                                                                                <span class="glyphicon glyphicon-time"></span>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <div class='input-group date '>
                                                                            <span class="input-group-addon">Fim: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                                                                            <asp:TextBox ID="txtIntervaloFim3" CssClass="form-control datetimepicker3" TabIndex="2" runat="server" MaxLength="6" />
                                                                            <span class="input-group-addon">
                                                                                <span class="glyphicon glyphicon-time"></span>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                             
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div> 

                                            </div>
                                            <div role="tabpanel" class="tab-pane" id="consulta2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <div class='input-group date '>
                                                            <span class="input-group-addon">Data/Hora Inicio: &nbsp;&nbsp;&nbsp; </span>
                                                            <asp:TextBox ID="txtDtHrInicio" CssClass="form-control datetimepicker1" TabIndex="2" runat="server" MaxLength="10" />
                                                            <span class="input-group-addon">
                                                                <span class="glyphicon glyphicon-calendar"></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <div class='input-group date '>
                                                            <span class="input-group-addon">Data/Hora Fim: &nbsp;&nbsp;&nbsp; </span>
                                                            <asp:TextBox ID="txtDtHrFim" CssClass="form-control datetimepicker1" TabIndex="2" runat="server" MaxLength="10" />
                                                            <span class="input-group-addon">
                                                                <span class="glyphicon glyphicon-calendar"></span>
                                                            </span>
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
            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnExcluir" />
            </Triggers>

        </asp:UpdatePanel>
    </div>
</asp:Content>
