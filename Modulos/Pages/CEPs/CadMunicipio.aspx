<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadMunicipio.aspx.cs" Inherits="SoftHabilInformatica.Pages.CEPs.CadMunicipio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>

    <script type="text/javascript">
        function openModal() {
            $('#myModalMsg').modal('show');
        }
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <asp:UpdatePanel ID="upPrincipal" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" DefaultButton="btnButton">

                <asp:Button ID="btnButton" runat="server" OnClick="btnMensagem_Click" BackColor="White" BorderStyle="None" />

                <div class="panel panel-primary">
                    <div class="panel-heading">Cadastro de Municípios</div>
                    <div class="panel-body" style="">
                        <asp:Panel ID="pnlExcluir" runat="server" Visible="false">
                            <div class="modal-dialog" role="document" style="height: 30%; width: 500">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title" id="H2">Atenção</h4>
                                    </div>
                                    <div class="modal-body">Deseja Excluir este Município ?</div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnCfmSim" runat="server" Text="Sim" CssClass="btn btn-danger" TabIndex="-1" UseSubmitBehavior="false" OnClick="btnCfmSim_Click"></asp:Button>
                                        <asp:Button ID="btnCfmNao" runat="server" Text="Não" CssClass="btn btn-default" TabIndex="-1" UseSubmitBehavior="false" OnClick="btnCfmNao_Click"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" UseSubmitBehavior="false" OnClick="btnExcluir_Click"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>


                        <asp:LinkButton ID="btnMensagem" runat="server" Text="Mensagem" class="btn btn-default" data-toggle="modal" Visible="false" data-target="#myModalMsg" OnClick="btnMensagem_Click"> 
                        </asp:LinkButton>
                        <asp:Panel ID="pnlMensagem" runat="server" Visible="false">
                            <!-- Modal -->
                            <div class="modal fade" id="myModalMsg" role="dialog">
                                <div class="modal-dialog modal-sm" style="width: 30%; position: fixed; top: 30%; left: 35%;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                            <h5 class="modal-title" id="H3">Atenção</h5>
                                        </div>
                                        <div class="modal-body">
                                            <p>
                                                <asp:Label ID="lblMensagem" runat="server" Font-Size="Small"></asp:Label></p>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnCfmNao2" runat="server" Text="Fechar" CssClass="btn btn-default" data-dismiss="modal" TabIndex="-1" UseSubmitBehavior="false" OnClick="btnCfmNao_Click"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>



                        <br />
                        <br />


                        <div class="input-group">
                            <span class="input-group-addon">Código do Município : &nbsp;&nbsp;&nbsp; </span>
                            <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" runat="server" TabIndex="1"
                              MaxLength="8" OnTextChanged="txtCodigo_TextChanged" />
                        </div>

                        <br />


                        <asp:UpdatePanel ID="upEstado" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="input-group">
                                    <span class="input-group-addon">Código do Estado: &nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp</span>
                                    <asp:TextBox ID="txtCodEstado"
                                        runat="server"
                                        Width="20%"
                                        PlaceHolder="Informe Número"
                                        pattern="[0-9]+$"
                                        TabIndex="2"
                                        UseSubmitBehavior="false"
                                        OnTextChanged="txtCodEstado_TextChanged"
                                        onkeypress="return PermiteSomenteNumeros(event);"
                                        MaxLength="2" CssClass="form-control"  />

                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="form-control btn btn-primary" Width="15%" UseSubmitBehavior="false" TabIndex="-1" data-toggle="modal" data-target="#myCadSisEstado"> 
                                                <span aria-hidden="true" title="Estado" class="glyphicon glyphicon-search" ></span>
                                    </asp:LinkButton>
                                    <asp:TextBox ID="txtDcrEstado" runat="server" CssClass="form-control" Enabled="false"  width="65%"/>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="txtCodEstado" />
                                <asp:PostBackTrigger ControlID="btnCfmEstado" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <br />

                        <div class="input-group">
                            <span class="input-group-addon">Descrição do Município : </span>
                            <asp:TextBox ID="txtDescricao" CssClass="form-control" name="txtDescricao" UseSubmitBehavior="false" TabIndex="3" runat="server" MaxLength="50" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnSalvar" />
                <asp:PostBackTrigger ControlID="btnExcluir" />
            </Triggers>

        </asp:UpdatePanel>
    </div>

    <!-- Large modal -->
    <div class="modal fade" id="myCadSisEstado"  data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:900;">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H1">Zoom de Estados</h4>
          </div>
          <div class="modal-body">
              <iframe id="iframe1" src="ConEstado.aspx" width='850' height='60%' frameborder='0'></iframe>
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnCfmEstado" runat="server" Text="Confirma" CssClass="btn btn-primary" AutoPostBack="true" OnClick="btnCfmEstado_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>

