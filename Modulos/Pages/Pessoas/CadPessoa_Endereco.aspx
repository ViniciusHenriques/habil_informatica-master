<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" CodeBehind="CadPessoa_Endereco.aspx.cs" Inherits="SoftHabilInformatica.Pages.Pessoas.CadPessoa_Endereco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">

    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsCep.js" type="text/javascript"></script>
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
                    <div class="panel-heading">Endereço da Pessoa
                        <div class="messagealert" id="alert_container"></div>
                        <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                            <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                                <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                                <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                            </div> 
                        </asp:Panel>
                    </div>
        
                    <div class="panel-body">
                        <asp:LinkButton ID="btnCanEndereco" runat="server" Text="Excluir" CssClass="btn btn-default" UseSubmitBehavior="false" Visible="true" OnClick="btnCanEndereco_Click"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-default"></span>  Cancelar
                        </asp:LinkButton>
                                
                        <asp:LinkButton ID="btnSlvEndereco" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" Visible="true" OnClick="btnSlvEndereco_Click"> 
                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcEndereco" runat="server" Text="Excluir" CssClass="btn btn-danger" UseSubmitBehavior="false" Visible="false" OnClick="btnExcEndereco_Click"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>

                        <br/>
                        <br/>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>

                                <div class="container-fluid">
                                    <div class="row" style="background-color:white;border:none;">
                                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Item</label>
                                            <asp:TextBox ID="txtEndItem" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Incrição da Pessoa</label>
                                            <asp:DropDownList ID="DdlTipoInscricao" runat="server" AutoPostBack="true" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>


                                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Tipo Endereço</label>
                                            <asp:DropDownList ID="ddlTipoEndereco" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlTipoEndereco_SelectedIndexChanged">
                                                
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row" style="background-color:white;border:none;">

                                        <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">CEP</label>
                                            <asp:TextBox ID="txtcodcep"
                                                name="txtcodcep"
                                                CssClass="form-control"
                                                runat="server"
                                                TabIndex="0" MaxLength="10"
                                                AutoPostBack="true"

                                                OnTextChanged="txtcodcep_TextChanged" />
                                        </div>
                                        <div class="col-md-1" style="margin-top:22px">
                                             <asp:LinkButton ID="btnAddUnidade" runat="server" Text="Adicionar" CssClass="btn btn-success"  UseSubmitBehavior="false" OnClick="btnAddUnidade_Click" height="31"> 
                                                <span aria-hidden="true" title="Adicionar CEP" class="glyphicon glyphicon-plus"></span>
                                            </asp:LinkButton>
                                        </div>

                                        <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Estado (<asp:Label ID="lblCodEstado" runat="server"/>)</label>
                                            <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" TabIndex="1"/>
                                        </div>
                                    </div>
                                    <div class="row" style="background-color:white;border:none;">

                                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Município (<asp:Label ID="lblCodMunicipio" runat="server"/>)</label>
                                            <asp:TextBox ID="txtMunicipio" runat="server" CssClass="form-control" TabIndex="3"/>
                                        </div>

                                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Bairro (<asp:Label ID="lblCodBairro" runat="server"/>)</label>
                                            <asp:TextBox ID="txtBairro" runat="server" CssClass="form-control" TabIndex="4"/>
                                        </div>
                                    </div>
                                    <div class="row" style="background-color:white;border:none;">
                                        <div class="col-md-7" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Logradouro</label>
                                            <asp:TextBox ID="txtLogradouro" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Nro</label>
                                            <asp:TextBox ID="txtNroEndereco" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Complemento</label>
                                            <asp:TextBox ID="txtComplemento" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTipoEndereco" EventName="SelectedIndexChanged"/>
                                <asp:AsyncPostBackTrigger ControlID="txtcodcep" EventName="TextChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cttRodape" runat="server">
</asp:Content>
