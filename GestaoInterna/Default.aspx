<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GestaoInterna.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link type="text/css" href="Content/bootstrap.css" rel="stylesheet" />
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <link rel="stylesheet" href='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
</head>

<script src="Scripts/jquery-2.1.4.min.js"></script>
<script src="Scripts/bootstrap.min.js"></script>
<script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
<script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

<script type="text/javascript" language="javascript">
    function passaCampo(e, proximoCampo) {
        if (e.keyCode == 13) {
            e.keyCode = 0
            proximoCampo.focus()
        }
    }
</script>
    <%--#ff6a00--%>
<body style ="background-color:#4169E1">
    
<div class="panel panel-default">
  <div class="panel-heading">
    <h3 class="panel-title">Hábil Informática: Software de Aplicação Comercial</h3>
  </div>
  <div class="panel-body">
    Controle de Versões Web. 
  </div>
</div>
    <form id="form1" defaultbutton="btnNada" runat="server">
        <div class="col-md-offset-4 col-md-4">

            <div class="panel panel-default">
                <div class="panel-heading">
                    Identificação do Sistema
                </div>

                <div class="panel-body">
                    <br />
                    <br />
                    <table style="display:flex;justify-content:center;align-items: center;">
                        <tr>
                            <td>
                    <asp:TextBox ID="txtUsuario1" Font-Size="8pt" PlaceHolder="Informe Usuário" Font-Bold="true" runat="server" te CssClass="form-control" MaxLength="20" Width="250"></asp:TextBox>
                    <br />

                            </td>
                        </tr>
                        <tr>
                            <td>
                    <asp:TextBox ID="pwdSenha1" Font-Bold="true" PlaceHolder="Informe a Senha" Font-Size="8pt" CssClass="form-control" runat="server" MaxLength="8" Width="250" TextMode="Password"></asp:TextBox>
                    <br />

                            </td>
                        </tr>
                    </table>
                    <p style="text-align: center">
                        <asp:LinkButton ID="btnEntrar" runat="server" UseSubmitBehavior="false" CssClass="btn btn-primary" OnClick="btnEntrar_Click"> 
                    <span aria-hidden="true" title="Entrar" class="glyphicon glyphicon-ok"></span>  Entrar
                        </asp:LinkButton>
                    </p>
                    <asp:Label ID="lblMensagem" runat="server" Text="" Font-Size="Small" ForeColor="Red"></asp:Label>

                </div>
            </div>
        </div>

        <asp:Button ID="btnNada" Text="btnNada" runat="server" Visible="false" OnClick="btnNada_Click" />
        <br />
    </form>

</body>
</html>
