<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="RelPflUsuario.aspx.cs" Inherits="HabilInformatica.Relatorios.RelPflUsuario" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

    <script src="../../Scripts/funcoes.js"></script>
    <script type="text/javascript">
        function openModal() {
            $('#myModalMsg').modal('show');
        }
    </script>
    
        <div class="panel panel-primary">
            <div  class="panel-heading">Relatório de Perfis do Usuários</div>
            <div class="panel-body" style="">

                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>
                <asp:LinkButton ID="btnVisualizar" runat="server" Text="Visualizar" CssClass="btn btn-success" OnClick="btnVisualizar_Click"> 
                    <span aria-hidden="true" title="Visualizar" class="glyphicon glyphicon-sunglasses"></span>  Visualizar
                </asp:LinkButton>

                <asp:LinkButton ID="btnImprimir" runat="server"  UseSubmitBehavior="false" CssClass="btn btn-warning" OnClick="btnImprimir_Click"> 
                    <span aria-hidden="true" title="Imprimir" class="glyphicon glyphicon-print"></span>  Imprimir
                </asp:LinkButton>

                <asp:LinkButton ID="btnMensagem" runat="server" Text="Mensagem"  Visible="false" class="btn btn-default" data-toggle="modal"  data-target="#myModalMsg" OnClick="btnMensagem_Click"> 
                </asp:LinkButton>

                <asp:Panel ID ="pnlMensagem" runat="server" Visible="false" >
                        <!-- Modal -->
                        <div class="modal fade" id="myModalMsg" role="dialog">
                        <div class="modal-dialog modal-sm" style="width:30%;position: fixed; top:30%; left:35%;">
                            <div class="modal-content">
                                <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h5 class="modal-title" id="H3">Atenção</h5>
                                </div>
                                <div class="modal-body">
                                    <p><asp:Label ID ="lblMensagem" runat="server" Font-Size="Small"></asp:Label></p>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnCfmNao" runat="server" Text="Fechar" CssClass="btn btn-default" data-dismiss="modal" TabIndex="-1" UseSubmitBehavior="false"  OnClick="btnCfmNao_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        </div>
                </asp:Panel>

                <br />
                <br />
                <div class="input-group">
                    <span class="input-group-addon">Pesquisar por: 
                        <asp:DropDownlist id="ddlPesquisa" AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddlPesquisa_SelectedIndexChanged"/>
                    </span>
                    &nbsp;
                    <asp:TextBox ID="txtVarchar" CssClass="form-control" Width="150" runat="server" />
                </div>
                <br />    
 


                <table id="reportviewertable" align="center">
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="760px">
                          <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" SizeToReportContent="True" Height="100%">
                                <LocalReport ReportPath="Relatorios\RLDC\RelPflUsuario.rdlc">
                                </LocalReport>
                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>
           </div>
        </div>

    </div>

</asp:Content>
