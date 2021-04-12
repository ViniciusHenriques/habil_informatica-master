<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ManTabIcms.aspx.cs" 
    Inherits="SoftHabilInformatica.Pages.Impostos.ManTabIcms"  MaintainScrollPositionOnPostback="False" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script> 
    $(document).ready(function() {
        $('.js-example-basic-single').select2();
    });
     </script>
     <style type="text/css">
         .select2-selection--single{
             height:33px!important;
             font-size:12px;
             padding:2px;
        }
        .select2-selection__arrow{
            height:30px!important;
        }
        .form-control{
            z-index:0!important;
        }
    </style>



    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary">
            <div class="panel-heading">Manutenção na Tabela de Alíquotas ICMS
                <div class="messagealert" id="alert_container"></div>
            </div>
            <div class="panel-body">
                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar Permissões" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>
                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
                <br/>
                <br/>
                <div class="row" style="background-color:white;border:none;">
                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                        <asp:DropDownList ID="ddlEstadoOrigem" runat="server"  AutoPostBack="False"  CssClass="form-control js-example-basic-single" Width="100%">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                        <asp:LinkButton ID="btnProcurar" runat="server" Text="Procurar" CssClass="btn btn-primary" UseSubmitBehavior="false" OnClick="btnProcurar_Click"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-search"></span>  Procurar
                        </asp:LinkButton>
                    </div>
                </div>
                
                <br/>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:GridView ID="grdTabIcms" runat="server"
                            Width="100%"
                            CssClass="table table-bordered table-hover table-striped"
                            GridLines="None" AutoGenerateColumns="False"
                            Font-Size="8pt" >
                            <Columns>
                                <asp:BoundField DataField="CodTabAliqIcms" HeaderText="Item" />
                                <asp:BoundField DataField="UFOrigem" HeaderText="Estado Origem" HeaderStyle-Width="30%" />
                                <asp:BoundField DataField="UFDestino" HeaderText="Estado Destino" HeaderStyle-Width="30%"  />
                                <asp:TemplateField HeaderText="ICMS Interno"> 
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtIcmsInterno" CssClass="form-control" runat="server" Text='<%# Eval("IcmsInterno").ToString() %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="ICMS Interestadual">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtIcmsInterEstadual" CssClass="form-control" runat="server" Text='<%# Eval("IcmsInterEstadual").ToString() %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="ICMS Produto do Exterior">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtIcmsExterno" name="txtIcmsExterior" CssClass="form-control" runat="server" Text='<%# Eval("IcmsExterno").ToString() %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <RowStyle CssClass="cursor-pointer" />
                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdTabIcms" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div> 
</asp:Content>

