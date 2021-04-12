<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" CodeBehind="CadPessoa_Contato.aspx.cs" Inherits="SoftHabilInformatica.Pages.Pessoas.CadPessoa_Contato" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsCep.js" type="text/javascript"></script>
    <script src="../../Scripts/funcoes.js" type="text/javascript"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <div id="divNavTeste" style="padding-left:30px;padding-top:30px;padding-right:30px;font-size:small;" >
        <div class="panel panel-primary">
            <div class="panel-heading">Cadastro de Pessoas</div>

            <div class="panel-body"  style="padding-left:30px;padding-top:30px;padding-right:30px;font-size:small;">

                <div class="container-fluid">
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Pessoa (<asp:Label ID="txtCodPessoa" runat="server"/>)</label>
                            <asp:Label ID="txtRazSocial" runat="server" CssClass="form-control" />
                        </div>
                    </div>                            
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">Contato da Pessoa
                        <div class="messagealert" id="alert_container"></div>
                        <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                            <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                                <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                                <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                            </div> 
                        </asp:Panel>

                    </div>
        
                    <div class="panel-body">
                        <div class="container-fluid">
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="btnCanContato" runat="server" Text="Excluir" CssClass="btn btn-default" UseSubmitBehavior="false" Visible="true" OnClick="btnCanContato_Click"> 
                                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-default"></span>  Cancelar
                                        </asp:LinkButton>
                                
                                        <asp:LinkButton ID="btnSlvContato" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" Visible="true" OnClick="btnSlvContato_Click"> 
                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnExcContato" runat="server" Text="Excluir" CssClass="btn btn-danger" UseSubmitBehavior="false" Visible="false" OnClick="btnExcContato_Click"> 
                                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnTiraFoto" runat="server" Text="Tirar Foto" CssClass="btn btn-success" UseSubmitBehavior="false" Visible="True" OnClick="btnTiraFoto_Click"> 
                                        <span aria-hidden="true" title="Tirar Foto" class="glyphicon glyphicon-camera"></span> Tirar Foto
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnCapturaFoto" runat="server" Text="Tirar Foto" CssClass="btn btn-success" UseSubmitBehavior="false" Visible="True" OnClick="btnCapturaFoto_Click"> 
                                        <span aria-hidden="true" title="Tirar Foto" class="glyphicon glyphicon-camera"></span> Capturar Foto
                                        </asp:LinkButton>
                                        <br/>
                                        <br/>

                                        <div class="row" style="background-color:white;border:none;">
                                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Item</label>
                                                <asp:TextBox ID="txtCttItem" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                            
                                            <div class="col-md-5" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Tipo de Contato</label>
                                                <asp:DropDownList ID="ddlTipoContato" runat="server" AutoPostBack="true" CssClass="form-control" >
                                        
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-md-5" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Inscrição da Pessoa</label>

                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="DdlTipoInscricao" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="DdlTipoInscricao_SelectedIndexChanged"></asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="DdlTipoInscricao" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>

                                            </div>

                                            <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Nome do Contato</label>
                                                <asp:TextBox ID="txtNomeContato" runat="server" CssClass="form-control" Width="120%" TabIndex="1"/>
                                            </div>
                                        </div>                            

                                    </td>
                                    <td>
                                        <div class="col-md-12" style="background-color:white;border:none;text-align:right;font-size:x-small; margin-left:100% ">
                                            <asp:Image ID="imaFoto" ImageUrl=""  runat="server" BorderStyle ="Dashed"  BorderWidth ="2" ImageAlign ="Right" Width="350"  />
                                        </div>
                                    </td>
                                </tr>
                            </table>


                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Fone Principal</label>
                                    <div class="input-group " >
                                        <div class="input-group-addon" style="padding-right:20px">
                                            <img src="../../Images/WhatsApp.png" width="20" height="20" title="Marcar se o Telefone for WhatsApp" style="float:left;margin-right:5px" />
                                            <div style="margin-top:4px;"><asp:CheckBox runat="server" ID="chkWhatsApp"  /></div>
                                        </div>
                                        <span class="input-group-addon">+55</span>
                                        <asp:TextBox ID="txtfone1"
                                            name="txtfone1"
                                            CssClass="form-control"
                                            runat="server"
                                            TabIndex="2"  MaxLength="15"
                                            OnTextChanged="txtfone1_TextChanged" PlaceHolder="(DD) 99999-9999" />
                                    </div>
                                </div>

                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Fone Secundário</label>
                                    
                                    <div class="input-group ">
                                        <span class="input-group-addon">+55</span>
                                        <asp:TextBox ID="txtfone2"
                                            name="txtfone1"
                                            CssClass="form-control"
                                            runat="server"
                                            TabIndex="3" MaxLength="15"
                                            OnTextChanged="txtfone2_TextChanged" PlaceHolder="(DD) 99999-9999"/>
                                    </div>
                                </div>                               

                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Fone Recado</label>
                                    <div class="input-group ">
                                        <span class="input-group-addon">+55</span>
                                        <asp:TextBox ID="txtfone3"
                                            name="txtfone1"
                                            CssClass="form-control"
                                            runat="server"
                                            TabIndex="4"  MaxLength="15"
                                            OnTextChanged="txtfone3_TextChanged" PlaceHolder="(DD) 99999-9999"/>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">E-Mail Principal</label>
                                    <asp:TextBox ID="txtmail1"
                                        name="txtmailnfe"
                                        CssClass="form-control"
                                        runat="server"
                                        MaxLength="100"
                                        OnTextChanged="txtmail1_TextChanged" />
                                </div>
                                <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Senha Email</label>
                                    <asp:TextBox ID="txtMailSenha"
                                        name="txtMailSenha"
                                        CssClass="form-control"
                                        runat="server"
                                        MaxLength="100"
                                       TextMode="Password" />
                                </div>
                            </div>
                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">E-Mail NF-e</label>
                                    <asp:TextBox ID="txtmailnfe"
                                        name="txtmailnfe"
                                        CssClass="form-control"
                                        runat="server"
                                        MaxLength="100"
                                        OnTextChanged="txtmailnfe_TextChanged" />
                                </div>
                            </div>
                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">E-Mail NFS-e</label>
                                    <asp:TextBox ID="txtmailNFSe"
                                        name="txtmailnfe"
                                        CssClass="form-control"
                                        runat="server"
                                        MaxLength="100"
                                        OnTextChanged="txtmailNFSe_TextChanged" />
                                </div>
                            </div>

                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">E-Mail Secundário</label>
                                    <asp:TextBox ID="txtmail2"
                                        name="txtmailnfe"
                                        CssClass="form-control"
                                        runat="server"
                                        MaxLength="100"
                                        OnTextChanged="txtmail2_TextChanged" />
                                </div>
                            </div>

                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">E-Mail Recado</label>
                                    <asp:TextBox ID="txtmail3"
                                        name="txtmailnfe"
                                        CssClass="form-control"
                                        runat="server"
                                        MaxLength="100"
                                        OnTextChanged="txtmail3_TextChanged" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cttRodape" runat="server">
</asp:Content>
