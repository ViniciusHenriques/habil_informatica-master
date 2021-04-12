<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="HabilInformatica.Pages.Error" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Ocorreu um erro!</title>
        <link rel="icon" href="images/IconePagina.svg" type="">
        <link type="text/css" href="../Content/EstiloDefinidoPorParametro.css" rel="stylesheet" />
        <link type="text/css" href="../Content/bootstrap.css" rel="stylesheet" />
        <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
        <link rel="stylesheet" href='Content/bootstrap3/bootstrap.min.css' media="screen" />
        <script src="Scripts/jquery-2.1.4.min.js"></script>
        <script src="Scripts/bootstrap.min.js"></script>
        <script type="text/javascript" src='../Content/bootstrap3/jquery-1.8.3.min.js'></script>
        <script type="text/javascript" src='../Content/bootstrap3/bootstrap.min.js'></script>


        <script type="text/javascript" language="javascript">
            function passaCampo(e, proximoCampo) {
                if (e.keyCode == 13) {
                    e.keyCode = 0
                    proximoCampo.focus()
                }
            }
        </script>
    </head>
    <body class="CorPadraoEscolhida">
    
        <div class="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Software de Aplicação Comercial</h3>
            </div>
            <div class="panel-body">
                Sistema Web. Versão 1.0
            </div>
        </div>
        <form id="form1" runat="server">


            <div class="col-md-offset-2 col-md-8">

                <div class="panel panel-default">
                    <div class="panel-heading">
                        Oops!!! Erro
                    </div>

                    <div class="panel-body">
                        <div style="text-align:center">
                            <div class="col-md-12">
                                <p style="font-size:150px">:(</p>
                            </div>
                            <div class="col-md-12"><h2>Ocorreu um erro, tente novamente!</h2></div>
                        </div>
                        <p style="text-align: center">
                            <asp:LinkButton ID="btnSair" runat="server" UseSubmitBehavior="false" CssClass="btn btn-info" OnClick="btnSair_Click"> 
                        <span aria-hidden="true" title="Tentar Entrar Novamente" class="glyphicon glyphicon-off"></span>  Fechar
                            </asp:LinkButton>
                        </p>

                    </div>
                </div>
            </div>
        </form>
    </body>
</html>
