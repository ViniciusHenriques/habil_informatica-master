<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadPermissao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Usuarios.CadPermissao"  MaintainScrollPositionOnPostback="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script> 
         $(document).ready(function() {
            $('.js-example-basic-single').select2();
        });
         var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
             $('.js-example-basic-single').select2({
                language: $.extend({},
                $.fn.select2.defaults.defaults.language, {
                  noResults: function() {
                    var term = id_categoria
                      .data('select2')
                      .$dropdown.find("input[type='search']")
                      .val();

                    return $("Nenhum resultado");
                  }
                })
            });

        }

     </script>
        <style type="text/css">
        
        @media screen and (max-width: 1100px) {

            .noprint{display:none;}    
            
            
        }

    </style>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    
    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px" resource="Javascript: resolucao(); " onchange="Javascript: resolucao();" >

        <asp:LinkButton ID="btnMensagem" runat="server" Text="Mensagem" class="btn btn-default" data-toggle="modal" Visible="false" data-target="#myModalMsg" OnClick="btnMensagem_Click"> 
        </asp:LinkButton>

        <asp:Panel ID ="pnlMensagem" runat="server" Visible="false" >
                <!-- Modal -->
            <div class="modal fade" id="myModalMsg" role="dialog" >
                <div class="modal-dialog modal-sm" style="width:30%;position: fixed; top:30%; left:35%;">
                    <div class="modal-content">
                        <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h5 class="modal-title" id="H1">Atenção</h5>
                        </div>
                        <div class="modal-body">
                            <p><asp:Label ID ="lblMensagem" runat="server" Font-Size="Small"></asp:Label></p>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCfmNao2" runat="server" Text="Fechar" CssClass="btn btn-default" data-dismiss="modal" TabIndex="-1" UseSubmitBehavior="false"  OnClick="btnCfmNao2_Click"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>


        <div class="panel panel-primary">
            <div class="panel-heading">Permissões do Usuário
                <div class="messagealert" id="alert_container"></div>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
 
                        <div class="input-group" >
                            <span class="input-group-addon">Cód. do Módulo:</span>
                            <asp:DropDownList ID="DdlModulo" runat="server"  AutoPostBack="True" CssClass="js-example-basic-single "  Width="100%"
                                OnSelectedIndexChanged="DdlModulo_SelectedIndexChanged"> 
                            </asp:DropDownList>
                        </div>
                        <br />
                        <div class="input-group">
                            <span class="input-group-addon">Cód. do Perfil: &nbsp;&nbsp;</span>
                            <asp:TextBox ID="txtCodPerfil"
                                runat="server"
                                Width="12%"
                                PlaceHolder="Informe Número"
                                pattern="[0-9]+$"
                                TabIndex="4"
                                AutoPostBack="true"
                                OnTextChanged="txtCodPerfil_TextChanged"
                                onkeypress="return PermiteSomenteNumeros(event);"
                                MaxLength="2" CssClass="form-control" 
                                />

                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="form-control btn btn-primary" Width="15%" TabIndex="-1" data-toggle="modal" data-target="#myCadSisPerfil"> 
                                    <span aria-hidden="true" title="Perfil" class="glyphicon glyphicon-search"></span>
                            </asp:LinkButton>
                            <asp:TextBox ID="txtDcrPerfil" runat="server" CssClass="form-control" Enabled="false" Width="73%" />

                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtCodPerfil" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="LinkButton2" />
                        <asp:AsyncPostBackTrigger ControlID="DdlModulo" EventName="TextChanged" />

                    </Triggers>
                </asp:UpdatePanel>

                <br />

                <asp:Panel ID="pnlHistorico" runat="server" Visible="false">
                    <div class="modal-dialog" role="document" style="height:100%; width:900px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title" id="H2">Histórico do Item da Permissão</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="lblHistorico" runat="server" Height="131px" TextMode="MultiLine"></asp:Label>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="Button1" runat="server" Text="Fechar" CssClass="btn btn-default" OnClick="Button1_Click"></asp:Button>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlConsulta" runat="server">
                    <div class="container-fluid">
                            <div class="row">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                     <ContentTemplate>
                                        <asp:LinkButton ID="btnBuscar" runat="server" Text="Buscar Permissões" CssClass="btn btn-primary " UseSubmitBehavior="false" OnClick="btnBuscar_Click"  > 
                                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-refresh"></span>  Buscar Permissões
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar Permissões" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar Permissões
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info " OnClick="btnSair_Click">
                                            <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                                        </asp:LinkButton>
                                     </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click"/>
                                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"/>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                    </div>                               

                    <br />
                    
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div  style="margin-bottom:10px;margin-left:2px;">
                            <asp:CheckBox runat="server"  id="chkAll"  OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" Visible="false"  />
                            <asp:Label runat="server" ID="lblChkAll" Visible="false" Font-Size="Small"><b>Acesso a Todos</b></asp:Label>
                        </div>
                        <div  style="margin-bottom:10px;margin-left:2px;">
                            <asp:CheckBox runat="server"  id="chkAcessoAll"  OnCheckedChanged="chkAcessoAll_CheckedChanged" AutoPostBack="true" Visible="false"  />
                            <asp:Label runat="server" ID="lblAcessoAll" Visible="false" Font-Size="Small"><b>Acesso Completo a todos</b></asp:Label>
                        </div>
                        <asp:GridView ID="grdPermissao" runat="server"
                            OnRowDataBound="grdPermissao_RowDataBound"
                            Width="100%"
                            CssClass="table table-bordered table-striped"
                            GridLines="None" AutoGenerateColumns="False"
                            Font-Size="8pt" 
                            OnSelectedIndexChanged="grdPermissao_SelectedIndexChanged" >
                            <Columns>
                                <asp:TemplateField HeaderText="Acesso">
                                    <ItemTemplate >
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                            
                                            <ContentTemplate>
                                                <asp:CheckBox ID="chkLiberado" runat="server" AutoPostBack="true" OnCheckedChanged="chkLiberado_CheckedChanged" Checked='<%# Eval("Liberado").ToString().Equals("True") %>' />
                                            </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkLiberado" EventName="CheckedChanged"/>
                                        </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="CodigoItem" HeaderText="Item" />

                                <asp:BoundField DataField="DescricaoDoMenu" HeaderText="Descrição do Menu" HeaderStyle-Width ="200%" />

                                <asp:BoundField DataField="TipoFormulario" HeaderText="Tipo de Formulário" />

                                <asp:TemplateField HeaderText="Acesso Completo" >
                                    <ItemTemplate >
                                        <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:CheckBox ID="chkAcessoCompleto"  runat="server" AutoPostBack="true" OnCheckedChanged="chkAcessoCompleto_CheckedChanged" Checked='<%# Eval("AcessoCompleto").ToString().Equals("True") %>' />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="chkAcessoCompleto" EventName="CheckedChanged"/>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Consulta" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkConsulta" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoConsulta").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Relatório" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRelatorio" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoRelatorio").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Imprimir" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkImprimir" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoImprimir").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Incluir" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIncluir" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoIncluir").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Alterar" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAlterar" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoAlterar").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Excluir" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkExcluir" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoExcluir").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Download Arquivos" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDownload" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoDownload").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Upload Arquivos" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkUpload" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoUpload").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Exclusão Arquivos" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkExclusaoAnexo" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoExclusaoAnexo").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Esp 1" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEspecial1" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoEspecial1").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Esp 2" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEspecial2" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoEspecial2").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Esp 3" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEspecial3" runat="server" Enabled='<%# Eval("TipoFormulario").ToString().Equals("Cadastro") %>' Checked='<%# Eval("AcessoEspecial3").ToString().Equals("True") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:CommandField HeaderText="Hist." ShowSelectButton="True" 
                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                        ControlStyle-Width ="20px" ControlStyle-Height ="20px" />

                            </Columns>
                            <RowStyle CssClass="cursor-pointer" />
                        </asp:GridView>

                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="grdPermissao" />
                            <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged"/>
                            <asp:AsyncPostBackTrigger ControlID="chkAcessoAll" EventName="CheckedChanged"/>
                        </Triggers>
                    </asp:UpdatePanel>


                </asp:Panel>
                
            </div>
        </div>
        


    </div> 

     <!-- Large modal -->
    <div class="modal fade" id="myCadSisPerfil"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:80%">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Zoom de Perfis de Usuários</h4>
          </div>
          <div class="modal-body">
              <iframe id="iframe2" src="ConPflUsuario2.aspx" width="100%" height='60%' frameborder='0'></iframe>
          </div>
          <div class="modal-footer">
            <asp:Button ID="Button2" runat="server" Text="Confirma" CssClass="btn btn-primary"  AutoPostBack="true"   OnClick="btnConfirma_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>


</asp:Content>

