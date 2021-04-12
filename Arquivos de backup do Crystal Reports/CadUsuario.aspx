<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadUsuario.aspx.cs" Inherits="SoftHabilInformatica.Pages.Usuarios.CadUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../../Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">

        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
            <ContentTemplate>

                <div class="panel panel-primary">
                    <div class="panel-heading">Cadastro de Usuários do Sistema
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                            <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                        </div> 
                    </asp:Panel>

                    </div>
                    <div class="panel-body">

                        <asp:Panel ID="pnlCadastrarSenha" runat="server" Visible="false">
                            <div class="panel panel-default" style ="width:30%">
                                <div class="panel-heading">Inclusão/Alteração de Senhas</div>
                                <div class="panel-body">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <asp:Label ID="Label1" runat="server" Text="Senha Atual : "></asp:Label></span>
                                        <asp:TextBox ID="txtSenhaAtual" CssClass="form-control" runat="server" TabIndex="12" TextMode="Password" UseSubmitBehavior="false"></asp:TextBox>
                                    </div>
                                    <br />
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <asp:Label ID="Label4" runat="server" Text="Nova Senha : "></asp:Label></span>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtNovaSenha" TabIndex="13" TextMode="Password" UseSubmitBehavior="false"></asp:TextBox>
                                    </div>
                                    <br />
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <asp:Label ID="Label5" runat="server" Text="Confirma Senha : "></asp:Label></span>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtConfirmaSenha" TabIndex="14" TextMode="Password" UseSubmitBehavior="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="panel-footing">
                                    <asp:Button ID="btnCfmSenhaMesmo" runat="server" Text="Confirma" CssClass="btn btn-primary" AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnCfmSenhaMesmo_Click" TabIndex="15"></asp:Button>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                            <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnResetarSenha" runat="server" Text="Reiniciar Senha" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnResetarSenha_Click"> 
                            <span aria-hidden="true" title="Salvar" class="	glyphicon glyphicon-ok"></span>  Reiniciar Senha
                        </asp:LinkButton>

                        <br />
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                <div class="input-group ">
                                    <span class="input-group-addon">Usuário: &nbsp;&nbsp;</span>
                                    <asp:TextBox ID="txtCodUsuario" runat="server" CssClass="form-control"  Enabled="false" />
                                </div>
                            </div>

                            <div class="col-md-7">
                                <div class="input-group" >
                                    <span class="input-group-addon">Pessoa: </span>
                                    <asp:TextBox ID="txtCodPessoa" CssClass="form-control" runat="server" OnTextChanged="txtCodPessoa_TextChanged" TabIndex="1" AutoPostBack="true"  Width="80%" MaxLength="9" />                                                      
                                    <asp:LinkButton ID="btnPessoa" title="Pesquise os Pessoas" runat="server" CssClass="form-control btn btn-primary" Width="20%" TabIndex="2" OnClick="btnPessoa_Click" AutoPostBack="true"> 
                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="input-group">
                                    <span class="input-group-addon">Situação: </span>
                                    <asp:DropDownList ID="ddlSituacao" runat="server" Font-Size="Small" width="100%" CssClass="form-control" TabIndex="3"></asp:DropDownList>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-9">
                                <div class="input-group">
                                    <span class="input-group-addon">Nome: </span>
                                    <asp:TextBox ID="txtPessoa" CssClass="form-control" name="txtCodPessoa"  runat="server"  Enabled="false" Visible ="true"   TabIndex="4"/>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="input-group">
                                    <span class="input-group-addon">Cargo: </span>
                                    <asp:DropDownList ID="ddlCargo" runat="server" Font-Size="Small" width="100%" CssClass="form-control" TabIndex="5"></asp:DropDownList>
                                </div>
                            </div>


                        </div>
                        
                        <div class="row">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <span class="input-group-addon">Login :</span>
                                    <asp:TextBox ID="txtLogin" runat="server" MaxLength="20" CssClass="form-control"
                                        PlaceHolder="Informe Texto" TabIndex="6" UseSubmitBehavior="false" Width ="40%" />

                                    <asp:LinkButton ID="btnDisponivel" runat="server" Text="Verificar Disponibilidade" CssClass="btn btn-success" TabIndex="7" UseSubmitBehavior="false" OnClick="btnDisponivel_Click"> 
                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-ok"></span>  Verificar Disponibilidade
                                    </asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="Button4" runat="server" Text="Cadastrar Senha" CssClass="btn btn-primary" TabIndex="8"
                                        AutoPostBack="false" UseSubmitBehavior="false" OnClick="Button4_Click"> 
                                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-link"></span>  Senhas
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="input-group">
                                    <span class="input-group-addon">Perfil:</span>
                                    <asp:TextBox ID="txtCodPerfil"
                                        runat="server"
                                        Width="20%"
                                        PlaceHolder="Informe Número"
                                        pattern="[0-9]+$"
                                        TabIndex="9"
                                        AutoPostBack="true"
                                        OnTextChanged="txtCodPerfil_TextChanged"
                                        onkeypress="return PermiteSomenteNumeros(event);"
                                        MaxLength="2" CssClass="form-control" />

                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="form-control btn btn-primary" Width="10%" TabIndex="10" data-toggle="modal" data-target="#myCadPerfil"> 
                                                    <span aria-hidden="true" title="Perfil" class="glyphicon glyphicon-search"></span>
                                    </asp:LinkButton>
                                    <asp:TextBox ID="txtDcrPerfil" runat="server" CssClass="form-control" Enabled="false" Width="70%" />
                                </div>
                            </div>
                        </div>


                        <asp:TextBox ID="txtSenha" runat="server" MaxLength="128"
                            Width="1000" PlaceHolder="Informe Texto" Visible="false" UseSubmitBehavior="false" TabIndex="11"/>
                        <br />

                    </div>
                </div>
            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtCodPessoa" EventName="TextChanged" />
                <asp:PostBackTrigger ControlID="btnSalvar" />
                <asp:PostBackTrigger ControlID="btnExcluir" />
                <asp:PostBackTrigger ControlID="btnCfmSenhaMesmo" />
                <asp:PostBackTrigger ControlID="btnDisponivel" />
            </Triggers>

        </asp:UpdatePanel>
    </div>

    <!-- Large modal -->
    <div class="modal fade" id="myCadPerfil"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:900px;">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H1">Zoom de Perfis de Usuários </h4>
          </div>
          <div class="modal-body">
              <iframe id="iframe1" src="ConPflUsuario2.aspx" width='850' height='60%' frameborder='0'></iframe>
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnCfmPerfil" runat="server" Text="Confirma" CssClass="btn btn-primary" UseSubmitBehavior ="false"  OnClick="btnCfmPerfil_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>
    <!-- Large modal -->
    

       <!-- Exclui Bairro -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H4">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir este Usuário ?
          </div>
          <div class="modal-footer">
            <asp:LinkButton ID="btnCfmSim2" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim2_Click"></asp:LinkButton>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>
