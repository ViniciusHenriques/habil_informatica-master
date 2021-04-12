 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConAtividadeExtra.aspx.cs" Inherits="SoftHabilInformatica.Pages.Vendas.ConAtividadeExtra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <style>
         
        .ButtonGrid{
            padding:0!important;           
        }
        @media screen and (max-width: 1000px) {
            .scroll-on {
                overflow:auto;
            }
        }
    </style>
    <body>
        <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
            <div class="panel panel-primary" >
                <div  class="panel-heading">Atividades Extras
                    <div class="messagealert" id="alert_container"></div>
                </div>            
                <div class="panel-body scroll-on" >
                    <div class="row">    
                        <div class ="col-md-12" style="text-align:center">
                            
                            <asp:Label runat="server" ID="lblNenhumAtividade">Nenhuma atividade liberada</asp:Label>
                            <asp:GridView ID="grdAtividades" runat="server" Width="100%" 
                                    AutoGenerateColumns="False" CssClass="table table-hover"
                                    GridLines="None" 
                                    Font-Size="8pt" 
                                    OnRowCommand="grdAtividades_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="CodigoIndex" HeaderText="Código" ItemStyle-Width = "2%"/>
                                    <asp:BoundField DataField="DataHoraLancamento" HeaderText="Data/Hora Lançamento"  ItemStyle-Width = "5%" />
                                    <asp:BoundField DataField="DescricaoAtividade" HeaderText="Descrição" ItemStyle-Width = "18%"  />
                                    <asp:BoundField DataField="DescricaoFiltro" HeaderText="Filtro" />
                                    <asp:BoundField DataField="Chave" HeaderText="Chave"  ItemStyle-Width = "18%" />
                                    <asp:TemplateField HeaderText="Impostos" ItemStyle-CssClass="centerVertical " ItemStyle-Width = "2%">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" id="chkImpostos" Checked='<%# Eval("Impostos").ToString().Equals("True") %>' Visible='<%# Eval("VisibleCheckBox")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Situação" ItemStyle-CssClass="centerVertical " ItemStyle-Width = "8%">
                                        <ItemTemplate>
                                            <label class="badge <%# Eval("Cpl_DsSituacao").ToString() %> "><%# Eval("Cpl_DsSituacao").ToString() %></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gerar Relatório" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="ButtonGrid" ItemStyle-Width = "5%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnGerar" runat="server" CssClass="btn btn-link " CommandName="Gerar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  >
                                                <img runat="server" src="../../Images/download.png" width="20" height="20"/>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cancelar" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="ButtonGrid" ItemStyle-Width = "5%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-link " CommandName="Cancelar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  >
                                                <img runat="server" src="../../Images/excluir.png" width="20" height="20"/>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>                    
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView> 
                               
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </body>
</asp:Content>
