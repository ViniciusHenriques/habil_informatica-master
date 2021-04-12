    <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManRegFisIcms.aspx.cs" Inherits="SoftHabilInformatica.Pages.Impostos.ManRegFisIcms"  
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/
        coes.js"></script>
    <script src="../../Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script>

        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });

        function pageLoad(sender, args) {
            $(".js-example-basic-single").select2();
        }
    </script>
    <style type="text/css">
        .form-control{
            border-radius:3px!important;
        }
        .select2-selection--single {
            height: 33px !important;
            font-size: 12px;
            padding: 2px;
        }

        .select2-selection__arrow {
            height: 30px !important;
        }

        .form-control {
            z-index: 0 !important;
        }
    </style>
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtdtinicial']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
        });
    </script>

    <div id="divNavTeste" style="padding-left: 10px; padding-top: 20px; padding-right: 10px;background-color: white;">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Manutenção da Regra Fiscal de Icms
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body">
                <div class="row" style="background-color: white; border: none;">
                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: small;">
                        <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click"> 
                            <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                        </asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false" OnClick="btnSalvar_Click"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnExcluir" runat="server" Text="Inativar" CssClass="btn btn-danger" TabIndex="2" data-toggle="modal" data-target="#myexcpes"> 
                            <span aria-hidden="true" title="Inativar" class="glyphicon glyphicon-remove"></span>  Inativar
                        </asp:LinkButton>
                    </div>

                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                        <asp:Panel ID="pnlAlterar" runat ="server" Visible ="true">
                            <div class="input-group">
                                <label for="usr">O que vou Alterar :  &nbsp;&nbsp;</label>
                                <asp:DropDownList ID="ddlAlteracao" AutoPostBack ="true"  runat="server" CssClass="form-control js-example-basic-single" TabIndex="3" Font-Size="Small" OnSelectedIndexChanged="ddlAlteracao_SelectedIndexChanged" >
                                    <asp:ListItem Text="* Nenhum Selecionado" Selected="True" Value="0" />
                                    <asp:ListItem Text="Data de Vigência" Value="1" />
                                    <asp:ListItem Text="Combinação: Localização, Grupos, Aplicação e Operação" Value="2" />
                                    <asp:ListItem Text="Aplicação da Regra" Value="3" />
                                </asp:DropDownList>
                            </div>
                        </asp:Panel>
                    </div>

                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                        <div class="input-group">
                            <label for="usr">Ação a ser aplicada :  &nbsp;&nbsp;</label>
                            <asp:DropDownList ID="ddlAcao" runat="server" AutoPostBack="false" CssClass="form-control js-example-basic-single" TabIndex="3" Font-Size="Small">
                                <asp:ListItem Text="Salvar somente" Selected="True" Value="0" />
                                <asp:ListItem Text="Salvar e continuar Editando" Value="1" />
                                <asp:ListItem Text="Salvar e Inativar Regra Anterior" Value="2" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row" style="background-color: white; border: none;padding-left: 20px; padding-top: 20px; padding-right: 20px;">
                    <div class="panel panel-default" style="padding-left: 10px; padding-top: 20px; padding-right: 10px;background-color: white;">
                        <div class="panel-body">
                            <asp:Panel runat="server" ID="pnlRegra" >

                                <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                    <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr">Regra :</label>
                                        <asp:TextBox ID="txtCodRegra" CssClass="form-control" runat="server" Enabled="false" />
                                        <br/>
                                    </div>
                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr">Data de Vigência:</label>
                                        <asp:TextBox ID="txtdtinicial" name="txtdtinicial" AutoPostBack="false" CssClass="form-control" TabIndex="5" runat="server"  />
                                        <br/>
                                    </div>
                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr">Data Inclusão:</label>
                                        <asp:TextBox ID="txtDataHora" CssClass="form-control" runat="server" Enabled="false" />
                                        <br/>
                                    </div>
                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr">Data Atualização:</label>
                                        <asp:TextBox ID="txtdtfinal" name="txtdtfinal" Enabled ="false"  AutoPostBack="False" CssClass="form-control" runat="server" TabIndex="6" />
                                        <br/>
                                    </div>
                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr">Situação :</label>
                                        <asp:DropDownList ID="ddlSituacao" runat="server" Enabled ="false"  AutoPostBack="False" TabIndex="4" CssClass="form-control" Font-Size="Small">
                                        </asp:DropDownList>
                                        <br/>
                                    </div>

                                    <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                        <label for="usr">Descrição da Regra :</label>
                                        <asp:TextBox ID="txtDescricao" name="txtDescricao" AutoPostBack="False" CssClass="form-control" runat="server" MaxLength ="100" TabIndex="6"  Width=100% />
                                        <br/>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>

                <asp:Panel runat="server" ID="pnlGrupos">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="background-color: white; border: none;padding-left: 20px; padding-top: 20px; padding-right: 20px;">
                                <div class="panel panel-default" style="padding-left: 10px; padding-top: 20px; padding-right: 10px;background-color: white;">
                                    <div class="panel-body">
                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr">Estado Origem :</label>
                                            <asp:DropDownList ID="ddlEstadoOrigem" runat="server" AutoPostBack="False" TabIndex="8" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlEstadoOrigem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr">Estado Destino :</label>
                                            <asp:DropDownList ID="ddlEstadoDestino" runat="server" AutoPostBack="False" TabIndex="9" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlEstadoDestino_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                            <label for="usr">&nbsp</label>
                                            <asp:LinkButton ID="BtnAddLocalizacao" runat="server" Text="Adicionar" TabIndex="10" CssClass="btn btn-success" OnClick="BtnAddLocalizacao_Click" Height="33"> 
                                            <span aria-hidden="true" title="Adicionar Localização" class="glyphicon glyphicon-plus"></span>
                                            </asp:LinkButton>
                                        </div>

                                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                            <asp:GridView ID="grdLocalizacao" runat="server"
                                                AutoPostBack="true"
                                                CssClass="table table-bordered table-hover table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt" BackColor="#99FFCC" AllowSorting ="true" OnSorting="grdLocalizacao_Sorting"
                                                OnSelectedIndexChanged="grdLocalizacao_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CodTabAliqIcms" HeaderText="Cód" />
                                                    <asp:BoundField DataField="UFOrigem" HeaderText="Estado de Origem" SortExpression="UFOrigem" />
                                                    <asp:BoundField DataField="UFDestino" HeaderText="Estado de Destino" SortExpression="UFDestino"  />
                                                    <asp:CommandField HeaderText="" ShowSelectButton="True" 
                                                                                                        ItemStyle-Height ="10px" ItemStyle-Width ="15px" 
                                                                                                        ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                                        ControlStyle-Width ="15px" ControlStyle-Height ="15px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlEstadoOrigem" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlEstadoDestino" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="BtnAddLocalizacao" EventName="Click" />
                        </Triggers>

                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <div class="row" style="background-color: white; border: none;padding-left: 10px; padding-top: 20px; padding-right: 10px;">
                                    <div class="panel panel-default" style="padding-left: 10px; padding-top: 20px; padding-right: 10px;background-color: white;">

                                        <label for="usr">Grupo de Tributação de Pessoas
                                            <asp:LinkButton ID="BtnGpoTribPessoa" runat="server" Text="Adicionar" CssClass="btn btn-success" TabIndex="12" OnClick="BtnGpoTribPessoa_Click" Height="30" Width="40"> 
                                            <span aria-hidden="true" title="Adicionar Grupo Tributação Pessoa" class="glyphicon glyphicon-plus"></span>
                                            </asp:LinkButton>
                                        </label>
                                        <asp:DropDownList ID="ddlGpoTribPessoa" runat="server" AutoPostBack="false" TabIndex="11" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlGpoTribPessoa_SelectedIndexChanged"/>
                                        <br /><br />
                                        <asp:GridView ID="grdGpoTribPessoa" runat="server" 
                                            AutoPostBack="false"
                                            CssClass="table table-bordered table-hover table-striped"
                                            GridLines="None" AutoGenerateColumns="False"
                                            Font-Size="8pt" BackColor="#99FFCC"
                                            OnSelectedIndexChanged="grdGpoTribPessoa_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="CodigoGpoTribPessoa" HeaderText="Cód" />
                                                <asp:BoundField DataField="DescricaoGpoTribPessoa" HeaderText="Descrição" />
                                                <asp:CommandField HeaderText="" ShowSelectButton="True" 
                                                                                                ItemStyle-Height ="10px" ItemStyle-Width ="15px" 
                                                                                                ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                                ControlStyle-Width ="15px" ControlStyle-Height ="15px" />
                                            </Columns>
                                            <RowStyle CssClass="cursor-pointer" />
                                        </asp:GridView>
                                    </div>
                               </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlGpoTribPessoa" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="BtnGpoTribPessoa" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdGpoTribPessoa" EventName="SelectedIndexChanged" />
                        </Triggers>
 
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <div class="row" style="background-color: white; border: none;padding-left: 10px; padding-top: 20px; padding-right: 10px;">
                                    <div class="panel panel-default" style="padding-left: 10px; padding-top: 20px; padding-right: 10px;background-color: white;">
                                        <label for="usr">Grupo de Tributação de Produtos
                                            <asp:LinkButton ID="BtnGpoTribProduto" runat="server" Text="Adicionar" CssClass="btn btn-success" TabIndex="14" OnClick="BtnGpoTribProduto_Click" Height="30" Width="40"> 
                                                <span aria-hidden="true" title="Adicionar Grupo Tributação Produto" class="glyphicon glyphicon-plus"></span>
                                            </asp:LinkButton>

                                        </label>
                                        <asp:DropDownList ID="ddlGpoTribProduto" runat="server" AutoPostBack="false" TabIndex="13" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlGpoTribProduto_SelectedIndexChanged"/>
                                        <br /><br />
                                        <asp:GridView ID="grdGpoTribProduto" runat="server"
                                            AutoPostBack="false"
                                            CssClass="table table-bordered table-hover table-striped"
                                            GridLines="None" AutoGenerateColumns="False"
                                            Font-Size="8pt" BackColor="#99FFCC"
                                            OnSelectedIndexChanged="grdGpoTribProduto_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="CodigoGpoTribProduto" HeaderText="Cód" />
                                                <asp:BoundField DataField="DescricaoGpoTribProduto" HeaderText="Descrição" />
                                                <asp:CommandField HeaderText="" ShowSelectButton="True" 
                                                                                                ItemStyle-Height ="10px" ItemStyle-Width ="15px" 
                                                                                                ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                                ControlStyle-Width ="15px" ControlStyle-Height ="15px" />
                                            </Columns>
                                            <RowStyle CssClass="cursor-pointer" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlGpoTribProduto" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="BtnGpoTribProduto" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdGpoTribProduto" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <div class="row" style="background-color: white; border: none;padding-left: 10px; padding-top: 20px; padding-right: 10px;">
                                    <div class="panel panel-default" style="padding-left: 10px; padding-top: 20px; padding-right: 10px;background-color: white;">

                                        <label for="usr">Aplicação no Uso
                                            <asp:LinkButton ID="BtnAplicacaoUso" runat="server" Text="Adicionar" CssClass="btn btn-success" TabIndex="16" OnClick="BtnAplicacaoUso_Click" Height="30" Width ="40"> 
                                            <span aria-hidden="true" title="Adicionar Estado Origem" class="glyphicon glyphicon-plus"></span>
                                            </asp:LinkButton>

                                        </label>

                                        <asp:DropDownList ID="ddlAplicacaoUso" runat="server" AutoPostBack="false" TabIndex="15" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlAplicacaoUso_SelectedIndexChanged"/>
                                        <br /><br />
                                        <asp:GridView ID="grdAplicacaoUso" runat="server"
                                            AutoPostBack="false"
                                            CssClass="table table-bordered table-hover table-striped"
                                            GridLines="None" AutoGenerateColumns="False"
                                            Font-Size="8pt" BackColor="#99FFCC"
                                            OnSelectedIndexChanged="grdAplicacaoUso_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="CodigoHabil_AplicacaoUso" HeaderText="Cód" />
                                                <asp:BoundField DataField="DescricaoHabil_AplicacaoUso" HeaderText="Descrição" />
                                                <asp:CommandField HeaderText="" ShowSelectButton="True" 
                                                                                                    ItemStyle-Height ="10px" ItemStyle-Width ="15px" 
                                                                                                    ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                                    ControlStyle-Width ="15px" ControlStyle-Height ="15px" />
                                            </Columns>
                                            <RowStyle CssClass="cursor-pointer" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlAplicacaoUso" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="BtnAplicacaoUso" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdAplicacaoUso" EventName="SelectedIndexChanged" />
                        </Triggers>

                    </asp:UpdatePanel>
                    
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                <div class="row" style="background-color: white; border: none;padding-left: 10px; padding-top: 20px; padding-right: 10px;">
                                    <div class="panel panel-default" style="padding-left: 10px; padding-top: 20px; padding-right: 10px;background-color: white;">
                                        <label for="usr">Tipo de Operação Fiscal
                                            <asp:LinkButton ID="btnOperFiscal" runat="server" Text="Adicionar" CssClass="btn btn-success" TabIndex="18" OnClick="btnOperFiscal_Click" Height="30" Width ="40">
                                            <span aria-hidden="true" title="Adicionar Operação Fiscal" class="glyphicon glyphicon-plus"></span>
                                            </asp:LinkButton>
                                        </label>
                                        <asp:DropDownList ID="ddlOperFiscal" runat="server" AutoPostBack="false" TabIndex="17" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlOperFiscal_SelectedIndexChanged">
                                        </asp:DropDownList>

                                        <asp:GridView ID="grdOperFiscal" runat="server"
                                            AutoPostBack="true"
                                            CssClass="table table-bordered table-hover table-striped"
                                            GridLines="None" AutoGenerateColumns="False"
                                            Font-Size="8pt" BackColor="#99FFCC"
                                            OnSelectedIndexChanged="grdOperFiscal_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="CodigoHabil_TipoOperFiscal" HeaderText="Cód" />
                                                <asp:BoundField DataField="DescricaoHabil_TipoOperFiscal" HeaderText="Descrição" />
                                                <asp:CommandField HeaderText="" ShowSelectButton="True" 
                                                    ItemStyle-Height ="10px" ItemStyle-Width ="15px" 
                                                    ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                    ControlStyle-Width ="15px" ControlStyle-Height ="15px" />

                                            </Columns>
                                            <RowStyle CssClass="cursor-pointer" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlOperFiscal" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="BtnOperFiscal" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdOperFiscal" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlAplicacao">
                    <div class="row" style="background-color: white; border: none;">
                        <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; padding-left:30px;padding-right:30px;">
                            <div class="panel panel-default">
                                <div class="panel-heading">Aplicação da Regra Fiscal - ICMS</div>
                                <div class="panel-body">

                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" onFocus="this.select()" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="row" style="margin-bottom:15px">
                                                <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                    <label for="usr">Regime Tributário : </label>
                                                </div>
                                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                    <asp:DropDownList ID="ddlRegTributario" Width="100%" runat="server" onFocus="this.select()" AutoPostBack="true" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlRegTributario_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>


                                            <asp:Panel ID="pnlImpostos" runat="server" onFocus="this.select()" Visible="false">
                                                <div class="row">
                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                        <label for="usr">CST/CSOSN : </label>
                                                    </div>
                                                    <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                        <asp:DropDownList ID="ddlCSTCSOSN" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlCSTCSOSN_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                        <label for="usr">Modalidade de determinação da BC do ICMS : </label>
                                                    </div>
                                                    <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                        <asp:DropDownList ID="ddlModDetBCIcms" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                        <label for="usr">Modalidade de determinação da BC do ICMS ST : </label>
                                                    </div>
                                                    <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                        <asp:DropDownList ID="ddlModDetBCIcmsST" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                        <label for="usr">Margem de Valor Agregado (MVA) Entrada : </label>
                                                    </div>
                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                        <asp:TextBox ID="txtMvaEntrada" CssClass="form-control" AutoPostBack="true" name="txtMvaEntrada" runat="server" onFocus="this.select()"
                                                            MaxLength="50" Style="text-align: right;" OnTextChanged="txtMvaEntrada_TextChanged" />
                                                    </div>
                                                    <div class="col-md-6"></div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                        <label for="usr">Mensagem do Icms : </label>
                                                    </div>

                                                    <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                        <asp:TextBox ID="txtMsgIcms" CssClass="form-control" AutoPostBack="true" name="txtMsgIcms" runat="server" MaxLength="1000" Height="240" TextMode="MultiLine" />
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-4" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                        <label for="usr">Código do Benefício Fiscal : </label>
                                                    </div>
                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                        <asp:TextBox ID="txtCodBnfFiscal" CssClass="form-control" AutoPostBack="true" name="txtCodBnfFiscal" runat="server" MaxLength="15" />
                                                    </div>
                                                    <div class="col-md-6"></div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-md-3"></div>

                                                    <div class="col-md-6">

                                                        <div class="panel panel-info">
                                                            <div class="panel-heading" style="text-align: center;">
                                                                <asp:Label ID="lblRegTributario" Text="" runat="server" onFocus="this.select()" />
                                                                <br />
                                                                <asp:Label ID="lblCSTCSOSN" Text="" runat="server" onFocus="this.select()" />
                                                            </div>
                                                            <div class="panel-body">

                                                                <asp:Panel ID="pnlCST00" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Interna da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST00Icms" CssClass="form-control" AutoPostBack="true" name="txtAliquota" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST00Icms_TextChanged" />
                                                                        </div>
                                                                        <div class="col-md-2"></div>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCST10" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST10RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCST10RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST10RedBCIcmsST_TextChanged" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST10Icms" CssClass="form-control" AutoPostBack="true" name="txtCST10Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST10Icms_TextChanged" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST10RedBCIcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST10RedBCIcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST10RedBCIcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST10IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST10IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" OnTextChanged="txtCST10IcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST10Mva" CssClass="form-control" AutoPostBack="true" name="txtCST10Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST10Mva_TextChanged" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <div id="divindicadores1" style="padding-left: 0px; border: none; border-radius: 0px;">
                                                                                <asp:CheckBox ID="chkCST10Difal" Text="&emsp;Calcula Difal: " CssClass="form-control-static" runat="server" onFocus="this.select()" AutoPostBack="true" Enabled="true" Font-Size="Small" value="1" OnCheckedChanged="chkCST10Difal_CheckedChanged" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        </div>
                                                                        <br />
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Diferencial de Alíquota : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST10Difal" CssClass="form-control" AutoPostBack="true" name="txtCST10Difal" runat="server" onFocus="this.select()" MaxLength="50" Enabled="false" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST10Difal_TextChanged" />
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCST20" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST20RedBC" CssClass="form-control" AutoPostBack="true" name="txtCST20RedBC" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST20RedBC_TextChanged" />
                                                                        </div>
                                                                        <div class="col-md-2"></div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Interna da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST20Icms" CssClass="form-control" AutoPostBack="true" name="txtCST20Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST20Icms_TextChanged" />
                                                                        </div>
                                                                        <div class="col-md-2"></div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Alíquota Efetiva : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST20Efetiva" CssClass="form-control" AutoPostBack="true" name="txtCST20Efetiva" runat="server" onFocus="this.select()" MaxLength="50" Enabled="false" Text="0,00" Style="text-align: right;" />
                                                                        </div>
                                                                        <div class="col-md-2"></div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Motivo da Desoneração: </label>
                                                                        </div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:DropDownList ID="ddlCST20MovDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCST30" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST30RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCST30RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST30RedBCIcmsST_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST30Icms" CssClass="form-control" AutoPostBack="true" name="txtCST30Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST30Icms_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST30IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST30IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" OnTextChanged="txtCST30IcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST30Mva" CssClass="form-control" AutoPostBack="true" name="txtCST30Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST30Mva_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Motivo da Desoneração: </label>
                                                                        </div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:DropDownList ID="ddlCST30MotDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCST404150" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <label for="usr">Motivo da Desoneração: </label>
                                                                        <asp:DropDownList ID="ddlCST405051MovDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                    </div>

                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCST51" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST51RedBC" CssClass="form-control" AutoPostBack="true" name="txtCST51RedBC" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST51RedBC_TextChanged" />
                                                                        </div>
                                                                        <div class="col-md-2"></div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Interna da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST51Icms" CssClass="form-control" AutoPostBack="true" name="txtCST51Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST51Icms_TextChanged" />
                                                                        </div>
                                                                        <div class="col-md-2"></div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Diferimento (Total ou Parcial) : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST51Diferimento" CssClass="form-control" AutoPostBack="true" name="txtCST51Diferimento" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" OnTextChanged="txtCST51Diferimento_TextChanged" />
                                                                        </div>
                                                                        <div class="col-md-2"></div>
                                                                    </div>

                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCST70" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST70RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCST10RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST70RedBCIcmsST_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST70Icms" CssClass="form-control" AutoPostBack="true" name="txtCST70Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST70Icms_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST70RedBCIcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST70RedBCIcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST70RedBCIcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST70IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST10IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" OnTextChanged="txtCST70IcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST70Mva" CssClass="form-control" AutoPostBack="true" name="txtCST70Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST70Mva_TextChanged" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Motivo da Desoneração: </label>
                                                                        </div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:DropDownList ID="ddlCST70MovDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCST90" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST90RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCST90RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST90RedBCIcmsST_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST90Icms" CssClass="form-control" AutoPostBack="true" name="txtCST90Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCST90Icms_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST90RedBCIcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST90RedBCIcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST90RedBCIcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST90IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST90IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" OnTextChanged="txtCST90IcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST90Mva" CssClass="form-control" AutoPostBack="true" name="txtCST90Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST90Mva_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <div id="divindicadores2" style="padding-left: 0px; border: none; border-radius: 0px;">
                                                                                <asp:CheckBox ID="chkCST90CalcDifal" Text="&emsp;Calcula Difal: " CssClass="form-control-static" runat="server" onFocus="this.select()" AutoPostBack="true" Enabled="true" Font-Size="Small" value="1" OnCheckedChanged="chkCST90CalcDifal_CheckedChanged" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                        </div>
                                                                        <br />
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Diferencial de Alíquota : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCST90Difal" CssClass="form-control" AutoPostBack="true" name="txtCST90Difal" runat="server" onFocus="this.select()" MaxLength="50" Enabled="false" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCST90Difal_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Motivo da Desoneração: </label>
                                                                        </div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:DropDownList ID="ddlCST90MovDesoneracao" runat="server" onFocus="this.select()" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>

                                                                <asp:Panel ID="pnlCSOSN101" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Interna da Operação do Simples Nacional : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN101IcmsSimples" CssClass="form-control" AutoPostBack="true" name="txtCSOSN101IcmsSimples" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN101IcmsSimples_TextChanged" />
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCSOSN201" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN201RedBCST" CssClass="form-control" AutoPostBack="true" name="txtCSOSN201RedBCST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN201RedBCST_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN201Icms" CssClass="form-control" AutoPostBack="true" name="txtCSOSN201Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN201Icms_TextChanged" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN201IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCST10IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN201Mva" CssClass="form-control" AutoPostBack="true" name="txtCSOSN201Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN201Mva_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Interna da Operação do Simples Nacional : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN201IcmsSimples" CssClass="form-control" AutoPostBack="true" name="txtCSOSN201IcmsSimples" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN201IcmsSimples_TextChanged" />
                                                                        </div>
                                                                    </div>



                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCSOSN202203" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN202203RedBCST" CssClass="form-control" AutoPostBack="true" name="txtCSOSN202203RedBCST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN202203RedBCST_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN202203Icms" CssClass="form-control" AutoPostBack="true" name="txtCSOSN202203Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN202203Icms_TextChanged" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN202203IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCSOSN202203IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" OnTextChanged="txtCSOSN202203IcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN202203Mva" CssClass="form-control" AutoPostBack="true" name="txtCSOSN202203Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN202203Mva_TextChanged" />
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCSOSN900" runat="server" onFocus="this.select()" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo ST : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN900RedBCIcmsST" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900RedBCIcmsST" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN900RedBCIcmsST_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota da Operação : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN900Icms" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900Icms" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN900Icms_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">% Redução da Base de Cálculo Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN900RedBCIcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900RedBCIcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN900RedBCIcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Icms Próprio : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN900IcmsProprio" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900IcmsProprio" runat="server" onFocus="this.select()" MaxLength="50" Text="0,00" Style="text-align: right;" OnTextChanged="txtCSOSN900IcmsProprio_TextChanged" />
                                                                        </div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Margem de Valor Agregado (MVA) Saída : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN900Mva" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900Mva" runat="server" onFocus="this.select()" MaxLength="50" Text="0,0000" Style="text-align: right;" OnTextChanged="txtCSOSN900Mva_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-2"></div>
                                                                        <div class="col-md-7" style="background-color: white; border: none; text-align: left; padding-top: 10px; font-size: small;">
                                                                            <label for="usr">Alíquota Interna da Operação do Simples Nacional : </label>
                                                                        </div>
                                                                        <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                                            <asp:TextBox ID="txtCSOSN900IcmsSimples" CssClass="form-control" AutoPostBack="true" name="txtCSOSN900IcmsSimples" runat="server" onFocus="this.select()" Text="0,00" MaxLength="50" Style="text-align: right;" OnTextChanged="txtCSOSN900IcmsSimples_TextChanged" />
                                                                        </div>
                                                                    </div>

                                                                </asp:Panel>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </asp:Panel>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlRegTributario" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCSTCSOSN" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlModDetBCIcms" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlModDetBCIcmsST" EventName="SelectedIndexChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtMvaEntrada" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST00Icms" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCST10RedBCIcmsST" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST10Icms" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST10IcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST10RedBCIcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST10Mva" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="chkCST10Difal" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST10Difal" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCST20RedBC" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST20Icms" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCST30RedBCIcmsST" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST30Icms" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST30IcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST30Mva" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCST51RedBC" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST51Icms" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST51Diferimento" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCST70RedBCIcmsST" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST70Icms" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST70IcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST70RedBCIcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST70Mva" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCST90RedBCIcmsST" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST90Icms" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST90IcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST90RedBCIcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST90Mva" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="chkCST90CalcDifal" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCST90Difal" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN101IcmsSimples" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN201RedBCST" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN201Icms" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN201Mva" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN201IcmsSimples" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN202203RedBCST" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN202203Icms" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN202203IcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN202203Mva" EventName="TextChanged" />

                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN900RedBCIcmsST" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN900Icms" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN900IcmsProprio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN900Mva" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSOSN900IcmsSimples" EventName="TextChanged" />
                                            
                                        </Triggers>
                                    </asp:UpdatePanel>


                                    <div class="container-fluid">
                                        <div class="row" style="background-color: white; border: none; padding-left: 0%;">
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small;">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">Log</div>
                                                    <div class="panel-body">
                                                        <asp:GridView ID="grdLog" runat="server" AutoPostBack="false"
                                                            CssClass="table table-bordered table-hover table-striped"
                                                            GridLines="None" AutoGenerateColumns="False"
                                                            Font-Size="8pt" BackColor="#99FFCC">
                                                            <Columns>
                                                                <asp:BoundField DataField="DataHora" HeaderText="Data/Hora" />
                                                                <asp:BoundField DataField="EstacaoNome" HeaderText="Máquina" />
                                                                <asp:BoundField DataField="UsuarioNome" HeaderText="Usuário" />
                                                                <asp:BoundField DataField="Cpl_DescricaoOperacao" HeaderText="Operação" />
                                                                <asp:BoundField DataField="DescricaoLog" HeaderText="Detalhes do Log" />
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
                </asp:Panel>

            </div> <!-- body -->
        </div> <!-- div class="panel panel-primary" -->
    </div>     <!-- divNavTeste -->

    <!-- Exclui Grupo -->
    <div class="modal fade" id="myexcpes" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Inativar esta Regra Fiscal de ICMS ?
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCfmSim" runat="server" Text="Inativar" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="False" OnClick="btnCfmSim_Click"></asp:Button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
