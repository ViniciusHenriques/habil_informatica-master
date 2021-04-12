<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="AbrInventario.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.AbrInventario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    
    <script src="../../Scripts/funcoes.js"></script>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>


    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    
    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));
        });
    </script>


<div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
    <div class="panel panel-primary">
        <div class="panel-heading">
            Abertura de Inventario
            <div class="messagealert" id="alert_container"></div>
        </div>
        <div class="panel-body">
            <div id="Tabs" role="tabpanel">
                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                <span aria-hidden="true" TabIndex="18" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
            </asp:LinkButton>
                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" 
                TabIndex="17"> 
                <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
            </asp:LinkButton>
                <asp:LinkButton ID="btnSair" runat="server" onFocus="this.select()" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                <span aria-hidden="true" title="Fechar" TabIndex="19" class="glyphicon glyphicon-off"></span>  Fechar
            </asp:LinkButton>
                <br/>
                <br/>
            <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                    <li role="presentation"><a href="#home"     aria-controls="home"    role="tab" data-toggle="tab"  class="tab-select"><span class="glyphicon glyphicon-filter"></span>&nbsp;&nbsp;Seleção dos Dados</a></li>
                    <li role="presentation"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-th-list"></span>&nbsp;&nbsp;Consulta</a></li>
                </ul>
            <!-- Tab panes -->
                <div class="tab-content" runat="server" onFocus="this.select()" id="PanelContext">
                    <div role="tabpanel" class="tab-pane" id="home" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;"  >
                        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" onFocus="this.select()" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="row" style="margin-top: 10px">
                                    <div class="col-md-6">
                                        <label for="usr"> Empresa: </label>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="1" Width="100%"> </asp:DropDownList>		   
                                                </ContentTemplate> 
                                            <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                                </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-3">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <label for="usr"> Categoria</label>
                                                <label for="usr"> De: </label>
                                                <asp:DropDownList ID="ddlCategoria1" runat="server" AutoPostBack="true" OnTextChanged="ddlCategoria1_TextChanged" CssClass="form-control js-example-basic-single" Width="100%" Font-Size="Small" TabIndex="8"> </asp:DropDownList>		   
                                            </ContentTemplate> 
                                            <Triggers> 
                                                <asp:AsyncPostBackTrigger ControlID="ddlCategoria1" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                     <div class="col-md-3">
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <label for="usr"> Até: </label>
                                                <asp:DropDownList ID="ddlCategoria2" runat="server" AutoPostBack="true" OnTextChanged="ddlCategoria2_TextChanged" CssClass="form-control js-example-basic-single" Width="100%" Font-Size="Small" TabIndex="8"> </asp:DropDownList>		   
                                            </ContentTemplate> 
                                            <Triggers> 
                                                <asp:AsyncPostBackTrigger ControlID="ddlCategoria2" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                     
                                </div>
                                <div class="row" style="margin-top: 10px">
                                    <div class="col-md-12" >
                                        <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Produto: </label>
                                            <div class="input-group " style="width:100%">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtProduto" CssClass="form-control"  runat="server" TabIndex="5" OnTextChanged="txtProduto_TextChanged" OnSelectedIndexChanged="txtProduto_SelectedIndexChanged" AutoPostBack="true"                                
                                                            Width="15%" MaxLength="6" />                                                      
                                                            <asp:LinkButton ID="btnPesquisarItem"  runat="server" CssClass="form-control btn btn-primary" Width="8%" TabIndex="6" OnClick="btnPesquisarItem_Click" AutoPostBack="true"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                            </asp:LinkButton>
                                                        <asp:TextBox ID="txtDcrproduto" CssClass="form-control"  runat="server"  Enabled="false"  Width="77%"  />
                                                    </ContentTemplate> 
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtProduto" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDcrproduto" EventName="TextChanged" />
                                                        </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                    </div>              
                                </div> 
                                <div class="row" style="margin-top: 10px">
                                <div class="col-md-6">
                                        <label for="usr"> Localização </label>
                                        <label for="usr"> De: </label>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlLocalizacao1" runat="server" OnTextChanged="ddlLocalizacao1_TextChanged" AutoPostBack="true" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="7"> </asp:DropDownList>		   
                                                </ContentTemplate> 
                                                    <Triggers> 
                                                        <asp:AsyncPostBackTrigger ControlID="ddlLocalizacao1" EventName="TextChanged" />
                                                    </Triggers>
                                            </asp:UpdatePanel>
                                    </div>

                                    <div class="col-md-6">
                                        <label for="usr"> Até: </label>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlLocalizacao2" runat="server" AutoPostBack="true" OnTextChanged="ddlLocalizacao2_TextChanged" Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="7"> </asp:DropDownList>		   
                                                </ContentTemplate> 
                                                    <Triggers> 
                                                        <asp:AsyncPostBackTrigger ControlID="ddlLocalizacao2" EventName="TextChanged" />
                                                    </Triggers>
                                            </asp:UpdatePanel>
                                    </div>                                   
                                </div>
                                <div class="row" style="margin-top: 10px"><br>
                                    <div class="col-md-2" >
                                        <label for="usr"> Contagem: </label>
                                        <asp:DropDownList ID="ddlContagem" CssClass="form-control" runat="server" TabIndex="11" Font-Size="Medium">
                                            <asp:ListItem Value="1" Text="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                    </asp:DropDownList>                                  

                                </div>
                        </div>
                            
</div>
                </ContentTemplate>
                        </asp:UpdatePanel>
                    <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;">
                        <div style = "overflow-x: auto;" class="row">
                                    <asp:GridView ID="grdGrid" runat="server" CssClass="table table-bordered table-hover table-striped" GridLines="None" AutoGenerateColumns="False"
                                            Font-Size="8pt" AllowPaging="true" PageSize="100" 
                                            PagerSettings-Mode="NumericFirstLast">
                                            <PagerStyle HorizontalAlign = "Right"   CssClass = "GridPager" Font-Size="Smaller" />
                                            <Columns>
                                                <asp:BoundField DataField="DtGeracao" HeaderText="Data de Geração" />
                                                <asp:BoundField DataField="CodigoIndice" HeaderText="Cód. Indice" />
                                                <asp:BoundField DataField="DescInventario" HeaderText="Descrição" />                                                
                                                <asp:BoundField DataField="NrContagem" HeaderText="Contagem"  />
                                                <asp:BoundField DataField="DescSituacao" HeaderText="Desc. Situação" />
                                            </Columns>
                                        <RowStyle CssClass="cursor-pointer"  />
                                    </asp:GridView>
                                </div>
                    </div>
                </div>
            </div>
        </div>
</div>
</div>
</div>

</asp:Content>
