<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="CadPessoa.aspx.cs" Inherits="SoftHabilInformatica.Pages.Pessoas.CadPessoa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {

            $('#myTabs a[href="'+ item +'"]').tab('show');            

            console.log($(item));
        });

        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {          
            $('.js-example-basic-single').select2({});           
        }
    </script>
    <style type="text/css">
        @media screen and (max-width: 600px) {
            .btn-success{
                margin-top:5px;
                margin-bottom:5px;
            }         
        }
         @media screen and (max-width: 800px) {

            .noprint{display:none;}    
            
        }
         
    </style>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;" >
        <div class="panel panel-primary">
            <div class="panel-heading">Cadastro de Pessoas
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                    <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                    </div> 
                </asp:Panel>
            </div>
            <div class="panel-body">
                <div class="col-md-10" style="margin-left:0!important;padding-left:0!important">
                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click"> 
                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>
                    <br/><br/>
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-2 col-xs-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Código Pessoa</label>
                            <asp:TextBox ID="txtCodPessoa" runat="server" CssClass="form-control"  Font-Size ="Small"  ></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-xs-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <label for="usr" style ="margin-top:1px;"><Asp:label runat="server" ID="lblDsSisAnterior" >Cód. Sistema Anterior</Asp:label></label>
                                    <asp:TextBox ID="txtCodSisAnterior" runat="server" CssClass="form-control"  Font-Size ="Small" MaxLength="20" ></asp:TextBox>
                                </ContentTemplate> 
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-2 col-xs-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Data Cadastro</label>
                            <asp:TextBox ID="txtDataCadastro" runat="server" Enabled="false" CssClass="form-control"  Font-Size ="Small"  ></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-xs-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Data Atualização</label>
                            <asp:TextBox ID="txtDataAtualizacao" runat="server" Enabled="false" CssClass="form-control"  Font-Size ="Small"  ></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-xs-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Situação</label>
                            <asp:DropDownList ID="txtCodSituacao" runat="server" AutoPostBack="True"  CssClass="form-control js-example-basic-single"  Font-Size ="Small"   >
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2 col-xs-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Etapas do Cadastro</label>
                            <asp:DropDownList ID="txtCodFase" runat="server" AutoPostBack="True"  CssClass="form-control js-example-basic-single"  Font-Size ="Small"  >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br/>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Razão Social / Nome Pessoa</label>
                                    <asp:TextBox ID="txtRazSocial" runat="server" CssClass="form-control col-md-7"  Font-Size ="Small" AutoPostBack ="true"  OnTextChanged ="txtRazSocial_TextChanged" MaxLength="100"></asp:TextBox>
                                </div>
                                <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Nome Fantasia / Apelido</label>
                                    <asp:TextBox ID="txtNomeFantasia" runat="server" CssClass="form-control col-md-4"  Font-Size ="Small" MaxLength="100"></asp:TextBox>
                                </div>
                            </div>
                            </ContentTemplate> 
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtRazSocial" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="col-md-2" style="margin-top:5px!important">
                    <div class="row " style="border:none;">
                        <div class="col-md-12 " style="border:none;text-align:left;font-size:x-small;">
                            <div id="divindicadores" class="CorPadraoEscolhida CorPadraoEscolhidaBorder" style=" border-radius: 10px;padding:10px">
                                <a style="font-size:small; font-weight:bold" class="CorPadraoEscolhidaTexto">&emsp;Papéis da Pessoa: &emsp;</a>
                                <br/>
                                <asp:checkBox ID="chkEmpresa"   Text ="&emsp;Empresa" CssClass="form-control-static CorPadraoEscolhidaTexto" runat="server" Enabled="true" ForeColor="White" Font-Size="Small" value="1" /><br/>
                                <asp:checkBox ID="chkUsuario" Text ="&emsp;Usuário" CssClass="form-control-static CorPadraoEscolhidaTexto" runat="server" Enabled="true" ForeColor="White" Font-Size="Small" value="5"/><br/>
                                <asp:checkBox ID="chkVendedor" Text ="&emsp;Vendedor" CssClass="form-control-static CorPadraoEscolhidaTexto" runat="server" Enabled="false" ForeColor="White" Font-Size="Small" value="6"/><br/>
                                <asp:checkBox ID="chkComprador" Text ="&emsp;Comprador" CssClass="form-control-static CorPadraoEscolhidaTexto" runat="server" Enabled="false" ForeColor="White" Font-Size="Small" value="7"/><br/>
                                <asp:checkBox ID="chkCliente" Text ="&emsp;Cliente" CssClass="form-control-static CorPadraoEscolhidaTexto" runat="server" Enabled="true" ForeColor="White" Font-Size="Small" value="2"/><br/>
                                <asp:checkBox ID="chkFornecedor" Text ="&emsp;Fornecedor" CssClass="form-control-static CorPadraoEscolhidaTexto" runat="server" Enabled="true"  ForeColor="White" Font-Size="Small" value="3"/><br/>
                                <asp:checkBox ID="chkTransportador" Text ="&emsp;Transportador" CssClass="form-control-static CorPadraoEscolhidaTexto" runat="server" Enabled="true" ForeColor="White" Font-Size="Small" value="4"/><br/>

                                <%--&emsp;--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <asp:HiddenField ID="TabName" runat="server" />
                
                <div id="Tabs" role="tabpanel">

                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                        <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-user"></span>&nbsp;&nbsp;Inscrições (CNPJ/CPF)</a></li>
                        <li role="presentation"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Endereços</a></li>
                        <li role="presentation"><a href="#contact" aria-controls="contact" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-phone"></span>&nbsp;&nbsp;Contatos</a></li>
                        <li role="presentation"><a href="#parameter" aria-controls="parameter" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-check"></span>&nbsp;&nbsp;Parâmetros</a></li>
                    </ul>
                    <!-- Tab panes -->
                        
                    <div class="tab-content" runat="server" id="PanelContext">
                        <div role="tabpanel" class="tab-pane " id="home" >
                            <br/>
                            &emsp;
                            <asp:LinkButton ID="btnNovInscricao" runat="server"  Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovInscricao_Click"> 
                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span> Nova Inscrição
                            </asp:LinkButton>
                            <br />
                            <br />

                            <asp:GridView ID="grdInscricao" runat="server"
                                    CssClass="table table-bordered table-hover table-striped"
                                    GridLines="None" AutoGenerateColumns="False"
                                    Font-Size="8pt"  Visible="true"
                                    OnSelectedIndexChanged="grdInscricao_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="_CodigoItem" HeaderText="Item" />
                                    <asp:BoundField DataField="_TipoInscricaoD" HeaderText="Tipo"  />
                                    <asp:BoundField DataField="_NumeroInscricao" HeaderText="CNPJ / CPF" />
                                    <asp:BoundField DataField="_NumeroIERG" HeaderText="Inscr. Estadual / RG" />
                                    <asp:BoundField DataField="_NumeroIM" HeaderText="Inscr. Municipal" />
                                    <asp:BoundField DataField="_DataDeAbertura" HeaderText="Data de Abertura" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_DataDeEncerramento" HeaderText="Data de Encerramento" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_OBS" HeaderText="Observações" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>
                        </div>

                        <div role="tabpanel" class="tab-pane" id="profile" >
                            <br/>
                            &emsp;
                            <asp:LinkButton ID="BtnIncEndereco" runat="server" Text="Novo" CssClass="btn btn-info" OnClick="BtnIncEndereco_Click"> 
                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span> Novo Endereço
                            </asp:LinkButton>
                            <br />
                            <br />
                            <asp:GridView ID="grdEndereco" runat="server"
                                    CssClass="table table-bordered table-hover table-striped"
                                    GridLines="None" AutoGenerateColumns="False"
                                    Font-Size="8pt" Visible="true"
                                    OnSelectedIndexChanged="grdEndereco_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="_CodigoItem" HeaderText="Item" />
                                    <asp:BoundField DataField="_TipoEnderecoD" HeaderText="Tipo"  />
                                    <asp:BoundField DataField="_CodigoInscricao" HeaderText="ID Inscrição"  />
                                    <asp:BoundField DataField="_Logradouro" HeaderText="Endereço" />
                                    <asp:BoundField DataField="_NumeroLogradouro" HeaderText="Número" />
                                    <asp:BoundField DataField="_Complemento" HeaderText="Complemento" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_CodigoCEP" HeaderText="CEP" />
                                    <asp:BoundField DataField="_DescricaoEstado" HeaderText="Estado" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_DescricaoMunicipio" HeaderText="Município" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_DescricaoBairro" HeaderText="Bairro" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>
                        </div>
 
                        <div role="tabpanel" class="tab-pane" id="contact">
                            <br/>
                            &emsp;
                                
                            <asp:LinkButton ID="BtnIncContato" runat="server" Text="Novo" CssClass="btn btn-info" OnClick="BtnIncContato_Click"> 
                            <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span> Novo Contato
                            </asp:LinkButton>
                            
                            <br />
                            <br />
                                
                            <asp:GridView ID="grdContato" runat="server"
                                CssClass="table table-bordered table-hover table-striped"
                                GridLines="None" AutoGenerateColumns="False"
                                Font-Size="8pt"  Visible="true"
                                OnSelectedIndexChanged="grdContato_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="_CodigoItem" HeaderText="Item" />
                                    <asp:BoundField DataField="_TipoContatoD" HeaderText="Tipo de Contato" />
                                    <asp:BoundField DataField="_CodigoInscricao" HeaderText="ID Inscrição"  />
                                    <asp:BoundField DataField="_NomeContato" HeaderText="Nome do Contato" />
                                    <asp:BoundField DataField="_Fone1" HeaderText="Fone Principal" />
                                    <asp:BoundField DataField="_Fone2" HeaderText="Fone Secundário" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_Fone3" HeaderText="Fone Recado" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_MailNFE" HeaderText="E-Mail NF-e" />
                                    <asp:BoundField DataField="_MailNFSE" HeaderText="E-Mail NFS-e" />
                                    <asp:BoundField DataField="_Mail1" HeaderText="E-Mail Principal" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_Mail2" HeaderText="E-mail Secuntário" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:BoundField DataField="_Mail3" HeaderText="E-mail Recado" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>

                        </div>

                        <div role="tabpanel" class="tab-pane " id="parameter">
                            <div class="container-fluid">
                                <div class="row" style="background-color:white;border:none;">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <br />
                                            <div class="row" >
                                                <div class="col-md-4" style="font-size:x-small;"  >
                                                    <label for="usr">Grupo de Pessoa: </label>
                                                    <asp:DropDownList ID="ddlGpoPessoa" runat="server" TabIndex="2" CssClass="form-control js-example-basic-single" Width="100%" Height="33" OnSelectedIndexChanged="ddlGpoPessoa_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-1" style=" font-size: x-small; margin-top:19px">
                                                    <asp:LinkButton ID="BtnAddGpoPessoa" runat="server" Text="Adicionar" TabIndex="10" CssClass="btn btn-success" OnClick="BtnAddGpoPessoa_Click" style="height:33px!important;padding-top:7px" title="Clique para adicionar um novo grupo de Pessoa"> 
                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="row" style=" font-size: x-small; margin-top:19px">
                                                <div class="col-md-12" >
                                                    <div class="row" style=" margin-bottom:10px">
                                                        <div class="col-md-6">
                                                            <asp:Label for="usr" runat="server" ID="lblGerSeq" Style="font-weight: bold">Representantes  <span style='color:red'>(Clique no [+] para adicionar)<span></asp:Label>
                                                            <asp:DropDownList ID="ddlRepresentantes" runat="server" TabIndex="8" CssClass="form-control js-example-basic-single" Width="100%" AutoPostBack="false"></asp:DropDownList>
                                                        </div>

                                                        <div class="col-md-1" style="margin-top: 13px">
                                                            <asp:LinkButton ID="BtnAddRepresentante" runat="server" Text="Adicionar" TabIndex="9" CssClass="btn btn-success" OnClick="BtnAddRepresentante_Click" Style="height: 33px" title="Adicionar representante a pessoa" > 
                                                                <span aria-hidden="true" class="glyphicon glyphicon-plus"></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <asp:GridView ID="grdRepresentantes" runat="server"
                                                        CssClass="table  table-hover table-striped"
                                                        GridLines="None" AutoGenerateColumns="False"
                                                        Font-Size="8pt" Visible="true"
                                                        OnSelectedIndexChanged="grdRepresentantes_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:BoundField DataField="CodigoVendedor" HeaderText="Cod. Vendedor" />
                                                            <asp:BoundField DataField="NomePessoa" HeaderText="Nome Pessoa" />
                                                            <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                                                ItemStyle-Height="15px" ItemStyle-Width="50px"
                                                                ButtonType="Image" SelectImageUrl="~/Images/Excluir.png"
                                                                ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                                        </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                                                    </asp:GridView>
                                                </div>
                                            </div> 
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="grdRepresentantes" EventName="SelectedIndexChanged"/>
                                            <asp:AsyncPostBackTrigger ControlID="ddlGpoPessoa" EventName="SelectedIndexChanged"/>
                                            <asp:AsyncPostBackTrigger ControlID="BtnAddRepresentante" EventName="Click"/>
                                            <asp:PostBackTrigger ControlID="BtnAddGpoPessoa" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div> 
    </div>

       <!--  Pessoa -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
      <div class="modal-dialog" role="document" style ="height:100%;width:300px">
        <div class="modal-content" >
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="H3">Atenção</h4>
          </div>
          <div class="modal-body">
              Deseja Excluir esta Pessoa ?
          </div>
          <div class="modal-footer">
            <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir " CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:LinkButton>
            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>
