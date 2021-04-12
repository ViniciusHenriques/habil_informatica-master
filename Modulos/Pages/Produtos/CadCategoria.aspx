<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadCategoria.aspx.cs" Inherits="SoftHabilInformatica.Pages.Produtos.CadCategoria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    
    <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script src="../../Scripts/jquery.maskedinput.min.js"></script>


    <script type="text/javascript">
        item = '<%=MaskCategoria%>';

        jQuery(function ($) {
            $("#txtCodigo").mask(item);
        });

    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

            <div  class="panel panel-primary" >
                <div class="panel-heading">Cadastro de Categorias
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                            <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                        </div> 
                    </asp:Panel>
                </div>
                <div class="panel-body" style="font-size:x-small">

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
                    <div class="col-md-6">
                        <label for="usr">Lançamento: </label>
                        <asp:TextBox ID="txtLancamento" CssClass="form-control" name="txtLancamento" Enabled="false" runat="server" TabIndex="1"  />
                    </div>
                    <div class="col-md-6" >
                        <label for="usr">Código da Categoria: </label>
                        <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" Font-Size ="Medium" Font-Bold ="true"  ForeColor="Blue"  runat="server" TabIndex="2" ClientIDMode="Static" />
                    </div>
                    <div class="col-md-6">
                        <label for="usr">Descrição da Categoria: </label>
                        <asp:TextBox ID="txtDescricao"  CssClass="form-control" name="txtDescricao" TabIndex="3" runat="server"  MaxLength="50" />
                    </div>

                    <div class="col-md-6">
                        <label for="usr">Departamento: </label>
                        <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small"> </asp:DropDownList>
                    </div>
                    
                    <div class="col-md-6">
                        <label for="usr">Grupo Comissão: </label>
                        <asp:DropDownList ID="ddlGpoComissao" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small"> </asp:DropDownList>
                    </div>

                </div>
            </div>
    </div>

       <!-- Exclui Categoria -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:100%;width:300px">
            <div class="modal-content" >
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir esta Categoria ?
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
