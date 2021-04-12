<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="RelModSistema.aspx.cs" Inherits="GestaoInterna.Relatorios.RelModSistema" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <div class="panel panel-primary">
            <div  class="panel-heading">Relatório de Módulos do Sistema</div>
            <div class="panel-body">

                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>
                <asp:LinkButton ID="btnVisualizar" runat="server" Text="Visualizar" CssClass="btn btn-success" OnClick="btnVisualizar_Click"> 
                    <span aria-hidden="true" title="Visualizar" class="glyphicon glyphicon-sunglasses"></span>  Visualizar
                </asp:LinkButton>

                <asp:LinkButton ID="btnImprimir" runat="server"  CssClass="btn btn-warning" OnClick="btnImprimir_Click"> 
                    <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-print"></span>  Imprimir
                </asp:LinkButton>
                <br />
                <br />
                <br />    
 


                <table id="reportviewertable" align="center">
                    <tr>
                        <td>
                            <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="True" ForeColor="Red" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td width="760px">
                          <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" SizeToReportContent="True" Height="100%" ZoomMode="Percent">
                                <LocalReport ReportPath="Relatorios\RLDC\RelModSistema.rdlc">
                                </LocalReport>
                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>

           
                <p>
                    <asp:Label ID="lblMensagem" runat="server" />
                </p>
            </div>
        </div>
    </div>
</asp:Content>
