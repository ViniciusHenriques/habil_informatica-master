<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="CadBanco.aspx.cs" Inherits="SoftHabilInformatica.Pages.Financeiros.CadBanco"  
     %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Cadastro de Banco
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
                 </div>
                <br/>
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none;">
                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Código</label>
                            <asp:TextBox ID="txtCod" CssClass="form-control" runat="server"  Text ="" Font-Size="Small" TabIndex="1"/>
                        </div>
                        <div class="col-md-10" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                            <label for="usr" style ="margin-top:1px;">Descrição</label>
                            <asp:TextBox ID="txtDescricao" AutoPostBack="False" CssClass="form-control" TabIndex="2" runat="server" />
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
                    Deseja Excluir esta conta corrente?
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="False" OnClick="btnCfmSim_Click"></asp:Button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
