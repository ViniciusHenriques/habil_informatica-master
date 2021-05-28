<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" CodeBehind="CadPessoa_Inscricao.aspx.cs" Inherits="SoftHabilInformatica.Pages.Pessoas.CadPessoa_Inscricao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap.min.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtdtabertura']").datepicker({
                todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",
                locate: "pt-BR"});


            $("input[id*='txtdtencerramento']").datepicker({
                todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",
                locate: "pt-BR"
            });


            $('.js-example-basic-single').select2({});
        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});           
        }
    </script>
    <style>
        .ddlstyle
        {
            width: 250px;
        }
        option
        {
            padding-left: 40px;
            font-size: 16px;
        }
    </style>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <div id="divNavTeste" style="padding-left: 30px; padding-top: 30px; padding-right: 30px; font-size: small;">
        <div class="panel panel-primary">
            <div class="panel-heading">Cadastro de Pessoas</div>

            <div class="panel-body" style="padding-left: 30px; padding-top: 30px; padding-right: 30px; font-size: small;">
                <div class="container-fluid">
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Pessoa (<asp:Label ID="txtCodPessoa" runat="server"/>)</label>
                            <asp:Label ID="txtRazSocial" runat="server" CssClass="form-control" />
                        </div>
                    </div>                            
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        Inscrição da Pessoa
                        <div class="messagealert" id="alert_container"></div>
                    </div>
                    <div class="panel-body">
                        <asp:LinkButton ID="btnCanInscricao" runat="server" Text="Excluir" CssClass="btn btn-default" UseSubmitBehavior="false" Visible="true" OnClick="btnCanInscricao_Click"> 
                            <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-default"></span>  Cancelar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnSlvInscricao" runat="server" Text="Salvar" CssClass="btn btn-success" AutoPostBack="true" Visible="true" OnClick="btnSlvInscricao_Click"> 
                            <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnExcInscricao" runat="server" Text="Excluir" CssClass="btn btn-danger" UseSubmitBehavior="false" Visible="false" OnClick="btnExcInscricao_Click"> 
                            <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                        </asp:LinkButton>

                        <br />
                        <br />


                        <div class="container-fluid">
                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Item</label>
                                    <asp:TextBox ID="txtInsItem" runat="server" CssClass="form-control" Enabled="false" Text="" ></asp:TextBox>
                                </div>
                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <label for="usr" style ="margin-top:1px;">País</label>
                                            <asp:DropDownList ID="ddlPaises" runat="server" CssClass="form-control js-example-basic-single" TabIndex="0"></asp:DropDownList>
                                            </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlPaises" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">CPF/CNPJ</label>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtCNPJCPF" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="txtCNPJCPF_TextChanged" MaxLength="15" TabIndex="1"></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCNPJCPF" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <label for="usr" style ="margin-top:1px;">Tipo de Pessoa</label>
                                            <asp:DropDownList ID="ddlTipoInscricao" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlTipoInscricao_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCNPJCPF" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>

                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Inscrição Estadual  / RG</label>
                                    <asp:TextBox ID="txtIERG" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </div>

                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Inscrição Municipal</label>
                                    <asp:TextBox ID="txtIM" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="background-color:white;border:none;">
                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Data Abertura/Nascimento</label>
                                    <asp:TextBox ID="txtdtabertura" name="txtdtabertura" runat="server" CssClass="form-control" TabIndex="4" />
                                </div>
                                <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">Data Encerramento</label>
                                    <asp:TextBox ID="txtdtencerramento" name="txtdtencerramento" runat="server" CssClass="form-control " TabIndex="5"></asp:TextBox>
                                </div>
                                <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                    <label for="usr" style ="margin-top:1px;">OBS</label>
                                    <asp:TextBox ID="txtOBSInscr" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cttRodape" runat="server">



</asp:Content>

