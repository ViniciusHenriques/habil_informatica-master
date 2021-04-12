<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManGerEmails.aspx.cs" Inherits="SoftHabilInformatica.Pages.HabilUtilitarios.ManGerEmails"  
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server" >

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('.js-example-basic-single').select2({
               
            });
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
        @media screen and (max-width: 600px) {
            .noprint{display:none;}  
        
        }
        @media screen and (max-width: 1000px) {
            .acao{
                margin-top:20px!important;
                
            }  
              
            #consulta3,#consulta1,#consulta2{
                padding-left:0px!important;
            }
            
        }
        .valor{
            border-left:0!important;
        }
         .centerHeaderText{
            text-align: center!important;
        }
    </style>
    

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary">
            <div class="panel-heading" >

                Manutenção do Gerador de E-mail
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>

            </div>
            <div class="panel-body" >
                <div class="row" style="background-color: white; border: none;">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: small;">
                                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltar_Click" > 
                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>&nbsp;Voltar  
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>&nbsp;Salvar  
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnEncaminhar" runat="server" Text="Salvar" CssClass="btn btn-info"  UseSubmitBehavior="false" OnClick="btnEncaminhar_Click"> 
                                    <span aria-hidden="true" class="glyphicon glyphicon-repeat"></span>&nbsp;Encaminhar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-warning" UseSubmitBehavior="false" OnClick="btnEnviar_Click"> 
                                    <span aria-hidden="true" class="glyphicon glyphicon-envelope"></span>&nbsp;Enviar  
                                </asp:LinkButton>
                            </div>
                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Lançamento</label>
                                <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small"/>
                            </div>
                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                <label for="usr" style ="margin-top:-1px;" class="acao">Data Lançamento</label>
                                <asp:TextBox ID="txtDataLancamento" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small"/>
                            </div>
                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                <label for="usr" style ="margin-top:-1px;" class="acao">Situação</label>
                                <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="True" Enabled ="false"  CssClass="form-control js-example-basic-single" Font-Size="Small">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                <label for="usr" style ="margin-top:-1px;" class="acao">Data Envio</label>
                                <asp:TextBox ID="txtDataEnvio" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" />
                            </div>
                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                <label for="usr" style ="margin-top:-1px;" class="acao">Tentativas Envio</label>
                                <asp:TextBox ID="txtTentaEnvio" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" />
                            </div>
                        </ContentTemplate> 
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSalvar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <br/>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="panel panel-default">                 
                            <div class="panel-heading" >
                                <asp:LinkButton runat="server" title="Editar Contatos" ID="blnContatos" OnClick="blnContatos_Click" CssClass="btn btn-info" style="height:33px; margin-right:20px;float:left;margin-top:-8px" TabIndex="17">
                                    <span class="glyphicon glyphicon-user"></span>
                                    <span class="glyphicon glyphicon-user"></span>
                                    <span class="glyphicon glyphicon-user"></span>
                                </asp:LinkButton>                                  
                                <asp:LinkButton runat="server" title="Anexos" ID="btnAnexo" OnClick="btnAnexo_Click" CssClass="btn btn-info" style="height:33px; margin-right:20px;float:left;margin-top:-8px" TabIndex="17">
                                    <span class="glyphicon glyphicon-paperclip"></span>
                                </asp:LinkButton>                                  
                                Contatos e Anexos do Usuário - Endereços Eletrônicos
                            </div>
    
                            <div class="panel-body" >
                                <asp:Panel ID="pnlVerificar" runat="server">
                                    
                                    <div class="row" style="background-color: white; border: none;">
                                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Para:</label>
                                            <asp:TextBox ID="txtDestinatarioPara" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="2" />

                                        </div>
                                    </div>
                                    
                                    <div class="row" style="background-color: white; border: none;">
                                        
                                        <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Cópia:</label>
                                            <asp:TextBox ID="txtDestinatarioCopia" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="2"/>
                                        </div>
                                    
                                        <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Cópia Oculta:</label>
                                            <asp:TextBox ID="txtDestinatarioCopiaOculta" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="2"/>
                                        </div>
                                    </div>

                                </asp:Panel>

                                <asp:Panel ID="pnlNovo" runat="server" Visible ="false" >
                                    <div class="row" style="background-color: white; border: none;">
                                        
                                        <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Nome:</label>
                                            <asp:TextBox ID="txtNomeContato" CssClass="form-control" runat="server" Enabled="true" Text ="" Font-Size="Small" TabIndex="2"/>
                                        </div>
                                        
                                        <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Email:</label>
                                            <asp:TextBox ID="txtEmailContato" CssClass="form-control" runat="server" Enabled="true" Text ="" Font-Size="Small" TabIndex="2"/>
                                        </div>
                                        
                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                <label for="usr" style ="margin-top:1px;">Ação</label>
                                            <br/>
                                                <asp:LinkButton ID="btnSalvarContato" runat="server" Text="Salvar Contato" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false" Height ="33"  OnClick="btnSalvarContato_Click"> 
                                                    <span aria-hidden="true" class="glyphicon glyphicon-save"></span>  
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnVoltarContato" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltarContato_Click" Height ="33"> 
                                                    <span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span>  
                                                </asp:LinkButton>

                                        </div>

                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="pnlAlterar" runat="server" Visible ="false" >
                                    <div class="row" style="background-color: white; border: none;">
                                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr" style ="margin-top:1px;">Busca Contato:</label>
                                            <asp:DropDownList ID="ddlContato" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single" TabIndex="2" Font-Size="Small" Width ="70%">
                                            </asp:DropDownList>
                                            <asp:LinkButton ID="btnAddContato" runat="server" Text="Adicionar" CssClass="btn btn-success" TabIndex="12" OnClick="btnAddContato_Click" Height="33"> 
                                                <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                            </asp:LinkButton>

                                        </div>
                                    </div>

                                    <br/>
                                    <div class="row" style="background-color: white; border: none;">
                                        <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <label for="usr" style ="margin-top:1px;">Para:</label>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="BtnPara" runat="server" Text="Adicionar" CssClass="btn btn-success" TabIndex="12" OnClick="BtnPara_Click" Height="33"> 
                                                            <span aria-hidden="true" title="Adicionar Grupo Tributação Pessoa" class="glyphicon glyphicon-plus"></span>
                                                            </asp:LinkButton>
                                                            <br />
                                                            <br />
                                                            <asp:GridView ID="GrdPara" runat="server"
                                                                AutoPostBack="false"
                                                                CssClass="table table-bordered table-hover table-striped"
                                                                GridLines="None" AutoGenerateColumns="False"
                                                                Font-Size="8pt" BackColor="#99FFCC"
                                                                OnSelectedIndexChanged="GrdPara_SelectedIndexChanged">
                                                                <Columns>
                                                                    <asp:BoundField DataField="NM_Destinatario" HeaderText="Apelido" />
                                                                    <asp:BoundField DataField="TX_EMAIL" HeaderText="E-Mail" />
                                                                    <asp:CommandField HeaderText="" ShowSelectButton="True" 
                                                                                                                    ItemStyle-Height ="10px" ItemStyle-Width ="15px" 
                                                                                                                    ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                                                    ControlStyle-Width ="15px" ControlStyle-Height ="15px" />
                                                                </Columns>
                                                                <RowStyle CssClass="cursor-pointer" />
                                                            </asp:GridView>

                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnPara" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdPara" EventName="SelectedIndexChanged" />
                                                        </Triggers>

                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <label for="usr">&nbspCom Cópia</label>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="BtnComCopia" runat="server" Text="Adicionar" CssClass="btn btn-success" TabIndex="12" OnClick="BtnComCopia_Click" Height="33"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                            </asp:LinkButton>
                                                            <br />
                                                            <br />
                                                            <asp:GridView ID="GrdComCopia" runat="server"
                                                                AutoPostBack="false"
                                                                CssClass="table table-bordered table-hover table-striped"
                                                                GridLines="None" AutoGenerateColumns="False"
                                                                Font-Size="8pt" BackColor="#99FFCC"
                                                                OnSelectedIndexChanged="GrdComCopia_SelectedIndexChanged">
                                                                <Columns>
                                                                    <asp:BoundField DataField="NM_Destinatario" HeaderText="Apelido" />
                                                                    <asp:BoundField DataField="TX_EMAIL" HeaderText="E-Mail" />
                                                                    <asp:CommandField HeaderText="" ShowSelectButton="True" 
                                                                                                                    ItemStyle-Height ="10px" ItemStyle-Width ="15px" 
                                                                                                                    ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                                                    ControlStyle-Width ="15px" ControlStyle-Height ="15px" />
                                                                </Columns>
                                                                <RowStyle CssClass="cursor-pointer" />
                                                            </asp:GridView>

                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnComCopia" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdComCopia" EventName="SelectedIndexChanged" />
                                                        </Triggers>

                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <label for="usr">&nbspCom Cópia Oculta</label>

                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="BtnComCopiaOculta" runat="server" Text="Adicionar" CssClass="btn btn-success" TabIndex="12" OnClick="BtnComCopiaOculta_Click" Height="33"> 
                                                            <span aria-hidden="true" class="glyphicon glyphicon-plus"></span>
                                                            </asp:LinkButton>
                                                            <br />
                                                            <br />
                                                            <asp:GridView ID="grdComCopiaOculta" runat="server"
                                                                AutoPostBack="false"
                                                                CssClass="table table-bordered table-hover table-striped"
                                                                GridLines="None" AutoGenerateColumns="False"
                                                                Font-Size="8pt" BackColor="#99FFCC"
                                                                OnSelectedIndexChanged="grdComCopiaOculta_SelectedIndexChanged">
                                                                <Columns>
                                                                    <asp:BoundField DataField="NM_Destinatario" HeaderText="Apelido" />
                                                                    <asp:BoundField DataField="TX_EMAIL" HeaderText="E-Mail" />
                                                                    <asp:CommandField HeaderText="" ShowSelectButton="True" 
                                                                                                                    ItemStyle-Height ="10px" ItemStyle-Width ="15px" 
                                                                                                                    ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                                                    ControlStyle-Width ="15px" ControlStyle-Height ="15px" />
                                                                </Columns>
                                                                <RowStyle CssClass="cursor-pointer" />
                                                            </asp:GridView>

                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnComCopiaOculta" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdComCopiaOculta" EventName="SelectedIndexChanged" />
                                                        </Triggers>

                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </asp:Panel>

                                <asp:Panel ID="pnlAnexos" runat="server" Visible="false">
                                     <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btnNovoAnexo" runat="server" style="padding-left:10px; margin-bottom:10px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovoAnexo_Click" TabIndex="21"> 
                                                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span> Novo Anexo
                                            </asp:LinkButton>
                                            <br />
                                            <asp:GridView ID="grdAnexo" runat="server"
                                                CssClass="table  table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  Visible="true"
                                                OnSelectedIndexChanged="grdAnexo_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CD_ANEXO" HeaderText="#"  />
                                                    <asp:BoundField DataField="DS_ARQUIVO" HeaderText="Descricao"  />
                                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>                                           
                                         </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnNovoAnexo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="grdAnexo" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </div>
                        </div>

                        </ContentTemplate> 
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSalvarContato" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnAddContato" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                <div class="row">         
                    <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                       <label for="usr" style ="margin-top:1px;">Assunto</label>
                       <asp:TextBox ID="txtAssunto" CssClass="form-control" runat="server" Enabled="true" Text ="" Font-Size="Small" TabIndex="2" />
                    </div>
                    <div class="col-md-12" style="margin-top:15px">
                        <div class="panel panel-default">
                            <div class="panel-heading" >
                                Visualização
                                <asp:LinkButton runat="server" title="Editar Descrição" ID="BtnEditarDS" OnClick="BtnEditarDS_Click" CssClass="btn btn-info" style="height:33px; margin-right:20px;float:left;margin-top:-8px" TabIndex="17"><span class=" glyphicon glyphicon-pencil"></span></asp:LinkButton>                                  
                            </div>
                            
                            <div class="panel-body" >
                                <asp:Literal runat="server" ID="litDescricao" ></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>                                          

            </div>
        </div>
    </div>
</asp:Content>
