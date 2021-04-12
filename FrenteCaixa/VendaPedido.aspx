<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendaPedido.aspx.cs" Inherits="FrenteCaixa.VendaPedido" %>

<!DOCTYPE html>
<html lang="en">

<head>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Sigma: Ponto de Venda</title>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

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
                <a class="navbar-brand">Criação do Pedido</a>
            </div>
            <!-- /.navbar-header -->

            <ul class="nav navbar-top-links navbar-right">
                <li class="dropdown">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                        <i class="fa fa-user fa-fw"></i> <i class="fa fa-caret-down"></i>
                        <asp:Label ID="lblNomefuncionario" runat="server"></asp:Label>
                    </a>
                    <!-- /.dropdown-user -->
                </li>
                <!-- /.dropdown -->
            </ul>
            <!-- /.navbar-top-links -->

            <div class="navbar-default sidebar" role="navigation">
                <div class="sidebar-nav navbar-collapse">
                    <ul class="nav" id="side-menu">
                        <li><asp:LinkButton runat="server" ID="btnCancelar" Text="Cancelar Pedido" CssClass ="btn btn-danger"  OnClick="btnCancelar_Click" /></li>
                        <li><asp:LinkButton runat="server" ID="btnAguardar" Text="Aguardar" CssClass ="btn btn-warning" OnClick="btnAguardar_Click" /></li>
                        <li><asp:LinkButton runat="server" ID="LinkButton1" Text="Produzir" CssClass ="btn btn-success" OnClick="btnAguardar_Click" /></li>
                        <li><asp:LinkButton runat="server" ID="btnPagamento" Text="Pagamento" CssClass ="btn btn-info"  OnClick="btnPagamento_Click" /></li>
                        <li><a href="Principal.aspx"><i class="fa-fw"></i>Voltar a Tela Principal</a></li>
                    </ul>
                </div>
                <!-- /.sidebar-collapse -->
            </div>
            <!-- /.navbar-static-side -->
        </nav>

        <div id="page-wrapper">
            <div class="row">
                <div class="col-lg-12" >
                    <br>
                </div>
                <div class="col-lg-12" >
                    <div class="panel panel-default">
                        <div class="panel-heading">Pedido 1234 - Data: 15/03/2018 Hora: 12:77:99</div>
                        <div class="panel-body" style="text-align:left;">
                            <table>
                                <tr style="" >
                                    <td>
                                        <div class="panel panel-default" style ="padding-left:1px;height:220px;">
                                            <div class="panel-heading">Modalidade do Pedido</div>
                                            <div class="panel-body" style="text-align: left">
                                                <div class="col-sm-12">
                                                    <asp:RadioButtonList runat="server" ID="OpModoPedido">
                                                        <asp:ListItem Text="Presencial" />
                                                        <asp:ListItem Text="Tele_Entrega" />
                                                    </asp:RadioButtonList>
                                                    <br/>

                                                    Mesa:<asp:TextBox runat="server" CssClass="form-control" Width="100"  Id="txtMesa" />   

                                                </div>
                                            </div>
                                        </div>
                                    </td>

                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>

                                    <td>
                                        <div class="panel panel-default" style ="padding-left:1px;height:220px;">
                                            <div class="panel-heading">Cliente</div>
                                            <div class="panel-body" style="text-align: left">
                                                <div class="col-sm-12">
                                                    Nome : <asp:TextBox runat="server" CssClass="form-control"  Text ="" Width="700"  Id="TextBox1" />   
                                                    End : <asp:TextBox runat="server" CssClass="form-control"  Text ="" Width="700"  Id="TextBox3" />   
                                                    <table>
                                                        <tr>
                                                            <td>Fone : <asp:TextBox runat="server" CssClass="form-control"  Text =""   Id="TextBox4" /></td>
                                                            <td>Tipo de Pagamento :
                                                                <asp:DropDownList runat="server" CssClass ="form-control">
                                                                    <asp:ListItem Text="Dinheiro" Selected ="True"  />
                                                                    <asp:ListItem Text="Cartão de Débito" />
                                                                    <asp:ListItem Text="Cheque de Crédito" />
                                                                    <asp:ListItem Text="Cheque" />
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Troco : <asp:TextBox runat="server" CssClass="form-control"  Text =""   Id="txtTroco" /></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td>
                                        <div class="panel panel-default" style ="padding-left:1px;height:100px;">
                                            <div class="panel-heading">Situação</div>
                                            <div class="panel-body" style="text-align: left">
                                                <div class="col-sm-12">
                                                    <asp:TextBox runat="server" CssClass="form-control"  Text ="ABERTO" Font-Size ="XX-Large" BorderStyle ="None" Style="text-align:center;" Id="txtSituacao" />   
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel panel-default" style ="padding-left:1px;height:100px;">
                                            <div class="panel-heading">Valor Total</div>
                                            <div class="panel-body" style="text-align: left">
                                                <div class="col-sm-12">
                                                    <asp:TextBox runat="server" CssClass="form-control"  Text ="R$ 1.000,00" Font-Size ="XX-Large" BorderStyle ="None" Style="text-align:end;" Id="txtVlrTotal" />   
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            
                            Observações:<asp:TextBox runat="server" CssClass="form-control" Id="txtOBS" />   
                            <br/>

                            <table>
                                <tr>
                                    <td style="width: 50%">
                                        <div class="col-sm-12">
                                            <div class="panel panel-default" style ="padding-left:1px;">
                                                <div class="panel-heading">
                                                    Categoria de Produtos
                                                </div>
                                                <div class="panel-body" style="text-align: left">
                                                    <div class="col-sm-12">
                                                        <asp:LinkButton runat="server" ID="btnSubGrupo1" Text="Pastéis Salgados" CssClass="btn btn-warning" Width="140" OnClick="btnSubGrupo1_Click" />
                                                        <asp:LinkButton runat="server" ID="btnSubGrupo2" Text="Pastéis Doces" CssClass="btn btn-info" Width="140" OnClick="btnSubGrupo2_Click" />
                                                        <asp:LinkButton runat="server" ID="btnSubGrupo3" Text="Bebidas" CssClass="btn btn-danger" Width="140" OnClick="btnSubGrupo3_Click" />
                                                        <asp:LinkButton runat="server" ID="btnSubGrupo4" Text="Outros" CssClass="btn btn-success" Width="140" OnClick="btnSubGrupo4_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-12">
                                            <div class="panel panel-default" style ="padding-left:1px;">
                                                <div class="panel-heading">
                                                    Descrição de Produtos
                                                </div>
                                                <div class="panel-body" style="text-align: left">
                                                    <asp:GridView ID="grdGrid" runat="server" Width="100%"
                                                        CssClass="table table-hover"
                                                        GridLines="None" AutoGenerateColumns="False"
                                                        Font-Size="10pt"
                                                        OnSelectedIndexChanged="grdGrid_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:BoundField DataField="QtdeItem" HeaderText="Qtde" />
                                                            <asp:BoundField DataField="CodigoProduto" HeaderText="Código" />
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" />
                                                            <asp:CommandField HeaderText="Adicionar" ButtonType="Button"
                                                                ControlStyle-CssClass="btn btn-primary"
                                                                ShowSelectButton="true"
                                                                ControlStyle-Width=" 30"
                                                                ControlStyle-Height=" 30"
                                                                SelectText="+" />
                                                        </Columns>
                                                        <RowStyle />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>

                                    </td>
                                    <td style="width: 50%">
                                        <div class="col-sm-12">
                                            <div class="panel panel-default" style ="padding-left:1px;">
                                                <div class="panel-heading">
                                                    Itens do Pedido
                                                </div>
                                                <div class="panel-body" style="text-align: left">
                                                    <asp:GridView ID="grdGridItens" runat="server" Width="100%"
                                                        CssClass="table table-hover"
                                                        GridLines="None" AutoGenerateColumns="False"
                                                        Font-Size="10pt">
                                                        <Columns>
                                                            <asp:BoundField DataField="DescricaoProduto" HeaderText="Descrição" />
                                                            <asp:BoundField DataField="QtdeItem" HeaderText="Qtde" />
                                                            <asp:BoundField DataField="qtdeItem" HeaderText="Vlr. Unit" />
                                                            <asp:BoundField DataField="qtdeItem" HeaderText="Vl. Total" />
                                                            <asp:CommandField HeaderText="Excluir" ButtonType="Button"
                                                                ControlStyle-CssClass="btn btn-danger"
                                                                ShowSelectButton="true"
                                                                ControlStyle-Width=" 30"
                                                                ControlStyle-Height=" 30"
                                                                SelectText="-" />
                                                        </Columns>
                                                        <RowStyle />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>


                        </div>
                    </div>
                </div>

            </div>
            <!-- /.row -->

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
