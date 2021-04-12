<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="CadEmbRetornavel.aspx.cs"
    Inherits="SoftHabilInformatica.Pages.Estoque.CadEmbRetornavel" MaintainScrollPositionOnPostback="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
        function abc()
        {
            $("#CdCaixas .modal-title").html("");
            $("#CdCaixas").modal("show");
        }
    </script>

    <script src="../../Scripts/jsMensagemAlert.js"></script>
    
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Embalagem Retornável
                <div class="messagealert" id="alert_container"></div>
            </div>

            <div class="panel-body">
                <div class="row" style="margin-top:5px; margin-left:5px">
                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click" TabIndex="11"> 
                            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" TabIndex="9"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnExcluir" runat="server" OnClick="btnExcluir_Click" Text="Excluir" CssClass="btn btn-danger" TabIndex="10"> 
                            <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>
                </div>

                <div class="row" style="margin-top:25px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-2">
                                <asp:label Font-Bold="true" runat="server"> Lançamento: </asp:label>
                                <asp:TextBox ID="txtLancamento" Enabled="false" CssClass="form-control" AutoPostBack="true" Text="Novo" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4" />
                             </div>
                            <div class="col-md-7">
                                <asp:label Font-Bold="true" runat="server"> Código de Barras: </asp:label>
                                <asp:TextBox ID="txtCodBarras" CssClass="form-control" OnTextChanged="txtCodBarras_TextChanged" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="14" MaxLength="14"/>
                            </div>
                            <div class="col-md-3">
                                <asp:label Font-Bold="true" runat="server"> Situação: </asp:label>
                                <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="true" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="4"> </asp:DropDownList>		   
                            </div> 
                        </ContentTemplate>
                            <Triggers>
								<asp:AsyncPostBackTrigger ControlID ="txtCodBarras" EventName="TextChanged"/>
								<asp:PostBackTrigger ControlID ="ddlSituacao"/>
							</Triggers>
                    </asp:UpdatePanel> 
                </div> 
            </div>
        </div>
    </div>
     <!-- Exclui Estoque -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Always">
            <ContentTemplate>
                <div class="modal fade" id="CdCaixas" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                    <div class="modal-dialog" role="document" style ="height:100%;width:300px">
                        <div class="modal-content" >
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title" id="H3">Atenção</h4>
                            </div>
                            <div class="modal-body">
                                Deseja Excluir esta Embalagem?
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click" > 
                                    <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                                </asp:LinkButton>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
    </asp:UpdatePanel> 

</asp:Content>
<%--  --%>  