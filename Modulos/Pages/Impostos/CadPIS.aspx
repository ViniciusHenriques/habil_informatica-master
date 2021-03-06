<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadPIS.aspx.cs" Inherits="SoftHabilInformatica.Pages.Impostos.CadPIS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<%--    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />--%>
    <script src="../../Scripts/funcoes.js"></script>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">

        <div class="panel panel-primary">
            <div class="panel-heading">
                Cadastro de PIS
                    <div class="messagealert" id="alert_container"></div>
                
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body">
                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click" TabIndex="8"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>

                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" TabIndex="6"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>

                <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" TabIndex="7"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                </asp:LinkButton>
                  <div class="row" style="margin-top: 10px">

                    <div class="col-md-3">
                        <label for="usr">Lançamento: </label>
                        <asp:TextBox ID="txtLancamento" CssClass="form-control" name="txtLancamento" Enabled="false" runat="server" TabIndex="1"  />
                    </div>
                    <div class="col-md-3">
                        <label for="usr"> Código PIS:  </label>
                        <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo_TextChanged" Font-Size ="Medium" runat="server" TabIndex="2" MaxLength="4"/>
                    </div>
            
		               	  <div class="col-md-3">
                    <label for="usr"> Tipo: </label>
                               <asp:DropDownList ID="ddlPIS" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="3"> </asp:DropDownList>		   
                          </div> 
                        <div class="col-md-3">
                        <label for="usr"> Valor do PIS:  </label>
                        <asp:TextBox ID="txtVLPIS" CssClass="form-control" name="txtCodigo_TextChanged" Font-Size ="Medium" runat="server" TabIndex="4" MaxLength="4"/>
                    </div>

                      </div>
                <div class="row" style="margin-top: 10px">
                <div class="col-md-12">
                        <label for="usr"> Descrição PIS: </label>
                        <asp:TextBox ID="txtDescricaoPIS"  CssClass="form-control" name="txtDescricaoPIS" TabIndex="5" runat="server"  MaxLength="150" TextMode="multiline" Columns="8" Rows="4"/>
                    </div>
                   
                </div>
            </div>
        </div>
     </div>
       <!-- Exclui PIS -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:100%;width:300px">
            <div class="modal-content" >
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir este PIS ?
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
</asp:Content>
