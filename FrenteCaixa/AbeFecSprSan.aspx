<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AbeFecSprSan.aspx.cs" Inherits="FrenteCaixa.AbeFecSprSan" %>

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

<body style ="background-color:#fff ">

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
                        <i class="fa fa-user fa-fw"></i><i class="fa fa-caret-down"></i>
                        <asp:Label ID="lblNomefuncionario" runat="server"></asp:Label>
                    </a>
                    <ul class="dropdown-menu dropdown-user">
                        <li><a href="#"><i class="fa fa-gear fa-fw"></i>Configurações</a>
                        </li>
                        <li class="divider"></li>
                        <li><a href="login.aspx"><i class="fa fa-sign-out fa-fw"></i>Sair</a>
                        </li>
                    </ul>
                    <!-- /.dropdown-user -->
                </li>
                <!-- /.dropdown -->
            </ul>
            <!-- /.navbar-top-links -->

        </nav>

        <div id="page">
            <form id="frm" runat="server">
                <div class="row" style ="background-color:#fff ">
                    <div class="col-lg-12" >
                        <h1 class="page-header">
                            <asp:Label ID="lblTitulo" runat="server"></asp:Label>
                        </h1>
                    </div>
                </div>
                <!-- /.row -->
                <div class="row" style ="background-color:#fff ">
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-1" style ="background-color:#fff ">
                    </div>
                    <div class="col-lg-6">
                        <div class="panel panel-primary">
                            <div class="panel-heading">Autorização</div>
                            <div class="panel-body">
                                <div class="row" style ="background-color:#fff ">
                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                      <label for="usr" style ="margin-top:1px;">Código do Caixa</label>
                                        <asp:TextBox ID="txtCodCaixa" runat ="server" PlaceHolder="Código" CssClass="form-control" MaxLength ="3" Width="100" ></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row" style ="background-color:#fff ">
                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                      <label for="usr" style ="margin-top:1px;">Código</label>
                                        <asp:TextBox ID="txtCodFuncionario" runat ="server" PlaceHolder="Código" CssClass="form-control"  Width="100" OnTextChanged="txtCodFuncionario_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                    </div>
                                    <div class="col-md-9" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Nome Funcionário</label>
                                        <asp:Label ID="lblDscFuncionario" runat="server" PlaceHolder="Nome Funcionário" CssClass="form-control"  ></asp:Label>
                                    </div>
                                    <br/>
                                </div>
                                <div class="row" style ="background-color:#fff ">
                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Senha</label>
                                        <asp:TextBox ID="txtSenha" runat ="server" CssClass="form-control" PlaceHolder="Senha" TextMode="Password"></asp:TextBox>
                                    </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Panel runat="server" ID="pnlEscondeFormas" Visible="false">
                            <div class="col-lg-5" style ="background-color:#fff ">
                                <table class="table table-hover">
                                    <tr>
                                    <th>Cód.</th>
                                    <th>Forma de Pagamento</th>
                                    <th style="text-align:right">Valor</th>
                                    </tr>
                                    <tr>

                                    <td>1</td>
                                    <td>Dinheiro</td>
                                    <td style="text-align:right"><asp:TextBox ID="txtFormaDinheiro" CssClass="form-control"   Text="0,00" style="text-align:right" runat ="server" ></asp:TextBox></td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <div class="col-lg-1" style ="background-color:#fff ">
                        </div>
                    </div>
                <br/>
                <br/>
                <div class="row" style ="background-color:#fff;text-align:center">
                    <asp:LinkButton runat="server" ID="btnConfirma" Text="Confirmar" CssClass="btn btn-success" OnClick="btnConfirma_Click"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnCancelar" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click"></asp:LinkButton>

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
            </form>


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

</body>

</html>
