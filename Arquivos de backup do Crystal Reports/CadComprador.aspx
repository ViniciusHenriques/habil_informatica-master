<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadComprador.aspx.cs" Inherits="SoftHabilInformatica.Pages.Pessoas.CadComprador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<%--    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />--%>
    <script src="../../Scripts/funcoes.js"></script>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;" >

            <div class="panel panel-primary">
                <div class="panel-heading">Cadastro de Compradores
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                            <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
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

                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>
                    

                    <div class="row" style="margin-top:10px">
                           
                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr">Código Comprador</label>
                            <asp:TextBox ID="txtCodComprador" runat="server" Enabled="false" CssClass="form-control" ></asp:TextBox> 
                        </div>
                        <div class="form-group col-md-10" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="exampleInputPassword1">Nome do Comprador</label>
                            <asp:TextBox ID="txtNomeComprador" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                         
                                        <label for="usr" style ="margin-top:1px; ">Pessoa</label>
                                        <div class="input-group " style="width:100%">
                                                                               
                                            <asp:TextBox ID="txtCodPessoa"
                                                    CssClass="form-control"
                                                    runat="server"
                                                        OnTextChanged="txtCodPessoa_TextChanged" AutoPostBack="true"  Width="15%" MaxLength="14"   />

                                            <asp:LinkButton ID="BtnConPessoa" runat="server" CssClass="form-control btn btn-primary" TabIndex="-1" OnClick="BtnConPessoa_Click" Width="10%"> 
                                                <span aria-hidden="true" title="Usuários" class="glyphicon glyphicon-search"></span>
                                            </asp:LinkButton>
                                            <asp:TextBox ID="txtNomePessoa" runat="server" CssClass="form-control" Enabled="false" Width="75%" />

                                        </div>
                                        
                                    <%--<div class="form-group">
                                        <label for="usr">Pessoa</label>
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtCodPessoa"
                                                CssClass="form-control"
                                                runat="server"
                                                    OnTextChanged="txtCodPessoa_TextChanged" AutoPostBack="true"  Width="100" MaxLength="14"   />

                                                <asp:LinkButton ID="BtnConPessoa" runat="server" CssClass="form-control btn btn-primary" TabIndex="-1" OnClick="BtnConPessoa_Click"> 
                                                    <span aria-hidden="true" title="Usuários" class="glyphicon glyphicon-search"></span>
                                                </asp:LinkButton>
                                                <asp:TextBox ID="txtNomePessoa" runat="server" CssClass="form-control" Enabled="false" Width="470" />

                                        </div>
                                    </div>--%>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtCodPessoa" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnConPessoa" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="form-group col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:1px;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label for="usr" style ="margin-top:1px; ">Usuário</label>
                                        <div class="input-group " style="width:100%">
                                                                               
                                            <asp:TextBox ID="txtCodUsuario"
                                                    CssClass="form-control"
                                                    runat="server"
                                                        OnTextChanged="txtCodUsuario_TextChanged" AutoPostBack="true"  Width="15%" MaxLength="14"   />

                                            <asp:LinkButton ID="btnConUsuario" runat="server" CssClass="form-control btn btn-primary" TabIndex="-1" OnClick="btnConUsuario_Click" Width="10%"> 
                                                <span aria-hidden="true" title="Usuários" class="glyphicon glyphicon-search"></span>
                                            </asp:LinkButton>
                                            <asp:TextBox ID="txtNomeUsuario" runat="server" CssClass="form-control" Enabled="false" Width="75%" />

                                        </div>
                                        <%--<label for="usr">Usuario</label>
                                        <%-- <div class="form-inline">
                                            <asp:TextBox ID="txtCodUsuario"
                                                CssClass="form-control"
                                                runat="server"
                                                    OnTextChanged="txtCodUsuario_TextChanged" AutoPostBack="true"  Width="100" MaxLength="14"   />

                                                <asp:LinkButton ID="btnConUsuario" runat="server" CssClass="form-control btn btn-primary" TabIndex="-1" OnClick="btnConUsuario_Click"> 
                                                    <span aria-hidden="true" title="Usuários" class="glyphicon glyphicon-search"></span>
                                                </asp:LinkButton>
                                                <asp:TextBox ID="txtNomeUsuario" runat="server" CssClass="form-control" Enabled="false" Width="470" />
                                        </div>--%>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtCodUsuario" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnConUsuario" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div> 
    </div>

       <!-- Exclui Comprador -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir este Comprador ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>
