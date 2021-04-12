 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="FreteFabesul.aspx.cs" Inherits="SoftHabilInformatica.Pages.Transporte.FreteFabesul" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <body>
        <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
            <div class="panel panel-primary" >
                <div  class="panel-heading">Importação de XML
                    <div class="messagealert" id="alert_container"></div>
                </div>            
                <div class="panel-body">

                    <div class="row">   
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                    <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:2px" >
                                        <asp:LinkButton runat="server" id="btnSelecionarPasta" OnClick="btnSelecionarPasta_Click" CssClass="btn btn-info" AutoPostBack="true" ToolTip="Escolha um diretório para buscar os arquivos">
                                            <span aria-hidden="true" class="glyphicon glyphicon-plus"></span> Escolher diretório
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnRefresh" runat="server"  CssClass="btn btn-default" TabIndex="0" AutoPostBack="true" OnClick="btnRefresh_Click" height="34" > 
                                            <span aria-hidden="true" class="glyphicon glyphicon-refresh"></span> Atualizar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnLimpar" runat="server" CssClass="btn btn-default" TabIndex="0" OnClick="btnLimpar_Click" AutoPostBack="true"> 
                                            <span aria-hidden="true" class="glyphicon glyphicon-remove-circle"></span>  Limpar
                                        </asp:LinkButton>             
                                        <asp:LinkButton ID="BtnLerXML" runat="server" CssClass="btn btn-success" TabIndex="0" OnClick="BtnLerXML_Click" AutoPostBack="true" title="Transforme seus XMLs em uma Planilha excel"> 
                                            <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span>  Gerar planilha
                                        </asp:LinkButton>
                                    </div>
                                     <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:5px" >
                                         <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdXMLs" runat="server"
                                                        CssClass="table  table-hover table-striped"
                                                        GridLines="None" OnSelectedIndexChanged="grdXMLs_SelectedIndexChanged" 
                                                        Font-Size="8pt"  >
                                                    <PagerStyle HorizontalAlign = "left"   CssClass = "GridPager" />
                                                    <Columns>   
                                                        <asp:CommandField HeaderText="Remover" ShowSelectButton="True" 
                                                            ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                            ControlStyle-Width ="20px" ControlStyle-Height ="20px" />
                                                    </Columns>
                                                    <RowStyle CssClass="cursor-pointer" />
                                                </asp:GridView>
                                            </ContentTemplate> 
                                        </asp:UpdatePanel>
                                    </div>
                                </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnSelecionarPasta" EventName="Click"/>
                                <asp:PostBackTrigger ControlID="BtnLerXML" />
                                <asp:AsyncPostBackTrigger ControlID="grdXMLs" EventName="SelectedIndexChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </body>
</asp:Content>
