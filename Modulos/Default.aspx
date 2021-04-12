
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HabilInformatica.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Identificação do Sistema</title>
        <link rel="icon" href="images/IconePagina.svg" type="">
        <link type="text/css" href="../Content/EstiloDefinidoPorParametro.css" rel="stylesheet" />
        <link type="text/css" href="Content/bootstrap.css" rel="stylesheet" />
        <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
        <script src="../../Scripts/jsMensagemAlert.js"></script>
    
        <link rel="stylesheet" href='Content/Bootstrap3.0.3/bootstrap.min.css' media="screen" />
    
    </head>

    <script src="Scripts/jquery-2.1.4.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script type="text/javascript" src='Content/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='Content/bootstrap.min.js'></script>

    <script type="text/javascript" language="javascript">
        function passaCampo(e, proximoCampo) {
            if (e.keyCode == 13) {
                e.keyCode = 0
                proximoCampo.focus()
            }
        }
    </script>

    <style>
        .barra-apresentacao-sistema{
            background-color:rgba(255,255,255,0.9);
            padding-left:20px;
            padding-right:20px;
            padding-top:10px;
            padding-bottom:10px;
            color:black;
            margin-bottom:40px;
            height:70px;
            border-bottom:2px solid black;
        }
    </style>
    <%--#ff6a00--%>
    <body class="CorPadraoEscolhida" style="background-image:url('Images/background_default.jpg'); background-size:100% 200%">
        <%--<body style ="background-color:#4169E1" >--%>
    
        <div class="barra-apresentacao-sistema">
            <div style="width:7%;float:left">
                <a href="http://habilinformatica.com.br/" target="_blank"><img src="Images/LogoGR2.png" width="55" height="50" /></a>
            </div>
            <div style="width:93%;float:left">
                <h3 class="panel-title">Hábil Informática: Software de Aplicação Comercial</h3>
                <h5>Sistema Web. Versão 1.0 &emsp;<asp:Label Text="" ID ="lblMaquinaIP" runat="server" /></h5>
            </div>
        </div>

        <form id="form1" defaultbutton="btnNada" runat="server">
            <asp:Panel ID="pnlInicial" runat="server">
                <div class="col-md-offset-1 col-md-5 col-lg-offset-1 col-lg-5" >
                    <div class="panel panel-default" style="background-color:rgba(255,255,255,0.8);min-height:350px">
                        <div class="panel-heading" style="text-align:center; font-size:16px; font-family:'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
                            Licença
                        </div>
                        <div class="panel-body" style="text-align:left;padding-left:25px">
                            <asp:label runat="server" ID="lblInfoLicenca"></asp:label>
                        </div>
                    </div>
                </div>
                <div class="col-md-5 col-lg-5">
                    <div class="panel panel-default" style="background-color:rgba(255,255,255,0.8)!important;height:350px">
                        <div class="panel-heading" style="text-align:center; font-size:16px; font-family:'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
                            Identificação do Sistema
                        </div>
                        <div class="panel-body">
                           <br />
                                <div class="row" style="margin-bottom:10px">
                                    <div class="col-md-2 col-xs-2 "></div>
                                     <div class="input-group col-md-8 col-xs-8" style="height:45px!important">
                                        <span class="input-group-addon glyphicon glyphicon-user" style="background-color:white!important; border-right:0!important;margin-top:0!important;height:100%!important"></span>
                                        <asp:TextBox ID="txtUsuario1" Font-Size="8pt" PlaceHolder="Informe Usuário" Font-Bold="true" runat="server" CssClass="form-control" MaxLength="20" style="border-left:0!important;margin-top:1px!important;height:100%!important;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-xs-2"></div>
                                     <div class="input-group col-md-8 col-xs-8" style="height:45px!important">
                                        <span class="input-group-addon glyphicon glyphicon-lock" style="background-color:white!important; border-right:0!important;height:100%"></span>
                                        <asp:TextBox ID="pwdSenha1" Font-Bold="true" PlaceHolder="Informe a Senha" Font-Size="8pt" CssClass="form-control" runat="server" MaxLength="20" style="border-left:0!important;margin-top:1px!important;height:100%!important; " TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>                                    
                            <br />
                            <p style="text-align: center">
                                <asp:LinkButton ID="btnEntrar" runat="server" CssClass="btn btn-primary" OnClick="btnEntrar_Click" > 
                                    <span aria-hidden="true" title="Entrar" class="glyphicon glyphicon-log-in"></span>  Entrar
                                </asp:LinkButton>
                            
                                <asp:LinkButton ID="btnLicenca" runat="server" UseSubmitBehavior="false" CssClass="btn btn-default" OnClick="btnLicenca_Click" Visible ="false" > 
                                    <span aria-hidden="true" title="Instalar Licenças" class="glyphicon glyphicon-download-alt"></span>  Instalar Licenças
                                </asp:LinkButton>

                                <asp:LinkButton ID="BtnClienteNovo" runat="server" UseSubmitBehavior="false" CssClass="btn btn-success" OnClick="BtnClienteNovo_Click"> 
                                    <span aria-hidden="true" title="Instalar Licenças" class="glyphicon glyphicon-user"></span>  Cliente Novo
                                </asp:LinkButton>
                                <br />
                            
                                <asp:Label ID="lblMensagem" runat="server" Text="" Font-Size="Small" ForeColor="Red" ></asp:Label>
                            </p>  
                            <br />
                            <p style="text-align: center">Ainda não é nosso cliente? <a href="http://habilinformatica.com.br/" target="_blank">Clique aqui!</a></p>
                            <div class="messagealert" id="alert_container" style ="" ></div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlLicenca" runat="server" Visible="false">
                <div class="col-md-offset-3 col-md-6">

                <div class="panel panel-default">
                    <div class="panel-heading">
                        Instalação de Licenças
                    </div>

                    <div class="panel-body">
                        <asp:Label ID="Label1" runat="server" Text="Pode ser que sua Licença tenha sido expirada, por favor entre em contato." Font-Size="Small" ForeColor="Red"></asp:Label>

                        <br />
                        <br />

                        <table style="display:flex;justify-content:center;align-items: center;">
                            <tr>
                                <td>
                                    Cliente :
                                    <asp:TextBox ID="txtCliente" Font-Size="8pt" PlaceHolder="" Font-Bold="true" runat="server" CssClass="form-control" MaxLength="50" Width="600"></asp:TextBox>
                                    <br />
                                </td>
                            </tr>
                            <tr>

                                <td>
                                    Data de Validade: 
                                    <asp:TextBox ID="txtData" Font-Size="8pt" PlaceHolder="" Font-Bold="true" runat="server" CssClass="form-control" MaxLength="10" Width="100"></asp:TextBox>
                                    <br />
                                </td>
                            </tr>
                            <tr>

                                <td>
                                    Arquivo para Autenticação: 
                                    <asp:FileUpload ID="FileUploader" runat="server"  CssClass="form-control" Text=""/>

                                </td>
                            </tr>
                         </table>
                        <br/>
                        <p style="text-align: center">
                            <asp:LinkButton ID="btnVoltar" runat="server" UseSubmitBehavior="false" CssClass="btn btn-primary" OnClick="btnVoltar_Click"> 
                                <span aria-hidden="true" title="Entrar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnInserirLicenca" runat="server" UseSubmitBehavior="false" CssClass="btn btn-success" OnClick="btnInserirLicenca_Click"> 
                                <span aria-hidden="true" title="Instalar Licenças" class="glyphicon glyphicon-ok"></span>  Instalar
                            </asp:LinkButton>
                        </p>
                        <asp:Label ID="Label3" runat="server" Text="" Font-Size="Small" ForeColor="Red"></asp:Label>

                    </div>
                </div>
            </div>
            </asp:Panel>

            <asp:Panel ID="pnlCliNovo" runat="server" Visible="false">
                <div class="col-md-offset-3 col-md-6">

                <div class="panel panel-default">
                    <div class="panel-heading">
                        Implantação de Sistema
                    </div>

                    <div class="panel-body">
                        <asp:Label ID="Label2" runat="server" Text="Pode ser que sua Licença tenha sido expirada, por favor entre em contato." Font-Size="Small" ForeColor="Red"></asp:Label>
                        <br />
                        <br />

                        <table style="display:flex;justify-content:center;align-items: center;">
                            <tr>

                                <td>
                                    Arquivo para Implementação: 
                                    <asp:FileUpload ID="FileUpload1" runat="server"  CssClass="form-control" Text=""/>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Senha do Dia :
                                    <asp:TextBox ID="txtSenhaDoDia" Font-Size="8pt" PlaceHolder="" Font-Bold="true" TextMode="Password" runat="server" CssClass="form-control" MaxLength="50" Width="600"></asp:TextBox>
                                    <br />
                                </td>
                            </tr>

                         </table>
                        <br/>
                        <p style="text-align: center">
                            <asp:LinkButton ID="blnVoltar2" runat="server" UseSubmitBehavior="false" CssClass="btn btn-primary" OnClick="btnVoltar_Click"> 
                                <span aria-hidden="true" title="Entrar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                            </asp:LinkButton>

                            <asp:LinkButton ID="blnImplantarBanco" runat="server" UseSubmitBehavior="false" CssClass="btn btn-success" OnClick="blnImplantarBanco_Click"> 
                                <span aria-hidden="true" title="Instalar Licenças" class="glyphicon glyphicon-ok"></span>  Implantar
                            </asp:LinkButton>
                        </p>
                        <asp:Label ID="Label4" runat="server" Text="" Font-Size="Small" ForeColor="Red"></asp:Label>

                    </div>
                </div>
            </div>
            </asp:Panel>

            <asp:Panel ID="pnlSenha" runat="server" Visible="false">
                <div class="col-md-offset-3 col-md-6">
                    <div class="panel panel-default" style ="width:70%">
                        <div class="panel-heading">Alteração de Senhas</div>
                        <div class="panel-body">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <asp:Label ID="Label6" runat="server" Text="Nova Senha : "></asp:Label></span>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtNovaSenha" TabIndex="13" TextMode="Password" UseSubmitBehavior="false"></asp:TextBox>
                            </div>
                            <br />
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <asp:Label ID="Label7" runat="server" Text="Confirma Senha : "></asp:Label></span>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtConfirmaSenha" TabIndex="14" TextMode="Password" UseSubmitBehavior="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="panel-footing" style ="text-align :right; ">
                            <asp:Label ID="Label5" runat="server" Text="" Font-Size="Small" ForeColor="Red"></asp:Label>
                            <asp:Button ID="btnVoltar2" runat="server" Text="Voltar" CssClass="btn btn-info" AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnVoltar_Click" TabIndex="15"></asp:Button>
                            <asp:Button ID="btnCfmSenhaMesmo" runat="server" Text="Confirma" CssClass="btn btn-success" AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnCfmSenhaMesmo_Click" TabIndex="15"></asp:Button>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Button ID="btnNada" Text="btnNada" runat="server" Visible="false" OnClick="btnNada_Click" />
            <br />
            
        </form>
    </body>
</html>
