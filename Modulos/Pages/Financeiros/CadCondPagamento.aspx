<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadCondPagamento.aspx.cs" Inherits="SoftHabilInformatica.Pages.Financeiros.CadCondPagamento" MaintainScrollPositionOnPostback="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script> 
        $(document).ready(function() {
            $('.js-example-basic-single').select2();
        });

     </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div  class="panel panel-primary" >
                <div class="panel-heading">Cadastro de Condições de Pagamento
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
                   <br/><br />
                    <div class="input-group">
                        <span class="input-group-addon">Código da Forma de Pagamento : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; </span>
                        <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" Enabled="false" runat="server" TabIndex="1" 
                        onkeypress="return PermiteSomenteNumeros(event);" 
                        MaxLength="4"  />

                        <span class="input-group-addon">Situação : </span>
                        <asp:DropDownList ID="txtCodSituacao" runat="server" AutoPostBack="True"  CssClass="form-control" Font-Size ="Small"   >
                        </asp:DropDownList>

                    </div>
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon">Descrição da Forma de Pagamento : &nbsp;&nbsp;&nbsp; </span>
                        <asp:TextBox ID="txtDescricao"  CssClass="form-control" name="txtDescricao" TabIndex="2" runat="server" MaxLength="50" />
                    </div>
                    <br />

                    <div class="input-group">
                        <span class="input-group-addon">Tipo de Pagamento : 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                        </span>
                        
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="DdlTipoPagamento" runat="server"  AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlTipoPagamento_SelectedIndexChanged" > 
                                    
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="DdlTipoPagamento" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <span class="input-group-addon">Dia Fixo do Mês : &nbsp;&nbsp;&nbsp; </span>
                        <asp:TextBox ID="txtDiaFixo" runat="server"  CssClass="form-control" TabIndex="3" Enabled="false"  MaxLength="2" />
                    </div>
                    <br />


                    <div class="input-group">
                        <span class="input-group-addon">Forma de Pagamento : 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp

                        </span>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="DdlFormaPagamento" runat="server"  AutoPostBack="True" CssClass="form-control js-example-basic-single"  > 
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="DdlFormaPagamento" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon">Tipo de Cobrança : 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp

                        </span>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="DdlTipoCobranca" runat="server"  AutoPostBack="True" CssClass="form-control js-example-basic-single"  > 
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="DdlTipoCobranca" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon">Quantidade de Parcelas : 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                        </span>

                        <asp:LinkButton ID="btnMenos" Width="45" runat="server" Text="Salvar"  CssClass="form-control btn-success" UseSubmitBehavior="false" OnClick="btnMenos_Click"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-minus"></span>
                        </asp:LinkButton>

                        <asp:TextBox ID="txtQtdeParcelas" Width ="100"  CssClass="form-control" Style="text-align:center;"  name="txtQtdeParcelas" TabIndex="4" runat="server" MaxLength="2" />
                        
                        <asp:LinkButton ID="btnMais" Width="45" runat="server" Text="Salvar" CssClass="form-control btn-success" UseSubmitBehavior="false" OnClick="btnMais_Click"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-plus"></span>
                        </asp:LinkButton>
                    </div>
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon">Parcela 1 / % Proporção : </span>
                        <asp:TextBox ID="txtParc1"  CssClass="form-control" name="txtParc1" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged="txtParc1_TextChanged" />
                        <asp:TextBox ID="txtProp1"  CssClass="form-control" name="txtProp1" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp1_TextChanged" />
                        <span class="input-group-addon">Parcela 2 / % Proporção : </span>
                        <asp:TextBox ID="txtParc2"  CssClass="form-control" name="txtParc2" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged="txtParc2_TextChanged" />
                        <asp:TextBox ID="txtProp2"  CssClass="form-control" name="txtProp2" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp2_TextChanged" />
                        <span class="input-group-addon">Parcela 3 / % Proporção : </span>
                        <asp:TextBox ID="txtParc3"  CssClass="form-control" name="txtParc3" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged="txtParc3_TextChanged" />
                        <asp:TextBox ID="txtProp3"  CssClass="form-control" name="txtProp3" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp3_TextChanged" />
                        <span class="input-group-addon">Parcela 4 / % Proporção : </span>
                        <asp:TextBox ID="txtParc4"  CssClass="form-control" name="txtParc4" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged ="txtParc4_TextChanged" />
                        <asp:TextBox ID="txtProp4"  CssClass="form-control" name="txtProp4" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged ="txtProp4_TextChanged" />
                        <span class="input-group-addon">Parcela 5 / % Proporção : </span>
                        <asp:TextBox ID="txtParc5"  CssClass="form-control" name="txtParc5" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged ="txtParc5_TextChanged" />
                        <asp:TextBox ID="txtProp5"  CssClass="form-control" name="txtProp5" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp5_TextChanged" />
                    </div>
                    <div class="input-group">
                        <span class="input-group-addon">Parcela 6 / % Proporção : </span>
                        <asp:TextBox ID="txtParc6"  CssClass="form-control" name="txtParc6" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged="txtParc6_TextChanged" />
                        <asp:TextBox ID="txtProp6"  CssClass="form-control" name="txtProp6" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp6_TextChanged"/>
                        <span class="input-group-addon">Parcela 7 / % Proporção : </span>
                        <asp:TextBox ID="txtParc7"  CssClass="form-control" name="txtParc7" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged="txtParc7_TextChanged" />
                        <asp:TextBox ID="txtProp7"  CssClass="form-control" name="txtProp7" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp7_TextChanged" />
                        <span class="input-group-addon">Parcela 8 / % Proporção : </span>
                        <asp:TextBox ID="txtParc8"  CssClass="form-control" name="txtParc8" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged="txtParc8_TextChanged" />
                        <asp:TextBox ID="txtProp8"  CssClass="form-control" name="txtProp8" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp8_TextChanged" />
                        <span class="input-group-addon">Parcela 9 / % Proporção : </span>
                        <asp:TextBox ID="txtParc9"  CssClass="form-control" name="txtParc9" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged ="txtParc9_TextChanged" />
                        <asp:TextBox ID="txtProp9"  CssClass="form-control" name="txtProp9" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp9_TextChanged" />
                        <span class="input-group-addon">Parcela 10 / % Proporção : </span>
                        <asp:TextBox ID="txtParc10"  CssClass="form-control" name="txtParc10" Style="text-align:center;" TabIndex="5" runat="server" MaxLength="10" OnTextChanged="txtParc10_TextChanged" />
                        <asp:TextBox ID="txtProp10"  CssClass="form-control" name="txtProp10" Style="text-align:center;" TabIndex="6" runat="server" MaxLength="10" OnTextChanged="txtProp10_TextChanged" />
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

       <!-- Exclui Forma de Pagamento -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir esta Condição de Pagamento ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>
</asp:Content>
