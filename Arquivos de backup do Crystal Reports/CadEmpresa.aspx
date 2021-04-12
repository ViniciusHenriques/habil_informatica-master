<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadEmpresa.aspx.cs" Inherits="SoftHabilInformatica.Pages.Empresas.CadEmpresa" %>
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
            $('#myTabs a[href="' + item + '"]').tab('show');
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
                Cadastro de Empresas
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

                <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes"> 
                    <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                </asp:LinkButton>

                <br />
                <br />

                <div class="col-md-3">
                    <div class="input-group">
                        <span class="input-group-addon">Código do Empresa : &nbsp;&nbsp;&nbsp; </span>
                        <asp:TextBox ID="txtCodEmpresa" CssClass="form-control" name="txtCodEmpresa" Enabled="false" runat="server" TabIndex="1"
                            onkeypress="return PermiteSomenteNumeros(event);"
                            MaxLength="2" />
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="input-group">
                        <span class="input-group-addon">Código da Pessoa : &nbsp;&nbsp;&nbsp; </span>

                        <asp:TextBox ID="txtCodPessoa" CssClass="form-control" name="txtCodPessoa" Enabled="false" runat="server" TabIndex="1" Width="70%"
                            onkeypress="return PermiteSomenteNumeros(event);"
                            MaxLength="2" />

                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="form-control btn btn-primary" TabIndex="-1" OnClick="LinkButton2_Click" Width="20%"> 
                                <span aria-hidden="true" title="Pessoa" class="glyphicon glyphicon-search"></span>
                        </asp:LinkButton>
                    </div>

                </div>

                <div class="col-md-3">
                    <div class="input-group">
                        <span class="input-group-addon">Situação : </span>
                        <asp:DropDownList ID="txtCodSituacao" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="col-md-12">
                    <div class="input-group">
                        <span class="input-group-addon">Nome da Empresa :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                        <asp:TextBox ID="txtNomeEmpresa" Enabled="false" CssClass="form-control" name="txtNomeEmpresa" TabIndex="2" runat="server" MaxLength="50" />
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="input-group">
                        <span class="input-group-addon">Regime Tributário :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                        <asp:DropDownList ID="ddlRegTributario" runat="server" AutoPostBack="true" TabIndex="7" CssClass="form-control "></asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-12">
                    <asp:HiddenField ID="TabName" runat="server" />

                    <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;">

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist" id="myTabs">
                            <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-user"></span>&nbsp;&nbsp;Perfis de Usuário</a></li>
                            <li role="presentation"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-file"></span>&nbsp;&nbsp;Tipo de Documento</a></li>

                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content" runat="server" id="PanelContext">

                            <div role="tabpanel" class="tab-pane col-md-6" id="home" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                <asp:GridView ID="grdPermissao" runat="server"
                                    Width="100%" AutoPostBack="true"
                                    CssClass="table table-bordered table-hover table-striped"
                                    GridLines="None" AutoGenerateColumns="False"
                                    Font-Size="8pt">

                                    <Columns>
                                        <asp:TemplateField HeaderText="Liberado">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkLiberado" runat="server" Checked='<%# Eval("Liberado").ToString().Equals("True") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CodigoPflUsuario" HeaderText="Perfil" />
                                        <asp:BoundField DataField="DescricaoPflUsuario" HeaderText="Descrição do Perfil" />
                                    </Columns>
                                    <RowStyle CssClass="cursor-pointer" />
                                </asp:GridView>
                            </div>

                            <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">

                                <div class="row" style="background-color: white; border: none; padding-left: 0%;">
                                    <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                        <div class="row" style="background-color: white; border: none;">


                                            <div class="col-md-11" style="background-color: white; border: none; text-align: left; font-size: x-small;">

                                                <asp:Label for="usr" runat="server" ID="lblGerSeq" Style="font-weight: bold">Gerador Sequencial </asp:Label>
                                                <asp:DropDownList ID="ddlGerSeq" runat="server" TabIndex="8" CssClass="form-control js-example-basic-single" Width="100%" AutoPostBack="false"></asp:DropDownList>

                                            </div>

                                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small; margin-top: 13px">
                                                <asp:LinkButton ID="BtnAddGerSeq" runat="server" Text="Adicionar" TabIndex="9" CssClass="btn btn-success" OnClick="BtnAddGerSeq_Click" Style="height: 33px"> 
                                                    <span aria-hidden="true" title="AdicionarServiço" class="glyphicon glyphicon-plus"></span>
                                                </asp:LinkButton>
                                            </div>

                                        </div>
                                        <br />
                                        <br />
                                        <asp:GridView ID="grdGerSeq" runat="server"
                                            CssClass="table table-bordered table-hover table-striped"
                                            GridLines="None" AutoGenerateColumns="False"
                                            Font-Size="8pt" Visible="true"
                                            OnSelectedIndexChanged="grdGerSeq_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="CodigoGeradorSequencialDocumento" HeaderText="Seq." />
                                                <asp:BoundField DataField="Cpl_Descricao" HeaderText="Descrição " />
                                                <asp:BoundField DataField="Cpl_SerieConteudo" HeaderText="Série Conteúdo " />
                                                <asp:BoundField DataField="Cpl_SerieNumero" HeaderText="Série Número" />


                                                <asp:CommandField HeaderText="Ação" ShowSelectButton="True"
                                                    ItemStyle-Height="15px" ItemStyle-Width="50px"
                                                    ButtonType="Image" SelectImageUrl="~/Images/Excluir.png"
                                                    ControlStyle-Width="25px" ControlStyle-Height="25px" />
                                            </Columns>
                                            <RowStyle CssClass="cursor-pointer" />
                                        </asp:GridView>

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
    <div class="modal fade" id="myexcpes" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir esta Empresa ?
                </div>
                <div class="modal-footer">

                    <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" UseSubmitBehavior="false" OnClick="btnCfmSim_Click" > 
                        Excluir
                    </asp:LinkButton>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
