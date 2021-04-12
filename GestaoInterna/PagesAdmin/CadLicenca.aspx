<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadLicenca.aspx.cs" Inherits="GestaoInterna.PagesAdmin.CadLicenca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="../../Content/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap-datepicker.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtToDoDate']").datepicker({ format: "dd/mm/yyyy" });
        });
    </script>
    
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

            <div  class="panel panel-primary" >
                <div class="panel-heading">Cadastro de Licença
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
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

                    <div class="container-fluid"  style="background-color:white;border:none;">
                        <div class="row" style="background-color:white;border:none;">
                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Código da Licença</label>
                                <asp:TextBox ID="txtCodLicenca" runat="server" CssClass="form-control" Width="120" Enabled ="false"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Código do Cliente</label>
                                <asp:TextBox ID="txtCodCliente" runat="server" CssClass="form-control" Width="120"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Atualização Banco</label>
                                <asp:TextBox ID="txtAtuaBanco" runat="server" CssClass="form-control" Width="120" Enabled ="false"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small">
                            </div>

                            <div class="form-group col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <label for="exampleInputPassword1">Nome do Cliente</label>
                                <asp:TextBox ID="txtNomeCliente" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <label for="exampleInputPassword1">Servidor/Instância</label>
                                <asp:TextBox ID="txtServidor" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <label for="exampleInputPassword1">Nome do Banco</label>
                                <asp:TextBox ID="txtBanco" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <label for="exampleInputPassword1">Usuário do Banco</label>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                <label for="exampleInputPassword1">Senha do Banco</label>
                                <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                        </div>
                        <asp:Panel ID ="pnlLicenca" runat="server" Visible ="false">
                            <div class="panel panel-default" style ="background-color:white;">
                                <div class="panel-heading">Licenças do Cliente</div>
                                <div class="panel-body">

                                    <asp:Panel ID ="pnlGrid" runat="server">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">Consulta</div>
        
                                            <div class="panel-body">

                                                <asp:LinkButton ID="btnNovaChave" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovaChave_Click"> 
                                                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-edit"></span>  Nova Chave
                                                </asp:LinkButton>
                                                <br/><br/>
                                                <asp:GridView ID="grdConsulta" runat="server"
                                                    AutoPostBack=true  
                                                    CssClass="table table-bordered table-hover table-striped"
                                                    GridLines="None" AutoGenerateColumns="False"
                                                    Font-Size="8pt" BackColor="#99FFCC"
                                                    OnSelectedIndexChanged="grdConsulta_SelectedIndexChanged"  
                                                    OnRowDataBound ="grdConsulta_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="CodigoDoItem" HeaderText="Licença" />
                                                        <asp:BoundField DataField="DataDeValidade" HeaderText="Data de Validade" />
                                                        <asp:BoundField DataField="Guid" HeaderText="Guid" />
                                                        <asp:BoundField DataField="ChaveDeAutenticacao" HeaderText="Chave Autenticada" />
                                                        <asp:CommandField HeaderText="Consultar" ButtonType="Button" ControlStyle-CssClass="btn btn-info" SelectText="Acessar" FooterStyle-Font-Size ="X-Small"  ShowSelectButton="True" />
                                                    </Columns>
                                                    <RowStyle CssClass="cursor-pointer" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID ="pnlMexerChave" runat="server" Visible="false">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">Geração da Licença</div>
        
                                            <div class="panel-body">
                                                <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                     <asp:LinkButton ID="btnVoltarItem" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnVoltarItem_Click"> 
                                                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span> Voltar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnGravaItem" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" OnClick="btnGravaItem_Click"> 
                                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>Salvar
                                                    </asp:LinkButton>
                                                </div>    

                                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                    <label for="usr" style ="margin-top:1px;">Data de Validade</label>
                                                     <asp:TextBox ID="txtToDoDate" name="txtToDoDate" CssClass ="form-control" runat="server" Width="100" Text=""></asp:TextBox>
                                                </div>
                                                <div class="form-group col-md-10" style="background-color:white;border:none;text-align:left;font-size:x-small">
                                                    <label for="usr" style ="margin-top:1px;">Chave Gerada</label>
                                                     <asp:TextBox ID="txtChave" name="txtToDoDate" CssClass ="form-control" runat="server" Text=""></asp:TextBox>
                                                </div>
                                                

                                                <asp:GridView ID="grdPermissao" runat="server"
                                                    Width="100%" AutoPostBack=true
                                                    CssClass="table table-bordered table-hover table-striped"
                                                    GridLines="None" AutoGenerateColumns="False"
                                                    Font-Size="8pt" BackColor="#99FFCC">
                                
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Liberar">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkLiberar" runat="server"  Checked='<%# Eval("Liberar").ToString().Equals("True") %>'  />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CodigoModulo" HeaderText="Código" />
                                                        <asp:BoundField DataField="DescricaoModulo" HeaderText="Descrição do Módulo" />
                                                    </Columns>
                                                    <RowStyle CssClass="cursor-pointer" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </div>
                            </div>

                        </asp:Panel>
                    </div>
              </div>
            </div>
    </div>

</asp:Content>
