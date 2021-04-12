<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadTiposSituacao.aspx.cs" Inherits="GestaoInterna.PagesAdmin.CadTiposSituacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<link type="text/css" href="~/Content/style.css" rel="stylesheet" />
<script type="text/javascript" src="../../Scripts/funcoes.js"></script>
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
        <div  class="panel panel-primary" >
            <div class="panel-heading">Cadastro de Tipos e Situações</div>
            <div class="panel-body" >

                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>

                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>

                <button type="button" class="btn btn-danger" data-toggle="modal"  data-target="#myModal" onclick="">
                    <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                    Excluir
                </button>
                <br/> <br/>
                <div class="container-fluid"  style="background-color:white;border:none;">
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Código do Tipo ou Situação</label>
                            <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" runat="server" TabIndex="1" onkeypress="return PermiteSomenteNumeros(event);" MaxLength="4"/>
                        </div>
                        <br/><br/><br/><br/>
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Descrição do Tipo ou Situação</label>
                            <asp:TextBox ID="txtDescricao"  CssClass="form-control" name="txtDescricao" TabIndex="2" runat="server" MaxLength="50" />
                        </div>

                        <br/><br/><br/><br/>
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Tipo do Tipo</label>
                            <asp:DropDownList id="ddlTipoTipo" Runat="Server" CssClass="form-control">
                                <asp:ListItem Text = "Situação" Value="1"/>
                                <asp:ListItem Text = "Tipos" Value="2"/>
                            </asp:DropDownList>

                        </div>

                        <br/><br/><br/><br/>
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Observações</label>
                            <asp:TextBox ID="txtObs"  CssClass="form-control" name="txtObs" TabIndex="2" runat="server" MaxLength="100" />
                        </div>
                        
                    </div>
                
                </div>

            </div>
        </div>
    </div>

        <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="myModalLabel">Atenção</h4>
          </div>
          <div class="modal-body">
             Deseja Realmente Excluir este Tipo ou Situação ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="Button1" runat="server" Text="Excluir" UseSubmitBehavior="false" CssClass="btn btn-danger"  OnClick="btnExcluir_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

    <p>
        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black" BackColor="Red" Font-Size="Small" />
    </p>
</asp:Content>
