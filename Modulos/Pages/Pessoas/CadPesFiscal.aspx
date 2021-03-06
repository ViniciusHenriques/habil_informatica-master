<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" 
    CodeBehind="CadPesFiscal.aspx.cs" Inherits="SoftHabilInformatica.Pages.Pessoas.CadPesFiscal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />

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
            <div class="panel-heading">Cadastro Fiscal de Pessoa
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                    <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                    </div> 
                </asp:Panel>
            </div>
            <div class="panel-body">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" OnClick="btnVoltar_Click"> 
                                <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click"> 
                                <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                            </asp:LinkButton>

                        </td>

                    </tr>
                </table>
                <br/>
                <div class="panel panel-default">
                    <div class="panel-heading">Pessoa
               
                    </div>
                    <div class="panel-body">
                          <div class="row" style="background-color:white;border:none;">
                            <div class="col-md-3 col-xs-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Código Pessoa</label>
                                <asp:TextBox ID="txtCodPessoa" runat="server" CssClass="form-control"  Font-Size ="Small"  ></asp:TextBox>
                            </div>

                            <div class="col-md-3 col-xs-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Data Atualização</label>
                                <asp:TextBox ID="txtDataAtualizacao" runat="server" Enabled="false" CssClass="form-control"  Font-Size ="Small"  ></asp:TextBox>
                            </div>

                            <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                <label for="usr" style ="margin-top:1px;">Razão Social</label>
                                <asp:TextBox ID="txtRazSocial" runat="server" CssClass="form-control col-md-6"  Font-Size ="Small" ></asp:TextBox>
                            </div>

                        </div>
                    </div>
                </div>
                 <div class="row" style="background-color:white;border:none;margin-top:13px;" >
                                        
                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                        <label for="usr">Regime Tributário : </label>
                        <asp:DropDownList ID="ddlRegTributario" runat="server" AutoPostBack="false" TabIndex="1" CssClass="form-control js-example-basic-single" Width="100%" Height="100%"></asp:DropDownList>
                    </div>
                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                        <label for="usr">Grupo de Tributação : </label>
                        <asp:DropDownList ID="ddlGpoTribPessoa" runat="server" AutoPostBack="false" TabIndex="2" CssClass="form-control js-example-basic-single" Width="100%" Height="100%"></asp:DropDownList>
                    </div>
                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                        <label for="usr">Tipo de Operação: </label>
                        <asp:DropDownList ID="ddlTipoOperacao" runat="server" AutoPostBack="false" TabIndex="3" CssClass="form-control js-example-basic-single" Width="100%" Height="100%"></asp:DropDownList>
                    </div> 
                     <div class="col-md-6" style="font-size:x-small;">
                        <label for="usr">PIS: </label>
                        <asp:DropDownList ID="ddlPIS" runat="server" AutoPostBack="false" TabIndex="4" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                    </div>
                    <div class="col-md-6" style="font-size:x-small;">
                        <label for="usr">COFINS: </label>
                        <asp:DropDownList ID="ddlCOFINS" runat="server" AutoPostBack="false" TabIndex="5" CssClass="form-control js-example-basic-single" Width="100%"></asp:DropDownList>
                    </div>
                </div> 
            </div>
        </div> 
    </div>

       <!-- Exclui Pessoa -->
   

</asp:Content>
