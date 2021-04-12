<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFornecedor.Master" AutoEventWireup="true" 
    CodeBehind="WelcomeFornecedor.aspx.cs" Inherits="SoftHabilInformatica.Pages.WelcomeFornecedor" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <script src="../../Scripts/funcoes.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>

    <link rel="stylesheet" href='../Content/bootstrap3/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='../Content/bootstrap3/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='../Content/bootstrap3/bootstrap.min.js'></script>

    <div class ="container" style="padding-top : 50px;">


        <div class="row">
            <div class="col-lg-3 col-md-6">
                <div class="panel" style="background-color:orange;color:white;height:100px;">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-2">
                                <i class="glyphicon glyphicon-edit" style="zoom:250%;"></i> 
                            </div>
                            <div class="col-xs-10 text-right">
                                <div class="huge"><asp:Label runat="server"  Text = "0" Font-Bold ="true"></asp:Label> </div>
                                <div style=""><asp:Label runat="server" Text ="Solicitações de Compra" Font-Bold ="true" ></asp:Label></div>
                                <br/>
                                <div>
                                    <asp:LinkButton runat="server" Text="+ Detalhes" BorderStyle ="None" ID="btnCaixa1" OnClick="btnCaixa1_Click" BackColor="Orange" ForeColor="White" ></asp:LinkButton>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

            </div>
        
            <div class="col-lg-3 col-md-6">
                <div class="panel" style="background-color:cornflowerblue;color:white;height:100px;">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-2">
                                <i class="glyphicon glyphicon-check"  style="zoom:250%;"></i> 
                            </div>
                            <div class="col-xs-10 text-right">
                                <div class="huge">999.999</div>
                                <div>Cotações de Preço</div>
                                <br />    
                                <div>Ver +</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-6">
                <div class="panel" style="background-color:yellowgreen;color:white;height:100px;">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-2">
                                <i class="glyphicon glyphicon-list"  style="zoom:250%;"></i> 
                            </div>
                            <div class="col-xs-10 text-right">
                                <div class="huge">999.999</div>
                                <div>Ordens de Compra</div>
                                <br />    
                                <div>Ver +</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-6">
                <div class="panel" style="background-color:orangered;color:white;height:100px;">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-2">
                                <i class="glyphicon glyphicon-align-justify"  style="zoom:250%;"></i> 
                            </div>
                            <div class="col-xs-10 text-right">
                                <div class="huge">999.999</div>
                                <div>Notas de Entrada</div>
                                <br />    
                                <div>Ver +</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlFigura" runat ="server" HorizontalAlign ="Center">
        <div class="PapelDeParedePadrao"  title="MSoftGate" Style="height:100%; width:100%; margin-top:0px!important;" ></div>
    </asp:Panel>     


</asp:Content>
