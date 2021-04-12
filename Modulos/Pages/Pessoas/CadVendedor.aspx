<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadVendedor.aspx.cs" Inherits="SoftHabilInformatica.Pages.Pessoas.CadVendedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <%--    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />--%>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script>
        function MostrarModalConfirmSemUsuario() {
            $("#ModalConfirmSemUsuario").modal("show");
        }
    </script>
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">

        <div class="panel panel-primary">
            <div class="panel-heading">
                Cadastro de Vendedores
                    <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body">
                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>

                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>

                <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                </asp:LinkButton>


                <asp:UpdatePanel ID="UpdatePanel3" runat="server">

                    <ContentTemplate>

                        <div class="row" style="margin-top: 10px">

                            <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <label for="usr" style="margin-top: 1px;">Empresa</label>
                                <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="true" TabIndex="0" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_TextChanged" />
                            </div>

                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <label for="usr" style="margin-top: 1px;">Pessoa</label>

                                <div class="input-group " style="width: 100%">
                                    <asp:TextBox ID="txtCodPessoa"
                                        CssClass="form-control"
                                        runat="server"
                                        OnTextChanged="txtCodPessoa_TextChanged" AutoPostBack="true" Width="50%" MaxLength="14" Style="left: 0px; top: 0px" />

                                    <asp:LinkButton ID="BtnConPessoa" runat="server" CssClass="form-control btn btn-primary" TabIndex="-1" OnClick="BtnConPessoa_Click" Width="25%"> 
                                                        <span aria-hidden="true" title="Usuários" class="glyphicon glyphicon-search"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <label for="usr">Código Usuário</label>
                                <asp:TextBox ID="txtCodUsuario" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <label for="usr">Situação</label>
                                <asp:DropDownList ID="ddlSituacao" runat="server" Font-Size="Small" Width="100%" CssClass="form-control" TabIndex="3"></asp:DropDownList>
                            </div>


                        </div>
                        <div class="row" style="margin-top: 10px">

                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <label for="usr">Código Vendedor</label>
                                <asp:TextBox ID="txtCodVendedor" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>

                           <div class="col-md-10" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <label for="usr">Nome do Vendedor</label>
                                <asp:TextBox ID="txtNomePessoa" runat="server" CssClass="form-control" Enabled="false"  />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">

                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <label for="usr" style="margin-top: 1px;">Tipo de Vendedor</label>
                                <asp:DropDownList ID="ddlTipoVendedor" runat="server" AutoPostBack="true" TabIndex="0" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlTipoVendedor_TextChanged" />
                            </div>

                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <label for="usr">Comissão %</label>
                                <asp:TextBox ID="txtPercentualComissao" runat="server" CssClass="form-control" Enabled="true" Width="75%" />
                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlTipoVendedor" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnConPessoa" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="txtCodPessoa" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Exclui Vendedor -->
    <div class="modal fade" id="myexcpes" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir este Vendedor ?
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="ModalConfirmSemUsuario" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" style="height: 100%; width: 500px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H3">Atenção</h4>
                        </div>
                        <div class="modal-body">
                            Vendedor não tem usuário, logo não poderá utilizar o sistema apartir do mesmo, continuar ?
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSimSemUsuario" OnClientClick="this.disabled = true; this.value = 'Salvando...';" UseSubmitBehavior="false" OnClick="btnSimSemUsuario_Click" CssClass="btn btn-success"  Text="Sim" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Não</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSimSemUsuario"  />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
