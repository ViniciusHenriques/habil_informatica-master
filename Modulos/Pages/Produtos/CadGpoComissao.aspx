<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadGpoComissao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Produtos.CadGpoComissao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div  class="panel panel-primary" >
                <div class="panel-heading">Cadastro de GpoComissaos
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                            <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                        </div> 
                    </asp:Panel>

                </div>
                <div class="panel-body">
                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>

                   <br/><br/>
                    <div class="col-md-8">
                        <div class="input-group ">
                            <span class="input-group-addon">Código da GpoComissao : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" Enabled="false" runat="server" TabIndex="1" 
                                onkeypress="return PermiteSomenteNumeros(event);" 
                                 MaxLength="2"  />
                        </div>
                    </div>
                    <div class="col-md-4">
                      <div class="input-group " style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Situação : &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="True"  CssClass="form-control"  Font-Size ="Small"  TabIndex="2" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="col-md-12">
                        <div class="input-group">
                            <span class="input-group-addon">Descrição da GpoComissao : </span>
                            <asp:TextBox ID="txtDescricao"  CssClass="form-control" name="txtDescricao" TabIndex="2" runat="server"  MaxLength="50" />
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnSalvar" />
                <asp:PostBackTrigger ControlID="btnExcluir" />
            </Triggers>

        </asp:UpdatePanel>
    </div>

       <!-- Exclui Bairro -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir esta GpoComissao? ?
          </div>
          <div class="modal-footer">
            <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:LinkButton>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>
</asp:Content>
