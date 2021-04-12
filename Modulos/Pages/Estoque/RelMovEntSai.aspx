<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="RelMovEntSai.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.RelMovEntSai" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    
    

    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtDtDe']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
            
        });
         $(document).ready(function () {
            $("input[id*='txtDtAte']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
            
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

            $("input[id*='txtDtAte']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });

            $("input[id*='txtDtDe']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
        }
    </script>


<div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <asp:Label id="lbnPagName" Text="" runat="server" />
            <div class="messagealert" id="alert_container"></div>
        </div>
        <div class="panel-body">
                    <asp:LinkButton ID="btnSair" runat="server" onFocus="this.select()" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                        <span aria-hidden="true" title="Fechar" TabIndex="19" class="glyphicon glyphicon-off"></span>  Fechar
                    </asp:LinkButton>                  

                <br/>
                <br/>

                    <asp:Panel id="pnlCon" runat="server">
                        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" onFocus="this.select()" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="row" style="margin-top: 10px">
                                    <div class="col-md-6">
                                        <label for="usr"> Empresa: </label>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="1" Width="100%"> </asp:DropDownList>		   
                                                </ContentTemplate> 
                                            <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                                </asp:UpdatePanel>
                                    </div> 
                    
                                    <div class="col-md-3">
                                        <label for="usr">Periodo </label>
                                        <label for="usr">De: </label>
                                        <asp:TextBox ID="txtDtDe" CssClass="form-control" Enabled="true" runat="server" TabIndex="2"/>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="usr">Até: </label>    
                                        <asp:TextBox ID="txtDtAte" CssClass="form-control" Enabled="true" runat="server" TabIndex="3"/>
                                    </div>                                   
                                </div>
                                <div class="row" style="margin-top: 10px">  
                                    <div class="col-md-3">
                                        <label for="usr">Documento: </label>
                                        <asp:TextBox ID="txtDocumento" CssClass="form-control" runat="server" TabIndex="4"  />
                                    </div>
                                    <div class="col-md-9" >
                                        <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Produto: </label>
                                            <div class="input-group " style="width:100%">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtProduto" CssClass="form-control"  runat="server" TabIndex="5" OnTextChanged="txtProduto_TextChanged" OnSelectedIndexChanged="txtProduto_SelectedIndexChanged" AutoPostBack="true"                                
                                                            Width="15%" MaxLength="6" />                                                      
                                                            <asp:LinkButton ID="btnPesquisarItem"  runat="server" CssClass="form-control btn btn-primary" Width="8%" TabIndex="6" OnClick="btnPesquisarItem_Click" AutoPostBack="true"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                            </asp:LinkButton>
                                                        <asp:TextBox ID="txtDcrproduto" CssClass="form-control"  runat="server"  Enabled="false"  Width="77%"  />
                                                    </ContentTemplate> 
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtProduto" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDcrproduto" EventName="TextChanged" />
                                                        </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                    </div>              
                                </div> 
                                <div class="row" style="margin-top: 10px">
                                    <div class="col-md-6">
                                        <label for="usr"> Localização: </label>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlLocalizacao" runat="server" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="7"> </asp:DropDownList>		   
                                                </ContentTemplate> 
                                                    <Triggers> 
                                                        <asp:AsyncPostBackTrigger ControlID="ddlLocalizacao" EventName="TextChanged" />
                                                    </Triggers>
                                            </asp:UpdatePanel>
                                    </div> 
                                    <div class="col-md-6">
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <label for="usr"> Lote: </label>
                                                <asp:DropDownList ID="ddlLote" runat="server" AutoPostBack="true" CssClass="form-control js-example-basic-single" Width="100%" Font-Size="Small" TabIndex="8"> </asp:DropDownList>		   
                                            </ContentTemplate> 
                                            <Triggers> 
                                                <asp:AsyncPostBackTrigger ControlID="ddlLote" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div> 
                                </div>
                                <div class="row" style="margin-top: 10px">
                                    
                                </div>
                                <br />
                                <div class="row" style="margin-top: 10px">
  
                                        <div class="col-md-3">
                                        <asp:DropDownList ID="ddlTpOper" CssClass="form-control" runat="server" TabIndex="3" Font-Size="Medium" >
                                            <asp:ListItem Value="1" Text="Relatórios de Entradas" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Relatórios de Saídas"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1" >
                                            <asp:LinkButton ID="btnConsultar" runat="server" onFocus="this.select()" TabIndex="12" CssClass="btn btn-default" UseSubmitBehavior="false"  OnClick="btnConsultar_Click"> 
                                                    <span aria-hidden="true" title="Consultar" class="glyphicon glyphicon-search"></span>Consultar
                                            </asp:LinkButton>
                                        </div>
                                </div>
                            </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnConsultar" />
                                </Triggers>                            
                        </asp:UpdatePanel>
                        </asp:Panel>
                            <asp:Panel id="pnlRel" runat="server" Visible="false">
                                <div class="col-md-1">
                                    <asp:LinkButton ID="btnVoltar" runat="server" Visible="false" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click" TabIndex="11"> 
                                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                    </asp:LinkButton>
                                </div>                
                            <br />
                            <br />  
                            <div class="row" style="margin-top: 20px; padding-left: 30px; padding-right:50px;">
                            <div class="col-md-12">
                                <CR:CrystalReportViewer ID="CRViewer" Visible="false" runat="server" AutoDataBind="True" HasRefreshButton="True"
                                        EnableDatabaseLogonPrompt="False" Height="100%" ToolPanelWidth="200px" Width="100%" />
                            </div>
                        </div>
                    </asp:Panel>

        </div>
    </div>
</div>

   
</asp:Content>
