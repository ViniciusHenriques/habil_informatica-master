<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadParSistema.aspx.cs" 
    Inherits="SoftHabilInformatica.Pages.Empresas.CadParSistema" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<link type="text/css" href="~/Content/style.css" rel="stylesheet" />
<script src="../../Scripts/funcoes.js"></script>
<script src="Scripts/jquery-2.1.4.js"></script>
<script src="../../Scripts/jsMensagemAlert.js"></script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>
        
        item = '#<%=PanelSelect%>';
        item2 = '#<%=PanelSelect2%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
            $('#myTabs2 a[href="' + item2 + '"]').tab('show');
            console.log($(item));
        });
        
        $(document).ready(function () {
            $('.js-example-basic-single').select2({
                
            });
        });

        
    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px">

        <div class="panel panel-primary">
            <div class="panel-heading">
                Manutenção de Parâmetros do Sistema
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
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


                <div class="row col-md-12" style="background-color:white;border:none;padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: x-small;">
                    <label for="usr" style ="margin-top:1px;">Empresa</label>
                    <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="true"  TabIndex="0" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                </div>

                <br />
                <br />


                <div class="col-md-12">
                <br />
                <br />
                    <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;">

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist" id="myTabs">
                            <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-edit"></span>&nbsp;&nbsp;
                                Geral</a></li>
                            <li role="presentation"><a href="#item1" aria-controls="profile" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;
                                Documentos Fiscais Eletrônicos - DF-e</a></li>
                             <li role="presentation"><a href="#item2" aria-controls="profile" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-picture"></span>&nbsp;&nbsp;
                                Design Sistema</a></li>
                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content" runat="server" id="PanelContext">

                            <div role="tabpanel" class="tab-pane col-md-12" id="home" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                <div class="row" style="background-color:white;border:none;">
                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Característica da Categoria : </label>
                                        <asp:TextBox ID="txtCaracCategoria" runat="server" CssClass="form-control" Font-Size ="Small" 
                                        PlaceHolder="Gerar formato com 9 ou Letra a" MaxLength="20" ></asp:TextBox>
                                    </div>

                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Característica da Localização : </label>
                                        <asp:TextBox ID="txtCaracLocalizacao" runat="server" CssClass="form-control"  Font-Size ="Small" 
                                        PlaceHolder="Gerar formato com 9 ou Letra a" MaxLength="20" ></asp:TextBox>
                                    </div>

                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small; top:20px;">
                                        <div id="divindicadores" style="background-color:black;border:solid 5px black; border-radius: 0px;padding-left:10px">
                                            <asp:checkBox ID="chkEspelhamento"   Text ="&emsp;Localização Espelhada na Categoria" CssClass="form-control-static" runat="server" 
                                            Enabled="true" ForeColor="White" Font-Size="Small" value="1" />&emsp;
                                        </div>
                                    </div>
                                </div>
                            </div>
 
                            <div role="tabpanel" class="tab-pane" id="item1" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                            
                                <div id="Tabs2" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;">

                                    <!-- Nav tabs -->
                                    <ul class="nav nav-tabs" role="tablist" id="myTabs2">
                                        <li role="presentation"><a href="#home2" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-wrench"></span>&nbsp;&nbsp;
                                            NFS-e</a></li>
                                        <li role="presentation"><a href="#item21" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-shopping-cart"></span>&nbsp;&nbsp;
                                            NF-e</a></li>
                                    </ul>

                                    <!-- Tab panes -->
                                    <div class="tab-content" runat="server" id="PanelContext2">

                                        <div role="tabpanel" class="tab-pane col-md-12" id="home2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                            <div class="row" style="background-color:white;border:none;">
                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Tipo Operação</label>
                                                    <asp:DropDownList ID="ddlTipoOperacao" runat="server"   TabIndex="6"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" />
                                                </div>

                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                </div>

                                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small; top:20px;">
                                                </div>
                                            </div>
                                        </div>
 
                                        <div role="tabpanel" class="tab-pane" id="item21" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                            
                                            Testes NF-e
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div role="tabpanel" class="tab-pane" id="item2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                            
                                <div id="Tabs3" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;">
                                    <div class=" col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:5px">
                                        <label for="usr" style ="margin-top:1px;">Cor Padrão</label>
                                        <asp:TextBox ID="txtCorPadrao" type="color" CssClass="form-control" runat="server" Text ="" Font-Size="Small" TabIndex="1" />
                                    </div>
                                    <div class=" col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:5px">
                                        <label for="usr" style ="margin-top:1px;">Cor de Fundo</label>
                                        <asp:TextBox ID="txtCorFundo" type="color" CssClass="form-control" runat="server" Text ="" Font-Size="Small" TabIndex="1" />
                                    </div>

                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:5px">
                                        <label for="usr" style ="margin-top:1px;">Tipo Menu</label>
                                        <asp:DropDownList ID="ddlTipoMenu" runat="server"   TabIndex="6"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small">
                                            <asp:ListItem Value="1" Text="Horizontal"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Vertical"></asp:ListItem>
                                        </asp:DropDownList>
                                        
                                    </div>
                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:5px" >
                                        <label for="usr" style ="margin-top:1px;">Logo da Empresa</label>                                     
                                        <asp:FileUpload ID="ArquivoImagemLogo" runat="server"  CssClass="form-control" Text=""  onchange="Javascript: VerificaTamanhoArquivo();" />    
                                        <asp:Image src="../../Images/LogoDaEmpresa.png" runat="server" Width="100%" Height="200" Style="border:1px solid black;margin-top:10px"></asp:Image>
                                    </div> 
                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:5px;" >
                                        <label for="usr" style ="margin-top:1px;">Papel de Parede</label>
                                        <asp:FileUpload ID="ArquivoImagemPapelParede" runat="server"  CssClass="form-control" Text=""  onchange="Javascript: VerificaTamanhoArquivo();" /> 
                                        <asp:Image src="../../Images/PapelDeParede.jpg" runat="server" Width="100%" Height="200" Style="border:1px solid black;margin-top:10px"></asp:Image>
                                    </div> 
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
                    
    

       <!-- Exclui Empresa -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir esta Empresa ?
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:Button>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>
