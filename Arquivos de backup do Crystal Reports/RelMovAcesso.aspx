<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" CodeBehind="RelMovAcesso.aspx.cs" Inherits="HabilInformatica.Pages.Acesso.RelMovAcesso" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
</asp:Content>
<style type="text/css">
    .form-control{
        z-index:0!important;
    }
</style>
<asp:Content ID="Content2" ContentPlaceHolderID="cttRodape" runat="server">
   <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;" >

            <div class="panel panel-primary">
                <div class="panel-heading">Relatório Movimentação de Acessos
                    <div class="messagealert" id="alert_container"></div>
                </div>
                
                <div class="panel-body">
                    <div class="container-fluid">
                        <div class="row" style="background-color:white;border:none;">
                            <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <asp:LinkButton ID="BtnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="BtnVoltar_Click"> 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                </asp:LinkButton>
                            </div>        
                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <asp:LinkButton ID="BtnGerar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="BtnGerar_Click"> 
                                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Gerar Relatório
                                </asp:LinkButton>
                            </div>        
                            <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style="font-size:small;">Tipo Relatório : </label>
                            </div>        

                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <asp:DropDownList ID="ddlTipoRelatorio" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Completo" Value="1" Selected="True" />
                                    <asp:ListItem Text="Quebra por Tipos de Acesso" Value="2" />
                                    <asp:ListItem Text="Quebra por Pessoas" Value="3" />
                                    <asp:ListItem Text="Quebra por Veículos" Value="4" />
                                </asp:DropDownList>
                            </div>        
                        </div>
                    </div>
                    <br/>
                    <br/>
                    <CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="True" HasRefreshButton="True"
                        EnableDatabaseLogonPrompt="False" Height="50px" ToolPanelWidth="200px" Width="350px" />
                </div>
            </div> 
    </div>
</asp:Content>
