<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadTipoDocumento.aspx.cs" Inherits="SoftHabilInformatica.Pages.Empresas.CadTipoDocumento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/funcoes.js"></script>

    <script type="text/javascript">
        function openModal() {
            $('#myModalMsg').modal('show');
        }
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div  class="panel panel-primary" >
                <div class="panel-heading">Cadastro de Tipos de Documento</div>
                <div class="panel-body" >
                    <asp:Panel ID ="pnlExcluir" runat="server" Visible ="false" > 
                        <div class="modal-dialog" role="document" style ="height:30%;width:500" >
                            <div class="modal-content" >
                                <div class="modal-header"><h4 class="modal-title" id="H2">Atenção</h4></div>
                                <div class="modal-body">Deseja Excluir Tipo de Documento ?</div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnCfmSim" runat="server" Text="Sim" CssClass="btn btn-danger" TabIndex="-1" UseSubmitBehavior="false"  OnClick="btnCfmSim_Click"></asp:Button>
                                    <asp:Button ID="btnCfmNao" runat="server" Text="Não" CssClass="btn btn-default" TabIndex="-1" UseSubmitBehavior="false"  OnClick="btnCfmNao_Click"></asp:Button>
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

                    <asp:LinkButton ID="btnMensagem" runat="server" Text="Mensagem"  Visible="false" class="btn btn-default" data-toggle="modal"  data-target="#myModalMsg" OnClick="btnMensagem_Click"> 
                    </asp:LinkButton>

                    <asp:Panel ID ="pnlMensagem" runat="server" Visible="false" >
                                <!-- Modal -->
                        <div class="modal fade" id="myModalMsg" role="dialog">
                            <div class="modal-dialog modal-sm" style="width:30%;position: fixed; top:30%; left:35%;">
                                <div class="modal-content">
                                    <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h5 class="modal-title" id="H3">Atenção</h5>
                                    </div>
                                    <div class="modal-body">
                                        <p><asp:Label ID ="lblMensagem" runat="server" Font-Size="Small"></asp:Label></p>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="Button1" runat="server" Text="Fechar" CssClass="btn btn-default" data-dismiss="modal" TabIndex="-1" UseSubmitBehavior="false"  OnClick="btnCfmNao_Click"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>


                   <br/><br/><br/>
                    <div class="input-group">
                        <span class="input-group-addon">Código do Tipo de Documento : &nbsp;&nbsp;&nbsp; </span>
                        <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" runat="server" TabIndex="1" 
                        onkeypress="return PermiteSomenteNumeros(event);" 
                        MaxLength="2"  />

                        <span class="input-group-addon">Situação : </span>
                        <asp:DropDownList ID="txtCodSituacao" runat="server" AutoPostBack="True"  CssClass="form-control" Font-Size ="Small"   >
                        </asp:DropDownList>
                    </div>
                
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon">Descrição do Tipo de Documento : </span>
                        <asp:TextBox ID="txtDescricao"  CssClass="form-control" name="txtDescricao" TabIndex="2" runat="server" MaxLength="50" />
                    </div>
                    
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon">Campo : </span>
                        <asp:DropDownList ID="DdlDigitavel" runat="server" AutoPostBack="True"  CssClass="form-control" Font-Size ="Small" OnSelectedIndexChanged="DdlDigitavel_SelectedIndexChanged"   >
                        </asp:DropDownList>

                        <span class="input-group-addon">Incremental por Empresa : </span>
                        <asp:DropDownList ID="DdlIncEmpresa" runat="server" AutoPostBack="True"  CssClass="form-control" Font-Size ="Small"   >
                        </asp:DropDownList>

                        <span class="input-group-addon">Abertura de Série : </span>
                        <asp:DropDownList ID="DdlAbeSerie" runat="server" AutoPostBack="True"  CssClass="form-control" Font-Size ="Small"   >
                        </asp:DropDownList>
                    </div>
                    
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon">Nome da Tabela : </span>
                        <asp:TextBox ID="tx_tabela"  CssClass="form-control" name="txtDescricao" TabIndex="2" runat="server" MaxLength="50" />
                    </div>
                <br /><br />

            </div>
            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnSalvar" />
                <asp:PostBackTrigger ControlID="btnExcluir" />
            </Triggers>

        </asp:UpdatePanel>
    </div>
</asp:Content>
