<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadEstoque.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.CadEstoque" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<%--    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />--%>
    <script src="../../Scripts/funcoes.js"></script>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script>
        $(document).ready(function () {
            $('.js-example-basic-single').select2({
                
            });
        });
    </script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">

        <div class="panel panel-primary">
            <div class="panel-heading">
                Cadastro de Estoque
                    <div class="messagealert" id="alert_container"></div>
                
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body">
                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click" TabIndex="11"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>

                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" TabIndex="9"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>

                <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" TabIndex="10"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                </asp:LinkButton>
                <div class="row" style="margin-top: 10px">
                    <div class="col-md-3">
                        <label for="usr">Lançamento: </label>
                        <asp:TextBox ID="txtLancamento" CssClass="form-control" name="txtLancamento" Enabled="false" runat="server" TabIndex="1"  />
                    </div>
                     <div class="col-md-6">
                        <label for="usr"> Empresa: </label>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                               <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="2"> </asp:DropDownList>		   
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
		            <div class="col-md-3">
                        <label for="usr"> Situação: </label>
                        <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="3"> </asp:DropDownList>		   
                    </div> 
                 </div>
                <div class="row" style="margin-top: 10px">
                     <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                        <div class="col-md-4">
                            <label for="usr"> Localização: </label>
                            <asp:DropDownList ID="ddlLocalizacao" runat="server" AutoPostBack="false" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="4"> </asp:DropDownList>		   
                        </div> 
                        </ContentTemplate> 
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlLocalizacao" EventName="TextChanged" />
                     

                    </Triggers>
                 </asp:UpdatePanel>
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                     <div class="col-md-8" >
                 <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Produto </label>
                 <div class="input-group " style="width:100%">                              
                     <asp:TextBox ID="txtProduto" CssClass="form-control"  runat="server" TabIndex="5" OnTextChanged="txtProduto_TextChanged" OnSelectedIndexChanged="txtProduto_SelectedIndexChanged" AutoPostBack="true"                                
                     Width="25%" MaxLength="6" />                                                      
                     <asp:LinkButton ID="btnPesquisarItem"  runat="server" CssClass="form-control btn btn-primary" Width="10%" TabIndex="6" OnClick="btnPesquisarItem_Click" AutoPostBack="true"> 
                             <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                     </asp:LinkButton>
                     <asp:TextBox ID="txtDcrproduto" CssClass="form-control"  runat="server"  Enabled="false"  Width="65%"  />
                 </div>
             </div> 
                     </ContentTemplate> 
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtProduto" EventName="TextChanged" />
 
                        <asp:AsyncPostBackTrigger ControlID="txtDcrproduto" EventName="TextChanged" />
                     

                    </Triggers>
                 </asp:UpdatePanel>
                </div> 
                <div class="row" style="margin-top: 10px">
                                  <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                     <div class="col-md-6">
                            <label for="usr"> Lote: </label>
                         <asp:DropDownList ID="ddlLote" runat="server" AutoPostBack="true"  CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="7"> </asp:DropDownList>		   
                     </div> 
                      <div class="col-md-6">
                         <label for="usr"> Quantidade:  </label>
                             <asp:TextBox ID="txtquantidade" CssClass="form-control" AutoPostBack="true" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="8" MaxLength="4" OnTextChanged="txtquantidade_TextChanged"/>
                     </div>

                   
                            </ContentTemplate> 
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtquantidade" EventName="TextChanged" />
                     

                    </Triggers>
                 </asp:UpdatePanel>
             </div>
        </div>
    </div>
</div>



   <!-- Exclui Estoque -->
<div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:100%;width:300px">
            <div class="modal-content" >
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir este Estoque ?
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
    <script>
        function pageLoad(sender, args) {
            $(".js-example-basic-single").select2();
        }

    </script>    

</asp:Content>
