<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CadLocalizacao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.CadLocalizacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    
    <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script src="../../Scripts/jquery.maskedinput.min.js"></script>


    <script type="text/javascript">
        item = '<%=MaskLocalizacao%>';

        jQuery(function ($) {
            $("#txtCodigo").mask(item);
        });

    </script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >

            <div  class="panel panel-primary" >
                <div class="panel-heading">Cadastro de Localizações - Endereços de Estoque
                    <div class="messagealert" id="alert_container"></div>
                    <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                        <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                            <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                            <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
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

                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>
                   
                    <br/>
                    <br/>
                    <br/>

                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Empresa : </span>
                                <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="true"  TabIndex="0" CssClass="form-control js-example-basic-single" Font-Size="Small"/>
                            </div>
                        </div>
                    </div>
                    <br/>
                    <br/>

                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Lançamento : &nbsp;&nbsp;&nbsp; </span>
                                <asp:TextBox ID="txtLancamento" CssClass="form-control" name="txtLancamento" Enabled="false" runat="server" TabIndex="1"  />
                            </div>
                        </div>


                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Situação : </span>
                                <asp:DropDownList ID="ddlSituacao" runat="server" CssClass="form-control" >
                                </asp:DropDownList>
                            </div>
                            
                        </div>

                    </div>

                    <br/>
                    <br/>
                    
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Código da Localização : &nbsp;&nbsp;&nbsp; </span>
                                <asp:TextBox ID="txtCodigo" CssClass="form-control" name="txtCodigo" Font-Size ="Medium" Font-Bold ="true"  ForeColor="Blue" runat="server" TabIndex="2" ClientIDMode="Static" />
                            </div>
                        </div>
                        
                        <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <div class="input-group">
                                <span class="input-group-addon">Tipo de Localização : </span>
                                <asp:DropDownList ID="ddlTipoLocalizacao" runat="server" CssClass="form-control" >
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>
                    
                    <br />
                    <br />
                    
                    <div class="input-group">
                        <span class="input-group-addon">Descrição da Localização : </span>
                        <asp:TextBox ID="txtDescricao"  CssClass="form-control" name="txtDescricao" TabIndex="3" runat="server"  MaxLength="50" />
                    </div>

                </div>
            </div>

    </div>

       <!-- Exclui Categoria -->
    <div class="modal fade" id="myexcpes"  tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
        <div class="modal-dialog" role="document" style ="height:100%;width:300px">
            <div class="modal-content" >
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Atenção</h4>
                </div>
                <div class="modal-body">
                    Deseja Excluir esta Localização ?
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click" > 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                    </asp:LinkButton>

                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
