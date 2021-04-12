<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadStrDataBase.aspx.cs" Inherits="GestaoInterna.PagesAdmin.CadStrDataBase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="../../Content/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap-datepicker.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtToDoDate']").datepicker({ format: "dd/mm/yyyy" });
        });
    </script>
    
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

            <div  class="panel panel-primary" >
                <div class="panel-heading">Cadastro/Geração de Estruturas do Banco de Dados
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                        </div> 
                    </asp:Panel>

                </div>
                <div class="panel-body">

                    <asp:LinkButton ID="btnSair" runat="server" Text="Salvar" CssClass="btn btn-primary" UseSubmitBehavior="false" OnClick="btnSair_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-close"></span>  Fechar
                    </asp:LinkButton>
                    <br/><br/><br/>
                    <div class="container-fluid"  style="background-color:white;border:none;">
                            <div class="panel panel-default" style ="background-color:white;">
                                <div class="panel-heading">Geração</div>
                                <div class="panel-body">

                                <label for="usr" style ="margin-top:1px;">Cliente: </label>
                                <asp:DropDownList ID="DdlCliente" runat="server"  AutoPostBack="True" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="DdlCliente_SelectedIndexChanged1"> 
                                </asp:DropDownList>

                                <asp:LinkButton ID="btnBuscarCliente" runat="server" Text="Salvar" CssClass="btn btn-default" UseSubmitBehavior="false" OnClick="btnBuscarCliente_Click"> 
                                    <span aria-hidden="true" title="Buscar Dados Cliente" class="glyphicon glyphicon-clean"></span>  Buscar Dados Cliente
                                </asp:LinkButton>

                                <label for="usr" style ="margin-top:1px;">Atualização do Banco: </label>
                                <asp:DropDownList ID="DdlAtuBanco" runat="server"  AutoPostBack="True" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="DdlAtuBanco_SelectedIndexChanged"> 
                                </asp:DropDownList>

                                    <asp:LinkButton ID="btnGerar" runat="server" Text="Salvar" CssClass="btn btn-warning" UseSubmitBehavior="false" OnClick="btnGerar_Click"> 
                                        <span aria-hidden="true" title="Gerar Arquivo" class="glyphicon glyphicon-save"></span>  Gerar
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="panel panel-default" style ="background-color:white;">
                                <div class="panel-heading">Cadastro</div>
                                <div class="panel-body">

                                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnLimpar" runat="server" Text="Salvar" CssClass="btn btn-default" UseSubmitBehavior="false" OnClick="btnLimpar_Click"> 
                                        <span aria-hidden="true" title="Limpar Dados" class="glyphicon glyphicon-clean"></span>  Limpar Dados
                                    </asp:LinkButton>
                                    <br/><br/>

                                    <label for="usr" style ="margin-top:1px;">Descrição: </label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDescricao" Width ="1000" />

                                    <br/><br/>
                                    <label for="usr" style ="margin-top:1px;">Conteúdo do Script: </label>
                                    <asp:TextBox runat="server" CssClass="form-control" TextMode="MultiLine" ID="txtConteudo" Height="1000" />

                                </div>
                            </div>

                    </div>
              </div>
            </div>
    </div>

</asp:Content>
