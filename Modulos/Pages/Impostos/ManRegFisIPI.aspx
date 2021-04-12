<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ManRegFisIPI.aspx.cs" Inherits="SoftHabilInformatica.Pages.Impostos.ManRegFisIPI" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<%--    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />--%>
    <script src="../../Scripts/funcoes.js"></script>

    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    <script src="../../Scripts/jquery.maskedinput.min.js"></script>

     <script type="text/javascript">
        $(document).ready(function () {
            $("input[id*='txtDtVigencia']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });
            
         });
        jQuery(function ($) {
            $("#txtCodNcm").mask("9999.99.99");
         });
        function abc()
        {
            $("#CdCaixas .modal-title").html("");
            $("#CdCaixas").modal("show");
        }
    </script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Manuntenção Regra Fiscal de IPI
                    <div class="messagealert" id="alert_container"></div>
            </div>
            <div class="panel-body">
                <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" UseSubmitBehavior="false" Visible="true" OnClick="btnVoltar_Click"> 
                    <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                </asp:LinkButton>
                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" TabIndex="17"> 
                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>
                <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" OnClick="btnExcluir_Click" TabIndex="10"> 
                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                </asp:LinkButton>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-1" style="padding-right:0!important">
                                <label for="usr">Código Regra</label>
                                <asp:TextBox ID="txtCodRegra" CssClass="form-control" name="txt" Enabled="false" runat="server" TabIndex="-1"  />
                            </div>
                            <div class="col-md-2" style="padding-right:0!important">
                                <label for="usr">Dt. Vigência</label>
                                <asp:TextBox ID="txtDtVigencia" CssClass="form-control" name="txt" runat="server" TabIndex="10"  />
                            </div>
                            <div class="col-md-2">
                                <label for="usr">Dt. Atualizacão</label>
                                <asp:TextBox ID="txtDtAtualizacao" CssClass="form-control" Enabled="false" name="txt" runat="server" TabIndex="10"  />
                            </div>
                            <div class="col-md-2">
                                <label for="usr">Situação Tributaria</label>
                                <asp:TextBox ID="txtCodTrib" CssClass="form-control" name="txt" runat="server" MaxLength="2" TabIndex="10"  />
                            </div>
                            <div class="col-md-2" style="padding-right:0!important">
                                <label for="usr">Código de Exceção</label>
                                <div class="input-group " style="width:100%">  
                                    <asp:TextBox ID="TxtEx" CssClass="form-control" Font-Bold="true" style="border-right:unset"  Width="25%" runat="server" TabIndex="5" Enabled="false"  AutoPostBack="false" MaxLength="4" />                                                      
                                    <asp:TextBox ID="txtCodEx" CssClass="form-control" MaxLength="2"  runat="server" style="border-left:unset" BackColor="Transparent" Width="75%"  />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label for="usr">Situação</label>
                                <asp:DropDownList ID="ddlSituacao" CssClass="form-control" AutoPostBack="true" name="txt" runat="server" TabIndex="10"  />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-3" style="padding-right:0!important">
                                <label for="usr">Código NCM</label>
                                <asp:TextBox ID="txtCodNcm" CssClass="form-control" Font-Size="Medium" ClientIDMode="Static" Font-Bold="true" name="txt" runat="server" TabIndex="10"  />
                            </div>
                            <div class="col-md-12">
                                <label for="usr">Descrição NCM</label>
                                <asp:TextBox ID="txtDsNcm" CssClass="form-control" TextMode="multiline" Columns="10" Rows="5" MaxLength="255" name="txt" runat="server" TabIndex="10"  />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-4" style="padding-right:0!important">
                                <label for="usr">Tipo de IPI</label>
                                <asp:DropDownList ID="ddlTipo" CssClass="form-control" AutoPostBack="true" name="txt" runat="server" TabIndex="10"  />
                            </div>
                            <div class="col-md-4" style="padding-right:0!important">
                                <label for="usr">Percentual de IPI</label>
                                <asp:TextBox ID="txtVlPctIpi" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtVlPctIpi_TextChanged" name="txt" runat="server" TabIndex="10"  />
                            </div>
                            <div class="col-md-4">
                                <label for="usr">Código Enquadramento</label>
                                <asp:TextBox ID="txtEnquadramento" CssClass="form-control" name="txt" runat="server" MaxLength="3" TabIndex="10"  />
                            </div>
                        </div>
                    </ContentTemplate> 
                    <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtVlPctIpi" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlSituacao"/>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipo"/>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
       <!-- Exclui COFINS -->
  <asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Always">
            <ContentTemplate>
                <div class="modal fade" id="CdCaixas" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                    <div class="modal-dialog" role="document" style ="height:100%;width:300px">
                        <div class="modal-content" >
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title" id="H3">Atenção</h4>
                            </div>
                            <div class="modal-body">
                                Deseja Excluir esta Regra de IPI?
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click" > 
                                    <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                                </asp:LinkButton>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
