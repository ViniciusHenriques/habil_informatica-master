<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FrenteCaixa.Login" %>

<!DOCTYPE html>
<html lang="en">

<head>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Sigma: Ponto de Vendas</title>

    <!-- Bootstrap Core CSS -->
    <link href="../vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">

    <!-- MetisMenu CSS -->
    <link href="../vendor/metisMenu/metisMenu.min.css" rel="stylesheet">

    <!-- Custom CSS -->
    <link href="../dist/css/sb-admin-2.css" rel="stylesheet">

    <!-- Custom Fonts -->
    <link href="../vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->

</head>


<body style ="background-color:#4169E1">
    <%--<div id="wrapper">--%>

        <!-- Navigation -->
        <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand">Módulo de Vendas</a>
            </div>
            <!-- /.navbar-header -->
        </nav>

        <%--<div id="page-wrapper">--%>
            <div class="row">
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <br/>
                            <br/>

                <div class="col-lg-4" style ="background-color:#4169E1 ">
                </div>
                <!-- /.col-lg-4 -->
                <div class="col-lg-4" style ="background-color:#4169E1">
                    <form id="frm" runat="server">
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <div class="panel panel-default">
                        <div class="panel-heading">
                            Sigma: Ponto de Vendas
                        </div>
                        <div class="panel-body" style="text-align:center">
                            <asp:TextBox ID="txtUsuario1" Font-Size="8pt" PlaceHolder="Funcionário" Font-Bold="true" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            <br />
                            <asp:TextBox ID="pwdSenha1" Font-Bold="true" PlaceHolder="Senha" Font-Size="8pt" CssClass="form-control" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox>
                            <br />
                            <asp:LinkButton runat="server" ID="btnConfirma" Text="Entrar" CssClass="btn btn-success" TabIndex="3" OnClick="btnConfirma_Click"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="btnCancelar" Text="Sair" CssClass="btn btn-danger" OnClick="btnCancelar_Click"></asp:LinkButton>
                            <br />
                            <br />

                            <asp:Panel ID="PnlMsg" runat ="server">
                                <div class="alert alert-danger">
                                    <table>
                                        <tr>
                                            <asp:Label ID="lblMensagem" runat="server" Text="" Font-Size="Small"></asp:Label>
                                            <asp:LinkButton runat="server" ID="btnFecharMensagem" Text="Ok" Font-Size="Small" CssClass="btn btn-danger" TabIndex="5"  OnClick="btnFecharMensagem_Click"></asp:LinkButton>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                         </div>
                    </div>
                    </form>

                </div>
                <!-- /.col-lg-4 -->
                <div class="col-lg-4" style ="background-color:#4169E1">
                </div>
                <!-- /.col-lg-4 -->

            </div>
        <%--</div>--%>
        <!-- /#page-wrapper -->
    <%--</div>--%>
    <!-- /#wrapper -->

    <!-- jQuery -->
    <script src="../vendor/jquery/jquery.min.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="../vendor/bootstrap/js/bootstrap.min.js"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="../vendor/metisMenu/metisMenu.min.js"></script>

    <!-- Custom Theme JavaScript -->
    <script src="../dist/js/sb-admin-2.js"></script>
</body>

</html>
