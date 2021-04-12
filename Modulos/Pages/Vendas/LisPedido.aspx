<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="LisPedido.aspx.cs"
    Inherits="SoftHabilInformatica.Pages.Vendas.LisPedido" MaintainScrollPositionOnPostback="True" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />

    <%--<link rel="stylesheet" type="text/css" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" >--%>

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });


    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <style type="text/css">
        .buttonGrid {
            padding: 0 !important;
        }

        .GridPager {
            text-align: center !important;
            glyph-orientation-horizontal: inherit;
        }

        @media screen and (max-width: 800px) {
            .noprint {
                display: none;
            }
        }

        .listButton {
            background-color: transparent;
            border: 0 !important;
            padding: 1px !important;
            padding-left: 20px !important;
            padding-right: 20px !important;
        }

        .dropdown-menu {
            padding-top: 1px !important;
        }

        .padding-top-15 {
            padding-top: 14px !important;
        }

        .padding-top-10 {
            padding-top: 10px !important;
        }
    </style>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>



    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Listagem de Pedido               
                <div class="messagealert" id="alert_container"></div>
            </div>
            <br />
            <div class="tab-content" runat="server" id="PanelContext">
                <%----------------BOTÕES INICIAIS -----------------%>
                <div class="row">
                    <div class="col-md-2">
                        <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" style="margin-top: 25px;" OnClick="btnSair_Click">
                                <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-1" style="font-size: medium;">
                    </div>
                    <div class="col-md-6" style="font-size: medium;">
                        <label for="usr" style="margin-top: 1px;">Empresa <span style="color: red;" title="Campo Obrigátorio">*</span></label>
                        <asp:DropDownList ID="ddlEmpresa" runat="server" TabIndex="4" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" AutoPostBack="false" />
                    </div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="btnPesquisarItem" runat="server" CssClass="form-control btn btn-primary glyphicon glyphicon-search" OnClick="btnPesquisarItem_Click" Width="100px" TabIndex="8"  style="margin-top: 25px;" AutoPostBack="true"/> 
                    </div>
                </div>
                <div class="row" >
                    <div class="col-md-12" style="font-size: medium;">
                        <label> </label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4" style="font-size: medium;">
                    </div>
                    <div class="col-md-2" style="font-size: medium;">
                        <label for="usr" style="margin-top: 1px; font-size: small "> Pedido ou Separação </label>
                        <asp:DropDownList ID="ddlPesquisa" runat="server" TabIndex="4" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" AutoPostBack="false" />

                    </div>
                    <div class="col-md-2" style="font-size: medium;">
                        <label for="usr" style="margin-top: 1px; font-size: small"> Nrº Pedido ou Separação: </label>
                        <asp:TextBox ID="txtVarchar" CssClass="form-control" runat="server" TabIndex="5" Text="" AutoPostBack="false" MaxLength="20"  BorderStyle="Solid" BackColor="Transparent" ForeColor="Black" /> 
                    </div>
                </div>
                <br/>
                <br/>
                <%--------------------DOCAS-----------------------------%>
                <asp:Panel id="pnlEscolhasDocas" runat="server" Visible="false">
                    <div class="container-fluid">
                        <asp:Panel ID="Doca1" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca1" CssClass="btn alert-info col-md-12" OnClick="btnDoca1_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca2" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:transparent;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca2" CssClass="btn alert-info col-md-12" OnClick="btnDoca2_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca3" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca3" CssClass="btn alert-info col-md-12" OnClick="btnDoca3_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca4" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca4" CssClass="btn alert-info col-md-12" OnClick="btnDoca4_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca5" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca5" CssClass="btn alert-info col-md-12" OnClick="btnDoca5_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca6" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca6" CssClass="btn alert-info col-md-12" OnClick="btnDoca6_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca7" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca7" CssClass="btn alert-info col-md-12" OnClick="btnDoca7_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca8" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca8" CssClass="btn alert-info col-md-12" OnClick="btnDoca8_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca9" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca9" CssClass="btn alert-info col-md-12" OnClick="btnDoca9_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca10" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca10" CssClass="btn alert-info col-md-12" OnClick="btnDoca10_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca11" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca11" CssClass="btn alert-info col-md-12" OnClick="btnDoca11_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca12" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca12" CssClass="btn alert-info col-md-12" OnClick="btnDoca12_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca13" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca13" CssClass="btn alert-info col-md-12" OnClick="btnDoca13_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca14" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca14" CssClass="btn alert-info col-md-12" OnClick="btnDoca14_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca15" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca15" CssClass="btn alert-info col-md-12"  OnClick="btnDoca15_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca16" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca16" CssClass="btn alert-info col-md-12" OnClick="btnDoca16_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca17" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca17" CssClass="btn alert-info col-md-12" OnClick="btnDoca17_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca18" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca18" CssClass="btn alert-info col-md-12" OnClick="btnDoca18_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca19" runat="server" Visible="false">
                            
                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                                <div class="input-group">
                                     
                                    <asp:LinkButton ID="btnDoca19" CssClass="btn alert-info col-md-12" OnClick="btnDoca19_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                                </div>
                            </div>                                                  
                        </asp:Panel>
                        <asp:Panel ID="Doca20" runat="server" Visible="false">
                            
                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;">
                            <div class="input-group">
                                     
                                <asp:LinkButton ID="btnDoca20" CssClass="btn alert-info col-md-12" OnClick="btnDoca20_Click" Width="275px" Height="35px"  name="btnfiltrodata31" runat="server" MaxLength="100" />
                            </div>
                        </div>                                                  
                    </asp:Panel>
                    </div>
                </asp:Panel>
                <asp:Panel id="pnlBtn" runat="server" Visible="false" >                       
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-1" style="font-size: medium;">
                                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click" Visible="true" > 
                                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                </asp:LinkButton>
                            </div>
                            <div class="col-md-1" style="font-size: medium;">
                                <asp:LinkButton ID="btnImprimir" runat="server" Visible="true" Text="Nova Consulta" CssClass="btn btn-success" OnClick="btnImprimir_Click"> 
                                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-print"></span> Imprimir
                                </asp:LinkButton>
                            </div>
                            <div class="col-md-3" style="font-size: medium;">
                                <div class="input-group " style="width:100%">  
                                    <asp:checkBox ID="chkImprimir" Visible="true" Width="10%" CssClass="form-control" runat="server" BorderColor="Transparent" BackColor="Transparent" Enabled="true" ForeColor="White" Font-Size="Small" value="1" />  
                                    <asp:TextBox ID="txtLbn" CssClass="form-control" Visible="true"  runat="server" TabIndex="5" Text="Exibir Relatório" AutoPostBack="true" Width="90%" MaxLength="4" Enabled="false" BorderStyle="None" BackColor="Transparent" ForeColor="Black" /> 
                                </div>
                            </div>
                            <div class="col-md-4" style="font-size: medium;">
                            </div>
                            <div class="col-md-3" style="font-size: medium;">
                                <asp:DropDownList ID="ddlDoca" runat="server" Visible="true" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="2"> </asp:DropDownList>		   
                            </div>
                        </ContentTemplate> 
                        <Triggers>
                            <asp:PostBackTrigger ControlID ="btnVoltar"/>
                            <asp:PostBackTrigger ControlID ="btnImprimir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel id="pnlGrid" runat="server" Visible="false">  
                    <asp:GridView ID="grdGrid" runat="server" CssClass="table table-bordered table-striped" Width="100%"
                                GridLines="None" AutoGenerateColumns="False" EnableEventValidation="False"
                                Font-Size="8pt"
                                PageSize="50" AllowPaging="true"
                                PagerSettings-Mode="NumericFirstLast">
                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Ação" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkLiberar" runat="server" OnCheckedChanged="chkLiberar_CheckedChanged"  Checked='<%# Eval("imprimir").ToString().Equals("True") %>' AutoPostBack="true"/>
                                                </ContentTemplate> 
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkLiberar" EventName="CheckedChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NrDocumento" HeaderText="Pedido" />
                                    <asp:BoundField DataField="CodigoEmpresa" HeaderText="Emp" />
                                    <asp:BoundField DataField="CodigoCliente" HeaderText="Cód." />
                                    <asp:BoundField DataField="NomeCliente" HeaderText="Cliente"/>
                                    <asp:BoundField DataField="NomeCidade" HeaderText="Cidade"/>
                                    <asp:BoundField DataField="NomeBairro" HeaderText="Bairro"/>
                                    <asp:BoundField DataField="Transportadora" HeaderText="Transportadora"/>
                                    <asp:BoundField DataField="Cont" HeaderText="Itens" />
                                    <asp:BoundField DataField="NomeVendedor" HeaderText="Vendedor"/>
                                    <asp:BoundField DataField="CodigoDoca" HeaderText="Cód. Doca" />
                                    <asp:BoundField DataField="DescricaoSituacao" HeaderText="Situação" />
                                    <asp:BoundField DataField="CodigoSituacao" HeaderText="Cód.Sit" />
                                    <asp:BoundField DataField="CodigoDocumento" HeaderText="Cód.Doc" />                                        
                                    </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>
                    <div class="row" padding-left: 30px; padding-right:50px;">
                        <div class="col-md-12">

                        </div>
                    </div>

                </asp:Panel>
            </div>

        </div>
    </div>
</asp:Content>
<%--  --%>