<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" CodeBehind="VendaBalcaoPrincipal.aspx.cs" Inherits="SoftHabilInformatica.Pages.VendaBalcao.VendaBalcaoPrincipal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <table>
        <tr>
            <td style="width: 60%;">
                <div id="divCliente" style="padding-left: 20px; padding-top: 20px; padding-right: 10px; font-size: small">

                    <div class="panel panel-default">
                        <div class="panel-heading">Cliente</div>
                        <div class="panel-body">
                            00001 - NÃO IDENTIFICADO
                        </div>
                    </div>
                </div>

            </td>
            <td style="width: 80%;">
                <div id="divOperacoes" style="padding-left: 20px; padding-top: 20px; padding-right: 10px; font-size: small">
                    <div class="panel panel-default">
                        <div class="panel-heading">Operações do Caixa</div>
                        <div class="panel-body">
                            <asp:LinkButton ID="btnAbrirCaixa" runat="server" Font-Size="Small" Text="Abrir Caixa" CssClass="btn btn-primary" UseSubmitBehavior="false"> 
                                    <span aria-hidden="true" title="Suprimento" class="glyphicon glyphicon-save"></span>  Abrir Caixa
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnSupri" runat="server" Font-Size="Small" Text="Suprimento" CssClass="btn btn-success" UseSubmitBehavior="false"> 
                                    <span aria-hidden="true" title="Suprimento" class="glyphicon glyphicon-save"></span>  Suprimento
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnSangra" runat="server" Font-Size="Small" Text="Sangria" CssClass="btn btn-danger" UseSubmitBehavior="false"> 
                                    <span aria-hidden="true" title="Sangria" class="glyphicon glyphicon-save"></span>  Sangria
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnFechar" runat="server" Font-Size="Small" Text="Fechar Caixa" CssClass="btn btn-primary" UseSubmitBehavior="false"> 
                                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-save"></span>  Fechar Caixa
                            </asp:LinkButton>

                        </div>
                    </div>
                </div>

            </td>
        </tr>
    </table>

    <table>
        <tr style="height: 50%">
            <td style="padding-left: 20px; padding-top: 1px; padding-right: 10px; font-size: small; width: 700px;">
                <div id="divItens">

                    <div class="panel panel-primary">
                        <div class="panel-heading">Itens</div>
                        <div class="panel-body" style="padding-left: 20px; padding-top: 10px; padding-right: 20px; height: 200px">
                            1 - CocaCola                 2                  5,00                 10,00
                        </div>
                        <div class="panel-footer">SubTotal : </div>
                    </div>
                </div>
                <table>
                    <tr>
                        <td >
                            <div id="div1" style="padding-right: 20px; font-size: small">
                                <div class="panel panel-default">
                                    <div class="panel-heading">Operações da Venda</div>
                                    <div class="panel-body">
                                        <asp:LinkButton ID="LinkButton1" runat="server" Text="Abrir Caixa" Font-Size="Small" CssClass="btn btn-primary" UseSubmitBehavior="false"> 
                                <span aria-hidden="true" title="Suprimento" class="glyphicon glyphicon-save"></span>  Nova Venda
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="LinkButton2" runat="server" Text="Suprimento" Font-Size="Small" CssClass="btn btn-success" UseSubmitBehavior="false"> 
                                <span aria-hidden="true" title="Suprimento" Font-Size="Small" class="glyphicon glyphicon-save"></span>  Cancelar
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="LinkButton3" runat="server" Font-Size="Small" Text="Sangria" CssClass="btn btn-danger" UseSubmitBehavior="false"> 
                                <span aria-hidden="true" title="Sangria"  class="glyphicon glyphicon-save"></span>  Aguardar
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="LinkButton4" runat="server" Font-Size="Small" Text="Fechar Caixa" CssClass="btn btn-primary" UseSubmitBehavior="false"> 
                                <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-save"></span>  Finalizar
                                        </asp:LinkButton>

                                    </div>
                                </div>
                            </div>

                        </td>

                        <td >
                            <div id="div2" style="font-size: small">
                                <div class="panel panel-danger">
                                    <div class="panel-heading">Valor Total</div>
                                    <div class="panel-body" style="width:180px">
                                        R$ 1.000,00
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>

            </td>
            <td style="padding-left: 20px; padding-top: 1px; padding-right: 20px; font-size: small; width: 700px;">
                <div id="div3">

                    <div class="panel panel-primary">
                        <div class="panel-heading">Seleção</div>
                        <div class="panel-body" style="padding-left: 20px; padding-top: 10px; padding-right: 20px; height: 380px">
                        1
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    






</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cttRodape" runat="server">
</asp:Content>
