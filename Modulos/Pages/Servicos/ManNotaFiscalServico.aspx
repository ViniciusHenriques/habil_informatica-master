<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" 
    CodeBehind="ManNotaFiscalServico.aspx.cs" Inherits="SoftHabilInformatica.Pages.Servicos.ManNotaFiscalServico"  
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server" >

    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>

    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <script src="../..Scripts/jquery-2.1.4.js"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/GridViewPager.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="../../Content/DateTimePicker/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../../Content/DateTimePicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Content/DateTimePicker/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    <script>
        item = '#<%=PanelSelect%>';
        $(document).ready(function (e) {
            $('#myTabs a[href="' + item + '"]').tab('show');
            console.log($(item));

            $('.js-example-basic-single').select2({
               
            });

            $("input[id*='txtDt1']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt2']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt3']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt4']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt5']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt6']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt7']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt8']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt9']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt10']").datepicker({todayBtn: "linked",clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
        });

        
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});
           
            $("input[id*='txtDt1']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt2']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt3']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt4']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt5']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt6']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt7']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt8']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt9']").datepicker({todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR",locate: "pt-BR"});
            $("input[id*='txtDt10']").datepicker({ todayBtn: "linked", clearBtn: true, format: "dd/mm/yyyy", autoclose: "false", language: "pt-BR", locate: "pt-BR" });

            
        }
        
    </script>
    <style type="text/css">
        @media screen and (max-width: 600px) {
            .noprint{display:none;}  
        
        }
        @media screen and (max-width: 1000px) {
            .acao{
                margin-top:20px!important;
                
            }  
              
            #consulta3,#consulta1,#consulta2{
                padding-left:0px!important;
            }
            
        }
        .valor{
            border-left:0!important;
        }
         .centerHeaderText{
            text-align: center!important;
        }
    </style>
    

    <div id="divNavTeste" style="padding-left: 20px; padding-top: 20px; padding-right: 20px;"  >
        <div class="panel panel-primary ">
            <div class="panel-heading panel-heading-padrao" >

                Manutenção do Nota Fiscal de Serviço
                <div class="messagealert" id="alert_container"></div>
                <asp:Panel ID="pnlMensagem" Visible="false" Style="margin-left: 10px; margin-right: 10px;" runat="server">
                    <div id="divmensa" style="background-color: red; border-radius: 15px;">
                        <asp:Label ID="lblMensagem" runat="server" ForeColor="Black"></asp:Label>
                        <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick="BtnOkExcluir_Click" />
                    </div>
                </asp:Panel>

            </div>
            <div class="panel-body" >
                    <div class="row" style="background-color: white; border: none;">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="col-md-8" style="background-color: white; border: none; text-align: left; font-size: small;">
                            
                                    <asp:LinkButton ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-info" TabIndex="0" UseSubmitBehavior="false" OnClick="btnVoltar_Click" ToolTip="Ao voltar, TODAS alterações feitas no documento será perdida, inclusive novos serviços e anexos!"> 
                                        <span aria-hidden="true" title="Voltar" class="glyphicon glyphicon-arrow-left"></span>  Voltar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" TabIndex="1" UseSubmitBehavior="false"  OnClick="btnSalvar_Click"> 
                                        <span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnExcluir" runat="server" Text="Excluir" CssClass="btn btn-danger" data-toggle="modal" data-target="#myexcpes" > 
                                        <span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                                    </asp:LinkButton>

                                </div>

                                <div class="col-md-4 " style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                    <label for="usr" style ="margin-top:-5px;" class="acao">Ação</label>
                                    <asp:DropDownList ID="ddlAcao" runat="server" AutoPostBack="True" CssClass="form-control js-example-basic-single" TabIndex="2" Font-Size="x-Small">
                                        <asp:ListItem Text="Salvar Registro" Selected="True" Value="0" />
                                        <asp:ListItem Text="Salvar Registro, Continuar Editando" Value="1" />
                                        <asp:ListItem Text="Salvar Registro, Continuar Incluindo" Value="2" />
                                    </asp:DropDownList>
                                </div>
                            </ContentTemplate> 
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnSalvar" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlAcao" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                <div class="container-fluid">
                    <div class="row" style="background-color: white; border: none; margin-top:10px!important">
                        <asp:HiddenField ID="TabName" runat="server"/>            
                        <div id="Tabs" role="tabpanel" style="background-color: white; border: none; text-align: left; font-size: small;" >

                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                <ContentTemplate>  
                                    <ul class="nav nav-tabs" role="tablist" id="myTabs" >
                                        <li role="presentation"><a href="#home" aria-controls="home" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-home"></span>&nbsp;&nbsp;Geral</a></li>
                                        <li role="presentation"><a href="#consulta1" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-user"></span>&nbsp;&nbsp;Dados Tomador</a></li>
                                        <li role="presentation" style="<%=TabEventos%>"><a href="#consulta2" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Eventos&nbsp;&nbsp;</a></li>
                                        <li role="presentation" style="<%=TabLogs%>"><a href="#consulta3" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-check"></span>&nbsp;&nbsp;Logs&nbsp;&nbsp;</a></li>
                                        <li role="presentation" style="<%=TabAnexos%>"><a href="#consulta4" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Anexos&nbsp;&nbsp;</a></li>
                                        <li role="presentation"><a href="#consulta5" aria-controls="profile" role="tab" data-toggle="tab" class="tab-select"><span class="glyphicon glyphicon-usd"></span>&nbsp;&nbsp;Pagamento</a></li>
                                    </ul>
                                </ContentTemplate> 
                            </asp:UpdatePanel>
                            <!-- Tab panes -->
                            <div class="tab-content" runat="server" id="PanelContext">                                
                                <div role="tabpanel" class="tab-pane" id="home" style="padding-left: 10px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Código</label>
                                                <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" Enabled="false" Text ="" Font-Size="Small" TabIndex="2"/>
                                            </div>                                       
                                            <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Data Lançamento</label>
                                                <asp:TextBox ID="txtdtentrada" name="txtdtentrada" AutoPostBack="False" CssClass="form-control" Enabled="false" TabIndex="3" runat="server" />
                                            </div>
                                            <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Data Emissão</label>
                                                <asp:TextBox ID="txtdtemissao" CssClass="form-control"  TabIndex="4" runat="server" name="txtdtemissao"  AutoPostBack="true" Enabled="false" />
                                            </div>                                                                      
                                            <div class="col-md-3" style="background-color: white; border: none; text-align: left; font-size:x-small;">
                                                <label for="usr" style ="margin-top:1px;">Situação</label>
                                                <asp:DropDownList ID="ddlSituacao" runat="server" Enabled="false"  Width="100%" TabIndex="5" CssClass="form-control js-example-basic-single"  Font-Size="Small"/>
                                            </div>
                                            <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size:x-small;margin-top:3px">
                                                <label for="usr" style ="margin-top:1px;">Tipo Operação</label>
                                                <asp:DropDownList ID="ddlTipoOperacao" runat="server"   TabIndex="6"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" />
                                            </div>
                                            <div class="col-md-6" style="background-color: white; border: none; text-align: left; font-size:x-small;margin-top:3px">
                                                <label for="usr" style ="margin-top:1px;">Empresa Prestador</label>
                                                <asp:DropDownList ID="ddlEmpresa" runat="server"   TabIndex="6"  Width="100%" CssClass="form-control js-example-basic-single" Font-Size="Small" OnTextChanged="ddlEmpresa_TextChanged"   AutoPostBack="true"/>
                                            </div>
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                <label for="usr" style ="margin-top:1px;">Número Documento</label>
                                                <asp:TextBox ID="txtNroDocumento" AutoPostBack="False" CssClass="form-control"  TabIndex="7" runat="server" />
                                            </div>
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:3px">
                                                <label for="usr" style ="margin-top:1px;">Série</label>
                                                <asp:TextBox ID="txtNroSerie" AutoPostBack="False" CssClass="form-control"  TabIndex="8" runat="server" />
                                            </div>                                   
                                            <div class="col-md-8" >
                                                <label for="usr" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:4px">Código Tomador</label>
                                                <div class="input-group " style="width:100%">                              
                                                    <asp:TextBox ID="txtCodTomador" CssClass="form-control" name="txtCodCredor"  runat="server" TabIndex="9" OnTextChanged="SelectedTomador"   AutoPostBack="true"                                
                                                    Width="15%" MaxLength="6" />
                                                      
                                                    <asp:LinkButton ID="LinkButton1" title="Pesquise os Forncedores" runat="server" CssClass="form-control btn btn-primary" Width="10%"  OnClick="ConTomador" AutoPostBack="true"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-search"></span>
                                                    </asp:LinkButton>
                                                    <asp:TextBox ID="txtCredor" CssClass="form-control" name="txtCodPessoa"  runat="server"  Enabled="false"  Width="75%"  />

                                                </div>
                                            </div>                                  
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; margin-top:20px    ">
                                               
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">Tributação</div>
                                                    <div class="panel-body">
                                                        <div class="row" style="background-color:white;border:none;">


                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <label for="usr">Aliquota %</label>
                                                                <asp:TextBox ID="txtAliquota"  CssClass="form-control" TabIndex="10" runat="server" MaxLength="50" Text ="0,00"  OnTextChanged="txtAliquota_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                            </div>

                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <label for="usr">Estado </label>
                                                                <asp:DropDownList ID="ddlEstado" runat="server" TabIndex="11" CssClass="form-control  js-example-basic-single" Width="100%" AutoPostBack="true" OnTextChanged="ddlEstado_TextChanged"></asp:DropDownList>                                                
                                                            </div> 
                                                        
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <label for="usr">Municipio Prestador</label>
                                                                <asp:DropDownList ID="ddlMunicipio" runat="server" TabIndex="12" CssClass="form-control  js-example-basic-single" Width="100%" ></asp:DropDownList>                                                
                                                            </div>

                                                        </div>              
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; margin-top:20px    ">                                              
                                                   
                                                <div class="panel panel-default">

                                                    <div class="panel-heading">Serviços</div>
                                                    <div class="panel-body">
                                                
                                                        <div class="row" style="margin-bottom:15px">
                                                            <div class="col-md-11" style="background-color:white;border:none;text-align:left;font-size:x-small;">

                                                                <label for="usr">Tipo Serviço </label>
                                                                <asp:DropDownList ID="ddlTipoServico" runat="server" TabIndex="13" CssClass="form-control js-example-basic-single" Width="100%" AutoPostBack="false"></asp:DropDownList>
                                                
                                                            </div>
                                        
                                                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small; margin-top:18px">
                                                                <asp:LinkButton ID="BtnAddServico" title="Adicionar Serviço" runat="server" Text="Adicionar" TabIndex="14" CssClass="btn btn-success" OnClick="BtnAddServico_Click" style="height:33px!important;padding-top:7px"> 
                                                                <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                
                                                        <asp:GridView ID="grdServicos" runat="server" 
                                                            CssClass="table table-bordered table-hover table-striped"
                                                            GridLines="None" AutoGenerateColumns="False"
                                                            Font-Size="8pt" 
                                                            OnSelectedIndexChanged="grdServicos_SelectedIndexChanged">
                                                            <Columns>
                                                                <asp:BoundField DataField="CodigoServico" HeaderText="Código" />
                                                                <asp:BoundField DataField="DescricaoTipoServico" HeaderText="Descrição"  />
                                                                <asp:BoundField DataField="CodigoCNAE" HeaderText="Código CNAE" />
                                                                <asp:BoundField DataField="CodigoServicoLei" HeaderText="Código Lei 116" />
                                                                <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                                                                                ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                                                                ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                                                                ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                                            </Columns>
                                                            <RowStyle CssClass="cursor-pointer" />
                                                        </asp:GridView>
                                                        
                                                        </div>
                                                        <div class="panel-heading" style="border-radius:0!important;border-top: 1px solid #ccc!important;border-bottom: 1px solid #ccc!important;">Itens do Serviço</div>
                                                            <div class="panel-body">
                                                   
                                                                <div class="row" style="margin-bottom:15px">
                                                                
                                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">

                                                                        <label for="usr">Descrição: </label>
                                                                        <asp:DropDownList ID="ddlProduto" runat="server" TabIndex="15" CssClass="form-control js-example-basic-single" Width="100%"  ></asp:DropDownList>
                                                
                                                                    </div>
                                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">

                                                                        <label for="usr">Tipo Serviço: </label>
                                                                        <asp:DropDownList ID="ddlTipoServicoAdicionado" runat="server" TabIndex="16" CssClass="form-control js-example-basic-single " Width="100%"  ></asp:DropDownList>
                                                
                                                                    </div>
                                                                    <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small;">

                                                                        <label for="usr">Quantidade: </label>
                                                                        <asp:TextBox ID="txtQtde"  CssClass="form-control" name="txtQtde" TabIndex="17" runat="server" Text="0,00" MaxLength="50"   AutoPostBack="true" onFocus="this.select()"/>

                                                                    </div>
                                                                    <div class="col-md-1" style="background-color:white;border:none;text-align:left;font-size:x-small; " >

                                                                        <label for="usr">Preço: </label>
                                                                        <asp:TextBox ID="txtPreco" Text="0,00" CssClass="form-control" name="txtPreco" TabIndex="18" runat="server" MaxLength="50"  AutoPostBack="true"  onFocus="this.select()"/>

                                                                    </div>
                                                        

                                                                    <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:18px">
                                                                        <asp:LinkButton ID="BtnAddProduto" runat="server" title="Adicionar/Alterar Produto" TabIndex="20" CssClass="btn btn-success"  OnClick="BtnAddProduto_Click" style="height:33px!important;padding-top:7px"> 
                                                                            <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="BtnExcluirProduto" runat="server" Text="Adicionar" title="Excluir Produto" TabIndex="19" CssClass="btn btn-danger"  OnClick="BtnExcluirProduto_Click" style="height:33px!important;padding-top:7px"> 
                                                                            <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                        </asp:LinkButton>                                                   
                                                            
                                                                    </div>
                                                                        <div class="col-md-2" style="background-color:white;border:none;text-align:left;font-size:x-small;">

                                                                        <label for="usr">Valor Total: </label>
                                                                        <asp:TextBox ID="txtVlrTotal"  CssClass="form-control" Text="0,00"  name="txtPreco" TabIndex="21" runat="server" MaxLength="50" Enabled="false"    />

                                                                    </div>        
                                                                </div>
                                                    
                                                                <asp:GridView ID="grdProduto" runat="server" 
                                                                    Width="100%"
                                                                    CssClass="table table-bordered table-hover table-striped"
                                                                    GridLines="None" AutoGenerateColumns="False"
                                                                    Font-Size="8pt" 
                                                                    OnSelectedIndexChanged="grdProduto_SelectedIndexChanged">

                                                                    <Columns>
                                                                        <asp:BoundField DataField="CodigoProdutoDocumento" HeaderText="Código" />
                                                                        <asp:BoundField DataField="CodigoServico" HeaderText="Código do Serviço" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" />
                                                                        <asp:BoundField DataField="Cpl_DscProduto" HeaderText="Descrição"  />
                                                                        <asp:BoundField DataField="Quantidade" HeaderText="Quantidade"  DataFormatString="{0:F}"/>
                                                                        <asp:BoundField DataField="PrecoItem" HeaderText="Preço"  DataFormatString="{0:C}"/>

                                                                     
                                                                        <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                                                                ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                                                ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                                                                ControlStyle-Width ="25px" ControlStyle-Height ="25px" /> 
                                                                    
                                                                    </Columns>

                                                                <RowStyle CssClass="cursor-pointer" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                            </div>
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; margin-top:20px    ">
                                               
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">Retenções Federais</div>
                                                    <div class="panel-body">
                                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                            <label for="usr" style ="margin-top:1px;">PIS R$</label>
                                                            <asp:TextBox ID="txtPIS" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="22" OnTextChanged="txtPIS_TextChanged" AutoPostBack="true"  onFocus="this.select()" />
                                                        </div>
                                
                                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                            <label for="usr" style ="margin-top:1px;">Cofins R$</label>
                                                            <asp:TextBox ID="txtCofins" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="23" OnTextChanged="txtCofins_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                        </div>
                                    
                
                                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                            <label for="usr" style ="margin-top:1px;">CSLL R$</label>
                                                            <asp:TextBox ID="txtCSLL" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="24" OnTextChanged="txtCSLL_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                        </div>

                                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                            <label for="usr" style ="margin-top:1px;">IRRF R$</label>
                                                            <asp:TextBox ID="txtIRRF" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="25" OnTextChanged="txtIRRF_TextChanged" AutoPostBack="true" onFocus="this.select()"  />
                                                        </div>
                                
                                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                            <label for="usr" style ="margin-top:1px;">INSS R$</label>
                                                            <asp:TextBox ID="txtINSS" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="26" OnTextChanged="txtINSS_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                        </div>
                                    
                
                                                        <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                            <label for="usr" style ="margin-top:1px;">Outras Retenções</label>
                                                            <asp:TextBox ID="txtOutras" CssClass="form-control" runat="server" Enabled="true" Text ="0,00" TabIndex="27" OnTextChanged="txtOutras_TextChanged" AutoPostBack="true" onFocus="this.select()" />
                                                        </div>   
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: small; margin-top:20px    ">
                                               
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">Descrição Geral do Serviço</div>
                                                    <div class="panel-body">
                                                        <asp:TextBox ID="txtDescricao" CssClass="form-control" runat="server"  TabIndex="28" TextMode="multiline" Columns="10" Rows="5" MaxLength="300" onFocus="this.select()" />  
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCodigo" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlTipoServico" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="BtnAddServico" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlProduto" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlTipoServicoAdicionado" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="BtnAddProduto" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="txtQtde" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtPreco" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtPIS" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCofins" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCSLL" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtIRRF" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtINSS" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtOutras" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtAliquota" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlEstado" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlMunicipio" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCodTomador" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCredor" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>                                
                                <div role="tabpanel" class="tab-pane" id="consulta1" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">CNPJ/CPF</label>
                                                <asp:TextBox ID="txtCNPJCPFCredor" CssClass="form-control" runat="server" Enabled="false" Text =""  />
                                            </div>
                                
                                            <div class="col-md-10" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Razão Social</label>
                                                <asp:TextBox ID="txtRazSocial" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                    
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Endereço</label>
                                                <asp:TextBox ID="txtEndereco" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                    
                                            <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">CEP</label>
                                                <asp:TextBox ID="txtCEP" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>

                                            <div class="col-md-1" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Estado</label>
                                                <asp:TextBox ID="txtEstado" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>

                                            <div class="col-md-5" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Cidade</label>
                                                <asp:TextBox ID="txtCidade" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>

                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">Bairro</label>
                                                <asp:TextBox ID="txtBairro" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                            <div class="col-md-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">
                                                <label for="usr" style ="margin-top:1px;">E-mail NFS-e</label>
                                                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" Enabled="false" Text ="" />
                                            </div>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCNPJCPFCredor" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtRazSocial" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtEndereco" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCEP" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtEstado" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCidade" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtBairro" EventName="TextChanged" />

                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta2" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                        <ContentTemplate>

                                            <asp:GridView ID="GrdEventoDocumento" runat="server" AutoPostBack="false"
                                                CssClass="table  table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt" Visible="true">
                                                     
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoEvento" HeaderText="Código" />
                                                    <asp:BoundField DataField="DataHoraEvento" HeaderText="Data" />
                                                    <asp:BoundField DataField="Cpl_NomeMaquina" HeaderText="Máquina" />
                                                    <asp:BoundField DataField="Cpl_NomeUsuario" HeaderText="Usuário" />
                                                    <asp:BoundField DataField="Cpl_Situacao" HeaderText="Situação" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>                                                                                                

                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="GrdEventoDocumento" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta3" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdLogDocumento" runat="server" CssClass="table  table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  
                                                OnSelectedIndexChanged="grdLogDocumento_SelectedIndexChanged"
                                                PageSize="10" AllowPaging="true"
                                                OnPageIndexChanging="grdLogDocumento_PageIndexChanging"
                                                PagerSettings-Mode ="NumericFirstLast" >
                                                <PagerStyle HorizontalAlign = "Right"   CssClass = "GridPager" />
                                                <Columns>

                                                    <asp:BoundField DataField="DataHora" HeaderText="Data" />
                                                    <asp:BoundField DataField="EstacaoNome" HeaderText="Máquina" />
                                                    <asp:BoundField DataField="UsuarioNome" HeaderText="Usuário" />
                                                    <asp:BoundField DataField="Cpl_DescricaoOperacao" HeaderText="Operação" />
                                                    <asp:BoundField DataField="DescricaoLog" HeaderText="Descrição" />
                                                                

                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="grdLogDocumento"  />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="consulta4" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btnNovoAnexo" runat="server" style="padding-left:20px; margin-bottom:10px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnNovoAnexo_Click" TabIndex="21"> 
                                                <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Novo Anexo&nbsp;
                                            </asp:LinkButton>
                                            <br />
                                            <asp:GridView ID="grdAnexo" runat="server"
                                                CssClass="table  table-striped"
                                                GridLines="None" AutoGenerateColumns="False"
                                                Font-Size="8pt"  Visible="true"
                                                OnSelectedIndexChanged="grdAnexo_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="CodigoAnexo" HeaderText="#"  />
                                                    <asp:BoundField DataField="DataHoraLancamento" HeaderText="Data/Hora Lançamento"  />
                                                    <asp:BoundField DataField="Cpl_Maquina" HeaderText="Estação" />
                                                    <asp:BoundField DataField="Cpl_Usuario" HeaderText="Usuário"  />
                                                    <asp:BoundField DataField="DescricaoArquivo" HeaderText="Descrição"/>
                                                    <asp:BoundField DataField="NomeArquivo" HeaderText="Nome"/>
                                                    <asp:BoundField DataField="ExtensaoArquivo" HeaderText="Extensão" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                                                    <asp:CommandField HeaderText="Ação" ShowSelectButton="True" 
                                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" 
                                                        ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                                        ControlStyle-Width ="25px" ControlStyle-Height ="25px" />
                                                </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                                            </asp:GridView>
                                         </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnNovoAnexo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="grdAnexo" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>  
                                <div role="tabpanel" class="tab-pane" id="consulta5" style="padding-left: 20px; padding-top: 20px; padding-right: 20px; font-size: small;">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                        <ContentTemplate>

                                            <div class="row">
                                                <div class="col-lg-9 col-md-8 col-xs-12" style="background-color: white; border: none; text-align: left; font-size: x-small;">

                                                    <label for="usr">Condição de Pagamento: </label>
                                                    <asp:DropDownList ID="ddlPagamento" runat="server" TabIndex="16" CssClass="form-control js-example-basic-single " Width="100%" OnTextChanged="ddlPagamento_TextChanged1" AutoPostBack="true" ></asp:DropDownList>
                                                
                                                </div>
                                                <div class="col-lg-1 col-md-2 col-xs-6">
                                                    <asp:LinkButton ID="btnLimpar" runat="server" style=" margin-top:19px"  CssClass="btn btn-default" UseSubmitBehavior="false" OnClick="btnLimpar_Click" TabIndex="21"> 
                                                        <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-repeat"></span>  Limpar
                                                    </asp:LinkButton>
                                                </div>
                                                <div class="col-lg-2 col-md-2 col-xs-6">
                                                
                                                    <asp:LinkButton ID="btnGerarParcela" runat="server" style=" margin-top:19px"  CssClass="btn btn-info" UseSubmitBehavior="false" OnClick="btnGerarParcela_Click" TabIndex="21"> 
                                                        <span aria-hidden="true" title="Novo" class="glyphicon glyphicon-edit"></span>  Gerar Parcelas
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-top:10px;" >
                                                <div class="col-lg-2 col-md-3 col-xs-6">
                                                    <asp:panel runat="server" id="pnlAddTodos" Visible="false" Width="100%">
                                                    
                                                        <asp:LinkButton ID="btnAddTodas" runat="server" title="Adicionar/Alterar Todas Parcelas" TabIndex="20" Width="100%" CssClass="btn btn-success"  OnClick="btnAddTodas_Click" > 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span> Adicionar Todas
                                                        </asp:LinkButton>
                                                    
                                                    </asp:panel>
                                                </div>
                                                <div class="col-lg-2 col-md-3 col-xs-6">
                                                    <asp:panel runat="server" id="pnlRemoverTodos" Visible="false" Width="100%">

                                                    
                                                        <asp:LinkButton ID="btnRemoverTodas" runat="server" title="Remover Todas Parcelas" TabIndex="20" CssClass="btn btn-danger"  Width="100%" OnClick="btnRemoverTodas_Click" > 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span> Remover Todas
                                                        </asp:LinkButton>
                                                    
                                                    </asp:panel>
                                                </div>

                                            </div>
                                            <div class="row">
                                                 
                                                <div class="col-lg-8 col-md-12" style="margin-top:20px;">
                                                    <asp:Panel ID="pnlParcela1" runat="server" Visible="false">
                                                        <div class="row" >                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:18px">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:18px">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt1" name="txtDt1" AutoPostBack="False" CssClass="form-control" Enabled="true" TabIndex="3" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;margin-top:18px">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor: </span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor"  CssClass="form-control" style="border-left:0!important"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" />
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-top:18px;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela" runat="server"  title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela2" runat="server" Visible="false">
                                                        <div class="row">                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro2"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt2" name="txtDt2" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor:</span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor2"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela2" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela2_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela2" runat="server"  title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela2_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela3" runat="server" Visible="false">
                                                        <div class="row">                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro3"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt3" name="txtDt3" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor:</span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor3"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela3" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela3_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela3" runat="server"  title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela3_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela4" runat="server" Visible="false">
                                                        <div class="row">                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro4"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt4" name="txtDt4" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor:</span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor4"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela4" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela4_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela4" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela4_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela5" runat="server" Visible="false">
                                                        <div class="row">                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro5"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt5" name="txtDt5" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor:</span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor5"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela5" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela5_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela5" runat="server"  title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela5_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela6" runat="server" Visible="false">
                                                        <div class="row">                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro6"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt6" name="txtDt6" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor:</span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor6"   CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela6" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela6_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela6" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela6_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela7" runat="server" Visible="false">
                                                        <div class="row">                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro7"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt7" name="txtDt7"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor:</span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor7"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela7" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela7_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela7" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela7_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela8" runat="server" Visible="false">
                                                        <div class="row">                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro8"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt8" name="txtDt8"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor:</span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor8"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela8" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela8_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela8" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela8_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela9" runat="server" Visible="false">
                                                        <div class="row">                                                
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N° Doc:</span>
                                                                    <asp:TextBox ID="txtNro9"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Data Vencimento:</span>
                                                                    <asp:TextBox ID="txtDt9" name="txtDt9" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">Valor:</span>
                                                                    <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                                    <asp:TextBox ID="txtValor9"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                                </div>
                                                            </div>
                                                             <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                                <asp:LinkButton ID="btnAddParcela9" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela9_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExcluirParcela9" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela9_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                                    <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                                </asp:LinkButton>                                                   
                                                            
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlParcela10" runat="server" Visible="false">
                                                <div class="row">                                                
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">N° Doc:</span>
                                                            <asp:TextBox ID="txtNro10"  CssClass="form-control" TabIndex="2" runat="server" MaxLength="50" Enabled="false"/>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">Data Vencimento:</span>
                                                            <asp:TextBox ID="txtDt10" name="txtDt10" CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">Valor:</span>
                                                            <span class="input-group-addon" style="border-right:0!important;padding-right:0!important">R$</span>
                                                            <asp:TextBox ID="txtValor10"  CssClass="form-control"  TabIndex="2" runat="server" MaxLength="50" Enabled="false" style="border-left:0!important"/>
                                                        </div>
                                                    </div>
                                                     <div class="col-md-2" style="background-color: white; border: none; text-align: left; font-size: x-small;margin-bottom:11px">
                                                        <asp:LinkButton ID="btnAddParcela10" runat="server" title="Adicionar/Alterar Parcela" TabIndex="20" CssClass="btn btn-success"  OnClick="btnAddParcela10_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-plus"></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnExcluirParcela10" runat="server" title="Excluir Parcela" TabIndex="19" CssClass="btn btn-danger"  OnClick="btnExcluirParcela10_Click" style="height:33px!important;padding-top:7px" Visible="false"> 
                                                            <span aria-hidden="true"  class="glyphicon glyphicon-remove"></span>
                                                        </asp:LinkButton>                                                   
                                                            
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                                </div>
                                               <asp:Panel ID="pnlParcelas" runat="server" Visible="false">
                                                   <div class="col-md-12 col-lg-4" style="margin-top:37px">
                                                       <div class="panel panel-primary">
                                                            <div class="panel-heading" style="text-align:center;">Parcelas</div>
                                                            <div class="panel-body" style="padding-left:0px!important;padding-right:0px!important;padding-bottom:0px!important">
                                                                 <asp:GridView ID="grdPagamento" runat="server"
                                                                            CssClass="table  table-striped"
                                                                            GridLines="None" AutoGenerateColumns="False"
                                                                            Font-Size="8pt"  Visible="true"
                                                                            OnSelectedIndexChanged="grdPagamento_SelectedIndexChanged">
                                                                     <PagerStyle HorizontalAlign = "Center" CssClass = "GridPager" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CodigoParcela" HeaderText="#"   HeaderStyle-Width ="15%" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="centerHeaderText"/>
                                                                        <asp:BoundField DataField="DGNumeroDocumento" HeaderText="Nro Doc."   HeaderStyle-Width ="25%" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="centerHeaderText"/>
                                                                        <asp:BoundField DataField="DataVencimento" HeaderText="Data Venc."  DataFormatString="{0: dd/MM/yyyy}" HeaderStyle-Width ="30%"  HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="centerHeaderText"/>
                                                                        <asp:BoundField DataField="ValorParcela" HeaderText="Valor Parcela" DataFormatString="{0:C}"   HeaderStyle-Width ="30%" HeaderStyle-CssClass="centerHeaderText" ItemStyle-CssClass="centerHeaderText"/>

                                                                    </Columns>
                                                                    <RowStyle CssClass="cursor-pointer" />
                                                                 </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>

                                               </asp:Panel>
                                               
                                            </div>
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlPagamento" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="grdPagamento" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Exclui Grupo -->
    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="modal fade" id="myexcpes" tabindex="-1" role="dialog" aria-labelledby="myLargeModal">
                <div class="modal-dialog" role="document" style="height: 100%; width: 300px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="H3">Atenção</h4>
                        </div>
                        <div class="modal-body">
                            Deseja Excluir esta Nota Fiscal ?
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCfmSim" runat="server" Text="Excluir" CssClass="btn btn-danger" TabIndex="-1" AutoPostBack="true" OnClick="btnCfmSim_Click"></asp:LinkButton>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
          </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCfmSim"  />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
