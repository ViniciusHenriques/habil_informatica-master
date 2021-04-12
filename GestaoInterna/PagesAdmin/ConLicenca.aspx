<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConLicenca.aspx.cs" Inherits="GestaoInterna.PagesAdmin.ConLicenca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

<%--    <link href="../../Content/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap-datepicker.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtToDoDate']").datepicker({ format: "dd/mm/yyyy" });
            $("input[id*='TextBox1']").datepicker({ format: "dd/mm/yyyy" });
        });
    </script>
    
    <br/><br/><br/><br/><br/><br/>
     <asp:TextBox ID="txtToDoDate" name="txtToDoDate" CssClass ="form-control" runat="server" Width="100" Text=""></asp:TextBox>
    <br/><br/><br/><br/><br/> 
    <asp:TextBox ID="TextBox1" name="txtToDoDate" CssClass ="form-control" runat="server" Width="100" Text=""></asp:TextBox>
--%>

    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

        <div class="panel panel-primary" >
            <div  class="panel-heading">Consulta de Licenças
                <div class="messagealert" id="alert_container"></div>
            </div>
            <div class="panel-body">
                <br />
                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>

                <asp:LinkButton ID="btnSair" runat="server" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
                <br />
                <br />

                <asp:HiddenField ID="TabName" runat="server"/>
                
                <div id="Tabs" role="tabpanel" >

                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                        <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab">Seleção dos Dados</a></li>
                        <li role="presentation"><a href="#consulta" aria-controls="profile" role="tab" data-toggle="tab">Consulta</a></li>
                    </ul>
                    <!-- Tab panes -->
                    <div class="tab-content" runat="server" id="PanelContext">
                        
                        
                        <div role="tabpanel" class="tab-pane" id="home" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;"  >
                        <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <table>
                                <tr>
                                    <asp:Panel ID="pnlFiltro1" runat="server" Visible ="false" >
                                        <td>
                                             <asp:Label  ID="lblFiltro1" CssClass="form-control" Text="Filtro 1" name="lblFiltro1" runat="server" Font-Size="Small" style="white-space: nowrap;"/>
                                             <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">De :</span>
                                                <asp:TextBox ID="txtFiltro11" CssClass="form-control" name="txtFiltro11" runat="server" Width="300" MaxLength="100" />
                                            </div>
                                            <br/>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                            <br/>

                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">Até : </span>
                                                <asp:TextBox ID="txtFiltro12" CssClass="form-control" name="txtFiltro12" runat="server" Width="300" MaxLength="100" />
                                          </div>
                                          <br/>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                            <br/>
                                        </td>

                                    </asp:Panel> 
                                </tr>
                                <tr>
                                    <asp:Panel ID="pnlFiltro2" runat ="server" Visible ="false">
                                        <td>
                                             <asp:Label  ID="lblFiltro2" CssClass="form-control" Text="Filtro 2" name="lblFiltro2" runat="server" runat="server" Font-Size="Small" style="white-space: nowrap;" />
                                            <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">De :</span>
                                                <asp:TextBox ID="txtFiltro21" CssClass="form-control" name="txtFiltro21" runat="server" Width="300" MaxLength="100" />
                                            </div>
                                            <br/>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                            <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">Até : </span>
                                                <asp:TextBox ID="txtFiltro22" CssClass="form-control" name="txtFiltro22" runat="server" Width="300" MaxLength="100" />
                                            </div>
                                            <br/>
                                        </td>

                                    </asp:Panel>
                                </tr>
                                <tr>
                                    <asp:Panel ID ="pnlFiltro3" runat="server" Visible ="false" >
                                        <td>
                                             <asp:Label  ID="lblFiltro3" CssClass="form-control" Text="Filtro 3: " name="lblFiltro3" runat="server" Font-Size="Small" style="white-space: nowrap;" />
                                             <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">De :</span>
                                                <asp:TextBox ID="txtFiltro31" CssClass="form-control" name="txtFiltro31" runat="server" Width="300" MaxLength="100" />
                                            </div>
                                             <br/>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                             <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">Até : </span>
                                                <asp:TextBox ID="txtFiltro32" CssClass="form-control" name="txtFiltro32" runat="server" Width="300" MaxLength="200" />
                                            </div>
                                             <br/>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                             <br/>
                                        </td>


                                    </asp:Panel>
                                </tr>
                                <tr>        
                                    <asp:Panel ID ="pnlFiltro4" runat="server" Visible ="false" >
                                        <td>
                                             <asp:Label  ID="lblFiltro4" CssClass="form-control" Text="Filtro 4: " name="lblFiltro4" runat="server" Font-Size="Small" style="white-space: nowrap;" />
                                             <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">De :</span>
                                                <asp:TextBox ID="txtFiltro41" CssClass="form-control" name="txtFiltro41" runat="server" Width="300" MaxLength="100" />
                                            </div>
                                             <br/>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                             <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">Até : </span>
                                                <asp:TextBox ID="txtFiltro42" CssClass="form-control" name="txtFiltro42" runat="server" Width="300" MaxLength="200" />
                                            </div>
                                             <br/>
                                        </td>


                                    </asp:Panel>
                                </tr>
                                <tr>
                                    <asp:Panel ID ="pnlFiltro5" runat="server" Visible="false">
                                        <td>
                                             <asp:Label  ID="lblFiltro5" CssClass="form-control" Text="Filtro 5: " name="lblFiltro5" runat="server" Font-Size="Small" style="white-space: nowrap;"  />
                                            <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">De :</span>
                                                <asp:TextBox ID="txtFiltro51" CssClass="form-control" name="txtFiltro51" runat="server" Width="300" MaxLength="100" />
                                            </div>
                                            <br/>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                            <br/>
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">Até : </span>
                                                <asp:TextBox ID="txtFiltro52" CssClass="form-control" name="txtFiltro52" runat="server" Width="300" MaxLength="200" />
                                            </div>
                                            <br/>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                            <br/>
                                        </td>


                                    </asp:Panel>
                                </tr>
                                <tr>
                                    <asp:Panel ID ="pnlFiltro6" runat="server" Visible ="false">
                                        <td>
                                             <asp:Label  ID="lblFiltro6" CssClass="form-control" Text="Filtro 6: " name="lblDescricao2" runat="server" Font-Size="Small" style="white-space: nowrap;"  />
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">De :</span>
                                                <asp:TextBox ID="txtFiltro61" CssClass="form-control" name="txtFiltro61" runat="server" Width="300" MaxLength="100" />
                                            </div>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <div class="input-group">
                                                <span class="input-group-addon">Até : </span>
                                                <asp:TextBox ID="txtFiltro62" CssClass="form-control" name="txtFiltro62" runat="server" Width="300" MaxLength="200" />
                                            </div>
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </table>                
                            <br />

                            Mostrar os Primeiros
                            <asp:DropDownList ID="ddlRegistros" CssClass="input-group-addon" runat="server" Width="100" Font-Size="Medium" OnSelectedIndexChanged="ddlRegistros_SelectedIndexChanged">
                                <asp:ListItem Value="50" Text="50" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="100" Text="100" ></asp:ListItem>
                                <asp:ListItem Value="1000" Text="1000" ></asp:ListItem>
                            </asp:DropDownList>              
                            Registros
                            &nbsp&nbsp&nbsp&nbsp
                            <asp:LinkButton ID="btnConsultar" runat="server"   CssClass="btn btn-default" OnClick="btnConsultar_Click"> 
                                <span aria-hidden="true" title="Consultar" class="glyphicon glyphicon-search"></span>Consultar
                            </asp:LinkButton>


                        </ContentTemplate>

                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnConsultar" />
                        </Triggers>

                        </asp:UpdatePanel>
                        </div>





                        <div role="tabpanel" class="tab-pane" id="consulta" style="padding-left:20px;padding-top:20px;padding-right:20px;font-size:small;"   >
                            <br/>
                            <asp:LinkButton ID="btnVoltarSelecao" runat="server" Text="Nova Consulta" CssClass="btn btn-default" UseSubmitBehavior="false" OnClick="btnVoltarSelecao_Click"> 
                                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>Nova Consulta
                            </asp:LinkButton>
                            <br/>
                            <br/>
                            <asp:GridView ID="grdGrid" runat="server" Width="100%" 
                                CssClass ="table table-bordered table-hover table-striped" 
                                GridLines="None" AutoGenerateColumns="False" 
                                Font-Size="8pt" BackColor="#99FFCC" 
                                OnSelectedIndexChanged="grdGrid_SelectedIndexChanged" 
                                AllowPaging="true" PageSize= "10" 
                                OnPageIndexChanging="grdGrid_PageIndexChanging" 
                                PagerSettings-Mode ="NextPrevious"
                                PagerSettings-FirstPageText="/Prim/"
                                PagerSettings-PreviousPageText="/Ant/" 
                                PagerSettings-NextPageText="/Próx/"
                                PagerSettings-LastPageText="/Últ/"  >
                                <Columns>
                                    <asp:BoundField DataField="CodigodaLicenca" HeaderText="Licença" />
                                    <asp:BoundField DataField="CodigodoCliente" HeaderText="Código" />
                                    <asp:BoundField DataField="NomeDoCliente" HeaderText="Descrição" />
                                    <asp:CommandField HeaderText="Ação" ButtonType="Button"  ControlStyle-CssClass="btn btn-info" ShowSelectButton="True" />
                                </Columns>
                                <RowStyle CssClass="cursor-pointer" />
                            </asp:GridView>        

                            &emsp;
                        </div>
 
                    </div>
                </div>
            </div>
        </div>
    </div>


<script>
    item = '#<%=PanelSelect%>';
    $(document).ready(function (e) {
        $('#myTabs a[href="' + item + '"]').tab('show');
        console.log($(item));
    });
</script>

</asp:Content>
<%--  --%>