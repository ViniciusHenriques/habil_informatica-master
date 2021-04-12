<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SistemaEmManut.aspx.cs" Inherits="HabilInformatica.SistemaEmManut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="../Content/EstiloDefinidoPorParametro.css" rel="stylesheet" />
    <link type="text/css" href="Content/bootstrap.css" rel="stylesheet" />
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <link rel="stylesheet" href='Content/Bootstrap3.0.3/bootstrap.min.css' media="screen" />
</head>

<script src="Scripts/jquery-2.1.4.min.js"></script>
<script src="Scripts/bootstrap.min.js"></script>
<script type="text/javascript" src='Content/Bootstrap3.0.3/jquery-1.8.3.min.js'></script>
<script type="text/javascript" src='Content/Bootstrap3.0.3/bootstrap.min.js'></script>

<script type="text/javascript" language="javascript">
    function passaCampo(e, proximoCampo) {
        if (e.keyCode == 13) {
            e.keyCode = 0
            proximoCampo.focus()
        }
    }
</script>
    <%--#ff6a00--%>
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


        <div class="col-md-offset-4 col-md-4">

            <div class="panel panel-default">
                <div class="panel-heading">
                    Oops!!! Erro
                </div>

                <div class="panel-body">
                    <br />
                    <h3>Sistema temporariamente fora do ar para Manutenção. por favor aguarde alguns minutos.</h3>
                    <br />
                    <p style="text-align: center">
                        <asp:LinkButton ID="btnEntrar" runat="server" UseSubmitBehavior="false" CssClass="btn btn-primary" OnClick="btnEntrar_Click"> 
                    <span aria-hidden="true" title="Tentar Entrar Novamente" class="glyphicon glyphicon-ok"></span>  Entrar
                        </asp:LinkButton>
                    </p>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
