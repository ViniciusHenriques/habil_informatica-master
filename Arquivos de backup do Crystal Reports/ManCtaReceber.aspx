<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManCtaReceber.aspx.cs" Inherits="SoftHabilInformatica.Pages.Financeiros.ManCtaReceber"  
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
    </script>

    <script>

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
        }
    </script>
    <style type="text/css">

        @media screen and (max-width: 600px) {
            .acao{
                margin-top:10px!important;
                
            }  
            .noprint{display:none;}    
            #consulta3,#consulta1,#consulta2{
                padding-left:0px!important;
            }
            
        }

    </style>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $('.js-example-basic-single').select2({
               
            });
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
        });
    </script>
    
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Manutenção do Contas a Receber
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body">
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none;">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                            
                                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click" ToolTip="Ao voltar, TODAS alterações feitas no documento será perdida, inclusive novas baixas e anexos!"> 
                                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnCancelar" runat="server" Text="Inativar" CssClass="btn btn-warning" TabIndex="2" data-toggle="modal" data-target="#myexcpes2"> 
                                        <span aria-hidden="true" title="Cancelar" class="glyphicon glyphicon-remove"></span>  Cancelar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Inativar" CssClass="btn btn-danger" TabIndex="3" data-toggle="modal" data-target="#myexcpes"> 
                                        <span aria-hidden="true" title="Inativar" class="glyphicon glyphicon-remove"></span>  Excluir
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnReativar" runat="server" Text="Reativar Lançamento" CssClass="btn btn-warning" TabIndex="4" data-toggle="false" OnClick="btnReativar_Click"> 
                                        <span aria-hidden="true" title="Cancelar" class="glyphicon glyphicon-remove"></span>  Reativar Lançamento
                                    </asp:LinkButton>
                                </div>

                                <div class="col-md-4 " style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                    <label for="usr" style ="margin-top:-10px;" class="acao">Ação</label>
                                    <asp:DropDownList ID="ddlAcao" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single" TabIndex="5" Font-Size="x-Small">
                                        <asp:ListItem Text="Salvar Registro" Selected="True" Value="0" />
                                        <asp:ListItem Text="Salvar Registro, Continuar Editando" Value="1" />
                                        <asp:ListItem Text="Salvar Registro, Continuar Incluindo" Value="2" />
                                    </asp:DropDownList>
                                </div>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnSalvar" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnCancelar" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnReativar" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnExcluir" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlAcao" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                 </div>

                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none;margin-top:20px">
    
  
                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                            <div class="panel panel-default">
                                <div class="panel-heading">Geral</div>
                                <div class="panel-body">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                        <ContentTemplate>

                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Lançamento</label>
                                                <asp:TextBox ID="txtCodDocumento" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" />
                                            </div>

                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Dt Lançamento</label>
                                                <asp:TextBox ID="txtdtentrada" name="txtdtentrada" AutoPostBack="False" CssClass="form-control" Enabled="false" TabIndex="6" runat="server" />
                                            </div>

                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Empresa</label>
                                                <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="False"  TabIndex="7" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                            </div>

                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Classificação</label>
                                                <asp:DropDownList ID="ddlClassificacao" runat="server" AutoPostBack="False"  TabIndex="8" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                            </div>

                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Situação</label>
                                                <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="False" TabIndex="9" CssClass="form-control js-example-basic-single" Font-Size="Small" enabled="false"/>
                                            </div>

                                            <br /><br /><br /><br />

                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Documento</label>
                                                 <asp:TextBox ID="txtNroDocumento" name="txtNroDocumento" AutoPostBack="False" CssClass="form-control" MaxLength="30" TabIndex="10" runat="server" />
                                            </div>

                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                 <asp:TextBox ID="txtdtemissao" name="txtdtemissao" AutoPostBack="False" CssClass="form-control" runat="server" TabIndex="11" />
                                            </div>

                                            <div class="col-sm-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Dt Vencimento</label>
                                                <asp:TextBox ID="txtdtvencimento" name="txtdtvencimento" AutoPostBack="False" CssClass="form-control" runat="server" TabIndex="12" />
                                            </div>

                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Cobrança</label>
                                                <asp:DropDownList ID="ddlTipoCobranca" runat="server" AutoPostBack="False"  TabIndex="13" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                                            </div>

                                            <br /><br /><br /><br />
                                            <div class="col-md-6" >
                                                <label for="usr" style ="margin-top:1px; ">Código Devedor</label>
                                                <div class="input-group " style="width:100%">
                                                                               
                                                    <asp:TextBox ID="txtCodCredor" CssClass="form-control" name="txtCodCredor"  runat="server" TabIndex="13" OnTextChanged="SelectedCredor"   AutoPostBack="true"                                
                                                    Width="15%" MaxLength="6" />

                                                    <asp:LinkButton ID="LinkButton2" title="Pesquise os Forncedores" runat="server" CssClass="form-control btn btn-primary" Width="10%"  OnClick="ConCredor" > 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtCredor" CssClass="form-control" name="txtCodPessoa"  runat="server" TabIndex="14"  Enabled="false"                                  
                                                    Width="75%"  />

                                                </div>
                                            </div>
                                   
                                            <div class="col-md-6">
                                                <label for="usr" style ="margin-top:1px">Plano de Contas</label>
                                                <asp:DropDownList ID="ddlPlanoConta" runat="server" AutoPostBack="False"  TabIndex="15" CssClass="form-control js-example-basic-single" Font-Size="Small" />
                                            </div>

                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Observação</label>
                                                <asp:TextBox ID="txtOBS" name="txtOBS" AutoPostBack="False" CssClass="form-control" TabIndex="16" MaxLength="300" runat="server" />
                                            </div>
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; margin-top:20px    ">
                                               
                                                    <div class="panel panel-primary">
                                                        <div class="panel-heading">Documento Original</div>
                                                        <div class="panel-body">
                                                            <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Código Documento </label>
                                                                <asp:TextBox ID="txtCodOriginal" name="txtCodOriginal" AutoPostBack="False" CssClass="form-control" TabIndex="16" runat="server" Enabled="false"/>
                                                            </div>
                                                            <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Tipo Documento</label>
                                                                <asp:DropDownList ID="ddlTipoDocumento" runat="server" AutoPostBack="False"  TabIndex="8" CssClass="form-control js-example-basic-single" Font-Size="Small"  enabled="false"/>
                                                            </div>
                                                            <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Número</label>
                                                                <asp:TextBox ID="txtNroOriginal" name="txtNroOriginal" AutoPostBack="False" CssClass="form-control" TabIndex="16" runat="server" enabled="false" />
                                                            </div>
                                                            <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                <label for="usr" style ="margin-top:1px;">Número de Série</label>
                                                                <asp:TextBox ID="txtSerieOriginal" name="txtSerieOriginal" AutoPostBack="False" CssClass="form-control" TabIndex="16" runat="server"  enabled="false"/>
                                                            </div>
                                                    
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate> 
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtCodCredor" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:HiddenField ID="TabName" runat="server"/>
            
                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;" >

                            <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                                <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-usd"></span>&nbsp;&nbsp;Valores</a></li>
                                <li role="presentation"><a href="#consulta" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-user"></span>&nbsp;&nbsp;Dados Credor</a></li>
                                <li role="presentation"><a href="#consulta1" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-menu-down"></span>&nbsp;&nbsp;Baixas Contas a Pagar</a></li>
                                <li role="presentation" ><a href="#consulta2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Eventos&nbsp;&nbsp;</a></li>
                                <li role="presentation" ><a href="#consulta4" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-check"></span>&nbsp;&nbsp;Logs &nbsp;&nbsp;</a></li>
                                <li role="presentation" ><a href="#consulta3" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexos &nbsp;&nbsp;</a></li>
                            </ul>

                            <div class="tab-content" runat="server" id="PanelContext">

                                <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Valor do Documento</label>
                                                <asp:TextBox ID="txtVlrDocumento" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="17" OnTextChanged="txtVlrDocumento_TextChanged" AutoPostBack="true"  />
                                            </div>
                                
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Valor do Acréscimo</label>
                                                <asp:TextBox ID="txtVlrAcres" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="18" OnTextChanged="txtVlrAcres_TextChanged" AutoPostBack="true" />
                                            </div>
                                    
                
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Valor do Desconto</label>
                                                <asp:TextBox ID="txtVlrDesc" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="19" OnTextChanged="txtVlrDesc_TextChanged" AutoPostBack="true" />
                                            </div>
                                    
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Valor Total</label>
                                                <asp:TextBox ID="txtVlrTotal" CssClass="form-control" runat="server" Enabled="false" Text ="0,00" TabIndex="20" />
                                            </div>
                                    
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Saldo Pago</label>
                                                <asp:TextBox ID="txtVlrPago" CssClass="form-control" runat="server" Enabled="false"  TabIndex="21" />
                                            </div>
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Saldo á pagar</label>
                                                <asp:TextBox ID="txtVlrPagar" CssClass="form-control" runat="server" Enabled="false"  TabIndex="21" />
                                            </div>

                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtVlrDocumento" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtVlrAcres" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtVlrDesc" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>

                                <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr" style ="margin-top:1px;">CNPJ/CPF</label>
                                        <asp:TextBox ID="txtCNPJCPFCredor" CssClass="form-control" runat="server" Enabled="false" Text =""  />
                                    </div>
                                
                                    <div class="col-md-10" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr" style ="margin-top:1px;">Razão Social</label>
                                        <asp:TextBox ID="txtRazSocial" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                    </div>
                                    
                                    <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
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
                                </div>

                                <div role="tabpanel" class="tab-pane" id="consulta1" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">                                                
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btnNovBaixa" runat="server" style="padding-left:20px;" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovBaixa_Click" TabIndex="21"> 
                                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Nova Baixa&nbsp;
                                            </asp:LinkButton>
                                            <br /><br />
                                            <asp:GridView ID="grdBxCtaPagar" runat="server"
                                                CssClass="table  table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  Visible="true"
                                                OnSelectedIndexChanged="grdBxCtaPagar_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoBaixa" HeaderText="#"  />
                                                    <asp:BoundField DataField="Cpl_Cobranca" HeaderText="Tipo Cobrança " DataFormatString="{0:C}" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                                    <asp:BoundField DataField="DataBaixa" HeaderText="Dt. Baixa" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="ValorBaixa" HeaderText="Vl. Baixa" DataFormatString="{0:C}"/>
                                                    <asp:BoundField DataField="ValorDesconto" HeaderText="Vl. Desc." DataFormatString="{0:C}" />
                                                    <asp:BoundField DataField="ValorAcrescimo" HeaderText="Vl. Acrésc." DataFormatString="{0:C}"/>
                                                    <asp:BoundField DataField="ValorTotalBaixa" HeaderText="Vl. Total Baixa " DataFormatString="{0:C}" />
                                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                            </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="grdBxCtaPagar" />

                                        </Triggers>
                                    </asp:UpdatePanel>  
                                </div>

                                <div role="tabpanel" class="tab-pane" id="consulta2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                                
                                            <asp:GridView ID="GrdEventoDocumento" runat="server" AutoPostBack="false"
                                                CssClass="table  table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  Visible="true">
                                                     
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
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta4" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                                
                                            <asp:GridView ID="grdLogDocumento" runat="server" CssClass="table  table-striped"
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
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="grdLogDocumento" />

                                        </Triggers>
                                    </asp:UpdatePanel>      
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta3" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">                                           
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                                <asp:LinkButton ID="btnNovoAnexo" runat="server" style="padding-left:20px;" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovAnexo_Click" TabIndex="21"> 
                                                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo Anexo&nbsp;
                                                </asp:LinkButton>
                                                <br /><br />
                                            <asp:GridView ID="grdAnexo" runat="server"
                                                CssClass="table table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  Visible="true"
                                                OnSelectedIndexChanged="grdAnexo_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoAnexo" HeaderText="#"  />
                                                    <asp:BoundField DataField="DataHoraLancamento" HeaderText="Data/Hora Lançamento"  />
                                                    <asp:BoundField DataField="Cpl_Maquina" HeaderText="Estação" />
                                                    <asp:BoundField DataField="Cpl_Usuario" HeaderText="Usuário"  />
                                                    <asp:BoundField DataField="NomeArquivo" HeaderText="Descrição"/>
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
                                            <asp:AsyncPostBackTrigger ControlID="grdAnexo" />

                                        </Triggers>
                                    </asp:UpdatePanel> 
                                    
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <!-- Exclui Grupo -->
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
                            Deseja Excluir esta Conta a Pagar ?
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:LinkButton>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCfmSim" />
        </Triggers>
    </asp:UpdatePanel> 

    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="myexcpes2" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H2">Atenção</h4>
                        </div>
                        <div class="modal-body">
                            Deseja Cancelar esta Conta a Pagar ?
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btn_Cancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCancelar_Click"></asp:LinkButton>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_Cancelar" />
        </Triggers>
    </asp:UpdatePanel> 

</asp:Content>
