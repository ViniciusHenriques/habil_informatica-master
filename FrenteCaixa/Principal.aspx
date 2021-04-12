<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="FrenteCaixa.Principal" %>

<!DOCTYPE html>
<html lang="en">

<head>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Módulo de Vendas</title>

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

<body>
<form id="Form1" runat="server">

    <div id="wrapper">

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

            <ul class="nav navbar-top-links navbar-right">
                <li class="dropdown">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                        <i class="fa fa-user fa-fw"></i> <i class="fa fa-caret-down"></i>
                        <asp:Label ID="lblNomefuncionario" runat="server"></asp:Label>
                    </a>
                    <ul class="dropdown-menu dropdown-user">
<%--                        <li><a href="#"><i class="fa fa-gear fa-fw"></i> Configurações</a>
                        </li>
                        <li class="divider"></li>--%>
                        <li><a href="login.aspx"><i class="fa fa-sign-out fa-fw"></i> Sair</a>
                        </li>
                    </ul>
                    <!-- /.dropdown-user -->
                </li>
                <!-- /.dropdown -->
            </ul>
            <!-- /.navbar-top-links -->

            <div class="navbar-default sidebar" role="navigation">
                <div class="sidebar-nav navbar-collapse">
                    <ul class="nav" id="side-menu">
                        <li><asp:LinkButton runat="server" ID="btnAbertura" Text="Abertura de Caixa" OnClick="btnAbertura_Click" /></li>
                        <li><asp:LinkButton runat="server" ID="btnSuprimento" Text="Suprimento" OnClick="btnSuprimento_Click" /></li>
                        <li><asp:LinkButton runat="server" ID="btnSangria" Text="Sangria" OnClick="btnSangria_Click" /></li>
                        <li><asp:LinkButton runat="server" ID="btnEncerra" Text="Fechamento de Caixa" OnClick="btnEncerra_Click" /></li>
                        <li><asp:LinkButton runat="server" ID="btnCxAbertos" Text="Listagem de Caixas Aberto" OnClick="btnCxAbertos_Click" /></li>

                        <li><a href="VendaPedido.aspx"><i class="fa-fw"></i> Nova Venda</a></li>
                    </ul>
                </div>
                <!-- /.sidebar-collapse -->
            </div>
            <!-- /.navbar-static-side -->
        </nav>

        <div id="page-wrapper">
            <div class="row">
                <div class="col-lg-12">
                    <h3 class="page-header"><asp:Label ID="lblTitulo" runat="server" Text="Bem Vindo !!!"></asp:Label></h3>
                </div>
                <!-- /.col-lg-12 -->
            </div>
            <!-- /.row -->

            <asp:Panel runat="server" ID="pnlDash" Visible="false">
                <div class="row">
                    <div class="col-lg-3 col-md-6">
                       <div class="panel panel-primary">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-3">
                                        <i class="fa  fa-warning fa-3x"></i>
                                    </div>
                                    <div class="col-xs-9 text-right">
                                        <div class="huge">99</div>
                                        <div>Pedidos Abertos</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    <%--</div>--%>
                    <div class="col-lg-3 col-md-6">
                        <div class="panel panel-green">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-3">
                                        <i class="fa  fa-check-circle fa-3x"></i>
                                    </div>
                                    <div class="col-xs-9 text-right">
                                        <div class="huge">99</div>
                                        <div>Vendas Realizadas</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6">
                        <div class="panel panel-yellow">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-3">
                                        <i class="fa  fa-check-circle fa-3x"></i>
                                    </div>
                                    <div class="col-xs-9 text-right">
                                        <div class="huge">302,32</div>
                                        <div>Venda Total</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6">
                        <div class="panel panel-red">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-3">
                                        <i class="fa  fa-times-circle fa-3x"></i>
                                    </div>
                                    <div class="col-xs-9 text-right">
                                        <div class="huge">13</div>
                                        <div>Vendas Canceladas</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-12">
                        <h3>Pedidos</h3>
                    </div>
                        

                            <table class="table table-hover">
                              <tr>
                                <th>ID</th>
                                <th>Descrição</th>
                                <th style="text-align:right">Mesa</th>
                                <th style="text-align:right">Situação</th>
                                <th style="text-align:right">Valor</th>
                              </tr>
                              <tr>
                                <td>1</td>
                                <td>Pedido Nro 1</td>
                                <td style="text-align:right">3</td>
                                <td style="text-align:right">Aberto</td>
                                <td style="text-align:right">100,00</td>
                              </tr>
                              <tr>
                                <td>2</td>
                                <td>Pedido Nro 2</td>
                                <td style="text-align:right">0</td>
                                <td style="text-align:right">Finalizado</td>
                                <td style="text-align:right">100,00</td>
                              </tr>
                              <tr>
                                <td>3</td>
                                <td>Pedido Nro 3</td>
                                <td style="text-align:right">9</td>
                                <td style="text-align:right">Cancelado</td>
                                <td style="text-align:right">10,00</td>
                              </tr>
                            </table>
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlCxsAbertos" Visible="false">
                <div class="row">
                    <div class="col-lg-12">
                        <h3>Caixas Abertos</h3>
                    </div>
                    <table class="table table-hover">
                        <tr>
                        <th>ID</th>
                        <th>Descrição</th>
                        <th>Máquina</th>
                        </tr>
                        <tr>
                        <td>1</td>
                        <td>Caixa Nro 1</td>
                        <td>MARCIO10</td>
                        </tr>
                        <tr>
                        <td>2</td>
                        <td>Caixa Nro 2</td>
                        <td>MARCIO10</td>
                        </tr>
                        <tr>
                        <td>3</td>
                        <td>Caixa Nro 3</td>
                        <td>MARCIO10</td>
                        </tr>
                    </table>
                </div>

                <asp:LinkButton runat="server" ID="btnVoltarPrinc" CssClass ="btn btn-default" Text="Listagem de Caixas Aberto" OnClick="btnVoltarPrinc_Click" />

            </asp:Panel>


        </div>
        <!-- /#page-wrapper -->
    </div>
    <!-- /#wrapper -->

    <!-- jQuery -->
    <script src="../vendor/jquery/jquery.min.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="../vendor/bootstrap/js/bootstrap.min.js"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="../vendor/metisMenu/metisMenu.min.js"></script>

    <!-- Custom Theme JavaScript -->
    <script src="../dist/js/sb-admin-2.js"></script>


</form>
</body>

</html>
