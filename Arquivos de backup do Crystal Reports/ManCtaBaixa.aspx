<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" 
    CodeBehind="ManCtaBaixa.aspx.cs" Inherits="SoftHabilInformatica.Pages.Financeiros.ManCtaBaixa"  
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
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
        $(document).ready(function () {
            $('.js-example-basic-single').select2({
                
            });
        });
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

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
                Baixa do Documento
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
                            <span aria-hidden="true" title="Inativar" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>

                            

                    </div>                      

                 </div>

                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none; padding:15px!important;">
                      
                        <asp:HiddenField ID="TabName" runat="server"/>
            
                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;" >

                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                                <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-arrow-down"></span>&nbsp;&nbsp;Baixa</a></li>
                                <li role="presentation"><a href="#consulta" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-file"></span>&nbsp;&nbsp;Informações</a></li>
                            </ul>

                            <!-- Tab panes -->
                            <div class="tab-content" runat="server" id="PanelContext">
                                <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <div class="row">
                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Sequência:</label>
                                            <asp:TextBox ID="txtCodBaixa" CssClass="form-control" runat="server" Enabled="false"  TabIndex="19"  />
                                        </div>
                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Data de vencimento:</label>
                                            <asp:TextBox ID="txtdtVencimentoBaixa" CssClass="form-control" runat="server" TabIndex="18" name="txtdtBaixa" Enabled="false" />
                                        </div>
                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Data da Baixa:</label>
                                            <asp:TextBox ID="txtdtBaixa" CssClass="form-control" runat="server" TabIndex="18" name="txtdtBaixa" />
                                        </div>
                                        <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Tipo da Baixa:</label>
                                            <asp:DropDownList ID="ddlTipoBaixa" runat="server" AutoPostBack="False" TabIndex="19" CssClass="form-control js-example-basic-single" Font-Size="Small" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Tipo de Cobrança:</label>
                                            <asp:DropDownList ID="ddlTpCobranca" runat="server" AutoPostBack="False" TabIndex="19" CssClass="form-control js-example-basic-single" Font-Size="Small" />
                                        </div>
                                     

                                        <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <div class="col-md-11">
                                                <label for="usr" style ="margin-top:1px;">Conta Corrente:</label>
                                                <asp:DropDownList ID="ddlCtaCorrente" runat="server" AutoPostBack="False" TabIndex="19" CssClass="form-control js-example-basic-single" Font-Size="Small" />
                                            </div>
                                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small; margin-top:19px">
                                                
                                                <asp:LinkButton ID="BtnAddCtaCorrente" runat="server" Text="Adicionar" TabIndex="10" CssClass="btn btn-success" OnClick="BtnAddCtaCorrente_Click" style="height:33px!important;padding-top:7px" > 
                                                <span aria-hidden="true" title="Adicionar Conta Corrente" class="glyphicon glyphicon-plus"></span>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                         <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Observação:</label>
                                            <asp:TextBox ID="TxtOb" CssClass="form-control" runat="server" Enabled="true"  TabIndex="20"  />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Valor Pago:</label>
                                                    <asp:TextBox ID="txtVlrBaixa" CssClass="form-control" runat="server" Enabled="true"  TabIndex="17" OnTextChanged="txtVlrDocumento_TextChanged" AutoPostBack="true" />
                                                </div>
                                                <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Valor Acréscimo:</label>
                                                    <asp:TextBox ID="TxtVlrAcrescimo" CssClass="form-control" runat="server" Enabled="true"  TabIndex="17"  OnTextChanged="txtVlrAcres_TextChanged" AutoPostBack="true" />
                                                </div>
                                                <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Valor Desconto:</label>
                                                    <asp:TextBox ID="TxtVlrDesconto" CssClass="form-control" runat="server" Enabled="true"  TabIndex="17"  OnTextChanged="txtVlrDesc_TextChanged" AutoPostBack="true" />
                                                </div>
                                        
                                                <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Valor Total Pago:</label>
                                                    <asp:TextBox ID="TxtVlTotalbaixa" CssClass="form-control" runat="server" Enabled="false"  TabIndex="17"  />
                                                </div>
                                            </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtVlrBaixa" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="TxtVlrAcrescimo" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="TxtVlrDesconto" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                   </div>
                                    
                                    
                                   
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">Informações do Documento</div>
                                            <div class="panel-body">

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
                                                    <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="False"  TabIndex="7" CssClass="form-control " Font-Size="Small" Enabled="false"/>
                                                </div>

                                                <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Classificação</label>
                                                    <asp:DropDownList ID="ddlClassificacao" runat="server" AutoPostBack="False"  TabIndex="8" CssClass="form-control " Font-Size="Small" Enabled="false"/>
                                                </div>

                                                <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Situação</label>
                                                    <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="False" TabIndex="9" CssClass="form-control " Font-Size="Small" Enabled="false"/>
                                                </div>

                                                <br /><br /><br /><br />

                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Documento</label>
                                                     <asp:TextBox ID="txtNroDocumento" name="txtNroDocumento" AutoPostBack="False" CssClass="form-control" TabIndex="10" runat="server" Enabled="false"/>
                                                </div>

                                                <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Dt Emissão</label>
                                                     <asp:TextBox ID="txtdtemissao" name="txtdtemissao" AutoPostBack="False" CssClass="form-control" runat="server" TabIndex="11" Enabled="false" />
                                                </div>

                                                <div class="col-sm-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Dt Vencimento</label>
                                                    <asp:TextBox ID="txtdtvencimento" name="txtdtvencimento" AutoPostBack="False" CssClass="form-control" runat="server" TabIndex="12" Enabled="false"/>
                                                </div>

                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Cobrança</label>
                                                    <asp:DropDownList ID="ddlTipoCobranca" runat="server" AutoPostBack="False"  TabIndex="13" CssClass="form-control " Font-Size="Small" Enabled="false"/>
                                                </div>

                                                <br /><br /><br /><br />

                                                <div class="col-md-6" >
                                                    <label for="usr" style ="margin-top:1px;"> Credor</label>
                                                                                                                                 
                                                    <asp:TextBox ID="txtCodCredor" CssClass="form-control" name="txtCodPessoa"  runat="server" TabIndex="13"  Enabled="false"                                
                                                     MaxLength="6" />                                                     
                                                </div>
                                   
                                                <div class="col-md-6">
                                                    <label for="usr" style ="margin-top:1px;">Plano de Contas</label>
                                                    <asp:DropDownList ID="ddlPlanoConta" runat="server" AutoPostBack="False"  TabIndex="15" CssClass="form-control " Font-Size="Small" Enabled="false"/>
                                                </div>

                                                <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Observações</label>
                                                    <asp:TextBox ID="txtOBS" name="txtOBS" AutoPostBack="False" CssClass="form-control" TabIndex="16" runat="server" Enabled="false"/>
                                                </div>
                                                 <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Valor do Documento</label>
                                                    <asp:TextBox ID="txtVlrDocumento" CssClass="form-control" runat="server" Enabled="false" Text ="0,00" TabIndex="17" AutoPostBack="true"  />
                                                </div>
                                
                                                <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Valor do Acréscimo</label>
                                                    <asp:TextBox ID="txtVlrAcres" CssClass="form-control" runat="server" Enabled="false" Text ="0,00" TabIndex="18"  AutoPostBack="true" />
                                                </div>
                                    
                                                <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Valor do Desconto</label>
                                                    <asp:TextBox ID="txtVlrDesc" CssClass="form-control" runat="server" Enabled="false" Text ="0,00" TabIndex="19" AutoPostBack="true" />
                                                </div>
                                    
                                                <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Valor Total</label>
                                                    <asp:TextBox ID="txtVlrTotal" CssClass="form-control" runat="server" Enabled="false" Text ="0,00" TabIndex="20" />
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
                    Deseja Excluir esta Baixa ?
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="False" OnClick="btnCfmSim_Click"></asp:LinkButton>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
    

</asp:Content>
