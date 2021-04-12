<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadMenu.aspx.cs" Inherits="GestaoInterna.PagesAdmin.CadMenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<link type="text/css" href="~/Content/style.css" rel="stylesheet" />
<script src="../../Scripts/funcoes.js"></script>

    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <div  class="panel panel-primary" >
            <div class="panel-heading">Cadastro de Itens do Menu do Sistema</div>
            <div class="panel-body" style="max-height:430px;overflow-y: scroll;">


                <asp:Panel ID ="pnlExcluir" runat="server" Visible ="false" > 
                    <div class="modal-dialog" role="document" style ="height:30%;width:500" >
                        <div class="modal-content" >
                            <div class="modal-header"><h4 class="modal-title" id="H2">Atenção</h4></div>
                            <div class="modal-body">Deseja Excluir Item do Menu ?</div>
                            <div class="modal-footer">
                               <asp:Button ID="btnCfmSim" runat="server" Text="Sim" CssClass="btn btn-danger" OnClick="btnCfmSim_Click"></asp:Button>
                               <asp:Button ID="btnCfmNao" runat="server" Text="Não" CssClass="btn btn-default" OnClick="btnCfmNao_Click"></asp:Button>
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
                    <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Salvar
                </asp:LinkButton>

                <table>
                    <tr>
                        <td>Código do Menu: </td>
                        <td><asp:TextBox ID="txtCodMenu" runat="server" 
                            PlaceHolder="Informe Número" 
                            pattern="[0-9]+$" Width="100" 
                            onkeypress="return PermiteSomenteNumeros(event);"/> </td>
                    </tr>
                    <tr>
                        <td>Código do Módulo:  </td>
                        <td>
                            <asp:TextBox ID="txtCodModulo" 
                            runat="server" 
                            Width="100" 
                            PlaceHolder="Informe Número"  
                            pattern="[0-9]+$" 
                            TabIndex="1" 
                            AutoPostBack="true"  
                            OnTextChanged="txtCodModulo_TextChanged"
                            onkeypress="return PermiteSomenteNumeros(event);"
                            MaxLength="2" />

                            <button type="button" class="btn btn-primary" data-toggle="modal"  data-target="#myCadSisModulo">
                                <span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                            </button>

                            <asp:TextBox ID="txtDcrModulo" runat="server" Enabled="false" Width="805" />
                            <br/>
                        </td>
                    </tr>
                    <tr>
                        <td>Opção do Menu:  </td>
                        <td><asp:TextBox ID="txtNomeMenu" runat="server" 
                            Width="1000" PlaceHolder="Informe Texto"  TabIndex="2"/></td>
                    </tr>
                    <tr>
                        <td>Descrição do Menu:  </td>
                        <td><asp:TextBox ID="txtDcrMenu" runat="server" MaxLength="50" 
                            Width="1000" PlaceHolder="Informe Texto" TabIndex="3"/></td>
                    </tr>
                    <tr>
                        <td>Ordem de Apresentação:  </td>
                        <td><asp:TextBox ID="txtOrdApresentacao" runat="server" pattern="[0-9]+$" 
                            PlaceHolder="Informe Número"  Width="100" TabIndex="4" MaxLength="50" 
                            onkeypress="return PermiteSomenteNumeros(event);"/></td>
                    </tr>
                    <tr>
                        <td>Código do Menu Pai:  </td>
                        <td><asp:TextBox ID="txtCodPaiMenu" runat="server" MaxLength="4"  pattern="[0-9]+$" PlaceHolder="Informe Número" Width="100"  TabIndex="5" onkeypress="return PermiteSomenteNumeros(event);"/><br/></td>
                    </tr>
                    <tr>
                        <td>Url do Programa:  </td>
                        <td><asp:TextBox ID="txtUrlPrograma" runat="server" Width="1000" MaxLength="100"  PlaceHolder="Informe Texto" TabIndex="6"/></td>

                    </tr>
                    <tr>
                        <td>Url do Ícone:  </td>
                        <td><asp:TextBox ID="TxtUrlIcone" runat="server" Width="1000" MaxLength="100"  PlaceHolder="Informe Texto" TabIndex="6"/></td>
                    </tr>
                    <tr>
                        <td>Tipo de Formulário:  </td>
                        <td>
                            <asp:DropDownList id="ddlTipoFormulario" Runat="Server">
                                <asp:ListItem Text="Nenhum" Value="Nenhum"/>
                                <asp:ListItem Text="Cadastro" Value="Cadastro"/>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Ajuda:  </td>
                        <td><asp:TextBox ID="txtAjuda" runat="server" Width="1000px"  PlaceHolder="Informe Texto" TabIndex="7" Height="131px" TextMode="MultiLine"/></td>
                    </tr>

                </table>
                <br/>
                <br/>
            </div>
        </div>
    </div>

    <!-- Large modal -->
    <div class="modal fade" id="myCadSisModulo"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:900;">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H1">Zoom de Módulos do Sistema</h4>
          </div>
          <div class="modal-body">
              <iframe id="iframe1" src="ConModSistema2.aspx" width='850' height='60%' frameborder='0'></iframe>
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnConfirma" runat="server" Text="Confirma" CssClass="btn btn-primary"  OnClick="btnConfirma_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>
