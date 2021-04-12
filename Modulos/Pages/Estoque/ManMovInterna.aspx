<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ManMovInterna.aspx.cs" Inherits="SoftHabilInformatica.Pages.Estoque.ManMovInterna" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
<%--    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />--%>
    <script src="../../Scripts/funcoes.js"></script>
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script>
        $(document).ready(function () {
            $('.js-example-basic-single').select2({
            });
        });
    </script>

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">

        <div class="panel panel-primary">
            <div class="panel-heading">
                Movimentação Interna
                <div class="messagealert" id="alert_container"></div>
            </div>
            <div class="panel-body">
                <asp:LinkButton ID="btnNovo" runat="server" Text="Novo" CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovo_Click"> 
                    <span aria-hidden="true" TabIndex="18" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo
                </asp:LinkButton>
                <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" 
                    TabIndex="17"> 
                    <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                </asp:LinkButton>
                <asp:LinkButton ID="btnSair" runat="server" onFocus="this.select()" Text="Fechar" CssClass="btn btn-info" OnClick="btnSair_Click">
                    <span aria-hidden="true" title="Fechar" TabIndex="19" class="glyphicon glyphicon-off"></span>  Fechar
                </asp:LinkButton>
            <asp:Panel id="pnlGrid" runat="server">
                <div class="row" style="margin-top: 10px">
                    <div class="col-md-3">
                        <label for="usr">Lançamento: </label>
                        <asp:TextBox ID="txtLancamento" CssClass="form-control" name="txtLancamento" Enabled="false" runat="server" TabIndex="1"  />
                    </div>
                    <div class="col-md-6">
                        <label for="usr"> Empresa: </label>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="2"> </asp:DropDownList>		   
                                </ContentTemplate> 
                            <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="SelectedIndexChanged" />
                            </Triggers>
                                </asp:UpdatePanel>
                   </div> 
                    <div class="col-md-3">
                        <label for="usr">Data de Lançamento: </label>
                            <asp:TextBox ID="txtDtLancamento" CssClass="form-control" Enabled="false" runat="server" TabIndex="3"  />
                    </div>               
                </div>
                
                <div class="row" style="margin-top: 10px">
                    <div class="col-md-3">
                        <label for="usr">Documento: </label>
                        <asp:TextBox ID="txtDocumento" CssClass="form-control" runat="server" TabIndex="4"  />
                    </div>           
       
                    <div class="col-md-7" >
                        <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Tipo de Operação: </label>
                        <div class="input-group " style="width:100%">  
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always"> 
                                <ContentTemplate>
                                    <asp:TextBox ID="TxtCdTpOperacao" CssClass="form-control"  runat="server" TabIndex="5" OnTextChanged="TxtCdTpOperacao_TextChanged"  AutoPostBack="true"                                
                                         Width="25%" MaxLength="4" />                                                      
                                    <asp:LinkButton ID="btnTpOperacao"  runat="server" CssClass="form-control btn btn-primary" Width="8%" TabIndex="6" OnClick="btnTpOperacao_Click" AutoPostBack="true"> 
                                        <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                    </asp:LinkButton>
                                    <asp:TextBox ID="Dsctpoperacao" CssClass="form-control"  runat="server"  Enabled="false"  Width="67%"  />
                                </ContentTemplate> 
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="TxtCdTpOperacao" /> 
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                    <div class="col-md-2">
                        <label for="usr">Operação: </label>
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always"> 
                                <ContentTemplate>
                            <asp:TextBox ID="txtOperacao" CssClass="form-control" runat="server" BackColor="Yellow" ForeColor="Red" Font-Bold="true" Enabled="false" AutoPostBack ="true"  />
                    </ContentTemplate> 
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtOperacao" EventName="TextChanged" /> 
                                </Triggers>
                            </asp:UpdatePanel>
                    </div>   
                </div>
                
                <div class="row" style="margin-top: 10px">
            
                    <div class="col-md-12" >
                        <label for="usr" style="background-color: white; border: none; text-align: left; font-size: small ;margin-top:3px" >Produto: </label>
                            <div class="input-group " style="width:100%">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtProduto" CssClass="form-control"  runat="server" TabIndex="7" OnTextChanged="txtProduto_TextChanged" AutoPostBack="true"                                
                                        Width="15%" MaxLength="6" />                                                      
                                            <asp:LinkButton ID="btnPesquisarItem"  runat="server" CssClass="form-control btn btn-primary" Width="5%" TabIndex="8" OnClick="btnPesquisarItem_Click" AutoPostBack="true"> 
                                                <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                            </asp:LinkButton>
                                        <asp:TextBox ID="txtDcrproduto" CssClass="form-control"  runat="server"  Enabled="false"  Width="80%"  AutoPostBack="true"/>
                                    </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtProduto" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtDcrproduto" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                            </div>
                     </div>              
                </div> 

                <div class="row" style="margin-top: 10px">
                    <div class="col-md-6">
                        <label for="usr"> Localização: </label>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlLocalizacao" runat="server" AutoPostBack="true"  OnTextChanged="ddlLocalizacao_TextChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="9"> </asp:DropDownList>		   
                                </ContentTemplate> 
                                <Triggers> 
                                    <asp:AsyncPostBackTrigger ControlID="ddlLocalizacao" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                    </div> 
                    <div class="col-md-6">
                         <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <label for="usr"> Lote: </label>
                                <asp:DropDownList ID="ddlLote" runat="server" AutoPostBack="true" OnTextChanged="ddlLote_TextChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="10"> </asp:DropDownList>		   
                            </ContentTemplate> 
                            <Triggers> 
                                <asp:AsyncPostBackTrigger ControlID="ddlLote" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div> 
                </div> 


                <div class="row" style="margin-top: 10px">
                    <div class="col-md-3">
                        <label for="usr"> Quantidade Movimentada:  </label>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:TextBox ID="TxtQtMovimentada" CssClass="form-control" AutoPostBack="true" Text="0,00" OnTextChanged="TxtQtMovimentada_TextChanged" Font-Size ="Medium" runat="server" TabIndex="11" MaxLength="4" />
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtQtMovimentada" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>        
                    <div class="col-md-3">                        
                        <label for="usr"> Valor Ajuste:  </label>
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:TextBox ID="txtValorAjuste" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtValorAjuste_TextChanged" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="13" MaxLength="4"/>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtValorUnitario" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    
                    <div class="col-md-3">                        
                        <label for="usr"> Custo Unitario:  </label>
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:TextBox ID="TxtValorUnitario" CssClass="form-control" AutoPostBack="true" OnTextChanged="TxtValorUnitario_TextChanged" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="12" MaxLength="4"/>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtValorUnitario" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-3">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <label for="usr"> Valor Total:  </label>
                                <asp:TextBox ID="txtVlTotal" CssClass="form-control" OnTextChanged="txtVlTotal_TextChanged" BackColor="Yellow" ForeColor="Red" Font-Bold="true" AutoPostBack="true" Enabled="false" Font-Size ="Medium" runat="server" Text="0,00" TabIndex="14" MaxLength="4" />
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtVlTotal" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

            <div class="row" style="margin-top: 10px">                
                <div class="col-md-3">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <label for="usr"> Saldo Anterior: </label>
                                <asp:TextBox ID="txtSdlAnterior" CssClass="form-control" OnTextChanged="txtSdlAnterior_TextChanged" BackColor="Yellow" onForeColor="Red" Font-Bold="true" AutoPostBack="true" Enabled="false" Font-Size ="Medium" runat="server" TabIndex="15" MaxLength="4" />
                        </ContentTemplate> 
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtSdlAnterior" EventName="TextChanged" />
                                </Triggers>
                    </asp:UpdatePanel>
                </div>
    
                <div class="col-md-6"></div>
                <div class="col-md-3">
                    <label for="usr"> Saldo Final:</label>
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                            <ContentTemplate>      
                                <asp:TextBox ID="txtSldFinal" CssClass="form-control" OnTextChanged="txtSldFinal_TextChanged" BackColor="Yellow" ForeColor="Red" Font-Bold="true" AutoPostBack="true" Enabled="false" Text="0,00" Font-Size ="Medium" runat="server" TabIndex="16" MaxLength="4" />
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtSldFinal" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>
                <br />  
                <br />  
        <asp:Panel id="pnlNovaLoc" runat="server" Visible="false">
        
            <div class="panel panel-primary">
                <div class="panel-heading">
                 Movimentação de Contra Partida
                    <div class="messagealert" id="bla"></div>
                </div>
                <div class="panel-body">
                    <div class="row" style="margin-top: 10px">
                        <div class="col-md-4">
                        <label for="usr">Nova Empresa: </label>
                            <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlEmpresaCt" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlEmpresaCt_SelectedIndexChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="2"> </asp:DropDownList>		   
                                </ContentTemplate> 
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlEmpresaCt" EventName="SelectedIndexChanged" />
                                    </Triggers>
                            </asp:UpdatePanel>
                        </div> 
                        <div class="col-md-4">
                        <label for="usr">Nova Localização: </label>
                            <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlLocalizacaoCt" runat="server" AutoPostBack="true"  OnTextChanged="ddlLocalizacaoCt_TextChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="9"> </asp:DropDownList>		   
                                </ContentTemplate> 
                                    <Triggers> 
                                        <asp:AsyncPostBackTrigger ControlID="ddlLocalizacaoCt" EventName="TextChanged" />
                                    </Triggers>
                            </asp:UpdatePanel>
                    </div> 
                        <div class="col-md-4">
                         <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <label for="usr">Novo Lote: </label>
                                <asp:DropDownList ID="ddlLoteCt" runat="server" AutoPostBack="true" OnTextChanged="ddlLoteCt_TextChanged" CssClass="form-control js-example-basic-single" Font-Size="Small" TabIndex="10"> </asp:DropDownList>		   
                            </ContentTemplate> 
                                <Triggers>
                                    
                                    <asp:AsyncPostBackTrigger ControlID="ddlLoteCt" EventName="TextChanged" />
                                </Triggers>
                        </asp:UpdatePanel>
                    </div> 
                </div> 
                </div>
            </div>
        </asp:Panel>
        </div>
    </div>
</div>
   
</asp:Content>
