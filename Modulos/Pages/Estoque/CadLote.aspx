<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadLote.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.CadLote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
  
    <script src="../../Scripts/funcoes.js"></script>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>


    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />

    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtdtvalidade']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
            
        });
         $(document).ready(function () {
            $("input[id*='txtdtfabricacao']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
            
        });
    </script>






    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">

        <div class="panel panel-primary">
            <div class="panel-heading">
                Cadastro de Lote
                    <div class="messagealert" id="alert_container"></div>
                
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body">
                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click" TabIndex="11"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>

                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" TabIndex="12"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>

                <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" TabIndex="13"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                </asp:LinkButton>
                  <div class="row" style="margin-top: 10px">

                    <div class="col-md-3">
                        <label for="usr">Lançamento: </label>
                        <asp:TextBox ID="txtLancamento" CssClass="form-control" name="txtLancamento" Enabled="false" runat="server" TabIndex="1"  />
                    </div>
                     <div class="col-md-6">
                    <label for="usr"> Empresa: </label>
                               <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" CssClass="form-control" Font-Size="Small" TabIndex="2"> </asp:DropDownList>		   
                          </div> 
   
		                <div class="col-md-3">
                            <label for="usr"> Situação: </label>
                        <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="false"  CssClass="form-control " Font-Size="Small" TabIndex="3"> </asp:DropDownList>		   
                    </div> 
                 </div> 
                    
                <div class="row" style="margin-top: 10px">
                    <%-- <div class="col-md-6">
                            <label for="usr"> Lote: </label>
                         <asp:DropDownList ID="ddlLote" runat="server" AutoPostBack="false"  CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="7"> </asp:DropDownList>		   
                     </div>--%>
                  <div class="col-md-3">
                         <label for="usr"> Número Lote:  </label>
                             <asp:TextBox ID="txtNumero" CssClass="form-control" AutoPostBack="false" Text=""  Font-Size ="Medium" runat="server" TabIndex="4" MaxLength="20" />
                     </div>
                    
                   <div class="col-md-3">
                         <label for="usr"> Série Lote:  </label>
                             <asp:TextBox ID="txtSerie" CssClass="form-control" Text="" AutoPostBack="false"  Font-Size ="Medium" runat="server" TabIndex="5" MaxLength="10" />
                     </div>
                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                         <label for="usr" style ="margin-top:1px;">Data de Fabricação</label>
                         <asp:TextBox ID="txtdtfabricacao" CssClass="form-control"  TabIndex="7"  runat="server" name="txtdtvalidade"  Font-Size="Medium"/>
                     </div>
                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                         <label for="usr" style ="margin-top:1px;">Data de Validade</label>
                         <asp:TextBox ID="txtdtvalidade" CssClass="form-control"  TabIndex="6"  runat="server" name="txtdtvalidade"  Font-Size="Medium"/>
                     </div>

                      

                    </div>

                 <div class="row" style="margin-top: 10px">

                     <div class="col-md-12" >
                 <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Produto </label>
                 <div class="input-group " style="width:100%">                              
                     <asp:TextBox ID="txtProduto" CssClass="form-control"  runat="server" TabIndex="5" OnTextChanged="txtProduto_TextChanged"   AutoPostBack="true"                                
                     Width="25%" MaxLength="8" />                                                      
                     <asp:LinkButton ID="btnPesquisarItem"  runat="server" CssClass="form-control btn btn-primary" Width="10%" TabIndex="9" OnClick="btnPesquisarItem_Click" AutoPostBack="true"> 
                             <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                     </asp:LinkButton>
                     <asp:TextBox ID="txtDcrproduto" CssClass="form-control"  runat="server"  Enabled="false"  Width="65%"  />
                 </div>
             </div> 
                     
                </div> 
                     <div class="row" style="margin-top: 10px">
                                  <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                     <div class="col-md-6">
                         <label for="usr"> Quantidade:  </label>
                             <asp:TextBox ID="txtquantidade" CssClass="form-control" Text="0,00" AutoPostBack="true" Font-Size ="Medium" runat="server" TabIndex="10"  OnTextChanged="txtquantidade_TextChanged"/>
                     </div>
                     
                            </ContentTemplate> 
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtquantidade" EventName="TextChanged" />
                    </Triggers>
                 </asp:UpdatePanel>
             </div>
        </div>
   </div>         
</div>


   <!-- Exclui Lote -->
<div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:100%;width:300px">
            <div class="modal-content" >
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir este Lote?
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
